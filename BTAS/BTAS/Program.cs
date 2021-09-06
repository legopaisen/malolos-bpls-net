using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BTAS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmBTAS());
            //Application.Run(new frmMainFormACE());
        }
    }
}