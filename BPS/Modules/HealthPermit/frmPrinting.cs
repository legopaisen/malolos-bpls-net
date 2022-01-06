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
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmPrinting : Form
    {
        public frmPrinting()
        {
            InitializeComponent();
        }

        public DataTable dt = new DataTable();

        private string m_sIssuedDate = String.Empty;
        public string IssuedDate
        {
            set { m_sIssuedDate = value; }
        }
        private string m_sPermitNo = String.Empty;
        public string PermitNo
        {
            set { m_sPermitNo = value; }
        }
        private string m_sReportType = String.Empty;
        private string m_sBin = String.Empty;
        private string m_sTaxYear = String.Empty;
        public string BIN
        {
            set { m_sBin = value; }
        }
        public string ReportType
        {
            set { m_sReportType = value; }
        }
        public string TaxYear
        {
            set { m_sTaxYear = value; }
        }

        private string m_sORNo = String.Empty;
        private string m_sORDate = String.Empty;
        private string m_sFeeAmount = String.Empty;

        //JHB 20190926 for Brgy Clearance (s)
        private string m_sIssuedOn = String.Empty;
        public string IssuedOn
        {
            set { m_sIssuedOn = value; }
        }

        private string m_sTempBIN = string.Empty;
        public string TempBIN
        {
            get { return m_sTempBIN; }
            set { m_sTempBIN = value; }
        }
        //JHB 20190926 for Brgy Clearance (e)
        private bool brgy_reprint = false; //jhb 20200108

        public string ORNo
        {
            set { m_sORNo = value; }
        }
        public string ORDate
        {
            set { m_sORDate = value; }
        }
        public string FeeAmount
        {
            set { m_sFeeAmount = value; }
        }

        private string m_sBnsName = string.Empty;
        public string BnsName
        {
            set { m_sBnsName = value; }
        }

        private string m_sBnsCode = string.Empty;
        public string BnsCode
        {
            set { m_sBnsCode = value; }
        }

        private string m_sBnsAdd = string.Empty;
        public string BnsAdd
        {
            set { m_sBnsAdd = value; }
        }

        private string m_sBnsOwn = string.Empty;
        public string BnsOwn
        {
            set { m_sBnsOwn = value; }
        }

        //MCR 20150113 (s) Working Permit
        private string m_sEmpID = string.Empty;
        public string EmpID
        {
            set { m_sEmpID = value; }
        }

        //MCR 20150113 (e) Working Permit

        public string m_timeIN = string.Empty;
        public string timeIN
        {
            set { m_timeIN = value; }
        }

        private string m_sOffice = string.Empty;
        public string Office
        {
            set { m_sOffice = value; }
        }

        private string m_sEngRemarks = string.Empty;

        private void frmPrinting_Load(object sender, EventArgs e)
        {
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape


            if (m_sReportType == "Health")
            {
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                Health();
            }
            else if (m_sReportType == "Working Permit")
            {
                axVSPrinter1.MarginTop = 1080;
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                WorkingPermit();
            }
            else if (m_sReportType == "Sanitary" || m_sReportType == "Sanitary-Ext")
            {
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                //Sanitary();
                SanitaryNew();  // RMC 20141222 modified permit printing
            }
            else if (m_sReportType == "Zoning")
            {
                axVSPrinter1.MarginTop = 1080;
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                Zoning();
            }
            else if (m_sReportType == "Annual Inspection" || m_sReportType == "Annual Inspection-Ext")
            {
                axVSPrinter1.MarginTop = 1080;
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                AnnualInspection();
            }
            else if (m_sReportType == "Barangay Clearance")
            {
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                Barangay_Certificate_new();
            }

            else if (m_sReportType == "ApprovalTrail")
            {
                axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; 
                this.Text = "Approval Trail";
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.MarginLeft = 500;
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                ApprovalTrail();
            }
            else if (m_sReportType == "ListApproved")
            {
                axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; 
                this.Text = "List of Approved";
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.MarginLeft = 500;
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.FontSize = 10;
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                ListApproved();
            }

            else if (m_sReportType == "PrintList")
            {
                axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; 
                this.Text = "Approval Trail";
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.MarginLeft = 500;
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                PrintApprovalList();
            }
            else if (m_sReportType == "HealthList") //AFM 20220103
            {
                axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
                this.Text = "List of Employees";
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.MarginLeft = 500;
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.FontSize = 10;
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                HealthEmployees();
            }

            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void HealthEmployees() //AFM 20220103
        {
            string sFormat = string.Empty;
            string sObject = string.Empty;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.Paragraph = "No of Employees";
            axVSPrinter1.Paragraph = "";

            sFormat = "^2000|^3500;";
            sObject = "BIN|No. of Employees";

            axVSPrinter1.Table = sFormat + sObject;
            axVSPrinter1.Paragraph = "";

            sFormat = "<500|<2000;";

            string sValue = string.Empty;
            int iNumEmployees = 0;
            OracleResultSet res = new OracleResultSet();
            res.Query = "select nvl(num_employees,0) from business_que where bin = '" + m_sBin + "'";
            int.TryParse(res.ExecuteScalar(), out iNumEmployees);

            sValue = m_sBin + "|" + iNumEmployees.ToString();

            this.axVSPrinter1.Table = string.Format("^2000|^3500;{0}", sValue);
        }

        private void PrintApprovalList()
        {
            string sValue = string.Empty;
            int iCnt = 0;
            axVSPrinter1.FontSize = 10;

            axVSPrinter1.Paragraph = "For Approval List";
            axVSPrinter1.Paragraph = "As of " + AppSettingsManager.GetCurrentDate().ToString("MMMM dd, yyyy");
            axVSPrinter1.Paragraph = "Office: " + m_sOffice;
            axVSPrinter1.Paragraph = "";

            sValue = "Seq|Violation|BIN|Business Name|Tax Year|Last Name|First Name|M.I.|Business Address|Bns Stat|Area|Lessor";
            this.axVSPrinter1.Table = string.Format("^500|^2000|^1500|^2000|^700|^1000|^1000|^1000|^2000|^1000|^700|^1500;{0}", sValue);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sValue = "";
                iCnt++;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(sValue))
                        sValue += "|";
                    sValue += dt.Rows[i].ItemArray[j].ToString();

                }

                this.axVSPrinter1.Table = string.Format("<500|<2000|<1500|<2000|<700|<1000|<1000|<1000|<2000|<1000|<700|<1500;{0}", sValue);
            }

            axVSPrinter1.Table = string.Format("<10000; ");
            axVSPrinter1.Table = string.Format("<10000;Total Records: {0}", iCnt.ToString("#,###"));
        }

        private void Barangay_Certificate_new() //AFM 20211207 barangay clearance
        {
            this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            this.axVSPrinter1.DrawPicture(Properties.Resources.blue_line, "0.01", "2.1in", "80%", "90%", 10, false);
            this.axVSPrinter1.DrawPicture(Properties.Resources.blue_line___vertical, "2.5in", "1.65in", "92%", "100%", 10, false); 


            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sYear2 = string.Empty;
            string sBrgy = string.Empty;
            string sDate = string.Empty;
            string sStat = AppSettingsManager.GetBnsStat(m_sBin);
            string sTempBin = m_sTempBIN;
            string sOwnCode = AppSettingsManager.GetOwnCode(m_sBin);
            string sUserNm = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            DateTime sDateSave = AppSettingsManager.GetSystemDate();

            //Baranagy Officials
            string m_strBrgyCode = string.Empty;
            string m_strCapt = string.Empty;
            string m_strTreas = string.Empty;
            string m_strSec = string.Empty;
            string m_strKwd1 = string.Empty;
            string m_strKwd2 = string.Empty;
            string m_strKwd3 = string.Empty;
            string m_strKwd4 = string.Empty;
            string m_strKwd5 = string.Empty;
            string m_strKwd6 = string.Empty;
            string m_strKwd7 = string.Empty;
            string m_strSk = string.Empty;
            //Baranagy Officials

            string DATE_CREATED = string.Format("{0:dd-MMM-yyyy}", sDateSave);
            string strProvinceName = string.Empty;

            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate());
            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
            sMonth = string.Format("{0:MMMM}", AppSettingsManager.GetCurrentDate());
            sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());

            sBrgy = AppSettingsManager.GetBnsBrgy(m_sBin);
            sDate = sMonth + " " + sDay + ", " + sYear2;

            //header
            //  long yGrid = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = 900;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            // this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontName = "Segoe UI";
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.Table = "^16000;REPUBLIC OF THE PHILIPPINES";
            this.axVSPrinter1.FontBold = false;

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValueRemarks("08") + " " + strProvinceName);
            this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("09"));
            
            //Barangay
            this.axVSPrinter1.CurrentY = 1800;
            this.axVSPrinter1.FontSize = (float)13.0;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^16000;BARANGAY {0}", sBrgy);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)13.5;
            this.axVSPrinter1.Table = string.Format("^16000;OFFICE OF THE PUNONG BARANGAY");

            //BARANGAY HEADER
            this.axVSPrinter1.MarginLeft = 3000;
            this.axVSPrinter1.CurrentY = 3500;
            this.axVSPrinter1.SpaceBefore = 750;
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = "<10|^5000;|BARANGAY BUSINESS CLEARANCE";
            this.axVSPrinter1.FontBold = false;


            
            axVSPrinter1.MarginLeft = 4200;
            this.axVSPrinter1.CurrentY = 2900;
            axVSPrinter1.SpaceBefore = 750;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.FontSize = 12;
            


            //BARANGAY OFFICIAL (s)
            result2.Query = "Select * from brgy where brgy_nm = '" + sBrgy + "' ";
            if (result2.Execute())
            {
                while (result2.Read())
                {
                    m_strBrgyCode = result2.GetString("BRGY_CODE");
                    m_strCapt = result2.GetString("BRGY_CAPT");
                    m_strTreas = result2.GetString("BRGY_TREAS");
                    m_strSec = result2.GetString("BRGY_SEC");
                    m_strKwd1 = result2.GetString("BRGY_KAGAWAD1");
                    m_strKwd2 = result2.GetString("BRGY_KAGAWAD2");
                    m_strKwd3 = result2.GetString("BRGY_KAGAWAD3");
                    m_strKwd4 = result2.GetString("BRGY_KAGAWAD4");
                    m_strKwd5 = result2.GetString("BRGY_KAGAWAD5");
                    m_strKwd6 = result2.GetString("BRGY_KAGAWAD6");
                    m_strKwd7 = result2.GetString("BRGY_KAGAWAD7");
                    m_strSk = result2.GetString("SK_CHAIR");
                }
            }
            result2.Close();
            //BARANGAY OFFICIAL (e)

            //001	ANILAO
            if (m_strBrgyCode == "001")
            {

            }
            //002	ATLAG
            if (m_strBrgyCode == "002")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_atlag, "1.5in", "0.55in", "90%", "90%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_atlag_signatory, "5.9in", "8.55in", "100%", "100%", 10, false);
            }
            //003	BABATNIN
            if (m_strBrgyCode == "003")
            {

            }
            //004	BAGNA
            if (m_strBrgyCode == "004")
            {

            }
            //005	BAGONG BAYAN
            if (m_strBrgyCode == "005")
            {

            }
            //006	BALAYONG
            if (m_strBrgyCode == "006")
            {

            }
            //007	BALITE
            if (m_strBrgyCode == "007")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_balite, "1.5in", "0.55in", "90%", "90%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_balite_sig, "5.9in", "8.55in", "100%", "100%", 10, false);
            }
            //008	BANGKAL
            if (m_strBrgyCode == "008")
            {

            }
            //009	BARIHAN
            if (m_strBrgyCode == "009")
            {
            }
            //010	BULIHAN
            if (m_strBrgyCode == "010")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_bulihan, "1.5in", "0.55in", "90%", "90%", 10, false);

            }
            //011	BUNGAHAN
            if (m_strBrgyCode == "011")
            {
            }
            //012	CAINGIN
            if (m_strBrgyCode == "012")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_caingin, "1.5in", "0.55in", "90%", "90%", 10, false);
            }
            //013	CALERO
            if (m_strBrgyCode == "013")
            {
                
            }
            //014	CANALATE
            if (m_strBrgyCode == "014")
            {

            }
            //015	CANIOGAN
            if (m_strBrgyCode == "015")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_caniogan, "1.5in", "0.55in", "90%", "90%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_caniogan_sig, "5.9in", "8.55in", "100%", "100%", 10, false);
            }
            //016	CATMON
            if (m_strBrgyCode == "016")
            {

            }
            //017	COFRADIA
            if (m_strBrgyCode == "017")
            {
                
            }
            //018	DAKILA
            if (m_strBrgyCode == "018")
            {
                
            }
            //019	GUINHAWA
            if (m_strBrgyCode == "019")
            {
                
            }
            //020	KALILIGAWAN
            if (m_strBrgyCode == "020")
            {
                
            }
            //021	LIANG
            if (m_strBrgyCode == "021")
            {

            }
            //022	LIGAS
            if (m_strBrgyCode == "022")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_ligas, "1.5in", "0.55in", "90%", "90%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_ligas_sig, "5.9in", "8.55in", "100%", "100%", 10, false);
            }
            //023	LONGOS
            if (m_strBrgyCode == "023")
            {

            }
            //024	LOOK 1ST
            if (m_strBrgyCode == "024")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_look1st, "1.5in", "0.55in", "90%", "90%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_look1st_sig, "5.9in", "8.55in", "100%", "100%", 10, false);
            }
            //025	LOOK 2ND
            if (m_strBrgyCode == "025")
            {

            }
            //026	LUGAM
            if (m_strBrgyCode == "026")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_lugam, "1.5in", "0.55in", "90%", "90%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_lugam_sig, "5.9in", "8.55in", "100%", "100%", 10, false);
            }
            //027	MABOLO
            if (m_strBrgyCode == "027")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_mabolo, "1.5in", "0.55in", "90%", "90%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_mabolo_sig, "5.9in", "8.55in", "100%", "100%", 10, false);
            }
            //028	MAMBOG
            if (m_strBrgyCode == "028")
            {

            }
            //029	MASILE
            if (m_strBrgyCode == "029")
            {

            }
            //030	MATIMBO
            if (m_strBrgyCode == "030")
            {

            }
            //031	MOJON
            if (m_strBrgyCode == "031")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_mojon, "1.5in", "0.55in", "90%", "90%", 10, false);
            }
            //032	NAMAYAN
            if (m_strBrgyCode == "032")
            {
            }
            //033	NIUGAN
            if (m_strBrgyCode == "033")
            {
            }
            //034	PAMARAWAN
            if (m_strBrgyCode == "034")
            {
            }
            //035	PANASAHAN
            if (m_strBrgyCode == "035")
            {
            }
            //036	PINAGBAKAHAN
            if (m_strBrgyCode == "036")
            {
            }
            //037	SAN AGUSTIN
            if (m_strBrgyCode == "037")
            {
            }
            //038	SAN GABRIEL
            if (m_strBrgyCode == "038")
            {
            }
            //039	SAN JUAN
            if (m_strBrgyCode == "039")
            {
            }
            //040	SAN PABLO
            if (m_strBrgyCode == "040")
            {
            }
            //041	SANTIAGO
            if (m_strBrgyCode == "041")
            {
            }
            //042	SANTISMA TRINIDAD
            if (m_strBrgyCode == "042")
            {
            }
            //043	STO. CRISTO
            if (m_strBrgyCode == "043")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_stocristo, "1.5in", "0.55in", "90%", "90%", 10, false);
            }
            //044	STO NINO
            if (m_strBrgyCode == "044")
            {
            }
            //045	SANTOR
            if (m_strBrgyCode == "045")
            {
            }
            //046	STO ROSARIO
            if (m_strBrgyCode == "046")
            {
            }
            //047	SAN VICENTE
            if (m_strBrgyCode == "047")
            {
            }
            //048	SUMAPANG BATA
            if (m_strBrgyCode == "048")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.brgy_sumapangbata_sig, "5.9in", "8.55in", "100%", "100%", 10, false);
            }
            //049	SUMAPANG MATANDA
            if (m_strBrgyCode == "049")
            {
            }
            //050	TAAL
            if (m_strBrgyCode == "050")
            {
            }
            //051	TIKAY
            if (m_strBrgyCode == "051")
            {
            }


            //BARANGAY OFFICIAL HEADER(s)
            string sTitle = string.Empty;
            this.axVSPrinter1.MarginRight = 12900;
            this.axVSPrinter1.FontItalic = true;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 9;
            this.axVSPrinter1.CurrentY = 3300;
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strCapt;
            this.axVSPrinter1.CurrentY = 3500;
            this.axVSPrinter1.FontBold = false;
            sTitle = "Punong Barangay";
            this.axVSPrinter1.Table = "<500|<2000;|" + sTitle;

            this.axVSPrinter1.Table = "<500|<3000;|Barangay Kagawad:";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = 5500; 
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd1;

            this.axVSPrinter1.CurrentY = 6000;
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd2;

            this.axVSPrinter1.CurrentY = 6500; 
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd3;

            this.axVSPrinter1.CurrentY = 7000; 
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd4;

            this.axVSPrinter1.CurrentY = 7500; 
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd5;

            this.axVSPrinter1.CurrentY = 8000; 
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd6;

            this.axVSPrinter1.CurrentY = 8500; 
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd7;

            this.axVSPrinter1.CurrentY = 10400; 
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strSk;
            this.axVSPrinter1.CurrentY = 10600;
            this.axVSPrinter1.FontBold = false;
            sTitle = "SK Chairman";
            this.axVSPrinter1.Table = "<500|<2000;|" + sTitle;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = 11450; 
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strTreas;
            this.axVSPrinter1.CurrentY = 11650;
            this.axVSPrinter1.FontBold = false;
            sTitle = "Treasurer";
            this.axVSPrinter1.Table = "<500|<2000;|" + sTitle;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = 12500; 
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strSec;
            this.axVSPrinter1.CurrentY = 12700;
            this.axVSPrinter1.FontBold = false;
            sTitle = "Secretary";
            this.axVSPrinter1.Table = "<500|<2000;|" + sTitle;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontItalic = false;

            this.axVSPrinter1.TextColor = Color.Black;
            //BARANGAY OFFICIAL HEADER(e)

            this.axVSPrinter1.MarginRight = 0;


            //BUSINESS NAME  (S)
            string sBnsName = m_sBnsName.Trim();
            // string  sBnsName = StringUtilities.HandleApostrophe(m_sBnsName).Trim();
            // string sBnsName = StringUtilities.RemoveApostrophe(m_sBnsName).Trim(); 
            this.axVSPrinter1.CurrentY = 6800;
            this.axVSPrinter1.MarginLeft = 500;//3800
            if (sBnsName.Length > 100)
            {
                axVSPrinter1.FontSize = 7;
            }
            else if (sBnsName.Length > 35)
            {
                axVSPrinter1.FontSize = 8;
            }
            else
            {
                axVSPrinter1.FontSize = 10;
            }

            this.axVSPrinter1.FontBold = false;
            //BUSINESS NAME (E)


            OracleResultSet res = new OracleResultSet();
            string sBnsCode = string.Empty;
            res.Query = "select bns_code from businesses where bin = '" + m_sBin + "'";
            if (res.Execute())
                if (res.Read())
                    sBnsCode = res.GetString(0);
            string sBnsKind = AppSettingsManager.GetBnsDesc(sBnsCode);

            //PARAGRAPH (s)


            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.CurrentY = 4500;
            this.axVSPrinter1.Table = "<10|<2800;|This is to certify that ";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.CurrentY = 5500;
            this.axVSPrinter1.Table = ">2500|<8000;|Business Name: " + m_sBnsName;

            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.CurrentY = 6100;
            this.axVSPrinter1.Table = ">2500|<8000;|Taxpayer's Name: " + m_sBnsOwn;

            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.CurrentY = 6600;
            this.axVSPrinter1.Table = ">2500|<8000;|Line of Business: " + sBnsKind;

            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.CurrentY = 7100;
            this.axVSPrinter1.Table = ">1000|<6500;|Business Address: " + m_sBnsAdd;


            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.MarginLeft = 2000;
            this.axVSPrinter1.CurrentY = 9000;
            this.axVSPrinter1.Table = "<5300|<9000;|is doing business in Barangay " + sBrgy + ", City of Malolos, Bulacan.";

            this.axVSPrinter1.MarginLeft = 2000;
            this.axVSPrinter1.CurrentY = 10000;
            this.axVSPrinter1.Table = "<2300|<6000;|This Certification is issued under the Local Government Code Republic Act No. 7160, Article IV, Section 152, Paragraph C, for the purpose of securing business permit from the City Government of Malolos for the year 2022";


            this.axVSPrinter1.FontItalic = true;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 2000;
            this.axVSPrinter1.CurrentY = 12000;
            this.axVSPrinter1.Table = "<4000|^6000;|" + m_strCapt;
            this.axVSPrinter1.CurrentY = 12200;
            this.axVSPrinter1.FontBold = false;
            sTitle = "Punong Barangay";
            this.axVSPrinter1.Table = "<4000|^4000;|" + sTitle;


            this.axVSPrinter1.FontItalic = false;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.MarginLeft = 3500;
            this.axVSPrinter1.CurrentY = 12700;
            this.axVSPrinter1.Table = "<10|<8000;|Amount Paid: " + m_sFeeAmount;

            this.axVSPrinter1.MarginLeft = 3500;
            this.axVSPrinter1.CurrentY = 13000;
            this.axVSPrinter1.Table = "<10|<8000;|OR No: " + m_sORNo;

            this.axVSPrinter1.MarginLeft = 3500;
            this.axVSPrinter1.CurrentY = 13300;
            this.axVSPrinter1.Table = "<10|<8000;|Date: " + m_sORDate;
            //PARAGRAPH (e)


            CheckifBrgyCleanExist(m_sBin);


            if (brgy_reprint == false)
            {

                result.Query = "Insert into BRGY_CLEARANCE(BIN, TAX_YEAR, TEMP_BIN, BNS_NM, BNS_STAT, OWN_CODE, OWN_NM, MARITAL_STAT, ";
                result.Query += "BNS_BRGY, CTC_NO, CTC_ISSUED_ON, CTC_ISSUED_AT, CTC_AMT, DATE_CREATED, BNS_USER) ";
                result.Query += "values('" + m_sBin + "', '" + sYear2 + "', '" + sTempBin + "', '" + StringUtilities.HandleApostrophe(sBnsName) + "', '" + sStat + "', '" + sOwnCode + "', '" + StringUtilities.HandleApostrophe(m_sBnsOwn) + "', '', ";
                result.Query += "'" + sBrgy + "', '" + m_sORNo + "', '" + m_sIssuedOn + "', '" + m_sORDate + "', " + m_sFeeAmount + ", '" + DATE_CREATED + "', '" + sUserNm + "')";
            }
            else
            {
                result.Query = "UPDATE BRGY_CLEARANCE SET CTC_NO = '" + m_sORNo + "',CTC_ISSUED_ON = '" + m_sIssuedOn + "', BNS_BRGY = '" + sBrgy + "',OWN_CODE = '" + sOwnCode + "',BNS_STAT= '" + sStat + "', ";
                result.Query += "CTC_ISSUED_AT = '" + m_sORNo + "', CTC_AMT = " + m_sFeeAmount + " WHERE BIN = '" + m_sBin + "' and tax_year = '" + sYear2 + "' ";

            }

            result.ExecuteNonQuery();
            if (!result.Commit())
            {
                result.Rollback();
            }
            result.Close();

            
            // RMC 20170822 added transaction log feature for tracking of transactions per bin (s)
            if (AuditTrail.InsertTrail("ABBC", "Barangay Clearance", "Print Barangay Clearance BIN: " + m_sBin + " for tax year " + m_sTaxYear) == 0)
            {
                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // RMC 20170822 added transaction log feature for tracking of transactions per bin (e)

        } //JHB 20191012 add for brgy clearance (e)

        private void Barangay_Certificate() //JHB 20191012 add for brgy clearance  clearance old(s)(s)
        {
            //   this.axVSPrinter1.DrawPicture(Properties.Resources.Untitled_1," 0.10in", "-2.5in", "33.0%", "46.8%", 10, false);
            OracleResultSet result = new OracleResultSet();
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sYear2 = string.Empty;
            string sBrgy = string.Empty;
            string sDate = string.Empty;
            string sStat = AppSettingsManager.GetBnsStat(m_sBin);
            //string sMaritalStat = AppSettingsManager.GetMaritalStat(m_sBin);
            string sMaritalStat = "";
            string sTempBin = m_sTempBIN;
            string sOwnCode = AppSettingsManager.GetOwnCode(m_sBin);
            string sUserNm = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            DateTime sDateSave = AppSettingsManager.GetSystemDate();
            string DATE_CREATED = string.Format("{0:MM/dd/yyyy}", sDateSave);


            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate());
            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
            sMonth = string.Format("{0:MMMM}", AppSettingsManager.GetCurrentDate());
            sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());

            sBrgy = AppSettingsManager.GetBnsBrgy(m_sBin);


            sDate = sMonth + " " + sDay + ", " + sYear2;
            long yGrid = Convert.ToInt64(axVSPrinter1.CurrentY);


            //DATE
            axVSPrinter1.MarginLeft = 3800;
            this.axVSPrinter1.CurrentY = 3000;
            axVSPrinter1.SpaceBefore = 750;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Table = "<10|>7500;|" + sDate;



            //OWNER'S ADDRESS  (S)
            this.axVSPrinter1.CurrentY = 6300;
            axVSPrinter1.MarginLeft = 3800;

            if (m_sBnsAdd.Length > 100)
            {
                axVSPrinter1.FontSize = 7;
            }
            else if (m_sBnsAdd.Length > 35)
            {
                axVSPrinter1.FontSize = 9;
            }
            else
            {
                axVSPrinter1.FontSize = 12;
            }
            axVSPrinter1.Table = "<10|<5000;|" + m_sBnsAdd;
            //OWNER'S ADDRESS (E)

            //BUSINESS NAME  (S)
            string sBnsName = m_sBnsName;
            //   string sBnsName = StringUtilities.HandleApostrophe(m_sBnsName).Trim();
            this.axVSPrinter1.CurrentY = 7000;
            axVSPrinter1.MarginLeft = 3800;
            if (sBnsName.Length > 100)
            {
                axVSPrinter1.FontSize = 7;
            }
            else if (sBnsName.Length > 35)
            {
                axVSPrinter1.FontSize = 8;
            }
            else
            {
                axVSPrinter1.FontSize = 10;
            }

            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = "<10|<5000;|" + sBnsName;
            //BUSINESS NAME (E)


            //OR DETAILS (S)

            axVSPrinter1.FontSize = 12;
            axVSPrinter1.CurrentY = 11800; //12000;//11700
            axVSPrinter1.MarginLeft = 6300;
            axVSPrinter1.Table = "<10|<5000;| " + m_sORNo;
            axVSPrinter1.CurrentY = 12280; //12400;//12200
            axVSPrinter1.MarginLeft = 5200;
            axVSPrinter1.Table = "<10|<5000;|" + m_sORDate;
            axVSPrinter1.CurrentY = 12680; //12900;//12700
            axVSPrinter1.MarginLeft = 5200;
            axVSPrinter1.Table = "<10|<5000;|" + m_sIssuedOn;
            //axVSPrinter1.CurrentY = 12900;
            //axVSPrinter1.MarginLeft = 5200;
            //axVSPrinter1.Table = "<10|<5000;|O.R. Date: " + m_sFeeAmount;
            CheckifBrgyCleanExist(m_sBin);

            if (brgy_reprint == false)
            {

                result.Query = "Insert into BRGY_CLEARANCE(BIN, TAX_YEAR, TEMP_BIN, BNS_NM, BNS_STAT, OWN_CODE, OWN_NM, MARITAL_STAT, ";
                result.Query += "BNS_BRGY, CTC_NO, CTC_ISSUED_ON, CTC_ISSUED_AT, CTC_AMT, DATE_CREATED, BNS_USER) ";
                result.Query += "values('" + m_sBin + "', '" + sYear2 + "', '" + sTempBin + "', '" + StringUtilities.HandleApostrophe(sBnsName).Trim() + "', '" + sStat + "', '" + sOwnCode + "', '" + StringUtilities.HandleApostrophe(m_sBnsOwn).Trim() + "', '" + sMaritalStat + "', ";
                result.Query += "'" + sBrgy + "', '" + m_sORNo + "', '" + m_sIssuedOn + "', '" + m_sORDate + "', '" + m_sFeeAmount + "', '" + DATE_CREATED + "', '" + sUserNm + "')";
            }
            else
            {
                result.Query = "UPDATE BRGY_CLEARANCE SET CTC_NO = '" + m_sORNo + "',CTC_ISSUED_ON = '" + m_sIssuedOn + "', MARITAL_STAT = '" + sMaritalStat + "',BNS_BRGY = '" + sBrgy + "',OWN_CODE = '" + sOwnCode + "',BNS_STAT= '" + sStat + "', ";
                result.Query += "CTC_ISSUED_AT = '" + m_sORDate + "', CTC_AMT = '" + m_sFeeAmount + "' WHERE BIN = '" + m_sBin + "' and tax_year = '" + sYear2 + "' ";

            }

            result.ExecuteNonQuery();
            if (!result.Commit())
            {
                result.Rollback();
            }
            result.Close();


            //axVSPrinter1.Table = "<10|<2000|<2000|O.R. No.: " + m_sORNo;
            //axVSPrinter1.Table = "<10|<2000|<2000|O.R. Date: " + m_sORDate;
            //axVSPrinter1.Table = "<10|<2000|<2000|O.R. Amount: " + m_sFeeAmount;
            //axVSPrinter1.SpaceBefore = 0;
            //axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. Date ________| OIC- SANITATION| Municipal Health Officer";
            ////axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. Date. _________|" + AppSettingsManager.GetConfigRemarksValue("58") + "|" + AppSettingsManager.GetConfigObject("59");
            //axVSPrinter1.Table = "<10|<2800;|Fee Paid Php______";

        }

        private void Zoning()
        {
            OracleResultSet result = new OracleResultSet();

            string sORNo = string.Empty;
            string sFeeAmount = string.Empty;
            string sTCTNo = string.Empty;
            string sZoning = string.Empty;
            string sName = string.Empty;
            string sSPNo = string.Empty;
            string sMPNo = string.Empty;
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;

            // RMC 20150105 mods in permit printing (s)
            
            string sAdd = string.Empty;
            result.Query = "select * from emp_names where (bin = '" + m_sBin + "' or temp_bin = '" + m_sBin + "') and tax_year = '" + m_sTaxYear + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sAdd = result.GetString("bns_add");
                }
            }
            result.Close();
            // RMC 20150105 mods in permit printing (e)

            result.Query = "Select * from zoning where bin ='" + m_sBin + "'";
            result.Query += " and tax_year = '" + m_sTaxYear + "'";     // RMC 20150105 mods in permit printing
            if (result.Execute())
            {
                while (result.Read())
                {
                    try
                    {
                        sTCTNo = "TCT No. " + result.GetString("TCT_No");
                        sZoning = result.GetString("zoning");
                        sName = result.GetString("own_ln") + ", " + result.GetString("own_fn") + " " + result.GetString("own_mi");
                        sSPNo = result.GetString("sp_no");
                        sMPNo = result.GetString("mp_no");
                        sYear = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
                        sMonth = string.Format("{0:MMM}", AppSettingsManager.GetCurrentDate());
                        //sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());
                        sDay = string.Format("{0}", AppSettingsManager.GetCurrentDate().Day);

                        axVSPrinter1.MarginLeft = 750;
                        axVSPrinter1.SpaceBefore = 3500;
                        /*axVSPrinter1.FontSize = 14;
                        axVSPrinter1.Table = "<10|<4200|<3500|<3000;|This is to certify that a lot bearing|" + sTCTNo + "| located in Barangay";
                        axVSPrinter1.SpaceBefore = 50;
                        axVSPrinter1.Table = "<10|<11000|<3200;|San Roque Arbol, Lubao, Pampanga, is within the " + sZoning;
                        axVSPrinter1.SpaceBefore = 100;
                        axVSPrinter1.Table = "<10|<11000;|Municipality's Comprehensive Land User Plan approved by the Sangguniang";
                        axVSPrinter1.Table = "<10|<5000|<3500|<4000;|Panlalawigan through SP Resolution No.|" + sSPNo + "|and adopted by the";
                        axVSPrinter1.SpaceBefore = 70;
                        axVSPrinter1.Table = "<10|<7600|<3500;|Sangguniang Bayan of Lubao through Municipal Resolution No.|" + sMPNo;
                        axVSPrinter1.SpaceBefore = 200;
                        axVSPrinter1.Table = "<10|<4300|<3000|<700|<1200|<1300|<1800;|This certification is being issued to |" + sName + "  this |" + sDay + "th" + "| day of |" + sMonth + ". " + sYear;
                        axVSPrinter1.SpaceBefore = 100;
                        axVSPrinter1.Table = "<10|<8000;|for reference purposes only.";*/

                        // RMC 20150105 mods in permit printing (s)
                        axVSPrinter1.FontSize = 12;
                        //axVSPrinter1.TabIndex = 100;
                        //string sFormat = "=9000;";
                        string sData = string.Empty;

                        this.axVSPrinter1.IndentTab = "0.25in";
                        this.axVSPrinter1.IndentFirst = "0.25in";
                        sData = "This is to certify that a lot bearing " + sTCTNo + " located in " + sAdd + ", ";
                        sData += "Lubao, Pampanga, is within the " + sZoning + " Zone of Municipality's Comprehensive Land Use Plan approved by the Sangguniang ";
                        sData += "Panlalawigan through SP Resolution No. " + sSPNo + " and adopted by the ";
                        sData += "Sangguniang Bayan of Lubao through Municipal Resolution No. " + sMPNo + ".";
                        this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);
                        //axVSPrinter1.Table = sFormat + sData;
                        //axVSPrinter1.Paragraph = "";
                        //sFormat = "=9000;";
                        axVSPrinter1.SpaceBefore = 0;
                        this.axVSPrinter1.IndentTab = "0.25in";
                        this.axVSPrinter1.IndentFirst = "0.25in";
                        sData = "This certification is being issued to  " + sName + "  this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ". " + sYear + " ";
                        sData += "for reference purposes only.";
                        this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);
                        this.axVSPrinter1.IndentFirst = "0.0in";
                        // RMC 20150105 mods in permit printing (e)

                        axVSPrinter1.MarginLeft = 7000;
                        axVSPrinter1.SpaceBefore = 2000;
                        axVSPrinter1.Table = "<10|^3000;|" + AppSettingsManager.GetConfigValue("60");
                        axVSPrinter1.SpaceBefore = 0;
                        axVSPrinter1.Table = "<10|^3000;|MPDC/DZA";
                        axVSPrinter1.MarginLeft = 800;
                        axVSPrinter1.SpaceBefore = 1000;
                        //axVSPrinter1.Table = "<10|<4000;|O.R. No. _________"; // RMC 20150107 removed
                        axVSPrinter1.SpaceBefore = 0;
                        //axVSPrinter1.Table = "<10|<4000;|O.R. Date ________"; // RMC 20150107 removed
                        //axVSPrinter1.Table = "<10|<3500;|Fee Paid Php______"; // RMC 20150107 removed
                    }
                    catch
                    {
                        sORNo = "";
                        sFeeAmount = "";
                        sTCTNo = "";
                        sZoning = "";
                        sName = "";
                        sSPNo = "";
                        sMPNo = "";
                    }
                }
            }
        }

        private void Health()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rCount = new OracleResultSet();

            string sName = string.Empty;
            string sAge = string.Empty;
            string sSex = string.Empty;
            string sOccupation = string.Empty;
            string sPlaceOfWork = string.Empty;
            string sNationality = string.Empty;
            string sDate = string.Empty;
            string sExpiration = string.Empty;
            string sXRay = string.Empty;
            string sXRayResult = string.Empty;
            string sHepa = string.Empty;
            string sHepaResult = string.Empty;
            string sDrugTest = string.Empty;
            string sDrugTestResult = string.Empty;
            string sFeca = string.Empty;
            string sFecaResult = string.Empty;
            string sUrine = string.Empty;
            string sUrineResult = string.Empty;
            string sRegNo = string.Empty;
            int sCount = 0;
            int iCount = 0;

            //result.Query = "Select * from emp_names where bin ='" + m_sBin + "' and tax_year = '" + m_sTaxYear + "'";
            //rCount.Query = "Select count(*) as emp_count from emp_names where bin ='" + m_sBin + "' and tax_year = '" + m_sTaxYear + "'";
            result.Query = "Select * from emp_names where (bin ='" + m_sBin + "' or temp_bin = '" + m_sBin + "') and tax_year = '" + m_sTaxYear + "'";  // RMC 20141228 modified permit printing (lubao)
            rCount.Query = "Select count(*) as emp_count from emp_names where (bin ='" + m_sBin + "' or temp_bin = '" + m_sBin + "') and tax_year = '" + m_sTaxYear + "'";  // RMC 20141228 modified permit printing (lubao)
            int.TryParse(rCount.ExecuteScalar(), out sCount);

            if (result.Execute())
            {
                while (result.Read())
                {
                    try
                    {
                        sName = result.GetString("emp_ln") + ", " + result.GetString("emp_fn") + " " + result.GetString("emp_mi");
                        sAge = result.GetInt("emp_age").ToString();
                        sSex = result.GetString("emp_gender");
                        sOccupation = result.GetString("emp_occupation");
                        sPlaceOfWork = result.GetString("emp_place_of_work");
                        sNationality = result.GetString("emp_nationality");
                        sDate = result.GetString("emp_issuance_date");
                        sExpiration = result.GetString("emp_expiration_date");
                        sXRay = result.GetString("emp_xray_date");
                        sXRayResult = result.GetString("emp_xray_result");
                        sHepa = result.GetString("emp_hepab_date");
                        sHepaResult = result.GetString("emp_hepab_result");
                        sDrugTest = result.GetString("emp_drug_test_date");
                        sDrugTestResult = result.GetString("emp_drug_test_result");
                        sFeca = result.GetString("emp_fecalysis_date");
                        sFecaResult = result.GetString("emp_fecalysis_result");
                        sUrine = result.GetString("emp_urinalysis_date");
                        sUrineResult = result.GetString("emp_urinalysis_result");
                        sRegNo = result.GetString("emp_id");

                        axVSPrinter1.MarginLeft = 4560;
                        axVSPrinter1.Table = "<10|<5000;|";
                        axVSPrinter1.SpaceBefore = 200;
                        axVSPrinter1.FontSize = 8;
                        axVSPrinter1.Table = "<10|<5000;|" + sRegNo;
                        axVSPrinter1.MarginLeft = 510;
                        axVSPrinter1.SpaceBefore = 70;
                        axVSPrinter1.FontSize = 12;
                        axVSPrinter1.Table = "<10|<5000;|" + sName;
                        axVSPrinter1.SpaceBefore = 580;
                        axVSPrinter1.Table = "<10|<1700|<5000;|" + sAge + "|" + sSex;
                        axVSPrinter1.Table = "<10|<5000;|" + sOccupation;
                        axVSPrinter1.Table = "<10|<5000;|" + sPlaceOfWork;
                        axVSPrinter1.Table = "<10|<5000;|" + sNationality;
                        axVSPrinter1.SpaceBefore = 0;

                        iCount++;
                        if (iCount <= sCount)
                        {
                            axVSPrinter1.NewPage();
                            axVSPrinter1.MarginLeft = 500;
                            axVSPrinter1.Table = "<10|<5000;|";
                            axVSPrinter1.SpaceBefore = 350;
                            axVSPrinter1.Table = "<10|^2300|^3400;|" + sDate + "|" + sExpiration;
                            axVSPrinter1.SpaceBefore = 1200;
                            axVSPrinter1.Table = "<10|<3500|<3400;|" + sXRay + "|" + sXRayResult;
                            axVSPrinter1.SpaceBefore = 450;
                            axVSPrinter1.Table = "<10|<3500|<3400;|" + sHepa + "|" + sHepaResult;
                            axVSPrinter1.Table = "<10|<3500|<3400;|" + sDrugTest + "|" + sDrugTestResult;
                            axVSPrinter1.Table = "<10|<3500|<3400;|" + sFeca + "|" + sFecaResult;
                            axVSPrinter1.Table = "<10|<3500|<3400;|" + sUrine + "|" + sUrineResult;
                            axVSPrinter1.SpaceBefore = 0;
                            if (iCount < sCount)
                            {
                                axVSPrinter1.NewPage();
                            }
                        }
                    }
                    catch
                    {
                        sName = "";
                        sAge = "";
                        sSex = "";
                        sOccupation = "";
                        sPlaceOfWork = "";
                        sNationality = "";
                    }
                }
            }
            result.Close();
        }

        private void SanitaryNew()
        {
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sYear2 = string.Empty;

            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate());
            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
            sMonth = string.Format("{0:MMMM}", AppSettingsManager.GetCurrentDate());
            //sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());
            sDay = string.Format("{0}", AppSettingsManager.GetCurrentDate().Day);

            axVSPrinter1.MarginLeft = 9000; 
            axVSPrinter1.SpaceBefore = 750; 
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Table = "<10|<5000;| PERMIT NO." + m_sPermitNo;

            axVSPrinter1.MarginLeft = 500; 
            axVSPrinter1.SpaceBefore = 2100;

            if (m_sBnsName.Length > 50)  
            {
                axVSPrinter1.FontSize = 18;  // MCR 20150121 mods in permit printing
            }
            else if (m_sBnsName.Length > 20)   // RMC 20150105 mods in permit printing
            {
                axVSPrinter1.FontSize = 24;  // RMC 20150105 mods in permit printing
                //axVSPrinter1.SpaceBefore = 1200;    // RMC 20150105 mods in permit printing
            }
            else
            {
                axVSPrinter1.FontSize = 32;
                //axVSPrinter1.SpaceBefore = 1800;    // RMC 20150105 mods in permit printing
            }

            axVSPrinter1.FontBold = true;

            axVSPrinter1.SpaceBefore = 1800;    // RMC 20150107
            long yGrid = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = 2476;

            if (m_sReportType == "Sanitary")
            {
                // RMC 20150107 (s)
                string sTmp = AppSettingsManager.GetDTIName(m_sBin).Trim();
                if (sTmp != "")
                    m_sBnsName = sTmp;
                // RMC 20150107 (e)
            }            
             
            axVSPrinter1.Table = "<10|^10000;|" + m_sBnsName;
            axVSPrinter1.MarginLeft = 3000; 
            //axVSPrinter1.SpaceBefore = 100; 
            
            axVSPrinter1.SpaceBefore = 1000;     // RMC 20150105 mods in permit printing
            
            //this.axVSPrinter1.CurrentY = 5032;  // RMC 20150105 mods in permit printing
            axVSPrinter1.FontSize = 14;
            axVSPrinter1.FontBold = false;
            yGrid = Convert.ToInt32(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = 5032;
            axVSPrinter1.Table = "<10|^8000;|" + m_sBnsAdd;

            axVSPrinter1.MarginLeft = 3000;
            axVSPrinter1.SpaceBefore = 300;
            //axVSPrinter1.SpaceBefore = 0;   // RMC 20150107
            //this.axVSPrinter1.CurrentY = 6368;  // RMC 20150105 mods in permit printing
            //yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
            //yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());    // RMC 20150113 corrections in sanitary, put rem
            this.axVSPrinter1.CurrentY = 6368;
            axVSPrinter1.Table = "<10|^6000;|" + m_sBnsOwn;
            
            //axVSPrinter1.MarginLeft = 5500;
            //axVSPrinter1.SpaceBefore = 750;
            axVSPrinter1.MarginLeft = 5550; // RMC 20150105 mods in permit printing
            axVSPrinter1.SpaceBefore = 900; // RMC 20150105 mods in permit printing
            //this.axVSPrinter1.CurrentY = 7004;  // RMC 20150105 mods in permit printing
            //yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
            axVSPrinter1.Table = "<10|<500;|" + sYear;

            //axVSPrinter1.MarginLeft = 4200;
            axVSPrinter1.MarginLeft = 4250; // RMC 20150105 mods in permit printing
            //axVSPrinter1.SpaceBefore = 330;
            axVSPrinter1.SpaceBefore = 360; // RMC 20150105 mods in permit printing
            //axVSPrinter1.Table = "<10|^500|^3850|^500;|" + sDay + "|" + sMonth + "|" + sYear;
            //this.axVSPrinter1.CurrentY = 8240;  // RMC 20150105 mods in permit printing
            //yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
            axVSPrinter1.Table = "<10|^500|^3950|^500;|" + sDay + "|" + sMonth + "|" + sYear;   // RMC 20150105 mods in permit printing

            axVSPrinter1.MarginLeft = 1800;
            axVSPrinter1.SpaceBefore = 2000;
            axVSPrinter1.FontSize = 32;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = "<10|<3000;|" + sYear2;

            axVSPrinter1.MarginLeft = 4800;
            axVSPrinter1.SpaceBefore = 100;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.FontBold = false;
            axVSPrinter1.Table = "<10|<3000|<1300;| Recommending Approval: | Approved:";


            //DJAC-20150102 added o.r fields
            axVSPrinter1.MarginLeft = 1800;
            axVSPrinter1.SpaceBefore = 300;
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Table = "<10|<2800|^3000|^3000;|O.R. No. _________|" + AppSettingsManager.GetConfigValue("58") + "|" + AppSettingsManager.GetConfigValue("59");
            axVSPrinter1.SpaceBefore = 0;
            axVSPrinter1.Table = "<10|<2800|^3000|^3000;|O.R. Date ________| Sanitary Inspector IV | Municipal Health Officer";
            axVSPrinter1.Table = "<10|<2800;|Fee Paid Php______";
        }

        private void Sanitary()
        {
            OracleResultSet result = new OracleResultSet();

            string sLocation = string.Empty;
            string sBNSName = string.Empty;
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sBIN = string.Empty;
            string sORNo = string.Empty;
            string sORDate = string.Empty;
            string sFeeAmount = string.Empty;

            result.Query = "Select * from businesses where bin ='" + m_sBin + "'";
            
            if (result.Execute())
            {
                while (result.Read())
                {
                    try
                    {
                        sYear = Convert.ToDateTime(m_sIssuedDate).Year.ToString();
                        sMonth = Convert.ToDateTime(m_sIssuedDate).ToString("MMMM");
                        sDay = Convert.ToDateTime(m_sIssuedDate).Day.ToString();
                        sLocation = result.GetString("bns_house_no")
                            + " " + result.GetString("bns_street")
                            + " " + result.GetString("bns_brgy")
                            + " " + result.GetString("bns_dist")
                            + " " + result.GetString("bns_zone")
                            + " " + result.GetString("bns_mun")
                            + " " + result.GetString("bns_zip")
                            + " " + result.GetString("bns_prov");
                        sBNSName = result.GetString("bns_nm");
                        sBIN = m_sBin;
                        sORNo = m_sORNo;
                        sORDate = m_sORDate;
                        sFeeAmount = m_sFeeAmount;

                        axVSPrinter1.FontSize = 7;
                        axVSPrinter1.MarginLeft = 4500;
                        axVSPrinter1.Table = "<10|<1600;| PERMIT NO." + m_sPermitNo;
                        axVSPrinter1.MarginLeft = 500;
                        axVSPrinter1.SpaceBefore = 1000;
                        axVSPrinter1.FontSize = 16;
                        axVSPrinter1.FontBold = true;
                        
                        axVSPrinter1.Table = "<10|^5500;|" + sBNSName;
                        axVSPrinter1.MarginLeft = 460;
                        axVSPrinter1.SpaceBefore = 500;
                        axVSPrinter1.FontSize = 7;
                        axVSPrinter1.FontBold = false;
                        
                        axVSPrinter1.Table = "<10|<700|<4700;|" + "located at|" + sLocation;
                        axVSPrinter1.SpaceBefore = 0;
                        axVSPrinter1.MarginLeft = 1170;
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                        axVSPrinter1.Table = "<4600; ";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                        axVSPrinter1.MarginLeft = 330;
                        axVSPrinter1.Table = "<10|<800|<3500|<2500;|" + "operated by|" + sBNSName + "|is hereby granted a";
                        axVSPrinter1.SpaceBefore = 0;
                        axVSPrinter1.MarginLeft = 1140;
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                        axVSPrinter1.Table = "<3500; ";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                        axVSPrinter1.MarginLeft = 2300;
                        axVSPrinter1.Table = "<10|<1600;| SANITARY PERMIT";
                        axVSPrinter1.MarginLeft = 330;
                        axVSPrinter1.SpaceBefore = 180;
                        axVSPrinter1.Table = "<10|^5200;|" + "This expires on the  31st of  December " + sYear + " unless sooner revoked when the interest of the public so requires.";
                        axVSPrinter1.MarginLeft = 1400;
                        axVSPrinter1.SpaceBefore = 20;
                        
                        axVSPrinter1.Table = "<10|^3000;|" + "Issued this  " + sDay + "  day of  " + sMonth + ", " + sYear + ".";
                        axVSPrinter1.MarginLeft = 700;
                        axVSPrinter1.SpaceBefore = 1150;
                        axVSPrinter1.FontSize = 16;
                        axVSPrinter1.FontBold = true;
                        
                        axVSPrinter1.Table = "<10|<5500;|" + sYear;
                        axVSPrinter1.MarginLeft = 2800;
                        axVSPrinter1.SpaceBefore = 100;
                        axVSPrinter1.FontSize = 6;
                        axVSPrinter1.FontBold = false;
                        axVSPrinter1.Table = "<10|<1800|<1600;| Recommending Approval: | Approved:";
                        axVSPrinter1.MarginLeft = 460;
                        axVSPrinter1.SpaceBefore = 20;
                        axVSPrinter1.Table = "<10|<850|<1800;|B.I.N.|" + sBIN;
                        axVSPrinter1.SpaceBefore = 0;
                        axVSPrinter1.Table = "<10|<850|<1560|^1450|^1800;|O.R. No.|" + sORNo + "|" + AppSettingsManager.GetConfigValue("58") + "|" + AppSettingsManager.GetConfigValue("59");
                        axVSPrinter1.Table = "<10|<850|<1560|^1450|^1800;|O.R. Date|" + sORDate + "| Sanitary Inspector IV | Municipal Health Officer";
                        axVSPrinter1.Table = "<10|<850|<1800;|Amount Paid|Php " + sFeeAmount;
                    }
                    catch
                    {
                        sLocation = "";
                        sBNSName = "";
                        sDay = "";
                        sMonth = "";
                        sYear = "";
                        sBIN = "";
                        sORNo = "";
                        sORDate = "";
                        sFeeAmount = "";
                    }
                }
            }
            result.Close();
        }

        private void AnnualInspection()
        {
            OracleResultSet result = new OracleResultSet();

            string sYear = string.Empty;
            string sORDate = string.Empty;
            string sFeeAmount = string.Empty;
            string sYear2 = string.Empty;
            string sName = string.Empty;
            string sCharOfOcc = string.Empty;
            string sCharGrp = string.Empty;
            string sLocation = string.Empty;
            string sCertOcc = string.Empty;
            string sDate = string.Empty;
            string sCertORNo = string.Empty;
            string sStallNo = string.Empty;

            int iCnt = 0;
            //result.Query = "select count(*) from sanitary_bldg_ext where bin = '" + m_sBin + "'";
            //int.TryParse(result.ExecuteScalar(), out iCnt);

            for (int ix = 0; ix <= iCnt; ix++)
            {
                result.Query = "Select * from annual_insp where bin ='" + m_sBin + "'";
                if (result.Execute())
                {

                    while (result.Read())
                    {
                        try
                        {
                            //sYear = m_sIssuedDate.ToString().Substring(m_sIssuedDate.Length - 2, 2);
                            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate()); //DJAC-20150102 change format
                            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
                            sORDate = m_sORDate;
                            sFeeAmount = m_sFeeAmount;
                            sName = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBin));

                            // RMC 20150117 (s)
                            if(sName.Trim() == "")
                                sName = result.GetString("own_ln") + ", " + result.GetString("own_fn") + " " + result.GetString("own_mi");
                            // RMC 20150117 (e)

                            if (m_sReportType == "Annual Inspection")
                                sCharOfOcc = result.GetString("char_occ");
                            else
                                sCharOfOcc = GetExtName("name");
                            sCharGrp = result.GetString("char_grp");
                            sLocation = result.GetString("location");
                            sCertOcc = result.GetString("cert_occ");
                            sDate = result.GetDateTime("cert_date").ToString("MM/dd/yyyy");
                            sCertORNo = result.GetString("cert_or_no");
                            //if (ix == 0)
                            if(m_sReportType =="Annual Inspection")
                                sStallNo = result.GetString("stall_no");
                            else
                                sStallNo = GetExtName("area");

                            axVSPrinter1.FontSize = 12;
                            axVSPrinter1.MarginLeft = 2000;
                            //axVSPrinter1.SpaceBefore = 2800;
                            //axVSPrinter1.Table = "<10|<4500|<6000;|" + "NO. _____________|DATE OF ISSUANCE: ____________________";

                            // RMC 20150105 mods in permit printing (s)
                            axVSPrinter1.SpaceBefore = 2900;

                            OracleResultSet pSetT = new OracleResultSet();
                            string sPermitNo = string.Empty;
                            string sDateIssued = string.Empty;

                            pSetT.Query = "select * from permit_type where bin = '" + m_sBin + "' and current_year = '" + sYear2 + "' ";
                            if (m_sReportType == "Annual Inspection")
                                pSetT.Query += " and perm_type = 'ANNUAL'";
                            else
                                pSetT.Query += " and perm_type = 'ANNUAL-EXT'";
                            if (pSetT.Execute())
                            {
                                if (pSetT.Read())
                                {
                                    sPermitNo = pSetT.GetString("current_year") + "-" + pSetT.GetString("permit_number");
                                    sDateIssued = pSetT.GetString("issued_date");
                                }
                            }

                            axVSPrinter1.Table = "<10|<4500|<6000;|" + "NO. " + sPermitNo + "|DATE OF ISSUANCE: " + sDateIssued;
                            // RMC 20150105 mods in permit printing (e)

                            //axVSPrinter1.FontSize = 10;
                            //axVSPrinter1.MarginLeft = 350;
                            //axVSPrinter1.SpaceBefore = 200;
                            //axVSPrinter1.Table = "<10|<6000;|This certification of Annual Inspection is issued/granted Pursuant to pertinent provision of Rule III of";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|the National Building Code PD 1096.";
                            //axVSPrinter1.FontSize = 12;

                            axVSPrinter1.MarginLeft = 1500;
                            axVSPrinter1.SpaceBefore = 1200;
                            if (sName.Length > 50)
                                axVSPrinter1.FontSize = 10;

                            axVSPrinter1.Table = "<10|<3500|<7000;|Name of Owner/Lessee: |" + sName;
                            axVSPrinter1.SpaceBefore = 0;
                            // RMC 20150107 (s)
                            string sTmp = AppSettingsManager.GetDTIName(m_sBin).Trim();
                            if (sTmp != "")
                                sCharOfOcc = sTmp;
                            // RMC 20150107 (e)
                            axVSPrinter1.Table = "<10|<3500|<7000;|Character of Occupancy: |" + sCharOfOcc; ;
                            axVSPrinter1.Table = "<10|<3500|<7000;|Group: |" + sCharGrp;
                            axVSPrinter1.Table = "<10|<3500|<5000;|Located at/Along: |" + sLocation;
                            axVSPrinter1.Table = "<10|<3500|<5000;|Under Certificate of Occupancy: |" + sCertOcc;
                            axVSPrinter1.Table = "<10|<3500|<5000;|Date: |" + sDate;
                            axVSPrinter1.Table = "<10|<3500|<5000;|With Official Receipt No.: |" + sCertORNo;
                            axVSPrinter1.Table = "<10|<3500|<5000;|Area/Stall No.: |" + sStallNo + " SQ.M";      // RMC 20150113 corrections in annual inspection entry of area

                            //axVSPrinter1.FontSize = 6;
                            //axVSPrinter1.MarginLeft = 350;
                            //axVSPrinter1.SpaceBefore = 180;
                            //axVSPrinter1.Table = "<10|<6000;|The Owner/Lessee shall properly maintain in the Building/Structure to enhance architectural well-";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|being structural stability, sanitation and fire-protective properties and shall not be occupied or used";
                            //axVSPrinter1.Table = "<10|<6000;|for the purpose other that its intended as stated above.";
                            //axVSPrinter1.SpaceBefore = 100;
                            //axVSPrinter1.Table = "<10|<6000;|No alteration/addition/repairs/new electronics or mechanical/plumbing/sanitary installation shall be";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|made thereon without a permit therefore.";
                            //axVSPrinter1.SpaceBefore = 100;
                            //axVSPrinter1.Table = "<10|<6000;|The designer is aware that the under Article 1723 of the Civil Code of the Philippines be is";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|responsible for damages if within fifteen(15) years from the completion of the structure. It should";
                            //axVSPrinter1.Table = "<10|<6000;|collapse due to defect in the plans of specifications of the structure to ensure that the condition";
                            //axVSPrinter1.Table = "<10|<6000;|under which the structure was designed are not being violated abused.";
                            //axVSPrinter1.SpaceBefore = 100;
                            //axVSPrinter1.Table = "<10|<6000;|A certified copy hereof shall be posted within the premises of the building and shall not be remove";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|without authority from the building official.";

                            axVSPrinter1.FontSize = 32;
                            axVSPrinter1.MarginLeft = 1800;
                            axVSPrinter1.SpaceBefore = 4650;
                            axVSPrinter1.FontBold = true;
                            axVSPrinter1.Table = "<10|<3000;|" + sYear2;
                            axVSPrinter1.SpaceBefore = 0;

                            axVSPrinter1.FontSize = 12;
                            axVSPrinter1.MarginLeft = 1800;
                            //axVSPrinter1.SpaceBefore = 300;
                            axVSPrinter1.SpaceBefore = 100;
                            axVSPrinter1.FontBold = false;
                            axVSPrinter1.Table = "<10|<4000|^5000;|O.R. No. _________|" + AppSettingsManager.GetConfigValue("61");
                            axVSPrinter1.SpaceBefore = 0;
                            axVSPrinter1.Table = "<10|<4000|^5000;|O.R. Date ________| Building Official";
                            axVSPrinter1.Table = "<10|<3500;|Fee Paid Php______";
                            //axVSPrinter1.FontSize = 7;
                            //axVSPrinter1.FontBold = false;
                            //axVSPrinter1.Table = "<10|<800|<1850|<1600;|Date Paid |" + sORDate + "|" + AppSettingsManager.GetConfigValue("61");
                            //axVSPrinter1.Table = "<10|<800|<1850|<1600;|Fee Paid |" + sFeeAmount + "|" + "Building Official";

                        }
                        catch
                        {
                            sYear = "";
                            sYear2 = "";
                            sORDate = "";
                            sFeeAmount = "";
                            sName = "";
                            sCharOfOcc = "";
                            sCharGrp = "";
                            sLocation = "";
                            sCertOcc = "";
                            sDate = "";
                            sCertORNo = "";
                            sStallNo = "";
                        }
                    }
                }
                result.Close();

                //axVSPrinter1.NewPage();
            }
        }

        private void WorkingPermit()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet rCount = new OracleResultSet();

            string m_sOccupation = string.Empty;
            string m_sPermitNo = string.Empty;
            string m_sPermitDate = string.Empty;
            string sPeriodCovered = string.Empty;
            string sTIN = string.Empty;
            string sCTCNo = string.Empty;
            string sIssuedOn = string.Empty;
            string sIssuedAt = string.Empty;
            string sApplicant = string.Empty;
            string sAddress = string.Empty;
            string sGender = string.Empty;
            string sDOB = string.Empty;
            string sJob = string.Empty;
            string sEmployer = string.Empty;
            string sBusinessAdd = string.Empty;

            int sCount = 0;
            int iCount = 0;

            m_sOccupation = "OWNER";

            result2.Query = "Select current_year, permit_number, issued_date from permit_working_number where bin ='" + m_sBin + "' and emp_id = '" + m_sEmpID + "'";
            if (result2.Execute())
            {
                while (result2.Read())
                {
                    m_sPermitNo = result2.GetString("current_year") + "-" + result2.GetString("permit_number");
                    m_sPermitDate = result2.GetDateTime("issued_date").ToString("MM/dd/yyyy");
                }
            }

            result.Query = @"select EMP_ID,EMP_TIN,EMP_CTC_NUMBER,EMP_CTC_ISSUED_ON,EMP_CTC_ISSUED_AT,
EMP_LN||', '||EMP_FN||' '||EMP_MI as EMP_NAME,EMP_ADDRESS,EMP_GENDER,EMP_DATE_OF_BIRTH,EMP_OCCUPATION,BIN,TAX_YEAR
from emp_names where (bin = '" + m_sBin + "' or temp_bin  = '" + m_sBin + "') and tax_year = '" + m_sTaxYear + "' and emp_id = '" + m_sEmpID + "'";

            //rCount.Query = "Select count(*) as emp_count from emp_names where bin ='" + m_sBin + "' and tax_year = '" + m_sTaxYear + "' and emp_occupation != '" + m_sOccupation + "'";
            //int.TryParse(rCount.ExecuteScalar(), out sCount);

            axVSPrinter1.FontSize = 8;
            if (result.Execute())
            {
                while (result.Read())
                {
                    sPeriodCovered = "";
                    sTIN = result.GetString("emp_tin");
                    sCTCNo = result.GetString("emp_ctc_number");
                    sIssuedOn = result.GetString("emp_ctc_issued_on");
                    sIssuedAt = result.GetString("emp_ctc_issued_at");

                    sApplicant = result.GetString("emp_name");
                    sAddress = result.GetString("emp_address");
                    sGender = result.GetString("emp_gender");
                    sDOB = result.GetString("emp_date_of_birth");
                    sJob = result.GetString("emp_occupation");
                    sEmployer = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBin));
                    sBusinessAdd = AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(m_sBin));

                    axVSPrinter1.CurrentY = 1200;
                    axVSPrinter1.MarginLeft = 2600;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + m_sORNo + "|" + m_sPermitNo;
                    axVSPrinter1.SpaceBefore = 160;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + m_sORDate + "|" + m_sPermitDate;
                    axVSPrinter1.Table = "<10|<3000;|" + sPeriodCovered;
                    axVSPrinter1.SpaceBefore = 0;

                    axVSPrinter1.MarginLeft = 2400;
                    this.axVSPrinter1.CurrentY = 2105;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + sTIN + "|" + sCTCNo;
                    this.axVSPrinter1.CurrentY = 2521;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + sIssuedOn + "|" + sIssuedAt;
                    axVSPrinter1.MarginLeft = 290;

                    this.axVSPrinter1.CurrentY = 2960;
                    axVSPrinter1.MarginLeft = 1700;
                    axVSPrinter1.Table = "<10|<4600;|" + sApplicant;
                    axVSPrinter1.MarginLeft = 290;
                    this.axVSPrinter1.CurrentY = 3350;
                    axVSPrinter1.Table = "<10|<5000;|" + sAddress;

                    this.axVSPrinter1.CurrentY = 3870;
                    axVSPrinter1.Table = "<10|<1050|<1400|<2500;|" + sGender + "|" + sDOB + "|" + sJob;

                    this.axVSPrinter1.CurrentY = 4260;
                    axVSPrinter1.MarginLeft = 1700;
                    axVSPrinter1.Table = "<10|<4600;|" + sEmployer;
                    axVSPrinter1.MarginLeft = 290;
                    this.axVSPrinter1.CurrentY = 4610;
                    axVSPrinter1.Table = "<10|<5000;|" + sBusinessAdd;

                    axVSPrinter1.MarginLeft = 4120;
                    this.axVSPrinter1.CurrentY = 5053;
                    axVSPrinter1.Table = "<10|<2000;|" + m_sTaxYear;
                    axVSPrinter1.SpaceBefore = 0;

                    //iCount++;
                    //if (iCount < sCount)
                    //{
                    //    axVSPrinter1.NewPage();
                    //}
                }
            }
            result.Close();
        }

        private void ToolPrint_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintQuality = VSPrinter7Lib.PrintQualitySettings.pqHigh;

            if (MessageBox.Show("Are you sure you want to print?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            axVSPrinter1.PrintDoc(1, 1, axVSPrinter1.PageCount);

            // RMC 20141222 modified permit printing (s)
            OracleResultSet pSet = new OracleResultSet();
            if (m_sReportType == "Zoning")
            {
                if (AuditTrail.InsertTrail("ABZC-P", "zoning", StringUtilities.HandleApostrophe(m_sBin)) == 0)
                {
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (m_sReportType == "Annual Inspection")
            {
                if (AuditTrail.InsertTrail("ABAIP-P", "annual_insp", StringUtilities.HandleApostrophe(m_sBin)) == 0)
                {
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (m_sReportType == "Sanitary")
            {
                if (AuditTrail.InsertTrail("ABSP-P", "sanitary", StringUtilities.HandleApostrophe(m_sBin)) == 0)
                {
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // RMC 20141222 modified permit printing (e)
        }

        private void toolSettingPageSetup_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPageSetup);
        }

        private void toolSettingPrintPage_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string ConvertDayInOrdinalForm(string sDay)
        {
            // RMC 20150105 mods in permit printing
            string sLastNo = "";
            string sDayth = "";

            sLastNo = StringUtilities.Right(sDay, 1);

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

        private string GetExtName(string sSwitch)
        {
            OracleResultSet result = new OracleResultSet();
            string sExt = string.Empty;

            result.Query = "select * from sanitary_bldg_ext where bin = '" + m_sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    if(sSwitch == "name")
                        sExt = result.GetString("bns_nm");
                    else if (sSwitch == "area")
                    {
                        double dArea = 0;
                        double.TryParse(result.GetString("area"), out dArea);
                        sExt = string.Format("{0:#,###}", dArea);
                    }
                }
            }
            result.Close();

            return sExt;
        }

        private bool CheckifBrgyCleanExist(string sBin) //JHB 20191012 add for brgy clearance
        {
            OracleResultSet pSet = new OracleResultSet();


            pSet.Query = "select * from brgy_clearance where bin = '" + m_sBin + "' and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {

                    brgy_reprint = true;
                }
            }

            pSet.Close();
            return true;
        }

        private void ApprovalTrail()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sAppBy = string.Empty;
            string sAppDate = string.Empty;
            string sObject = string.Empty;
            string sFormat = string.Empty;

            axVSPrinter1.FontSize = 10;

            axVSPrinter1.Paragraph = "Approval Trail";
            axVSPrinter1.Paragraph = "As of " + AppSettingsManager.GetCurrentDate().ToString("MMMM dd, yyyy");
            axVSPrinter1.Paragraph = "";

            sFormat = "<2000|<3000|<1500|<2000;";
            sObject = "BIN:|" + m_sBin + "|Tax Year:|" + m_sTaxYear;
            axVSPrinter1.Table = sFormat + sObject;
            sFormat = "<2700|<7000;";
            sObject = "Business Name:|" + m_sBnsName;
            axVSPrinter1.Table = sFormat + sObject;
            sObject = "Business Address:|" + m_sBnsAdd;
            axVSPrinter1.Table = sFormat + sObject;

            axVSPrinter1.Paragraph = "";

            sFormat = "^2000|^2000|^2000|^2000";
            sObject = "ENGINEERING|PLANNING|HEALTH|CENRO";
            if (m_sBnsAdd.Contains("PUBLIC MARKET") || m_sBnsAdd.Contains("MAPUMA"))
            {
                sFormat += "|^2000;";
                sObject += "|MARKET";
            }
            else
                sFormat += ";";
            axVSPrinter1.Table = sFormat + sObject;
            axVSPrinter1.Paragraph = "";

            sObject = "";
            pSet.Query = "select * from trans_approve where bin = '" + m_sBin + "' and tax_year = '" + m_sTaxYear + "'";
            pSet.Query += " and office_nm = 'ENGINEERING'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sAppBy = AppSettingsManager.GetUserName(pSet.GetString("app_by"));
                    sAppDate = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("app_dt").ToShortDateString());
                }
                else
                {
                    if (CheckIfDisapproved(m_sBin, "ENGINEERING", m_sTaxYear))
                    {
                        sAppBy = "DISAPPROVED";
                        sAppBy += "\nRemarks: " + m_sEngRemarks;
                    }
                }
            }
            pSet.Close();

            if (!string.IsNullOrEmpty(sAppBy))
                sObject = sAppBy + " " + sAppDate;

            sAppBy = ""; sAppDate = "";
            pSet.Query = "select * from trans_approve where bin = '" + m_sBin + "' and tax_year = '" + m_sTaxYear + "'";
            pSet.Query += " and office_nm = 'PLANNING'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sAppBy = AppSettingsManager.GetUserName(pSet.GetString("app_by"));
                    sAppDate = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("app_dt").ToShortDateString());
                }
                else
                {
                    if (CheckIfDisapproved(m_sBin, "PLANNING", m_sTaxYear))
                        sAppBy = "DISAPPROVED";
                }
            }
            pSet.Close();

            if (!string.IsNullOrEmpty(sAppBy))
                sObject += "|" + sAppBy + " " + sAppDate;
            else
                sObject += "|";


            sAppBy = ""; sAppDate = "";
            pSet.Query = "select * from trans_approve where bin = '" + m_sBin + "' and tax_year = '" + m_sTaxYear + "'";
            pSet.Query += " and office_nm = 'HEALTH'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sAppBy = AppSettingsManager.GetUserName(pSet.GetString("app_by"));
                    sAppDate = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("app_dt").ToShortDateString());
                }
                else
                {
                    if (CheckIfDisapproved(m_sBin, "HEALTH", m_sTaxYear))
                        sAppBy = "DISAPPROVED";
                }
            }
            pSet.Close();

            if (!string.IsNullOrEmpty(sAppBy))
                sObject += "|" + sAppBy + " " + sAppDate;
            else
                sObject += "|";

            sAppBy = ""; sAppDate = "";
            pSet.Query = "select * from trans_approve where bin = '" + m_sBin + "' and tax_year = '" + m_sTaxYear + "'";
            pSet.Query += " and office_nm = 'CENRO'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sAppBy = AppSettingsManager.GetUserName(pSet.GetString("app_by"));
                    sAppDate = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("app_dt").ToShortDateString());
                }
                else
                {
                    if (CheckIfDisapproved(m_sBin, "CENRO", m_sTaxYear))
                        sAppBy = "DISAPPROVED";
                }
            }
            pSet.Close();

            if (!string.IsNullOrEmpty(sAppBy))
                sObject += "|" + sAppBy + " " + sAppDate;
            else
                sObject += "|";

            if (m_sBnsAdd.Contains("PUBLIC MARKET") || m_sBnsAdd.Contains("MAPUMA"))
            {
                sAppBy = ""; sAppDate = "";
                pSet.Query = "select * from trans_approve where bin = '" + m_sBin + "' and tax_year = '" + m_sTaxYear + "'";
                pSet.Query += " and office_nm = 'MARKET'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sAppBy = AppSettingsManager.GetUserName(pSet.GetString("app_by"));
                        sAppDate = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("app_dt").ToShortDateString());
                    }
                    else
                    {
                        if (CheckIfDisapproved(m_sBin, "MARKET", m_sTaxYear))
                            sAppBy = "DISAPPROVED";
                    }
                }
                pSet.Close();

                if (!string.IsNullOrEmpty(sAppBy))
                    sObject += "|" + sAppBy + " " + sAppDate;
                else
                    sObject += "|";
            }

            sFormat = "<2000|<2000|<2000|<2000";
            if (m_sBnsAdd.Contains("PUBLIC MARKET") || m_sBnsAdd.Contains("MAPUMA"))
            {
                sFormat += "|<2000;";
            }
            else
                sFormat += ";";
            axVSPrinter1.Table = sFormat + sObject;

        }

        private void ListApproved()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sObject = string.Empty;
            string sFormat = string.Empty;
            string sBIN = string.Empty;
            string sAppBy = string.Empty;
            string sAppDt = string.Empty;

            axVSPrinter1.Paragraph = "List of Approved in " + m_sOffice;
            axVSPrinter1.Paragraph = "As of " + AppSettingsManager.GetCurrentDate().ToString("MMMM dd, yyyy");
            axVSPrinter1.Paragraph = "";

            if (m_sOffice == "ENGINEERING")
            {
                sFormat = "^2000|^3500|^2000|^1500|^1200;";
                sObject = "BIN|Business Name|Approved By|Approved Date|Assessment";

                axVSPrinter1.Table = sFormat + sObject;
                axVSPrinter1.Paragraph = "";

                sFormat = "<500|<2000|<3500|<2000|<1500|>1200;";
            }
            else if (m_sOffice == "PLANNING") //AFM 20220103 MAO-21-1627
            {
                sFormat = "^2000|^2000|^2000|^2000|^2000|^2000|^1500;";
                sObject = "BIN|Business Name|Business Address|Business Type|Approved By|Approved Date"; //AFM 20220105 requested new column - business type

                axVSPrinter1.Table = sFormat + sObject;
                axVSPrinter1.Paragraph = "";

                sFormat = "<500|<2000|<2000|<2000|<2000|<2000|<2000|<1500;";
            }
            else
            {
                sFormat = "^2000|^4000|^2500|^1500;";
                sObject = "BIN|Business Name|Approved By|Approved Date";

                axVSPrinter1.Table = sFormat + sObject;
                axVSPrinter1.Paragraph = "";

                sFormat = "<500|<2000|<4000|<2500|<1500;";
            }
            int iCnt = 0;
            OracleResultSet result = new OracleResultSet();

            pRec.Query = "select * from trans_approve where office_nm = '" + m_sOffice + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "' order by app_dt, bin";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iCnt++;
                    sBIN = pRec.GetString("bin");
                    sAppBy = AppSettingsManager.GetUserName(pRec.GetString("app_by"));
                    sAppDt = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("app_dt").ToShortDateString());

                    if (m_sOffice == "ENGINEERING")
                    {
                        double dAssess = 0;
                        result.Query = "select * from eps_assess_app where bin = '" + sBIN + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                dAssess = result.GetDouble("annual_fee");
                            }
                        }
                        result.Close();

                        sObject = iCnt.ToString() + "|" + sBIN + "|" + AppSettingsManager.GetBnsName(sBIN) + "|" + sAppBy + "|" + sAppDt + "|" + dAssess.ToString("#,###.00");
                    }
                    else if (m_sOffice == "PLANNING") //AFM 20220103 MAO-21-1627
                    {
                        double dAssess = 0;
                        result.Query = "select * from eps_assess_app where bin = '" + sBIN + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                dAssess = result.GetDouble("annual_fee");
                            }
                        }
                        result.Close();

                        sObject = iCnt.ToString() + "|" + sBIN + "|" + AppSettingsManager.GetBnsName(sBIN) + "|" + AppSettingsManager.GetBnsAdd(sBIN, "") + "|" + AppSettingsManager.GetBnsDesc(AppSettingsManager.GetBnsCodeMain(sBIN)) +"|" + sAppBy + "|" + sAppDt + "|" + dAssess.ToString("#,###.00");
                    }
                    else
                    {
                        sObject = iCnt.ToString() + "|" + sBIN + "|" + AppSettingsManager.GetBnsName(sBIN) + "|" + sAppBy + "|" + sAppDt;
                    }
                    axVSPrinter1.Table = sFormat + sObject;
                }
            }
            pRec.Close();

        }


        private bool CheckIfDisapproved(string sBIN, string sOffice, string sTaxYear)
        {
            OracleResultSet pSet = new OracleResultSet();
            int iCnt = 0;
            m_sEngRemarks = "";

            if (sOffice == "ENGINEERING")
            {
                pSet.Query = "select * from eps_assess_app where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "' and app_stat = '3'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        m_sEngRemarks = pSet.GetString("remarks");
                        iCnt++;
                    }
                }
                pSet.Close();


            }
            else
            {
                pSet.Query = "select count(*) from TRANS_APPROVE_HIST where bin = '" + sBIN + "' and office_nm = '" + sOffice + "' and tax_year = '" + sTaxYear + "' and remarks = 'DISAPPROVED'";
                int.TryParse(pSet.ExecuteScalar(), out iCnt);
            }

            if (iCnt > 0)
                return true;
            else
                return false;

        }

        
    }

}