using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Printing;
using Amellar.Common.PrintUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;


namespace Amellar.Modules.Utilities
{
    public class PrintSchedule
    {
        private VSPrinterEmuModel model;
        private VSPrinterEmuModel pageHeaderModel;
        private string m_strSource = string.Empty;
        private string m_strReportName = string.Empty;
        private long lngY1 = 0;
        private long lngX1 = 0;
                        
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

        public PrintSchedule()
        {
            model = new VSPrinterEmuModel();
           /*model.LeftMargin = 50;
            model.TopMargin = 25;
            model.MaxY = 1100 - 25;*/

            model.LeftMargin = 50;
            model.TopMargin = 50;
            model.MaxY = 1100 - 200;

            pageHeaderModel = new VSPrinterEmuModel();
            /*pageHeaderModel.LeftMargin = 50;
            pageHeaderModel.TopMargin = 25;
            pageHeaderModel.MaxY = 1100 - 25;*/

            pageHeaderModel.LeftMargin = 50;
            pageHeaderModel.TopMargin = 500;
            pageHeaderModel.MaxY = 1100 - 200;

        }

        public void FormLoad()
        {
            if (m_strSource == "1")
                PrintLicense();
            if (m_strSource == "2")
                PrintFees();
            if (m_strSource == "3")
                PrintAddl();

            pageHeaderModel.PageCount = model.PageCount;
            PrintReportPreview();
        }

        
        
        private void PrintLicense()
        {
            OracleResultSet result = new OracleResultSet();
            string strData = string.Empty;

            CreateHeader();
            CreatePageHeader();

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

                        int intLength;
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
                        }
                        
                        

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
                            model.SetTable("");
                        }

                        
                        strData += strGr1 + "|" + strGr2 + "|" + strExRate + "|" + strPlusRate + "|" + strExcessNo + "|" + strExcessAmt + "|" + strMin + "|" + strMax + "|" + strAmount;
                        model.SetTable(string.Format("<800|<1500|>1500|>1500|>800|>800|>800|>800|>800|>800|>1000;{0}", strData));
                                               
                    }
                }
                result.Close();
            }

            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

            // RMC 20110919 modified printing of schedule (s)
            string strRemarks = " ";
            model.SetTableBorder(1);
            model.SetTable(string.Format("<11000;{0}", strRemarks));
            model.SetTableBorder(0);
            // RMC 20110919 modified printing of schedule (e)

            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));

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
                            CreatePageHeader();
                        }

                        if (iCnt > 1)
                        {
                            model.PageBreak();
                            //CreatePageHeader(); 

                            model.SetTextAlign(1);
                            model.SetFontBold(1);
                            model.SetFontSize(10);

                            long lngYY2;
                            if(AppSettingsManager.GetConfigValue("08") == "")
                                //lngYY2 = 1700;
                                lngYY2 = 1500;
                            else
                                //lngYY2 = 1900;
                                lngYY2 = 1700;
                            model.SetCurrentY(lngYY2);
                            model.SetParagraph(m_strReportName);
                            model.SetParagraph(string.Empty);

                            model.SetFontBold(0);
                            model.SetTextAlign(0);
                            model.SetTable(string.Empty);
                            model.SetTable(string.Empty);
                            model.SetFontSize(8);
                        }
                        
                        rGroup.Query = string.Format("select distinct (group_code), group_desc from rep_sched_fees where fees_code = '{0}' order by group_code", strFeesCode);
                        if (rGroup.Execute())
                        {
                            while (rGroup.Read())
                            {
                                strGroupCode = rGroup.GetString("group_code");
                                strGroupDesc = rGroup.GetString("group_desc");

                                model.SetFontBold(1);
                                strData = strGroupCode + "|" + strGroupDesc;
                                model.SetTable(string.Format("<800|~2500;{0}", strData));
                                model.SetFontBold(0);

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
                                        model.SetTable(string.Format("<800|<2500|^500|>1500|>1500|>500|>500|>500|>500|>500|>500|>1000;{0}", strData));

                                        string sQ2 = string.Empty;
                                        string sQ3 = string.Empty;
                                        string sQ4 = string.Empty;

                                        //check per qtr config
                                        rQtrConfig.Query = string.Format("select * from qtr_fee_config where det_buss_code = '{0}' and fees_code = '{1}' and rev_year = '{2}'",strBnsCode,strFeesCode, AppSettingsManager.GetConfigValue("07"));
                                        if (strDataType == "RR" || strDataType == "AR" || strDataType == "QR")
                                        {
                                            rQtrConfig.Query+= string.Format(" and gr_1 = {0} ", result.GetDouble("range1"));
                                        }
                                        rQtrConfig.Query += " order by fees_code, det_buss_code, gr_1";
                                        if (rQtrConfig.Execute())
                                        {
                                            if (rQtrConfig.Read())
                                            {
                                                sQ2 = string.Format("{0:#,##.00}", rQtrConfig.GetDouble("amount2"));
                                                strData = "||||||||||Q2|" + sQ2;
                                                model.SetTable(string.Format("<800|<2500|^500|>1500|>1500|>500|>500|>500|>500|>500|>500|>1000;{0}", strData));

                                                sQ3 = string.Format("{0:#,##.00}", rQtrConfig.GetDouble("amount3"));
                                                strData = "||||||||||Q3|" + sQ3;
                                                model.SetTable(string.Format("<800|<2500|^500|>1500|>1500|>500|>500|>500|>500|>500|>500|>1000;{0}", strData));

                                                sQ4 = string.Format("{0:#,##.00}", rQtrConfig.GetDouble("amount4"));
                                                strData = "||||||||||Q4|" + sQ4;
                                                model.SetTable(string.Format("<800|<2500|^500|>1500|>1500|>500|>500|>500|>500|>500|>500|>1000;{0}", strData));
                                            }
                                        }
                                        rQtrConfig.Close();

                                    }

                                    model.SetTable("");
                                }
                                result.Close();
                            }

                            // RMC 20110919 modified printing of schedule (s)
                            model.SetTable(string.Empty);
                            model.SetTable(string.Empty);
                            model.SetTable(string.Empty);

                            string strRemarks = " ";
                            model.SetTableBorder(1);
                            model.SetTable(string.Format("<11000;{0}", strRemarks));
                            model.SetTableBorder(0);    

                            strRemarks = "Legend:   [F] fixed amount - [Q] quantity - [A] area - [RR] rate range - [QR] quantity range - [AR] area range";
                            model.SetTable(string.Format("<10000;{0}", strRemarks));
                            model.SetTable(string.Empty);
                            
                            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
                            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));
                            // RMC 20110919 modified printing of schedule (e)
                        }
                        rGroup.Close();

                        /*model.SetTable(string.Empty);
                        model.SetTable(string.Empty);
                        model.SetTable(string.Empty);

                        string strRemarks;
                        strRemarks = "Legend:   [F] fixed amount - [Q] quantity - [A] area - [RR] rate range - [QR] quantity range - [AR] area range";

                        model.SetTable(string.Format("<10000;{0}", strRemarks));
                        model.SetTable(string.Empty);

                        model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
                        model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));
                        */
                        // RMC 20110919 modified printing of schedule, transferred
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
            CreatePageHeader();

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
                    model.SetTable(string.Format("<2000|<800|<2500|<1000|>1000;{0}", strData));
                    
                }
            }
            result.Close();

            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

            // RMC 20110919 modified printing of schedule (s)
            string strRemarks = " ";
            model.SetTableBorder(1);
            model.SetTable(string.Format("<11000;{0}", strRemarks));
            model.SetTableBorder(0);
            // RMC 20110919 modified printing of schedule (e)

            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));

            if (AuditTrail.InsertTrail("AUTPS-F", "rep_schedule_addl", "Schedule-Addl report") == 0)
            {
                result.Rollback();
                result.Close();
                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void PrintReportPreview()
        {
            model.MaxY = 1100;
            VSPrinterEmuDocument doc = new VSPrinterEmuDocument();
            doc.Model = model;

            doc.PageHeaderModel = pageHeaderModel;
            doc.Model.Reset();

            doc.DefaultPageSettings.Margins.Top = 50;
            doc.DefaultPageSettings.Margins.Left = 100;
            doc.DefaultPageSettings.Margins.Bottom = 50;
            doc.DefaultPageSettings.Margins.Right = 100;

            
            doc.DefaultPageSettings.PaperSize = new PaperSize("", 850, 1100);

            doc.PrintPage += new PrintPageEventHandler(_Doc_PrintPage);
        
            frmMyPrintPreviewDialog dlgPreview = new frmMyPrintPreviewDialog();
            dlgPreview.Document = doc;
            dlgPreview.ClientSize = new System.Drawing.Size(640, 480);
            dlgPreview.ShowIcon = true;
            //dlgPreview.IsAllowExport = true;
            dlgPreview.WindowState = FormWindowState.Maximized;
            dlgPreview.ShowDialog();
            model.Dispose();
        }

        private void _Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            //bool blnVisibleState = this.Visible;
            VSPrinterEmuDocument doc = (VSPrinterEmuDocument)sender;
            doc.Render(e);
        }

        

        private void CreateHeader()
        {
            
            string strProvinceName = string.Empty;
            model.SetTextAlign(1);
            model.SetFontBold(1);
            model.SetFontSize(10);
            model.SetParagraph("Republic of the Philippines");
            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                model.SetParagraph(strProvinceName);

            model.SetParagraph(AppSettingsManager.GetConfigValue("09"));

            //model.SetParagraph(ConfigurationAttributes.OfficeName);   // RMC 20110809 deleted
            model.SetParagraph(string.Empty);

            //long lngYY2;
            //lngYY2 = model.GetCurrentY();
            //model.SetCurrentY(lngYY2);
            model.SetParagraph(m_strReportName);
            model.SetParagraph(string.Empty);

            model.SetFontBold(0);
            model.SetTextAlign(0);
            model.SetTable(string.Empty);
            model.SetFontSize(8);

            //lngYY2 = model.GetCurrentY();

            string strData = string.Empty;
            if (m_strSource == "1")
            {
                strData = "Code|Business Type|Gross Range|Ex Rate|Plus Rate|On Excess|Minimum|Maximum|Amount";
                model.SetTable(string.Format("^800|~1500|^3000|^800|^800|^1600|^800|^800|^1000;{0}", strData));
                model.SetTableBorder(2);    // RMC 20110919 modified printing of schedule
                strData = " | |From|To| | |For Every|Add|Tax|Tax| ";    // RMC 20110919 modified printing of schedule
                model.SetTable(string.Format("^800|~1500|^1500|^1500|^800|^800|^800|^800|^800|^800|^1000;{0}", strData));
                model.SetTable(string.Empty);
                model.SetTableBorder(0);    // RMC 20110919 modified printing of schedule
                
            }
            else if (m_strSource == "2")
            {
                strData = "Code|Business Type|Type|Range From|Range To|Ex Rate|Plus|On Excess|Min.|Max.|Amount";
                model.SetTable(string.Format("^800|^2500|^500|^1500|^1500|^500|^500|^1100|^500|^500|^1000;{0}", strData));
                model.SetTableBorder(2);    // RMC 20110919 modified printing of schedule
                strData = " | | | | | |Rate|For Every|Add|Fee|Fee| "; // RMC 20110919 modified printing of schedule
                model.SetTable(string.Format("^800|^2500|^500|^1500|^1500|^500|^500|^600|^500|^500|^500|^1000;{0}", strData));
                model.SetTable(string.Empty);
                model.SetTableBorder(0);    // RMC 20110919 modified printing of schedule
            }
            else
            {
                model.SetTableBorder(2);    // RMC 20110919 modified printing of schedule
                strData = " |Code|Fees Description|Type|Amount";    // RMC 20110919 modified printing of schedule
                model.SetTable(string.Format("^2000|^800|^2500|^1000|^1000;{0}", strData));
                model.SetTable(string.Empty);
                model.SetTableBorder(0);    // RMC 20110919 modified printing of schedule

                
            }
            
        }

        private void CreatePageHeader()
        {
            pageHeaderModel.Clear();
            string strProvinceName = string.Empty;
            pageHeaderModel.SetTextAlign(1);
            pageHeaderModel.SetFontBold(1);
            pageHeaderModel.SetFontSize(10);
            pageHeaderModel.SetParagraph("Republic of the Philippines");
            
            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                pageHeaderModel.SetParagraph(strProvinceName);
            pageHeaderModel.SetParagraph(AppSettingsManager.GetConfigValue("09"));

            //pageHeaderModel.SetParagraph(ConfigurationAttributes.OfficeName); // RMC 20110809 deleted
            pageHeaderModel.SetParagraph(string.Empty);

           // pageHeaderModel.SetParagraph(m_strReportName);  // RMC 20110919 enabled
            //pageHeaderModel.SetParagraph(string.Empty);     // RMC 20110919 enabled

            pageHeaderModel.SetFontBold(0);
            pageHeaderModel.SetTextAlign(0);
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetFontSize(8);

            
            string strData;
            if (m_strSource == "1")
            {
                strData = "Code|Business Type|Gross Range|Ex Rate|Plus Rate|On Excess|Minimum|Maximum|Amount";
                pageHeaderModel.SetTable(string.Format("^800|~1500|^3000|^800|^800|^1600|^800|^800|^1000;{0}", strData));
                pageHeaderModel.SetTableBorder(2);  // RMC 20110919 modified printing of schedule
                strData = " | |From|To| | |For Every|Add|Tax|Tax| ";    // RMC 20110919 modified printing of schedule
                pageHeaderModel.SetTable(string.Format("^800|~1500|^1500|^1500|^800|^800|^800|^800|^800|^800|^1000;{0}", strData));
                pageHeaderModel.SetTable(string.Empty);
                pageHeaderModel.SetTableBorder(0);  // RMC 20110919 modified printing of schedule
            }
            else if (m_strSource == "2")
            {
                strData = "Code|Business Type|Type|Range From|Range To|Ex Rate|Plus|On Excess|Min.|Max.|Amount";
                pageHeaderModel.SetTable(string.Format("^800|^2500|^500|^1500|^1500|^500|^500|^1100|^500|^500|^1000;{0}", strData));
                pageHeaderModel.SetTableBorder(2);  // RMC 20110919 modified printing of schedule
                strData = " | | | | | |Rate|For Every|Add|Fee|Fee| ";   // RMC 20110919 modified printing of schedule
                pageHeaderModel.SetTable(string.Format("^800|^2500|^500|^1500|^1500|^500|^500|^600|^500|^500|^500|^1000;{0}", strData));
                pageHeaderModel.SetTable(string.Empty);
                pageHeaderModel.SetTableBorder(0);  // RMC 20110919 modified printing of schedule
            }
            else
            {
                pageHeaderModel.SetTableBorder(2);  // RMC 20110919 modified printing of schedule
                strData = " |Code|Fees Description|Type|Amount";        // RMC 20110919 modified printing of schedule
                pageHeaderModel.SetTable(string.Format("^2000|^800|^2500|^1000|^1000;{0}", strData));
                pageHeaderModel.SetTable(string.Empty);
                pageHeaderModel.SetTableBorder(0);  // RMC 20110919 modified printing of schedule
            }

        }

        
       
    }

        
}
