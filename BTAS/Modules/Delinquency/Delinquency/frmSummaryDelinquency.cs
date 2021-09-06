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
    public partial class frmSummaryDelinquency : Form
    {
        public frmSummaryDelinquency()
        {
            InitializeComponent();
        }

        ArrayList arr_BnsCode = new ArrayList();

        int m_iRadioNumber = 0;
        string m_sBnsCode = String.Empty;
        string m_sReportName = String.Empty, m_sDate = String.Empty, m_sYear = String.Empty, m_sDelqQtr = String.Empty;
        string m_sQuery = String.Empty;


        //MCR 20140715 (s)
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
        //MCR 20140715 (e)

        private void frmSummaryDelinquency_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            LoadBrgy();
            LoadBusType();
            LoadDist();
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

        private void cmbBusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_sBnsCode = arr_BnsCode[cmbBusType.SelectedIndex].ToString();
            }
            catch { return; }
        }

        private void BtnControl(bool blnEnable)
        {
            cmbBrgy.Enabled = blnEnable;
            cmbBusType.Enabled = blnEnable;
            cmbDist.Enabled = blnEnable;
            cmbOrgnKind.Enabled = blnEnable;
        }

        private void rdoBrgy_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 0;
            BtnControl(false);
            cmbBrgy.Enabled = true;
            cmbBrgy.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
        }

        private void rdoDist_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 1;
            BtnControl(false);
            cmbDist.Enabled = true;
            cmbBrgy.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
        }

        private void rdoMainBus_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 2;
            BtnControl(false);
            cmbBusType.Enabled = true;
            cmbBrgy.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
        }

        private void rdoOwnKnd_Click(object sender, EventArgs e)
        {
            m_iRadioNumber = 3;
            BtnControl(false);
            cmbOrgnKind.Enabled = true;
            cmbBrgy.SelectedIndex = 0;
            cmbBusType.SelectedIndex = 0;
            cmbDist.SelectedIndex = 0;
            cmbOrgnKind.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            String sJanuary, sApril, sJuly, sOctober, sDay;
            DateTime odtJanuary, odtApril, odtJuly, odtOctober, odtDate;

            DateTime m_vodtDate;
            //m_vodtDate = COleDateTime::GetCurrentTime();
            m_vodtDate = AppSettingsManager.GetSystemDate();

            sDay = AppSettingsManager.GetConfigValue("14");
            m_sYear = m_vodtDate.Year.ToString();
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

            if (m_iRadioNumber == 0)
                m_sReportName = "SUMMARY OF DELINQUENT BY BARANGAY";
            else if (m_iRadioNumber == 1)
                m_sReportName = "SUMMARY OF DELINQUENT BY DISTRICT";
            else if (m_iRadioNumber == 2)
                m_sReportName = "SUMMARY OF DELINQUENT BY MAIN BUSINESS";
            else if (m_iRadioNumber == 3)
                m_sReportName = "SUMMARY OF DELINQUENT BY OWNERSHIP KIND";


            if (AppSettingsManager.GenerateInfo(m_sReportName))
            {
                OnReportSummaryOfDelinquent();

                m_sQuery = @"SELECT rep_summ_delq.heading, rep_summ_delq.subgroup1, rep_summ_delq.ren_no, rep_summ_delq.ren_gr, rep_summ_delq.new_no, rep_summ_delq.new_gr, rep_summ_delq.ret_no, rep_summ_delq.ret_gr, rep_summ_delq.cut_off_dt, rep_summ_delq.current_user
	FROM REP_SUMM_DELQ rep_summ_delq WHERE report_name = '" + m_sReportName + "' and user_code = '" + AppSettingsManager.SystemUser.UserCode + "' ORDER BY  rep_summ_delq.subgroup1 ASC";

                frmDelinquencyReport frmDelinquencyReport = new frmDelinquencyReport();
                frmDelinquencyReport.ReportName = m_sReportName;
                frmDelinquencyReport.Query = m_sQuery;
                frmDelinquencyReport.ShowDialog();
            }
        }

        private void OnReportSummaryOfDelinquent()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            OracleResultSet pSet3 = new OracleResultSet();

            String sQuery = "";
            String sBnsCode, sDelqOrgnKind = "", sDelqBnsDesc = "", sBnsCodeSub, sQBnsCode, sTmpBrgyName = "", sTmpDistName = "", sTmpOrgnKind = "", sTmpStatus = "", sCurrentUser;
            String sDelqBin, sDelqBnsName = "", sDelqBrgyName = "", sDelqDistName = "", sDelqBnsStat, sDelqBnsAddr, sDelqBnsCode, sDelqOwnName, sDelqOwnAddr, sDelqOwnCode, sDelqPermit, sDelqTaxYear, sDelqOrNo, sDelqQtrPaid, sDelqTerm, sDelqOrDate;
            double dDelqBnsGr1, dDelqBnsGr2, dDelqBnsCap, dDelqGross, dBFeesDue, dRFeesDue, dFeesPen, dFeesSurch, dSurchPen;
            int iBrgyCount;
            int dDelqRenNo, dDelqNewNo, dDelqRetNo;
            double dDelqRenGross, dDelqNewGross, dDelqRetGross;

            //{(s)----------------------INITIALIZED ALL NECESSARY DATA--------------------
            dDelqRenNo = 0;
            dDelqRenGross = 0;
            dDelqNewNo = 0;
            dDelqNewGross = 0;
            dDelqRetNo = 0;
            dDelqRetGross = 0;

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

            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            if (cmbBrgy.Text.Trim() == "ALL")
                sTmpBrgyName = "%%";
            else
                sTmpBrgyName = cmbBrgy.Text.Trim() + "%";

            if (cmbDist.Text.Trim() == "ALL")
                sTmpDistName = "%%";
            else
                sTmpBrgyName = cmbDist.Text.Trim() + "%";

            if (m_sBnsCode == "")
                sBnsCode = "%";
            else
                sBnsCode = m_sBnsCode + "%";

            if (cmbOrgnKind.Text.Trim() == "ALL")
                sTmpOrgnKind = "%%";
            else
                sTmpOrgnKind = cmbOrgnKind.Text.Trim() + "%";

            //}(e)----------------------INITIALIZED ALL NECESSARY DATA--------------------
            pSet.Query = string.Format("DELETE FROM REP_SUMM_DELQ WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
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

            if (m_iRadioNumber == 0)
                sQuery = string.Format("select count(distinct bns_brgy) as iCount from businesses where bns_brgy like '{0}'", sTmpBrgyName);
            else if (m_iRadioNumber == 1)
                sQuery = string.Format("select count(distinct bns_dist) as iCount from businesses where bns_brgy like '{0}'", sTmpDistName);
            else if (m_iRadioNumber == 2)
            {
                sQuery = string.Format(@"select count(*) as iCount from bns_table where rtrim(bns_code) like '{0}'
				and fees_code = 'B' and rev_year = '{1}' and length(rtrim(bns_code)) = 2", sBnsCode, AppSettingsManager.GetConfigValue("07"));
            }
            else if (m_iRadioNumber == 3)
            {
                sQuery = string.Format(@"select count(distinct orgn_kind) as iCount from businesses where orgn_kind like '{0}'", sTmpOrgnKind);
            }

            #region Progress
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            pSet.Query = sQuery;
            intCountIncreament = 0;
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            if (m_iRadioNumber == 0)
                sQuery = string.Format("select distinct bns_brgy from businesses where bns_brgy like '{0}' order by bns_brgy", sTmpBrgyName);
            else if (m_iRadioNumber == 1)
                sQuery = string.Format("select distinct bns_dist from businesses where bns_dist like '{0}' order by bns_dist", sTmpDistName);
            else if (m_iRadioNumber == 2)
            {
                sQuery = string.Format(@"select * from bns_table where rtrim(bns_code) like '{0}' and fees_code = 'B'
				and length(rtrim(bns_code)) = 2 and rev_year = '{1}' order by bns_code", sBnsCode, AppSettingsManager.GetConfigValue("07"));
            }
            else if (m_iRadioNumber == 3)
            {
                sQuery = string.Format("select distinct orgn_kind from businesses where rtrim(orgn_kind) like '{0}' order by orgn_kind", sTmpOrgnKind);
            }

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dDelqRenNo = 0;
                    dDelqRenGross = 0;
                    dDelqNewNo = 0;
                    dDelqNewGross = 0;
                    dDelqRetNo = 0;
                    dDelqRetGross = 0;

                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    if (m_iRadioNumber == 0)
                    {
                        sDelqBrgyName = StringUtilities.RemoveApostrophe(pSet.GetString("bns_brgy").Trim());
                        sQuery = string.Format(@"select unique a.bin,b.bns_nm,b.bns_stat,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
					             where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_brgy = '{1}' order by b.bns_nm,a.bin", m_sYear, sDelqBrgyName);
                    }
                    else if (m_iRadioNumber == 1)
                    {
                        sDelqDistName = StringUtilities.RemoveApostrophe(pSet.GetString("bns_dist").Trim()) + "%";
                        sQuery = string.Format(@"select unique a.bin,b.bns_nm,b.bns_stat,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
					             where a.tax_year <= '{0}' and a.bin = b.bin and b.bns_dist like '{1}' order by b.bns_nm,a.bin", m_sYear, sDelqDistName);
                    }
                    else if (m_iRadioNumber == 2)
                    {
                        sDelqBnsCode = StringUtilities.RemoveApostrophe(pSet.GetString("bns_code").Trim()) + "%";
                        sDelqBnsDesc = StringUtilities.RemoveApostrophe(pSet.GetString("bns_desc").Trim());
                        sQuery = string.Format(@"select distinct a.bin,b.bns_nm,b.bns_stat,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
							where a.tax_year <= '{0}' and a.bin = b.bin and rtrim(b.bns_code) like '{1}' order by b.bns_nm,a.bin", m_sYear, sDelqBnsCode);
                    }
                    else if (m_iRadioNumber == 3)
                    {
                        sDelqOrgnKind = StringUtilities.RemoveApostrophe(pSet.GetString("orgn_kind").Trim());
                        sQuery = string.Format(@"select unique a.bin,b.bns_nm,b.bns_stat,b.gr_1,b.gr_2,b.capital from pay_hist a, businesses b
					             where a.tax_year <= '{0}' and a.bin = b.bin and b.orgn_kind = '{1}' order by b.bns_nm,a.bin", m_sYear, sDelqOrgnKind);
                    }

                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                    {
                        while (pSet1.Read())
                        {
                            sDelqBin = StringUtilities.RemoveApostrophe(pSet1.GetString("bin").Trim());
                            sDelqBnsStat = StringUtilities.RemoveApostrophe(pSet1.GetString("bns_stat").Trim());
                            dDelqBnsGr1 = pSet1.GetDouble("gr_1");
                            dDelqBnsGr2 = pSet1.GetDouble("gr_2");
                            dDelqBnsCap = pSet1.GetDouble("capital");

                            dDelqGross = dDelqBnsGr1 + dDelqBnsGr2;

                            sQuery = string.Format(@"select tax_year,qtr_paid from pay_hist where bin = '{0}' order by tax_year desc, qtr_paid desc", sDelqBin);
                            pSet2.Query = sQuery;
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                {
                                    sDelqTaxYear = pSet2.GetString("tax_year").Trim();
                                    sDelqQtrPaid = pSet2.GetString("qtr_paid").Trim();

                                    //(s){---------------------------------SAVING VALUES-----------------------------
                                    if (Convert.ToInt32(sDelqTaxYear) < Convert.ToInt32(m_sYear))
                                    {
                                        if (sDelqBnsStat == "REN")
                                        {
                                            dDelqRenNo += 1;
                                            dDelqRenGross += dDelqGross;
                                        }
                                        else if (sDelqBnsStat == "NEW")
                                        {
                                            dDelqNewNo += 1;
                                            dDelqNewGross += dDelqBnsCap;
                                        }
                                        else if (sDelqBnsStat == "RET")
                                        {
                                            dDelqRetNo += 1;
                                            dDelqRetGross += dDelqGross;
                                        }

                                    }

                                    if (sDelqQtrPaid != "F")// && sDelqQtrPaid != "Y" && sDelqQtrPaid != "P" && sDelqQtrPaid != "")
                                    {
                                        int numValue = 0;
                                        bool parsed = Int32.TryParse(sDelqQtrPaid, out numValue);

                                        if ((Convert.ToInt32(sDelqTaxYear) == Convert.ToInt32(m_sYear)) && (numValue < Convert.ToInt32(m_sDelqQtr)))
                                        {
                                            if (sDelqBnsStat == "REN")
                                            {
                                                dDelqRenNo += 1;
                                                dDelqRenGross += dDelqGross;
                                            }
                                            else if (sDelqBnsStat == "NEW")
                                            {
                                                dDelqNewNo += 1;
                                                dDelqNewGross += dDelqBnsCap;
                                            }
                                            else if (sDelqBnsStat == "RET")
                                            {
                                                dDelqRetNo += 1;
                                                dDelqRetGross += dDelqGross;
                                            }

                                        }
                                    }
                                    //(e)}---------------------------------SAVING VALUES-----------------------------

                                }
                            pSet2.Close();
                        }
                    }
                    pSet1.Close();

                    //Saving..............
                    if (sDelqBrgyName == " " || sDelqBrgyName == "")
                        sDelqBrgyName = "NO BARANGAY";

                    //Saving..............
                    if (sDelqDistName == " " || sDelqDistName == "" || sDelqDistName == "%")
                        sDelqDistName = "NO DISTRICT";

                    if (m_iRadioNumber == 0)
                    {
                        sQuery = string.Format(@"insert into rep_summ_delq values('{0}','','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09")),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBrgyName.Trim(), 30)),
                                               dDelqRenNo.ToString("0.00"),
                                               dDelqRenGross.ToString("0.00"),
                                               dDelqNewNo.ToString("0.00"),
                                               dDelqNewGross.ToString("0.00"),
                                               dDelqRetNo.ToString("0.00"),
                                               dDelqRetGross.ToString("0.00"),
                                               StringUtilities.HandleApostrophe(m_sDate.Trim()),
                                               StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                               StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                               StringUtilities.HandleApostrophe(sCurrentUser.Trim()));
                    }
                    else if (m_iRadioNumber == 1)
                    {
                        sQuery = string.Format(@"insert into rep_summ_delq values('{0}','','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                                                  StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09")),
                                                  StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqDistName.Trim(), 30)),
                                                  dDelqRenNo.ToString("0.00"),
                                                  dDelqRenGross.ToString("0.00"),
                                                  dDelqNewNo.ToString("0.00"),
                                                  dDelqNewGross.ToString("0.00"),
                                                  dDelqRetNo.ToString("0.00"),
                                                  dDelqRetGross.ToString("0.00"),
                                                  StringUtilities.HandleApostrophe(m_sDate.Trim()),
                                                  StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                  StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                  StringUtilities.HandleApostrophe(sCurrentUser.Trim()));
                    }
                    else if (m_iRadioNumber == 2)
                    {
                        sQuery = string.Format(@"insert into rep_summ_delq values('{0}','','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                                                  StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09")),
                                                  StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqBnsDesc.Trim(), 30)),
                                                  dDelqRenNo.ToString("0.00"),
                                                  dDelqRenGross.ToString("0.00"),
                                                  dDelqNewNo.ToString("0.00"),
                                                  dDelqNewGross.ToString("0.00"),
                                                  dDelqRetNo.ToString("0.00"),
                                                  dDelqRetGross.ToString("0.00"),
                                                  StringUtilities.HandleApostrophe(m_sDate.Trim()),
                                                  StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                  StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                  StringUtilities.HandleApostrophe(sCurrentUser.Trim()));
                    }
                    else if (m_iRadioNumber == 3)
                    {
                        sQuery = string.Format(@"insert into rep_summ_delq values('{0}','','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                                                  StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09")),
                                                  StringUtilities.HandleApostrophe(StringUtilities.Left(sDelqOrgnKind.Trim(), 30)),
                                                  dDelqRenNo.ToString("0.00"),
                                                  dDelqRenGross.ToString("0.00"),
                                                  dDelqNewNo.ToString("0.00"),
                                                  dDelqNewGross.ToString("0.00"),
                                                  dDelqRetNo.ToString("0.00"),
                                                  dDelqRetGross.ToString("0.00"),
                                                  StringUtilities.HandleApostrophe(m_sDate.Trim()),
                                                  StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                                  StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode),
                                                  StringUtilities.HandleApostrophe(sCurrentUser.Trim()));
                    }

                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                    
                    intCountSaved++;
                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            pSet.Close();

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }
    }
}