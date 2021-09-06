using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmTellerTransaction : Form
    {
        public frmTellerTransaction()
        {
            InitializeComponent();
        }

        string m_sReportName = String.Empty;

        private int m_iReportSwitch = 0;

        public int ReportSwitch
        {
            get { return m_iReportSwitch; }
            set { m_iReportSwitch = value; }
        }

        private void LoadTeller()
        {
            String sQuery = string.Format("select teller, ln from tellers order by teller");
            cmbTeller.Items.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                cmbTeller.Items.Add("ALL");
                while (pSet.Read())
                {
                    cmbTeller.Items.Add(pSet.GetString("teller"));
                }
            }
            cmbTeller.SelectedIndex = 0;
            pSet.Close();
        }

        private void frmTellerTransaction_Load(object sender, EventArgs e)
        {
            dtpFrom.Text = AppSettingsManager.GetSystemDate().ToShortDateString();
            dtpTo.Text = AppSettingsManager.GetSystemDate().ToShortDateString();

            LoadTeller();

            // RMC 20150504 QA corrections (s)
            if (m_iReportSwitch == 6)
            {
                m_sReportName = "Liquidating Officer RCD";
                lblPeriod.Location = new System.Drawing.Point(112, 68);
                dtpFrom.Location = new System.Drawing.Point(112, 68);
                lblPeriod.Text = "RCD DATE";
                lblTellerName.Visible = false;
                cmbTeller.Visible = false;
                lblFr.Visible = true;
                lblTo.Visible = false;
                dtpTo.Visible = false;
                this.Text = m_sReportName;
                lblFr.Location = new System.Drawing.Point(43, 70);
                lblFr.Text = "RCD Date";

            }// RMC 20150504 QA corrections (e)
            else
            {
                m_sReportName = "LIST OF CANCELLED CHECKS";
                this.Text = m_sReportName;

                lblPeriod.Location = new System.Drawing.Point(23, 75);  // RMC 20150504 QA corrections


                if (m_iReportSwitch == 4)
                {
                    m_sReportName = "REPORT OF CANCELLED OFFICIAL RECEIPT";
                    this.Text = m_sReportName;
                }

                if (m_iReportSwitch == 5)
                {
                    m_sReportName = "REPORT OF TELLER TRANSACTIONS";
                    this.Text = m_sReportName;
                }
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
            if (m_sReportName == "REPORT OF CANCELLED OFFICIAL RECEIPT")
            {
                if (AppSettingsManager.GenerateInfo(m_sReportName))
                    OnReportOfCancelledOR();
            }
            else if (m_sReportName == "REPORT OF TELLER TRANSACTIONS")
            {
                if (AppSettingsManager.GenerateInfo(m_sReportName))
                    OnReportTellerTransaction();
            }
            // RMC 20150504 QA corrections (s)
            else if (m_sReportName == "Liquidating Officer RCD")    
            {
                OnReportLORCD();
            }// RMC 20150504 QA corrections (e)
            else //m_sReportName = "LIST OF CANCELLED CHECKS";
            {
                if (AppSettingsManager.GenerateInfo(m_sReportName))
                    OnReportsListOfCancelledChecks();
            }
        }

        private void OnReportOfCancelledOR()
        {
            String sTeller, sQuery, sDateFrom, sDateTo;
            
            sTeller = cmbTeller.Text;

            frmLiqReports dlg = new frmLiqReports();
            dlg.ReportTitle = m_sReportName;
            dlg.ReportSwitch = "CancelledOR";
            dlg.Teller = sTeller;
            dlg.DateFrom = dtpFrom.Value;
            dlg.DateTo = dtpTo.Value;

            sDateFrom = dtpFrom.Text;
            sDateTo = dtpTo.Text;

            if (sTeller == "ALL")
                sQuery = "select * from a_trail ";
            else
                sQuery = "select * from a_trail where usr_code = '" + sTeller + "' and ";

            sQuery += "trunc(tdatetime) >= to_date('" + sDateFrom + "','MM/dd/yyyy') and trunc(tdatetime) <= to_date('" + sDateTo + "','MM/dd/yyyy') ";
            sQuery += "and mod_code = 'CTCP'";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (pSet.Read())
                    dlg.ShowDialog();
                else
                {
                    if (sTeller == "ALL")
                        MessageBox.Show("'ALL' tellers have no cancelled payments from '" + dtpFrom.Text + "' to '" + dtpTo.Text + "'", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Teller '" + sTeller + "' has no cancelled payments from '" + dtpFrom.Text + "' to '" + dtpTo.Text + "'", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            pSet.Close();
        }

        private void OnReportTellerTransaction()
        {
            String sTeller, sQuery;

            sTeller = cmbTeller.Text;

            frmLiqReports dlg = new frmLiqReports();
            dlg.ReportTitle = m_sReportName;
            dlg.ReportSwitch = "TellerTransaction";
            dlg.Teller = sTeller;
            dlg.DateFrom = dtpFrom.Value;
            dlg.DateTo = dtpTo.Value;

            if (sTeller == "ALL")
                sQuery = "select * from teller_transaction where";
            else
                sQuery = "select * from teller_transaction where teller = '" + sTeller + "' and";

            sQuery += " trunc(dt_save) >= to_date('" + dtpFrom.Value.ToShortDateString() + "','MM/dd/yyyy') and trunc(dt_save) <= to_date('" + dtpTo.Value.ToShortDateString() + "','MM/dd/yyyy')";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    dlg.ShowDialog();
                }
                else
                {
                    if (sTeller == "ALL")
                        MessageBox.Show("'ALL' tellers have no transaction(s) from '" + dtpFrom.Text + "' to '" + dtpTo.Text + "'", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Teller '" + sTeller + "' has no transaction(s) from '" + dtpFrom.Text + "' to '" + dtpTo.Text + "'", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            pSet.Close();
        }

        private void OnReportsListOfCancelledChecks()
        {
            String sQuery, sFromDate, sToDate, sCurrentUser, sTellerCode = String.Empty;
            String sChkTeller, sChkNo, sChkDate, sChkBankCode, sChkAcctNo, sChkAcctLn, sChkAcctFn, sChkAcctMi, sChkOrNo, sChkOrDate, sChkDtAccept, sChkMemo, sChkBin, sChkDateCancelled, sChkAcctName;
            double dChkAmount, dCashAmount;
            int iCtr =0, iProgressCtr = 0;

            dChkAmount = 0;
            dCashAmount = 0;

            //User
            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            if ((cmbTeller.Text == "ALL") || (cmbTeller.Text == "%"))
                sTellerCode = "%%";
            else // ASt 20150427
                sTellerCode = string.Format("%{0}%", cmbTeller.Text);

            //Date
            sFromDate = dtpFrom.Text;
            sToDate = dtpTo.Text;

            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = string.Format("DELETE FROM REP_CANCEL_CHK WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.ExecuteNonQuery();
            pSet.Close();

            pSet.Query = string.Format("DELETE FROM GEN_INFO WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.ExecuteNonQuery();
            pSet.Close();

            //(s)----------> Insert Data from Temp Report Table
            String sCurrentDate;
            DateTime m_dtSaveTime;
            m_dtSaveTime = AppSettingsManager.GetSystemDate();
            sCurrentDate = string.Format("{1}/{2}/{0}",
                    m_dtSaveTime.Year,
                    m_dtSaveTime.Month,
                    m_dtSaveTime.Day); // {3}:{4}:{5} m_dtSaveTime.Hour, m_dtSaveTime.Minute, m_dtSaveTime.Second);

            pSet.Query = string.Format("INSERT INTO GEN_INFO VALUES('{0}',sysdate,'{1}',{2},'COL')", m_sReportName, AppSettingsManager.SystemUser.UserCode, 1);
            pSet.ExecuteNonQuery();
            pSet.Close();

            sQuery = string.Format(@"select cancelled_chk.*,bin,memo,date_posted from cancelled_chk, cancelled_payment
			where cancelled_chk.teller like '{0}' and cancelled_chk.or_no = cancelled_payment.or_no
			and date_posted between to_date('{1}','MM/dd/yyyy') and to_date('{2}','MM/dd/yyyy') order by dt_accepted", sTellerCode, sFromDate, sToDate);

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iProgressCtr = iProgressCtr + 1;

                    sChkTeller = StringUtilities.HandleApostrophe(pSet.GetString("teller")).Trim();
                    sChkNo = StringUtilities.HandleApostrophe(pSet.GetString("chk_no")).Trim();
                    sChkDate = pSet.GetDateTime("chk_date").ToShortDateString().Trim();
                    sChkBankCode = StringUtilities.HandleApostrophe(pSet.GetString("bank_code")).Trim();
                    sChkAcctNo = StringUtilities.HandleApostrophe(pSet.GetString("acct_no")).Trim();
                    sChkAcctLn = StringUtilities.HandleApostrophe(pSet.GetString("acct_ln")).Trim();
                    sChkAcctFn = StringUtilities.HandleApostrophe(pSet.GetString("acct_fn")).Trim();
                    sChkAcctMi = StringUtilities.HandleApostrophe(pSet.GetString("acct_mi")).Trim();
                    sChkOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no")).Trim();
                    sChkOrDate = pSet.GetDateTime("or_date").ToShortDateString().Trim();
                    sChkDtAccept = pSet.GetDateTime("dt_accepted").ToShortDateString().Trim();
                    dChkAmount = pSet.GetDouble("chk_amt");
                    dCashAmount = pSet.GetDouble("cash_amt");
                    sChkMemo = StringUtilities.HandleApostrophe(pSet.GetString("memo")).Trim();
                    sChkBin = StringUtilities.HandleApostrophe(pSet.GetString("bin")).Trim();
                    sChkDateCancelled = pSet.GetDateTime("date_posted").ToShortDateString().Trim();

                    //Account Name
                    if ((sChkAcctFn == "") || (sChkAcctFn == "."))
                        sChkAcctName = sChkAcctLn;
                    else
                    {
                        if ((sChkAcctMi == "") || (sChkAcctMi == "."))
                            sChkAcctName = sChkAcctFn + " " + sChkAcctLn;
                        else
                            sChkAcctName = sChkAcctFn + " " + sChkAcctMi + ". " + sChkAcctLn;
                    }

                    sQuery = string.Format(@"insert into rep_cancel_chk values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                                       StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                                       StringUtilities.HandleApostrophe(sChkBin),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sChkTeller, 20)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sChkNo, 20)),
                                                       StringUtilities.HandleApostrophe(sChkDate),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sChkBankCode, 30)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sChkAcctNo, 30)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sChkAcctName, 50)),
                                                       StringUtilities.HandleApostrophe(sChkOrNo),
                                                       StringUtilities.HandleApostrophe(sChkOrDate),
                                                       StringUtilities.HandleApostrophe(sChkDtAccept),
                                                       dChkAmount.ToString("#.##"),
                                                       dCashAmount.ToString("#.##"),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sChkMemo, 300)),
                                                       StringUtilities.HandleApostrophe(sChkDateCancelled),
                                                       StringUtilities.HandleApostrophe(sFromDate),
                                                       StringUtilities.HandleApostrophe(sToDate),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)));
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
            }
            pSet.Close();

            frmLiqReports dlg = new frmLiqReports();
            dlg.ReportSwitch = "ListOfCancelledChecks";//39;
            dlg.ReportTitle = m_sReportName;
            dlg.DateFrom = dtpFrom.Value;
            dlg.DateTo = dtpTo.Value;
            dlg.ShowDialog();

            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnReportLORCD()
        {
            // RMC 20150504 QA corrections 
            frmLiqReports dlg = new frmLiqReports();
            dlg.ReportSwitch = m_sReportName;
            dlg.ReportTitle = m_sReportName;
            dlg.DateFrom = dtpFrom.Value;
            dlg.ShowDialog();
        }

        private void frameWithShadow1_Load(object sender, EventArgs e)
        {

        }
    }
}