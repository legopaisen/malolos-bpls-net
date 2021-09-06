
// RMC 20150518 report corrections
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using System.Threading;

namespace Amellar.Modules.Utilities
{
    public partial class frmReportSched : Form
    {
        private string m_strSource = string.Empty;
        private string m_strReportName = string.Empty;

        public string Source
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public string ReportName
        {
            get { return m_strReportName; }
            set { m_strReportName = value; }
        }

        public frmReportSched()
        {
            InitializeComponent();
        }

        private void ExportFile() //MCR 20140618
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.DefaultExt = "html";
                dlg.Title = "Reports";
                dlg.Filter = "HTML Files (*.html;*.htm)|*.html|RTF Files (*.rtf)|*.rtf|Excel Files (*.xls)|*.xls";
                dlg.FilterIndex = 3;
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    axVSPrinter1.ExportFormat = VSPrinter7Lib.ExportFormatSettings.vpxPlainHTML;
                    axVSPrinter1.ExportFile = dlg.FileName;
                }
            }
        }
        
        string sSize = "";
        bool m_bInit = true;

       
        private void frmReport_Load(object sender, EventArgs e)
        {
            ExportFile();

            this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

            this.axVSPrinter1.MarginLeft = 750;
            this.axVSPrinter1.MarginTop = 750;
            this.axVSPrinter1.MarginBottom = 750;

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            if (m_strSource == "1")
                PrintLicense();
            if (m_strSource == "2")
                PrintFees();
            if (m_strSource == "3")
                PrintAddl();
            if (m_strSource == "4")
                PrintBrgyList();

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        
        public void CreateHeader()
        {
            string strProvinceName = string.Empty;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 10;

            this.axVSPrinter1.Paragraph = "Republic of the Philippines";
            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                this.axVSPrinter1.Paragraph = "PROVINCE OF " + strProvinceName;

            this.axVSPrinter1.Paragraph = AppSettingsManager.GetConfigValue("09");

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Paragraph = m_strReportName;
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 8; 

            string strData = string.Empty;
            if (m_strSource == "1")
            {
                strData = "Code|Business Type|Gross Range|Ex Rate|Plus Rate|On Excess|Minimum|Maximum|Amount";
                
                this.axVSPrinter1.Table = string.Format("^800|~1300|^3000|^900|^900|^1600|^900|^900|^1000;{0}", strData);
                               
                strData = " | |From|To| | |For Every|Add|Tax|Tax| ";    // RMC 20110919 modified printing of schedule
                this.axVSPrinter1.Table = string.Format("^800|~1300|^1500|^1500|^900|^900|^800|^800|^900|^900|^1000;{0}", strData);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            }
            else if (m_strSource == "2")
            {
                strData = "Code|Business Type|Type|Range From|Range To|Ex|Plus|On Excess|Min.|Max.|Amount";
                this.axVSPrinter1.Table = string.Format("^800|^2500|^500|^1500|^1500|^500|^500|^1100|^500|^500|^1000;{0}", strData);
                strData = " | | | | |Rate|Rate|For Every|Add|Fee|Fee| "; 
                this.axVSPrinter1.Table = string.Format("^800|^2500|^500|^1500|^1500|^500|^500|^600|^500|^500|^500|^1000;{0}", strData);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                
            }
            else if(m_strSource == "3")
            {
                strData = " |Code|Fees Description|Type|Amount";    
                this.axVSPrinter1.Table = string.Format("^2000|^800|^2500|^1000|^1000;{0}", strData);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (m_strSource == "4")
            {
                strData = "BARANGAY CODE|BARANGAY NAME|ZONE";
                this.axVSPrinter1.Table = string.Format("^2000|^5000|^3000;{0}", strData);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            if (!m_bInit)
                CreateHeader();

            m_bInit = false;

            axVSPrinter1.FontBold = false;
            String strUser = "";
            try { strUser = AppSettingsManager.SystemUser.UserCode; }
            catch { strUser = "SYS_PROG"; }

            axVSPrinter1.HdrFontName = "Arial";
            axVSPrinter1.HdrFontSize = 8.0f;
            axVSPrinter1.Footer = "PRINTED BY: " + strUser + "\nDate Printed: " + AppSettingsManager.GetCurrentDate().ToString("dd MMMM, yyyy hh:mm:ss");

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

        private void PrintLicense()
        {
            OracleResultSet result = new OracleResultSet();
            string strData = string.Empty;

            CreateHeader();

            result.Query = "select * from rep_sched_license order by bns_code asc, gross_from asc";
            if (result.Execute())
            {
                string strBnsCode = string.Empty;
                string strBnsDesc = string.Empty;
                string strTmpBnsCode = string.Empty;
                string strTmpBnsDesc = string.Empty;
                string strGr1 = string.Empty;
                string strGr2 = string.Empty;
                string strExRate = string.Empty;
                string strPlusRate = string.Empty;
                string strAmount = string.Empty;
                string strExcessNo = string.Empty;
                string strExcessAmt = string.Empty;
                string strMin = string.Empty;
                string strMax = string.Empty;
                string strMainBnsCode = string.Empty;

                if (result.Read())
                    strMainBnsCode = result.GetString("bns_code").Trim();
                result.Close();

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        string sTmp = string.Empty;

                        strBnsCode = result.GetString("bns_code").Trim();
                        sTmp = result.GetString("bns_type").Trim();
                        
                        strBnsDesc = StringUtilities.Left(sTmp, 25);

                        /*int intLength;
                        intLength = sTmp.Length;

                        if (intLength > 25)
                        {
                            while (intLength > 25)
                            {
                                intLength -= 25;
                                sTmp = StringUtilities.Right(sTmp, intLength);

                                if (intLength >= 25)
                                    strBnsDesc += "\n" + StringUtilities.Left(sTmp, 25);

                            }

                            if (sTmp.Length < 25)
                                strBnsDesc += "\n" + sTmp;
                        }*/
                        
                        strGr1 = string.Format("{0:#,##.00}", result.GetDouble("gross_from"));
                        strGr2 = string.Format("{0:#,##.00}", result.GetDouble("gross_to"));
                        strExRate = string.Format("{0:##.0000000}", result.GetDouble("ex_rate"));
                        strPlusRate = string.Format("{0:##.0000000}", result.GetDouble("plus_rate"));
                        strAmount = string.Format("{0:#,##.00}", result.GetDouble("amount"));
                        strExcessNo = string.Format("{0:#,##.00}", result.GetDouble("excess_no"));
                        strExcessAmt = string.Format("{0:#,##.00}", result.GetDouble("excess_amt"));
                        strMin = string.Format("{0:#,##.00}", result.GetDouble("minimum_amt"));
                        strMax = string.Format("{0:#,##.00}", result.GetDouble("maximum_amt"));

                        if (strGr1 == ".00" && strGr2 == ".00")
                        {
                            strGr1 = "";
                            strGr2 = "";
                        }
                        if (strExRate == ".0000000")
                            strExRate = "";
                        if (strPlusRate == ".0000000")
                            strPlusRate = "";
                        if (strAmount == ".00")
                            strAmount = "";
                        if (strExcessNo == ".00")
                            strExcessNo = "";
                        if (strExcessAmt == ".00")
                            strExcessAmt = "";
                        if (strMin == ".00")
                            strMin = "";
                        if (strMax == ".00")
                            strMax = "";

                        if (strGr1 == "" && strGr2 == "" && strExRate == "" && strPlusRate == "" &&
                            strAmount == "" && strExcessNo == "" && strExcessAmt == "" &&
                            strMin == "" && strMax == "")   // RMC 20110815
                            strBnsDesc = StringUtilities.HandleApostrophe(result.GetString("bns_type").Trim());

                        if (strTmpBnsCode != strBnsCode)
                        {
                            strTmpBnsCode = strBnsCode;
                            strTmpBnsDesc = strBnsDesc;

                            strData = strBnsCode + "|" + strTmpBnsDesc + "|";
                        }
                        else
                            strData = "||";

                        if ((strBnsCode != strMainBnsCode) && strBnsCode.Length == 2)
                        {
                            strMainBnsCode = strBnsCode;
                            this.axVSPrinter1.Paragraph = "";
                        }


                        strData += strGr1 + "|" + strGr2 + "|" + strExRate + "|" + strPlusRate + "|" + strExcessNo + "|" + strExcessAmt + "|" + strMin + "|" + strMax + "|" + strAmount;
                        axVSPrinter1.Table = string.Format("<800|<1300|>1500|>1500|>900|>900|>800|>800|>900|>900|>1000;{0}", strData);

                    }
                }
                result.Close();
            }

            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";

            string strRemarks = " ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            axVSPrinter1.Table = string.Format("<11000;{0}", strRemarks);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                        
            axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
            axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());

            if (AuditTrail.InsertTrail("AUTPS-L", "rep_schedule_license", "Schedule-License report") == 0)
            {
                result.Rollback();
                result.Close();
                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void PrintFees()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rFees = new OracleResultSet();
            OracleResultSet rGroup = new OracleResultSet();
            OracleResultSet rQtrConfig = new OracleResultSet();

            string strData = string.Empty;
            string strFeesCode = string.Empty;
            string strFeesDesc = string.Empty;
            string strGroupCode = string.Empty;
            string strGroupDesc = string.Empty;

            try
            {
                rFees.Query = "select distinct (fees_code),fees_desc from rep_sched_fees order by fees_code";
                if (rFees.Execute())
                {
                    int iCnt = 0;
                    while (rFees.Read())
                    {
                        iCnt++;
                        strFeesCode = rFees.GetString("fees_code");
                        strFeesDesc = rFees.GetString("fees_desc");

                        m_strReportName = "SCHEDULE OF RATES FOR " + strFeesDesc;

                        if (iCnt == 1)
                        {
                            CreateHeader();
                            //CreatePageHeader();
                        }

                        if (iCnt > 1)
                        {
                            this.axVSPrinter1.NewPage();
                            
                            this.axVSPrinter1.FontBold = true;
                            this.axVSPrinter1.FontSize = 10;
                            
                            long lngYY2;
                            if (AppSettingsManager.GetConfigValue("08") == "")
                                //lngYY2 = 1700;
                                lngYY2 = 1500;
                            else
                                //lngYY2 = 1900;
                                lngYY2 = 1700;

                            this.axVSPrinter1.CurrentY = lngYY2;

                            this.axVSPrinter1.Paragraph = m_strReportName;
                            this.axVSPrinter1.Paragraph = "";

                            this.axVSPrinter1.FontBold = false;
                            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
                            this.axVSPrinter1.Paragraph = "";
                            this.axVSPrinter1.Paragraph = "";
                            this.axVSPrinter1.FontSize = 8;
                        }

                        rGroup.Query = string.Format("select distinct (group_code), group_desc from rep_sched_fees where fees_code = '{0}' order by group_code", strFeesCode);
                        if (rGroup.Execute())
                        {
                            while (rGroup.Read())
                            {
                                strGroupCode = rGroup.GetString("group_code");
                                strGroupDesc = rGroup.GetString("group_desc");

                                this.axVSPrinter1.FontBold = true;
                                strData = strGroupCode + "|" + strGroupDesc;
                                this.axVSPrinter1.Table = string.Format("<800|~2500;{0}", strData);
                                this.axVSPrinter1.FontBold = false;

                                result.Query = string.Format("select * from rep_sched_fees where fees_code = '{0}' and group_code = '{1}' order by fees_code ASC, group_code ASC,bns_code ASC,range1 ASC", strFeesCode, strGroupCode);
                                if (result.Execute())
                                {
                                    string strBnsCode = string.Empty;
                                    string strBnsDesc = string.Empty;
                                    string strDataType = string.Empty;
                                    string strRange1 = string.Empty;
                                    string strRange2 = string.Empty;
                                    string strExRate = string.Empty;
                                    string strPlusRate = string.Empty;
                                    string strAmount = string.Empty;
                                    string strExcessNo = string.Empty;
                                    string strExcessAmt = string.Empty;
                                    string strMin = string.Empty;
                                    string strMax = string.Empty;
                                    string strTmpBnsCode = string.Empty;

                                    while (result.Read())
                                    {
                                        string sTmp = string.Empty;

                                        strBnsCode = result.GetString("bns_code");
                                        sTmp = result.GetString("bns_desc");

                                        strBnsDesc = StringUtilities.Left(sTmp, 30);

                                        int intLength;
                                        intLength = sTmp.Length;

                                        if (intLength > 30)
                                        {
                                            while (intLength > 30)
                                            {
                                                intLength -= 30;
                                                sTmp = StringUtilities.Right(sTmp, intLength);

                                                if (intLength >= 30)
                                                    strBnsDesc += "\n" + StringUtilities.Left(sTmp, 30);

                                            }

                                            if (sTmp.Length < 30)
                                                strBnsDesc += "\n" + sTmp;
                                        }

                                        strDataType = result.GetString("data_type");
                                        strRange1 = string.Format("{0:#,##.00}", result.GetDouble("range1"));
                                        strRange2 = string.Format("{0:#,##.00}", result.GetDouble("range2"));
                                        strExRate = string.Format("{0:#,##.0000000}", result.GetDouble("ex_rate"));
                                        strPlusRate = string.Format("{0:#,##.0000000}", result.GetDouble("plus_rate"));
                                        strAmount = string.Format("{0:#,##.00}", result.GetDouble("amount"));
                                        strExcessNo = string.Format("{0:#,##.00}", result.GetDouble("excess_no"));
                                        strExcessAmt = string.Format("{0:#,##.00}", result.GetDouble("excess_amt"));
                                        strMin = string.Format("{0:#,##.00}", result.GetDouble("minimum_amt"));
                                        strMax = string.Format("{0:#,##.00}", result.GetDouble("maximum_amt"));

                                        if (strRange1 == ".00" && strRange2 == ".00")
                                        {
                                            strRange1 = "";
                                            strRange2 = "";
                                        }
                                        if (strExRate == ".0000000")
                                            strExRate = "";
                                        if (strPlusRate == ".0000000")
                                            strPlusRate = "";
                                        if (strAmount == ".00")
                                            strAmount = "";
                                        if (strExcessNo == ".00")
                                            strExcessNo = "";
                                        if (strExcessAmt == ".00")
                                            strExcessAmt = "";
                                        if (strMin == ".00")
                                            strMin = "";
                                        if (strMax == ".00")
                                            strMax = "";

                                        if (strTmpBnsCode != strBnsCode)
                                        {
                                            strTmpBnsCode = strBnsCode;
                                            strData = strBnsCode + "|" + StringUtilities.RemoveApostrophe(strBnsDesc) + "|";
                                        }
                                        else
                                            strData = "||";

                                        strData += strDataType + "|" + strRange1 + "|" + strRange2 + "|" + strExRate + "|" + strPlusRate + "|" + strExcessNo + "|" + strExcessAmt + "|" + strMin + "|" + strMax + "|" + strAmount;
                                        this.axVSPrinter1.Table = string.Format("<800|<2500|^500|>1500|>1500|>500|>500|>500|>500|>500|>500|>1000;{0}", strData);

                                        string sQ2 = string.Empty;
                                        string sQ3 = string.Empty;
                                        string sQ4 = string.Empty;

                                        //check per qtr config
                                        rQtrConfig.Query = string.Format("select * from qtr_fee_config where det_buss_code = '{0}' and fees_code = '{1}' and rev_year = '{2}'", strBnsCode, strFeesCode, AppSettingsManager.GetConfigValue("07"));
                                        if (strDataType == "RR" || strDataType == "AR" || strDataType == "QR")
                                        {
                                            rQtrConfig.Query += string.Format(" and gr_1 = {0} ", result.GetDouble("range1"));
                                        }
                                        rQtrConfig.Query += " order by fees_code, det_buss_code, gr_1";
                                        if (rQtrConfig.Execute())
                                        {
                                            if (rQtrConfig.Read())
                                            {
                                                sQ2 = string.Format("{0:#,##.00}", rQtrConfig.GetDouble("amount2"));
                                                strData = "||||||||||Q2|" + sQ2;
                                                this.axVSPrinter1.Table = string.Format("<800|<2500|^500|>1500|>1500|>500|>500|>500|>500|>500|>500|>1000;{0}", strData);

                                                sQ3 = string.Format("{0:#,##.00}", rQtrConfig.GetDouble("amount3"));
                                                strData = "||||||||||Q3|" + sQ3;
                                                this.axVSPrinter1.Table = string.Format("<800|<2500|^500|>1500|>1500|>500|>500|>500|>500|>500|>500|>1000;{0}", strData);

                                                sQ4 = string.Format("{0:#,##.00}", rQtrConfig.GetDouble("amount4"));
                                                strData = "||||||||||Q4|" + sQ4;
                                                this.axVSPrinter1.Table = string.Format("<800|<2500|^500|>1500|>1500|>500|>500|>500|>500|>500|>500|>1000;{0}", strData);
                                            }
                                        }
                                        rQtrConfig.Close();

                                    }

                                    this.axVSPrinter1.Paragraph = "";
                                }
                                result.Close();
                            }

                            // RMC 20110919 modified printing of schedule (s)
                            this.axVSPrinter1.Paragraph = "";
                            this.axVSPrinter1.Paragraph = "";
                            this.axVSPrinter1.Paragraph = "";

                            string strRemarks = " ";
                            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                            this.axVSPrinter1.Table = string.Format("<11000;{0}", strRemarks);
                            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                            strRemarks = "Legend:   [F] fixed amount - [Q] quantity - [A] area - [RR] rate range - [QR] quantity range - [AR] area range";
                            this.axVSPrinter1.Table = string.Format("<10000;{0}", strRemarks);
                            this.axVSPrinter1.Paragraph = "";

                            this.axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
                            this.axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());
                            
                        }
                        rGroup.Close();

                        
                    }
                }
                rFees.Close();
            }
            catch
            {
                MessageBox.Show(rFees.Query.ToString());
                MessageBox.Show(rGroup.Query.ToString());
                MessageBox.Show(result.Query.ToString());
            }

            if (AuditTrail.InsertTrail("AUTPS-F", "rep_schedule_fees", "Schedule-Fees report") == 0)
            {
                result.Rollback();
                result.Close();
                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }

        private void PrintAddl()
        {
            OracleResultSet result = new OracleResultSet();
            string strData = string.Empty;

            CreateHeader();
            
            string strCode = "";
            string strDesc = "";
            string strType = "";
            string strAmt = "";

            result.Query = "select * from rep_sched_addl order by fees_code";
            if (result.Execute())
            {
                while (result.Read())
                {
                    strCode = result.GetString("fees_code");
                    strDesc = result.GetString("fees_desc");
                    strType = result.GetString("type");
                    strAmt = string.Format("{0:#,##.00}", result.GetDouble("amount"));

                    strData = "|" + strCode + "|" + strDesc + "|" + strType + "|" + strAmt;
                    axVSPrinter1.Table = string.Format("<2000|<800|<2500|<1000|>1000;{0}", strData);

                }
            }
            result.Close();

            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";

            string strRemarks = " ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strRemarks);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());

            if (AuditTrail.InsertTrail("AUTPS-F", "rep_schedule_addl", "Schedule-Addl report") == 0)
            {
                result.Rollback();
                result.Close();
                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void PrintBrgyList()
        {
            CreateHeader();

            OracleResultSet result = new OracleResultSet();
            OracleResultSet rBrgy = new OracleResultSet();

            string strDistCode = string.Empty;
            string strDistName = string.Empty;
            string strData = string.Empty;
            string strBrgyCode = string.Empty;
            string strBrgyName = string.Empty;
            string strZone = string.Empty;
            int iTotalRecCount = 0;

            result.Query = "select distinct dist_code,dist_nm from brgy order by dist_nm";
            if (result.Execute())
            {
                while (result.Read())
                {
                    try
                    {
                        strDistCode = result.GetString("dist_code");
                        strDistName = result.GetString("dist_nm");
                    }
                    catch
                    {
                        strDistCode = "";
                        strDistName = "";
                    }

                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = string.Format("<1100;{0}", strDistName);
                    this.axVSPrinter1.FontBold = false;

                    rBrgy.Query = "select distinct brgy_code,brgy_nm,zone from brgy ";
                    if (strDistCode != "")
                        rBrgy.Query += string.Format("where dist_code = '{0}' ", strDistCode);
                    //rBrgy.Query += "order by brgy_nm";
                    rBrgy.Query += "order by brgy_code";
                    if (rBrgy.Execute())
                    {
                        while (rBrgy.Read())
                        {
                            iTotalRecCount++;

                            strBrgyCode = rBrgy.GetString("brgy_code");
                            strBrgyName = rBrgy.GetString("brgy_nm");
                            strZone = rBrgy.GetString("zone");

                            strData = strBrgyCode + "|" + strBrgyName + "|" + strZone;
                            this.axVSPrinter1.Table = string.Format("^2000|<5000|<3000;{0}", strData);
                        }
                    }
                    rBrgy.Close();
                }
            }
            result.Close();

            strData = string.Format("{0:#,##}", iTotalRecCount);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            
            this.axVSPrinter1.Table = string.Format("<2500|<1000;Total No. of Barangay :|{0}", strData);
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());
            
        }
        

        
    }
}
