using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BPLSBilling
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
            Application.Run(new frmMainForm());
            //Application.Run(new Amellar.Modules.Utilities.frmSignatories());
            
        }
    }
}