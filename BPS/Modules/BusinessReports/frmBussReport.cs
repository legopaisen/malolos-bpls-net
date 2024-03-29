

// RMC 20120329 Modifications in Notice of violation
// RMC 20120222 added printing of notice for un-official business in business mapping
// RMC 20120221 added count of printed certifications in Certifications module
// RMC 20120220 added option for using pre-printed form in Certification
// MCR 20140102 added ListBnsQtrDatePaid,ListBnsPrmtNum,ListBnsGrossReceipt,ListBnsBusStatus,ListBnsOrgKind,ListBnsMainBusiness,ListBnsDist,ListBnsBrgy,RetirementReport
// MCR 20140307 added Messagebox
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Common.Message_Box;
using InspectionTool;
using System.Collections;
using Amellar.Common.DynamicProgressBar;
using System.Threading;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmBussReport : Form
    {
        OracleResultSet pRec2 = new OracleResultSet();
        OracleResultSet rs2 = new OracleResultSet();
        public bool bOfficial = false;
        public string sBrgy = string.Empty;
        public bool bUnofficial = false;
        public string sUser = string.Empty;
        private string m_sReportSwitch = "";
        private string m_sBIN = "";
        private DateTime m_dtDate = DateTime.Now;
        public string m_sMonth = "";
        private string m_sOwnCode = "";
        private string m_sOwnLN = "";
        private string m_sOwnFN = "";
        private string m_sOwnMI = "";
        private string m_sOwnAdd = "";
        private string m_sRequestBy = "";
        private string m_sPurpose = ""; //JHB 20180511
        private bool m_bUsePreprinted = false;
        private string m_sUnoffBnsName = "";
        private string m_sQuery = "";   // RMC 20120329 Modifications in Notice of violation
        public DateTime m_dFrom = DateTime.Now; // GDE 20120507
        public DateTime m_dTo = DateTime.Now; // GDE 20120507
        private string m_sReportName = ""; //MCR 20131226
        private string m_sBrgyName = ""; //MCR 20140102
        private string m_sData1 = "";
        public string m_sTaxYear = ""; // JHMN 20170118 early bird taxyear
        public string m_sTop = "";    // JHMN 20170118 early bird top range
        string sPrevBin = "";
        public string sStat = "NEW";
        private string strBrgyTmp;
        private bool blnHasRecord;


        private string m_sInsCode = ""; //JARS 20170911
        public string InspectorCode
        {
            get { return m_sInsCode; }
            set { m_sInsCode = value; }
        }
        private string m_sContactP = "";
        public string ContactPerson
        {
            get { return m_sContactP; }
            set { m_sContactP = value; }
        }
        private string m_sContactN = "";
        public string ContactNumber
        {
            get { return m_sContactN; }
            set { m_sContactN = value; }
        }
        private string m_sEmail = "";
        public string Email
        {
            get { return m_sEmail; }
            set { m_sEmail = value; }
        }
        private string m_sPosition = "";
        public string Position
        {
            get { return m_sPosition; }
            set { m_sPosition = value; }
        }

        //JARS 20171118(S)
        private string m_sSignatory = "";
        private string m_sSignatory2 = ""; //AFM 20210817
        private int m_iId = 0;
        private int m_iId2 = 0; //AFM 20210817
        public int Id
        {
            get { return m_iId; }
            set { m_iId = value; }
        }
        public int Id2 //AFM 20210817
        {
            get { return m_iId2; }
            set { m_iId2 = value; }
        }
        public string Signatory
        {
            get { return m_sSignatory; }
            set { m_sSignatory = value; }
        }
        public string Signatory2 //AFM 20210817
        {
            get { return m_sSignatory2; }
            set { m_sSignatory2 = value; }
        }
        //JARS 20171118(E)

        private string m_sSigposition;

        public string SigPosition
        {
            get { return m_sSigposition; }
            set { m_sSigposition = value; }
        }
	

        //MCR 20171005 (s) EPS CLEARANCE
        public string m_sControlNo = string.Empty;
        public string m_sApplicantStatus = string.Empty;
        public string m_sBussName = string.Empty;
        public string m_sBussLine = string.Empty;
        public string m_sBussAdd = string.Empty;
        public string m_sOwnName = string.Empty;
        public string m_sDateIssued = string.Empty;
        //MCR 20171005 (e) EPS CLEARANCE

        //AFM 20200106 (s) SANITARY CLEARANCE
        public string m_sYrExpiration = string.Empty;
        public string m_sCHOsignatory = string.Empty;
        public string m_sDtMonth = string.Empty;
        public string m_sDtYear = string.Empty;
        //AFM 20200106 (e) SANITARY CLEARANCE

        private string m_sZoneClass = ""; //JARS 20190131
        public string ZoneClassification
        {
            get { return m_sZoneClass; }
            set { m_sZoneClass = value; }
        }

        //MCR 20140103 Retirement Report (s)
        private string m_sRetireFrom = "";
        private string m_sRetireTo = "";
        private bool m_bCheckApp = false;
        public bool CheckApproved
        {
            get { return m_bCheckApp; }
            set { m_bCheckApp = value; }
        }
        public string RetireFrom
        {
            get { return m_sRetireFrom; }
            set { m_sRetireFrom = value; }
        }
        public string RetireTo
        {
            get { return m_sRetireTo; }
            set { m_sRetireTo = value; }
        }
        //MCR 20140103 Retirement Report (e)


        //MCR 20140709 (s)
        frmProgress m_form;
        private Thread m_objThread;
        public delegate void DifferentDelegate(object value, frmProgress.ProgressMode mode);
        public static void DoSomethingDifferent(object value, frmProgress.ProgressMode mode, DifferentDelegate threadFunction)
        {
            threadFunction(value, mode); // NOTE: invoked with a parameter
        }
        private void ThreadProcess()
        {
            using (m_form = new frmProgress())
            {

                m_form.TopMost = true;

                m_form.ShowDialog();
            }
        }
        //MCR 20140709 (e)

        private bool m_bInit = true;

        public string Data1
        {
            get { return m_sData1; }
            set { m_sData1 = value; }
        } //MCR 20140102 List Of Businesses Permit Number

        public string ReportName
        {
            get { return m_sReportName; }
            set { m_sReportName = value; }
        } //MCR 20131226 List Of Businesses Reports

        public string BrgyName
        {
            get { return m_sBrgyName; }
            set { m_sBrgyName = value; }
        } //MCR 20140102 List Of Businesses Permit Number

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

        public string OwnFirstName
        {
            get { return m_sOwnFN; }
            set { m_sOwnFN = value; }
        }

        public string OwnLastName
        {
            get { return m_sOwnLN; }
            set { m_sOwnLN = value; }
        }

        public string OwnMI
        {
            get { return m_sOwnMI; }
            set { m_sOwnMI = value; }
        }

        public string OwnAddress
        {
            get { return m_sOwnAdd; }
            set { m_sOwnAdd = value; }
        }

        public string RequestedBy
        {
            get { return m_sRequestBy; }
            set { m_sRequestBy = value; }
        }

        public string Purpose
        {
            get { return m_sPurpose; }
            set { m_sPurpose = value; } //JHB 20180511
        }

        public string Query // RMC 20120329 Modifications in Notice of violation
        {
            get { return m_sQuery; }
            set { m_sQuery = value; }
        }

        public string UnofficialBnsNm
        {
            get { return m_sUnoffBnsName; }
            set { m_sUnoffBnsName = value; }
        }

        //MCR 20170530 (s) re-assessment
        private int m_iSwitchFilter = 0;
        public int SwitchFilter
        {
            get { return m_iSwitchFilter; }
            set { m_iSwitchFilter = value; }
        }
        //MCR 20170530 (e) re-assessment

        // RMC 20171114 added capturing of certification payment (s)
        private string m_sORNo = string.Empty;
        private string m_sAmount = string.Empty;
        private string m_sORDate = string.Empty;

        public string ORNo
        {
            set { m_sORNo = value; }
        }

        public string ORAmount
        {
            set { m_sAmount= value; }
        }

        public string ORDate
        {
            set { m_sORDate = value; }
        }
        // RMC 20171114 added capturing of certification payment (e)

        public frmBussReport()
        {
            InitializeComponent();
        }

        private string m_sSeriesYr;
        public string SeriesYr
        {
            get { return m_sSeriesYr; }
            set { m_sSeriesYr = value; }
        }

        //JARS 20170823 (S)
        private string m_sDocNo;
        public string DocNo
        {
            get { return m_sDocNo; }
            set { m_sDocNo = value; }
        }

        private string m_sPageNo;
        public string PageNo
        {
            get { return m_sPageNo; }
            set { m_sPageNo = value; }
        }

        private string m_sBookNo;
        public string BookNo
        {
            get { return m_sBookNo; }
            set { m_sBookNo = value; }
        }

        private string m_sNotary;
        public string Notary
        {
            get { return m_sNotary; }
            set { m_sNotary = value; }
        }

        private string m_sDueTo;
        public string DueTo
        {
            get { return m_sDueTo; }
            set { m_sDueTo = value; }
        }

        private string m_sInspector;
        public string Inspector
        {
            get { return m_sInspector; }
            set { m_sInspector = value; }
        }

        private string m_sCertOption;
        public string CertificationOption
        {
            get { return m_sCertOption; }
            set { m_sCertOption = value; }
        }
        //JARS 20170823 (E)

        // JAV 20170914 (S)
        private string m_sMode;
        public string sMode 
        {
            get { return m_sMode; }
            set { m_sMode = value; }
        }
        string sCurrentUser = "";
        // JAV 20170914 (E)

        private string m_sYearFrom = "";
        private string m_sYearTo = "";
        private string m_sYear1 = "";
        private string m_sYear2 = "";
        private string m_sYear3 = "";
        public string Year1 //JARS 20180327
        {
            get { return m_sYear1; }
            set { m_sYear1 = value; }
        }

        public string Year2 //JARS 20180327
        {
            get { return m_sYear2; }
            set { m_sYear2 = value; }
        }

        public string Year3
        {
            get { return m_sYear3; }
            set { m_sYear3 = value; }
        }

        public string YearTo //JARS 20180326
        {
            get { return m_sYearTo; }
            set { m_sYearTo = value; }
        }

        public string YearFrom //JARS 20180326
        {
            get { return m_sYearFrom; }
            set { m_sYearFrom = value; }
        }

        private bool m_blnIsPercent;

        public bool IsPercent
        {
            get { return m_blnIsPercent; }
            set { m_blnIsPercent = value; }
        }

        private string m_sPercent;

        public string Percent
        {
            get { return m_sPercent; }
            set { m_sPercent = value; }
        }
	
	

        private void ExportFile() //MCR 20140618 (s)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.DefaultExt = "html";
                dlg.Title = "Reports";
                dlg.Filter = "HTML Files (*.html;*.htm)|*.html|Excel Files (*.xls)|*.xls";
                dlg.FilterIndex = 3;
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.axVSPrinter1.ExportFormat = VSPrinter7Lib.ExportFormatSettings.vpxPlainHTML;
                    this.axVSPrinter1.ExportFile = dlg.FileName;
                }
            }
        }

        private void frmBussReport_Load(object sender, EventArgs e)
       {
            if (m_sReportSwitch == "ListBnsGrossReceipt" || m_sReportSwitch == "ListBnsBusStatus"
                || m_sReportSwitch == "ListBnsOrgKind" || m_sReportSwitch == "ListBnsMainBusiness"
                || m_sReportSwitch == "ListBnsDist" || m_sReportSwitch == "ListBnsBrgy"
                || m_sReportSwitch == "ListOfEarlyBird" || m_sReportSwitch == "Re-assessment")
            {
                //this.axVSPrinter1.PaperHeight = 1100;
                //this.axVSPrinter1.PaperWidth = 1487;
                //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; //orLandscape 
            }
            else if (m_sReportSwitch == "ListBnsQtrDatePaid" || m_sReportSwitch == "ListBnsPrmtNum")
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; //pprFanfoldUS
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; //orLandscape
            }
            else if (m_sReportSwitch == "RetirementReport")
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS; //pprFanfoldUS
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; //orPortrait
            }
            else if (m_sReportSwitch == "ListBnsOwnBday")
            {
                //dlg.ReportName = "List of Business Owner's Birthday";
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS; //pprFanfoldUS
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; //orPortrait
            }
            else if (/*m_sReportSwitch == "Retirement" || m_sReportSwitch == "WithApplication" || m_sReportSwitch == "NoBusiness" 
                ||*/ m_sReportSwitch == "InspectionReport" || m_sReportSwitch == "ComparativeGrossRevenue") 
                //JHMN 20170104 switch of retirement certificate printing //JARS 20170915 WithBussPermit
                //JARS 20180515 SummaryBnsRoll
            {
                //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;  
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; //JARS 20170817   
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            }
            else if (m_sReportSwitch == "Renewal" || m_sReportSwitch == "WithBussPermit" || m_sReportSwitch == "CertStat"
            || m_sReportSwitch == "Retirement" || m_sReportSwitch == "WithApplication" || m_sReportSwitch == "NoBusiness"
        || m_sReportSwitch == "BussCertificate" || m_sReportSwitch == "Comparative Report" || m_sReportSwitch == "Comparative Report List By Date")   // RMC 20171121 transferred and modified Certificate of status   // RMC 20171128 adjustment in certificate of status and With Business // RMC 20180126 added comparative report as requested by Malolos BPLO Head // AFM 20200228 Comparative list by date MAO-20-12445
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            }
            else if (m_sReportSwitch == "EPSClearance" || m_sReportSwitch == "ZoningClearance" || m_sReportSwitch == "SanitaryClearance" || m_sReportSwitch == "SanitaryReport") //JHMN 20170104 switch of retirement certificate printing //AFM 20200103 added sanitary //AFM 20200130 added sanitary report
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            }
            else if (m_sReportSwitch == "Closure")
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
                //JARS 20170927
                if (MessageBox.Show("Use pre-printed form?", "Certification", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    m_bUsePreprinted = true;
                else
                    m_bUsePreprinted = false;
            }
            else if (m_sReportSwitch == "BiggestInvestment") //AFM 20200306
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            }

            else
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            }

            //if (m_sReportSwitch == "Certification")
            if (m_sReportSwitch == "Certification" || m_sReportSwitch == "Retirement" || m_sReportSwitch == "WithApplication"
                || m_sReportSwitch == "NoBusiness" || m_sReportSwitch == "Renewal" || m_sReportSwitch == "BussCertificate"
                || m_sReportSwitch == "CertStat")   // RMC 20171121 transferred and modified Certificate of status  
            {
                this.axVSPrinter1.MarginLeft = 0;   // RMC 20171114 added capturing of certification payment
                this.axVSPrinter1.MarginLeft = 1500;  // RMC 20171114 added capturing of certification payment, changed from 1000 to 1500
                this.axVSPrinter1.MarginTop = 1000;
                this.axVSPrinter1.MarginBottom = 500; //this.axVSPrinter1.MarginBottom = 2000; //MOD MCR 20143005 1 page of certifaction

                // RMC 20120220 added option for using pre-printed form in Certification (s)
                if (MessageBox.Show("Use pre-printed form?", "Certification", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    m_bUsePreprinted = true;
                else
                    m_bUsePreprinted = false;
                // RMC 20120220 added option for using pre-printed form in Certification (e)
            }
            else if (m_sReportSwitch == "Comparative Report" || m_sReportSwitch == "Comparative Report List By Date")  // RMC 20180126 added comparative report as requested by Malolos BPLO Head // AFM 20200228 Comparative list by date MAO-20-12445
            {
                this.axVSPrinter1.MarginLeft = 0;
                this.axVSPrinter1.MarginLeft = 1500;
                this.axVSPrinter1.MarginTop = 1000;
                this.axVSPrinter1.MarginBottom = 500;
            }
            else if (m_sReportSwitch == "ListEngineeringClearance" || m_sReportSwitch == "ListZoningClearance")
            {
                this.axVSPrinter1.MarginTop = 1000;
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            }
            else
            {
                if (m_sReportSwitch != "ListBnsQtrDatePaid" && m_sReportSwitch != "ListBnsPrmtNum"
                    && m_sReportSwitch != "ListBnsGrossReceipt" && m_sReportSwitch != "ListBnsBusStat"
                    && m_sReportSwitch != "ListBnsOrgKind" && m_sReportSwitch != "ListBnsMainBusiness"
                    && m_sReportSwitch != "ListBnsDist" && m_sReportSwitch != "ListBnsBrgy"
                    && m_sReportSwitch != "RetirementReport" && m_sReportSwitch != "Re-assessment")
                {
                    this.axVSPrinter1.MarginLeft = 0;   // RMC 20171114 added capturing of certification payment
                    this.axVSPrinter1.MarginLeft = 1200;
                    this.axVSPrinter1.MarginTop = 1000;
                    this.axVSPrinter1.MarginBottom = 1300; //2000
                }
                else
                    this.axVSPrinter1.MarginBottom = 1000;

                //m_bUsePreprinted = false;    // RMC 20120220 added option for using pre-printed form in Certification
            }

            //JARS 20170927 REM
            //// RMC 20120419 use pre-printed form in printing final notice (s)
            //if (m_sReportSwitch == "Closure")
            //    m_bUsePreprinted = true;
            //// RMC 20120419 use pre-printed form in printing final notice (e)

            // RMC 20171121 transferred and modified Certificate of status, put rem, certifications dont need to be exported (s)
            if (m_sReportSwitch == "Certification" || m_sReportSwitch == "Retirement" || m_sReportSwitch == "WithApplication"
                || m_sReportSwitch == "NoBusiness" || m_sReportSwitch == "Renewal" || m_sReportSwitch == "BussCertificate"
                || m_sReportSwitch == "CertStat" || m_sReportSwitch == "WithBussPermit"
                || m_sReportSwitch == "Comparative Report" || m_sReportSwitch == "ComparativeGrossRevenue")    // RMC 20171128 adjustment in certificate of status and With Business   // RMC 20180126 added comparative report as requested by Malolos BPLO Head
            { } // RMC 20171121 transferred and modified Certificate of status, put rem, certifications dont need to be exported (e)
            else
            //if (m_sReportSwitch != "BussCert" || m_sReportSwitch != "WithBussPermit") // JAV 20171010
                ExportFile(); //MCR 20140618


            //if (m_sReportSwitch != "NoBusiness" && m_sReportSwitch != "InspectionReport") //JARS 20170927 REM
            //{
            //    CreateHeader();
            //}
            //MCR 20140618 (e)
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc; //JARS BEFORE ANYTING ELSE, PLEASE PUT STARTDOC AT VERY START

            if (!m_bUsePreprinted)
                CreateHeader();

            if (m_sReportSwitch == "Renewal")
                PrintNotice();
            else if (m_sReportSwitch == "Reassess")
                PrintNoticeForReassess();
            else if (m_sReportSwitch == "Violation")
                PrintNoticeofViolation();
            else if (m_sReportSwitch == "Certification")
                PrintCertification();
            else if (m_sReportSwitch == "BussCertificate")
            {
                if (AppSettingsManager.GetConfigValue("10") == "216")   // RMC 20171128 adjustment in certificate of status and With Business
                    PrintBussCertNew();
                else
                    PrintBussCert();
            }
            else if (m_sReportSwitch == "Closure")  // RMC 20120329 Modifications in Notice of violation
                //PrintClosure();
                PrintClosure2(); //JARS 20170908
            else if (m_sReportSwitch == "Notice List")  // RMC 20120329 Modifications in Notice of violation
                PrintNoticeList();
            else if (m_sReportSwitch == "CDO Report")  // GDE 20120530
                CDOReport();
            else if (m_sReportSwitch == "BSP Report")
                BSPReport();
            //(s) MCR 20140102
            else if (m_sReportSwitch == "ListBnsOwnBday")
                ListBnsOwnBday();
            else if (m_sReportSwitch == "ListBnsQtrDatePaid")
                ListBnsQtrDatePaid();
            else if (m_sReportSwitch == "ListBnsPrmtNum")
                ListBnsPrmtNum();
            else if (m_sReportSwitch == "ListBnsGrossReceipt")
                ListBnsGrossReceipt();
            else if (m_sReportSwitch == "ListBnsBusStatus")
                ListBnsBusStatus();
            else if (m_sReportSwitch == "ListBnsOrgKind")
                ListBnsOrgKind();
            else if (m_sReportSwitch == "ListBnsMainBusiness")
                ListBnsMainBusiness();
            else if (m_sReportSwitch == "ListBnsDist")
                ListBnsDist();
            else if (m_sReportSwitch == "ListBnsBrgy")
                ListBnsBrgy();
            else if (m_sReportSwitch == "RetirementReport")
                RetirementReport();
            //(e) MCR 20140102
            else if (m_sReportSwitch == "Retirement") //JHMN 20170104 switch of retirement certificate printing
                RetCertBINAN();
            else if (m_sReportSwitch == "WithApplication")  //JHMN 20170103 temporary certificate for business application without mayors permit number
                AppCert();
            else if (m_sReportSwitch == "NoBusiness")   //JHMN 20170105 no business certificate
                NoBusCert();
            else if (m_sReportSwitch == "ListOfEarlyBird")
                ListOfEarlyBird();
            else if (m_sReportSwitch == "Re-assessment") //MCR 20170530
                ReAssessment();
            else if (m_sReportSwitch == "InspectionReport") //JARS 20170911
                InspectionReport();
            else if (m_sReportSwitch == "WithBussPermit") //JARS 20170915
                WithBussPermit();
            else if (m_sReportSwitch == "CertStat") // RMC 20171121 transferred and modified Certificate of status
                CertStatus();
            else if (m_sReportSwitch == "Comparative Report")  // RMC 20180126 added comparative report as requested by Malolos BPLO Head
                ComparativeReport();
            // ComparativeReportNew(); // RMC 20180209 modified comparative report as requested by Malolos BPLO (format proposed by Jester)
            else if (m_sReportSwitch == "ComparativeGrossRevenue")
                BussRollSummary();
            else if (m_sReportSwitch == "EPSClearance") //MCR 20170922
                EPSClearance();
            else if (m_sReportSwitch == "ListEngineeringClearance") //JARS 20190123
                ListEPS_Clearance();
            else if (m_sReportSwitch == "ZoningClearance") //JARS 20190130
                //ZoningClearance();
                ZoningClearanceV2(); //AFM 20210812
            else if (m_sReportSwitch == "ListZoningClearance")
                ListZoningCLearance();
            else if (m_sReportSwitch == "SanitaryClearance") //AFM 20200106
                SanitaryClearance();
            else if (m_sReportSwitch == "EngineeringReport") //AFM 20200121
                EngineeringReport();
            else if (m_sReportSwitch == "SanitaryReport") //AFM 20200130
                SanitaryReport();
            else if (m_sReportSwitch == "Comparative Report List By Date") //AFM 20200228
                ComparativeReportList();
            else if (m_sReportSwitch == "BiggestInvestment") //AFM 20200306
                BiggestInvestment();
            else if (m_sReportSwitch == "EPSTrail") //AFM 20210719
                EPSTrail();
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void EPSTrail()
        {
            OracleResultSet res = new OracleResultSet();
            string sFormat = string.Empty;
            string sBin = string.Empty;
            string sBnsName = string.Empty;
            string sDtTime = string.Empty;
            string sUser = string.Empty;
            string sTaxYear = string.Empty;
            double dLastAmt = 0;
            double dNewAmt = 0;
            sFormat = "^2000|^2000|^2000|^4000|^900|^1300|^1300;";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = sFormat + "DATE/TIME|NAME OF USER|BIN|BUSINESS|TAX YEAR|LAST AMOUNT|NEW AMOUNT";
            this.axVSPrinter1.Paragraph = "";
            sFormat = "^2000|^2000|^2000|^4000|^900|>1300|>1300;";
            res.Query = "select * from EPS_ASSESS_HIST where bin = '" + m_sBIN + "' order by tax_year, date_time";
            if(res.Execute())
                while (res.Read())
                {
                    sBin = res.GetString("bin");
                    sUser = res.GetString("sys_user");
                    sBnsName = AppSettingsManager.GetBnsName(sBin);
                    sTaxYear = res.GetString("tax_year");
                    sDtTime = res.GetDateTime("date_time").ToString("MM/dd/yy HH:mm:ss");
                    double.TryParse(res.GetDouble("prev_fee").ToString(), out dLastAmt);
                    double.TryParse(res.GetDouble("new_fee").ToString(), out dNewAmt);

                    this.axVSPrinter1.Table = sFormat + sDtTime + "|" + sUser + "|" + BIN + "|" + sBnsName + "|" + sTaxYear + "|" + dLastAmt + "|" + dNewAmt;
                }

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontName = "Consolas";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<5000;Printed by : " + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial";

        }

        private void BiggestInvestment()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            result.Query = m_sQuery;
            int intCount = Convert.ToInt32(m_sPercent);
            int intCountIncreament = 0;
            int iBnsCount = 0;
            string sFormat = string.Empty;

            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.FontSize = 10f;
            sFormat = ">500|<2500|^300|^7000;";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sFormat + "|" + "City/Municipality|" + ":|" + AppSettingsManager.GetConfigObject("02");
            axVSPrinter1.FontBold = false;
            axVSPrinter1.Paragraph = "";
            
            result2.Query = "SELECT COUNT(*) FROM ("; //get count
            result2.Query += m_sQuery + ")";
            int.TryParse(result2.ExecuteScalar(), out iBnsCount);

            if (m_blnIsPercent == true) //if percentage, compute percentage
            {
                float fCntTmp = float.Parse(m_sPercent);
                float fCnt = iBnsCount * fCntTmp / 100;
                m_sPercent = string.Format("{0:###}", fCnt);
                intCount = Convert.ToInt32(m_sPercent);
            }
            else
                intCount = Convert.ToInt32(m_sPercent);

            m_objThread = new Thread(ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

            //start
            iBnsCount = 1; //count starts at 1 for display
            if(result.Execute())
                while (result.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    axVSPrinter1.FontSize = 10f;
                    axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                    axVSPrinter1.Table = sFormat + iBnsCount + "|" + "Name of Business|" + ":|" + result.GetString("bns_nm") ;
                    axVSPrinter1.Table = sFormat + "|" + "Amount of Investment|" + ":|" + string.Format("{0:#,##.#0}", result.GetDouble("capital"));
                    axVSPrinter1.Paragraph = "";

                    iBnsCount++;
                    if (iBnsCount > intCount)
                        break;

                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

        }
        private void ComparativeReportList() //AFM 20200228 (s)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();
            int intcount = 0;
            int iTotal = 0;
            int intCount = 0;
            int intCountIncreament = 0;
            string sQuery = string.Empty;
            string sData = string.Empty;
            string sFormat = string.Empty;
            int iYearFrom = 0;
            int iYearTo =  0;
            int.TryParse(m_dFrom.Year.ToString(), out iYearFrom);
            int.TryParse(m_dTo.Year.ToString(), out iYearTo);
            DateTime dtTo = AppSettingsManager.GetSystemDate();
            DateTime dtFrom = AppSettingsManager.GetSystemDate();
            List<int> lstYear = new List<int>();

            //get year list
            for (int icnt = iYearFrom; icnt <= iYearTo; icnt++)
                lstYear.Add(icnt);

            axVSPrinter1.FontBold = true;
            axVSPrinter1.Paragraph = "Comparative Report List by Date";
            axVSPrinter1.FontSize = 11;
            axVSPrinter1.FontBold = false;
            axVSPrinter1.Paragraph = "Date Period: " + m_dFrom.ToShortDateString() + " - " + m_dTo.ToShortDateString();
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Paragraph = "";

            sData = "NEW|RENEWAL|RETIREMENT|DATE";
            sFormat = "^1500|^1200|^1500|^1800|^1500;";
            axVSPrinter1.Table = string.Format(sFormat + "|" + sData);
            axVSPrinter1.FontBold = false;

            //init query
            result.Query = "select DISTINCT(TRUNC(PH.DATE_POSTED)) AS DATE_POSTED, BNS.BNS_STAT, BNS.BIN,PH.PAYMENT_TERM, PH.QTR_PAID from BUSINESSES BNS ";
            result.Query += "RIGHT JOIN PAY_HIST PH ON PH.BIN = BNS.BIN ";
            result.Query += "where ((";
            for (int cnt = 0; lstYear.Count > cnt; cnt++)// loop to get range per year
            {
                dtFrom = new DateTime(lstYear[cnt], m_dFrom.Month, m_dFrom.Day);
                dtTo = new DateTime(lstYear[cnt], m_dTo.Month, m_dTo.Day);
                result.Query += "TRUNC(PH.DATE_POSTED) between '" + dtFrom.ToString("dd-MMM-yyyy") + "' AND '" + dtTo.ToString("dd-MMM-yyyy") + "') ";
                if (lstYear.Count - 1 == cnt)
                    result.Query += ")";
                else 
                    result.Query += "OR (";
            }
            result.Query += "AND PH.DATA_MODE <> 'POS' ";
            result.Query += "AND BNS.BNS_STAT IS NOT NULL ";
            result.Query += "AND PH.QTR_PAID != '2' AND PH.QTR_PAID != '3' AND PH.QTR_PAID != '4' AND PH.QTR_PAID != 'A' AND PH.QTR_PAID != 'P'"; //excluded installments, rev adjustments, and permit updates
            result.Query += "GROUP BY TRUNC(PH.DATE_POSTED),BNS.BNS_STAT,BNS.BIN, PH.PAYMENT_TERM, PH.QTR_PAID ";

            result.Query += "UNION ";

            result.Query += "select DISTINCT(TRUNC(PH.DATE_POSTED)),BNS2.BNS_STAT,BNS2.BIN, PH.PAYMENT_TERM, PH.QTR_PAID from BUSS_HIST BNS2 ";
            result.Query += "RIGHT JOIN PAY_HIST PH ON PH.BIN = BNS2.BIN ";
            result.Query += "where ((";
            for (int cnt = 0; lstYear.Count > cnt; cnt++)// loop to get range per year
            {
                dtFrom = new DateTime(lstYear[cnt], m_dFrom.Month, m_dFrom.Day);
                dtTo = new DateTime(lstYear[cnt], m_dTo.Month, m_dTo.Day);
                result.Query += "TRUNC(PH.DATE_POSTED) between '" + dtFrom.ToString("dd-MMM-yyyy") + "' AND '" + dtTo.ToString("dd-MMM-yyyy") + "') ";
                if (lstYear.Count - 1 == cnt)
                    result.Query += ")";
                else
                    result.Query += "OR (";
            }
            result.Query += " AND BNS2.bin not in(select bin from businesses where to_number(tax_year) between " + iYearFrom + " and " + iYearTo + ") ";
            result.Query += "AND PH.DATA_MODE <> 'POS' ";
            result.Query += "AND BNS2.BNS_STAT IS NOT NULL ";
            result.Query += "GROUP BY TRUNC(PH.DATE_POSTED),BNS2.BNS_STAT,BNS2.BIN, PH.PAYMENT_TERM, PH.QTR_PAID ";
            result.Query += "order by 1,2";

            sQuery = result.Query;

            pSet.Query = "select count(*) from (" + sQuery + ")";
            int.TryParse(pSet.ExecuteScalar(), out intCount);

            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

            //start of loop
            string sTmp = sFormat;
            string sDt = string.Empty;
            string sDtTmp = string.Empty;
            string sStatTmp = string.Empty;
            string sTaxYearTmp = string.Empty;
            int iNewCnt = 0;
            int iRenCnt = 0;
            int iRetCnt = 0;
            int iTotNewCnt = 0;
            int iTotRenCnt = 0;
            int iTotRetCnt = 0;
            bool isOk = false;
            string[] arrStat = new string[3];
            int iChkLast = 0;
            arrStat[0] = "NEW";
            arrStat[1] = "REN";
            arrStat[2] = "RET";
            if(result.Execute())
                while(result.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sTmp = "";
                    isOk = false;
                    sStatTmp = "";
                    iChkLast = 0;
                    iNewCnt = 0;
                    iRenCnt = 0;
                    iRetCnt = 0;
                    if (sDtTmp != result.GetDateTime("date_posted").ToShortDateString())
                    {
                        //group init query by date
                        result2.Query = "select bns_stat,TRUNC(date_posted) AS DATE_POSTED, count(bin) as bin_count from (";
                        result2.Query += sQuery + ")";
                        result2.Query += "where trunc(date_posted) = '" + result.GetDateTime("date_posted").ToString("dd-MMM-yyyy") + "' ";
                        result2.Query += "group by bns_stat,TRUNC(date_posted) ";
                        result2.Query += "order by 1";
                        if (result2.Execute())
                        {
                            while (result2.Read())
                            {
                                string sStat = result2.GetString("bns_stat");
                                string sDtYr = result2.GetDateTime("date_posted").Year.ToString();
                                int iCntAll = 0;

                                //get addl bns (s)
                                if (isOk == false)
                                {
                                    for (int cnt = 0; cnt < arrStat.Length; cnt++) //loop to count new, ren, ret
                                    {
                                        result3.Query = "SELECT AD.BNS_STAT,AD.TAX_YEAR,COUNT(AD.BIN) AS BIN_COUNT FROM ADDL_BNS AD ";
                                        result3.Query += "WHERE AD.TAX_YEAR = '" + sDtYr + "' ";
                                        result3.Query += "AND AD.BNS_STAT = '" + arrStat[cnt] + "' ";
                                        result3.Query += "AND AD.BIN IN (SELECT BIN FROM PAY_HIST WHERE AD.BIN = PAY_HIST.BIN AND TO_DATE(DATE_POSTED) = '" + result.GetDateTime("date_posted").ToString("dd-MMM-yyyy") + "') ";
                                        result3.Query += "GROUP BY AD.BNS_STAT,AD.TAX_YEAR ";
                                        result3.Query += "ORDER BY 2,1";
                                        if (result3.Execute())
                                            if (result3.Read())
                                            {
                                                if (result3.GetString("bns_stat") == "NEW")
                                                {
                                                    iNewCnt += result3.GetInt("bin_count");
                                                    iTotNewCnt += result3.GetInt("bin_count");
                                                }
                                                else if (result3.GetString("bns_stat") == "REN")
                                                {
                                                    iRenCnt += result3.GetInt("bin_count");
                                                    iTotRenCnt += result3.GetInt("bin_count");
                                                }
                                                else if (result3.GetString("bns_stat") == "RET")
                                                {
                                                    iRetCnt += result3.GetInt("bin_count");
                                                    iTotRetCnt += result3.GetInt("bin_count");
                                                }
                                            }
                                    }
                                    if (iNewCnt == 0 && iRenCnt == 0 && iRetCnt == 0)
                                        isOk = false;
                                    else
                                        isOk = true;
                                }
                                //get addl bns (e)
                                //conditions set below based on order of business status
                                if (sStat == "NEW")
                                {
                                    iNewCnt += result2.GetInt("bin_count");
                                    sTmp += string.Format("{0:#,##0}", iNewCnt) + "|";
                                    iTotNewCnt += result2.GetInt("bin_count");
                                }
                                else if (sStat == "REN" && sStatTmp == "NEW")
                                {
                                    iRenCnt += result2.GetInt("bin_count");
                                    sTmp += string.Format("{0:#,##0}", iRenCnt) + "|";
                                    iTotRenCnt += result2.GetInt("bin_count");
                                }
                                else if (sStat == "RET" && sStatTmp == "REN")
                                {
                                    iRetCnt += result2.GetInt("bin_count");
                                    sTmp += string.Format("{0:#,##0}", iRetCnt) + "|";
                                    iTotRetCnt += result2.GetInt("bin_count");
                                }
                                else
                                {
                                    if (sStat == "REN")
                                    {
                                        if (iNewCnt == 0)
                                            sTmp += "0|"; //applies zero if business status has no count
                                        else
                                            sTmp += string.Format("{0:#,##0}", iNewCnt) + "|";
                                        iRenCnt += result2.GetInt("bin_count");
                                        iCntAll = iRenCnt;
                                    }
                                    else if (sStat == "RET")
                                    {
                                        if(iNewCnt == 0 && iRenCnt == 0)
                                            sTmp += "0|0|"; //applies zero if business status has no count

                                        iRetCnt += result2.GetInt("bin_count");
                                        iCntAll = iRetCnt;
                                    }

                                    sTmp += string.Format("{0:#,##0}", iCntAll) + "|";

                                    if (sStat == "NEW")
                                        iTotNewCnt += result2.GetInt("bin_count");
                                    if (sStat == "REN")
                                        iTotRenCnt += result2.GetInt("bin_count");
                                    if (sStat == "RET")
                                        iTotRetCnt += result2.GetInt("bin_count");
                                }
                                

                                sStatTmp = result2.GetString("bns_stat");
                                sDt = result2.GetDateTime("date_posted").ToShortDateString();
                                sTaxYearTmp = result2.GetDateTime("date_posted").Year.ToString();
                            }
                            iChkLast = sTmp.Split('|').Length - 1;
                            if (iChkLast != 3 && isOk == false) //applies 0 if no record so it displays properly
                            {
                                if (sStatTmp == "NEW")
                                    sTmp += "0|0|";
                                else
                                    sTmp += "0|";
                            }
                            else if (iChkLast != 3 && isOk == true)
                                sTmp += iRetCnt + "|";
                            axVSPrinter1.FontBold = false;
                            sTmp += sDt;
                            axVSPrinter1.Table = sFormat + "|" + sTmp;
                            sDtTmp = result.GetDateTime("DATE_POSTED").ToShortDateString();
                        }
                    }
                    else
                    {
                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                        continue;
                    }

                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            result.Close();
            result2.Close();
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sFormat + "GRAND TOTAL|" + string.Format("{0:#,##0}", iTotNewCnt) + "|" + string.Format("{0:#,##0}", iTotRenCnt) + "|" + string.Format("{0:#,##0}", iTotRetCnt) + "|";
            axVSPrinter1.FontBold = false;
            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

        }
        //AFM 20200228 (e)

        private void SanitaryReport()  //AFM 20200130
        {
            string sModCode = "ARSCR";
            string sBIN = string.Empty;
            string sBnsName = string.Empty;
            string sBnsAdd = string.Empty;
            string sOwner = string.Empty;
            string sOwnTmp = string.Empty;
            string sDtIssued = string.Empty;
            string sDtFrom = m_dFrom.ToString("dd/MMM/yyyy");
            string sDtTo = m_dTo.ToString("dd/MMM/yyyy");
            int iTotal = 0;
            int intCount = 0;
            int intCountIncreament = 0;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();

            //AFM 20200230 progress bar
            Thread.Sleep(500);
            pSet.Query = "select count(*) from (select distinct(bin), max(dt_issued) as DATEISSUED from sanitary where to_date(dt_issued) BETWEEN '" + sDtFrom + "' and '" + sDtTo + "' group by bin)";
            int.TryParse(pSet.ExecuteScalar(), out intCount);

            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

            result.Query = @"select distinct(bin), max(dt_issued) as DATEISSUED from sanitary where to_date(dt_issued) BETWEEN '" + sDtFrom + "' and '" + sDtTo + "' group by bin order by DATEISSUED";
            if(result.Execute())
                while(result.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;
                    sBIN = result.GetString("bin");
                    sBnsName = AppSettingsManager.GetBnsName(sBIN);
                    sOwnTmp = AppSettingsManager.GetOwnCode(sBIN);
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnTmp);
                    sBnsAdd = AppSettingsManager.GetBnsAdd(sBIN, "");
                    sDtIssued = result.GetDateTime("DATEISSUED").ToShortDateString();

                    this.axVSPrinter1.Table = ("<2000|<3000|<2500|<3000|<1000;" + sBIN + "|" + sBnsName + "|" + sOwner + "|" + sBnsAdd + "|" + sDtIssued);
                    iTotal++;

                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            result.Close();
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontName = "Consolas";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<2000;Total : " + iTotal.ToString("#,##0");
            this.axVSPrinter1.Table = "<5000;Printed by : " + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial";

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            //trail
            result.Query = "select * from TRAIL_TABLE WHERE usr_rights = 'ARSCR'";
            if (result.Execute())
            {
                if (!result.Read())
                {
                    result.Close();
                    result.Query = "insert into trail_table values (";
                    result.Query += "'ARSCR','ASS-REPORTS-SANITARY CLEARANCE-REPORTS')";
                    if (result.ExecuteNonQuery() == 0)
                    { }
                }

            }
            result.Close();

            if (AuditTrail.InsertTrail(sModCode, "trail table", StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName)) == 0)
            { }
        }

        private void EngineeringReport() //AFM 20200121
        {
            string sModCode = "ARECR";
            string sBIN = string.Empty;
            string sBnsName = string.Empty;
            string sCtrlNo = string.Empty;
            string sBnsStat = string.Empty;
            string sOwner = string.Empty;
            string sDtIssued = string.Empty;
            string sTaxYear = string.Empty;
            string sAmt = string.Empty;
            string sORNO = string.Empty;
            string sOwnTmp = string.Empty;
            string sRemarks = string.Empty;
            string sDtFrom = m_dFrom.ToString("dd/MMM/yyyy");
            string sDtTo = m_dTo.ToString("dd/MMM/yyyy");
            int iTotal = 0;
            int intCount = 0;
            int intCountIncreament = 0;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

             m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();

            //AFM 20200230 progress bar
            Thread.Sleep(500);
            pSet.Query = @"select count(*) from (select distinct(EP.TAX_YEAR), EP.BIN, EP.REMARKS, EC.DT_ISSUED, EC.EPS_CONTROL_SERIES 
            from EPS_ASSESS_APP EP, EPS_ASSESSMENT EA, EPS_CONTROL_TBL EC
            WHERE EA.BIN = EP.BIN AND EA.BIN = EC.BIN
            AND (EC.dt_issued BETWEEN '" + sDtFrom + "' AND '" + sDtTo + "') GROUP BY EP.TAX_YEAR, EP.BIN, EP.REMARKS, EC.DT_ISSUED,EC. EPS_CONTROL_SERIES order by EC.dt_issued)";
            int.TryParse(pSet.ExecuteScalar(), out intCount);

            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            
            result.Query = @"select distinct(EP.TAX_YEAR), EP.BIN, EP.REMARKS, EC.DT_ISSUED, EC.EPS_CONTROL_SERIES 
            from EPS_ASSESS_APP EP, EPS_ASSESSMENT EA, EPS_CONTROL_TBL EC
            WHERE EA.BIN = EP.BIN AND EA.BIN = EC.BIN
            AND (EC.dt_issued BETWEEN '" + sDtFrom + "' AND '" + sDtTo + "') GROUP BY EP.TAX_YEAR, EP.BIN, EP.REMARKS, EC.DT_ISSUED,EC. EPS_CONTROL_SERIES order by EC.dt_issued";


            if (result.Execute())
                while (result.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sBIN = result.GetString("BIN");
                    sBnsName = AppSettingsManager.GetBnsName(sBIN);
                    sCtrlNo = result.GetString("EPS_CONTROL_SERIES");
                    sBnsStat = AppSettingsManager.GetBnsStat(sBIN);
                    sOwnTmp = AppSettingsManager.GetOwnCode(sBIN);
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnTmp);
                    sDtIssued = result.GetDateTime("DT_ISSUED").ToShortDateString();
                    sTaxYear = result.GetString("TAX_YEAR");
                    //sAmt = result.GetDouble("ANNUAL_FEE").ToString("#,##0.00");
                    sRemarks = result.GetString("REMARKS");

                    result2.Query = @"select * from pay_hist where bin = '"+ sBIN +"' and tax_year = '"+ sTaxYear +"'";
                    if(result2.Execute())
                        while (result2.Read())
                        {
                            result3.Query = @"select * from or_table where or_no = '" + result2.GetString("OR_NO") + "' order by fees_code desc"; //order by desc to read annual insp fast
                            if(result3.Execute())
                                while (result3.Read())
                                {
                                    if (result3.GetString("FEES_CODE") == "25") //change annual inspection fees code depending on lgu fees code
                                    {
                                        sORNO = result3.GetString("OR_NO");
                                        sAmt = result3.GetDouble("FEES_DUE").ToString("#,##0.00");
                                        break;// decrease load time
                                    }
                                }
                        }


                    this.axVSPrinter1.Table = ("^2000|^3000|^1000|^1000|^2500|^1000|^700|^1000|^1000|^3000;" + sBIN + "|" + sBnsName + "|" + sCtrlNo + "|" + sBnsStat + "|" + sOwner + "|" + sDtIssued + "|" + sTaxYear + "|" + sORNO + "|" + sAmt + "|" + sRemarks);
                    iTotal++;

                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            result.Close();
            result2.Close();
            result3.Close();
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontName = "Consolas";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<2000;Total : " + iTotal.ToString("#,##0");
            this.axVSPrinter1.Table = "<5000;Printed by : " + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial";

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            //trail
            result.Query = "select * from TRAIL_TABLE WHERE usr_rights = 'ARECR'";
            if (result.Execute())
            {
                if (!result.Read())
                {
                    result.Close();
                    result.Query = "insert into trail_table values (";
                    result.Query += "'ARECR','ASS-REPORTS-ENGINEERING CLEARANCE-REPORTS')";
                    if (result.ExecuteNonQuery() == 0)
                    { }
                }
            
            }
            result.Close();

            if (AuditTrail.InsertTrail(sModCode, "trail table", StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName)) == 0)
            { }

        }

        private void SanitaryClearance() //AFM 20200106 (s)
        {
            object y;
            this.axVSPrinter1.MarginLeft = 3000;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 14f;
            this.axVSPrinter1.FontBold = false;
            y = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<10000;Issued To: ____________________________________________ ";
            this.axVSPrinter1.Table = "^6500|<10000;|Name of Establishment ";
            this.axVSPrinter1.CurrentY = y;
            this.axVSPrinter1.FontSize = 12f;
            this.axVSPrinter1.Table = "^10000;" + m_sBussName;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = 14f;
            y = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<10000;_____________________________________________________ ";
            this.axVSPrinter1.Table = "^7000|<10000;|Owner / Operator";
            this.axVSPrinter1.CurrentY = y;
            this.axVSPrinter1.FontSize = 12f;
            this.axVSPrinter1.Table = "^10000;" + m_sOwnName;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = 14f;
            y = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<10000;Address ______________________________________________ ";
            this.axVSPrinter1.CurrentY = y;
            this.axVSPrinter1.FontSize = 10f;
            this.axVSPrinter1.Table = "^10000;" + m_sBussAdd;
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = 14f;
            this.axVSPrinter1.Paragraph = "";
            y = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<10000;Sanitary Permit No._________ Date Issued ____________, 20___ ";
            this.axVSPrinter1.CurrentY = y;
            this.axVSPrinter1.FontSize = 12f;
            m_sDtYear = m_sDtYear.Substring(m_sDtYear.Length - 2); // get last two digit of year
            this.axVSPrinter1.Table = ">5000;" + m_sDtMonth;
            this.axVSPrinter1.CurrentY = y;
            this.axVSPrinter1.Table = ">7000;" + m_sDtYear;
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = 14f;
            this.axVSPrinter1.Paragraph = "";
            y = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<10000;Date of Expiration December 31, 20___.";
            this.axVSPrinter1.CurrentY = y;
            this.axVSPrinter1.FontSize = 12f;
            m_sYrExpiration = m_sYrExpiration.Substring(m_sYrExpiration.Length - 2); // get last two digit of year
            this.axVSPrinter1.Table = "<700;" + m_sYrExpiration;
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = 14f;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<10000;This permit is not transferable and will be revoked for violation of the ";
            this.axVSPrinter1.Table = "<10000;Sanitary Rules, Laws of Regulation of P.D.522 & P.D.856 and";
            this.axVSPrinter1.Table = "<10000;Pertinent City ordinance"; 
            this.axVSPrinter1.Paragraph = "";

            if (Signatory.Trim() != "") //AFM 20220104 requested by Malolos - show recommending approval for NEW and 13 sectors only
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.sanitary_rec_approval_e_sig, "1.9in", "6.7in", "80%", "80%", 10, false);

                this.axVSPrinter1.FontSize = 14f;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = "<10000;Recommending Approval";
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.FontSize = 14f;
                this.axVSPrinter1.Paragraph = "";
                y = this.axVSPrinter1.CurrentY;
                this.axVSPrinter1.Table = "^6300|>5000;_____________________________";
                //this.axVSPrinter1.Table = "^2300|^6300;|" + m_sSigposition;
                this.axVSPrinter1.Table = "^6300|>5000;" + m_sSigposition + "|";
                this.axVSPrinter1.CurrentY = y; ;
                //this.axVSPrinter1.Table = "^2300|^10000;|" + Signatory;
                this.axVSPrinter1.Table = "^10000|>5000;" + Signatory + "|";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
            }
            else
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
            }

            this.axVSPrinter1.DrawPicture(Properties.Resources.sanitary_clearance_e_sig, "4.9in", "8in", "100%", "100%", 10, false); //AFM 20220104 requested by Malolos
            this.axVSPrinter1.FontSize = 14f;
            this.axVSPrinter1.Table = "<4000;Approved";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = 14f;
            this.axVSPrinter1.Table = ">7000;_____________________________";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ">6000;" + AppSettingsManager.GetConfigValue("72");
            this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.Table = ">5800;" + AppSettingsManager.GetConfigValueRemarks("72");
            this.axVSPrinter1.Table = ">4800; City Health Officer"; //AFM 20200121 imitate exact casing of sanitary clearance
            this.axVSPrinter1.Paragraph = "";
        }
        //AFM 20200106 (e)

        private void EPSClearance() //MCR 20170922 
        {
            object y;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<3000|<7000;CONTROL NUMBER: |" + m_sControlNo;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = "<3000|<7000;BIN: |" + m_sBIN;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            if (m_sApplicantStatus == "REN")
                this.axVSPrinter1.Table = "<3000|<7000;APPLICANT STATUS: | RENEWAL";
            else if (m_sApplicantStatus == "RET")
                this.axVSPrinter1.Table = "<3000|<7000;APPLICANT STATUS: | RETIRED";
            else
                this.axVSPrinter1.Table = "<3000|<7000;APPLICANT STATUS: |" + m_sApplicantStatus;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            y = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<3000|<7000;BUSINESS NAME: | ";
            this.axVSPrinter1.CurrentY = y;
            this.axVSPrinter1.FontSize = 15f;
            this.axVSPrinter1.Table = "<3000|<7000; |" + m_sBussName;
            this.axVSPrinter1.FontSize = 9.75f;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            y = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<3000|<7000;OWNER NAME: | ";
            this.axVSPrinter1.CurrentY = y;
            //this.axVSPrinter1.FontSize = 15f;
            this.axVSPrinter1.Table = "<3000|<7000; |" + m_sOwnName;
            this.axVSPrinter1.FontSize = 9.75f;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select bns_desc from bns_table where bns_code in (select bns_code from businesses where bin = '" + m_sBIN + @"' union all
select bns_code_main From addl_bns where bin = '" + m_sBIN + "') and fees_code = 'B'";
            String sBnsLine = "";
            if (pSet.Execute())
                while (pSet.Read())
                    sBnsLine += pSet.GetString(0) + ", ";
            pSet.Close();

            sBnsLine = sBnsLine.Remove(sBnsLine.Length - 2, 2);

            this.axVSPrinter1.Table = "<3000|<7000;BUSINESS LINE: |" + sBnsLine;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<3000|<7000;BUSINESS ADDRESS: |" + AppSettingsManager.GetBnsAddress(m_sBIN);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<3000|<7000;DATE ISSUED: |" + m_sDateIssued;
            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.MarginLeft = 100;
            this.axVSPrinter1.Table = "<6000;" + "Noted by:";
            this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 15f;
            this.axVSPrinter1.Table = "^12000;" + AppSettingsManager.GetConfigValue("69");
            this.axVSPrinter1.FontSize = 9.75f;
            this.axVSPrinter1.Table = "^12000;" + AppSettingsManager.GetConfigValueRemarks("69");
            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = ">10000|<1500;OR NO: |" + m_sORNo;
            this.axVSPrinter1.Table = ">10000|<1500;OR DATE: |" + m_sORDate;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 15f;
            this.axVSPrinter1.MarginLeft = 1450;
            this.axVSPrinter1.Table = "^10000;(THIS MUST BE DISPLAYED WITHIN PUBLIC VIEW)";
            this.axVSPrinter1.FontSize = 9.75f;
            this.axVSPrinter1.MarginLeft = 500;

            //AFM 20200102
            OracleResultSet result = new OracleResultSet();
            DateTime dt = DateTime.Now;
            string sYear = dt.Year.ToString();
            result.Query = "select distinct(remarks) from eps_assess_app_remarks where bin = '" + m_sBIN + "' and tax_year = '" + sYear + "' ";
            if(result.Execute())
                if(result.Read())
                {
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.MarginLeft = 500;
                    this.axVSPrinter1.Table = "<10000;REMARKS: " + result.GetString(0);
                    this.axVSPrinter1.FontSize = 9.75f;
                    this.axVSPrinter1.MarginLeft = 500;
                }
        }
        
        private void ListEPS_Clearance()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBin = "", sBnsNm = "", sBnsAddress = "", sControlSeries = "", sDateIssued = "";
            string sDateFrom = "", sDateTo = "", sQuery = "";
            int iTotal = 0;

            sDateFrom = m_dFrom.ToString("dd/MM/yyyy");
            sDateTo = m_dTo.ToString("dd/MM/yyyy");


            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;

            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();

            Thread.Sleep(500);
            pSet.Query = "select count(*) from (select a.bin,b.bns_nm,eps_control_series,dt_issued from eps_control_tbl a, businesses b where a.bin = b.bin and ";
            pSet.Query += "dt_issued between to_date('" + sDateFrom + "', 'dd/MM/yyyy') and to_date('" + sDateTo + "', 'dd/MM/yyyy') order by dt_issued)";

            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out intCount);

            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion


            //pSet.Query = "select a.bin,b.bns_nm,eps_control_series,dt_issued from eps_control_tbl a, businesses b where a.bin = b.bin and ";
            //pSet.Query += "dt_issued between to_date('"+ sDateFrom +"', 'dd/MM/yyyy') and to_date('"+ sDateTo +"', 'dd/MM/yyyy') order by dt_issued"
            //JARS 20190218 revise query
            pSet.Query = "select a.bin,b.bns_nm,eps_control_series,dt_issued from eps_control_tbl a, businesses b ";
            pSet.Query += "where a.bin = b.bin and to_char(dt_issued, 'MM/dd/yyyy') between '"+ sDateFrom +"'";
            pSet.Query += "and '"+sDateTo+"'  and to_char(dt_issued, 'yyyy') = '"+ m_dFrom.ToString("yyyy") +"' order by dt_issued desc";

            if(pSet.Execute())
            {
                while(pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sBin = pSet.GetString("bin");
                    sBnsNm = pSet.GetString("bns_nm");
                    sBnsAddress = AppSettingsManager.GetBnsAddress(sBin);
                    sControlSeries = pSet.GetString("eps_control_series");
                    sDateIssued = pSet.GetDateTime("dt_issued").ToString("MMMM dd, yyyy");

                    this.axVSPrinter1.Table = "^2000|<3000|<3000|>1500|>1500;" + sBin + "|" + sBnsNm + "|" + sBnsAddress + "|" + sControlSeries + "|" + sDateIssued;
                    iTotal++;

                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            pSet.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontName = "Consolas";
            this.axVSPrinter1.FontSize = 15;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ">5000;Total Number of Engineering Clearance: " + iTotal.ToString("#,##0");
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial";

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);
        }

        private void ZoningClearanceV2() //AFM 20210812 MAO-21-15525 New format for zoning clearance
        {
            OracleResultSet res = new OracleResultSet();
            string sPosition = string.Empty;
            long y1 = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            long y2;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.Table = string.Format("<9000;" + AppSettingsManager.GetSystemDate().ToString("MMM dd, yyyy") + "");
            this.axVSPrinter1.CurrentY = y1 + 100;
            this.axVSPrinter1.Table = string.Format("<10000;___________________");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("<10000;To Whom It May Concern:");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            y1 = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = string.Format("<10000;This Zoning Clearance is granted to __________________________________________");
            this.axVSPrinter1.CurrentY = y1;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("<3500|^7000;|{0}", m_sBussName);
            this.axVSPrinter1.FontBold = false;

            y1 = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            string sBnsType = AppSettingsManager.GetBnsDesc(AppSettingsManager.GetBnsCodeMain(m_sBIN)).ToString();
            this.axVSPrinter1.Table = string.Format("<10000;with    ______________________________________________");
            this.axVSPrinter1.CurrentY = y1;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("<100|<8000;|{0}", sBnsType);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = y1;
            this.axVSPrinter1.Table = string.Format("<14000|<10000;| as nature of business");

            y1 = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = string.Format("<10000;located at    _____________________________________________________________");
            this.axVSPrinter1.CurrentY = y1;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("<1100|^8000;|{0}", AppSettingsManager.GetBnsAddress(m_sBIN));
            this.axVSPrinter1.FontBold = false;

            y1 = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = string.Format("<10000;in the name of    _______________________________________");
            this.axVSPrinter1.CurrentY = y1;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("<5000;{0}", m_sOwnName);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = y1;
            this.axVSPrinter1.Table = string.Format("<14000|<10000;| as it conforms with the");
            this.axVSPrinter1.Table = string.Format("<10000;Zoning Ordinance (Ordinance No. 6-97) of the City of Malolos.");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("<10000;Evaluated by:");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            axVSPrinter1.DrawPicture(Properties.Resources.grace_cabasal_sig, "1.2in", "6.72in", "16%", "16%", 10, false); //evaluated by  //AFM 20210916 MAO-21-15525 png received 9/16/21
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)13.0;
            this.axVSPrinter1.Table = string.Format("<10000;{0}", m_sSignatory);
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.FontBold = false;

            res.Query = "select sig_position from signatory where id = '" + m_iId + "'"; // signatory 1
            if (res.Execute())
            {
                if (res.Read())
                {
                    sPosition = res.GetString("sig_position");
                }
            }
            res.Close();
            this.axVSPrinter1.Table = string.Format("<10000;{0}", sPosition);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("<10000;Approved by:");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            axVSPrinter1.DrawPicture(Properties.Resources.CPDO_engr_benjamin_sig, "1.1in", "8.12in", "16%", "16%", 10, false); //approved by  //AFM 20210916 MAO-21-15525 png received 9/16/21
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)13.0;
            this.axVSPrinter1.Table = string.Format("<10000;{0}", m_sSignatory2);
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.FontBold = false;

            res.Query = "select sig_position from signatory where id = '" + m_iId2 + "'"; // signatory 1
            if (res.Execute())
            {
                if (res.Read())
                {
                    sPosition = res.GetString("sig_position");
                }
            }
            res.Close();
            this.axVSPrinter1.Table = string.Format("<10000;{0}", sPosition);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("<10000;Control #  {0}", m_sControlNo);            



        }

        private void ZoningClearance() //JARS 20190130
        {
            object lngY;
            object lngY2;
            OracleResultSet pSet = new OracleResultSet();
            string sData = "", sTick = "•", sBusinessArea = "", sPosition = "";

            pSet.Query = "select * from other_info where bin = '"+BIN+"' and default_code = '0002' and tax_year = '"+AppSettingsManager.GetConfigObject("12")+"'";
            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    sBusinessArea = pSet.GetDouble("data").ToString("#,##0.00");
                }
            }
            pSet.Close();

            this.axVSPrinter1.MarginBottom = 200;

            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.FontName = "Times New Roman";

            this.axVSPrinter1.FontBold = false;

            lngY = Convert.ToDouble(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "<1800|^2000|<1800|^2000|<1800|^1600;BIN:|||||";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<1800|^2000|<1800|^2000|<1800|^1600;CONTROL NO.:||DATE ISSUED:||VALID UNTIL:|";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<2000|^9000;BUSINESS NAME:|";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<3000|^8000;TYPE OF ESTABLISHMENT:|";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<2200|^8800;BUSINESS ADDRESS:|";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<1200|^9800;OWNER:|";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<2200|^8800;OWNER ADDRESS:|";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<2700|^5000|<2300|^1000;ZONE CLASSIFICATION:||LOT AREA IN (SQM):|";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<3000|^8000;EVALUATION OF FACTS:|";
            lngY2 = Convert.ToDouble(axVSPrinter1.CurrentY);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = Convert.ToDouble(lngY);
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.Table = "<1800|^2500|<1800|^2000|<1800|^1600;|"+ BIN +"||||";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<1800|<2000|<1800|<2000|<1800|<1600;|" + m_sControlNo + "||" + m_sDateIssued + "||12/31/" + AppSettingsManager.GetConfigObject("12");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<2000|<9000;|" + m_sBussName;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<3000|<8000;|" + AppSettingsManager.GetBnsDesc(AppSettingsManager.GetBnsCodeMain(BIN));
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<2200|<8800;|" + AppSettingsManager.GetBnsAddress(BIN);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<1200|<9800;|" + m_sOwnName;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<2200|<8800;|" + AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(BIN));
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<2700|<5000|<2300|<1000;|"+ m_sZoneClass +"||" + sBusinessArea;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<3000|<8000;|BUSINESS ACTIVITY IS PERMISSIBLE IN THE ZONE";
            this.axVSPrinter1.FontUnderline = false;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.MarginLeft = 1000;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<10000;CONDITIONS:";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.FontName = "Times New Roman";
            this.axVSPrinter1.FontSize = 10;

            sData = "This certification must not be construed as a Locational Clearance/Certificate of Zoning Conformance, Subdivision Approval, ";
            sData += "Preliminary Approval and Locational Clearance (PALC), Development Permit or Certificate of Locational Viability;";

            this.axVSPrinter1.Table = "^500|<10000;"+ sTick +"|" + sData;

            sData = "All Conditions stipulated herein form part of this clearance and are subject to monitoring;";

            this.axVSPrinter1.Table = "^500|<10000;" + sTick + "|" + sData;

            sData = "No Activity other than applied for shall be conducted within the project site;";

            this.axVSPrinter1.Table = "^500|<10000;" + sTick + "|" + sData;

            sData = "No major expansion, alteration and/or improvement shall be introduced without prior clearance from this Office;";

            this.axVSPrinter1.Table = "^500|<10000;" + sTick + "|" + sData;

            sData = "Any misrepresentation, false statements or allegations material to the issuance of this decision shall be sufficient cause of its ";
            sData += "revocation";

            this.axVSPrinter1.Table = "^500|<10000;" + sTick + "|" + sData;

            sData = "This certification is valid for a period of _________________ year/s from the date within 3mos. (3) prior to its expiration.";

            this.axVSPrinter1.Table = "^500|<10000;" + sTick + "|" + sData;

            sData = "That this office reserves the right to inspect and review said project operation and should there be any violation found, to institute ";
            sData += "cancellation proceedings.";

            this.axVSPrinter1.Table = "^500|<10000;" + sTick + "|" + sData;

            // AFM 20191227 MAO-19-11740 removed
            //sData = "That the pertinent provisions of Bi񡮠City Comprehensive Zoning Ordinance relative to performance standards shall be complied with.";

            //this.axVSPrinter1.Table = "^500|<10000;" + sTick + "|" + sData;

            sData = "That no advertising and business sign to be displayed or put for public views shall be extended beyond the property line of the proponent.";

            this.axVSPrinter1.Table = "^500|<10000;" + sTick + "|" + sData;


            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
            this.axVSPrinter1.Table = "^5000; ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^11000;" + m_sSignatory;

            pSet.Query = "select sig_position from signatory where id = '" + m_iId + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sPosition = pSet.GetString("sig_position");
                }
            }
            pSet.Close();
            this.axVSPrinter1.Table = "^5000;" + sPosition;

            //this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetConfigObject("74");
            //this.axVSPrinter1.FontSize = 10;
            //this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetConfigValueRemarks("74");

            //this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.Table = "^11000;(This must be displayed within public view)";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.FontName = "Arial Narrow";
            // AFM 20191227 MAO-19-11740 removed
            //this.axVSPrinter1.Table = "^11000;City Planning and Development Office, 3rd floor Bi񡮠City Government";
            //this.axVSPrinter1.Table = "^11000;Brgy. Zapote, Bi񡮠City";
            //this.axVSPrinter1.Table = "^11000;Telephone # 049 5135018, Telefax 049 5135019";
            axVSPrinter1.Table = ("^11000;Prepared by : " + AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode));

        }

        private void ListZoningCLearance() //JARS 20190201
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBin = "", sBnsNm = "", sBnsAddress = "", sControlSeries = "", sDateIssued = "";
            string sDateFrom = "", sDateTo = "", sQuery = "";
            int iTotal = 0;

            sDateFrom = m_dFrom.ToString("dd/MM/yyyy");
            sDateTo = m_dTo.ToString("dd/MM/yyyy");


            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;

            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();

            Thread.Sleep(500);
            pSet.Query = "select count(*) from (select a.bin,b.bns_nm,a.zon_control_series,dt_issued from zoning_control_tbl a, businesses b where a.bin = b.bin and ";
            pSet.Query += "dt_issued between to_date('" + sDateFrom + "', 'dd/MM/yyyy') and to_date('" + sDateTo + "', 'dd/MM/yyyy') order by dt_issued)";

            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out intCount);

            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion


            pSet.Query = "select a.bin,b.bns_nm,zon_control_series,dt_issued from zoning_control_tbl a, businesses b where a.bin = b.bin and ";
            pSet.Query += "to_char(dt_issued, 'dd/MM/yyyy') between '" + sDateFrom + "' and '" + sDateTo + "' order by dt_issued";

            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sBin = pSet.GetString("bin");
                    sBnsNm = pSet.GetString("bns_nm");
                    sBnsAddress = AppSettingsManager.GetBnsAddress(sBin);
                    sControlSeries = pSet.GetString("zon_control_series");
                    sDateIssued = pSet.GetDateTime("dt_issued").ToString("MMMM dd, yyyy");

                    this.axVSPrinter1.Table = "^2000|<3000|<3000|>1500|>1500;" + sBin + "|" + sBnsNm + "|" + sBnsAddress + "|" + sControlSeries + "|" + sDateIssued;
                    iTotal++;

                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            pSet.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontName = "Consolas";
            this.axVSPrinter1.FontSize = 15;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ">7000;Total Number of Zoning Clearance: " + iTotal.ToString("#,##0");
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial";

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);
        }

        private void ReAssessment() //MCR 20170530
        {
            String sQuery = "", sTotalMain = "", sBin = "", sTaxYear = "", BnsName = "", sOwnCode = "";
            int iTotalMain = 0;

            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            if (m_iSwitchFilter == 1) //1 for brgy
            {
                String sBrgy = m_sBrgyName;
                if (sBrgy == "ALL")
                    sBrgy = "%%";
                else
                    sBrgy = StringUtilities.HandleApostrophe(m_sBrgyName) + "%";

                sQuery = @"select * from (
select bin,tax_year,bns_brgy,bns_nm,own_code from businesses where bns_stat = 'REN' and bns_brgy like '" + sBrgy + @"'
union all 
select bin,tax_year,bns_brgy,bns_nm,own_code from business_que where bns_stat = 'REN' and bns_brgy like '" + sBrgy + @"'
union all 
select bin,tax_year,bns_brgy,bns_nm,own_code from buss_hist where bns_stat = 'REN' and bns_brgy like '" + sBrgy + @"') a ";
                sQuery += "inner join REASS_WATCH_LIST rwl on rwl.bin = a.bin and rwl.tax_year = a. tax_year ";
                if (m_sTaxYear != "")
                    sQuery += "where a.tax_year = '" + StringUtilities.HandleApostrophe(m_sTaxYear) + "'";
            }
            else //2 for bin
            {
                sQuery = @"select * from (
select bin,tax_year,bns_brgy,bns_nm,own_code from businesses where bns_stat = 'REN'
union all 
select bin,tax_year,bns_brgy,bns_nm,own_code from business_que where bns_stat = 'REN'
union all 
select bin,tax_year,bns_brgy,bns_nm,own_code from buss_hist where bns_stat = 'REN') a ";
                sQuery += "inner join REASS_WATCH_LIST rwl on rwl.bin = a.bin and rwl.tax_year = a. tax_year ";
                if (m_sTaxYear != "")
                    sQuery += "  where a.bin = '" + m_sBIN + "' and a.tax_year = '" + StringUtilities.HandleApostrophe(m_sTaxYear) + "'";
            }

            sQuery += " order by a.bin,a.tax_year";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sBin = pSet.GetString(0);
                    sTaxYear = pSet.GetString(1);
                    BnsName = StringUtilities.RemoveApostrophe(pSet.GetString(3));
                    sOwnCode = pSet.GetString(4);

                    iTotalMain++;
                    this.axVSPrinter1.Table = ("<1850|<3900|<3900|<3900|<3900|>1050|^1200;" + sBin + "|" + BnsName + "|" + AppSettingsManager.GetBnsOwner(sOwnCode) + "|" + AppSettingsManager.GetBnsAdd(sBin, "") + "|" + AppSettingsManager.GetBnsOwnAdd(sOwnCode) + "|" + string.Format("{0:#,##0.#0}", AppSettingsManager.GetCompoundedGross(sBin, sTaxYear)) + "|" + sTaxYear);
                }
            }
            pSet.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            sTotalMain = iTotalMain.ToString("#,##0");
            this.axVSPrinter1.Table = "<3000|<1000;Total No. of Businesses :|" + sTotalMain;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ListBnsOwnBday()
        {
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            int iCount = 0;
            string sOwnCode = string.Empty;
            string sOwnNm = string.Empty;
            string sBDate = string.Empty;
            
            OracleResultSet result = new OracleResultSet();
            if (m_sMonth == "00")
                result.Query = "select B.bin,OP.own_code,OP.Tel_no,OP.Bdate,B.bns_nm from own_profile OP,businesses B where B.Own_Code = Op.Own_Code and OP.BDate is not null order by OP.bdate asc";
            else
                result.Query = "Select * from (Select B.bin, OP.Own_Code, Op.Tel_No, OP.Bdate, to_char(to_date(OP.bdate,'MM dd yyyy'),'MM') Months,B.Bns_Nm from own_profile OP inner join Businesses B on B.Own_Code = Op.Own_Code) a where Months = '" + m_sMonth + "' order by bdate asc";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sOwnCode = result.GetString("own_code");
                    sOwnNm = StringUtilities.RemoveApostrophe(AppSettingsManager.GetBnsOwner(sOwnCode));

                    iCount += 1;
                    sBDate = result.GetString("Bdate");
                    this.axVSPrinter1.Table = ("^800|^1900|<3000|<3000|<2000|^1100;" + iCount + "|" + result.GetString("bin") + "|" + sOwnNm + "|" + result.GetString("bns_nm") + "|" + result.GetString("tel_no") + "|" + sBDate);
                }
            }
            result.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<3200|<11500;|Total Number of Business Owners :" + string.Format("{0:#,###}", iCount));

        }

        private void RetirementReport()
        {
            String sQuery, sDistrict, sFrom, sTo;
            String sBin, sBnsName = String.Empty, sBnsAdd = String.Empty, sOwnCode, sOwnName = String.Empty, sFullPartial, sMemo, sAppdate, sAppNo, sOrNo, sBnsDec, sBnsCode, sGross = String.Empty;
            String sContent, sBlank, sRecCount, sTotalRecCount;
            int iRecCount = 0, iTotalRecCount = 0;

            //mc_vspReports.SetOrientation(1); this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;


            

            sFrom = m_sRetireFrom;
            sTo = m_sRetireTo;

            sContent = "^1850|<2000|<2000|<1500|^500|<2000|^1000|^1000|^1000|<1500|>1000;";

            if (m_sBrgyName == "ALL")
                sQuery = "select brgy_nm from brgy order by brgy_nm";
            else
                sQuery = "select brgy_nm from brgy where brgy_nm = '" + m_sBrgyName + "'";

            OracleResultSet pSet2 = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    iRecCount = 0;
                    sDistrict = StringUtilities.HandleApostrophe(pSet.GetString("brgy_nm"));
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = ("<14200|;" + sDistrict);
                    this.axVSPrinter1.Table = (sDistrict);
                    this.axVSPrinter1.FontBold = false;

                    if (m_bCheckApp == false)
                        sQuery = string.Format("select * from retired_bns where bin in (select bin from businesses where bns_brgy = '{0}') order by apprvd_date,bin", sDistrict);
                    else
                        //sQuery = string.Format("select * from retired_bns where bin in (select bin from businesses where bns_brgy = '{0}') and apprvd_date >= to_date('{1}','MM/dd/yyyy') and apprvd_date <= to_date('{2}','MM/dd/yyyy') order by apprvd_date,bin", sDistrict, sFrom, sTo);
                        sQuery = string.Format("select * from retired_bns where bin in (select bin from businesses where bns_brgy = '{0}') and trunc(apprvd_date) >= trunc(to_date('{1}','MM/dd/yyyy')) and trunc(apprvd_date) <= trunc(to_date('{2}','MM/dd/yyyy')) order by apprvd_date,bin", sDistrict, sFrom, sTo); // RMC 20151118 correction in Retirement report

                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        while (pSet1.Read())
                        {
                            iRecCount++;
                            iTotalRecCount++;
                            sContent = "^1850|<2000|<2000|<1500|^500|<2000|^1000|^1000|^1000|<1500|>1000;";

                            sBin = StringUtilities.HandleApostrophe(pSet1.GetString("bin"));
                            sBnsCode = StringUtilities.HandleApostrophe(pSet1.GetString("bns_code_main"));
                            sGross = string.Format("{0:##0.00}", pSet1.GetDouble("gross"));
                            sFullPartial = StringUtilities.HandleApostrophe(pSet1.GetString("qtr"));
                            sMemo = StringUtilities.HandleApostrophe(pSet1.GetString("memoranda"));
                            //sAppdate = StringUtilities.HandleApostrophe(pSet1.GetDateTime("apprvd_date").ToShortDateString());
                            sAppdate = pSet1.GetDateTime("apprvd_date").ToString("MM/dd/yyyy");  // RMC 20151118 correction in Retirement report
                            sAppNo = StringUtilities.HandleApostrophe(pSet1.GetInt("retirement_no").ToString());
                            sOrNo = StringUtilities.HandleApostrophe(pSet1.GetString("or_no"));
                            sBnsDec = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsDesc(sBnsCode));

                            if (sFullPartial == "F")
                                sFullPartial = "F";
                            else
                                sFullPartial = "P";

                            sQuery = string.Format("select * from businesses where bin = '{0}'", sBin);
                            pSet2.Query = sQuery;
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                {
                                    sBnsName = StringUtilities.HandleApostrophe(pSet2.GetString("bns_nm"));
                                    sBnsAdd = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBin));
                                    sOwnCode = StringUtilities.HandleApostrophe(pSet2.GetString("own_code"));
                                    sOwnName = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(sOwnCode));
                                }
                            pSet2.Close();

                            String sPayment = String.Empty;
                            sQuery = string.Format("select sum(fees_amtdue) as fees_amtdue from or_table where qtr_paid = 'X' and or_no = '{0}' and bns_code_main = '{1}'", sOrNo, sBnsCode);
                            pSet2.Query = sQuery;
                            if (pSet2.Execute())
                            {
                                if (pSet2.Read())
                                    sPayment = string.Format("{0:##0.00}", pSet2.GetDouble("fees_amtdue"));
                                else
                                    sPayment = "0.00";
                            }
                            pSet2.Close();


                            sContent += sBin + "|" + sBnsName + "|" + sBnsAdd + "|" + sOwnName + "|" + sFullPartial + "|" + sMemo + "|" + sAppdate + "|" + sAppNo + "|" + sOrNo + "|" + sBnsDec + "|" + sGross + " / " + sPayment;		// RTL 10182006
                            //this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterTop;
                            this.axVSPrinter1.Table = sContent;
                            //this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.
                        }
                    pSet1.Close();

                    if (m_bCheckApp == false)
                        sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses where bns_brgy = '{0}')", sDistrict);
                    else
                        //sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses where bns_brgy = '{0}') and apprvd_date >= to_date('{1}','MM/dd/yyyy') and apprvd_date <= to_date('{2}','MM/dd/yyyy')", sDistrict, sFrom, sTo);
                        sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses where bns_brgy = '{0}') and trunc(apprvd_date) >= trunc(to_date('{1}','MM/dd/yyyy')) and trunc(apprvd_date) <= trunc(to_date('{2}','MM/dd/yyyy'))", sDistrict, sFrom, sTo);    // RMC 20151118 correction in Retirement report

                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        if (pSet1.Read())
                        {
                            iRecCount = pSet1.GetInt(0);
                        }
                    pSet1.Close();

                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = ("<2000|<12200;Total No. Records :|" + iRecCount.ToString());

                    if (m_bCheckApp == false)
                        sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses where bns_brgy = '{0}') and qtr = 'F'", sDistrict);
                    else
                        //sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses where bns_brgy = '{0}') and qtr = 'F' and apprvd_date >= to_date('{1}','MM/dd/yyyy') and apprvd_date <= to_date('{2}','MM/dd/yyyy')", sDistrict, sFrom, sTo);
                        sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses where bns_brgy = '{0}') and qtr = 'F' and trunc(apprvd_date) >= trunc(to_date('{1}','MM/dd/yyyy')) and trunc(apprvd_date) <= trunc(to_date('{2}','MM/dd/yyyy'))", sDistrict, sFrom, sTo);// RMC 20151118 correction in Retirement report

                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        if (pSet1.Read())
                        {
                            iRecCount = pSet1.GetInt(0);
                        }
                    pSet1.Close();

                    this.axVSPrinter1.Table = ("<2000|<12200;Full Retirement :|" + iRecCount.ToString());

                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
            pSet.Close();

            if (m_bCheckApp == false)
                sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses)");
            else
                //sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses) and apprvd_date >= to_date('{0}','MM/dd/yyyy') and apprvd_date <= to_date('{1}','MM/dd/yyyy')", sFrom, sTo);
                sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses) and trunc(apprvd_date) >= trunc(to_date('{0}','MM/dd/yyyy')) and trunc(apprvd_date) <= trunc(to_date('{1}','MM/dd/yyyy'))", sFrom, sTo);  // RMC 20151118 correction in Retirement report

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    iTotalRecCount = pSet.GetInt(0);
                }
            pSet.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ("<3000|<11200;Total No. Retired Businesses :|" + iTotalRecCount.ToString());

            if (m_bCheckApp == false)
                sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses) and qtr = 'F'");
            else
                //sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses)  and qtr = 'F' and apprvd_date >= to_date('{0}','MM/dd/yyyy') and apprvd_date <= to_date('{1}','MM/dd/yyyy')", sFrom, sTo);
                sQuery = string.Format("select count(distinct bin) from retired_bns where bin in (select bin from businesses)  and qtr = 'F' and trunc(apprvd_date) >= trunc(to_date('{0}','MM/dd/yyyy')) and trunc(apprvd_date) <= trunc(to_date('{1}','MM/dd/yyyy'))", sFrom, sTo);// RMC 20151118 correction in Retirement report

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    iTotalRecCount = pSet.GetInt(0);
                }
            pSet.Close();
            this.axVSPrinter1.Table = ("<3000|<11200;Full Retirememt:|" + iTotalRecCount.ToString());

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<1500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<1500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ListBnsBrgy()
        {
            String sQuery, sTotalRecCount, sTotalMain, sTotalAddl;
            String sBin, sBnsNm, sBnsAdd, sBnsStat, sBnsPrmtNo, sOrNo, sOrDate, sTaxYear, sQtr, sTerm, sSubgroup1, sContact, sNoEmp, sTempDataGroup = String.Empty, sDataGroup, sOwnNm = String.Empty;
            int iTotalRecCount = 0, iTotalMain = 0;
            double dGrCap = 0, dBTax = 0, dFees = 0, dPen = 0, dCap = 0, dGrandTotal = 0;
            String sTempGrCap, sTempBTax, sTempFees, sTempPenalty, sTempCap, sTempGrandTotal;
            String sGrandGrCap = "", sGrandBTax = "", sGrandPen = "", sGrandFees = "", sGrandCap = "", sGrandTotal = "";

            //mc_vspReports.SetOrientation(0); this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            sQuery = string.Format("SELECT DATA_GROUP, SUBGROUP1, SUBGROUP2, BIN, BNS_NM, BNS_ADDR, BNS_STAT, BNS_GROSS, BNS_CAPITAL, PERMIT_NO, BTAX, FEES, SUR_INT, OR_NO, OR_DATE, PAYMENT_TERM, TAX_YEAR, QTR_PAID, CURRENT_USER, CONTACT_NO, NO_EMP FROM REP_LIST_BNS WHERE report_name = '{0}' AND user_code = '{1}' ORDER BY data_group ASC,", m_sReportName, StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));

            string sTempValue = "";
            using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Order by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "By Business Name";
                MsgBoxOptions.RadioNo = "By Permit Number";
                MsgBoxOptions.btnNo.Enabled = false;
                MsgBoxOptions.ShowDialog();
                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                sQuery += " bns_nm ASC";
            else //if (sTempValue != String.Empty)
                sQuery += " permit_no ASC";
                
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iTotalMain++;

                    sDataGroup = StringUtilities.HandleApostrophe(pSet.GetString("data_group"));
                    sSubgroup1 = StringUtilities.HandleApostrophe(pSet.GetString("subgroup1"));
                    sOwnNm = StringUtilities.HandleApostrophe(pSet.GetString("subgroup2"));
                    sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                    sBnsNm = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                    sBnsAdd = StringUtilities.HandleApostrophe(pSet.GetString("bns_addr"));
                    sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                    sTempGrCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_gross"));
                    sTempCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_capital"));
                    sBnsPrmtNo = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                    sTempBTax = string.Format("{0:##0.00}", pSet.GetDouble("btax"));
                    sTempFees = string.Format("{0:##0.00}", pSet.GetDouble("fees"));
                    sTempPenalty = string.Format("{0:##0.00}", pSet.GetDouble("sur_int"));
                    sOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                    if (sOrNo.Trim() == String.Empty || sOrNo == null)
                        sOrDate = "";
                    else
                        sOrDate = StringUtilities.HandleApostrophe(pSet.GetString("or_date"));
                    sTerm = StringUtilities.HandleApostrophe(pSet.GetString("payment_term"));
                    sTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));
                    sQtr = StringUtilities.HandleApostrophe(pSet.GetString("qtr_paid"));
                    sContact = StringUtilities.HandleApostrophe(pSet.GetString("contact_no"));
                    sNoEmp = StringUtilities.HandleApostrophe(pSet.GetString("no_emp"));

                    dGrCap += Convert.ToDouble(sTempGrCap);
                    dCap += Convert.ToDouble(sTempCap);
                    dBTax += Convert.ToDouble(sTempBTax);
                    dFees += Convert.ToDouble(sTempFees);
                    dPen += Convert.ToDouble(sTempPenalty);
                    dGrandTotal += (Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));
                    sTempGrandTotal = Convert.ToString(Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));
                    
                    this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                    if (sTempDataGroup == "")
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<18800;" + sTempDataGroup;
                        this.axVSPrinter1.FontBold = false;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<18800;" + sDataGroup;
                        this.axVSPrinter1.FontBold = false;
                    }
                    this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
                    this.axVSPrinter1.Table = "<1850|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|^1000|^1000|^600|^700|^600|<1200|^1000;" + sBin + "|" + sBnsNm + "\n" + sBnsAdd + "\n" + sOwnNm + "|" + sSubgroup1 + "\n" + sBnsStat + "|" + Convert.ToDouble(sTempGrCap).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempCap).ToString("#,##0.00") + "|" + sBnsPrmtNo + "|" + Convert.ToDouble(sTempBTax).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempFees).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempPenalty).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempGrandTotal).ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sTerm + "|" + sTaxYear + "|" + sQtr + "|" + sContact + "|" + sNoEmp;
                                                 //1    2     3       4    5      6   7     8    9    10    11    12    13    14   15  16     17
                }
            }
            pSet.Close();

            //_variant_t xStart, xEnd, yGrid, yEnd;

            sTotalMain = iTotalMain.ToString("#,##0");
            sTotalAddl = (iTotalRecCount - iTotalMain).ToString();
            sTotalRecCount = iTotalRecCount.ToString();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //yGrid = this.axVSPrinter1.CurrentY();
            //CreateLine(700, 20700, yGrid, yGrid);

            sGrandGrCap = string.Format("{0:#,##0.00}", dGrCap);
            sGrandBTax = string.Format("{0:#,##0.00}", dBTax);
            sGrandFees = string.Format("{0:#,##0.00}", dFees);
            sGrandCap = string.Format("{0:#,##0.00}", dCap);
            sGrandPen = string.Format("{0:#,##0.00}", dPen);
            sGrandTotal = string.Format("{0:#,##0.00}", dGrandTotal);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^1850|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|^1000|^1000|^600|^700|^600|<1200|^1000;||GRAND TOTAL:|" + sGrandGrCap + "|" + sGrandCap + "||" + sGrandBTax + "|" + sGrandFees + "|" + sGrandPen + "|" + sGrandTotal + "|||||||";

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = "<3000|<1000;Total No. of Businesses :|" + sTotalMain;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ListBnsDist()
        {
            String sQuery, sTotalRecCount, sTotalMain, sTotalAddl;
            String sBin, sBnsNm, sBnsAdd, sBnsStat, sBnsPrmtNo, sOrNo, sOrDate, sTaxYear, sQtr, sTerm, sSubgroup1, sTotal, sOwnNm = String.Empty;
            int iTotalRecCount = 0, iTotalMain = 0;
            double dGrCap = 0, dBTax = 0, dFees = 0, dPen = 0, dCap = 0, dGrandTotal = 0;
            String sTempGrCap, sTempBTax, sTempFees, sTempPenalty, sTempCap, sTempGrandTotal;
            String sGrandGrCap = "", sGrandBTax = "", sGrandPen = "", sGrandFees = "", sGrandCap = "", sGrandTotal = "";

            //mc_vspReports.SetOrientation(0); this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            sQuery = string.Format("SELECT SUBGROUP1, SUBGROUP2, BIN, BNS_NM, BNS_ADDR, BNS_STAT, BNS_GROSS, BNS_CAPITAL, PERMIT_NO, BTAX, FEES, SUR_INT, OR_NO, OR_DATE, PAYMENT_TERM, TAX_YEAR, QTR_PAID, CURRENT_USER FROM REP_LIST_BNS WHERE report_name = '{0}' AND user_code = '{1}' ORDER BY DATA_GROUP ASC,", m_sReportName, StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));

            string sTempValue = "";
            using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Order by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "By Business Name";
                MsgBoxOptions.RadioNo = "By Permit Number";
                MsgBoxOptions.btnNo.Enabled = false;
                MsgBoxOptions.ShowDialog();
                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                sQuery += " bns_nm ASC";
            else //if (sTempValue != String.Empty)
                sQuery += " permit_no ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iTotalMain++;

                    sSubgroup1 = StringUtilities.HandleApostrophe(pSet.GetString("subgroup1"));
                    sOwnNm = StringUtilities.HandleApostrophe(pSet.GetString("subgroup2"));
                    sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                    sBnsNm = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                    sBnsAdd = StringUtilities.HandleApostrophe(pSet.GetString("bns_addr"));
                    sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                    sTempGrCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_gross"));
                    sTempCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_capital"));
                    sBnsPrmtNo = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                    sTempBTax = string.Format("{0:##0.00}", pSet.GetDouble("btax"));
                    sTempFees = string.Format("{0:##0.00}", pSet.GetDouble("fees"));
                    sTempPenalty = string.Format("{0:##0.00}", pSet.GetDouble("sur_int"));
                    sOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                    if (sOrNo.Trim() == String.Empty || sOrNo == null)
                        sOrDate = "";
                    else
                        sOrDate = StringUtilities.HandleApostrophe(pSet.GetString("or_date"));
                    sTerm = StringUtilities.HandleApostrophe(pSet.GetString("payment_term"));
                    sTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));
                    sQtr = StringUtilities.HandleApostrophe(pSet.GetString("qtr_paid"));

                    dGrCap += Convert.ToDouble(sTempGrCap);
                    dCap += Convert.ToDouble(sTempCap);
                    dBTax += Convert.ToDouble(sTempBTax);
                    dFees += Convert.ToDouble(sTempFees);
                    dPen += Convert.ToDouble(sTempPenalty);
                    dGrandTotal += (Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));
                    sTempGrandTotal = Convert.ToString(Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));
                    this.axVSPrinter1.Table = "^1700|<2000|^1200|>1500|>1200|<1000|>1000|>1200|>1200|>1200|^1200|^1200|^700|^700|^700;" + sBin + "|" + sBnsNm + "\n" + sBnsAdd + "\n" + sOwnNm + "|" + sSubgroup1 + "\n" + sBnsStat + "|" + Convert.ToDouble(sTempGrCap).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempCap).ToString("#,##0.00") + "|" + sBnsPrmtNo + "|" + Convert.ToDouble(sTempBTax).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempFees).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempPenalty).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempGrandTotal).ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sTerm + "|" + sTaxYear + "|" + sQtr;
                    //1    2     3       4    5      6   7     8      9    10    11    12    13    14   15
                }
            }
            pSet.Close();

            //_variant_t xStart, xEnd, yGrid, yEnd;

            sTotalMain = iTotalMain.ToString("#,##0");
            sTotalAddl = (iTotalRecCount - iTotalMain).ToString();
            sTotalRecCount = iTotalRecCount.ToString();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //yGrid = this.axVSPrinter1.CurrentY();
            //CreateLine(700, 20700, yGrid, yGrid);

            sGrandGrCap = string.Format("{0:#,##0.00}", dGrCap);
            sGrandBTax = string.Format("{0:#,##0.00}", dBTax);
            sGrandFees = string.Format("{0:#,##0.00}", dFees);
            sGrandCap = string.Format("{0:#,##0.00}", dCap);
            sGrandPen = string.Format("{0:#,##0.00}", dPen);
            sGrandTotal = string.Format("{0:#,##0.00}", dGrandTotal);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^1700|<2000|^1200|>1500|>1200|<1000|>1000|>1200|>1200|>1200|^1200|^1200|^700|^700|^700;||GRAND TOTAL:|" + sGrandGrCap + "|" + sGrandCap + "||" + sGrandBTax + "|" + sGrandFees + "|" + sGrandPen + "|" + sGrandTotal + "|||||";

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = "<3000|<1000;Total No. of Businesses :|" + sTotalMain;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ListBnsMainBusiness()
        {
            String sQuery, sTotalRecCount, sTotalMain, sTotalAddl;
            String sBin, sBnsNm, sBnsAdd, sBnsStat, sBnsPrmtNo, sOrNo, sOrDate, sTaxYear, sQtr, sTerm, sSubgroup1, sContact, sNoEmp, sTempDataGroup = String.Empty, sDataGroup, sOwnNm = String.Empty;
            int iTotalRecCount = 0, iTotalMain = 0;
            double dGrCap = 0, dBTax = 0, dFees = 0, dPen = 0, dCap = 0, dGrandTotal = 0;
            String sTempGrCap, sTempBTax, sTempFees, sTempPenalty, sTempCap, sTempGrandTotal;
            String sGrandGrCap = "", sGrandBTax = "", sGrandPen = "", sGrandFees = "", sGrandCap = "", sGrandTotal = "";

            //mc_vspReports.SetOrientation(0); this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            sQuery = string.Format("SELECT DATA_GROUP, SUBGROUP1, SUBGROUP2, BIN, BNS_NM, BNS_ADDR, BNS_STAT, BNS_GROSS, BNS_CAPITAL, PERMIT_NO, BTAX, FEES, SUR_INT, OR_NO, OR_DATE, PAYMENT_TERM, TAX_YEAR, QTR_PAID, CURRENT_USER, CONTACT_NO, NO_EMP FROM REP_LIST_BNS WHERE report_name = '{0}' AND user_code = '{1}' ORDER BY data_group ASC,", m_sReportName, StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));

            string sTempValue = "";
            using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Order by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "By Business Name";
                MsgBoxOptions.RadioNo = "By Permit Number";
                MsgBoxOptions.btnNo.Enabled = false;
                MsgBoxOptions.ShowDialog();
                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                sQuery += " bns_nm ASC";
            else //if (sTempValue != String.Empty)
                sQuery += " permit_no ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iTotalMain++;

                    sDataGroup = StringUtilities.HandleApostrophe(pSet.GetString("data_group"));
                    sSubgroup1 = StringUtilities.HandleApostrophe(pSet.GetString("subgroup1"));
                    sOwnNm = StringUtilities.HandleApostrophe(pSet.GetString("subgroup2"));
                    sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                    sBnsNm = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                    sBnsAdd = StringUtilities.HandleApostrophe(pSet.GetString("bns_addr"));
                    sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                    sTempGrCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_gross"));
                    sTempCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_capital"));
                    sBnsPrmtNo = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                    sTempBTax = string.Format("{0:##0.00}", pSet.GetDouble("btax"));
                    sTempFees = string.Format("{0:##0.00}", pSet.GetDouble("fees"));
                    sTempPenalty = string.Format("{0:##0.00}", pSet.GetDouble("sur_int"));
                    sOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                    if (sOrNo.Trim() == String.Empty || sOrNo == null)
                        sOrDate = "";
                    else
                        sOrDate = StringUtilities.HandleApostrophe(pSet.GetString("or_date"));
                    sTerm = StringUtilities.HandleApostrophe(pSet.GetString("payment_term"));
                    sTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));
                    sQtr = StringUtilities.HandleApostrophe(pSet.GetString("qtr_paid"));
                    sContact = StringUtilities.HandleApostrophe(pSet.GetString("contact_no"));
                    sNoEmp = StringUtilities.HandleApostrophe(pSet.GetString("no_emp"));

                    dGrCap += Convert.ToDouble(sTempGrCap);
                    dCap += Convert.ToDouble(sTempCap);
                    dBTax += Convert.ToDouble(sTempBTax);
                    dFees += Convert.ToDouble(sTempFees);
                    dPen += Convert.ToDouble(sTempPenalty);
                    dGrandTotal += (Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));
                    sTempGrandTotal = Convert.ToString(Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));

                    if (sTempDataGroup == "")
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<18800;" + sTempDataGroup;
                        this.axVSPrinter1.FontBold = false;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<18800;" + sDataGroup;
                        this.axVSPrinter1.FontBold = false;
                    }

                    this.axVSPrinter1.Table = "^1850|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|^1000|^1000|^600|^700|^600|<1200|^1000;" + sBin + "|" + sBnsNm + "\n" + sBnsAdd + "\n" + sOwnNm + "|" + sSubgroup1 + "\n" + sBnsStat + "|" + Convert.ToDouble(sTempGrCap).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempCap).ToString("#,##0.00") + "|" + sBnsPrmtNo + "|" + Convert.ToDouble(sTempBTax).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempFees).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempPenalty).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempGrandTotal).ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sTerm + "|" + sTaxYear + "|" + sQtr + "|" + sContact + "|" + sNoEmp;
                    //1    2     3     4    5      6   7     8    9    10    11    12    13    14   15  16     17
                }
            }
            pSet.Close();

            //_variant_t xStart, xEnd, yGrid, yEnd;

            sTotalMain = iTotalMain.ToString("#,##0");
            sTotalAddl = (iTotalRecCount - iTotalMain).ToString();
            sTotalRecCount = iTotalRecCount.ToString();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //yGrid = this.axVSPrinter1.CurrentY();
            //CreateLine(700, 20700, yGrid, yGrid);

            sGrandGrCap = string.Format("{0:#,##0.00}", dGrCap);
            sGrandBTax = string.Format("{0:#,##0.00}", dBTax);
            sGrandFees = string.Format("{0:#,##0.00}", dFees);
            sGrandCap = string.Format("{0:#,##0.00}", dCap);
            sGrandPen = string.Format("{0:#,##0.00}", dPen);
            sGrandTotal = string.Format("{0:#,##0.00}", dGrandTotal);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^1850|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|^1000|^1000|^600|^700|^600|<1200|^1000;||GRAND TOTAL:|" + sGrandGrCap + "|" + sGrandCap + "||" + sGrandBTax + "|" + sGrandFees + "|" + sGrandPen + "|" + sGrandTotal + "|||||||";

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = "<3000|<1000;Total No. of Businesses :|" + sTotalMain;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ListBnsOrgKind()
        {
            String sQuery, sTotalRecCount, sTotalMain, sTotalAddl;
            String sBin, sBnsNm, sBnsAdd, sBnsStat, sBnsPrmtNo, sOrNo, sOrDate, sTaxYear, sQtr, sTerm, sSubgroup1, sTotal, sContact, sNoEmp;
            int iTotalRecCount = 0, iTotalMain = 0;
            double dGrCap = 0, dBTax = 0, dFees = 0, dPen = 0, dCap = 0, dGrandTotal = 0;
            String sTempGrCap, sTempBTax, sTempFees, sTempPenalty, sTempCap, sTempGrandTotal;
            String sGrandGrCap = "", sGrandBTax = "", sGrandPen = "", sGrandFees = "", sGrandCap = "", sGrandTotal = "";
            String sTemp = "", sDataGroup, sOwnNm = String.Empty;
            //mc_vspReports.SetOrientation(0); this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            sQuery = string.Format("SELECT DATA_GROUP, SUBGROUP1, SUBGROUP2, BIN, BNS_NM, BNS_ADDR, BNS_STAT, BNS_GROSS, BNS_CAPITAL, PERMIT_NO, BTAX, FEES, SUR_INT, OR_NO, OR_DATE, PAYMENT_TERM, TAX_YEAR, QTR_PAID, CURRENT_USER, CONTACT_NO, NO_EMP FROM REP_LIST_BNS WHERE report_name = '{0}' AND user_code = '{1}' ORDER BY DATA_GROUP ASC,", m_sReportName, StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));

            string sTempValue = "";
            using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Order by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "By Business Name";
                MsgBoxOptions.RadioNo = "By Permit Number";
                MsgBoxOptions.btnNo.Enabled = false;
                MsgBoxOptions.ShowDialog();
                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                sQuery += " bns_nm ASC";
            else //if (sTempValue != String.Empty)
                sQuery += " permit_no ASC";


            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iTotalMain++;

                    sDataGroup = StringUtilities.HandleApostrophe(pSet.GetString("data_group"));
                    sSubgroup1 = StringUtilities.HandleApostrophe(pSet.GetString("subgroup1"));
                    sOwnNm = StringUtilities.HandleApostrophe(pSet.GetString("subgroup2"));
                    sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                    sBnsNm = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                    sBnsAdd = StringUtilities.HandleApostrophe(pSet.GetString("bns_addr"));
                    sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                    sTempGrCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_gross"));
                    sTempCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_capital"));
                    sBnsPrmtNo = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                    sTempBTax = string.Format("{0:##0.00}", pSet.GetDouble("btax"));
                    sTempFees = string.Format("{0:##0.00}", pSet.GetDouble("fees"));
                    sTempPenalty = string.Format("{0:##0.00}", pSet.GetDouble("sur_int"));
                    sOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                    if (sOrNo.Trim() == String.Empty || sOrNo == null)
                        sOrDate = "";
                    else
                        sOrDate = StringUtilities.HandleApostrophe(pSet.GetString("or_date"));
                    sTerm = StringUtilities.HandleApostrophe(pSet.GetString("payment_term"));
                    sTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));
                    sQtr = StringUtilities.HandleApostrophe(pSet.GetString("qtr_paid"));
                    sContact = StringUtilities.HandleApostrophe(pSet.GetString("contact_no"));
                    sNoEmp = StringUtilities.HandleApostrophe(pSet.GetString("no_emp"));

                    dGrCap += Convert.ToDouble(sTempGrCap);
                    dCap += Convert.ToDouble(sTempCap);
                    dBTax += Convert.ToDouble(sTempBTax);
                    dFees += Convert.ToDouble(sTempFees);
                    dPen += Convert.ToDouble(sTempPenalty);
                    dGrandTotal += (Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));
                    sTempGrandTotal = Convert.ToString(Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));

                    if (sTemp == "")
                    {
                        sTemp = sDataGroup;
                        this.axVSPrinter1.FontBold = true;
                        this.axVSPrinter1.Table = "<18800;" + sTemp;
                        this.axVSPrinter1.FontBold = false;
                    }
                    else if (sTemp != sDataGroup)
                    {
                        sTemp = sDataGroup;
                        this.axVSPrinter1.FontBold = true;
                        this.axVSPrinter1.Table = "<18800;" + sTemp;
                        this.axVSPrinter1.FontBold = false;
                    }
                    //JARS 20170302 ADJUSTED COLUMN NO. OF EMP
                    this.axVSPrinter1.Table = "<1550|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|^1000|^1000|^600|^700|^600|>1200|^500;" + sBin + "|" + sBnsNm + "\n" + sBnsAdd + "\n" + sOwnNm + "|" + sSubgroup1 + "\n" + sBnsStat + "|" + Convert.ToDouble(sTempGrCap).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempCap).ToString("#,##0.00") + "|" + sBnsPrmtNo + "|" + Convert.ToDouble(sTempBTax).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempFees).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempPenalty).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempGrandTotal).ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sTerm + "|" + sTaxYear + "|" + sQtr + "|" + sContact + "|" + sNoEmp;
                    //this.axVSPrinter1.Table = "^1850|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|^1000|^1000|^600|^700|^600|<1200|^1000;" + sBin + "|" + sBnsNm + "\n" + sBnsAdd + "\n" + sOwnNm + "|" + sSubgroup1 + "\n" + sBnsStat + "|" + Convert.ToDouble(sTempGrCap).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempCap).ToString("#,##0.00") + "|" + sBnsPrmtNo + "|" + Convert.ToDouble(sTempBTax).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempFees).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempPenalty).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempGrandTotal).ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sTerm + "|" + sTaxYear + "|" + sQtr + "|" + sContact + "|" + sNoEmp;
                    //1    2     3      4    5      6     7     8    9     10    11    12    13    14   15
                }
            }
            pSet.Close();

            //_variant_t xStart, xEnd, yGrid, yEnd;

            sTotalMain = iTotalMain.ToString("#,##0");
            sTotalAddl = (iTotalRecCount - iTotalMain).ToString();
            sTotalRecCount = iTotalRecCount.ToString();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //yGrid = this.axVSPrinter1.CurrentY();
            //CreateLine(700, 20700, yGrid, yGrid);

            sGrandGrCap = string.Format("{0:#,##0.00}", dGrCap);
            sGrandBTax = string.Format("{0:#,##0.00}", dBTax);
            sGrandFees = string.Format("{0:#,##0.00}", dFees);
            sGrandCap = string.Format("{0:#,##0.00}", dCap);
            sGrandPen = string.Format("{0:#,##0.00}", dPen);
            sGrandTotal = string.Format("{0:#,##0.00}", dGrandTotal);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^1550|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|^1000|^1000|^600|^700|^600|>1200|^500;||GRAND TOTAL:|" + sGrandGrCap + "|" + sGrandCap + "||" + sGrandBTax + "|" + sGrandFees + "|" + sGrandPen + "|" + sGrandTotal + "|||||||";
            //this.axVSPrinter1.Table = "^1850|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|<1000|<1000|<600|<700|<600|<1200|^1000;||GRAND TOTAL:|" + sGrandGrCap + "|" + sGrandCap + "||" + sGrandBTax + "|" + sGrandFees + "|" + sGrandPen + "|" + sGrandTotal + "|||||||";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = "<3000|<1000;Total No. of Businesses :|" + sTotalMain;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ListBnsBusStatus()
        {
            String sQuery, sTotalRecCount, sTotalMain, sTotalAddl;
            String sBin, sBnsNm, sBnsAdd, sBnsStat, sBnsPrmtNo, sOrNo, sOrDate, sTaxYear, sQtr, sTerm, sSubgroup1, sContact, sNoEmp, sTempDataGroup = String.Empty, sDataGroup, sOwnNm = String.Empty;
            int iTotalRecCount = 0, iTotalMain = 0;
            double dGrCap = 0, dBTax = 0, dFees = 0, dPen = 0, dCap = 0, dGrandTotal = 0;
            String sTempGrCap, sTempBTax, sTempFees, sTempPenalty, sTempCap, sTempGrandTotal;
            String sGrandGrCap = "", sGrandBTax = "", sGrandPen = "", sGrandFees = "", sGrandCap = "", sGrandTotal = "";

            //mc_vspReports.SetOrientation(0); this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            
            sQuery = string.Format("SELECT DATA_GROUP, SUBGROUP1, SUBGROUP2, BIN, BNS_NM, BNS_ADDR, BNS_STAT, BNS_GROSS, BNS_CAPITAL, PERMIT_NO, BTAX, FEES, SUR_INT, OR_NO, OR_DATE, PAYMENT_TERM, TAX_YEAR, QTR_PAID, CURRENT_USER, CONTACT_NO, NO_EMP FROM REP_LIST_BNS WHERE report_name = '{0}' AND user_code = '{1}' ORDER BY data_group ASC,", m_sReportName, StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));

            string sTempValue = "";
            using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Order by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "By Business Name";
                MsgBoxOptions.RadioNo = "By Permit Number";
                MsgBoxOptions.btnNo.Enabled = false;
                MsgBoxOptions.ShowDialog();
                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                sQuery += " bns_nm ASC";
            else //if (sTempValue != String.Empty)
                sQuery += " permit_no ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iTotalMain++;

                    sDataGroup = StringUtilities.HandleApostrophe(pSet.GetString("data_group"));
                    sSubgroup1 = StringUtilities.HandleApostrophe(pSet.GetString("subgroup1"));
                    sOwnNm = StringUtilities.HandleApostrophe(pSet.GetString("subgroup2"));
                    sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                    sBnsNm = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                    sBnsAdd = StringUtilities.HandleApostrophe(pSet.GetString("bns_addr"));
                    sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                    sTempGrCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_gross"));
                    sTempCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_capital"));
                    sBnsPrmtNo = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                    sTempBTax = string.Format("{0:##0.00}", pSet.GetDouble("btax"));
                    sTempFees = string.Format("{0:##0.00}", pSet.GetDouble("fees"));
                    sTempPenalty = string.Format("{0:##0.00}", pSet.GetDouble("sur_int"));
                    sOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                    if (sOrNo.Trim() == String.Empty || sOrNo == null)
                        sOrDate = "";
                    else
                        sOrDate = StringUtilities.HandleApostrophe(pSet.GetString("or_date"));
                    sTerm = StringUtilities.HandleApostrophe(pSet.GetString("payment_term"));
                    sTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));
                    sQtr = StringUtilities.HandleApostrophe(pSet.GetString("qtr_paid"));
                    sContact = StringUtilities.HandleApostrophe(pSet.GetString("contact_no"));
                    sNoEmp = StringUtilities.HandleApostrophe(pSet.GetString("no_emp"));

                    dGrCap += Convert.ToDouble(sTempGrCap);
                    dCap += Convert.ToDouble(sTempCap);
                    dBTax += Convert.ToDouble(sTempBTax);
                    dFees += Convert.ToDouble(sTempFees);
                    dPen += Convert.ToDouble(sTempPenalty);
                    dGrandTotal += (Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));
                    sTempGrandTotal = Convert.ToString(Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));

                    if (sTempDataGroup == "")
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<18800;" + sTempDataGroup;
                        this.axVSPrinter1.FontBold = false;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<18800;" + sDataGroup;
                        this.axVSPrinter1.FontBold = false;
                    }

                    this.axVSPrinter1.Table = "^1850|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|^1000|^1000|^600|^700|^600|<1200|^1000;" + sBin + "|" + sBnsNm + "\n" + sBnsAdd + "\n" + sOwnNm + "|" + sSubgroup1 + "\n" + sBnsStat + "|" + Convert.ToDouble(sTempGrCap).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempCap).ToString("#,##0.00") + "|" + sBnsPrmtNo + "|" + Convert.ToDouble(sTempBTax).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempFees).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempPenalty).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempGrandTotal).ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sTerm + "|" + sTaxYear + "|" + sQtr + "|" + sContact + "|" + sNoEmp;
                    //1    2     3      4    5      6   7     8    9    10    11    12    13    14   15  16     17
                }
            }
            pSet.Close();

            //_variant_t xStart, xEnd, yGrid, yEnd;

            sTotalMain = iTotalMain.ToString("#,##0");
            sTotalAddl = (iTotalRecCount - iTotalMain).ToString();
            sTotalRecCount = iTotalRecCount.ToString();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //yGrid = this.axVSPrinter1.CurrentY();
            //CreateLine(700, 20700, yGrid, yGrid);

            sGrandGrCap = string.Format("{0:#,##0.00}", dGrCap);
            sGrandBTax = string.Format("{0:#,##0.00}", dBTax);
            sGrandFees = string.Format("{0:#,##0.00}", dFees);
            sGrandCap = string.Format("{0:#,##0.00}", dCap);
            sGrandPen = string.Format("{0:#,##0.00}", dPen);
            sGrandTotal = string.Format("{0:#,##0.00}", dGrandTotal);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^1850|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|^1000|^1000|^600|^700|^600|<1200|^1000;||GRAND TOTAL:|" + sGrandGrCap + "|" + sGrandCap + "||" + sGrandBTax + "|" + sGrandFees + "|" + sGrandPen + "|" + sGrandTotal + "|||||||";

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = "<3000|<1000;Total No. of Businesses :|" + sTotalMain;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ListBnsGrossReceipt()
        {
            String sQuery, sTotalRecCount, sTotalMain, sTotalAddl;
            String sBin, sBnsNm, sBnsAdd, sBnsStat, sBnsPrmtNo, sOrNo, sOrDate, sTaxYear, sQtr, sTerm, sSubgroup1, sTotal, sContact, sNoEmp, sOwnNm = String.Empty;
            int iTotalRecCount = 0, iTotalMain = 0;
            double dGrCap = 0, dBTax = 0, dFees = 0, dPen = 0, dCap = 0, dGrandTotal = 0;
            String sTempGrCap, sTempBTax, sTempFees, sTempPenalty, sTempCap, sTempGrandTotal;
            String sGrandGrCap = "", sGrandBTax = "", sGrandPen = "", sGrandFees = "", sGrandCap = "", sGrandTotal = "";

            //mc_vspReports.SetOrientation(0); this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            sQuery = string.Format("SELECT SUBGROUP1, SUBGROUP2, BIN, BNS_NM, BNS_ADDR, BNS_STAT, BNS_GROSS, BNS_CAPITAL, PERMIT_NO, BTAX, FEES, SUR_INT, OR_NO, OR_DATE, PAYMENT_TERM, TAX_YEAR, QTR_PAID, CURRENT_USER, CONTACT_NO, NO_EMP FROM REP_LIST_BNS WHERE report_name = '{0}' AND user_code = '{1}' ORDER BY BNS_GROSS DESC, BNS_NM ASC,", m_sReportName, StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));

            string sTempValue = "";
            using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Order by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "By Business Name";
                MsgBoxOptions.RadioNo = "By Permit Number";
                MsgBoxOptions.btnNo.Enabled = false;
                MsgBoxOptions.ShowDialog();
                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                sQuery += " bns_nm ASC";
            else //if (sTempValue != String.Empty)
                sQuery += " permit_no ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iTotalMain++;

                    sSubgroup1 = StringUtilities.HandleApostrophe(pSet.GetString("subgroup1"));
                    sOwnNm = StringUtilities.HandleApostrophe(pSet.GetString("subgroup2"));
                    sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                    sBnsNm = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                    sBnsAdd = StringUtilities.HandleApostrophe(pSet.GetString("bns_addr"));
                    sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                    sTempGrCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_gross"));
                    sTempCap = string.Format("{0:##0.00}", pSet.GetDouble("bns_capital"));
                    sBnsPrmtNo = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                    sTempBTax = string.Format("{0:##0.00}", pSet.GetDouble("btax"));
                    sTempFees = string.Format("{0:##0.00}", pSet.GetDouble("fees"));
                    sTempPenalty = string.Format("{0:##0.00}", pSet.GetDouble("sur_int"));
                    sOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                    if (sOrNo.Trim() == String.Empty || sOrNo == null)
                        sOrDate = "";
                    else
                        sOrDate = StringUtilities.HandleApostrophe(pSet.GetString("or_date"));
                    sTerm = StringUtilities.HandleApostrophe(pSet.GetString("payment_term"));
                    sTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));
                    sQtr = StringUtilities.HandleApostrophe(pSet.GetString("qtr_paid"));
                    sContact = StringUtilities.HandleApostrophe(pSet.GetString("contact_no"));
                    sNoEmp = StringUtilities.HandleApostrophe(pSet.GetString("no_emp"));

                    dGrCap += Convert.ToDouble(sTempGrCap);
                    dCap += Convert.ToDouble(sTempCap);
                    dBTax += Convert.ToDouble(sTempBTax);
                    dFees += Convert.ToDouble(sTempFees);
                    dPen += Convert.ToDouble(sTempPenalty);
                    dGrandTotal += (Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));
                    sTempGrandTotal = Convert.ToString(Convert.ToDouble(sTempFees) + Convert.ToDouble(sTempBTax) + Convert.ToDouble(sTempPenalty));
                    this.axVSPrinter1.Table = "^1850|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|^1000|^1000|^600|^700|^600|<1200|^1000;" + sBin + "|" + sBnsNm + "\n" + sBnsAdd + "\n" + sOwnNm + "|" + sSubgroup1 + "\n" + sBnsStat + "|" + Convert.ToDouble(sTempGrCap).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempCap).ToString("#,##0.00") + "|" + sBnsPrmtNo + "|" + Convert.ToDouble(sTempBTax).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempFees).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempPenalty).ToString("#,##0.00") + "|" + Convert.ToDouble(sTempGrandTotal).ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sTerm + "|" + sTaxYear + "|" + sQtr + "|" + sContact + "|" + sNoEmp;
                    //1    2     3       4    5      6    7     8    9    10    11    12    13    14   15
                }
            }
            pSet.Close();

            //_variant_t xStart, xEnd, yGrid, yEnd;

            sTotalMain = iTotalMain.ToString("#,##0");
            sTotalAddl = (iTotalRecCount - iTotalMain).ToString();
            sTotalRecCount = iTotalRecCount.ToString();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //yGrid = this.axVSPrinter1.CurrentY();
            //CreateLine(700, 20700, yGrid, yGrid);

            sGrandGrCap = string.Format("{0:#,##0.00}", dGrCap);
            sGrandBTax = string.Format("{0:#,##0.00}", dBTax);
            sGrandFees = string.Format("{0:#,##0.00}", dFees);
            sGrandCap = string.Format("{0:#,##0.00}", dCap);
            sGrandPen = string.Format("{0:#,##0.00}", dPen);
            sGrandTotal = string.Format("{0:#,##0.00}", dGrandTotal);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^1850|<2000|^1000|>1500|>1400|<1050|>1200|>1200|>1200|>1200|<1000|<1000|<600|<700|<600|<1200|^1000;||GRAND TOTAL:|" + sGrandGrCap + "|" + sGrandCap + "||" + sGrandBTax + "|" + sGrandFees + "|" + sGrandPen + "|" + sGrandTotal + "|||||||";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = "<3000|<1000;Total No. of Businesses :|" + sTotalMain;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ListBnsPrmtNum() //MCR 20130102
        {
            String sQuery;
            String sPermitNo, sBnsName, sBnsAdd, sOwnName, sBnsDec, sRepName;
            String sBIN, sBnsStat, sGrossCap, sTax, sFees, sSurInt, sOrNo, sOrDate, sTerm, sTaxYear; // ALJ 01222008 list of bns by permit revision
            String sContent = "", sBlank, sTotalRecCount, sHeader;
            int iTotalRecCount = 0;

            string sTempValue = "";
            using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
            {
                MsgBoxOptions.MessageCaption = "Order by?";
                MsgBoxOptions.RadioType = 0;
                MsgBoxOptions.RadioYes = "By Business Name";
                MsgBoxOptions.RadioNo = "By Permit Number";
                MsgBoxOptions.btnNo.Enabled = false;
                MsgBoxOptions.ShowDialog();
                sTempValue = MsgBoxOptions.SelectedText;
            }

            if (sTempValue == "Yes")
                sContent += " order by bns_nm ASC";
            else //if (sTempValue != String.Empty)
                sContent += " order by permit_no ASC";

            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();
            sQuery = string.Format("select * from rep_list_bns where report_name = '{0}' and user_code = '{1}'", m_sReportName, StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));
            sQuery += sContent;
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iTotalRecCount++;

                    sPermitNo = StringUtilities.HandleApostrophe(pSet.GetString("permit_no"));
                    sOwnName = StringUtilities.HandleApostrophe(pSet.GetString("subgroup1"));
                    sBnsName = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                    sBnsAdd = StringUtilities.HandleApostrophe(pSet.GetString("bns_addr"));
                    sBnsDec = StringUtilities.HandleApostrophe(pSet.GetString("subgroup2"));

                    // ALJ 01222008 (s) list of bns by permit revision
                    sBIN = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                    sBnsStat = StringUtilities.HandleApostrophe(pSet.GetString("bns_stat"));
                    sBnsDec = sBnsDec + "(" + sBnsStat + ")";
                    sGrossCap = "0.00";
                    if (sBnsStat == "NEW")
                        sGrossCap = pSet.GetDouble("bns_capital").ToString("#,##0.00");
                    else
                        sGrossCap = pSet.GetDouble("bns_gross").ToString("#,##0.00");

                    //sGrossCap = string.Format("%0.2f", atof(sGrossCap));
                    sTax = pSet.GetDouble("btax").ToString("#,##0.00");
                    sFees = pSet.GetDouble("fees").ToString("#,##0.00");
                    sSurInt = pSet.GetDouble("sur_int").ToString("#,##0.00");
                    //sTax.Format("%0.2f", atof(sTax));
                    //sFees.Format("%0.2f", atof(sFees));
                    //sSurInt.Format("%0.2f", atof(sSurInt));

                    sOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                    if (sOrNo.Trim() == String.Empty || sOrNo == null)
                        sOrDate = "";
                    else
                        sOrDate = StringUtilities.HandleApostrophe(pSet.GetString("or_date"));
                    sTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));
                    sTerm = sTaxYear + " ";
                    sTerm += StringUtilities.HandleApostrophe(pSet.GetString("payment_term")) + "-";
                    sTerm += StringUtilities.HandleApostrophe(pSet.GetString("qtr_paid"));

                    sContent = "<2000|<1300|<2130|<2430|<2130|<2010|>1500|>1000|>1000|>800|^800|^1000|^1000;";
                    sContent += sBIN + "|" + sPermitNo + "|" + sOwnName + "|" + sBnsName + "|" + sBnsAdd + "|" + sBnsDec + "|" + sGrossCap + "|" + sTax + "|" + sFees + "|" + sSurInt + "|" + sOrNo + "|" + sOrDate + "|" + sTerm;
                    this.axVSPrinter1.Table = sContent;

                    sQuery = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}'", sBIN, sTaxYear);
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                    {
                        while (pSet1.Read())
                        {
                            sBnsStat = StringUtilities.HandleApostrophe(pSet1.GetString("bns_stat"));
                            sBnsDec = AppSettingsManager.GetBnsDesc(pSet1.GetString("bns_code_main"));
                            sBnsDec = sBnsDec + "(" + sBnsStat + ")";
                            sGrossCap = "0.00";
                            if (sBnsStat == "NEW")
                                sGrossCap = pSet1.GetDouble("capital").ToString("#,##0.00");
                            else
                                sGrossCap = pSet1.GetDouble("gross").ToString("#,##0.00");

                            //sGrossCap.Format("%0.2f", atof(sGrossCap));

                            sContent = "<2000|<1300|<2130|<2430|<2130|<2010|>1500|>1000|>1000|>800|^800|^1000|^1000;|||||" + sBnsDec + "|" + sGrossCap + "||||||"; // ALJ 01222008 list of bns by permit revision
                            this.axVSPrinter1.Table = sContent;
                        }
                    }
                    pSet1.Close();
                }
            }
            pSet.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<2500|<1000;Total No. of Businesses :|" + iTotalRecCount.ToString("#,##0"); ;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<1200|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<1200|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ListBnsQtrDatePaid() //MCR 20131226
        {
            String sQuery, sBlank, sContent, sTotalRecCount, sTotalMain, sTotalAddl, sGrandContent;
            String sBin, sBnsNm, sBnsAdd, sOwnNm, sOrNo, sOrDate, sTaxYear, sQtr, sBnsStat, sBnsDesc;
            int iTotalRecCount = 0, iTotalMain = 0;
            double dGrCap = 0, dBTax = 0, dFees = 0, dPen = 0, dTotal = 0;
            String sTempGrCap, sTempBTax, sTempFees, sTempPenalty, sTempTotal;
            String sGrandGrCap = "", sGrandBTax = "", sGrandPen = "", sGrandFees = "", sGrandTotal = "";

            //mc_vspReports.SetOrientation(0); this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            sQuery = string.Format("select distinct bin,bns_nm,bns_addr,subgroup1,or_no,or_date from rep_list_bns where report_name = '{0}' and user_code = '{1}' order by bin,or_date", m_sReportName, StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iTotalMain++;

                    sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                    sBnsNm = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                    sBnsAdd = StringUtilities.HandleApostrophe(pSet.GetString("bns_addr"));
                    sOwnNm = StringUtilities.HandleApostrophe(pSet.GetString("subgroup1"));
                    sOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                    if (sOrNo == String.Empty)
                        sOrDate = "";
                    else
                        sOrDate = StringUtilities.HandleApostrophe(pSet.GetString("or_date"));

                    this.axVSPrinter1.Table = "^1700|<2000|<2000|<2000|<1000|<1000|^800|^700|^800|<2000|>1200|>1200|>1200|>1200|>1200;" + sBin + "|" + sBnsNm + "|" + sBnsAdd + "|" + sOwnNm + "|" + sOrNo + "|" + sOrDate + "|||||||||";
                    // this.axVSPrinter1.Table = sContent;

                    sQuery = string.Format("select tax_year,qtr_paid,bns_stat,subgroup2,bns_gross,btax,fees,sur_int,bns_capital from rep_list_bns where report_name = '{0}' and user_code = '{1}' and bin = '{2}' and or_no = '{3}'", m_sReportName, StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode), sBin, sOrNo);
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                    {
                        while (pSet1.Read())
                        {
                            iTotalRecCount++;

                            sTaxYear = StringUtilities.HandleApostrophe(pSet1.GetString("tax_year"));
                            sQtr = StringUtilities.HandleApostrophe(pSet1.GetString("qtr_paid"));
                            sBnsStat = StringUtilities.HandleApostrophe(pSet1.GetString("bns_stat"));
                            sBnsDesc = StringUtilities.HandleApostrophe(pSet1.GetString("subgroup2"));

                            sTempGrCap = string.Format("{0:##0.00}", pSet1.GetDouble("bns_gross"));
                            sTempBTax = string.Format("{0:##0.00}", pSet1.GetDouble("btax"));
                            sTempFees = string.Format("{0:##0.00}", pSet1.GetDouble("fees"));
                            sTempPenalty = string.Format("{0:##0.00}", pSet1.GetDouble("sur_int"));
                            sTempTotal = string.Format("{0:##0.00}", pSet1.GetDouble("bns_capital"));

                            dGrCap += Convert.ToDouble(sTempGrCap);
                            dBTax += Convert.ToDouble(sTempBTax);
                            dFees += Convert.ToDouble(sTempFees);
                            dPen += Convert.ToDouble(sTempPenalty);
                            dTotal += Convert.ToDouble(sTempTotal);

                            this.axVSPrinter1.Table = "^1700|<2000|<2000|<2000|<1000|<1000|^800|^700|^800|<2000|>1200|>1200|>1200|>1200|>1200;||||||" + sTaxYear + "|" + sQtr + "|" + sBnsStat + "|" + sBnsDesc + "|" + sTempGrCap + "|" + sTempBTax + "|" + sTempFees + "|" + sTempPenalty + "|" + sTempTotal;
                        }
                    }
                    pSet1.Close();
                }
            }
            pSet.Close();

            //_variant_t xStart, xEnd, yGrid, yEnd;

            sTotalMain = iTotalMain.ToString("#,##0");
            sTotalAddl = (iTotalRecCount - iTotalMain).ToString("#,##0");
            sTotalRecCount = iTotalRecCount.ToString("#,##0");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //yGrid = this.axVSPrinter1.CurrentY();
            //CreateLine(700, 20700, yGrid, yGrid);

            sGrandGrCap = string.Format("{0:#,##0.00}", dGrCap);
            sGrandBTax = string.Format("{0:#,##0.00}", dBTax);
            sGrandFees = string.Format("{0:#,##0.00}", dFees);
            sGrandTotal = string.Format("{0:#,##0.00}", dTotal);
            sGrandPen = string.Format("{0:#,###.00}", dPen);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^1700|<2000|<2000|<2000|<1000|<1000|^800|^700|^800|<2000|>1200|>1200|>1200|>1200|>1200;|||||||||GRAND TOTAL:|" + sGrandGrCap + "|" + sGrandBTax + "|" + sGrandFees + "|" + sGrandPen + "|" + sGrandTotal;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = "<3000|<1000;Total No. of Main Businesses :|" + sTotalMain;
            this.axVSPrinter1.Table = "<3000|<1000;Total No. of Additional Businesses :|" + sTotalAddl;
            this.axVSPrinter1.Table = "<3000|<1000;Total No. of Businesses :|" + sTotalRecCount;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void CDOReport()
        {
            int iCount = 0;
            string sData = string.Empty;
            string sBnsPin = string.Empty;
            string sBldgCode = string.Empty;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            result.Query = "select * from CDO_REPORT where sys_user = :1";
            result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
            if (result.Execute())
            {
                while (result.Read())
                {
                    result2.Query = "select * from btm_gis_loc where bin = '" + result.GetString("bin").Trim() + "'";
                    if (result2.Execute())
                    {
                        if (result2.Read())
                        {
                            sBnsPin = result2.GetString("land_pin");
                            sBldgCode = result2.GetString("bldg_code");
                            if (sBldgCode.Trim() != string.Empty)
                                sBnsPin = "LAND PIN: " + sBnsPin + "-1" + sBldgCode;
                            else
                                sBnsPin = "LAND PIN: " + sBnsPin;
                        }

                    }
                    result2.Close();

                    iCount = iCount + 1;
                    sData = iCount.ToString() + "|" + result.GetString("bin") + "|" + result.GetString("bns_nm") + "|" + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(result.GetString("bin"))) + "|" + string.Format("{0:dd-MMM-yyyy}", result.GetDateTime("tag_date")) + ";";
                    this.axVSPrinter1.Table = string.Format("^500|^2000|<3800|<2800|^2400;{0}", sData + ";");
                    this.axVSPrinter1.FontItalic = true;
                    this.axVSPrinter1.Table = string.Format("<1000|<10500;|" + sBnsPin + "       --" + AppSettingsManager.GetBnsAdd(result.GetString("bin"), "") + "||;;");
                    this.axVSPrinter1.FontItalic = false;
                }
            }
            result.Close();


            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<11500;Total Number of Business/es :" + string.Format("{0:#,###}", iCount));

        }

        private void BSPReport()
        {
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            int iCount = 0;
            string sData = string.Empty;
            string sBnsPin = string.Empty;
            string sBldgCode = string.Empty;
            string sRetDt = string.Empty;
            string sBnsCode = string.Empty;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            result.Query = "select * from businesses where bns_code like '08%' and (bns_code <> '0801' and bns_code <> '0810') order by bns_nm";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sBnsCode = result.GetString("bns_code").Trim();
                    result2.Query = "select * from retired_bns where bin = '" + result.GetString("bin").Trim() + "'";
                    if (result2.Execute())
                    {
                        if (result2.Read())
                            sRetDt = string.Format("{0:dd-MMM-yyyy}", result.GetDateTime("apprvd_date"));
                        else
                            sRetDt = string.Empty;
                    }
                    result2.Close();

                    iCount = iCount + 1;
                    //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
                    //this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
                    sData = iCount.ToString() + "|" + result.GetString("bns_nm") + "|" + AppSettingsManager.GetBnsAdd(result.GetString("bin"), "") + "|" + result.GetString("bns_telno").Trim() + "|" + string.Format("{0:dd-MMM-yyyy}", result.GetDateTime("dt_applied")) + "|" + string.Format("{0:dd-MMM-yyyy}", result.GetDateTime("save_tm")) + "|" + sRetDt + "|" + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(result.GetString("bin"))) + "|" + result.GetString("tin_no").Trim() + "| | | ";
                    this.axVSPrinter1.Table = string.Format("^500|<3000|<2000|<1000|<1000|<1000|<1000|<2000|<1000|<1000|<1000|<1000;{0}", sData + ";");
                }
            }
            result.Close();


            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<11500;Total Number of Business/es :" + string.Format("{0:#,###}", iCount));

        }

        private void PrintNotice()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sOwnCode = "";
            string sData = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            string sDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            this.axVSPrinter1.Table = string.Format(">9000;{0}", sDate);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            sOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetBnsOwner(sOwnCode));
            this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetBnsOwnAdd(sOwnCode));
            this.axVSPrinter1.Table = string.Format("<9000;Business Name: {0}", AppSettingsManager.GetBnsName(m_sBIN));

            this.axVSPrinter1.MarginLeft = 8500;  // JHMN 20170110 added report name for notice of renewal (s)
            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.Table = string.Format("=10000;;;{0}", m_sReportName);

            this.axVSPrinter1.MarginLeft = 1000;
            // JHMN 20170110 added for report name of notice (e)

            this.axVSPrinter1.Table = string.Format("<9000;;;Sir/Madam:");

            sData = "Please be advised that based on the records of this office, your establishment fails to renew ";
            sData += "your permit to operate your business. Accordingly, we are requesting you to facilitate your renewal ";
            sData += "as soon as possible to avoid inconvenience and payment of penalties/surcharges.";
            this.axVSPrinter1.IndentTab = "0.25in";
            this.axVSPrinter1.IndentFirst = "0.25in";
            this.axVSPrinter1.Table = string.Format("=9000;;{0}", sData);
            sData = "Kindly disregard this notice in case your application for renewal is already being processed.";
            this.axVSPrinter1.IndentTab = "0.25in";
            this.axVSPrinter1.IndentFirst = "0.25in";
            this.axVSPrinter1.Table = string.Format("=9000;;{0}", sData);


            
            this.axVSPrinter1.IndentFirst = "0.0in";
            this.axVSPrinter1.Table = string.Format("<9000;;;Respectfully Yours,");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Table = string.Format("<4000;{0}", AppSettingsManager.GetConfigValue("38")); //JARS 20170929 BPLD HEAD
            //this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetConfigObject("37
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("38")); //JARS 20170929 BPLD HEAD // JAV 20171003 align center
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigObject("37"));// JAV 20171003 align center
            //this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetConfigValue("41"));
        }

        private void CreateHeader()
        {
            string strProvinceName = string.Empty;

            //this.axVSPrinter1

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            if (m_bUsePreprinted)   // RMC 20120220 added option for using pre-printed form in Certification (s)
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                // RMC 20120220 added option for using pre-printed form in Certification (e)
            }
            else
            {
                this.axVSPrinter1.FontName = "Arial Narrow";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
                this.axVSPrinter1.FontBold = true;
                if (m_sReportSwitch == "Certification" || m_sReportSwitch == "Retirement")
                    this.axVSPrinter1.FontSize = (float)14.0;
                else
                    //this.axVSPrinter1.FontSize = (float)10.0;
                    this.axVSPrinter1.FontSize = (float)12.0;
                this.axVSPrinter1.FontBold = false;

                //MOD MCR 20140618 (s)
                string sSize = string.Empty;
                if (m_sReportSwitch == "CDO Report")
                    sSize = "11500";
                else if (m_sReportSwitch == "BSP Report")
                    sSize = "15500";
                else if (m_sReportSwitch == "ListBnsQtrDatePaid")
                    sSize = "20000";
                else if (m_sReportSwitch == "ListBnsPrmtNum")
                    sSize = "19100";
                else if (m_sReportSwitch == "ListBnsBrgy" || m_sReportSwitch == "ListBnsGrossReceipt"
                    || m_sReportSwitch == "ListBnsBusStatus" || m_sReportSwitch == "ListBnsOrgKind"
               || m_sReportSwitch == "ListBnsMainBusiness" || m_sReportSwitch == "ListOfEarlyBird"
               || m_sReportSwitch == "Re-assessment")    // RMC 20170221 modifications in Early bird report //MOD MCR 20170530 re-assessment
                    sSize = "19700";
                else if (m_sReportSwitch == "ListBnsDist")
                    sSize = "17700";
                else if (m_sReportSwitch == "RetirementReport")
                    sSize = "15350";
                else if (m_sReportSwitch == "ListBnsOwnBday")
                    sSize = "11800";
                else if(m_sReportSwitch == "WithBussPermit")
                    sSize = "11000";
                else if(m_sReportSwitch == "BussCert")
                    //sSize = "15350";
                    sSize = "11000";    // RMC 20171128 adjustment in certificate of status and With Business
                else
                    sSize = "11000";

                if (m_sReportSwitch == "BiggestInvestment") //AFM 20200306
                {
                    this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "3.65in", ".32in", "25%", "25%", 10, false);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                }
                strProvinceName = AppSettingsManager.GetConfigValue("08");
                if (m_sReportSwitch == "SanitaryClearance") //AFM 20200103
                {
                    this.axVSPrinter1.Table = string.Format("^8000;                                Republic of the Philippines", sSize);
                }
                else if(m_sReportSwitch == "ZoningClearance") //AFM 20210812 for v2
                {
                    long y1 = Convert.ToInt32(this.axVSPrinter1.CurrentY);
                    long y2;
                    this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "0.65in", "0.62in", "25%", "25%", 10, false);
                    this.axVSPrinter1.FontSize = (float)13.0;
                    this.axVSPrinter1.FontName = "Calibri";
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = "<7500;Republic of the Philippines";
                    this.axVSPrinter1.FontBold = false;

                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = string.Format("<7500;Province of {0}", strProvinceName);
                    this.axVSPrinter1.FontBold = false;

                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = (float)15.0;
                    this.axVSPrinter1.Table = string.Format("<7500;{0}", AppSettingsManager.GetConfigValue("09"));

                    y2 = Convert.ToInt32(this.axVSPrinter1.CurrentY);
                    this.axVSPrinter1.CurrentY = y1;
                    this.axVSPrinter1.DrawPicture(Properties.Resources.zoning_dept_logo, "4.55in", "0.62in", "90%", "90%", 10, false); //placeholder. no icon given yet
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = (float)8.0;
                    this.axVSPrinter1.Table = string.Format(">13700|<10000;|CITY PLANNING AND DEVELOPMENT OFFICE");
                    this.axVSPrinter1.FontSize = (float)7.5;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Table = string.Format(">13700|<10000;|3rd Floor, New City Hall, Bulihan");
                    this.axVSPrinter1.Table = string.Format(">13700|<10000;|City of Malolos, Bulacan");
                    this.axVSPrinter1.Table = string.Format(">13700|<10000;|Email: maloloscpdo@yahoo.com");
                    this.axVSPrinter1.Table = string.Format(">13700|<10000;|Cellphone No. 09108373772");

                    this.axVSPrinter1.CurrentY = y2;
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
                    this.axVSPrinter1.Table = string.Format("^10500; ");
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";

                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontSize = (float)22.0;
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = "^10500; ZONING CLEARANCE";
                    this.axVSPrinter1.Paragraph = "";
                }
                else 
                    this.axVSPrinter1.Table = string.Format("^{0};Republic of the Philippines", sSize);
                //if (m_sReportSwitch != "Retirement")
                if (m_bUsePreprinted == false)  // RMC 20171114 corrections in certification printing
                {
                    if (strProvinceName != string.Empty)
                        if (m_sReportSwitch == "SanitaryClearance")
                        {
                            this.axVSPrinter1.Table = string.Format("^8000;                                PROVINCE OF {0}", strProvinceName.ToUpper(), sSize); //AFM 20200103
                        }
                        else if (m_sReportSwitch != "ZoningClearance")
                            this.axVSPrinter1.Table = string.Format("^{1};PROVINCE OF {0}", strProvinceName.ToUpper(), sSize); // AST 20150429

                }
                if(m_sReportSwitch == "SanitaryClearance") //AFM 20200103
                    this.axVSPrinter1.FontBold = false;
                else
                    this.axVSPrinter1.FontBold = true;

                if (m_sReportSwitch == "SanitaryClearance") //AFM 20200103
                    this.axVSPrinter1.Table = string.Format("^6000;                     {0}", AppSettingsManager.GetConfigValue("09"), sSize);
                else if (m_sReportSwitch != "ZoningClearance")
                    this.axVSPrinter1.Table = string.Format("^{1};{0}", AppSettingsManager.GetConfigValue("09"), sSize);
                //this.axVSPrinter1.FontBold = false;
                //JARS 20170817(S)
                //if (m_sReportSwitch != "Retirement")  

                if (m_sReportSwitch == "EPSClearance")
                    this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "0.8in", ".52in", "35%", "35%", 10, false); //AFM 20200103
                if (m_sReportSwitch == "SanitaryClearance")
                    this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "2.4in", ".42in", "35%", "35%", 10, false); //AFM 20200103

                if (m_bUsePreprinted == false)  // RMC 20171114 corrections in certification printing
                {
                    if (m_sReportSwitch == "EPSClearance") //AFM 20200102
                        this.axVSPrinter1.Table = string.Format("^{1};{0}", "CITY ENGINEER'S OFFICE", sSize);
                    else if (m_sReportSwitch == "SanitaryClearance")
                        this.axVSPrinter1.Table = string.Format("^10000;                                                         {0}", "OFFICE OF THE " + AppSettingsManager.GetConfigValue("01") + " HEALTH OFFICER", sSize); //AFM 20200103
                    else if (m_sReportSwitch == "EPSTrail")
                        this.axVSPrinter1.Table = "";
                    else if (m_sReportSwitch == "BiggestInvestment")
                    {
                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.FontSize = 16f;
                        this.axVSPrinter1.FontName = "Times New Roman";
                        this.axVSPrinter1.Table = string.Format("^15000;{0}", "PROVINCIAL COOPERATIVE AND ENTERPRISE DEVELOPMENT OFFICE");
                    }
                    else if (m_sReportSwitch != "ZoningClearance")
                        this.axVSPrinter1.Table = string.Format("^{1};{0}", "OFFICE OF THE " + AppSettingsManager.GetConfigValue("01") + " ADMINISTRATOR", sSize);
                }
                if (m_sReportSwitch == "EPSClearance") //AFM 20200102
                {
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 15f;
                    this.axVSPrinter1.Table = string.Format("^{1};{0}", "BUSINESS CLEARANCE", sSize);
                }
                else if (m_sReportSwitch == "SanitaryClearance") //AFM20200103
                {
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 18f;
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = string.Format("^{1};{0}", "SANITARY PERMIT TO OPERATE", sSize);
                    this.axVSPrinter1.FontBold = false;
                }
                else if (m_sReportSwitch == "EngineeringReport") //AFM 20200121
                {
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 15f;
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = string.Format("^{1};{0}", "ENGINEERING CLEARANCE REPORT", sSize);
                    this.axVSPrinter1.FontBold = false;
                }
                else if (m_sReportSwitch == "EPSTrail") //AFM 20210726
                {
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 15f;
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = string.Format("^{1};{0}", "Trail Log", sSize);
                    this.axVSPrinter1.FontBold = false;
                }
                else if (m_sReportSwitch == "SanitaryReport") //AFM 20200130
                {
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 15f;
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = string.Format("^{1};{0}", "SANITARY CLEARANCE REPORT", sSize);
                    this.axVSPrinter1.FontBold = false;
                }
                else if (m_sReportSwitch == "BiggestInvestment") //AFM 20200306
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 13f;
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontName = "Arial";
                    this.axVSPrinter1.Table = string.Format("^{1};{0}", "Biggest Investment (NEW)", sSize);
                    this.axVSPrinter1.FontBold = false;
                    if(m_sTaxYear != "%")
                        this.axVSPrinter1.Table = string.Format("^{1};{0}", m_sTaxYear, sSize);
                }
                else if(m_sReportSwitch != "ZoningClearance") 
                    this.axVSPrinter1.Table = string.Format("^{1};{0}", "BUSINESS PERMIT AND LICENSING DIVISION", sSize);
                //MOD MCR 20140618 (e)
                this.axVSPrinter1.FontSize = 12;
                if (m_sReportSwitch == "Retirement" || m_sReportSwitch == "Renewal") //JARS 20170929 PLACE REPORTS HERE THAT NEED MALOLOS LOGO
                {
                    //axVSPrinter1.DrawPicture(Properties.Resources.back_white, "1in", "0.5in", "110%", "110%", 10, false); //MCR 20161221 image path, x, y, witdh, height, shade
                    //this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "1.0in", ".62in", "25%", "25%", 10, false);
               
                }
                //JARS 20170817(E)

            }
            if (m_sReportSwitch != "ZoningClearance")
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
            }

            this.axVSPrinter1.FontName = "Arial";
            // RMC 20120329 Modifications in Notice of violation (S)
            if (m_sReportSwitch == "Notice List")
            {
                // RMC 20120417 Modified Final notice format (s)
                if (m_sOwnCode == "1")
                    this.axVSPrinter1.Table = string.Format("^9000;List of Unofficial Businesses without Notice");
                else if (m_sOwnCode == "2")
                    this.axVSPrinter1.Table = string.Format("^9000;List of Unofficial Businesses with Inspection Notice");
                else if (m_sOwnCode == "3")
                    this.axVSPrinter1.Table = string.Format("^9000;List of Unofficial Businesses with Second Notice");
                else if (m_sOwnCode == "4")
                    this.axVSPrinter1.Table = string.Format("^9000;List of Unofficial Businesses with Final Notice");
                else
                    this.axVSPrinter1.Table = string.Format("^9000;List of Unofficial Businesses tagged for Closure");
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                // RMC 20120417 Modified Final notice format (e)

                //this.axVSPrinter1.Table = string.Format("^2300|^4000|^4000;TEMPORARY BIN|BUSINESS NAME|OWNER'S NAME"); // GDE 20120508
                this.axVSPrinter1.Table = string.Format("^1500|^3800|^2800|^1200|^1200;TEMPORARY BIN|BUSINESS NAME|OWNER'S NAME|DATE ISSUED|DATE RECEIVED");
                this.axVSPrinter1.Paragraph = "";
            }
            // RMC 20120329 Modifications in Notice of violation (E)
            // GDE 20120530 ADDED
            else if (m_sReportSwitch == "CDO Report")
            {
                this.axVSPrinter1.Table = string.Format("^11500;List of Businesses tagged for CDO");
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                // RMC 20120417 Modified Final notice format (e)

                //this.axVSPrinter1.Table = string.Format("^2300|^4000|^4000;TEMPORARY BIN|BUSINESS NAME|OWNER'S NAME"); // GDE 20120508
                this.axVSPrinter1.Table = string.Format("^500|^2000|^3800|^2800|^2400;#|BIN/TEMPORARY BIN|BUSINESS NAME / ADDRESS|OWNER'S NAME|DATE TAGGED");
                this.axVSPrinter1.Paragraph = "";

            }
            else if (m_sReportSwitch == "BSP Report")
            {
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
                this.axVSPrinter1.Table = string.Format("^15500;List of Businesses for BSP");
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                // RMC 20120417 Modified Final notice format (e)

                this.axVSPrinter1.FontSize = (float)8.0;
                this.axVSPrinter1.FontBold = true;
                //this.axVSPrinter1.Table = string.Format("^2300|^4000|^4000;TEMPORARY BIN|BUSINESS NAME|OWNER'S NAME"); // GDE 20120508
                this.axVSPrinter1.Table = string.Format("^500|^5000|^1000|^1000|^1000|^1000|^2000|^1000|^1000|^1000|^1000;#|Business Name / Address|Tel No.|Date Applied|Date Encoded|Date Retired|Owner Name|Tin No.|FXD|HO|Remarks");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch == "Violation" /*|| m_sReportSwitch == "Closure"*/) //JARS 20170908
            {
                axVSPrinter1.FontSize = (float)8.0;// GDE 20120516 as per jester
                axVSPrinter1.Footer = "Note: Please disregard this letter if you already have a Business Permit issued by the " + AppSettingsManager.GetConfigValue("02") + " Business Permits & Licensing Office";
                axVSPrinter1.FontSize = (float)10.0;// GDE 20120516 as per jester
            }   
            else
            {
                if (m_sReportSwitch.Contains("ListBnsOwnBday"))
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^11800;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">11800;" + "Page " + sPage);
                    this.axVSPrinter1.FontSize = 8;
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^800|^1900|^3000|^3000|^2000|^1100;|BIN|Taxpayer Name|Business Name|Contact No.|Birth Date");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch.Contains("ListBnsQtrDatePaid"))
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^20000;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">20000;" + "Page " + sPage);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^1700|^2000|^2000|^2000|^1000|^1000|^800|^700|^800|^2000|^1200|^1200|^1200|^1200|^1200;Bin|Business Name|Address|Taxpayer Name|O.R. No.|O.R. Date|T.Y.|Qtr|Stat|Business Nature|GR/Cap|Tax|Fees|Penalty|Total");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch.Contains("ListBnsPrmtNum"))
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^19100;" + m_sReportName);
                    if (m_sData1 != String.Empty)
                    {
                        this.axVSPrinter1.Table = ("^19100;" + m_sData1);
                    }
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    this.axVSPrinter1.Table = ("^19100;BRGY:" + m_sBrgyName);
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">19100;" + "Page " + sPage);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    //this.axVSPrinter1.Table = ("^1700|^2000|^2000|^2000|^1000|^1000|^800|^700|^800|^2000|^1200|^1200|^1200|^1200|^1200;BIN|BUSINESS NAME|ADDRESS|TAXPAYER NAME|OR NO|OR DATE|T.Y.|QTR|STAT|BUSINESS NATURE|GR/CAP|TAX|FEES|PENALTY|TOTAL");
                    this.axVSPrinter1.Table = ("^2000|^1300|^2130|^2430|^2130|^2010|^1500|^1000|^1000|^800|^800|^1000|^1000;Bin|Permit#|Owner|Business Name|Address|Classification|Gross/Cap|Btax|Fees|Int|O.R. No.|O.R. Date|Term");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch == "ListBnsGrossReceipt")
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^19700;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">19700;" + "Page " + sPage);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^1850|^2000|^1000|^1500|^1400|^1050|^1200|^1200|^1200|^1200|^1000|^1000|^600|^700|^600|^1200|^1000;Bin|Business Name/\nOwner's Name|Code/Stat|Gross Receipts|Capital|Permit #|Business Tax|Fees And Charges|Surcharge & Interest|Total|O.R. No.|O.R. Date|Term|Tax Year|Qtr Paid|Contact No.|No. of Employees");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch == "ListBnsBusStatus")
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^19700;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">19700;" + "Page " + sPage);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^1850|^2000|^1000|^1500|^1400|^1050|^1200|^1200|^1200|^1200|^1000|^1000|^600|^700|^600|^1200|^1000;Bin|Business Name/\nOwner's Name|Code/Stat|Gross Receipts|Capital|Permit #|Business Tax|Fees And Charges|Surcharge & Interest|Total|O.R. No.|O.R. Date|Term|Tax Year|Qtr Paid|Contact No.|No. of Employees");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch == "ListBnsOrgKind")
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^19700;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">19700;" + "Page " + sPage);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    //JARS 20170302
                    this.axVSPrinter1.Table = ("^1550|^2000|^1000|^1500|^1400|^1050|^1200|^1200|^1200|^1200|^1000|^1000|^600|^700|^600|^1200|^500;Bin|Business Name/\nOwner's Name|Code/Stat|Gross Receipts|Capital|Permit #|Business Tax|Fees And Charges|Surcharge & Interest|Total|O.R. No.|O.R. Date|Term|Tax Year|Qtr Paid|Contact No.|No. of Emp");
                    //this.axVSPrinter1.Table = ("^1850|^2000|^1000|^1500|^1400|^1050|^1200|^1200|^1200|^1200|^1000|^1000|^600|^700|^600|^1200|^1000;Bin|Business Name/\nOwner's Name|Code/Stat|Gross Receipts|Capital|Permit #|Business Tax|Fees And Charges|Surcharge & Interest|Total|O.R. No.|O.R. Date|Term|Tax Year|Qtr Paid|Contact No.|No. of Employees");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch == "ListBnsMainBusiness")
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^19700;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">19700;" + "Page " + sPage);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^1850|^2000|^1000|^1500|^1400|^1050|^1200|^1200|^1200|^1200|^1000|^1000|^600|^700|^600|^1200|^1000;Bin|Business Name/\nOwner's Name|Code/Stat|Gross Receipts|Capital|Permit #|Business Tax|Fees And Charges|Surcharge & Interest|Total|O.R. No.|O.R. Date|Term|Tax Year|Qtr Paid|Contact No.|No. of Employees");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch == "ListBnsDist")
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^17700;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">17700;" + "Page " + sPage);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^1700|^2000|^1200|^1500|^1200|^1000|^1000|^1200|^1200|^1200|^1200|^1200|^700|^700|^700;Bin|Business Name/\nOwner's Name|Code/Stat|Gross Receipts|Capital|Permit #|Business Tax|Fees And Charges|Surcharge & Interest|Total|O.R. No.|O.R. Date|Term|Tax Year|Qtr Paid");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch == "ListBnsBrgy")
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^19700;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">19700;" + "Page " + sPage);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^1850|^2000|^1000|^1500|^1400|^1050|^1200|^1200|^1200|^1200|^1000|^1000|^600|^700|^600|^1200|^1000;Bin|Business Name/\nOwner's Name|Code/Stat|Gross Receipts|Capital|Permit #|Business Tax|Fees And Charges|Surcharge & Interest|Total|O.R. No.|O.R. Date|Term|Tax Year|Qtr Paid|Contact No.|No. of Employees");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch == "RetirementReport")
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^15350;" + m_sReportName);

                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">15350;" + "Page " + sPage);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^1850|^2000|^2000|^1500|^500|^2000|^1000|^1000|^1000|^1500|^1000;Bin|Business Name|Business Address|Owner's Name|F/P|Reason For Retirement|Date App'd|Ret. No.|O.R. No.|Business Type|Gross / Pay't");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }// RMC 20170221 modifications in Early bird report (s)
                else if (m_sReportSwitch == "ListOfEarlyBird")
                {
                    string sDate = string.Format("{0:MMMM dd, yyyy}", m_dtDate);

                    this.axVSPrinter1.FontSize = (float)10.0;
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = string.Format("^10000;{0}", m_sReportName);
                    this.axVSPrinter1.Table = string.Format("^5000;;As of {0}", sDate);

                    this.axVSPrinter1.MarginLeft = 1000;
                    this.axVSPrinter1.Table = string.Format("^6000|^4000;;TOP: {0} EARLY BIRD TAXPAYER|TAX YEAR: {1}", m_sTop, m_sTaxYear);

                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = string.Format("^3000|^4000|^4000|^3000|^2500;BIN|BUSINESS NAME|OWNER'S NAME|AMOUNT PAID|OR DATE");

                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                }
                // RMC 20170221 modifications in Early bird report (e)
                else if (m_sReportSwitch == "Re-assessment") //MCR 20170530 (s) re-assessment
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^19700;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    String sPage = this.axVSPrinter1.PageCount.ToString();
                    this.axVSPrinter1.Table = (">19700;" + "Page " + sPage);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^1850|^3900|^3900|^3900|^3900|^1050|^1200;Bin|Business Name|Owner's Name|Business Address|Owner's Address|Gross|Tax Year");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                } //MCR 20170530 (e) re-assessment
                else if (m_sReportSwitch == "Comparative Report")   // RMC 20180209 modified comparative report as requested by Malolos BPLO (format proposed by Jester)
                {
                    this.axVSPrinter1.Table = ("^10000;" + m_sReportSwitch);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 10;
                }
                else if (m_sReportSwitch == "ListEngineeringClearance") //AFM 20191218 based on binan
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^11000;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    this.axVSPrinter1.Table = ("^11000;From: " + m_dFrom.ToString("MMM dd, yyyy") + " To :" + m_dTo.ToString("MMM dd,yyyy"));
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^2000|^3000|^3000|^1500|^1500;BIN|Business Name|Address|Control Series|Date Issued");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch == "ListZoningClearance") //AFM 20191218 based on binan
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Table = ("^11000;" + m_sReportName);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    this.axVSPrinter1.Table = ("^11000;From: " + m_dFrom.ToString("MMM dd, yyyy") + " To :" + m_dTo.ToString("MMM dd,yyyy"));
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^2000|^3000|^3000|^1500|^1500;BIN|Business Name|Address|Control Series|Date Issued");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch == "EngineeringReport") //AFM 20200121
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    this.axVSPrinter1.Table = ("^11000;From: " + m_dFrom.ToString("MMM dd, yyyy") + " To :" + m_dTo.ToString("MMM dd,yyyy"));
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^2000|^3000|^1000|^1000|^2500|^1000|^700|^1000|^1000|^3000;BIN|Business Name|Control Number|Status|Owner|Date Issued|Tax Year|OR No.|Amount|Remarks");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else if (m_sReportSwitch == "SanitaryReport") //AFM 20200130
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;
                    this.axVSPrinter1.Table = ("^11000;From: " + m_dFrom.ToString("MMM dd, yyyy") + " To :" + m_dTo.ToString("MMM dd,yyyy"));
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    this.axVSPrinter1.Table = ("^2000|^3000|^2500|^3000|^1000;BIN|Business Name|Owner|Address|Date Issued");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }

                if (m_sReportSwitch != "Certification")
                    axVSPrinter1.Footer = AppSettingsManager.GetConfigValue("16");
            }
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
            if (!m_bInit)
                CreateHeader();

            m_bInit = false;
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

        private void PrintNoticeForReassess()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sOwnCode = "";
            string sData = "";

            string sDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            this.axVSPrinter1.Table = string.Format(">9000;{0}", sDate);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            sOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetBnsOwner(sOwnCode));
            this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetBnsOwnAdd(sOwnCode));
            this.axVSPrinter1.Table = string.Format("<9000;Business Name: {0}", AppSettingsManager.GetBnsName(m_sBIN));

            this.axVSPrinter1.Table = string.Format("<9000;;;Sir/Madam:");

            sData = "Please be advised that based on the records of this office, your establishment is included ";
            sData += "in the list of businesses whose permits are for re-assessment. Accordingly, we are requesting for ";
            sData += "the submission of your audited financial statement to facilitate our computation.";
            this.axVSPrinter1.IndentTab = "0.25in";
            this.axVSPrinter1.IndentFirst = "0.25in";
            this.axVSPrinter1.Table = string.Format("=9000;;{0}", sData);
            sData = "For your immediate compliance please.";
            this.axVSPrinter1.IndentTab = "0.25in";
            this.axVSPrinter1.IndentFirst = "0.25in";
            this.axVSPrinter1.Table = string.Format("=9000;;{0}", sData);


            this.axVSPrinter1.IndentFirst = "0.0in";
            this.axVSPrinter1.Table = string.Format("<9000;;;Respectfully Yours,");

            this.axVSPrinter1.Table = string.Format("<9000;;;{0}", AppSettingsManager.GetConfigValue("03"));
            this.axVSPrinter1.Table = string.Format("<9000;HEAD, {0}", AppSettingsManager.GetConfigValue("41"));
        }

        private void PrintNoticeofViolation()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sOwnCode = "";
            string sData = "";

            string sDate = string.Format("{0:MMMM dd, yyyy}", m_dtDate);
            /*this.axVSPrinter1.Table = string.Format(">9000;{0}", sDate);*/
            sData = "5-DAYS NOTICE OF VIOLATION";
            this.axVSPrinter1.Table = string.Format("^9000;{0}", sData);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            // RMC 20120222 added printing of notice for un-official business in business mapping (s)
            if (m_sOwnCode != "")
            {
                sOwnCode = m_sOwnCode;
                this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetBnsOwner(sOwnCode));
                this.axVSPrinter1.Table = string.Format("<9000;{0}", GetUnofficialBnsAddress(m_sBIN, sOwnCode));
                this.axVSPrinter1.Table = string.Format("<9000;Business Name: {0}", m_sUnoffBnsName);
            }
            else
            {
                sOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN);
                this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetBnsOwner(sOwnCode));
                this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetBnsOwnAdd(sOwnCode));
                this.axVSPrinter1.Table = string.Format("<9000;Business Name: {0}", AppSettingsManager.GetBnsName(m_sBIN));
            }

            this.axVSPrinter1.Table = string.Format("<9000;;;Sir/Madam:");

            sData = "This has reference to the inspection/verification conducted by our Business Permits and Licensing ";
            sData += "Isnpectors on " + sDate + ", wherein you were cited for violation/s of the " + AppSettingsManager.GetConfigValue("02") + " ";
            sData += "" + AppSettingsManager.GetConfigValue("42") + " or the " + AppSettingsManager.GetConfigValue("02") + " Tax Code, ";
            sData += "and was advised to visit our office to comply with the requirements for acquiring the necessary Business Permits and Licenses.";
            this.axVSPrinter1.IndentTab = "0.25in";
            this.axVSPrinter1.IndentFirst = "0.25in";
            this.axVSPrinter1.Table = string.Format("=9000;;{0}", sData);

            sData = "In this regard, we reiterate our request for you or your authorized representative to appear before our ";
            sData += "office within five (5) days from receipt of this letter. If we fail to hear from you, we shall be constrained ";
            sData += "to refer your case to our Legal Department for the appropriate legal action. ";
            sData += "" + AppSettingsManager.GetConfigValue("16") + ".";
            this.axVSPrinter1.IndentTab = "0.25in";
            this.axVSPrinter1.IndentFirst = "0.25in";
            this.axVSPrinter1.Table = string.Format("=9000;;{0}", sData);

            this.axVSPrinter1.IndentFirst = "0.0in";
            this.axVSPrinter1.Table = string.Format("<9000;;;Very truly yours,");

            this.axVSPrinter1.Table = string.Format("<9000;;;{0}", AppSettingsManager.GetConfigValue("03"));
            this.axVSPrinter1.Table = string.Format("<9000;HEAD, {0}", AppSettingsManager.GetConfigValue("41"));


            sData = "Received by:|_________________________";
            this.axVSPrinter1.Table = string.Format("<1500|<7500;;{0}", sData);
            sData = "|Signature over printed name";
            this.axVSPrinter1.Table = string.Format("<1500|<7500;{0}", sData);
            sData = "Date Received:|_________________________";
            this.axVSPrinter1.Table = string.Format("<1500|<7500;;{0}", sData);

        }

        private void PrintCertification()
        {
            OracleResultSet pRec = new OracleResultSet();

            string sData = "";
            string sModCode = "";
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.FontSize = (float)12.0;

            this.axVSPrinter1.Paragraph = "";
            DateTime dtDate = Convert.ToDateTime(AppSettingsManager.GetCurrentDate());
            //string sDay = string.Format("{0:dd}", dtDate);
            string sDay = string.Format("{0}", dtDate.Day); // MCR 20190710 .day
            string sMonth = string.Format("{0:MMMM}", dtDate);
            string sYear = string.Format("{0:yyyy}", dtDate);

            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.Table = string.Format("^10000;{0}", "CERTIFICATION");
            this.axVSPrinter1.FontUnderline = false;

            sData = "To Whom It May Concern:";
            this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

            sData = "This is to certify that ;;";
            this.axVSPrinter1.IndentTab = "0.25in";
            this.axVSPrinter1.IndentFirst = "0.25in";
            this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

            sData = m_sOwnFN + " " + m_sOwnMI + " " + m_sOwnLN;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.Table = string.Format("^10000;;{0}", sData);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontUnderline = false;
            this.axVSPrinter1.Table = string.Format("^10000;;{0}", "of");
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.Table = string.Format("^10000;;{0}", m_sOwnAdd);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontUnderline = false;


            if (m_sOwnCode == "")    
            {
                sData = "is not registered in this office for the year " + ConfigurationAttributes.CurrentYear + ", ";
                sData += "No Business/Mayor's Permit had been issued to them as of this date. ";
                this.axVSPrinter1.IndentTab = "0.0in";
                this.axVSPrinter1.IndentFirst = "0.0in";
                this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

                sModCode = "ARCNB";
            }
            else
            {
                // with record
                sData = "is registered in this office as owner of the ff. business/es: ";
                this.axVSPrinter1.IndentTab = "0.0in";
                this.axVSPrinter1.IndentFirst = "0.0in";
                this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

                sModCode = "ARCWB";

                string sBnsNm = "";
                string sTaxYear = "";
                string sBin = "";
                string sBnsCode = "";
                pRec.Query = "select * from businesses where own_code = '" + m_sOwnCode + "' order by tax_year";
                if (pRec.Execute())
                {
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = (float)8.0;
                    sData = "BIN|Business Name|Business Nature|Business Address|Tax Year Paid";
                    this.axVSPrinter1.Table = string.Format("^1500|^3000|^1500|^3000|^1000;{0}", sData);

                    while (pRec.Read())
                    {
                        sBin = pRec.GetString("bin");
                        sBnsNm = pRec.GetString("Bns_nm");
                        sTaxYear = pRec.GetString("tax_year");
                        sBnsCode = pRec.GetString("bns_code");
                        sBnsCode = AppSettingsManager.GetBnsDesc(sBnsCode);

                        sData = sBin + "|" + sBnsNm + "|" + sBnsCode + "|" + AppSettingsManager.GetBnsAddress(sBin) + "|" + sTaxYear;
                        this.axVSPrinter1.Table = string.Format("<1500|<3000|<1500|<3000|^1000;{0}", sData);
                    }

                    this.axVSPrinter1.Paragraph = "";
                }
                pRec.Close();
                this.axVSPrinter1.FontSize = (float)12.0;
            }

            sData = "This certification is issued upon the request of ;";
            this.axVSPrinter1.IndentTab = "0.25in";
            this.axVSPrinter1.IndentFirst = "0.25in";
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.Table = string.Format("^10000;;{0}", m_sRequestBy);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontUnderline = false;
            sData = "for whatever legal purposes it may serve.";
            this.axVSPrinter1.IndentTab = "0.25in";
            //this.axVSPrinter1.IndentFirst = "0.25in";
            this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

            sData = "Issued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + ", " + AppSettingsManager.GetConfigValue("02") + ".";
            this.axVSPrinter1.IndentTab = "0.25in";
            this.axVSPrinter1.IndentFirst = "0.25in";
            this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);
            
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            
            sData = AppSettingsManager.GetConfigValue("03") + ";";
            this.axVSPrinter1.Table = string.Format(">10000;;{0}", sData);
            sData = "BPLO - HEAD"; //sData = "BPTFO-Department Head"; //MOD MCR 20143005
            this.axVSPrinter1.Table = string.Format(">10000;{0}", sData);

            // RMC 20120221 added count of printed certifications in Certifications module (s)
            pRec.Query = "select * from TRAIL_TABLE WHERE (usr_rights = 'ARCWB' or usr_rights = 'ARCNB')";
            if (pRec.Execute())
            {
                if (!pRec.Read())
                {
                    pRec.Close();
                    pRec.Query = "insert into trail_table values (";
                    pRec.Query += "'ARCNB','ASS-REPORTS-CERTIFICATIONS-NO BUSINESS')";
                    if (pRec.ExecuteNonQuery() == 0)
                    { }

                    pRec.Query = "insert into trail_table values (";
                    pRec.Query += "'ARCWB','ASS-REPORTS-CERTIFICATIONS-WITH BUSINESS')";
                    if (pRec.ExecuteNonQuery() == 0)
                    { }
                }
            }

            if (AuditTrail.InsertTrail(sModCode, "trail table", m_sOwnFN + " " + m_sOwnMI + " " + m_sOwnLN) == 0)
            { }
            // RMC 20120221 added count of printed certifications in Certifications module (e)
        }

        private string ConvertDayInOrdinalForm(string sDay)
        {
            string sLastNo = "";
            string sDayth = "";

            if (sDay == "11")
            {
                sDayth = sDay + "th";
            }
            else
            {
                sLastNo = StringUtilities.Right(sDay, 1);

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

        private string GetUnofficialBnsAddress(string sTmpBin, string sOwnCode)
        {
            OracleResultSet pTmpBin = new OracleResultSet();
            string sAddress = "";
            string sOwnHouseNo = "";
            string sOwnStreet = "";
            string sOwnBrgy = "";
            string sOwnMun = "";
            string sProv = "";
            string sZone = "";
            string sDistrict = "";

            //sAddress = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
            //sAddress = AppSettingsManager.GetBnsAddress(sTmpBin);

            if (sAddress.Trim() == "")
            {
                pTmpBin.Query = "select * from btm_temp_businesses where tbin = '" + sTmpBin + "'";
                if (pTmpBin.Execute())
                {
                    if (pTmpBin.Read())
                    {
                        sOwnHouseNo = pTmpBin.GetString("bns_house_no").Trim();
                        sOwnStreet = pTmpBin.GetString("bns_street").Trim();
                        sOwnBrgy = pTmpBin.GetString("bns_brgy").Trim();
                        sOwnMun = pTmpBin.GetString("bns_mun").Trim();

                        sProv = pTmpBin.GetString("bns_prov").Trim();
                        sZone = pTmpBin.GetString("bns_zone").Trim();
                        sDistrict = pTmpBin.GetString("bns_dist").Trim();

                        if (sProv == "." || sProv == "")
                            sProv = "";
                        else
                            sProv = ", " + sProv;
                        if (sZone == "." || sZone == "ZONE" || sZone == "")
                            sZone = "";
                        else
                            sZone = "ZONE " + sZone + " ";
                        if (sDistrict == "." || sDistrict == "")
                            sDistrict = "";
                        else
                            sDistrict = sDistrict + ", ";

                        if (sOwnHouseNo == "." || sOwnHouseNo == "")
                            sOwnHouseNo = "";
                        else
                            sOwnHouseNo = sOwnHouseNo + ",";
                        if (sOwnStreet == "." || sOwnStreet == "")
                            sOwnStreet = "";
                        else
                            sOwnStreet = sOwnStreet + ", ";
                        if (sOwnBrgy == "." || sOwnBrgy == "")
                            sOwnBrgy = "";
                        else
                            sOwnBrgy = sOwnBrgy + ", ";
                        if (sOwnMun == "." || sOwnMun == "")
                            sOwnMun = "";

                        if (sZone.Trim() == string.Empty)
                            sAddress = sOwnHouseNo + " " + sOwnStreet + " " + sOwnBrgy + " " + sDistrict + ", " + sOwnMun + " " + sProv;
                        if (sDistrict.Trim() == string.Empty)
                            sAddress = sOwnHouseNo + " " + sOwnStreet + " " + sOwnBrgy + " " + sZone + ", " + sOwnMun + " " + sProv;
                        if (sZone.Trim() == string.Empty && sDistrict.Trim() == string.Empty)
                            sAddress = sOwnHouseNo + " " + sOwnStreet + " " + sOwnBrgy + " " + sOwnMun + ", " + sProv;
                        else
                            sAddress = sOwnHouseNo + " " + sOwnStreet + " " + sOwnBrgy + " " + sZone + ", " + sDistrict + " " + sOwnMun + ", " + sProv;
                    }
                }
                pTmpBin.Close();
            }

            return sAddress;
        }

        private void PrintClosure()
        {
            // RMC 20120417 Modified Final notice format

            OracleResultSet pRec = new OracleResultSet();
            string sDateRcd = "";
            string sInspector = "";

            pRec.Query = "select * from unofficial_notice_closure where is_number = '" + m_sBIN + "'";
            // GDE 20120523
            //if(AppSettingsManager.GetConfigValue("19") == "2")
            //    pRec.Query += " and notice_number = '1'";
            //else
            //    pRec.Query += " and notice_number = '2'";
            //pRec.Query += " order by notice_sent desc";
            // GDE 20120523
            pRec.Query += " order by notice_date asc";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sDateRcd = string.Format("{0:MMMM dd, yyyy}", pRec.GetDateTime("notice_date"));
                }
            }
            pRec.Close();

            pRec.Query = "select * from btm_temp_businesses where tbin = '" + m_sBIN + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sInspector = pRec.GetString("inspected_by");
                }

            }
            pRec.Close();

            string sInsLn = "";
            string sInsFn = "";
            string sInsMi = "";

            pRec.Query = "select * from inspector where inspector_code = '" + sInspector + "'";
            if (pRec.Execute())
            {
                sInspector = "";
                if (pRec.Read())
                {
                    sInsLn = pRec.GetString("inspector_ln");
                    sInsFn = pRec.GetString("inspector_fn");
                    sInsMi = pRec.GetString("inspector_mi");

                    if (sInsFn != "")
                        sInspector += sInsFn + " ";
                    if (sInsMi != "")
                        sInspector += sInsMi + ". ";
                    if (sInsLn != "")
                        sInspector += sInsLn + " ";
                }
                else
                    sInspector = "____________________";
            }
            pRec.Close();


            string sOwnCode = "";
            string sData = "";
            string sBnsPin = string.Empty;
            string sBldgCode = string.Empty;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.MarginLeft = 8000;
            string sDate = string.Format("{0:MMMM dd, yyyy}", m_dtDate);
            this.axVSPrinter1.Table = string.Format("<4000;{0}", sDate);

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

            if (m_sOwnCode != "")
            {
                sOwnCode = m_sOwnCode;
                this.axVSPrinter1.Table = string.Format("<10000;{0}", m_sUnoffBnsName);
                this.axVSPrinter1.Table = string.Format("<10000;{0}", AppSettingsManager.GetBnsOwner(sOwnCode));
                this.axVSPrinter1.Table = string.Format("<10000;{0}", GetUnofficialBnsAddress(m_sBIN, sOwnCode));
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sBnsPin);
            }
            else
            {
                sOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN);
                this.axVSPrinter1.Table = string.Format("<10000;{0}", AppSettingsManager.GetBnsName(m_sBIN));
                this.axVSPrinter1.Table = string.Format("<10000;{0}", AppSettingsManager.GetBnsOwner(sOwnCode));
                this.axVSPrinter1.Table = string.Format("<10000;{0}", AppSettingsManager.GetBnsAddress(sOwnCode));
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sBnsPin);
            }

            this.axVSPrinter1.Table = string.Format("<10000;;;Dear Sir/Madam:");

            sData = "Per inspection conducted by our field inspectors on " + sDateRcd + ", we have ";
            sData += "confirmed that your business, located at the above-stated address, is operating without the required ";
            sData += "business permit from the city government. These findings are noted in the inspection notice issued ";
            sData += "to you on the same date. Such operational activity is deemed in violation of the ";
            sData += "" + AppSettingsManager.GetConfigValue("02") + " Tax Code which states that any business operation is ";
            sData += "considered illegal in the absence of a permit from this office.";
            this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

            sData = "In view thereof, you are STERNLY REMINDED to secure a business permit within five (5) days upon receipt ";
            sData += "hereof. Your failure to do so would constrain this office to come out with an order for the immediate ";
            sData += "CLOSURE of your business. This is without prejudice to the filing of appropriated charges in court ";
            sData += "against you. Please consider this as our FINAL NOTICE.";
            this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

            sData = "For your information.";
            this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

            this.axVSPrinter1.MarginLeft = 8000;
            this.axVSPrinter1.Table = string.Format("<4000;;;Very truly yours,");

            this.axVSPrinter1.Table = string.Format("<4000;;;{0}", AppSettingsManager.GetConfigValue("43"));
            this.axVSPrinter1.Table = string.Format("<4000;Head, Inspection Section");

            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.Table = string.Format("<10000;;;Noted by:");
            this.axVSPrinter1.Table = string.Format("<10000;;;{0}", AppSettingsManager.GetConfigValue("03"));
            this.axVSPrinter1.Table = string.Format("<10000;Department Head, {0}", AppSettingsManager.GetConfigValue("41"));
        }

        private void PrintClosure2() //JARS 20170908 PRINT CLOSURE FOR MALOLOS
        {
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            OracleResultSet pSet = new OracleResultSet();
            string sBnsName = string.Empty;
            string sBnsAddress = string.Empty;
            string sBnsOwner = string.Empty;
            string sDate = string.Empty;
            long y1;
            long y2;
            DateTime dtDate = Convert.ToDateTime(AppSettingsManager.GetCurrentDate());
            sDate = dtDate.ToString("MM/dd/yyyy");
            this.axVSPrinter1.FontName = "Arial";

            pSet.Query = "select * from businesses where bin = '"+ m_sBIN +"'";
            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    sBnsName = pSet.GetString("bns_nm");
                    sBnsAddress = AppSettingsManager.GetBnsAdd(m_sBIN, "");
                    sBnsOwner = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBIN));
                }
            }
            pSet.Close();


            // JAV 20171004 LGU LOGO (s)
            if (m_bUsePreprinted == false)
            {
                //axVSPrinter1.DrawPicture(Properties.Resources.back_white, ".50in", "5.70in", "50%", "50%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "2.90in", ".62in", "25%", "25%", 10, false);
            }
            else
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
            }
            // JAV 20171004 LGU LOGO (e)

            axVSPrinter1.FontSize = 22;
            axVSPrinter1.Table = "^15000; ";
            //axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontName = "Times New Roman";
            axVSPrinter1.Table = "^15000;NOTICE OF CLOSURE";
            this.axVSPrinter1.FontName = "Arial";
            axVSPrinter1.FontBold = false;
            axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.MarginLeft = 1500;
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.FontBold = true;
            y1 = Convert.ToInt32(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            axVSPrinter1.Table = "<2500|<8000|>2000|<2000;Owner/Operator:||Date:|";
            axVSPrinter1.Table = "<2500|<8000|^2000|^2000;Business Name:|||";
            axVSPrinter1.Table = "<2500|<8000|^2000|^2000;Business Address:|||";
            y2 = Convert.ToInt32(axVSPrinter1.CurrentY);
            axVSPrinter1.FontBold = false;
            axVSPrinter1.CurrentY = y1;
            axVSPrinter1.FontUnderline = true;
            axVSPrinter1.Table = "<2500|<8000|>2000|<2000;|" + sBnsOwner + "||" + sDate;
            axVSPrinter1.Table = "<2500|<8000;|" + sBnsName;
            axVSPrinter1.Table = "<2500|<8000;|" + sBnsAddress;
            axVSPrinter1.FontUnderline = false;
            axVSPrinter1.CurrentY = y2;
            axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = "^15000; ";    // RMC 20171201 adjustment in Notice of Closure printing
            //axVSPrinter1.Table = "<13000;\t Violation: NO BUSINESS PERMIT as provided under City Ordinance No. 08-2004, The Revenue Code of the City of Malolos";
            axVSPrinter1.Table = "<13000;\t Violation of City Ordinance No. 08-2004, The Revenue Code of the City of Malolos "; // RMC 20171206 modified notice of closure
            axVSPrinter1.Table = "<13000;\t (" + ViolationsTool.BinViolation(m_sBIN) + ")"; // RMC 20171206 modified notice of closure
            axVSPrinter1.FontBold = false;
            //BPLD OFFICER
            axVSPrinter1.FontSize = 11;
            axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = "^2500|^8000|^4000;||" + AppSettingsManager.GetConfigValue("38");
            axVSPrinter1.FontBold = false;  // RMC 20171206 modified notice of closure
            //axVSPrinter1.FontSize = 9;
            axVSPrinter1.Table = "^2500|^8000|^4000;||" + AppSettingsManager.GetConfigValue("37");
            axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.Table = "^15000; ";
            //CITY ADMIN
            axVSPrinter1.FontSize = 11;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = "^2500|^8000|^4000;||" + AppSettingsManager.GetConfigValue("42");  // RMC 20171201 adjustment in Notice of Closure printing, changed 44 to 42
            /*axVSPrinter1.FontBold = false;
            axVSPrinter1.FontSize = 9;
            axVSPrinter1.Table = "^2500|^8000|^4000;||" + AppSettingsManager.GetConfigObject("42"); //JARS 20171023 ON SITE  // RMC 20171201 adjustment in Notice of Closure printing, changed 44 to 42
            */  // RMC 20171206 modified notice of closure, put rem
            axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.Table = "^15000; ";
            axVSPrinter1.Table = "^15000; ";
            //CITY MAYOR
            /*axVSPrinter1.FontSize = 11;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = "^2500|^8000|^4000;||" + AppSettingsManager.GetConfigValue("36");
            axVSPrinter1.FontBold = false;
            axVSPrinter1.FontSize = 9;
            axVSPrinter1.Table = "^2500|^8000|^4000;||" + AppSettingsManager.GetConfigObject("01") + " MAYOR";*/
            // RMC 20171201 adjustment in Notice of Closure printing, removed



        }
        private void PrintNoticeList()
        {
            // RMC 20120329 Modifications in Notice of violation

            OracleResultSet pList = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();
            string sIsNum = "";
            string sBnsNm = "";
            string sOwnCode = "";
            string sOwner = "";
            string sData = "";
            string sCnt = "";
            string sDateIssued = string.Empty;
            string sDateReceived = string.Empty;
            string sNoticeNo = string.Empty;
            int iCnt = 0;


            pList.Query = m_sQuery; // GDE 20120507 add order by
            if (pList.Execute())
            {
                while (pList.Read())
                {
                    iCnt++;
                    sIsNum = pList.GetString("tbin").Trim();
                    sBnsNm = pList.GetString("Bns_Nm").Trim();
                    sOwnCode = pList.GetString("own_code").Trim();
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);

                    result.Query = "select * from unofficial_notice_closure where is_number = '" + sIsNum.Trim() + "' order by notice_number desc";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sDateIssued = result.GetDateTime("notice_date").ToShortDateString();
                            sDateReceived = result.GetDateTime("notice_sent").ToShortDateString();
                            sNoticeNo = result.GetString("notice_number");
                            if (AppSettingsManager.WithDateReceived(sIsNum, sNoticeNo) == false)
                                sDateReceived = string.Empty;
                        }
                    }
                    result.Close();

                    sCnt = string.Format("{0:###}", iCnt);

                    sData = sCnt + ") " + sIsNum + "|" + sBnsNm + "|" + sOwner + "|" + sDateIssued + "|" + sDateReceived;
                    //this.axVSPrinter1.Table = string.Format("<2300|<4000|<4000;{0}", sData); // GDE 20120508
                    this.axVSPrinter1.Table = string.Format("<1500|<3800|<2800|<1200|<1200;{0}", sData);

                }
            }
            pList.Close();


            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<5000|<5000;Printed by: {0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<5000|<5000;Date printed: {0}", AppSettingsManager.GetCurrentDate());
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

        private void RetCertBINAN() //JARS 20170817 EDITED FOR MALOLOS USE
        {
            //JHMN 20170104 switch of retirement certificate printing

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
            string sRetDate = string.Empty;
            string sOrNo = string.Empty;
            string sOrDate = string.Empty;
            string sOrAmt = string.Empty;
            DateTime dtDate = Convert.ToDateTime(AppSettingsManager.GetCurrentDate());
            //string sDay = string.Format("{0}", dtDate.Day);
            string sDay = string.Empty;
            //string sMonth = string.Format("{0:MMMM}", dtDate);
            //string sYear = string.Format("{0:yyyy}", dtDate);
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sMemo = string.Empty;
            string sInspectorName = string.Empty;

            pRec.Query = "select updated_dt from ret_info_cert where bin = '" + m_sBIN + "'"; //AFM 20211216 MAO-21-16196 will get date of last generation update
            if (pRec.Execute())
                if (pRec.Read())
                {
                    sDay = pRec.GetDateTime(0).Day.ToString();
                    sMonth = string.Format("{0:MMMM}",pRec.GetDateTime(0));
                    sYear = pRec.GetDateTime(0).Year.ToString();
                }

            axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "1.0in", ".62in", "25%", "25%", 10, false);  // RMC 20180103 added printing of logo in Retirement certificate

            pRec.Query = "SELECT * FROM retired_bns WHERE bin = '" + m_sBIN + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    //sRetDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("apprvd_date"));
                    sRetDate = string.Format("{0:MMMM dd, yyyy}", pRec.GetDateTime("apprvd_date"));  // RMC 20171205 corrected retirement date in Retirement certificate
                }

            }
            pRec.Close();

            pRec.Query = "select distinct * from pay_hist where bin = '" + m_sBIN + "' AND bns_stat = 'RET' ORDER BY or_date DESC";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sOrNo = pRec.GetString("or_no").Trim();
                    sOrDate = pRec.GetDateTime("or_date").ToShortDateString();
                }
                else
                    sOrNo = "NO PAYMENT";

            }
            pRec.Close();

            pRec.Query = "select sum(fees_amtdue) as amt from or_table where or_no = '" + sOrNo + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    try
                    {
                        sOrAmt = string.Format("{0:#,##0.#0}", pRec.GetDouble("amt"));
                    }
                    catch
                    {
                        sOrAmt = "0.00";
                    }
                }
            }
            pRec.Close();

            pRec.Query = "select memoranda from retired_bns where bin = '"+ m_sBIN +"'";
            if(pRec.Execute())
            {
                if(pRec.Read())
                {
                    sMemo = pRec.GetString("memoranda");
                }

            }
            pRec.Close();
            pRec.Query = "select inspector_fn, inspector_mi, inspector_ln from inspector where inspector_code = '"+ m_sInspector +"'";
            if(pRec.Execute())
            {
                if(pRec.Read())
                {
                    sInspectorName = pRec.GetString("inspector_fn") + " "  + pRec.GetString("inspector_mi") + " " + pRec.GetString("inspector_ln");
                }
            }
            pRec.Close();
            sOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            sGender = AppSettingsManager.GetBnsOwnerGender(sOwnCode);
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            if (m_bUsePreprinted == true)  
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
            }
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 9000;
            //this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = string.Format("<3000;{0:MMMM dd, yyyy}", dtDate);

            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)16;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("^11000;C E R T I F I C A T I O N");

            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<11000;TO WHOM IT MAY CONCERN:");
            this.axVSPrinter1.FontBold = false;

            //this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //sContent = "\tThis is to certify that based on the "+ m_sCertOption +" (Doc No. "+ m_sDocNo +", Page No. "+ m_sPageNo +", Book No. "+ m_sBookNo +" Series of " + AppSettingsManager.GetConfigValue("12") + " of";
            //sContent = "\tThis is to certify that based on the " + m_sCertOption + " (Doc No. " + m_sDocNo + ", Page No. " + m_sPageNo + ", Book No. " + m_sBookNo + " Series of " + sRetDate + " of"; //AFM 20220103 MAO-21-16283
            sContent = "\tThis is to certify that based on the " + m_sCertOption + " (Doc No. " + m_sDocNo + ", Page No. " + m_sPageNo + ", Book No. " + m_sBookNo + " Series of " + SeriesYr + " of"; //AFM 20220106 MAO-22-16337
            sContent += " Notary Public "+ m_sNotary +" of the City of Malolos, Bulacan) of "+ AppSettingsManager.GetBnsOwner(sOwnCode) +" and inspection certificate of "+ sInspectorName +" submitted in this office, "+ AppSettingsManager.GetBnsName(m_sBIN) +" ";
            sContent += " located at " + AppSettingsManager.GetBnsAddress(m_sBIN) + " registered in the name of " + AppSettingsManager.GetBnsOwner(sOwnCode) + " ceased to";
            //sContent += " operate on " + dDate.ToString("MMM dd, yyyy") + " and has remained closed up to present due to " + m_sDueTo + ".";
            sContent += " operate on " + sRetDate + " and has remained closed up to present due to " + m_sDueTo + ".";    // RMC 20171205 corrected retirement date in Retirement certificate
            this.axVSPrinter1.Table = string.Format("=11000;{0}", sContent);

            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            //JHB 20180620 request to hide/remove this part
            sContent = "\tThis certification is been issued upon the request of " + AppSettingsManager.GetBnsOwner(sOwnCode) + ". "; // replace " the interested party " with requestor's name
            //sContent += " for " + m_sPurpose + "." ; // JHB 20180511 all legal intents and purposes."; // RMC 20171114 added capturing of certification payment, removed 'and'
            this.axVSPrinter1.Table = string.Format("=11000;{0}", sContent);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                //sContent = "\tIssued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + " at City of " + AppSettingsManager.GetConfigValue("02") + ", " + AppSettingsManager.GetConfigValue("08") + "."; //JAV 20170804 Remove City of
                sContent = "\tIssued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + " at " + AppSettingsManager.GetConfigValue("02") + " " + AppSettingsManager.GetConfigValue("08") + "."; //JAV 20170804 Remove City of
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                sContent = "\tIssued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + " at Municipality of " + AppSettingsManager.GetConfigValue("02") + ", " + AppSettingsManager.GetConfigValue("08") + ".";
            this.axVSPrinter1.Table = string.Format("=11000;{0}", sContent);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            

            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = string.Format("<6000|^4000;|{0}", AppSettingsManager.GetConfigValue("38"));
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.FontItalic = true;
            //this.axVSPrinter1.Table = string.Format("<6000|^5000;|BUSINESS PERMIT & LICENSING OFFICER");
            //this.axVSPrinter1.FontItalic = false;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 7500;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("38"));
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            //axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.MarginLeft = 7500;
            this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("37"));
            
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.Table = string.Format("<3000;{0}","Last Payment Information:");     // RMC 20171114 added capturing of certification payment
            this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;O.R. No.|:|" + sOrNo);  
            this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Amount|:|P " + Convert.ToDecimal(sOrAmt).ToString("#,##0.00"));    
            this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Date|:|" + sOrDate);

            // RMC 20171123 enable capturing of certification payment if retirement certificate is reprinted (s)
            if (m_sORNo != "")
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = string.Format("<3000;{0}", "Certification Payment:");     // RMC 20171114 added capturing of certification payment
                this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;O.R. No.|:|" + m_sORNo);
                this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Amount|:|P " + Convert.ToDecimal(m_sAmount).ToString("#,##0.00"));
                this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Date|:|" + m_sORDate);
            }
            // RMC 20171123 enable capturing of certification payment if retirement certificate is reprinted (e)
            
            if (AuditTrail.InsertTrail("ARCR", "trail table", m_sBIN) == 0)
            { }
        }

        private void AppCert()
        {
            //JHMN 20170104 application certificate

            string strCurrentDate = string.Empty;
            OracleResultSet pRec = new OracleResultSet();
            string sBnsNm = string.Empty;
            string sBnsAdd = string.Empty;
            string sBnsOwner = string.Empty;
            string sTaxYear = string.Empty;
            string sGender = string.Empty;
            string sOwnCode = string.Empty;
            string sNoticeCertDate = string.Empty;
            string sContent = string.Empty;
            string sOrNo = string.Empty;
            string sOrDate = string.Empty;
            string sOrAmt = string.Empty;
            DateTime dtDate = Convert.ToDateTime(AppSettingsManager.GetCurrentDate());
            //string sDay = string.Format("{0:dd}", dtDate);
            string sDay = string.Format("{0}", dtDate.Day); // MCR 20190710 .day
            string sMonth = string.Format("{0:MMMM}", dtDate);
            string sYear = string.Format("{0:yyyy}", dtDate);
            long lngY = 0;
            
            strCurrentDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

            pRec.Query = "select distinct * from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "' AND (bns_stat = 'REN' OR bns_stat = 'NEW')";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sOrNo = pRec.GetString("or_no").Trim();
                    sOrDate = pRec.GetDateTime("or_date").ToShortDateString();
                }

            }
            pRec.Close();

            pRec.Query = "select sum(fees_amtdue) as amt from or_table where or_no = '" + sOrNo + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    try
                    {
                        sOrAmt = string.Format("{0:#,##0.#0}", pRec.GetDouble("amt"));
                    }
                    catch
                    {
                        sOrAmt = "0.00";
                    }
                }
            }
            pRec.Close();

            sOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN);
            sGender = AppSettingsManager.GetBnsOwnerGender(sOwnCode);

            // JAV 20170929 LGU LOGO (s)
            if (m_bUsePreprinted == false)
            {
                //axVSPrinter1.DrawPicture(Properties.Resources.back_white, ".50in", "5.70in", "50%", "50%", 10, false);
                axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "1.0in", ".62in", "25%", "25%", 10, false);
            }
            else
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
            }
            // JAV 20170929 LGU LOGO (s)
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 9000;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = string.Format("<3000;{0:MMMM dd, yyyy}", dtDate);
            // JAV 20170929 LGU LOGO (e)

            //lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontSize = (float)16;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^10500;C E R T I F I C A T I O N");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.Table = string.Format("<10000;TO WHOM IT MAY CONCERN:");
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Paragraph = "";
            sContent = "\tThis is to certify that, " + AppSettingsManager.GetBnsName(m_sBIN);
            sContent += " Owned by " + AppSettingsManager.GetBnsOwner(sOwnCode) + " located at " + AppSettingsManager.GetBnsAddress(m_sBIN) + ", " + AppSettingsManager.GetConfigValue("08");
            sContent += " has applied for its BUSINESS PERMIT in the City Hall but still in process";
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            sContent = "\tThis certification is being issued upon request of the above mentioned";
            sContent += " establishment for whatever legal intent and purposes it may serve.";
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                //sContent = "\tIssued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + " at City of " + AppSettingsManager.GetConfigValue("02") + ", " + AppSettingsManager.GetConfigValue("08") + ".";
                sContent = "\tIssued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + "at " + AppSettingsManager.GetConfigValue("02") + ", " + AppSettingsManager.GetConfigValue("08") + "."; // JAV 20171003 Remove City of 
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                sContent = "\tIssued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + " at Municipality of " + AppSettingsManager.GetConfigValue("02") + ", " + AppSettingsManager.GetConfigValue("08") + ".";
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.Table = string.Format("<6000|^4000;|{0}", AppSettingsManager.GetConfigValue("03"));
            //this.axVSPrinter1.Table = string.Format("<6000|^4000;|CHIEF, Business Permit & Licensing Office"); // JAV 20171003 change to BPLO
            this.axVSPrinter1.MarginLeft = 6300;
            // RMC 20171128 added config for certification signatory except for Retirement certificate (s)
            if (AppSettingsManager.GetConfigValue("10") == "216")
            {
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("68"));
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                //this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("69"));
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValueRemarks("68")); //AFM 20200205
            }// RMC 20171128 added config for certification signatory except for Retirement certificate (e)
            else
            {
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("03"));
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                this.axVSPrinter1.Table = string.Format("^4000;CHIEF, BPLO");// JAV 20171003 change to BPLO
            }

            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("<2000|<8000;O.R. No. |" + sOrNo);
            this.axVSPrinter1.Table = string.Format("<2000|<8000;O.R. Date: |" + sOrDate);
            this.axVSPrinter1.Table = string.Format("<2000|<8000;O.R. Amount: |P " + Convert.ToDecimal(sOrAmt).ToString("#,##0.00"));

            if (AuditTrail.InsertTrail("ARCWA", "trail table", m_sBIN) == 0)
            { }
        }

        private void NoBusCert() //JARS 20170824 REVISED FOR MALOLOS
        {
            //JHMN 20170105 no business certificate
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            string strCurrentDate = string.Empty;
            this.axVSPrinter1.FontName = "Times New Roman";
            OracleResultSet pRec = new OracleResultSet();
            string sContent = string.Empty;
            DateTime dtDate = Convert.ToDateTime(AppSettingsManager.GetCurrentDate());
            //string sDay = string.Format("{0:dd}", dtDate);
            string sDay = string.Format("{0}", dtDate.Day); // MCR 20190710 .day
            string sMonth = string.Format("{0:MMMM}", dtDate);
            string sYear = string.Format("{0:yyyy}", dtDate);
            string sTagDate = string.Empty;

            //JARS 20170824 (S)
            if (sMonth == "January")
            {
                sTagDate = "Enero";
            }
            else if(sMonth == "February")
            {
                sTagDate = "Pebrero";
            }
            else if (sMonth == "March")
            {
                sTagDate = "Marso";
            }
            else if(sMonth == "April")
            {
                sTagDate = "Abril";
            }
            else if(sMonth == "May")
            {
                sTagDate = "Mayo";
            }
            else if (sMonth == "June")
            {
                sTagDate = "Junyo";
            }
            else if(sMonth == "July")
            {
                sTagDate = "Julyo";
            }
            else if(sMonth == "August")
            {
                sTagDate = "Agosto";
            }
            else if(sMonth == "September")
            {
                sTagDate = "Septyembre";
            }
            else if(sMonth == "October")
            {
                sTagDate = "Oktubre";
            }
            else if(sMonth == "November")
            {
                sTagDate = "Nobyembre";
            }
            else if(sMonth == "December")
            {
                sTagDate = "Disyembre";
            }
            //JARS 20170824 (E)

            strCurrentDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

            this.axVSPrinter1.CurrentY = 3000;
            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.Table = string.Format("^7000|<2500;|{0}", sTagDate + " " +sDay + ", " + sYear);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)13;
            this.axVSPrinter1.Table = string.Format("^10500;P A G P A P A T U N A Y");
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<9000;SA SINO MANG KINAUUKULAN");
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            #region comments
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            /*
            sContent = "\tThis is to certify that, MR./MS. " + OwnLastName + " a resident of";
            sContent += " " + m_sOwnAdd + ", " + AppSettingsManager.GetConfigValue("01") + " OF " + AppSettingsManager.GetConfigValue("02") + ", " + AppSettingsManager.GetConfigValue("08") + " has NO BUSINESS RECORD";
            sContent += " registered to this City Hall up to this date.";
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);
            */

            /*
            sContent = "\tThis certification is being issued upon request of the above mentioned";
            sContent += " person/ establishment for whatever legal intent and purposes it may serve.";
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);
            */
            #endregion
            sContent = "\tBatay sa pagpapatunay ng tanggapan ng Ingat-Yamang Panlunsod ay pinatutunayan ko na si / ang " + OwnLastName +" na naninirahan sa ";
            sContent += m_sOwnAdd + " ay walang anumang binayarang 'business permit' sa Pamahalaang Lungsod ng Malolos.";
            this.axVSPrinter1.Table = string.Format("=9000;{0}", sContent);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            sContent = "\tAng pagpapatunay na ito ay ipinagkaloob kay / sa " + OwnLastName + " para sa anumang kapakinabangang legal.";
            this.axVSPrinter1.Table = string.Format("=9000;{0}", sContent);
            #region comments
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //if (AppSettingsManager.GetConfigObject("01") == "CITY")
            //    sContent = "\tIssued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + " at City of " + AppSettingsManager.GetConfigValue("02") + ", " + AppSettingsManager.GetConfigValue("08") + ".";
            //else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
            //    sContent = "\tIssued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + " at Municipality of " + AppSettingsManager.GetConfigValue("02") + ", " + AppSettingsManager.GetConfigValue("08") + ".";
            //this.axVSPrinter1.Table = string.Format("=10000;{0}", sContent);
            #endregion
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("<6000|^4000;|{0}", AppSettingsManager.GetConfigValue("36"));
            this.axVSPrinter1.Table = string.Format("<6000|^4000;|Punong Lungsod");

            if (AuditTrail.InsertTrail("ARCNB", "trail table", OwnLastName) == 0)
            { }
        }

        public void ListOfEarlyBird() // JHMN 20170117 print early bird taxpayer
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            string sBin = "";
            DateTime sORDate;
            string sORNo = "";
            double dORAmount = 0.00;
            string sOwnCode = "";
            string sDate = string.Format("{0:MMMM dd, yyyy}", m_dtDate);

            /*this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^10000;{0}", m_sReportName);
            this.axVSPrinter1.Table = string.Format("^5000;;As of {0}", sDate);

            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.Table = string.Format("^6000|^4000;;TOP: {0} EARLY BIRD TAXPAYER|TAX YEAR: {1}", m_sTop, m_sTaxYear);

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.Table = string.Format("^3000|^4000|^4000|^3000|^2500;BIN|BUSINESS NAME|OWNER'S NAME|AMOUNT PAID|OR DATE");

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;*/ // RMC 20170221 modifications in Early bird report, put rem

            int iCtr = 0;   // RMC 20170221 modifications in Early bird report
            int iTop = 0;   // RMC 20170221 modifications in Early bird report
            int.TryParse(m_sTop, out iTop); // RMC 20170221 modifications in Early bird report

            //pSet.Query = "SELECT * FROM (SELECT DISTINCT BIN, OR_DATE, OR_NO FROM PAY_HIST WHERE TAX_YEAR = '" + m_sTaxYear + "' ORDER BY OR_DATE) WHERE ROWNUM <= '" + m_sTop + "'";
            pSet.Query = "select DISTINCT BIN, OR_DATE, OR_NO from pay_hist where tax_year = '" + m_sTaxYear + "' and to_char(or_date,'yyyy') = '" + m_sTaxYear + "' and bns_stat = 'REN' order by or_date";   // RMC 20170221 modifications in Early bird report
            if (pSet.Execute())
                while (pSet.Read())
                {
                    iCtr++; // RMC 20170221 modifications in Early bird report
                    sBin = pSet.GetString("bin");
                    sORNo = pSet.GetString("or_no");
                    sORDate = pSet.GetDateTime("or_date");
                    sOwnCode = AppSettingsManager.GetBnsOwnCode(sBin);

                    pSet2.Query = "SELECT SUM(fees_due) as paid_amount FROM or_table WHERE or_no = '" + sORNo + "' AND tax_year = '" + m_sTaxYear + "'";
                    if (pSet2.Execute())
                        if (pSet2.Read())
                        {
                            dORAmount = pSet2.GetDouble("paid_amount");
                        }
                    pSet2.Close();

                    //this.axVSPrinter1.Table = string.Format("<3000|<4000|<4000|>3000|^2500;;{0}|{1}|{2}|{3}|{4}", sBin, AppSettingsManager.GetBnsName(sBin), AppSettingsManager.GetBnsOwner(sOwnCode), string.Format("{0:#,###.00}", dORAmount), string.Format("{0:MM/dd/yyyy}", sORDate));

                    // RMC 20170221 modifications in Early bird report (s)
                    if (iCtr > iTop)
                        break;
                    this.axVSPrinter1.Table = string.Format("<3000|<4000|<4000|>3000|^2500;;{0}|{1}|{2}|{3}|{4}", iCtr.ToString() + ". " + sBin, AppSettingsManager.GetBnsName(sBin), AppSettingsManager.GetBnsOwner(sOwnCode), string.Format("{0:#,###.00}", dORAmount), string.Format("{0:MM/dd/yyyy}", sORDate));
                    // RMC 20170221 modifications in Early bird report (e)
                                        
                }
            pSet.Close();

            // RMC 20170221 modifications in Early bird report (s)
            string sObject = "Print Early Bird Top: " + m_sTop + "/ Tax Year: " + m_sTaxYear ;
            if (AuditTrail.InsertTrail("ARLEB", "trail table", sObject) == 0)
            { }
            // RMC 20170221 modifications in Early bird report (e)
        }

        private void InspectionReport() //JARS 20170911
        {
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            OracleResultSet pSet = new OracleResultSet();
            string sDate = string.Empty;
            string sData = string.Empty;
            string sInsFN = string.Empty;
            string sInsLN = string.Empty;
            string sInsMI = string.Empty;
            string sObject = string.Empty;
            DateTime dtDate = Convert.ToDateTime(AppSettingsManager.GetCurrentDate());
            sDate = dtDate.ToString("MMM dd, yyyy");

            long y1;
            long y2;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            //this.axVSPrinter1.Table = "^11000; ";
            //this.axVSPrinter1.Table = "^11000; ";
            //this.axVSPrinter1.Table = "^11000; ";
            //this.axVSPrinter1.Table = "^11000; ";
            //this.axVSPrinter1.Table = "^11000; ";
            this.axVSPrinter1.FontName = "Times New Roman";
            this.axVSPrinter1.FontSize = 13;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^11000;INSPECTION  REPORT";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.Table = "^11000; ";
            this.axVSPrinter1.Table = "^11000; ";

            y1 = Convert.ToInt32(this.axVSPrinter1.CurrentY);
            // RMC 20171213 added business status option in printing of inspection report (s)
            this.axVSPrinter1.BrushColor = Color.White;
            this.axVSPrinter1.DrawRectangle(600, y1, 800, y1+250);
            this.axVSPrinter1.DrawRectangle(1900, y1, 2100, y1+250);
            this.axVSPrinter1.DrawRectangle(3600, y1, 3800, y1+250);
            this.axVSPrinter1.PenColor = Color.Black;
            this.axVSPrinter1.Table = "<300|<1000|<300|<1500|<300|<1500;|NEW||RENEWAL||RETIRED";
            this.axVSPrinter1.Paragraph = "";
            // RMC 20171213 added business status option in printing of inspection report (e)


            this.axVSPrinter1.Table = "<1000|<3000;DATE:|" + sDate;
            this.axVSPrinter1.Table = "<3000|<8000;BUSINESS NAME:|" + AppSettingsManager.GetBnsName(m_sBIN);
            this.axVSPrinter1.Table = "<3000|<8000;BUSINESS OWNER:|" + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBIN));
            this.axVSPrinter1.Table = "<3000|<8000;BUSINESS ADDRESS:|" + AppSettingsManager.GetBnsAddress(m_sBIN);
            this.axVSPrinter1.Table = "<3000|<3000|<1500|<4000;CONTACT PERSON:|" + m_sContactP + "|POSITION:|" + m_sPosition;
            this.axVSPrinter1.Table = "<3000|<3000|<1500|<4000;CONTACT NUMBER:|" + m_sContactN + "|E-MAIL:|" + m_sEmail;
            y2 = Convert.ToInt32(this.axVSPrinter1.CurrentY);

            #region comments
            //this.axVSPrinter1.CurrentY = y1;
            //this.axVSPrinter1.FontUnderline = true;
            //this.axVSPrinter1.Table = "<1000|<3000;|" + sDate;
            //this.axVSPrinter1.Table = "<3000|<8000;|" + AppSettingsManager.GetBnsName(m_sBIN);
            //this.axVSPrinter1.Table = "<3000|<8000;|" + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBIN));
            //this.axVSPrinter1.Table = "<3000|<8000;|" + AppSettingsManager.GetBnsAddress(m_sBIN);
            //this.axVSPrinter1.Table = "<3000|<4000|<1500|<4000;|" + m_sContactP + "||" + m_sPosition;
            //this.axVSPrinter1.Table = "<3000|<4000|<1500|<4000;|" + m_sContactN + "||" + m_sEmail;
            //this.axVSPrinter1.FontUnderline = false;
            #endregion
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = y2;
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000;Mahal na Mangangalakal";
            this.axVSPrinter1.Table = "<11000;Batay sa aming isinagawang inspeksyon sa lugar ng inyong negosyo ngayong " +sDate+ " ay napag-alaman ang mga sumusunod:";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000;Wala ang mga sumusunod na may check:";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<1500|<3000;________|Business Permit";
            this.axVSPrinter1.Table = "<1500|<3000;________|Occupancy Permit";
            this.axVSPrinter1.Table = "<1500|<3000;________|Barangay Clearance";
            this.axVSPrinter1.Table = "<1500|<3000;________|Sanitary Certificate";
            this.axVSPrinter1.Table = "<1500|<3000;________|CNC/PECC";
            this.axVSPrinter1.Table = "<1500|<3000;________|FSIC";
            this.axVSPrinter1.Table = "<1500|<3000;________|MAPUMA Certificate";
            this.axVSPrinter1.Table = "<1500|<8000;________|Others (Pls. Specify) _________________________________";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            sData = "Ang nasabing obserbasyon ay labag sa batas. Dahil dito kayo ay binibigyan ng tatlong araw mula sa pagkatanggap ";
            sData += "nito upang iayos ang inyong negosyo at magsumite ng mga nasabing kakulangan sa Tanggapang ito. Ang hindi pagtalima ";
            sData += "sa itinagubulin nito ay nangangahulugan ng tuwirang rebokasyon ng inyong Business Permit at tuluyang pagpapasara ng inyong negosyo.";
            this.axVSPrinter1.Table = "<11000;" + sData;
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000;Para sa inyong kaalaman at mahigpit na pagtalima.";

            pSet.Query = "select inspector_ln,inspector_fn,inspector_mi from inspector where inspector_code = '" + m_sInsCode + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sInsFN = pSet.GetString("inspector_fn");
                    sInsLN = pSet.GetString("inspector_ln");
                    sInsMI = pSet.GetString("inspector_mi");
                }
            }
            pSet.Close();
            this.axVSPrinter1.Table = "^11000; ";
            this.axVSPrinter1.Table = "^11000; ";
            this.axVSPrinter1.Table = "^11000; ";
            this.axVSPrinter1.MarginLeft = 7000;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^4000;" + sInsFN + " " + sInsMI + ". " + sInsLN;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = "^4000;INSPECTOR";
            this.axVSPrinter1.FontSize = 13;

            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.Table = "<11000; ";
            /*this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<1500|<8000;Conforme:|" + m_sContactP;
            this.axVSPrinter1.Table = "<4000|<3000;Signature over Printed Name|Date: " + sDate;*/

            // RMC 20171222 adjustments in Inspection report (s)
            this.axVSPrinter1.Table = "<5400;Conforme:";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<5400;___________________________";
            this.axVSPrinter1.Table = "<5400;Signature over Printed Name";
            this.axVSPrinter1.Table = "<5400;Date:";
            // RMC 20171222 adjustments in Inspection report (e)

            sObject = "Inspector Report BIN: " + m_sBIN;
            if (AuditTrail.InsertTrail("ABIR", "trail table", sObject) == 0)
            { }
        }
        private void PrintBussCert()
        {


             this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;


             OracleResultSet pRec = new OracleResultSet();
             long lngY = 0;
             long lngY2 = 0;
             string sData = "";
             string sModCode = "";
             string sBnsNm = "";
             string sTaxYear = "";
             string sBin = "";
             string sBnsCode = "";
             string sAddress = "";
             string sOrNo = "";
             double dAmtPaid = 0;
             string sAmtPaid = "";
             DateTime dtOrDate = new DateTime();
             string sOrDate = "";



             this.axVSPrinter1.FontName = "Arial";
             this.axVSPrinter1.FontSize = (float)12.0;



             this.axVSPrinter1.Paragraph = "";
             DateTime dtDate = Convert.ToDateTime(AppSettingsManager.GetCurrentDate());
             //string sDay = string.Format("{0:dd}", dtDate);
             string sDay = string.Format("{0}", dtDate.Day); // MCR 20190710 .day
             string sMonth = string.Format("{0:MMMM}", dtDate);
             string sYear = string.Format("{0:yyyy}", dtDate);



             // JAV 20170929 LGU LOGO (s)
             if (m_bUsePreprinted == false)
             {
                 //axVSPrinter1.DrawPicture(Properties.Resources.back_white, ".50in", "5.70in", "50%", "50%", 10, false);
                 this.axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "2.90in", ".62in", "25%", "25%", 10, false);
                 
             }
             else
             {
                 this.axVSPrinter1.Paragraph = ""; 
                 this.axVSPrinter1.Paragraph = ""; 
                 this.axVSPrinter1.Paragraph = ""; 
                 this.axVSPrinter1.Paragraph = "";
                 this.axVSPrinter1.Paragraph = "";
             }
             // JAV 20170929 LGU LOGO (e)
             // JAV 20170929 Date (s)
             
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
             this.axVSPrinter1.MarginLeft = 11000;
             this.axVSPrinter1.FontBold = false;
             this.axVSPrinter1.Table = string.Format("<3000;{0:MMMM dd, yyyy}",dtDate);
             // JAV 20170929 Date (e)


             lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
             this.axVSPrinter1.FontBold = true;
             this.axVSPrinter1.FontUnderline = true;
             
             this.axVSPrinter1.MarginLeft = 1500;
             this.axVSPrinter1.FontSize = (float)16;
             this.axVSPrinter1.Paragraph = "";
             this.axVSPrinter1.Table = string.Format("^11500;{0}", "C E R T I F I C A T I O N");
             
             this.axVSPrinter1.FontUnderline = false;
             this.axVSPrinter1.FontBold = false;
             this.axVSPrinter1.Paragraph = " ";
             this.axVSPrinter1.Paragraph = " ";

             lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
             this.axVSPrinter1.FontSize = (float)10;
             this.axVSPrinter1.CurrentY = lngY;
             this.axVSPrinter1.MarginLeft = 1500;
             sData = "To Whom It May Concern:";
             this.axVSPrinter1.Table = string.Format("<11500;{0}", sData);
             this.axVSPrinter1.Paragraph = " ";

             lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
             this.axVSPrinter1.FontSize = (float)10;
             this.axVSPrinter1.CurrentY = lngY;
             this.axVSPrinter1.MarginLeft = 2500; 
             sData = "This is to certify that upon verification on the records of this office,;;";
                          
             this.axVSPrinter1.IndentTab = "0.25in";
             this.axVSPrinter1.IndentFirst = "0.25in";
             this.axVSPrinter1.Table = string.Format("<11500;{0}", sData);
             #region MyRegion
             //sData = m_sOwnFN + " " + m_sOwnMI + " " + m_sOwnLN;
             //this.axVSPrinter1.FontBold = true;
             //this.axVSPrinter1.FontUnderline = true;
             //this.axVSPrinter1.Table = string.Format("^10000;;{0}", sData);
             //this.axVSPrinter1.FontBold = false;
             //this.axVSPrinter1.FontUnderline = false;
             //this.axVSPrinter1.Table = string.Format("^10000;;{0}", "of");
             //this.axVSPrinter1.FontBold = true;
             //this.axVSPrinter1.FontUnderline = true;
             //this.axVSPrinter1.Table = string.Format("^10000;;{0}", m_sOwnAdd);
             //this.axVSPrinter1.FontBold = false;
             //this.axVSPrinter1.FontUnderline = false;

             //if (m_sOwnCode == "")    // no record
             //{
             #endregion
             string chk = "X";
             sData = m_sOwnLN + ", " + m_sOwnFN + " " + m_sOwnMI + ".";
             //lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
             //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
             //axVSPrinter1.CurrentY = lngY;
             //this.axVSPrinter1.MarginLeft = 2500;
             //this.axVSPrinter1.FontSize = (float)8;
             //this.axVSPrinter1.Table = string.Format("<11500; ");
             if (m_sMode == "No Record") // no record
             {
                 #region start
                 //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                 //axVSPrinter1.CurrentY = lngY;
                 //this.axVSPrinter1.MarginLeft = -8500;
                 //this.axVSPrinter1.FontSize = (float)4.1;
                 //this.axVSPrinter1.Table = string.Format("<200; ");
                 //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                 //axVSPrinter1.CurrentY = lngY;
                 //this.axVSPrinter1.MarginLeft = -8500;
                 //this.axVSPrinter1.FontSize = (float)8;
                 //this.axVSPrinter1.Table = string.Format("<1000;{0}", chk);
                 #endregion
                 lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
                 this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                 this.axVSPrinter1.CurrentY = lngY;
                 this.axVSPrinter1.MarginLeft = -5000;
                 this.axVSPrinter1.FontSize = (float)10;
                 this.axVSPrinter1.Table = string.Format("<4000;No business is registered in the name of");
                 this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
                 this.axVSPrinter1.CurrentY = lngY;
                 this.axVSPrinter1.MarginLeft = 5000;
                 this.axVSPrinter1.FontSize = (float)10;
                 this.axVSPrinter1.FontBold = true;
                 this.axVSPrinter1.Table = string.Format("^6000;{0}", sData);
                 this.axVSPrinter1.FontBold = false;
                 this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                 this.axVSPrinter1.CurrentY = lngY;
                 this.axVSPrinter1.MarginLeft = 11500;
                 this.axVSPrinter1.FontSize = (float)10;
                 this.axVSPrinter1.Table = string.Format("<1000;of");
                 this.axVSPrinter1.Paragraph = " ";

                 lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
                 #region start
                 //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                 //axVSPrinter1.CurrentY = lngY;
                 //this.axVSPrinter1.MarginLeft = 1000;
                 //this.axVSPrinter1.FontSize = (float)8;
                 //this.axVSPrinter1.Table = string.Format("<10000; ");
                 #endregion
                 this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
                 this.axVSPrinter1.CurrentY = lngY;
                 this.axVSPrinter1.MarginLeft = -3300;
                 this.axVSPrinter1.FontSize = (float)10;
                 this.axVSPrinter1.FontBold = true;
                 this.axVSPrinter1.Table = string.Format("^5000; {0}", m_sOwnAdd);
                 this.axVSPrinter1.FontBold = false;
                 this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                 this.axVSPrinter1.CurrentY = lngY;
                 this.axVSPrinter1.MarginLeft = 5000;
                 this.axVSPrinter1.FontSize = (float)10;
                 this.axVSPrinter1.Table = string.Format("<4000;City of Malolos, Bulacan.");

                 sModCode = "ARCNB";
             }


             if (m_sMode == "With Record")
             {
                 lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
                 #region MyRegion
                 //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                 //axVSPrinter1.CurrentY = lngY;
                 //this.axVSPrinter1.MarginLeft = -8500;
                 //this.axVSPrinter1.FontSize = (float)4.1;
                 //this.axVSPrinter1.Table = string.Format("<200; ");
                 //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                 //axVSPrinter1.CurrentY = lngY;
                 //this.axVSPrinter1.MarginLeft = -8500;
                 //this.axVSPrinter1.FontSize = (float)8;
                 //this.axVSPrinter1.Table = string.Format("<1000;{0}", chk);
                 #endregion

                 this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                 this.axVSPrinter1.CurrentY = lngY + 200;
                 this.axVSPrinter1.MarginLeft = -4000;
                 this.axVSPrinter1.FontSize = (float)10;
                 this.axVSPrinter1.Table = string.Format("<5000;the following business is registered in the name of");
                 this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
                 this.axVSPrinter1.CurrentY = lngY + 200;
                 this.axVSPrinter1.MarginLeft = 5000;
                 this.axVSPrinter1.FontSize = (float)10;
                 this.axVSPrinter1.FontBold = true;
                 this.axVSPrinter1.Table = string.Format("^5000;{0}", sData);
                 this.axVSPrinter1.FontBold = false;

                 //sModCode = "ARCNB";

                 lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
                 this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                 this.axVSPrinter1.CurrentY = lngY + 400;
                 this.axVSPrinter1.MarginLeft = 1500;
                 pRec.Query = "select * from businesses where own_code = '" + m_sOwnCode + "' order by tax_year";
                 if (pRec.Execute())
                 {
                     //this.axVSPrinter1.Paragraph = "";
                     this.axVSPrinter1.FontSize = (float)10;
                     //sData = "BIN|Business Name|Business Nature|Business Address|Tax Year Paid";
                     sData = "Business Name|Address|Amount Paid|O.R.No.|Date";
                     this.axVSPrinter1.FontUnderline = true;
                     this.axVSPrinter1.FontBold = true;
                     this.axVSPrinter1.Table = string.Format("^2900|^2800|^1500|^1400|^1400;{0}", sData);
                     this.axVSPrinter1.FontBold = false;
                     this.axVSPrinter1.FontUnderline = false;

                     while (pRec.Read())
                     {
                         sBin = pRec.GetString("bin");
                         sBnsNm = pRec.GetString("Bns_nm");
                         sAddress = pRec.GetString("bns_house_no") + " " + pRec.GetString("bns_street");
                         sTaxYear = pRec.GetString("tax_year");

                         OracleResultSet pRec1 = new OracleResultSet();
                         pRec1.Query = string.Format("select * from pay_hist where bin = '{0}' and tax_year = '{1}'", sBin, sTaxYear);
                         if (pRec1.Execute())
                         {
                             if (pRec1.Read())
                             {
                                 sOrNo = pRec1.GetString("or_no");
                                 dtOrDate = pRec1.GetDateTime("or_date");
                                 sOrDate = string.Format("{0:dd/MM/yyyy}", dtOrDate);
                             }
                         }
                         pRec1.Close();

                         pRec1.Query = string.Format("select sum(fees_amtdue) as amt from or_table where or_no = '{0}' and tax_year = '{1}'", sOrNo, sTaxYear);
                         if (pRec1.Execute())
                         {
                             if (pRec1.Read())
                             {
                                 try
                                 {
                                     sAmtPaid = string.Format("{0:#,##0.#0}", pRec1.GetDouble("amt"));
                                 }
                                 catch
                                 {
                                     sAmtPaid = "0.00";
                                 }
                             }
                         }
                         pRec1.Close();
                         sAmtPaid = "Php" + sAmtPaid;
                         sData = sBnsNm + "|" + sAddress + "|" + sAmtPaid + "|" + sOrNo + "|" + sOrDate;
                         this.axVSPrinter1.Table = string.Format("^2900|^2800|^1500|^1400|^1400;{0}", sData);
                     }
                 }
                 pRec.Close();

                 sModCode = "ARCWB";
             }


             #region Start
             //this.axVSPrinter1.FontSize = (float)12.0;
             //}
             //else
             //{
             //    // with record
             //    sData = "is registered in this office as owner of the ff. business/es: ";
             //    this.axVSPrinter1.IndentTab = "0.0in";
             //    this.axVSPrinter1.IndentFirst = "0.0in";
             //    this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

             //    sModCode = "ARCWB";

             //    string sBnsNm = "";
             //    string sTaxYear = "";
             //    string sBin = "";
             //    string sBnsCode = "";
             //    pRec.Query = "select * from businesses where own_code = '" + m_sOwnCode + "' order by tax_year";
             //    if (pRec.Execute())
             //    {
             //        this.axVSPrinter1.Paragraph = "";
             //        this.axVSPrinter1.FontSize = (float)8.0;
             //        sData = "BIN|Business Name|Business Nature|Business Address|Tax Year Paid";
             //        this.axVSPrinter1.Table = string.Format("^1500|^3000|^1500|^3000|^1000;{0}", sData);

             //        while (pRec.Read())
             //        {
             //            sBin = pRec.GetString("bin");
             //            sBnsNm = pRec.GetString("Bns_nm");
             //            sTaxYear = pRec.GetString("tax_year");
             //            sBnsCode = pRec.GetString("bns_code");
             //            sBnsCode = AppSettingsManager.GetBnsDesc(sBnsCode);

             //            sData = sBin + "|" + sBnsNm + "|" + sBnsCode + "|" + AppSettingsManager.GetBnsAddress(sBin) + "|" + sTaxYear;
             //            this.axVSPrinter1.Table = string.Format("<1500|<3000|<1500|<3000|^1000;{0}", sData);
             //        }

             //        this.axVSPrinter1.Paragraph = "";
             //    }
             //    pRec.Close();
             //    this.axVSPrinter1.FontSize = (float)12.0;
             //}

             //lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
             //this.axVSPrinter1.FontSize = (float)8;
             //axVSPrinter1.CurrentY = lngY;
             //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

             //sData = "This certification is issued upon the request of ;";
             //this.axVSPrinter1.IndentTab = "0.25in";
             //this.axVSPrinter1.IndentFirst = "0.25in";
             //this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);
             //this.axVSPrinter1.FontBold = true;
             //this.axVSPrinter1.FontUnderline = true;
             //this.axVSPrinter1.Table = string.Format("^10000;;{0}", m_sRequestBy);
             //this.axVSPrinter1.FontBold = false;
             //this.axVSPrinter1.FontUnderline = false;
             //sData = "for whatever legal purposes it may serve.";
             //this.axVSPrinter1.IndentTab = "0.25in";
             ////this.axVSPrinter1.IndentFirst = "0.25in";
             //this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

             //sData = "Issued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + ", " + AppSettingsManager.GetConfigValue("02") + ".";
             //this.axVSPrinter1.IndentTab = "0.25in";
             //this.axVSPrinter1.IndentFirst = "0.25in";
             //this.axVSPrinter1.Table = string.Format("=10000;;{0}", sData);

             ////this.axVSPrinter1.Paragraph = "";
             ////this.axVSPrinter1.Paragraph = "";
             ////this.axVSPrinter1.Paragraph = "";

             //sData = AppSettingsManager.GetConfigValue("03") + ";";
             //this.axVSPrinter1.Table = string.Format(">10000;;{0}", sData);
             //sData = "BPLO - HEAD"; //sData = "BPTFO-Department Head"; //MOD MCR 20143005
             //this.axVSPrinter1.Table = string.Format(">10000;{0}", sData);

             // RMC 20120221 added count of printed certifications in Certifications module (s)

             #endregion

             lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
             this.axVSPrinter1.CurrentY = lngY + 400;
             this.axVSPrinter1.MarginLeft = -4800;    
             this.axVSPrinter1.Table = "<4500;This certification is issued upon the request of";
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
             this.axVSPrinter1.CurrentY = lngY + 400;
             this.axVSPrinter1.MarginLeft = 3400;
             this.axVSPrinter1.FontBold = true;
             this.axVSPrinter1.Table = string.Format("^3800;{0}", m_sRequestBy);
             this.axVSPrinter1.FontBold = false;
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
             this.axVSPrinter1.CurrentY = lngY + 400;
             this.axVSPrinter1.MarginLeft = 10500;
             this.axVSPrinter1.Table = "<4000;for whatever legal purpose it";
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
             this.axVSPrinter1.CurrentY = lngY + 600;
             this.axVSPrinter1.MarginLeft = -5200;
             this.axVSPrinter1.Table = "<4000;may serve.";
             this.axVSPrinter1.Paragraph = " ";
             this.axVSPrinter1.Paragraph = " ";
             this.axVSPrinter1.Paragraph = " ";

             lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
             this.axVSPrinter1.CurrentY = lngY;
             this.axVSPrinter1.MarginLeft = 7500;
             this.axVSPrinter1.FontBold = true;
             this.axVSPrinter1.Table = string.Format("^4000;{0}",AppSettingsManager.GetConfigValue("38"));
             this.axVSPrinter1.FontBold = false;
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
             this.axVSPrinter1.CurrentY = lngY + 200;
             this.axVSPrinter1.MarginLeft = 7500;
             this.axVSPrinter1.Table = string.Format("^4000;{0}",AppSettingsManager.GetConfigValue("37"));


             sCurrentUser = AppSettingsManager.SystemUser.UserCode;
             sCurrentUser = AppSettingsManager.GetUserName(sCurrentUser); //JARS 20170920 user NAME instead of CODE
             lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
             this.axVSPrinter1.CurrentY = lngY;
             this.axVSPrinter1.MarginLeft = -4800;
             this.axVSPrinter1.FontBold = true;
             this.axVSPrinter1.Table = string.Format("<1800|^4000;Verified by:|{0}", sCurrentUser); //JARS 20170920
             this.axVSPrinter1.FontBold = false;
             lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
             this.axVSPrinter1.CurrentY = lngY;
             this.axVSPrinter1.MarginLeft = -3000;
             this.axVSPrinter1.Table = "^4000;BPLD Staff"; //JARS 20170920
             this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
             this.axVSPrinter1.CurrentY = lngY;
             this.axVSPrinter1.MarginLeft = -3700;
             this.axVSPrinter1.Table = "<4000; "; //JARS 20170920

             pRec.Query = "select * from TRAIL_TABLE WHERE (usr_rights = 'ARCWB' or usr_rights = 'ARCNB')";
             if (pRec.Execute())
             {
                 if (!pRec.Read())
                 {
                     pRec.Close();
                     pRec.Query = "insert into trail_table values (";
                     pRec.Query += "'ARCNB','ASS-REPORTS-CERTIFICATIONS-NO BUSINESS')";
                     if (pRec.ExecuteNonQuery() == 0)
                     { }

                     pRec.Query = "insert into trail_table values (";
                     pRec.Query += "'ARCWB','ASS-REPORTS-CERTIFICATIONS-WITH BUSINESS')";
                     if (pRec.ExecuteNonQuery() == 0)
                     { }
                 }
             }

             if (AuditTrail.InsertTrail(sModCode, "trail table", m_sOwnFN + " " + m_sOwnMI + " " + m_sOwnLN) == 0)
             { }
             // RMC 20120221 added count of printed certifications in Certifications module (e)
        }

        private void WithBussPermit() //JARS 20170915
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            string sBnsName = string.Empty;
            string sBnsAddress = string.Empty;
            string sBnsOwner = string.Empty;
            string sRequested = string.Empty;
            string sPermitNo = string.Empty;
            string sORNo = string.Empty;
            string sORDate = string.Empty;
            string sData = string.Empty;
            DateTime dtDate = Convert.ToDateTime(AppSettingsManager.GetCurrentDate());
            string sDateToday = dtDate.ToString("MM/dd/yyyy");
            string sYear = string.Empty;
            sYear = dtDate.ToString("yyyy");



            // JAV 20170929 LGU LOGO (s)
            if (m_bUsePreprinted == false)
            {
                //axVSPrinter1.DrawPicture(Properties.Resources.back_white, ".50in", "5.70in", "50%", "50%", 10, false);
                axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "1.0in", ".62in", "25%", "25%", 10, false);
            }
            // JAV 20170929 LGU LOGO (e)

            double dAmount = 0;

            this.axVSPrinter1.MarginLeft = 0;   // RMC 20171114 added capturing of certification payment
            this.axVSPrinter1.MarginLeft = 1500;    // RMC 20171114 added capturing of certification payment, 1000 to 1500
            //this.axVSPrinter1.FontName = "Times New Roman";   // RMC 20171114 added capturing of certification payment, put rem
            this.axVSPrinter1.FontSize = 12;
            //this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontBold = false; // RMC 20171114 added capturing of certification payment
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = ">9000;" + dtDate.ToString("MMMM dd, yyyy");
            this.axVSPrinter1.Table = "<11000; ";
            //this.axVSPrinter1.FontSize = 15;
            //this.axVSPrinter1.Table = "^11000;CERTIFICATION";
            this.axVSPrinter1.FontBold = true;  // RMC 20171114 added capturing of certification payment
            this.axVSPrinter1.FontSize = (float)16; // RMC 20171114 added capturing of certification payment
            this.axVSPrinter1.Table = string.Format("^11000;C E R T I F I C A T I O N");    // RMC 20171114 added capturing of certification payment
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.FontSize = 12;

            sBnsName = AppSettingsManager.GetBnsName(m_sBIN);
            sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
            sBnsOwner = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBIN));
            sRequested = m_sRequestBy;

            pSet.Query = "select permit_no from businesses where bin = '" + m_sBIN + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sPermitNo = pSet.GetString("permit_no");

                }
            }
            pSet.Close();

            pSet.Query = "select * from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + sYear + "' order by or_date asc";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sORNo = pSet.GetString("or_no");
                    sORDate = pSet.GetDateTime("or_date").ToString("MM/dd/yyyy");
                    pSet2.Query = "select sum(fees_amtdue) as amount from or_table where or_no = '" + sORNo + "'";
                    if (pSet2.Execute())
                    {
                        if (pSet2.Read())
                        {
                            dAmount = pSet2.GetDouble("amount");
                        }
                    }
                    pSet2.Close();
                }
            }
            pSet.Close();

            sData = "\tThis is to certify that " + sBnsName.ToUpper() + " located at " + sBnsAddress + " owned by " + sBnsOwner;
            sData += " has applied for Mayor's Permit No. " + sPermitNo + " under this office with Official Reciept No. ";
            sData += sORNo + " dated " + sORDate + " amounting to Php" + dAmount.ToString("#,##0.00") + ".";

            //this.axVSPrinter1.Table = "<10000;" + sData;
            this.axVSPrinter1.Table = string.Format("=11000;{0}", sData);   // RMC 20171114 added capturing of certification payment
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";

            //sData = "\tThis certification is issued to the request of the above-named for the requirements in " + sRequested;

            //this.axVSPrinter1.Table = "<11000;" + sData;
            // RMC 20171114 added capturing of certification payment (s)
            sData = "\tThis certification has been issued upon the request of the interested party";
            sData += " for " + m_sPurpose + "."; // JHB 20180511 all legal intents and purposes.";
            this.axVSPrinter1.Table = string.Format("=11000;{0}", sData);
            // RMC 20171114 added capturing of certification payment (e)

            this.axVSPrinter1.MarginLeft = 7000;
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.FontBold = true;
            // RMC 20171128 added config for certification signatory except for Retirement certificate (s)
            if (AppSettingsManager.GetConfigValue("10") == "216")
            {
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("68"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                //this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("69"));
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValueRemarks("68")); //AFM 20200205
            }// RMC 20171128 added config for certification signatory except for Retirement certificate (e)
            else
            {
                this.axVSPrinter1.Table = "^4000;" + AppSettingsManager.GetConfigValue("38");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                this.axVSPrinter1.Table = "^4000;" + AppSettingsManager.GetConfigValue("37");
            }

            // RMC 20171114 added capturing of certification payment (s)
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;O.R. No.|:|" + m_sORNo);
            this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Amount|:|P " + Convert.ToDecimal(m_sAmount).ToString("#,##0.00"));
            this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Date|:|" + m_sORDate);
            // RMC 20171114 added capturing of certification payment (e)
        }

        private void frmBussReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void CertStatus()
        {
            string sData = string.Empty;
            string sBnsStat = string.Empty;
            string sTaxYear = string.Empty;
            string sDay = "";
            string sMonth = "";
            string sYear = "";

            DateTime current = AppSettingsManager.GetCurrentDate();
            sDay = current.ToString("dd");
            sMonth = current.ToString("MMMM");
            sYear = current.ToString("yyyy");

            if (m_bUsePreprinted == false)
            {   
                axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "1.0in", ".62in", "25%", "25%", 10, false);
            }
            
            this.axVSPrinter1.MarginLeft = 0;   
            this.axVSPrinter1.MarginLeft = 1500;    
            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.FontBold = false; 
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = ">9000;" + current.ToString("MMMM dd, yyyy");
            this.axVSPrinter1.Table = "<11000; ";
            
            this.axVSPrinter1.FontBold = true;  
            this.axVSPrinter1.FontSize = (float)16; 
            this.axVSPrinter1.Table = string.Format("^11000;C E R T I F I C A T I O N");    
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            
            this.axVSPrinter1.FontSize = 12;
            
            sData = "\tIn response to your request letter, below is the status of the Taxpayer based on our records;";
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);

            this.axVSPrinter1.FontSize = 10;    // RMC 20171123 added printing of taxpayer name in Certificate of status
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            //this.axVSPrinter1.Table = string.Format("^4000|^4000|^2000;Business Name|Business Address|Status");
            this.axVSPrinter1.Table = string.Format("^3000|^3000|^3000|^2000;Taxpayer Name|Business Name|Business Address|Status");   // RMC 20171123 added printing of taxpayer name in Certificate of status
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = "<11000; ";

            OracleResultSet pSet = new OracleResultSet();
            //pSet.Query = "select * from pay_hist where bin = '" + m_sBIN + "'order by or_date desc";
            pSet.Query = "select * from pay_hist where bin = '" + m_sBIN + "'order by or_date desc, tax_year desc"; // JAA 20190327 added tax_year

            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sBnsStat = pSet.GetString("bns_stat");
                    sTaxYear = pSet.GetString("tax_year");
                }
            }
            pSet.Close();
            if (sBnsStat == "REN")
            {
                sBnsStat = "Renewed";
            }
            else if (sBnsStat == "RET")
            {
                sBnsStat = "Retired";
            }
            else if (sBnsStat == "NEW")
            {
                sBnsStat = "New";
            }
            
            //sData = AppSettingsManager.GetBnsName(m_sBIN) + "|" + AppSettingsManager.GetBnsAddress(m_sBIN) + "|" + sBnsStat + " " + sTaxYear;
            //this.axVSPrinter1.Table = string.Format("<4000|<4000|<2000;{0}", sData);
            // RMC 20171123 added printing of taxpayer name in Certificate of status (s)
            string sOwnCode = AppSettingsManager.GetOwnCode(m_sBIN);
            sData = AppSettingsManager.GetBnsOwner(sOwnCode) + "|" + AppSettingsManager.GetBnsName(m_sBIN) + "|" + AppSettingsManager.GetBnsAddress(m_sBIN) + "|" + sBnsStat + " " + sTaxYear;
            this.axVSPrinter1.Table = string.Format("<3000|<3000|<3000|<2000;{0}", sData);  
            this.axVSPrinter1.FontSize = 12;
            // RMC 20171123 added printing of taxpayer name in Certificate of status (e)
            this.axVSPrinter1.Table = "<11000; ";
            sData = "\tThis certification is being issued upon the request of " + m_sRequestBy + " for " + m_sPurpose + "."; //JHB 20180511whatever legal intent & purpose it may serve."; 
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);

            this.axVSPrinter1.Table = "<11000; ";
            sData = "\tIssued this " + sDay + " day of " + sMonth + ", " + sYear + " at " + AppSettingsManager.GetConfigValue("02") + " " + AppSettingsManager.GetConfigValue("08") + ".";
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);

            this.axVSPrinter1.MarginLeft = 7000;
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.FontBold = true;
            // RMC 20171128 added config for certification signatory except for Retirement certificate (s)
            if (AppSettingsManager.GetConfigValue("10") == "216")
            {
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("68"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                //this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("69"));
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValueRemarks("68")); //AFM 20200205
            }// RMC 20171128 added config for certification signatory except for Retirement certificate (e)
            else
            {
                this.axVSPrinter1.Table = "^4000;" + AppSettingsManager.GetConfigValue("38");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                this.axVSPrinter1.Table = "^4000;" + AppSettingsManager.GetConfigValue("37");
            }

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            if (m_sORNo != ".")
            {
                this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;O.R. No.|:|" + m_sORNo);
                this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Amount|:|P " + Convert.ToDecimal(m_sAmount).ToString("#,##0.00"));
                this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Date|:|" + m_sORDate);
            }
        }

        private void PrintBussCertNew()
        {
            // RMC 20171128 adjustment in certificate of status and With Business

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;


            OracleResultSet pRec = new OracleResultSet();
            long lngY = 0;
            long lngY2 = 0;
            string sData = "";
            string sModCode = "";
            string sBnsNm = "";
            string sTaxYear = "";
            string sBin = "";
            string sBnsCode = "";
            string sAddress = "";
            string sOrNo = "";
            double dAmtPaid = 0;
            string sAmtPaid = "";
            DateTime dtOrDate = new DateTime();
            string sOrDate = "";

            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.FontSize = (float)12.0;

            this.axVSPrinter1.Paragraph = "";
            DateTime dtDate = Convert.ToDateTime(AppSettingsManager.GetCurrentDate());
            //string sDay = string.Format("{0:dd}", dtDate);
            string sDay = string.Format("{0}", dtDate.Day); // MCR 20190710 .day
            string sMonth = string.Format("{0:MMMM}", dtDate);
            string sYear = string.Format("{0:yyyy}", dtDate);


            if (m_bUsePreprinted == false)
            {

                axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "1.0in", ".62in", "25%", "25%", 10, false); // RMC 20171128 adjustment in certificate of status and With Business
            }
            else
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
            }


            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 10000;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = string.Format("<3000;{0:MMMM dd, yyyy}", dtDate);
            // JAV 20170929 Date (e)


            lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("^10000;{0}", "C E R T I F I C A T I O N");
            this.axVSPrinter1.FontUnderline = false;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = " ";
            //this.axVSPrinter1.Paragraph = " ";

            lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.CurrentY = lngY;

            sData = "To Whom It May Concern:";
            this.axVSPrinter1.Table = string.Format("<11500;{0}", sData);
            this.axVSPrinter1.Paragraph = " ";

            lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.CurrentY = lngY;

            sData = "\tThis is to certify that upon verification on the records of this office,";

            if (m_sMode == "No Record")
            {
                if (m_sOwnFN.Trim() != "")
                    m_sOwnLN += ", " + m_sOwnFN.Trim() + " ";
                m_sOwnLN += m_sOwnMI.Trim();

                sData += string.Format(" no business is registered in the name of {0} ", m_sOwnLN + " located at " + m_sOwnAdd); 

                this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);

                sModCode = "ARCNB";
            }
            else
            {

                sData += string.Format(" the following business/es is registered in the name of {0} ", AppSettingsManager.GetBnsOwner(m_sOwnCode));

                this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);

                lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.CurrentY = lngY + 400;
                this.axVSPrinter1.MarginLeft = 1000;
                pRec.Query = "select * from businesses where own_code = '" + m_sOwnCode + "' order by tax_year";
                if (pRec.Execute())
                {
                    //this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = (float)10;
                    //sData = "BIN|Business Name|Business Nature|Business Address|Tax Year Paid";
                    sData = "Business Name|Address|Amount Paid|O.R.No.|Date";
                    this.axVSPrinter1.FontUnderline = true;
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = string.Format("^2900|^2500|^1800|^1400|^1400;{0}", sData);
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.FontUnderline = false;

                    while (pRec.Read())
                    {
                        sBin = pRec.GetString("bin");
                        sBnsNm = pRec.GetString("Bns_nm");
                        sAddress = AppSettingsManager.GetBnsAddress(sBin);
                        sTaxYear = pRec.GetString("tax_year");

                        OracleResultSet pRec1 = new OracleResultSet();
                        pRec1.Query = string.Format("select * from pay_hist where bin = '{0}' and tax_year = '{1}'", sBin, sTaxYear);
                        if (pRec1.Execute())
                        {
                            if (pRec1.Read())
                            {
                                sOrNo = pRec1.GetString("or_no");
                                dtOrDate = pRec1.GetDateTime("or_date");
                                sOrDate = string.Format("{0:dd/MM/yyyy}", dtOrDate);
                            }
                        }
                        pRec1.Close();

                        pRec1.Query = string.Format("select sum(fees_amtdue) as amt from or_table where or_no = '{0}' and tax_year = '{1}'", sOrNo, sTaxYear);
                        if (pRec1.Execute())
                        {
                            if (pRec1.Read())
                            {
                                try
                                {
                                    sAmtPaid = string.Format("{0:#,##0.#0}", pRec1.GetDouble("amt"));
                                }
                                catch
                                {
                                    sAmtPaid = "0.00";
                                }
                            }
                        }
                        pRec1.Close();
                        sAmtPaid = "Php" + sAmtPaid;
                        sData = sBnsNm + "|" + sAddress + "|" + sAmtPaid + "|" + sOrNo + "|" + sOrDate;
                        this.axVSPrinter1.Table = string.Format("<2900|<2500|>1800|<1400|<1400;{0}", sData);
                    }
                }
                pRec.Close();

                sModCode = "ARCWB";
            }


            //this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            sData = "\tThis certification is being issued upon the request of " + m_sRequestBy + " for " + m_sPurpose + ".";  //whatever legal intent & purpose it may serve."; //JHB 20180511
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);
            

            this.axVSPrinter1.Table = "<11000; ";
            //sData = "\tIssued this " + sDay + " day of " + sMonth + ", " + sYear + " at " + AppSettingsManager.GetConfigValue("02") + " " + AppSettingsManager.GetConfigValue("08") + ".";
            sData = "\tIssued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + " at " + AppSettingsManager.GetConfigValue("02") + " " + AppSettingsManager.GetConfigValue("08") + "."; //JHB 20180620
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);
           

            this.axVSPrinter1.MarginLeft = 7000;
            //this.axVSPrinter1.Table = "<11000; ";
            //this.axVSPrinter1.Table = "<11000; ";
            //this.axVSPrinter1.Table = "<11000; ";
            //this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.FontBold = true;
            // RMC 20171128 added config for certification signatory except for Retirement certificate (s)
            if (AppSettingsManager.GetConfigValue("10") == "216")
            {
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("68"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                //this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("69"));
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValueRemarks("68")); //AFM 20200127 corrected display of title
            }// RMC 20171128 added config for certification signatory except for Retirement certificate (e)
            else
            {
                this.axVSPrinter1.Table = "^4000;" + AppSettingsManager.GetConfigValue("38");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                this.axVSPrinter1.Table = "^4000;" + AppSettingsManager.GetConfigValue("37");
            }

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<5000|<5000;Printed by: {0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<5000|<5000;Date printed: {0}", AppSettingsManager.GetCurrentDate());
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
            this.axVSPrinter1.BorderStyle = VSPrinter7Lib.BorderStyleSettings.bsNone;
            this.axVSPrinter1.Table = string.Format("^10000; ", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            //JHB 20180711 duplicate cert of no business (s)
            if (m_bUsePreprinted == false)
            {
                axVSPrinter1.DrawPicture(Properties.Resources.Ph_seal_bulacan_malolos, "1.0in", "5.7in", "25%", "25%", 10, false); 
            }
            else
            {
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
            }
            CreateHeader();
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 10000;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = string.Format("<3000;{0:MMMM dd, yyyy}", dtDate);
            lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("^10000;{0}", "C E R T I F I C A T I O N");
            this.axVSPrinter1.FontUnderline = false;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = " ";
            //this.axVSPrinter1.Paragraph = " ";

            lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.CurrentY = lngY;

            sData = "To Whom It May Concern:";
            this.axVSPrinter1.Table = string.Format("<11500;{0}", sData);
            this.axVSPrinter1.Paragraph = " ";

            lngY = Convert.ToInt64(this.axVSPrinter1.CurrentY);
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.CurrentY = lngY;

              sData = "\tThis is to certify that upon verification on the records of this office,";
              sData += string.Format(" no business is registered in the name of {0} ", m_sOwnLN + " located at " + m_sOwnAdd); //JHB 20180312
              this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);

            this.axVSPrinter1.Table = "<11000; ";
            sData = "\tThis certification is being issued upon the request of " + m_sRequestBy + " for " + m_sPurpose + ".";  //whatever legal intent & purpose it may serve."; //JHB 20180511
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);
            

            this.axVSPrinter1.Table = "<11000; ";
            //sData = "\tIssued this " + sDay + " day of " + sMonth + ", " + sYear + " at " + AppSettingsManager.GetConfigValue("02") + " " + AppSettingsManager.GetConfigValue("08") + ".";
            sData = "\tIssued this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ", " + sYear + " at " + AppSettingsManager.GetConfigValue("02") + " " + AppSettingsManager.GetConfigValue("08") + "."; //JHB 20180620
            this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);
            this.axVSPrinter1.MarginLeft = 7000;
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.FontBold = true;
            // RMC 20171128 added config for certification signatory except for Retirement certificate (s)
            if (AppSettingsManager.GetConfigValue("10") == "216")
            {
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("68"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                //this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValue("69"));
                this.axVSPrinter1.Table = string.Format("^4000;{0}", AppSettingsManager.GetConfigValueRemarks("68")); //AFM 20200205 corrected display of title
            }// RMC 20171128 added config for certification signatory except for Retirement certificate (e)
            else
            {
                this.axVSPrinter1.Table = "^4000;" + AppSettingsManager.GetConfigValue("38");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                this.axVSPrinter1.Table = "^4000;" + AppSettingsManager.GetConfigValue("37");
            }

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 0;
            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<5000|<5000;Printed by: {0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<5000|<5000;Date printed: {0}", AppSettingsManager.GetCurrentDate());

            //JHB 20180711 duplicate cert of no business (e)
            if (m_sORNo != ".")
            {
                this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;O.R. No.|:|" + m_sORNo);
                this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Amount|:|P " + Convert.ToDecimal(m_sAmount).ToString("#,##0.00"));
                this.axVSPrinter1.Table = string.Format("<1500|^500|<7000;Date|:|" + m_sORDate);
            }

            pRec.Query = "select * from TRAIL_TABLE WHERE (usr_rights = 'ARCWB' or usr_rights = 'ARCNB')";
            if (pRec.Execute())
            {
                if (!pRec.Read())
                {
                    pRec.Close();
                    pRec.Query = "insert into trail_table values (";
                    pRec.Query += "'ARCNB','ASS-REPORTS-CERTIFICATIONS-NO BUSINESS')";
                    if (pRec.ExecuteNonQuery() == 0)
                    { }

                    pRec.Query = "insert into trail_table values (";
                    pRec.Query += "'ARCWB','ASS-REPORTS-CERTIFICATIONS-WITH BUSINESS')";
                    if (pRec.ExecuteNonQuery() == 0)
                    { }
                }
            }

            if (AuditTrail.InsertTrail(sModCode, "trail table", m_sOwnFN + " " + m_sOwnMI + " " + m_sOwnLN) == 0)
            { }
            // RMC 20120221 added count of printed certifications in Certifications module (e)
        }

        private void ComparativeReport()
        {
            // RMC 20180126 added comparative report as requested by Malolos BPLO Head
            // collection based on OR Date, regardless of tax code, fees code and penalty, surcharge

            axVSPrinter1.FontBold = false;
            OracleResultSet pRec = new OracleResultSet();
            string sYear = ConfigurationAttributes.CurrentYear;
            string sP1Year = string.Format("{0:####}", Convert.ToInt32(sYear) - 1);
            string sP2Year = string.Format("{0:####}", Convert.ToInt32(sYear) - 2);
            string sP3Year = string.Format("{0:####}", Convert.ToInt32(sYear) - 3);
            string sDateFr = string.Empty;
            string sDateTo = string.Empty;
            string sTmpYear = string.Empty;
            string sColAmt = string.Empty;
            string sData = string.Empty;

            ArrayList arrDate = new ArrayList();
            ArrayList arrYear = new ArrayList();
            ArrayList arrValue = new ArrayList();

            axVSPrinter1.Paragraph = "Comparative Collection Report";

            // based on current date
            for (int i = 1; i <= 4; i++)
            {
                if (i == 1)
                    sTmpYear = sP3Year;
                if (i == 2)
                    sTmpYear = sP2Year;
                if (i == 3)
                    sTmpYear = sP1Year;
                if (i == 4)
                    sTmpYear = sYear;

                sDateFr = string.Format("{0}/{1}/", m_dFrom.Month, m_dFrom.Day);
                sDateFr += sTmpYear;
                sDateTo = string.Format("{0}/{1}/", m_dTo.Month, m_dTo.Day);
                sDateTo += sTmpYear;

                pRec.Query = @"select sum(fees_amtdue) from or_table where or_no in
                    (select or_no from pay_hist where or_date between to_date('"+sDateFr+"','MM/dd/yyyy') and to_date('"+sDateTo+"','MM/dd/yyyy'))";
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        sColAmt = string.Format("{0:####.00}", pRec.GetDouble(0));

                        arrDate.Add(sDateFr + "-" + sDateTo);
                        arrYear.Add(sTmpYear);
                        arrValue.Add(sColAmt);
                    }
                }
                pRec.Close();
            }

            // print
            sData = "Covered Date|Year|Collected Amount";
            this.axVSPrinter1.Table = string.Format("^3000|^2000|^3000;{0}", sData);

            for (int j = 0; j < arrDate.Count; j++)
            {
                sData = arrDate[j].ToString() + "|";
                sData += arrYear[j].ToString() + "|";
                sData += string.Format("{0:#,###.00}", Convert.ToDouble(arrValue[j].ToString()));
                this.axVSPrinter1.Table = string.Format("<3000|<2000|>3000;{0}", sData);
            }


            // based on year 
            arrDate = new ArrayList();
            arrYear = new ArrayList();
            arrValue = new ArrayList();

            for (int i = 1; i <= 4; i++)
            {
                if (i == 1)
                    sTmpYear = sP3Year;
                if (i == 2)
                    sTmpYear = sP2Year;
                if (i == 3)
                    sTmpYear = sP1Year;
                if (i == 4)
                    sTmpYear = sYear;

                sDateFr = "1/01/" + sTmpYear;
                sDateTo = "12/31/" + sTmpYear;

                pRec.Query = @"select sum(fees_amtdue) from or_table where or_no in
                    (select or_no from pay_hist where or_date between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy'))";
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        sColAmt = string.Format("{0:####.00}", pRec.GetDouble(0));

                        arrDate.Add("Jan-Dec");
                        arrYear.Add(sTmpYear);
                        arrValue.Add(sColAmt);
                    }
                }
                pRec.Close();
            }

            // print
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            sData = "Month Covered|Year|Collected Amount";
            this.axVSPrinter1.Table = string.Format("^3000|^2000|^3000;{0}", sData);

            for (int j = 0; j < arrDate.Count; j++)
            {
                sData = arrDate[j].ToString() + "|";
                sData += arrYear[j].ToString() + "|";
                sData += string.Format("{0:#,###.00}", Convert.ToDouble(arrValue[j].ToString()));
                this.axVSPrinter1.Table = string.Format("<3000|<2000|>3000;{0}", sData);
            }


            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ComparativeReportNew()
        {
            // RMC 20180209 modified comparative report as requested by Malolos BPLO (format proposed by Jester)
            axVSPrinter1.FontBold = false;
            
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.MarginLeft = 500;

            string sText = string.Empty;
            string sFormat = string.Empty;
            string sPaidBns = string.Empty;
            string sPaidAmt = string.Empty;
            string sRegFeeAmt = string.Empty; //JHB 20180326


            string sAppBns = string.Empty;
            string sAppGC = string.Empty;

            string sDate = string.Empty;
            string sDatefr = string.Format("{0:MM/dd/yyyy}",m_dFrom);
            string sDateto = string.Format("{0:MM/dd/yyyy}", m_dTo);
            TimeSpan ts = m_dTo - m_dFrom;
            double dDays = ts.TotalDays + 1;
            DateTime dtDate = m_dFrom;

            int iTotAppBns = 0;
            int iTotPdBns = 0;
            double dTotGC = 0;
            double dTotPd = 0;
            double dTotRd = 0;   //JHB 20180326

            axVSPrinter1.FontUnderline = true;
            axVSPrinter1.Paragraph = "";
            sFormat = "<3000;RENEWAL";
            axVSPrinter1.Table = sFormat;
            axVSPrinter1.FontUnderline = false;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            sFormat = "^1500|^2000|^2000|^1500|^2000|<2000;";  
            sText = "DATE|NO. OF APPLICATION|TOTAL GROSS|NO. PAID BUSINESS|BUSINESS TAX PAID|REG. FEES PAID";
            axVSPrinter1.Table = sFormat + sText;
            sFormat = "^1500|>2000|>2000|>1500|>2000|>2000;";

            for (int i = 1; i<= Convert.ToInt32(dDays); i++)
            {
                ApplicationDetails(dtDate, "REN", out sAppBns, out sAppGC);
                PaymentDetails(dtDate, "REN", out sPaidBns, out sPaidAmt, out sRegFeeAmt); //  JHB 20180326

                sDate = string.Format("{0:MM/dd/yyyy}",dtDate);
                if (sAppBns.Trim() != "" || sPaidBns.Trim() != "")
                {
                    sText = sDate + "|" + sAppBns + "|" + sAppGC + "|" + sPaidBns + "|" + sPaidAmt + "|" + sRegFeeAmt;  // TRADEMARK JHB 20182603
                    axVSPrinter1.Table = sFormat + sText;
                    if(sAppBns == "") //JARS 20180215 AVOID CONVERTING NULL TO INTEGER/DOUBLE
                    {
                        sAppBns = "0";
                    }
                    iTotAppBns += Convert.ToInt32(sAppBns);
                    dTotGC += Convert.ToDouble(sAppGC);
                    iTotPdBns += Convert.ToInt32(sPaidBns);
                    dTotPd += Convert.ToDouble(sPaidAmt);
                    dTotRd += Convert.ToDouble(sRegFeeAmt);  // JHB 20180326

                }
                dtDate = dtDate.AddDays(1);
            }
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            PaymentDetails("REN", out sPaidBns, out sPaidAmt, out sRegFeeAmt);
            if (sPaidBns.Trim() != "")
            {
                axVSPrinter1.Paragraph = "";
                sFormat = "<8000;TAX YEAR BELOW " + m_dFrom.Year;
                axVSPrinter1.Table = sFormat;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                sFormat = "^1500|>2000|>2000|>1500|>2000|>2000;";
                sText = "|||" + sPaidBns + "|" + sPaidAmt + "|" + sRegFeeAmt; //JHB 20180326
                axVSPrinter1.Table = sFormat + sText;

                iTotPdBns += Convert.ToInt32(sPaidBns);
                dTotPd += Convert.ToDouble(sPaidAmt);
                dTotRd += Convert.ToDouble(sRegFeeAmt);  //JHB 20180326

            }

            sAppBns = string.Format("{0:#,###}",iTotAppBns);
            sAppGC = string.Format("{0:#,###.00}", dTotGC);
            sPaidBns = string.Format("{0:#,###}", iTotPdBns);
            sPaidAmt = string.Format("{0:#,###.00}", dTotPd);
            sRegFeeAmt = string.Format("{0:#,###.00}", dTotRd); //JHB 20180326


            this.axVSPrinter1.Paragraph = "";
            sText = "TOTAL|" + sAppBns + "|" + sAppGC + "|" + sPaidBns + "|" + sPaidAmt + "|" + sRegFeeAmt; //JHB 20180326
            axVSPrinter1.Table = sFormat + sText;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            axVSPrinter1.FontUnderline = true;
            axVSPrinter1.Paragraph = "";
            sFormat = "<3000;NEW";
            axVSPrinter1.Table = sFormat;
            axVSPrinter1.FontUnderline = false;

            iTotAppBns = 0;
            iTotPdBns = 0;
            dTotGC = 0;
            dTotPd = 0;
            dTotRd = 0;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            sFormat = "^1500|^2000|^2000|^1500|^2000|^2000;";
            sText = "DATE|NO. OF APPLICATION|TOTAL CAPITAL|NO. PAID BUSINESS|BUSINESS TAX PAID|REG. FEES PAID "; //JHB 20180326
            axVSPrinter1.Table = sFormat + sText;
            sFormat = "^1500|>2000|>2000|>1500|>2000|>2000;"; //2000

            dtDate = m_dFrom;
            for (int i = 1; i <= Convert.ToInt32(dDays); i++)
            {
                ApplicationDetails(dtDate, "NEW", out sAppBns, out sAppGC);
                PaymentDetails(dtDate, "NEW", out sPaidBns, out sPaidAmt ,out sRegFeeAmt);   // JHB 20182603

                sDate = string.Format("{0:MM/dd/yyyy}", dtDate);
                if (sAppBns.Trim() != "" || sPaidBns.Trim() != "")
                {
                    sText = sDate + "|" + sAppBns + "|" + sAppGC + "|" + sPaidBns + "|" + sPaidAmt + "|" + sRegFeeAmt;  // TRADEMARK JHB 20182603
                    axVSPrinter1.Table = sFormat + sText;
                    if (sAppBns == "")
                    {
                        sAppBns = "0";
                    }
                    iTotAppBns += Convert.ToInt32(sAppBns);
                    dTotGC += Convert.ToDouble(sAppGC);
                    iTotPdBns += Convert.ToInt32(sPaidBns);
                    dTotPd += Convert.ToDouble(sPaidAmt);
                    dTotRd += Convert.ToDouble(sRegFeeAmt);
                }
                dtDate = dtDate.AddDays(1);
            }

            PaymentDetails("NEW", out sPaidBns, out sPaidAmt, out sRegFeeAmt);
            if (sPaidBns.Trim() != "")
            {
                axVSPrinter1.Paragraph = "";
                sFormat = "<8000;TAX YEAR BELOW " + m_dFrom.Year;
                axVSPrinter1.Table = sFormat;

                sFormat = "^1500|>2000|>2000|>1500|>2000|>2000;";
                sText = "|||" + sPaidBns + "|" + sPaidAmt + "|" + sRegFeeAmt; // JHB 20182603
                axVSPrinter1.Table = sFormat + sText;

                iTotPdBns += Convert.ToInt32(sPaidBns);
                dTotPd += Convert.ToDouble(sPaidAmt);
                dTotRd += Convert.ToDouble(sRegFeeAmt);  // JHB 20182603
            }

            sAppBns = string.Format("{0:#,###}", iTotAppBns);
            sAppGC = string.Format("{0:#,###.00}", dTotGC);
            sPaidBns = string.Format("{0:#,###}", iTotPdBns);
            sPaidAmt = string.Format("{0:#,###.00}", dTotPd);
            sRegFeeAmt = string.Format("{0:#,###.00}", dTotRd); //JHB 20182603

            this.axVSPrinter1.Paragraph = "";
            sText = "TOTAL|" + sAppBns + "|" + sAppGC + "|" + sPaidBns + "|" + sPaidAmt + "|" + sRegFeeAmt;
            axVSPrinter1.Table = sFormat + sText;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ApplicationDetails(DateTime dtAppDate, string sStat, out string sNoBns, out string sGrossCap)
        {
            // RMC 20180209 modified comparative report as requested by Malolos BPLO (format proposed by Jester)
            OracleResultSet pRec = new OracleResultSet();
            sNoBns = "";
            sGrossCap = "";
                
            pRec.Query = "select count(*) from ";
            pRec.Query+= "(select bin from businesses where to_date(to_char(save_tm,'MM/dd/yyyy'),'MM/dd/yyyy') = to_date('"+dtAppDate.ToShortDateString()+"','MM/dd/yyyy') and bns_stat = '" + sStat + "' union ";
            pRec.Query += "select bin from business_que where to_date(to_char(save_tm,'MM/dd/yyyy'),'MM/dd/yyyy') = to_date('" + dtAppDate.ToShortDateString() + "','MM/dd/yyyy') and bns_stat = '" + sStat + "')";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sNoBns = string.Format("{0:#,###}", pRec.GetInt(0));
                }
            }
            pRec.Close();

            
            if (sStat == "REN")
            {
                pRec.Query = "select sum(gross) from bill_gross_info where tax_year = '" + dtAppDate.Year + "'";
            }
            else if(sStat == "NEW")
            {
                pRec.Query = "select sum(capital) from bill_gross_info where tax_year = '" + dtAppDate.Year + "'";
            }
            pRec.Query += " and bin in (select bin from businesses where to_date(to_char(save_tm,'MM/dd/yyyy'),'MM/dd/yyyy') = to_date('" + dtAppDate.ToShortDateString() + "','MM/dd/yyyy') and bns_stat = '" + sStat + "' union ";
            pRec.Query += "select bin from business_que where to_date(to_char(save_tm,'MM/dd/yyyy'),'MM/dd/yyyy') = to_date('" + dtAppDate.ToShortDateString() + "','MM/dd/yyyy') and bns_stat = '" + sStat + "')";
            if (pRec.Execute())
            {
                if (pRec.Read())
                    sGrossCap = string.Format("{0:#,###.00}", pRec.GetDouble(0));
            }
            pRec.Close();

            // RMC 20180502 retrieve declared gross and capital from tables prior to system migration of malolos (s)
            if (Convert.ToDouble(sGrossCap) == 0)
            {
                if (sStat == "REN")
                {
                    pRec.Query = "select sum(declared_gr) from declared_gross where tax_year = '" + dtAppDate.Year + "'";
                    pRec.Query += " and bin in (select bin from buss_hist where to_date(to_char(save_tm,'MM/dd/yyyy'),'MM/dd/yyyy') = to_date('" + dtAppDate.ToShortDateString() + "','MM/dd/yyyy') and bns_stat = '" + sStat + "')";
                }
                else if (sStat == "NEW")
                {
                    pRec.Query = string.Format(@"select coalesce(capital,0) + (select coalesce(sum(capital),0) from addl_bns_hist where bin in 
(select bin from buss_hist where to_date(to_char(save_tm,'MM/dd/yyyy'),'MM/dd/yyyy') = to_date('{0}','MM/dd/yyyy') 
and bns_stat = 'NEW')) from buss_hist where to_date(to_char(save_tm,'MM/dd/yyyy'),'MM/dd/yyyy') = to_date('{0}','MM/dd/yyyy') 
and bns_stat = 'NEW'", dtAppDate.ToShortDateString());

                }
               
                if (pRec.Execute())
                {
                    if (pRec.Read())
                        sGrossCap = string.Format("{0:#,###.00}", pRec.GetDouble(0));
                }
                pRec.Close();

            }
            // RMC 20180502 retrieve declared gross and capital from tables prior to system migration of malolos (e)
        }

        private void PaymentDetails(DateTime dtOrDate, string sStat, out string sNoBns, out string sTotAmt, out string sRegFeeAmt)  //JHB 20180326
        {
            // RMC 20180209 modified comparative report as requested by Malolos BPLO (format proposed by Jester)
            OracleResultSet pRec = new OracleResultSet();
            sNoBns = "";
            sTotAmt = "";
            sRegFeeAmt = ""; //JHB 20182603



            pRec.Query = "select count(distinct bin) from pay_hist where or_date = to_date('" + dtOrDate.ToShortDateString() + "','MM/dd/yyyy') ";
            pRec.Query += " and tax_year = '" + dtOrDate.Year + "' and bns_stat = '" + sStat + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                    sNoBns = string.Format("{0:#,###}", pRec.GetInt(0));
            }
            pRec.Close(); 

            //pRec.Query = "select sum(fees_amtdue) from or_Table where tax_year = '" + dtOrDate.Year + "'";
           // pRec.Query += " and or_no in";
           // pRec.Query += " (select or_no from pay_hist where or_date = to_date('" + dtOrDate.ToShortDateString() + "','MM/dd/yyyy') ";
           // pRec.Query += " and tax_year = '" + dtOrDate.Year + "' and bns_stat = '" + sStat + "')";
            pRec.Query = "select sum(fees_amtdue) from or_Table where fees_code = 'B' and tax_year = '" + dtOrDate.Year + "'";   // JHB 20182603 total of bns_tax
            pRec.Query += " and or_no in";
            pRec.Query += " (select or_no from pay_hist where or_date = to_date('" + dtOrDate.ToShortDateString() + "','MM/dd/yyyy') ";
            pRec.Query += " and tax_year = '" + dtOrDate.Year + "' and bns_stat = '" + sStat + "')";


            if (pRec.Execute())
            {
                if (pRec.Read())
                    sTotAmt = string.Format("{0:#,###.00}", pRec.GetInt(0));
            }
            pRec.Close();
            

            //for Reg.Fee total amount as per sir jester request
            pRec.Query = "select sum(fees_amtdue) from or_Table where fees_code != 'B' and tax_year = '" + dtOrDate.Year + "'";   // JHB 20182603 total of bns_tax
            pRec.Query += " and or_no in";
            pRec.Query += " (select or_no from pay_hist where or_date = to_date('" + dtOrDate.ToShortDateString() + "','MM/dd/yyyy') ";
            pRec.Query += " and tax_year = '" + dtOrDate.Year + "' and bns_stat = '" + sStat + "')";


            if (pRec.Execute())
            {
                if (pRec.Read())
                    sRegFeeAmt = string.Format("{0:#,###.00}", pRec.GetInt(0));
            }
            pRec.Close();
        }

        private void PaymentDetails(string sStat, out string sNoBns, out string sTotAmt, out string sRegFeeAmt) //JHB 20180326
        {
            // RMC 20180209 modified comparative report as requested by Malolos BPLO (format proposed by Jester)
            OracleResultSet pRec = new OracleResultSet();
            sNoBns = "";
            sTotAmt = "";
            sRegFeeAmt = "";
            

            pRec.Query = "select count(distinct bin) from pay_hist where ";
            pRec.Query += " or_date between to_date('" + m_dFrom.ToShortDateString() + "','MM/dd/yyyy') ";
            pRec.Query += " and to_date('" + m_dTo.ToShortDateString() + "','MM/dd/yyyy') ";
            pRec.Query += " and tax_year < '" + m_dFrom.Year + "' and bns_stat = '" + sStat + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                    sNoBns = string.Format("{0:#,###}", pRec.GetInt(0));
            }
            pRec.Close();

            pRec.Query = "select sum(fees_amtdue) from or_Table where fees_code = 'B' and tax_year = '" + m_dFrom.Year + "'";
            pRec.Query += " and or_no in";
            pRec.Query += " (select or_no from pay_hist where or_date between";
            pRec.Query += " to_date('" + m_dFrom.ToShortDateString() + "','MM/dd/yyyy') "; 
            pRec.Query += " and to_date('" + m_dTo.ToShortDateString() + "','MM/dd/yyyy') ";
            pRec.Query += " and tax_year < '" + m_dFrom.Year + "' and bns_stat = '" + sStat + "')";
            if (pRec.Execute())
            {
                if (pRec.Read())
                    sTotAmt = string.Format("{0:#,###.00}", pRec.GetInt(0));
            }
            pRec.Close();

            //JHB 20180326 total regulatory fee as sir jester request
            pRec.Query = "select sum(fees_amtdue) from or_Table where fees_code != 'B' and tax_year = '" + m_dFrom.Year + "'";
            pRec.Query += " and or_no in";
            pRec.Query += " (select or_no from pay_hist where or_date between";
            pRec.Query += " to_date('" + m_dFrom.ToShortDateString() + "','MM/dd/yyyy') ";
            pRec.Query += " and to_date('" + m_dTo.ToShortDateString() + "','MM/dd/yyyy') ";
            pRec.Query += " and tax_year < '" + m_dFrom.Year + "' and bns_stat = '" + sStat + "')";
            if (pRec.Execute())
            {
                if (pRec.Read())
                    sRegFeeAmt = string.Format("{0:#,###.00}", pRec.GetInt(0));
            }
            pRec.Close();


        }
        #region CompGrossRevenue
        /*
        private void CompGrossRevenue()
        {
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            string sBrgyName = "";
            string sBnsName = "";
            string sBnsCategory = "";
            string sBnsSubCategory = "";
            string sBnsCode = "";
            string sBin = "";
            string sPrevBin = "";
            string sGross = "";
            string sYearFrom = "";
            string sYearTo = "";
            string sGross1 = "";
            string sGross2 = "";
            string sGross3 = "";
            Object lngY;

            if (m_sBrgyName == "ALL" || m_sBrgyName == "")
            {
                m_sBrgyName = "%%";
            }

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;

            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();

            Thread.Sleep(500);
            pRec.Query = "select count(*) from ( SELECT * from ( ";
            pRec.Query += "select bin, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2015' THEN gr_1 END), 0.00) AS gross_1, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2016' THEN gr_1 END), 0.00) AS gross_2, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2017' THEN gr_1 END), 0.00) AS gross_3, ";
            pRec.Query += "tax_year ";
            pRec.Query += "FROM buss_hist where tax_year between '2015' and '2017' ";
            pRec.Query += "GROUP BY bin,tax_year order by bin) ";
            pRec.Query += "union all SELECT * FROM ( ";
            pRec.Query += "select bin, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2015' THEN gr_1 END), 0.00) AS gross_1, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2016' THEN gr_1 END), 0.00) AS gross_2, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2017' THEN gr_1 END), 0.00) AS gross_3, ";
            pRec.Query += "tax_year ";
            pRec.Query += "FROM businesses where tax_year between '2015' and '2017' ";
            pRec.Query += "GROUP BY bin,tax_year order by bin) ";
            pRec.Query += ") order by bin";
            int.TryParse(pRec.ExecuteScalar(), out intCount);

            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion


            sYearFrom = m_sYear1;

            if (m_sYear3 == "")
            {
                sYearTo = m_sYear2;
            }
            else
            {
                sYearTo = m_sYear3;
            }

            pRec.Query = "select distinct bin,bns_nm,bns_code from ( SELECT * from ( ";
            pRec.Query += "select bin,bns_nm,bns_code,bns_brgy, ";
            #region comments
            //pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '"+ m_sYear1 +"' THEN gr_1 END), 0.00) AS gross_1, ";
            //pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '"+ m_sYear2 +"' THEN gr_1 END), 0.00) AS gross_2, ";
            //if(m_sYear3 != "")
            //{
            //    pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear3 + "' THEN gr_1 END), 0.00) AS gross_3, ";
            //}
            #endregion
            pRec.Query += "tax_year ";
            pRec.Query += "FROM buss_hist where tax_year between '" + sYearFrom + "' and '" + sYearTo + "' and bns_brgy like '" + m_sBrgyName + "' and bns_stat <> 'NEW' ";
            pRec.Query += "GROUP BY bin,tax_year,bns_nm,bns_code,bns_brgy order by bin) ";
            pRec.Query += "union all SELECT * FROM ( ";
            pRec.Query += "select bin,bns_nm,bns_code,bns_brgy, ";
            #region comments
            //pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear1 + "' THEN gr_1 END), 0.00) AS gross_1, ";
            //pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear2 + "' THEN gr_1 END), 0.00) AS gross_2, ";
            //if (m_sYear3 != "")
            //{
            //    pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear3 + "' THEN gr_1 END), 0.00) AS gross_3, ";
            //}
            #endregion
            pRec.Query += "tax_year ";
            pRec.Query += "FROM businesses where tax_year between '" + sYearFrom + "' and '" + sYearTo + "' and bns_brgy like '" + m_sBrgyName + "' and bns_stat <> 'NEW' ";
            pRec.Query += "GROUP BY bin,tax_year,bns_nm,bns_code,bns_brgy order by bin) ";
            pRec.Query += ") order by bns_nm";

            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sGross1 = "";
                    sGross2 = "";
                    sGross3 = "";
                    sBnsName = pRec.GetString("bns_nm");
                    sBin = pRec.GetString("bin");
                    sBnsSubCategory = AppSettingsManager.GetBnsDesc(pRec.GetString("bns_code"));
                    sBnsCode = pRec.GetString("bns_code");

                    if (sBin != sPrevBin)
                    {
                        #region comments
                        //pRec2.Query = "select gross_1,gross_2,gross_3,tax_year from ( SELECT * from (";
                        //pRec2.Query += "select bin,bns_nm,bns_code,bns_brgy, ";
                        //pRec2.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear1 + "' THEN gr_1 END), 0.00) AS gross_1, ";
                        //pRec2.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear2 + "' THEN gr_1 END), 0.00) AS gross_2, ";
                        //if (m_sYear3 != "")
                        //{
                        //    pRec2.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear3 + "' THEN gr_1 END), 0.00) AS gross_3, ";
                        //}
                        //pRec2.Query += "tax_year ";
                        //pRec2.Query += "FROM buss_hist where bin = '" + sBin + "' and tax_year between '" + sYearFrom + "' and '" + sYearTo + "' and bns_brgy like '" + m_sBrgyName + "' ";
                        //pRec2.Query += "GROUP BY bin,tax_year,bns_nm,bns_code,bns_brgy order by bin) ";
                        //pRec2.Query += "union all SELECT * FROM ( ";
                        //pRec2.Query += "select bin,bns_nm,bns_code,bns_brgy, ";
                        //pRec2.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear1 + "' THEN gr_1 END), 0.00) AS gross_1, ";
                        //pRec2.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear2 + "' THEN gr_1 END), 0.00) AS gross_2, ";
                        //if (m_sYear3 != "")
                        //{
                        //    pRec2.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear3 + "' THEN gr_1 END), 0.00) AS gross_3, ";
                        //}
                        //pRec2.Query += "tax_year ";
                        //pRec2.Query += "FROM businesses where bin = '" + sBin + "' and tax_year between '" + sYearFrom + "' and '" + sYearTo + "' and bns_brgy like 'BALANTACAN' ";
                        //pRec2.Query += "GROUP BY bin,tax_year,bns_nm,bns_code,bns_brgy order by bin) ";
                        //pRec2.Query += ") order by tax_year";
                        #endregion
                        //JARS 20180402 GOT FROM BILL_GROSS_INFO INSTEAD
                        pRec2.Query = "select gross_1,gross_2, ";
                        if (m_sYear3 != "")
                        {
                            pRec2.Query += "gross_3, ";
                        }
                        pRec2.Query += "tax_year from ( ";
                        pRec2.Query += "SELECT * from ( ";
                        pRec2.Query += "select bin,bns_code, ";
                        pRec2.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear1 + "' THEN gross END), 0.00) AS gross_1, ";
                        pRec2.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear2 + "' THEN gross END), 0.00) AS gross_2, ";
                        if (m_sYear3 != "")
                        {
                            pRec2.Query += "COALESCE(MAX(CASE WHEN tax_year = '" + m_sYear3 + "' THEN gross END), 0.00) AS gross_3, ";
                        }
                        pRec2.Query += "tax_year FROM ";
                        pRec2.Query += "bill_gross_info where bin = '" + sBin + "' and tax_year between '" + sYearFrom + "' and '" + sYearTo + "' and bns_code = '" + pRec.GetString("bns_code") + "' ";
                        pRec2.Query += "GROUP BY bin,tax_year,bns_code order by tax_year))";

                        if (pRec2.Execute())
                        {
                            while (pRec2.Read())
                            {
                                if (pRec2.GetDouble("gross_1") != 0)
                                {
                                    sGross1 = pRec2.GetDouble("gross_1").ToString("#,##0.00");
                                }
                                if (pRec2.GetDouble("gross_2") != 0)
                                {
                                    sGross2 = pRec2.GetDouble("gross_2").ToString("#,##0.00");
                                }
                                if (m_sYear3 != "")
                                {
                                    if (pRec2.GetDouble("gross_3") != 0)
                                    {
                                        sGross3 = pRec2.GetDouble("gross_3").ToString("#,##0.00");
                                    }
                                }
                            }
                        }
                        pRec2.Close();

                        pRec2.Query = "select * from bns_table where fees_code = 'B' and bns_code like '" + sBnsCode.Substring(0, 2) + "'";
                        if (pRec2.Execute())
                        {
                            if (pRec2.Read())
                            {
                                sBnsCategory = pRec2.GetString("bns_desc");
                            }
                        }
                        pRec2.Close();
                        lngY = Convert.ToInt32(axVSPrinter1.CurrentY);
                        if (m_sYear3 != "")
                        {
                            if (sGross1 == "")
                            {
                                sGross1 = "No Gross Declaration";
                            }
                            if (sGross2 == "")
                            {
                                sGross2 = "No Gross Declaration";
                            }
                            if (sGross3 == "")
                            {
                                sGross3 = "No Gross Declaration";
                            }

                            axVSPrinter1.Table = string.Format("^2000|<5000|<4000|<4000|^1500|^1500|^1500 ;" + sBin + "|" + sBnsName + "|" + sBnsCategory + "|" + sBnsSubCategory + "|" + sGross1 + "|" + sGross2 + "|" + sGross3);
                        }
                        else
                        {
                            if (sGross1 == "")
                            {
                                sGross1 = "No Gross Declaration";
                            }
                            if (sGross2 == "")
                            {
                                sGross2 = "No Gross Declaration";
                            }
                            axVSPrinter1.Table = string.Format("^2000|<5000|<4000|<4000|^1500|^1500 ;" + sBin + "|" + sBnsName + "|" + sBnsCategory + "|" + sBnsSubCategory + "|" + sGross1 + "|" + sGross2);
                        }
                    }
                    sPrevBin = pRec.GetString("bin");

                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            pRec.Close();
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<5000|<5000;Printed by: {0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<5000|<5000;Date printed: {0}", AppSettingsManager.GetCurrentDate());

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);
        }
        */
        #endregion
        private void BussRollSummary()
        {
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            string sBrgyName = "";
            string sBnsName = "";
            string sBnsCategory = "";
            string sBnsSubCategory = "";
            string sBnsCode = "";
            string sBin = "";
            string sPrevBin = "";
            string sGross = "";
            string sYearFrom = "";
            string sYearTo = "";
            string sGross1 = "";
            string sGross2 = "";
            string sGross3 = "";
            string sStat = "NEW";

            int iTotNew1 = 0;
            int iTotNew2 = 0;
            int iTotNew3 = 0;
            int iTotRen1 = 0;
            int iTotRen2 = 0;
            int iTotRen3 = 0;
            Double dCapital1 = 0;
            Double dCapital2 = 0;
            Double dCapital3 = 0;
            List<string> lstStat = new List<string>(); ;
            lstStat.Add("NEW");
            lstStat.Add("REN");

            string sBnsStat = "";
            Object lngY;

            if (m_sBrgyName == "ALL" || m_sBrgyName == "")
            {
                m_sBrgyName = "%%";
            }
            #region Progress
            /*
            int intCount = 0;
            int intCountIncreament = 0;

            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();

            Thread.Sleep(500);
            pRec.Query = "select count(*) from ( SELECT * from ( ";
            pRec.Query += "select bin, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2015' THEN gr_1 END), 0.00) AS gross_1, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2016' THEN gr_1 END), 0.00) AS gross_2, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2017' THEN gr_1 END), 0.00) AS gross_3, ";
            pRec.Query += "tax_year ";
            pRec.Query += "FROM buss_hist where tax_year between '2015' and '2017' ";
            pRec.Query += "GROUP BY bin,tax_year order by bin) ";
            pRec.Query += "union all SELECT * FROM ( ";
            pRec.Query += "select bin, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2015' THEN gr_1 END), 0.00) AS gross_1, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2016' THEN gr_1 END), 0.00) AS gross_2, ";
            pRec.Query += "COALESCE(MAX(CASE WHEN tax_year = '2017' THEN gr_1 END), 0.00) AS gross_3, ";
            pRec.Query += "tax_year ";
            pRec.Query += "FROM businesses where tax_year between '2015' and '2017' ";
            pRec.Query += "GROUP BY bin,tax_year order by bin) ";
            pRec.Query += ") order by bin";
            int.TryParse(pRec.ExecuteScalar(), out intCount);

            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            */
            #endregion
            sYearFrom = m_sYear1;

            if (m_sYear3 == "")
            {
                sYearTo = m_sYear2;
            }
            else
            {
                sYearTo = m_sYear3;
            }
             
            //pRec.Query = "select distinct * from ";
            //pRec.Query += "( ";
            //pRec.Query += "select bin,bns_nm,bns_stat,tax_year,capital from businesses where tax_year between '"+ sYearFrom +"' and '"+ sYearTo +"' and bns_stat <> 'RET' ";
            //pRec.Query += "union all ";
            //pRec.Query += "select bin,bns_nm,bns_stat,tax_year,capital from buss_hist where tax_year between '" + sYearFrom + "' and '" + sYearTo + "' and bns_stat <> 'RET' ";
            //pRec.Query += "union all ";
            //pRec.Query += "select bin,bns_nm,bns_stat,tax_year,capital from business_que where tax_year between '" + sYearFrom + "' and '" + sYearTo + "' and bns_stat <> 'RET' ";
            //pRec.Query += ") order by tax_year";

            //mjbb 20180710 changed economic data query, must tally with buss. roll (s)
            int i;
            int from = Convert.ToInt32(sYearFrom);
            int to = Convert.ToInt32(sYearTo);

                   pRec.Query = string.Format("select distinct bns_brgy from businesses where bns_brgy like '%%' order by bns_brgy");
                   if (pRec.Execute())
                   {
                       while (pRec.Read())
                       {
                           for (i = from; i <= to; i++)
                           {
                               foreach (string stat in lstStat)
                               {
                               blnHasRecord = false;
                               strBrgyTmp = pRec.GetString("bns_brgy");

                               pRec2.Query = "select * from businesses where ";
                               pRec2.Query += string.Format("rtrim(businesses.bns_brgy) like '{2}' and rtrim(businesses.bns_stat) like '{0}' and businesses.tax_year = '{1}' ", stat, i, strBrgyTmp);
                               pRec2.Query += "union all select * from buss_hist where ";
                               pRec2.Query += string.Format("rtrim(buss_hist.bns_brgy) like '{2}' and rtrim(buss_hist.bns_stat) like '{0}' and buss_hist.tax_year = '{1}'", stat, i, strBrgyTmp);
                               pRec2.Query += string.Format(" and bin not in (select bin from businesses where tax_year = '{0}')", i);   


                               if (pRec2.Execute())
                               {
                                   while (pRec2.Read())
                                   {
                                       blnHasRecord = true;
                                       break;
                                   }
                               }
                               pRec2.Close();

                               if (blnHasRecord)
                               {
                                   if (pRec2.Execute())
                                   {
                                       while (pRec2.Read())
                                       {
                                           if (pRec2.GetString("bin") != sPrevBin)
                                           {
                                               if (stat == "NEW")
                                               {
                                                   if (i.ToString() == m_sYear1)
                                                   {
                                                       iTotNew1++;
                                                       dCapital1 += pRec2.GetDouble("capital");
                                                   }
                                                   else if (i.ToString() == m_sYear2)
                                                   {
                                                       iTotNew2++;
                                                       dCapital2 += pRec2.GetDouble("capital");
                                                   }
                                                   else if (m_sYear3 != "")
                                                   {
                                                       if (i.ToString() == m_sYear3)
                                                       {
                                                           iTotNew3++;
                                                           dCapital3 += pRec2.GetDouble("capital");
                                                       }
                                                   }
                                               }
                                               else if (stat == "REN")
                                               {
                                                   if (i.ToString() == m_sYear1)
                                                   {
                                                       iTotRen1++;
                                                   }
                                                   else if (i.ToString() == m_sYear2)
                                                   {
                                                       iTotRen2++;
                                                   }
                                                   else if (m_sYear3 != "")
                                                   {
                                                       if (i.ToString() == m_sYear3)
                                                       {
                                                           iTotRen3++;
                                                       }
                                                   }
                                               }

                                               rs2.Query = string.Format("select gross,capital from addl_bns where bin = '{0}' and tax_year = '{1}'", pRec2.GetString("bin"), i);
                                               if (rs2.Execute())
                                               {
                                                   while (rs2.Read())
                                                   {
                                                       if (stat == "NEW")
                                                       {
                                                           if (i.ToString() == m_sYear1)
                                                           {
                                                               dCapital1 += rs2.GetDouble("capital");
                                                           }
                                                           else if (i.ToString() == m_sYear2)
                                                           {
                                                               dCapital2 += rs2.GetDouble("capital");
                                                           }
                                                           else if (m_sYear3 != "")
                                                           {
                                                               if (i.ToString() == m_sYear3)
                                                               {
                                                                   dCapital3 += rs2.GetDouble("capital");
                                                               }
                                                           }
                                                       }
                                                   }
                                               }
                                               rs2.Close();
                                           }
                                           sPrevBin = pRec2.GetString("bin");
                                       }
                                   }
                               }
                               }   
                           }
                       }
                   }
                   pRec.Close();
                   //mjbb 20180710 changed economic data query, must tally with buss. roll (e)

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            axVSPrinter1.Table = "^11000; SUMMARY OF ECONOMIC DATA";
            lngY = Convert.ToInt32(axVSPrinter1.CurrentY);
            axVSPrinter1.Table = "^5000|^6000; No. of Businesses based on Application \n |Tax Year";
            axVSPrinter1.CurrentY = Convert.ToInt32(lngY) + 288;
            axVSPrinter1.MarginLeft = 5500;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            if (m_sYear3 != "")
            {
                axVSPrinter1.Table = "^2000|^2000|^2000;" + m_sYear1 + "|" + m_sYear2 + "|" + m_sYear3;
                axVSPrinter1.MarginLeft = 500;
                axVSPrinter1.Table = "^5000|^2000|^2000|^2000;Total No. of New Businesses|" + iTotNew1.ToString("#,##0") + "|" + iTotNew2.ToString("#,##0") + "|" + iTotNew3.ToString("#,##0");
                axVSPrinter1.Table = "^5000|^2000|^2000|^2000;Total No. of Renewed Businesses|" + iTotRen1.ToString("#,##0") + "|" + iTotRen2.ToString("#,##0") + "|" + iTotRen3.ToString("#,##0");
            }
            else
            {
                axVSPrinter1.Table = "^3000|^3000;" + m_sYear1 + "|" + m_sYear2;
                axVSPrinter1.MarginLeft = 500;
                axVSPrinter1.Table = "^5000|^3000|^3000;Total No. of New Businesses|" + iTotNew1.ToString("#,##0") + "|" + iTotNew2.ToString("#,##0");
                axVSPrinter1.Table = "^5000|^3000|^3000;Total No. of Renewed Businesses|" + iTotRen1.ToString("#,##0") + "|" + iTotRen2.ToString("#,##0");
            }
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";

            axVSPrinter1.Table = "^11000; TOTAL CAPITAL COLLECTION";
            lngY = Convert.ToInt32(axVSPrinter1.CurrentY);
            axVSPrinter1.Table = "^5000|^6000; \n |Tax Year";
            axVSPrinter1.CurrentY = Convert.ToInt32(lngY) + 288;
            axVSPrinter1.MarginLeft = 5500;
            if (m_sYear3 != "")
            {
                axVSPrinter1.Table = "^2000|^2000|^2000;" + m_sYear1 + "|" + m_sYear2 + "|" + m_sYear3;
                axVSPrinter1.MarginLeft = 500;
                axVSPrinter1.Table = "^5000|^2000|^2000|^2000;Capital Investments derived from registered New Businesses|" + dCapital1.ToString("#,##0.00") + "|" + dCapital2.ToString("#,##0.00") + "|" + dCapital3.ToString("#,##0.00");
            }
            else
            {
                axVSPrinter1.Table = "^3000|^3000;" + m_sYear1 + "|" + m_sYear2;
                axVSPrinter1.MarginLeft = 500;
                axVSPrinter1.Table = "^5000|^3000|^3000;Capital Investments derived from registered New Businesses|" + dCapital1.ToString("#,##0.00") + "|" + dCapital2.ToString("#,##0.00");
            }
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }
    }
}