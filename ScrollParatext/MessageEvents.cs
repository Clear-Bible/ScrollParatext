// Stephen Toub
// Slightly modified; based on http://msdn.microsoft.com/en-us/magazine/cc163417.aspx

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Paratext.LibronixSantaFeTranslator
{
    public class MessageReceivedEventArgs : EventArgs
    {
        private readonly Message _message;
        public MessageReceivedEventArgs(Message message)
        {
            _message = message;
        }

        public Message Message
        {
            get { return _message; }
        }
    }

    public static class MessageEvents
    {
        private static readonly object _lock = new object();
        private static MessageWindow _window;
        private static IntPtr _windowHandle;
        private static SynchronizationContext Context { get; set; }

        public static event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public static void WatchMessage(int message)
        {
            EnsureInitialized();
            _window.RegisterEventForMessage(message);
        }

        private static void EnsureInitialized()
        {
            lock (_lock)
            {
                if (_window != null)
                    return;

                Context = AsyncOperationManager.SynchronizationContext;
                _window = new MessageWindow();
                // getting the window handle is required for the WndProc method to receive events - probably
                // because this form is never displayed
                _windowHandle = _window.Handle;
            }
        }

        private class MessageWindow : Form
        {
            private readonly ReaderWriterLock m_lock = new ReaderWriterLock();
            private readonly Dictionary<int, bool> m_messageSet = new Dictionary<int, bool>();

            public void RegisterEventForMessage(int messageID)
            {
                m_lock.AcquireWriterLock(Timeout.Infinite);
                m_messageSet[messageID] = true;
                m_lock.ReleaseWriterLock();
            }

            protected override void WndProc(ref Message m)
            {
                m_lock.AcquireReaderLock(Timeout.Infinite);
                bool handleMessage = m_messageSet.ContainsKey(m.Msg);
                m_lock.ReleaseReaderLock();

                if (handleMessage)
                {
                    Context.Post(delegate (object state)
                    {
                        EventHandler<MessageReceivedEventArgs> handler = MessageReceived;
                        if (handler != null)
                            handler(null, new MessageReceivedEventArgs((Message)state));
                    }, m);
                }

                base.WndProc(ref m);
            }
        }
    }
}
