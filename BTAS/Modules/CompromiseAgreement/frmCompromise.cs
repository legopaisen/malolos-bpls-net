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
using Amellar.Common.SearchBusiness;
using Amellar.Common.LogIn;

namespace Amellar.Modules.CompromiseAgreement
{
    public partial class frmCompromise : Form
    {
        public frmCompromise()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
            dtpApp.Value = AppSettingsManager.GetSystemDate();
            dtpStart.Value = AppSettingsManager.GetSystemDate();
        }

        string m_sBIN = String.Empty;

        String m_sYear; // ALJ 07312003 for rev year
        String m_sIsQtrly; // ALJX 001/002
        String m_sMsgTitle;
        int m_iMultiTaxYear; // ALJ PDELQ0523
        String m_sPreviousOR;
        String m_sPreviewType;
        String m_sCCChkAmt, m_sCCCashAmt;
        String m_sDebitCredit;
        String m_sCreditLeft;
        String m_sTimePosted;
        String m_sMode;
        String m_sPaymentType;
        String m_sNextQtrToPay;
        String m_sPaymentTerm;
        String m_sDtOperated;
        String m_sCurrentTaxYear;
        String m_sORDate;
        double m_dPenRate;
        double m_dSurchRate;
        String m_sMonthlyCutoffDate;
        String m_sInitialDueDate;
        String m_sNewWithQtr;
        String sBnsCode;
        bool m_bRetVoidSurchPen;
        String m_sStatus;
        String m_sTaxYear;
        bool m_bPartial;

        double m_dSurch1, m_dSurch2, m_dSurch3, m_dSurch4;
        double m_dPenRate1, m_dPenRate2, m_dPenRate3, m_dPenRate4;
        double m_dSurchQuart, m_dPenQuart;

        private void frmCompromise_Load(object sender, EventArgs e)
        {

        }

        private void dtpApp_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (sender.Equals(dtpApp))
            {
                if (dtpApp.Value.Date > cdtToday.Date)
                {
                    MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtpApp.Value = AppSettingsManager.GetSystemDate();
                }
            }
            else if (sender.Equals(dtpStart))
            {
                if (dtpStart.Value.Date < cdtToday.Date)
                {
                    MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtpStart.Value = AppSettingsManager.GetSystemDate();
                }
            }
        }

        private void SelectedObject(bool blnEnable)
        {
            txtRefNum.ReadOnly = !blnEnable;
            txtNoPayment.ReadOnly = !blnEnable;
            dtpApp.Enabled = blnEnable;
            dtpStart.Enabled = blnEnable;
        }

        private bool CheckCompromise()
        {
            bool bAns = false;
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from compromise_tbl where bin = '" + m_sBIN + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    bAns = true;
            pSet.Close();
            return bAns;
        }

        private bool OpenRecords()
        {
            bool bBool = false;
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from business_que where bin = '" + m_sBIN + "'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    pSet.Close();
                    pSet.Query = "select * from businesses where bin = '" + m_sBIN + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                            bBool = true;
                        else
                            pSet.Close();
                }
                else
                    bBool = true;
            return bBool;
        }

        private bool CheckTaxDue()
        {
            bool bAns = false;
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from taxdues where bin = '" + m_sBIN + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    bAns = true;
            pSet.Close();
            return bAns;
        }

        private void GetRecords()
        {
            String sText;

            if (CheckCompromise())
            {
                MessageBox.Show("Business has existing Compromised Agreement!", "Compromise Agreement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                bin1.txtTaxYear.Text = "";
                bin1.txtBINSeries.Text = "";
                bin1.txtTaxYear.Focus();
            }
            else
            {
                if (!CheckTaxDue())
                {
                    MessageBox.Show("No taxdues for the said BIN!", "Compromise Agreement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    bin1.txtTaxYear.Text = "";
                    bin1.txtBINSeries.Text = "";
                    bin1.txtTaxYear.Focus();
                }
                else
                {
                    OracleResultSet pSet = new OracleResultSet();
                    pSet.Query = "select * from business_que where bin = '" + m_sBIN + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sBnsCode = pSet.GetString("bns_code");
                            txtBnsName.Text = pSet.GetString("bns_nm");
                            txtBnsAdd.Text = AppSettingsManager.GetBnsAdd(m_sBIN, "");
                            txtOwnName.Text = AppSettingsManager.GetBnsOwner(pSet.GetString("own_code"));
                            txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(pSet.GetString("own_code"));
                            txtCurrentYear.Text = pSet.GetString("tax_year");
                        }
                        else
                        {
                            pSet.Close();
                            pSet.Query = "select * from businesses where bin = '" + m_sBIN + "'";
                            if (pSet.Execute())
                                if (pSet.Read())
                                {
                                    sBnsCode = pSet.GetString("bns_code");
                                    txtBnsName.Text = pSet.GetString("bns_nm");
                                    txtBnsAdd.Text = AppSettingsManager.GetBnsAdd(m_sBIN, "");
                                    txtOwnName.Text = AppSettingsManager.GetBnsOwner(pSet.GetString("own_code"));
                                    txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(pSet.GetString("own_code"));
                                    txtCurrentYear.Text = pSet.GetString("tax_year");
                                }
                        }
                    pSet.Close();

                    ComputeCompromise();

                    LoadInfo(m_sBIN);
                }
            }
        }

        private void LoadInfo(string pBin)
        {
            String sQuery;
            double dAmount = 0, dTotalAmt = 0;

            OracleResultSet pSet = new OracleResultSet();
            sQuery = "select tax_year, sum(fees_due) as fdue, sum(fees_surch) as fsurch, sum(fees_pen) as fpen,";
            sQuery += " sum(fees_totaldue) as ftdue from pay_temp1";
            sQuery += " where bin = '" + pBin + "' and qtr_to_pay = 'F'";
            sQuery += " group by tax_year order by tax_year";
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dgView.Rows.Add(pSet.GetString("tax_year"), pSet.GetDouble("fdue").ToString("#,##0.00"), pSet.GetDouble("fsurch").ToString("#,##0.00"), pSet.GetDouble("fpen").ToString("#,##0.00"), pSet.GetDouble("ftdue").ToString("#,##0.00"));

                    dAmount = pSet.GetDouble("ftdue");
                    dTotalAmt += dAmount;
                }
            }
            // JJP 04102006 (s) include retirement in compromise terms
            else
            {
                pSet.Close();

                sQuery = "select tax_year, sum(fees_due) as fdue, sum(fees_surch) as fsurch, sum(fees_pen) as fpen,";
                sQuery += " sum(fees_totaldue) as ftdue from pay_temp1";
                sQuery += " where bin = '" + m_sBIN + "' and due_state = 'X'";
                sQuery += " group by tax_year order by tax_year";
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        dgView.Rows.Add(pSet.GetString("tax_year"), pSet.GetDouble("fdue").ToString("#,##0.00"), pSet.GetDouble("fsurch").ToString("#,##0.00"), pSet.GetDouble("fpen").ToString("#,##0.00"), pSet.GetDouble("ftdue").ToString("#,##0.00"));
                        dAmount = pSet.GetDouble("ftdue");
                        dTotalAmt += dAmount;
                    }
                }
            }
            // JJP 04102006 (e) include retirement in compromise terms
            pSet.Close();
            txtGrandTotal.Text = dTotalAmt.ToString("#,##0.00");
        }

        private void ComputeCompromise()
        {
            string sString, sCurrentYear;
            sCurrentYear = AppSettingsManager.GetSystemDate().Year.ToString();
            m_sYear = AppSettingsManager.GetConfigValue("07");
            bool bPendingApp = false;
            OracleResultSet pSet = new OracleResultSet();

            string sTaxYear;

            pSet.Query = string.Format("select is_qtrly from new_table where rev_year = '{0}'", m_sYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    m_sIsQtrly = pSet.GetString("is_qtrly").ToString();
                }
            pSet.Close();

            m_sInitialDueDate = AppSettingsManager.GetConfigValue("13");
            m_sMonthlyCutoffDate = AppSettingsManager.GetConfigValue("14");

            pSet.Query = string.Format("select * from surch_sched where rev_year = '{0}'", m_sYear); // ALJ 07312003 for rev year
            if (pSet.Execute())
                if (pSet.Read())
                {
                    m_dSurchRate = pSet.GetFloat("surch_rate");
                    m_dPenRate = pSet.GetFloat("pen_rate");
                }
            pSet.Close();

            pSet.Query = string.Format("delete from pay_temp1 where bin = '{0}'", m_sBIN);
            pSet.ExecuteNonQuery();
            pSet.Close();

            m_bRetVoidSurchPen = false; // ALJ 06182003 for RET
            pSet.Query = string.Format("select * from business_que where bin = '{0}'", m_sBIN); // CTS 09152003
            if (pSet.Execute())
                if (pSet.Read())
                {
                    bPendingApp = true;
                    m_sStatus = pSet.GetString("bns_stat").Trim();
                    m_sTaxYear = pSet.GetString("tax_year").Trim();
                    if (m_sStatus == "RET")
                    {
                        pSet.Close();
                        pSet.Query = string.Format("select distinct bin from pay_hist where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                m_bRetVoidSurchPen = true;
                            }
                    }
                    // ALJ 06182003 (e) for RET
                }
                else
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from businesses where bin = '{0}'", m_sBIN); // CTS 09152003
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            m_sStatus = pSet.GetString("bns_stat").Trim();
                            m_sTaxYear = pSet.GetString("tax_year").Trim();
                            sTaxYear = m_sTaxYear;
                        }
                }
            pSet.Close();

            // CTS 10102003 (e)
            sString = "select * from taxdues where bin = '" + m_sBIN + "' order by tax_year desc";
            pSet.Query = sString;
            if (pSet.Execute())
                if (pSet.Read())
                    m_sTaxYear = pSet.GetString("tax_year");
            pSet.Close();

            ComputeTaxFees(m_sBIN, m_sTaxYear);

            if (m_sTaxYear == sCurrentYear)					// JJP 04102006 include adjustment in compromise terms (Replace)
                LessCurrentDue(sCurrentYear);
        }

        private void LessCurrentDue(string sYear)
        {
            string sQuery = string.Empty;
            sQuery = "delete from pay_temp1 where bin = '" + m_sBIN + "'";
            sQuery += " and tax_year = '" + sYear + "'";
            sQuery += " and due_state <> 'X'";		// JJP 04102006 include retirement in compromise terms (Added)
            sQuery += " and due_state <> 'A' ";		// JJP 04102006 include adjustment in compromise terms (Added)
            sQuery += " and bin not in (select bin from retired_bns_temp)";		// JJP 04102006 include retirement in compromise terms (Added)

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
            pSet.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "S&earch")
            {
                btnSearch.Text = "C&lear";
                string sBin = string.Empty;
                SelectedObject(true);
                if (bin1.txtTaxYear.Text.Trim() == string.Empty && bin1.txtBINSeries.Text.Trim() == string.Empty)
                {
                    frmSearchBusiness fSearch = new frmSearchBusiness();
                    fSearch.ShowDialog();
                    sBin = fSearch.sBIN;
                }
                else
                    sBin = bin1.GetBin();

                m_sBIN = sBin;
                GetRecords();
            }
            else
            {
                btnSearch.Text = "S&earch";

                //txtBINYr.Text = string.Empty;
                //txtBINSerial.Text = string.Empty;
                bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
                bin1.GetDistCode = ConfigurationAttributes.DistCode;
                bin1.txtBINSeries.Text = "";
                bin1.txtTaxYear.Text = "";

                foreach (object a in this.Controls)
                    if (a is TextBox)
                        ((TextBox)a).Text = string.Empty;
                
                dgView.Rows.Clear();
                SelectedObject(false);
                bin1.txtTaxYear.Focus();
                dtpApp.Value = AppSettingsManager.GetSystemDate();
                dtpStart.Value = AppSettingsManager.GetSystemDate();
            }
        }

        struct TaxDuesStruct
        {
            public String bin;
            public String tax_year;
            public String qtr_to_pay;
            public String bns_code_main;
            public String tax_code;
            public double amount;
            public String due_state;
        }

        struct PayTempStruct
        {
            public String bin;
            public String tax_year;
            public String term;
            public String qtr;
            public String fees_desc;
            public double fees_due;
            public double fees_surch;
            public double fees_pen;
            public double fees_totaldue;
            public String fees_code;
            public String due_state;
            public String qtr_to_pay;
            public String fees_code_sort; // ALJ 01092004 sorting of tax and fees
        }

        private bool IsTaxFull(String p_sBnsCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            sQuery = "select * from btax_full_table"; // create this table for future enhancement
            sQuery += " where bns_code like '" + p_sBnsCode.Substring(0, 2) + "%%'";
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    return true;
                else
                    return false;
            pSet.Close();
            return false;
        }

        private string FeesTerm(String sFeesCode)
        {
            String sQuery;
            String sFeesTerm = String.Empty;
            OracleResultSet pSet = new OracleResultSet();
            if (sFeesCode.Substring(0, 1) == "B")
            {
                sFeesTerm = "Q";
            }
            else
            {
                if (m_bPartial == false)
                {
                    //sQuery = string.Format("select * from tax_and_fees_table where fees_code = '{0}'", sFeesCode); // CTS 09152003
                    sQuery = string.Format("select * from tax_and_fees_table where fees_code = '{0}' and rev_year = '{1}'", sFeesCode, AppSettingsManager.GetConfigValue("07")); // MCR 20141118
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sFeesTerm = pSet.GetString("fees_term");
                        }
                    pSet.Close();
                }
                else
                {
                    sQuery = string.Format("select * from partial_fees where fees_code = '{0}'", sFeesCode); // CTS 09152003
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sFeesTerm = pSet.GetString("fees_term");
                        }
                    pSet.Close();
                }
            }
            return sFeesTerm;
        }

        private bool FeeQtrlyCheck(String p_sBIN, String p_sTaxYear, String p_sFeeCode, String p_sBnsCode)
        {
            String sQuery;
            //if (p_sFeeCode == "03" || (p_sFeeCode.Left(1) == 'B' && p_sBnsCode.Left(2) == "22")) // GARBAGE/Delivery Van temporary hard coded -- revise code in the future
            /*	// RTL 03082007 put rem
            if (p_sFeeCode == "03" || (p_sFeeCode.Left(1) == 'B' && p_sBnsCode.Left(3) == "B22")) // GARBAGE/Delivery Van temporary hard coded -- revise code in the future		// JJP 03142006 Correct 2nd quarter payment - do not include delivery vehicle
            {
                if (p_sFeeCode.Left(1) == 'B')
                    p_sFeeCode = 'B';

                sQuery = "select a.or_no, a.qtr_paid, a.payment_term from pay_hist a, or_table b";
                sQuery+= " where bin = '"+p_sBIN+"'";
                sQuery+= " and a.or_no = b.or_no";
                sQuery+= " and a.tax_year = b.tax_year";
                sQuery+= " and a.tax_year = '"+p_sTaxYear+"'";
                sQuery+= " and payment_term = 'I'";
                sQuery+= " and fees_code = '"+p_sFeeCode+"'";
                sQuery+= " and bns_code_main = '"+p_sBnsCode+"'";

                if(pApp->RecordFound(sQuery))
                    return TRUE;
                else
                    return FALSE;
            }
            else
            */
            return false;
        }

        private bool WithPayment(String sBIN, String sTaxYear)
        {
            OracleResultSet pSet = new OracleResultSet();
            //string.Format("select * from pay_hist where bin = '%s' and tax_year = '%s' and mode <> 'UNP'",sBIN,sTaxYear); // CTS 09152003
            pSet.Query = string.Format("select * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode <> 'UNP'", sBIN, sTaxYear); // JGR 09192005 Oracle Adjustment
            if (pSet.Execute())
                if (pSet.Read())
                    return true;
                else
                    return false;
            pSet.Close();
            return false;
        }

        private string FeesWithSurch(String sFeesCode)
        {
            String sQuery;
            String sFeesWithSurch = String.Empty;
            OracleResultSet pSet = new OracleResultSet();
            if (sFeesCode.Substring(0, 1) == "B")
            {
                sFeesWithSurch = "Y";
            }
            else
            {
                //sQuery = string.Format("select * from tax_and_fees_table where fees_code = '{0}'", sFeesCode); // CTS 09152003
                sQuery = string.Format("select * from tax_and_fees_table where fees_code = '{0}' and rev_year = '{1}'", sFeesCode, AppSettingsManager.GetConfigValue("07")); // MCR 20141118
                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sFeesWithSurch = pSet.GetString("fees_withsurch").Trim();
                    }
                pSet.Close();
            }
            return sFeesWithSurch;
        }

        private string FeesWithPen(String sFeesCode)
        {
            String sQuery;
            String sFeesWithPen = String.Empty;
            OracleResultSet pSet = new OracleResultSet();

            if (sFeesCode.Substring(0, 1) == "B")
            {
                sFeesWithPen = "Y";
            }
            else
            {
                //sQuery = string.Format("select * from tax_and_fees_table where fees_code = '{0}'", sFeesCode); // CTS 09152003
                sQuery = string.Format("select * from tax_and_fees_table where fees_code = '{0}' and rev_year = '{1}'", sFeesCode, AppSettingsManager.GetConfigValue("07")); // MCR 20141118
                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sFeesWithPen = pSet.GetString("fees_withpen").Trim();
                    }
                pSet.Close();
            }
            return sFeesWithPen;
        }

        private void ComputeTaxFees(String p_sBIN, String p_sTaxYear)
        {
            String sQuery;
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            TaxDuesStruct t;
            PayTempStruct p;
            String sBnsCodeMain, sQtrPay;

            t.tax_year = "";
            t.qtr_to_pay = "";
            t.amount = 0;
            t.due_state = "";

            p.fees_due = 0;
            p.fees_surch = 0;
            p.fees_pen = 0;
            p.fees_totaldue = 0;


            m_bPartial = false; // JJP 10142005 COMPROMISE - QA
            int m_iMultiTaxYear = 0;

            pSet.Query = string.Format("select count(distinct tax_year) from taxdues where bin = '{0}'", p_sBIN);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    m_iMultiTaxYear = pSet.GetInt(0);
                }
            pSet.Close();

            m_sORDate = AppSettingsManager.GetSystemDate().ToShortDateString();
            m_sTimePosted = AppSettingsManager.GetSystemDate().ToShortTimeString();
            m_sCurrentTaxYear = AppSettingsManager.GetSystemDate().Year.ToString();

            pSet.Query = string.Format("select * from taxdues where bin = '{0}' and tax_year <= '{1}' order by tax_year,qtr_to_pay", p_sBIN, p_sTaxYear); // CTS 09152003
            if (pSet.Execute()) // ALJ PDELQ0523 m_sCurrentTaxYear
            {
                if (Convert.ToInt32(p_sTaxYear) <= Convert.ToInt32(m_sCurrentTaxYear))
                    while (pSet.Read())
                    {
                        t.bin = pSet.GetString("bin");
                        t.tax_year = pSet.GetString("tax_year");
                        t.qtr_to_pay = pSet.GetString("qtr_to_pay");
                        sQtrPay = t.qtr_to_pay; // CTS 10052003 
                        sBnsCodeMain = pSet.GetString("bns_code_main");
                        t.tax_code = pSet.GetString("tax_code");
                        t.amount = pSet.GetDouble("amount");
                        t.due_state = pSet.GetString("due_state");

                        if (t.qtr_to_pay != "1" && m_iMultiTaxYear == 1) // ALJ IDELQ0522
                        {
                            m_sNextQtrToPay = t.qtr_to_pay;
                            //OnInstallment();
                        }
                        if (m_iMultiTaxYear != 1 && Convert.ToInt32(p_sTaxYear) < Convert.ToInt32(m_sCurrentTaxYear))
                        {
                            //OnFull();				
                        }

                        if (t.tax_code.Substring(0, 1) == "B")
                        {
                            t.tax_code += sBnsCodeMain.Trim();
                        }

                        if (t.tax_code.Substring(0, 1) == "B")
                        {
                            p.fees_desc = AppSettingsManager.GetBnsDesc(sBnsCodeMain);
                            p.fees_desc = "TAX ON " + p.fees_desc;
                            p.fees_code_sort = "00"; // ALJ 01092004 sorting of tax and fees
                        }
                        else
                        {
                            p.fees_desc = AppSettingsManager.GetFeesDesc(t.tax_code); //To get fees_description
                            p.fees_code_sort = t.tax_code; // ALJ 01092004 sorting of tax and fees	
                        }

                        if (m_sStatus == "NEW")
                        {
                            if (t.due_state == "Q")
                            {
                                p.term = "Q";
                                p.qtr = t.qtr_to_pay;
                            }
                            else
                            {
                                p.term = "F";
                                p.qtr = t.qtr_to_pay;
                            }
                        }
                        else //if "REN"
                        {
                            if (t.qtr_to_pay == "1")
                            {
                                p.term = "F";
                                p.qtr = "F";
                            }
                            else
                            {
                                p.term = "I";
                                p.qtr = t.qtr_to_pay;
                            }

                        }

                        m_dSurch1 = 0; m_dSurch2 = 0; m_dSurch3 = 0; m_dSurch4 = 0;
                        m_dPenRate1 = 0; m_dPenRate2 = 0; m_dPenRate3 = 0; m_dPenRate4 = 0;
                        m_dSurchQuart = 0; m_dPenQuart = 0;

                        ComputePenRate(t.tax_year, t.qtr_to_pay, t.due_state, m_dSurch1, m_dSurch2, m_dSurch3, m_dSurch4, m_dPenRate1, m_dPenRate2, m_dPenRate3, m_dPenRate4, m_dSurchQuart, m_dPenQuart);

                        //Added by EMS 12152002 (s)
                        if (FeesWithPen(t.tax_code) == "N")
                        {
                            m_dPenRate1 = 0;
                            m_dPenRate2 = 0;
                            m_dPenRate3 = 0;
                            m_dPenRate4 = 0;
                        }
                        if (FeesWithSurch(t.tax_code) == "N")
                        {
                            m_dSurch1 = 0;
                            m_dSurch2 = 0;
                            m_dSurch3 = 0;
                            m_dSurch4 = 0;
                        }
                        //Added by EMS 12152002 (e)

                        if (t.due_state == "N")
                        {
                            m_sNewWithQtr = AppSettingsManager.GetConfigValue("15").Trim();
                            if (m_sNewWithQtr == "N")
                            {
                                //GetDlgItem(IDC_INSTALLMENT)->EnableWindow(0);
                            }
                        }

                        String sAllSurch, sAllPen, sAllTotalDue;
                        sAllSurch = "0";
                        sAllPen = "0";
                        sAllTotalDue = "0";
                        if (t.due_state == "R" || m_sNewWithQtr == "Y")							// JJP 04102006 include retirement in compromise terms (Remarks)
                        {
                            if ((t.tax_code.Substring(0, 1) == "B" && IsTaxFull(sBnsCodeMain) == false) || (FeesTerm(t.tax_code) == "Q" || FeeQtrlyCheck(t.bin, t.tax_year, t.tax_code, sBnsCodeMain) == true)) 	// JJP 04202006 Do not devide 4 if tax is full term (Replace)
                            {
                                p.fees_due = (t.amount) / 4;
                            }
                            else
                            {
                                p.fees_due = t.amount;
                            }
                            //Added by EMS 12092002 (e)

                            p.fees_surch = (p.fees_due) * m_dSurch1;
                            p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate1;
                            p.fees_totaldue = (p.fees_due + p.fees_surch + p.fees_pen);

                            if (((t.tax_year != m_sCurrentTaxYear) && Convert.ToInt16(t.qtr_to_pay) > 1))  //'if' added by EMS 12092002
                            {
                            }
                            else
                            {
                                sAllSurch = sAllSurch + p.fees_surch;
                                sAllPen = sAllPen + p.fees_pen;
                                sAllTotalDue = sAllTotalDue + p.fees_totaldue;
                            }

                            p.bin = t.bin;
                            p.tax_year = t.tax_year;
                            p.fees_code = t.tax_code;
                            p.due_state = t.due_state;
                            p.qtr_to_pay = "1";

                            p.term = "I";
                            p.qtr = "1";

                            if (((t.tax_year != m_sCurrentTaxYear) && Convert.ToDouble(t.qtr_to_pay) > 1))  //'if' added by EMS 12092002
                            {
                            }
                            else
                            {
                                if ((t.tax_year == m_sCurrentTaxYear) || (t.tax_year != m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year))
                                {
                                    sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                    sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                    sQuery += " '" + p.fees_due + "', ";
                                    sQuery += " '" + p.fees_surch + "', ";
                                    sQuery += " '" + p.fees_pen + "', ";
                                    sQuery += " '" + p.fees_totaldue + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                    pSet.Query = sQuery;
                                    pSet.ExecuteNonQuery();
                                }

                                p.qtr_to_pay = "F";
                                if ((t.tax_year != m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year)) //Edited by EMS 02102003
                                {
                                    sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                    sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                    sQuery += " '" + p.fees_due + "', ";
                                    sQuery += " '" + p.fees_surch + "', ";
                                    sQuery += " '" + p.fees_pen + "', ";
                                    sQuery += " '" + p.fees_totaldue + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                    pSet.Query = sQuery;
                                    pSet.ExecuteNonQuery();
                                }

                                if ((t.tax_year == m_sCurrentTaxYear) && WithPayment(m_sBIN, t.tax_year) && Convert.ToInt16(t.qtr_to_pay) < 2)
                                {
                                    sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                    sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                    sQuery += " '" + p.fees_due + "', ";
                                    sQuery += " '" + p.fees_surch + "', ";
                                    sQuery += " '" + p.fees_pen + "', ";
                                    sQuery += " '" + p.fees_totaldue + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                    pSet.Query = sQuery;
                                    pSet.ExecuteNonQuery();
                                }

                            }
                            if ((t.tax_code.Substring(0, 1) == "B" && IsTaxFull(sBnsCodeMain) == false) || (FeesTerm(t.tax_code) == "Q" || FeeQtrlyCheck(t.bin, t.tax_year, t.tax_code, sBnsCodeMain) == true)) 	// JJP 04202006 Do not devide 4 if tax is full term (Replace)
                            {
                                p.fees_due = t.amount / 4;
                                p.fees_surch = p.fees_due * m_dSurch2;
                                p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate2;
                                p.fees_totaldue = (p.fees_due + p.fees_surch + p.fees_pen);
                                if (((t.tax_year != m_sCurrentTaxYear) && Convert.ToInt16(t.qtr_to_pay) > 2))  //'if' added by EMS 12092002
                                {
                                }
                                else
                                {
                                    sAllSurch = sAllSurch + p.fees_surch;
                                    sAllPen = sAllPen + p.fees_pen;
                                    sAllTotalDue = sAllTotalDue + p.fees_totaldue;
                                }

                                p.bin = t.bin;
                                p.tax_year = t.tax_year;
                                p.fees_code = t.tax_code;
                                p.due_state = t.due_state;
                                p.qtr_to_pay = "2";
                                if (m_iMultiTaxYear > 1 && t.tax_year != m_sCurrentTaxYear)
                                    p.qtr_to_pay = "1";

                                p.term = "I";
                                p.qtr = "2";

                                if (((t.tax_year != m_sCurrentTaxYear) && Convert.ToInt16(t.qtr_to_pay) > 2))  //'if' added by EMS 12092002
                                {
                                }
                                else
                                {
                                    if ((t.tax_year == m_sCurrentTaxYear) || (t.tax_year != m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year))
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    } //Added by EMS 02102003

                                    p.qtr_to_pay = "F";
                                    if ((t.tax_year != m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year)) //Edited by EMS 02102003
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    }

                                    if ((t.tax_year == m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year) && Convert.ToInt16(t.qtr_to_pay) < 3)
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    }
                                }
                            }

                            if ((t.tax_code.Substring(0, 1) == "B" && IsTaxFull(sBnsCodeMain) == false) || (FeesTerm(t.tax_code) == "Q" || FeeQtrlyCheck(t.bin, t.tax_year, t.tax_code, sBnsCodeMain) == true)) 	// JJP 04202006 Do not devide 4 if tax is full term (Replace)
                            {
                                p.fees_due = t.amount / 4;
                                p.fees_surch = p.fees_due * m_dSurch3;
                                p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate3;
                                p.fees_totaldue = p.fees_due + p.fees_surch + p.fees_pen;

                                if (((t.tax_year != m_sCurrentTaxYear) && Convert.ToInt16(t.qtr_to_pay) > 3))  //'if' added by EMS 12092002
                                {
                                }
                                else
                                {
                                    sAllSurch = sAllSurch + p.fees_surch;
                                    sAllPen = sAllPen + p.fees_pen;
                                    sAllTotalDue = sAllTotalDue + p.fees_totaldue;
                                }

                                p.bin = t.bin;
                                p.tax_year = t.tax_year;
                                p.fees_code = t.tax_code;
                                p.due_state = t.due_state;
                                p.qtr_to_pay = "3";
                                if (m_iMultiTaxYear > 1 && t.tax_year != m_sCurrentTaxYear)
                                    p.qtr_to_pay = "1";

                                p.term = "I";
                                p.qtr = "3";

                                if (((t.tax_year != m_sCurrentTaxYear) && Convert.ToInt16(t.qtr_to_pay) > 3))  //'if' added by EMS 12092002
                                {
                                }
                                else
                                {
                                    if ((t.tax_year == m_sCurrentTaxYear) || (t.tax_year != m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year))
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    } // Added by EMS 02102003

                                    p.qtr_to_pay = "F";
                                    if ((t.tax_year != m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year)) //Edited by EMS 02102003
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    }

                                    if ((t.tax_year == m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year) && Convert.ToInt16(t.qtr_to_pay) < 4)
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    }
                                }
                            }

                            if ((t.tax_code.Substring(0, 1) == "B" && IsTaxFull(sBnsCodeMain) == false) || (FeesTerm(t.tax_code) == "Q" || FeeQtrlyCheck(t.bin, t.tax_year, t.tax_code, sBnsCodeMain) == true)) 	// JJP 04202006 Do not devide 4 if tax is full term (Replace)
                            {
                                p.fees_due = t.amount / 4;
                                p.fees_surch = p.fees_due * m_dSurch4;
                                p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate4;
                                p.fees_totaldue = p.fees_due + p.fees_surch + p.fees_pen;
                                if (((t.tax_year != m_sCurrentTaxYear) && Convert.ToInt16(t.qtr_to_pay) > 4))  //'if' added by EMS 12092002
                                {
                                }
                                else
                                {
                                    sAllSurch = sAllSurch + p.fees_surch;
                                    sAllPen = sAllPen + p.fees_pen;
                                    sAllTotalDue = sAllTotalDue + p.fees_totaldue;
                                }

                                p.bin = t.bin;
                                p.tax_year = t.tax_year;
                                p.fees_code = t.tax_code;
                                p.due_state = t.due_state;
                                p.qtr_to_pay = "4";

                                if (m_iMultiTaxYear > 1 && t.tax_year != m_sCurrentTaxYear)
                                    p.qtr_to_pay = "1";

                                p.term = "I";
                                p.qtr = "4";

                                if (((t.tax_year != m_sCurrentTaxYear) && Convert.ToInt16(t.qtr_to_pay) > 4))  //'if' added by EMS 12092002
                                {
                                }
                                else
                                {
                                    if ((t.tax_year == m_sCurrentTaxYear) || (t.tax_year != m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year))
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    }  // Added by EMS 02102003

                                    p.qtr_to_pay = "F";
                                    if ((t.tax_year != m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year)) //Edited by EMS 02102003
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    }

                                    if ((t.tax_year == m_sCurrentTaxYear) && WithPayment(p_sBIN, t.tax_year) && Convert.ToInt16(t.qtr_to_pay) <= 4)
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    }
                                }
                            }

                            if ((t.tax_code.Substring(0, 1) == "B" && IsTaxFull(sBnsCodeMain) == false) || (FeesTerm(t.tax_code) == "Q" || FeeQtrlyCheck(t.bin, t.tax_year, t.tax_code, sBnsCodeMain) == true)) 	// JJP 04202006 Do not devide 4 if tax is full term (Replace)
                            {
                                p.fees_due = t.amount;
                                p.fees_surch = Convert.ToDouble(sAllSurch);
                                p.fees_pen = Convert.ToDouble(sAllPen);
                                p.fees_totaldue = t.amount + Convert.ToDouble(sAllSurch) + Convert.ToDouble(sAllPen); // ALJ 09022003 to correct the discrepancy of total per row

                                p.bin = t.bin;
                                p.tax_year = t.tax_year;
                                p.fees_code = t.tax_code;
                                p.due_state = t.due_state;
                                p.qtr_to_pay = "F";
                                p.term = "F";
                                p.qtr = "F";

                                if ((t.tax_year == m_sCurrentTaxYear) && (!WithPayment(p_sBIN, t.tax_year)))		// JJP 04102006 Include retirement in compromise terms - computation if partially paid (Replace)
                                {
                                    sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                    sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                    sQuery += " '" + p.fees_due + "', ";
                                    sQuery += " '" + p.fees_surch + "', ";
                                    sQuery += " '" + p.fees_pen + "', ";
                                    sQuery += " '" + p.fees_totaldue + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                    pSet.Query = sQuery;
                                    pSet.ExecuteNonQuery();
                                }

                                else
                                {
                                    if ((t.tax_year != m_sCurrentTaxYear) && (!WithPayment(p_sBIN, t.tax_year)))
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();

                                        p.qtr_to_pay = "1";
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    }
                                }

                            }
                            else
                            {
                                p.fees_due = t.amount;
                                p.bin = t.bin;
                                p.tax_year = t.tax_year;
                                p.fees_code = t.tax_code;
                                p.due_state = t.due_state;
                                p.qtr_to_pay = "F";
                                p.term = "F";
                                p.qtr = "F";

                                if ((t.tax_year == m_sCurrentTaxYear) && (!WithPayment(p_sBIN, t.tax_year)))		// JJP 04102006 Include retirement in compromise terms - computation if partially paid (Replace)
                                {
                                    sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                    sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                    sQuery += " '" + p.fees_due + "', ";
                                    sQuery += " '" + p.fees_surch + "', ";
                                    sQuery += " '" + p.fees_pen + "', ";
                                    sQuery += " '" + p.fees_totaldue + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                    sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                    pSet.Query = sQuery;
                                    pSet.ExecuteNonQuery();
                                }
                                else
                                {
                                    if ((t.tax_year != m_sCurrentTaxYear) && (!WithPayment(p_sBIN, t.tax_year)))
                                    {
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();

                                        p.qtr_to_pay = "1";
                                        sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                                        sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                                        sQuery += " '" + p.fees_due + "', ";
                                        sQuery += " '" + p.fees_surch + "', ";
                                        sQuery += " '" + p.fees_pen + "', ";
                                        sQuery += " '" + p.fees_totaldue + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                                        pSet.Query = sQuery;
                                        pSet.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        if (t.due_state == "N" && m_sNewWithQtr == "N")
                        {
                            p.fees_due = t.amount;
                            p.fees_surch = p.fees_due * m_dSurch1;
                            p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate1;
                            p.fees_totaldue = p.fees_due + p.fees_surch + p.fees_pen;

                            p.bin = t.bin;
                            p.tax_year = t.tax_year;
                            p.fees_code = t.tax_code;
                            p.due_state = t.due_state;
                            p.qtr_to_pay = "1";
                            p.term = "F";
                            p.qtr = AppSettingsManager.GetQtr(m_sDtOperated);

                            sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                            sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                            sQuery += " '" + p.fees_due + "', ";
                            sQuery += " '" + p.fees_surch + "', ";
                            sQuery += " '" + p.fees_pen + "', ";
                            sQuery += " '" + p.fees_totaldue + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();

                            p.qtr_to_pay = "F";
                            if (m_sIsQtrly == "N")
                            {
                                p.qtr = "F";
                            }

                            sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                            sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                            sQuery += " '" + p.fees_due + "', ";
                            sQuery += " '" + p.fees_surch + "', ";
                            sQuery += " '" + p.fees_pen + "', ";
                            sQuery += " '" + p.fees_totaldue + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();
                        }

                        if (t.due_state == "Q")
                        {
                            p.fees_due = t.amount;
                            p.fees_surch = p.fees_due * m_dSurchQuart;
                            p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenQuart;
                            p.fees_totaldue = p.fees_due + p.fees_surch + p.fees_pen;

                            p.bin = t.bin;
                            p.tax_year = t.tax_year;
                            p.fees_code = t.tax_code;
                            p.due_state = t.due_state;
                            p.qtr_to_pay = "1";
                            p.term = "Q";
                            p.qtr = t.qtr_to_pay;

                            sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                            sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                            sQuery += " '" + p.fees_due + "', ";
                            sQuery += " '" + p.fees_surch + "', ";
                            sQuery += " '" + p.fees_pen + "', ";
                            sQuery += " '" + p.fees_totaldue + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();

                            p.qtr_to_pay = "F";
                            sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values ";
                            sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                            sQuery += " '" + p.fees_due + "', ";
                            sQuery += " '" + p.fees_surch + "', ";
                            sQuery += " '" + p.fees_pen + "', ";
                            sQuery += " '" + p.fees_totaldue + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "') ";
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();
                        }

                        if (t.due_state == "A")
                        {
                            p.fees_due = t.amount;
                            bool bAdjLate = true;
                            if (bAdjLate)
                            {
                                if (t.qtr_to_pay == "1")
                                {
                                    p.fees_surch = p.fees_due * m_dSurch1;	// JJP 08172005 QA-1 PENALTY 
                                    p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate1;
                                }
                                else if (t.qtr_to_pay == "2")
                                {
                                    p.fees_surch = p.fees_due * m_dSurch2;	// JJP 08172005 QA-1 PENALTY 
                                    p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate2;
                                }
                                else if (t.qtr_to_pay == "3")
                                {
                                    p.fees_surch = p.fees_due * m_dSurch3;	// JJP 08172005 QA-1 PENALTY 
                                    p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate3;
                                }
                                else if (t.qtr_to_pay == "4")
                                {
                                    p.fees_surch = p.fees_due * m_dSurch4;	// JJP 08172005 QA-1 PENALTY 
                                    p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate4;
                                }
                            }
                            else
                            {
                                p.fees_surch = 0;
                                p.fees_pen = 0;
                            }

                            p.fees_totaldue = p.fees_due + p.fees_surch + p.fees_pen;

                            if (((t.tax_year != m_sCurrentTaxYear) && Convert.ToInt16(t.qtr_to_pay) > 1))  //'if' added by EMS 12092002
                            {
                            }
                            else
                            {
                                sAllSurch = sAllSurch + p.fees_surch;
                                sAllPen = sAllPen + p.fees_pen;
                                sAllTotalDue = sAllTotalDue + p.fees_totaldue;
                            }

                            p.bin = t.bin;
                            p.tax_year = t.tax_year;
                            p.fees_code = t.tax_code;
                            p.due_state = t.due_state;
                            p.qtr_to_pay = "1";

                            p.term = "I";
                            p.qtr = "1";

                            p.term = "F";
                            p.qtr = "A";
                            p.qtr_to_pay = "F";	// JJP 08042005 QA-1 just added

                            if (p.fees_code == "01") //IF Added by EMS 01082005
                            {
                                p.fees_code = p.fees_code + "-" + sBnsCodeMain;
                                p.fees_desc = "(" + sBnsCodeMain + ")-" + p.fees_desc;
                            }
                            sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values "; // ALJ 01092004 sorting of tax and fees
                            sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                            sQuery += " '" + StringUtilities.Left(StringUtilities.HandleApostrophe("ADJ-" + t.qtr_to_pay + " " + p.fees_desc), 40) + "', "; // ALJ 11202003 fixed SQL temporary solution		// JJP 06292005 REVENUE EXAM ENHANCEMENT
                            sQuery += p.fees_due + ", ";      // ALJ 11202003 fixed SQL
                            sQuery += p.fees_surch + ", ";    // ALJ 11202003 fixed SQL
                            sQuery += p.fees_pen + ", ";      // ALJ 11202003 fixed SQL
                            sQuery += p.fees_totaldue + ", "; // ALJ 11202003 fixed SQL
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "')"; // ALJ 01092004 sorting of tax and fees
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();
                        }
                        if (t.due_state == "X")
                        {
                            String sFeesDueTemp = String.Empty;	// RTL 02172006 fix bugs
                            if (((t.tax_code.Substring(0, 1) == "B" && IsTaxFull(sBnsCodeMain) == false) || (FeesTerm(t.tax_code) == "Q" || FeeQtrlyCheck(t.bin, t.tax_year, t.tax_code, sBnsCodeMain) == true)) && Convert.ToInt32(m_sTaxYear) == AppSettingsManager.GetSystemDate().Year)	// JJP 04202006 Do not devide 4 if tax is full term (Replace)
                            {
                                p.fees_due = t.amount;
                                sFeesDueTemp = p.fees_due.ToString();
                                p.fees_due = t.amount / 4;
                            }
                            else
                            {
                                if (t.qtr_to_pay == "1")
                                {
                                    p.fees_due = t.amount;
                                    sFeesDueTemp = p.fees_due.ToString();
                                    p.fees_due = t.amount / 4;
                                }
                            }

                            if (Convert.ToInt32(m_sTaxYear) == AppSettingsManager.GetSystemDate().Year)
                            {
                                if (t.qtr_to_pay == "1")
                                {
                                    p.fees_surch = p.fees_due * m_dSurch1;
                                    p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate1;
                                }
                                else if (t.qtr_to_pay == "2")
                                {
                                    p.fees_surch = p.fees_due * m_dSurch2;
                                    p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate2;
                                }
                                else if (t.qtr_to_pay == "3")
                                {
                                    p.fees_surch = p.fees_due * m_dSurch3;
                                    p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate3;
                                }
                                else if (t.qtr_to_pay == "4")
                                {
                                    p.fees_surch = p.fees_due * m_dSurch4;
                                    p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate4;
                                }
                                if (sFeesDueTemp != "")
                                    p.fees_due = Convert.ToDouble(sFeesDueTemp);
                            }
                            else
                            {
                                if (t.qtr_to_pay == "1")
                                {
                                    String sDue = "0", sSurch = "0", sPen = "0", sTotal = "0";
                                    for (int ii = 1; ii < 5; ii++)
                                    {
                                        if (ii == 1)
                                        {
                                            p.fees_surch = p.fees_due * m_dSurch1;
                                            p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate1;
                                        }
                                        else if (ii == 2)
                                        {
                                            p.fees_surch = p.fees_due * m_dSurch2;
                                            p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate2;
                                        }
                                        else if (ii == 3)
                                        {
                                            p.fees_surch = p.fees_due * m_dSurch3;
                                            p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate3;
                                        }
                                        else if (ii == 4)
                                        {
                                            p.fees_surch = p.fees_due * m_dSurch4;
                                            p.fees_pen = (p.fees_due + p.fees_surch) * m_dPenRate4;
                                        }

                                        p.fees_totaldue = p.fees_due + p.fees_surch + p.fees_pen;

                                        sDue = sDue + p.fees_due;
                                        sSurch = sSurch + p.fees_surch;
                                        sPen = sPen + p.fees_pen;
                                        sTotal = sTotal + p.fees_totaldue;
                                    }

                                    p.fees_due = Convert.ToDouble(sDue);
                                    p.fees_surch = Convert.ToDouble(sSurch);
                                    p.fees_pen = Convert.ToDouble(sPen);
                                    p.fees_totaldue = Convert.ToDouble(sTotal);
                                    p.fees_totaldue = Convert.ToDouble(sFeesDueTemp) + p.fees_surch + p.fees_pen;
                                    if (sFeesDueTemp != "")
                                        p.fees_due = Convert.ToDouble(sFeesDueTemp);
                                }
                            }

                            if (((t.tax_year != m_sCurrentTaxYear) && Convert.ToInt16(t.qtr_to_pay) > 1))
                            {
                            }
                            else
                            {
                                sAllSurch = sAllSurch + p.fees_surch;
                                sAllPen = sAllPen + p.fees_pen;
                                sAllTotalDue = sAllTotalDue + p.fees_totaldue;
                            }

                            p.bin = t.bin;
                            p.tax_year = t.tax_year;
                            p.fees_code = t.tax_code;
                            p.due_state = t.due_state;
                            p.qtr_to_pay = "F";
                            p.term = "F";
                            p.qtr = "X";

                            if (p.fees_code == "01")
                            {
                                p.fees_code = p.fees_code + "-" + sBnsCodeMain;
                                p.fees_desc = p.fees_desc + "(" + sBnsCodeMain + ")";
                            }

                            sQuery = "insert into pay_temp1(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,qtrpay) values "; // ALJ 01092004 sorting of tax and fees
                            sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                            sQuery += " '" + StringUtilities.Left(StringUtilities.HandleApostrophe(p.fees_desc), 40) + "', "; // ALJ 11212003 fixed SQL temporary solution
                            sQuery += p.fees_due + ", ";      // ALJ 11212003 fixed SQL
                            sQuery += p.fees_surch + ", ";    // ALJ 11212003 fixed SQL
                            sQuery += p.fees_pen + ", ";      // ALJ 11212003 fixed SQL
                            sQuery += p.fees_totaldue + ", "; // ALJ 11212003 fixed SQL
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(sQtrPay) + "')"; // ALJ 01092004 sorting of tax and fees
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();
                        }
                    }
            }
            else // ALJ PDELQ0523 (s)
            {
                MessageBox.Show("No Taxdues for the said Tax Year", m_sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            pSet.Close();

            if (t.tax_year == m_sCurrentTaxYear && t.qtr_to_pay == "1")
            {
                //OnFull();
            }
            else
            {
                if (t.tax_year != m_sCurrentTaxYear && t.due_state == "N")
                {
                    //OnFull();
                }
                if (t.tax_year != m_sCurrentTaxYear && t.due_state == "R")
                {
                    //OnFull();
                }
                if (t.qtr_to_pay != "1" && t.due_state == "N")
                {
                    //OnFull();
                    if (m_sNewWithQtr == "N")
                    {
                        //GetDlgItem(IDC_INSTALLMENT)->EnableWindow(0);
                    }
                }
                if (t.qtr_to_pay != "1" && t.due_state == "Q")
                {
                    //OnFull();
                    if (m_sNewWithQtr == "N")
                    {
                        //GetDlgItem(IDC_INSTALLMENT)->EnableWindow(0);
                    }
                }
                if (t.qtr_to_pay != "1" && t.due_state == "R")
                {
                    //OnInstallment();
                }

                if (m_iMultiTaxYear != 1 && Convert.ToInt32(m_sTaxYear) < Convert.ToInt32(m_sCurrentTaxYear))
                {
                    //OnFull();				
                    //GetDlgItem(IDC_INSTALLMENT)->EnableWindow(0);
                }
            }
        }

        public void ComputePenRate(String sTaxYear, String sQtr, String sDueState, double p_dSurchRate1, double p_dSurchRate2, double p_dSurchRate3, double p_dSurchRate4, double p_dPenRate1, double p_dPenRate2, double p_dPenRate3, double p_dPenRate4, double p_SurchQuart, double p_PenQuart)
        {
            String sQuery;
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

            OracleResultSet pSet = new OracleResultSet();
            // JJP 10182005 RETIREMENT PENALTY (s)
            String sSQL, sMaxYear = String.Empty;
            bool boRetStatus = false;
            sSQL = "select max(tax_year) as max_yr from pay_hist where bin = '" + m_sBIN + "'";
            pSet.Query = sSQL;
            if (pSet.Execute())
                if (pSet.Read())
                    sMaxYear = pSet.GetString("max_yr");
            pSet.Close();

            if (sDueState == "X")
            {
                sSQL = "select * from retired_bns_temp where bin = '" + m_sBIN + "' and tax_year = '" + sMaxYear + "'";
                pSet.Query = sSQL;
                if (pSet.Execute())
                    if (pSet.Read())
                        boRetStatus = true;
                pSet.Close();
            }

            sQuery = "select dt_operated from businesses where bin = '" + m_sBIN + "'";
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    m_sDtOperated = pSet.GetDateTime("dt_operated").ToString();
                }
                else
                {
                    pSet.Close();
                    sQuery = "select dt_operated from business_que where bin = '" + m_sBIN + "'";
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            m_sDtOperated = pSet.GetDateTime("dt_operated").ToString();
                        }
                }
            pSet.Close();

            String sDueJan = String.Empty, sDueFeb = String.Empty, sDueMar = String.Empty, sDueApr = String.Empty, sDueMay = String.Empty, sDueJun = String.Empty, sDueJul = String.Empty, sDueAug = String.Empty, sDueSep = String.Empty, sDueOct = String.Empty, sDueNov = String.Empty, sDueDec = String.Empty;
            pSet.Query = string.Format("select * from due_dates where due_year = '{0}'", AppSettingsManager.GetSystemDate().Year); // ALJ 02112005 pApp->m_sCurrentYear ask JJP 
            if (pSet.Execute())
                while (pSet.Read())
                {
                    String sCode = pSet.GetString("due_code").Trim();
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

            if (sDueState == "N")
            {
                m_sNewWithQtr = AppSettingsManager.GetConfigValue("15").Trim();

                if (m_sNewWithQtr == "N")
                {
                    //GetDlgItem(IDC_INSTALLMENT)->EnableWindow(0);
                }
            }

            m_sORDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
            m_sCurrentTaxYear = AppSettingsManager.GetSystemDate().Year.ToString();
            cdtDtOperated = Convert.ToDateTime(m_sDtOperated);
            sDtOperatedYear = sTaxYear; //LEO 02112003
            String sDtO1st, sDtO2nd, sDtO3rd, sDtO4th;
            sDtO1st = sDueJan.Substring(0, 6) + sDtOperatedYear;								// JJP 11062004 DUE DATES MANILA

            String sEndingDay;
            sEndingDay = m_sMonthlyCutoffDate;
            if (Convert.ToInt32(m_sMonthlyCutoffDate) > 30)
                sEndingDay = "30";

            sDtO2nd = sDueApr.Substring(0, 6) + sDtOperatedYear;								// JJP 11062004 DUE DATES MANILA
            sDtO3rd = sDueJul.Substring(0, 6) + sDtOperatedYear;								// JJP 11062004 DUE DATES MANILA
            sDtO4th = sDueOct.Substring(0, 6) + sDtOperatedYear;								// JJP 11062004 DUE DATES MANILA
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
                //(e) JJP 11062004 DUE DATES MANILA

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
                        m_sORDate = sDtO1st;
                    }
                    if (sQtr == "2")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO2nd);
                        cdtDtOperated = Convert.ToDateTime(sDtO2nd);
                        m_sORDate = sDtO2nd;
                    }
                    if (sQtr == "3")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO3rd);
                        cdtDtOperated = Convert.ToDateTime(sDtO3rd);
                        m_sORDate = sDtO3rd;
                    }
                    if (sQtr == "4")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO4th);
                        cdtDtOperated = Convert.ToDateTime(sDtO4th);
                        m_sORDate = sDtO4th;
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Jan || m_sORDate == sJan)
                {
                    if (m_sORDate == sJan)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dSurchRate1 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate2 += m_dPenRate;
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Jan && cdtQtrDate <= Jan)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Feb || m_sORDate == sFeb)
                {
                    if (m_sORDate == sFeb)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dPenRate1 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate2 += m_dPenRate;
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }

                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Feb && cdtQtrDate <= Feb)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Mar || m_sORDate == sMar)
                {
                    if (m_sORDate == sMar)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dPenRate1 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate2 += m_dPenRate;
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Mar && cdtQtrDate <= Mar)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Apr || m_sORDate == sApr)
                {
                    if (m_sORDate == sApr)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dSurchRate2 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }

                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Apr && cdtQtrDate <= Apr)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > May || m_sORDate == sMay)
                {
                    if (m_sORDate == sMay)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= May && cdtQtrDate <= May)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Jun || m_sORDate == sJun)
                {
                    if (m_sORDate == sJun)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Jun && cdtQtrDate <= Jun)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Jul || m_sORDate == sJul)
                {
                    if (m_sORDate == sJul)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dSurchRate3 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Jul && cdtQtrDate <= Jul)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Aug || m_sORDate == sAug)
                {
                    if (m_sORDate == sAug)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Aug && cdtQtrDate <= Aug)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Sep || m_sORDate == sSep)
                {
                    if (m_sORDate == sSep)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Sep && cdtQtrDate <= Sep)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Oct || m_sORDate == sOct)
                {
                    if (m_sORDate == sOct)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dSurchRate4 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Oct && cdtQtrDate <= Oct)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Nov || m_sORDate == sNov)
                {
                    if (m_sORDate == sNov)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Nov && cdtQtrDate <= Nov)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (Convert.ToDateTime(m_sORDate) > Dec || m_sORDate == sDec)
                {
                    if (m_sORDate == sDec)
                    {
                    }
                    else
                    {
                        if (sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < AppSettingsManager.GetSystemDate().Year || Convert.ToInt32(sMaxYear) < AppSettingsManager.GetSystemDate().Year) && boRetStatus == false) || sDueState == "A") // RTL 04012005 for retirement // JJP 03182005 REVENUE EXAM	// JJP 01062006 revised penalty of retirement
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Dec && cdtQtrDate <= Dec)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
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

                m_dSurch1 = dSurchRate1;
                m_dSurch2 = dSurchRate2;
                m_dSurch3 = dSurchRate3;
                m_dSurch4 = dSurchRate4;
                m_dPenRate1 = dPenRate1;
                m_dPenRate2 = dPenRate2;
                m_dPenRate3 = dPenRate3;
                m_dPenRate4 = dPenRate4;
                m_dSurchQuart = SurchQuart;
                m_dPenQuart = PenQuart;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String sDate = "", sUser = "";
            String sStartPayDate = "";

            if (dgView.Rows.Count <= 0)
            {
                MessageBox.Show("Empty Field(s)", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Save record for compromise agreement?", "Compromise Agreement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (frmLogIn frmLogInOut = new frmLogIn())
                {
                    frmLogInOut.ShowDialog();
                    sUser = frmLogInOut.m_sUserCode.Trim();
                }

                if (sUser != "")
                {
                    sDate = string.Format("{0}/{1}/{2}", dtpApp.Value.Month, dtpApp.Value.Day, dtpApp.Value.Year);
                    sStartPayDate = string.Format("{0}/{1}/{2}", dtpStart.Value.Month, dtpStart.Value.Day, dtpStart.Value.Year); // ALJ 08112006 save monthly due date
                    SaveCompromise(sBnsCode, txtRefNum.Text, txtNoPayment.Text, sDate, sUser);
                    SaveDueDates(sStartPayDate, txtNoPayment.Text); // ALJ 08112006 save monthly due date
                    MessageBox.Show("Record saved for compromise agreement!", "Compromise Agreement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSearch.PerformClick();
                }
            }
        }

        struct stCompDues
        {
            public String sTaxYear;
            public String sFeesCode;
            public double dFeesDue;
            public double dFeesSurch;
            public double dFeesPen;
            public double dTFeesDue;
            public String sDueState;
            public int iTermPay;
            public String sPaySw;
        }

        private void SaveCompromise(String sBnsCode, String sRefNo, String sNoPayment, String sApprove, String sUser)
        {
            String sDateFrom = "", sDateTo = "", sString = "", sQuery = "";
            OracleResultSet pSet = new OracleResultSet();
            int intCount = 0;
            try
            {
                stCompDues stCompDuesx;
                sQuery = "select distinct tax_year from pay_temp1 where bin = '" + m_sBIN + "'";
                sQuery += " and qtr_to_pay = 'F' order by tax_year";		// JJP 01182006 Compromise QA - correct saving of tax_year in from & to field
                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sDateFrom = pSet.GetString("tax_year");
                        //int.TryParse(pSet.ExecuteScalar().Trim(), out intCount);
                        //if (intCount == 1)
                            sDateTo = sDateFrom;
                        //else
                        //{
                        //  //should get the last record from database
                        //    sDateTo = pSet.GetString("tax_year");
                        //}
                    }
                pSet.Close();

                sQuery = "insert into compromise_tbl values(";
                sQuery += "'" + m_sBIN + "',";
                sQuery += "'" + sDateFrom + "',";
                sQuery += "'" + sDateTo + "',";
                sQuery += "'" + sRefNo.Trim() + "',";
                sQuery += sNoPayment + ",";
                sQuery += "to_date('" + sApprove + "','MM/dd/yyyy'),";
                sQuery += "'" + sUser + "')";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();

                sQuery = "select * from pay_temp1 where bin = '" + m_sBIN + "'";
                sQuery += " and qtr_to_pay = 'F'";
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        stCompDuesx.sTaxYear = pSet.GetString("tax_year");
                        stCompDuesx.sFeesCode = pSet.GetString("fees_code");
                        stCompDuesx.dFeesDue = pSet.GetDouble("fees_due");
                        stCompDuesx.dFeesSurch = pSet.GetDouble("fees_surch");
                        stCompDuesx.dFeesPen = pSet.GetDouble("fees_pen");
                        stCompDuesx.dTFeesDue = pSet.GetDouble("fees_totaldue");
                        stCompDuesx.sDueState = pSet.GetString("due_state");
                        stCompDuesx.iTermPay = 0;

                        sQuery = "insert into compromise_due values (";
                        sQuery += "'" + m_sBIN + "',";
                        sQuery += "'" + stCompDuesx.sTaxYear + "',";
                        // JJP 04112006 (s) Fixed the centavos difference
                        String sTmpBnsCode;
                        if (stCompDuesx.sFeesCode.Substring(0, 1) == "B")
                        {
                            sTmpBnsCode = stCompDuesx.sFeesCode.Substring(stCompDuesx.sFeesCode.Length - 1);
                            sQuery += "'" + sTmpBnsCode + "',";
                        }
                        else
                            // JJP 04112006 (e) Fixed the centavos difference
                        sQuery += "'" + sBnsCode + "',";
                        sQuery += "'" + stCompDuesx.sFeesCode + "',";

                        sString = stCompDuesx.dFeesDue.ToString();
                        sQuery += sString + ",";

                        sString = stCompDuesx.dFeesSurch.ToString();
                        sQuery += sString + ",";

                        sString = stCompDuesx.dFeesPen.ToString();
                        sQuery += sString + ",";

                        sString = stCompDuesx.dTFeesDue.ToString();
                        sQuery += sString + ",";
                        sQuery += "'" + stCompDuesx.sDueState + "',";

                        sString = stCompDuesx.iTermPay.ToString();
                        sQuery += sString + ",";
                        sQuery += "'N')";
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                    }
                }
                pSet.Close();

                sQuery = "delete from taxdues where bin = '" + m_sBIN + "'";
                sQuery += " and tax_year between '" + sDateFrom + "' and '" + sDateTo + "'";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();

                //(s) JJP 02062006 Fixing Compromise Agreement
                sQuery = "delete from business_que where bin = '" + m_sBIN + "'";
                sQuery += " and tax_year between '" + sDateFrom + "' and '" + sDateTo + "'";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();

                sQuery = "insert into buss_hist select * from businesses where bin = '" + m_sBIN + "'";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();

                sQuery = "update businesses set bns_stat = 'REN', tax_year = '" + sDateTo + "'";
                sQuery += " where bin = '" + m_sBIN + "'";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void SaveDueDates(String p_sStartPayDate, String p_sNoPayment)
        {
            String sQuery, sDateOfPay = String.Empty, sConvert, sRem = String.Empty;
            OracleResultSet pSet = new OracleResultSet();
            try
            {
                sQuery = "delete from comp_duedate where bin = '" + m_sBIN + "'";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();

                for (int i = 0; i <= Convert.ToInt32(p_sNoPayment); i++)
                {
                    if (i == 0)
                    {
                        sDateOfPay = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                        sRem = "25% INITIAL PAYMENT";
                    }
                    if (i == 1)
                    {
                        sDateOfPay = p_sStartPayDate;
                        sRem = "1 of " + p_sNoPayment;
                    }
                    if (i > 1)
                    {
                        sDateOfPay = Convert.ToDateTime(p_sStartPayDate).AddMonths(i - 1).ToString("MM/dd/yyyy");
                        sRem = string.Format("{0} of {1}", i, p_sNoPayment);

                    }

                    sQuery = "insert into comp_duedate values(";
                    sQuery += "'" + m_sBIN + "',";
                    sQuery += "to_date('" + sDateOfPay + "','MM/dd/yyyy'),";
                    sConvert = i.ToString();
                    sQuery += sConvert + ",";
                    sQuery += "'" + sRem + "')";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
