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
using System.Collections;
using Amellar.Common.DynamicProgressBar;
using System.Threading;
using Amellar.Modules.RCD;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmLiqReports : Form
    {
        public frmLiqReports()
        {
            InitializeComponent();
        }

        static string sImagepath = string.Empty; //JAV 20170906 
        private SystemUser m_objSystemUser = new SystemUser(); // JAV 20170918
        private string m_sCap = string.Empty;   // RMC 20140122 view capital, gross, pre-gross on payment hist (s)
        private string m_sPreGross = string.Empty;  // RMC 20140122 view capital, gross, pre-gross on payment hist (e)
        string m_sGross = string.Empty;
        public string m_sQtrPAid = string.Empty;

        private string m_sReportTitle = String.Empty;
        private string m_sTeller = String.Empty;
        private string m_sReportSwitch = String.Empty;
        //private string m_sBin = String.Empty; //JAV 20170906 
        private int m_iReportSwitch = 0;
        private DateTime m_dtFrom;
        private DateTime m_dtTo;
        private DateTime m_dtDate;
        private bool m_bBatch2 = false; // RMC 20150126
        private int m_iBatchCnt = 0; // RMC 20150126

        //JMBC 20140926
        private string m_sBCode = String.Empty;
        private string m_sArea = String.Empty;
        private DateTime m_dtBillDate;
        private string m_sORDate;
        private string m_sORno = String.Empty;
        private int m_iSelectedData = 0;

        //JMBC 20141001

        private string m_sBnsName = String.Empty;
        private string m_sBnsAddress = String.Empty;
        private string m_sBnsOwner = String.Empty;
        private string m_sOwnCode = String.Empty;
        private string m_sTerm = String.Empty;
        private string m_sForm = String.Empty;
        private bool m_bPage2 = false;  // RMC 20161230 modified abstract teller report for Binan
        private bool m_bORDate = false; // RMC 20170123 Added RCD date option in Abstract of Teller
        private string m_sRCDNo = string.Empty; // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date

        public bool BasedonORDate   // RMC 20170123 Added RCD date option in Abstract of Teller
        {
            set { m_bORDate = value; }
        }

        private string m_sSelectedbrgy; //AFM 20200204

        public string SelectedBrgy
        {
            get { return m_sSelectedbrgy; }
            set { m_sSelectedbrgy = value; }
        }
	

        //MCR 20140714 (s)
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
        public bool m_bReprintOR = false;
        public string m_sBrgy = "";
        //MCR 20140714 (e)

        private bool bSolo = false; //MCR 20190613
        private bool bPreprinted = false;
        public bool PreprintedFrom
        {
            set { bPreprinted = value; }
        }

        private string m_sReportFormat = String.Empty;

        //MCR 20140818 (s)
        string sFormat = "";
        ArrayList arrColumnName = new ArrayList();
        ArrayList arrColumnCode = new ArrayList();
        ArrayList arrAbv8 = new ArrayList();
        //MCR 20140818 (e)

        // RMC 20161230 modified abstract teller report for Binan (s)
        ArrayList arrTaxName = new ArrayList();
        ArrayList arrTaxCode = new ArrayList();
        ArrayList arrTaxAbv8 = new ArrayList();
        // RMC 20161230 modified abstract teller report for Binan (e)

        //MCR 20140819 (s)
        public string ORFrom
        {
            set { m_sOrFrom = value; }
        }
        public string ORTo
        {
            set { m_sOrTo = value; }
        }
        private string m_sOrFrom = String.Empty;
        private string m_sOrTo = String.Empty;
        private string m_sRCDNumber = "";
        public string RCDNumber
        {
            set { m_sRCDNumber = value; }
        }
        //MCR 20140819 (e)

        private bool m_bInit = true;

        //MCR 20140905 ReprintOR (s)
        private DataGridView m_dgView = null;
        public DataGridView DataGridView
        {
            set { m_dgView = value; }
        }
        public string m_sBIN = string.Empty;
        public string m_sTaxYear = string.Empty;
        public string m_sStatus = string.Empty;
        //MCR 20140905 ReprintOR (e)

        // RMC 20141020 printing of OR (s)
        private string m_sPaymentType = string.Empty;
        private double m_dToBeCredited = 0;
        public string PaymentType   
        {
            set { m_sPaymentType = value; }
        }
        public double ToBeCredited
        {
            set { m_dToBeCredited = value; }
        }
        // RMC 20141020 printing of OR (e)

        public bool ShowGrossNCapital
        {
            set { m_bShowGrossNCapital = value; }
        }
        private bool m_bShowGrossNCapital = false;

        private void ExportFile() //MCR 20140828
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
                    axVSPrinter1.ExportFormat = VSPrinter7Lib.ExportFormatSettings.vpxPlainHTML;
                    axVSPrinter1.ExportFile = dlg.FileName;
                }
            }
        }
        //JMBC 20140926
        public string BCode
        {
            set { m_sBCode = value; }
        }

        public int SelectedData
        {
            set { m_iSelectedData = value; }
        }
        public string Area
        {
            set { m_sArea = value; }
        }

        public DateTime BillDate
        {
            set { m_dtBillDate = value; }
        }

        public String ORDate
        {
            set { m_sORDate = value; }

        }

        public string ORNo
        {
            set { m_sORno = value; }
        }

        //JMBC 20141001
        public string bnsName
        {
            set { m_sBnsName = value; }
        }

        public string bnsAddress
        {
            set { m_sBnsAddress = value; }
        }

        public string bnsOwner
        {
            set { m_sBnsOwner = value; }
        }

        public string ownCode
        {
            set { m_sOwnCode = value; }
        }

        public string term
        {
            set { m_sTerm = value; }
        }

        public string formUse
        {
            set { m_sForm = value; }
        }

        //MCR 20150130
        private string m_sFund = string.Empty;
        public string Fund
        {
            set { m_sFund = value; }
        }
                
        public string ReportFormat
        {
            set { m_sReportFormat = value; }
        }

        public string ReportTitle
        {
            set { m_sReportTitle = value; }
        }

        public string Teller
        {
            set { m_sTeller = value; }
        }

        public DateTime Date
        {
            set { m_dtDate = value; }
        }

        public DateTime DateFrom
        {
            set { m_dtFrom = value; }
        }

        public DateTime DateTo
        {
            set { m_dtTo = value; }
        }

        public string sTaxYear
        {
            get { return m_sTaxYear; }
            set { m_sTaxYear = value; }
        }

        private string m_sQuery = "";
        public string Query
        {
            set { m_sQuery = value; }
        }

        public string ReportSwitch
        {
            set { m_sReportSwitch = value; }
        }

        public int iReportSwitch
        {
            get { return m_iReportSwitch; }
            set { m_iReportSwitch = value; }
        }

        //MCR 20190509 (s) Gross income report
        private string m_sFilterValue = string.Empty;
        public string sFilterValue
        {
            get { return m_sFilterValue; }
            set { m_sFilterValue = value; }
        }
        //MCR 20190509 (e) Gross income report

        private void frmLiqReports_Load(object sender, EventArgs e)
        {
            if (m_sReportSwitch == "ListOfCancelledChecks")
            {
                this.axVSPrinter1.MarginLeft = 700;
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; //pprFanfoldUS
                this.axVSPrinter1.MarginBottom = 700;
                this.axVSPrinter1.MarginTop = 700;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            }
            else if (m_sReportSwitch == "ListOfCancelledChecks"
               || m_sReportSwitch == "AbstractCollectByDaily"
               || m_sReportSwitch == "AbstractCollectByOR"
            || m_sReportSwitch == "AbstractCollectByTeller") // for landscape reports
            {
                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = @"select TAFT.fees_code,TAFT.fees_desc from btax_rep_abstract BRA inner join tax_and_fees_table TAFT on TAFT.fees_code = BRA.fees_code
where BRA.user_code = '" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode) + "' and report_name = '" + m_sReportTitle + "'";
                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        arrColumnCode.Add(pSet.GetString(0).Trim());
                        arrColumnName.Add(pSet.GetString(1).Trim());
                    }
                pSet.Close();

                // if (m_sReportSwitch == "AbstractCollectByTeller")
                //this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; //orLandscape orPortrait
                if(m_iReportSwitch == 3)
                    this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; //orLandscape orPortrait
                else
                    this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; //AFM 20200123 MAO-20-11975
                //else
                //  this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; //orLandscape orPortrait
                //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS; // pprLegal pprLetter
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; //AFM 20200123 MAO-20-11975
                //this.axVSPrinter1.MarginLeft = 300;
                this.axVSPrinter1.MarginLeft = 900;//AFM 20200123 MAO-20-11975
                this.axVSPrinter1.MarginBottom = 700;
                 this.axVSPrinter1.MarginTop = 700;
                //this.axVSPrinter1.MarginBottom = 2000;
            }
            else if (m_sReportSwitch.Contains("SummaryOfCollection"))
            {
                ExportFile();

                //    if (m_sReportSwitch == "OnReportWideSummaryOfCollectionBarangay")
                /*this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; //orLandscape orPortrait
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // pprLegal pprLetter*/

                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; //orLandscape orPortrait
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS;

                //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprQuarto;
                //this.axVSPrinter1.PaperHeight = 13000;
                //this.axVSPrinter1.PaperWidth = 30840;
                //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;

                this.axVSPrinter1.MarginLeft = 200;
                this.axVSPrinter1.MarginBottom = 500;
                this.axVSPrinter1.MarginTop = 500;
            }
            else if (m_sReportSwitch.Contains("ReprintOR")) //MCR 20140905 ReprintOR
            {
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; //orLandscape orPortrait
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4; // pprLegal pprLetter
                this.axVSPrinter1.MarginLeft = 1200;
                this.axVSPrinter1.MarginBottom = 100;
                this.axVSPrinter1.MarginTop = 1200; //1550
            }
            else if (m_sReportSwitch.Contains("ACEAbstractCollection")) //MCR 20150130
            {
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; //orLandscape orPortrait
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // pprLegal pprLetter
                this.axVSPrinter1.MarginLeft = 700;
                this.axVSPrinter1.MarginTop = 700;
                this.axVSPrinter1.MarginBottom = 1600;
            }
            else if (m_sReportSwitch == "AbstractCollectByTellerBinan") // RMC 20161230 modified abstract teller report for Binan
            {
                m_bPage2 = false;  // RMC 20161230 modified abstract teller report for Binan
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // pprLegal pprLetter
                
                this.axVSPrinter1.MarginLeft = 300;
                this.axVSPrinter1.MarginBottom = 700;
                this.axVSPrinter1.MarginTop = 700;

                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = @"select TAFT.fees_code,TAFT.fees_desc from btax_rep_abstract BRA inner join tax_and_fees_table TAFT on TAFT.fees_code = BRA.fees_code
where BRA.user_code = '" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode) + "' and report_name = '" + m_sReportTitle + "' and BRA.fees_code not like 'B%'";
                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        arrColumnCode.Add(pSet.GetString(0).Trim());
                        arrColumnName.Add(pSet.GetString(1).Trim());
                    }
                pSet.Close();

                pSet.Query = @"select TAFT.bns_code,TAFT.bns_desc from btax_rep_abstract BRA inner join bns_table TAFT on TAFT.fees_code || TAFT.bns_code = BRA.fees_code
where BRA.user_code = '" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode) + "' and report_name = '" + m_sReportTitle + "' and BRA.fees_code like 'B%'";
                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        arrTaxCode.Add(pSet.GetString(0).Trim());
                        arrTaxName.Add(pSet.GetString(1).Trim());
                    }
                pSet.Close();
            }

            else
            {
                if (m_sReportSwitch == "Report_of_Abstract_of_Checks")
                {
                    this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                    this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                    this.axVSPrinter1.MarginLeft = 700; //1000
                    this.axVSPrinter1.MarginTop = 700;
                    this.axVSPrinter1.MarginBottom = 700;
                }
                // RMC 20150504 QA corrections (s)
                else if (m_sReportSwitch == "Liquidating Officer RCD")
                {
                    this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                    this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                    this.axVSPrinter1.MarginLeft = 1000; //1000
                    this.axVSPrinter1.MarginTop = 700;
                    this.axVSPrinter1.MarginBottom = 700;
                }
                // JAV 20170906 Payment History (s)
                else if (m_sReportSwitch == "Payment History")
                {
                    this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                    this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                    this.axVSPrinter1.MarginLeft = 1000;
                    this.axVSPrinter1.MarginTop = 700;
                    this.axVSPrinter1.MarginBottom = 700;
                }
                else if (m_sReportSwitch == "PayHistDetails")
                {
                    this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                    this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                    this.axVSPrinter1.MarginLeft = 1000;
                    this.axVSPrinter1.MarginTop = 700;
                    this.axVSPrinter1.MarginBottom = 700;
                }
                // JAV 20170906 Payment History (e)
                // RMC 20150504 QA corrections (e)
                else
                {
                    if (m_sReportSwitch == "TellerTransaction")
                        this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                    else
                        this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;

                    this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                    if (m_sReportSwitch != "OnReportTotalCollections")
                    {
                        this.axVSPrinter1.MarginLeft = 400; //1000
                        this.axVSPrinter1.MarginTop = 700;
                        this.axVSPrinter1.MarginBottom = 700;
                    }
                    else
                    {
                        this.axVSPrinter1.MarginLeft = 700; //1000
                        this.axVSPrinter1.MarginTop = 700;
                        this.axVSPrinter1.MarginBottom = 700;
                    }
                }
            }

            if (m_sReportSwitch.Contains("AbstractCollect") && !m_sReportSwitch.Contains("ACE"))
                ExportFile();

            if (m_sReportSwitch == "Negative List") // AFM 20200310
                ExportFile();

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

            if (m_sReportSwitch.Contains("SummaryOfCollection"))
            {
                m_bInit = false;

                // RMC 20150126 (S)
                m_iBatchCnt = 1;
                // RMC 20150126 (E)

                CreateHeaderLubao();
            }

            if (m_sReportSwitch == "ACEAbstractCollection")
            {
                CreateHeaderLubao();
                ACEAbstractCollection();
            }
            // JAV 20170906 (s)
            else if (m_sReportSwitch == "Payment History")
            {
                CreateHeader();
                ReportPaymentHist(m_sBIN);
            }
            else if (m_sReportSwitch == "PayHistDetails")
            {
                CreateHeader();
                ReportPayHistDetails(m_sBIN, m_sTaxYear, m_sORno, m_sQtrPAid);
                
            }
            // JAV 20170906 (e)
            else if (m_sReportSwitch == "ReprintOR" || m_sReportSwitch == "PrintOR") // RMC 20141020 printing of OR
                ReprintORLUBAO();
            //ReprintOR();
            else if (m_sReportSwitch == "ListOfCancelledChecks")
                ListOfCancelledChecks();
            else if (m_sReportSwitch == "TellerTransaction")
                TellerTransaction();
            else if (m_sReportSwitch == "CancelledOR")
                CancelledOR();
            else if (m_sReportSwitch == "Report_of_Abstract_of_Checks")
            {
                CreateHeader(); //AFM 20200420
                ReportofAbstractofChecks();
            }
            else if (m_sReportSwitch.Contains("AbstractCollect"))
            {
                m_bInit = false;

                // RMC 20161230 modified abstract teller report for Binan (s)
                if (m_sReportSwitch == "AbstractCollectByTellerBinan")
                {
                    /*
                    CreateHeaderPage1Binan();
                    if (!m_bORDate) // RMC 20170123 Added RCD date option in Abstract of Teller
                        ReportAbstractTellerBinanRCD();
                    else
                        ReportAbstractTellerBinan();*/
                    // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date, put rem

                    // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date (s)
                    m_bInit = true;
                    if (!m_bORDate)
                    {
                        OracleResultSet pRec = new OracleResultSet();
                        pRec.Query = "select distinct rcd_series from partial_remit where teller = '" + m_sTeller + "' ";
                        pRec.Query += "and to_date(to_char(dt_save,'MM/dd/yyyy'),'MM/dd/yyyy') between ";
                        pRec.Query += "to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and ";
                        pRec.Query += "to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') ";
                        pRec.Query += "order by rcd_series";
                        if (pRec.Execute())
                        {
                            int i = 0;
                            while (pRec.Read())
                            {
                                i++;
                                m_sRCDNo = pRec.GetString(0);

                                if (i > 1)
                                    axVSPrinter1.NewPage();

                                if (m_bInit)
                                    CreateHeaderPage1Binan();

                                m_bInit = false;

                                ReportAbstractTellerBinanRCD();

                            }
                        }
                        pRec.Close();
                    }
                    else
                    {
                        CreateHeaderPage1Binan();
                        ReportAbstractTellerBinan();
                    }
                    // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date (e)

                }// RMC 20161230 modified abstract teller report for Binan (e)
                else
                {
                    CreateHeaderLubao();
                    ReportAbstract();
                }
            }
            else if (m_sReportSwitch == "OnReportWideSummaryOfCollection")
                OnReportWideSummaryOfCollectionLubao();

            /* REM MCR 20150128*/
            else if (m_sReportSwitch == "OnReportWideSummaryOfCollectionBarangay")
                OnReportWideSummaryOfCollectionBarangay();
            else if (m_sReportSwitch == "OnReportSimpSummaryOfCollectionBarangay")
                OnReportSimpSummaryOfCollectionBarangay();
            else if (m_sReportSwitch == "OnReportWideSummaryOfCollectionDistrict")
                OnReportWideSummaryOfCollectionDistrict();
            else if (m_sReportSwitch == "OnReportSimpSummaryOfCollectionDistrict")
                OnReportSimpSummaryOfCollectionDistrict();
            else if (m_sReportSwitch == "OnReportWideSummaryOfCollectionMainBusiness")
                OnReportWideSummaryOfCollectionMainBusiness();
            else if (m_sReportSwitch == "OnReportSimpSummaryOfCollectionMainBusiness")
                OnReportSimpSummaryOfCollectionMainBusiness();
            else if (m_sReportSwitch == "OnReportStanWideSummaryOfCollectionMainBusiness")
                OnReportStanWideSummaryOfCollectionMainBusiness();
            else if (m_sReportSwitch == "OnReportStanSimpSummaryOfCollectionMainBusiness")
                OnReportStanSimpSummaryOfCollectionMainBusiness();
            else if (m_sReportSwitch == "OnReportWideSummaryOfCollectionOrgnKind")
                OnReportWideSummaryOfCollectionOrgnKind();
            else if (m_sReportSwitch == "OnReportStanWideSummaryOfCollectionOrgnKind")
                OnReportStanWideSummaryOfCollectionOrgnKind();
            else if (m_sReportSwitch == "OnReportWideSummaryOfCollectionBusinessStatus")
                OnReportWideSummaryOfCollectionBusinessStatus();
            else if (m_sReportSwitch == "OnReportStanWideSummaryOfCollectionBusinessStatus")
                OnReportStanWideSummaryOfCollectionBusinessStatus();
            else if (m_sReportSwitch == "OnReportWideSummaryOfCollectionLineOfBusiness")
                OnReportWideSummaryOfCollectionLineOfBusiness();
            else if (m_sReportSwitch == "OnReportTotalCollections")
                OnReportTotalCollections();
            /**/

            // RMC 20150504 QA corrections (s)
            else if (m_sReportSwitch == "Liquidating Officer RCD")
            {
                OnReportLORCD();
            }
            // RMC 20150504 QA corrections (e)
            else if (m_sReportSwitch == "Negative List")
            {
                CreateHeader();
                NegativeList();
            }
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;

            // RMC 20141020 printing of OR (s)
            if (m_sReportSwitch == "ReprintOR" || m_sReportSwitch == "PrintOR")   
            {
                // auto-printing and disabled viewing     
                //this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paPrintPage; // AST 20160125                
                //if (this.axVSPrinter1.PageCount > 1)
                //    this.axVSPrinter1.PrintDoc(1, 1, this.axVSPrinter1.PageCount); // AST 20160125 added for printing multi OR.
                //else
                //    this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paPrintPage;
                //this.Close(); 
            }
            // RMC 20141020 printing of OR (e)
        }

        private void ACEAbstractCollection() //MCR 20150130
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.CreateNewConnectionARCS();
            double dAmount = 0;
            double dTotAmount = 0;

            pSet.Query = "select * from (";
            pSet.Query += "select MF.fees_desc,MF.fees_code,MF.gen_code, 0 ";
            pSet.Query += "from major_fees MF left join payments_info PI on MF.fees_code = (substr(rtrim(PI.fees_code),1,2)) ";

            if (m_sFund == "General Fund")
                pSet.Query += "where MF.fees_desc not like '% SEF' and MF.form_type not like '%TF%' ";
            else if (m_sFund == "Special Education Fund")
                pSet.Query += "where (MF.fees_desc like '% SEF' or MF.fees_desc = 'REAL PROPERTY TAX') and MF.form_type not like '%TF%' ";
            else //TRUST
                pSet.Query += "where MF.form_type like '%TF%' ";

            pSet.Query += "group by MF.fees_desc,MF.fees_code,MF.gen_code union all ";
            pSet.Query += "select SC.fees_desc,SC.fees_code,null, nvl(sum(nvl(PII.fees_amt_due,0)),0) as fees_amt_due ";
            pSet.Query += "from subcategories SC ";
            pSet.Query += "left join payments_info PII on SC.fees_code = PII.fees_code ";
            pSet.Query += "left join major_fees MF on MF.fees_code = (substr(rtrim(SC.fees_code),1,2)) ";

            if (m_sFund == "General Fund")
                pSet.Query += "where MF.fees_desc not like '% SEF' and SC.fees_desc not like '%SEF %' and MF.form_type not like '%TF%' ";
            else if (m_sFund == "Special Education Fund")
                pSet.Query += "where MF.fees_desc like '% SEF' or SC.fees_desc like '%SEF %' ";
            else //TRUST
                pSet.Query += "where MF.form_type like '%TF%' ";

            pSet.Query += "and to_date(PII.or_date) between to_date('" + m_dtFrom.ToString("MM/dd/yyyy") + "', 'MM/dd/yyyy') and to_date('" + m_dtTo.ToString("MM/dd/yyyy") + "', 'MM/dd/yyyy') ";
            pSet.Query += "group by SC.fees_desc,SC.fees_code,null";
            pSet.Query += ") a order by a.fees_code, a.fees_desc, a.gen_code";

            bool bEntered = false;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    if (pSet.GetString(2).Trim() != String.Empty)
                    {
                        if (bEntered == true)
                        {
                            this.axVSPrinter1.FontBold = true;
                            this.axVSPrinter1.Table = "<8000|>2500; |" + dAmount.ToString("#,##0.00");
                            this.axVSPrinter1.FontBold = false;
                        }

                        this.axVSPrinter1.FontBold = true;
                        this.axVSPrinter1.Table = "<8000;" + pSet.GetString(0);
                        this.axVSPrinter1.FontBold = false;
                        dAmount = 0;
                        bEntered = true;
                    }
                    else
                        this.axVSPrinter1.Table = "<200|<7800|>2500; |" + pSet.GetString(0) + "|" + pSet.GetDouble(3).ToString("#,##0.00");

                    dAmount += pSet.GetDouble(3);
                    dTotAmount += pSet.GetDouble(3);
                }
            }
            pSet.Close();

            if (bEntered == true)
            {
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = "<8000|>2500; |" + dAmount.ToString("#,##0.00");
                this.axVSPrinter1.FontBold = false;
            }

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<8000|>2500;Sub Total |" + dTotAmount.ToString("#,##0.00");
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<8000;BUSINESS TAX";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = "";

            double dBPLSAmountTmp = 0;
            double dBPLSAmount = 0;
            if (m_sFund == "General Fund")
            {
                OracleResultSet pSetBPLS = new OracleResultSet();
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        pSetBPLS.Query = "select BT.bns_code, BT.bns_desc, nvl(sum(nvl(ot.fees_amtdue,0)),0) as fees_amtdue from bns_table BT ";
                        pSetBPLS.Query += "left join or_table OT on BT.bns_code = substr(OT.bns_code_main,1,2) ";
                        pSetBPLS.Query += "where length(BT.bns_code) = 2 and OT.fees_code = 'B' ";
                        pSetBPLS.Query += "and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' and or_no in ";
                        pSetBPLS.Query += "(select or_no from pay_hist where or_date between to_date('" + m_dtFrom.ToString("MM/dd/yyyy") + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToString("MM/dd/yyyy") + "','MM/dd/yyyy')) ";
                        pSetBPLS.Query += "group by BT.bns_code,BT.bns_desc order by BT.bns_code";
                    }
                    else
                    {
                        pSetBPLS.Query = "select TFT.fees_code,TFT.fees_desc, nvl(sum(nvl(ot.fees_amtdue,0)),0) as fees_amtdue from tax_and_fees_table TFT ";
                        pSetBPLS.Query += "left join or_table OT on TFT.fees_code = OT.fees_code where OT.fees_code <> 'B' ";
                        pSetBPLS.Query += "and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' and or_no in ";
                        pSetBPLS.Query += "(select or_no from pay_hist where or_date between to_date('" + m_dtFrom.ToString("MM/dd/yyyy") + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToString("MM/dd/yyyy") + "','MM/dd/yyyy')) ";
                        pSetBPLS.Query += "group by TFT.fees_code,TFT.fees_desc order by TFT.fees_code asc";

                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.FontBold = true;
                        this.axVSPrinter1.Table = "<8000;REGULATORY FEES";
                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.Paragraph = "";
                    }

                    if (pSetBPLS.Execute())
                    {
                        while (pSetBPLS.Read())
                        {
                            this.axVSPrinter1.Table = "<8000|>2500;" + pSetBPLS.GetString(1) + "|" + pSetBPLS.GetDouble(2).ToString("#,##0.00");
                            dBPLSAmountTmp += pSetBPLS.GetDouble(2);
                            dBPLSAmount += pSetBPLS.GetDouble(2);
                        }
                    }
                    pSetBPLS.Close();

                    if (dBPLSAmountTmp != 0)
                    {
                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.FontBold = true;
                        this.axVSPrinter1.Table = "<8000|>2500;Sub Total |" + dBPLSAmountTmp.ToString("#,##0.00");
                        this.axVSPrinter1.FontBold = false;
                    }
                    dBPLSAmountTmp = 0;
                }

            }

            double dGrandTotal = 0;
            dGrandTotal = dBPLSAmount + dTotAmount;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<8000|>2500;Grand Total |" + dGrandTotal.ToString("#,##0.00"); 
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<1500|<7000;Printed By:|" + AppSettingsManager.SystemUser.UserName;
            this.axVSPrinter1.Table = "<1500|<7000;Printed Date:|" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
        }

        private void ReprintORMalolos() //JARS 20170824 FOR MALOLOS, MASYADO MAGULO ReprintORLubao()
        {
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            OracleResultSet result = new OracleResultSet();
            string strTmpQuery = string.Empty;
            bool bMultiple = false;
            double dDebitExtra = 0;
            DateTime dtORDate = new DateTime(); 
            DateTime.TryParse(m_sORDate, out dtORDate); 
            OracleResultSet pRec = new OracleResultSet();
            int iOrCnt = 0;
            string sTmpOrNo = string.Empty;
            if (m_sReportSwitch == "PrintOR") 
            {
                pRec.Query = "select distinct or_no from pay_hist where bin = '" + m_sBIN + "' and teller = '" + m_sTeller + "' ";
                pRec.Query += "and or_date = to_date('" + m_sORDate + "','MM/dd/yyyy') and ";
                pRec.Query += "(memo like '%" + m_sORno + "%' or or_no = '" + m_sORno + "') order by or_no";
            }
            else
            {
                pRec.Query = string.Format("select distinct or_no from pay_hist where bin = '{0}' and teller = '{1}' ", m_sBIN, m_sTeller);
                pRec.Query += string.Format("and or_date = to_date('{0}', 'MM/dd/yyyy') and or_no = '{1}'", dtORDate.ToShortDateString(), m_sORno);
            }
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iOrCnt++;
                    if (iOrCnt >= 2)
                    {
                        bMultiple = true;
                    }
                    if (sTmpOrNo == "")
                        sTmpOrNo = pRec.GetString("or_no");

                    if (iOrCnt > 1 && sTmpOrNo != pRec.GetString("or_no"))
                    {
                        this.axVSPrinter1.NewPage();
                        sTmpOrNo = pRec.GetString("or_no");
                    }

                    if (m_sReportSwitch == "PrintOR")
                        m_sORno = pRec.GetString("or_no");

                    this.axVSPrinter1.MarginLeft = 500;
                    if (m_bReprintOR == true)
                    {
                        this.axVSPrinter1.FontSize = 10;
                        this.axVSPrinter1.FontBold = true;
                        if (AppSettingsManager.GetConfigValue("10") == "019")   // RMC 20150504 QA corrections
                            this.axVSPrinter1.Table = "^5000;" + AppSettingsManager.GetConfigValue("02");
                        else
                            this.axVSPrinter1.Table = "^5000;";
                        this.axVSPrinter1.Table = "<3400|<2000;|RE-PRINTED OR";
                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.Table = "<100; ";
                        this.axVSPrinter1.Table = "<100; ";
                    }
                    else
                    {
                        this.axVSPrinter1.FontSize = 10;
                        if (AppSettingsManager.GetConfigValue("10") == "019")
                            this.axVSPrinter1.Table = "^5000;" + AppSettingsManager.GetConfigValue("02");
                        else
                            this.axVSPrinter1.Table = "^5000;";
                        this.axVSPrinter1.Table = "<100; ";
                        this.axVSPrinter1.Table = "<100; ";
                        this.axVSPrinter1.Table = "<100; ";
                    }

                    this.axVSPrinter1.Table = "<100; ";
                    this.axVSPrinter1.SpaceBefore = 100;
                    object lng1;

                    this.axVSPrinter1.CurrentY = 4000;
                    this.axVSPrinter1.MarginLeft = 3500;

                    this.axVSPrinter1.Table = "<2500;" + m_sORno;
                    this.axVSPrinter1.Table = "<2500;" + m_sORDate;
                    this.axVSPrinter1.MarginLeft = 4500;
                    string sTmp = "";
                    string sTmpTaxYear = "";
                    string sTmpBnsStat = "";
                    string sTmpQtr = "";
                    AppSettingsManager.GetLastPaymentInfo(m_sBIN, m_sORno, out sTmpTaxYear, out sTmpQtr, out sTmpBnsStat);
                    if (sTmpBnsStat == "NEW")
                        this.axVSPrinter1.Table = "<2500;Capital: " + AppSettingsManager.GetCapitalGross(m_sBIN, m_sBCode, m_sTaxYear, sTmpBnsStat);
                    else
                        this.axVSPrinter1.Table = "<2500;Gross: " + AppSettingsManager.GetCapitalGross(m_sBIN, m_sBCode, m_sTaxYear, sTmpBnsStat);
                    this.axVSPrinter1.Table = "<2500;" + sTmpTaxYear + " " + sTmpQtr;
                    #region comments
                    //if (AppSettingsManager.GetConfigValue("10") == "019")
                    //{
                    //    this.axVSPrinter1.Table = "<3700;" + m_sORDate;
                    //    this.axVSPrinter1.SpaceBefore = 300;
                    //    lng1 = this.axVSPrinter1.CurrentY;

                    //    this.axVSPrinter1.Table = "<3700;" + m_sBnsOwner + "\n" + m_sBrgy;
                    //}
                    //else if (AppSettingsManager.GetConfigValue("10") == "243")
                    //{
                    //    this.axVSPrinter1.CurrentY = 2280;

                    //    this.axVSPrinter1.Table = "<3500|<3700;|" + m_sORno;
                    //    lng1 = this.axVSPrinter1.CurrentY;
                    //    this.axVSPrinter1.FontSize = 8;

                    //    this.axVSPrinter1.CurrentY = 2780;
                    //    lng1 = this.axVSPrinter1.CurrentY;
                    //    // RMC 20170109 adjust location of or no and font of total due in OR (e)
                    //    //this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1) - 200; // RMC 20170109 adjust location of or no and font of total due in OR
                    //    this.axVSPrinter1.Table = "<500|<2500|<3700;|" + m_sORDate;

                    //    lng1 = this.axVSPrinter1.CurrentY;
                    //    this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1) + 200;
                    //    this.axVSPrinter1.FontSize = 10;

                    //    this.axVSPrinter1.Table = "<700|<3700;|" + AppSettingsManager.GetConfigValue("02");
                    //    this.axVSPrinter1.FontSize = 8;

                    //    this.axVSPrinter1.Table = "<700|<3700;|" + m_sBnsOwner + "\n" + m_sBrgy + "/ BIN: " + m_sBIN;
                    //}// RMC 20161217 adjusted or date and or no in OR (e)
                    //else
                    //{
                    //    this.axVSPrinter1.CurrentY = 2080;
                    //    lng1 = Convert.ToInt32(this.axVSPrinter1.CurrentY); //JARS 20170824
                    //    ////this.axVSPrinter1.Table = "<2500|<3700;|" + m_sORDate;
                    //    //this.axVSPrinter1.Table = "<3500|<3700;|" + m_sORDate;
                    //    //this.axVSPrinter1.FontSize = 8;
                    //    ////this.axVSPrinter1.Table = "<2500|<3700;" + m_sORno;
                    //    //this.axVSPrinter1.Table = "<500|<2500|<3700;||" + m_sORno;
                    //    //this.axVSPrinter1.Table = "<2500|<3700;|" + m_sORDate;
                    //    this.axVSPrinter1.Table = "<3500|<3700;|" + m_sORno;
                    //    this.axVSPrinter1.FontSize = 8;
                    //    this.axVSPrinter1.Table = "<500|<2500|<3700;||" + m_sORDate;
                    //    string sTmp = "";
                    //    string sTmpTaxYear = "";
                    //    string sTmpBnsStat = "";
                    //    string sTmpQtr = "";
                    //    AppSettingsManager.GetLastPaymentInfo(m_sBIN, m_sORno, out sTmpTaxYear, out sTmpQtr, out sTmpBnsStat);
                    //    if (sTmpBnsStat == "NEW")
                    //        sTmp = "Capital: ";
                    //    else
                    //        sTmp = "Gross: ";


                    //    //this.axVSPrinter1.SpaceBefore = 200;
                    //    this.axVSPrinter1.CurrentY = lng1;
                    //    this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1) + 1000;
                    //    this.axVSPrinter1.FontSize = 10;
                    //    //this.axVSPrinter1.Table = "<3700;" + AppSettingsManager.GetConfigValue("02");
                    //    this.axVSPrinter1.Table = "<700|<3700;|" + AppSettingsManager.GetConfigValue("02");
                    //    this.axVSPrinter1.FontSize = 8;
                    //    //this.axVSPrinter1.Table = "<3700;" + m_sBnsOwner + "\n" + m_sBrgy + "/ BIN: " + m_sBIN;
                    //    this.axVSPrinter1.Table = "<700|<3700;|" + m_sBnsOwner + "\n" + m_sBrgy + "/ BIN: " + m_sBIN;
                    //}
                    #endregion
                    this.axVSPrinter1.MarginLeft = 500;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";


                    object lngY;
                    lngY = axVSPrinter1.CurrentY;
                    axVSPrinter1.CurrentY = Convert.ToInt32(lngY) + 600;   // RMC 20150611
                    double dTotDue = 0;
                    double dTotSurchPen = 0;
                    double dTotTotalDue = 0;
                    string sContent = string.Empty;
                    string sDue = string.Empty;
                    string sSurch = string.Empty;
                    string sTotDue = string.Empty;

                    this.axVSPrinter1.FontSize = 9;
                    #region comments
                    /*for (int i = 0; i < m_dgView.Rows.Count; i++)
                    {
                        string sBnsCode = string.Empty;
                        string sParticulars = string.Empty;
                        string sTaxYear = string.Empty;
                        string sCapGross = string.Empty;
                        string sPen = string.Empty;
                        string sQtr = string.Empty;
                        string sMode = string.Empty;
                        double dTmp = 0;
                        double dSurchPen = 0;

                        sTaxYear = m_dgView[0, i].Value.ToString();
                        sBnsCode = m_dgView[10, i].Value.ToString();
                        sParticulars = m_dgView[3, i].Value.ToString();
                        sTotDue = m_dgView[7, i].Value.ToString();

                        if (sTaxYear == "")
                        {
                        }

                        double.TryParse(sTotDue, out dTmp);
                        sTotDue = string.Format("{0:#,###.00}", dTmp);
                        dTotTotalDue += dTmp;

                        this.axVSPrinter1.Table = "<2700|>1200;" + sParticulars + "|" + sTotDue;
                        this.axVSPrinter1.SpaceBefore = 50;
                    }*/
                    #endregion
                    string sParticulars = string.Empty;
                    double dTmp = 0;
                    bool bMultipleOR = false;

                    OracleResultSet pSet = new OracleResultSet();

                    int iCnt = 0;
                    int iFeesCnt = 0;
                    int iTYCnt = 0;
                    pSet.Query = @"select count(distinct tax_year) as cnt from or_table where or_no = '" + m_sORno + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                            iCnt = pSet.GetInt(0);
                    pSet.Close();
                    //MCR 20150123 (s)
                    pSet.Query = @"select count(distinct fees_code) as iCnt from or_table where or_no = '" + m_sORno + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                            iFeesCnt = pSet.GetInt(0);
                    pSet.Close();

                    pSet.Query = @"select count(distinct tax_year) as iCnt from or_table where or_no = '" + m_sORno + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                            iTYCnt = pSet.GetInt(0);
                    pSet.Close();
                    //MCR 20150123 (e)

                    /*string sTemp = "";
                    if (iCnt > 1)
                        sTemp = "||' ('||a.tax_year||')'";
                    else
                        sTemp = "";

                        pSet.Query = @"select BT.bns_desc "+sTemp+@" as bns_desc,sum(fees_amtdue),BT.fees_code from 
        (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + @"')
        a , or_table OT left join bns_table BT on bt.fees_code = ot.fees_code and (bt.bns_code = ot.bns_code_main or bt.bns_code = substr(ot.bns_code_main,0,4))
        where OT.or_no = '" + m_sORno + "' and BT.rev_year = '" + AppSettingsManager.GetConfigValue("07") + @"' and BT.bns_Desc not like '%.%'
        group by OT.bns_code_main,OT.fees_amtdue,bt.fees_code,a.tax_year,BT.bns_desc order by bt.fees_code asc";*/

                    //if (iFeesCnt >= 12) //MCR 20150123
                    if (iFeesCnt >= 9)
                        this.axVSPrinter1.FontSize = 7;

                    double dSurch = 0;  // RMC 20150203 separate surch and int in OR printing
                    double dInt = 0;    // RMC 20150203 separate surch and int in OR printing
                    this.axVSPrinter1.Table = "<100; ";
                    this.axVSPrinter1.Table = "<100; ";
                    this.axVSPrinter1.Table = "<100; ";
                    this.axVSPrinter1.Table = "<100; ";
                    for (int i = 1; i <= 2; i++)
                    {
                        if (i == 1)
                        {
                            //pSet.Query = "select fees_code, SUM(fees_amtdue), bns_code_main from or_table where or_no = '" + m_sORno + "' and fees_code = 'B' group by fees_code, bns_code_main order by bns_code_main";
                            //pSet.Query = "select fees_code, SUM(fees_amtdue), bns_code_main, a.tax_year from (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + "') a, or_table where or_no = '" + m_sORno + "' and fees_code = 'B' group by fees_code, bns_code_main, a.tax_year order by bns_code_main"; //MCR 20150123
                            pSet.Query = "select fees_code, SUM(fees_due), bns_code_main, a.tax_year, sum(fees_surch), sum(fees_pen) from (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + "') a, or_table where or_no = '" + m_sORno + "' and fees_code = 'B' group by fees_code, bns_code_main, a.tax_year order by bns_code_main";   // RMC 20150203 separate surch and int in OR printing
                        }
                        else
                            //pSet.Query = "select fees_code, sum(fees_amtdue) from or_table where or_no = '" + m_sORno + "' and fees_code <> 'B' group by fees_code";
                            //pSet.Query = @"select fees_code, sum(fees_amtdue),a.tax_year from (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + "') a, or_table where or_no = '" + m_sORno + "' and fees_code <> 'B' group by fees_code,a.tax_year"; //MCR 20150123
                            //pSet.Query = @"select fees_code, sum(fees_due),a.tax_year, sum(fees_surch), sum(fees_pen) from (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + "') a, or_table where or_no = '" + m_sORno + "' and fees_code <> 'B' group by fees_code,a.tax_year"; // RMC 20150203 separate surch and int in OR printing
                            pSet.Query = @"select fees_code, sum(fees_due),a.tax_year, sum(fees_surch), sum(fees_pen) from (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + "') a, or_table where or_no = '" + m_sORno + "' and fees_code <> 'B' group by fees_code,a.tax_year order by fees_code"; // RMC 20161218 added order by fees code in OR printing
                        {
                            if (pSet.Execute())
                            {
                                while (pSet.Read())
                                {
                                    sTotDue = pSet.GetDouble(1).ToString();
                                    if (pSet.GetString(0) == "B")
                                    {
                                        if (iTYCnt <= 1) //MCR 20150123
                                            sParticulars = "TAX ON " + AppSettingsManager.GetBnsDesc(pSet.GetString(2));
                                        else
                                            sParticulars = "TAX ON " + AppSettingsManager.GetBnsDesc(pSet.GetString(2)) + " (" + pSet.GetString(3) + ")";
                                        // RMC 20150203 separate surch and int in OR printing (s)
                                        dSurch += pSet.GetDouble(4);
                                        dInt += pSet.GetDouble(5);
                                        // RMC 20150203 separate surch and int in OR printing (e)
                                    }
                                    else
                                    {
                                        if (iTYCnt <= 1) //MCR 20150123
                                            sParticulars = AppSettingsManager.GetFeesDesc(pSet.GetString(0));
                                        else
                                            sParticulars = AppSettingsManager.GetFeesDesc(pSet.GetString(0)) + " (" + pSet.GetString(2) + ")";
                                        // RMC 20150203 separate surch and int in OR printing (s)
                                        dSurch += pSet.GetDouble(3);
                                        dInt += pSet.GetDouble(4);
                                        // RMC 20150203 separate surch and int in OR printing (e)
                                    }


                                    double.TryParse(sTotDue, out dTmp);
                                    sTotDue = string.Format("{0:#,###.00}", dTmp);
                                    dTotTotalDue += dTmp;
                                    this.axVSPrinter1.Table = "<4000|>1100;" + sParticulars + "|" + sTotDue;
                                    this.axVSPrinter1.SpaceBefore = 50;
                                }
                            }
                        }
                        pSet.Close();
                        //i = i + 1;


                    }


                    // RMC 20150203 separate surch and int in OR printing (s)
                    //if (dSurch != 0 && dInt != 0) // RMC 20170201 corrected display of penalty/sur in OR,put rem
                    if (dSurch != 0)    // RMC 20170201 corrected display of penalty/sur in OR
                    {
                        sParticulars = "SURCHARGE";
                        sTotDue = string.Format("{0:#,###.00}", dSurch);
                        this.axVSPrinter1.Table = "<4000|>1100;" + sParticulars + "|" + sTotDue;
                        this.axVSPrinter1.SpaceBefore = 50;
                    }

                    if (dInt != 0)   // RMC 20170201 corrected display of penalty/sur in OR
                    {
                        sParticulars = "PENALTY";
                        sTotDue = string.Format("{0:#,###.00}", dInt);
                        this.axVSPrinter1.Table = "<4000|>1100;" + sParticulars + "|" + sTotDue;
                        this.axVSPrinter1.SpaceBefore = 50;

                        dTotTotalDue += dSurch + dInt;
                    }
                    // RMC 20150203 separate surch and int in OR printing (s)

                    double dDebit = 0;
                    dDebit = Convert.ToDouble(GetDebitMemo(m_sORno));

                    // RMC 20160105 corrected printing of OR if multiple transaction (s)
                    if (CheckMultiCheckPay(m_sORno))
                    {
                        //dTotTotalDue = dTotTotalDue + dDebit;
                        dDebit = 0;
                    }
                    // RMC 20160105 corrected printing of OR if multiple transaction (e)

                    if (dDebit > 0)
                    {
                        string sTmp1 = dDebit.ToString("#,###.00");
                        this.axVSPrinter1.Table = ">4000|>1100;Debit Memo |(" + sTmp1 + ")";
                        dTotTotalDue = dTotTotalDue - dDebit;
                    }
                    else
                    {
                        // RMC 20150506 QA corrections (s)
                        dDebit = Convert.ToDouble(GetCreditMemo(m_sORno));
                        string sTmp1 = dDebit.ToString("#,###.00");
                        if (dDebit > 0)  // RMC 20150611 shoot OR
                            this.axVSPrinter1.Table = ">4000|>1100;Credit Memo |" + sTmp1 + "";
                        dTotTotalDue = dTotTotalDue + dDebit;
                        // RMC 20150506 QA corrections (e)
                    }

                    this.axVSPrinter1.FontSize = 9;
                    //axVSPrinter1.CurrentY = 7400; //5350 // AST 20160126 rem
                    // RMC 20170110 adjust printing of Total so that it will not overwrite fees (s)
                    lng1 = this.axVSPrinter1.CurrentY;
                    if (Convert.ToInt64(lng1) > 7550)
                        axVSPrinter1.CurrentY = lng1;// RMC 20170110 adjust printing of Total so that it will not overwrite fees (e)
                    else
                        axVSPrinter1.CurrentY = 7550; // AST 20160126

                    string a = NumberWording.AmountInWords(dTotTotalDue);
                    //this.axVSPrinter1.FontSize = 11; // RMC 20170109 adjust location of or no and font of total due in OR
                    this.axVSPrinter1.FontSize = (float)11.5;  // RMC 20170119 adjust font size of total and amount in words in OR
                    //if(dTotTotalDue < 0 && iOrCnt <= 1) //JARS 20170731 FOR DEBIT/CREDIT MEMO FIX
                    //{
                    //    dDebitExtra = dTotTotalDue;
                    //    dTotTotalDue = 0.00;
                    //}
                    //if (iOrCnt >= 2 && dDebitExtra != 0)
                    //{
                    //    dTotTotalDue = dTotTotalDue + dDebitExtra;
                    //}
                    this.axVSPrinter1.CurrentY = Convert.ToInt32(axVSPrinter1.CurrentY) + 400;
                    this.axVSPrinter1.Table = "<3000|>2100;|" + dTotTotalDue.ToString("#,###.00");

                    this.axVSPrinter1.Paragraph = "";
                    //this.axVSPrinter1.SpaceBefore = 150; // AST 20160126 rem
                    this.axVSPrinter1.SpaceBefore = 100; // AST 20160126 rem
                    //this.axVSPrinter1.Table = "<4500;" + NumberWording.AmountInWords(dTotTotalDue);

                    //this.axVSPrinter1.FontSize = 9; // RMC 20170109 adjust location of or no and font of total due in OR
                    this.axVSPrinter1.FontSize = (float)9.5; // RMC 20170119 adjust font size of total and amount in words in OR
                    axVSPrinter1.CurrentY = 8300 + 400;  // RMC 20170110 adjust printing of Total so that it will not overwrite fees

                    this.axVSPrinter1.MarginLeft = 1200; //JARS 20170824
                    this.axVSPrinter1.Table = "<1000|<3500;|" + NumberWording.AmountInWords(dTotTotalDue);
                    // this.axVSPrinter1.FontSize = 10;  // RMC 20170110 adjust printing of Total so that it will not overwrite fees, put rem

                    //this.axVSPrinter1.SpaceBefore = 200;
                    this.axVSPrinter1.SpaceBefore = 400;
                    this.axVSPrinter1.MarginLeft = 600; //JARS 20170824
                    if (m_sPaymentType == "CS")
                    {
                        this.axVSPrinter1.CurrentY = 9100;//6230
                        //this.axVSPrinter1.Table = "<100|<2700;|X";
                        this.axVSPrinter1.Table = "<250|<2700;|X";
                    }
                    else if (m_sPaymentType == "CQ" || m_sPaymentType == "CC")
                    {
                        String sChkNo = "";
                        String sBankNm = "";
                        String sBankDate = "";
                        //result.Query = "select B.bank_nm,chk_no,or_date from chk_tbl C left join bank_table B on B.bank_code = C.bank_code where or_no = '" + m_sORno + "'";
                        result.Query = "select B.bank_nm,chk_no,chk_date from chk_tbl C left join bank_table B on B.bank_code = C.bank_code where or_no = '" + m_sORno + "'";   // RMC 20170120 corrected display of check date in OR not OR date
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                sBankNm = result.GetString(0);
                                sChkNo = result.GetString(1);
                                sBankDate = result.GetDateTime(2).ToString("MM/dd/yyyy");
                            }
                        }
                        result.Close();

                        if (m_sPaymentType == "CC")
                        {
                            this.axVSPrinter1.CurrentY = 9300;//6230
                            //this.axVSPrinter1.Table = "<100|<2700;|X";
                            this.axVSPrinter1.Table = "<250|<2700;|X";
                        }

                        this.axVSPrinter1.CurrentY = 8650;//6430
                        //this.axVSPrinter1.Table = "<100|<1750|<1000|<1130|<1200;|X|" + sBankNm + "|" + sChkNo + "|" + sBankDate;
                        this.axVSPrinter1.Table = "<250|<1750|<1000|<1130|<1200;|X|" + sBankNm + "|" + sChkNo + "|" + sBankDate;
                    }
                    #region comments
                    //if (m_sPaymentType == "CQTC" || m_sPaymentType == "CSTC" || m_sPaymentType == "CCTC")
                    //{
                    //    this.axVSPrinter1.CurrentY = 6800;
                    //    this.axVSPrinter1.FontSize = 8;
                    //    string sTotDBCR = string.Empty;
                    //    sTotDBCR = string.Format("{0:#,###.00}", m_dToBeCredited);
                    //    this.axVSPrinter1.Table = "<800|<3500|>1500|>3000|>1600|<1500; |||Total Due:|" + sTotDue + "|";
                    //    this.axVSPrinter1.Table = "<800|<3500|>1500|>3000|>1600|<1500; |||Applied Credit Memo:|" + sTotDBCR + "|";
                    //    dTotTotalDue += m_dToBeCredited;
                    //    sTotDue = string.Format("{0:#,###.00}", dTotTotalDue);
                    //}
                    //this.axVSPrinter1.CurrentY = 10100;//7300
                    #endregion
                    this.axVSPrinter1.CurrentY = 10000;
                    if (AppSettingsManager.GetConfigValue("10") == "019")   // RMC 20150504 QA corrections
                        this.axVSPrinter1.Table = "<2550|<1700;|" + AppSettingsManager.GetTeller(m_sTeller, 0);
                    else
                        //this.axVSPrinter1.Table = "<2000|<3000;|" + AppSettingsManager.GetTeller(m_sTeller, 0); // RMC 20150504 QA corrections
                        this.axVSPrinter1.Table = "<2550|<3000;" + AppSettingsManager.GetTeller(m_sTeller, 0) + "|" + AppSettingsManager.GetConfigValue("05"); // RMC 20150506 QA corrections
                }   // RMC 20151229 merged multiple OR use from Mati
            }
            pRec.Close();   // RMC 20151229 merged multiple OR use from Mati

        }

        private void ReprintORLUBAO() //JARS 20170824 REVISED FOR MALOLOS USE
        {
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;   // RMC 20141020 printing of OR 
            //JMBC 20141001
            OracleResultSet result = new OracleResultSet();
            string strTmpQuery = string.Empty;
            bool bMultiple = false;
            double dDebitExtra = 0;
            DateTime dtORDate = new DateTime(); // AST 20160126
            DateTime.TryParse(m_sORDate, out dtORDate); // AST 20160126

            // RMC 20151229 merged multiple OR use from Mati (s)
            OracleResultSet pRec = new OracleResultSet();
            int iOrCnt = 0;
            string sTmpOrNo = string.Empty; // RMC 20160503 correction in saving payment using multiple OR
            if (m_sReportSwitch == "PrintOR") // AST 20160126 considered reprint
            {
                pRec.Query = "select distinct or_no from pay_hist where bin = '" + m_sBIN + "' and teller = '" + m_sTeller + "' ";
                pRec.Query += "and or_date = to_date('" + m_sORDate + "','MM/dd/yyyy') and ";
                pRec.Query += "(memo like '%" + m_sORno + "%' or or_no = '" + m_sORno + "') order by or_no";
            }
            else 
            {
                pRec.Query = string.Format("select distinct or_no from pay_hist where bin = '{0}' and teller = '{1}' ", m_sBIN, m_sTeller);
                pRec.Query += string.Format("and or_date = to_date('{0}', 'MM/dd/yyyy') and or_no = '{1}'", dtORDate.ToShortDateString(), m_sORno);
            }
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iOrCnt++;
                    if(iOrCnt >= 2)
                    {
                        bMultiple = true;
                    }
                    // RMC 20160503 correction in saving payment using multiple OR (s)
                    if(sTmpOrNo == "")
                        sTmpOrNo = pRec.GetString("or_no");
                    // RMC 20160503 correction in saving payment using multiple OR (e)

                    //if (iOrCnt > 1)   // RMC 20160503 correction in saving payment using multiple OR, put rem
                    if (iOrCnt > 1 && sTmpOrNo != pRec.GetString("or_no"))  // RMC 20160503 correction in saving payment using multiple OR
                    {
                        this.axVSPrinter1.NewPage();
                        sTmpOrNo = pRec.GetString("or_no"); // RMC 20160503 correction in saving payment using multiple OR
                    }

                    if (m_sReportSwitch == "PrintOR") // AST 20160125 
                        m_sORno = pRec.GetString("or_no");                    

                    // RMC 20151229 merged multiple OR use from Mati (e)

                    //this.axVSPrinter1.MarginLeft = 250;
                    this.axVSPrinter1.MarginLeft = 500;
                    if (m_bReprintOR == true)
                    {
                        this.axVSPrinter1.FontSize = 10;
                        this.axVSPrinter1.FontBold = true;
                        if (AppSettingsManager.GetConfigValue("10") == "019")   // RMC 20150504 QA corrections
                            this.axVSPrinter1.Table = "^5000;" + AppSettingsManager.GetConfigValue("02");
                        else
                            this.axVSPrinter1.Table = "^5000;"; // RMC 20150504 QA corrections
                        this.axVSPrinter1.Table = "<3400|<2000;|RE-PRINTED OR";
                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.Table = "<100; ";
                        this.axVSPrinter1.Table = "<100; ";
                    }
                    else
                    {
                        this.axVSPrinter1.FontSize = 10;
                        if (AppSettingsManager.GetConfigValue("10") == "019")   // RMC 20150504 QA corrections (s)
                            this.axVSPrinter1.Table = "^5000;" + AppSettingsManager.GetConfigValue("02");
                        else
                            this.axVSPrinter1.Table = "^5000;"; // RMC 20150504 QA corrections (s)
                        this.axVSPrinter1.Table = "<100; ";
                        this.axVSPrinter1.Table = "<100; ";
                        this.axVSPrinter1.Table = "<100; ";
                    }

                    this.axVSPrinter1.Table = "<100; ";
                    //this.axVSPrinter1.Table = "<100; ";
                    this.axVSPrinter1.SpaceBefore = 100;
                    object lng1; // RMC 20150504 QA corrections
                    if (AppSettingsManager.GetConfigValue("10") == "019")   // RMC 20150504 QA corrections
                    {
                        this.axVSPrinter1.Table = "<3700;" + m_sORDate;
                        this.axVSPrinter1.SpaceBefore = 300;
                        //this.axVSPrinter1.Table = "<100; ";
                        //this.axVSPrinter1.Table = "<100; ";
                        //object lng1 = this.axVSPrinter1.CurrentY;
                        lng1 = this.axVSPrinter1.CurrentY;  // RMC 20150504 QA corrections

                        this.axVSPrinter1.Table = "<3700;" + m_sBnsOwner + "\n" + m_sBrgy;
                    }// RMC 20150504 QA corrections (s)
                        // RMC 20161217 adjusted or date and or no in OR (s)
                    else if (AppSettingsManager.GetConfigValue("10") == "243")
                    {
                        //this.axVSPrinter1.CurrentY = 2780;
                        this.axVSPrinter1.CurrentY = 2280;  // RMC 20170109 adjust location of or no and font of total due in OR

                        this.axVSPrinter1.Table = "<3500|<3700;|" + m_sORno;
                        lng1 = this.axVSPrinter1.CurrentY;
                        this.axVSPrinter1.FontSize = 8;

                        // RMC 20170109 adjust location of or no and font of total due in OR (s)
                        this.axVSPrinter1.CurrentY = 2780;
                        lng1 = this.axVSPrinter1.CurrentY;
                        // RMC 20170109 adjust location of or no and font of total due in OR (e)
                        //this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1) - 200; // RMC 20170109 adjust location of or no and font of total due in OR
                        this.axVSPrinter1.Table = "<500|<2500|<3700;|" + m_sORDate;
                        
                        lng1 = this.axVSPrinter1.CurrentY;
                        this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1) + 200;
                        this.axVSPrinter1.FontSize = 10;
                        
                        this.axVSPrinter1.Table = "<700|<3700;|" + AppSettingsManager.GetConfigValue("02");
                        this.axVSPrinter1.FontSize = 8;
                        
                        this.axVSPrinter1.Table = "<700|<3700;|" + m_sBnsOwner + "\n" + m_sBrgy + "/ BIN: " + m_sBIN;
                    }// RMC 20161217 adjusted or date and or no in OR (e)
                    else
                    {
                        this.axVSPrinter1.CurrentY = 1880;
                        lng1 = Convert.ToInt32(this.axVSPrinter1.CurrentY); //JARS 20170824
                        #region comments
                        ////this.axVSPrinter1.Table = "<2500|<3700;|" + m_sORDate;
                        //this.axVSPrinter1.Table = "<3500|<3700;|" + m_sORDate;
                        //this.axVSPrinter1.FontSize = 8;
                        ////this.axVSPrinter1.Table = "<2500|<3700;" + m_sORno;
                        //this.axVSPrinter1.Table = "<500|<2500|<3700;||" + m_sORno;
                        //this.axVSPrinter1.Table = "<2500|<3700;|" + m_sORDate;
                        #endregion
                        this.axVSPrinter1.CurrentY = Convert.ToInt64(this.axVSPrinter1.CurrentY) + 1050;
                        this.axVSPrinter1.Table = "<3500|<3700;|" + m_sORno;
                        this.axVSPrinter1.FontSize = 8;
                        this.axVSPrinter1.Table = "<500|<2500|<3700;||" + m_sORDate;
                        this.axVSPrinter1.CurrentY = Convert.ToInt64(this.axVSPrinter1.CurrentY) - 1050;
                        //this.axVSPrinter1.SpaceBefore = 200;
                        this.axVSPrinter1.CurrentY = lng1;
                        this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1) + 1800;
                        lng1 = Convert.ToInt32(this.axVSPrinter1.CurrentY); //JARS 20170824
                        this.axVSPrinter1.FontSize = 10;
                        //this.axVSPrinter1.Table = "<3700;" + AppSettingsManager.GetConfigValue("02");
                        this.axVSPrinter1.Table = "<700|<3700;|" + AppSettingsManager.GetConfigValue("02");
                        this.axVSPrinter1.FontSize = 8;
                        //this.axVSPrinter1.Table = "<3700;" + m_sBnsOwner + "\n" + m_sBrgy + "/ BIN: " + m_sBIN;
                        //this.axVSPrinter1.Table = "<700|<3700;|" + m_sBnsOwner + "\n" + m_sBrgy + "/ BIN: " + m_sBIN;
                        this.axVSPrinter1.Table = "<700|<3700;|" + m_sBnsOwner + "\n" + m_sBrgy + "/ BIN: " + m_sBIN + "\n" + m_sBnsName;   // RMC 20171122 added printing of business name in OR
                    }
                    this.axVSPrinter1.FontSize = 10;
                    // RMC 20150504 QA corrections (e)
                    //Gross
                    //MCR 20150119 (s)
                    this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1);
                    string sTmp = "";
                    string sTmpTaxYear = "";
                    string sTmpBnsStat = "";
                    string sTmpQtr = "";
                    AppSettingsManager.GetLastPaymentInfo(m_sBIN, m_sORno, out sTmpTaxYear, out sTmpQtr, out sTmpBnsStat);
                    if (sTmpBnsStat == "NEW")
                        sTmp = "Capital: ";
                    else
                        sTmp = "Gross: ";
                    
                    //this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1) - 200;

                    // RMC 20150616 hide gross (s)
                    if (AppSettingsManager.GetConfigValue("10") == "011")
                    {
                        sTmp = "";
                        this.axVSPrinter1.Table = "<3700|<1600;|" + sTmp;
                    }// RMC 20150616 hide gross (e)
                    else
                        this.axVSPrinter1.Table = "<3700|<1600; |" + sTmp + AppSettingsManager.GetCapitalGross(m_sBIN, m_sBCode, m_sTaxYear, sTmpBnsStat);
                    lng1 = this.axVSPrinter1.CurrentY;
                    //LastPayment
                    this.axVSPrinter1.Table = "<100; ";
                    this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1) - 100;
                    this.axVSPrinter1.Table = "<3700|<1600; |" + sTmpTaxYear + " " + sTmpQtr;
                    //MCR 20150119 (e)
                    this.axVSPrinter1.FontBold = false; // RMC 20141020 printing of OR
                    //strData1 = "BIN\n" + m_sBIN + "\n |Status\n" + m_sStatus + "|Business Code\n" + m_sBCode + "\n|Area\n" + area + "\n |Bill date\n" + bill_date + "|OR Date\n" + m_sORDate + "|OR No.\n" + m_sORno;
                    //strData2 = "Business Name\n" + m_sBnsName + "\n | Business Address\n " + m_sBnsAddress + "\n | Tax Year\n" + m_sTaxYear;
                    //strData3 = "Owner\n" + m_sBnsOwner + "\n    |Owner's Address\n" + address + "\n       | Term\n" + m_sTerm + "\n     ";  // RMC 20141020 printing of OR
                    this.axVSPrinter1.Paragraph = "";

                    object lngY;
                    lngY = axVSPrinter1.CurrentY;
                    //axVSPrinter1.CurrentY = 4000; //3030
                    //axVSPrinter1.CurrentY = Convert.ToInt32(lngY) + 600;   // RMC 20150611
                    this.axVSPrinter1.FontBold = false;
                    double dTotDue = 0;
                    double dTotSurchPen = 0;
                    double dTotTotalDue = 0;
                    string sContent = string.Empty;
                    string sDue = string.Empty;
                    string sSurch = string.Empty;
                    string sTotDue = string.Empty;

                    this.axVSPrinter1.FontSize = 9;
                    #region comments
                    /*for (int i = 0; i < m_dgView.Rows.Count; i++)
                    {
                        string sBnsCode = string.Empty;
                        string sParticulars = string.Empty;
                        string sTaxYear = string.Empty;
                        string sCapGross = string.Empty;
                        string sPen = string.Empty;
                        string sQtr = string.Empty;
                        string sMode = string.Empty;
                        double dTmp = 0;
                        double dSurchPen = 0;

                        sTaxYear = m_dgView[0, i].Value.ToString();
                        sBnsCode = m_dgView[10, i].Value.ToString();
                        sParticulars = m_dgView[3, i].Value.ToString();
                        sTotDue = m_dgView[7, i].Value.ToString();

                        if (sTaxYear == "")
                        {
                        }

                        double.TryParse(sTotDue, out dTmp);
                        sTotDue = string.Format("{0:#,###.00}", dTmp);
                        dTotTotalDue += dTmp;

                        this.axVSPrinter1.Table = "<2700|>1200;" + sParticulars + "|" + sTotDue;
                        this.axVSPrinter1.SpaceBefore = 50;
                    }*/
                    #endregion
                    string sParticulars = string.Empty;
                    double dTmp = 0;
                    bool bMultipleOR = false;

                    OracleResultSet pSet = new OracleResultSet();

                    int iCnt = 0;
                    int iFeesCnt = 0;
                    int iTYCnt = 0;
                    pSet.Query = @"select count(distinct tax_year) as cnt from or_table where or_no = '" + m_sORno + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                            iCnt = pSet.GetInt(0);
                    pSet.Close();
                    //MCR 20150123 (s)
                    pSet.Query = @"select count(distinct fees_code) as iCnt from or_table where or_no = '" + m_sORno + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                            iFeesCnt = pSet.GetInt(0);
                    pSet.Close();

                    pSet.Query = @"select count(distinct tax_year) as iCnt from or_table where or_no = '" + m_sORno + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                            iTYCnt = pSet.GetInt(0);
                    pSet.Close();
                    //MCR 20150123 (e)

                    /*string sTemp = "";
                    if (iCnt > 1)
                        sTemp = "||' ('||a.tax_year||')'";
                    else
                        sTemp = "";

                        pSet.Query = @"select BT.bns_desc "+sTemp+@" as bns_desc,sum(fees_amtdue),BT.fees_code from 
        (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + @"')
        a , or_table OT left join bns_table BT on bt.fees_code = ot.fees_code and (bt.bns_code = ot.bns_code_main or bt.bns_code = substr(ot.bns_code_main,0,4))
        where OT.or_no = '" + m_sORno + "' and BT.rev_year = '" + AppSettingsManager.GetConfigValue("07") + @"' and BT.bns_Desc not like '%.%'
        group by OT.bns_code_main,OT.fees_amtdue,bt.fees_code,a.tax_year,BT.bns_desc order by bt.fees_code asc";*/

                    //if (iFeesCnt >= 12) //MCR 20150123
                    if (iFeesCnt >= 9)
                        this.axVSPrinter1.FontSize = 7;

                    double dSurch = 0;  // RMC 20150203 separate surch and int in OR printing
                    double dInt = 0;    // RMC 20150203 separate surch and int in OR printing
                    this.axVSPrinter1.Table = "<100; ";
                    this.axVSPrinter1.Table = "<100; ";
                    this.axVSPrinter1.Table = "<100; ";
                    this.axVSPrinter1.Table = "<100; ";
                    this.axVSPrinter1.MarginLeft = 500;
                    this.axVSPrinter1.CurrentY = Convert.ToInt64(this.axVSPrinter1.CurrentY) - 600; //JARS 20170824
                    for (int i = 1; i <= 2; i++)
                    {
                        if (i == 1)
                        {
                            //pSet.Query = "select fees_code, SUM(fees_amtdue), bns_code_main from or_table where or_no = '" + m_sORno + "' and fees_code = 'B' group by fees_code, bns_code_main order by bns_code_main";
                            //pSet.Query = "select fees_code, SUM(fees_amtdue), bns_code_main, a.tax_year from (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + "') a, or_table where or_no = '" + m_sORno + "' and fees_code = 'B' group by fees_code, bns_code_main, a.tax_year order by bns_code_main"; //MCR 20150123
                            pSet.Query = "select fees_code, SUM(fees_due), bns_code_main, a.tax_year, sum(fees_surch), sum(fees_pen) from (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + "') a, or_table where or_no = '" + m_sORno + "' and fees_code = 'B' group by fees_code, bns_code_main, a.tax_year order by bns_code_main";   // RMC 20150203 separate surch and int in OR printing
                        }
                        else
                            //pSet.Query = "select fees_code, sum(fees_amtdue) from or_table where or_no = '" + m_sORno + "' and fees_code <> 'B' group by fees_code";
                            //pSet.Query = @"select fees_code, sum(fees_amtdue),a.tax_year from (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + "') a, or_table where or_no = '" + m_sORno + "' and fees_code <> 'B' group by fees_code,a.tax_year"; //MCR 20150123
                            //pSet.Query = @"select fees_code, sum(fees_due),a.tax_year, sum(fees_surch), sum(fees_pen) from (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + "') a, or_table where or_no = '" + m_sORno + "' and fees_code <> 'B' group by fees_code,a.tax_year"; // RMC 20150203 separate surch and int in OR printing
                            pSet.Query = @"select fees_code, sum(fees_due),a.tax_year, sum(fees_surch), sum(fees_pen) from (select min(tax_year)||'-'||max(tax_year) as tax_year from or_table ORT where 1=1 and or_no = '" + m_sORno + "') a, or_table where or_no = '" + m_sORno + "' and fees_code <> 'B' group by fees_code,a.tax_year order by fees_code"; // RMC 20161218 added order by fees code in OR printing
                        {
                            if (pSet.Execute())
                            {
                                while (pSet.Read())
                                {
                                    sTotDue = pSet.GetDouble(1).ToString();
                                    if (pSet.GetString(0) == "B")
                                    {
                                        if (iTYCnt <= 1) //MCR 20150123
                                            sParticulars = "TAX ON " + AppSettingsManager.GetBnsDesc(pSet.GetString(2));
                                        else
                                            sParticulars = "TAX ON " + AppSettingsManager.GetBnsDesc(pSet.GetString(2)) + " (" + pSet.GetString(3) + ")";
                                        // RMC 20150203 separate surch and int in OR printing (s)
                                        dSurch += pSet.GetDouble(4);
                                        dInt += pSet.GetDouble(5);
                                        // RMC 20150203 separate surch and int in OR printing (e)
                                    }
                                    else
                                    {
                                        if (iTYCnt <= 1) //MCR 20150123
                                            sParticulars = AppSettingsManager.GetFeesDesc(pSet.GetString(0));
                                        else
                                            sParticulars = AppSettingsManager.GetFeesDesc(pSet.GetString(0)) + " (" + pSet.GetString(2) + ")";
                                        // RMC 20150203 separate surch and int in OR printing (s)
                                        dSurch += pSet.GetDouble(3);
                                        dInt += pSet.GetDouble(4);
                                        // RMC 20150203 separate surch and int in OR printing (e)
                                    }


                                    double.TryParse(sTotDue, out dTmp);
                                    sTotDue = string.Format("{0:#,###.00}", dTmp);
                                    dTotTotalDue += dTmp;
                                    this.axVSPrinter1.Table = "<4000|>1000;" + sParticulars + "|" + sTotDue;
                                    this.axVSPrinter1.SpaceBefore = 50;
                                }
                            }
                        }
                        pSet.Close();
                        //i = i + 1;


                    }

                    this.axVSPrinter1.MarginLeft = 500;
                    // RMC 20150203 separate surch and int in OR printing (s)
                    //if (dSurch != 0 && dInt != 0) // RMC 20170201 corrected display of penalty/sur in OR,put rem
                    if (dSurch != 0)    // RMC 20170201 corrected display of penalty/sur in OR
                    {
                        sParticulars = "SURCHARGE";
                        sTotDue = string.Format("{0:#,###.00}", dSurch);
                        this.axVSPrinter1.Table = "<4000|>1100;" + sParticulars + "|" + sTotDue;
                        this.axVSPrinter1.SpaceBefore = 50;

                        dTotTotalDue += dSurch; //AFM 20200708 added
                    }

                    if (dInt != 0)   // RMC 20170201 corrected display of penalty/sur in OR
                    {
                        sParticulars = "PENALTY";
                        sTotDue = string.Format("{0:#,###.00}", dInt);
                        this.axVSPrinter1.Table = "<4000|>1100;" + sParticulars + "|" + sTotDue;
                        this.axVSPrinter1.SpaceBefore = 50;

                        //dTotTotalDue += dSurch + dInt;
                        dTotTotalDue += dInt; //AFM 20200708 separated surcharge and interest since it originally has two conditions for each
                    }
                    // RMC 20150203 separate surch and int in OR printing (s)

                    double dDebit = 0;
                    dDebit = Convert.ToDouble(GetDebitMemo(m_sORno));

                    // RMC 20160105 corrected printing of OR if multiple transaction (s)
                    if (CheckMultiCheckPay(m_sORno))
                    {
                        //dTotTotalDue = dTotTotalDue + dDebit;
                        //dDebit = 0; //TEST
                    }
                    // RMC 20160105 corrected printing of OR if multiple transaction (e)

                    if (dDebit > 0)
                    {
                        string sTmp1 = dDebit.ToString("#,###.00");
                        this.axVSPrinter1.Table = ">4000|>1100;Debit Memo |(" + sTmp1 + ")";
                        dTotTotalDue = dTotTotalDue - dDebit;
                    }
                    else
                    {
                        // RMC 20150506 QA corrections (s)
                        dDebit = Convert.ToDouble(GetCreditMemo(m_sORno));
                        string sTmp1 = dDebit.ToString("#,###.00");
                        if (dDebit > 0)  // RMC 20150611 shoot OR
                            this.axVSPrinter1.Table = ">4000|>1100;Credit Memo |" + sTmp1 + "";
                        dTotTotalDue = dTotTotalDue + dDebit;
                        // RMC 20150506 QA corrections (e)
                    }

                    this.axVSPrinter1.FontSize = 9;
                    //axVSPrinter1.CurrentY = 7400; //5350 // AST 20160126 rem
                    // RMC 20170110 adjust printing of Total so that it will not overwrite fees (s)
                    lng1 = this.axVSPrinter1.CurrentY;
                    if (Convert.ToInt64(lng1) > 7350)
                        axVSPrinter1.CurrentY = lng1;// RMC 20170110 adjust printing of Total so that it will not overwrite fees (e)
                    else
                        axVSPrinter1.CurrentY = 7350; // AST 20160126

                    string a = NumberWording.AmountInWords(dTotTotalDue);
                    //this.axVSPrinter1.FontSize = 11; // RMC 20170109 adjust location of or no and font of total due in OR
                    this.axVSPrinter1.FontSize = (float)11.5;  // RMC 20170119 adjust font size of total and amount in words in OR
                    //if(dTotTotalDue < 0 && iOrCnt <= 1) //JARS 20170731 FOR DEBIT/CREDIT MEMO FIX
                    //{
                    //    dDebitExtra = dTotTotalDue;
                    //    dTotTotalDue = 0.00;
                    //}
                    //if (iOrCnt >= 2 && dDebitExtra != 0)
                    //{
                    //    dTotTotalDue = dTotTotalDue + dDebitExtra;
                    //}
                    this.axVSPrinter1.CurrentY = Convert.ToInt32(axVSPrinter1.CurrentY) + 400;
                    this.axVSPrinter1.Table = "<3000|>2100;|" + dTotTotalDue.ToString("#,###.00");

                    this.axVSPrinter1.Paragraph = "";
                    //this.axVSPrinter1.SpaceBefore = 150; // AST 20160126 rem
                    this.axVSPrinter1.SpaceBefore = 100; // AST 20160126 rem
                    //this.axVSPrinter1.Table = "<4500;" + NumberWording.AmountInWords(dTotTotalDue);

                    //this.axVSPrinter1.FontSize = 9; // RMC 20170109 adjust location of or no and font of total due in OR
                    this.axVSPrinter1.FontSize = (float)9.5; // RMC 20170119 adjust font size of total and amount in words in OR
                    axVSPrinter1.CurrentY = 8300;  // RMC 20170110 adjust printing of Total so that it will not overwrite fees

                    this.axVSPrinter1.MarginLeft = 1200; //JARS 20170824
                    this.axVSPrinter1.Table = "<1000|<3500;|" + NumberWording.AmountInWords(dTotTotalDue);
                    // this.axVSPrinter1.FontSize = 10;  // RMC 20170110 adjust printing of Total so that it will not overwrite fees, put rem
                    
                    //this.axVSPrinter1.SpaceBefore = 200;
                    this.axVSPrinter1.SpaceBefore = 400;
                    this.axVSPrinter1.MarginLeft = 600; //JARS 20170824
                    if (m_sPaymentType == "CS")
                    {
                        this.axVSPrinter1.CurrentY = 8900;//6230
                        //this.axVSPrinter1.Table = "<100|<2700;|X";
                        this.axVSPrinter1.Table = "<250|<2700;|X";
                    }
                    else if (m_sPaymentType == "CQ" || m_sPaymentType == "CC" || m_sPaymentType == "CCTC") // JAA 20190313 add CCTC
                    {
                        String sChkNo = "";
                        String sBankNm = "";
                        String sBankDate = "";
                        //result.Query = "select B.bank_nm,chk_no,or_date from chk_tbl C left join bank_table B on B.bank_code = C.bank_code where or_no = '" + m_sORno + "'";
                        result.Query = "select B.bank_nm,chk_no,chk_date from chk_tbl C left join bank_table B on B.bank_code = C.bank_code where or_no = '" + m_sORno + "'";   // RMC 20170120 corrected display of check date in OR not OR date
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                sBankNm = result.GetString(0);
                                sChkNo = result.GetString(1);
                                sBankDate = result.GetDateTime(2).ToString("MM/dd/yyyy");
                            }
                        }
                        result.Close();

                        if (m_sPaymentType == "CC")
                        {
                            this.axVSPrinter1.CurrentY = 9100;//6230
                            //this.axVSPrinter1.Table = "<100|<2700;|X";
                            this.axVSPrinter1.Table = "<250|<2700;|X";
                        }
                        this.axVSPrinter1.CurrentY = 9050;//6430 //8450
                        //this.axVSPrinter1.Table = "<100|<1750|<1000|<1130|<1200;|X|" + sBankNm + "|" + sChkNo + "|" + sBankDate;
                        this.axVSPrinter1.Table = "<250|<1750|<880|<1250|<1200;|X|" + sBankNm + "|" + sChkNo + "|" + sBankDate;
                    }
                 
                    #region comments
                    //if (m_sPaymentType == "CQTC" || m_sPaymentType == "CSTC" || m_sPaymentType == "CCTC")
                    //{
                    //    this.axVSPrinter1.CurrentY = 6800;
                    //    this.axVSPrinter1.FontSize = 8;
                    //    string sTotDBCR = string.Empty;
                    //    sTotDBCR = string.Format("{0:#,###.00}", m_dToBeCredited);
                    //    this.axVSPrinter1.Table = "<800|<3500|>1500|>3000|>1600|<1500; |||Total Due:|" + sTotDue + "|";
                    //    this.axVSPrinter1.Table = "<800|<3500|>1500|>3000|>1600|<1500; |||Applied Credit Memo:|" + sTotDBCR + "|";
                    //    dTotTotalDue += m_dToBeCredited;
                    //    sTotDue = string.Format("{0:#,###.00}", dTotTotalDue);
                    //}
                    //this.axVSPrinter1.CurrentY = 10100;//7300
                    #endregion
                    this.axVSPrinter1.CurrentY = 10300;
                    if (AppSettingsManager.GetConfigValue("10") == "019")   // RMC 20150504 QA corrections
                        this.axVSPrinter1.Table = "<2550|<1700;|" + AppSettingsManager.GetTeller(m_sTeller, 0);
                    else
                        //this.axVSPrinter1.Table = "<2000|<3000;|" + AppSettingsManager.GetTeller(m_sTeller, 0); // RMC 20150504 QA corrections
                        this.axVSPrinter1.Table = "<2550|<3000;" + AppSettingsManager.GetTeller(m_sTeller, 0) + "|" + AppSettingsManager.GetConfigValue("05"); // RMC 20150506 QA corrections
                }   // RMC 20151229 merged multiple OR use from Mati
            }
            pRec.Close();   // RMC 20151229 merged multiple OR use from Mati

        }//MCR 20141215

        private void ReprintOR()
        {
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;   // RMC 20141020 printing of OR 
            //JMBC 20141001
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from own_names where own_code ='" + m_sOwnCode + "'";
            String address = null;
            if (result.Execute())
            {
                while (result.Read())
                {
                    address = result.GetString("OWN_HOUSE_NO") + " ";
                    if (!(result.GetString("OWN_STREET") == "."))
                    {
                        address += result.GetString("OWN_STREET") + " ";
                    }
                    if (!(result.GetString("OWN_DIST") == "."))
                    {
                        address += result.GetString("OWN_DIST") + " ";
                    }
                    if (!(result.GetString("OWN_ZONE") == "."))
                    {
                        address += result.GetString("OWN_ZONE") + " ";
                    }
                    if (!(result.GetString("OWN_BRGY") == "."))
                    {
                        address += result.GetString("OWN_BRGY") + " ";
                    }
                    if (!(result.GetString("OWN_MUN") == "."))
                    {
                        address += result.GetString("OWN_MUN") + " ";
                    }
                    if (!(result.GetString("OWN_PROV") == "."))
                    {
                        address += result.GetString("OWN_PROV");
                    }
                }
            }
            result.Close();
            string bill_date = ".";
            //string query = "select * from bill_hist where bin= '" + m_sBIN + "' and tax_year='" + m_sTaxYear + "'";
            result.Query = "select * from bill_hist where bin= '" + m_sBIN + "' and tax_year='" + m_sTaxYear + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    bill_date = result.GetString("BILL_DATE");
                }
            }
            result.Close();
            double area = 0;
            result.Query = "select * from other_info where bin='" + m_sBIN + "' and tax_year='" + m_sTaxYear + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    area = result.GetDouble("data");
                }
            }

            /* JMBC 20140926*/
            result.Close();
            result.Query = "select * from bill_gross_info where bin= '" + m_sBIN + "' and tax_year='" + m_sTaxYear + "'";
            double gross = 0;
            double capital = 0;
            if (result.Execute())
            {
                while (result.Read())
                {
                    gross = result.GetDouble("gross");
                    capital = result.GetDouble("capital");
                }
            }
            this.axVSPrinter1.FontSize = 12 ;
            this.axVSPrinter1.FontBold = true;
            if (m_sForm == "Not")// RMC 20141020 printing of OR
                this.axVSPrinter1.Table = ">3900|;B U S I N E S S  L I C E N S E";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 8;
            //this.axVSPrinter1.MarginLeft = 1100;
            this.axVSPrinter1.MarginLeft = 700;  // RMC 20141020 printing of OR 
            //this.axVSPrinter1.FontBold = true;
            
            //this.axVSPrinter1.CurrentY = 3280;
            
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;   // RMC 20141020 printing of OR put rem
            //this.axVSPrinter1.Table = ">1200|>1500|>1300|>700|>1800|>1000|>1000|; | | | | | | |";
            this.axVSPrinter1.Table = ">1200|>1500|>1300|>1000|>1800|>1000|>1000|; | | | | | | |";
            string strData1 = "", strData2="", strData3="";

            this.axVSPrinter1.FontBold = false; // RMC 20141020 printing of OR
            strData1 = "BIN\n" + m_sBIN + "\n |Status\n" + m_sStatus + "|Business Code\n" + m_sBCode + "\n|Area\n" + area + "\n |Bill date\n" + bill_date + "|OR Date\n" + m_sORDate + "|OR No.\n" + m_sORno;
            strData2 = "Business Name\n" + m_sBnsName + "\n | Business Address\n " + m_sBnsAddress + "\n | Tax Year\n" + m_sTaxYear;
            strData3 = "Owner\n" + m_sBnsOwner + "\n    |Owner's Address\n" + address + "\n       | Term\n" + m_sTerm + "\n     ";  // RMC 20141020 printing of OR

            this.axVSPrinter1.BrushColor = Color.White;
            if (m_sForm == "Reprinted")
            {
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.CurrentY = 3180; 
                /*this.axVSPrinter1.PenColor = Color.White;
                this.axVSPrinter1.DrawRectangle(0, 0, 18790, 3750);*/
                // RMC 20141020 printing of OR put rem
                strData1 = " \n" + m_sBIN + "\n | \n" + m_sStatus + "\n | \n" + m_sBCode + "\n | \n" + area + "\n | \n" + bill_date + "\n | \n" + m_sORDate + "\n | \n" + m_sORno;
                strData2 = "| \n" + m_sBnsName + "\n |  \n " + m_sBnsAddress + "\n |  \n" + m_sTaxYear;
                //strData3 = " | " + m_dgView.Rows[m_iSelectedData].Cells[7].Value; // RMC 20141020 printing of OR put rem
                strData3 = "| \n" + m_sBnsOwner + "\n    | \n" + address + "\n       | \n" + m_sTerm + "\n     ";// RMC 20141020 printing of OR
                this.axVSPrinter1.CurrentY = 3180;  // RMC 20141020 printing of OR 
                this.axVSPrinter1.Table = "<2600|800|1700|1000|1800|1000|1000;" + strData1;
                this.axVSPrinter1.CurrentY = 3480;  // RMC 20141020 printing of OR 
                this.axVSPrinter1.Table = "<500|<4600|<4100|>1400;" + strData2;
                this.axVSPrinter1.CurrentY = 3880;  // RMC 20141020 printing of OR 
                this.axVSPrinter1.Table = "<500|<4600|<4100|>1400;" + strData3;
            }
            else
            {
                //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColTopBottom; // RMC 20141020 printing of OR

                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.Table = "<2600|800|1700|1000|1800|1000|1000;" + strData1;
                this.axVSPrinter1.Table = "<4600|<4100|>1400;" + strData2;
                this.axVSPrinter1.Table = "<4600|<4100|>1400;" + strData3;    // RMC 20141020 printing of OR
            }

            //this.axVSPrinter1.Table = "<4100|4100|1400;Owner\n" + m_sBnsOwner + "\n    |Owner's Address\n" + address + "\n       | Term\n" + m_sTerm + "\n     ";// RMC 20141020 printing of OR

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;   // RMC 20141020 printing of OR
           
            if (m_sForm == "Not")
            {
                /*this.axVSPrinter1.BrushColor = Color.Green;
                this.axVSPrinter1.PenColor = Color.Green;
                this.axVSPrinter1.TextColor = Color.White;

                this.axVSPrinter1.DrawRectangle(950, 4650, 10790, 5050);

                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;   */  // RMC 20141020 printing of OR put rem

                //this.axVSPrinter1.Table = "<800|<2000|<1500|<1500|<1500|<1600|;" +
                //     "Code|                Particulars|GrossReceipt or Initial Capital|Amount of Tax and Fees|Surch/Pen|Amount Due| Qtr.";
                //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColumns;  // RMC 20141020 printing of OR
                this.axVSPrinter1.Table = "^800|^3500|^1500|^1500|^1500|^1600|<1500;Code|Particulars|GrossReceipt or Initial Capital|Amount of Tax and Fees|Surch/Pen|Amount Due|Qtr.";
                this.axVSPrinter1.PenColor = Color.Black;
            }
            else
            {
                //this.axVSPrinter1.Table = "<800|<2000|<1500|<1500|<1500|<1600|;";
                this.axVSPrinter1.Table = "^800|^3500|^1500|^1500|^1500|^1600|^1500;";  // RMC 20141020 printing of OR
            }

            this.axVSPrinter1.TextColor = Color.Black;
            this.axVSPrinter1.Paragraph = "";
          
            /* end of my edit JMBC 20140926 */
            /*if (m_sForm == "Not")
            {
                this.axVSPrinter1.Table = "<800|2000|>1500|>1500|>1500|>1600|;\n" +
               m_sBCode + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n\n\n\n \n |\n " +
               m_dgView.Rows[m_iSelectedData].Cells[3].Value + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n\n\n\n \n |\n "
               + gross.ToString() + "/" + capital.ToString() + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n\n\n\n \n |\n "
               + m_dgView.Rows[m_iSelectedData].Cells[4].Value + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n\n\n\n \n |\n"
               + m_dgView.Rows[m_iSelectedData].Cells[5].Value +
               "/" + m_dgView.Rows[m_iSelectedData].Cells[6].Value + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n\n\n\n \n |\n"
               + m_dgView.Rows[m_iSelectedData].Cells[7].Value + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n\n\n\n \n |\n "
               + m_dgView.Rows[m_iSelectedData].Cells[1].Value + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n\n\n\n \n ";
            }
            else
            {
                this.axVSPrinter1.Table = "<800|2000|>1500|>1500|>1500|>1600|;\n \n \n \n " +
                 m_sBCode + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n\n\n\n \n |\n \n \n \n  " +
                 m_dgView.Rows[m_iSelectedData].Cells[3].Value + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n\n\n\n \n |\n \n\n\n "
                 + gross.ToString() + "/" + capital.ToString() + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n\n |\n \n\n\n"
                 + m_dgView.Rows[m_iSelectedData].Cells[4].Value + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n |\n\n\n\n "
                 + m_dgView.Rows[m_iSelectedData].Cells[5].Value +
                 "/" + m_dgView.Rows[m_iSelectedData].Cells[6].Value + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n |\n\n\n\n "
                 + m_dgView.Rows[m_iSelectedData].Cells[7].Value + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n |\n \n\n\n "
                 + m_dgView.Rows[m_iSelectedData].Cells[1].Value + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n\n\n\n \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n \n\n\n\n \n ";
            }
            // JMBC 20140930 */

            // RMC 20141020 printing of OR (s)
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            double dTotDue = 0;
            double dTotSurchPen = 0;
            double dTotTotalDue = 0;
            string sContent = string.Empty;
            string sDue = string.Empty;
            string sSurch = string.Empty;
            string sTotDue = string.Empty;

            for (int i = 0; i < m_dgView.Rows.Count; i++)
            {
                string sBnsCode = string.Empty;
                string sParticulars = string.Empty;
                string sTaxYear = string.Empty;
                string sCapGross = string.Empty;
                string sPen = string.Empty;
                string sQtr =string.Empty;
                string sMode = string.Empty;
                double dTmp = 0;
                double dSurchPen = 0;
                try
                {
                    sBnsCode = m_dgView[10, i].Value.ToString();
                    sParticulars = m_dgView[3, i].Value.ToString();
                    sTaxYear = m_dgView[0, i].Value.ToString();
                    sTaxYear = "'" + sTaxYear.Substring(2, 2);
                    sDue = m_dgView[4, i].Value.ToString();
                    sSurch = m_dgView[5, i].Value.ToString();
                    sPen = m_dgView[6, i].Value.ToString();
                    sTotDue = m_dgView[7, i].Value.ToString();
                    sQtr = m_dgView[1,i].Value.ToString();
                    sMode = m_dgView[2,i].Value.ToString();

                    if (sBnsCode.Substring(0, 1) == "B")
                    {
                        sCapGross = AppSettingsManager.GetCapitalGross(m_sBIN, sBnsCode, sTaxYear, m_sStatus);
                        double.TryParse(sCapGross, out dTmp);
                        sCapGross = string.Format("{0:#,###.00}", dTmp);
                    }
                    else
                        sCapGross = "";

                    double.TryParse(sSurch, out dTmp);
                    dSurchPen = dTmp;
                    double.TryParse(sPen, out dTmp);
                    dSurchPen += dTmp;
                    sSurch = string.Format("{0:#,###.00}", dSurchPen);
                    dTotSurchPen += dSurchPen;

                    double.TryParse(sTotDue, out dTmp);
                    sTotDue = string.Format("{0:#,###.00}", dTmp);
                    dTotTotalDue += dTmp;
                    double.TryParse(sDue, out dTmp);
                    sDue = string.Format("{0:#,###.00}", dTmp);
                    dTotDue += dTmp;
                }
                catch {
                sBnsCode = "";
                    sParticulars = "";
                    sTaxYear = "";
                    sDue = "";
                    sSurch = "";
                    sPen = "";
                    sTotDue = "";
                }

                sContent = sBnsCode + "|" + sParticulars + "|" + sCapGross + "|" + sDue + "|" + sSurch + "|" + sTotDue + "|" + sTaxYear + " " + sQtr + " " + sMode;
                this.axVSPrinter1.Table = "<800|<3500|>1500|>1500|>1500|>1600|<1500;" + sContent;

            }

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            sDue = string.Format("{0:#,###.00}", dTotDue);
            sSurch = string.Format("{0:#,###.00}", dTotSurchPen);
            sTotDue = string.Format("{0:#,###.00}", dTotTotalDue);

            sContent = "||Sub Total|" + sDue + "|" + sSurch + "|" + sTotDue + "|";
            this.axVSPrinter1.Table = "<800|3500|>1500|>1500|>1500|>1600|<1500;" + sContent;
            
            if (m_sPaymentType == "CQTC" || m_sPaymentType == "CSTC" || m_sPaymentType == "CCTC")
            {
                this.axVSPrinter1.CurrentY = 10100;
                this.axVSPrinter1.FontSize = 8;
                string sTotDBCR = string.Empty;
                sTotDBCR = string.Format("{0:#,###.00}", m_dToBeCredited);
                this.axVSPrinter1.Table = "<800|<3500|>1500|>3000|>1600|<1500; |||Total Due:|" + sTotDue + "|";
                this.axVSPrinter1.Table = "<800|<3500|>1500|>3000|>1600|<1500; |||Applied Credit Memo:|" + sTotDBCR + "|";
                dTotTotalDue += m_dToBeCredited;
                sTotDue = string.Format("{0:#,###.00}", dTotTotalDue);
            }
            // RMC 20141020 printing of OR (e)

                //this.axVSPrinter1.CurrentY = 14100;
            this.axVSPrinter1.CurrentY = 13800; // RMC 20141020 printing of OR
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;  // RMC 20141020 printing of OR
            this.axVSPrinter1.Table = ">800|2000|1500|1500|1500|1500|; | | | | | | |";
            //this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.FontSize = 8; // RMC 20141020 printing of OR
            this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;   // RMC 20141020 printing of OR


            //this.axVSPrinter1.Table = ">3000|>5800|;" + strData3;
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = "<800|<3500|>1500|>3000|>1600|<1500; |||Amount Received:|" + sTotDue + "|";    // RMC 20141020 printing of OR
            
            this.axVSPrinter1.FontSize = 7;
            this.axVSPrinter1.FontBold = false;
            //if (m_sForm == "Not") // RMC 20141020 printing of OR
            {
                //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColumns;  // RMC 20141020 printing of OR put rem
                
                /*this.axVSPrinter1.Table = "<3200|2000|1500|3000; Teller| Ref no. | | City Treasurer                   |";
                this.axVSPrinter1.Table = "<3200|2000|1500|3000; | | | |";
                this.axVSPrinter1.Table = "<3200|2000|1500|3000;  " + AppSettingsManager.GetTeller(m_sTeller,0) + "|  " + m_sORno + "| | |";    // RMC 20141020 printing of OR
                 */

                // RMC 20141020 printing of OR (s)
                if (m_sForm == "Not")
                    this.axVSPrinter1.Table = "<4300|<2000|1500|3000; Teller| Ref no. | | City Treasurer                   |";
                this.axVSPrinter1.Table = "<4300|<2000;|";
                this.axVSPrinter1.Table = "<4300|<2000;|";
                this.axVSPrinter1.Table = "<4300|<2000;" + AppSettingsManager.GetTeller(m_sTeller, 0) + "|" + m_sORno;
                // RMC 20141020 printing of OR (e)
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
                this.axVSPrinter1.MarginLeft = 900;
                //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColumns;  // RMC 20141020 printing of OR put rem

                this.axVSPrinter1.Table = ">6730|; | ";
                this.axVSPrinter1.Table = ">6730|; | ";
                this.axVSPrinter1.FontSize = 9;
                this.axVSPrinter1.FontBold = true;
                //this.axVSPrinter1.Table = ">6730|3000;  |    JOSEFINA O. DE JESUS";
                this.axVSPrinter1.Table = ">8000|<3000;  |    " + AppSettingsManager.GetConfigValue("05");   // RMC 20141020 printing of OR
                this.axVSPrinter1.Table = ">6730|; | ";

                //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;  // RMC 20141020 printing of OR put rem
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taRightTop;
                this.axVSPrinter1.MarginRight = 1350;
                this.axVSPrinter1.Table = ">2210|; | ";
            }
            //end of edit JMBC 09302014
        }
        
        // Lubao
        private void OnReportWideSummaryOfCollectionLubao()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sRen, sNew, sRet, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sData17, sData18, sTotalTax, sSurcharge, sTotalCollect;
            double dREN = 0, dNEW = 0, dRET = 0, dRenGross = 0, dNewGross = 0, dRetGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dData17 = 0, dData18 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTREN = 0, dTNEW = 0, dTRET = 0, dTRenGross = 0, dTNewGross = 0, dTRetGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTData17 = 0, dTData18 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;
            double dTData19 = 0, dTData20 = 0, dTData21 = 0, dData19 = 0, dData20 = 0, dData21 = 0;
            String sData19, sData20, sData21;
            double dSubData = 0, dTSubData = 0;
            String sSubData = "";

            for (int i = 1; i <= 2; i++) // RMC 20150126
            {
                // RMC 20150126 (s)
                m_iBatchCnt = i;

                sTempDataGroup = "";

                if (m_iBatchCnt == 2)
                    this.axVSPrinter1.NewPage();
                // RMC 20150126 (e)

                if (m_sReportSwitch.Contains("OnReportWideSummaryOfCollection"))
                {
                    sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16,RS.data17, RS.data18,RS.data19,RS.data20, RS.data21, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21+rs.sur_interest) as TOTALCOLLECTION";
                    sQuery += ", sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10) as SUBTOTALTAXES"; //MCR 20150128
                    sQuery += " FROM REPORT_SUMCOLL RS";
                    sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
                    sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                    sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.data17, RS.data18, RS.data19, RS.data20, RS.data21, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";
                }

                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                        sDataGroup = pSet.GetString("data_group1").Trim();
                        sDataGroup2 = pSet.GetString("data_group2").Trim();

                        sRen = pSet.GetInt("ren_no").ToString();
                        sRenGross = pSet.GetDouble("ren_gross").ToString();
                        sNew = pSet.GetInt("new_no").ToString();
                        sNewGross = pSet.GetDouble("new_cap").ToString();
                        sRet = pSet.GetInt("ret_no").ToString();
                        sRetGross = pSet.GetDouble("ret_gross").ToString();

                        sData1 = pSet.GetDouble("data1").ToString();
                        sData2 = pSet.GetDouble("data2").ToString();
                        sData3 = pSet.GetDouble("data3").ToString();
                        sData4 = pSet.GetDouble("data4").ToString();
                        sData5 = pSet.GetDouble("data5").ToString();
                        sData6 = pSet.GetDouble("data6").ToString();
                        sData7 = pSet.GetDouble("data7").ToString();
                        sData8 = pSet.GetDouble("data8").ToString();
                        sData9 = pSet.GetDouble("data9").ToString();
                        sData10 = pSet.GetDouble("data10").ToString();
                        sData11 = pSet.GetDouble("data11").ToString();
                        sData12 = pSet.GetDouble("data12").ToString();
                        sData13 = pSet.GetDouble("data13").ToString();
                        sData14 = pSet.GetDouble("data14").ToString();
                        sData15 = pSet.GetDouble("data15").ToString();
                        sData16 = pSet.GetDouble("data16").ToString();
                        sData17 = pSet.GetDouble("data17").ToString();
                        sData18 = pSet.GetDouble("data18").ToString();
                        sData19 = pSet.GetDouble("data19").ToString();
                        sData20 = pSet.GetDouble("data20").ToString();
                        sData21 = pSet.GetDouble("data21").ToString();
                        sSubData = pSet.GetDouble("SUBTOTALTAXES").ToString(); //MCR 20150128

                        sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                        sSurcharge = pSet.GetDouble("sur_interest").ToString();
                        sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                        #region Total

                        if (m_iBatchCnt == 1)
                        {
                            dTREN += Convert.ToDouble(sRen);
                            dTNEW += Convert.ToDouble(sNew);
                            dTRET += Convert.ToDouble(sRet);
                            dTRenGross += Convert.ToDouble(sRenGross);
                            dTNewGross += Convert.ToDouble(sNewGross);
                            dTRetGross += Convert.ToDouble(sRetGross);

                            dTData1 += Convert.ToDouble(sData1);
                            dTData2 += Convert.ToDouble(sData2);
                            dTData3 += Convert.ToDouble(sData3);
                            dTData4 += Convert.ToDouble(sData4);
                            dTData5 += Convert.ToDouble(sData5);
                            dTData6 += Convert.ToDouble(sData6);
                            dTData7 += Convert.ToDouble(sData7);
                            dTData8 += Convert.ToDouble(sData8);
                            dTData9 += Convert.ToDouble(sData9);
                            dTData10 += Convert.ToDouble(sData10);
                            dTSubData += Convert.ToDouble(sSubData); //MCR 20150128
                        }
                        else //2
                        {
                            dTData11 += Convert.ToDouble(sData11);
                            dTData12 += Convert.ToDouble(sData12);
                            dTData13 += Convert.ToDouble(sData13);
                            dTData14 += Convert.ToDouble(sData14);
                            dTData15 += Convert.ToDouble(sData15);
                            dTData16 += Convert.ToDouble(sData16);
                            dTData17 += Convert.ToDouble(sData17);
                            dTData18 += Convert.ToDouble(sData18);
                            dTData19 += Convert.ToDouble(sData19);
                            dTData20 += Convert.ToDouble(sData20);
                            dTData21 += Convert.ToDouble(sData21);

                            dTTotalTax += Convert.ToDouble(sTotalTax);
                            dTSurcharge += Convert.ToDouble(sSurcharge);
                            dTTotalCollect += Convert.ToDouble(sTotalCollect);
                        }
                        #endregion

                        if (sTempDataGroup == "")
                        {
                            this.axVSPrinter1.FontBold = true;
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<2000;" + sTempDataGroup;
                            this.axVSPrinter1.FontBold = false;
                        }
                        else if (sDataGroup != sTempDataGroup)
                        {
                            this.axVSPrinter1.FontBold = true;

                            // RMC 20150126 (S)
                            if (m_iBatchCnt == 1)
                            {
                                if (m_sReportSwitch.Contains("OnReportWideSummaryOfCollection"))
                                {
                                    if (m_bShowGrossNCapital == true)
                                    {
                                        this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL|"
                                            + dREN.ToString() + "|"
                                            + dRenGross.ToString("#,##0.00") + "|"
                                            + dNEW.ToString() + "|"
                                            + dNewGross.ToString("#,##0.00") + "|"
                                            + dRET.ToString() + "|"
                                            + dRetGross.ToString("#,##0.00") + "|"
                                              + dData1.ToString("#,##0.00") + "|"
                                              + dData2.ToString("#,##0.00") + "|"
                                              + dData3.ToString("#,##0.00") + "|"
                                              + dData4.ToString("#,##0.00") + "|"
                                              + dData5.ToString("#,##0.00") + "|"
                                              + dData6.ToString("#,##0.00") + "|"
                                              + dData7.ToString("#,##0.00") + "|"
                                              + dData8.ToString("#,##0.00") + "|"
                                              + dData9.ToString("#,##0.00") + "|"
                                              + dData10.ToString("#,##0.00");
                                    }
                                    else
                                    {
                                        this.axVSPrinter1.Table = ">2000|>1800|>1800|>1700|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL|"
                                               + dREN.ToString() + "|"
                                               + dNEW.ToString() + "|"
                                               + dRET.ToString() + "|"
                                                 + dData1.ToString("#,##0.00") + "|"
                                                 + dData2.ToString("#,##0.00") + "|"
                                                 + dData3.ToString("#,##0.00") + "|"
                                                 + dData4.ToString("#,##0.00") + "|"
                                                 + dData5.ToString("#,##0.00") + "|"
                                                 + dData6.ToString("#,##0.00") + "|"
                                                 + dData7.ToString("#,##0.00") + "|"
                                                 + dData8.ToString("#,##0.00") + "|"
                                                 + dData9.ToString("#,##0.00") + "|"
                                                 + dData10.ToString("#,##0.00");
                                    }
                                }
                            }
                            else
                            {
                                this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL|"
                                    + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dData19.ToString("#,##0.00") + "|"
                                      + dData20.ToString("#,##0.00") + "|"
                                      + dData21.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");
                            }

                            // RMC 20150126 (E)
                            dREN = 0;
                            dRenGross = 0;
                            dNEW = 0;
                            dNewGross = 0;
                            dRET = 0;
                            dRetGross = 0;
                            dData1 = 0;
                            dData2 = 0;
                            dData3 = 0;
                            dData4 = 0;
                            dData5 = 0;
                            dData6 = 0;
                            dData7 = 0;
                            dData8 = 0;
                            dData9 = 0;
                            dData10 = 0;
                            dData11 = 0;
                            dData12 = 0;
                            dData13 = 0;
                            dData14 = 0;
                            dData15 = 0;
                            dData16 = 0;
                            dData17 = 0;
                            dData18 = 0;
                            dData19 = 0;
                            dData20 = 0;
                            dData21 = 0;
                            dTotalTax = 0;
                            dSurcharge = 0;
                            dTotalCollect = 0;

                            dSubData = 0; //MCR 20150128

                            this.axVSPrinter1.Paragraph = "";
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<2000;" + sDataGroup;
                            this.axVSPrinter1.FontBold = false;
                        }

                        dREN += Convert.ToDouble(sRen);
                        dNEW += Convert.ToDouble(sNew);
                        dRET += Convert.ToDouble(sRet);
                        dRenGross += Convert.ToDouble(sRenGross);
                        dNewGross += Convert.ToDouble(sNewGross);
                        dRetGross += Convert.ToDouble(sRetGross);

                        dData1 += Convert.ToDouble(sData1);
                        dData2 += Convert.ToDouble(sData2);
                        dData3 += Convert.ToDouble(sData3);
                        dData4 += Convert.ToDouble(sData4);
                        dData5 += Convert.ToDouble(sData5);
                        dData6 += Convert.ToDouble(sData6);
                        dData7 += Convert.ToDouble(sData7);
                        dData8 += Convert.ToDouble(sData8);
                        dData9 += Convert.ToDouble(sData9);
                        dData10 += Convert.ToDouble(sData10);
                        dData11 += Convert.ToDouble(sData11);
                        dData12 += Convert.ToDouble(sData12);
                        dData13 += Convert.ToDouble(sData13);
                        dData14 += Convert.ToDouble(sData14);
                        dData15 += Convert.ToDouble(sData15);
                        dData16 += Convert.ToDouble(sData16);
                        dData17 += Convert.ToDouble(sData17);
                        dData18 += Convert.ToDouble(sData18);
                        dData19 += Convert.ToDouble(sData19);
                        dData20 += Convert.ToDouble(sData20);
                        dData21 += Convert.ToDouble(sData21);

                        //dSubData += Convert.ToDouble(sSubData); //MCR 20150128
                        dTotalTax += Convert.ToDouble(sTotalTax);
                        dSurcharge += Convert.ToDouble(sSurcharge);
                        dTotalCollect += Convert.ToDouble(sTotalCollect);

                        // RMC 20150126 (S)
                        if (m_iBatchCnt == 1)
                        {
                            this.axVSPrinter1.Table = @"^2000|>500|>1300|>500|>1300|>500|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;"
                          + sDataGroup2 + "|"
                          + sRen + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|" + sNew + "|" + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|" + sRet + "|" + Convert.ToDouble(sRetGross).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData10).ToString("#,##0.00");// +"|"
                            //+ Convert.ToDouble(sSubData).ToString("#,##0.00");
                        }
                        else
                        {
                            this.axVSPrinter1.Table = @"^2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;"
                          + sDataGroup2 + "|"
                          + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData19).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData20).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData21).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                        }
                        // RMC 20150126 (e)
                    }
                    pSet.Close();
                }

                // RMC 20150126 (S)
                if (m_iBatchCnt == 1)
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL |"
                                        + dREN.ToString() + "|"
                                        + dRenGross.ToString("#,##0.00") + "|"
                                        + dNEW.ToString() + "|"
                                        + dNewGross.ToString("#,##0.00") + "|"
                                        + dRET.ToString() + "|"
                                        + dRetGross.ToString("#,##0.00") + "|"
                                          + dData1.ToString("#,##0.00") + "|"
                                          + dData2.ToString("#,##0.00") + "|"
                                          + dData3.ToString("#,##0.00") + "|"
                                          + dData4.ToString("#,##0.00") + "|"
                                          + dData5.ToString("#,##0.00") + "|"
                                          + dData6.ToString("#,##0.00") + "|"
                                          + dData7.ToString("#,##0.00") + "|"
                                          + dData8.ToString("#,##0.00") + "|"
                                          + dData9.ToString("#,##0.00") + "|"
                                          + dData10.ToString("#,##0.00");// +"|"
                    //+ dSubData.ToString("#,##0.00"); //MCR 20150128

                    this.axVSPrinter1.Paragraph = "";

                    //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
                    this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;TOTAL |"
                         + dTREN.ToString() + "|"
                         + dTRenGross.ToString("#,##0.00") + "|"
                         + dTNEW.ToString() + "|"
                         + dTNewGross.ToString("#,##0.00") + "|"
                         + dTRET.ToString() + "|"
                         + dTRetGross.ToString("#,##0.00") + "|"
                         + dTData1.ToString("#,##0.00") + "|"
                         + dTData2.ToString("#,##0.00") + "|"
                         + dTData3.ToString("#,##0.00") + "|"
                         + dTData4.ToString("#,##0.00") + "|"
                         + dTData5.ToString("#,##0.00") + "|"
                         + dTData6.ToString("#,##0.00") + "|"
                         + dTData7.ToString("#,##0.00") + "|"
                         + dTData8.ToString("#,##0.00") + "|"
                         + dTData9.ToString("#,##0.00") + "|"
                         + dTData10.ToString("#,##0.00");// +"|"
                    //+ dTSubData.ToString("#,##0.00");
                }
                else
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL |"
                                      + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dData19.ToString("#,##0.00") + "|"
                                      + dData20.ToString("#,##0.00") + "|"
                                      + dData21.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");

                    this.axVSPrinter1.Paragraph = "";

                                                 //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;TOTAL|"
                              + dTData11.ToString("#,##0.00") + "|"
                              + dTData12.ToString("#,##0.00") + "|"
                              + dTData13.ToString("#,##0.00") + "|"
                              + dTData14.ToString("#,##0.00") + "|"
                              + dTData15.ToString("#,##0.00") + "|"
                              + dTData16.ToString("#,##0.00") + "|"
                              + dTData17.ToString("#,##0.00") + "|"
                              + dTData18.ToString("#,##0.00") + "|"
                              + dTData19.ToString("#,##0.00") + "|"
                              + dTData20.ToString("#,##0.00") + "|"
                              + dTData21.ToString("#,##0.00") + "|"
                              + dTTotalTax.ToString("#,##0.00") + "|"
                              + dTSurcharge.ToString("#,##0.00") + "|"
                              + dTTotalCollect.ToString("#,##0.00");
                }
                // RMC 20150126 (e)

            }
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        #region commented
        //Barangay
        /*private void OnReportSimpSummaryOfCollectionBarangay()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sRen, sNew, sRet, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sData17, sData18, sTotalTax, sSurcharge, sTotalCollect;
            double dREN = 0, dNEW = 0, dRET = 0, dRenGross = 0, dNewGross = 0, dRetGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTREN = 0, dTNEW = 0, dTRET = 0, dTRenGross = 0, dTNewGross = 0, dTRetGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;

            sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+rs.sur_interest) as TOTALCOLLECTION";
            sQuery += " FROM REPORT_SUMCOLL RS";
            sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
            sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
            sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                    sDataGroup = pSet.GetString("data_group1");
                    sDataGroup2 = pSet.GetString("data_group2");

                    sRen = pSet.GetInt("ren_no").ToString();
                    sRenGross = pSet.GetDouble("ren_gross").ToString();
                    sNew = pSet.GetInt("new_no").ToString();
                    sNewGross = pSet.GetDouble("new_cap").ToString();
                    sRet = pSet.GetInt("ret_no").ToString();
                    sRetGross = pSet.GetDouble("ret_gross").ToString();

                    sData1 = pSet.GetDouble("data1").ToString();
                    sData2 = pSet.GetDouble("data2").ToString();
                    sData3 = pSet.GetDouble("data3").ToString();
                    sData4 = pSet.GetDouble("data4").ToString();
                    sData5 = pSet.GetDouble("data5").ToString();
                    sData6 = pSet.GetDouble("data6").ToString();
                    sData7 = pSet.GetDouble("data7").ToString();
                    sData8 = pSet.GetDouble("data8").ToString();
                    sData9 = pSet.GetDouble("data9").ToString();
                    sData10 = pSet.GetDouble("data10").ToString();
                    sData11 = pSet.GetDouble("data11").ToString();
                    sData12 = pSet.GetDouble("data12").ToString();
                    sData13 = pSet.GetDouble("data13").ToString();
                    sData14 = pSet.GetDouble("data14").ToString();
                    sData15 = pSet.GetDouble("data15").ToString();

                    sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                    sSurcharge = pSet.GetDouble("sur_interest").ToString();
                    sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                    #region Total
                    dTREN += Convert.ToDouble(sRen);
                    dTNEW += Convert.ToDouble(sNew);
                    dTRET += Convert.ToDouble(sRet);
                    dTRenGross += Convert.ToDouble(sRenGross);
                    dTNewGross += Convert.ToDouble(sNewGross);
                    dTRetGross += Convert.ToDouble(sRetGross);

                    dTData1 += Convert.ToDouble(sData1);
                    dTData2 += Convert.ToDouble(sData2);
                    dTData3 += Convert.ToDouble(sData3);
                    dTData4 += Convert.ToDouble(sData4);
                    dTData5 += Convert.ToDouble(sData5);
                    dTData6 += Convert.ToDouble(sData6);
                    dTData7 += Convert.ToDouble(sData7);
                    dTData8 += Convert.ToDouble(sData8);
                    dTData9 += Convert.ToDouble(sData9);
                    dTData10 += Convert.ToDouble(sData10);
                    dTData11 += Convert.ToDouble(sData11);
                    dTData12 += Convert.ToDouble(sData12);
                    dTData13 += Convert.ToDouble(sData13);
                    dTData14 += Convert.ToDouble(sData14);
                    dTData15 += Convert.ToDouble(sData15);

                    dTTotalTax += Convert.ToDouble(sTotalTax);
                    dTSurcharge += Convert.ToDouble(sSurcharge);
                    dTTotalCollect += Convert.ToDouble(sTotalCollect);
                    #endregion

                    if (sTempDataGroup == "")
                    {
                        sTempDataGroup = sDataGroup;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                        //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
                        this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                                + sTempDataGroup + "|"
                                + dREN.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNEW.ToString() + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                + dRET.ToString() + "|"
                                + dRetGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

                        this.axVSPrinter1.Paragraph = "";

                        dREN = 0;
                        dRenGross = 0;
                        dNEW = 0;
                        dNewGross = 0;
                        dRET = 0;
                        dRetGross = 0;
                        dData1 = 0;
                        dData2 = 0;
                        dData3 = 0;
                        dData4 = 0;
                        dData5 = 0;
                        dData6 = 0;
                        dData7 = 0;
                        dData8 = 0;
                        dData9 = 0;
                        dData10 = 0;
                        dData11 = 0;
                        dData12 = 0;
                        dData13 = 0;
                        dData14 = 0;
                        dData15 = 0;
                        dTotalTax = 0;
                        dSurcharge = 0;
                        dTotalCollect = 0;

                        sTempDataGroup = sDataGroup;
                    }

                    dREN += Convert.ToDouble(sRen);
                    dNEW += Convert.ToDouble(sNew);
                    dRET += Convert.ToDouble(sRet);
                    dRenGross += Convert.ToDouble(sRenGross);
                    dNewGross += Convert.ToDouble(sNewGross);
                    dRetGross += Convert.ToDouble(sRetGross);

                    dData1 += Convert.ToDouble(sData1);
                    dData2 += Convert.ToDouble(sData2);
                    dData3 += Convert.ToDouble(sData3);
                    dData4 += Convert.ToDouble(sData4);
                    dData5 += Convert.ToDouble(sData5);
                    dData6 += Convert.ToDouble(sData6);
                    dData7 += Convert.ToDouble(sData7);
                    dData8 += Convert.ToDouble(sData8);
                    dData9 += Convert.ToDouble(sData9);
                    dData10 += Convert.ToDouble(sData10);
                    dData11 += Convert.ToDouble(sData11);
                    dData12 += Convert.ToDouble(sData12);
                    dData13 += Convert.ToDouble(sData13);
                    dData14 += Convert.ToDouble(sData14);
                    dData15 += Convert.ToDouble(sData15);

                    dTotalTax += Convert.ToDouble(sTotalTax);
                    dSurcharge += Convert.ToDouble(sSurcharge);
                    dTotalCollect += Convert.ToDouble(sTotalCollect);
                }
                pSet.Close();
            }
            //last record
            this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                                + sTempDataGroup + "|"
                                + dREN.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNEW.ToString() + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                + dRET.ToString() + "|"
                                + dRetGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
                                        //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25   
            this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                      + dTREN.ToString() + "|"
                      + dTRenGross.ToString("#,##0.00") + "|"
                      + dTNEW.ToString() + "|"
                      + dTNewGross.ToString("#,##0.00") + "|"
                      + dTRET.ToString() + "|"
                      + dTRetGross.ToString("#,##0.00") + "|"
                      + dTData1.ToString("#,##0.00") + "|"
                      + dTData2.ToString("#,##0.00") + "|"
                      + dTData3.ToString("#,##0.00") + "|"
                      + dTData4.ToString("#,##0.00") + "|"
                      + dTData5.ToString("#,##0.00") + "|"
                      + dTData6.ToString("#,##0.00") + "|"
                      + dTData7.ToString("#,##0.00") + "|"
                      + dTData8.ToString("#,##0.00") + "|"
                      + dTData9.ToString("#,##0.00") + "|"
                      + dTData10.ToString("#,##0.00") + "|"
                      + dTData11.ToString("#,##0.00") + "|"
                      + dTData12.ToString("#,##0.00") + "|"
                      + dTData13.ToString("#,##0.00") + "|"
                      + dTData14.ToString("#,##0.00") + "|"
                      + dTData15.ToString("#,##0.00") + "|"
                      + dTTotalTax.ToString("#,##0.00") + "|"
                      + dTSurcharge.ToString("#,##0.00") + "|"
                      + dTTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }*/
        #endregion

        private void OnReportWideSummaryOfCollectionBarangay()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sRen, sNew, sRet, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sData17, sData18, sTotalTax, sSurcharge, sTotalCollect;
            double dREN = 0, dNEW = 0, dRET = 0, dRenGross = 0, dNewGross = 0, dRetGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dData17 = 0, dData18 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTREN = 0, dTNEW = 0, dTRET = 0, dTRenGross = 0, dTNewGross = 0, dTRetGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTData17 = 0, dTData18 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;
            double dTData19 = 0, dTData20 = 0, dTData21 = 0, dData19 = 0, dData20 = 0, dData21 = 0;
            String sData19, sData20, sData21;
            double dSubData = 0, dTSubData = 0;
            String sSubData = "";

            for (int i = 1; i <= 2; i++) // RMC 20150126
            {
                //dTData11 = 0; dTData12 = 0; dTData13 = 0; dTData14 = 0; dTData15 = 0; dTData16 = 0; dTData17 = 0; dTData18 = 0; // RMC 20150520 corrections in reports

                // RMC 20150522 corrections in reports (s)
                sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                dTotalTax = 0; dSurcharge = 0; dTotalCollect = 0;
                // RMC 20150522 corrections in reports (e)

                // RMC 20150126 (s)
                m_iBatchCnt = i;

                sTempDataGroup = "";

                if (m_iBatchCnt == 2)
                    this.axVSPrinter1.NewPage();
                // RMC 20150126 (e)

                sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16,RS.data17, RS.data18,RS.data19,RS.data20, RS.data21, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21+rs.sur_interest) as TOTALCOLLECTION";
                sQuery += ", sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10) as SUBTOTALTAXES"; //MCR 20150128
                sQuery += " FROM REPORT_SUMCOLL RS";
                sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
                sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.data17, RS.data18, RS.data19, RS.data20, RS.data21, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";
            
                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        // RMC 20150522 corrections in reports (s)
                        sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                        sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                        sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                        sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                        sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                        // RMC 20150522 corrections in reports (e)

                        sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                        sDataGroup = pSet.GetString("data_group1").Trim();
                        sDataGroup2 = pSet.GetString("data_group2").Trim();

                        sRen = pSet.GetInt("ren_no").ToString();
                        sRenGross = pSet.GetDouble("ren_gross").ToString();
                        sNew = pSet.GetInt("new_no").ToString();
                        sNewGross = pSet.GetDouble("new_cap").ToString();
                        sRet = pSet.GetInt("ret_no").ToString();
                        sRetGross = pSet.GetDouble("ret_gross").ToString();

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            sData1 = pSet.GetDouble("data1").ToString();
                            sData2 = pSet.GetDouble("data2").ToString();
                            sData3 = pSet.GetDouble("data3").ToString();
                            sData4 = pSet.GetDouble("data4").ToString();
                            sData5 = pSet.GetDouble("data5").ToString();
                            sData6 = pSet.GetDouble("data6").ToString();
                            sData7 = pSet.GetDouble("data7").ToString();
                            sData8 = pSet.GetDouble("data8").ToString();
                            sData9 = pSet.GetDouble("data9").ToString();
                            sData10 = pSet.GetDouble("data10").ToString();
                        }
                        else// RMC 20150522 corrections in reports
                        {
                            sData11 = pSet.GetDouble("data11").ToString();
                            sData12 = pSet.GetDouble("data12").ToString();
                            sData13 = pSet.GetDouble("data13").ToString();
                            sData14 = pSet.GetDouble("data14").ToString();
                            sData15 = pSet.GetDouble("data15").ToString();
                            sData16 = pSet.GetDouble("data16").ToString();
                            sData17 = pSet.GetDouble("data17").ToString();
                            sData18 = pSet.GetDouble("data18").ToString();
                            sData19 = pSet.GetDouble("data19").ToString();
                            sData20 = pSet.GetDouble("data20").ToString();
                            sData21 = pSet.GetDouble("data21").ToString();
                        }
                        sSubData = pSet.GetDouble("SUBTOTALTAXES").ToString(); //MCR 20150128

                        sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                        sSurcharge = pSet.GetDouble("sur_interest").ToString();
                        sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                        #region Total

                        if (m_iBatchCnt == 1)
                        {
                            dTREN += Convert.ToDouble(sRen);
                            dTNEW += Convert.ToDouble(sNew);
                            dTRET += Convert.ToDouble(sRet);
                            dTRenGross += Convert.ToDouble(sRenGross);
                            dTNewGross += Convert.ToDouble(sNewGross);
                            dTRetGross += Convert.ToDouble(sRetGross);

                            dTData1 += Convert.ToDouble(sData1);
                            dTData2 += Convert.ToDouble(sData2);
                            dTData3 += Convert.ToDouble(sData3);
                            dTData4 += Convert.ToDouble(sData4);
                            dTData5 += Convert.ToDouble(sData5);
                            dTData6 += Convert.ToDouble(sData6);
                            dTData7 += Convert.ToDouble(sData7);
                            dTData8 += Convert.ToDouble(sData8);
                            dTData9 += Convert.ToDouble(sData9);
                            dTData10 += Convert.ToDouble(sData10);
                            dTSubData += Convert.ToDouble(sSubData); //MCR 20150128
                        }
                        else //2
                        {
                            dTData11 += Convert.ToDouble(sData11);
                            dTData12 += Convert.ToDouble(sData12);
                            dTData13 += Convert.ToDouble(sData13);
                            dTData14 += Convert.ToDouble(sData14);
                            dTData15 += Convert.ToDouble(sData15);
                            dTData16 += Convert.ToDouble(sData16);
                            dTData17 += Convert.ToDouble(sData17);
                            dTData18 += Convert.ToDouble(sData18);
                            dTData19 += Convert.ToDouble(sData19);
                            dTData20 += Convert.ToDouble(sData20);
                            dTData21 += Convert.ToDouble(sData21);

                            dTTotalTax += Convert.ToDouble(sTotalTax);
                            dTSurcharge += Convert.ToDouble(sSurcharge);
                            dTTotalCollect += Convert.ToDouble(sTotalCollect);
                        }
                        #endregion

                        if (sTempDataGroup == "")
                        {
                            this.axVSPrinter1.FontBold = true;
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<2000;" + sTempDataGroup;
                            this.axVSPrinter1.FontBold = false;
                        }
                        else if (sDataGroup != sTempDataGroup)
                        {
                            this.axVSPrinter1.FontBold = true;
                            //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
                            /*this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                    + dREN.ToString() + "|"
                                    + dRenGross.ToString("#,##0.00") + "|"
                                    + dNEW.ToString() + "|"
                                    + dNewGross.ToString("#,##0.00") + "|"
                                    + dRET.ToString() + "|"
                                    + dRetGross.ToString("#,##0.00") + "|"
                                      + dData1.ToString("#,##0.00") + "|"
                                      + dData2.ToString("#,##0.00") + "|"
                                      + dData3.ToString("#,##0.00") + "|"
                                      + dData4.ToString("#,##0.00") + "|"
                                      + dData5.ToString("#,##0.00") + "|"
                                      + dData6.ToString("#,##0.00") + "|"
                                      + dData7.ToString("#,##0.00") + "|"
                                      + dData8.ToString("#,##0.00") + "|"
                                      + dData9.ToString("#,##0.00") + "|"
                                      + dData10.ToString("#,##0.00") + "|"
                                      + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");*/

                            // RMC 20150126 (S)
                            if (m_iBatchCnt == 1)
                            {
                                this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL|"
                                    + dREN.ToString() + "|"
                                    + dRenGross.ToString("#,##0.00") + "|"
                                    + dNEW.ToString() + "|"
                                    + dNewGross.ToString("#,##0.00") + "|"
                                    + dRET.ToString() + "|"
                                    + dRetGross.ToString("#,##0.00") + "|"
                                      + dData1.ToString("#,##0.00") + "|"
                                      + dData2.ToString("#,##0.00") + "|"
                                      + dData3.ToString("#,##0.00") + "|"
                                      + dData4.ToString("#,##0.00") + "|"
                                      + dData5.ToString("#,##0.00") + "|"
                                      + dData6.ToString("#,##0.00") + "|"
                                      + dData7.ToString("#,##0.00") + "|"
                                      + dData8.ToString("#,##0.00") + "|"
                                      + dData9.ToString("#,##0.00") + "|"
                                      + dData10.ToString("#,##0.00") + "|"
                                      + dSubData.ToString("#,##0.00");
                            }
                            else
                            {
                                this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL|"
                                    + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dData19.ToString("#,##0.00") + "|"
                                      + dData20.ToString("#,##0.00") + "|"
                                      + dData21.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");
                            }

                            // RMC 20150126 (E)
                            dREN = 0;
                            dRenGross = 0;
                            dNEW = 0;
                            dNewGross = 0;
                            dRET = 0;
                            dRetGross = 0;
                            dData1 = 0;
                            dData2 = 0;
                            dData3 = 0;
                            dData4 = 0;
                            dData5 = 0;
                            dData6 = 0;
                            dData7 = 0;
                            dData8 = 0;
                            dData9 = 0;
                            dData10 = 0;
                            dData11 = 0;
                            dData12 = 0;
                            dData13 = 0;
                            dData14 = 0;
                            dData15 = 0;
                            dData16 = 0;
                            dData17 = 0;
                            dData18 = 0;
                            dData19 = 0;
                            dData20 = 0;
                            dData21 = 0;
                            dTotalTax = 0;
                            dSurcharge = 0;
                            dTotalCollect = 0;

                            dSubData = 0; //MCR 20150128

                            this.axVSPrinter1.Paragraph = "";
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<2000;" + sDataGroup;
                            this.axVSPrinter1.FontBold = false;
                        }

                        dREN += Convert.ToDouble(sRen);
                        dNEW += Convert.ToDouble(sNew);
                        dRET += Convert.ToDouble(sRet);
                        dRenGross += Convert.ToDouble(sRenGross);
                        dNewGross += Convert.ToDouble(sNewGross);
                        dRetGross += Convert.ToDouble(sRetGross);

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            dData1 += Convert.ToDouble(sData1);
                            dData2 += Convert.ToDouble(sData2);
                            dData3 += Convert.ToDouble(sData3);
                            dData4 += Convert.ToDouble(sData4);
                            dData5 += Convert.ToDouble(sData5);
                            dData6 += Convert.ToDouble(sData6);
                            dData7 += Convert.ToDouble(sData7);
                            dData8 += Convert.ToDouble(sData8);
                            dData9 += Convert.ToDouble(sData9);
                            dData10 += Convert.ToDouble(sData10);
                        }
                        else
                        {
                            dData11 += Convert.ToDouble(sData11);
                            dData12 += Convert.ToDouble(sData12);
                            dData13 += Convert.ToDouble(sData13);
                            dData14 += Convert.ToDouble(sData14);
                            dData15 += Convert.ToDouble(sData15);
                            dData16 += Convert.ToDouble(sData16);
                            dData17 += Convert.ToDouble(sData17);
                            dData18 += Convert.ToDouble(sData18);
                            dData19 += Convert.ToDouble(sData19);
                            dData20 += Convert.ToDouble(sData20);
                            dData21 += Convert.ToDouble(sData21);
                        }

                        dSubData += Convert.ToDouble(sSubData); //MCR 20150128
                        dTotalTax += Convert.ToDouble(sTotalTax);
                        dSurcharge += Convert.ToDouble(sSurcharge);
                        dTotalCollect += Convert.ToDouble(sTotalCollect);
                        //this.axVSPrinter1.FontSize;


                        //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25    26    27    28
                        /*this.axVSPrinter1.Table = @"^2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                          + sDataGroup2 + "|"
                          + sRen + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|" + sNew + "|" + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|" + sRet + "|" + Convert.ToDouble(sRetGross).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");*/

                        // RMC 20150126 (S)
                        if (m_iBatchCnt == 1)
                        {
                            this.axVSPrinter1.Table = @"^2000|>500|>1300|>500|>1300|>500|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;"
                          + sDataGroup2 + "|"
                          + sRen + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|" + sNew + "|" + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|" + sRet + "|" + Convert.ToDouble(sRetGross).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSubData).ToString("#,##0.00");
                        }
                        else
                        {
                            this.axVSPrinter1.Table = @"^2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;"
                          + sDataGroup2 + "|"
                          + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData19).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData20).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData21).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                        }
                        // RMC 20150126 (e)
                    }
                    pSet.Close();
                }

                /*this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                    + dREN.ToString() + "|"
                                    + dRenGross.ToString("#,##0.00") + "|"
                                    + dNEW.ToString() + "|"
                                    + dNewGross.ToString("#,##0.00") + "|"
                                    + dRET.ToString() + "|"
                                    + dRetGross.ToString("#,##0.00") + "|"
                                      + dData1.ToString("#,##0.00") + "|"
                                      + dData2.ToString("#,##0.00") + "|"
                                      + dData3.ToString("#,##0.00") + "|"
                                      + dData4.ToString("#,##0.00") + "|"
                                      + dData5.ToString("#,##0.00") + "|"
                                      + dData6.ToString("#,##0.00") + "|"
                                      + dData7.ToString("#,##0.00") + "|"
                                      + dData8.ToString("#,##0.00") + "|"
                                      + dData9.ToString("#,##0.00") + "|"
                                      + dData10.ToString("#,##0.00") + "|"
                                      + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");

                

                this.axVSPrinter1.Paragraph = "";

                //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
                this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                          + dTREN.ToString() + "|"
                          + dTRenGross.ToString("#,##0.00") + "|"
                          + dTNEW.ToString() + "|"
                          + dTNewGross.ToString("#,##0.00") + "|"
                          + dTRET.ToString() + "|"
                          + dTRetGross.ToString("#,##0.00") + "|"
                          + dTData1.ToString("#,##0.00") + "|"
                          + dTData2.ToString("#,##0.00") + "|"
                          + dTData3.ToString("#,##0.00") + "|"
                          + dTData4.ToString("#,##0.00") + "|"
                          + dTData5.ToString("#,##0.00") + "|"
                          + dTData6.ToString("#,##0.00") + "|"
                          + dTData7.ToString("#,##0.00") + "|"
                          + dTData8.ToString("#,##0.00") + "|"
                          + dTData9.ToString("#,##0.00") + "|"
                          + dTData10.ToString("#,##0.00") + "|"
                          + dTData11.ToString("#,##0.00") + "|"
                          + dTData12.ToString("#,##0.00") + "|"
                          + dTData13.ToString("#,##0.00") + "|"
                          + dTData14.ToString("#,##0.00") + "|"
                          + dTData15.ToString("#,##0.00") + "|"
                          + dTData16.ToString("#,##0.00") + "|"
                          + dTData17.ToString("#,##0.00") + "|"
                          + dTData18.ToString("#,##0.00") + "|"
                          + dTTotalTax.ToString("#,##0.00") + "|"
                          + dTSurcharge.ToString("#,##0.00") + "|"
                          + dTTotalCollect.ToString("#,##0.00");*/

                // RMC 20150126 (S)
                if (m_iBatchCnt == 1)
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL |"
                                        + dREN.ToString() + "|"
                                        + dRenGross.ToString("#,##0.00") + "|"
                                        + dNEW.ToString() + "|"
                                        + dNewGross.ToString("#,##0.00") + "|"
                                        + dRET.ToString() + "|"
                                        + dRetGross.ToString("#,##0.00") + "|"
                                          + dData1.ToString("#,##0.00") + "|"
                                          + dData2.ToString("#,##0.00") + "|"
                                          + dData3.ToString("#,##0.00") + "|"
                                          + dData4.ToString("#,##0.00") + "|"
                                          + dData5.ToString("#,##0.00") + "|"
                                          + dData6.ToString("#,##0.00") + "|"
                                          + dData7.ToString("#,##0.00") + "|"
                                          + dData8.ToString("#,##0.00") + "|"
                                          + dData9.ToString("#,##0.00") + "|"
                                          + dData10.ToString("#,##0.00") + "|"
                                          + dSubData.ToString("#,##0.00"); //MCR 20150128

                    this.axVSPrinter1.Paragraph = "";

                    //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
                    this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;TOTAL |"
                         + dTREN.ToString() + "|"
                         + dTRenGross.ToString("#,##0.00") + "|"
                         + dTNEW.ToString() + "|"
                         + dTNewGross.ToString("#,##0.00") + "|"
                         + dTRET.ToString() + "|"
                         + dTRetGross.ToString("#,##0.00") + "|"
                         + dTData1.ToString("#,##0.00") + "|"
                         + dTData2.ToString("#,##0.00") + "|"
                         + dTData3.ToString("#,##0.00") + "|"
                         + dTData4.ToString("#,##0.00") + "|"
                         + dTData5.ToString("#,##0.00") + "|"
                         + dTData6.ToString("#,##0.00") + "|"
                         + dTData7.ToString("#,##0.00") + "|"
                         + dTData8.ToString("#,##0.00") + "|"
                         + dTData9.ToString("#,##0.00") + "|"
                         + dTData10.ToString("#,##0.00") + "|"
                         + dTSubData.ToString("#,##0.00");
                }
                else
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL |"
                                      + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dData19.ToString("#,##0.00") + "|"
                                      + dData20.ToString("#,##0.00") + "|"
                                      + dData21.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");



                    this.axVSPrinter1.Paragraph = "";

                    //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28

                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;TOTAL|"
                              + dTData11.ToString("#,##0.00") + "|"
                              + dTData12.ToString("#,##0.00") + "|"
                              + dTData13.ToString("#,##0.00") + "|"
                              + dTData14.ToString("#,##0.00") + "|"
                              + dTData15.ToString("#,##0.00") + "|"
                              + dTData16.ToString("#,##0.00") + "|"
                              + dTData17.ToString("#,##0.00") + "|"
                              + dTData18.ToString("#,##0.00") + "|"
                              + dTData19.ToString("#,##0.00") + "|"
                              + dTData20.ToString("#,##0.00") + "|"
                              + dTData21.ToString("#,##0.00") + "|"
                              + dTTotalTax.ToString("#,##0.00") + "|"
                              + dTSurcharge.ToString("#,##0.00") + "|"
                              + dTTotalCollect.ToString("#,##0.00");
                }
                // RMC 20150126 (e)

            }
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        //District
        private void OnReportSimpSummaryOfCollectionDistrict()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sRen, sNew, sRet, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sTotalTax, sSurcharge, sTotalCollect;
            double dREN = 0, dNEW = 0, dRET = 0, dRenGross = 0, dNewGross = 0, dRetGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTREN = 0, dTNEW = 0, dTRET = 0, dTRenGross = 0, dTNewGross = 0, dTRetGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;

            sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.sur_interest) as TOTALCOLLECTION";
            sQuery += " FROM REPORT_SUMCOLL RS";
            sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
            sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
            sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                    sDataGroup = pSet.GetString("data_group1");
                    sDataGroup2 = pSet.GetString("data_group2");

                    sRen = pSet.GetInt("ren_no").ToString();
                    sRenGross = pSet.GetDouble("ren_gross").ToString();
                    sNew = pSet.GetInt("new_no").ToString();
                    sNewGross = pSet.GetDouble("new_cap").ToString();
                    sRet = pSet.GetInt("ret_no").ToString();
                    sRetGross = pSet.GetDouble("ret_gross").ToString();

                    sData1 = pSet.GetDouble("data1").ToString();
                    sData2 = pSet.GetDouble("data2").ToString();
                    sData3 = pSet.GetDouble("data3").ToString();
                    sData4 = pSet.GetDouble("data4").ToString();
                    sData5 = pSet.GetDouble("data5").ToString();
                    sData6 = pSet.GetDouble("data6").ToString();
                    sData7 = pSet.GetDouble("data7").ToString();
                    sData8 = pSet.GetDouble("data8").ToString();
                    sData9 = pSet.GetDouble("data9").ToString();
                    sData10 = pSet.GetDouble("data10").ToString();
                    sData11 = pSet.GetDouble("data11").ToString();
                    sData12 = pSet.GetDouble("data12").ToString();
                    sData13 = pSet.GetDouble("data13").ToString();
                    sData14 = pSet.GetDouble("data14").ToString();
                    sData15 = pSet.GetDouble("data15").ToString();
                    sData16 = pSet.GetDouble("data16").ToString();

                    sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                    sSurcharge = pSet.GetDouble("sur_interest").ToString();
                    sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                    #region Total
                    dTREN += Convert.ToDouble(sRen);
                    dTNEW += Convert.ToDouble(sNew);
                    dTRET += Convert.ToDouble(sRet);
                    dTRenGross += Convert.ToDouble(sRenGross);
                    dTNewGross += Convert.ToDouble(sNewGross);
                    dTRetGross += Convert.ToDouble(sRetGross);

                    dTData1 += Convert.ToDouble(sData1);
                    dTData2 += Convert.ToDouble(sData2);
                    dTData3 += Convert.ToDouble(sData3);
                    dTData4 += Convert.ToDouble(sData4);
                    dTData5 += Convert.ToDouble(sData5);
                    dTData6 += Convert.ToDouble(sData6);
                    dTData7 += Convert.ToDouble(sData7);
                    dTData8 += Convert.ToDouble(sData8);
                    dTData9 += Convert.ToDouble(sData9);
                    dTData10 += Convert.ToDouble(sData10);
                    dTData11 += Convert.ToDouble(sData11);
                    dTData12 += Convert.ToDouble(sData12);
                    dTData13 += Convert.ToDouble(sData13);
                    dTData14 += Convert.ToDouble(sData14);
                    dTData15 += Convert.ToDouble(sData15);
                    dTData16 += Convert.ToDouble(sData16);

                    dTTotalTax += Convert.ToDouble(sTotalTax);
                    dTSurcharge += Convert.ToDouble(sSurcharge);
                    dTTotalCollect += Convert.ToDouble(sTotalCollect);
                    #endregion

                    if (sTempDataGroup == "")
                    {
                        sTempDataGroup = sDataGroup;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                                                    //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26 
                        this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                                + sTempDataGroup + "|"
                                + dREN.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNEW.ToString() + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                + dRET.ToString() + "|"
                                + dRetGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dData16.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

                        this.axVSPrinter1.Paragraph = "";

                        dREN = 0;
                        dRenGross = 0;
                        dNEW = 0;
                        dNewGross = 0;
                        dRET = 0;
                        dRetGross = 0;
                        dData1 = 0;
                        dData2 = 0;
                        dData3 = 0;
                        dData4 = 0;
                        dData5 = 0;
                        dData6 = 0;
                        dData7 = 0;
                        dData8 = 0;
                        dData9 = 0;
                        dData10 = 0;
                        dData11 = 0;
                        dData12 = 0;
                        dData13 = 0;
                        dData14 = 0;
                        dData15 = 0;
                        dData16 = 0;
                        dTotalTax = 0;
                        dSurcharge = 0;
                        dTotalCollect = 0;

                        sTempDataGroup = sDataGroup;
                    }

                    dREN += Convert.ToDouble(sRen);
                    dNEW += Convert.ToDouble(sNew);
                    dRET += Convert.ToDouble(sRet);
                    dRenGross += Convert.ToDouble(sRenGross);
                    dNewGross += Convert.ToDouble(sNewGross);
                    dRetGross += Convert.ToDouble(sRetGross);

                    dData1 += Convert.ToDouble(sData1);
                    dData2 += Convert.ToDouble(sData2);
                    dData3 += Convert.ToDouble(sData3);
                    dData4 += Convert.ToDouble(sData4);
                    dData5 += Convert.ToDouble(sData5);
                    dData6 += Convert.ToDouble(sData6);
                    dData7 += Convert.ToDouble(sData7);
                    dData8 += Convert.ToDouble(sData8);
                    dData9 += Convert.ToDouble(sData9);
                    dData10 += Convert.ToDouble(sData10);
                    dData11 += Convert.ToDouble(sData11);
                    dData12 += Convert.ToDouble(sData12);
                    dData13 += Convert.ToDouble(sData13);
                    dData14 += Convert.ToDouble(sData14);
                    dData15 += Convert.ToDouble(sData15);
                    dData16 += Convert.ToDouble(sData16);

                    dTotalTax += Convert.ToDouble(sTotalTax);
                    dSurcharge += Convert.ToDouble(sSurcharge);
                    dTotalCollect += Convert.ToDouble(sTotalCollect);
                }
                pSet.Close();
            }
            //last record
            this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                                + sTempDataGroup + "|"
                                + dREN.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNEW.ToString() + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                + dRET.ToString() + "|"
                                + dRetGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dData16.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";

                                        //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25   26
            this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                      + dTREN.ToString() + "|"
                      + dTRenGross.ToString("#,##0.00") + "|"
                      + dTNEW.ToString() + "|"
                      + dTNewGross.ToString("#,##0.00") + "|"
                      + dTRET.ToString() + "|"
                      + dTRetGross.ToString("#,##0.00") + "|"
                      + dTData1.ToString("#,##0.00") + "|"
                      + dTData2.ToString("#,##0.00") + "|"
                      + dTData3.ToString("#,##0.00") + "|"
                      + dTData4.ToString("#,##0.00") + "|"
                      + dTData5.ToString("#,##0.00") + "|"
                      + dTData6.ToString("#,##0.00") + "|"
                      + dTData7.ToString("#,##0.00") + "|"
                      + dTData8.ToString("#,##0.00") + "|"
                      + dTData9.ToString("#,##0.00") + "|"
                      + dTData10.ToString("#,##0.00") + "|"
                      + dTData11.ToString("#,##0.00") + "|"
                      + dTData12.ToString("#,##0.00") + "|"
                      + dTData13.ToString("#,##0.00") + "|"
                      + dTData14.ToString("#,##0.00") + "|"
                      + dTData15.ToString("#,##0.00") + "|"
                      + dTData16.ToString("#,##0.00") + "|"
                      + dTTotalTax.ToString("#,##0.00") + "|"
                      + dTSurcharge.ToString("#,##0.00") + "|"
                      + dTTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        private void OnReportWideSummaryOfCollectionDistrict()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sRen, sNew, sRet, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sTotalTax, sSurcharge, sTotalCollect;
            double dREN = 0, dNEW = 0, dRET = 0, dRenGross = 0, dNewGross = 0, dRetGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTREN = 0, dTNEW = 0, dTRET = 0, dTRenGross = 0, dTNewGross = 0, dTRetGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;

            sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.sur_interest) as TOTALCOLLECTION";
            sQuery += " FROM REPORT_SUMCOLL RS";
            sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
            sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
            sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                    sDataGroup = pSet.GetString("data_group1");
                    sDataGroup2 = pSet.GetString("data_group2");

                    sRen = pSet.GetInt("ren_no").ToString();
                    sRenGross = pSet.GetDouble("ren_gross").ToString();
                    sNew = pSet.GetInt("new_no").ToString();
                    sNewGross = pSet.GetDouble("new_cap").ToString();
                    sRet = pSet.GetInt("ret_no").ToString();
                    sRetGross = pSet.GetDouble("ret_gross").ToString();

                    sData1 = pSet.GetDouble("data1").ToString();
                    sData2 = pSet.GetDouble("data2").ToString();
                    sData3 = pSet.GetDouble("data3").ToString();
                    sData4 = pSet.GetDouble("data4").ToString();
                    sData5 = pSet.GetDouble("data5").ToString();
                    sData6 = pSet.GetDouble("data6").ToString();
                    sData7 = pSet.GetDouble("data7").ToString();
                    sData8 = pSet.GetDouble("data8").ToString();
                    sData9 = pSet.GetDouble("data9").ToString();
                    sData10 = pSet.GetDouble("data10").ToString();
                    sData11 = pSet.GetDouble("data11").ToString();
                    sData12 = pSet.GetDouble("data12").ToString();
                    sData13 = pSet.GetDouble("data13").ToString();
                    sData14 = pSet.GetDouble("data14").ToString();
                    sData15 = pSet.GetDouble("data15").ToString();
                    sData16 = pSet.GetDouble("data16").ToString();

                    sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                    sSurcharge = pSet.GetDouble("sur_interest").ToString();
                    sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                    #region Total
                    dTREN += Convert.ToDouble(sRen);
                    dTNEW += Convert.ToDouble(sNew);
                    dTRET += Convert.ToDouble(sRet);
                    dTRenGross += Convert.ToDouble(sRenGross);
                    dTNewGross += Convert.ToDouble(sNewGross);
                    dTRetGross += Convert.ToDouble(sRetGross);

                    dTData1 += Convert.ToDouble(sData1);
                    dTData2 += Convert.ToDouble(sData2);
                    dTData3 += Convert.ToDouble(sData3);
                    dTData4 += Convert.ToDouble(sData4);
                    dTData5 += Convert.ToDouble(sData5);
                    dTData6 += Convert.ToDouble(sData6);
                    dTData7 += Convert.ToDouble(sData7);
                    dTData8 += Convert.ToDouble(sData8);
                    dTData9 += Convert.ToDouble(sData9);
                    dTData10 += Convert.ToDouble(sData10);
                    dTData11 += Convert.ToDouble(sData11);
                    dTData12 += Convert.ToDouble(sData12);
                    dTData13 += Convert.ToDouble(sData13);
                    dTData14 += Convert.ToDouble(sData14);
                    dTData15 += Convert.ToDouble(sData15);
                    dTData16 += Convert.ToDouble(sData16);

                    dTTotalTax += Convert.ToDouble(sTotalTax);
                    dTSurcharge += Convert.ToDouble(sSurcharge);
                    dTTotalCollect += Convert.ToDouble(sTotalCollect);
                    #endregion

                    if (sTempDataGroup == "")
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<1500;" + sTempDataGroup;
                        this.axVSPrinter1.FontBold = false;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                                                   //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26
                        this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                + dREN.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNEW.ToString() + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                + dRET.ToString() + "|"
                                + dRetGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dData16.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

                        dREN = 0;
                        dRenGross = 0;
                        dNEW = 0;
                        dNewGross = 0;
                        dRET = 0;
                        dRetGross = 0;
                        dData1 = 0;
                        dData2 = 0;
                        dData3 = 0;
                        dData4 = 0;
                        dData5 = 0;
                        dData6 = 0;
                        dData7 = 0;
                        dData8 = 0;
                        dData9 = 0;
                        dData10 = 0;
                        dData11 = 0;
                        dData12 = 0;
                        dData13 = 0;
                        dData14 = 0;
                        dData15 = 0;
                        dData16 = 0;
                        dTotalTax = 0;
                        dSurcharge = 0;
                        dTotalCollect = 0;

                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<1500;" + sDataGroup;
                        this.axVSPrinter1.FontBold = false;
                    }

                    dREN += Convert.ToDouble(sRen);
                    dNEW += Convert.ToDouble(sNew);
                    dRET += Convert.ToDouble(sRet);
                    dRenGross += Convert.ToDouble(sRenGross);
                    dNewGross += Convert.ToDouble(sNewGross);
                    dRetGross += Convert.ToDouble(sRetGross);

                    dData1 += Convert.ToDouble(sData1);
                    dData2 += Convert.ToDouble(sData2);
                    dData3 += Convert.ToDouble(sData3);
                    dData4 += Convert.ToDouble(sData4);
                    dData5 += Convert.ToDouble(sData5);
                    dData6 += Convert.ToDouble(sData6);
                    dData7 += Convert.ToDouble(sData7);
                    dData8 += Convert.ToDouble(sData8);
                    dData9 += Convert.ToDouble(sData9);
                    dData10 += Convert.ToDouble(sData10);
                    dData11 += Convert.ToDouble(sData11);
                    dData12 += Convert.ToDouble(sData12);
                    dData13 += Convert.ToDouble(sData13);
                    dData14 += Convert.ToDouble(sData14);
                    dData15 += Convert.ToDouble(sData15);
                    dData16 += Convert.ToDouble(sData16);

                    dTotalTax += Convert.ToDouble(sTotalTax);
                    dSurcharge += Convert.ToDouble(sSurcharge);
                    dTotalCollect += Convert.ToDouble(sTotalCollect);
                    //this.axVSPrinter1.FontSize;

                                                   //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16     17   18     19    20    21   22   23   24   25    26 
                    this.axVSPrinter1.Table = @"^2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                      + sDataGroup2 + "|"
                      + sRen + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|" + sNew + "|" + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|" + sRet + "|" + Convert.ToDouble(sRetGross).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                }
                pSet.Close();
            }

            this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                + dREN.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNEW.ToString() + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                + dRET.ToString() + "|"
                                + dRetGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dData16.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";

                                        //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26
            this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                      + dTREN.ToString() + "|"
                      + dTRenGross.ToString("#,##0.00") + "|"
                      + dTNEW.ToString() + "|"
                      + dTNewGross.ToString("#,##0.00") + "|"
                      + dTRET.ToString() + "|"
                      + dTRetGross.ToString("#,##0.00") + "|"
                      + dTData1.ToString("#,##0.00") + "|"
                      + dTData2.ToString("#,##0.00") + "|"
                      + dTData3.ToString("#,##0.00") + "|"
                      + dTData4.ToString("#,##0.00") + "|"
                      + dTData5.ToString("#,##0.00") + "|"
                      + dTData6.ToString("#,##0.00") + "|"
                      + dTData7.ToString("#,##0.00") + "|"
                      + dTData8.ToString("#,##0.00") + "|"
                      + dTData9.ToString("#,##0.00") + "|"
                      + dTData10.ToString("#,##0.00") + "|"
                      + dTData11.ToString("#,##0.00") + "|"
                      + dTData12.ToString("#,##0.00") + "|"
                      + dTData13.ToString("#,##0.00") + "|"
                      + dTData14.ToString("#,##0.00") + "|"
                      + dTData15.ToString("#,##0.00") + "|"
                      + dTData16.ToString("#,##0.00") + "|"
                      + dTTotalTax.ToString("#,##0.00") + "|"
                      + dTSurcharge.ToString("#,##0.00") + "|"
                      + dTTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        //Main Business
        private void OnReportSimpSummaryOfCollectionMainBusiness()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sNew, sRenGross, sNewGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sTotalTax, sSurcharge, sTotalCollect;
            double dNEW = 0, dRenGross = 0, dNewGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTNEW = 0, dTRenGross = 0, dTNewGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;
            double dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTData17 = 0, dTData18 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dData17 = 0, dData18 = 0;
            double dTData19 = 0, dTData20 = 0, dTData21 = 0, dData19 = 0, dData20 = 0, dData21 = 0;
            String sData13, sData14, sData15, sData16, sData17, sData18, sData19, sData20, sData21;
            double dSubData = 0, dTSubData = 0;
            String sSubData = "";

            for (int i = 1; i <= 2; i++)
            {
                sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                dTotalTax = 0; dSurcharge = 0; dTotalCollect = 0;

                m_iBatchCnt = i;

                sTempDataGroup = "";

                if (m_iBatchCnt == 2)
                    this.axVSPrinter1.NewPage();

                sQuery = @"SELECT RS.data_group1, sum(RS.new_no) as new_no, sum(RS.new_cap) as new_cap, sum(RS.ren_no) as ren_no,  ";
                sQuery += " sum(RS.ren_gross) as ren_gross, sum(RS.ret_no) as ret_no, sum(RS.ret_gross) as ret_gross, sum(RS.data1) as data1, ";
                sQuery += " sum(RS.data2) as data2, sum(RS.data3) as data3, sum(RS.data4) as data4, sum(RS.data5) as data5, ";
                sQuery += " sum(RS.data6) as data6, sum(RS.data7) as data7, sum(RS.data8) as data8, sum(RS.data9) as data9, ";
                sQuery += " sum(RS.data10) as data10, sum(RS.data11) as data11, sum(RS.data12) as data12, sum(RS.data13) as data13, ";
                sQuery += " sum(RS.data14) as data14, sum(RS.data15) as data15, sum(RS.data16) as data16, sum(RS.data17) as data17, ";
                sQuery += " sum(RS.data18) as data18, sum(RS.data19) as data19, sum(RS.data20) as data20, sum(RS.data21) as data21, ";
                sQuery += " sum(RS.sur_interest) as sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, ";
                sQuery += " sum(RS.data1)+sum(RS.data2)+sum(RS.data3)+sum(RS.data4)+sum(RS.data5)+sum(RS.data6)+sum(RS.data7)+sum(RS.data8)+";
                sQuery += " sum(RS.data9)+sum(RS.data10)+sum(RS.data11)+sum(RS.data12)+sum(RS.data13)+sum(RS.data14)+sum(RS.data15)+";
                sQuery += " sum(RS.data16)+sum(RS.data17)+sum(RS.data18)+sum(RS.data19)+sum(RS.data20)+sum(RS.data21) as TOTALTAXES, ";
                sQuery += " sum(RS.data1)+sum(RS.data2)+sum(RS.data3)+sum(RS.data4)+sum(RS.data5)+sum(RS.data6)+sum(RS.data7)+sum(RS.data8)+";
                sQuery += " sum(RS.data9)+sum(RS.data10)+sum(RS.data11)+sum(RS.data12)+sum(RS.data13)+sum(RS.data14)+sum(RS.data15)+";
                sQuery += " sum(RS.data16)+sum(RS.data17)+sum(RS.data18)+sum(RS.data19)+sum(RS.data20)+sum(RS.data21)+sum(rs.sur_interest) as TOTALCOLLECTION, ";
                sQuery += " sum(RS.data1)+sum(RS.data2)+sum(RS.data3)+sum(RS.data4)+sum(RS.data5)+sum(RS.data6)+sum(RS.data7)+sum(RS.data8)+";
                sQuery += " sum(RS.data9)+sum(RS.data10) as SUBTOTALTAXES";
                sQuery += " FROM REPORT_SUMCOLL RS";
                sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
                sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                if (m_sFilterValue != "")
                    sQuery += " AND RS.data_group1 in (" + m_sFilterValue + ")";
                sQuery += " GROUP BY RS.data_group1, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC";

                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                        sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                        sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                        sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                        sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                        dTotalTax = 0; dSurcharge = 0; dTotalCollect = 0;

                        sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                        sDataGroup = pSet.GetString("data_group1");
                      
                        sNew = pSet.GetInt("new_no").ToString();
                        sRenGross = pSet.GetDouble("ren_gross").ToString();
                        sNewGross = pSet.GetDouble("new_cap").ToString();

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            sData1 = pSet.GetDouble("data1").ToString();
                            sData2 = pSet.GetDouble("data2").ToString();
                            sData3 = pSet.GetDouble("data3").ToString();
                            sData4 = pSet.GetDouble("data4").ToString();
                            sData5 = pSet.GetDouble("data5").ToString();
                            sData6 = pSet.GetDouble("data6").ToString();
                            sData7 = pSet.GetDouble("data7").ToString();
                            sData8 = pSet.GetDouble("data8").ToString();
                            sData9 = pSet.GetDouble("data9").ToString();
                            sData10 = pSet.GetDouble("data10").ToString();
                            sSubData = pSet.GetDouble("SUBTOTALTAXES").ToString();
                        }
                        else
                        {
                            sData11 = pSet.GetDouble("data11").ToString();
                            sData12 = pSet.GetDouble("data12").ToString();
                            sData13 = pSet.GetDouble("data13").ToString();
                            sData14 = pSet.GetDouble("data14").ToString();
                            sData15 = pSet.GetDouble("data15").ToString();
                            sData16 = pSet.GetDouble("data16").ToString();
                            sData17 = pSet.GetDouble("data17").ToString();
                            sData18 = pSet.GetDouble("data18").ToString();
                            sData19 = pSet.GetDouble("data19").ToString();
                            sData20 = pSet.GetDouble("data20").ToString();
                            sData21 = pSet.GetDouble("data21").ToString();
                            sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                            sSurcharge = pSet.GetDouble("sur_interest").ToString();
                            sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();
                        }

                        if (m_iBatchCnt == 1)
                        {//1   2    3     4     5      6    7      8    9    10     11    12    13    14    15   16    17   18     19  
                            this.axVSPrinter1.Table = "<2000|>500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                                    + sDataGroup + "|"
                                    + Convert.ToDouble(sNew).ToString("#,##0") + "|"
                                    + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|"
                                    + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|"
                                      + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sSubData).ToString("#,##0.00");
                        }
                        else
                        {
                            this.axVSPrinter1.Table = "<2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;"
                                + sDataGroup + "|"
                                  + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData19).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData20).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData21).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                        }

                        this.axVSPrinter1.Paragraph = "";
                        #region Total
                        

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            dTNEW += Convert.ToDouble(sNew);
                            dTRenGross += Convert.ToDouble(sRenGross);
                            dTNewGross += Convert.ToDouble(sNewGross);
                            dTData1 += Convert.ToDouble(sData1);
                            dTData2 += Convert.ToDouble(sData2);
                            dTData3 += Convert.ToDouble(sData3);
                            dTData4 += Convert.ToDouble(sData4);
                            dTData5 += Convert.ToDouble(sData5);
                            dTData6 += Convert.ToDouble(sData6);
                            dTData7 += Convert.ToDouble(sData7);
                            dTData8 += Convert.ToDouble(sData8);
                            dTData9 += Convert.ToDouble(sData9);
                            dTData10 += Convert.ToDouble(sData10);
                            dTSubData += Convert.ToDouble(sSubData);
                        }
                        else
                        {
                            dTData11 += Convert.ToDouble(sData11);
                            dTData12 += Convert.ToDouble(sData12);
                            dTData13 += Convert.ToDouble(sData13);
                            dTData14 += Convert.ToDouble(sData14);
                            dTData15 += Convert.ToDouble(sData15);
                            dTData16 += Convert.ToDouble(sData16);
                            dTData17 += Convert.ToDouble(sData17);
                            dTData18 += Convert.ToDouble(sData18);
                            dTData19 += Convert.ToDouble(sData19);
                            dTData20 += Convert.ToDouble(sData20);
                            dTData21 += Convert.ToDouble(sData21);
                            dTTotalTax += Convert.ToDouble(sTotalTax);
                            dTSurcharge += Convert.ToDouble(sSurcharge);
                            dTTotalCollect += Convert.ToDouble(sTotalCollect);
                        }

                        
                        
                        #endregion
 
                    }
                    pSet.Close();
                }
                //last record

                
            }
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
            //1    2    3      4    5      6    7      8    9    10     11    12    13    14    15   16    17   18     19  
            if (m_iBatchCnt == 1)
            {
                this.axVSPrinter1.Table = "<2000|>500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;TOTAL |"
                    //this.axVSPrinter1.Table = "<4000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                          + dTNEW.ToString() + "|"
                          + dTRenGross.ToString("#,##0.00") + "|"
                          + dTNewGross.ToString("#,##0.00") + "|"
                          + dTData1.ToString("#,##0.00") + "|"
                          + dTData2.ToString("#,##0.00") + "|"
                          + dTData3.ToString("#,##0.00") + "|"
                          + dTData4.ToString("#,##0.00") + "|"
                          + dTData5.ToString("#,##0.00") + "|"
                          + dTData6.ToString("#,##0.00") + "|"
                          + dTData7.ToString("#,##0.00") + "|"
                          + dTData8.ToString("#,##0.00") + "|"
                          + dTData9.ToString("#,##0.00") + "|"
                          + dTData10.ToString("#,##0.00") + "|"
                          + dTSubData.ToString("#,##0.00");
            }
            else
            {
                this.axVSPrinter1.Table = "<2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;TOTAL |"
                  + dTData11.ToString("#,##0.00") + "|"
                  + dTData12.ToString("#,##0.00") + "|"
                  + dTData13.ToString("#,##0.00") + "|"
                  + dTData14.ToString("#,##0.00") + "|"
                  + dTData15.ToString("#,##0.00") + "|"
                  + dTData16.ToString("#,##0.00") + "|"
                  + dTData17.ToString("#,##0.00") + "|"
                  + dTData18.ToString("#,##0.00") + "|"
                  + dTData19.ToString("#,##0.00") + "|"
                  + dTData20.ToString("#,##0.00") + "|"
                  + dTData21.ToString("#,##0.00") + "|"
                  + dTTotalTax.ToString("#,##0.00") + "|"
                  + dTSurcharge.ToString("#,##0.00") + "|"
                  + dTTotalCollect.ToString("#,##0.00");
            }

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        private void OnReportWideSummaryOfCollectionMainBusiness()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sNew, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sData17, sData18, sData19,sData20,sData21,sTotalTax, sSurcharge, sTotalCollect, sSubData;
            double dNEW = 0, dRenGross = 0, dNewGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dData17 = 0, dData18 = 0, dData19 = 0,dData20 = 0,dData21 = 0,dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0, dTSubData = 0;
            double dTNEW = 0, dTRenGross = 0, dTNewGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTData17 = 0, dTData18 = 0, dTData19 = 0,dTData20 = 0,dTData21 = 0,dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;
            double dSubData = 0;

            for (int i = 1; i <= 2; i++)
            {
                // RMC 20150522 corrections in reports (s)
                sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                dTotalTax = 0; dSurcharge = 0; dTotalCollect = 0;
                // RMC 20150522 corrections in reports (e)

                m_iBatchCnt = i;

                sTempDataGroup = "";

                if (m_iBatchCnt == 2)
                {
                    this.axVSPrinter1.NewPage();
                    CreateHeaderLubao(); //AFM 20200331
                }

                sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16,RS.data17, RS.data18,RS.data19,RS.data20, RS.data21, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21+rs.sur_interest) as TOTALCOLLECTION";
                sQuery += ", sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10) as SUBTOTALTAXES";
                sQuery += " FROM REPORT_SUMCOLL RS";
                sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
                sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                if (m_sFilterValue != "")
                    sQuery += " AND RS.data_group1 in (" + m_sFilterValue + ")";
                sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.data17, RS.data18, RS.data19, RS.data20, RS.data21,RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        // RMC 20150522 corrections in reports (s)
                        sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                        sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                        sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                        sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                        sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                        // RMC 20150522 corrections in reports (e)

                        sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                        sDataGroup = pSet.GetString("data_group1");
                        sDataGroup2 = pSet.GetString("data_group2");

                        sNew = pSet.GetInt("new_no").ToString();
                        sRenGross = pSet.GetDouble("ren_gross").ToString();
                        sNewGross = pSet.GetDouble("new_cap").ToString();

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            sData1 = pSet.GetDouble("data1").ToString();
                            sData2 = pSet.GetDouble("data2").ToString();
                            sData3 = pSet.GetDouble("data3").ToString();
                            sData4 = pSet.GetDouble("data4").ToString();
                            sData5 = pSet.GetDouble("data5").ToString();
                            sData6 = pSet.GetDouble("data6").ToString();
                            sData7 = pSet.GetDouble("data7").ToString();
                            sData8 = pSet.GetDouble("data8").ToString();
                            sData9 = pSet.GetDouble("data9").ToString();
                            sData10 = pSet.GetDouble("data10").ToString();
                        }
                        else
                        {
                            sData11 = pSet.GetDouble("data11").ToString();
                            sData12 = pSet.GetDouble("data12").ToString();
                            sData13 = pSet.GetDouble("data13").ToString();
                            sData14 = pSet.GetDouble("data14").ToString();
                            sData15 = pSet.GetDouble("data15").ToString();
                            sData16 = pSet.GetDouble("data16").ToString();
                            sData17 = pSet.GetDouble("data17").ToString();
                            sData18 = pSet.GetDouble("data18").ToString();
                            sData19 = pSet.GetDouble("data19").ToString();
                            sData20 = pSet.GetDouble("data20").ToString();
                            sData21 = pSet.GetDouble("data21").ToString();
                        }
                        sSubData = pSet.GetDouble("SUBTOTALTAXES").ToString();

                        sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                        sSurcharge = pSet.GetDouble("sur_interest").ToString();
                        sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                        #region Total
                        if (m_iBatchCnt == 1)
                        {
                            dTNEW += Convert.ToDouble(sNew);
                            dTRenGross += Convert.ToDouble(sRenGross);
                            dTNewGross += Convert.ToDouble(sNewGross);

                            dTData1 += Convert.ToDouble(sData1);
                            dTData2 += Convert.ToDouble(sData2);
                            dTData3 += Convert.ToDouble(sData3);
                            dTData4 += Convert.ToDouble(sData4);
                            dTData5 += Convert.ToDouble(sData5);
                            dTData6 += Convert.ToDouble(sData6);
                            dTData7 += Convert.ToDouble(sData7);
                            dTData8 += Convert.ToDouble(sData8);
                            dTData9 += Convert.ToDouble(sData9);
                            dTData10 += Convert.ToDouble(sData10);
                            dTSubData += Convert.ToDouble(sSubData);
                        }
                        else
                        {
                            dTData11 += Convert.ToDouble(sData11);
                            dTData12 += Convert.ToDouble(sData12);
                            dTData13 += Convert.ToDouble(sData13);
                            dTData14 += Convert.ToDouble(sData14);
                            dTData15 += Convert.ToDouble(sData15);
                            dTData16 += Convert.ToDouble(sData16);
                            dTData17 += Convert.ToDouble(sData17);
                            dTData18 += Convert.ToDouble(sData18);
                            dTData19 += Convert.ToDouble(sData19);
                            dTData20 += Convert.ToDouble(sData20);
                            dTData21 += Convert.ToDouble(sData21);

                            dTTotalTax += Convert.ToDouble(sTotalTax);
                            dTSurcharge += Convert.ToDouble(sSurcharge);
                            dTTotalCollect += Convert.ToDouble(sTotalCollect);
                        }
                        #endregion

                        if (sTempDataGroup == "")
                        {
                            this.axVSPrinter1.FontBold = true;
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<4000;" + sTempDataGroup;
                            this.axVSPrinter1.FontBold = false;
                        }
                        else if (sDataGroup != sTempDataGroup)
                        {
                            this.axVSPrinter1.FontBold = true;
                            //1    2    3      4   5      6    7      8    9     10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    
                            if (m_iBatchCnt == 1)
                            {
                                this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                        + dNEW.ToString() + "|"
                                        + dRenGross.ToString("#,##0.00") + "|"
                                        + dNewGross.ToString("#,##0.00") + "|"
                                          + dData1.ToString("#,##0.00") + "|"
                                          + dData2.ToString("#,##0.00") + "|"
                                          + dData3.ToString("#,##0.00") + "|"
                                          + dData4.ToString("#,##0.00") + "|"
                                          + dData5.ToString("#,##0.00") + "|"
                                          + dData6.ToString("#,##0.00") + "|"
                                          + dData7.ToString("#,##0.00") + "|"
                                          + dData8.ToString("#,##0.00") + "|"
                                          + dData9.ToString("#,##0.00") + "|"
                                          + dData10.ToString("#,##0.00") + "|"
                                          + dSubData.ToString("#,##0.00");
                            }
                            else
                            {
                                this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL|"
                                      + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dData19.ToString("#,##0.00") + "|"
                                      + dData20.ToString("#,##0.00") + "|"
                                      + dData21.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");
                            }
                            dRenGross = 0;
                            dNEW = 0;
                            dNewGross = 0;
                            dData1 = 0;
                            dData2 = 0;
                            dData3 = 0;
                            dData4 = 0;
                            dData5 = 0;
                            dData6 = 0;
                            dData7 = 0;
                            dData8 = 0;
                            dData9 = 0;
                            dData10 = 0;
                            dData11 = 0;
                            dData12 = 0;
                            dData13 = 0;
                            dData14 = 0;
                            dData15 = 0;
                            dData16 = 0;
                            dData17 = 0;
                            dData18 = 0;
                            dData19 = 0;
                            dData20 = 0;
                            dData21 = 0;
                            dSubData = 0;

                            dTotalTax = 0;
                            dSurcharge = 0;
                            dTotalCollect = 0;

                            this.axVSPrinter1.Paragraph = "";
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<4000;" + sDataGroup;
                            this.axVSPrinter1.FontBold = false;

                        }

                        dNEW += Convert.ToDouble(sNew);
                        dRenGross += Convert.ToDouble(sRenGross);
                        dNewGross += Convert.ToDouble(sNewGross);

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            dData1 += Convert.ToDouble(sData1);
                            dData2 += Convert.ToDouble(sData2);
                            dData3 += Convert.ToDouble(sData3);
                            dData4 += Convert.ToDouble(sData4);
                            dData5 += Convert.ToDouble(sData5);
                            dData6 += Convert.ToDouble(sData6);
                            dData7 += Convert.ToDouble(sData7);
                            dData8 += Convert.ToDouble(sData8);
                            dData9 += Convert.ToDouble(sData9);
                            dData10 += Convert.ToDouble(sData10);
                        }
                        else
                        {
                            dData11 += Convert.ToDouble(sData11);
                            dData12 += Convert.ToDouble(sData12);
                            dData13 += Convert.ToDouble(sData13);
                            dData14 += Convert.ToDouble(sData14);
                            dData15 += Convert.ToDouble(sData15);
                            dData16 += Convert.ToDouble(sData16);
                            dData17 += Convert.ToDouble(sData17);
                            dData18 += Convert.ToDouble(sData18);
                            dData19 += Convert.ToDouble(sData19);
                            dData20 += Convert.ToDouble(sData20);
                            dData21 += Convert.ToDouble(sData21);
                        }
                        dSubData += Convert.ToDouble(sSubData);
                        dTotalTax += Convert.ToDouble(sTotalTax);
                        dSurcharge += Convert.ToDouble(sSurcharge);
                        dTotalCollect += Convert.ToDouble(sTotalCollect);
                        //this.axVSPrinter1.FontSize;


                        //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25    
                        if (m_iBatchCnt == 1)
                        {
                            this.axVSPrinter1.Table = @">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300;"
                              + sDataGroup2 + "|"
                              + sNew + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|" + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                                + Convert.ToDouble(sSubData).ToString("#,##0.00");
                        }
                        else
                        {
                            this.axVSPrinter1.Table = @">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;"
                            +sDataGroup2 + "|"
                          + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData19).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData20).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData21).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                        }
                    }
                }
                pSet.Close();


                if (m_iBatchCnt == 1)
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                        + dNEW.ToString() + "|"
                                        + dRenGross.ToString("#,##0.00") + "|"
                                        + dNewGross.ToString("#,##0.00") + "|"
                                          + dData1.ToString("#,##0.00") + "|"
                                          + dData2.ToString("#,##0.00") + "|"
                                          + dData3.ToString("#,##0.00") + "|"
                                          + dData4.ToString("#,##0.00") + "|"
                                          + dData5.ToString("#,##0.00") + "|"
                                          + dData6.ToString("#,##0.00") + "|"
                                          + dData7.ToString("#,##0.00") + "|"
                                          + dData8.ToString("#,##0.00") + "|"
                                          + dData9.ToString("#,##0.00") + "|"
                                          + dData10.ToString("#,##0.00") + "|"
                                          + dSubData.ToString("#,##0.00");

                    this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                      + dTNEW.ToString() + "|"
                      + dTRenGross.ToString("#,##0.00") + "|"
                      + dTNewGross.ToString("#,##0.00") + "|"
                      + dTData1.ToString("#,##0.00") + "|"
                      + dTData2.ToString("#,##0.00") + "|"
                      + dTData3.ToString("#,##0.00") + "|"
                      + dTData4.ToString("#,##0.00") + "|"
                      + dTData5.ToString("#,##0.00") + "|"
                      + dTData6.ToString("#,##0.00") + "|"
                      + dTData7.ToString("#,##0.00") + "|"
                      + dTData8.ToString("#,##0.00") + "|"
                      + dTData9.ToString("#,##0.00") + "|"
                      + dTData10.ToString("#,##0.00") + "|"
                    + dTSubData.ToString("#,##0.00");
                }
                else
                {
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL |"
                    +dData11.ToString("#,##0.00") + "|"
                    + dData12.ToString("#,##0.00") + "|"
                    + dData13.ToString("#,##0.00") + "|"
                    + dData14.ToString("#,##0.00") + "|"
                    + dData15.ToString("#,##0.00") + "|"
                    + dData16.ToString("#,##0.00") + "|"
                    + dData17.ToString("#,##0.00") + "|"
                    + dData18.ToString("#,##0.00") + "|"
                    + dData19.ToString("#,##0.00") + "|"
                    + dData20.ToString("#,##0.00") + "|"
                    + dData21.ToString("#,##0.00") + "|"
                    + dTotalTax.ToString("#,##0.00") + "|"
                    + dSurcharge.ToString("#,##0.00") + "|"
                    + dTotalCollect.ToString("#,##0.00");

                    this.axVSPrinter1.Paragraph = "";

                    //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    
                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;TOTAL|"
                    +dTData11.ToString("#,##0.00") + "|"
                    + dTData12.ToString("#,##0.00") + "|"
                    + dTData13.ToString("#,##0.00") + "|"
                    + dTData14.ToString("#,##0.00") + "|"
                    + dTData15.ToString("#,##0.00") + "|"
                    + dTData16.ToString("#,##0.00") + "|"
                    + dTData17.ToString("#,##0.00") + "|"
                    + dTData18.ToString("#,##0.00") + "|"
                    + dTData19.ToString("#,##0.00") + "|"
                    + dTData20.ToString("#,##0.00") + "|"
                    + dTData21.ToString("#,##0.00") + "|"
                    + dTTotalTax.ToString("#,##0.00") + "|"
                    + dTSurcharge.ToString("#,##0.00") + "|"
                    + dTTotalCollect.ToString("#,##0.00");
                }
            }
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }
        
        private void OnReportStanSimpSummaryOfCollectionMainBusiness()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sNew, sRenGross, sNewGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12,sData13, sTotalTax, sSurcharge, sTotalCollect;
            double dNEW = 0, dRenGross = 0, dNewGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTNEW = 0, dTRenGross = 0, dTNewGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;

            sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+rs.sur_interest) as TOTALCOLLECTION";
            sQuery += " FROM REPORT_SUMCOLL RS";
            sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
            sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
            sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                    sDataGroup = pSet.GetString("data_group1");
                    sDataGroup2 = pSet.GetString("data_group2");

                    sNew = pSet.GetInt("new_no").ToString();
                    sRenGross = pSet.GetDouble("ren_gross").ToString();
                    sNewGross = pSet.GetDouble("new_cap").ToString();

                    sData1 = pSet.GetDouble("data1").ToString();
                    sData2 = pSet.GetDouble("data2").ToString();
                    sData3 = pSet.GetDouble("data3").ToString();
                    sData4 = pSet.GetDouble("data4").ToString();
                    sData5 = pSet.GetDouble("data5").ToString();
                    sData6 = pSet.GetDouble("data6").ToString();
                    sData7 = pSet.GetDouble("data7").ToString();
                    sData8 = pSet.GetDouble("data8").ToString();
                    sData9 = pSet.GetDouble("data9").ToString();
                    sData10 = pSet.GetDouble("data10").ToString();
                    sData11 = pSet.GetDouble("data11").ToString();
                    sData12 = pSet.GetDouble("data12").ToString();
                    sData13 = pSet.GetDouble("data13").ToString();

                    sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                    sSurcharge = pSet.GetDouble("sur_interest").ToString();
                    sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                    #region Total
                    dTNEW += Convert.ToDouble(sNew);
                    dTRenGross += Convert.ToDouble(sRenGross);
                    dTNewGross += Convert.ToDouble(sNewGross);

                    dTData1 += Convert.ToDouble(sData1);
                    dTData2 += Convert.ToDouble(sData2);
                    dTData3 += Convert.ToDouble(sData3);
                    dTData4 += Convert.ToDouble(sData4);
                    dTData5 += Convert.ToDouble(sData5);
                    dTData6 += Convert.ToDouble(sData6);
                    dTData7 += Convert.ToDouble(sData7);
                    dTData8 += Convert.ToDouble(sData8);
                    dTData9 += Convert.ToDouble(sData9);
                    dTData10 += Convert.ToDouble(sData10);
                    dTData11 += Convert.ToDouble(sData11);
                    dTData12 += Convert.ToDouble(sData12);
                    dTData13 += Convert.ToDouble(sData13);

                    dTTotalTax += Convert.ToDouble(sTotalTax);
                    dTSurcharge += Convert.ToDouble(sSurcharge);
                    dTTotalCollect += Convert.ToDouble(sTotalCollect);
                    #endregion

                    if (sTempDataGroup == "")
                    {
                        sTempDataGroup = sDataGroup;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                                                     //1   2    3     4     5      6    7      8    9    10     11    12    13    14    15   16    17   18     19   20
                        this.axVSPrinter1.Table = "<4000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                                + sTempDataGroup + "|"
                                + dNEW.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

                        this.axVSPrinter1.Paragraph = "";

                        dNEW = 0;
                        dRenGross = 0;
                        dNewGross = 0;
                        dData1 = 0;
                        dData2 = 0;
                        dData3 = 0;
                        dData4 = 0;
                        dData5 = 0;
                        dData6 = 0;
                        dData7 = 0;
                        dData8 = 0;
                        dData9 = 0;
                        dData10 = 0;
                        dData11 = 0;
                        dData12 = 0;
                        dData13 = 0;
                        dTotalTax = 0;
                        dSurcharge = 0;
                        dTotalCollect = 0;

                        sTempDataGroup = sDataGroup;
                    }

                    dNEW += Convert.ToDouble(sNew);
                    dRenGross += Convert.ToDouble(sRenGross);
                    dNewGross += Convert.ToDouble(sNewGross);

                    dData1 += Convert.ToDouble(sData1);
                    dData2 += Convert.ToDouble(sData2);
                    dData3 += Convert.ToDouble(sData3);
                    dData4 += Convert.ToDouble(sData4);
                    dData5 += Convert.ToDouble(sData5);
                    dData6 += Convert.ToDouble(sData6);
                    dData7 += Convert.ToDouble(sData7);
                    dData8 += Convert.ToDouble(sData8);
                    dData9 += Convert.ToDouble(sData9);
                    dData10 += Convert.ToDouble(sData10);
                    dData11 += Convert.ToDouble(sData11);
                    dData12 += Convert.ToDouble(sData12);
                    dData13 += Convert.ToDouble(sData13);

                    dTotalTax += Convert.ToDouble(sTotalTax);
                    dSurcharge += Convert.ToDouble(sSurcharge);
                    dTotalCollect += Convert.ToDouble(sTotalCollect);
                }
                pSet.Close();
            }
            //last record
            this.axVSPrinter1.Table = "<4000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                                + sTempDataGroup + "|"
                                + dNEW.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
                                        //1    2    3      4    5      6    7      8    9    10     11    12    13    14    15   16    17   18     19     20
            this.axVSPrinter1.Table = "<4000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                      + dTNEW.ToString() + "|"
                      + dTRenGross.ToString("#,##0.00") + "|"
                      + dTNewGross.ToString("#,##0.00") + "|"
                      + dTData1.ToString("#,##0.00") + "|"
                      + dTData2.ToString("#,##0.00") + "|"
                      + dTData3.ToString("#,##0.00") + "|"
                      + dTData4.ToString("#,##0.00") + "|"
                      + dTData5.ToString("#,##0.00") + "|"
                      + dTData6.ToString("#,##0.00") + "|"
                      + dTData7.ToString("#,##0.00") + "|"
                      + dTData8.ToString("#,##0.00") + "|"
                      + dTData9.ToString("#,##0.00") + "|"
                      + dTData10.ToString("#,##0.00") + "|"
                      + dTData11.ToString("#,##0.00") + "|"
                      + dTData12.ToString("#,##0.00") + "|"
                      + dTData13.ToString("#,##0.00") + "|"
                      + dTTotalTax.ToString("#,##0.00") + "|"
                      + dTSurcharge.ToString("#,##0.00") + "|"
                      + dTTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        private void OnReportStanWideSummaryOfCollectionMainBusiness()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sNew, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sTotalTax, sSurcharge, sTotalCollect;
            double dNEW = 0, dRenGross = 0, dNewGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTNEW = 0, dTRenGross = 0, dTNewGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;

            sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+rs.sur_interest) as TOTALCOLLECTION";
            sQuery += " FROM REPORT_SUMCOLL RS";
            sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
            sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
            sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                    sDataGroup = pSet.GetString("data_group1");
                    sDataGroup2 = pSet.GetString("data_group2");
                    
                    sNew = pSet.GetInt("new_no").ToString();
                    sRenGross = pSet.GetDouble("ren_gross").ToString();
                    sNewGross = pSet.GetDouble("new_cap").ToString();

                    sData1 = pSet.GetDouble("data1").ToString();
                    sData2 = pSet.GetDouble("data2").ToString();
                    sData3 = pSet.GetDouble("data3").ToString();
                    sData4 = pSet.GetDouble("data4").ToString();
                    sData5 = pSet.GetDouble("data5").ToString();
                    sData6 = pSet.GetDouble("data6").ToString();
                    sData7 = pSet.GetDouble("data7").ToString();
                    sData8 = pSet.GetDouble("data8").ToString();
                    sData9 = pSet.GetDouble("data9").ToString();
                    sData10 = pSet.GetDouble("data10").ToString();
                    sData11 = pSet.GetDouble("data11").ToString();
                    sData12 = pSet.GetDouble("data12").ToString();
                    sData13 = pSet.GetDouble("data13").ToString();
                    sData14 = pSet.GetDouble("data14").ToString();
                    sData15 = pSet.GetDouble("data15").ToString();
                    sData16 = pSet.GetDouble("data16").ToString();

                    sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                    sSurcharge = pSet.GetDouble("sur_interest").ToString();
                    sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                    #region Total
                    dTNEW += Convert.ToDouble(sNew);
                    dTRenGross += Convert.ToDouble(sRenGross);
                    dTNewGross += Convert.ToDouble(sNewGross);

                    dTData1 += Convert.ToDouble(sData1);
                    dTData2 += Convert.ToDouble(sData2);
                    dTData3 += Convert.ToDouble(sData3);
                    dTData4 += Convert.ToDouble(sData4);
                    dTData5 += Convert.ToDouble(sData5);
                    dTData6 += Convert.ToDouble(sData6);
                    dTData7 += Convert.ToDouble(sData7);
                    dTData8 += Convert.ToDouble(sData8);
                    dTData9 += Convert.ToDouble(sData9);
                    dTData10 += Convert.ToDouble(sData10);
                    dTData11 += Convert.ToDouble(sData11);
                    dTData12 += Convert.ToDouble(sData12);
                    dTData13 += Convert.ToDouble(sData13);
                    dTData14 += Convert.ToDouble(sData14);
                    dTData15 += Convert.ToDouble(sData15);
                    dTData16 += Convert.ToDouble(sData16);

                    dTTotalTax += Convert.ToDouble(sTotalTax);
                    dTSurcharge += Convert.ToDouble(sSurcharge);
                    dTTotalCollect += Convert.ToDouble(sTotalCollect);
                    #endregion

                    if (sTempDataGroup == "")
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<4000;" + sTempDataGroup;
                        this.axVSPrinter1.FontBold = false;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                        this.axVSPrinter1.FontBold = true;
                                                    //1    2    3      4   5      6    7      8    9     10     11    12    13    14    15   16    17   18     19    20 
                        this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                + dNEW.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

                        dRenGross = 0;
                        dNEW = 0;
                        dNewGross = 0;
                        dData1 = 0;
                        dData2 = 0;
                        dData3 = 0;
                        dData4 = 0;
                        dData5 = 0;
                        dData6 = 0;
                        dData7 = 0;
                        dData8 = 0;
                        dData9 = 0;
                        dData10 = 0;
                        dData11 = 0;
                        dData12 = 0;
                        dData13 = 0;
                        dTotalTax = 0;
                        dSurcharge = 0;
                        dTotalCollect = 0;

                        this.axVSPrinter1.Paragraph = "";
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<4000;" + sDataGroup;
                        this.axVSPrinter1.FontBold = false;

                    }

                    dNEW += Convert.ToDouble(sNew);
                    dRenGross += Convert.ToDouble(sRenGross);
                    dNewGross += Convert.ToDouble(sNewGross);

                    dData1 += Convert.ToDouble(sData1);
                    dData2 += Convert.ToDouble(sData2);
                    dData3 += Convert.ToDouble(sData3);
                    dData4 += Convert.ToDouble(sData4);
                    dData5 += Convert.ToDouble(sData5);
                    dData6 += Convert.ToDouble(sData6);
                    dData7 += Convert.ToDouble(sData7);
                    dData8 += Convert.ToDouble(sData8);
                    dData9 += Convert.ToDouble(sData9);
                    dData10 += Convert.ToDouble(sData10);
                    dData11 += Convert.ToDouble(sData11);
                    dData12 += Convert.ToDouble(sData12);
                    dData13 += Convert.ToDouble(sData13);

                    dTotalTax += Convert.ToDouble(sTotalTax);
                    dSurcharge += Convert.ToDouble(sSurcharge);
                    dTotalCollect += Convert.ToDouble(sTotalCollect);
                    //this.axVSPrinter1.FontSize;


                                                 //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16     17   18     19    20   
                    this.axVSPrinter1.Table = @"^2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                      + sDataGroup2 + "|"
                      + sNew + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|"  + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                }
                pSet.Close();
            }

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                + dNEW.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";
                                        //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    
            this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                      + dTNEW.ToString() + "|"
                      + dTRenGross.ToString("#,##0.00") + "|"
                      + dTNewGross.ToString("#,##0.00") + "|"
                      + dTData1.ToString("#,##0.00") + "|"
                      + dTData2.ToString("#,##0.00") + "|"
                      + dTData3.ToString("#,##0.00") + "|"
                      + dTData4.ToString("#,##0.00") + "|"
                      + dTData5.ToString("#,##0.00") + "|"
                      + dTData6.ToString("#,##0.00") + "|"
                      + dTData7.ToString("#,##0.00") + "|"
                      + dTData8.ToString("#,##0.00") + "|"
                      + dTData9.ToString("#,##0.00") + "|"
                      + dTData10.ToString("#,##0.00") + "|"
                      + dTData11.ToString("#,##0.00") + "|"
                      + dTData12.ToString("#,##0.00") + "|"
                      + dTData13.ToString("#,##0.00") + "|"
                      + dTTotalTax.ToString("#,##0.00") + "|"
                      + dTSurcharge.ToString("#,##0.00") + "|"
                      + dTTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        //AFM 20190828 MAO-19-10709 (s)
        private void OnReportTotalCollections()
        {
            String sQuery;
            String sCurrentUser;
            string fFees, fSurch, fPenalty, fFeesAmtDue, fTotalAmt;
            double dFees, dSurch, dPenalty, dFeesAmtDue, dTotalAmt;
            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            fFees = string.Empty;
            fSurch = string.Empty;
            fPenalty = string.Empty;
            fFeesAmtDue = string.Empty;
            fTotalAmt = string.Empty;

            OracleResultSet pSet = new OracleResultSet();

            //January
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%JAN%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 01 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;January|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //February
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%FEB%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 02 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;February|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //March
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%MAR%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 03 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;March|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //1st qtr subtotal
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and (OU.TRN_DATE LIKE '%JAN%' OR OU.TRN_DATE LIKE '%FEB%' OR OU.TRN_DATE LIKE '%MAR%')";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) between 01 and 03 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;Sub Total|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");
            this.axVSPrinter1.FontBold = false;

            //April
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%APR%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 04 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;April|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //May
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%MAY%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 05 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;May|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //June
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%JUN%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 06 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;June|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //2nd qtr subtotal
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and (OU.TRN_DATE LIKE '%APR%' OR OU.TRN_DATE LIKE '%MAY%' OR OU.TRN_DATE LIKE '%JUN%')";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) between 04 and 06 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;Sub Total|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");
            this.axVSPrinter1.FontBold = false;

            //July
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%JUL%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 07 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;July|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //August
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%AUG%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 08 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;August|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //September
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%SEP%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 09 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;September|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //3rd qtr subtotal
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and (OU.TRN_DATE LIKE '%JUL%' OR OU.TRN_DATE LIKE '%AUG%' OR OU.TRN_DATE LIKE '%SEP%')";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) between 07 and 09 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;Sub Total|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");
            this.axVSPrinter1.FontBold = false;

            //October
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%OCT%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 10 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;October|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //November
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%NOV%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 11 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;November|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //December
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and OU.TRN_DATE LIKE '%DEC%'";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) = 12 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;December|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");

            //4th qtr subtotal
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and (OU.TRN_DATE LIKE '%OCT%' OR OU.TRN_DATE LIKE '%NOV%' OR OU.TRN_DATE LIKE '%DEC%')";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and extract (month from PH.or_date) between 10 and 12 and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;Sub Total|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");
            this.axVSPrinter1.FontBold = false;

            //Grand Total
            //pSet.Query = "select SUM(OT.FEES_DUE) AS FEESDUE, SUM(OT.FEES_SURCH) AS SURCH, SUM(OT.FEES_PEN) AS PEN, SUM(OT.FEES_AMTDUE) AS AMT from or_table OT INNER JOIN OR_USED OU ON OU.OR_NO = OT.OR_NO";
            //AFM 20191028 fixed query
            //pSet.Query = "select SUM(coalesce(OT.FEES_DUE,0)) AS FEESDUE, SUM(coalesce(OT.FEES_SURCH,0)) AS SURCH, SUM(coalesce(OT.FEES_PEN,0)) AS PEN, SUM(coalesce(OT.FEES_AMTDUE,0)) AS AMT from or_table OT INNER JOIN OR_USED OU ON to_char(OU.OR_NO) = OT.OR_NO";
            //pSet.Query += string.Format(" where tax_year = '{0}'", sTaxYear);
            //pSet.Query += " and (OU.TRN_DATE LIKE '%JAN%' OR OU.TRN_DATE LIKE '%FEB%' OR OU.TRN_DATE LIKE '%MAR%' OR OU.TRN_DATE LIKE '%APR%' OR OU.TRN_DATE LIKE '%MAY%' OR OU.TRN_DATE LIKE '%JUN%' OR OU.TRN_DATE LIKE '%JUL%' OR OU.TRN_DATE LIKE '%AUG%' OR OU.TRN_DATE LIKE '%SEP%' OR OU.TRN_DATE LIKE '%OCT%' OR OU.TRN_DATE LIKE '%NOV%' OR OU.TRN_DATE LIKE '%DEC%')";
            //AFM 20191120 modified query
            pSet.Query = "select sum(fees_due) as FEESDUE,sum(fees_surch) as SURCH,sum(fees_pen) as PEN,sum(fees_amtdue) as AMT from or_table";
            pSet.Query += " where or_no in (select or_No from pay_hist PH";
            pSet.Query += string.Format(" where  extract (year from PH.or_date) = '{0}' and PH.or_no not in (select or_no from cancelled_payment) and PH.data_mode != 'POS')", sTaxYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    fFees = pSet.GetDouble("FEESDUE").ToString();
                    fSurch = pSet.GetDouble("SURCH").ToString();
                    fPenalty = pSet.GetDouble("PEN").ToString();
                    fFeesAmtDue = pSet.GetDouble("AMT").ToString();
                }
            dFees = Convert.ToDouble(fFees);
            dSurch = Convert.ToDouble(fSurch);
            dPenalty = Convert.ToDouble(fPenalty);
            dFeesAmtDue = Convert.ToDouble(fFeesAmtDue);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 12;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.Table = "<2000|>2200|>2200|>2200|>2200;Grand Total|"
                      + dFees.ToString("#,##0.00") + "|"
                      + dSurch.ToString("#,##0.00") + "|"
                      + dPenalty.ToString("#,##0.00") + "|"
                      + dFeesAmtDue.ToString("#,##0.00");
            this.axVSPrinter1.FontBold = false;




        }
        //AFM 20190828 MAO-19-10709 (e)


        //Org Kind
        private void OnReportWideSummaryOfCollectionOrgnKind()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sRen, sNew, sRet, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sData17, sData18, sTotalTax, sSurcharge, sTotalCollect;
            double dREN = 0, dNEW = 0, dRET = 0, dRenGross = 0, dNewGross = 0, dRetGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dData17 = 0, dData18 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTREN = 0, dTNEW = 0, dTRET = 0, dTRenGross = 0, dTNewGross = 0, dTRetGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTData17 = 0, dTData18 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;
            double dSubData = 0, dData19 = 0, dData20 = 0, dData21 = 0, dTData19 = 0, dTData20 = 0, dTData21 = 0, dTSubData = 0;
            String sData19, sData20, sData21, sSubData; 

            // RMC 20150522 corrections in reports (s)
            for (int i = 1; i <= 2; i++)
            {
                //dTData11 = 0; dTData12 = 0; dTData13 = 0; dTData14 = 0; dTData15 = 0; dTData16 = 0; dTData17 = 0; dTData18 = 0; // RMC 20150520 corrections in reports
                // RMC 20150522 corrections in reports (s)
                sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                dTotalTax = 0; dSurcharge = 0; dTotalCollect = 0;
                // RMC 20150522 corrections in reports (e)

                m_iBatchCnt = i;

                sTempDataGroup = "";

                if (m_iBatchCnt == 2)
                    this.axVSPrinter1.NewPage();

                // RMC 20150522 corrections in reports (e)
                sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16,RS.data17, RS.data18,RS.data19,RS.data20, RS.data21, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21+rs.sur_interest) as TOTALCOLLECTION";
                sQuery += ", sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10) as SUBTOTALTAXES"; // RMC 20150522 corrections in reports
                sQuery += " FROM REPORT_SUMCOLL RS";
                sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
                sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.data17, RS.data18, RS.data19,RS.data20,RS.data21,RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        // RMC 20150522 corrections in reports (s)
                        sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                        sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                        sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                        sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                        sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                        // RMC 20150522 corrections in reports (e)

                        sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                        sDataGroup = pSet.GetString("data_group1");
                        sDataGroup2 = pSet.GetString("data_group2");

                        sNew = pSet.GetInt("new_no").ToString();
                        sRenGross = pSet.GetDouble("ren_gross").ToString();
                        sNewGross = pSet.GetDouble("new_cap").ToString();

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            sData1 = pSet.GetDouble("data1").ToString();
                            sData2 = pSet.GetDouble("data2").ToString();
                            sData3 = pSet.GetDouble("data3").ToString();
                            sData4 = pSet.GetDouble("data4").ToString();
                            sData5 = pSet.GetDouble("data5").ToString();
                            sData6 = pSet.GetDouble("data6").ToString();
                            sData7 = pSet.GetDouble("data7").ToString();
                            sData8 = pSet.GetDouble("data8").ToString();
                            sData9 = pSet.GetDouble("data9").ToString();
                            sData10 = pSet.GetDouble("data10").ToString();
                        }
                        else// RMC 20150522 corrections in reports
                        {
                            sData11 = pSet.GetDouble("data11").ToString();
                            sData12 = pSet.GetDouble("data12").ToString();
                            sData13 = pSet.GetDouble("data13").ToString();
                            sData14 = pSet.GetDouble("data14").ToString();
                            sData15 = pSet.GetDouble("data15").ToString();
                            sData16 = pSet.GetDouble("data16").ToString();
                            sData17 = pSet.GetDouble("data17").ToString();
                            sData18 = pSet.GetDouble("data18").ToString();
                            sData19 = pSet.GetDouble("data19").ToString();
                            sData20 = pSet.GetDouble("data20").ToString();
                            sData21 = pSet.GetDouble("data21").ToString();
                        }

                        sSubData = pSet.GetDouble("SUBTOTALTAXES").ToString();

                        sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                        sSurcharge = pSet.GetDouble("sur_interest").ToString();
                        sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                        #region Total
                        if (m_iBatchCnt == 1)
                        {
                            dTNEW += Convert.ToDouble(sNew);
                            dTRenGross += Convert.ToDouble(sRenGross);
                            dTNewGross += Convert.ToDouble(sNewGross);

                            dTData1 += Convert.ToDouble(sData1);
                            dTData2 += Convert.ToDouble(sData2);
                            dTData3 += Convert.ToDouble(sData3);
                            dTData4 += Convert.ToDouble(sData4);
                            dTData5 += Convert.ToDouble(sData5);
                            dTData6 += Convert.ToDouble(sData6);
                            dTData7 += Convert.ToDouble(sData7);
                            dTData8 += Convert.ToDouble(sData8);
                            dTData9 += Convert.ToDouble(sData9);
                            dTData10 += Convert.ToDouble(sData10);
                            dTSubData += Convert.ToDouble(sSubData);
                        }
                        else
                        {
                            dTData11 += Convert.ToDouble(sData11);
                            dTData12 += Convert.ToDouble(sData12);
                            dTData13 += Convert.ToDouble(sData13);
                            dTData14 += Convert.ToDouble(sData14);
                            dTData15 += Convert.ToDouble(sData15);
                            dTData16 += Convert.ToDouble(sData16);
                            dTData17 += Convert.ToDouble(sData17);
                            dTData18 += Convert.ToDouble(sData18);
                            dTData19 += Convert.ToDouble(sData19);
                            dTData20 += Convert.ToDouble(sData20);
                            dTData21 += Convert.ToDouble(sData21);

                            dTTotalTax += Convert.ToDouble(sTotalTax);
                            dTSurcharge += Convert.ToDouble(sSurcharge);
                            dTTotalCollect += Convert.ToDouble(sTotalCollect);
                        }

                        #endregion

                        if (sTempDataGroup == "")
                        {
                            this.axVSPrinter1.FontBold = true;
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<4000;" + sTempDataGroup;
                            this.axVSPrinter1.Paragraph = "";
                            this.axVSPrinter1.FontBold = false;
                        }
                        else if (sDataGroup != sTempDataGroup)
                        {
                            this.axVSPrinter1.FontBold = true;
                            //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
                            /*this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                    + dNEW.ToString() + "|"
                                    + dRenGross.ToString("#,##0.00") + "|"
                                    + dNewGross.ToString("#,##0.00") + "|"
                                      + dData1.ToString("#,##0.00") + "|"
                                      + dData2.ToString("#,##0.00") + "|"
                                      + dData3.ToString("#,##0.00") + "|"
                                      + dData4.ToString("#,##0.00") + "|"
                                      + dData5.ToString("#,##0.00") + "|"
                                      + dData6.ToString("#,##0.00") + "|"
                                      + dData7.ToString("#,##0.00") + "|"
                                      + dData8.ToString("#,##0.00") + "|"
                                      + dData9.ToString("#,##0.00") + "|"
                                      + dData10.ToString("#,##0.00") + "|"
                                      + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");
                            */

                            if (m_iBatchCnt == 1)
                            {
                                //this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"   // RMC 20150522 corrections in reports
                                    + dNEW.ToString() + "|"
                                    + dRenGross.ToString("#,##0.00") + "|"
                                    + dNewGross.ToString("#,##0.00") + "|"
                                      + dData1.ToString("#,##0.00") + "|"
                                      + dData2.ToString("#,##0.00") + "|"
                                      + dData3.ToString("#,##0.00") + "|"
                                      + dData4.ToString("#,##0.00") + "|"
                                      + dData5.ToString("#,##0.00") + "|"
                                      + dData6.ToString("#,##0.00") + "|"
                                      + dData7.ToString("#,##0.00") + "|"
                                      + dData8.ToString("#,##0.00") + "|"
                                      + dData9.ToString("#,##0.00") + "|"
                                      + dData10.ToString("#,##0.00") + "|"
                                      + dSubData.ToString("#,##0.00");
                            }
                            else
                            {
                                this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL|"
                                    + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dData19.ToString("#,##0.00") + "|"
                                      + dData20.ToString("#,##0.00") + "|"
                                      + dData21.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");
                            }

                            // RMC 20150126 (E)
                            dNEW = 0;
                            dRenGross = 0;
                            dNewGross = 0;
                            dData1 = 0;
                            dData2 = 0;
                            dData3 = 0;
                            dData4 = 0;
                            dData5 = 0;
                            dData6 = 0;
                            dData7 = 0;
                            dData8 = 0;
                            dData9 = 0;
                            dData10 = 0;
                            dData11 = 0;
                            dData12 = 0;
                            dData13 = 0;
                            dData14 = 0;
                            dData15 = 0;
                            dData16 = 0;
                            dData17 = 0;
                            dData18 = 0;
                            dData19 = 0;
                            dData20 = 0;
                            dData21 = 0;
                            dTotalTax = 0;
                            dSurcharge = 0;
                            dTotalCollect = 0;
                            dSubData = 0;

                            this.axVSPrinter1.Paragraph = "";
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<4000;" + sDataGroup;
                            this.axVSPrinter1.FontBold = false;
                            this.axVSPrinter1.Paragraph = "";
                        }

                        dNEW += Convert.ToDouble(sNew);
                        dRenGross += Convert.ToDouble(sRenGross);
                        dNewGross += Convert.ToDouble(sNewGross);

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            dData1 += Convert.ToDouble(sData1);
                            dData2 += Convert.ToDouble(sData2);
                            dData3 += Convert.ToDouble(sData3);
                            dData4 += Convert.ToDouble(sData4);
                            dData5 += Convert.ToDouble(sData5);
                            dData6 += Convert.ToDouble(sData6);
                            dData7 += Convert.ToDouble(sData7);
                            dData8 += Convert.ToDouble(sData8);
                            dData9 += Convert.ToDouble(sData9);
                            dData10 += Convert.ToDouble(sData10);
                            dSubData += Convert.ToDouble(sSubData);
                        }
                        else
                        {
                            dData11 += Convert.ToDouble(sData11);
                            dData12 += Convert.ToDouble(sData12);
                            dData13 += Convert.ToDouble(sData13);
                            dData14 += Convert.ToDouble(sData14);
                            dData15 += Convert.ToDouble(sData15);
                            dData16 += Convert.ToDouble(sData16);
                            dData17 += Convert.ToDouble(sData17);
                            dData18 += Convert.ToDouble(sData18);
                            dData19 += Convert.ToDouble(sData19);
                            dData20 += Convert.ToDouble(sData20);
                            dData21 += Convert.ToDouble(sData21);
                            dTotalTax += Convert.ToDouble(sTotalTax);
                            dSurcharge += Convert.ToDouble(sSurcharge);
                            dTotalCollect += Convert.ToDouble(sTotalCollect);
                        }
                        //this.axVSPrinter1.FontSize;

                        //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25    26    27    28
                        /*this.axVSPrinter1.Table = @"^2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                          + sDataGroup2 + "|"
                          + sNew + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|"  + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                        */

                        if (m_iBatchCnt == 1)
                        {
                            this.axVSPrinter1.Table = @"^2000|>500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                          + sDataGroup2 + "|"
                          + sNew + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|" + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSubData).ToString("#,##0.00");
                        }
                        else
                        {
                            this.axVSPrinter1.Table = @"^2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;"
                          + sDataGroup2 + "|"
                          + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData19).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData20).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData21).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                        }
                        this.axVSPrinter1.Paragraph = "";
                    }
                    pSet.Close();
                }

                this.axVSPrinter1.FontBold = true;
                if (m_iBatchCnt == 1)
                {
                    this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                        + dNEW.ToString() + "|"
                                        + dRenGross.ToString("#,##0.00") + "|"
                                        + dNewGross.ToString("#,##0.00") + "|"
                                          + dData1.ToString("#,##0.00") + "|"
                                          + dData2.ToString("#,##0.00") + "|"
                                          + dData3.ToString("#,##0.00") + "|"
                                          + dData4.ToString("#,##0.00") + "|"
                                          + dData5.ToString("#,##0.00") + "|"
                                          + dData6.ToString("#,##0.00") + "|"
                                          + dData7.ToString("#,##0.00") + "|"
                                          + dData8.ToString("#,##0.00") + "|"
                                          + dData9.ToString("#,##0.00") + "|"
                                          + dData10.ToString("#,##0.00") + "|"
                                          + dSubData.ToString("#,##0.00");
                    this.axVSPrinter1.Paragraph = "";
                }
                else
                {
                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL |"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dData16.ToString("#,##0.00") + "|"
                                  + dData17.ToString("#,##0.00") + "|"
                                  + dData18.ToString("#,##0.00") + "|"
                                  + dData19.ToString("#,##0.00") + "|"
                                  + dData20.ToString("#,##0.00") + "|"
                                  + dData21.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");
                }

                this.axVSPrinter1.Paragraph = "";

                //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    
                /*this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                          + dTNEW.ToString() + "|"
                          + dTRenGross.ToString("#,##0.00") + "|"
                          + dTNewGross.ToString("#,##0.00") + "|"
                          + dTData1.ToString("#,##0.00") + "|"
                          + dTData2.ToString("#,##0.00") + "|"
                          + dTData3.ToString("#,##0.00") + "|"
                          + dTData4.ToString("#,##0.00") + "|"
                          + dTData5.ToString("#,##0.00") + "|"
                          + dTData6.ToString("#,##0.00") + "|"
                          + dTData7.ToString("#,##0.00") + "|"
                          + dTData8.ToString("#,##0.00") + "|"
                          + dTData9.ToString("#,##0.00") + "|"
                          + dTData10.ToString("#,##0.00") + "|"
                          + dTData11.ToString("#,##0.00") + "|"
                          + dTData12.ToString("#,##0.00") + "|"
                          + dTData13.ToString("#,##0.00") + "|"
                          + dTData14.ToString("#,##0.00") + "|"
                          + dTData15.ToString("#,##0.00") + "|"
                          + dTData16.ToString("#,##0.00") + "|"
                          + dTData17.ToString("#,##0.00") + "|"
                          + dTData18.ToString("#,##0.00") + "|"
                          + dTTotalTax.ToString("#,##0.00") + "|"
                          + dTSurcharge.ToString("#,##0.00") + "|"
                          + dTTotalCollect.ToString("#,##0.00");*/

                if (m_iBatchCnt == 1)
                {
                    this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                      + dTNEW.ToString() + "|"
                      + dTRenGross.ToString("#,##0.00") + "|"
                      + dTNewGross.ToString("#,##0.00") + "|"
                      + dTData1.ToString("#,##0.00") + "|"
                      + dTData2.ToString("#,##0.00") + "|"
                      + dTData3.ToString("#,##0.00") + "|"
                      + dTData4.ToString("#,##0.00") + "|"
                      + dTData5.ToString("#,##0.00") + "|"
                      + dTData6.ToString("#,##0.00") + "|"
                      + dTData7.ToString("#,##0.00") + "|"
                      + dTData8.ToString("#,##0.00") + "|"
                      + dTData9.ToString("#,##0.00") + "|"
                      + dTData10.ToString("#,##0.00") + "|"
                      + dTSubData.ToString("#,##0.00");
                }
                else
                {
                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200; TOTAL |"
                    + dTData11.ToString("#,##0.00") + "|"
                      + dTData12.ToString("#,##0.00") + "|"
                      + dTData13.ToString("#,##0.00") + "|"
                      + dTData14.ToString("#,##0.00") + "|"
                      + dTData15.ToString("#,##0.00") + "|"
                      + dTData16.ToString("#,##0.00") + "|"
                      + dTData17.ToString("#,##0.00") + "|"
                      + dTData18.ToString("#,##0.00") + "|"
                      + dTData19.ToString("#,##0.00") + "|"
                      + dTData20.ToString("#,##0.00") + "|"
                      + dTData21.ToString("#,##0.00") + "|"
                      + dTTotalTax.ToString("#,##0.00") + "|"
                      + dTSurcharge.ToString("#,##0.00") + "|"
                      + dTTotalCollect.ToString("#,##0.00");
                }
            }

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        private void OnReportStanWideSummaryOfCollectionOrgnKind()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sRen, sNew, sRet, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sData17, sData18, sTotalTax, sSurcharge, sTotalCollect;
            double dREN = 0, dNEW = 0, dRET = 0, dRenGross = 0, dNewGross = 0, dRetGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dData17 = 0, dData18 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTREN = 0, dTNEW = 0, dTRET = 0, dTRenGross = 0, dTNewGross = 0, dTRetGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTData17 = 0, dTData18 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;

            sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16,RS.data17, RS.data18, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+rs.sur_interest) as TOTALCOLLECTION";
            sQuery += " FROM REPORT_SUMCOLL RS";
            sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
            sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
            sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.data17, RS.data18, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                    sDataGroup = pSet.GetString("data_group1");
                    sDataGroup2 = pSet.GetString("data_group2");

                    sRen = pSet.GetInt("ren_no").ToString();
                    sRenGross = pSet.GetDouble("ren_gross").ToString();
                    sNew = pSet.GetInt("new_no").ToString();
                    sNewGross = pSet.GetDouble("new_cap").ToString();
                    sRet = pSet.GetInt("ret_no").ToString();
                    sRetGross = pSet.GetDouble("ret_gross").ToString();

                    sData1 = pSet.GetDouble("data1").ToString();
                    sData2 = pSet.GetDouble("data2").ToString();
                    sData3 = pSet.GetDouble("data3").ToString();
                    sData4 = pSet.GetDouble("data4").ToString();
                    sData5 = pSet.GetDouble("data5").ToString();
                    sData6 = pSet.GetDouble("data6").ToString();
                    sData7 = pSet.GetDouble("data7").ToString();
                    sData8 = pSet.GetDouble("data8").ToString();
                    sData9 = pSet.GetDouble("data9").ToString();
                    sData10 = pSet.GetDouble("data10").ToString();
                    sData11 = pSet.GetDouble("data11").ToString();
                    sData12 = pSet.GetDouble("data12").ToString();
                    sData13 = pSet.GetDouble("data13").ToString();
                    sData14 = pSet.GetDouble("data14").ToString();
                    sData15 = pSet.GetDouble("data15").ToString();
                    sData16 = pSet.GetDouble("data16").ToString();
                    sData17 = pSet.GetDouble("data17").ToString();
                    sData18 = pSet.GetDouble("data18").ToString();

                    sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                    sSurcharge = pSet.GetDouble("sur_interest").ToString();
                    sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                    #region Total
                    dTREN += Convert.ToDouble(sRen);
                    dTNEW += Convert.ToDouble(sNew);
                    dTRET += Convert.ToDouble(sRet);
                    dTRenGross += Convert.ToDouble(sRenGross);
                    dTNewGross += Convert.ToDouble(sNewGross);
                    dTRetGross += Convert.ToDouble(sRetGross);

                    dTData1 += Convert.ToDouble(sData1);
                    dTData2 += Convert.ToDouble(sData2);
                    dTData3 += Convert.ToDouble(sData3);
                    dTData4 += Convert.ToDouble(sData4);
                    dTData5 += Convert.ToDouble(sData5);
                    dTData6 += Convert.ToDouble(sData6);
                    dTData7 += Convert.ToDouble(sData7);
                    dTData8 += Convert.ToDouble(sData8);
                    dTData9 += Convert.ToDouble(sData9);
                    dTData10 += Convert.ToDouble(sData10);
                    dTData11 += Convert.ToDouble(sData11);
                    dTData12 += Convert.ToDouble(sData12);
                    dTData13 += Convert.ToDouble(sData13);
                    dTData14 += Convert.ToDouble(sData14);
                    dTData15 += Convert.ToDouble(sData15);
                    dTData16 += Convert.ToDouble(sData16);
                    dTData17 += Convert.ToDouble(sData17);
                    dTData18 += Convert.ToDouble(sData18);

                    dTTotalTax += Convert.ToDouble(sTotalTax);
                    dTSurcharge += Convert.ToDouble(sSurcharge);
                    dTTotalCollect += Convert.ToDouble(sTotalCollect);
                    #endregion

                    if (sTempDataGroup == "")
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<1500;" + sTempDataGroup;
                        this.axVSPrinter1.FontBold = false;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                        this.axVSPrinter1.FontBold = true;
                        //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
                        this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                + dREN.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNEW.ToString() + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                + dRET.ToString() + "|"
                                + dRetGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dData16.ToString("#,##0.00") + "|"
                                  + dData17.ToString("#,##0.00") + "|"
                                  + dData18.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

                        dREN = 0;
                        dRenGross = 0;
                        dNEW = 0;
                        dNewGross = 0;
                        dRET = 0;
                        dRetGross = 0;
                        dData1 = 0;
                        dData2 = 0;
                        dData3 = 0;
                        dData4 = 0;
                        dData5 = 0;
                        dData6 = 0;
                        dData7 = 0;
                        dData8 = 0;
                        dData9 = 0;
                        dData10 = 0;
                        dData11 = 0;
                        dData12 = 0;
                        dData13 = 0;
                        dData14 = 0;
                        dData15 = 0;
                        dData16 = 0;
                        dData17 = 0;
                        dData18 = 0;
                        dTotalTax = 0;
                        dSurcharge = 0;
                        dTotalCollect = 0;

                        this.axVSPrinter1.Paragraph = "";
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<1500;" + sDataGroup;
                        this.axVSPrinter1.FontBold = false;

                    }

                    dREN += Convert.ToDouble(sRen);
                    dNEW += Convert.ToDouble(sNew);
                    dRET += Convert.ToDouble(sRet);
                    dRenGross += Convert.ToDouble(sRenGross);
                    dNewGross += Convert.ToDouble(sNewGross);
                    dRetGross += Convert.ToDouble(sRetGross);

                    dData1 += Convert.ToDouble(sData1);
                    dData2 += Convert.ToDouble(sData2);
                    dData3 += Convert.ToDouble(sData3);
                    dData4 += Convert.ToDouble(sData4);
                    dData5 += Convert.ToDouble(sData5);
                    dData6 += Convert.ToDouble(sData6);
                    dData7 += Convert.ToDouble(sData7);
                    dData8 += Convert.ToDouble(sData8);
                    dData9 += Convert.ToDouble(sData9);
                    dData10 += Convert.ToDouble(sData10);
                    dData11 += Convert.ToDouble(sData11);
                    dData12 += Convert.ToDouble(sData12);
                    dData13 += Convert.ToDouble(sData13);
                    dData14 += Convert.ToDouble(sData14);
                    dData15 += Convert.ToDouble(sData15);
                    dData16 += Convert.ToDouble(sData16);
                    dData17 += Convert.ToDouble(sData17);
                    dData18 += Convert.ToDouble(sData18);

                    dTotalTax += Convert.ToDouble(sTotalTax);
                    dSurcharge += Convert.ToDouble(sSurcharge);
                    dTotalCollect += Convert.ToDouble(sTotalCollect);
                    //this.axVSPrinter1.FontSize;


                    //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25    26    27    28
                    this.axVSPrinter1.Table = @"^2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                      + sDataGroup2 + "|"
                      + sRen + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|" + sNew + "|" + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|" + sRet + "|" + Convert.ToDouble(sRetGross).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                }
                pSet.Close();
            }

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                + dREN.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNEW.ToString() + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                + dRET.ToString() + "|"
                                + dRetGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dData16.ToString("#,##0.00") + "|"
                                  + dData17.ToString("#,##0.00") + "|"
                                  + dData18.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";

            //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
            this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                      + dTREN.ToString() + "|"
                      + dTRenGross.ToString("#,##0.00") + "|"
                      + dTNEW.ToString() + "|"
                      + dTNewGross.ToString("#,##0.00") + "|"
                      + dTRET.ToString() + "|"
                      + dTRetGross.ToString("#,##0.00") + "|"
                      + dTData1.ToString("#,##0.00") + "|"
                      + dTData2.ToString("#,##0.00") + "|"
                      + dTData3.ToString("#,##0.00") + "|"
                      + dTData4.ToString("#,##0.00") + "|"
                      + dTData5.ToString("#,##0.00") + "|"
                      + dTData6.ToString("#,##0.00") + "|"
                      + dTData7.ToString("#,##0.00") + "|"
                      + dTData8.ToString("#,##0.00") + "|"
                      + dTData9.ToString("#,##0.00") + "|"
                      + dTData10.ToString("#,##0.00") + "|"
                      + dTData11.ToString("#,##0.00") + "|"
                      + dTData12.ToString("#,##0.00") + "|"
                      + dTData13.ToString("#,##0.00") + "|"
                      + dTData14.ToString("#,##0.00") + "|"
                      + dTData15.ToString("#,##0.00") + "|"
                      + dTData16.ToString("#,##0.00") + "|"
                      + dTData17.ToString("#,##0.00") + "|"
                      + dTData18.ToString("#,##0.00") + "|"
                      + dTTotalTax.ToString("#,##0.00") + "|"
                      + dTSurcharge.ToString("#,##0.00") + "|"
                      + dTTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        //BusinessStatus
        
        private void OnReportWideSummaryOfCollectionBusinessStatus()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sRen, sNew, sRet, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sData17, sData18, sTotalTax, sSurcharge, sTotalCollect;
            double dREN = 0, dNEW = 0, dRET = 0, dRenGross = 0, dNewGross = 0, dRetGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dData17 = 0, dData18 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTREN = 0, dTNEW = 0, dTRET = 0, dTRenGross = 0, dTNewGross = 0, dTRetGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTData17 = 0, dTData18 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;
            double dSubData = 0, dData19 = 0, dData20 = 0, dData21 = 0, dTData19 = 0, dTData20 = 0, dTData21 = 0, dTSubData = 0;
            String sData19, sData20, sData21, sSubData; 

            // RMC 20150522 corrections in reports (s)
            for (int i = 1; i <= 2; i++)
            {
                sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                dTotalTax = 0; dSurcharge = 0; dTotalCollect = 0;

                m_iBatchCnt = i;

                sTempDataGroup = "";

                if (m_iBatchCnt == 2)
                    this.axVSPrinter1.NewPage();
                // RMC 20150522 corrections in reports (e)

                sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16,RS.data17, RS.data18, RS.data19, RS.data20, RS.data21, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21+rs.sur_interest) as TOTALCOLLECTION";
                sQuery += ", sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10) as SUBTOTALTAXES";
                sQuery += " FROM REPORT_SUMCOLL RS";
                sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
                sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.data16, RS.data17, RS.data18, RS.data19, RS.data20, RS.data21, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        // RMC 20150522 corrections in reports (s)
                        sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                        sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                        sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                        sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                        sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                        // RMC 20150522 corrections in reports (e)

                        sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                        sDataGroup = pSet.GetString("data_group1");
                        sDataGroup2 = pSet.GetString("data_group2");

                        sNew = pSet.GetInt("new_no").ToString();
                        sRenGross = pSet.GetDouble("ren_gross").ToString();
                        sNewGross = pSet.GetDouble("new_cap").ToString();

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            sData1 = pSet.GetDouble("data1").ToString();
                            sData2 = pSet.GetDouble("data2").ToString();
                            sData3 = pSet.GetDouble("data3").ToString();
                            sData4 = pSet.GetDouble("data4").ToString();
                            sData5 = pSet.GetDouble("data5").ToString();
                            sData6 = pSet.GetDouble("data6").ToString();
                            sData7 = pSet.GetDouble("data7").ToString();
                            sData8 = pSet.GetDouble("data8").ToString();
                            sData9 = pSet.GetDouble("data9").ToString();
                            sData10 = pSet.GetDouble("data10").ToString();
                        }
                        else
                        {
                            sData11 = pSet.GetDouble("data11").ToString();
                            sData12 = pSet.GetDouble("data12").ToString();
                            sData13 = pSet.GetDouble("data13").ToString();
                            sData14 = pSet.GetDouble("data14").ToString();
                            sData15 = pSet.GetDouble("data15").ToString();
                            sData16 = pSet.GetDouble("data16").ToString();
                            sData17 = pSet.GetDouble("data17").ToString();
                            sData18 = pSet.GetDouble("data18").ToString();
                            sData19 = pSet.GetDouble("data19").ToString();
                            sData20 = pSet.GetDouble("data20").ToString();
                            sData21 = pSet.GetDouble("data21").ToString();
                        }
                        sSubData = pSet.GetDouble("SUBTOTALTAXES").ToString();

                        sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                        sSurcharge = pSet.GetDouble("sur_interest").ToString();
                        sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                        #region Total
                        dTNEW += Convert.ToDouble(sNew);
                        dTRenGross += Convert.ToDouble(sRenGross);
                        dTNewGross += Convert.ToDouble(sNewGross);

                        if (m_iBatchCnt == 1)
                        {
                            dTData1 += Convert.ToDouble(sData1);
                            dTData2 += Convert.ToDouble(sData2);
                            dTData3 += Convert.ToDouble(sData3);
                            dTData4 += Convert.ToDouble(sData4);
                            dTData5 += Convert.ToDouble(sData5);
                            dTData6 += Convert.ToDouble(sData6);
                            dTData7 += Convert.ToDouble(sData7);
                            dTData8 += Convert.ToDouble(sData8);
                            dTData9 += Convert.ToDouble(sData9);
                            dTData10 += Convert.ToDouble(sData10);
                            dTSubData += Convert.ToDouble(sSubData);
                        }
                        else
                        {
                            dTData11 += Convert.ToDouble(sData11);
                            dTData12 += Convert.ToDouble(sData12);
                            dTData13 += Convert.ToDouble(sData13);
                            dTData14 += Convert.ToDouble(sData14);
                            dTData15 += Convert.ToDouble(sData15);
                            dTData16 += Convert.ToDouble(sData16);
                            dTData17 += Convert.ToDouble(sData17);
                            dTData18 += Convert.ToDouble(sData18);
                            dTData19 += Convert.ToDouble(sData19);
                            dTData20 += Convert.ToDouble(sData20);
                            dTData21 += Convert.ToDouble(sData21);

                            dTTotalTax += Convert.ToDouble(sTotalTax);
                            dTSurcharge += Convert.ToDouble(sSurcharge);
                            dTTotalCollect += Convert.ToDouble(sTotalCollect);
                        }
                        #endregion

                        if (sTempDataGroup == "")
                        {
                            this.axVSPrinter1.FontBold = true;
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<4000;" + sTempDataGroup;
                            this.axVSPrinter1.Paragraph = "";
                            this.axVSPrinter1.FontBold = false;
                        }
                        else if (sDataGroup != sTempDataGroup)
                        {
                            this.axVSPrinter1.FontBold = true;
                            //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
                            /*this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                    + dNEW.ToString() + "|"
                                    + dRenGross.ToString("#,##0.00") + "|"
                                    + dNewGross.ToString("#,##0.00") + "|"
                                      + dData1.ToString("#,##0.00") + "|"
                                      + dData2.ToString("#,##0.00") + "|"
                                      + dData3.ToString("#,##0.00") + "|"
                                      + dData4.ToString("#,##0.00") + "|"
                                      + dData5.ToString("#,##0.00") + "|"
                                      + dData6.ToString("#,##0.00") + "|"
                                      + dData7.ToString("#,##0.00") + "|"
                                      + dData8.ToString("#,##0.00") + "|"
                                      + dData9.ToString("#,##0.00") + "|"
                                      + dData10.ToString("#,##0.00") + "|"
                                      + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");
                            */
                            if (m_iBatchCnt == 1)
                            {
                                this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                    + dNEW.ToString() + "|"
                                    + dRenGross.ToString("#,##0.00") + "|"
                                    + dNewGross.ToString("#,##0.00") + "|"
                                      + dData1.ToString("#,##0.00") + "|"
                                      + dData2.ToString("#,##0.00") + "|"
                                      + dData3.ToString("#,##0.00") + "|"
                                      + dData4.ToString("#,##0.00") + "|"
                                      + dData5.ToString("#,##0.00") + "|"
                                      + dData6.ToString("#,##0.00") + "|"
                                      + dData7.ToString("#,##0.00") + "|"
                                      + dData8.ToString("#,##0.00") + "|"
                                      + dData9.ToString("#,##0.00") + "|"
                                      + dData10.ToString("#,##0.00") + "|"
                                      + dSubData.ToString("#,##0.00");
                            }
                            else
                            {
                                this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;SUB TOTAL|"
                                + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dData19.ToString("#,##0.00") + "|"
                                      + dData20.ToString("#,##0.00") + "|"
                                      + dData21.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");
                            }

                            dNEW = 0;
                            dRenGross = 0;
                            dNewGross = 0;
                            dData1 = 0;
                            dData2 = 0;
                            dData3 = 0;
                            dData4 = 0;
                            dData5 = 0;
                            dData6 = 0;
                            dData7 = 0;
                            dData8 = 0;
                            dData9 = 0;
                            dData10 = 0;
                            dData11 = 0;
                            dData12 = 0;
                            dData13 = 0;
                            dData14 = 0;
                            dData15 = 0;
                            dData16 = 0;
                            dData17 = 0;
                            dData18 = 0;
                            dData19 = 0;
                            dData20 = 0;
                            dData21 = 0;
                            dSubData = 0;
                            dTotalTax = 0;
                            dSurcharge = 0;
                            dTotalCollect = 0;

                            this.axVSPrinter1.Paragraph = "";
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<4000;" + sDataGroup;
                            this.axVSPrinter1.FontBold = false;
                            this.axVSPrinter1.Paragraph = "";
                        }

                        dNEW += Convert.ToDouble(sNew);
                        dRenGross += Convert.ToDouble(sRenGross);
                        dNewGross += Convert.ToDouble(sNewGross);

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            dData1 += Convert.ToDouble(sData1);
                            dData2 += Convert.ToDouble(sData2);
                            dData3 += Convert.ToDouble(sData3);
                            dData4 += Convert.ToDouble(sData4);
                            dData5 += Convert.ToDouble(sData5);
                            dData6 += Convert.ToDouble(sData6);
                            dData7 += Convert.ToDouble(sData7);
                            dData8 += Convert.ToDouble(sData8);
                            dData9 += Convert.ToDouble(sData9);
                            dData10 += Convert.ToDouble(sData10);
                        }
                        else
                        {
                            dData11 += Convert.ToDouble(sData11);
                            dData12 += Convert.ToDouble(sData12);
                            dData13 += Convert.ToDouble(sData13);
                            dData14 += Convert.ToDouble(sData14);
                            dData15 += Convert.ToDouble(sData15);
                            dData16 += Convert.ToDouble(sData16);
                            dData17 += Convert.ToDouble(sData17);
                            dData18 += Convert.ToDouble(sData18);
                            dData19 += Convert.ToDouble(sData19);
                            dData20 += Convert.ToDouble(sData20);
                            dData21 += Convert.ToDouble(sData21);
                        }

                        dTotalTax += Convert.ToDouble(sTotalTax);
                        dSurcharge += Convert.ToDouble(sSurcharge);
                        dTotalCollect += Convert.ToDouble(sTotalCollect);
                        dSubData += Convert.ToDouble(sSubData);

                        //this.axVSPrinter1.FontSize;


                        //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25    26    27    28
                        /*this.axVSPrinter1.Table = @"^2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                          + sDataGroup2 + "|"
                          + sNew + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|"  + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                        */
                        if (m_iBatchCnt == 1)
                        {
                            this.axVSPrinter1.Table = @"^2000|>500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                          + sDataGroup2 + "|"
                          + sNew + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|" + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSubData).ToString("#,##0.00");
                        }
                        else
                        {
                            this.axVSPrinter1.Table = @"^2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;"
                            + sDataGroup2 + "|"
                            + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData19).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData20).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData21).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                        }

                        this.axVSPrinter1.Paragraph = "";
                    }
                    pSet.Close();
                }

                this.axVSPrinter1.FontBold = true;
                /*this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                    + dNEW.ToString() + "|"
                                    + dRenGross.ToString("#,##0.00") + "|"
                                    + dNewGross.ToString("#,##0.00") + "|"
                                      + dData1.ToString("#,##0.00") + "|"
                                      + dData2.ToString("#,##0.00") + "|"
                                      + dData3.ToString("#,##0.00") + "|"
                                      + dData4.ToString("#,##0.00") + "|"
                                      + dData5.ToString("#,##0.00") + "|"
                                      + dData6.ToString("#,##0.00") + "|"
                                      + dData7.ToString("#,##0.00") + "|"
                                      + dData8.ToString("#,##0.00") + "|"
                                      + dData9.ToString("#,##0.00") + "|"
                                      + dData10.ToString("#,##0.00") + "|"
                                      + dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");

                this.axVSPrinter1.Paragraph = "";

                //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    
                this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                          + dTNEW.ToString() + "|"
                          + dTRenGross.ToString("#,##0.00") + "|"
                          + dTNewGross.ToString("#,##0.00") + "|"
                          + dTData1.ToString("#,##0.00") + "|"
                          + dTData2.ToString("#,##0.00") + "|"
                          + dTData3.ToString("#,##0.00") + "|"
                          + dTData4.ToString("#,##0.00") + "|"
                          + dTData5.ToString("#,##0.00") + "|"
                          + dTData6.ToString("#,##0.00") + "|"
                          + dTData7.ToString("#,##0.00") + "|"
                          + dTData8.ToString("#,##0.00") + "|"
                          + dTData9.ToString("#,##0.00") + "|"
                          + dTData10.ToString("#,##0.00") + "|"
                          + dTData11.ToString("#,##0.00") + "|"
                          + dTData12.ToString("#,##0.00") + "|"
                          + dTData13.ToString("#,##0.00") + "|"
                          + dTData14.ToString("#,##0.00") + "|"
                          + dTData15.ToString("#,##0.00") + "|"
                          + dTData16.ToString("#,##0.00") + "|"
                          + dTData17.ToString("#,##0.00") + "|"
                          + dTData18.ToString("#,##0.00") + "|"
                          + dTTotalTax.ToString("#,##0.00") + "|"
                          + dTSurcharge.ToString("#,##0.00") + "|"
                          + dTTotalCollect.ToString("#,##0.00");*/

                if (m_iBatchCnt == 1)
                {
                    this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                    + dNEW.ToString() + "|"
                                    + dRenGross.ToString("#,##0.00") + "|"
                                    + dNewGross.ToString("#,##0.00") + "|"
                                      + dData1.ToString("#,##0.00") + "|"
                                      + dData2.ToString("#,##0.00") + "|"
                                      + dData3.ToString("#,##0.00") + "|"
                                      + dData4.ToString("#,##0.00") + "|"
                                      + dData5.ToString("#,##0.00") + "|"
                                      + dData6.ToString("#,##0.00") + "|"
                                      + dData7.ToString("#,##0.00") + "|"
                                      + dData8.ToString("#,##0.00") + "|"
                                      + dData9.ToString("#,##0.00") + "|"
                                      + dData10.ToString("#,##0.00") + "|"
                                      + dSubData.ToString("#,##0.00");

                                      

                    this.axVSPrinter1.Paragraph = "";

                    //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    
                    this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                              + dTNEW.ToString() + "|"
                              + dTRenGross.ToString("#,##0.00") + "|"
                              + dTNewGross.ToString("#,##0.00") + "|"
                              + dTData1.ToString("#,##0.00") + "|"
                              + dTData2.ToString("#,##0.00") + "|"
                              + dTData3.ToString("#,##0.00") + "|"
                              + dTData4.ToString("#,##0.00") + "|"
                              + dTData5.ToString("#,##0.00") + "|"
                              + dTData6.ToString("#,##0.00") + "|"
                              + dTData7.ToString("#,##0.00") + "|"
                              + dTData8.ToString("#,##0.00") + "|"
                              + dTData9.ToString("#,##0.00") + "|"
                              + dTData10.ToString("#,##0.00") + "|"
                              + dTSubData.ToString("#,##0.00");

                              
                }
                else
                {
                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200; SUB TOTAL |"
                    +dData11.ToString("#,##0.00") + "|"
                                      + dData12.ToString("#,##0.00") + "|"
                                      + dData13.ToString("#,##0.00") + "|"
                                      + dData14.ToString("#,##0.00") + "|"
                                      + dData15.ToString("#,##0.00") + "|"
                                      + dData16.ToString("#,##0.00") + "|"
                                      + dData17.ToString("#,##0.00") + "|"
                                      + dData18.ToString("#,##0.00") + "|"
                                      + dData19.ToString("#,##0.00") + "|"
                                      + dData20.ToString("#,##0.00") + "|"
                                      + dData21.ToString("#,##0.00") + "|"
                                      + dTotalTax.ToString("#,##0.00") + "|"
                                      + dSurcharge.ToString("#,##0.00") + "|"
                                      + dTotalCollect.ToString("#,##0.00");

                    this.axVSPrinter1.Paragraph = "";

                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200; TOTAL|"
                    +dTData11.ToString("#,##0.00") + "|"
                              + dTData12.ToString("#,##0.00") + "|"
                              + dTData13.ToString("#,##0.00") + "|"
                              + dTData14.ToString("#,##0.00") + "|"
                              + dTData15.ToString("#,##0.00") + "|"
                              + dTData16.ToString("#,##0.00") + "|"
                              + dTData17.ToString("#,##0.00") + "|"
                              + dTData18.ToString("#,##0.00") + "|"
                              + dTData19.ToString("#,##0.00") + "|"
                              + dTData20.ToString("#,##0.00") + "|"
                              + dTData21.ToString("#,##0.00") + "|"
                              + dTTotalTax.ToString("#,##0.00") + "|"
                              + dTSurcharge.ToString("#,##0.00") + "|"
                              + dTTotalCollect.ToString("#,##0.00");

                }
            }

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        private void OnReportStanWideSummaryOfCollectionBusinessStatus()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sNew,  sRenGross, sNewGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sTotalTax, sSurcharge, sTotalCollect;
            double dNEW = 0, dRenGross = 0, dNewGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTNEW = 0, dTRenGross = 0, dTNewGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;

            sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.sur_interest) as TOTALCOLLECTION";
            sQuery += " FROM REPORT_SUMCOLL RS";
            sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
            sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
            sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                    sDataGroup = pSet.GetString("data_group1");
                    sDataGroup2 = pSet.GetString("data_group2");

                    sNew = pSet.GetInt("new_no").ToString();
                    sRenGross = pSet.GetDouble("ren_gross").ToString();
                    sNewGross = pSet.GetDouble("new_cap").ToString();

                    sData1 = pSet.GetDouble("data1").ToString();
                    sData2 = pSet.GetDouble("data2").ToString();
                    sData3 = pSet.GetDouble("data3").ToString();
                    sData4 = pSet.GetDouble("data4").ToString();
                    sData5 = pSet.GetDouble("data5").ToString();
                    sData6 = pSet.GetDouble("data6").ToString();
                    sData7 = pSet.GetDouble("data7").ToString();
                    sData8 = pSet.GetDouble("data8").ToString();
                    sData9 = pSet.GetDouble("data9").ToString();
                    sData10 = pSet.GetDouble("data10").ToString();
                    sData11 = pSet.GetDouble("data11").ToString();
                    sData12 = pSet.GetDouble("data12").ToString();
                    sData13 = pSet.GetDouble("data13").ToString();

                    sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                    sSurcharge = pSet.GetDouble("sur_interest").ToString();
                    sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                    #region Total
                    dTNEW += Convert.ToDouble(sNew);
                    dTRenGross += Convert.ToDouble(sRenGross);
                    dTNewGross += Convert.ToDouble(sNewGross);

                    dTData1 += Convert.ToDouble(sData1);
                    dTData2 += Convert.ToDouble(sData2);
                    dTData3 += Convert.ToDouble(sData3);
                    dTData4 += Convert.ToDouble(sData4);
                    dTData5 += Convert.ToDouble(sData5);
                    dTData6 += Convert.ToDouble(sData6);
                    dTData7 += Convert.ToDouble(sData7);
                    dTData8 += Convert.ToDouble(sData8);
                    dTData9 += Convert.ToDouble(sData9);
                    dTData10 += Convert.ToDouble(sData10);
                    dTData11 += Convert.ToDouble(sData11);
                    dTData12 += Convert.ToDouble(sData12);
                    dTData13 += Convert.ToDouble(sData13);

                    dTTotalTax += Convert.ToDouble(sTotalTax);
                    dTSurcharge += Convert.ToDouble(sSurcharge);
                    dTTotalCollect += Convert.ToDouble(sTotalCollect);
                    #endregion

                    if (sTempDataGroup == "")
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<4000;" + sTempDataGroup;
                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.FontBold = false;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                        this.axVSPrinter1.FontBold = true;
                                                     //1   2    3      4    5      6     7      8    9    10     11    12    13    14    15   16    17   18     19    20  
                        this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                + dNEW.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

                        dNEW = 0;
                        dRenGross = 0;
                        dNewGross = 0;
                        dData1 = 0;
                        dData2 = 0;
                        dData3 = 0;
                        dData4 = 0;
                        dData5 = 0;
                        dData6 = 0;
                        dData7 = 0;
                        dData8 = 0;
                        dData9 = 0;
                        dData10 = 0;
                        dData11 = 0;
                        dData12 = 0;
                        dData13 = 0;
                        dTotalTax = 0;
                        dSurcharge = 0;
                        dTotalCollect = 0;

                        this.axVSPrinter1.Paragraph = "";
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<4000;" + sDataGroup;
                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.Paragraph = "";
                    }

                    dNEW += Convert.ToDouble(sNew);
                    dRenGross += Convert.ToDouble(sRenGross);
                    dNewGross += Convert.ToDouble(sNewGross);

                    dData1 += Convert.ToDouble(sData1);
                    dData2 += Convert.ToDouble(sData2);
                    dData3 += Convert.ToDouble(sData3);
                    dData4 += Convert.ToDouble(sData4);
                    dData5 += Convert.ToDouble(sData5);
                    dData6 += Convert.ToDouble(sData6);
                    dData7 += Convert.ToDouble(sData7);
                    dData8 += Convert.ToDouble(sData8);
                    dData9 += Convert.ToDouble(sData9);
                    dData10 += Convert.ToDouble(sData10);
                    dData11 += Convert.ToDouble(sData11);
                    dData12 += Convert.ToDouble(sData12);
                    dData13 += Convert.ToDouble(sData13);

                    dTotalTax += Convert.ToDouble(sTotalTax);
                    dSurcharge += Convert.ToDouble(sSurcharge);
                    dTotalCollect += Convert.ToDouble(sTotalCollect);
                    //this.axVSPrinter1.FontSize;


                                                 //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16     17   18     19    20 
                    this.axVSPrinter1.Table = @"^2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                      + sDataGroup2 + "|"
                      + sNew + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|" + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");

                    this.axVSPrinter1.Paragraph = "";
                }
                pSet.Close();
            }

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                + dNEW.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";

                                         //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20 
            this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                      + dTNEW.ToString() + "|"
                      + dTRenGross.ToString("#,##0.00") + "|"
                      + dTNewGross.ToString("#,##0.00") + "|"
                      + dTData1.ToString("#,##0.00") + "|"
                      + dTData2.ToString("#,##0.00") + "|"
                      + dTData3.ToString("#,##0.00") + "|"
                      + dTData4.ToString("#,##0.00") + "|"
                      + dTData5.ToString("#,##0.00") + "|"
                      + dTData6.ToString("#,##0.00") + "|"
                      + dTData7.ToString("#,##0.00") + "|"
                      + dTData8.ToString("#,##0.00") + "|"
                      + dTData9.ToString("#,##0.00") + "|"
                      + dTData10.ToString("#,##0.00") + "|"
                      + dTData11.ToString("#,##0.00") + "|"
                      + dTData12.ToString("#,##0.00") + "|"
                      + dTData13.ToString("#,##0.00") + "|"
                      + dTTotalTax.ToString("#,##0.00") + "|"
                      + dTSurcharge.ToString("#,##0.00") + "|"
                      + dTTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        //Line of Business

        private void OnReportWideSummaryOfCollectionLineOfBusiness()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sRen, sNew, sRet, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sData17, sData18, sTotalTax, sSurcharge, sTotalCollect;
            double dNEW = 0, dRenGross = 0, dNewGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTNEW = 0, dTRenGross = 0, dTNewGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;
            OracleResultSet pSet = new OracleResultSet();

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);

            sQuery = @"SELECT count (*) FROM REPORT_SUMCOLL RS";
            sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
            sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
            if (m_sFilterValue != "")
                sQuery += " AND RS.data_group1 in (" + m_sFilterValue + ")";
            sQuery += " ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            sQuery = @"SELECT RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21) as TOTALTAXES,sum(RS.data1+RS.data2+RS.data3+RS.data4+RS.data5+RS.data6+RS.data7+RS.data8+RS.data9+RS.data10+RS.data11+RS.data12+RS.data13+RS.data14+RS.data15+RS.data16+RS.data17+RS.data18+RS.data19+RS.data20+RS.data21+RS.sur_interest) as TOTALCOLLECTION";
            sQuery += " FROM REPORT_SUMCOLL RS";
            sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
            sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
            if (m_sFilterValue != "")
                sQuery += " AND RS.data_group1 in (" + m_sFilterValue + ")";
            sQuery += " GROUP BY RS.data_group1, RS.data_group2, RS.new_no, RS.new_cap, RS.ren_no, RS.ren_gross, RS.ret_no, RS.ret_gross, RS.data1, RS.data2, RS.data3, RS.data4, RS.data5, RS.data6, RS.data7, RS.data8, RS.data9, RS.data10, RS.data11, RS.data12, RS.data13, RS.data14, RS.data15, RS.sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC, RS.data_group2 ASC";

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                    sDataGroup = pSet.GetString("data_group1");
                    sDataGroup2 = pSet.GetString("data_group2");

                    sNew = pSet.GetInt("new_no").ToString();
                    sRenGross = pSet.GetDouble("ren_gross").ToString();
                    sNewGross = pSet.GetDouble("new_cap").ToString();

                    sData1 = pSet.GetDouble("data1").ToString();
                    sData2 = pSet.GetDouble("data2").ToString();
                    sData3 = pSet.GetDouble("data3").ToString();
                    sData4 = pSet.GetDouble("data4").ToString();
                    sData5 = pSet.GetDouble("data5").ToString();
                    sData6 = pSet.GetDouble("data6").ToString();
                    sData7 = pSet.GetDouble("data7").ToString();
                    sData8 = pSet.GetDouble("data8").ToString();
                    sData9 = pSet.GetDouble("data9").ToString();
                    sData10 = pSet.GetDouble("data10").ToString();
                    sData11 = pSet.GetDouble("data11").ToString();
                    sData12 = pSet.GetDouble("data12").ToString();
                    sData13 = pSet.GetDouble("data13").ToString();
                    sData14 = pSet.GetDouble("data14").ToString();
                    sData15 = pSet.GetDouble("data15").ToString();

                    sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                    sSurcharge = pSet.GetDouble("sur_interest").ToString();
                    sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                    #region Total
                    dTNEW += Convert.ToDouble(sNew);
                    dTRenGross += Convert.ToDouble(sRenGross);
                    dTNewGross += Convert.ToDouble(sNewGross);

                    dTData1 += Convert.ToDouble(sData1);
                    dTData2 += Convert.ToDouble(sData2);
                    dTData3 += Convert.ToDouble(sData3);
                    dTData4 += Convert.ToDouble(sData4);
                    dTData5 += Convert.ToDouble(sData5);
                    dTData6 += Convert.ToDouble(sData6);
                    dTData7 += Convert.ToDouble(sData7);
                    dTData8 += Convert.ToDouble(sData8);
                    dTData9 += Convert.ToDouble(sData9);
                    dTData10 += Convert.ToDouble(sData10);
                    dTData11 += Convert.ToDouble(sData11);
                    dTData12 += Convert.ToDouble(sData12);
                    dTData13 += Convert.ToDouble(sData13);
                    dTData14 += Convert.ToDouble(sData14);
                    dTData15 += Convert.ToDouble(sData15);

                    dTTotalTax += Convert.ToDouble(sTotalTax);
                    dTSurcharge += Convert.ToDouble(sSurcharge);
                    dTTotalCollect += Convert.ToDouble(sTotalCollect);
                    #endregion

                    if (sTempDataGroup == "")
                    {
                        this.axVSPrinter1.FontBold = true;
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<4000;" + sTempDataGroup;
                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.FontBold = false;
                    }
                    else if (sDataGroup != sTempDataGroup)
                    {
                        this.axVSPrinter1.FontBold = true;
                                                    //1    2    3      4   5      6    7      8    9     10     11    12    13    14    15    16    17   18     19    20    21   22  
                        this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|; SUB TOTAL |"
                                + dNEW.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

                        dNEW = 0;
                        dRenGross = 0;
                        dNewGross = 0;
                        dData1 = 0;
                        dData2 = 0;
                        dData3 = 0;
                        dData4 = 0;
                        dData5 = 0;
                        dData6 = 0;
                        dData7 = 0;
                        dData8 = 0;
                        dData9 = 0;
                        dData10 = 0;
                        dData11 = 0;
                        dData12 = 0;
                        dData13 = 0;
                        dData14 = 0;
                        dData15 = 0;
                        dTotalTax = 0;
                        dSurcharge = 0;
                        dTotalCollect = 0;

                        this.axVSPrinter1.Paragraph = "";
                        sTempDataGroup = sDataGroup;
                        this.axVSPrinter1.Table = "<4000;" + sDataGroup;
                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.Paragraph = "";
                    }

                    dNEW += Convert.ToDouble(sNew);
                    dRenGross += Convert.ToDouble(sRenGross);
                    dNewGross += Convert.ToDouble(sNewGross);

                    dData1 += Convert.ToDouble(sData1);
                    dData2 += Convert.ToDouble(sData2);
                    dData3 += Convert.ToDouble(sData3);
                    dData4 += Convert.ToDouble(sData4);
                    dData5 += Convert.ToDouble(sData5);
                    dData6 += Convert.ToDouble(sData6);
                    dData7 += Convert.ToDouble(sData7);
                    dData8 += Convert.ToDouble(sData8);
                    dData9 += Convert.ToDouble(sData9);
                    dData10 += Convert.ToDouble(sData10);
                    dData11 += Convert.ToDouble(sData11);
                    dData12 += Convert.ToDouble(sData12);
                    dData13 += Convert.ToDouble(sData13);
                    dData14 += Convert.ToDouble(sData14);
                    dData15 += Convert.ToDouble(sData15);

                    dTotalTax += Convert.ToDouble(sTotalTax);
                    dSurcharge += Convert.ToDouble(sSurcharge);
                    dTotalCollect += Convert.ToDouble(sTotalCollect);
                    //this.axVSPrinter1.FontSize;


                                                 //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16     17   18     19    20    21   22
                    this.axVSPrinter1.Table = @"^2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;"
                      + sDataGroup2 + "|"
                      + sNew + "|" + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|" + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");

                    this.axVSPrinter1.Paragraph = "";
                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                }
            }
            pSet.Close();
            
            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; SUB TOTAL |"
                                + dNEW.ToString() + "|"
                                + dRenGross.ToString("#,##0.00") + "|"
                                + dNewGross.ToString("#,##0.00") + "|"
                                  + dData1.ToString("#,##0.00") + "|"
                                  + dData2.ToString("#,##0.00") + "|"
                                  + dData3.ToString("#,##0.00") + "|"
                                  + dData4.ToString("#,##0.00") + "|"
                                  + dData5.ToString("#,##0.00") + "|"
                                  + dData6.ToString("#,##0.00") + "|"
                                  + dData7.ToString("#,##0.00") + "|"
                                  + dData8.ToString("#,##0.00") + "|"
                                  + dData9.ToString("#,##0.00") + "|"
                                  + dData10.ToString("#,##0.00") + "|"
                                  + dData11.ToString("#,##0.00") + "|"
                                  + dData12.ToString("#,##0.00") + "|"
                                  + dData13.ToString("#,##0.00") + "|"
                                  + dData14.ToString("#,##0.00") + "|"
                                  + dData15.ToString("#,##0.00") + "|"
                                  + dTotalTax.ToString("#,##0.00") + "|"
                                  + dSurcharge.ToString("#,##0.00") + "|"
                                  + dTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
                                        //1    2    3      4   5      6    7      8     9    10     11    12    13    14    15   16    17    18     19     20    21   22     
            this.axVSPrinter1.Table = ">2000|>500|>1300|>1300|>1300|>1300|>1400|>1300|>1300|>1550|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300; TOTAL |"
                      + dTNEW.ToString() + "|"
                      + dTRenGross.ToString("#,##0.00") + "|"
                      + dTNewGross.ToString("#,##0.00") + "|"
                      + dTData1.ToString("#,##0.00") + "|"
                      + dTData2.ToString("#,##0.00") + "|"
                      + dTData3.ToString("#,##0.00") + "|"
                      + dTData4.ToString("#,##0.00") + "|"
                      + dTData5.ToString("#,##0.00") + "|"
                      + dTData6.ToString("#,##0.00") + "|"
                      + dTData7.ToString("#,##0.00") + "|"
                      + dTData8.ToString("#,##0.00") + "|"
                      + dTData9.ToString("#,##0.00") + "|"
                      + dTData10.ToString("#,##0.00") + "|"
                      + dTData11.ToString("#,##0.00") + "|"
                      + dTData12.ToString("#,##0.00") + "|"
                      + dTData13.ToString("#,##0.00") + "|"
                      + dTData14.ToString("#,##0.00") + "|"
                      + dTData15.ToString("#,##0.00") + "|"
                      + dTTotalTax.ToString("#,##0.00") + "|"
                      + dTSurcharge.ToString("#,##0.00") + "|"
                      + dTTotalCollect.ToString("#,##0.00");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        private void ReportAbstract()//MCR 20140819
         {
            String sQuery = "";

            DateTime dtORDate = DateTime.Now;
            double dTotColl = 0, dbTax = 0, dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dOthFee = 0, dTaxFees = 0, dSurchInt = 0, dExcessCQ = 0, dAppTC = 0;
            string sOrNo = string.Empty;
            double dTotBns = 0;// RMC 20180201 display business count in Abstract - request from Jester (Malolos)
            double dTotal = 0;
            double dTotalTmp = 0;
            bool isNew = true;
            double dTotBnsTmp = 0;
            double dTotBnsTmp2 = 0;
            double dTotBnsTmp3 = 0;
            double dBnsCnt = 0;
            double dBnsCnt2 = 0;

            string sSOLO = ""; //MCR 20190613
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            string sBrgy = string.Empty;
            string sBrgyTmp = string.Empty;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();
            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);

            try
            {
                if (m_sReportSwitch == "AbstractCollectByDaily")
                {
                    if(m_iReportSwitch == 3)
                        pSet.Query = @"select count(*) from (select distinct(p.or_date) as Cnt, bs.bns_brgy from pay_hist p, businesses bs where p.bin = bs.bin and p.bin in (select bin from businesses where p.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "'  and p.or_no in (select or_no from or_table where fees_code = '50')) and p.or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and p.data_mode <> 'POS' order by p.or_date)"; //AFM 20200204
                    else
                        pSet.Query = @"select Count(distinct(or_date)) as Cnt from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' order by or_date";
                }
                else if (m_sReportSwitch == "AbstractCollectByOR")
                {
                    if (m_sOrFrom == string.Empty && m_sOrTo == string.Empty)
                    {
                        if (m_iReportSwitch == 3)
                            pSet.Query = "select Count(distinct(or_no)) as Cnt from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "' and pay_hist.or_no in (select or_no from or_table where fees_code = '50')) and or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' order by or_no"; //AFM 20200204
                        else
                            pSet.Query = "select Count(distinct(or_no)) as Cnt from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' order by or_no";
                    }
                    else
                    {
                        if (AppSettingsManager.GetConfigValue("10") == "021")   // RMC 20140909 Migration QA
                            pSet.Query = "select Count(distinct(or_no)) as Cnt from pay_hist where to_number(substr(or_no,3,20)) between to_number(substr('" + m_sOrFrom + "',3,20)) and to_number(substr('" + m_sOrTo + "',3,20)) and substr(or_no,1,2) = substr('" + m_sOrFrom + "',1,2) and data_mode <> 'POS' order by or_no";    // RMC 20140909 Migration QA
                        else
                        {
                            if (m_iReportSwitch == 3)
                                pSet.Query = "select Count(distinct(or_no)) as Cnt from pay_hist where  bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy like '" + m_sSelectedbrgy.Trim() + "' and pay_hist.or_no in (select or_no from or_table where fees_code = '50')) and or_no between '" + m_sOrFrom + "' and '" + m_sOrTo + "' and data_mode <> 'POS' order by or_no";
                            else
                                pSet.Query = "select Count(distinct(or_no)) as Cnt from pay_hist where or_no between '" + m_sOrFrom + "' and '" + m_sOrTo + "' and data_mode <> 'POS' order by or_no";
                        }
                    }
                }
                else //AbstractCollectByTeller
                {
                    if(m_iReportSwitch == 3)
                        pSet.Query = @"select Count(distinct(or_no)) as Cnt from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "' and pay_hist.or_no in (select or_no from or_table where fees_code = '50')) and or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and teller like '" + m_sTeller + "' order by or_no"; //AFM 20200204
                    else
                        pSet.Query = @"select Count(distinct(or_no)) as Cnt from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and teller like '" + m_sTeller + "' order by or_no";
                }
                int.TryParse(pSet.ExecuteScalar(), out intCount);
                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork); 
            #endregion

                if (m_sReportSwitch == "AbstractCollectByDaily")
                {
                    // RMC 20150617 (s)
                    if (m_iReportSwitch == 2)   
                        pSet.Query = @"select distinct(or_date) as or_date from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode = 'POS' order by or_date";
                        // RMC 20150617 (e)
                    else if (m_iReportSwitch == 3)
                        pSet.Query = @"select distinct(p.or_date) as or_date, bs.bns_brgy from pay_hist p, businesses bs where p.bin = bs.bin and p.bin in (select b.bin from businesses b where p.bin = b.bin and b.bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "'  and p.or_no in (select or_no from or_table where fees_code = '50')) and p.or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and p.data_mode <> 'POS' order by bs.bns_brgy, p.or_date"; //AFM 20200204 MAO-20-12236
                    else
                        pSet.Query = @"select distinct(or_date) as or_date from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' order by or_date";
                }
                else if (m_sReportSwitch == "AbstractCollectByOR")
                {
                    // RMC 20150617 (s)
                    if (m_iReportSwitch == 2)
                    {
                        if (m_sOrFrom == string.Empty && m_sOrTo == string.Empty)
                            pSet.Query = "select distinct(or_no) as or_no, bin, or_date from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode = 'POS' order by or_no";  // MCR 20141023
                        else
                        {
                            if (AppSettingsManager.GetConfigValue("10") == "021")
                                pSet.Query = "select distinct(or_no) as or_no, bin, or_date from pay_hist where to_number(substr(or_no,3,20)) between to_number(substr('" + m_sOrFrom + "',3,20)) and to_number(substr('" + m_sOrTo + "',3,20)) and substr(or_no,1,2) = substr('" + m_sOrFrom + "',1,2) and data_mode = 'POS' order by to_number(substr(or_no,3,20))"; //MCR 20141023
                            else
                                pSet.Query = "select distinct(or_no) as or_no, bin, or_date from pay_hist where or_no between '" + m_sOrFrom + "' and '" + m_sOrTo + "' and data_mode = 'POS' order by or_no"; //MCR 20141023
                        }
                    }// RMC 20150617 (e)
                    else if (m_iReportSwitch == 3) //AFM 20200204 MAO-20-12236
                    {
                        if (m_sOrFrom == string.Empty && m_sOrTo == string.Empty)
                            pSet.Query = "select distinct(PH.or_no) as or_no, PH.bin, PH.or_date, BN.bns_brgy from pay_hist PH, businesses BN where PH.bin = BN.bin and PH.bin in (select bin from businesses where PH.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "' and PH.or_no in (select or_no from or_table where fees_code = '50')) and PH.or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and PH.data_mode <> 'POS' order by bns_brgy, or_date"; 
                        else
                            pSet.Query = "select distinct(PH.or_no) as or_no, PH.bin, PH.or_date, BN.bns_brgy from pay_hist PH, businesses BN where PH.bin = BN.bin and PH.bin in (select bin from businesses where PH.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "' and PH.or_no in (select or_no from or_table where fees_code = '50')) and PH.or_no between '" + m_sOrFrom + "' and '" + m_sOrTo + "' and PH.data_mode <> 'POS' order by bns_brgy, or_date";
                    }
                    else
                    {
                        if (m_sOrFrom == string.Empty && m_sOrTo == string.Empty)
                            //pSet.Query = "select distinct(or_no) as or_no from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' order by or_no";
                            //pSet.Query = "select distinct(or_no) as or_no, bin from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' order by or_no";  // RMC 20140909 Migration QA
                            pSet.Query = "select distinct(or_no) as or_no, bin, or_date from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' order by or_no";  // MCR 20141023
                        else
                        {
                            //pSet.Query = "select distinct(or_no) as or_no from pay_hist where or_no between '" + m_sOrFrom + "' and '" + m_sOrTo + "' and data_mode <> 'POS' order by or_no";
                            // RMC 20140909 Migration QA (s)
                            if (AppSettingsManager.GetConfigValue("10") == "021")
                                //pSet.Query = "select distinct(or_no) as or_no, bin from pay_hist where to_number(substr(or_no,3,20)) between to_number(substr('" + m_sOrFrom + "',3,20)) and to_number(substr('" + m_sOrTo + "',3,20)) and substr(or_no,1,2) = substr('" + m_sOrFrom + "',1,2) and data_mode <> 'POS' order by to_number(substr(or_no,3,20))";
                                pSet.Query = "select distinct(or_no) as or_no, bin, or_date from pay_hist where to_number(substr(or_no,3,20)) between to_number(substr('" + m_sOrFrom + "',3,20)) and to_number(substr('" + m_sOrTo + "',3,20)) and substr(or_no,1,2) = substr('" + m_sOrFrom + "',1,2) and data_mode <> 'POS' order by to_number(substr(or_no,3,20))"; //MCR 20141023
                            else
                                //pSet.Query = "select distinct(or_no) as or_no, bin from pay_hist where or_no between '" + m_sOrFrom + "' and '" + m_sOrTo + "' and data_mode <> 'POS' order by or_no";
                                pSet.Query = "select distinct(or_no) as or_no, bin, or_date from pay_hist where or_no between '" + m_sOrFrom + "' and '" + m_sOrTo + "' and data_mode <> 'POS' order by or_no"; //MCR 20141023
                            // RMC 20140909 Migration QA (e)
                        }
                    }
                }
                else //AbstractCollectByTeller
                {
                    // RMC 20150617 (s)
                    if (m_iReportSwitch == 2)
                    {
                        pSet.Query = @"select distinct(or_no) as or_no, bin, or_date from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode = 'POS' and (teller like '" + m_sTeller + "' or bns_user like '" + m_sTeller + "') order by or_no"; 
                    }
                    // RMC 20150617 (e)
                    else if (m_iReportSwitch == 3)
                        pSet.Query = "select distinct(PH.or_no) as or_no,PH.bin,PH.or_date, BN.bns_brgy from pay_hist PH, businesses BN where PH.bin = BN.bin and PH.bin in (select bin from businesses where PH.bin = BN.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "' and PH.or_no in (select or_no from or_table where fees_code = '50')) and PH.or_no in (select or_no from rcd_remit where rcd_series = '" + m_sRCDNumber + "')"; //AFM 20200204
                    else
                    {
                        //pSet.Query = @"select distinct(or_no) as or_no from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and teller like '" + m_sTeller + "' order by or_no";
                        //pSet.Query = @"select distinct(or_no) as or_no, bin from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and teller like '" + m_sTeller + "' order by or_no"; // RMC 20140909 Migration QA
                        //pSet.Query = @"select distinct(or_no) as or_no, bin, or_date from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and teller like '" + m_sTeller + "' order by or_no"; // MCR 20141023
                        pSet.Query = "select distinct or_no as or_no,bin,or_date from pay_hist where or_no in (select or_no from rcd_remit where rcd_series = '"+ m_sRCDNumber +"')"; //JARS 20180207
                    }
                }
                int iCnt = 0;
                if (pSet.Execute()) //start
                    while (pSet.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        string sOrDetailsData = string.Empty;
                        string sBIN = string.Empty; // RMC 20140909 Migration QA
                        string sBnsName = string.Empty; // RMC 20140909 Migration QA
                        string sBrgy2 = string.Empty;

                        if (m_sReportSwitch == "AbstractCollectByDaily")
                        {
                            if (m_iReportSwitch == 3)
                                sBrgy2 = pSet.GetString("bns_brgy"); 
                            dtORDate = pSet.GetDateTime("or_date");
                            sOrDetailsData = dtORDate.ToShortDateString() + "|";
                            // RMC 20180201 display business count in Abstract - request from Jester (Malolos) (s)
                            string sBnsCnt = GetBnsCounts(dtORDate, sBrgy2);
                            sOrDetailsData += sBnsCnt + "|";   
                            dTotBns += Convert.ToInt32(sBnsCnt);
                            dTotBnsTmp2 = Convert.ToInt32(sBnsCnt); // for previous brgy count
                            // RMC 20180201 display business count in Abstract - request from Jester (Malolos) (e)

                            //AFM 20200211 total per brgy (s) 
                            if (m_iReportSwitch == 3)
                            {
                                sBrgy = pSet.GetString("bns_brgy");

                                if (sBrgyTmp != sBrgy) //compare previous brgy 
                                {
                                    if (isNew == false)
                                    {
                                        dTotalTmp += dTotal;
                                        this.axVSPrinter1.FontBold = true;
                                        if (dTotalTmp != 0 && dBnsCnt != 0)
                                        {
                                            this.axVSPrinter1.Table = sFormat + "TOTAL |" + dBnsCnt.ToString() + "|" + dTotalTmp.ToString("#,##0.00");    // AFM 20200130 added space for bin column
                                        }
                                        else if (dTotalTmp != 0 && dBnsCnt == 0)
                                        {
                                            this.axVSPrinter1.Table = sFormat + "TOTAL |" + dBnsCnt2.ToString() + "|" + dTotalTmp.ToString("#,##0.00");    // AFM 20200130 added space for bin column
                                        }
                                        else
                                        {
                                            this.axVSPrinter1.Table = sFormat + "TOTAL |" + dTotBnsTmp.ToString() + "|" + dTotal.ToString("#,##0.00");    // AFM 20200130 added space for bin column
                                        }
                                        dTotal = 0;
                                        dTotalTmp = 0;
                                        dTotBnsTmp2 = 0;
                                        dTotBnsTmp = 0;
                                        dTotBnsTmp3 = 0;
                                        dBnsCnt = 0;
                                        this.axVSPrinter1.FontBold = false;
                                        iCnt++;
                                        dBnsCnt2 = Convert.ToInt32(sBnsCnt);
                                    }
                                    else
                                    {
                                        dBnsCnt2 += Convert.ToInt32(sBnsCnt);
                                        dBnsCnt = 0;
                                        dTotBnsTmp3 = 0;
                                    }

                                    this.axVSPrinter1.FontBold = true;
                                    this.axVSPrinter1.Paragraph = "";
                                    this.axVSPrinter1.Table = "2000;" + sBrgy;
                                    this.axVSPrinter1.FontBold = false;
                                    this.axVSPrinter1.Paragraph = "";
                                }
                                else
                                {
                                    isNew = true;
                                    dTotalTmp += dTotal;
                                    dBnsCnt2 += dTotBnsTmp2;
                                }
                                dBnsCnt = dTotBnsTmp3;
                            }
                            //AFM 20200211 total per brgy (e)

                            // RMC 20150617 (s)
                            if (m_iReportSwitch == 2)  
                                result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode = 'POS') and fees_code like 'B%'";
                            // RMC 20150617 (e)
                            else if (m_iReportSwitch != 3) //AFM 20200210 uninclude business tax in barangay clearance collection
                                result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS') and fees_code like 'B%'";
                        }
                        else// if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                        {
                            sOrNo = pSet.GetString("or_no");
                            dtORDate = pSet.GetDateTime("or_date"); //MCR 20141023
                            // RMC 20140909 Migration QA (s)
                            sBIN = pSet.GetString("bin");
                            sBnsName = AppSettingsManager.GetBnsName(sBIN);

                            //AFM 20200211 total per brgy (s)
                            if (m_iReportSwitch == 3)
                            {
                                sBrgy = pSet.GetString("bns_brgy");
                                if (sBrgyTmp != sBrgy)
                                {
                                    if (isNew == false)
                                    {
                                        dTotalTmp += dTotal;
                                        this.axVSPrinter1.FontBold = true;
                                        if (dTotalTmp != 0)
                                        {
                                            this.axVSPrinter1.Table = sFormat + "TOTAL |" + "|" + "|" + "|" + dTotalTmp.ToString("#,##0.00");    // AFM 20200130 added space for bin column
                                        }
                                        else
                                        {
                                            this.axVSPrinter1.Table = sFormat + "TOTAL |" + "|" + "|" + "|" + dTotal.ToString("#,##0.00");    // AFM 20200130 added space for bin column
                                        }
                                        dTotal = 0;
                                        dTotalTmp = 0;
                                        this.axVSPrinter1.FontBold = false;
                                        iCnt++;
                                    }

                                    this.axVSPrinter1.FontBold = true;
                                    this.axVSPrinter1.Paragraph = "";
                                    this.axVSPrinter1.Table = "2000;" + sBrgy;
                                    this.axVSPrinter1.FontBold = false;
                                    this.axVSPrinter1.Paragraph = "";
                                }
                                else
                                {
                                    isNew = true;
                                    dTotalTmp += dTotal;
                                }
                            }
                            //AFM 20200211 total per brgy (e)

                            //sOrDetailsData = sOrNo + "|" + sBnsName + "|";
                            if(m_iReportSwitch == 3)
                                sOrDetailsData = sBIN + "|" + sOrNo + "|" + dtORDate.ToShortDateString() + "|"; //AFM 20200128 MAO-20-12075 added bin column
                            else
                                sOrDetailsData = sBIN + "|" + sOrNo + "|" + sBnsName + "|"; //AFM 20200128 MAO-20-12075 added bin column

                            // RMC 20140909 Migration QA (e)
                            
                            //sOrDetailsData = sOrNo + "|";
                            // RMC 20150617 (s)
                            if (m_iReportSwitch == 2)  
                                result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode = 'POS') and fees_code like 'B%'";
                            // RMC 20150617 (e)
                            else if(m_iReportSwitch != 3) //AFM 20200210 uninclude business tax in barangay clearance collection
                                result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code like 'B%'";
                        }
                        //AFM 20200211 added catch for barangay clearance fee not including bns tax
                        try
                        {
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";
                                    dbTax += result1.GetDouble("fees_due");
                                }
                        }
                        catch { }

                        result1.Close();

                        for (int i = 0; i < arrColumnCode.Count; i++)
                        {
                            if (arrColumnCode.Count == 1) //AFM 20200129 MAO-20-12093 added brackets preventing wrong condition applied due to commented code below
                            {
                                // sSOLO = arrColumnCode[i].ToString(); peb 20200102 removing the duplicating data
                            }
                            // RMC 20150617 (s)
                            if (m_iReportSwitch == 2)
                            {
                                if (m_sReportSwitch == "AbstractCollectByDaily")
                                    result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode = 'POS') and fees_code = '" + arrColumnCode[i].ToString().Trim() + "'";
                                else //if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                                    result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode = 'POS') and fees_code = '" + arrColumnCode[i].ToString().Trim() + "'";
                            }
                            else// RMC 20150617 (e)
                            {
                                if (m_sReportSwitch == "AbstractCollectByDaily")
                                {
                                    if (m_iReportSwitch == 3)
                                        result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + sBrgy.Trim() + "' and pay_hist.or_no in (select or_no from or_table where fees_code = '50')) and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS') and fees_code = '" + arrColumnCode[i].ToString().Trim() + "'"; // AFM 20200204
                                    else
                                        result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS') and fees_code = '" + arrColumnCode[i].ToString().Trim() + "'";
                                }
                                else //if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                                {
                                    if(m_iReportSwitch == 3)
                                        result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "' and pay_hist.or_no in (select or_no from or_table where fees_code = '50')) and or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code = '" + arrColumnCode[i].ToString().Trim() + "'";
                                    else
                                        result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code = '" + arrColumnCode[i].ToString().Trim() + "'";
                                }
                            }
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    if (m_iReportSwitch == 3 && m_sReportSwitch != "AbstractCollectByDaily")
                                    {
                                        sOrDetailsData += string.Format(sBnsName + "|" + "{0:#,##0.00}", result1.GetDouble("fees_due"));
                                        dTotal = result1.GetDouble("fees_due");
                                    }
                                    else if (m_iReportSwitch == 3 && m_sReportSwitch == "AbstractCollectByDaily")
                                    {
                                        sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due"));
                                        dTotal = result1.GetDouble("fees_due");
                                    }
                                    else
                                        sOrDetailsData += string.Format("{0:#,##0.00}" + "|", result1.GetDouble("fees_due"));

                                    if (i == 0)
                                        dData1 += result1.GetDouble("fees_due");
                                    else if (i == 1)
                                        dData2 += result1.GetDouble("fees_due");
                                    else if (i == 2)
                                        dData3 += result1.GetDouble("fees_due");
                                    else if (i == 3)
                                        dData4 += result1.GetDouble("fees_due");
                                    else if (i == 4)
                                        dData5 += result1.GetDouble("fees_due");
                                    else if (i == 5)
                                        dData6 += result1.GetDouble("fees_due");
                                    else if (i == 6)
                                        dData7 += result1.GetDouble("fees_due");
                                    else if (i == 7)
                                        dData8 += result1.GetDouble("fees_due");
                                    // RMC 20140909 Migration QA
                                }
                            result1.Close();
                        }

                        //sOthFee
                        // RMC 20150617 (s)
                        if (m_iReportSwitch == 2)
                        {
                            if (m_sReportSwitch == "AbstractCollectByDaily")
                                result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode = 'POS') and fees_code not in (select fees_code from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "') and fees_code not in ('B', 'B1','B2')";
                            else //if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                                result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode = 'POS') and fees_code not in (select fees_code from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "') and fees_code not in ('B', 'B1','B2')";
                        }
                        else// RMC 20150617 (e)
                        {
                            if (m_sReportSwitch == "AbstractCollectByDaily")
                            {
                                //if (m_iReportSwitch == 3)
                                   //result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "') and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS') and fees_code not in (select fees_code from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "') and fees_code not in ('B', 'B1','B2')"; //AFM 20200204
                                if(m_iReportSwitch != 3)
                                    result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS') and fees_code not in (select fees_code from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "') and fees_code not in ('B', 'B1','B2')";
                                else
                                    result1.Query = "";
                            }
                            else //if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                            {
                                //if (m_iReportSwitch != 3)
                                    //result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '"+ m_sSelectedbrgy.Trim() +"') and or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code not in (select fees_code from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "') and fees_code not in ('B', 'B1','B2')"; //AFM 20200204
                                if (m_iReportSwitch != 3)
                                    result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code not in (select fees_code from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "') and fees_code not in ('B', 'B1','B2')";
                                else
                                    result1.Query = "";
                            }
                        }
                        try
                        {
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";
                                    dOthFee += result1.GetDouble("fees_due");
                                }
                        }
                        catch { }
                        result1.Close();

                        //Tax&Fee
                        // RMC 20150617 (s)
                        if (m_iReportSwitch == 2)
                        {
                            if (m_sReportSwitch == "AbstractCollectByDaily")
                                result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode = 'POS')";
                            else //if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                                result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode = 'POS')";
                        }
                        else// RMC 20150617 (e)
                        {
                            if (m_sReportSwitch == "AbstractCollectByDaily")
                            {
                                //if (m_iReportSwitch == 3)
                                    //result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "') and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS')"; //AFM 20200204
                                if (m_iReportSwitch != 3)
                                    result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS')";
                                else
                                    result1.Query = "";
                            }
                            else //if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                            {
                                //if (m_iReportSwitch == 3)
                                    //result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '"+ m_sSelectedbrgy.Trim() +"') and or_no = '" + sOrNo + "' and data_mode <> 'POS')"; //AFM 20200204
                                if (m_iReportSwitch != 3)
                                    result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS')";
                            }
                        }
                        try
                        {
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";
                                    dTaxFees += result1.GetDouble("fees_due");
                                    dTotColl = result1.GetDouble("fees_due");
                                }
                        }
                        catch { }
                        result1.Close();

                        //Surch&Int
                        double dSurch = 0;
                        double dPen = 0;
                        // RMC 20150617 (s)
                        if (m_iReportSwitch == 2)
                        {
                            if (m_sReportSwitch == "AbstractCollectByDaily")
                                result1.Query = "select sum(fees_surch) as fees_surch, sum(fees_pen) as fees_pen from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode = 'POS')";
                            else //if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                                result1.Query = "select sum(fees_surch) as fees_surch, sum(fees_pen) as fees_pen from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode = 'POS')";
                        }
                        else// RMC 20150617 (e)
                        {
                            if (m_sReportSwitch == "AbstractCollectByDaily")
                            {
                                //if (m_iReportSwitch == 3)
                                    //result1.Query = "select sum(fees_surch) as fees_surch, sum(fees_pen) as fees_pen from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "') and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS')"; //AFM 20200205
                                if (m_iReportSwitch != 3)
                                    result1.Query = "select sum(fees_surch) as fees_surch, sum(fees_pen) as fees_pen from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS')";

                            }
                            else //if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                            {
                                //if (m_iReportSwitch == 3)
                                    //result1.Query = "select sum(fees_surch) as fees_surch, sum(fees_pen) as fees_pen from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "') and or_no = '" + sOrNo + "' and data_mode <> 'POS')"; //AFM 20200205
                                if (m_iReportSwitch != 3)
                                    result1.Query = "select sum(fees_surch) as fees_surch, sum(fees_pen) as fees_pen from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS')";
                                else
                                    result1.Query = "";
                            }
                        }
                        try
                        {
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    dSurch = result1.GetDouble("fees_surch");
                                    dPen = result1.GetDouble("fees_pen");
                                    sOrDetailsData += string.Format("{0:#,##0.00}", dSurch + dPen) + "|";
                                    dSurchInt += dSurch + dPen;
                                    dTotColl += dSurch + dPen;
                                }
                        }
                        catch { }
                        result1.Close();

                        //ExcessCQ
                        double dExcess = 0;
                        // RMC 20150617 (s)
                        if (m_iReportSwitch == 2)
                        {
                            if (m_sReportSwitch == "AbstractCollectByDaily") //JARS 20171010
                                result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode = 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and multi_pay = 'N'";
                            //result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N'"; //MCR 20141023
                            else //if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                                result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode = 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and multi_pay = 'N' and or_no = '" + sOrNo + "'";
                            //result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and or_no = '" + sOrNo + "'"; //MCR 20141023
                        }
                        else// RMC 20150617 (e)
                        {
                            if (m_sReportSwitch == "AbstractCollectByDaily") //JARS 20171010
                            {
                                //if (m_iReportSwitch == 3)
                                    //result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "') and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and multi_pay = 'N'"; //AFM 20200205
                                if (m_iReportSwitch != 3)
                                    //result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and multi_pay = 'N'";
                                    result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and multi_pay = 'N'";
                                else
                                    result1.Query = "";
                            }
                            //result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N'"; //MCR 20141023
                            else //if (m_sReportSwitch == "AbstractCollectByOR" || m_sReportSwitch == "AbstractCollectByTeller")
                            {
                                //if (m_iReportSwitch == 3)
                                    //result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "') and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and multi_pay = 'N' and or_no = '" + sOrNo + "'"; //AFM 20200205
                                if (m_iReportSwitch != 3)
                                    //result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and multi_pay = 'N' and or_no = '" + sOrNo + "'";
                                    result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and multi_pay = 'N' and or_no = '" + sOrNo + "'";
                                else
                                    result1.Query = "";
                            }
                            //result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and or_no = '" + sOrNo + "'"; //MCR 20141023
                        }
                        try
                        {
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    try
                                    { dExcess = result1.GetDouble("balance"); }
                                    catch
                                    { dExcess = 0; }
                                    sOrDetailsData += string.Format("{0:#,##0.00}", dExcess) + "|";
                                    dExcessCQ += dExcess;
                                    dTotColl += dExcess;
                                }
                        }
                        catch { }
                        result1.Close();

                        //AppTC
                        double dAppliedTC = 0;
                        //result1.Query = "select sum(fees_amtdue) as tot_fee from or_table where or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and payment_type = 'TC')";
                        // RMC 20150617 (s)
                        if (m_iReportSwitch == 2)
                            result1.Query = "select sum(fees_amtdue) as tot_fee from or_table where or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and payment_type = 'TC' and data_mode = 'POS')";
                        else
                        {
                            //if (m_iReportSwitch == 3)
                                //result1.Query = "select sum(fees_amtdue) as tot_fee from or_table where or_no in (select or_no from pay_hist where bin in (select bin from businesses where pay_hist.bin = businesses.bin and bns_brgy LIKE '" + m_sSelectedbrgy.Trim() + "') and or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and payment_type = 'TC' and data_mode <> 'POS')"; //AFM 20200205
                            if (m_iReportSwitch != 3)
                                result1.Query = "select sum(fees_amtdue) as tot_fee from or_table where or_no in (select or_no from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + "','MM/dd/yyyy') and payment_type = 'TC' and data_mode <> 'POS')";
                            else
                                result1.Query = "";
                        }
                        // RMC 20150617 (e)
                        try
                        {
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    try
                                    { dAppliedTC = result1.GetDouble("tot_fee"); }
                                    catch
                                    { dAppliedTC = 0; }
                                    sOrDetailsData += string.Format("{0:#,##0.#0}", dAppliedTC) + "|";
                                    dAppTC += dAppliedTC;
                                    dTotColl = dTotColl - dAppliedTC;
                                }
                        }
                        catch { }
                        result1.Close();

                        if(m_iReportSwitch !=3)
                            sOrDetailsData += string.Format("{0:#,##0.#0}", dTotColl) + ";";
                        else
                            sOrDetailsData += ";";
                        this.axVSPrinter1.Table = sFormat + sOrDetailsData;

                        sBrgyTmp = sBrgy;
                        isNew = false;

                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork); 
                        Thread.Sleep(3);
                    }
                if (m_iReportSwitch == 3 && m_sReportSwitch != "AbstractCollectByDaily" && iCnt > 0) //AFM 20200213 get total of last brgy
                {
                    this.axVSPrinter1.FontBold = true;
                    dTotal += dTotalTmp;
                    this.axVSPrinter1.Table = sFormat + "TOTAL |" + "|" + "|" + "|" + dTotal.ToString("#,##0.00");    // AFM 20200130 added space for bin column
                    this.axVSPrinter1.FontBold = false;
                }
                else if (m_iReportSwitch == 3 && m_sReportSwitch == "AbstractCollectByDaily" && iCnt > 0)
                {
                    this.axVSPrinter1.FontBold = true;
                    if(dBnsCnt == 0)
                        this.axVSPrinter1.Table = sFormat + "TOTAL |" + dBnsCnt2.ToString() + "|" + dTotal.ToString("#,##0.00");    // AFM 20200130 added space for bin column
                    else
                        this.axVSPrinter1.Table = sFormat + "TOTAL |" + dBnsCnt.ToString() + "|" + dTotal.ToString("#,##0.00");    // AFM 20200130 added space for bin column
                    this.axVSPrinter1.FontBold = false;
                }

                pSet.Close();

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";

                string sDefaultValue = "";

                sDefaultValue = "|" + dOthFee.ToString("#,##0.00") + "|" + dTaxFees.ToString("#,##0.00") + "|" + dSurchInt.ToString("#,##0.00") + "|" + dExcessCQ.ToString("#,##0.00") + "|" + dAppTC.ToString("#,##0.00") + "|" + Convert.ToDouble((dTaxFees + dSurchInt + dExcessCQ) - dAppTC).ToString("#,##0.00");

                if (arrColumnCode.Count == 1)
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                    {
                        if (m_iReportSwitch == 3)
                            this.axVSPrinter1.Table = sFormat + "GRAND TOTAL |" + dTotBns.ToString() + "|" + dData1.ToString("#,##0.00");    // AFM 20200130 added space for bin column
                        else
                            this.axVSPrinter1.Table = sFormat + "TOTAL |" + dTotBns.ToString() + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                    }
                    else
                    {
                        if (m_iReportSwitch == 3)
                            this.axVSPrinter1.Table = sFormat + "GRAND TOTAL |" + "|" + "|" + "|" + dData1.ToString("#,##0.00");    // AFM 20200130 added space for bin column
                        else
                            //this.axVSPrinter1.Table = sFormat + "|TOTAL |" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name
                            this.axVSPrinter1.Table = sFormat + "|TOTAL |" + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + sDefaultValue;    // AFM 20200130 added space for bin column
                    }
                else if (arrColumnCode.Count == 2)
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                         this.axVSPrinter1.Table = sFormat + "TOTAL |" + dTotBns.ToString() + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                    else
                        //this.axVSPrinter1.Table = sFormat + "|TOTAL |" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name
                        this.axVSPrinter1.Table = sFormat + "|TOTAL |" + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + sDefaultValue;    // AFM 20200130 added space for bin column
                else if (arrColumnCode.Count == 3)
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                        this.axVSPrinter1.Table = sFormat + "TOTAL |" + dTotBns.ToString() + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                    else
                        //this.axVSPrinter1.Table = sFormat + "|TOTAL |" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name
                        this.axVSPrinter1.Table = sFormat + "|TOTAL |" + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + sDefaultValue; // AFM 20200130 added space for bin column
                else if (arrColumnCode.Count == 4)
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                        this.axVSPrinter1.Table = sFormat + "TOTAL |" + dTotBns.ToString() + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                    else
                        //this.axVSPrinter1.Table = sFormat + "|TOTAL |" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name
                        this.axVSPrinter1.Table = sFormat + "|TOTAL |" + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + sDefaultValue;    // AFM 20200130 added space for bin column
                else if (arrColumnCode.Count == 5)
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                        this.axVSPrinter1.Table = sFormat + "TOTAL |" + dTotBns.ToString() + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + "|" + dData5.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                    else
                        //this.axVSPrinter1.Table = sFormat + "|TOTAL |" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + "|" + dData5.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name
                        this.axVSPrinter1.Table = sFormat + "|TOTAL |" + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + "|" + dData5.ToString("#,##0.00") + sDefaultValue;    // AFM 20200130 added space for bin column
                else if (arrColumnCode.Count == 6) //JARS 20180107
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                        this.axVSPrinter1.Table = sFormat + "TOTAL |" + dTotBns.ToString() + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + "|" + dData5.ToString("#,##0.00") + "|" + dData6.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                    else
                        //this.axVSPrinter1.Table = sFormat + "|TOTAL |" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + "|" + dData5.ToString("#,##0.00") + "|" + dData6.ToString("#,##0.00") + sDefaultValue;    // RMC 20140909 Migration QA, added space for business name
                        this.axVSPrinter1.Table = sFormat + "|TOTAL |" + "|" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + "|" + dData5.ToString("#,##0.00") + "|" + dData6.ToString("#,##0.00") + sDefaultValue;    // AFM 20200130 added space for bin column
                else if (arrColumnCode.Count == 7) //JARS 20180207
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                    //this.axVSPrinter1.Table = sFormat + "|TOTAL |" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + "|" + dData5.ToString("#,##0.00") + "|" + dData6.ToString("#,##0.00") + "|" + dData7.ToString("#,##0.00") + sDefaultValue;
                        this.axVSPrinter1.Table = sFormat + "|TOTAL |" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + "|" + dData5.ToString("#,##0.00") + "|" + dData6.ToString("#,##0.00") + "|" + dData7.ToString("#,##0.00") + sDefaultValue;
                    else
                        this.axVSPrinter1.Table = sFormat + "TOTAL | | |" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + "|" + dData5.ToString("#,##0.00") + "|" + dData6.ToString("#,##0.00") + "|" + dData7.ToString("#,##0.00") + sDefaultValue;
                else if (arrColumnCode.Count == 8)
                    this.axVSPrinter1.Table = sFormat + "TOTAL |" + dbTax.ToString("#,##0.00") + "|" + dData1.ToString("#,##0.00") + "|" + dData2.ToString("#,##0.00") + "|" + dData3.ToString("#,##0.00") + "|" + dData4.ToString("#,##0.00") + "|" + dData5.ToString("#,##0.00") + "|" + dData6.ToString("#,##0.00") + "|" + dData7.ToString("#,##0.00") + "|" + dData8.ToString("#,##0.00") + sDefaultValue;
                
                // RMC 20140909 Migration QA

                //sTotalData1 += string.Format("{0:#,##0.#0}", (dGrandTotal + dTotFeesPen + dTotFeesSurch + dTotExcess) - dTotAppliedTC) + ";";

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
                this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");

                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);


                //MCR 20190613 (s)
                if (sSOLO != "")
                {
                    if (m_sReportTitle == "ABSTRACT OF RECEIPTS OF BUSINESS TAX AND REGULATORY FEES BY DAILY AGGREGATE")
                    {
                        bSolo = true;
                        this.axVSPrinter1.NewPage();
                        string sOrDetailsData = "";
                        double dAmount = 0;
                        dTotBns = 0;

                        pSet.Query = @"select distinct(or_date) as or_date from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' order by or_date";
                        if (pSet.Execute())
                            while (pSet.Read())
                            {
                                dtORDate = pSet.GetDateTime("or_date");
                                sOrDetailsData = dtORDate.ToShortDateString() + "|";
                                // RMC 20180201 display business count in Abstract - request from Jester (Malolos) (s)
                                string sBnsCnt = GetBnsCounts(dtORDate, "");
                                sOrDetailsData += sBnsCnt + "|";
                                dTotBns += Convert.ToInt32(sBnsCnt);
                                // RMC 20180201 display business count in Abstract - request from Jester (Malolos) (e)

                                result1.Query = @"select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no 
from pay_hist where or_date = to_date('" + dtORDate.ToShortDateString() + @"','MM/dd/yyyy') and data_mode <> 'POS') and fees_code = '" + sSOLO + "'";
                                if (result1.Execute())
                                {
                                    while (result1.Read())
                                    {
                                        sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due"));
                                        dAmount += result1.GetDouble("fees_due");
                                        this.axVSPrinter1.Table = "<1000|<1000|>5200;" + sOrDetailsData;
                                    }
                                }
                                result1.Close();
                            }
                        pSet.Close();

                        this.axVSPrinter1.FontBold = true;
                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.Paragraph = "";

                        this.axVSPrinter1.Table = "<1000|<1000|>5200;" + "Total:|" + dTotBns.ToString("#,##0.00") + "|" + dAmount.ToString("#,##0.00") + "";

                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
                        this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                    }
                }
                //MCR 20190613 (s)

                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportTitle, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
            }
            catch {
                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork); // RMC 20140909 Migration QA
            }

        }

        private void ReportAbstractofCollectionByTeller()
        {
            String sQuery = "", sDataGroup = "", sDataGroup2, sDataGroup3="", sDateGen = "", sTempDataGroup="",sHeader21="";
            String sData1,sSurch_Pen, sData2, sData4, sData3, sData7, sPlatenSticker, sOtherFees, sNetTotal, sData21, sGrandTotal;
            double dData1 = 0, dSurch_Pen = 0, dData2 = 0, dData4 = 0, dData3 = 0, dData7 = 0, dPlatenSticker = 0, dOtherFees = 0, dNetTotal = 0, dData21 = 0, dGrandTotal = 0;

            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            sQuery = "SELECT data_group1,data_group3,data_group4,data_group2,data1,surch_pen,data2,data4,data3,data7,(data5 + data6) as PlatenSticker,(data17 + data18) as OtherFees,net_total,data21,sum(DATA1 + DATA2 + DATA3 + DATA4 + DATA5 + DATA6 + DATA7 + DATA18 + DATA17 + SURCH_PEN + NET_TOTAL)-(data21) as GrandTotal,DT_GEN ";
            sQuery += "FROM REPORT_ABSTRACT WHERE report_abstract.report_name = '" + m_sReportTitle + "' AND ";
            sQuery += "user_code = '" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode) + "' GROUP BY data_group1, data_group3 ,data_group4, data_group2, data1, surch_pen, data2, data4, data3, data7, (data5 + data6), (data17 + data18), net_total, data21,dt_gen";
            sQuery += " ORDER BY data_group1 ASC,data_group2 ASC";
		
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                    sDataGroup = pSet.GetString("data_group1");
                    sDataGroup3 = pSet.GetString("data_group3");
                    sHeader21 = pSet.GetString("data_group4");
                    sDataGroup2 = pSet.GetString("data_group2");
                    
                    sData1 = pSet.GetDouble("data1").ToString();
                    sSurch_Pen = pSet.GetDouble("surch_pen").ToString();
                    sData2 = pSet.GetDouble("data2").ToString();
                    sData4 = pSet.GetDouble("data4").ToString();
                    sData3 = pSet.GetDouble("data3").ToString();
                    sData7 = pSet.GetDouble("data7").ToString();
                    sPlatenSticker = pSet.GetDouble("PlatenSticker").ToString();
                    sOtherFees = pSet.GetDouble("OtherFees").ToString();
                    sNetTotal = pSet.GetDouble("net_total").ToString();
                    sData21 = pSet.GetDouble("data21").ToString();
                    sGrandTotal = pSet.GetDouble("GrandTotal").ToString();

                    dData1 += Convert.ToDouble(sData1);
                    dSurch_Pen += Convert.ToDouble(sSurch_Pen);
                    dData2 += Convert.ToDouble(sData2);
                    dData4 += Convert.ToDouble(sData4);
                    dData3 += Convert.ToDouble(sData3);
                    dData7 += Convert.ToDouble(sData7);
                    dPlatenSticker += Convert.ToDouble(sPlatenSticker);
                    dOtherFees += Convert.ToDouble(sOtherFees);
                    dNetTotal += Convert.ToDouble(sNetTotal);
                    dData21 += Convert.ToDouble(sData21);
                    dGrandTotal += Convert.ToDouble(sGrandTotal);

                    if (iReportSwitch != 2)
                    {
                        if (sTempDataGroup == "")
                        {
                            this.axVSPrinter1.FontBold = true;
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<17600;" + sTempDataGroup;
                            this.axVSPrinter1.FontBold = false;
                        }
                        else if (sDataGroup != sTempDataGroup)
                        {
                            this.axVSPrinter1.FontBold = true;
                            sTempDataGroup = sDataGroup;
                            this.axVSPrinter1.Table = "<17600;" + sDataGroup;
                            this.axVSPrinter1.FontBold = false;
                        }
                    }

                    //this.axVSPrinter1.FontSize;
                    this.axVSPrinter1.Table = @"<3600|^900|>1300|>1300|>1300|>1300|>1000|>1300|>1100|>1000|>1300|>1100|>1100;"
                      + sDataGroup3 + "|" + sDataGroup2 + "|"
                      + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sSurch_Pen).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sPlatenSticker).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sOtherFees).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sNetTotal).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sData21).ToString("#,##0.00") + "|"
                      + Convert.ToDouble(sGrandTotal).ToString("#,##0.00");
                    this.axVSPrinter1.Table = @"<4100|>13600;" + " > " + sHeader21 + "|";
                }
                pSet.Close();
            }

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<3600|^900|>1300|>1300|>1300|>1300|>1000|>1300|>1100|>1000|>1300|>1100|>1100; |TOTAL |"
                      + dData1.ToString("#,##0.00") + "|"
                      + dSurch_Pen.ToString("#,##0.00") + "|"
                      + dData2.ToString("#,##0.00") + "|"
                      + dData4.ToString("#,##0.00") + "|"
                      + dData3.ToString("#,##0.00") + "|"
                      + dData7.ToString("#,##0.00") + "|"
                      + dPlatenSticker.ToString("#,##0.00") + "|"
                      + dOtherFees.ToString("#,##0.00") + "|"
                      + dNetTotal.ToString("#,##0.00") + "|("
                      + dData21.ToString("#,##0.00") + ")|"
                      + dGrandTotal.ToString("#,##0.00");
            
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        private void ReportAbstractofCollection()
        {
            String sQuery="", sDataGroup="", sDataGroup2, sDateGen = "";
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sTotal, sSurch_Pen, sTotal_col;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dTotal = 0, dSurch_Pen = 0, dTotal_col = 0;
            double dTemptTotal = 0, dTempGrand = 0;

            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            OracleResultSet pSet = new OracleResultSet();
            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            
            sQuery = @"SELECT Count(*) FROM REPORT_ABSTRACT report_abstract WHERE report_name = '" + m_sReportTitle + "' AND user_code = '" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode) + "' ORDER BY data_group1 ASC";
            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            if (m_sReportSwitch == "AbstractCollectByDaily")
            {
                sQuery = @"SELECT to_date(data_group1,'MM/dd/yyyy') as data_group1,data_group2, data1, data2, data3, data4, data5, data6, data7, data8, data9, data10, data11, data12, data13, data14, data15, data16, surch_pen, total_col, dt_gen ";
                sQuery += "FROM REPORT_ABSTRACT report_abstract WHERE report_name = '" + m_sReportTitle + "' AND user_code = '" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode) + "' ORDER BY data_group1 desc";
            }
            else if (m_sReportSwitch == "AbstractCollectByOR")
            {
                sQuery = @"SELECT data_group1,data_group2, data1, data2, data3, data4, data5, data6, data7, data8, data9, data10, data11, data12, data13, data14, data15, data16, surch_pen, total_col, dt_gen ";
                sQuery += "	FROM REPORT_ABSTRACT report_abstract WHERE report_name = '" + m_sReportTitle + "' AND user_code = '" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode) + "' ORDER BY data_group1 ASC";
            }
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                        sDataGroup = pSet.GetDateTime("data_group1").ToShortDateString();
                    else if (m_sReportSwitch == "AbstractCollectByOR")
                        sDataGroup = pSet.GetString("data_group1");
                    else if (m_sReportSwitch == "AbstractCollectByTeller")
                        sDataGroup = pSet.GetString("data_group1");
                    sDataGroup2 = pSet.GetString("data_group2");
                    sData1 = pSet.GetDouble("data1").ToString();
                    sData2 = pSet.GetDouble("data2").ToString();
                    sData3 = pSet.GetDouble("data3").ToString();
                    sData4 = pSet.GetDouble("data4").ToString();
                    sData5 = pSet.GetDouble("data5").ToString();
                    sData6 = pSet.GetDouble("data6").ToString();
                    sData7 = pSet.GetDouble("data7").ToString();
                    sData8 = pSet.GetDouble("data8").ToString();
                    sData9 = pSet.GetDouble("data9").ToString();
                    sData10 = pSet.GetDouble("data10").ToString();
                    sData11 = pSet.GetDouble("data11").ToString();
                    sData12 = pSet.GetDouble("data12").ToString();
                    sData13 = pSet.GetDouble("data13").ToString();
                    sData14 = pSet.GetDouble("data14").ToString();
                    sData15 = pSet.GetDouble("data15").ToString();
                    sData16 = pSet.GetDouble("data16").ToString();
                    sSurch_Pen = pSet.GetDouble("surch_pen").ToString();

                    dTemptTotal = pSet.GetDouble("data1") + pSet.GetDouble("data2") + pSet.GetDouble("data3") + pSet.GetDouble("data4") + pSet.GetDouble("data5")
                        + pSet.GetDouble("data6") + pSet.GetDouble("data7") + pSet.GetDouble("data8") + pSet.GetDouble("data9") + pSet.GetDouble("data10") + pSet.GetDouble("data11")
                        + pSet.GetDouble("data12") + pSet.GetDouble("data13") + pSet.GetDouble("data14") + pSet.GetDouble("data15") + pSet.GetDouble("data16");
                    
                    dTempGrand = (dTemptTotal + pSet.GetDouble("surch_pen"));

                    sTotal = dTemptTotal.ToString();
                    sTotal_col = dTempGrand.ToString();

                    dData1 += Convert.ToDouble(sData1);
                    dData2 += Convert.ToDouble(sData2);
                    dData3 += Convert.ToDouble(sData3);
                    dData4 += Convert.ToDouble(sData4);
                    dData5 += Convert.ToDouble(sData5);
                    dData6 += Convert.ToDouble(sData6);
                    dData7 += Convert.ToDouble(sData7);
                    dData8 += Convert.ToDouble(sData8);
                    dData9 += Convert.ToDouble(sData9);
                    dData10 += Convert.ToDouble(sData10);
                    dData11 += Convert.ToDouble(sData11);
                    dData12 += Convert.ToDouble(sData12);
                    dData13 += Convert.ToDouble(sData13);
                    dData14 += Convert.ToDouble(sData14);
                    dData15 += Convert.ToDouble(sData15);
                    dData16 += Convert.ToDouble(sData16);
                    dTotal += Convert.ToDouble(sTotal);
                    dSurch_Pen += Convert.ToDouble(sSurch_Pen);
                    dTotal_col += Convert.ToDouble(sTotal_col);
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                    {
                        this.axVSPrinter1.Table = @"^900|>900|>900|>900|>900|>900|>1150|>1000|>1000|>1000|>1200|>1000|>1100|>1100|>1250|>900|>1450|>900|>900|>900;"
                          + sDataGroup + "|" + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotal).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sSurch_Pen).ToString("#,##0.00") + "|"
                          + Convert.ToDouble(sTotal_col).ToString("#,##0.00");
                    }
                    else if (m_sReportSwitch == "AbstractCollectByOR")
                    {
                        this.axVSPrinter1.Table = @"^900|^1300|>900|>900|>900|>900|>900|>1150|>1000|>1000|>1000|>1200|>1000|>1100|>1100|>1250|>900|>1450|>900|>900|>900;"
                              + sDataGroup + "|" + sDataGroup2 +  "|" 
                              + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sTotal).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sSurch_Pen).ToString("#,##0.00") + "|"
                              + Convert.ToDouble(sTotal_col).ToString("#,##0.00");
                    }

                    this.axVSPrinter1.Paragraph = "";

                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
                pSet.Close();
            }

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            if (m_sReportSwitch == "AbstractCollectByDaily")
            {
                this.axVSPrinter1.Table = "^900|>900|>900|>900|>900|>900|>1150|>1000|>1000|>1000|>1200|>1000|>1100|>1100|>1250|>900|>1450|>900|>900|>900; TOTAL |"
                    + dData1.ToString("#,##0.00") + "|"
                    + dData2.ToString("#,##0.00") + "|"
                    + dData3.ToString("#,##0.00") + "|"
                    + dData4.ToString("#,##0.00") + "|"
                    + dData5.ToString("#,##0.00") + "|"
                    + dData6.ToString("#,##0.00") + "|"
                    + dData7.ToString("#,##0.00") + "|"
                    + dData8.ToString("#,##0.00") + "|"
                    + dData9.ToString("#,##0.00") + "|"
                    + dData10.ToString("#,##0.00") + "|"
                    + dData11.ToString("#,##0.00") + "|"
                    + dData12.ToString("#,##0.00") + "|"
                    + dData13.ToString("#,##0.00") + "|"
                    + dData14.ToString("#,##0.00") + "|"
                    + dData15.ToString("#,##0.00") + "|"
                    + dData16.ToString("#,##0.00") + "|"
                    + dTotal.ToString("#,##0.00") + "|"
                    + dSurch_Pen.ToString("#,##0.00") + "|"
                    + dTotal_col.ToString("#,##0.00");
            }
            else if (m_sReportSwitch == "AbstractCollectByOR")
            {
                this.axVSPrinter1.Table = "^900|^1300|>900|>900|>900|>900|>900|>1150|>1000|>1000|>1000|>1200|>1000|>1100|>1100|>1250|>900|>1450|>900|>900|>900; |TOTAL |"
                        + dData1.ToString("#,##0.00") + "|"
                        + dData2.ToString("#,##0.00") + "|"
                        + dData3.ToString("#,##0.00") + "|"
                        + dData4.ToString("#,##0.00") + "|"
                        + dData5.ToString("#,##0.00") + "|"
                        + dData6.ToString("#,##0.00") + "|"
                        + dData7.ToString("#,##0.00") + "|"
                        + dData8.ToString("#,##0.00") + "|"
                        + dData9.ToString("#,##0.00") + "|"
                        + dData10.ToString("#,##0.00") + "|"
                        + dData11.ToString("#,##0.00") + "|"
                        + dData12.ToString("#,##0.00") + "|"
                        + dData13.ToString("#,##0.00") + "|"
                        + dData14.ToString("#,##0.00") + "|"
                        + dData15.ToString("#,##0.00") + "|"
                        + dData16.ToString("#,##0.00") + "|"
                        + dTotal.ToString("#,##0.00") + "|"
                        + dSurch_Pen.ToString("#,##0.00") + "|"
                        + dTotal_col.ToString("#,##0.00");
            }

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        private void ReportofAbstractofChecks()
        {
            Boolean bIsNeg = false; // temp code to fix negative values in applied ammount field
            String sQuery, sDate, sMonth, sYear, sDay, sAmount, sBlank, sTeller;
            Boolean bFirstPrint = false;

            String sGrandTotalAmountPaid, sGrandTotalExcess = "0.00", sTotalCollected = "0.00";
            String sTotalNoOfChecks, sTotalAmountOfChecks = "0.00", sTotalTaxDue;
            int iTotalNoOfChecks = 0;
            double dTotalAmountOfChecks = 0, dTotalTaxDue = 0;
            double dTotalCheck, dTotalCheckMult, dTotalCheckSingle, dTotalMultiple, dTotalSingle, dTotalCollected;

            String sHeader, sHeader1, sHeader2, sHeader3, sBody, sTaxDue, sBsc, sSef;
            String sDateFrom, sMonthFrom, sYearFrom, sDayFrom, sMonthFrom2;
            String sDateTo, sMonthTo, sYearTo, sDayTo;
            String sCheckNo, sCheckDate, sBankCode, sOrNo = "", sCheckAmount, sBank = "";
            String sTotalChkAmt;
            DateTime odtDate;

            int iDay, iMonth, iYear;  //LGF 01132005 abstract of check -- add iCtr, iCount

            sTeller = AppSettingsManager.GetTeller(m_sTeller, 0);

            sGrandTotalAmountPaid = "";
            sGrandTotalExcess = "";

            // Out to VSPrinter
            this.axVSPrinter1.FontName = "Times New Roman";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontSize = 8;

            //SetHeading();
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            if (m_sReportSwitch == "Report_of_Abstract_of_Checks") //ABSTRACT OF CHECKS
            {
                String sCCheckNo, sCCheckDate, sCBankCode, sCTeller, sCCheckAmount, sCheckAmount2 = "0.00";
                String sExcessSingle = "", sExcessMultiple = "0.00", sTotalExcess, sCheckAmount3 = "0.00", sCheckAmount4;
                double dExcessSingle = 0, dExcessMultiple, dTotalExcess, dCheckAmount2;
                double dTotalExcesS, dTotalExcessM;
                String sTempCheckNo, sTempCheckDate, sTempBank, sTempCheckAmount, sTempBankCode = "";
                String sTempBankCode2 = "";
                String sTempOrNo;
                String sTotalExcessM = "0.00", sTotalExcesS = "0.00";
                bool bSameCheck = false; //JARS 20180618

                //(s) RMC 01242005
                ArrayList arr_sArrchk = new ArrayList();	//sArrchk[3];
                ArrayList arr_sArrN = new ArrayList();
                ArrayList arr_sArrchkAmt = new ArrayList();

                int iChkCtr, iCtr, iPrint;
                String sChkNo, sChkAmt;

                string strRefOrNo = string.Empty; // AST 20160128
                string strRefOrNoTmp = string.Empty; // AST 20160128

                OracleResultSet pSet = new OracleResultSet();
                OracleResultSet pSet2 = new OracleResultSet();
                OracleResultSet pSet3 = new OracleResultSet();
                OracleResultSet pSet4 = new OracleResultSet();

                sQuery = "select * from multi_check_pay where teller = '" + m_sTeller + "' ";
                sQuery += "and chk_no in (select chk_no from chk_tbl where or_date >= to_date('" + m_dtFrom.Date.ToShortDateString() + "','MM/dd/yyyy') and or_date <= to_date('" + m_dtTo.Date.ToShortDateString() + "','MM/dd/yyyy'))";
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sChkNo = pSet.GetString("chk_no");
                        sChkAmt = pSet.GetDouble("chk_amt").ToString();
                        arr_sArrchk.Add(sChkNo); //0
                        arr_sArrN.Add("N");    //1
                        arr_sArrchkAmt.Add(sChkAmt);//2
                    }
                }
                pSet.Close();
                iCtr = arr_sArrchk.Count;

                #region Progress
                int intCount = 0;
                int intCountIncreament = 0;
                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();

                Thread.Sleep(500);

                sQuery = "select Count(*) from chk_tbl where teller = '" + m_sTeller + "' ";
                sQuery += "and or_date >= to_date('" + m_dtFrom.Date.ToShortDateString() + "','MM/dd/yyyy') and or_date <= to_date('" + m_dtTo.Date.ToShortDateString() + "','MM/dd/yyyy') ";
                sQuery += "order by teller, or_no";


                pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);

                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
                #endregion

                sQuery = "select * from chk_tbl where teller = '" + m_sTeller + "' ";
                sQuery += "and or_date >= to_date('" + m_dtFrom.Date.ToShortDateString() + "','MM/dd/yyyy') and or_date <= to_date('" + m_dtTo.Date.ToShortDateString() + "','MM/dd/yyyy') ";
                sQuery += "order by teller, or_no";


                pSet2.Query = sQuery;
                if (pSet2.Execute())
                {
                    sTempCheckNo = "";
                    sTempOrNo = "";
                    sTeller = "";
                    iTotalNoOfChecks = 0;
                    dTotalExcess = 0;
                    dTotalMultiple = 0;
                    dTotalCheckMult = 0;
                    dTotalCheckSingle = 0;
                    dTotalCheck = 0;
                    dTotalMultiple = 0;
                    dTotalSingle = 0;
                    dTotalCollected = 0;
                    dTotalExcesS = 0;
                    dTotalExcessM = 0;
                    iChkCtr = 0;
                    iPrint = 1;

                    while (pSet2.Read())
                    {
                        iTotalNoOfChecks++;
                        bSameCheck = false;
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        this.axVSPrinter1.FontBold = true;
                        if (sTeller == AppSettingsManager.GetTeller(pSet2.GetString("teller").Trim(), 0))
                            this.axVSPrinter1.Table = ("<10800;");
                        else
                        {
                            sTeller = AppSettingsManager.GetTeller(pSet2.GetString("teller").Trim(), 0);
                            this.axVSPrinter1.Table = ("<10800;;" + sTeller + ";;");
                        }
                        this.axVSPrinter1.FontBold = false;

                        sCheckNo = pSet2.GetString("chk_no");
                        sCheckDate = pSet2.GetDateTime("chk_date").ToShortDateString();
                        sBankCode = pSet2.GetString("bank_code");
                        sOrNo = pSet2.GetString("or_no");
                        sCheckAmount = pSet2.GetDouble("chk_amt").ToString();

                        //sCheckAmount.Format("%0.2f",atof(sCheckAmount));	// RTL 20070326

                        //if (sOrNo == "BAT-005390")
                        //    sOrNo = sOrNo;

                        sQuery = "select * from bank_table where bank_code = '" + sBankCode + "' ";
                        pSet3.Query = sQuery;
                        if (pSet3.Execute())
                            if (pSet3.Read())
                                sBank = pSet3.GetString("bank_nm");
                        pSet3.Close();

                    Again: // AST 20160128
                        if(bSameCheck) //JARS 20180618 AVOID MULTIPLE ROWS OF CHECK AMOUNT OF THE SAME CHECK
                        {
                            sCheckAmount = "0";
                        }

                        //if (sTempCheckNo != sCheckNo)
                        if (sTempCheckNo != sCheckNo || sTempBankCode != sBankCode) //AFM 20200515 added condition for diferrent bank same check no.
                        {
                            //sQuery = "select sum(chk_amt) as sum from chk_tbl where chk_no = '" + sCheckNo + "' ";
                            sQuery = "select sum(chk_amt) as sum from chk_tbl where chk_no = '" + sCheckNo + "' and bank_code = '"+ sBankCode +"' "; //AFM 20200506 added bank code
                            pSet3.Query = sQuery; 
                            if (pSet3.Execute())
                                if (pSet3.Read())
                                    sCheckAmount3 = pSet3.GetDouble("sum").ToString();
                            pSet3.Close();
                            // used for computing excess (e)

                            //sQuery = "select * from multi_check_pay where chk_no = '" + sCheckNo + "' and used_sw = 'Y' order by chk_no";
                            sQuery = "select * from multi_check_pay where chk_no = '" + sCheckNo + "' and used_sw = 'Y' and bank_code = '" + sBankCode + "' order by chk_no";
                            pSet3.Query = sQuery;
                            if (pSet3.Execute())
                            {
                                if (pSet3.Read())
                                {
                                    for (int i = 0; i < iCtr; i++)
                                    {
                                        if (sCheckNo == arr_sArrchk[i].ToString() && arr_sArrN[i].ToString() == "N")
                                        {
                                            arr_sArrN[i] = "Y";
                                            iChkCtr = i;
                                            iPrint = 1;
                                            i = iCtr + 1;
                                        }
                                    }
                                    //(e) RMC 01242005
                                    sCCheckAmount = pSet3.GetDouble("chk_amt").ToString();
                                    if (arr_sArrN[iChkCtr].ToString() == "Y" && iPrint == 1)
                                    {
                                        bFirstPrint = false;
                                        dTotalMultiple += Convert.ToDouble(sCheckAmount);
                                        dTotalCheckMult += Convert.ToDouble(sCCheckAmount);

                                        sTempCheckNo = sCheckNo;
                                        sTempCheckDate = sCheckDate;
                                        sTempBank = sBank;
                                        sTempBankCode = sBankCode; //AFM 202005515
                                        sTempCheckAmount = sCCheckAmount;
                                        dExcessMultiple = Convert.ToDouble(sCCheckAmount) - Convert.ToDouble(sCheckAmount3);
                                        sExcessMultiple = dExcessMultiple.ToString();

                                        sBody = " <1100|<1000|<3000|<1200|>1500|>1500|>1500;";
                                        sBody += sTempCheckNo.Trim() + "|";
                                        sBody += sTempCheckDate + "|";
                                        sBody += sTempBank + "|";
                                        sBody += sOrNo + "|";
                                        sBody += Convert.ToDouble(sTempCheckAmount).ToString("#,##0.00") + "|";
                                        sBody += Convert.ToDouble(sCheckAmount).ToString("#,##0.00") + "|";
                                        sBody += Convert.ToDouble(sExcessMultiple).ToString("#,##0.00") + ";";	//RMC 01192005 put remarks

                                        this.axVSPrinter1.Table = sBody;

                                        dTotalExcessM += Convert.ToDouble(sExcessMultiple);
                                        sTotalExcessM = dTotalExcessM.ToString("#,##0.00");

                                        iPrint++;
                                    }
                                    else
                                    {
                                        String sCheck = "";
                                        for (int i = 0; i < iCtr; i++)
                                        {
                                            if (sCheckNo == arr_sArrchk[i].ToString() && arr_sArrN[i].ToString() == "Y")
                                            {
                                                sCheck = arr_sArrchkAmt[i].ToString();
                                                i = iCtr + 1;
                                            }
                                        }
                                        iTotalNoOfChecks--;
                                        String sNone = "(          )";
                                        sBody = " <1100|<1000|<3000|<1200|>1500|>1500|>1500;";

                                        if (bFirstPrint == false)
                                            sBody += "( " + sCheckNo.Trim() + " )" + "|";
                                        else
                                            sBody += " |";

                                        sBody += sCheckDate + "|";
                                        sBody += sBank + "|";
                                        sBody += sOrNo + "|";
                                        if (bFirstPrint == false)
                                            sBody += "( " + Convert.ToDouble(sCheck).ToString("#,##0.00") + " )" + "|";
                                        else
                                            sBody += " |";
                                        sBody += Convert.ToDouble(sCheckAmount).ToString("#,##0.00") + "|";

                                        this.axVSPrinter1.Table = sBody;
                                        dTotalSingle += Convert.ToDouble(sCheckAmount);
                                        bFirstPrint = true;
                                    }
                                }
                                else
                                {
                                    //    if(sOrNo == "BAT-005390")
                                    //        sOrNo = sOrNo;

                                    dCheckAmount2 = 0.00;
                                    // get total amt paid from or_table
                                    sQuery = "select sum(fees_amtdue) from or_table where or_no = '" + sOrNo + "' ";
                                    pSet4.Query = sQuery;
                                    if (pSet4.Execute())
                                        if (pSet4.Read())
                                            dCheckAmount2 = pSet4.GetDouble(0);
                                    pSet4.Close();

                                    // check if or of this check has another check for multiple trans
                                    sQuery = "select * from chk_tbl where or_no = '" + sOrNo + "' ";
                                    sQuery += "and chk_no in (select chk_no from multi_check_pay)";
                                    pSet4.Query = sQuery;
                                    if (pSet4.Execute())
                                        if (pSet4.Read())
                                            dCheckAmount2 = Convert.ToDouble(sCheckAmount);
                                    pSet4.Close();

                                    double dCheckAmts = 0, dCheckAmount6 = 0, dCheckAmount3 = 0, dCheckAmount4 = 0 ;
                                    //GET TOTAL AMOUT APPLIED FOR CHECK
                                    dCheckAmount3 = Convert.ToDouble(sCheckAmount3);
                                    dCheckAmount4 = Convert.ToDouble(sCheckAmount3);
                                    if (dCheckAmount2 >= dCheckAmount3)
                                    {
                                        // get total checks from the same or
                                        sQuery = "select sum(chk_amt) from chk_tbl where or_no = '" + sOrNo + "'";

                                        pSet4.Query = sQuery;
                                        if (pSet4.Execute())
                                            while (pSet4.Read())
                                            {
                                                dCheckAmts += pSet4.GetDouble(0);
                                            }
                                        pSet4.Close();

                                        dCheckAmount6 += dCheckAmts;
                                        // get total cash from the same or
                                        sQuery = "select sum(cash_amt) from chk_tbl where or_no = '" + sOrNo + "'";
                                        pSet4.Query = sQuery;
                                        if (pSet4.Execute())
                                        {
                                            dCheckAmts = 0;
                                            while (pSet4.Read())
                                            {
                                                dCheckAmts += pSet4.GetDouble(0);
                                            }
                                            //temp code to fix negative values in applied ammount field
                                            bIsNeg = false;
                                            if ((dCheckAmount2 - dCheckAmts) >= 0)
                                                dCheckAmount2 -= dCheckAmts;
                                            else
                                                bIsNeg = true;
                                        }
                                        pSet4.Close();

                                        // get total tax credit used from the same or
                                        sQuery = "select sum(debit) from dbcr_memo where or_no = '" + sOrNo + "' and served = 'Y' and multi_pay = 'N' and memo like 'DEBIT%'"; //JARS 20171010
                                        pSet4.Query = sQuery;
                                        if (pSet4.Execute())
                                        {
                                            dCheckAmts = 0;
                                            while (pSet4.Read())
                                            {
                                                dCheckAmts += pSet4.GetDouble(0);
                                            }
                                            dCheckAmount2 -= dCheckAmts;
                                        }
                                        pSet4.Close();

                                        // get total tax credit excess from the same or
                                        sQuery = "select sum(credit) from dbcr_memo where or_no = '" + sOrNo + "' and served = 'Y' and multi_pay = 'N' and memo like 'CREDIT%' and chk_no <> '" + sCheckNo + "'"; //JARS 20171010
                                        pSet4.Query = sQuery;
                                        if (pSet4.Execute())
                                        {
                                            dCheckAmts = 0;
                                            while (pSet4.Read())
                                            {
                                                dCheckAmts += pSet4.GetDouble(0);
                                            }
                                            dCheckAmount2 += dCheckAmts;
                                        }
                                        pSet4.Close();
                                    }
                                    else
                                    {
                                        sQuery = "select * from dbcr_memo where chk_no = '" + sCheckNo + "'"; //JARS 20171010
                                        sQuery += " and memo like 'CREDIT%' and multi_pay = 'N'";
                                        sQuery += " and or_date = to_date('" + sCheckDate + "','MM/dd/yyyy')";

                                        pSet4.Query = sQuery;
                                        if (pSet4.Execute())
                                            if (pSet4.Read())
                                            {
                                                if (dCheckAmount3 == dCheckAmount2)
                                                {
                                                    dCheckAmount3 = dCheckAmount2;
                                                    dCheckAmount3 -= pSet4.GetDouble("credit");
                                                    dCheckAmount6 = dCheckAmount2;
                                                    dCheckAmount2 = dCheckAmount3;
                                                }
                                                else
                                                {
                                                    dCheckAmount3 = dCheckAmount2;
                                                    dCheckAmount3 += pSet4.GetDouble("credit");
                                                    dCheckAmount6 = dCheckAmount3;
                                                }
                                            }
                                            else
                                                dCheckAmount6 = dCheckAmount3;
                                        pSet4.Close();

                                        sQuery = "select nvl(sum(debit),0) from dbcr_memo where or_no = '" + sOrNo + "' and served = 'Y' and multi_pay = 'N' and memo like 'DEBIT%'"; //JARS 20171010
                                        pSet4.Query = sQuery;
                                        if (pSet4.Execute())
                                        {
                                            dCheckAmts = 0;
                                            while (pSet4.Read())
                                            {
                                                dCheckAmts += pSet4.GetDouble(0);
                                            }
                                            dCheckAmount2 = dCheckAmount2 - dCheckAmts;
                                        }
                                        pSet4.Close();
                                    }

                                    sCheckAmount2 = dCheckAmount2.ToString();
                                    //sTempCheckNo = sCheckNo; test
                                    sTempCheckAmount = sCheckAmount;
                                    bool isMulti = false;
                                    if (sTempOrNo != sOrNo)
                                    {
                                        //temp code to fix negative values in applied ammount field

                                        if (!bIsNeg)
                                        {
                                            //MCR 20141021
                                            //dExcessSingle = dCheckAmount6 - dCheckAmount2;
                                            for (int i = 0; i < arr_sArrchk.Count; i++)
                                                if (arr_sArrchk[i].ToString() == sCheckNo)
                                                {
                                                    dCheckAmount3 = dCheckAmount4; // AFM 20200506
                                                    dExcessSingle = Convert.ToDouble(arr_sArrchkAmt[i].ToString()) - dCheckAmount3;
                                                    sTempCheckAmount = arr_sArrchkAmt[i].ToString();
                                                    isMulti = true;
                                                }
                                                //else
                                                    //dExcessSingle = Convert.ToDouble(sCheckAmount) - dCheckAmount2; //AFM 20200505 removed to properly get excess check
                                        }
                                        else //JARS 20170905 //AFM 20200505 previously commented
                                            dExcessSingle = Convert.ToDouble(sCheckAmount) - dCheckAmount2;
                                        if (!isMulti) //TEST
                                            dExcessSingle = dCheckAmount2 - dCheckAmount3;

                                        // AFM 20200729 MAO-20-13374 condition for multi pay cheque - only display excess check to last attached (s)
                                        if (arr_sArrchk.Count > 1 && sCheckNo == arr_sArrchk[0].ToString())
                                        {
                                            if (sTempCheckNo == sCheckNo && sTempBankCode2 == sBankCode)
                                                sExcessSingle = dExcessSingle.ToString("#,##0.00");
                                            else
                                            {
                                                dExcessSingle = 0;
                                                sExcessSingle = dExcessSingle.ToString("#,##0.00");
                                            }

                                        }
                                        // AFM 20200729 MAO-20-13374 condition for multi pay cheque - only display excess check to last attached (e)
                                        else
                                            sExcessSingle = dExcessSingle.ToString("#,##0.00");

                                    }
                                    else
                                    {
                                        dExcessSingle = 0;
                                        sExcessSingle = dExcessSingle.ToString("#,##0.00");
                                    }

                                    // AST 20160203 Correction in getting balance chk amt if multi OR used (s)
                                    string strORNoTmp = "";
                                    strORNoTmp = AppSettingsManager.GetOtherOrNumber(sOrNo, "").Trim();
                                    OracleResultSet rs = new OracleResultSet();
                                    if (!string.IsNullOrEmpty(strORNoTmp.Trim()))
                                    {
                                        rs.Query = string.Format("select sum(fees_amtdue) from or_table where or_no = '{0}' ", strORNoTmp);
                                        if (rs.Execute())
                                        {
                                            if (rs.Read())
                                            {
                                                dExcessSingle = dExcessSingle - rs.GetDouble(0);
                                                
                                                if (dExcessSingle < 0)
                                                    dExcessSingle = 0;

                                                sExcessSingle = dExcessSingle.ToString("#,##0.00");
                                            }
                                        }
                                        rs.Close();
                                    }
                                    // AST 20160203 Correction in getting balance chk amt if multi OR used (e)

                                    sBody = " <1100|<1000|<3000|<1200|>1500|>1500|>1500;";
                                    sBody += sCheckNo.Trim() + "|";
                                    sBody += sCheckDate + "|";
                                    sBody += sBank + "|";

                                    // (s) RMC 01282005 for multiple check payment in single or
                                    if (sTempOrNo != sOrNo)
                                    {
                                        dTotalSingle += Convert.ToDouble(sCheckAmount2);
                                        sBody += sOrNo + "|";
                                        sBody += Convert.ToDouble(sTempCheckAmount).ToString("#,##0.00") + "|";
                                        sBody += Convert.ToDouble(sCheckAmount2).ToString("#,##0.00") + "|";
                                        sTempOrNo = sOrNo;
                                    }
                                    else
                                    {
                                        if (Convert.ToDouble(sCheckAmount2) != Convert.ToDouble(sTempCheckAmount))
                                            sCheckAmount2 = "0.00";

                                        dTotalSingle += Convert.ToDouble(sCheckAmount2);
                                        sBody += "|";
                                        sBody += Convert.ToDouble(sTempCheckAmount).ToString("#,##0.00") + "|";
                                        sBody += sCheckAmount2 + "|";
                                        sBody += "|";

                                    }

                                    sBody += Convert.ToDouble(sExcessSingle).ToString("#,##0.00") + ";";
                                    this.axVSPrinter1.Table = sBody;

                                    dTotalCheckSingle += Convert.ToDouble(sTempCheckAmount);
                                    dTotalExcesS += Convert.ToDouble(sExcessSingle);
                                    sTotalExcesS = dTotalExcesS.ToString();
                                }
                                pSet3.Close();
                            }                            
                        }
                        else
                        {
                            sBody = " <1100|<1000|<3000|<1200|>1500|>1500|>1500;";
                            sBody += "|";
                            sBody += "|";
                            sBody += "|";
                            sBody += sOrNo + "|";
                            sBody += "|";
                            sBody += Convert.ToDouble(sCheckAmount).ToString("#,##0.00") + "|";
                            this.axVSPrinter1.Table = sBody;
                            iTotalNoOfChecks--;
                            dTotalMultiple += Convert.ToDouble(sCheckAmount);
                        }

                        dTotalCollected = dTotalMultiple + dTotalSingle;
                        sTotalCollected = dTotalCollected.ToString();

                        sTempCheckNo = sCheckNo; //test
                        sTempBankCode2 = sBankCode;
                        
                        // AST 20160128 added checking and getting of other or no. if multi or used. (s)
                        strRefOrNoTmp = sOrNo;
                        sOrNo = AppSettingsManager.GetOtherOrNumber(sOrNo, strRefOrNo).Trim();
                        if (!string.IsNullOrEmpty(sOrNo))
                        {
                            strRefOrNo = strRefOrNoTmp;
                            sTempCheckNo = ""; // Make it empty to allow same check
                            bSameCheck = true;
                            goto Again;
                        }
                        // AST 20160128 added checking and getting of other or no. if multi or used. (e)

                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                    }
                    pSet2.Close();


                    DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                    Thread.Sleep(10);

                    dTotalCheck = dTotalCheckMult + dTotalCheckSingle;
                    sTotalAmountOfChecks = dTotalCheck.ToString();
                    dTotalExcess += Convert.ToDouble(sTotalExcessM) + Convert.ToDouble(sTotalExcesS);
                    sTotalExcess = dTotalExcess.ToString();
                    sGrandTotalExcess = sTotalExcess;                    
                }

            }

            sTotalTaxDue = dTotalTaxDue.ToString();
            sTotalNoOfChecks = iTotalNoOfChecks.ToString();

            this.axVSPrinter1.FontSize = 9;
            this.axVSPrinter1.FontBold = true;

            this.axVSPrinter1.Table = (";;");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            this.axVSPrinter1.Table = ("<5100|>1200|>1500|>1500|>1500;Total Number of Checks:  " + sTotalNoOfChecks + "|" + "TOTAL|" + Convert.ToDouble(sTotalAmountOfChecks).ToString("#,##0.00") + "|" + Convert.ToDouble(sTotalCollected).ToString("#,##0.00") + "|" + Convert.ToDouble(sGrandTotalExcess).ToString("#,##0.00"));

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontSize = 8;
            this.axVSPrinter1.FontBold = false;
            if (m_sTeller != "ALL")
                this.axVSPrinter1.Table = (">7000|>3000;;;;;Signature:|__________________________");

            this.axVSPrinter1.FontSize = 8;

            this.axVSPrinter1.Table = "<2500|<8800;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<8800;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void ListOfCancelledChecks()
        {
            String sQuery, sFromDate, sToDate, sCurrentUser, sTellerCode = String.Empty;
            String sChkTeller, sChkNo, sChkDate, sChkBankCode, sChkAcctNo, sChkAcctName, sChkOrNo, sChkOrDate, sChkDtAccept, sChkMemo, sChkBin, sChkDateCancelled;
            double dChkAmount = 0, dCashAmount = 0;
            int iCtr = 0, iProgressCtr = 0;

            //mc_vspReports.SetOrientation(0); this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            sQuery = string.Format(@"SELECT heading, bin, teller, chk_no, chk_date, acct_no, acct_name, or_no, or_date, chk_amt, cash_amt, memo, date_posted, dt_from, dt_to, user_name, report_name 
            FROM REP_CANCEL_CHK WHERE report_name = '{0}' and user_code = '{1}' ORDER BY rep_cancel_chk.date_posted ASC", m_sReportTitle, StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sChkDateCancelled = Convert.ToDateTime(pSet.GetString("date_posted")).ToString("MM/dd/yyyy");
                    sChkBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                    sChkNo = StringUtilities.HandleApostrophe(pSet.GetString("chk_no"));
                    sChkDate = Convert.ToDateTime(pSet.GetString("chk_date")).ToString("MM/dd/yyyy");
                    sChkAcctNo = StringUtilities.HandleApostrophe(pSet.GetString("acct_no"));
                    sChkAcctName = StringUtilities.HandleApostrophe(pSet.GetString("acct_name"));
                    sChkOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                    sChkOrDate = Convert.ToDateTime(pSet.GetString("or_date")).ToString("MM/dd/yyyy");
                    dChkAmount = pSet.GetDouble("chk_amt");
                    dCashAmount = pSet.GetDouble("cash_amt");
                    sChkTeller = StringUtilities.HandleApostrophe(pSet.GetString("teller"));
                    sChkMemo = StringUtilities.HandleApostrophe(pSet.GetString("memo"));
                                             //^1000|^2100|^1200|^1000|^1300|^3000|^1000|^1000|^2200|^2200|^1200|^2300
                    this.axVSPrinter1.Table = "^1100|<2100|<1200|<1100|<1300|<3000|<1000|^1100|>1750|>1750|<1200|<2100;" + sChkDateCancelled + "|" + sChkBin + "|" + sChkNo + "|" + sChkDate + "|" + sChkAcctNo + "|" + sChkAcctName + "|" + sChkOrNo + "|" + sChkOrDate + "|" + dChkAmount.ToString("#,##0.00") + "|" + dCashAmount.ToString("#,##0.00") + "|" + sChkTeller + "|" + sChkMemo;
                                                //1    2     3       4    5      6   7     8    9    10    11    12   
                }
            }
            pSet.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void NegativeList()
        {
            String sQuery;
            String sValue1, sValue2, sValue3, sValue4, sValue5, sValue6, sValue7, sValue8;
            int iBinCnt = 0;
            double dChkAmount = 0, dCashAmount = 0;
            int iCtr = 0, iProgressCtr = 0;

            //mc_vspReports.SetOrientation(0); this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            sQuery = m_sQuery;

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sValue1 = pSet.GetString(0);
                    sValue2 = pSet.GetString(1);
                    sValue3 = StringUtilities.RemoveApostrophe(pSet.GetString(2)); 
                    sValue4 = pSet.GetString(3);
                    sValue5 = pSet.GetString(4);
                    sValue6 = AppSettingsManager.GetBnsName(sValue1);
                    sValue7 = AppSettingsManager.GetBnsAdd(sValue1, "");
                    sValue8 = AppSettingsManager.GetBnsOwnCode(sValue1);
                    sValue8 = AppSettingsManager.GetBnsOwner(sValue8);

                    //this.axVSPrinter1.Table = "<2300|<1500|<4000|<1500|<1500;" + sValue1 + "|" + sValue2 + "|" + sValue3 + "|" + sValue4 + "|" + sValue5;
                    this.axVSPrinter1.Table = "<1500|<1500|<1500|<1500|<1500|<2000|<1000|<1000;" + sValue1 + "|" + sValue6 + "|" + sValue7 + "|" + sValue8 + "|" + sValue2 + "|" + sValue3 + "|" + sValue4 + "|" + sValue5; //AFM 20200306 added business name
                    iBinCnt++;
                }
            }
            pSet.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 10f;
            this.axVSPrinter1.Table = "<1500;" + "TOTAL: " + iBinCnt; //AFM 20200309
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = 8f;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void CancelledOR()
        {

        }

        private void TellerTransaction()
        {
            String sBody, sTeller, sQuery;
            String sTransCode, sBIN, sOrNo, sTotalAmount = "0.00", sCash, sChange, sPaymentType, sDtSave, sCheck, sTaxCredit;
            String sTellerNm;
            DateTime odtCurrentDate;
            String sTotalOfBody, sTotalChange = "0.00", sTotalCash = "0.00", sTotalOR = "0.00", sTotalCredit = "0.00", sTotalCheck = "0.00";
            String sTotalExcessTC = "0.00", sTotalExcessCheck = "0.00";

            double dTotalAmount = 0, dTotalCash = 0, dTotalChange = 0, dTotalCheck = 0, dTotalCredit = 0;
            double dTotalExcessTC = 0, dTotalExcessCheck = 0;

            if (m_sTeller == "ALL")
                sQuery = "select Count(*) from teller_transaction where ";
            else
                sQuery = "select Count(*) from teller_transaction where teller = '" + m_sTeller + "' and ";

            sQuery += "trunc(dt_save) >= to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and trunc(dt_save) <= to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') ";
            sQuery += "order by teller, dt_save";

            OracleResultSet pSet = new OracleResultSet();

            #region progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);

            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            if (m_sTeller == "ALL")
                sQuery = "select * from teller_transaction where ";
            else
                sQuery = "select * from teller_transaction where teller = '" + m_sTeller + "' and ";

            sQuery += "trunc(dt_save) >= to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and trunc(dt_save) <= to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') ";
            sQuery += "order by teller, dt_save";

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                sTeller = "";
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sTellerNm = StringUtilities.HandleApostrophe(pSet.GetString("teller"));
                    sTransCode = StringUtilities.HandleApostrophe(pSet.GetString("transaction_code"));
                    sBIN = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                    sOrNo = StringUtilities.HandleApostrophe(pSet.GetString("or_no"));
                    sTotalAmount = pSet.GetDouble("total_amount").ToString();
                    sTaxCredit = pSet.GetDouble("tax_credit").ToString();
                    sCheck = pSet.GetDouble("check_amount").ToString();
                    sCash = pSet.GetDouble("cash_tender").ToString();
                    sChange = pSet.GetDouble("change").ToString();
                    sPaymentType = StringUtilities.HandleApostrophe(pSet.GetString("payment_type")).Trim();
                    sDtSave = pSet.GetDateTime("dt_save").ToString();

                    //(s) JJP 01122005 CREATE TOTAL
                    if (sTransCode == "ONL" || sTransCode == "OFL")
                    {
                        dTotalAmount += Convert.ToDouble(sTotalAmount);
                        dTotalCredit += Convert.ToDouble(sTaxCredit);
                        dTotalCheck += Convert.ToDouble(sCheck);
                        dTotalCash += Convert.ToDouble(sCash);

                        if (sPaymentType == "CS" || sPaymentType == "CC" || sPaymentType == "CCTC" || sPaymentType == "CSTC")
                            dTotalChange = dTotalChange + Convert.ToDouble(sChange);
                        else
                            if (sPaymentType == "TC")
                                dTotalExcessTC = dTotalExcessTC + Convert.ToDouble(sChange);
                            else
                                dTotalExcessCheck = dTotalExcessCheck + Convert.ToDouble(sChange);
                    }
                    //(e) JJP 01122005 CREATE TOTAL

                    if (sTeller == AppSettingsManager.GetTeller(sTellerNm.Trim(), 0))
                    {
                        this.axVSPrinter1.Table = ("<11700;");
                    }
                    else
                    {
                        sTeller = AppSettingsManager.GetTeller(sTellerNm.Trim(), 0);

                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.Table = ("<11700;" + sTeller);
                        this.axVSPrinter1.Paragraph = "";
                    }

                    sBody = "<1200|^700|>1200|>1200|>1200|>1000|>1200|>1200|>1000|^600|^1200;";
                    sBody += sOrNo + "|";
                    sBody += sTransCode + "|";
                    sBody += Convert.ToDouble(sTotalAmount).ToString("#,##0.00") + "|";
                    sBody += Convert.ToDouble(sCash).ToString("#,##0.00") + "|";
                    sBody += Convert.ToDouble(sCheck).ToString("#,##0.00") + "|";

                    if (sPaymentType == "CS" || sPaymentType == "CC" || sPaymentType == "CCTC" || sPaymentType == "CSTC")
                    {
                        sBody += Convert.ToDouble(sChange).ToString("#,##0.00") + "|0.00|";
                        sBody += Convert.ToDouble(sTaxCredit).ToString("#,##0.00") + "|0.00|";
                    }
                    else
                        if (sPaymentType == "TC")
                        {
                            sBody += "0.00|0.00|";
                            sBody += Convert.ToDouble(sTaxCredit).ToString("#,##0.00") + "|" + Convert.ToDouble(sChange).ToString("#,##0.00") + "|";
                        }
                        else
                        {
                            sBody += "0.00|" + Convert.ToDouble(sChange).ToString("#,##0.00") + "|";
                            sBody += Convert.ToDouble(sTaxCredit).ToString("#,##0.00") + "|0.00|";
                        }
                    sBody += sPaymentType + "|";
                    sBody += sDtSave + "|";

                    this.axVSPrinter1.Table = (sBody);
                    this.axVSPrinter1.Table = "";
                    this.axVSPrinter1.Table = "";
             
                    DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            else
            {
                MessageBox.Show("No Records found", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            pSet.Close();

            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            if (m_sTeller == "ALL")
            {
                sQuery = "select count(distinct (or_no)) from teller_transaction where trunc(dt_save) >= to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and trunc(dt_save) <= to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') ";
            }
            else
            {
                sQuery = "select count(distinct (or_no)) from teller_transaction where teller = '" + m_sTeller + "' ";
                sQuery += "and trunc(dt_save) >= to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and trunc(dt_save) <= to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') ";
            }

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sTotalOR = pSet.GetInt(0).ToString();
                }
            }
            pSet.Close();

            this.axVSPrinter1.FontSize = 36;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 8;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;

            //sTotalAmt.Format("%0.2f",dTotalAmount);
            //sTotalCredit.Format("%0.2f",dTotalCredit);
            //sTotalCheck.Format("%0.2f",dTotalCheck);
            //sTotalCash.Format("%0.2f",dTotalCash);
            //sTotalChange.Format("%0.2f",dTotalChange);
            //sTotalExcessTC.Format("%0.2f",dTotalExcessTC);
            //sTotalExcessCheck.Format("%0.2f",dTotalExcessCheck);

            sTotalOfBody = "<1200|^700|>1200|>1200|>1200|>1000|>1200|>1200|>1000|^600|^1200;";
            sTotalOfBody += "|";
            sTotalOfBody += sTotalOR + "|";
            sTotalOfBody += (dTotalAmount).ToString("#,##0.00") + "|";
            sTotalOfBody += (dTotalCash).ToString("#,##0.00") + "|";
            sTotalOfBody += (dTotalCheck).ToString("#,##0.00") + "|";
            sTotalOfBody += (dTotalChange).ToString("#,##0.00") + "|";
            sTotalOfBody += (dTotalExcessCheck).ToString("#,##0.00") + "|";
            sTotalOfBody += (dTotalCredit).ToString("#,##0.00") + "|";
            sTotalOfBody += (dTotalExcessTC).ToString("#,##0.00") + "||";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = (sTotalOfBody);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = 36;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 8;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void TopHeader(int iValue)
        {
            string strProvinceName = string.Empty;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            bool m_bInit = true;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            // RMC 20161230 modified abstract teller report for Binan (s)
            if (m_sReportSwitch == "AbstractCollectByTellerBinan")
            {
                this.axVSPrinter1.FontSize = (float)10.0;
                this.axVSPrinter1.Table = "<800|<" + iValue + ";|CTO-005-A-0";
            }
            // RMC 20161230 modified abstract teller report for Binan (e)

            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.Table = "^" + iValue + ";Republic of the Philippines";
            this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.Table = string.Format("^{1};{0}", AppSettingsManager.GetConfigValue("09"), iValue);
            if (AppSettingsManager.GetConfigValue("08").Trim() != "")
                this.axVSPrinter1.Table = string.Format("^{1};PROVINCE OF {0}", AppSettingsManager.GetConfigValue("08"), iValue);
            this.axVSPrinter1.Table = string.Format("^{1};{0}", AppSettingsManager.GetConfigValue("09"), iValue);   // RMC 20150501 QA BTAS
        }

        private void CreateHeaderLubao()
        {
            if (m_sReportSwitch.Contains("ReprintOR") && m_sForm == "Not")  // RMC 20141020 printing of OR
            {
                TopHeader(11700);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "", sHeader3, sQuery = "";

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontSize = 11;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
                this.axVSPrinter1.Table = "^9000;Office of the City Treasurer";
                this.axVSPrinter1.FontSize = 15;
                sHeader3 = "^11700;" + "O F F I C I A L   R E C E I P T";   // RMC 20141020 printing of OR
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (sHeader3);
                this.axVSPrinter1.FontBold = false;
            }
            else if (m_sReportSwitch.Contains("ACEAbstractCollection"))
            {
                TopHeader(11700);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader3;

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                sHeader3 = "^11700;" + m_sReportTitle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (sHeader3);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (">10500;Covered Period:   " + m_dtFrom.ToShortDateString() + " To " + m_dtTo.ToShortDateString());
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.Table = ("^8000|^2500;Particulars|Amount");
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
            }
            else if (m_sReportSwitch.Contains("TellerTransaction"))
            {
                TopHeader(11700);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "", sHeader3;

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                sHeader3 = "^11700;" + m_sReportTitle;
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (sHeader3);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (">11700;Date:   " + m_dtFrom.ToString("MMMM dd, yyyy"));
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.Table = ("^1200|^700|^1200|^1200|^1200|^1000|^1200|^1200|^1000|^600|^1200;O.R. Number|Code|Total Amount|Cash Tender|Check Amount|Change|Excess of Check|Tax Credit|Excess of T.C.|Pay Type|Date And Time");
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
            }
            else if (m_sReportSwitch.Contains("ListOfCancelledChecks"))
            {
                TopHeader(18700);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^18700;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^18700;Office of the Municipal Treasurer";

                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^18700;" + m_sReportTitle);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("<18700;Covered Period:   " + m_dtFrom.ToShortDateString() + " To " + m_dtTo.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Table = ("^1100|^2100|^1200|^1100|^1300|^3000|^1000|^1100|^1750|^1750|^1200|^2100;Date\nCancelled|Bin|Check Number|Check\nDate|Account Number|Account Name|O.R. Number|O.R. Date|Check Amount|Cash Amount|Teller|Remarks");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("Report_of_Abstract_of_Checks"))
            {
                TopHeader(10800);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^10800;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^10800;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                string sHeader3 = "^10800;" + m_sReportTitle.ToUpper();
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (sHeader3);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = ("^1100|^1000|^3000|^1200|^1500|^1500|^1500;Check Number|Date of Check|Bank of which drawn|Official Receipt No.|Amount of Check|Amount Applied|Excess of Check");  //LGF 01132005 abstract of check	// JGR 02142006

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
            }
            else if (m_sReportSwitch.Contains("AbstractCollect"))
            {
                if (bSolo == true)
                {
                    TopHeader(11700);
                    this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
                    this.axVSPrinter1.FontName = "Arial";
                    String sHeader2 = "";
                    this.axVSPrinter1.MarginLeft = 500;
                    if (AppSettingsManager.GetConfigObject("01") == "CITY")
                        sHeader2 = "^11700;Office of the City Treasurer";
                    else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                        sHeader2 = "^11700;Office of the Municipal Treasurer";

                    this.axVSPrinter1.Table = (sHeader2);
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = ("^11700;" + m_sReportTitle);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;


                    if (m_sReportSwitch.Contains("ByOR") || m_sReportSwitch.Contains("ByDaily"))
                        this.axVSPrinter1.Table = (">10000;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                    else
                    {
                        this.axVSPrinter1.Table = (">10000;" + "Teller: " + AppSettingsManager.GetTeller(m_sTeller.Trim(), 0));
                        this.axVSPrinter1.Table = (">10000;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());  // RMC 20150617
                    }



                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                    string sFixed = "";
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                    {
                        sFixed = "O.R. DATE";
                        sFixed += "|No. of Bns|";    // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                    }

                    string sData = "";

                    arrAbv8.Clear();
                    for (int i = 0; i < arrColumnCode.Count; i++)
                    {
                        sData += AppSettingsManager.GetAbv8FeesDesc(arrColumnCode[i].ToString(), 4) + "";
                        arrAbv8.Add(AppSettingsManager.GetAbv8FeesDesc(arrColumnCode[i].ToString(), 4));
                    }

                    if (m_sReportSwitch == "AbstractCollectByDaily")
                        sFormat = "<1000|<1000|>5200;";    // RMC 20180201 display business count in Abstract - request from Jester (Malolos)

                    this.axVSPrinter1.MarginLeft = 2000;
                    this.axVSPrinter1.Table = sFormat.Replace('>', '^').Replace('<', '^') + sFixed + sData;
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
                else
                {
                    TopHeader(19600);
                    this.axVSPrinter1.FontName = "Arial";
                    String sHeader2 = "";
                    if (AppSettingsManager.GetConfigObject("01") == "CITY")
                        sHeader2 = "^18000;Office of the City Treasurer";
                    else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                        sHeader2 = "^18000;Office of the Municipal Treasurer";
                    this.axVSPrinter1.Table = (sHeader2);
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = ("^18000;" + m_sReportTitle);
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 8;

                    if (m_sReportSwitch.Contains("ByOR") || m_sReportSwitch.Contains("ByDaily"))
                    {
                        if(m_iReportSwitch == 3)
                            this.axVSPrinter1.Table = (">10000;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                        else                        
                            this.axVSPrinter1.Table = (">18000;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                    }
                    else
                    {
                        if (m_iReportSwitch == 3)
                        {
                            this.axVSPrinter1.Table = (">10000;" + "Teller: " + AppSettingsManager.GetTeller(m_sTeller.Trim(), 0));
                            this.axVSPrinter1.Table = (">10000;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());  // RMC 20150617
                        }
                        else
                        {
                            this.axVSPrinter1.Table = (">18000;" + "Teller: " + AppSettingsManager.GetTeller(m_sTeller.Trim(), 0));
                            this.axVSPrinter1.Table = (">18000;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());  // RMC 20150617
                        }
                    }


                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                    string sFixed = "";
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                    {
                        sFixed = "O.R. DATE";
                        sFixed += "|No. of Bns";    // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                    }
                    else
                    {
                        if (m_iReportSwitch == 3)
                        {
                            sFixed = "BIN"; 
                            sFixed += "|O.R. NO";
                            sFixed += "|O.R. DATE";
                            sFixed += "|BUSINESS NAME|";
                        }
                        else
                        {
                            sFixed = "BIN"; //AFM 20200128 MAO-20-12075 added bin column
                            sFixed += "|O.R. NO";
                            sFixed += "|BUSINESS NAME"; // RMC 20140909 Migration QA
                        }
                    }
                    string sData = "";

                    arrAbv8.Clear();
                    for (int i = 0; i < arrColumnCode.Count; i++)
                    {
                        if(m_iReportSwitch == 3)
                            sData += AppSettingsManager.GetAbv8FeesDesc(arrColumnCode[i].ToString(), 4);
                        else
                            sData += AppSettingsManager.GetAbv8FeesDesc(arrColumnCode[i].ToString(), 4) + "|";
                        arrAbv8.Add(AppSettingsManager.GetAbv8FeesDesc(arrColumnCode[i].ToString(), 4));
                    }
                    if(m_iReportSwitch != 3)
                        sData += "OTHER FEES|TOTAL TAX & FEES|SURCH & INT|EXCESS CQ|APPLIED TC|TOTAL";

                    if (arrColumnName.Count == 1)
                        if (m_sReportSwitch == "AbstractCollectByDaily")
                        {
                            if(m_iReportSwitch == 3)
                                sFormat = "<4000|<4000|>2000;";
                            else
                                //sFormat = "<1000|>2000|>6400|>1300|>1300|>1200|>1000|>1000|>1600;";
                                sFormat = "<1000|<1000|>2000|>5400|>1300|>1300|>1200|>1000|>1000|>1600;";    // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                        }
                        else
                        {
                            if (m_iReportSwitch == 3)
                                sFormat = "<2000|<1000|<1000|<4500|>1500;";//AFM 20200128 added bin column
                            else
                                //sFormat = "<1000|<2000|>1400|>5100|>1300|>1300|>1200|>1000|>1000|>1600;";
                                sFormat = "<2000|<1000|<3000|>1400|>2100|>1300|>1300|>1200|>1000|>1000|>1600;";//AFM 20200128 added bin column
                        }
                    else if (arrColumnName.Count == 2)
                        if (m_sReportSwitch == "AbstractCollectByDaily")
                            //  sFormat = "<1000|>2000|>3900|>3900|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1000|<1000|>2000|>3400|>3400|>1300|>1300|>1200|>1000|>1000|>1600;"; // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                        else
                            //sFormat = "<1000|<2000|>2600|>2600|>2600|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<2500|<1500|<2500|>1500|>1000|>1000|>1300|>1300|>1200|>1000|>1000|>2000;";//AFM 20200128 added bin column
                    else if (arrColumnName.Count == 3)
                        if (m_sReportSwitch == "AbstractCollectByDaily")
                            //    sFormat = "<1000|>2000|>2600|>2600|>2600|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1000|<1000|>2000|>2100|>2100|>2600|>1300|>1300|>1200|>1000|>1000|>1600;";// RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                        else
                            //sFormat = "<1000|<2000|>1950|>1950|>1950|>1950|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<2500|<1500|<2000|>1500|>1000|>1000|>1000|>1300|>1300|>1200|>1000|>1000|>1600;"; //AFM 20200128 added bin column
                    else if (arrColumnName.Count == 4)
                        if (m_sReportSwitch == "AbstractCollectByDaily")
                            //   sFormat = "<1000|>2000|>1950|>1950|>1950|>1950|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1000|<1000|>2000|>1450|>1450|>1950|>1950|>1300|>1300|>1200|>1000|>1000|>1600;";   // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                        else
                            //sFormat = "<1000|<2000|>1560|>1560|>1560|>1560|>1560|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<2000|<1500|<2000|>1500|>1000|>1000|>1000|>1000|>1300|>1300|>1200|>1000|>1000|>1600;"; //AFM 20200128 added bin column
                    else if (arrColumnName.Count == 5)
                        if (m_sReportSwitch == "AbstractCollectByDaily")
                            //    sFormat = "<1000|>2000|>1560|>1560|>1560|>1560|>1560|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1000|<1000|>2000|>1160|>1160|>1560|>1560|>1560|>1300|>1300|>1200|>1000|>1000|>1600;"; // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                        else
                            //sFormat = "<1000|<2000|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1000|<1000|<1500|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;"; //AFM 20200128 added bin column
                    else if (arrColumnName.Count == 6) //JARS 20180207 UP TO 10
                        if (m_sReportSwitch == "AbstractCollectByDaily")
                            //    sFormat = "<1000|>2000|>1560|>1560|>1560|>1560|>1560|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1000|<500|>1200|>1160|>1160|>1560|>1560|>1560|>1300|>1300|>1300|>1200|>1000|>1000|>1600;"; // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                        else
                            //sFormat = "<1000|<2000|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1500|<1000|<1500|>1000|>1000|>1000|>1000|>1000|>1000|>1000|>1000|>1000|>1000|>1000|>1000|>1600;";//AFM 20200128 added bin column
                    else if (arrColumnName.Count == 7)
                        if (m_sReportSwitch == "AbstractCollectByDaily")
                            //    sFormat = "<1000|>2000|>1560|>1560|>1560|>1560|>1560|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1000|<700|>1200|>1160|>1160|>1000|>1000|>1560|>1300|>1300|>1300|>1300|>1000|>1000|>1000|>1600;"; // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                        else
                            //sFormat = "<1000|<2000|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1500|<1000|<2000|>1000|>1000|>1000|>1000|>1000|>1000|>1000|>1000|>1000|>1000|>900|>1000|>1000|>1600;"; //AFM 20200128 added bin column
                    else if (arrColumnName.Count == 8)
                        if (m_sReportSwitch == "AbstractCollectByDaily")
                            //    sFormat = "<1000|>2000|>1560|>1560|>1560|>1560|>1560|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1000|<1000|>2000|>1160|>1160|>1560|>1560|>1560|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;"; // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                        else
                            sFormat = "<1000|<2000|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;";
                    else if (arrColumnName.Count == 9)
                        if (m_sReportSwitch == "AbstractCollectByDaily")
                            //    sFormat = "<1000|>2000|>1560|>1560|>1560|>1560|>1560|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1000|<1000|>2000|>1160|>1160|>1560|>1560|>1560|>1300|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;"; // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                        else
                            sFormat = "<1000|<2000|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;";
                    else if (arrColumnName.Count == 10)
                        if (m_sReportSwitch == "AbstractCollectByDaily")
                            //    sFormat = "<1000|>2000|>1560|>1560|>1560|>1560|>1560|>1300|>1300|>1200|>1000|>1000|>1600;";
                            sFormat = "<1000|<1000|>2000|>1160|>1160|>1560|>1560|>1560|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;"; // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
                        else
                            sFormat = "<1000|<2000|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;";
                    if (m_iReportSwitch == 3 && m_sReportSwitch != "AbstractCollectByDaily")
                        this.axVSPrinter1.Table = sFormat.Replace('>', '^').Replace('<', '^') + sFixed + sData;
                    else if (m_iReportSwitch == 3 && m_sReportSwitch == "AbstractCollectByDaily")
                        this.axVSPrinter1.Table = sFormat.Replace('>', '^').Replace('<', '^') + sFixed + "|" + sData;
                    else
                        this.axVSPrinter1.Table = sFormat.Replace('>', '^').Replace('<', '^') + sFixed + "|BNS TAX|" + sData;

                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Paragraph = "";
                }
            }
            //else if (m_sReportSwitch.Contains("WideSummaryOfCollection"))
            else if (m_sReportSwitch.Contains("SummaryOfCollection"))   // RMC 20150524 corrections in reports
            {
                TopHeader(35050);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";

                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^35050;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^35050;Office of the Municipal Treasurer";
                if (m_iBatchCnt == 1) //AFM 20200331
                {
                    this.axVSPrinter1.Table = (sHeader2);
                    this.axVSPrinter1.Paragraph = "";
                }
                    this.axVSPrinter1.FontBold = true;

                //if (m_sReportSwitch.Contains("WideSummaryOfCollectionBarangay"))
                if (m_sReportSwitch.Contains("SummaryOfCollectionBarangay"))    // RMC 20150524 corrections in reports
                    this.axVSPrinter1.Table = ("^35050;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY BARANGAY");
                //else if (m_sReportSwitch.Contains("WideSummaryOfCollectionOrgnKind"))
                else if (m_sReportSwitch.Contains("SummaryOfCollectionOrgnKind")) // RMC 20150524 corrections in reports
                    this.axVSPrinter1.Table = ("^35050;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY ORGANIZATION KIND");
                //else if (m_sReportSwitch.Contains("WideSummaryOfCollectionMainBusiness"))
                else if (m_sReportSwitch.Contains("SummaryOfCollectionMainBusiness") && m_iBatchCnt == 1)   // RMC 20150524 corrections in reports //AFM 20200331 added condition for first batch
                    this.axVSPrinter1.Table = ("^35050;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY MAIN BUSINESS");
                //else if (m_sReportSwitch.Contains("WideSummaryOfCollectionBusinessStatus"))
                else if (m_sReportSwitch.Contains("SummaryOfCollectionBusinessStatus")) // RMC 20150524 corrections in reports
                    this.axVSPrinter1.Table = ("^35050;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY BUSINESS STATUS");
                //else if (m_sReportSwitch.Contains("WideSummaryOfCollectionLineOfBusiness"))
                else if (m_sReportSwitch.Contains("SummaryOfCollectionLineOfBusiness")) // RMC 20150524 corrections in reports
                    this.axVSPrinter1.Table = ("^35050;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY LINE OF BUSINESS KIND");

                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                if (m_iBatchCnt == 1) //AFM 20200331
                {
                    this.axVSPrinter1.Table = ("^35050;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                }
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;

                // RMC 20150126 (s)
                OracleResultSet pTmp = new OracleResultSet();
                string sFormat = "";
                string sData = "";
                int iCnt = 0;

                if (m_iBatchCnt == 1)
                {
                    //if (m_sReportSwitch.Contains("WideSummaryOfCollectionBarangay"))
                    if (m_sReportSwitch.Contains("SummaryOfCollectionBarangay"))    // RMC 20150524 corrections in reports
                        this.axVSPrinter1.Table = ("^2000|^1800|^1800|^1800|^6600; |R E N E W A L|N E W|R E T I R E D| T A X E S    A N D   F E E S");
                    else /*if (m_sReportSwitch.Contains("WideSummaryOfCollectionOrgnKind")
                        || m_sReportSwitch.Contains("WideSummaryOfCollectionMainBusiness")
                        || m_sReportSwitch.Contains("WideSummaryOfCollectionBusinessStatus")
                        || m_sReportSwitch.Contains("WideSummaryOfCollectionLineOfBusiness"))*/
                        if (m_sReportSwitch.Contains("SummaryOfCollectionOrgnKind") // RMC 20150524 corrections in reports
                        || m_sReportSwitch.Contains("SummaryOfCollectionMainBusiness")
                        || m_sReportSwitch.Contains("SummaryOfCollectionBusinessStatus")
                        || m_sReportSwitch.Contains("SummaryOfCollectionLineOfBusiness"))
                        this.axVSPrinter1.Table = ("^17600; T A X E S    A N D   F E E S");

                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                    //if (m_sReportSwitch.Contains("WideSummaryOfCollectionBarangay"))
                    if (m_sReportSwitch.Contains("SummaryOfCollectionBarangay"))    // RMC 20150524 corrections in reports
                    {
                        if (m_bShowGrossNCapital == false)
                        {
                            //sFormat = "^2000|^500|^1300|^500|^1300|^500|^1300|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";
                            sFormat = "^2000|^500|^1300|^500|^1300|^500|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";  // RMC 20150522 corrections in reports
                            sData = "Particulars|No.|Gross\nReceipts|No.|Initial\nCapital|No.|Gross\nReceipts";
                            
                        }
                        else
                        {
                            sFormat = "^2000|^1800|^1800|^1800|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";
                            sData = "Particulars|Number of Filers|Number of Filers|Number of Filers";
                        }
                    }
                    //else if (m_sReportSwitch.Contains("WideSummaryOfCollectionOrgnKind"))
                    else if (m_sReportSwitch.Contains("SummaryOfCollectionOrgnKind"))
                    {
                        if (m_bShowGrossNCapital == false)
                        {
                            //sFormat = "^2000|^500|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;";
                            sFormat = "^2000|^500|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;";  // RMC 20150522 corrections in reports
                            sData = "Particulars|No.|Gross\nReceipts|Initial\nCapital";
                            
                        }
                        else
                        {
                            sFormat = "^2000|^1800|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";
                            sData = "Particulars|Number of Filers";
                        }
                    }
                    //else if (m_sReportSwitch.Contains("WideSummaryOfCollectionMainBusiness"))
                    else if (m_sReportSwitch.Contains("SummaryOfCollectionMainBusiness"))
                    {
                        if (m_bShowGrossNCapital == false)
                        {
                            sFormat = "^2000|^500|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;";
                            sData = "Particulars|No.|Gross\nReceipts|Initial\nCapital";
                            
                        }
                        else
                        {
                            sFormat = "^2000|^1800|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";
                            sData = "Particulars|Number of Filers";
                        }
                    }
                    //else if (m_sReportSwitch.Contains("WideSummaryOfCollectionBusinessStatus"))
                    else if (m_sReportSwitch.Contains("SummaryOfCollectionBusinessStatus"))
                    {
                        if (m_bShowGrossNCapital == false)
                        {
                            sFormat = "^2000|^500|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;";
                            sData = "Particulars|No.|Gross\nReceipts|Initial\nCapital";
                        }
                        else
                        {
                            sFormat = "^2000|^1800|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";
                            sData = "Particulars|Number of Filers";
                        }
                    }
                    //else if (m_sReportSwitch.Contains("WideSummaryOfCollectionLineOfBusiness"))
                    else if (m_sReportSwitch.Contains("SummaryOfCollectionLineOfBusiness"))
                    {
                        if (m_bShowGrossNCapital == false)
                        {
                            sFormat = "^2000|^500|^1300|^1300|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";
                            sData = "Particulars|No.|Gross\nReceipts|Initial\nCapital";
                            
                        }
                        else
                        {
                            sFormat = "^2000|^1800|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";
                            sData = "Particulars|Number of Filers";
                        }
                    }

                    pTmp.Query = "select header1, header2, header3, header4, header5, header6, header7, header8, header9, header10 from REPORT_SUMCOLL ";
                    pTmp.Query += " WHERE report_name = '" + m_sReportTitle + "' AND";
                    pTmp.Query += " user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                    if (pTmp.Execute())
                    {
                        if (pTmp.Read())
                        {
                            sData += "|" + pTmp.GetString(0).Trim();
                            sData += "|" + pTmp.GetString(1).Trim();
                            sData += "|" + pTmp.GetString(2).Trim();
                            sData += "|" + pTmp.GetString(3).Trim();
                            sData += "|" + pTmp.GetString(4).Trim();
                            sData += "|" + pTmp.GetString(5).Trim();
                            sData += "|" + pTmp.GetString(6).Trim();
                            sData += "|" + pTmp.GetString(7).Trim();
                            sData += "|" + pTmp.GetString(8).Trim();
                            sData += "|" + pTmp.GetString(9).Trim();
                            sData += "|SUB-TOTAL";  // RMC 20150520 corrections in reports
                        }
                    }
                    pTmp.Close();

                    this.axVSPrinter1.Table = sFormat + sData;
                }
                else
                {
                    this.axVSPrinter1.Table = ("^2000|^1800|^1800|^1800|^6600; ||||");

                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    sFormat = "^2000|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";
                    pTmp.Query = "select header11, header12, header13, header14, header15, header16, header17, header18, header19, header20, header21 from REPORT_SUMCOLL ";
                    pTmp.Query += " WHERE report_name = '" + m_sReportTitle + "' AND";
                    pTmp.Query += " user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                    if (pTmp.Execute())
                    {
                        if (pTmp.Read())
                        {
                            sData = "Particulars|" + pTmp.GetString(0).Trim(); //MCR 20150128 added particulars
                            sData += "|" + pTmp.GetString(1).Trim();
                            sData += "|" + pTmp.GetString(2).Trim();
                            sData += "|" + pTmp.GetString(3).Trim();
                            sData += "|" + pTmp.GetString(4).Trim();
                            sData += "|" + pTmp.GetString(5).Trim();
                            sData += "|" + pTmp.GetString(6).Trim();
                            sData += "|" + pTmp.GetString(7).Trim();
                            sData += "|" + pTmp.GetString(8).Trim();
                            sData += "|" + pTmp.GetString(9).Trim();
                            sData += "|" + pTmp.GetString(10).Trim();
                        }
                    }
                    pTmp.Close();

                    sData += "|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection";

                    this.axVSPrinter1.Table = sFormat + sData;
                }
                // RMC 20150126 (e)
            }
            // RMC 20150504 QA corrections (s)
            else if (m_sReportSwitch.Contains("Liquidating Officer RCD"))
            {
                TopHeader(11500);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^11500;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^11500;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^11500;" + "REPORT OF DAILY COLLECTIONS AND DEPOSITS");
                this.axVSPrinter1.Table = ("^11500;" + "(BUSINESS)");
                this.axVSPrinter1.Table = ("^11500;" + m_dtFrom.ToString("MMMM dd, yyyy"));
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
            }
            // RMC 20150504 QA corrections (e)

            else if (m_sReportSwitch == "OnReportTotalCollections")
            {
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.FontSize = 16;
                this.axVSPrinter1.FontName = "Arial";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Table = ("^11000;;" + "SUMMARY OF MONTHLY COLLECTIONS");
                this.axVSPrinter1.Table = ("^11000;;" + sTaxYear.ToString());

                this.axVSPrinter1.FontSize = 12;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.Table = ("<2000|<2200|<2200|<2200|<2200;Month|Fees|Surcharge|Penalty|Total Amount");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;

        }

        private void CreateHeader()
        {
            //if (m_sReportSwitch.Contains("ReprintOR"))    // RMC 20141020 printing of OR
            if (m_sReportSwitch.Contains("ReprintOR") && m_sForm == "Not")  // RMC 20141020 printing of OR
            {
                TopHeader(11700);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "", sHeader3, sQuery = "";

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontSize = 11;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
                this.axVSPrinter1.Table = "^9000;Office of the City Treasurer";
                this.axVSPrinter1.FontSize = 15;
                //sHeader3 = "^11700;" + "O F F I C I A L   R E C E I P T S";
                sHeader3 = "^11700;" + "O F F I C I A L   R E C E I P T";   // RMC 20141020 printing of OR
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (sHeader3);
                this.axVSPrinter1.FontBold = false;
            }
            else if (m_sReportSwitch.Contains("TellerTransaction"))
            {
                TopHeader(11700);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "", sHeader3;

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                sHeader3 = "^11700;" + m_sReportTitle;
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (sHeader3);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (">11700;Date:   " + m_dtFrom.ToString("MMMM dd, yyyy"));
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.Table = ("^1200|^700|^1200|^1200|^1200|^1000|^1200|^1200|^1000|^600|^1200;O.R. Number|Code|Total Amount|Cash Tender|Check Amount|Change|Excess of Check|Tax Credit|Excess of T.C.|Pay Type|Date And Time");
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
            }
            else if (m_sReportSwitch.Contains("ListOfCancelledChecks"))
            {
                TopHeader(18700);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^18700;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^18700;Office of the Municipal Treasurer";

                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^18700;" + m_sReportTitle);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("<18700;Covered Period:   " + m_dtFrom.ToShortDateString() + " To " + m_dtTo.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Table = ("^1100|^2100|^1200|^1100|^1300|^3000|^1000|^1100|^1750|^1750|^1200|^2100;Date\nCancelled|Bin|Check Number|Check\nDate|Account Number|Account Name|O.R. Number|O.R. Date|Check Amount|Cash Amount|Teller|Remarks");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            // JAV 20170906 (s)
            else if (m_sReportSwitch.Contains("Payment History") || m_sReportSwitch.Contains("PayHistDetails"))
            {
                //TopHeader(18700);
                //this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";

                string strProvinceName = string.Empty;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                bool m_bInit = true;
                this.axVSPrinter1.FontName = "Arial Narrow";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
                this.axVSPrinter1.FontSize = (float)12.0;
                this.axVSPrinter1.Table = "^18700;Republic of the Philippines";

                this.axVSPrinter1.Table = string.Format("^18700;{0}", AppSettingsManager.GetConfigValue("09"));
             
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^18700;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^18700;Office of the Municipal Treasurer";

                this.axVSPrinter1.Table = (sHeader2);
            }
            // JAV 20170906 (s)
            else if (m_sReportSwitch.Contains("Report_of_Abstract_of_Checks"))
            {
                TopHeader(10800);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^10800;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^10800;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                string sHeader3 = "^10800;" + m_sReportTitle.ToUpper();
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (sHeader3);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = ("^1100|^1000|^3000|^1200|^1500|^1500|^1500;Check Number|Date of Check|Bank of which drawn|Official Receipt No.|Amount of Check|Amount Applied|Excess of Check");  //LGF 01132005 abstract of check	// JGR 02142006

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
            }
            else if (m_sReportSwitch.Contains("AbstractCollect"))
            {
                TopHeader(19600);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^18000;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^18000;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^18000;" + m_sReportTitle);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = 8;
                if (m_sReportSwitch.Contains("ByOR") || m_sReportSwitch.Contains("ByDaily"))
                    this.axVSPrinter1.Table = (">18000;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                else
                    this.axVSPrinter1.Table = (">18000;" + "Teller: " + AppSettingsManager.GetTeller(m_sTeller.Trim(), 0));
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                string sFixed = "";
                if (m_sReportSwitch == "AbstractCollectByDaily")
                {
                    sFixed = "O.R. DATE";
                    
                }
                else
                {
                    sFixed = "O.R. NO";
                    sFixed += "|BUSINESS NAME"; // RMC 20140909 Migration QA
                }
                string sData = "";

                arrAbv8.Clear();
                for (int i = 0; i < arrColumnCode.Count; i++)
                {
                    sData += AppSettingsManager.GetAbv8FeesDesc(arrColumnCode[i].ToString(), 4) + "|";
                    arrAbv8.Add(AppSettingsManager.GetAbv8FeesDesc(arrColumnCode[i].ToString(), 4));
                }
                sData += "OTHER FEES|TOTAL TAX & FEES|SURCH & INT|EXCESS CQ|APPLIED TC|TOTAL";

                // RMC 20140909 Migration QA (s)
                if (arrColumnName.Count == 1)
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                        sFormat = "<1000|<2000|>6400|>1300|>1300|>1200|>1000|>1000|>1600;";
                    else
                        sFormat = "<1000|<2000|>1400|>5100|>1300|>1300|>1200|>1000|>1000|>1600;";
                else if (arrColumnName.Count == 2)
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                        sFormat = "<1000|<2000|>3900|>3900|>1300|>1300|>1200|>1000|>1000|>1600;";
                    else
                        sFormat = "<1000|<2000|>2600|>2600|>2600|>1300|>1300|>1200|>1000|>1000|>1600;";
                else if (arrColumnName.Count == 3)
                   if (m_sReportSwitch == "AbstractCollectByDaily")
                        sFormat = "<1000|<2000|>2600|>2600|>2600|>1300|>1300|>1200|>1000|>1000|>1600;";
                    else
                        sFormat = "<1000|<2000|>1950|>1950|>1950|>1950|>1300|>1300|>1200|>1000|>1000|>1600;";
                else if (arrColumnName.Count == 4)
                   if (m_sReportSwitch == "AbstractCollectByDaily")
                        sFormat = "<1000|<2000|>1950|>1950|>1950|>1950|>1300|>1300|>1200|>1000|>1000|>1600;";
                    else
                        sFormat = "<1000|<2000|>1560|>1560|>1560|>1560|>1560|>1300|>1300|>1200|>1000|>1000|>1600;";
                else if (arrColumnName.Count == 5)
                    if (m_sReportSwitch == "AbstractCollectByDaily")
                        sFormat = "<1000|<2000|>1560|>1560|>1560|>1560|>1560|>1300|>1300|>1200|>1000|>1000|>1600;";
                    else
                        sFormat = "<1000|<2000|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1200|>1000|>1000|>1600;";
                // RMC 20140909 Migration QA (e)

                this.axVSPrinter1.Table = sFormat.Replace('>', '^').Replace('<', '^') + sFixed + "|BNS TAX|" + sData;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("WideSummaryOfCollectionBarangay"))
            {
                TopHeader(35050);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^35050;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^35050;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^35050;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY BARANGAY");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^35050;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
               
                // RMC 20150126 (s)
                OracleResultSet pTmp = new OracleResultSet();
                string sFormat = "";
                string sData = "";
                int iCnt = 0;

                if (m_iBatchCnt == 1)
                {
                    this.axVSPrinter1.Table = ("^2000|^1800|^1800|^1800|^6600; |R E N E W A L|N E W|R E T I R E D| T A X E S    A N D   F E E S");
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                    sFormat = "^2000|^500|^1300|^500|^1300|^500|^1300|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";
                    sData = "Particulars|No.|Gross\nReceipts|No.|Initial\nCapital|No.|Gross\nReceipts";

                    pTmp.Query = "select header1, header2, header3, header4, header5, header6, header7, header8, header9, header10 from REPORT_SUMCOLL ";
                    pTmp.Query += " WHERE report_name = '" + m_sReportTitle + "' AND";
                    pTmp.Query += " user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                    if (pTmp.Execute())
                    {
                        if (pTmp.Read())
                        {
                            sData += "|" + pTmp.GetString(0).Trim();
                            sData += "|" + pTmp.GetString(1).Trim();
                            sData += "|" + pTmp.GetString(2).Trim();
                            sData += "|" + pTmp.GetString(3).Trim();
                            sData += "|" + pTmp.GetString(4).Trim();
                            sData += "|" + pTmp.GetString(5).Trim();
                            sData += "|" + pTmp.GetString(6).Trim();
                            sData += "|" + pTmp.GetString(7).Trim();
                            sData += "|" + pTmp.GetString(8).Trim();
                            sData += "|" + pTmp.GetString(9).Trim();
                            //sData += "|SUB TOTAL TAXES"; //MCR 20150128
                        }
                    }
                    pTmp.Close();

                    this.axVSPrinter1.Table = sFormat + sData;
                }
                else
                {
                    this.axVSPrinter1.Table = ("^2000|^1800|^1800|^1800|^6600; ||||");

                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                    sFormat = "^2000|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^1200;";
                    pTmp.Query = "select header11, header12, header13, header14, header15, header16, header17, header18, header19, header20, header21 from REPORT_SUMCOLL ";
                    pTmp.Query += " WHERE report_name = '" + m_sReportTitle + "' AND";
                    pTmp.Query += " user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                    if (pTmp.Execute())
                    {
                        if (pTmp.Read())
                        {
                            sData = "Particulars|" + pTmp.GetString(0).Trim(); //MCR 20150128 added particulars
                            sData += "|" + pTmp.GetString(1).Trim();
                            sData += "|" + pTmp.GetString(2).Trim();
                            sData += "|" + pTmp.GetString(3).Trim();
                            sData += "|" + pTmp.GetString(4).Trim();
                            sData += "|" + pTmp.GetString(5).Trim();
                            sData += "|" + pTmp.GetString(6).Trim();
                            sData += "|" + pTmp.GetString(7).Trim();
                            sData += "|" + pTmp.GetString(8).Trim();
                            sData += "|" + pTmp.GetString(9).Trim();
                            sData += "|" + pTmp.GetString(10).Trim();
                        }
                    }
                    pTmp.Close();

                    sData += "|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection";

                    this.axVSPrinter1.Table = sFormat + sData;
                }
                // RMC 20150126 (e)

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("SimpSummaryOfCollectionBarangay"))
            {
                TopHeader(31150);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^31150;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^31150;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^31150;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY BARANGAY"); //m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^31150;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^2000|^1800|^1800|^1800|^6600; |R E N E W A L|N E W|R E T I R E D| T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3      4   5     6    7      8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25  
                this.axVSPrinter1.Table = ("^2000|^500|^1300|^500|^1300|^500|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|No.|Initial\nCapital|No.|Gross\nReceipts|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|CNC|ECC|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1     2          3        4          5         6          7              8              9                      10                       11         12                           13                    14                  15                     16                          17                          18                                19                           20             21   22           23                 24                    25              
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 7;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("WideSummaryOfCollectionDistrict"))
            {
                TopHeader(32450);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^32450;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^32450;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^32450;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY DISTRICT");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^32450;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^2000|^1800|^1800|^1800|^6600; |R E N E W A L|N E W|R E T I R E D| T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3      4   5     6    7      8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25    26  
                this.axVSPrinter1.Table = ("^2000|^500|^1300|^500|^1300|^500|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|No.|Initial\nCapital|No.|Gross\nReceipts|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|Flammable/\nCombustible\nSubstance|Others|Environmental\nMgt Clearance|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1     2          3        4          5         6          7              8              9                      10                       11         12                           13                    14                  15                     16                          17                          18                                19                           20                             21                      22             23                 24                           25               26            
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("SimpSummaryOfCollectionDistrict"))
            {
                TopHeader(32450);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^32450;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^32450;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^32450;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY DISTRICT"); //m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^32450;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^2000|^1800|^1800|^1800|^6600; |R E N E W A L|N E W|R E T I R E D| T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3      4   5     6    7      8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25  
                this.axVSPrinter1.Table = ("^2000|^500|^1300|^500|^1300|^500|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|No.|Initial\nCapital|No.|Gross\nReceipts|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|Flammable/\nCombustible\nSubstance|Others|Environmental\nMgt Clearance|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1     2          3        4          5         6          7              8              9                      10                       11         12                           13                    14                  15                     16                          17                          18                                19                           20                             21                      22             23                 24                           25               26            
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 7;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("WideSummaryOfCollectionMainBusiness"))
            {
                TopHeader(32750);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^32750;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^32750;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^32750;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY MAIN BUSINESS");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^32750;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^17600; T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3    4       5     6     7     8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25   
                this.axVSPrinter1.Table = ("^2000|^500|^1300|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|Initial\nCapital|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|Flammable/\nCombustible\nSubstance|Others|Environmental\nMgt Clearance|CSWD|Weight\n&\nMeasure|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1     2          3               4                5              6          7                                   8              9                      10                       11                12                           13                    14                              15                            16                          17                          18                       19                    20            21            22             23                 24           25         
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("SimpSummaryOfCollectionMainBusiness"))
            {
                TopHeader(22800);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^22800;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^22800;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^22800;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY MAIN BUSINESS");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^22800;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^17600; T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3      4   5     6      7      8    9    10     11    12    13    14    15   16     17   18     19 
                this.axVSPrinter1.Table = ("^4000|^500|^1300|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|Initial\nCapital|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1       2          3          4                   5             6                           7                    8           9                    10                             11                  12                     13                            14                        15                              16                         17                        18                 19  
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("StanWideSummaryOfCollectionMainBusiness"))
            {
                TopHeader(26250);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^26250;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^26250;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^26250;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY MAIN BUSINESS");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^26250;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^17600; T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3    4       5     6     7     8    9    10     11    12    13    14    15   16     17   18     19    20  
                this.axVSPrinter1.Table = ("^2000|^500|^1300|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|Initial\nCapital|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1     2          3               4                5              6          7                                   8              9                      10                       11                12                           13                    14                          15                            16                          17                          18                       19                    20          
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("StanSimpSummaryOfCollectionMainBusiness"))
            {
                TopHeader(28250);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^28250;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^28250;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^28250;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY MAIN BUSINESS");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^28250;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^17600; T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3      4   5     6      7      8    9    10     11    12    13    14    15   16     17   18     19   20  
                this.axVSPrinter1.Table = ("^4000|^500|^1300|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|Initial\nCapital|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1       2          3          4                   5             6                           7                    8           9                    10                             11                  12                     13                            14                        15                              16                         17                        18                 19                        20     
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("WideSummaryOfCollectionOrgnKind"))
            {
                TopHeader(32750);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^32750;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^32750;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^32750;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY ORGANIZATION KIND");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^32750;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^17600; T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3    4       5     6     7     8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25   
                this.axVSPrinter1.Table = ("^2000|^500|^1300|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|Initial\nCapital|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|Flammable/\nCombustible\nSubstance|Others|Environmental\nMgt Clearance|CSWD|Weight\n&\nMeasure|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1     2          3               4                5              6          7                                   8              9                      10                       11                12                           13                    14                              15                            16                          17                          18                       19                    20            21            22             23                 24           25         
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("WideSimpSummaryOfCollectionOrgnKind"))
            {
                TopHeader(28850);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^28850;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^28850;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^28850;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY ORGANIZATION KIND");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^28850;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.Table = ("^17600; T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3    4       5     6     7     8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     
                this.axVSPrinter1.Table = ("^2000|^500|^1300|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|Initial\nCapital|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|CNC|ECC|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1     2          3               4                5              6          7                                   8              9                      10                       11                12                           13                    14                              15                            16                          17              18  19            20                      21            22  
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("WideSummaryOfCollectionBusinessStatus"))
            {
                TopHeader(32750);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^32750;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^32750;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^32750;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY BUSINESS STATUS");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^32750;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^17600; T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3    4       5     6     7     8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     23   24     25   
                this.axVSPrinter1.Table = ("^2000|^500|^1300|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|Initial\nCapital|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|Flammable/\nCombustible\nSubstance|Others|Environmental\nMgt Clearance|CSWD|Weight\n&\nMeasure|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1     2          3               4                5              6          7                                   8              9                      10                       11                12                           13                    14                              15                            16                          17                          18                       19                    20            21            22             23                 24                       25         
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("StanWideSummaryOfCollectionBusinessStatus"))
            {
                TopHeader(26250);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^26250;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^26250;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^26250;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY BUSINESS STATUS");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^26250;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^17600; T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3    4       5     6     7     8    9    10     11    12    13    14    15   16     17   18     19    20     
                this.axVSPrinter1.Table = ("^2000|^500|^1300|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|Initial\nCapital|License\nTax|Mayor's\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1     2          3               4                5              6          7                                   8              9                      10                       11                12                           13                    14                              15                            16                          17                    18                  19               20       
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("WideSummaryOfCollectionLineOfBusiness"))
            {
                TopHeader(28850);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^28850;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^28850;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = (sHeader2);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^28850;" + "SUMMARY OF BUSINESS TAX COLLECTIONS AND REGULATORY FEES BY LINE OF BUSINESS KIND");//+ m_sReportTitle);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^28850;" + "Covered Period: " + m_dtFrom.Date.ToShortDateString() + " TO " + m_dtTo.Date.ToShortDateString());
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("^17600; T A X E S    A N D   F E E S");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                //1    2    3    4       5     6     7     8    9    10     11    12    13    14    15   16     17   18     19    20    21   22     
                this.axVSPrinter1.Table = ("^2000|^500|^1300|^1300|^1300|^1300|^1400|^1300|^1300|^1550|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;Particulars|No.|Gross\nReceipts|Initial\nCapital|License\nTax|Mayors\nPermit Fee|Delivery\nTrucks/Van\nPermit Fee|Zoning Fee|Garbage Fee|Anti-Pollution/\nCertification\nFee|Sanitary Insp.\nFee|Bldg Inspection\nFee|Electrical\nInspection\nFee|Mechanical\nInspection\nFee|Plumbing\nInspection\nFee|Signboard/\nBillboard Renewal\nFee|Signboard/\nBillboard Fee|CNC|ECC|Total\nTaxes &\nFees|Surcharges\n&\nInterests|Total\nCollection");
                // 1     2          3               4                5              6          7                                   8              9                      10                       11                12                           13                    14                              15                            16                          17              18  19            20                      21            22  
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportSwitch.Contains("Negative List"))
            {
                TopHeader(10800);
                this.axVSPrinter1.FontName = "Arial";
                String sHeader2 = "";

                // JAA 20200309 commented (s)
                //if (AppSettingsManager.GetConfigObject("01") == "CITY")
                //    sHeader2 = "^10800;Office of the City Treasurer";
                //else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                //    sHeader2 = "^10800;Office of the Municipal Treasurer";
                // JAA 20200309 commented (e)

                // JAA 20200309 added MAO-20-12518 (s)
               sHeader2 = "^10800;Business Permit and Licensing Office";
                // JAA 20200309 added MAO-20-12518 (e)

                this.axVSPrinter1.Table = sHeader2;
                string sHeader3 = "^10800;" + m_sReportTitle.ToUpper();
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = (sHeader3);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "Date:" + m_dtFrom.ToString("MM/dd/yyyy") + " - " + m_dtTo.ToString("MM/dd/yyyy");

                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Paragraph = "";
                //this.axVSPrinter1.Table = ("^2300|^1500|^4000|^1500|^1500;BIN|OFFICE|VIOLATION|DATE|OFFICER");  //LGF 01132005 abstract of check	// JGR 02142006
                this.axVSPrinter1.Table = ("^1500|^1500|^1500|^1500|^1500|^2000|^1000|^1000;BIN|BUSINESS NAME|BUSINESS ADDRESS|OWNER NAME|OFFICE|VIOLATION|DATE|OFFICER");  // AFM 20200306 added business name, bns address, owner name

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
            }
        }

        private void axVSPrinter1_BeforeFooter(object sender, EventArgs e)
        {
            this.axVSPrinter1.HdrFontName = "Arial Narrow";
            this.axVSPrinter1.HdrFontSize = (float)8.0;
            this.axVSPrinter1.HdrFontItalic = true;
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            // RMC 20161230 modified abstract teller report for Binan (s)
            if (m_sReportSwitch == "AbstractCollectByTellerBinan")
            {
                if (!m_bInit)
                {
                    if (!m_bPage2)
                        CreateHeaderPage1Binan();
                    else
                        CreateHeaderPage2Binan();
                }

                m_bInit = false;
            }// RMC 20161230 modified abstract teller report for Binan (e)
            else
            {
                if (m_sReportSwitch.Contains("SummaryOfCollection") || m_sReportSwitch.Contains("AbstractCollect"))
                {
                    if (!m_bInit)
                        CreateHeaderLubao();

                    m_bInit = false;
                }

                else if (m_sReportSwitch != "ReprintOR" && m_sReportSwitch != "PrintOR")
                    CreateHeaderLubao();
                //else
                //CreateHeader();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private string GetDebitMemo(string sOrNo)
        {
            OracleResultSet pTmp = new OracleResultSet();
            string sDebit = "0";

            pTmp.Query = "select sum(debit) from dbcr_memo where or_no = '" + sOrNo + "' and served = 'Y'"; //JARS 20171010
            if (pTmp.Execute())
            {
                if (pTmp.Read())
                {
                    sDebit = string.Format("{0:#,###.00}", pTmp.GetDouble(0));
                }
            }
            pTmp.Close();

            return sDebit;
        }

        private string GetCreditMemo(string sOrNo)
        {
            // RMC 20150506 QA corrections
            OracleResultSet pTmp = new OracleResultSet();
            string sCredit = "0";

            pTmp.Query = "select sum(credit) from dbcr_memo where memo = 'CREDITED IN EXCESS TO CHECK PAYMENT/S HAVING OR_NO " + sOrNo + "'"; //JARS 20171010
            if (pTmp.Execute())
            {
                if (pTmp.Read())
                {
                    sCredit = string.Format("{0:#,###.00}", pTmp.GetDouble(0));
                }
            }
            pTmp.Close();

            // RMC 20170119 correction in multiple transaction with remaining credit on last bin (s)
            if (Convert.ToDouble(sCredit) == 0)
            {
                pTmp.Query = "select sum(credit) from dbcr_memo where memo = 'REMAINING BALANCE AFTER PAYMENT W/ DEBIT HAVING OR " + sOrNo + "' and served = 'N'"; //JARS 20171010
                if (pTmp.Execute())
                {
                    if (pTmp.Read())
                    {
                        sCredit = string.Format("{0:#,###.00}", pTmp.GetDouble(0));
                    }
                }
                pTmp.Close();
            }
            // RMC 20170119 correction in multiple transaction with remaining credit on last bin (e)

            return sCredit;
        }

        private void OnReportLORCD()
        {
            OracleResultSet pRec = new OracleResultSet();
            // RMC 20150504 QA corrections

            //m_dtFrom
            //this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = "<5500|<5500;A. SUMMARY OF COLLECTIONS|B. SUMMARY OF DEPOSITS";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.Table = "^2000|^3500|^3500|^2000;FORM TYPE|OFFICIAL RECEIPT SERIES|COLLECTION/LIQUIDATING OFFICER|AMOUNT";
            string sDate = string.Format("{0:MM/dd/yyyy}", m_dtFrom);
            string sTellerCode, sRcdSeries, sOrFr, sOrTo, sORDate, sForm, sAmtCol, sData;
            int iRowCnt = 0;
            double dTotAmt = 0;

/*
            pRec.Query = "select * from partial_remit where to_date(dt_save) = to_date('" + sDate + "', 'MM/dd/yyyy')";
            pRec.Query += " order by teller, rcd_series, or_from";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sTellerCode = pRec.GetString("teller");
                    sRcdSeries = pRec.GetString("rcd_series");
                    sOrFr = pRec.GetString("or_from");
                    sOrTo = pRec.GetString("or_to");
                    sORDate = pRec.GetString("or_date");
                    sForm = "FORM 51";
                    sAmtCol = string.Format("{0:#,###.00}", pRec.GetDouble("total_collection"));
                    dTotAmt += Convert.ToDouble(sAmtCol);

                    iRowCnt++;

                    // RMC 20150707 Modified Summary of Collection (Daily comparative) amount of Yesterday collection (s)
                    if(sOrFr == sOrTo)
                        sData = sForm + "|" + sOrFr + "|" + AppSettingsManager.GetTeller(sTellerCode, 0) + "|" + sAmtCol;
                    // RMC 20150707 Modified Summary of Collection (Daily comparative) amount of Yesterday collection (e)
                    else
                        sData = sForm + "|" + sOrFr + "-" + sOrTo + "|" + AppSettingsManager.GetTeller(sTellerCode, 0) + "|" + sAmtCol;
                    this.axVSPrinter1.Table = ("<2000|<3500|<3500|>2000;" + sData);
                }
            }
            pRec.Close();
*/  // RMC 20150915 corrections in print of RCD, put rem

            // RMC 20150915 corrections in print of RCD (s)
            OracleResultSet pRec2 = new OracleResultSet();
            sOrFr = string.Empty;
            sOrTo = string.Empty;
            pRec2.Query = "select distinct rcd_series from partial_remit where to_date(dt_save) = to_date('" + sDate + "', 'MM/dd/yyyy')";
            pRec2.Query += " order by rcd_series ";
            if(pRec2.Execute())
            {
                while (pRec2.Read())
                {
                    sRcdSeries = pRec2.GetString(0);

                    //get ORs
                    string sTmpOR = string.Empty;

                    pRec.Query = "select * from partial_remit where to_date(dt_save) = to_date('" + sDate + "', 'MM/dd/yyyy')";
                    pRec.Query += " and rcd_series = '" + sRcdSeries + "' order by or_from";
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            if(sTmpOR.Trim() != "")
                                sTmpOR += ", ";
                            sOrFr = pRec.GetString("or_from");
                            sOrTo = pRec.GetString("or_to");
                            sORDate = pRec.GetString("or_date");
                            if (sOrFr == sOrTo)
                                sTmpOR += sOrFr;
                            else
                                sTmpOR += sOrFr + "-" + sOrTo;

                            
                        }
                    }
                    pRec.Close();

                    pRec.Query = "select * from partial_remit where to_date(dt_save) = to_date('" + sDate + "', 'MM/dd/yyyy')";
                    pRec.Query += " and rcd_series = '" + sRcdSeries + "' order by or_from";
                    if(pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sTellerCode = pRec.GetString("teller");
                            sForm = "FORM 51";
                            sAmtCol = string.Format("{0:#,###.00}", pRec.GetDouble("total_collection"));
                            dTotAmt += Convert.ToDouble(sAmtCol);

                            iRowCnt++;
                            sData = sForm + "|" + sTmpOR + "|" + AppSettingsManager.GetTeller(sTellerCode, 0) + "|" + sAmtCol;
                            this.axVSPrinter1.Table = ("<2000|<3500|<3500|>2000;" + sData);
                        }
                    }
                    pRec.Close();
                }
            }
            pRec2.Close();
            // RMC 20150915 corrections in print of RCD (e)

            for (int i = iRowCnt; i < 25; i++)
            {
                sData = "|||";
                this.axVSPrinter1.Table = ("<2000|<3500|<3500|>2000;" + sData);
            }

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            
            sAmtCol = string.Format("{0:#,###.00}", dTotAmt);
            sData = "||TOTAL|" + sAmtCol;
            this.axVSPrinter1.FontBold = true;

            this.axVSPrinter1.Table = ("<2000|<3500|<3500|>2000;" + sData);

            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            this.axVSPrinter1.Table = ("<5500|<5500;C. CERTIFICATION|D. ACKNOWLEDGEMENT");
            sData = "\n	I hereby certify on my official oath that the above is a true and correct statement";
            sData += " of collections and deposits at the close of business on " + string.Format("{0:MMMM dd, yyyy}",m_dtFrom) + ".";
            sData += "\n\n\n";
            sData += "		" + AppSettingsManager.GetConfigValue("05");
            if (AppSettingsManager.GetConfigValue("01") == "MUNICIPALITY")
                sData += "\n		Municipal Treasurer";
            else
                sData += "\n		City Treasurer";
            string sData2;

            sData2 = "\nReceived the reports and its supporting documents.";
            sData2 += "\n\n\n\n\n";
            sData2 += "	____________________________";
            sData2 += "\n	Accounting Officer	Date ";
            this.axVSPrinter1.Table = ("<5500|<5500;" + sData + "|" + sData2);

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            this.axVSPrinter1.Table = ("<5500|<5500;F. ACCOUNTING RECAPITULATION|");
            this.axVSPrinter1.Table = ("<5500|<5500;To Journal Collections and Deposits|To Subsidiary Ledger");
            this.axVSPrinter1.Table = ("<1500|<4000|<1500|<4000;8-70-100|P" + sAmtCol + "|8-70-100|P" + sAmtCol);
            this.axVSPrinter1.Table = ("<1500|<4000|<1500|<4000;0-90-100|P" + sAmtCol + "|0-90-100|P" + sAmtCol);

            this.axVSPrinter1.NewPage();

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Table = ("<7500|<3500;Fund: ________________|Date: " + string.Format("{0:MMMM dd, yyyy}",m_dtFrom));
            this.axVSPrinter1.Table = ("<7500|<3500;Name of Accountable Officer: " + AppSettingsManager.SystemUser.UserName + "|Report No: ____________");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = ("<11000;2. For Liquidation Officers/Treasurers");

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.FontBold = true;

            sData = "Name of Accountable Officers|Report No.|Amount";
            this.axVSPrinter1.Table = ("^4000|^3500|^3500;" + sData);
            this.axVSPrinter1.FontBold = false;
            sData = "|||";
            this.axVSPrinter1.Table = ("^4000|^3500|^3500;" + sData);

            iRowCnt = 0;
            sDate = string.Format("{0:MM/dd/yyyy}", m_dtFrom);
            dTotAmt = 0;

            pRec.Query = "select distinct rcd_series, teller, total_collection from partial_remit where to_date(dt_save) = to_date('" + sDate + "', 'MM/dd/yyyy')";
            pRec.Query += " order by rcd_series, teller";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sRcdSeries = pRec.GetString(0);
                    sTellerCode = pRec.GetString(1);
                    sAmtCol = string.Format("{0:#,###.00}", pRec.GetDouble(2));

                    iRowCnt++;
                    sData = AppSettingsManager.GetTeller(sTellerCode, 0) + "|" + sRcdSeries + "|" + sAmtCol;
                    this.axVSPrinter1.Table = ("<4000|<3500|>3500;" + sData);

                    dTotAmt += Convert.ToDouble(sAmtCol);
                }
            }
            pRec.Close();

            for (int i = iRowCnt; i < 25; i++)
            {
                sData = "||";
                this.axVSPrinter1.Table = ("<4000|<3500|>3500;" + sData);
            }

            sAmtCol = string.Format("{0:#,###.00}", dTotAmt);
            sData = "|TOTAL|" + sAmtCol;
            this.axVSPrinter1.FontBold = true;

            this.axVSPrinter1.Table = ("<4000|<3500|>3500;" + sData);

            this.axVSPrinter1.FontBold = false;

            sData = "\n	I hereby certify on my official oath that the above is a true and correct statement";
            sData += " of collections and deposits at the close of business on " + string.Format("{0:MMMM dd, yyyy}",m_dtFrom);
            sData += "\n\n\n";
            sData += "		" + AppSettingsManager.GetConfigValue("05");
            if (AppSettingsManager.GetConfigValue("01") == "MUNICIPALITY")
                sData += "\n		Municipal Treasurer";
            else
                sData += "\n		City Treasurer";

            this.axVSPrinter1.Table = ("11000;" + sData);

            // RMC 20150623 Customized Summary of Collection (Daily comparative) in L.O. RCD (s)
            this.axVSPrinter1.NewPage();

            this.axVSPrinter1.Table = ("^11500;SUMMARY OF COLLECTION");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
     

           DateTime dtYesterday = m_dtFrom.AddDays(-1);
            string sYesterday = string.Format("{0:ddd}", dtYesterday);

           /* if(sYesterday == "Sun")
                dtYesterday = dtYesterday.AddDays(-2);

            sYesterday = string.Format("{0:ddd}", dtYesterday); //for checking
            */
            // RMC 20150707 Modified Summary of Collection (Daily comparative) amount of Yesterday collection, put rem
            
            string sDtYesterday = string.Format("{0:MM/dd/yyyy}",dtYesterday);
            //get total collection from previous day
            string sAmtColYesterday = string.Empty;
            double dAmtColYesterday = 0;

            /*while (dAmtColYesterday == 0)
            {
                pRec.Query = "select distinct rcd_series, teller, total_collection from partial_remit where to_date(dt_save) = to_date('" + sDtYesterday + "', 'MM/dd/yyyy')";
                pRec.Query += " order by rcd_series, teller";
                if (pRec.Execute())
                {
                    while (pRec.Read())
                    {
                        dAmtColYesterday += pRec.GetDouble(2);
                    }
                }
                pRec.Close();

                dtYesterday = dtYesterday.AddDays(-1);
                sDtYesterday = string.Format("{0:MM/dd/yyyy}", dtYesterday);
            }*/
            // RMC 20150707 Modified Summary of Collection (Daily comparative) amount of Yesterday collection, put rem

            // RMC 20150707 Modified Summary of Collection (Daily comparative) amount of Yesterday collection (s)
            string sYesterdayFr = m_dtFrom.Month + "/01/" + m_dtFrom.Year;
            DateTime dtYesterdayFr;
            DateTime.TryParse(sYesterdayFr, out dtYesterdayFr);

            if (dtYesterdayFr == m_dtFrom)
                dAmtColYesterday = 0;
            else
            {
                pRec.Query = "select distinct rcd_series, teller, total_collection from partial_remit where ";
                pRec.Query += "to_date(dt_save) between to_date('" + sYesterdayFr + "', 'MM/dd/yyyy') ";
                pRec.Query += "and to_date('" + sDtYesterday + "', 'MM/dd/yyyy') ";
                pRec.Query += " order by rcd_series, teller";
                if (pRec.Execute())
                {
                    while (pRec.Read())
                    {
                        dAmtColYesterday += pRec.GetDouble(2);
                    }
                }
                pRec.Close();
            }
            // RMC 20150707 Modified Summary of Collection (Daily comparative) amount of Yesterday collection (e)

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;

            sAmtColYesterday = string.Format("{0:#,###.00}", dAmtColYesterday);

            this.axVSPrinter1.Table = "<4000;COLLECTION YESTERDAY";
            object lng1 = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<1000|<3000|>3000;; |Prov. Form No. 130(A)|___________________";
            this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1);
            this.axVSPrinter1.Table = "<1000|<3000|>3000;;||" + sAmtColYesterday;
            lng1 = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<4000|>3000;;TOTAL COLLECTIONS|___________________";
            this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1);
            this.axVSPrinter1.Table = "<4000|>3000;;|" + sAmtColYesterday;

            this.axVSPrinter1.Table = "<4000;;COLLECTION TODAY";
            lng1 = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<1000|<3000|>3000;; |Prov. Form No. 130(A)|___________________";
            this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1);
            this.axVSPrinter1.Table = "<1000|<3000|>3000;;||" + sAmtCol;
            lng1 = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<4000|>3000;;TOTAL COLLECTIONS|___________________";
            this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1);
            this.axVSPrinter1.Table = "<4000|>3000;;|" + sAmtCol;

            double dTotSumCol = dAmtColYesterday + dTotAmt;
            string sTotSumCol = string.Format("{0:#,###.00}", dTotSumCol);

            this.axVSPrinter1.Table = "<4000;;COLLECTION TODAY";
            lng1 = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<1000|<3000|>3000;; |Prov. Form No. 130(A)|___________________";
            this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1);
            this.axVSPrinter1.Table = "<1000|<3000|>3000;;||" + sTotSumCol;
            lng1 = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = "<4000|>3000;;TOTAL COLLECTIONS|___________________";
            this.axVSPrinter1.CurrentY = Convert.ToInt64(lng1);
            this.axVSPrinter1.Table = "<4000|>3000;;|" + sTotSumCol;
            // RMC 20150623 Customized Summary of Collection (Daily comparative) in L.O. RCD (e)
        }

        private void OnReportSimpSummaryOfCollectionBarangay()
        {
            // RMC 20150524 corrections in reports

            String sQuery = "", sDataGroup = "", sDataGroup2, sDateGen = "", sTempDataGroup = "";
            String sRen, sNew, sRet, sRenGross, sNewGross, sRetGross;
            String sData1, sData2, sData3, sData4, sData5, sData6, sData7, sData8, sData9, sData10, sData11, sData12, sData13, sData14, sData15, sData16, sData17, sData18, sTotalTax, sSurcharge, sTotalCollect;
            double dREN = 0, dNEW = 0, dRET = 0, dRenGross = 0, dNewGross = 0, dRetGross = 0;
            double dData1 = 0, dData2 = 0, dData3 = 0, dData4 = 0, dData5 = 0, dData6 = 0, dData7 = 0, dData8 = 0, dData9 = 0, dData10 = 0, dData11 = 0, dData12 = 0, dData13 = 0, dData14 = 0, dData15 = 0, dData16 = 0, dData17 = 0, dData18 = 0, dTotalTax = 0, dSurcharge = 0, dTotalCollect = 0;
            double dTREN = 0, dTNEW = 0, dTRET = 0, dTRenGross = 0, dTNewGross = 0, dTRetGross = 0;
            double dTData1 = 0, dTData2 = 0, dTData3 = 0, dTData4 = 0, dTData5 = 0, dTData6 = 0, dTData7 = 0, dTData8 = 0, dTData9 = 0, dTData10 = 0, dTData11 = 0, dTData12 = 0, dTData13 = 0, dTData14 = 0, dTData15 = 0, dTData16 = 0, dTData17 = 0, dTData18 = 0, dTTotalTax = 0, dTSurcharge = 0, dTTotalCollect = 0;
            double dTData19 = 0, dTData20 = 0, dTData21 = 0, dData19 = 0, dData20 = 0, dData21 = 0;
            String sData19, sData20, sData21;
            double dSubData = 0, dTSubData = 0;
            String sSubData = "";

            for (int i = 1; i <= 2; i++) // RMC 20150126
            {
                // RMC 20150522 corrections in reports (s)
                sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                dTotalTax = 0; dSurcharge = 0; dTotalCollect = 0;
                // RMC 20150522 corrections in reports (e)

                // RMC 20150126 (s)
                m_iBatchCnt = i;

                sTempDataGroup = "";

                if (m_iBatchCnt == 2)
                    this.axVSPrinter1.NewPage();
                // RMC 20150126 (e)

                sQuery = @"SELECT RS.data_group1, sum(RS.new_no) as new_no, sum(RS.new_cap) as new_cap, sum(RS.ren_no) as ren_no,  ";
                sQuery += " sum(RS.ren_gross) as ren_gross, sum(RS.ret_no) as ret_no, sum(RS.ret_gross) as ret_gross, sum(RS.data1) as data1, ";
                sQuery += " sum(RS.data2) as data2, sum(RS.data3) as data3, sum(RS.data4) as data4, sum(RS.data5) as data5, ";
                sQuery += " sum(RS.data6) as data6, sum(RS.data7) as data7, sum(RS.data8) as data8, sum(RS.data9) as data9, ";
                sQuery += " sum(RS.data10) as data10, sum(RS.data11) as data11, sum(RS.data12) as data12, sum(RS.data13) as data13, ";
                sQuery += " sum(RS.data14) as data14, sum(RS.data15) as data15, sum(RS.data16) as data16, sum(RS.data17) as data17, ";
                sQuery += " sum(RS.data18) as data18, sum(RS.data19) as data19, sum(RS.data20) as data20, sum(RS.data21) as data21, ";
                sQuery += " sum(RS.sur_interest) as sur_interest, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen, ";
                sQuery += " sum(RS.data1)+sum(RS.data2)+sum(RS.data3)+sum(RS.data4)+sum(RS.data5)+sum(RS.data6)+sum(RS.data7)+sum(RS.data8)+";
                sQuery += " sum(RS.data9)+sum(RS.data10)+sum(RS.data11)+sum(RS.data12)+sum(RS.data13)+sum(RS.data14)+sum(RS.data15)+";
                sQuery += " sum(RS.data16)+sum(RS.data17)+sum(RS.data18)+sum(RS.data19)+sum(RS.data20)+sum(RS.data21) as TOTALTAXES, ";
                sQuery += " sum(RS.data1)+sum(RS.data2)+sum(RS.data3)+sum(RS.data4)+sum(RS.data5)+sum(RS.data6)+sum(RS.data7)+sum(RS.data8)+";
                sQuery += " sum(RS.data9)+sum(RS.data10)+sum(RS.data11)+sum(RS.data12)+sum(RS.data13)+sum(RS.data14)+sum(RS.data15)+";
                sQuery += " sum(RS.data16)+sum(RS.data17)+sum(RS.data18)+sum(RS.data19)+sum(RS.data20)+sum(RS.data21)+sum(rs.sur_interest) as TOTALCOLLECTION, ";
                sQuery += " sum(RS.data1)+sum(RS.data2)+sum(RS.data3)+sum(RS.data4)+sum(RS.data5)+sum(RS.data6)+sum(RS.data7)+sum(RS.data8)+";
                sQuery += " sum(RS.data9)+sum(RS.data10) as SUBTOTALTAXES";
                sQuery += " FROM REPORT_SUMCOLL RS";
                sQuery += " WHERE RS.report_name = '" + m_sReportTitle + "' AND";
                sQuery += " RS.user_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                sQuery += " GROUP BY RS.data_group1, RS.dt_from, RS.dt_to, RS.current_user, RS.dt_gen ORDER BY RS.data_group1 ASC";

                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        // RMC 20150522 corrections in reports (s)
                        sData1 = ""; sData2 = ""; sData3 = ""; sData4 = ""; sData5 = "";
                        sData6 = ""; sData7 = ""; sData8 = ""; sData9 = ""; sData10 = "";
                        sData11 = ""; sData12 = ""; sData13 = ""; sData14 = ""; sData15 = "";
                        sData16 = ""; sData17 = ""; sData18 = ""; sData19 = ""; sData20 = ""; sData21 = "";
                        sTotalTax = ""; sSurcharge = ""; sTotalCollect = "";
                        // RMC 20150522 corrections in reports (e)

                        sDateGen = pSet.GetDateTime("DT_GEN").ToShortDateString();
                        sDataGroup = pSet.GetString("data_group1").Trim();
                        
                        sRen = pSet.GetInt("ren_no").ToString();
                        sRenGross = pSet.GetDouble("ren_gross").ToString();
                        sNew = pSet.GetInt("new_no").ToString();
                        sNewGross = pSet.GetDouble("new_cap").ToString();
                        sRet = pSet.GetInt("ret_no").ToString();
                        sRetGross = pSet.GetDouble("ret_gross").ToString();

                        if (m_iBatchCnt == 1)   // RMC 20150522 corrections in reports
                        {
                            sData1 = pSet.GetDouble("data1").ToString();
                            sData2 = pSet.GetDouble("data2").ToString();
                            sData3 = pSet.GetDouble("data3").ToString();
                            sData4 = pSet.GetDouble("data4").ToString();
                            sData5 = pSet.GetDouble("data5").ToString();
                            sData6 = pSet.GetDouble("data6").ToString();
                            sData7 = pSet.GetDouble("data7").ToString();
                            sData8 = pSet.GetDouble("data8").ToString();
                            sData9 = pSet.GetDouble("data9").ToString();
                            sData10 = pSet.GetDouble("data10").ToString();
                        }
                        else// RMC 20150522 corrections in reports
                        {
                            sData11 = pSet.GetDouble("data11").ToString();
                            sData12 = pSet.GetDouble("data12").ToString();
                            sData13 = pSet.GetDouble("data13").ToString();
                            sData14 = pSet.GetDouble("data14").ToString();
                            sData15 = pSet.GetDouble("data15").ToString();
                            sData16 = pSet.GetDouble("data16").ToString();
                            sData17 = pSet.GetDouble("data17").ToString();
                            sData18 = pSet.GetDouble("data18").ToString();
                            sData19 = pSet.GetDouble("data19").ToString();
                            sData20 = pSet.GetDouble("data20").ToString();
                            sData21 = pSet.GetDouble("data21").ToString();
                        }
                        sSubData = pSet.GetDouble("SUBTOTALTAXES").ToString(); //MCR 20150128

                        sTotalTax = pSet.GetDouble("TOTALTAXES").ToString();
                        sSurcharge = pSet.GetDouble("sur_interest").ToString();
                        sTotalCollect = pSet.GetDouble("TOTALCOLLECTION").ToString();

                        #region Total

                        if (m_iBatchCnt == 1)
                        {
                            this.axVSPrinter1.Table = "<2000|>500|>1300|>500|>1300|>500|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;"
                                + sDataGroup + "|"
                                + sRen + "|"
                                + Convert.ToDouble(sRenGross).ToString("#,##0.00") + "|"
                                + sNew + "|"
                                + Convert.ToDouble(sNewGross).ToString("#,##0.00") + "|"
                                + sRet + "|"
                                + Convert.ToDouble(sRetGross).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData1).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData2).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData3).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData4).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData5).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData6).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData7).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData8).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData9).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData10).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sSubData).ToString("#,##0.00");
                        }
                        else
                        {
                            this.axVSPrinter1.Table = "<2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;"
                                + sDataGroup + "|"
                                + Convert.ToDouble(sData11).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData12).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData13).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData14).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData15).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData16).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData17).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData18).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData19).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData20).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sData21).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sTotalTax).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sSurcharge).ToString("#,##0.00") + "|"
                                  + Convert.ToDouble(sTotalCollect).ToString("#,##0.00");
                        }

                        if (m_iBatchCnt == 1)
                        {
                            dTREN += Convert.ToDouble(sRen);
                            dTNEW += Convert.ToDouble(sNew);
                            dTRET += Convert.ToDouble(sRet);
                            dTRenGross += Convert.ToDouble(sRenGross);
                            dTNewGross += Convert.ToDouble(sNewGross);
                            dTRetGross += Convert.ToDouble(sRetGross);

                            dTData1 += Convert.ToDouble(sData1);
                            dTData2 += Convert.ToDouble(sData2);
                            dTData3 += Convert.ToDouble(sData3);
                            dTData4 += Convert.ToDouble(sData4);
                            dTData5 += Convert.ToDouble(sData5);
                            dTData6 += Convert.ToDouble(sData6);
                            dTData7 += Convert.ToDouble(sData7);
                            dTData8 += Convert.ToDouble(sData8);
                            dTData9 += Convert.ToDouble(sData9);
                            dTData10 += Convert.ToDouble(sData10);
                            dTSubData += Convert.ToDouble(sSubData); //MCR 20150128
                        }
                        else //2
                        {
                            dTData11 += Convert.ToDouble(sData11);
                            dTData12 += Convert.ToDouble(sData12);
                            dTData13 += Convert.ToDouble(sData13);
                            dTData14 += Convert.ToDouble(sData14);
                            dTData15 += Convert.ToDouble(sData15);
                            dTData16 += Convert.ToDouble(sData16);
                            dTData17 += Convert.ToDouble(sData17);
                            dTData18 += Convert.ToDouble(sData18);
                            dTData19 += Convert.ToDouble(sData19);
                            dTData20 += Convert.ToDouble(sData20);
                            dTData21 += Convert.ToDouble(sData21);

                            dTTotalTax += Convert.ToDouble(sTotalTax);
                            dTSurcharge += Convert.ToDouble(sSurcharge);
                            dTTotalCollect += Convert.ToDouble(sTotalCollect);
                        }
                        #endregion
                                              
                    }
                    pSet.Close();
                }

                

                // RMC 20150126 (S)
                if (m_iBatchCnt == 1)
                {
                    this.axVSPrinter1.FontBold = true;
                    
                    this.axVSPrinter1.Paragraph = "";

                    //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28
                    this.axVSPrinter1.Table = ">2000|>500|>1300|>500|>1300|>500|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;TOTAL |"
                         + dTREN.ToString() + "|"
                         + dTRenGross.ToString("#,##0.00") + "|"
                         + dTNEW.ToString() + "|"
                         + dTNewGross.ToString("#,##0.00") + "|"
                         + dTRET.ToString() + "|"
                         + dTRetGross.ToString("#,##0.00") + "|"
                         + dTData1.ToString("#,##0.00") + "|"
                         + dTData2.ToString("#,##0.00") + "|"
                         + dTData3.ToString("#,##0.00") + "|"
                         + dTData4.ToString("#,##0.00") + "|"
                         + dTData5.ToString("#,##0.00") + "|"
                         + dTData6.ToString("#,##0.00") + "|"
                         + dTData7.ToString("#,##0.00") + "|"
                         + dTData8.ToString("#,##0.00") + "|"
                         + dTData9.ToString("#,##0.00") + "|"
                         + dTData10.ToString("#,##0.00") + "|"
                         + dTSubData.ToString("#,##0.00");
                }
                else
                {
                    this.axVSPrinter1.FontBold = true;
                   
                    this.axVSPrinter1.Paragraph = "";

                    //1    2    3      4   5      6    7      8    9    10     11    12    13    14    15   16    17   18     19    20    21   22     23   24     25    26    27    28

                    this.axVSPrinter1.Table = ">2000|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>1200;TOTAL|"
                              + dTData11.ToString("#,##0.00") + "|"
                              + dTData12.ToString("#,##0.00") + "|"
                              + dTData13.ToString("#,##0.00") + "|"
                              + dTData14.ToString("#,##0.00") + "|"
                              + dTData15.ToString("#,##0.00") + "|"
                              + dTData16.ToString("#,##0.00") + "|"
                              + dTData17.ToString("#,##0.00") + "|"
                              + dTData18.ToString("#,##0.00") + "|"
                              + dTData19.ToString("#,##0.00") + "|"
                              + dTData20.ToString("#,##0.00") + "|"
                              + dTData21.ToString("#,##0.00") + "|"
                              + dTTotalTax.ToString("#,##0.00") + "|"
                              + dTSurcharge.ToString("#,##0.00") + "|"
                              + dTTotalCollect.ToString("#,##0.00");
                }
                // RMC 20150126 (e)

            }
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + sDateGen;
        }

        private void CreateHeaderPage1Binan()
        {
            TopHeader(19600);
            this.axVSPrinter1.FontName = "Arial";
            String sHeader2 = "";
            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                sHeader2 = "^18000;Office of the City Treasurer";
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                sHeader2 = "^18000;Office of the Municipal Treasurer";
            this.axVSPrinter1.Table = (sHeader2);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ("^18000;" + m_sReportTitle);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 8;

            long lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.Table = ("<3000|<10000|<1500|<3500;" + "Fund: ||Covered Period:|");
            // RMC 20170123 Added RCD date option in Abstract of Teller (s)
            if (!m_bORDate)
                //this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "Fund: ||Covered Period (RCD Date):|");
                this.axVSPrinter1.Table = ("<3000|<7300|<2700|<5000;" + "Fund: ||Covered Period (RCD Date):|"); // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date
            else
                this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "Fund: ||Covered Period (OR Date):|");
            // RMC 20170123 Added RCD date option in Abstract of Teller (e)
            if (!m_bORDate)
                this.axVSPrinter1.Table = ("<3000|<7300|<2700|<5000;" + "Name of Accountable Officer: ||Page:|");   // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date
            else
                this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "Name of Accountable Officer: ||Page:|");
            this.axVSPrinter1.CurrentY = lngY1;
            this.axVSPrinter1.FontUnderline = true;
            string sDateFr = string.Empty;
            string sDateTo = string.Empty;
            sDateFr = string.Format("{0:MMMM dd, yyyy}", m_dtFrom);
            sDateTo = string.Format("{0:MMMM dd, yyyy}", m_dtTo);
            // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date (s)
            if (!m_bORDate)
            {
                if (sDateFr == sDateTo)
                    this.axVSPrinter1.Table = ("<3000|<7300|<2700|<5000;" + "|___________________________||" + sDateFr + " RCD No: " + m_sRCDNo);
                else
                    this.axVSPrinter1.Table = ("<3000|<7300|<2700|<5000;" + "|___________________________||" + sDateFr + " TO " + sDateTo + " RCD No: " + m_sRCDNo);
                this.axVSPrinter1.Table = ("<3000|<7300|<2700|<5000;" + "|" + AppSettingsManager.GetTeller(m_sTeller.Trim(), 0) + "||1-A");
            }// RMC 20170201 separate printing of Abstract by RCD No. if same RCD date (e)
            else
            {
                if (sDateFr == sDateTo)
                    this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "|___________________________||" + sDateFr);
                else
                    this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "|___________________________||" + sDateFr + " TO " + sDateTo);
                this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "|" + AppSettingsManager.GetTeller(m_sTeller.Trim(), 0) + "||1-A");
            }

            
            this.axVSPrinter1.FontUnderline = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.Table = ("<18000;A.COLLECTIONS");
            this.axVSPrinter1.Table = ("<18000;   For Collectors");
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

            string sFixed = "";
            sFixed = "DATE ISSUED";
            sFixed += "|Acct. Form No. 51";
            sFixed += "|BUSINESS NAME|";

            string sData = "";

            //arrTaxAbv8.Clear();
            for (int i = 0; i < arrTaxCode.Count; i++)
            {
                sData += arrTaxName[i].ToString() + "|";
                //arrTaxAbv8.Add(arrTaxName[i].ToString());
            }

            sData += "OTHER BUSS. TAXES";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            sFormat = "^1000|^1300|^2000|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300|^1300;";
            this.axVSPrinter1.Table = sFormat + sFixed + sData;

            sFormat = "<1000|<1300|<2000|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300|>1300;";
            this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.CurrentY = 4768;
        }

        private void CreateHeaderPage2Binan()
        {
            TopHeader(19600);
            this.axVSPrinter1.FontName = "Arial";
            String sHeader2 = "";
            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                sHeader2 = "^18000;Office of the City Treasurer";
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                sHeader2 = "^18000;Office of the Municipal Treasurer";
            this.axVSPrinter1.Table = (sHeader2);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ("^18000;" + m_sReportTitle);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 8;

            long lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.Table = ("<3000|<10000|<1500|<3500;" + "Fund: ||Covered Period:|");
            // RMC 20170123 Added RCD date option in Abstract of Teller (s)
            if (!m_bORDate)
                //this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "Fund: ||Covered Period (RCD Date):|");
                this.axVSPrinter1.Table = ("<3000|<7300|<2700|<5000;" + "Fund: ||Covered Period (RCD Date):|"); // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date
            else
                this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "Fund: ||Covered Period (OR Date):|");
            // RMC 20170123 Added RCD date option in Abstract of Teller (e)
            if (!m_bORDate)
                this.axVSPrinter1.Table = ("<3000|<7300|<2700|<5000;" + "Name of Accountable Officer: ||Page:|");   // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date
            else
                this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "Name of Accountable Officer: ||Page:|");
            this.axVSPrinter1.CurrentY = lngY1;
            this.axVSPrinter1.FontUnderline = true;
            string sDateFr = string.Empty;
            string sDateTo = string.Empty;
            sDateFr = string.Format("{0:MMMM dd, yyyy}", m_dtFrom);
            sDateTo = string.Format("{0:MMMM dd, yyyy}", m_dtTo);
            // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date (s)
            if (!m_bORDate)
            {
                if (sDateFr == sDateTo)
                    this.axVSPrinter1.Table = ("<3000|<7300|<2700|<5000;" + "|___________________________||" + sDateFr + " RCD No: " + m_sRCDNo);
                else
                    this.axVSPrinter1.Table = ("<3000|<7300|<2700|<5000;" + "|___________________________||" + sDateFr + " TO " + sDateTo + " RCD No: " + m_sRCDNo);
                this.axVSPrinter1.Table = ("<3000|<7300|<2700|<5000;" + "|" + AppSettingsManager.GetTeller(m_sTeller.Trim(), 0) + "||1-B");
            }// RMC 20170201 separate printing of Abstract by RCD No. if same RCD date (e)
            else
            {
                if (sDateFr == sDateTo)
                    this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "|___________________________||" + sDateFr);
                else
                    this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "|___________________________||" + sDateFr + " TO " + sDateTo);
                this.axVSPrinter1.Table = ("<3000|<8800|<2700|<3500;" + "|" + AppSettingsManager.GetTeller(m_sTeller.Trim(), 0) + "||1-B");
            }
            
            this.axVSPrinter1.FontUnderline = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.Table = ("<18000;A.COLLECTIONS");
            this.axVSPrinter1.Table = ("<18000;   For Collectors");
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

            string sFixed = "";
            sFixed = "Acct. Form No. 51";
            sFixed += "|BUSINESS NAME|";


            string sData = "";

            //arrTaxAbv8.Clear();
            for (int i = 0; i < arrColumnCode.Count; i++)
            {
                sData += arrColumnName[i].ToString() + "|";
                //arrTaxAbv8.Add(arrColumnCode[i].ToString());
            }

            sData += "OTHER FEES|SURCH & INT|EXCESS CQ|APPLIED TC|TOTAL";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            sFormat = "^1100|^2000|^950|^950|^950|^950|^950|^950|^950|^950|^950|^950|^950|^950|^800|^800|^1400;";
            this.axVSPrinter1.Table = sFormat + sFixed + sData;

            sFormat = "<1100|<2000|>950|>950|>950|>950|>950|>950|>950|>950|>950|>950|>950|>950|>800|>800|>1400;";

            this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.CurrentY = 4768;
        }

        private void ReportAbstractTellerBinan()
        {
            // RMC 20161230 modified abstract teller report for Binan
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);

            pSet.Query = @"select Count(distinct(or_no)) as Cnt from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and teller like '" + m_sTeller + "' order by or_no";
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            string sOrNo = "";
            string sOrDate = "";
            string sBIN = "";
            string sBnsName = "";
            string sOrDetailsData = "";
            DateTime dtOrDate;
            double dTaxData1 = 0;
            double dTaxData2 = 0;
            double dTaxData3 = 0;
            double dTaxData4 = 0;
            double dTaxData5 = 0;
            double dTaxData6 = 0;
            double dTaxData7 = 0;
            double dTaxData8 = 0;
            double dTaxData9 = 0;
            double dTaxDataOth = 0;
            double dTotTax = 0;
            double dTotal = 0;

            double dFeeData1 = 0;
            double dFeeData2 = 0;
            double dFeeData3 = 0;
            double dFeeData4 = 0;
            double dFeeData5 = 0;
            double dFeeData6 = 0;
            double dFeeData7 = 0;
            double dFeeData8 = 0;
            double dFeeData9 = 0;
            double dFeeData10 = 0;
            double dFeeDataOth = 0;
            double dSurchInt = 0;
            double dTotFee = 0;
            double dExcessCQ = 0;
            double dAppTC = 0;

            pSet.Query = "select distinct(or_no) as or_no, bin, or_date from pay_hist where or_date ";
            pSet.Query += " between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and ";
            pSet.Query += " to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' ";
            pSet.Query += " and teller = '" + m_sTeller + "' order by or_no";   // RMC 20170112 addl mods in Abstract by teller
            
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sOrNo = pSet.GetString("or_no");
                    dtOrDate = pSet.GetDateTime("or_date");
                    sOrDate = string.Format("{0:MM/dd/yyyy}", dtOrDate);
                    sBIN = pSet.GetString("bin");
                    sBnsName = AppSettingsManager.GetBnsName(sBIN);
                    sOrDetailsData = sOrDate + "|" + sOrNo + "|" + sBnsName + "|";

                    for (int i = 0; i < arrTaxCode.Count; i++)
                    {
                        result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code like 'B%' and substr(bns_code_main,1,2) = '" + arrTaxCode[i].ToString().Trim() + "'";

                        if (result1.Execute())
                        {
                            if (result1.Read())
                            {
                                sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";

                                if (i == 0)
                                    dTaxData1 += result1.GetDouble("fees_due");
                                else if (i == 1)
                                    dTaxData2 += result1.GetDouble("fees_due");
                                else if (i == 2)
                                    dTaxData3 += result1.GetDouble("fees_due");
                                else if (i == 3)
                                    dTaxData4 += result1.GetDouble("fees_due");
                                else if (i == 4)
                                    dTaxData5 += result1.GetDouble("fees_due");
                                else if (i == 5)
                                    dTaxData6 += result1.GetDouble("fees_due");
                                else if (i == 6)
                                    dTaxData7 += result1.GetDouble("fees_due");
                                else if (i == 7)
                                    dTaxData8 += result1.GetDouble("fees_due");
                                else if (i == 8)
                                    dTaxData9 += result1.GetDouble("fees_due");
                            }
                        }
                        result1.Close();
                    }

                   // dTotTax = dTaxData1 + dTaxData2 + dTaxData3 + dTaxData4 + dTaxData5 + dTaxData6 + dTaxData7 + dTaxData8 + dTaxData9;

                    //result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code like 'B%' and substr(bns_code_main,1,2) not in (select substr(fees_code,1,2) from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "')" ;
                    result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code like 'B%' and 'B'||substr(bns_code_main,1,2) not in  (select fees_code from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "')";
                    if (result1.Execute())
                    {
                        if (result1.Read())
                        {
                            sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";
                            dTaxDataOth += result1.GetDouble("fees_due");
                        }
                    }
                    result1.Close();

                    

                    this.axVSPrinter1.Table = sFormat + sOrDetailsData;
                    
                    
                }
            }
            pSet.Close();

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = sFormat + "||TOTAL|" + dTaxData1.ToString("#,##0.00") + "|" + dTaxData2.ToString("#,##0.00") + "|" + dTaxData3.ToString("#,##0.00") + "|" + dTaxData4.ToString("#,##0.00") + "|" + dTaxData5.ToString("#,##0.00") + "|" + dTaxData6.ToString("#,##0.00") + "|" + dTaxData7.ToString("#,##0.00") + "|" + dTaxData8.ToString("#,##0.00") + "|" + dTaxData9.ToString("#,##0.00") + "|" + dTaxDataOth.ToString("#,##0.00");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");

            m_bPage2 = true;
            this.axVSPrinter1.NewPage();

            sOrNo = "";
            sOrDate = "";
            sBIN = "";
            sBnsName = "";
            sOrDetailsData = "";
            string sAppliedTC = string.Empty;

            pSet.Query = "select distinct(or_no) as or_no, bin, or_date from pay_hist where or_date between ";
            pSet.Query += "to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and ";
            pSet.Query += "to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' ";
            pSet.Query += "and teller = '" + m_sTeller + "' order by or_no";    // RMC 20170112 addl mods in Abstract by teller
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sOrNo = pSet.GetString("or_no");
                    dtOrDate = pSet.GetDateTime("or_date");
                    sOrDate = string.Format("{0:MM/dd/yyyy}", dtOrDate);
                    sBIN = pSet.GetString("bin");
                    sBnsName = AppSettingsManager.GetBnsName(sBIN);
                    sOrDetailsData = sOrNo + "|" + sBnsName + "|";

                    for (int i = 0; i < arrColumnCode.Count; i++)
                    {
                        result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code not like 'B%' and fees_code = '" + arrColumnCode[i].ToString().Trim() + "'";

                        if (result1.Execute())
                        {
                            if (result1.Read())
                            {
                                sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";

                                if (i == 0)
                                    dFeeData1 += result1.GetDouble("fees_due");
                                else if (i == 1)
                                    dFeeData2 += result1.GetDouble("fees_due");
                                else if (i == 2)
                                    dFeeData3 += result1.GetDouble("fees_due");
                                else if (i == 3)
                                    dFeeData4 += result1.GetDouble("fees_due");
                                else if (i == 4)
                                    dFeeData5 += result1.GetDouble("fees_due");
                                else if (i == 5)
                                    dFeeData6 += result1.GetDouble("fees_due");
                                else if (i == 6)
                                    dFeeData7 += result1.GetDouble("fees_due");
                                else if (i == 7)
                                    dFeeData8 += result1.GetDouble("fees_due");
                                else if (i == 8)
                                    dFeeData9 += result1.GetDouble("fees_due");
                                else if (i == 9)
                                    dFeeData10 += result1.GetDouble("fees_due");
                                
                            }
                        }
                        result1.Close();
                    }

                    //dTotFee += dFeeData1 + dFeeData2 + dFeeData3 + dFeeData4 + dFeeData5 + dFeeData6 + dFeeData7 + dFeeData8 + dFeeData9;

                    //result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code like 'B%' and substr(bns_code_main,1,2) not in (select substr(fees_code,1,2) from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "')" ;
                    result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code not like 'B%' and fees_code not in  (select fees_code from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "')";
                    if (result1.Execute())
                    {
                        if (result1.Read())
                        {
                            sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";
                            dFeeDataOth += result1.GetDouble("fees_due");
                        }
                    }
                    result1.Close();

                    //dTotFee += dFeeDataOth;

                    double dSurch = 0;
                    double dPen = 0;

                    result1.Query = "select sum(fees_surch) as fees_surch, sum(fees_pen) as fees_pen from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS')";
                    if (result1.Execute())
                        if (result1.Read())
                        {
                            dSurch = result1.GetDouble("fees_surch");
                            dPen = result1.GetDouble("fees_pen");
                            sOrDetailsData += string.Format("{0:#,##0.00}", dSurch + dPen) + "|";
                            dSurchInt += dSurch + dPen;
                            
                            
                        }
                    result1.Close();

                    

                    //
                    double dExcess = 0;
                    result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtOrDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtOrDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and multi_pay = 'N' and or_no = '" + sOrNo + "'"; //JARS 20171010
                    if (result1.Execute())
                        if (result1.Read())
                        {
                            try
                            { dExcess = result1.GetDouble("balance"); }
                            catch
                            { dExcess = 0; }
                           
                        }
                    result1.Close();

                    sOrDetailsData += string.Format("{0:#,##0.00}", dExcess) + "|";
                    dExcessCQ += dExcess;

                    dTotal += dExcess;

                    double dAppliedTC = 0;

                    result1.Query = "select sum(fees_amtdue) as tot_fee from or_table where or_no in (select or_no from pay_hist where or_date = to_date('" + dtOrDate.ToShortDateString() + "','MM/dd/yyyy') and or_no = '" + sOrNo + "' and payment_type = 'TC' and data_mode <> 'POS')";
                    if (result1.Execute())
                        if (result1.Read())
                        {
                            try
                            { dAppliedTC = result1.GetDouble("tot_fee"); }
                            catch
                            { dAppliedTC = 0; }
                            
                        }
                    result1.Close();

                    // RMC 20170127 added addtl query for applied TC in Abstract by teller report (s)
                    result1.Query = "select * from pay_hist where or_no = '" + sOrNo + "' and (payment_type = 'CSTC' or payment_type = 'CQTC' or payment_type = 'CCTC') and data_mode <> 'POS'";
                    if (result1.Execute())
                    {
                        if (result1.Read())
                        {
                            OracleResultSet pApplied = new OracleResultSet();

                            pApplied.Query = "select sum(debit) as tot_fee from dbcr_memo where or_no = '" + sOrNo + "' and served = 'Y'"; //JARS 20171010
                            if (pApplied.Execute())
                            {
                                if (pApplied.Read())
                                {
                                    try
                                    { dAppliedTC += pApplied.GetDouble("tot_fee"); }
                                    catch
                                    { dAppliedTC += 0; }
                                }
                            }
                            pApplied.Close();
                        }
                    }
                    result1.Close();
                    // RMC 20170127 added addtl query for applied TC in Abstract by teller report (e)

                    //sOrDetailsData += string.Format("{0:#,##0.#0}", dAppliedTC) + "|";    // RMC 20170221 added parenthesis in Applied tax credit in Abstract of collection report, put rem

                    // RMC 20170221 added parenthesis in Applied tax credit in Abstract of collection report (s)
                    sAppliedTC = string.Format("{0:#,##0.#0}", dAppliedTC);

                    if (dAppliedTC != 0)
                        sAppliedTC = "-" + sAppliedTC;

                    sOrDetailsData += sAppliedTC + "|";
                    // RMC 20170221 added parenthesis in Applied tax credit in Abstract of collection report (e)
                    dAppTC += dAppliedTC;
                    dTotal = dTotal - dAppliedTC;

                    double dRowTotal = 0;
                    result1.Query = "select sum(fees_amtdue) as fees_amtdue from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS')";
                    if (result1.Execute())
                        if (result1.Read())
                        {
                            dRowTotal = result1.GetDouble("fees_amtdue");
                            dRowTotal = dRowTotal + dExcess - dAppliedTC;
                            sOrDetailsData += string.Format("{0:#,##0.00}", dRowTotal);
                            dTotal += result1.GetDouble("fees_amtdue");
                        }
                    result1.Close();

                    this.axVSPrinter1.Table = sFormat + sOrDetailsData;
                    

                }
            }
            pSet.Close();

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.Table = sFormat + "|TOTAL|" + dFeeData1.ToString("#,##0.00") + "|" + dFeeData2.ToString("#,##0.00") + "|" + dFeeData3.ToString("#,##0.00") + "|" + dFeeData4.ToString("#,##0.00") + "|" + dFeeData5.ToString("#,##0.00") + "|" + dFeeData6.ToString("#,##0.00") + "|" + dFeeData7.ToString("#,##0.00") + "|" + dFeeData8.ToString("#,##0.00") + "|" + dFeeData9.ToString("#,##0.00") + "|" + dFeeData10.ToString("#,##0.00") + "|" + dFeeDataOth.ToString("#,##0.00") + "|" + dSurchInt.ToString("#,##0.00") + "|" + dExcessCQ.ToString("#,##0.00") + "|" + dAppTC.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00");

            // RMC 20170221 added parenthesis in Applied tax credit in Abstract of collection report (s)
            sAppliedTC = string.Format("{0:#,##0.#0}", dAppTC);

            if (dAppTC != 0)
                sAppliedTC = "-" + sAppliedTC;

            this.axVSPrinter1.Table = sFormat + "|TOTAL|" + dFeeData1.ToString("#,##0.00") + "|" + dFeeData2.ToString("#,##0.00") + "|" + dFeeData3.ToString("#,##0.00") + "|" + dFeeData4.ToString("#,##0.00") + "|" + dFeeData5.ToString("#,##0.00") + "|" + dFeeData6.ToString("#,##0.00") + "|" + dFeeData7.ToString("#,##0.00") + "|" + dFeeData8.ToString("#,##0.00") + "|" + dFeeData9.ToString("#,##0.00") + "|" + dFeeData10.ToString("#,##0.00") + "|" + dFeeDataOth.ToString("#,##0.00") + "|" + dSurchInt.ToString("#,##0.00") + "|" + dExcessCQ.ToString("#,##0.00") + "|" + sAppliedTC + "|" + dTotal.ToString("#,##0.00");
            // RMC 20170221 added parenthesis in Applied tax credit in Abstract of collection report (e)

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");


            DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
            Thread.Sleep(3);




            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            pSet.Query = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportTitle, AppSettingsManager.SystemUser.UserCode);
            pSet.ExecuteNonQuery();
            
        }

        private bool CheckMultiCheckPay(string sOrNo)
        {
            // RMC 20160105 corrected printing of OR if multiple transaction
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from dbcr_memo where or_no = '" + sOrNo + "' and multi_pay = 'Y'"; //JARS 20171010
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

            return false;
        }

        private void ReportAbstractTellerBinanRCD()
        {
            // RMC 20170123 Added RCD date option in Abstract of Teller
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);

            pSet.Query = @"select Count(distinct(or_no)) as Cnt from pay_hist where or_date between to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and teller like '" + m_sTeller + "' order by or_no";
            int.TryParse(pSet.ExecuteScalar(), out intCount);
            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            string sOrNo = "";
            string sOrDate = "";
            string sBIN = "";
            string sBnsName = "";
            string sOrDetailsData = "";
            DateTime dtOrDate;
            double dTaxData1 = 0;
            double dTaxData2 = 0;
            double dTaxData3 = 0;
            double dTaxData4 = 0;
            double dTaxData5 = 0;
            double dTaxData6 = 0;
            double dTaxData7 = 0;
            double dTaxData8 = 0;
            double dTaxData9 = 0;
            double dTaxDataOth = 0;
            double dTotTax = 0;
            double dTotal = 0;

            double dFeeData1 = 0;
            double dFeeData2 = 0;
            double dFeeData3 = 0;
            double dFeeData4 = 0;
            double dFeeData5 = 0;
            double dFeeData6 = 0;
            double dFeeData7 = 0;
            double dFeeData8 = 0;
            double dFeeData9 = 0;
            double dFeeData10 = 0;
            double dFeeDataOth = 0;
            double dSurchInt = 0;
            double dTotFee = 0;
            double dExcessCQ = 0;
            double dAppTC = 0;

            OracleResultSet pRec = new OracleResultSet();
            pRec.Query = "select * from partial_remit where teller = '" + m_sTeller + "' ";
            pRec.Query += "and to_date(to_char(dt_save,'MM/dd/yyyy'),'MM/dd/yyyy') between ";
            pRec.Query += "to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and ";
            pRec.Query += "to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') ";
            pRec.Query += "and rcd_series = '" + m_sRCDNo + "' ";   // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date
            pRec.Query += "order by or_from";
            if (pRec.Execute())
            {
                string sOrFrom = string.Empty;
                string sOrTo = string.Empty;

                while (pRec.Read())
                {
                    sOrFrom = pRec.GetString("or_from");
                    sOrTo = pRec.GetString("or_to");

                    pSet.Query = "select distinct(or_no) as or_no, bin, or_date from pay_hist where teller = '" + m_sTeller + "' ";
                    pSet.Query += " and or_no between '" + sOrFrom + "' and '" + sOrTo + "' and data_mode <> 'POS'";
                    pSet.Query += " order by or_no";    // RMC 20170126 added sorting in Abstract report by teller
                    if (pSet.Execute())
                    {
                        while (pSet.Read())
                        {
                            DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                            intCountIncreament += 1;

                            sOrNo = pSet.GetString("or_no");
                            dtOrDate = pSet.GetDateTime("or_date");
                            sOrDate = string.Format("{0:MM/dd/yyyy}", dtOrDate);
                            sBIN = pSet.GetString("bin");
                            sBnsName = AppSettingsManager.GetBnsName(sBIN);
                            sOrDetailsData = sOrDate + "|" + sOrNo + "|" + sBnsName + "|";

                            for (int i = 0; i < arrTaxCode.Count; i++)
                            {
                                result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code like 'B%' and substr(bns_code_main,1,2) = '" + arrTaxCode[i].ToString().Trim() + "'";

                                if (result1.Execute())
                                {
                                    if (result1.Read())
                                    {
                                        sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";

                                        if (i == 0)
                                            dTaxData1 += result1.GetDouble("fees_due");
                                        else if (i == 1)
                                            dTaxData2 += result1.GetDouble("fees_due");
                                        else if (i == 2)
                                            dTaxData3 += result1.GetDouble("fees_due");
                                        else if (i == 3)
                                            dTaxData4 += result1.GetDouble("fees_due");
                                        else if (i == 4)
                                            dTaxData5 += result1.GetDouble("fees_due");
                                        else if (i == 5)
                                            dTaxData6 += result1.GetDouble("fees_due");
                                        else if (i == 6)
                                            dTaxData7 += result1.GetDouble("fees_due");
                                        else if (i == 7)
                                            dTaxData8 += result1.GetDouble("fees_due");
                                        else if (i == 8)
                                            dTaxData9 += result1.GetDouble("fees_due");
                                    }
                                }
                                result1.Close();
                            }

                            // dTotTax = dTaxData1 + dTaxData2 + dTaxData3 + dTaxData4 + dTaxData5 + dTaxData6 + dTaxData7 + dTaxData8 + dTaxData9;

                            //result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code like 'B%' and substr(bns_code_main,1,2) not in (select substr(fees_code,1,2) from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "')" ;
                            result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code like 'B%' and 'B'||substr(bns_code_main,1,2) not in  (select fees_code from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "')";
                            if (result1.Execute())
                            {
                                if (result1.Read())
                                {
                                    sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";
                                    dTaxDataOth += result1.GetDouble("fees_due");
                                }
                            }
                            result1.Close();

                            this.axVSPrinter1.Table = sFormat + sOrDetailsData;

                        }
                    }
                    pSet.Close();
                }
            }
            pRec.Close();

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = sFormat + "||TOTAL|" + dTaxData1.ToString("#,##0.00") + "|" + dTaxData2.ToString("#,##0.00") + "|" + dTaxData3.ToString("#,##0.00") + "|" + dTaxData4.ToString("#,##0.00") + "|" + dTaxData5.ToString("#,##0.00") + "|" + dTaxData6.ToString("#,##0.00") + "|" + dTaxData7.ToString("#,##0.00") + "|" + dTaxData8.ToString("#,##0.00") + "|" + dTaxData9.ToString("#,##0.00") + "|" + dTaxDataOth.ToString("#,##0.00");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");

            m_bPage2 = true;
            this.axVSPrinter1.NewPage();


            sOrNo = "";
            sOrDate = "";
            sBIN = "";
            sBnsName = "";
            sOrDetailsData = "";
            string sAppliedTC = string.Empty;

            pRec.Query = "select * from partial_remit where teller = '" + m_sTeller + "' ";
            pRec.Query += "and to_date(to_char(dt_save,'MM/dd/yyyy'),'MM/dd/yyyy') between ";
            pRec.Query += "to_date('" + m_dtFrom.ToShortDateString() + "','MM/dd/yyyy') and ";
            pRec.Query += "to_date('" + m_dtTo.ToShortDateString() + "','MM/dd/yyyy') ";
            pRec.Query += "and rcd_series = '" + m_sRCDNo + "' ";   // RMC 20170201 separate printing of Abstract by RCD No. if same RCD date
            pRec.Query += "order by or_from";
            if (pRec.Execute())
            {
                string sOrFrom = string.Empty;
                string sOrTo = string.Empty;
                
                while (pRec.Read())
                {
                    sOrFrom = pRec.GetString("or_from");
                    sOrTo = pRec.GetString("or_to");

                    pSet.Query = "select distinct(or_no) as or_no, bin, or_date from pay_hist where teller = '" + m_sTeller + "' ";
                    pSet.Query += " and or_no between '" + sOrFrom + "' and '" + sOrTo + "' and data_mode <> 'POS'";
                    pSet.Query += " order by or_no";    // RMC 20170126 added sorting in Abstract report by teller
                    if (pSet.Execute())
                    {
                        while (pSet.Read())
                        {
                            DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                            intCountIncreament += 1;

                            sOrNo = pSet.GetString("or_no");
                            dtOrDate = pSet.GetDateTime("or_date");
                            sOrDate = string.Format("{0:MM/dd/yyyy}", dtOrDate);
                            sBIN = pSet.GetString("bin");
                            sBnsName = AppSettingsManager.GetBnsName(sBIN);
                            sOrDetailsData = sOrNo + "|" + sBnsName + "|";

                            for (int i = 0; i < arrColumnCode.Count; i++)
                            {
                                result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code not like 'B%' and fees_code = '" + arrColumnCode[i].ToString().Trim() + "'";

                                if (result1.Execute())
                                {
                                    if (result1.Read())
                                    {
                                        sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";

                                        if (i == 0)
                                            dFeeData1 += result1.GetDouble("fees_due");
                                        else if (i == 1)
                                            dFeeData2 += result1.GetDouble("fees_due");
                                        else if (i == 2)
                                            dFeeData3 += result1.GetDouble("fees_due");
                                        else if (i == 3)
                                            dFeeData4 += result1.GetDouble("fees_due");
                                        else if (i == 4)
                                            dFeeData5 += result1.GetDouble("fees_due");
                                        else if (i == 5)
                                            dFeeData6 += result1.GetDouble("fees_due");
                                        else if (i == 6)
                                            dFeeData7 += result1.GetDouble("fees_due");
                                        else if (i == 7)
                                            dFeeData8 += result1.GetDouble("fees_due");
                                        else if (i == 8)
                                            dFeeData9 += result1.GetDouble("fees_due");
                                        else if (i == 9)
                                            dFeeData10 += result1.GetDouble("fees_due");

                                    }
                                }
                                result1.Close();
                            }

                            //dTotFee += dFeeData1 + dFeeData2 + dFeeData3 + dFeeData4 + dFeeData5 + dFeeData6 + dFeeData7 + dFeeData8 + dFeeData9;

                            //result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code like 'B%' and substr(bns_code_main,1,2) not in (select substr(fees_code,1,2) from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "')" ;
                            result1.Query = "select sum(fees_due) as fees_due from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code not like 'B%' and fees_code not in  (select fees_code from btax_rep_abstract where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and report_name = '" + m_sReportTitle + "')";
                            if (result1.Execute())
                            {
                                if (result1.Read())
                                {
                                    sOrDetailsData += string.Format("{0:#,##0.00}", result1.GetDouble("fees_due")) + "|";
                                    dFeeDataOth += result1.GetDouble("fees_due");
                                }
                            }
                            result1.Close();

                            //dTotFee += dFeeDataOth;

                            double dSurch = 0;
                            double dPen = 0;

                            result1.Query = "select sum(fees_surch) as fees_surch, sum(fees_pen) as fees_pen from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS')";
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    dSurch = result1.GetDouble("fees_surch");
                                    dPen = result1.GetDouble("fees_pen");
                                    sOrDetailsData += string.Format("{0:#,##0.00}", dSurch + dPen) + "|";
                                    dSurchInt += dSurch + dPen;


                                }
                            result1.Close();



                            //
                            double dExcess = 0; //JARS 20171010
                            result1.Query = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where or_date = to_date('" + dtOrDate.ToShortDateString() + "','MM/dd/yyyy') and data_mode <> 'POS' and or_date = to_date('" + dtOrDate.ToShortDateString() + "','MM/dd/yyyy')) and served = 'N' and multi_pay = 'N' and or_no = '" + sOrNo + "'";
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    try
                                    { dExcess = result1.GetDouble("balance"); }
                                    catch
                                    { dExcess = 0; }

                                }
                            result1.Close();

                            sOrDetailsData += string.Format("{0:#,##0.00}", dExcess) + "|";
                            dExcessCQ += dExcess;

                            dTotal += dExcess;

                            double dAppliedTC = 0;

                            result1.Query = "select sum(fees_amtdue) as tot_fee from or_table where or_no in (select or_no from pay_hist where or_date = to_date('" + dtOrDate.ToShortDateString() + "','MM/dd/yyyy') and or_no = '" + sOrNo + "' and payment_type = 'TC' and data_mode <> 'POS')";
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    try
                                    { dAppliedTC = result1.GetDouble("tot_fee"); }
                                    catch
                                    { dAppliedTC = 0; }

                                }
                            result1.Close();

                            // RMC 20170127 added addtl query for applied TC in Abstract by teller report (s)
                            result1.Query = "select * from pay_hist where or_no = '" + sOrNo + "' and (payment_type = 'CSTC' or payment_type = 'CQTC' or payment_type = 'CCTC') and data_mode <> 'POS'";
                            if (result1.Execute())
                            {
                                if (result1.Read())
                                {
                                    OracleResultSet pApplied = new OracleResultSet();

                                    pApplied.Query = "select sum(debit) as tot_fee from dbcr_memo where or_no = '" + sOrNo + "' and served = 'Y'"; //JARS 20171010
                                    if (pApplied.Execute())
                                    {
                                        if (pApplied.Read())
                                        {
                                            try
                                            { dAppliedTC += pApplied.GetDouble("tot_fee"); }
                                            catch
                                            { dAppliedTC += 0; }
                                        }
                                    }
                                    pApplied.Close();
                                }
                            }
                            result1.Close();
                            // RMC 20170127 added addtl query for applied TC in Abstract by teller report (e)

                            //sOrDetailsData += string.Format("{0:#,##0.#0}", dAppliedTC) + "|";    // RMC 20170221 added parenthesis in Applied tax credit in Abstract of collection report, put rem

                            // RMC 20170221 added parenthesis in Applied tax credit in Abstract of collection report (s)
                            sAppliedTC = string.Format("{0:#,##0.#0}", dAppliedTC);

                            if (dAppliedTC != 0)
                                sAppliedTC = "-" + sAppliedTC;

                            sOrDetailsData += sAppliedTC + "|";
                            // RMC 20170221 added parenthesis in Applied tax credit in Abstract of collection report (e)

                            dAppTC += dAppliedTC;
                            dTotal = dTotal - dAppliedTC;

                            double dRowTotal = 0;
                            result1.Query = "select sum(fees_amtdue) as fees_amtdue from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS')";
                            if (result1.Execute())
                                if (result1.Read())
                                {
                                    dRowTotal = result1.GetDouble("fees_amtdue");
                                    dRowTotal = dRowTotal + dExcess - dAppliedTC;
                                    sOrDetailsData += string.Format("{0:#,##0.00}", dRowTotal);
                                    dTotal += result1.GetDouble("fees_amtdue");
                                }
                            result1.Close();

                            this.axVSPrinter1.Table = sFormat + sOrDetailsData;

                        }
                    }
                    pSet.Close();
                }
            }
            pRec.Close();

            //m_bPage2 = false; // RMC 20170221 correction in Abstract by Teller report, put rem

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.Table = sFormat + "|TOTAL|" + dFeeData1.ToString("#,##0.00") + "|" + dFeeData2.ToString("#,##0.00") + "|" + dFeeData3.ToString("#,##0.00") + "|" + dFeeData4.ToString("#,##0.00") + "|" + dFeeData5.ToString("#,##0.00") + "|" + dFeeData6.ToString("#,##0.00") + "|" + dFeeData7.ToString("#,##0.00") + "|" + dFeeData8.ToString("#,##0.00") + "|" + dFeeData9.ToString("#,##0.00") + "|" + dFeeData10.ToString("#,##0.00") + "|" + dFeeDataOth.ToString("#,##0.00") + "|" + dSurchInt.ToString("#,##0.00") + "|" + dExcessCQ.ToString("#,##0.00") + "|" + dAppTC.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00");

            // RMC 20170221 added parenthesis in Applied tax credit in Abstract of collection report (s)
            sAppliedTC = string.Format("{0:#,##0.#0}", dAppTC);

            if (dAppTC != 0)
                sAppliedTC = "-" + sAppliedTC;

            this.axVSPrinter1.Table = sFormat + "|TOTAL|" + dFeeData1.ToString("#,##0.00") + "|" + dFeeData2.ToString("#,##0.00") + "|" + dFeeData3.ToString("#,##0.00") + "|" + dFeeData4.ToString("#,##0.00") + "|" + dFeeData5.ToString("#,##0.00") + "|" + dFeeData6.ToString("#,##0.00") + "|" + dFeeData7.ToString("#,##0.00") + "|" + dFeeData8.ToString("#,##0.00") + "|" + dFeeData9.ToString("#,##0.00") + "|" + dFeeData10.ToString("#,##0.00") + "|" + dFeeDataOth.ToString("#,##0.00") + "|" + dSurchInt.ToString("#,##0.00") + "|" + dExcessCQ.ToString("#,##0.00") + "|" + sAppliedTC + "|" + dTotal.ToString("#,##0.00");
            // RMC 20170221 added parenthesis in Applied tax credit in Abstract of collection report (e)

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = "<2500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2500|<5000;Date Generated :|" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");

            m_bPage2 = false;   // RMC 20170221 correction in Abstract by Teller report
            DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
            Thread.Sleep(3);




            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            pSet.Query = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportTitle, AppSettingsManager.SystemUser.UserCode);
            pSet.ExecuteNonQuery();
        }

        public void ReportPaymentHist(string sBin) 
        {
            DateTime dDate = AppSettingsManager.GetCurrentDate();   // RMC 20110725
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
            //JARS 20171024 ON SITE (S)
            string sFeesCode = string.Empty;
            string sPrevTaxYear = string.Empty;
            double dTax = 0;
            double dFees = 0;
            double dSurPen = 0;
            double dTotal = 0;
            //JARS 20171024 ON SITE (E)
            string sNoOfQtr = string.Empty;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            long lngY1 = 0;
            long lngY2 = 0;
            //this.ExportFile();
            //this.axVSPrinter1.Clear();
            //sImagepath = "C:\\Documents and Settings\\Godfrey\\My Documents\\Downloads\\logo_header.jpg";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.FontSize = (float)15;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11500;Payment History";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<1700|<9800;Available Record as of|" + AppSettingsManager.MonthsInWords(dDate);
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.FontSize = (float)15;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11500;" + sBin;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<11500;Business Identification Number";
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";

            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.Table = "<11500; \n \n \n \n \n \n \n";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY1 + 200;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<1500|<500|<9000;Business Name\nAddress\nOwner's Name\nAddress\nBusiness Plate\nPermit No.|:\n:\n:\n:\n:\n:\n|";   // RMC 20171122 added printing of current permit no. in Payment hist printing
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY1 + 200;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<1500|<500|<9000;|| {0}\n{1}\n {2}\n{3}\n {4}\n {5}",
                AppSettingsManager.GetBnsName(sBin.Trim()),AppSettingsManager.GetBnsAdd(sBin.Trim(),""),AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin.Trim())),
                AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetOwnCode(sBin.Trim())), AppSettingsManager.GetBnsPlate(sBin.Trim()),
                AppSettingsManager.GetPermitNo(sBin.Trim()));  // RMC 20171122 added printing of current permit no. in Payment hist printing
           
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY1 + 300;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontBold = true;
            // this.axVSPrinter1.Table = "<958|<958|<958|<958|<958|<958|<958|<958|<958|<958|<958|<958;Date|OR#|Tax Year|Capital|Gross|Term|QTR|No. of QTR|Tax|Fees|Sur/Pen|Total";
            this.axVSPrinter1.Table = "^958|^1539|^800|^1200|^1200|^600|^400|^800|^1000|^1000|^1000|^1000;Date|OR#|Tax Year|Capital|Gross|Term|QTR|No. of QTR|Tax|Fees|Sur/Pen|Total";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            string sBnsCode = string.Empty;
            string cOrNo = string.Empty;
            //result.Query = "select distinct * from pay_hist_temp where bin ='" + sBin + "' order by or_date asc, tax_year, qtr asc";
            result.Query = "select * from pay_hist where bin = '"+ sBin +"' order by or_date, tax_year, qtr_paid"; //JARS 20171024 ON SITE
            if (result.Execute())
            {
                while (result.Read())
                {
                    sDate = result.GetDateTime("or_date").ToShortDateString();
                    sTxYr = result.GetString("tax_year");
                    sOrNo = result.GetString("or_no");
                    //sTerm = result.GetString("term");
                    //sQtr = result.GetString("qtr");
                    sTerm = result.GetString("payment_term");
                    sQtr = result.GetString("qtr_paid");

                    //JARS 20171024 ON SITE (S)
                    dTax = 0;
                    dFees = 0;
                    dTotal = 0;
                    result2.Query = "select * from or_table where or_no = '"+ sOrNo +"' and qtr_paid = '"+ sQtr +"'";
                    if(result2.Execute())
                    {
                        while(result2.Read())
                        {
                            sFeesCode = result2.GetString("fees_code");
                            sBnsCode = result2.GetString("bns_code_main");
                            if(sFeesCode == "B")
                            {
                                dTax += result2.GetDouble("fees_due");
                                dSurPen += result2.GetDouble("fees_surch") + result2.GetDouble("fees_pen");
                                dTotal += result2.GetDouble("fees_amtdue");
                            }
                            else
                            {
                                dFees += result2.GetDouble("fees_due");
                                dSurPen += result2.GetDouble("fees_surch") + result2.GetDouble("fees_pen");
                                dTotal += result2.GetDouble("fees_amtdue");
                            }
                        }
                    }
                    result2.Close();

                    sTax = dTax.ToString("#,##0.00");
                    sFees = dFees.ToString("#,##0.00");
                    sSurPen = dSurPen.ToString("#,##0.00");
                    sTotal = dTotal.ToString("#,##0.00");
                    //JARS 20171024 ON SITE (E)

                    //JARS 20171024 ON SITE: REM FOR FIX IN PAY_HIST PRINT
                    //sTax = string.Format("{0:#,##0.00}", result.GetDouble("tax"));
                    //sFees = string.Format("{0:#,##0.00}", result.GetDouble("fees"));
                    //sSurPen = string.Format("{0:#,##0.00}", result.GetDouble("surch_pen"));
                    //sTotal = string.Format("{0:#,##0.00}", result.GetDouble("total"));

                    sNoOfQtr = GetNoOfQtr(sOrNo);   // RMC 20130109 display no. of qtr in payment hist

                    sCapital = AppSettingsManager.GetCapitalGross(sBin.Trim(), AppSettingsManager.GetBnsCodeMain(sBin.Trim()), sTxYr.Trim(), "NEW");
                    sGross = AppSettingsManager.GetCapitalGross(sBin.Trim(), AppSettingsManager.GetBnsCodeMain(sBin.Trim()), sTxYr.Trim(), "REN");
                    //JARS 20181022 (S)
                    if (sQtr == "A")
                    {
                        sGross = AppSettingsManager.GetCapitalGross(sBin.Trim(), AppSettingsManager.GetBnsCodeMain(sBin.Trim()), sTxYr.Trim(), "ADJ");
                    }
                    //JARS 20181022 (E)
                    //model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|>1200|>1200|>1200|>1200;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", sDate, sOrNo, sTxYr, sCapital, sGross, sTerm, sQtr, sTax, sFees, sSurPen, sTotal));
                    //model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sDate, sOrNo, sTxYr, sCapital, sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal));  // RMC 20130109 display no. of qtr in payment hist
                    this.axVSPrinter1.Table = string.Format("<958|>1539|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;{0}|{1}|{2}|||{3}|{4}|{5}||||", sDate, sOrNo, sTxYr, "", "", "");
                    this.axVSPrinter1.Table = string.Format("<2497|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", AppSettingsManager.GetBnsDesc(sBnsCode), sBnsCode, sCapital, sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal);
                    lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
                    this.axVSPrinter1.MarginLeft = 1500;
                    this.axVSPrinter1.FontSize = (float)8;
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    //GetBnsCode(sBin, sOrNo, sTxYr, sDate, sCapital, sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal); //JARS 20171024 ON SITE
                    sPrevTaxYear = result.GetString("tax_year");
                }
            }
            result.Close();

            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            this.axVSPrinter1.Table = "<11500; \n ";


            //JAV 20170803(s)
            string strCurrentDate = string.Empty;
            string strCurrentTime = string.Empty;
            string strUser = string.Empty;
            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            strCurrentTime = string.Format("{0:hh:mm:ss tt}", AppSettingsManager.GetCurrentDate());
            strUser = AppSettingsManager.SystemUser.UserCode;
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY1;
            this.axVSPrinter1.MarginLeft = 100;
            this.axVSPrinter1.Table = string.Format("<1500|<300|<2000;DATE PRINTED|:|" + strCurrentDate);
            this.axVSPrinter1.Table = string.Format("<1500|<300|<2000;TIME PRINTED|:|" + strCurrentTime);
            this.axVSPrinter1.Table = string.Format("<1500|<300|<2000;PRINTED BY|:|" + strUser);
            #region comments
            //this.axVSPrinter1.MarginLeft = -6300;
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = string.Format("<1500|<300|<2000;DATE PRINTED|:|");
            //this.axVSPrinter1.CurrentY = lngY1 + 200;
            //this.axVSPrinter1.MarginLeft = -6300;
            //this.axVSPrinter1.Table = string.Format("<1500|<300|<2000;TIME PRINTED|:|");
            //this.axVSPrinter1.CurrentY = lngY1 + 400;
            //this.axVSPrinter1.MarginLeft = -6300;
            //this.axVSPrinter1.Table = string.Format("<1500|<300|<2000;PRINTED BY|:|");
            //this.axVSPrinter1.FontBold = false;

            //this.axVSPrinter1.CurrentY = lngY1;
            //this.axVSPrinter1.MarginLeft = -6300;
            //this.axVSPrinter1.Table = string.Format("<1500|<300|<2000;||{0}", strCurrentDate);
            //this.axVSPrinter1.CurrentY = lngY1 + 200;
            //this.axVSPrinter1.MarginLeft = -6300;
            //this.axVSPrinter1.Table = string.Format("<1500|<300|<2000;||{0}", strCurrentTime);
            //this.axVSPrinter1.CurrentY = lngY1 + 400;
            //this.axVSPrinter1.MarginLeft = -6300;
            //this.axVSPrinter1.Table = string.Format("<1500|<300|<2000;||{0}", strUser);
            //JAV 20170803(e)
            #endregion
        }

        public void ReportPayHistDetails(string sBin, string p_sTaxYear, string sOrNo, string sQtrPaid) 
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


            DateTime dDate = AppSettingsManager.GetCurrentDate();
            long lngY1 = 0;
            long lngY2 = 0;
            OracleResultSet result = new OracleResultSet();

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.FontSize = (float)15;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11500;Payment History Details";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<1700|<9800;Available Record as of|" + AppSettingsManager.MonthsInWords(dDate);
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.FontSize = (float)15;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11500;" + sBin;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<11500;Business Identification Number";
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.FontSize = (float)15;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11500;" + sOrNo.Trim();
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)8;
            //this.axVSPrinter1.Table = "<11500;Business Identification Number";
            this.axVSPrinter1.Table = "<11500;Official Receipt";    // RMC 20171120 changed label of OR in Payment HIstory report
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";

            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.Table = "<11500; \n \n \n \n \n \n \n";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY1 + 200;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<1500|<500|<9000;Business Name\nAddress\nOwner's Name\nAddress\nBusiness Plate\nPermit No.|:\n:\n:\n:\n:\n:\n:|";   // RMC 20171122 added printing of current permit no. in Payment hist printing
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY1 + 200;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<1500|<500|<9000;|| {0}\n{1}\n {2}\n{3}\n {4}\n {5}",
                AppSettingsManager.GetBnsName(sBin.Trim()), 
                AppSettingsManager.GetBnsAdd(sBin.Trim(), ""), 
                AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin.Trim())),
                AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetOwnCode(sBin.Trim())), 
                AppSettingsManager.GetBnsPlate(sBin.Trim()),
                AppSettingsManager.GetPermitNo(sBin.Trim()));  // RMC 20171122 added printing of current permit no. in Payment hist printing
            

            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";


            //lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            //this.axVSPrinter1.MarginLeft = 1500;
            //this.axVSPrinter1.FontSize = (float)8;
            //this.axVSPrinter1.CurrentY = lngY1 + 300;

            //this.axVSPrinter1.Table = "<11496; \n \n ";


            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY1 + 300;
            this.axVSPrinter1.Table = "<11496; \n \n ";
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY1 + 300;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            // this.axVSPrinter1.Table = "<958|<958|<958|<958|<958|<958|<958|<958|<958|<958|<958|<958;Date|OR#|Tax Year|Capital|Gross|Term|QTR|No. of QTR|Tax|Fees|Sur/Pen|Total";
            //this.axVSPrinter1.Table = "^958|^1539|^800|^1200|^1200|^600|^400|^800|^1000|^1000|^1000|^1000;Date|OR#|Tax Year|Capital|Gross|Term|QTR|No. of QTR|Tax|Fees|Sur/Pen|Total";
            
            //model.SetTable("^500|^700|^500|^500|^500|^500|^700|^2200|^1300|^1000|^1000|^1300;TY|OR DATE|STAT|TERM|QTR|NO. OF|CODE|DESCRIPTION|FEES|SURCH|PEN|TOTAL");
            //model.SetTable("^500|^700|^500|^500|^500|^500|^700|^2200|^1300|^1000|^1000|^1300;|||||QTR|||AMOUNT|||AMOUNT");
            this.axVSPrinter1.Table = "^500|^900|^600|^600|^600|^700|^800|^2200|^1300|^1000|^1000|^1296;TY|OR DATE|STAT|TERM|QTR|NO. OF|CODE|DESCRIPTION|FEES|SURCH|PEN|TOTAL";
            this.axVSPrinter1.Table = "^500|^900|^600|^600|^600|^700|^800|^2200|^1300|^1000|^1000|^1296;|||||QTR|||AMOUNT|||AMOUNT";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Paragraph = " ";

            string sNoOfQtr = string.Empty; // RMC 20130109 display no. of qtr in payment hist

            //result.Query = "select distinct * from pay_hist where or_no = :1 and bin = :2";
            //result.Query = "select distinct * from pay_hist where or_no = :1 and bin = :2 order by tax_year";    // RMC 20140114 added sorting by tax year in payment hist
            //distincts

            if (!sOrNo.Contains("-")) //MCR 20210708
                result.Query = "select distinct * from pay_hist where or_no = :1 and bin = :2 and qtr_paid = :3 order by tax_year";    // MCR 2014 paymenthist by qtr only
            else
                result.Query = "select distinct * from pay_hist where bill_no = :1 and bin = :2 and qtr_paid = :3 order by tax_year";   // MCR 2014 paymenthist by qtr only
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
                    if (!withOtherOR) //JARS 20170728 ADJUSTED THE USE OF THIS VARIABLE
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
                            if (!sOrNo.Contains("-")) //MCR 20210708
                                pSet.Query = "select * from or_table where or_no = :1 and fees_code = 'B' and tax_year = :2 and qtr_paid = :3";    // MCR 20140321 paymenthist by qtr only
                            else
                                pSet.Query = "select * from or_table where bill_no = :1 and fees_code = 'B' and tax_year = :2 and qtr_paid = :3";    // MCR 20140321 paymenthist by qtr only
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
                                    sTaxYear = pSet.GetString("Tax_Year");
                                }

                                //model.SetTable(string.Format("^500|^700|^500|^500|^500|^500|^700|<2200|>1300|>1000|>1000|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, AppSettingsManager.GetBnsDesc(sFeesCode), sFees, sTotSurB, sTotPenB, sTotal));
                                //this.axVSPrinter1.Table = string.Format("^500|^700|^500|^500|^500|^500|^700|<2200|>1300|>1000|>1000|>1300;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, AppSettingsManager.GetBnsDesc(sFeesCode), sFees, sTotSurB, sTotPenB, sTotal);
                                this.axVSPrinter1.Table = string.Format("^500|^900|^600|^600|^600|^700|^800|<2200|>1300|>1000|>1000|>1296;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, AppSettingsManager.GetBnsDesc(sFeesCode), sFees, sTotSurB, sTotPenB, sTotal);
                                                                         
                                // RMC 20131226 modified printing of business tax paid per bns type (e)

                                // RMC 20140122 view capital, gross, pre-gross on payment hist (s)
                                if (sQtr == "A")
                                {
                                    GetCapGross(sBin.Trim(), sTaxYear.Trim(), sFeesCode, "ADJ");
                                }
                                else
                                {
                                    m_sStatus = sBnsStat;
                                    GetCapGross(sBin.Trim(), sTaxYear.Trim(), sFeesCode, sQtr);
                                }
                                if (sBnsStat.Trim() == "NEW")
                                {
                                    //model.SetTable(string.Format("^500|^800|^500|^500|^500|^500|^800|<3000|>1300|>1000|>1300;|||||||{0}|||", "Capital: " + m_sCap));
                                    this.axVSPrinter1.Table = string.Format("^500|^900|^600|^600|^600|^700|^800|<2200|>1300|>1000|>1000|>1296;|||||||{0}|||", "Capital: " + m_sCap);
                                }
                                else
                                {
                                    //model.SetTable(string.Format("^500|^800|^500|^500|^500|^500|^800|<3000|>1300|>1000|>1300;|||||||{0}|||", "Gross: " + m_sGross + " P.Gross: " + m_sPreGross));
                                    this.axVSPrinter1.Table = string.Format("^500|^900|^600|^600|^600|^700|^800|<2200|>1300|>1000|>1000|>1296;|||||||{0}|||", "Gross: " + m_sGross + " P.Gross: " + m_sPreGross);
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
            result.Close();


            //result.Query = "select a.fees_desc as fees_desc, b.fees_code as fees_code, sum(b.fees_due) as sum_fees_due, sum(b.fees_surch+b.fees_pen) as sum_surch_pen, sum(b.fees_amtdue) as sum_amtdue from tax_and_fees_table a, or_table b where or_no = :1 and a.fees_code = b.fees_code and rev_year = :2 group by a.fees_desc, b.fees_code order by b.fees_code";
            // RMC 20150501 QA BTAS (s)
            double dTotPen = 0;
            //result.Query = "select a.fees_desc as fees_desc, b.fees_code as fees_code, sum(b.fees_due) as sum_fees_due, sum(b.fees_surch) as sum_surch_pen, sum(b.fees_pen) as sum_pen, sum(b.fees_amtdue) as sum_amtdue from tax_and_fees_table a, or_table b where or_no = :1 and a.fees_code = b.fees_code and rev_year = :2 group by a.fees_desc, b.fees_code order by b.fees_code";
            // RMC 20150501 QA BTAS (e)
            if (!sOrNo.Contains("-")) //MCR 20210708
                result.Query = "select a.fees_desc as fees_desc, b.fees_code as fees_code, sum(b.fees_due) as sum_fees_due, sum(b.fees_surch) as sum_surch_pen, sum(b.fees_pen) as sum_pen, sum(b.fees_amtdue) as sum_amtdue,b.tax_year  from tax_and_fees_table a, or_table b where or_no = :1 and a.fees_code = b.fees_code and rev_year = :2 group by a.fees_desc, b.fees_code, b.tax_year order by b.tax_year, b.fees_code";
            else
                result.Query = "select a.fees_desc as fees_desc, b.fees_code as fees_code, sum(b.fees_due) as sum_fees_due, sum(b.fees_surch) as sum_surch_pen, sum(b.fees_pen) as sum_pen, sum(b.fees_amtdue) as sum_amtdue,b.tax_year  from tax_and_fees_table a, or_table b where bill_no = :1 and a.fees_code = b.fees_code and rev_year = :2 group by a.fees_desc, b.fees_code, b.tax_year order by b.tax_year, b.fees_code";
            String sTmpTaxYear = ""; //MCR 20150901
            result.AddParameter(":1", sOrNo.Trim());
            result.AddParameter(":2", AppSettingsManager.GetConfigValue("07"));
            if (result.Execute())
            {
                while (result.Read())
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
                    this.axVSPrinter1.Table = string.Format("^500|^900|^600|^600|^600|^700|^800|<2200|>1300|>1000|>1000|>1296;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", sTmpTaxYear.Trim(), sOrDate, sBnsStat.Trim(), sTerm.Trim(), sQtr.Trim(), sNoOfQtr.Trim(), sFeesCode, sFeesDesc, sFees, sSurPen, sPen, sTotal);    // MCR 20150901
                }
            }
            result.Close();

            sTotal = string.Format("{0:#,##0.00}", dTotTotAmtB);
            this.axVSPrinter1.Paragraph = " ";
            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY1;
            this.axVSPrinter1.Table = "<11496; \n \n ";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.CurrentY = lngY1;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = " ";
            this.axVSPrinter1.Table = ">11496;TOTAL: " + sTotal + " ";
            this.axVSPrinter1.FontBold = false;

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

            #region start
            //if (sMode == "POS")
            //{
            //    if (m_objSystemUser.Load(sUser))
            //    {
            //        sUser = m_objSystemUser.UserName;
            //    }
            //    //model.SetTable("<1500|<9800;DATA MODE|: POSTING");
            //    model.SetTable("<1800|^500|<9300;DATA MODE|:|POSTING");
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //    //model.SetTable("<1500|<9800;PAYMENT TYPE|: " + sPayType);
            //    model.SetTable("<1800|^500|<9300;PAYMENT TYPE|:|" + sPayType);
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //    //model.SetTable("<1500|<9800;ENCODED BY|: " + sUser + "   ");
            //    model.SetTable("<1800|^500|<9300;ENCODED BY|:|" + sUser + "   ");
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //    //model.SetTable("<1500|<9800;DATE ENCODED|: " + sEncodeDate + "   ");
            //    model.SetTable("<1800|^500|<9300;DATE ENCODED|:|" + sEncodeDate + "   ");
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //}
            //if (sMode == "ONL")
            //{
            //    if (m_objSystemUser.Load(sTeller))
            //    {
            //        sTeller = m_objSystemUser.UserName;
            //    }
            //    model.SetTable("<1800|<9800;DATA MODE|: ONLINE");
            //    model.SetTable("<1800|^500|<9300;DATA MODE|:|ONLINE");
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //    //model.SetTable("<1800|<9800;PAYMENT TYPE|:" + sPayType);
            //    model.SetTable("<1800|^500|<9300;PAYMENT TYPE|:|" + sPayType);
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //    //model.SetTable("<1800|<9800;TELLER|: " + sTeller + "   ");
            //    model.SetTable("<1800|^500|<9300;TELLER|:|" + sTeller + "   ");
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //    //model.SetTable("<1800|<9800;TRANSACTION DATE|: " + sEncodeDate + "   ");
            //    model.SetTable("<1800|^500|<9300;TRANSACTION DATE|:|" + sEncodeDate + "   ");
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;


            //}
            //if (sMode == "OFL")
            //{
            //    if (m_objSystemUser.Load(sTeller))
            //    {
            //        sTeller = m_objSystemUser.UserName;
            //    }
            //    model.SetTable("<1800|<9800;DATA MODE|: OFFLINE");
            //    model.SetTable("<1800|^500|<9300;DATA MODE|:|OFFLINE");
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //    //model.SetTable("<1800|<9800;PAYMENT TYPE|:" + sPayType);
            //    model.SetTable("<1800|^500|<9300;PAYMENT TYPE|:|" + sPayType);
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //    //model.SetTable("<1800|<9800;TELLER|: " + sTeller + "   ");
            //    model.SetTable("<1800|^500|<9300;TELLER|:|" + sTeller + "   ");
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //    //model.SetTable("<1800|<9800;TRANSACTION DATE|: " + sEncodeDate + "   "); //JAV 20170804 fix the alignment
            //    model.SetTable("<1800|^500|<9300;TRANSACTION DATE|:|" + sEncodeDate + "   "); //JAV 20170804 fix the alignment
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //    model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //}
            #endregion

            if (sMode == "POS")
            {
                if (m_objSystemUser.Load(sUser))
                {
                    sUser = m_objSystemUser.UserName;
                }
                lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.MarginLeft = 1500;
                this.axVSPrinter1.FontSize = (float)8;
                this.axVSPrinter1.CurrentY = lngY1;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;DATA MODE|:|");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;PAYMENT TYPE|:|");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;ENCODED BY|:|");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;DATE ENCODED|:|");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.MarginLeft = 1500;
                this.axVSPrinter1.FontSize = (float)8;
                this.axVSPrinter1.CurrentY = lngY1;
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|POSTING");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}", sPayType);
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}", sUser);
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}", sEncodeDate);

            }
            if (sMode == "ONL")
            {
                if (m_objSystemUser.Load(sTeller))
                {
                    sTeller = m_objSystemUser.UserName;
                }
                lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.MarginLeft = 1500;
                this.axVSPrinter1.FontSize = (float)8;
                this.axVSPrinter1.CurrentY = lngY1;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;DATA MODE|:|");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;PAYMENT TYPE|:|");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;TELLER|:|");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;TRANSACTION DATE|:|");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.MarginLeft = 1500;
                this.axVSPrinter1.FontSize = (float)8;
                this.axVSPrinter1.CurrentY = lngY1;
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|ONLINE");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}", sPayType);
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}", sTeller); //JARS 20171004 NOT USER
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}", sEncodeDate);
            }
            if (sMode == "OFL")
            {
                if (m_objSystemUser.Load(sTeller))
                {
                    sTeller = m_objSystemUser.UserName;
                }
                lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.MarginLeft = 1500;
                this.axVSPrinter1.FontSize = (float)8;
                this.axVSPrinter1.CurrentY = lngY1;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;DATA MODE|:|");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;PAYMENT TYPE|:|");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;TELLER|:|");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;TRANSACTION DATE|:|");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.MarginLeft = 1500;
                this.axVSPrinter1.FontSize = (float)8;
                this.axVSPrinter1.CurrentY = lngY1;
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|OFFLINE");
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}", sPayType);
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}", sTeller); //JARS 20171004 NOT USER
                this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}", sEncodeDate);
            }

            string strCurrentDate = string.Empty;
            string strCurrentTime = string.Empty;
            string strUser = string.Empty;
            strCurrentDate = string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetCurrentDate());
            strCurrentTime = string.Format("{0:hh:mm:ss tt}", AppSettingsManager.GetCurrentDate());
            strUser = AppSettingsManager.SystemUser.UserCode;

            lngY1 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY1;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;DATE PRINTED|:|");
            this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;TIME PRINTED|:|");
            this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;PRINTED BY|:|");
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY1;
            this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}",strCurrentDate);
            this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}",strCurrentTime);
            this.axVSPrinter1.Table = string.Format("<1600|^500|<9396;|:|{0}",strUser);



            //model.SetTable("<1800|^500|<9300;DATE PRINTED|:|" + strCurrentDate);
            //model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //model.SetTable("<1800|^500|<9300;TIME PRINTED|:|" + strCurrentTime);
            //model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            //model.SetTable("<1800|^500|<9300;PRINTED BY|:|" + strUser);
            //model.Tables[model.Tables.Count - 1].Items[0].Font.FontName = "Arial";
            //model.Tables[model.Tables.Count - 1].Items[0].Font.FontSize = 8;
            //model.Tables[model.Tables.Count - 1].Items[0].Font.FontStyle = 1;
            
        }

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

                    GetCapGross(sBin.Trim(), sTxYr, sFeesCode, ""); //JARS 20181022 ADDED PARAMETER


                    if (sTxYr != xy)
                    {
                        this.axVSPrinter1.Table = string.Format("<958|>1539|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;{0}|{1}|{2}|||{3}|{4}|{5}||||", sDate, sOrNo, sTxYr, "", "", "");
                        if (sFeesCode != yx)
                        {
                            //model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", AppSettingsManager.GetBnsDesc(sFeesCode), sFeesCode, m_sCap, m_sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal));
                            //this.axVSPrinter1.Table = string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", AppSettingsManager.GetBnsDesc(sFeesCode), sFeesCode, m_sCap, m_sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal);
                            this.axVSPrinter1.Table = string.Format("<2497|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", AppSettingsManager.GetBnsDesc(sFeesCode), sFeesCode, m_sCap, m_sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal);
                        
                        }
                        xy = sTxYr;
                        //yx = sOrNo;
                    }
                    else
                    {
                        //model.SetTable(string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", AppSettingsManager.GetBnsDesc(sFeesCode), sFeesCode, m_sCap, m_sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal));
                        //this.axVSPrinter1.Table = string.Format("<800|>1000|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", sBnsDesc, sFeesCode, m_sCap, m_sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal);
                        this.axVSPrinter1.Table = string.Format("<2497|^800|>1200|>1200|^600|^400|^800|>1000|>1000|>1000|>1000;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10};", AppSettingsManager.GetBnsDesc(sFeesCode), sFeesCode, m_sCap, m_sGross, sTerm, sQtr, sNoOfQtr, sTax, sFees, sSurPen, sTotal);
                    }
                    // model.SetTable(string.Format(""));      
                }
            }
        }

        private void GetCapGross(string sBin, string sTaxYear, string sBnsCode, string sStatus)
        {
            //string m_sCap = string.Empty;   // RMC 20140122 view capital, gross, pre-gross on payment hist (s)
            //string m_sPreGross = string.Empty;  // RMC 20140122 view capital, gross, pre-gross on payment hist (e)
            //string m_sGross = string.Empty;

            // RMC 20140122 view capital, gross, pre-gross on payment hist
            m_sCap = "";
            m_sGross = "";
            m_sPreGross = "";
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            //OracleResultSet result2 = new OracleResultSet();

            //JARS 20170717 (S) COMMENT OUT, SINCE THAT THERE IS NO DATA YET IN BILL GROSS INFO
            result.Query = "select * from bill_gross_info where bin = '" + sBin + "' ";
            result.Query += " and tax_year = '" + sTaxYear + "' and bns_code = '" + sBnsCode + "'";
            if (m_sStatus == "RET")  //AFM 20200529 added condition to get specific gross for retirement (prevents getting gross from current year renewal for businesses that was retired for same year)
                result.Query += " and due_state = '" + sStatus + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sCap = string.Format("{0:#,###.##}", result.GetInt("capital"));
                    if (sStatus == "ADJ") //JARS 20181022 IF THE TRANSACTION IS ADJUSTMENT, GET THE ADJ_GROSS INSTEAD OF GROSS, AND VICE VERSA
                    {
                        m_sGross = string.Format("{0:#,###.##}", result.GetInt("adj_gross"));
                    }
                    else
                    {
                        m_sGross = string.Format("{0:#,###.##}", result.GetInt("gross"));
                    }
                    m_sPreGross = string.Format("{0:#,###.##}", result.GetInt("pre_gross"));

                }
                else
                {
                    if (m_sStatus == "RET") //AFM 20200529 get retirement gross here if not found in bill_gross_info table(s)
                    {
                        result2.Query = "select * from retired_bns where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "'";
                        if (result2.Execute())
                            if (result2.Read())
                            {
                                m_sCap = string.Format("{0:#,###.00}", result.GetInt("capital")); //get capital on first query since there's no capital in retired bns table
                                m_sGross = string.Format("{0:#,###.00}", result2.GetInt("gross"));
                            }
                    }
                    //AFM 20200529 get retirement gross here if not found in bill_gross_info table(e)
                    else
                        result.Close();
                        result.Query = "select * from buss_hist where bin = '" + sBin + "' and bns_code = '" + sBnsCode + "' and tax_year = '" + sTaxYear + "'"; //JARS 20170809 TO GET GROSS AND CAPITAL OF SPECIFIC TAX YEAR
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                m_sCap = string.Format("{0:#,###.00}", result.GetInt("capital"));
                                m_sGross = string.Format("{0:#,###.00}", result.GetInt("gr_1"));
                                //m_sPreGross = "";
                            }
                        }
                        result.Close();
                }
            }
            result.Close();
            #region comments
            /*
            //result.Query = "select * from businesses where bin = '"+ sBin +"' and bns_code = '"+ sBnsCode +"'"; //ALTERNATE DATA SOURCE
            result.Query = "select * from buss_hist where bin = '" + sBin + "' and bns_code = '" + sBnsCode + "' and tax_year = '" + sTaxYear + "'"; //JARS 20170809 TO GET GROSS AND CAPITAL OF SPECIFIC TAX YEAR
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sCap = string.Format("{0:#,###.00}", result.GetInt("capital"));
                    m_sGross = string.Format("{0:#,###.00}", result.GetInt("gr_1"));
                    //m_sPreGross = "";
                }
                else
                {
                    result.Close();
                    result.Query = "select * from addl_bns where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and bns_code_main  = '" + sBnsCode + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            m_sCap = string.Format("{0:#,###.00}", result.GetInt("capital"));
                            m_sGross = string.Format("{0:#,###.00}", result.GetInt("gross"));
                        }
                        else //JARS 20170815
                        {
                            result.Close();
                            result.Query = "select * from declared_gross where bin = :1 and bns_code = :2 and tax_year = :3";
                            result.AddParameter(":1", sBin.Trim());
                            result.AddParameter(":2", sBnsCode.Trim());
                            result.AddParameter(":3", sTaxYear.Trim());
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    m_sGross = string.Format("{0:##0.00}", result.GetDouble("declared_gr"));
                                }
                            }
                        }
                    }
                }
            }
            result.Close();
            //JARS 20170717 (E)
            */
            #endregion

        }

        private string GetBnsCounts(DateTime dtOrDate , string sBrgy)
        {
            // RMC 20180201 display business count in Abstract - request from Jester (Malolos)
            OracleResultSet pSet = new OracleResultSet();
            int iCnt = 0;
            string sORDate = string.Format("{0:MM/dd/yyyy}",dtOrDate);

            if(m_iReportSwitch == 3 && m_sReportSwitch == "AbstractCollectByDaily")
                pSet.Query = "select count(distinct bin) from pay_hist where or_date = to_date('" + sORDate + "','MM/dd/yyyy') and or_no in (select or_no from or_table where fees_code = '50') and bin in (select bin from businesses where pay_hist.bin = businesses. bin and bns_brgy = '" + sBrgy.Trim() + "')";
            else
                pSet.Query = "select count(distinct bin) from pay_hist where or_date = to_date('" + sORDate + "','MM/dd/yyyy')";
            if (pSet.Execute())
            {
                if (pSet.Read())
                    iCnt = pSet.GetInt(0);
            }
            pSet.Close();

            return iCnt.ToString();

        }

        private void axVSPrinter1_StartDocEvent(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}