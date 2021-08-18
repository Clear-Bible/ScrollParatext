using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Paratext.LibronixSantaFeTranslator;
using SIL.Scripture;

namespace ScrollParatext
{
    public partial class Form1 : Form
    {
        private int _lastBBCCCVVV;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageEvents.WatchMessage(SantaFeFocusMessageHandler.FocusMsg);
            MessageEvents.MessageReceived += OnSantaFeFocusMessage;
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
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageEvents.MessageReceived -= OnSantaFeFocusMessage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var scrRef = new VerseRef(001001001);
            if (!scrRef.Valid || _lastBBCCCVVV == scrRef.BBBCCCVVV)
                return;


            //Debug.WriteLine("New position from Libronix: {0}; Book {1}, Chapter {2}, Verse {3}; {4} Link={5}",
            //    e.BcvRef, scrRef.Book, scrRef.Chapter, scrRef.Verse, scrRef.Text, e.LinkSet);

            _lastBBCCCVVV = scrRef.BBBCCCVVV;
            SantaFeFocusMessageHandler.SendFocusMessage(scrRef.Text + "LNK:1");
        }
    }
}
