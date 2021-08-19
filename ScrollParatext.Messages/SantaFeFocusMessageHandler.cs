// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2007, SIL International. All Rights Reserved.
// <copyright from='2007' to='2007' company='SIL International'>
//		Copyright (c) 2007, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: SantaFeFocusMessageHandler.cs
// Responsibility: TE Team
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;


namespace ScrollParatext.Messages
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public static class SantaFeFocusMessageHandler
    {
        #region Windows API methods
        /// <summary>The RegisterWindowMessage function defines a new window message that is
        /// guaranteed to be unique throughout the system. The message value can be used when
        /// sending or posting messages.</summary>
        /// <param name="name">unique name of a message</param>
        /// <returns>message identifier in the range 0xC000 through 0xFFFF, or 0 if an error
        /// occurs</returns>
        [DllImport("User32.dll")]
        private static extern uint RegisterWindowMessage(string name);

        /// <summary></summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostMessage(IntPtr hWnd, int msg, uint wParam, uint lParam);
        #endregion

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Types of "focus sharing" supported by TE (must match SanatFe spec)
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private enum FocusTypes
        {
            /// <summary></summary>
            ScriptureReferenceFocus = 1,
        }

        /// <summary>
        /// The registry key for synchronizing apps to a Scripture reference (must match SanatFe spec)
        /// </summary>
        private static readonly RegistryKey s_SantaFeRefKey =
            Registry.CurrentUser.CreateSubKey(@"Software\SantaFe\Focus\ScriptureReference");

        /// <summary>
        /// The registry key for synchronizing apps to a scroll group
        /// </summary>
        private static readonly RegistryKey s_scrollGroupKey =
            Registry.CurrentUser.CreateSubKey(@"Software\SantaFe\Focus\ScrollGroup");

        /// <summary>
        /// The Windows message used for synchronized scrolling (must match SanatFe spec)
        /// </summary>
        private static readonly int s_FocusMsg = (int)RegisterWindowMessage("SantaFeFocus");

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Notify all Santa Fe windows that a Scripture Reference focus change has occured.
        /// </summary>
        /// <param name="sRef">The string representation of the reference</param>
        /// ------------------------------------------------------------------------------------
        public static void SendFocusMessage(string sRef)
        {
            string linkIndex = sRef.Substring(sRef.IndexOf("LNK:", StringComparison.Ordinal) + 4);
            sRef = sRef.Substring(0, sRef.IndexOf("LNK:", StringComparison.Ordinal));
            s_SantaFeRefKey.SetValue(null, sRef);
            s_scrollGroupKey.SetValue(null, linkIndex);
            PostMessage(new IntPtr(-1), s_FocusMsg, (uint)FocusTypes.ScriptureReferenceFocus, 0);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Receives the focus message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>The string representation of the reference (e.g. MAT 1:1)</returns>
        /// ------------------------------------------------------------------------------------
        public static string ReceiveFocusMessage(Message msg)
        {
            int focusType = msg.WParam.ToInt32();
            if (focusType != (int)FocusTypes.ScriptureReferenceFocus)
                return string.Empty;

            string reference = s_SantaFeRefKey.GetValue(null).ToString();
            string scrollGrp = s_scrollGroupKey.GetValue(null).ToString();
            Debug.Print("Reference: [" + reference + "]");
            Debug.Print("ScrollGrp: [" + scrollGrp + "]");

            return reference;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the focus message.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static int FocusMsg
        {
            get { return s_FocusMsg; }
        }
    }
}
