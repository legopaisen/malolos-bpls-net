using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.DynamicProgressBar;
using System.Threading;

namespace Amellar.Modules.Delinquency
{
    public partial class frmListDelinquency : Form
    {
        public frmListDelinquency()
        {
            InitializeComponent();
        }

        ArrayList arr_BnsCode = new ArrayList();
        ArrayList arr_BnsCodeSub = new ArrayList();
        string m_sBnsCode = String.Empty, m_sBnsCodeSub = String.Empty, m_sQuery = String.Empty;
        string m_sReportName = String.Empty, m_sDate = String.Empty, m_sYear = String.Empty, m_sDelqQtr = String.Empty;
        int m_iOptionFormat = 0;

        //MCR 20140714 (s)
        public static frmProgress m_form;
        private Thread m_objThread;
        public delegate void DifferentDelegate(object value, frmProgress.ProgressMode mode);
        public static void DoSomethingDifferent(object value, frmProgress.ProgressMode mode, DifferentDelegate threadFunction)
        {
            threadFunction(value, mode); // NOTE: invoked with a parameter
        }
        private void ThreadProcess()
        {
            using (m_form = new frmProgress())
            {

                m_form.TopMost = true;

                m_form.ShowDialog();
            }
        }
        public static int intCount = 0;
        public static int intCountIncreament = 0;
        public static int intCountSaved = 0;
        //MCR 20140714 (e)

        private void frmListDelinquency_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            LoadBrgy();
            LoadDist();
            LoadBusType();
            LoadLineofBusiness();
            rdoBrgy.PerformClick();
        }

        private void LoadBrgy()
        {
            cmbBrgy.Items.Clear();
            cmbBrgy.Items.Add("ALL");

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select brgy_nm from brgy order by brgy_nm";
            if (pSet.Execute())
                while (pSet.Read())
                {
                    cmbBrgy.Items.Add(pSet.GetString(0));
                }
            pSet.Close();

            cmbBrgy.SelectedIndex = 0;
        }

        private void LoadDist()
        {
            cmbDist.Items.Clear();
            cmbDist.Items.Add("ALL");

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select distinct dist_nm from brgy order by dist_nm";
            if (pSet.Execute())
                while (pSet.Read())
                {
                    cmbDist.Items.Add(pSet.GetString(0));
                }
            pSet.Close();

            cmbDist.SelectedIndex = 0;
        }

        private void LoadBusType()
        {
            cmbBusType.Items.Clear();
            cmbBusType.Items.Add("ALL");
            arr_BnsCode.Clear();
            arr_BnsCode.Add("");
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select bns_desc,bns_code from bns_table where fees_code = 'B' and length(rtrim(bns_code)) = 2 and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' order by bns_code";
            if (pSet.Execute())
                while (pSet.Read())
                {
                    cmbBusType.Items.Add(pSet.GetString(0).Trim());
                    arr_BnsCode.Add(pSet.GetString(1).Trim());
                }
            pSet.Close();

            cmbBusType.SelectedIndex = 0;
        }

        private void LoadLineofBusiness()
        {
            cmbLineBus.Items.Clear();
            cmbLineBus.Items.Add("ALL");
            arr_BnsCodeSub.Clear();
            arr_BnsCodeSub.Add("");
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = string.Format("select bns_desc,bns_code from bns_table where fees_code = 'B' and length(rtrim(bns_code)) > 3 and rev_year = '{0}' and rtrim(bns_code) like '{1}' order by bns_code", AppSettingsManager.GetConfigValue("07"), m_sBnsCode + "%%");
            if (pSet.Execute())
                while (pSet.Read())
                {
                    cmbLineBus.Items.Add(pSet.GetString(0).Trim());
                    arr_BnsCodeSub.Add(pSet.GetString(1).Trim());
                }
            pSet.Close();

            cmbLineBus.SelectedIndex = 0;
        }

        private void txtTop_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbLineBus_DropDown(object sender, EventArgs e)
        {
            LoadLineofBusiness();
        }

        private void cmbBusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_sBnsCode = arr_BnsCode[cmbBusType.SelectedIndex].ToString();
            }
            catch { return; }
        }

        private void cmbLineBus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_sBnsCodeSub = arr_BnsCodeSub[cmbLineBus.SelectedIndex].ToString();
            }
            catch { return; }
        }

        private void BtnControl(bool blnEnable)
        {
            cmbBrgy.Enabled = blnEnable;
            cmbDist.Enabled = blnEnable;
            cmbLineBus.Enabled = blnEnable;
            cmbBusType.Enabled = blnEnable;
            cmbOrgnKind.Enabled = blnEnable;
            chkTopDelinq.Visible = blnEnable;
            txtTop.Enabled = blnEnable;
            txtGrossFrom.Enabled = blnEnable;
            txtGrossTo.Enabled = blnEnable;
            chkTopDelinq.Checked = blnEnable;

        }

        private void rdoBrgy_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 0;
            BtnControl(false);
            cmbBrgy.Enabled = true;
            cmbBusType.Enabled = true;
            cmbLineBus.Enabled = true;
            cmbOrgnKind.Enabled = true;
            cmbBusStat.Enabled = true;

            cmbBrgy.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbLineBus.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbBusStat.SelectedIndex = 0;
        }

        private void rdoDist_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 1;
            BtnControl(false);
            cmbDist.Enabled = true;
            cmbBusType.Enabled = true;
            cmbLineBus.Enabled = true;
            cmbOrgnKind.Enabled = true;
            cmbBusStat.Enabled = true;

            cmbBrgy.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbLineBus.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbBusStat.SelectedIndex = 0;
        }

        private void rdoMainBus_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 2;
            BtnControl(false);
            cmbBrgy.Enabled = true;
            cmbBusType.Enabled = true;
            cmbLineBus.Enabled = true;
            cmbOrgnKind.Enabled = true;
            cmbBusStat.Enabled = true;

            cmbBrgy.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbLineBus.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbBusStat.SelectedIndex = 0;
        }

        private void rdoBusStat_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 3;
            BtnControl(false);
            cmbBrgy.Enabled = true;
            cmbBusType.Enabled = true;
            cmbLineBus.Enabled = true;
            cmbOrgnKind.Enabled = true;
            cmbBusStat.Enabled = true;

            cmbBrgy.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbLineBus.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbBusStat.SelectedIndex = 0;
        }

        private void rdoOwnKind_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 4;
            BtnControl(false);
            cmbBrgy.Enabled = true;
            cmbBusType.Enabled = true;
            cmbLineBus.Enabled = true;
            cmbOrgnKind.Enabled = true;
            cmbBusStat.Enabled = true;

            cmbBrgy.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbLineBus.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbBusStat.SelectedIndex = 0;
        }

        private void rdoGross_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 5;
            BtnControl(false);
            cmbDist.Enabled = true;
            cmbBusType.Enabled = true;
            cmbLineBus.Enabled = true;
            txtGrossFrom.Enabled = true;
            txtGrossTo.Enabled = true;

            cmbBrgy.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbLineBus.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbBusStat.SelectedIndex = 0;
        }

        private void rdoTopDelinq_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 6;
            BtnControl(false);
            cmbDist.Enabled = true;
            cmbBusType.Enabled = true;
            cmbLineBus.Enabled = true;
            cmbBusStat.Enabled = true;
            chkTopDelinq.Visible = true;
            txtTop.Enabled = true;

            cmbBrgy.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbLineBus.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbBusStat.SelectedIndex = 0;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            String sJanuary, sApril, sJuly, sOctober, sDay;
            DateTime odtJanuary, odtApril, odtJuly, odtOctober, odtDate; // ALJ 10052003 change to date format for comparison

            DateTime m_vodtDate;
            m_vodtDate = AppSettingsManager.GetSystemDate();

            m_sYear = m_vodtDate.Year.ToString();

            sDay = AppSettingsManager.GetConfigValue("14");
            sJanuary = string.Format("01/{1}/{0}", m_sYear, sDay);
            sApril = string.Format("04/{1}/{0}", m_sYear, sDay);
            sJuly = string.Format("07/{1}/{0}", m_sYear, sDay);
            sOctober = string.Format("10/{1}/{0}", m_sYear, sDay);

            m_sDate = string.Format("{0}/{1}/{2}", m_vodtDate.Month.ToString(), m_vodtDate.Day.ToString(), m_vodtDate.Year.ToString());

            // ALJ 10052003 (s) change to date format for comparison
            odtDate = Convert.ToDateTime(m_sDate);
            odtJanuary = Convert.ToDateTime(sJanuary);
            odtApril = Convert.ToDateTime(sApril);
            odtJuly = Convert.ToDateTime(sJuly);
            odtOctober = Convert.ToDateTime(sOctober);

            if (odtDate > odtJanuary)
                m_sDelqQtr = "1";
            if ((odtDate > odtApril) && (odtDate < odtJuly))
                m_sDelqQtr = "2";
            if ((odtDate > odtJuly) && (odtDate < odtOctober))
                m_sDelqQtr = "3";
            if (odtDate > odtOctober)
                m_sDelqQtr = "4";
            // ALJ 10052003 (e) change to date format for comparison

            m_sQuery = @"SELECT";

            if (m_iOptionFormat == 0)
                m_sReportName = "LIST OF DELINQUENT BY BARANGAY";
            else if (m_iOptionFormat == 1)
                m_sReportName = "LIST OF DELINQUENT BY DISTRICT";
            else if (m_iOptionFormat == 2)
                m_sReportName = "LIST OF DELINQUENT BY MAIN BUSINESS";
            else if (m_iOptionFormat == 3)
                m_sReportName = "LIST OF DELINQUENT BY BUSINESS STATUS";
            else if (m_iOptionFormat == 4)
                m_sReportName = "LIST OF DELINQUENT BY OWNERSHIP KIND";
            else if (m_iOptionFormat == 5)
                m_sReportName = "LIST OF DELINQUENT BY GROSS RECEIPTS";
            else if (m_iOptionFormat == 6)
            {
                m_sReportName = "LIST OF DELINQUENT BY TOP GROSS";
                m_sQuery += " (Select max(TO_NUMBER(RLD.group2)) from rep_list_delq RLD where report_name = 'LIST OF DELINQUENT BY TOP GROSS' and user_code = 'SYS_PROG') as MAX,";
            }

            m_sQuery += " rep_list_delq.heading, rep_list_delq.group1, rep_list_delq.group2, rep_list_delq.bin, rep_list_delq.bns_nm, rep_list_delq.bns_address, rep_list_delq.own_nm, rep_list_delq.own_address, rep_list_delq.bns_code, rep_list_delq.gross, rep_list_delq.capital, rep_list_delq.btax, rep_list_delq.fees_charges, rep_list_delq.surch_int, rep_list_delq.or_no, rep_list_delq.or_date, rep_list_delq.payment_term, rep_list_delq.tax_year, rep_list_delq.qtr_paid, rep_list_delq.current_user, rep_list_delq.cut_off_dt FROM REP_LIST_DELQ rep_list_delq WHERE report_name = '" + m_sReportName + "' and user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";

            if (AppSettingsManager.GenerateInfo(m_sReportName))
            {
                if (!chkTopDelinq.Checked)
                    OnReportListOfDelinquent();
                else
                    GetTopDelinquent();

                if (m_iOptionFormat == 6)
                    m_sQuery += " ORDER BY rep_list_delq.group1 ASC, to_number(rep_list_delq.group2) ASC, rep_list_delq.bns_nm ASC";
                else if (m_iOptionFormat != 5)
                    m_sQuery += " ORDER BY rep_list_delq.group1 ASC";
                else
                    m_sQuery += " ORDER BY rep_list_delq.gross DESC, rep_list_delq.bns_nm ASC";

                frmDelinquencyReport frmDelinquencyReport = new frmDelinquencyReport();
                frmDelinquencyReport.ReportName = m_sReportName;
                frmDelinquencyReport.Query = m_sQuery;
                frmDelinquencyReport.ShowDialog();
            }
        }

        private void GetTopDelinquent()
        {
            OracleResultSet pSet = new OracleResultSet();

            String sQuery, sTmpStatus;
            String sBnsCode, sCurrentUser;
            String sDelqBin, sDelqBnsBrgy, sDelqHeading, sDelqBnsName, sDelqBrgyName, sDelqBnsStat, sDelqBnsAddr, sDelqBnsCode, sDelqOwnName, sDelqOwnAddr, sDelqOwnCode, sDelqPermit, sDelqTaxYear, sDelqOrNo, sDelqQtrPaid, sDelqTerm, sDelqOrDate;
            double dDelqBnsGr, dDelqBnsCap, dDelqGross, dDelqBtax, dDelqFees, dDelqSurch, dNewGrossCapital, dOldGrossCapital, dTmpGrossCapital;
            int iRankCtr;

            if (txtTop.Text.Trim() == "")
                txtTop.Text = "0";
            //{(s)----------------------INITIALIZED ALL NECESSARY DATA--------------------
            dDelqGross = 0;
            dDelqBnsGr = 0;
            dDelqBnsCap = 0;
            dDelqGross = 0;
            dDelqBtax = 0;
            dDelqFees = 0;
            dDelqSurch = 0;
            iRankCtr = 0;
            dNewGrossCapital = 0;
            dOldGrossCapital = 0;
            dTmpGrossCapital = 0;

            sQuery = string.Format("delete from rep_list_delq where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();

            if (cmbBusStat.Text.Trim() == "ALL")
                sTmpStatus = "%%";
            else
                sTmpStatus = cmbBusStat.Text.Trim() + "%";

            #region Progress
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            sQuery = string.Format("select count(*) from rep_temp_delq where bns_stat like '{0}' order by grosscap DESC, bns_nm ASC", sTmpStatus);
            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            intCountSaved = 0;
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            sQuery = string.Format("select * from rep_temp_delq where bns_stat like '{0}' order by grosscap DESC, bns_nm ASC", sTmpStatus);
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sDelqHeading = StringUtilities.RemoveApostrophe(pSet.GetString("heading").Trim());
                    sDelqBnsBrgy = StringUtilities.RemoveApostrophe(pSet.GetString("group1").Trim());
                    sDelqBin = StringUtilities.RemoveApostrophe(pSet.GetString("bin").Trim());
                    sDelqBnsName = StringUtilities.RemoveApostrophe(pSet.GetString("bns_nm").Trim());
                    sDelqBnsAddr = StringUtilities.RemoveApostrophe(pSet.GetString("bns_address").Trim());
                    sDelqOwnName = StringUtilities.RemoveApostrophe(pSet.GetString("own_nm").Trim());
                    sDelqOwnAddr = StringUtilities.RemoveApostrophe(pSet.GetString("own_address").Trim());
                    sDelqBnsCode = StringUtilities.RemoveApostrophe(pSet.GetString("bns_code").Trim());
                    sDelqBnsStat = StringUtilities.RemoveApostrophe(pSet.GetString("bns_stat").Trim());
                    dDelqGross = pSet.GetDouble("grosscap");
                    dDelqBnsGr = pSet.GetDouble("gross");
                    dDelqBnsCap = pSet.GetDouble("capital");
                    sDelqPermit = StringUtilities.RemoveApostrophe(pSet.GetString("permit_no").Trim());
                    dDelqBtax = pSet.GetDouble("btax");
                    dDelqFees = pSet.GetDouble("fees_charges");
                    dDelqSurch = pSet.GetDouble("surch_int");
                    sDelqOrNo = StringUtilities.RemoveApostrophe(pSet.GetString("or_no").Trim());
                    sDelqOrDate = StringUtilities.RemoveApostrophe(pSet.GetString("or_date").Trim());
                    sDelqTerm = StringUtilities.RemoveApostrophe(pSet.GetString("payment_term").Trim());
                    sDelqTaxYear = StringUtilities.RemoveApostrophe(pSet.GetString("tax_year").Trim());
                    sDelqQtrPaid = StringUtilities.RemoveApostrophe(pSet.GetString("qtr_paid").Trim());

                    if (sDelqBnsStat == "REN")
                        dNewGrossCapital = dDelqGross;
                    else
                        dNewGrossCapital = dDelqBnsCap;

                    //Saving//
                    if (dNewGrossCapital != dOldGrossCapital)
                    {
                        iRankCtr = iRankCtr + 1;
                        dOldGrossCapital = dNewGrossCapital;

                        if (iRankCtr > Convert.ToInt32(txtTop.Text))
                        {
                            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                            Thread.Sleep(10);
                            return;
                        }
                    }

                    sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                               StringUtilities.HandleApostrophe(sDelqHeading.Trim()),
                               StringUtilities.HandleApostrophe(sDelqBnsBrgy.Trim()),
                               iRankCtr,
                               StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                               StringUtilities.HandleApostrophe(sDelqBnsName).Trim(),
                               StringUtilities.HandleApostrophe(sDelqBnsAddr).Trim(),
                               StringUtilities.HandleApostrophe(sDelqOwnName).Trim(),
                               StringUtilities.HandleApostrophe(sDelqOwnAddr).Trim(),
                               StringUtilities.HandleApostrophe(sDelqBnsCode).Trim(),
                               dDelqBnsGr,
                               dDelqBnsCap,
                               StringUtilities.HandleApostrophe(sDelqPermit).Trim(),
                               dDelqBtax,
                               dDelqFees,
                               dDelqSurch,
                               StringUtilities.HandleApostrophe(sDelqOrNo).Trim(),
                               StringUtilities.HandleApostrophe(sDelqOrDate).Trim(),
                               StringUtilities.HandleApostrophe(sDelqTerm).Trim(),
                               StringUtilities.HandleApostrophe(sDelqTaxYear).Trim(),
                               StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                               StringUtilities.HandleApostrophe(m_sReportName).Trim(),
                               StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode).Trim(),
                               StringUtilities.HandleApostrophe(AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode)).Trim(),
                               StringUtilities.HandleApostrophe(m_sDate).Trim());
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                    intCountSaved++;
                    DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            pSet.Close();

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);
        }

        private void OnReportListOfDelinquent()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            OracleResultSet pSet3 = new OracleResultSet();

            String sQuery = "";
            String sBnsCode = "", sBnsCodeSub = "", sQBnsCode, sTmpBrgyName, sTmpOrgnKind, sTmpStatus, sCurrentUser;
            String sDelqBin, sDelqBnsName, sDelqBrgyName = "", sDelqBnsStat = "", sDelqBnsAddr, sDelqBnsCode, sDelqOwnName, sDelqOwnAddr, sDelqOwnCode, sDelqPermit, sDelqTaxYear, sDelqOrNo, sDelqQtrPaid, sDelqTerm, sDelqOrDate;
            double dDelqBnsGr1, dDelqBnsGr2, dDelqBnsCap, dDelqGross, dBFeesDue, dRFeesDue, dFeesPen, dFeesSurch, dSurchPen, dNewGrossCapital, dOldGrossCapital, dTmpGrossCapital;
            int iBrgyCount, iProgressCtr, iRankCtr;
            String sTmpDistName, sDelqDistName = "", sDelqBnsDesc = "", sDelqOrgnKind = "";

            //{(s)----------------------INITIALIZED ALL NECESSARY DATA--------------------
            dDelqBnsGr1 = 0;
            dDelqBnsGr2 = 0;
            dDelqBnsCap = 0;
            dDelqGross = 0;
            dBFeesDue = 0;
            dRFeesDue = 0;
            dFeesPen = 0;
            dFeesSurch = 0;
            dSurchPen = 0;
            iBrgyCount = 0;
            iProgressCtr = 0;
            iRankCtr = 0;       // Counter for RANK
            dNewGrossCapital = 0;
            dOldGrossCapital = 0;
            dTmpGrossCapital = 0;

            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            if (m_sBnsCodeSub == "")
                sQBnsCode = m_sBnsCode + "%";
            else
                sQBnsCode = m_sBnsCodeSub + "%";

            if (cmbDist.Text.Trim() == "ALL")
                sTmpDistName = "%%";
            else
                sTmpDistName = cmbDist.Text.Trim() + "%";

            if (cmbBrgy.Text.Trim() == "ALL")
                sTmpBrgyName = "%%";
            else
                sTmpBrgyName = cmbBrgy.Text.Trim() + "%";

            if (cmbOrgnKind.Text.Trim() == "ALL")
                sTmpOrgnKind = "%%";
            else
                sTmpOrgnKind = cmbOrgnKind.Text.Trim() + "%";

            if (cmbBusStat.Text.Trim() == "ALL")
                sTmpStatus = "%%";
            else
                sTmpStatus = cmbBusStat.Text.Trim() + "%";

            //}(e)----------------------INITIALIZED ALL NECESSARY DATA--------------------
            if (m_iOptionFormat == 6)
            {
                pSet.Query = string.Format("DELETE FROM REP_TEMP_DELQ WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.ExecuteNonQuery();
                pSet.Close();
            }

            pSet.Query = string.Format("DELETE FROM REP_LIST_DELQ WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.ExecuteNonQuery();
            pSet.Close();

            pSet.Query = string.Format("DELETE FROM GEN_INFO WHERE REPORT_NAME = '{0}'", m_sReportName);
            pSet.ExecuteNonQuery();
            pSet.Close();

            DateTime m_dtSaveTime = AppSettingsManager.GetSystemDate();
            String sCurrentDate = string.Format("{1}/{2}/{0}",
                    m_dtSaveTime.Year,
                    m_dtSaveTime.Month,
                    m_dtSaveTime.Day);//  {3}:{4}:{5}, m_dtSaveTime.Hour, m_dtSaveTime.Minute, m_dtSaveTime.Second);

            pSet.Query = string.Format("INSERT INTO GEN_INFO VALUES('{0}',sysdate,'{1}',{2},'COL')", m_sReportName, AppSettingsManager.SystemUser.UserCode, 1);
            pSet.ExecuteNonQuery();
            pSet.Close();

            if (m_iOptionFormat == 0)
                sQuery = string.Format(@"select nvl(sum(count(distinct a.bin)),0) as cnt from pay_hist a, businesses b where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_brgy like '{1}' and b.bns_code like '{2}' and b.bns_stat like '{3}' and b.orgn_kind like '{4}' and b.bns_stat <> 'RET' group by a.bin", m_sYear, sTmpBrgyName, sQBnsCode, sTmpStatus, sTmpOrgnKind);
            else if (m_iOptionFormat == 1)
                sQuery = string.Format(@"select nvl(sum(count(distinct a.bin)),0) as cnt pay_hist a, businesses b where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_dist like '{1}' and b.bns_code like '{2}' and b.bns_stat like '{3}' and b.orgn_kind like '{4}' group by a.bin", m_sYear, sTmpDistName, sQBnsCode, sTmpStatus, sTmpOrgnKind);
            else if (m_iOptionFormat == 2)
                sQuery = string.Format(@"select nvl(sum(count(distinct a.bin)),0) as cnt from pay_hist a, businesses b where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_brgy like '{1}' and b.bns_code like '{2}' and b.bns_stat like '{3}' and b.bns_stat <> 'RET' and b.orgn_kind like '{4}' group by a.bin", m_sYear, sTmpBrgyName, sQBnsCode, sTmpStatus, sTmpOrgnKind);
            else if (m_iOptionFormat == 3)
                sQuery = string.Format(@"select nvl(sum(count(distinct a.bin)),0) as cnt from pay_hist a, businesses b where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_brgy like '{1}' and b.bns_code like '{2}' and b.bns_stat like '{3}' and b.bns_stat <> 'RET' and b.orgn_kind like '{4}' group by a.bin", m_sYear, sTmpBrgyName, sQBnsCode, sTmpStatus, sTmpOrgnKind);
            else if (m_iOptionFormat == 4)
                sQuery = string.Format(@"select nvl(sum(count(distinct a.bin)),0) as cnt from pay_hist a, businesses b where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_brgy like '{1}' and b.bns_code like '{2}' and b.bns_stat like '{3}' and b.bns_stat <> 'RET' and b.orgn_kind = '{4}' group by a.bin", m_sYear, sTmpBrgyName, sQBnsCode, sTmpStatus, sTmpOrgnKind);
            else if (m_iOptionFormat == 5)
                sQuery = string.Format(@"select nvl(sum(count(distinct a.bin)),0) as cnt from pay_hist a, businesses b where a.tax_year <= '{0}' and a.bin = b.bin and a.data_mode <> 'UNP' and a.bns_stat = 'REN' and a.bns_stat = b.bns_stat and b.bns_dist like '{1}' and b.bns_code like '{2}' and (b.gr_1+b.gr_2) between '{3}' and '{4}' group by a.bin", m_sYear, sTmpDistName, sQBnsCode, txtGrossFrom.Text, txtGrossTo.Text);
            else
                sQuery = string.Format(@"select nvl(sum(count(distinct a.bin)),0) as cnt from pay_hist a, businesses b where a.tax_year <= '{0}' and a.bin = b.bin and a.data_mode <> 'UNP' and a.bns_stat like '{1}%' and a.bns_stat = b.bns_stat and b.bns_dist like '{2}' and b.bns_code like '{3}' group by a.bin", m_sYear, sTmpStatus, sTmpDistName, sQBnsCode);

            #region Progress
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            intCountSaved = 0;
            intCountIncreament = 0;
            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            if (m_iOptionFormat == 0)
                sQuery = string.Format("select distinct bns_brgy from businesses where trim(bns_brgy) like '{0}' and bns_stat <> 'RET' order by bns_brgy", sTmpBrgyName);
            else if (m_iOptionFormat == 1 || m_iOptionFormat == 5 || m_iOptionFormat == 6)
                sQuery = string.Format("select distinct dist_nm from brgy where dist_nm like '{0}' order by dist_nm", sTmpDistName);
            else if (m_iOptionFormat == 2)
                sQuery = string.Format("select bns_code,bns_desc from bns_table where bns_code like '{0}' and fees_code like 'B%%' order by bns_code", sQBnsCode);
            else if (m_iOptionFormat == 3)
                sQuery = string.Format("select distinct bns_stat from businesses where bns_brgy like '{0}' and bns_stat like '{1}' and bns_stat <> 'RET' and orgn_kind like '{2}' order by bns_stat", sTmpBrgyName, sTmpStatus, sTmpOrgnKind);
            else // if (m_iOptionFormat == 4)
                sQuery = string.Format("select distinct orgn_kind from businesses where bns_brgy like '{0}' and bns_stat like '{1}' and bns_stat <> 'RET' and orgn_kind like '{2}' order by orgn_kind", sTmpBrgyName, sTmpStatus, sTmpOrgnKind);

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    if (m_iOptionFormat == 0)
                    {
                        sDelqBrgyName = StringUtilities.RemoveApostrophe(pSet.GetString("bns_brgy").Trim());  // JJP 10082003 for Manila
                        sQuery = string.Format(@"select distinct a.bin,bns_code,b.bns_nm,b.bns_stat,b.own_code,b.permit_no,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
					        where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_brgy = '{1}' and b.bns_code like '{2}' and b.bns_stat like '{3}'
							and b.orgn_kind like '{4}' and b.bns_stat <> 'RET' order by b.bns_nm,a.bin,bns_code", m_sYear, sDelqBrgyName, sQBnsCode, sTmpStatus, sTmpOrgnKind); // GDE 20090520 remove ret
                    }
                    else if (m_iOptionFormat == 1)
                    {
                        sDelqDistName = StringUtilities.RemoveApostrophe(pSet.GetString("dist_nm").Trim());// +"%";
                        sQuery = string.Format(@"select distinct a.bin,bns_code,b.bns_nm,b.bns_stat,b.own_code,b.permit_no,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
							where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_dist = '{1}' and b.bns_code like '{2}' and b.bns_stat like '{3}'
							and b.orgn_kind like '{4}' order by b.bns_nm,a.bin,bns_code", m_sYear, sDelqDistName, sQBnsCode, sTmpStatus, sTmpOrgnKind);
                    }
                    else if (m_iOptionFormat == 2)
                    {
                        sDelqBnsCode = StringUtilities.RemoveApostrophe(pSet.GetString("bns_code").Trim());
                        sDelqBnsDesc = StringUtilities.RemoveApostrophe(pSet.GetString("bns_desc").Trim());

                        sQuery = string.Format(@"select distinct a.bin,bns_code,b.bns_nm,b.bns_stat,b.own_code,b.permit_no,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
							where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_brgy like '{1}' and b.bns_code = '{2}' and b.bns_stat like '{3}' and b.bns_stat <> 'RET'
							and b.orgn_kind like '{4}' order by b.bns_nm,a.bin,bns_code", m_sYear, sTmpBrgyName, sDelqBnsCode, sTmpStatus, sTmpOrgnKind);
                    }
                    else if (m_iOptionFormat == 3)
                    {
                        sDelqBnsStat = StringUtilities.RemoveApostrophe(pSet.GetString("bns_stat").Trim());
                        sQuery = string.Format(@"select distinct a.bin,bns_code,b.bns_nm,b.bns_brgy,b.own_code,b.permit_no,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
							where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_brgy like '{1}' and b.bns_code like '{2}' and b.bns_stat like '{3}' and b.bns_stat <> 'RET'
							and b.orgn_kind like '{4}' order by b.bns_nm,a.bin,bns_code", m_sYear, sTmpBrgyName, sQBnsCode, sDelqBnsStat, sTmpOrgnKind);
                    }
                    else if (m_iOptionFormat == 4)
                    {
                        sDelqOrgnKind = StringUtilities.RemoveApostrophe(pSet.GetString("orgn_kind").Trim());
                        sQuery = string.Format(@"select distinct a.bin,bns_code,b.bns_nm,b.bns_stat,b.own_code,b.permit_no,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
							where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_brgy like '{1}' and b.bns_code like '{2}' and b.bns_stat like '{3}' and b.bns_stat <> 'RET'
							and b.orgn_kind = '{4}' order by b.bns_nm,a.bin,bns_code", m_sYear, sTmpBrgyName, sQBnsCode, sTmpStatus, sDelqOrgnKind);
                    }
                    else if (m_iOptionFormat == 5)
                    {
                        sDelqDistName = StringUtilities.RemoveApostrophe(pSet.GetString("dist_nm").Trim()) + "%";
                        sQuery = string.Format(@"select distinct a.bin,bns_code,b.bns_nm,b.bns_stat,b.own_code,b.permit_no,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
							where a.tax_year <= '{0}' and a.bin = b.bin and a.data_mode <> 'UNP' and a.bns_stat = 'REN' and a.bns_stat = b.bns_stat and b.bns_dist like '{1}'
							and b.bns_code like '{2}' and (b.gr_1+b.gr_2) between '{3}' and '{4}'", m_sYear, sDelqDistName, sQBnsCode, txtGrossFrom.Text, txtGrossTo.Text);
                    }
                    else
                    {
                        sDelqDistName = StringUtilities.RemoveApostrophe(pSet.GetString("dist_nm").Trim()) + "%";
                        sQuery = string.Format(@"select distinct a.bin,bns_code,b.bns_nm,b.bns_stat,b.own_code,b.permit_no,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
					        where a.tax_year <= '{0}' and a.bin = b.bin and a.data_mode <> 'UNP' and a.bns_stat like '{1}%' and a.bns_stat = b.bns_stat and b.bns_dist like '{2}'
							and b.bns_code like '{3}'", m_sYear, sTmpStatus, sDelqDistName, sQBnsCode);
                    }

                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                    {
                        while (pSet1.Read())
                        {
                            DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                            intCountIncreament += 1;

                            sDelqBin = StringUtilities.RemoveApostrophe(pSet1.GetString("bin").Trim());
                            sDelqBnsCode = StringUtilities.RemoveApostrophe(pSet1.GetString("bns_code").Trim());
                            sDelqBnsName = StringUtilities.RemoveApostrophe(pSet1.GetString("bns_nm").Trim());
                            if (m_iOptionFormat != 3)
                                sDelqBnsStat = StringUtilities.RemoveApostrophe(pSet1.GetString("bns_stat").Trim());
                            sDelqOwnCode = StringUtilities.RemoveApostrophe(pSet1.GetString("own_code").Trim());
                            sDelqPermit = StringUtilities.RemoveApostrophe(pSet1.GetString("permit_no").Trim());
                            dDelqBnsGr1 = pSet1.GetDouble("gr_1");
                            dDelqBnsGr2 = pSet1.GetDouble("gr_2");
                            dDelqBnsCap = pSet1.GetDouble("capital");

                            dDelqGross = dDelqBnsGr1 + dDelqBnsGr2;

                            sDelqBnsAddr = AppSettingsManager.GetBnsAdd(sDelqBin, "");
                            sDelqOwnName = AppSettingsManager.GetBnsOwner(sDelqOwnCode);
                            sDelqOwnAddr = AppSettingsManager.GetBnsOwnAdd(sDelqOwnCode);

                            if (m_iOptionFormat == 0 || m_iOptionFormat == 2 || m_iOptionFormat == 3 || m_iOptionFormat == 4)
                                sQuery = string.Format("select tax_year,qtr_paid,or_no,or_date,payment_term from pay_hist where bin = '{0}' and bns_stat <> 'RET' order by tax_year desc, qtr_paid desc", sDelqBin);
                            else // if (m_iOptionFormat == 1 || m_iOptionFormat == 5 || m_iOptionFormat == 6 )
                            {
                                if (m_iOptionFormat == 6)
                                {
                                    if (sDelqBnsStat == "REN")
                                        dTmpGrossCapital = dDelqGross;
                                    else
                                        dTmpGrossCapital = dDelqBnsCap;
                                }
                                sQuery = string.Format("select tax_year,qtr_paid,or_no,or_date,payment_term from pay_hist where bin = '{0}' order by tax_year desc, qtr_paid desc", sDelqBin);
                            }
                            pSet2.Query = sQuery;
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                {
                                    sDelqTaxYear = StringUtilities.RemoveApostrophe(pSet2.GetString("tax_year").Trim());
                                    sDelqQtrPaid = StringUtilities.RemoveApostrophe(pSet2.GetString("qtr_paid").Trim());
                                    sDelqOrNo = StringUtilities.RemoveApostrophe(pSet2.GetString("or_no").Trim());
                                    sDelqOrDate = pSet2.GetDateTime("or_date").ToShortDateString();
                                    sDelqTerm = StringUtilities.RemoveApostrophe(pSet2.GetString("payment_term").Trim());

                                    sQuery = string.Format("select sum(fees_due) as sumBFeesDue from or_table where or_no = '{0}' and fees_code like 'B%%'", sDelqOrNo);	// JGR 09192005 Oracle Adjustment
                                    pSet3.Query = sQuery;
                                    if (pSet3.Execute())
                                        if (pSet3.Read())
                                        {
                                            dBFeesDue = pSet3.GetDouble("sumBFeesDue");
                                        }
                                    pSet3.Close();

                                    sQuery = string.Format("select sum(fees_due) as sumRFeesDue from or_table where or_no = '{0}' and fees_code not like 'B%%'", sDelqOrNo);	// JGR 09192005 Oracle Adjustment
                                    pSet3.Query = sQuery;
                                    if (pSet3.Execute())
                                        if (pSet3.Read())
                                        {
                                            dRFeesDue = pSet3.GetDouble("sumRFeesDue");
                                        }
                                    pSet3.Close();

                                    sQuery = string.Format("select sum(fees_pen) as sumFeesPen, sum(fees_surch) as sumFeesSurch from or_table where or_no = '{0}'", sDelqOrNo);	// JGR 09192005 Oracle Adjustment
                                    pSet3.Query = sQuery;
                                    if (pSet3.Execute())
                                        if (pSet3.Read())
                                        {
                                            dFeesPen = pSet3.GetDouble("sumFeesPen");
                                            dFeesSurch = pSet3.GetDouble("sumFeesSurch");

                                            dSurchPen = dFeesPen + dFeesSurch;
                                        }
                                    pSet3.Close();

                                    if (m_iOptionFormat == 0)
                                    {
                                        #region 0
                                        //(s){---------------------------------SAVING VALUES-----------------------------
                                        if (Convert.ToInt32(sDelqTaxYear) < Convert.ToInt32(m_sYear))
                                        {
                                            sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                       StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBrgyName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                       dDelqGross.ToString("0.00"),
                                                       dDelqBnsCap.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                       dBFeesDue.ToString("0.00"),
                                                       dRFeesDue.ToString("0.00"),
                                                       dSurchPen.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                       StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                      StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                       StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                       StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                            pSet.Query = sQuery;
                                            pSet.ExecuteNonQuery();

                                            intCountSaved++;
                                        }
                                        int numValue = 0;
                                        bool parse = Int32.TryParse(sDelqQtrPaid, out numValue);

                                        if (sDelqQtrPaid != "F")
                                            if ((Convert.ToInt32(sDelqTaxYear) == Convert.ToInt32(m_sYear)) && (numValue < Convert.ToInt32(m_sDelqQtr)))
                                            {
                                                sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                           StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBrgyName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                           dDelqGross.ToString("0.00"),
                                                           dDelqBnsCap.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                           dBFeesDue.ToString("0.00"),
                                                           dRFeesDue.ToString("0.00"),
                                                           dSurchPen.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                           StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                          StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                           StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                           StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                                pSet.Query = sQuery;
                                                pSet.ExecuteNonQuery();

                                                intCountSaved++;
                                            }
                                        //(e)}---------------------------------SAVING VALUES-----------------------------
                                        #endregion 0
                                    }
                                    else if (m_iOptionFormat == 1)
                                    {
                                        if (sDelqDistName == "%")
                                            sDelqDistName = "NO DISTRICT";

                                        #region 1
                                        if (Convert.ToInt32(sDelqTaxYear) < Convert.ToInt32(m_sYear))
                                        {
                                            sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                       StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqDistName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                       dDelqGross.ToString("0.00"),
                                                       dDelqBnsCap.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                       dBFeesDue.ToString("0.00"),
                                                       dRFeesDue.ToString("0.00"),
                                                       dSurchPen.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                       StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                      StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                       StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                       StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                            pSet.Query = sQuery;
                                            pSet.ExecuteNonQuery();

                                            intCountSaved++;
                                        }
                                        int numValue = 0;
                                        bool parse = Int32.TryParse(sDelqQtrPaid, out numValue);

                                        if (sDelqQtrPaid != "F")
                                            if ((Convert.ToInt32(sDelqTaxYear) == Convert.ToInt32(m_sYear)) && (numValue < Convert.ToInt32(m_sDelqQtr)))
                                            {
                                                sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                           StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqDistName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                           dDelqGross.ToString("0.00"),
                                                           dDelqBnsCap.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                           dBFeesDue.ToString("0.00"),
                                                           dRFeesDue.ToString("0.00"),
                                                           dSurchPen.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                           StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                          StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                           StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                           StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                                pSet.Query = sQuery;
                                                pSet.ExecuteNonQuery();

                                                intCountSaved++;
                                            }
                                        #endregion
                                    }
                                    else if (m_iOptionFormat == 2)
                                    {
                                        #region 2
                                        if (Convert.ToInt32(sDelqTaxYear) < Convert.ToInt32(m_sYear))
                                        {
                                            sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                       StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsDesc.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                       dDelqGross.ToString("0.00"),
                                                       dDelqBnsCap.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                       dBFeesDue.ToString("0.00"),
                                                       dRFeesDue.ToString("0.00"),
                                                       dSurchPen.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                       StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                      StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                       StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                       StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                            pSet.Query = sQuery;
                                            pSet.ExecuteNonQuery();

                                            intCountSaved++;
                                        }
                                        int numValue = 0;
                                        bool parse = Int32.TryParse(sDelqQtrPaid, out numValue);

                                        if (sDelqQtrPaid != "F")
                                            if ((Convert.ToInt32(sDelqTaxYear) == Convert.ToInt32(m_sYear)) && (numValue < Convert.ToInt32(m_sDelqQtr)))
                                            {
                                                sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                           StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsDesc.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                           dDelqGross.ToString("0.00"),
                                                           dDelqBnsCap.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                           dBFeesDue.ToString("0.00"),
                                                           dRFeesDue.ToString("0.00"),
                                                           dSurchPen.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                           StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                          StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                           StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                           StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                                pSet.Query = sQuery;
                                                pSet.ExecuteNonQuery();

                                                intCountSaved++;
                                            }
                                        #endregion
                                    }
                                    else if (m_iOptionFormat == 3)
                                    {
                                        #region 3
                                        if (Convert.ToInt32(sDelqTaxYear) < Convert.ToInt32(m_sYear))
                                        {
                                            sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                       StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBrgyName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                       dDelqGross.ToString("0.00"),
                                                       dDelqBnsCap.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                       dBFeesDue.ToString("0.00"),
                                                       dRFeesDue.ToString("0.00"),
                                                       dSurchPen.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                       StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                      StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                       StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                       StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                            pSet.Query = sQuery;
                                            pSet.ExecuteNonQuery();

                                            intCountSaved++;
                                        }
                                        int numValue = 0;
                                        bool parse = Int32.TryParse(sDelqQtrPaid, out numValue);

                                        if (sDelqQtrPaid != "F")
                                            if ((Convert.ToInt32(sDelqTaxYear) == Convert.ToInt32(m_sYear)) && (numValue < Convert.ToInt32(m_sDelqQtr)))
                                            {
                                                sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                           StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBrgyName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                           dDelqGross.ToString("0.00"),
                                                           dDelqBnsCap.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                           dBFeesDue.ToString("0.00"),
                                                           dRFeesDue.ToString("0.00"),
                                                           dSurchPen.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                           StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                          StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                           StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                           StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                                pSet.Query = sQuery;
                                                pSet.ExecuteNonQuery();

                                                intCountSaved++;
                                            }
                                        #endregion
                                    }
                                    else if (m_iOptionFormat == 4)
                                    {
                                        #region 4
                                        if (Convert.ToInt32(sDelqTaxYear) < Convert.ToInt32(m_sYear))
                                        {
                                            sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                       StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOrgnKind.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                       dDelqGross.ToString("0.00"),
                                                       dDelqBnsCap.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                       dBFeesDue.ToString("0.00"),
                                                       dRFeesDue.ToString("0.00"),
                                                       dSurchPen.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                       StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                      StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                       StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                       StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                            pSet.Query = sQuery;
                                            pSet.ExecuteNonQuery();

                                            intCountSaved++;
                                        }
                                        int numValue = 0;
                                        bool parse = Int32.TryParse(sDelqQtrPaid, out numValue);

                                        if (sDelqQtrPaid != "F")
                                            if ((Convert.ToInt32(sDelqTaxYear) == Convert.ToInt32(m_sYear)) && (numValue < Convert.ToInt32(m_sDelqQtr)))
                                            {
                                                sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                           StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOrgnKind.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                           dDelqGross.ToString("0.00"),
                                                           dDelqBnsCap.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                           dBFeesDue.ToString("0.00"),
                                                           dRFeesDue.ToString("0.00"),
                                                           dSurchPen.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                           StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                          StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                           StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                           StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                                pSet.Query = sQuery;
                                                pSet.ExecuteNonQuery();

                                                intCountSaved++;
                                            }
                                        #endregion
                                    }
                                    else if (m_iOptionFormat == 5)
                                    {
                                        String sDistrict;
                                        if (sTmpDistName == "%%")
                                            sDistrict = "ALL";
                                        else
                                            sDistrict = sDelqDistName;

                                        #region 5
                                        if (Convert.ToInt32(sDelqTaxYear) < Convert.ToInt32(m_sYear))
                                        {
                                            sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                       StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                       StringUtilities.HandleApostrophe(sDistrict.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                       dDelqGross.ToString("0.00"),
                                                       dDelqBnsCap.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                       dBFeesDue.ToString("0.00"),
                                                       dRFeesDue.ToString("0.00"),
                                                       dSurchPen.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                      StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                       StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                       StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                      StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                       StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                       StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                            pSet.Query = sQuery;
                                            pSet.ExecuteNonQuery();

                                            intCountSaved++;
                                        }
                                        int numValue = 0;
                                        bool parse = Int32.TryParse(sDelqQtrPaid, out numValue);

                                        if (sDelqQtrPaid != "F")
                                            if ((Convert.ToInt32(sDelqTaxYear) == Convert.ToInt32(m_sYear)) && (numValue < Convert.ToInt32(m_sDelqQtr)))
                                            {
                                                sQuery = string.Format(@"insert into rep_list_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                           StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                                           StringUtilities.HandleApostrophe(sDistrict.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsStat.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBin.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqBnsAddr.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnName.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOwnAddr.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqBnsCode.Trim()),
                                                           dDelqGross.ToString("0.00"),
                                                           dDelqBnsCap.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqPermit.Trim()),
                                                           dBFeesDue.ToString("0.00"),
                                                           dRFeesDue.ToString("0.00"),
                                                           dSurchPen.ToString("0.00"),
                                                           StringUtilities.HandleApostrophe(sDelqOrNo.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqOrDate.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                          StringUtilities.HandleApostrophe(sDelqTaxYear.Trim()),
                                                           StringUtilities.HandleApostrophe(sDelqQtrPaid).Trim(),
                                                           StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                          StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                           StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                                           StringUtilities.HandleApostrophe(m_sDate.Trim()));

                                                pSet.Query = sQuery;
                                                pSet.ExecuteNonQuery();

                                                intCountSaved++;
                                            }
                                        #endregion
                                    }
                                    else
                                    {
                                        String sDistrict;
                                        if (sTmpDistName == "%%")
                                            sDistrict = "ALL";
                                        else
                                            sDistrict = sDelqDistName;

                                        #region 6
                                        if (Convert.ToInt32(sDelqTaxYear) < Convert.ToInt32(m_sYear))
                                        {
                                            sQuery = string.Format(@"insert into rep_temp_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}')",
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.GetConfigValue("09"), 50)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDistrict.Trim(), 30)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsStat.Trim(), 30)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBin.Trim(), 19)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsName.Trim(), 300)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsAddr.Trim(), 100)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqOwnName.Trim(), 200)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqOwnAddr.Trim(), 100)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsCode.Trim(), 8)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsStat.Trim(), 3)),
                                                       dTmpGrossCapital.ToString("0.00"),
                                                       dDelqGross.ToString("0.00"),
                                                       dDelqBnsCap.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqPermit.Trim(), 20)),
                                                       dBFeesDue.ToString("0.00"),
                                                       dRFeesDue.ToString("0.00"),
                                                       dSurchPen.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqOrNo.Trim(), 10)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqOrDate.Trim(), 10)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqTerm.Trim(), 1)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqTaxYear.Trim(), 4)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqQtrPaid.Trim(), 1)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName.Trim(), 100)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser.Trim(), 30)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(m_sDate.Trim(), 10)));
                                            pSet.Query = sQuery;
                                            pSet.ExecuteNonQuery();

                                            intCountSaved++;
                                        }

                                        int numValue = 0;
                                        bool parse = Int32.TryParse(sDelqQtrPaid, out numValue);

                                        if (sDelqQtrPaid != "F")
                                            if ((Convert.ToInt32(sDelqTaxYear) == Convert.ToInt32(m_sYear)) && (numValue < Convert.ToInt32(m_sDelqQtr)))
                                            {
                                                sQuery = string.Format(@"insert into rep_temp_delq values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}')",
                                                           StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.GetConfigValue("09"), 50)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDistrict.Trim(), 30)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsStat.Trim(), 30)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBin.Trim(), 19)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsName.Trim(), 300)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsAddr.Trim(), 100)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqOwnName.Trim(), 200)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqOwnAddr.Trim(), 100)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsCode.Trim(), 8)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsStat.Trim(), 3)),
                                                       dTmpGrossCapital.ToString("0.00"),
                                                       dDelqGross.ToString("0.00"),
                                                       dDelqBnsCap.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqPermit.Trim(), 20)),
                                                       dBFeesDue.ToString("0.00"),
                                                       dRFeesDue.ToString("0.00"),
                                                       dSurchPen.ToString("0.00"),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqOrNo.Trim(), 10)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqOrDate.Trim(), 10)),
                                                       StringUtilities.HandleApostrophe(sDelqTerm.Trim()),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqTaxYear.Trim(), 4)),
                                                       StringUtilities.HandleApostrophe(sDelqQtrPaid.Trim()),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName.Trim(), 100)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser.Trim(), 30)),
                                                       StringUtilities.HandleApostrophe(StringUtilities.Left(m_sDate.Trim(), 10)));
                                                pSet.Query = sQuery;
                                                pSet.ExecuteNonQuery();

                                                intCountSaved++;
                                            }
                                        #endregion
                                    }
                                }

                            pSet2.Close();
                            DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                            Thread.Sleep(3);
                        }
                        pSet1.Close();
                    }

                }
            }
            pSet.Close();

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            if (m_iOptionFormat == 6)
                GetTopDelinquent();

            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

    }
}


