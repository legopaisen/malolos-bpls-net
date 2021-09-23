using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.BPLSApp;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using System.Collections;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;

namespace Amellar.Common.Reports
{
    public partial class frmSoaPrint : Form
    {
        protected Library LibraryClass = new Library();
        DateTime dDate = AppSettingsManager.GetCurrentDate();   // RMC 20110725
        private SystemUser m_objSystemUser = new SystemUser();
        public string sBin = string.Empty;
        public string sTermPay = string.Empty;
        public string sQtrToPay = string.Empty;
        public string sSOATaxYear = "0";
        string sTellerName = string.Empty;
        public bool isCheckedCredit = false; //AFM 20200114

       

        private bool bPreprinted = false; 
        public bool PreprintedForm
        {
            set { bPreprinted = value; }
        }

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

        public frmSoaPrint()
        {
            InitializeComponent();
        }

        private void frmSoaPrint_Load(object sender, EventArgs e)
        {
            SOALUBAO();
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void CreateHeader()
        {
            string sHeader = "", sHeader1 = "", sHeader2 = "";

            //axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4; // AST 20150429
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
            axVSPrinter1.MarginLeft = 500;
            axVSPrinter1.MarginRight = 500;

            this.axVSPrinter1.MarginTop = 500;   // RMC 20161215 display quarter due and date in SOA for Binan

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;

            sHeader = AppSettingsManager.GetConfigValue("09");
            axVSPrinter1.FontName = "Arial Narrow";
            
            axVSPrinter1.FontSize = 10;

            //axVSPrinter1.Table = string.Format("<10000;{0}", "BPLO-011-0"); // RMC 20161124 added ISO form # for Tax Order of Payment

            if (!bPreprinted)
            {
                axVSPrinter1.Table = string.Format("^10000;{0}", "Republic of the Philippines");
                //axVSPrinter1.Table = string.Format("^10000;{0}", "PROVINCE OF " + AppSettingsManager.GetConfigValue("PROV"));
                if (AppSettingsManager.GetConfigValue("08") != "")
                    axVSPrinter1.Table = string.Format("^10000;{0}", "PROVINCE OF " + AppSettingsManager.GetConfigValue("08"));
                axVSPrinter1.FontBold = true;
                //axVSPrinter1.Table = string.Format("^10000;{0}", AppSettingsManager.GetConfigValue("02"));
                axVSPrinter1.Table = string.Format("^10000;{0}", AppSettingsManager.GetConfigValue("09"));
                axVSPrinter1.FontBold = false;
                axVSPrinter1.FontUnderline = false;
                axVSPrinter1.FontBold = true;
                // RMC 20161215 display quarter due and date in SOA for Binan (s)
                #region comments
                //if (AppSettingsManager.GetConfigValue("10") == "243")
                //{
                //    axVSPrinter1.Table = "^11000;BUSINESS PERMIT AND LICENSING OFFICE";
                //} // RMC 20161215 display quarter due and date in SOA for Binan (e)
                //else
                //{
                //    if (AppSettingsManager.GetConfigValue("01") == "CITY")
                //        axVSPrinter1.Table = "^11000;OFFICE OF THE " + AppSettingsManager.GetConfigValue("01") + " TREASURER";
                //    else
                //        axVSPrinter1.Table = "^11000;OFFICE OF THE MUNICIPAL TREASURER";
                //}
                #endregion
            }
            else
                axVSPrinter1.Table = "^11000;;;;;";

            //axVSPrinter1.Table = "^11000;S T A T E M E N T  O F  A C C O U N T;";
            axVSPrinter1.Table = "^11000; ";
            axVSPrinter1.Table = "^11000; O R D E R  O F  P A Y M E N T;";    //JARS 20170817 FOR MALOLOS

            axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;

            if(!bPreprinted) //JARS 20171102 LOGO IN SOA
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.back_white, "1.0in", ".30in", "65%", "65%", 10, false);
            }
        }

        private void SOALUBAO()
        {
            OracleResultSet result = new OracleResultSet();

            BPLSAppSettingList sList = new BPLSAppSettingList();
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

            axVSPrinter1.Clear();
            string sImagepath = "C:\\Documents and Settings\\Godfrey\\My Documents\\Downloads\\logo_header.jpg"; // GDE 20111209 ask if this is applicable 
            
            sList.ReturnValuesByBinQue = sBin;
            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.Table = string.Format("^10000;");
                axVSPrinter1.Table = string.Format("^10000;");
                string sCapital = string.Empty; //jhb 20181211
                string sYear, sTerm, sQrt, sTmpGross = string.Empty, sGross = string.Empty, sParticular, sDue, sSurch, sInt, sTotal;
                double dDue, dSurch, dInt, dTotal, dGross;
                double dGrandTotDue = 0, dGrandTotSurch = 0, dGrandTotInt = 0, dGrandTotTotal = 0;
                string sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal, sGrandTotTotal1 = string.Empty;
                string sTerm1 = string.Empty;
                string sTaxCredit = string.Empty;
                string sTaxDebit = string.Empty;
                string sTellerPos = string.Empty;
                double dTaxCredit = 0;
                double dTaxDebit = 0; //JHB 20180503

                sQrt = string.Empty;

                if (sTermPay == "F")
                    //sTerm1 = "FULL YEAR";
                    sTerm1 = "FULL YR";
                else
                {
                    if (sQtrToPay.Trim() == "1")
                        //sTerm1 = "QTRLY-1st Qtr";
                        sTerm1 = "1st Qtr";
                    if (sQtrToPay.Trim() == "2")
                        //sTerm1 = "QTRLY-2nd Qtr";
                        sTerm1 = "2nd Qtr";
                    if (sQtrToPay.Trim() == "3")
                        //sTerm1 = "QTRLY-3rd Qtr";
                        sTerm1 = "3rd Qtr";
                    if (sQtrToPay.Trim() == "4")
                        //sTerm1 = "QTRLY-4th Qtr";
                        sTerm1 = "4th Qtr";
                }

                axVSPrinter1.FontSize = 10;

                //if (bPreprinted == false)
                {
                    CreateHeader();

                    axVSPrinter1.FontSize = 12;
                    //axVSPrinter1.Table = string.Format("^10000;{0}", AppSettingsManager.GetConfigValue("09"));

                    axVSPrinter1.FontBold = false;
                }
                /*else
                {
                    axVSPrinter1.Paragraph = "";
                    axVSPrinter1.Paragraph = "";
                    axVSPrinter1.Paragraph = "";
                    axVSPrinter1.Paragraph = "";
                    axVSPrinter1.Paragraph = "";
                    axVSPrinter1.Paragraph = "";
                }*/
                // RMC 20150616 modified SOA printing
                axVSPrinter1.FontSize = 10;

                axVSPrinter1.Paragraph = "";
                axVSPrinter1.FontBold = false;
                //axVSPrinter1.FontSize = 8;

                axVSPrinter1.FontBold = true;

                axVSPrinter1.Paragraph = "";
                if (bPreprinted == false)
                {

                }

                String strYear = "";
                String sNoofEmps = "";
                String sAssetSize = "0";
                String sArea = "";
                result.Query = "select max(year) from soa_tbl where bin = '" + sBin + "'";
                if (result.Execute())
                    if (result.Read())
                        strYear = result.GetString(0);
                result.Close();

                result.Query = @"select DC.default_desc,OI.data from other_info OI left join default_code DC on dc.default_code = oi.default_code
where DC.rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' and bin = '" + sBin + "' and tax_year = '" + strYear + "' and bns_code = '" + m_strBnsCode + @"' and (
dc.default_desc = 'NUMBER OF SQ METERS' or dc.default_desc = 'NUMBER OF EMPLOYEE'
or dc.default_desc = 'ASSET SIZE')"; //bns_code added MCR 20150127
                if (result.Execute())
                    while (result.Read())
                    {
                        if (result.GetString(0) == "ASSET SIZE")
                            sAssetSize = result.GetDouble(1).ToString("#,##0.00");
                        else if (result.GetString(0) == "NUMBER OF EMPLOYEE") //JARS 20171026
                            //sNoofEmps = result.GetDouble(1).ToString();
                            sNoofEmps = string.Format("{0:###}", result.GetDouble(1));  // RMC 20150616
                        else if (result.GetString(0) == "NUMBER OF SQ METERS")
                            sArea = result.GetDouble(1).ToString();
                    }
                result.Close();
                //MCR 20150102 (e)

                //axVSPrinter1.Table = "<2100|<1000|<1000|<3400|<700|<1600|<1000;BIN|P.STATUS|STATUS|LINE/ NATURE OF BUSINESS|AREA|BILL NO.|BILL DATE";
                //axVSPrinter1.Table = "<2100|<1000|<1000|<3400|<700|<1600|<1000;BIN|Prev Stat|Status|Line/ Nature of Business|Area|Bill No.|Bill Date";
                axVSPrinter1.Table = "<2100|<1000|<1000|<3400|<700|<1600|<1500;BIN|Prev Stat|Status|Line/ Nature of Business||Bill No.|Ref No.";  // RMC 20161208 removed other info in SOA for binan
                axVSPrinter1.FontBold = false;

                if (bPreprinted == false)
                {

                }
                //axVSPrinter1.Table = "<2100|<1000|<1000|<3400|<700|<1600|<1000;" + sBin + "|" + AppSettingsManager.GetPrevBnsStat(sBin) +"|" + sList.BPLSAppSettings[i].sBnsStat.Trim() + "|" + AppSettingsManager.GetBnsDesc(sList.BPLSAppSettings[i].sBnsCode.Trim()) + "|" + sArea + "|" + AppSettingsManager.GetBillNoAndDate(sBin, sSOATaxYear, sList.BPLSAppSettings[i].sBnsCode.Trim(), 0) + "|" + AppSettingsManager.GetBillNoAndDate(sBin, sSOATaxYear, sList.BPLSAppSettings[i].sBnsCode.Trim(), 1);
                axVSPrinter1.Table = "<2100|<1000|<1000|<3400|<700|<1600|<1500;" + sBin + "|" + AppSettingsManager.GetPrevBnsStat(sBin) + "|" + sList.BPLSAppSettings[i].sBnsStat.Trim() + "|" + AppSettingsManager.GetBnsDesc(sList.BPLSAppSettings[i].sBnsCode.Trim()) + "||" + AppSettingsManager.GetBillNoAndDate(sBin, sSOATaxYear, sList.BPLSAppSettings[i].sBnsCode.Trim(), 0) + "|" + GetSOARefNo(sBin);  // RMC 20161208 removed other info in SOA for binan
                axVSPrinter1.FontBold = true;

                if (bPreprinted == false)
                {

                }
                //axVSPrinter1.Table = "<4900|<4900|<1000;BUSINESS NAME|BUSINESS ADDRESS|YEAR";
                axVSPrinter1.Table = "<4900|<4900|<1000;Business Name|Business Address|Year";
                axVSPrinter1.FontBold = false;

                if (bPreprinted == false)
                {

                }
                axVSPrinter1.Table = "<4900|<4900|<1000;" + m_strBnsNm + "|" + m_strBnsAdd + "|" + sSOATaxYear.Trim();
                axVSPrinter1.FontBold = true;

                if (bPreprinted == false)
                {

                }
                //axVSPrinter1.Table ="<3400|<2000|<2600|<1600|<1200;OWNER/ TAXPAYER|LAST PAYMENT|PREVIOUS PERMIT NO.|DATE ISSUED|PERIOD";
                //axVSPrinter1.Table = "<3400|<2000|<2600|<1600|<1200;Owner/ Taxpayer|Last Payment|Prev Permit No.|Date Issued|Period";
                axVSPrinter1.Table = "<3400|<2000|<2600|<1600|<1200;Owner/ Taxpayer|Last Payment|Prev Permit No.|Date Last Issued|Period"; //JHB 20180621
                axVSPrinter1.FontBold = false;

                //JHB 20180621 (s) display date last issued SOA 
                DateTime d = DateTime.Now;
                string LDate = d.ToString("MM/dd/yyyy");
                DateTime LDateIssued = new DateTime();
                OracleResultSet SOA = new OracleResultSet();
                SOA.Query = "SELECT * FROM (SELECT trans_in FROM trans_log  where  bin ='" + sBin + "'";
                SOA.Query += "and to_char(trans_in, 'MM/dd/yyyy')  <> '" + LDate + "'  and trans_code = 'SOA' ORDER BY trans_in DESC) WHERE ROWNUM = 1 ";
                if (SOA.Execute())
                {
                    if (SOA.Read())
                    {
                        LDateIssued = SOA.GetDateTime("trans_in");
                        LDate = string.Format("{0:MM/dd/yyyy}", LDateIssued);
                    }


                }
                SOA.Close();

                //pSet.Close();   
                //JHB 20180621 (e) display date last issued SOA 


                if (bPreprinted == false)
                {

                }

                m_strBnsOwn = AppSettingsManager.GetOwnCode(sBin);


                //axVSPrinter1.Table = "<3400|<2000|<2600|<1600|<1200;" + AppSettingsManager.GetBnsOwner(m_strBnsOwn) + "|" + AppSettingsManager.GetLastPaymentInfo(sBin, "AMOUNT") + "|" + sList.BPLSAppSettings[i].sPermitNo + "|" + sList.BPLSAppSettings[i].sPermitDate + "|" + sTerm1;
                axVSPrinter1.Table = "<3400|<2000|<2600|<1600|<1200;" + AppSettingsManager.GetBnsOwner(m_strBnsOwn) + "|" + AppSettingsManager.GetLastPaymentInfo(sBin, "AMOUNT") + "|" + sList.BPLSAppSettings[i].sPermitNo + "|" + LDate + "|" + sTerm1; //JHB20180621
                if (bPreprinted == false)
                {

                }
                OracleResultSet pSet = new OracleResultSet();
                this.axVSPrinter1.SpaceBefore = 100;
                if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "REN" || sList.BPLSAppSettings[i].sBnsStat.Trim() == "RET")
                {
                    axVSPrinter1.FontBold = true;
                    // RMC 20150426 QA corrections (s)
                    if (AppSettingsManager.GetConfigValue("10") == "011")
                        axVSPrinter1.Table = string.Format("<3500|<2000|<2000|<1500|<1500;Business Line|||No. of Emp|Capital");
                    else// RMC 20150426 QA corrections (e)
                        //axVSPrinter1.Table = string.Format("<3500|<2000|<2000|<1500|<1500;Business Line|Previous Capital|Last gross Receipts|No. of Emp|Capital");
                        axVSPrinter1.Table = string.Format("<3500|<2000|<2000|<1500|<1500;||||");   // RMC 20161208 removed other info in SOA for binan
                    axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.SpaceBefore = 0;

                    int iCnt = 0;
                    pSet.Query = "select count(*) as Cnt from soa_tbl where bin = '" + sBin + "' and fees_code_sort like '%B%'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                            iCnt = pSet.GetInt(0);
                    }
                    pSet.Close();

                    String sData = "";
                    String sTemp = "";
                    //sData = AppSettingsManager.GetBnsDesc(AppSettingsManager.GetBnsCodeMain(sBin)) + "|"; // RMC 20161208 removed other info in SOA for binan put rem
                    sData = "|";   // RMC 20161208 removed other info in SOA for binan

                    int iYearTmp = 0;
                    if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "REN")
                    {
                        iYearTmp = Convert.ToInt32(sSOATaxYear) - 1;
                        result.Query = "select capital,gross from bill_gross_info where tax_year = '" + iYearTmp + "' and bin = '" + sBin.Trim() + "' and bns_stat = 'REN'";
                    }
                    else if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "RET") //MCR 20150114
                    {
                        iYearTmp = Convert.ToInt32(sSOATaxYear);
                        result.Query = "select capital,gross from bill_gross_info where tax_year = '" + iYearTmp + "' and bin = '" + sBin.Trim() + "' and bns_stat = 'RET'";
                    }
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            // RMC 20150426 QA corrections (s)
                            if (AppSettingsManager.GetConfigValue("10") == "011")
                                sData += "|";
                            else// RMC 20150426 QA corrections (e)
                            {
                                /*
                                sData += result.GetDouble(0).ToString("#,##0.00");
                                sData += "|" + result.GetDouble(1).ToString("#,##0.00");
                                */
                                // RMC 20161208 removed other info in SOA for binan put rem

                                sData += "|";   // RMC 20161208 removed other info in SOA for binan   
                            }
                        }
                        else
                        {
                            result.Close();
                            if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "REN")
                                result.Query = "select capital,gr_1 from businesses where tax_year = '" + iYearTmp + "' and bin = '" + sBin.Trim() + "' ";
                            else if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "RET") //MCR 20150114
                                result.Query = "select capital,gr_1 from business_que where tax_year = '" + iYearTmp + "' and bin = '" + sBin.Trim() + "' ";

                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    // RMC 20150426 QA corrections (s)
                                    if (AppSettingsManager.GetConfigValue("10") == "011")
                                        sData += "|";
                                    else// RMC 20150426 QA corrections (e)
                                    {
                                        /*
                                        sData += result.GetDouble("capital").ToString("#,##0.00");
                                        sData += "|" + result.GetDouble("gr_1").ToString("#,##0.00");
                                       */
                                        // RMC 20161208 removed other info in SOA for binan put rem
                                        sData += "|";   // RMC 20161208 removed other info in SOA for binan   
                                    }
                                }
                                else
                                    sData += "|";
                            }
                            result.Close();
                        }
                    }
                    result.Close();

                    //sData += "|" + sNoofEmps + "|" + Convert.ToDouble(sAssetSize).ToString("#,##0.00");
                    sData += "||"; // RMC 20161208 removed other info in SOA for binan
                    axVSPrinter1.Table = string.Format("<3500|<2000|<2000|<1500|<1500;" + sData);

                    pSet.Query = "select capital,gross,bns_code_main from addl_bns where bin = '" + sBin + "' and tax_year = '" + Convert.ToString(AppSettingsManager.GetSystemDate().Year - 1) + "'";
                    if (pSet.Execute())
                    {
                        while (pSet.Read())
                        {
                            sData = AppSettingsManager.GetBnsDesc(pSet.GetString("bns_code_main")) + "|";

                            if (AppSettingsManager.GetConfigValue("10") == "243" || AppSettingsManager.GetConfigValue("10") == "216")   // RMC 20161208 removed other info in SOA for binan  // RMC 20180115 removed other info in SOA for Malolos
                                sData += "|";
                            else
                            {
                                sData += pSet.GetDouble(0).ToString("#,##0.00");
                                sData += "|" + pSet.GetDouble(1).ToString("#,##0.00");
                            }

                            axVSPrinter1.Table = string.Format("<3500|<2000|<2000;" + sData);
                        }
                    }
                    pSet.Close();
                }
                else
                {
                    pSet.Query = "select capital from business_que where bin = '" + sBin + "'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            sAssetSize = pSet.GetDouble("capital").ToString("#,##0.00");
                        }
                    }
                    pSet.Close();
                    this.axVSPrinter1.SpaceBefore = 0;
                    String sData = "";
                    axVSPrinter1.Table = string.Format("<7500|<1500|<1500;|No. of Emp|Capital");
                    sData += "|" + sNoofEmps + "|" + Convert.ToDouble(sAssetSize).ToString("#,##0.00");
                    //sData += "||";  // RMC 20161208 removed other info in SOA for binan
                    axVSPrinter1.Table = string.Format("<7500|<1500|<1500;" + sData);
                }
                #region comments
                /*
                 if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "REN")
                {
                    int iCnt = 0;
                    OracleResultSet pSet = new OracleResultSet();
                    pSet.Query = "select count(*) as Cnt from soa_tbl where bin = '" + sBin + "' and fees_code_sort like '%B%'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                            iCnt = pSet.GetInt(0);
                    }
                    pSet.Close();

                    string sTemp = "";
                    sTemp = AppSettingsManager.GetBnsDesc(AppSettingsManager.GetBnsCodeMain(sBin));

                    axVSPrinter1.FontBold = true;
                    string sData = string.Empty;

                    if (iCnt == 1)
                        sData = "Previous Capital:|Last gross Receipts:|Number of Employees:|Capital:";
                    else
                    {
                        axVSPrinter1.Table = string.Format("<3000;" + sTemp);
                        sData = "Previous Capital:|Last gross Receipts:|Number of Employees:|Capital:";
                    }
                    axVSPrinter1.Table = string.Format("<3000|<3000|<3000|<3000;" + sData);

                    sData = "";

                    int iYearTmp = Convert.ToInt32(sSOATaxYear) - 1;
                    result.Query = "select capital,gross from bill_gross_info where tax_year = '" + iYearTmp + "' and bin = '" + sBin.Trim() + "' and bns_stat = 'REN'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sData = result.GetDouble(0).ToString("#,##0.00");
                            sData += "|" + result.GetDouble(1).ToString("#,##0.00");
                        }
                        else
                        {
                            result.Close();
                            result.Query = "select capital,gr_1 from businesses where tax_year = '" + iYearTmp + "' and bin = '" + sBin.Trim() + "' ";//and bns_stat = 'REN'";
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    sData = result.GetDouble("capital").ToString("#,##0.00");
                                    sData += "|" + result.GetDouble("gr_1").ToString("#,##0.00");
                                }
                            }
                            result.Close();
                        }
                    }
                    result.Close();

                    sData += "|" + sNoofEmps + "|" + Convert.ToDouble(sAssetSize).ToString("#,##0.00");

                    axVSPrinter1.FontBold = false;
                    axVSPrinter1.Table = string.Format("<3000|<3000|3000|<3000;" + sData);

                    if (iCnt > 1)
                    {
                        pSet.Query = "select capital,gross,bns_code_main from addl_bns where bin = '" + sBin + "' and tax_year = '" + Convert.ToString(AppSettingsManager.GetSystemDate().Year - 1) + "'";
                        if (pSet.Execute())
                        {
                            while (pSet.Read())
                            {
                                sTemp = AppSettingsManager.GetBnsDesc(pSet.GetString("bns_code_main"));
                                axVSPrinter1.FontBold = true;

                                axVSPrinter1.Table = string.Format("<3000;" + sTemp);
                                sData = "Previous Capital:|Last gross Receipts:";
                                axVSPrinter1.Table = string.Format("<3000|<3000;" + sData);
                                sData = "";
                       
                                sData = result.GetDouble(0).ToString("#,##0.00");
                                sData += "|" + result.GetDouble(1).ToString("#,##0.00");
                              
                                axVSPrinter1.FontBold = false;
                                axVSPrinter1.Table = string.Format("<3000|<3000;" + sData);
                            }
                        }
                        pSet.Close();
                    }
                }
                */
                #endregion
                //axVSPrinter1.Paragraph = "";

                axVSPrinter1.FontSize = 8;

                //axVSPrinter1.Paragraph = "";
                //axVSPrinter1.Paragraph = "";

                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.FontBold = false;
                axVSPrinter1.FontName = "Arial Narrow";
                axVSPrinter1.FontSize = 9;
                axVSPrinter1.Paragraph = "";
                //if (bPreprinted == false) // RMC 20150616 modified SOA printing
                {
                    axVSPrinter1.FontSize = 10;
                    lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
                    /*axVSPrinter1.DrawLine(250, lngY1, 11300, lngY1);
                    axVSPrinter1.DrawLine(250, lngY1, 250, lngY1 + 180); // LEFT
                    axVSPrinter1.DrawLine(11300, lngY1, 11300, lngY1 + 180); // RIGHT*/

                    //JARS 20170817
                    axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
                    #region comments
                    /*if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "REN" || sList.BPLSAppSettings[i].sBnsStat.Trim() == "RET")
                        //axVSPrinter1.Table = "^600|^800|<3000|>1200|>1500|>1300|>1300|>1300;YEAR|PERIOD|PARTICULARS|GROSS|DUE|SURCHARGE|INTEREST|TOTAL";
                        // RMC 20150921 corrections in SOA of Bin with Permit Update transaction (s)
                        if(ValidatePUTWithNew(sBin))
                            axVSPrinter1.Table = "^600|^800|^3000|^1200|^1500|^1300|^1300|^1300;YEAR|PERIOD|PARTICULARS|GROSS/CAPITAL|DUE|SURCHARGE|INTEREST|TOTAL";
                        else// RMC 20150921 corrections in SOA of Bin with Permit Update transaction (e)
                            axVSPrinter1.Table = "^600|^800|^3000|^1200|^1500|^1300|^1300|^1300;YEAR|PERIOD|PARTICULARS|GROSS|DUE|SURCHARGE|INTEREST|TOTAL";
                    else
                        //axVSPrinter1.Table = "^600|^800|<3000|>1200|>1500|>1300|>1300|>1300;YEAR|PERIOD|PARTICULARS|CAPITAL|DUE|SURCHARGE|INTEREST|TOTAL";
                        axVSPrinter1.Table = "^600|^800|^3000|^1200|^1500|^1300|^1300|^1300;YEAR|PERIOD|PARTICULARS|CAPITAL|DUE|SURCHARGE|INTEREST|TOTAL";
                    // RMC 20150107
                     */
                    // RMC 20170118 adjust width of gross column in SOA, put rem
                    #endregion
                    // RMC 20170118 adjust width of gross column in SOA (s)
                    axVSPrinter1.FontSize = 7;
                    axVSPrinter1.FontBold = true;
                    if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "REN" || sList.BPLSAppSettings[i].sBnsStat.Trim() == "RET")
                        if (ValidatePUTWithNew(sBin))
                            axVSPrinter1.Table = "^600|^800|^3000|^1800|^1500|^1000|^1000|^1300;YEAR|PERIOD|PARTICULARS|GROSS/CAPITAL|DUE|SURCHARGE|INTEREST|TOTAL";
                        else
                            axVSPrinter1.Table = "^600|^800|^3000|^1800|^1500|^1000|^1000|^1300;YEAR|PERIOD|PARTICULARS|GROSS|DUE|SURCHARGE|INTEREST|TOTAL";
                    else
                        axVSPrinter1.Table = "^600|^800|^3000|^1800|^1500|^1000|^1000|^1300;YEAR|PERIOD|PARTICULARS|CAPITAL|DUE|SURCHARGE|INTEREST|TOTAL";
                    // RMC 20170118 adjust width of gross column in SOA (e)
                    axVSPrinter1.FontBold = false;

                    axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
                    //axVSPrinter1.DrawLine(250, lngY1, 11300, lngY1);
                    axVSPrinter1.FontSize = 9;
                }
                /*else
                    axVSPrinter1.Paragraph = "";*/
                // RMC 20150616 modified SOA printing

                axVSPrinter1.Paragraph = "";
                //axVSPrinter1.Paragraph = "";

                axVSPrinter1.FontSize = 10;

                ArrayList arrParticulars = new ArrayList();
                ArrayList arrParticularsCode = new ArrayList();

                int iTmpCnt = 0;
                result.Query = @"select count(distinct(year)) as Cnt from soa_tbl where bin = '" + sBin + "' order by year asc";
                if (result.Execute())
                    if (result.Read())
                        iTmpCnt = result.GetInt(0);
                result.Close();

                //MCR 20150112 (s)
                String sTmpYear = "";
                double SubDue = 0;
                double SubSurch = 0;
                double SubInt = 0;
                double SubTotal = 0;
                //MCR 20150112 (e)

                // RMC 20161218 arranged sorting of tax and fees in SOA (s)
                OracleResultSet resultr = new OracleResultSet();
                string sTaxYear = string.Empty;

                resultr.Query = "select distinct year from soa_tbl where bin = '" + sBin + "' order by year ";
                if (resultr.Execute())
                {
                    while (resultr.Read())
                    {
                        sTaxYear = resultr.GetString(0);

                        for (int ixy = 1; ixy <= 2; ixy++)
                        {
                            //result.Query = "select * from soa_tbl where bin = '" + sBin + "' order by year asc, fees_code_sort desc, qtr asc";        // RMC 20161218 arranged sorting of tax and fees in SOA, put rem


                            if (ixy == 1)
                                result.Query = "select * from soa_tbl where bin = '" + sBin + "' and fees_code_sort like 'B%' and year = '" + sTaxYear + "' order by year asc, fees_code_sort asc, qtr asc";
                            else
                                result.Query = "select * from soa_tbl where bin = '" + sBin + "' and fees_code_sort not like 'B%' and year = '" + sTaxYear + "' order by year asc, fees_code_sort asc, qtr asc";
                            // RMC 20161218 arranged sorting of tax and fees in SOA (e)
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
                                    #region comments
                                    //JARS 20170830 REM DOES NOT APPLY TO MALOLOS BPLS
                                    //// RMC 20170119 limit display of gross is soa for quarterly (s)
                                    //if ((sTerm == "Q" && sQrt == "1") || (sTerm == "F" && sQrt == "Y") || (sTerm == "F" && sQrt == "X"))
                                    //{ }
                                    //else
                                    //{
                                    //    if (sQrt == "A")    // RMC 20170505 display gross in SOA for rev-exam adjustment
                                    //    { }
                                    //    else
                                    //        dGross = 0;
                                    //}
                                    //// RMC 20170119 limit display of gross is soa for quarterly (e)
                                    #endregion
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

                                    if (sParticular.Contains("TAX ON"))
                                    {
                                        arrParticulars.Add(sParticular);
                                        arrParticularsCode.Add(result.GetString("fees_code_sort"));
                                    }

                                    //if (sParticular.Contains("MAYORS PERMIT"))
                                    if (sParticular.Contains("MAYORS PERMIT") || sParticular.Contains("MAYOR'S PERMIT"))    // RMC 20160512 corrected display of Mayor's permit particular in SOA
                                    {
                                        for (int j = 0; j < arrParticularsCode.Count; j++)
                                        {
                                            string a = result.GetString("fees_code_sort").Remove(0, 3).ToString();
                                            if (arrParticularsCode[j].ToString().Contains(result.GetString("fees_code_sort").Remove(0, 3).ToString()))
                                            {
                                                sParticular += " (" + arrParticulars[j].ToString().Remove(0, 7) + ")";
                                                break;
                                            }
                                        }
                                    }
                                    //MCR 20150112 (s)
                                    if (iTmpCnt > 1)
                                    {
                                        if (sTmpYear == "")
                                        {
                                            sTmpYear = sYear;
                                            SubDue += dDue;
                                            SubSurch += dSurch;
                                            SubInt += dInt;
                                            SubTotal += dTotal;
                                        }
                                        else if (sYear == sTmpYear)
                                        {
                                            sTmpYear = sYear;
                                            SubDue += dDue;
                                            SubSurch += dSurch;
                                            SubInt += dInt;
                                            SubTotal += dTotal;
                                        }
                                        else if (sYear != sTmpYear)
                                        {
                                            sTmpYear = sYear;
                                            axVSPrinter1.FontBold = true;
                                            //axVSPrinter1.Table = string.Format("^600|^800|<3000|>1200|>1500|>1300|>1300|>1300;{0}|{1} {2}|{3}|{4}|{5}|{6}|{7}|{8};", "", "", "", "", "Sub Total:", SubDue.ToString("#,##0.00"), SubSurch.ToString("#,##0.00"), SubInt.ToString("#,##0.00"), SubTotal.ToString("#,##0.00"));
                                            axVSPrinter1.Table = string.Format("^600|^800|<3000|>1800|>1500|>1000|>1000|>1300;{0}|{1} {2}|{3}|{4}|{5}|{6}|{7}|{8};", "", "", "", "", "Sub Total:", SubDue.ToString("#,##0.00"), SubSurch.ToString("#,##0.00"), SubInt.ToString("#,##0.00"), SubTotal.ToString("#,##0.00")); // RMC 20170118 adjust width of gross column in SOA
                                            axVSPrinter1.FontBold = false;
                                            SubDue = dDue;
                                            SubSurch = dSurch;
                                            SubInt = dInt;
                                            SubTotal = dTotal;
                                        }
                                    }
                                    //MCR 20150112 (e)
                                    //axVSPrinter1.Table = string.Format("^600|^800|<3000|>1200|>1500|>1300|>1300|>1300;{0}|{1} {2}|{3}|{4}|{5}|{6}|{7}|{8};", sYear, sTerm, sQrt, sParticular, sGross, sDue, sSurch, sInt, sTotal);

                                    axVSPrinter1.Table = string.Format("^600|^800|<3000|>1800|>1500|>1000|>1000|>1300;{0}|{1} {2}|{3}|{4}|{5}|{6}|{7}|{8};", sYear, sTerm, sQrt, sParticular, sGross, sDue, sSurch, sInt, sTotal);  // RMC 20170118 adjust width of gross column in SOA


                                }
                            }
                            result.Close();
                        }


                    }
                }
                resultr.Close();

                //axVSPrinter1.FontSize = 9;    
                //Last Subtotal MCR 20150112 (s)
                if (iTmpCnt > 1)
                {
                    axVSPrinter1.FontBold = true;
                    //axVSPrinter1.Table = string.Format("^600|^800|<3000|>1200|>1500|>1300|>1300|>1300;{0}|{1} {2}|{3}|{4}|{5}|{6}|{7}|{8};", "", "", "", "", "Sub Total:", SubDue.ToString("#,##0.00"), SubSurch.ToString("#,##0.00"), SubInt.ToString("#,##0.00"), SubTotal.ToString("#,##0.00"));
                    axVSPrinter1.Table = string.Format("^600|^800|<3000|>1800|>1500|>1000|>1000|>1300;{0}|{1} {2}|{3}|{4}|{5}|{6}|{7}|{8};", "", "", "", "", "Sub Total:", SubDue.ToString("#,##0.00"), SubSurch.ToString("#,##0.00"), SubInt.ToString("#,##0.00"), SubTotal.ToString("#,##0.00")); //  RMC 20170118 adjust width of gross column in SOA
                    axVSPrinter1.FontBold = false;
                }
                //MCR 20150112 (e)

                result.Query = "select * from dbcr_memo where bin = '" + sBin.Trim() + "' and (memo like 'REMAINING BALANCE AFTER %' or memo like 'REMAINING BALANCE AFTER ADDING OF TAX CREDIT%') and served = 'N' and multi_pay = 'N'";    // RMC 20140113 corrected display of added tax credit in SOA //JARS 20171010
                //result.Query = "select * from dbcr_memo where own_code = '" + m_strBnsOwn + "' and (memo like 'REMAINING BALANCE AFTER PAYMENT%' or memo like 'REMAINING BALANCE AFTER ADDING OF TAX CREDIT%') and served = 'N' and multi_pay = 'N'"; //JARS 20170704 USED OWN_CODE INSTEAD OF BIN
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dTaxCredit = result.GetDouble("credit");
                        dTaxDebit = result.GetDouble("debit");
                    }


                }
                result.Close();
                // tax credit
                sGrandTotDue = string.Format("{0:#,##0.00}", dGrandTotDue);
                sGrandTotSurch = string.Format("{0:#,##0.00}", dGrandTotSurch);
                sGrandTotInt = string.Format("{0:#,##0.00}", dGrandTotInt);
                sTaxCredit = string.Format("{0:#,##0.00}", dTaxCredit);
                sTaxDebit = string.Format("{0:#,##0.00}", dTaxDebit); //JHB 20180503
                sGrandTotTotal = string.Format("{0:#,##0.00}", dGrandTotTotal);
                if (isCheckedCredit == true) //AFM 20200114 applied condition to display credit correctly (s)
                {
                    sGrandTotTotal1 = string.Format("{0:#,##0.00}", dGrandTotTotal - dTaxCredit);
                    sGrandTotTotal1 = string.Format("{0:#,##0.00}", Convert.ToDouble(sGrandTotTotal1) + dTaxDebit); //dGrandTotTotal //AFM 20200114
                }
                else
                    sGrandTotTotal1 = string.Format("{0:#,##0.00}", sGrandTotTotal + dTaxDebit);
                //AFM 20200114 applied condition to display credit correctly (e)


                axVSPrinter1.Paragraph = "";
                lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
                //axVSPrinter1.DrawLine(5500, lngY1, 11300, lngY1);
                //axVSPrinter1.FontBold = true;
                //axVSPrinter1.Table = string.Format(">5600|>1500|>1300|>1300|>1300;Total:|{0}|{1}|{2}|{3};", sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal);
                //axVSPrinter1.Table = string.Format(">6200|>1500|>1000|>1000|>1300;Total:|{0}|{1}|{2}|{3};", sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal);    // RMC 20170118 adjust width of gross column in SOA
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom; //JARS 20170817
                axVSPrinter1.Table = string.Format(">6200|>1500|>1000|>1000|>1300;SUB-TOTAL|{0}|{1}|{2}|{3};", sGrandTotDue, sGrandTotSurch, sGrandTotInt, sGrandTotTotal); //JARS 20170817
                // RMC 20151002 added display of tax credit in SOA (s)
                if (dTaxCredit > 0 || dTaxDebit > 0)
                {
                    //axVSPrinter1.FontBold = false;
                    //axVSPrinter1.Table = string.Format(">5600|>1500|>2600|>1300;||Available Tax Credit:|{0};", sTaxCredit);
                    if (isCheckedCredit == true) //AFM 20200114 (s)
                    {
                        axVSPrinter1.Table = string.Format(">6200|>1500|>2000|>1300;||Available Tax Credit:|{0};", sTaxCredit); // RMC 20170118 adjust width of gross column in SOA
                    }
                    else
                        axVSPrinter1.Table = string.Format(">6200|>1500|>2000|>1300;||Available Tax Credit:|0.00"); // RMC 20170118 adjust width of gross column in SOA
                    //AFM 20200114 (e)

                    axVSPrinter1.Table = string.Format(">6200|>1500|>2000|>1300;||Available Tax Debit:|{0};", sTaxDebit);  //JHB 20180503
                    axVSPrinter1.FontBold = true;
                    //axVSPrinter1.Table = string.Format(">5600|>1500|>2600|>1300;||Net Due:|{0};", sGrandTotTotal1);
                    axVSPrinter1.Table = string.Format(">6200|>1500|>2000|>1300;||Net Due:|{0};", sGrandTotTotal1); // RMC 20170118 adjust width of gross column in SOA
                }
                // RMC 20151002 added display of tax credit in SOA (e)
                axVSPrinter1.FontBold = false;

                //axVSPrinter1.Paragraph = "";
                axVSPrinter1.Paragraph = "";

                axVSPrinter1.FontSize = 10;
                #region comments
                /*if (sList.BPLSAppSettings[i].sBnsStat.Trim() == "REN")
                {
                    int iYearTmp = Convert.ToInt32(sSOATaxYear) - 1;
                    result.Query = "select capital,gross from bill_gross_info where tax_year = '" + iYearTmp + "' and bin = '" + sBin.Trim() + "' and bns_stat = 'REN'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            axVSPrinter1.Table = "<8500;Previous Capital: " + result.GetDouble(0).ToString("#,##0.00");
                            axVSPrinter1.Table = "<8500;Lastgross Receipts: " + result.GetDouble(1).ToString("#,##0.00");
                        }
                        else
                        {
                            result.Close();
                            result.Query = "select capital,gr_1 from businesses where tax_year = '" + iYearTmp + "' and bin = '" + sBin.Trim() + "' and bns_stat = 'REN'";
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    axVSPrinter1.Table = "<8500;Previous Capital: " + result.GetDouble("capital").ToString("#,##0.00");
                                    axVSPrinter1.Table = "<8500;Lastgross Receipts: " + result.GetDouble("gr_1").ToString("#,##0.00");
                                }
                            }
                        }
                    }
                    result.Close();
                }*/

                //axVSPrinter1.Table = "<8500;Number of Employees: " + sNoofEmps;
                //axVSPrinter1.Table ="<8500;Capital: " + Convert.ToDouble(sAssetSize).ToString("#,##0.00");
                //axVSPrinter1.Paragraph = "";
                #endregion

                #region QuarterlyDues
                // RMC 20161215 display quarter due and date in SOA for Binan (s)
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                string sQ1Due = string.Empty;
                string sQ2Due = string.Empty;
                string sQ3Due = string.Empty;
                string sQ4Due = string.Empty;
                double dDelq = 0;
                double dQ1 = 0;
                double dQ2 = 0;
                double dQ3 = 0;
                double dQ4 = 0;
                double dFees = 0;

                // get previous year dues
                result.Query = "select sum(total) from soa_tbl where bin = '" + sBin + "' ";
                result.Query += " and year < '" + ConfigurationAttributes.CurrentYear + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dDelq = result.GetDouble(0);
                    }
                }
                result.Close();

                // current year fees
                result.Query = "select sum(total) from soa_tbl where bin = '" + sBin + "' ";
                result.Query += "and year = '" + ConfigurationAttributes.CurrentYear + "' and term = 'F'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dFees = result.GetDouble(0);
                    }
                }
                result.Close();

                // get current year per qtr buss tax
                result.Query = "select sum(total) from soa_tbl where bin = '" + sBin + "' ";
                result.Query += "and year = '" + ConfigurationAttributes.CurrentYear + "' and term = 'Q' and qtr = '1'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dQ1 = result.GetDouble(0);
                    }
                }
                result.Close();

                result.Query = "select sum(total) from soa_tbl where bin = '" + sBin + "' ";
                result.Query += "and year = '" + ConfigurationAttributes.CurrentYear + "' and term = 'Q' and qtr = '2'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dQ2 = result.GetDouble(0);
                    }
                }
                result.Close();

                result.Query = "select sum(total) from soa_tbl where bin = '" + sBin + "' ";
                result.Query += "and year = '" + ConfigurationAttributes.CurrentYear + "' and term = 'Q' and qtr = '3'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dQ3 = result.GetDouble(0);
                    }
                }
                result.Close();

                result.Query = "select sum(total) from soa_tbl where bin = '" + sBin + "' ";
                result.Query += "and year = '" + ConfigurationAttributes.CurrentYear + "' and term = 'Q' and qtr = '4'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dQ4 = result.GetDouble(0);
                    }
                }
                result.Close();

                string sCurrQtr = string.Empty;
                sCurrQtr = LibraryClass.GetQtr(AppSettingsManager.GetSystemDate());

                if (sCurrQtr == "1")
                    dQ1 = dDelq + dFees + dQ1;
                else if (sCurrQtr == "2")
                {
                    dQ2 = dDelq + dFees + dQ1 + dQ2;
                    dQ1 = 0;
                }
                else if (sCurrQtr == "3")
                {
                    dQ3 = dDelq + dFees + dQ1 + dQ2 + dQ3;
                    dQ1 = 0;
                    dQ2 = 0;
                } 
                else
                {
                    dQ4 = dDelq + dFees + dQ1 + dQ2 + dQ3 + dQ4;
                    dQ1 = 0;
                    dQ2 = 0;
                    dQ3 = 0;

                }
                string sQ1 = string.Empty;
                string sQ2 = string.Empty;
                string sQ3 = string.Empty;
                string sQ4 = string.Empty;

                sQ1 = string.Format("{0:#,##0.00}", dQ1);
                sQ2 = string.Format("{0:#,##0.00}", dQ2);
                sQ3 = string.Format("{0:#,##0.00}", dQ3);
                sQ4 = string.Format("{0:#,##0.00}", dQ4);

                //JARS 20170817 COMMENT OUT NOT NEEDED IN MALOLOS SOA
                //JARS 20181823 ENABLED THIS SECTION
                axVSPrinter1.Table = "<2200|^2200|^2200|^2200|^2200;Due Date|First Quarter (Jan 20)|Second Quarter (Apr 20)|Third Quarter (Jul 20)|Fourth Quarter (Oct 20)";
                axVSPrinter1.FontBold = true;
                axVSPrinter1.Table = string.Format("<2200|>2200|>2200|>2200|>2200;Amount Due:|{0}|{1}|{2}|{3};", sQ1, sQ2, sQ3, sQ4);
                axVSPrinter1.FontBold = false;
                #endregion


                axVSPrinter1.Paragraph = "";
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom; //JARS 20170817
                axVSPrinter1.Table = string.Format(">6200|>1500|>1000|>1000|>1300;GRAND TOTAL||||{0};", sGrandTotTotal1); //JARS 20170817 //JHB 20180503
                //axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                /*OracleResultSet pRec = new OracleResultSet(); //JARS 20171018
                pRec.Query = "select * from dbcr_memo where bin = '"+sBin+"' and memo like 'CREDIT IN EXCESS TO PAYMENT%' order by ";

                axVSPrinter1.Table = string.Format(">6200|>1500|>1000|>1000|>1300;AVAILABLE TAX CREDIT:||||{0};", sGrandTotTotal); //JARS 20170817*/

                axVSPrinter1.Paragraph = "";
                // RMC 20161215 display quarter due and date in SOA for Binan (e)

                //AFM 20191209 MAO-19-11500 applied based on binan (s)
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.MarginLeft = 8500;

                axVSPrinter1.Table = "<3000;" + AdditionalFee(); // RMC 20181121 Customized Fire Tax Fee computation

                this.axVSPrinter1.MarginLeft = 500;
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                //AFM 20191209 MAO-19-11500 applied based on binan (e)

                // RMC 20180104 display balance dues if installment, based on their old version format (s)
                string sTmpQtrToPay = string.Empty;
                string paticular = string.Empty;
                double gr1 = 0.0;

                //JHB 20180504 Display fees code 12-0706 for bns_code B0706 only (s)
               // result.Query = "select * from soa_tbl where bin = '" + sBin + "' ";
                result.Query = "select T2.gr_1 as GROSS, T1.particulars as particulars from soa_tbl T1" ;
                result.Query += " left join  businesses T2 on T1.bin = T2.bin where T2.bin =  '" + sBin + "' ";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        paticular = result.GetString("particulars");
                        gr1 = result.GetDouble("GROSS");
                    }
                }
                result.Close();

                if (paticular == "FRANCHISE TAX")
                {
                    result.Query = "select max(qtr) from soa_tbl where bin = '" + sBin + "' and fees_code_sort like '12%' and term = 'Q' and year = '" + ConfigurationAttributes.CurrentYear + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sTmpQtrToPay = result.GetString(0);
                        }
                    }
                    result.Close();
                }
                else
                {
                    result.Query = "select max(qtr) from soa_tbl where bin = '" + sBin + "' and fees_code_sort like 'B%' and term = 'Q' and year = '" + ConfigurationAttributes.CurrentYear + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sTmpQtrToPay = result.GetString(0);
                        }
                    }
                    result.Close();
                }
                // 20180627 (E) AS PER MALOLOS REQUEST
                result.Query = "select * from pay_temp where bin = '" + sBin + "' and qtr > '" + sTmpQtrToPay + "' and qtr <> 'F' and tax_year = '" + ConfigurationAttributes.CurrentYear + "' order by qtr asc";
                if (result.Execute())
                {
                    string sQtr = string.Empty;
                    string sQtrDues = string.Empty;
                    double dQtrDues = 0;
                    int iCnt = 0;
                    while (result.Read())
                    {
                        iCnt++;

                        axVSPrinter1.FontUnderline = true;
                        if (iCnt == 1)
                        {
                            axVSPrinter1.Table = string.Format("<2000|>2500;BALANCE DUES:|");
                            axVSPrinter1.Table = string.Format("<2000|>2500;QTR - YEAR:|");
                        }
                        axVSPrinter1.FontUnderline = false;

                        sQtr = result.GetString("qtr") + " - " + result.GetString("tax_year");
                        dQtrDues += result.GetDouble("fees_totaldue");
                        sQtrDues = string.Format("{0:#,###.00}", result.GetDouble("fees_totaldue"));

                        axVSPrinter1.Table = string.Format("<2000|>2500;{0}|{1}", sQtr, sQtrDues);

                    }

                    if (dQtrDues > 0)
                    {
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.Table = string.Format("<2000|>2500;TOTAL BALANCE|{0}", string.Format("{0:#,###.00}", dQtrDues));
                        if (paticular == "FRANCHISE TAX")
                        axVSPrinter1.Table = string.Format("<2000|>2500;GROSS DECLARED|{0}", string.Format("{0:#,###.00}", gr1));
                        axVSPrinter1.Paragraph = "";

                    }
                }
                result.Close();
                // RMC 20180104 display balance dues if installment, based on their old version format (e)

                if (m_objSystemUser.Load(AppSettingsManager.SystemUser.UserCode))
                {
                    sTellerName = m_objSystemUser.UserName;
                    sTellerPos = m_objSystemUser.Position;
                }

                axVSPrinter1.Table = "<8500;This Statement is valid until " + AppSettingsManager.ValidUntil(sBin, ConfigurationAttributes.CurrentYear);
                axVSPrinter1.Table = "<8500;You can register at " + AppSettingsManager.GetConfigValue("76") + " and apply online for renewal"; //MCR 20210616
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.Table = "<2500|<1500|<1500|<2500;Prepared by:|||"; //JARS 20170818
                axVSPrinter1.Table = "^3000|<2000|<1500|<2500;" + string.Format("{0:0}", sTellerName) + "|||";

                // RMC 20180117 display date and time printed in Soa (s)
                axVSPrinter1.Table = "<2000|<2500|<1000|<2500;Print Date & Time:|" + AppSettingsManager.GetCurrentDate() + "||";
                // RMC 20180117 display date and time printed in Soa (e)

                //axVSPrinter1.Table = "<8500;Please disregard this statement if payment has been made. Thank you.";
                axVSPrinter1.FontSize = 10;

                axVSPrinter1.Paragraph = "";
                //axVSPrinter1.Paragraph = "";

                axVSPrinter1.FontSize = 10;
                // RMC 20161208 modified SOA for Binan (s)
                if (AppSettingsManager.GetConfigValue("10") == "243")
                {
                    axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|||Assessed By:");
                    axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|||;");
                    axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|||{0};", AppSettingsManager.GetConfigValue("03"));
                    axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|||BPLO HEAD");
                }// RMC 20161208 modified SOA for Binan (e)
                else if (AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170817
                {
                    //axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|||Recommending Approval:");
                    //axVSPrinter1.Paragraph = "";
                    //axVSPrinter1.Paragraph = "";
                    //axVSPrinter1.Paragraph = "";

                    //MCR 20210701 validation for soa approval (s)
                    string sStatus = string.Empty;
                    pSet.Query = "select status from soa_monitoring where bin = '" + sBin + "' and status = 'APPROVED'";
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            pSet.Close();
                            byte[] blobData = null;
                            pSet.Query = "select signature from BO_SIGNATORIES where isactive = 1";
                            if (pSet.Execute())
                                if (pSet.Read())
                                {
                                    blobData = pSet.GetBlob(0);
                                    Image image = (Bitmap)((new ImageConverter()).ConvertFrom(blobData));
                                    this.axVSPrinter1.DrawPicture(image, "5.8in", "8.4in", "10%", "5%", 11, false);
                                }
                            pSet.Close();

                            blobData = null;
                            pSet.Query = "select qr_code from BO_QR where bin = '" + sBin + "'";
                            if (pSet.Execute())
                                if (pSet.Read())
                                {
                                    blobData = pSet.GetBlob(0);
                                    Image image = (Bitmap)((new ImageConverter()).ConvertFrom(blobData));
                                    this.axVSPrinter1.DrawPicture(image, "4in", "8.5in", "50%", "50%", 11, false);
                                }
                            pSet.Close();
                        }
                    //MCR 20210701 validation for soa approval (s)

                    axVSPrinter1.Table = string.Format(">4400|<3000|<1000|^3000;|||{0};", AppSettingsManager.GetConfigValue("38"));
                    axVSPrinter1.Table = string.Format(">4400|<3000|<1000|^3000;|||" + AppSettingsManager.GetConfigValue("37"));
                    //axVSPrinter1.Paragraph = "";
                    //axVSPrinter1.Paragraph = "";
                    //axVSPrinter1.Table = string.Format(">4400|<3000|<1000|^3000;|||Approved By:");
                    //axVSPrinter1.Paragraph = "";
                    //axVSPrinter1.Paragraph = "";
                    //axVSPrinter1.Table = string.Format(">4400|<3000|<1000|^3000;|||{0};", AppSettingsManager.GetConfigValue("36"));
                    //axVSPrinter1.Table = string.Format(">4400|<3000|<1000|^3000;|||" + AppSettingsManager.GetConfigValue("01") + " MAYOR");
                }
                else
                {
                    axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|Assessed By:||Approved By:");
                    axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|||;");
                    //axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|||;");
                    axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|{0}||{1};", AppSettingsManager.GetConfigValue("03"), AppSettingsManager.GetConfigValue("05"));
                    //axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|BPLO HEAD||MUNICIPAL TREASURER;");
                    axVSPrinter1.Table = string.Format(">4400|<3000|<1500|<2500;|BPLO HEAD||" + AppSettingsManager.GetConfigValue("01") + " TREASURER;");    // RMC 20161206 corrected printing of treasurer position in SOA
                }
                //JARS 20180818 for last payment (s)
                string sLastORDate = "";
                //string sLastTaxYear = "";
                string sLastTerm = "";
                string sLastORNo = ""; //JARS 20171107
                string sCredit = "";
                double dCredit = 0;
                //string sLastPaymentMode = "";
                //string sLastTeller = "";
                string sLastBnsStat = "";
                if (sList.BPLSAppSettings[i].sBnsStat.Trim() != "NEW") //JARS 20171107 HIDE WHEN STAT = NEW
                {
                    axVSPrinter1.FontUnderline = true;
                    axVSPrinter1.Table = "<8500;Additional Details of Last Payment";
                    axVSPrinter1.FontUnderline = false;
                    result.Query = "select * from pay_hist where bin = '" + sBin + "' order by or_date desc";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sLastORNo = result.GetString("or_no");
                            sLastORDate = result.GetDateTime("or_date").ToString("MM/dd/yyyy");
                            sLastTaxYear = result.GetString("tax_year");
                            sLastTerm = result.GetString("qtr_paid");
                            sLastPaymentMode = result.GetString("data_mode");
                            sLastTeller = AppSettingsManager.GetTeller(result.GetString("teller"), 0);
                            sLastBnsStat = result.GetString("bns_stat");
                            sTellerName = m_objSystemUser.UserName;

                            //if (sLastTerm == "F")
                            //{
                            //    sLastTerm = "FULL";
                            //}
                            axVSPrinter1.Table = "<1500|^500|<5000;" + "OR Date: " + "||" + sLastORDate;
                            axVSPrinter1.Table = "<1500|^500|<5000;" + "OR No.: " + "||" + sLastORNo;   // RMC 20180117 display last or no in SOA
                            axVSPrinter1.Table = "<1500|^500|<5000;" + "Tax Year / Qtr: " + "||" + sLastTaxYear + "-" + sLastTerm;

                            if (sLastTerm == "F")
                            {
                                sLastTerm = "Full";
                            }
                            else
                            {
                                sLastTerm = "Installment";
                            }

                            axVSPrinter1.Table = "<1500|^500|<5000;" + "Payment Term: " + "||" + sLastTerm;
                            axVSPrinter1.Table = "<1500|^500|<5000;" + "Teller: " + "||" + sLastTeller;
                            axVSPrinter1.Table = "<1500|^500|<5000;" + "Business Status: " + "||" + sLastBnsStat;
                        }
                    }
                    result.Close();
                    //JARS 20171107 (S) debit/credit info placed in soa instead of billing
                    result.Query = "select or_no, or_date, credit from dbcr_memo where memo like '%" + sLastORNo + "' AND CREDIT > 0";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sCredit = result.GetDouble("credit").ToString("#,##0.00");
                            axVSPrinter1.FontUnderline = true;
                            axVSPrinter1.Table = "<8500;Available Tax Credit for this BIN";
                            axVSPrinter1.FontUnderline = false;
                            axVSPrinter1.Table = "<1500|^500|<5000;" + "Tax Credit: " + "||" + "Php " + sCredit;
                            dCredit = Convert.ToDouble(sCredit);
                            axVSPrinter1.Table = "<1500|^500|<10000;" + "Amount in Words: " + "||" + NumberWording.AmountInWords(dCredit);
                        }
                    }
                    result.Close();
                    //JARS 20171107 (E)
                }


                //JARS 20180818 for last payment (e)
                #region comments
                //axVSPrinter1.Paragraph = "";
                /*axVSPrinter1.Table = "<1500|<3500;Prepared by:|" + string.Format("{0:0}", sTellerName);  // RMC 20111228 changed soa signatory to treas
                axVSPrinter1.Table = "<1500|<3500;Receieved SOA:|";  // RMC 20111228 changed soa signatory to treas
                axVSPrinter1.Table = "<1500|<3500;By:|";  // RMC 20111228 changed soa signatory to treas
                axVSPrinter1.Table = "<1500|<3500|<4000|<1500|^2000;Date Printed:|" + dDate.ToShortDateString() + "   " + dDate.ToLongTimeString() + "|||";
                  */

                //axVSPrinter1.Table = "<1500|<2500|<1500|<2500;Prepared by:|" + string.Format("{0:0}", sTellerName) + "|Date Printed:|" + dDate.ToShortDateString() + "   " + dDate.ToLongTimeString();
                //axVSPrinter1.Table = "<1500|<2500|<1500|<2500;Receieved SOA:||BY:|";  // RMC 20111228 changed soa signatory to treas
                //axVSPrinter1.Table = "<1500|<2500|<1500|<2500;Received TOP:||BY:|";  // RMC 20161208 modified SOA for Binan
                #endregion
            }
        }

        private string AdditionalFee() //AFM 20191209 MAO-19-11500 applied based on binan 
        {
            // RMC 20181121 Customized Fire Tax Fee computation
            string sAddlFee = string.Empty;
            OracleResultSet pSet = new OracleResultSet();
            int iCnt = 0;

            pSet.Query = "select count(*) from fire_tax_tag where rev_year = '" + ConfigurationAttributes.RevYear + "'";
            int.TryParse(pSet.ExecuteScalar(), out iCnt);

            if (iCnt == 0)
            {
                sAddlFee = "";
            }
            else
            {
                //if (AppSettingsManager.GetConfigValue("73") == "Y")
                if (AppSettingsManager.GetConfigValue("74") == "Y") //AFM 20200205
                    sAddlFee = "";
                else
                {
                    double dRate = 0;
                    double dTaxAmt = 0;
                    double dAddlAmt = 0;
                    double dTotAddlAmt = 0;
                    double dMinAmt = 0;

                    pSet.Close();
                    pSet.Query = "select fees_rate from fire_tax_tag where rev_year = '" + ConfigurationAttributes.RevYear + "'";
                    double.TryParse(pSet.ExecuteScalar(), out dRate);

                    //pSet.Query = "select a.fees_code, a.amount, b.fees_code, b.fees_rate from bill_table a, fire_tax_tag b";
                    //pSet.Query+= " where a.fees_code = b.fees_code and a.bin = '" + sBin + "' and ";
                    //pSet.Query+= " b.rev_year = '" + ConfigurationAttributes.RevYear + "' and fees_type = 'TF' order by a.fees_code";

                    //JARS 20190114 HAD TO THINK OF A WAY TO INCLUDE: SURCHARGE AND INTEREST AND DELINQUENT YEARS

                    pSet.Query = "select a.total,a.particulars,b.fees_rate,b.amount From soa_tbl a, fire_tax_tag b where substr(a.fees_code_sort, 0,2) = b.fees_code and ";
                    pSet.Query += "a.bin = '" + sBin + "' and substr(a.fees_code_sort,0,1) <> 'B' ";
                    pSet.Query += "order by a.fees_code_sort, a.year";

                    if (pSet.Execute())
                    {
                        while (pSet.Read())
                        {
                            dTaxAmt = pSet.GetDouble("total");
                            dRate = pSet.GetDouble("fees_rate");

                            dAddlAmt = dTaxAmt * dRate / 100;
                            dTotAddlAmt += dAddlAmt;

                            dMinAmt = pSet.GetDouble("amount"); //MCR 20191022
                        }

                        //MCR 20191022 (s)
                        if (dTotAddlAmt < dMinAmt)
                            dTotAddlAmt = dMinAmt;
                        //MCR 20191022 (e)

                        sAddlFee = "FIRE INSPECTION FEE: " + dTotAddlAmt.ToString("#,##0.00");
                    }
                    pSet.Close();
                }
            }

            return sAddlFee;
        }

        private void model_StartDocEvent(object sender, EventArgs e)
        {

        }

        private void ToolPrint_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintQuality = VSPrinter7Lib.PrintQualitySettings.pqHigh;

            /*if (MessageBox.Show("Are you sure you want to print?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;*/
            // RMC 20180105 remove too much prompts in billing/soa requested by Malolos ralph, put rem
            axVSPrinter1.PrintDoc(1, 1, axVSPrinter1.PageCount);

            //JHB 20190121 add trail of SOA printing (S)

            if (AuditTrail.AuditTrail.InsertTrail("ABPS", "ASS-BILLING-PRINT SOA", "PRINT SOA BIN: " + sBin) == 0)
            {
                return;
            }
            //JHB 20190121 add trail of SOA printing (E)
            
        }

        private void toolSettingPageSetup_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPageSetup);
        }

        private void toolSettingPrintPage_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);
        }

        private bool ValidatePUTWithNew(string sBin)
        {
            // RMC 20150921 corrections in SOA of Bin with Permit Update transaction
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from bill_gross_info where bin = '" + sBin + "' and bns_stat = 'NEW' and due_state = 'P'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                    return true;
                else
                    return false;
            }

            return false;
        }

        private void axVSPrinter1_StartDocEvent(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private string GetSOARefNo(string sBin)
        {
            String sValue = "";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select refno from soa_monitoring where bin = '" + sBin + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sValue = pSet.GetString(0);
            pSet.Close();

            return sValue;
        }
    }
}