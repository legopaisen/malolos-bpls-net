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
using Amellar.Modules.Payment;
using Amellar.Common.Reports;
using Amellar.Common.DynamicProgressBar;
using System.Threading;

namespace Amellar.Modules.Collectibles
{
    public partial class frmCollectibles : Form
    {
        public frmCollectibles()
        {
            InitializeComponent();
        }

        private int m_iOptionFormat = 0;

        public int OptionFormat
        {
            set { m_iOptionFormat = value; }
        }

        ArrayList arr_BnsCode = new ArrayList();
        ArrayList arr_BnsCodeSub = new ArrayList();
        string m_sBnsCodeMain = String.Empty, m_sBnsCodeSub = String.Empty, m_sQuery = String.Empty, m_sReportName = String.Empty;
        int m_iRadioNumber = 0, m_iField = 0;

        //MCR 20140714 (s)
        frmProgress m_form; 
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
                //if (m_form.Cancel)
                //{
                //    if (m_objThread != null)
                //    {
                //        if (m_objThread.ThreadState == ThreadState.Running)
                //        {
                //            m_form.Dispose();
                //            m_objThread.Abort();
                //        }
                //    }
                //}
            }
        }
        //MCR 20140714 (e)

        //OnReportListofCollectibleBarangay
        String m_sGroup = "";
        String m_sMonthlyCutoffDate = "", m_sInitialDueDate = "", m_sDtOperated = "";
        double m_dPenRate = 0, m_dSurchRate = 0;
        String m_sTaxYear = "", m_sBrgyNm = "", m_sMainBns = "", m_sBnsCode = "", m_sBnsStat = "", m_sOrgKind = "", m_sDistNm = "", m_sUserCode = "";

        //ComputePenRate
        double mp_dSurchRate1 = 0;
        double mp_dSurchRate2 = 0;
        double mp_dSurchRate3 = 0;
        double mp_dSurchRate4 = 0;
        double mp_dPenRate1 = 0;
        double mp_dPenRate2 = 0;
        double mp_dPenRate3 = 0;
        double mp_dPenRate4 = 0;
        double mp_SurchQuart = 0;
        double mp_PenQuart = 0;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCollectibles_Load(object sender, EventArgs e)
        {
            // RMC 20150524 corrections in reports (s)
            if (AppSettingsManager.GetConfigValue("01") == "MUNICIPALITY")
                rdoDist.Enabled = false;
            // RMC 20150524 corrections in reports (e)

            if (m_iOptionFormat == 0)
                this.Text = "List of Collectibles";
            else
            {
                this.Text = "Summary of Collectibles";
                rdoTopCollectibles.Visible = false;
                lblTop.Visible = false;
                txtTop.Visible = false;
                BtnControl(false);
            }

            LoadBrgy();
            LoadDist();
            LoadBusType();
            LoadLineofBusiness();
            LoadTaxYear();
            rdoBrgy.PerformClick();
        }

        private void LoadTaxYear()
        {
            cmbTaxYear.Items.Add("ALL");

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select distinct(tax_year) from rep_list_coll order by tax_year desc";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbTaxYear.Items.Add(pSet.GetString("tax_year").Trim());
                }
            }
            pSet.Close();
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

        private void cmbBusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_sBnsCodeMain = arr_BnsCode[cmbBusType.SelectedIndex].ToString();
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

        private void cmbLineBus_DropDown(object sender, EventArgs e)
        {
            LoadLineofBusiness();
        }

        private void BtnControl(bool blnEnable)
        {
            cmbBrgy.Enabled = blnEnable;
            cmbDist.Enabled = blnEnable;
            cmbBusType.Enabled = blnEnable;
            cmbLineBus.Enabled = blnEnable;
            cmbBusStat.Enabled = blnEnable;
            cmbOrgnKind.Enabled = blnEnable;
            txtTop.Enabled = blnEnable;
        }

        private void rdoBrgy_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 0;
            if (m_iOptionFormat == 0)
            {
                BtnControl(true);
                cmbDist.Enabled = false;
                txtTop.Enabled = false;
            }
            cmbBusStat.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbTaxYear.SelectedIndex = 0;
        }

        private void rdoMainBus_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 1;
            if (m_iOptionFormat == 0)
            {
                BtnControl(true);
                cmbDist.Enabled = false;
                txtTop.Enabled = false;
            }
            cmbBusStat.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbTaxYear.SelectedIndex = 0;
        }

        private void rdoBusStat_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 2;
            if (m_iOptionFormat == 0)
            {
                BtnControl(true);
                cmbDist.Enabled = false;
                txtTop.Enabled = false;
            }
            cmbBusStat.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbTaxYear.SelectedIndex = 0;
        }

        private void rdoOwnKind_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 3;
            if (m_iOptionFormat == 0)
            {
                BtnControl(true);
                cmbDist.Enabled = false;
                txtTop.Enabled = false;
            }
            cmbBusStat.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbTaxYear.SelectedIndex = 0;
        }

        private void rdoDist_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 4;
            if (m_iOptionFormat == 0)
            {
                BtnControl(true);
                cmbBrgy.Enabled = false;
                txtTop.Enabled = false;
            }
            cmbBusStat.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbTaxYear.SelectedIndex = 0;
        }

        private void rdoTopCollectibles_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 5;
            if (m_iOptionFormat == 0)
            {
                BtnControl(true);
                cmbDist.Enabled = false;
                txtTop.Focus();
            }
            cmbBusStat.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
            cmbTaxYear.SelectedIndex = 0;
        }

        private void chkFilter_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkFilter.Checked)
                cmbTaxYear.Enabled = true;
            else
                cmbTaxYear.Enabled = false;
        }

        private void txtTop_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbTaxYear.Text.Trim() == "ALL")
                m_sTaxYear = "%";
            else
                m_sTaxYear = cmbTaxYear.Text.Trim() + "%";

            if (cmbDist.Text.Trim() == "ALL")
                m_sDistNm = "%";
            else
                m_sDistNm = cmbDist.Text.Trim() + "%";

            if (cmbBrgy.Text.Trim() == "ALL")
                m_sBrgyNm = "%";
            else
                m_sBrgyNm = cmbBrgy.Text.Trim() + "%";

            if (cmbBusStat.Text.Trim() == "ALL")
                m_sBnsStat = "%";
            else if (cmbBusStat.Text.Trim() == "RENEWAL")
                m_sBnsStat = "REN";
            else if (cmbBusStat.Text.Trim() == "RETIRED")
                m_sBnsStat = "RET";

            if (cmbOrgnKind.Text.Trim() == "ALL")
                m_sOrgKind = "%";
            else
                m_sOrgKind = cmbOrgnKind.Text.Trim() + "%";

            if (cmbLineBus.Text.Trim() == "ALL")
                m_sBnsCode = m_sBnsCodeMain + "%";
            else
                m_sBnsCode = m_sBnsCodeSub + "%";

            m_sUserCode = AppSettingsManager.SystemUser.UserCode;

            if (m_iRadioNumber == 0)
            {
                //m_iField = 12; // bns_brgy
                m_iField = 4; // bns_brgy
                if (m_iOptionFormat == 0)
                    m_sReportName = "LIST OF RECEIVABLES BY BARANGAY";
                else
                    m_sReportName = "SUMMARY OF COLLECTIBLES BY BARANGAY";
            }
            else if (m_iRadioNumber == 1)
            {
                //m_iField = 29; // bns_code
                m_iField = 5; // bns_code
                if (m_iOptionFormat == 0)
                    m_sReportName = "LIST OF RECEIVABLES BY MAIN BUSINESS";
                else
                    m_sReportName = "SUMMARY OF COLLECTIBLES BY MAIN BUSINESS";
            }
            else if (m_iRadioNumber == 2)
            {
                //m_iField = 6; // bns_stat
                m_iField = 6; // bns_stat
                if (m_iOptionFormat == 0)
                    m_sReportName = "LIST OF RECEIVABLES BY BUSINESS STATUS";
                else
                    m_sReportName = "SUMMARY OF COLLECTIBLES BY BUSINESS STATUS";
            }
            else if (m_iRadioNumber == 3)
            {
                //m_iField = 24; // orgn_kind
                m_iField = 7; // orgn_kind
                if (m_iOptionFormat == 0)
                    m_sReportName = "LIST OF RECEIVABLES BY OWNERSHIP KIND";
                else
                    m_sReportName = "SUMMARY OF COLLECTIBLES BY OWNERSHIP KIND";
            }
            else if (m_iRadioNumber == 4)
            {
                m_iField = 8; // dist_nm
                if (m_iOptionFormat == 0)
                    m_sReportName = "LIST OF RECEIVABLES BY DISTRICT";
                else
                    m_sReportName = "SUMMARY OF COLLECTIBLES BY DISTRICT";
            }
            else if (m_iRadioNumber == 5)
            {
                //m_iField = 12; // bns_brgy 
                m_iField = 4; // bns_brgy 
                m_sReportName = "LIST OF TOP N COLLECTIBLES";
            }

            if (AppSettingsManager.GenerateInfo(m_sReportName))
            {
                OnReportListOfCollectiblesByBarangay();

                if (m_iOptionFormat == 0)
                {
                    m_sQuery = "SELECT rep_list_coll.heading, rep_list_coll.data_group, rep_list_coll.bin, rep_list_coll.bns_nm, rep_list_coll.bns_addr, rep_list_coll.owner_name, rep_list_coll.owner_addr, rep_list_coll.main_bns, rep_list_coll.bns_stat, rep_list_coll.last_dt_pay, rep_list_coll.tax_year, rep_list_coll.qtr_to_pay, rep_list_coll.tax_amt, rep_list_coll.fees_amt, rep_list_coll.charges_amt ";
                    m_sQuery += "FROM REP_LIST_COLL rep_list_coll WHERE report_name = '" + m_sReportName + "' and user_code = '" + AppSettingsManager.SystemUser.UserCode + "' ";   
                    m_sQuery += "ORDER BY rep_list_coll.data_group ASC, rep_list_coll.bin ASC";//, rep_list_coll.tax_year ASC, rep_list_coll.qtr_to_pay ASC, rep_list_coll.tax_amt DESC ";
                }
                else
                {
                    m_sQuery = "SELECT rep_list_coll.heading, rep_list_coll.data_group, rep_list_coll.bin, rep_list_coll.tax_year, rep_list_coll.tax_amt, rep_list_coll.fees_amt, rep_list_coll.charges_amt ";
                    m_sQuery += "FROM REP_LIST_COLL rep_list_coll WHERE report_name = '" + m_sReportName + "' and user_code = '" + AppSettingsManager.SystemUser.UserCode + "' ";
                    m_sQuery += "ORDER BY rep_list_coll.data_group ASC, rep_list_coll.bin ASC";
                }

                if (m_iRadioNumber == 5)
                {
                    m_sQuery = "SELECT rep_list_coll.heading, rep_list_coll.data_group, rep_list_coll.subgroup1, rep_list_coll.bin, rep_list_coll.bns_nm, rep_list_coll.bns_addr, rep_list_coll.owner_name, rep_list_coll.owner_addr, rep_list_coll.main_bns, rep_list_coll.bns_stat, rep_list_coll.last_dt_pay, rep_list_coll.tax_year, rep_list_coll.qtr_to_pay, rep_list_coll.tax_amt, rep_list_coll.fees_amt, rep_list_coll.charges_amt ";
                    //m_sQuery += "FROM  REP_LIST_COLL rep_list_coll WHERE  report_name = '" + m_sReportName + "' and user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and data_group = '" + txtTop.Text.Trim() + "' ";
                    m_sQuery += "FROM  REP_LIST_COLL rep_list_coll WHERE  report_name like 'LIST OF TOP%' and user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and data_group = '" + txtTop.Text.Trim() + "' ";   // RMC 20161209 correction in top collectibles
                    m_sQuery += "ORDER BY rep_list_coll.subgroup1 ASC, rep_list_coll.bin ASC";//, rep_list_coll.tax_year ASC, rep_list_coll.qtr_to_pay ASC";
                }

                frmCompromiseAgreement frmCompromiseAgreement = new frmCompromiseAgreement();
                frmCompromiseAgreement.ReportTitle = m_sReportName;
                frmCompromiseAgreement.Query = m_sQuery;
                frmCompromiseAgreement.User = AppSettingsManager.SystemUser.UserCode;
                frmCompromiseAgreement.ShowDialog();
            }
        }

        private void OnReportListOfCollectiblesByBarangay()
        {
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            String sTaxAmount = "0", sFeesAmount = "0";

            String sGroup, sBin, sBnsName, sBnsAddr, sOwnCode, sOwnerName, sOwnerAddr, sBnsCode, sMainBns, sBnsStat, sLastDatePay, sTaxYear, sDueState, sQtrToPay;
            double fTaxAmt1, fTaxAmt2, fTaxAmt3, fTaxAmt4;
            double fFeesAmt1, fFeesAmt2, fFeesAmt3, fFeesAmt4;
            double fChargesAmt1, fChargesAmt2, fChargesAmt3, fChargesAmt4;
            double fTaxAmt, fFeesAmt, fChargesAmt;
            int iCount, iProgressCtr;
            String sQuery, sCurrentUser;

            try
            {
                m_sMonthlyCutoffDate = AppSettingsManager.GetConfigValue("14");
                m_sInitialDueDate = AppSettingsManager.GetConfigValue("13");

                pSet1.Query = string.Format("select * from surch_sched order by tax_fees_code");
                if (pSet1.Execute())
                    if (pSet1.Read())
                    {
                        m_dSurchRate = pSet1.GetDouble("surch_rate");//)),&cTmp);				
                        m_dPenRate = pSet1.GetDouble("pen_rate");//)),&cTmp);				
                    }
                pSet1.Close();

                iCount = 0;
                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

                pSet1.Query = string.Format("delete from rep_list_coll where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet1.ExecuteNonQuery();

                pSet1.Query = string.Format("delete from gen_info where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet1.ExecuteNonQuery();

                String sCurrentDate;
                DateTime m_vodtSaveTime;

                m_vodtSaveTime = AppSettingsManager.GetSystemDate();
                sCurrentDate = string.Format("{0}-{1}-{2}",
                            m_vodtSaveTime.Year,
                            m_vodtSaveTime.Month,
                            m_vodtSaveTime.Day);

                pSet1.Query = string.Format("insert into gen_info values('{0}',sysdate,'{1}',{2},'COL')", m_sReportName, AppSettingsManager.SystemUser.UserCode, 1);
                pSet1.ExecuteNonQuery();
                // RTL 05022006 (s)
                int iFees_cnt = 0;
                iProgressCtr = 0;

                pSet1.Query = "select count(distinct bns_code) as iFees from bns_table where fees_code like 'B%%' and length(rtrim(bns_code)) = 2 and bns_code like '" + m_sBnsCode + "'";
                if (pSet1.Execute())
                    if (pSet1.Read())
                        iFees_cnt = pSet1.GetInt("iFees");
                pSet1.Close();

                #region Progress
                int intCount = 0;
                int intCountIncreament = 0;
                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();
                Thread.Sleep(500);

                sQuery = string.Format(@"select sum(a.Cnt) from(
                                       select sum(count(distinct taxdues.bin)) as Cnt
                                       from taxdues , businesses
                                        where taxdues.bin = businesses.bin
                                         and businesses.bns_brgy like '{0}'
                                         and businesses.bns_code like '{1}%'
                                         and businesses.bns_stat like '{2}'
                                         and businesses.orgn_kind like '{3}'
                                         and businesses.bns_dist like '{4}'
                                         and businesses.bin not in (select bin from business_que)
                                         and taxdues.tax_year like '{5}' group by taxdues.bin,taxdues.tax_year,due_state,qtr_to_pay,
                                         businesses.bns_brgy,bns_code,bns_stat,orgn_kind,bns_dist,bns_nm,own_code,dt_operated
                                       union all
                                       select sum(count(distinct taxdues.bin)) as Cnt
                                       from taxdues , business_que
                                       where taxdues.bin = business_que.bin
                                         and business_que.bns_brgy like '{0}'
                                         and business_que.bns_code like '{1}%'
                                         and business_que.bns_stat like '{2}'
                                         and business_que.orgn_kind like '{3}'
                                         and business_que.bns_dist like '{4}'
                                         and taxdues.tax_year like '{5}' group by taxdues.bin,taxdues.tax_year,due_state,qtr_to_pay,
                                         business_que.bns_brgy,bns_code,bns_stat,orgn_kind,bns_dist,bns_nm,own_code,dt_operated) a", m_sBrgyNm, m_sBnsCode, m_sBnsStat, m_sOrgKind, m_sDistNm, m_sTaxYear);  // GDE 20090403 add m_sTaxYear
                pSet1.Query = sQuery;
                int.TryParse(pSet1.ExecuteScalar(), out intCount);
                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
                #endregion

                pSet1.Query = string.Format(@"select distinct(taxdues.bin),taxdues.tax_year,due_state,qtr_to_pay,
                                              businesses.bns_brgy,bns_code,bns_stat,orgn_kind,bns_dist,bns_nm,own_code,dt_operated
                                       from taxdues , businesses
                                        where taxdues.bin = businesses.bin
                                         and businesses.bns_brgy like '{0}'
                                         and businesses.bns_code like '{1}%'
                                         and businesses.bns_stat like '{2}'
                                         and businesses.orgn_kind like '{3}'
                                         and businesses.bns_dist like '{4}'
                                         and businesses.bin not in (select bin from business_que)
                                         and taxdues.tax_year like '{5}'
                                       union all
                                       select distinct(taxdues.bin),taxdues.tax_year,due_state,qtr_to_pay,
                                              business_que.bns_brgy,bns_code,bns_stat,orgn_kind,bns_dist,bns_nm,own_code,dt_operated
                                       from taxdues , business_que
                                       where taxdues.bin = business_que.bin
                                         and business_que.bns_brgy like '{0}'
                                         and business_que.bns_code like '{1}%'
                                         and business_que.bns_stat like '{2}'
                                         and business_que.orgn_kind like '{3}'
                                         and business_que.bns_dist like '{4}'
                                         and taxdues.tax_year like '{5}'", m_sBrgyNm, m_sBnsCode, m_sBnsStat, m_sOrgKind, m_sDistNm, m_sTaxYear);  // GDE 20090403 add m_sTaxYear
                if (pSet1.Execute())
                {
                    while (pSet1.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        if (m_iField == 5)
                        {
                            // RMC 20150524 corrections in reports (s)

                            m_sBnsCode = pSet1.GetString("bns_code");
                            // RMC 20150524 corrections in reports (e)

                            m_sGroup = AppSettingsManager.GetBnsDesc(m_sBnsCode.Substring(0, 2));
                        }
                        else
                            m_sGroup = pSet1.GetString(m_iField).Trim();

                        sBin = pSet1.GetString(0).Trim();

                        sBnsName = pSet1.GetString("bns_nm").Trim();
                        sOwnCode = pSet1.GetString("own_code").Trim();
                        sBnsAddr = AppSettingsManager.GetBnsAddress(sBin);
                        sOwnerName = AppSettingsManager.GetBnsOwner(sOwnCode);
                        sOwnerAddr = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                        sBnsCode = pSet1.GetString("bns_code").Trim();
                        sMainBns = AppSettingsManager.GetBnsDesc(sBnsCode);
                        sBnsStat = pSet1.GetString("bns_stat").Trim();
                        sTaxYear = pSet1.GetString(1).Trim();
                        sDueState = pSet1.GetString("due_state").Trim();
                        sQtrToPay = pSet1.GetString("qtr_to_pay").Trim();

                        if (sDueState == "N" || sDueState == "Q")
                            sBnsStat = "NEW";
                        else if (sDueState == "R")
                            sBnsStat = "REN";
                        else if (sDueState == "X")
                            sBnsStat = "RET";

                        m_sDtOperated = pSet1.GetDateTime("dt_operated").ToShortDateString();

                        pSet2.Query = string.Format("select bill_date from bill_no where bin = '{0}' and tax_year = '{1}' and qtr = '{2}'", sBin, sTaxYear, sQtrToPay);
                        if (pSet2.Execute())
                        {
                            if (pSet2.Read())
                            {
                                sLastDatePay = pSet2.GetDateTime("bill_date").ToShortDateString();
                                pSet2.Close();
                            }
                            else
                            {
                                pSet2.Close();
                                switch (Convert.ToInt16(sQtrToPay))
                                {
                                    case 2:
                                        {
                                            pSet2.Query = string.Format("select distinct(or_date) from pay_hist where bin = '{0}' and qtr_paid = '1' and tax_year = '{1}'", sBin, sTaxYear);
                                            if (pSet2.Execute())
                                                if (pSet2.Read())
                                                {
                                                    sLastDatePay = pSet2.GetDateTime(0).ToShortDateString();
                                                }
                                            pSet2.Close();
                                            break;
                                        }
                                    case 3:
                                        {
                                            pSet2.Query = string.Format("select distinct(or_date) from pay_hist where bin = '{0}' and qtr_paid = '2' and tax_year = '{1}'", sBin, sTaxYear);
                                            if (pSet2.Execute())
                                                if (pSet2.Read())
                                                {
                                                    sLastDatePay = pSet2.GetDateTime(0).ToShortDateString();
                                                }
                                            pSet2.Close();
                                            break;
                                        }
                                    case 4:
                                        {
                                            pSet2.Query = string.Format("select distinct(or_date) from pay_hist where bin = '{0}' and qtr_paid = '3' and tax_year = '{1}'", sBin, sTaxYear);
                                            if (pSet2.Execute())
                                                if (pSet2.Read())
                                                {
                                                    sLastDatePay = pSet2.GetDateTime(0).ToShortDateString();
                                                }
                                            pSet2.Close();
                                            break;
                                        }
                                }
                            }
                        }
                        fTaxAmt1 = 0;
                        fTaxAmt2 = 0;
                        fTaxAmt3 = 0;
                        fTaxAmt4 = 0;
                        fFeesAmt1 = 0;
                        fFeesAmt2 = 0;
                        fFeesAmt3 = 0;
                        fFeesAmt4 = 0;
                        fChargesAmt1 = 0;
                        fChargesAmt2 = 0;
                        fChargesAmt3 = 0;
                        fChargesAmt4 = 0;

                        if (sBin == "011-34-2015-0000002")
                        {

                        }

                        ComputeTaxAndFeesColl(sBin, sTaxYear, sQtrToPay, sDueState, fTaxAmt1, fTaxAmt2, fTaxAmt3, fTaxAmt4, fFeesAmt1, fFeesAmt2, fFeesAmt3, fFeesAmt4, fChargesAmt1, fChargesAmt2, fChargesAmt3, fChargesAmt4);

                        if (fTaxAmt1 > 0 && fTaxAmt2 > 0 && fTaxAmt3 > 0 && fTaxAmt4 > 0)
                        {
                            pSet2.Query = string.Format("select sum(amount) from taxdues where bin = '{0}' and tax_code like 'B%%' and tax_year = '{1}'", sBin, sTaxYear);
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                    sTaxAmount = pSet2.GetDouble(0).ToString();
                            pSet2.Close();
                            fTaxAmt = Convert.ToDouble(sTaxAmount);
                        }
                        else
                            // ALJ 09032003 (e)
                            fTaxAmt = fTaxAmt1 + fTaxAmt2 + fTaxAmt3 + fTaxAmt4;

                        // ALJ 09032003 (s) get the full taxdue for a year (for SOA and Collectibles/Notice be equal/tally)
                        if (fFeesAmt1 > 0 && fFeesAmt2 > 0 && fFeesAmt3 > 0 && fFeesAmt4 > 0)
                        {
                            pSet2.Query = string.Format("select sum(amount) from taxdues where bin = '{0}' and tax_code not like 'B%%' and tax_year = '{1}'", sBin, sTaxYear);
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                    sFeesAmount = pSet2.GetDouble(0).ToString();
                            pSet2.Close();
                            fFeesAmt = Convert.ToDouble(sFeesAmount);
                        }
                        else
                            // ALJ 09032003 (e)
                            fFeesAmt = fFeesAmt1 + fFeesAmt2 + fFeesAmt3 + fFeesAmt4;

                        if (fChargesAmt1 < 0)
                            fChargesAmt1 = 0;
                        if (fChargesAmt2 < 0)
                            fChargesAmt2 = 0;
                        if (fChargesAmt3 < 0)
                            fChargesAmt3 = 0;
                        if (fChargesAmt4 < 0)
                            fChargesAmt4 = 0;

                        fChargesAmt = fChargesAmt1 + fChargesAmt2 + fChargesAmt3 + fChargesAmt4;

                        DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);

                    }
                }
                pSet1.Close();

                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);
                if (m_iRadioNumber == 5) // top collectibles
                {
                    OnReportListOfTopNCollectibles();
                }
                else
                {
                    pSet1.Query = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                    pSet1.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            { }
        }

        private string FeesPen(String sTaxYear, String sTempPen, String sQtr, String m_sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            String sPen, sPenTemp;

            sPen = sTempPen;
            if (sQtr == "2")
            {
                pSet.Query = "select distinct * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '2' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sPen = "0.00";
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select distinct * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '1' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                pSet.Close();
                                pSet.Query = "select sum(fees_pen) as fees_pen from pay_temp where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and qtr = '1' and term = 'I'";
                                if (pSet.Execute())
                                    if (pSet.Read())
                                    {
                                        double fPenTemp = 0;
                                        fPenTemp = pSet.GetDouble("fees_pen");
                                        sPenTemp = fPenTemp.ToString();
                                        if (fPenTemp >= 0)
                                            sPen = "0.00";
                                    }
                                pSet.Close();
                            }
                    }
            }

            if (sQtr == "3")
            {
                pSet.Query = "select distinct * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '3' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sPen = "0.00";
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select distinct * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '2' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                pSet.Close();
                                pSet.Query = "select sum(fees_pen) as fees_pen from pay_temp where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and qtr = '2' and term = 'I'";
                                if (pSet.Execute())
                                    if (pSet.Read())
                                    {
                                        double fPenTemp = 0;
                                        fPenTemp = pSet.GetDouble("fees_pen");
                                        sPenTemp = fPenTemp.ToString();
                                        if (fPenTemp >= 0)
                                            sPen = "0.00";
                                    }
                                pSet.Close();
                            }
                    }
            }

            if (sQtr == "4")
            {
                pSet.Query = "select distinct * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '4' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sPen = "0.00";
                    else
                    {
                        pSet.Close();
                        sQuery = "select distinct * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '3' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                pSet.Close();
                                pSet.Query = "select sum(fees_pen) as fees_pen from pay_temp where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and qtr = '3' and term = 'I'";
                                if (pSet.Execute())
                                    if (pSet.Read())
                                    {
                                        double fPenTemp = 0;
                                        fPenTemp = pSet.GetDouble("fees_pen");
                                        sPenTemp = fPenTemp.ToString();
                                        if (fPenTemp >= 0)
                                            sPen = "0.00";
                                    }
                                pSet.Close();
                            }
                    }
            }
            return sPen;
        }

        private string FeesSurch(String sTaxYear, String sDueTemp, double dSurch, String sQtr, String m_sBIN, String sTaxCode, String sBnsCode)
        {
            String sQuery;
            String sSurch, sSurchTemp;
            String sLastQtrpaid;

            sSurch = Convert.ToString(Convert.ToDouble(sDueTemp) * dSurch);

            return sSurch;
        }

        private void OnReportListOfTopNCollectibles()
        {
            OracleResultSet pSet = new OracleResultSet();

            String sQuery, sBin, sCollAmt, sCollAmtTmp;
            int iRank, iProgressCtr;

            //pApp->JobStatus(0);

            if (txtTop.Text.Trim() == "0")
                txtTop.Text = "0";

            iRank = 0;
            sCollAmtTmp = "0.00";
            sQuery = string.Format("select bin, (tax_amt+fees_amt+charges_amt) a from rep_list_coll where report_name = '{0}' and user_code = '{1}' order by a desc", m_sReportName, m_sUserCode);

            //Start STATUS BAR
            iProgressCtr = 0;
            //pApp->iProgressTotal = atoi(mv_sTop);
            pSet.Query = sQuery;    // RMC 20161209 correction in top collectibles
            if (pSet.Execute())
            {
                while (pSet.Read() && (iRank <= Convert.ToInt32(txtTop.Text.Trim())))
                {
                    sBin = pSet.GetString(0).Trim();
                    sCollAmt = pSet.GetDouble(1).ToString();
                    if (sCollAmt != sCollAmtTmp)
                    {
                        sCollAmtTmp = sCollAmt;
                        iRank = iRank + 1;
                        if (iRank > Convert.ToInt32(txtTop.Text.Trim()))
                        {
                            break;
                        }
                        iProgressCtr = iProgressCtr + 1;
                    }
                    sQuery = string.Format(@"update rep_list_coll set data_group = '{0}', subgroup1 = '{1}' where bin = '{2}'
							 and report_name = '{3}' and user_code = '{4}'", txtTop.Text.Trim(), iRank, sBin, m_sReportName, m_sUserCode);

                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    //pApp->JobStatus(iProgressCtr);

                }
            }
            pSet.Close();
            //pApp->JobStatus(pApp->iProgressTotal+1);

            //OnPrintReportListOfCollectiblesByBarangay(mv_sReportName, m_sUserCode);

            m_sReportName = "LIST OF TOP " + txtTop.Text.ToString() + " COLLECTIBLES"; ;

            //(s) JJP Update Gen info
            //pSet.Query = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, m_sUserCode);
            pSet.Query = string.Format("update gen_info set switch = 0 where report_name like 'LIST OF TOP%' and user_code = '{1}'", m_sReportName, m_sUserCode);   // RMC 20161209 correction in top collectibles
            pSet.ExecuteNonQuery();
        }

        private void ComputeTaxAndFeesColl(String p_sBin, String p_sTaxYear, String p_sQtrToPay, String p_sDueState,
                                                 double p_dTaxAmt1, double p_dTaxAmt2, double p_dTaxAmt3, double p_dTaxAmt4,
                                                 double p_dFeesAmt1, double p_dFeesAmt2, double p_dFeesAmt3, double p_dFeesAmt4,
                                                 double p_dChargesAmt1, double p_dChargesAmt2, double p_dChargesAmt3, double p_dChargesAmt4)
        {

            String sQuery = "", sFeesTerm;
            String sTaxCode, sAmount, sSurch, sPen, sTax, sFees, sCharges;
            String sBnsCodeMain, sBin, sTaxYear; // ALJ 05022007 apply latest adjustments in CPayments - MANILA	
            String sContactNo = "", sNoEmp = "";
            frmPayment pPayments = new frmPayment();
            double dSurch1 = 0, dSurch2 = 0, dSurch3 = 0, dSurch4 = 0;
            double dPenRate1 = 0, dPenRate2 = 0, dPenRate3 = 0, dPenRate4 = 0;
            double dSurchQuart = 0, dPenQuart = 0;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            OracleResultSet pSet3 = new OracleResultSet();
            OracleResultSet pSet4 = new OracleResultSet();
            OracleResultSet pSet5 = new OracleResultSet();

            bool mg_boInstallment = false;

            String sInstallment = AppSettingsManager.GetConfigValue("15");
            if (sInstallment == "Y")
                mg_boInstallment = true;
            else
                mg_boInstallment = false;

            try
            {
                // put some values into CPayments
                pPayments.m_sMonthlyCutoffDate = m_sMonthlyCutoffDate;
                pPayments.m_sInitialDueDate = m_sInitialDueDate;
                pPayments.m_dPenRate = m_dPenRate;
                pPayments.m_dSurchRate = m_dSurchRate;
                pPayments.m_sDtOperated = m_sDtOperated;
                pPayments.mv_cdtORDate = AppSettingsManager.GetSystemDate();
                //

                // GDE 20090310 for getting bns info (s){
                pSet.Query = "select * from businesses where bin = '" + p_sBin + "'";
                String sBnsName = "", sBnsCode = "", sGroup = "", sBnsAddr = "", sOwnCode = "", sOwnerName = "", sOwnerAddr = "", sMainBns = "", sBnsStat = "", sLastDatePay = "", sQtrToPay = "", sCurrentUser = "";
                //String sTaxYear;
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sBin = p_sBin;

                        //if (sBin == "232-00-2013-0000079" || sBin == "232-00-2013-0000201" || sBin == "232-00-2013-0000210" || sBin == "232-00-2013-0000238" || sBin == "232-00-2013-0000265" || sBin == "232-00-2013-0000278" || sBin == "232-00-2013-0000299")
                        //    MessageBox.Show(sBin, "");
                        if (sBin == "011-34-2015-0000002")
                        { 
                            
                        }
                        sBnsCode = pSet.GetString("bns_code").Trim();
                        sBnsName = pSet.GetString("bns_nm").Trim();
                        sBnsAddr = AppSettingsManager.GetBnsAddress(p_sBin);
                        sOwnCode = pSet.GetString("own_code").Trim();
                        sOwnerName = AppSettingsManager.GetBnsOwner(sOwnCode);
                        sOwnerAddr = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                        sMainBns = AppSettingsManager.GetBnsDesc(sBnsCode);

                        sContactNo = pSet.GetString("bns_telno").Trim();
                        sNoEmp = pSet.GetInt("num_employees").ToString().Trim();

                        pSet1.Query = "select qtr_to_pay, tax_year, due_state from taxdues where bin = '" + p_sBin + "'";
                        if (pSet1.Execute())
                            if (pSet1.Read())
                            {
                                sQtrToPay = pSet1.GetString("qtr_to_pay").Trim();
                                sTaxYear = pSet1.GetString("tax_year").Trim();
                                sBnsStat = pSet1.GetString("due_state").Trim();

                                if (sBnsStat.Trim() == "R")
                                    sBnsStat = "REN";
                                else if (sBnsStat.Trim() == "N")
                                    sBnsStat = "NEW";
                                else if (sBnsStat.Trim() == "X")
                                    sBnsStat = "RET";

                                pSet2.Query = string.Format(@"select bill_date from bill_no where bin = '{0}' and tax_year = '{1}' and qtr = '{2}'", sBin, sTaxYear, sQtrToPay);
                                if (pSet2.Execute())
                                    if (pSet2.Read())
                                    {
                                        sLastDatePay = pSet2.GetDateTime("bill_date").ToShortDateString();
                                        pSet2.Close();
                                    }
                                    else
                                    {
                                        pSet2.Close();
                                        switch (Convert.ToInt16(sQtrToPay))
                                        {
                                            case 2:
                                                {
                                                    pSet2.Query = string.Format("select distinct(or_date) from pay_hist where bin = '{0}' and qtr_paid = '1' and tax_year = '{1}'", sBin, sTaxYear);
                                                    if (pSet2.Execute())
                                                        if (pSet2.Read())
                                                        {
                                                            sLastDatePay = pSet2.GetDateTime(0).ToShortDateString();
                                                        }
                                                    pSet2.Close();
                                                    break;
                                                }
                                            case 3:
                                                {
                                                    pSet2.Query = string.Format("select distinct(or_date) from pay_hist where bin = '{0}' and qtr_paid = '2' and tax_year = '{1}'", sBin, sTaxYear);
                                                    if (pSet2.Execute())
                                                        if (pSet2.Read())
                                                        {
                                                            sLastDatePay = pSet2.GetDateTime(0).ToShortDateString();
                                                        }
                                                    pSet2.Close();
                                                    break;
                                                }
                                            case 4:
                                                {
                                                    pSet2.Query = string.Format("select distinct(or_date) from pay_hist where bin = '{0}' and qtr_paid = '3' and tax_year = '{1}'", sBin, sTaxYear);
                                                    if (pSet2.Execute())
                                                        if (pSet2.Read())
                                                        {
                                                            sLastDatePay = pSet2.GetDateTime(0).ToShortDateString();
                                                        }
                                                    pSet2.Close();
                                                    break;
                                                }
                                        }
                                    }

                            }
                        pSet1.Close(); // GDE 20090310
                    }
                    // GDE 20090310 buss que
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select * from business_que where bin = '" + p_sBin + "'";

                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                sBin = p_sBin;

                                sBnsCode = pSet.GetString("bns_code").Trim();
                                sBnsName = pSet.GetString("bns_nm").Trim();
                                sBnsAddr = AppSettingsManager.GetBnsAddress(p_sBin);
                                sOwnCode = pSet.GetString("own_code").Trim();
                                sOwnerName = AppSettingsManager.GetBnsOwner(sOwnCode);
                                sOwnerAddr = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                                sMainBns = AppSettingsManager.GetBnsDesc(sBnsCode);

                                pSet1.Query = "select qtr_to_pay, tax_year, due_state from taxdues where bin = '" + p_sBin + "'";
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                    {
                                        sQtrToPay = pSet1.GetString("qtr_to_pay").Trim();
                                        sTaxYear = pSet1.GetString("tax_year").Trim();
                                        sBnsStat = pSet1.GetString("due_state").Trim();

                                        if (sBnsStat.Trim() == "R")
                                            sBnsStat = "REN";
                                        else if (sBnsStat.Trim() == "N")
                                            sBnsStat = "NEW";
                                        else if (sBnsStat.Trim() == "X")
                                            sBnsStat = "RET";

                                        pSet2.Query = string.Format("select bill_date from bill_no where bin = '{0}' and tax_year = '{1}' and qtr = '{2}'", sBin, sTaxYear, sQtrToPay);
                                        if (pSet2.Execute())
                                            if (pSet2.Read())
                                            {
                                                sLastDatePay = pSet2.GetDateTime("bill_date").ToShortDateString();
                                                pSet2.Close();
                                            }
                                            else
                                            {
                                                pSet2.Close();
                                                switch (Convert.ToInt16(sQtrToPay))
                                                {
                                                    case 2:
                                                        {
                                                            pSet2.Query = string.Format("select distinct(or_date) from pay_hist where bin = '{0}' and qtr_paid = '1' and tax_year = '{1}'", sBin, sTaxYear);
                                                            if (pSet2.Execute())
                                                                if (pSet2.Read())
                                                                {
                                                                    sLastDatePay = pSet2.GetDateTime(0).ToShortDateString();
                                                                }
                                                            pSet2.Close();
                                                            break;
                                                        }
                                                    case 3:
                                                        {
                                                            pSet2.Query = string.Format("select distinct(or_date) from pay_hist where bin = '{0}' and qtr_paid = '2' and tax_year = '{1}'", sBin, sTaxYear);
                                                            if (pSet2.Execute())
                                                                if (pSet2.Read())
                                                                {
                                                                    sLastDatePay = pSet2.GetDateTime(0).ToShortDateString();
                                                                }
                                                            pSet2.Close();
                                                            break;
                                                        }
                                                    case 4:
                                                        {
                                                            pSet2.Query = string.Format("select distinct(or_date) from pay_hist where bin = '{0}' and qtr_paid = '3' and tax_year = '{1}'", sBin, sTaxYear);
                                                            if (pSet2.Execute())
                                                                if (pSet2.Read())
                                                                {
                                                                    sLastDatePay = pSet2.GetDateTime(0).ToShortDateString();
                                                                }
                                                            pSet2.Close();
                                                            break;
                                                        }
                                                }
                                            }

                                    }
                                pSet1.Close(); // GDE 20090310

                            }
                    }
                // GDE 20090310 buss que
                pSet.Close();
                // GDE 20090310 for getting bns info (e)}

                sTax = "0";
                sFees = "0";
                sCharges = "0";

                // GDE 20090302 set negative values to zero (s){
                if (m_dPenRate < 0)
                    m_dPenRate = 0;
                // GDE 20090302 set negative values to zero (e)}

                pSet.Query = string.Format("select * from taxdues where bin = '{0}' and tax_year = '{1}' and qtr_to_pay = '{2}' order by tax_year,qtr_to_pay", p_sBin, p_sTaxYear, p_sQtrToPay); // GDE try pay_temp
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        // ALJ 05022007 (s) apply latest adjustments in CPayments - MANILA	
                        sBin = pSet.GetString("bin").Trim();
                        sBnsCodeMain = pSet.GetString("bns_code_main").Trim(); // GDE 20090309
                        sTaxYear = pSet.GetString("tax_year").Trim();
                        // ALJ 05022007 (e) apply latest adjustments in CPayments - MANILA	
                        sTaxCode = pSet.GetString("tax_code").Trim(); // GDE 20090309
                        sAmount = pSet.GetDouble("amount").ToString(); // GDE 20090309

                        if (sBin == "011-34-2015-0000014")
                        {
                            
                        }

                        //if (sBin == "232-00-2013-0000079")
                        //    MessageBox.Show(sBin, "");

                        //pPayments.m_sBIN = sBin; // GDE 20090429
                        ComputePenRate(sBin, p_sTaxYear, p_sQtrToPay, p_sDueState, dSurch1, dSurch2, dSurch3, dSurch4, dPenRate1, dPenRate2, dPenRate3, dPenRate4, dSurchQuart, dPenQuart);
                        dSurch1 = mp_dSurchRate1;
                        dSurch2 = mp_dSurchRate2;
                        dSurch3 = mp_dSurchRate3;
                        dSurch4 = mp_dSurchRate4;
                        dPenRate1 = mp_dPenRate1;
                        dPenRate2 = mp_dPenRate2;
                        dPenRate3 = mp_dPenRate3;
                        dPenRate4 = mp_dPenRate4;
                        dSurchQuart = mp_SurchQuart;
                        dPenQuart = mp_PenQuart;
                        //pPayments.ComputeTaxAndFees(sBin);

                        sQtrToPay = pSet.GetString("qtr_to_pay").Trim(); // GDE 20110915 test lng
                        sFeesTerm = pPayments.FeesTerm(sTaxCode);

                        if (pPayments.FeesWithPen(sTaxCode) == "N")
                        {
                            dPenRate1 = 0;
                            dPenRate2 = 0;
                            dPenRate3 = 0;
                            dPenRate4 = 0;
                        }
                        if (pPayments.FeesWithSurch(sTaxCode) == "N")
                        {
                            dSurch1 = 0;
                            dSurch2 = 0;
                            dSurch3 = 0;
                            dSurch4 = 0;
                        }

                        String sOrigBTaxAmount, sOrigFeeAmount, sTempAmount;
                        String sTmpSurch1, sTmpSurch2, sTmpSurch3, sTmpSurch4;
                        String sTmpPen1, sTmpPen2, sTmpPen3, sTmpPen4;

                        if (p_sDueState == "R" || p_sDueState == "X" || mg_boInstallment)
                        {
                            if ((sTaxCode.Substring(0, 1) == "B" && pPayments.IsTaxFull(sBnsCodeMain) == false) || (sFeesTerm == "Q" || pPayments.FeeQtrlyCheck(sBin, sTaxYear, sTaxCode, sBnsCodeMain) == true)) // ALJ 05022007 apply latest adjustments in CPayments - MANILA	
                            {
                                //if (sBin == "232-00-2013-0000079")
                                //    MessageBox.Show(sBin, "");

                                sOrigBTaxAmount = sAmount;
                                sAmount = (Convert.ToDouble(sAmount) / 4).ToString();
                            }
                            else
                            {
                                //sAmount = sAmount;
                                sOrigBTaxAmount = sAmount;
                            }

                            double fSurch = 0, fPen = 0;
                            if (Convert.ToDouble(p_sQtrToPay) <= 1)
                            {
                                sFees = "0.00";
                                //if (sTaxCode.Substring(0, 1) == "B" || sTaxCode.Substring(0, 2) == "01" || sTaxCode.Substring(0, 2) == "02") 
                                if (sTaxCode.Substring(0, 1) == "B") // AST 20160215 put Bns Tax only
                                {
                                    sSurch = (Convert.ToDouble(sOrigBTaxAmount) * dSurch1).ToString("0.##");
                                    sTmpSurch1 = sSurch; // GDE 20081008 added
                                    sPen = ((Convert.ToDouble(sOrigBTaxAmount) + Convert.ToDouble(sSurch)) * dPenRate1).ToString("0.##");
                                    sTmpPen1 = sPen;
                                    sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                    sTax = sOrigBTaxAmount;
                                    p_dChargesAmt1 += Convert.ToDouble(sCharges);
                                    p_dTaxAmt1 += Convert.ToDouble(sTax);

                                    /// try lang
                                    // GDE 20090310 insert in rep_list_coll (s){
                                    sQtrToPay = "1";
                                    //sTax = Convert.ToString(Convert.ToDouble(sTax) / 4); //AFM 20210120 MAO-21-14478 removed

                                    pSet.Query = string.Format(@"insert into rep_list_coll values ('{0}','{1}',NULL,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                    AppSettingsManager.GetConfigValue("09").Trim(),
                                    StringUtilities.Left(m_sGroup, 30),
                                    sBin,
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsName), 60),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsAddr), 100),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerName), 60),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerAddr), 100),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sMainBns), 30), sBnsStat,
                                    sLastDatePay, sTaxYear, sQtrToPay,
                                    Convert.ToDouble(sTax).ToString("0.00"),
                                    Convert.ToDouble(sFees).ToString("0.00"),
                                    Convert.ToDouble(sCharges).ToString("0.00"),
                                    StringUtilities.Left(m_sReportName, 100),
                                    AppSettingsManager.SystemUser.UserCode,
                                    sCurrentUser, sContactNo, sNoEmp); // ALJ 12052003 fixed SQL
                                    pSet.ExecuteNonQuery();
                                }
                                else
                                {
                                    sTax = "0.00"; // GDE 20090310 try
                                    sSurch = (Convert.ToDouble(sAmount) * dSurch1).ToString("0.##");
                                    sPen = ((Convert.ToDouble(sAmount) + Convert.ToDouble(sSurch)) * dPenRate1).ToString("0.##");
                                    //sCharges = (Convert.ToDouble((sSurch) + Convert.ToDouble(sPen))).ToString("0.##");
                                    sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##"); //AFM 20210120 MAO-21-14478	
                                    sFees = sOrigBTaxAmount;
                                    p_dChargesAmt1 += Convert.ToDouble(sCharges);
                                    p_dFeesAmt1 += Convert.ToDouble(sFees);

                                    sQtrToPay = "1";

                                    pSet.Query = string.Format(@"insert into rep_list_coll values ('{0}','{1}',NULL,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                    AppSettingsManager.GetConfigValue("09").Trim(),
                                    StringUtilities.Left(m_sGroup, 30),
                                    sBin,
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsName), 60),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsAddr), 100),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerName), 60),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerAddr), 100),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sMainBns), 30), sBnsStat,
                                    sLastDatePay, sTaxYear, sQtrToPay,
                                    Convert.ToDouble(sTax).ToString("0.00"),
                                    Convert.ToDouble(sFees).ToString("0.00"),
                                    Convert.ToDouble(sCharges).ToString("0.00"),
                                    StringUtilities.Left(m_sReportName, 100),
                                    AppSettingsManager.SystemUser.UserCode,
                                    sCurrentUser, sContactNo, sNoEmp); // ALJ 12052003 fixed SQL
                                    pSet.ExecuteNonQuery();
                                }
                            }

                            //if (Convert.ToDouble(p_sQtrToPay) <= 2)
                            else if (Convert.ToDouble(p_sQtrToPay) <= 2)
                            {
                                //if (sTaxCode.Substring(0, 1) == "B" || sTaxCode.Substring(0, 2) == "01" || sTaxCode.Substring(0, 2) == "02")
                                if (sTaxCode.Substring(0, 1) == "B") // AST 20160215 put Bns Tax only
                                {
                                    sFees = "0.00"; // GDE 20090310 try lang
                                    sTempAmount = (Convert.ToDouble(sOrigBTaxAmount) / 4).ToString();
                                    sSurch = (Convert.ToDouble(sTempAmount) * dSurch2).ToString("0.##");
                                    sPen = ((Convert.ToDouble(sAmount) + Convert.ToDouble(sSurch)) * dPenRate2).ToString("0.##"); // GDE 20081008
                                    sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                    sTax = sAmount;
                                    p_dChargesAmt2 += Convert.ToDouble(sCharges);
                                    p_dTaxAmt2 += Convert.ToDouble(sTax);

                                    sQtrToPay = "2";
                                    pSet.Query = string.Format(@"insert into rep_list_coll values ('{0}','{1}',NULL,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                    AppSettingsManager.GetConfigValue("09").Trim(),
                                    StringUtilities.Left(m_sGroup, 30),
                                    sBin,
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsName), 60),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsAddr), 100),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerName), 60),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerAddr), 100),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sMainBns), 30), sBnsStat,
                                    sLastDatePay, sTaxYear, sQtrToPay,
                                    Convert.ToDouble(sTax).ToString("0.00"),
                                    Convert.ToDouble(sFees).ToString("0.00"),
                                    Convert.ToDouble(sCharges).ToString("0.00"),
                                    StringUtilities.Left(m_sReportName, 100),
                                    AppSettingsManager.SystemUser.UserCode,
                                    sCurrentUser, sContactNo, sNoEmp); // ALJ 12052003 fixed SQL
                                    pSet.ExecuteNonQuery();
                                }
                                else
                                {
                                    sTax = "0.00";
                                    if (sFeesTerm == "Q" || pPayments.FeeQtrlyCheck(sBin, sTaxYear, sTaxCode, sBnsCodeMain) == true) // ALJ 05022007 apply latest adjustments in CPayments - MANILA	
                                    {
                                        sTempAmount = (Convert.ToDouble(sOrigBTaxAmount) / 4).ToString("0.##");
                                        sSurch = (Convert.ToDouble(sTempAmount) * dSurch2).ToString("0.##");
                                        sPen = ((Convert.ToDouble(sAmount) + Convert.ToDouble(sSurch)) * dPenRate2).ToString("0.##");
                                        sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                        sFees = sTempAmount;
                                        p_dChargesAmt2 += Convert.ToDouble(sCharges);
                                        p_dFeesAmt2 += Convert.ToDouble(sFees);

                                        // GDE 20090310 insert in rep_list_coll (s){
                                        sQtrToPay = "2";
                                        pSet.Query = string.Format(@"insert into rep_list_coll values ('{0}','{1}',NULL,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                        AppSettingsManager.GetConfigValue("09").Trim(),
                                        StringUtilities.Left(m_sGroup, 30),
                                        sBin,
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsName), 60),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsAddr), 100),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerName), 60),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerAddr), 100),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sMainBns), 30), sBnsStat,
                                        sLastDatePay, sTaxYear, sQtrToPay,
                                        Convert.ToDouble(sTax).ToString("0.00"),
                                        Convert.ToDouble(sFees).ToString("0.00"),
                                        Convert.ToDouble(sCharges).ToString("0.00"),
                                        StringUtilities.Left(m_sReportName, 100),
                                        AppSettingsManager.SystemUser.UserCode,
                                        sCurrentUser, sContactNo, sNoEmp); // ALJ 12052003 fixed SQL
                                        pSet.ExecuteNonQuery();
                                    }
                                }

                            }
                            //if (Convert.ToDouble(p_sQtrToPay) <= 3)
                            else if (Convert.ToDouble(p_sQtrToPay) <= 3)
                            {
                                sFees = "0.00";
                                //if (sTaxCode.Substring(0, 1) == "B" || sTaxCode.Substring(0, 2) == "01" || sTaxCode.Substring(0, 2) == "02")
                                if (sTaxCode.Substring(0, 1) == "B") // AST 20160215 put Bns Tax only
                                {
                                    // GDE 20081008
                                    sTempAmount = (Convert.ToDouble(sOrigBTaxAmount) / 4).ToString("0.##");
                                    sSurch = (Convert.ToDouble(sTempAmount) * dSurch3).ToString("0.##");
                                    sPen = ((Convert.ToDouble(sAmount) + Convert.ToDouble(sSurch)) * dPenRate3).ToString("0.##"); // GDE 20081009 re-code
                                    sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                    sTax = sAmount; // GDE 20081009 re-code
                                    p_dChargesAmt3 += Convert.ToDouble(sCharges);
                                    p_dTaxAmt3 += Convert.ToDouble(sTax);

                                    // GDE 20090310 insert in rep_list_coll (s){
                                    sQtrToPay = "3";
                                    pSet.Query = string.Format(@"insert into rep_list_coll values ('{0}','{1}',NULL,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                    AppSettingsManager.GetConfigValue("09").Trim(),
                                    StringUtilities.Left(m_sGroup, 30),
                                    sBin,
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsName), 60),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsAddr), 100),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerName), 60),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerAddr), 100),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sMainBns), 30), sBnsStat,
                                    sLastDatePay, sTaxYear, sQtrToPay,
                                    Convert.ToDouble(sTax).ToString("0.00"),
                                    Convert.ToDouble(sFees).ToString("0.00"),
                                    Convert.ToDouble(sCharges).ToString("0.00"),
                                    StringUtilities.Left(m_sReportName, 100),
                                    AppSettingsManager.SystemUser.UserCode,
                                    sCurrentUser, sContactNo, sNoEmp); // ALJ 12052003 fixed SQL
                                    pSet.ExecuteNonQuery();

                                }
                                else
                                {
                                    sTax = "0.00";
                                    if (sFeesTerm == "Q" || pPayments.FeeQtrlyCheck(sBin, sTaxYear, sTaxCode, sBnsCodeMain) == true) // ALJ 05022007 apply latest adjustments in CPayments - MANILA	
                                    {
                                        sSurch = (Convert.ToDouble(sAmount) * dSurch3).ToString("0.##");
                                        sPen = ((Convert.ToDouble(sAmount) + Convert.ToDouble(sSurch)) * dPenRate3).ToString("0.##");
                                        sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                        sFees = sAmount;
                                        p_dChargesAmt3 = Convert.ToDouble(sCharges) + p_dChargesAmt3;
                                        p_dFeesAmt3 = Convert.ToDouble(sFees) + p_dFeesAmt3;

                                        // GDE 20090310 insert in rep_list_coll (s){
                                        sQtrToPay = "3";
                                        pSet.Query = string.Format(@"insert into rep_list_coll values ('{0}','{1}',NULL,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                        AppSettingsManager.GetConfigValue("09").Trim(),
                                        StringUtilities.Left(m_sGroup, 30),
                                        sBin,
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsName), 60),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsAddr), 100),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerName), 60),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerAddr), 100),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sMainBns), 30), sBnsStat,
                                        sLastDatePay, sTaxYear, sQtrToPay,
                                        Convert.ToDouble(sTax).ToString("0.00"),
                                        Convert.ToDouble(sFees).ToString("0.00"),
                                        Convert.ToDouble(sCharges).ToString("0.00"),
                                        StringUtilities.Left(m_sReportName, 100),
                                        AppSettingsManager.SystemUser.UserCode,
                                        sCurrentUser, sContactNo, sNoEmp); // ALJ 12052003 fixed SQL
                                        pSet.ExecuteNonQuery();
                                    }
                                }


                            }
                            //if (Convert.ToDouble(p_sQtrToPay) <= 4)
                            else if (Convert.ToDouble(p_sQtrToPay) <= 4)
                            {
                                sFees = "0.00";
                                //if (sTaxCode.Substring(0, 1) == "B" || sTaxCode.Substring(0, 2) == "01" || sTaxCode.Substring(0, 2) == "02")
                                if (sTaxCode.Substring(0, 1) == "B") // AST 20160215 put Bns Tax only
                                {
                                    // GDE 20081008
                                    sTempAmount = (Convert.ToDouble(sOrigBTaxAmount) / 4).ToString("0.##");
                                    sSurch = (Convert.ToDouble(sTempAmount) * dSurch4).ToString("0.##");
                                    sPen = ((Convert.ToDouble(sAmount) + Convert.ToDouble(sSurch)) * dPenRate4).ToString("0.##");
                                    sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                    sTax = sAmount;
                                    p_dChargesAmt4 += Convert.ToDouble(sCharges);// +p_dChargesAmt4;
                                    p_dTaxAmt4 += Convert.ToDouble(sTax);

                                    // GDE 20090310 insert in rep_list_coll (s){
                                    sQtrToPay = "4";
                                    pSet.Query = string.Format(@"insert into rep_list_coll values ('{0}','{1}',NULL,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                    AppSettingsManager.GetConfigValue("09").Trim(),
                                    StringUtilities.Left(m_sGroup, 30),
                                    sBin,
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsName), 60),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsAddr), 100),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerName), 60),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerAddr), 100),
                                    StringUtilities.Left(StringUtilities.HandleApostrophe(sMainBns), 30), sBnsStat,
                                    sLastDatePay, sTaxYear, sQtrToPay,
                                    Convert.ToDouble(sTax).ToString("0.00"),
                                    Convert.ToDouble(sFees).ToString("0.00"),
                                    Convert.ToDouble(sCharges).ToString("0.00"),
                                    StringUtilities.Left(m_sReportName, 100),
                                    AppSettingsManager.SystemUser.UserCode,
                                    sCurrentUser, sContactNo, sNoEmp); // ALJ 12052003 fixed SQL
                                    pSet.ExecuteNonQuery();
                                }
                                else
                                {
                                    sTax = "0.00";
                                    if (sFeesTerm == "Q" || pPayments.FeeQtrlyCheck(sBin, sTaxYear, sTaxCode, sBnsCodeMain) == true) // ALJ 05022007 apply latest adjustments in CPayments - MANILA	
                                    {
                                        sSurch = (Convert.ToDouble(sAmount) * dSurch4).ToString("0.##");
                                        sPen = (Convert.ToDouble((sAmount) + Convert.ToDouble(sSurch)) * dPenRate4).ToString("0.##");
                                        sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                        sFees = sAmount;
                                        p_dChargesAmt4 = Convert.ToDouble(sCharges) + p_dChargesAmt4;
                                        p_dFeesAmt4 = Convert.ToDouble(sFees) + p_dFeesAmt4;
                                        // GDE 20090310 insert in rep_list_coll (s){

                                        sQtrToPay = "4";
                                        pSet.Query = string.Format(@"insert into rep_list_coll values ('{0}','{1}',NULL,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                        AppSettingsManager.GetConfigValue("09").Trim(),
                                        StringUtilities.Left(m_sGroup, 30),
                                        sBin,
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsName), 60),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsAddr), 100),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerName), 60),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerAddr), 100),
                                        StringUtilities.Left(StringUtilities.HandleApostrophe(sMainBns), 30), sBnsStat,
                                        sLastDatePay, sTaxYear, sQtrToPay,
                                        Convert.ToDouble(sTax).ToString("0.00"),
                                        Convert.ToDouble(sFees).ToString("0.00"),
                                        Convert.ToDouble(sCharges).ToString("0.00"),
                                        StringUtilities.Left(m_sReportName, 100),
                                        AppSettingsManager.SystemUser.UserCode,
                                        sCurrentUser, sContactNo, sNoEmp); // ALJ 12052003 fixed SQL
                                        pSet.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        //
                        if (p_sDueState == "N" && !mg_boInstallment) //15 installment 
                        {
                            if (sTaxCode.Substring(0, 1) == "B")
                            {
                                sSurch = (Convert.ToDouble(sAmount) * dSurch1).ToString("0.##");
                                sPen = ((Convert.ToDouble(sAmount) + Convert.ToDouble(sSurch)) * dPenRate1).ToString("0.##");
                                sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen) + Convert.ToDouble(sCharges)).ToString("0.##");
                                sTax = (Convert.ToDouble(sAmount) + Convert.ToDouble(sTax)).ToString("0.##");
                                switch (Convert.ToInt16(p_sQtrToPay))
                                {
                                    case 1:
                                        {
                                            sCharges = Convert.ToString(Convert.ToDouble(sSurch) + Convert.ToDouble(sPen));
                                            sTax = sAmount;
                                            p_dChargesAmt1 += Convert.ToDouble(sCharges);
                                            p_dTaxAmt1 += Convert.ToDouble(sTax);

                                            break;
                                        }
                                    case 2:
                                        {
                                            sCharges = Convert.ToString(Convert.ToDouble(sSurch) + Convert.ToDouble(sPen));
                                            sTax = sAmount;
                                            p_dChargesAmt2 += Convert.ToDouble(sCharges);
                                            p_dTaxAmt2 += Convert.ToDouble(sTax);

                                            break;
                                        }
                                    case 3:
                                        {
                                            sCharges = Convert.ToString(Convert.ToDouble(sSurch) + Convert.ToDouble(sPen));
                                            sTax = sAmount;
                                            p_dChargesAmt3 += Convert.ToDouble(sCharges);
                                            p_dTaxAmt3 += Convert.ToDouble(sTax);

                                            break;
                                        }
                                    case 4:
                                        {
                                            sCharges = Convert.ToString(Convert.ToDouble(sSurch) + Convert.ToDouble(sPen));
                                            sTax = sAmount;
                                            p_dChargesAmt4 += Convert.ToDouble(sCharges);
                                            p_dTaxAmt4 += Convert.ToDouble(sTax);

                                            break;
                                        }
                                }

                                sFees = "0.00";
                                sQtrToPay = "F";
                                pSet.Query = string.Format(@"insert into rep_list_coll values ('{0}','{1}',NULL,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                AppSettingsManager.GetConfigValue("09").Trim(),
                                StringUtilities.Left(m_sGroup, 30),
                                sBin,
                                StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsName), 60),
                                StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsAddr), 100),
                                StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerName), 60),
                                StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerAddr), 100),
                                StringUtilities.Left(StringUtilities.HandleApostrophe(sMainBns), 30), sBnsStat,
                                sLastDatePay, sTaxYear, sQtrToPay,
                                Convert.ToDouble(sTax).ToString("0.00"),
                                Convert.ToDouble(sFees).ToString("0.00"),
                                Convert.ToDouble(sCharges).ToString("0.00"),
                                StringUtilities.Left(m_sReportName, 100),
                                AppSettingsManager.SystemUser.UserCode,
                                sCurrentUser, sContactNo, sNoEmp); // ALJ 12052003 fixed SQL
                                pSet.ExecuteNonQuery();
                                // GDE 20090310 insert in rep_list_coll (e)}

                            }
                            else
                            {
                                sSurch = (Convert.ToDouble(sAmount) * dSurch1).ToString("0.##");
                                sPen = ((Convert.ToDouble(sAmount) + Convert.ToDouble(sSurch)) * dPenRate1).ToString("0.##");

                                switch (Convert.ToInt16(p_sQtrToPay))
                                {
                                    case 1:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sFees = sAmount;
                                            p_dChargesAmt1 += Convert.ToDouble(sCharges);
                                            p_dFeesAmt1 += Convert.ToDouble(sFees);

                                            break;
                                        }
                                    case 2:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sFees = sAmount;
                                            p_dChargesAmt2 += Convert.ToDouble(sCharges);
                                            p_dFeesAmt2 += Convert.ToDouble(sFees);

                                            break;
                                        }
                                    case 3:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sFees = sAmount;
                                            p_dChargesAmt3 += Convert.ToDouble(sCharges);
                                            p_dFeesAmt3 += Convert.ToDouble(sFees);

                                            break;
                                        }
                                    case 4:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sFees = sAmount;
                                            p_dChargesAmt4 += Convert.ToDouble(sCharges);
                                            p_dFeesAmt4 += Convert.ToDouble(sFees);

                                            break;
                                        }
                                }
                                // GDE 20090310 insert in rep_list_coll (s){	
                                sTax = "0.00";
                                sQtrToPay = "F";
                                pSet.Query = string.Format(@"insert into rep_list_coll values ('{0}','{1}',NULL,'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                AppSettingsManager.GetConfigValue("09").Trim(),
                                StringUtilities.Left(m_sGroup, 30),
                                sBin,
                                StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsName), 60),
                                StringUtilities.Left(StringUtilities.HandleApostrophe(sBnsAddr), 100),
                                StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerName), 60),
                                StringUtilities.Left(StringUtilities.HandleApostrophe(sOwnerAddr), 100),
                                StringUtilities.Left(StringUtilities.HandleApostrophe(sMainBns), 30), sBnsStat,
                                sLastDatePay, sTaxYear, sQtrToPay,
                                Convert.ToDouble(sTax).ToString("0.00"),
                                Convert.ToDouble(sFees).ToString("0.00"),
                                Convert.ToDouble(sCharges).ToString("0.00"),
                                StringUtilities.Left(m_sReportName, 100),
                                AppSettingsManager.SystemUser.UserCode,
                                sCurrentUser, sContactNo, sNoEmp); // ALJ 12052003 fixed SQL
                                pSet.ExecuteNonQuery();
                                // GDE 20090310 insert in rep_list_coll (e)}
                            }

                        }
                        if (p_sDueState == "Q")
                        {
                            if (sTaxCode.Substring(0, 1) == "B" || sTaxCode.Substring(0, 2) == "01" || sTaxCode.Substring(0, 2) == "02")                            
                            {
                                sSurch = (Convert.ToDouble(sAmount) * dSurchQuart).ToString("0.##");
                                sPen = ((Convert.ToDouble(sAmount) + Convert.ToDouble(sSurch)) * dPenQuart).ToString("0.##");

                                switch (Convert.ToInt16(p_sQtrToPay))
                                {
                                    case 1:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sTax = sAmount;
                                            p_dChargesAmt1 += Convert.ToDouble(sCharges);
                                            p_dTaxAmt1 += Convert.ToDouble(sTax);
                                            break;
                                        }
                                    case 2:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sTax = sAmount;
                                            p_dChargesAmt2 += Convert.ToDouble(sCharges);
                                            p_dTaxAmt2 += Convert.ToDouble(sTax);
                                            break;
                                        }
                                    case 3:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sTax = sAmount;
                                            p_dChargesAmt3 += Convert.ToDouble(sCharges);
                                            p_dTaxAmt3 += Convert.ToDouble(sTax);
                                            break;
                                        }
                                    case 4:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sTax = sAmount;
                                            p_dChargesAmt4 += Convert.ToDouble(sCharges);
                                            p_dTaxAmt4 += Convert.ToDouble(sTax);
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                sSurch = (Convert.ToDouble(sAmount) * dSurchQuart).ToString("0.##");
                                sPen = ((Convert.ToDouble(sAmount) + Convert.ToDouble(sSurch)) * dPenQuart).ToString("0.##");

                                switch (Convert.ToInt16(p_sQtrToPay))
                                {
                                    case 1:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sFees = sAmount;
                                            p_dChargesAmt1 += Convert.ToDouble(sCharges);
                                            p_dFeesAmt1 += Convert.ToDouble(sFees);
                                            break;
                                        }
                                    case 2:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sFees = sAmount;
                                            p_dChargesAmt2 += Convert.ToDouble(sCharges);
                                            p_dFeesAmt2 += Convert.ToDouble(sFees);
                                            break;
                                        }
                                    case 3:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sFees = sAmount;
                                            p_dChargesAmt3 += Convert.ToDouble(sCharges);
                                            p_dFeesAmt3 += Convert.ToDouble(sFees);
                                            break;
                                        }
                                    case 4:
                                        {
                                            sCharges = (Convert.ToDouble(sSurch) + Convert.ToDouble(sPen)).ToString("0.##");
                                            sFees = sAmount;
                                            p_dChargesAmt4 += Convert.ToDouble(sCharges);
                                            p_dFeesAmt4 += Convert.ToDouble(sFees);
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }
                pSet.Close();
            }
            catch { return; }
        }

        private void ComputePenRate(String m_sBIN, String sTaxYear, String sQtr, String sDueState, double p_dSurchRate1, double p_dSurchRate2, double p_dSurchRate3, double p_dSurchRate4, double p_dPenRate1, double p_dPenRate2, double p_dPenRate3, double p_dPenRate4, double p_SurchQuart, double p_PenQuart)
        {
            OracleResultSet pSet = new OracleResultSet();

            String m_sNewWithQtr = "";
            String sQuery, m_sORDate, m_sCurrentTaxYear;
            DateTime mv_cdtORDate = AppSettingsManager.GetSystemDate();
            String sJan, sFeb, sMar, sApr, sMay, sJun, sJul, sAug, sSep, sOct, sNov, sDec;
            DateTime Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec;
            double dSurchRate1, dSurchRate2, dSurchRate3, dSurchRate4;
            double dPenRate1, dPenRate2, dPenRate3, dPenRate4;
            double SurchQuart, PenQuart;
            DateTime cdtDtOperated, cdtDtO1st, cdtDtO2nd, cdtDtO3rd, cdtDtO4th;
            DateTime cdtQtrDate = new DateTime();
            String sDtOperatedYear;
            dSurchRate1 = 0;
            dSurchRate2 = 0;
            dSurchRate3 = 0;
            dSurchRate4 = 0;
            dPenRate1 = 0;
            dPenRate2 = 0;
            dPenRate3 = 0;
            dPenRate4 = 0;
            SurchQuart = 0;
            PenQuart = 0;

            // JJP 10182005 RETIREMENT PENALTY (s)
            String sMaxYear = "";
            bool boRetStatus = false;
            pSet.Query = "select max(tax_year) as max_yr from pay_hist where bin ='" + m_sBIN + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sMaxYear = pSet.GetString("max_yr");
            pSet.Close();

            if (sDueState == "X")
            {
                pSet.Query = "select * from retired_bns_temp where bin ='" + m_sBIN + "' and tax_year = '" + sMaxYear + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        boRetStatus = true;
                pSet.Close();
            }
            // JJP 10182005 RETIREMENT PENALTY (e)
            //(s) JJP 11062004 DUE DATES MANILA
            String sDueJan = "", sDueFeb = "", sDueMar = "", sDueApr = "", sDueMay = "", sDueJun = "", sDueJul = "", sDueAug = "", sDueSep = "", sDueOct = "", sDueNov = "", sDueDec = "";

            sQuery = string.Format("select * from due_dates where due_year = '{0}'", AppSettingsManager.GetSystemDate().Year); // ALJ 02112005 pApp->m_sCurrentYear ask JJP 
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    String sCode = pSet.GetString("due_code");
                    if (sCode == "01")
                        sDueJan = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "02")
                        sDueFeb = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "03")
                        sDueMar = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "04")
                        sDueApr = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "05")
                        sDueMay = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "06")
                        sDueJun = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "07")
                        sDueJul = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "08")
                        sDueAug = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "09")
                        sDueSep = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "10")
                        sDueOct = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "11")
                        sDueNov = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                    else if (sCode == "12")
                        sDueDec = pSet.GetDateTime("due_date").ToString("MM/dd/yyyy");
                }
            pSet.Close();

            //(e) JJP 11062004 DUE DATES MANILA
            if (sDueState == "N")
            {
                //string.Format("select * from  config where code like '15'");
                pSet.Query = "select * from  config where code = '15'";
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        m_sNewWithQtr = pSet.GetString("object");
                    }
                pSet.Close();

                if (m_sNewWithQtr == "N")
                {
                    //GetDlgItem(IDC_INSTALLMENT)->EnableWindow(0); // ALJ 03172003 temporarily put remarks (bec of the collectibles report
                }
            }

            // GDE 20081015 for checking if the bns has delq qtr (s){
            String sQtrPaid = "";
            pSet.Query = "select distinct * from pay_hist where bin = '" + m_sBIN + "' order by tax_year desc, qtr_paid desc";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sQtrPaid = pSet.GetString("qtr_paid");
                    if (sQtrPaid == "F")
                        sQtrPaid = "4";
                }
            pSet.Close();
            // GDE 20081015 for checking if the bns has delq qtr (e)}

            m_sORDate = string.Format("{0}/{1}/{2}", mv_cdtORDate.Month, mv_cdtORDate.Day, mv_cdtORDate.Year);
            m_sCurrentTaxYear = string.Format("{0}", mv_cdtORDate.Year);
            cdtDtOperated = Convert.ToDateTime(m_sDtOperated);
            sDtOperatedYear = sTaxYear; //LEO 02112003

            String sDtO1st, sDtO2nd, sDtO3rd, sDtO4th;
            sDtO1st = sDueJan.Substring(0, 6) + sDtOperatedYear;

            //(s)//LEO 02022003 End of Day
            String sEndingDay;
            sEndingDay = m_sMonthlyCutoffDate;
            if (Convert.ToInt32(m_sMonthlyCutoffDate) > 30)
                sEndingDay = "30";
            //(e)//LEO 02022003

            sDtO2nd = sDueApr.Substring(0, 6) + sDtOperatedYear;
            sDtO3rd = sDueJul.Substring(0, 6) + sDtOperatedYear;
            sDtO4th = sDueOct.Substring(0, 6) + sDtOperatedYear;
            cdtDtO1st = Convert.ToDateTime(sDtO1st);
            cdtDtO2nd = Convert.ToDateTime(sDtO2nd);
            cdtDtO3rd = Convert.ToDateTime(sDtO3rd);
            cdtDtO4th = Convert.ToDateTime(sDtO4th);

            if (sQtr == "1")
                cdtQtrDate = Convert.ToDateTime(sDtO1st);
            if (sQtr == "2")
                cdtQtrDate = Convert.ToDateTime(sDtO2nd);
            if (sQtr == "3")
                cdtQtrDate = Convert.ToDateTime(sDtO3rd);
            if (sQtr == "4")
                cdtQtrDate = Convert.ToDateTime(sDtO4th);

            String sBaseYear;
            for (int dYear = Convert.ToInt32(sTaxYear); dYear <= Convert.ToInt32(m_sCurrentTaxYear); dYear++)
            {
                sBaseYear = dYear.ToString();

                sJan = sDueJan.Substring(0, 6) + sBaseYear;
                sFeb = sDueFeb.Substring(0, 6) + sBaseYear;
                sMar = sDueMar.Substring(0, 6) + sBaseYear;
                sApr = sDueApr.Substring(0, 6) + sBaseYear;
                sMay = sDueMay.Substring(0, 6) + sBaseYear;
                sJun = sDueJun.Substring(0, 6) + sBaseYear;
                sJul = sDueJul.Substring(0, 6) + sBaseYear;
                sAug = sDueAug.Substring(0, 6) + sBaseYear;
                sSep = sDueSep.Substring(0, 6) + sBaseYear;
                sOct = sDueOct.Substring(0, 6) + sBaseYear;
                sNov = sDueNov.Substring(0, 6) + sBaseYear;
                sDec = sDueDec.Substring(0, 6) + sBaseYear;

                Jan = Convert.ToDateTime(sJan);
                Feb = Convert.ToDateTime(sFeb);
                Mar = Convert.ToDateTime(sMar);
                Apr = Convert.ToDateTime(sApr);
                May = Convert.ToDateTime(sMay);
                Jun = Convert.ToDateTime(sJun);
                Jul = Convert.ToDateTime(sJul);
                Aug = Convert.ToDateTime(sAug);
                Sep = Convert.ToDateTime(sSep);
                Oct = Convert.ToDateTime(sOct);
                Nov = Convert.ToDateTime(sNov);
                Dec = Convert.ToDateTime(sDec);

                //(s)//LEO 02112003
                if (sDueState == "Q")  //LEO 02112003
                {
                    if (sQtr == "1")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO1st);
                        cdtDtOperated = Convert.ToDateTime(sDtO1st);
                        //m_sORDate = sDtO1st;
                    }
                    if (sQtr == "2")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO2nd);
                        cdtDtOperated = Convert.ToDateTime(sDtO2nd);
                        //m_sORDate = sDtO2nd;
                    }
                    if (sQtr == "3")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO3rd);
                        cdtDtOperated = Convert.ToDateTime(sDtO3rd);
                        ///m_sORDate = sDtO3rd;
                    }
                    if (sQtr == "4")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO4th);
                        cdtDtOperated = Convert.ToDateTime(sDtO4th);
                        //m_sORDate = sDtO4th;
                    }
                }
                //(e)//LEO 02112003

                //if (mv_cdtORDate > Jan || m_sORDate == sJan || (((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("01/20/" + sBaseYear) || m_sORDate == ("01/20/" + sBaseYear)) && mv_cdtORDate > Dec) || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("01/20/" + sBaseYear) || m_sORDate == ("01/20/" + sBaseYear)) && Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year)))
                if ((((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("01/20/" + sBaseYear) || m_sORDate == ("01/20/" + sBaseYear)) && mv_cdtORDate > Dec) || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("01/20/" + sBaseYear) || m_sORDate == ("01/20/" + sBaseYear)) && Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year))) //AFM 20210120 MAO-21-14478
                {
                    if (m_sORDate == sJan)
                    {
                        if (sBaseYear != sTaxYear)
                        {
                            /*
                            dPenRate2 = dPenRate2 + m_dPenRate;				
                            dPenRate3 = dPenRate3 + m_dPenRate;				
                            dPenRate4 = dPenRate4 + m_dPenRate;	
                            */
                        }
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate1 = m_dSurchRate;
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate2 = dPenRate2 + m_dPenRate;
                                dPenRate3 = dPenRate3 + m_dPenRate;
                                dPenRate4 = dPenRate4 + m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Jan && cdtQtrDate <= Jan)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                if (mv_cdtORDate > Feb || m_sORDate == sFeb)
                {
                    if (m_sORDate == sFeb)
                    {

                        if (sBaseYear != sTaxYear)
                        {
                            /*
                            dPenRate2 = dPenRate2 + m_dPenRate;				
                            dPenRate3 = dPenRate3 + m_dPenRate;				
                            dPenRate4 = dPenRate4 + m_dPenRate;	
                            */
                        }
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate2 = dPenRate2 + m_dPenRate;
                                dPenRate3 = dPenRate3 + m_dPenRate;
                                dPenRate4 = dPenRate4 + m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Feb && cdtQtrDate <= Feb)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                //if ( mv_cdtORDate > Mar)
                if (mv_cdtORDate > Mar || m_sORDate == sMar)
                {
                    if (m_sORDate == sMar)
                    {
                        /*
                        if (sBaseYear != sTaxYear)
                        {
                            dPenRate2 = dPenRate2 + m_dPenRate;				
                            dPenRate3 = dPenRate3 + m_dPenRate;				
                            dPenRate4 = dPenRate4 + m_dPenRate;				
                            }
                        */
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest					

                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate2 = dPenRate2 + m_dPenRate;
                                dPenRate3 = dPenRate3 + m_dPenRate;
                                dPenRate4 = dPenRate4 + m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Mar && cdtQtrDate <= Mar)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                if (mv_cdtORDate > Apr || m_sORDate == sApr || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("04/20/" + sBaseYear) || m_sORDate == ("04/20/" + sBaseYear)) && Convert.ToInt32(sQtrPaid) <= 1 && Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year)) // GDE 20090429 try pag prompt payer
                {
                    if (m_sORDate == sApr)
                    {
                        /*
                        if (sBaseYear != sTaxYear)
                        {
                            dPenRate3 = dPenRate3 + m_dPenRate;				
                            dPenRate4 = dPenRate4 + m_dPenRate;				
                            }
                        */
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate2 = m_dSurchRate;
                            dPenRate1 = dPenRate1 + m_dPenRate;

                            // GDE 20130402
                            if (IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "1") == true)
                                dPenRate2 = dPenRate2 + m_dPenRate;
                            else
                                dPenRate2 = dPenRate1;
                            // GDE 20130402

                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate3 = dPenRate3 + m_dPenRate;
                                dPenRate4 = dPenRate4 + m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Apr && cdtQtrDate <= Apr)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                if (mv_cdtORDate > May || m_sORDate == sMay)
                {
                    if (m_sORDate == sMay)
                    {
                        /*
                        if (sBaseYear != sTaxYear)
                        {
                            dPenRate3 = dPenRate3 + m_dPenRate;				
                            dPenRate4 = dPenRate4 + m_dPenRate;				
                            }
                        */
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            // GDE 20130402
                            if (IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "1") == true)
                                dPenRate2 = dPenRate2 + m_dPenRate;
                            else
                                dPenRate2 = dPenRate1;
                            // GDE 20130402
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate3 = dPenRate3 + m_dPenRate;
                                dPenRate4 = dPenRate4 + m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= May && cdtQtrDate <= May)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                if (mv_cdtORDate > Jun || m_sORDate == sJun)
                {
                    if (m_sORDate == sJun)
                    {
                        /*
                        if (sBaseYear != sTaxYear)
                        {
                            dPenRate3 = dPenRate3 + m_dPenRate;				
                            dPenRate4 = dPenRate4 + m_dPenRate;				
                            }
                        */
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            dPenRate2 = dPenRate2 + m_dPenRate;
                            // GDE 20130402
                            if (IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "1") == false)
                                dPenRate2 = dPenRate1;
                            // GDE 20130402
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate3 = dPenRate3 + m_dPenRate;
                                dPenRate4 = dPenRate4 + m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Jun && cdtQtrDate <= Jun)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                if (mv_cdtORDate > Jul || m_sORDate == sJul || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("07/20/" + sBaseYear) || m_sORDate == ("07/20/" + sBaseYear)) && Convert.ToInt16(sQtrPaid) <= 2 && Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year))
                {
                    if (m_sORDate == sJul)
                    {
                        //if (sBaseYear != sTaxYear)
                        //    dPenRate4 = dPenRate4 + m_dPenRate;				
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate3 = m_dSurchRate;
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            dPenRate2 = dPenRate2 + m_dPenRate;
                            dPenRate3 = dPenRate3 + m_dPenRate;
                            if (IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "2") == false)
                                dPenRate3 = dPenRate2;

                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate4 = dPenRate4 + m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Jul && cdtQtrDate <= Jul)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                if (mv_cdtORDate > Aug || m_sORDate == sAug)
                {
                    if (m_sORDate == sAug)
                    {
                        /*
                        if (sBaseYear != sTaxYear)
                        {
                            dPenRate4 = dPenRate4 + m_dPenRate;				
                            }
                        */
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            dPenRate2 = dPenRate2 + m_dPenRate;
                            dPenRate3 = dPenRate3 + m_dPenRate;
                            // GDE 20130402
                            if (IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "2") == false)
                                dPenRate3 = dPenRate2;
                            // GDE 20130402				
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate4 = dPenRate4 + m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Aug && cdtQtrDate <= Aug)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                if (mv_cdtORDate > Sep || m_sORDate == sSep)
                {
                    if (m_sORDate == sSep)
                    {
                        /*
                        if (sBaseYear != sTaxYear)
                        {
                            dPenRate4 = dPenRate4 + m_dPenRate;				
                            }
                        */
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            dPenRate2 = dPenRate2 + m_dPenRate;
                            dPenRate3 = dPenRate3 + m_dPenRate;
                            // GDE 20130402
                            if (IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "2") == false)
                                dPenRate3 = dPenRate2;
                            // GDE 20130402					
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate4 = dPenRate4 + m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Sep && cdtQtrDate <= Sep)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                if (mv_cdtORDate > Oct || m_sORDate == sOct || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("10/20/" + sBaseYear) || m_sORDate == ("10/20/" + sBaseYear)) && Convert.ToInt32(sQtrPaid) <= 3 && Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year))
                {
                    if (m_sORDate == sOct)
                    {
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate4 = m_dSurchRate;
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            dPenRate2 = dPenRate2 + m_dPenRate;
                            dPenRate3 = dPenRate3 + m_dPenRate;
                            dPenRate4 = dPenRate4 + m_dPenRate;
                            // GDE 20130402	
                            if (IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "3") == false)
                                dPenRate4 = dPenRate3;
                            // GDE 20130402	
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Oct && cdtQtrDate <= Oct)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                if (mv_cdtORDate > Nov || m_sORDate == sNov)
                {
                    if (m_sORDate == sNov)
                    {
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            dPenRate2 = dPenRate2 + m_dPenRate;
                            dPenRate3 = dPenRate3 + m_dPenRate;
                            dPenRate4 = dPenRate4 + m_dPenRate;
                            // GDE 20130402	
                            if (IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "3") == false)
                                dPenRate4 = dPenRate3;
                            // GDE 20130402	
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Nov && cdtQtrDate <= Nov)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02032004 to compute surcharge/interest
                if (mv_cdtORDate > Dec || m_sORDate == sDec)
                {
                    if (m_sORDate == sDec)
                    {
                    }
                    else
                    {
                        //(e)//LEO 02032004 to compute surcharge/interest
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            dPenRate2 = dPenRate2 + m_dPenRate;
                            dPenRate3 = dPenRate3 + m_dPenRate;
                            dPenRate4 = dPenRate4 + m_dPenRate;
                            // GDE 20130402	
                            if (IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "3") == false)
                                dPenRate4 = dPenRate3;
                            // GDE 20130402			
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Dec && cdtQtrDate <= Dec)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart = PenQuart + m_dPenRate;
                            }
                        }
                    }
                }

                //(s)//LEO 02072003
                if (dPenRate1 > 0.72)
                    dPenRate1 = 0.72;
                if (dPenRate2 > 0.72)
                    dPenRate2 = 0.72;
                if (dPenRate3 > 0.72)
                    dPenRate3 = 0.72;
                if (dPenRate4 > 0.72)
                    dPenRate4 = 0.72;
                if (PenQuart > 0.72)
                    PenQuart = 0.72;
                //(e)//LEO 02072003

                mp_dSurchRate1 = dSurchRate1;
                mp_dSurchRate2 = dSurchRate2;
                mp_dSurchRate3 = dSurchRate3;
                mp_dSurchRate4 = dSurchRate4;
                mp_dPenRate1 = dPenRate1;
                mp_dPenRate2 = dPenRate2;
                mp_dPenRate3 = dPenRate3;
                mp_dPenRate4 = dPenRate4;
                mp_SurchQuart = SurchQuart;
                mp_PenQuart = PenQuart;
            }

            m_sORDate = string.Format("{0}/{1}/{2}", mv_cdtORDate.Month, mv_cdtORDate.Day, mv_cdtORDate.Year);
        }

        private bool IsPaidOnLastQtr(String sBin, String sTaxYear, String sBnsCode, String sQtr)
        {
            bool bResult = false;
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select distinct * from pay_hist where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and qtr_paid = '" + sQtr + "'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    bResult = true;
                }
            pSet.Close();
            return bResult;
        }

        

    }
}