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
    public partial class frmUnrenewedBusiness : Form
    {
        public frmUnrenewedBusiness()
        {
            InitializeComponent();
        }
        int m_iOptionFormat = 0;
        string m_sReportName = String.Empty;
        ArrayList arr_BnsCode = new ArrayList();
        string m_sBnsCode = String.Empty;

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
        //MCR 20140714 (e)

        private void frmUnrenewedBusiness_Load(object sender, EventArgs e)
        {
            LoadBrgy();
            LoadBusType();
            LoadDist();
            rdoMainBus.PerformClick();
            rdoBnsName.PerformClick();
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

        private void BtnControl(bool blnEnable)
        {
            cmbBrgy.Enabled = blnEnable;
            cmbBusType.Enabled = blnEnable;
            cmbDist.Enabled = blnEnable;
        }

        private void rdoMainBus_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 0;
            BtnControl(false);
            cmbBusType.Enabled = true;
            cmbBusType.SelectedIndex = 0;
        }

        private void rdoBrgy_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 1;
            BtnControl(false);
            cmbBrgy.Enabled = true;
            cmbBrgy.SelectedIndex = 0;
        }

        private void rdoDist_Click(object sender, EventArgs e)
        {
            m_iOptionFormat = 2;
            BtnControl(false);
            cmbDist.Enabled = true;
            cmbDist.SelectedIndex = 0;
        }

        private void rdoBnsName_Click(object sender, EventArgs e)
        {

        }

        private void rdoOwnName_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string m_sQuery = String.Empty;

            if (m_iOptionFormat == 0)
            {
                m_sReportName = "LIST OF BUSINESSES WITH UNRENEWED PERMITS BY MAIN BUSINESS";
            }
            else if (m_iOptionFormat == 1)
            {
                m_sReportName = "LIST OF BUSINESSES WITH UNRENEWED PERMITS BY BARANGAY";
            }
            else if (m_iOptionFormat == 2)
            {
                m_sReportName = "LIST OF BUSINESSES WITH UNRENEWED PERMITS BY DISTRICT";
            }

            if (AppSettingsManager.GenerateInfo(m_sReportName))
            {
                OnReportListOfUnrenewed();

                m_sQuery = @"SELECT rep_unren_bns.heading, rep_unren_bns.data_group,(Select distinct count(data_group) from rep_unren_bns RB where REP_UNREN_BNS.DATA_GROUP = RB.DATA_GROUP and RB.report_name = '" + m_sReportName + "' AND RB.user_code = '" + AppSettingsManager.SystemUser.UserCode + @"') as Counts, rep_unren_bns.bin, rep_unren_bns.bns_nm, 
	rep_unren_bns.bns_address, rep_unren_bns.own_nm, rep_unren_bns.own_address, rep_unren_bns.bns_stat,
	rep_unren_bns.last_tyear, rep_unren_bns.last_date, rep_unren_bns.last_or, rep_unren_bns.pay_type, rep_unren_bns.qtr_paid, rep_unren_bns.current_user, rep_unren_bns.report_name 
	FROM rep_unren_bns WHERE rep_unren_bns.report_name = '" + m_sReportName + "' AND rep_unren_bns.user_code = '" + AppSettingsManager.SystemUser.UserCode + "' ORDER BY rep_unren_bns.data_group ASC,";

                if (rdoBnsName.Checked)
                    m_sQuery += @" rep_unren_bns.bns_nm ASC";
                else
                    m_sQuery += @" rep_unren_bns.own_nm ASC";

                frmDelinquencyReport frmDelinquencyReport = new frmDelinquencyReport();
                frmDelinquencyReport.ReportName = m_sReportName;
                frmDelinquencyReport.Query = m_sQuery;
                frmDelinquencyReport.ShowDialog();
            }
        }

        private void cmbBusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_sBnsCode = arr_BnsCode[cmbBusType.SelectedIndex].ToString();
            }
            catch
            {
                cmbBusType.SelectedIndex = 0;
                return;
            }
        }

        private void OnReportListOfUnrenewed()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            String sQuery = "", sCurrentUser, sBnsCode, sBarangay = "", unrenBnsBrgy = "", sDistrict = "", unrenBnsDist = "";
            String unrenBin, unrenBnsDesc = "", unrenBnsNm = "", unrenBnsStat = "", unrenTaxYear, unrenBnsAdd = "", unrenOwnNm = "", unrenOwnAdd = "", unrenOrNo = "", unrenOrDate = "", unrenOrQtr = "", unrenOrTY = "", unrenOrTerm = "", sTmpCode;

            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            if (cmbBrgy.Text == "ALL")
                sBarangay = "%%";
            else
                sBarangay = cmbBrgy.Text + "%";

            if (m_sBnsCode == "")
                sBnsCode = "%%";
            else
                sBnsCode = m_sBnsCode + "%";

            if (cmbDist.Text == "ALL")
                sDistrict = "%%";
            else
                sDistrict = cmbDist.Text + "%";

            pSet.Query = string.Format("DELETE FROM REP_UNREN_BNS WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
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

            //BODY

            String sTmpTaxYear = m_dtSaveTime.Year.ToString();

            #region Progress
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);

            if (m_iOptionFormat == 0)
                sQuery = string.Format(@"select count(distinct bin) as iFees from businesses, bns_table
				where tax_year < '{0}' and businesses.bns_stat <> 'RET' and businesses.bns_code like '{1}'
				and businesses.bns_code = bns_table.bns_code
				and fees_code = 'B' ", sTmpTaxYear, sBnsCode);
            else if (m_iOptionFormat == 1)
                sQuery = string.Format(@"select count(distinct bin) as iFees from businesses, own_names
		    where tax_year < '{0}' and businesses.bns_stat <> 'RET' and businesses.bns_brgy like '{1}'
			and businesses.own_code = own_names.own_code", sTmpTaxYear, sBarangay);
            else if (m_iOptionFormat == 2)
                sQuery = string.Format(@"select count(distinct bin) as iFees from businesses, own_names
				where tax_year < '{0}' and businesses.bns_dist like '{1}'
				and businesses.own_code = own_names.own_code", sTmpTaxYear, sDistrict);

            pSet.Query = sQuery;
            intCountIncreament = 0;
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            intCount += intCount;

            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            if (m_iOptionFormat == 0)
                sQuery = string.Format(@"select distinct bin, bns_desc, bns_nm from businesses, bns_table
				where tax_year < '{0}' and businesses.bns_stat <> 'RET' and businesses.bns_code like '{1}'
				and businesses.bns_code = bns_table.bns_code
				and fees_code = 'B' order by bns_desc, bns_nm", sTmpTaxYear, sBnsCode);
            else if (m_iOptionFormat == 1)
                sQuery = string.Format(@"select distinct bin, bns_nm, bns_brgy from businesses, own_names
				where tax_year < '{0}' and businesses.bns_stat <> 'RET' and businesses.bns_brgy like '{1}'
				and businesses.own_code = own_names.own_code order by bns_brgy", sTmpTaxYear, sBarangay);
            else if (m_iOptionFormat == 2)
                sQuery = string.Format(@"select distinct bin, bns_nm, bns_dist from businesses, own_names
				where tax_year < '{0}' and businesses.bns_dist like '{1}'
				and businesses.own_code = own_names.own_code order by bns_dist", sTmpTaxYear, sDistrict);

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    unrenBin = pSet.GetString("bin").Trim();
                    unrenBnsNm = StringUtilities.RemoveApostrophe(pSet.GetString("bns_nm").Trim());
                    if (m_iOptionFormat == 0)
                        unrenBnsDesc = StringUtilities.RemoveApostrophe(pSet.GetString("bns_desc").Trim());
                    else if (m_iOptionFormat == 1)
                        unrenBnsBrgy = StringUtilities.RemoveApostrophe(pSet.GetString("bns_brgy").Trim());
                    else if (m_iOptionFormat == 2)
                        unrenBnsDist = StringUtilities.RemoveApostrophe(pSet.GetString("bns_dist").Trim());

                    sQuery = string.Format("select bns_stat, tax_year, own_code from businesses where bin = '{0}' and bns_stat <> 'RET'", unrenBin);
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        if (pSet1.Read())
                        {
                            unrenBnsStat = pSet1.GetString("bns_stat").Trim();
                            unrenTaxYear = pSet1.GetString("tax_year").Trim();
                            sTmpCode = pSet1.GetString("own_code").Trim();

                            unrenBnsAdd = AppSettingsManager.GetBnsAddress(unrenBin);
                            unrenOwnNm = AppSettingsManager.GetBnsOwner(sTmpCode);
                            unrenOwnAdd = AppSettingsManager.GetBnsOwnAdd(sTmpCode);

                            sQuery = string.Format(@"select or_no, or_date, qtr_paid, payment_term,tax_year from pay_hist 
                                        where pay_hist.bin = '{0}' and bns_stat <> 'RET' order by tax_year desc,qtr_paid desc,or_date desc", unrenBin);
                            pSet2.Query = sQuery;
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                {
                                    unrenOrNo = pSet2.GetString("or_no").Trim();
                                    unrenOrDate = pSet2.GetDateTime("or_date").ToShortDateString().Trim();
                                    unrenOrQtr = pSet2.GetString("qtr_paid").Trim();
                                    unrenOrTerm = pSet2.GetString("payment_term").Trim();
                                    unrenOrTY = pSet2.GetString("tax_year").Trim();
                                }
                            pSet2.Close();
                        }
                    pSet1.Close();
                    if (m_iOptionFormat == 0)
                    {
                        sQuery = string.Format(@"insert into rep_unren_bns values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')",
                                         StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                         StringUtilities.HandleApostrophe(StringUtilities.Left(unrenBnsDesc.Trim(), 30)),
                                         StringUtilities.HandleApostrophe(unrenBin.Trim()),
                                         StringUtilities.HandleApostrophe(StringUtilities.Left(unrenBnsNm.Trim(), 50)),
                                         StringUtilities.HandleApostrophe(StringUtilities.Left(unrenBnsAdd.Trim(), 100)),
                                         StringUtilities.HandleApostrophe(StringUtilities.Left(unrenOwnNm.Trim(), 50)),
                                         StringUtilities.HandleApostrophe(StringUtilities.Left(unrenOwnAdd.Trim(), 100)),
                                         StringUtilities.HandleApostrophe(unrenBnsStat.Trim()),
                                         StringUtilities.HandleApostrophe(unrenOrTY.Trim()),
                                         StringUtilities.HandleApostrophe(unrenOrDate.Trim()),
                                         StringUtilities.HandleApostrophe(unrenOrNo.Trim()),
                                         StringUtilities.HandleApostrophe(unrenOrTerm.Trim()),
                                         StringUtilities.HandleApostrophe(unrenOrQtr.Trim()),
                                         StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                         StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                         StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode.Trim()));
                    }
                    else if (m_iOptionFormat == 1)
                    {
                        sQuery = string.Format(@"insert into rep_unren_bns values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')",
                                       StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                       StringUtilities.HandleApostrophe(unrenBnsBrgy.Trim()),
                                       StringUtilities.HandleApostrophe(unrenBin.Trim()),
                                       StringUtilities.HandleApostrophe(StringUtilities.Left(unrenBnsNm.Trim(), 50)),
                                       StringUtilities.HandleApostrophe(StringUtilities.Left(unrenBnsAdd.Trim(), 100)),
                                       StringUtilities.HandleApostrophe(StringUtilities.Left(unrenOwnNm.Trim(), 50)),
                                       StringUtilities.HandleApostrophe(StringUtilities.Left(unrenOwnAdd.Trim(), 100)),
                                       StringUtilities.HandleApostrophe(unrenBnsStat.Trim()),
                                       StringUtilities.HandleApostrophe(unrenOrTY.Trim()),
                                       StringUtilities.HandleApostrophe(unrenOrDate.Trim()),
                                       StringUtilities.HandleApostrophe(unrenOrNo.Trim()),
                                       StringUtilities.HandleApostrophe(unrenOrTerm.Trim()),
                                       StringUtilities.HandleApostrophe(unrenOrQtr.Trim()),
                                       StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                       StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                       StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode.Trim()));
                    }
                    else if (m_iOptionFormat == 2)
                    {
                        sQuery = string.Format(@"insert into rep_unren_bns values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')",
                                       StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("09").Trim()),
                                       StringUtilities.HandleApostrophe(StringUtilities.Left(unrenBnsDist.Trim(), 30)),
                                       StringUtilities.HandleApostrophe(unrenBin.Trim()),
                                       StringUtilities.HandleApostrophe(StringUtilities.Left(unrenBnsNm.Trim(), 50)),
                                       StringUtilities.HandleApostrophe(StringUtilities.Left(unrenBnsAdd.Trim(), 100)),
                                       StringUtilities.HandleApostrophe(StringUtilities.Left(unrenOwnNm.Trim(), 50)),
                                       StringUtilities.HandleApostrophe(StringUtilities.Left(unrenOwnAdd.Trim(), 100)),
                                       StringUtilities.HandleApostrophe(unrenBnsStat.Trim()),
                                       StringUtilities.HandleApostrophe(unrenOrTY.Trim()),
                                       StringUtilities.HandleApostrophe(unrenOrDate.Trim()),
                                       StringUtilities.HandleApostrophe(unrenOrNo.Trim()),
                                       StringUtilities.HandleApostrophe(unrenOrTerm.Trim()),
                                       StringUtilities.HandleApostrophe(unrenOrQtr.Trim()),
                                       StringUtilities.HandleApostrophe(sCurrentUser.Trim()),
                                       StringUtilities.HandleApostrophe(m_sReportName.Trim()),
                                       StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode.Trim()));
                    }
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            else
            {
                MessageBox.Show("No Record(s) Found!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pSet.Close();

            sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

    }
}