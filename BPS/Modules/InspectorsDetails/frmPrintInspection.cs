// RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmPrintInspection : Form
    {
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
        private bool m_bInit = true;

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

        public frmPrintInspection()
        {
            InitializeComponent();
        }

        private void frmPrintInspection_Load(object sender, EventArgs e)
        {
            this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
           
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

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;

        }

        private void CreateHeader()
        {
            
            string strProvinceName = string.Empty;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterTop;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Paragraph = "Republic of the Philippines";

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty )
            {
                if(strProvinceName.Contains("PROVINCE OF"))
                    this.axVSPrinter1.Paragraph = strProvinceName;
                else
                    this.axVSPrinter1.Paragraph = "PROVINCE OF " + strProvinceName;
            }
            
            this.axVSPrinter1.Paragraph = AppSettingsManager.GetConfigValue("09");

            this.axVSPrinter1.Paragraph = "OFFICE OF THE " + AppSettingsManager.GetConfigValue("01") + " ADMINISTRATOR";
            this.axVSPrinter1.Paragraph = "BUSINESS PERMIT AND LICENSING DIVISION";

            // RMC 20171212 adjustments in Inspector's Deficient report by BIN (s)
            if (AppSettingsManager.GetConfigValue("10") == "216")
                //this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "1.0in", ".62in", "25%", "25%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "1.0in", "1.0in", "25%", "25%", 10, false);
            // RMC 20171212 adjustments in Inspector's Deficient report by BIN (e)

            this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.Paragraph = m_strReportName;

            if (m_strSource == "1" || m_strSource == "4")
                this.axVSPrinter1.Paragraph = m_strDateCover;
            if (m_strSource == "2")
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "INSPECTION REPORT";
            }
            if (m_strSource == "3")
            {
                
                if (m_strNoticeNum == "First")
                    this.axVSPrinter1.Table = string.Format("^10500;NOTICE OF NON-RENEWAL OF BUSINESS PERMIT");
                else if (m_strNoticeNum == "Second")
                    this.axVSPrinter1.Table = string.Format("^10500;FINAL NOTICE OF NON-RENEWAL");
                this.axVSPrinter1.FontBold = false;
                
            }
            if (m_strSource == "5")
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "UNOFFICIAL BUSINESS";
            }

            if (m_strSource == "6")
            {
                this.axVSPrinter1.FontUnderline = true;
                if (m_strNoticeNum == "First")
                    this.axVSPrinter1.Table = string.Format("^10500;NOTICE OF VIOLATION");
                else if (m_strNoticeNum == "Second")
                    this.axVSPrinter1.Table = string.Format("^10500;FINAL NOTICE OF VIOLATION");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontUnderline = false;
            }

            if (m_strSource == "2")
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = 14;
                this.axVSPrinter1.Paragraph = "NOTICE";
            }

            this.axVSPrinter1.FontSize = 10;

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 8;

            string strData = string.Empty;

            if (m_strSource == "1")
            {
                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = true;
                strData = "BIN/DATE INSPECTED|NAME OF PERMITTEE/ADDRESS||REMARKS/ADDITIONAL INFO||ORDINANCE VIOLATION";
                this.axVSPrinter1.Table  = string.Format("^2000|^3000|^200|^2800|^200|^2800;{0}", strData);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = false;
            }

            if (m_strSource == "4")
            {
                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = true;
                strData = "IS No./DATE INSPECTED|NAME OF PERMITTEE/ADDRESS||REMARKS/ADDITIONAL INFO||ORDINANCE VIOLATION";
                this.axVSPrinter1.Table  = string.Format("^2000|^3000|^200|^2800|^200|^2800;{0}", strData);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = false;
            }

            m_bInit = false;
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
            OracleResultSet pSet = new OracleResultSet();

            sDataRow = "";



            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

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
                    this.axVSPrinter1.Table = string.Format("<2000|=3000|<200|<2800|<200|<2800;{0}", sDataRow);
                    

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

                            sViolation += Violations.GetViolationDesc(sViolationCode) + "\n\n";

                            
                        }

                    }
                    pRec.Close();

                    sDataRow = sDate + "|";
                    sDataRow += StringUtilities.HandleApostrophe(sBnsAddr) + "||";
                    sDataRow += StringUtilities.HandleApostrophe(sRemarks) + "||";
                    sDataRow += StringUtilities.HandleApostrophe(sViolation) + ";";
                    sViolation = "";

                    this.axVSPrinter1.Table = string.Format("<2000|=3000|<200|=2800|<200|=2800;{0}", sDataRow);

                    sDataRow = "|||" + sAddlInfo + "||";
                    this.axVSPrinter1.Table = string.Format("<2000|=3000|<200|=2800|<200|=2800;{0}", sDataRow);
                    this.axVSPrinter1.Table = "";
                }
            }
            pSet.Close();

            this.axVSPrinter1.Table = "";
            this.axVSPrinter1.Table = "";string.Format("<10900;Inspector: {0}", m_strInspector);
            this.axVSPrinter1.Table = "";string.Format("<10900;{0}", m_strPosition);

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
            OracleResultSet pSet = new OracleResultSet();

         
            pSet.Query = m_strQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    this.axVSPrinter1.FontSize = 10;

                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Table = string.Format(">10000;{0}", strCurrentDate);

                    this.axVSPrinter1.Paragraph = "";   // RMC 20171212 adjustments in Inspector's Deficient report by BIN
                    this.axVSPrinter1.Paragraph = "";   // RMC 20171212 adjustments in Inspector's Deficient report by BIN

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

                            sViolation += Violations.GetViolationDesc(sViolationCode) + "\n\n";
                            
                        }

                    }
                    pRec.Close();

                    //sDataRow = "NAME OF PERMITTEE|: " + sBnsName;
                    sDataRow = "BUSINESS NAME|: " + sBnsName;   // RMC 20171212 adjustments in Inspector's Deficient report by BIN
                    this.axVSPrinter1.Table = string.Format("<2000|<7000;{0}", sDataRow);

                    sDataRow = "BUSINESS ADDRESS|: " + sBnsAddr;
                    this.axVSPrinter1.Table = string.Format("<2000|<7000;{0}", sDataRow);

                    sDataRow = "DATE INSPECTED|: " + sDate;
                    this.axVSPrinter1.Table = string.Format("<2000|<7000;{0}", sDataRow);

                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Table = string.Format("<10000;FINDINGS:;");
                    this.axVSPrinter1.Paragraph = "";   // RMC 20171212 adjustments in Inspector's Deficient report by BIN
                    this.axVSPrinter1.Table = string.Format("<10000;{0}", sRemarks);    // RMC 20171212 adjustments in Inspector's Deficient report by BIN, changed format size
                    this.axVSPrinter1.Table = string.Format("<10000;{0}", sAddlInfo);   // RMC 20171212 adjustments in Inspector's Deficient report by BIN, changed format size
                    this.axVSPrinter1.Paragraph = "";   // RMC 20171212 adjustments in Inspector's Deficient report by BIN

                    if (sViolation != "")
                    {
                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.Table = string.Format("<10000;VIOLATIONS:;");
                        this.axVSPrinter1.Table = string.Format("<10000;{0}", sViolation);  // RMC 20171212 adjustments in Inspector's Deficient report by BIN, changed format size

                    }

                    this.axVSPrinter1.CurrentY = 10000;
                    
                    //this.axVSPrinter1.Table = string.Format("<5450|<5450;Noted by :|Verified by :;");
                    this.axVSPrinter1.Table = string.Format("<5450|<5450;Conforme :|Verified by :;");   // RMC 20171212 adjustments in Inspector's Deficient report by BIN
                    this.axVSPrinter1.Paragraph = "";    // RMC 20171212 adjustments in Inspector's Deficient report by BIN
                    this.axVSPrinter1.Paragraph = "";   // RMC 20171212 adjustments in Inspector's Deficient report by BIN
                    this.axVSPrinter1.Paragraph = "";    // RMC 20171212 adjustments in Inspector's Deficient report by BIN
                    this.axVSPrinter1.Paragraph = "";   // RMC 20171212 adjustments in Inspector's Deficient report by BIN
                    //this.axVSPrinter1.Table = string.Format("<5450|<5450;|{0};", m_strInspector);
                    //this.axVSPrinter1.Table = string.Format("<5450|<5450;|{0};", m_strPosition);    // RMC 20171212 adjustments in Inspector's Deficient report by BIN
                    this.axVSPrinter1.Table = string.Format("<5450|<5450;___________________________|{0};", m_strInspector);
                    this.axVSPrinter1.Table = string.Format("<5450|<5450;Signature over Printed Name|{0};", m_strPosition); // RMC 20171222 adjustments in Inspector's Deficient report by BIN
                    this.axVSPrinter1.Table = string.Format("<5450|<5450;Date:|"); // RMC 20171222 adjustments in Inspector's Deficient report by BIN
                    this.axVSPrinter1.FontSize = 8;

                    //this.axVSPrinter1.Table = string.Format("<5450|<5450;Account Chief Inspection Services|{0};", m_strPosition); // RMC 20171212 adjustments in Inspector's Deficient report by BIN, put rem

                    //this.axVSPrinter1.NewPage();
                    
                }

            }
            pSet.Close();
        }

        private void PrintNotice()
        {
            string sContent = string.Empty;
            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            string sBnsPin = string.Empty;
            string sBldgCode = string.Empty;
            OracleResultSet pSet = new OracleResultSet();

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

            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            if (m_strNoticeNum == "First")
                strCurrentDate = GetNoticeDate(m_sBIN, 1);
            else if (m_strNoticeNum == "Second")
                strCurrentDate = GetNoticeDate(m_sBIN, 2);
            else if (m_strNoticeNum == "Third")
                strCurrentDate = GetNoticeDate(m_sBIN, 3);

            this.axVSPrinter1.Table = string.Format(">10000;{0}", strCurrentDate);
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.MarginLeft = 1000;

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

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("<10000;{0}", AppSettingsManager.GetBnsName(m_sBIN));
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = string.Format("<10000;{0}", sGender + m_strOwnNm);
            this.axVSPrinter1.Table = string.Format("=10000;{0}", m_strBnsAdd);
            this.axVSPrinter1.Table = string.Format("<10000;{0}", sBnsPin);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            if (sGender != "")
                this.axVSPrinter1.Table = string.Format("<10000;Dear {0}:", sGender + AppSettingsManager.GetBnsOwnerLastName(sOwnCode));
            else
                this.axVSPrinter1.Table = string.Format("<10000;The Manager");
            this.axVSPrinter1.Paragraph = "";

            if (m_strNoticeNum == "First") //|| m_strNoticeNum == "Second" //JARS
            {
                this.axVSPrinter1.FontSize = 10;
                sContent = "\tPlease be reminded that as per our record you failed to renew your business permit for the year " + AppSettingsManager.GetSystemDate().Year + " pursuant to ";
                sContent += AppSettingsManager.GetConfigObject("42");
                this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

                
                this.axVSPrinter1.Paragraph = "";
                sContent = "\tIn view thereof, we are requesting you, or your authorized representative, to renew your permit within five (5)";
                sContent += " working days after receipt of this notice.";
                this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

                
                this.axVSPrinter1.Paragraph = "";
                sContent = "\tKindly give this matter your preferential action.";
                this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = string.Format("<7000|<3500;|Very truly yours,");

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("<7000|<3500;|{0}", AppSettingsManager.GetConfigValue("03"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = string.Format("<7000|^1500;|OIC, BPLO");

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.MarginLeft = 300;
                this.axVSPrinter1.FontItalic = true;
               
                sContent = "\tNote: Please disregard this notice if you have already renewed your business permit.";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);
                this.axVSPrinter1.FontItalic = false;

                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";
                sContent = "\tReceived by     :_____________________________________________";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);
                sContent = "\tDate Received :_____________________________________________";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);

                
            }
            else if (m_strNoticeNum == "Second")//|| m_strNoticeNum == "Third"
            {
                string first = GetNoticeDate(m_sBIN, 1);
                string second = GetNoticeDate(m_sBIN, 2);

                sContent = "\tThis is to remind you again of your failure to renew your business permit for the current year despite our previous notice";
                sContent += " dated " + first + ".";// + " and " + second
                this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

                this.axVSPrinter1.Paragraph = "";
                sContent = "\tWe are giving you another five (5) working days after receipt of this notice to renew your permit.";
                sContent += " Should you fail to act on this, we have no choice but to recommend closure of your business pursuant to";
                sContent += AppSettingsManager.GetConfigObject("42");
                this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = string.Format("<7000|<3500;|Very truly yours,");
                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("<7000|<3500;|{0}", AppSettingsManager.GetConfigValue("03"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = string.Format("<7000|^1500;|OIC, BPLO");

                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.MarginLeft = 300;
                this.axVSPrinter1.FontItalic = true;
                sContent = "\tNote: Please disregard this notice if you have already renewed your business permit.";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);
                this.axVSPrinter1.FontItalic = false;

                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";
                sContent = "\tReceived by     :_____________________________________________";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);
                sContent = "\tDate Received :_____________________________________________";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);

                

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
            OracleResultSet pSet = new OracleResultSet();

            sDataRow = "";


            this.axVSPrinter1.MarginLeft = 500;
            
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

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
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sBnsName = pRec.GetString(0);
                            sBnsAddr = pRec.GetString(1);
                        }
                    }
                    pRec.Close();

                    sDataRow = sISNum + "|";
                    sDataRow += sBnsName + "||||";
                    this.axVSPrinter1.Table = string.Format("<2000|=3000|<200|<2800|<200|<2800;{0}", sDataRow);
                   

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
                            sViolation += Violations.GetViolationDesc(sViolationCode) + "\n\n";

                        }

                    }
                    pRec.Close();

                    sDataRow = sDate + "|";
                    sDataRow += StringUtilities.HandleApostrophe(sBnsAddr) + "||";
                    sDataRow += StringUtilities.HandleApostrophe(sRemarks) + "||";
                    sDataRow += StringUtilities.HandleApostrophe(sViolation) + ";";
                    sViolation = "";

                    this.axVSPrinter1.Table = string.Format("<2000|=3000|<200|=2800|<200|=2800;{0}", sDataRow);

                    sDataRow = "|||" + sAddlInfo + "||";
                    this.axVSPrinter1.Table = string.Format("<2000|=3000|<200|=2800|<200|=2800;{0}", sDataRow);
                    this.axVSPrinter1.Paragraph = ""; 
                }
            }
            pSet.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<10900;Inspector: {0}", m_strInspector);
            this.axVSPrinter1.Table = string.Format("<10900;{0}", m_strPosition);

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
            OracleResultSet pSet = new OracleResultSet();

            
            

            pSet.Query = m_strQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    this.axVSPrinter1.FontSize = 10;

                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Table = string.Format(">10000;{0}", strCurrentDate);

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
                    this.axVSPrinter1.Table = string.Format("<2000|<7000;{0}", sDataRow);

                    sDataRow = "BUSINESS ADDRESS|: " + sBnsAddr;
                    this.axVSPrinter1.Table = string.Format("<2000|<7000;{0}", sDataRow);

                    sDataRow = "NAME OF OWNER|: " + sOwner;
                    this.axVSPrinter1.Table = string.Format("<2000|<7000;{0}", sDataRow);

                    sDataRow = "DATE INSPECTED|: " + sDate;
                    this.axVSPrinter1.Table = string.Format("<2000|<7000;{0}", sDataRow);

                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Table = string.Format("<10000;FINDINGS:;");

                    this.axVSPrinter1.Table = string.Format("=8000;{0}", sRemarks);
                    this.axVSPrinter1.Table = string.Format("=8000;{0}", sAddlInfo);

                    if (sViolation != "")
                    {
                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.Table =string.Format("<10000;VIOLATIONS:;");
                        this.axVSPrinter1.Table =string.Format("=8000;{0}", sViolation);

                    }

                    this.axVSPrinter1.CurrentY = 10000;
                    this.axVSPrinter1.Table =string.Format("<5450|<5450;Noted by :|Verified by :;");
                    this.axVSPrinter1.Table =string.Format("<5450|<5450;|{0};", m_strInspector);
                    this.axVSPrinter1.FontSize = 8;

                    this.axVSPrinter1.Table = string.Format("<5450|<5450;Account Chief Inspection Services|{0};", m_strPosition);

                    //this.axVSPrinter1.NewPage();
                }

            }
            pSet.Close();
        }

        private void PrintNoticeViolation()
        {
            string sContent = string.Empty;
            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            string sBnsPin = string.Empty;
            string sBldgCode = string.Empty;
            OracleResultSet pSet = new OracleResultSet();
            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
           
            if (m_sBIN == "")
                m_sBIN = m_sISNum;
            
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

            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Paragraph = ""; this.axVSPrinter1.Paragraph = "";

            if (m_strNoticeNum == "First")
                strCurrentDate = GetNoticeDate(m_sISNum, 1);
            else if (m_strNoticeNum == "Second")
                strCurrentDate = GetNoticeDate(m_sISNum, 2);
            else if (m_strNoticeNum == "Third")
                strCurrentDate = GetNoticeDate(m_sISNum, 3);

            if (strCurrentDate == "")
                strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            
            this.axVSPrinter1.Table = string.Format(">10000;{0}", strCurrentDate);
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.MarginLeft = 1000;

            this.axVSPrinter1.Table = string.Format("<10000;{0}", m_strBnsOwner);
            this.axVSPrinter1.Table = string.Format("=10000;{0}", AppSettingsManager.GetBnsAddress(m_sBIN));
            this.axVSPrinter1.Table = string.Format("<10000;{0}", sBnsPin);
            this.axVSPrinter1.Paragraph = ""; this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<10000;Dear Sir/Madam: " + m_strBnsOwner);
            this.axVSPrinter1.Paragraph = ""; this.axVSPrinter1.Paragraph = "";

            if (m_strNoticeNum == "First")
            {
                this.axVSPrinter1.FontSize = 10;
                
                if (AppSettingsManager.GetConfigValue("01") == "MUNICIPALITY")
                {
                    sContent = "\tOur record shows that you have violated " + AppSettingsManager.GetConfigObject("42") + ", of this Municipality by operating the following business ";
                    sContent += "undertaking/s without securing the necessary permit from the Office of the Municipal Mayor:";
                }
                else
                {
                    sContent = "\tOur record shows that you have violated " + AppSettingsManager.GetConfigObject("42") + ", of this city by operating the following business ";
                    sContent += "undertaking/s without securing the necessary permit from the Office of the City Mayor:";
                }
                this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = string.Format("<2000|=10500;|1. {0}", m_strBnsType);

                this.axVSPrinter1.FontSize = 10;
                this.axVSPrinter1.Paragraph = "";
                sContent = "\tIn view thereof, we are requesting you, or your authorized representative, to secure permit for your business within five (5)";
                sContent += " working days after receipt of this notice.";
                this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

                this.axVSPrinter1.FontSize = 10;
                this.axVSPrinter1.Paragraph = "";
                sContent = "\tPlease give this matter your most preferential attention.";
                this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = string.Format("<7000|<3500;|Very truly yours,");

                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("<7000|<3500;|{0}", AppSettingsManager.GetConfigValue("03"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = string.Format("<7000|^1500;|{0}", AppSettingsManager.GetConfigValue("remarks", "config", "code", "03", "03"));  

                this.axVSPrinter1.FontSize = 10;
                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.MarginLeft = 300;

                sContent = "\tReceived by     :_____________________________________________";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);
                sContent = "\tDate Received :_____________________________________________";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);

                
            }
            else if (m_strNoticeNum == "Second")
            {
                string first = GetNoticeDate(m_sISNum, 1);
                string second = GetNoticeDate(m_sISNum, 2);

                sContent = "\tThis is to remind you of your failure to secure business permit of business undertaking/s despite our previous notices";
                sContent += " dated " + first + " respectively.";
                this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

                this.axVSPrinter1.Paragraph = "";
                sContent = "\tIn view of the foregoing, we are giving you five (5) working days after receipt of this notice to secure your permit.";
                sContent += " Should you fail to act on this, we have no choice but to recommend closure of your business pursuant to ";
                sContent += AppSettingsManager.GetConfigObject("42");
                this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = string.Format("<7000|<3500;|Very truly yours,");
                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("<7000|<3500;|{0}", AppSettingsManager.GetConfigValue("03"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = string.Format("<7000|^1500;|OIC, BPLO");

                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.MarginLeft = 300;
                this.axVSPrinter1.FontItalic = true;
                sContent = "\tNote: Please disregard this notice if you already have a business permit";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);
                this.axVSPrinter1.FontItalic = false;

                this.axVSPrinter1.Paragraph = "";this.axVSPrinter1.Paragraph = "";
                sContent = "\tReceived by     :_____________________________________________";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);
                sContent = "\tDate Received :_____________________________________________";
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);

               
            }

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

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            CreateHeader();
        }
    }
}