using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmAbstractChecks : Form
    {
        public frmAbstractChecks()
        {
            InitializeComponent();
        }

        private void LoadTeller()
        {
            String sQuery = string.Format("select teller, ln from tellers order by teller");
            cmbTeller.Items.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbTeller.Items.Add(pSet.GetString("teller"));
                }
            }
            cmbTeller.SelectedIndex = 0;
            pSet.Close();
        }

        private string m_sReportTitle = "";

        private bool m_bSw = false;

        private int m_iReportSwitch = 0;

        public int ReportSwitch
        {
            get { return m_iReportSwitch; }
            set { m_iReportSwitch = value; }
        }

        private void frmAbstractChecks_Load(object sender, EventArgs e)
        {
            dtpFrom.Text = AppSettingsManager.GetSystemDate().ToShortDateString();
            dtpTo.Text = AppSettingsManager.GetSystemDate().ToShortDateString();
            dtpDate.Text = AppSettingsManager.GetSystemDate().ToShortDateString();

            LoadTeller();

            rdoAcctForm.Visible = true;
            rdoCollect.Visible = true;
            //(e) RMC 12162004 ABSTRACT OF CHECKS
            rdoDummy.Focus();
            m_bSw = true;

            if (m_iReportSwitch == 2)		// RTL 11162004 for RCDManila
            {
                rdoCollect.Focus();
            }
            else if (m_iReportSwitch == 3)
            {
                m_sReportTitle = "Report of Abstract of Checks";
                //mc_vstFrame.SetCaption("Abstract of Checks");
                this.Text = m_sReportTitle;
                rdoAcctForm.Visible = false;
                rdoCollect.Visible = false;
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                dtpDate.Enabled = true;
                txtTeller.Visible = false;
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpDate.Value.Date > cdtToday.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpDate.Value = AppSettingsManager.GetSystemDate();
            }
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            String sQuery, sQuery2, sTeller;

            //if (cmbTeller.Text == "ALL")
            //    sQuery = string.Format("select * from tellers order by teller");
            //else
            sQuery = string.Format("select * from tellers where teller = '{0}'", cmbTeller.Text.Trim());
            
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (!pSet.Read())
                {
                    MessageBox.Show("Teller '" + cmbTeller.Text.Trim() + "' not found", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //for Manila
                    //OnReportCollRemitt(mv_iRadioOption);// RTL put rem for RCDManila
                    frmLiqReports dlg = new frmLiqReports();
                    dlg.ReportTitle = m_sReportTitle;
                    dlg.ReportSwitch = m_sReportTitle.Trim().Replace(' ', '_');
                    dlg.Date = dtpDate.Value;
                    //dlg.m_sTeller	   = txtTeller.text;
                    dlg.Teller = cmbTeller.Text;
                    dlg.DateFrom = dtpFrom.Value;
                    dlg.DateTo = dtpTo.Value;

                    if (m_iReportSwitch == 3)
                    {
                        //if (cmbTeller.Text == "ALL")
                        //{
                        //    sQuery2 = "select * from chk_tbl where";
                        //    sQuery2 += " or_date >= to_date('" + dtpFrom.Value.ToShortDateString() + "','MM/dd/yyyy') and or_date <= to_date('" + dtpTo.Value.ToShortDateString() + "','MM/dd/yyyy') order by teller, or_no";
                        //}
                        //else
                        //{
                        sQuery2 = "select * from chk_tbl where teller = '" + cmbTeller.Text.Trim() + "' and";
                        sQuery2 += " or_date >= to_date('" + dtpFrom.Value.ToShortDateString() + "','MM/dd/yyyy') and or_date <= to_date('" + dtpTo.Value.ToShortDateString() + "','MM/dd/yyyy') order by or_no";

                        pSet1.Query = sQuery2;
                        if (pSet1.Execute())
                            if (!pSet1.Read())
                            {
                                //if (cmbTeller.Text == "ALL")
                                //    MessageBox.Show("'ALL' tellers have no check collections from '" + dtpFrom.Value.ToShortDateString() + "' to '" + dtpTo.Value.ToShortDateString() + "'", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //else
                                MessageBox.Show("Teller '" + cmbTeller.Text.Trim() + "' has no check collections from '" + dtpFrom.Value.ToShortDateString() + "' to '" + dtpTo.Value.ToShortDateString() + "'", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                                dlg.ShowDialog();
                    }
                    else
                        dlg.ShowDialog();
                }
        }

        private void rdoCollect_Click(object sender, EventArgs e)
        {
            dtpFrom.Enabled = false;
            dtpTo.Enabled = false;
            dtpDate.Enabled = true;
            txtTeller.Visible = true;
            m_sReportTitle = "Report of Collections and Deposits";	// for RCDManila
            txtTeller.Focus();
        }

        private void rdoAcctForm_Click(object sender, EventArgs e)
        {
            dtpFrom.Enabled = false;
            dtpTo.Enabled = false;
            dtpDate.Enabled = true;
            txtTeller.Visible = true;
            txtTeller.Focus();
        }

        private void OnReportColl()
        {
            String sQuery, sTmp, sPrompt, sFormula = "";
            
            sQuery = @"SELECT data_group, subgroup1, or_no, payor, particulars, chk_no, chk_date, chk_amt, cash, total, report_name, current_user 
            FROM REP_COLL_REMITT rep_coll_remitt WHERE report_name = '" + m_sReportTitle + "' AND current_user = '" + AppSettingsManager.SystemUser.UserCode + "'";

            //pApp->OnCrystalEngine(sQuery, "CollectionsRemittances-Collection-LUCENA.rpt");
        }

        private void OnReportCollRemitt()
        { 

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}