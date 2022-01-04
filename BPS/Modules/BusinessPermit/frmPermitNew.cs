
// RMC 20120419 added display of tax credit used in Permit
// RMC 20120418 corrections in total displayed in permit
// RMC 20120413 Corrections in printing Permit
// RMC 20120411 Disable RMC 20120315 mods by Jester's meeting with the BPLS
// RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ 
// RMC 20120315 added option to print or not penalty and surcharge in Business Permit
// RMC 20120228 modified printing of permit with change of line of business and payment adjustment
// RMC 20120228 added printing of mp for new payment of addl bns that does not have buss tax
// RMC 20120221 corrected printing of payment due dates in Permit
// RMC 20120221 added printing of newly added other of business in Permit
// RMC 20120125 modified printing of permit with addl payment
// RMC 20120124 added printing of remarks in Permit (user-request)
// RMC 20120120 added order by qtr_paid in permit printing
// RMC 20120117 added filter of current year in Permit printing
// RMC 20120116 added display of exemption (peza/boi) in permit
// RMC 20120111 added display of other line of businesses with no paid tax if exempted in Business permit printing
// RMC 20120105 Modified getting of unit in printing Permit
// RMC 20120103 Modified getting of area in printing Permit
// RMC 20111219 Added tax year in trail of permit printing
// RMC 20110819 added filter by due_year
// RMC 20110810 modified

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.BusinessPermit
{
    public partial class frmPermitNew : Form
    {
        private string m_sReportSwitch = "";
        private string m_sBIN = "";
        private DateTime m_dtDate = DateTime.Now;
        private string m_sOwnCode = "";
        private string m_sPermitNo = "";
        private string m_sTaxYear = "";
        private string m_sNotation = "";
        private string m_sAddlOr = "";    // RMC 20120125 modified printing of permit with addl payment
        private string m_sAddlOrDate = "";  // RMC 20120125 modified printing of permit with addl payment
        private double m_dRemainingCredit = 0;  // RMC 20120228 modified printing of permit with change of line of business and payment adjustment
        private double m_dAppliedCredit = 0;
        private string m_sOrgnKind = "";
        private string m_sBnsStat = "";
        private string m_sBnsOccupancy = ""; //AFM 20190823
        private string m_sMemo = "";
        private string m_sFlrArea = "";
        private string m_sTelNo = "";
        private string m_sUnitNo = "";
        private string m_sRoomNo = "";
        private string m_sGross = "";
        private string m_sCapital = "";
        private string m_sRemarks = "";
        private string m_sPlate = "";
        private string m_sNoEmp = "";
        private string m_sTinNo = "";
        private string m_sCitizenship = "";
        private string[] m_sArrayBnsCode = new string[] { "" };
        private bool m_bPermitUpdateNoBilling = true;
        private double m_dAddlFeesDue = 0;  // RMC 20120413 Corrections in printing Permit
        private bool m_bWithBalQtr = false; // RMC 20120413 Corrections in printing Permit
        private string m_sOrNo = "";        // RMC 20120413 Corrections in printing Permit
        private string m_sOrDate = "";      // RMC 20120413 Corrections in printing Permit
        private string m_sMaxQtrPaid = "";  // RMC 20120413 Corrections in printing Permit
        private bool m_bForTYletter = false; //MCR 20140304 Thank you letter for new businesses
        private string m_sTempList = string.Empty;  // RMC 20171110 added configurable option to print temporary permit - requested by Malolos
        private string m_sDTIDt = string.Empty; // RMC 20171117 revised format of Business Permit
        private DateTime m_dDTIDtComp; // MCR 20190724
        private string[] m_sArrayInfo = new string[] { "" };    // RMC 20171124 customized special permit printing where payment was made at aRCS

        public string ReportSwitch
        {
            get { return m_sReportSwitch; }
            set { m_sReportSwitch = value; }
        }

        public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; }
        }

        public DateTime NoticeDate
        {
            get { return m_dtDate; }
            set { m_dtDate = value; }
        }

        public string OwnCode
        {
            get { return m_sOwnCode; }
            set { m_sOwnCode = value; }
        }

        public string PermitNo
        {
            get { return m_sPermitNo; }
            set { m_sPermitNo = value; }
        }

        public string TaxYear
        {
            get { return m_sTaxYear; }
            set { m_sTaxYear = value; }
        }

        public string TempList  // RMC 20171110 added configurable option to print temporary permit - requested by Malolos
        {
            get { return m_sTempList; }
            set { m_sTempList = value; }
        }

        public string[] ArrayInfo   // RMC 20171124 customized special permit printing where payment was made at aRCS
        {
            get { return m_sArrayInfo; }
            set { m_sArrayInfo = value; }
        }

        public frmPermitNew()
        {
            InitializeComponent();
        }

        private void frmPermitNew_Load(object sender, EventArgs e)
        {
            //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter; // AST 20160126 rem
            //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser; // AST 20160126
            //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
            this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4; //JARS 20170815
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

            this.axVSPrinter1.MarginLeft = 1200;
            if (AppSettingsManager.GetConfigValue("10") == "243")   // RMC 20161204 customized permit for Binan
                this.axVSPrinter1.MarginTop = 200;
            else
                this.axVSPrinter1.MarginTop = 1000;
            this.axVSPrinter1.MarginBottom = 500;

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            if (m_sReportSwitch == "Permit")
            {    //PrintPermitModified();
                //PrintPermitMati();
                if (AppSettingsManager.GetConfigValue("10") == "019")
                    PrintPermitLubao();
                else if (AppSettingsManager.GetConfigValue("10") == "011")  // RMC 20150425 modified printing of permit for Tumauini
                    PrintPermitTumauini();
                else if (AppSettingsManager.GetConfigValue("10") == "243")  // RMC 20161204 customized permit for Binan
                    PrintPermitBinan();
                else if (AppSettingsManager.GetConfigValue("10") == "216")  // JARS 20170815 FOR MALOLOS BPLS.NET
                    //MCR 20190702 (s)
                    if (AppSettingsManager.GetConfigValue("70") == "Y") 
                        PrintPermitMalolosNew();
                    //MCR 20190702 (e)
                    else
                        PrintPermitMalolos();
                //PrintPermitMalolosNew();    // RMC 20171117 revised format of Business Permit
                else
                    PrintPermitMatiNew(); // CJC 20131111
            }
            // RMC 20171110 added configurable option to print temporary permit - requested by Malolos (s)
            else if (m_sReportSwitch == "Temporary Permit")
            {
                PrintTempPermitMalolosNew(); //AFM 20190813 new temporary permit format
                //PrintTemporaryPermit();
            }// RMC 20171110 added configurable option to print temporary permit - requested by Malolos (e)
            else if (m_sReportSwitch == "Special Permit")
                PrintSpecialPermit();   // RMC 20171124 customized special permit printing where payment was made at aRCS
            else
                PrintCertificate();

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void PrintPermit()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            string sBnsNm = "";
            string sOwnNm = "";
            string sBnsAddress = "";
            string sBnsMainDesc = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sORNo = "";
            string sOrgnKind = "";
            string sBnsStat = "";
            string sTelNo = "";
            string sDay = "";
            string sCapital = "";
            string sGross = "";
            string sFeeAmt = "";
            string sRemarks = "";
            string sFlrArea = "";
            string sUnitNo = "";
            string sRoomNo = "";
            string sQtrRemarks = "";
            string sORDate = "";
            string sQtrPaid = "";
            string sPlate = "";
            string sCitizenship = "";
            string sQtrsPaid = "";
            string sMemo = "";  // RMC 20120124 added printing of remarks in Permit (user-request)
            DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate;
            string sOldBnsCode = "";    // RMC 20120228 modified printing of permit with change of line of business and payment adjustment

            pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
            pSet.Query += " and (qtr_paid <> 'A' and qtr_paid <> 'P')"; // RMC 20120125 modified printing of permit with addl payment
            pSet.Query += " order by or_date desc, qtr_paid desc";    // RMC 20120120 added order by qtr_paid in permit printing
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sQtrPaid = pSet.GetString("qtr_paid");
                    sBnsNm = AppSettingsManager.GetBnsName(m_sBIN);
                    sOwnNm = AppSettingsManager.GetBnsOwnCode(m_sBIN);
                    sOwnNm = AppSettingsManager.GetBnsOwner(sOwnNm);
                    sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
                    sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);

                    // RMC 20120228 modified printing of permit with change of line of business and payment adjustment (s)
                    sOldBnsCode = GetOldBnsType(sBnsCode);
                    // RMC 20120228 modified printing of permit with change of line of business and payment adjustment (e)

                    sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
                    sBnsMainDesc = AppSettingsManager.GetSubBnsDesc(sBnsCode.Substring(0, 2), "B");
                    sBnsMainDesc = GetExemption(sBnsMainDesc);
                    sORNo = pSet.GetString("or_no");

                    //sQtrsPaid = ConsolidatedQtr(sORNo, sQtrPaid);   // RMC 20120120   // RMC 20120413 Corrections in printing Permit, put rem
                    dtpORDate = pSet.GetDateTime("or_date");
                    sORDate = string.Format("{0:MMMM dd, yyyy}", dtpORDate);
                    m_sAddlOr = GetAddlOr(sORNo, sORDate);  // RMC 20120125 modified printing of permit with addl payment

                    // RMC 20120228 modified printing of permit with change of line of business and payment adjustment (s)
                    if (sOldBnsCode != "")
                        sBnsCode = sOldBnsCode;
                    // RMC 20120228 modified printing of permit with change of line of business and payment adjustment (e)

                    pRec.Query = string.Format("select * from businesses where bin = '{0}'", m_sBIN);
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sOrgnKind = pRec.GetString("orgn_kind").Trim();
                            sBnsStat = pRec.GetString("bns_stat");
                            sMemo = pRec.GetString("memoranda");    // RMC 20120124 added printing of remarks in Permit (user-request)

                            // RMC 20120103 Modified getting of area in printing Permit (s)
                            sFlrArea = GetArea(m_sBIN, sBnsCode, m_sTaxYear);
                            if (sFlrArea == "0.00")
                                sFlrArea = "AREA: " + string.Format("{0:##0.00}", pRec.GetDouble("flr_area")); // RMC 20110810 modified
                            else
                                sFlrArea = "AREA: " + sFlrArea;
                            // RMC 20120103 Modified getting of area in printing Permit (e)

                            //sFlrArea = "AREA: " + string.Format("{0:##0.00}", pRec.GetDouble("flr_area")); // RMC 20110810 modified
                            sTelNo = pRec.GetString("bns_telno");

                            //sUnitNo = "No. of Units: ";     // pending anong field ito?
                            // RMC 20120105 Modified getting of unit in printing Permit (s)
                            sUnitNo = "No. of Units: " + GetUnit(m_sBIN, sBnsCode, m_sTaxYear, "UNIT");
                            sRoomNo = "No. of Rooms: " + GetUnit(m_sBIN, sBnsCode, m_sTaxYear, "ROOM");
                            // RMC 20120105 Modified getting of unit in printing Permit (e)

                            if (sBnsStat == "REN")
                            {
                                sBnsStat = "RENEWAL";
                                sGross = string.Format("{0:#,##0.00}", pRec.GetDouble("gr_1"));
                                sCapital = "";

                                if (sGross == "0.00")
                                    //    sRemarks = "No Operation";
                                    sRemarks = "Zero Gross";
                            }
                            else if (sBnsStat == "NEW")
                            {
                                sCapital = string.Format("{0:#,##0.00}", pRec.GetDouble("capital"));
                                sGross = "";
                            }

                        }
                    }
                    pRec.Close();

                    pRec.Query = String.Format("select * from buss_plate where bin = '{0}'", m_sBIN);
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sPlate = pRec.GetString("bns_plate");
                        }
                    }
                    pRec.Close();

                    pRec.Query = string.Format("select * from own_profile where own_code = '{0}'", AppSettingsManager.GetBnsOwnCode(m_sBIN));
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sCitizenship = pRec.GetString("citizenship");
                        }
                    }
                    pRec.Close();

                    string strData = "";

                    this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
                    //this.axVSPrinter1.CurrentY = 800;
                    this.axVSPrinter1.CurrentY = 900;
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontName = "Arial Narrow";
                    this.axVSPrinter1.FontSize = (float)10.0;
                    strData = "|" + m_sPermitNo + "||" + sPlate;
                    this.axVSPrinter1.Table = string.Format("<150|<1850|^5800|>2000;{0}", strData);
                    this.axVSPrinter1.FontBold = false;

                    this.axVSPrinter1.MarginLeft = 3000;
                    this.axVSPrinter1.CurrentY = 3499;
                    //this.axVSPrinter1.CurrentY = 3599;
                    this.axVSPrinter1.Table = string.Format("<10000;{0}", sBnsNm);
                    this.axVSPrinter1.Table = string.Format("<10000;{0}", sBnsAddress);
                    this.axVSPrinter1.Table = string.Format("<10000;{0}", sOwnNm);

                    if (sOrgnKind == "SINGLE PROPRIETORSHIP")
                        sOrgnKind = "SINGLE PROP.";

                    strData = sTelNo + "||" + sOrgnKind + "||" + sCitizenship;
                    this.axVSPrinter1.Table = string.Format("<3000|<1000|<2000|<1000|<2000;{0}", strData);

                    //payment details

                    double dFeeAmt = 0;
                    pRec.Query = string.Format("select fees_due, qtr_paid from or_table where or_no = '{0}' and fees_code = 'B' and bns_code_main = '{1}' and tax_year = '{2}'", sORNo, sBnsCode, m_sTaxYear);  // RMC 20110810 modified   // RMC 20120117 added filter of current year in Permit printing
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            dFeeAmt += pRec.GetDouble(0);
                            //sQtrPaid = pRec.GetString(1);
                        }

                        // RMC 20120228 modified printing of permit with change of line of business and payment adjustment (s)
                        if (sOldBnsCode != "")
                        {
                            dFeeAmt -= GetTaxCreditAmt();
                        }
                        // RMC 20120228 modified printing of permit with change of line of business and payment adjustment (e)

                        sFeeAmt = string.Format("{0:#,##0.00}", dFeeAmt);
                    }
                    pRec.Close();

                    // RMC 20120125 modified printing of permit with addl payment (s)
                    if (m_sAddlOr.Trim() != "")
                    {
                        pRec.Query = string.Format("select fees_due, qtr_paid from or_table where or_no = '{0}' and fees_code = 'B' and bns_code_main = '{1}' and tax_year = '{2}'", m_sAddlOr, sBnsCode, m_sTaxYear);
                        if (pRec.Execute())
                        {
                            while (pRec.Read())
                            {
                                dFeeAmt += pRec.GetDouble(0);
                            }

                            sFeeAmt = string.Format("{0:#,##0.00}", dFeeAmt);
                        }
                        pRec.Close();
                    }
                    // RMC 20120125 modified printing of permit with addl payment (e)

                    if (sQtrPaid == "F")
                        sQtrPaid = "Annually";
                    else
                    {
                        if (sQtrPaid == "1")
                            sQtrPaid = "1st Qtr";
                        else if (sQtrPaid == "2")
                            sQtrPaid = "2nd Qtr";
                        else if (sQtrPaid == "3")
                            sQtrPaid = "3rd Qtr";
                        else
                            sQtrPaid = "4th Qtr";
                    }

                    if (sQtrPaid != "Annually" && sQtrPaid != "4th Qtr")    // RMC 20120120 added order by qtr_paid in permit printing   
                    {
                        this.axVSPrinter1.CurrentY = 6938;
                        this.axVSPrinter1.MarginLeft = 8600;
                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.FontSize = (float)7.0;
                        sQtrRemarks = "BALANCE TO BE ";
                        this.axVSPrinter1.Table = string.Format(">3000;{0}", sQtrRemarks);
                        sQtrRemarks = "PAID ON OR BEFORE:";
                        this.axVSPrinter1.Table = string.Format(">3000;{0}", sQtrRemarks);

                        DateTime dtDate;
                        string sDate = "";


                        pRec.Query = "select * from due_dates where due_year = '" + m_sTaxYear + "' order by due_code ";    // RMC 20110819 added filter by due_year
                        if (pRec.Execute())
                        {
                            while (pRec.Read())
                            {
                                if (pRec.GetString("due_code") == "04" ||
                                    pRec.GetString("due_code") == "07" ||
                                    pRec.GetString("due_code") == "10")
                                {
                                    dtDate = pRec.GetDateTime("due_date");
                                    sDate = string.Format("{0:MMMM dd,yyyy}", dtDate);
                                    if (pRec.GetString("due_code") == "04")
                                        sQtrRemarks = "2nd Qtr. " + sDate;
                                    if (pRec.GetString("due_code") == "07")
                                        sQtrRemarks = "3rd Qtr. " + sDate;
                                    if (pRec.GetString("due_code") == "10")
                                        sQtrRemarks = "4th Qtr. " + sDate;

                                    /*if (sQtrPaid == "2nd Qtr" && pRec.GetString("due_code") == "04")
                                    { }
                                    else if (sQtrPaid == "3rd Qtr" && pRec.GetString("due_code") == "07")
                                    { }
                                    else*/
                                    // RMC 20120221 corrected printing of payment due dates in Permit
                                    // RMC 20120221 corrected printing of payment due dates in Permit (s)
                                    if (sQtrPaid == "2nd Qtr" && Convert.ToInt32(pRec.GetString("due_code")) <= 4)
                                    { }
                                    else if (sQtrPaid == "3rd Qtr" && Convert.ToInt32(pRec.GetString("due_code")) <= 7)
                                    { }
                                    else
                                        // RMC 20120221 corrected printing of payment due dates in Permit (e)
                                        this.axVSPrinter1.Table = string.Format(">3000;{0}", sQtrRemarks);

                                }
                            }

                        }
                        pRec.Close();

                        this.axVSPrinter1.MarginLeft = 2000;
                    }

                    this.axVSPrinter1.CurrentY = 5150;
                    this.axVSPrinter1.MarginLeft = 1200;


                    string sAddlBns = "";
                    string sAddlStat = "";
                    double dAddlCap = 0;
                    double dAddlGross = 0;

                    int intC = 0;
                    pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "'";
                    pRec.Query += " and tax_year = '" + m_sTaxYear + "'";   // RMC 20120106 corrected
                    if (pRec.Execute())
                    {

                        while (pRec.Read())
                        {
                            intC++;

                            sAddlStat = pRec.GetString("bns_stat");
                            if (sAddlStat == "NEW")
                                dAddlCap += pRec.GetDouble("capital");
                            else
                                dAddlGross += pRec.GetDouble("gross");


                        }

                        if (sCapital != "")
                            dAddlCap += Convert.ToDouble(sCapital);
                        if (sGross != "")
                            dAddlGross += Convert.ToDouble(sGross);

                        if (dAddlCap != 0)
                            sCapital = string.Format("{0:#,##0.00}", dAddlCap);

                        if (dAddlGross != 0)
                            sGross = string.Format("{0:#,##0.00}", dAddlGross);
                    }
                    pRec.Close();


                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.CurrentY = 5849;

                    // include breakdown of other line's business tax & mayor's permit
                    string sFeesCode = "";
                    string sFeeCode = "";
                    pRec.Query = string.Format("select * from tax_and_fees_table where (fees_desc = '{0}'", StringUtilities.HandleApostrophe(sFeesCode));
                    pRec.Query += " or fees_desc like '%MAYOR%')";
                    pSet.Query += " and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sFeesCode = pRec.GetString("fees_code");
                        }
                    }
                    pRec.Close();

                    this.axVSPrinter1.FontSize = (float)8.0;
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>100|<2000|>500|<1000;" + sBnsMainDesc + "|CAPITAL INVESTMENT:||" + sCapital + "||" + sBnsStat);
                    this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|<2000|>1000|<1000;" + "||GROSS SALES:||" + sGross + "|||");
                    /*main business*/

                    //this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + "|BUSINESS TAX:||" + sFeeAmt + "|" + sQtrPaid); 
                    this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + "|BUSINESS TAX:||" + sFeeAmt + "|" + sQtrsPaid);  // RMC 20120120 added order by qtr_paid in permit printing
                    sFeeCode = AppSettingsManager.GetFeesDesc(sFeesCode);
                    sFeeAmt = GetFeeAmount(sORNo, sFeesCode, sBnsCode, m_sTaxYear);
                    //this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "|" + sQtrPaid);
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "|" + sQtrsPaid); // RMC 20120120 added order by qtr_paid in permit printing
                    /*main business*/

                    string sTempBnsCode = "";
                    string sTempBnsDesc = "";

                    if (m_sAddlOr.Trim() != "")
                        pRec.Query = string.Format("select distinct bns_code_main from or_table where (or_no = '{0}' or or_no = '{1}') and fees_code = 'B' and bns_code_main <> '{2}' and tax_year = '{3}' order by fees_code ", sORNo, m_sAddlOr, sBnsCode, m_sTaxYear);   // RMC 20120221 added printing of newly added other of business in Permit
                    else
                        pRec.Query = string.Format("select distinct bns_code_main from or_table where or_no = '{0}' and fees_code = 'B' and bns_code_main <> '{1}' and tax_year = '{2}' order by fees_code ", sORNo, sBnsCode, m_sTaxYear); // RMC 20120117 added filter of current year in Permit printing   
                    if (pRec.Execute())
                    {
                        /*    string sTempBnsCode = "";
                             string sTempBnsDesc = "";
                        */
                        dFeeAmt = 0;

                        while (pRec.Read())
                        {
                            sTempBnsCode = pRec.GetString(0);

                            sFeeAmt = GetFeeAmount(sORNo, "B", sTempBnsCode, m_sTaxYear);
                            sTempBnsDesc = AppSettingsManager.GetBnsDesc(sTempBnsCode);

                            //this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sTempBnsDesc + "|BUSINESS TAX:||" + sFeeAmt + "|" + sQtrPaid);
                            this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sTempBnsDesc + "|BUSINESS TAX:||" + sFeeAmt + "|" + sQtrsPaid);  // RMC 20120120 added order by qtr_paid in permit printing

                            sFeeCode = AppSettingsManager.GetFeesDesc(sFeesCode);
                            sFeeAmt = GetFeeAmount(sORNo, sFeesCode, sTempBnsCode, m_sTaxYear);
                            //this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "|" + sQtrPaid);
                            this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "|" + sQtrsPaid);  // RMC 20120120 added order by qtr_paid in permit printing
                        }
                    }
                    pRec.Close();

                    // RMC 20120111 added display of other line of businesses with no paid tax if exempted in Business permit printing (s)
                    if (sTempBnsCode == "")
                    {
                        pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "'";
                        pRec.Query += " and tax_year = '" + m_sTaxYear + "'";   // RMC 20120106 corrected
                        if (pRec.Execute())
                        {
                            string stmpbnscode = "";
                            while (pRec.Read())
                            {
                                stmpbnscode = pRec.GetString("bns_code_main");

                                /*if (BnsIsExempted(stmpbnscode))
                                {
                                    stmpbnscode = AppSettingsManager.GetBnsDesc(stmpbnscode);
                                    this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + stmpbnscode + "||||");
                                    
                                }*/

                                // RMC 20120228 added printing of mp for new payment of addl bns that does not have buss tax (s)

                                if (m_sAddlOr.Trim() != "")
                                {
                                    OracleResultSet pTmpRec = new OracleResultSet();

                                    pTmpRec.Query = string.Format("select distinct bns_code_main from or_table where (or_no = '{0}' or or_no = '{1}') and bns_code_main = '{2}' and tax_year = '{3}' and fees_code = '{4}'  order by fees_code ", sORNo, m_sAddlOr, stmpbnscode, m_sTaxYear, sFeesCode);

                                    stmpbnscode = AppSettingsManager.GetBnsDesc(stmpbnscode);
                                    this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + stmpbnscode + "||||");

                                    if (pTmpRec.Execute())
                                    {
                                        if (pTmpRec.Read())
                                        {
                                            stmpbnscode = pTmpRec.GetString(0);

                                            sFeeAmt = GetFeeAmount(sORNo, sFeesCode, stmpbnscode, m_sTaxYear);
                                            sFeeCode = AppSettingsManager.GetFeesDesc(sFeesCode);
                                            sTempBnsDesc = AppSettingsManager.GetBnsDesc(stmpbnscode);

                                            this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "|" + sQtrsPaid);
                                        }
                                    }
                                    pTmpRec.Close();
                                }
                                // RMC 20120228 added printing of mp for new payment of addl bns that does not have buss tax (e)
                            }
                        }
                        pRec.Close();
                    }
                    // RMC 20120111 added display of other line of businesses with no paid tax if exempted in Business permit printing (E)


                    this.axVSPrinter1.FontBold = false;


                    int intCnt = 0;

                    //pRec.Query = string.Format("select * from or_table where or_no = '{0}' and fees_code <> 'B' and fees_code <> '{1}' and tax_year = '{2}' order by qtr_paid, fees_code", sORNo, sFeesCode, m_sTaxYear);  // RMC 20110822  // RMC 20120120 added order by qtr_paid in permit printing

                    if (m_sAddlOr.Trim() != "") // RMC 20120125 modified printing of permit with addl payment
                        pRec.Query = string.Format("select sum(fees_due), fees_code from or_table where (or_no = '{0}' or or_no = '{1}') and fees_code <> 'B' and fees_code <> '{2}' and tax_year = '{3}' group by fees_code order by fees_code", sORNo, m_sAddlOr, sFeesCode, m_sTaxYear);
                    else
                        pRec.Query = string.Format("select sum(fees_due), fees_code from or_table where or_no = '{0}' and fees_code <> 'B' and fees_code <> '{1}' and tax_year = '{2}' group by fees_code order by fees_code", sORNo, sFeesCode, m_sTaxYear);  // RMC 20110822  // RMC 20120120 added order by qtr_paid in permit printing
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            string sTmpQtr = "";
                            sTmpQtr = sQtrsPaid;

                            intCnt++;
                            if (intCnt > 2)
                                sQtrPaid = "";

                            sFeeCode = pRec.GetString("fees_code");
                            // RMC 20120120 added order by qtr_paid in permit printing (s)
                            if (GetFeeTerm(sFeeCode) == "F")
                                sTmpQtr = "";
                            else
                                sTmpQtr = sQtrsPaid;
                            // RMC 20120120 added order by qtr_paid in permit printing (e)

                            sFeeCode = AppSettingsManager.GetFeesDesc(sFeeCode);

                            //sFeeAmt = string.Format("{0:#,##0.00}", pRec.GetDouble("fees_due"));
                            sFeeAmt = string.Format("{0:#,##0.00}", pRec.GetDouble(0)); // RMC 20120120

                            //this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "|" + sQtrPaid);  // RMC 20110810 adjusted
                            this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "|" + sTmpQtr);  // RMC 20120120
                        }
                    }
                    pRec.Close();

                    string sSurch = "";
                    string sPen = "";
                    string sTotal = "";
                    string sCredit = "";

                    double dblSurch = 0;
                    double dblPen = 0;
                    double dblTotal = 0;
                    double dblCredit = 0;

                    pRec.Query = string.Format("select sum(fees_surch) from or_table where or_no = '{0}' and tax_year = '{1}'", sORNo, m_sTaxYear);
                    double.TryParse(pRec.ExecuteScalar(), out dblSurch);
                    sSurch = string.Format("{0:#,##0.00}", dblSurch);
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + sFlrArea + "|25% Penalty||" + sSurch + "|");

                    pRec.Query = string.Format("select sum(fees_pen) from or_table where or_no = '{0}' and tax_year = '{1}'", sORNo, m_sTaxYear);
                    double.TryParse(pRec.ExecuteScalar(), out dblPen);
                    sPen = string.Format("{0:#,##0.00}", dblPen);
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + sUnitNo + "|2% Interest||" + sPen + "|");

                    // tax credit (note: value for clarification )
                    pRec.Query = string.Format("select sum(credit) from debit_credit where bin = '{0}' and or_no = '{0}'", m_sBIN, sORNo);
                    double.TryParse(pRec.ExecuteScalar(), out dblCredit);
                    sCredit = string.Format("{0:#,##0.00}", dblCredit);
                    //this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + sRemarks + "|Tax Credits||" + sCredit + "|");
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + sRoomNo + "|Tax Credits||" + sCredit + "|");    // RMC 20120105 Modified getting of unit in printing Permit

                    if (m_sAddlOr.Trim() != "") // RMC 20120125 modified printing of permit with addl payment
                        pRec.Query = string.Format("select sum(fees_amtdue) from or_table where (or_no = '{0}' or or_no = '{1}') and tax_year = '{2}'", sORNo, m_sAddlOr, m_sTaxYear);
                    else
                        pRec.Query = string.Format("select sum(fees_amtdue) from or_table where or_no = '{0}' and tax_year = '{1}'", sORNo, m_sTaxYear);
                    double.TryParse(pRec.ExecuteScalar(), out dblTotal);

                    dblTotal -= GetTaxCreditAmt();  // RMC 20120228 modified printing of permit with change of line of business and payment adjustment

                    sTotal = string.Format("{0:#,##0.00}", dblTotal);

                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontBold = true;
                    //this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|TOTAL TAX PAID:||" + sTotal + "|");
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + sRemarks + "|TOTAL TAX PAID:||" + sTotal + "|");   // RMC 20120105 Modified getting of unit in printing Permit
                    this.axVSPrinter1.FontBold = false;

                    if (GetNotation(sORNo))
                    {
                        this.axVSPrinter1.CurrentY = 9222;
                        this.axVSPrinter1.MarginLeft = 2000;
                        this.axVSPrinter1.Table = string.Format("<10000;{0}", m_sNotation);
                    }

                    // RMC 20120124 added printing of remarks in Permit (user-request) (s)
                    if (sMemo.Trim() != "") // RMC 20120125
                    {
                        if (MessageBox.Show("Print Remarks?", "Business Permits", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.axVSPrinter1.CurrentY = 9622;
                            this.axVSPrinter1.MarginLeft = 2000;
                            this.axVSPrinter1.Table = string.Format("<10000;{0}", sMemo);
                        }
                    }
                    // RMC 20120124 added printing of remarks in Permit (user-request) (e)

                    this.axVSPrinter1.CurrentY = 10622;
                    this.axVSPrinter1.MarginLeft = 9500;
                    this.axVSPrinter1.Table = string.Format("<3000;{0}", m_sBIN);
                    this.axVSPrinter1.CurrentY = 11022;
                    // RMC 20120125 modified printing of permit with addl payment (s)
                    if (m_sAddlOr.Trim() != "")
                        sORNo += "/ " + m_sAddlOr;
                    if (m_sAddlOrDate.Trim() != "")
                        sORDate += "/ " + m_sAddlOrDate;
                    // RMC 20120125 modified printing of permit with addl payment (e)
                    this.axVSPrinter1.Table = string.Format("<3000;{0}", sORNo);
                    this.axVSPrinter1.CurrentY = 11422;
                    this.axVSPrinter1.Table = string.Format("<3000;{0}", sORDate);
                }
            }


            if (AuditTrail.InsertTrail("ABBP", "rep_mp", "Print Permit of " + m_sBIN + " for tax year " + m_sTaxYear) == 0) // RMC 20111219 Added tax year in trail of permit printing
            {
                pSet.Rollback();
                pSet.Close();
                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void CreateHeader()
        {
            /* string strProvinceName = string.Empty;

             //this.axVSPrinter1
            
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
             this.axVSPrinter1.FontName = "Arial Narrow";
             this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
             this.axVSPrinter1.FontBold = true;
             this.axVSPrinter1.FontSize = (float)10.0;
             this.axVSPrinter1.Table = "^9000;Republic of the Philippines";
             this.axVSPrinter1.FontBold = false;

             strProvinceName = AppSettingsManager.GetConfigValue("08");
             if (strProvinceName != string.Empty)
                 this.axVSPrinter1.Table = string.Format("^9000;{0}", strProvinceName);
             this.axVSPrinter1.Table = string.Format("^9000;{0}", AppSettingsManager.GetConfigValue("09"));
             this.axVSPrinter1.Table = string.Format("^9000;{0}", AppSettingsManager.GetConfigValue("41"));
             this.axVSPrinter1.Paragraph = "";
             this.axVSPrinter1.Paragraph = "";
             this.axVSPrinter1.Paragraph = "";*/
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            /* Image newImage = Image.FromFile("D:\\bgbpls_Calamba.bmp");
             Point ulCorner = new Point(100,100);

             axVSPrinter1.DrawPicture(newImage,0,0);*/
            /*axVSPrinter1.X1 = 1000;
            axVSPrinter1.Y1 = 1000;
            axVSPrinter1.X2 = 2400;
            axVSPrinter1.Y2 = 2400;
            axVSPrinter1.Picture = newImage;*/

            /*CreateHeader();

            if (m_sReportSwitch == "Violation")
                axVSPrinter1.Footer = "Note: Please disregard this letter if you already have a Business Permit issued by the " + AppSettingsManager.GetConfigValue("02") + " Business Permits & Licensing Office";
            else
                axVSPrinter1.Footer = AppSettingsManager.GetConfigValue("16");*/
        }

        private void axVSPrinter1_BeforeFooter(object sender, EventArgs e)
        {
            this.axVSPrinter1.HdrFontName = "Arial Narrow";
            this.axVSPrinter1.HdrFontSize = (float)8.0;
            this.axVSPrinter1.HdrFontItalic = true;
        }

        private void axVSPrinter1_StartDocEvent(object sender, EventArgs e)
        {

        }

        private void PrintCertificate()
        {
        }

        public bool GetNotation(string sORNo)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sMinYear = "";
            string sMaxYear = "";
            m_sNotation = "";

            pRec.Query = string.Format("select min(tax_year), max(tax_year) from pay_hist where or_no = '{0}' and tax_year <> '{1}' and bin = '{2}' ", sORNo, m_sTaxYear, m_sBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sMinYear = pRec.GetString(0);
                    sMaxYear = pRec.GetString(1);

                    pRec.Close();

                    if (sMinYear != sMaxYear)
                        m_sNotation = "Note: With payment for delinquent years: " + sMinYear + "-" + sMaxYear;
                    else if (sMinYear != "")
                        m_sNotation = "Note: With payment for delinquent year: " + sMinYear;
                    else
                        m_sNotation = "";

                }
            }
            pRec.Close();



            if (m_sNotation != "")
                return true;

            return false;
        }

        private string GetFeeAmount(string sORNo, string sFeeCode, string sBnsCode, string sTaxYear)
        {
            OracleResultSet pRec = new OracleResultSet();
            double dFeeAmt = 0;
            string sFeeAmt = "";

            pRec.Query = string.Format("select fees_due from or_table where or_no = '{0}' and fees_code = '{1}' and bns_code_main = '{2}' and tax_year = '{3}'", sORNo, sFeeCode, sBnsCode, sTaxYear);
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    dFeeAmt += pRec.GetDouble(0);
                }
                sFeeAmt = string.Format("{0:#,##0.00}", dFeeAmt);
            }
            pRec.Close();

            // RMC 20120125 modified printing of permit with addl payment (s)
            /*if (m_sAddlOr.Trim() != "")
            {
                pRec.Query = string.Format("select fees_due from or_table where or_no = '{0}' and fees_code = '{1}' and bns_code_main = '{2}' and tax_year = '{3}'", m_sAddlOr, sFeeCode, sBnsCode, sTaxYear);
                if (pRec.Execute())
                {
                    while (pRec.Read())
                    {
                        dFeeAmt += pRec.GetDouble(0);
                    }
                    sFeeAmt = string.Format("{0:#,##0.00}", dFeeAmt);
                }
                pRec.Close();
            }
            // RMC 20120125 modified printing of permit with addl payment (e)
             */
            return sFeeAmt;

        }

        private string GetArea(string sBIN, string sBnsCode, string sTaxYear)
        {
            // RMC 20120103 Modified getting of area in printing Permit
            OracleResultSet pCode = new OracleResultSet();
            OracleResultSet pArea = new OracleResultSet();
            string sDefaultCode = "";
            string sArea = "";
            double dArea = 0;

            pCode.Query = "select * from default_code where default_desc like 'AREA IN%'";
            if (pCode.Execute())
            {
                while (pCode.Read())
                {
                    sDefaultCode = pCode.GetString("default_code");

                    pArea.Query = "select * from other_info where default_code = '" + sDefaultCode + "'";
                    pArea.Query += " and bin = '" + sBIN + "' and bns_code = '" + sBnsCode + "'";
                    pArea.Query += " and tax_year = '" + sTaxYear + "'";
                    if (pArea.Execute())
                    {
                        if (pArea.Read())
                        {
                            dArea = pArea.GetDouble("data");
                        }
                    }
                    pArea.Close();
                }
            }
            pCode.Close();

            sArea = string.Format("{0:##0.00}", dArea);

            return sArea;
        }

        private string GetUnit(string sBIN, string sBnsCode, string sTaxYear, string sReturnValue)
        {
            // RMC 20120105 Modified getting of unit in printing Permit
            OracleResultSet pCode = new OracleResultSet();
            OracleResultSet pArea = new OracleResultSet();
            string sDefaultCode = "";
            string sUnit = "";
            double dUnit = 0;

            if (sReturnValue == "UNIT")
                pCode.Query = "select * from default_code where default_desc like 'NUMBER OF APARTMENT UNIT%'";
            else
                pCode.Query = "select * from default_code where default_desc like 'NUMBER OF ROOM%'";
            if (pCode.Execute())
            {
                while (pCode.Read())
                {
                    sDefaultCode = pCode.GetString("default_code");

                    pArea.Query = "select * from other_info where default_code = '" + sDefaultCode + "'";
                    pArea.Query += " and bin = '" + sBIN + "' and bns_code = '" + sBnsCode + "'";
                    pArea.Query += " and tax_year = '" + sTaxYear + "'";
                    if (pArea.Execute())
                    {
                        if (pArea.Read())
                        {
                            dUnit = pArea.GetDouble("data");
                        }
                    }
                    pArea.Close();
                }
            }
            pCode.Close();

            sUnit = string.Format("{0:##0}", dUnit);

            return sUnit;
        }

        private bool BnsIsExempted(string p_sBnsCode)
        {
            // RMC 20120111 added display of other line of businesses with no paid tax if exempted in Business permit printing
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from exempted_bns where bns_code = '" + p_sBnsCode + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();
                    return true;
                }
                else
                {
                    pSet.Close();
                    return false;
                }
            }
            return true;
        }

        private string GetExemption(string sBnsType)
        {
            // RMC 20120116 added display of exemption (peza/boi) in permit
            OracleResultSet pExempt = new OracleResultSet();
            string sBnsNewType = "";

            // RMC 20140110 modified permit if business under incentives - Mati (s)
            if (AppSettingsManager.GetConfigValue("10") == "232")
            {
                pExempt.Query = "select * from boi_table where bin = '" + m_sBIN + "'";
                if (pExempt.Execute())
                {
                    string sYearFr = string.Empty;
                    string sYearTo = string.Empty;
                    int iYearFr = 0;
                    int iYearTo = 0;
                    int iCurrYear = 0;
                    int.TryParse(m_sTaxYear, out iCurrYear);

                    if (pExempt.Read())
                    {
                        sYearFr = string.Format("{0:yyyy}", pExempt.GetDateTime("datefrom"));
                        sYearTo = string.Format("{0:yyyy}", pExempt.GetDateTime("dateto"));

                        int.TryParse(sYearFr, out iYearFr);
                        int.TryParse(sYearTo, out iYearTo);
                        //if ((iYearFr <= iCurrYear) && (iCurrYear >= iYearTo))
                        if ((iYearFr <= iCurrYear) && (iYearTo >= iCurrYear))   // RMC 20140124 adjustment in printing exemption in Permit
                        {
                            sYearFr = string.Format("{0:MMM dd, yyyy}", pExempt.GetDateTime("datefrom"));
                            sYearTo = string.Format("{0:MMM dd, yyyy}", pExempt.GetDateTime("dateto"));

                            sBnsNewType = AppSettingsManager.GetConfigValue("50") + "  ";
                            sBnsNewType += sYearFr + "-" + sYearTo;
                        }
                    }
                }
            }
            else
            {
                // RMC 20140110 modified permit if business under incentives - Mati (e)


                sBnsNewType = sBnsType;

                pExempt.Query = "select * from boi_table where bin = '" + m_sBIN + "'";
                if (pExempt.Execute())
                {
                    if (pExempt.Read())
                    {

                        sBnsNewType += " - " + pExempt.GetString("exempt_type");
                    }
                }
                pExempt.Close();
            }

            return sBnsNewType;

        }

        //private string ConsolidatedQtr(string sOrNo, string sQtrPaid)
        private string ConsolidatedQtr()    // RMC 20120413 Corrections in printing Permit
        {
            // RMC 20120120 added order by qtr_paid in permit printing
            OracleResultSet pConsoQtr = new OracleResultSet();
            string sMinQtr = "";
            string sMaxQtr = "";

            /*pConsoQtr.Query = "select min(qtr_paid), max(qtr_paid) from pay_hist ";
            pConsoQtr.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "'";
            pConsoQtr.Query += " and or_no = '" + sOrNo + "'";
            if (pConsoQtr.Execute())
            {
                if (pConsoQtr.Read())
                {
                    sMinQtr = pConsoQtr.GetString(0);
                    sMaxQtr = pConsoQtr.GetString(1);

                    // RMC 20120125 (S)
                    if (sMaxQtr != "F" && sMaxQtr != "P" && sMaxQtr != "" && sMinQtr != "")
                    {
                        int iTmpQtr = 0;
                        int.TryParse(sMaxQtr, out iTmpQtr);
                        if (iTmpQtr == 0)
                        {
                            sMaxQtr = sMinQtr;
                            sQtrPaid = sMinQtr;
                        }
                    }
                    // RMC 20120125 (E)

                    if (sMinQtr == "" || sMinQtr == sMaxQtr)
                    {
                        if (sMinQtr == "F" || sMinQtr == "P")
                            sMinQtr = "Annually";
                        else
                        {
                            sMinQtr = sQtrPaid;
                            if (sMinQtr == "1")
                                sMinQtr = "1st Qtr";
                            else if (sMinQtr == "2")
                                sMinQtr = "2nd Qtr";
                            else if (sMinQtr == "3")
                                sMinQtr = "3rd Qtr";
                            else if (sMinQtr == "4")
                                sMinQtr = "4th Qtr";
                            else if(sMinQtr == "A")
                                sMinQtr = "Annually";
                            else
                                sMinQtr = "";
                        }
                    }
                    else
                    {
                        if (sMaxQtr == "F" || sMaxQtr == "P")
                            sMaxQtr = "Annually";
                        else
                        {
                            if (sMaxQtr == "1")
                                sMaxQtr = "1st Qtr";
                            else if (sMaxQtr == "2")
                                sMaxQtr = "2nd Qtr";
                            else if (sMaxQtr == "3")
                                sMaxQtr = "3rd Qtr";
                            else if (sMaxQtr == "4")
                                sMaxQtr = "4th Qtr";
                            else if(sMaxQtr == "A")
                                sMaxQtr = "Annually";
                            else
                                sMaxQtr = "";
                        }

                        sMinQtr += "-" + sMaxQtr;
                    }
                }
                else
                    sMinQtr = sQtrPaid;
            }
            pConsoQtr.Close();
            sMinQtr = sMinQtr;
            return = sMinQtr;
            
             */

            // RMC 20120413 Corrections in printing Permit (s)
            string sQtrPaid = "";
            string sTmpTaxYear = "";
            string sMinQtrPaid = "";
            string sMaxQtrPaid = "";
            string sQtrsPaid = "";
            m_bWithBalQtr = false;
            m_sMaxQtrPaid = "";

            pConsoQtr.Query = "select * from or_table where or_no in ";
            pConsoQtr.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "') ";
            //pConsoQtr.Query += " and fees_code = 'B' order by qtr_paid";    // RMC 20140116 adjustment in Permit printing
            pConsoQtr.Query += " and (fees_code = 'B' or fees_code = '01') order by qtr_paid";  // RMC 20140127 consider exempted businesses in permit printing
            if (pConsoQtr.Execute())
            {
                while (pConsoQtr.Read())
                {
                    sTmpTaxYear = pConsoQtr.GetString("tax_year");

                    if (sTmpTaxYear == m_sTaxYear)
                    {
                        sQtrPaid = pConsoQtr.GetString("qtr_paid");

                        if (sQtrPaid != "A" && sQtrPaid != "P" && sQtrPaid != "Y")
                        {
                            if (sMinQtrPaid == "")
                                sMinQtrPaid = sQtrPaid;

                            sMaxQtrPaid = sQtrPaid;
                        }
                    }
                }
            }
            pConsoQtr.Close();

            if (sMinQtrPaid == "F")
            {
                //sQtrsPaid = "Annually";
                sQtrsPaid = "Annual";   // RMC 20140114 edited "Annually" to "Annual" in permit
                m_bWithBalQtr = false;
            }
            else
            {
                if (sMaxQtrPaid == "1")
                {
                    sQtrsPaid = "1st Qtr";
                    m_bWithBalQtr = true;
                    m_sMaxQtrPaid = sQtrsPaid;
                }
                else if (sMaxQtrPaid == "2")
                {
                    sQtrsPaid = "2nd Qtr";
                    m_bWithBalQtr = true;
                    m_sMaxQtrPaid = sQtrsPaid;
                }
                else if (sMaxQtrPaid == "3")
                {
                    sQtrsPaid = "3rd Qtr";
                    m_bWithBalQtr = true;
                    m_sMaxQtrPaid = sQtrsPaid;
                }
                else
                {
                    sQtrsPaid = "4th Qtr";
                    m_bWithBalQtr = false;
                }

                if (sMinQtrPaid == sMaxQtrPaid)
                    sQtrsPaid = sQtrsPaid;
                else
                {
                    sQtrsPaid = sMinQtrPaid + "-" + sQtrsPaid;
                }

                // RMC 20140116 adjustment in Permit printing (s)
                if (sQtrsPaid == "1-4th Qtr")
                {
                    sQtrsPaid = "Annual";
                    m_bWithBalQtr = false;
                }   // RMC 20140116 adjustment in Permit printing (e)
                else
                    sQtrsPaid = "Quarterly"; // GDE 20130618 - disregard qtrs
            }
            // RMC 20120413 Corrections in printing Permit (e)

            return sQtrsPaid;
        }

        private string GetFeeTerm(string sFeesCode)
        {
            // RMC 20120120 added order by qtr_paid in permit printing
            OracleResultSet pFeeTerm = new OracleResultSet();
            string sFeeTerm = "";

            pFeeTerm.Query = "select * from tax_and_fees_table where fees_code = '" + sFeesCode + "'";
            pFeeTerm.Query += " and rev_year = '" + ConfigurationAttributes.RevYear + "'";
            if (pFeeTerm.Execute())
            {
                if (pFeeTerm.Read())
                {
                    sFeeTerm = pFeeTerm.GetString("fees_term");
                }
            }
            pFeeTerm.Close();

            return sFeeTerm;
        }

        private string GetAddlOr(string sORNo, string sORDate)
        {
            // RMC 20120125 modified printing of permit with addl payment

            OracleResultSet pAddlOr = new OracleResultSet();
            string sAddlOr = "";
            m_sAddlOrDate = "";

            pAddlOr.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
            pAddlOr.Query += " and (qtr_paid = 'A' or qtr_paid = 'P')";
            pAddlOr.Query += " and or_no <> '" + sORNo + "'";
            pAddlOr.Query += " order by or_date desc, qtr_paid desc";
            if (pAddlOr.Execute())
            {
                //    if (pAddlOr.Read())
                int iCnt = 0;   // RMC 20120413 Corrections in printing Permit
                while (pAddlOr.Read())  // RMC 20120413 Corrections in printing Permit
                {
                    // RMC 20120413 Corrections in printing Permit (s)
                    if (iCnt > 1)
                    {
                        sAddlOr += " or ";
                        if (m_sAddlOrDate != "")
                            m_sAddlOrDate += "/ ";
                    }
                    sAddlOr += pAddlOr.GetString("or_no");
                    m_sAddlOrDate += string.Format("{0:MMMM dd, yyyy}", pAddlOr.GetDateTime("or_date"));
                    // RMC 20120413 Corrections in printing Permit (e)

                    /*sAddlOr = pAddlOr.GetString("or_no");
                    m_sAddlOrDate = string.Format("{0:MMMM dd, yyyy}", pAddlOr.GetDateTime("or_date"));*/

                    if (sORDate == m_sAddlOrDate)
                        m_sAddlOrDate = "";

                    // RMC 20120413 Corrections in printing Permit
                    iCnt++;
                }
            }
            pAddlOr.Close();

            return sAddlOr;
        }

        private string GetOldBnsType(string sBnsCode)
        {
            // RMC 20120228 modified printing of permit with change of line of business and payment adjustment
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet(); //MCR ADD 20140415
            string sOldBnsCode = "";
            string sOrNo = "";

            pSet.Query = "select * from permit_update_appl where bin = '" + m_sBIN + "'";
            pSet.Query += " and tax_year = '" + m_sTaxYear + "' and appl_type = 'CTYP'";
            pSet.Query += " and new_bns_code = '" + sBnsCode + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sOldBnsCode = pSet.GetString("old_bns_code");
                    sOrNo = pSet.GetString("or_no");

                    if (sOrNo == "VOID")
                        m_bPermitUpdateNoBilling = true;

                    m_sRemarks += "\n ** With amendment from " + AppSettingsManager.GetBnsDesc(sOldBnsCode) + " to " + AppSettingsManager.GetBnsDesc(sBnsCode);
                }
            }
            pSet.Close();

            pSet.Query = "select distinct * from pay_hist where bin = '" + m_sBIN + "'";
            pSet.Query += " and tax_year = '" + m_sTaxYear + "'"; //and qtr_paid = 'A'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    //MCR ADD 20140415 get the OldBnsCode(s)
                    pSet1.Query = "select * from or_table where or_no = '" + pSet.GetString("or_no") + "'";
                    if (pSet1.Execute())
                    {
                        if (pSet1.Read())
                        {
                            sOldBnsCode = pSet1.GetString("bns_code_main");
                        }
                    }
                    pSet1.Close();
                    //MCR ADD 20140415 get the OldBnsCode(e)
                }
                else
                    sOldBnsCode = "";
            }
            pSet.Close();

            return sOldBnsCode;

        }

        private double GetTaxCreditAmt()
        {
            // RMC 20120228 modified printing of permit with change of line of business and payment adjustment

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            string sOrNo = "";
            double dCredit = 0;
            m_dRemainingCredit = 0;
            m_dAppliedCredit = 0; // GDE 20120424 re-allocate here

            pSet.Query = "select distinct(or_no) as or_no from pay_hist where bin = '" + m_sBIN + "'";
            //pSet.Query += " and tax_year = '" + m_sTaxYear + "' and qtr_paid = 'A'";
            pSet.Query += " and tax_year = '" + m_sTaxYear + "'";   // RMC 20120419 added display of tax credit used in Permit
            if (pSet.Execute())
            {
                //if (pSet.Read())
                while (pSet.Read())
                {
                    sOrNo = pSet.GetString("or_no");

                    pSet2.Query = "select sum(balance) from dbcr_memo where bin = '" + m_sBIN + "'"; // RMC 20120419 added display of tax credit used in Permit
                    pSet2.Query += " and or_no = '" + sOrNo + "' and served = 'N' and multi_pay = 'N'";
                    if (pSet2.Execute())
                    {
                        if (pSet2.Read())
                        {
                            m_dRemainingCredit = pSet2.GetDouble(0);
                            //dCredit += m_dRemainingCredit; // GDE 20120424
                            dCredit = m_dRemainingCredit;
                        }
                    }
                    pSet2.Close();

                    GetAppliedTaxCredit(sOrNo);
                }
            }
            pSet.Close();

            //pSet.Query = "select sum(credit) from dbcr_memo where bin = '" + m_sBIN + "'";
            /*pSet.Query = "select sum(balance) from dbcr_memo where bin = '" + m_sBIN + "'"; // RMC 20120419 added display of tax credit used in Permit
            pSet.Query += " and or_no = '" + sOrNo + "' and served = 'N'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_dRemainingCredit = pSet.GetDouble(0);
                    dCredit += m_dRemainingCredit;
                }
            }
            pSet.Close();

            GetAppliedTaxCredit(sOrNo);*/
            return dCredit;
        }

        private void GetAppliedTaxCredit(string sOrNo)
        {
            OracleResultSet pSet = new OracleResultSet();

            //            m_dAppliedCredit = 0; // GDE 20120424 re-allocate

            pSet.Query = "select sum(debit) from dbcr_memo where bin = '" + m_sBIN + "'";
            pSet.Query += " and or_no = '" + sOrNo + "' and served = 'Y'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_dAppliedCredit += pSet.GetDouble(0);

                }
            }
            pSet.Close();


        }

        private void PrintPermitModified()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            string sBnsNm = "";
            string sOwnNm = "";
            string sBnsAddress = "";
            string sBnsMainDesc = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sORNo = "";
            string sOrgnKind = "";
            string sBnsStat = "";
            string sTelNo = "";
            string sDay = "";
            string sCapital = "";
            string sGross = "";
            string sFeeAmt = "";
            string sRemarks = "";
            string sFlrArea = "";
            string sUnitNo = "";
            string sRoomNo = "";
            string sQtrRemarks = "";
            string sORDate = "";
            string sQtrPaid = "";
            string sPlate = "";
            string sCitizenship = "";
            string sQtrsPaid = "";
            string sMemo = "";
            DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate;
            string sOldBnsCode = "";

            m_bPermitUpdateNoBilling = false;

            pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
            pSet.Query += " and (qtr_paid <> 'A' and qtr_paid <> 'P')";
            //pSet.Query += " order by or_date desc, qtr_paid desc";   
            pSet.Query += " order by or_date asc, qtr_paid asc";    // RMC 20120413 Corrections in printing Permit
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sQtrPaid = pSet.GetString("qtr_paid");
                    sBnsNm = GetDTIBnsName(m_sBIN); // RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ 

                    if (sBnsNm == "")    // RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ 
                        sBnsNm = AppSettingsManager.GetBnsName(m_sBIN);
                    sOwnNm = AppSettingsManager.GetBnsOwnCode(m_sBIN);
                    sOwnNm = AppSettingsManager.GetBnsOwner(sOwnNm);
                    sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
                    sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);   //gets new bns type upon permit update appl change of bns type

                    sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
                    sBnsMainDesc = AppSettingsManager.GetSubBnsDesc(sBnsCode.Substring(0, 2), "B");
                    sBnsMainDesc = GetExemption(sBnsMainDesc);
                    sORNo = pSet.GetString("or_no");


                    dtpORDate = pSet.GetDateTime("or_date");
                    sORDate = string.Format("{0:MMMM dd, yyyy}", dtpORDate);
                    m_sAddlOr = GetAddlOr(sORNo, sORDate);

                    GetOtherBusinessInfo(sBnsCode);

                    string strData = "";

                    this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
                    //this.axVSPrinter1.CurrentY = 800;
                    this.axVSPrinter1.CurrentY = 900;
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontName = "Arial Narrow";
                    this.axVSPrinter1.FontSize = (float)10.0;
                    strData = "|" + m_sPermitNo + "||" + m_sPlate;
                    this.axVSPrinter1.Table = string.Format("<150|<1850|^5800|>2000;{0}", strData);
                    this.axVSPrinter1.FontBold = false;

                    this.axVSPrinter1.MarginLeft = 3000;
                    this.axVSPrinter1.CurrentY = 3499;
                    //this.axVSPrinter1.CurrentY = 3599;
                    this.axVSPrinter1.Table = string.Format("<10000;{0}", sBnsNm);
                    this.axVSPrinter1.Table = string.Format("<10000;{0}", sBnsAddress);
                    this.axVSPrinter1.Table = string.Format("<10000;{0}", sOwnNm);

                    if (sOrgnKind == "SINGLE PROPRIETORSHIP")
                        sOrgnKind = "SINGLE PROP.";

                    strData = m_sTelNo + "||" + m_sOrgnKind + "||" + m_sCitizenship;
                    this.axVSPrinter1.Table = string.Format("<3000|<1000|<2000|<1000|<2000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 5150;
                    this.axVSPrinter1.MarginLeft = 1200;

                    double dFeeAmt = 0;
                    string sFeesCode = "";

                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.CurrentY = 5849;

                    this.axVSPrinter1.FontSize = (float)8.0;
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>100|<2000|>500|<1000;" + sBnsMainDesc + "|CAPITAL INVESTMENT:||" + m_sCapital + "||" + m_sBnsStat);
                    this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|<2000|>1000|<1000;" + "||GROSS SALES:||" + m_sGross + "|||");

                    //PAYMENT DETAILS
                    //{ ** line of business - buss tax - permit fee
                    int iBnsCount = 0;
                    iBnsCount = m_sArrayBnsCode.Length;
                    string sTmpBnsCode = "";
                    string sTmpOldBnsCode = "";
                    string sFeeCode = "";
                    string sTmpOrNo = "";

                    for (int ix = 1; ix <= iBnsCount; ix++)
                    {
                        sTmpBnsCode = m_sArrayBnsCode.GetValue(ix - 1).ToString();
                        sTmpOldBnsCode = GetOldBnsType(sTmpBnsCode);
                        sBnsDesc = AppSettingsManager.GetBnsDesc(sTmpBnsCode);

                        if (sTmpOldBnsCode != "")
                            sTmpOrNo = GetOrNo(sTmpOldBnsCode);
                        else
                            sTmpOrNo = GetOrNo(sTmpBnsCode);

                        //sQtrsPaid = ConsolidatedQtr(sTmpOrNo, sQtrPaid);  // RMC 20120413 Corrections in printing Permit

                        if (m_bPermitUpdateNoBilling == true && sTmpOldBnsCode != "")
                        {
                            sTmpBnsCode = sTmpOldBnsCode;
                            sTmpOldBnsCode = "";
                        }

                        for (int i = 1; i <= 2; i++)
                        {
                            if (i == 1)
                                sFeesCode = "B"; // business tax
                            else
                                sFeesCode = GetPermitFeeCode();  // permit fee

                            if (sTmpOldBnsCode != "")
                                dFeeAmt = GetFeeAdjustment(sFeesCode, sTmpBnsCode, m_sTaxYear, GetQtrPaid(sORNo, m_sTaxYear));
                            else
                                dFeeAmt = Convert.ToDouble(GetFeeAmount(sORNo, sFeesCode, sTmpBnsCode, m_sTaxYear));

                            if (m_sAddlOr.Trim() != "")
                            {
                                if (sTmpOldBnsCode != "")
                                    dFeeAmt += Convert.ToDouble(GetFeeAmount(m_sAddlOr, sFeesCode, sTmpOldBnsCode, m_sTaxYear));
                                else
                                    dFeeAmt += Convert.ToDouble(GetFeeAmount(m_sAddlOr, sFeesCode, sTmpBnsCode, m_sTaxYear));
                            }

                            sFeeAmt = string.Format("{0:#,##0.00}", dFeeAmt);
                            sFeeCode = AppSettingsManager.GetFeesDesc(sFeesCode);

                            if (i == 1)
                                this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + "|BUSINESS TAX:||" + sFeeAmt + "|" + sQtrsPaid);
                            else
                                this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "|" + sQtrsPaid);
                        }
                    }
                    long lngY1 = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
                    //** line of business - buss tax - permit fee }

                    if (sQtrPaid == "F")
                        sQtrPaid = "Annually";
                    else
                    {
                        if (sQtrPaid == "1")
                            sQtrPaid = "1st Qtr";
                        else if (sQtrPaid == "2")
                            sQtrPaid = "2nd Qtr";
                        else if (sQtrPaid == "3")
                            sQtrPaid = "3rd Qtr";
                        else
                            sQtrPaid = "4th Qtr";
                    }

                    if (sQtrPaid != "Annually" && sQtrPaid != "4th Qtr")    // RMC 20120120 added order by qtr_paid in permit printing   
                    {
                        this.axVSPrinter1.CurrentY = 6938;
                        this.axVSPrinter1.MarginLeft = 8600;
                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.FontSize = (float)7.0;
                        sQtrRemarks = "BALANCE TO BE ";
                        this.axVSPrinter1.Table = string.Format(">3000;{0}", sQtrRemarks);
                        sQtrRemarks = "PAID ON OR BEFORE:";
                        this.axVSPrinter1.Table = string.Format(">3000;{0}", sQtrRemarks);

                        DateTime dtDate;
                        string sDate = "";
                        PrintPaymentDueRemarks();
                        this.axVSPrinter1.MarginLeft = 2000;
                    }

                    this.axVSPrinter1.MarginLeft = 1200;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.FontSize = (float)8.0;

                    this.axVSPrinter1.CurrentY = lngY1;
                    int intCnt = 0;

                    if (m_sAddlOr.Trim() != "") // RMC 20120125 modified printing of permit with addl payment
                        pRec.Query = string.Format("select sum(fees_due), fees_code, qtr_paid from or_table where (or_no = '{0}' or or_no = '{1}') and fees_code <> 'B' and fees_code <> '{2}' and tax_year = '{3}' group by fees_code, qtr_paid order by fees_code", sORNo, m_sAddlOr, sFeesCode, m_sTaxYear);
                    else
                        pRec.Query = string.Format("select sum(fees_due), fees_code, qtr_paid from or_table where or_no = '{0}' and fees_code <> 'B' and fees_code <> '{1}' and tax_year = '{2}' group by fees_code, qtr_paid order by fees_code", sORNo, sFeesCode, m_sTaxYear);  // RMC 20110822  // RMC 20120120 added order by qtr_paid in permit printing
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            string sTmpQtr = "";
                            sQtrPaid = pRec.GetString(2);
                            //sQtrsPaid = ConsolidatedQtr(sORNo, sQtrPaid); // RMC 20120413 Corrections in printing Permit

                            sTmpQtr = sQtrsPaid;

                            intCnt++;
                            if (intCnt > 2)
                                sQtrPaid = "";

                            sFeeCode = pRec.GetString("fees_code");
                            // RMC 20120120 added order by qtr_paid in permit printing (s)
                            if (GetFeeTerm(sFeeCode) == "F")
                                sTmpQtr = "";
                            else
                                sTmpQtr = sQtrsPaid;
                            // RMC 20120120 added order by qtr_paid in permit printing (e)

                            sFeeCode = AppSettingsManager.GetFeesDesc(sFeeCode);

                            //sFeeAmt = string.Format("{0:#,##0.00}", pRec.GetDouble("fees_due"));
                            sFeeAmt = string.Format("{0:#,##0.00}", pRec.GetDouble(0)); // RMC 20120120

                            //this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "|" + sQtrPaid);  // RMC 20110810 adjusted
                            this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "|" + sTmpQtr);  // RMC 20120120
                        }
                    }
                    pRec.Close();

                    string sSurch = "";
                    string sPen = "";
                    string sTotal = "";
                    string sCredit = "";

                    double dblSurch = 0;
                    double dblPen = 0;
                    double dblTotal = 0;
                    double dblCredit = 0;

                    if (m_sAddlOr.Trim() != "")
                        pRec.Query = string.Format("select sum(fees_surch) from or_table where (or_no = '{0}' or or_no = '{1}') and tax_year = '{2}'", sORNo, m_sAddlOr, m_sTaxYear);
                    else
                        pRec.Query = string.Format("select sum(fees_surch) from or_table where or_no = '{0}' and tax_year = '{1}'", sORNo, m_sTaxYear);
                    double.TryParse(pRec.ExecuteScalar(), out dblSurch);
                    sSurch = string.Format("{0:#,##0.00}", dblSurch);

                    // RMC 20120315 added option to print or not penalty and surcharge in Business Permit (s)
                    /*   bool bPrintPenSur = true;

                       if (dblSurch != 0)
                       {
                           if (MessageBox.Show("Print penalty and surcharge?", "Business Permits", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                           {
                               bPrintPenSur = false;
                           }
                       }

                       if (!bPrintPenSur)
                           sSurch = "0.00";
                       // RMC 20120315 added option to print or not penalty and surcharge in Business Permit (e)
                   */
                    // RMC 20120411 Disable RMC 20120315 mods by Jester's meeting with the BPLS

                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + m_sFlrArea + "|25% Penalty||" + sSurch + "|");

                    if (m_sAddlOr.Trim() != "")
                        pRec.Query = string.Format("select sum(fees_pen) from or_table where (or_no = '{0}' or or_no = '{1}') and tax_year = '{2}'", sORNo, m_sAddlOr, m_sTaxYear);
                    else
                        pRec.Query = string.Format("select sum(fees_pen) from or_table where or_no = '{0}' and tax_year = '{1}'", sORNo, m_sTaxYear);
                    double.TryParse(pRec.ExecuteScalar(), out dblPen);
                    sPen = string.Format("{0:#,##0.00}", dblPen);

                    // RMC 20120315 added option to print or not penalty and surcharge in Business Permit (s)
                    /*  if (!bPrintPenSur)
                          sPen = "0.00";
                      // RMC 20120315 added option to print or not penalty and surcharge in Business Permit (e)
                    */
                    // RMC 20120411 Disable RMC 20120315 mods by Jester's meeting with the BPLS

                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + m_sUnitNo + "|2% Interest||" + sPen + "|");

                    // tax credit (note: value for clarification )
                    /* pRec.Query = string.Format("select sum(credit) from debit_credit where bin = '{0}' and or_no = '{0}'", m_sBIN, sORNo);
                     double.TryParse(pRec.ExecuteScalar(), out dblCredit);*/
                    dblCredit = GetTaxCreditAmt();
                    sCredit = string.Format("{0:#,##0.00}", dblCredit);
                    //this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + sRemarks + "|Tax Credits||" + sCredit + "|");
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + m_sRoomNo + "|Tax Credits||" + sCredit + "|");    // RMC 20120105 Modified getting of unit in printing Permit

                    if (m_sAddlOr.Trim() != "") // RMC 20120125 modified printing of permit with addl payment
                        pRec.Query = string.Format("select sum(fees_amtdue) from or_table where (or_no = '{0}' or or_no = '{1}') and tax_year = '{2}'", sORNo, m_sAddlOr, m_sTaxYear);
                    else
                        pRec.Query = string.Format("select sum(fees_amtdue) from or_table where or_no = '{0}' and tax_year = '{1}'", sORNo, m_sTaxYear);
                    double.TryParse(pRec.ExecuteScalar(), out dblTotal);

                    // RMC 20120315 added option to print or not penalty and surcharge in Business Permit (s)
                    /*  if (!bPrintPenSur)
                      {
                          dblTotal -= dblPen + dblSurch;
                      }
                      // RMC 20120315 added option to print or not penalty and surcharge in Business Permit (e)
                     */
                    // RMC 20120411 Disable RMC 20120315 mods by Jester's meeting with the BPLS

                    dblTotal -= m_dAppliedCredit;  // RMC 20120228 modified printing of permit with change of line of business and payment adjustment

                    sTotal = string.Format("{0:#,##0.00}", dblTotal);

                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontBold = true;
                    //this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|TOTAL TAX PAID:||" + sTotal + "|");
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + m_sRemarks + "|TOTAL TAX PAID:||" + sTotal + "|");   // RMC 20120105 Modified getting of unit in printing Permit
                    this.axVSPrinter1.FontBold = false;

                    if (GetNotation(sORNo))
                    {
                        this.axVSPrinter1.CurrentY = 9222;
                        this.axVSPrinter1.MarginLeft = 2000;
                        this.axVSPrinter1.Table = string.Format("<10000;{0}", m_sNotation);
                    }

                    // RMC 20120124 added printing of remarks in Permit (user-request) (s)
                    if (sMemo.Trim() != "") // RMC 20120125
                    {
                        if (MessageBox.Show("Print Remarks?", "Business Permits", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.axVSPrinter1.CurrentY = 9622;
                            this.axVSPrinter1.MarginLeft = 2000;
                            this.axVSPrinter1.Table = string.Format("<10000;{0}", sMemo);
                        }
                    }
                    // RMC 20120124 added printing of remarks in Permit (user-request) (e)



                    this.axVSPrinter1.CurrentY = 10622;
                    this.axVSPrinter1.MarginLeft = 9500;
                    this.axVSPrinter1.Table = string.Format("<3000;{0}", m_sBIN);
                    this.axVSPrinter1.CurrentY = 11022;
                    // RMC 20120125 modified printing of permit with addl payment (s)
                    if (m_sAddlOr.Trim() != "")
                        sORNo += "/ " + m_sAddlOr;
                    if (m_sAddlOrDate.Trim() != "")
                        sORDate += "/ " + m_sAddlOrDate;
                    // RMC 20120125 modified printing of permit with addl payment (e)
                    this.axVSPrinter1.Table = string.Format("<3000;{0}", sORNo);
                    this.axVSPrinter1.CurrentY = 11422;
                    this.axVSPrinter1.Table = string.Format("<3000;{0}", sORDate);
                }
            }


            if (AuditTrail.InsertTrail("ABBP", "rep_mp", "Print Permit of " + m_sBIN + " for tax year " + m_sTaxYear) == 0) // RMC 20111219 Added tax year in trail of permit printing
            {
                pSet.Rollback();
                pSet.Close();
                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void PrintPaymentDueRemarks()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sQtrRemarks = "";
            string sDate = "";
            string sQtrPaid = "";
            DateTime dtDate;

            sQtrPaid = m_sMaxQtrPaid;

            pRec.Query = "select * from due_dates where due_year = '" + m_sTaxYear + "' order by due_code ";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    if (pRec.GetString("due_code") == "04" ||
                        pRec.GetString("due_code") == "07" ||
                        pRec.GetString("due_code") == "10")
                    {
                        dtDate = pRec.GetDateTime("due_date");
                        sDate = string.Format("{0:MMMM dd,yyyy}", dtDate);
                        if (pRec.GetString("due_code") == "04")
                            sQtrRemarks = "2nd Qtr. " + sDate;
                        if (pRec.GetString("due_code") == "07")
                            sQtrRemarks = "3rd Qtr. " + sDate;
                        if (pRec.GetString("due_code") == "10")
                            sQtrRemarks = "4th Qtr. " + sDate;

                        if (sQtrPaid == "2nd Qtr" && Convert.ToInt32(pRec.GetString("due_code")) <= 4)
                        { }
                        else if (sQtrPaid == "3rd Qtr" && Convert.ToInt32(pRec.GetString("due_code")) <= 7)
                        { }
                        else
                            // sFinalRemarks += string.Format(">3000;{0}", sQtrRemarks);
                            this.axVSPrinter1.Table = string.Format(">3000;{0}", sQtrRemarks);



                    }
                }

            }
            pRec.Close();


        }

        private void GetOtherBusinessInfo(string sBnsCode)
        {
            OracleResultSet pRec = new OracleResultSet();
            m_sOrgnKind = "";
            m_sBnsStat = "";
            m_sBnsOccupancy = ""; //AFM 20190823
            m_sMemo = "";
            m_sFlrArea = "";
            m_sTelNo = "";
            m_sUnitNo = "";
            m_sRoomNo = "";
            m_sGross = "";
            m_sCapital = "";
            m_sRemarks = "";
            m_sPlate = "";
            m_sCitizenship = "";
            m_sNoEmp = "";
            m_sTinNo = "";
            m_sDTIDt = "";  // RMC 20171117 revised format of Business Permit
            m_dDTIDtComp = new DateTime();

            pRec.Query = string.Format("select * from businesses where bin = '{0}'", m_sBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    m_sOrgnKind = pRec.GetString("orgn_kind").Trim();
                    m_sBnsStat = pRec.GetString("bns_stat");
                    m_sMemo = pRec.GetString("memoranda");
                    m_sDTIDt = pRec.GetString("dti_reg_no") + "   " + string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dti_reg_dt"));  // RMC 20171117 revised format of Business Permit
                    m_dDTIDtComp = pRec.GetDateTime("dti_reg_dt");
                    m_sBnsOccupancy = pRec.GetString("place_occupancy"); //AFM 20190823

                    m_sFlrArea = GetArea(m_sBIN, sBnsCode, m_sTaxYear);
                    if (m_sFlrArea == "0.00")
                        m_sFlrArea = "AREA: " + string.Format("{0:##0.00}", pRec.GetDouble("flr_area")); // RMC 20110810 modified
                    else
                        m_sFlrArea = "AREA: " + m_sFlrArea;

                    m_sTelNo = pRec.GetString("bns_telno");

                    try
                    {
                        //m_sNoEmp = pRec.GetDouble("NUM_EMPLOYEES").ToString();
                        m_sNoEmp = GetNumEmployees(m_sBIN, sBnsCode, m_sTaxYear); ;  // RMC 20140106
                    }
                    catch
                    {
                        m_sNoEmp = "0";
                    }

                    m_sTinNo = pRec.GetString("TIN_NO");

                    m_sUnitNo = "No. of Units: " + GetUnit(m_sBIN, sBnsCode, m_sTaxYear, "UNIT");
                    m_sRoomNo = "No. of Rooms: " + GetUnit(m_sBIN, sBnsCode, m_sTaxYear, "ROOM");

                    if (m_sBnsStat == "REN")
                    {
                        m_sBnsStat = "RENEWAL";
                        m_sGross = string.Format("{0:#,##0.00}", pRec.GetDouble("gr_1"));
                        m_sCapital = "";

                        if (m_sGross == "0.00")
                            m_sRemarks = "Zero Gross";
                    }
                    else if (m_sBnsStat == "NEW")
                    {
                        m_sCapital = string.Format("{0:#,##0.00}", pRec.GetDouble("capital"));
                        m_sGross = "";
                    }

                }
            }
            pRec.Close();

            double dAddlCap = 0;
            double dAddlGross = 0;
            string sAddlStat = "";

            int iCntAddlBns = 0;
            pRec.Query = "select count(*) from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "'";            
            int.TryParse(pRec.ExecuteScalar(), out iCntAddlBns);

            m_sArrayBnsCode = new string[iCntAddlBns + 1];
            m_sArrayBnsCode[0] = sBnsCode;

            iCntAddlBns = 0;
            pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "'";
            pRec.Query += " and tax_year = '" + m_sTaxYear + "'";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iCntAddlBns++;

                    m_sArrayBnsCode[iCntAddlBns] = pRec.GetString("bns_code_main");

                    sAddlStat = pRec.GetString("bns_stat");
                    if (sAddlStat == "NEW")
                        dAddlCap += pRec.GetDouble("capital");
                    else
                        dAddlGross += pRec.GetDouble("gross");
                }

                if (m_sCapital != "")
                    dAddlCap += Convert.ToDouble(m_sCapital);
                if (m_sGross != "")
                    dAddlGross += Convert.ToDouble(m_sGross);

                if (dAddlCap != 0)
                    m_sCapital = string.Format("{0:#,##0.00}", dAddlCap);

                if (dAddlGross != 0)
                    m_sGross = string.Format("{0:#,##0.00}", dAddlGross);
            }
            pRec.Close();

            pRec.Query = String.Format("select * from buss_plate where bin = '{0}'", m_sBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    m_sPlate = pRec.GetString("bns_plate");
                }
            }
            pRec.Close();

            pRec.Query = string.Format("select * from own_profile where own_code = '{0}'", AppSettingsManager.GetBnsOwnCode(m_sBIN));
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    m_sCitizenship = pRec.GetString("citizenship");
                }
                else
                    m_sCitizenship = "FILIPINO";    // RMC 20160517 added default nationality to FILIPINO in Permit if no info
            }
            pRec.Close();

        }

        private double GetFeeAdjustment(string sFeesCode, string sNewBnsCode, string sTaxYear, string sQtrPaid)
        {
            OracleResultSet pRec = new OracleResultSet();
            double dBillAmount = 0;
            double dNewAmount = 0;
            string sQtr = "";

            if (sQtrPaid == "F")
                sQtr = "4";

            pRec.Query = "select * from TAXDUES_DUP where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "'";
            pRec.Query += " and tax_code = '" + sFeesCode + "' and bns_code_main = '" + sNewBnsCode + "' and due_state = 'A'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    dBillAmount = pRec.GetDouble("amount");

                    dNewAmount = dBillAmount / 4 * Convert.ToDouble(sQtr);
                }
            }
            pRec.Close();

            return dNewAmount;
        }

        private string GetQtrPaid(string sORNo, string sTaxYear)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sMaxQtr = "";

            pRec.Query = "select max(qtr_paid) from pay_hist where bin = '" + m_sBIN + "'";
            pRec.Query += " and or_no = '" + sORNo + "' and tax_year = '" + sTaxYear + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                    sMaxQtr = pRec.GetString(0);
            }
            pRec.Close();

            return sMaxQtr;
        }

        private string GetPermitFeeCode()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sFeeCode = "";
            string sFeesCode = "";

            pRec.Query = string.Format("select * from tax_and_fees_table where (fees_desc = '{0}'", StringUtilities.HandleApostrophe(sFeesCode));
            pRec.Query += " or fees_desc like '%MAYOR%')";
            pRec.Query += " and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sFeeCode = pRec.GetString("fees_code");
                }
            }
            pRec.Close();

            return sFeeCode;
        }

        private string GetOrNo(string sBnsCode)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sOrNo = "";

            pRec.Query = "select * from or_table where or_no in (";
            pRec.Query += "select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
            pRec.Query += " and bns_code_main = '" + sBnsCode + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sOrNo = pRec.GetString("or_no");
                }
            }
            pRec.Close();

            return sOrNo;
        }

        private string GetDTIBnsName(string sBIN)
        {
            // RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ 

            OracleResultSet pSet = new OracleResultSet();
            string sBnsName = "";

            pSet.Query = "select * from dti_bns_name where bin = '" + sBIN + "' order by saved_tm desc";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sBnsName = pSet.GetString("dti_bns_nm");
                }
            }
            pSet.Close();

            return sBnsName;
        }

        private void PrintPermitModified_3rd()
        {
            // RMC 20120413 Corrections in printing Permit
            OracleResultSet pSet = new OracleResultSet();
            string sBnsNm = "";
            string sOwnNm = "";
            string sBnsAddress = "";
            string sBnsMainDesc = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sORNo = "";
            string sOrgnKind = "";
            string sBnsStat = "";
            string sTelNo = "";
            string sDay = "";
            string sCapital = "";
            string sGross = "";
            string sFeeAmt = "";
            string sRemarks = "";
            string sFlrArea = "";
            string sUnitNo = "";
            string sRoomNo = "";
            string sQtrRemarks = "";
            string sORDate = "";
            string sQtrPaid = "";
            string sPlate = "";
            string sCitizenship = "";
            string sQtrsPaid = "";
            string sMemo = "";
            string strData = "";
            DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate;
            string sOldBnsCode = "";
            string sFeesAmtDue = "";
            string sPermitFeeCode = "";

            m_bPermitUpdateNoBilling = false;

            sBnsNm = GetDTIBnsName(m_sBIN);
            if (sBnsNm == "")
                sBnsNm = AppSettingsManager.GetBnsName(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwner(sOwnNm);
            sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
            sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);   //gets new bns type upon permit update appl change of bns type

            sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
            sBnsMainDesc = AppSettingsManager.GetSubBnsDesc(sBnsCode.Substring(0, 2), "B");
            sBnsMainDesc = GetExemption(sBnsMainDesc);
            GetOtherBusinessInfo(sBnsCode);

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.CurrentY = 2800;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)10.0;
            strData = "||" + m_sPermitNo + "|" + m_sPlate;
            this.axVSPrinter1.Table = string.Format("<150|<1850|>5800|>2000;{0}", strData);
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.MarginLeft = 3100;
            this.axVSPrinter1.CurrentY = 3500;
            this.axVSPrinter1.Table = string.Format("<10000;{0}", sBnsNm);
            this.axVSPrinter1.CurrentY = 3700;
            this.axVSPrinter1.Table = string.Format("<10000;{0}", sBnsAddress);
            this.axVSPrinter1.CurrentY = 3900;
            this.axVSPrinter1.Table = string.Format("<10000;{0}", sOwnNm);
            this.axVSPrinter1.CurrentY = 4100;
            if (sOrgnKind == "SINGLE PROPRIETORSHIP")
                sOrgnKind = "SINGLE PROP.";

            strData = m_sTelNo + "||" + m_sOrgnKind + "||" + m_sCitizenship;
            this.axVSPrinter1.Table = string.Format("<3000|<1000|<2000|<1000|<2000;{0}", strData);

            this.axVSPrinter1.CurrentY = 5150;
            this.axVSPrinter1.MarginLeft = 1200;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = 5820;

            this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.Table = string.Format("<3000|>2900|>100|<2000|>500|<1000;" + sBnsMainDesc + "|CAPITAL INVESTMENT:||" + m_sCapital + "||" + m_sBnsStat);
            this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|<2000|>1000|<1000;" + "||GROSS SALES:||" + m_sGross + "|||");

            sQtrsPaid = ConsolidatedQtr();
            sPermitFeeCode = GetPermitFeeCode();

            //get business tax and mayor's permit paid for main business and other line of business
            string sTmpTaxYear = "";
            string sFeesCode = "";
            string sTmpBnsCode = "";
            string sTmpOldBnsCode = "";
            string sDelqTaxYear = "";
            string sDelqTaxYears = "";
            string sTmpDelqTaxYear = "";
            string sTotal = "";
            double dFeesDueFee = 0;
            double dFeesDueBTax = 0;
            double dTotalAmtPaid = 0;
            double dblSurch = 0;
            double dblPen = 0;
            double dblCredit = 0;
            bool bPaidUsingTaxCredit = false;

            m_dAddlFeesDue = 0;

            int iBnsCount = 0;
            iBnsCount = m_sArrayBnsCode.Length;

            string sTmpQtrPaid = "";
            for (int ix = 1; ix <= iBnsCount; ix++)
            {
                sTmpQtrPaid = "";
                sDelqTaxYears = string.Empty;
                sTmpBnsCode = m_sArrayBnsCode.GetValue(ix - 1).ToString();
                sTmpOldBnsCode = GetOldBnsType(sTmpBnsCode);
                sBnsDesc = AppSettingsManager.GetBnsDesc(sTmpBnsCode);

                dFeesDueFee = 0;
                dFeesDueBTax = 0;
                if (m_bPermitUpdateNoBilling == true && sTmpOldBnsCode != "")
                {
                    sTmpBnsCode = sTmpOldBnsCode;
                    sTmpOldBnsCode = "";
                }

                pSet.Query = "select * from or_table where or_no in ";
                pSet.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
                if (sTmpOldBnsCode != "")
                    pSet.Query += " and bns_code_main = '" + sTmpOldBnsCode + "' and (fees_code = 'B' or fees_code = '" + sPermitFeeCode + "')";
                else
                    pSet.Query += " and bns_code_main = '" + sTmpBnsCode + "' and (fees_code = 'B' or fees_code = '" + sPermitFeeCode + "')";
                //pSet.Query += " order by tax_year, qtr_paid";
                pSet.Query += " order by tax_year";
                if (pSet.Execute())
                {
                    int iCnt = 0;
                    int iCntDelqYear = 0;
                    while (pSet.Read())
                    {
                        iCnt++;
                        sTmpTaxYear = pSet.GetString("tax_year");
                        sFeesCode = pSet.GetString("fees_code");
                        sTmpQtrPaid = pSet.GetString("qtr_paid");

                        if (!bPaidUsingTaxCredit)
                            bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet.GetString("or_no"));

                        if (sTmpTaxYear == m_sTaxYear)
                        {
                            if (sFeesCode == sPermitFeeCode)
                                dFeesDueFee += pSet.GetDouble("fees_due");
                            else
                                dFeesDueBTax += pSet.GetDouble("fees_due");

                            dblSurch += pSet.GetDouble("fees_surch");
                            dblPen += pSet.GetDouble("fees_pen");
                        }
                        else
                        {
                            m_dAddlFeesDue += pSet.GetDouble("fees_due");
                            m_dAddlFeesDue += pSet.GetDouble("fees_surch");
                            m_dAddlFeesDue += pSet.GetDouble("fees_pen");

                            sDelqTaxYear = sTmpTaxYear;

                            if (sTmpDelqTaxYear != sDelqTaxYear)
                            {
                                iCntDelqYear++;

                                if (iCntDelqYear > 1)
                                    sDelqTaxYears += "/ ";
                                sDelqTaxYears += sDelqTaxYear;

                                sTmpDelqTaxYear = sDelqTaxYear;
                            }
                        }


                    }

                    dTotalAmtPaid += dFeesDueFee;
                    dTotalAmtPaid += dFeesDueBTax;
                    dTotalAmtPaid += dblSurch;
                    dTotalAmtPaid += dblPen;

                }
                pSet.Close();

                sFeesAmtDue = string.Format("{0:#,###.00}", dFeesDueBTax);
                // GDE 20120509 added for testing (s){
                if (sTmpQtrPaid == "P")
                    this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + "|BUSINESS TAX:||" + sFeesAmtDue + "|Annually");
                else
                    // GDE 20120823 for testing
                    if (sTmpQtrPaid == string.Empty)
                        this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + "|BUSINESS TAX:||" + sFeesAmtDue + "|Annually");
                    else
                        // GDE 20120823 for testing
                        // GDE 20120509 added for testing (e)}
                        this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + "|BUSINESS TAX:||" + sFeesAmtDue + "|" + sQtrsPaid);
                sFeesAmtDue = string.Format("{0:#,###.00}", dFeesDueFee); ;
                if (sTmpQtrPaid == "P")
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sPermitFeeCode) + "||" + sFeesAmtDue + "|Annually");
                else
                    // GDE 20120823 for testing
                    if (sTmpQtrPaid == string.Empty)
                        this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sPermitFeeCode) + "||" + sFeesAmtDue + "|Annually");
                    else
                        // GDE 20120823 for testing
                        this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sPermitFeeCode) + "||" + sFeesAmtDue + "|" + sQtrsPaid);
            }

            //long lngY1 = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());


            if (m_bWithBalQtr)
            {
                this.axVSPrinter1.CurrentY = 6938;
                this.axVSPrinter1.MarginLeft = 8600;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = (float)7.0;
                sQtrRemarks = "BALANCE TO BE ";
                this.axVSPrinter1.Table = string.Format(">3000;{0}", sQtrRemarks);
                sQtrRemarks = "PAID ON OR BEFORE:";
                this.axVSPrinter1.Table = string.Format(">3000;{0}", sQtrRemarks);
                PrintPaymentDueRemarks();
                this.axVSPrinter1.MarginLeft = 2000;

                this.axVSPrinter1.MarginLeft = 1200;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = (float)8.0;

                //this.axVSPrinter1.CurrentY = lngY1;
                int intCnt = 0;
            }

            // for other fees
            string sTmpFeeCode = "";
            this.axVSPrinter1.FontBold = false;
            OracleResultSet pSet2 = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();
            string sQryResult = string.Empty;

            pSet.Query = "select distinct fees_code from or_table where or_no in ";
            pSet.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
            pSet.Query += " and fees_code <> 'B' and fees_code <> '" + sPermitFeeCode + "' order by fees_code";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sFeesCode = pSet.GetString("fees_code");
                    dFeesDueFee = 0;

                    pSet2.Query = "select * from or_table where or_no in ";
                    pSet2.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
                    pSet2.Query += " and fees_code = '" + sFeesCode + "' order by tax_year, qtr_paid";
                    if (pSet2.Execute())
                    {
                        while (pSet2.Read())
                        {
                            sTmpQtrPaid = "";

                            sTmpTaxYear = pSet2.GetString("tax_year");

                            if (sTmpTaxYear == m_sTaxYear)
                            {
                                dFeesDueFee += pSet2.GetDouble("fees_due");
                                dblSurch += pSet2.GetDouble("fees_surch");
                                dblPen += pSet2.GetDouble("fees_pen");

                                sTmpQtrPaid = pSet2.GetString("qtr_paid");

                                // RMC 20120418 corrections in total displayed in permit (s)
                                //dTotalAmtPaid += pSet2.GetDouble("fees_surch"); // GDE 20120504
                                //dTotalAmtPaid += pSet2.GetDouble("fees_pen"); // GDE 20120504
                                // RMC 20120418 corrections in total displayed in permit (e)
                            }
                            else
                            {
                                m_dAddlFeesDue += pSet2.GetDouble("fees_due");
                                m_dAddlFeesDue += pSet2.GetDouble("fees_surch");
                                m_dAddlFeesDue += pSet2.GetDouble("fees_pen");
                            }
                        }

                        if (AppSettingsManager.GetFeesDesc(sFeesCode) != "GARBAGE FEE")
                        {
                            sQtrsPaid = "";
                            sTmpQtrPaid = "";
                        }

                        sFeesAmtDue = string.Format("{0:#,###.00}", dFeesDueFee);
                        if (sTmpQtrPaid == "P")
                            this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sFeesCode) + "||" + sFeesAmtDue + "|Annually");
                        else
                            this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sFeesCode) + "||" + sFeesAmtDue + "|" + sQtrsPaid);

                        dTotalAmtPaid += dFeesDueFee;
                    }
                    pSet2.Close();

                }

                /* dTotalAmtPaid += dblSurch;
                 dTotalAmtPaid += dblPen;
                 */
                // RMC 20120418 corrections in total displayed in permit

            }
            pSet.Close();

            string sSurch = "";
            string sPen = "";
            string sCredit = "";
            if (m_sMemo.Trim() != "")
            {
                if (MessageBox.Show("Print Remarks?", "Business Permits", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //this.axVSPrinter1.CurrentY = 9622; // GDE 20120607
                    //this.axVSPrinter1.MarginLeft = 2000;
                    this.axVSPrinter1.Table = string.Format("<10000;{0}", m_sMemo);
                }
            }

            sSurch = string.Format("{0:#,##0.00}", dblSurch);
            this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + m_sFlrArea + "|25% Penalty||" + sSurch + "|");

            sPen = string.Format("{0:#,##0.00}", dblPen);
            this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + m_sUnitNo + "|2% Interest||" + sPen + "|");

            dblCredit = GetTaxCreditAmt();
            sCredit = string.Format("{0:#,##0.00}", dblCredit);
            this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + m_sRoomNo + "|Tax Credits||" + sCredit + "|");

            // GDE 20120504 test computation 
            string sTempTotal = string.Empty;
            result.Query = "select sum(fees_amtdue) as fees_amtdue from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "') and tax_year = '" + m_sTaxYear + "'"; // GDE 20121116 add tax year to or_table as per mam ellen
            if (result.Execute())
            {
                if (result.Read())
                    sTempTotal = string.Format("{0:#,##0.00}", result.GetDouble("fees_amtdue"));
            }
            result.Close();
            // GDE 20120504 test computation 

            //sTotal = string.Format("{0:#,##0.00}", dTotalAmtPaid);
            //if(double.Parse(sTotal) > double.Parse(sTempTotal))
            sTotal = string.Format("{0:#,##0.00}", double.Parse(sTempTotal));

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + m_sRemarks + "|TOTAL TAX PAID:||" + sTotal + "|");
            this.axVSPrinter1.FontBold = false;

            m_sNotation = "";

            this.axVSPrinter1.CurrentY = 9222;
            this.axVSPrinter1.MarginLeft = 2000;
            string sTempLastDelq = string.Empty;


            sTempLastDelq = sDelqTaxYears;

            if (sDelqTaxYears != "")
            {
                sTotal = string.Format("{0:#,##0.00}", m_dAddlFeesDue);
                m_sNotation = "Note: With payment for delinquent year/s: " + sDelqTaxYears;
                m_sNotation += " in the amount of P " + sTotal;
            }

            //if (bPaidUsingTaxCredit)
            if (bPaidUsingTaxCredit && m_dAppliedCredit != 0)   // RMC 20120419 added display of tax credit used in Permit
            {
                sTotal = string.Format("{0:#,##0.00}", m_dAppliedCredit);
                m_sNotation += ";Note: Payment using Tax Credit P " + sTotal;   // RMC 20120419 added display of tax credit used in Permit
            }

            this.axVSPrinter1.Table = string.Format("<10000;{0}", m_sNotation);
            /*
            if (m_sMemo.Trim() != "") 
            {
                if (MessageBox.Show("Print Remarks?", "Business Permits", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.axVSPrinter1.CurrentY = 9622; // GDE 20120607
                    this.axVSPrinter1.MarginLeft = 2000;
                    this.axVSPrinter1.Table = string.Format("<10000;{0}", m_sMemo);
                }
            }
             */

            this.axVSPrinter1.CurrentY = 10200;
            this.axVSPrinter1.MarginLeft = 9500;
            this.axVSPrinter1.Table = string.Format("<3000;{0}", m_sBIN);
            this.axVSPrinter1.CurrentY = 10600;

            // get ors
            GetOrs(""); // RMC 20140114 TEMP
            this.axVSPrinter1.Table = string.Format("<3000;{0}", m_sOrNo);
            this.axVSPrinter1.CurrentY = 11000;
            this.axVSPrinter1.Table = string.Format("<3000;{0}", m_sOrDate);

            pSet.Query = "select * from btm_gis_loc where bin = '" + m_sBIN + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    string sLPin = string.Empty;
                    this.axVSPrinter1.CurrentY = 11400;
                    sLPin = pSet.GetString("land_pin").Trim();
                    this.axVSPrinter1.Table = string.Format("<3000;{0}", sLPin);
                }
            }
            pSet.Close();

            if (AuditTrail.InsertTrail("ABBP", "rep_mp", "Print Permit of " + m_sBIN + " for tax year " + m_sTaxYear) == 0) // RMC 20111219 Added tax year in trail of permit printing
            {
                pSet.Rollback();
                pSet.Close();
                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private string GetDefaultDesc(string sDefaultCode)
        {
            string sResult = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from default_code where default_code = '" + sDefaultCode + "'";
            if (result.Execute())
            {
                if (result.Read())
                    sResult = result.GetString("default_desc");
            }
            result.Close();

            return sResult;
        }

        private void PrintPermitMati()
        {
            // RMC 20120413 Corrections in printing Permit
            bool blnIsHeadPrint = true;
            double dTotDue = 0;
            OracleResultSet result2 = new OracleResultSet();
            string sDefaultCode = string.Empty;
            string sDefaultDesc = string.Empty;
            string sAmount = string.Empty;
            double dData = 0;


            OracleResultSet pSet = new OracleResultSet();
            string sBnsNm = "";
            string sOwnNm = "";
            string sBnsAddress = "";
            string sBnsMainDesc = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sORNo = "";
            string sOrgnKind = "";
            string sBnsStat = "";
            string sTelNo = "";
            string sDay = "";
            string sCapital = "";
            string sGross = "";
            string sFeeAmt = "";
            string sRemarks = "";
            string sFlrArea = "";
            string sUnitNo = "";
            string sRoomNo = "";
            string sQtrRemarks = "";
            string sORDate = "";
            string sQtrPaid = "";
            string sPlate = "";
            string sCitizenship = "";
            string sQtrsPaid = "";
            string sMemo = "";
            string strData = "";
            DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate;
            string sOldBnsCode = "";
            string sFeesAmtDue = "";
            string sPermitFeeCode = "";

            m_bPermitUpdateNoBilling = false;

            sBnsNm = GetDTIBnsName(m_sBIN);
            if (sBnsNm == "")
                sBnsNm = AppSettingsManager.GetBnsName(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwner(sOwnNm);
            sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
            sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);   //gets new bns type upon permit update appl change of bns type

            sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
            sBnsMainDesc = AppSettingsManager.GetSubBnsDesc(sBnsCode.Substring(0, 2), "B");
            sBnsMainDesc = GetExemption(sBnsMainDesc);
            GetOtherBusinessInfo(sBnsCode);


            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterTop;
            this.axVSPrinter1.CurrentY = 700;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)10.0;

            strData = "Repuplic of the Philippines";
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            strData = "PROVINCE OF " + AppSettingsManager.GetConfigValue("08");
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                strData = "CITY OF" + AppSettingsManager.GetConfigValue("02");
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                strData = "MUNICIPALITY OF" + AppSettingsManager.GetConfigValue("02");
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            this.axVSPrinter1.FontBold = true;
            strData = "OFFICE OF THE CITY MAYOR";
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);

            this.axVSPrinter1.CurrentY = 2200;
            this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.FontName = "Bookman Old Style";
            this.axVSPrinter1.FontSize = (float)18.0;
            strData = "MAYOR'S PERMIT";
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)11.0;
            strData = " ";
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
            strData = "This certifies that";
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taJustTop;
            this.axVSPrinter1.CurrentY = 700;

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)8.0;
            strData = "Application .No:| " + m_sPermitNo;
            this.axVSPrinter1.Table = string.Format(">9000|>1500;{0}", strData);
            strData = "Business ID No.:| " + m_sBIN;
            this.axVSPrinter1.Table = string.Format(">9000|>1500;{0}", strData);
            strData = "Business Permit No.:| " + m_sPermitNo;
            this.axVSPrinter1.Table = string.Format(">9000|>1500;{0}", strData);
            strData = "Mode of Payment:| " + ConsolidatedQtr();
            this.axVSPrinter1.Table = string.Format(">9000|>1500;{0}", strData);
            strData = "Date Issued:| " + dtpBillDate.ToShortDateString();
            this.axVSPrinter1.Table = string.Format(">9000|>1500;{0}", strData);
            strData = "Expiry Date:| 12/31/" + ConfigurationAttributes.CurrentYear;
            this.axVSPrinter1.Table = string.Format(">9000|>1500;{0}", strData);
            strData = "Type:| " + m_sBnsStat;
            this.axVSPrinter1.Table = string.Format(">9000|>1500;{0}", strData);

            this.axVSPrinter1.CurrentY = 3000;
            strData = " ";
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);

            //this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taJustTop;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)9.0;
            strData = "NAME OF TAXPAYER | " + sOwnNm;
            this.axVSPrinter1.Table = string.Format("<2300|<8000;{0}", strData);
            strData = "BUSINESS NAME | " + sBnsNm;
            this.axVSPrinter1.Table = string.Format("<2300|<8000;{0}", strData);
            strData = "ADDRESS | " + sBnsAddress;
            this.axVSPrinter1.Table = string.Format("<2300|<8000;{0}", strData);
            strData = "TEL./MOBILE NOS. | " + m_sTelNo + "|NO. OF EMPLOYEES|" + m_sNoEmp + "|TIN|" + m_sTinNo;
            this.axVSPrinter1.Table = string.Format("<2300|<2000|<2500|*1000|<700|^1500;{0}", strData);

            this.axVSPrinter1.FontSize = (float)10.0;
            strData = " ";
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
            strData = "has been granted PERMIT to operate the following business/es pursuant to TAX ORDINANCE NO. 02, Series of 1993, of the ";
            strData += "City of Mati and after payment of taxes, fees and regulatory charges and subject to the compliance of such other pertinent laws, ordinances and related administrative regulations.";
            this.axVSPrinter1.Table = string.Format("=11000;{0}", strData);
            strData = " ";
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
            /*
            this.axVSPrinter1.CurrentY = 4100;
            if (sOrgnKind == "SINGLE PROPRIETORSHIP")
                sOrgnKind = "SINGLE PROP.";

            strData = m_sTelNo + "||" + m_sOrgnKind + "||" + m_sCitizenship;
            this.axVSPrinter1.Table = string.Format("<3000|<1000|<2000|<1000|<2000;{0}", strData);

            this.axVSPrinter1.CurrentY = 5150;
            this.axVSPrinter1.MarginLeft = 1200;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = 5820;

            this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.Table = string.Format("<3000|>2900|>100|<2000|>500|<1000;" + sBnsMainDesc + "|CAPITAL INVESTMENT:||" + m_sCapital + "||" + m_sBnsStat);
            this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|<2000|>1000|<1000;" + "||GROSS SALES:||" + m_sGross + "|||");

            sQtrsPaid = ConsolidatedQtr();
            sPermitFeeCode = GetPermitFeeCode();
             */
            sPermitFeeCode = GetPermitFeeCode();
            //get business tax and mayor's permit paid for main business and other line of business
            string sFeesDue = string.Empty;
            string sTmpTaxYear = "";
            string sFeesCode = "";
            string sTmpBnsCode = "";
            string sTmpOldBnsCode = "";
            string sDelqTaxYear = "";
            string sDelqTaxYears = "";
            string sTmpDelqTaxYear = "";
            string sTotal = "";
            double dFeesDueFee = 0;
            double dFeesDueBTax = 0;
            double dTotalAmtPaid = 0;
            double dblSurch = 0;
            double dblPen = 0;
            double dblCredit = 0;
            bool bPaidUsingTaxCredit = false;

            m_dAddlFeesDue = 0;

            int iBnsCount = 0;
            iBnsCount = m_sArrayBnsCode.Length;
            this.axVSPrinter1.CurrentY = 6230; // GDE TRY
            string sTmpQtrPaid = "";
            for (int ix = 1; ix <= iBnsCount; ix++)
            {
                sTmpQtrPaid = "";
                sDelqTaxYears = string.Empty;
                sTmpBnsCode = m_sArrayBnsCode.GetValue(ix - 1).ToString();
                sTmpOldBnsCode = GetOldBnsType(sTmpBnsCode);
                sBnsDesc = AppSettingsManager.GetBnsDesc(sTmpBnsCode);

                dFeesDueFee = 0;
                dFeesDueBTax = 0;
                if (m_bPermitUpdateNoBilling == true && sTmpOldBnsCode != "")
                {
                    sTmpBnsCode = sTmpOldBnsCode;
                    sTmpOldBnsCode = "";
                }

                pSet.Query = "select * from or_table where or_no in ";
                pSet.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
                if (sTmpOldBnsCode != "")
                    //pSet.Query += " and bns_code_main = '" + sTmpOldBnsCode + "' and (fees_code = 'B' or fees_code = '" + sPermitFeeCode + "')";
                    pSet.Query += " and bns_code_main = '" + sTmpOldBnsCode + "' and (fees_code = 'B' or fees_code = '01')";
                else
                    //pSet.Query += " and bns_code_main = '" + sTmpBnsCode + "' and (fees_code = 'B' or fees_code = '" + sPermitFeeCode + "')"; 
                    pSet.Query += " and bns_code_main = '" + sTmpBnsCode + "' and (fees_code = 'B' or fees_code = '01')";
                //pSet.Query += " order by tax_year, qtr_paid";
                pSet.Query += " order by tax_year";

                if (pSet.Execute())
                {
                    int iCnt = 0;
                    int iCntDelqYear = 0;
                    while (pSet.Read())
                    {
                        iCnt++;
                        sTmpTaxYear = pSet.GetString("tax_year");
                        sFeesCode = pSet.GetString("fees_code");
                        sTmpQtrPaid = pSet.GetString("qtr_paid");



                        if (!bPaidUsingTaxCredit)
                            bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet.GetString("or_no"));

                        if (sTmpTaxYear == m_sTaxYear)
                        {
                            if (sFeesCode == sPermitFeeCode)
                                dFeesDueFee = pSet.GetDouble("fees_due");
                            else
                                dFeesDueBTax = pSet.GetDouble("fees_due");

                            dblSurch += pSet.GetDouble("fees_surch");
                            dblPen += pSet.GetDouble("fees_pen");
                        }
                        else
                        {
                            m_dAddlFeesDue += pSet.GetDouble("fees_due");
                            m_dAddlFeesDue += pSet.GetDouble("fees_surch");
                            m_dAddlFeesDue += pSet.GetDouble("fees_pen");

                            sDelqTaxYear = sTmpTaxYear;

                            if (sTmpDelqTaxYear != sDelqTaxYear)
                            {
                                iCntDelqYear++;

                                if (iCntDelqYear > 1)
                                    sDelqTaxYears += "/ ";
                                sDelqTaxYears += sDelqTaxYear;

                                sTmpDelqTaxYear = sDelqTaxYear;
                            }
                        }


                    }

                    //dTotalAmtPaid += dFeesDueFee;
                    dTotalAmtPaid += dFeesDueBTax;
                    dTotalAmtPaid += dblSurch;
                    dTotalAmtPaid += dblPen;

                }
                pSet.Close();
                GetOrs(""); // RMC 20140114 TEMP


                sFeesAmtDue = string.Format("{0:#,###.00}", dFeesDueBTax);
                sFeesDue = string.Format("{0:#,###.00}", dFeesDueFee);
                this.axVSPrinter1.DrawLine(500, 6100, 11800, 6100);
                this.axVSPrinter1.DrawLine(500, 7150, 11800, 7150);
                this.axVSPrinter1.DrawLine(500, 6100, 500, 15500);
                this.axVSPrinter1.DrawLine(11800, 6100, 11800, 15500);
                this.axVSPrinter1.DrawLine(500, 15500, 11800, 15500);

                this.axVSPrinter1.DrawLine(500, 10000, 11800, 10000);
                this.axVSPrinter1.DrawLine(500, 10300, 11800, 10300);
                this.axVSPrinter1.DrawLine(500, 10600, 11800, 10600);


                this.axVSPrinter1.DrawLine(4300, 6100, 4300, 10300);
                this.axVSPrinter1.DrawLine(7500, 6100, 7500, 10300);
                this.axVSPrinter1.DrawLine(10400, 6100, 10400, 10300);

                this.axVSPrinter1.DrawLine(7500, 6500, 10400, 6500);
                this.axVSPrinter1.DrawLine(9000, 6500, 9000, 10300);

                this.axVSPrinter1.FontSize = (float)6.0;
                /*
                strData = this.axVSPrinter1.CurrentY.ToString();
                this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
                this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
                this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
                this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
                 */
                m_sCapital = AppSettingsManager.GetCapitalGross(m_sBIN, sTmpBnsCode, m_sTaxYear, "NEW");
                m_sGross = AppSettingsManager.GetCapitalGross(m_sBIN, sTmpBnsCode, m_sTaxYear, "REN");



                //pSet.Query = "select * from other_info where bin = '" + m_sBIN + "' and bns_code = '" + sBnsCode + "'";
                pSet.Query = "select * from other_info where bin = '" + m_sBIN + "' and bns_code = '" + sBnsCode + "' and data <> 0"; // CJC 20131016
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        try
                        {
                            dData = pSet.GetDouble("data");
                        }
                        catch
                        {
                            dData = 0;
                        }
                        /*
                        if (dData > 0)
                        {
                            sDefaultCode = pSet.GetString("default_code").Trim();
                            this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300; |" + GetDefaultDesc(sDefaultCode) + " - (" + string.Format("{0:0}", dData) + " units)| | | | ");
                        }
                         */
                    }
                }
                pSet.Close();



                this.axVSPrinter1.FontSize = (float)10.0;
                if (blnIsHeadPrint)
                {
                    this.axVSPrinter1.Table = string.Format("<100|^3900|^2900|^3000|^1300;" + "||Capitalization/Investment or|Amount Paid|");
                    this.axVSPrinter1.Table = string.Format("<100|^3900|^2900|^1500|^1500|^1300;" + "|Kind of Business/Business Line|Preceeding Year Gross|TAX |FEE|OR Number");
                    this.axVSPrinter1.Table = string.Format("<100|^3900|^2900|^1500|^1500|^1300;" + "||Receipts or Sales|Surcharges||Date Issued");
                }
                blnIsHeadPrint = false;
                strData = " ";
                this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
                // GDE 20120509 added for testing (s){
                if (sTmpQtrPaid == "P")
                    this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |BUSINESS TAX:||" + sFeesAmtDue + "|Annually");
                else
                    // GDE 20120823 for testing
                    if (sTmpQtrPaid == string.Empty)
                        if (m_sBnsStat == "NEW")
                            this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sCapital + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                        else
                            this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300;;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sGross + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                    else
                        // GDE 20120823 for testing
                        // GDE 20120509 added for testing (e)}
                        //this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + "|BUSINESS TAX:||" + sFeesAmtDue + "|" + sQtrsPaid);
                        if (m_sBnsStat == "NEW")
                            this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sCapital + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                        else
                            this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sGross + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                /*
                sFeesAmtDue = string.Format("{0:#,###.00}", dFeesDueFee); ;
                if (sTmpQtrPaid == "P")
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sPermitFeeCode) + "||" + sFeesAmtDue + "|Annually");
                else
                    // GDE 20120823 for testing
                    if (sTmpQtrPaid == string.Empty)
                        this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sPermitFeeCode) + "||" + sFeesAmtDue + "|Annually");
                    else
                        // GDE 20120823 for testing
                        this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sPermitFeeCode) + "||" + sFeesAmtDue + "|" + sQtrsPaid);
                 */
                dTotDue += double.Parse(sFeesDue);
            }

            //long lngY1 = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());



            axVSPrinter1.CurrentY = 10100;
            this.axVSPrinter1.Table = string.Format("<100|^3900|>2900|>1500|>1500|>1300;" + "|TOTAL||" + string.Format("{0:#,##0.#0}", dTotalAmtPaid) + "|" + string.Format("{0:#,##0.#0}", dTotDue) + "|");
            strData = "ERASURES OR ALTERATIONS WILL INVALIDATE THIS PERMIT";
            this.axVSPrinter1.Table = string.Format("^12000;{0}", strData);
            strData = " ";
            this.axVSPrinter1.Table = string.Format("^12000;{0}", strData);
            this.axVSPrinter1.FontSize = (float)9.0;
            this.axVSPrinter1.FontBold = true;
            strData = "|IMPORTANT REMINDERS:|DEADLINE OF QUARTERLY PAYMENT:";
            this.axVSPrinter1.Table = string.Format("<100|<5900|<5900;{0}", strData);
            this.axVSPrinter1.FontBold = false;
            strData = "|1.|Secure OCCUPATIONAL PERMIT for employees.||     - 2nd quarter - on or before April 20";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);

            strData = "|2.|Post this Permit in a conspicuous place  ||     - 3rd quarter - on or before July 20";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
            strData = "||in your establishment. Failure to display is subject to fine. ||     - 4th quarter - on or before October 20";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
            strData = "|3.|Valid only at the business address indicated above. ||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);



            strData = "|4.|Surrender this permit within 30 days upon termination, closure, or retirement of the business.  Any tax due shall first be paid before any business or undertaking is fully teminated. ||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
            strData = "|5.|FAILURE TO COMPLY WITH THE APPLICABLE PROVISIONS OF TAX ORDINANCE NO. 02, SERIES OF 1993, AND OTHER PERTINENT LAWS, ORDINANCES AND OTHER ADMINISTRATIVE REGULATIONS SHALL CAUSE THE IMMEDIATE REVOCATION OF THIS PERMIT. ||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);

            if (m_sBnsStat == "NEW")
            {
                this.axVSPrinter1.CurrentY = 12600;
                this.axVSPrinter1.MarginLeft = 6500;

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.FontSize = (float)12.0;
                strData = "APPROVED BY:";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = " ";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = " ";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = AppSettingsManager.GetConfigValue("36");
                this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
                this.axVSPrinter1.FontBold = false;
                strData = "CITY MAYOR";
                this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            }
            else
            {
                this.axVSPrinter1.CurrentY = 12400;
                this.axVSPrinter1.MarginLeft = 6500;

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.FontSize = (float)12.0;
                strData = "APPROVED BY:";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = " ";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = AppSettingsManager.GetConfigValue("36");
                this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
                this.axVSPrinter1.FontBold = false;
                strData = "CITY MAYOR";
                this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);

                strData = " ";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                this.axVSPrinter1.FontBold = true;
                strData = "By Authority of the City Mayor:";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = " ";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = AppSettingsManager.GetConfigValue("48");
                this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
                this.axVSPrinter1.FontBold = false;
                strData = AppSettingsManager.GetConfigValue("49");
                this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            }
        }

        private bool ValidatePaymentUsingTaxCredit(string sOrNo)
        {
            // RMC 20120413 Corrections in printing Permit
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select * from dbcr_memo where or_no = '" + sOrNo + "' and served = 'Y'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    pRec.Close();
                    return true;
                }
            }
            pRec.Close();

            return false;
        }

        private void GetOrs(string sBnsCode)    // RMC 20140114 mods in Permit if with permit update transaction
        {
            // RMC 20120413 Corrections in printing Permit
            OracleResultSet pRec = new OracleResultSet();

            m_sOrNo = "";
            m_sOrDate = "";

            //pRec.Query = "select distinct or_no, or_date from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "'";
            // RMC 20140114 mods in Permit if with permit update transaction (s)
            pRec.Query = "select distinct a.or_no, b.or_date from or_table a, pay_hist b where ";
            pRec.Query += " a.or_no = b.or_no and bin = '" + m_sBIN + "' and a.tax_year = '" + m_sTaxYear + "' ";
            pRec.Query += " and a.bns_code_main = '" + sBnsCode + "' and a.tax_year = b.tax_year";
            // RMC 20140114 mods in Permit if with permit update transaction (e)
            if (pRec.Execute())
            {
                int iCnt = 0;
                while (pRec.Read())
                {
                    iCnt++;

                    if (iCnt > 1)
                    {
                        /*m_sOrNo += "   /   ";
                        m_sOrDate += "   /   ";
                         */
                        // RMC 20140114 mods in Permit if with permit update transaction (s)
                        m_sOrNo += " / ";
                        m_sOrDate += " / ";
                        // RMC 20140114 mods in Permit if with permit update transaction (s)
                    }

                    m_sOrNo += pRec.GetString(0);
                    m_sOrDate += string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime(1));
                }
            }
            pRec.Close();
        }

        private void PrintPermitMatiNew() // CJC 20131111
        {
            // RMC 20120413 Corrections in printing Permit
            bool blnIsHeadPrint = true;
            double dTotDue = 0;
            OracleResultSet result2 = new OracleResultSet();
            string sDefaultCode = string.Empty;
            string sDefaultDesc = string.Empty;
            string sAmount = string.Empty;
            double dData = 0;


            OracleResultSet pSet = new OracleResultSet();
            string sBnsNm = "";
            string sOwnNm = "";
            string sBnsAddress = "";
            string sBnsMainDesc = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sORNo = "";
            string sOrgnKind = "";
            string sBnsStat = "";
            string sTelNo = "";
            string sDay = "";
            string sCapital = "";
            string sGross = "";
            string sFeeAmt = "";
            string sRemarks = "";
            string sFlrArea = "";
            string sUnitNo = "";
            string sRoomNo = "";
            string sQtrRemarks = "";
            string sORDate = "";
            string sQtrPaid = "";
            string sPlate = "";
            string sCitizenship = "";
            string sQtrsPaid = "";
            string sMemo = "";
            string strData = "";
            //DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpBillDate = m_dtDate;    // RMC 20140129 Added module to input permit date issuance before printing of Permit
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate;
            string sOldBnsCode = "";
            string sFeesAmtDue = "";
            string sPermitFeeCode = "";
            string strSurPen = string.Empty;

            m_bPermitUpdateNoBilling = false;

            sBnsNm = GetDTIBnsName(m_sBIN);
            if (sBnsNm == "")
                sBnsNm = AppSettingsManager.GetBnsName(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwner(sOwnNm);
            sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
            sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);   //gets new bns type upon permit update appl change of bns type

            sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
            sBnsMainDesc = AppSettingsManager.GetSubBnsDesc(sBnsCode.Substring(0, 2), "B");
            //sBnsMainDesc = GetExemption(sBnsMainDesc);
            GetOtherBusinessInfo(sBnsCode);

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taJustTop;
            //this.axVSPrinter1.CurrentY = 2200;
            this.axVSPrinter1.CurrentY = 2400;  // RMC 20131227 adjusted based on pre-printed form

            // RMC 20140110 modified permit if business under incentives - Mati (s)
            string sExemption = string.Empty;
            sExemption = GetExemption(sBnsMainDesc);
            // RMC 20140110 modified permit if business under incentives - Mati (e)

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";
            //this.axVSPrinter1.FontSize = (float)7.0;
            this.axVSPrinter1.FontSize = (float)8.0;    // RMC 20140117 adjusted font size in permit
            this.axVSPrinter1.FontSize = (float)8.0;
            //// RMC 20140110 modified permit if business under incentives - Mati (s)
            //if (sExemption != "")
            //    m_sPermitNo = "";
            //// RMC 20140110 modified permit if business under incentives - Mati (e)

            //strData = "Application No:| " + m_sPermitNo;
            strData = "App. No:| " + m_sPermitNo;// RMC 20140117 adjusted font size in permit
            //this.axVSPrinter1.Table = string.Format(">9100|>1500;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9180|>1480;{0}", strData);    // RMC 20131227 adjusted based on pre-printed form
            //strData = "Business ID No:| " + m_sBIN;
            strData = "Buss. ID No:| " + m_sBIN;    // RMC 20140117 adjusted font size in permit
            //this.axVSPrinter1.Table = string.Format(">9100|>1500;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9180|>1480;{0}", strData);    // RMC 20131227 adjusted based on pre-printed form
            //strData = "Business Permit No.:| " + m_sPermitNo;

            // RMC 20140110 modified permit if business under incentives - Mati (s)
            if (sExemption != "")
                m_sPermitNo = "";
            // RMC 20140110 modified permit if business under incentives - Mati (e)

            strData = "Buss. Permit No:| " + m_sPermitNo;  // RMC 20140102
            //this.axVSPrinter1.Table = string.Format(">9100|>1500;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9180|>1480;{0}", strData);    // RMC 20131227 adjusted based on pre-printed form
            // RMC 20140116 adjustment in Permit printing (s)
            string sModeOfPayment = string.Empty;
            sModeOfPayment = ConsolidatedQtr();
            // RMC 20140116 adjustment in Permit printing (e)

            // RMC 20140110 modified permit if business under incentives - Mati (s)
            if (sExemption != "")
                strData = "Mode of Payment:| ";
            else
                // RMC 20140110 modified permit if business under incentives - Mati (e)
                //strData = "Mode of Payment:| " + ConsolidatedQtr();
                strData = "Mode of Payment:| " + sModeOfPayment;    // RMC 20140116 adjustment in Permit printing

            //this.axVSPrinter1.Table = string.Format(">9100|>1500;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9180|>1480;{0}", strData);    // RMC 20131227 adjusted based on pre-printed form

            strData = "Date Issued:| " + dtpBillDate.ToShortDateString();
            //this.axVSPrinter1.Table = string.Format(">9100|>1500;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9180|>1480;{0}", strData);    // RMC 20131227 adjusted based on pre-printed form
            strData = "Expiry Date:| 12/31/" + ConfigurationAttributes.CurrentYear;
            //this.axVSPrinter1.Table = string.Format(">9100|>1500;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9180|>1480;{0}", strData);    // RMC 20131227 adjusted based on pre-printed form
            // RMC 20140110 modified permit if business under incentives - Mati (s)
            if (sExemption != "")
                strData = "Type:| ";    // RMC 20140110 modified permit if business under incentives - Mati (e)
            else
                strData = "Type:| " + m_sBnsStat;
            //this.axVSPrinter1.Table = string.Format(">9100|>1500;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9180|>1480;{0}", strData);    // RMC 20131227 adjusted based on pre-printed form

            //this.axVSPrinter1.CurrentY = 3750;  // RMC 20140117
            this.axVSPrinter1.CurrentY = 3950;  // RMC 20131227 adjusted based on pre-printed form

            strData = " ";
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);

            //this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taJustTop;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";
            //this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.FontSize = (float)10.0;    // RMC 20140117 adjusted font size in permit
            this.axVSPrinter1.CurrentY = 4380;  // RMC 20140117 adjusted font size in permit
            strData = " | " + sOwnNm;
            //this.axVSPrinter1.Table = string.Format("<2300|<8000;{0}", strData);
            this.axVSPrinter1.Table = string.Format("<2100|<8000;{0}", strData);    // RMC 20140117 adjusted font size in permit
            //this.axVSPrinter1.CurrentY = 4420;
            this.axVSPrinter1.CurrentY = 4620;  // RMC 20131227 adjusted based on pre-printed form
            /*strData = " | " + sBnsNm;
            this.axVSPrinter1.Table = string.Format("<2300|<8000;{0}", strData);*/
            // RMC 20140121 adjusted font size in permit (s)
            if (sBnsNm.Length > 42)
            {
                this.axVSPrinter1.FontSize = (float)8.0;
                string sTmpBnsNm1 = string.Empty;
                string sTmpBnsNm2 = string.Empty;

                if (sBnsNm.Length > 56)
                {
                    sTmpBnsNm1 = sBnsNm.Substring(0, 56);
                    sTmpBnsNm2 = sBnsNm.Substring(56, (sBnsNm.Length - 56));
                    strData = " | " + sTmpBnsNm1 + "| |" + m_sTelNo;
                    this.axVSPrinter1.Table = string.Format("<2100|<6000|<1100|<1500;{0}", strData);
                    this.axVSPrinter1.CurrentY = 4760;
                    strData = " | " + sTmpBnsNm2 + "| |";
                    this.axVSPrinter1.Table = string.Format("<2100|<6000|<1100|<1500;{0}", strData);
                }
                else
                {
                    strData = " | " + sBnsNm + "| |" + m_sTelNo;
                    this.axVSPrinter1.Table = string.Format("<2100|<6000|<1100|<1500;{0}", strData);
                }
            }
            else
            // RMC 20140121 adjusted font size in permit (e)
            {
                strData = " | " + sBnsNm + "| |" + m_sTelNo;    // RMC 20140107 added display of bns tel no in Permit
                //this.axVSPrinter1.Table = string.Format("<2300|<6000|<1000|<1500;{0}", strData);    // RMC 20140107 added display of bns tel no in Permit
                this.axVSPrinter1.Table = string.Format("<2100|<6000|<1100|<1500;{0}", strData);    // RMC 20140117 adjusted font size in permit
            }

            this.axVSPrinter1.FontSize = (float)10.0;   // RMC 20140121 adjusted font size in permit
            //this.axVSPrinter1.CurrentY = 4660;

            this.axVSPrinter1.CurrentY = 4860;  // RMC 20131227 adjusted based on pre-printed form

            // RMC 20140122 adjusted font size in permit (s)
            if (sBnsNm.Length > 56)
            {
                this.axVSPrinter1.FontSize = (float)8.0;
                this.axVSPrinter1.CurrentY = 4960;
            }
            // RMC 20140122 adjusted font size in permit (e)

            strData = " | " + sBnsAddress;
            //this.axVSPrinter1.Table = string.Format("<2300|<8000;{0}", strData);
            this.axVSPrinter1.Table = string.Format("<2100|<9000;{0}", strData);    // RMC 20140117 adjusted font size in permit

            this.axVSPrinter1.FontSize = (float)10.0;   // RMC 20140122 adjusted font size in permit

            //this.axVSPrinter1.CurrentY = 4900;

            this.axVSPrinter1.CurrentY = 5100;  // RMC 20131227 adjusted based on pre-printed form
            //strData = " | " + m_sTelNo + "| |" + m_sNoEmp + "| |" + m_sTinNo;
            strData = " | " + m_sOrgnKind + "| |" + m_sNoEmp + "| |" + m_sTinNo;    // RMC 20140103 adjustment 
            //this.axVSPrinter1.Table = string.Format("<2300|<2500|<2000|*1000|<700|^1500;{0}", strData);
            this.axVSPrinter1.Table = string.Format("<2100|<3000|<1800|*1000|<800|^1500;{0}", strData); // RMC 20140117 adjusted font size in permit

            this.axVSPrinter1.FontSize = (float)10.0;
            strData = " ";
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
            strData = "has been granted PERMIT to operate the following business/es pursuant to TAX ORDINANCE NO. 02, Series of 1993, of the ";
            strData += "City of Mati and after payment of taxes, fees and regulatory charges and subject to the compliance of such other pertinent laws, ordinances and related administrative regulations.";
            strData = " ";
            this.axVSPrinter1.Table = string.Format("=11000;{0}", strData);
            strData = " ";
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
            this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
            /*
            this.axVSPrinter1.CurrentY = 4100;
            if (sOrgnKind == "SINGLE PROPRIETORSHIP")
                sOrgnKind = "SINGLE PROP.";

            strData = m_sTelNo + "||" + m_sOrgnKind + "||" + m_sCitizenship;
            this.axVSPrinter1.Table = string.Format("<3000|<1000|<2000|<1000|<2000;{0}", strData);

            this.axVSPrinter1.CurrentY = 5150;
            this.axVSPrinter1.MarginLeft = 1200;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = 5820;

            this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.Table = string.Format("<3000|>2900|>100|<2000|>500|<1000;" + sBnsMainDesc + "|CAPITAL INVESTMENT:||" + m_sCapital + "||" + m_sBnsStat);
            this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|<2000|>1000|<1000;" + "||GROSS SALES:||" + m_sGross + "|||");

            sQtrsPaid = ConsolidatedQtr();
            sPermitFeeCode = GetPermitFeeCode();
             */
            sPermitFeeCode = GetPermitFeeCode();
            //get business tax and mayor's permit paid for main business and other line of business
            string sFeesDue = string.Empty;
            string sTmpTaxYear = "";
            string sFeesCode = "";
            string sTmpBnsCode = "";
            string sTmpOldBnsCode = "";
            string sDelqTaxYear = "";
            string sDelqTaxYears = "";
            string sTmpDelqTaxYear = "";
            string sTotal = "";
            double dFeesDueFee = 0;
            double dFeesDueBTax = 0;
            double dTotalAmtPaid = 0;
            double dblSurch = 0;
            double dblPen = 0;
            double dblCredit = 0;
            bool bPaidUsingTaxCredit = false;
            double dTotSurPen = 0.0;

            m_dAddlFeesDue = 0;

            int iBnsCount = 0;
            iBnsCount = m_sArrayBnsCode.Length;
            //this.axVSPrinter1.CurrentY = 6400; // GDE TRY
            this.axVSPrinter1.CurrentY = 6900;  // RMC 20131227 adjusted based on pre-printed form
            string sTmpQtrPaid = "";

            for (int ix = 1; ix <= iBnsCount; ix++)
            {
                sTmpQtrPaid = "";
                sDelqTaxYears = string.Empty;
                sTmpBnsCode = m_sArrayBnsCode.GetValue(ix - 1).ToString();
                if (iBnsCount == 1) //MCR 20140429
                    sTmpOldBnsCode = GetOldBnsType(sTmpBnsCode);  // RMC 20140114 mods in Permit if with permit update transaction
                sBnsDesc = AppSettingsManager.GetBnsDesc(sTmpBnsCode);

                dFeesDueFee = 0;
                dFeesDueBTax = 0;
                if (m_bPermitUpdateNoBilling == true && sTmpOldBnsCode != "")
                {
                    sTmpBnsCode = sTmpOldBnsCode;
                    sTmpOldBnsCode = "";
                }

                pSet.Query = "select * from or_table where or_no in ";
                pSet.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
                if (sTmpOldBnsCode != "")
                    //pSet.Query += " and bns_code_main = '" + sTmpOldBnsCode + "' and (fees_code = 'B' or fees_code = '" + sPermitFeeCode + "')";
                    pSet.Query += " and bns_code_main = '" + sTmpOldBnsCode + "' and (fees_code = 'B' or fees_code = '01')";
                else
                    //pSet.Query += " and bns_code_main = '" + sTmpBnsCode + "' and (fees_code = 'B' or fees_code = '" + sPermitFeeCode + "')"; 
                    pSet.Query += " and bns_code_main = '" + sTmpBnsCode + "' and (fees_code = 'B' or fees_code = '01')";
                //pSet.Query += " order by tax_year, qtr_paid";   // RMC 20131227 adjusted based on pre-printed form, enabled
                pSet.Query += " order by tax_year, qtr_paid desc";  // RMC 20140114 mods in Permit if with permit update transaction
                //pSet.Query += " order by tax_year";   // RMC 20131227 adjusted based on pre-printed form
                string sBnsCodeX = string.Empty;    // RMC 20140114 mods in Permit if with permit update transaction
                if (pSet.Execute())
                {
                    int iCnt = 0;
                    int iCntDelqYear = 0;
                    while (pSet.Read())
                    {
                        iCnt++;
                        sTmpTaxYear = pSet.GetString("tax_year");
                        sFeesCode = pSet.GetString("fees_code");
                        sTmpQtrPaid = pSet.GetString("qtr_paid");

                        sBnsCodeX = pSet.GetString("BNS_CODE_MAIN");   // RMC 20140114 mods in Permit if with permit update transaction (s)

                        if (!bPaidUsingTaxCredit)
                            bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet.GetString("or_no"));

                        if (sTmpTaxYear == m_sTaxYear)
                        {
                            if (sFeesCode == sPermitFeeCode)
                                //dFeesDueFee = pSet.GetDouble("fees_due");
                                dFeesDueFee += pSet.GetDouble("fees_due");  // RMC 20140114 mods in Permit if with permit update transaction
                            else
                                //dFeesDueBTax = pSet.GetDouble("fees_due");
                                dFeesDueBTax += pSet.GetDouble("fees_due"); // RMC 20140114 mods in Permit if with permit update transaction

                            dblSurch += pSet.GetDouble("fees_surch");
                            dblPen += pSet.GetDouble("fees_pen");
                        }
                        else
                        {
                            m_dAddlFeesDue += pSet.GetDouble("fees_due");
                            m_dAddlFeesDue += pSet.GetDouble("fees_surch");
                            m_dAddlFeesDue += pSet.GetDouble("fees_pen");

                            sDelqTaxYear = sTmpTaxYear;

                            if (sTmpDelqTaxYear != sDelqTaxYear)
                            {
                                iCntDelqYear++;

                                if (iCntDelqYear > 1)
                                    sDelqTaxYears += "/ ";
                                sDelqTaxYears += sDelqTaxYear;

                                sTmpDelqTaxYear = sDelqTaxYear;
                            }
                        }


                    }

                    //dTotalAmtPaid += dFeesDueFee;
                    dTotalAmtPaid += dFeesDueBTax;
                    //dTotalAmtPaid += dblSurch;
                    //dTotalAmtPaid += dblPen;

                }
                pSet.Close();
                //GetOrs();
                GetOrs(sBnsCodeX);  // RMC 20140114 mods in Permit if with permit update transaction


                sFeesAmtDue = string.Format("{0:#,###.00}", dFeesDueBTax);
                sFeesDue = string.Format("{0:#,###.00}", dFeesDueFee);
                strSurPen = string.Format("{0:#,###.00}", dblSurch + dblPen);

                // RMC 20140110 modified permit if business under incentives - Mati (s)
                if ((dFeesDueBTax + dFeesDueFee + dblSurch + dblPen) == 0)
                {
                    if (dFeesDueBTax == 0)
                        sFeesAmtDue = "";
                    if (dFeesDueFee == 0)
                        sFeesDue = "";
                    if ((dblSurch + dblPen) == 0)
                        strSurPen = "";
                }
                // RMC 20140110 modified permit if business under incentives - Mati (e)

                /*this.axVSPrinter1.DrawLine(500, 6100, 11800, 6100);
                this.axVSPrinter1.DrawLine(500, 7150, 11800, 7150);
                this.axVSPrinter1.DrawLine(500, 6100, 500, 15500);
                this.axVSPrinter1.DrawLine(11800, 6100, 11800, 15500);
                this.axVSPrinter1.DrawLine(500, 15500, 11800, 15500);

                this.axVSPrinter1.DrawLine(500, 10000, 11800, 10000);
                this.axVSPrinter1.DrawLine(500, 10300, 11800, 10300);
                this.axVSPrinter1.DrawLine(500, 10600, 11800, 10600);
                

                this.axVSPrinter1.DrawLine(4300, 6100, 4300, 10300);
                this.axVSPrinter1.DrawLine(7500, 6100, 7500, 10300);
                this.axVSPrinter1.DrawLine(10400, 6100, 10400, 10300);

                this.axVSPrinter1.DrawLine(7500, 6500, 10400, 6500);
                this.axVSPrinter1.DrawLine(9000, 6500, 9000, 10300);
                */
                this.axVSPrinter1.FontSize = (float)6.0;
                /*
                strData = this.axVSPrinter1.CurrentY.ToString();
                this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
                this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
                this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
                this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
                 */
                m_sCapital = AppSettingsManager.GetCapitalGross(m_sBIN, sTmpBnsCode, m_sTaxYear, "NEW");
                m_sGross = AppSettingsManager.GetCapitalGross(m_sBIN, sTmpBnsCode, m_sTaxYear, "REN");

                //// RMC 20140110 added module to insert gross/capital to be reflected in Permit (s) Mati
                //if (m_sBnsStat == "NEW" || m_sBnsStat == "REN")
                //{
                //    pSet.Query = "select * from mati_permit_gross where bin = '" + m_sBIN + "' and bns_code_main = '" + sTmpBnsCode + "' and tax_year = '" + m_sTaxYear + "'";
                //    if (pSet.Execute())
                //    {
                //        if (pSet.Read())
                //        {
                //            m_sCapital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
                //            m_sGross = string.Format("{0:#,##0.00}", pSet.GetDouble("gross"));
                //        }
                //    }
                //    pSet.Close();
                //}
                //// RMC 20140110 added module to insert gross/capital to be reflected in Permit (e)


                //pSet.Query = "select * from other_info where bin = '" + m_sBIN + "' and bns_code = '" + sBnsCode + "'";
                pSet.Query = "select * from other_info where bin = '" + m_sBIN + "' and bns_code = '" + sBnsCode + "' and data <> 0"; // CJC 20131016
                pSet.Query += " and (default_code = '0002' or default_code = '0003' or default_code = '0004' or default_code = '0005' or default_code = '0006')";   // RMC 20140102 adjusted
                if (pSet.Execute())
                {
                    dData = 0;  // RMC 20140102
                    //if (pSet.Read())
                    while (pSet.Read())  // RMC 20140102
                    {
                        try
                        {
                            //dData = pSet.GetDouble("data");
                            dData += pSet.GetDouble("data"); // RMC 20140102
                        }
                        catch
                        {
                            //dData = 0;
                            dData += 0;  // RMC 20140102
                        }
                        /*
                        if (dData > 0)
                        {
                            sDefaultCode = pSet.GetString("default_code").Trim();
                            this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300; |" + GetDefaultDesc(sDefaultCode) + " - (" + string.Format("{0:0}", dData) + " units)| | | | ");
                        }
                         */
                    }
                }
                pSet.Close();



                //this.axVSPrinter1.FontSize = (float)10.0;
                this.axVSPrinter1.FontSize = (float)8.0;
                if (blnIsHeadPrint)
                {
                    this.axVSPrinter1.Table = string.Format("<100|^3900|^2900|^3000|^1300;" + "| | | |");
                    this.axVSPrinter1.Table = string.Format("<100|^3900|^2900|^1500|^1500|^1300;" + "| | | | |");
                    this.axVSPrinter1.Table = string.Format("<100|^3900|^2900|^1500|^1500|^1300;" + "| | | | |");
                }
                blnIsHeadPrint = false;
                strData = " ";
                // RMC 20140102 (S)
                string sTmpUnit = string.Empty;
                if (dData > 0)
                    sTmpUnit = "- (" + string.Format("{0:0}", dData) + " units)";
                // RMC 20140102 (E)
                //this.axVSPrinter1.Table = string.Format(">9000;{0}", strData);
                // GDE 20120509 added for testing (s){
                /*if (sTmpQtrPaid == "P")
                {
                    //this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |BUSINESS TAX:||" + sFeesAmtDue + "|Annually");
                    this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + " " + sTmpUnit + " |BUSINESS TAX:||" + sFeesAmtDue + "|Annually");    // RMC 20140102
                }
                else*/
                // RMC 20140114 mods in Permit if with permit update transaction, put rem
                // GDE 20120823 for testing
                if (sTmpQtrPaid == string.Empty)
                    if (m_sBnsStat == "NEW")
                        //this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sCapital + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                        //    this.axVSPrinter1.Table = string.Format("<700|<3000|>2000|>1250|>1250|>1250|>1500;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sCapital + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + strSurPen + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                        this.axVSPrinter1.Table = string.Format("<700|<3000|>2000|>1250|>1250|>1250|>1700;" + "|" + sBnsDesc + " " + sTmpUnit + "|" + m_sCapital + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + strSurPen + "|" + m_sOrNo + " - " + m_sOrDate + ";"); // RMC 20140102
                    else
                        //this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300;;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sGross + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                        //    this.axVSPrinter1.Table = string.Format("<700|<3000|>2000|>1250|>1250|>1250|>1500;;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sGross + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + strSurPen + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                        this.axVSPrinter1.Table = string.Format("<700|<3000|>2000|>1250|>1250|>1250|>1700;;" + "|" + sBnsDesc + " " + sTmpUnit + "|" + m_sGross + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + strSurPen + "|" + m_sOrNo + " - " + m_sOrDate + ";");  // RMC 20140102
                else
                    // GDE 20120823 for testing
                    // GDE 20120509 added for testing (e)}
                    //this.axVSPrinter1.Table = string.Format("<100|<2900|>2900|>100|>2000|>1000|<1000;" + "|" + sBnsDesc + "|BUSINESS TAX:||" + sFeesAmtDue + "|" + sQtrsPaid);
                    if (m_sBnsStat == "NEW")
                        //this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sCapital + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                        //this.axVSPrinter1.Table = string.Format("<700|<3000|>2000|>1250|>1250|>1250|>1500;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sCapital + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + strSurPen + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                        this.axVSPrinter1.Table = string.Format("<700|<3000|>2000|>1250|>1250|>1250|>1700;" + "|" + sBnsDesc + " " + sTmpUnit + "|" + m_sCapital + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + strSurPen + "|" + m_sOrNo + " - " + m_sOrDate + ";"); // RMC 20140102
                    else
                        //this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sGross + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                        //this.axVSPrinter1.Table = string.Format("<700|<3000|>2000|>1250|>1250|>1250|>1500;" + "|" + sBnsDesc + "  - (" + string.Format("{0:0}", dData) + " units) |" + m_sGross + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + strSurPen + "|" + m_sOrNo + " - " + m_sOrDate + ";");
                        this.axVSPrinter1.Table = string.Format("<700|<3000|>2000|>1250|>1250|>1250|>1700;" + "|" + sBnsDesc + " " + sTmpUnit + "|" + m_sGross + "|" + sFeesAmtDue + "|" + sFeesDue + "|" + strSurPen + "|" + m_sOrNo + " - " + m_sOrDate + ";");   // RMC 20140102
                /*
                sFeesAmtDue = string.Format("{0:#,###.00}", dFeesDueFee); ;
                if (sTmpQtrPaid == "P")
                    this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sPermitFeeCode) + "||" + sFeesAmtDue + "|Annually");
                else
                    // GDE 20120823 for testing
                    if (sTmpQtrPaid == string.Empty)
                        this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sPermitFeeCode) + "||" + sFeesAmtDue + "|Annually");
                    else
                        // GDE 20120823 for testing
                        this.axVSPrinter1.Table = string.Format("<3000|>2900|>600|>1500|>1000|<1000;" + "|" + AppSettingsManager.GetFeesDesc(sPermitFeeCode) + "||" + sFeesAmtDue + "|" + sQtrsPaid);
                 */
                try
                {
                    dTotDue += double.Parse(sFeesDue);
                }
                catch
                {
                    dTotDue += 0;   // RMC 20140110 modified permit if business under incentives - Mati
                }

                try
                {
                    dTotSurPen += double.Parse(strSurPen);
                }
                catch
                {
                    dTotSurPen += 0;    // RMC 20140110 modified permit if business under incentives - Mati
                }
            }

            //long lngY1 = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());

            // RMC 20140110 modified permit if business under incentives - Mati (s)
            if (sExemption != "")
            {
                strData = "";
                this.axVSPrinter1.Table = string.Format("<700|<5000;" + "|" + strData);
                strData = sExemption;
                this.axVSPrinter1.Table = string.Format("<700|<5000;" + "|" + strData);
            }
            // RMC 20140110 modified permit if business under incentives - Mati (e)

            this.axVSPrinter1.FontBold = true;
            if (iBnsCount <= 8)  // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
                axVSPrinter1.CurrentY = 9700;
            //this.axVSPrinter1.Table = string.Format("<100|<3900|>2900|>1500|>1500|>1300;" + "|TOTAL||" + string.Format("{0:#,##0.#0}", dTotalAmtPaid) + "|" + string.Format("{0:#,##0.#0}", dTotDue) + "|");
            //this.axVSPrinter1.Table = string.Format("<700|<3000|>2000|>1250|>1250|>1250|>1500;" + "|TOTAL||" + string.Format("{0:#,##0.#0}", dTotalAmtPaid) + "|" + string.Format("{0:#,##0.#0}", dTotDue) + "|" + string.Format("{0:#,##0.#0}", dTotSurPen) + "|");
            if ((dTotalAmtPaid + dTotDue + dTotSurPen) != 0)   // RMC 20140110 modified permit if business under incentives - Mati
                this.axVSPrinter1.Table = string.Format("<700|<3000|>2000|>1250|>1250|>1250|>1700;" + "|TOTAL||" + string.Format("{0:#,##0.#0}", dTotalAmtPaid) + "|" + string.Format("{0:#,##0.#0}", dTotDue) + "|" + string.Format("{0:#,##0.#0}", dTotSurPen) + "|");
            strData = "ERASURES OR ALTERATIONS WILL INVALIDATE THIS PERMIT";
            strData = " ";
            //this.axVSPrinter1.Table = string.Format("^12000;{0}", strData);
            strData = " ";
            this.axVSPrinter1.Table = string.Format("^12000;{0}", strData);
            this.axVSPrinter1.FontSize = (float)9.0;
            this.axVSPrinter1.FontBold = true;

            if (sModeOfPayment != "Annual") // RMC 20140116 adjustment in Permit printing
            {
                if (sExemption != "") { }   // RMC 20140110 modified permit if business under incentives - Mati
                else
                {
                    if (sTmpQtrPaid != "F") // RMC 20131227 adjusted based on pre-printed form
                    {
                        strData = "| |DEADLINE OF QUARTERLY PAYMENT:";
                        if (iBnsCount > 8)  // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
                        {
                            this.axVSPrinter1.FontSize = (float)8.0;    // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
                            this.axVSPrinter1.Table = string.Format("<100|<5900|>5300;{0}", strData);   // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
                        }
                        else
                            this.axVSPrinter1.Table = string.Format("<100|<5900|<5900;{0}", strData);
                    }
                    this.axVSPrinter1.FontBold = false;
                    try
                    {
                        if (Convert.ToInt32(sTmpQtrPaid) >= 1 && Convert.ToInt32(sTmpQtrPaid) < 2)  // RMC 20131227 adjusted based on pre-printed form
                        {
                            strData = "| | | |     - 2nd quarter - on or before April 20";
                            if (iBnsCount > 8)  // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
                            {
                                this.axVSPrinter1.FontSize = (float)8.0;    // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
                                this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|>5000;{0}", strData);
                            }
                            else
                                this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
                        }
                    }
                    catch { }
                }

                try
                {
                    if (Convert.ToInt32(sTmpQtrPaid) >= 1 && Convert.ToInt32(sTmpQtrPaid) < 3)  // RMC 20131227 adjusted based on pre-printed form
                    {
                        strData = "| | | |     - 3rd quarter - on or before July 20";
                        if (iBnsCount > 8)  // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
                        {
                            this.axVSPrinter1.FontSize = (float)8.0;    // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
                            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|>5000;{0}", strData);
                        }
                        else
                            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
                    }
                }
                catch { }

                try
                {
                    if (Convert.ToInt32(sTmpQtrPaid) >= 1 && Convert.ToInt32(sTmpQtrPaid) < 4)  // RMC 20131227 adjusted based on pre-printed form
                    {
                        strData = "| | | |     - 4th quarter - on or before October 20";
                        if (iBnsCount > 8)  // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
                        {
                            this.axVSPrinter1.FontSize = (float)8.0;    // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
                            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|>5000;{0}", strData);
                        }
                        else
                            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
                    }
                }
                catch { }
            }

            this.axVSPrinter1.FontSize = (float)9.0;    // RMC 20140130 corrected overlapping in Permit for business with more than 8 lines of business
            strData = "| | | |";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);


            strData = "|4.|Surrender this permit within 30 days upon termination, closure, or retirement of the business.  Any tax due shall first be paid before any business or undertaking is fully teminated. ||";
            strData = "| | | |";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
            strData = "|5.|FAILURE TO COMPLY WITH THE APPLICABLE PROVISIONS OF TAX ORDINANCE NO. 02, SERIES OF 1993, AND OTHER PERTINENT LAWS, ORDINANCES AND OTHER ADMINISTRATIVE REGULATIONS SHALL CAUSE THE IMMEDIATE REVOCATION OF THIS PERMIT. ||";
            strData = "| | | |";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);

            /*if (m_sBnsStat == "NEW")
            {
                this.axVSPrinter1.CurrentY = 11900;
                this.axVSPrinter1.MarginLeft = 6600;

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.FontSize = (float)11.0;
                strData = "APPROVED BY:";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = " ";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = " ";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                this.axVSPrinter1.MarginLeft = 5700;
                strData = AppSettingsManager.GetConfigValue("36");
                this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
                this.axVSPrinter1.FontBold = false;
                strData = "CITY MAYOR";
                this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            }
            else*/
            // RMC 20130108 same signatories in Permit for new and renewal as requested by Mr. Capalit
            {
                this.axVSPrinter1.CurrentY = 11600;
                this.axVSPrinter1.MarginLeft = 6600;

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.FontSize = (float)11.0;
                strData = "APPROVED BY:";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = " ";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                strData = " ";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                this.axVSPrinter1.MarginLeft = 5700;
                strData = AppSettingsManager.GetConfigValue("36");
                this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
                this.axVSPrinter1.FontBold = false;
                //strData = "CITY MAYOR";
                strData = AppSettingsManager.GetConfigValue("55");
                this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
                this.axVSPrinter1.MarginLeft = 6600;
                strData = " ";
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);  // RMC 20140102

                if (AppSettingsManager.GetConfigValue("52") == "Y") // CJC 20140506
                {
                    this.axVSPrinter1.FontBold = true;
                    strData = "By Authority of the City Mayor:";
                    this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                    strData = " ";
                    this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);
                    this.axVSPrinter1.Table = string.Format("<5000;{0}", strData);  // RMC 20140102
                    this.axVSPrinter1.MarginLeft = 5700;


                    strData = AppSettingsManager.GetConfigValue("48");
                    this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
                    this.axVSPrinter1.FontBold = false;
                    strData = AppSettingsManager.GetConfigValue("49");
                    this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
                }

                //(s) MCR 20140304 Thank you Letter
                if (m_sBnsStat == "NEW" && m_sTaxYear == "2014")
                {
                    this.axVSPrinter1.NewPage(); //MCR 20140304
                    this.axVSPrinter1.MarginLeft = 1000;
                    this.axVSPrinter1.MarginRight = 1000;
                    this.axVSPrinter1.MarginTop = 1000;
                    this.axVSPrinter1.MarginBottom = 1000;
                    this.axVSPrinter1.FontName = "Arial Narrow";

                    //this.axVSPrinter1.DrawPicture(Properties.Resources.mati_logo2, "0.6in", "0.2in", "25%", "25%", 11, false);
                    //this.axVSPrinter1.DrawPicture(Properties.Resources.ilovemati_logoHighRes, "6.5in", "0.1in", "3.5%", "3.5%", 11, false);

                    this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
                    this.axVSPrinter1.FontSize = (float)10.0;
                    //this.axVSPrinter1.DrawPicture(Properties.Resources.ilovemati_logoHighRes, "2in", "2in", "10%", "10%", 2, false);

                    //this.axVSPrinter1.Table = "^10600;Republic of the Philippines";
                    this.axVSPrinter1.Paragraph = "";
                    //string strProvinceName = AppSettingsManager.GetConfigValue("08");
                    //if (strProvinceName != string.Empty)
                    //    this.axVSPrinter1.Table = string.Format("^9000;{0}", strProvinceName);
                    this.axVSPrinter1.Paragraph = "";

                    this.axVSPrinter1.FontBold = true;
                    //this.axVSPrinter1.Table = string.Format("^10600;{0}", AppSettingsManager.GetConfigValue("09"));
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontName = "Arial";
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                    //this.axVSPrinter1.Table = string.Format("^10600;{0}", "OFFICE OF THE CITY");
                    this.axVSPrinter1.Paragraph = "";
                    //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
                    this.axVSPrinter1.Table = "^10600; ";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    
                    this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Table = "<2000;" + AppSettingsManager.MonthsInWords(AppSettingsManager.GetSystemDate());
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";

                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = "<4000;" + sOwnNm;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Table = "<4000;PROPRIETOR";
                    this.axVSPrinter1.Table = "<4000;" + sBnsNm;
                    this.axVSPrinter1.Table = "<4000;" + Convert.ToString(sBnsAddress.Replace(AppSettingsManager.GetConfigValue("02"), " "));
                    this.axVSPrinter1.Table = "<4000;" + AppSettingsManager.GetConfigValue("02").ToUpperInvariant();
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    if (AppSettingsManager.GetBnsOwnerGender(m_sOwnCode) == "FEMALE")
                        this.axVSPrinter1.Table = "<4000;Dear Ms. " + AppSettingsManager.GetBnsOwnerLastName(m_sOwnCode);
                    else if (AppSettingsManager.GetBnsOwnerGender(m_sOwnCode) == "MALE")
                        this.axVSPrinter1.Table = "<4000;Dear Mr. " + AppSettingsManager.GetBnsOwnerLastName(m_sOwnCode);
                    else
                        this.axVSPrinter1.Table = "<4000;Dear SIR/MADAME";

                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taJustMiddle;
                    this.axVSPrinter1.Paragraph = "Thank you very much for investing and opening up a new business establishment in the City of Mati. We are heartened with this venture as an indication of your confidence in the City and its people to deliver a profitable return of your investment.";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "We would like to reciprocate your trust with the assurance of local government support in your endeavor in the matter of sound business policies and a transparent and fair dealing with you as well as the other business stakeholders in the City of Mati.";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "We are guided by a strong belief in the private as a strong and indispensable partner that will bring about a more vibrant and prosperous economy for all of us. ";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "Again, thank you very much and my warm regards.";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "Very truly yours,";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontBold = true;
                    strData = AppSettingsManager.GetConfigValue("36");
                    this.axVSPrinter1.Table = string.Format("<5000;" + strData);
                    this.axVSPrinter1.FontBold = false;
                    strData = "City Mayor";

                    this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                    this.axVSPrinter1.HdrFontSize = 8;
                    this.axVSPrinter1.HdrFontName = "Arial";
                    this.axVSPrinter1.HdrFontItalic = true;
                    this.axVSPrinter1.Table = string.Format("<5000;" + strData);
                    string sFooter = string.Format("{0}||{1}\n", "City Hall, Nazareno St., Brgy. Central,", "Tel No.(087) 388-3974");
                    sFooter += string.Format("{0,-170}{1}\n", "City of Mati, Davao Oriental", "Fax No. (087) 388-3976", "E-Mail maticitymayor@gmail.com", "www.mati.gov.ph");
                    sFooter += string.Format("{0,-170}{1}", "E-Mail maticitymayor@gmail.com", "www.mati.gov.ph");
                    this.axVSPrinter1.Footer = sFooter;
                }
                // (e) MCR 20140304 Thank You Letter
            }
        }

        private void PrintPermitLubao() // MCR 20150102
        {
            // RMC 20120413 Corrections in printing Permit
            bool blnIsHeadPrint = true;
            double dTotDue = 0;
            OracleResultSet result2 = new OracleResultSet();
            string sDefaultCode = string.Empty;
            string sDefaultDesc = string.Empty;
            string sAmount = string.Empty;
            double dData = 0;
            OracleResultSet pSet = new OracleResultSet();
            string sBnsNm = "";
            string sOwnNm = "";
            string sBnsAddress = "";
            string sBnsMainDesc = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sORNo = "";
            string sOrgnKind = "";
            string sBnsStat = "";
            string sTelNo = "";
            string sDay = "";
            string sCapital = "";
            string sGross = "";
            string sFeeAmt = "";
            string sRemarks = "";
            string sFlrArea = "";
            string sUnitNo = "";
            string sRoomNo = "";
            string sQtrRemarks = "";
            string sORDate = "";
            string sQtrPaid = "";
            string sPlate = "";
            string sCitizenship = "";
            string sQtrsPaid = "";
            string sMemo = "";
            string strData = "";
            //DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpBillDate = m_dtDate;    // RMC 20140129 Added module to input permit date issuance before printing of Permit
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate;
            string sOldBnsCode = "";
            string sFeesAmtDue = "";
            string sPermitFeeCode = "";
            string strSurPen = string.Empty;

            m_bPermitUpdateNoBilling = false;
            sBnsNm = GetDTIBnsName(m_sBIN);
            if (sBnsNm == "")
                sBnsNm = AppSettingsManager.GetBnsName(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwner(sOwnNm);
            sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
            sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);

            sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
            sBnsMainDesc = AppSettingsManager.GetSubBnsDesc(sBnsCode.Substring(0, 2), "B");
            GetOtherBusinessInfo(sBnsCode);

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taJustTop;
            this.axVSPrinter1.CurrentY = 2400;

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)12.0;
            
            string sOrgKind = AppSettingsManager.GetBnsOrgKind(m_sBIN);
            if (m_sBnsStat == "NEW")
            {
                if (m_sPlate.Trim() == "")
                    this.axVSPrinter1.Table = string.Format("^9180;NEW               PERMIT NO. {0}", m_sPermitNo);
                else
                    this.axVSPrinter1.Table = string.Format("^9180;NEW               PERMIT NO. {0}               BUSINESS PLATE NO. {1}", m_sPermitNo, m_sPlate);

                //else if (sOrgKind != "COOPERATIVE")
                //    this.axVSPrinter1.Table = string.Format("^9180;NEW               PERMIT NO. {0}               BUSINESS PLATE NO. {1}", m_sPermitNo, m_sPlate);
                //else
                //    this.axVSPrinter1.Table = string.Format("^9180;NEW               PERMIT NO. {0}", m_sPermitNo);
            }
            else //REN
            {
                if (m_sPlate.Trim() == "")
                    this.axVSPrinter1.Table = string.Format("^9180;RENEWAL               PERMIT NO. {0}", m_sPermitNo);
                else
                    this.axVSPrinter1.Table = string.Format("^9180;RENEWAL               PERMIT NO. {0}               BUSINESS PLATE NO. {1}", m_sPermitNo, m_sPlate);
                //else if (sOrgKind != "COOPERATIVE")
                //    this.axVSPrinter1.Table = string.Format("^9180;RENEWAL               PERMIT NO. {0}               BUSINESS PLATE NO. {1}", m_sPermitNo, m_sPlate);
                //else
                //    this.axVSPrinter1.Table = string.Format("^9180;RENEWAL               PERMIT NO. {0}", m_sPermitNo);
            }

            this.axVSPrinter1.Table = ";;;;;;;";
            // RMC 20150105 mods in permit printing (s)
            if (sOwnNm.Length > 50)
                this.axVSPrinter1.FontSize = 17;    // RMC 20150105 mods in permit printing (e)
            else if (sBnsNm.Length > 25)
                this.axVSPrinter1.FontSize = 20;    // RMC 20150105 mods in permit printing (e)
            else
                this.axVSPrinter1.FontSize = 30;
            //long yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
            this.axVSPrinter1.CurrentY = 4360;
            this.axVSPrinter1.Table = "^10180;" + sBnsNm;//9180
            this.axVSPrinter1.FontSize = (float)12.0;

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = ";;";
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.Table = string.Format("<9000;Business Address: {0}", sBnsAddress);
            if (sOwnNm.Length > 50)
                this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = string.Format("<11500;Taxpayer's Name: {0}", sOwnNm);
            this.axVSPrinter1.FontSize = 12;
            if (sOrgKind == "COOPERATIVE" || sOrgKind == "CORPORATION")
                strData = "Tel/Cellphone No.:" + m_sTelNo + "| Form of Ownership: " + m_sOrgnKind + "|";
            else
                strData = "Tel/Cellphone No.:" + m_sTelNo + "| Form of Ownership: " + m_sOrgnKind + "| Nationality: " + m_sCitizenship;
            this.axVSPrinter1.Table = string.Format("<4000|<4900|<2300;{0}", strData);
            this.axVSPrinter1.Table = ";;;;;;;";

            this.axVSPrinter1.FontSize = 7;
            this.axVSPrinter1.Table = ";;";
            //yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
            this.axVSPrinter1.CurrentY = 8024;  // RMC 20150105 mods in permit printing
            this.axVSPrinter1.FontSize = 12;

            // RMC 20150120 adjust validity of permit based on qtr paid
            string sTmpOrNo = string.Empty;
            string sTmpQtrPaidX = string.Empty;
            string sValidity = string.Empty;

            pSet.Query = "select * from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' ORDER BY or_date desc, QTR_PAID DESC";    // RMC 20150126 ADDED ORDER BY OR_DATE DESC
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sTmpOrNo = pSet.GetString("or_no");
                    sTmpQtrPaidX = pSet.GetString("qtr_paid");

                    if (sTmpQtrPaidX == "1")
                        sValidity = "MARCH 31, ";
                    else if (sTmpQtrPaidX == "2")
                        sValidity = "JUNE 30, ";
                    else if (sTmpQtrPaidX == "3")
                        sValidity = "OCTOBER 31, ";
                    else
                        sValidity = "DECEMBER 31, ";
                }
            }
            pSet.Close();
            // RMC 20150120 

            //this.axVSPrinter1.Table = string.Format(">5300;DECEMBER 31, {0}", ConfigurationAttributes.CurrentYear);
            this.axVSPrinter1.Table = string.Format(">5300;{0}", sValidity + ConfigurationAttributes.CurrentYear); // RMC 20150120
            this.axVSPrinter1.Table = ";;;";

            sPermitFeeCode = GetPermitFeeCode();
            string sFeesDue = string.Empty;
            string sTmpTaxYear = "";
            string sFeesCode = "";
            string sTmpBnsCode = "";
            string sTmpOldBnsCode = "";
            string sDelqTaxYear = "";
            string sDelqTaxYears = "";
            string sTmpDelqTaxYear = "";
            string sTotal = "";
            double dFeesDueFee = 0;
            double dFeesDueBTax = 0;
            double dTotalAmtPaid = 0;
            double dblSurch = 0;
            double dblPen = 0;
            double dblCredit = 0;
            bool bPaidUsingTaxCredit = false;
            double dTotSurPen = 0.0;

            m_dAddlFeesDue = 0;

            int iBnsCount = 0;
            iBnsCount = m_sArrayBnsCode.Length;
            string sTmpQtrPaid = "";
            
            double dTotAmount = 0;

            for (int ix = 1; ix <= iBnsCount; ix++)   // RMC 20150126 PUT REM
            {
                m_dAddlFeesDue = 0; // RMC 20150126
                sTmpQtrPaid = "";
                sDelqTaxYears = string.Empty;
                sTmpBnsCode = m_sArrayBnsCode.GetValue(ix - 1).ToString();
                if (iBnsCount == 1) //MCR 20140429
                    sTmpOldBnsCode = GetOldBnsType(sTmpBnsCode);  // RMC 20140114 mods in Permit if with permit update transaction
                sBnsDesc = AppSettingsManager.GetBnsDesc(sTmpBnsCode);

                dFeesDueFee = 0;
                dFeesDueBTax = 0;
                if (m_bPermitUpdateNoBilling == true && sTmpOldBnsCode != "")
                {
                    sTmpBnsCode = sTmpOldBnsCode;
                    sTmpOldBnsCode = "";
                }

                pSet.Query = "select * from or_table where or_no in ";
                pSet.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
                pSet.Query += " and or_no = '" + sTmpOrNo + "'";    // RMC 20150120 
                pSet.Query += " and bns_code_main = '" + sTmpBnsCode + "'"; // RMC 20150126
                
                //if (sTmpOldBnsCode != "")
                //    pSet.Query += " and bns_code_main = '" + sTmpOldBnsCode + "' and (fees_code = 'B' or fees_code = '01')";
                //else
                //    pSet.Query += " and bns_code_main = '" + sTmpBnsCode + "' and (fees_code = 'B' or fees_code = '01')";
                 
                //pSet.Query += " and tax_year = '" + m_sTaxYear + "' order by tax_year, qtr_paid desc";  // RMC 20140114 mods in Permit if with permit update transaction


                pSet.Query += " order by tax_year, qtr_paid desc";  // RMC 20150107 include all years paid in the permit as per sir juli
                
                string sBnsCodeX = string.Empty;    // RMC 20140114 mods in Permit if with permit update transaction
                if (pSet.Execute())
                {
                    int iCnt = 0;
                    int iCntDelqYear = 0;
                    while (pSet.Read())
                    {
                        iCnt++;
                        sTmpTaxYear = pSet.GetString("tax_year");
                        sFeesCode = pSet.GetString("fees_code");
                        sTmpQtrPaid = pSet.GetString("qtr_paid");

                        sBnsCodeX = pSet.GetString("BNS_CODE_MAIN");   // RMC 20140114 mods in Permit if with permit update transaction (s)

                        if (!bPaidUsingTaxCredit)
                            bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet.GetString("or_no"));

                        if (sTmpTaxYear == m_sTaxYear)
                        {
                            if (sFeesCode == sPermitFeeCode)
                                dFeesDueFee += pSet.GetDouble("fees_due");  // RMC 20140114 mods in Permit if with permit update transaction
                            else
                                dFeesDueBTax += pSet.GetDouble("fees_due"); // RMC 20140114 mods in Permit if with permit update transaction

                            dblSurch += pSet.GetDouble("fees_surch");
                            dblPen += pSet.GetDouble("fees_pen");
                        }
                        else
                        {
                            m_dAddlFeesDue += pSet.GetDouble("fees_due");
                            m_dAddlFeesDue += pSet.GetDouble("fees_surch");
                            m_dAddlFeesDue += pSet.GetDouble("fees_pen");

                            sDelqTaxYear = sTmpTaxYear;

                            if (sTmpDelqTaxYear != sDelqTaxYear)
                            {
                                iCntDelqYear++;

                                if (iCntDelqYear > 1)
                                    sDelqTaxYears += "/ ";
                                sDelqTaxYears += sDelqTaxYear;

                                sTmpDelqTaxYear = sDelqTaxYear;
                            }
                        }
                    }
                    dTotalAmtPaid += dFeesDueBTax;
                }
                pSet.Close();
                GetOrs(sBnsCodeX);  // RMC 20140114 mods in Permit if with permit update transaction
                sFeesAmtDue = string.Format("{0:#,###.00}", dFeesDueBTax);
                sFeesDue = string.Format("{0:#,###.00}", dFeesDueFee);
                strSurPen = string.Format("{0:#,###.00}", dblSurch + dblPen);
                //dTotAmount = dFeesDueBTax + dFeesDueFee;
                dTotAmount += dFeesDueBTax + dFeesDueFee;    // RMC 20150126
                dTotAmount += m_dAddlFeesDue;   // RMC 20150107 include all years paid in the permit as per sir juli
                dTotAmount += dblSurch + dblPen;    // RMC 20150203 include current year surcharge and penalty in permit

                if ((dFeesDueBTax + dFeesDueFee + dblSurch + dblPen) == 0)
                {
                    if (dFeesDueBTax == 0)
                        sFeesAmtDue = "";
                    if (dFeesDueFee == 0)
                        sFeesDue = "";
                    if ((dblSurch + dblPen) == 0)
                        strSurPen = "";
                }

                m_sCapital = AppSettingsManager.GetCapitalGross(m_sBIN, sTmpBnsCode, m_sTaxYear, "NEW");
                m_sGross = AppSettingsManager.GetCapitalGross(m_sBIN, sTmpBnsCode, m_sTaxYear, "REN");

                if (m_sBnsStat == "NEW" || m_sBnsStat == "REN")
                {
                    pSet.Query = "select * from mati_permit_gross where bin = '" + m_sBIN + "' and bns_code_main = '" + sTmpBnsCode + "' and tax_year = '" + m_sTaxYear + "'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            m_sCapital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
                            m_sGross = string.Format("{0:#,##0.00}", pSet.GetDouble("gross"));
                        }
                    }
                    pSet.Close();
                }

                pSet.Query = "select * from other_info where bin = '" + m_sBIN + "' and bns_code = '" + sBnsCode + "' and data <> 0"; // CJC 20131016
                pSet.Query += " and (default_code = '0002' or default_code = '0003' or default_code = '0004' or default_code = '0005' or default_code = '0006')";   // RMC 20140102 adjusted
                if (pSet.Execute())
                {
                    dData = 0;  // RMC 20140102
                    while (pSet.Read())  // RMC 20140102
                    {
                        try
                        {
                            dData += pSet.GetDouble("data"); // RMC 20140102
                        }
                        catch
                        {
                            dData += 0;  // RMC 20140102
                        }
                    }
                }
                pSet.Close();

                strData = " ";
                string sTmpUnit = string.Empty;
                if (dData > 0)
                    sTmpUnit = "- (" + string.Format("{0:0}", dData) + " units)";

                if (sTmpQtrPaid == string.Empty)
                    if (m_sBnsStat == "NEW")
                        this.axVSPrinter1.Table = string.Format("<1700|<3000;" + "|" + sBnsDesc + ";"); // RMC 20140102
                    else
                        this.axVSPrinter1.Table = string.Format("<1700|<3000;" + "|" + sBnsDesc + ";");  // RMC 20140102
                else
                    if (m_sBnsStat == "NEW")
                        this.axVSPrinter1.Table = string.Format("<1700|<3000;" + "|" + sBnsDesc + ";"); // RMC 20140102
                    else
                        this.axVSPrinter1.Table = string.Format("<1700|<3000;" + "|" + sBnsDesc + ";");   // RMC 20140102

                try
                {
                    dTotDue += double.Parse(sFeesDue);
                }
                catch
                {
                    dTotDue += 0;   // RMC 20140110 modified permit if business under incentives - Mati
                }

                try
                {
                    dTotSurPen += double.Parse(strSurPen);
                }
                catch
                {
                    dTotSurPen += 0;    // RMC 20140110 modified permit if business under incentives - Mati
                }
            }

            this.axVSPrinter1.Table = ";;;;;";
            this.axVSPrinter1.FontSize = 50;
            this.axVSPrinter1.Table = "<8000;" + ConfigurationAttributes.CurrentYear;
            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.Table = ";;;;;";
            //long yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
            this.axVSPrinter1.CurrentY = 12816;
            this.axVSPrinter1.Table = "<8000;BIN: " + m_sBIN;
            this.axVSPrinter1.Table = "<8000;O.R. No.: " + m_sOrNo;
            this.axVSPrinter1.Table = "<8000;O.R. Date: " + m_sOrDate;
            this.axVSPrinter1.Table = "<8000;Amount Paid: Php " + dTotAmount.ToString("#,##0.00");
        }

        //private string GetNumEmployees(string sBIN, string sBnsCode, string sTaxYear)
        public string GetNumEmployees(string sBIN, string sBnsCode, string sTaxYear)    // RMC 20140114 modified getting of num of employees
        {
            // RMC 20140106 corrected viewing of no. of employees in permit
            OracleResultSet pSet = new OracleResultSet();

            string sDefaultCode = string.Empty;
            string sNoEmp = string.Empty;

            pSet.Query = "select * from default_code where default_desc like '%OF EMPLOYEE%'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                    sDefaultCode = pSet.GetString("default_code").Trim();
            }
            pSet.Close();


            pSet.Query = string.Format("select * from other_info where bin = '{0}' and tax_year = '{1}' and default_code = '{2}' and bns_code = '{3}'", sBIN, sTaxYear, sDefaultCode, AppSettingsManager.GetBnsCodeMain(m_sBIN));
            if (pSet.Execute())
            {
                if (pSet.Read())
                    sNoEmp = string.Format("{0:#0}", pSet.GetInt("data"));
                else
                    sNoEmp = "0";
            }
            pSet.Close();

            // RMC 20140114 mods in Permit if with permit update transaction (s)
            if (sNoEmp == "0")
            {
                OracleResultSet pRec = new OracleResultSet();
                int iNoEmp = 0;

                pSet.Query = "select * from addl_bns where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "'";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sBnsCode = pSet.GetString("bns_code_main");

                        pRec.Query = string.Format("select * from other_info where bin = '{0}' and tax_year = '{1}' and default_code = '{2}' and bns_code = '{3}'", sBIN, sTaxYear, sDefaultCode, sBnsCode);
                        if (pRec.Execute())
                        {
                            if (pRec.Read())
                                iNoEmp += pRec.GetInt("data");
                        }
                        pRec.Close();
                    }
                }
                pSet.Close();

                sNoEmp = string.Format("{0:#0}", iNoEmp);
            }

            if (sNoEmp == "0")
            {
                int iNoEmp = 0;
                int iTmp = 0;
                string sValue = string.Empty;
                string sAddlCode = string.Empty;
                for (int iCnt = 1; iCnt <= 2; iCnt++)
                {
                    string sTmpSwt = string.Empty;

                    if (iCnt == 1)
                    {
                        pSet.Query = "select * from addl_info_tbl where addl_desc like '%MALE WORKER%' and addl_desc not like '%FEMALE WORKER%'";

                    }
                    else
                    {
                        pSet.Query = "select * from addl_info_tbl where addl_desc like '%FEMALE WORKER%'";

                    }
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                            sAddlCode = pSet.GetString("addl_code");
                    }
                    pSet.Close();

                    pSet.Query = "select * from addl_info where bin = '" + sBIN + "' and addl_code = '" + sAddlCode + "'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            sValue = pSet.GetString("value");
                            int.TryParse(sValue, out iTmp);

                            iNoEmp += iTmp;
                        }
                    }
                    pSet.Close();
                }

                sNoEmp = string.Format("{0:#0}", iNoEmp);
            }
            // RMC 20140114 mods in Permit if with permit update transaction (e)

            return sNoEmp;
        }

        private void ToolPrint_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintQuality = VSPrinter7Lib.PrintQualitySettings.pqHigh;

            if (MessageBox.Show("Are you sure you want to print?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            axVSPrinter1.PrintDoc(1,1,axVSPrinter1.PageCount);
        }

        private void toolSettingPageSetup_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPageSetup);
        }

        private void toolSettingPrintPage_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);
        }


        private void PrintPermitTumauini()
        {
            // RMC 20150425 modified printing of permit for Tumauini
            object objCurrY = "";

            bool blnIsHeadPrint = true;
            double dTotDue = 0;
            OracleResultSet result2 = new OracleResultSet();
            string sDefaultCode = string.Empty;
            string sDefaultDesc = string.Empty;
            string sAmount = string.Empty;
            double dData = 0;
            OracleResultSet pSet = new OracleResultSet();
            string sBnsNm = "";
            string sOwnNm = "";
            string sBnsAddress = "";
            string sBnsMainDesc = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sORNo = "";
            string sOrgnKind = "";
            string sBnsStat = "";
            string sTelNo = "";
            string sDay = "";
            string sCapital = "";
            string sGross = "";
            string sFeeAmt = "";
            string sRemarks = "";
            string sFlrArea = "";
            string sUnitNo = "";
            string sRoomNo = "";
            string sQtrRemarks = "";
            string sORDate = "";
            string sQtrPaid = "";
            string sPlate = "";
            string sCitizenship = "";
            string sQtrsPaid = "";
            string sMemo = "";
            string strData = "";
            //DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpBillDate = m_dtDate;    // RMC 20140129 Added module to input permit date issuance before printing of Permit
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate;
            string sOldBnsCode = "";
            string sFeesAmtDue = "";
            string sPermitFeeCode = "";
            string strSurPen = string.Empty;

            m_bPermitUpdateNoBilling = false;
            sBnsNm = GetDTIBnsName(m_sBIN);
            if (sBnsNm == "")
                sBnsNm = AppSettingsManager.GetBnsName(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwner(sOwnNm);
            sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
            sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);

            sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
            sBnsMainDesc = AppSettingsManager.GetSubBnsDesc(sBnsCode.Substring(0, 2), "B");
            GetOtherBusinessInfo(sBnsCode);

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taJustTop;
            this.axVSPrinter1.CurrentY = 2400;

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)12.0;

            string sOrgKind = AppSettingsManager.GetBnsOrgKind(m_sBIN);
            if (m_sBnsStat == "NEW")
            {
                if (m_sPlate.Trim() == "")
                    this.axVSPrinter1.Table = string.Format("<9180;NEW               PERMIT NO. {0}", m_sPermitNo);
                else
                    this.axVSPrinter1.Table = string.Format("<9180;NEW               PERMIT NO. {0}               BUSINESS PLATE NO. {1}", m_sPermitNo, m_sPlate);

            }
            else //REN
            {
                if (m_sPlate.Trim() == "")
                    this.axVSPrinter1.Table = string.Format("<9180;RENEWAL               PERMIT NO. {0}", m_sPermitNo);
                else
                    this.axVSPrinter1.Table = string.Format("<9180;RENEWAL               PERMIT NO. {0}               BUSINESS PLATE NO. {1}", m_sPermitNo, m_sPlate);

            }

            this.axVSPrinter1.Table = ";;;;;;;";

            if (sBnsNm.Length > 50)
                this.axVSPrinter1.FontSize = 17;    // RMC 20150105 mods in permit printing (e)
            else if (sBnsNm.Length > 25)
                this.axVSPrinter1.FontSize = 20;    // RMC 20150105 mods in permit printing (e)
            else
                this.axVSPrinter1.FontSize = 30;
            //long yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
            this.axVSPrinter1.CurrentY = 4360;
            this.axVSPrinter1.Table = "^10180;" + sBnsNm;//9180
            this.axVSPrinter1.FontSize = (float)12.0;

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = ";;";
            this.axVSPrinter1.FontName = "Arial Narrow";
            
            /*this.axVSPrinter1.Table = string.Format("<9000;Business Address: {0}", sBnsAddress);
            if (sOwnNm.Length > 50)
                this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = string.Format("<11500;Taxpayer's Name: {0}", sOwnNm);
            this.axVSPrinter1.FontSize = 12;
            if (sOrgKind == "COOPERATIVE" || sOrgKind == "CORPORATION")
                strData = "Tel/Cellphone No.:" + m_sTelNo + "| Form of Ownership: " + m_sOrgnKind + "|";
            else
                strData = "Tel/Cellphone No.:" + m_sTelNo + "| Form of Ownership: " + m_sOrgnKind + "| Nationality: " + m_sCitizenship;
            this.axVSPrinter1.Table = string.Format("<4000|<4900|<2300;{0}", strData);*/

            object lng1;
            lng1 = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = string.Format("<2000|<7000;Business Address:");
            this.axVSPrinter1.Table = string.Format("<2000|<7000;Taxpayer's Name:");
            this.axVSPrinter1.Table = string.Format("<2000|<2000|<2000|<2900|<1300|<1300;Tel/Cellphone No.:||Form of Ownership:||Nationality:|");
            this.axVSPrinter1.CurrentY = lng1;
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.Table = string.Format("<2000|<7000;|" + sBnsAddress);
            if (sOwnNm.Length > 50)
                this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = string.Format("<2000|<7000;|" + sOwnNm);
            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.Table = string.Format("<2000|<2000|<2000|<2900|<1300|<1300;|" + m_sTelNo + "||" + m_sOrgnKind + "||" + m_sCitizenship);
            this.axVSPrinter1.FontUnderline = false;

            this.axVSPrinter1.FontSize = 7;
            this.axVSPrinter1.Table = ";;";
            this.axVSPrinter1.CurrentY = 7024;
            this.axVSPrinter1.FontSize = 12;

            sPermitFeeCode = GetPermitFeeCode();
            string sFeesDue = string.Empty;
            string sTmpTaxYear = "";
            string sFeesCode = "";
            string sTmpBnsCode = "";
            string sTmpOldBnsCode = "";
            string sDelqTaxYear = "";
            string sDelqTaxYears = "";
            string sTmpDelqTaxYear = "";
            string sTotal = "";
            double dFeesDueFee = 0;
            double dFeesDueBTax = 0;
            double dTotalAmtPaid = 0;
            double dblSurch = 0;
            double dblPen = 0;
            double dblCredit = 0;
            bool bPaidUsingTaxCredit = false;
            double dTotSurPen = 0.0;

            m_dAddlFeesDue = 0;

            int iBnsCount = 0;
            iBnsCount = m_sArrayBnsCode.Length;
            string sTmpQtrPaid = "";

            double dTotAmount = 0;

            string sTmpOrNo = string.Empty;
            string sTmpQtrPaidX = string.Empty;
            
            double addTo = 0;
            string sValidity = string.Empty;
            OracleResultSet amount = new OracleResultSet(); //JARS20151111

            string strORNum = string.Empty; // AST 20160127
            string strORDate = string.Empty; // AST 20160127

            pSet.Query = "select * from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' ORDER BY or_date desc, QTR_PAID DESC";    // RMC 20150126 ADDED ORDER BY OR_DATE DESC
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sTmpOrNo = pSet.GetString("or_no");
                    sTmpQtrPaidX = pSet.GetString("qtr_paid");
                    sTmpTaxYear = pSet.GetString("tax_year");
                    amount.Query = "select sum(fees_amtdue) from or_table where or_no = '" + sTmpOrNo + "' and tax_year = '" + sTmpTaxYear + "'";
                    if (amount.Execute())
                    {
                        if (amount.Read())
                        {
                            addTo = pSet.GetDouble(0);
                            dTotAmount += addTo;
                        }
                    }
                }
            }
            pSet.Close();
            //checkpoint
            
            for (int ix = 1; ix <= iBnsCount; ix++)
            {
                m_dAddlFeesDue = 0;
                sTmpQtrPaid = "";
                sDelqTaxYears = string.Empty;
                sTmpBnsCode = m_sArrayBnsCode.GetValue(ix - 1).ToString();
                if (iBnsCount == 1)
                    sTmpOldBnsCode = GetOldBnsType(sTmpBnsCode);
                sBnsDesc = AppSettingsManager.GetBnsDesc(sTmpBnsCode);

                dFeesDueFee = 0;
                dFeesDueBTax = 0;
                if (m_bPermitUpdateNoBilling == true && sTmpOldBnsCode != "")
                {
                    sTmpBnsCode = sTmpOldBnsCode;
                    sTmpOldBnsCode = "";
                }

                pSet.Query = "select * from or_table where or_no in ";
                pSet.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
                //pSet.Query += " and or_no = '" + sTmpOrNo + "'";  // RMC 20150504 QA corrections, put rem
                pSet.Query += " and bns_code_main = '" + sTmpBnsCode + "'";
                pSet.Query += " order by tax_year, qtr_paid desc";

                string sBnsCodeX = string.Empty;
                if (pSet.Execute())
                {
                    int iCnt = 0;
                    int iCntDelqYear = 0;
                    while (pSet.Read())
                    {
                        iCnt++;
                        sTmpTaxYear = pSet.GetString("tax_year");
                        sFeesCode = pSet.GetString("fees_code");
                        sTmpQtrPaid = pSet.GetString("qtr_paid");

                        sBnsCodeX = pSet.GetString("BNS_CODE_MAIN");

                        if (!bPaidUsingTaxCredit)
                            bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet.GetString("or_no"));

                        if (sTmpTaxYear == m_sTaxYear)
                        {
                            if (sFeesCode == sPermitFeeCode)
                                dFeesDueFee += pSet.GetDouble("fees_due");
                            else
                                dFeesDueBTax += pSet.GetDouble("fees_due");

                            dblSurch += pSet.GetDouble("fees_surch");
                            dblPen += pSet.GetDouble("fees_pen");
                        }
                        else
                        {
                            m_dAddlFeesDue += pSet.GetDouble("fees_due");
                            m_dAddlFeesDue += pSet.GetDouble("fees_surch");
                            m_dAddlFeesDue += pSet.GetDouble("fees_pen");

                            sDelqTaxYear = sTmpTaxYear;

                            if (sTmpDelqTaxYear != sDelqTaxYear)
                            {
                                iCntDelqYear++;

                                if (iCntDelqYear > 1)
                                    sDelqTaxYears += "/ ";
                                sDelqTaxYears += sDelqTaxYear;

                                sTmpDelqTaxYear = sDelqTaxYear;
                            }
                        }
                    }
                    dTotalAmtPaid += dFeesDueBTax;
                }
                pSet.Close();
                GetOrs(sBnsCodeX);  // RMC 20140114 mods in Permit if with permit update transaction

                // AST 20160127 (s)
                if (!string.IsNullOrEmpty(m_sOrNo.Trim()))
                    strORNum = m_sOrNo;
                if (!string.IsNullOrEmpty(m_sOrDate.Trim()))
                    strORDate = m_sOrDate;
                // AST 20160127 (e)

                sFeesAmtDue = string.Format("{0:#,###.00}", dFeesDueBTax);
                sFeesDue = string.Format("{0:#,###.00}", dFeesDueFee);
                strSurPen = string.Format("{0:#,###.00}", dblSurch + dblPen);
                //dTotAmount = dFeesDueBTax + dFeesDueFee;
                dTotAmount += dFeesDueBTax + dFeesDueFee;    // RMC 20150126
                dTotAmount += m_dAddlFeesDue;   // RMC 20150107 include all years paid in the permit as per sir juli
                dTotAmount += dblSurch + dblPen;    // RMC 20150203 include current year surcharge and penalty in permit

                if ((dFeesDueBTax + dFeesDueFee + dblSurch + dblPen) == 0)
                {
                    if (dFeesDueBTax == 0)
                        sFeesAmtDue = "";
                    if (dFeesDueFee == 0)
                        sFeesDue = "";
                    if ((dblSurch + dblPen) == 0)
                        strSurPen = "";
                }

                m_sCapital = AppSettingsManager.GetCapitalGross(m_sBIN, sTmpBnsCode, m_sTaxYear, "NEW");
                m_sGross = AppSettingsManager.GetCapitalGross(m_sBIN, sTmpBnsCode, m_sTaxYear, "REN");

                if (m_sBnsStat == "NEW" || m_sBnsStat == "REN")
                {
                    pSet.Query = "select * from mati_permit_gross where bin = '" + m_sBIN + "' and bns_code_main = '" + sTmpBnsCode + "' and tax_year = '" + m_sTaxYear + "'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            m_sCapital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
                            m_sGross = string.Format("{0:#,##0.00}", pSet.GetDouble("gross"));
                        }
                    }
                    pSet.Close();
                }

                pSet.Query = "select * from other_info where bin = '" + m_sBIN + "' and bns_code = '" + sBnsCode + "' and data <> 0"; // CJC 20131016
                pSet.Query += " and (default_code = '0002' or default_code = '0003' or default_code = '0004' or default_code = '0005' or default_code = '0006')";   // RMC 20140102 adjusted
                if (pSet.Execute())
                {
                    dData = 0;  // RMC 20140102
                    while (pSet.Read())  // RMC 20140102
                    {
                        try
                        {
                            dData += pSet.GetDouble("data"); // RMC 20140102
                        }
                        catch
                        {
                            dData += 0;  // RMC 20140102
                        }
                    }
                }
                pSet.Close();

                strData = " ";
                string sTmpUnit = string.Empty;
                if (dData > 0)
                    sTmpUnit = "- (" + string.Format("{0:0}", dData) + " units)";


                if (ix == 1)
                {
                    this.axVSPrinter1.Table = string.Format("<3000;LINE/S OF BUSINESS:");
                    objCurrY = axVSPrinter1.CurrentY;
                }

                if (ix == 7) // AST 20160126 
                {
                    axVSPrinter1.CurrentY = objCurrY;

                    if (sTmpQtrPaid == string.Empty)
                        if (m_sBnsStat == "NEW")
                            this.axVSPrinter1.Table = string.Format("<1700|<5000|<5000;" + "||" + sBnsDesc + ";");
                        else
                            this.axVSPrinter1.Table = string.Format("<1700|<5000|<5000;" + "||" + sBnsDesc + ";");
                    else
                        if (m_sBnsStat == "NEW")
                            this.axVSPrinter1.Table = string.Format("<1700|<5000|<5000;" + "||" + sBnsDesc + ";");
                        else
                            this.axVSPrinter1.Table = string.Format("<1700|<5000|<5000;" + "||" + sBnsDesc + ";");
                }
                else
                {
                    if (sTmpQtrPaid == string.Empty)
                        if (m_sBnsStat == "NEW")
                            this.axVSPrinter1.Table = string.Format("<1700|<5000;" + "|" + sBnsDesc + ";");
                        else
                            this.axVSPrinter1.Table = string.Format("<1700|<5000;" + "|" + sBnsDesc + ";");
                    else
                        if (m_sBnsStat == "NEW")
                            this.axVSPrinter1.Table = string.Format("<1700|<5000;" + "|" + sBnsDesc + ";");
                        else
                            this.axVSPrinter1.Table = string.Format("<1700|<5000;" + "|" + sBnsDesc + ";");
                }

                try
                {
                    dTotDue += double.Parse(sFeesDue);
                }
                catch
                {
                    dTotDue += 0;
                }

                try
                {
                    dTotSurPen += double.Parse(strSurPen);
                }
                catch
                {
                    dTotSurPen += 0;
                }
               
            }

            
            
            this.axVSPrinter1.CurrentY = 8816;
            sRemarks = "PERMIT is hereby granted to the above mentioned person/partnership/corporation to engage in the";
            sRemarks += " above stated business subject to the payment of the required Municipal License and Permit fees";
            sRemarks += " in compliance with ordinances and regulations governing the business or trade.";
            this.axVSPrinter1.Table = string.Format("^10000;;" + sRemarks);

            string sCurrentDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
            //sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate()); // AST 20160106 change curr date to issuance dt
            //sDay = string.Format("{0:dd}", dtpBillDate); // AST 20160106 change curr date to issuance dt
            sDay = string.Format("{0}", dtpBillDate.Day); // MCR 20190710 .day

            //sRemarks = "Given this " + ConvertDayInOrdinalForm(sDay) + " day of " + Convert.ToDateTime(sCurrentDate).ToString("MMMM yyyy") + " at " + AppSettingsManager.GetConfigValue("09") + ", " + AppSettingsManager.GetConfigValue("08") + "."; // AST 20160204
            sRemarks = "Given this " + ConvertDayInOrdinalForm(sDay) + " day of " + dtpBillDate.ToString("MMMM yyyy") + " at " + AppSettingsManager.GetConfigValue("09") + ", " + AppSettingsManager.GetConfigValue("08") + ".";
            this.axVSPrinter1.Table = string.Format("^10000;;" + sRemarks);            
            
            if (sTmpQtrPaidX == "F")
                sTmpQtrPaidX = "Full Year";
            else
                sTmpQtrPaidX = "Quarterly";

            this.axVSPrinter1.CurrentY = 10816;
            this.axVSPrinter1.FontSize = 50;

            /*this.axVSPrinter1.DrawLine(500, 10816, 2500, 10816);
            this.axVSPrinter1.DrawLine(500, 12016, 2500, 12016);
            this.axVSPrinter1.DrawLine(500, 10816, 500, 12010);
            this.axVSPrinter1.DrawLine(2500, 10816, 2500, 12016);*/
            this.axVSPrinter1.FontName = "Times New Roman";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.BrushColor = Color.Yellow;
            this.axVSPrinter1.PenColor = Color.Yellow;
            
            this.axVSPrinter1.DrawRectangle(500, 10816, 2700, 12010);
            this.axVSPrinter1.PenColor = Color.Black;

            this.axVSPrinter1.Table = "<8000;" + ConfigurationAttributes.CurrentYear;
            this.axVSPrinter1.FontName = "Times New Roman";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.FontName = "Arial Narrow";

            this.axVSPrinter1.CurrentY = 12816;
            this.axVSPrinter1.Table = "<2300|<2500;BIN:|" + m_sBIN;
            //this.axVSPrinter1.Table = "<2300|<2500;O.R. No.:|" + m_sOrNo;
            //this.axVSPrinter1.Table = "<2300|<2500;O.R. Date:|" + m_sOrDate;
            this.axVSPrinter1.Table = "<2300|<2500;O.R. No.:|" + strORNum;
            this.axVSPrinter1.Table = "<2300|<2500;O.R. Date:|" + strORDate;
            //this.axVSPrinter1.Table = "<2300|<2500;Amount Paid (Current Year):|Php " + dTotAmount.ToString("#,##0.00");
            this.axVSPrinter1.Table = "<2300|<2500;Amount Paid (Current Year):|Php " + dTotAmount.ToString("#,##0.00");
            this.axVSPrinter1.Table = "<2300|<2500;Mode of Payment:|" + sTmpQtrPaidX;
        }

        private string ConvertDayInOrdinalForm(string sDay)
        {
            // RMC 20150105 mods in permit printing
            string sLastNo = "";
            string sDayth = "";

            sLastNo = StringUtilities.Right(sDay, 1);

            if (Convert.ToInt32(sDay) > 9 && Convert.ToInt32(sDay) < 20)
            {
                sDayth = sDay + "th";
            }
            else
            {
                switch (Convert.ToInt32(sLastNo))
                {

                    case 1: sDayth = sDay + "st"; break;
                    case 2: sDayth = sDay + "nd"; break;
                    case 3: sDayth = sDay + "rd"; break;
                    default: sDayth = sDay + "th"; break;
                }
            }

            return sDayth;
        }

        private void PrintPermitMalolos() //JARS 20170815
        {
            OracleResultSet pRec = new OracleResultSet();
            string sGrossCap = "";
            string sGross = "";
            string sCap = "";
            string sPaymentTerm = "";
            string sAssessedTax = "";
            string sAmountPaid = "";
            string sTerm = "";
            string sTime = "";
            string sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);
            string sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
            long Y;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)12.0;

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.FontBold = true;

            pRec.Query = string.Format("select * from businesses where bin = '{0}'", m_sBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    m_sBnsStat = pRec.GetString("bns_stat");
                }
            }
            pRec.Close();

            //PERMIT NUMBER
            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.CurrentY = 900;
            this.axVSPrinter1.MarginLeft = 9200;
            this.axVSPrinter1.Table = "<2000;" + m_sPermitNo;
            //BUSINESS NAME, ADDRESS, OWNER NAME
            this.axVSPrinter1.MarginLeft = 2700;
            this.axVSPrinter1.CurrentY = 3800;
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (s)
            if(AppSettingsManager.GetBnsName(m_sBIN).Length > 70)
                this.axVSPrinter1.Table = "<7200;" + AppSettingsManager.GetBnsName(m_sBIN);
            else
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (e)
            this.axVSPrinter1.Table = "<11000;" + AppSettingsManager.GetBnsName(m_sBIN);
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = 4700;
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (s)
            if(AppSettingsManager.GetBnsAddress(m_sBIN).Length > 66)
                this.axVSPrinter1.FontSize = (float)10.0;
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (e)
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetBnsAddress(m_sBIN);
            this.axVSPrinter1.FontSize = (float)12.0;   // RMC 20171201 adjust printing in permit if data character exceeds the space provided
            this.axVSPrinter1.CurrentY = 5000;
            string sBnsOwnCode = string.Empty;
            sBnsOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            this.axVSPrinter1.MarginLeft = 5700;
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (s)
            if (AppSettingsManager.GetBnsOwner(sBnsOwnCode).Length > 48)
            {
                this.axVSPrinter1.FontSize = (float)10.0;
                this.axVSPrinter1.Table = "<5300;" + AppSettingsManager.GetBnsOwner(sBnsOwnCode);
            }
            else
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (e)
                this.axVSPrinter1.Table = "<11000;" + AppSettingsManager.GetBnsOwner(sBnsOwnCode);
            this.axVSPrinter1.FontSize = (float)12.0;   // RMC 20171201 adjust printing in permit if data character exceeds the space provided

            //pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' order by bns_code_main";
            pRec.Query = "select distinct a.* from addl_bns a where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' order by bns_code_main"; // RMC 20180115 correction in permit printing of double bns type
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBnsCode = pRec.GetString("bns_code_main");
                    sBnsDesc += "/ " + AppSettingsManager.GetBnsDesc(sBnsCode);
                }
            }
            pRec.Close();

            //LINE OF BUSINESS
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = 6900;
            this.axVSPrinter1.Table = "^11000;" + sBnsDesc;

            string sAmtPaid = string.Empty;
            GetPaymentInfo(m_sBIN, m_sTaxYear, out m_sOrNo, out m_sOrDate, out sAmtPaid);

            //BILL DATE
            string sDay = string.Empty;
            DateTime dtpBillDate = m_dtDate;
            //sDay = string.Format("{0:dd}", dtpBillDate);
            sDay = string.Format("{0}", dtpBillDate.Day); // MCR 20190710 .day
            this.axVSPrinter1.CurrentY = 7500;
            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.Table = "^1000|^1000|^2000;" + ConvertDayInOrdinalForm(sDay) + "||" + dtpBillDate.ToString("MMMM, yyyy");


            //EXPIRATION DATE
            this.axVSPrinter1.CurrentY = 8300;
            this.axVSPrinter1.MarginLeft = 6500;
            this.axVSPrinter1.Table = "^2000;" + "December 31 " + dtpBillDate.ToString("yyyy");

            //ATTY PIRMA
            axVSPrinter1.DrawPicture(Properties.Resources.pirma, "5in", "6.4in", "70%", "70%", 10, false); //MCR 20161221 image path, x, y, witdh, height, shade

            //PAYMENT DETAILS
            #region queries
            if (m_sBnsStat == "NEW")
            {
                pRec.Query = "select capital as grosscap from businesses where bin = '"+ m_sBIN +"'";
            }
            else
            {
                pRec.Query = "select gr_1 as grosscap from businesses where bin = '"+ m_sBIN +"'";
            }

            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sGrossCap = pRec.GetDouble("grosscap").ToString("#,###.00");
                }
            }
            pRec.Close();

            pRec.Query = "select sum(fees_amtdue) as fees_amtdue from or_table where or_no = '"+ m_sOrNo +"'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sAmountPaid = pRec.GetDouble("fees_amtdue").ToString("#,###.00");
                }
            }
            pRec.Close();

            pRec.Query = "select sum(amount) as amount from taxdues_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sAssessedTax = pRec.GetDouble("amount").ToString("#,###.00");
                }
            }
            pRec.Close();

            //pRec.Query = "select qtr_paid from pay_hist where bin = '"+m_sBIN+"' and or_no = '"+m_sOrNo+"'";
            pRec.Query = "select payment_term from pay_hist where bin = '" + m_sBIN + "' and or_no = '" + m_sOrNo + "'";    // RMC 20180112 corrected display of payment term in permit
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    //sTerm = pRec.GetString("qtr_paid");
                    sTerm = pRec.GetString(0);  // RMC 20180112 corrected display of payment term in permit
                }
            }
            pRec.Close();

            if (sTerm == "F")
            {
                sTerm = "FULL";
            }
            else
            {
                sTerm = "QUARTERLY";
            }

            //FOR CURRENT Y
            Y = 11000;
            pRec.Query = "select * from addl_bns where bin ='"+ m_sBIN +"' and tax_year = '"+ m_sTaxYear +"'";
            if(pRec.Execute())
            {
                while(pRec.Read())
                {
                    Y = Y - 200;
                }
            }

            #endregion
            this.axVSPrinter1.FontBold = false;
            sTime = string.Format("{0:hh:mm:ss:tt}", dtpBillDate);
            this.axVSPrinter1.CurrentY = 12200;
            this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.FontSize = (float)8.5;
            long lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "GROSS/CAPITAL: " + sGrossCap + "||" + "AMOUNT PAID: " + sAmountPaid;
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "GROSS/CAPITAL: " + sGrossCap + "||" + "AMOUNT PAID: " + sAmtPaid; // RMC 20171129 corrected display of OR amount in Permit
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "ASSESSED TAX: " + sAssessedTax + "||" + "OR NUMBER: " + m_sOrNo;
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "PAYMENT TERM: " + sTerm + "||" + "OR DATE: " + m_sOrDate;
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);

            string sPlate = string.Empty;
            pRec.Query = String.Format("select * from buss_plate where bin = '{0}'", m_sBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sPlate = pRec.GetString("bns_plate");
                }
            }
            pRec.Close();

            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "PLATE NUMBER: " + sPlate + "||" + m_sBnsStat;
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "BIN: " + m_sBIN + "||" + "TIME GENERATED: " + sTime;
            this.axVSPrinter1.Table = "<3000|^5000|<3500;|||";
            //this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "||" + "||" + "GENERATED BY: " + AppSettingsManager.SystemUser.UserCode;
            //this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "||" + "||" + "DATE GENERATED: " + dtpBillDate;

            //JHB 20181023 add Printed by & Date printed
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "||" + "GENERATED BY: " + AppSettingsManager.SystemUser.UserCode;
            sTime = String.Format("{0:MMMM d, yyyy}", dtpBillDate);
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "||" + "DATE GENERATED: " + sTime;

            //ADDITIONAL LINES OF BUSINESS
            pRec.Query = "select distinct a.bns_code, a.bns_desc, b.capital, b.gross from bns_table a right join addl_bns b ";
            pRec.Query += "on a.bns_code = b.bns_code_main where a.fees_code = 'B' and b.tax_year = '" + m_sTaxYear + "' and b.bin = '" + m_sBIN + "'";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBnsDesc = AppSettingsManager.GetBnsDesc(pRec.GetString("bns_code"));
                    sCap = pRec.GetDouble("capital").ToString("#,##0.00");
                    sGross = pRec.GetDouble("gross").ToString("#,###0.00");
                    this.axVSPrinter1.Table = "<3000|^5000|<3500;" + sBnsDesc + "||" + "GROSS/CAPITAL: " + sGross + "/" + sCap;
                }
            }
            pRec.Close();

            this.axVSPrinter1.Table = "<3000|^5000|<3500;|||";
            this.axVSPrinter1.Table = "<11000;Renew this business permit on January 2 to 20";
        }

        private void PrintPermitBinan()
        {
            // RMC 20161204 customized permit for Binan
            OracleResultSet pRec = new OracleResultSet();
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.MarginLeft = 500;

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11000;BPLO-010-0";
            this.axVSPrinter1.FontBold = true;

            this.axVSPrinter1.CurrentY = 2800;//3000;
            this.axVSPrinter1.FontSize = (float)24.0;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^11000;" + m_sTaxYear;

            pRec.Query = string.Format("select * from businesses where bin = '{0}'", m_sBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    m_sBnsStat = pRec.GetString("bns_stat");
                }
            }
            pRec.Close();
                        
            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.CurrentY = 5000;//5200;//5400;
            this.axVSPrinter1.MarginLeft = 2700;
            this.axVSPrinter1.Table = "<4000|<3000|>1000;" + m_sPermitNo + "|BIN: " + m_sBIN + "|" + m_sBnsStat;
            
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = 5700;//5800;//6000;
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetBnsName(m_sBIN);
            this.axVSPrinter1.CurrentY = 6700;//6800;//7000;
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetBnsAddress(m_sBIN);
            this.axVSPrinter1.CurrentY = 7800;//8000;
            string sBnsOwnCode = string.Empty;
            sBnsOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetBnsOwner(sBnsOwnCode);
            this.axVSPrinter1.CurrentY = 8800;//9000;  //nature of business

            
            string sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);
            string sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);

            //AppSettingsManager.GetLastPaymentInfo(m_sBIN, AppSettingsManager.GetLastPaymentInfo(m_sBIN, "OR"), out sLastPaidYear, out sLastPaidQtr, out sLastBnsStat);

            pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' order by bns_code_main";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBnsCode = pRec.GetString("bns_code_main");
                    sBnsDesc += "/ " + AppSettingsManager.GetBnsDesc(sBnsCode);
                }
            }
            pRec.Close();
            this.axVSPrinter1.Table = "^11000;" + sBnsDesc;

            string sAmtPaid = string.Empty;
            GetPaymentInfo(m_sBIN, m_sTaxYear, out m_sOrNo, out m_sOrDate, out sAmtPaid);

            this.axVSPrinter1.CurrentY = 10450;//10650;//11000//10000
            this.axVSPrinter1.MarginLeft = 5000;//5100;//5600
            this.axVSPrinter1.Table = "<1000;" + m_sTaxYear;
            string sDay = string.Empty;
            DateTime dtpBillDate = m_dtDate; 
            //sDay = string.Format("{0:dd}", dtpBillDate);
            sDay = string.Format("{0}", dtpBillDate.Day); // MCR 20190710 .day
            this.axVSPrinter1.MarginLeft = 5400;
            //this.axVSPrinter1.Table = "^1000|^300|^2000;" + ConvertDayInOrdinalForm(sDay) + "||" + dtpBillDate.ToString("MMMM, yyyy");
            this.axVSPrinter1.Table = "^1000|^280|^2000;" + ConvertDayInOrdinalForm(sDay) + "||" + dtpBillDate.ToString("MMMM, yyyy");
            this.axVSPrinter1.MarginLeft = 6000;
            long lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "^2000;" + m_sOrNo;
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "^2000;" + m_sOrDate;
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "^2000;" + sAmtPaid;

            //axVSPrinter1.DrawPicture(Properties.Resources.DIMAGUILA_WALFREDO_SIGN_2, "4in", "8.75in", "27%", "18%", 10, false); //MCR 20161221 image path, x, y, witdh, height, shade
            axVSPrinter1.DrawPicture(Properties.Resources.DIMAGUILA_WALFREDO_SIGN_2, "4.5in", "8.75in", "27%", "18%", 10, false); //MCR 20161221 image path, x, y, witdh, height, shade
            
            /*Point ulCorner = new Point(100,100);
            axVSPrinter1.DrawPicture(newImage,0,0);*/
            /*Image newImage = Properties.Resources.DIMAGUILA_WALFREDO_SIGN_2;

            axVSPrinter1.X1 = 1000;
            axVSPrinter1.Y1 = 1000;
            axVSPrinter1.X2 = 2400;
            axVSPrinter1.Y2 = 2400;
            axVSPrinter1.BrushStyle = VSPrinter7Lib.BrushStyleSettings.bsTransparent;
            axVSPrinter1.Picture = newImage;*/
        }
            
        public void GetPaymentInfo(string sBin, string sTaxYear, out string sOrNo, out string sOrDate, out string sAmtPaid)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();
            sOrNo = "";
            sOrDate = "";
            sAmtPaid = "";

            try
            {
                //pRec.Query = "select distinct (or_no), or_date from pay_hist where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and data_mode <> 'UNP' order by or_date";
                pRec.Query = "select distinct (or_no), or_date from pay_hist where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and data_mode <> 'UNP' and qtr_paid <> 'A' order by or_date"; // RMC 20180125 correction of payment term in permit printing
                if (pRec.Execute())
                {

                    string sTmpOrNo = string.Empty;

                    while (pRec.Read())
                    {
                        if (sOrNo != "")
                        {
                            sOrNo += "/ ";
                            sOrDate += "/ ";
                            sAmtPaid += "/ ";
                        }

                        sTmpOrNo = pRec.GetString(0);
                        sOrNo += sTmpOrNo;
                        sOrDate += string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime(1));

                        double dAmtPaid = 0;
                        //pSet.Query = "select sum(fees_amtdue) from or_table where or_no = '" + sTmpOrNo + "' and tax_year = '" + sTaxYear + "'";
                        pSet.Query = "select sum(fees_amtdue) from or_table where or_no = '" + sTmpOrNo + "'";  // RMC 20180112 display full amount paid in permit, same value at OR requested by MALOLOS
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                                dAmtPaid = pSet.GetDouble(0);
                        }
                        pSet.Close();

                        sAmtPaid += string.Format("{0:#,###.00}", dAmtPaid);
                    }
                }
                pRec.Close();
            }
            catch { }
        }

        private void PrintTemporaryPermit()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sGrossCap = "";
            string sGross = "";
            string sCap = "";
            string sPaymentTerm = "";
            string sAssessedTax = "";
            string sAmountPaid = "";
            string sTerm = "";
            string sTime = "";
            string sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);
            string sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
            long Y;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)12.0;

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.FontBold = true;

            pRec.Query = string.Format("select * from businesses where bin = '{0}'", m_sBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    m_sBnsStat = pRec.GetString("bns_stat");
                }
            }
            pRec.Close();

            //PERMIT NUMBER
            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.CurrentY = 900;
            this.axVSPrinter1.MarginLeft = 9200;
            this.axVSPrinter1.Table = "<2000;TEMP-" + m_sPermitNo;
            //BUSINESS NAME, ADDRESS, OWNER NAME
            this.axVSPrinter1.MarginLeft = 2700;
            this.axVSPrinter1.CurrentY = 3800;
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (s)
            if(AppSettingsManager.GetBnsName(m_sBIN).Length > 70)
                this.axVSPrinter1.Table = "<7200;" + AppSettingsManager.GetBnsName(m_sBIN);
            else
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (e)
                this.axVSPrinter1.Table = "<11000;" + AppSettingsManager.GetBnsName(m_sBIN);
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = 4700;
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (s)
            if (AppSettingsManager.GetBnsAddress(m_sBIN).Length > 66)
                this.axVSPrinter1.FontSize = (float)10.0;
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (e)
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetBnsAddress(m_sBIN);
            this.axVSPrinter1.FontSize = (float)12.0;   // RMC 20171201 adjust printing in permit if data character exceeds the space provided
            this.axVSPrinter1.CurrentY = 5000;
            string sBnsOwnCode = string.Empty;
            sBnsOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            this.axVSPrinter1.MarginLeft = 5700;
            // RMC 20171201 adjust printing in permit if data character exceeds the space provided (s)
            if (AppSettingsManager.GetBnsOwner(sBnsOwnCode).Length > 48)
            {
                this.axVSPrinter1.FontSize = (float)10.0;
                this.axVSPrinter1.Table = "<5300;" + AppSettingsManager.GetBnsOwner(sBnsOwnCode);
            }
            else
                // RMC 20171201 adjust printing in permit if data character exceeds the space provided (e)
                this.axVSPrinter1.Table = "<11000;" + AppSettingsManager.GetBnsOwner(sBnsOwnCode);
            this.axVSPrinter1.FontSize = (float)12.0;   // RMC 20171201 adjust printing in permit if data character exceeds the space provided

            pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' order by bns_code_main";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBnsCode = pRec.GetString("bns_code_main");
                    sBnsDesc += "/ " + AppSettingsManager.GetBnsDesc(sBnsCode);
                }
            }
            pRec.Close();

            //LINE OF BUSINESS
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = 6900;
            this.axVSPrinter1.Table = "^11000;" + sBnsDesc;

            string sAmtPaid = string.Empty;
            GetPaymentInfo(m_sBIN, m_sTaxYear, out m_sOrNo, out m_sOrDate, out sAmtPaid);

            //BILL DATE
            string sDay = string.Empty;
            //DateTime dtpBillDate = m_dtDate;
            DateTime dtpBillDate = AppSettingsManager.GetCurrentDate(); // RMC 20180104 correction in temp permit issued date
            //sDay = string.Format("{0:dd}", dtpBillDate);
            sDay = string.Format("{0}", dtpBillDate.Day); // MCR 20190710 .day
            this.axVSPrinter1.CurrentY = 7500;
            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.Table = "^1000|^1000|^2000;" + ConvertDayInOrdinalForm(sDay) + "||" + dtpBillDate.ToString("MMMM, yyyy");
            //this.axVSPrinter1.Table = "^1000|^1000|^2000;";

            dtpBillDate = m_dtDate; // RMC 20180104 correction in temp permit issued date
            //sDay = string.Format("{0:dd}", dtpBillDate);     // RMC 20180104 correction in temp permit issued date  
            sDay = string.Format("{0}", dtpBillDate.Day); // MCR 20190710 .day
            //EXPIRATION DATE
            this.axVSPrinter1.CurrentY = 8300;
            this.axVSPrinter1.MarginLeft = 6500;
            this.axVSPrinter1.Table = "^3000;" + ConvertDayInOrdinalForm(sDay) + " " + dtpBillDate.ToString("MMMM") + " " + dtpBillDate.ToString("yyyy");

            //ATTY PIRMA
            axVSPrinter1.DrawPicture(Properties.Resources.pirma, "5in", "6.4in", "70%", "70%", 10, false); //MCR 20161221 image path, x, y, witdh, height, shade

            //PAYMENT DETAILS
            #region queries
            if (m_sBnsStat == "NEW")
            {
                pRec.Query = "select capital as grosscap from businesses where bin = '" + m_sBIN + "'";
            }
            else
            {
                pRec.Query = "select gr_1 as grosscap from businesses where bin = '" + m_sBIN + "'";
            }

            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sGrossCap = pRec.GetDouble("grosscap").ToString("#,###.00");
                }
            }
            pRec.Close();

            pRec.Query = "select sum(fees_amtdue) as fees_amtdue from or_table where or_no = '" + m_sOrNo + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sAmountPaid = pRec.GetDouble("fees_amtdue").ToString("#,###.00");
                }
            }
            pRec.Close();

            pRec.Query = "select sum(amount) as amount from taxdues_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sAssessedTax = pRec.GetDouble("amount").ToString("#,###.00");
                }
            }
            pRec.Close();

            pRec.Query = "select qtr_paid from pay_hist where bin = '" + m_sBIN + "' and or_no = '" + m_sOrNo + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sTerm = pRec.GetString("qtr_paid");
                }
            }
            pRec.Close();

            if (sTerm == "F")
            {
                sTerm = "FULL";
            }
            else
            {
                sTerm = "QUARTERLY";
            }

            //FOR CURRENT Y
            Y = 11000;
            pRec.Query = "select * from addl_bns where bin ='" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "'";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    Y = Y - 200;
                }
            }

            #endregion
            this.axVSPrinter1.FontBold = false;
            sTime = string.Format("{0:hh:mm:ss:tt}", dtpBillDate);
            this.axVSPrinter1.CurrentY = 12200;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8.5;
            long lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "GROSS/CAPITAL: " + sGrossCap + "||" + "AMOUNT PAID: " + sAmountPaid;
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "ASSESSED TAX: " + sAssessedTax + "||" + "OR NUMBER: " + m_sOrNo;
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "PAYMENT TERM: " + sTerm + "||" + "OR DATE: " + m_sOrDate;
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);

            string sPlate = string.Empty;
            pRec.Query = String.Format("select * from buss_plate where bin = '{0}'", m_sBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sPlate = pRec.GetString("bns_plate");
                }
            }
            pRec.Close();

            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "PLATE NUMBER: " + sPlate + "||" + m_sBnsStat;
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "BIN: " + m_sBIN + "||" + "TIME GENERATED: " + sTime;
            this.axVSPrinter1.Table = "<3000|^5000|<3500;|||";
            //JHB 20181023 add Printed by & Date printed
            this.axVSPrinter1.Table = "<3000|^5000|<3500;"  + "||" + "GENERATED BY: " + AppSettingsManager.SystemUser.UserCode;
            sTime = String.Format("{0:MMMM d, yyyy}", dtpBillDate);
            this.axVSPrinter1.Table = "<3000|^5000|<3500;" + "||" + "DATE GENERATED: " + sTime;

            //ADDITIONAL LINES OF BUSINESS
            pRec.Query = "select distinct a.bns_code, a.bns_desc, b.capital, b.gross from bns_table a right join addl_bns b ";
            pRec.Query += "on a.bns_code = b.bns_code_main where a.fees_code = 'B' and b.tax_year = '" + m_sTaxYear + "' and b.bin = '" + m_sBIN + "'";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBnsDesc = AppSettingsManager.GetBnsDesc(pRec.GetString("bns_code"));
                    sCap = pRec.GetDouble("capital").ToString("#,##0.00");
                    sGross = pRec.GetDouble("gross").ToString("#,###0.00");
                    this.axVSPrinter1.Table = "<3000|^5000|<3500;" + sBnsDesc + "||" + "GROSS/CAPITAL: " + sGross + "/" + sCap;
                }
            }
            pRec.Close();

            /*this.axVSPrinter1.Table = "<3000|^5000|<3500;||FOR COMPLIANCE";
            this.axVSPrinter1.Table = "<3000|^5000|<3500;||" + m_sTempList;*/   // RMC 20171206 adjust printing of for compliance in temp permit
            // RMC 20171206 adjust printing of for compliance in temp permit (s)
            this.axVSPrinter1.Table = "<11000;FOR COMPLIANCE";
            this.axVSPrinter1.Table = "<11000;" + m_sTempList;
            // RMC 20171206 adjust printing of for compliance in temp permit (e)
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
        }

        private void PrintPermitMalolosNew()
        {
            // RMC 20171117 revised format of Business Permit

            bool blnIsHeadPrint = true;
            double dTotDue = 0;
            OracleResultSet result2 = new OracleResultSet();
            string sDefaultCode = string.Empty;
            string sDefaultDesc = string.Empty;
            string sAmount = string.Empty;
            double dData = 0;
            long lngY1 = 0;

            OracleResultSet pSet = new OracleResultSet();
            string sBnsNm = "";
            string sOwnNm = "";
            string sBnsAddress = "";
            string sBnsMainDesc = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sORNo = "";
            string sOrgnKind = "";
            string sBnsStat = "";
            string sTelNo = "";
            string sDay = "";
            string sCapital = "";
            string sGross = "";
            string sFeeAmt = "";
            string sRemarks = "";
            string sFlrArea = "";
            string sUnitNo = "";
            string sRoomNo = "";
            string sQtrRemarks = "";
            string sORDate = "";
            string sQtrPaid = "";
            string sPlate = "";
            string sCitizenship = "";
            string sQtrsPaid = "";
            string sMemo = "";
            string strData = "";
            //DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpBillDate = m_dtDate;    // RMC 20140129 Added module to input permit date issuance before printing of Permit
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate;
            string sOldBnsCode = "";
            string sFeesAmtDue = "";
            string sPermitFeeCode = "";
            string strSurPen = string.Empty;

            m_bPermitUpdateNoBilling = false;

            sBnsNm = GetDTIBnsName(m_sBIN);
            if (sBnsNm == "")
                sBnsNm = AppSettingsManager.GetBnsName(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwner(sOwnNm);
            sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
            sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);   //gets new bns type upon permit update appl change of bns type

            sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
            sBnsMainDesc = AppSettingsManager.GetSubBnsDesc(sBnsCode.Substring(0, 2), "B");
            //sBnsMainDesc = GetExemption(sBnsMainDesc);
            GetOtherBusinessInfo(sBnsCode);

            //AFM 20200103 temporary requested (s)
            string sBnsAddlBns = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select distinct(AB.bin) , AB.bns_code_main, BT.bns_desc from addl_bns AB, bns_table BT where bin = '" + BIN + "' and (AB.bns_code_main = BT.bns_code) and BT.fees_code = 'B' and tax_year = '" + m_sTaxYear + "'";
            if (result.Execute())
                while (result.Read())
                {
                    sBnsAddlBns += result.GetString("bns_desc") + "/";
                }
            //AFM 20200103 temporary requested (e)


            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 500;

            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)9.0;

            this.axVSPrinter1.CurrentY = 800;
            strData = "BPLD FORM - 008";
            this.axVSPrinter1.Table = string.Format("<8000|<3000;|{0}", strData);
            strData = "Permit No:|";
            this.axVSPrinter1.Table = string.Format("<8000|<1500|<1500;|{0}", strData);
            strData = "Business Plate:|";
            this.axVSPrinter1.Table = string.Format("<8000|<1500|<1500;|{0}", strData);
            this.axVSPrinter1.CurrentY = 800;
            this.axVSPrinter1.FontBold = true;
            strData = "";
            this.axVSPrinter1.Table = string.Format("<8000|<3000;|{0}", strData);
            strData = "| " + m_sPermitNo;
            this.axVSPrinter1.Table = string.Format("<8000|<1500|<1500;|{0}", strData);
            strData = "| " + m_sPlate;
            this.axVSPrinter1.Table = string.Format("<8000|<1500|<1500;|{0}", strData);

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = 1200;

            //this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, ".5in", ".62in", "25%", "25%", 10, false); //AFM 20200220 MAO-20-12381 removed logo
            //this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos_wm, "3in", "4in", "100%", "100%", 10, false);
            
            strData = "Republic of the Philippines";
            this.axVSPrinter1.Table = string.Format("<2000|<6000;|{0}", strData);
            strData = "PROVINCE OF " + AppSettingsManager.GetConfigValue("08");
            if (strData != string.Empty)
                this.axVSPrinter1.Table = string.Format("<2000|<6000;|{0}", strData);
            strData = AppSettingsManager.GetConfigValue("02");
            this.axVSPrinter1.Table = string.Format("<2000|<6000;|{0}", strData);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.FontSize = (float)18.0;
            strData = "BUSINESS PERMIT";
            this.axVSPrinter1.Table = string.Format("<2000|<6000;|{0}", strData);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.FontSize = (float)9.0;
            this.axVSPrinter1.FontItalic = true;
            //strData = "BUSINESS PERMIT";
            //this.axVSPrinter1.Table = string.Format("<2000|<6000;|{0}", strData);
            this.axVSPrinter1.FontItalic = false;

            string sExemption = string.Empty;
            sExemption = GetExemption(sBnsMainDesc);

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";

            this.axVSPrinter1.FontSize = (float)8.0;
            //this.axVSPrinter1.Paragraph = "";
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            strData = "Application No:| ";
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "Business ID No:| ";
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "Mode of Payment:|";
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "Date Issued:|";
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "Expiry Date:|";
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            //strData = "Type:|";
            strData = "Status:|"; //AFM 20200220 MAO-20-12381
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY1;
            //strData = "|" + AppSettingsManager.GetAppNo(m_sBIN);
            strData = "|" + AppSettingsManager.GetBillNoAndDate(m_sBIN, m_sTaxYear, sBnsCode, 0);
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "|" + m_sBIN;
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            string sModeOfPayment = string.Empty;
            sModeOfPayment = ConsolidatedQtr();
            strData = "|" + sModeOfPayment;
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "|" + dtpBillDate.ToShortDateString();
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "|12/31/" + ConfigurationAttributes.CurrentYear;
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "|" + m_sBnsStat;
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";

            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.FontBold = true;
            strData = "This certifies that:";
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";

            //strData = "Taxpayer's Name:|" + sOwnNm;
            //this.axVSPrinter1.Table = string.Format("<2000|<8000;{0}", strData);
            //AFM 20200220 MAO-20-12381 (s)
            object y2;
            y2 = axVSPrinter1.CurrentY;
            strData = "Taxpayer's Name:";
            this.axVSPrinter1.Table = string.Format("<2000;{0}", strData);
            axVSPrinter1.CurrentY = y2;
            strData = "|" + sOwnNm;
            axVSPrinter1.FontSize = 14;
            this.axVSPrinter1.Table = string.Format("<2000|<8000;{0}", strData);
            axVSPrinter1.FontSize = 9.75f;
            //AFM 20200220 MAO-20-12381 (e)

            strData = "Business Name:|" + sBnsNm;
            this.axVSPrinter1.Table = string.Format("<2000|<8000;{0}", strData);

            strData = "Business Address:|" + sBnsAddress;

            this.axVSPrinter1.Table = string.Format("<2000|<8000;{0}", strData);

            //strData = "Form of Ownership:|" + m_sOrgnKind + "|No. of Employees:|" + m_sNoEmp;
            //this.axVSPrinter1.Table = string.Format("<2000|<3000|<3000|*1000;{0}", strData);
            strData = "Form of Ownership:|" + m_sOrgnKind;
            this.axVSPrinter1.Table = string.Format("<2000|<3000;{0}", strData);


            object y;
            y = this.axVSPrinter1.CurrentY;
            //AFM 20190823 additional display requested by malolos (s)
            if (sBnsAddlBns.Trim() != "")
                strData = "LINE OF BUSINESS:|" + sBnsDesc + "/" + sBnsAddlBns; // AFM20190103 added addl bns temporarily
            else
                strData = "LINE OF BUSINESS:|" + sBnsDesc; // AFM20190103 added addl bns temporarily
            this.axVSPrinter1.Table = string.Format("<2020|<8000;{0}", strData);
            //AFM 20190823 additional display requested by malolos (e)

            //AFM 20190823 MAO-20-12381
            strData = "No. of Employees|" + m_sNoEmp;
            this.axVSPrinter1.Table = string.Format("<2020|*1000;{0}", strData);


            strData = " ";
            //this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = (float)9.0;
            this.axVSPrinter1.FontBold = false;
            strData = "has been granted PERMIT to operate the following business/es pursuant to Ordinance no. 08, Series of 2004,\n";
            strData += "The Revenue Code of the City of Malolos, after payment of taxes, fees and regulatory charges. Subject to compliance\n";
            strData += "with laws, ordinances and administrative regulations.";

            this.axVSPrinter1.Table = string.Format("=10000;{0}", strData);
            strData = " ";
            this.axVSPrinter1.Paragraph = "";
            
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            
            strData = "Nature of Business|Capitalization/Gross|Amount Paid|OR No./Date";
            this.axVSPrinter1.Table = string.Format("^3000|^2400|^2300|^2500;{0}", strData);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            
            sPermitFeeCode = GetPermitFeeCode();
            //get business tax and mayor's permit paid for main business and other line of business
            string sFeesDue = string.Empty;
            
            bool bPaidUsingTaxCredit = false;
            
            m_dAddlFeesDue = 0;

            int iBnsCount = 0;
            iBnsCount = m_sArrayBnsCode.Length;

            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);            
            List<DateTime> sDtTimeTmp = new List<DateTime>();
            OracleResultSet pSet2 = new OracleResultSet();
            string sTmpOrNo = "";
            int cnt = 0;
            double dPaidDue = 0;
            string sBnsCodeX = string.Empty;
            double dTotalAmtPaid = 0;
            pSet.Query = "select distinct or_no, or_date from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' order by or_date"; //AFM 20190814 fixed ordering
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    //if (sTmpOrNo != "")
                    //    sTmpOrNo += "\n";
                    sTmpOrNo = pSet.GetString(0) + " - " + string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime(1));
                    sDtTimeTmp.Add(pSet.GetDateTime(1)); //AFM 20190814 for datetime of or
                    cnt += 1;

                    //AFM 20191023 (s)
                    pSet2.Query = "select bns_code_main, sum(fees_amtdue), or_no from or_table where or_no in ";
                    pSet2.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
                    pSet2.Query += " and fees_code = 'B' and or_no = '"+ pSet.GetString("or_no") +"' group by bns_code_main, or_no order by bns_code_main";

                    cnt = 0;
                    DateTime[] sTmpDtTime = sDtTimeTmp.ToArray();
                    
                    if (pSet2.Execute())
                    {
                        while (pSet2.Read())
                        {
                            dPaidDue = pSet2.GetDouble(1);
                            sBnsCodeX = pSet2.GetString(0);

                            sORNo = pSet2.GetString("or_no");

                            if (!bPaidUsingTaxCredit)
                                bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet2.GetString("or_no"));

                            m_sCapital = "";
                            m_sGross = "";

                            m_sCapital = AppSettingsManager.GetCapitalGross(m_sBIN, sBnsCodeX, m_sTaxYear, "NEW");
                            m_sGross = AppSettingsManager.GetCapitalGross(m_sBIN, sBnsCodeX, m_sTaxYear, "REN");

                            string sCapGross = string.Empty;
                            if (m_sCapital != "" && Convert.ToDouble(m_sCapital) != 0)
                                sCapGross = m_sCapital;
                            else
                                sCapGross = m_sGross;

                            if (sTmpOrNo != sORNo)
                                sTmpOrNo = sTmpOrNo; //sTmpOrNo = sORNo;
                            else
                                sTmpOrNo = "";

                            //sTmpOrNo += " - " + string.Format("{0:MM/dd/yyyy}", sTmpDtTime[cnt]); //AFM 20190814 inserted to apply correction in permit format

                            if (cnt == 0)
                            {
                                strData = "TAX ON " + AppSettingsManager.GetBnsDesc(sBnsCodeX) + "|" + string.Format("{0:#,##0.#0}", Convert.ToDouble(sCapGross)) + "|" + string.Format("{0:#,##0.#0}", dPaidDue) + "|" + sTmpOrNo;
                                this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2700;{0}", strData);
                            }
                            else
                            {
                                strData = "TAX ON " + AppSettingsManager.GetBnsDesc(sBnsCodeX) + "|" + string.Format("{0:#,##0.#0}", Convert.ToDouble(sCapGross)) + "|" + string.Format("{0:#,##0.#0}", dPaidDue);
                                this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2700;{0}", strData);
                            }

                            dTotalAmtPaid += dPaidDue;
                            cnt += 1;
                        }
                    }
                    pSet2.Close();
                    //AFM 20191023 (e)
                }
            }
            pSet.Close();

            //this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2300;|||{0}", sTmpOrNo);
            //axVSPrinter1.CurrentY = lngY1;

            //test comment
            //double dTotalAmtPaid = 0;

            //pSet.Query = "select bns_code_main, sum(fees_amtdue), or_no from or_table where or_no in ";
            //pSet.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
            //pSet.Query += " and fees_code = 'B' group by bns_code_main, or_no";

            ////AFM 20190814 new query to include OR date (s)
            ////pSet.Query = "select OT.bns_code_main, sum(OT.fees_amtdue), OT.or_no, PH.OR_DATE from or_table OT, pay_hist PH where OT.or_no = PH.or_no and PH.QTR_PAID = OT.QTR_PAID and OT.or_no in ";
            ////pSet.Query += " (select PHT.or_no from pay_hist PHT where PHT.bin = '" + m_sBIN + "' and PHT.tax_year = '" + m_sTaxYear + "')";
            ////pSet.Query += " and OT.fees_code = 'B' GROUP BY OT.bns_code_main, OT.fees_amtdue, OT.or_no, PH.OR_DATE";
            ////AFM 20190814 new query to include OR date (e)


            //cnt = 0;
            //DateTime[] sTmpDtTime = sDtTimeTmp.ToArray();
            //double dPaidDue = 0;
            //string sBnsCodeX = string.Empty;
            //if (pSet.Execute())
            //{
            //    while (pSet.Read())
            //    {
            //        dPaidDue = pSet.GetDouble(1);
            //        sBnsCodeX = pSet.GetString(0);
                                                          
            //        sORNo = pSet.GetString("or_no");

            //        if (!bPaidUsingTaxCredit)
            //            bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet.GetString("or_no"));

            //        m_sCapital = "";
            //        m_sGross = "";

            //        m_sCapital = AppSettingsManager.GetCapitalGross(m_sBIN, sBnsCodeX, m_sTaxYear, "NEW");
            //        m_sGross = AppSettingsManager.GetCapitalGross(m_sBIN, sBnsCodeX, m_sTaxYear, "REN");

            //        string sCapGross = string.Empty;
            //        if (m_sCapital != "" && Convert.ToDouble(m_sCapital) != 0)
            //            sCapGross = m_sCapital;
            //        else
            //            sCapGross = m_sGross;

            //        if (sTmpOrNo != sORNo)
            //            sTmpOrNo = sTmpOrNo; //sTmpOrNo = sORNo;
            //        else
            //            sTmpOrNo = "";

            //        //sTmpOrNo += " - " + string.Format("{0:MM/dd/yyyy}", sTmpDtTime[cnt]); //AFM 20190814 inserted to apply correction in permit format

            //        strData = "TAX ON " + AppSettingsManager.GetBnsDesc(sBnsCodeX) + "|" + string.Format("{0:#,##0.#0}", Convert.ToDouble(sCapGross)) + "|" + string.Format("{0:#,##0.#0}", dPaidDue) + "|" + sTmpOrNo;
            //        this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2700;{0}", strData);
            //        dTotalAmtPaid += dPaidDue;
            //        cnt += 1;
            //    }
            //}
            //pSet.Close();

            pSet.Query = "select fees_code, sum(fees_amtdue), or_no from or_table where or_no in ";
            pSet.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
            pSet.Query += " and fees_code <> 'B' group by fees_code, or_no order by fees_code asc";

            if (pSet.Execute())
            {
                
                while (pSet.Read())
                {
                    dPaidDue = pSet.GetDouble(1);
                    sBnsCodeX = pSet.GetString(0);
                    sORNo = pSet.GetString("or_no");

                    if (!bPaidUsingTaxCredit)
                        bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet.GetString("or_no"));

                    if (sTmpOrNo != sORNo)
                        sTmpOrNo = sORNo;
                    else
                        sTmpOrNo = "";

                    strData = AppSettingsManager.GetFeesDesc(sBnsCodeX) + "||" + string.Format("{0:#,##0.#0}", dPaidDue) + "|";
                    this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2300;{0}", strData);
                    dTotalAmtPaid += dPaidDue;
                }
            }
            pSet.Close();
                        
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            if (dTotalAmtPaid != 0)
                this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2300;|TOTAL|{0}|", string.Format("{0:#,##0.#0}", dTotalAmtPaid));
            this.axVSPrinter1.FontBold = false;

            strData = "ERASURES OR ALTERATIONS WILL INVALIDATE THIS PERMIT";
            strData = " ";

            strData = " ";
            this.axVSPrinter1.Table = string.Format("^12000;{0}", strData);
            this.axVSPrinter1.FontSize = (float)9.0;
            this.axVSPrinter1.FontBold = true;
            
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            if (AppSettingsManager.GetConfigValue("79") == "N") //AFM 20220103 requested to hide submitted requirements
            {
                strData = "SUBMITTED REQUIREMENTS:";
                this.axVSPrinter1.Table = string.Format("<7000;{0}", strData);
                this.axVSPrinter1.FontBold = false;
                string sReq = "";
                int iCount = 0;

                ArrayList arrWag = new ArrayList();
                arrWag.Add("024"); //SSS CERTIFICATE
                arrWag.Add("025"); //PHILHEALTH CERTIFICATE
                arrWag.Add("026"); //PAGIBIG CERTIFICATE
                arrWag.Add("045"); //SECRETARY'S CERTIFICATE AUTHORIZING THE REPRESENTATIVE TO TRANSACT



                //MCR 20190724 MAO-19-10438 (s)
                if (m_sBnsStat != "NEW")
                    arrWag.Add("038"); //SIGNAGE
                if (!sBnsAddress.Contains("SUBD"))
                    arrWag.Add("030"); //HOME OWNERS CERTIFICATE
                //MCR 20190724 MAO-19-10438 (e)
                //AFM 20190823 MAO-19-10705 (s)
                if (m_sBnsOccupancy != "RENTED")
                    arrWag.Add("040"); //
                //AFM 20190823 MAO-19-10705 (e)

                pSet.Query = "select distinct a.req_code, req_desc from requirements_chklist a, requirements_tbl b ";
                pSet.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' and a.req_code = b.req_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        if (!arrWag.Contains(pSet.GetString(0)))
                        {
                            iCount++;
                            if (sReq != "")
                                sReq += "\n";
                            sReq += pSet.GetString(1);
                        }
                    }
                }
                pSet.Close();



                DateTime dCurrent = new DateTime();
                dCurrent = AppSettingsManager.GetCurrentDate();
                double dCnt = 0;
                TimeSpan ts;
                ts = (dCurrent.Date - m_dDTIDtComp.Date);
                dCnt = ts.TotalDays;

                if (dCnt <= 1825) //1825 is 5 years
                {
                    if (m_sOrgnKind == "SINGLE PROPRIETORSHIP")
                        sReq += "\nDTI No.: " + m_sDTIDt;
                    else if (m_sOrgnKind == "COOPERATIVE")
                        sReq += "\nCDA No.: " + m_sDTIDt;
                    else //CORPORATION and PARTNERSHIP
                        sReq += "\nSEC No.: " + m_sDTIDt;
                }

                //if (iCount >= 13)
                //    this.axVSPrinter1.FontSize = (float)7;
                //else if (iCount > 8)
                //    this.axVSPrinter1.FontSize = (float)8;

                //AFM 20200227 requested by sir jester - QR CODE (s) (for study)

                //this.axVSPrinter1.DrawPicture(Properties.Resources.QR_malolos, ".5in", ".62in", "25%", "25%", 10, false); //AFM 20200220 MAO-20-12381 removed logo

                //AFM 20200227 requested by sir jester and sir ryan (for presentation purposes) QR CODE (e)




                this.axVSPrinter1.Table = string.Format("<7000;{0}", sReq);

            }
            this.axVSPrinter1.CurrentY = 11600;
            this.axVSPrinter1.MarginLeft = 6600;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)11.0;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.MarginLeft = 6600;
            strData = AppSettingsManager.GetConfigValue("38");
            this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            this.axVSPrinter1.FontItalic = true;
            this.axVSPrinter1.FontBold = false;
            strData = "Business Permit and Licensing Officer";
            this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            this.axVSPrinter1.FontItalic = false;
                                    
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            
            //axVSPrinter1.DrawPicture(Properties.Resources.pirma, "10in", "6.4in", "70%", "70%", 10, false);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)12.0;
            strData = AppSettingsManager.GetConfigValue("36");
            this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            this.axVSPrinter1.FontItalic = true;
            this.axVSPrinter1.FontBold = false;
            strData = AppSettingsManager.GetConfigValue("01") + " MAYOR";
            this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            this.axVSPrinter1.FontItalic = false;


            //AFM 20220103 MAO-21-16284 (s) QR in business permit as requested by Malolos
            pSet.Query = "select status from soa_monitoring where bin = '" + m_sBIN + "' and status = 'APPROVED'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    byte[] blobData = null;
                    pSet.Query = "select qr_code from BO_QR where bin = '" + m_sBIN + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            blobData = pSet.GetBlob(0);
                            Image image = (Bitmap)((new ImageConverter()).ConvertFrom(blobData));
                            this.axVSPrinter1.DrawPicture(image, "6.1in", "2.7in", "50%", "50%", 11, false);
                        }
                    pSet.Close();
                }
            //AFM 20220103 MAO-21-16284 (e)

            this.axVSPrinter1.CurrentY = 14000;

            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 1000;
            strData = "|IMPORTANT REMINDERS:|";
            this.axVSPrinter1.Table = string.Format("<100|<5900|<5900;{0}", strData);
            this.axVSPrinter1.FontBold = false;

            strData = "|*|Post this Permit in a conspicuous place in your establishment.||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
            strData = "|*|Valid only at the business address indicated above. ||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
            strData = "|*|Surrender this permit upon termination or closure of the business.  Any tax due shall first be paid before any business or undertaking is fully teminated. ||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
            strData = "|*|FAILURE TO COMPLY WITH THE APPLICABLE ORDINANCES, LAWS AND ADMINISTRATIVE REGULATIONS SHALL RENDER THIS PERMIT AUTOMATICALLY REVOKED. ||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);

            this.axVSPrinter1.FontSize = (float)7.0;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<100|<8000;|GENERATED BY " + AppSettingsManager.SystemUser.UserName + " " + AppSettingsManager.GetCurrentDate().ToString("MM/dd/yyyy hh:mm:ss tt"));
        }

        //AFM 20190813 for temp permit of Malolos.
        private void PrintTempPermitMalolosNew()
        {
            // RMC 20171117 revised format of Business Permit

            bool blnIsHeadPrint = true;
            double dTotDue = 0;
            OracleResultSet result2 = new OracleResultSet();
            string sDefaultCode = string.Empty;
            string sDefaultDesc = string.Empty;
            string sAmount = string.Empty;
            double dData = 0;
            long lngY1 = 0;

            OracleResultSet pSet = new OracleResultSet();
            string sBnsNm = "";
            string sOwnNm = "";
            string sBnsAddress = "";
            string sBnsMainDesc = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sORNo = "";
            string sOrgnKind = "";
            string sBnsStat = "";
            string sTelNo = "";
            string sDay = "";
            string sCapital = "";
            string sGross = "";
            string sFeeAmt = "";
            string sRemarks = "";
            string sFlrArea = "";
            string sUnitNo = "";
            string sRoomNo = "";
            string sQtrRemarks = "";
            string sORDate = "";
            string sQtrPaid = "";
            string sPlate = "";
            string sCitizenship = "";
            string sQtrsPaid = "";
            string sMemo = "";
            string strData = "";
            //DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpBillDate = m_dtDate;    // RMC 20140129 Added module to input permit date issuance before printing of Permit
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate;
            string sOldBnsCode = "";
            string sFeesAmtDue = "";
            string sPermitFeeCode = "";
            string strSurPen = string.Empty;

            m_bPermitUpdateNoBilling = false;

            sBnsNm = GetDTIBnsName(m_sBIN);
            if (sBnsNm == "")
                sBnsNm = AppSettingsManager.GetBnsName(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            sOwnNm = AppSettingsManager.GetBnsOwner(sOwnNm);
            sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
            sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);   //gets new bns type upon permit update appl change of bns type

            sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
            sBnsMainDesc = AppSettingsManager.GetSubBnsDesc(sBnsCode.Substring(0, 2), "B");
            //sBnsMainDesc = GetExemption(sBnsMainDesc);
            GetOtherBusinessInfo(sBnsCode);

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 500;

            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)9.0;

            this.axVSPrinter1.CurrentY = 800;
            //strData = "BPLD FORM - 008";
            this.axVSPrinter1.Table = string.Format("<8000|<3000;|{0}", strData);
            //strData = "Permit No:|";
            strData = "TEMP|"; //AFM 20190813 new temp permit format - malolos ver.
            this.axVSPrinter1.Table = string.Format("<8000|<1500|<1500;|{0}", strData);
            strData = "Business Plate:|";
            this.axVSPrinter1.Table = string.Format("<8000|<1500|<1500;|{0}", strData);
            this.axVSPrinter1.CurrentY = 800;
            this.axVSPrinter1.FontBold = true;
            strData = "";
            this.axVSPrinter1.Table = string.Format("<8000|<3000;|{0}", strData);
            strData = "| " + m_sPermitNo;
            this.axVSPrinter1.Table = string.Format("<8000|<1500|<1500;|{0}", strData);
            strData = "| " + m_sPlate;
            this.axVSPrinter1.Table = string.Format("<8000|<1500|<1500;|{0}", strData);

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = 1200;

            //this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, ".5in", ".62in", "25%", "25%", 10, false); //AFM 20200220 MAO-20-12381
            //this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos_wm, "3in", "4in", "100%", "100%", 10, false);

            strData = "Republic of the Philippines";
            this.axVSPrinter1.Table = string.Format("<2000|<6000;|{0}", strData);
            strData = "PROVINCE OF " + AppSettingsManager.GetConfigValue("08");
            if (strData != string.Empty)
                this.axVSPrinter1.Table = string.Format("<2000|<6000;|{0}", strData);
            strData = AppSettingsManager.GetConfigValue("02");
            this.axVSPrinter1.Table = string.Format("<2000|<6000;|{0}", strData);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.FontSize = (float)18.0;
            strData = "BUSINESS PERMIT";
            this.axVSPrinter1.Table = string.Format("<2000|<6000;|{0}", strData);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.FontSize = (float)9.0;
            this.axVSPrinter1.FontItalic = true;
            //strData = "BUSINESS PERMIT";
            //this.axVSPrinter1.Table = string.Format("<2000|<6000;|{0}", strData);
            this.axVSPrinter1.FontItalic = false;

            string sExemption = string.Empty;
            sExemption = GetExemption(sBnsMainDesc);

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";

            this.axVSPrinter1.FontSize = (float)8.0;
            //this.axVSPrinter1.Paragraph = "";
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            strData = "Application No:| ";
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "Business ID No:| ";
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "Mode of Payment:|";
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "Date Issued:|";
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "Expiry Date:|";
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            //strData = "Type:|";
            strData = "Status:|"; //AFM 20200220
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY1;
            //strData = "|" + AppSettingsManager.GetAppNo(m_sBIN);
            strData = "|" + AppSettingsManager.GetBillNoAndDate(m_sBIN, m_sTaxYear, sBnsCode, 0);
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "|" + m_sBIN;
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            string sModeOfPayment = string.Empty;
            sModeOfPayment = ConsolidatedQtr();
            strData = "|" + sModeOfPayment;
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            //strData = "|" + dtpBillDate.ToShortDateString(); 
            strData = "|" + AppSettingsManager.GetCurrentDate().ToShortDateString(); //AFM 20190913 (s)
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            //strData = "|12/31/" + ConfigurationAttributes.CurrentYear; 
            strData = "|" + dtpBillDate.ToShortDateString(); //AFM 20190913 (e)
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);
            strData = "|" + m_sBnsStat;
            this.axVSPrinter1.Table = string.Format("<7500|<1500|<2000;|{0}", strData);

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial Narrow";

            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.FontBold = true;
            strData = "This certifies that:";
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";

            //strData = "Taxpayer's Name:|" + sOwnNm;
            //this.axVSPrinter1.Table = string.Format("<2000|<8000;{0}", strData);

            //AFM 20200220 MAO-20-12381 (s)
            object y2;
            y2 = axVSPrinter1.CurrentY;
            strData = "Taxpayer's Name:";
            this.axVSPrinter1.Table = string.Format("<2000;{0}", strData);
            axVSPrinter1.CurrentY = y2;
            strData = "|" + sOwnNm;
            axVSPrinter1.FontSize = 14;
            this.axVSPrinter1.Table = string.Format("<2000|<8000;{0}", strData);
            axVSPrinter1.FontSize = 9.75f;
            //AFM 20200220 MAO-20-12381 (e)

            strData = "Business Name:|" + sBnsNm;
            this.axVSPrinter1.Table = string.Format("<2000|<8000;{0}", strData);

            strData = "Business Address:|" + sBnsAddress;

            this.axVSPrinter1.Table = string.Format("<2000|<8000;{0}", strData);

            //strData = "Form of Ownership:|" + m_sOrgnKind + "|No. of Employees:|" + m_sNoEmp;
            //this.axVSPrinter1.Table = string.Format("<2000|<3000|<3000|*1000;{0}", strData);
            //AFM 20200220 MAO-20-12381
            strData = "Form of Ownership:|" + m_sOrgnKind;
            this.axVSPrinter1.Table = string.Format("<2000|<3000;{0}", strData);

            //AFM 20190823 additional display requested by malolos (s)
            strData = "LINE OF BUSINESS:|" + sBnsDesc;
            this.axVSPrinter1.Table = string.Format("<2020|<8000;{0}", strData);
            //AFM 20190823 additional display requested by malolos (e)

            strData = "No. of Employees|" + m_sNoEmp;
            this.axVSPrinter1.Table = string.Format("<2020|*1000;{0}", strData);

            strData = " ";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = (float)9.0;
            this.axVSPrinter1.FontBold = false;
            strData = "has been granted PERMIT to operate the following business/es pursuant to Ordinance no. 08, Series of 2004,\n";
            strData += "The Revenue Code of the City of Malolos, after payment of taxes, fees and regulatory charges. Subject to compliance\n";
            strData += "with laws, ordinances and administrative regulations.";

            this.axVSPrinter1.Table = string.Format("=10000;{0}", strData);
            strData = " ";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            strData = "Nature of Business|Capitalization/Gross|Amount Paid|OR No./Date";
            this.axVSPrinter1.Table = string.Format("^3000|^2400|^2300|^2300;{0}", strData);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";

            sPermitFeeCode = GetPermitFeeCode();
            //get business tax and mayor's permit paid for main business and other line of business
            string sFeesDue = string.Empty;

            bool bPaidUsingTaxCredit = false;

            m_dAddlFeesDue = 0;

            int iBnsCount = 0;
            iBnsCount = m_sArrayBnsCode.Length;

            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            List<DateTime> sDtTimeTmp = new List<DateTime>();
            OracleResultSet pSet2 = new OracleResultSet();
            string sTmpOrNo = "";
            int cnt = 0;
            double dPaidDue = 0;
            string sBnsCodeX = string.Empty;
            double dTotalAmtPaid = 0;
            pSet.Query = "select distinct or_no, or_date from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' order by or_date";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    if (sTmpOrNo != "")
                        sTmpOrNo += "\n";
                    sTmpOrNo = pSet.GetString(0) + " - " + string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime(1));
                    sDtTimeTmp.Add(pSet.GetDateTime(1)); //AFM 20190814 for datetime of or
                    cnt += 1;

                    //AFM 20191023 (s)
                    pSet2.Query = "select bns_code_main, sum(fees_amtdue), or_no from or_table where or_no in ";
                    pSet2.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
                    pSet2.Query += " and fees_code = 'B' and or_no = '"+ pSet.GetString("or_no") +"' group by bns_code_main, or_no order by bns_code_main";

                    cnt = 0;
                    DateTime[] sTmpDtTime = sDtTimeTmp.ToArray();
                    
                    if (pSet2.Execute())
                    {
                        while (pSet2.Read())
                        {
                            dPaidDue = pSet2.GetDouble(1);
                            sBnsCodeX = pSet2.GetString(0);

                            sORNo = pSet2.GetString("or_no");

                            if (!bPaidUsingTaxCredit)
                                bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet2.GetString("or_no"));

                            m_sCapital = "";
                            m_sGross = "";

                            m_sCapital = AppSettingsManager.GetCapitalGross(m_sBIN, sBnsCodeX, m_sTaxYear, "NEW");
                            m_sGross = AppSettingsManager.GetCapitalGross(m_sBIN, sBnsCodeX, m_sTaxYear, "REN");

                            string sCapGross = string.Empty;
                            if (m_sCapital != "" && Convert.ToDouble(m_sCapital) != 0)
                                sCapGross = m_sCapital;
                            else
                                sCapGross = m_sGross;

                            if (sTmpOrNo != sORNo)
                                sTmpOrNo = sTmpOrNo; //sTmpOrNo = sORNo;
                            else
                                sTmpOrNo = "";

                            //sTmpOrNo += " - " + string.Format("{0:MM/dd/yyyy}", sTmpDtTime[cnt]); //AFM 20190814 inserted to apply correction in permit format

                            if (cnt == 0)
                            {
                                strData = "TAX ON " + AppSettingsManager.GetBnsDesc(sBnsCodeX) + "|" + string.Format("{0:#,##0.#0}", Convert.ToDouble(sCapGross)) + "|" + string.Format("{0:#,##0.#0}", dPaidDue) + "|" + sTmpOrNo;
                                this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2700;{0}", strData);
                            }
                            else
                            {
                                strData = "TAX ON " + AppSettingsManager.GetBnsDesc(sBnsCodeX) + "|" + string.Format("{0:#,##0.#0}", Convert.ToDouble(sCapGross)) + "|" + string.Format("{0:#,##0.#0}", dPaidDue);
                                this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2700;{0}", strData);
                            }

                            dTotalAmtPaid += dPaidDue;
                            cnt += 1;
                        }
                    }
                    pSet2.Close();
                    //AFM 20191023 (e)
                }
            }
            pSet.Close();

            //test comment
            //this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2300;|||{0}", sTmpOrNo);
            //axVSPrinter1.CurrentY = lngY1;

            //double dTotalAmtPaid = 0;
            //DateTime[] sTmpDtTime = sDtTimeTmp.ToArray();
            //cnt = 0;

            //pSet.Query = "select bns_code_main, sum(fees_amtdue), or_no from or_table where or_no in ";
            //pSet.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
            //pSet.Query += " and fees_code = 'B' group by bns_code_main, or_no";

            //double dPaidDue = 0;
            //string sBnsCodeX = string.Empty;
            //if (pSet.Execute())
            //{
            //    while (pSet.Read())
            //    {
            //        dPaidDue = pSet.GetDouble(1);
            //        sBnsCodeX = pSet.GetString(0);

            //        sORNo = pSet.GetString("or_no");

            //        if (!bPaidUsingTaxCredit)
            //            bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet.GetString("or_no"));

            //        m_sCapital = "";
            //        m_sGross = "";

            //        m_sCapital = AppSettingsManager.GetCapitalGross(m_sBIN, sBnsCodeX, m_sTaxYear, "NEW");
            //        m_sGross = AppSettingsManager.GetCapitalGross(m_sBIN, sBnsCodeX, m_sTaxYear, "REN");

            //        string sCapGross = string.Empty;
            //        if (m_sCapital != "" && Convert.ToDouble(m_sCapital) != 0)
            //            sCapGross = m_sCapital;
            //        else
            //            sCapGross = m_sGross;

            //        if (sTmpOrNo != sORNo)
            //            sTmpOrNo = sTmpOrNo; //sTmpOrNo = sORNo;
            //        else
            //            sTmpOrNo = "";

            //        //sTmpOrNo += " - " + string.Format("{0:MM/dd/yyyy}", sTmpDtTime[cnt]); //AFM 20190814 inserted to apply correction in permit format

            //        strData = "TAX ON " + AppSettingsManager.GetBnsDesc(sBnsCodeX) + "|" + string.Format("{0:#,##0.#0}", Convert.ToDouble(sCapGross)) + "|" + string.Format("{0:#,##0.#0}", dPaidDue) + "|" + sTmpOrNo;
            //        this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2300;{0}", strData);
            //        dTotalAmtPaid += dPaidDue;
            //        cnt += 1;
            //    }
            //}
            //pSet.Close();

            pSet.Query = "select fees_code, sum(fees_amtdue), or_no from or_table where or_no in ";
            pSet.Query += " (select or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "')";
            pSet.Query += " and fees_code <> 'B' group by fees_code, or_no  order by fees_code asc"; //AFM 20190930 MAO-19-10959

            if (pSet.Execute())
            {

                while (pSet.Read())
                {
                    dPaidDue = pSet.GetDouble(1);
                    sBnsCodeX = pSet.GetString(0);
                    sORNo = pSet.GetString("or_no");

                    if (!bPaidUsingTaxCredit)
                        bPaidUsingTaxCredit = ValidatePaymentUsingTaxCredit(pSet.GetString("or_no"));

                    if (sTmpOrNo != sORNo)
                        sTmpOrNo = sORNo;
                    else
                        sTmpOrNo = "";

                    strData = AppSettingsManager.GetFeesDesc(sBnsCodeX) + "||" + string.Format("{0:#,##0.#0}", dPaidDue) + "|";
                    this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2300;{0}", strData);
                    dTotalAmtPaid += dPaidDue;
                }
            }
            pSet.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            if (dTotalAmtPaid != 0)
                this.axVSPrinter1.Table = string.Format("<3000|>2400|>2300|<2300;|TOTAL|{0}|", string.Format("{0:#,##0.#0}", dTotalAmtPaid));
            this.axVSPrinter1.FontBold = false;

            strData = "ERASURES OR ALTERATIONS WILL INVALIDATE THIS PERMIT";
            strData = " ";

            strData = " ";
            this.axVSPrinter1.Table = string.Format("^12000;{0}", strData);
            this.axVSPrinter1.FontSize = (float)9.0;
            this.axVSPrinter1.FontBold = true;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            if (AppSettingsManager.GetConfigValue("79") == "N") //AFM 20220103 requested to hide submitted requirements
            {
                strData = "SUBMITTED REQUIREMENTS:";
                this.axVSPrinter1.Table = string.Format("<7000;{0}", strData);
                this.axVSPrinter1.FontBold = false;
                string sReq = "";
                int iCount = 0;

                ArrayList arrWag = new ArrayList();
                arrWag.Add("024"); //SSS CERTIFICATE
                arrWag.Add("025"); //PHILHEALTH CERTIFICATE
                arrWag.Add("026"); //PAGIBIG CERTIFICATE
                arrWag.Add("045"); //SECRETARY'S CERTIFICATE AUTHORIZING THE REPRESENTATIVE TO TRANSACT

                //MCR 20190724 MAO-19-10438 (s)
                if (m_sBnsStat != "NEW")
                    arrWag.Add("038"); //SIGNAGE
                if (!sBnsAddress.Contains("SUBD"))
                    arrWag.Add("030"); //HOME OWNERS CERTIFICATE
                //MCR 20190724 MAO-19-10438 (e)


                pSet.Query = "select distinct a.req_code, req_desc from requirements_chklist a, requirements_tbl b ";
                pSet.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' and a.req_code = b.req_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        if (!arrWag.Contains(pSet.GetString(0)))
                        {
                            iCount++;
                            if (sReq != "")
                                sReq += "\n";
                            sReq += pSet.GetString(1);
                        }
                    }
                }
                pSet.Close();

                DateTime dCurrent = new DateTime();
                dCurrent = AppSettingsManager.GetCurrentDate();
                double dCnt = 0;
                TimeSpan ts;
                ts = (dCurrent.Date - m_dDTIDtComp.Date);
                dCnt = ts.TotalDays;

                if (dCnt <= 1825) //1825 is 5 years
                {
                    if (m_sOrgnKind == "SINGLE PROPRIETORSHIP")
                        sReq += "\nDTI No.: " + m_sDTIDt;
                    else if (m_sOrgnKind == "COOPERATIVE")
                        sReq += "\nCDA No.: " + m_sDTIDt;
                    else //CORPORATION and PARTNERSHIP
                        sReq += "\nSEC No.: " + m_sDTIDt;
                }
                //if (iCount >= 13)
                //    this.axVSPrinter1.FontSize = (float)7;
                //else if (iCount > 8)
                //    this.axVSPrinter1.FontSize = (float)8;


                this.axVSPrinter1.Table = string.Format("<7000;{0}", sReq);
            }

            this.axVSPrinter1.CurrentY = 12800;
            //AFM inserted for malolos new temp permit format (s)
            // RMC 20171206 adjust printing of for compliance in temp permit (s)
            this.axVSPrinter1.Table = "<11000;FOR COMPLIANCE";
            this.axVSPrinter1.Table = "<11000;" + m_sTempList;
            // RMC 20171206 adjust printing of for compliance in temp permit (e)
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            //AFM inserted for malolos new temp permit format (e)

            this.axVSPrinter1.CurrentY = 11600;
            this.axVSPrinter1.MarginLeft = 6600;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)11.0;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.MarginLeft = 6600;
            strData = AppSettingsManager.GetConfigValue("38");
            this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            this.axVSPrinter1.FontItalic = true;
            this.axVSPrinter1.FontBold = false;
            strData = "Business Permit and Licensing Officer";
            this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            this.axVSPrinter1.FontItalic = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //axVSPrinter1.DrawPicture(Properties.Resources.pirma, "10in", "6.4in", "70%", "70%", 10, false);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)12.0;
            strData = AppSettingsManager.GetConfigValue("36");
            this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            this.axVSPrinter1.FontItalic = true;
            this.axVSPrinter1.FontBold = false;
            strData = AppSettingsManager.GetConfigValue("01") + " MAYOR";
            this.axVSPrinter1.Table = string.Format("^5000;{0}", strData);
            this.axVSPrinter1.FontItalic = false;

            this.axVSPrinter1.CurrentY = 14000;

            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 1000;
            strData = "|IMPORTANT REMINDERS:|";
            this.axVSPrinter1.Table = string.Format("<100|<5900|<5900;{0}", strData);
            this.axVSPrinter1.FontBold = false;

            strData = "|*|Post this Permit in a conspicuous place in your establishment.||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
            strData = "|*|Valid only at the business address indicated above. ||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
            strData = "|*|Surrender this permit upon termination or closure of the business.  Any tax due shall first be paid before any business or undertaking is fully teminated. ||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);
            strData = "|*|FAILURE TO COMPLY WITH THE APPLICABLE ORDINANCES, LAWS AND ADMINISTRATIVE REGULATIONS SHALL RENDER THIS PERMIT AUTOMATICALLY REVOKED. ||";
            this.axVSPrinter1.Table = string.Format("<100|^300|=5000|<900|<5900;{0}", strData);

            this.axVSPrinter1.FontSize = (float)7.0;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<100|<8000;|GENERATED BY " + AppSettingsManager.SystemUser.UserName + " " + AppSettingsManager.GetCurrentDate().ToString("MM/dd/yyyy hh:mm:ss tt"));
        }

        private void PrintSpecialPermit()
        {
            // RMC 20171124 customized special permit printing where payment was made at aRCS
            string strData = string.Empty;
            string sBnsStat = string.Empty;
            string sTaxYear = string.Empty;
            string sDay = "";
            string sMonth = "";
            string sYear = "";

            DateTime current = AppSettingsManager.GetCurrentDate();
            sDay = current.ToString("dd");
            sMonth = current.ToString("MMMM");
            sYear = current.ToString("yyyy");

            
            this.axVSPrinter1.CurrentY = 1200;

            this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, ".5in", ".62in", "25%", "25%", 10, false);

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.FontBold = false;
            strData = "Republic of the Philippines";
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            strData = "PROVINCE OF " + AppSettingsManager.GetConfigValue("08");
            if (strData != string.Empty)
                this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            this.axVSPrinter1.FontBold = true;
            strData = AppSettingsManager.GetConfigValue("02");
            this.axVSPrinter1.FontSize = (float)12.0; 
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = string.Format("^10000;OFFICE OF THE CITY ADMINISTRATOR");
            this.axVSPrinter1.Table = string.Format("^10000;BUSINESS PERMIT AND LICENSING DIVISION");
            
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.Table = ">9000;" + current.ToString("MMMM dd, yyyy");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)18.0;
            this.axVSPrinter1.FontBold = true;
            strData = "SPECIAL PERMIT";
            this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;

            
            strData = "Business Name: |" + m_sArrayInfo.GetValue(0);
            this.axVSPrinter1.Table = string.Format("<3000|<7000;{0}", strData);
            this.axVSPrinter1.Paragraph = "";
            strData = "Business Owner: |" + m_sArrayInfo.GetValue(1);
            this.axVSPrinter1.Table = string.Format("<3000|<7000;{0}", strData);
            this.axVSPrinter1.Paragraph = "";
            strData = "Business Address: |" + m_sArrayInfo.GetValue(2);
            this.axVSPrinter1.Table = string.Format("<3000|<7000;{0}", strData);
            this.axVSPrinter1.Paragraph = "";
            strData = "Event: |" + m_sArrayInfo.GetValue(3);
            this.axVSPrinter1.Table = string.Format("<3000|<7000;{0}", strData);
            this.axVSPrinter1.Paragraph = "";
            strData = "Date & Time: |" + m_sArrayInfo.GetValue(4);
            this.axVSPrinter1.Table = string.Format("<3000|<7000;{0}", strData);
            this.axVSPrinter1.Paragraph = "";
            strData = "Place: |" + m_sArrayInfo.GetValue(5);
            this.axVSPrinter1.Table = string.Format("<3000|<7000;{0}", strData);
            this.axVSPrinter1.Paragraph = "";
            strData = "Validity: |" + m_sArrayInfo.GetValue(6);
            this.axVSPrinter1.Table = string.Format("<3000|<7000;{0}", strData);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            /*strData = "Prepared By: |" + AppSettingsManager.SystemUser.UserName;
            this.axVSPrinter1.Table = string.Format("<2000|^5000;{0}", strData);
            strData = "|" + AppSettingsManager.SystemUser.Position;
            this.axVSPrinter1.Table = string.Format("<2000|^5000;{0}", strData);*/  // RMC 20171127 removed as per Nori's instruction

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.MarginLeft = 7000;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^4000;" + AppSettingsManager.GetConfigValue("38");
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            this.axVSPrinter1.Table = "^4000;" + AppSettingsManager.GetConfigValue("37");

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;O.R. No.|:|" + m_sArrayInfo.GetValue(7));
            this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Date|:|" + m_sArrayInfo.GetValue(8));  
            this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Amount|:|P " + m_sArrayInfo.GetValue(9));
             
            
        }
    }
}