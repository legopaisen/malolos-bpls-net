

// RMC 20111227 added Gross monitoring module for gross >= 200000 (s)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.Reports;
using ComponentFactory.Krypton.Toolkit;
using Amellar.Common.BPLSApp;
using Amellar.Common.SearchBusiness;
using BIN;
using Amellar.Common.StringUtilities;
using Amellar.Common.Reports;
using Amellar.Common.TransactionLog;
using System.IO;
using System.Diagnostics;

namespace Amellar.Common.SOA
{
    public partial class frmSOA : Form
    {
        BPLSAppSettingList sList = new BPLSAppSettingList();
        BPLSAppSettingList sOwnList = new BPLSAppSettingList();
        //ReportClass rClass = new ReportClass();   // RMC 20150107


        string strQtr1 = string.Empty;
        string strQtr2 = string.Empty;
        string strQtr3 = string.Empty;
        string strQtr4 = string.Empty;

        bool m_bExistTaxDue = false;
        bool m_bExistTaxPU = false;
        bool m_bMultiBns = false;
        bool m_bIsRenPUT = false;
        bool m_bRetVoidSurchPen = false;
        bool bCompromise = false;
        bool m_bIsCompromise = false;

        double m_dTotalDue = 0, m_dTotalSurcharge = 0, m_dTotalPenalty = 0, m_dTotalTotalDue = 0;
        double m_dTotalDue1 = 0, m_dTotalSurcharge1 = 0, m_dTotalPenalty1 = 0, m_dTotalTotalDue1 = 0;
        double dDue = 0, dPen = 0, dSurch = 0, dTot = 0;

        int m_iNextQtrToPay = 1;
        int m_iSwitch = 0;
        int iCountTaxYear = 0;
        int iCountBinSeries = 0;

        public int iFormState = 0;
        public string sBIN;

        string m_sRevYear = string.Empty;
        string m_sGross = string.Empty;
        string m_sIsQtrly = string.Empty;
        string m_sNextQtrToPay = string.Empty;
        string m_sTerm = string.Empty;
        string m_sQtr1 = string.Empty, m_sQtr2 = string.Empty, m_sQtr3 = string.Empty, m_sQtr4 = string.Empty;
        string p_bin = string.Empty, p_tax_year = string.Empty, p_term = string.Empty, p_qtr = string.Empty, p_fees_desc = string.Empty;
        string p_fees_due = string.Empty, p_fees_surch = string.Empty, p_fees_pen = string.Empty, p_fees_totaldue = string.Empty;
        string p_fees_code = string.Empty, p_due_state = string.Empty, p_qtr_to_pay = string.Empty;
        string strPaymentTerm = string.Empty;
        string m_sOwnCode = string.Empty;
        string m_sPrevOwnCode = string.Empty;
        string m_sNewOwnCode = string.Empty;
        string m_sCheckOwnerCode = string.Empty;
        string m_sOldBnsName = string.Empty;
        string m_sDtOperated = string.Empty;
        string sTaxYear = string.Empty;
        string m_sStatus = string.Empty;
        string m_sPrevBnsLoc = string.Empty;
        string m_sCompPaySw = string.Empty;
        string m_sCreditLeft = string.Empty;

        //MCR 20140801 (s)
        string m_sMonthlyCutoffDate = "";
        string m_sInitialDueDate = "";
        string m_sCurrentTaxYear = "";
        string m_sNewWithQtr = "";
        double m_dPenRate = 0;
        double m_dSurchRate = 0;
        DateTime mv_cdtORDate;
        bool m_bEnableTaxDue = false, m_bEnableTaxPU = false;
        //MCR 20140801 (e)

        int m_iMultiTaxYear = 0;

        public DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin

        public frmSOA()
        {
            InitializeComponent();
        }

        private void frmSOA_Load(object sender, EventArgs e)
        {
            bin1.txtLGUCode.Text = AppSettingsManager.GetConfigObject("10");
            bin1.txtDistCode.Text = AppSettingsManager.GetConfigObject("11");
            bin1.txtTaxYear.Focus();

            // RMC 20161215 display quarter due and date in SOA for Binan (s)
            if (AppSettingsManager.GetConfigValue("10") == "243")
                groupBox1.Visible = false;
            // RMC 20161215 display quarter due and date in SOA for Binan (e)

            if (iFormState == 0)
            {
                txtLGUCode.Text = sBIN.Substring(0, 3);
                txtDistCode.Text = sBIN.Substring(4, 2);
                txtBinYr.Text = sBIN.Substring(7, 4);
                txtBinSeries.Text = sBIN.Substring(12, 7);

                // RMC 20170110 added additional validation in SOA of subject for verification (s)
                if (AppSettingsManager.TreasurerModule(sBIN))
                {
                    MessageBox.Show("SOA is subject for verification.");
                    return;
                }
                // RMC 20170110 added additional validation in SOA of subject for verification (e)

                CleanPayTemp(sBIN);
                ComputeTaxAndFees(sBIN);

                CreateSOATBL();//MCR 20210610
            }

        }

        private void GetGross(string sBIN, string sTaxYear, string sTaxState, string sQtr, string sBnsCode)
        {
            m_sGross = "";  // RMC 20150921 corrections in SOA of Bin with Permit Update transaction
            OracleResultSet result = new OracleResultSet();
            string sStat = string.Empty;
            //string sCode = sBnsCode.StartsWith("B")
            //sBnsCode = sBnsCode.Substring(1, sBnsCode.Length);
            if (sBnsCode.StartsWith("B"))
                sBnsCode = sBnsCode.Substring(1);

            if (sTaxState == "R" || sTaxState == "X" || sTaxState == "A")
            {
                //result.Query = "select gross from bill_gross_info where bin = '" + sBIN.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "' and bns_code = '" + sBnsCode.Trim() + "' and due_state <> 'N'";
                result.Query = "select * from bill_gross_info where bin = '" + sBIN.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "' and bns_code = '" + sBnsCode.Trim() + "' and due_state <> 'N'";    // RMC 20161208 print pre gross in SOA if with value for Binan
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        if (sTaxState == "R" || sTaxState == "X") //MCR 20150115 X added
                        {
                            if (result.GetDouble("pre_gross") != 0)  // RMC 20161208 print pre gross in SOA if with value for Binan (s)
                                m_sGross = result.GetDouble("pre_gross").ToString();    // RMC 20161208 print pre gross in SOA if with value for Binan (e)
                            else
                                m_sGross = result.GetDouble("gross").ToString();
                        }
                        if (sTaxState == "A" || result.GetDouble("adj_gross") != 0) //JHB 20181211 merge from BINAN display rev gross on SOA
                            m_sGross = result.GetDouble("adj_gross").ToString();

                        sStat = result.GetString("bns_stat"); //AFM 20200527
                    }
                }
                result.Close();
            }

            if(sTaxState == "X" && sStat == "REN") //AFM 202005115 another condition for retirement with 1st qtr current year payment to properly get gross of retirement
            {
                //get gross form retired_bns_temp
                result.Query = "select * from retired_bns_temp where bin = '" + sBIN.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "' and bns_code_main = '" + sBnsCode.Trim() + "' and bns_stat = 'RET'";
                if(result.Execute())
                    if(result.Read())
                    {
                        m_sGross = result.GetDouble("gross").ToString();
                    }
                result.Close();
            }

            if (sTaxState == "Q")
            {
                result.Query = "select gross from bill_gross_info where bin = '" + sBIN.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "' and bns_code = '" + sBnsCode.Trim() + "' and due_state = 'Q'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        m_sGross = result.GetDouble("gross").ToString();
                    }
                }
                result.Close();
            }

            if (sTaxState == "N")
            {
                //result.Query = "select capital from bill_gross_info where bin = '" + sBIN.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "' and bns_code = '" + sBnsCode.Trim() + "' and due_state = 'N'";
                result.Query = "select capital from bill_gross_info where bin = '" + sBIN.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "' and bns_code = '" + sBnsCode.Trim() + "' and (due_state = 'N' or bns_stat = 'NEW')";    // RMC 20150108
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        m_sGross = result.GetDouble("capital").ToString();
                    }
                }
                result.Close();
            }

            // RMC 20150921 corrections in SOA of Bin with Permit Update transaction (s)
            if (sTaxState == "P")
            {
                result.Query = "select * from bill_gross_info where bin = '" + sBIN.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "' and bns_code = '" + sBnsCode.Trim() + "' and due_state = 'P'"; 
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        if(result.GetString("bns_stat") == "NEW")
                            m_sGross = result.GetDouble("capital").ToString();
                        else
                            m_sGross = result.GetDouble("gross").ToString();
                    }
                }
                result.Close();
            }
            // RMC 20150921 corrections in SOA of Bin with Permit Update transaction (e)

        }

        public void CleanPayTemp(string sBIN)
        {
            OracleResultSet result = new OracleResultSet();
            /*
            bool bExist = false;
            result.Query = "select * from pay_temp where bin = '" + sBIN + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bExist = true;
            }
            result.Close();

            if (bExist)
            {
                result.Query = "delete from pay_temp where bin = '" + sBIN + "'";
                if (result.ExecuteNonQuery() != 0)
                {
                }
                result.Close();
            }*/
            result.Query = "delete from pay_temp where bin = '" + sBIN + "'";
            if (result.ExecuteNonQuery() != 0)
            {
            }
            result.Close();

            result.Query = "delete from pay_temp_bns where bin = '" + sBIN + "'";
            if (result.ExecuteNonQuery() != 0)
            {
            }
            result.Close();




        }

        public void GetData(string sBIN)
        {
            string sBnsHouseNo, sBnsStreet, sBnsBrgy, sBnsMun, sOwnCode = "";
            string sLn, sFn, sMi;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            result.Query = "select * from businesses where bin = '" + sBIN + "'";
            sBIN = sBIN;
            if (result.Execute())
            {
                if (result.Read())
                {
                    txtBnsName.Text = result.GetString("bns_nm").Trim();
                    txtCode.Text = result.GetString("bns_code").Trim();
                    //txtStat.Text = result.GetString("bns_stat").Trim();
                    txtBin.Text = result.GetString("bin").Trim();
                    //txtTaxYear.Text = result.GetString("tax_year").Trim();
                    sBnsHouseNo = result.GetString("bns_house_no").Trim();
                    sBnsStreet = result.GetString("bns_street").Trim();
                    sBnsBrgy = result.GetString("bns_brgy").Trim();
                    sBnsMun = result.GetString("bns_mun").Trim();
                    sOwnCode = result.GetString("own_code");
                    txtBnsAdd.Text = sBnsHouseNo + " " + sBnsStreet + " " + sBnsBrgy + " " + sBnsMun;


                    result2.Query = "select * from own_names where own_code = '" + sOwnCode + "'";
                    if (result2.Execute())
                    {
                        if (result2.Read())
                        {
                            txtLName.Text = result2.GetString("own_ln").Trim();
                            txtFName.Text = result2.GetString("own_fn").Trim();
                            txtMI.Text = result2.GetString("own_mi").Trim();
                        }
                    }
                    result2.Close();
                }
                else
                {
                    result.Close();
                    result.Query = "select * from business_que where bin = '" + sBIN + "'";
                    sBIN = sBIN;
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            txtBnsName.Text = result.GetString("bns_nm").Trim();
                            txtCode.Text = result.GetString("bns_code").Trim();
                            txtBin.Text = result.GetString("bin").Trim();
                            txtTaxYear.Text = result.GetString("tax_year").Trim();
                            sBnsHouseNo = result.GetString("bns_house_no").Trim();
                            sBnsStreet = result.GetString("bns_street").Trim();
                            sBnsBrgy = result.GetString("bns_brgy").Trim();
                            sBnsMun = result.GetString("bns_mun").Trim();
                            sOwnCode = result.GetString("own_code");
                            txtBnsAdd.Text = sBnsHouseNo + " " + sBnsStreet + " " + sBnsBrgy + " " + sBnsMun;


                            result2.Query = "select * from own_names where own_code = '" + sOwnCode + "'";
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    txtLName.Text = result2.GetString("own_ln").Trim();
                                    txtFName.Text = result2.GetString("own_fn").Trim();
                                    txtMI.Text = result2.GetString("own_mi").Trim();
                                }
                            }
                            result2.Close();
                        }
                        else
                        {
                            result.Close();
                            result.Query = "select * from spl_business_que where bin = '" + sBIN + "'";
                            sBIN = sBIN;
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    txtBnsName.Text = result.GetString("bns_nm").Trim();
                                    txtCode.Text = result.GetString("bns_code").Trim();
                                    txtBin.Text = result.GetString("bin").Trim();
                                    txtTaxYear.Text = result.GetString("tax_year").Trim();
                                    sBnsHouseNo = result.GetString("bns_house_no").Trim();
                                    sBnsStreet = result.GetString("bns_street").Trim();
                                    sBnsBrgy = result.GetString("bns_brgy").Trim();
                                    sBnsMun = result.GetString("bns_mun").Trim();
                                    sOwnCode = result.GetString("own_code");
                                    txtBnsAdd.Text = sBnsHouseNo + " " + sBnsStreet + " " + sBnsBrgy + " " + sBnsMun;


                                    result2.Query = "select * from own_names where own_code = '" + sOwnCode + "'";
                                    if (result2.Execute())
                                    {
                                        if (result2.Read())
                                        {
                                            txtLName.Text = result2.GetString("own_ln").Trim();
                                            txtFName.Text = result2.GetString("own_fn").Trim();
                                            txtMI.Text = result2.GetString("own_mi").Trim();
                                        }
                                    }
                                    result2.Close();
                                }
                            }
                        }
                    }

                }
            }
            result.Close();

            // RMC 20160226 added display of tax credit in frmSoa (s)
            if (sOwnCode != "")
            {
                double dBalance = 0;
                
                //result.Query = "select * from dbcr_memo where bin = '" + sBIN.Trim() + "' and served = 'N' and multi_pay = 'N'";
                //result.Query = "select * from dbcr_memo where own_code = '" + sOwnCode.Trim() + "' and served = 'N' and multi_pay = 'N'"; //JARS 20170703
                result.Query = "select * from dbcr_memo where bin = '" + sBIN.Trim() + "' and served = 'N' and multi_pay = 'N'"; //JARS 20171010
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dBalance = result.GetDouble("balance");
                        m_sCreditLeft = string.Format("{0:#,##0.00}", dBalance);
                        txtCreditLeft.Text = m_sCreditLeft;
                    }
                }
                result.Close();
            }

            
            // RMC 20160226 added display of tax credit in frmSoa (e)
        }

        struct PayTempStruct
        {
            public string bin;
            public string tax_year;
            public string term;
            public string qtr;
            public string fees_desc;
            public string fees_due;
            public string fees_surch;
            public string fees_pen;
            public string fees_totaldue;
            public string fees_code;
            public string due_state;
            public string qtr_to_pay;
            public string fees_code_sort;
        }

        struct TaxDuesStruct
        {
            public string bin;
            public string tax_year;
            public string qtr_to_pay;
            public string tax_code;
            public string amount;
            public string due_state;
        }

        struct PenRate
        {
            public double p_dSurchRate1;
            public double p_dSurchRate2;
            public double p_dSurchRate3;
            public double p_dSurchRate4;
            public double p_dPenRate1;
            public double p_dPenRate2;
            public double p_dPenRate3;
            public double p_dPenRate4;
            public double p_SurchQuart;
            public double p_PenQuart;
        }

        public void ComputePenRate(String sTaxYear, String sQtr, String sDueState
            , double p_dSurchRate1, double p_dSurchRate2, double p_dSurchRate3, double p_dSurchRate4
            , double p_dPenRate1, double p_dPenRate2, double p_dPenRate3, double p_dPenRate4
            , double p_SurchQuart, double p_PenQuart)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            String sJan, sFeb, sMar, sApr, sMay, sJun, sJul, sAug, sSep, sOct, sNov, sDec;
            DateTime Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec;
            double dSurchRate1, dSurchRate2, dSurchRate3, dSurchRate4;
            double dPenRate1, dPenRate2, dPenRate3, dPenRate4;
            double SurchQuart, PenQuart;
            DateTime cdtDtOperated, cdtDtO1st, cdtDtO2nd, cdtDtO3rd, cdtDtO4th, cdtQtrDate;
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

            PenRate pr;

            String sMaxYear = "", m_sORDate;
            m_sORDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");

            bool boRetStatus = false;
            pSet.Query = "select max(tax_year) as max_yr from pay_hist where bin = '" + sBIN + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sMaxYear = pSet.GetString("max_yr");
            pSet.Close();

            if (sDueState == "X")
            {
                pSet.Query = "select * from retired_bns_temp where bin ='" + sBIN + "' and tax_year = '" + sMaxYear + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        boRetStatus = true;
                pSet.Close();
            }

            DateTime sDueJan = new DateTime(), sDueFeb = new DateTime(), sDueMar = new DateTime(), sDueApr = new DateTime(), sDueMay = new DateTime(), sDueJun = new DateTime(), sDueJul = new DateTime(), sDueAug = new DateTime(), sDueSep = new DateTime(), sDueOct = new DateTime(), sDueNov = new DateTime(), sDueDec = new DateTime();
            pSet.Query = string.Format("select * from due_dates where due_year = '{0}'", AppSettingsManager.GetConfigValue("12"));
            if (pSet.Execute())
                while (pSet.Read())
                {
                    String sCode = pSet.GetString("due_code").Trim();
                    if (sCode == "01")
                        sDueJan = pSet.GetDateTime("due_date");
                    else if (sCode == "02")
                        sDueFeb = pSet.GetDateTime("due_date");
                    else if (sCode == "03")
                        sDueMar = pSet.GetDateTime("due_date");
                    else if (sCode == "04")
                        sDueApr = pSet.GetDateTime("due_date");
                    else if (sCode == "05")
                        sDueMay = pSet.GetDateTime("due_date");
                    else if (sCode == "06")
                        sDueJun = pSet.GetDateTime("due_date");
                    else if (sCode == "07")
                        sDueJul = pSet.GetDateTime("due_date");
                    else if (sCode == "08")
                        sDueAug = pSet.GetDateTime("due_date");
                    else if (sCode == "09")
                        sDueSep = pSet.GetDateTime("due_date");
                    else if (sCode == "10")
                        sDueOct = pSet.GetDateTime("due_date");
                    else if (sCode == "11")
                        sDueNov = pSet.GetDateTime("due_date");
                    else if (sCode == "12")
                        sDueDec = pSet.GetDateTime("due_date");
                }
            pSet.Close();

            if (sDueState == "N")
            {
                m_sNewWithQtr = AppSettingsManager.GetConfigValue("15").Trim();

                if (m_sNewWithQtr == "N")
                {

                }
            }


            String sQtrPaid = "";
            pSet.Query = "select * from pay_hist where bin = '" + sBIN + "' order by tax_year desc, qtr_paid desc";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sQtrPaid = pSet.GetString("qtr_paid");
                    if (sQtrPaid == "F")
                        sQtrPaid = "4";
                }
            pSet.Close();

            m_sCurrentTaxYear = AppSettingsManager.GetSystemDate().Year.ToString();
            cdtDtOperated = Convert.ToDateTime(m_sDtOperated);
            sDtOperatedYear = sTaxYear;

            String sDtO1st, sDtO2nd, sDtO3rd, sDtO4th;

            String sEndingDay;
            sEndingDay = m_sMonthlyCutoffDate;
            if (Convert.ToInt32(m_sMonthlyCutoffDate) > 30)
                sEndingDay = "30";

            sDtO1st = sDueJan.ToString("MM/dd/").Substring(0, 6) + sDtOperatedYear;
            sDtO2nd = sDueApr.ToString("MM/dd/").Substring(0, 6) + sDtOperatedYear;
            sDtO3rd = sDueJul.ToString("MM/dd/").Substring(0, 6) + sDtOperatedYear;
            sDtO4th = sDueOct.ToString("MM/dd/").Substring(0, 6) + sDtOperatedYear;

            cdtDtO1st = Convert.ToDateTime(sDtO1st);
            cdtDtO2nd = Convert.ToDateTime(sDtO2nd);
            cdtDtO3rd = Convert.ToDateTime(sDtO3rd);
            cdtDtO4th = Convert.ToDateTime(sDtO4th);
            cdtQtrDate = new DateTime();

            if (sQtr == "1")
                cdtQtrDate = Convert.ToDateTime(sDtO1st);
            if (sQtr == "2")
                cdtQtrDate = Convert.ToDateTime(sDtO2nd);
            if (sQtr == "3")
                cdtQtrDate = Convert.ToDateTime(sDtO3rd);
            if (sQtr == "4")
                cdtQtrDate = Convert.ToDateTime(sDtO4th);

            String sBaseYear;
            for (int dYear = Convert.ToInt16(sTaxYear); dYear <= Convert.ToInt16(m_sCurrentTaxYear); dYear++)
            {
                sBaseYear = dYear.ToString();

                sJan = sDueJan.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sFeb = sDueFeb.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sMar = sDueMar.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sApr = sDueApr.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sMay = sDueMay.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sJun = sDueJun.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sJul = sDueJul.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sAug = sDueAug.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sSep = sDueSep.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sOct = sDueOct.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sNov = sDueNov.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sDec = sDueDec.ToString("MM/dd/").Substring(0, 6) + sBaseYear;

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

                if (sDueState == "Q")
                {
                    if (sQtr == "1")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO1st);
                        cdtDtOperated = Convert.ToDateTime(sDtO1st);
                    }
                    if (sQtr == "2")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO2nd);
                        cdtDtOperated = Convert.ToDateTime(sDtO2nd);
                    }
                    if (sQtr == "3")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO3rd);
                        cdtDtOperated = Convert.ToDateTime(sDtO3rd);
                    }
                    if (sQtr == "4")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO4th);
                        cdtDtOperated = Convert.ToDateTime(sDtO4th);
                    }
                }

                if (mv_cdtORDate > Jan || m_sORDate == sJan || (((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("01/20/" + sBaseYear) || m_sORDate == ("01/20/" + sBaseYear)) && mv_cdtORDate > Dec) || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("01/20/" + sBaseYear) || m_sORDate == ("01/20/" + sBaseYear)) && Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")))))
                {

                    if (m_sORDate == sJan || (mv_cdtORDate < Jan && mv_cdtORDate.Month == Jan.Month && mv_cdtORDate.Year == Jan.Year))
                    {

                        if (sBaseYear != sTaxYear)
                        {

                        }
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
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

                if (mv_cdtORDate > Feb || m_sORDate == sFeb)
                {
                    if (m_sORDate == sFeb || (mv_cdtORDate < Feb && mv_cdtORDate.Month == Feb.Month && mv_cdtORDate.Year == Feb.Year))
                    {

                        if (sBaseYear != sTaxYear)
                        {

                        }
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 = dPenRate1 + m_dPenRate;
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

                if (mv_cdtORDate > Mar || m_sORDate == sMar)
                {
                    if (m_sORDate == sMar || (mv_cdtORDate < Mar && mv_cdtORDate.Month == Mar.Month && mv_cdtORDate.Year == Mar.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
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

                if (mv_cdtORDate > Apr || m_sORDate == sApr || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("04/20/" + sBaseYear) || m_sORDate == ("04/20/" + sBaseYear)) && Convert.ToInt16(sQtrPaid) <= 1 && Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))))
                {
                    if (m_sORDate == sApr || (mv_cdtORDate < Apr && mv_cdtORDate.Month == Apr.Month && mv_cdtORDate.Year == Apr.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate2 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;

                            if ((IsPaidOnLastQtr(sBIN, sTaxYear, "", "1") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate2 = dPenRate1;

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

                if (mv_cdtORDate > May || m_sORDate == sMay)
                {
                    if (m_sORDate == sMay || (mv_cdtORDate < May && mv_cdtORDate.Month == May.Month && mv_cdtORDate.Year == May.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            if ((IsPaidOnLastQtr(sBIN, sTaxYear, "", "1") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate2 = dPenRate1;
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

                if (mv_cdtORDate > Jun || m_sORDate == sJun)
                {
                    if (m_sORDate == sJun || (mv_cdtORDate < Jun && mv_cdtORDate.Month == Jun.Month && mv_cdtORDate.Year == Jun.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            if ((IsPaidOnLastQtr(sBIN, sTaxYear, "", "1") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate2 = dPenRate1;
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

                if (mv_cdtORDate > Jul || m_sORDate == sJul || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("07/20/" + sBaseYear) || m_sORDate == ("07/20/" + sBaseYear)) && Convert.ToInt16(sQtrPaid) <= 2 && Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))))
                {
                    if (m_sORDate == sJul || (mv_cdtORDate < Jul && mv_cdtORDate.Month == Jul.Month && mv_cdtORDate.Year == Jul.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate3 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;

                            if ((IsPaidOnLastQtr(sBIN, sTaxYear, "", "2") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate3 = dPenRate2;

                            if (sBaseYear != sTaxYear)
                                dPenRate4 += m_dPenRate;
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


                if (mv_cdtORDate > Aug || m_sORDate == sAug)
                {
                    if (m_sORDate == sAug || (mv_cdtORDate < Aug && mv_cdtORDate.Month == Aug.Month && mv_cdtORDate.Year == Aug.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            if ((IsPaidOnLastQtr(sBIN, sTaxYear, "", "2") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate3 = dPenRate2;
                            if (sBaseYear != sTaxYear)
                                dPenRate4 += m_dPenRate;
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

                if (mv_cdtORDate > Sep || m_sORDate == sSep)
                {
                    if (m_sORDate == sSep || (mv_cdtORDate < Sep && mv_cdtORDate.Month == Sep.Month && mv_cdtORDate.Year == Sep.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;

                            if ((IsPaidOnLastQtr(sBIN, sTaxYear, "", "2") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate3 = dPenRate2;

                            if (sBaseYear != sTaxYear)
                                dPenRate4 += m_dPenRate;
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


                if (mv_cdtORDate > Oct || m_sORDate == sOct || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("10/20/" + sBaseYear) || m_sORDate == ("10/20/" + sBaseYear)) && Convert.ToInt16(sQtrPaid) <= 3 && Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))))
                {
                    if (m_sORDate == sOct || (mv_cdtORDate < Oct && mv_cdtORDate.Month == Oct.Month && mv_cdtORDate.Year == Oct.Year))
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate4 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                            if ((IsPaidOnLastQtr(sBIN, sTaxYear, "", "3") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate4 = dPenRate3;
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

                if (mv_cdtORDate > Nov || m_sORDate == sNov)
                {
                    if (m_sORDate == sNov || (mv_cdtORDate < Nov && mv_cdtORDate.Month == Nov.Month && mv_cdtORDate.Year == Nov.Year))
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                            if ((IsPaidOnLastQtr(sBIN, sTaxYear, "", "3") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate4 = dPenRate3;
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

                if (mv_cdtORDate > Dec || m_sORDate == sDec)
                {
                    if (m_sORDate == sDec || (mv_cdtORDate < Dec && mv_cdtORDate.Month == Dec.Month && mv_cdtORDate.Year == Dec.Year))
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                            if ((IsPaidOnLastQtr(sBIN, sTaxYear, "", "3") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate4 = dPenRate3;
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

                pr.p_dSurchRate1 = dSurchRate1;
                pr.p_dSurchRate2 = dSurchRate2;
                pr.p_dSurchRate3 = dSurchRate3;
                pr.p_dSurchRate4 = dSurchRate4;
                pr.p_dPenRate1 = dPenRate1;
                pr.p_dPenRate2 = dPenRate2;
                pr.p_dPenRate3 = dPenRate3;
                pr.p_dPenRate4 = dPenRate4;
                pr.p_SurchQuart = SurchQuart;
                pr.p_PenQuart = PenQuart;
            }
        }

        private void SaveToPayTemp(String p_sBin, String p_sTaxYear, String p_sTerm, String p_sQtr, String p_sFeesDesc, String p_sFeesDue, String p_sFeesSurch, String p_sFeesPen, String p_sFeesTotal, String p_sFeesCode, String p_sDueState, String p_sQtrToPay, String p_sFeesSort)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            sQuery = "insert into pay_temp(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,fees_code_sort) values ";
            sQuery += "('" + StringUtilities.StringUtilities.HandleApostrophe(p_sBin) + "', ";
            sQuery += " '" + StringUtilities.StringUtilities.HandleApostrophe(p_sTaxYear) + "', ";
            sQuery += " '" + StringUtilities.StringUtilities.HandleApostrophe(p_sTerm) + "', ";
            sQuery += " '" + StringUtilities.StringUtilities.HandleApostrophe(p_sQtr) + "', ";
            sQuery += " '" + StringUtilities.StringUtilities.Left(StringUtilities.StringUtilities.HandleApostrophe(p_sFeesDesc), 40) + "', "; // ALJ 11202003 fixed SQL temporary solution
            sQuery += p_sFeesDue + ", ";
            sQuery += p_sFeesSurch + ", ";
            sQuery += p_sFeesPen + ", ";
            sQuery += p_sFeesTotal + ", ";
            sQuery += " '" + StringUtilities.StringUtilities.HandleApostrophe(p_sFeesCode) + "', ";
            sQuery += " '" + StringUtilities.StringUtilities.HandleApostrophe(p_sDueState) + "', ";
            sQuery += " '" + StringUtilities.StringUtilities.HandleApostrophe(p_sQtrToPay) + "', ";
            sQuery += " '" + StringUtilities.StringUtilities.HandleApostrophe(p_sFeesSort) + "')";
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

        private bool IsPaidOnLastQtr(String sBin, String sTaxYear, String sBnsCode, String sQtr)
        {
            bool bResult = false;
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from pay_hist where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and qtr_paid = '" + sQtr + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    bResult = true;
            pSet.Close();
            return bResult;
        }

        public string FeesWithPen(String sFeesCode)
        {
            OracleResultSet pSet = new OracleResultSet();

            String sQuery;
            String sFeesWithPen = "";

            try
            {
                if (sFeesCode.Substring(0, 1) == "B")
                {
                    sFeesWithPen = "Y";
                }
                else
                {
                    //pSet.Query = string.Format("select * from tax_and_fees_table where fees_code = '{0}'", sFeesCode); // CTS 09152003
                    pSet.Query = string.Format("select * from tax_and_fees_table where fees_code = '{0}' and rev_year = '{1}'", sFeesCode, AppSettingsManager.GetConfigValue("07")); // MCR 20141118 
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sFeesWithPen = pSet.GetString("fees_withpen").Trim();
                        }
                    pSet.Close();
                }
            }
            catch { }
            return sFeesWithPen;
        }

        public string FeesWithSurch(String sFeesCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            String sFeesWithSurch = "";

            try
            {
                if (sFeesCode.Substring(0, 1) == "B")
                {
                    sFeesWithSurch = "Y";
                }
                else
                {
                    //pSet.Query = string.Format("select * from tax_and_fees_table where fees_code = '{0}'", sFeesCode); // CTS 09152003
                    pSet.Query = string.Format("select * from tax_and_fees_table where fees_code = '{0}' and rev_year = '{1}'", sFeesCode, AppSettingsManager.GetConfigValue("07")); // MCR 20141118 
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sFeesWithSurch = pSet.GetString("fees_withsurch").Trim();
                        }
                    pSet.Close();
                }
            }
            catch { }
            return sFeesWithSurch;
        }

        private void DisplayDues(string sBin, string sQtrToPay, string sQtrToPay2, string sQtrToPay3, string sQtrToPay4)
        {
            if (sQtrToPay == "A") //MCR 20140603 adj
                dgvTaxFees.Rows.Clear();

            bool m_bExistTaxPU = false;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            m_dTotalTotalDue = 0;
            m_dTotalSurcharge = 0;
            m_dTotalPenalty = 0;
            m_dTotalDue = 0;

            m_dTotalDue1 = 0;
            m_dTotalSurcharge1 = 0;
            m_dTotalPenalty1 = 0;
            m_dTotalTotalDue1 = 0;
            string sRange1 = string.Empty;
            string sRange2 = string.Empty;

            if (m_sNextQtrToPay.Trim() == string.Empty)
                m_sNextQtrToPay = "1";

            if (sQtrToPay2.Trim() == string.Empty && sQtrToPay3.Trim() == string.Empty && sQtrToPay4.Trim() == string.Empty)
            {
                if (sQtrToPay == "F" && int.Parse(m_sNextQtrToPay) > 1 && txtStat.Text.Trim() != "NEW")
                    sQtrToPay = string.Empty;
                if (sQtrToPay == "A" || sQtrToPay == "X")
                {
                    result.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and due_state = '" + sQtrToPay.Trim() + "'  order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    if (sQtrToPay == "X")
                        //result.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and due_state = 'X' and qtr_to_pay = 'F'  order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                        result.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and due_state = 'X' and (qtr_to_pay = 'F' or qtr_to_pay = '1')  order by tax_year,qtr_to_pay,qtr,fees_code_sort";  // RMC 20140708 added retirement billing
                }
                else
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'X' and due_state <> 'A' and qtr_to_pay = '" + sQtrToPay.Trim() + "' order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                sRange1 = "1";
            }
            else
            {
                sRange1 = "1";
                if (sQtrToPay2 != "" && sQtrToPay3 == "" && sQtrToPay4 == "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('" + sQtrToPay.Trim() + "','" + sQtrToPay2.Trim() + "') order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange2 = "2";
                }
                if (sQtrToPay2 != "" && sQtrToPay3 != "" && sQtrToPay4 == "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('" + sQtrToPay.Trim() + "','" + sQtrToPay2.Trim() + "','" + sQtrToPay3.Trim() + "') order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange2 = "3";
                }
                if (sQtrToPay2 != "" && sQtrToPay3 != "" && sQtrToPay4 != "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('" + sQtrToPay.Trim() + "','" + sQtrToPay2.Trim() + "','" + sQtrToPay3.Trim() + "','" + sQtrToPay4.Trim() + "') order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange2 = "4";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 != "" && sQtrToPay4 == "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay  = '" + sQtrToPay3.Trim() + "' order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange1 = "3";
                    sRange2 = "";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 != "" && sQtrToPay4 != "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('" + sQtrToPay3.Trim() + "','" + sQtrToPay4.Trim() + "') order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange1 = "3";
                    sRange2 = "4";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 == "" && sQtrToPay4 != "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay = '" + sQtrToPay4.Trim() + "' order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange1 = "4";
                    sRange2 = "";
                }

            }

            // codes for discount

            if (result.Execute())
            {
                while (result.Read())
                {
                    m_bExistTaxDue = true;
                    p_bin = result.GetString("bin").Trim();
                    p_tax_year = result.GetString("tax_year").Trim();
                    p_term = result.GetString("term").Trim();
                    p_qtr = result.GetString("qtr").Trim();

                    if (p_term == "I")
                        p_term = "Q";

                    if (p_qtr == "F")
                        p_qtr = "Y";

                    p_fees_desc = result.GetString("fees_desc").Trim();
                    p_fees_due = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_due").ToString()));
                    p_fees_surch = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_surch").ToString()));
                    p_fees_pen = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_pen").ToString()));
                    p_fees_totaldue = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_totaldue").ToString()));
                    p_fees_code = result.GetString("fees_code").Trim();
                    p_due_state = result.GetString("due_state").Trim();
                    p_qtr_to_pay = result.GetString("qtr_to_pay").Trim();

                    double.TryParse(p_fees_due, out m_dTotalDue);
                    double.TryParse(p_fees_surch, out m_dTotalSurcharge);
                    double.TryParse(p_fees_pen, out m_dTotalPenalty);
                    double.TryParse(p_fees_totaldue, out m_dTotalTotalDue);

                    m_dTotalDue1 = m_dTotalDue1 + m_dTotalDue;
                    m_dTotalSurcharge1 = m_dTotalSurcharge1 + m_dTotalSurcharge;
                    m_dTotalPenalty1 = m_dTotalPenalty1 + m_dTotalPenalty;
                    m_dTotalTotalDue1 = m_dTotalTotalDue1 + m_dTotalTotalDue;

                    // GDE 20101103 for decimal places (s){
                    double.TryParse(p_fees_due.ToString(), out dDue);
                    p_fees_due = string.Format("{0:0.00}", (double)dDue);
                    // GDE 20101103 for decimal places (e)}

                    dgvTaxFees.Rows.Add(p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);

                }
            }
            result.Close();

            // permit update codes

            if (m_bExistTaxPU == false)
            {
                result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state = 'P' and qtr_to_pay = 'F' order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        m_bExistTaxPU = true;
                        p_bin = result.GetString("bin").Trim();
                        p_tax_year = result.GetString("tax_year").Trim();
                        p_term = result.GetString("term").Trim();
                        p_qtr = result.GetString("qtr").Trim();
                        if (p_term == "I")
                            p_term = "Q";
                        if (p_qtr == "F")
                            p_qtr = "Y";

                        p_fees_desc = result.GetString("fees_desc").Trim();
                        p_fees_due = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_due").ToString()));
                        p_fees_surch = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_surch").ToString()));
                        p_fees_pen = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_pen").ToString()));
                        p_fees_totaldue = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_totaldue").ToString()));
                        p_fees_code = result.GetString("fees_code").Trim();
                        p_due_state = result.GetString("due_state").Trim();
                        p_qtr_to_pay = result.GetString("qtr_to_pay").Trim();

                        double.TryParse(p_fees_due, out m_dTotalDue);
                        double.TryParse(p_fees_surch, out m_dTotalSurcharge);
                        double.TryParse(p_fees_pen, out m_dTotalPenalty);
                        double.TryParse(p_fees_totaldue, out m_dTotalTotalDue);

                        m_dTotalDue1 = m_dTotalDue1 + m_dTotalDue;
                        m_dTotalSurcharge1 = m_dTotalSurcharge1 + m_dTotalSurcharge;
                        m_dTotalPenalty1 = m_dTotalPenalty1 + m_dTotalPenalty;
                        m_dTotalTotalDue1 = m_dTotalTotalDue1 + m_dTotalTotalDue;

                        // GDE 20101103 for decimal places (s){
                        double.TryParse(p_fees_due.ToString(), out dDue);
                        p_fees_due = string.Format("{0:0.00}", (double)dDue);
                        // GDE 20101103 for decimal places (e)}

                        dgvTaxFees.Rows.Add(p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);
                    }
                }
                result.Close();
            }




            //(s) RTL 11302005 for installment in NEW

            result.Query = "select * from  pay_hist where bin = '" + sBin.Trim() + "' and bns_stat = 'NEW'";
            if (result.Execute()/* insert condition for qtrly dec of new */)
            {
                if (sQtrToPay != "F" && sQtrToPay != "1" && txtStat.Text.Trim() == "NEW")
                {
                    result2.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state = 'N' and qtr_to_pay = 'F' and fees_code not like 'B%' order by fees_code_sort";
                    if (result2.Execute())
                    {
                        while (result2.Read())
                        {
                            p_bin = result.GetString("bin").Trim();
                            p_tax_year = result.GetString("tax_year").Trim();
                            p_term = result.GetString("term").Trim();
                            p_qtr = result.GetString("qtr").Trim();
                            if (p_term == "I")
                                p_term = "Q";
                            if (p_qtr == "F")
                                p_qtr = "Y";

                            p_fees_desc = result.GetString("fees_desc").Trim();
                            p_fees_due = result.GetDouble("fees_due").ToString();
                            p_fees_surch = result.GetDouble("fees_surch").ToString();
                            p_fees_pen = result.GetDouble("fees_pen").ToString();
                            p_fees_totaldue = result.GetDouble("fees_totaldue").ToString();
                            p_fees_code = result.GetString("fees_code").Trim();
                            p_due_state = result.GetString("due_state").Trim();
                            p_qtr_to_pay = result.GetString("qtr_to_pay").Trim();

                            //                    dgvTaxFees.Rows.Add(p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);

                            double.TryParse(p_fees_due, out m_dTotalDue);
                            double.TryParse(p_fees_surch, out m_dTotalSurcharge);
                            double.TryParse(p_fees_pen, out m_dTotalPenalty);
                            double.TryParse(p_fees_totaldue, out m_dTotalTotalDue);

                            m_dTotalDue1 = m_dTotalDue1 + m_dTotalDue;
                            m_dTotalSurcharge1 = m_dTotalSurcharge1 + m_dTotalSurcharge;
                            m_dTotalPenalty1 = m_dTotalPenalty1 + m_dTotalPenalty;
                            m_dTotalTotalDue1 = m_dTotalTotalDue1 + m_dTotalTotalDue;

                            // GDE 20101103 for decimal places (s){
                            double.TryParse(p_fees_due.ToString(), out dDue);
                            p_fees_due = string.Format("{0:0.00}", (double)dDue);

                            // GDE 20101103 for decimal places (e)}

                            dgvTaxFees.Rows.Add(p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);
                        }
                    }
                    result2.Close();

                }
            }
            result.Close();
            //(e) RTL 11302005 for installment in NEW

            // Permit Update Transaction Code Here

            // For Installment New codes here

            // Compute Total Codes Here

            //REM MCR 20142905 (s)
            //txtTotDue.Text = string.Format("{0:#,##0.00}", m_dTotalDue1);
            //txtTotSurch.Text = string.Format("{0:#,##0.00}", m_dTotalSurcharge1);
            //txtTotPen.Text = string.Format("{0:#,##0.00}", m_dTotalPenalty1);
            //txtTotTotDue.Text = string.Format("{0:#,##0.00}", m_dTotalTotalDue1);
            //REM MCR 20142905 (e)

            // controlling control codes here
            // MCR 20140603 (s)
            if (dgvTaxFees.Rows.Count >= 1)
                dgvTaxFees.Rows[0].Selected = true;
            dgvTaxFees.ClearSelection();
            // MCR 20140603 (e)
        }

        private void DisplayDuesNew(string sBin, string sQtrToPay, string sQtrToPay2, string sQtrToPay3, string sQtrToPay4)
        {
            // RMC 20150112 mods in retirement billing+
            //if (sQtrToPay == "X")   // RMC 20150108
            //   dgvTaxFees.Rows.Clear();
            // RMC 20150112 mods in retirement billing, put rem

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            String sQuery = "";
            String sRange1 = "", sRange2 = "";

            String sQtrPaid = "";

            if (sQtrToPay != "A" && sQtrToPay != "F" && m_iMultiTaxYear == 1 && sQtrToPay != "X") // MCR 20143005 include Adj
            //if (sQtrToPay != "A" && sQtrToPay != "F" && sQtrToPay != "X") // MCR 20143005 include Adj
            {

                // AST 20160112 (s)
                //sQuery = "select qtr_paid from pay_hist where payment_term = 'I' and bin = '" + sBin + "' order by tax_year desc, qtr_paid desc";
                //sQuery = "select qtr_paid from pay_hist where payment_term = 'I' and bin = '" + sBin + "' and tax_year = '" + txtTaxYear.Text + "' ";
                //sQuery += "order by tax_year desc, qtr_paid desc";
                // AST 20160112 (e)

                // AST 20160201 merge (s)

                //sQuery = "select qtr_paid from pay_hist where payment_term = 'I' and bin = '" + sBin + "' order by tax_year desc, qtr_paid desc";

                // RMC 20160113 corrections in SOA error if Installemnt (s)
                //sQuery = "select qtr_paid from pay_hist where payment_term = 'I' and bin = '" + sBin + "' ";
                sQuery = "select qtr_paid from pay_hist where (payment_term = 'I' or payment_term <> 'I') and bin = '" + sBin + "' ";   // RMC 20180117 correction in SOA and Payment
                sQuery += " and (qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'Y' and qtr_paid <> 'X')";
                sQuery += " and tax_year >= (select max(tax_year) from pay_hist where bin = '" + sBin + "') order by tax_year desc, qtr_paid desc";
                // RMC 20160113 corrections in SOA error if Installemnt (e)

                // AST 20160201 merge (e)


                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sQtrPaid = pSet.GetString("qtr_paid");
                        // RMC 20180117 correction in SOA and Payment (s)
                        if (sQtrPaid == "F")
                            sQtrPaid = "4";
                        // RMC 20180117 correction in SOA and Payment (e)
                        if (Convert.ToInt16(sQtrPaid) == 1)
                            sQtrToPay = "";
                        if (Convert.ToInt16(sQtrPaid) == 2)
                        {
                            sQtrToPay = "";
                            sQtrToPay2 = "";
                        }
                        if (Convert.ToInt16(sQtrPaid) == 3)
                        {
                            sQtrToPay = "";
                            sQtrToPay2 = "";
                            sQtrToPay3 = "";
                        }
                    }
                pSet.Close();
            }

            PayTempStruct p;
            p.due_state = "";
            p.qtr_to_pay = "";
            m_bExistTaxDue = false;

            if (sQtrToPay2 == "" && sQtrToPay3 == "" && sQtrToPay4 == "")
            {
                //if (sQtrToPay == "F" && Convert.ToInt16(m_sNextQtrToPay) > 1 && txtStat.Text != "NEW" && m_iMultiTaxYear == 1)
                if (m_sNextQtrToPay == "")
                    m_sNextQtrToPay = "0";
                if (sQtrToPay == "F" && Convert.ToInt16(m_sNextQtrToPay) > 1 && txtStat.Text != "NEW")
                    sQtrToPay = "";

                if (sQtrToPay == "A" || sQtrToPay == "X")
                {
                    if (sQtrToPay == "X")
                        //sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state = 'X' and qtr_to_pay = 'F' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);
                        //sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state = 'X' and (qtr_to_pay = 'F' or qtr_to_pay = '1') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);    // RMC 20150108
                        //sQuery = string.Format("select * from pay_temp where bin = '{0}' and ((due_state = 'X' and (qtr_to_pay = 'F' or qtr_to_pay = '1')) or (due_state <> 'X' and qtr_to_pay = 'F')) order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);  // RMC 20170119 correction in SOA of RET with previous year delinq
                    //    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state = 'X' and (qtr_to_pay = 'F' or qtr_to_pay = '1') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);    // RMC 20170228 correction in SOA of RET
                        sQuery = string.Format("select distinct a.* from pay_temp a where bin = '{0}' and due_state = 'X' and (qtr_to_pay = 'F' or qtr_to_pay = '1') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);    // RMC 20180111 CORRECTION IN SOA DOUBLE DISPLAY
                    else
                        sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state = '{1}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay);
                }
                else
                {
                    // RMC 20180108 correction in SOA for current year installment and with prev year delinq (s)
                    if (m_iMultiTaxYear > 1)
                    {
                        sQuery = "select * from pay_temp where bin = '" + sBin + "' and due_state <> 'P' and due_state <> 'X' and due_state <> 'A' ";
                        sQuery += " and ((qtr_to_pay = '" + sQtrToPay + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "') or ";
                        sQuery += " (qtr_to_pay = 'F' and tax_year < '" + ConfigurationAttributes.CurrentYear + "'))";

                    }// RMC 20180108 correction in SOA for current year installment and with prev year delinq (e)
                    else
                    {
                        sQuery = "select * from pay_temp where bin = '" + sBin + "' and due_state <> 'P' and due_state <> 'X' and due_state <> 'A' and qtr_to_pay = '" + sQtrToPay + "' ";
                        if (sBin == "192-00-2011-0001708")
                            sQuery += " and qtr <> '4' ";

                    }
                    sQuery += " order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                }
                sRange1 = "1";
            }
            else
            {
                sRange1 = "1";
                if (sQtrToPay2 != "" && sQtrToPay3 == "" && sQtrToPay4 == "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('{1}','{2}') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay, sQtrToPay2);
                    sRange2 = "2";
                }
                if (sQtrToPay2 != "" && sQtrToPay3 != "" && sQtrToPay4 == "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('{1}','{2}','{3}') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay, sQtrToPay2, sQtrToPay3);
                    sRange2 = "3";
                }
                if (sQtrToPay2 != "" && sQtrToPay3 != "" && sQtrToPay4 != "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('{1}','{2}','{3}','{4}') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay, sQtrToPay2, sQtrToPay3, sQtrToPay4);
                    sRange2 = "4";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 != "" && sQtrToPay4 == "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('{1}') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay3);
                    sRange1 = "3";
                    sRange2 = "";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 != "" && sQtrToPay4 != "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('{1}','{2}') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay3, sQtrToPay4);
                    sRange1 = "3";
                    sRange2 = "4";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 == "" && sQtrToPay4 != "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay = '{1}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay4);
                    sRange1 = "4";
                    sRange2 = "";
                }

                // RMC 20180108 correction in SOA for current year installment and with prev year delinq (s)
                // this will disregard query above
                //note: only current year can be paid in installment
                sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and ",sBin);
                sQuery += string.Format(" ((qtr_to_pay in ('{0}','{1}','{2}','{3}') and tax_year = '" + ConfigurationAttributes.CurrentYear + "') or ", sQtrToPay, sQtrToPay2, sQtrToPay3, sQtrToPay4);
                sQuery += " (qtr_to_pay = 'F' and tax_year < '" + ConfigurationAttributes.CurrentYear + "'))";
                sQuery += " order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                // RMC 20180108 correction in SOA for current year installment and with prev year delinq (e) 


            }

            //double dTotalDiscountAmount;
            //dTotalDiscountAmount = 0;
            //int iSwDiscounted = 0;

            /* Discount
            if (sQtrToPay == "F" &&  AppSettingsManager.OnCheckIfDiscounted(m_sORDate) == true && txtStat.Text.Trim() != "NEW")	// JJP 12142004 FIXED BUGS IN PRACTICE SESSIONS - DISCOUNT // ALJ 02142005 change function definition/declaration to accomodate OFL payment
                iSwDiscounted = 1;
            */

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    m_bExistTaxDue = true;
                    p.bin = pSet.GetString("bin");
                    p.tax_year = pSet.GetString("tax_year");
                    p.term = pSet.GetString("term");
                    if (p.term == "I")
                        p.term = "Q";
                    p.qtr = pSet.GetString("qtr");
                    if (p.qtr == "F")
                        p.qtr = "Y";
                    p.fees_desc = pSet.GetString("fees_desc");
                    /*p.fees_due = pSet.GetDouble("fees_due").ToString("##0.00");
                    p.fees_surch = pSet.GetDouble("fees_surch").ToString("##0.00");
                    p.fees_pen = pSet.GetDouble("fees_pen").ToString("##0.00");
                    p.fees_totaldue = pSet.GetDouble("fees_totaldue").ToString("##0.00");*/

                    // RMC 20170109 fix .01 difference in rounding off amount in SOA and Online payment (s)
                    p.fees_due = pSet.GetDouble("fees_due").ToString("##0.000");
                    p.fees_surch = pSet.GetDouble("fees_surch").ToString("##0.000");
                    p.fees_pen = pSet.GetDouble("fees_pen").ToString("##0.000");
                    p.fees_totaldue = pSet.GetDouble("fees_totaldue").ToString("##0.000");
                    // RMC 20170109 fix .01 difference in rounding off amount in SOA and Online payment (e)

                    p.fees_code = pSet.GetString("fees_code");
                    p.due_state = pSet.GetString("due_state");
                    p.qtr_to_pay = pSet.GetString("qtr_to_pay");

                    p.fees_desc = AppSettingsManager.EnhanceFeesDesc(sQuery, p.bin, p.fees_desc, p.fees_code); // AST 20160115

                    dgvTaxFees.Rows.Add(p.tax_year, p.term, p.qtr, p.fees_desc, p.fees_due, p.fees_surch, p.fees_pen, p.fees_totaldue, p.due_state, p.qtr_to_pay, p.fees_code);
                }
            }
            pSet.Close();

            if (m_bExistTaxPU == false)
            {
                // RMC 20170216 added btax qtrly payment if with permit-update transaction (s)
                if(AppSettingsManager.GetConfigValue("10") == "243")
                {
                    // RMC 20170404 additional validation in SOA and Payment if with new addl bns (s)
                    if (ValidatePUAddlBns(sBin))
                    {
                        sQtrToPay = "";
                        sQtrToPay2 = "";
                        sQtrToPay3 = "";
                        sQtrToPay4 = "";
                    }
                    // RMC 20170404 additional validation in SOA and Payment if with new addl bns (e)

                    sQuery = "select * from  pay_temp where bin = '"+sBin+"' and due_state = 'P' ";
                    //sQuery += " and qtr_to_pay in ('"+sQtrToPay+"','"+sQtrToPay2+"','"+sQtrToPay3+"','"+sQtrToPay4+"') ";
                    //sQuery += " and qtr_to_pay in ('" + sQtrToPay + "','" + sQtrToPay2 + "','" + sQtrToPay3 + "','" + sQtrToPay4 + "','F') ";   // RMC 20170223 corrected in SOA permit update transaction not displayed   

                    // RMC 20170224 corrected error in SOA of permit-update (s)
                    if (sQtrToPay == "" && sQtrToPay2 == "" && sQtrToPay3 == "" && sQtrToPay4 == "")
                        sQuery += " and qtr_to_pay in ('F') ";
                    else
                        sQuery += " and qtr_to_pay in ('" + sQtrToPay + "','" + sQtrToPay2 + "','" + sQtrToPay3 + "','" + sQtrToPay4 + "') ";
                    // RMC 20170224 corrected error in SOA of permit-update (e)

                    sQuery += " order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                }
                else// RMC 20170216 added btax qtrly payment if with permit-update transaction (e)
                    sQuery = string.Format("select  distinct * from  pay_temp where bin = '{0}' and due_state = 'P' and qtr_to_pay = 'F' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);
                
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        m_bExistTaxPU = true;
                        //iSwDiscounted = 0;
                        //chkFull.Enabled = true;

                        p.bin = pSet.GetString("bin");
                        p.tax_year = pSet.GetString("tax_year");
                        p.term = pSet.GetString("term");
                        if (p.term == "I")
                            p.term = "Q";
                        p.qtr = pSet.GetString("qtr");
                        if (p.qtr == "F")
                            p.qtr = "Y";
                        p.fees_desc = pSet.GetString("fees_desc");
                        /*p.fees_due = pSet.GetDouble("fees_due").ToString("##0.00");
                        p.fees_surch = pSet.GetDouble("fees_surch").ToString("##0.00");
                        p.fees_pen = pSet.GetDouble("fees_pen").ToString("##0.00");
                        p.fees_totaldue = pSet.GetDouble("fees_totaldue").ToString("##0.00");*/

                        // RMC 20170109 fix .01 difference in rounding off amount in SOA and Online payment (s)
                        p.fees_due = pSet.GetDouble("fees_due").ToString("##0.000");
                        p.fees_surch = pSet.GetDouble("fees_surch").ToString("##0.000");
                        p.fees_pen = pSet.GetDouble("fees_pen").ToString("##0.000");
                        p.fees_totaldue = pSet.GetDouble("fees_totaldue").ToString("##0.000");
                        // RMC 20170109 fix .01 difference in rounding off amount in SOA and Online payment (e)
                        p.fees_code = pSet.GetString("fees_code");
                        p.due_state = pSet.GetString("due_state");
                        p.qtr_to_pay = pSet.GetString("qtr_to_pay");

                        dgvTaxFees.Rows.Add(p.tax_year, p.term, p.qtr, p.fees_desc, p.fees_due, p.fees_surch, p.fees_pen, p.fees_totaldue, p.due_state, p.qtr_to_pay, p.fees_code);
                    }

                }
                pSet.Close();
            }

            //if (m_bIsDisCompromise == true || m_sCompPaySw == "N")
            //    OnCompromise(sBin, sRange1, sRange2);

            sQuery = string.Format("select * from  pay_hist where bin = '{0}' and bns_stat = 'NEW'", sBin);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (!pSet.Read() && AppSettingsManager.GetConfigValue("15").ToUpper() == "Y")
                {
                    if (sQtrToPay != "F" && sQtrToPay != "1" && m_sStatus == "NEW")
                    {
                        sQuery = string.Format("select * from  pay_temp where bin = '{0}' and due_state = 'N' and qtr_to_pay = 'F' and fees_code not like 'B%%' order by fees_code_sort", sBin);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            while (pSet1.Read())
                            {
                                p.bin = pSet.GetString("bin");
                                p.tax_year = pSet.GetString("tax_year");
                                p.term = pSet.GetString("term");
                                p.qtr = pSet.GetString("qtr");
                                p.fees_desc = pSet.GetString("fees_desc");
                                /*p.fees_due = pSet.GetDouble("fees_due").ToString("##0.00");
                                p.fees_surch = pSet.GetDouble("fees_surch").ToString("##0.00");
                                p.fees_pen = pSet.GetDouble("fees_pen").ToString("##0.00");
                                p.fees_totaldue = pSet.GetDouble("fees_totaldue").ToString("##0.00");*/

                                // RMC 20170109 fix .01 difference in rounding off amount in SOA and Online payment (s)
                                p.fees_due = pSet.GetDouble("fees_due").ToString("##0.000");
                                p.fees_surch = pSet.GetDouble("fees_surch").ToString("##0.000");
                                p.fees_pen = pSet.GetDouble("fees_pen").ToString("##0.000");
                                p.fees_totaldue = pSet.GetDouble("fees_totaldue").ToString("##0.000");
                                // RMC 20170109 fix .01 difference in rounding off amount in SOA and Online payment (e)
                                p.fees_code = pSet.GetString("fees_code");
                                p.due_state = pSet.GetString("due_state");
                                p.qtr_to_pay = pSet.GetString("qtr_to_pay");

                                dgvTaxFees.Rows.Add(p.tax_year, p.term, p.qtr, p.fees_desc, p.fees_surch, p.fees_pen, p.fees_totaldue, p.qtr_to_pay, p.fees_code);
                            }
                        }
                        pSet1.Close();
                    }
                }
            pSet.Close();


            ComputeTotal();
            //SetControls(true);

            if (p.qtr_to_pay != "1" && m_iMultiTaxYear == 1)
            //if (p.qtr_to_pay != "1")
            {
                chkFull.Enabled = false;
                if (p.qtr_to_pay != "F")
                    chkQtr.Enabled = true;
                else
                    chkQtr.Enabled = false;
            }
            if (m_iMultiTaxYear != 1 && Convert.ToInt16(txtTaxYear.Text) < AppSettingsManager.GetSystemDate().Year)
            //if (m_iMultiTaxYear != 1 && Convert.ToInt16(txtTaxYear.Text) < Convert.ToInt16(m_sCurrentTaxYear))
            //if (Convert.ToInt16(txtTaxYear.Text) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")))
            {
                chkFull.Enabled = true;
                chkQtr.Enabled = false;
            }
            if (p.due_state == "N" || p.due_state == "Q")
            {
                chkFull.Enabled = true;
                chkQtr.Enabled = false;
            }

            //if (m_bEnableTaxPU == true || txtStat.Text == "NEW")
            //{
            //    chkFull.Enabled = true;
            //    chkQtr.Enabled = false;
            //    if (m_bEnableTaxDue == true && txtStat.Text != "NEW")
            //        chkQtr.Enabled = true;
            //}

            //mv_sGrandTotalDue = mv_sDiscountedTotal;
            //DeductTaxCredit();
            //CompLessCredit(mv_sDiscountedTotal, m_sCreditLeft, mv_sToBeCredited, &mv_sGrandTotalDue, &mv_sCreditLeft);

        }

        private void ComputeTaxAndFeesNew(string sBin)
        {
            m_sRevYear = AppSettingsManager.GetConfigObject("07");
            m_sIsQtrly = "N";
            DateTime dtSystemDate = DateTime.Now;
            DateTime dtDateOperated = DateTime.Now;
            string sQtrToPay = string.Empty;
            string sBnsCodeMain, sOldTaxYear, sNewtaxYear, sTaxYear;
            string sCurrentYear, sToday, sDateOperated = string.Empty, sYearOperated = string.Empty;
            dtSystemDate = AppSettingsManager.GetSystemDate();
            sToday = string.Format("{0:MM/dd/yyyy}", dtSystemDate);
            sCurrentYear = ConfigurationAttributes.CurrentYear;
            
            TaxDuesStruct t;
            PayTempStruct p;
            PenRate pr;

            pr.p_dSurchRate1 = 0;
            pr.p_dSurchRate2 = 0;
            pr.p_dSurchRate3 = 0;
            pr.p_dSurchRate4 = 0;
            pr.p_dPenRate1 = 0;
            pr.p_dPenRate2 = 0;
            pr.p_dPenRate3 = 0;
            pr.p_dPenRate4 = 0;
            pr.p_SurchQuart = 0;
            pr.p_PenQuart = 0;

            if (txtStat.Text.Trim() == "REN")
                m_sIsQtrly = "Y";

            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            result.Query = "select count(distinct tax_year) as iCount from taxdues where bin = '" + sBin + "'";
            if (result.Execute())
                if (result.Read())
                    m_iMultiTaxYear = result.GetInt("iCount");
            result.Close();

            if (m_iMultiTaxYear > 1)
                txtTaxYear.ReadOnly = false;
            else
                txtTaxYear.ReadOnly = true;

            result.Query = "select * from businesses where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    dtDateOperated = result.GetDateTime("dt_operated");
                    sDateOperated = string.Format("{0:MM/dd/yyyy}", dtDateOperated);
                    sYearOperated = sDateOperated.Substring(6, 4);
                }
                else
                {
                    result.Close();
                    result.Query = "select * from business_que where bin = '" + sBin + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            dtDateOperated = result.GetDateTime("dt_operated");
                            sDateOperated = string.Format("{0:MM/dd/yyyy}", dtDateOperated);
                            sYearOperated = sDateOperated.Substring(6, 4);
                        }
                    }
                }
            }
            result.Close();

            // try
            /*
            if (txtStat.Text.Trim() == "RET")
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'X' order by tax_year asc";
            else
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' order by tax_year DESC";
            if (result.Execute())
            {
                if (result.Read())
                {
                    txtTaxYear.Text = result.GetString("tax_year").Trim();
                }
            }
            result.Close();
             */
            //try

            if (txtStat.Text.Trim() == "RET")   // RMC 20150108
                m_sNextQtrToPay = result.GetString("QTR_TO_PAY");

            result.Query = "delete from pay_temp where bin = '" + sBin.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            result.Query = "delete from pay_temp_bns where bin = '" + sBin.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "compute_tax_and_fees";
            plsqlCmd.ParamValue = sBin;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sToday;
            plsqlCmd.AddParameter("m_sOrDate", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sCurrentYear;
            plsqlCmd.AddParameter("m_sCurrentTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtStat.Text;
            plsqlCmd.AddParameter("m_sStatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtTaxYear.Text;
            plsqlCmd.AddParameter("m_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_iMultiTaxYear;
            plsqlCmd.AddParameter("iMultiTaxYear", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sRevYear;
            plsqlCmd.AddParameter("m_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sDateOperated;
            plsqlCmd.AddParameter("m_sDateOperated", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sYearOperated;
            plsqlCmd.AddParameter("m_sYearOperated", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sIsQtrly;
            plsqlCmd.AddParameter("m_sQtrly", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sNextQtrToPay;
            plsqlCmd.AddParameter("m_sNxtQtrToPay", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Output);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();

            }
            plsqlCmd.Close();
            m_sQtr1 = "F";
            m_sTerm = "F";


            // for displaying dues
            if (txtStat.Text.Trim() == "RET")
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'X' and tax_year <= '" + txtTaxYear.Text.Trim() + "'";
            else
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state <> 'P' and tax_year <= '" + txtTaxYear.Text.Trim() + "' order by tax_year desc ,qtr_to_pay desc ,due_state desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sQtrToPay = result.GetString("qtr_to_pay").Trim();
                    if (sQtrToPay != "1" && m_iMultiTaxYear != 1)
                    {
                        m_sNextQtrToPay = sQtrToPay;
                        OnInstallment(sBin);
                    }
                    else
                        if (sQtrToPay != "1" && m_iNextQtrToPay != 1)
                        {
                            result2.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'A'  and bin in (select bin from taxdues where due_state = 'X')";
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    m_sNextQtrToPay = sQtrToPay;
                                    OnInstallment(sBin);
                                }
                            }
                            result2.Close();
                        }
                        else
                        {
                            m_sNextQtrToPay = sQtrToPay;
                            btn1st.Checked = false;
                            btn2nd.Checked = false;
                            btn3rd.Checked = false;
                            btn4th.Checked = false;
                            chkFull.Checked = true;
                            chkQtr.Checked = false;
                            DisplayDuesNew(sBin, "F", "", "", "");
                        }

                    if (m_iMultiTaxYear != 1 && int.Parse(txtTaxYear.Text.Trim()) <= AppSettingsManager.GetCurrentDate().Year)
                    {
                        m_sNextQtrToPay = sQtrToPay;
                        btn1st.Checked = false;
                        btn2nd.Checked = false;
                        btn3rd.Checked = false;
                        btn4th.Checked = false;
                        chkFull.Checked = true;
                        chkQtr.Checked = false;
                        DisplayDuesNew(sBin, "F", "", "", "");
                    }
                    else
                    {
                        if (int.Parse(txtTaxYear.Text.Trim()) < AppSettingsManager.GetCurrentDate().Year)
                        {
                            m_sNextQtrToPay = sQtrToPay;
                            btn1st.Checked = false;
                            btn2nd.Checked = false;
                            btn3rd.Checked = false;
                            btn4th.Checked = false;
                            chkFull.Checked = true;
                            chkQtr.Checked = false;
                            DisplayDuesNew(sBin, "F", "", "", "");
                            m_sTerm = "F";
                        }
                        else
                        {
                            if (txtStat.Text != "NEW" && sQtrToPay != "1")
                            {
                                m_sNextQtrToPay = sQtrToPay;
                                chkFull.Checked = false;
                                chkQtr.Checked = true;
                                OnInstallment(sBin);
                            }
                            else
                            {
                                btn1st.Checked = false;
                                btn2nd.Checked = false;
                                btn3rd.Checked = false;
                                btn4th.Checked = false;
                                chkFull.Checked = true;
                                chkQtr.Checked = false;
                                OnFull(); // GDE 20110412 added for testing
                            }
                        }
                    }
                }

                // GDE 20110414 added for testing (s){
                else
                {
                    result.Close();
                    result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'P' and tax_year = '" + txtTaxYear.Text.Trim() + "' order by tax_year desc ,qtr_to_pay desc ,due_state desc";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sQtrToPay = result.GetString("qtr_to_pay").Trim();
                            if (sQtrToPay != "1" && m_iMultiTaxYear != 1)
                            {
                                m_sNextQtrToPay = sQtrToPay;
                                OnInstallment(sBin);
                            }
                            else
                                if (sQtrToPay != "1" && m_iNextQtrToPay != 1)
                                {
                                    result2.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'A'  and bin in (select bin from taxdues where due_state = 'X')";
                                    if (result2.Execute())
                                    {
                                        if (result2.Read())
                                        {
                                            m_sNextQtrToPay = sQtrToPay;
                                            OnInstallment(sBin);
                                        }
                                    }
                                    result2.Close();
                                }
                                else
                                {
                                    m_sNextQtrToPay = sQtrToPay;
                                    btn1st.Checked = false;
                                    btn2nd.Checked = false;
                                    btn3rd.Checked = false;
                                    btn4th.Checked = false;
                                    chkFull.Checked = true;
                                    chkQtr.Checked = false;
                                    DisplayDuesNew(sBin, "F", "", "", "");
                                }

                            if (m_iMultiTaxYear != 1 && int.Parse(txtTaxYear.Text.Trim()) <= AppSettingsManager.GetCurrentDate().Year)
                            {
                                m_sNextQtrToPay = sQtrToPay;
                                btn1st.Checked = false;
                                btn2nd.Checked = false;
                                btn3rd.Checked = false;
                                btn4th.Checked = false;
                                chkFull.Checked = true;
                                chkQtr.Checked = false;
                                DisplayDuesNew(sBin, "F", "", "", "");
                            }
                            else
                            {
                                if (int.Parse(txtTaxYear.Text.Trim()) < AppSettingsManager.GetCurrentDate().Year)
                                {
                                    m_sNextQtrToPay = sQtrToPay;
                                    btn1st.Checked = false;
                                    btn2nd.Checked = false;
                                    btn3rd.Checked = false;
                                    btn4th.Checked = false;
                                    chkFull.Checked = true;
                                    chkQtr.Checked = false;
                                    DisplayDuesNew(sBin, "F", "", "", "");
                                    m_sTerm = "F";
                                }
                                else
                                {
                                    if (txtStat.Text != "NEW" && sQtrToPay != "1")
                                    {
                                        m_sNextQtrToPay = sQtrToPay;
                                        chkFull.Checked = false;
                                        chkQtr.Checked = true;
                                        OnInstallment(sBin);
                                    }
                                    else
                                    {
                                        btn1st.Checked = false;
                                        btn2nd.Checked = false;
                                        btn3rd.Checked = false;
                                        btn4th.Checked = false;
                                        chkFull.Checked = true;
                                        chkQtr.Checked = false;
                                        OnFull(); // GDE 20110412 added for testing
                                    }
                                }
                            }
                        }
                    }
                }
                // GDE 20110414 added for testing (e)}
            }
            result.Close();
        } //MOD MCR 20140227/20140722

        private void ComputeTaxAndFees(string sBin)
        {
            m_sRevYear = AppSettingsManager.GetConfigObject("07");
            m_sIsQtrly = "N";
            DateTime dtSystemDate = DateTime.Now;
            DateTime dtDateOperated = DateTime.Now;
            string sQtrToPay = string.Empty;
            string sDueState = string.Empty;
            string sCurrentYear, sToday, sDateOperated = string.Empty, sYearOperated = string.Empty;
            dtSystemDate = AppSettingsManager.GetSystemDate();
            sToday = string.Format("{0:MM/dd/yyyy}", dtSystemDate);
            //sCurrentYear = sToday.Substring(6, 4);
            sCurrentYear = ConfigurationAttributes.CurrentYear;

            if (txtStat.Text.Trim() == "REN")
                m_sIsQtrly = "Y";

            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            result.Query = "select count(distinct tax_year) as iCount from taxdues where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_iMultiTaxYear = result.GetInt("iCount");
                }
            }
            result.Close();

            if (m_iMultiTaxYear > 1)
                txtTaxYear.ReadOnly = false;
            else
                txtTaxYear.ReadOnly = true;

            // insert permit update transaction here

            bool blnIsSplBns = false;
            result.Query = "select * from spl_business_que where bin  = :1";
            result.AddParameter(":1", sBin);
            if (result.Execute())
            {
                if (result.Read())
                {
                    blnIsSplBns = true;
                }
                else
                {
                    result.Close();
                    result.Query = "select * from spl_businesses where bin  = :1";
                    result.AddParameter(":1", sBin);
                    if (result.Execute())
                    {
                        if (result.Read())
                        { blnIsSplBns = true; }
                    }
                }
            }
            result.Close();



            if (blnIsSplBns)
                result.Query = "select * from spl_business_que where bin = '" + sBin + "'";
            else
                result.Query = "select * from business_que where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    dtDateOperated = result.GetDateTime("dt_operated");
                    sDateOperated = string.Format("{0:MM/dd/yyyy}", dtDateOperated);
                    sYearOperated = sDateOperated.Substring(6, 4);
                }
                else
                {
                    result.Close();
                    result.Query = "select * from businesses where bin = '" + sBin + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            dtDateOperated = result.GetDateTime("dt_operated");
                            sDateOperated = string.Format("{0:MM/dd/yyyy}", dtDateOperated);
                            sYearOperated = sDateOperated.Substring(6, 4);
                        }
                    }
                }
            }
            result.Close();

            if (txtStat.Text.Trim() == "RET")
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'X' order by tax_year asc";
            else
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' order by tax_year DESC";
            if (result.Execute())
            {
                if (result.Read())
                {
                    txtTaxYear.Text = result.GetString("tax_year").Trim();
                    sDueState = result.GetString("due_state");
                    if (txtStat.Text.Trim() == "RET")   // RMC 20150108
                        m_sNextQtrToPay = result.GetString("QTR_TO_PAY");
                }
            }
            result.Close();

            result.Query = "delete from pay_temp where bin = '" + sBin.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {
            }
            result.Close();

            result.Query = "delete from pay_temp_bns where bin = '" + sBin.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {
            }
            result.Close();
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            //plsqlCmd.ProcedureName = "compute_tax_and_fees";
            plsqlCmd.ProcedureName = "compute_tax_and_fees_new";    // RMC 20170725 configured penalty computation, merged from Binan
            plsqlCmd.ParamValue = sBin;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sToday;
            plsqlCmd.AddParameter("m_sOrDate", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sCurrentYear;
            plsqlCmd.AddParameter("m_sCurrentTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtStat.Text;
            plsqlCmd.AddParameter("m_sStatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtTaxYear.Text;
            plsqlCmd.AddParameter("m_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_iMultiTaxYear;
            plsqlCmd.AddParameter("m_iMultiTaxYear", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sRevYear;
            plsqlCmd.AddParameter("m_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sDateOperated;
            plsqlCmd.AddParameter("m_sDateOperated", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sYearOperated;
            plsqlCmd.AddParameter("m_sYearOperated", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sIsQtrly;
            plsqlCmd.AddParameter("m_sQtrly", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sNextQtrToPay;
            plsqlCmd.AddParameter("m_sNxtQtrToPay", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Output);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();

            }
            plsqlCmd.Close();
            m_sQtr1 = "F";
            m_sTerm = "F";

            

            // for displaying dues
            if (txtStat.Text.Trim() == "RET")
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'X' and tax_year <= '" + txtTaxYear.Text.Trim() + "'";
            else
              //result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state <> 'P' and tax_year <= '" + txtTaxYear.Text.Trim() + "' order by tax_year desc ,qtr_to_pay desc ,due_state desc";
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state <> 'X' and tax_year <= '" + txtTaxYear.Text.Trim() + "' order by tax_year desc ,qtr_to_pay desc ,due_state desc"; // RMC 20170112 corrected SOA of Permit-update billing //JHB 20181121
               
            if (result.Execute())
            {
                if (result.Read())
                {
                    sQtrToPay = result.GetString("qtr_to_pay").Trim();
                    if (sQtrToPay != "1" && m_iMultiTaxYear != 1)
                    {
                        m_sNextQtrToPay = sQtrToPay;
                        OnInstallment(sBin);
                    }
                    else
                        if (sQtrToPay != "1" && m_iNextQtrToPay != 1)
                        {
                            result2.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'A'  and bin in (select bin from taxdues where due_state = 'X')";
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    m_sNextQtrToPay = sQtrToPay;
                                    chkFull.Checked = false;
                                    chkQtr.Checked = true;

                                    //OnInstallment(sBin);  // RMC 20160517 correction in SOA if with current qtr due and permit-update transaction, put rem, OnInstallment() is already called on chkQtr_CheckedChanged
                                }
                            }
                            result2.Close();
                        }

                    if (m_iMultiTaxYear != 1 && int.Parse(txtTaxYear.Text.Trim()) <= AppSettingsManager.GetCurrentDate().Year)
                    {
                        m_sNextQtrToPay = sQtrToPay;
                        btn1st.Checked = false;
                        btn2nd.Checked = false;
                        btn3rd.Checked = false;
                        btn4th.Checked = false;
                        // RMC 20161215 display quarter due and date in SOA for Binan (s)
                        if (AppSettingsManager.GetConfigValue("10") == "243")
                        {
                            chkFull.Checked = false;
                            chkQtr.Checked = true;
                            /*// RMC 20170104 corrected display of SOA for retirement (s)
                            if (txtStat.Text.Trim() == "RET")
                                OnFull(); // RMC 20170119 temp
                            // RMC 20170104 corrected display of SOA for retirement (e)
                             */
                            // RMC 20170228 correction in SOA of RET, put rem
                        } // RMC 20161215 display quarter due and date in SOA for Binan (e)
                        else
                        {
                            chkFull.Checked = true;
                            chkQtr.Checked = false;
                        }
                        //DisplayDues(sBin, "F", "", "", ""); //MCR 20140603
                    }
                    else
                    {
                        if (txtStat.Text != "NEW" && sQtrToPay != "1")
                        {
                            m_sNextQtrToPay = sQtrToPay;
                            chkFull.Checked = false;
                            chkQtr.Checked = true;
                            //OnInstallment(sBin);  // RMC 20160517 correction in SOA if with current qtr due and permit-update transaction, put rem, OnInstallment() is already called on chkQtr_CheckedChanged
                        }
                        else
                        {
                            
                            // RMC 20161215 display quarter due and date in SOA for Binan (s)
                            if (AppSettingsManager.GetConfigValue("10") == "243")
                            {
                                chkFull.Checked = false;
                                chkQtr.Checked = true;
                                /*// RMC 20170104 corrected display of SOA for retirement (s)
                                if (txtStat.Text.Trim() == "RET")
                                    OnFull();// RMC 20170119 temp
                                // RMC 20170104 corrected display of SOA for retirement (e)*/
                                // RMC 20170228 correction in SOA of RET, put rem
                            } // RMC 20161215 display quarter due and date in SOA for Binan (e)
                            else
                            {
                                btn1st.Checked = false;
                                btn2nd.Checked = false;
                                btn3rd.Checked = false;
                                btn4th.Checked = false;
                                OnFull();
                            }
                        }
                    }

                }

            }
            result.Close();
            /*if (sDueState == "P")
            {
                OnFull();
            }*/
            //  // RMC 20160517 correction in SOA if with current qtr due and permit-update transaction, put rem

            

        }

        private void OnInstallment(string sBin)
        {
            chkFull.Checked = false;
            chkQtr.Checked = true;

            //MCR 20140603 (s)

            btn1st.Checked = false;
            btn2nd.Checked = false;
            btn3rd.Checked = false;
            btn4th.Checked = false;

            //MCR 20140603 (e)
            int iNextQtrToPay = 0;
            if (m_sNextQtrToPay.Trim() == string.Empty || m_sNextQtrToPay == "0")   // RMC 20141217 adjustments
                m_sNextQtrToPay = "1";

            //MCR 20140731 (s)
            String sQtr1 = "";
            String sQtr2 = "";
            String sQtr3 = "";
            String sQtr4 = "";

            DateTime sDateNow = AppSettingsManager.GetSystemDate();
            OracleResultSet pSet = new OracleResultSet();
            DateTime sDueJan = new DateTime(), sDueFeb, sDueMar, sDueApr = new DateTime(), sDueMay, sDueJun, sDueJul = new DateTime(), sDueAug, sDueSep, sDueOct = new DateTime(), sDueNov, sDueDec;
            pSet.Query = string.Format("select * from due_dates where due_year = '{0}'", AppSettingsManager.GetConfigValue("12"));
            if (pSet.Execute())
                while (pSet.Read())
                {
                    String sCode = pSet.GetString("due_code").Trim();
                    if (sCode == "01")
                        sDueJan = pSet.GetDateTime("due_date");
                    else if (sCode == "02")
                        sDueFeb = pSet.GetDateTime("due_date");
                    else if (sCode == "03")
                        sDueMar = pSet.GetDateTime("due_date");
                    else if (sCode == "04")
                        sDueApr = pSet.GetDateTime("due_date");
                    else if (sCode == "05")
                        sDueMay = pSet.GetDateTime("due_date");
                    else if (sCode == "06")
                        sDueJun = pSet.GetDateTime("due_date");
                    else if (sCode == "07")
                        sDueJul = pSet.GetDateTime("due_date");
                    else if (sCode == "08")
                        sDueAug = pSet.GetDateTime("due_date");
                    else if (sCode == "09")
                        sDueSep = pSet.GetDateTime("due_date");
                    else if (sCode == "10")
                        sDueOct = pSet.GetDateTime("due_date");
                    else if (sCode == "11")
                        sDueNov = pSet.GetDateTime("due_date");
                    else if (sCode == "12")
                        sDueDec = pSet.GetDateTime("due_date");
                }
            pSet.Close();

            // RMC 20141217 adjustments, this is temporary - check c++ san galing ang value ng sTaxYear?
            if (sTaxYear == "")
                sTaxYear = txtTaxYear.Text;
            // RMC 20141217 adjustments, this is temporary - check c++ san galing ang value ng sTaxYear?

            if (sDateNow >= sDueJan || Convert.ToInt16(txtTaxYear.Text) < Convert.ToInt16(sTaxYear))
            {
                m_sNextQtrToPay = "1";
                sQtr1 = "1";
            }
            if (sDateNow >= sDueApr || Convert.ToInt16(txtTaxYear.Text) < Convert.ToInt16(sTaxYear))
            {
                m_sNextQtrToPay = "2";
                sQtr2 = "2";
            }
            if (sDateNow >= sDueJul || Convert.ToInt16(txtTaxYear.Text) < Convert.ToInt16(sTaxYear))
            {
                m_sNextQtrToPay = "3";
                sQtr3 = "3";
            }
            if (sDateNow >= sDueOct || Convert.ToInt16(txtTaxYear.Text) < Convert.ToInt16(sTaxYear))
            {
                m_sNextQtrToPay = "4";
                sQtr4 = "4";
            }
            //MCR 20140731 (e)

            int.TryParse(m_sNextQtrToPay, out iNextQtrToPay);

            btn1st.Checked = true;
            string sCurrDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());    // RMC 20151109 enable unchecking/checking of current quarter even if the Business was delinquent - requested by Tumauini thru Jeslin
            if (iNextQtrToPay >= 1)
            {   
                btn1st.Enabled = false;
                btn1st.Checked = false;

                // RMC 20151109 enable unchecking/checking of current quarter even if the Business was delinquent - requested by Tumauini thru Jeslin (s)
                if (AppSettingsManager.GetQtr(sCurrDate) == "1")
                {
                    btn1st.Enabled = true;
                    btn1st.Checked = true;
                }
                // RMC 20151109 enable unchecking/checking of current quarter even if the Business was delinquent - requested by Tumauini thru Jeslin (e)
            }
            if (iNextQtrToPay >= 2)
            {
                btn2nd.Enabled = false;
                btn2nd.Checked = false;

                // RMC 20151109 enable unchecking/checking of current quarter even if the Business was delinquent - requested by Tumauini thru Jeslin (s)
                if (AppSettingsManager.GetQtr(sCurrDate) == "2")
                {
                    btn2nd.Enabled = true;
                    btn2nd.Checked = true;
                }
                // RMC 20151109 enable unchecking/checking of current quarter even if the Business was delinquent - requested by Tumauini thru Jeslin (e)
            }
            if (iNextQtrToPay >= 3)
            {
                btn3rd.Enabled = false;
                btn3rd.Checked = false;

                // RMC 20151109 enable unchecking/checking of current quarter even if the Business was delinquent - requested by Tumauini thru Jeslin (s)
                if (AppSettingsManager.GetQtr(sCurrDate) == "3")
                {
                    btn3rd.Enabled = true;
                    btn3rd.Checked = true;
                }
                // RMC 20151109 enable unchecking/checking of current quarter even if the Business was delinquent - requested by Tumauini thru Jeslin (e)
            }
            if (iNextQtrToPay == 4)
            {
                btn4th.Enabled = false;
                btn4th.Checked = false;

                // RMC 20151109 enable unchecking/checking of current quarter even if the Business was delinquent - requested by Tumauini thru Jeslin (s)
                if (AppSettingsManager.GetQtr(sCurrDate) == "4")
                {
                    btn4th.Enabled = true;
                    btn4th.Checked = true;
                }
                // RMC 20151109 enable unchecking/checking of current quarter even if the Business was delinquent - requested by Tumauini thru Jeslin (e)
            }

            if (m_sNextQtrToPay == "1")
            {
                btn1st.Checked = true;
                btn2nd.Checked = false;
                btn3rd.Checked = false;
                btn4th.Checked = false;
                sQtr1 = "1"; sQtr2 = ""; sQtr3 = ""; sQtr4 = "";
            }
            if (m_sNextQtrToPay == "2")
            {
                btn1st.Checked = true;
                btn2nd.Checked = true;
                btn3rd.Checked = false;
                btn4th.Checked = false;
                sQtr1 = "1"; sQtr2 = "2"; sQtr3 = ""; sQtr4 = "";
            }
            if (m_sNextQtrToPay == "3")
            {
                btn1st.Checked = true;
                btn2nd.Checked = true;
                btn3rd.Checked = true;
                btn4th.Checked = false;
                sQtr1 = "1"; sQtr2 = "2"; sQtr3 = "3"; sQtr4 = "";
            }
            if (m_sNextQtrToPay == "4")
            {
                btn1st.Checked = true;
                btn2nd.Checked = true;
                btn3rd.Checked = true;
                btn4th.Checked = true;
                sQtr1 = "1"; sQtr2 = "2"; sQtr3 = "3"; sQtr4 = "4";
            }

            /* // RMC 20140725 Added Permit update billing (s)
            // force SOA up to current qtr
            string sCurrDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
            if (AppSettingsManager.GetQtr(sCurrDate) == "2" && !btn2nd.Checked)
            {
               // m_sNextQtrToPay = "2";
                btn2nd.Checked = true;
            }
            if (AppSettingsManager.GetQtr(sCurrDate) == "3" && !btn3rd.Checked)
            {
                //m_sNextQtrToPay = "3";
                btn3rd.Checked = true;
            }
            if (AppSettingsManager.GetQtr(sCurrDate) == "4" && !btn4th.Checked)
            {
                //m_sNextQtrToPay = "4";
                btn4th.Checked = true;
            }
            // RMC 20140725 Added Permit update billing (e)*/
            /*
            DisplayDues(sBin, m_sNextQtrToPay, "", "", "");

            if (bRevExam(sBIN))
                DisplayDues(sBIN, "A", "", "", "");
            */

            //ADD: MCR 20142905 (s)
            dgvTaxFees.Rows.Clear();
            m_bExistTaxPU = false;
            if (!bRevExam(sBIN))
            {
                if (txtStat.Text.Trim() == "RET")   // RMC 20140708 added retirement billing
                    DisplayDuesNew(sBIN, "X", "", "", "");
                //else  // RMC 20170228 correction in SOA of RET, put rem
                {
                    if (m_sNextQtrToPay == "1")
                        DisplayDuesNew(sBin, "1", "", "", "");
                    else if (m_sNextQtrToPay == "2")
                        DisplayDuesNew(sBin, sQtr1, sQtr2, "", "");
                    else if (m_sNextQtrToPay == "3")
                        DisplayDuesNew(sBin, sQtr1, sQtr2, sQtr3, "");
                    else if (m_sNextQtrToPay == "4")
                        DisplayDuesNew(sBin, sQtr1, sQtr2, sQtr3, sQtr4);
                    else
                        DisplayDuesNew(sBin, sQtr1, sQtr2, sQtr3, sQtr4);

                    //DisplayDues(sBin, m_sNextQtrToPay, "", "", "");
                }
            }
            else
            {
                DisplayDuesNew(sBIN, "A", "", "", "");

                if (m_sNextQtrToPay == "1")
                    DisplayDuesNew(sBin, "1", "", "", "");
                else if (m_sNextQtrToPay == "2")
                    DisplayDuesNew(sBin, sQtr1, sQtr2, "", "");
                else if (m_sNextQtrToPay == "3")
                    DisplayDuesNew(sBin, sQtr1, sQtr2, sQtr3, "");
                else if (m_sNextQtrToPay == "4")
                    DisplayDuesNew(sBin, sQtr1, sQtr2, sQtr3, sQtr4);
                else
                    DisplayDuesNew(sBin, sQtr1, sQtr2, sQtr3, sQtr4);

                //DisplayDues(sBin, m_sNextQtrToPay, "", "", "");
            }
            //ADD: MCR 20142905 (e)
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSOA_Click(object sender, EventArgs e)
        {
            CreateSOATBL(); //AFM 20210722 reenabled

            frmSoaPrint rClass = new frmSoaPrint(); // RMC 20150107

            rClass.sSOATaxYear = txtTaxYear.Text.Trim();
            // CJC 20130816 (s)
            rClass.BnsNm = txtBnsName.Text.Trim(); // CJC 20130816
            rClass.BnsAdd = txtBnsAdd.Text.Trim();
            rClass.BnsOwn = m_sOwnCode;
            rClass.BnsCode = txtCode.Text.Trim();
            if (MessageBox.Show("Use pre-printed form?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                rClass.PreprintedForm = true;
            else
                rClass.PreprintedForm = false;
            // CJC 20130816 (e)
            //rClass.SOALUBAO(sBIN, m_sTerm, m_sQtr1);  // RMC 20150107

            // RMC 20150107
            rClass.sBin = sBIN;
            rClass.sTermPay = m_sTerm;
            rClass.sQtrToPay = m_sQtr1;
            rClass.isCheckedCredit = chkTaxCredit.Checked; //AFM 20200114
            rClass.ShowDialog();
            // RMC 20150107

            TransLog.UpdateLog(sBIN, txtStat.Text.Trim(), txtTaxYear.Text.Trim(), "SOA", m_dTransLogIn, AppSettingsManager.GetSystemDate());   // RMC 20170822 added transaction log feature for tracking of transactions per bin
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text.Trim() == "Search")
            {
                btnSearch.Text = "Clear";
                /*
                if (AppSettingsManager.IsForSoaPrint(bin1.GetBin(), txtTaxYear.Text.Trim())) // GDE 20111214 for soa
                    OnOnlineSearch();
                // GDE 20111214 for soa (s){
                else
                {
                    MessageBox.Show("Cannot generate SOA / Or Accept Payment.");
                    return;
                }
                // GDE 20111214 for soa (e)}
                 */
                OnOnlineSearch();
            }
            else
            {
                btnSearch.Text = "Search";
                ClearFields();
            }

        }

        private void OnOnlineSearch()
        {
            sBIN = bin1.GetBin();
            if (bin1.txtTaxYear.Text.Trim() == string.Empty && bin1.txtBINSeries.Text.Trim() == string.Empty)
            {
                frmSearchBusiness fSearch = new frmSearchBusiness();
                fSearch.ShowDialog();
                this.sBIN = fSearch.sBIN;
                if (sBIN.Trim() != string.Empty)
                {
                    bin1.txtTaxYear.Text = this.sBIN.Substring(7, 4);
                    bin1.txtBINSeries.Text = this.sBIN.Substring(12, 7);
                }
            }
            ReturnValues();
        }

        private void ReturnValues()
        {
            sList.ReturnValuesByBinQue = sBIN;

            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                sOwnList.OwnName = sList.BPLSAppSettings[i].sOwnCode;
                txtBnsName.Text = sList.BPLSAppSettings[i].sBnsNm;
                txtBnsAdd.Text = sList.BPLSAppSettings[i].sBnsStat;
                txtType.Text = AppSettingsManager.GetBnsDesc(sList.BPLSAppSettings[i].sBnsCode);
                txtStat.Text = sList.BPLSAppSettings[i].sBnsStat;
                txtCode.Text = sList.BPLSAppSettings[i].sBnsCode;
                txtTaxYear.Text = sList.BPLSAppSettings[i].sTaxYear;
                txtLName.Text = sOwnList.OwnNamesSetting[i].sLn;
                txtFName.Text = sOwnList.OwnNamesSetting[i].sFn;
                txtMI.Text = sOwnList.OwnNamesSetting[i].sMi;

                m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
            }

            if (AppSettingsManager.IsForSoaPrint(sBIN, txtTaxYear.Text.Trim())) // GDE 20111214 for soa
            { }
            // GDE 20111214 for soa (s){
            else
            {
                if (sBIN != string.Empty)
                    MessageBox.Show("Cannot generate SOA / Or Accept Payment.");
                else
                    btnSearch.Text = "Search";

                return;
            }
            // GDE 20111214 for soa (e)}
            CheckData();

        }

        private void CheckData()
        {

            m_bMultiBns = AppSettingsManager.MultiBns(sBIN);
            if (m_iSwitch != 1 && m_iSwitch != 2)
            {
                if (AppSettingsManager.CheckBns(sBIN))
                {

                }
                else
                {
                    ClearFields();
                    OnOnlineSearch();
                }

                if (AppSettingsManager.TreasurerModule(sBIN))
                {
                    MessageBox.Show("SOA is subject for verification.");
                    return;
                }

            }
            else
            {
                OnFull();
                sList.ReturnValueByBin = sBIN;
                for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
                {
                    txtBnsName.Text = sList.BPLSAppSettings[i].sBnsNm;
                    txtTaxYear.Text = sList.BPLSAppSettings[i].sTaxYear;
                    txtStat.Text = sList.BPLSAppSettings[i].sBnsStat;
                }

                CheckTransfer();
            }

            if (AppSettingsManager.BounceCheck(sBIN))
            {
                MessageBox.Show("Cannot proceed. \nThis record has been paid under a bounced check...");
                return;
            }

            if (AppSettingsManager.TagForClosure(sBIN))
            {
                MessageBox.Show("Record was tagged for closure. Cannot continue.");
                return;
            }

            /*
            // RMC 20131229 retirement adjustment (s)
            if (ValidateRet_PUT(sBIN))
            {
                MessageBox.Show("Record has retirement or permit update application.\nView SOA at Collect.exe");
                ClearFields();
                return;
            }
            // RMC 20131229 retirement adjustment (e)
            */
            // RMC 20140708 added retirement billing

            string sMess = string.Empty;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            result.Query = "select * from hold_records where bin = '" + sBIN + "' and status = 'HOLD'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sMess = "Cannot continue! This record is currently on hold.\nUser Code: " + result.GetString("user_code").Trim() + "  Date: " + result.GetDateTime("dt_save").ToShortDateString() + "\nRemarks: " + result.GetString("remarks");
                    MessageBox.Show(sMess);
                    return;
                }
            }
            result.Close();

            if (m_iSwitch != 1 && m_iSwitch != 2)
            {
                OracleResultSet pSet = new OracleResultSet();

                bool blnIsSplBns = false;
                pSet.Query = "select * from spl_business_que where bin  = :1";
                pSet.AddParameter(":1", sBIN);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        blnIsSplBns = true;
                    }
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select * from spl_businesses where bin  = :1";
                        pSet.AddParameter(":1", sBIN);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            { blnIsSplBns = true; }
                        }
                    }
                }
                pSet.Close();

                if (blnIsSplBns)
                    result.Query = "select * from spl_business_que where bin = '" + sBIN + "'";
                else
                    result.Query = "select * from business_que where bin = '" + sBIN + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        m_sOwnCode = result.GetString("own_code").Trim();
                        result2.Query = "select new_own_code from permit_update_appl ";
                        result2.Query += "where bin = '" + sBIN + "' and appl_type = 'TOWN' and data_mode = 'QUE'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                m_sPrevOwnCode = m_sOwnCode;
                                m_sOwnCode = result2.GetString("new_own_code");
                                m_bIsRenPUT = true;
                            }
                        }
                        result2.Close();

                        result2.Query = "select * from permit_update_appl ";
                        result2.Query += "where bin = '" + sBIN + "' and appl_type = 'ADDL' and data_mode = 'QUE'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                                m_bIsRenPUT = true;
                        }
                        result2.Close();

                        m_sCheckOwnerCode = m_sOwnCode;
                        txtBnsName.Text = result.GetString("bns_nm");
                        txtCode.Text = result.GetString("bns_code");

                        result2.Query = "select * from permit_update_appl ";
                        result2.Query += "where bin = '" + sBIN + "' and appl_type = 'CBNS' and data_mode = 'QUE'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                txtBnsName.Text = result2.GetString("new_bns_name");
                                m_sOldBnsName = result2.GetString("old_bns_name");
                                m_bIsRenPUT = true;
                            }

                        }
                        result2.Close();

                        result2.Query = "select * from permit_update_appl ";
                        result2.Query += "where bin = '" + sBIN + "' and appl_type = 'CTYP' and data_mode = 'QUE'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                result3.Query = "select new_bns_code from change_class_tbl ";
                                result3.Query += "where bin = '" + sBIN + "' and is_main = 'Y'";
                                if (result3.Execute())
                                {
                                    if (result3.Read())
                                    {
                                        txtCode.Text = result3.GetString("new_bns_code");
                                        txtType.Text = AppSettingsManager.GetBnsDesc(txtCode.Text);
                                        m_bIsRenPUT = true;
                                    }
                                }
                                result3.Close();
                            }

                        }
                        result2.Close();

                        txtStat.Text = result.GetString("bns_stat");
                        txtTaxYear.Text = result.GetString("tax_year");
                        m_sDtOperated = result.GetDateTime("dt_operated").ToShortDateString();

                        if (txtStat.Text.Trim() == "RET")
                        {
                            result2.Query = "select distinct(bin) from pay_hist where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                    m_bRetVoidSurchPen = true;
                            }
                            result2.Close();
                        }


                    }
                    else
                    {
                        result.Close();
                        result.Query = "select * from businesses where bin = '" + sBIN + "'";
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                m_sOwnCode = result.GetString("own_code");
                                result2.Query = "select new_own_code from permit_update_appl ";
                                result2.Query += "where bin = '" + sBIN + "' and appl_type = 'TOWN' and data_mode = 'QUE'";
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        m_sPrevOwnCode = m_sOwnCode;
                                        m_sOwnCode = result2.GetString("new_own_code");
                                        m_bIsRenPUT = true;
                                    }
                                }
                                result2.Close();

                                result2.Query = "select * from permit_update_appl ";
                                result2.Query += "where bin = '" + sBIN + "' and appl_type = 'ADDL' and data_mode = 'QUE'";
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                        m_bIsRenPUT = true;
                                }
                                result2.Close();

                                m_sCheckOwnerCode = m_sOwnCode;
                                txtBnsName.Text = result.GetString("bns_nm");
                                txtCode.Text = result.GetString("bns_code");

                                result2.Query = "select * from permit_update_appl ";
                                result2.Query += "where bin = '" + sBIN + "' and appl_type = 'CBNS' and data_mode = 'QUE'";
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        txtBnsName.Text = result2.GetString("new_bns_name");
                                        m_sOldBnsName = result2.GetString("old_bns_name");
                                        m_bIsRenPUT = true;
                                    }

                                }
                                result2.Close();

                                result2.Query = "select * from permit_update_appl ";
                                result2.Query += "where bin = '" + sBIN + "' and appl_type = 'CTYP' and data_mode = 'QUE'";
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        result3.Query = "select new_bns_code from change_class_tbl ";
                                        result3.Query += "where bin = '" + sBIN + "' and is_main = 'Y'";
                                        if (result3.Execute())
                                        {
                                            if (result3.Read())
                                            {
                                                txtCode.Text = result3.GetString("new_bns_code");
                                                txtType.Text = AppSettingsManager.GetBnsDesc(txtCode.Text);
                                                m_bIsRenPUT = true;
                                            }
                                        }
                                        result3.Close();
                                    }

                                }
                                result2.Close();

                                txtStat.Text = result.GetString("bns_stat");
                                txtTaxYear.Text = result.GetString("tax_year");

                                // RMC 20131226 corrected soa of retirement for billing (s)
                                /*result2.Query = "select * from retired_bns_temp where bin = '" + sBIN + "'";
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        txtStat.Text = result2.GetString("bns_stat");
                                        txtTaxYear.Text = result2.GetString("tax_year");
                                    }
                                    
                                }
                                result2.Close();*/
                                // RMC 20131226 corrected soa of retirement for billing (e)

                                if (txtStat.Text.Trim() == "NEW")
                                {
                                    result2.Query = "select * from taxdues where bin = '" + sBIN + "' order by tax_year desc";
                                    if (result2.Execute())
                                    {
                                        if (result2.Read())
                                        {
                                            string sTempTaxYear = string.Empty;
                                            sTempTaxYear = result2.GetString("tax_year");
                                            if (int.Parse(sTempTaxYear) > int.Parse(txtTaxYear.Text))
                                                txtStat.Text = "REN";
                                        }
                                    }
                                    result2.Close();
                                }
                                m_sDtOperated = result.GetDateTime("dt_operated").ToShortDateString();
                                sTaxYear = txtTaxYear.Text;
                            }
                            else
                            {
                                ClearFields();
                                bin1.txtTaxYear.Focus();
                                return;
                            }
                        }

                        result2.Query = "select * from partial_payer where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                chkPartial.Checked = true;
                            }
                            else
                                chkPartial.Checked = false;
                        }
                        result2.Close();


                    }
                }
                result.Close();

                m_sStatus = txtStat.Text.Trim();
                txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(sBIN);

                result.Query = "select new_bns_loc from permit_update_appl ";
                result.Query += "where bin = '" + sBIN.Trim() + "' and appl_type = 'TLOC' and data_mode = 'QUE'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        m_sPrevBnsLoc = txtBnsAdd.Text.Trim();
                        txtBnsAdd.Text = result.GetString("new_bns_loc");
                        result2.Query = "select * from business_que where bin = '" + sBIN.Trim() + "'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                                m_bIsRenPUT = true;
                            else
                                m_bIsRenPUT = false;
                        }
                        result2.Close();
                    }
                }
                result.Close();

                sList.OwnName = m_sOwnCode;
                for (int i = 0; i < sList.OwnNamesSetting.Count; i++)
                {
                    txtLName.Text = sList.OwnNamesSetting[i].sLn;
                    txtFName.Text = sList.OwnNamesSetting[i].sFn;
                    txtMI.Text = sList.OwnNamesSetting[i].sMi;
                }
                txtType.Text = AppSettingsManager.GetBnsDesc(txtCode.Text.Trim());


                if (txtStat.Text.Trim() == "RET")
                    result.Query = "select * from taxdues where bin = '" + sBIN.Trim() + "' and due_state = 'X' order by tax_year";
                else
                    result.Query = "select * from taxdues where bin = '" + sBIN.Trim() + "' order by tax_year";

                if (result.Execute())
                {
                    if (result.Read())
                    {
                    }
                    else
                    {
                        result2.Query = "select * from compromise_tbl where bin = '" + sBIN.Trim() + "'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                if (int.Parse(sTaxYear) < int.Parse(AppSettingsManager.GetSystemDate().Year.ToString()))
                                {
                                    MessageBox.Show("Unrenewd Business or not yet billed.");
                                    ClearFields();
                                    bin1.txtTaxYear.Focus();
                                    return;
                                }
                                MessageBox.Show("No taxdues for the said BIN");
                                ClearFields();
                                bin1.txtTaxYear.Focus();
                                return;
                            }
                            else
                                bCompromise = true;
                        }
                        result2.Close();


                    }
                    if (bCompromise == false)
                    {
                        result2.Query = "select * from taxdues where bin = '" + sBIN + "' order by tax_year desc";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                                txtTaxYear.Text = result2.GetString("tax_year");
                        }
                        result2.Close();

                    }
                }
                result.Close();

                result.Query = "delete from pay_temp where bin = '" + sBIN.Trim() + "'";
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();
                result.Query = "delete from pay_temp_bns where bin = '" + sBIN.Trim() + "'";
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();
                //ClearFields();
                result.Query = "select * from compromise_due ";
                result.Query += "where bin = '" + sBIN.Trim() + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        m_sCompPaySw = result.GetString("pay_sw");
                        m_bIsCompromise = true;
                    }
                    else
                        m_bIsCompromise = false;
                }
                result.Close();
                ComputeTaxAndFees(sBIN);
            }
            /*
            mv_sCashAmount = "0.00"; // CTS 11302004 initialize of cash amount
			mv_sCreditLeft = "0.00";
			mv_sToBeCredited = "0.00";
			mc_bTaxCredit.EnableWindow(FALSE);
            */

            if (m_sOwnCode.Trim() != string.Empty)
            {
                double dBalance = 0;
                //                result.Query = "select * from dbcr_memo where own_code = '" + m_sOwnCode.Trim() + "' and served = 'N' and multi_pay = 'N'"; // GDE 20111021
                //result.Query = "select * from dbcr_memo where bin = '" + sBIN.Trim() + "' and served = 'N' and multi_pay = 'N'";
                //result.Query = "select * from dbcr_memo where own_code = '" + m_sOwnCode + "' and served = 'N' and multi_pay = 'N'"; //JARS 20170704
                result.Query = "select * from dbcr_memo where bin = '" + sBIN.Trim() + "' and served = 'N' and multi_pay = 'N'"; //JARS 20171010
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dBalance = result.GetDouble("balance");
                        m_sCreditLeft = string.Format("{0:#,##0.00}", dBalance);
                        txtCreditLeft.Text = m_sCreditLeft;
                    }
                }
                result.Close();
            }

            bool dDeducted = false;
            if (txtCreditLeft.Text.Trim() == string.Empty)
                txtCreditLeft.Text = "0.00";
            if (txtTotTotDue.Text.Trim() == string.Empty)
                txtTotTotDue.Text = "0.00";
            //if ((double.Parse(txtTotTotDue.Text) <= double.Parse(txtCreditLeft.Text)) && double.Parse(txtCreditLeft.Text) != 0)   // RMC 20160226 added display of tax credit in frmSoa, put rem
            if (double.Parse(txtCreditLeft.Text) != 0)  // RMC 20160226 added display of tax credit in frmSoa
            {
                if (chkTaxCredit.Checked == true) //AFM 20200114 applied condition to not adjust total since checkbox is default to false (s)
                {
                    double dGrandTotTotDue = 0;
                    if (m_bMultiBns == true)
                    {
                        dDeducted = false;
                        txtGrandTotTotDue.Text = txtTotTotDue.Text;
                    }
                    else
                    {
                        // ComputeLessCredit - same behavior
                        dDeducted = true;
                        dGrandTotTotDue = double.Parse(txtTotTotDue.Text) - double.Parse(txtCreditLeft.Text);
                        txtGrandTotTotDue.Text = string.Format("{0:#,##0.00}", dGrandTotTotDue);
                    }
                }
                else
                    txtGrandTotTotDue.Text = txtTotTotDue.Text; //AFM 20200114 applied condition to not adjust total since checkbox is default to false (e)


            }
            #region
            // for payment class
            /*
            mc_bCash.SetCheck(FALSE);
            mc_bCheck.SetCheck(FALSE);
            mv_bTaxCredit = FALSE; // ALJ 07012005 TEST confirm JON
            mc_bTaxCredit.SetCheck(FALSE);
            // ^ CTS 12112004 teller will have option to choose of what payment type

            // CTS 11302004 add this for tax credit
            if (bDeducted == TRUE)
            {
                mv_bTaxCredit = TRUE; // ALJ 07012005 TEST confirm JON
                mc_bTaxCredit.SetCheck(TRUE);
                mc_bTaxCredit.EnableWindow(TRUE);
                if (atof(mv_sGrandTotalDue) == 0.00)
                {
                    mc_bCash.EnableWindow(FALSE);
                    mc_bCheck.EnableWindow(FALSE);
                }
                else
                {
                    mc_bCash.EnableWindow(TRUE);
                    mc_bCheck.EnableWindow(TRUE);
                }

            }
            */
            // for payment class

            // DITO AQ NATAPOS
            #endregion
            CreateSOATBL(); //MCR 20210610
        }

        private void CheckTransfer()
        {
            string strTranscode = string.Empty;
            string sAppName = string.Empty;
            string sLName = string.Empty;
            string sFName = string.Empty;
            string sMI = string.Empty;
            string m_sAddNo = string.Empty;
            string m_sStreet = string.Empty;
            string m_sBrgy = string.Empty;
            string m_sZone = string.Empty;
            string m_sDist = string.Empty;
            string m_sMun = string.Empty;
            string m_sProv = string.Empty;
            string m_sAddrNo = string.Empty;

            OracleResultSet result = new OracleResultSet();
            result.Query = "select transfer_table.* from transfer_table,trans_fees_table where transfer_table.bin = trans_fees_table.bin ";
            result.Query += " transfer_table.trans_app_code = trans_fees_table.trans_app_code and trans_fees_table.tax_year = '" + txtTaxYear.Text + "' and transfer_table.bin = '" + sBIN + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    strTranscode = result.GetString("trans_app_code").Trim();
                    if (strTranscode == "TO")
                    {
                        sAppName += "Transfer of Ownership";
                        txtLName.Text = result.GetString("own_ln");
                        txtFName.Text = result.GetString("own_fn");
                        txtMI.Text = result.GetString("own_mi");
                        m_sPrevOwnCode = result.GetString("prev_own_code");
                        m_sNewOwnCode = result.GetString("new_own_code");
                        m_sOwnCode = m_sNewOwnCode;
                    }
                    if (strTranscode == "TL")
                    {
                        sAppName += "Transfer of Location";
                        m_sAddrNo = result.GetString("addr_no");
                        m_sStreet = result.GetString("addr_street");
                        m_sBrgy = result.GetString("addr_brgy");
                        m_sZone = result.GetString("addr_zone");
                        m_sDist = result.GetString("addr_dist");
                        m_sMun = result.GetString("addr_mun");
                        m_sProv = result.GetString("addr_prov");
                        txtBnsAdd.Text = m_sAddrNo + " " + m_sStreet + ", " + m_sBrgy + " " + m_sZone + " " + m_sDist + " " + m_sMun + " " + m_sProv;
                    }
                    if (strTranscode == "TC")
                    {
                        sAppName += "Change of Classification";
                        txtCode.Text = result.GetString("bns_code");
                        txtType.Text = AppSettingsManager.GetBnsDesc(txtCode.Text);
                    }
                }


                /*
                 sQuery = "select * from businesses where bin = '"+ sBIN +"'";
						pRec1->Open(_bstr_t(sQuery),pApp->m_pConnection.GetInterfacePtr(),adOpenStatic,adLockReadOnly,adCmdText);
						if (!pRec1->adoEOF)
						{
							if (mv_sOwnNm.IsEmpty())
							{
								m_sNewOwnCode = pApp->TrimAll(pApp->GetStrVariant(pRec1->GetCollect("own_code")));
								m_sOwnCode    = m_sNewOwnCode;
								m_sCheckOwnerCode = m_sOwnCode; // CTS 01022005 initialize to default own_code

								string.Format("select own_ln,own_fn,own_mi from own_names where own_code = '%s'",m_sNewOwnCode);
								pRec2->Open(_bstr_t(sQuery),pApp->m_pConnection.GetInterfacePtr(),adOpenStatic,adLockReadOnly,adCmdText);
								if (!pRec2->adoEOF)
								{
									sLName = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec2->GetCollect("own_ln"))));
									sFName = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec2->GetCollect("own_fn"))));
									sMI	   = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec2->GetCollect("own_mi"))));
									mv_sOwnNm = sLName + ", " + sFName + " " + sMI;
								}
								pRec2->Close();							
							}
							
							if (mv_sBnsLoc.IsEmpty())
							{
								m_sAddrNo = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec1->GetCollect("bns_house_no"))));
								m_sStreet = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec1->GetCollect("bns_street"))));
								m_sBrgy   = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec1->GetCollect("bns_brgy"))));
								m_sZone   = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec1->GetCollect("bns_zone"))));
								m_sDist   = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec1->GetCollect("bns_dist"))));
								m_sMun    = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec1->GetCollect("bns_mun"))));
								m_sProv   = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec1->GetCollect("bns_prov"))));
								mv_sBnsLoc = m_sAddrNo + " " + m_sStreet + ", " + m_sBrgy + " " + m_sZone + " " + m_sDist + " " + m_sMun + " " + m_sProv;
							}
										
							if (mv_sBnsCode.IsEmpty())
							{
								mv_sBnsCode    = pApp->TrimAll(pApp->GetStrVariant(pRec1->GetCollect("bns_code")));
								sQuery = "select bns_desc from bns_table where bns_code = '"+ mv_sBnsCode +"' and fees_code = 'B' and rev_year = '"+pApp->m_sRevYear+"'";
								pRec2->Open(_bstr_t(sQuery),pApp->m_pConnection.GetInterfacePtr(),adOpenStatic,adLockReadOnly,adCmdText);
								if (!pRec2->adoEOF)
									mv_sBnsDesc = pApp->HandleApostrophe(pApp->TrimAll(pApp->GetStrVariant(pRec2->GetCollect("bns_desc"))));
								pRec2->Close();
							}
						}
						pRec1->Close();

                 */
            }
            result.Close();

            result.Query = "select transfer_table.* from transfer_table,trans_fees_table where transfer_table.bin = trans_fees_table.bin ";
            result.Query += " transfer_table.trans_app_code = trans_fees_table.trans_app_code and trans_fees_table.tax_year = '" + txtTaxYear.Text + "' and transfer_table.bin = '" + sBIN + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                }
                else
                {
                    ClearFields();
                    MessageBox.Show("No Pending Application or not yet Billed.");
                    return;
                }
            }
            result.Close();



            // LoadBills
        }

        private void LoadBills()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet transCount = new OracleResultSet();
            string sFeesCode = string.Empty;
            string sFeesDue = string.Empty;
            string sFeesSurch = string.Empty;
            string sFeesPen = string.Empty;
            string sFeesAmtDue = string.Empty;
            string sFeesDesc = string.Empty;
            string sFees = string.Empty;
            string sTransAppCode = string.Empty;
            string sTransCount = string.Empty;
            int iTransCount = 0;

            result.Query = "select * from trans_fees_table where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sFeesCode = result.GetString("fees_code");
                    sFeesDue = string.Format("{0:#,##0.00}", double.Parse(result.GetString("fees_due")));
                    sFeesSurch = string.Format("{0:#,##0.00}", double.Parse(result.GetString("fees_surch")));
                    sFeesPen = string.Format("{0:#,##0.00}", double.Parse(result.GetString("fees_pen")));
                    sFeesAmtDue = string.Format("{0:#,##0.00}", double.Parse(result.GetString("fees_amtdue")));
                    sTransAppCode = result.GetString("trans_app_code");
                    sFees = sTransAppCode + "-" + AppSettingsManager.GetFeesDesc(sFeesCode.Trim());

                    transCount.Query = "select count(*) as iCount from pay_hist where bin = '" + sBIN + "' and tax_yeat = '" + txtTaxYear.Text + "' and qtr_paid like 'T%%'";
                    if (transCount.Execute())
                    {
                        if (transCount.Read())
                            iTransCount = transCount.GetInt("iCount");
                        sTransCount = string.Format("{0:0,000}", iTransCount);
                    }
                    transCount.Close();
                    sTransCount = "T" + sTransCount;

                    dgvTaxFees.Rows.Add(txtTaxYear.Text, "F", sTransCount, sFees, sFeesDue, sFeesSurch, sFeesPen, sFeesAmtDue, string.Empty, string.Empty, string.Empty);
                }
            }
            result.Close();
            // ComputeTotal;


        }

        private void OnFull()
        {
            strPaymentTerm = "F";
            ControlPaymentTerm(strPaymentTerm);

            m_bExistTaxPU = false;
            
            dgvTaxFees.Rows.Clear(); //MCR 20140604
            try
            {
                if (!bRevExam(sBIN))
                {
                    if (txtStat.Text.Trim() == "RET")   // RMC 20140708 added retirement billing
                    {
                        // RMC 20170228 correction in SOA of RET (s)
                       // OnInstallment();
                        // RMC 20170228 correction in SOA of RET (e)

                        DisplayDuesNew(sBIN, "X", "", "", "");
                    }
                    //else  // RMC 20170228 correction in SOA of RET, put rem      
                        DisplayDuesNew(sBIN, "F", "", "", "");
                }
                else
                {
                    DisplayDuesNew(sBIN, "A", "", "", "");
                    DisplayDuesNew(sBIN, "F", "", "", "");
                }
            }
            catch { }


            // GDE 20110418
            if (txtStat.Text.Trim() == "NEW")
                chkQtr.Enabled = false;
            else
                chkQtr.Enabled = true;
            // GDE 20110418

            if ((Convert.ToInt32(txtTaxYear.Text) == AppSettingsManager.GetSystemDate().Year)
                && ((AppSettingsManager.GetSystemDate().Month - 1) / 3 + 1) == 4)
                chkQtr.Enabled = false;
        }

        private bool bRevExam(string strBin)
        {
            bool bResult = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from taxdues where bin = '" + strBin.Trim() + "' and due_state = 'A'";
            if (result.Execute())
            {
                if (result.Read())
                    bResult = true;
            }
            result.Close();
            return bResult;
        }

        private void ControlPaymentTerm(string strTerm)
        {
            if (strTerm == "F")
            {
                chkQtr.Checked = false;
                btn1st.Enabled = false;
                btn2nd.Enabled = false;
                btn3rd.Enabled = false;
                btn4th.Enabled = false;
                btn1st.Checked = false;
                btn2nd.Checked = false;
                btn3rd.Checked = false;
                btn4th.Checked = false;
                chkFull.Checked = true;
            }
            else
            {
                chkFull.Checked = false;
                chkQtr.Checked = true;
                btn1st.Enabled = true;
                btn2nd.Enabled = true;
                btn3rd.Enabled = true;
                btn4th.Enabled = true;
            }

        }

        private void ClearFields()
        {
            txtBinSeries.Text = string.Empty;
            txtBinYr.Text = string.Empty;
            txtTaxYear.Text = string.Empty;
            txtBnsAdd.Text = string.Empty;
            txtBnsName.Text = string.Empty;
            txtType.Text = string.Empty;
            txtStat.Text = string.Empty;
            txtCode.Text = string.Empty;
            txtLName.Text = string.Empty;
            txtFName.Text = string.Empty;
            txtMI.Text = string.Empty;
            txtTotDue.Text = string.Empty;
            txtTotPen.Text = string.Empty;
            txtTotSurch.Text = string.Empty;
            txtTotTotDue.Text = string.Empty;
            chkFull.Checked = false;
            chkQtr.Checked = false;
            btn1st.Checked = false;
            btn2nd.Checked = false;
            btn3rd.Checked = false;
            btn4th.Checked = false;
            dgvTaxFees.Rows.Clear();
            chkQtr.Enabled = true;
            chkFull.Enabled = true;
            txtTaxYear.Text = string.Empty;
            chkFull.Checked = false;
            chkQtr.Checked = false;
            bin1.txtTaxYear.Text = string.Empty;
            bin1.txtBINSeries.Text = string.Empty;
            bin1.txtTaxYear.Focus();

            // RMC 20151002 added display of tax credit in SOA (s)
            txtCreditLeft.Text = string.Empty;
            txtGrandTotTotDue.Text = string.Empty;
            txtGrandTotPen.Text = string.Empty;
            txtGrandTotSurch.Text = string.Empty;
            txtGrabdTotDue.Text = string.Empty;
            // RMC 20151002 added display of tax credit in SOA (e)
        }
        private void txtBinYr_TextChanged(object sender, EventArgs e)
        {
            iCountTaxYear = txtBinYr.Text.Trim().Length;
            if (iCountTaxYear == 4)
                txtBinSeries.Focus();
        }

        private void txtBinSeries_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void txtBinSeries_Leave(object sender, EventArgs e)
        {
            try
            {
                txtBinSeries.Text = string.Format("{0:0000000}", int.Parse(txtBinSeries.Text));
            }
            catch { }

        }

        private void chkQtr_CheckedChanged(object sender, EventArgs e)
        {
            if (chkQtr.Checked == true)
            {
                btn1st.Enabled = true;
                btn2nd.Enabled = true;
                btn3rd.Enabled = true;
                btn4th.Enabled = true;
                chkFull.Checked = false;
                m_sTerm = "I";
                //ComputeTaxAndFees(sBIN.Trim());
                // RMC 20161215 display quarter due and date in SOA for Binan (s)
                if (AppSettingsManager.GetConfigValue("10") == "243")
                {
                    //default values
                    btn1st.Checked = true;
                    btn2nd.Checked = true;
                    btn3rd.Checked = true;
                    btn4th.Checked = true;
                }
                else
                // RMC 20161215 display quarter due and date in SOA for Binan (e)
                    OnInstallment(sBIN);
            }

        }

        private void chkFull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFull.Checked == true)
            {
                //btn1st.Enabled = false;
                //btn2nd.Enabled = false;
                //btn3rd.Enabled = false;
                //btn4th.Enabled = false;
                //chkQtr.Checked = false;
                //btn1st.Checked = false;
                //btn2nd.Checked = false;
                //btn3rd.Checked = false;
                //btn4th.Checked = false;
                m_sTerm = "F";  // RMC 20140107 corrected display of term in SOA if FULL
                //ComputeTaxAndFees(sBIN.Trim());
                chkFull.Checked = true;
                OnFull();
            }
        }

        private void txtTaxYear_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTaxYear_KeyUp(object sender, KeyEventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            if (txtTaxYear.Text.Trim().Length == 4)
            {
                result.Query = "delete from pay_temp where bin = '" + sBIN.Trim() + "'";
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();
                dgvTaxFees.Rows.Clear();

                result.Query = "select * from taxdues where bin = '" + sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                    }
                    else
                    {
                        MessageBox.Show("No taxdues for the said year");
                        txtTaxYear.Focus();
                        return;
                    }
                }
                result.Close();
                ComputeTaxAndFees(sBIN.Trim());
            }
        }

        private void chkFull_Click(object sender, EventArgs e)
        {
            chkFull.Checked = true;
            chkQtr.Checked = false;
        }

        private void chkQtr_Click(object sender, EventArgs e)
        {
            chkQtr.Checked = true;
            chkFull.Checked = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtMI_TextChanged(object sender, EventArgs e)
        {

        }

        private bool ValidateRet_PUT(string sBIN)
        {
            // RMC 20131229 retirement adjustment
            // temp disabled soa of bin with retirement or PUT application in BTAS until fully migrated
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from taxdues where bin = '" + sBIN + "' and (due_state = 'X' or due_state = 'P') ";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();
                    return true;
                }
            }
            pSet.Close();

            return false;
        }

        //ADD MCR 20142905 (s)
        private void btn1st_Click(object sender, EventArgs e)
        {
            if (chkQtr.Checked == true) // RMC 20140107
            {
                m_sQtr1 = "1";
                m_sTerm = "I";
                dgvTaxFees.Rows.Clear();
                
                //MOD MCR 20142905 (s)
                m_bExistTaxPU = false;
                if (!bRevExam(sBIN))
                {
                    DisplayDuesNew(sBIN, "1", "", "", "");
                    // RMC 20170228 correction in SOA of RET (s)
                    if (txtStat.Text.Trim() == "RET")
                        DisplayDuesNew(sBIN, "X", "", "", "");
                    // RMC 20170228 correction in SOA of RET (e)
                }
                else
                {
                    DisplayDuesNew(sBIN, "A", "", "", "");
                    DisplayDuesNew(sBIN, "1", "", "", "");
                }
                //MOD MCR 20142905 (e)
                //DisplayDues(sBIN);
            }
        }

        private void btn2nd_Click(object sender, EventArgs e)
        {
            if (chkQtr.Checked == true) // RMC 20140107
            {
                m_sQtr1 = "2";
                m_sTerm = "I";

                //if(btn2nd.Checked == true)
                //{
                btn3rd.Checked = false;
                btn4th.Checked = false;
                //}

                if (m_sNextQtrToPay == "1" && btn1st.Checked == false)
                    btn2nd.Checked = false;

                dgvTaxFees.Rows.Clear();

                if (btn1st.Checked == true)
                    strQtr1 = "1";
                else
                    strQtr1 = string.Empty;
                if (btn2nd.Checked == true)
                    strQtr2 = "2";
                else
                    strQtr2 = string.Empty;
                if (btn3rd.Checked == true)
                    strQtr3 = "3";
                else
                    strQtr3 = string.Empty;
                if (btn4th.Checked == true)
                    strQtr4 = "4";
                else
                    strQtr4 = string.Empty;

                //MOD MCR 20142905 (s)
                m_bExistTaxPU = false;
                if (!bRevExam(sBIN))
                {
                    DisplayDuesNew(sBIN, strQtr1, strQtr2, strQtr3, strQtr4);
                    // RMC 20170228 correction in SOA of RET (s)
                    if (txtStat.Text.Trim() == "RET")
                        DisplayDuesNew(sBIN, "X", "", "", "");
                    // RMC 20170228 correction in SOA of RET (e)
                }
                else
                {
                    DisplayDuesNew(sBIN, "A", "", "", "");
                    DisplayDuesNew(sBIN, strQtr1, strQtr2, strQtr3, strQtr4);
                }
                //MOD MCR 20142905 (e)

                // codes computation of debit credit
            }
        }

        private void btn3rd_Click(object sender, EventArgs e)
        {
            if (chkQtr.Checked == true) // RMC 20140107
            {
                m_sQtr1 = "3";
                m_sTerm = "I";
                dgvTaxFees.Rows.Clear();


                //if (btn3rd.Checked == true)
                //{
                btn4th.Checked = false;
                //}

                if (m_sNextQtrToPay == "1" && btn2nd.Checked == false)
                    btn3rd.Checked = false;
                if (m_sNextQtrToPay == "2" && btn2nd.Checked == false)
                    btn3rd.Checked = false;


                if (btn1st.Checked == true)
                    strQtr1 = "1";
                else
                    strQtr1 = string.Empty;
                if (btn2nd.Checked == true)
                    strQtr2 = "2";
                else
                    strQtr2 = string.Empty;
                if (btn3rd.Checked == true)
                    strQtr3 = "3";
                else
                    strQtr3 = string.Empty;
                if (btn4th.Checked == true)
                    strQtr4 = "4";
                else
                    strQtr4 = string.Empty;

                //MOD MCR 20142905 (s)
                m_bExistTaxPU = false;
                if (!bRevExam(sBIN))
                {
                    DisplayDuesNew(sBIN, strQtr1, strQtr2, strQtr3, strQtr4);
                    // RMC 20170228 correction in SOA of RET (s)
                    if (txtStat.Text.Trim() == "RET")   
                        DisplayDuesNew(sBIN, "X", "", "", "");
                    // RMC 20170228 correction in SOA of RET (e)
                }
                else
                {
                    DisplayDuesNew(sBIN, "A", "", "", "");
                    DisplayDuesNew(sBIN, strQtr1, strQtr2, strQtr3, strQtr4);
                }
                //MOD MCR 20142905 (e)
                //DisplayDues(sBIN);
            }
        }

        private void btn4th_Click(object sender, EventArgs e)
        {
            if (chkQtr.Checked == true) // RMC 20140107
            {
                m_sQtr1 = "4";
                m_sTerm = "I";
                dgvTaxFees.Rows.Clear();



                if (btn4th.Checked == true)
                {
                    if (btn1st.Checked == true && btn2nd.Checked == false)
                        btn1st.Checked = false;

                    if (btn1st.Checked == true && btn3rd.Checked == false)
                    {
                        btn1st.Checked = false;
                        btn2nd.Checked = false;
                    }

                    if (btn2nd.Checked == true && btn3rd.Checked == false)
                    {
                        btn2nd.Checked = false;
                        btn3rd.Checked = false;
                    }


                }



                if (btn1st.Checked == true)
                    strQtr1 = "1";
                else
                    strQtr1 = string.Empty;
                if (btn2nd.Checked == true)
                    strQtr2 = "2";
                else
                    strQtr2 = string.Empty;
                if (btn3rd.Checked == true)
                    strQtr3 = "3";
                else
                    strQtr3 = string.Empty;
                if (btn4th.Checked == true)
                    strQtr4 = "4";
                else
                    strQtr4 = string.Empty;

                //MOD MCR 20142905 (s)
                m_bExistTaxPU = false;
                if (!bRevExam(sBIN))
                {
                    DisplayDuesNew(sBIN, strQtr1, strQtr2, strQtr3, strQtr4);
                    // RMC 20170228 correction in SOA of RET (s)
                    if (txtStat.Text.Trim() == "RET")
                        DisplayDuesNew(sBIN, "X", "", "", "");
                    // RMC 20170228 correction in SOA of RET (e)
                }
                else
                {
                    DisplayDuesNew(sBIN, "A", "", "", "");
                    DisplayDuesNew(sBIN, strQtr1, strQtr2, strQtr3, strQtr4);
                }
                //MOD MCR 20142905 (e)
                //DisplayDues(sBIN);
            }
        }

        private void ComputeTotal()
        {

            double dDue = 0;
            double dPen = 0;
            double dSurch = 0;
            double dTotDue = 0;

            for (int i = 0; i < dgvTaxFees.Rows.Count; i++)
            {
                dDue += Convert.ToDouble(dgvTaxFees.Rows[i].Cells[4].Value);
                dSurch += Convert.ToDouble(dgvTaxFees.Rows[i].Cells[5].Value);
                dPen += Convert.ToDouble(dgvTaxFees.Rows[i].Cells[6].Value);
                dTotDue += Convert.ToDouble(dgvTaxFees.Rows[i].Cells[7].Value);
            }

            txtTotDue.Text = dDue.ToString("#,##0.00");
            txtTotPen.Text = dPen.ToString("#,##0.00");
            txtTotSurch.Text = dSurch.ToString("#,##0.00");
            txtTotTotDue.Text = dTotDue.ToString("#,##0.00");

            // RMC 20160226 added display of tax credit in frmSoa (s)
            bool dDeducted = false;
            if (txtCreditLeft.Text.Trim() == string.Empty)
                txtCreditLeft.Text = "0.00";
            if (txtTotTotDue.Text.Trim() == string.Empty)
                txtTotTotDue.Text = "0.00";
            if (double.Parse(txtCreditLeft.Text) != 0)
            {
                double dGrandTotTotDue = 0;
                if (m_bMultiBns == true)
                {
                    dDeducted = false;
                    txtGrandTotTotDue.Text = txtTotTotDue.Text;
                }
                else
                {
                    // ComputeLessCredit - same behavior
                    dDeducted = true;
                    // AFM 20200722 MAO-20-13348 should deduct tax credit only if checked (s)
                    if (chkTaxCredit.Checked == true)
                        dGrandTotTotDue = double.Parse(txtTotTotDue.Text) - double.Parse(txtCreditLeft.Text);
                    else
                        dGrandTotTotDue = double.Parse(txtTotTotDue.Text);
                    // AFM 20200722 MAO-20-13348 should deduct tax credit only if checked (e)

                    txtGrandTotTotDue.Text = string.Format("{0:#,##0.00}", dGrandTotTotDue);
                }

            }
            // RMC 20160226 added display of tax credit in frmSoa (e)
        }

        private void dgvTaxFees_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ComputeTotal();
        }
        //ADD MCR 20142905 (e)

        private void btn1st_CheckedChanged(object sender, EventArgs e)
        {
            if (btn1st.Checked == true)
                btn1st_Click(sender, e);
        }

        private void btn2nd_CheckedChanged(object sender, EventArgs e)
        {
            if (btn2nd.Checked == true)
                btn2nd_Click(sender, e);
        }

        private void btn3rd_CheckedChanged(object sender, EventArgs e)
        {
            if (btn3rd.Checked == true)
                btn3rd_Click(sender, e);
        }

        private void btn4th_CheckedChanged(object sender, EventArgs e)
        {
            if (btn4th.Checked == true)
                btn4th_Click(sender, e);
        }

        private bool ValidatePUAddlBns(string sBin)
        {
            // RMC 20170404 additional validation in SOA and Payment if with new addl bns
            OracleResultSet pSetPU = new OracleResultSet();
            string sFeesCode = string.Empty;
            string sTaxYear = string.Empty;

            pSetPU.Query = "select * from  pay_temp where bin = '" + sBin + "' and due_state = 'P' and fees_code like 'B%'";
            if (pSetPU.Execute())
            {
                if(pSetPU.Read())
                {
                    sFeesCode = pSetPU.GetString("fees_code");
                    sTaxYear = pSetPU.GetString("tax_year");
                }
            }
            pSetPU.Close();

            if (sFeesCode != "")
            {
                sFeesCode = sFeesCode.Substring(1, sFeesCode.Length - 1);

                pSetPU.Query = "select * from permit_update_appl where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "'";
                pSetPU.Query += " and new_bns_code = '" + sFeesCode + "' and appl_type = 'ADDL'";
                if (pSetPU.Execute())
                {
                    if (pSetPU.Read())
                    {
                        pSetPU.Close();
                        return true;
                    }
                }
                pSetPU.Close();
            }

            return false;
        }

        private void chkTaxCredit_CheckedChanged(object sender, EventArgs e)
        {
            //AFM 20200114 (s)
            double dGrandTotTotDue = 0;
            double dCreditLeft = 0;
            double.TryParse(txtGrandTotTotDue.Text, out dGrandTotTotDue);
            double.TryParse(txtCreditLeft.Text, out dCreditLeft);
            if (chkTaxCredit.Checked == false)
            {
                dGrandTotTotDue += dCreditLeft;
                txtGrandTotTotDue.Text = string.Format("{0:#,##0.00}", dGrandTotTotDue);
            }
            else
            {
                dGrandTotTotDue -= dCreditLeft;
                txtGrandTotTotDue.Text = string.Format("{0:#,##0.00}", dGrandTotTotDue);
            }
            //AFM 20200114 (e)


            
        }

        private void CreateSOATBL() //MCR 20210610
        {
            // RMC 20151002 added display of tax credit in SOA (s)
            double dTmp = 0;
            double.TryParse(txtTotDue.Text, out dTmp);
            if (dTmp == 0)
            {
                MessageBox.Show("No data to view", "SOA", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // RMC 20151002 added display of tax credit in SOA (e)


            if (btn1st.Checked == true)
                m_sQtr1 = "1";
            if (btn2nd.Checked == true)
                m_sQtr1 = "2";
            if (btn3rd.Checked == true)
                m_sQtr1 = "3";
            if (btn4th.Checked == true)
                m_sQtr1 = "4";
            OracleResultSet result = new OracleResultSet();
            string sCode, sDueState, sTaxYear;
            string sQuery, sBalContent, sBalHeader, sTotalBal;
            string sContent, sColumnHeader, sGross, sTotal;
            string sTotalLabel, sTellerRow, sTellerLabel, sRefLabel, sRefNo, sTellerData;
            string sCreditMemo;
            double dGross = 0, dTotal, dSurch, dInt, dDue;

            sContent = "<550|<300|<300|<2500|>1300|>1350|>400|>1000|>1000|>1500;";

            int iLoopCount = 0;
            double dTmpFeesDue = 0, dTmpFeesSur = 0, dTmpFeesPen = 0, dTmpFeesTot = 0;

            result.Query = "delete from soa_tbl where bin = '" + sBIN + "'";
            if (result.ExecuteNonQuery() != 0)
            {
            }
            result.Close();

            for (int i = 0; i < dgvTaxFees.RowCount; i++)
            {
                sCode = dgvTaxFees.Rows[i].Cells[10].Value.ToString();
                sDueState = dgvTaxFees.Rows[i].Cells[8].Value.ToString();
                sTaxYear = dgvTaxFees.Rows[i].Cells[0].Value.ToString();
                if (sCode.Substring(0, 1) != "B" || sDueState == "A")
                {
                    sGross = "";
                    if (txtCode.Text.Substring(0, 2) == "09" && sCode.Substring(0, 2) == "11" || txtCode.Text.Substring(0, 2) == "05")
                    {
                        GetGross(sBIN, sTaxYear, sDueState, dgvTaxFees.Rows[i].Cells[2].Value.ToString(), txtCode.Text);
                        double.TryParse(m_sGross, out dGross);
                    }
                    if ((txtCode.Text.Substring(0, 2) == "07" || txtCode.Text.Substring(0, 2) == "02" || txtCode.Text.Substring(0, 2) == "18") && sCode.Substring(0, 2) == "12")
                    {
                        GetGross(sBIN, sTaxYear, sDueState, dgvTaxFees.Rows[i].Cells[2].Value.ToString(), txtCode.Text);
                        double.TryParse(m_sGross, out dGross);
                    }

                    // RMC 20170505 display gross in SOA for rev-exam adjustment (s)
                    if (sCode.Substring(0, 1) == "B")
                    {
                        GetGross(sBIN, sTaxYear, sDueState, dgvTaxFees.Rows[i].Cells[2].Value.ToString(), sCode); //MOD MCR 20170704 txtCode.Text to sCode
                        double.TryParse(m_sGross, out dGross);
                    }
                    // RMC 20170505 display gross in SOA for rev-exam adjustment (e)
                }
                else
                {
                    GetGross(sBIN, sTaxYear, sDueState, dgvTaxFees.Rows[i].Cells[2].Value.ToString(), dgvTaxFees.Rows[i].Cells[10].Value.ToString());
                    double.TryParse(m_sGross, out dGross);
                }

                // Codes for Manila (excluded) GDE 20100721
                // saving in SOA_TBL
                if (sCode.Substring(0, 1) != "B")
                    dGross = 0;
                result.Query = "insert into SOA_TBL VALUES (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,to_date(:13,'mm/dd/yyyy'))";
                result.AddParameter(":1", sBIN);
                result.AddParameter(":2", dgvTaxFees.Rows[i].Cells[0].Value.ToString());
                result.AddParameter(":3", dgvTaxFees.Rows[i].Cells[1].Value.ToString());
                result.AddParameter(":4", dgvTaxFees.Rows[i].Cells[2].Value.ToString());
                //result.AddParameter(":5", m_sGross);
                result.AddParameter(":5", dGross);
                result.AddParameter(":6", dgvTaxFees.Rows[i].Cells[3].Value.ToString());
                double.TryParse(dgvTaxFees.Rows[i].Cells[4].Value.ToString(), out dDue);
                double.TryParse(dgvTaxFees.Rows[i].Cells[5].Value.ToString(), out dSurch);
                double.TryParse(dgvTaxFees.Rows[i].Cells[6].Value.ToString(), out dInt);
                dTotal = dDue + dSurch + dInt;
                result.AddParameter(":7", dDue);
                result.AddParameter(":8", dSurch);
                result.AddParameter(":9", dInt);
                result.AddParameter(":10", dTotal);
                result.AddParameter(":11", dgvTaxFees.Rows[i].Cells[10].Value.ToString());
                //MCR 20210610 (s)
                result.AddParameter(":12", AppSettingsManager.GetBillNoAndDate(sBIN, txtTaxYear.Text, txtCode.Text, 0));
                result.AddParameter(":13", AppSettingsManager.ValidUntil(sBIN, ConfigurationAttributes.CurrentYear));
                //MCR 20210610 (e)
                if (result.ExecuteNonQuery() != 0)
                {
                }
                result.Close();

            }

            // RMC 20111227 added Gross monitoring module for gross >= 200000 (s)
            if (!AppSettingsManager.ValidateGrossMonitoring(sBIN, txtTaxYear.Text.ToString()))
                return;
            // RMC 20111227 added Gross monitoring module for gross >= 200000 (e)

            CreateQRCODE(); //MCR 20210701
            //string fileName = Directory.GetCurrentDirectory() + "\\QR\\QRCODE.exe";
            String sDir = Directory.GetCurrentDirectory() + "\\QRCODE.exe";
            Process.Start(sDir);
        }

        private void CreateQRCODE()
        {
            //string fileName = Directory.GetCurrentDirectory() + "\\QR\\QRCODE.txt";
            string fileName = Directory.GetCurrentDirectory() + "\\QRCODE.txt";
            try
            {
                String sBillNo = "", sValidity = "";
                string sBillUser = string.Empty;
                double dTotal = 0;
                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = "select sum(soa_tbl.total), soa_tbl.bill_no, soa_tbl.soa_validity, bill_no.bill_user from soa_tbl, bill_no where soa_tbl.bin = '" + sBIN + "' and soa_tbl.bill_no = bill_no.bill_no group by soa_tbl.bill_no, soa_tbl.SOA_VALIDITY, bill_no.bill_user"; //AFM 20210701 added bill user
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        dTotal = pSet.GetDouble(0);
                        sBillNo = pSet.GetString(1);
                        sValidity = pSet.GetDateTime(2).ToString("MM/dd/yyyy");
                        sBillUser = pSet.GetString("bill_user");
                    }
                pSet.Close();

                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                    File.Delete(fileName);

                // Create a new file     
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine("BIN: " + sBIN);
                    sw.WriteLine("TAX YEAR: " + txtTaxYear.Text);
                    sw.WriteLine("BILL NO: " + sBillNo);
                    sw.WriteLine("AMOUNT: " + dTotal.ToString("#,##0.00"));
                    sw.WriteLine("DATE BILLED: " + sValidity);
                    sw.WriteLine("PREPARED BY: " + sBillUser);
                    sw.WriteLine("VALIDITY DATE: " + sValidity);
                    sw.WriteLine("APPROVED BY: " + AppSettingsManager.SystemUser.UserCode);
                }

                //// Write file contents on console.     
                //using (StreamReader sr = File.OpenText(fileName))
                //{
                //    string s = "";
                //    while ((s = sr.ReadLine()) != null)
                //    {
                //        Console.WriteLine(s);
                //    }
                //}
            }
            catch
            {
            }
        }
    }
}