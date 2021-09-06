using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Amellar.Common.AppSettings
{
    public class CapsLock
    {
        public bool bCaps = false;
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        const int KEYEVENTF_EXTENDEDKEY = 0x1;
        const int KEYEVENTF_KEYUP = 0x2;

        public void Main()
        {
            if (Control.IsKeyLocked(Keys.CapsLock))
                bCaps = true;
                //keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                //keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,
                 //   (UIntPtr)0);
            else
                bCaps = false;
        }
    }
}
