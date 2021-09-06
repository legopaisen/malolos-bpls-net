using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Modules.DILGReport;
using Amellar.Modules.Payment;
using Amellar.Common.Reports;

namespace BTAS
{
    public partial class frmOtherReports : Form
    {
        public frmOtherReports()
        {
            InitializeComponent();
        }

        private void btnDILG_Click(object sender, EventArgs e)
        {
            frmDILG fDILGReport = new frmDILG();
            fDILGReport.ShowDialog();
        }

        private void btnPUP_Click(object sender, EventArgs e)
        {
            frmPaymentMadeUnderProtest frmPaymentMadeUnderProtest = new frmPaymentMadeUnderProtest();
            frmPaymentMadeUnderProtest.ShowDialog();
        }

        private void btnDCDELog_Click(object sender, EventArgs e)
        {
            frmEncodersLog frmEncodersLog = new frmEncodersLog();
            frmEncodersLog.ShowDialog();
        }

        private void btnDefRec_Click(object sender, EventArgs e)
        {
            frmDeficiencyReport frmDeficiencyReport = new frmDeficiencyReport();
            frmDeficiencyReport.ShowDialog();
        }

        private void btnCompAgree_Click(object sender, EventArgs e)
        {
            frmCompromiseAgreement frmCompromiseAgreement = new frmCompromiseAgreement();
            frmCompromiseAgreement.ReportTitle = "LIST OF COMPROMISE AGREEMENT";
            frmCompromiseAgreement.ShowDialog();
        }

        private void btnFP_Click(object sender, EventArgs e)
        {
            frmFPCert frmFPCert = new frmFPCert();
            frmFPCert.ShowDialog();
        }

    }
}