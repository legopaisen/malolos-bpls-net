using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Amellar.Modules.LiquidationReports;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.DynamicProgressBar;
using System.Threading;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmSummaryofCollection : Form
    {
        public frmSummaryofCollection()
        {
            InitializeComponent();
        }

        private string m_sRadioBrgy = "";
        private string m_sBnsCode = "";
        ArrayList arr_BnsCode = new ArrayList();
        private string m_sReportName = "";
        private string m_sRevYear = AppSettingsManager.GetConfigValue("07");
        private int m_iOptionFormat = 0;

        //MCR 20140709 (s)
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
            }
        }
        //MCR 20140709 (e)

        private void frmSummaryofCollection_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            dtpTo.Value = AppSettingsManager.GetSystemDate();
            rdoBrgy.PerformClick();
            LoadBrgy();
            LoadBusType();
            LoadDist();
            LoadLineofBusiness();
            LoadTaxYear(); //AFM 20190827
            cmbOrgnKind.SelectedIndex = 0;
            cmbBusStat.SelectedIndex = 0;

            // RMC 20150505 QA reports (s)
            if (AppSettingsManager.GetConfigValue("01") == "MUNICIPALITY")
                rdoDist.Enabled = false;
            // RMC 20150505 QA reports (e)
        }

        private void BtnStyle()
        {
            foreach (Object c in this.Controls)
            {
                if (c is ComponentFactory.Krypton.Toolkit.KryptonButton)
                {
                    ((ComponentFactory.Krypton.Toolkit.KryptonButton)c).ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
                }
            }
        }

        private void BtnControls(Boolean blnEnable)
        {
            btnSim.Enabled = blnEnable;
            btnSimDist.Enabled = blnEnable;
            btnStan.Enabled = blnEnable;
            btnStanDist.Enabled = blnEnable;
            cmbBrgy.Enabled = blnEnable;
            cmbBusStat.Enabled = blnEnable;
            cmbBusType.Enabled = blnEnable;
            cmbDist.Enabled = blnEnable;
            cmbLineBus.Enabled = blnEnable;
            cmbOrgnKind.Enabled = blnEnable;
            chkFilterReport.Visible = blnEnable; //MCR 20190509
            chkFilterReport.Checked = blnEnable;
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
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = string.Format("select bns_desc,bns_code from bns_table where fees_code = 'B' and length(rtrim(bns_code)) > 3 and rev_year = '{0}' and rtrim(bns_code) like '{1}' order by bns_code", AppSettingsManager.GetConfigValue("07"), m_sBnsCode + "%%");
            if (pSet.Execute())
                while (pSet.Read())
                {
                    cmbLineBus.Items.Add(pSet.GetString(0).Trim());
                }
            pSet.Close();

            cmbLineBus.SelectedIndex = 0;
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

        private void cmbDist_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void rdoBrgy_Click(object sender, EventArgs e)
        {
            m_sRadioBrgy = "0";
            BtnControls(false);
            btnStan.Enabled = true;
            btnSim.Enabled = true;
            btnStan.PerformClick();
            chkStandardFormat.Checked = true;
            lblYear.Visible = false;
            cmbYear.Visible = false;
            dtpFrom.Visible = true;
            dtpTo.Visible = true;
            label7.Visible = true;
            label6.Visible = true;
            chkGenReport.Visible = true;
        }

        private void rdoDist_Click(object sender, EventArgs e)
        {
            m_sRadioBrgy = "1";
            BtnControls(false);
            btnStan.Enabled = true;
            btnSim.Enabled = true;
            btnStan.PerformClick();
            lblYear.Visible = false;
            cmbYear.Visible = false;
            dtpFrom.Visible = true;
            dtpTo.Visible = true;
            label7.Visible = true;
            label6.Visible = true;
            chkGenReport.Visible = true;
        }

        private void rdoMainBus_Click(object sender, EventArgs e)
        {
            m_sRadioBrgy = "2";
            BtnControls(false);
            btnStan.Enabled = true;
            btnSim.Enabled = true;
            btnSimDist.Enabled = true;
            btnStanDist.Enabled = true;
            chkFilterReport.Visible = true; //MCR 20190509
            chkStandardFormat.Checked = true;
            btnStan.PerformClick();
            lblYear.Visible = false;
            cmbYear.Visible = false;
            dtpFrom.Visible = true;
            dtpTo.Visible = true;
            label7.Visible = true;
            label6.Visible = true;
            chkGenReport.Visible = true;
        }

        private void rdoOwnKind_Click(object sender, EventArgs e)
        {
            m_sRadioBrgy = "3";
            BtnControls(false);
            btnStan.Enabled = true;
            btnStanDist.Enabled = true;
            chkStandardFormat.Checked = true;
            btnStan.PerformClick();
            lblYear.Visible = false;
            cmbYear.Visible = false;
            dtpFrom.Visible = true;
            dtpTo.Visible = true;
            label7.Visible = true;
            label6.Visible = true;
            chkGenReport.Visible = true;
        }

        private void rdoBusStat_Click(object sender, EventArgs e)
        {
            m_sRadioBrgy = "4";
            BtnControls(false);
            btnStan.Enabled = true;
            btnStanDist.Enabled = true;
            chkStandardFormat.Checked = true;
            btnStan.PerformClick();
            lblYear.Visible = false;
            cmbYear.Visible = false;
            dtpFrom.Visible = true;
            dtpTo.Visible = true;
            label7.Visible = true;
            label6.Visible = true;
            chkGenReport.Visible = true;
        }

        private void rdoLineBus_Click(object sender, EventArgs e)
        {
            m_sRadioBrgy = "5";
            BtnControls(false);
            btnStan.Enabled = true;
            cmbBusType.Enabled = true;
            chkFilterReport.Visible = true; //MCR 20190509
            chkStandardFormat.Checked = true;
            btnStan.PerformClick();
            lblYear.Visible = false;
            cmbYear.Visible = false;
            dtpFrom.Visible = true;
            dtpTo.Visible = true;
            label7.Visible = true;
            label6.Visible = true;
            chkGenReport.Visible = true;
            chkStandardFormat.Visible = true;
            chkSimplifiedFormat.Visible = true;
        }

        //AFM 20190827
        private void rdoTotalCol_Click(object sender, EventArgs e)
        {
            m_sRadioBrgy = "6";
            cmbYear.Enabled = true;
            lblYear.Visible = true;
            cmbYear.Visible = true;
            dtpFrom.Visible = false;
            dtpTo.Visible = false;
            label7.Visible = false;
            label6.Visible = false;
            chkGenReport.Visible = false;
            chkStandardFormat.Visible = false;
            chkSimplifiedFormat.Visible = false;
            BtnControls(false);
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

        private void btnStan_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 0;
            BtnStyle();
            btnStan.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Alternate;
        }

        private void btnSim_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 1;
            BtnStyle();
            btnSim.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Alternate;
        }

        private void btnStanDist_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 2;
            BtnStyle();
            btnStanDist.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Alternate;
        }

        private void btnSimDist_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 3;
            BtnStyle();
            btnSimDist.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Alternate;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // RMC 20150524 corrections in reports (s)
            if (chkStandardFormat.Checked == false && chkSimplifiedFormat.Checked == false)
            {
                MessageBox.Show("Select report format before generation","Summary of Collection",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            // RMC 20150524 corrections in reports (e)

            //AFM 20190827 for total collection report (s)
            if (rdoTotalCol.Checked == true)
            {
                if (cmbYear.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Tax Year!");
                    return;
                }
            }
            //AFM 20190827 for total collection report (e);

            switch (m_sRadioBrgy)
            {
                case "0":
                    {
                        m_sReportName = "SUMMARY OF COLLECTION BY BARANGAY";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            if (chkGenReport.Checked == false)
                                OnReportWideSummaryOfCollectionBarangay();
                            else
                                OnPrintReportSummaryOfCollectionBarangay(m_sReportName);
                        }
                        break;
                    }
                case "1":
                    {
                        m_sReportName = "SUMMARY OF COLLECTION BY DISTRICT";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            if (chkGenReport.Checked == false)
                                OnReportWideSummaryOfCollectionDistrict();
                            else
                                OnPrintReportWideSummaryOfCollectionDistrict(m_sReportName);
                        }
                        break;
                    }
                case "2":
                    {
                        m_sReportName = "SUMMARY OF COLLECTION BY MAIN BUSINESS";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            if (chkGenReport.Checked == false)
                                OnReportWideSummaryOfCollectionMainBusiness();
                            else
                                OnPrintReportWideSummaryOfCollectionMainBusiness(m_sReportName);
                        }
                        break;
                    }
                case "3":
                    {
                        m_sReportName = "SUMMARY OF COLLECTION BY OWNERSHIP KIND";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            if (chkGenReport.Checked == false)
                                OnReportWideSummaryOfCollectionOrgnKind();
                            else
                                OnPrintReportWideSummaryOfCollectionOrgnKind(m_sReportName);
                        }
                        break;
                    }
                case "4":
                    {
                        m_sReportName = "SUMMARY OF COLLECTION BY BUSINESS STATUS";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            if (chkGenReport.Checked == false)
                                OnReportWideSummaryOfCollectionBusinessStatus();
                            else
                                OnPrintReportWideSummaryOfCollectionBusinessStatus(m_sReportName);
                        }
                        break;
                    }
                case "5":
                    {
                        m_sReportName = "SUMMARY OF COLLECTION BY LINE OF BUSINESS";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            if (chkGenReport.Checked == false)
                                OnReportWideSummaryOfCollectionLineOfBusiness();
                            else
                                OnPrintReportWideSummaryOfCollectionLineOfBusiness(m_sReportName);
                        }
                        break;
                    }
                case "6":
                    {
                        m_sReportName = "SUMMARY OF MONTHLY COLLECTIONS";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            OnInitReportTotalCollections(m_sReportName);
                        }
                        break;
                    }
            }
        }

        private void OnInitReportTotalCollections(string p_sReportName)
        {
            frmLiqReports dlg = new frmLiqReports();
            OracleResultSet pSet = new OracleResultSet();
            string sQuery;

            DateTime m_dtSaveTime = AppSettingsManager.GetSystemDate();
            String sCurrentDate = string.Format("{1}/{2}/{0}",
                    m_dtSaveTime.Year,
                    m_dtSaveTime.Month,
                    m_dtSaveTime.Day);//  {3}:{4}:{5}, m_dtSaveTime.Hour, m_dtSaveTime.Minute, m_dtSaveTime.Second);

            pSet.Query = string.Format("DELETE FROM GEN_INFO WHERE REPORT_NAME = '{0}'", m_sReportName);
            pSet.ExecuteNonQuery();
            pSet.Close();

            pSet.Query = string.Format("INSERT INTO GEN_INFO VALUES('{0}',sysdate,'{1}',{2},'COL')", m_sReportName, AppSettingsManager.SystemUser.UserCode, 1);
            pSet.ExecuteNonQuery();
            pSet.Close();

            dlg.ReportSwitch = "OnReportTotalCollections";
            dlg.ReportTitle = p_sReportName;
            dlg.sTaxYear = cmbYear.Text;
            dlg.ShowDialog();


            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

 

        private void OnReportWideSummaryOfCollectionBarangay()
        {
            String sQuery;//,sFeesCode[21],sFeesDesc[21];
            String sBrgyName, sOwnKind = "", sFeesCodeX, sOrNo, sBin, sBinV, sBinX;
            String sCurrentUser;
            int i, ii, iFees_cnt, iRen, iNew, iRet, iCtr1;
            double fOrFees_due, fOrFees_surch, fOrFees_pen, fOrFees_amtdue;
            double fRenGr, fRenGross, fRetGr, fRetGross, fNewCap, fNewCapital;
            double fSurchPen, fTotalFees; //fHeaderFees[21]

            ArrayList arr_fHeaderFees = new ArrayList();
            ArrayList arr_sFeesCode = new ArrayList();
            ArrayList arr_sFeesDesc = new ArrayList();

            fSurchPen = 0;
            fTotalFees = 0;

            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            pSet.Query = string.Format("DELETE FROM REPORT_SUMCOLL WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
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

            for (ii = 0; ii <= 21; ii++)
                arr_fHeaderFees.Add(0);

            iCtr1 = 0;
            i = 0;
            ii = 0;
            arr_sFeesCode.Add("B");
            arr_sFeesDesc.Add("LICENSE TAX");

            sQuery = string.Format("select fees_code,fees_desc from tax_and_fees_table where rev_year = '{0}' order by fees_code", m_sRevYear);
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    iCtr1 += 1;

                    if (iCtr1 <= 19)    // RMC 20150126
                    {
                        arr_sFeesCode.Add(pSet.GetString("fees_code").Trim());
                        arr_sFeesDesc.Add(pSet.GetString("fees_desc").Trim());
                    }
                    
                }
            pSet.Close();
            
            // RMC 20150126 (S)
            if (iCtr1 > 19)
            {
                arr_sFeesCode.Add("00");
                arr_sFeesDesc.Add("OTHER FEES");
                iCtr1 = 20;
            }// RMC 20150126 (E)

            // RMC 20150505 QA reports (S)
            while (iCtr1 <= 20)
            {
                arr_sFeesCode.Add(" ");
                arr_sFeesDesc.Add(" ");
                iCtr1++;
            }
            // RMC 20150505 QA reports (E)

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            pSet.Query = "Select count(distinct bns_brgy) from businesses order by bns_brgy";
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            pSet.Query = "select distinct bns_brgy from businesses order by bns_brgy";	// Include No District
            if (pSet.Execute())
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sBrgyName = pSet.GetString("bns_brgy").Trim();		// Include No District
                    String sBnsStatus = "";
                    for (int iStat = 1; iStat <= 4; iStat++)
                    {
                        switch (iStat)
                        {
                            case 1:
                                sOwnKind = "SINGLE PROPRIETORSHIP";
                                break;
                            case 2:
                                sOwnKind = "PARTNERSHIP";
                                break;
                            case 3:
                                sOwnKind = "CORPORATION";
                                break;
                            case 4:
                                sOwnKind = "COOPERATIVE";
                                break;
                        }

                        iRen = 0;
                        iRet = 0;
                        iNew = 0;
                        fNewCapital = 0;
                        fRenGross = 0;
                        fRetGross = 0;
                        fSurchPen = 0;
                        fTotalFees = 0;

                        int iStatusCtr = 0;
                        double dStatusCap = 0;
                        double dStatusGross = 0;

                        arr_fHeaderFees.Clear();
                        for (ii = 0; ii <= iCtr1; ii++)
                            arr_fHeaderFees.Add(0);

                        for (i = 1; i <= 3; i++)
                        {
                            switch (i)
                            {
                                case 1:
                                    sBnsStatus = "NEW";
                                    break;
                                case 2:
                                    sBnsStatus = "REN";
                                    break;
                                case 3:
                                    sBnsStatus = "RET";
                                    break;
                            }

                            sBinX = "";

                            sQuery = string.Format(@"select distinct(pay_hist.or_no),pay_hist.bin,businesses.capital,businesses.gr_1 from pay_hist,businesses
								 where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy') and
								 businesses.bns_brgy = '{2}' and businesses.orgn_kind = '{3}' and businesses.bns_stat = '{4}'
							   order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"), sBrgyName, sOwnKind, sBnsStatus);

                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                            {
                                while (pSet1.Read())
                                {
                                    sOrNo = pSet1.GetString("or_no").Trim();
                                    sBin = pSet1.GetString("bin").Trim();

                                    
                                        //(s) RTL 06092006 revise the loop
                                    if (sBin != sBinX)
                                    {
                                        if (sBnsStatus == "NEW")
                                        {
                                            iNew = iNew + 1;
                                            fNewCap = pSet1.GetDouble("capital");
                                            fNewCapital = fNewCapital + fNewCap;
                                            iStatusCtr = iStatusCtr + 1;          // Added
                                            dStatusCap = dStatusCap + fNewCap;    // Added
                                        }
                                        if (sBnsStatus == "REN")
                                        {
                                            iRen = iRen + 1;
                                            fRenGr = pSet1.GetDouble("gr_1");
                                            fRenGross = fRenGross + fRenGr;
                                            iStatusCtr = iStatusCtr + 1;         // Added
                                            dStatusGross = dStatusGross + fRenGr;  // Added
                                        }
                                        if (sBnsStatus == "RET")
                                        {
                                            iRet = iRet + 1;
                                            fRetGr = pSet1.GetDouble("gr_1");
                                            fRetGross = fRetGross + fRetGr;
                                            iStatusCtr = iStatusCtr + 1;         // Added
                                            dStatusGross = dStatusGross + fRetGr;  // Added
                                        }
                                        sBinX = sBin;
                                    }
                                    

                                    sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no = '" + sOrNo + "' and fees_code like 'B%%' having sum(fees_amtdue) is not null");
                                    pSet2.Query = sQuery;
                                    if (pSet2.Execute())
                                    {
                                        if (pSet2.Read())
                                        {
                                            //if (pSet2.GetDouble("fees_amtdue").ToString() != null)
                                            //{
                                            fOrFees_due = pSet2.GetDouble("fees_due");
                                            fOrFees_surch = pSet2.GetDouble("fees_surch");
                                            fOrFees_pen = pSet2.GetDouble("fees_pen");
                                            fOrFees_amtdue = pSet2.GetDouble("fees_amtdue");

                                            arr_fHeaderFees[0] = Convert.ToDouble(arr_fHeaderFees[0].ToString()) + fOrFees_due;
                                            fSurchPen += (fOrFees_surch + fOrFees_pen);
                                            fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                            //}
                                        }
                                    }
                                    pSet2.Close();

                                    for (ii = 1; ii <= iCtr1; ii++)
                                    {
                                        sFeesCodeX = arr_sFeesCode[ii].ToString();

                                        sQuery = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                        sQuery += " where or_no = '" + sOrNo + "'";
                                        sQuery += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";

                                        pSet2.Query = sQuery;
                                        if (pSet2.Execute())
                                        {
                                            if (pSet2.Read())
                                            //if (pSet2.GetDouble("fees_amtdue").ToString() != null)
                                            {
                                                fOrFees_due = pSet2.GetDouble("fees_due");
                                                fOrFees_surch = pSet2.GetDouble("fees_surch");
                                                fOrFees_pen = pSet2.GetDouble("fees_pen");
                                                fOrFees_amtdue = pSet2.GetDouble("fees_amtdue");

                                                arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                                fSurchPen += (fOrFees_surch + fOrFees_pen);
                                                fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                            }
                                        }
                                        pSet2.Close();

                                        // RMC 20150126 (s)
                                        if (ii > 19)
                                        {
                                            OracleResultSet pTmp = new OracleResultSet();
                                            fOrFees_due = 0;
                                            fOrFees_surch = 0;
                                            fOrFees_pen = 0;
                                            fOrFees_amtdue = 0;

                                            int iTmpCtr = 0;
                                            pSet2.Query = "select * from tax_and_fees_table where rev_year = '" + m_sRevYear + "' order by fees_code";
                                            if (pSet2.Execute())
                                            {
                                                while (pSet2.Read())
                                                {
                                                    iTmpCtr++;
                                                    sFeesCodeX = pSet2.GetString("fees_code");

                                                    if (iTmpCtr > 19)
                                                    {
                                                        pTmp.Query = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                                        pTmp.Query += " where or_no = '" + sOrNo + "'";
                                                        pTmp.Query += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                                        if (pTmp.Execute())
                                                        {
                                                            if (pTmp.Read())
                                                            {
                                                                fOrFees_due += pTmp.GetDouble("fees_due");
                                                                fOrFees_surch += pTmp.GetDouble("fees_surch");
                                                                fOrFees_pen += pTmp.GetDouble("fees_pen");
                                                                fOrFees_amtdue += pTmp.GetDouble("fees_amtdue");
                                                            }
                                                        }
                                                        pTmp.Close();
                                                    }
                                                }

                                                arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                                fSurchPen += (fOrFees_surch + fOrFees_pen);
                                                fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                            }
                                            pSet2.Close();
                                        }
                                        // RMC 20150126 (e)
                                    }
                                }
                            }
                            pSet1.Close();
                        }

                        // RTL 04272006 (s){
                        //saving
                        if (sBrgyName.Trim() == "")
                            sBrgyName = "NO BARANGAY";

                        if (sOwnKind == "SINGLE PROPRIETORSHIP")
                            sOwnKind = "SINGLE PROP.";

                        sQuery = "insert into report_sumcoll values (";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sBrgyName.Trim(), 100)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnKind.Trim(), 100)) + "', '','',";
                        sQuery += " " + iNew + ", ";
                        sQuery += " " + fNewCapital.ToString("0.##") + ", ";
                        sQuery += " " + iRen + ", ";
                        sQuery += " " + fRenGross.ToString("0.##") + ", ";
                        sQuery += " " + iRet + ", ";
                        sQuery += " " + fRetGross.ToString("0.##") + ", ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[0].ToString().Trim(), 800)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[1].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[2].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[3].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[4].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[5].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[6].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[7].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[8].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[9].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[10].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[11].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[12].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[13].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[14].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[15].ToString().Trim(), 50)) + "', ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[0]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[1]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[2]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[3]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[4]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[5]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[6]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[7]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[8]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[9]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[10]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[11]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[12]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[13]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[14]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[15]).ToString("0.##") + ", ";
                        sQuery += "0, " + fSurchPen + ", " + fTotalFees + ", ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(dtpFrom.Value.ToString("MM/dd/yyyy")) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(dtpTo.Value.ToString("MM/dd/yyyy")) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 20)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(m_sRevYear) + "', ";
                        sQuery += "to_date('" + sCurrentDate + "', 'MM/dd/yyyy'), ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[16].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[17].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[18].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[19].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[20].ToString().Trim(), 50)) + "', ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[16]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[17]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[18]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[19]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[20]).ToString("0.##") + ")";

                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                    }

                    DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            pSet.Close();

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            OnPrintReportSummaryOfCollectionBarangay(m_sReportName);

            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

        private void OnReportWideSummaryOfCollectionBusinessStatus()
        {
            ArrayList arr_fHeaderFees = new ArrayList();
            ArrayList arr_sFeesCode = new ArrayList();
            ArrayList arr_sFeesDesc = new ArrayList();

            String sQuery, sBnsStatus = ""; //,sFeesCode[21],sFeesDesc[21]
            String sDistName, sOwnKind = "", sFeesCodeX, sOrNo, sBin, sBinV, sBinX;
            String sCurrentUser;
            int i, ii, iFees_cnt, iRen, iNew, iRet, iCtr1, iStatusCtr;
            double fOrFees_due, fOrFees_surch, fOrFees_pen, fOrFees_amtdue;
            double fRenGr = 0, fRenGross = 0, fRetGr = 0, fRetGross = 0, fNewCap = 0, fNewCapital = 0;
            double fSurchPen, fTotalFees, dStatusCap, dStatusGross; //fHeaderFees[21]

            String sFormatDistrict = "";

            fSurchPen = 0;
            fTotalFees = 0;

            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            pSet.Query = string.Format("DELETE FROM REPORT_SUMCOLL WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
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

            for (ii = 0; ii <= 21; ii++)
                arr_fHeaderFees.Add(0);

            iCtr1 = 0;
            i = 0;
            ii = 0;
            arr_sFeesCode.Add("B");
            arr_sFeesDesc.Add("LICENSE TAX");

            sQuery = string.Format("select fees_code,fees_desc from tax_and_fees_table where rev_year = '{0}' order by fees_code", m_sRevYear);
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    iCtr1 += 1;

                    if (iCtr1 <= 19)    // RMC 20150126
                    {
                        arr_sFeesCode.Add(pSet.GetString("fees_code").Trim());
                        arr_sFeesDesc.Add(pSet.GetString("fees_desc").Trim());
                    }
                }
            pSet.Close();

            // RMC 20150126 (S)
            if (iCtr1 > 19)
            {
                arr_sFeesCode.Add("00");
                arr_sFeesDesc.Add("OTHER FEES");
                iCtr1 = 20;
            }// RMC 20150126 (E)

            // RMC 20150505 QA reports (S)
            while (iCtr1 <= 20)
            {
                arr_sFeesCode.Add(" ");
                arr_sFeesDesc.Add(" ");
                iCtr1++;
            }
            // RMC 20150505 QA reports (E)
            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);

            if (m_iOptionFormat == 1)
            {
                sQuery = string.Format(@"select count(distinct pay_hist.or_no) from pay_hist,businesses
					where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy')
					order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"));
            }
            else
            {
                sQuery = string.Format(@"select count(distinct pay_hist.or_no) from pay_hist,businesses
					where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy')
					order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"));
            }
    
            pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            for (int iStat = 1; iStat <= 3; iStat++)
            {
                switch (iStat)
                {
                    case 1:
                        sBnsStatus = "NEW";
                        break;
                    case 2:
                        sBnsStatus = "REN";
                        break;
                    case 3:
                        sBnsStatus = "RET";
                        break;
                }

                for (i = 1; i <= 4; i++)
                {
                    iRen = 0;
                    iRet = 0;
                    iNew = 0;
                    fNewCap = 0;
                    fRenGross = 0;
                    fRetGross = 0;
                    fSurchPen = 0;
                    fTotalFees = 0;
                    iStatusCtr = 0;
                    dStatusCap = 0;
                    dStatusGross = 0;

                    arr_fHeaderFees.Clear();
                    for (ii = 0; ii <= iCtr1; ii++)
                        arr_fHeaderFees.Add(0);

                    switch (i)
                    {
                        case 1:
                            sOwnKind = "SINGLE PROPRIETORSHIP";
                            break;
                        case 2:
                            sOwnKind = "PARTNERSHIP";
                            break;
                        case 3:
                            sOwnKind = "CORPORATION";
                            break;
                        case 4:
                            sOwnKind = "COOPERATIVE";
                            break;
                    }

                    sBinX = "";

                    if (m_iOptionFormat == 1)
                    {
                        sQuery = string.Format(@"select distinct(pay_hist.or_no), pay_hist.bin,businesses.capital,businesses.gr_1 from pay_hist,businesses
					where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy') and
					businesses.bns_stat  = '{2}' and bns_dist = '{3}' and orgn_kind = '{4}'
					order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"), sBnsStatus, sFormatDistrict, sOwnKind);
                    }
                    else
                    {
                        sQuery = string.Format(@"select distinct(pay_hist.or_no), pay_hist.bin,businesses.capital,businesses.gr_1 from pay_hist,businesses
					where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy') and
					businesses.bns_stat  = '{2}' and orgn_kind = '{3}'
					order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"), sBnsStatus, sOwnKind);
                    }
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        while (pSet.Read())
                        {
                            DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                            intCountIncreament += 1;

                            sOrNo = pSet.GetString("or_no").Trim();
                            sBin = pSet.GetString("bin").Trim();

                            //(s) RTL 06092006 revise the loop
                            if (sBin != sBinX)
                            {
                                if (sBnsStatus == "NEW")
                                {
                                    iNew = iNew + 1;
                                    fNewCap = pSet.GetDouble("capital");
                                    fNewCapital = fNewCapital + fNewCap;
                                    iStatusCtr = iStatusCtr + 1;          // Added
                                    dStatusCap = dStatusCap + fNewCap;    // Added
                                }
                                if (sBnsStatus == "REN")
                                {
                                    iRen = iRen + 1;
                                    //fRenGr       = atof(pApp->GetStrVariant(pTable3->GetCollect("gr_1"))) + atof(pApp->GetStrVariant(pTable3->GetCollect("gr_2")));
                                    fRenGr = pSet.GetDouble("gr_1");
                                    fRenGross = fRenGross + fRenGr;
                                    iStatusCtr = iStatusCtr + 1;         // Added
                                    dStatusGross = dStatusGross + fRenGr;  // Added
                                }
                                if (sBnsStatus == "RET")
                                {
                                    iRet = iRet + 1;
                                    //fRetGr       = atof(pApp->GetStrVariant(pTable3->GetCollect("gr_1"))) + atof(pApp->GetStrVariant(pTable3->GetCollect("gr_2")));
                                    fRetGr = pSet.GetDouble("gr_1");
                                    fRetGross = fRetGross + fRetGr;
                                    iStatusCtr = iStatusCtr + 1;         // Added
                                    dStatusGross = dStatusGross + fRetGr;  // Added
                                }
                                sBinX = sBin;
                            }

                            sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no = '" + sOrNo + "' and fees_code like 'B%%' having sum(fees_amtdue) is not null");
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                            {
                                if (pSet1.Read())
                                {
                                    //if (pSet1.GetDouble("fees_amtdue").ToString() != null)
                                    //{
                                    fOrFees_due = pSet1.GetDouble("fees_due");
                                    fOrFees_surch = pSet1.GetDouble("fees_surch");
                                    fOrFees_pen = pSet1.GetDouble("fees_pen");
                                    fOrFees_amtdue = pSet1.GetDouble("fees_amtdue");

                                    arr_fHeaderFees[0] = Convert.ToDouble(arr_fHeaderFees[0].ToString()) + fOrFees_due;
                                    fSurchPen += (fOrFees_surch + fOrFees_pen);
                                    fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                    // }
                                }
                            }
                            pSet1.Close();

                            for (ii = 1; ii <= iCtr1; ii++)
                            {
                                sFeesCodeX = arr_sFeesCode[ii].ToString();

                                sQuery = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                sQuery += " where or_no = '" + sOrNo + "'";
                                sQuery += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                pSet1.Query = sQuery;
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                    {
                                        //if (pSet1.GetDouble("fees_amtdue").ToString() != null)
                                        //{
                                        fOrFees_due = pSet1.GetDouble("fees_due");
                                        fOrFees_surch = pSet1.GetDouble("fees_surch");
                                        fOrFees_pen = pSet1.GetDouble("fees_pen");
                                        fOrFees_amtdue = pSet1.GetDouble("fees_amtdue");

                                        arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                        fSurchPen += (fOrFees_surch + fOrFees_pen);
                                        fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                        // }
                                    }
                                pSet1.Close();

                                // RMC 20150126 (s)
                                if (ii > 19)
                                {
                                    OracleResultSet pTmp = new OracleResultSet();
                                    fOrFees_due = 0;
                                    fOrFees_surch = 0;
                                    fOrFees_pen = 0;
                                    fOrFees_amtdue = 0;

                                    int iTmpCtr = 0;
                                    pSet2.Query = "select * from tax_and_fees_table where rev_year = '" + m_sRevYear + "' order by fees_code";
                                    if (pSet2.Execute())
                                    {
                                        while (pSet2.Read())
                                        {
                                            iTmpCtr++;
                                            sFeesCodeX = pSet2.GetString("fees_code");

                                            if (iTmpCtr > 19)
                                            {
                                                pTmp.Query = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                                pTmp.Query += " where or_no = '" + sOrNo + "'";
                                                pTmp.Query += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                                if (pTmp.Execute())
                                                {
                                                    if (pTmp.Read())
                                                    {
                                                        fOrFees_due += pTmp.GetDouble("fees_due");
                                                        fOrFees_surch += pTmp.GetDouble("fees_surch");
                                                        fOrFees_pen += pTmp.GetDouble("fees_pen");
                                                        fOrFees_amtdue += pTmp.GetDouble("fees_amtdue");
                                                    }
                                                }
                                                pTmp.Close();
                                            }
                                        }

                                        arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                        fSurchPen += (fOrFees_surch + fOrFees_pen);
                                        fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                    }
                                    pSet2.Close();
                                }
                                // RMC 20150126 (e)
                            }

                            DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                            Thread.Sleep(3);
                        }
                    pSet.Close();

                    //saving

                    if (sFormatDistrict == "")
                        sDistName = "NO DISTRICT";

                    if (sOwnKind == "SINGLE PROPRIETORSHIP")
                        sOwnKind = "SINGLE PROP.";

                    sQuery = "insert into report_sumcoll values (";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsStatus.Trim(), 100)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnKind.Trim(), 100)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sFormatDistrict.Trim(), 100)) + "', '',";
                    sQuery += " " + iStatusCtr + ", ";
                    sQuery += " " + dStatusCap.ToString("0.##") + ", '0', ";
                    sQuery += " " + dStatusGross.ToString("0.##") + ", '0', '0', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[0].ToString().Trim(), 800)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[1].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[2].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[3].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[4].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[5].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[6].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[7].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[8].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[9].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[10].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[11].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[12].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[13].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[14].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[15].ToString().Trim(), 50)) + "', ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[0]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[1]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[2]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[3]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[4]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[5]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[6]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[7]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[8]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[9]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[10]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[11]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[12]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[13]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[14]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[15]).ToString("0.##") + ", '0',";
                    sQuery += " " + fSurchPen + ", " + fTotalFees + ", ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(dtpFrom.Value.ToString("MM/dd/yyyy")) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(dtpTo.Value.ToString("MM/dd/yyyy")) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 20)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(m_sRevYear) + "', ";
                    sQuery += " to_date('" + sCurrentDate + "','MM/dd/yyyy'), ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[16].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[17].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[18].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[19].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[20].ToString().Trim(), 50)) + "', ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[16]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[17]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[18]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[19]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[20]).ToString("0.##") + ")";

                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
            }
            
            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            OnPrintReportWideSummaryOfCollectionBusinessStatus(m_sReportName);

            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

        private void OnReportWideSummaryOfCollectionDistrict()
        {
            String sQuery;//,sFeesCode[21],sFeesDesc[21];
            String sDistName, sOwnKind = "", sFeesCodeX, sOrNo, sBin, sBinV, sBinX;
            String sCurrentUser;
            int i, ii, iFees_cnt, iRen, iNew, iRet, iProgressCtr = 0, iCtr1;
            double fOrFees_due, fOrFees_surch, fOrFees_pen, fOrFees_amtdue;
            double fRenGr = 0, fRenGross = 0, fRetGr = 0, fRetGross = 0, fNewCap = 0, fNewCapital = 0;
            double fSurchPen, fTotalFees; //fHeaderFees[21]

            ArrayList arr_fHeaderFees = new ArrayList();
            ArrayList arr_sFeesCode = new ArrayList();
            ArrayList arr_sFeesDesc = new ArrayList();

            fSurchPen = 0;
            fTotalFees = 0;

            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            pSet.Query = string.Format("DELETE FROM REPORT_SUMCOLL WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
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

            for (ii = 0; ii <= 21; ii++)
                arr_fHeaderFees.Add(0);

            iCtr1 = 0;
            i = 0;
            ii = 0;
            arr_sFeesCode.Add("B");
            arr_sFeesDesc.Add("LICENSE TAX");

            sQuery = string.Format("select fees_code,fees_desc from tax_and_fees_table where rev_year = '{0}' order by fees_code", m_sRevYear);
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    iCtr1 += 1;

                    if (iCtr1 <= 19)    // RMC 20150126
                    {
                        arr_sFeesCode.Add(pSet.GetString("fees_code").Trim());
                        arr_sFeesDesc.Add(pSet.GetString("fees_desc").Trim());
                    }
                }
            pSet.Close();

            // RMC 20150126 (S)
            if (iCtr1 > 19)
            {
                arr_sFeesCode.Add("00");
                arr_sFeesDesc.Add("OTHER FEES");
                iCtr1 = 20;
            }// RMC 20150126 (E)

            pSet.Query = "select distinct bns_dist from businesses order by bns_dist";	// Include No District
            if (pSet.Execute())
                while (pSet.Read())
                {
                    iProgressCtr = iProgressCtr + 1;

                    sDistName = pSet.GetString("bns_dist").Trim();		// Include No District

                    String sBnsStatus = "";

                    for (int iStat = 1; iStat <= 4; iStat++)
                    {
                        switch (iStat)
                        {
                            case 1:
                                sOwnKind = "SINGLE PROPRIETORSHIP";
                                break;
                            case 2:
                                sOwnKind = "PARTNERSHIP";
                                break;
                            case 3:
                                sOwnKind = "CORPORATION";
                                break;
                            case 4:
                                sOwnKind = "COOPERATIVE";
                                break;
                        }

                        iRen = 0;
                        iRet = 0;
                        iNew = 0;
                        fNewCapital = 0;
                        fRenGross = 0;
                        fRetGross = 0;
                        fSurchPen = 0;
                        fTotalFees = 0;

                        int iStatusCtr = 0;
                        double dStatusCap = 0;
                        double dStatusGross = 0;

                        for (i = 1; i <= 3; i++)
                        {
                            switch (i)
                            {
                                case 1:
                                    sBnsStatus = "NEW";
                                    break;
                                case 2:
                                    sBnsStatus = "REN";
                                    break;
                                case 3:
                                    sBnsStatus = "RET";
                                    break;
                            }

                            sBinX = "";

                            sQuery = string.Format(@"select distinct(pay_hist.or_no), pay_hist.bin from pay_hist,businesses
							 where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy') and
							 businesses.bns_dist  = '{2}' and businesses.orgn_kind = '{3}'
						     order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"), sDistName, sOwnKind);

                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                            {
                                while (pSet1.Read())
                                {
                                    sOrNo = pSet1.GetString("or_no").Trim();
                                    sBin = pSet1.GetString("bin").Trim();

                                    sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no = '" + sOrNo + "' and fees_code like 'B%%' having sum(fees_amtdue) is not null");
                                    pSet2.Query = sQuery;
                                    if (pSet2.Execute())
                                    {
                                        if (pSet2.Read())
                                        {
                                            //if (pSet2.GetDouble("fees_amtdue").ToString() != null)
                                            //{
                                            fOrFees_due = pSet2.GetDouble("fees_due");
                                            fOrFees_surch = pSet2.GetDouble("fees_surch");
                                            fOrFees_pen = pSet2.GetDouble("fees_pen");
                                            fOrFees_amtdue = pSet2.GetDouble("fees_amtdue");

                                            arr_fHeaderFees[0] = Convert.ToDouble(arr_fHeaderFees[0].ToString()) + fOrFees_due;
                                            fSurchPen += (fOrFees_surch + fOrFees_pen);
                                            fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                            //}
                                        }
                                    }
                                    pSet2.Close();

                                    for (ii = 1; ii <= iCtr1; ii++)
                                    {
                                        sFeesCodeX = arr_sFeesCode[ii].ToString();

                                        sQuery = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                        sQuery += " where or_no = '" + sOrNo + "'";
                                        sQuery += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";

                                        pSet2.Query = sQuery;
                                        if (pSet2.Execute())
                                        {
                                            if (pSet2.Read())
                                            //if (pSet2.GetDouble("fees_amtdue").ToString() != null)
                                            {
                                                fOrFees_due = pSet2.GetDouble("fees_due");
                                                fOrFees_surch = pSet2.GetDouble("fees_surch");
                                                fOrFees_pen = pSet2.GetDouble("fees_pen");
                                                fOrFees_amtdue = pSet2.GetDouble("fees_amtdue");

                                                arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                                fSurchPen += (fOrFees_surch + fOrFees_pen);
                                                fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                            }
                                        }
                                        pSet2.Close();

                                        // RMC 20150126 (s)
                                        if (ii > 19)
                                        {
                                            OracleResultSet pTmp = new OracleResultSet();
                                            fOrFees_due = 0;
                                            fOrFees_surch = 0;
                                            fOrFees_pen = 0;
                                            fOrFees_amtdue = 0;

                                            int iTmpCtr = 0;
                                            pSet2.Query = "select * from tax_and_fees_table where rev_year = '" + m_sRevYear + "' order by fees_code";
                                            if (pSet2.Execute())
                                            {
                                                while (pSet2.Read())
                                                {
                                                    iTmpCtr++;
                                                    sFeesCodeX = pSet2.GetString("fees_code");

                                                    if (iTmpCtr > 19)
                                                    {
                                                        pTmp.Query = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                                        pTmp.Query += " where or_no = '" + sOrNo + "'";
                                                        pTmp.Query += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                                        if (pTmp.Execute())
                                                        {
                                                            if (pTmp.Read())
                                                            {
                                                                fOrFees_due += pTmp.GetDouble("fees_due");
                                                                fOrFees_surch += pTmp.GetDouble("fees_surch");
                                                                fOrFees_pen += pTmp.GetDouble("fees_pen");
                                                                fOrFees_amtdue += pTmp.GetDouble("fees_amtdue");
                                                            }
                                                        }
                                                        pTmp.Close();
                                                    }
                                                }

                                                arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                                fSurchPen += (fOrFees_surch + fOrFees_pen);
                                                fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                            }
                                            pSet2.Close();
                                        }
                                        // RMC 20150126 (e)
                                    }

                                    //REN
                                    sQuery = "select distinct businesses.bin, businesses.gr_1, businesses.gr_2 from businesses,pay_hist";
                                    sQuery += " where pay_hist.or_no = '" + sOrNo + "'";
                                    sQuery += " and pay_hist.bin = businesses.bin and businesses.bns_stat = 'REN'";

                                    pSet2.Query = sQuery;
                                    if (pSet2.Execute())
                                        if (pSet2.Read())
                                        {
                                            sBinV = pSet2.GetString("bin").Trim();
                                            if (sBinV != sBinX)
                                            {
                                                iRen = iRen + 1;
                                                fRenGr = pSet2.GetDouble("gr_1") + pSet2.GetDouble("gr_2");
                                                fRenGross = fRenGross + fRenGr;
                                            }
                                        }
                                    pSet2.Close();

                                    //RET
                                    sQuery = "select distinct businesses.bin, businesses.gr_1, businesses.gr_2 from businesses,pay_hist";
                                    sQuery += " where pay_hist.or_no = '" + sOrNo + "'";
                                    sQuery += " and pay_hist.bin = businesses.bin and businesses.bns_stat = 'RET'";
                                    pSet2.Query = sQuery;
                                    if (pSet2.Execute())
                                        if (pSet2.Read())
                                        {
                                            sBinV = pSet2.GetString("bin").Trim();
                                            if (sBinV != sBinX)
                                            {
                                                iRen = iRen + 1;
                                                fRenGr = pSet2.GetDouble("gr_1") + pSet2.GetDouble("gr_2");
                                                fRenGross = fRenGross + fRenGr;
                                            }
                                        }
                                    pSet2.Close();

                                    //NEW
                                    sQuery = "select distinct businesses.bin, businesses.capital from businesses,pay_hist";
                                    sQuery += " where pay_hist.or_no = '" + sOrNo + "'";
                                    sQuery += " and pay_hist.bin = businesses.bin and businesses.bns_stat = 'NEW'";
                                    pSet2.Query = sQuery;
                                    if (pSet2.Execute())
                                        if (pSet2.Read())
                                        {
                                            sBinV = pSet2.GetString("bin").Trim();
                                            if (sBinV != sBinX)
                                            {
                                                iNew = iNew + 1;
                                                fNewCap = pSet2.GetDouble("capital");
                                                fNewCapital = fNewCapital + fNewCap;
                                            }
                                        }
                                    pSet2.Close();
                                }
                            }
                            pSet1.Close();
                        }

                        if (sDistName.Trim() == "")
                            sDistName = "NO DISTRICT";

                        if (sOwnKind == "SINGLE PROPRIETORSHIP")
                            sOwnKind = "SINGLE PROP.";

                        sQuery = string.Format(@"insert into report_sumcoll values ('{0}','{1}','','','{2}','{3}','{4}','{5}','{6}','{7}',
					       '{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}',
						   '{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}',
				           '{40}','{41}',
						   '{42}','{43}',
                           '{44}','{45}','{46}','{47}','{48}',to_date('{49}','MM/dd/yyyy'),
                           '{50}','{51}','{52}','{53}','{58}','{54}','{55}','{56}','{57}','{59}')",
                           StringUtilities.HandleApostrophe(StringUtilities.Left(sDistName.Trim(), 100)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnKind.Trim(), 100)),
                           iNew,
                           fNewCap.ToString("0.##"),
                           iRen,
                           fRenGross.ToString("0.##"),
                           iRet,
                           fRetGross.ToString("0.##"),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[0].ToString().Trim(), 800)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[1].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[2].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[3].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[4].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[5].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[6].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[7].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[8].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[9].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[10].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[11].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[12].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[13].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[14].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[15].ToString().Trim(), 50)),
                           Convert.ToDouble(arr_fHeaderFees[0]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[1]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[2]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[3]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[4]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[5]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[6]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[7]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[8]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[9]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[10]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[11]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[12]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[13]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[14]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[15]).ToString("0.##"),
                           fSurchPen,
                           fTotalFees,
                           StringUtilities.HandleApostrophe(dtpFrom.Value.ToString("MM/dd/yyyy")),
                           StringUtilities.HandleApostrophe(dtpTo.Value.ToString("MM/dd/yyyy")),
                           StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 20)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 50)),
                           StringUtilities.HandleApostrophe(m_sRevYear),
                           sCurrentDate,
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[16].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[17].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[18].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[19].ToString().Trim(), 50)),
                           Convert.ToDouble(arr_fHeaderFees[16]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[17]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[18]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[19]).ToString("0.##"),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[20].ToString().Trim(), 50)), // RMC 20150126
                           Convert.ToDouble(arr_fHeaderFees[20]).ToString("0.##"));
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                    }
                }
            pSet.Close();

            OnPrintReportWideSummaryOfCollectionDistrict(m_sReportName);

            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

        private void OnReportWideSummaryOfCollectionLineOfBusiness()
        {
            String sQuery, sBnsCode, sBnsDesc, sBnsStatus = "", sXBnsCode; //,sFeesCode[21],sFeesDesc[21]
            String sDistName = "", sOwnKind = "", sFeesCodeX, sOrNo, sBin, sBinV, sBinX;
            String sCurrentUser;
            int i, ii, iFees_cnt, iRen, iNew, iRet, iCtr1, iStatusCtr;
            double fOrFees_due, fOrFees_surch, fOrFees_pen, fOrFees_amtdue;
            double fRenGr = 0, fRenGross = 0, fRetGr = 0, fRetGross = 0, fNewCap = 0, fNewCapital = 0;
            double fSurchPen, fTotalFees, dStatusCap, dStatusGross; //fHeaderFees[21],

            ArrayList arr_fHeaderFees = new ArrayList();
            ArrayList arr_sFeesCode = new ArrayList();
            ArrayList arr_sFeesDesc = new ArrayList();

            fSurchPen = 0;
            fTotalFees = 0;

            //Used for format
            String sLineCode, sLineDesc;
            sLineCode = "";
            sLineDesc = cmbBusType.Text;

            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            OracleResultSet pSet2 = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = string.Format("DELETE FROM REPORT_SUMCOLL WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
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

            for (ii = 0; ii <= 21; ii++)
                arr_fHeaderFees.Add(0);

            iCtr1 = 0;
            i = 0;
            ii = 0;
            arr_sFeesCode.Add("B");
            arr_sFeesDesc.Add("LICENSE TAX");

            sQuery = string.Format("select fees_code,fees_desc from tax_and_fees_table where rev_year = '{0}' order by fees_code", m_sRevYear);
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    iCtr1 += 1;

                    if (iCtr1 <= 19)    // RMC 20150126
                    {
                        arr_sFeesCode.Add(pSet.GetString("fees_code").Trim());
                        arr_sFeesDesc.Add(pSet.GetString("fees_desc").Trim());
                    }
                }
            pSet.Close();

            // RMC 20150126 (S)
            if (iCtr1 > 19)
            {
                arr_sFeesCode.Add("00");
                arr_sFeesDesc.Add("OTHER FEES");
                iCtr1 = 20;
            }// RMC 20150126 (E)

            // RMC 20150505 QA reports (S)
            while (iCtr1 <= 20)
            {
                arr_sFeesCode.Add(" ");
                arr_sFeesDesc.Add(" ");
                iCtr1++;
            }
            // RMC 20150505 QA reports (E)

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            sQuery = string.Format("select Count(*) from bns_table where rtrim(fees_code) like 'B%%' and bns_code like '%{0}%' and length(rtrim(bns_code)) > 2", sLineCode);
            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

            #endregion

            pSet.Query = string.Format("select bns_code,bns_desc from bns_table where rtrim(fees_code) like 'B%%' and bns_code like '%{0}%' and length(rtrim(bns_code)) > 2", sLineCode);
            if (pSet.Execute())
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sBnsCode = pSet.GetString("bns_code").Trim();
                    sBnsDesc = pSet.GetString("bns_desc").Trim();

                    for (i = 1; i <= 3; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                sBnsStatus = "NEW";
                                break;
                            case 2:
                                sBnsStatus = "REN";
                                break;
                            case 3:
                                sBnsStatus = "RET";
                                break;
                        }

                        iRen = 0;
                        iRet = 0;
                        iNew = 0;
                        fNewCap = 0;
                        fRenGross = 0;
                        fRetGross = 0;
                        fSurchPen = 0;
                        fTotalFees = 0;
                        iStatusCtr = 0;
                        dStatusCap = 0;
                        dStatusGross = 0;
                        arr_fHeaderFees.Clear();
                        for (ii = 0; ii <= iCtr1; ii++)
                            arr_fHeaderFees.Add(0);

                        sXBnsCode = sBnsCode;
                        sBinX = "";

                        sQuery = string.Format(@"select distinct(pay_hist.or_no), pay_hist.bin,businesses.capital,businesses.gr_1 from pay_hist,businesses
						where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy') and
						businesses.bns_stat  = '{2}' and businesses.bns_code like '{3}'
						order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"), sBnsStatus, sXBnsCode);

                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            while (pSet1.Read())
                            {
                                sOrNo = pSet1.GetString("or_no").Trim();
                                sBin = pSet1.GetString("bin").Trim();

                                if (sBin != sBinX)
                                {
                                    if (sBnsStatus == "NEW")
                                    {
                                        iNew = iNew + 1;
                                        fNewCap = pSet1.GetDouble("capital");
                                        fNewCapital = fNewCapital + fNewCap;
                                        iStatusCtr = iStatusCtr + 1;          // Added
                                        dStatusCap = dStatusCap + fNewCap;    // Added
                                    }
                                    if (sBnsStatus == "REN")
                                    {
                                        iRen = iRen + 1;
                                        fRenGr = pSet1.GetDouble("gr_1");
                                        fRenGross = fRenGross + fRenGr;
                                        iStatusCtr = iStatusCtr + 1;         // Added
                                        dStatusGross = dStatusGross + fRenGr;  // Added
                                    }
                                    if (sBnsStatus == "RET")
                                    {
                                        iRet = iRet + 1;
                                        fRetGr = pSet1.GetDouble("gr_1");
                                        fRetGross = fRetGross + fRetGr;
                                        iStatusCtr = iStatusCtr + 1;         // Added
                                        dStatusGross = dStatusGross + fRetGr;  // Added
                                    }
                                    sBinX = sBin;
                                }

                                sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no = '" + sOrNo + "' and fees_code like 'B%%' having sum(fees_amtdue) is not null");

                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                    if (pSet2.Read())
                                    {
                                        //if (pSet2.GetDouble("fees_amtdue").ToString() != null)
                                        //{
                                        fOrFees_due = pSet2.GetDouble("fees_due");
                                        fOrFees_surch = pSet2.GetDouble("fees_surch");
                                        fOrFees_pen = pSet2.GetDouble("fees_pen");
                                        fOrFees_amtdue = pSet2.GetDouble("fees_amtdue");

                                        arr_fHeaderFees[0] = Convert.ToDouble(arr_fHeaderFees[0].ToString()) + fOrFees_due;
                                        fSurchPen += (fOrFees_surch + fOrFees_pen);
                                        fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                        //}
                                    }
                                pSet2.Close();

                                for (ii = 1; ii <= iCtr1; ii++)
                                {
                                    sFeesCodeX = arr_sFeesCode[ii].ToString();

                                    sQuery = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                    sQuery += " where or_no = '" + sOrNo + "'";
                                    sQuery += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";

                                    pSet2.Query = sQuery;
                                    if (pSet2.Execute())
                                    {
                                        if (pSet2.Read())
                                        {
                                            //if (pSet2.GetDouble("fees_amtdue").ToString() != null)
                                            //{
                                            fOrFees_due = pSet2.GetDouble("fees_due");
                                            fOrFees_surch = pSet2.GetDouble("fees_surch");
                                            fOrFees_pen = pSet2.GetDouble("fees_pen");
                                            fOrFees_amtdue = pSet2.GetDouble("fees_amtdue");

                                            arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                            fSurchPen += (fOrFees_surch + fOrFees_pen);
                                            fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                            //}
                                        }
                                    }
                                    pSet2.Close();

                                    // RMC 20150126 (s)
                                    if (ii > 19)
                                    {
                                        OracleResultSet pTmp = new OracleResultSet();
                                        fOrFees_due = 0;
                                        fOrFees_surch = 0;
                                        fOrFees_pen = 0;
                                        fOrFees_amtdue = 0;

                                        int iTmpCtr = 0;
                                        pSet2.Query = "select * from tax_and_fees_table where rev_year = '" + m_sRevYear + "' order by fees_code";
                                        if (pSet2.Execute())
                                        {
                                            while (pSet2.Read())
                                            {
                                                iTmpCtr++;
                                                sFeesCodeX = pSet2.GetString("fees_code");

                                                if (iTmpCtr > 19)
                                                {
                                                    pTmp.Query = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                                    pTmp.Query += " where or_no = '" + sOrNo + "'";
                                                    pTmp.Query += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                                    if (pTmp.Execute())
                                                    {
                                                        if (pTmp.Read())
                                                        {
                                                            fOrFees_due += pTmp.GetDouble("fees_due");
                                                            fOrFees_surch += pTmp.GetDouble("fees_surch");
                                                            fOrFees_pen += pTmp.GetDouble("fees_pen");
                                                            fOrFees_amtdue += pTmp.GetDouble("fees_amtdue");
                                                        }
                                                    }
                                                    pTmp.Close();
                                                }
                                            }

                                            arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                            fSurchPen += (fOrFees_surch + fOrFees_pen);
                                            fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                        }
                                        pSet2.Close();
                                    }
                                    // RMC 20150126 (e)
                                }
                            }
                        pSet1.Close();

                        if (sDistName.Trim() == "")
                            sDistName = "NO DISTRICT";

                        if (sOwnKind == "SINGLE PROPRIETORSHIP")
                            sOwnKind = "SINGLE PROP.";

                        sQuery = "insert into report_sumcoll values (";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsDesc.Trim(), 100)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsStatus.Trim(), 100)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sLineDesc.Trim(), 100)) + "', '',";
                        sQuery += " " + iStatusCtr + ", ";
                        sQuery += " " + dStatusCap.ToString("0.##") + ", '0', ";
                        sQuery += " " + dStatusGross.ToString("0.##") + ", '0','0', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[0].ToString().Trim(), 800)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[1].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[2].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[3].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[4].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[5].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[6].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[7].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[8].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[9].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[10].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[11].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[12].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[13].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[14].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[15].ToString().Trim(), 50)) + "', ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[0]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[1]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[2]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[3]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[4]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[5]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[6]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[7]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[8]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[9]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[10]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[11]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[12]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[13]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[14]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[15]).ToString("0.##") + ", ";
                        sQuery += "0, " + fSurchPen + ", " + fTotalFees + ", ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(dtpFrom.Value.ToString("MM/dd/yyyy")) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(dtpTo.Value.ToString("MM/dd/yyyy")) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 20)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(m_sRevYear) + "', ";
                        sQuery += "to_date('" + sCurrentDate + "', 'MM/dd/yyyy'), ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[16].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[17].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[18].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[19].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[20].ToString().Trim(), 50)) + "', ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[16]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[17]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[18]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[19]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[20]).ToString("0.##") + ")";
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                    }

                    DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                }
            pSet.Close();
            
            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);
            OnPrintReportWideSummaryOfCollectionLineOfBusiness(m_sReportName);

            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

        private void OnReportWideSummaryOfCollectionMainBusiness()
        {
            String sQuery, sBnsCode, sBnsDesc, sBnsStatus = "", sXBnsCode; //,sFeesCode[21],sFeesDesc[21]
            String sDistName = "", sOwnKind = "", sFeesCodeX, sOrNo, sBin, sBinV, sBinX;
            String sCurrentUser;
            int i, ii, iFees_cnt, iRen, iNew, iRet, iProgressCtr = 0, iCtr1, iStatusCtr;
            double fOrFees_due, fOrFees_surch, fOrFees_pen, fOrFees_amtdue;
            double fRenGr = 0, fRenGross = 0, fRetGr = 0, fRetGross = 0, fNewCap = 0, fNewCapital = 0;
            double fSurchPen, fTotalFees, dStatusCap, dStatusGross; //fHeaderFees[21]

            ArrayList arr_fHeaderFees = new ArrayList();
            ArrayList arr_sFeesCode = new ArrayList();
            ArrayList arr_sFeesDesc = new ArrayList();

            fSurchPen = 0;
            fTotalFees = 0;

            //Used for format
            String sFormatDistrict = cmbDist.Text;

            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            pSet.Query = string.Format("DELETE FROM REPORT_SUMCOLL WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
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

            for (ii = 0; ii <= 21; ii++)
                arr_fHeaderFees.Add(0);

            iCtr1 = 0;
            i = 0;
            ii = 0;
            arr_sFeesCode.Add("B");
            arr_sFeesDesc.Add("LICENSE TAX");

            sQuery = string.Format("select fees_code,fees_desc from tax_and_fees_table where rev_year = '{0}' order by fees_code", m_sRevYear);
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    iCtr1 += 1;

                    if (iCtr1 <= 19)    // RMC 20150126
                    {
                        arr_sFeesCode.Add(pSet.GetString("fees_code").Trim());
                        arr_sFeesDesc.Add(pSet.GetString("fees_desc").Trim());
                    }
                }
            pSet.Close();

            // RMC 20150126 (S)
            if (iCtr1 > 19)
            {
                arr_sFeesCode.Add("00");
                arr_sFeesDesc.Add("OTHER FEES");
                iCtr1 = 20;
            }// RMC 20150126 (E)

            // RMC 20150505 QA reports (S)
            while (iCtr1 <= 20)
            {
                arr_sFeesCode.Add(" ");
                arr_sFeesDesc.Add(" ");
                iCtr1++;
            }
            // RMC 20150505 QA reports (E)
            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            pSet.Query = "select count(*) from bns_table where rtrim(fees_code) like 'B%%' and length(rtrim(bns_code)) = 2 order by bns_code";
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            pSet.Query = string.Format("select bns_code,bns_desc from bns_table where rtrim(fees_code) like 'B%%' and length(rtrim(bns_code)) = 2 order by bns_code");
            if (pSet.Execute())
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sBnsCode = pSet.GetString("bns_code").Trim();
                    sBnsDesc = pSet.GetString("bns_desc").Trim();

                    for (i = 1; i <= 3; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                sBnsStatus = "NEW";
                                break;
                            case 2:
                                sBnsStatus = "REN";
                                break;
                            case 3:
                                sBnsStatus = "RET";
                                break;
                        }

                        iRen = 0;
                        iRet = 0;
                        iNew = 0;
                        fNewCap = 0;
                        fRenGross = 0;
                        fRetGross = 0;
                        fSurchPen = 0;
                        fTotalFees = 0;
                        iStatusCtr = 0;
                        dStatusCap = 0;
                        dStatusGross = 0;

                        arr_fHeaderFees.Clear();
                        for (ii = 0; ii <= iCtr1; ii++)
                            arr_fHeaderFees.Add(0);

                        sXBnsCode = sBnsCode + "%%";
                        sBinX = "";
                        if (m_iOptionFormat == 2 || m_iOptionFormat == 3)
                        {
                            sQuery = string.Format(@"select distinct(pay_hist.or_no), pay_hist.bin, businesses.capital, businesses.gr_1 from pay_hist,businesses
							where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy') and
							businesses.bns_stat  = '{2}' and businesses.bns_code like '{3}' and bns_dist = '{4}'
							order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"), sBnsStatus, sXBnsCode, sFormatDistrict);
                        }
                        else
                        {
                            sQuery = string.Format(@"select distinct(pay_hist.or_no), pay_hist.bin, businesses.capital, businesses.gr_1 from pay_hist,businesses
							where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy') and
							businesses.bns_stat  = '{2}' and businesses.bns_code like '{3}'
							order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"), sBnsStatus, sXBnsCode);
                        }

                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            while (pSet1.Read())
                            {
                                sOrNo = pSet1.GetString("or_no").Trim();
                                sBin = pSet1.GetString("bin").Trim();

                                if (sBin != sBinX)
                                {
                                    if (sBnsStatus == "NEW")
                                    {
                                        iNew = iNew + 1;
                                        fNewCap = pSet1.GetDouble("capital");
                                        fNewCapital = fNewCapital + fNewCap;
                                        iStatusCtr = iStatusCtr + 1;          // Added
                                        dStatusCap = dStatusCap + fNewCap;    // Added
                                    }
                                    if (sBnsStatus == "REN")
                                    {
                                        iRen = iRen + 1;
                                        fRenGr = pSet1.GetDouble("gr_1");
                                        fRenGross = fRenGross + fRenGr;
                                        iStatusCtr = iStatusCtr + 1;         // Added
                                        dStatusGross = dStatusGross + fRenGr;  // Added
                                    }
                                    if (sBnsStatus == "RET")
                                    {
                                        iRet = iRet + 1;
                                        fRetGr = pSet1.GetDouble("gr_1");
                                        fRetGross = fRetGross + fRetGr;
                                        iStatusCtr = iStatusCtr + 1;         // Added
                                        dStatusGross = dStatusGross + fRetGr;  // Added
                                    }
                                    sBinX = sBin;
                                }

                                sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no = '" + sOrNo + "' and fees_code like 'B%%' having sum(fees_amtdue) is not null");
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                    if (pSet2.Read())
                                    {
                                        //if (pSet2.GetDouble("fees_amtdue").ToString() != null)
                                        //{
                                        fOrFees_due = pSet2.GetDouble("fees_due");
                                        fOrFees_surch = pSet2.GetDouble("fees_surch");
                                        fOrFees_pen = pSet2.GetDouble("fees_pen");
                                        fOrFees_amtdue = pSet2.GetDouble("fees_amtdue");

                                        arr_fHeaderFees[0] = Convert.ToDouble(arr_fHeaderFees[0].ToString()) + fOrFees_due;
                                        fSurchPen += (fOrFees_surch + fOrFees_pen);
                                        fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                        //}
                                    }
                                pSet2.Close();

                                for (ii = 1; ii <= iCtr1; ii++)
                                {
                                    sFeesCodeX = arr_sFeesCode[ii].ToString();

                                    sQuery = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                    sQuery += " where or_no = '" + sOrNo + "'";
                                    sQuery += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                    pSet2.Query = sQuery;
                                    if (pSet2.Execute())
                                    {
                                        if (pSet2.Read())
                                        {
                                            //if (pSet2.GetDouble("fees_amtdue").ToString() != null)
                                            //{
                                            fOrFees_due = pSet2.GetDouble("fees_due");
                                            fOrFees_surch = pSet2.GetDouble("fees_surch");
                                            fOrFees_pen = pSet2.GetDouble("fees_pen");
                                            fOrFees_amtdue = pSet2.GetDouble("fees_amtdue");

                                            arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                            fSurchPen += (fOrFees_surch + fOrFees_pen);
                                            fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                            //}
                                        }
                                    }
                                    pSet2.Close();

                                    // RMC 20150126 (s)
                                    if (ii > 19)
                                    {
                                        OracleResultSet pTmp = new OracleResultSet();
                                        fOrFees_due = 0;
                                        fOrFees_surch = 0;
                                        fOrFees_pen = 0;
                                        fOrFees_amtdue = 0;

                                        int iTmpCtr = 0;
                                        pSet2.Query = "select * from tax_and_fees_table where rev_year = '" + m_sRevYear + "' order by fees_code";
                                        if (pSet2.Execute())
                                        {
                                            while (pSet2.Read())
                                            {
                                                iTmpCtr++;
                                                sFeesCodeX = pSet2.GetString("fees_code");

                                                if (iTmpCtr > 19)
                                                {
                                                    pTmp.Query = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                                    pTmp.Query += " where or_no = '" + sOrNo + "'";
                                                    pTmp.Query += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                                    if (pTmp.Execute())
                                                    {
                                                        if (pTmp.Read())
                                                        {
                                                            fOrFees_due += pTmp.GetDouble("fees_due");
                                                            fOrFees_surch += pTmp.GetDouble("fees_surch");
                                                            fOrFees_pen += pTmp.GetDouble("fees_pen");
                                                            fOrFees_amtdue += pTmp.GetDouble("fees_amtdue");
                                                        }
                                                    }
                                                    pTmp.Close();
                                                }
                                            }

                                            arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                            fSurchPen += (fOrFees_surch + fOrFees_pen);
                                            fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                        }
                                        pSet2.Close();
                                    }
                                    // RMC 20150126 (e)
                                }
                            }
                        pSet1.Close();

                        if (sDistName.Trim() == "")
                            sDistName = "NO DISTRICT";

                        if (sOwnKind == "SINGLE PROPRIETORSHIP")
                            sOwnKind = "SINGLE PROP.";

                        sQuery = "insert into report_sumcoll values (";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsDesc.Trim(), 100)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsStatus.Trim(), 100)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sFormatDistrict.Trim(), 100)) + "', '',";
                        sQuery += " " + iStatusCtr + ", ";
                        sQuery += " " + dStatusCap.ToString("0.##") + ", 0, " + dStatusGross.ToString("0.##") + ",0,0,";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[0].ToString().Trim(), 800)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[1].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[2].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[3].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[4].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[5].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[6].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[7].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[8].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[9].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[10].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[11].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[12].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[13].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[14].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[15].ToString().Trim(), 50)) + "', ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[0]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[1]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[2]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[3]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[4]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[5]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[6]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[7]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[8]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[9]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[10]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[11]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[12]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[13]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[14]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[15]).ToString("0.##") + ", 0, ";
                        sQuery += " " + fSurchPen + ",";
                        sQuery += " " + fTotalFees + ",";
                        sQuery += "'" + StringUtilities.HandleApostrophe(dtpFrom.Value.ToString("MM/dd/yyyy")) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(dtpTo.Value.ToString("MM/dd/yyyy")) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 20)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(m_sRevYear) + "', ";
                        sQuery += "to_date('" + sCurrentDate + "', 'MM/dd/yyyy'), ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[16].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[17].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[18].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[19].ToString().Trim(), 50)) + "', ";
                        sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[20].ToString().Trim(), 50)) + "', ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[16]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[17]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[18]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[19]).ToString("0.##") + ", ";
                        sQuery += " " + Convert.ToDouble(arr_fHeaderFees[20]).ToString("0.##") + ")";

                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                    }
                    DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            pSet.Close();

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);
            OnPrintReportWideSummaryOfCollectionMainBusiness(m_sReportName);

            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

        private void OnReportWideSummaryOfCollectionOrgnKind()
        {
            String sQuery, sBnsStatus = ""; //sFeesCode[21],sFeesDesc[21],
            String sDistName = "", sOwnKind = "", sFeesCodeX, sOrNo, sBin, sBinV, sBinX;
            String sCurrentUser;
            int i, ii, iFees_cnt, iRen, iNew, iRet, iCtr1, iStatusCtr = 0;
            double fOrFees_due, fOrFees_surch, fOrFees_pen, fOrFees_amtdue;
            double fRenGr = 0, fRenGross = 0, fRetGr = 0, fRetGross = 0, fNewCap = 0, fNewCapital = 0;
            double fSurchPen, fTotalFees, dStatusCap = 0, dStatusGross = 0; //fHeaderFees[21]

            ArrayList arr_fHeaderFees = new ArrayList();
            ArrayList arr_sFeesCode = new ArrayList();
            ArrayList arr_sFeesDesc = new ArrayList();

            fSurchPen = 0;
            fTotalFees = 0;

            String sFormatDistrict = "";
            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            pSet.Query = string.Format("DELETE FROM REPORT_SUMCOLL WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
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

            for (ii = 0; ii <= 21; ii++)
                arr_fHeaderFees.Add(0);

            iCtr1 = 0;
            i = 0;
            ii = 0;
            arr_sFeesCode.Add("B");
            arr_sFeesDesc.Add("LICENSE TAX");

            sQuery = string.Format("select fees_code,fees_desc from tax_and_fees_table where rev_year = '{0}' order by fees_code", m_sRevYear);
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    iCtr1 += 1;

                    if (iCtr1 <= 19)    // RMC 20150126
                    {
                        arr_sFeesCode.Add(pSet.GetString("fees_code").Trim());
                        arr_sFeesDesc.Add(pSet.GetString("fees_desc").Trim());
                    }
                }
            pSet.Close();

            // RMC 20150126 (S)
            if (iCtr1 > 19)
            {
                arr_sFeesCode.Add("00");
                arr_sFeesDesc.Add("OTHER FEES");
                iCtr1 = 20;
            }// RMC 20150126 (E)

            // RMC 20150505 QA reports (S)
            while (iCtr1 <= 20)
            {
                arr_sFeesCode.Add(" ");
                arr_sFeesDesc.Add(" ");
                iCtr1++;
            }
            // RMC 20150505 QA reports (E)

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);

            if (m_iOptionFormat == 1)
            {
                sQuery = string.Format(@"select count(distinct(pay_hist.or_no)) from pay_hist,businesses
					where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy')
					order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"));
            }
            else
            {
                sQuery = string.Format(@"select count(distinct(pay_hist.or_no)) from pay_hist,businesses
					where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy')
					order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"));
            }

            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            for (int iStat = 1; iStat <= 4; iStat++)
            {
                switch (iStat)
                {
                    case 1:
                        sOwnKind = "SINGLE PROPRIETORSHIP";
                        break;
                    case 2:
                        sOwnKind = "PARTNERSHIP";
                        break;
                    case 3:
                        sOwnKind = "CORPORATION";
                        break;
                    case 4:
                        sOwnKind = "COOPERATIVE";
                        break;
                }

                for (i = 1; i <= 3; i++)
                {
                    iRen = 0;
                    iRet = 0;
                    iNew = 0;
                    fNewCap = 0;
                    fRenGross = 0;
                    fRetGross = 0;
                    fSurchPen = 0;
                    fTotalFees = 0;
                    iStatusCtr = 0;
                    dStatusCap = 0;
                    dStatusGross = 0;

                    arr_fHeaderFees.Clear();
                    for (ii = 0; ii <= iCtr1; ii++)
                        arr_fHeaderFees.Add(0);

                    switch (i)
                    {
                        case 1:
                            sBnsStatus = "NEW";
                            break;
                        case 2:
                            sBnsStatus = "REN";
                            break;
                        case 3:
                            sBnsStatus = "RET";
                            break;
                    }

                    sBinX = "";

                    if (m_iOptionFormat == 1)
                    {
                        sQuery = string.Format(@"select distinct(pay_hist.or_no), pay_hist.bin,businesses.capital,businesses.gr_1 from pay_hist,businesses
					where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy') and
					businesses.bns_stat = '{2}' and bns_dist = '{3}' and orgn_kind = '{4}'
					order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"), sBnsStatus, sFormatDistrict, sOwnKind);
                    }
                    else
                    {
                        sQuery = string.Format(@"select distinct(pay_hist.or_no), pay_hist.bin,businesses.capital,businesses.gr_1 from pay_hist,businesses
					where pay_hist.bin = businesses.bin and pay_hist.or_date >= to_date('{0}','MM/dd/yyyy') and pay_hist.or_date <= to_date('{1}','MM/dd/yyyy') and
					businesses.bns_stat = '{2}' and orgn_kind = '{3}'
					order by pay_hist.bin, pay_hist.or_no", dtpFrom.Value.ToString("MM/dd/yyyy"), dtpTo.Value.ToString("MM/dd/yyyy"), sBnsStatus, sOwnKind);
                    }

                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                    {
                        while (pSet1.Read())
                        {
                            DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                            intCountIncreament += 1;

                            sOrNo = pSet1.GetString("or_no").Trim();
                            sBin = pSet1.GetString("bin").Trim();

                            //(s) RTL 06092006 revise the loop
                            if (sBin != sBinX)
                            {
                                if (sBnsStatus == "NEW")
                                {
                                    iNew = iNew + 1;
                                    fNewCap = pSet1.GetDouble("capital");
                                    fNewCapital = fNewCapital + fNewCap;
                                    iStatusCtr = iStatusCtr + 1;          // Added
                                    dStatusCap = dStatusCap + fNewCap;    // Added
                                }
                                if (sBnsStatus == "REN")
                                {
                                    iRen = iRen + 1;
                                    fRenGr = pSet1.GetDouble("gr_1");
                                    fRenGross = fRenGross + fRenGr;
                                    iStatusCtr = iStatusCtr + 1;         // Added
                                    dStatusGross = dStatusGross + fRenGr;  // Added
                                }
                                if (sBnsStatus == "RET")
                                {
                                    iRet = iRet + 1;
                                    fRetGr = pSet1.GetDouble("gr_1");
                                    fRetGross = fRetGross + fRetGr;
                                    iStatusCtr = iStatusCtr + 1;         // Added
                                    dStatusGross = dStatusGross + fRetGr;  // Added
                                }
                                sBinX = sBin;
                            }

                            sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no = '" + sOrNo + "' and fees_code like 'B%%' having sum(fees_amtdue) is not null");
                            pSet2.Query = sQuery;
                            if (pSet2.Execute())
                            {
                                if (pSet2.Read())
                                {
                                    //if (pSet2.GetDouble("fees_amtdue") != 0)
                                    //{
                                    fOrFees_due = pSet2.GetDouble("fees_due");
                                    fOrFees_surch = pSet2.GetDouble("fees_surch");
                                    fOrFees_pen = pSet2.GetDouble("fees_pen");
                                    fOrFees_amtdue = pSet2.GetDouble("fees_amtdue");

                                    arr_fHeaderFees[0] = Convert.ToDouble(arr_fHeaderFees[0].ToString()) + fOrFees_due;
                                    fSurchPen += (fOrFees_surch + fOrFees_pen);
                                    fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                    //}
                                }
                            }
                            pSet2.Close();

                            for (ii = 1; ii <= iCtr1; ii++)
                            {
                                sFeesCodeX = arr_sFeesCode[ii].ToString();

                                sQuery = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                sQuery += " where or_no = '" + sOrNo + "'";
                                sQuery += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";

                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    if (pSet2.Read())
                                    {
                                        //if (pSet2.GetDouble("fees_amtdue") != 0)
                                        //{
                                        fOrFees_due = pSet2.GetDouble("fees_due");
                                        fOrFees_surch = pSet2.GetDouble("fees_surch");
                                        fOrFees_pen = pSet2.GetDouble("fees_pen");
                                        fOrFees_amtdue = pSet2.GetDouble("fees_amtdue");

                                        arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                        fSurchPen += (fOrFees_surch + fOrFees_pen);
                                        fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                        //  }
                                    }
                                }
                                pSet2.Close();

                                // RMC 20150126 (s)
                                if (ii > 19)
                                {
                                    OracleResultSet pTmp = new OracleResultSet();
                                    fOrFees_due = 0;
                                    fOrFees_surch = 0;
                                    fOrFees_pen = 0;
                                    fOrFees_amtdue = 0;

                                    int iTmpCtr = 0;
                                    pSet2.Query = "select * from tax_and_fees_table where rev_year = '" + m_sRevYear + "' order by fees_code";
                                    if (pSet2.Execute())
                                    {
                                        while (pSet2.Read())
                                        {
                                            iTmpCtr++;
                                            sFeesCodeX = pSet2.GetString("fees_code");

                                            if (iTmpCtr > 19)
                                            {
                                                pTmp.Query = "select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                                pTmp.Query += " where or_no = '" + sOrNo + "'";
                                                pTmp.Query += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                                if (pTmp.Execute())
                                                {
                                                    if (pTmp.Read())
                                                    {
                                                        fOrFees_due += pTmp.GetDouble("fees_due");
                                                        fOrFees_surch += pTmp.GetDouble("fees_surch");
                                                        fOrFees_pen += pTmp.GetDouble("fees_pen");
                                                        fOrFees_amtdue += pTmp.GetDouble("fees_amtdue");
                                                    }
                                                }
                                                pTmp.Close();
                                            }
                                        }

                                        arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                        fSurchPen += (fOrFees_surch + fOrFees_pen);
                                        fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                    }
                                    pSet2.Close();
                                }
                                // RMC 20150126 (e)
                            }

                            sQuery = string.Format("select distinct businesses.bin, businesses.capital,businesses.gr_1, businesses.gr_2 from businesses,pay_hist where pay_hist.or_no = '{0}' and pay_hist.bin = businesses.bin and businesses.bns_stat = '{1}'", sOrNo, sBnsStatus);
                            pSet2.Query = sQuery;
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                {
                                    sBinV = pSet2.GetString("bin").Trim();
                                    if (sBinV != sBinX)
                                    {
                                        if (sBnsStatus == "NEW")
                                        {
                                            iNew = iNew + 1;
                                            fNewCap = pSet2.GetDouble("capital");
                                            fNewCapital = fNewCapital + fNewCap;
                                            iStatusCtr = iStatusCtr + 1;          // Added
                                            dStatusCap = dStatusCap + fNewCap;    // Added
                                        }
                                        if (sBnsStatus == "REN")
                                        {
                                            iRen = iRen + 1;
                                            fRenGr = pSet2.GetDouble("gr_1") + pSet2.GetDouble("gr_2");
                                            fRenGross = fRenGross + fRenGr;
                                            iStatusCtr = iStatusCtr + 1;         // Added
                                            dStatusGross = dStatusGross + fRenGr;  // Added
                                        }
                                        if (sBnsStatus == "RET")
                                        {
                                            iRet = iRet + 1;
                                            fRetGr = pSet2.GetDouble("gr_1") + pSet2.GetDouble("gr_2");
                                            fRetGross = fRetGross + fRetGr;
                                            iStatusCtr = iStatusCtr + 1;         // Added
                                            dStatusGross = dStatusGross + fRetGr;  // Added
                                        }
                                    }
                                }
                            pSet2.Close();

                            sBinX = pSet1.GetString("bin").Trim();
                            DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                            Thread.Sleep(3);
                        }
                    }
                    pSet1.Close();

                    if (sDistName.Trim() == "")
                        sDistName = "NO DISTRICT";

                    if (sOwnKind == "SINGLE PROPRIETORSHIP")
                        sOwnKind = "SINGLE PROP.";

                    sQuery = "insert into report_sumcoll values (";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnKind.Trim(), 100)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsStatus.Trim(), 100)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sFormatDistrict.Trim(), 100)) + "', '',";
                    sQuery += " " + iStatusCtr + ", ";
                    sQuery += " " + dStatusCap.ToString("0.##") + ", 0,";
                    sQuery += " " + dStatusGross.ToString("0.##") + ",'0','0', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[0].ToString().Trim(), 800)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[1].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[2].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[3].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[4].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[5].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[6].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[7].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[8].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[9].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[10].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[11].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[12].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[13].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[14].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[15].ToString().Trim(), 50)) + "', ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[0]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[1]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[2]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[3]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[4]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[5]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[6]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[7]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[8]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[9]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[10]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[11]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[12]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[13]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[14]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[15]).ToString("0.##") + ", ";
                    sQuery += "0, " + fSurchPen + ", ";
                    sQuery += " " + fTotalFees + ", ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(dtpFrom.Value.ToString("MM/dd/yyyy")) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(dtpTo.Value.ToString("MM/dd/yyyy")) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 20)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(m_sRevYear) + "', ";
                    sQuery += "to_date('" + sCurrentDate + "', 'MM/dd/yyyy'), ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[16].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[17].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[18].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[19].ToString().Trim(), 50)) + "', ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[20].ToString().Trim(), 50)) + "', ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[16]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[17]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[18]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[19]).ToString("0.##") + ", ";
                    sQuery += " " + Convert.ToDouble(arr_fHeaderFees[20]).ToString("0.##") + ")";


                    pSet1.Query = sQuery;
                    pSet1.ExecuteNonQuery();

                    if (sOwnKind == "SINGLE PROP.")
                        sOwnKind = "SINGLE PROPRIETORSHIP";
                }
            }

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            OnPrintReportWideSummaryOfCollectionOrgnKind(m_sReportName);

            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

        private void OnPrintReportSummaryOfCollectionBarangay(string p_sReportName)
        {
            frmLiqReports dlg = new frmLiqReports();
            if (m_iOptionFormat == 0)
                dlg.ReportSwitch = "OnReportWideSummaryOfCollectionBarangay";
            else //(m_iOptionFormat == 1)
                dlg.ReportSwitch = "OnReportSimpSummaryOfCollectionBarangay";

            if (chkGC.Checked == true)
                dlg.ShowGrossNCapital = true;
            else
                dlg.ShowGrossNCapital = false;

            dlg.ReportTitle = p_sReportName;
            dlg.DateFrom = dtpFrom.Value;
            dlg.DateTo = dtpTo.Value;
            dlg.ShowDialog();
        }

        private void OnPrintReportWideSummaryOfCollectionDistrict(string p_sReportName)
        {
            frmLiqReports dlg = new frmLiqReports();
            if (m_iOptionFormat == 0)
                dlg.ReportSwitch = "OnReportWideSummaryOfCollectionDistrict";
            else //if (m_iOptionFormat == 1)
                dlg.ReportSwitch = "OnReportSimpSummaryOfCollectionDistrict";

            if (chkGC.Checked == true)
                dlg.ShowGrossNCapital = true;
            else
                dlg.ShowGrossNCapital = false;

            dlg.ReportTitle = p_sReportName;
            dlg.DateFrom = dtpFrom.Value;
            dlg.DateTo = dtpTo.Value;
            dlg.ShowDialog();
        }

        private void OnPrintReportWideSummaryOfCollectionMainBusiness(string p_sReportName)
        {
            //MCR 20190509 (s) Gross income report
            string sValue = "";
            if (chkFilterReport.Checked == true)
            {
                frmFilter frmfilter = new frmFilter();
                frmfilter.HeaderTitle = "Business Type";
                frmfilter.ShowDialog();

                if (frmfilter.isProceed == false)
                {
                    MessageBox.Show("Please select a business to be filtered", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                for (int i = 0; i < frmfilter.arrSelectedValue.Count; i++)
                    sValue += frmfilter.arrSelectedValue[i].ToString();
                sValue = sValue.Remove(sValue.Length - 1, 1);
            }
            //MCR 20190509 (e) Gross income report

            frmLiqReports dlg = new frmLiqReports();
            if (m_iOptionFormat == 0)
                dlg.ReportSwitch = "OnReportWideSummaryOfCollectionMainBusiness";
            else if (m_iOptionFormat == 1)
                dlg.ReportSwitch = "OnReportSimpSummaryOfCollectionMainBusiness";
            else if (m_iOptionFormat == 2)
                dlg.ReportSwitch = "OnReportStanWideSummaryOfCollectionMainBusiness";
            else //if (m_iOptionFormat == 3)
                dlg.ReportSwitch = "OnReportStanSimpSummaryOfCollectionMainBusiness";

            if (chkGC.Checked == true)
                dlg.ShowGrossNCapital = true;
            else
                dlg.ShowGrossNCapital = false;

            dlg.ReportTitle = p_sReportName;
            dlg.DateFrom = dtpFrom.Value;
            dlg.DateTo = dtpTo.Value;
            dlg.sFilterValue = sValue;
            dlg.ShowDialog();
        }

        private void OnPrintReportWideSummaryOfCollectionOrgnKind(string p_sReportName)
        {
            frmLiqReports dlg = new frmLiqReports();
            if (m_iOptionFormat == 0)
                dlg.ReportSwitch = "OnReportWideSummaryOfCollectionOrgnKind";
            else //if (m_iOptionFormat == 2)
                dlg.ReportSwitch = "OnReportStanWideSummaryOfCollectionOrgnKind";

            if (chkGC.Checked == true)
                dlg.ShowGrossNCapital = true;
            else
                dlg.ShowGrossNCapital = false;

            dlg.ReportTitle = p_sReportName;
            dlg.DateFrom = dtpFrom.Value;
            dlg.DateTo = dtpTo.Value;
            dlg.ShowDialog();
        }

        private void OnPrintReportWideSummaryOfCollectionBusinessStatus(string p_sReportName)
        {
            frmLiqReports dlg = new frmLiqReports();
            if (m_iOptionFormat == 0)
                dlg.ReportSwitch = "OnReportWideSummaryOfCollectionBusinessStatus";
            else //if (m_iOptionFormat == 2)
                dlg.ReportSwitch = "OnReportStanWideSummaryOfCollectionBusinessStatus";

            if (chkGC.Checked == true)
                dlg.ShowGrossNCapital = true;
            else
                dlg.ShowGrossNCapital = false;

            dlg.ReportTitle = p_sReportName;
            dlg.DateFrom = dtpFrom.Value;
            dlg.DateTo = dtpTo.Value;
            dlg.ShowDialog();
        }

        private void OnPrintReportWideSummaryOfCollectionLineOfBusiness(string p_sReportName)
        {
            //MCR 20190509 (s) Gross income report
            string sValue = "";
            frmFilter frmfilter = new frmFilter();
            if (chkFilterReport.Checked == true)
            {
                frmfilter.HeaderTitle = "Line of Business";
                frmfilter.BnsCode = m_sBnsCode;
                frmfilter.ShowDialog();

                if (frmfilter.isProceed == false)
                {
                    MessageBox.Show("Please select a business to be filtered", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                for (int i = 0; i < frmfilter.arrSelectedValue.Count; i++)
                    sValue += frmfilter.arrSelectedValue[i].ToString();
                sValue = sValue.Remove(sValue.Length - 1, 1);
            }
            //MCR 20190509 (e) Gross income report

            frmLiqReports dlg = new frmLiqReports();
            if (chkGC.Checked == true)
                dlg.ShowGrossNCapital = true;
            else
                dlg.ShowGrossNCapital = false;
            dlg.ReportSwitch = "OnReportWideSummaryOfCollectionLineOfBusiness";
            dlg.ReportTitle = p_sReportName;
            dlg.DateFrom = dtpFrom.Value;
            dlg.DateTo = dtpTo.Value;
            dlg.sFilterValue = sValue;
            dlg.ShowDialog();
        }

        private void chkStandardFormat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStandardFormat.Checked == true)
            {
                m_iOptionFormat = 0;
                chkSimplifiedFormat.Checked = false;
            }
        }

        private void chkSimplifiedFormat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSimplifiedFormat.Checked == true)
            {
                m_iOptionFormat = 1;
                chkStandardFormat.Checked = false;
            }
        }

        private void rdoBrgy_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdoTotalCol_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void rdoLineBus_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LoadTaxYear()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select distinct(tax_year) from rep_list_coll order by tax_year desc";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbYear.Items.Add(pSet.GetString("tax_year").Trim());
                }
            }
            pSet.Close();
        }

        private void rdoTotalCol_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        /*private bool ValidateDupBin(string sBin, string sUserCode)
        {
           // RMC 20170228 correction in generation of Summary of collection
          OracleResultSet pSet = new OracleResultSet();
           OracleResultSet pCmd = new OracleResultSet();
            
           pSet.Query = "select * from report_tmp_tbl where bin = '" + sBin + "' and rep_nm = '" + m_sReportName + "' ";
           pSet.Query += "and usr_code = '" + sUserCode + "'";
           if(pSet.Execute())
           {
               if(!pSet.Read())
               {
                   pCmd.Query = "insert into report_tmp_tbl values (";
                   pCmd.Query += "'" + sBin + "', ";
                   pCmd.Query += "'" + m_sReportName + "', ";
                   pCmd.Query += "'" + sUserCode + "')";
                   if (pCmd.ExecuteNonQuery() == 0)
                   { }

                   return false;
               }
           }
           pSet.Close();

           return true;

       }*/


    }
}