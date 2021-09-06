using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmAbstractACE : Form
    {
        public frmAbstractACE()
        {
            InitializeComponent();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpFrom.Value.Date > cdtToday.Date || dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpFrom.Value = AppSettingsManager.GetSystemDate();
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpTo.Value.Date > cdtToday.Date || dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpTo.Value = AppSettingsManager.GetSystemDate();
            }
        }

        private void frmAbstractACE_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            String sTitle = String.Empty;
            if (cmbFund.Text.Trim() == "General Fund")
                sTitle = "ABSTRACT OF GEN COLLECTION";
            else if (cmbFund.Text.Trim() == "Special Education Fund")
                sTitle = "ABSTRACT OF SEF COLLECTION";
            else if (cmbFund.Text.Trim() == "Trust Fund")
                sTitle = "ABSTRACT OF TRUST COLLECTION";
            else
            {
                MessageBox.Show("Select Fund!", "ACE Abstract of Collection", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            frmLiqReports frmLiqReports = new frmLiqReports();
            frmLiqReports.Fund = cmbFund.Text.Trim();
            frmLiqReports.ReportTitle = sTitle;
            frmLiqReports.ReportSwitch = "ACEAbstractCollection";
            frmLiqReports.DateFrom = dtpFrom.Value;
            frmLiqReports.DateTo = dtpTo.Value;
            frmLiqReports.ShowDialog();
        }
    }
}