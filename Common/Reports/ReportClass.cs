
// RMC 20120117 corrected display of other_info details in Business Profile
// RMC 20111228 changed soa signatory to treas

using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.PrintUtilities;
using System.Drawing;
using System.Drawing.Printing;
using Amellar.Common.BPLSApp; 
using Amellar.Common.AppSettings;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Data;
using System.Threading;
using System.IO;

namespace Amellar.Common.Reports
{
    public class ReportClass
    {
        static string sImagepath = string.Empty;
        VSPrinterEmuDocument sss = new VSPrinterEmuDocument();
        
        private SystemUser m_objSystemUser = new SystemUser();

        //DateTime dDate = DateTime.Now;    // RMC 20110725 put rem
        DateTime dDate = AppSettingsManager.GetCurrentDate();   // RMC 20110725
        OracleResultSet result = new OracleResultSet();
        OracleResultSet result2 = new OracleResultSet();
        public int iReport = 0;
        string m_sGross = string.Empty;
        string sTellerName = string.Empty;
        public string m_sContentSOA;
        private VSPrinterEmuModel model;
        public string sSOATaxYear = string.Empty;
        BPLSAppSettingList sList = new BPLSAppSettingList();
        private string m_strModule = string.Empty;  // RMC 20110311
        private VSPrinterEmuModel pageHeaderModel;
        private bool m_bPrintBIN = false;   // RMC 20111007 added progress bar in List of Lessors report
        public int i_PageCount = 0;
        public int i_Curr = 0;
        private string m_sCap = string.Empty;   // RMC 20140122 view capital, gross, pre-gross on payment hist (s)
        private string m_sPreGross = string.Empty;  // RMC 20140122 view capital, gross, pre-gross on payment hist (e)
        private string m_sAdjGross = string.Empty; //JHB 20181203 merge from LAL-LO JARS 20181004
        public string m_sTaxYear = string.Empty; //JARS 20170907
        public string m_sBusinessDesc = string.Empty;
        public string m_sBusinessCode = string.Empty;

        //MCR 20140527
        public string sOrNo = string.Empty;
        public string sOrAmt = string.Empty;
        public string sOrDate = string.Empty;
        public string sAcceptedDate = string.Empty;
        public string sIssuedDate = string.Empty;
        public string sCeasedDate = string.Empty;
        public string sFPTaxYear = string.Empty;
        public string sQuery = string.Empty; //MCR 20170529
        private bool bPreprinted = false; //MCR 20141215

        public bool m_bFullRetirement = false;  // RMC 20151005 mods in retirement module
        public bool PreprintedForm
        {
            set { bPreprinted = value; }
        }

        // CJC 20130816 (s)
        private string m_strBnsNm = string.Empty;
        private string m_strBnsAdd = string.Empty; 
        private string m_strBnsOwn = string.Empty;
        private string m_strBnsCode = string.Empty;
        public string BnsNm 
        {
            get { return m_strBnsNm; }
            set { m_strBnsNm = value; }
        }
        public string BnsAdd 
        {
            get { return m_strBnsAdd; }
            set { m_strBnsAdd = value; }
        }
        public string BnsOwn
        {
            get { return m_strBnsOwn; }
            set { m_strBnsOwn = value; }
        }
        public string BnsCode
        {
            get { return m_strBnsCode; }
            set { m_strBnsCode = value; }
        }
        // CJC 20130816 (e)        

        public string ModuleCode
        {
            get { return m_strModule; }
            set { m_strModule = value; }
        }   // RMC 20110311
        public bool PrintBIN    // RMC 20111007 added progress bar in List of Lessors report
        {
            get { return m_bPrintBIN; }
            set { m_bPrintBIN = value; }
        }
        public ReportClass()
        {
            model = new VSPrinterEmuModel();
            model.LeftMargin = 50;
            //model.TopMargin = 25;
            //model.MaxY = 1100 - 25;
            model.TopMargin = 50;
            model.MaxY = 1100 - 200;

            // RMC 20111007 Added report for List of Lessors (s)
            pageHeaderModel = new VSPrinterEmuModel();
            pageHeaderModel.LeftMargin = 50;
            //pageHeaderModel.TopMargin = 25;
            //pageHeaderModel.MaxY = 1100 - 25;
            pageHeaderModel.TopMargin = 50;
            pageHeaderModel.MaxY = 1100 - 200;
            // RMC 20111007 Added report for List of Lessors (e)            
        }
        public void _Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            //bool blnVisibleState = this.Visible;
            //this.Visible = false;
            VSPrinterEmuDocument doc = (VSPrinterEmuDocument)sender;
            doc.Render(e);
            //if (blnVisibleState)
             //   this.Visible = true;
        }
        public void NoticeDelq(string sBrgy)
        {
            model.Reset();
            if (sBrgy == "ALL")
                sBrgy = "%";
            model.Clear();
            int iCtr = 0;
            string sQtr = string.Empty;
            bool bIsWithBilling = false;
            bool bIsWithOutCurrentPayment = false;
            string sBin = string.Empty;
            string sTaxYear = string.Empty;
            string sBnsCode = string.Empty;
            string sBnsStat = string.Empty;
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            OracleResultSet resultPayHist = new OracleResultSet();
            double dGross = 0;
            DateTime dDate = DateTime.Now;
            DateTime dQtrDate1 = new DateTime();
            DateTime dQtrDate2 = new DateTime();
            DateTime dQtrDate3 = new DateTime();
            DateTime dQtrDate4 = new DateTime();
            double dSurch = 0;
            double dPen = 0;
            dQtrDate1 = DateTime.Parse("1/20/" + AppSettingsManager.GetSystemDate().Year.ToString());
            dQtrDate2 = DateTime.Parse("4/20/" + AppSettingsManager.GetSystemDate().Year.ToString());
            dQtrDate3 = DateTime.Parse("7/20/" + AppSettingsManager.GetSystemDate().Year.ToString());
            dQtrDate4 = DateTime.Parse("10/20/" + AppSettingsManager.GetSystemDate().Year.ToString());
            double dTotTotal = 0;
            int iQtr = 0;
            if (dDate >= dQtrDate1)
                iQtr = 1;
            if (dDate >= dQtrDate2)
                iQtr = 2;
            if (dDate >= dQtrDate3)
                iQtr = 3;
            if (dDate >= dQtrDate4)
                iQtr = 4;
            sQtr = iQtr.ToString();
            
            result2.Query = "select * from businesses where bns_brgy like :1";
            result2.AddParameter(":1", sBrgy);
            //result2.AddParameter(":2", iQtr.ToString());
            //result2.AddParameter(":2", AppSettingsManager.GetSystemDate().Year.ToString());
            if (result2.Execute())
            {
                while (result2.Read())
                {
                    dTotTotal = 0;
                    try
                    {
                        dGross = result2.GetDouble("gr_1");
                    }
                    catch
                    { }
                    sBnsStat = result2.GetString("bns_stat").Trim();
                    sBnsCode = result2.GetString("bns_code").Trim();
                    sBin = result2.GetString("bin").Trim();
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
                    sTaxYear = result2.GetString("tax_year").Trim();
                    result.Query = "select * from taxdues where bin = :1";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                            bIsWithBilling = true;
                        else
                            bIsWithBilling = false;
                    }
                    result.Close();

                    result.Query = "select distinct * from pay_hist where bin = :1 and tax_year = :2";
                    result.AddParameter(":1", sBin);
                    result.AddParameter(":2", AppSettingsManager.GetSystemDate().Year.ToString());
                    if(result.Execute())
                    {
                        if(result.Read())
                            bIsWithOutCurrentPayment = false;
                        else
                            bIsWithOutCurrentPayment = true;
                    }
                    result.Close();

                    resultPayHist.Query = "select distinct * from pay_hist where bin = :1 and tax_year = :2";
                    resultPayHist.AddParameter(":1", sBin);
                    resultPayHist.AddParameter(":2", AppSettingsManager.GetSystemDate().Year.ToString());
                    if(resultPayHist.Execute())
                    {
                        if (resultPayHist.Read())
                            sQtr = "F";
                    }
                    resultPayHist.Close();
                    
                    if(bIsWithOutCurrentPayment)
                    {
                        if (sBnsStat == "RET")
                            sBnsStat = "RETIREMENT";
                        if (sBnsStat == "REN")
                            sBnsStat = "RENEWAL";

                        model.SetCurrentY(2500);
                        model.SetFontSize(14);
                        model.SetTable("^11000;Notice of Annual Delinquency");
                        model.SetTable("");
                        model.SetTable("");
                        model.SetTable("");
                        model.SetFontSize(10);
                        model.SetTable("<11000;" + sBin);
                        model.SetTable("<11000;" + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin)));
                        model.SetTable("<11000;" + AppSettingsManager.GetBnsName(sBin));
                        model.SetTable("<11000;OWNER'S ADDRESS: " + AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(sBin)));
                        model.SetTable("<11000;BUSINESS ADDRESS: " + AppSettingsManager.GetBnsAddress(sBin));
                        model.SetTable("<11000;BUSINESS TYPE: " + AppSettingsManager.GetBnsDesc(sBnsCode));
                        model.SetTable("<11000;STATUS: " + sBnsStat);
                        model.SetTable("");
                        model.SetTable("");
                        model.SetTable("");
                        model.SetTable("<11000;Sir/Madam:");
                        model.SetTable("");
                        model.SetTable("");
                        
                        if (!bIsWithBilling)
                        {
                            string sContent = string.Empty;
                            sContent = "     Records on file disclosed that the above-mentioned business has outstanding taxes and fees due to non-renewal of Mayor’s Permit since " + sTaxYear + ".  ";
                            if (sBnsStat == "NEW")
                            {
                                result.Query = "select capital from businesses where bin = :1";
                                result.AddParameter(":1", sBin);
                                if (result.Execute())
                                {
                                    if (result.Read())
                                    {
                                        try
                                        {
                                            dGross = result.GetDouble("capital");
                                        }
                                        catch
                                        {
                                        }
                                        
                                    }
                                }
                                result.Close();

                                sContent += " The last declared Capital is ";
                            }
                            else
                                sContent += " The last declared Gross Receipt is ";

                            sContent+= string.Format("{0:#,##0.#0}", dGross) + ".";

                            model.SetTable("=10500;" + sContent);

                        }
                        else
                        {
                            
                            model.SetTable("<11000;Records on this file disclosed that the above-mentioned business has outstanding taxes and fees, detailed below:");
                            model.SetTable("");
                            model.SetTable("<1000|^1000|^1000|>1500|>1500|>1500|>1500;|TAX YEAR|QTR|TAX|FEES|CHARGES|TOTAL;");
                            result.Query = "select distinct(tax_year) as tax_year, qtr_to_pay  from taxdues where bin = :1 order by tax_year";
                            result.AddParameter(":1", sBin.Trim());
                            if (result.Execute())
                            {
                                while (result.Read())
                                {
                                    sTaxYear = result.GetString("tax_year").Trim();
                                    sQtr = result.GetString("qtr_to_pay");
                                    
                                    if (sQtr == "1")
                                        sQtr = "F";
                                    if (sTaxYear != AppSettingsManager.GetSystemDate().Year.ToString())
                                    {
                                        if(sQtr == "2")
                                            sQtr = "2-4";
                                        if(sQtr == "3")
                                            sQtr = "3-4";
                                    }

                                    // GDE 20120703 insert PL/SQL HERE for COMPUTE_TAX_AND_FEES
                                    ComputeTaxAndFees(sBin, sTaxYear);
                                    dTotTotal += AppSettingsManager.GetSurchDues(sBin, sTaxYear) + AppSettingsManager.GetPenDues(sBin, sTaxYear) + AppSettingsManager.GetBNSTaxDues(sBin, sTaxYear) + AppSettingsManager.GetRegTaxDues(sBin, sTaxYear);
                                    model.SetTable("<1000|^1000|^1000|>1500|>1500|>1500|>1500;|" + result.GetString("tax_year") + "|" + sQtr + "|" + string.Format("{0:#,##0.#0}", AppSettingsManager.GetBNSTaxDues(sBin, sTaxYear)) + "|" + string.Format("{0:#,##0.#0}", AppSettingsManager.GetRegTaxDues(sBin, sTaxYear)) + "|" + string.Format("{0:#,##0.#0}", AppSettingsManager.GetSurchDues(sBin, sTaxYear) + AppSettingsManager.GetPenDues(sBin, sTaxYear)) + "|" + string.Format("{0:#,##0.#0}", AppSettingsManager.GetSurchDues(sBin, sTaxYear) + AppSettingsManager.GetPenDues(sBin, sTaxYear) + AppSettingsManager.GetBNSTaxDues(sBin, sTaxYear) + AppSettingsManager.GetRegTaxDues(sBin, sTaxYear)) + ";");
                                    
                                }
                            }
                            result.Close();
                        }
                        if (bIsWithBilling)
                        {
                            model.SetTable("");
                            model.SetTable("");
                            model.SetTable(">6000|>3000;TOTAL:|" + string.Format("{0:#,##0.#0}", dTotTotal) + ";");
                        }
                        model.SetTable("");
                        model.SetTable("");
                        model.SetTable("=10500;          In this regard, kindly come to the Office of the City Treasurer within seven (7) days from receipt hereof to settle your obligations to the City.  Failure of which shall compel this Office to pursue legal actions to enforce collection.");
                        model.SetTable("");
                        model.SetTable("=10500;          Kindly disregard this notice if taxes/fees have already been paid and present the official receipt/s evidencing payment and we shall consider this matter closed.  For more information, please contact us a telephone no. 545-6789 local 8110/8111/8112.");
                        model.SetTable("");
                        model.SetTable("=10500;          We trust you will give your full cooperation for a better Calamba.");
                        model.SetTable("");
                        model.SetTable("");
                        model.SetTable("<8000|^2500;|Very truly yours,");
                        model.SetTable("");
                        model.SetTable("");
                        model.SetTable("");
                        model.SetFontBold(1);
                        model.SetTable("<8000|^2500;|FRANCISCA M. ADRIANO");
                        model.SetFontBold(0);
                        model.SetTable("<8000|^2500;|ICO-City Treasurer");
                        model.SetTable("");
                        model.SetTable("");
                        model.SetTable("");
                        model.SetTable("<8000|^2500;Received by:|");
                        model.SetTable("<8000|^2500;Name:    ___________________|");
                        model.SetTable("<8000|^2500;Position:___________________|");
                        model.SetTable("<8000|^2500;Date:    ___________________|");
                        model.PageBreak();
                    }

                }
            }
            result2.Close();
            PreviewDocu();
        }
        private string GetNoticeDate(string pBin, int pNoticeNumber)
        {
            string strCurrentDate = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from official_notice_closure where bin = '" + pBin + "' and notice_number = " + pNoticeNumber + "";
            if (pSet.Execute())
                if (pSet.Read())
                    strCurrentDate = pSet.GetDateTime("notice_date").ToString("MMM dd, yyyy");
            pSet.Close();
            return strCurrentDate;
        }
        public void NoticeDelqNew(string sBrgy)
        {
            model.Reset();
            if (sBrgy == "ALL")
                sBrgy = "%";
            model.Clear();
            int iCtr = 0;
            string sQtr = string.Empty;
            bool bIsWithBilling = false;
            bool bIsWithOutCurrentPayment = false;
            string sBin = string.Empty;
            string sTaxYear = string.Empty;
            string sContent = string.Empty;
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            OracleResultSet resultPayHist = new OracleResultSet();
            double dTax = 0;
            double dSurch = 0;
            double dFee = 0;
            double dDue = 0;
            double dTotTotal = 0;
            DateTime dtSystemDate;

            long lngY1 = 0;
            dtSystemDate = AppSettingsManager.GetSystemDate();
            string sCurrentDate = string.Format("{0:MM/dd/yyyy}", dtSystemDate);

            result2.Query = "Select B.* From Official_Notice_Closure Onc Left Join Businesses B On B.Bin = Onc.Bin where bns_brgy like :1 and Onc.Notice_Number = '2'";
            result2.AddParameter(":1", sBrgy);
            //result2.AddParameter(":2", iQtr.ToString());
            //result2.AddParameter(":2", AppSettingsManager.GetSystemDate().Year.ToString());
            if (result2.Execute())
            {
                while (result2.Read())
                {
                    dTotTotal = 0;
                    sBin = result2.GetString("bin").Trim();

                    result.Query = "delete from pay_temp where bin = '" + sBin.Trim() + "'";
                    if (result.ExecuteNonQuery() != 0)
                    {
                    }
                    result.Close();

                    result.Query = "delete from pay_temp_bns where bin = '" + sBin.Trim() + "'";
                    if (result.ExecuteNonQuery() != 0)
                    {
                    }

                    //model.SetCurrentY(2500);
                    model.SetFontName("Arial");
                    model.SetFontSize(12);
                    model.SetTable("<11000;Republic of the Philippines");
                    model.SetTable("<11000;PROVINCE OF " + AppSettingsManager.GetConfigValue("08"));
                    if (AppSettingsManager.GetConfigObject("01") == "CITY")
                        model.SetTable("<11000;CITY OF" + AppSettingsManager.GetConfigValue("02"));
                    else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                        model.SetTable("<11000;MUNICIPALITY OF" + AppSettingsManager.GetConfigValue("02"));

                    model.SetFontSize(16);
                    model.SetFontBold(1);
                    if (AppSettingsManager.GetConfigObject("01") == "CITY")
                        model.SetTable("<11000;OFFICE OF THE CITY TREASURER");
                    else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                        model.SetTable("<11000;OFFICE OF THE MUNICIPAL TREASURER");

                    model.SetTable("");
                    model.SetTable("");

                    model.SetFontSize(11);

                    model.SetTable("<1100|<500|<9500;DATE|:|" + sCurrentDate);
                    model.SetTable("");
                    model.SetTable("<1100|<500|<9500;TO|:|" + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin)));
                    model.SetFontBold(0);
                    model.SetTable("<1100|<500|<9500;||" + AppSettingsManager.GetBnsName(sBin));
                    model.SetTable("<1100|<500|<9500;||" + AppSettingsManager.GetBnsAddress(sBin));
                    model.SetFontBold(1);
                    model.SetTable("");
                    model.SetTable("<1100|<500|<9500;SUBJECT|:|FINAL NOTICE OF DEMAND");
                    model.SetFontBold(0);

                    model.SetTable("");
                    model.SetTable(string.Format("^10500;_____________________________________________________________________________"));
                    model.SetTable("");

                    model.SetMarginLeft(1200);
                    sContent = "It appears in our records that you have failed to secure the necessary business permit for";
                    sContent += " CY " + AppSettingsManager.GetSystemDate().Year + ". The Business Permits & License Office (BPLO) has issued notices of violations";
                    model.SetTable(string.Format("=9000;{0}", sContent));

                    lngY1 = model.GetCurrentY();
                    sContent = "on ";
                    model.SetTable(string.Format("=9000;{0}", sContent));
                    model.SetFontBold(1);
                    model.SetCurrentY(lngY1);
                    sContent = GetNoticeDate(sBin, 1) + " and on " + GetNoticeDate(sBin, 2) + ".";// + ",  " + GetNoticeDate(sBin, 3);
                    model.SetTable(string.Format("<400|=9000;|{0}", sContent));
                    model.SetFontBold(0);
                    model.SetTable("");

                    result.Query = "select distinct(tax_year) as tax_year, qtr_to_pay  from taxdues where bin = :1 order by tax_year";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            sTaxYear = result.GetString("tax_year").Trim();
                            sQtr = result.GetString("qtr_to_pay");

                            if (sQtr == "1")
                                sQtr = "F";
                            if (sTaxYear != AppSettingsManager.GetSystemDate().Year.ToString())
                            {
                                if (sQtr == "2")
                                    sQtr = "2-4";
                                if (sQtr == "3")
                                    sQtr = "3-4";
                            }

                            // GDE 20120703 insert PL/SQL HERE for COMPUTE_TAX_AND_FEES
                            ComputeTaxAndFees(sBin, sTaxYear);
                            dTotTotal += AppSettingsManager.GetSurchDues(sBin, sTaxYear) + AppSettingsManager.GetPenDues(sBin, sTaxYear) + AppSettingsManager.GetBNSTaxDues(sBin, sTaxYear) + AppSettingsManager.GetRegTaxDues(sBin, sTaxYear);
                        }
                    }
                    result.Close();

                    lngY1 = model.GetCurrentY();
                    sContent = "In view of the above, this letter shall serve as the";
                    model.SetTable(string.Format("=9000;{0}", sContent));
                    model.SetCurrentY(lngY1);
                    model.SetFontUnderline(1);
                    sContent = "final notice of demand";
                    model.SetTable(string.Format("<5100|=9000;|{0}", sContent));
                    model.SetFontUnderline(0);
                    model.SetCurrentY(lngY1);
                    sContent = " and accordingly,";
                    model.SetTable(string.Format("<7500|=9000;|{0}", sContent));

                    lngY1 = model.GetCurrentY();
                    sContent = "final demand is made for you to pay the amount of ";
                    model.SetTable(string.Format("=9000;{0}", sContent));
                    model.SetCurrentY(lngY1);
                    model.SetFontUnderline(1);
                    model.SetFontBold(1);
                    sContent = dTotTotal.ToString("#,##0.00");
                    model.SetTable(string.Format("<5250|=9000;|{0}", sContent));
                    model.SetFontUnderline(0);
                    model.SetFontBold(0);
                    model.SetCurrentY(lngY1);
                    sContent = " corresponding to your tax";
                    model.SetTable(string.Format("<6600|=9000;|{0}", sContent));

                    sContent = "liabilities including penalties and surcharges as reflected in the hereto attached TAX ORDER PAYMENT";
                    model.SetTable(string.Format("=9000;{0}", sContent));

                    model.SetTable("");
                    sContent = "A period of fifteen (15) days is hereby prescribed within which to settle your tax delinquency.";
                    model.SetTable(string.Format("=9000;{0}", sContent));

                    model.SetTable("");
                    sContent = "Please note that failure to comply with this notice within the prescribed period will force us to subject your tax liability";
                    sContent += " to court action in accordance with the applicable provisions in the local tax code.";
                    model.SetTable(string.Format("=9000;{0}", sContent));

                    model.SetTable("");
                    sContent = "For compliance.";
                    model.SetTable(string.Format("=9000;{0}", sContent));
                    model.SetMarginLeft(800);

                    model.SetParagraph("");
                    model.SetParagraph("");
                    model.SetFontBold(1);
                    model.SetTable(string.Format("^10000;{0}", AppSettingsManager.GetConfigValue("05")));
                    model.SetFontBold(0);

                    model.SetTable("^10000;City Treasurer");
                    model.SetTable("");
                    model.SetTable("");
                    model.SetTable("");
                    model.SetTable("<1000;Copy furnished");
                    model.SetTable("<1000;     •   BPLO");
                    model.SetTable("<1000;     •   City Administrator");
                    model.SetTable("<1000;     •   City Legal Officer");
                    model.PageBreak();

                    //model.Next();
                    dTotTotal = 0;
                    dDue = 0;
                    dTax = 0;
                    dFee = 0;
                    dSurch = 0;
                    model.SetFontSize(12);
                    model.SetTable("^11000;Republic of the Philippines");
                    model.SetTable("<11000;PROVINCE OF " + AppSettingsManager.GetConfigValue("08"));
                    model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("02"));
                    model.SetTable("^11000;Office of the City Treasurer");
                    model.SetTable("");
                    model.SetFontSize(15);
                    model.SetFontBold(1);
                    model.SetTable("<11000;Statement of Account");
                    model.SetFontBold(0);
                    model.SetFontSize(11);
                    model.SetTable("<1500|<9500;Balance as of|" + AppSettingsManager.MonthsInWords(dDate));

                    model.SetTable("");
                    model.SetFontBold(0);
                    model.SetFontSize(15);
                    model.SetFontBold(1);
                    model.SetTable("<11000;" + sBin);

                    model.SetFontBold(0);
                    model.SetFontSize(11);
                    model.SetTable("<11000;Business Identification Number");
                    model.SetTable("");

                    model.SetTable("<11000;NAME: " + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin)));
                    model.SetTable("<11000;BUSINESS NAME: " + AppSettingsManager.GetBnsName(sBin));
                    model.SetTable("<11000;OWNER'S ADDRESS: " + AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(sBin)));
                    model.SetTable("<11000;BUSINESS ADDRESS: " + AppSettingsManager.GetBnsAddress(sBin));

                    result.Query = "delete from pay_temp where bin = '" + sBin.Trim() + "'";
                    if (result.ExecuteNonQuery() != 0)
                    {
                    }
                    result.Close();

                    result.Query = "delete from pay_temp_bns where bin = '" + sBin.Trim() + "'";
                    if (result.ExecuteNonQuery() != 0)
                    {
                    }

                    model.SetTable("");
                    model.SetTableBorder(9);
                    model.SetTable("<1000|^1200|^1000|>1500|>1500|>1500|>1500;|TAX YEAR|QTR|TAX|FEES|CHARGES|TOTAL;");
                    model.SetTableBorder(0);
                    model.SetTable("");
                    result.Query = "select distinct(tax_year) as tax_year, qtr_to_pay  from taxdues where bin = :1 order by tax_year";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            sTaxYear = result.GetString("tax_year").Trim();
                            sQtr = result.GetString("qtr_to_pay");

                            if (sQtr == "1")
                                sQtr = "F";
                            if (sTaxYear != AppSettingsManager.GetSystemDate().Year.ToString())
                            {
                                if (sQtr == "2")
                                    sQtr = "2-4";
                                if (sQtr == "3")
                                    sQtr = "3-4";
                            }

                            // GDE 20120703 insert PL/SQL HERE for COMPUTE_TAX_AND_FEES
                            ComputeTaxAndFees(sBin, sTaxYear);
                            dTotTotal += AppSettingsManager.GetSurchDues(sBin, sTaxYear) + AppSettingsManager.GetPenDues(sBin, sTaxYear) + AppSettingsManager.GetBNSTaxDues(sBin, sTaxYear) + AppSettingsManager.GetRegTaxDues(sBin, sTaxYear);

                            dTax += AppSettingsManager.GetBNSTaxDues(sBin, sTaxYear);
                            dFee += AppSettingsManager.GetRegTaxDues(sBin, sTaxYear);
                            dSurch += AppSettingsManager.GetSurchDues(sBin, sTaxYear) + AppSettingsManager.GetPenDues(sBin, sTaxYear);

                            dDue += (AppSettingsManager.GetBNSTaxDues(sBin, sTaxYear) + AppSettingsManager.GetRegTaxDues(sBin, sTaxYear));
                            model.SetTable("<1000|^1200|^1000|>1500|>1500|>1500|>1500;|" + result.GetString("tax_year") + "|" + sQtr + "|" + string.Format("{0:#,##0.#0}", AppSettingsManager.GetBNSTaxDues(sBin, sTaxYear)) + "|" + string.Format("{0:#,##0.#0}", AppSettingsManager.GetRegTaxDues(sBin, sTaxYear)) + "|" + string.Format("{0:#,##0.#0}", AppSettingsManager.GetSurchDues(sBin, sTaxYear) + AppSettingsManager.GetPenDues(sBin, sTaxYear)) + "|" + string.Format("{0:#,##0.#0}", AppSettingsManager.GetSurchDues(sBin, sTaxYear) + AppSettingsManager.GetPenDues(sBin, sTaxYear) + AppSettingsManager.GetBNSTaxDues(sBin, sTaxYear) + AppSettingsManager.GetRegTaxDues(sBin, sTaxYear)) + ";");
                        }
                    }
                    result.Close();

                    model.SetTable("");
                    model.SetFontSize(12);
                    model.SetTableBorder(1);
                    model.SetTable("<2200|^1000|>1500|>1500|>1500|>1500;|TOTAL:|" + string.Format("{0:#,##0.#0}", dTax) + "|" + string.Format("{0:#,##0.#0}", dFee) + "|" + string.Format("{0:#,##0.#0}", dSurch) + "|" + string.Format("{0:#,##0.#0}", dTotTotal) + ";");
                    model.SetTableBorder(0);
                    model.SetTable("");
                    model.SetTable("");
                    model.SetTable("");
                    model.SetTableBorder(9);
                    model.SetTable("<3333|<3333|<3333;Total Amount Due|Surcharge and Interest|Grand Total Due;");
                    model.SetFontSize(14);
                    lngY1 = model.GetCurrentY();
                    model.SetTable(string.Format("<3333|<3333|<3333;P {0}|P {1}|", dDue.ToString("#,##0.00"), dSurch.ToString("#,##0.00")));
                    model.SetFontBold(1);
                    model.SetCurrentY(lngY1);
                    model.SetTable(string.Format("<3333|<3333|<3333;||P {0}", dTotTotal.ToString("#,##0.00")));
                    model.SetFontBold(0);
                    model.SetFontSize(11);
                    model.SetTableBorder(0);
                    model.PageBreak();
                }
            }
            result2.Close();
            PreviewDocu();
        }
        public void Knootsky()
        {
            long lngY1 = 0;
            long lngY2 = 0;
            long lngX1 = 0;
            long lngX2 = 0;
            long termY = 0;
            
            model.Clear();
            model.SetMarginLeft(720);
            model.SetTableBorder(11);
            model.SetTable("^11000;REPORT OF COLLECTIONS AND DEPOSITS;");
            model.SetTableBorder(5);
            model.SetTable("^11000; ");
            model.SetTable("^11000;LGU-APARRI, CAGAYAN;");
            model.SetTable("^11000; ");
            lngX1 = model.GetCurrentX();
            lngY1 = model.GetCurrentY();
            model.SetTableBorder(0);
            model.SetTable("<6000|<5000;Fund     __________________________| Date: ;");
            model.SetTable("<6000|<5000;Name of Accountable Form | Report No.: ;");

            model.SetTableBorder(11);
            model.SetTable("<11000;A. COLLECTIONS;");
            model.SetTableBorder(12);
            model.SetTable("<11000;1. For Collections");
            model.SetTableBorder(11);
            termY = model.GetCurrentY();
            //model.SetTable("^4000; ");
            //model.SetTableBorder(5);
            model.SetTable("^4000;Type (Form No.);");
            model.SetTableBorder(12);
            model.SetTable("^4000; ");
            model.SetTableBorder(9);
            for (int i = 0; i < 9; i++)
            {
                model.SetTable("^4000; ");
            }


            model.SetMarginLeft(4720);
            model.SetCurrentY(termY);
            model.SetTableBorder(9);
            model.SetTable("^4000;Official Receipts / Serial No.;");
            model.SetTable("^2000|^2000;From|To;");
            
            model.SetTableBorder(9);
            for (int i = 0; i < 9; i++)
            {
                model.SetTable("^2000|^2000; |; ");
            }


            model.SetMarginLeft(8720);
            model.SetCurrentY(termY);
            model.SetTableBorder(11);
            model.SetTable("^3000;Amount;");
            model.SetTableBorder(12);
            model.SetTable("^3000; ;");
            model.SetTableBorder(9);
            for (int i = 0; i < 9; i++)
            {
                model.SetTable("^2300|<700; | ; ");
            }



            
            lngY2 = model.GetCurrentY();
            model.DrawLine(lngX1 - 220, lngY1, lngX1 - 220, lngY2); // RIGHT
            model.DrawLine(720, lngY1, 720, lngY2); // LEFT

            model.SetMarginLeft(720);
            model.SetTableBorder(9);
            model.SetTable("<11000;2.For Liquidating Officers/Treasurers");
            model.SetTable("^6000|^2000|^2300|^700;  DENOMINATIONS                 NO. OF PIECES                 AMOUNT|REPORT NO.|AMOUNT| ;");
            for (int i = 0; i < 6; i++)
            {
                model.SetTable("^6000|^2000|^2300|^700; | | | ; ");
            }
            model.SetTable("<6000|^2000|^2300|^700;               COINS:| | | ;");
            model.SetTable("^6000|^2000|^2300|^700; | | | ; ");
            model.SetTable("<11000;B.REMITTANCES / DEPOSITS");
            model.SetTable("^6000|^2000|^2300|^700;Accountable Officer/Bank|Reference|AMOUNT| ;");
            for (int i = 0; i < 4; i++)
            {
                model.SetTable("^6000|^2000|^2300|^700; | | | ; ");
            }
            model.SetTable("<11000;C.ACCOUNTABILITY FOR ACCOUNTABLE FORMS");
            termY = model.GetCurrentY();
            model.SetTableBorder(11);
            model.SetTable("^1500;Name of Form & No.;");
            model.SetTableBorder(12);
            model.SetTable("^1500; ");
            model.SetTableBorder(9);
            for (int i = 0; i < 10; i++)
            {
                model.SetTable("^1500; ");
            }
            model.SetCurrentY(termY);
            model.SetMarginLeft(2220);
            model.SetTable("^2500;Beginning Balance;");
            model.SetTable("^2500;Qty. Inclusive Serial Nos.;");
            model.SetTable("^2500;From     To;");
            for (int i = 0; i < 9; i++)
            {
                model.SetTable("^2500; ");
            }

            model.SetCurrentY(termY);
            model.SetMarginLeft(4720);
            model.SetTable("^2500;Receipt;");
            model.SetTable("^2500;Qty. Inclusive Serial Nos.;");
            model.SetTable("^2500;From     To;");
            for (int i = 0; i < 9; i++)
            {
                model.SetTable("^2500; ");

            }
            model.SetCurrentY(termY);
            model.SetMarginLeft(7220);
            model.SetTable("^2500;Issued;");
            model.SetTable("^2500;Qty. Inclusive Serial Nos.;");
            model.SetTable("^2500;From     To;");
            for (int i = 0; i < 9; i++)
            {
                model.SetTable("^2500; ");
            }
            model.SetCurrentY(termY);
            model.SetMarginLeft(9720);
            model.SetTable("^2000;Ending Balance;");
            model.SetTable("^2000;Qty. Inclusive Serial Nos.;");
            model.SetTable("^2000;From     To;");
            for (int i = 0; i < 9; i++)
            {
                model.SetTable("^2000; ");
            }
            model.SetTableBorder(0);
            model.SetMarginLeft(720);
            lngY1 = model.GetCurrentY();
            model.SetTable("<5000|<6000;D.SUMMARY OF COLLECTIONS & REMITTANCES / DEPOSITS|List of Checks;");
            model.SetTable("");
            termY = model.GetCurrentY();
            model.SetTable("<5000|^2000|^2000|^2000;Beginning Balance| | |;");
            model.SetTable("");
            model.SetTable("<6000; Add: Collection");
            model.SetTable("");
            model.SetTable("<500|<10500; |Cash;");
            model.SetTable("");
            model.SetTable("<500|<10500; |Checks;");
            model.SetTable("");
            model.SetTable("<100|<10900; |Less:Remittance/Deposits to Cashier/Treasury?Depository Bank;");
            model.SetTable("");
            model.SetTable("<500|<10500; |Balance;");

            model.SetCurrentY(termY);
            model.SetMarginLeft(5720);
            model.SetTableBorder(1);
            model.SetTable("<2000|<2000|<2000;Check No.|Payee|Amount;");
            model.SetTableBorder(11);
            model.SetTable("<6000; ");
            model.SetTableBorder(5);
            model.SetTable("<6000; ");
            model.SetTable("<6000; ");
            model.SetTable("<6000; ");
            model.SetTable("<6000; ");
            model.SetTable("<6000; ");
            model.SetTable("<6000; ");
            model.SetTable("<6000; ");
            model.SetTable("<6000; ");
            model.SetTable("<6000; ");
            model.SetTable("<6000; ");
            lngY2 = model.GetCurrentY();
            model.DrawLine(720, lngY1, 720, lngY2);
            model.DrawLine(11720, lngY1, 11720, lngY2);
            model.SetTableBorder(11);
            model.SetMarginLeft(720);
            // CERTIFICATIONS
            model.SetTable("<5000|<6000;CERTIFICATION:|VERIFICATIONS;");
            model.SetTableBorder(5);
            model.SetTable("<5000|<6000; | ;");
            model.SetTable("=5000|=6000;I hereby certify that the foregoing report of collections and deposits, and accountability for accountable forms is true and correct. | I hereby certify that the report of collections and has been verified and acknowledge receipt of ____ Php ____ ;");
            model.SetTable("<5000|<6000; | ;");
            model.SetTable("<5000|<6000; | ;");
            model.SetTable("<5000|<6000; | ;");
            model.SetTableBorder(12);
            model.SetTable("<11000; ");

            PreviewDocu();

        }

        private void ComputeTaxAndFees(string sBin, string sTaxYear)
        {
            //OraclePLSQL plsqlCmd = new OraclePLSQL();
            string m_sRevYear = string.Empty;
            string m_sIsQtrly = string.Empty;
            string sStat = string.Empty;
            //string sTaxYear = string.Empty;
            string m_sNextQtrToPay = string.Empty;
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

            
            int iMultiYear = 0;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            result.Query = "select count(distinct tax_year) as iCount from taxdues where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    iMultiYear = result.GetInt("iCount");
                }
            }
            result.Close();

            
            // insert permit update transaction here

            result.Query = "select * from business_que where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    dtDateOperated = result.GetDateTime("dt_operated");
                    sDateOperated = string.Format("{0:MM/dd/yyyy}", dtDateOperated);
                    sYearOperated = sDateOperated.Substring(6, 4);
                    sStat = result.GetString("bns_stat").Trim();
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
                            sStat = result.GetString("bns_stat").Trim();
                        }
                    }
                }
            }
            result.Close();

            if (sStat.Trim() == "REN")
                m_sIsQtrly = "Y";

            if (sStat.Trim() == "RET")
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'X' order by tax_year asc";
            else
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' order by tax_year DESC";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sDueState = result.GetString("due_state");
                    //sTaxYear = result.GetString("tax_year");
                }
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
            plsqlCmd.ParamValue = sStat;
            plsqlCmd.AddParameter("m_sStatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sTaxYear;
            plsqlCmd.AddParameter("m_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = iMultiYear;
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

        }
        public void SOAold(string sBin, string sTermPay, string sQtrToPay)
        {
            long lngY1 = 0;
            long lngY2 = 0;
            model.Clear();
            sList.ReturnValueByBin = sBin;
            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                model.SetFontName("Arial Narrow");
                model.SetTable("");
                model.SetTable("");

                string sYear, sTerm, sQrt, sTmpGross = string.Empty, sGross = string.Empty, sParticular, sDue, sSurch, sInt, sTotal;
                double dDue, dSurch, dInt, dTotal, dGross;
                double dGrandTotDue = 0, dGrandTotSurch = 0, dGrandTotInt = 0, dGrandTotTotal = 0;
                string sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal;

                model.SetFontSize(12);
                model.SetTable("^11000;REPUBLIC OF THE PHILIPPINES");
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    model.SetTable("^11000;CITY OF" + AppSettingsManager.GetConfigValue("02"));
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    model.SetTable("^11000;MUNICIPALITY OF" + AppSettingsManager.GetConfigValue("02"));
                model.SetTable("");
                model.SetTable("");
                model.SetFontSize(20);
                model.SetTable("^11000;S T A T E M E N T  O F  A C C O U N T");
                model.SetFontSize(10);
                model.SetTable("");
                model.SetTable("");
                model.SetTable("");
                lngY1 = model.GetCurrentY();
                model.DrawLine(500, lngY1, 12000, lngY1);
                
                //model.SetTable(string.Format(sContent));
                //result.Query = "select * from soa_tbl where bin = '" + sBin + "' and term = '" + sTermPay + "' and qtr_to_pay = '" + sQtrToPay + "' order by fees_code_sort desc, qtr asc";
                result.Query = "select * from soa_tbl where bin = '" + sBin + "' order by fees_code_sort desc, qtr asc";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        sYear = result.GetString("year").Trim();
                        sTerm = result.GetString("term").Trim();
                        sQrt = result.GetString("qtr").Trim();
                        //sGross = result.GetString("gross").Trim();
                        sParticular = result.GetString("particulars").Trim();
                        //                    sDue = result.GetDouble("due").ToString();

                        double.TryParse(result.GetDouble("gross").ToString(), out dGross);
                        double.TryParse(result.GetDouble("due").ToString(), out dDue);
                        double.TryParse(result.GetDouble("surch").ToString(), out dSurch);
                        double.TryParse(result.GetDouble("pen").ToString(), out dInt);
                        double.TryParse(result.GetDouble("total").ToString(), out dTotal);
                        dGrandTotDue = dGrandTotDue + dDue;
                        dGrandTotSurch = dGrandTotSurch + dSurch;
                        dGrandTotInt = dGrandTotInt + dInt;
                        dGrandTotTotal = dGrandTotTotal + dTotal;

                        sGross = string.Format("{0:#,##0.00}", dGross);
                        sDue = string.Format("{0:#,##0.00}", dDue);
                        sSurch = string.Format("{0:#,##0.00}", dSurch);
                        sInt = string.Format("{0:#,##0.00}", dInt);
                        sTotal = string.Format("{0:#,##0.00}", dTotal);

                        if (sGross == "0.00")
                            sGross = "";


                        //model.SetTable(string.Format("^400|^200|^200|>900|<1700|>1200|>1200|>1200|>1200;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", sYear, sTerm, sQrt, sGross, sParticular, sTerm, sTerm, sTerm, sTerm));
                        model.SetTable(string.Format("<600|^200|^200|>900|<2500|>1500|>1500|>1500|>1500;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8};", sYear, sTerm, sQrt, sGross, sParticular, sDue, sSurch, sInt, sTotal));
                    }
                }
                result.Close();
                sGrandTotDue = string.Format("{0:#,##0.00}", dGrandTotDue);
                sGrandTotSurch = string.Format("{0:#,##0.00}", dGrandTotSurch);
                sGrandTotInt = string.Format("{0:#,##0.00}", dGrandTotInt);
                sGrandTotTotal = string.Format("{0:#,##0.00}", dGrandTotTotal);
                model.SetTable(string.Format(">4400|>1500|>1500|>1500|>1500;TOTAL|{0}|{1}|{2}|{3};", sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal));
                model.SetTable("");
                model.SetTable(string.Format(">4400|>1500|>1500|>1500|>1500;GRAND TOTAL|{0}|{1}|{2}|{3};", sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal));
                PreviewDocu();
            }
        }
        public void SOAold2(string sBin, string sTermPay, string sQtrToPay)
        {
            //MessageBox.Show(dDate.ToShortDateString());
            long lngY1 = 0;
            long lngY2 = 0;
            long lngX1 = 0;
            long lngX2 = 0;
            long termY = 0;

            // for last paymeny info
            string sLastOrDate = string.Empty;
            string sLastTaxYear = string.Empty;
            string sLastPaymentTerm = string.Empty;
            string sLastPaymentMode = string.Empty;
            string sLastTeller = string.Empty;
            string sLastStat = string.Empty;
            string sLastQtr = string.Empty;

            model.Clear();
            
            //model.LeftMargin = 200;
            sList.ReturnValueByBin = sBin;
            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                model.SetFontName("Arial");
                model.SetTable("");
                model.SetTable("");

                string sYear, sTerm, sQrt, sTmpGross = string.Empty, sGross = string.Empty, sParticular, sDue, sSurch, sInt, sTotal;
                double dDue, dSurch, dInt, dTotal, dGross;
                double dGrandTotDue = 0, dGrandTotSurch = 0, dGrandTotInt = 0, dGrandTotTotal = 0;
                string sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal, sGrandTotTotal1;
                string sTerm1 = string.Empty;
                string sTaxCredit = string.Empty;
                string sTellerPos = string.Empty;
                double dTaxCredit = 0;

                if(sTermPay == "F")
                    sTerm1 = "FULL YEAR";
                else
                    if (sQtrToPay.Trim() == "1")
                        sTerm1 = "QTRLY-1st Qtr";
                    if (sQtrToPay.Trim() == "2")
                        sTerm1 = "QTRLY-2nd Qtr";
                    if (sQtrToPay.Trim() == "3")
                        sTerm1 = "QTRLY-3rd Qtr";
                    if (sQtrToPay.Trim() == "4")
                        sTerm1 = "QTRLY-4th Qtr";

                model.SetFontSize(12);
                model.SetTable("^11000;Republic of the Philippines"); 
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    model.SetTable("^11000;CITY OF" + AppSettingsManager.GetConfigValue("02"));
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    model.SetTable("^11000;MUNICIPALITY OF" + AppSettingsManager.GetConfigValue("02"));
                model.SetTable("");
                model.SetFontSize(10);
                //model.SetTable(">10310;TERM");
                model.SetTable(">8600|^2400;|TERM");
                model.SetFontSize(15);
                model.SetFontBold(1);
                model.SetTable("<11000;Statement of Account");
                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetTable("<1300|<9700;Balance as of|" + AppSettingsManager.MonthsInWords(dDate));
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 10;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
                model.SetFontSize(15);
                model.SetFontBold(1);
                lngY1 = model.GetCurrentY();
                model.SetCurrentY(2100);
                model.SetTable(">8600|^2400;|" + sTerm1.Trim());
                model.SetCurrentY(lngY1);
                model.SetTable("");
                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetFontSize(15);
                model.SetFontBold(1);
                model.SetTable("<11000;" + sBin);

                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetTable("<11000;Business Identification Number");
                model.SetTable("");
                
                model.SetFontSize(8);
                model.SetTable(">11000;BILL NUMBER & DATE:   " + AppSettingsManager.GetBillNoAndDate(sBin, sSOATaxYear, sList.BPLSAppSettings[i].sBnsCode.Trim()));
                model.SetFontBold(1);
                model.SetTableBorder(0);
                lngY1 = model.GetCurrentY();
                model.DrawLine(700, lngY1, 11800, lngY1);
                model.SetTable("");

                model.SetTable("<1800|<4000|<1400|<4000;BUSINESS NAME|: " + sList.BPLSAppSettings[i].sBnsNm.Trim() + "|DESCRIPTION|: " + AppSettingsManager.GetBnsDesc(sList.BPLSAppSettings[i].sBnsCode.Trim()));
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.SetTable("<1800|<4000|<1400|<2000|<1000|<1000;BUSINESS ADDRESS|: " + AppSettingsManager.GetBnsAddress(sBin.Trim()) + "|STATUS|: " + sList.BPLSAppSettings[i].sBnsStat.Trim() + "|TAX YEAR|: " + sSOATaxYear.Trim());
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[5].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontStyle = 0;
                model.SetTable("<1800|<4000|<1400|<2000|<1000|<1000;BUSINESS OWNER|: " + AppSettingsManager.GetBnsOwner(sList.BPLSAppSettings[i].sOwnCode.Trim()) + "|LAST PAYMENT|: P " + AppSettingsManager.GetLastPaymentInfo(sBin, "AMOUNT") + "|LAST OR#|: " + AppSettingsManager.GetLastPaymentInfo(sBin, "OR"));
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[5].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontStyle = 0;
                model.SetTable("<1800|<4000|<1400|<2000|<1000|<1000;LAST PERMIT NO|: " + sList.BPLSAppSettings[i].sPermitNo + "|DATE ISSUED|: ||");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.SetTable("");
                lngY2 = model.GetCurrentY();
                model.DrawLine(700, lngY2, 11800, lngY2);
                model.DrawLine(700, lngY1, 700, lngY2);
                model.DrawLine(11800, lngY1, 11800, lngY2);
                model.SetTableBorder(0);
                model.SetFontBold(0);
                model.SetFontName("Arial Narrow");
                model.SetFontSize(8);
                model.SetTable("");
                lngY1 = model.GetCurrentY();
                model.DrawLine(700, lngY1, 11800, lngY1);
                model.SetTable("^600|^200|^200|<2500|>1300|>1500|>1500|>1500|>1500;YEAR|||PARTICULARS|GROSS / CAPITAL|DUE|SURCH|INT|TOTAL");
                lngY2 = model.GetCurrentY();
                model.DrawLine(700, lngY2, 11800, lngY2);
                model.DrawLine(700, lngY1, 700, lngY2);
                model.DrawLine(11800, lngY1, 11800, lngY2);


                // TERM
                model.DrawLine(9300, 1400, 11800, 1400); // TOP
                model.DrawLine(9300, 2700, 11800, 2700); // BOTTOM
                model.DrawLine(9300, 1800, 11800, 1800); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(9300, 1400, 9300, 2700); // LEFT
                model.DrawLine(11800, 1400, 11800, 2700); // RIGHT
                // TERM
                model.SetTable("");
                //model.SetTable(string.Format(sContent));
                //result.Query = "select * from soa_tbl where bin = '" + sBin + "' and term = '" + sTermPay + "' and qtr_to_pay = '" + sQtrToPay + "' order by fees_code_sort desc, qtr asc";
                result.Query = "select * from soa_tbl where bin = '" + sBin + "' order by fees_code_sort desc, qtr asc";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        sYear = result.GetString("year").Trim();
                        sTerm = result.GetString("term").Trim();
                        sQrt = result.GetString("qtr").Trim();
                        //sGross = result.GetString("gross").Trim();
                        sParticular = result.GetString("particulars").Trim();
                        //                    sDue = result.GetDouble("due").ToString();

                        double.TryParse(result.GetDouble("gross").ToString(), out dGross);
                        double.TryParse(result.GetDouble("due").ToString(), out dDue);
                        double.TryParse(result.GetDouble("surch").ToString(), out dSurch);
                        double.TryParse(result.GetDouble("pen").ToString(), out dInt);
                        double.TryParse(result.GetDouble("total").ToString(), out dTotal);
                        dGrandTotDue = dGrandTotDue + dDue;
                        dGrandTotSurch = dGrandTotSurch + dSurch;
                        dGrandTotInt = dGrandTotInt + dInt;
                        dGrandTotTotal = dGrandTotTotal + dTotal;

                        sGross = string.Format("{0:#,##0.00}", dGross);
                        sDue = string.Format("{0:#,##0.00}", dDue);
                        sSurch = string.Format("{0:#,##0.00}", dSurch);
                        sInt = string.Format("{0:#,##0.00}", dInt);
                        sTotal = string.Format("{0:#,##0.00}", dTotal);

                        if (sGross == "0.00")
                            sGross = "";


                        //model.SetTable(string.Format("^400|^200|^200|>900|<1700|>1200|>1200|>1200|>1200;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", sYear, sTerm, sQrt, sGross, sParticular, sTerm, sTerm, sTerm, sTerm));
                        model.SetTable(string.Format("^600|^200|^200|<2500|>1300|>1500|>1500|>1500|>1500;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8};", sYear, sTerm, sQrt, sParticular, sGross, sDue, sSurch, sInt, sTotal));
                    }
                }
                result.Close();

                // tax credit
                //result.Query = "select * from dbcr_memo where own_code in (select own_code from businesses where bin = '" + sBin.Trim() + "') and memo like 'REMAINING BALANCE AFTER PAYMENT%' and served = 'N'"; // GDE 20111021
                result.Query = "select * from dbcr_memo where bin = '" + sBin.Trim() + "' and memo like 'REMAINING BALANCE AFTER PAYMENT%' and served = 'N'"; //JARS 20171010
                if(result.Execute())
                {
                    if(result.Read())
                    {
                        dTaxCredit = result.GetDouble("credit");
                                               
                    }
                }
                result.Close();
                // tax credit
                sGrandTotDue = string.Format("{0:#,##0.00}", dGrandTotDue);
                sGrandTotSurch = string.Format("{0:#,##0.00}", dGrandTotSurch);
                sGrandTotInt = string.Format("{0:#,##0.00}", dGrandTotInt);
                sTaxCredit = string.Format("{0:#,##0.00}", dTaxCredit);
                sGrandTotTotal = string.Format("{0:#,##0.00}", dGrandTotTotal);
                sGrandTotTotal1 = string.Format("{0:#,##0.00}", dGrandTotTotal - dTaxCredit);
                

                model.SetTable("");
                lngY1 = model.GetCurrentY();
                model.DrawLine(5500, lngY1, 11800, lngY1);
                model.SetTable(string.Format(">4800|>1500|>1500|>1500|>1500;Sub-Total:|{0}|{1}|{2}|{3};", sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal));
                model.SetTable(string.Format(">4800|>1500|>1500|>1500|>1500;Less Available Tax Credit:||||{0};", sTaxCredit));

                model.SetTable("");
                model.SetTable("");

                lngY1 = model.GetCurrentY();
                // TOTAL DUE
                model.SetFontSize(8);
                model.SetFontName("Arial");
                model.DrawLine(700, lngY1, 3200, lngY1); // TOP
                model.SetTable("<2500|<2500|<3600|<2500;   Total Amount Due|   Surcharge and Interest|   Less Available Tax Credit|   Grand Total Due");
                model.DrawLine(700, lngY1 + 900, 3200, lngY1 + 900); // BOTTOM
                model.DrawLine(700, lngY1 + 200, 3200, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(700, lngY1, 700, lngY1 + 900); // LEFT
                model.DrawLine(3200, lngY1, 3200, lngY1 + 900); // RIGHT
                
                // SURCH AND INTEREST
                model.DrawLine(3200, lngY1, 5700, lngY1); // TOP
                model.DrawLine(3200, lngY1 + 900, 5700, lngY1 + 900); // BOTTOM
                model.DrawLine(3200, lngY1 + 200, 5700, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(3200, lngY1, 3200, lngY1 + 900); // LEFT
                model.DrawLine(5700, lngY1, 5700, lngY1 + 900); // RIGHT
                model.SetTable("");
                //model.SetFontSize(12);
                //model.SetFontBold(1);
                model.SetTable("^2500|^2500|^2500|<1100|^2500;P " + sGrandTotDue + "|P " + sGrandTotSurch + "|P " +sTaxCredit + "||P " + sGrandTotTotal);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontSize = 14;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontStyle = 1;
                //model.SetFontSize(8);
                //model.SetFontBold(0);
                // TAX CREDIT
                model.DrawLine(5700, lngY1, 8200, lngY1); // TOP
                model.DrawLine(5700, lngY1 + 900, 8200, lngY1 + 900); // BOTTOM
                model.DrawLine(5700, lngY1 + 200, 8200, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(5700, lngY1, 5700, lngY1 + 900); // LEFT
                model.DrawLine(8200, lngY1, 8200, lngY1 + 900); // RIGHT
                // GRAND TOTAL
                model.DrawLine(9300, lngY1, 11800, lngY1); // TOP
                model.DrawLine(9300, lngY1 + 900, 11800, lngY1 + 900); // BOTTOM
                model.DrawLine(9300, lngY1 + 200, 11800, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(9300, lngY1, 9300, lngY1 + 900); // LEFT
                model.DrawLine(11800, lngY1, 11800, lngY1 + 900); // RIGHT

                model.SetTable("");
                model.SetTable("");
                model.SetTable("");
                model.SetTable("");
                lngY1 = model.GetCurrentY();
                
                
                model.DrawLine(700, lngY1 + 200, 11800, lngY1 + 200); // TOP
                model.SetCurrentY(lngY1 + 400);
                model.SetFontBold(1);
                model.SetFontUnderline(1);
                model.SetTable("<11000; Additional Details of Previous Payment");
                model.SetFontBold(0);
                model.SetFontUnderline(0);

                result.Query = "select distinct * from pay_hist where bin = '" + sBin.Trim() + "' order by or_date desc, tax_year desc";
                if(result.Execute())
                {
                    if(result.Read())
                    {
                        sLastOrDate = result.GetDateTime("or_date").ToShortDateString();
                        sLastTaxYear = result.GetString("tax_year").Trim();
                        sLastQtr = result.GetString("qtr_paid").Trim();
                        sLastPaymentTerm = result.GetString("payment_term").Trim();
                        sLastPaymentMode = result.GetString("data_mode").Trim();
                        sLastTeller = result.GetString("teller").Trim();
                        sLastStat = result.GetString("bns_stat").Trim();
                    }
                }
                result.Close();

                if (sLastQtr.Trim() == "F")
                    sLastQtr = "FULL";
                else
                {
                    if (sLastQtr == "1")
                        sLastQtr = "1st Qtr";
                    if (sLastQtr == "2")
                        sLastQtr = "2nd Qtr";
                    if (sLastQtr == "3")
                        sLastQtr = "3rd Qtr";
                    if (sLastQtr == "4")
                        sLastQtr = "4th Qtr";
                }

                if (sLastPaymentTerm.Trim() == "F")
                    sLastPaymentTerm = "Full Payment";
                else
                    sLastPaymentTerm = "Installment";

                if (sLastPaymentMode.Trim() == "POS")
                    sLastPaymentMode = "Posting";
                else if (sLastPaymentMode.Trim() == "UNP")
                    sLastPaymentMode = "For Posting";
                else if (sLastPaymentMode.Trim() == "OFL")
                    sLastPaymentMode = "Offline Payment";
                else if (sLastPaymentMode.Trim() == "ONL")
                    sLastPaymentMode = "Online Payment";

                if (sLastStat.Trim() == "NEW")
                    sLastStat = "New Business";
                else if (sLastStat.Trim() == "REN")
                    sLastStat = "Renewal";
                else
                    sLastStat = "Retired Business";

                if (m_objSystemUser.Load(sLastTeller.Trim()))
                {
                    sLastTeller = m_objSystemUser.UserName;
                }
                
                model.SetTable("<1500|<10000; O.R. Date|: " + sLastOrDate);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Tax Year / Qtr|: " + sLastTaxYear + " - " + sLastQtr);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Payment Term|: " + sLastPaymentTerm);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Payment Mode|: " + sLastPaymentMode);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Teller|: " + sLastTeller);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Business Status|: " + sLastStat);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.DrawLine(700, lngY1 + 1800 , 11800, lngY1 + 1800); // BOTTOM LINE

                if (m_objSystemUser.Load(AppSettingsManager.SystemUser.UserCode))
                {
                    sTellerName = m_objSystemUser.UserName;
                    sTellerPos = m_objSystemUser.Position;
                }
                lngY1 = model.GetCurrentY();
                model.SetCurrentY(lngY1 + 600);
                // GDE 20101110 city treas name temporary hard coded
                model.SetTable("<1500|=2000|<4000|<1500|^2000;Prepared by:|" + string.Format("{0:0}", sTellerName) + "||Approved By:| ELVIRA M. REYES");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontStyle = 9;
                //string sDate = string.Empty;
                
                model.SetTable("<1500|^2000|<4000|<1500|^2000;|" + sTellerPos + "|||City Treasurer");
                model.SetTable("<1500|<2000|<4000|<1500|^2000;Date Printed:|" + dDate.ToShortDateString() + "   " + dDate.ToLongTimeString() + "|||");
                model.SetTable("");
                
                model.SetFontSize(6);
                model.SetTable("<100|<800|<9200;»|NOTE|:This Statement is valid until (PLACE VALID UNTIL DATE HERE)");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 6;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 1;
                model.SetTable("<100|<800|=9200;»|IMPORTANT|:Please inform us of any error in this statement of account such as misspelled names, incomplete address, etcetera.  We are in the process of computerizing our taxpayer services for your future convenience.  Thank you very much.");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 6;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 1;
                //lngY1 = model.GetCurrentY();
                //sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal
               
                


                
                PreviewDocu();
            }
        }
        public void PayHistPrint(string sBin)
        {
            string sDate = string.Empty;
            string sOrNo = string.Empty;
            string sTxYr = string.Empty;
            string sCapital = string.Empty;
            string sGross = string.Empty;
            string sTerm = string.Empty;
            string sQtr = string.Empty;
            string sTax = string.Empty;
            string sFees = string.Empty;
            string sSurPen = string.Empty;
            string sTotal = string.Empty;

            long lngY1 = 0;
            long lngY2 = 0;
            model.Clear();
            sImagepath = "C:\\Documents and Settings\\Godfrey\\My Documents\\Downloads\\logo_header.jpg";
            //sImagepath = "D:\\Rachel\\Projects\\Mati\\SourceCodes_fr_Godfrey\\_Latest 20131219\\aBPLs-e\\BPS\\BPS\\Resources\\logo-mati.png";
            model.SetFontName("Arial");
            model.SetFontSize(10);
            model.SetTable("^11000;Republic of the Philippines");
            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                model.SetTable("^11000;CITY OF" + AppSettingsManager.GetConfigValue("02"));
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                model.SetTable("^11000;MUNICIPALITY OF" + AppSettingsManager.GetConfigValue("02"));
            model.SetTable("");
            // RMC 20170130 added signatory in Payment History of BTAS (s)
            if (AppSettingsManager.GetSystemType == "C")
            {
                if (AppSettingsManager.GetConfigValue("01") == "CITY")
                    model.SetTable("^11000;OFFICE OF THE CITY TREASURER");
                else
                    model.SetTable("^11000;OFFICE OF THE MUNICIPAL TREASURER");
            }
            else
                model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("41")); 
            // RMC 20170130 added signatory in Payment History of BTAS (e)
            
            //model.SetTable("^11000;Business Permits & Licensing Office");
            model.SetTable("");
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetTable("<11000;Payment History");
            model.SetFontBold(0);
            model.SetFontSize(8);
            model.SetTable("<1700|<8800;Available Record as of|" + AppSettingsManager.MonthsInWords(dDate));
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetTable("");
            model.SetTable("<11000;" + sBin);
            model.SetFontBold(0);
            model.SetFontSize(8);
            model.SetTable("<11000;Business Identification Number");
            model.SetTable("");
            model.SetTable("");
            lngY1 = model.GetCurrentY();
            model.DrawLine(700, lngY1, 11500, lngY1);
            model.SetTable("");
            model.SetTable("<1700|<9300;   Business Name|: " + AppSettingsManager.GetBnsName(sBin.Trim()));;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            model.SetTable("<1700|<9300;   Address|: " + AppSettingsManager.GetBnsAdd(sBin.Trim(), ""));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            model.SetTable("<1700|<9300;   Owner's Name|: " + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin.Trim())));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            model.SetTable("<1700|<9300;   Address|: " + AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetOwnCode(sBin.Trim())));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            
            //MCR 20150129 (s)
            model.SetTable("<1700|<9300;   Business Plate|: " + AppSettingsManager.GetBnsPlate(sBin.Trim()));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            //MCR 20150129 (e)

            model.SetTable("");
            lngY2 = model.GetCurrentY();            
            model.DrawLine(700, lngY2, 11500, lngY2);
            lngY2 = model.GetCurrentY();
            //model.DrawLine(700, lngY2, 11800, lngY2);
            model.DrawLine(700, lngY1, 700, lngY2);
            model.DrawLine(11500, lngY1, 11500, lngY2);
            //model.DrawLine(11800, lngY1, 11800, lngY2);
            model.SetTable("");
            model.SetTable("");
            model.SetFontSize(8);
            model.SetFontName("Arial Narrow");
            lngY1 = model.GetCurrentY();
            model.DrawLine(700, lngY1, 11500, lngY1);
            model.SetFontBold(1);
            //model.SetTable("^800|^1000|^800|^1200|^1200|^600|^400|^1200|^1200|^1200|^1200;DATE|OR#|TAX YEAR|CAPITAL|GROSS|TERM|QTR|TAX|FEES|SUR/PEN|TOTAL");
            model.SetTable("^800|^1000|^800|^1200|^1200|^600|^400|^800|^1000|^1000|^1000|^1000;DATE|OR#|TAX YEAR|CAPITAL|GROSS|TERM|QTR|NO. OF QTR|TAX|FEES|SUR/PEN|TOTAL");    // RMC 20130109 display no. of qtr in payment hist
            model.SetFontBold(0);
            lngY2 = model.GetCurrentY();
            model.DrawLine(700, lngY2, 11500, lngY2);
            model.DrawLine(700, lngY1, 700, lngY2);
            model.DrawLine(11500, lngY1, 11500, lngY2);
            model.SetTable("");

            string sNoOfQtr = string.Empty; // RMC 20130109 display no. of qtr in payment hist
            result.Query = "select distinct * from pay_hist_temp where bin = '" + sBin.Trim() + "' order by or_date asc, tax_year, qtr asc"; // RMC 20140114 added sorting by tax year in payment hist
            if(result.Execute())
            {
                while(result.Read())
                {
                    sDate = result.GetDateTime("or_date").ToShortDateString();
                    sOrNo = result.GetString("or_no");
                    sTxYr = result.GetString("tax_year");
                    sTerm = result.GetString("term");
                    sQtr = result.GetString("qtr");
                    sTax = string.Format("{0:#,##0.00}", result.GetDouble("tax"));
                    sFees = string.Format("{0:#,##0.00}", result.GetDouble("fees"));
                    sSurPen = string.Format("{0:#,##0.00}", result.GetDouble("surch_pen"));
                    sTotal = string.Format("{0:#,##0.00}", result.GetDouble("total"));
                    sNoOfQtr = GetNoOfQtr(sOrNo);   // RMC 20130109 display no. of qtr in payment hist

                    sCapital = AppSettingsManager.GetCapitalGross(sBin.Trim(), AppSettingsManager.GetBnsCodeMain(sBin.Trim()), sTxYr.Trim(), "NEW");
                    sGross = AppSettingsManager.GetCapitalGross(sBin.Trim(), AppSettingsManager.GetBnsCodeMain(sBin.Trim()), sTxYr.Trim(), "REN");
                    //model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|>1200|>1200|>1200|>1200;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", sDate, sOrNo, sTxYr, sCapital, sGross, sTerm, sQtr, sTax, sFees, sSurPen, sTotal));
                    model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sDate, sOrNo, sTxYr, sCapital, sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal));  // RMC 20130109 display no. of qtr in payment hist
                }
            }
            result.Close();
            model.SetTable("");
            lngY2 = model.GetCurrentY();
            model.DrawLine(700, lngY2, 11500, lngY2);
            model.SetTable("");
            model.SetTable("");
            model.SetTable("");

            //JAV 20170803(s)
            string strCurrentDate = string.Empty;
            string strCurrentTime = string.Empty;
            string strUser = string.Empty;
            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            strCurrentTime = string.Format("{0:hh:mm:ss tt}", AppSettingsManager.GetCurrentDate());
            strUser = AppSettingsManager.SystemUser.UserCode;
            model.SetTable("<1100|^500|<9400;DATE PRINTED|:|" + strCurrentDate);
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.SetTable("<1100|^500|<9400;TIME PRINTED|:|" + strCurrentTime);
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.SetTable("<1100|^500|<9400;PRINTED BY|:|" + strUser);
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //JAV 20170803(e)

            //JARS 20170831 REM
            //// RMC 20170130 added signatory in Payment History of BTAS (s)
            //if (AppSettingsManager.GetSystemType == "C")
            //{
            //    model.SetTable("");
            //    model.SetTable("");
            //    model.SetFontBold(1);
            //    model.SetTable(string.Format("<7800|^3000;|{0}", AppSettingsManager.GetConfigValue("05")));
            //    model.SetFontBold(0);
            //    model.SetTable(string.Format("<7800|^3000;|{0}", AppSettingsManager.GetConfigValue("01") + " TREASURER"));
            //}
            //// RMC 20170130 added signatory in Payment History of BTAS (e)

            /*if(AppSettingsManager.WithAddlBns(sBin.Trim()))
            {
                model.SetFontBold(1);
                model.SetTable("<10000;ADDITIONAL BUSINESS INFORMATION");
                model.SetTable("");
                model.SetTable("^800|^2000|^800|^800|^1500|^1500;DATE|ADDITIONAL BUSINESS|TAX YEAR|STATUS|CAPITAL|GROSS;");
                model.SetFontBold(0);
                lngY2 = model.GetCurrentY();
                model.DrawLine(700, lngY2, 8100, lngY2);
                model.SetTable("");

            }*/

        }
        public void Reassessment(string sBin)
        {
            int iReass = i_PageCount;
            i_Curr+= 1;
            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            string sBnsPin = string.Empty;
            string sBldgCode = string.Empty;
            string sContent = string.Empty;
            model.SetFontItalic(0);
            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());

            //CreateHeader(); // gde 20120504

            pRec.Query = "select * from btm_gis_loc where bin = '" + sBin + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sBnsPin = pRec.GetString("land_pin");
                    sBldgCode = pRec.GetString("bldg_code");
                    sBnsPin = "LAND PIN: " + sBnsPin + "-1" + sBldgCode;
                }

            }
            pRec.Close();

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");

            model.SetFontSize(10);
            model.SetParagraph("");
            model.SetTable(string.Format(">10000;{0}", strCurrentDate));
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetMarginLeft(1000);
            model.SetTable(string.Format("<10000;{0}", AppSettingsManager.GetBnsName(sBin)));
            model.SetTable(string.Format("<10000;{0}", AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin))));
            model.SetTable(string.Format("=10000;{0}", AppSettingsManager.GetBnsAdd(sBin, "")));
            model.SetTable(string.Format("<10000;{0}", sBnsPin));
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("<10000;Sir/Madam:"));
            model.SetParagraph("");
            model.SetParagraph("");

            sContent = "\tPlease be advised that based on the records of this office, your establishment is among the list ";
            sContent += "of businesses whose permits are subject for reassessment.  Accordingly, we are requiring the submission ";
            sContent += "of your audited, BIR-stamped FINANCIAL STATEMENT to facilitate our computation of your business taxes. ";
            sContent += "In case your business has several branches in different areas, please make certain that the financial ";
            sContent += "statement is broken down by vicinity/branch to reflect the gross sales of your establishment in the Calamba area.";
            model.SetTable(string.Format("=10500;{0}", sContent));

            model.SetParagraph("");
            model.SetParagraph("");
            sContent = "\tYour failure to comply with this requirement would compel our office to revoke your business ";
            sContent += "permit and refer this matter to our City Legal Office for their proper disposition and action.";
            model.SetTable(string.Format("=10500;{0}", sContent));

            model.SetParagraph("");
            sContent = "\tWe look forward to your prompt response and utmost cooperation on this matter.";
            model.SetTable(string.Format("=10500;{0}", sContent));

            model.SetParagraph("");
            sContent = "\tFor your immediate compliance.";
            model.SetTable(string.Format("=10500;{0}", sContent));
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable("=10500;Very truly yours,");
            model.SetParagraph("");
            model.SetTable(string.Format("<10000;;;{0}", AppSettingsManager.GetConfigValue("34")));
            model.SetTable("=10500;Head - Business Permits and Tricycle Franchising Office");


            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable("=10500;Received by: ____________________________");
            model.SetParagraph("");
            model.SetTable("=10500;Delivered by: ____________________________");
            model.SetFontItalic(1);
            model.SetFontSize(8); // GDE 20120516 as per jester
            //model.SetTable(string.Format("<10000;;;For inquiries, please contact us at (049) 545-6789 loc. 8101-8102"));
            model.SetFontSize(10); // GDE 20120516 as per jester
            if (i_Curr < i_PageCount)
                model.PageBreak();

        }
        public void FPCert(string sBin, int iFrmt) //MCR ADD FullPayment 20140520 0=year,1=qtr 
        {
            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            string sBnsNm = string.Empty;
            string sBnsAdd = string.Empty;
            string sBnsOwner = string.Empty;
            string sTaxYear = string.Empty;
            string sGender = string.Empty;
            string sOwnCode = string.Empty;
            DateTime dDate = DateTime.Now;
            string sContent = string.Empty;
            model.SetFontItalic(0);
            //strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());

            //CreateHeader(); // gde 20120504

            sBnsNm = AppSettingsManager.GetBnsName(sBin);
            sBnsAdd = AppSettingsManager.GetBnsAddress(sBin);
            sBnsOwner = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));

            if (sOrAmt.Trim() == "" || sOrAmt == "0.00" || sOrAmt == "0")
            {
                result.Query = "select sum(fees_amtdue) as amt from or_table where or_no = '" + sOrNo + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        try
                        {
                            sOrAmt = string.Format("{0:#,##0.#0}", result.GetDouble("amt"));
                        }
                        catch
                        {
                            sOrAmt = "0.00";
                        }
                    }
                }
                result.Close();
            }

            sOwnCode = AppSettingsManager.GetBnsOwnCode(sBin);
            sGender = AppSettingsManager.GetBnsOwnerGender(sOwnCode);

            if (sGender == "MALE")
                sGender = "MR. ";
            else if (sGender == "FEMALE")
                sGender = "MS./MRS. ";
            else
                sGender = "";

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("^10500;Republic of the Philippines"));
            model.SetTable(string.Format("^10500;Province of " + AppSettingsManager.GetConfigObject("08")));
            model.SetTable(string.Format("^10500;" + AppSettingsManager.GetConfigObject("02")));
            model.SetFontSize(12);
            model.SetTable(string.Format("^10500;OFFICE OF THE CITY TREASURER"));
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetFontItalic(1);
            model.SetTable("^10500;C E R T I F I C A T I O N");
            model.SetFontItalic(0);
            model.SetFontSize(10);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetFontSize(10);
            model.SetParagraph("");
            model.SetTable("<10000;TO WHOM IT MAY CONCERN:");
            model.SetFontBold(0);
            model.SetParagraph("");
            model.SetParagraph("");

            int iOut = 0;
            int iQtr = 0;

            if (int.TryParse(AppSettingsManager.GetCurrQtrPaid(sBin), out iOut))
                iQtr = Convert.ToInt16(AppSettingsManager.GetCurrQtrPaid(sBin));

            if (iFrmt == 0 || iQtr == 4)
            {
                sContent = "\tTHIS IS TO CERTIFY that, per records the business tax, license and other regulatory fees of " + sGender + sBnsOwner + " have been paid for the";
                sContent += " whole year ending December 31," + sFPTaxYear + " which engaged in " + sBnsNm + ", located at " + sBnsAdd + ". It is";
                sContent += " futher certified on the payment stated below:";
                model.SetTable(string.Format("=10500;{0}", sContent));
            }
            else // 1
            {
                sContent = "\tTHIS IS TO CERTIFY that, per our records, business tax, license, other regulatory fees, and prior years delinquencies, have been paid as of ";
                sContent += iQtr+DateSuffix(iQtr) + " quarter ending " + QtrEndDate(iQtr) + ", on the business owned/ operated by " + sGender + sBnsOwner + " as " + sBnsNm + ", located at " + sBnsAdd + ". It is";
                sContent += " futher certified on the payment stated below:";
                model.SetTable(string.Format("=10500;{0}", sContent));
            }

            model.SetParagraph("");
            model.SetParagraph("");

            int iCnt = 0;
            double iTax = 0, iFees = 0, iSurch = 0, iTotal = 0;
            model.SetTableBorder(4);
            model.SetTable(string.Format("^1000|^1000|^1000|^500|^500|^700|^1300|^1300|^1300|^1600;Year|O.R. No.|Date|Term|Qtr|No. of Qtr|Tax|Fees|Surch/Int|Total"));
            model.SetTableBorder(0);
            model.SetParagraph("");
            pRec.Query = "select distinct * from pay_hist_temp where bin = '" + sBin.Trim() + "' and tax_year = '" + sFPTaxYear + "' order by tax_year,or_no, or_date,term,qtr";
            if (pRec.Execute())
            {
                while(pRec.Read())
                {
                    iCnt++;
                    iTax += pRec.GetDouble("tax");
                    iFees += pRec.GetDouble("fees");
                    iSurch += pRec.GetDouble("surch_pen");
                    iTotal += pRec.GetDouble("total");
                    model.SetTable(string.Format(">1000|>1000|>1000|^500|^500|^700|>1300|>1300|>1300|>1600;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", pRec.GetString("tax_year").Trim(), pRec.GetString("or_no").Trim(), pRec.GetDateTime("or_date").ToShortDateString(), pRec.GetString("term").Trim(), pRec.GetString("qtr").Trim(), GetNoOfQtr(pRec.GetString("or_no").Trim()), pRec.GetDouble("tax").ToString("#,##0.00"), pRec.GetDouble("fees").ToString("#,##0.00"), pRec.GetDouble("surch_pen").ToString("#,##0.00"), pRec.GetDouble("total").ToString("#,##0.00")));
                }
            }
            pRec.Close();

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format(">4700|>1300|>1300|>1300|>1600;GRAND TOTAL|{0}|{1}|{2}|{3}", iTax.ToString("#,##0.00"), iFees.ToString("#,##0.00"), iSurch.ToString("#,##0.00"), iTotal.ToString("#,##0.00")));

            for (int i = iCnt; i < 4; i++)
            {
                model.SetParagraph("");
            }

            model.SetParagraph("");
            sContent = "\tThis certification is being issued upon the request of " + sGender + sBnsOwner + " for whatever legal purpose it may serve him/her best";
            model.SetTable(string.Format("=10500;{0}", sContent));

            model.SetParagraph("");
            if (sCeasedDate == String.Empty)
                //sContent = "\tIssued this " + Convert.ToDateTime(sOrDate).Day + DateSuffix(Convert.ToDateTime(sOrDate).Day) + " day of " + Convert.ToDateTime(sOrDate).ToString("MMMM, yyyy") + " at City of Mati, Province of Davao Oriental.";
                sContent = "\tIssued this " + Convert.ToDateTime(sOrDate).Day + DateSuffix(Convert.ToDateTime(sOrDate).Day) + " day of " + Convert.ToDateTime(sOrDate).ToString("MMMM, yyyy") + " at " + AppSettingsManager.GetConfigValue("09") + ", PROVINCE OF " + AppSettingsManager.GetConfigValue("08") + ".";
            else
                //sContent = "\tIssued this " + Convert.ToDateTime(sAcceptedDate).Day + DateSuffix(Convert.ToDateTime(sAcceptedDate).Day) + " day of " + Convert.ToDateTime(sAcceptedDate).ToString("MMMM, yyyy") + " at City of Mati, Province of Davao Oriental.";
                sContent = "\tIssued this " + Convert.ToDateTime(sAcceptedDate).Day + DateSuffix(Convert.ToDateTime(sAcceptedDate).Day) + " day of " + Convert.ToDateTime(sAcceptedDate).ToString("MMMM, yyyy") + " at " + AppSettingsManager.GetConfigValue("09") + ", PROVINCE OF " + AppSettingsManager.GetConfigValue("08") + ".";
            model.SetTable(string.Format("=10500;{0}", sContent));

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format(">10130;{0}", AppSettingsManager.GetConfigValue("53")));
            model.SetTable(string.Format(">10500;{0}", AppSettingsManager.GetConfigValue("54")));//BPTFO, Department Head              
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable("=10500;Paid under:");
            model.SetParagraph("");
            model.SetTable("=10500;O.R. No. " + sOrNo);
            model.SetTable("=10500;Date: " + sOrDate);
            model.SetTable("=10500;Amount: P" + Convert.ToDecimal(sOrAmt).ToString("#,##0.00"));

            model.PageBreak();
        }
        public void Jameson()
        {
            model.SetTable(string.Format("^10500;Republic of the Philippines"));
            model.PageBreak();
        }
        public void RetCert(string sBin)
        {
            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            string sBnsNm = string.Empty;
            string sBnsAdd = string.Empty;
            string sBnsOwner = string.Empty;
            string sTaxYear = string.Empty;
            string sGender = string.Empty;
            string sOwnCode = string.Empty;
            string sNoticeCertDate = string.Empty;
            DateTime dDate = DateTime.Now;
            string sContent = string.Empty;
            model.SetFontItalic(0);
            //strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());

            //CreateHeader(); // gde 20120504

            pRec.Query = "select * from RETIRED_BNS where bin = '" + sBin + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sBnsNm = AppSettingsManager.GetBnsName(sBin);
                    sBnsAdd = AppSettingsManager.GetBnsAddress(sBin);
                    sBnsOwner = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));
                    sTaxYear = pRec.GetString("tax_year").Trim();
                    dDate = pRec.GetDateTime("APPRVD_DATE");
                    sNoticeCertDate = AppSettingsManager.MonthsInWords(dDate);
                }
            }
            pRec.Close();

            if (sOrNo == "")
            {
                pRec.Query = "select distinct * from pay_hist where bin = '" + sBin + "' and bns_stat = 'RET'";
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        sOrNo = pRec.GetString("or_no").Trim();
                        sOrDate = pRec.GetDateTime("or_date").ToShortDateString();
                    }

                }
                pRec.Close();
            }

            if (sOrAmt.Trim() == "" || sOrAmt == "0.00" || sOrAmt == "0")
            {
                result.Query = "select sum(fees_amtdue) as amt from or_table where or_no = '" + sOrNo + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        try
                        {
                            sOrAmt = string.Format("{0:#,##0.#0}", result.GetDouble("amt"));
                        }
                        catch
                        {
                            sOrAmt = "0.00";
                        }
                    }
                }
                result.Close();
            }
            sOwnCode = AppSettingsManager.GetBnsOwnCode(sBin);
            sGender = AppSettingsManager.GetBnsOwnerGender(sOwnCode);

            if (sGender == "MALE")
                sGender = "MR. ";
            else if (sGender == "FEMALE")
                sGender = "MS./MRS. ";
            else
                sGender = "";

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("^10500;Republic of the Philippines"));
            model.SetTable(string.Format("^10500;Province of " + AppSettingsManager.GetConfigObject("08")));
            model.SetTable(string.Format("^10500;" + AppSettingsManager.GetConfigObject("02")));
            model.SetFontSize(12);
            model.SetTable(string.Format("^10500;OFFICE OF THE CITY TREASURER"));
            model.SetFontSize(10);
            model.SetTable(string.Format("^10500;_________________________________________________________________________________________________________________________"));
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable("^10500;C E R T I F I C A T I O N");
            model.SetFontSize(10);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetFontSize(10);
            model.SetParagraph("");
            model.SetTable("<10000;TO WHOM IT MAY CONCERN:");
            model.SetFontBold(0);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");

            model.SetParagraph("");
            sContent = "\tTHIS IS TO CERTIFY that according to records of this Office, " + sGender + sBnsOwner + " engaged in business of " + sBnsNm;
            sContent += " located at " + sBnsAdd;
            model.SetTable(string.Format("=10500;{0}", sContent));

            model.SetParagraph("");
            sContent = "\tFurther certify that letter of intent for closure/retirement of business dated ";
            if (sIssuedDate != String.Empty)
                sContent += Convert.ToDateTime(sIssuedDate).ToLongDateString();
            else
                sContent += dDate.ToLongDateString().Trim();
            sContent += " made by the Applicant has been accepted/approved by the Office of the City Administrator effective ";
            if (sCeasedDate != String.Empty)
                sContent += Convert.ToDateTime(sCeasedDate).ToString("MMMM dd, yyyy");
            else
                sContent += Convert.ToDateTime(sOrDate).ToString("MMMM dd, yyyy");
            model.SetTable(string.Format("=10500;{0}", sContent));

            model.SetParagraph("");
            sContent = "\tThis certification is being issued upon the request of " + sGender + sBnsOwner + " for whatever legal purpose it may serve him/her best";
            model.SetTable(string.Format("=10500;{0}", sContent));

            model.SetParagraph("");
            //sContent = "\tIssued this " + Convert.ToDateTime(sOrDate).Day + DateSuffix(Convert.ToDateTime(sOrDate).Day) + " day of " + Convert.ToDateTime(sOrDate).ToString("MMMM, yyyy") + " at City of Mati, Province of Davao Oriental.";
            sContent = "\tIssued this " + Convert.ToDateTime(sOrDate).Day + DateSuffix(Convert.ToDateTime(sOrDate).Day) + " day of " + Convert.ToDateTime(sOrDate).ToString("MMMM, yyyy") + " at " + AppSettingsManager.GetConfigValue("09") + ", PROVINCE OF " + AppSettingsManager.GetConfigValue("08") + ".";
            model.SetTable(string.Format("=10500;{0}", sContent));

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format(">10130;{0}", AppSettingsManager.GetConfigValue("53")));
            model.SetTable(string.Format(">10500;{0}", AppSettingsManager.GetConfigValue("54")));//BPTFO, Department Head              
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable("=10500;Paid under:");
            model.SetParagraph("");
            model.SetTable("=10500;O.R. No. " + sOrNo);
            model.SetTable("=10500;Date: " + sOrDate);
            model.SetTable("=10500;Amount: P" + Convert.ToDecimal(sOrAmt).ToString("#,##0.00"));

            model.PageBreak();
        }
        public void RetCertLUBAO(string sBin)
        {
            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            string sBnsNm = string.Empty;
            string sBnsAdd = string.Empty;
            string sBnsOwner = string.Empty;
            string sTaxYear = string.Empty;
            string sGender = string.Empty;
            string sOwnCode = string.Empty;
            string sNoticeCertDate = string.Empty;
            DateTime dDate = DateTime.Now;
            string sContent = string.Empty;
            model.SetFontItalic(0);

            sOwnCode = AppSettingsManager.GetBnsOwnCode(sBin);
            sGender = AppSettingsManager.GetBnsOwnerGender(sOwnCode);

            if (sGender == "MALE")
                sGender = "MR. ";
            else if (sGender == "FEMALE")
                sGender = "MS./MRS. ";
            else
                sGender = "";

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("^10500;Republic of the Philippines"));
            model.SetTable(string.Format("^10500;PROVINCE OF " + AppSettingsManager.GetConfigObject("08")));
            model.SetTable(string.Format("^10500;" + AppSettingsManager.GetConfigObject("01") + " OF " + AppSettingsManager.GetConfigObject("02")));
            model.SetFontSize(12);
            model.SetParagraph("");
            model.SetParagraph("");
            if (AppSettingsManager.GetConfigObject("01").Contains("MUNICI"))
                model.SetTable(string.Format("^10500;OFFICE OF THE MUNICIPAL TREASURER"));
            else
                model.SetTable(string.Format("^10500;OFFICE OF THE CITY TREASURER"));
            model.SetFontSize(10);
            model.SetTable(string.Format("^10500;__________________________________________________________________________________________________________________________"));
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable("^10500;C E R T I F I C A T I O N");
            model.SetFontSize(10);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetFontSize(10);
            model.SetParagraph("");
            model.SetTable("<1130|<10000;|TO WHOM IT MAY CONCERN:");
            model.SetFontBold(0);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");

            model.SetParagraph("");
            sContent = "\tThis is to certify as per records on file, the business establishment known as " + AppSettingsManager.GetBnsName(sBin);
            // RMC 20151005 mods in retirement module (s)
            if (!m_bFullRetirement)
                sContent += " and business line/s " + GetRetBns(sBin) + "";
            // RMC 20151005 mods in retirement module (e)
            sContent += ", operated and owned by " + sGender + AppSettingsManager.GetBnsOwner(sOwnCode) + " located at " + AppSettingsManager.GetBnsAddress(sBin) + ", " + AppSettingsManager.GetConfigValue("08");
            sContent += ", ceased to operate since " + Convert.ToDateTime(sCeasedDate).ToString("MMMM dd, yyyy") + " under O.R. # " + sOrNo;
            model.SetTable(string.Format("<1130|=8500;|{0}", sContent));

            model.SetParagraph("");
            model.SetParagraph("");
            sContent = "\tIssued this " + Convert.ToDateTime(sIssuedDate).Day + DateSuffix(Convert.ToDateTime(sIssuedDate).Day) + " day of " + Convert.ToDateTime(sIssuedDate).ToString("MMMM, yyyy");
            sContent += " upon request of the aboved-named person for any legal intents and purpose.";
            model.SetTable(string.Format("<1130|=8500;|{0}", sContent));

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("<8000|^2130;|{0}", AppSettingsManager.GetConfigValue("05")));
            if (AppSettingsManager.GetConfigObject("01").Contains("MUNICI"))
                model.SetTable(string.Format("<8000|^2130;|Municipal Treasurer"));
            else
                model.SetTable(string.Format("<8000|^2130;|City Treasurer"));
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("<1130|^2130;|" + AppSettingsManager.SystemUser.UserName));
            model.SetTable(string.Format("<1130|^2130;|" + AppSettingsManager.SystemUser.Position));
        }
        public static string DateSuffix(int day) //MCR ADD DateOrdinal 20140515
        {
            Math.DivRem(day, 10, out day);
            switch (day)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
        public string QtrEndDate(int iQtr)
        {
            string sDate = String.Empty;
            DateTime today = AppSettingsManager.GetSystemDate();

            if (iQtr == 1)
                iQtr = 3;
            else if (iQtr == 2)
                iQtr = 6;
            else if (iQtr == 3)
                iQtr = 9;
            else //4
                iQtr = 12;

            DateTime endOfMonth = new DateTime(today.Year, iQtr, 1).AddMonths(1).AddDays(-1);
            return endOfMonth.ToString("MMMM dd, yyyy");
        }

        public void Special(string sBnsCode, string sFeesCode)
        {
            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            string sBnsNm = string.Empty;
            string sBin = string.Empty;
            string sBnsOwner = string.Empty;
            string sTaxYear = string.Empty;
            string sNoticeCertDate = string.Empty;
            DateTime dDate = DateTime.Now;
            string sContent = string.Empty;
            string sOrNo = string.Empty;
            string sOrAmt = string.Empty;
            string sOrDate = string.Empty;
            model.SetFontItalic(0);
            double dTotal = 0;
            //strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            StringBuilder strValues = new StringBuilder();
            //CreateHeader(); // gde 20120504
            string strCurrentDirectory = Directory.GetCurrentDirectory();
                    using (SaveFileDialog dlg = new SaveFileDialog()) //fix some issue on stream saving //JVL 09122008mal
                    {
                        
                        dlg.Title = "Export to EXCEL";
                        dlg.Filter = "EXCEL documents (*.xls)|*.xls";
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            int iCnt = 0;
                            using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                                {
                                    model.SetTableBorder(9);
                                    model.SetTable("^4500|^4000|^2000;BUSINESS NAME|BUSINESS TYPE|ANNUAL INSP FEE;");
                                    if (sBnsCode == string.Empty)
                                        sBnsCode = "%";
                                     if (sFeesCode == string.Empty)
                                        sFeesCode = "%";
                                     else
                                        sFeesCode = AppSettingsManager.GetFeesCodeByDesc(sFeesCode.Trim(), "AD");

                                    pRec.Query = "select * from businesses where bns_code like '" + sBnsCode + "%' order by bns_nm";
                                    if (pRec.Execute())
                                    {
                                        while (pRec.Read())
                                        {
                                            iCnt++;
                                            sBin = pRec.GetString("bin").Trim();
                                            sBnsNm = pRec.GetString("bns_nm");
                                            sBnsCode = pRec.GetString("bns_code");
                                            pRec2.Query = "select sum(fees_amtdue) as fees from or_table where fees_code like '" + sFeesCode + "' and or_no in (select distinct(or_no) from pay_hist where bin = '" + sBin.Trim() + "')";
                                            if (pRec2.Execute())
                                            {
                                                if (pRec2.Read())
                                                {
                                                    try
                                                    {
                                                        dTotal += pRec2.GetDouble("fees");
                                                        sOrAmt = string.Format("{0:#,##0.#0}", pRec2.GetDouble("fees"));
                                                    }
                                                    catch
                                                    {
                                                        sOrAmt = "0.00";
                                                    }
                                                }
                                            }
                                            pRec2.Close();

                                            pRec2.Query = "select bns_desc from bns_table where bns_code = '" + sBnsCode + "' and fees_code = 'B'";
                                            if (pRec2.Execute())
                                            {
                                                if (pRec2.Read())
                                                {
                                                    sTaxYear = pRec2.GetString("bns_desc");
                                                }
                                            }
                                            pRec2.Close();

                                            model.SetTable("<4500|<4000|>2000;" + sBnsNm + "|" + sTaxYear + "|" + sOrAmt + ";");

                                            strValues.Append(sBnsNm + "  -  " + sTaxYear + "  -  " + sOrAmt + "\r");

                                            //ADD MCR 2014305 QA insert header on next page (s)
                                            if (iCnt == 78)
                                            {
                                                iCnt = 0;
                                                model.PageBreak();
                                                model.SetTableBorder(9);
                                                model.SetTable("^4500|^4000|^2000;BUSINESS NAME|BUSINESS TYPE|ANNUAL INSP FEE;");
                                            }
                                            //ADD MCR 2014305 QA insert header on next page (e)
                    // GDE 20120822
                                        }
                                    }
                                    pRec.Close();
                                    streamWriter.WriteLine(strValues);
                                }
                            }
                        }
                    }
                    Directory.SetCurrentDirectory(strCurrentDirectory); //JVL fix the resetting of xml menus // JVL 09122008mal

            model.SetTableBorder(0);
            model.SetTable("");
            model.SetTableBorder(9);
            model.SetFontBold(1);
            model.SetTable(">8500|>2000;TOTAL|" + string.Format("{0:#,##0.#0}", dTotal) + ";");
            model.SetFontBold(0);
            
            model.PageBreak();

        }
        public void PrintNoticeOnQue(string sBin, string sInspectorName, DateTime dtInspected, string sNoticeNum)
        {
            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            string sBnsPin = string.Empty;
            string sBldgCode = string.Empty;
            string sContent = string.Empty;
            model.SetFontItalic(0);
            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());

            //CreateHeader(); // gde 20120504

            pRec.Query = "select * from btm_gis_loc where bin = '" + sBin + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sBnsPin = pRec.GetString("land_pin");
                    sBldgCode = pRec.GetString("bldg_code");
                    sBnsPin = "BLDG PIN: " + sBnsPin + "-1" + sBldgCode;
                }

            }
            pRec.Close();

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");

            model.SetFontSize(10);
            model.SetParagraph("");
            model.SetTable(string.Format(">10000;{0}", strCurrentDate));
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetMarginLeft(1000);
            model.SetTable(string.Format("<10000;{0}", AppSettingsManager.GetBnsName(sBin)));
            model.SetTable(string.Format("<10000;{0}", AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin))));
            model.SetTable(string.Format("=10000;{0}", AppSettingsManager.GetBnsAdd(sBin, "")));
            model.SetTable(string.Format("<10000;{0}", sBnsPin));
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("<10000;Dear Sir/Madam:"));
            model.SetParagraph("");
            model.SetParagraph("");

            if (sNoticeNum == "1")
            {


                
                sContent = "\tAn occular inspection conducted on your establishment by an inspector of this";
                sContent += " office revealed that you are operating:";
                model.SetTable(string.Format("=10500;{0}", sContent));

                model.SetParagraph("");
                //sContent = "\t" + m_strRemarks;
                // model.SetTable(string.Format("=10500;{0}", sContent));
                //sContent = "\t" + m_strAddlRemarks;
                //model.SetTable(string.Format("=10500;{0}", sContent));

                model.SetParagraph("");
                sContent = "\tIn view thereof, you are hereby given THREE (3) DAYS from receipt";
                sContent += "hereof to settle/ comply with the above noted deficiencies.";
                model.SetTable(string.Format("=10500;{0}", sContent));

                model.SetParagraph("");
                sContent = "\tWe look forward to your prompt response and utmost cooperation on this matter.";
                model.SetTable(string.Format("=10500;{0}", sContent));

                model.SetParagraph("");
                sContent = "\tKindly disregard this notice if compliance has already been made.";
                model.SetTable(string.Format("=10500;{0}", sContent));
                model.PageBreak();
            }
            else
            {
                model.SetFontItalic(0);
                string sInspector = string.Empty;
                string sDateRcd = string.Empty;
                result.Query = "select * from inspector_details where bin = '" + sBin + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sInspector = AppSettingsManager.GetInspector(result.GetString("inspector_code").Trim());
                        //sDateRcd = DateTime.Parse(result.GetString("date_inspected")).ToLongDateString();
                    }
                }
                result.Close();
                result.Query = "select * from official_notice_closure where bin = '" + sBin + "' and notice_number = '1'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        //sInspector = AppSettingsManager.GetInspector(result.GetString("inspector_code").Trim());
                        sDateRcd = result.GetDateTime("notice_date").ToLongDateString();
                    }
                }
                result.Close();
                model.SetMarginLeft(1000);
                //sContent = "Per inspection conducted by our License Inspector Mr./Ms. " + sInspector + " on " + sDateRcd + ", we have "; // GDE 20120731 as per Jester
                sContent = "Per inspection conducted by our field inspectors on " + sDateRcd + ", we have ";
                sContent += "confirmed that your business, located at the above-stated address, is operating WITHOUT THE REQUIRED BUSINESS PERMIT / ";
                sContent += "WITH PAYMENT DELINQUENCIES from the city government. These findings are noted in the inspection notice issued ";
                sContent += "to you on the same date. Such operational activity is deemed in violation of the ";
                sContent += "" + AppSettingsManager.GetConfigValue("02") + " Tax Code which states that any business operation is ";
                sContent += "considered illegal in the absence of a permit from this office.";
                model.SetTable(string.Format("=10000;{0}", sContent));
                //this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);
                model.SetParagraph("");
                model.SetFontItalic(1);
                model.SetFontSize(8);
                sContent = "Batay sa inspeksyon na isinagawa ng aming inspektor na si G./Bb. " + sInspector + " noong " + sDateRcd + ", aming ";
                sContent += "napatunayan na ang inyong negosyo, na makikita sa pahatirang sulat na nakatala sa itaas, ay patuloy sa operasyon nito, ";
                sContent += "sa kabila ng KAWALAN NG KINAKAILANGANG PAHINTULOT KALAKAL / PAGKABIGONG BAYARAN ANG NAKATAKDANG BUWIS mula sa ating ";
                sContent += "Pamahalaang Panlungsod.  Nais naming ipabatid na ito ay isang paglabag sa kautusang Bayan ukol sa Pagbubuwis ng Lungsod ";
                sContent += "ng Calamba, na nagsasaad ng anumang operasyon ng negosyo ay maituturing na iligal kung walang kaukulang pahintulot mula ";
                sContent += "sa aming tanggapan.";
                model.SetTable(string.Format("=12500;{0}", sContent));

                model.SetFontItalic(0);
                model.SetFontSize(10);
                model.SetParagraph("");
                sContent = "In view thereof, you are STERNLY REMINDED to secure a business permit within five (5) days upon receipt ";
                sContent += "hereof. Your failure to do so would constrain this office to come out with an order for the immediate ";
                sContent += "CLOSURE of your business. This is without prejudice to the filing of appropriated charges in court ";
                sContent += "against you. Please consider this as our FINAL NOTICE.";
                model.SetTable(string.Format("=10000;{0}", sContent));
                model.SetParagraph("");
                model.SetFontItalic(1);
                model.SetFontSize(8);
                sContent = "Kaugnay nito, kayo ay MAHIGPIT NA PINAALALAHANAN na kumuha ng Pahintulot Kalakal sa loob ng limang (5) araw mula sa ";
                sContent += "inyong pagtanggap ng abisong ito.  Ang inyong pagkabigong sumunod sa kautusang ito ay magtutulak sa amin upang ";
                sContent += "magpalabas ng Kauutusan upang Ipasara ang Inyong Negosyo.  Ang kautusang ito ay hindi magiging sagabal sa posibilidad na ";
                sContent += "kayo ay mahainan ng kaso sa husgado kung kinakailangan.  Ang liham na ito ay ang aming HULING ABISO.";
                model.SetTable(string.Format("=12500;{0}", sContent));
                model.SetFontItalic(0);
                model.SetFontSize(10);
                model.SetParagraph("");
                sContent = "For your information.";
                model.SetTable(string.Format("=10000;{0}", sContent));
                model.SetFontItalic(1);
                model.SetFontSize(8);
                sContent = "Para sa inyong kaalaman.";
                model.SetTable(string.Format("=12500;{0}", sContent));
                model.SetFontItalic(0);
                model.SetFontSize(10);

                model.SetParagraph("");
                model.SetParagraph("");
                model.SetMarginLeft(8000);
                model.SetTable(string.Format("<4000;;;Lubos na sumainyo,"));

                model.SetParagraph("");
                //model.SetTable(string.Format("<4000;;;{0}", AppSettingsManager.GetConfigValue("43"))); // GDE 20120504 temp hard coded
                model.SetTable(string.Format("<4000;;;MANUEL M. DOMINGO"));
                model.SetTable(string.Format("<4000;Head, Pinunong Inspector"));

                model.SetParagraph("");
                model.SetParagraph("");
                model.SetParagraph("");
                model.SetMarginLeft(1000);
                model.SetTable(string.Format("<10000;;;Binigyang Pansin:"));
                model.SetParagraph("");
                model.SetParagraph("");
                model.SetTable(string.Format("<10000;;;{0}", AppSettingsManager.GetConfigValue("03")));
                model.SetTable(string.Format("<10000;Hepe - {0}", AppSettingsManager.GetConfigValue("41")));



                model.SetCurrentY(14500);
                model.SetFontItalic(1);
                model.SetFontSize(8); // GDE 20120516 as per jester
                model.SetTable(string.Format("<10000;;;Note: Please disregard this letter if you already have a Business Permit issued by the " + AppSettingsManager.GetConfigValue("02") + " Business Permits & Licensing Office"));
                model.SetFontSize(10); // GDE 20120516 as per jester
                model.PageBreak();
            }
           
        }
        public void PayHistDetails(string sOrNo, string sBin, string sQtrPaid, string p_sTaxYear) //MOD MCR Added TaxYear 20150104
        {
            string sBnsStat = string.Empty;
            string sTerm = string.Empty;
            string sQtr = string.Empty;
            string sTax = string.Empty;
            string sFees = string.Empty;
            string sSurPen = string.Empty;
            string sTotal = string.Empty;
            string sTaxYear = string.Empty;
            string sOrDate = string.Empty;
            string sUser = string.Empty;
            string sTeller = string.Empty;
            string sEncodeDate = string.Empty;
            string sFeesDesc = string.Empty;
            string sFeesCode = string.Empty;
            string sMode = string.Empty;
            string sPayType = string.Empty;
            string sTotSurB = string.Empty; //MCR 20150204
            string sTotPenB = string.Empty; //MCR 20150204


            double dTotFeeB = 0;
            double dTotAmtFeeB = 0;
            double dTotSurB = 0;
            double dTotPenB = 0;
            double dTotAmtB = 0;
            double dTotTotAmtB = 0;
            double dTotSurPenB = 0;

            double dTotFeeF = 0;
            double dTotAmtFeeF = 0;
            double dTotSurF = 0;
            double dTotPenF = 0;
            double dTotAmtF = 0;
            double dTotTotAmtF = 0;
            double dTotSurPenF = 0;

            double dTotal = 0;
            bool withOtherOR = false;
            long lngY1 = 0;
            long lngY2 = 0;
            model.Clear();
            sImagepath = "C:\\Documents and Settings\\Godfrey\\My Documents\\Downloads\\logo_header.jpg";
            model.SetFontName("Arial");
            model.SetFontSize(10);
            model.SetTable("^11000;Republic of the Philippines");
            //model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("02"));
            // RMC 20150501 QA BTAS (s)
            string sProv = AppSettingsManager.GetConfigValue("08").Trim();
            if(sProv != "")
                model.SetTable("^11000;PROVINCE OF " + sProv);
            model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("09"));
            // RMC 20150501 QA BTAS (e)
            model.SetTable("");
            //model.SetTable("^11000;Business Permits & Licensing Office");
            // RMC 20170130 added signatory in Payment History of BTAS (s)
            if (AppSettingsManager.GetSystemType == "C")
            {
                if(AppSettingsManager.GetConfigValue("01") == "CITY")
                    model.SetTable("^11000;OFFICE OF THE CITY TREASURER");
                else
                    model.SetTable("^11000;OFFICE OF THE MUNICIPAL TREASURER");
            }// RMC 20170130 added signatory in Payment History of BTAS (e)
            else
                model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("41"));    // RMC 20111006 Merged viewing of Payment hist in BPS from BTAS
            model.SetTable("");
            model.SetTable("");
            model.SetTable("");
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetTable("<11000;Payment History Details");
            model.SetFontBold(0);
            model.SetFontSize(8);
            model.SetTable("<1700|<8800;Available Record as of|" + AppSettingsManager.MonthsInWords(dDate));
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetTable("");
            model.SetTable("<11000;" + sBin);
            model.SetFontBold(0);
            model.SetFontSize(8);
            model.SetTable("<11000;Business Identification Number");
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetTable("");
            model.SetTable("<11000;" + sOrNo.Trim());
            model.SetFontBold(0);
            model.SetFontSize(8);
            model.SetTable("<11000;Official Receipt Number");
            model.SetTable("");
            model.SetTable("");
            lngY1 = model.GetCurrentY();
            model.DrawLine(700, lngY1, 11500, lngY1);
            model.SetTable("");
            model.SetTable("<1700|<9300;   Business Name|: " + AppSettingsManager.GetBnsName(sBin.Trim())); ;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            model.SetTable("<1700|<9300;   Address|: " + AppSettingsManager.GetBnsAdd(sBin.Trim(), ""));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            model.SetTable("<1700|<9300;   Owner's Name|: " + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin.Trim())));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            model.SetTable("<1700|<9300;   Address|: " + AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetOwnCode(sBin.Trim())));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

            //MCR 20150129 (s)
            model.SetTable("<1700|<9300;   Business Plate|: " + AppSettingsManager.GetBnsPlate(sBin.Trim()));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            //MCR 20150129 (e)

            //MCR 20150204 (s)
            string sTempTY = AppSettingsManager.GetAppNo(sBin.Trim(), p_sTaxYear);
            if (sTempTY.Trim() != "")
            {
                model.SetTable("<1700|<9300;   Application No.|: " + AppSettingsManager.GetAppNo(sBin.Trim(), p_sTaxYear));
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            }
            //MCR 20150204 (e)

            model.SetTable("");
            lngY2 = model.GetCurrentY();
            model.DrawLine(700, lngY2, 11500, lngY2);
            lngY2 = model.GetCurrentY();
            //model.DrawLine(700, lngY2, 11800, lngY2);
            model.DrawLine(700, lngY1, 700, lngY2);
            model.DrawLine(11500, lngY1, 11500, lngY2);
            //model.DrawLine(11800, lngY1, 11800, lngY2);
            model.SetTable("");
            model.SetTable("");
            model.SetFontSize(8);
            model.SetFontName("Arial Narrow");
            lngY1 = model.GetCurrentY();
            model.DrawLine(700, lngY1, 11500, lngY1);
            model.SetFontBold(1);
            //model.SetTable("^500|^800|^500|^500|^500|^800|<3000|^1300|^1300|^1300;TY|OR DATE|STAT|TERM|QTR|CODE|DESCRIPTION|FEES AMOUNT|SURCH/PEN|TOTAL AMOUNT");
            //model.SetTable("^500|^800|^500|^500|^500|^500|^800|<3000|^1300|^1000|^1300;TY|OR DATE|STAT|TERM|QTR|NO. OF QTR|CODE|DESCRIPTION|FEES AMOUNT|SURCH/PEN|TOTAL AMOUNT");   // RMC 20130109 display no. of qtr in payment hist
            // RMC 20140122 view capital, gross, pre-gross on payment hist (s)
            /*model.SetTable("^500|^800|^500|^500|^500|^500|^800|<3000|^1300|^1000|^1300;TY|OR DATE|STAT|TERM|QTR|NO. OF|CODE|DESCRIPTION|FEES|SURCH|TOTAL");
            model.SetTable("^500|^800|^500|^500|^500|^500|^800|<3000|^1300|^1000|^1300;|||||QTR|||AMOUNT|/PEN|AMOUNT");*/
            // RMC 20140122 view capital, gross, pre-gross on payment hist (e)

            // RMC 20150501 QA BTAS (s)
            model.SetTable("^500|^700|^500|^500|^500|^500|^700|^2200|^1300|^1000|^1000|^1300;TY|OR DATE|STAT|TERM|QTR|NO. OF|CODE|DESCRIPTION|FEES|SURCH|PEN|TOTAL");
            model.SetTable("^500|^700|^500|^500|^500|^500|^700|^2200|^1300|^1000|^1000|^1300;|||||QTR|||AMOUNT|||AMOUNT");
            // RMC 20150501 QA BTAS (e)
            model.SetFontBold(0);
            lngY2 = model.GetCurrentY();
            model.DrawLine(700, lngY2, 11500, lngY2);
            model.DrawLine(700, lngY1, 700, lngY2);
            model.DrawLine(11500, lngY1, 11500, lngY2);
            model.SetTable("");

            string sNoOfQtr = string.Empty; // RMC 20130109 display no. of qtr in payment hist

            //result.Query = "select distinct * from pay_hist where or_no = :1 and bin = :2";
            //result.Query = "select distinct * from pay_hist where or_no = :1 and bin = :2 order by tax_year";    // RMC 20140114 added sorting by tax year in payment hist
            //distincts
            result.Query = "select distinct * from pay_hist where or_no = :1 and bin = :2 and qtr_paid = :3 order by tax_year";    // MCR 2014 paymenthist by qtr only
            result.AddParameter(":1", sOrNo.Trim());
            result.AddParameter(":2", sBin.Trim());
            result.AddParameter(":3", sQtrPaid.Trim());
            if (result.Execute())
            {
                //if(result.Read())
                while (result.Read())   // RMC 20140114 added sorting by tax year in payment hist
                {
                    sBnsStat = result.GetString("bns_stat").Trim();
                    sTerm = result.GetString("payment_term").Trim();
                    sQtr = result.GetString("qtr_paid").Trim();
                    sOrDate = result.GetDateTime("or_date").ToShortDateString();
                    sTaxYear = result.GetString("tax_year").Trim();
                    sUser = result.GetString("bns_user").Trim();
                    sTeller = result.GetString("teller").Trim();
                    sEncodeDate = result.GetDateTime("date_posted").ToShortDateString();
                    sMode = result.GetString("data_mode").Trim();
                    sPayType = result.GetString("payment_type").Trim();
                    // RMC 20130109 display no. of qtr in payment hist (s)
                    sNoOfQtr = result.GetString("no_of_qtr").Trim();
                    if (sQtr == "F")
                        sNoOfQtr = "4";
                    else
                    {
                        if (sNoOfQtr == "")
                            sNoOfQtr = "1";
                    }
                    // RMC 20130109 display no. of qtr in payment hist (e)
                    /*}
                }
                result.Close();*/
                    // RMC 20140114 added sorting by tax year in payment hist, put rem

                    //result.Query = "select * from or_table where or_no = :1 and fees_code = 'B'";

                    OracleResultSet pSet = new OracleResultSet();   // RMC 20140114 added sorting by tax year in payment hist, changed result to pSet
                    //pSet.Query = "select * from or_table where or_no = :1 and fees_code = 'B' and tax_year = :2";    // RMC 20140114 added sorting by tax year in payment hist

                    // AST 20160304 remove tax year if this payment is multi OR used. (s)
                    //bool withOtherOR = false;
                    if(!withOtherOR) //JARS 20170728 ADJUSTED THE USE OF THIS VARIABLE
                    {
                        if (!string.IsNullOrEmpty(AppSettingsManager.GetOtherOrNumber(sOrNo, "").Trim()))
                        {
                            pSet.Query = "select * from or_table where or_no = :1 and fees_code = 'B' and qtr_paid = :2";
                            pSet.AddParameter(":1", sOrNo.Trim());
                            pSet.AddParameter(":2", sQtrPaid.Trim());
                            withOtherOR = true;
                        }
                        // AST 20160304 remove tax year if this payment is multi OR used. (e)
                        else
                        {
                            pSet.Query = "select * from or_table where or_no = :1 and fees_code = 'B' and tax_year = :2 and qtr_paid = :3";    // MCR 20140321 paymenthist by qtr only
                            pSet.AddParameter(":1", sOrNo.Trim());
                            pSet.AddParameter(":2", sTaxYear.Trim());
                            pSet.AddParameter(":3", sQtrPaid.Trim());
                        }
                        if (pSet.Execute())
                        {
                            while (pSet.Read())
                            {
                                try
                                {
                                    dTotFeeB = pSet.GetDouble("fees_due");
                                }
                                catch (System.Exception ex)
                                {
                                    dTotFeeB = 0;
                                }
                                try
                                {
                                    dTotSurB = pSet.GetDouble("fees_surch");
                                }
                                catch (System.Exception ex)
                                {
                                    dTotSurB = 0;
                                }
                                try
                                {
                                    dTotPenB = pSet.GetDouble("fees_pen");
                                }
                                catch (System.Exception ex)
                                {
                                    dTotPenB = 0;
                                }
                                try
                                {
                                    dTotAmtB = pSet.GetDouble("fees_amtdue");
                                }
                                catch (System.Exception ex)
                                {
                                    dTotAmtB = 0;
                                }
                                dTotAmtFeeB = dTotAmtFeeB + dTotFeeB;
                                dTotSurPenB = dTotSurB + dTotPenB;

                                dTotTotAmtB = dTotTotAmtB + dTotAmtB;
                                /*sFees = string.Format("{0:#,##0.00}", dTotAmtFeeB);
                                sSurPen = string.Format("{0:#,##0.00}", dTotSurPenB);
                                sTotal  = string.Format("{0:#,##0.00}", dTotTotAmtB);*/

                                // RMC 20131226 modified printing of business tax paid per bns type (s)
                                sFees = string.Format("{0:#,##0.00}", dTotFeeB);
                                sSurPen = string.Format("{0:#,##0.00}", dTotSurPenB);
                                sTotal = string.Format("{0:#,##0.00}", dTotFeeB + dTotSurPenB);
                                sFeesCode = pSet.GetString("bns_code_main").Trim();

                                sTotPenB = string.Format("{0:#,##0.00}", dTotPenB);
                                sTotSurB = string.Format("{0:#,##0.00}", dTotSurB);

                                //model.SetTable(string.Format("^500|^800|^500|^500|^500|^800|<3000|>1300|>1300|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sFeesCode, AppSettingsManager.GetBnsDesc(sFeesCode), sFees, sSurPen, sTotal));
                                //model.SetTable(string.Format("^500|^800|^500|^500|^500|^500|^800|<3000|>1300|>1000|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, AppSettingsManager.GetBnsDesc(sFeesCode), sFees, sSurPen, sTotal));  // RMC 20130109 display no. of qtr in payment hist
                                //model.SetTable(string.Format("^500|^700|^500|^500|^500|^500|^700|<2200|>1300|>1000|>1000|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, AppSettingsManager.GetBnsDesc(sFeesCode), sFees, sTotSurB, sTotPenB, sTotal));  // MCR 20150204 SPLIT SURCH / PEN
                                if (withOtherOR) // AST 20160304
                                {
                                    sTaxYear = pSet.GetString("Tax_Year"); ;
                                }

                                model.SetTable(string.Format("^500|^700|^500|^500|^500|^500|^700|<2200|>1300|>1000|>1000|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, AppSettingsManager.GetBnsDesc(sFeesCode), sFees, sTotSurB, sTotPenB, sTotal));
                                // RMC 20131226 modified printing of business tax paid per bns type (e)

                                // RMC 20140122 view capital, gross, pre-gross on payment hist (s)
                                //GetCapGross(sBin.Trim(), sTaxYear.Trim(), sFeesCode);
                                GetCapGross(sBin.Trim(), sTaxYear.Trim(), sFeesCode, sOrNo, sBnsStat); //JHB 20181203 merge from LAL_LO JARS 20181004
                                if (sBnsStat.Trim() == "NEW")
                                {
                                    model.SetTable(string.Format("^500|^800|^500|^500|^500|^500|^800|<3000|>1300|>1000|>1300;|||||||{0}|||", "Capital: " + m_sCap));
                                }
                                else
                                {
                                    model.SetTable(string.Format("^500|^800|^500|^500|^500|^500|^800|<3000|>1300|>1000|>1300;|||||||{0}|||", "Gross: " + m_sGross + " P.Gross: " + m_sPreGross));
                                }
                                // RMC 20140122 view capital, gross, pre-gross on payment hist (e)
                            }
                            //model.SetTable(string.Format("^500|^800|^500|^500|^500|^800|<3000|>1300|>1300|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), AppSettingsManager.GetBnsCodeMain(sBin.Trim()), AppSettingsManager.GetBnsDesc(AppSettingsManager.GetBnsCodeMain(sBin.Trim())), sFees, sSurPen, sTotal));
                        }
                        //result.Close();
                        pSet.Close();   // RMC 20140114 added sorting by tax year in payment hist, changed result to pSet
                    }
                }
            }
            result.Close(); // RMC 20140114 added sorting by tax year in payment hist, changed result to pSet

            //result.Query = "select a.fees_desc as fees_desc, b.fees_code as fees_code, sum(b.fees_due) as sum_fees_due, sum(b.fees_surch+b.fees_pen) as sum_surch_pen, sum(b.fees_amtdue) as sum_amtdue from tax_and_fees_table a, or_table b where or_no = :1 and a.fees_code = b.fees_code and rev_year = :2 group by a.fees_desc, b.fees_code order by b.fees_code";
            // RMC 20150501 QA BTAS (s)
            double dTotPen = 0;
            //result.Query = "select a.fees_desc as fees_desc, b.fees_code as fees_code, sum(b.fees_due) as sum_fees_due, sum(b.fees_surch) as sum_surch_pen, sum(b.fees_pen) as sum_pen, sum(b.fees_amtdue) as sum_amtdue from tax_and_fees_table a, or_table b where or_no = :1 and a.fees_code = b.fees_code and rev_year = :2 group by a.fees_desc, b.fees_code order by b.fees_code";
            // RMC 20150501 QA BTAS (e)
            result.Query = "select a.fees_desc as fees_desc, b.fees_code as fees_code, sum(b.fees_due) as sum_fees_due, sum(b.fees_surch) as sum_surch_pen, sum(b.fees_pen) as sum_pen, sum(b.fees_amtdue) as sum_amtdue,b.tax_year  from tax_and_fees_table a, or_table b where or_no = :1 and a.fees_code = b.fees_code and rev_year = :2 group by a.fees_desc, b.fees_code, b.tax_year order by b.tax_year, b.fees_code";
            String sTmpTaxYear = ""; //MCR 20150901
            result.AddParameter(":1", sOrNo.Trim());
            result.AddParameter(":2", AppSettingsManager.GetConfigValue("07"));
            if(result.Execute())
            {
                while(result.Read())
                {
                    sFeesDesc = result.GetString("fees_desc").Trim();
                    sFeesCode = result.GetString("fees_code").Trim();
                    sTmpTaxYear = result.GetString("tax_year").Trim();
                    try
                    {
                        dTotFeeF = result.GetDouble("sum_fees_due");
                    }
                    catch (System.Exception ex)
                    {
                        dTotFeeF = 0;
                    }
                    try
                    {
                        dTotSurF = result.GetDouble("sum_surch_pen");
                    }
                    catch (System.Exception ex)
                    {
                        dTotSurF = 0;
                    }

                    // RMC 20150501 QA BTAS (s)
                    try
                    {
                        dTotPen = result.GetDouble("sum_pen");
                    }
                    catch (System.Exception ex)
                    {
                        dTotPen = 0;
                    }
                    // RMC 20150501 QA BTAS (e)

                    try
                    {
                        dTotAmtF = result.GetDouble("sum_amtdue");
                    }
                    catch (System.Exception ex)
                    {
                        dTotAmtF = 0;
                    }
                    

                    sFees = string.Format("{0:#,##0.00}", dTotFeeF);
                    sSurPen = string.Format("{0:#,##0.00}", dTotSurF);
                    sTotal = string.Format("{0:#,##0.00}", dTotAmtF);
                    string sPen = string.Format("{0:#,##0.00}", dTotPen); // RMC 20150501 QA BTAS
                    dTotTotAmtB = dTotTotAmtB + dTotAmtF;
                    //model.SetTable(string.Format("^500|^800|^500|^500|^500|^800|<3000|>1300|>1300|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sFeesCode, sFeesDesc, sFees, sSurPen, sTotal));
                    //model.SetTable(string.Format("^500|^800|^500|^500|^500|^500|^800|<3000|>1300|>1000|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, sFeesDesc, sFees, sSurPen, sTotal));  // RMC 20130109 display no. of qtr in payment hist
                    //model.SetTable(string.Format("^500|^800|^500|^500|^500|^500|^800|<3000|>1300|>1000|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, sFeesDesc, sFees, sSurPen, sTotal));  
                    //model.SetTable(string.Format("^500|^700|^500|^500|^500|^500|^700|<2200|>1300|>1000|>1000|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, sFeesDesc, sFees, sSurPen, sPen, sTotal));    // RMC 20150501 QA BTAS
                    model.SetTable(string.Format("^500|^700|^500|^500|^500|^500|^700|<2200|>1300|>1000|>1000|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sTmpTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, sFeesDesc, sFees, sSurPen, sPen, sTotal));    // MCR 20150901
                }
            }
            result.Close();

            model.SetTable("");
            lngY2 = model.GetCurrentY();
            model.DrawLine(700, lngY2, 11500, lngY2);
            model.SetTable("");
            sTotal = string.Format("{0:#,##0.00}", dTotTotAmtB);
            model.SetFontBold(1);
            model.SetFontSize(12);
            model.SetTable(">10800;TOTAL:  " + sTotal + "   ");
            model.SetTable("");
            model.SetTable("");
            model.SetFontBold(0);
            model.SetFontSize(8);

            if (sPayType == "CS")
                sPayType = "CASH";
            else if (sPayType == "CQ")
                sPayType = "CHEQUE";
            else if (sPayType == "CC")
                sPayType = "CASH & CHEQUE";
            else if (sPayType == "CSTC")
                sPayType = "CASH & TAX CREDIT";
            else if (sPayType == "CQTC")
                sPayType = "CHEQUE & TAX CREDIT";

            
            if (sMode == "POS")
            {
                if (m_objSystemUser.Load(sUser))
                {
                    sUser = m_objSystemUser.UserName;
                }
                //model.SetTable("<1500|<9800;DATA MODE|: POSTING");
                model.SetTable("<1800|^500|<9300;DATA MODE|:|POSTING");
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                //model.SetTable("<1500|<9800;PAYMENT TYPE|: " + sPayType);
                model.SetTable("<1800|^500|<9300;PAYMENT TYPE|:|" + sPayType);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                //model.SetTable("<1500|<9800;ENCODED BY|: " + sUser + "   ");
                model.SetTable("<1800|^500|<9300;ENCODED BY|:|" + sUser + "   ");
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                //model.SetTable("<1500|<9800;DATE ENCODED|: " + sEncodeDate + "   ");
                model.SetTable("<1800|^500|<9300;DATE ENCODED|:|" + sEncodeDate + "   ");
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;

            }
            if (sMode == "ONL")
            {
                if (m_objSystemUser.Load(sTeller))
                {
                    sTeller = m_objSystemUser.UserName;
                }
                //model.SetTable("<1800|<9800;DATA MODE|: ONLINE");
                model.SetTable("<1800|^500|<9300;DATA MODE|:|ONLINE");
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                //model.SetTable("<1800|<9800;PAYMENT TYPE|:" + sPayType);
                model.SetTable("<1800|^500|<9300;PAYMENT TYPE|:|" + sPayType);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                //model.SetTable("<1800|<9800;TELLER|: " + sTeller + "   ");
                model.SetTable("<1800|^500|<9300;TELLER|:|" + sTeller + "   ");
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                //model.SetTable("<1800|<9800;TRANSACTION DATE|: " + sEncodeDate + "   ");
                model.SetTable("<1800|^500|<9300;TRANSACTION DATE|:|" + sEncodeDate + "   ");
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            }
            if (sMode == "OFL")
            {
                if (m_objSystemUser.Load(sTeller))
                {
                    sTeller = m_objSystemUser.UserName;
                }
                //model.SetTable("<1800|<9800;DATA MODE|: OFFLINE");
                model.SetTable("<1800|^500|<9300;DATA MODE|:|OFFLINE");
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                //model.SetTable("<1800|<9800;PAYMENT TYPE|:" + sPayType);
                model.SetTable("<1800|^500|<9300;PAYMENT TYPE|:|" + sPayType);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                //model.SetTable("<1800|<9800;TELLER|: " + sTeller + "   ");
                model.SetTable("<1800|^500|<9300;TELLER|:|" + sTeller + "   ");
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                //model.SetTable("<1800|<9800;TRANSACTION DATE|: " + sEncodeDate + "   "); //JAV 20170804 fix the alignment
                model.SetTable("<1800|^500|<9300;TRANSACTION DATE|:|" + sEncodeDate + "   "); //JAV 20170804 fix the alignment
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            }

            //JAV 20170803(s)
            string strCurrentDate = string.Empty;
            string strCurrentTime = string.Empty;
            string strUser = string.Empty;
            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            strCurrentTime = string.Format("{0:hh:mm:ss tt}", AppSettingsManager.GetCurrentDate());
            strUser = AppSettingsManager.SystemUser.UserCode;
            model.SetTable("<1800|^500|<9300;DATE PRINTED|:|" + strCurrentDate);
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.SetTable("<1800|^500|<9300;TIME PRINTED|:|" + strCurrentTime);
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.SetTable("<1800|^500|<9300;PRINTED BY|:|" + strUser);
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //JAV 20170803(e)

            // RMC 20170130 added signatory in Payment History of BTAS (s)
            //if (AppSettingsManager.GetSystemType == "C")
            //{
            //    model.SetTable("");
            //    model.SetTable("");
            //    model.SetFontBold(1);
            //    model.SetTable(string.Format("<7800|^3000;|{0}", AppSettingsManager.GetConfigValue("05")));
            //    model.SetFontBold(0);
            //    model.SetTable(string.Format("<7800|^3000;|{0}", AppSettingsManager.GetConfigValue("01") + " TREASURER"));
            //}
            // RMC 20170130 added signatory in Payment History of BTAS (e)
            
        }
        public void OfficialReceipt(string sOrNo, string sBin, string sTermPay, string sQtrToPay, string sBnsStat)
        {
            long lngY1 = 0;
            long lngY2 = 0;
            long lngX1 = 0;
            long lngX2 = 0;
            long termY = 0;

            string sFeesCode = string.Empty;
            // for last paymeny info
            string sLastOrDate = string.Empty;
            string sLastTaxYear = string.Empty;
            string sLastPaymentTerm = string.Empty;
            string sLastPaymentMode = string.Empty;
            string sLastTeller = string.Empty;
            string sLastStat = string.Empty;
            string sLastQtr = string.Empty;

            model.Clear();
            sImagepath = "C:\\BPLS\\Images\\logo_header.jpg";
            model.LeftMargin = 30;
            sList.ReturnValueByBin = sBin;
            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                model.SetFontName("Arial");
                model.SetTable("");
                model.SetTable("");

                string sYear, sTerm, sQrt, sTmpGross = string.Empty, sGross = string.Empty, sParticular, sDue, sSurch, sInt, sTotal;
                double dDue, dSurch, dInt, dTotal, dGross;
                double dGrandTotDue = 0, dGrandTotSurch = 0, dGrandTotInt = 0, dGrandTotTotal = 0;
                string sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal, sGrandTotTotal1;
                string sTerm1 = string.Empty;
                string sTaxCredit = string.Empty;
                string sTellerPos = string.Empty;
                double dTaxCredit = 0;
                int iMultiYear = 0;
                sYear = string.Empty;
                sTerm = string.Empty;
                sQrt = string.Empty;
                // string sLastTaxYear = string.Empty;

                result.Query = "select count(distinct(tax_year)) as taxCount from or_table where or_no = '" + sOrNo + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        iMultiYear = result.GetInt("taxCount");
                    }
                }
                result.Close();

                result.Query = "select distinct * from pay_hist where or_no = '" + sOrNo.Trim() + "' order by tax_year desc";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sTerm = result.GetString("payment_term").Trim();
                        sQrt = result.GetString("qtr_paid").Trim();
                        sLastTaxYear = result.GetString("tax_year").Trim();
                    }
                }
                result.Close();



                if (sTerm == "F")
                    sTerm1 = "FULL YEAR";
                else
                    if (sQrt.Trim() == "1")
                        sTerm1 = "QTRLY-1st Qtr";
                if (sQrt.Trim() == "2")
                    sTerm1 = "QTRLY-2nd Qtr";
                if (sQrt.Trim() == "3")
                    sTerm1 = "QTRLY-3rd Qtr";
                if (sQrt.Trim() == "4")
                    sTerm1 = "QTRLY-4th Qtr";

                model.SetFontSize(12);
                model.SetTable("^11000;Republic of the Philippines");
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    model.SetTable("^11000;CITY OF" + AppSettingsManager.GetConfigValue("02"));
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    model.SetTable("^11000;MUNICIPALITY OF" + AppSettingsManager.GetConfigValue("02"));
                model.SetTable("");
                model.SetFontSize(10);
                model.SetTable(">8150|^2900;|TERM");
                model.SetFontSize(15);
                model.SetFontBold(1);
                model.SetTable("<11000;Official Receipt");
                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetTable("<1300|<9700;O.R. Date: |" + AppSettingsManager.MonthsInWords(dDate));
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 10;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
                model.SetFontSize(15);
                model.SetFontBold(1);
                lngY1 = model.GetCurrentY();
                model.SetCurrentY(2100);
                model.SetTable(">8150|^2900;|" + sTerm1.Trim());
                model.SetCurrentY(lngY1);
                model.SetTable("");
                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetFontSize(15);
                model.SetFontBold(1);

                model.SetTable("<11000;" + sOrNo);
                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetTable("<11000;Official Receipt Number");
                model.SetTable("");
                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetFontSize(15);
                model.SetFontBold(1);
                model.SetTable("<11000;" + sBin);

                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetTable("<11000;Business Identification Number");
                model.SetTable("");

                model.SetFontSize(8);
                //model.SetTable(">10500;BILL NUMBER & DATE:   " + AppSettingsManager.GetBillNoAndDate(sBin, sSOATaxYear, sList.BPLSAppSettings[i].sBnsCode.Trim()));
                model.SetFontBold(1);
                model.SetTableBorder(0);
                lngY1 = model.GetCurrentY();
                model.DrawLine(250, lngY1, 11300, lngY1);
                model.SetTable("");

                model.SetTable("<1800|<4000|<1400|<4000;BUSINESS NAME|: " + sList.BPLSAppSettings[i].sBnsNm.Trim() + "|DESCRIPTION|: " + AppSettingsManager.GetBnsDesc(sList.BPLSAppSettings[i].sBnsCode.Trim()));
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS ADDRESS|: " + AppSettingsManager.GetBnsAddress(sBin.Trim()) + "|PREV. STATUS|: " + sList.BPLSAppSettings[i].sBnsStat.Trim() + "| | ");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[5].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontStyle = 0;
                model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS OWNER|: " + AppSettingsManager.GetBnsOwner(sList.BPLSAppSettings[i].sOwnCode.Trim()) + "|TAX YEAR|: " + sLastTaxYear + " | | ");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[5].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontStyle = 0;
                model.SetTable("<1800|<4000|<1400|<2000|<1000|<1000;LAST PERMIT NO|: " + sList.BPLSAppSettings[i].sPermitNo + "|DATE ISSUED|: | |");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.SetTable("");
                lngY2 = model.GetCurrentY();
                model.DrawLine(250, lngY2, 11300, lngY2);
                model.DrawLine(250, lngY1, 250, lngY2);
                model.DrawLine(11300, lngY1, 11300, lngY2);
                model.SetTableBorder(0);
                model.SetFontBold(0);
                model.SetFontName("Arial Narrow");
                model.SetFontSize(8);
                model.SetTable("");
                lngY1 = model.GetCurrentY();
                model.DrawLine(250, lngY1, 11300, lngY1);
                model.SetTable("^600|^200|^200|<2500|>1300|>1500|>1500|>1500|>1500;YEAR|||PARTICULARS|GROSS / CAPITAL|DUE|SURCH|INT|TOTAL");
                lngY2 = model.GetCurrentY();
                model.DrawLine(250, lngY2, 11300, lngY2);
                model.DrawLine(250, lngY1, 250, lngY2);
                model.DrawLine(11300, lngY1, 11300, lngY2);


                // TERM
                model.DrawLine(8850, 1400, 11300, 1400); // TOP
                model.DrawLine(8850, 2700, 11300, 2700); // BOTTOM
                model.DrawLine(8850, 1800, 11300, 1800); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(8850, 1400, 8850, 2700); // LEFT
                model.DrawLine(11300, 1400, 11300, 2700); // RIGHT
                // TERM
                model.SetTable("");


                result.Query = "select * from or_table where or_no = '" + sOrNo.Trim() + "' order by tax_year, fees_code";
                if (result.Execute())
                {
                    while (result.Read())
                    {



                        sFeesCode = result.GetString("fees_code").Trim();
                        if (sFeesCode.Trim() != "B")
                            sParticular = AppSettingsManager.GetFeesDesc(sFeesCode);
                        else
                            sParticular = "TAX ON " + AppSettingsManager.GetBnsDesc(result.GetString("bns_code_main").Trim());
                        sYear = result.GetString("tax_year").Trim();
                        double.TryParse(AppSettingsManager.GetCapitalGross(sBin, result.GetString("bns_code_main").Trim(), result.GetString("tax_year").Trim(), sBnsStat).ToString(), out dGross);
                        double.TryParse(result.GetDouble("fees_due").ToString(), out dDue);
                        double.TryParse(result.GetDouble("fees_surch").ToString(), out dSurch);
                        double.TryParse(result.GetDouble("fees_pen").ToString(), out dInt);
                        double.TryParse(result.GetDouble("fees_amtdue").ToString(), out dTotal);
                        dGrandTotDue = dGrandTotDue + dDue;
                        dGrandTotSurch = dGrandTotSurch + dSurch;
                        dGrandTotInt = dGrandTotInt + dInt;
                        dGrandTotTotal = dGrandTotTotal + dTotal;

                        //
                        sGross = string.Format("{0:#,##0.00}", dGross);
                        sDue = string.Format("{0:#,##0.00}", dDue);
                        sSurch = string.Format("{0:#,##0.00}", dSurch);
                        sInt = string.Format("{0:#,##0.00}", dInt);
                        sTotal = string.Format("{0:#,##0.00}", dTotal);
                        if (sFeesCode.Trim() != "B")
                            sGross = string.Empty;

                        if (sGross == "0.00")
                            sGross = "";

                        result2.Query = "select distinct * from pay_hist where or_no = '" + sOrNo.Trim() + "' and tax_year = '" + sYear + "'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                sTerm = result2.GetString("payment_term").Trim();
                                sQrt = result2.GetString("qtr_paid").Trim();
                                //sLastTaxYear = result2.GetString("tax_year").Trim();
                                if (sFeesCode.Trim() != "B")
                                {
                                    sTerm = "F";
                                    sQrt = "F";
                                }
                            }
                        }
                        result2.Close();

                        model.SetTable(string.Format("^600|^200|^200|<2500|>1300|>1500|>1500|>1500|>1500;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8};", sYear, sTerm, sQrt, sParticular, sGross, sDue, sSurch, sInt, sTotal));
                    }
                }
                result.Close();

                // tax credit
                //result.Query = "select * from dbcr_memo where own_code in (select own_code from businesses where bin = '" + sBin.Trim() + "') and memo like 'REMAINING BALANCE AFTER PAYMENT%' and served = 'N'";
                //if (result.Execute())
                //{
                //    if (result.Read())
                //    {
                //        dTaxCredit = result.GetDouble("credit");

                //   }
                //}
                // result.Close();
                // tax credit
                sGrandTotDue = string.Format("{0:#,##0.00}", dGrandTotDue);
                sGrandTotSurch = string.Format("{0:#,##0.00}", dGrandTotSurch);
                sGrandTotInt = string.Format("{0:#,##0.00}", dGrandTotInt);
                sTaxCredit = string.Format("{0:#,##0.00}", dTaxCredit);
                sGrandTotTotal = string.Format("{0:#,##0.00}", dGrandTotTotal);
                sGrandTotTotal1 = string.Format("{0:#,##0.00}", dGrandTotTotal - dTaxCredit);


                model.SetTable("");
                lngY1 = model.GetCurrentY();
                model.DrawLine(5500, lngY1, 11300, lngY1);
                model.SetTable(string.Format(">4800|>1500|>1500|>1500|>1500;Sub-Total:|{0}|{1}|{2}|{3};", sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal));
                model.SetTable(string.Format(">4800|>1500|>1500|>1500|>1500;Less Available Tax Credit:||||{0};", sTaxCredit));

                model.SetTable("");
                model.SetTable("");

                lngY1 = model.GetCurrentY();
                // TOTAL DUE
                model.SetFontSize(8);
                model.SetFontName("Arial");
                model.DrawLine(250, lngY1, 2750, lngY1); // TOP
                model.SetTable("<2500|<2500|<3200|<2500; Total Amount Due| Surcharge and Interest| Less Available Tax Credit|   Grand Total Due");
                model.DrawLine(250, lngY1 + 900, 2750, lngY1 + 900); // BOTTOM
                model.DrawLine(250, lngY1 + 200, 2750, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(250, lngY1, 250, lngY1 + 900); // LEFT
                model.DrawLine(2750, lngY1, 2750, lngY1 + 900); // RIGHT

                // SURCH AND INTEREST
                model.DrawLine(2750, lngY1, 5250, lngY1); // TOP
                model.DrawLine(2750, lngY1 + 900, 5250, lngY1 + 900); // BOTTOM
                model.DrawLine(2750, lngY1 + 200, 5250, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(2750, lngY1, 2750, lngY1 + 900); // LEFT
                model.DrawLine(5250, lngY1, 5250, lngY1 + 900); // RIGHT
                model.SetTable("");


                string sTotSurchAndPen = string.Empty;
                sTotSurchAndPen = string.Format("{0:#,###.#0}", dGrandTotSurch + dGrandTotInt);
                model.SetTable("<2500|<2500|<2500|<700|^2700;P " + sGrandTotDue + "|P " + sTotSurchAndPen + "|P " + sTaxCredit + "||P " + sGrandTotTotal);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontSize = 14;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontStyle = 1;
                // TAX CREDIT
                model.DrawLine(5250, lngY1, 7750, lngY1); // TOP
                model.DrawLine(5250, lngY1 + 900, 7750, lngY1 + 900); // BOTTOM
                model.DrawLine(5250, lngY1 + 200, 7750, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(5250, lngY1, 5250, lngY1 + 900); // LEFT
                model.DrawLine(7750, lngY1, 7750, lngY1 + 900); // RIGHT
                // GRAND TOTAL
                model.DrawLine(8650, lngY1, 11300, lngY1); // TOP
                model.DrawLine(8650, lngY1 + 900, 11300, lngY1 + 900); // BOTTOM
                model.DrawLine(8650, lngY1 + 200, 11300, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(8650, lngY1, 8650, lngY1 + 900); // LEFT
                model.DrawLine(11300, lngY1, 11300, lngY1 + 900); // RIGHT

                model.SetTable("");
                model.SetTable("");
                model.SetTable("");
                model.SetTable("");
                lngY1 = model.GetCurrentY();

                model.DrawLine(250, lngY1 + 200, 11300, lngY1 + 200); // TOP
                model.SetCurrentY(lngY1 + 400);
                model.SetFontBold(1);


                result.Query = "select distinct * from pay_hist where bin = '" + sBin.Trim() + "' order by or_date desc, tax_year desc";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sLastOrDate = result.GetDateTime("or_date").ToShortDateString();
                        sLastTaxYear = result.GetString("tax_year").Trim();
                        sLastQtr = result.GetString("qtr_paid").Trim();
                        sLastPaymentTerm = result.GetString("payment_term").Trim();
                        sLastPaymentMode = result.GetString("data_mode").Trim();
                        sLastTeller = result.GetString("teller").Trim();
                        sLastStat = result.GetString("bns_stat").Trim();
                    }
                }
                result.Close();

                if (sLastQtr.Trim() == "F")
                    sLastQtr = "FULL";
                else
                {
                    if (sLastQtr == "1")
                        sLastQtr = "1st Qtr";
                    if (sLastQtr == "2")
                        sLastQtr = "2nd Qtr";
                    if (sLastQtr == "3")
                        sLastQtr = "3rd Qtr";
                    if (sLastQtr == "4")
                        sLastQtr = "4th Qtr";
                }

                if (sLastPaymentTerm.Trim() == "F")
                    sLastPaymentTerm = "Full Payment";
                else
                    sLastPaymentTerm = "Installment";

                if (sLastPaymentMode.Trim() == "POS")
                    sLastPaymentMode = "Posting";
                else if (sLastPaymentMode.Trim() == "UNP")
                    sLastPaymentMode = "For Posting";
                else if (sLastPaymentMode.Trim() == "OFL")
                    sLastPaymentMode = "Offline Payment";
                else if (sLastPaymentMode.Trim() == "ONL")
                    sLastPaymentMode = "Online Payment";

                if (sLastStat.Trim() == "NEW")
                    sLastStat = "New Business";
                else if (sLastStat.Trim() == "REN")
                    sLastStat = "Renewal";
                else
                    sLastStat = "Retired Business";

                if (m_objSystemUser.Load(sLastTeller.Trim()))
                {
                    sLastTeller = m_objSystemUser.UserName;
                }



                if (m_objSystemUser.Load(AppSettingsManager.SystemUser.UserCode))
                {
                    sTellerName = m_objSystemUser.UserName;
                    sTellerPos = m_objSystemUser.Position;
                }
                lngY1 = model.GetCurrentY();
                model.SetCurrentY(lngY1 + 600);
                // GDE 20101110 city treas name temporary hard coded
                model.SetFontBold(0);
                model.SetTable("<1900|^2000|<3600|<1500|^2000;Payment Received by:|" + sTellerName + "||Approved By:| ELVIRA M. REYES");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontStyle = 9;
                //string sDate = string.Empty;

                model.SetTable("<1900|^2000|<3600|<1500|^2000;|" + sTellerPos + "|||City Treasurer");
                model.SetTable("<1900|<2000|<3600|<1500|^2000;Date Printed:|" + dDate.ToShortDateString() + "   " + dDate.ToLongTimeString() + "|||");
                model.SetTable("");



                PreviewDocu();
            }
        }
        public void SOA(string sBin, string sTermPay, string sQtrToPay)
        {
            long lngY1 = 0;
            long lngY2 = 0;
            long lngX1 = 0;
            long lngX2 = 0;
            long termY = 0;

            // for last paymeny info
            string sLastOrDate = string.Empty;
            string sLastTaxYear = string.Empty;
            string sLastPaymentTerm = string.Empty;
            string sLastPaymentMode = string.Empty;
            string sLastTeller = string.Empty;
            string sLastStat = string.Empty;
            string sLastQtr = string.Empty;

            model.Clear();
            sImagepath = "C:\\Documents and Settings\\Godfrey\\My Documents\\Downloads\\logo_header.jpg"; // GDE 20111209 ask if this is applicable 
            model.LeftMargin = 30;
            sList.ReturnValuesByBinQue = sBin;
            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                model.SetFontName("Arial");
                model.SetTable("");
                model.SetTable("");

                string sYear, sTerm, sQrt, sTmpGross = string.Empty, sGross = string.Empty, sParticular, sDue, sSurch, sInt, sTotal;
                double dDue, dSurch, dInt, dTotal, dGross;
                double dGrandTotDue = 0, dGrandTotSurch = 0, dGrandTotInt = 0, dGrandTotTotal = 0;
                string sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal, sGrandTotTotal1;
                string sTerm1 = string.Empty;
                string sTaxCredit = string.Empty;
                string sTellerPos = string.Empty;
                double dTaxCredit = 0;
                sQrt = string.Empty;

                if (sTermPay == "F")
                    sTerm1 = "FULL YEAR";
                else
                {
                    if (sQtrToPay.Trim() == "1")
                        sTerm1 = "QTRLY-1st Qtr";
                    if (sQtrToPay.Trim() == "2")
                        sTerm1 = "QTRLY-2nd Qtr";
                    if (sQtrToPay.Trim() == "3")
                        sTerm1 = "QTRLY-3rd Qtr";
                    if (sQtrToPay.Trim() == "4")
                        sTerm1 = "QTRLY-4th Qtr";
                }

                model.SetFontSize(12);
                model.SetTable("^11000;Republic of the Philippines");
                model.SetTable("<11000;PROVINCE OF " + AppSettingsManager.GetConfigValue("08"));
                model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("02"));
                model.SetTable("^11000;Office of the City Treasurer");
                model.SetTable("");
                model.SetFontSize(10);
                model.SetTable(">8150|^2900;|TERM");
                model.SetFontSize(15);
                model.SetFontBold(1);
                model.SetTable("<11000;Statement of Account");
                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetTable("<1300|<9700;Balance as of|" + AppSettingsManager.MonthsInWords(dDate));
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 10;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
                model.SetFontSize(15);
                model.SetFontBold(1);
                lngY1 = model.GetCurrentY();
                model.SetCurrentY(2100);
                model.SetTable(">8150|^2900;|" + sTerm1.Trim());
                model.SetCurrentY(lngY1);
                model.SetTable("");
                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetFontSize(15);
                model.SetFontBold(1);
                model.SetTable("<11000;" + sBin);

                model.SetFontBold(0);
                model.SetFontSize(10);
                model.SetTable("<11000;Business Identification Number");
                model.SetTable("");

                model.SetFontSize(8);
                model.SetTable(">10500;BILL NUMBER & DATE:   " + AppSettingsManager.GetBillNoAndDate(sBin, sSOATaxYear, sList.BPLSAppSettings[i].sBnsCode.Trim()));
                model.SetFontBold(1);
                model.SetTableBorder(0);
                lngY1 = model.GetCurrentY();
                model.DrawLine(250, lngY1, 11300, lngY1);
                model.SetTable("");

                //model.SetTable("<1800|<4000|<1400|<4000;BUSINESS NAME|: " + sList.BPLSAppSettings[i].sBnsNm.Trim() + "|DESCRIPTION|: " + AppSettingsManager.GetBnsDesc(sList.BPLSAppSettings[i].sBnsCode.Trim()));
                model.SetTable("<1800|<4000|<1400|<4000;BUSINESS NAME|: " + m_strBnsNm + "|DESCRIPTION|: " + AppSettingsManager.GetBnsDesc(sList.BPLSAppSettings[i].sBnsCode.Trim())); // CJC 20130816
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                //model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS ADDRESS|: " + AppSettingsManager.GetBnsAddress(sBin.Trim()) + "|PREV. STATUS|: " + sList.BPLSAppSettings[i].sBnsStat.Trim() + "|TAX YEAR|: " + sSOATaxYear.Trim());
                //model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS ADDRESS|: " + m_strBnsAdd + "|PREV. STATUS|: " + sList.BPLSAppSettings[i].sBnsStat.Trim() + "|TAX YEAR|: " + sSOATaxYear.Trim()); // CJC 20130816
                model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS ADDRESS|: " + m_strBnsAdd + "|STATUS|: " + sList.BPLSAppSettings[i].sBnsStat.Trim() + "|TAX YEAR|: " + sSOATaxYear.Trim()); // RMC 20140708 added retirement billing
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[5].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontStyle = 0;
                //model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS OWNER|: " + AppSettingsManager.GetBnsOwner(sList.BPLSAppSettings[i].sOwnCode.Trim()) + "|LAST PAYMENT|: P " + AppSettingsManager.GetLastPaymentInfo(sBin, "AMOUNT") + "|LAST OR#|: " + AppSettingsManager.GetLastPaymentInfo(sBin, "OR"));                
                model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS OWNER|: " + AppSettingsManager.GetBnsOwner(m_strBnsOwn) + "|LAST PAYMENT|: P " + AppSettingsManager.GetLastPaymentInfo(sBin, "AMOUNT") + "|LAST OR#|: " + AppSettingsManager.GetLastPaymentInfo(sBin, "OR")); // CJC 20131494
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[5].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontStyle = 0;
                model.SetTable("<1800|<4000|<1400|<2000|<1000|<1000;LAST PERMIT NO|: " + sList.BPLSAppSettings[i].sPermitNo + "|DATE ISSUED|: ||");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.SetTable("");
                lngY2 = model.GetCurrentY();
                model.DrawLine(250, lngY2, 11300, lngY2);
                model.DrawLine(250, lngY1, 250, lngY2);
                model.DrawLine(11300, lngY1, 11300, lngY2);
                model.SetTableBorder(0);
                model.SetFontBold(0);
                model.SetFontName("Arial Narrow");
                model.SetFontSize(8);
                model.SetTable("");
                lngY1 = model.GetCurrentY();
                model.DrawLine(250, lngY1, 11300, lngY1);
                if (AppSettingsManager.GetConfigValue("10") == "232")    // RMC 20140108 hide gross/capital in soa
                    model.SetTable("^600|^200|^200|<2500|>1300|>1500|>1500|>1500|>1500;YEAR|||PARTICULARS||DUE|SURCH|INT|TOTAL");
                else
                    model.SetTable("^600|^200|^200|<2500|>1300|>1500|>1500|>1500|>1500;YEAR|||PARTICULARS|GROSS / CAPITAL|DUE|SURCH|INT|TOTAL");
                lngY2 = model.GetCurrentY();
                model.DrawLine(250, lngY2, 11300, lngY2);
                model.DrawLine(250, lngY1, 250, lngY2);
                model.DrawLine(11300, lngY1, 11300, lngY2);


                // TERM
                model.DrawLine(8850, 1400, 11300, 1400); // TOP
                model.DrawLine(8850, 2700, 11300, 2700); // BOTTOM
                model.DrawLine(8850, 1800, 11300, 1800); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(8850, 1400, 8850, 2700); // LEFT
                model.DrawLine(11300, 1400, 11300, 2700); // RIGHT
                // TERM
                model.SetTable("");

                result.Query = "select * from soa_tbl where bin = '" + sBin + "' order by year asc, fees_code_sort desc, qtr asc";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        sYear = result.GetString("year").Trim();
                        sTerm = result.GetString("term").Trim();
                        sQrt = result.GetString("qtr").Trim();
                        sParticular = result.GetString("particulars").Trim();

                        double.TryParse(result.GetDouble("gross").ToString(), out dGross);
                        double.TryParse(result.GetDouble("due").ToString(), out dDue);
                        double.TryParse(result.GetDouble("surch").ToString(), out dSurch);
                        double.TryParse(result.GetDouble("pen").ToString(), out dInt);
                        double.TryParse(result.GetDouble("total").ToString(), out dTotal);
                        dGrandTotDue = dGrandTotDue + dDue;
                        dGrandTotSurch = dGrandTotSurch + dSurch;
                        dGrandTotInt = dGrandTotInt + dInt;
                        dGrandTotTotal = dGrandTotTotal + dTotal;

                        sGross = string.Format("{0:#,##0.00}", dGross);
                        sDue = string.Format("{0:#,##0.00}", dDue);
                        sSurch = string.Format("{0:#,##0.00}", dSurch);
                        sInt = string.Format("{0:#,##0.00}", dInt);
                        sTotal = string.Format("{0:#,##0.00}", dTotal);

                        if (sGross == "0.00")
                            sGross = "";

                        if (AppSettingsManager.GetConfigValue("10") == "232")    // RMC 20140108 hide gross/capital in soa
                        {
                            sGross = "";
                        }
                        
                        model.SetTable(string.Format("^600|^200|^200|<2500|>1300|>1500|>1500|>1500|>1500;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8};", sYear, sTerm, sQrt, sParticular, sGross, sDue, sSurch, sInt, sTotal));
                    }
                }
                result.Close();

                // tax credit
                //result.Query = "select * from dbcr_memo where own_code in (select own_code from businesses where bin = '" + sBin.Trim() + "') and memo like 'REMAINING BALANCE AFTER PAYMENT%' and served = 'N'"; // GDE 20111021
                //result.Query = "select * from dbcr_memo where bin = '" + sBin.Trim() + "' and memo like 'REMAINING BALANCE AFTER PAYMENT%' and served = 'N' and multi_pay = 'N'";
                //result.Query = "select * from dbcr_memo where bin = '" + sBin.Trim() + "' and (memo like 'REMAINING BALANCE AFTER PAYMENT%' or memo like 'REMAINING BALANCE AFTER ADDING OF TAX CREDIT%') and served = 'N' and multi_pay = 'N'";    // RMC 20140113 corrected display of added tax credit in SOA
                //result.Query = "select * from dbcr_memo where own_code in (select own_code from businesses where bin = '" + sBin.Trim() + "') and (memo like 'REMAINING BALANCE AFTER PAYMENT%' or memo like 'REMAINING BALANCE AFTER ADDING OF TAX CREDIT%') and served = 'N' and multi_pay = 'N'";    //JARS 20170713
                result.Query = "select * from dbcr_memo where bin = '" + sBin.Trim() + "' and (memo like 'REMAINING BALANCE AFTER PAYMENT%' or memo like 'REMAINING BALANCE AFTER ADDING OF TAX CREDIT%') and served = 'N' and multi_pay = 'N'";    // RMC 20140113 corrected display of added tax credit in SOA //JARS 20171010
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dTaxCredit = result.GetDouble("credit");

                    }
                }
                result.Close();
                // tax credit
                sGrandTotDue = string.Format("{0:#,##0.00}", dGrandTotDue);
                sGrandTotSurch = string.Format("{0:#,##0.00}", dGrandTotSurch);
                sGrandTotInt = string.Format("{0:#,##0.00}", dGrandTotInt);
                sTaxCredit = string.Format("{0:#,##0.00}", dTaxCredit);
                sGrandTotTotal = string.Format("{0:#,##0.00}", dGrandTotTotal);
                sGrandTotTotal1 = string.Format("{0:#,##0.00}", dGrandTotTotal - dTaxCredit);


                model.SetTable("");
                lngY1 = model.GetCurrentY();
                model.DrawLine(5500, lngY1, 11300, lngY1);
                model.SetTable(string.Format(">4800|>1500|>1500|>1500|>1500;Sub-Total:|{0}|{1}|{2}|{3};", sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal));
                model.SetTable(string.Format(">4800|>1500|>1500|>1500|>1500;Less Available Tax Credit:||||{0};", sTaxCredit));

                model.SetTable("");
                model.SetTable("");

                lngY1 = model.GetCurrentY();
                // TOTAL DUE
                model.SetFontSize(8);
                model.SetFontName("Arial");
                model.DrawLine(250, lngY1, 2750, lngY1); // TOP
                model.SetTable("<2500|<2500|<3200|<2500; Total Amount Due| Surcharge and Interest| Less Available Tax Credit|   Grand Total Due");
                model.DrawLine(250, lngY1 + 900, 2750, lngY1 + 900); // BOTTOM
                model.DrawLine(250, lngY1 + 200, 2750, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(250, lngY1, 250, lngY1 + 900); // LEFT
                model.DrawLine(2750, lngY1, 2750, lngY1 + 900); // RIGHT

                // SURCH AND INTEREST
                model.DrawLine(2750, lngY1, 5250, lngY1); // TOP
                model.DrawLine(2750, lngY1 + 900, 5250, lngY1 + 900); // BOTTOM
                model.DrawLine(2750, lngY1 + 200, 5250, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(2750, lngY1, 2750, lngY1 + 900); // LEFT
                model.DrawLine(5250, lngY1, 5250, lngY1 + 900); // RIGHT
                model.SetTable("");


                string sTotSurchAndPen = string.Empty;
                sTotSurchAndPen = string.Format("{0:#,###.#0}", dGrandTotSurch + dGrandTotInt);
                //model.SetTable("<2500|<2500|<2500|<700|^2700;P " + sGrandTotDue + "|P " + sTotSurchAndPen + "|P " + sTaxCredit + "||P " + sGrandTotTotal);
                model.SetTable("<2500|<2500|<2500|<700|^2700;P " + sGrandTotDue + "|P " + sTotSurchAndPen + "|P " + sTaxCredit + "||P " + sGrandTotTotal1); // RMC 20140113 corrected display of added tax credit in SOA
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontSize = 14;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontStyle = 1;
                // TAX CREDIT
                model.DrawLine(5250, lngY1, 7750, lngY1); // TOP
                model.DrawLine(5250, lngY1 + 900, 7750, lngY1 + 900); // BOTTOM
                model.DrawLine(5250, lngY1 + 200, 7750, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(5250, lngY1, 5250, lngY1 + 900); // LEFT
                model.DrawLine(7750, lngY1, 7750, lngY1 + 900); // RIGHT
                // GRAND TOTAL
                model.DrawLine(8650, lngY1, 11300, lngY1); // TOP
                model.DrawLine(8650, lngY1 + 900, 11300, lngY1 + 900); // BOTTOM
                model.DrawLine(8650, lngY1 + 200, 11300, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(8650, lngY1, 8650, lngY1 + 900); // LEFT
                model.DrawLine(11300, lngY1, 11300, lngY1 + 900); // RIGHT

                model.SetTable("");
                model.SetTable("");
                /*model.SetTable("");
                model.SetTable("");*/   // RMC 20140116 adjustment in SOA printing

                
                
                double dblDBCRAmount = 0;
                result.Query = "select sum(dbcr_amount) as dbcr_amount from tmp_dbcr where bin = '" + sBin.Trim() + "' and own_code = '" + AppSettingsManager.GetOwnCode(sBin.Trim()) + "' and dbcr_type = 'DEBIT' and served = 'N'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dblDBCRAmount = result.GetDouble("dbcr_amount");
                        if (dblDBCRAmount > 0)
                            model.SetTable("<11500;* A total of P" + dblDBCRAmount.ToString() + " over payment resulting from Adjustment/Revenue Examination of this record.;");

                    }
                }
                result.Close();
                result.Query = "select exempt_type from boi_table where bin = '" + sBin.Trim() + "'";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        model.SetTable("<11500;*" + result.GetString("exempt_type") + " member;");

                    }
                }
                result.Close();
                model.SetTable("");
                model.SetTable("");

                // CJC 20130816
                string sPUTStrTmp;
                string sPrevData = "";
                string sPUTRem = "";
                OracleResultSet sQuery = new OracleResultSet();
                sQuery.Query = "select distinct appl_type from permit_update_appl";
                sQuery.Query += " where bin = '" + sBin + "'";
                sQuery.Query += " and tax_year = '" + sSOATaxYear + "'";
                sQuery.Query += " and data_mode  = 'QUE'";
                if (sQuery.Execute())
                {
                    while (sQuery.Read())
                    {
                        sPUTStrTmp = sQuery.GetString("appl_type").Trim();
                        if (sPUTStrTmp == "TOWN")
                        {
                            sPrevData += "Prev Own :" + AppSettingsManager.GetBnsOwner(sList.BPLSAppSettings[i].sOwnCode.Trim()) + "\n";
                            sPUTStrTmp = "CHANGE OWN";
                        }
                        if (sPUTStrTmp == "TLOC")
                        {
                            sPrevData += "Prev Loc :" + AppSettingsManager.GetBnsAddress(sBin.Trim()) + "\n";
                            sPUTStrTmp = "TRANSFER LOC";
                        }
                        if (sPUTStrTmp == "ADDL")
                        {
                            sPUTStrTmp = "ADDL BNS";
                        }
                        if (sPUTStrTmp == "CTYP")
                        {                            
                            sPrevData += "Change Class.:" + "Test" + "\n";
                            sPUTStrTmp = "CHANGE CLASS";
                        }
                        //(s) RTL 02212006 for new business name
                        if (sPUTStrTmp == "CBNS")
                        {
                            sPrevData += "Prev Bns Name :" + sList.BPLSAppSettings[i].sBnsNm.Trim() + "\n";
                            sPUTStrTmp = "CHANGE BNS NAME";
                        }
                        //(e) RTL 02212006 for new business name
                        sPUTRem = sPUTRem + "/" + sPUTStrTmp;
                        
                    }
                }
                sQuery.Close();
                //////////

                if(sPUTRem != "")   // RMC 20131223
                    model.SetTable("<11500;*" + sPUTRem);
                if (sPrevData != "") // RMC 20131223
                    model.SetTable("<11500;*" + sPrevData);

                lngY1 = model.GetCurrentY();

                //model.DrawLine(250, lngY1 + 200, 11300, lngY1 + 200); // TOP  // RMC 20140106 PUT REM
                //model.SetCurrentY(lngY1 + 400);   // RMC 20140106 PUT REM
                model.SetTable("<11000;");  // RMC 20140106
                model.SetTable("<11000;");  // RMC 20140106
                
                model.SetFontBold(1);
                model.SetFontUnderline(1);
                model.SetTable("<11000; Additional Details of Previous Payment");
                model.SetFontBold(0);
                model.SetFontUnderline(0);

                result.Query = "select distinct * from pay_hist where bin = '" + sBin.Trim() + "' order by or_date desc, tax_year desc";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sLastOrDate = result.GetDateTime("or_date").ToShortDateString();
                        sLastTaxYear = result.GetString("tax_year").Trim();
                        sLastQtr = result.GetString("qtr_paid").Trim();
                        sLastPaymentTerm = result.GetString("payment_term").Trim();
                        sLastPaymentMode = result.GetString("data_mode").Trim();
                        sLastTeller = result.GetString("teller").Trim();
                        sLastStat = result.GetString("bns_stat").Trim();
                    }
                }
                result.Close();

                if (sLastQtr.Trim() == "F")
                    sLastQtr = "FULL";
                else
                {
                    if (sLastQtr == "1")
                        sLastQtr = "1st Qtr";
                    if (sLastQtr == "2")
                        sLastQtr = "2nd Qtr";
                    if (sLastQtr == "3")
                        sLastQtr = "3rd Qtr";
                    if (sLastQtr == "4")
                        sLastQtr = "4th Qtr";
                }

                if (sLastPaymentTerm.Trim() == "F")
                    sLastPaymentTerm = "Full Payment";
                else if (sLastPaymentTerm.Trim() == "") // RMC 20131223 FOR NEWLY APPLIED BUSINESS, NO RECORD OF PREVIOUS PAYMENT
                    sLastPaymentTerm = "";
                else
                    sLastPaymentTerm = "Installment";

                if (sLastPaymentMode.Trim() == "POS")
                    sLastPaymentMode = "Posting";
                else if (sLastPaymentMode.Trim() == "UNP")
                    sLastPaymentMode = "For Posting";
                else if (sLastPaymentMode.Trim() == "OFL")
                    sLastPaymentMode = "Offline Payment";
                else if (sLastPaymentMode.Trim() == "ONL")
                    sLastPaymentMode = "Online Payment";

                if (sLastStat.Trim() == "NEW")
                    sLastStat = "New Business";
                else if (sLastStat.Trim() == "REN")
                    sLastStat = "Renewal";
                else
                {
                    if (sLastPaymentMode == "" && sLastStat == "")  // RMC 20131223 FOR NEWLY APPLIED BUSINESS, NO RECORD OF PREVIOUS PAYMENT
                        sLastStat = "";
                    else
                        sLastStat = "Retired Business";
                }

                if (m_objSystemUser.Load(sLastTeller.Trim()))
                {
                    sLastTeller = m_objSystemUser.UserName;
                }



                model.SetTable("<1500|<10000; O.R. Date|: " + sLastOrDate);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Tax Year / Qtr|: " + sLastTaxYear + " - " + sLastQtr);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Payment Term|: " + sLastPaymentTerm);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Payment Mode|: " + sLastPaymentMode);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Teller|: " + sLastTeller);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Business Status|: " + sLastStat);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.DrawLine(250, lngY1 + 1800, 11300, lngY1 + 1800); // BOTTOM LINE

                // GDE 20130215 (s)
                if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "NEW")
                {
                    model.SetCurrentY(lngY1 + 400);
                    model.SetFontBold(1);
                    model.SetFontUnderline(1);
                    model.SetTable("<3250|<3000|<2500|<2000; | ||");
                    model.SetFontSize(7);
                    model.SetCurrentY(lngY1 + 400);
                    model.SetTable("<3250|<3000|<2400|<2000; |||Balance to be paid On or Before:");
                    model.SetFontSize(8);
                    model.SetFontBold(0);
                    model.SetFontUnderline(0);
                    model.SetFontSize(2);
                    model.SetTable("");
                    model.SetFontSize(8);
                }
                // GDE 20130215 (E)

                // RMC 20140119 deleted hard-coded due dates in SOA (s)
                string sQtr2Due = string.Empty;
                string sQtr3Due = string.Empty;
                string sQtr4Due = string.Empty;

                result.Query = "select * from due_dates where due_year = '" + sSOATaxYear + "' order by due_date";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        if (result.GetString("due_desc") == "APRIL")
                            sQtr2Due = "2nd Qtr " + string.Format("{0:MMMM dd, yyyy}", result.GetDateTime("due_date"));
                        if (result.GetString("due_desc") == "JULY")
                            sQtr3Due = "3rd Qtr " + string.Format("{0:MMMM dd, yyyy}", result.GetDateTime("due_date"));
                        if (result.GetString("due_desc") == "OCTOBER")
                            sQtr4Due = "4th Qtr " + string.Format("{0:MMMM dd, yyyy}", result.GetDateTime("due_date"));
                    }
                }
                result.Close();
                // RMC 20140119 deleted hard-coded due dates in SOA (e)

                // CJC 20130121 (s)
                if (sTermPay == "I")
                {
                    model.SetCurrentY(lngY1 + 400);
                    model.SetFontBold(1);
                    model.SetFontUnderline(1);
                    model.SetTable("<3250|<3000|<2500|<2000; |Remaining Quarter Dues||");
                    model.SetFontSize(7);
                    model.SetCurrentY(lngY1 + 400);
                    model.SetTable("<3250|<3000|<2400|<2000; |||Balance to be paid On or Before:");
                    model.SetFontSize(8);
                    model.SetFontBold(0);
                    model.SetFontUnderline(0);
                    model.SetFontSize(2);
                    model.SetTable("");
                    model.SetFontSize(8);

                    string sTaxCode = string.Empty;
                    string sBnsCode = string.Empty;
                    bool blRunOnce = false;
                    dTotal = 0.0;
                    sParticular = string.Empty;
                    sTotal = string.Empty;

                    //result.Query = "select * from taxdues where bin = '" + sBin + "' and tax_code in ('B', '02') and tax_year = '" + sSOATaxYear + "' order by tax_year desc, tax_code desc";
                    result.Query = "select * from taxdues where bin = '" + sBin + "' and tax_code = 'B' and tax_year = '" + sSOATaxYear + "' order by tax_year desc, tax_code desc";
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            sBnsCode = result.GetString("bns_code_main").Trim();
                            sTaxCode = result.GetString("tax_code").Trim();
                            if (sTaxCode == "B")
                                sParticular = AppSettingsManager.GetBnsDesc(sBnsCode);
                            else
                                sParticular = AppSettingsManager.GetFeesDesc(sTaxCode);

                            double.TryParse(result.GetDouble("amount").ToString(), out dDue);
                            dTotal = dTotal + dDue;
                            sDue = string.Format("{0:#,##0.00}", dDue / 4);
                            sTotal = string.Format("{0:#,##0.00}", dTotal / 4);

                            if (sQtrToPay.Trim() == "1")
                            {
                                if (!blRunOnce)
                                {
                                    model.SetFontBold(1);
                                    model.SetTable("<3250|<2300|^1000|^1000|^1000;|Particulars|2nd Qtr|3rd Qtr|4th Qtr");
                                    model.SetFontSize(2);
                                    model.SetTable("");
                                    model.SetFontSize(7);
                                    model.SetFontBold(0);
                                    blRunOnce = true;
                                }
                                model.SetTable(string.Format("<3250|<2300|>1000|>1000|>1000;|{0}|{1}|{2}|{3};", sParticular, sDue, sDue, sDue));
                            }
                            else if (sQtrToPay.Trim() == "2")
                            {
                                if (!blRunOnce)
                                {
                                    model.SetFontBold(1);
                                    model.SetTable("<3250|<2300|^1000|^1000;|Particulars|3rd Qtr|4th Qtr");
                                    model.SetFontSize(2);
                                    model.SetTable("");
                                    model.SetFontSize(7);
                                    model.SetFontBold(0);
                                    blRunOnce = true;
                                }
                                model.SetTable(string.Format("<3250|<2300|>1000|>1000;|{0}|{1}|{2};", sParticular, sDue, sDue));
                            }
                            else if (sQtrToPay.Trim() == "3")
                            {
                                if (!blRunOnce)
                                {
                                    model.SetFontBold(1);
                                    model.SetTable("<3250|<2300|^1000;|Particulars|4th Qtr");
                                    model.SetFontSize(2);
                                    model.SetTable("");
                                    model.SetFontSize(7);
                                    model.SetFontBold(0);
                                    blRunOnce = true;
                                }
                                model.SetTable(string.Format("<3250|<2300|>1000;|{0}|{1};", sParticular, sDue));
                            }
                        }
                    }
                    result.Close();

                    model.SetFontSize(2);
                    model.SetTable("");
                    model.SetFontSize(7);
                    model.SetFontBold(1);

                    long lngY5 = model.GetCurrentY();

                    

                    if (sQtrToPay.Trim() == "1")
                    {
                        model.DrawLine(6300, lngY5, 9000, lngY5);
                        model.SetTable(string.Format("<3250|>2300|>1000|>1000|>1000;|{0}|{1}|{2}|{3};", "Total", sTotal, sTotal, sTotal));
                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        /*model.SetTable("<8800|<1000;|2nd Qtr April 20, 2013");
                        model.SetTable("<8800|<1000;|3rd Qtr July 20, 2013");
                        model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");*/
                        // RMC 20140119 deleted hard-coded due dates in SOA (s)
                        model.SetTable("<8800|<1000;|" + sQtr2Due);
                        model.SetTable("<8800|<1000;|" + sQtr3Due);
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                    else if (sQtrToPay.Trim() == "2")
                    {
                        model.DrawLine(6300, lngY5, 8000, lngY5);
                        model.SetTable(string.Format("<3250|>2300|>1000|>1000;|{0}|{1}|{2};", "Total", sTotal, sTotal));
                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        /*model.SetTable("<8800|<1000;|3rd Qtr July 20, 2013");
                        model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");*/
                        // RMC 20140119 deleted hard-coded due dates in SOA
                        model.SetTable("<8800|<1000;|" + sQtr3Due);
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                    else if (sQtrToPay.Trim() == "3")
                    {
                        model.DrawLine(6300, lngY5, 7000, lngY5);
                        model.SetTable(string.Format("<3250|>2300|>1000;|{0}|{1};", "Total", sTotal));
                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        //model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");
                        // RMC 20140119 deleted hard-coded due dates in SOA (s)
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                }

                if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "NEW")
                {
                    if (sQrt.Trim() == "1")
                    {
                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        /*model.SetTable("<8800|<1000;|2nd Qtr April 20, 2013");
                        model.SetTable("<8800|<1000;|3rd Qtr July 20, 2013");
                        model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");*/
                        // RMC 20140119 deleted hard-coded due dates in SOA (s)
                        model.SetTable("<8800|<1000;|" + sQtr2Due);
                        model.SetTable("<8800|<1000;|" + sQtr3Due);
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                    else if (sQrt.Trim() == "2")
                    {
                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        /*model.SetTable("<8800|<1000;|3rd Qtr July 20, 2013");
                        model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");*/
                        // RMC 20140119 deleted hard-coded due dates in SOA
                        model.SetTable("<8800|<1000;|" + sQtr3Due);
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                    else if (sQrt.Trim() == "3")
                    {

                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        //model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");
                        // RMC 20140119 deleted hard-coded due dates in SOA (s)
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                }
                // CJC 20130121 (e)

                if (m_objSystemUser.Load(AppSettingsManager.SystemUser.UserCode))
                {
                    sTellerName = m_objSystemUser.UserName;
                    sTellerPos = m_objSystemUser.Position;
                }
                lngY1 = model.GetCurrentY();
                /*if (sTermPay == "F") // CJC 20130816
                    model.SetCurrentY(lngY1 + 600);
                else
                    model.SetCurrentY(lngY1 + 1200); // CJC 20130816*/  // RMC 20131223

                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20140116
                model.SetParagraph(""); // RMC 20140116
                model.SetParagraph(""); // RMC 20140116
                model.SetParagraph(""); // RMC 20140116

                // GDE 20101110 city treas name temporary hard coded
                //model.SetTable("<1500|<2000|<4000|<1500|^2000;Prepared by:|" + string.Format("{0:0}", sTellerName) + "||Approved By:|" + AppSettingsManager.GetConfigValue("36"));
                model.SetTable("<1500|<2000|<4000|<1500|^2000;Prepared by:|" + string.Format("{0:0}", sTellerName) + "||Approved By:|" + AppSettingsManager.GetConfigValue("05"));  // RMC 20111228 changed soa signatory to treas
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontStyle = 9;
                //string sDate = string.Empty;

                //model.SetTable("<1500|^2000|<4000|<1500|^2000;|" + sTellerPos + "|||City Mayor");
                model.SetTable("<1500|^2000|<4000|<1500|^2000;|" + sTellerPos + "|||TREASURER");    // RMC 20111228 changed soa signatory to treas
                model.SetTable("<1500|<2000|<4000|<1500|^2000;Date Printed:|" + dDate.ToShortDateString() + "   " + dDate.ToLongTimeString() + "|||");
                model.SetTable("");

                model.SetFontSize(6);
                model.SetTable("<100|<800|<9200;»|NOTE|:This Statement is valid until " + AppSettingsManager.ValidUntil(sBin, ConfigurationAttributes.CurrentYear));
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 6;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 1;
                model.SetTable("<100|<800|=9200;»|IMPORTANT|:Please inform us of any error in this statement of account such as misspelled names, incomplete address, etcetera.  We are in the process of computerizing our taxpayer services for your future convenience.  Thank you very much.");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 6;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 1;

                PreviewDocu();
            }
        }
        public void SOALUBAO(string sBin, string sTermPay, string sQtrToPay)
        {
            long lngY1 = 0;
            long lngY2 = 0;
            long lngX1 = 0;
            long lngX2 = 0;
            long termY = 0;

            // for last paymeny info
            string sLastOrDate = string.Empty;
            string sLastTaxYear = string.Empty;
            string sLastPaymentTerm = string.Empty;
            string sLastPaymentMode = string.Empty;
            string sLastTeller = string.Empty;
            string sLastStat = string.Empty;
            string sLastQtr = string.Empty;

            model.Clear();
            sImagepath = "C:\\Documents and Settings\\Godfrey\\My Documents\\Downloads\\logo_header.jpg"; // GDE 20111209 ask if this is applicable 
            model.LeftMargin = 30;
            sList.ReturnValuesByBinQue = sBin;
            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                model.SetFontName("Arial");
                model.SetTable("");
                model.SetTable("");

                string sYear, sTerm, sQrt, sTmpGross = string.Empty, sGross = string.Empty, sParticular, sDue, sSurch, sInt, sTotal;
                double dDue, dSurch, dInt, dTotal, dGross;
                double dGrandTotDue = 0, dGrandTotSurch = 0, dGrandTotInt = 0, dGrandTotTotal = 0;
                string sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal, sGrandTotTotal1;
                string sTerm1 = string.Empty;
                string sTaxCredit = string.Empty;
                string sTellerPos = string.Empty;
                double dTaxCredit = 0;
                sQrt = string.Empty;

                if (sTermPay == "F")
                    sTerm1 = "FULL YEAR";
                else
                {
                    if (sQtrToPay.Trim() == "1")
                        sTerm1 = "QTRLY-1st Qtr";
                    if (sQtrToPay.Trim() == "2")
                        sTerm1 = "QTRLY-2nd Qtr";
                    if (sQtrToPay.Trim() == "3")
                        sTerm1 = "QTRLY-3rd Qtr";
                    if (sQtrToPay.Trim() == "4")
                        sTerm1 = "QTRLY-4th Qtr";
                }

                model.SetFontSize(10);
                if (bPreprinted == false)
                {
                    model.SetTable("^11000;Republic of the Philippines");
                    model.SetFontBold(1);
                    model.SetFontSize(12);
                    model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("09"));
                    model.SetFontBold(0);
                    model.SetFontSize(10);
                    model.SetTable("^11000;PROVINCE OF " + AppSettingsManager.GetConfigValue("08"));
                    model.SetFontBold(1);
                    if (AppSettingsManager.GetConfigValue("01") == "CITY")
                        model.SetTable("^11000;OFFICE OF THE " + AppSettingsManager.GetConfigValue("01") + " TREASURER");
                    else
                        model.SetTable("^11000;OFFICE OF THE MUNICIPAL TREASURER");
                    //model.SetTable("");
                    //model.SetFontSize(10);
                    //model.SetTable(">8150|^2900;|TERM");
                    model.SetFontSize(12);
                    model.SetTable("^11000;STATEMENT OF ACCOUNT");
                    model.SetFontBold(0);
                }
                else
                {
                    model.SetTable("");
                    model.SetTable("");
                    model.SetTable("");
                    model.SetTable("");
                    model.SetTable("");
                    model.SetTable("");
                }
                model.SetFontSize(10);
                //model.SetTable("<1300|<9700;Balance as of|" + AppSettingsManager.MonthsInWords(dDate));
                //model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                //model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 10;
                //model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
                //model.SetFontSize(15);
                //model.SetFontBold(1);
                //lngY1 = model.GetCurrentY();
                //model.SetCurrentY(2100);
                //model.SetTable(">8150|^2900;|" + sTerm1.Trim());
                //model.SetCurrentY(lngY1);
                model.SetTable("");
                model.SetFontBold(0);
                model.SetFontSize(8);
                //model.SetFontSize(15);
                model.SetFontBold(1);
                //model.SetTable("<11000;" + sBin);
                model.SetTable("");
                if (bPreprinted == false)
                {
                    //lngY1 = model.GetCurrentY();
                    //model.DrawLine(250, lngY1, 11300, lngY1);
                    //model.DrawLine(250, lngY1, 250, lngY1 + 180); // LEFT
                    //model.DrawLine(2500, lngY1, 2500, lngY1 + 180); // RIGHT
                    //model.DrawLine(3500, lngY1, 3500, lngY1 + 180); // RIGHT
                    //model.DrawLine(7900, lngY1, 7900, lngY1 + 180); // RIGHT
                    //model.DrawLine(8600, lngY1, 8600, lngY1 + 180); // RIGHT
                    //model.DrawLine(10200, lngY1, 10200, lngY1 + 180); // RIGHT
                    //model.DrawLine(11300, lngY1, 11300, lngY1 + 180); // RIGHT
                }
                //MCR 20150102 (s)
                String strYear = "";
                String sNoofEmps = "";
                String sAssetSize = "";
                String sArea = "";
                result.Query = "select max(year) from soa_tbl where bin = '" + sBin + "'";
                if (result.Execute())
                    if (result.Read())
                        strYear = result.GetString(0);
                result.Close();

                result.Query = @"select DC.default_desc,OI.data from other_info OI left join default_code DC on dc.default_code = oi.default_code
where DC.rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' and bin = '" + sBin + "' and tax_year = '" + strYear + @"' and (
dc.default_desc = 'NUMBER OF SQ METERS' or dc.default_desc = 'NUMBER OF WORKERS'
or dc.default_desc = 'ASSET SIZE')";
                if (result.Execute())
                    while (result.Read())
                    {
                        if (result.GetString(0) == "ASSET SIZE")
                            sAssetSize = result.GetDouble(1).ToString();
                        else if (result.GetString(0) == "NUMBER OF WORKERS")
                            sNoofEmps = result.GetDouble(1).ToString();
                        else if (result.GetString(0) == "NUMBER OF SQ METERS")
                            sArea = result.GetDouble(1).ToString();
                    }
                result.Close();
                //MCR 20150102 (e)

                model.SetTable("<2100|<1000|<4400|<700|<1600|<1000;BIN|STATUS|LINE/ NATURE OF BUSINESS|AREA|BILL NO.|BILL DATE");
                model.SetFontBold(0);
                if (bPreprinted == false)
                {
                    //lngY1 = model.GetCurrentY();
                    //model.DrawLine(250, lngY1, 11300, lngY1);
                    //model.DrawLine(2500, lngY1, 2500, lngY1 + 180); // RIGHT
                    //model.DrawLine(3500, lngY1, 3500, lngY1 + 180); // RIGHT
                    //model.DrawLine(7900, lngY1, 7900, lngY1 + 180); // RIGHT
                    //model.DrawLine(8600, lngY1, 8600, lngY1 + 180); // RIGHT
                    //model.DrawLine(10200, lngY1, 10200, lngY1 + 180); // RIGHT
                }
                model.SetTable("<2100|<1000|<4400|<700|<1600|<1000;" + sBin + "|" + sList.BPLSAppSettings[i].sBnsStat.Trim() + "|" + AppSettingsManager.GetBnsDesc(sList.BPLSAppSettings[i].sBnsCode.Trim()) + "|" + sArea + "|" + AppSettingsManager.GetBillNoAndDate(sBin, sSOATaxYear, sList.BPLSAppSettings[i].sBnsCode.Trim(), 0) + "|" + AppSettingsManager.GetBillNoAndDate(sBin, sSOATaxYear, sList.BPLSAppSettings[i].sBnsCode.Trim(), 1));
                model.SetFontBold(1);

                if (bPreprinted == false)
                {
                    //lngY1 = model.GetCurrentY();
                    //model.DrawLine(250, lngY1, 11300, lngY1);
                    //model.DrawLine(250, lngY1, 250, lngY1 + 180); // LEFT
                    //model.DrawLine(5300, lngY1, 5300, lngY1 + 180); // RIGHT
                    //model.DrawLine(10200, lngY1, 10200, lngY1 + 180); // RIGHT
                    //model.DrawLine(11300, lngY1, 11300, lngY1 + 180); // RIGHT
                }
                model.SetTable("<4900|<4900|<1000;BUSINESS NAME|BUSINESS ADDRESS|YEAR");
                model.SetFontBold(0);

                if (bPreprinted == false)
                {
                    //lngY1 = model.GetCurrentY();
                    //model.DrawLine(250, lngY1, 11300, lngY1);
                    //model.DrawLine(5300, lngY1, 5300, lngY1 + 180); // RIGHT
                    //model.DrawLine(10200, lngY1, 10200, lngY1 + 180); // RIGHT
                }
                model.SetTable("<4900|<4900|<1000;" + m_strBnsNm + "|" + m_strBnsAdd + "|" + sSOATaxYear.Trim());
                model.SetFontBold(1);
                if (bPreprinted == false)
                {
                    //lngY1 = model.GetCurrentY();
                    //model.DrawLine(250, lngY1, 11300, lngY1);
                    //model.DrawLine(250, lngY1, 250, lngY1 + 180); // LEFT
                    //model.DrawLine(3800, lngY1, 3800, lngY1 + 180); // RIGHT
                    //model.DrawLine(5800, lngY1, 5800, lngY1 + 180); // RIGHT
                    //model.DrawLine(8600, lngY1, 8600, lngY1 + 180); // RIGHT
                    //model.DrawLine(10200, lngY1, 10200, lngY1 + 180); // RIGHT
                    //model.DrawLine(11300, lngY1, 11300, lngY1 + 180); // RIGHT
                }
                model.SetTable("<3400|<2000|<2800|<1600|<1000;OWNER/ TAXPAYER|LAST PAYMENT|PREVIOUS PERMIT NO.|DATE ISSUED|PERIOD");
                model.SetFontBold(0);

                if (bPreprinted == false)
                {
                    //lngY1 = model.GetCurrentY();
                    //model.DrawLine(250, lngY1, 11300, lngY1);
                    //model.DrawLine(3800, lngY1, 3800, lngY1 + 180); // RIGHT
                    //model.DrawLine(5800, lngY1, 5800, lngY1 + 180); // RIGHT
                    //model.DrawLine(8600, lngY1, 8600, lngY1 + 180); // RIGHT
                    //model.DrawLine(10200, lngY1, 10200, lngY1 + 180); // RIGHT
                }
                model.SetTable("<3400|<2000|<2800|<1600|<1000;" + AppSettingsManager.GetBnsOwner(m_strBnsOwn) + "|" + AppSettingsManager.GetLastPaymentInfo(sBin, "AMOUNT") + "|" + sList.BPLSAppSettings[i].sPermitNo + "|" + sList.BPLSAppSettings[i] .sPermitDate+ "|" + sTerm1);
                if (bPreprinted == false)
                {
                    //lngY1 = model.GetCurrentY();
                    //model.DrawLine(250, lngY1, 11300, lngY1);
                }
                //model.SetFontBold(0);
                //model.SetFontSize(10);
                //model.SetTable("<11000;Business Identification Number");
                model.SetTable("");

                model.SetFontSize(8);
                /*
                model.SetTable(">10500;BILL NUMBER & DATE:   " + AppSettingsManager.GetBillNoAndDate(sBin, sSOATaxYear, sList.BPLSAppSettings[i].sBnsCode.Trim()));
                model.SetFontBold(1);
                model.SetTableBorder(0);
                lngY1 = model.GetCurrentY();
                model.DrawLine(250, lngY1, 11300, lngY1);
                model.SetTable("");

                model.SetTable("<1800|<4000|<1400|<4000;BUSINESS NAME|: " + sList.BPLSAppSettings[i].sBnsNm.Trim() + "|DESCRIPTION|: " + AppSettingsManager.GetBnsDesc(sList.BPLSAppSettings[i].sBnsCode.Trim()));
                model.SetTable("<1800|<4000|<1400|<4000;BUSINESS NAME|: " + m_strBnsNm + "|DESCRIPTION|: " + AppSettingsManager.GetBnsDesc(sList.BPLSAppSettings[i].sBnsCode.Trim())); // CJC 20130816
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                //model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS ADDRESS|: " + AppSettingsManager.GetBnsAddress(sBin.Trim()) + "|PREV. STATUS|: " + sList.BPLSAppSettings[i].sBnsStat.Trim() + "|TAX YEAR|: " + sSOATaxYear.Trim());
                //model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS ADDRESS|: " + m_strBnsAdd + "|PREV. STATUS|: " + sList.BPLSAppSettings[i].sBnsStat.Trim() + "|TAX YEAR|: " + sSOATaxYear.Trim()); // CJC 20130816
                model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS ADDRESS|: " + m_strBnsAdd + "|STATUS|: " + sList.BPLSAppSettings[i].sBnsStat.Trim() + "|TAX YEAR|: " + sSOATaxYear.Trim()); // RMC 20140708 added retirement billing
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[5].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontStyle = 0;
                //model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS OWNER|: " + AppSettingsManager.GetBnsOwner(sList.BPLSAppSettings[i].sOwnCode.Trim()) + "|LAST PAYMENT|: P " + AppSettingsManager.GetLastPaymentInfo(sBin, "AMOUNT") + "|LAST OR#|: " + AppSettingsManager.GetLastPaymentInfo(sBin, "OR"));                
                model.SetTable("<1800|<4000|<1400|<1700|<1000|<1300;BUSINESS OWNER|: " + AppSettingsManager.GetBnsOwner(m_strBnsOwn) + "|LAST PAYMENT|: P " + AppSettingsManager.GetLastPaymentInfo(sBin, "AMOUNT") + "|LAST OR#|: " + AppSettingsManager.GetLastPaymentInfo(sBin, "OR")); // CJC 20131494
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[5].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[5].Font.FontStyle = 0;
                model.SetTable("<1800|<4000|<1400|<2000|<1000|<1000;LAST PERMIT NO|: " + sList.BPLSAppSettings[i].sPermitNo + "|DATE ISSUED|: ||");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

                model.Tables[model.Tables.Count - 1].Items[3].Font.FontName = "Arial Narrow";
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[3].Font.FontStyle = 0;
                */
                model.SetTable("");
                //lngY2 = model.GetCurrentY();
                //model.DrawLine(250, lngY2, 11300, lngY2);
                //model.DrawLine(250, lngY1, 250, lngY2);
                //model.DrawLine(11300, lngY1, 11300, lngY2);
                model.SetTableBorder(0);
                model.SetFontBold(0);
                model.SetFontName("Arial Narrow");
                model.SetFontSize(9);
                model.SetTable("");
                if (bPreprinted == false)
                {
                    model.SetFontSize(10);
                    lngY1 = model.GetCurrentY();
                    model.DrawLine(250, lngY1, 11300, lngY1);
                    model.DrawLine(250, lngY1, 250, lngY1 + 180); // LEFT
                    //model.DrawLine(1000, lngY1, 1000, lngY1 + 180); // RIGHT
                    //model.DrawLine(1700, lngY1, 1700, lngY1 + 180); // RIGHT
                    //model.DrawLine(4100, lngY1, 4100, lngY1 + 180); // RIGHT
                    //model.DrawLine(5300, lngY1, 5300, lngY1 + 180); // RIGHT
                    //model.DrawLine(6800, lngY1, 6800, lngY1 + 180); // RIGHT
                    //model.DrawLine(8300, lngY1, 8300, lngY1 + 180); // RIGHT
                    //model.DrawLine(9800, lngY1, 9800, lngY1 + 180); // RIGHT
                    model.DrawLine(11300, lngY1, 11300, lngY1 + 180); // RIGHT

                    model.SetTable("^600|^700|<2400|>1200|>1500|>1500|>1500|>1500;YEAR|PERIOD|PARTICULARS|GROSS / CAPITAL|DUE|SURCHARGE|INTEREST|TOTAL");

                    lngY1 = model.GetCurrentY();
                    model.DrawLine(250, lngY1, 11300, lngY1);
                    model.SetFontSize(9);
                }
                else
                    model.SetTable("");
                model.SetTable("");
                model.SetTable("");
                //// TERM
                //model.DrawLine(8850, 1400, 11300, 1400); // TOP
                //model.DrawLine(8850, 2700, 11300, 2700); // BOTTOM
                //model.DrawLine(8850, 1800, 11300, 1800); // UPPER PORTION FOR "TERM" CAPTION
                //model.DrawLine(8850, 1400, 8850, 2700); // LEFT
                //model.DrawLine(11300, 1400, 11300, 2700); // RIGHT
                //// TERM
                //model.SetTable("");

                model.SetFontSize(10);
                result.Query = "select * from soa_tbl where bin = '" + sBin + "' order by year asc, fees_code_sort desc, qtr asc";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        sYear = result.GetString("year").Trim();
                        sTerm = result.GetString("term").Trim();
                        sQrt = result.GetString("qtr").Trim();
                        sParticular = result.GetString("particulars").Trim();

                        double.TryParse(result.GetDouble("gross").ToString(), out dGross);
                        double.TryParse(result.GetDouble("due").ToString(), out dDue);
                        double.TryParse(result.GetDouble("surch").ToString(), out dSurch);
                        double.TryParse(result.GetDouble("pen").ToString(), out dInt);
                        double.TryParse(result.GetDouble("total").ToString(), out dTotal);
                        dGrandTotDue = dGrandTotDue + dDue;
                        dGrandTotSurch = dGrandTotSurch + dSurch;
                        dGrandTotInt = dGrandTotInt + dInt;
                        dGrandTotTotal = dGrandTotTotal + dTotal;

                        sGross = string.Format("{0:#,##0.00}", dGross);
                        sDue = string.Format("{0:#,##0.00}", dDue);
                        sSurch = string.Format("{0:#,##0.00}", dSurch);
                        sInt = string.Format("{0:#,##0.00}", dInt);
                        sTotal = string.Format("{0:#,##0.00}", dTotal);

                        if (sGross == "0.00")
                            sGross = "";

                        if (AppSettingsManager.GetConfigValue("10") == "232")    // RMC 20140108 hide gross/capital in soa
                        {
                            sGross = "";
                        }

                        model.SetTable(string.Format("^600|^700|<2400|>1200|>1500|>1500|>1500|>1500;{0}|{1} {2}|{3}|{4}|{5}|{6}|{7}|{8};", sYear, sTerm, sQrt, sParticular, sGross, sDue, sSurch, sInt, sTotal));
                    }
                }
                result.Close();
                model.SetFontSize(9);

                // tax credit
                //result.Query = "select * from dbcr_memo where own_code in (select own_code from businesses where bin = '" + sBin.Trim() + "') and memo like 'REMAINING BALANCE AFTER PAYMENT%' and served = 'N'"; // GDE 20111021
                //result.Query = "select * from dbcr_memo where bin = '" + sBin.Trim() + "' and memo like 'REMAINING BALANCE AFTER PAYMENT%' and served = 'N' and multi_pay = 'N'";
                //result.Query = "select * from dbcr_memo where bin = '" + sBin.Trim() + "' and (memo like 'REMAINING BALANCE AFTER PAYMENT%' or memo like 'REMAINING BALANCE AFTER ADDING OF TAX CREDIT%') and served = 'N' and multi_pay = 'N'";    // RMC 20140113 corrected display of added tax credit in SOA
                //result.Query = "select * from dbcr_memo where own_code in (select own_code from businesses where bin = '" + sBin.Trim() + "') and (memo like 'REMAINING BALANCE AFTER PAYMENT%' or memo like 'REMAINING BALANCE AFTER ADDING OF TAX CREDIT%') and served = 'N' and multi_pay = 'N'";    //JARS 20170713
                result.Query = "select * from dbcr_memo where bin = '" + sBin.Trim() + "' and (memo like 'REMAINING BALANCE AFTER PAYMENT%' or memo like 'REMAINING BALANCE AFTER ADDING OF TAX CREDIT%') and served = 'N' and multi_pay = 'N'";    // RMC 20140113 corrected display of added tax credit in SOA //JARS 20171010
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dTaxCredit = result.GetDouble("credit");
                    }
                }
                result.Close();
                // tax credit
                sGrandTotDue = string.Format("{0:#,##0.00}", dGrandTotDue);
                sGrandTotSurch = string.Format("{0:#,##0.00}", dGrandTotSurch);
                sGrandTotInt = string.Format("{0:#,##0.00}", dGrandTotInt);
                sTaxCredit = string.Format("{0:#,##0.00}", dTaxCredit);
                sGrandTotTotal = string.Format("{0:#,##0.00}", dGrandTotTotal);
                sGrandTotTotal1 = string.Format("{0:#,##0.00}", dGrandTotTotal - dTaxCredit);


                model.SetTable("");
                lngY1 = model.GetCurrentY();
                model.DrawLine(5500, lngY1, 11300, lngY1);
                model.SetTable(string.Format(">4900|>1500|>1500|>1500|>1500;Total:|{0}|{1}|{2}|{3};", sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal));
                //model.SetTable(string.Format(">4900|>1500|>1500|>1500|>1500;Less Available Tax Credit:||||{0};", sTaxCredit));
                
                /*
                model.SetTable("");
                model.SetTable("");

                lngY1 = model.GetCurrentY();
                // TOTAL DUE
                model.SetFontSize(8);
                model.SetFontName("Arial");
                model.DrawLine(250, lngY1, 2750, lngY1); // TOP
                model.SetTable("<2500|<2500|<3200|<2500; Total Amount Due| Surcharge and Interest| Less Available Tax Credit|   Grand Total Due");
                model.DrawLine(250, lngY1 + 900, 2750, lngY1 + 900); // BOTTOM
                model.DrawLine(250, lngY1 + 200, 2750, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(250, lngY1, 250, lngY1 + 900); // LEFT
                model.DrawLine(2750, lngY1, 2750, lngY1 + 900); // RIGHT

                // SURCH AND INTEREST
                model.DrawLine(2750, lngY1, 5250, lngY1); // TOP
                model.DrawLine(2750, lngY1 + 900, 5250, lngY1 + 900); // BOTTOM
                model.DrawLine(2750, lngY1 + 200, 5250, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(2750, lngY1, 2750, lngY1 + 900); // LEFT
                model.DrawLine(5250, lngY1, 5250, lngY1 + 900); // RIGHT
                model.SetTable("");


                string sTotSurchAndPen = string.Empty;
                sTotSurchAndPen = string.Format("{0:#,###.#0}", dGrandTotSurch + dGrandTotInt);
                //model.SetTable("<2500|<2500|<2500|<700|^2700;P " + sGrandTotDue + "|P " + sTotSurchAndPen + "|P " + sTaxCredit + "||P " + sGrandTotTotal);
                model.SetTable("<2500|<2500|<2500|<700|^2700;P " + sGrandTotDue + "|P " + sTotSurchAndPen + "|P " + sTaxCredit + "||P " + sGrandTotTotal1); // RMC 20140113 corrected display of added tax credit in SOA
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontSize = 12;
                model.Tables[model.Tables.Count - 1].Items[2].Font.FontStyle = 0;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontSize = 14;
                model.Tables[model.Tables.Count - 1].Items[4].Font.FontStyle = 1;
                // TAX CREDIT
                model.DrawLine(5250, lngY1, 7750, lngY1); // TOP
                model.DrawLine(5250, lngY1 + 900, 7750, lngY1 + 900); // BOTTOM
                model.DrawLine(5250, lngY1 + 200, 7750, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(5250, lngY1, 5250, lngY1 + 900); // LEFT
                model.DrawLine(7750, lngY1, 7750, lngY1 + 900); // RIGHT
                // GRAND TOTAL
                model.DrawLine(8650, lngY1, 11300, lngY1); // TOP
                model.DrawLine(8650, lngY1 + 900, 11300, lngY1 + 900); // BOTTOM
                model.DrawLine(8650, lngY1 + 200, 11300, lngY1 + 200); // UPPER PORTION FOR "TERM" CAPTION
                model.DrawLine(8650, lngY1, 8650, lngY1 + 900); // LEFT
                model.DrawLine(11300, lngY1, 11300, lngY1 + 900); // RIGHT

                model.SetTable("");
                model.SetTable("");
                /*model.SetTable("");
                model.SetTable("");*/
                // RMC 20140116 adjustment in SOA printing

                /*

                double dblDBCRAmount = 0;
                result.Query = "select sum(dbcr_amount) as dbcr_amount from tmp_dbcr where bin = '" + sBin.Trim() + "' and own_code = '" + AppSettingsManager.GetOwnCode(sBin.Trim()) + "' and dbcr_type = 'DEBIT' and served = 'N'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dblDBCRAmount = result.GetDouble("dbcr_amount");
                        if (dblDBCRAmount > 0)
                            model.SetTable("<11500;* A total of P" + dblDBCRAmount.ToString() + " over payment resulting from Adjustment/Revenue Examination of this record.;");

                    }
                }
                result.Close();
                result.Query = "select exempt_type from boi_table where bin = '" + sBin.Trim() + "'";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        model.SetTable("<11500;*" + result.GetString("exempt_type") + " member;");

                    }
                }
                result.Close();
                model.SetTable("");
                model.SetTable("");

                // CJC 20130816
                string sPUTStrTmp;
                string sPrevData = "";
                string sPUTRem = "";
                OracleResultSet sQuery = new OracleResultSet();
                sQuery.Query = "select distinct appl_type from permit_update_appl";
                sQuery.Query += " where bin = '" + sBin + "'";
                sQuery.Query += " and tax_year = '" + sSOATaxYear + "'";
                sQuery.Query += " and data_mode  = 'QUE'";
                if (sQuery.Execute())
                {
                    while (sQuery.Read())
                    {
                        sPUTStrTmp = sQuery.GetString("appl_type").Trim();
                        if (sPUTStrTmp == "TOWN")
                        {
                            sPrevData += "Prev Own :" + AppSettingsManager.GetBnsOwner(sList.BPLSAppSettings[i].sOwnCode.Trim()) + "\n";
                            sPUTStrTmp = "CHANGE OWN";
                        }
                        if (sPUTStrTmp == "TLOC")
                        {
                            sPrevData += "Prev Loc :" + AppSettingsManager.GetBnsAddress(sBin.Trim()) + "\n";
                            sPUTStrTmp = "TRANSFER LOC";
                        }
                        if (sPUTStrTmp == "ADDL")
                        {
                            sPUTStrTmp = "ADDL BNS";
                        }
                        if (sPUTStrTmp == "CTYP")
                        {
                            sPrevData += "Change Class.:" + "Test" + "\n";
                            sPUTStrTmp = "CHANGE CLASS";
                        }
                        //(s) RTL 02212006 for new business name
                        if (sPUTStrTmp == "CBNS")
                        {
                            sPrevData += "Prev Bns Name :" + sList.BPLSAppSettings[i].sBnsNm.Trim() + "\n";
                            sPUTStrTmp = "CHANGE BNS NAME";
                        }
                        //(e) RTL 02212006 for new business name
                        sPUTRem = sPUTRem + "/" + sPUTStrTmp;

                    }
                }
                sQuery.Close();
                //////////

                if (sPUTRem != "")   // RMC 20131223
                    model.SetTable("<11500;*" + sPUTRem);
                if (sPrevData != "") // RMC 20131223
                    model.SetTable("<11500;*" + sPrevData);

                lngY1 = model.GetCurrentY();

                //model.DrawLine(250, lngY1 + 200, 11300, lngY1 + 200); // TOP  // RMC 20140106 PUT REM
                //model.SetCurrentY(lngY1 + 400);   // RMC 20140106 PUT REM
                model.SetTable("<11000;");  // RMC 20140106
                model.SetTable("<11000;");  // RMC 20140106
                */

                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20140116
                model.SetParagraph(""); // RMC 20140116
                model.SetParagraph(""); // RMC 20140116

                model.SetFontSize(10);
                if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "REN")
                {
                    int iYearTmp = Convert.ToInt32(sSOATaxYear) - 1;
                    result.Query = "select capital,gross from bill_gross_info where tax_year = '" + iYearTmp + "' and bin = '" + sBin.Trim() + "' and bns_stat = 'REN'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            model.SetTable("<8500;Previous Capital: " + result.GetDouble(0).ToString("#,##0.00"));
                            model.SetTable("<8500;Lastgross Receipts: " + result.GetDouble(1).ToString("#,##0.00"));
                        }
                        else
                        {
                            result.Close();
                            result.Query = "select capital,gr_1 from businesses where tax_year = '" + iYearTmp + "' and bin = '" + sBin.Trim() + "' and bns_stat = 'REN'";
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    model.SetTable("<8500;Previous Capital: " + result.GetDouble("capital").ToString("#,##0.00"));
                                    model.SetTable("<8500;Lastgross Receipts: " + result.GetDouble("gr_1").ToString("#,##0.00"));
                                }
                            }
                        }
                    }
                    result.Close();
                }

                model.SetTable("<8500;Number of Employees: " + sNoofEmps);
                model.SetTable("<8500;Capital: " + Convert.ToDouble(sAssetSize).ToString("#,##0.00"));
                model.SetTable("");
                model.SetTable("<8500;This Statement is valid until " + AppSettingsManager.ValidUntil(sBin, ConfigurationAttributes.CurrentYear));
                model.SetTable("");
                model.SetTable("<1000|=9000;IMPORTANT:| Please inform us of any error in this statement of account such as misspelled names, incomplete address, etcetera.  We are in the process of computerizing our taxpayer services for your future convenience.  Thank you for bearing with us.");
                model.SetFontSize(10);
                /*
                model.SetFontBold(1);
                model.SetFontUnderline(1);
                model.SetTable("<11000; Additional Details of Previous Payment");
                model.SetFontBold(0);
                model.SetFontUnderline(0);

                result.Query = "select distinct * from pay_hist where bin = '" + sBin.Trim() + "' order by or_date desc, tax_year desc";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sLastOrDate = result.GetDateTime("or_date").ToShortDateString();
                        sLastTaxYear = result.GetString("tax_year").Trim();
                        sLastQtr = result.GetString("qtr_paid").Trim();
                        sLastPaymentTerm = result.GetString("payment_term").Trim();
                        sLastPaymentMode = result.GetString("data_mode").Trim();
                        sLastTeller = result.GetString("teller").Trim();
                        sLastStat = result.GetString("bns_stat").Trim();
                    }
                }
                result.Close();

                if (sLastQtr.Trim() == "F")
                    sLastQtr = "FULL";
                else
                {
                    if (sLastQtr == "1")
                        sLastQtr = "1st Qtr";
                    if (sLastQtr == "2")
                        sLastQtr = "2nd Qtr";
                    if (sLastQtr == "3")
                        sLastQtr = "3rd Qtr";
                    if (sLastQtr == "4")
                        sLastQtr = "4th Qtr";
                }

                if (sLastPaymentTerm.Trim() == "F")
                    sLastPaymentTerm = "Full Payment";
                else if (sLastPaymentTerm.Trim() == "") // RMC 20131223 FOR NEWLY APPLIED BUSINESS, NO RECORD OF PREVIOUS PAYMENT
                    sLastPaymentTerm = "";
                else
                    sLastPaymentTerm = "Installment";

                if (sLastPaymentMode.Trim() == "POS")
                    sLastPaymentMode = "Posting";
                else if (sLastPaymentMode.Trim() == "UNP")
                    sLastPaymentMode = "For Posting";
                else if (sLastPaymentMode.Trim() == "OFL")
                    sLastPaymentMode = "Offline Payment";
                else if (sLastPaymentMode.Trim() == "ONL")
                    sLastPaymentMode = "Online Payment";

                if (sLastStat.Trim() == "NEW")
                    sLastStat = "New Business";
                else if (sLastStat.Trim() == "REN")
                    sLastStat = "Renewal";
                else
                {
                    if (sLastPaymentMode == "" && sLastStat == "")  // RMC 20131223 FOR NEWLY APPLIED BUSINESS, NO RECORD OF PREVIOUS PAYMENT
                        sLastStat = "";
                    else
                        sLastStat = "Retired Business";
                }

                if (m_objSystemUser.Load(sLastTeller.Trim()))
                {
                    sLastTeller = m_objSystemUser.UserName;
                }



                model.SetTable("<1500|<10000; O.R. Date|: " + sLastOrDate);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Tax Year / Qtr|: " + sLastTaxYear + " - " + sLastQtr);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Payment Term|: " + sLastPaymentTerm);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Payment Mode|: " + sLastPaymentMode);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Teller|: " + sLastTeller);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.SetTable("<1500|<10000; Business Status|: " + sLastStat);
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
                model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
                model.DrawLine(250, lngY1 + 1800, 11300, lngY1 + 1800); // BOTTOM LINE

                // GDE 20130215 (s)
                if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "NEW")
                {
                    model.SetCurrentY(lngY1 + 400);
                    model.SetFontBold(1);
                    model.SetFontUnderline(1);
                    model.SetTable("<3250|<3000|<2500|<2000; | ||");
                    model.SetFontSize(7);
                    model.SetCurrentY(lngY1 + 400);
                    model.SetTable("<3250|<3000|<2400|<2000; |||Balance to be paid On or Before:");
                    model.SetFontSize(8);
                    model.SetFontBold(0);
                    model.SetFontUnderline(0);
                    model.SetFontSize(2);
                    model.SetTable("");
                    model.SetFontSize(8);
                }
                // GDE 20130215 (E)

                // RMC 20140119 deleted hard-coded due dates in SOA (s)
                string sQtr2Due = string.Empty;
                string sQtr3Due = string.Empty;
                string sQtr4Due = string.Empty;

                result.Query = "select * from due_dates where due_year = '" + sSOATaxYear + "' order by due_date";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        if (result.GetString("due_desc") == "APRIL")
                            sQtr2Due = "2nd Qtr " + string.Format("{0:MMMM dd, yyyy}", result.GetDateTime("due_date"));
                        if (result.GetString("due_desc") == "JULY")
                            sQtr3Due = "3rd Qtr " + string.Format("{0:MMMM dd, yyyy}", result.GetDateTime("due_date"));
                        if (result.GetString("due_desc") == "OCTOBER")
                            sQtr4Due = "4th Qtr " + string.Format("{0:MMMM dd, yyyy}", result.GetDateTime("due_date"));
                    }
                }
                result.Close();
                // RMC 20140119 deleted hard-coded due dates in SOA (e)

                // CJC 20130121 (s)
                if (sTermPay == "I")
                {
                    model.SetCurrentY(lngY1 + 400);
                    model.SetFontBold(1);
                    model.SetFontUnderline(1);
                    model.SetTable("<3250|<3000|<2500|<2000; |Remaining Quarter Dues||");
                    model.SetFontSize(7);
                    model.SetCurrentY(lngY1 + 400);
                    model.SetTable("<3250|<3000|<2400|<2000; |||Balance to be paid On or Before:");
                    model.SetFontSize(8);
                    model.SetFontBold(0);
                    model.SetFontUnderline(0);
                    model.SetFontSize(2);
                    model.SetTable("");
                    model.SetFontSize(8);

                    string sTaxCode = string.Empty;
                    string sBnsCode = string.Empty;
                    bool blRunOnce = false;
                    dTotal = 0.0;
                    sParticular = string.Empty;
                    sTotal = string.Empty;

                    //result.Query = "select * from taxdues where bin = '" + sBin + "' and tax_code in ('B', '02') and tax_year = '" + sSOATaxYear + "' order by tax_year desc, tax_code desc";
                    result.Query = "select * from taxdues where bin = '" + sBin + "' and tax_code = 'B' and tax_year = '" + sSOATaxYear + "' order by tax_year desc, tax_code desc";
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            sBnsCode = result.GetString("bns_code_main").Trim();
                            sTaxCode = result.GetString("tax_code").Trim();
                            if (sTaxCode == "B")
                                sParticular = AppSettingsManager.GetBnsDesc(sBnsCode);
                            else
                                sParticular = AppSettingsManager.GetFeesDesc(sTaxCode);

                            double.TryParse(result.GetDouble("amount").ToString(), out dDue);
                            dTotal = dTotal + dDue;
                            sDue = string.Format("{0:#,##0.00}", dDue / 4);
                            sTotal = string.Format("{0:#,##0.00}", dTotal / 4);

                            if (sQtrToPay.Trim() == "1")
                            {
                                if (!blRunOnce)
                                {
                                    model.SetFontBold(1);
                                    model.SetTable("<3250|<2300|^1000|^1000|^1000;|Particulars|2nd Qtr|3rd Qtr|4th Qtr");
                                    model.SetFontSize(2);
                                    model.SetTable("");
                                    model.SetFontSize(7);
                                    model.SetFontBold(0);
                                    blRunOnce = true;
                                }
                                model.SetTable(string.Format("<3250|<2300|>1000|>1000|>1000;|{0}|{1}|{2}|{3};", sParticular, sDue, sDue, sDue));
                            }
                            else if (sQtrToPay.Trim() == "2")
                            {
                                if (!blRunOnce)
                                {
                                    model.SetFontBold(1);
                                    model.SetTable("<3250|<2300|^1000|^1000;|Particulars|3rd Qtr|4th Qtr");
                                    model.SetFontSize(2);
                                    model.SetTable("");
                                    model.SetFontSize(7);
                                    model.SetFontBold(0);
                                    blRunOnce = true;
                                }
                                model.SetTable(string.Format("<3250|<2300|>1000|>1000;|{0}|{1}|{2};", sParticular, sDue, sDue));
                            }
                            else if (sQtrToPay.Trim() == "3")
                            {
                                if (!blRunOnce)
                                {
                                    model.SetFontBold(1);
                                    model.SetTable("<3250|<2300|^1000;|Particulars|4th Qtr");
                                    model.SetFontSize(2);
                                    model.SetTable("");
                                    model.SetFontSize(7);
                                    model.SetFontBold(0);
                                    blRunOnce = true;
                                }
                                model.SetTable(string.Format("<3250|<2300|>1000;|{0}|{1};", sParticular, sDue));
                            }
                        }
                    }
                    result.Close();

                    model.SetFontSize(2);
                    model.SetTable("");
                    model.SetFontSize(7);
                    model.SetFontBold(1);

                    long lngY5 = model.GetCurrentY();



                    if (sQtrToPay.Trim() == "1")
                    {
                        model.DrawLine(6300, lngY5, 9000, lngY5);
                        model.SetTable(string.Format("<3250|>2300|>1000|>1000|>1000;|{0}|{1}|{2}|{3};", "Total", sTotal, sTotal, sTotal));
                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        /*model.SetTable("<8800|<1000;|2nd Qtr April 20, 2013");
                        model.SetTable("<8800|<1000;|3rd Qtr July 20, 2013");
                        model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");
                        // RMC 20140119 deleted hard-coded due dates in SOA (s)
                        model.SetTable("<8800|<1000;|" + sQtr2Due);
                        model.SetTable("<8800|<1000;|" + sQtr3Due);
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                    else if (sQtrToPay.Trim() == "2")
                    {
                        model.DrawLine(6300, lngY5, 8000, lngY5);
                        model.SetTable(string.Format("<3250|>2300|>1000|>1000;|{0}|{1}|{2};", "Total", sTotal, sTotal));
                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        /*model.SetTable("<8800|<1000;|3rd Qtr July 20, 2013");
                        model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");
                        // RMC 20140119 deleted hard-coded due dates in SOA
                        model.SetTable("<8800|<1000;|" + sQtr3Due);
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                    else if (sQtrToPay.Trim() == "3")
                    {
                        model.DrawLine(6300, lngY5, 7000, lngY5);
                        model.SetTable(string.Format("<3250|>2300|>1000;|{0}|{1};", "Total", sTotal));
                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        //model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");
                        // RMC 20140119 deleted hard-coded due dates in SOA (s)
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                }

                if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "NEW")
                {
                    if (sQrt.Trim() == "1")
                    {
                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        /*model.SetTable("<8800|<1000;|2nd Qtr April 20, 2013");
                        model.SetTable("<8800|<1000;|3rd Qtr July 20, 2013");
                        model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");
                        // RMC 20140119 deleted hard-coded due dates in SOA (s)
                        model.SetTable("<8800|<1000;|" + sQtr2Due);
                        model.SetTable("<8800|<1000;|" + sQtr3Due);
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                    else if (sQrt.Trim() == "2")
                    {
                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        /*model.SetTable("<8800|<1000;|3rd Qtr July 20, 2013");
                        model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");
                        // RMC 20140119 deleted hard-coded due dates in SOA
                        model.SetTable("<8800|<1000;|" + sQtr3Due);
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                    else if (sQrt.Trim() == "3")
                    {

                        model.SetCurrentY(lngY1 + 600);
                        model.SetFontBold(0);
                        //model.SetTable("<8800|<1000;|4th Qtr October 20, 2013");
                        // RMC 20140119 deleted hard-coded due dates in SOA (s)
                        model.SetTable("<8800|<1000;|" + sQtr4Due);
                        // RMC 20140119 deleted hard-coded due dates in SOA (e)
                    }
                }
                // CJC 20130121 (e)
                */
                if (m_objSystemUser.Load(AppSettingsManager.SystemUser.UserCode))
                {
                    sTellerName = m_objSystemUser.UserName;
                    sTellerPos = m_objSystemUser.Position;
                }
                /*lngY1 = model.GetCurrentY();
                if (sTermPay == "F") // CJC 20130816
                    model.SetCurrentY(lngY1 + 600);
                else
                    model.SetCurrentY(lngY1 + 1200); // CJC 20130816*/
                // RMC 20131223
                

                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20131223
                model.SetParagraph(""); // RMC 20140116
                model.SetParagraph(""); // RMC 20140116
                model.SetParagraph(""); // RMC 20140116
                model.SetParagraph(""); // RMC 20140116

                // GDE 20101110 city treas name temporary hard coded
                //model.SetTable("<1500|<2000|<4000|<1500|^2000;Prepared by:|" + string.Format("{0:0}", sTellerName) + "||Approved By:|" + AppSettingsManager.GetConfigValue("36"));
                //model.SetTable("<1500|<2000|<4000|<1500|^2000;|" + string.Format("{0:0}", sTellerName) + "||Approved By:|" + AppSettingsManager.GetConfigValue("05"));  // RMC 20111228 changed soa signatory to treas
                //string sDate = string.Empty;

                //model.SetTable("<1500|^2000|<4000|<1500|^2000;|" + sTellerPos + "|||City Mayor");
                //model.SetTable("<1500|^2000|<4000|<1500|^2000;|" + sTellerPos + "|||TREASURER");    // RMC 20111228 changed soa signatory to treas
                //model.SetTable("<1500|<2000|<4000|<1500|^2000;Date Printed:|" + dDate.ToShortDateString() + "   " + dDate.ToLongTimeString() + "|||");

                model.SetFontSize(10);
                model.SetTable(string.Format(">4400|<2500|<1500|<2500;|Assessed By:||Approved By:"));
                model.SetTable(string.Format(">4400|<2500|<1500|<2500;|||;"));
                model.SetTable(string.Format(">4400|<2500|<1500|<2500;|||;"));
                model.SetTable(string.Format(">4400|<2500|<1500|<2500;|{0}||{1};", AppSettingsManager.GetConfigValue("03"), AppSettingsManager.GetConfigValue("05")));
                model.SetTable(string.Format(">4400|<2500|<1500|<2500;|BPLO HEAD||MUNICIPAL TREASURER;"));
                
                model.SetTable("");
                model.SetTable("");
                model.SetTable("");

                model.SetTable("<1500|<2000;Prepared by:|" + string.Format("{0:0}", sTellerName));  // RMC 20111228 changed soa signatory to treas
                model.SetTable("<1500|<2000;Receieved SOA:|");  // RMC 20111228 changed soa signatory to treas
                model.SetTable("<1500|<2000;By:|");  // RMC 20111228 changed soa signatory to treas
                model.SetTable("<1500|<2000|<4000|<1500|^2000;Date Printed:|" + dDate.ToShortDateString() + "   " + dDate.ToLongTimeString() + "|||");

                /*model.SetTable("<100|<800|<9200;»|NOTE|:This Statement is valid until " + AppSettingsManager.ValidUntil(sBin, ConfigurationAttributes.CurrentYear));
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 6;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 1;
                model.SetTable("<100|<800|=9200;»|IMPORTANT|:Please inform us of any error in this statement of account such as misspelled names, incomplete address, etcetera.  We are in the process of computerizing our taxpayer services for your future convenience.  Thank you very much.");
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 6;
                model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 1;
                */
                PreviewDocu();
            }
        }

        public void SpecialPermit(string sSPLNo)
        {
            model.Clear();
            //sImagepath = "D:\\Rachel\\Projects\\Mati\\SourceCodes_fr_Godfrey\\_Latest 20131219\\aBPLs-e\\BPS\\BPS\\Resources\\matilogo_ilove.png";
            sImagepath = "..\\BPLS-GUI\\BPS\\matilogo_ilove.png";
            model.SetFontSize(12);
            model.SetTable("^11000;Republic of the Philippines");
            model.SetTable("^11000;Office of the City Mayor");
            model.SetFontSize(14);
            model.SetTable("^11000;BUSINESS PERMIT & LICENSING OFFICE");
            model.SetTable("");
            model.SetFontSize(10);
            /*
            model.SetTable(">8150|^2900;|TERM");
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetTable("<11000;Statement of Account");
            model.SetFontBold(0);
            model.SetFontSize(10);
            model.SetTable("<1300|<9700;Balance as of|" + AppSettingsManager.MonthsInWords(dDate));
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 10;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
             */

            string sName = string.Empty;
            string sPermitTo = string.Empty;
            string sAddress = string.Empty;
            DateTime dExpDate = DateTime.Now;
            string sData = string.Empty;
            string sOrNo = string.Empty;
            double dOrAmount = 0;
            DateTime dOrDate = DateTime.Now;

            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from spl_permit_data where spl_no = '" + sSPLNo.Trim() +"'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sName = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(result.GetString("bin")));
                    sAddress = AppSettingsManager.GetBnsAddress(result.GetString("bin"));
                    sPermitTo = result.GetString("spl_note").Trim();
                    sOrNo = result.GetString("or_no").Trim();
                    try
                    {
                        dOrAmount = result.GetDouble("amount");
                    }
                    catch
                    {
                        dOrAmount = 0;
                    }
                    dExpDate = result.GetDateTime("exp_date");
                    dOrDate = result.GetDateTime("or_date");
                }
                else
                {
                    result.Close();
                    MessageBox.Show("No Record Found");
                    return;
                }
            }
            result.Close();
            model.SetTable("");
            model.SetTable("");
            model.SetTable(">10000;SP No.:" + sSPLNo.Trim());
            model.SetTable("");
            model.SetTable("");
            model.SetFontBold(1);
            model.SetFontSize(14);
            model.SetTable("^11000;SPECIAL PERMIT;");
            model.SetFontBold(0);
            model.SetFontSize(10);
            model.SetTable("");
            model.SetTable("");
            sData = "         This is to certify that " + sName + ", has been granted this special permit to " ;
            sData += sPermitTo + " at " + sAddress + ", pursuant to Local Tax Ordinance No. 2, series of 1993, and after ";
            sData += "payment of taxes, fees and other charges subject to compliance of such other pertinent national and local ";
            sData += "laws, ordinances and related administrative regulations, and to the following specific terms and conditions:";
            model.SetTable("=10000;" + sData);
            sData = "";
            model.SetTable("=10000;" + sData);
            sData = " |1.|That the permitee shall only be allowed to sell his/her/its stocks and/or conduct business at the designated area as determined by the City Government;";
            model.SetTable("<500|<200|=9300;" + sData);

            sData = "";
            model.SetTable("=10000;" + sData);
            sData = " |2.|That sanitary condition in the area of business operation and proper disposal of waste shall be strictly observed;";
            model.SetTable("<500|<200|=9300;" + sData);

            sData = "";
            model.SetTable("=10000;" + sData);
            sData = " |3.|That business stall/booth/cart shall be erected or positioned in such a way that it does note pose any fire/pollution hazard or obstruction to the flow of traffic of pedestrians and vehicles;";
            model.SetTable("<500|<200|=9300;" + sData);

            sData = "";
            model.SetTable("=10000;" + sData);
            sData = " |4.|That the following fees shall be paid first prior to the issuance of this special permit: Mayor's permit, garbage, sanitary and anti-pollution;";
            model.SetTable("<500|<200|=9300;" + sData);

            sData = "";
            model.SetTable("=10000;" + sData);
            sData = " |5.|That this special permit is not transferable, and;";
            model.SetTable("<500|<200|=9300;" + sData);

            sData = "";
            model.SetTable("=10000;" + sData);
            sData = " |6.|That any violation of the foregoing terms and conditions, and other applicable local/national laws and policies shall mean cancellation of this special permit.;";
            model.SetTable("<500|<200|=9300;" + sData);
            sData = "";
            model.SetFontBold(1);
            model.SetTable("=10000;" + sData);
            //sData = " |This Special permit shall expires on " + dExpDate.ToLongDateString() + ".";
            sData = " |This Special permit shall expire on " + dExpDate.ToLongDateString() + ".";   // RMC 20140107 added logo in special permit
            model.SetTable("<500|=9500;" + sData);
            model.SetFontBold(0);
            sData = "";
            model.SetTable("=10000;" + sData);
            model.SetTable("=10000;" + sData);
            sData = " |Recommending Approval:|Approved:";
            model.SetTable("<500|<6000|<5000;" + sData);
            sData = "";
            model.SetTable("=10000;" + sData);
            model.SetTable("=10000;" + sData);
            model.SetFontBold(1);
            model.SetFontSize(13);
            sData = " |" + AppSettingsManager.GetConfigObject("46") + " | | HON. " + AppSettingsManager.GetConfigObject("36") + "|";
            model.SetTable("<500|^3000|<3000|^3000|<2000;" + sData);
            model.SetFontBold(0);
            model.SetFontSize(10);
            //sData = " |" + AppSettingsManager.GetConfigObject("47") + " | |City Mayor|";
            // RMC 20140107 added logo in special permit (S)
            if(AppSettingsManager.GetConfigValue("01") == "CITY")
                sData = " |" + AppSettingsManager.GetConfigObject("47") + " | |CITY MAYOR|";    
            else
                sData = " |" + AppSettingsManager.GetConfigObject("47") + " | |MUNICIPAL MAYOR|";
            // RMC 20140107 added logo in special permit (E)
            model.SetTable("<500|^3000|<3000|^3000|<2000;" + sData);
            sData = "";
            model.SetTable("=10000;" + sData);
            model.SetTable("=10000;" + sData);
            model.SetTable("=10000;" + sData);
            model.SetTable("=10000;" + sData);
            sData = "|Paid Under OR No.:|" + sOrNo;
            model.SetTable("<7000|<2000|>1000;" + sData);
            sData = "|Amount Paid:|P " + string.Format("{0:#,##0.#0}", dOrAmount);
            model.SetTable("<7000|<2000|>1000;" + sData);
            sData = "|Date Paid:| " + dOrDate.ToShortDateString();
            model.SetTable("<7000|<2000|>1000;" + sData);
            sData = "";
            model.SetTable("=10000;" + sData);
            model.SetTable("=10000;" + sData);
            model.SetTable("=10000;" + sData);
            model.SetTable("=10000;" + sData);
            //model.SetFontSize(8);

            //sData = "(G/F, City Hall) City of Mati, Davao Oriental";
            //location
            sData = " ";
            model.SetTable("=10000;" + sData);
            sData = "Tel. No. ";
            model.SetTable("=10000;" + sData);
            sData = "E-mail Add: ";
            model.SetTable("=10000;" + sData);

            PreviewDocu();


        }
        public void BusinessRecord(string sBin)
        {
            OracleResultSet pRec = new OracleResultSet();   // RMC 20110914 Modified Business record profile view

            sList.ModuleCode = m_strModule;
            sList.ReturnValueByBin = sBin;

            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                model.SetFontName("Arial Narrow");
                model.SetTable("");
                model.SetTable("");

                //model.SetFontSize(15);    // RMC 20110914 Modified Business record profile view
                //model.SetTable("^11000;REPUBLIC OF THE PHILIPPINES"); // RMC 20110914 Modified Business record profile view
                model.SetFontSize(10);  // RMC 20110914 Modified Business record profile view
                model.SetTable("^11000;Republic of the Philippines");   // RMC 20110914 Modified Business record profile view
                if (iReport == 1)// GDE 20090220 Business Record View
                {

                    //model.SetFontSize(12);    // RMC 20110914 Modified Business record profile view
                    //model.SetTable("");   // RMC 20110914 Modified Business record profile view
                    model.SetFontBold(1);   // RMC 20110914 Modified Business record profile view
                    string sLGUName = ConfigurationAttributes.LGUName;
                    // RMC 20150426 QA corrections (s)
                    string strProvinceName = AppSettingsManager.GetConfigValue("08");
                    if (strProvinceName != string.Empty)
                        model.SetTable("^11000;PROVINCE OF " + strProvinceName);
                    // RMC 20150426 QA corrections (e)
                    model.SetTable("^11000;" + sLGUName);
                    model.SetFontBold(0);   // RMC 20110914 Modified Business record profile view
                    model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("41"));    // RMC 20110914 Modified Business record profile view
                    model.SetTable("");

                    if (m_strModule == "NEW-APP-VIEW")   // RMC 20110311
                        model.SetTable("^11000;Business Information - Application: NEW");
                    else if (m_strModule == "REN-APP-VIEW")
                        model.SetTable("^11000;Business Information - Application: RENEWAL");
                    else
                        model.SetTable("^11000;Business Information - Business Record");
                    model.SetTable("");
                    model.SetFontUnderline(1);

                    // business information (s){
                    model.SetFontBold(1);
                    //model.SetFontName("Times New Roman");
                    model.SetFontName("Arial");
                    model.SetFontSize(12); //MCR 20141015 (10)
                    model.SetTable("^11000;BUSINESS INFORMATION");  // RMC 20110914 deleted '-='
                    model.SetFontBold(0);
                    model.SetFontUnderline(0);
                    model.SetFontSize(10);
                    string sTaxYear = sList.BPLSAppSettings[i].sTaxYear;
                    string sPermitNo = sList.BPLSAppSettings[i].sPermitNo;
                    string stelno = sList.BPLSAppSettings[i].sBnsTelNo;
                    //string sBnsNm = StringUtilities.StringUtilities.RemoveApostrophe(sList.BPLSAppSettings[i].sBnsNm);
                    string sBnsNm = this.StringCutter(StringUtilities.StringUtilities.RemoveApostrophe(sList.BPLSAppSettings[i].sBnsNm), 70); // CJC 20131016
                    string sOrgKind = sList.BPLSAppSettings[i].sOrgnKind;
                    string sMainBns = sList.BPLSAppSettings[i].sBnsCode;                    
                    //string sBnsAdd = AppSettingsManager.GetBnsAdd(sBin);    // RMC 20110311
                    string sBnsAdd = this.StringCutter(AppSettingsManager.GetBnsAdd(sBin, ""), 70); // CJC 20131016

                    // RMC 20110914 Modified Business record profile view (s)
                    string sDateOperated = sList.BPLSAppSettings[i].sDTOperated;    
                    string sDTIRegNo = sList.BPLSAppSettings[i].sDTIRegNo;
                    string sDTIRegDt = sList.BPLSAppSettings[i].sDTIRegDate;

                    if (sDTIRegNo.Trim() == "" || sDTIRegDt == AppSettingsManager.GetSystemDate().ToShortDateString())
                        sDTIRegDt = "";

                    string sBussPlate = "";
                    string sPEZARegNo = "";
                    string sPEZARegDt = "";
                    pRec.Query = "select * from buss_plate where bin = '" + sBin + "'";
                    if(pRec.Execute())
                    {
                        if(pRec.Read())
                        {
                            sBussPlate = pRec.GetString("bns_plate");
                        }
                    }
                    pRec.Close();

                    pRec.Query = "select * from boi_table where bin = '" + sBin + "' and exempt_type = 'PEZA'";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sPEZARegNo = pRec.GetString("reg_no");
                            try
                            {
                                sPEZARegDt = pRec.GetDateTime("reg_dt").ToShortDateString();
                            }
                            catch
                            {
                                sPEZARegDt = "";
                            }

                        }
                        
                    }
                    pRec.Close();
                    // RMC 20110914 Modified Business record profile view (e)

                    sList.MainBns = sMainBns;
                    sMainBns = sList.m_sMainBns;
                    sMainBns = this.StringCutter(sList.m_sMainBns, 40); // CJC 20131016
                    model.SetTable("");
                    model.SetTable("");

                    //model.SetTable(string.Format("<2000|<3000|<1500|<6500;BIN:|{0}|Tax Year|{1}", sBin, sTaxYear));
                    model.SetTable(string.Format("<2000|<3000|<1500|<1500|<1500|<2500;BIN:|{0}|Tax Year:|{1}|Date Operated:|{2}", sBin, sTaxYear, sDateOperated)); // RMC 20110914 Modified Business record profile view
                    model.SetTable(string.Format("<2000|<3000|<1500|<1500|<1000|<3000;Permit No.:|{0}|Permit Date:|{1}|Tel. No.:|{2}", sPermitNo, sTaxYear, "")); // from stelno to "" //MCR 20141222
                    //MCR 20141222 (s)
                    long lng1 = model.GetCurrentY();
                    model.SetCurrentY(lng1 + 500);
                    stelno = stelno.Replace('/', '\n');
                    model.SetTable(string.Format("<9000|<1000; |{0}", stelno));
                    model.SetCurrentY(lng1 + 700);
                    //MCR 20141222 (e)
                    model.SetTable(string.Format("<2000|<30000;Business Name:|{0}", sBnsNm));
                    model.SetTable(string.Format("<2000|<30000;Business Address:|{0}", sBnsAdd));   // RMC 20110311
                    //model.SetTable(string.Format("<2000|<3000|<1500|<1500|<1000|<3000;Orgn. Kind:|{0}|||Main Bns:|{1}", sOrgKind, sMainBns));
                    //model.SetTable(string.Format("<2000|<3000|<1000|<5000;Orgn. Kind:|{0}|Main Bns:|{1}", sOrgKind, sMainBns));   // RMC 20110816
                    model.SetTable(string.Format("<2000|<3000|<2000|<5000;Orgn. Kind:|{0}|Business Plate:|{1}", sOrgKind, sBussPlate));     // RMC 20110914 Modified Business record profile view

                    // RMC 20110809 transferred to business info (s)
                    string sCap = string.Format("{0:#,###.##}", Convert.ToDouble(sList.BPLSAppSettings[i].sCapital));
                    string sGross = string.Format("{0:#,###.##}", Convert.ToDouble(sList.BPLSAppSettings[i].sGr1));
                    string sBnsStat = sList.BPLSAppSettings[i].sBnsStat;
                    //model.SetTable(string.Format("<2000|<3000|<1500|<1500|<2000|<2000;Business Status:|{0}|||Inital Capital:|{1}", sBnsStat, sCap));
                    //model.SetTable(string.Format("<2000|<3000|<1500|<1500|<2000|<2000;Operation Start:|{0}|||Annual Gross:|{1}", sTaxYear, sGross));
                    /*model.SetTable(string.Format("<2000|<3000|<2000|<2000;Business Status:|{0}|Inital Capital:|{1}", sBnsStat, sCap));
                    model.SetTable(string.Format("<2000|<3000|<2000|<2000;Operation Start:|{0}|Annual Gross:|{1}", sTaxYear, sGross));*/
                    // RMC 20110809 transferred to business info (e)

                    // RMC 20110914 Modified Business record profile view (s)
                    model.SetParagraph("");
                    model.SetTable(string.Format("<1000|<2000|<2000|<2000|<2000;|DTI Registration No.:|{0}|Registration Date:|{1}", sDTIRegNo, sDTIRegDt));
                    model.SetTable(string.Format("<1000|<2000|<2000|<2000|<2000;|PEZA Registration No.:|{0}|Registration Date:|{1}", sPEZARegNo, sPEZARegDt));
                    // RMC 20110914 Modified Business record profile view (e)

                    // business information (e)}

                    // owner's information (s){
                    model.SetTable("");
                    model.SetTable("");
                    model.SetFontSize(12);
                    model.SetFontUnderline(1);
                    model.SetFontBold(1);
                    model.SetTable("^11000;OWNER'S INFORMATION");   // RMC 20110914 deleted '-='
                    model.SetFontBold(0);
                    model.SetFontUnderline(0);
                    model.SetFontSize(10);
                    model.SetTable("");
                    model.SetTable("");
                    string sOwnCode = sList.BPLSAppSettings[i].sOwnCode;
                    sList.OwnName = sOwnCode;
                    string sOwnLn = sList.OwnNamesSetting[i].sLn;
                    string sOwnFn = sList.OwnNamesSetting[i].sFn;
                    string sOwnMi = sList.OwnNamesSetting[i].sMi;
                    //string sOwnAdd = sList.OwnNamesSetting[i].sOwnHouseNo + " " + sList.OwnNamesSetting[i].sOwnStreet + " " + sList.OwnNamesSetting[i].sOwnBrgy + " " + sList.OwnNamesSetting[i].sOwnMun; // RMC 20110914
                    //string sOwnAdd = FormatAddress(sList.OwnNamesSetting[i].sOwnHouseNo, sList.OwnNamesSetting[i].sOwnStreet, sList.OwnNamesSetting[i].sOwnBrgy, sList.OwnNamesSetting[i].sOwnMun); // RMC 20110914
                    string sOwnAdd = this.StringCutter(FormatAddress(sList.OwnNamesSetting[i].sOwnHouseNo, sList.OwnNamesSetting[i].sOwnStreet, sList.OwnNamesSetting[i].sOwnBrgy, sList.OwnNamesSetting[i].sOwnMun), 70); // CJC 20131016

                    model.SetTable(string.Format("<2000|<9000;Last Name:|{0}", sOwnLn));
                    model.SetTable(string.Format("<2000|<3000|<1500|<1500|<1000|<3000;First Name:|{0}|Middle Initial:|{1}||", sOwnFn, sOwnMi));
                    model.SetTable(string.Format("<2000|<9000;Address:|{0}", sOwnAdd));
                    // owner's information (e)}

                    // location status (s){
                    model.SetTable("");
                    //model.SetTable("");
                    model.SetFontSize(12);
                    model.SetFontUnderline(1);
                    model.SetFontBold(1);
                    model.SetTable("^11000;LOCATION STATUS");   // RMC 20110914 deleted '-='
                    model.SetFontBold(0);
                    model.SetFontUnderline(0);
                    model.SetFontSize(10);
                    model.SetTable("");
                    //model.SetTable("");

                    string sPlaceOwnership = sList.BPLSAppSettings[i].sPlaceOccupancy;
                    //string sPlaceOwnCode = sList.BPLSAppSettings[i].sPrevBnsOwn;  // RMC 20110808 put rem

                    // RMC 20110808 (s)
                    string sPlaceOwnCode = "";

                    if(sPlaceOwnership == "RENTED")
                        sPlaceOwnCode = sList.BPLSAppSettings[i].m_sBUSN_OWN;
                    // RMC 20110808 (e)
                    
                    // gde 20101102
                    if (sPlaceOwnership == string.Empty)
                        sPlaceOwnership = "OWNED";

                    if (sPlaceOwnCode == string.Empty)
                        sPlaceOwnCode = sList.BPLSAppSettings[i].m_sOWN_CODE;
                        
                    // gde 20101102
                    sList.OwnName = sPlaceOwnCode;
                    string sPrevOwnLn = sList.OwnNamesSetting[i].sLn;
                    string sPrevOwnFn = sList.OwnNamesSetting[i].sFn;
                    string sPrevOwnMi = sList.OwnNamesSetting[i].sMi;
                    //string sPrevOwnAdd = sList.OwnNamesSetting[i].sOwnHouseNo + " " + sList.OwnNamesSetting[i].sOwnStreet + " " + sList.OwnNamesSetting[i].sOwnBrgy + " " + sList.OwnNamesSetting[i].sOwnMun; // RMC 20110915

                    //string sPrevOwnAdd = FormatAddress(sList.OwnNamesSetting[i].sOwnHouseNo, sList.OwnNamesSetting[i].sOwnStreet, sList.OwnNamesSetting[i].sOwnBrgy, sList.OwnNamesSetting[i].sOwnMun); // RMC 20110915
                    string sPrevOwnAdd = this.StringCutter(FormatAddress(sList.OwnNamesSetting[i].sOwnHouseNo, sList.OwnNamesSetting[i].sOwnStreet, sList.OwnNamesSetting[i].sOwnBrgy, sList.OwnNamesSetting[i].sOwnMun), 70); // CJC 20131016

                    string sMontlyRent = string.Format("{0:#,###.##}", Convert.ToDouble(sList.BPLSAppSettings[i].sRentLeaseMo));    // RMC 20110914
                    string sNoEmp = sList.BPLSAppSettings[i].sNumEmployees;
                    string sNumProf = sList.BPLSAppSettings[i].sNumProfessional;
                    string sPayroll = sList.BPLSAppSettings[i].sAnnualWages;
                    string sDelv = sList.BPLSAppSettings[i].sNumDelivVehicle;
                    string sStorey = sList.BPLSAppSettings[i].sNumStorey;
                    string sTotArea = sList.BPLSAppSettings[i].sTotFlrArea;
                    string sNumMach = sList.BPLSAppSettings[i].sNumMachineries;
                    string sGroundFlr = sList.BPLSAppSettings[i].sFlrArea;
                    string sBldgVal = sList.BPLSAppSettings[i].sBldgVal;
                    model.SetTable(string.Format("<3000|<2000|<1500|<1500|<2000|<2000;Business Place Ownership:|{0}|Since When:|{1}|Monthly Rental:|{2}", sPlaceOwnership, sTaxYear, sMontlyRent));    // RMC 20110914 adjusted
                    model.SetTable(string.Format("<2000|<9000;Last Name:|{0}", sPrevOwnLn));
                    model.SetTable(string.Format("<2000|<3000|<1500|<1500|<1000|<3000;First Name:|{0}|Middle Initial:|{1}||", sPrevOwnFn, sPrevOwnMi));
                    model.SetTable(string.Format("<2000|<9000;Address:|{0}", sPrevOwnAdd));
                    /*model.SetTable(string.Format("<2500|<2000|<1800|<1500|<2000|<2000;No. of Employees:|{0}|No. of Professionals:|{1}|Annual Payroll:|{2}", sNoEmp, sNumProf, sPayroll));   // RMC 20110809 adjusted format
                    model.SetTable(string.Format("<2500|<2000|<1800|<1500|<2000|<2000;No. of Delivery Vehicle:|{0}|No. of Storey:|{1}|Total Floor Area:|{2}", sDelv, sStorey, sTotArea));   // RMC 20110809 adjusted format
                    model.SetTable(string.Format("<2500|<2000|<1800|<1500|<2000|<2000;No. of Machineries:|{0}|Ground Floor Area:|{1}|Building Value:|{2}", sNumMach, sGroundFlr, sBldgVal));    // RMC 20110809 adjusted format
                    */  // RMC 20110914 Modified Business record profile view 
                    
                    // location status (e)}

                    // prev owner info (s){
                    model.SetTable("");
                    //model.SetTable("");
                    model.SetFontSize(12);
                    model.SetFontUnderline(1);
                    model.SetFontBold(1);
                    model.SetTable("^11000;PREVIOUS OWNER INFORMATION");   // RMC 20110914 deleted '-='
                    model.SetFontBold(0);
                    model.SetFontUnderline(0);
                    model.SetFontSize(10);
                    model.SetTable("");
                    //model.SetTable("");
                    string sPlaceOwnCode2 = sList.BPLSAppSettings[i].sPrevBnsOwn; // for verification (owner code)

                    // gde 20101102
                    if (sPlaceOwnCode2 == string.Empty)
                        sPlaceOwnCode2 = sList.BPLSAppSettings[i].m_sOWN_CODE;
                    // gde 20101102
                    sList.OwnName = sPlaceOwnCode2;
                    string sPrevOwnLn2 = sList.OwnNamesSetting[i].sLn;
                    string sPrevOwnFn2 = sList.OwnNamesSetting[i].sFn;
                    string sPrevOwnMi2 = sList.OwnNamesSetting[i].sMi;
                    //string sPrevOwnAdd2 = sList.OwnNamesSetting[i].sOwnHouseNo + " " + sList.OwnNamesSetting[i].sOwnStreet + " " + sList.OwnNamesSetting[i].sOwnBrgy + " " + sList.OwnNamesSetting[i].sOwnMun;    // RMC 20110914
                    
                    //string sPrevOwnAdd2 = FormatAddress(sList.OwnNamesSetting[i].sOwnHouseNo, sList.OwnNamesSetting[i].sOwnStreet, sList.OwnNamesSetting[i].sOwnBrgy, sList.OwnNamesSetting[i].sOwnMun);    // RMC 20110914
                    string sPrevOwnAdd2 = this.StringCutter(FormatAddress(sList.OwnNamesSetting[i].sOwnHouseNo, sList.OwnNamesSetting[i].sOwnStreet, sList.OwnNamesSetting[i].sOwnBrgy, sList.OwnNamesSetting[i].sOwnMun), 70); // CJC 20131016

                    /*string sBnsStat = sList.BPLSAppSettings[i].sBnsStat;
                    string sCap = sList.BPLSAppSettings[i].sCapital;
                    string sGross = sList.BPLSAppSettings[i].sGr1;*/    // RMC 20110809 transferred to business info
                    model.SetTable(string.Format("<2000|<9000;Last Name:|{0}", sPrevOwnLn2));
                    model.SetTable(string.Format("<2000|<3000|<1500|<1500|<1000|<3000;First Name:|{0}|Middle Initial:|{1}||", sPrevOwnFn2, sPrevOwnMi2));
                    model.SetTable(string.Format("<2000|<9000;Address:|{0}", sPrevOwnAdd2));
                    /*model.SetTable(string.Format("<2000|<3000|<1500|<1500|<2000|<2000;Business Status:|{0}|||Inital Capital:|{1}", sBnsStat, sCap));
                    model.SetTable(string.Format("<2000|<3000|<1500|<1500|<2000|<2000;Operation Start:|{0}|||Annual Gross:|{1}", sTaxYear, sGross));*/  // RMC 20110809 transferred to business info
                    // prev owner info (e)}

                    // RMC 20110914 Modified Business record profile view (S)
                    int iCnt = 0;
                    pRec.Query = "select count(*) from other_info where bin = '" + sBin + "' and bns_code in ";
                    pRec.Query += "(select bns_code from businesses where bin = '" + sBin + "')";
                    pRec.Query += " and tax_year = '" + sTaxYear + "'"; // RMC 20120117 corrected display of other_info details in Business Profile
                    pRec.Query += " and data <> 0"; // RMC 20140112 corrected display of other info in record view
                    int.TryParse(pRec.ExecuteScalar(), out iCnt);

                    if (iCnt > 0)
                    {
                        model.SetTable("");
                        model.SetFontSize(12);
                        model.SetFontUnderline(1);
                        model.SetFontBold(1);
                        model.SetTable("^11000;ADDITIONAL INFORMATION");
                        model.SetFontBold(0);
                        model.SetFontUnderline(0);
                        model.SetFontSize(10);
                        model.SetTable("");

                        //model.SetTable(string.Format("^4000|^1000;Description|Value"));
                        model.SetTable(string.Format("^8000|^1000;Description|Value")); // CJC 20131016

                        string sDefaultCode = "";
                        string sDefaultDesc = "";
                        string sValue = "";
                        pRec.Query = "select * from other_info where bin = '" + sBin + "' and bns_code in ";
                        pRec.Query += "(select bns_code from businesses where bin = '" + sBin + "')";
                        pRec.Query += " and tax_year = '" + sTaxYear + "'"; // RMC 20120117 corrected display of other_info details in Business Profile
                        pRec.Query += " and data <> 0"; // RMC 20140112 corrected display of other info in record view
                        if (pRec.Execute())
                        {
                            while (pRec.Read())
                            {
                                sDefaultCode = pRec.GetString("default_code");
                                sValue = string.Format("{0:###.00}", pRec.GetDouble("data"));

                                sDefaultDesc = GetDefaultDesc(sDefaultCode);

                                //model.SetTable(string.Format("<4000|>1000;{0}|{1}", sDefaultDesc, sValue));
                                model.SetTable(string.Format("<8000|>1000;{0}|{1}", sDefaultDesc, sValue)); // CJC 20131016
                            }
                        }
                        pRec.Close();
                    }
                    // RMC 20110914 Modified Business record profile view (E)

                    // insurance info (s){
                    model.SetTable("");
                    //model.SetTable("");
                    model.SetFontSize(12);
                    model.SetFontUnderline(1);
                    model.SetFontBold(1);
                    //model.SetTable("^11000;-= ADDITIONAL LINE OF BUSINESS =-"); // RMC 20110809 Added viewing of addl line of business in Buss-View
                    //model.SetTable("^11000;-= OTHER LINE OF BUSINESS =-");  // RMC 20110816
                    model.SetTable("^11000;LINE/S OF BUSINESS");  // RMC 20110914 Modified Business record profile view 
                    model.SetFontBold(0);
                    model.SetFontUnderline(0);
                    model.SetFontSize(10);
                    model.SetTable("");
                    //model.SetTable("");
                    // insurance info (e)}

                    // RMC 20110914 Modified Business record profile view (s)
                    model.SetTable(string.Format("^5000|^1000|^1500|^1500;Line of Business|Status|Capital|Gross"));
                    model.SetTable(string.Format("<5000|^1000|>1500|>1500;(Main) {0}|{1}|{2}|{3}", sMainBns,sBnsStat,sCap,sGross));
                    // RMC 20110914 Modified Business record profile view (e)

                    // RMC 20170109 enable adding of new buss in renewal application (s)
                    if (m_strModule == "NEW-APP-VIEW" || m_strModule == "REN-APP-VIEW")
                    {
                        result.Query = "select * from addl_bns_que where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "'";
                    }// RMC 20170109 enable adding of new buss in renewal application (e)
                    else
                    {
                        // RMC 20110809 Added viewing of addl line of business in Buss-View (s)
                        result.Query = "select * from addl_bns where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "'";
                    }
                    if (result.Execute())
                    {
                        string sAddlBnsCode = "";
                        string sAddlBnsStat = "";
                        string sAddlGross = "";
                        string sAddlCapital = "";
                        
                        while (result.Read())
                        {
                            sAddlBnsCode = result.GetString("bns_code_main").Trim();
                            sAddlBnsCode = AppSettingsManager.GetBnsDesc(sAddlBnsCode);
                            sAddlBnsStat = result.GetString("bns_stat").Trim();
                            sAddlGross = string.Format("{0:#,###.##}", result.GetDouble("gross"));
                            sAddlCapital = string.Format("{0:#,###.##}", result.GetDouble("capital"));

                            /*model.SetTable(string.Format("<2000|<4000|<1000|<1000|<1000;Business:|{0}||Status:|{1}", sAddlBnsCode, sAddlBnsStat));
                            model.SetTable(string.Format("<2000|<4000|<1000|<1000|<1000;Capital:|{0}||Gross:|{1}", sAddlCapital, sAddlGross));*/
                            model.SetTable(string.Format("<5000|^1000|>1500|>1500;{0}|{1}|{2}|{3}", sAddlBnsCode, sAddlBnsStat, sAddlCapital, sAddlGross)); // RMC 20110914 Modified Business record profile view
                        }
                    }
                    result.Close();
                    // RMC 20110809 Added viewing of addl line of business in Buss-View (e)
                }
                model.SetTable(string.Empty);

                model.SetTable(string.Format("<2000|=8000;Memoranda:|{0}", sList.BPLSAppSettings[i].m_sMEMORANDA));    // RMC 20171127 display memoranda in Business Record view report

                model.SetTable(string.Empty);

                model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
                model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));
            }
        }

        public void PreviewDocu()
        {
            VSPrinterEmuDocument doc = new VSPrinterEmuDocument();
            doc.Model = model;

            doc.PageHeaderModel = pageHeaderModel;

            doc.Model.Reset();
            doc.DefaultPageSettings.Landscape = false;
            /*doc.DefaultPageSettings.Margins.Top = 50;
            doc.DefaultPageSettings.Margins.Left = 50;
            doc.DefaultPageSettings.Margins.Bottom = 50;
            doc.DefaultPageSettings.Margins.Right = 50;*/
            doc.DefaultPageSettings.Margins.Top = 500;
            doc.DefaultPageSettings.Margins.Left = 200;
            doc.DefaultPageSettings.Margins.Bottom = 50;
            doc.DefaultPageSettings.Margins.Right = 200;
            doc.SetImagePath(sImagepath, false);
            
            doc.DefaultPageSettings.PaperSize = new PaperSize("", 850, 1100);
            doc.PrintPage += new PrintPageEventHandler(_Doc_PrintPage);
     
            

            frmMyPrintPreviewDialog dlgPreview = new frmMyPrintPreviewDialog();
            dlgPreview.Document = doc;
            dlgPreview.ClientSize = new System.Drawing.Size(640, 480);
            dlgPreview.WindowState = FormWindowState.Maximized;
            dlgPreview.ShowDialog();
            model.Dispose();
        }

        private string GetDefaultDesc(string sCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sDefaultDesc = "";

            pSet.Query = string.Format("select * from default_code where default_code = '{0}' and rev_year = '{1}'", sCode, ConfigurationAttributes.RevYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sDefaultDesc = pSet.GetString("default_desc").Trim();
                }
            }
            pSet.Close();

            return sDefaultDesc;
        }

        private string FormatAddress(string sAddNo, string sStreet, string sBrgy, string sMun)
        {
            string sAddress = "";

            if (sAddNo.Trim() != "" && sAddNo.Trim() != ".")
                sAddress = sAddNo.Trim() + " ";
            if (sStreet.Trim() != "" && sStreet.Trim() != ".")
                sAddress += sStreet.Trim() + " ";
            if (sBrgy.Trim() != "" && sBrgy.Trim() != ".")
                sAddress += sBrgy.Trim() + " ";
            if (sMun.Trim() != "" && sMun.Trim() != ".")
                sAddress += sMun.Trim();

            return sAddress;
        }

        public void PrintLessor()
        {
            // RMC 20111007 Added report for List of Lessors
            OracleResultSet pSet = new OracleResultSet();
            
            string sRecCnt = "0";
            string sOwnCode = "";
            string sOwnLn = "";
            string sOwnFn = "";
            string sOwnMi = "";
            string sOwnAdd = "";
            string sLessorName = "";
            string sRemarks = "";
            string strProvinceName = "";
            string sRowCnt = "";
            int iRowCnt = 0;
            int iTotRecCnt = 0;
            
            

            CreatePageHeader(); 

            model.SetFontName("Arial");
            model.SetFontSize(8);
            model.SetFontBold(0);
            model.SetTable("^11000;Republic of the Philippines");
            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                model.SetTable("^11000;" + strProvinceName);
            model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("09"));
            model.SetTable("");
            model.SetFontSize(10);
            model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("41"));
            model.SetFontBold(1);
            model.SetTable("^11000;List of Lessors");
            model.SetFontBold(0);
            model.SetFontSize(8);
            model.SetTable("");
            model.SetTable("");

            model.SetTable(string.Format("^7000|^1500|^1000;Lessor's Name/ Address|No. of Lessee|Remarks"));
            model.SetTable("");

            
            pSet.Query = "select * from own_names where own_code in";
            pSet.Query += " (select busn_own from businesses where PLACE_OCCUPANCY = 'RENTED')";
            pSet.Query += " order by own_ln, own_fn";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iRowCnt++;
                    
                    sOwnCode = pSet.GetString("own_code").Trim();
                    sOwnLn = pSet.GetString("Own_ln").Trim();
                    sOwnFn = pSet.GetString("Own_fn").Trim();
                    sOwnMi = pSet.GetString("own_mi").Trim();

                    sLessorName = sOwnLn;
                    if (sOwnFn != "")
                        sLessorName += ", " + sOwnFn;
                    if (sOwnMi != "")
                        sLessorName += " " + sOwnMi;

                    sOwnAdd = AppSettingsManager.GetBnsOwnAdd(sOwnCode);

                    sRecCnt = string.Format("{0:###}", GetBnsCount(sOwnCode));

                    if (ValidateIfRegisteredLessor(sOwnCode))
                        sRemarks = "Registered lessor";
                    else
                        sRemarks = "Not registered lessor";

                    sRowCnt = string.Format("{0:###}", iRowCnt);

                    model.SetTable(string.Format("<7000|^1500|<1000;{0}.) {1}|{2}|{3}", sRowCnt,sLessorName, sRecCnt, sRemarks));
                    model.SetTable(string.Format("<7000;{0}", sOwnAdd));
                    model.SetTable("");

                    if (m_bPrintBIN)
                        PrintLesseeInfo(sOwnCode);
                }
            }
            pSet.Close();

            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));

            pageHeaderModel.PageCount = model.PageCount;
            PreviewDocu();
        }
        private int GetBnsCount(string sOwnCode)
        {
            OracleResultSet pRec = new OracleResultSet();
            int iCnt = 0;

            pRec.Query = string.Format("select count(*) from businesses where busn_own = '{0}' and place_occupancy = 'RENTED'", sOwnCode);
            int.TryParse(pRec.ExecuteScalar(), out iCnt);

            return iCnt;
        }
        private bool ValidateIfRegisteredLessor(string sOwnCode)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBnsCode = "";

            pRec.Query = "select * from bns_table where fees_code = 'B'";
            pRec.Query += " and length(bns_code) = 2 and (bns_desc like '%LESSOR COML%'";
            pRec.Query += " or bns_desc like '%COM% LESSOR%')";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sBnsCode = pRec.GetString("bns_code").Trim();
                }
            }
            pRec.Close();

            pRec.Query = string.Format("select * from businesses where own_code = '{0}'", sOwnCode);
            pRec.Query += string.Format(" and substr(bns_code, 1,2) = '{0}'", sBnsCode);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    pRec.Close();
                    return true;
                }
                else
                {
                    pRec.Close();
                    return false;
                }
            }
            pRec.Close();

            return true;

        }

        private void CreatePageHeader()
        {

            pageHeaderModel.Clear();
            model.SetFontName("Arial");
            string strProvinceName = string.Empty;
           // pageHeaderModel.SetTextAlign(1);
            pageHeaderModel.SetFontBold(0);
            pageHeaderModel.SetFontSize(8);
            pageHeaderModel.SetTable("^11000; Republic of the Philippines");

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                pageHeaderModel.SetTable("^11000;" + strProvinceName);
            pageHeaderModel.SetTable("^11000;" + AppSettingsManager.GetConfigValue("09"));

            pageHeaderModel.SetTable("");
            pageHeaderModel.SetFontSize(10);
            pageHeaderModel.SetTable("^11000;" + AppSettingsManager.GetConfigValue("41"));
            pageHeaderModel.SetFontBold(1);
            pageHeaderModel.SetTable("^11000;List of Lessors");
            pageHeaderModel.SetFontBold(0);
            pageHeaderModel.SetFontSize(8);
            pageHeaderModel.SetParagraph(string.Empty);
            pageHeaderModel.SetParagraph(string.Empty);

            pageHeaderModel.SetTable(string.Format("^7000|^1500|^1000;Lessor's Name/ Address|No. of Lessee|Remarks"));
            pageHeaderModel.SetTable("");
            

        }
        private void PrintLesseeInfo(string sOwnCode)
        {
            // RMC 20111128 added printing of lessee info in List of Lessors report
            OracleResultSet pRec = new OracleResultSet();
            string sBIN = "";
            string sBnsName = "";

            sBIN = "|Lessee Info:|";
            model.SetTable(string.Format("<3000|<2000|<5000;{0}", sBIN));

            pRec.Query = string.Format("select * from businesses where busn_own = '{0}' and place_occupancy = 'RENTED' order by Bin", sOwnCode);
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBIN = pRec.GetString("bin");
                    sBnsName = pRec.GetString("bns_nm");

                    model.SetTable(string.Format("<3000|<2000|<5000;|{0}|{1}", sBIN, sBnsName));
                }
            }
            pRec.Close();


        }

        // CJC 20131016 (s)
        private string StringCutter(string strInput, int intLength)
        {
            string strOutput = strInput;
            
            if (strOutput.Length > intLength)
            {
                int intWrap = intLength;
                int intWrapMult = 0;
                intWrapMult = strOutput.Length / intWrap;
                //intWrap = intWrap * intWrapMult;

                for (intWrap = intLength * intWrapMult; intWrapMult >= 1; --intWrapMult, intWrap -= intLength)
                {
                    //   strLine2 = strLine2.Insert(intWrap, "\n");
                    if (strOutput.Substring(0, intWrap).LastIndexOf(" ") != -1)
                    {
                        strOutput = strOutput.Insert(strOutput.Substring(0, intWrap).LastIndexOf(" "), "\n");
                    }
                    else if (strOutput.Substring(0, intWrap).LastIndexOf("/") != -1)
                    {
                        strOutput = strOutput.Insert(strOutput.Substring(0, intWrap).LastIndexOf("/"), "\n");
                    }
                    else
                    {
                        strOutput = strOutput.Insert(strOutput.Substring(0, intWrap).LastIndexOf("-"), "\n");
                    }
                }

            }
            return strOutput;
        }
        // CJC 20131016 (e)

        private string GetNoOfQtr(string p_sOrNo)
        {
            // RMC 20130109 display no. of qtr in payment hist
            OracleResultSet pSet = new OracleResultSet();
            string sNoOfQtr = string.Empty;
            string sQtr = string.Empty;

            pSet.Query = "select distinct * from pay_hist where or_no = '" + p_sOrNo + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sNoOfQtr = pSet.GetString("no_of_qtr").Trim();
                    sQtr = pSet.GetString("qtr_paid").Trim();

                    if (sQtr == "F")
                        sNoOfQtr = "4";
                    else
                    {
                        if (sNoOfQtr == "")
                            sNoOfQtr = "1";
                    }
                }
            }
            pSet.Close();

            return sNoOfQtr;
        }
        
       // private void GetCapGross(string sBin, string sTaxYear, string sBnsCode)
       private void GetCapGross(string sBin, string sTaxYear, string sBnsCode, string sOrNo, string sBnsStat) //JHB 20181203 merge from LAL-LO REV adj
        {
            // RMC 20140122 view capital, gross, pre-gross on payment hist
            m_sCap = "";   
            m_sGross = "";
            m_sPreGross = "";
            OracleResultSet result = new OracleResultSet();
            //OracleResultSet result2 = new OracleResultSet();

            //JHB 20181203 merger from LAL-LO JARS 20181129(S) ATTEMPT TO ONLY DEPEND IN OR_TABLE, ELSE BILL_GROSS_INFO
            result.Query = "select coalesce(declared_gross_cap, 0) as gross from or_table where or_no = '" + sOrNo + "' ";
            result.Query += "and tax_year = '" + sTaxYear + "' and bns_code_main = '" + sBnsCode + "' and fees_code = 'B'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    if (sBnsStat == "REN" || sBnsStat == "RET")
                    {
                        m_sGross = result.GetDouble("gross").ToString("#,##0.00");
                    }
                    else //NEW
                    {
                        m_sCap = result.GetDouble("gross").ToString("#,##0.00"); ;
                    }
                }
                else
                {
                    result.Close();
                    result.Query = "select * from bill_gross_info where bin = '" + sBin + "' ";
                    result.Query += " and tax_year = '" + sTaxYear + "' and bns_code = '" + sBnsCode + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            m_sCap = string.Format("{0:#,###.##}", result.GetInt("capital"));
                            m_sGross = string.Format("{0:#,###.##}", result.GetInt("gross"));
                            m_sPreGross = string.Format("{0:#,###.##}", result.GetInt("pre_gross"));
                            m_sAdjGross = result.GetInt("adj_gross").ToString("#,##0.00"); //JARS 20181004
                        }
                    }
                    result.Close();
                }
            }
            result.Close();
            //JARS 20181129(E)

          #region hide
            //JARS 20170717 (S) COMMENT OUT, SINCE THAT THERE IS NO DATA YET IN BILL GROSS INFO
            //result.Query = "select * from bill_gross_info where bin = '" + sBin + "' ";
            //result.Query += " and tax_year = '" + sTaxYear + "' and bns_code = '" + sBnsCode + "'";
            //if (result.Execute())
            //{
            //    if (result.Read())
            //    {
            //        m_sCap = string.Format("{0:#,###.##}",result.GetInt("capital"));
            //        m_sGross = string.Format("{0:#,###.##}",result.GetInt("gross"));
            //        m_sPreGross = string.Format("{0:#,###.##}",result.GetInt("pre_gross"));
                    
            //    }
            //}
            //result.Close();
        
            //result.Query = "select * from businesses where bin = '"+ sBin +"' and bns_code = '"+ sBnsCode +"'"; //ALTERNATE DATA SOURCE
            //result.Query = "select * from buss_hist where bin = '"+ sBin +"' and bns_code = '"+ sBnsCode +"' and tax_year = '"+ sTaxYear +"'"; //JARS 20170809 TO GET GROSS AND CAPITAL OF SPECIFIC TAX YEAR
            //if (result.Execute())
            //{
            //    if (result.Read())
            //    {
            //        m_sCap = string.Format("{0:#,###.00}", result.GetInt("capital"));
            //        m_sGross = string.Format("{0:#,###.00}", result.GetInt("gr_1"));
            //        //m_sPreGross = "";
            //    }
            //    else
            //    {
            //        result.Close();
            //        result.Query = "select * from addl_bns where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and bns_code_main  = '" + sBnsCode + "'";
            //        if (result.Execute())
            //        {
            //            if (result.Read())
            //            {
            //                m_sCap = string.Format("{0:#,###.00}", result.GetInt("capital"));
            //                m_sGross = string.Format("{0:#,###.00}", result.GetInt("gross"));
            //            }
            //            else //JARS 20170815
            //            {
            //                result.Close();
            //                result.Query = "select * from declared_gross where bin = :1 and bns_code = :2 and tax_year = :3";
            //                result.AddParameter(":1", sBin.Trim());
            //                result.AddParameter(":2", sBnsCode.Trim());
            //                result.AddParameter(":3", sTaxYear.Trim());
            //                if (result.Execute())
            //                {
            //                    if (result.Read())
            //                    {
            //                        m_sGross = string.Format("{0:##0.00}", result.GetDouble("declared_gr"));
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //result.Close();
            //JARS 20170717 (E)
          #endregion
        }
        private string GetRetBns(string sBin)
        {
            // RMC 20151005 mods in retirement module
            OracleResultSet result = new OracleResultSet();
            string sBnsCode = string.Empty;
            string sBnsDesc = string.Empty;

            result.Query = "select * from retired_bns where bin = '" + sBin + "'";
            result.Query += " and tax_year = '" + sFPTaxYear + "'";
            if (result.Execute())
            {
                int ictr = 0;
                while (result.Read())
                {
                    ictr++;
                    if (ictr > 1)
                        sBnsDesc += " and ";

                    sBnsCode = result.GetString("bns_code_main");
                    sBnsDesc += AppSettingsManager.GetBnsDesc(sBnsCode);
                }
            }
            result.Close();

            return sBnsDesc;

        }

        public void PayHist2(string sBin)
        {
            string sDate = string.Empty;
            string sOrNo = string.Empty;
            string sTxYr = string.Empty;
            string sCapital = string.Empty;
            string sGross = string.Empty;
            string sTerm = string.Empty;
            string sQtr = string.Empty;
            string sTax = string.Empty;
            string sFees = string.Empty;
            string sSurPen = string.Empty;
            string sTotal = string.Empty;

            string sNoOfQtr = string.Empty;


            long lngY1 = 0;
            long lngY2 = 0;
            model.Clear();
            sImagepath = "C:\\Documents and Settings\\Godfrey\\My Documents\\Downloads\\logo_header.jpg";
            //sImagepath = "D:\\Rachel\\Projects\\Mati\\SourceCodes_fr_Godfrey\\_Latest 20131219\\aBPLs-e\\BPS\\BPS\\Resources\\logo-mati.png";
            model.SetFontName("Arial");
            model.SetFontSize(10);
            model.SetTable("^11000;Republic of the Philippines");
            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                //model.SetTable("^11000;CITY OF" + AppSettingsManager.GetConfigValue("02"));
                model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("02"));
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                model.SetTable("^11000;MUNICIPALITY OF" + AppSettingsManager.GetConfigValue("02"));
            model.SetTable("");
            // RMC 20170130 added signatory in Payment History of BTAS (s)
            if (AppSettingsManager.GetSystemType == "C")
            {
                if (AppSettingsManager.GetConfigValue("01") == "CITY")
                    model.SetTable("^11000;OFFICE OF THE CITY TREASURER");
                else
                    model.SetTable("^11000;OFFICE OF THE MUNICIPAL TREASURER");
            }
            else
                model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("41"));

            model.SetTable("");
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetTable("<11000;Payment History");
            model.SetFontBold(0);
            model.SetFontSize(8);
            model.SetTable("<1700|<8800;Available Record as of|" + AppSettingsManager.MonthsInWords(dDate));
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 9;
            model.SetFontSize(15);
            model.SetFontBold(1);
            model.SetTable("");
            model.SetTable("<11000;" + sBin);
            model.SetFontBold(0);
            model.SetFontSize(8);
            model.SetTable("<11000;Business Identification Number");
            model.SetTable("");
            model.SetTable("");
            lngY1 = model.GetCurrentY();
            model.DrawLine(700, lngY1, 11500, lngY1);
            model.SetTable("");
            model.SetTable("<1700|<9300;   Business Name|: " + AppSettingsManager.GetBnsName(sBin.Trim())); ;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            model.SetTable("<1700|<9300;   Address|: " + AppSettingsManager.GetBnsAdd(sBin.Trim(), ""));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            model.SetTable("<1700|<9300;   Owner's Name|: " + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin.Trim())));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            model.SetTable("<1700|<9300;   Address|: " + AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetOwnCode(sBin.Trim())));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;

            //MCR 20150129 (s)
            model.SetTable("<1700|<9300;   Business Plate|: " + AppSettingsManager.GetBnsPlate(sBin.Trim()));
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontName = "Arial Narrow";
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[1].Font.FontStyle = 0;
            //MCR 20150129 (e)

            model.SetTable("");
            lngY2 = model.GetCurrentY();
            model.DrawLine(700, lngY2, 11500, lngY2);
            lngY2 = model.GetCurrentY();
            //model.DrawLine(700, lngY2, 11800, lngY2);
            model.DrawLine(700, lngY1, 700, lngY2);
            model.DrawLine(11500, lngY1, 11500, lngY2);
            //model.DrawLine(11800, lngY1, 11800, lngY2);
            model.SetTable("");
            model.SetTable("");
            model.SetFontSize(8);
            model.SetFontName("Arial Narrow");
            lngY1 = model.GetCurrentY();
            model.DrawLine(700, lngY1, 11500, lngY1);
            model.SetFontBold(1);
            //model.SetTable("^800|^1000|^800|^1200|^1200|^600|^400|^1200|^1200|^1200|^1200;DATE|OR#|TAX YEAR|CAPITAL|GROSS|TERM|QTR|TAX|FEES|SUR/PEN|TOTAL");
            model.SetTable("^800|^1000|^800|^1200|^1200|^600|^400|^800|^1000|^1000|^1000|^1000;DATE|OR#|TAX YEAR|CAPITAL|GROSS|TERM|QTR|NO. OF QTR|TAX|FEES|SUR/PEN|TOTAL");    // RMC 20130109 display no. of qtr in payment hist
            model.SetFontBold(0);
            lngY2 = model.GetCurrentY();
            model.DrawLine(700, lngY2, 11500, lngY2);
            model.DrawLine(700, lngY1, 700, lngY2);
            model.DrawLine(11500, lngY1, 11500, lngY2);
            model.SetTable("");

            //PayHistPrint();
            string sBnsCode = string.Empty;
            string cOrNo = string.Empty;
            result.Query = "select distinct * from pay_hist_temp where bin ='" + sBin + "' order by or_date asc, tax_year, qtr asc";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sDate = result.GetDateTime("or_date").ToShortDateString();
                    sOrNo = result.GetString("or_no");
                    sTxYr = result.GetString("tax_year");
                    sTerm = result.GetString("term");
                    sQtr = result.GetString("qtr");
                    sTax = string.Format("{0:#,##0.00}", result.GetDouble("tax"));
                    sFees = string.Format("{0:#,##0.00}", result.GetDouble("fees"));
                    sSurPen = string.Format("{0:#,##0.00}", result.GetDouble("surch_pen"));
                    sTotal = string.Format("{0:#,##0.00}", result.GetDouble("total"));
                    sNoOfQtr = GetNoOfQtr(sOrNo);   // RMC 20130109 display no. of qtr in payment hist

                    sCapital = AppSettingsManager.GetCapitalGross(sBin.Trim(), AppSettingsManager.GetBnsCodeMain(sBin.Trim()), sTxYr.Trim(), "NEW");
                    sGross = AppSettingsManager.GetCapitalGross(sBin.Trim(), AppSettingsManager.GetBnsCodeMain(sBin.Trim()), sTxYr.Trim(), "REN");
                    //model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|>1200|>1200|>1200|>1200;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", sDate, sOrNo, sTxYr, sCapital, sGross, sTerm, sQtr, sTax, sFees, sSurPen, sTotal));
                    //model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sDate, sOrNo, sTxYr, sCapital, sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal));  // RMC 20130109 display no. of qtr in payment hist
                    
                    GetBnsCode(sBin, sOrNo, sTxYr, sDate, sCapital, sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal);
                    //model.SetTable("");
                }
            }
            result.Close();


            model.SetTable("");
            lngY2 = model.GetCurrentY();
            model.DrawLine(700, lngY2, 11500, lngY2);
            model.SetTable("");
            model.SetTable("");
            model.SetTable("");

            //JAV 20170803(s)
            string strCurrentDate = string.Empty;
            string strCurrentTime = string.Empty;
            string strUser = string.Empty;
            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            strCurrentTime = string.Format("{0:hh:mm:ss tt}", AppSettingsManager.GetCurrentDate());
            strUser = AppSettingsManager.SystemUser.UserCode;
            model.SetTable("<1100|^500|<9400;DATE PRINTED|:|" + strCurrentDate);
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.SetTable("<1100|^500|<9400;TIME PRINTED|:|" + strCurrentTime);
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            model.SetTable("<1100|^500|<9400;PRINTED BY|:|" + strUser);
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //JAV 20170803(e)

            // RMC 20170130 added signatory in Payment History of BTAS (s)
            //if (AppSettingsManager.GetSystemType == "C")
            //{
            //    model.SetTable("");
            //    model.SetTable("");
            //    model.SetFontBold(1);
            //    model.SetTable(string.Format("<7800|^3000;|{0}", AppSettingsManager.GetConfigValue("05")));
            //    model.SetFontBold(0);
            //    model.SetTable(string.Format("<7800|^3000;|{0}", AppSettingsManager.GetConfigValue("01") + " TREASURER"));
            //}

        }

        private void GetBnsCode(string sBin, string sOrNo, string sTxYr, string sDate, string sCapital, string sGross, string sTerm, string sQtr, string sNoOfQtr, string sTax, string sFees, string sSurPen, string sTotal)
        {

            OracleResultSet pRec = new OracleResultSet();
            string xy = string.Empty;
            string yx = string.Empty;
            string sBnsDesc = string.Empty;
            pRec.Query = string.Format("select distinct bns_code_main from or_table where or_no = '{0}' and tax_year = '{1}'", sOrNo, sTxYr);
            if (pRec.Execute())
            {

                while (pRec.Read())
                {
                    String sFeesCode = pRec.GetString("bns_code_main").Trim(); // sFeesCode or sBnsCode
                    double dTax = 0;
                    double dFees = 0;
                    double dSurchPen = 0;
                    double dTotal = 0;
                    
                    
                    OracleResultSet pRec2 = new OracleResultSet();

                    pRec2.Query = string.Format("select bns_desc from bns_table where bns_code ={0}", sFeesCode);
                    if (pRec2.Execute())
                    {
                        
                        while (pRec2.Read())
                        {
                            sBnsDesc = pRec2.GetString("bns_desc");
                        }
                    } 
                    pRec2.Close();

                    pRec2.Query = "select * from or_table where or_no = '" + sOrNo.Trim() + "' and fees_code = 'B' and tax_year = '" + sTxYr + "'";
                    if (pRec2.Execute())
                    {

                        while (pRec2.Read())
                        {


                            try
                            {
                                dTax = pRec2.GetDouble("fees_due");
                            }
                            catch (Exception ex)
                            {
                                dTax = 0;
                            }
                        }
                    }
                    pRec2.Close();

                    pRec2.Query = "select * from or_table where or_no = '" + sOrNo.Trim() + "' and fees_code != 'B' and tax_year = '" + sTxYr + "' and bns_code_main = '" + sFeesCode + "' ";
                    if (pRec2.Execute())
                    {
                        while (pRec2.Read())
                        {
                            try
                            {
                                dFees += pRec2.GetDouble("fees_due");
                            }
                            catch (Exception ex)
                            {
                                dFees += 0;
                            }
                        }
                    }
                    pRec2.Close();

                    pRec2.Query = "select * from or_table where or_no = '" + sOrNo.Trim() + "' and tax_year = '" + sTxYr + "' and bns_code_main = '" + sFeesCode + "' ";
                    if (pRec2.Execute())
                    {
                        while (pRec2.Read())
                        {
                            try
                            {
                                dSurchPen += pRec2.GetDouble("fees_surch") + pRec2.GetDouble("fees_pen");
                            }
                            catch (Exception ex)
                            {
                                dSurchPen += 0;
                            }
                        }
                    }
                    pRec2.Close();

                    dTotal = dTax + dFees + dSurchPen;
                    sTax = string.Format("{0:#,##0.00}", dTax);
                    sFees = string.Format("{0:#,##0.00}", dFees);
                    sSurPen = string.Format("{0:#,##0.00}", dSurchPen);
                    sTotal = string.Format("{0:#,##0.00}", dTotal);

                    //GetCapGross(sBin.Trim(), sTxYr, sFeesCode);
                    GetCapGross(sBin.Trim(), sTxYr, sFeesCode, sOrNo, BnsCode); //JHB 20181203 
                    
                    
                    if (sTxYr != xy)
                    {
                        model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;{0}|{1}|{2}|||{3}|{4}|{5}||||", sDate, sOrNo, sTxYr, "", "", ""));
                        if (sFeesCode != yx) 
                        {
                            //model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", AppSettingsManager.GetBnsDesc(sFeesCode), sFeesCode, m_sCap, m_sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal));
                            model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", AppSettingsManager.GetBnsDesc(sFeesCode), sFeesCode, m_sCap, m_sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal));
                        }
                        xy = sTxYr;
                        //yx = sOrNo;
                    }
                    else
                    {
                        //model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", AppSettingsManager.GetBnsDesc(sFeesCode), sFeesCode, m_sCap, m_sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal));
                        model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", sBnsDesc, sFeesCode, m_sCap, m_sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal));
                    }
                   // model.SetTable(string.Format(""));      
                }
            }
        }

        public void NTRC() //JARS 20170907
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            OracleResultSet pSet3 = new OracleResultSet();

            string sBnsGross1 = string.Empty;
            string sBnsGross2 = string.Empty;
            string sBin = string.Empty;
            double dBnsGr = 0;
            double dBnsTaxPaid = 0;
            double dTotalGross = 0;
            double dTotalTaxPaid = 0;
            int iCnt = 0;

            model.SetFontName("Arial");
            model.SetFontSize(11);
            model.SetTable("^11000;Republic of the Philippines");
            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                model.SetTable("^11000;" + AppSettingsManager.GetConfigValue("02"));
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                model.SetTable("^11000;MUNICIPALITY OF" + AppSettingsManager.GetConfigValue("02"));
            model.SetTable("");
            model.SetFontSize(12);
            model.SetFontBold(1);
            model.SetTable("^11000;NATIONAL TAX RESEARCH CENTER");
            model.SetFontBold(0);
            model.SetTable("");
            model.SetTable("");
            model.SetFontSize(9);
            model.SetTable("<11000;Business Type: " + m_sBusinessDesc);
            model.SetTable("");
            model.SetTable("^500|^3000;|GROSS SALES OR RECIEPTS FOR THE");
            model.SetTable("^500|^3000|^2000|^2000|^2000|^2000;|PRECEEDING CALENDAR YEAR|TAX RATE|NUMBER OF|TOTAL GROSS|TOTAL AMOUNT");
            model.SetTable("^500|^1500|^1500|^2000|^2000|^2000|^2000;|FROM|TO||TAX PAYERS|SALES OR RECIEPTS|OF TAXES PAID");
            model.SetTable("");

            pSet.Query = "select gr_1, gr_2 from btax_sched  where bns_code = '" + m_sBusinessCode + "' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' order by gr_1";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iCnt = 0;
                    dBnsTaxPaid = 0;
                    dBnsGr = 0;

                    sBnsGross1 = pSet.GetDouble("gr_1").ToString();
                    sBnsGross2 = pSet.GetDouble("gr_2").ToString();

                    pSet2.Query = "select a.bin as BIN,a.tax_year as TAX_YEAR,sum(a.gr_1) as gr_1 from (";
                    pSet2.Query += "select BIN, BNS_STAT, bns_code, nvl(gr_1,0) as gr_1, tax_year from businesses where tax_year = '" + m_sTaxYear + "' and bns_code like '" + m_sBusinessCode + "%' union ";
                    pSet2.Query += "select BIN, BNS_STAT, bns_code, nvl(gr_1,0) as gr_1, tax_year from buss_hist where tax_year = '" + m_sTaxYear + "' and bns_code like '" + m_sBusinessCode + "%'";
                    pSet2.Query += "and bin not in (select BIN from businesses where tax_year = " + m_sTaxYear + ") union ";
                    pSet2.Query += "select BIN,BNS_STAT,BNS_CODE_MAIN,nvl(gross,0) as gross,TAX_YEAR from addl_bns where tax_year = '" + m_sTaxYear + "' and BNS_CODE_MAIN like '" + m_sBusinessCode + "%') a ";
                    pSet2.Query += "group by a.BIN,a.tax_year having sum(a.gr_1) between " + sBnsGross1 + " and " + sBnsGross2 + "";

                    if (pSet2.Execute())
                    {
                        while (pSet2.Read())
                        {
                            iCnt++;
                            sBin = pSet2.GetString("BIN");
                            dBnsGr += pSet2.GetDouble("gr_1");

                            pSet3.Query = "select distinct OT.or_no,OT.fees_amtdue as fees_amtdue From pay_hist PH ";
                            pSet3.Query += "inner join or_table OT on OT.or_no = PH.OR_NO ";
                            pSet3.Query += "where PH.bin = '" + sBin + "' and PH.tax_year = '" + m_sTaxYear + "' and PH.data_mode <> 'UNP'";

                            if (pSet3.Execute())
                            {
                                while (pSet3.Read())
                                {
                                    dBnsTaxPaid += pSet3.GetDouble("fees_amtdue");
                                }
                            }
                            pSet3.Close();
                        }
                    }
                    pSet2.Close();

                    model.SetTable(">1500|>1500|^2000|^2000|>2000|>2000;" + sBnsGross1 + "|" + sBnsGross2 + "||" + iCnt.ToString("#,##0") + "|" + dBnsGr.ToString("#,##0.00") + "|" + dBnsTaxPaid.ToString("#,##0.00"));

                    dTotalGross += dBnsGr;
                    dTotalTaxPaid += dBnsTaxPaid;
                }
            }
            pSet.Close();
            model.SetTable("");
            model.SetFontBold(1);
            model.SetTable("^1500|^1500|^2000|^2000|>2000|>2000;|||TOTAL:|" + dTotalGross.ToString("#,##0.00") + "|" + dTotalTaxPaid.ToString("#,##0.00"));
            model.SetFontBold(0);

        }
    }
}