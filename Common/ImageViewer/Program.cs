using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Common.ImageViewer
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

            ImageOperations operations = new ImageOperations();
            if (operations.HasConnectionString || operations.IsInsertOnStartup)
            {
                if (!DataConnectorManager.Instance.OpenConnection())
                {
                    MessageBox.Show("Cannot connect to database.", "Image Viewer", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            if (operations.IsInsertOnStartup)
                operations.InsertQueries();
            else
                Application.Run(new frmImageViewer());

        }
    }
}