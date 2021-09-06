using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Modules.BusinessReports;
using Amellar.Common.ProgressControl;
using Amellar.Common.Reports;
using Amellar.Modules.DILGReport;

namespace BPLSBilling
{
    public partial class frmList : Form
    {
        public frmList()
        {
            InitializeComponent();
        }

        private void btnListofBnsEmpGen_Click(object sender, EventArgs e)
        {
            // RMC 20140105 added report with employee gender (dilg report -requested by mati)
            using (frmEmpGender form = new frmEmpGender())
            {
                form.ShowDialog();
            }
        }

        private void btnLstofLess_Click(object sender, EventArgs e)
        {
            // RMC 20111007 Added report for List of Lessors
            frmProgressControl frmProgress = new frmProgressControl();
            frmProgress.ShowDialog();
        }

        private void btnGender_Click(object sender, EventArgs e)
        {
            using (frmGender GenderFrm = new frmGender()) // CJC 20130429
            {
                GenderFrm.ShowDialog();
            }
        }

        private void btnBOB_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("This Report's content is data gathered from Additional Owner's Information in the Business Application process. \nPlease make sure that you fill up that portion accordingly. \nContinue Generating?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) //JARS 20170807
                return;

            using (frmBnsOwnBday frmBnsOwnBday = new frmBnsOwnBday())
            {
                frmBnsOwnBday.ShowDialog();
            }
        }

        private void btnDCDELog_Click(object sender, EventArgs e)
        {
            // RMC 20150226 added build-up encoder's report 
            frmEncodersLog form = new frmEncodersLog();
            form.ShowDialog();
        }

        /// <summary>
        /// AST 20150428 req. to move this here in bps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDILG_Click(object sender, EventArgs e)
        {
            frmDILG fDILGReport = new frmDILG();
            fDILGReport.ShowDialog();
        }

        private void btnEarlyBird_Click(object sender, EventArgs e)
        {
            //JHMN 20170117 print early bird taxpayer
            frmEarlyBird fEarlyBird = new frmEarlyBird();
            fEarlyBird.ShowDialog();
        }
    }
}