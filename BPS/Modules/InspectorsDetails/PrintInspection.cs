using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using Amellar.Common.PrintUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.InspectorsDetails
{
    public class PrintInspection
    {
        private VSPrinterEmuModel model;
        private VSPrinterEmuModel pageHeaderModel;
        OracleResultSet pSet = new OracleResultSet();
        private string m_strSource = string.Empty;
        private string m_strReportName = string.Empty;
        private string m_strQuery = string.Empty;
        private string m_strInspector = string.Empty;
        private string m_strPosition = string.Empty;
        private string m_strDateCover = string.Empty;
        private string m_strRemarks = string.Empty;
        private string m_strNoticeNum = string.Empty;
        private string m_strOwnNm = string.Empty;
        private string m_strBnsAdd = string.Empty;
        private string m_strAddlRemarks = string.Empty;
        public string m_sBIN = string.Empty;
        
        public string m_sISNum = string.Empty;
        private string m_strBnsName = string.Empty;
        private string m_strBnsOwner = string.Empty;
        private string m_strBnsType = string.Empty;

        public string BnsName
        {
            get { return m_strBnsName; }
            set { m_strBnsName = value; }
        }

        public string BnsType
        {
            get { return m_strBnsType; }
            set { m_strBnsType = value; }
        }

        public string BnsOwner
        {
            get { return m_strBnsOwner; }
            set { m_strBnsOwner = value; }
        }

        public string Source
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public string Query
        {
            get { return m_strQuery; }
            set { m_strQuery = value; }
        }

        public string ReportName
        {
            get { return m_strReportName; }
            set { m_strReportName = value; }
        }

        public string Inspector
        {
            get { return m_strInspector; }
            set { m_strInspector = value; }
        }

        public string Position
        {
            get { return m_strPosition; }
            set { m_strPosition = value; }
        }

        public string DateCover
        {
            get { return m_strDateCover; }
            set { m_strDateCover = value; }
        }

        public string Remarks
        {
            get { return m_strRemarks; }
            set { m_strRemarks = value; }
        }

        public string NoticeNum
        {
            get { return m_strNoticeNum; }
            set { m_strNoticeNum = value; }
        }

        public string Owner
        {
            get { return m_strOwnNm; }
            set { m_strOwnNm = value; }
        }

        public string BnsAdd
        {
            get { return m_strBnsAdd; }
            set { m_strBnsAdd = value; }
        }

        public string AddlRemarks
        {
            get { return m_strAddlRemarks; }
            set { m_strAddlRemarks = value; }
        }

        public PrintInspection()
        {
            model = new VSPrinterEmuModel();
            model.LeftMargin = 50;      
            model.TopMargin = 50;
            model.MaxY = 1100 - 200;

            pageHeaderModel = new VSPrinterEmuModel();
            pageHeaderModel.LeftMargin = 50; 
            pageHeaderModel.TopMargin = 500;
            pageHeaderModel.MaxY = 1100 - 200;
        }

        public void FormLoad()
        {
            if (m_strSource == "1")
                PrintDeficient();
            else if (m_strSource == "2")
                PrintInspectionReport();
            else if (m_strSource == "3")
                PrintNotice();
            else if (m_strSource == "4")
                PrintUnofficialBusiness();
            else if (m_strSource == "5")
                PrintUnofficialBusinessbyIS();
            else if (m_strSource == "6")
                PrintNoticeViolation();
            

            pageHeaderModel.PageCount = model.PageCount;
            PrintReportPreview();
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

            //model.SetParagraph(ConfigurationAttributes.OfficeName);
            model.SetParagraph(AppSettingsManager.GetConfigValue("41"));    // RMC 20110811
            model.SetParagraph(string.Empty);

            model.SetParagraph(m_strReportName);
            if (m_strSource == "1" || m_strSource == "4")
                model.SetParagraph(m_strDateCover);
            if(m_strSource == "2")
            {
                model.SetParagraph(string.Empty);
                model.SetParagraph("INSPECTION REPORT");
            }
            if (m_strSource == "5")
            {
                model.SetParagraph(string.Empty);
                model.SetParagraph("UNOFFICIAL BUSINESS");
            }
            

            model.SetParagraph(string.Empty);

            model.SetFontBold(0);
            model.SetTextAlign(0);
            model.SetTable(string.Empty);
            model.SetFontSize(8);

            //lngYY2 = model.GetCurrentY();

            string strData = string.Empty;

            if (m_strSource == "1")
            {
                model.SetFontBold(1);
                strData = "BIN/DATE INSPECTED|NAME OF PERMITTEE/ADDRESS||REMARKS/ADDITIONAL INFO||ORDINANCE VIOLATION";
                model.SetTable(string.Format("^2000|^3000|^200|^2800|^200|^2800;{0}", strData));
                model.SetTable(string.Empty);
                model.SetFontBold(0);
            }

            if (m_strSource == "4")
            {
                model.SetFontBold(1);
                strData = "IS No./DATE INSPECTED|NAME OF PERMITTEE/ADDRESS||REMARKS/ADDITIONAL INFO||ORDINANCE VIOLATION";
                model.SetTable(string.Format("^2000|^3000|^200|^2800|^200|^2800;{0}", strData));
                model.SetTable(string.Empty);
                model.SetFontBold(0);
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

            //pageHeaderModel.SetParagraph(ConfigurationAttributes.OfficeName);
            //model.SetParagraph(AppSettingsManager.GetConfigValue("41"));    // RMC 20110811
            pageHeaderModel.SetParagraph(AppSettingsManager.GetConfigValue("41"));  // RMC 20110902
            pageHeaderModel.SetParagraph(string.Empty);

            pageHeaderModel.SetParagraph(m_strReportName);
            if (m_strSource == "1" || m_strSource == "4")
                pageHeaderModel.SetParagraph(m_strDateCover);
            if(m_strSource == "2")
            {
                pageHeaderModel.SetParagraph(string.Empty);
                pageHeaderModel.SetParagraph("INSPECTION REPORT");
            }
            if (m_strSource == "5")
            {
                pageHeaderModel.SetParagraph(string.Empty);
                pageHeaderModel.SetParagraph("UNOFFICIAL BUSINESS");
            }
                        
            pageHeaderModel.SetParagraph(string.Empty);

            pageHeaderModel.SetFontBold(0);
            pageHeaderModel.SetTextAlign(0);
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetFontSize(8);


            string strData;

            if (m_strSource == "1")
            {
                pageHeaderModel.SetFontBold(1);
                strData = "BIN/DATE INSPECTED|NAME OF PERMITTEE/ADDRESS||REMARKS/ADDITIONAL INFO||ORDINANCE VIOLATION";
                pageHeaderModel.SetTable(string.Format("^2000|^3000|^200|^2800|^200|^2800;{0}", strData));
                pageHeaderModel.SetTable(string.Empty);
                pageHeaderModel.SetFontBold(0);
                
            }

            if (m_strSource == "4")
            {
                pageHeaderModel.SetFontBold(1);
                strData = "IS No./DATE INSPECTED|NAME OF PERMITTEE/ADDRESS||REMARKS/ADDITIONAL INFO||ORDINANCE VIOLATION";
                pageHeaderModel.SetTable(string.Format("^2000|^3000|^200|^2800|^200|^2800;{0}", strData));
                pageHeaderModel.SetTable(string.Empty);
                pageHeaderModel.SetFontBold(0);
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

        private void PrintDeficient()
        {
            string sInspectorCode = "";
            string sBin = "";
            string sDate = "";
            string sRemarks = "";
            string sAddlInfo = "";
            string sBnsName = "";
            string sBnsAddr = "";
            string sViolationCode = "";
            string sViolation = "";
            string sDataRow = "";
            string sRemarksAddl = "";
            
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec1 = new OracleResultSet();

            sDataRow = "";

            CreateHeader();
            CreatePageHeader();

            model.SetParagraph("");
            model.SetParagraph("");

            pSet.Query = m_strQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sInspectorCode = pSet.GetString("inspector_code").Trim();
                    sBin = pSet.GetString("bin").Trim();
                    sDate = pSet.GetString("date_inspected");
                    sRemarks = pSet.GetString("inspector_remarks").Trim();
                    sAddlInfo = pSet.GetString("addition_info").Trim();

                    sBnsName = AppSettingsManager.GetBnsName(sBin);
                    sBnsAddr = AppSettingsManager.GetBnsAddress(sBin);

                    sDataRow = sBin + "|";
                    sDataRow += sBnsName + "||||";
                    model.SetTable(string.Format("<2000|=3000|<200|<2800|<200|<2800;{0}", sDataRow));
                    /*sBinDate = sBin + "\n" + sDate;
                    sBnsNameAddr = sBnsName + "\n" + sBnsAddr;*/

                    if (sRemarks != "")
                        sRemarks = " * " + sRemarks;
                    if (sAddlInfo != "")
                        sAddlInfo = " * " + sAddlInfo;

                    sRemarksAddl = sRemarks + "\n" + sAddlInfo;
                    
                    pRec.Query = string.Format("select violation_code from violations where inspector_code = '{0}' and bin = '{1}' and date_inspected = '{2}' order by violation_code", sInspectorCode, sBin, sDate);
                    if (pRec.Execute())
                    {
                        sViolation = "";

                        int iCtr = 0;
                        while (pRec.Read())
                        {
                            sViolationCode = pRec.GetString("violation_code");

                            pRec1.Query = string.Format("select violation_desc from violation_table where violation_code = '{0}'", sViolationCode);
                            if (pRec1.Execute())
                            {
                                if (pRec1.Read())
                                {
                                    sViolation += pRec1.GetString("violation_desc") + "\n\n";
                                }
                            }
                            pRec1.Close();
                        }

                    }
                    pRec.Close();

                    sDataRow = sDate + "|";
                    sDataRow += StringUtilities.HandleApostrophe(sBnsAddr) + "||";
                    sDataRow += StringUtilities.HandleApostrophe(sRemarks) + "||";
                    sDataRow += StringUtilities.HandleApostrophe(sViolation) + ";";
                    sViolation = "";

                    model.SetTable(string.Format("<2000|=3000|<200|=2800|<200|=2800;{0}", sDataRow));
                    
                    sDataRow = "|||" + sAddlInfo + "||";
                    model.SetTable(string.Format("<2000|=3000|<200|=2800|<200|=2800;{0}", sDataRow));
                    model.SetParagraph("");
                }
            }
            pSet.Close();

            model.SetParagraph("");
            model.SetTable(string.Format("<10900;Inspector: {0}", m_strInspector));
            model.SetTable(string.Format("<10900;{0}", m_strPosition));
            
        }

        private void PrintInspectionReport()
        {
            string sInspectorCode = "";
            string sBin = "";
            string sDate = "";
            string sRemarks = "";
            string sAddlInfo = "";
            string sBnsName = "";
            string sBnsAddr = "";
            string sViolationCode = "";
            string sViolation = "";
            string sDataRow = "";
            string strCurrentDate = "";

            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec1 = new OracleResultSet();

            CreateHeader();
            CreatePageHeader();

            pSet.Query = m_strQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    model.SetFontSize(10);
                    
                    model.SetParagraph("");
                    model.SetParagraph("");
                    model.SetTable(string.Format(">10000;{0}", strCurrentDate));

                    sInspectorCode = pSet.GetString("inspector_code").Trim();
                    sBin = pSet.GetString("bin").Trim();
                    sDate = pSet.GetString("date_inspected");
                    sRemarks = pSet.GetString("inspector_remarks").Trim();
                    sAddlInfo = pSet.GetString("addition_info").Trim();

                    sBnsName = AppSettingsManager.GetBnsName(sBin);
                    sBnsAddr = AppSettingsManager.GetBnsAddress(sBin);


                    if (sRemarks != "")
                        sRemarks = "    * " + sRemarks;
                    if (sAddlInfo != "")
                        sAddlInfo = "   * " + sAddlInfo;

                    
                    pRec.Query = string.Format("select violation_code from violations where inspector_code = '{0}' and bin = '{1}' and date_inspected = '{2}' order by violation_code", sInspectorCode, sBin, sDate);
                    if (pRec.Execute())
                    {
                        sViolation = "  ";

                        int iCtr = 0;
                        while (pRec.Read())
                        {
                            sViolationCode = pRec.GetString("violation_code");

                            pRec1.Query = string.Format("select violation_desc from violation_table where violation_code = '{0}'", sViolationCode);
                            if (pRec1.Execute())
                            {
                                if (pRec1.Read())
                                {
                                    sViolation += pRec1.GetString("violation_desc") + "\n\n";
                                }
                            }
                            pRec1.Close();
                        }

                    }
                    pRec.Close();

                    sDataRow = "NAME OF PERMITTEE|: " + sBnsName;
                    model.SetTable(string.Format("<2000|<7000;{0}", sDataRow));

                    sDataRow = "BUSINESS ADDRESS|: " + sBnsAddr;
                    model.SetTable(string.Format("<2000|<7000;{0}", sDataRow));

                    sDataRow = "DATE INSPECTED|: " + sDate;
                    model.SetTable(string.Format("<2000|<7000;{0}", sDataRow));

                    model.SetParagraph("");
                    model.SetParagraph("");
                    model.SetTable(string.Format("<10000;FINDINGS:;"));

                    model.SetTable(string.Format("=8000;{0}", sRemarks));
                    model.SetTable(string.Format("=8000;{0}", sAddlInfo));

                    if (sViolation != "")
                    {
                        model.SetParagraph("");
                        model.SetTable(string.Format("<10000;VIOLATIONS:;"));
                        model.SetTable(string.Format("=8000;{0}", sViolation));

                    }

                    model.SetCurrentY(10000);
                    model.SetTable(string.Format("<5450|<5450;Noted by :|Verified by :;"));
                    model.SetTable(string.Format("<5450|<5450;|{0};", m_strInspector));
                    model.SetFontSize(8);

                    model.SetTable(string.Format("<5450|<5450;Account Chief Inspection Services|{0};", m_strPosition));

                    model.PageBreak();
                }
		
            }
            pSet.Close();
        }

        private string GetNoticeDate(string pBin, int pNoticeNumber)
        {
            string strCurrentDate = "";
            OracleResultSet pSet = new OracleResultSet();
            if (m_strSource == "3")
                pSet.Query = "select * from official_notice_closure where bin = '" + pBin + "' and notice_number = " + pNoticeNumber + "";
            else // (m_strSource == "6")
                pSet.Query = "select * from unofficial_notice_closure where is_number = '" + pBin + "' and notice_number = " + pNoticeNumber + "";
            if (pSet.Execute())
                if (pSet.Read())
                    strCurrentDate = pSet.GetDateTime("notice_date").ToString("MMMM dd, yyyy");
            pSet.Close();
            return strCurrentDate;
        }

        private void CreateFooter(int pNoticeNumber)
        {
            //string sContent = "";  
            //model.SetParagraph("");
            //if (m_strSource == "6")
            //{
            //    if (pNoticeNumber == 1 || pNoticeNumber == 2)
            //        sContent = "\t2/F City Hall, City of Mati, Davao Oriental";
            //    else //3
            //        sContent = "\tG/F City Hall, City of Mati, Davao Oriental";
            //}
            //else
            //    sContent = "\tG/F City Hall, City of Mati, Davao Oriental";

            //model.SetTable(string.Format("<10000;{0}", sContent));
            //sContent = "\tTelephone: (087)3884-696; Fax: (087)811-0459";
            //model.SetTable(string.Format("<10000;{0}", sContent));
            //sContent = "\tE-mail Add: bizbureau_mati@yahoo.com";
            //model.SetTable(string.Format("<10000;{0}", sContent));
        }

        private void PrintNotice()
        {
            string sContent = string.Empty;
            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            string sBnsPin = string.Empty;
            string sBldgCode = string.Empty;


            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());

            //CreateHeader(); // gde 20120504

            pRec.Query = "select * from btm_gis_loc where bin = '" + m_sBIN + "'";
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

            model.SetMarginLeft(1000);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("^10500;Republic of the Philippines"));
            model.SetTable(string.Format("^10500;Province of " + AppSettingsManager.GetConfigObject("08")));
            model.SetTable(string.Format("^10500;" + AppSettingsManager.GetConfigObject("09")));
            model.SetFontSize(12);
            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                model.SetTable("^10500;OFFICE OF THE CITY MAYOR");
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                model.SetTable("^10500;OFFICE OF THE MUNICIPAL MAYOR");
            //model.SetTable(string.Format("^10500;OFFICE OF THE CITY MAYOR"));
            model.SetFontSize(14);
            model.SetTable(string.Format("^10500;BUSINESS PERMIT & LICENSING OFFICE"));
            model.SetFontSize(10);
            model.SetTable(string.Format("^10500;_________________________________________________________________________________________________________________________"));

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetFontSize(14);
            model.SetFontBold(1);
            model.SetFontUnderline(1);
            if (m_strNoticeNum == "First")//|| m_strNoticeNum == "Second"
                model.SetTable(string.Format("^10500;NOTICE OF NON-RENEWAL OF BUSINESS PERMIT"));
            else if (m_strNoticeNum == "Second")//|| m_strNoticeNum == ""Third
                model.SetTable(string.Format("^10500;FINAL NOTICE OF NON-RENEWAL"));
            model.SetFontBold(0);
            model.SetFontUnderline(0);

            model.SetFontSize(10);
            model.SetParagraph("");
            model.SetParagraph("");

            if (m_strNoticeNum == "First")
                strCurrentDate = GetNoticeDate(m_sBIN, 1);
            else if (m_strNoticeNum == "Second")
                strCurrentDate = GetNoticeDate(m_sBIN, 2);
            else if (m_strNoticeNum == "Third")
                strCurrentDate = GetNoticeDate(m_sBIN, 3);

            model.SetTable(string.Format(">10000;{0}", strCurrentDate));
            model.SetParagraph("");

            model.SetMarginLeft(1000);

            string sGender = "";
            string sOwnCode = "";
            sOwnCode = AppSettingsManager.GetOwnCode(m_sBIN);
            sGender = AppSettingsManager.GetBnsOwnerGender(sOwnCode);

            if (sGender == "FEMALE")
                sGender = "MS./MRS. ";
            else if (sGender == "MALE")
                sGender = "MR. ";
            else
                sGender = "";

            model.SetFontBold(1);
            model.SetTable(string.Format("<10000;{0}", AppSettingsManager.GetBnsName(m_sBIN)));
            model.SetFontBold(0);
            model.SetTable(string.Format("<10000;{0}", sGender + m_strOwnNm));
            model.SetTable(string.Format("=10000;{0}", m_strBnsAdd));
            model.SetTable(string.Format("<10000;{0}", sBnsPin));
            model.SetParagraph("");
            model.SetParagraph("");
            if (sGender != "")
                model.SetTable(string.Format("<10000;Dear {0}:", sGender + AppSettingsManager.GetBnsOwnerLastName(sOwnCode)));
            else
                model.SetTable(string.Format("<10000;The Manager"));
            model.SetParagraph("");

            if (m_strNoticeNum == "First") //|| m_strNoticeNum == "Second" //JARS
            {
                model.SetFontSize(10);
                sContent = "\tPlease be reminded that as per our record you failed to renew your business permit for the year " + AppSettingsManager.GetSystemDate().Year + " pursuant to ";
                sContent += AppSettingsManager.GetConfigObject("42");
                model.SetTable(string.Format("=10000;{0}", sContent));

                model.SetFontSize(10);
                model.SetParagraph("");
                sContent = "\tIn view thereof, we are requesting you, or your authorized representative, to renew your permit within five (5)";
                sContent += " working days after receipt of this notice.";
                model.SetTable(string.Format("=10000;{0}", sContent));

                model.SetFontSize(10);
                model.SetParagraph("");
                sContent = "\tKindly give this matter your preferential action.";
                model.SetTable(string.Format("=10000;{0}", sContent));

                model.SetParagraph("");
                model.SetParagraph("");
                model.SetParagraph("");
                model.SetTable(string.Format("<7000|<3500;|Very truly yours,"));

                model.SetParagraph("");
                model.SetParagraph("");
                model.SetParagraph("");

                model.SetFontBold(1);
                model.SetTable(string.Format("<7000|<3500;|{0}", AppSettingsManager.GetConfigValue("03")));
                model.SetFontBold(0);
                model.SetTable(string.Format("<7000|^1500;|OIC, BPLO"));

                model.SetParagraph("");
                model.SetMarginLeft(300);
                model.SetFontItalic(1);
                sContent = "\tNote: Please disregard this notice if you have already renewed your business permit.";
                model.SetTable(string.Format("<10000;{0}", sContent));
                model.SetFontItalic(0);

                model.SetParagraph("");
                model.SetParagraph("");
                sContent = "\tReceived by     :_____________________________________________";
                model.SetTable(string.Format("<10000;{0}", sContent));
                sContent = "\tDate Received :_____________________________________________";
                model.SetTable(string.Format("<10000;{0}", sContent));

                CreateFooter(1);
            }
            else if (m_strNoticeNum == "Second")//|| m_strNoticeNum == "Third"
            {
                string first = GetNoticeDate(m_sBIN, 1);
                string second = GetNoticeDate(m_sBIN, 2);

                sContent = "\tThis is to remind you again of your failure to renew your business permit for the current year despite our previous notice";
                sContent += " dated " + first + ".";// + " and " + second
                model.SetTable(string.Format("=10000;{0}", sContent));

                model.SetParagraph("");
                sContent = "\tWe are giving you another five (5) working days after receipt of this notice to renew your permit.";
                sContent += " Should you fail to act on this, we have no choice but to recommend closure of your business pursuant to";
                sContent += AppSettingsManager.GetConfigObject("42");
                model.SetTable(string.Format("=10000;{0}", sContent));

                model.SetParagraph("");
                model.SetParagraph("");
                model.SetTable(string.Format("<7000|<3500;|Very truly yours,"));
                model.SetParagraph("");
                model.SetParagraph("");
                model.SetParagraph("");

                model.SetFontBold(1);
                model.SetTable(string.Format("<7000|<3500;|{0}", AppSettingsManager.GetConfigValue("03")));
                model.SetFontBold(0);
                model.SetTable(string.Format("<7000|^1500;|OIC, BPLO"));

                model.SetParagraph("");

                model.SetMarginLeft(300);
                model.SetFontItalic(1);
                sContent = "\tNote: Please disregard this notice if you have already renewed your business permit.";
                model.SetTable(string.Format("<10000;{0}", sContent));
                model.SetFontItalic(0);

                model.SetParagraph("");
                model.SetParagraph("");
                sContent = "\tReceived by     :_____________________________________________";
                model.SetTable(string.Format("<10000;{0}", sContent));
                sContent = "\tDate Received :_____________________________________________";
                model.SetTable(string.Format("<10000;{0}", sContent));

                CreateFooter(3);

            }

        }

        private void PrintNoticeViolation()
        {
            string sContent = string.Empty;
            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            string sBnsPin = string.Empty;
            string sBldgCode = string.Empty;

            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());

            //CreateHeader(); // gde 20120504

            // RMC 20151005 corrections in printing Notice of Unofficial business (s)
            if (m_sBIN == "")
                m_sBIN = m_sISNum;
            // RMC 20151005 corrections in printing Notice of Unofficial business (e)

            pRec.Query = "select * from btm_gis_loc where bin = '" + m_sBIN + "'";
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

            model.SetMarginLeft(1000);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("^10500;Republic of the Philippines"));
            model.SetTable(string.Format("^10500;Province of " + AppSettingsManager.GetConfigObject("08")));
            model.SetTable(string.Format("^10500;Municipality of " + AppSettingsManager.GetConfigObject("02")));
            model.SetFontSize(12);
            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                model.SetTable("^10500;OFFICE OF THE CITY MAYOR");
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                model.SetTable("^10500;OFFICE OF THE MUNICIPAL MAYOR");
            model.SetFontSize(14);
            model.SetTable(string.Format("^10500;BUSINESS PERMIT & LICENSING OFFICE"));
            model.SetFontSize(10);
            model.SetTable(string.Format("^10500;_________________________________________________________________________________________________________________________"));

            model.SetParagraph("");
            model.SetParagraph("");
            model.SetFontSize(14);
            model.SetFontBold(1);
            model.SetFontUnderline(1);
            if (m_strNoticeNum == "First")// || m_strNoticeNum == "Second"
                model.SetTable(string.Format("^10500;NOTICE OF VIOLATION"));
            else if (m_strNoticeNum == "Second")// || m_strNoticeNum == ""Third
                model.SetTable(string.Format("^10500;FINAL NOTICE OF VIOLATION"));
            model.SetFontBold(0);
            model.SetFontUnderline(0);

            model.SetFontSize(10);
            model.SetParagraph("");
            model.SetParagraph("");

            if (m_strNoticeNum == "First")
                strCurrentDate = GetNoticeDate(m_sISNum, 1);
            else if (m_strNoticeNum == "Second")
                strCurrentDate = GetNoticeDate(m_sISNum, 2);
            else if (m_strNoticeNum == "Third")
                strCurrentDate = GetNoticeDate(m_sISNum, 3);

            // RMC 20151005 corrections in printing Notice of Unofficial business (s)
            if(strCurrentDate == "")
                strCurrentDate = string.Format("{0:MMMM dd, yyyy}",AppSettingsManager.GetCurrentDate());
            // RMC 20151005 corrections in printing Notice of Unofficial business (e)

            model.SetTable(string.Format(">10000;{0}", strCurrentDate));
            model.SetParagraph("");

            model.SetParagraph("");
            model.SetMarginLeft(1000);

            model.SetTable(string.Format("<10000;{0}", m_strBnsOwner));
            model.SetTable(string.Format("=10000;{0}", AppSettingsManager.GetBnsAddress(m_sBIN)));
            model.SetTable(string.Format("<10000;{0}", sBnsPin));
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("<10000;Dear Sir/Madam: " + m_strBnsOwner));
            model.SetParagraph("");
            model.SetParagraph("");

            if (m_strNoticeNum == "First")// || m_strNoticeNum == "Second"
            {
                model.SetFontSize(10);
                // RMC 20151005 corrections in printing Notice of Unofficial business (s)
                if (AppSettingsManager.GetConfigValue("01") == "MUNICIPALITY")
                {
                    sContent = "\tOur record shows that you have violated " + AppSettingsManager.GetConfigObject("42") + ", of this Municipality by operating the following business ";
                    sContent += "undertaking/s without securing the necessary permit from the Office of the Municipal Mayor:";
                }
                else// RMC 20151005 corrections in printing Notice of Unofficial business (e)
                {
                    sContent = "\tOur record shows that you have violated " + AppSettingsManager.GetConfigObject("42") + ", of this city by operating the following business ";
                    sContent += "undertaking/s without securing the necessary permit from the Office of the City Mayor:";
                }
                model.SetTable(string.Format("=10000;{0}", sContent));

                model.SetParagraph("");
                model.SetTable(string.Format("<2000|=10500;|1. {0}", m_strBnsType));

                model.SetFontSize(10);
                model.SetParagraph("");
                sContent = "\tIn view thereof, we are requesting you, or your authorized representative, to secure permit for your business within five (5)";
                sContent += " working days after receipt of this notice.";
                model.SetTable(string.Format("=10000;{0}", sContent));

                model.SetFontSize(10);
                model.SetParagraph("");
                sContent = "\tPlease give this matter your most preferential attention.";
                model.SetTable(string.Format("=10000;{0}", sContent));

                model.SetParagraph("");
                model.SetParagraph("");
                model.SetParagraph("");
                model.SetTable(string.Format("<7000|<3500;|Very truly yours,"));

                model.SetParagraph("");
                model.SetParagraph("");
                model.SetParagraph("");

                model.SetFontBold(1);
                model.SetTable(string.Format("<7000|<3500;|{0}", AppSettingsManager.GetConfigValue("03")));
                model.SetFontBold(0);
                //model.SetTable(string.Format("<7000|^1500;|OIC, BPLO"));
                model.SetTable(string.Format("<7000|^1500;|{0}",AppSettingsManager.GetConfigValue("remarks","config","code","03","03")));  // RMC 20151005 corrections in printing Notice of Unofficial business

                model.SetFontSize(10);
                model.SetParagraph("");
                model.SetParagraph("");
                model.SetParagraph("");

                model.SetMarginLeft(300);

                sContent = "\tReceived by     :_____________________________________________";
                model.SetTable(string.Format("<10000;{0}", sContent));
                sContent = "\tDate Received :_____________________________________________";
                model.SetTable(string.Format("<10000;{0}", sContent));

                CreateFooter(1);
            }
            else if (m_strNoticeNum == "Second")// || m_strNoticeNum == "Third"
            {
                string first = GetNoticeDate(m_sISNum, 1);
                string second = GetNoticeDate(m_sISNum, 2);

                sContent = "\tThis is to remind you of your failure to secure business permit of business undertaking/s despite our previous notices";
                sContent += " dated " + first + " respectively.";//+ " and " + second 
                model.SetTable(string.Format("=10000;{0}", sContent));

                model.SetParagraph("");
                sContent = "\tIn view of the foregoing, we are giving you five (5) working days after receipt of this notice to secure your permit.";
                sContent += " Should you fail to act on this, we have no choice but to recommend closure of your business pursuant to ";
                sContent += AppSettingsManager.GetConfigObject("42");
                model.SetTable(string.Format("=10000;{0}", sContent));

                model.SetParagraph("");
                model.SetParagraph("");
                model.SetTable(string.Format("<7000|<3500;|Very truly yours,"));
                model.SetParagraph("");
                model.SetParagraph("");
                model.SetParagraph("");

                model.SetFontBold(1);
                model.SetTable(string.Format("<7000|<3500;|{0}", AppSettingsManager.GetConfigValue("03")));
                model.SetFontBold(0);
                model.SetTable(string.Format("<7000|^1500;|OIC, BPLO"));

                model.SetParagraph("");
                model.SetParagraph("");

                model.SetMarginLeft(300);
                model.SetFontItalic(1);
                sContent = "\tNote: Please disregard this notice if you already have a business permit";
                model.SetTable(string.Format("<10000;{0}", sContent));
                model.SetFontItalic(0);

                model.SetParagraph("");
                model.SetParagraph("");
                sContent = "\tReceived by     :_____________________________________________";
                model.SetTable(string.Format("<10000;{0}", sContent));
                sContent = "\tDate Received :_____________________________________________";
                model.SetTable(string.Format("<10000;{0}", sContent));

                CreateFooter(3);
            }

        }

        private void PrintUnofficialBusiness()
        {
            string sInspectorCode = "";
            string sISNum = "";
            string sDate = "";
            string sRemarks = "";
            string sAddlInfo = "";
            string sBnsName = "";
            string sBnsAddr = "";
            string sViolationCode = "";
            string sViolation = "";
            string sDataRow = "";
            string sRemarksAddl = "";
            
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec1 = new OracleResultSet();

            sDataRow = "";

            CreateHeader();
            CreatePageHeader();

            model.SetParagraph("");
            model.SetParagraph("");

            pSet.Query = m_strQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sInspectorCode = pSet.GetString("inspector_code").Trim();
                    sISNum = pSet.GetString("is_number").Trim();
                    sDate = pSet.GetString("date_inspected");
                    sRemarks = pSet.GetString("inspector_remarks").Trim();
                    sAddlInfo = pSet.GetString("addition_info").Trim();

                    pRec.Query = "select bns_nm, bns_house_no || ' ' || bns_street || ' ' ";
                    pRec.Query += "|| bns_brgy || ' ' || bns_dist || ' ' || bns_mun ";
                    pRec.Query += string.Format("from unofficial_info_tbl where trim(is_number) = '{0}'", sISNum);
                    if(pRec.Execute())
                    {
                        if(pRec.Read())
                        {
                            sBnsName = pRec.GetString(0);
                            sBnsAddr = pRec.GetString(1);
                        }
                    }
                    pRec.Close();

                    sDataRow = sISNum + "|";
                    sDataRow += sBnsName + "||||";
                    model.SetTable(string.Format("<2000|=3000|<200|<2800|<200|<2800;{0}", sDataRow));
                    /*sBinDate = sBin + "\n" + sDate;
                    sBnsNameAddr = sBnsName + "\n" + sBnsAddr;*/

                    if (sRemarks != "")
                        sRemarks = " * " + sRemarks;
                    if (sAddlInfo != "")
                        sAddlInfo = " * " + sAddlInfo;

                    sRemarksAddl = sRemarks + "\n" + sAddlInfo;
                    
                    pRec.Query = string.Format("select violation_code from violation_ufc where inspector_code = '{0}' and is_number = '{1}' and date_inspected = '{2}' order by violation_code", sInspectorCode, sISNum, sDate);
                    if (pRec.Execute())
                    {
                        sViolation = "";

                        int iCtr = 0;
                        while (pRec.Read())
                        {
                            sViolationCode = pRec.GetString("violation_code");

                            pRec1.Query = string.Format("select violation_desc from violation_table where violation_code = '{0}'", sViolationCode);
                            if (pRec1.Execute())
                            {
                                if (pRec1.Read())
                                {
                                    sViolation += pRec1.GetString("violation_desc") + "\n\n";
                                }
                            }
                            pRec1.Close();
                        }

                    }
                    pRec.Close();

                    sDataRow = sDate + "|";
                    sDataRow += StringUtilities.HandleApostrophe(sBnsAddr) + "||";
                    sDataRow += StringUtilities.HandleApostrophe(sRemarks) + "||";
                    sDataRow += StringUtilities.HandleApostrophe(sViolation) + ";";
                    sViolation = "";

                    model.SetTable(string.Format("<2000|=3000|<200|=2800|<200|=2800;{0}", sDataRow));
                    
                    sDataRow = "|||" + sAddlInfo + "||";
                    model.SetTable(string.Format("<2000|=3000|<200|=2800|<200|=2800;{0}", sDataRow));
                    model.SetParagraph("");
                }
            }
            pSet.Close();

            model.SetParagraph("");
            model.SetTable(string.Format("<10900;Inspector: {0}", m_strInspector));
            model.SetTable(string.Format("<10900;{0}", m_strPosition));

        }

        private void PrintUnofficialBusinessbyIS()
        {
            string sInspectorCode = "";
            string sISNum = "";
            string sDate = "";
            string sRemarks = "";
            string sAddlInfo = "";
            string sBnsName = "";
            string sBnsAddr = "";
            string sViolationCode = "";
            string sViolation = "";
            string sDataRow = "";
            string strCurrentDate = "";
            string sOwner = "";

            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec1 = new OracleResultSet();

            CreateHeader();
            CreatePageHeader();

            pSet.Query = m_strQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    model.SetFontSize(10);

                    model.SetParagraph("");
                    model.SetParagraph("");
                    model.SetTable(string.Format(">10000;{0}", strCurrentDate));

                    sInspectorCode = pSet.GetString("inspector_code").Trim();
                    sISNum = pSet.GetString("is_number").Trim();
                    sDate = pSet.GetString("date_inspected");
                    sRemarks = pSet.GetString("inspector_remarks").Trim();
                    sAddlInfo = pSet.GetString("addition_info").Trim();

                    pRec.Query = "select a.bns_nm, a.bns_house_no || ' ' || a.bns_street || ' ' ";
                    pRec.Query += "|| a.bns_brgy || ' ' || a.bns_dist || ' ' || a.bns_mun, b.own_ln || ' ' || b.own_fn || ' ' || b.own_mi ";
                    pRec.Query += "from unofficial_info_tbl a, own_names b where trim(a.own_code) = trim(b.own_code) ";
                    pRec.Query += string.Format(" and trim(a.is_number) = '{0}'", sISNum);
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sBnsName = pRec.GetString(0);
                            sBnsAddr = pRec.GetString(1);
                            sOwner = pRec.GetString(2);
                        }
                    }
                    pRec.Close();

                    if (sRemarks != "")
                        sRemarks = "    * " + sRemarks;
                    if (sAddlInfo != "")
                        sAddlInfo = "   * " + sAddlInfo;


                    pRec.Query = string.Format("select violation_code from violation_ufc where is_number = '{0}' order by violation_code", sISNum);
                    if (pRec.Execute())
                    {
                        sViolation = "  ";

                        int iCtr = 0;
                        while (pRec.Read())
                        {
                            sViolationCode = pRec.GetString("violation_code");

                            pRec1.Query = string.Format("select violation_desc from violation_table where violation_code = '{0}'", sViolationCode);
                            if (pRec1.Execute())
                            {
                                if (pRec1.Read())
                                {
                                    sViolation += pRec1.GetString("violation_desc") + "\n\n";
                                }
                            }
                            pRec1.Close();
                        }

                    }
                    pRec.Close();

                    sDataRow = "NAME OF PERMITTEE|: " + sBnsName;
                    model.SetTable(string.Format("<2000|<7000;{0}", sDataRow));

                    sDataRow = "BUSINESS ADDRESS|: " + sBnsAddr;
                    model.SetTable(string.Format("<2000|<7000;{0}", sDataRow));

                    sDataRow = "NAME OF OWNER|: " + sOwner;
                    model.SetTable(string.Format("<2000|<7000;{0}", sDataRow));

                    sDataRow = "DATE INSPECTED|: " + sDate;
                    model.SetTable(string.Format("<2000|<7000;{0}", sDataRow));

                    model.SetParagraph("");
                    model.SetParagraph("");
                    model.SetTable(string.Format("<10000;FINDINGS:;"));

                    model.SetTable(string.Format("=8000;{0}", sRemarks));
                    model.SetTable(string.Format("=8000;{0}", sAddlInfo));

                    if (sViolation != "")
                    {
                        model.SetParagraph("");
                        model.SetTable(string.Format("<10000;VIOLATIONS:;"));
                        model.SetTable(string.Format("=8000;{0}", sViolation));

                    }

                    model.SetCurrentY(10000);
                    model.SetTable(string.Format("<5450|<5450;Noted by :|Verified by :;"));
                    model.SetTable(string.Format("<5450|<5450;|{0};", m_strInspector));
                    model.SetFontSize(8);

                    model.SetTable(string.Format("<5450|<5450;Account Chief Inspection Services|{0};", m_strPosition));

                    model.PageBreak();
                }

            }
            pSet.Close();
        }

        
            
        
    }
}
