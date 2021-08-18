using System.Diagnostics;
using Paratext.Base;
using PtxUtils;
using SIL.Scripture;
using SIL.Utils;

namespace Paratext.LibronixSantaFeTranslator
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Translates sync messages from Libronix to Santa Fe format (e.g. TE and Paratext)
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class LiSaFT
	{
        #region Member variables
        //private ILogosPositionHandler m_positionHandler;
        private static LiSaFT lisa;
        private readonly ILogosPositionHandler[] handlers = new ILogosPositionHandler[6];
		private int _lastBBCCCVVV;
        private static bool santaFeLinkRunning;
        #endregion

        #region Constructors
        static LiSaFT()
	    {
	        if (PtxUtils.Platform.IsLinux)
	            return;

            lisa = new LiSaFT();
	    }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="LiSaFT"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private LiSaFT()
		{
			LogosPositionHandlerFactory.Created += OnLogosPositionHandlerCreated;

			try
			{
				InitLibronix();
                santaFeLinkRunning = true;
			}
            catch (LibronixNotRunningException)
            {
                santaFeLinkRunning = false;
            }
            catch (LibronixNotInstalledException)
            {
                santaFeLinkRunning = false;
            }

			MessageEvents.WatchMessage(SantaFeFocusMessageHandler.FocusMsg);
			MessageEvents.MessageReceived += OnSantaFeFocusMessage;
		}

	    ~LiSaFT()
	    {
	        ErrorUtils.IgnoreErrors("Exception in finalizer causes Paratext to exit", () => lisa?.Dispose());
	    }
	    #endregion

        #region Public properties
        public static bool IsEnabled
	    {
	        get { return lisa != null; }
	        set
	        {
	            if (value && lisa == null)
	            {
                    lisa = new LiSaFT();
	            }
                else if (!value && lisa != null)
	            {
                    lisa.Dispose();
	                lisa = null;
	            }
	            FocusSharer.Instance.SendToLogos = Properties.Settings.Default.ContextToLogos = value;
            }
        }
        #endregion

        #region Public methods
	    public static void Close()
	    {
	        lisa?.Dispose();
	    }
        #endregion

        #region Event handlers
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Called when the logos position handler got created.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void OnLogosPositionHandlerCreated(object sender, CreatedEventArgs e)
		{
            ILogosPositionHandler handler = e.PositionHandler;
            if (handler != null)
                handler.PositionChanged += OnPositionInLibronixChanged;
        }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when a SantaFeFocusMessage is received.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="MessageReceivedEventArgs"/> instance 
		/// containing the event data.</param>
		/// ------------------------------------------------------------------------------------
		private void OnSantaFeFocusMessage(object sender, MessageReceivedEventArgs e)
		{
			Debug.Assert(e.Message.Msg == SantaFeFocusMessageHandler.FocusMsg);
            VerseRef scrRef;
			if (!VerseRef.TryParse(SantaFeFocusMessageHandler.ReceiveFocusMessage(e.Message), out scrRef) || _lastBBCCCVVV == scrRef.BBBCCCVVV) 
				return;

            int linkChannel = e.Message.LParam.ToInt32();

			Debug.WriteLine("New position from SantaFe: {0}; Book {1}, Chapter {2}, Verse {3}; {4} Link={5}",
                scrRef.BBBCCCVVV, scrRef.Book, scrRef.Chapter, scrRef.Verse,
           		scrRef.Text, linkChannel);

			_lastBBCCCVVV = scrRef.BBBCCCVVV;
			OnPositionInSantaFeChanged(new PositionChangedEventArgs(scrRef.BBBCCCVVV, linkChannel));
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the position in Libronix changed
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="SIL.Utils.PositionChangedEventArgs"/> 
		/// instance containing the event data.</param>
		/// ------------------------------------------------------------------------------------
		private void OnPositionInLibronixChanged(object sender, PositionChangedEventArgs e)
		{
			var scrRef = new VerseRef(e.BcvRef);
			if (!scrRef.Valid || _lastBBCCCVVV == scrRef.BBBCCCVVV)
				return;

            if (!santaFeLinkRunning)
            {
                Refresh();
                if (!santaFeLinkRunning)
                    return;
            }

            Debug.WriteLine("New position from Libronix: {0}; Book {1}, Chapter {2}, Verse {3}; {4} Link={5}",
				e.BcvRef, scrRef.Book, scrRef.Chapter, scrRef.Verse, scrRef.Text, e.LinkSet);

			_lastBBCCCVVV = scrRef.BBBCCCVVV;            
			SantaFeFocusMessageHandler.SendFocusMessage(scrRef.Text + "LNK:" + e.LinkSet);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the position changed messages comes from SantaFe.
		/// </summary>
		/// <param name="e">The <see cref="SIL.Utils.PositionChangedEventArgs"/> 
		/// instance containing the event data.</param>
		/// ------------------------------------------------------------------------------------
		private void OnPositionInSantaFeChanged(PositionChangedEventArgs e)
		{
            int linkChannel = e.LinkSet;
            if (linkChannel < 0 || linkChannel >= handlers.Length)
                return;

            if (!santaFeLinkRunning)
            {
                Refresh();
                if (!santaFeLinkRunning)
                    return;
            }

            if (handlers[linkChannel] != null)
            {
                try
                {
                    handlers[linkChannel].SetReference(e.BcvRef);
                }
                catch (LibronixNotRunningException)
                {
                    santaFeLinkRunning = false;
                }
                catch (LibronixNotInstalledException)
                {
                    // FB 44407 seems that we can get this now and then even when Logos was
                    // valid earlier. Maybe a Logos update in progress.
                    santaFeLinkRunning = false;
                }
            }
		}
        #endregion

        #region Private helper methods
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes Libronix.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void InitLibronix()
	    {
	        for (int index = 0; index < handlers.Length; index++)
	        {
	            if (handlers[index] != null)
	                continue;
	            handlers[index] = LogosPositionHandlerFactory.CreateInstance(Properties.Settings.Default.StartLibronix, index, true);

	            if (handlers[index] != null)
	                OnLogosPositionHandlerCreated(null, new CreatedEventArgs { PositionHandler = handlers[index] });
	        }
	    }

		/// ------------------------------------------------------------------------------------
        /// <summary>
        /// Renew the subscriptions to all open windows in Libronix
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void Refresh()
		{
            LogosPositionHandlerFactory.Created -= OnLogosPositionHandlerCreated;
            MessageEvents.MessageReceived -= OnSantaFeFocusMessage;

            foreach (ILogosPositionHandler handler in handlers)
            {
                if (handler != null)
                    handler.Dispose();
            }

            try
            {
                LogosPositionHandlerFactory.Created += OnLogosPositionHandlerCreated;
                InitLibronix();
                santaFeLinkRunning = true;
            }
            catch (LibronixNotRunningException)
            {
                santaFeLinkRunning = false;
            }
            catch (LibronixNotInstalledException)
            {
                santaFeLinkRunning = false;
            }

            MessageEvents.WatchMessage(SantaFeFocusMessageHandler.FocusMsg);
            MessageEvents.MessageReceived += OnSantaFeFocusMessage;
        }

	    private void Dispose()
	    {
	        santaFeLinkRunning = false;
	        foreach (ILogosPositionHandler handler in handlers)
	        {
	            handler.Dispose();
	        }
	        LogosPositionHandlerFactory.Created -= OnLogosPositionHandlerCreated;
	        MessageEvents.MessageReceived -= OnSantaFeFocusMessage;
	    }
	    #endregion
    }
}