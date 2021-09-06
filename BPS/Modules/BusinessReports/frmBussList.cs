using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Common.Message_Box;
using System.Threading;
using Amellar.Common.DynamicProgressBar;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmBussList : Form
    {
        public frmBussList()
        {
            InitializeComponent();
        }

        private string m_sORRangeFrom = String.Empty;
        private string m_sORRangeTo = String.Empty;

        private string m_strRevYear = string.Empty;
        ArrayList arr_BnsCode = new ArrayList();
        ArrayList arr_BnsNatureCode = new ArrayList(); // JHMN 20170126 added array for nature of business code 
        private string bnsCode = string.Empty;
        private string bnsNatureCode = string.Empty; // JHMN 20170126 added for nature of business code 
        private string m_sReportName = string.Empty;
        Boolean m_boSwPrinting;

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

        private int m_iRadioBrgy = -1;

        private void frmBussList_Load(object sender, EventArgs e)
        {
            m_strRevYear = AppSettingsManager.GetConfigValue("07");
            LoadBrgy();
            LoadBusinessType();
            LoadDistrict();
            LoadNatureBusiness();

            cmbBrgy.SelectedIndex = 0;
            cmbBussType.SelectedIndex = 0;
            cmbDistName.SelectedIndex = 0;
            cmbNatureBuss.SelectedIndex = 0;
            cmbOrgKind.SelectedIndex = 0;
            cmbBussStatus.SelectedIndex = 0;
            //rdoBrgy.Checked = true;   // RMC 20150429 corrected reports, put rem
            // RMC 20150429 corrected reports (s)
            rdoDummy.Checked = true;    
            if(AppSettingsManager.GetConfigValue("01") == "MUNICIPALITY")
                rdoDst.Enabled = false;
            // RMC 20150429 corrected reports (e)
            lblPercent.Visible = false;
        }

        private void EnableControls(bool blnEnable)
        {
            cmbBrgy.Enabled = blnEnable;
            cmbDistName.Enabled = blnEnable;
            cmbBussStatus.Enabled = blnEnable;
            cmbBussType.Enabled = blnEnable;
            cmbNatureBuss.Enabled = blnEnable;
            cmbOrgKind.Enabled = blnEnable;
            txtGrossFrom.Enabled = blnEnable;
            txtGrossTo.Enabled = blnEnable;
            txtTaxYear.Enabled = blnEnable;
            txtTopCap.Enabled = blnEnable;
        }

        private void GetORrangeYES()
        {
            frmORRange dlgORRange = new frmORRange();
            dlgORRange.m_sTitle = "LIST";
            dlgORRange.ShowDialog();

            m_sORRangeFrom = dlgORRange.Datefrom;
            m_sORRangeTo = dlgORRange.Dateto;
            txtTaxYear.Enabled = false;
            txtTaxYear.Text = "";
        }

        private void GetORrangeNO()
        {
            m_sORRangeFrom = "%";
            m_sORRangeTo = "%";
            txtTaxYear.Enabled = true; // GDE 20090518 request # MAO-09-2014
            txtTaxYear.Text = "";
            txtTaxYear.Focus();
        }

        private void LoadBrgy()
        {
            cmbBrgy.Items.Add("ALL"); //JARS 20170510 TO
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "SELECT DISTINCT(TRIM(BRGY_NM)) FROM BRGY  ORDER BY TRIM(BRGY_NM) ASC";
            if (pSet.Execute())
            {
                //cmbBrgy.Items.Add("ALL"); //JARS 20170510 FROM
                while (pSet.Read())
                {
                    cmbBrgy.Items.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();
        }

        private void LoadDistrict()
        {
            OracleResultSet pSet = new OracleResultSet();
            cmbDistName.Items.Add("ALL"); //JARS 20170510 TO
            pSet.Query = "SELECT DISTINCT(TRIM(DIST_NM)) FROM BRGY  ORDER BY TRIM(DIST_NM) ASC";
            if (pSet.Execute())
            {
                //cmbDistName.Items.Add("ALL"); //JARS 20170510 FROM

                while (pSet.Read())
                {
                    cmbDistName.Items.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();
        }

        private void LoadBusinessType()
        {
            cmbBussType.Items.Clear(); // JHMN 20170126 clear combobox collections
            arr_BnsCode.Clear();
            cmbBussType.Items.Add("ALL");
            OracleResultSet pSet = new OracleResultSet();
            /*SELECT BNS_DESC FROM BNS_TABLE WHERE FEES_CODE = 'B' AND LENGTH(RTRIM(BNS_CODE)) = 2 AND REV_YEAR = '%S' ORDER BY BNS_CODE*/
            pSet.Query = string.Format("SELECT BNS_DESC,BNS_CODE FROM BNS_TABLE WHERE FEES_CODE = 'B' AND LENGTH(RTRIM(BNS_CODE)) = 2 AND REV_YEAR = '{0}' ORDER BY BNS_CODE", m_strRevYear);
            if (pSet.Execute())
            {
                //cmbBussType.Items.Add("ALL");
                arr_BnsCode.Add("");
                while (pSet.Read())
                {
                    cmbBussType.Items.Add(pSet.GetString(0).Trim());
                    arr_BnsCode.Add(pSet.GetString(1).Trim());
                }
            }
            pSet.Close();
        }

        private void LoadNatureBusiness()
        {
            cmbNatureBuss.Text = ""; // JHMN 20170126 reset collections (s)
            cmbNatureBuss.Items.Clear();
            arr_BnsNatureCode.Clear();// JHMN 20170126 reset collections (e)
            cmbNatureBuss.Items.Add("ALL"); // JHMN 20170126 add the query result to combobox and array collection (s)
            OracleResultSet pSet = new OracleResultSet();
            /*pSet.Query = string.Format("select bns_code from bns_table where fees_code = 'B' and bns_desc = '{0}' and rev_year = '{1}'", StringUtilities.HandleApostrophe(sRowBnsCode), m_strRevYear);*/
            pSet.Query = string.Format("SELECT BNS_DESC FROM BNS_TABLE WHERE FEES_CODE = 'B' AND LENGTH(RTRIM(BNS_CODE)) > 3 AND REV_YEAR = '{0}' AND RTRIM(BNS_CODE) LIKE '{1}' ORDER BY BNS_CODE", m_strRevYear, bnsCode + "%%");
            if (pSet.Execute())
            {
                /*cmbNatureBuss.Items.Add("ALL");
                
                while (pSet.Read())
                {
                    cmbNatureBuss.Items.Add(pSet.GetString(0).Trim());
                }*/

                //cmbNatureBuss.Items.Add("ALL"); // JHMN 20170126 add the query result to combobox and array collection (s)
                arr_BnsNatureCode.Add("");
                while (pSet.Read())
                {
                    cmbNatureBuss.Items.Add(pSet.GetString(0).Trim());
                    arr_BnsNatureCode.Add(pSet.GetString(1).Trim());
                }// JHMN 20170126 add the query result to combobox and array collection (e)
            }
            pSet.Close();
        }

        private void rdoBrgy_Click(object sender, EventArgs e)
        {
            m_iRadioBrgy = 0;
            EnableControls(false);
            cmbBrgy.Enabled = true;
            txtTaxYear.Enabled = true;

            string sTempValue = "";

            //// RMC 20150429 corrected reports (s)
            //LoadBrgy();
            //LoadBusinessType();
            //LoadDistrict();
            //LoadNatureBusiness();
            //// RMC 20150429 corrected reports (e)

            /*using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Set by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "O.R. Date Range";
                MsgBoxOptions.RadioNo = "Tax Year";
                MsgBoxOptions.ShowDialog();

                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                GetORrangeYES();
            else if (sTempValue != String.Empty)
                GetORrangeNO();*/
            // RMC 20150429 corrected reports, put rem, (List of Business is based on Records)
        }

        private void rdoDst_Click(object sender, EventArgs e)
        {
            m_iRadioBrgy = 1;
            EnableControls(false);
            cmbDistName.Enabled = true;
            txtTaxYear.Enabled = true;

            //// RMC 20150429 corrected reports (s)
            //LoadBrgy();
            //LoadBusinessType();
            //LoadDistrict();
            //LoadNatureBusiness();
            //// RMC 20150429 corrected reports (e)
        }

        private void rdoMainBuss_Click(object sender, EventArgs e)
        {
            m_iRadioBrgy = 2;
            EnableControls(false);
            cmbBrgy.Enabled = true;
            cmbBussType.Enabled = true;
            cmbNatureBuss.Enabled = true;
            txtTaxYear.Enabled = true;

            string sTempValue = "";

            //// RMC 20150429 corrected reports (s)
            //LoadBrgy();
            //LoadBusinessType();
            //LoadDistrict();
            //LoadNatureBusiness();
            //// RMC 20150429 corrected reports (e)
            /*using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Set by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "O.R. Date Range";
                MsgBoxOptions.RadioNo = "Tax Year";
                MsgBoxOptions.ShowDialog();

                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                GetORrangeYES();
            else if (sTempValue != String.Empty)
                GetORrangeNO();*/
            // RMC 20150429 corrected reports, put rem, (List of Business is based on Records)
        }

        private void rdoOwner_Click(object sender, EventArgs e)
        {
            m_iRadioBrgy = 3;
            EnableControls(false);
            cmbBrgy.Enabled = true;
            cmbOrgKind.Enabled = true;
            txtTaxYear.Enabled = true;

            string sTempValue = "";
            //// RMC 20150429 corrected reports (s)
            //LoadBrgy();
            //LoadBusinessType();
            //LoadDistrict();
            //LoadNatureBusiness();
            //// RMC 20150429 corrected reports (e)

            /*using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Set by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "O.R. Date Range";
                MsgBoxOptions.RadioNo = "Tax Year";
                MsgBoxOptions.ShowDialog();

                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                GetORrangeYES();
            else if (sTempValue != String.Empty)
                GetORrangeNO();*/
            // RMC 20150429 corrected reports, put rem, (List of Business is based on Records)
        }

        private void rdoBussStat_Click(object sender, EventArgs e)
        {
            m_iRadioBrgy = 4;
            EnableControls(false);
            cmbBrgy.Enabled = true;
            cmbBussStatus.Enabled = true;
            txtTaxYear.Enabled = true;

            string sTempValue = "";
            //// RMC 20150429 corrected reports (s)
            //LoadBrgy();
            //LoadBusinessType();
            //LoadDistrict();
            //LoadNatureBusiness();
            //// RMC 20150429 corrected reports (e)

            /*using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Set by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "O.R. Date Range";
                MsgBoxOptions.RadioNo = "Tax Year";
                MsgBoxOptions.ShowDialog();

                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                GetORrangeYES();
            else if (sTempValue != String.Empty)
                GetORrangeNO();*/
            // RMC 20150429 corrected reports, put rem, (List of Business is based on Records)
        }

        private void rdoGross_Click(object sender, EventArgs e)
        {
            m_iRadioBrgy = 5;
            EnableControls(false);
            cmbBrgy.Enabled = true;
            txtGrossFrom.Enabled = true;
            txtGrossTo.Enabled = true;
            txtTaxYear.Enabled = true;

            string sTempValue = "";
            //// RMC 20150429 corrected reports (s)
            //LoadBrgy();
            //LoadBusinessType();
            //LoadDistrict();
            //LoadNatureBusiness();
            //// RMC 20150429 corrected reports (e)

            /*using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Set by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "O.R. Date Range";
                MsgBoxOptions.RadioNo = "Tax Year";
                MsgBoxOptions.ShowDialog();

                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                GetORrangeYES();
            else if (sTempValue != String.Empty)
                GetORrangeNO();*/
            // RMC 20150429 corrected reports, put rem, (List of Business is based on Records)
        }

        private void rdoPrmt_Click(object sender, EventArgs e)
        {
            m_iRadioBrgy = 6;
            EnableControls(false);
            cmbBrgy.Enabled = true;
            txtTaxYear.Enabled = true;

            //// RMC 20150429 corrected reports (s)
            //LoadBrgy();
            //LoadBusinessType();
            //LoadDistrict();
            //LoadNatureBusiness();
            //// RMC 20150429 corrected reports (e)
        }

        private void rdpQrtPaid_Click(object sender, EventArgs e)
        {
            m_iRadioBrgy = 7;
            EnableControls(false);
        }

        private void txtTaxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            /*if (m_iRadioBrgy != 7)
                if ((m_sORRangeFrom == "" || m_sORRangeTo == "") && txtTaxYear.Text == String.Empty)
                    return;*/
            // RMC 20150429 corrected reports, put rem, (List of Business is based on Records)

            switch (m_iRadioBrgy)
            {
                case 0:
                    {
                        m_sReportName = "LIST OF BUSINESSES BY BARANGAY";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            OnReportListBnsBrgy();
                        }
                        break;
                    }
                case 1:
                    {
                        m_sReportName = "LIST OF BUSINESSES BY DISTRICT";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            OnReportListBnsDistrict();
                        }
                        break;
                    }
                case 2:
                    {
                        m_sReportName = "LIST OF BUSINESSES BY MAIN BUSINESS";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            OnReportListBnsMainBusiness();
                        }
                        break;
                    }
                case 3:
                    {
                        m_sReportName = "LIST OF BUSINESSES BY ORGANIZATION KIND";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            OnReportListBnsOrganizationKind();
                        }
                        break;
                    }
                case 4:
                    {
                        m_sReportName = "LIST OF BUSINESSES BY BUSINESS STATUS";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            OnReportListBnsBusinessStatus();
                        }
                        break;
                    }
                case 5:
                    {
                        m_sReportName = "LIST OF BUSINESSES BY GROSS RECEIPTS";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            OnReportListBnsGrossReceipts();
                        }
                        break;
                    }
                case 6:
                    {
                        m_sReportName = "LIST OF BUSINESSES BY PERMIT NUMBER";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            String m_sDateFrom = "";
                            String m_sDateTo = "";

                            frmDatePeriod DlgDtPd = new frmDatePeriod();

                            frmMsgBoxOptions frmMsgBoxOptions = new frmMsgBoxOptions();
                            frmMsgBoxOptions.RadioType = 0;
                            frmMsgBoxOptions.MessageCaption = "Input Permit Date?";
                            frmMsgBoxOptions.RadioYes = "Permit Date";
                            frmMsgBoxOptions.RadioNo = "O.R. Date";
                            frmMsgBoxOptions.ShowDialog();

                            if (frmMsgBoxOptions.SelectedText == "Yes")
                            {
                                DateTime odt;

                                m_sDateFrom = "01/01/" + txtTaxYear.Text;
                                m_sDateTo = AppSettingsManager.GetSystemDate().ToString();
                                odt = AppSettingsManager.GetSystemDate();

                                if (Convert.ToInt32(txtTaxYear.Text) < odt.Year)
                                    m_sDateTo = "12/31/" + txtTaxYear.Text;
                                else
                                    m_sDateTo = string.Format("{0}/{1}/{2}", odt.Month, odt.Day, txtTaxYear.Text);

                                DlgDtPd.Datefrom = m_sDateFrom;
                                DlgDtPd.Dateto = m_sDateTo;
                                DlgDtPd.ShowDialog();

                                m_sDateFrom = DlgDtPd.Datefrom;
                                m_sDateTo = DlgDtPd.Dateto;

                                if (DlgDtPd.IsOK)
                                    OnReportListBnsPermitNumber(m_sDateFrom, m_sDateTo);
                            }
                            else if (frmMsgBoxOptions.SelectedText != String.Empty)
                            {
                                DlgDtPd.Text = ("O.R. DATE");
                                DateTime odt;
                                m_sDateFrom = "01/01/" + txtTaxYear.Text;
                                m_sDateTo = AppSettingsManager.GetSystemDate().ToString();
                                odt = AppSettingsManager.GetSystemDate();

                                if (Convert.ToInt32(txtTaxYear.Text) < odt.Year)
                                    m_sDateTo = "12/31/" + txtTaxYear.Text;
                                else
                                    m_sDateTo = string.Format("{0}/{1}/{2}", odt.Month, odt.Day, txtTaxYear.Text);

                                DlgDtPd.Datefrom = m_sDateFrom;
                                DlgDtPd.Dateto = m_sDateTo;
                                DlgDtPd.ShowDialog();

                                m_sDateFrom = DlgDtPd.Datefrom;
                                m_sDateTo = DlgDtPd.Dateto;

                                if (DlgDtPd.IsOK)
                                    OnReportListBnsPermitNumber(m_sDateFrom, m_sDateTo);
                            }
                        }
                        break;
                    }
                case 7:
                    {
                        frmReportListBnsQtrPaid dlg = new frmReportListBnsQtrPaid();
                        dlg.ShowDialog();
                        break;
                    }
                case 8: //AFM 20200303 MAO-20-12440
                    {
                        m_sReportName = "BiggestInvestment";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                        {
                            int iTopCapTmp = int.Parse(txtTopCap.Text);
                            OnReportTopCapital();
                        }
                        break;
                    }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnReportTopCapital() //AFM 20200304 MAO-20-12440 BIGGEST INVESTMENT
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string sFormat = string.Empty;
            string sQuery = string.Empty;
            string strBnsCode = string.Empty;
            string strBnsCodeSub = string.Empty;
            string strQBnsCode = string.Empty;
            string strBrgy = string.Empty;
            string strTaxYear = string.Empty;
            string strBin = string.Empty;
            string strBnsName = string.Empty;
            string strBnsAdd = string.Empty;
            double dBnsCap = 0;
            string strBnsStat = string.Empty;
            string strCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            int iTopPercent = Convert.ToInt32(txtTopCap.Text);
            bool isChkd = false;

            if (chkByPercentage.Checked == true)
                isChkd = true;

            if (cmbBrgy.Text == "ALL")
                strBrgy = "%%";
            else
                strBrgy = cmbBrgy.Text;

            if (txtTaxYear.Text == "")
                strTaxYear = "%";
            else
            {
                if (txtTaxYear.Text.Length != 4)
                {
                    MessageBox.Show("Tax year format should be four digit");
                    return;
                }
                else
                    strTaxYear = txtTaxYear.Text;
            }

            strBnsCode = AppSettingsManager.GetBnsCodeByDesc("");

            if (cmbBussType.SelectedIndex > 0)
            {
                // check if by Sub Category
                strBnsCodeSub = AppSettingsManager.GetBnsCodeByDesc(cmbBussType.Text);
                strBnsCode = AppSettingsManager.GetBnsCodeByDesc(cmbBussType.Text);
            }
            else
            {
                strBnsCodeSub = "%";
                strBnsCode = "%";
                //sBnsNatureCode = "";
            }

            if (strBnsCodeSub == "%")
                strQBnsCode = strBnsCode + "%";
            else
                strQBnsCode = strBnsCodeSub + "%";

            //applied based on top grosses query but will get top capital
            result.Query = "select distinct a.bin,bns_nm,a.tax_year,bns_code,own_code,sum(distinct a.capital) + sum(b.capital) as capital ";
            result.Query += string.Format("from businesses a, addl_bns b  where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' and a.tax_year like '{2}' ", strQBnsCode, strBrgy, strTaxYear);
            result.Query += "and ((a.bns_stat = 'NEW' and b.bns_stat = 'NEW') OR (a.bns_stat = 'REN' and b.bns_stat = 'NEW')) and a.bin = b.bin and a.tax_year = b.tax_year and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
            result.Query += "group by a.bin,bns_nm,a.tax_year,bns_code,own_code,dt_operated ";
            result.Query += "union select distinct a.bin,bns_nm,a.tax_year,bns_code,own_code,sum(distinct gr_1) + sum(gross) from buss_hist a ,addl_bns b ";
            result.Query += string.Format("where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' and a.tax_year like '{2}' and a.bns_stat = 'NEW' and a.bin = b.bin and a.tax_year = b.tax_year ", strQBnsCode, strBrgy, strTaxYear);
            result.Query += "and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";

            result.Query += string.Format("and a.bin not in (select bin from businesses where tax_year like '{0}') ", strTaxYear);
            result.Query += string.Format("and a.bin NOT IN (SELECT bin FROM buss_hist WHERE tax_year like '{0}' having count(bin) > 1 group by bin) ", strTaxYear);

            result.Query += "group by a.bin,bns_nm,a.tax_year,bns_code,own_code ";

            result.Query += "union ";
            result.Query += "select bin,bns_nm,tax_year,bns_code,own_code,capital ";
            result.Query += "from businesses where bin not in (select bin from addl_bns where tax_year like '" + strTaxYear + "') and tax_year like '" + strTaxYear + "' ";
            result.Query += "and bns_stat = 'NEW' and bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
            result.Query += "and rtrim(bns_code) like '" + strQBnsCode + "' and bns_brgy like '" + strBrgy + "' ";
            result.Query += "union ";
            result.Query += "select bin,bns_nm,tax_year,bns_code,own_code,capital ";
            result.Query += "from buss_hist where bin not in (select bin from addl_bns  where tax_year like '" + strTaxYear + "') and tax_year like '" + strTaxYear + "' ";
            result.Query += "and bns_stat = 'NEW' and bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
            result.Query += "and rtrim(bns_code) like '" + strQBnsCode + "' and bns_brgy like '" + strBrgy + "' ";

            result.Query += string.Format("and bin not in (select bin from businesses where tax_year like '{0}') ", strTaxYear);
            result.Query += string.Format("and bin in (SELECT bin FROM buss_hist WHERE tax_year like '{0}' having count(bin) > 1 group by bin) ", strTaxYear);
            result.Query += "and permit_dt is not null ";

            result.Query += "order by 6 desc,2";

            sQuery = result.Query;

            frmBussReport dlg = new frmBussReport();

            dlg.IsPercent = isChkd;
            dlg.Percent = iTopPercent.ToString();
            dlg.ReportSwitch = m_sReportName;
            dlg.m_sTaxYear = strTaxYear;
            dlg.Query = sQuery;
            dlg.ShowDialog();
        }

        private void OnReportListBnsBrgy()
        {
            DateTime m_dtSaveTime;
            string sTempBrgyName = String.Empty, sTempTaxYear = String.Empty;
            string sQuery, sBin, sBrgyName, sBnsName, sBnsCode, sBnsCodeSub, sQBnsCode, sBnsDesc, sBnsStat, sBnsPermit, sBnsAddress, sPOrDate, sPTerm, sCurrentDate, sCurrentUser;
            string sPMode, sPOrNo, sPTaxYear, sPQtrPaid, sOrFeesCode, sBussTaxYear;
            double fOrFeesDue, fOrFeesSurch, fOrFeesInt, fOrFeesAmtDue;
            double fTmpFeesDue, fTmpFeesSurch, fTmpFeesInt, fTmpFeesAmtDue;
            double fBTmpFeesDue, fBTmpFeesSurch, fBTmpFeesInt, fBTmpFeesAmtDue, fAllTmpSurchInt;
            double dBnsGross, dBnsCap;
            string sContactNo, sNoEmp, sOwnNm;
            int iProgressCtr;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            try
            {
                //App JobStatus(0);
                if (cmbBrgy.Text == "ALL")
                    sTempBrgyName = "%%";
                else
                    sTempBrgyName = cmbBrgy.Text + "%";

                if (txtTaxYear.Text == "")
                    sTempTaxYear = "%";
                else
                {
                    //JHMN 20170510 added validation for tax year format (s)
                    if (txtTaxYear.Text.Length != 4)
                    {
                        MessageBox.Show("Tax year format should be four digit");
                        return;
                    }
                    else
                        sTempTaxYear = txtTaxYear.Text + "%";
                    //JHMN 20170510 added validation for tax year format (e)
                }

                int iPermit = 0;
                int iQuestion = 0;

                frmMsgBoxOptions frmMsgBoxOptions = new frmMsgBoxOptions();
                frmMsgBoxOptions.RadioType = 1;
                frmMsgBoxOptions.RadioYes = "Businesses with Permit";
                frmMsgBoxOptions.RadioNo = "Businesses without Permit";
                frmMsgBoxOptions.RadioBoth = "Both";
                frmMsgBoxOptions.ShowDialog();

                if (frmMsgBoxOptions.SelectedText == "Yes")
                {
                    iPermit = 0;
                    m_sReportName = m_sReportName + " - With Permit";
                }
                else if (frmMsgBoxOptions.SelectedText == "No")
                {
                    iPermit = 1;
                    m_sReportName = m_sReportName + " - Without Permit";
                }
                else if (frmMsgBoxOptions.SelectedText == "Both")
                {
                    iPermit = 2;
                }
                else
                    return;

                //User
                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

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

                #region Progress
                int intCount = 0;
                int intCountIncreament = 0;

                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();

                Thread.Sleep(500);
                /*if (m_sORRangeFrom.Trim() == "%" || m_sORRangeTo.Trim() == "%" || m_sORRangeFrom.Trim() == "" || m_sORRangeTo.Trim() == "")	// RMC 20110611 added option if or dates are null
                {
                    sQuery = "Select sum(a.cnt) from (select count (*) as cnt from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' ";
                    if (iPermit == 0)
                        sQuery += " and permit_no is not null ";
                    if (iPermit == 1)
                        sQuery += " and permit_no is null ";
                    sQuery += " union select count (*) as cnt from buss_hist where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "') a";
                }
                else
                {
                    sQuery = "Select sum(a.cnt) from (select count(*) as cnt from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' ";
                    if (iPermit == 0)
                        sQuery += " and permit_no is not null ";
                    if (iPermit == 1)
                        sQuery += " and permit_no is null ";
                    sQuery += "and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '%" + sTempTaxYear + "' and or_date between to_date('" + m_sORRangeFrom + "','MM/dd/yyyy') and to_date('" + m_sORRangeTo + "','MM/dd/yyyy'))) a";
                }*/
                // RMC 20150429 corrected reports, put rem, (List of Business is based on Records)

                // RMC 20150429 corrected reports (List of Business is based on Records)(s) edited by JHMN 20170510 added tax year
                sQuery = "select count (*) as cnt from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "'";
                if (iPermit == 0)
                    sQuery += " and permit_no is not null ";
                if (iPermit == 1)
                    sQuery += " and permit_no is null ";
                // RMC 20150429 corrected reports (List of Business is based on Records)(e) edited by JHMN 20170510 added tax year

                pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);

                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
                #endregion
                #region comments
                /*if (m_sORRangeFrom.Trim() == "%" || m_sORRangeTo.Trim() == "%" || m_sORRangeFrom.Trim() == "" || m_sORRangeTo.Trim() == "")	// RMC 20110611 added option if or dates are null
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' ";
                    if (iPermit == 0)
                        sQuery += " and permit_no is not null ";
                    if (iPermit == 1)
                        sQuery += " and permit_no is null ";
                    sQuery += " union select * from buss_hist where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "'";
                    if (iPermit == 0)
                        sQuery += " and permit_no is not null ";
                    if (iPermit == 1)
                        sQuery += " and permit_no is null ";
                }
                else
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' ";
                    if (iPermit == 0)
                        sQuery += " and permit_no is not null ";
                    if (iPermit == 1)
                        sQuery += " and permit_no is null ";
                    sQuery += "and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '%" + sTempTaxYear + "' and or_date between to_date('" + m_sORRangeFrom + "','MM/dd/yyyy') and to_date('" + m_sORRangeTo + "','MM/dd/yyyy'))";
                    if (iPermit == 0)
                        sQuery += " and permit_no is not null ";
                    if (iPermit == 1)
                        sQuery += " and permit_no is null ";
                }*/
                #endregion
                // RMC 20150429 corrected reports (List of Business is based on Records)(s) edited by JHMN 20170510 added tax year
                //if (sTempTaxYear != "")
                if (sTempTaxYear == "" || sTempTaxYear == "%" || sTempTaxYear == "%%")   // RMC 20171128 correction in Business List report
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "%'";
                }
                else
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "%' and tax_year like '%" + sTempTaxYear + "'";
                }
                //sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "%' and tax_year like '%" + sTempTaxYear + "'";
                if (iPermit == 0)
                    //sQuery += " and permit_no is not null ";
                    sQuery += " and permit_no is not null and permit_no = trim(permit_no)"; //AFM 20200218 MAO-20-12356 uninclude blank permit no
                if (iPermit == 1)
                    //sQuery += " and permit_no is null "; //AFM 20200218 MAO-20-12356 uninclude blank permit no
                    sQuery += " and (permit_no is null or permit_no like '% %')"; //AFM 20200218 MAO-20-12356
                // RMC 20150429 corrected reports (List of Business is based on Records)(e) edited by JHMN 20170510 added tax year

                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        dBnsGross = 0;
                        dBnsCap = 0;

                        sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                        sBnsCode = StringUtilities.HandleApostrophe(pSet.GetString("bns_code"));
                        sBnsName = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                        sBrgyName = StringUtilities.HandleApostrophe(pSet.GetString("bns_brgy"));
                        sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                        dBnsGross = pSet.GetDouble("gr_1");// +pSet.GetDouble("gr_2");
                        dBnsCap = pSet.GetDouble("capital");
                        sBnsPermit = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                        sBnsAddress = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBin));
                        sBussTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year")); //CTS 01242003 adding tax year for filtering
                        sOwnNm = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(pSet.GetString("own_code"))); // OwnersName(sOwnCode);
                        // GDE 20090514 added (s){
                        sContactNo = StringUtilities.HandleApostrophe(pSet.GetString("bns_telno"));
                        sNoEmp = StringUtilities.HandleApostrophe(pSet.GetInt("num_employees").ToString());

                        //if (m_sORRangeFrom.Trim() == "%" || m_sORRangeTo.Trim() == "%" || m_sORRangeFrom.Trim() == "" || m_sORRangeTo.Trim() == "")
                        //    sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear); // CTS 01242003 adding tax_year for filtering
                        //else
                        //    sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and or_date between to_date('{2}','MM/dd/yyyy') and to_date('{3}','MM/dd/yyyy') order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear, m_sORRangeFrom, m_sORRangeTo);
                        ////GDE 20090520 add or date filtering (e)}
                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear); //JARS 20151020
                        
                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);   // RMC 20170227 correction in Business List
                        sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin);   //JARS 20170807
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            if (pSet1.Read())
                               
                            {
                                sPMode = StringUtilities.HandleApostrophe(pSet1.GetString("data_mode"));	// JJP 09192005 ORACLE ADJUSTMENT
                                sPOrNo = StringUtilities.HandleApostrophe(pSet1.GetString("or_no"));
                                if (sPOrNo == String.Empty)
                                    sPOrDate = "";
                                else
                                    sPOrDate = StringUtilities.HandleApostrophe(pSet1.GetDateTime("or_date").ToShortDateString());
                                sPTerm = StringUtilities.HandleApostrophe(pSet1.GetString("payment_term"));
                                sPTaxYear = StringUtilities.HandleApostrophe(pSet1.GetString("tax_year"));
                                sPQtrPaid = StringUtilities.HandleApostrophe(pSet1.GetString("qtr_paid"));

                                //if (sPMode != "UNP")
                                //{
                                fOrFeesDue = 0;
                                fOrFeesSurch = 0;
                                fOrFeesInt = 0;
                                fOrFeesAmtDue = 0;
                                fTmpFeesDue = 0;
                                fTmpFeesSurch = 0;
                                fTmpFeesInt = 0;
                                fTmpFeesAmtDue = 0;
                                fBTmpFeesDue = 0;
                                fBTmpFeesSurch = 0;
                                fBTmpFeesInt = 0;
                                fBTmpFeesAmtDue = 0;
                                fAllTmpSurchInt = 0;
                                
                                sQuery = string.Format("select * from or_table where or_no = '{0}' and tax_year = '{1}' order by or_no", sPOrNo, sBussTaxYear);	// JJP 09192005 ORACLE ADJUSTMENT
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    while (pSet2.Read())
                                    {
                                        sOrFeesCode = StringUtilities.HandleApostrophe(pSet2.GetString("fees_code"));
                                        fOrFeesDue = pSet2.GetDouble("fees_due");
                                        fOrFeesSurch = pSet2.GetDouble("fees_surch");
                                        fOrFeesInt = pSet2.GetDouble("fees_pen");

                                        if (sOrFeesCode != "B")
                                        {
                                            fTmpFeesDue = fTmpFeesDue + fOrFeesDue;
                                            fTmpFeesSurch = fTmpFeesSurch + fOrFeesSurch;
                                            fTmpFeesInt = fTmpFeesInt + fOrFeesInt;
                                        }
                                        else
                                        {
                                            fBTmpFeesDue = fBTmpFeesDue + fOrFeesDue;
                                            fBTmpFeesSurch = fBTmpFeesSurch + fOrFeesSurch;
                                            fBTmpFeesInt = fBTmpFeesInt + fOrFeesInt;
                                        }
                                    }
                                    fAllTmpSurchInt = fTmpFeesSurch + fTmpFeesInt + fBTmpFeesSurch + fBTmpFeesInt;
                                }
                                pSet2.Close();

                                //(s)-----------> Saving to Report Table
                                sQuery = string.Format(@"insert into rep_list_bns values('{0}','{1}','{2}','{23}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBrgyName, 30)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsCode, 100)),
                                               StringUtilities.HandleApostrophe(sBin),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsName, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAddress, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               dBnsGross.ToString("0.##"),
                                               dBnsCap.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsPermit, 20)),
                                               fBTmpFeesDue.ToString("0.##"),
                                               fTmpFeesDue.ToString("0.##"),
                                               fAllTmpSurchInt.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(sPOrNo),
                                               StringUtilities.HandleApostrophe(sPOrDate),
                                               StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(sPTaxYear),
                                               StringUtilities.HandleApostrophe(sPQtrPaid),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)),
                                               StringUtilities.HandleApostrophe(sContactNo),
                                               StringUtilities.HandleApostrophe(sNoEmp),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 100)));
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();
                                //(e)-----------> Saving to Report Table
                            }
                        }
                        pSet1.Close();

                        DoSomethingDifferent(string.Format("{0:##} %",(Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                    }
                }
                pSet.Close();

                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);

                frmBussReport dlg = new frmBussReport();
                dlg.ReportSwitch = "ListBnsBrgy";//39;
                dlg.ReportName = m_sReportName;
                dlg.ShowDialog();

                m_sReportName = "LIST OF BUSINESSES BY BARANGAY";
                //(s)----------> Delete Data from Temp Report Table
                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
                //(e)----------> Delete Data from Temp Report Table
            }
            catch (Exception ex) //JARS 2017080
            {
                pSet.Rollback();
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);
                return;
            }
        }

        private void OnReportListBnsDistrict()
        {
            DateTime m_dtSaveTime;
            string sTempBrgyName = String.Empty, sTempTaxYear = String.Empty, sTempBrgyDistName = String.Empty;
            string sQuery, sBin, sBrgyName, sBnsName, sBnsCode, sBnsCodeSub, sQBnsCode, sBnsDesc, sBnsStat, sBnsPermit, sBnsAddress, sPOrDate, sPTerm, sCurrentDate, sCurrentUser, sDistName, sBnsDist, sOwnNm = String.Empty;
            string sPMode, sPOrNo, sPTaxYear, sPQtrPaid, sOrFeesCode, sBussTaxYear; // CTS 01252004 add tax_year for filtering;
            double fOrFeesDue, fOrFeesSurch, fOrFeesInt, fOrFeesAmtDue;
            double fTmpFeesDue, fTmpFeesSurch, fTmpFeesInt, fTmpFeesAmtDue;
            double fBTmpFeesDue, fBTmpFeesSurch, fBTmpFeesInt, fBTmpFeesAmtDue, fAllTmpSurchInt;
            double dBnsGross, dBnsCap;
            int iProgressCtr;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            try
            {
                //App JobStatus(0);
                if (cmbDistName.Text == "ALL")
                    sTempBrgyDistName = "%%";
                else
                    sTempBrgyDistName = cmbDistName.Text + "%";

                if (txtTaxYear.Text == "")
                    sTempTaxYear = "%";
                else
                {
                    //JHMN 20170510 added validation for tax year format (s)
                    if (txtTaxYear.Text.Length != 4)
                    {
                        MessageBox.Show("Tax year format should be four digit");
                        return;
                    }
                    else
                        sTempTaxYear = txtTaxYear.Text + "%";
                    //JHMN 20170510 added validation for tax year format (e)
                }

                //User
                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);


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

                //--(s) JJP Count Total Records
                iProgressCtr = 0;
                //pApp->iProgressTotal = 0;

                #region Progress
                //sQuery = string.Format("select count(*) as iCount from businesses where rtrim(bns_dist) like '{0}'", sTempBrgyDistName);
                //pSet = new OracleResultSet();
                //pSet.Query = sQuery;
                //if (pSet.Execute())
                //    if (pSet.Read())
                //        iProgressTotal = pSet.GetInt("iCount");
                //pSet.Close();
                int intCount = 0;
                int intCountIncreament = 0;

                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();

                Thread.Sleep(500);
                /*if (m_sORRangeFrom.Trim() == "%" || m_sORRangeTo.Trim() == "%" || m_sORRangeFrom.Trim() == "" || m_sORRangeTo.Trim() == "")	// RMC 20110611 added option if or dates are null
                {
                    sQuery = "Select sum(a.cnt) from (select count (*) as cnt from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' ";
                    if (iPermit == 0)
                        sQuery += " and permit_no is not null ";
                    if (iPermit == 1)
                        sQuery += " and permit_no is null ";
                    sQuery += " union select count (*) as cnt from buss_hist where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "') a";
                }
                else
                {
                    sQuery = "Select sum(a.cnt) from (select count(*) as cnt from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' ";
                    if (iPermit == 0)
                        sQuery += " and permit_no is not null ";
                    if (iPermit == 1)
                        sQuery += " and permit_no is null ";
                    sQuery += "and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '%" + sTempTaxYear + "' and or_date between to_date('" + m_sORRangeFrom + "','MM/dd/yyyy') and to_date('" + m_sORRangeTo + "','MM/dd/yyyy'))) a";
                }*/
                // RMC 20150429 corrected reports, put rem, (List of Business is based on Records)

                // RMC 20150429 corrected reports (List of Business is based on Records)(s) edited by JHMN 20170510 added tax year
                //sQuery = "select count (*) as cnt from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "'";
                //if (iPermit == 0)
                //    sQuery += " and permit_no is not null ";
                //if (iPermit == 1)
                //    sQuery += " and permit_no is null ";
                // RMC 20150429 corrected reports (List of Business is based on Records)(e) edited by JHMN 20170510 added tax year

                //pSet.Query = sQuery;
                //int.TryParse(pSet.ExecuteScalar(), out intCount);

                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
                #endregion
                //if (sTempTaxYear != "") //JARS 20170807
                if (sTempTaxYear != "" && sTempTaxYear != "%" && sTempTaxYear != "%%")   // RMC 20171128 correction in Business List report
                {
                    sQuery = string.Format("select * from businesses where bns_dist like '{0}' and tax_year like '%{1}' order by bns_dist,bns_nm", sTempBrgyDistName, sTempTaxYear); //JHMN 20170510 remove the rtrim function
                }
                else
                {
                    sQuery = string.Format("select * from businesses where bns_dist like '{0}' order by bns_dist,bns_nm", sTempBrgyDistName); //JHMN 20170510 remove the rtrim function
                }
                //sQuery = string.Format("select * from businesses where bns_dist like '{0}' and tax_year like '%{1}' order by bns_dist,bns_nm", sTempBrgyDistName, sTempTaxYear); //JHMN 20170510 remove the rtrim function

                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        iProgressCtr = iProgressCtr + 1;

                        dBnsGross = 0;
                        dBnsCap = 0;

                        sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                        sBnsCode = StringUtilities.HandleApostrophe(pSet.GetString("bns_code"));
                        sBnsName = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                        sOwnNm = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(pSet.GetString("own_code")));
                        sBrgyName = StringUtilities.HandleApostrophe(pSet.GetString("bns_brgy"));
                        sBnsDist = StringUtilities.HandleApostrophe(pSet.GetString("bns_dist"));
                        sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                        dBnsGross = pSet.GetDouble("gr_1"); //+ pSet.GetDouble("gr_2");
                        dBnsCap = pSet.GetDouble("capital");
                        sBnsPermit = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                        sBnsAddress = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBin));
                        sBussTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));

                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);
                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);  // RMC 20170227 correction in Business List
                        sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin);  //JARS 20170807
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            if (pSet1.Read())
                            {
                                sPMode = StringUtilities.HandleApostrophe(pSet1.GetString("data_mode"));	// JJP 09192005 ORACLE ADJUSTMENT
                                sPOrNo = StringUtilities.HandleApostrophe(pSet1.GetString("or_no"));
                                if (sPOrNo == String.Empty)
                                    sPOrDate = "";
                                else
                                    sPOrDate = StringUtilities.HandleApostrophe(pSet1.GetDateTime("or_date").ToShortDateString());
                                sPTerm = StringUtilities.HandleApostrophe(pSet1.GetString("payment_term"));
                                sPTaxYear = StringUtilities.HandleApostrophe(pSet1.GetString("tax_year"));
                                sPQtrPaid = StringUtilities.HandleApostrophe(pSet1.GetString("qtr_paid"));

                                //if (sPMode != "UNP")
                                //{
                                fOrFeesDue = 0;
                                fOrFeesSurch = 0;
                                fOrFeesInt = 0;
                                fOrFeesAmtDue = 0;
                                fTmpFeesDue = 0;
                                fTmpFeesSurch = 0;
                                fTmpFeesInt = 0;
                                fTmpFeesAmtDue = 0;
                                fBTmpFeesDue = 0;
                                fBTmpFeesSurch = 0;
                                fBTmpFeesInt = 0;
                                fBTmpFeesAmtDue = 0;
                                fAllTmpSurchInt = 0;

                                sQuery = string.Format("select * from or_table where or_no = '{0}' and tax_year = '{1}' order by or_no", sPOrNo, sBussTaxYear);
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    while (pSet2.Read())
                                    {
                                        sOrFeesCode = StringUtilities.HandleApostrophe(pSet2.GetString("fees_code"));
                                        fOrFeesDue = pSet2.GetDouble("fees_due");
                                        fOrFeesSurch = pSet2.GetDouble("fees_surch");
                                        fOrFeesInt = pSet2.GetDouble("fees_pen");

                                        if (sOrFeesCode != "B")
                                        {
                                            fTmpFeesDue = fTmpFeesDue + fOrFeesDue;
                                            fTmpFeesSurch = fTmpFeesSurch + fOrFeesSurch;
                                            fTmpFeesInt = fTmpFeesInt + fOrFeesInt;
                                        }
                                        else
                                        {
                                            fBTmpFeesDue = fBTmpFeesDue + fOrFeesDue;
                                            fBTmpFeesSurch = fBTmpFeesSurch + fOrFeesSurch;
                                            fBTmpFeesInt = fBTmpFeesInt + fOrFeesInt;
                                        }
                                    }
                                    fAllTmpSurchInt = fTmpFeesSurch + fTmpFeesInt + fBTmpFeesSurch + fBTmpFeesInt;
                                }
                                pSet2.Close();

                                //(s)-----------> Saving to Report Table
                                sQuery = string.Format(@"insert into rep_list_bns values('{0}','{1}','{2}','{21}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','','')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                               StringUtilities.HandleApostrophe(sBnsDist),
                                               StringUtilities.HandleApostrophe(sBnsCode),
                                               StringUtilities.HandleApostrophe(sBin),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsName, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAddress, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               dBnsGross.ToString("0.##"),
                                               dBnsCap.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsPermit, 20)),
                                               fBTmpFeesDue.ToString("0.##"),
                                               fTmpFeesDue.ToString("0.##"),
                                               fAllTmpSurchInt.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(sPOrNo),
                                               StringUtilities.HandleApostrophe(sPOrDate),
                                               StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(sPTaxYear),
                                               StringUtilities.HandleApostrophe(sPQtrPaid),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 100)));
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();
                                //(e)-----------> Saving to Report Table
                            }
                        }
                        pSet1.Close();
                        //pApp->JobStatus(iProgressCtr);
                    }
                }
                pSet.Close();
                //pApp->JobStatus(pApp->iProgressTotal+1);
                //OnPrintReportListBnsDistrict(m_sReportName, AppSettingsManager.SystemUser.UserCode);

                frmBussReport dlg = new frmBussReport();
                dlg.ReportSwitch = "ListBnsDist";//39;
                dlg.ReportName = m_sReportName;
                dlg.ShowDialog();

                //(s)----------> Delete Data from Temp Report Table
                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
                //(e)----------> Delete Data from Temp Report Table
            }
            catch (Exception ex) //JARS 2017080
            {
                pSet.Rollback();
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);
                return;
            }
        }

        private void OnReportListBnsMainBusiness()
        {
            DateTime m_dtSaveTime;
            string sTempBrgyName = String.Empty, sTempTaxYear = String.Empty;
            string sQuery, sBin, sBrgyName, sBnsName, sBnsCode = String.Empty, sBnsCodeSub, sQBnsCode, sBnsDesc = String.Empty, sBnsNatureCode = string.Empty, sBnsStat, sBnsPermit, sBnsAddress, sPOrDate, sPTerm, sCurrentDate, sCurrentUser; //JHMN 20170126 added sBnsNatureCode
            string sPMode, sPOrNo, sPTaxYear, sPQtrPaid, sOrFeesCode, sBussTaxYear;
            double fOrFeesDue, fOrFeesSurch, fOrFeesInt, fOrFeesAmtDue;
            double fTmpFeesDue, fTmpFeesSurch, fTmpFeesInt, fTmpFeesAmtDue;
            double fBTmpFeesDue, fBTmpFeesSurch, fBTmpFeesInt, fBTmpFeesAmtDue, fAllTmpSurchInt;
            double dBnsGross, dBnsCap;
            string sContactNo, sNoEmp, sOwnNm;
            int iProgressCtr;

            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            try
            {
                //App JobStatus(0);
                if (cmbBrgy.Text == "ALL")
                    sTempBrgyName = "%%";
                else
                    sTempBrgyName = cmbBrgy.Text + "%";

                if (txtTaxYear.Text == "")
                    sTempTaxYear = "%";
                else
                {
                    //JHMN 20170510 added validation for tax year format (s)
                    if (txtTaxYear.Text.Length != 4)
                    {
                        MessageBox.Show("Tax year format should be four digit");
                        return;
                    }
                    else
                        sTempTaxYear = txtTaxYear.Text + "%";
                    //JHMN 20170510 added validation for tax year format (e)
                }

                if (cmbBussType.SelectedIndex > 0)
                {
                    // check if by Sub Category
                    sBnsCodeSub = AppSettingsManager.GetBnsCodeByDesc(cmbBussType.Text);
                    sBnsCode = AppSettingsManager.GetBnsCodeByDesc(cmbBussType.Text);
                }
                else
                {
                    sBnsCodeSub = "%";
                    sBnsCode = "%";
                    sBnsNatureCode = "";
                }

                if (sBnsCodeSub == "%")
                    sQBnsCode = sBnsCode + "%";
                else
                    sQBnsCode = sBnsCodeSub + "%";

                //User
                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

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

                #region Progress
                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();

                Thread.Sleep(500);

                int intCount = 0;
                int intCountIncreament = 0;

                /*if (m_sORRangeFrom.Trim() == "%" || m_sORRangeTo.Trim() == "%")
                {
                    sQuery = "select sum(a.cnt) from (select count(*) as cnt from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and tax_year like '" + sTempTaxYear + "' and rtrim(bns_code) like '" + sQBnsCode + "' ";
                    sQuery += " union select count(*) as cnt from buss_hist where rtrim(bns_brgy) like '" + sTempBrgyName + "' and tax_year like '" + sTempTaxYear + "' and rtrim(bns_code) like '" + sQBnsCode + "') a";
                }
                else
                {
                    sQuery = "select sum(a.cnt) from (select count(*) as cnt from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and tax_year like '" + sTempTaxYear + "' and rtrim(bns_code) like '" + sQBnsCode + "' ";
                    sQuery += "and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '" + sTempTaxYear + "' and or_date between to_date('" + m_sORRangeFrom + "','MM/dd/yyyy') and to_date('" + m_sORRangeTo + "','MM/dd/yyyy')))a";

                    
                }*/

                // RMC 20150429 corrected reports (List of Business is based on Records)(s) edited by JHMN 20170510 added tax year
                sQuery = "select count(*) as cnt from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and rtrim(bns_code) like '" + sQBnsCode + "' and tax_year like '%" + sTempTaxYear + "'";
                // RMC 20150429 corrected reports (List of Business is based on Records)(e) edited by JHMN 20170510 added tax year

                pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);

                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

                #endregion

                /*if (m_sORRangeFrom.Trim() == "%" || m_sORRangeTo.Trim() == "%")
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and tax_year like '" + sTempTaxYear + "' and rtrim(bns_code) like '" + sQBnsCode + "' ";
                    sQuery += " union select * from buss_hist where rtrim(bns_brgy) like '" + sTempBrgyName + "' and tax_year like '" + sTempTaxYear + "' and rtrim(bns_code) like '" + sQBnsCode + "' ";
                }
                else
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and tax_year like '" + sTempTaxYear + "' and rtrim(bns_code) like '" + sQBnsCode + "' ";
                    sQuery += "and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '" + sTempTaxYear + "' and or_date between to_date('" + m_sORRangeFrom + "','MM/dd/yyyy') and to_date('" + m_sORRangeTo + "','MM/dd/yyyy'))";
                }*/
                // RMC 20150429 corrected reports (List of Business is based on Records)(s) edited by JHMN 20170126
                //if (sTempTaxYear != "") //JARS 20170807
                if (sTempTaxYear != "" && sTempTaxYear != "%" && sTempTaxYear != "%%")   // RMC 20171128 correction in Business List report
                {
                    if (cmbNatureBuss.Text == "" || cmbNatureBuss.Text == "ALL")
                    {
                        sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and rtrim(bns_code) like '" + sQBnsCode + "' and tax_year like '" + sTempTaxYear + "'";
                    }
                    else
                    {
                        sBnsNatureCode = AppSettingsManager.GetBnsCodeByDesc(cmbNatureBuss.Text);
                        sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and rtrim(bns_code) like '" + sQBnsCode + "' and rtrim(bns_code) = '" + sBnsNatureCode + "' and tax_year like '" + sTempTaxYear + "'";
                    }
                }
                else
                {
                    if (cmbNatureBuss.Text == "" || cmbNatureBuss.Text == "ALL")
                    {
                        sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and rtrim(bns_code) like '" + sQBnsCode + "'";
                    }
                    else
                    {
                        sBnsNatureCode = AppSettingsManager.GetBnsCodeByDesc(cmbNatureBuss.Text);
                        sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and rtrim(bns_code) like '" + sQBnsCode + "' and rtrim(bns_code) = '" + sBnsNatureCode + "'";
                    }

                }
                // RMC 20150429 corrected reports (List of Business is based on Records)(e) edited by JHMN 20170126

                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        dBnsGross = 0;
                        dBnsCap = 0;

                        sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                        sBnsCode = StringUtilities.HandleApostrophe(pSet.GetString("bns_code"));
                        sBnsName = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                        sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                        dBnsGross = pSet.GetDouble("gr_1");// +pSet.GetDouble("gr_2");
                        dBnsCap = pSet.GetDouble("capital");
                        sBnsPermit = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                        sBnsAddress = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBin));
                        sBussTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year")); //CTS 01242003 adding tax year for filtering
                        sOwnNm = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(pSet.GetString("own_code")));
                        // GDE 20090514 added (s){
                        sContactNo = StringUtilities.HandleApostrophe(pSet.GetString("bns_telno"));
                        sNoEmp = StringUtilities.HandleApostrophe(pSet.GetInt("num_employees").ToString());

                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);
                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);
                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);    // RMC 20170227 correction in Business List
                        sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin);    //JARS 20170807
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            if (pSet1.Read())
                            {
                                sPMode = StringUtilities.HandleApostrophe(pSet1.GetString("data_mode"));	// JJP 09192005 ORACLE ADJUSTMENT
                                sPOrNo = StringUtilities.HandleApostrophe(pSet1.GetString("or_no"));
                                if (sPOrNo == String.Empty)
                                    sPOrDate = "";
                                else
                                    sPOrDate = StringUtilities.HandleApostrophe(pSet1.GetDateTime("or_date").ToShortDateString());
                                sPTerm = StringUtilities.HandleApostrophe(pSet1.GetString("payment_term"));
                                sPTaxYear = StringUtilities.HandleApostrophe(pSet1.GetString("tax_year"));
                                sPQtrPaid = StringUtilities.HandleApostrophe(pSet1.GetString("qtr_paid"));

                                fOrFeesDue = 0;
                                fOrFeesSurch = 0;
                                fOrFeesInt = 0;
                                fOrFeesAmtDue = 0;
                                fTmpFeesDue = 0;
                                fTmpFeesSurch = 0;
                                fTmpFeesInt = 0;
                                fTmpFeesAmtDue = 0;
                                fBTmpFeesDue = 0;
                                fBTmpFeesSurch = 0;
                                fBTmpFeesInt = 0;
                                fBTmpFeesAmtDue = 0;
                                fAllTmpSurchInt = 0;

                                sQuery = string.Format("select bns_desc from bns_table where rtrim(fees_code) like 'B%%' and rtrim(bns_code) like '%{0}' and rev_year = '{1}'", sBnsCode, m_strRevYear);
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    if (pSet2.Read())
                                    {
                                        sBnsDesc = StringUtilities.HandleApostrophe(pSet2.GetString("bns_desc"));
                                    }
                                }
                                pSet2.Close();

                                sQuery = string.Format(@"select * from or_table where or_no = '{0}' and tax_year = '{1}' order by or_no", sPOrNo, sBussTaxYear);	// JJP 09192005 ORACLE ADJUSTMENT
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    while (pSet2.Read())
                                    {
                                        sOrFeesCode = StringUtilities.HandleApostrophe(pSet2.GetString("fees_code"));
                                        fOrFeesDue = pSet2.GetDouble("fees_due");
                                        fOrFeesSurch = pSet2.GetDouble("fees_surch");
                                        fOrFeesInt = pSet2.GetDouble("fees_pen");

                                        if (sOrFeesCode != "B")
                                        {
                                            fTmpFeesDue = fTmpFeesDue + fOrFeesDue;
                                            fTmpFeesSurch = fTmpFeesSurch + fOrFeesSurch;
                                            fTmpFeesInt = fTmpFeesInt + fOrFeesInt;
                                        }
                                        else
                                        {
                                            fBTmpFeesDue = fBTmpFeesDue + fOrFeesDue;
                                            fBTmpFeesSurch = fBTmpFeesSurch + fOrFeesSurch;
                                            fBTmpFeesInt = fBTmpFeesInt + fOrFeesInt;
                                        }
                                    }
                                    fAllTmpSurchInt = fTmpFeesSurch + fTmpFeesInt + fBTmpFeesSurch + fBTmpFeesInt;
                                }
                                pSet2.Close();

                                //(s)-----------> Saving to Report Table
                                sQuery = string.Format(@"insert into rep_list_bns values('{0}','{1}','{2}','{23}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsDesc, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsCode, 100)),
                                               StringUtilities.HandleApostrophe(sBin),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsName, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAddress, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               dBnsGross.ToString("0.##"),
                                               dBnsCap.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsPermit, 20)),
                                               fBTmpFeesDue.ToString("0.##"),
                                               fTmpFeesDue.ToString("0.##"),
                                               fAllTmpSurchInt.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(sPOrNo),
                                               StringUtilities.HandleApostrophe(sPOrDate),
                                               StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(sPTaxYear),
                                               StringUtilities.HandleApostrophe(sPQtrPaid),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)),
                                               StringUtilities.HandleApostrophe(sContactNo),
                                               StringUtilities.HandleApostrophe(sNoEmp),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 100)));
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();
                                //(e)-----------> Saving to Report Table
                            }
                        }
                        pSet1.Close();
                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                    }
                }
                pSet.Close();

                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);

                frmBussReport dlg = new frmBussReport();
                dlg.ReportSwitch = "ListBnsMainBusiness";//39;
                dlg.ReportName = m_sReportName;
                dlg.ShowDialog();

                //(s)----------> Delete Data from Temp Report Table
                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
                //(e)----------> Delete Data from Temp Report Table
            }
            catch (Exception ex) //JARS 2017080
            {
                pSet.Rollback();
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);
                return;
            }
        }

        private void OnReportListBnsOrganizationKind()
        {
            DateTime m_dtSaveTime;
            string sTempBrgyName = String.Empty, sTempOrgKind = String.Empty, sTempTaxYear = String.Empty;
            string sQuery, sBin, sBnsName, sBnsCode, sQBnsCode = String.Empty, sBnsDesc, sBnsStat, sBnsPermit, sBnsAddress, sPOrDate, sBnsOrgn = String.Empty, sPTerm, sCurrentDate, sCurrentUser;
            string sPMode, sPOrNo, sPTaxYear, sPQtrPaid, sOrFeesCode, sBussTaxYear;
            double fOrFeesDue, fOrFeesSurch, fOrFeesInt, fOrFeesAmtDue;
            double fTmpFeesDue, fTmpFeesSurch, fTmpFeesInt, fTmpFeesAmtDue;
            double fBTmpFeesDue, fBTmpFeesSurch, fBTmpFeesInt, fBTmpFeesAmtDue, fAllTmpSurchInt;
            double dBnsGross, dBnsCap;
            string sContactNo, sNoEmp, sOwnNm;
            int iProgressCtr;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            try
            {
                //App JobStatus(0);
                if (cmbBrgy.Text == "ALL")
                    sTempBrgyName = "%%";
                else
                    sTempBrgyName = cmbBrgy.Text + "%";

                if (cmbOrgKind.Text == "ALL")
                    sTempOrgKind = "%%";
                else
                    sTempOrgKind = cmbOrgKind.Text + "%";

                if (txtTaxYear.Text == "")
                    sTempTaxYear = "%%";
                else
                {
                    //JHMN 20170510 added validation for tax year format (s)
                    if (txtTaxYear.Text.Length != 4)
                    {
                        MessageBox.Show("Tax year format should be four digit");
                        return;
                    }
                    else
                        sTempTaxYear = txtTaxYear.Text + "%";
                    //JHMN 20170510 added validation for tax year format (e)
                }

                //User
                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

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

                #region Progress

                int intCount = 0;
                int intCountIncreament = 0;
                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();

                Thread.Sleep(500);

                /*if (m_sORRangeFrom.Trim() == "%" || m_sORRangeTo.Trim() == "%")
                {
                    sQuery = "select sum(a.cnt) from (select count (*) as cnt from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' and rtrim(orgn_kind) like '%" + sTempOrgKind + "' ";
                    sQuery += " union select count (*) as cnt from buss_hist where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' and rtrim(bns_code) like '%" + sQBnsCode + "') a";
                }
                else
                {
                    sQuery = "select count (*) as cnt from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' and rtrim(orgn_kind) like '%" + sTempOrgKind + "' ";
                    sQuery += "and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '%" + sTempTaxYear + "' and or_date between to_date('" + m_sORRangeFrom + "','MM/DD/YYYY') and to_date('" + m_sORRangeTo + "','MM/DD/YYYY'))";
                }*/

                // RMC 20150429 corrected reports (List of Business is based on Records)(s)
                sQuery = "select count (*) as cnt from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and rtrim(orgn_kind) like '%" + sTempOrgKind + "' ";
                // RMC 20150429 corrected reports (List of Business is based on Records)(e)

                pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);

                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

                #endregion

                /*if (m_sORRangeFrom.Trim() == "%" || m_sORRangeTo.Trim() == "%")
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' and rtrim(orgn_kind) like '%" + sTempOrgKind + "' ";
                    sQuery += " union select * from buss_hist where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' and rtrim(bns_code) like '%" + sQBnsCode + "' ";
                }
                else
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and tax_year like '%" + sTempTaxYear + "' and rtrim(orgn_kind) like '%" + sTempOrgKind + "' ";
                    sQuery += "and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '%" + sTempTaxYear + "' and or_date between to_date('" + m_sORRangeFrom + "','MM/DD/YYYY') and to_date('" + m_sORRangeTo + "','MM/DD/YYYY'))";
                }*/
                // RMC 20150429 corrected reports (List of Business is based on Records)(s) edited by JHMN 20170510 added tax year
               //if (sTempTaxYear != "") //JARS 20170807
                if (sTempTaxYear != "" && sTempTaxYear != "%" && sTempTaxYear != "%%")   // RMC 20171128 correction in Business List report
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and rtrim(orgn_kind) like '%" + sTempOrgKind + "' and tax_year like '%" + sTempTaxYear + "'";
                }
                else
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and rtrim(orgn_kind) like '%" + sTempOrgKind + "'";
                }
                //sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and rtrim(orgn_kind) like '%" + sTempOrgKind + "' and tax_year like '%" + sTempTaxYear + "'";
                // RMC 20150429 corrected reports (List of Business is based on Records)(e) edited by JHMN 20170510 added tax year

                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        dBnsGross = 0;
                        dBnsCap = 0;

                        sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                        sBnsCode = StringUtilities.HandleApostrophe(pSet.GetString("bns_code"));
                        sBnsName = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                        sBnsOrgn = StringUtilities.HandleApostrophe(pSet.GetString("orgn_kind"));
                        sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                        dBnsGross = pSet.GetDouble("gr_1");// +pSet.GetDouble("gr_2");
                        dBnsCap = pSet.GetDouble("capital");
                        sBnsPermit = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                        sBnsAddress = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBin));
                        sBussTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year")); //CTS 01242003 adding tax year for filtering
                        sOwnNm = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(pSet.GetString("own_code")));
                        // GDE 20090514 added (s){
                        sContactNo = StringUtilities.HandleApostrophe(pSet.GetString("bns_telno"));
                        sNoEmp = StringUtilities.HandleApostrophe(pSet.GetInt("num_employees").ToString());

                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);
                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);   // RMC 20170227 correction in Business List
                        sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin);   //JARS 20170807
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            if (pSet1.Read())
                            {
                                sPMode = StringUtilities.HandleApostrophe(pSet1.GetString("data_mode"));	// JJP 09192005 ORACLE ADJUSTMENT
                                sPOrNo = StringUtilities.HandleApostrophe(pSet1.GetString("or_no"));
                                if (sPOrNo == String.Empty)
                                    sPOrDate = "";
                                else
                                    sPOrDate = StringUtilities.HandleApostrophe(pSet1.GetDateTime("or_date").ToShortDateString());
                                sPTerm = StringUtilities.HandleApostrophe(pSet1.GetString("payment_term"));
                                sPTaxYear = StringUtilities.HandleApostrophe(pSet1.GetString("tax_year"));
                                sPQtrPaid = StringUtilities.HandleApostrophe(pSet1.GetString("qtr_paid"));

                                fOrFeesDue = 0;
                                fOrFeesSurch = 0;
                                fOrFeesInt = 0;
                                fOrFeesAmtDue = 0;
                                fTmpFeesDue = 0;
                                fTmpFeesSurch = 0;
                                fTmpFeesInt = 0;
                                fTmpFeesAmtDue = 0;
                                fBTmpFeesDue = 0;
                                fBTmpFeesSurch = 0;
                                fBTmpFeesInt = 0;
                                fBTmpFeesAmtDue = 0;
                                fAllTmpSurchInt = 0;

                                sQuery = string.Format("select bns_desc from bns_table where rtrim(fees_code) like 'B%%' and rtrim(bns_code) like '%{0}' and rev_year = '{1}'", sBnsCode, m_strRevYear);
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    if (pSet2.Read())
                                    {
                                        sBnsDesc = StringUtilities.HandleApostrophe(pSet2.GetString("bns_desc"));
                                    }
                                }
                                pSet2.Close();

                                sQuery = string.Format(@"select * from or_table where or_no = '{0}' and tax_year = '{1}' order by or_no", sPOrNo, sBussTaxYear);	// JJP 09192005 ORACLE ADJUSTMENT
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    while (pSet2.Read())
                                    {
                                        sOrFeesCode = StringUtilities.HandleApostrophe(pSet2.GetString("fees_code"));
                                        fOrFeesDue = pSet2.GetDouble("fees_due");
                                        fOrFeesSurch = pSet2.GetDouble("fees_surch");
                                        fOrFeesInt = pSet2.GetDouble("fees_pen");

                                        if (sOrFeesCode != "B")
                                        {
                                            fTmpFeesDue = fTmpFeesDue + fOrFeesDue;
                                            fTmpFeesSurch = fTmpFeesSurch + fOrFeesSurch;
                                            fTmpFeesInt = fTmpFeesInt + fOrFeesInt;
                                        }
                                        else
                                        {
                                            fBTmpFeesDue = fBTmpFeesDue + fOrFeesDue;
                                            fBTmpFeesSurch = fBTmpFeesSurch + fOrFeesSurch;
                                            fBTmpFeesInt = fBTmpFeesInt + fOrFeesInt;
                                        }
                                    }
                                    fAllTmpSurchInt = fTmpFeesSurch + fTmpFeesInt + fBTmpFeesSurch + fBTmpFeesInt;
                                }
                                pSet2.Close();

                                //(s)-----------> Saving to Report Table
                                sQuery = string.Format(@"insert into rep_list_bns values('{0}','{1}','{2}','{23}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsOrgn, 30)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsCode, 100)),
                                               StringUtilities.HandleApostrophe(sBin),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsName, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAddress, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               dBnsGross.ToString("0.##"),
                                               dBnsCap.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsPermit, 20)),
                                               fBTmpFeesDue.ToString("0.##"),
                                               fTmpFeesDue.ToString("0.##"),
                                               fAllTmpSurchInt.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(sPOrNo),
                                               StringUtilities.HandleApostrophe(sPOrDate),
                                               StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(sPTaxYear),
                                               StringUtilities.HandleApostrophe(sPQtrPaid),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)),
                                               StringUtilities.HandleApostrophe(sContactNo),
                                               StringUtilities.HandleApostrophe(sNoEmp),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 100)));
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();
                            }
                        }
                        pSet1.Close();
                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                    }
                }
                pSet.Close();

                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);

                frmBussReport dlg = new frmBussReport();
                dlg.ReportSwitch = "ListBnsOrgKind";//39;
                dlg.ReportName = m_sReportName;
                dlg.ShowDialog();

                //pApp->JobStatus(pApp->iProgressTotal+1);
                //OnPrintReportListBnsOrganizationKind(m_sReportName, AppSettingsManager.SystemUser.UserCode);

                //(s)----------> Delete Data from Temp Report Table
                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
                //(e)----------> Delete Data from Temp Report Table
            }
            catch (Exception ex) //JARS 2017080
            {
                pSet.Rollback();
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);
                return;
            }
        }

        private void OnReportListBnsBusinessStatus()
        {
            DateTime m_dtSaveTime;
            string sTempBrgyName = String.Empty, sTempTaxYear = String.Empty, sTempStatus = String.Empty;
            string sQuery, sBin, sBnsName, sBnsCode, sBnsStat, sBnsPermit, sBnsAddress, sPOrDate, sPTerm, sCurrentDate, sCurrentUser;
            string sPMode, sPOrNo, sPTaxYear, sPQtrPaid, sOrFeesCode, sBussTaxYear;
            double fOrFeesDue, fOrFeesSurch, fOrFeesInt;
            double fTmpFeesDue, fTmpFeesSurch, fTmpFeesInt;
            double fBTmpFeesDue, fBTmpFeesSurch, fBTmpFeesInt, fAllTmpSurchInt;
            double dBnsGross, dBnsCap;
            string sContactNo, sNoEmp, sOwnNm;
            int iProgressCtr;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            try
            {
                //App JobStatus(0);
                if (cmbBrgy.Text == "ALL")
                    sTempBrgyName = "%%";
                else
                    sTempBrgyName = cmbBrgy.Text + "%";

                if (cmbBussStatus.Text == "ALL")
                    sTempStatus = "%%";
                else
                    sTempStatus = cmbBussStatus.Text + "%";

                if (txtTaxYear.Text == "")
                    sTempTaxYear = "%";
                else
                {
                    //JHMN 20170510 added validation for tax year format (s)
                    if (txtTaxYear.Text.Length != 4)
                    {
                        MessageBox.Show("Tax year format should be four digit");
                        return;
                    }
                    else
                        sTempTaxYear = txtTaxYear.Text + "%";
                    //JHMN 20170510 added validation for tax year format (e)
                }

                //User
                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);


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

                #region Progress

                int intCount = 0;
                int intCountIncreament = 0;

                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();

                Thread.Sleep(500);

                /*if (m_sORRangeFrom.Trim() == "%" || m_sORRangeTo.Trim() == "%")
                {
                    sQuery = "select Count (*) from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and tax_year like '" + sTempTaxYear + "' and rtrim(bns_stat) like '" + sTempStatus + "' ";
                }
                else
                {

                    sQuery = "select Count(*) from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and tax_year like '" + sTempTaxYear + "' and rtrim(bns_stat) like '" + sTempStatus + "' ";
                    sQuery += "and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '" + sTempTaxYear + "' and or_date between to_date('" + m_sORRangeFrom + "','MM/dd/yyyy') and to_date('" + m_sORRangeTo + "','MM/dd/yyyy'))";
                }*/

                // RMC 20150429 corrected reports (List of Business is based on Records)(s)
                sQuery = "select Count (*) from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and rtrim(bns_stat) like '%" + sTempStatus + "' ";
                // RMC 20150429 corrected reports (List of Business is based on Records)(e)
                pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);

                #endregion

                /*if (m_sORRangeFrom.Trim() == "%" || m_sORRangeTo.Trim() == "%")
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and tax_year like '" + sTempTaxYear + "' and rtrim(bns_stat) like '" + sTempStatus + "' ";
                }
                else
                {

                    sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and tax_year like '" + sTempTaxYear + "' and rtrim(bns_stat) like '" + sTempStatus + "' ";
                    sQuery += "and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '" + sTempTaxYear + "' and or_date between to_date('" + m_sORRangeFrom + "','MM/dd/yyyy') and to_date('" + m_sORRangeTo + "','MM/dd/yyyy'))";
                }*/

                // RMC 20150429 corrected reports (List of Business is based on Records)(s) edited by JHMN 20170510 added tax year
                //if (sTempTaxYear != "") //JARS 20170807
                if (sTempTaxYear != "" && sTempTaxYear != "%" && sTempTaxYear != "%%")   // RMC 20171128 correction in Business List report
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and rtrim(bns_stat) like '%" + sTempStatus + "' and tax_year like '%" + sTempTaxYear + "'";
                }
                else
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '%" + sTempBrgyName + "' and rtrim(bns_stat) like '%" + sTempStatus + "'";
                }
                // RMC 20150429 corrected reports (List of Business is based on Records)(e) edited by JHMN 20170510 added tax year

                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        dBnsGross = 0;
                        dBnsCap = 0;

                        sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                        sBnsCode = StringUtilities.HandleApostrophe(pSet.GetString("bns_code"));
                        sBnsName = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                        sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                        dBnsGross = pSet.GetDouble("gr_1") + pSet.GetDouble("gr_2");
                        dBnsCap = pSet.GetDouble("capital");
                        sBnsPermit = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                        sBnsAddress = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBin));
                        sBussTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year")); //CTS 01242003 adding tax year for filtering
                        sOwnNm = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(pSet.GetString("own_code")));
                        // GDE 20090514 added (s){
                        sContactNo = StringUtilities.HandleApostrophe(pSet.GetString("bns_telno"));
                        sNoEmp = StringUtilities.HandleApostrophe(pSet.GetInt("num_employees").ToString());

                        string sStat = sBnsStat;
                        if (sStat == "REN")
                            sStat = "RENEWAL";
                        if (sStat == "RET")
                            sStat = "RETIRED";

                        //if (m_sORRangeFrom == "%" || m_sORRangeTo == "%")
                        //    sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear); // CTS 01252004 add tax_year for filtering
                        //else
                        //    sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year like '{1}' and or_date between to_date('{2}','MM/dd/yyyy') and to_date('{3}','MM/dd/yyyy') order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear, m_sORRangeFrom, m_sORRangeTo); //JARS 20151020
                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);
                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);  // RMC 20170227 correction in Business List
                        sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin);  //JARS 20170807
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            if (pSet1.Read())
                            {
                                sPMode = StringUtilities.HandleApostrophe(pSet1.GetString("data_mode"));	// JJP 09192005 ORACLE ADJUSTMENT
                                sPOrNo = StringUtilities.HandleApostrophe(pSet1.GetString("or_no"));
                                if (sPOrNo == String.Empty)
                                    sPOrDate = "";
                                else
                                    sPOrDate = StringUtilities.HandleApostrophe(pSet1.GetDateTime("or_date").ToShortDateString());
                                sPTerm = StringUtilities.HandleApostrophe(pSet1.GetString("payment_term"));
                                sPTaxYear = StringUtilities.HandleApostrophe(pSet1.GetString("tax_year"));
                                sPQtrPaid = StringUtilities.HandleApostrophe(pSet1.GetString("qtr_paid"));

                                fOrFeesDue = 0;
                                fOrFeesSurch = 0;
                                fOrFeesInt = 0;
                                fTmpFeesDue = 0;
                                fTmpFeesSurch = 0;
                                fTmpFeesInt = 0;
                                fBTmpFeesDue = 0;
                                fBTmpFeesSurch = 0;
                                fBTmpFeesInt = 0;
                                fAllTmpSurchInt = 0;

                                sQuery = string.Format(@"select * from or_table where or_no = '{0}' and tax_year = '{1}' order by or_no", sPOrNo, sBussTaxYear);	// JJP 09192005 ORACLE ADJUSTMENT
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    while (pSet2.Read())
                                    {
                                        sOrFeesCode = StringUtilities.HandleApostrophe(pSet2.GetString("fees_code"));
                                        fOrFeesDue = pSet2.GetDouble("fees_due");
                                        fOrFeesSurch = pSet2.GetDouble("fees_surch");
                                        fOrFeesInt = pSet2.GetDouble("fees_pen");

                                        if (sOrFeesCode != "B")
                                        {
                                            fTmpFeesDue = fTmpFeesDue + fOrFeesDue;
                                            fTmpFeesSurch = fTmpFeesSurch + fOrFeesSurch;
                                            fTmpFeesInt = fTmpFeesInt + fOrFeesInt;
                                        }
                                        else
                                        {
                                            fBTmpFeesDue = fBTmpFeesDue + fOrFeesDue;
                                            fBTmpFeesSurch = fBTmpFeesSurch + fOrFeesSurch;
                                            fBTmpFeesInt = fBTmpFeesInt + fOrFeesInt;
                                        }
                                    }
                                    fAllTmpSurchInt = fTmpFeesSurch + fTmpFeesInt + fBTmpFeesSurch + fBTmpFeesInt;
                                }
                                pSet2.Close();

                                //(s)-----------> Saving to Report Table
                                sQuery = string.Format(@"insert into rep_list_bns values('{0}','{1}','{2}','{23}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                               StringUtilities.HandleApostrophe(sStat),
                                               StringUtilities.HandleApostrophe(sBnsCode),
                                               StringUtilities.HandleApostrophe(sBin),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsName, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAddress, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               dBnsGross.ToString("0.##"),
                                               dBnsCap.ToString("0.##"),
                                                StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsPermit, 20)),
                                               fBTmpFeesDue.ToString("0.##"),
                                               fTmpFeesDue.ToString("0.##"),
                                               fAllTmpSurchInt.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(sPOrNo),
                                               StringUtilities.HandleApostrophe(sPOrDate),
                                               StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(sPTaxYear),
                                               StringUtilities.HandleApostrophe(sPQtrPaid),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)),
                                               StringUtilities.HandleApostrophe(sContactNo),
                                               StringUtilities.HandleApostrophe(sNoEmp),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 100)));
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();

                            }
                        }
                        pSet1.Close();
                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                    }
                }
                pSet.Close();

                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);

                frmBussReport dlg = new frmBussReport();
                dlg.ReportSwitch = "ListBnsBusStatus";//39;
                dlg.ReportName = m_sReportName;
                dlg.ShowDialog();

                //(s)----------> Delete Data from Temp Report Table
                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
                //(e)----------> Delete Data from Temp Report Table
            }
            catch (Exception ex) //JARS 2017080
            {
                pSet.Rollback();
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);
                return;
            }

        }

        private void OnReportListBnsGrossReceipts()
        {
            DateTime m_dtSaveTime;
            string sTempBrgyName = String.Empty, sTempTaxYear = String.Empty, sTempStatus = String.Empty, sTempGrossFrom = String.Empty, sTempGrossTo = String.Empty;
            string sQuery, sBin, sBnsName = String.Empty, sBnsOrgn = String.Empty, sBnsCode = String.Empty, sBnsStat = String.Empty, sBnsPermit = String.Empty, sBnsAddress, sPOrDate, sPTerm, sCurrentDate, sCurrentUser;
            string sPMode, sPOrNo, sPTaxYear, sPQtrPaid, sOrFeesCode, sBussTaxYear = String.Empty;
            double fOrFeesDue, fOrFeesSurch, fOrFeesInt;
            double fTmpFeesDue, fTmpFeesSurch, fTmpFeesInt;
            double fBTmpFeesDue, fBTmpFeesSurch, fBTmpFeesInt, fAllTmpSurchInt;
            double dBnsGross, dBnsCap;
            string sContactNo = "", sNoEmp = "", sOwnNm = "";
            int iProgressCtr;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            try
            {
                if (txtGrossFrom.Text == "")
                    sTempGrossFrom = "0";
                else
                    sTempGrossFrom = txtGrossFrom.Text;

                if (txtGrossTo.Text == "")
                    sTempGrossTo = "0";
                else
                    sTempGrossTo = txtGrossTo.Text;

                if (cmbBrgy.Text == "ALL")
                    sTempBrgyName = "%%";
                else
                    sTempBrgyName = cmbBrgy.Text + "%";

                /* 
                if (m_sORRangeFrom == "%" && m_sORRangeTo == "%" && txtTaxYear.Text == "")
                {
                    MessageBox.Show("Tax year required");
                    return;
                }
                 */
                if (txtTaxYear.Text == "")
                    sTempTaxYear = "%";
                else
                {
                    //JHMN 20170510 added validation for tax year format (s)
                    if (txtTaxYear.Text.Length != 4)
                    {
                        MessageBox.Show("Tax year format should be four digit");
                        return;
                    }
                    else
                        sTempTaxYear = txtTaxYear.Text + "%";
                    //JHMN 20170510 added validation for tax year format (e)
                }

                //User
                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);


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

                #region Progress

                int intCount = 0;
                int intCountIncreament = 0;
                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();

                Thread.Sleep(500);


                /*if (m_sORRangeFrom == "%" || m_sORRangeTo == "%")
                {
                    sQuery = "Select sum(a.cnt) from (select Count (*) as cnt from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1+gr_2 between " + sTempGrossFrom + " and " + sTempGrossTo + "";
                    sQuery += " and tax_year = '" + txtTaxYear.Text + "'";
                    sQuery += " union all";
                    sQuery += " select Count (*) as Cnt from buss_hist where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1+gr_2 between " + sTempGrossFrom + " and " + sTempGrossTo + "";
                    sQuery += " and tax_year = '" + txtTaxYear.Text + "') a";
                }
                else
                {
                    sQuery = "select Count (*) as cnt from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1+gr_2 between " + (sTempGrossFrom) + " and " + (sTempGrossTo) + " ";
                    sQuery += " and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '%" + txtTaxYear.Text + "' ";
                    sQuery += " and or_date between to_date('" + m_sORRangeFrom + "','MM/dd/yyyy') ";
                    sQuery += " and to_date('" + m_sORRangeTo + "','MM/dd/yyyy'))";
                }*/
                // RMC 20150429 corrected reports (List of Business is based on Records)(s)
                sQuery = "select Count (*) as cnt from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1+gr_2 between " + sTempGrossFrom + " and " + sTempGrossTo + "";
                // RMC 20150429 corrected reports (List of Business is based on Records)(e)

                pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);

                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

                #endregion

                /*if (m_sORRangeFrom == "%" || m_sORRangeTo == "%")
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1+gr_2 between " + sTempGrossFrom + " and " + sTempGrossTo + "";
                    sQuery += " and tax_year = '" + txtTaxYear.Text + "'";
                    sQuery += " union all";
                    sQuery += " select * from buss_hist where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1+gr_2 between " + sTempGrossFrom + " and " + sTempGrossTo + "";
                    sQuery += " and tax_year = '" + txtTaxYear.Text + "'";
                }
                else
                {
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1+gr_2 between " + (sTempGrossFrom) + " and " + (sTempGrossTo) + " ";
                    sQuery += " and bin in (select distinct(bin) from pay_hist where bin = businesses.bin and tax_year like '%" + txtTaxYear.Text + "' ";
                    sQuery += " and or_date between to_date('" + m_sORRangeFrom + "','MM/dd/yyyy') ";
                    sQuery += " and to_date('" + m_sORRangeTo + "','MM/dd/yyyy'))";
                }*/
                // RMC 20150429 corrected reports (List of Business is based on Records)(s) edited by JHMN 20170510 added tax year
                //if (sTempTaxYear != "") //JARS 20170807
                if (sTempTaxYear != "" && sTempTaxYear != "%" && sTempTaxYear != "%%")   // RMC 20171128 correction in Business List report
                {
                   // sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1+gr_2 between " + sTempGrossFrom + " and " + sTempGrossTo + " and tax_year like '%" + sTempTaxYear + "'";
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1 between " + sTempGrossFrom + " and " + sTempGrossTo + " and tax_year like '%" + sTempTaxYear + "'"; //JHB 20190731 fetch gr_1 only
                }
                else
                {
                    //  sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1+gr_2 between " + sTempGrossFrom + " and " + sTempGrossTo + ""; /JHB 20190731 fetch gr_1 only
                    sQuery = "select * from businesses where rtrim(bns_brgy) like '" + sTempBrgyName + "' and bns_stat <> 'NEW' and gr_1 between " + sTempGrossFrom + " and " + sTempGrossTo + "";
                }
                // RMC 20150429 corrected reports (List of Business is based on Records)(e) edited by JHMN 20170510 added tax year

                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        dBnsGross = 0;
                        dBnsCap = 0;

                        sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                        sQuery = string.Format("select * from businesses where bin = '{0}'", sBin);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            sBnsCode = StringUtilities.HandleApostrophe(pSet.GetString("bns_code"));
                            sBnsName = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                            sBnsOrgn = StringUtilities.HandleApostrophe(pSet.GetString("orgn_kind"));
                            sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                           // dBnsGross = pSet.GetDouble("gr_1") + pSet.GetDouble("gr_2");
                            dBnsGross = pSet.GetDouble("gr_1") ; //JHB 20190731 fetch gr_1 only
                            dBnsCap = pSet.GetDouble("capital");
                            sBnsPermit = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                            sBussTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));
                            sOwnNm = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(pSet.GetString("own_code")));
                            // GDE 20090514 added (s){
                            sContactNo = StringUtilities.HandleApostrophe(pSet.GetString("bns_telno"));
                            sNoEmp = StringUtilities.HandleApostrophe(pSet.GetInt("num_employees").ToString());
                        }
                        pSet1.Close();

                        sBnsAddress = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBin));

                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);
                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);   // RMC 20170227 correction in Business List
                        sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", sBin);   //JARS 20170807
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            if (pSet1.Read())
                            {
                                sPMode = StringUtilities.HandleApostrophe(pSet1.GetString("data_mode"));	// JJP 09192005 ORACLE ADJUSTMENT
                                sPOrNo = StringUtilities.HandleApostrophe(pSet1.GetString("or_no"));
                                if (sPOrNo == String.Empty)
                                    sPOrDate = "";
                                else
                                    sPOrDate = StringUtilities.HandleApostrophe(pSet1.GetDateTime("or_date").ToShortDateString());
                                sPTerm = StringUtilities.HandleApostrophe(pSet1.GetString("payment_term"));
                                sPTaxYear = StringUtilities.HandleApostrophe(pSet1.GetString("tax_year"));
                                sPQtrPaid = StringUtilities.HandleApostrophe(pSet1.GetString("qtr_paid"));

                                //if (sPMode != "UNP")
                                //{
                                fOrFeesDue = 0;
                                fOrFeesSurch = 0;
                                fOrFeesInt = 0;
                                fTmpFeesDue = 0;
                                fTmpFeesSurch = 0;
                                fTmpFeesInt = 0;
                                fBTmpFeesDue = 0;
                                fBTmpFeesSurch = 0;
                                fBTmpFeesInt = 0;
                                fAllTmpSurchInt = 0;

                                sQuery = string.Format("select * from or_table where or_no = '{0}' and tax_year = '{1}' order by or_no", sPOrNo, sBussTaxYear);
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    while (pSet2.Read())
                                    {
                                        sOrFeesCode = StringUtilities.HandleApostrophe(pSet2.GetString("fees_code"));
                                        fOrFeesDue = pSet2.GetDouble("fees_due");
                                        fOrFeesSurch = pSet2.GetDouble("fees_surch");
                                        fOrFeesInt = pSet2.GetDouble("fees_pen");

                                        if (sOrFeesCode != "B")
                                        {
                                            fTmpFeesDue = fTmpFeesDue + fOrFeesDue;
                                            fTmpFeesSurch = fTmpFeesSurch + fOrFeesSurch;
                                            fTmpFeesInt = fTmpFeesInt + fOrFeesInt;
                                        }
                                        else
                                        {
                                            fBTmpFeesDue = fBTmpFeesDue + fOrFeesDue;
                                            fBTmpFeesSurch = fBTmpFeesSurch + fOrFeesSurch;
                                            fBTmpFeesInt = fBTmpFeesInt + fOrFeesInt;
                                        }
                                    }
                                    fAllTmpSurchInt = fTmpFeesSurch + fTmpFeesInt + fBTmpFeesSurch + fBTmpFeesInt;
                                }
                                pSet2.Close();

                                //(s)-----------> Saving to Report Table
                                sQuery = string.Format(@"insert into rep_list_bns values('{0}','{1}','{2}','{23}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                               StringUtilities.HandleApostrophe(sBnsOrgn),
                                               StringUtilities.HandleApostrophe(sBnsCode),
                                               StringUtilities.HandleApostrophe(sBin),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsName, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAddress, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               dBnsGross.ToString("0.##"),
                                               dBnsCap.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsPermit, 20)),
                                               fBTmpFeesDue.ToString("0.##"),
                                               fTmpFeesDue.ToString("0.##"),
                                               fAllTmpSurchInt.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(sPOrNo),
                                               StringUtilities.HandleApostrophe(sPOrDate),
                                               StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(sPTaxYear),
                                               StringUtilities.HandleApostrophe(sPQtrPaid),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 15)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 30)),
                                               StringUtilities.HandleApostrophe(sContactNo),
                                               StringUtilities.HandleApostrophe(sNoEmp),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 100)));
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();
                            }
                        }
                        pSet1.Close();
                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                    }
                }
                pSet.Close();

                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);

                frmBussReport dlg = new frmBussReport();
                dlg.ReportSwitch = "ListBnsGrossReceipt";//39;
                dlg.ReportName = m_sReportName;
                dlg.ShowDialog();

                //(s)----------> Delete Data from Temp Report Table
                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
                //(e)----------> Delete Data from Temp Report Table
            }
            catch (Exception ex) //JARS 2017080
            {
                pSet.Rollback();
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);
                return;
            }
        }

        private void OnReportListBnsPermitNumber(string m_sDateFrom, string m_sDateTo)
        {
            DateTime m_dtSaveTime;
            string sTempBrgyName = String.Empty, sTempTaxYear = String.Empty, sTempStatus = String.Empty;
            string sQuery, sBin, sBrgyName, sBnsName = String.Empty, sBnsOrgn = String.Empty, sBnsCode = String.Empty, sBnsDesc, sBnsStat = String.Empty, sBnsPermit = String.Empty, sBnsAddress, sPOrDate, sPTerm, sCurrentDate, sCurrentUser;
            string sOwnCode = String.Empty, sOwnNm = String.Empty, sPMode, sPOrNo, sPTaxYear, sPQtrPaid, sOrFeesCode, sBussTaxYear = String.Empty;
            double fOrFeesDue, fOrFeesSurch, fOrFeesInt;
            double fTmpFeesDue, fTmpFeesSurch, fTmpFeesInt;
            double fBTmpFeesDue, fBTmpFeesSurch, fBTmpFeesInt, fAllTmpSurchInt;
            double dBnsGross, dBnsCap;
            int iProgressCtr;

            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            try
            {
                if (txtTaxYear.Text == "")
                    sTempTaxYear = "%%";
                else
                {
                    //JHMN 20170510 added validation for tax year format (s)
                    if (txtTaxYear.Text.Length != 4)
                    {
                        MessageBox.Show("Tax year format should be four digit");
                        return;
                    }
                    else
                        sTempTaxYear = txtTaxYear.Text + "%";
                    //JHMN 20170510 added validation for tax year format (e)
                }


                if (cmbBrgy.Text == "ALL")
                    sTempBrgyName = "%%";
                else
                    sTempBrgyName = cmbBrgy.Text + "%";

                //User
                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

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

                #region Progress

                int intCount = 0;
                int intCountIncreament = 0;
                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();

                Thread.Sleep(500);

                sQuery = string.Format("select Count (*) as cnt from businesses where tax_year like '{0}'", sTempTaxYear);
                sQuery += " and bns_brgy like '" + sTempBrgyName + "'"; // ALJ 01222008
                if (m_sDateFrom != String.Empty)	// ALJ 01222008 
                    sQuery += " and permit_dt between TO_DATE('" + m_sDateFrom + "','MM/dd/yyyy') and TO_DATE('" + m_sDateTo + "','MM/dd/yyyy')"; // ALJ 01222008
                pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);

                #endregion
                //if (sTempTaxYear != "") //JARS 20170807
                if (sTempTaxYear != "" && sTempTaxYear != "%" && sTempTaxYear != "%%")   // RMC 20171128 correction in Business List report
                {
                    sQuery = string.Format("select * from businesses where tax_year like '{0}'", sTempTaxYear);
                    sQuery += " and bns_brgy like '" + sTempBrgyName + "'"; // ALJ 01222008
                    if (m_sDateFrom != String.Empty)	// ALJ 01222008 
                        sQuery += " and permit_dt between TO_DATE('" + m_sDateFrom + "','MM/dd/yyyy') and TO_DATE('" + m_sDateTo + "','MM/dd/yyyy')"; // ALJ 01222008
                }
                else
                {
                    sQuery = string.Format("select * from businesses ");
                    sQuery += " where bns_brgy like '" + sTempBrgyName + "'"; // ALJ 01222008
                    if (m_sDateFrom != String.Empty)	// ALJ 01222008 
                        sQuery += " and permit_dt between TO_DATE('" + m_sDateFrom + "','MM/dd/yyyy') and TO_DATE('" + m_sDateTo + "','MM/dd/yyyy')"; // ALJ 01222008
                }
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;
                        dBnsGross = 0;
                        dBnsCap = 0;

                        sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                        sBnsName = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                        sBnsAddress = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBin));
                        sBnsPermit = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                        sOwnCode = StringUtilities.HandleApostrophe(pSet.GetString("own_code"));
                        sOwnNm = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(sOwnCode)); // OwnersName(sOwnCode);
                        sBnsCode = StringUtilities.HandleApostrophe(pSet.GetString("bns_code"));
                        sBussTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));
                        sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);

                        // ALJ 01222008 (s)
                        sBrgyName = StringUtilities.HandleApostrophe(pSet.GetString("bns_brgy"));
                        sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                        sBussTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year")); //CTS 01242003 adding tax year for filtering
                        dBnsGross = pSet.GetDouble("gr_1") + pSet.GetDouble("gr_2");
                        dBnsCap = pSet.GetDouble("capital");

                        //sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by tax_year desc,qtr_paid desc", sBin, sBussTaxYear);
                        sQuery = string.Format("select distinct * from pay_hist where bin = '{0}' order by tax_year desc,qtr_paid desc", sBin); //JARS 20170807
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            if (pSet1.Read())
                            {
                                sPMode = StringUtilities.HandleApostrophe(pSet1.GetString("data_mode"));	// JJP 09192005 ORACLE ADJUSTMENT
                                sPOrNo = StringUtilities.HandleApostrophe(pSet1.GetString("or_no"));
                                if (sPOrNo == String.Empty)
                                    sPOrDate = "";
                                else
                                    sPOrDate = StringUtilities.HandleApostrophe(pSet1.GetDateTime("or_date").ToShortDateString());
                                sPTerm = StringUtilities.HandleApostrophe(pSet1.GetString("payment_term"));
                                sPTaxYear = StringUtilities.HandleApostrophe(pSet1.GetString("tax_year"));
                                sPQtrPaid = StringUtilities.HandleApostrophe(pSet1.GetString("qtr_paid"));

                                //if (sPMode != "UNP")
                                //{
                                fOrFeesDue = 0;
                                fOrFeesSurch = 0;
                                fOrFeesInt = 0;
                                fTmpFeesDue = 0;
                                fTmpFeesSurch = 0;
                                fTmpFeesInt = 0;
                                fBTmpFeesDue = 0;
                                fBTmpFeesSurch = 0;
                                fBTmpFeesInt = 0;
                                fAllTmpSurchInt = 0;

                                sQuery = string.Format("select * from or_table where or_no = '{0}' and tax_year = '{1}' order by or_no", sPOrNo, sBussTaxYear);
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    while (pSet2.Read())
                                    {
                                        sOrFeesCode = StringUtilities.HandleApostrophe(pSet2.GetString("fees_code"));
                                        fOrFeesDue = pSet2.GetDouble("fees_due");
                                        fOrFeesSurch = pSet2.GetDouble("fees_surch");
                                        fOrFeesInt = pSet2.GetDouble("fees_pen");

                                        if (sOrFeesCode != "B")
                                        {
                                            fTmpFeesDue = fTmpFeesDue + fOrFeesDue;
                                            fTmpFeesSurch = fTmpFeesSurch + fOrFeesSurch;
                                            fTmpFeesInt = fTmpFeesInt + fOrFeesInt;
                                        }
                                        else
                                        {
                                            fBTmpFeesDue = fBTmpFeesDue + fOrFeesDue;
                                            fBTmpFeesSurch = fBTmpFeesSurch + fOrFeesSurch;
                                            fBTmpFeesInt = fBTmpFeesInt + fOrFeesInt;
                                        }
                                    }
                                    fAllTmpSurchInt = fTmpFeesSurch + fTmpFeesInt + fBTmpFeesSurch + fBTmpFeesInt;
                                }
                                pSet2.Close();

                                //(s)-----------> Saving to Report Table
                                sQuery = string.Format(@"insert into rep_list_bns values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','','')",
                                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBrgyName, 30)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOwnNm, 30)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsDesc, 30)),
                                               StringUtilities.HandleApostrophe(sBin),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsName, 60)),
                                               StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsAddress, 100)),
                                               StringUtilities.HandleApostrophe(sBnsStat),
                                               dBnsGross.ToString("0.##"),
                                               dBnsCap.ToString("0.##"),
                                                StringUtilities.HandleApostrophe(StringUtilities.Left(sBnsPermit, 20)),
                                               fBTmpFeesDue.ToString("0.##"),
                                               fTmpFeesDue.ToString("0.##"),
                                               fAllTmpSurchInt.ToString("0.##"),
                                               StringUtilities.HandleApostrophe(sPOrNo),
                                               StringUtilities.HandleApostrophe(sPOrDate),
                                               StringUtilities.HandleApostrophe(sPTerm),
                                               StringUtilities.HandleApostrophe(sPTaxYear),
                                               StringUtilities.HandleApostrophe(sPQtrPaid),
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
                }
                else
                {
                    MessageBox.Show("No Record Found!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pSet.Close();
                    return;
                }
                pSet.Close();

                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);

                frmBussReport dlg = new frmBussReport();
                dlg.ReportSwitch = "ListBnsPrmtNum";//18;
                dlg.ReportName = m_sReportName;
                if (m_sDateFrom != String.Empty)
                    dlg.Data1 = "Permit Date: " + m_sDateFrom + " - " + m_sDateTo;
                else
                    dlg.Data1 = "";
                dlg.BrgyName = cmbBrgy.Text;
                dlg.ShowDialog();

                //pApp->JobStatus(pApp->iProgressTotal+1);
                //(s)----------> Delete Data from Temp Report Table
                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
                //(e)----------> Delete Data from Temp Report Table
            }
            catch (Exception ex) //JARS 2017080
            {
                pSet.Rollback();
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);
                return;
            }
        }

        private void cmbBrgy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbBussType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bnsCode = arr_BnsCode[cmbBussType.SelectedIndex].ToString();
        }

        private void cmbNatureBuss_DropDown(object sender, EventArgs e)
        {
            LoadNatureBusiness();
        }

        private void rdoTopCap_Click(object sender, EventArgs e)
        {
            m_iRadioBrgy = 8;
            EnableControls(false);
            cmbBrgy.Enabled = true;
            txtTaxYear.Enabled = true;
            txtTopCap.Enabled = true;
            cmbBussType.Enabled = true;
            cmbNatureBuss.Enabled = true;
        }

        private void chkByPercentage_CheckedChanged(object sender, EventArgs e)
        {
            if (chkByPercentage.Checked == true)
                lblPercent.Visible = true;
            else
                lblPercent.Visible = false;
        }
    }
}

