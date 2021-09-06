using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.ContainerWithShadow;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.Reports
{
    public partial class frmDebitCredit : Form
    {
        public frmDebitCredit()
        {
            InitializeComponent();
        }

        int m_iFormat = 0;

        private void frmDebitCredit_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            dtpTo.Value = AppSettingsManager.GetSystemDate();
            PrepareUsers();
            rdoBoth.PerformClick();
        }

        private void PrepareUsers()
        {
            String sQuery = "";
            OracleResultSet pSet = new OracleResultSet();
            cmbUsers.Items.Clear();

            cmbUsers.Items.Add("ALL");
            sQuery = "select * from tellers order by teller asc";
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbUsers.Items.Add(StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("teller")).Trim());
                }
            }
            pSet.Close();

            cmbUsers.SelectedIndex = 0;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            String sFromDate, sToDate;
            sFromDate = string.Format("{0}/{1}/{2}", dtpFrom.Value.Month, dtpFrom.Value.Day, dtpFrom.Value.Year);
            sToDate = string.Format("{0}/{1}/{2}", dtpTo.Value.Month, dtpTo.Value.Day, dtpTo.Value.Year);

            //PopulateReport(mv_cmbUsers,sFromDate,sToDate,m_iFormat);
            frmCompromiseAgreement dlgReport = new frmCompromiseAgreement();
            //dlgReport.DebitCreditReport(mv_cmbUsers,sFromDate,sToDate,m_iFormat);
            dlgReport.FromDate = sFromDate;
            dlgReport.ToDate = sToDate;
            dlgReport.User = cmbUsers.Text;
            dlgReport.DebitFormat = m_iFormat;
            dlgReport.ReportTitle = "DEBIT CREDIT REPORTS";
            dlgReport.ShowDialog();
        }

        private void rdoBoth_Click(object sender, EventArgs e)
        {
            m_iFormat = 1;
        }

        private void rdoAuto_Click(object sender, EventArgs e)
        {
            m_iFormat = 2;
        }

        private void rdoManual_Click(object sender, EventArgs e)
        {
            m_iFormat = 3;
        }
    }
}