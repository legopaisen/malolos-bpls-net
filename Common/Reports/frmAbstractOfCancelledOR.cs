using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.ContainerWithShadow;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.Reports
{
    public partial class frmAbstractOfCancelledOR : Form
    {
        public frmAbstractOfCancelledOR()
        {
            InitializeComponent();
        }

        private int m_iSwAbstractReports = 0;

        int m_iRadioNumber = 0;

        public int AbstractReportFormat
        {
            set { m_iSwAbstractReports = value; }
        }

        private void frmAbstractOfCancelledOR_Load(object sender, EventArgs e)
        {
            if (m_iSwAbstractReports == 2 || m_iSwAbstractReports == 4)
                this.Text = "Reports of Credit Memo";
            else if (m_iSwAbstractReports == 3)
                this.Text = "Reports of Debit Memo";

            LoadTeller();

            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            dtpTo.Value = AppSettingsManager.GetSystemDate();
            rdoTeller.PerformClick();

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

        private void LoadTeller()
        {
            cmbTeller.Items.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select teller from tellers order by teller";
            if (pSet.Execute())
            {
                cmbTeller.Items.Add("");
                while (pSet.Read())
                {
                    cmbTeller.Items.Add(pSet.GetString(0));
                }
            }
            if (cmbTeller.Items.Count > 0)
                cmbTeller.SelectedIndex = 0;
            pSet.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoTeller_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 0;

            cmbTeller.Enabled = true;
            dtpTo.Enabled = false;
        }

        private void rdoOR_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 1;

            cmbTeller.Enabled = false;
            dtpTo.Enabled = true;
        }

        private void rdoDaily_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 2;

            cmbTeller.Enabled = false;
            dtpTo.Enabled = true;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            frmCompromiseAgreement dlg = new frmCompromiseAgreement();
            if (m_iSwAbstractReports == 1)		// JJP 01142005 ABTRACT REPORTS
            {
                if (m_iRadioNumber == 0)
                    dlg.ReportTitle = "ABSTRACT OF CANCELLED RECEIPTS BY TELLER";
                else if (m_iRadioNumber == 1)
                    dlg.ReportTitle = "ABSTRACT OF CANCELLED RECEIPTS BY OFFICIAL RECEIPT";
                else if (m_iRadioNumber == 2)
                    dlg.ReportTitle = "ABSTRACT OF CANCELLED RECEIPTS BY DAILY AGGREGATE";
            }
            //(s) JJP 01142005 ABTRACT REPORTS
            // JGR 07262005 (s) SEPARATE REPORT OF EXCESS OF TAX CREDIT AND EXCESS
            else if (m_iSwAbstractReports == 2)
            {
                if (m_iRadioNumber == 0)
                    dlg.ReportTitle = "ABSTRACT OF EXCESS OF CHECKS OF BUSINESS TAX AND REGULATORY FEES BY TELLER";
                else if (m_iRadioNumber == 1)
                    dlg.ReportTitle = "ABSTRACT OF EXCESS OF CHECKS OF BUSINESS TAX AND REGULATORY FEES BY OFFICIAL RECEIPT";
                else if (m_iRadioNumber == 2)
                    dlg.ReportTitle = "ABSTRACT OF EXCESS OF CHECKS OF BUSINESS TAX AND REGULATORY FEES BY DAILY AGGREGATE";
            }
            else if (m_iSwAbstractReports == 3)
            {
                if (m_iRadioNumber == 0)
                    dlg.ReportTitle = "ABSTRACT OF DEBITED CREDIT MEMOS OF BUSINESS TAX AND REGULATORY FEES BY TELLER";
                else if (m_iRadioNumber == 1)
                    dlg.ReportTitle = "ABSTRACT OF DEBITED CREDIT MEMOS OF BUSINESS TAX AND REGULATORY FEES BY OFFICIAL RECEIPT";
                else if (m_iRadioNumber == 2)
                    dlg.ReportTitle = "ABSTRACT OF DEBITED CREDIT MEMOS OF BUSINESS TAX AND REGULATORY FEES BY DAILY AGGREGATE";
            }
            // JGR 07262005 (s) SEPARATE REPORT FOR EXCESS OF TAX CREDIT
            else if (m_iSwAbstractReports == 4)
            {
                if (m_iRadioNumber == 0)
                    dlg.ReportTitle = "ABSTRACT OF EXCESS OF TAX CREDIT OF BUSINESS TAX AND REGULATORY FEES BY TELLER";
                else if (m_iRadioNumber == 1)
                    dlg.ReportTitle = "ABSTRACT OF EXCESS OF TAX CREDIT OF BUSINESS TAX AND REGULATORY FEES BY OFFICIAL RECEIPT";
                else if (m_iRadioNumber == 2)
                    dlg.ReportTitle = "ABSTRACT OF EXCESS OF TAX CREDIT OF BUSINESS TAX AND REGULATORY FEES BY DAILY AGGREGATE";
            }
            // JGR 07262005 (e) SEPARATE REPORT FOR EXCESS OF TAX CREDIT
            //(e) JJP 01142005 ABTRACT REPORTS

            String sFromDate, sToDate;
            sFromDate = string.Format("{0}/{1}/{2}", dtpFrom.Value.Month, dtpFrom.Value.Day, dtpFrom.Value.Year);
            sToDate = string.Format("{0}/{1}/{2}", dtpTo.Value.Month, dtpTo.Value.Day, dtpTo.Value.Year);
            dlg.AbstractFormat = m_iSwAbstractReports;
            dlg.FromDate = sFromDate;
            dlg.ToDate = sToDate;
            dlg.User = cmbTeller.Text;
            dlg.GroupFormat = m_iRadioNumber;
            dlg.ShowDialog();

        }
    }
}