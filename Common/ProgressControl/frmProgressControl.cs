
// RMC 20111010 added progress bar in List of Lessors report

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Amellar.Common.DataConnector;
using Amellar.Common.Reports;


namespace Amellar.Common.ProgressControl
{
    public partial class frmProgressControl : Form
    {
        System.Threading.Thread thrProgress;
        System.Threading.Thread thrUpdateGui;

        ReportClass rClass = new ReportClass();
        
        public frmProgressControl()
        {
            InitializeComponent();
        }

        private void frmProgressControl_Load(object sender, EventArgs e)
        {
           // st = new Thread(new ThreadStart(rClass.PrintLessor));
            
            //ProgressThread();
          //  st.Start();
            
        }

        private void ProgressThread()
        {
            int intCountCurrent = 0;

            bool blnRunOnce = false;

            while (thrProgress.IsAlive)
            {
                blnRunOnce = true;
                System.Threading.Thread.Sleep(10);

                intCountCurrent++;
                try
                {
                    SetProgressDelegate spd = SetProgressFunc;
                    Invoke(spd, intCountCurrent);
                }
                catch
                { }
               
            }

            if (!blnRunOnce && !thrProgress.IsAlive) 
            {
                SetProgressDelegate spd = SetProgressFunc;
                Invoke(spd, pBar.Maximum);
            }

            
            
        }

        private void btnGenerate_Click_1(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            int iTotRecCnt = 0;

            if (btnGenerate.Text == "Generate")
            {
                btnGenerate.Enabled = false;

                pSet.Query = "select count(*) from own_names where own_code in";
                pSet.Query += " (select busn_own from businesses where PLACE_OCCUPANCY = 'RENTED')";
                int.TryParse(pSet.ExecuteScalar(), out iTotRecCnt);
                pBar.Minimum = 1;
                pBar.Maximum = iTotRecCnt;
                pBar.Step = 1;

                thrProgress = new System.Threading.Thread(PrintReport);
                thrUpdateGui = new System.Threading.Thread(ProgressThread);

                thrProgress.Start();
                System.Threading.Thread.Sleep(500);
                thrUpdateGui.Start();

                
            }
            
            
        }

        private void PrintReport()
        {
            rClass = new ReportClass();

            if (chkPrintLessee.Checked)
                rClass.PrintBIN = true;
            else
                rClass.PrintBIN = false;
            rClass.PrintLessor();
        }

        private delegate void SetProgressDelegate(int intNumber);
        private void SetProgressFunc(int intNumber)
        {
            
            {
                pBar.Value = intNumber;
                
                if (pBar.Value == pBar.Maximum && (!thrProgress.IsAlive))
                {
                    MessageBox.Show("Process Completed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();

                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnGenerate.Enabled)
            {
                this.Dispose();
            }
            else if (thrProgress.ThreadState == System.Threading.ThreadState.Running || thrUpdateGui.ThreadState == System.Threading.ThreadState.Running)
            {
                MessageBox.Show("Finish generation first.", "Report Generation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.Dispose();
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            

        }

             

    }
}