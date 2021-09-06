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
using Amellar.Common.AuditTrail;
using Amellar.Common.DynamicProgressBar;
using System.Threading;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmReportListBnsQtrPaid : Form
    {
        public frmReportListBnsQtrPaid()
        {
            InitializeComponent();
        }

        int m_iQtr;
        string m_sQtr = String.Empty;
        string m_sReportName = String.Empty;

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

        private void txtTaxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void frmReportListBnsQtrPaid_Load(object sender, EventArgs e)
        {
            rdo1stqtr.PerformClick();
        }

        private void EnableControls(bool blnEnable)
        {
            txtTaxYear.Enabled = blnEnable;
            dtpFrom.Enabled = blnEnable;
            dtpTo.Enabled = blnEnable;
        }

        private void ListBnsQtrPaid()
        {
            DateTime m_dtSaveTime;
            String sQuery, sCurrentUser, sCurrentDate;
            String sBIN, sBnsNm = String.Empty, sBnsAdd = String.Empty, sOwnCode, sOwnNm = String.Empty, sORNo, sORDate, sBnsStat = String.Empty, sBnsCode = String.Empty, sBnsDesc = String.Empty;
            String sGrCap, sBTax, sFees, sPenalty, sTotal;

            try
            {
                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

                OracleResultSet pSet = new OracleResultSet();
                OracleResultSet pSet1 = new OracleResultSet();
                OracleResultSet pSet2 = new OracleResultSet();

                pSet.Query = string.Format("DELETE FROM REP_LIST_BNS WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.ExecuteNonQuery();
                pSet.Close();

                pSet.Query = string.Format("DELETE FROM GEN_INFO WHERE REPORT_NAME = '{0}'", m_sReportName);
                pSet.ExecuteNonQuery();
                pSet.Close();

                m_dtSaveTime = AppSettingsManager.GetSystemDate();
                sCurrentDate = string.Format("{1}/{2}/{0}",
                        m_dtSaveTime.Year,
                        m_dtSaveTime.Month,
                        m_dtSaveTime.Day); // {3}:{4}:{5} m_dtSaveTime.Hour, m_dtSaveTime.Minute, m_dtSaveTime.Second);

                pSet.Query = string.Format("INSERT INTO GEN_INFO VALUES('{0}', TO_DATE('" + sCurrentDate + "','MM/dd/yyyy'),'{1}',{2},'ASS')", m_sReportName, AppSettingsManager.SystemUser.UserCode, 1);
                pSet.ExecuteNonQuery();
                pSet.Close();

                #region Progress

                int intCount = 0;
                int intCountIncreament = 0;
                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();
                Thread.Sleep(500);

                sQuery = string.Format("select distinct count (*) from pay_hist where tax_year = '{0}' and qtr_paid = '{1}'", txtTaxYear.Text, m_sQtr);
                pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);

                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

                #endregion

                sQuery = string.Format("select distinct * from pay_hist where tax_year = '{0}' and qtr_paid = '{1}'", txtTaxYear.Text, m_sQtr);
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        sBIN = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                        sORNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                        sORDate = pSet.GetDateTime("or_date").ToShortDateString();

                        sQuery = string.Format("select * from businesses where bin = '{0}'", sBIN);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            if (pSet1.Read())
                            {
                                sBnsNm = StringUtilities.HandleApostrophe(pSet1.GetString("bns_nm"));
                                sBnsAdd = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBIN));
                                sOwnCode = StringUtilities.HandleApostrophe(pSet1.GetString("own_code"));
                                sOwnNm = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(sOwnCode)); // OwnersName(sOwnCode);
                                sBnsCode = StringUtilities.HandleApostrophe(pSet1.GetString("bns_code"));
                                sBnsDesc = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsDesc(sBnsCode));
                                sBnsStat = StringUtilities.HandleApostrophe(pSet1.GetString("bns_stat"));
                            }
                        }
                        pSet1.Close();

                        sBTax = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "B", "D");
                        sFees = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "TFD");
                        sPenalty = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "P");
                        sTotal = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "AD");

                        sGrCap = AppSettingsManager.GetCapitalGross(sBIN, sBnsCode, txtTaxYear.Text, "PRE");
                        if (sGrCap.Trim() == String.Empty)
                        {
                            sGrCap = AppSettingsManager.GetCapitalGross(sBIN, sBnsCode, txtTaxYear.Text, sBnsStat);
                        }
                        sQuery = string.Format(@"insert into rep_list_bns values('{0}','','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','','{9}','{10}','{11}','{12}','{13}','','{14}','{15}','{16}','{17}','{18}','','')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                            //StringUtilities.HandleApostrophe(sBrgyName),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 30)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsDesc, 30)),
                                               StringUtilities.HandleApostrophe(sBIN),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsNm, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAdd, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               sGrCap,
                                               sTotal,
                            //StringUtilities.HandleApostrophe(sBnsPermit),
                                               sBTax,
                                               sFees,
                                               sPenalty,
                                               StringUtilities.HandleApostrophe(sORNo),
                                               StringUtilities.HandleApostrophe(sORDate),
                            //StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(txtTaxYear.Text),
                                               StringUtilities.HandleApostrophe(m_sQtr),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)));
                        pSet1.Query = sQuery;
                        pSet1.ExecuteNonQuery();
                        //(e)-----------> Saving to Report Table

                        // FOR ADDITIONAL BUSINESSES (s){
                        sQuery = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}'", sBIN, txtTaxYear.Text);
                        pSet2.Query = sQuery;
                        if (pSet2.Execute())
                        {
                            while (pSet2.Read())
                            {
                                sBnsCode = StringUtilities.HandleApostrophe(pSet2.GetString("bns_code_main"));
                                sBnsDesc = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsDesc(sBnsCode));
                                sBnsStat = StringUtilities.HandleApostrophe(pSet2.GetString("bns_stat"));

                                sBTax = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "B", "D");
                                sFees = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "TFD");
                                sPenalty = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "P");
                                sTotal = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "AD");

                                sGrCap = AppSettingsManager.GetCapitalGross(sBIN, sBnsCode, txtTaxYear.Text, "PRE");
                                if (sGrCap.Trim() == String.Empty)
                                {
                                    sGrCap = AppSettingsManager.GetCapitalGross(sBIN, sBnsCode, txtTaxYear.Text, sBnsStat);
                                }
                                // INSERT INTO THE TABLE (s){
                                sQuery = string.Format("insert into rep_list_bns values ('{0}','','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','','{9}','{10}','{11}','{12}','{13}','','{14}','{15}','{16}','{17}','{18}','','')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                    //StringUtilities.HandleApostrophe(sBrgyName),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 30)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsDesc, 30)),
                                               StringUtilities.HandleApostrophe(sBIN),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsNm, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAdd, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               sGrCap,
                                               sTotal,
                                    //StringUtilities.HandleApostrophe(sBnsPermit),
                                               sBTax,
                                               sFees,
                                               sPenalty,
                                               StringUtilities.HandleApostrophe(sORNo),
                                               StringUtilities.HandleApostrophe(sORDate),
                                    //StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(txtTaxYear.Text),
                                               StringUtilities.HandleApostrophe(m_sQtr),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)));
                                pSet2.Query = sQuery;
                                pSet2.ExecuteNonQuery();
                            }
                        }
                        pSet2.Close();
                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                    }

                    DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                    Thread.Sleep(10);

                    frmBussReport dlg = new frmBussReport();
                    dlg.ReportSwitch = "ListBnsQtrDatePaid";//21;
                    dlg.ReportName = m_sReportName;
                    dlg.ShowDialog();
                }
                pSet.Close();
                //pApp->JobStatus(pApp->iProgressTotal+1);

                //(s)----------> Delete Data from Temp Report Table
                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
                //(e)----------> Delete Data from Temp Report Table
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ListBnsDateRange()
        {
            DateTime m_dtSaveTime;
            String sQuery, sCurrentUser, sCurrentDate;
            String sBIN, sBnsNm = String.Empty, sBnsAdd = String.Empty, sOwnCode, sOwnNm = String.Empty, sORNo = String.Empty, sORDate, sBnsStat = String.Empty, sBnsCode = String.Empty, sTaxYear = String.Empty, sQtr, sFrom, sTo, sBnsDesc = String.Empty;
            String sGrCap, sBTax, sFees, sPenalty, sTotal;

            try
            {
                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

                OracleResultSet pSet = new OracleResultSet();
                OracleResultSet pSet1 = new OracleResultSet();

                pSet.Query = string.Format("DELETE FROM REP_LIST_BNS WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.ExecuteNonQuery();
                pSet.Close();

                pSet.Query = string.Format("DELETE FROM GEN_INFO WHERE REPORT_NAME = '{0}'", m_sReportName);
                pSet.ExecuteNonQuery();
                pSet.Close();

                m_dtSaveTime = AppSettingsManager.GetSystemDate();
                sCurrentDate = string.Format("{1}/{2}/{0}",
                        m_dtSaveTime.Year,
                        m_dtSaveTime.Month,
                        m_dtSaveTime.Day); // {3}:{4}:{5} m_dtSaveTime.Hour, m_dtSaveTime.Minute, m_dtSaveTime.Second);

                pSet.Query = string.Format("INSERT INTO GEN_INFO VALUES('{0}',TO_DATE('" + sCurrentDate + "','MM/dd/yyyy'),'{1}',{2},'ASS')", m_sReportName, AppSettingsManager.SystemUser.UserCode, 1);
                pSet.ExecuteNonQuery();
                pSet.Close();

                sFrom = string.Format("{0}/{1}/{2}", dtpFrom.Value.Month, dtpFrom.Value.Day, dtpFrom.Value.Year);
                sTo = string.Format("{0}/{1}/{2}", dtpTo.Value.Month, dtpTo.Value.Day, dtpTo.Value.Year);

                #region Progress

                int intCount = 0;
                int intCountIncreament = 0;
                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();
                Thread.Sleep(500);
                sQuery = string.Format("select distinct count(*) from pay_hist where or_date >= to_date('{0}','MM/DD/YYYY') and or_date <= to_date('{1}','MM/DD/YYYY')", sFrom, sTo);
                pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);

                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

                #endregion

                sQuery = string.Format("select distinct bin,or_date from pay_hist where or_date >= to_date('{0}','MM/DD/YYYY') and or_date <= to_date('{1}','MM/DD/YYYY')", sFrom, sTo);
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        sBIN = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                        sORDate = pSet.GetDateTime("or_date").ToShortDateString();

                        sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and or_date = to_date('{1}','MM/DD/YYYY') order by tax_year desc", sBIN, sORDate);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            if (pSet1.Read())
                            {
                                sORNo = StringUtilities.HandleApostrophe(pSet1.GetString("or_no"));
                                sQtr = StringUtilities.HandleApostrophe(pSet1.GetString("qtr_paid"));
                            }
                        }
                        pSet1.Close();

                        pSet1 = new OracleResultSet();
                        sQuery = string.Format("select * from businesses where bin = '{0}'", sBIN);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            if (pSet1.Read())
                            {
                                sBnsNm = StringUtilities.HandleApostrophe(pSet1.GetString("bns_nm"));
                                sBnsAdd = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBIN));
                                sOwnCode = StringUtilities.HandleApostrophe(pSet1.GetString("own_code"));
                                sOwnNm = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(sOwnCode)); // OwnersName(sOwnCode);
                                sBnsCode = StringUtilities.HandleApostrophe(pSet1.GetString("bns_code"));
                                sBnsDesc = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsDesc(sBnsCode));
                                sBnsStat = StringUtilities.HandleApostrophe(pSet1.GetString("bns_stat"));
                                sTaxYear = StringUtilities.HandleApostrophe(pSet1.GetString("tax_year")); //CTS 01242003 adding tax year for filtering
                            }
                        }
                        pSet1.Close();

                        sBTax = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "B", "D");
                        sFees = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "TFD");
                        sPenalty = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "P");
                        sTotal = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "AD");

                        sGrCap = AppSettingsManager.GetCapitalGross(sBIN, sBnsCode, txtTaxYear.Text, "PRE");
                        if (sGrCap.Trim() == String.Empty || sGrCap.Trim() == "0.00")
                            sGrCap = AppSettingsManager.GetCapitalGross(sBIN, sBnsCode, txtTaxYear.Text, sBnsStat);
                        sQuery = string.Format(@"insert into rep_list_bns values('{0}','','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','','{9}','{10}','{11}','{12}','{13}','','{14}','{15}','{16}','{17}','{18}','','')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                            //StringUtilities.HandleApostrophe(sBrgyName),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 30)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsDesc, 30)),
                                               StringUtilities.HandleApostrophe(sBIN),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsNm, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAdd, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               sGrCap,
                                               sTotal,
                            //StringUtilities.HandleApostrophe(sBnsPermit),
                                               sBTax,
                                               sFees,
                                               sPenalty,
                                               StringUtilities.HandleApostrophe(sORNo),
                                               StringUtilities.HandleApostrophe(sORDate),
                            //StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(txtTaxYear.Text),
                                               StringUtilities.HandleApostrophe(m_sQtr),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)));
                        pSet1.Query = sQuery;
                        pSet1.ExecuteNonQuery();
                        //(e)-----------> Saving to Report Table

                        // FOR ADDITIONAL BUSINESSES (s){
                        sQuery = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}'", sBIN, sTaxYear);
                        if (pSet1.Execute())
                        {
                            while (pSet1.Read())
                            {
                                sBnsCode = StringUtilities.HandleApostrophe(pSet1.GetString("bns_code_main"));
                                sBnsDesc = StringUtilities.HandleApostrophe(pSet1.GetString(sBnsCode));
                                sBnsStat = StringUtilities.HandleApostrophe(pSet1.GetString("bns_stat"));

                                sBTax = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "B", "D");
                                sFees = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "TFD");
                                sPenalty = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "P");
                                sTotal = AppSettingsManager.GetOrAmount(sORNo, sBnsCode, "", "AD");

                                sGrCap = AppSettingsManager.GetCapitalGross(sBIN, sBnsCode, txtTaxYear.Text, "PRE");
                                if (sGrCap.Trim() == String.Empty || sGrCap.Trim() == "0.00")
                                    sGrCap = AppSettingsManager.GetCapitalGross(sBIN, sBnsCode, txtTaxYear.Text, sBnsStat);
                                // INSERT INTO THE TABLE (s){
                                sQuery = string.Format(@"insert into rep_list_bns values('{0}','','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','','{9}','{10}','{11}','{12}','{13}','','{14}','{15}','{16}','{17}','{18}','','')",
                                                      StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                    //StringUtilities.HandleApostrophe(sBrgyName),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 30)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsDesc, 30)),
                                               StringUtilities.HandleApostrophe(sBIN),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsNm, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAdd, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               sGrCap,
                                               sTotal,
                                    //StringUtilities.HandleApostrophe(sBnsPermit),
                                               sBTax,
                                               sFees,
                                               sPenalty,
                                               StringUtilities.HandleApostrophe(sORNo),
                                               StringUtilities.HandleApostrophe(sORDate),
                                    //StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(txtTaxYear.Text),
                                               StringUtilities.HandleApostrophe(m_sQtr),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)));
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();
                            }
                        }
                        pSet1.Close();
                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                    }

                    DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                    Thread.Sleep(10);

                    frmBussReport dlg = new frmBussReport();
                    dlg.ReportSwitch = "ListBnsQtrDatePaid";//21;
                    dlg.ReportName = m_sReportName;
                    dlg.ShowDialog();
                }
                pSet.Close();
                //pApp->JobStatus(pApp->iProgressTotal+1);

                //(s)----------> Delete Data from Temp Report Table
                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
                //(e)----------> Delete Data from Temp Report Table
            }
            catch (Exception a)
            {
                MessageBox.Show(a.ToString());
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (m_iQtr == 5)
                ListBnsDateRange();
            else
            {
                if (txtTaxYear.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Tax Year is Required", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ListBnsQtrPaid();
            }
        }

        private void rdo1stqtr_Click(object sender, EventArgs e)
        {
            m_iQtr = 0;
            EnableControls(false);
            txtTaxYear.Enabled = true;
            m_sReportName = "LIST OF BUSINESSES WITH FIRST QTR PAYMENT";
            m_sQtr = "1";
            txtTaxYear.Focus();
        }

        private void rdo2ndqtr_Click(object sender, EventArgs e)
        {
            m_iQtr = 1;
            EnableControls(false);
            txtTaxYear.Enabled = true;
            m_sReportName = "LIST OF BUSINESSES WITH SECOND QTR PAYMENT";
            m_sQtr = "2";
            txtTaxYear.Focus();
        }

        private void rdo3rdqtr_Click(object sender, EventArgs e)
        {
            m_iQtr = 2;
            EnableControls(false);
            txtTaxYear.Enabled = true;
            m_sReportName = "LIST OF BUSINESSES WITH THIRD QTR PAYMENT";
            m_sQtr = "3";
            txtTaxYear.Focus();
        }

        private void rdo4thqtr_Click(object sender, EventArgs e)
        {
            m_iQtr = 3;
            EnableControls(false);
            txtTaxYear.Enabled = true;
            m_sReportName = "LIST OF BUSINESSES WITH FOURTH QTR PAYMENT";
            m_sQtr = "4";
            txtTaxYear.Focus();
        }

        private void rdofyear_Click(object sender, EventArgs e)
        {
            m_iQtr = 4;
            EnableControls(false);
            txtTaxYear.Enabled = true;
            m_sReportName = "LIST OF BUSINESSES WITH FULL PAYMENT";
            m_sQtr = "F";
            txtTaxYear.Focus();
        }

        private void rdodtrange_Click(object sender, EventArgs e)
        {
            m_iQtr = 5;
            EnableControls(true);
            txtTaxYear.Enabled = false;
            m_sReportName = "LIST OF BUSINESSES WITH PAYMENT DATE";
            m_sQtr = String.Empty;
            dtpFrom.Focus();
        }
    }
}