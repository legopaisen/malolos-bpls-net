using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Modules.BusinessMapping;
using Amellar.Common.AppSettings;

namespace BPLSBilling
{
    public partial class frmSummary : Form
    {
        public frmSummary()
        {
            InitializeComponent();
        }

        private void btnListofBns_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmReportTest frmReport = new frmReportTest())
                {
                    frmReport.ReportName = "Summary";
                    frmReport.ReportSwitch = "";
                    frmReport.ShowDialog();
                }
            }
        }

        private void btnListofSec_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmReportTest frmReport = new frmReportTest())
                {
                    frmReport.ReportName = "Summary";
                    frmReport.ReportSwitch = "Section";
                    frmReport.ShowDialog();
                }
            }
        }
    }
}