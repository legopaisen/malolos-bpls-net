using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;

namespace Amellar.Common.Tools
{
    public partial class frmReport : Form
    {
        private string m_sReportName = "";
        private string m_sQuery = "";
        private string m_sData1 = "";
        private string m_sData2 = "";
        private bool m_bInit = true;
        private bool m_bIncHistory = false; // RMC 20130212 Data query - merged mods from Tarlac ver, inclusion of history or based on year registration
        private DateTime m_dtFrom; // CJC 20130429
        private DateTime m_dtTo; // CJC 20130429
        private string m_sTaxYear = ""; // JHMN 20170127 added for verify businesses

        public string ReportName
        {
            get { return m_sReportName; }
            set { m_sReportName = value; }
        }

        public string Query
        {
            get { return m_sQuery; }
            set { m_sQuery = value; }
        }

        public string Data1
        {
            get { return m_sData1; }
            set { m_sData1 = value; }
        }

        public string Data2
        {
            get { return m_sData2; }
            set { m_sData2 = value; }
        }

        public bool HistoryIncluded
        {
            get { return m_bIncHistory; }
            set { m_bIncHistory = value; }
        }

        public DateTime DateFrom // CJC 20130429
        {
            get { return m_dtFrom; }
            set { m_dtFrom = value; }
        }

        public DateTime DateTo // CJC 20130429
        {
            get { return m_dtTo; }
            set { m_dtTo = value; }
        }

        public string TaxYear// JHMN 20170127 added for verify businesses (s)
        {
            get { return m_sTaxYear; }
            set { m_sTaxYear = value; }
        }// JHMN 20170127 added for verify businesses (e)

        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            m_bInit = true;
            this.ExportFile();

            if (m_sReportName == "LIST OF BUSINESSES BY DATA QUERY")
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 1500; //MOD: MCR 20130331 OLD 1000
                this.axVSPrinter1.MarginTop = 1000;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateHeader();
                PrintDataQuery();
            }
            else if (m_sReportName == "LIST OF BUSINESSES BY GENDER") // CJC 20130429
            {

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 1000;
                this.axVSPrinter1.MarginTop = 1000;
                this.axVSPrinter1.MarginBottom = 1000;

                //this.ExportFile();

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateHeader();
                ListByGender();
            }
            else if (m_sReportName == "LIST OF BUSINESSES WITH EMPLOYEE GENDER")   // RMC 20140105 added report with employee gender (dilg report -requested by mati)
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 1000;
                this.axVSPrinter1.MarginTop = 1000;
                this.axVSPrinter1.MarginBottom = 1000;

                //this.ExportFile();

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateHeader();
                ListByEmpGender();
            }
            //else if (m_sReportName == "VERIFY BUSINESSES")   // JHMN 20170127 verify businesses module (s)
            else if (m_sReportName == "VERIFIED BUSINESSES")    // RMC 20170221 addl mods in report of Verified businesses
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 1500;
                this.axVSPrinter1.MarginTop = 1000;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateHeader();
                VerifyBusinesses();
            }// JHMN 20170127 verify businesses module (e)
            else
            {

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 1000;
                this.axVSPrinter1.MarginTop = 1000;
                this.axVSPrinter1.MarginBottom = 1000;

                //this.ExportFile();

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateHeader();
                ListHoldRecords();
            }

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
            axVSPrinter1.ExportFile = "";
        }

        private void ExportFile()
        {
            string strCurrentDirectory = Directory.GetCurrentDirectory();
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Export to HTML";
                dlg.Filter = "HTML documents (*.html)|*.html";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    axVSPrinter1.ExportFile = dlg.FileName;
                }
            }
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            if (!m_bInit)
                CreateHeader();

            m_bInit = false;

            this.axVSPrinter1.Footer = string.Format("{0}||{1}", "Printed by: " + AppSettingsManager.SystemUser.UserCode + "  Date:" + AppSettingsManager.GetCurrentDate(), "Page " + axVSPrinter1.PageCount.ToString() + "                "); // JHMN 20170307 added to fit the footer in legal paper size
            
        }

        private void CreateHeader()
        {
            string strProvinceName = string.Empty;
            string sData = "";
            //this.axVSPrinter1

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)10.0;
            if (m_sReportName == "LIST OF BUSINESSES BY DATA QUERY")
                this.axVSPrinter1.Table = "^19860;Republic of the Philippines";
            else
                this.axVSPrinter1.Table = "^16000;Republic of the Philippines";
            this.axVSPrinter1.FontBold = false;

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (m_sReportName == "LIST OF BUSINESSES BY DATA QUERY")
            {
                if (strProvinceName != string.Empty)
                    //this.axVSPrinter1.Table = string.Format("^19860;{0}", strProvinceName);
                    this.axVSPrinter1.Table = string.Format("^19860;PROVINCE OF {0}", strProvinceName); // RMC 20150429 
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("^19860;{0}", AppSettingsManager.GetConfigValue("09"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = string.Format("^19860;{0}", AppSettingsManager.GetConfigValue("41"));
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = string.Format("^19860;{0}", m_sReportName);

                axVSPrinter1.FontBold = true;

                if (m_sData1 == "")
                    m_sData1 = "ALL";
                if (m_sData2 == " ")
                    m_sData2 = "BIN";
                
                this.axVSPrinter1.FontSize = (float)8.0;
                this.axVSPrinter1.Table = string.Format("<16860;CRITERIA: " + m_sData1);
                this.axVSPrinter1.Table = string.Format("<16860;SORTED BY: " + m_sData2);
                // RMC 20130212 Data query - merged mods from Tarlac ver, inclusion of history or based on year registration (S)
                if (m_bIncHistory)
                    this.axVSPrinter1.Table = string.Format("<19860;NOTE: HISTORY INCLUDED");
                // RMC 20130212 Data query - merged mods from Tarlac ver, inclusion of history or based on year registration (E)


                sData = "BIN|BUSINESS NAME|OWNER|GENDER|ADDRESS|TAX YEAR|CLASSIFICATION|STATUS|GROSS/CAP|TOTAL GROSS/CAP|BILL AMOUNT|AMOUNT PAID|DUE QTR|NO. OF EMP";
                //this.axVSPrinter1.Table = string.Format("^1550|^2430|^2010|^800|^2010|^800|^2010|^800|^1500|^1500|^1250|^1250|^900|^900;{0}", sData);
                this.axVSPrinter1.Table = string.Format("^1050|^2430|^1810|^800|^1810|^800|^2010|^800|^1100|^1100|^950|^950|^500|^700;{0}", sData); // JHMN 20170307 edited columns
                axVSPrinter1.FontBold = false;
                //this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportName == "LIST OF BUSINESSES BY GENDER") // CJC 20130429
            {
                if (strProvinceName != string.Empty)
                    this.axVSPrinter1.Table = string.Format("^16000;{0}", "PROVINCE OF " + strProvinceName);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("09"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("41"));
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = (float)12.0;
                this.axVSPrinter1.Table = string.Format("^13000;{0}", m_sReportName);
                this.axVSPrinter1.FontSize = (float)8.0;
                this.axVSPrinter1.Table = string.Format("^13000;Date Period: {0:MMM dd, yyyy} - {1:MMM dd, yyyy}", m_dtFrom, m_dtTo);

                axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                sData = "B I N|BUSINESS NAME|BUSINESS ADDRESS|TAX PAYER'S NAME|TAX PAYER'S ADDRESS";
                this.axVSPrinter1.Table = string.Format("^1800|^3500|^3500|^3500|^3500;{0}", sData);
                axVSPrinter1.FontBold = false;
                //this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportName == "LIST OF BUSINESSES WITH EMPLOYEE GENDER") // RMC 20140105
            {
                if (strProvinceName != string.Empty)
                    this.axVSPrinter1.Table = string.Format("^16000;{0}", "PROVINCE OF " + strProvinceName);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("09"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("41"));
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = (float)12.0;
                this.axVSPrinter1.Table = string.Format("^13000;{0}", m_sReportName);
                this.axVSPrinter1.FontSize = (float)8.0;
                this.axVSPrinter1.Table = string.Format("^13000;As of: {0:MMM dd, yyyy}", AppSettingsManager.GetCurrentDate());

                axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                /*sData = "BUSINESS NAME|NATURE OF BUSINESS|ADDRESS|CONTACT INFORMATION|NUMBER OF WORKERS";
                this.axVSPrinter1.Table = string.Format("^4000|^2500|^4000|^2000|^2000;{0}", sData);
                sData = "||||MALE|FEMALE";
                this.axVSPrinter1.Table = string.Format("^4000|^2500|^4000|^2000|^1000|^1000;{0}", sData);*/
                // RMC 20140206 add own name in report list by employee gender (s)
                sData = "BUSINESS NAME|OWNER'S NAME|NATURE OF BUSINESS|ADDRESS|CONTACT INFORMATION|NUMBER OF WORKERS";
                this.axVSPrinter1.Table = string.Format("^3000|^2000|^2000|^3000|^2000|^2000;{0}", sData);
                sData = "|||||MALE|FEMALE";
                this.axVSPrinter1.Table = string.Format("^3000|^2000|^2000|^3000|^2000|^1000|^1000;{0}", sData);
                // RMC 20140206 add own name in report list by employee gender (e)
                axVSPrinter1.FontBold = false;
                //this.axVSPrinter1.Paragraph = "";
            }
            //else if (m_sReportName == "VERIFY BUSINESSES") // JHMN 20170130 added switch for verify businesses (s)
            else if (m_sReportName == "VERIFIED BUSINESSES")    // RMC 20170221 addl mods in report of Verified businesses
            {
                if (strProvinceName != string.Empty)
                    this.axVSPrinter1.Table = string.Format("^16000;{0}", "PROVINCE OF " + strProvinceName);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("09"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("41"));
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = (float)12.0;
                //this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("^13000;{0}", m_sReportName);
                this.axVSPrinter1.Table = string.Format("^13000;As of: {0:MMM dd, yyyy}", AppSettingsManager.GetCurrentDate());

                //this.axVSPrinter1.MarginLeft = 1000;
                this.axVSPrinter1.Table = string.Format("^13000;;TAX YEAR: {0}", m_sTaxYear);

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Table = string.Format("^3000|^4000|^3000|^3000|^3500;BIN|BUSINESS NAME|PREVIOUS TAX|CURRENT TAX|MEMORANDA");

                //this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }// JHMN 20170130 added switch for verify businesses (e)
            else
            {
                if (strProvinceName != string.Empty)
                    this.axVSPrinter1.Table = string.Format("^16000;{0}", strProvinceName);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("09"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("41"));
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = string.Format("^16000;{0}", m_sReportName);

                axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                sData = "B I N|TAX YEAR|BUSINESS NAME|BUSINESS ADDRESS|TAX PAYER'S NAME|TAX PAYER'S ADDRESS|LAST PAID AMOUNT|LAST PAID TAX YEAR - QTR|REMARKS";
                this.axVSPrinter1.Table = string.Format("^1800|^800|^2000|^2000|^2000|^2000|^1000|^1400|^3000;{0}", sData);
                axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            
        }

        private void ListHoldRecords()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec1 = new OracleResultSet();
            string sBin = "";
            string sTaxYear = ""; string sBnsName = ""; string sBnsAddr = "";
            string sOwnName = ""; string sOwnAddr = ""; string sUserCode = "";
            string sRemarks = ""; string sContent = "";
            DateTime dDateSave;

            pRec.Query = "select * from hold_records where status = 'HOLD' order by own_nm";
            if(pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBin = pRec.GetString("bin").Trim();
                    sTaxYear = pRec.GetString("tax_year").Trim();
                    sBnsName = pRec.GetString("bns_nm").Trim();
                    sBnsAddr = pRec.GetString("bns_addr").Trim();
                    sOwnName = pRec.GetString("own_nm").Trim();
                    sOwnAddr = pRec.GetString("own_addr").Trim();
                    sUserCode = pRec.GetString("user_code").Trim();
                    dDateSave = pRec.GetDateTime("dt_save");
                    sRemarks = pRec.GetString("remarks").Trim();

                    string sLastPayTaxYr = ""; string sLastPayAmt = ""; string sLastPayOrNo = "";

                    pRec1.Query = "select distinct * from pay_hist where bin = '"+ sBin + "' order by tax_year desc,qtr_paid desc";
                    if (pRec1.Execute())
                    {
                        if (pRec1.Read())
                        {
                            sLastPayTaxYr = pRec1.GetString("tax_year") + "-" + pRec1.GetString("qtr_paid");
                            sLastPayOrNo = pRec1.GetString("or_no");

                            pRec1.Close();

                            pRec1.Query = "select sum(fees_amtdue) as fees_amtdue from or_table where or_no = '" + sLastPayOrNo + "'";
                            if (pRec1.Execute())
                            {
                                if (pRec1.Read())
                                {
                                    sLastPayAmt = string.Format("{0:##.00}", pRec1.GetFloat("fees_amtdue"));
                                }
                            }
                            pRec1.Close();
                        }
                    }
                    pRec1.Close();

                    sContent = sBin + "|" + sTaxYear + "|" + sBnsName + "|" + sBnsAddr + "|" + sOwnName + "|" + sOwnAddr + "|" + sLastPayAmt + "|" + sLastPayTaxYr + "|" + sRemarks;	
                    this.axVSPrinter1.Table = string.Format("<1800|^800|<2000|<2000|<2000|<2000|>1000|^1400|<3000;{0}", sContent);
                    
                    
                }
            }
            pRec.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            
        }

        private void axVSPrinter1_BeforeFooter(object sender, EventArgs e)
        {
            this.axVSPrinter1.HdrFontName = "Arial Narrow";
            this.axVSPrinter1.HdrFontSize = (float)8.0;
            this.axVSPrinter1.HdrFontItalic = true;
        }

        private void PrintDataQuery()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sContent = "";
            string sBIN = "";
            string sBnsName = "";
            string sOwnName = "";
            string sBnsAdd = "";
            string sTaxYear = "";
            string sBnsDec = "";
            string sBnsStat = "";
            string sGrossCap = "";
            string sTotGrossCap = "";
            string sBillAmt = "";
            string sAmtPaid = "";
            string sDueQtr = "";
            string sTempBin = "";
            string sTotalRecCount = "";
            string sOwnGender = "";
            int iNoFemaleEmp = 0;
            int iNoMaleEmp = 0;
            string sTempEmp = "";
            int iTotalRecCount = 0;
            string sGrossTotal = "",sBillTotal="",sPaidTotal="";//GMC 20151109 Total Gross/Cap
            double dGrossTotal = 0.0,dBillTotal=0.0,dPaidTotal=0.0;//GMC 20151109 Total Gross/Cap
            //double dTotAmtPaid = 0; // RMC 20140123 added total amount paid in Data query report
            string sBnsTel = "";    // RMC 20171123 added Business contact no. in data query under the Business Name

            pRec.Query = m_sQuery;
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBIN = pRec.GetString("bin");
                    sBnsName = pRec.GetString("bns_nm");
                    sOwnName = pRec.GetString("own_nm");
                    sBnsAdd = pRec.GetString("bns_addr");
                    sTaxYear = pRec.GetString("tax_year");
                    sBnsDec = pRec.GetString("bns_type");
                    sBnsStat = pRec.GetString("bns_stat");
                    sGrossCap = string.Format("{0:#,###.00}", pRec.GetDouble("grcap"));
                    sTotGrossCap = string.Format("{0:#,###.00}", pRec.GetDouble("tot_grcap"));
                    sBillAmt = string.Format("{0:#,###.00}", pRec.GetDouble("bill_amt"));
                    sAmtPaid = string.Format("{0:#,###.00}", pRec.GetDouble("amt_paid"));
                    sDueQtr = pRec.GetString("due_qtr");
                    sOwnGender = pRec.GetString("own_gender");
                    iNoFemaleEmp = pRec.GetInt("no_female_emp");
                    iNoMaleEmp = pRec.GetInt("no_male_emp");

                    
                    if (iNoFemaleEmp == 0 && iNoMaleEmp != 0)
                        sTempEmp = iNoMaleEmp.ToString() + "M";
                    else if (iNoFemaleEmp != 0 && iNoMaleEmp == 0)
                        sTempEmp = iNoFemaleEmp.ToString() + "F";
                    else if (iNoFemaleEmp != 0 && iNoMaleEmp != 0)
                        sTempEmp = iNoFemaleEmp.ToString() + "F / " + iNoMaleEmp.ToString() + "M";
                    else
                        sTempEmp = "";
                    
                    if (sTempBin != sBIN)
                    {
                        iTotalRecCount++;
                        sTempBin = sBIN;
                        //dTotAmtPaid += pRec.GetDouble("amt_paid"); // RMC 20140123 added total amount paid in Data query report
                    }
                    else
                    {
                        sBIN = "";
                        sBnsName = "";
                        sOwnName = "";
                        sBnsAdd = "";
                        sTaxYear = "";
                        sTotGrossCap = "";
                        sBillAmt = "";
                        sAmtPaid = "";
                        sDueQtr = "";
                        sOwnGender = "";
                        sTempEmp = "";
                    }
                    dGrossTotal += pRec.GetDouble("tot_grcap");
                    dBillTotal += pRec.GetDouble("bill_amt");
                    dPaidTotal += pRec.GetDouble("amt_paid");
                    sContent = sBIN + "|" + sBnsName + "|" + sOwnName + "|" + sOwnGender + "|" + sBnsAdd + "|" + sTaxYear + "|" + sBnsDec + "|" + sBnsStat + "|" + sGrossCap + "|" + sTotGrossCap + "|" + sBillAmt + "|" + sAmtPaid + "|" + sDueQtr + "|" + sTempEmp;
                    //this.axVSPrinter1.Table = string.Format("<1550|<2430|<2010|<800|<2010|^800|<2010|^800|>1500|>1500|>1250|>1250|^900|^900;{0}", sContent);
                    this.axVSPrinter1.Table = string.Format("<1050|<2430|<1810|<800|<1810|^800|<2010|^800|>1100|>1100|>950|>950|^500|^700;{0}", sContent); // JHMN 20170307 edited columns
                }
            }
            pRec.Close();

            // RMC 20140123 added total amount paid in Data query report (s)
            /*this.axVSPrinter1.Paragraph = "";
            sAmtPaid = string.Format(string.Format("{0:#,###.00}",dTotAmtPaid));
            sContent = "|||||||||TOTAL|" + sAmtPaid + "|";
            this.axVSPrinter1.Table = string.Format("^1700|<2430|<2130|<2130|^800|<2010|^800|>1500|>1500|>1250|>1250|^1000;{0}", sContent);
            // RMC 20140123 added total amount paid in Data query report (e)*/
            sGrossTotal=string.Format("{0:#,###.00}", dGrossTotal);//GMC 20151109 Grand Total
            sBillTotal = string.Format("{0:#,###.00}", dBillTotal);
            sPaidTotal = string.Format("{0:#,###.00}", dPaidTotal);
            sTotalRecCount = string.Format("{0:###}", iTotalRecCount);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = string.Format("<2010|<2120|<2010|^800|<2010|^800|<2010|^800|>1500|>1500|>1250|>1250|^900|^900;Total No. of Businesses :|{0}|||||||Totals :|{1}|{2}|{3}", sTotalRecCount, sGrossTotal,sBillTotal,sPaidTotal);
            this.axVSPrinter1.Table = string.Format("<1850|<2430|<1010|<800|<1810|^800|<2010|^800|>1100|>1100|>950|>950|^500|^700;Total No. of Businesses :|{0}|||||||Totals :|{1}|{2}|{3}", sTotalRecCount, sGrossTotal, sBillTotal, sPaidTotal);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = "";
                
        }

        private void ListByGender() // CJC 20130429
        {
            StringBuilder strQuery = new StringBuilder();
            OracleResultSet pRec = new OracleResultSet();

            string sBin = string.Empty;
            string sBnsCode = string.Empty;
            string sBnsName = string.Empty;
            string sBnsAddr = string.Empty;
            string sOwnName = string.Empty;
            string sOwnAddr = string.Empty;
            string sOwnCode = string.Empty;
            string sContent = string.Empty;
            string sBnsStat = string.Empty;
            string sGender = string.Empty;
            string sBnsStatTemp = string.Empty;
            string sGenderTemp = string.Empty;
            string sBnsStatTmp2 = string.Empty;
            int intCountNewMale = 0;
            int intCountNewFemale = 0;
            int intCountNewUnSpecified = 0;
            int intCountRenMale = 0;
            int intCountRenFemale = 0;
            int intCountRenUnSpecified = 0;
            int intCountRetMale = 0;
            int intCountRetFemale = 0;
            int intCountRetUnSpecified = 0;

            //MCR ADD 20140512 MAT-14-5011 Percentale of Gender
            int intCountNewGenderTotal = 0;
            int intCountRenGenderTotal = 0;
            int intCountRetGenderTotal = 0;

            if (m_sData1 == "RENEWAL")
                m_sData1 = "REN";
            else if (m_sData1 == "RETIRED")
                m_sData1 = "RET";
            if (m_sData1 != "RET")
            {
                strQuery.Append("select a.bin, a.own_code, a.bns_stat, Coalesce(b.gender,'UNSPECIFIED') as Gender from businesses a, own_profile b where a.own_code = b.own_code ");
                if (m_sData1 != "ALL")
                    strQuery.Append(string.Format("and a.bns_stat = '{0}' ", m_sData1));
                if (m_sData2 != "ALL")
                    strQuery.Append(string.Format("and b.gender = '{0}' ", m_sData2));
                strQuery.Append(string.Format("and a.permit_dt between '{0:dd-MMM-yy}' and '{1:dd-MMM-yy}' order by a.bns_stat, b.gender desc", m_dtFrom, m_dtTo));
            }
            else
            {
                strQuery.Append("Select Distinct Rb.Bin,Rb.Bns_Stat,B.Own_Code, Coalesce(O.Gender,'UNSPECIFIED') as Gender From Retired_Bns Rb");
                strQuery.Append(" Left Join Businesses B On B.Bin = Rb.Bin Left Join Own_Profile O On O.Own_Code = B.Own_Code");
                strQuery.Append(string.Format(" where RB.apprvd_date >= '{0:dd-MMM-yy}' and RB.apprvd_date <= '{1:dd-MMM-yy}' order by Gender desc", m_dtFrom, m_dtTo));
            }
            pRec.Query = strQuery.ToString();
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBnsStat = pRec.GetString("bns_stat").Trim();
                    sBin = pRec.GetString("bin").Trim();
                    sBnsAddr = AppSettingsManager.GetBnsAddress(sBin);
                    sGender = pRec.GetString("gender").Trim();
                    sOwnCode = pRec.GetString("own_code").Trim();
                    sOwnName = AppSettingsManager.GetBnsOwner(sOwnCode);
                    sOwnAddr = AppSettingsManager.GetBnsOwnAdd(sOwnCode, true);
                    sBnsName = AppSettingsManager.GetBnsName(sBin);

                    if (sBnsStat == "NEW" && sGender == "MALE")
                        intCountNewMale++;
                    else if (sBnsStat == "NEW" && sGender == "FEMALE")
                        intCountNewFemale++;
                    else if (sBnsStat == "NEW" && sGender == "UNSPECIFIED")
                        intCountNewUnSpecified++;
                    else if (sBnsStat == "REN" && sGender == "MALE")
                        intCountRenMale++;
                    else if (sBnsStat == "REN" && sGender == "FEMALE")
                        intCountRenFemale++;
                    else if (sBnsStat == "REN" && sGender == "UNSPECIFIED")
                        intCountRenUnSpecified++;
                    else if (sBnsStat == "RET" && sGender == "MALE")
                        intCountRetMale++;
                    else if (sBnsStat == "RET" && sGender == "FEMALE")
                        intCountRetFemale++;
                    else if (sBnsStat == "RET" && sGender == "UNSPECIFIED")
                        intCountRetUnSpecified++;

                    if (sBnsStat != sBnsStatTemp)
                    {
                        if (sBnsStatTemp != string.Empty)
                            this.axVSPrinter1.Paragraph = "";

                        if (sBnsStat == "REN")
                            sBnsStatTmp2 = "RENEWAL";
                        else if (sBnsStat == "RET")
                            sBnsStatTmp2 = "RETIRED";
                        else
                            sBnsStatTmp2 = sBnsStat;

                        this.axVSPrinter1.FontBold = true;
                        this.axVSPrinter1.Table = string.Format("<16000;{0}", sBnsStatTmp2);
                        sBnsStatTemp = sBnsStat;
                        sGenderTemp = string.Empty;
                    }

                    if (sGender != sGenderTemp)
                    {
                        if (sGenderTemp != string.Empty)
                            this.axVSPrinter1.Paragraph = "";

                        this.axVSPrinter1.FontBold = true;
                        this.axVSPrinter1.Table = string.Format("<100|<15900;|{0}", sGender);
                        sGenderTemp = sGender;
                    }

                    sContent = sBin + "|" + sBnsName + "|" + sBnsAddr + "|" + sOwnName + "|" + sOwnAddr;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Table = string.Format("<300|<1800|<3500|<3500|<3500|<3500;|{0}", sContent);
                }
            }
            pRec.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.HdrFontItalic = true;
            //MCR MOD 20140512 MAT-14-5011 Percentale of Gender
            if (intCountNewMale != 0 || intCountNewFemale != 0)
            {
                intCountNewGenderTotal = intCountNewFemale + intCountNewMale + intCountNewUnSpecified;
                this.axVSPrinter1.Table = string.Format("<200|<15800;|{0}", "New");
                this.axVSPrinter1.Table = string.Format("<300|<15700;|Male: {0} - {1}%", intCountNewMale, Math.Round(((double)intCountNewMale / (double)intCountNewGenderTotal) * 100));
                this.axVSPrinter1.Table = string.Format("<300|<15700;|Female: {0} - {1}%", intCountNewFemale, Math.Round(((double)intCountNewFemale / (double)intCountNewGenderTotal) * 100));
                this.axVSPrinter1.Table = string.Format("<300|<15700;|Unspecified: {0} - {1}%", intCountNewUnSpecified, Math.Round(((double)intCountNewUnSpecified / (double)intCountNewGenderTotal) * 100));
            }
            if (intCountRenMale != 0 || intCountRenFemale != 0)
            {
                intCountRenGenderTotal = intCountRenMale + intCountRenFemale + intCountRenUnSpecified;
                this.axVSPrinter1.Table = string.Format("<200|<15800;|{0}", "Renewal");
                this.axVSPrinter1.Table = string.Format("<300|<15700;|Male: {0} - {1}%", intCountRenMale, Math.Round(((double)intCountRenMale / (double)intCountRenGenderTotal) * 100));
                this.axVSPrinter1.Table = string.Format("<300|<15700;|Female: {0} - {1}%", intCountRenFemale, Math.Round(((double)intCountRenFemale / (double)intCountRenGenderTotal) * 100));
                this.axVSPrinter1.Table = string.Format("<300|<15700;|Unspecified: {0} - {1}%", intCountRenUnSpecified, Math.Round(((double)intCountRenUnSpecified / (double)intCountRenGenderTotal) * 100));
            }
            if (intCountRetMale != 0 || intCountRetFemale != 0)
            {
                intCountRetGenderTotal = intCountRetMale + intCountRetFemale + intCountRetUnSpecified;
                this.axVSPrinter1.Table = string.Format("<200|<15800;|{0}", "Retired");
                this.axVSPrinter1.Table = string.Format("<300|<15700;|Male: {0} - {1}%", intCountRetMale, Math.Round(((double)intCountRetMale / (double)intCountRetGenderTotal) * 100));
                this.axVSPrinter1.Table = string.Format("<300|<15700;|Female: {0} - {1}%", intCountRetFemale, Math.Round(((double)intCountRetFemale / (double)intCountRetGenderTotal) * 100));
                this.axVSPrinter1.Table = string.Format("<300|<15700;|Unspecified: {0} - {1}%", intCountRetUnSpecified, Math.Round(((double)intCountRetUnSpecified / (double)intCountRetGenderTotal) * 100));
            }
        }

        private void ListByEmpGender()
        {
            // RMC 20140105 added report with employee gender (dilg report -requested by mati)
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();
            string sBIN = string.Empty;
            string sBnsNm = string.Empty;
            string sBnsType = string.Empty;
            string sBnsAdd = string.Empty;
            string sBnsTel = string.Empty;
            string sData = string.Empty;
            string sAddlCode = string.Empty;
            string sValue = string.Empty;
            string sRecCnt = string.Empty;
            string sOwnCode = string.Empty; // RMC 20140206 add own name in report list by employee gender
            int iRecCnt = 0;
            int iMaleCnt = 0;
            int iFemaleCnt = 0;

            if (m_sData1 == "RENEWAL")
                m_sData1 = "REN";
            else if (m_sData1 == "RETIRED")
                m_sData1 = "RET";

            pSet.Query = string.Format("select * from businesses where permit_dt between '{0:dd-MMM-yy}' and '{1:dd-MMM-yy}'", m_dtFrom, m_dtTo);
            if (m_sData1 != "ALL")
                pSet.Query += " and bns_stat = '" + m_sData1 + "'";
            pSet.Query += " order by bns_nm";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iRecCnt++;
                    sBIN = pSet.GetString("bin");
                    sBnsNm = pSet.GetString("bns_nm");
                    sBnsType = AppSettingsManager.GetBnsDesc(pSet.GetString("bns_code"));
                    sBnsAdd = AppSettingsManager.GetBnsAddress(sBIN);
                    sBnsTel = pSet.GetString("bns_telno");
                    sOwnCode = pSet.GetString("own_code");      // RMC 20140206 add own name in report list by employee gender
                    sRecCnt = string.Format("{0:###}", iRecCnt) + ".";

                    //sData = sRecCnt + "|" + sBnsNm + "|" + sBnsType + "|" + sBnsAdd + "|" + sBnsTel;
                    sData = sRecCnt + "|" + sBnsNm + "|" + AppSettingsManager.GetBnsOwner(sOwnCode) + "|" + sBnsType + "|" + sBnsAdd + "|" + sBnsTel;    // RMC 20140206 add own name in report list by employee gender
                    for (int iCnt = 1; iCnt <= 2; iCnt++)
                    {
                        string sTmpSwt = string.Empty;

                        if (iCnt == 1)
                        {
                            pRec.Query = "select * from addl_info_tbl where addl_desc like '%MALE WORKER%' and addl_desc not like '%FEMALE WORKER%'";
                            sTmpSwt = "MALE";
                        }
                        else
                        {
                            pRec.Query = "select * from addl_info_tbl where addl_desc like '%FEMALE WORKER%'";
                            sTmpSwt = "FEMALE";
                        }
                        if (pRec.Execute())
                        {
                            if (pRec.Read())
                                sAddlCode = pRec.GetString("addl_code");
                        }
                        pRec.Close();

                        int iTmpCnt = 0;
                        pRec.Query = "select * from addl_info where bin = '" + sBIN + "' and addl_code = '" + sAddlCode + "'";
                        if (pRec.Execute())
                        {
                            if (pRec.Read())
                            {
                                sValue = pRec.GetString("value");
                                sData += "|" + sValue;

                                int.TryParse(sValue, out iTmpCnt);
                                if(sTmpSwt == "MALE")
                                    iMaleCnt += iTmpCnt;
                                else
                                    iFemaleCnt += iTmpCnt;
                            }
                            else
                                sData += "|";
                        }
                        pRec.Close();
                    }

                    //this.axVSPrinter1.Table = string.Format("<500|<4000|<2500|<4000|<2000|^1000|^1000;{0}", sData);
                    this.axVSPrinter1.Table = string.Format("<500|<3000|<2000|<2000|<3000|<2000|^1000|^1000;{0}", sData); // RMC 20140206 add own name in report list by employee gender
                }
                sData = "";
                /*this.axVSPrinter1.Table = string.Format("<500|<4000|<2500|<4000|<2000|^1000|^1000;{0}", sData);
                this.axVSPrinter1.Table = string.Format("<500|<4000|<2500|<4000|<2000|^1000|^1000;{0}", sData);*/
                this.axVSPrinter1.Table = string.Format("<500|<3000|<2000|<2000|<3000|<2000|^1000|^1000;{0}", sData);   // RMC 20140206 add own name in report list by employee gender
                this.axVSPrinter1.Table = string.Format("<500|<3000|<2000|<2000|<3000|<2000|^1000|^1000;{0}", sData);   // RMC 20140206 add own name in report list by employee gender
                sRecCnt = string.Format("{0:###}", iMaleCnt);
                sData = "|||||" + sRecCnt + "|";
                sRecCnt = string.Format("{0:###}", iFemaleCnt);
                sData += sRecCnt;
                //this.axVSPrinter1.Table = string.Format("<500|<4000|<2500|<4000|<2000|^1000|^1000;{0}", sData);
                this.axVSPrinter1.Table = string.Format("<500|<3000|<2000|<2000|<3000|<2000|^1000|^1000;{0}", sData);   // RMC 20140206 add own name in report list by employee gender
            }
        }

        private void VerifyBusinesses() // JHMN 20170130 print verify businesses
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBin = "";
            string sMemo = "";

            if (m_sTaxYear.Contains(" - "))
            {
                pSet.Query = "SELECT * FROM treasurers_module WHERE action = '2' ORDER BY tax_year DESC";
            }
            else
            {
                pSet.Query = "SELECT * FROM treasurers_module WHERE action = '2' AND tax_year = '" + m_sTaxYear + "' ORDER BY tax_year DESC";
                pSet.Query += ", dt_approved";  // RMC 20170221 addl mods in report of Verified businesses
            }

            if (pSet.Execute())
                while (pSet.Read())
                {
                    sBin = pSet.GetString("bin");
                    sMemo = pSet.GetString("memoranda");

                    this.axVSPrinter1.Table = string.Format("<3000|<4000|>3000|>3000|<3500;{0}|{1}|{2}|{3}|{4}", sBin, AppSettingsManager.GetBnsName(sBin), string.Format("{0:#,###.00}", pSet.GetDouble("prev_tax")), string.Format("{0:#,###.00}", pSet.GetDouble("curr_tax")), sMemo);
                }
            pSet.Close();

            // RMC 20170221 addl mods in report of Verified businesses (s)
            if (AuditTrail.AuditTrail.InsertTrail("CRLVB", "trail table", "Print verified business of tax year " + m_sTaxYear + "") == 0)
            { }
            // RMC 20170221 addl mods in report of Verified businesses (e)
        }

    }
}