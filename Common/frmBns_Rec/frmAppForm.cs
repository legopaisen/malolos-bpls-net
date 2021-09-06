// RMC 20120105 Modified getting of area in printing App form
// RMC 20120102 Added trailing of printing application form
// RMC 20111228 added display of prev gross/cap in app form

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using System.Runtime.InteropServices;
using Amellar.Common.StringUtilities;
using System.Globalization;
using System.Collections;
using Amellar.Common.BPLSApp;
using Amellar.Common.Reports;



namespace Amellar.Common.frmBns_Rec
{
    public partial class frmAppForm : Form
    {
        //[DllImport("Winspool.drv")]
       // private static extern bool SetDefaultPrinter(string printerName);//GMC 20150810 Set printer default

        public bool m_bAdvancePrintForm = false;    // RMC 20161228 adjust tax year in form for advance printing
        public bool bIsRePrint = false;
        private bool m_bPrintSupp = false; //JARS 20171010
        private string m_sApplType = string.Empty;
        private string m_sOfcType = string.Empty;
        private string m_sMotherBIN = string.Empty;
        private string m_sBIN = string.Empty;
        private string m_sOwnLn = string.Empty;
        private string m_sOwnFn = string.Empty;
        private string m_sOwnMi = string.Empty;
        private string m_sOwnHse = string.Empty;
        private string m_sOwnStr = string.Empty;
        private string m_sOwnBrgy = string.Empty;
        private string m_sOwnMun = string.Empty;
        private string m_sOwnProv = string.Empty;
        private string m_sBldgName = string.Empty;
        private string m_sLandPIN = string.Empty;
        private string m_sOwnNationality = string.Empty;
        private string m_sOwnTelNo = string.Empty;
        private string m_sOwnEmailAdd = string.Empty;
        private string m_sSpouse = string.Empty;
        private string m_sBdate = string.Empty;
        private string m_sGender = string.Empty;
        private string m_sOwnZip = string.Empty;    // RMC 20171116 added info to display in application form

        private string m_sArea = string.Empty;
        private string m_sEmployee = string.Empty;
        private string m_sTelNo = string.Empty;
        private string m_sEmail = string.Empty;
        private string m_sBnsHse = string.Empty;
        private string m_sBnsStr = string.Empty;
        private string m_sBnsBrgy = string.Empty;
        private string m_sBnsMun = string.Empty;
        private string m_sBnsProv = string.Empty;
        private string m_sNoMale = string.Empty;
        private string m_sNoFemale = string.Empty;
        private string m_sNoGender = string.Empty;
        private string m_sBnsCode = string.Empty;
        private string sLessor = string.Empty;
        private string sRent = string.Empty;
        private string sGross = "";
        private string sTaxYear = "";
        private string sPrevGrossCap = "";
        private string m_sOldOwnCode = string.Empty;
        private string m_sNewOwnCode = string.Empty;
        private string m_sOldLocation = string.Empty;
        private string m_sNewLocation = string.Empty;
        private string m_sBussPlate = string.Empty; // RMC 20150623 shoot app form in pre-printed form
        private string m_sDateOperated = string.Empty;  // RMC 20161129 customized application form for Binan
        private string m_sBnsAddress = string.Empty;    // RMC 20161129 customized application form for Binan
        private bool m_bBlank = false;  // RMC 20161219 add printing of blank application form
        public int iYAxis;
        public double dGrossCapital;
        private string m_sMemo = string.Empty;
        string sOwn_Code = string.Empty;
        string m_sModuleCode = string.Empty;    // RMC 20171127 transferred printing of business record view to vsprinter

        public string ApplType
        {
            get { return m_sApplType; }
            set { m_sApplType = value; }
        }

        public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; }
        }

        public bool PrintBlank
        {
            get { return m_bBlank; }
            set { m_bBlank = value; }
        }
        
        //MCR 20150107 (s)
        private string m_sAppNo = string.Empty;
        private string m_sAppDate = string.Empty;
        public string BnsCode
        {
            set { m_sBnsCode = value; }
        }
        public string AppNo
        {
            set { m_sAppNo = value; }
        }
        public string AppDate
        {
            set { m_sAppDate = value; }
        }
        //MCR 20150107 (e)

        public string Memo
        { 
            set { m_sMemo = value; }
        }
        public string ModuleCode    // RMC 20171127 transferred printing of business record view to vsprinter
        {
            set { m_sModuleCode = value; }
        }
             

        public frmAppForm()
        {
            InitializeComponent();
        }

        private void frmAppForm_Load(object sender, EventArgs e)
        {
            /*this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;*/
            
            if (m_sApplType == "Business Mapping Deficiency")
            {
                this.Text = "Business Mapping Deficiency";

                //this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                //this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 300;
                this.axVSPrinter1.MarginTop = 360;
                this.axVSPrinter1.MarginBottom = 300;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                PrintDeficiency();
            }
            else if (m_sApplType == "Retirement") //MCR 20150107
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                this.axVSPrinter1.MarginLeft = 300;
                this.axVSPrinter1.MarginTop = 360;
                this.axVSPrinter1.MarginBottom = 300;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                //JHMN 20170110 retirement or closure application (s)
                if (AppSettingsManager.GetConfigValue("10") == "243")
                    PrintRetirementBinan();
                else//JHMN 20170110 retirement or closure application (e)
                    PrintRetirement();
            }
            else if(m_sApplType == "CERT-STAT") //JARS 20170320
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.MarginTop = 360;
                this.axVSPrinter1.MarginBottom = 300;

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                CertStatus();

            }
            else if (m_sApplType == "RECORD-VIEW")  // RMC 20171127 transferred printing of business record view to vsprinter
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.MarginTop = 500;
                this.axVSPrinter1.MarginBottom = 300;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                BusinessRecordView();
            }
                //JHB 20180716 VIEW PERMITS (s)
            else if (m_sApplType == "PERMIT-VIEW")  
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.MarginTop = 500;
                this.axVSPrinter1.MarginBottom = 300;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                PermitView();
            }
            //JHB 20180716 VIEW PERMITS (e)
            else
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                this.axVSPrinter1.MarginLeft = 300;
                if (AppSettingsManager.GetConfigValue("10") == "243")
                    this.axVSPrinter1.MarginTop = 500;
                else
                    this.axVSPrinter1.MarginTop = 360;
                this.axVSPrinter1.MarginBottom = 300;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                #region comments
                //// RMC 20141211 Added printing of application form not pre-printed (s)
                ////if (MessageBox.Show("Use pre-printed form?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                ////   PrintPrintedAppForm();
                ////else// RMC 20141211 Added printing of application form not pre-printed (e)
                //if (m_bBlank)  // RMC 20161219 add printing of blank application form (s)
                //    //PrintAppForm_Binan_Blank();
                //    PrintAppForm_MalolosV2_BLANK();
                //else // RMC 20161219 add printing of blank application form (e)
                //    PrintAppForm();
                //    //PrintAppForm_Malolos();
                #endregion

                //JARS 20171010 (S)
                if (m_bBlank)
                {
                    if (MessageBox.Show("Do you want to print an additional page for lines of business?", "Blank Application Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        m_bPrintSupp = true;
                    }
                    PrintAppForm_MalolosV2_BLANK();
                }
                else
                {
                    PrintAppForm();
                }
                //JARS 20171010 (E)


            }

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void PrintRetirement() //MCR 20150107
        {
            this.axVSPrinter1.CurrentY = 2600;
            this.axVSPrinter1.MarginLeft = 1400;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)12.0;

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from retired_bns where bin = '" + m_sBIN + "' and app_no = '" + m_sAppNo + "' and bns_code_main = '" + m_sBnsCode + "'";
            if (pSet.Execute())
                while (pSet.Read())
                {
                    this.axVSPrinter1.Table = "<2000;" + pSet.GetDateTime("APPRVD_DATE").ToString("MM/dd/yyyy");
                    this.axVSPrinter1.CurrentY = 5100;
                    this.axVSPrinter1.MarginLeft = 2100;
                    this.axVSPrinter1.Table = "<2000;" + AppSettingsManager.GetBnsName(m_sBIN);
                    this.axVSPrinter1.CurrentY = 5100;
                    this.axVSPrinter1.MarginLeft = 900;
                    this.axVSPrinter1.Table = @"=7800;                                                                                          " + AppSettingsManager.GetBnsAdd(m_sBIN, "1");
                    this.axVSPrinter1.CurrentY = 5450;
                    this.axVSPrinter1.MarginLeft = 7500;
                    this.axVSPrinter1.Table = "<2000;" + pSet.GetDouble("Gross").ToString("#,##0.00");
                    this.axVSPrinter1.MarginLeft = 1800;
                    this.axVSPrinter1.CurrentY = 5850;
                    this.axVSPrinter1.Table = "<1750|<1000;" + pSet.GetDateTime("APPRVD_DATE").ToString("MMMM dd") + "|" + pSet.GetDateTime("APPRVD_DATE").ToString("yy");

                    this.axVSPrinter1.CurrentY = 6470;
                    this.axVSPrinter1.MarginLeft = 2000;
                    this.axVSPrinter1.Table = "<3000;" + pSet.GetString("memoranda");

                    this.axVSPrinter1.CurrentY = 8800;
                    this.axVSPrinter1.MarginLeft = 800;//1800
                    this.axVSPrinter1.Table = "^4700;" + AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBIN));
                }
            pSet.Close();

            //Last Payment
            pSet.Query = "select p.or_no,p.or_date,p.date_posted,(select sum(fees_amtdue) as fees_amtdue from or_table where or_no = p.or_no) as fees_amtdue from pay_hist p where bin = '" + m_sBIN + "' order by or_date desc";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    this.axVSPrinter1.CurrentY = 13250;
                    this.axVSPrinter1.MarginLeft = 5100;
                    this.axVSPrinter1.Table = "<3000; " + pSet.GetDateTime("date_posted").ToString("MM/dd/yyyy");
                    this.axVSPrinter1.CurrentY = 13500;
                    this.axVSPrinter1.Table = "<3000; " + pSet.GetDouble("fees_amtdue").ToString("#,##0.00");
                    this.axVSPrinter1.CurrentY = 13760;
                    this.axVSPrinter1.Table = "<3000; " + pSet.GetString("or_no");
                    this.axVSPrinter1.CurrentY = 14020;
                    this.axVSPrinter1.Table = "<3000; " + pSet.GetDateTime("or_date").ToString("MM/dd/yyyy");
                }
            pSet.Close();

            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.CurrentY = 16850;
            this.axVSPrinter1.MarginLeft = 1200;
            this.axVSPrinter1.Table = "^5500; " + AppSettingsManager.SystemUser.UserName;
        }

        private void PrintAppForm()
        {
            //SetDefaultPrinter("HP LaserJet Professional P1102");//GMC 20150810 Set printer default to HP

            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            string strData = string.Empty;

            string sOwnCode = "";
            string sBnsName = "";
            string sAppDate = "";
            string sDTINo = "";
            string sDTIDate = "";
            string sOrgKind = "";
            string sCTCNo = "";
            //MCR 20150106 (s)
            string sCTCDate = ""; 
            string sTINDate = "";
            string sSSS = "";
            string sSSSDate = "";
            string sPhilhealth = "";
            string sPhilhealthDate = "";
            //MCR 20150106 (e)
            string sTIN = "";
            string sLandPin = "";
            string sPEZANo = "";
            string sPEZADate = "";
            string sBussPlate = "";
            string sMemo = string.Empty;
            //m_sBIN = "192-00-2011-0000031";

            double dReprintGross = 0;

            // GDE 20130110 add message box for reassessment (s){
            pRec.Query = "select * from REASS_WATCH_LIST where bin = '" + m_sBIN + "' and is_tag = '1' order by tax_year desc";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    MessageBox.Show("Tagged for reassessment TAX YEAR: " + pRec.GetString("tax_year"));
                }
            }
            pRec.Close();
            // GDE 20130110 add message box for reassessment (e)}

            m_sDateOperated = "";   // RMC 20161129 customized application form for Binan
            pRec.Query = "select * from business_que where bin = '" + m_sBIN + "'"; // GDE 20130103
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sOwnCode = pRec.GetString("own_code");
                    sOwn_Code = sOwnCode;
                    GetOwnerDetails(sOwnCode);
                    GetGISLocation(m_sBIN);
                    GetAddlInfo(m_sBIN);//MCR 20140617
                    sBnsName = pRec.GetString("bns_nm");
                    if (pRec.GetDateTime("dt_applied") == DateTime.Now)
                        sAppDate = "";
                    else
                        sAppDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dt_applied"));
                    sDTINo = pRec.GetString("dti_reg_no");
                    if (pRec.GetDateTime("dti_reg_dt") == DateTime.Now)
                        sDTIDate = "";
                    else
                        sDTIDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dti_reg_dt"));
                    sOrgKind = pRec.GetString("orgn_kind");
                    sCTCNo = pRec.GetString("ctc_no");
                    //MCR 20150106 (s)
                    if (pRec.GetDateTime("ctc_issued_on") == DateTime.Now)
                        sCTCDate = "";
                    else
                        sCTCDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("ctc_issued_on"));

                    if (pRec.GetDateTime("tin_issued_on") == DateTime.Now)
                        sTINDate = "";
                    else
                        sTINDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("tin_issued_on"));

                    if (pRec.GetDateTime("sss_issued_on") == DateTime.Now)
                        sSSSDate = "";
                    else
                        sSSSDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("sss_issued_on"));

                    sSSS = pRec.GetString("sss_no");
                    sPhilhealth = ""; 
                    sPhilhealthDate = "";
                    //MCR 20150106 (e)
                    sTIN = pRec.GetString("tin_no");
                    sLandPin = pRec.GetString("land_pin");
                    if (m_sLandPIN != "")
                        sLandPin = m_sLandPIN;
                    m_sArea = string.Format("{0:###.00}", pRec.GetDouble("flr_area"));

                    m_sEmployee = string.Format("{0:##}", pRec.GetInt("num_employees"));
                    m_sTelNo = pRec.GetString("bns_telno");
                    m_sEmail = pRec.GetString("bns_email");
                    m_sBnsHse = pRec.GetString("bns_house_no");
                    m_sBnsStr = pRec.GetString("bns_street");
                    m_sBnsBrgy = pRec.GetString("bns_brgy");
                    m_sBnsMun = pRec.GetString("bns_mun");
                    m_sBnsProv = pRec.GetString("bns_prov");
                    m_sBnsCode = pRec.GetString("bns_code");
                    sMemo = pRec.GetString("memoranda");
                    m_sDateOperated = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dt_operated"));   // RMC 20161129 customized application form for Binan

                    // RMC 20111228 added display of prev gross/cap in app form (s)
                    if (pRec.GetString("bns_stat").Trim() == "NEW")
                    {
                        if (bIsRePrint)
                        {
                            pRec2.Query = "select * from bill_gross_info where bin = :1 order by tax_year desc, qtr desc";
                            pRec2.AddParameter(":1", m_sBIN);
                            if (pRec2.Execute())
                            {
                                if (pRec2.Read())
                                {
                                    try
                                    {
                                        dReprintGross = pRec2.GetDouble("capital");
                                    }
                                    catch
                                    { }
                                }
                            }
                            pRec2.Close();
                            sPrevGrossCap = "(Curr. Capital) " + string.Format("{0:#,###.00}", dReprintGross);
                        }
                        else
                            sPrevGrossCap = "(Prev. Capital) " + string.Format("{0:#,###.00}", pRec.GetDouble("capital"));
                    }
                    else
                    {
                        if (bIsRePrint)
                        {
                            pRec2.Query = "select * from bill_gross_info where bin = :1 order by tax_year desc, qtr desc";
                            pRec2.AddParameter(":1", m_sBIN);
                            if (pRec2.Execute())
                            {
                                if (pRec2.Read())
                                {
                                    try
                                    {
                                        dReprintGross = pRec2.GetDouble("gross");
                                    }
                                    catch
                                    { }
                                }

                            }
                            pRec2.Close();
                            sPrevGrossCap = "(Curr. Gross) " + string.Format("{0:#,###.00}", dReprintGross);
                        }
                        else
                            sPrevGrossCap = "(Prev. Gross) " + string.Format("{0:#,###.00}", pRec.GetDouble("gr_1"));
                    }
                    // RMC 20111228 added display of prev gross/cap in app form (e)

                    sGross = "";    // gross will be filled-up by tax payer

                    sTaxYear = pRec.GetString("tax_year");
                    GetPermitUpdates(m_sBIN); //MCR 20140617

                    // RMC 20120105 Modified getting of area in printing App form (s)
                    if (pRec.GetDouble("flr_area") == 0)
                        m_sArea = GetArea(m_sBIN, m_sBnsCode, sTaxYear);
                    // RMC 20120105 Modified getting of area in printing App form (e)

                    if (pRec.GetString("place_occupancy") == "RENTED")
                    {
                        sLessor = pRec.GetString("busn_own");
                        sRent = string.Format("{0:#,##0.00}", pRec.GetDouble("rent_lease_mo"));
                    }
                    else
                    {
                        sLessor = "";
                        sRent = "";
                    }

                    pRec.Close();

                    pRec.Query = "select * from boi_table where bin = '" + m_sBIN + "'";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sPEZANo = pRec.GetString("reg_no").Trim();
                            sPEZADate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("reg_dt"));
                        }
                    }
                    pRec.Close();

                    pRec.Query = "select * from buss_plate where bin = '" + m_sBIN + "'";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            //sBussPlate = pRec.GetString("bns_plate");
                            m_sBussPlate = pRec.GetString("bns_plate");   // RMC 20150623 shoot app form in pre-printed form
                        }
                    }
                    pRec.Close();


                }
                    // GDE 20130103 added
                else
                {
                    pRec.Close();
                    pRec.Query = "select * from businesses where bin = :1";
                    pRec.AddParameter(":1", m_sBIN);
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sOwnCode = pRec.GetString("own_code");
                            GetOwnerDetails(sOwnCode);
                            GetGISLocation(m_sBIN);
                            GetAddlInfo(m_sBIN);//MCR 20140617
                            sBnsName = pRec.GetString("bns_nm");
                            if (pRec.GetDateTime("dt_applied") == DateTime.Now)
                                sAppDate = "";
                            else
                                sAppDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dt_applied"));
                            sDTINo = pRec.GetString("dti_reg_no");
                            if (pRec.GetDateTime("dti_reg_dt") == DateTime.Now)
                                sDTIDate = "";
                            else
                                sDTIDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dti_reg_dt"));
                            sOrgKind = pRec.GetString("orgn_kind");
                            sCTCNo = pRec.GetString("ctc_no");

                            if (pRec.GetDateTime("ctc_issued_on") == DateTime.Now)
                                sCTCDate = "";
                            else
                                sCTCDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("ctc_issued_on"));

                            if (pRec.GetDateTime("tin_issued_on") == DateTime.Now)
                                sTINDate = "";
                            else
                                sTINDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("tin_issued_on"));

                            if (pRec.GetDateTime("sss_issued_on") == DateTime.Now)
                                sSSSDate = "";
                            else
                                sSSSDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("sss_issued_on"));

                            sSSS = pRec.GetString("sss_no");
                            sPhilhealth = "";
                            sPhilhealthDate = "";
                            sOrgKind = pRec.GetString("orgn_kind");
                            sTIN = pRec.GetString("tin_no");
                            sLandPin = pRec.GetString("land_pin");
                            if (m_sLandPIN != "")
                                sLandPin = m_sLandPIN;
                            m_sArea = string.Format("{0:###.00}", pRec.GetDouble("flr_area"));

                            m_sEmployee = string.Format("{0:##}", pRec.GetInt("num_employees"));
                            m_sTelNo = pRec.GetString("bns_telno");
                            m_sEmail = pRec.GetString("bns_email");
                            m_sBnsHse = pRec.GetString("bns_house_no");
                            m_sBnsStr = pRec.GetString("bns_street");
                            m_sBnsBrgy = pRec.GetString("bns_brgy");
                            m_sBnsMun = pRec.GetString("bns_mun");
                            m_sBnsProv = pRec.GetString("bns_prov");
                            m_sBnsCode = pRec.GetString("bns_code");
                            sMemo = pRec.GetString("memoranda");
                            m_sDateOperated = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dt_operated"));   // RMC 20161129 customized application form for Binan

                            // RMC 20111228 added display of prev gross/cap in app form (s)
                            if (pRec.GetString("bns_stat").Trim() == "NEW")
                            {
                                if (bIsRePrint)
                                {
                                    pRec2.Query = "select * from bill_gross_info where bin = :1 order by tax_year desc, qtr desc";
                                    pRec2.AddParameter(":1", m_sBIN);
                                    if (pRec2.Execute())
                                    {
                                        if (pRec2.Read())
                                        {
                                            try
                                            {
                                                dReprintGross = pRec2.GetDouble("capital");
                                            }
                                            catch
                                            { }
                                        }
                                    }
                                    pRec2.Close();
                                    sPrevGrossCap = "(Curr. Capital) " + string.Format("{0:#,###.00}", dReprintGross);
                                }
                                else
                                    sPrevGrossCap = "(Prev. Capital) " + string.Format("{0:#,###.00}", pRec.GetDouble("capital"));
                            }
                            else
                            {
                                if (bIsRePrint)
                                {
                                    pRec2.Query = "select * from bill_gross_info where bin = :1 order by tax_year desc, qtr desc";
                                    pRec2.AddParameter(":1", m_sBIN);
                                    if (pRec2.Execute())
                                    {
                                        if (pRec2.Read())
                                        {
                                            try
                                            {
                                                dReprintGross = pRec2.GetDouble("gross");
                                            }
                                            catch
                                            { }
                                        }

                                    }
                                    pRec2.Close();
                                    sPrevGrossCap = "(Curr. Gross) " + string.Format("{0:#,###.00}", dReprintGross);
                                }
                                else
                                    sPrevGrossCap = "(Prev. Gross) " + string.Format("{0:#,###.00}", pRec.GetDouble("gr_1"));
                            }
                            // RMC 20111228 added display of prev gross/cap in app form (e)

                            sGross = "";    // gross will be filled-up by tax payer

                            sTaxYear = pRec.GetString("tax_year");
                            GetPermitUpdates(m_sBIN);//MCR 20140617

                            // RMC 20120105 Modified getting of area in printing App form (s)
                            if (pRec.GetDouble("flr_area") == 0)
                                m_sArea = GetArea(m_sBIN, m_sBnsCode, sTaxYear);
                            // RMC 20120105 Modified getting of area in printing App form (e)

                            if (pRec.GetString("place_occupancy") == "RENTED")
                            {
                                sLessor = pRec.GetString("busn_own");
                                sRent = string.Format("{0:#,##0.00}", pRec.GetDouble("rent_lease_mo"));
                            }
                            else
                            {
                                sLessor = "";
                                sRent = "";
                            }

                            pRec.Close();

                            pRec.Query = "select * from boi_table where bin = '" + m_sBIN + "'";
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                {
                                    sPEZANo = pRec.GetString("reg_no").Trim();
                                    sPEZADate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("reg_dt"));
                                }
                            }
                            pRec.Close();

                            pRec.Query = "select * from buss_plate where bin = '" + m_sBIN + "'";
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                {
                                    //sBussPlate = pRec.GetString("bns_plate");
                                    m_sBussPlate = pRec.GetString("bns_plate"); // RMC 20150623 shoot app form in pre-printed form
                                }
                            }
                            pRec.Close();
                        }
                    }
                }
                // GDE 20130103 added

                // RMC 20161129 customized application form for Binan (s)
                if (m_sBnsHse == "." || m_sBnsHse == "")
                    m_sBnsHse = "";
                else
                    m_sBnsHse = m_sBnsHse + " ";
                if (m_sBnsStr == "." || m_sBnsStr == "")
                    m_sBnsStr = "";
                else
                    m_sBnsStr = m_sBnsStr + ", ";
                if (m_sBnsBrgy == "." || m_sBnsBrgy == "")
                    m_sBnsBrgy = "";
                else
                    m_sBnsBrgy = m_sBnsBrgy + ", ";
                if (m_sBnsMun == "." || m_sBnsMun == "")
                    m_sBnsMun = "";

                m_sBnsAddress = m_sBnsHse + m_sBnsStr + m_sBnsBrgy + m_sBnsMun;
                // RMC 20161129 customized application form for Binan (e)

            }

            if (sOwnCode != "")
            {
                // RMC 20140103 modified printing of application form for Mati (s)
                if (AppSettingsManager.GetConfigValue("10") == "232" )
                {
                    PrintAppForm_Mati(sAppDate, sDTINo, sDTIDate, sOrgKind, sCTCNo, sTIN);
                    // RMC 20140103 modified printing of application form for Mati (e)
                }
                else if (AppSettingsManager.GetConfigValue("10") == "019")  // RMC 20150426 QA corrections
                {
                    //PrintAppForm_Lubao(sAppDate, sDTINo, sDTIDate, sOrgKind, sCTCNo, sTIN, sTINDate, sSSS, sSSSDate, sPhilhealth, sPhilhealthDate, sCTCDate, sBussPlate, m_sAppNo); //MCR 20150102
                    PrintAppForm_Lubao(sAppDate, sDTINo, sDTIDate, sOrgKind, sCTCNo, sTIN, sTINDate, sSSS, sSSSDate, sPhilhealth, sPhilhealthDate, sCTCDate, m_sBussPlate, m_sAppNo); //MCR 20150102  // RMC 20150623 shoot app form in pre-printed form
                }
                else if (AppSettingsManager.GetConfigValue("10") == "243")  // RMC 20161129 customized application form for Binan
                {
                    PrintAppForm_Binan(sAppDate, sDTINo, sDTIDate, sOrgKind, sCTCNo, sTIN, sTINDate, sSSS, sSSSDate, sPhilhealth, sPhilhealthDate, sCTCDate, m_sBussPlate, m_sAppNo);
                }
                else if (AppSettingsManager.GetConfigValue("10") == "216")  // RMC 20161129 customized application form for Binan
                {
                    PrintAppForm_Malolos(sAppDate, sDTINo, sDTIDate, sOrgKind, sCTCNo, sTIN, sTINDate, sSSS, sSSSDate, sPhilhealth, sPhilhealthDate, sCTCDate, m_sBussPlate, m_sAppNo);
                } 
                else if (AppSettingsManager.GetConfigValue("10") == "192")//calamba?
                {
                    this.axVSPrinter1.FontName = "Arial Narrow";
                    this.axVSPrinter1.FontSize = (float)10.0;
                    this.axVSPrinter1.FontBold = true;

                    strData = "x";
                    if (m_sApplType == "NEW")
                    {
                        this.axVSPrinter1.CurrentY = 1350;
                        this.axVSPrinter1.MarginLeft = 2464;
                        this.axVSPrinter1.Table = string.Format("<1500;{0}", strData);
                    }
                    else if (m_sApplType == "REN")
                    {
                        this.axVSPrinter1.CurrentY = 1550;
                        this.axVSPrinter1.MarginLeft = 3999;
                        this.axVSPrinter1.Table = string.Format("<1500;{0}", strData);
                    }
                    else if (m_sApplType == "ADDL")
                    {
                        this.axVSPrinter1.CurrentY = 1350;
                        this.axVSPrinter1.MarginLeft = 6810;
                        this.axVSPrinter1.Table = string.Format("<1500;{0}", strData);
                    }

                    this.axVSPrinter1.CurrentY = 1550;
                    this.axVSPrinter1.MarginLeft = 9100;
                    this.axVSPrinter1.Table = string.Format("<2500;{0}", m_sBIN);

                    this.axVSPrinter1.CurrentY = 3500;
                    this.axVSPrinter1.MarginLeft = 2464;
                    strData = sAppDate + "||" + sDTINo + "||" + sPEZANo;
                    this.axVSPrinter1.Table = string.Format("<1000|<3500|<1500|<2500|<1000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 3900;
                    this.axVSPrinter1.MarginLeft = 2464;
                    //strData = sBussPlate + "||" + sDTIDate + "||" + sPEZADate;
                    strData = m_sBussPlate + "||" + sDTIDate + "||" + sPEZADate;    // RMC 20150623 shoot app form in pre-printed form
                    this.axVSPrinter1.Table = string.Format("<1000|<3500|<1000|<2500|<1000;{0}", strData);

                    strData = "x";
                    if (sOrgKind == "SINGLE PROPRIETORSHIP")
                    {
                        this.axVSPrinter1.CurrentY = 4200;
                        this.axVSPrinter1.MarginLeft = 2464;
                        this.axVSPrinter1.Table = string.Format("<1500;{0}", strData);
                    }
                    else if (sOrgKind == "PARTNERSHIP")
                    {
                        this.axVSPrinter1.CurrentY = 4200;
                        this.axVSPrinter1.MarginLeft = 3400;
                        this.axVSPrinter1.Table = string.Format("<1500;{0}", strData);
                    }
                    else if (sOrgKind == "CORPORATION")
                    {
                        this.axVSPrinter1.CurrentY = 4200;
                        this.axVSPrinter1.MarginLeft = 4640;
                        this.axVSPrinter1.Table = string.Format("<1500;{0}", strData);
                    }
                    /*
                    else if (sOrgKind == "COOPERATIVE")
                    {
                    }
                     */

                    this.axVSPrinter1.CurrentY = 4200;
                    this.axVSPrinter1.MarginLeft = 8200;
                    //strData = sCTCNo + "||" + sTIN; // GDE 20121227
                    strData = "|" + sCTCNo + "||" + sTIN;
                    //this.axVSPrinter1.Table = string.Format("<1000|<500|<1500;{0}", strData); // GDE 20121227
                    this.axVSPrinter1.Table = string.Format("<2000|<1000|<500|<1500;{0}", strData);

                    this.axVSPrinter1.CurrentY = 4600;
                    this.axVSPrinter1.MarginLeft = 8200;
                    strData = "||" + m_sOwnNationality;
                    this.axVSPrinter1.Table = string.Format("<1000|<1200|<1500;{0}", strData);

                    this.axVSPrinter1.CurrentY = 5000;
                    this.axVSPrinter1.MarginLeft = 3000;
                    if (m_sOwnFn.Trim() == "")
                    {
                        strData = m_sOwnLn;
                        this.axVSPrinter1.Table = string.Format("<11500;{0}", strData);
                    }
                    else
                    {
                        strData = m_sOwnLn + "||" + m_sOwnFn + "||" + m_sOwnMi;
                        this.axVSPrinter1.Table = string.Format("<3000|<800|<3000|<800|<3000;{0}", strData);
                    }

                    this.axVSPrinter1.CurrentY = 5350;
                    this.axVSPrinter1.MarginLeft = 3000;
                    this.axVSPrinter1.Table = string.Format("<11500;{0}", sBnsName);

                    this.axVSPrinter1.CurrentY = 6700;
                    this.axVSPrinter1.MarginLeft = 2464;
                    strData = m_sBnsHse + "||" + m_sOwnHse;
                    this.axVSPrinter1.Table = string.Format("<4000|<1700|<3000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 6900;
                    strData = m_sBldgName + "| |";
                    this.axVSPrinter1.Table = string.Format("<3000|<1000|<3000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 7400;
                    this.axVSPrinter1.MarginLeft = 1464;
                    strData = "||" + m_sBnsStr + "||||" + m_sOwnStr;
                    this.axVSPrinter1.Table = string.Format("<1500|<800|<2000|<800|<1500|<1000|<2000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 7700;
                    strData = m_sBnsBrgy + "||" + m_sOwnBrgy;
                    this.axVSPrinter1.Table = string.Format("<4000|<1500|<4000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 8600;
                    this.axVSPrinter1.MarginLeft = 1500;
                    strData = m_sBnsMun + "||" + m_sBnsProv + "||" + m_sOwnMun + "||" + m_sOwnProv;
                    this.axVSPrinter1.Table = string.Format("<1500|<800|<2000|<1200|<1500|<800|<1500;{0}", strData);

                    this.axVSPrinter1.CurrentY = 8900;
                    strData = m_sTelNo + "||" + m_sEmail + "||" + m_sOwnTelNo + "||" + m_sOwnEmailAdd;
                    this.axVSPrinter1.Table = string.Format("<1500|<800|<2000|<1000|<1500|<1100|<1500;{0}", strData);

                    //strData = m_sTelNo + "||" + m_sEmail + "||" + m_sOwnTelNo + "||" + m_sOwnEmailAdd;
                    //this.axVSPrinter1.Table = string.Format("<8000|<2000;{0}", strData);
                    this.axVSPrinter1.Table = string.Format("<8000|<2000;TEST");

                    this.axVSPrinter1.CurrentY = 9200;
                    this.axVSPrinter1.MarginLeft = 3100;
                    this.axVSPrinter1.Table = string.Format("<11500;{0}", sLandPin);

                    this.axVSPrinter1.CurrentY = 9800;
                    this.axVSPrinter1.MarginLeft = 2500;
                    strData = m_sArea + "||" + m_sEmployee + "||";
                    this.axVSPrinter1.Table = string.Format("<2000|<2500|<1500|<3500|<1000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 10700;
                    this.axVSPrinter1.MarginLeft = 5499;
                    GetOwnerDetails(sLessor);
                    strData = m_sOwnLn + ", " + m_sOwnFn + " " + m_sOwnMi + "||" + sRent;
                    this.axVSPrinter1.Table = string.Format("<4000|<1000|<1500;{0}", strData);

                    this.axVSPrinter1.CurrentY = 11200;
                    this.axVSPrinter1.MarginLeft = 2464;
                    strData = "|" + m_sOwnHse + "||";
                    this.axVSPrinter1.Table = string.Format("<1000|<3000|<1000|<3000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 11600;
                    this.axVSPrinter1.MarginLeft = 2464;
                    strData = m_sOwnStr + "||" + m_sOwnMun;
                    this.axVSPrinter1.Table = string.Format("<4000|<1000|<3000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 12000;
                    this.axVSPrinter1.MarginLeft = 2464;
                    strData = m_sOwnBrgy + "||" + m_sOwnProv;
                    this.axVSPrinter1.Table = string.Format("<4000|<1000|<3000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 12400;
                    this.axVSPrinter1.MarginLeft = 2464;
                    strData = m_sOwnTelNo + "||" + m_sOwnEmailAdd;
                    this.axVSPrinter1.Table = string.Format("<4000|<1000|<3000;{0}", strData);

                    this.axVSPrinter1.CurrentY = 13000;
                    this.axVSPrinter1.MarginLeft = 800;
                    string sEss = "";
                    string sNonEss = "";
                    
                    if (ValidateEssential(m_sBnsCode))
                    {
                        sEss = sGross;
                        sNonEss = "";
                    }
                    else
                    {
                        sEss = "";
                        sNonEss = sGross;
                    }

                    strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevGrossCap + "|" + sEss + "|" + sNonEss;  // RMC 20111228 added display of prev gross/cap in app form
                    this.axVSPrinter1.Table = string.Format("<1000|<3000|>2500|>2000|>2000;{0}", strData);

                    pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' order by bns_code_main";
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            m_sBnsCode = pRec.GetString("bns_code_main");

                            // RMC 20111228 added display of prev gross/cap in app form (s)
                            if (pRec.GetString("bns_stat").Trim() == "NEW")
                                sPrevGrossCap = "(Prev. Capital) " + string.Format("{0:#,###.00}", pRec.GetDouble("capital"));
                            else
                                sPrevGrossCap = "(Prev. Gross) " + string.Format("{0:#,###.00}", pRec.GetDouble("gross"));
                            // RMC 20111228 added display of prev gross/cap in app form (e)

                            /*if (pRec.GetString("bns_stat").Trim() == "NEW")
                                sGross = string.Format("{0:#,###.00}", pRec.GetDouble("capital"));
                            else
                                sGross = string.Format("{0:#,###.00}", pRec.GetDouble("gross"));
                            */
                            sGross = "";    // gross will be filled-up by tax payer
                            if (ValidateEssential(m_sBnsCode))
                            {
                                sEss = sGross;
                                sNonEss = "";
                            }
                            else
                            {
                                sEss = "";
                                sNonEss = sGross;
                            }

                            strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevGrossCap + "|" + sEss + "|" + sNonEss;  // RMC 20111228 added display of prev gross/cap in app form
                            this.axVSPrinter1.Table = string.Format("<1000|<3000|>2500|>2000|>2000;{0}", strData);
                        }
                    }
                    pRec.Close();
                    this.axVSPrinter1.CurrentY = 17700;
                    this.axVSPrinter1.Table = string.Format("<10500;{0}", sMemo);
                }
                else
                {
                    // RMC 20150623 shoot app form in pre-printed form

                    //latest format from Tumauini (use pre-printed form of Tumauini)

                    PrintAppForm_Tumauini(sAppDate, sDTINo, sDTIDate, sOrgKind, sCTCNo, sTIN, sTINDate, sSSS, sSSSDate, sPhilhealth, sPhilhealthDate, sCTCDate, m_sBussPlate, m_sAppNo);
                }

                // RMC 20120102 Added trailing of printing application form (s)
                if (AuditTrail.AuditTrail.InsertTrail("AARAPF", "businesses", m_sBIN + " for tax year " + sTaxYear) == 0)
                {
                    pRec.Rollback();
                    pRec.Close();
                    MessageBox.Show(pRec.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // RMC 20120102 Added trailing of printing application form (e)
            }
        }

        private bool ValidateEssential(string sBnsCode)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from bns_table where fees_code = 'B' and length(bns_code) = 2 ";
            pSet.Query += " and bns_code = '" + sBnsCode.Substring(0, 2) + "'";
            pSet.Query += " and bns_desc like '%ESSENTIAL%'";
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
        private void GetAddlInfo(string sBIN) //MCR 20140617
        {
            OracleResultSet result = new OracleResultSet();
            m_sNoFemale = "0";
            m_sNoMale = "0";
            m_sNoGender = "0";  // RMC 20150819 display No. of resident employee in application form printing

            result.Query = "select * from addl_info_tmp where bin = '" + sBIN + "' order by addl_code";
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();

                    result.Query = "select * from addl_info_tmp where bin = '" + sBIN + "' order by addl_code";
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            if (result.GetString("addl_code") == "001")
                                m_sNoFemale = result.GetString("value");

                            //while (result.Read()) // RMC 20150819 display No. of resident employee in application form printing, put rem
                            {

                                if (result.GetString("addl_code") == "002")
                                    m_sNoMale = result.GetString("value");

                                // RMC 20150819 display No. of resident employee in application form printing (S)
                                if (result.GetString("addl_code") == "003")
                                    m_sNoGender = result.GetString("value");
                                // RMC 20150819 display No. of resident employee in application form printing (E)
                            }
                        }
                    }
                    result.Close(); // RMC 20150819 display No. of resident employee in application form printing
                }
                else
                {
                    result.Close();
                    result.Query = "select * from addl_info where bin = '" + sBIN + "' order by addl_code";
                    if (result.Execute())
                    {
                        //if (result.Read())
                        while (result.Read()) // RMC 20150819 display No. of resident employee in application form printing
                        {
                            if (result.GetString("addl_code") == "001")
                                m_sNoFemale = result.GetString("value");

                            //while (result.Read())     // RMC 20150819 display No. of resident employee in application form printing, put rem
                            {
                                if (result.GetString("addl_code") == "002")
                                    m_sNoMale = result.GetString("value");

                                // RMC 20150819 display No. of resident employee in application form printing (S)
                                if (result.GetString("addl_code") == "003")
                                    m_sNoGender = result.GetString("value");
                                // RMC 20150819 display No. of resident employee in application form printing (E)
                            }
                        }
                    }
                }
            }
            result.Close();

            /*
            //MCR 20150114 (s)
            int iTmp = Convert.ToInt32(m_sNoMale) + Convert.ToInt32(m_sNoFemale);
            m_sNoGender = iTmp.ToString();
            //MCR 20150114 (e)*/
            // RMC 20150819 display No. of resident employee in application form printing, put rem
        }

        private void GetPermitUpdates(string sBIN)
        {
            OracleResultSet result = new OracleResultSet();
            m_sOldOwnCode = "";
            m_sNewOwnCode = "";
            m_sOldLocation = "";
            m_sNewLocation = "";

            result.Query = "Select * From Permit_Update_Appl Where Bin = '" + sBIN + "' And Tax_Year = '" + sTaxYear + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    if (result.GetString("appl_type") == "TOWN")
                    {
                        m_sOldOwnCode = result.GetString("old_own_code");
                        m_sNewOwnCode = result.GetString("new_own_code");
                    }
                    else if (result.GetString("appl_type") == "TLOC")
                    {
                        m_sOldLocation = result.GetString("old_bns_loc");
                        m_sNewLocation = result.GetString("new_bns_loc");
                    }
                }
            }
            result.Close();
        }

        private void GetGISLocation(string sBIN)
        {
            OracleResultSet result = new OracleResultSet();
            m_sBldgName = "";
            m_sLandPIN = "";

            result.Query = "select * from btm_gis_loc where bin = '" + sBIN + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sBldgName = result.GetString("bldg_name");
                    m_sLandPIN = result.GetString("land_pin");
                }
            }
            result.Close();

            
        }

        private void GetOwnerDetails(string sOwnCode)
        {
            OracleResultSet result = new OracleResultSet();

            m_sOwnLn = "";
            m_sOwnFn = "";
            m_sOwnMi = "";
            m_sOwnHse = "";
            m_sOwnStr = "";
            m_sOwnBrgy = "";
            m_sOwnMun = "";
            m_sOwnProv = "";
            m_sOwnNationality = "";
            m_sOwnTelNo = "";
            m_sOwnEmailAdd = "";
            m_sSpouse = "";
            m_sGender = "";

            result.Query = "select * from own_names where own_code = '" + sOwnCode + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sOwnLn = result.GetString("own_ln").Trim();
                    m_sOwnFn = result.GetString("own_fn").Trim();
                    m_sOwnMi = result.GetString("own_mi").Trim();
                    m_sOwnHse = result.GetString("own_house_no");
                    m_sOwnStr = result.GetString("own_street");
                    m_sOwnBrgy = result.GetString("own_brgy");
                    m_sOwnMun = result.GetString("own_mun");
                    m_sOwnProv = result.GetString("own_prov");
                    m_sOwnZip = result.GetString("own_zip");   // RMC 20171116 added info to display in application form
                }
            }
            result.Close();
            
            result.Query = "select * from own_profile where own_code = '" + sOwnCode + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sOwnNationality = result.GetString("citizenship");
                    m_sOwnTelNo = result.GetString("tel_no");
                    m_sOwnEmailAdd = result.GetString("email_add");
                    m_sBdate = result.GetDateTime("bdate").ToShortDateString();
                    m_sSpouse = result.GetString("spouse"); 
                    m_sGender = result.GetString("gender");
                }
            }
            result.Close();

        }

        private void PrintDeficiency()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            string sBIN = string.Empty;
            string sBnsName = string.Empty;
            string sDetails = string.Empty;
            string sApplType = string.Empty;
            string sCurrent = string.Empty;
            string sNew = string.Empty;
            string strData = string.Empty;

            CreateHeader();

            pRec.Query = "select * from btm_businesses where bin = '" + m_sBIN + "'";
            pRec.Query += " and bin in ";
            pRec.Query += " (select bin from btm_update)";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sBIN = pRec.GetString("bin");
                    sBnsName = AppSettingsManager.GetBnsName(sBIN);

                    pSet.Query = "select * from btm_update where bin = '" + sBIN + "'";
                    if (pSet.Execute())
                    {
                        int iCnt = 0;
                        while (pSet.Read())
                        {
                            iCnt++;
                            sApplType = pSet.GetString("appl_type");

                            if (sApplType == "CBNS")
                            {
                                sDetails = "Business Name";
                                sCurrent = pSet.GetString("old_bns_name");
                                sNew = pSet.GetString("new_bns_name");
                            }
                            if (sApplType == "TLOC")
                            {
                                sDetails = "Location";
                                sCurrent = pSet.GetString("old_bns_loc");
                                sNew = pSet.GetString("new_bns_loc");
                            }
                            if (sApplType == "TOWN")
                            {
                                sDetails = "Owner";
                                sCurrent = pSet.GetString("old_own_code");
                                sCurrent = AppSettingsManager.GetBnsOwner(sCurrent);
                                sNew = pSet.GetString("new_own_code");
                                sNew = AppSettingsManager.GetBnsOwner(sNew);
                            }
                            if (sApplType == "CTYP")
                            {
                                sDetails = "Business Type";
                                sCurrent = pSet.GetString("old_bns_code");
                                sCurrent = AppSettingsManager.GetBnsDesc(sCurrent);
                                sNew = pSet.GetString("new_bns_code");
                                sNew = AppSettingsManager.GetBnsDesc(sNew);
                            }
                            if (sApplType == "ADDL")
                            {
                                sDetails = "Other Line/s of Business";
                            }
                            if (sApplType == "CBOW")
                            {
                                sDetails = "Lessor's details";
                                sCurrent = pSet.GetString("old_own_code");
                                sCurrent = AppSettingsManager.GetBnsOwner(sCurrent);
                                sNew = pSet.GetString("new_own_code");
                                sNew = AppSettingsManager.GetBnsOwner(sNew);
                            }
                            if (sApplType == "CPMT")
                            {
                                sDetails = "Permit No.";
                                sNew = pSet.GetString("permit_no");
                                sCurrent = GetPermitNo(sBIN, true);
                            }
                            if (sApplType == "CTYR")
                            {
                                sDetails = "Tax Year";
                                sNew = string.Format("{0:yyyy}", pSet.GetDateTime("permit_dt"));
                                sCurrent = GetPermitNo(sBIN, false);
                            }
                            if (sApplType == "AREA")
                            {
                                sDetails = "Area";
                                sCurrent = pSet.GetString("old_bns_name");
                                sNew = pSet.GetString("new_bns_name");
                            }
                            if (sApplType == "CSTR")
                            {
                                sDetails = "No. of storeys";
                                sCurrent = pSet.GetString("old_bns_name");
                                sNew = pSet.GetString("new_bns_name");
                            }

                            if (iCnt == 1)
                            {
                                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                                strData = sBIN + "|" + sBnsName + "|" + sDetails + "|" + sCurrent + "|" + sNew;
                                this.axVSPrinter1.Table = string.Format("<1500|<3500|<2000|<4000|<4000;{0}", strData);
                            }
                            else
                            {
                                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                                strData = "||" + sDetails + "|" + sCurrent + "|" + sNew;
                                this.axVSPrinter1.Table = string.Format("<1500|<3500|<2000|<4000|<4000;{0}", strData);
                            }
                        }
                    }
                    pSet.Close();

                    this.axVSPrinter1.Paragraph = "";
                }
            }
            pRec.Close();

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = string.Format("<5000;Printed by: {0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<5000;Date printed: {0}", AppSettingsManager.GetCurrentDate());
        }

        private string GetPermitNo(string sBIN, bool bNo)
        {
            OracleResultSet result = new OracleResultSet();
            string sPermitNo = "";
            string sPermitDt = "";

            result.Query = "select permit_no, permit_dt from businesses where bin = '" + sBIN + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sPermitNo = result.GetString(0);
                    sPermitDt = string.Format("{0:yyyy}", result.GetDateTime(1));
                }
            }
            result.Close();

            if (bNo)
                return sPermitNo;
            else
                return sPermitDt;
        }

        private void CreateHeader()
        {
            string strProvinceName = string.Empty;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Table = "^16000;Republic of the Philippines";
            this.axVSPrinter1.FontBold = false;

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                this.axVSPrinter1.Table = string.Format("^16000;{0}", "Province of " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strProvinceName.ToLower()));
            this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("09"));
            this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("41"));
            this.axVSPrinter1.Paragraph = "";
            if(m_sApplType == "CERT-STAT")
            {
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.FontSize = (float)14.0;
                this.axVSPrinter1.Table = "^16000;CERTIFICATION";
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sApplType != "Retirement")    // JHMN 20170116 added for retirement application
            {
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = "^16000;Deficient Business with Details";
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
                string strData = "";
                strData = "BIN|Business Name|Changed Field|Current|New";
                this.axVSPrinter1.Table = string.Format("^1500|^3500|^2000|^4000|^4000;{0}", strData);
                this.axVSPrinter1.Paragraph = "";
            }
        }
        private string GetArea(string sBIN, string sBnsCode, string sTaxYear)
        {
            // RMC 20120105 Modified getting of area in printing App form
            OracleResultSet pCode = new OracleResultSet();
            OracleResultSet pArea = new OracleResultSet();
            string sDefaultCode = "";
            string sArea = "";
            double dArea = 0;

            pCode.Query = "select * from default_code where default_desc like 'AREA IN%' or default_desc like '%SQ METERS%'";
            pCode.Query += " or default_desc like '%BUSINESS AREA%'";   // RMC 20150819 display area in application form printing
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

        private void PrintAppForm_Mati(string sAppDate, string sDTINo, string sDTIDate, string sOrgKind, string sCTCNo, string sTIN)
        {
            // RMC 20140103 modified printing of application form for Mati
            #region RMC
            /*
            string strData = string.Empty;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = 1350;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.FontBold = true;

            if (m_sApplType == "NEW")
                strData = "|x|||||";
            else if (m_sApplType == "REN")
                strData = "|||x|||";
            else
                strData = "||||x||";

            this.axVSPrinter1.Table = string.Format("<2000|^200|<2500|^200|<2500|^200|<2500;{0}", strData);
            this.axVSPrinter1.CurrentY = 1650;

            strData = "|" + sAppDate + "||" + sDTINo;
            this.axVSPrinter1.Table = string.Format("<2000|<2000|<2500|<2000;{0}", strData);
            strData = "|" + m_sBIN + "||" + sDTIDate;
            this.axVSPrinter1.Table = string.Format("<2000|<2000|<2500|<2000;{0}", strData);

            if (sOrgKind == "SINGLE PROPRIETORSHIP")
                strData = "|x|||||";
            else if (sOrgKind == "PARTNERSHIP")
                strData = "||x||||";
            else if (sOrgKind == "CORPORATION")
                strData = "|||x|||";
            else
                strData = "||||x||";
            strData+= sCTCNo + "||" + sTIN;
            this.axVSPrinter1.Table = string.Format("<2000|<1000|<1000|<1000|<1000|<500|<1000|<500|<1000;{0}", strData);
            */
            #endregion

            
            
            // MCR 20140616 shooting of application in pre-printed form for renewal
            string strData = string.Empty;
            string strOwner = string.Empty;
            this.axVSPrinter1.CurrentY = 2800;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.FontBold = true;
            strData = "x";

            if (m_sApplType == "NEW")
                this.axVSPrinter1.MarginLeft = 3120;
            else if (m_sApplType == "REN")
                this.axVSPrinter1.MarginLeft = 6005;
            else
                this.axVSPrinter1.MarginLeft = 8905;

            this.axVSPrinter1.Table = string.Format("<2000;{0}", strData);

            this.axVSPrinter1.MarginLeft = 3120;
            //From - to Transfer 
            if (m_sOldOwnCode != "" && m_sOldLocation != "")
            {
                this.axVSPrinter1.Table = "<2870|<2700;x|x";
                this.axVSPrinter1.MarginLeft = 1000;
                this.axVSPrinter1.Table = string.Format("<600|<7000|<2600|<7000|<3500;|{0}|{1}|{2}|{3}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), " ", m_sOldLocation, " ");
                //this.axVSPrinter1.Table = string.Format("<600|<2500|<2600|<3500|<3500;|{0}|{1}|{2}|{3}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), AppSettingsManager.GetBnsOwner(m_sNewOwnCode), m_sOldLocation, m_sNewLocation);
            }
            else if (m_sOldOwnCode != "" && m_sOldLocation == "")
            {
                this.axVSPrinter1.Table = "<2870|<2700;x|";
                this.axVSPrinter1.MarginLeft = 1000;
                this.axVSPrinter1.Table = string.Format("<600|<7000|<2600|<3500|<3500;|{0}|{1}|{2}|{3}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), " ", " ", " ");
                //this.axVSPrinter1.Table = string.Format("<600|<2500|<2600|<3500|<3500;|{0}|{1}|{2}|{3}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), AppSettingsManager.GetBnsOwner(m_sNewOwnCode), " ", " ");
            }
            else if (m_sOldOwnCode == "" && m_sOldLocation != "")
            {
                this.axVSPrinter1.Table = "<2870|<2700;|x";
                this.axVSPrinter1.MarginLeft = 1000;
                this.axVSPrinter1.Table = string.Format("<600|<2500|<2600|<7000|<3500;|{0}|{1}|{2}|{3}", " ", " ", m_sOldLocation, " ");
                //this.axVSPrinter1.Table = string.Format("<600|<2500|<2600|<3500|<3500;|{0}|{1}|{2}|{3}", " ", " ", m_sOldLocation, m_sNewLocation);
            }
            else
            {
                this.axVSPrinter1.Table = "<2870|<2700;|";
                this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            }
            this.axVSPrinter1.MarginLeft = 2500;
            this.axVSPrinter1.Table = string.Format("<2000|^2150|^2150|<2000;{0}|||{1}", sAppDate, sDTINo);
            this.axVSPrinter1.Table = string.Format("<2000|^2150|^2150|<2000;{0}|||{1}", m_sBIN, sDTIDate);

            this.axVSPrinter1.CurrentY = 4150;
            this.axVSPrinter1.MarginLeft = 2530;
            if (sOrgKind == "SINGLE PROPRIETORSHIP")
                strData = "x|||||";
            else if (sOrgKind == "PARTNERSHIP")
                strData = "|x||||";
            else if (sOrgKind == "CORPORATION")
                strData = "||x|||";
            else
                strData = "|||x||";
            strData += sCTCNo + "||" + sTIN;
            this.axVSPrinter1.Table = string.Format("<1750|<1080|<1090|<1000|<1000|<1000|<900|<3000;{0}", strData);

            strData = "|||||||" + m_sOwnNationality;
            this.axVSPrinter1.Table = string.Format("<1750|<1080|<1090|<1000|<1000|<1000|<1100|<3000;{0}", strData);

            if (m_sOwnFn.Trim() == "")
            {
                strOwner = m_sOwnLn;
                if (m_sBdate == "")
                {
                    strData = "|" + m_sOwnLn;
                    this.axVSPrinter1.Table = string.Format("<2000|<4600;{0}", strData);
                }
                else
                {
                    strData = "|" + m_sOwnLn + "|" + m_sBdate;
                    this.axVSPrinter1.Table = string.Format("<2000|<4600|<1000;{0}", strData);
                }
            }
            else
            {
                strOwner = m_sOwnLn + ", " + m_sOwnFn + " " + m_sOwnMi + ".";
                if (m_sBdate == "" && m_sGender == "")
                {
                    strData = "|" + m_sOwnLn + ", " + m_sOwnFn + " " + m_sOwnMi + ".";
                    this.axVSPrinter1.Table = string.Format("<2000|<4600;{0}", strData);
                }
                else if (m_sGender == "")
                {
                    strData = "|" + m_sOwnLn + ", " + m_sOwnFn + " " + m_sOwnMi + "." + "|" + m_sBdate;
                    this.axVSPrinter1.Table = string.Format("<2000|<4600|<400;{0}", strData);
                }
                else
                {
                    strData = "|" + m_sOwnLn + ", " + m_sOwnFn + " " + m_sOwnMi + "." + "|" + m_sBdate + "||x";
                    if (m_sGender == "MALE")
                        this.axVSPrinter1.Table = string.Format("<2000|<4600|<1000|<750|<500;{0}", strData);
                    else //if (m_sGender == "FEMALE")
                        this.axVSPrinter1.Table = string.Format("<2000|<4600|<1000|<1150|<500;{0}", strData);
                }
            }

            if (m_sSpouse == "")
                this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            else
                this.axVSPrinter1.Table = string.Format("<4600;{0}", m_sSpouse);

            this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetBnsName(m_sBIN));

            //Trade name/Franchise
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.Table = string.Format("<3500|<4600;|{0}", strOwner);
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");

            this.axVSPrinter1.CurrentY = 6350;
            this.axVSPrinter1.Table = string.Format("<5300|<5000;{0}|{1}", m_sBnsHse, m_sOwnHse);
            this.axVSPrinter1.MarginLeft = 1900;
            this.axVSPrinter1.Table = string.Format("<5300|<5000;{0}|{1}", m_sBnsStr, m_sOwnStr);
            //Subdivision
            this.axVSPrinter1.CurrentY = 6900;
            this.axVSPrinter1.Table = string.Format("<5300|<5000;{0}|{1}", " ", " ");
            this.axVSPrinter1.Table = string.Format("<2400|<2900|<3050|<1950;{0}|{1}|{2}|{3}", m_sBnsBrgy, m_sBnsMun, m_sOwnBrgy, m_sOwnMun);
            this.axVSPrinter1.Table = string.Format("<2400|<2900|<3050|<1950;{0}|{1}|{2}|{3}", m_sBnsProv, m_sTelNo, m_sOwnProv, m_sOwnTelNo);
            this.axVSPrinter1.Table = string.Format("<2400|<2900|<3050|<1950;{0}|{1}|{2}|{3}", " ", m_sEmail, " ", m_sOwnEmailAdd);
            
            this.axVSPrinter1.MarginLeft = 2400;
            this.axVSPrinter1.CurrentY = 8000;
            if (m_sLandPIN == "")
                this.axVSPrinter1.Table = string.Format("<4600;{0}", " ");
            else
                this.axVSPrinter1.Table = string.Format("<4600;{0}", m_sLandPIN);

            this.axVSPrinter1.Table = string.Format("<3800|<1500|<1000;{0}|{1}|{2}", m_sArea, m_sEmployee, m_sNoGender);
            if (sLessor != "")
            {
                GetOwnerDetails(sLessor);
                this.axVSPrinter1.CurrentY = 8530;
                this.axVSPrinter1.Table = string.Format("<3200|<3600|<1000|<2000;|{0}||{1}", AppSettingsManager.GetBnsOwner(sLessor), sRent);
                this.axVSPrinter1.Table = string.Format("<1000|<2100|<5000;|{0}|{1}", m_sOwnHse, m_sOwnStr);
                this.axVSPrinter1.MarginLeft = 1900;
                this.axVSPrinter1.Table = string.Format("<3800|<5000;{0}|{1}", " ", m_sOwnBrgy);
                this.axVSPrinter1.Table = string.Format("<3800|<5000;{0}|{1}", m_sOwnMun, m_sOwnProv);
                this.axVSPrinter1.Table = string.Format("<4000|<5000;{0}|{1}", m_sOwnTelNo, m_sOwnEmailAdd);
            }
            else
            {
                this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
                this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
                this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
                this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
                this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            }

            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = 10535;
            string sEss = "";
            string sNonEss = "";
            sGross = AppSettingsManager.GetCapitalGross(m_sBIN, m_sBnsCode, sTaxYear, m_sApplType);

            if (ValidateEssential(m_sBnsCode))
            {
                sEss = Convert.ToDouble(sGross).ToString("#,##0.00");
                sNonEss = "";
            }
            else
            {
                sEss = "";
                sNonEss = Convert.ToDouble(sGross).ToString("#,##0.00");
            }

            strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + " " + "|" +sEss + "|" + sNonEss;
            this.axVSPrinter1.Table = string.Format("<800|<5030|<2170|<1500|<2000;{0}", strData);

            this.axVSPrinter1.CurrentY = 10800;
            OracleResultSet pRec = new OracleResultSet();
            pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' order by bns_code_main";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sPrevGrossCap = "";
                    m_sBnsCode = pRec.GetString("bns_code_main");
                    if (pRec.GetString("bns_stat").Trim() == "NEW")
                        sPrevGrossCap = string.Format("{0:#,##0.00}", pRec.GetDouble("capital"));
                    else
                        sGross = string.Format("{0:#,##0.00}", pRec.GetDouble("gross"));
                    if (ValidateEssential(m_sBnsCode))
                    {
                        sEss = sGross;
                        sNonEss = "";
                    }
                    else
                    {
                        sEss = "";
                        sNonEss = sGross;
                    }

                    strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevGrossCap + "|" + sEss + "|" + sNonEss;  // RMC 20111228 added display of prev gross/cap in app form
                    this.axVSPrinter1.Table = string.Format("<800|<5030|<2170|<1500|<2000;{0}", strData);
                }
            }
            pRec.Close();
        }

        private void PrintAppForm_Lubao(string sAppDate, string sDTINo, string sDTIDate, string sOrgKind, string sCTCNo, string sTIN, string sTINDate, string sSSS, string sSSSDate, string sPhilhealth, string sPhilhealthDate, string sCTCDate, string sBnsPlate, string sAppNo)
        {
            // MCR 20140616 shooting of application in pre-printed form for renewal
            string strData = string.Empty;
            string strOwner = string.Empty;

            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.FontBold = true;

            this.axVSPrinter1.CurrentY = 2247; //2647
            this.axVSPrinter1.MarginLeft = 2200;
            this.axVSPrinter1.Table = string.Format("<4800|<1700;{0}|{1}", sAppNo, sBnsPlate);

            this.axVSPrinter1.CurrentY = 2610; //2890
            strData = "x";

            if (m_sApplType == "NEW")//+20
                this.axVSPrinter1.MarginLeft = 2980;
            else if (m_sApplType == "REN")
                this.axVSPrinter1.MarginLeft = 5830; //5800
            else
                this.axVSPrinter1.MarginLeft = 8690;

            this.axVSPrinter1.Table = string.Format("<2000;{0}", strData);

            this.axVSPrinter1.MarginLeft = 2945;
            this.axVSPrinter1.CurrentY = 2905; //3235
            //From - to Transfer 
            if (m_sOldOwnCode != "" && m_sOldLocation != "")
            {
                this.axVSPrinter1.Table = "<2875|<2520;x|x";
                this.axVSPrinter1.MarginLeft = 2650;
                this.axVSPrinter1.Table = string.Format("<5200|<5000;{0}|{1}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), m_sOldLocation);
                //this.axVSPrinter1.Table = string.Format("<600|<2500|<2600|<3500|<3500;|{0}|{1}|{2}|{3}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), AppSettingsManager.GetBnsOwner(m_sNewOwnCode), m_sOldLocation, m_sNewLocation);
            }
            else if (m_sOldOwnCode != "" && m_sOldLocation == "")
            {
                this.axVSPrinter1.Table = "<2875|<2520;x|";
                this.axVSPrinter1.MarginLeft = 2650;
                this.axVSPrinter1.Table = string.Format("<5200;{0}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode));
                //this.axVSPrinter1.Table = string.Format("<600|<2500|<2600|<3500|<3500;|{0}|{1}|{2}|{3}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), AppSettingsManager.GetBnsOwner(m_sNewOwnCode), " ", " ");
            }
            else if (m_sOldOwnCode == "" && m_sOldLocation != "")
            {
                this.axVSPrinter1.Table = "<2875|<2520;|x";
                this.axVSPrinter1.MarginLeft = 2650;
                this.axVSPrinter1.Table = string.Format("<5200|<5000;|{0}", m_sOldLocation);
                //this.axVSPrinter1.Table = string.Format("<600|<2500|<2600|<3500|<3500;|{0}|{1}|{2}|{3}", " ", " ", m_sOldLocation, m_sNewLocation);
            }
            else
            {
                this.axVSPrinter1.Table = "<2870|<2520;|";//2720
                this.axVSPrinter1.MarginLeft = 2650;
                this.axVSPrinter1.Table = string.Format("<5200;{0}", " ");
            }

            this.axVSPrinter1.CurrentY = 3410; //3780
            this.axVSPrinter1.MarginLeft = 2370;

            if (m_sAppDate != "") //MCR 20150108
                sAppDate = m_sAppDate;
            this.axVSPrinter1.Table = string.Format("<2000|^2150|^2100|<2000;{0}|||{1}", sAppDate, sDTINo);
            this.axVSPrinter1.Table = string.Format("<2000|^2150|^2100|<2000;{0}|||{1}", m_sBIN, sDTIDate);

            this.axVSPrinter1.CurrentY = 4040; //4310
            this.axVSPrinter1.MarginLeft = 2240;
            if (sOrgKind == "SINGLE PROPRIETORSHIP")
                strData = "x|||||";
            else if (sOrgKind == "PARTNERSHIP")
                strData = "|x||||";
            else if (sOrgKind == "CORPORATION")
                strData = "||x|||";
            else
                strData = "|||x||";
            strData += sCTCNo + "|" + sCTCDate;
            this.axVSPrinter1.Table = string.Format("<1900|<1150|<1160|<1000|<1000|<2450|<1000;{0}", strData);

            this.axVSPrinter1.CurrentY = 4607; //4887
            this.axVSPrinter1.MarginLeft = 2580;
            if (m_sOwnFn.Trim() == "")
            {
                strOwner = m_sOwnLn;
                if (m_sBdate == "")
                {
                    strData = m_sOwnLn;
                    this.axVSPrinter1.Table = string.Format("<4600;{0}", strData);
                }
                else
                {
                    strData = m_sOwnLn + "|" + m_sBdate;
                    this.axVSPrinter1.Table = string.Format("<6500|<1000;{0}", strData);
                }
            }
            else
            {
                strOwner = m_sOwnLn + ", " + m_sOwnFn + " " + m_sOwnMi + ".";
                if (m_sBdate == "" && m_sGender == "")
                {
                    strData = m_sOwnLn + "|" + m_sOwnFn + "|" + m_sOwnMi + ".";
                    this.axVSPrinter1.Table = string.Format("<2200|<2300|1000;{0}", strData);
                }
                else if (m_sGender == "")
                {
                    strData = m_sOwnLn + "|" + m_sOwnFn + "|" + m_sOwnMi + "." + "|" + m_sBdate;
                    this.axVSPrinter1.Table = string.Format("<2200|<2300|<900|<1000;{0}", strData);
                }
                else
                {
                    //this.axVSPrinter1.CurrentY = 4886.4; //567
                    strData = m_sOwnLn + "|" + m_sOwnFn + "|" + m_sOwnMi + ".|" + m_sBdate + "||x";
                    if (m_sGender == "MALE")
                        this.axVSPrinter1.Table = string.Format("<2200|<2300|<1700|<1000|<750|<600;{0}", strData);
                    else //if (m_sGender == "FEMALE")
                        this.axVSPrinter1.Table = string.Format("<2200|<2300|<1700|<1000|<1170|<600;{0}", strData);
                }
            }

            this.axVSPrinter1.CurrentY = 4907; //5167
            //sTIN
            this.axVSPrinter1.Table = string.Format("<4800|<1150;{0}|{1}", sTIN, sTINDate);
            this.axVSPrinter1.CurrentY = 5183; //5443
            //SSS
            this.axVSPrinter1.Table = string.Format("<4800|<1150;{0}|{1}", sSSS, sSSSDate);
            this.axVSPrinter1.CurrentY = 5461; //5721
            //PHILHEALTH
            this.axVSPrinter1.Table = string.Format("<4800|<1150;{0}|{1}", sPhilhealth, sPhilhealthDate);

            this.axVSPrinter1.CurrentY = 5721; //6001
            if (m_sSpouse == "")
                this.axVSPrinter1.Table = string.Format("<4600;{0}", " ");
            else
                this.axVSPrinter1.Table = string.Format("<4600;{0}", m_sSpouse);

            this.axVSPrinter1.CurrentY = 6045; //6335
            this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetBnsName(m_sBIN));

            //Trade name/Franchise
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.CurrentY = 6640; //6920
            //this.axVSPrinter1.Table = string.Format("<1800|<2100|<2300|<1000;|{0}", m_sOwnLn + "|" + m_sOwnFn + "|" + m_sOwnMi + "."); //REM MCR 20150127
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");

            this.axVSPrinter1.CurrentY = 7157; //7477
            this.axVSPrinter1.Table = string.Format("<5400|<5000;{0}|{1}", m_sBnsHse, m_sOwnHse);
            this.axVSPrinter1.MarginLeft = 1900;
            this.axVSPrinter1.CurrentY = 7497; //7787
            this.axVSPrinter1.Table = string.Format("<5300|<5000;{0}|{1}", m_sBnsStr, m_sOwnStr);
            //Subdivision
            this.axVSPrinter1.CurrentY = 7797; //8087
            this.axVSPrinter1.Table = string.Format("<5300|<5000;{0}|{1}", " ", " ");
            this.axVSPrinter1.Table = string.Format("<2400|<3000|<3100|<1950;{0}|{1}|{2}|{3}", m_sBnsBrgy, m_sBnsMun, m_sOwnBrgy, m_sOwnMun);
            this.axVSPrinter1.CurrentY = 8353; //8633
            this.axVSPrinter1.Table = string.Format("<2400|<3000|<3100|<1950;{0}|{1}|{2}|{3}", m_sBnsProv, m_sTelNo, m_sOwnProv, m_sOwnTelNo);
            this.axVSPrinter1.CurrentY = 8620; //8911
            this.axVSPrinter1.Table = string.Format("<1400|<4000|<3100;{0}|{1}|{2}", " ", m_sTelNo, m_sOwnEmailAdd);

            this.axVSPrinter1.MarginLeft = 2400;
            this.axVSPrinter1.CurrentY = 8850; //9187

            ////REM MCR 20150114 (s)
            //if (m_sLandPIN == "")
            //    this.axVSPrinter1.Table = string.Format("<4600;{0}", " ");
            //else
            //    this.axVSPrinter1.Table = string.Format("<4600;{0}", m_sLandPIN);
            ////REM MCR 20150114 (e)

            this.axVSPrinter1.Table = string.Format("<3750|<1050|<950|<1000;{0}|{1}|{2}|{3}", m_sArea, m_sEmployee, m_sNoMale, m_sNoFemale);
            if (sLessor != "")
            {
                GetOwnerDetails(sLessor);
                this.axVSPrinter1.MarginLeft = 2630;
                this.axVSPrinter1.CurrentY = 9160; //9420
                this.axVSPrinter1.Table = string.Format("<4100|<3600|<1300|<2000;|{0}||{1}", AppSettingsManager.GetBnsOwner(sLessor), sRent);
                this.axVSPrinter1.Table = string.Format("<1000|<2100|<5000;|{0}|{1}", m_sOwnHse, m_sOwnStr);
                this.axVSPrinter1.MarginLeft = 1950;
                this.axVSPrinter1.CurrentY = 9691; //9751
                this.axVSPrinter1.Table = string.Format("<3800|<5000;{0}|{1}", " ", m_sOwnBrgy);
                this.axVSPrinter1.CurrentY = 9961; //10081
                this.axVSPrinter1.Table = string.Format("<3800|<5000;{0}|{1}", m_sOwnMun, m_sOwnProv);
                this.axVSPrinter1.CurrentY = 10251; //10381
                this.axVSPrinter1.Table = string.Format("<4000|<5000;{0}|{1}", m_sOwnTelNo, m_sOwnEmailAdd);
            }
            else
            {
                this.axVSPrinter1.CurrentY = 10479; //10608
            }

            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.MarginLeft = 370;
            this.axVSPrinter1.CurrentY = 11260; //11600
            string sEss = "";
            string sNonEss = "";
            sGross = AppSettingsManager.GetCapitalGross(m_sBIN, m_sBnsCode, sTaxYear, m_sApplType);

            if (ValidateEssential(m_sBnsCode))
            {
                sEss = Convert.ToDouble(sGross).ToString("#,##0.00");
                sNonEss = "";
            }
            else
            {
                sEss = "";
                sNonEss = Convert.ToDouble(sGross).ToString("#,##0.00");
            }

            //MCR 20150112 (s)
            OracleResultSet pRec = new OracleResultSet();
            int iGross = 0;
            pRec.Query = @"select * from bill_gross_info where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "'";
            if (pRec.Execute())
                if (pRec.Read())
                    if (m_sApplType == "NEW")
                        iGross = pRec.GetInt("CAPITAL");
                    else
                        iGross = pRec.GetInt("GROSS");
            pRec.Close();

            if (m_sApplType == "NEW")
                strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + iGross.ToString("#,##0.00") + "|||";
            else
            {
                int iAssetSize = 0;
                pRec.Query = @"select DC.default_desc,OI.data from other_info OI left join default_code DC on dc.default_code = oi.default_code
where DC.rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' and bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + @"' and (dc.default_desc = 'ASSET SIZE')";
                if (pRec.Execute())
                    if (pRec.Read())
                        iAssetSize = pRec.GetInt(1);
                pRec.Close();

                //strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + iAssetSize.ToString("#,##0.00") + "|" + iGross.ToString("#,##0.00") + "|" + sEss + "|" + sNonEss;
                strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + iAssetSize.ToString("#,##0.00") + "|" + iGross.ToString("#,##0.00") + "|" + sEss + "|" + sNonEss; //MCR 20150128
            }
            //this.axVSPrinter1.Table = string.Format("<800|<3200|<2120|<1670|<1580|<1200|<1200;{0}", strData);
            this.axVSPrinter1.Table = string.Format("<800|<5320|<1670|<1580|<1200|<1200;{0}", strData); //MCR 20150128
            
            //MCR 20150112 (e)

            this.axVSPrinter1.CurrentY = 11439; //11879
            pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' order by bns_code_main";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sPrevGrossCap = "";
                    m_sBnsCode = pRec.GetString("bns_code_main");
                    if (pRec.GetString("bns_stat").Trim() == "NEW")
                        sPrevGrossCap = string.Format("{0:#,##0.00}", pRec.GetDouble("capital"));
                    else
                        sGross = string.Format("{0:#,##0.00}", pRec.GetDouble("gross"));
                    if (ValidateEssential(m_sBnsCode))
                    {
                        sEss = sGross;
                        sNonEss = "";
                    }
                    else
                    {
                        sEss = "";
                        sNonEss = sGross;
                    }

                    //REM MCR 20150112
                    //strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevGrossCap + "|" + sEss + "|" + sNonEss;  // RMC 20111228 added display of prev gross/cap in app form
                    //this.axVSPrinter1.Table = string.Format("<800|<5030|<2170|<1500|<2000;{0}", strData);

                    if (pRec.GetString("bns_stat").Trim() == "NEW")
                        strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevGrossCap + "||";
                    else
                        strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + sPrevGrossCap + "|" + sEss + "|" + sNonEss;

                    //this.axVSPrinter1.Table = string.Format("<800|<3200|<2120|<1670|<1580|<1200|<1200;{0}", strData);
                    this.axVSPrinter1.Table = string.Format("<800|<5320|<1670|<1580|<1200|<1200;{0}", strData);

                }
            }
            pRec.Close();

            this.axVSPrinter1.CurrentY = 13360; //13600
            this.axVSPrinter1.MarginLeft = 3700;
            string sFrom = "";
            string sTo = "";
            pRec.Query = "select tax_year from business_que where bin = '" + m_sBIN + "'";
            if (pRec.Execute())
                if (pRec.Read())
                {
                    pRec.Close();
                    pRec.Query = "select max(tax_year) from pay_hist where bin = '" + m_sBIN + "' and bns_stat <> 'RET'"; // MCR 20150129
                    if (pRec.Execute())
                        if (pRec.Read())
                        {
                            sFrom = pRec.GetString(0);
                            sTo = string.Format("{0}", AppSettingsManager.GetSystemDate().Year - 1);
                        }
                }
                else
                {
                    pRec.Close();
                    pRec.Query = "select tax_year from businesses where bin = '" + m_sBIN + "'";
                    if (pRec.Execute())
                        if (pRec.Read())
                        {
                            pRec.Close();
                            pRec.Query = @"select min(tax_year), max(tax_year) from pay_hist where or_no in
(select distinct or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "')";
                            if (pRec.Execute())
                                if (pRec.Read())
                                {
                                    sFrom = pRec.GetString(0);
                                    sTo = pRec.GetString(1);
                                }
                        }
                }
            pRec.Close();

            if (m_sApplType == "REN")
            {
                string stmpStat = "";
                string stmpDate = "";

                AppSettingsManager.GetPrevBnsStat(m_sBIN, out stmpStat, out stmpDate);
                if (stmpStat == "REN")
                    strData = "January 1, " + sFrom + "|" + "December 31, " + sTo;
                else
                    strData = Convert.ToDateTime(stmpDate).ToString("MMMM dd, yyyy") + "|" + "December 31, " + Convert.ToDateTime(stmpDate).ToString("yyyy");
                this.axVSPrinter1.Table = string.Format("<3000|<2000;{0}", strData);
            }

            this.axVSPrinter1.CurrentY = 14500; //15151
            strData = "|" + StringUtilities.NumberWording.ToOrdinalFigure(Convert.ToDateTime(sAppDate).Day).ToString() + "|" + Convert.ToDateTime(sAppDate).ToString("MMMM") + "|" + Convert.ToDateTime(sAppDate).ToString("yy");
            this.axVSPrinter1.Table = string.Format("<500|<1000|<2200|<2000;{0}", strData);

            this.axVSPrinter1.CurrentY = 14800; //15151
            strData = sCTCNo + "|" + sCTCDate;
            this.axVSPrinter1.Table = string.Format("<3000|<2000;{0}", strData);


            this.axVSPrinter1.CurrentY = 17609; //17849
            this.axVSPrinter1.MarginLeft = 1300;
            //this.axVSPrinter1.Table = string.Format("=4200|=3450|=4000;{0}|{1}|{2}", AppSettingsManager.SystemUser.UserName, AppSettingsManager.GetConfigValue("03"), AppSettingsManager.GetConfigValue("05"));
            this.axVSPrinter1.Table = string.Format("=4200;{0}", AppSettingsManager.SystemUser.UserName); //MCR 20150113

            //(s) MCR 20150106 Next Page
            String sBrgyClearanceDate = "";
            String sBrgyClearanceBy = "";
            String sZoningClearanceDate = "";
            String sZoningClearanceBy = "";
            String sSanitaryClearanceDate = "";
            String sSanitaryClearanceBy = "";
            String sOccClearanceDate = "";
            String sOccClearanceBy = "";
            String sAnnualClearanceDate = "";
            String sAnnualClearanceBy = "";
            String sFireClearanceDate = "";
            String sFireClearanceBy = "";
            String sOtherDate = "";
            String sOtherBy = "";

            pRec.Query = "select * from Permit_Type where bin = '" + m_sBIN + "' and current_year = '" + sTaxYear + "'";
            if (pRec.Execute())
                while (pRec.Read())
                {
                    if (pRec.GetString("perm_type") == "Sanitary")
                    {
                        sSanitaryClearanceDate = pRec.GetString("issued_date");
                        sSanitaryClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "ZONING")
                    {
                        sZoningClearanceDate = pRec.GetString("issued_date");
                        sZoningClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "Annual Inspection" || pRec.GetString("perm_type") == "ANNUAL")
                    {
                        sAnnualClearanceDate = pRec.GetString("issued_date");
                        sAnnualClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "WORKING")
                    {
                        sOtherDate = pRec.GetString("issued_date");
                        sOtherBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                }
            pRec.Close();

            this.axVSPrinter1.NewPage();
            //this.axVSPrinter1.MarginLeft = 7980;
            this.axVSPrinter1.MarginLeft = 6150;
            this.axVSPrinter1.CurrentY = 1200;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sBrgyClearanceDate, sBrgyClearanceBy);
            this.axVSPrinter1.CurrentY = 1500;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sZoningClearanceDate, sZoningClearanceBy);
            this.axVSPrinter1.CurrentY = 1780;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sSanitaryClearanceDate, sSanitaryClearanceBy);
            this.axVSPrinter1.CurrentY = 2070;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sOccClearanceDate, sOccClearanceBy);
            this.axVSPrinter1.CurrentY = 2390;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sAnnualClearanceDate, sAnnualClearanceBy);
            this.axVSPrinter1.CurrentY = 2680;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sFireClearanceDate, sFireClearanceBy);
            // (e) MCR 20150106 Next Page
            this.axVSPrinter1.MarginLeft = 3300;
            this.axVSPrinter1.CurrentY = 3000; //MCR 20150114
            if (sOtherBy.Trim() != "")
                this.axVSPrinter1.Table = string.Format("<2900|<3000|<2400;Working Permit / Mayor's Office|{0}|{1}", sOtherDate, sOtherBy);
        }
        private static string GetOfficeType(string sBIN) {// GMC 20150813 Get office type of the business(s)
            string sOfcType = string.Empty;

            OracleResultSet pType = new OracleResultSet();
            pType.Query = "select ofc_type from consol_gr where bin='"+ sBIN +"'";
            
            if(pType.Execute())
                if(pType.Read())
                    sOfcType = pType.GetString("ofc_type");
                
            return sOfcType;
        }// GMC 20150813 Get office type of the business(e)
        private void PrintAppForm_Tumauini(string sAppDate, string sDTINo, string sDTIDate, string sOrgKind, string sCTCNo, string sTIN, string sTINDate, string sSSS, string sSSSDate, string sPhilhealth, string sPhilhealthDate, string sCTCDate, string sBnsPlate, string sAppNo)
        {
            string strData = string.Empty;
            string strOwner = string.Empty;
            m_sOfcType = GetOfficeType(m_sBIN);

            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)10;
            //GMC 20150811 Shoot Data According To The Application For Business Permit(s)
            //AppNo / Business Plate
            if (m_sAppNo != "" && m_sBussPlate != "")
            {
                sAppNo = m_sAppNo;
                sBnsPlate = m_sBussPlate;
                this.axVSPrinter1.CurrentY = 1881;
                this.axVSPrinter1.MarginLeft = 2317;
                this.axVSPrinter1.Table = string.Format("<4764|<4763;{0}|{1}", sAppNo, sBnsPlate);
            }
            //ApplType
            if(m_sApplType.ToUpper().Equals("NEW")){
                this.axVSPrinter1.CurrentY = 2213;
                this.axVSPrinter1.MarginLeft = 3036;
                //this.axVSPrinter1.Table = string.Format("<2899;{0}", "~");
                this.axVSPrinter1.Table = string.Format("<2899;{0}", "x");  // RMC 20150811 QA application form printing
            }else if(m_sApplType.ToUpper().Equals("REN")){
                this.axVSPrinter1.CurrentY = 2213;
                this.axVSPrinter1.MarginLeft = 5935;
                //this.axVSPrinter1.Table = string.Format("<2865;{0}", "~");
                this.axVSPrinter1.Table = string.Format("<2865;{0}", "x");  // RMC 20150811 QA application form printing
            }else if (m_sApplType.ToUpper().Equals("ADD")){
                this.axVSPrinter1.CurrentY = 2213;
                this.axVSPrinter1.MarginLeft = 8800;
                //this.axVSPrinter1.Table = string.Format("<2899;{0}", "~");
                this.axVSPrinter1.Table = string.Format("<2899;{0}", "x");  // RMC 20150811 QA application form printing
            }
            //Transfer Own or Loc
            if (m_sOldOwnCode != "" && m_sOldLocation == "")
            {
                this.axVSPrinter1.CurrentY = 2508;
                this.axVSPrinter1.MarginLeft = 3035;
                //this.axVSPrinter1.Table = string.Format("<2899;{0}", "~");
                this.axVSPrinter1.Table = string.Format("<2899;{0}", "x");  // RMC 20150811 QA application form printing
                this.axVSPrinter1.CurrentY = 2741;
                this.axVSPrinter1.MarginLeft = 2889;
                this.axVSPrinter1.Table = string.Format("<1983|<3108;{0}|{1}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), AppSettingsManager.GetBnsOwner(m_sNewOwnCode));
            }
            else if (m_sOldOwnCode == "" && m_sOldLocation != "")
            {
                this.axVSPrinter1.CurrentY = 2508;
                this.axVSPrinter1.MarginLeft = 5934;
                //this.axVSPrinter1.Table = string.Format("<2899;{0}", "~");
                this.axVSPrinter1.Table = string.Format("<2899;{0}", "x");  // RMC 20150811 QA application form printing
                this.axVSPrinter1.CurrentY = 2741;
                this.axVSPrinter1.MarginLeft = 7980;
                this.axVSPrinter1.Table = string.Format("<1983|<3108;{0}|{1}", m_sOldLocation, m_sNewLocation);
            }
            //DateApp
            if (m_sAppDate != "")
                sAppDate = m_sAppDate;
            this.axVSPrinter1.CurrentY = 3040;
            this.axVSPrinter1.MarginLeft = 2317;
            this.axVSPrinter1.Table = string.Format("<6138|<3000;{0}|{1}", sAppDate, sDTINo);
            //BIN
            this.axVSPrinter1.CurrentY = 3300;
            this.axVSPrinter1.MarginLeft = 2317;
            this.axVSPrinter1.Table = string.Format("<6351|<3000;{0}|{1}", m_sBIN, sDTIDate);
            //Type of Org
            if(sOrgKind=="SINGLE PROPRIETORSHIP"){
                this.axVSPrinter1.CurrentY = 3649;
                this.axVSPrinter1.MarginLeft = 2333;
                //this.axVSPrinter1.Table = string.Format("<1907;{0}","~");
                this.axVSPrinter1.Table = string.Format("<1907;{0}", "x");  // RMC 20150811 QA application form printing
            }else if(sOrgKind=="PARTNERSHIP"){
                this.axVSPrinter1.CurrentY = 3649;
                this.axVSPrinter1.MarginLeft = 4240;
                //this.axVSPrinter1.Table = string.Format("<1150;{0}","~");
                this.axVSPrinter1.Table = string.Format("<1150;{0}", "x");  // RMC 20150811 QA application form printing
            }else if(sOrgKind=="CORPORATION"){
                this.axVSPrinter1.CurrentY = 3649;
                this.axVSPrinter1.MarginLeft = 5390;
                //this.axVSPrinter1.Table = string.Format("<1165;{0}","~");
                this.axVSPrinter1.Table = string.Format("<1165;{0}", "x");  // RMC 20150811 QA application form printing
            }else if (sOrgKind == "COOPERATIVE"){
                this.axVSPrinter1.CurrentY = 3649;
                this.axVSPrinter1.MarginLeft = 6555;
                //this.axVSPrinter1.Table = string.Format("<1165;{0}","~");
                this.axVSPrinter1.Table = string.Format("<1165;{0}", "x");  // RMC 20150811 QA application form printing
            }
            this.axVSPrinter1.CurrentY = 3617;
            this.axVSPrinter1.MarginLeft = 8727;
            this.axVSPrinter1.Table = string.Format("<2200|<1800;{0}|{1}",sCTCNo,sDTIDate);
            //Are you enjoying tax incentive from any government entity?
            this.axVSPrinter1.CurrentY = 3924;
            this.axVSPrinter1.MarginLeft = 4969;
            //this.axVSPrinter1.Table = string.Format("<470|<2541|<2392|<2000;{0}|{1}|{2}|{3}","~","~","",m_sOwnNationality);
            this.axVSPrinter1.Table = string.Format("<470|<2541|<2392|<2000;{0}|{1}|{2}|{3}", "", "", "", m_sOwnNationality); // RMC 20150811 QA application form printing
            //Name of payer
            this.axVSPrinter1.CurrentY = 4203;
            this.axVSPrinter1.MarginLeft = 2640;
            this.axVSPrinter1.Table = string.Format("<1933|<2448|<1800|<1700;{0}|{1}|{2}|{3}",m_sOwnLn,m_sOwnFn,m_sOwnMi,m_sBdate);

            if (m_sGender == "MALE"){
                this.axVSPrinter1.CurrentY = 4221;
                this.axVSPrinter1.MarginLeft = 10573;
                //this.axVSPrinter1.Table = string.Format("<500;{0}","~");
                this.axVSPrinter1.Table = string.Format("<500;{0}", "x");   // RMC 20150811 QA application form printing
            }
            else {
                this.axVSPrinter1.CurrentY = 4221;
                this.axVSPrinter1.MarginLeft = 11030;
                //this.axVSPrinter1.Table = string.Format("<500;{0}","~");
                this.axVSPrinter1.Table = string.Format("<500;{0}", "x");   // RMC 20150811 QA application form printing
            }
            //TIN
            this.axVSPrinter1.CurrentY = 4460;
            this.axVSPrinter1.MarginLeft = 2297;
            this.axVSPrinter1.Table = string.Format("<5118|<2000;{0}|{1}", sTIN, sTINDate);
            //SSS
            this.axVSPrinter1.CurrentY = 4744;
            this.axVSPrinter1.MarginLeft = 2297;
            this.axVSPrinter1.Table = string.Format("<5118|<2000;{0}|{1}", sSSS, sSSSDate);
            //PHILHEALTH
            this.axVSPrinter1.CurrentY = 5028;
            this.axVSPrinter1.MarginLeft = 2297;
            this.axVSPrinter1.Table = string.Format("<5118|<2000;{0}|{1}", sPhilhealth, sPhilhealthDate);
            //Spouse
            if (m_sSpouse != "")
            {
                this.axVSPrinter1.CurrentY = 5307;
                this.axVSPrinter1.MarginLeft = 2345;
                this.axVSPrinter1.Table = string.Format("<10000;{0}", m_sSpouse);
            }
            //Business Name
            this.axVSPrinter1.CurrentY = 5600;
            this.axVSPrinter1.Table = string.Format("<10000;{0}", AppSettingsManager.GetBnsName(m_sBIN));
            //Tradename
            // GMC 20150813 Trade Name according either branch or main(s)
            OracleResultSet pMotherBIN = new OracleResultSet();
            pMotherBIN.Query = "select mother_bin from consol_gr where bin='"+ m_sBIN +"'";
            if (pMotherBIN.Execute())
                if (pMotherBIN.Read())
                    m_sMotherBIN = pMotherBIN.GetString("mother_bin");

            if(m_sOfcType != ""){
                this.axVSPrinter1.CurrentY = 5886;
                this.axVSPrinter1.MarginLeft = 2345;

                if (m_sBIN == m_sMotherBIN){
                    this.axVSPrinter1.Table = string.Format("<6000;{0}", AppSettingsManager.GetBnsName(m_sBIN));
                    this.axVSPrinter1.CurrentY = 5933;
                    this.axVSPrinter1.MarginLeft = 7375;
                    //this.axVSPrinter1.Table = string.Format("<400;{0}", "~");
                    this.axVSPrinter1.Table = string.Format("<400;{0}", "x");   // RMC 20150811 QA application form printing
                }else{
                    this.axVSPrinter1.Table = string.Format("<6000;{0}", AppSettingsManager.GetBnsName(m_sMotherBIN));
                    this.axVSPrinter1.CurrentY = 5933;
                    this.axVSPrinter1.MarginLeft = 8809;
                    //this.axVSPrinter1.Table = string.Format("<400;{0}", "~");
                    this.axVSPrinter1.Table = string.Format("<400;{0}", "x");   // RMC 20150811 QA application form printing
                }
            }
            // GMC 20150813 Trade Name according either branch or main(e)

            //Name of Pres
            if (sOrgKind == "CORPORATION")//GMC 20150813 Name of President if Corporation
            {
                this.axVSPrinter1.CurrentY = 6200;
                this.axVSPrinter1.MarginLeft = 4124;
                this.axVSPrinter1.Table = string.Format("<10000;{0}", m_sOwnFn+" "+m_sOwnMi+" "+m_sOwnLn); 
                //this.axVSPrinter1.Table = string.Format("<10000;{0}", "");  // RMC 20150811 QA application form printing
            }
            //Business Address House No.
            this.axVSPrinter1.CurrentY = 6767;
            this.axVSPrinter1.MarginLeft = 2718;
            this.axVSPrinter1.Table = string.Format("<5596|<3000;{0}|{1}",m_sBnsHse,m_sOwnHse);
            //Street
            this.axVSPrinter1.CurrentY = 7052;
            this.axVSPrinter1.MarginLeft = 1682;
            this.axVSPrinter1.Table = string.Format("<5531|<3000;{0}|{1}", m_sBnsStr, m_sOwnStr);
            //Subdivision
            this.axVSPrinter1.CurrentY = 7338;
            this.axVSPrinter1.MarginLeft = 2040;
            this.axVSPrinter1.Table = string.Format("<5620|<3000;{0}|{1}", m_sBnsHse, m_sOwnStr);
            //Barangay/Municipality
            this.axVSPrinter1.CurrentY = 7624;
            this.axVSPrinter1.MarginLeft = 1902;
            this.axVSPrinter1.Table = string.Format("<2278|<3262|<3051|<3000;{0}|{1}|{2}|{3}", m_sBnsBrgy, m_sBnsMun,m_sOwnBrgy,m_sOwnMun);
            //Provine/ Tel No.
            this.axVSPrinter1.CurrentY = 7909;
            this.axVSPrinter1.MarginLeft = 1902;
            this.axVSPrinter1.Table = string.Format("<2278|<3262|<3051|<3000;{0}|{1}|{2}|{3}", m_sBnsProv, m_sTelNo,m_sOwnProv,m_sOwnTelNo);
            //Mobile No. / Email Address
            this.axVSPrinter1.CurrentY = 8205;
            this.axVSPrinter1.MarginLeft = 3305;
            this.axVSPrinter1.Table = string.Format("<4197|<3000;{0}|{1}",m_sOwnTelNo,m_sOwnEmailAdd);
            //Business Area
            this.axVSPrinter1.CurrentY = 8465;
            this.axVSPrinter1.MarginLeft = 2253;
            this.axVSPrinter1.Table = string.Format("<3867|<1114|<928|<3265|<500;{0}|{1}|{2}|{3}|{4}",m_sArea,m_sEmployee,m_sNoMale,m_sNoFemale,m_sNoGender);
            //If Rented
            if (sLessor != "")
            {
                GetOwnerDetails(sLessor);
                this.axVSPrinter1.CurrentY = 8740;
                this.axVSPrinter1.MarginLeft = 6165;
                this.axVSPrinter1.Table = string.Format("<4696|<3000;{0}|{1}", AppSettingsManager.GetBnsOwner(sLessor), sRent);
                //LessorAddress HouseNo.
                this.axVSPrinter1.CurrentY = 9033;
                this.axVSPrinter1.MarginLeft = 3389;
                this.axVSPrinter1.Table = string.Format("<1970|<3000;{0}|{1}", m_sOwnHse, m_sOwnStr);
                //Lessor Subdivision/brgy/contact person
                this.axVSPrinter1.CurrentY = 9315;
                this.axVSPrinter1.MarginLeft = 1986;
                this.axVSPrinter1.Table = string.Format("<3667|<4340|<3000;{0}|{1}|{2}", " ", m_sOwnBrgy, AppSettingsManager.GetBnsOwner(sLessor));
                //Lessor Mun/Prov/contact person Telno.
                this.axVSPrinter1.CurrentY = 9612;
                this.axVSPrinter1.MarginLeft = 1986;
                this.axVSPrinter1.Table = string.Format("<3667|<3832|<3000;{0}|{1}|{2}", m_sOwnMun, m_sOwnProv, m_sOwnTelNo);
                //Lessor Telephone/email add/contact person email add
                this.axVSPrinter1.CurrentY = 9897;
                this.axVSPrinter1.MarginLeft = 1986;
                this.axVSPrinter1.Table = string.Format("<3941|<3913|<3000;{0}|{1}|{2}", m_sOwnTelNo, m_sOwnEmailAdd, m_sOwnEmailAdd);
            }
            //Business Code / Business Activity
            this.axVSPrinter1.CurrentY = 10888;
            this.axVSPrinter1.MarginLeft = 302;
            this.axVSPrinter1.FontSize = (float)8;
            int iAssetSize = 0;
            string sEss = "";
            string sNonEss = "";
            sGross = AppSettingsManager.GetCapitalGross(m_sBIN, m_sBnsCode, sTaxYear, m_sApplType);

            if (ValidateEssential(m_sBnsCode)){
                sEss = Convert.ToDouble(sGross).ToString("#,##0.00");
                sNonEss = "";
            }
            else{
                sEss = "";
                sNonEss = Convert.ToDouble(sGross).ToString("#,##0.00");
            }
            OracleResultSet pRec = new OracleResultSet();
            int iGross = 0;
            pRec.Query = @"select * from bill_gross_info where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "'";
            if (pRec.Execute())
                if (pRec.Read())
                    if (m_sApplType == "NEW")
                        iGross = pRec.GetInt("CAPITAL");
                    else
                        iGross = pRec.GetInt("GROSS");
            pRec.Close();

            if (m_sApplType == "NEW")
            {
                this.axVSPrinter1.Table = string.Format("<1380|<3705|<1046|<1729|<1573|<1172|<1172;{0}|{1}|{2}|{3}|{4}|{5}|{6}", m_sBnsCode, AppSettingsManager.GetBnsDesc(m_sBnsCode), "", iGross.ToString("#,##0.00"), "", "", "");
            }
            else
            {
                pRec.Query = @"select DC.default_desc,OI.data from other_info OI left join default_code DC on dc.default_code = oi.default_code
where DC.rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' and bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + @"' and (dc.default_desc = 'ASSET SIZE')";
                if (pRec.Execute())
                    if (pRec.Read())
                        iAssetSize = pRec.GetInt(1);
                pRec.Close();
                this.axVSPrinter1.Table = string.Format("<1380|<3705|<1046|<1729|<1573|<1172|<1172;{0}|{1}|{2}|{3}|{4}|{5}|{6}", m_sBnsCode, AppSettingsManager.GetBnsDesc(m_sBnsCode), iAssetSize.ToString("#,##0.00"), "", iGross.ToString("#,##0.00"), sEss, sNonEss);
            }
            pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' order by bns_code_main";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sPrevGrossCap = "";
                    m_sBnsCode = pRec.GetString("bns_code_main");
                    if (pRec.GetString("bns_stat").Trim() == "NEW")
                        sPrevGrossCap = string.Format("{0:#,##0.00}", pRec.GetDouble("capital"));
                    else
                        sGross = string.Format("{0:#,##0.00}", pRec.GetDouble("gross"));
                    if (ValidateEssential(m_sBnsCode))
                    {
                        sEss = sGross;
                        sNonEss = "";
                    }
                    else
                    {
                        sEss = "";
                        sNonEss = sGross;
                    }

                    if (pRec.GetString("bns_stat").Trim() == "NEW")
                        this.axVSPrinter1.Table = string.Format("<1380|<3705|<1046|<1729|<1573|<1172|<1172;{0}|{1}|{2}|{3}|{4}|{5}|{6}", m_sBnsCode, AppSettingsManager.GetBnsDesc(m_sBnsCode), "", iGross.ToString("#,##0.00"), "", "", "");
                    else
                        this.axVSPrinter1.Table = string.Format("<1380|<3705|<1046|<1729|<1573|<1172|<1172;{0}|{1}|{2}|{3}|{4}|{5}|{6}", m_sBnsCode, AppSettingsManager.GetBnsDesc(m_sBnsCode), iAssetSize.ToString("#,##0.00"), "", iGross.ToString("#,##0.00"), sEss, sNonEss);
                }
            }
            pRec.Close();
            
            //TAXPAYER'S SWORN STATEMENT
            //From / To
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.CurrentY = 12853;
            this.axVSPrinter1.MarginLeft = 3762;

            string stmpStat = "";
            string stmpDate = "";
            string sFrom = "";
            string sTo = "";
            pRec.Query = "select tax_year from business_que where bin = '" + m_sBIN + "'";

            if (pRec.Execute())
                if (pRec.Read())
                {
                    pRec.Close();
                    pRec.Query = "select max(tax_year) from pay_hist where bin = '" + m_sBIN + "' and bns_stat <> 'RET'"; // MCR 20150129
                    if (pRec.Execute())
                        if (pRec.Read())
                        {
                            sFrom = pRec.GetString(0);
                            sTo = string.Format("{0}", AppSettingsManager.GetSystemDate().Year - 1);
                        }
                }
                else
                {
                    pRec.Close();
                    pRec.Query = "select tax_year from businesses where bin = '" + m_sBIN + "'";
                    if (pRec.Execute())
                        if (pRec.Read())
                        {
                            pRec.Close();
                            pRec.Query = @"select min(tax_year), max(tax_year) from pay_hist where or_no in
                            (select distinct or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "')";
                            if (pRec.Execute())
                                if (pRec.Read())
                                {
                                    sFrom = pRec.GetString(0);
                                    sTo = pRec.GetString(1);
                                }
                        }
                }
            pRec.Close();

            #region comments
            /** //JARS 20170809 REM NOT NEEDED IN MALOLOS APP FORM
            if (m_sApplType == "REN")
            {
                AppSettingsManager.GetPrevBnsStat(m_sBIN, out stmpStat, out stmpDate);
                // RMC 20150727 corrections in re-print of Application form error (s)
                if (stmpDate.Trim() == "")
                    stmpDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                // RMC 20150727 corrections in re-print of Application form error (e)

                if (stmpStat == "REN")
                    this.axVSPrinter1.Table = string.Format("<2843|<2000;{0}|{1}", "January 1, " + sFrom, "December 31, " + sTo);//GMC 20150812 QA Application form printing. 
                //strData = "January 1, " + sFrom + "|" + "December 31, " + sTo;
                else
                    this.axVSPrinter1.Table = string.Format("<2843|<2000;{0}|{1}", Convert.ToDateTime(stmpDate).ToString("MMMM dd, yyyy"), "December 31, " + Convert.ToDateTime(stmpDate).ToString("yyyy"));//GMC 20150812 QA Application form printing. 
                //strData = Convert.ToDateTime(stmpDate).ToString("MMMM dd, yyyy") + "|" + "December 31, " + Convert.ToDateTime(stmpDate).ToString("yyyy");
            }
            else 
            { 
                //GMC 20150812 If application type is New(s)
                AppSettingsManager.GetPrevBnsStat(m_sBIN, out stmpStat, out stmpDate);
                // RMC 20150727 corrections in re-print of Application form error (s)
                if (stmpDate.Trim() == "")
                    stmpDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                // RMC 20150727 corrections in re-print of Application form error (e)
                this.axVSPrinter1.Table = string.Format("<2843|<2000;{0}|{1}", Convert.ToDateTime(stmpDate).ToString("MMMM dd, yyyy"), "December 31, " + Convert.ToDateTime(stmpDate).ToString("yyyy"));//GMC 20150812 QA Application form printing. 
            }
            //GMC 20150812 If application type is New(e)
            **/
            #endregion

            //Owner
            this.axVSPrinter1.CurrentY = 13320;
            this.axVSPrinter1.MarginLeft = 8115;
            this.axVSPrinter1.Table = string.Format("<2500;{0}", m_sOwnFn + " " + m_sOwnMi + " " + m_sOwnLn);
            //This Day 
            this.axVSPrinter1.CurrentY = 14067;
            this.axVSPrinter1.MarginLeft = 3854;
            //if(sAppDate == "") //JARS 20170725 //JARS 20170809 REM NOT NEEDED IN MALOLOS APP FORM
            //{
            //    sAppDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
            //}
            this.axVSPrinter1.Table = string.Format("<1150|<2099|<554|<1200;{0}|{1}|{2}|{3}", StringUtilities.NumberWording.ToOrdinalFigure(Convert.ToDateTime(sAppDate).Day).ToString(), Convert.ToDateTime(sAppDate).ToString("MMMM"), Convert.ToDateTime(sAppDate).ToString("yy"),"OFFICE");
            //CTC
            this.axVSPrinter1.CurrentY = 14320;
            this.axVSPrinter1.MarginLeft = 3593;
            this.axVSPrinter1.Table = string.Format("<3083|<2800;{0}|{1}", sCTCNo, sCTCDate);
            //Administering
            this.axVSPrinter1.CurrentY = 14830;
            this.axVSPrinter1.MarginLeft = 2821;
            //this.axVSPrinter1.Table = string.Format("<4899|<2000;{0}|{1}", "DONALYN BARTOLOME", "ADMIN");
            this.axVSPrinter1.Table = string.Format("<4899|<2000;{0}|{1}", "", ""); // RMC 20150811 QA application form printing
            //Printed By
            this.axVSPrinter1.CurrentY = 15420;
            this.axVSPrinter1.MarginLeft = 1986;
            this.axVSPrinter1.Table = string.Format("<2843;{0}", AppSettingsManager.SystemUser.UserName);
            //Verification of Document
            String sBrgyClearanceDate = "";
            String sBrgyClearanceBy = "";
            String sZoningClearanceDate = "";
            String sZoningClearanceBy = "";
            String sSanitaryClearanceDate = "";
            String sSanitaryClearanceBy = "";
            String sOccClearanceDate = "";
            String sOccClearanceBy = "";
            String sAnnualClearanceDate = "";
            String sAnnualClearanceBy = "";
            String sFireClearanceDate = "";
            String sFireClearanceBy = "";
            String sOtherDate = "";
            String sOtherBy = "";

            pRec.Query = "select * from Permit_Type where bin = '" + m_sBIN + "' and current_year = '" + sTaxYear + "'";
            if (pRec.Execute())
                while (pRec.Read())
                {
                    if (pRec.GetString("perm_type") == "Sanitary")
                    {
                        sSanitaryClearanceDate = pRec.GetString("issued_date");
                        sSanitaryClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "ZONING")
                    {
                        sZoningClearanceDate = pRec.GetString("issued_date");
                        sZoningClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "Annual Inspection" || pRec.GetString("perm_type") == "ANNUAL")
                    {
                        sAnnualClearanceDate = pRec.GetString("issued_date");
                        sAnnualClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "WORKING")
                    {
                        sOtherDate = pRec.GetString("issued_date");
                        sOtherBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                }
            pRec.Close();

            //BRGY
            this.axVSPrinter1.CurrentY = 16433;
            this.axVSPrinter1.MarginLeft = 6279;
            this.axVSPrinter1.Table = string.Format("<2925|<2500;{0}|{1}", sBrgyClearanceDate, sBrgyClearanceBy);
            //Zoning
            this.axVSPrinter1.CurrentY = 16719;
            this.axVSPrinter1.MarginLeft = 6279;
            this.axVSPrinter1.Table = string.Format("<2925|<2800;{0}|{1}", sZoningClearanceDate, sZoningClearanceBy);
            //Sanitary
            this.axVSPrinter1.CurrentY = 17004;
            this.axVSPrinter1.MarginLeft = 6279;
            this.axVSPrinter1.Table = string.Format("<2925|<2500;{0}|{1}", sSanitaryClearanceDate, sSanitaryClearanceBy);
            //Occupation
            this.axVSPrinter1.CurrentY = 17289;
            this.axVSPrinter1.MarginLeft = 6279;
            this.axVSPrinter1.Table = string.Format("<2925|<2500;{0}|{1}", sOccClearanceDate, sOccClearanceBy);
            //Fire
            this.axVSPrinter1.CurrentY = 17575;
            this.axVSPrinter1.MarginLeft = 6279;
            this.axVSPrinter1.Table = string.Format("<2925|<2500;{0}|{1}", sFireClearanceDate, sFireClearanceBy);
            //Others
            if (sOtherBy.Trim() != "")
            {
                this.axVSPrinter1.CurrentY = 17860;
                this.axVSPrinter1.MarginLeft = 3286;
                this.axVSPrinter1.Table = string.Format("<2993|<2925|<2500;{0}|{1}|{2}","Working Permit/Mayor's Office", sOtherDate, sOtherBy);
            }
            //GMC 20150811 Shoot Data According To The Application For Business Permit(e)
            #region comments
            /*
            //this.axVSPrinter1.CurrentY = 2610; //2890
            //strData = "x";
            //this.axVSPrinter1.Table = string.Format("<2000;{0}", strData);

            //this.axVSPrinter1.MarginLeft = 2945;
            //this.axVSPrinter1.CurrentY = 2905; //3235
            //From - to Transfer 
            
            if (m_sOldOwnCode != "" && m_sOldLocation != "")
            {
                this.axVSPrinter1.Table = "<2875|<2520;x|x";
                this.axVSPrinter1.MarginLeft = 2650;
                this.axVSPrinter1.Table = string.Format("<5200|<5000;{0}|{1}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), m_sOldLocation);
                //this.axVSPrinter1.Table = string.Format("<600|<2500|<2600|<3500|<3500;|{0}|{1}|{2}|{3}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), AppSettingsManager.GetBnsOwner(m_sNewOwnCode), m_sOldLocation, m_sNewLocation);
            }
            else if (m_sOldOwnCode != "" && m_sOldLocation == "")
            {
                this.axVSPrinter1.Table = "<2875|<2520;x|";
                this.axVSPrinter1.MarginLeft = 2650;
                this.axVSPrinter1.Table = string.Format("<5200;{0}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode));
                //this.axVSPrinter1.Table = string.Format("<600|<2500|<2600|<3500|<3500;|{0}|{1}|{2}|{3}", AppSettingsManager.GetBnsOwner(m_sOldOwnCode), AppSettingsManager.GetBnsOwner(m_sNewOwnCode), " ", " ");
            }
            else if (m_sOldOwnCode == "" && m_sOldLocation != "")
            {
                this.axVSPrinter1.Table = "<2875|<2520;|x";
                this.axVSPrinter1.MarginLeft = 2650;
                this.axVSPrinter1.Table = string.Format("<5200|<5000;|{0}", m_sOldLocation);
                //this.axVSPrinter1.Table = string.Format("<600|<2500|<2600|<3500|<3500;|{0}|{1}|{2}|{3}", " ", " ", m_sOldLocation, m_sNewLocation);
            }
            else
            {
                this.axVSPrinter1.Table = "<2870|<2520;|";//2720
                this.axVSPrinter1.MarginLeft = 2650;
                this.axVSPrinter1.Table = string.Format("<5200;{0}", " ");
            }

            this.axVSPrinter1.CurrentY = 3410; //3780
            this.axVSPrinter1.MarginLeft = 2370;

            if (m_sAppDate != "") //MCR 20150108
                sAppDate = m_sAppDate;
            this.axVSPrinter1.Table = string.Format("<2000|^2150|^2100|<2000;{0}|||{1}", sAppDate, sDTINo);
            this.axVSPrinter1.Table = string.Format("<2000|^2150|^2100|<2000;{0}|||{1}", m_sBIN, sDTIDate);

            this.axVSPrinter1.CurrentY = 4040; //4310
            this.axVSPrinter1.MarginLeft = 2240;
            if (sOrgKind == "SINGLE PROPRIETORSHIP")
                strData = "x|||||";
            else if (sOrgKind == "PARTNERSHIP")
                strData = "|x||||";
            else if (sOrgKind == "CORPORATION")
                strData = "||x|||";
            else
                strData = "|||x||";
            strData += sCTCNo + "|" + sCTCDate;
            this.axVSPrinter1.Table = string.Format("<1900|<1150|<1160|<1000|<1000|<2450|<1000;{0}", strData);

            this.axVSPrinter1.CurrentY = 4607; //4887
            this.axVSPrinter1.MarginLeft = 2580;
            if (m_sOwnFn.Trim() == "")
            {
                strOwner = m_sOwnLn;
                if (m_sBdate == "")
                {
                    strData = m_sOwnLn;
                    this.axVSPrinter1.Table = string.Format("<4600;{0}", strData);
                }
                else
                {
                    strData = m_sOwnLn + "|" + m_sBdate;
                    this.axVSPrinter1.Table = string.Format("<6500|<1000;{0}", strData);
                }
            }
            else
            {
                strOwner = m_sOwnLn + ", " + m_sOwnFn + " " + m_sOwnMi + ".";
                if (m_sBdate == "" && m_sGender == "")
                {
                    strData = m_sOwnLn + "|" + m_sOwnFn + "|" + m_sOwnMi + ".";
                    this.axVSPrinter1.Table = string.Format("<2200|<2300|1000;{0}", strData);
                }
                else if (m_sGender == "")
                {
                    strData = m_sOwnLn + "|" + m_sOwnFn + "|" + m_sOwnMi + "." + "|" + m_sBdate;
                    this.axVSPrinter1.Table = string.Format("<2200|<2300|<900|<1000;{0}", strData);
                }
                else
                {
                    //this.axVSPrinter1.CurrentY = 4886.4; //567
                    strData = m_sOwnLn + "|" + m_sOwnFn + "|" + m_sOwnMi + ".|" + m_sBdate + "||x";
                    if (m_sGender == "MALE")
                        this.axVSPrinter1.Table = string.Format("<2200|<2300|<1700|<1000|<750|<600;{0}", strData);
                    else //if (m_sGender == "FEMALE")
                        this.axVSPrinter1.Table = string.Format("<2200|<2300|<1700|<1000|<1170|<600;{0}", strData);
                }
            }
            
            this.axVSPrinter1.CurrentY = 4907; //5167
            //sTIN
            this.axVSPrinter1.Table = string.Format("<4800|<1150;{0}|{1}", sTIN, sTINDate);
            this.axVSPrinter1.CurrentY = 5183; //5443
            //SSS
            this.axVSPrinter1.Table = string.Format("<4800|<1150;{0}|{1}", sSSS, sSSSDate);
            this.axVSPrinter1.CurrentY = 5461; //5721
            //PHILHEALTH
            this.axVSPrinter1.Table = string.Format("<4800|<1150;{0}|{1}", sPhilhealth, sPhilhealthDate);

            this.axVSPrinter1.CurrentY = 5721; //6001
            if (m_sSpouse == "")
                this.axVSPrinter1.Table = string.Format("<4600;{0}", " ");
            else
                this.axVSPrinter1.Table = string.Format("<4600;{0}", m_sSpouse);

            this.axVSPrinter1.CurrentY = 6045; //6335
            this.axVSPrinter1.Table = string.Format("<9000;{0}", AppSettingsManager.GetBnsName(m_sBIN));

            //Trade name/Franchise
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.CurrentY = 6640; //6920
            //this.axVSPrinter1.Table = string.Format("<1800|<2100|<2300|<1000;|{0}", m_sOwnLn + "|" + m_sOwnFn + "|" + m_sOwnMi + "."); //REM MCR 20150127
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");

            this.axVSPrinter1.CurrentY = 7157; //7477
            this.axVSPrinter1.Table = string.Format("<5400|<5000;{0}|{1}", m_sBnsHse, m_sOwnHse);
            this.axVSPrinter1.MarginLeft = 1900;
            this.axVSPrinter1.CurrentY = 7497; //7787
            this.axVSPrinter1.Table = string.Format("<5300|<5000;{0}|{1}", m_sBnsStr, m_sOwnStr);
            //Subdivision
            this.axVSPrinter1.CurrentY = 7797; //8087
            this.axVSPrinter1.Table = string.Format("<5300|<5000;{0}|{1}", " ", " ");
            this.axVSPrinter1.Table = string.Format("<2400|<3000|<3100|<1950;{0}|{1}|{2}|{3}", m_sBnsBrgy, m_sBnsMun, m_sOwnBrgy, m_sOwnMun);
            this.axVSPrinter1.CurrentY = 8353; //8633
            this.axVSPrinter1.Table = string.Format("<2400|<3000|<3100|<1950;{0}|{1}|{2}|{3}", m_sBnsProv, m_sTelNo, m_sOwnProv, m_sOwnTelNo);
            this.axVSPrinter1.CurrentY = 8620; //8911
            this.axVSPrinter1.Table = string.Format("<1400|<4000|<3100;{0}|{1}|{2}", " ", m_sTelNo, m_sOwnEmailAdd);

            this.axVSPrinter1.MarginLeft = 2400;
            this.axVSPrinter1.CurrentY = 8850; //9187

            ////REM MCR 20150114 (s)
            //if (m_sLandPIN == "")
            //    this.axVSPrinter1.Table = string.Format("<4600;{0}", " ");
            //else
            //    this.axVSPrinter1.Table = string.Format("<4600;{0}", m_sLandPIN);
            ////REM MCR 20150114 (e)

            this.axVSPrinter1.Table = string.Format("<3750|<1050|<950|<1000;{0}|{1}|{2}|{3}", m_sArea, m_sEmployee, m_sNoMale, m_sNoFemale);
            if (sLessor != "")
            {
                GetOwnerDetails(sLessor);
                this.axVSPrinter1.MarginLeft = 2630;
                this.axVSPrinter1.CurrentY = 9160; //9420
                this.axVSPrinter1.Table = string.Format("<4100|<3600|<1300|<2000;|{0}||{1}", AppSettingsManager.GetBnsOwner(sLessor), sRent);
                this.axVSPrinter1.Table = string.Format("<1000|<2100|<5000;|{0}|{1}", m_sOwnHse, m_sOwnStr);
                this.axVSPrinter1.MarginLeft = 1950;
                this.axVSPrinter1.CurrentY = 9691; //9751
                this.axVSPrinter1.Table = string.Format("<3800|<5000;{0}|{1}", " ", m_sOwnBrgy);
                this.axVSPrinter1.CurrentY = 9961; //10081
                this.axVSPrinter1.Table = string.Format("<3800|<5000;{0}|{1}", m_sOwnMun, m_sOwnProv);
                this.axVSPrinter1.CurrentY = 10251; //10381
                this.axVSPrinter1.Table = string.Format("<4000|<5000;{0}|{1}", m_sOwnTelNo, m_sOwnEmailAdd);
            }
            else
            {
                this.axVSPrinter1.CurrentY = 10479; //10608
            }

            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.Table = string.Format("<2000;{0}", " ");
            this.axVSPrinter1.MarginLeft = 370;
            this.axVSPrinter1.CurrentY = 11260; //11600
            string sEss = "";
            string sNonEss = "";
            sGross = AppSettingsManager.GetCapitalGross(m_sBIN, m_sBnsCode, sTaxYear, m_sApplType);

            if (ValidateEssential(m_sBnsCode))
            {
                sEss = Convert.ToDouble(sGross).ToString("#,##0.00");
                sNonEss = "";
            }
            else
            {
                sEss = "";
                sNonEss = Convert.ToDouble(sGross).ToString("#,##0.00");
            }

            //MCR 20150112 (s)
            OracleResultSet pRec = new OracleResultSet();
            int iGross = 0;
            pRec.Query = @"select * from bill_gross_info where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "'";
            if (pRec.Execute())
                if (pRec.Read())
                    if (m_sApplType == "NEW")
                        iGross = pRec.GetInt("CAPITAL");
                    else
                        iGross = pRec.GetInt("GROSS");
            pRec.Close();

            if (m_sApplType == "NEW")
                strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + iGross.ToString("#,##0.00") + "|||";
            else
            {
                int iAssetSize = 0;
                pRec.Query = @"select DC.default_desc,OI.data from other_info OI left join default_code DC on dc.default_code = oi.default_code
where DC.rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' and bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + @"' and (dc.default_desc = 'ASSET SIZE')";
                if (pRec.Execute())
                    if (pRec.Read())
                        iAssetSize = pRec.GetInt(1);
                pRec.Close();

                //strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + iAssetSize.ToString("#,##0.00") + "|" + iGross.ToString("#,##0.00") + "|" + sEss + "|" + sNonEss;
                strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + iAssetSize.ToString("#,##0.00") + "|" + iGross.ToString("#,##0.00") + "|" + sEss + "|" + sNonEss; //MCR 20150128
            }
            //this.axVSPrinter1.Table = string.Format("<800|<3200|<2120|<1670|<1580|<1200|<1200;{0}", strData);
            this.axVSPrinter1.Table = string.Format("<800|<5320|<1670|<1580|<1200|<1200;{0}", strData); //MCR 20150128

            //MCR 20150112 (e)

            this.axVSPrinter1.CurrentY = 11439; //11879
            pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' order by bns_code_main";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sPrevGrossCap = "";
                    m_sBnsCode = pRec.GetString("bns_code_main");
                    if (pRec.GetString("bns_stat").Trim() == "NEW")
                        sPrevGrossCap = string.Format("{0:#,##0.00}", pRec.GetDouble("capital"));
                    else
                        sGross = string.Format("{0:#,##0.00}", pRec.GetDouble("gross"));
                    if (ValidateEssential(m_sBnsCode))
                    {
                        sEss = sGross;
                        sNonEss = "";
                    }
                    else
                    {
                        sEss = "";
                        sNonEss = sGross;
                    }

                    //REM MCR 20150112
                    //strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevGrossCap + "|" + sEss + "|" + sNonEss;  // RMC 20111228 added display of prev gross/cap in app form
                    //this.axVSPrinter1.Table = string.Format("<800|<5030|<2170|<1500|<2000;{0}", strData);

                    if (pRec.GetString("bns_stat").Trim() == "NEW")
                        strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevGrossCap + "||";
                    else
                        strData = m_sBnsCode + "|" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + sPrevGrossCap + "|" + sEss + "|" + sNonEss;

                    //this.axVSPrinter1.Table = string.Format("<800|<3200|<2120|<1670|<1580|<1200|<1200;{0}", strData);
                    this.axVSPrinter1.Table = string.Format("<800|<5320|<1670|<1580|<1200|<1200;{0}", strData);

                }
            }
            pRec.Close();

            this.axVSPrinter1.CurrentY = 13360; //13600
            this.axVSPrinter1.MarginLeft = 3700;
            string sFrom = "";
            string sTo = "";
            pRec.Query = "select tax_year from business_que where bin = '" + m_sBIN + "'";
            if (pRec.Execute())
                if (pRec.Read())
                {
                    pRec.Close();
                    pRec.Query = "select max(tax_year) from pay_hist where bin = '" + m_sBIN + "' and bns_stat <> 'RET'"; // MCR 20150129
                    if (pRec.Execute())
                        if (pRec.Read())
                        {
                            sFrom = pRec.GetString(0);
                            sTo = string.Format("{0}", AppSettingsManager.GetSystemDate().Year - 1);
                        }
                }
                else
                {
                    pRec.Close();
                    pRec.Query = "select tax_year from businesses where bin = '" + m_sBIN + "'";
                    if (pRec.Execute())
                        if (pRec.Read())
                        {
                            pRec.Close();
                            pRec.Query = @"select min(tax_year), max(tax_year) from pay_hist where or_no in
(select distinct or_no from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "')";
                            if (pRec.Execute())
                                if (pRec.Read())
                                {
                                    sFrom = pRec.GetString(0);
                                    sTo = pRec.GetString(1);
                                }
                        }
                }
            pRec.Close();

            if (m_sApplType == "REN")
            {
                string stmpStat = "";
                string stmpDate = "";

                AppSettingsManager.GetPrevBnsStat(m_sBIN, out stmpStat, out stmpDate);
                // RMC 20150727 corrections in re-print of Application form error (s)
                if (stmpDate.Trim() == "")
                    stmpDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                // RMC 20150727 corrections in re-print of Application form error (e)

                if (stmpStat == "REN")
                    strData = "January 1, " + sFrom + "|" + "December 31, " + sTo;
                else
                    strData = Convert.ToDateTime(stmpDate).ToString("MMMM dd, yyyy") + "|" + "December 31, " + Convert.ToDateTime(stmpDate).ToString("yyyy");
                this.axVSPrinter1.Table = string.Format("<3000|<2000;{0}", strData);
            }

            this.axVSPrinter1.CurrentY = 14500; //15151
            strData = "|" + StringUtilities.NumberWording.ToOrdinalFigure(Convert.ToDateTime(sAppDate).Day).ToString() + "|" + Convert.ToDateTime(sAppDate).ToString("MMMM") + "|" + Convert.ToDateTime(sAppDate).ToString("yy");
            this.axVSPrinter1.Table = string.Format("<500|<1000|<2200|<2000;{0}", strData);

            this.axVSPrinter1.CurrentY = 14800; //15151
            strData = sCTCNo + "|" + sCTCDate;
            this.axVSPrinter1.Table = string.Format("<3000|<2000;{0}", strData);


            this.axVSPrinter1.CurrentY = 17609; //17849
            this.axVSPrinter1.MarginLeft = 1300;
            //this.axVSPrinter1.Table = string.Format("=4200|=3450|=4000;{0}|{1}|{2}", AppSettingsManager.SystemUser.UserName, AppSettingsManager.GetConfigValue("03"), AppSettingsManager.GetConfigValue("05"));
            this.axVSPrinter1.Table = string.Format("=4200;{0}", AppSettingsManager.SystemUser.UserName); //MCR 20150113

            //(s) MCR 20150106 Next Page
            String sBrgyClearanceDate = "";
            String sBrgyClearanceBy = "";
            String sZoningClearanceDate = "";
            String sZoningClearanceBy = "";
            String sSanitaryClearanceDate = "";
            String sSanitaryClearanceBy = "";
            String sOccClearanceDate = "";
            String sOccClearanceBy = "";
            String sAnnualClearanceDate = "";
            String sAnnualClearanceBy = "";
            String sFireClearanceDate = "";
            String sFireClearanceBy = "";
            String sOtherDate = "";
            String sOtherBy = "";

            pRec.Query = "select * from Permit_Type where bin = '" + m_sBIN + "' and current_year = '" + sTaxYear + "'";
            if (pRec.Execute())
                while (pRec.Read())
                {
                    if (pRec.GetString("perm_type") == "Sanitary")
                    {
                        sSanitaryClearanceDate = pRec.GetString("issued_date");
                        sSanitaryClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "ZONING")
                    {
                        sZoningClearanceDate = pRec.GetString("issued_date");
                        sZoningClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "Annual Inspection" || pRec.GetString("perm_type") == "ANNUAL")
                    {
                        sAnnualClearanceDate = pRec.GetString("issued_date");
                        sAnnualClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "WORKING")
                    {
                        sOtherDate = pRec.GetString("issued_date");
                        sOtherBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                }
            pRec.Close();

            //this.axVSPrinter1.MarginLeft = 7980;
            this.axVSPrinter1.MarginLeft = 6150;
            //this.axVSPrinter1.CurrentY = 1200;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sBrgyClearanceDate, sBrgyClearanceBy);
            //this.axVSPrinter1.CurrentY = 1500;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sZoningClearanceDate, sZoningClearanceBy);
            //this.axVSPrinter1.CurrentY = 1780;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sSanitaryClearanceDate, sSanitaryClearanceBy);
            //this.axVSPrinter1.CurrentY = 2070;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sOccClearanceDate, sOccClearanceBy);
            //this.axVSPrinter1.CurrentY = 2390;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sAnnualClearanceDate, sAnnualClearanceBy);
            //this.axVSPrinter1.CurrentY = 2680;
            this.axVSPrinter1.Table = string.Format("<3000|<2400;{0}|{1}", sFireClearanceDate, sFireClearanceBy);
            // (e) MCR 20150106 Next Page
            this.axVSPrinter1.MarginLeft = 3300;
            //this.axVSPrinter1.CurrentY = 3000; //MCR 20150114
            if (sOtherBy.Trim() != "")
                this.axVSPrinter1.Table = string.Format("<2900|<3000|<2400;Working Permit / Mayor's Office|{0}|{1}", sOtherDate, sOtherBy);
        */
            #endregion
        }
        private string ConvertDayInOrdinalForm(string sDay)
        {
            // RMC 20150105 mods in permit printing
            string sLastNo = "";
            string sDayth = "";

            sLastNo = sDay.Substring(sDay.Length-1);

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

        private void CertStatus() //JARS 20170320
        {
            string sBnsName = "";
            string sBnsAddress = "";
            string sRecord = "";
            string sBnsStat = "";
            string sTaxYear = "";
            string sOwnName = "";
            string sDay = "";
            string sMonth = "";
            string sYear = "";
            int iDay;
            OracleResultSet pRec = new OracleResultSet();
            pRec.Query = "select own_code from businesses where bin = '"+ m_sBIN +"'";
            if(pRec.Execute())
            {
                if(pRec.Read())
                {
                    sOwnName = AppSettingsManager.GetBnsOwner(pRec.GetString("own_code"));
                }
            }
            pRec.Close();
            DateTime current = DateTime.Now;
            sDay = current.ToString("dd");
            iDay = Convert.ToInt32(sDay);
            sBnsName = AppSettingsManager.GetBnsName(m_sBIN);
            sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);

            this.axVSPrinter1.StartDoc();
            this.axVSPrinter1.FontName = "Arial";
            CreateHeader();
            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.Table = "<11000;";
            this.axVSPrinter1.Table = "<11000;";

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.Table = "^9000;   In response to your request letter, below is the status of the Taxpayer based on our records;";
            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";

            this.axVSPrinter1.FontSize = (float)14.0;
            this.axVSPrinter1.MarginLeft = 700;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^4000|^4500|^2000;NAME|ADDRESS|STATUS";
            this.axVSPrinter1.FontBold = false;

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from pay_hist where bin = '"+ m_sBIN +"'order by or_date desc";
            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    sBnsStat = pSet.GetString("bns_stat");
                    sTaxYear = pSet.GetString("tax_year");
                }
            }
            pSet.Close();
            if(sBnsStat == "REN")
            {
                sBnsStat = "Renewed";
            }
            else if (sBnsStat == "RET")
            {
                sBnsStat = "Retired";
            }
            else if(sBnsStat == "NEW")
            {
                sBnsStat = "New";
            }
            sRecord = sBnsStat + " " + sTaxYear;

            this.axVSPrinter1.Table = "<4000|<4500|^2000;" + sBnsName + "|" + sBnsAddress + "|" + sRecord;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            sDay = iDay.ToString();
            sDay = ConvertDayInOrdinalForm(sDay);
            sMonth = current.ToString("MMMM");
            sYear = current.ToString("yyyy");
            this.axVSPrinter1.FontSize = (float)12.0;
            this.axVSPrinter1.Table = "^9000; ";
            this.axVSPrinter1.Table = "^9000; ";
            this.axVSPrinter1.Table = "<10000;          This certification is being issued upon the request of " + sOwnName + ". For whatever legal intent & prupose it may serve.";
            this.axVSPrinter1.Table = "^9000; ";
            //this.axVSPrinter1.Table = "<10000;          Issued this " + sDay + " day of " + sMonth + ", " + sYear + " at City of Biñan, Laguna."; //JAV 20170804 replace City of Biñan, Laguna. to City of Malolos
            this.axVSPrinter1.Table = "<10000;          Issued this " + sDay + " day of " + sMonth + ", " + sYear + " at " + AppSettingsManager.GetConfigValue("02") + " " + AppSettingsManager.GetConfigValue("08") + "."; //JAV 20170804 replace City of Biñan, Laguna. to City of Malolos
            
            this.axVSPrinter1.Table = "^9000; ";
            this.axVSPrinter1.Table = "^9000; ";
            this.axVSPrinter1.Table = "^9000; ";
            this.axVSPrinter1.Table = "^9000; ";
            this.axVSPrinter1.Table = "^9000; ";

            this.axVSPrinter1.FontSize = (float)14.0;
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = ">8000;" + AppSettingsManager.GetConfigObject("03");
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontUnderline = false;
            this.axVSPrinter1.FontSize = (float)13.0;
            this.axVSPrinter1.Table = ">5950|<2700; |" + AppSettingsManager.GetConfigObject("41");
            this.axVSPrinter1.EndDoc();
        }

        private void PrintAppForm_Binan(string sAppDate, string sDTINo, string sDTIDate, string sOrgKind, string sCTCNo, string sTIN, string sTINDate, string sSSS, string sSSSDate, string sPhilhealth, string sPhilhealthDate, string sCTCDate, string sBnsPlate, string sAppNo)
        {
            // RMC 20161129 customized application form for Binan
            long lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)10.5;
            this.axVSPrinter1.MarginLeft = 500;

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.Table = "<11000;BPLO-001-0";
            this.axVSPrinter1.FontBold = true;
            
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.Table = "^11000;APPLICATION FORM FOR BUSINESS PERMIT";
            // RMC 20161228 adjust tax year in form for advance printing (s)
            if (m_bAdvancePrintForm)
            {
                int iTmpYear = Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) + 1;
                this.axVSPrinter1.Table = "^11000;TAX YEAR " + string.Format("{0:####}", iTmpYear);
            }// RMC 20161228 adjust tax year in form for advance printing (e)
            else
                this.axVSPrinter1.Table = "^11000;TAX YEAR " + AppSettingsManager.GetConfigValue("12");
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetConfigValue("01") + " OF " + AppSettingsManager.GetConfigValue("02");
            this.axVSPrinter1.Paragraph = "";
        
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            
            string strData = string.Empty;
            strData = "INSTRUCTIONS:\n";
            strData += "1. Provide accurate information and print legibly to avoid delays. Incomplete application form will be returned to the applicant.\n";
            strData += "2. Ensure that all documents attached to this form (if any) are complete and properly filled out.";
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);
      
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.Table = "<11000;I. APPLICATION SECTION";
            this.axVSPrinter1.MarginLeft = 800;
            this.axVSPrinter1.Table = "<5500|>5500;1. BASIC INFORMATION|BIN: " + m_sBIN;
            this.axVSPrinter1.FontSize = (float)10.5;
            string sLastPaidYear = string.Empty;
            string sLastPaidQtr = string.Empty;
            string sLastBnsStat = string.Empty;
            AppSettingsManager.GetLastPaymentInfo(m_sBIN, AppSettingsManager.GetLastPaymentInfo(m_sBIN, "OR"), out sLastPaidYear, out sLastPaidQtr, out sLastBnsStat);

            this.axVSPrinter1.FontBold = false;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);

            
            //this.axVSPrinter1.Table = "<1000|<1500|<500|<5000|<3000; ___ New| ___ Renewal||Mode of Payment: __ Anually  __ Semi-Annually  __ Quarterly|Last Payment: " + sLastPaidYear + " " + sLastPaidQtr + " " + sLastBnsStat;
            this.axVSPrinter1.Table = "<1000|<1500|<500|<5000|<3000; ___ New| ___ Renewal||Mode of Payment: __ Anually  __ Semi-Annually  __ Quarterly|";
            this.axVSPrinter1.CurrentY = lngY;

            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select * from business_que where bin = '" + m_sBIN + "' order by tax_year desc";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    m_sApplType = pRec.GetString("bns_stat");
                    sAppDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dt_applied"));
                }
                else
                {
                    pRec.Close();
                    pRec.Query = "select * from businesses where bin = '" + m_sBIN + "'";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            m_sApplType = "REN";
                            sAppDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                        }
                    }
                }
            }
            pRec.Close();

            if (m_sApplType == "NEW")
                this.axVSPrinter1.Table = "<1000|<1500|<500|<5000;  X |||";
            else
                this.axVSPrinter1.Table = "<1000|<1500|<500|<5000; |  X  ||";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.Table = "<5500|<5500;Date of Application:  " + sAppDate + "|Date Started:  "+m_sDateOperated;
            this.axVSPrinter1.Table = "<5500|<5500;TIN No.:  " + sTIN + "|DTI/SEC/CDA Registration No.:  " + sDTINo;

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            /*this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;Type of Business:| ___ Single| ___ Partnership| ___ Corporation| ___ Cooperative";
            this.axVSPrinter1.CurrentY = lngY;

            if(sOrgKind == "SINGLE PROPRIETORSHIP")
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;|  X  | | | ";
            else if (sOrgKind == "PARTNERSHIP")
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;| |  X  | | ";
            else if (sOrgKind == "CORPORATION")
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;| | |  X  | ";
            else 
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;| | | |  X  ";*/

            if (sOrgKind == "SINGLE PROPRIETORSHIP")
                this.axVSPrinter1.Table = "<11000;Type of Business: X Single     Partnership       Corporation       Cooperative";
            else if (sOrgKind == "PARTNERSHIP")
                this.axVSPrinter1.Table = "<11000;Type of Business:   Single   X Partnership       Corporation       Cooperative";
            else if (sOrgKind == "CORPORATION")
                this.axVSPrinter1.Table = "<11000;Type of Business:   Single     Partnership     X Corporation       Cooperative";
            else
                this.axVSPrinter1.Table = "<11000;Type of Business:   Single     Partnership       Corporation     X Cooperative";

            
            this.axVSPrinter1.Table = "<1250|<750|<3000|<3000|<3000;Amendment:|From| ___ Single| ___ Partnership| ___ Corporation";
            this.axVSPrinter1.Table = "<1250|<750|<3000|<3000|<3000;|To| ___ Single| ___ Partnership| ___ Corporation";

            this.axVSPrinter1.Table = "<11000;Are you enjoying tax incentive from any Government Entity? ___ Yes   ___ No  Please specify the entity?";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            
            this.axVSPrinter1.Table = "^11000;Name of Taxpayer/Registrant";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            //this.axVSPrinter1.Table = "<5500|<4000|<1500;Last Name:  " + m_sOwnLn + "|First Name:  " + m_sOwnFn + "|Middle Name:  " + m_sOwnMi;
            //this.axVSPrinter1.Table = "<11000;Business Name:  " + AppSettingsManager.GetBnsName(m_sBIN);

            this.axVSPrinter1.FontSize = (float)11;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "<1600|<3900|<1500|<2000|<1500|<500;Last Name: ||First Name: ||Middle Name: |";
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<1600|<3900|<1500|<2000|<1500|<500;|" + m_sOwnLn + "||" + m_sOwnFn + "||" + m_sOwnMi;
            this.axVSPrinter1.FontBold = false;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "<1600|<9400;Business Name: |";
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<1600|<9400;|" + AppSettingsManager.GetBnsName(m_sBIN);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)10.5;
            
            this.axVSPrinter1.Table = "<11000;Trade Name/ Franchise: ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 800;
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.Table = "<11000;2. OTHER INFORMATION";
            
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<11000;Note: For renewal applications, do not fill up this section unless certain information have changed.";
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)10.5;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.Table = "<11000;Business Address: " + m_sBnsAddress;

            this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Postal Code:||Email Address:|" + m_sEmail;
            this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Telephone No.:|" + m_sTelNo + "|Mobile No.:|";
            this.axVSPrinter1.Table = "<2000|<9000;Owner's Address:|" + m_sOwnHse + " " + m_sOwnStr + " " + m_sOwnBrgy + " " + m_sOwnMun + " " + m_sOwnProv;
            this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Postal Code:||Email Address:|" + m_sOwnEmailAdd;
            this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Telephone No.:|" + m_sOwnTelNo + "|Mobile No.:|";
            this.axVSPrinter1.Table = "<11000;In case of emergency, provide name of contact person: ";
            this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Telephone/ Mobile No.:||Email Address:|";
            string sTotEmp = string.Empty;
            int iMale = 0; int iFemale = 0;
            int.TryParse(m_sNoMale, out iMale);
            int.TryParse(m_sNoFemale, out iFemale);
            sTotEmp = string.Format("{0:###}",iMale+iFemale);

            this.axVSPrinter1.Table = "<2000|<1000|<3000|<750|<3500|<750;Business Area (in sqm.)|" + m_sArea + "|Total No. of Employees in Establishment:|" + sTotEmp + "|No. of Employees Residing within LGU:|" + m_sNoGender;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11000;Note: Fill Up Only if Business Place is Rented";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            GetOwnerDetails(sLessor);
            string sLessorNm = string.Empty;
            sLessorNm = m_sOwnLn;
            if (m_sOwnFn.Trim() != "")
                sLessorNm += ", " + m_sOwnFn;
            if (m_sOwnMi.Trim() != "")
                sLessorNm += " " + m_sOwnMi;

            this.axVSPrinter1.Table = "<11000;Lessor's Full Name:   " + sLessorNm;
            this.axVSPrinter1.Table = "<11000;Lessor's Full Address:   " + m_sOwnHse + " " + m_sOwnStr + " " + m_sOwnBrgy + " " + m_sOwnMun + " " + m_sOwnProv;
            this.axVSPrinter1.Table = "<11000;Lessor's Fill Telephone/Mobile No.:   " + m_sOwnTelNo;
            this.axVSPrinter1.Table = "<5500|<5500;Lessor's Email Address:   " + m_sOwnEmailAdd + "|Monthly Rental:   " + sRent;
            
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 800;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = "<11000;3. BUSINESS ACTIVITY";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.Table = "^4000|^2500|^1500|^3000;||Capitalization|Gross/Sales Receipts (for Renewal)";
            this.axVSPrinter1.Table = "^4000|^2500|^1500|^1500|^1500;Line of Business||(for New Business)|Essential|Non-Essential";
            this.axVSPrinter1.FontSize = (float)10.5;
            //OracleResultSet pRec = new OracleResultSet();
            
            string sCap = string.Empty;
            string sEss = string.Empty;
            string sNonEss = string.Empty;
            string sSupple = string.Empty;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            int iAddlBns = 0;
            string sPrevCap = string.Empty;
            sPrevCap = GetPrevCapGross(m_sBIN, m_sBnsCode);

            strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevCap + "|" + sCap + "|" + sEss + "|" + sNonEss;
            this.axVSPrinter1.Table = string.Format("<4000|<2500|<1500|<1500|<1500;{0}", strData);
            iAddlBns++;
            sPrevCap = "";
            pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' order by bns_code_main";
            if (pRec.Execute())
            {
                
                while (pRec.Read())
                {
                    iAddlBns++;
                    m_sBnsCode = pRec.GetString("bns_code_main");
                    sPrevCap = GetPrevCapGross(m_sBIN, m_sBnsCode);

                    if (pRec.GetString("bns_stat").Trim() == "NEW")
                        sCap = string.Format("{0:#,###.00}", pRec.GetDouble("capital"));
                    else
                        sCap = string.Format("{0:#,###.00}", pRec.GetDouble("gross"));

                    if (Convert.ToDouble(sCap) == 0)
                        sCap = "";
                    
                    sGross = "0";   
                    if (ValidateEssential(m_sBnsCode))
                    {
                        sEss = sGross;
                        sNonEss = "0";
                    }
                    else
                    {
                        sEss = "0";
                        sNonEss = sGross;
                    }
                    
                    if (Convert.ToDouble(sNonEss) == 0)
                        sNonEss = "";

                    if (Convert.ToDouble(sEss) == 0)
                        sEss = "";

                    if(iAddlBns > 5)
                    {
                        sSupple = "\n" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + sCap + "|" + sEss + "|" + sNonEss;
                    }
                    else
                    {
                        strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|"+sPrevCap+"|" + sCap + "|" + sEss + "|" + sNonEss;
                        this.axVSPrinter1.Table = string.Format("<4000|<2500|<1500|<1500|<1500;{0}", strData);
                    }
                }

                if (iAddlBns == 0)
                    iAddlBns++;

                for (int ix = iAddlBns; ix <= 4; ix++)
                {
                    strData = "||||";
                    this.axVSPrinter1.Table = string.Format("<4000|<2500|<1500|<1500|<1500;{0}", strData);
                }
                 
            }
            pRec.Close();
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Table = "<10000;Last Payment: " + sLastPaidYear + " " + sLastPaidQtr + " " + sLastBnsStat;

            //this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.Table = "^10000;Oath of Undertaking";
            this.axVSPrinter1.Paragraph = "";
            
            strData = "I DECLARE UNDER PENALTY OF PERJURY that the foregoing information are true based on my personal knowledge and ";
            strData += "authentic records. Further, I agree to comply with the regulatory requirement and other deficiencies within 30 days from ";
            strData += "release of the business permit, and I am aware that non-compliance within the prescribed period would be sufficient ground ";
            strData += "for cancellation of my business permit and closure of my business establishments.";
            this.axVSPrinter1.Table = string.Format("<12000;{0}", strData);
            this.axVSPrinter1.Paragraph = "";
            
            this.axVSPrinter1.MarginLeft = 5000;

            this.axVSPrinter1.Table = string.Format("^5000;_________________________________________________");
            this.axVSPrinter1.Table = string.Format("^5000;SIGNATURE OF APPLICANT/TAXPAYER OVER PRINTED NAME");
            this.axVSPrinter1.Table = string.Format("^5000;________________________________________");
            this.axVSPrinter1.Table = string.Format("^5000;POSITION/TITLE");

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11000;II. LGU SECTION (Do Not Fill Up This Section)";
            this.axVSPrinter1.MarginLeft = 800;
            this.axVSPrinter1.Table = "<11000;1. VERIFICATION OF DOCUMENTS";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.Table = "^4000|^4000|^1000|^1000|^1000;Description|Office/Agency|Yes|No|Not Needed";
            this.axVSPrinter1.FontSize = (float)10.5;
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Engineering Clearance|Office of the Building Official|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Barangay Clearance (For Renewal)|Barangay|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Sanitary Permit/Health Clearance|City Health Office|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;City Environmental Certificate|City Environment and Natural Resources Office|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Market Clearance (For Stall Holders)|Office of the City Market Administrator|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Valid Fire Safety Inspection Certificate|Bureau of Fire Protection|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Zoning Clearance|City Planning & Development Office|||";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.MarginLeft = 5000;

            this.axVSPrinter1.Table = string.Format("^5000;Approved by:");
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = (float)10.5;
            this.axVSPrinter1.Table = string.Format("^5000;" + AppSettingsManager.GetConfigValue("03"));
            this.axVSPrinter1.Table = string.Format("^5000;BPLO HEAD");

            if (sSupple != "")
            {
                this.axVSPrinter1.NewPage();
                this.axVSPrinter1.Table = string.Format("^10000;SUPPLEMENTAL PAGE");
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.Table = "<10000;3. BUSINESS ACTIVITY";
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.MarginLeft = 500;

                this.axVSPrinter1.Table = "^5000|^1500|^1500|^3000;||Capitalization|Gross/Sales Receipts (for Renewal)";
                this.axVSPrinter1.Table = "^5000|^1500|^1500|^1500|^1500;Line of Business|No. of Units|(for New Business)|Essential|Non-Essential";
                this.axVSPrinter1.Table = string.Format("<5000|<1500|<1500|<1500|<1500;{0}", sSupple);
            }
        }

        private void PrintAppForm_Binan_Blank()
        {
            // RMC 20161219 add printing of blank application form
            long lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.MarginLeft = 500;

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.Table = "<11000;BPLO-001-0";
            this.axVSPrinter1.FontBold = true;

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.Table = "^11000;APPLICATION FORM FOR BUSINESS PERMIT";
            this.axVSPrinter1.Table = "^11000;TAX YEAR " + AppSettingsManager.GetConfigValue("12");
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetConfigValue("01") + " OF " + AppSettingsManager.GetConfigValue("02");
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;

            string strData = string.Empty;
            strData = "INSTRUCTIONS:\n";
            strData += "1. Provide accurate information and print legibly to avoid delays. Incomplete application form will be returned to the applicant.\n";
            strData += "2. Ensure that all documents attached to this form (if any) are complete and properly filled out.";
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);

            this.axVSPrinter1.FontBold = true;

            //this.axVSPrinter1.Table = "<11000;I. APPLICATION SECTION";
            //this.axVSPrinter1.MarginLeft = 800;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.Table = "<5500|<3500|<2000;1. BASIC INFORMATION||BIN: ";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11000;I. APPLICATION SECTION";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Table = "<800|<5500|<2700|<2000;|1. BASIC INFORMATION||BIN: ";

            string sLastPaidYear = string.Empty;
            string sLastPaidQtr = string.Empty;
            string sLastBnsStat = string.Empty;
            
            this.axVSPrinter1.FontBold = false;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);


            this.axVSPrinter1.Table = "<1000|<1500|<500|<5000|<3000; ___ New| ___ Renewal||Mode of Payment: __ Anually  __ Semi-Annually  __ Quarterly|Last Payment: || ";
                    

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.Table = "<5500|<5500;Date of Application:  |Date Started:  ";
            this.axVSPrinter1.Table = "<5500|<5500;TIN No.:  |DTI/SEC/CDA Registration No.:  ";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
           
            this.axVSPrinter1.Table = "<11000;Type of Business:   Single     Partnership       Corporation       Cooperative";
           
            this.axVSPrinter1.Table = "<1250|<750|<3000|<3000|<3000;Amendment:|From| ___ Single| ___ Partnership| ___ Corporation";
            this.axVSPrinter1.Table = "<1250|<750|<3000|<3000|<3000;|To| ___ Single| ___ Partnership| ___ Corporation";

            this.axVSPrinter1.Table = "<11000;Are you enjoying tax incentive from any Government Entity? ___ Yes   ___ No  Please specify the entity?";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "^11000;Name of Taxpayer/Registrant";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.Table = "<5500|<4000|<1500;Last Name:  |First Name: |Middle Name:  ";

            this.axVSPrinter1.Table = "<11000;Business Name:  " ;

            this.axVSPrinter1.Table = "<11000;Trade Name/ Franchise: ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 800;

            this.axVSPrinter1.Table = "<11000;2. OTHER INFORMATION";

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<11000;Note: For renewal applications, do not fill up this section unless certain information have changed.";
            this.axVSPrinter1.MarginLeft = 500;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.Table = "<11000;Business Address: " ;

            this.axVSPrinter1.Table = "<1500|<4000|<1500|<4000;Postal Code:||Email Address:|" ;
            this.axVSPrinter1.Table = "<1500|<4000|<1500|<4000;Telephone No.:||Mobile No.:|";
            this.axVSPrinter1.Table = "<11000;Owner's Address:";
            this.axVSPrinter1.Table = "<1500|<4000|<1500|<4000;Postal Code:||Email Address:|";
            this.axVSPrinter1.Table = "<1500|<4000|<1500|<4000;Telephone No.:||Mobile No.:|";
            this.axVSPrinter1.Table = "<11000;In case of emergency, provide name of contact person:";
            this.axVSPrinter1.Table = "<1500|<4000|<1500|<4000;Telephone/ Mobile No.:||Email Address:|";
            string sTotEmp = string.Empty;
            int iMale = 0; int iFemale = 0;
            int.TryParse(m_sNoMale, out iMale);
            int.TryParse(m_sNoFemale, out iFemale);
            sTotEmp = string.Format("{0:###}", iMale + iFemale);

            this.axVSPrinter1.Table = "<2000|<1000|<3000|<750|<3500|<750;Business Area (in sqm.)||Total No. of Employees in Establishment:||No. of Employees Residing within LGU:|";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11000;Note: Fill Up Only if Business Place is Rented";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
           

            this.axVSPrinter1.Table = "<11000;Lessor's Full Name:   ";
            this.axVSPrinter1.Table = "<11000;Lessor's Full Address:   ";
            this.axVSPrinter1.Table = "<11000;Lessor's Fill Telephone/Mobile No.:   ";
            this.axVSPrinter1.Table = "<5500|<5500;Lessor's Email Address:   |Monthly Rental:   ";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 800;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = "<11000;3. BUSINESS ACTIVITY";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.MarginLeft = 500;

            this.axVSPrinter1.Table = "^5000|^1500|^1500|^3000;||Capitalization|Gross/Sales Receipts (for Renewal)";
            this.axVSPrinter1.Table = "^5000|^1500|^1500|^1500|^1500;Line of Business|No. of Units|(for New Business)|Essential|Non-Essential";

            OracleResultSet pRec = new OracleResultSet();

            string sCap = string.Empty;
            string sEss = string.Empty;
            string sNonEss = string.Empty;
            string sSupple = string.Empty;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            int iAddlBns = 0;
            strData = "||||";
            this.axVSPrinter1.Table = string.Format("<5000|<1500|<1500|<1500|<1500;{0}", strData);
            iAddlBns++;

            if (iAddlBns == 0)
                iAddlBns++;

            for (int ix = iAddlBns; ix <= 5; ix++)
            {
                strData = "||||";
                this.axVSPrinter1.Table = string.Format("<5000|<1500|<1500|<1500|<1500;{0}", strData);
            }
           
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = "^10000;Oath of Undertaking";
            this.axVSPrinter1.Paragraph = "";

            strData = "I DECLARE UNDER PENALTY OF PERJURY that the foregoing information are true based on my personal knowledge and ";
            strData += "authentic records. Further, I agree to comply with the regulatory requirement and other deficiencies within 30 days from ";
            strData += "release of the business permit, and I am aware that non-compliance within the prescribed period would be sufficient ground ";
            strData += "for cancellation of my business permit and closure of my business establishments.";
            this.axVSPrinter1.Table = string.Format("<12000;{0}", strData);
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.MarginLeft = 5000;

            this.axVSPrinter1.Table = string.Format("^5000;_________________________________________________");
            this.axVSPrinter1.Table = string.Format("^5000;SIGNATURE OF APPLICANT/TAXPAYER OVER PRINTED NAME");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("^5000;________________________________________");
            this.axVSPrinter1.Table = string.Format("^5000;POSITION/TITLE");

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<11000;II. LGU SECTION (Do Not Fill Up This Section)";
            this.axVSPrinter1.MarginLeft = 800;
            this.axVSPrinter1.Table = "<11000;1. VERIFICATION OF DOCUMENTS";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "^4000|^4000|^1000|^1000|^1000;Description|Office/Agency|Yes|No|Not Needed";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Engineering Clearance|Office of the Building Official|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Barangay Clearance (For Renewal)|Barangay|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Sanitary Permit/Health Clearance|City Health Office|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;City Environmental Certificate|City Environment and Natural Resources Office|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Market Clearance (For Stall Holders)|Office of the City Market Administrator|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Valid Fire Safety Inspection Certificate|Bureau of Fire Protection|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Zoning Clearance|City Planning & Development Office|||";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.MarginLeft = 5000;

            this.axVSPrinter1.Table = string.Format("^5000;Approved by:");
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("^5000;" + AppSettingsManager.GetConfigValue("03"));
            this.axVSPrinter1.Table = string.Format("^5000;BPLO HEAD");
        }

        private void PrintAppForm_Malolos(string sAppDate, string sDTINo, string sDTIDate, string sOrgKind, string sCTCNo, string sTIN, string sTINDate, string sSSS, string sSSSDate, string sPhilhealth, string sPhilhealthDate, string sCTCDate, string m_sBussPlate, string m_sAppNo) 
        {

            
            // RMC 20161129 customized application form for Binan
            this.axVSPrinter1.CurrentY = 350;
            long lngY;
            long lngY2;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.MarginLeft = 800; //JARS 20170331
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            //this.axVSPrinter1.Table = "<11000;BPLO-001-0";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.DrawPicture(Resources.unnamed, "0.4in", "0.4in", "15%", "16%", 11, false);

            this.axVSPrinter1.MarginLeft = 500; //JARS 20170331
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.Table = "^11000;APPLICATION FORM FOR BUSINESS PERMIT";
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = "^11000;TAX YEAR " + AppSettingsManager.GetConfigValue("12");
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetConfigValue("41"); //JARS 20170331
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetConfigValue("09");

            this.axVSPrinter1.FontBold = false;

            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.MarginLeft = 1800;
            //this.axVSPrinter1.FontSize = (float)8;

            //this.axVSPrinter1.Table = "<11000;Republic of the Philippines";
            //this.axVSPrinter1.Table = "<11000;Province of " + AppSettingsManager.GetConfigValue("08");
            //this.axVSPrinter1.Table = "<11000;" + AppSettingsManager.GetConfigValue("09"); //JARS 20170331

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.MarginLeft = 500;

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;

            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.FontBold = true;
            string strData = string.Empty;
            strData = "INSTRUCTIONS:\n";
            strData += "1. Provide accurate information and print legibly to avoid delays. Incomplete application form will be returned to the applicant.\n";
            strData += "2. Ensure that all documents attached to this form (if any) are complete and properly filled out.";
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<11000;I. APPLICANT SECTION";
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.Table = "<800|<2000|>3500;|1. Application No :|BIN :"; //JARS 20170331
            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            #region comments
            //this.axVSPrinter1.MarginLeft = 2500;
            //this.axVSPrinter1.Table = "^1500; "; //JARS 20170331
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.MarginLeft = 10500;
            //this.axVSPrinter1.Table = "^1000; "; //JARS 20170331
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.MarginLeft = 5700;
            //this.axVSPrinter1.Table = "^1500; "; // JAV 20170407 Add box 
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.MarginLeft = 10500;
            //this.axVSPrinter1.Table = "^2500;"; //JARS 20170331
            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "<5500|>5500;1. BASIC INFORMATION|BIN: " + m_sBIN;
            #endregion
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<800|<5000|<2700|<2500;|1. BASIC INFORMATION||BIN: "; // JAV 20170815
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = string.Format("<800|<5000|<2700|<2500;|||BIN: {0}",m_sBIN); // JAV 20170815
            //this.axVSPrinter1.Table = "<11000;1. INFORMATION";
            
            string sLastPaidYear = string.Empty;
            string sLastPaidQtr = string.Empty;
            string sLastBnsStat = string.Empty;
            AppSettingsManager.GetLastPaymentInfo(m_sBIN, AppSettingsManager.GetLastPaymentInfo(m_sBIN, "OR"), out sLastPaidYear, out sLastPaidQtr, out sLastBnsStat);

            this.axVSPrinter1.FontBold = false;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = lngY;

            OracleResultSet pRec = new OracleResultSet();
            #region comments
            //pRec.Query = "select * from business_que where bin = '" + m_sBIN + "' order by tax_year desc";
            //if (pRec.Execute())
            //{
            //    if (pRec.Read())
            //    {
            //        m_sApplType = pRec.GetString("bns_stat");
            //        sAppDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dt_applied"));
            //    }
            //    else
            //    {
            //        pRec.Close();
            //        pRec.Query = "select * from businesses where bin = '" + m_sBIN + "'";
            //        if (pRec.Execute())
            //        {
            //            if (pRec.Read())
            //            {
            //                m_sApplType = "REN";
            //                sAppDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
            //            }
            //        }
            //    }
            //}
            //pRec.Close();
            #endregion
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            #region comments
            //if (m_sApplType == "NEW")
            //    this.axVSPrinter1.Table = "<1000|<1500|<500|<5000;  X |||";
            //else
            //    this.axVSPrinter1.Table = "<1000|<1500|<500|<5000; |  X  ||";
            #endregion
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "^500|^450|<1200|^450|<1200|<1750|<450|<1300|<450|<1800|<450|^1000;||New||Renewal|Mode of Payment:||Annually||Semi-Annually||Quarterly";
            //if (m_sApplType == "NEW") //JARS 20170331
            //    this.axVSPrinter1.Table = "^500|^450|<1200|^450|<1200|<1750|<450|<1300|<450|<1800|<450|^1000;|X|New||Renewal|Mode of Payment:||Annually||Semi-Annually||Quarterly"; 
            //else if (m_sApplType == "REN")
            //    this.axVSPrinter1.Table = "^500|^450|<1200|^450|<1200|<1750|<450|<1300|<450|<1800|<450|^1000;||New|X|Renewal|Mode of Payment:||Annually||Semi-Annually||Quarterly"; 
            lngY2 = Convert.ToInt64(axVSPrinter1.CurrentY);
            //CHECKBOXES
            this.axVSPrinter1.FontSize = (float)6;
            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 2650;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 6050;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 7800;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 10050;
            this.axVSPrinter1.Table = "^250; ";


            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            if (m_sApplType == "NEW") //JARS 20170331
            {
                this.axVSPrinter1.MarginLeft = 900;
                this.axVSPrinter1.Table = "^450;X";
            }
            else if (m_sApplType == "REN")
            {
                this.axVSPrinter1.MarginLeft = 2550;
                this.axVSPrinter1.Table = "^450;X";
            }
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = lngY2;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<2750|<2750;Date of Application:|");
            this.axVSPrinter1.Table = string.Format("<2750|<2750;TIN No.:|");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.MarginLeft = 6000;
            this.axVSPrinter1.CurrentY = lngY2;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<2750|<2750;DTI/SEC/CDA Registration Date: |");
            this.axVSPrinter1.Table = string.Format("<2750|<2750;DTI/SEC/CDA Registration No.: |");

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = lngY2;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<2750|<2750;|{0}",sAppDate);
            this.axVSPrinter1.Table = string.Format("<2750|<2750;|{0}",sTIN);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.MarginLeft = 6000;
            this.axVSPrinter1.CurrentY = lngY2;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<2750|<2750;|{0}",sDTIDate);
            this.axVSPrinter1.Table = string.Format("<2750|<2750;|{0}",sDTINo);
            //this.axVSPrinter1.Table = "<2750|<2750|<2750|<2750;Date of Application:||DTI/SEC/CDA Registration Date: |";
            //this.axVSPrinter1.Table = "<2750|<2750|<2750|<2750;TIN No.:  ||DTI/SEC/CDA Registration No.: |";
            //this.axVSPrinter1.Table = "<5500|<5500;  |PhilHealth:  ";
            //this.axVSPrinter1.Table = "<5500|<5500;SSS:  |Pag-ibig:  ";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            #region comments
            /*this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;Type of Business:| ___ Single| ___ Partnership| ___ Corporation| ___ Cooperative";
            this.axVSPrinter1.CurrentY = lngY;

            if(sOrgKind == "SINGLE PROPRIETORSHIP")
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;|  X  | | | ";
            else if (sOrgKind == "PARTNERSHIP")
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;| |  X  | | ";
            else if (sOrgKind == "CORPORATION")
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;| | |  X  | ";
            else 
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;| | | |  X  ";*/
            //if (sOrgKind == "SINGLE PROPRIETORSHIP")
            //    this.axVSPrinter1.Table = "<11000;Type of Business: X Single     Partnership       Corporation       Cooperative";
            //else if (sOrgKind == "PARTNERSHIP")
            //    this.axVSPrinter1.Table = "<11000;Type of Business:   Single   X Partnership       Corporation       Cooperative";
            //else if (sOrgKind == "CORPORATION")
            //    this.axVSPrinter1.Table = "<11000;Type of Business:   Single     Partnership     X Corporation       Cooperative";
            //else
            //    this.axVSPrinter1.Table = "<11000;Type of Business:   Single     Partnership       Corporation     X Cooperative"; 
            //if (sOrgKind == "SINGLE PROPRIETORSHIP") //JARS 20170331
            //    this.axVSPrinter1.Table = "<2000|^450|<1700|^450|<1700|^450|<1700|^450|<2100;Type of Business:|X|Single||Partnership||Corporation||Cooperative";
            //else if (sOrgKind == "PARTNERSHIP")
            //    this.axVSPrinter1.Table = "<2000|^450|<1700|^450|<1700|^450|<1700|^450|<2100;Type of Business:||Single|X|Partnership||Corporation||Cooperative";
            //else if (sOrgKind == "CORPORATION")
            //    this.axVSPrinter1.Table = "<2000|^450|<1700|^450|<1700|^450|<1700|^450|<2100;Type of Business:||Single||Partnership|X|Corporation||Cooperative";
            //else
            //    this.axVSPrinter1.Table = "<2000|^450|<1700|^450|<1700|^450|<1700|^450|<2100;Type of Business:||Single||Partnership||Corporation|X|Cooperative";
            #endregion
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;

            //CHECKBOXES
            this.axVSPrinter1.FontSize = (float)6;
            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 2500;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 4650;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 6800;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 8950;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
           // this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Type of Business:||Single||Partnership||Corporation||Cooperative";
            if (sOrgKind == "SINGLE PROPRIETORSHIP") //JARS 20170331
                this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Type of Business:|X|Single||Partnership||Corporation||Cooperative";
            else if (sOrgKind == "PARTNERSHIP")
                this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Type of Business:||Single|X|Partnership||Corporation||Cooperative";
            else if (sOrgKind == "CORPORATION")
                this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Type of Business:||Single||Partnership|X|Corporation||Cooperative";
            else
                this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Type of Business:||Single||Partnership||Corporation|X|Cooperative";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //CHECKBOXES
            this.axVSPrinter1.FontSize = (float)6;
            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 2500;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 4650;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 6800;
            this.axVSPrinter1.Table = "^250; ";

            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 8950;
            //this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Amendment From:||Single||Partnership||Corporation||Cooperative";
            this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Amendment From:||Single||Partnership||Corporation";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //CHECKBOXES
            this.axVSPrinter1.FontSize = (float)6;
            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 2500;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 4650;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 6800;
            this.axVSPrinter1.Table = "^250; ";

            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 8950;
            //this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;                       To:||Single||Partnership||Corporation||Cooperative";
            this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;                       To:||Single||Partnership||Corporation";

            //this.axVSPrinter1.Table = "<11000;Are you enjoying tax incentive from any Government Entity? ___ Yes   ___ No  Please specify the entity?";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //CHECKBOXES
            this.axVSPrinter1.FontSize = (float)6;
            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 5500;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 6450;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = "<5000|<450|<500|<450|<500|<4100;Are you enjoying tax incentive from any Government Entity?||Yes||No|Please specify the entity?"; //JARS 20170331

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.Table = "^11000;Name of Taxpayer/Registrant \n";
            this.axVSPrinter1.Table = "^11000;Name of Taxpayer/Registrant";
            this.axVSPrinter1.FontBold = false;
           
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColumns;
            //this.axVSPrinter1.FontSize = (float)8;
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.Table = "<1200|<3598|<1200|<3000|<1200|<802;Last Name: ||First Name: ||Middle Name: |";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<1200|<3299|<1200|<3299|<1200|<802;Last Name: |{0}|First Name: |{1}|Middle Name: |{2}", m_sOwnLn, m_sOwnFn, m_sOwnMi);

            lngY2 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "<1600|<3900|<1500|<3000|<500|<500;|" + m_sOwnLn + "||" + m_sOwnFn + "||" + m_sOwnMi; // JAV 20170410 Adjust the names space and remove the border
            //this.axVSPrinter1.Table = "<1100|<3900|<600|<3000|<1650|<500;|" + m_sOwnLn + "||" + m_sOwnFn + "||" + m_sOwnMi; // JAV 20170410 Adjust the names space and remove the border
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            ////CHECKBOXES
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);

            #region comments
            //this.axVSPrinter1.FontSize = (float)6;
            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 8600;
            //this.axVSPrinter1.Table = "^250; ";

            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 10050;
            //this.axVSPrinter1.Table = "^250; ";

            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.FontSize = (float)10;
            #endregion

            this.axVSPrinter1.CurrentY = lngY2;

            #region comments
            ////this.axVSPrinter1.Table = "<4300|<2700|<1000|^450|<1000|^450|<1100;|Birthdate: " + m_sBdate + "|Gender:||Male||Female";
            //this.axVSPrinter1.Table = "<4300|2600|<1000|^600|<700|^800|<1000;Birthdate:" + m_sBdate + "|Nationality:|Gender:||Male||Female"; // JAV 20170407
            //this.axVSPrinter1.CurrentY = lngY2;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.Table = "<4300|<2700|<1000|^450|<1000|^450|<1100;||||||";
            //if (m_sGender == "MALE")
            //{
            //    this.axVSPrinter1.Table = "<4300|<2700|<1000|^450|<1000|^450|<1100;|||X|||";
            //}
            //else
            //{
            //    this.axVSPrinter1.Table = "<4300|<2700|<1000|^450|<1000|^450|<1100;|||||X|"; //JAV 20170407 Remove the X mark in gender
            //}
            #endregion  

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<1700|<9300;Business Name: |";
            lngY2 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<1700|<9300;|" + AppSettingsManager.GetBnsName(m_sBIN);
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //CHECKBOXES
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            #region comments
            //this.axVSPrinter1.FontSize = (float)6;
            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 6000;
            //this.axVSPrinter1.Table = "^250; ";

            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 8750;
            //this.axVSPrinter1.Table = "^250; ";
            #endregion  
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY2;
            this.axVSPrinter1.Table = "<5500|<450|<2300|<450|<2300;Trade Name/ Franchise: ||Main||Branch";
            lngY2 = Convert.ToInt64(axVSPrinter1.CurrentY);

            this.axVSPrinter1.CurrentY = lngY2;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)9;
            #region comments
            //this.axVSPrinter1.Table = "<11000;2. OTHER INFORMATION";

            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.FontSize = (float)8;
            //this.axVSPrinter1.Table = "<11000;Note: For renewal applications, do not fill up this section unless certain information have changed.";
            #endregion
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = "<800|<10200;|2. OTHER INFORMATION \n   Note: For renewal applications, do not fill up this section unless certain information have changed.";
            this.axVSPrinter1.FontBold = false;
            

            
            #region comments
            //this.axVSPrinter1.Table = "<11000;Business Address: " + m_sBnsHse + " " + m_sBnsStr + " " + m_sBnsStr + " " + m_sBnsBrgy + " " + m_sBnsMun;

            //this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Postal Code:||Email Address:|" + m_sEmail;
            //this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Telephone No.:|" + m_sTelNo + "|Mobile No.:|";
            //this.axVSPrinter1.Table = "<2000|<9000;Owner's Address:|" + m_sOwnHse + " " + m_sOwnStr + " " + m_sOwnBrgy + " " + m_sOwnMun + " " + m_sOwnProv;
            //this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Postal Code:||Email Address:|" + m_sOwnEmailAdd;
            //this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Telephone No.:|" + m_sOwnTelNo + "|Mobile No.:|";
            //this.axVSPrinter1.Table = "<11000;In case of emergency, provide name of contact person: ";
            //this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Telephone/ Mobile No.:||Email Address:|";
            #endregion\
            #region comments
            ////JARS 20170331
            //this.axVSPrinter1.Table = "<1800|<3700|<1800|<3700;Business Address:||Owner's Address:|";
            //this.axVSPrinter1.Table = "<2000|<3500|<2000|<3500;House No./Bldg No.:|" + m_sBnsHse + "|House No./Bldg No.:|" + m_sOwnHse;
            //this.axVSPrinter1.Table = "<1800|<3700|<1800|<3700;Street:|" + m_sBnsStr + "|Street:|" + m_sOwnStr;
            //this.axVSPrinter1.Table = "<1800|<3700|<1800|<3700;Subdivision:||Subdivision:|";
            ////this.axVSPrinter1.Table = "<1800|<3700|<1800|<3700;Barangay:|" + m_sBnsBrgy + "   City/Mun: " + AppSettingsManager.GetConfigObject("02") + "|Barangay:|" + m_sOwnBrgy + "   City/Mun: " + m_sOwnMun; // JAV 20170407 Add column for City/Mun
            //this.axVSPrinter1.Table = "<1100|<1700|<1100|<1600|<1100|<1800|<1100|<1500;Barangay:|" + m_sBnsBrgy + "|City/Mun:|" + AppSettingsManager.GetConfigObject("02") + "|Barangay:|" + m_sOwnBrgy + "|City/Mun:|" + m_sOwnMun; // JAV 20170407 Add column for City/Mun
            //this.axVSPrinter1.Table = "<1800|<3700|<1800|<3700;Province:|" + AppSettingsManager.GetConfigObject("08") + "|Province:|" + m_sOwnProv;
            //this.axVSPrinter1.Table = "<2200|<3300|<2000|<3500;Telephone/Mobile No:|" + m_sOwnTelNo + "|Email Address:|";
            #endregion
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.Table = "<11000;Business Address: \n ";
            this.axVSPrinter1.Table = "<11000;Business Address: " + m_sBnsHse + " " + m_sBnsStr + " " + m_sBnsStr + " " + m_sBnsBrgy + " " + m_sBnsMun + "\n ";
            
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.Table = "<2750|<2750|<2750|<2750;Postal Code:||Email Address:|" + m_sEmail;
            this.axVSPrinter1.Table = "<2750|<2750|<2750|<2750;Telephone No.:|" + m_sTelNo + "|Mobile No.:";
            
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.Table = "<11000;Owner's Address: " + m_sOwnHse + " " + m_sOwnStr + " " + m_sOwnBrgy + " " + m_sOwnMun + "\n ";
            //this.axVSPrinter1.Table = "<2750|<2750|<2750|<2750;Postal Code:||Email Address:|" + m_sOwnEmailAdd;
            this.axVSPrinter1.Table = "<2750|<2750|<2750|<2750;Postal Code:|" + m_sOwnZip + "|Email Address:|" + m_sOwnEmailAdd;    // RMC 20171116 added info to display in application form
            this.axVSPrinter1.Table = "<2750|<2750|<2750|<2750;Telephone No.:|"+m_sOwnTelNo+"|Mobile No.:";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.Table = "<11000;In case of emergency, provide name of contact person:";
            this.axVSPrinter1.Table = "<11000;In case of emergency, provide name of contact person: " + m_sSpouse;  // RMC 20171116 added info to display in application form
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.Table = "<5500|<5500;Telephone/Mobile No.:|Email Address:";

            string sTotEmp = string.Empty;
            int iMale = 0; int iFemale = 0;
            int.TryParse(m_sNoMale, out iMale);
            int.TryParse(m_sNoFemale, out iFemale);
            sTotEmp = string.Format("{0:###}", iMale + iFemale);
            #region comments
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns; // JAV 20170410 Change the tbBoxColumns to tbBox
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox; // JAV 20170410 Change the tbBoxColumns to tbBox

            //lngY = Convert.ToInt64(axVSPrinter1.CurrentY);// JAV 20170410 Add design
            //lngY2 = Convert.ToInt64(axVSPrinter1.CurrentY);// JAV 20170410 Add design

            //this.axVSPrinter1.FontSize = (float)12;
            //this.axVSPrinter1.CurrentY = lngY + 70;
            //this.axVSPrinter1.MarginLeft = 2645;
            //this.axVSPrinter1.Table = "^1000; "; // JAV 20170410  will display the box

            //this.axVSPrinter1.FontSize = (float)12;
            //this.axVSPrinter1.CurrentY = lngY + 70;
            //this.axVSPrinter1.MarginLeft = 6200;
            //this.axVSPrinter1.Table = "^1000; "; // JAV 20170410  will display the box

            //this.axVSPrinter1.FontSize = (float)12;
            //this.axVSPrinter1.CurrentY = lngY + 70;
            //this.axVSPrinter1.MarginLeft = 10350;
            //this.axVSPrinter1.Table = "^1000; "; // JAV 20170410  will display the box

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;// JAV 20170410

            //this.axVSPrinter1.FontSize = (float)9;
            //this.axVSPrinter1.CurrentY = lngY2 + 70;
            //this.axVSPrinter1.MarginLeft = 2645;
            //this.axVSPrinter1.Table = "<2000; " + m_sArea;// JAV 20170410 Will display the result

            //this.axVSPrinter1.FontSize = (float)9;
            //this.axVSPrinter1.CurrentY = lngY + 70;
            //this.axVSPrinter1.MarginLeft = 6200;
            //this.axVSPrinter1.Table = "^1000; " + sTotEmp; // JAV 20170410 Will display the result

            //this.axVSPrinter1.FontSize = (float)9;
            //this.axVSPrinter1.CurrentY = lngY + 70;
            //this.axVSPrinter1.MarginLeft = 10350;
            //this.axVSPrinter1.Table = "^1000; " + m_sNoGender;  // JAV 20170410 Will display the result

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;// JAV 20170410

            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.FontSize = (float)9;
            //this.axVSPrinter1.CurrentY = lngY;
            ////this.axVSPrinter1.Table = "<2000|<1000|<3000|<750|<3500|<750;Business Area (in sqm.)|" + m_sArea + "|Total No. of Employees in Establishment:|" + sTotEmp + "|No. of Employees Residing within LGU:|" + m_sNoGender; // JAV 20170410 Adjust the spacing
            //this.axVSPrinter1.Table = "<2100|<1300|<2500|<1300|<2600|<1200;Business Area (in sqm.)||Total No. of Employees in Establishment:||No. of Employees Residing within LGU:||"; // JAV 20170410 Adjust the spacing
            #endregion  

            string sArea = string.Empty;
            sArea = GetArea(m_sBIN, AppSettingsManager.GetBnsCodeMain(m_sBIN), sTaxYear);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<2133|^1533|<3666|<3666;Business Area (in sqm.): |\n" + sArea + " |Total No. of Employees in Establishment: \n " + sTotEmp + "|No. of Employees Residing within LGU: \n " + m_sNoGender; // JAV 20170410 Adjust the spacing


            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = "<11000;Note: Fill Up Only if Business Place is Rented";
            this.axVSPrinter1.FontBold = false;
            GetOwnerDetails(sLessor);
            string sLessorNm = string.Empty;
            sLessorNm = m_sOwnLn;
            if (m_sOwnFn.Trim() != "")
                sLessorNm += ", " + m_sOwnFn;
            if (m_sOwnMi.Trim() != "")
                sLessorNm += " " + m_sOwnMi;
            #region comments
            //this.axVSPrinter1.Table = "<11000;Lessor's Full Name:   " + sLessorNm;
            //this.axVSPrinter1.Table = "<11000;Lessor's Full Address:   " + m_sOwnHse + " " + m_sOwnStr + " " + m_sOwnBrgy + " " + m_sOwnMun + " " + m_sOwnProv;
            //this.axVSPrinter1.Table = "<11000;Lessor's Fill Telephone/Mobile No.:   " + m_sOwnTelNo;
            //this.axVSPrinter1.Table = "<5500|<5500;Lessor's Email Address:   " + m_sOwnEmailAdd + "|Monthly Rental:   " + sRent;
            #endregion


            if (sLessor != "")
            {
                GetOwnerDetails(sLessor);
                string sLessAddress = string.Empty;
                string sLessTellNo = string.Empty;
                string sLessEmail = string.Empty;
                sLessAddress = m_sOwnHse + m_sOwnStr + m_sOwnBrgy;
                sLessTellNo = m_sOwnTelNo;
                sLessEmail = m_sOwnEmailAdd;
                
                //JARS 20170331
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                this.axVSPrinter1.FontSize = (float)8;
                //this.axVSPrinter1.Table = "<1800|<5000|<1500|<2700;Name of Lessor:|" + sLessorNm + "|Monthly Rental:|" + sRent;
                this.axVSPrinter1.Table = string.Format("<2500|250|<8250;Lessor's Full Name|:|{0}", AppSettingsManager.GetBnsOwner(sLessor));
                this.axVSPrinter1.Table = string.Format("<2500|250|<8250;Lessor's Full Address|:|{0}", sLessAddress);
                this.axVSPrinter1.Table = string.Format("<2500|250|<8250;Lessor's Telephone/Mobile No.|:|{0}", sLessTellNo);
                this.axVSPrinter1.Table = "<2500|<250|<8250;Lessor's Email Address|:|" + sLessEmail;
                this.axVSPrinter1.Table = "<2500|<250|<8250;Monthly Rental|:|" + sRent;
            }
            #region comments
            //this.axVSPrinter1.Table = "<2000|<2800|<1400|<4800;House No./Bldg No.:|" + m_sOwnHse + "|Street:|" + m_sOwnStr;
            //this.axVSPrinter1.Table = "<1800|<3000|<1400|<1800|<3000;Subdivision:||Barangay:|" + m_sOwnBrgy + "|Contact Person: ";
            //this.axVSPrinter1.Table = "<1800|<3000|<1400|<1800|<3000;City/Mun:|" + m_sOwnMun + "|Province:|" + m_sOwnProv + "|Tel Nos: ";
            //this.axVSPrinter1.Table = "<1800|<3000|<1600|<1600|<3000;Telephone:|" + m_sOwnTelNo + "|Email Address:||Email Address: ";
            #endregion
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = "<800|<10200;|3. BUSINESS ACTIVITY";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)9;
            //this.axVSPrinter1.Table = "^4000|^2500|^1500|^3000;||Capitalization|Gross/Sales Receipts (for Renewal)";
            //this.axVSPrinter1.Table = "^4000|^2500|^1500|^1500|^1500;Line of Business||(for New Business)|Essential|Non-Essential";
            object yy = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.FontSize = (float)8;
            //this.axVSPrinter1.Table = "^4000|^2000|^2000;Line of Business|No. of Units| Capitalization (for New Business)";
            //this.axVSPrinter1.Table = "^3500|^2000|^2000;Line of Business|Capitalization \n(for New Business)|Previous Gross"; //JARS 20170331
            this.axVSPrinter1.Table = "^3500|^2000|^2000;Line of Business|No of Units|Capitalization \n(for New Business)"; //JARS 20170331
            this.axVSPrinter1.CurrentY = yy;
            this.axVSPrinter1.MarginLeft = 8000;
            this.axVSPrinter1.Table = "^3500;Gross Sales Reciepts (for Renewal)";
            this.axVSPrinter1.Table = "^1750|^1750;Essential|Non - Essential";
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            //OracleResultSet pRec = new OracleResultSet();




            string sCap = string.Empty;
            string sEss = string.Empty;
            string sNonEss = string.Empty;
            string sSupple = string.Empty;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            int iAddlBns = 0;
            string sPrevCap = string.Empty;

            #region
            //sPrevCap = GetPrevCapGross(m_sBIN, m_sBnsCode);
            //sGross = "";

            //sGross = AppSettingsManager.GetCapitalGross(m_sBIN, m_sBnsCode, sTaxYear, m_sApplType);
            // RMC 20180103 display prev cap/ gross in application form, requested thru Jester (s)
          /*  BPLSAppSettingList sList = new BPLSAppSettingList();
            sList.ModuleCode = "APPFORM";
            sList.ReturnValueByBin = m_sBIN;
            string sBnsYear = string.Empty;
            string sBnsStat = string.Empty;
            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                sBnsYear = sList.BPLSAppSettings[i].sTaxYear;
                sBnsStat = sList.BPLSAppSettings[i].sBnsStat;
            }
            sPrevCap = AppSettingsManager.GetCapitalGross(m_sBIN, m_sBnsCode, sBnsYear, sBnsStat);
            if (sBnsStat == "NEW" && sPrevCap != "0")
                sPrevCap = "Prev. Cap: " + string.Format("{0:#,##0.00}", Convert.ToDouble(sPrevCap));
            else if (sBnsStat == "REN" && sPrevCap != "0")
                sPrevCap = "Prev. Gr: " + string.Format("{0:#,##0.00}", Convert.ToDouble(sPrevCap));
            else
                sPrevCap = "";
            // RMC 20180103 display prev cap/ gross in application form, requested thru Jester (e)
            */
            // RMC 20180103 removed as per Norie

            // RMC 20180103 corrected data for gross/cap in application form (s)
            #endregion

            sGross = AppSettingsManager.GetCapitalGross(m_sBIN, m_sBnsCode, AppSettingsManager.GetConfigValue("12"), m_sApplType);
            if (Convert.ToDouble(sGross) == 0)
                sGross = "";
            else
                sGross = string.Format("{0:#,##0.00}", Convert.ToDouble(sGross));
            // RMC 20180103 corrected data for gross/cap in application form (e)
            if (m_sApplType == "NEW")
            {
                //strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + string.Format("{0:#,##0.00}", sPrevCap) + "|" + sGross + "||" + sNonEss;
                strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevCap + "|" + sGross + "||" + sNonEss;   // RMC 20180103 display prev cap/ gross in application form, requested thru Jester    
                
            }
            else
            {
                //strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + string.Format("{0:#,##0.00}", sPrevCap) + "||" + sGross + "|" + sNonEss;
                strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevCap + "||" + sGross + "|" + sNonEss;    // RMC 20180103 display prev cap/ gross in application form, requested thru Jester
            }
            
            //this.axVSPrinter1.Table = string.Format("<3500|^2000|^2000|^1750|^1750;{0}", strData);
            this.axVSPrinter1.Table = string.Format("<3500|<2000|>2000|>1750|>1750;{0}", strData);  // RMC 20180103 corrected data for gross/cap in application form
            //object xY = this.axVSPrinter1.CurrentY;
            iAddlBns++;
            sPrevCap = "";
            int iCnt = 0;
            string sVal = "";
          
            ArrayList arrSuplementaryDesc = new ArrayList();
            //pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' order by bns_code_main";
            // JAV 20171020 add addl_bns_que table
            pRec.Query = "select * from (select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' union all select * from addl_bns_que where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "') order by bns_code_main";
            if (pRec.Execute())
            {

                while (pRec.Read())
                {
                    iAddlBns++;
                    m_sBnsCode = pRec.GetString("bns_code_main");
                    sPrevCap = GetPrevCapGross(m_sBIN, m_sBnsCode);

                    #region
                    /*if (pRec.GetString("bns_stat").Trim() == "NEW")
                        sCap = string.Format("{0:#,###.00}", pRec.GetDouble("capital"));
                    else
                        sCap = string.Format("{0:#,###.00}", pRec.GetDouble("gross"));*/
                    // RMC 20180103 corrected data for gross/cap in application form, put rem

                    // RMC 20180103 display prev cap/ gross in application form, requested thru Jester (s)
                   /* sPrevCap = AppSettingsManager.GetCapitalGross(m_sBIN, m_sBnsCode, sBnsYear, sBnsStat);
                    if (sBnsStat == "NEW" && sPrevCap != "0")
                        sPrevCap = "Prev. Cap: " + string.Format("{0:#,##0.00}", Convert.ToDouble(sPrevCap));
                    else if (sBnsStat == "REN" && sPrevCap != "0")
                        sPrevCap = "Prev. Gr: " + string.Format("{0:#,##0.00}", Convert.ToDouble(sPrevCap));
                    else
                        sPrevCap = "";
                    // RMC 20180103 display prev cap/ gross in application form, requested thru Jester (e)
                    */
                    // RMC 20180103 removed as per Norie
                    #endregion
                    sPrevCap = "";  // RMC 20180103 removed as per Norie, adjustment
                    sCap = AppSettingsManager.GetCapitalGross(m_sBIN, m_sBnsCode, AppSettingsManager.GetConfigValue("12"), m_sApplType);  // RMC 20180103 corrected data for gross/cap in application form

                    if (m_sApplType == "NEW")   // RMC 20180103 corrected data for gross/cap in application form
                    {
                        if (Convert.ToDouble(sCap) == 0)
                            sCap = "";
                        else
                            sCap = string.Format("{0:#,##0.00}", Convert.ToDouble(sCap));   // RMC 20180103 corrected data for gross/cap in application form
                        sEss = "";  // RMC 20180103 corrected data for gross/cap in application form
                        sNonEss = "";   // RMC 20180103 corrected data for gross/cap in application form
                    }
                    else
                    {
                        
                        sGross = "0";
                        sGross = sCap;  // RMC 20180103 corrected data for gross/cap in application form
                        if (ValidateEssential(m_sBnsCode))
                        {
                            sEss = sGross;
                            sNonEss = "0";
                        }
                        else
                        {
                            sEss = "0";
                            sNonEss = sGross;
                        }

                        if (Convert.ToDouble(sNonEss) == 0)
                            sNonEss = "";
                        else
                            sNonEss = string.Format("{0:#,##0.00}", Convert.ToDouble(sNonEss));   // RMC 20180103 corrected data for gross/cap in application form

                        if (Convert.ToDouble(sEss) == 0)
                            sEss = "";
                        else
                            sEss = string.Format("{0:#,##0.00}", Convert.ToDouble(sEss));   // RMC 20180103 corrected data for gross/cap in application form

                        sCap = "";  // RMC 20180103 corrected data for gross/cap in application form
                    }

                    if (iAddlBns > 3)
                    {
                        
                       // sSupple += "\n" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + sCap + "|" + sEss + "|" + sNonEss;
                        //arrSuplementaryDesc.Add(AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + sCap + "|" + sEss + "|" + sNonEss);
                        arrSuplementaryDesc.Add(AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevCap + "|" + sCap + "|" + sEss + "|" + sNonEss);  // RMC 20180103 display prev cap/ gross in application form, requested thru Jester
                        sVal += arrSuplementaryDesc[iCnt].ToString();
                        iCnt++;
                    }
                    else
                    {
                        //strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevCap + "|" + sCap + "|" + sEss + "|" + sNonEss;
                        //strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + sCap + "|" + sEss + "|" + sNonEss;
                        //this.axVSPrinter1.Table = string.Format("<3500|^2000|^2000|^1750|^1750;{0}", strData);
                        strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevCap + "|" + sCap + "|" + sEss + "|" + sNonEss; // RMC 20180103 display prev cap/ gross in application form, requested thru Jester
                        this.axVSPrinter1.Table = string.Format("<3500|<2000|>2000|>1750|>1750;{0}", strData);  // RMC 20180103 corrected data for gross/cap in application form
                    }

                    /*if (iAddlBns == 0) //JARS 20171006 QA
                        iAddlBns++;


                    for (int ix = iAddlBns; ix <= 2; ix++)
                    {
                        //strData = "||||"; // RMC 20180103 corrected data for gross/cap in application form, put rem
                        this.axVSPrinter1.Table = string.Format("<3500|^2000|^2000|^1750|^1750;{0}", strData);
                    }*/
                    // RMC 20180103 corrected data for gross/cap in application form, put rem
                }
                #region comments
                //if (iAddlBns == 0)
                //    iAddlBns++;


                //for (int ix = iAddlBns; ix <= 2; ix++)
                //{
                //    strData = "||||";
                //    this.axVSPrinter1.Table = string.Format("<3500|^2000|^2000|^1750|^1750;{0}", strData);
                //}
                #endregion
            }
            pRec.Close();

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            axVSPrinter1.Table = "<11000; \n \n \n \n \n \n \n \n \n \n \n \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY;
            axVSPrinter1.Table = "<3500|^2000|^2000|^1750|^1750;||||";
            strData = "I DECLARE UNDER PENALTY OF PERJURY that the foregoing information are true based on my personal knowledge and ";
            strData += "authentic records. Further, I agree to comply with the regulatory requirement and other deficiencies within 30 days from ";
            strData += "release of the business permit.";
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);  //JAV 20170410 Change the paragraph indention
            #region comments
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;

            //this.axVSPrinter1.Table = "<10000;Last Payment: " + sLastPaidYear + " " + sLastPaidQtr + " " + sLastBnsStat;

            //strData = "I DECLARE UNDER PENALTY OF PERJURY that the foregoing information are true based on my personal knowledge and ";
            //strData += "authentic records. Further, I agree to comply with the regulatory requirement and other deficiencies within 30 days from ";
            //strData += "release of the business permit.";
            ////strData += "and I am aware that non-compliance within the prescribed period would be sufficient ground ";
            ////strData += "for cancellation of my business permit and closure of my business establishments.";
            ////this.axVSPrinter1.Table = string.Format("^11000;{0}", strData);  //JAV 20170410 Change the paragraph indention
            //this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);  //JAV 20170410 Change the paragraph indention
            
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            #endregion
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.MarginLeft = 5000;

            //this.axVSPrinter1.DrawPicture(Resources.LubaoContact, "0.5in", "9.5in", "75%", "75%", 11, false);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^6000;_________________________________________________");
            this.axVSPrinter1.Table = string.Format("^6000;SIGNATURE OF APPLICANT/TAXPAYER OVER PRINTED NAME");
            this.axVSPrinter1.Paragraph = "";
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("^6000;________________________________________");
            this.axVSPrinter1.Table = string.Format("^6000;POSITION/TITLE");
            this.axVSPrinter1.MarginLeft = 500;

            #region comments
            //xY = this.axVSPrinter1.CurrentY; 
            //int aa = Convert.ToInt32(xY);
            //aa += 340;
            //xY = (object) aa;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.CurrentY = xY;
            //this.axVSPrinter1.MarginLeft = 500;
            #endregion
            
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = lngY + 600;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "<11000;Application Form for Business Permit";

            this.axVSPrinter1.Table = "<11000;II. LGU SECTION ( Do Not Fill Up This Section )";

            this.axVSPrinter1.Table = "<800|<10200;|1. VERIFICATION OF DOCUMENTS";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "^4000|^4000|^1000|^1000|^1000;Description|Office/Agency|Yes|No|Not Needed";
            this.axVSPrinter1.FontSize = (float)8;

            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Occupancy Permit(For New)|Office of the  Building Official|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Barangay Clearance(For Renewal)|Barangay|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Sanitary Permit/Health Clearance|City Health Office|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;City of Environmental Certificate|Bulacan Environmental and Natural Resources Office|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Market Clearance(For Stall Holders)|Malolos Public Market|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Valid Fire Safety Inspection Certificate|Bureau of Fire Protection|||";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = string.Format("<11000;Customer Feedback: Kamusta ang naging serbisyo ng BPLO sa inyo? Bilugan ang sagot.");
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.CurrentY = lngY + 250;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<330|<1800|<330|<1300|<330|<1300|<330|<1300|<330|<3750;O||O||O||O||O|";
            this.axVSPrinter1.CurrentY = lngY + 300;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<330|<1800|<330|<1300|<330|<1300|<330|<1300|<330|<3750;|Lubos na nasiyahan||Nasiyahan||Nakuntento||Di Nasiyahan||Lubos na nadismaya";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.CurrentY = lngY + 1000;
            this.axVSPrinter1.Table = string.Format("<5500|<5500;Komentaryo/Mungkahi:________________________________|                                        ");
            this.axVSPrinter1.CurrentY = lngY + 1000;
            this.axVSPrinter1.Table = string.Format("^7200|<3800;|Verified by: BPLO");
            this.axVSPrinter1.Table = string.Format("^6500|^4500;|________________________________");

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.CurrentY = lngY + 1500; // AFM 20210616 previously 2000
            this.axVSPrinter1.Table = string.Format("<11000;Every risk is worth taking as long as it's for a good cause, and contributes to a good life");

            this.axVSPrinter1.Table = "<11000;You can register at " + AppSettingsManager.GetConfigValue("76") + " and apply online for renewal"; //MCR 20210616
            this.axVSPrinter1.FontItalic = true;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontItalic = false;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n \n \n \n \n \n \n \n";
            this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n \n \n \n \n \n";


            this.axVSPrinter1.NewPage(); //PAGE 2

            
            #region not confirmed yet
            /**
            String sBrgyClearanceDate = "";
            String sBrgyClearanceBy = "";
            String sZoningClearanceDate = "";
            String sZoningClearanceBy = "";
            String sSanitaryClearanceDate = "";
            String sSanitaryClearanceBy = "";
            String sOccClearanceDate = "";
            String sOccClearanceBy = "";
            String sAnnualClearanceDate = "";
            String sAnnualClearanceBy = "";
            String sFireClearanceDate = "";
            String sFireClearanceBy = "";
            String sOtherDate = "";
            String sOtherBy = "";

            pRec.Query = "select * from Permit_Type where bin = '" + m_sBIN + "' and current_year = '" + sTaxYear + "'";
            if (pRec.Execute())
                while (pRec.Read())
                {
                    if (pRec.GetString("perm_type") == "Sanitary")
                    {
                        sSanitaryClearanceDate = "X";
                        sSanitaryClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "ZONING")
                    {
                        sZoningClearanceDate = pRec.GetString("issued_date");
                        sZoningClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "Annual Inspection" || pRec.GetString("perm_type") == "ANNUAL")
                    {
                        sAnnualClearanceDate = pRec.GetString("issued_date");
                        sAnnualClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "WORKING")
                    {
                        sOtherDate = pRec.GetString("issued_date");
                        sOtherBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                }
            pRec.Close();
            **/
            #endregion
            
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n \n \n \n \n \n \n \n";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^11000;LIST OF REQUIREMENTS \n ";
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "^11000;REQUIREMENTS FOR ASSESSMENTS \n ";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^3666|^3666|^3666;NEW \n |RENEWAL \n |RETIREMENT \n ");
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;DTI/SEC/CDA|Basis for Computing Tax|Original Business Permit/Official Receipt");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;Barangay Business Clearance|(BIR Return, Gross Receipts, Book of Sales, POS Report)|Business Plate");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;Occupancy Permit|Barangay Business Clearance|Barangay Business Closure");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;TCT/Contract of Lease(if renting)|Fire Safety Inspection Certificate|Affidavit/Broad Resolution of Closure");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;Basis for Computing Tax(Capital)|CNC(Certificate of non-coverage)|Basis for Computing Tax");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;CNC(Certificate of non-coverage)|SSS|Authorization Letter if Representative"); // RMC 20180104 added SSS, Pagibig, Philhealth in App form
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;Sworn Statement/Notarized|Pag-ibig|Photo Copy of Owners ID and Representative");   // RMC 20180104 added SSS, Pagibig, Philhealth in App form
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;Owner's ID|Philhealth|");    // RMC 20180104 added SSS, Pagibig, Philhealth in App form
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;|Authorization Letter if Representative|");    // RMC 20180109 additional requirements in application form
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;|Photo Copy of Owners ID and Representative|");    // RMC 20180109 additional requirements in application form

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = string.Format("^11000;STANDARD STEPS");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = string.Format("^5500|^5500;NEW & RENEWAL|RETIREMENT");
            this.axVSPrinter1.FontBold = false;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.Table = string.Format("<800;Step 1: \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = "<5500; \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.MarginLeft = 1300;
            this.axVSPrinter1.Table = string.Format("<4700;Application Filing & verification-Submission of complete application form with attached documentary require and one-time verification. \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 6010;
            this.axVSPrinter1.Table = string.Format("<800;Step 1: \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = "<5500; \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.MarginLeft = 6810;
            this.axVSPrinter1.Table = string.Format("<4700;Filing and Verification/Inspection of Application and Requirements \n  ");

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = string.Format("<800;Step 2: \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.Table = "<5500; \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.MarginLeft = 1300;
            this.axVSPrinter1.Table = string.Format("<4700;Assessment one-time assessment of taxes, fees, and charges \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 6010;
            this.axVSPrinter1.Table = string.Format("<800;Step 2: \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.Table = "<5500; \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.MarginLeft = 6810;
            this.axVSPrinter1.Table = string.Format("<4700;Payment \n ");


            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = string.Format("<800;Step 3: \n \n \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.Table = "<5500; \n \n \n  ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.MarginLeft = 1300;
            this.axVSPrinter1.Table = string.Format("<4700;Pay and claim one-time payment of taxes, fees, and charges receipt of Official Receipt as proof of payment of taxes, fees, and charges imposed by the city/municipality and BFP in securing clearance. Business Permit and other regulatory permit and");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 6010;
            this.axVSPrinter1.Table = string.Format("<800;Step 3: \n \n \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.Table = "<5500; \n \n \n  ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.MarginLeft = 6810;
            this.axVSPrinter1.Table = string.Format("<4700;Releasing \n \n \n ");


            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY + 1280;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = string.Format("^11000;STANDARD PROCESSING TIME \n ");
            this.axVSPrinter1.FontBold = false;

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = string.Format("^11000;");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "^2750;New \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.Table = "^2750;Renewal \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.Table = "^3500;1-2 Days \n  ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.Table = "^3500;1 day \n  ";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);

            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n ";
            axVSPrinter1.DrawPicture(Properties.Resources.back_white, ".50in", "5.70in", "50%", "50%", 10, false);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 690;
            this.axVSPrinter1.MarginLeft = 1800;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<2500;Republic of the Philippines");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 890;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.Table = string.Format("<2500;PROVINCE OF {0}", AppSettingsManager.GetConfigValue("08"));

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 890;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 8500;
            this.axVSPrinter1.Table = ">2500;Location Sketch of";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1050;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 8500;
            this.axVSPrinter1.Table = ">2500;Business Establishment";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1050;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 1800;
            this.axVSPrinter1.Table = string.Format("<2500;{0}", AppSettingsManager.GetConfigValue("02"));
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1210;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.Table = string.Format("<2500;www.facebook.com/bpldmalolos");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1370;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.Table = string.Format("<2500;bpld@maloloscity.gov.ph");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.Table = string.Format("<2500;www.malolos.gov.ph");
            this.axVSPrinter1.FontUnderline = false;


            
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 6500;
            this.axVSPrinter1.Table = "<250; ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 7600;
            this.axVSPrinter1.Table = "<250; ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 9100;
            this.axVSPrinter1.Table = "<250; ";
            if (m_sApplType == "NEW")
            {
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                this.axVSPrinter1.CurrentY = lngY + 1530;
                this.axVSPrinter1.FontSize = (float)7;
                this.axVSPrinter1.MarginLeft = 6500;
                this.axVSPrinter1.Table = "<250;X";
            }
            if (m_sApplType == "REN")
            {
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                this.axVSPrinter1.CurrentY = lngY + 1530;
                this.axVSPrinter1.FontSize = (float)7;
                this.axVSPrinter1.MarginLeft = 7600;
                this.axVSPrinter1.Table = "<250;X";
            }
            if (m_sApplType == "RET")
            {
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                this.axVSPrinter1.CurrentY = lngY + 1530;
                this.axVSPrinter1.FontSize = (float)7;
                this.axVSPrinter1.MarginLeft = 9100;
                this.axVSPrinter1.Table = "<250; ";
            }
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 6750;
            this.axVSPrinter1.Table = "<500; NEW";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 7850;
            this.axVSPrinter1.Table = "<900; RENEWAL";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 9350;
            this.axVSPrinter1.Table = "<900; CLOSURE";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY + 350;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "<1500|<250|<9250; Business Name|:|" + AppSettingsManager.GetBnsName(m_sBIN);
            this.axVSPrinter1.Table = "<1500|<250|<9250; Business Address|:|" + m_sBnsHse + " " + m_sBnsStr + " " + m_sBnsBrgy + " " + m_sBnsMun;
            GetOwnerDetails(sOwn_Code);
            this.axVSPrinter1.Table = "<1500|<250|<9250; Owner's Name|:|" + m_sOwnLn + " " + m_sOwnFn + " " + m_sOwnMi;
            this.axVSPrinter1.Table = "<1500|<250|<9250; Owner's Address|:|"  + m_sOwnHse + " " + m_sOwnStr + " " + m_sOwnBrgy + " " + m_sOwnMun;

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 150;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "^5500|^5500;Sketched by: |Date";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 500;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "^5500|^5500;_________________________|_________________________";
            this.axVSPrinter1.Table = "^5500|^5500;Signature over Printed Name|";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n ";
            

            #region comments
            //lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.MarginLeft = 6000;
            //this.axVSPrinter1.Table = string.Format("^5000;Printed and verified by:");
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.FontSize = (float)10;
            //this.axVSPrinter1.Table = string.Format("^5000;________________________________");
            //this.axVSPrinter1.Table = string.Format("^5000;BPLO STAFF");
            //this.axVSPrinter1.FontItalic = true;
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            ////this.axVSPrinter1.Table = "<3000;*SEE ATTACHED SOA";
            //this.axVSPrinter1.FontItalic = false;
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            ////this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n \n";
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.FontSize = (float)9;
            //if (sSupple != "")
            //{
            //    this.axVSPrinter1.NewPage();
            //    this.axVSPrinter1.Table = string.Format("^10000;SUPPLEMENTAL PAGE");
            //    this.axVSPrinter1.Paragraph = "";

            //    this.axVSPrinter1.Table = "<10000;3. BUSINESS ACTIVITY";
            //    this.axVSPrinter1.FontBold = false;

            //    this.axVSPrinter1.Table = "^5000|^1500|^1500|^3000;||Capitalization|Gross/Sales Receipts (for Renewal)";
            //    this.axVSPrinter1.Table = "^5000|^1500|^1500|^1500|^1500;Line of Business|No. of Units|(for New Business)|Essential|Non-Essential";
            //    this.axVSPrinter1.Table = string.Format("<5000|<1500|<1500|<1500|<1500;{0}", sSupple);
            //}
            //this.axVSPrinter1.MarginLeft = 9000;
            ////this.axVSPrinter1.FontItalic = true;
            ////this.axVSPrinter1.FontBold = true;
            ////this.axVSPrinter1.Table = "<3000;*SEE ATTACHED SOA";
            ////this.axVSPrinter1.FontItalic = false;
            ////this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "<5500|>5500;      2.   ASSESSMENT OF APPLICABLE FEES|*SEE ATTACHED SOA";
            //this.axVSPrinter1.FontBold = false;

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "^3500|^2500|^2500|^2500;Local Taxes|Amount Due|Penalty/Surcharge|Total";
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Gross Sales Tax|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Tax on Storage for Combustible / Flammable or Explosive Substance|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Tax on Signboard / Billboards|||";
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "<11000;REGULATORY FEES AND CHARGES";
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Mayor's Permit Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Garbage Charges|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Delivery Truck / Vans Permit Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Sanitary Inspection Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Building Inspection Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Electrical Inspection Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Signboard / Billboard Renewal Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Signboard / Billboard Renewal Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Storage and Sale of Combustible / Flammable or Explosive Substance|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Others";
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = ">3500|^2500|^2500|^2500;TOTAL FEES for LGU";
            //this.axVSPrinter1.Table = ">3500|^2500|^2500|^2500;FIRE SAFETY INSPECTION FEE (10%)";
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            ////this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;

            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.Table = "^500|<2500|<4000|<4000;|Assessed By: MTO||FSIF Assessment Approved by: BFP|";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.Table = "<3500|<3000|<4500;____________________________||___________________________________";
            //this.axVSPrinter1.FontBold = false;

            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "<11000;III.  CITY / MUNICIPALITY FIRE STATION SECTION";
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.Table = "^500|>10000|^500;|DATE:____________________|";
            //this.axVSPrinter1.Table = "<11000;Application No.:_________________________";
            //this.axVSPrinter1.Table = "<11000; ";
            //this.axVSPrinter1.Table = "^500|<10000|^500;|(TO BE FILLED UP BY APPLICANT/OWNER)|";
            //this.axVSPrinter1.Table = "<11000; ";
            //this.axVSPrinter1.Table = "<11000;Name of Applicant:__________________________________________________________________";
            //this.axVSPrinter1.Table = "^500|<10000|^500; ";
            //this.axVSPrinter1.Table = "<11000;Name of Business:__________________________________________________________________";
            //this.axVSPrinter1.Table = "^500|<10000|^500; ";
            //this.axVSPrinter1.Table = "<5500|<5500;Total Floor Area:_________________________|Contact No._______________________________";
            //this.axVSPrinter1.Table = "^500|<10000|^500; ";
            //this.axVSPrinter1.Table = "<11000;Address of Establishment:____________________________________________________________";
            //this.axVSPrinter1.Table = "^500|<10000|^500; ";
            //this.axVSPrinter1.Table = "^500|<10000|^500; ";
            //this.axVSPrinter1.Table = "^500|<10000|^500;|__________________________________|";
            //this.axVSPrinter1.Table = "^500|<10000|^500;|           Signature of Applicant/Owner|";
            //this.axVSPrinter1.Table = "^500|<10000|^500;||";
            //lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.Table = "^500|<10000|^500;|Certified By:|";
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.MarginLeft = 700;
            //this.axVSPrinter1.Table = "^250; ";
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.Table = "^500|<10000|^500;|Customer Relations Officer:|";
            //this.axVSPrinter1.Table = "^500|<10000|^500;|Time And Date Recieved: ______________________|";

            //this.axVSPrinter1.CurrentY = lngY;

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            //this.axVSPrinter1.MarginLeft = 6500;
            //this.axVSPrinter1.Table = "<2500|<2500;FIRE SAFETY INSPECTION \nFEE ASSESSMENT: \n |";
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.MarginLeft = 500;
            ////this.axVSPrinter1.FontItalic = true;
            ////this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.FontSize = (float)8.0;
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Table = "<11000;Important Notice: As per Section 12 of the Implementing Rules and Regulations of the Fire Code of 2008, certain establishments (e.g. building lessors, fire, earthquake, and explosion hazard insurance companies, and vendors of fire fighting equipment, appliances and devices) may be required to pay additional charges and fees other than the Fire Safety Inspection Fees. These Shall be collected during inspections or in another process to be communicated by representatives of the Bureau of Fire Protection (BFP).";
            ////this.axVSPrinter1.FontItalic = false;
            //this.axVSPrinter1.FontBold = false;

            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.CurrentY = lngY + 1000;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n \n";
            //this.axVSPrinter1.CurrentY = lngY + 1000;

            //int cnt = 0;
            //if (sSupple != "")
            //{
            #endregion
            if(arrSuplementaryDesc.Count != 0) //JARS 20171006 QA, ONLY PRINT SUPPLEMENTARY WHEN NEEDED
            {
                this.axVSPrinter1.NewPage();
                lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
                this.axVSPrinter1.CurrentY = lngY + 1000;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                this.axVSPrinter1.FontSize = (float)12;
                this.axVSPrinter1.Table = string.Format("^11000;SUPPLEMENTAL PAGE");
                //this.axVSPrinter1.Paragraph = "";
                lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                this.axVSPrinter1.CurrentY = lngY;
                this.axVSPrinter1.FontSize = (float)9;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = "<11000; ";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.CurrentY = lngY;
                this.axVSPrinter1.FontSize = (float)9;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = "<800|<10200;|3. BUSINESS ACTIVITY";
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.CurrentY = lngY + 220;
                this.axVSPrinter1.FontSize = (float)9;
                this.axVSPrinter1.Table = "^3500|^2000|^2000;Line of Business|No. of Units|Capitalization \n(for New Business)";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.CurrentY = lngY + 220;
                this.axVSPrinter1.FontSize = (float)9;
                this.axVSPrinter1.MarginLeft = 8000;
                this.axVSPrinter1.Table = "^3500;Gross Sales Reciepts (for Renewal)";
                this.axVSPrinter1.Table = "^1750|^1750;Essential|Non - Essential";
                lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.CurrentY = lngY;
                this.axVSPrinter1.FontSize = (float)9;
                this.axVSPrinter1.MarginLeft = 500;
                //this.axVSPrinter1.Table = "^5000|^1500|^1500|^3000;||Capitalization|Gross/Sales Receipts (for Renewal)";
                //this.axVSPrinter1.Table = "^5000|^1500|^1500|^1500|^1500;Line of Business|No. of Units|(for New Business)|Essential|Non-Essential";
                //    //this.axVSPrinter1.Table = string.Format("<5000|<1500|<1500|<1500|<1500;{0}", sSupple);
                //    cnt++;
                //}
                if (arrSuplementaryDesc.Count != 0)
                {
                    for (int i = 0; i < arrSuplementaryDesc.Count; i++)
                    {
                        //this.axVSPrinter1.Table = string.Format("<3500|<2000|<2000|<1750|<1750;{0}", arrSuplementaryDesc[i].ToString());
                        this.axVSPrinter1.Table = string.Format("<3500|<2000|>2000|>1750|>1750;{0}", arrSuplementaryDesc[i].ToString());    // RMC 20180103 display prev cap/ gross in application form, requested thru Jester
                    }
                }
                this.axVSPrinter1.MarginLeft = 9000;
            }
            this.axVSPrinter1.EndDoc();

        }

        private void PrintAppForm_MalolosV2_BLANK()
        {
            // RMC 20161129 customized application form for Binan
            this.axVSPrinter1.CurrentY = 350;
            long lngY;
            long lngY2;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.MarginLeft = 800; //JARS 20170331
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            //this.axVSPrinter1.Table = "<11000;BPLO-001-0";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.DrawPicture(Resources.unnamed, "0.4in", "0.4in", "15%", "16%", 11, false);

            this.axVSPrinter1.MarginLeft = 500; //JARS 20170331
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontSize = (float)10;
            this.axVSPrinter1.Table = "^11000;APPLICATION FORM FOR BUSINESS PERMIT";
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = "^11000;TAX YEAR " + AppSettingsManager.GetConfigValue("12");
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetConfigValue("41"); //JARS 20170331
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = "^11000;" + AppSettingsManager.GetConfigValue("09");

            this.axVSPrinter1.FontBold = false;

            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.MarginLeft = 1800;
            //this.axVSPrinter1.FontSize = (float)8;

            //this.axVSPrinter1.Table = "<11000;Republic of the Philippines";
            //this.axVSPrinter1.Table = "<11000;Province of " + AppSettingsManager.GetConfigValue("08");
            //this.axVSPrinter1.Table = "<11000;" + AppSettingsManager.GetConfigValue("09"); //JARS 20170331

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.MarginLeft = 500;

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;

            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.FontBold = true;
            string strData = string.Empty;
            strData = "INSTRUCTIONS:\n";
            strData += "1. Provide accurate information and print legibly to avoid delays. Incomplete application form will be returned to the applicant.\n";
            strData += "2. Ensure that all documents attached to this form (if any) are complete and properly filled out.";
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = "<11000;I. APPLICANT SECTION";
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            #region comments
            //this.axVSPrinter1.Table = "<800|<2000|>3500;|1. Application No :|BIN :"; //JARS 20170331
            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.FontBold = false;
            #endregion
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            #region comments
            //this.axVSPrinter1.MarginLeft = 2500;
            //this.axVSPrinter1.Table = "^1500; "; //JARS 20170331
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.MarginLeft = 10500;
            //this.axVSPrinter1.Table = "^1000; "; //JARS 20170331
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.MarginLeft = 5700;
            //this.axVSPrinter1.Table = "^1500; "; // JAV 20170407 Add box 
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.MarginLeft = 10500;
            //this.axVSPrinter1.Table = "^2500;"; //JARS 20170331
            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "<5500|>5500;1. BASIC INFORMATION|BIN: " + m_sBIN;
            #endregion
            this.axVSPrinter1.Table = "<800|<5500|<2700|<2000;|1. BASIC INFORMATION||BIN: "; // JAV 20170815
            //this.axVSPrinter1.Table = "<11000;1. INFORMATION";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)8;
            string sLastPaidYear = string.Empty;
            string sLastPaidQtr = string.Empty;
            string sLastBnsStat = string.Empty;
            AppSettingsManager.GetLastPaymentInfo(m_sBIN, AppSettingsManager.GetLastPaymentInfo(m_sBIN, "OR"), out sLastPaidYear, out sLastPaidQtr, out sLastBnsStat);

            this.axVSPrinter1.FontBold = false;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = lngY;

            OracleResultSet pRec = new OracleResultSet();
            #region comments
            //pRec.Query = "select * from business_que where bin = '" + m_sBIN + "' order by tax_year desc";
            //if (pRec.Execute())
            //{
            //    if (pRec.Read())
            //    {
            //        m_sApplType = pRec.GetString("bns_stat");
            //        sAppDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dt_applied"));
            //    }
            //    else
            //    {
            //        pRec.Close();
            //        pRec.Query = "select * from businesses where bin = '" + m_sBIN + "'";
            //        if (pRec.Execute())
            //        {
            //            if (pRec.Read())
            //            {
            //                m_sApplType = "REN";
            //                sAppDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
            //            }
            //        }
            //    }
            //}
            //pRec.Close();
            #endregion
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            #region comments
            //if (m_sApplType == "NEW")
            //    this.axVSPrinter1.Table = "<1000|<1500|<500|<5000;  X |||";
            //else
            //    this.axVSPrinter1.Table = "<1000|<1500|<500|<5000; |  X  ||";
            #endregion
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Table = "^500|^450|<1200|^450|<1200|<1750|<450|<1300|<450|<1800|<450|^1000;||New||Renewal|Mode of Payment:||Annually||Semi-Annually||Quarterly";
            //if (m_sApplType == "NEW") //JARS 20170331
            //    this.axVSPrinter1.Table = "^500|^450|<1200|^450|<1200|<1750|<450|<1300|<450|<1800|<450|^1000;|X|New||Renewal|Mode of Payment:||Annually||Semi-Annually||Quarterly"; 
            //else if (m_sApplType == "REN")
            //    this.axVSPrinter1.Table = "^500|^450|<1200|^450|<1200|<1750|<450|<1300|<450|<1800|<450|^1000;||New|X|Renewal|Mode of Payment:||Annually||Semi-Annually||Quarterly"; 
            lngY2 = Convert.ToInt64(axVSPrinter1.CurrentY);
            //CHECKBOXES
            this.axVSPrinter1.FontSize = (float)6;
            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 2650;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 6050;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 7800;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 10050;
            this.axVSPrinter1.Table = "^250; ";


            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            if (m_sApplType == "NEW") //JARS 20170331
            {
                this.axVSPrinter1.MarginLeft = 900;
                this.axVSPrinter1.Table = "^450;X";
            }
            else if (m_sApplType == "REN")
            {
                this.axVSPrinter1.MarginLeft = 2550;
                this.axVSPrinter1.Table = "^450;X";
            }
            this.axVSPrinter1.MarginLeft = 500;
            
            this.axVSPrinter1.CurrentY = lngY2;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<5500|<5500;Date of Application:|DTI/SEC/CDA Registration Date:  ";
            this.axVSPrinter1.Table = "<5500|<5500;TIN No.:  |DTI/SEC/CDA Registration No.:  ";
            //this.axVSPrinter1.Table = "<5500|<5500;  |PhilHealth:  ";
            //this.axVSPrinter1.Table = "<5500|<5500;SSS:  |Pag-ibig:  ";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            #region comments
            /*this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;Type of Business:| ___ Single| ___ Partnership| ___ Corporation| ___ Cooperative";
            this.axVSPrinter1.CurrentY = lngY;

            if(sOrgKind == "SINGLE PROPRIETORSHIP")
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;|  X  | | | ";
            else if (sOrgKind == "PARTNERSHIP")
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;| |  X  | | ";
            else if (sOrgKind == "CORPORATION")
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;| | |  X  | ";
            else 
                this.axVSPrinter1.Table = "<2000|<2000|<2000|<2000|<3000;| | | |  X  ";*/
            //if (sOrgKind == "SINGLE PROPRIETORSHIP")
            //    this.axVSPrinter1.Table = "<11000;Type of Business: X Single     Partnership       Corporation       Cooperative";
            //else if (sOrgKind == "PARTNERSHIP")
            //    this.axVSPrinter1.Table = "<11000;Type of Business:   Single   X Partnership       Corporation       Cooperative";
            //else if (sOrgKind == "CORPORATION")
            //    this.axVSPrinter1.Table = "<11000;Type of Business:   Single     Partnership     X Corporation       Cooperative";
            //else
            //    this.axVSPrinter1.Table = "<11000;Type of Business:   Single     Partnership       Corporation     X Cooperative"; 
            //if (sOrgKind == "SINGLE PROPRIETORSHIP") //JARS 20170331
            //    this.axVSPrinter1.Table = "<2000|^450|<1700|^450|<1700|^450|<1700|^450|<2100;Type of Business:|X|Single||Partnership||Corporation||Cooperative";
            //else if (sOrgKind == "PARTNERSHIP")
            //    this.axVSPrinter1.Table = "<2000|^450|<1700|^450|<1700|^450|<1700|^450|<2100;Type of Business:||Single|X|Partnership||Corporation||Cooperative";
            //else if (sOrgKind == "CORPORATION")
            //    this.axVSPrinter1.Table = "<2000|^450|<1700|^450|<1700|^450|<1700|^450|<2100;Type of Business:||Single||Partnership|X|Corporation||Cooperative";
            //else
            //    this.axVSPrinter1.Table = "<2000|^450|<1700|^450|<1700|^450|<1700|^450|<2100;Type of Business:||Single||Partnership||Corporation|X|Cooperative";
            #endregion
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;

            //CHECKBOXES
            this.axVSPrinter1.FontSize = (float)6;
            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 2500;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 4650;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 6800;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 8950;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Type of Business:||Single||Partnership||Corporation||Cooperative";
            #region comments
            //if (sOrgKind == "SINGLE PROPRIETORSHIP") //JARS 20170331
            //    this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Type of Business:|X|Single||Partnership||Corporation||Cooperative";
            //else if (sOrgKind == "PARTNERSHIP")
            //    this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Type of Business:||Single|X|Partnership||Corporation||Cooperative";
            //else if (sOrgKind == "CORPORATION")
            //    this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Type of Business:||Single||Partnership|X|Corporation||Cooperative";
            //else
            //    this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Type of Business:||Single||Partnership||Corporation|X|Cooperative";
            #endregion
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //CHECKBOXES
            this.axVSPrinter1.FontSize = (float)6;
            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 2500;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 4650;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 6800;
            this.axVSPrinter1.Table = "^250; ";

            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 8950;
            //this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Amendment From:||Single||Partnership||Corporation||Cooperative";
            this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;Amendment From:||Single||Partnership||Corporation";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //CHECKBOXES
            this.axVSPrinter1.FontSize = (float)6;
            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 2500;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 4650;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 6800;
            this.axVSPrinter1.Table = "^250; ";

            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 8950;
            //this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;                       To:||Single||Partnership||Corporation||Cooperative";
            this.axVSPrinter1.Table = "<1950|^450|<1650|^450|<1650|^450|<1650|^450|<2300;                       To:||Single||Partnership||Corporation";

            //this.axVSPrinter1.Table = "<11000;Are you enjoying tax incentive from any Government Entity? ___ Yes   ___ No  Please specify the entity?";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //CHECKBOXES
            this.axVSPrinter1.FontSize = (float)6;
            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 5500;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.CurrentY = lngY + 30;
            this.axVSPrinter1.MarginLeft = 6450;
            this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = "<5000|<450|<500|<450|<500|<4100;Are you enjoying tax incentive from any Government Entity?||Yes||No|Please specify the entity?"; //JARS 20170331

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.Table = "^11000;Name of Taxpayer/Registrant \n";
            this.axVSPrinter1.Table = "^11000;Name of Taxpayer/Registrant";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns; // JAV 20170407
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox; // JAV 20170407
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.Table = "<1600|<3900|<1500|<2000|<1500|<500;Last Name: ||First Name: ||Middle Name: |"; // JAV 20170410 Adjust the names space and remove the border
            //this.axVSPrinter1.Table = "<1600|<2900|<1500|<3000|<1500|<500;Last Name: ||First Name: ||Middle Name: |"; // JAV 20170410 Adjust the names space and remove the border
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<3666|<3666|<3666;Last Name: |First Name: |Middle Name: ";
            lngY2 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "<1600|<3900|<1500|<3000|<500|<500;|" + m_sOwnLn + "||" + m_sOwnFn + "||" + m_sOwnMi; // JAV 20170410 Adjust the names space and remove the border
            //this.axVSPrinter1.Table = "<1100|<3900|<600|<3000|<1650|<500;|" + m_sOwnLn + "||" + m_sOwnFn + "||" + m_sOwnMi; // JAV 20170410 Adjust the names space and remove the border
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            ////CHECKBOXES
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            #region comments
            //this.axVSPrinter1.FontSize = (float)6;
            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 8600;
            //this.axVSPrinter1.Table = "^250; ";

            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 10050;
            //this.axVSPrinter1.Table = "^250; ";

            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.FontSize = (float)10;
            #endregion
            this.axVSPrinter1.CurrentY = lngY2;
            #region comments
            ////this.axVSPrinter1.Table = "<4300|<2700|<1000|^450|<1000|^450|<1100;|Birthdate: " + m_sBdate + "|Gender:||Male||Female";
            //this.axVSPrinter1.Table = "<4300|2600|<1000|^600|<700|^800|<1000;Birthdate:" + m_sBdate + "|Nationality:|Gender:||Male||Female"; // JAV 20170407
            //this.axVSPrinter1.CurrentY = lngY2;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.Table = "<4300|<2700|<1000|^450|<1000|^450|<1100;||||||";
            //if (m_sGender == "MALE")
            //{
            //    this.axVSPrinter1.Table = "<4300|<2700|<1000|^450|<1000|^450|<1100;|||X|||";
            //}
            //else
            //{
            //    this.axVSPrinter1.Table = "<4300|<2700|<1000|^450|<1000|^450|<1100;|||||X|"; //JAV 20170407 Remove the X mark in gender
            //}
            #endregion
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<1700|<9300;Business Name: |";
            lngY2 = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<1700|<9300;|" + AppSettingsManager.GetBnsName(m_sBIN);
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //CHECKBOXES
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.FontSize = (float)6;
            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 6000;
            //this.axVSPrinter1.Table = "^250; ";

            //this.axVSPrinter1.CurrentY = lngY + 30;
            //this.axVSPrinter1.MarginLeft = 8750;
            //this.axVSPrinter1.Table = "^250; ";

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.CurrentY = lngY2;
            this.axVSPrinter1.Table = "<5500|<450|<2300|<450|<2300;Trade Name/ Franchise: ||Main||Branch";
            lngY2 = Convert.ToInt64(axVSPrinter1.CurrentY);

            this.axVSPrinter1.CurrentY = lngY2;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)9;
            #region comments
            //this.axVSPrinter1.Table = "<11000;2. OTHER INFORMATION";

            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.FontSize = (float)8;
            //this.axVSPrinter1.Table = "<11000;Note: For renewal applications, do not fill up this section unless certain information have changed.";
            #endregion
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = "<800|<10200;|2. OTHER INFORMATION \n   Note: For renewal applications, do not fill up this section unless certain information have changed.";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            #region comments
            //this.axVSPrinter1.Table = "<11000;Business Address: " + m_sBnsHse + " " + m_sBnsStr + " " + m_sBnsStr + " " + m_sBnsBrgy + " " + m_sBnsMun;

            //this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Postal Code:||Email Address:|" + m_sEmail;
            //this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Telephone No.:|" + m_sTelNo + "|Mobile No.:|";
            //this.axVSPrinter1.Table = "<2000|<9000;Owner's Address:|" + m_sOwnHse + " " + m_sOwnStr + " " + m_sOwnBrgy + " " + m_sOwnMun + " " + m_sOwnProv;
            //this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Postal Code:||Email Address:|" + m_sOwnEmailAdd;
            //this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Telephone No.:|" + m_sOwnTelNo + "|Mobile No.:|";
            //this.axVSPrinter1.Table = "<11000;In case of emergency, provide name of contact person: ";
            //this.axVSPrinter1.Table = "<2000|<3500|<1500|<4000;Telephone/ Mobile No.:||Email Address:|";
            #endregion\
            #region comments
            ////JARS 20170331
            //this.axVSPrinter1.Table = "<1800|<3700|<1800|<3700;Business Address:||Owner's Address:|";
            //this.axVSPrinter1.Table = "<2000|<3500|<2000|<3500;House No./Bldg No.:|" + m_sBnsHse + "|House No./Bldg No.:|" + m_sOwnHse;
            //this.axVSPrinter1.Table = "<1800|<3700|<1800|<3700;Street:|" + m_sBnsStr + "|Street:|" + m_sOwnStr;
            //this.axVSPrinter1.Table = "<1800|<3700|<1800|<3700;Subdivision:||Subdivision:|";
            ////this.axVSPrinter1.Table = "<1800|<3700|<1800|<3700;Barangay:|" + m_sBnsBrgy + "   City/Mun: " + AppSettingsManager.GetConfigObject("02") + "|Barangay:|" + m_sOwnBrgy + "   City/Mun: " + m_sOwnMun; // JAV 20170407 Add column for City/Mun
            //this.axVSPrinter1.Table = "<1100|<1700|<1100|<1600|<1100|<1800|<1100|<1500;Barangay:|" + m_sBnsBrgy + "|City/Mun:|" + AppSettingsManager.GetConfigObject("02") + "|Barangay:|" + m_sOwnBrgy + "|City/Mun:|" + m_sOwnMun; // JAV 20170407 Add column for City/Mun
            //this.axVSPrinter1.Table = "<1800|<3700|<1800|<3700;Province:|" + AppSettingsManager.GetConfigObject("08") + "|Province:|" + m_sOwnProv;
            //this.axVSPrinter1.Table = "<2200|<3300|<2000|<3500;Telephone/Mobile No:|" + m_sOwnTelNo + "|Email Address:|";
            #endregion
            this.axVSPrinter1.Table = "<11000;Business Address: \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.Table = "<5500|<5500;Postal Code:|Email Address:";
            this.axVSPrinter1.Table = "<5500|<5500;Telephone No.:|Mobile No.:";
            this.axVSPrinter1.Table = "<11000;Owner's Address: \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.Table = "<5500|<5500;Postal Code:|Email Address:";
            this.axVSPrinter1.Table = "<5500|<5500;Telephone No.:|Mobile No.:";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.Table = "<11000;In case of emergency, provide name of contact person:";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.Table = "<5500|<5500;Telephone/Mobile No.:|Email Address:";

            


            string sTotEmp = string.Empty;
            int iMale = 0; int iFemale = 0;
            int.TryParse(m_sNoMale, out iMale);
            int.TryParse(m_sNoFemale, out iFemale);
            sTotEmp = string.Format("{0:###}", iMale + iFemale);
            #region comments
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns; // JAV 20170410 Change the tbBoxColumns to tbBox
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox; // JAV 20170410 Change the tbBoxColumns to tbBox

            //lngY = Convert.ToInt64(axVSPrinter1.CurrentY);// JAV 20170410 Add design
            //lngY2 = Convert.ToInt64(axVSPrinter1.CurrentY);// JAV 20170410 Add design

            //this.axVSPrinter1.FontSize = (float)12;
            //this.axVSPrinter1.CurrentY = lngY + 70;
            //this.axVSPrinter1.MarginLeft = 2645;
            //this.axVSPrinter1.Table = "^1000; "; // JAV 20170410  will display the box

            //this.axVSPrinter1.FontSize = (float)12;
            //this.axVSPrinter1.CurrentY = lngY + 70;
            //this.axVSPrinter1.MarginLeft = 6200;
            //this.axVSPrinter1.Table = "^1000; "; // JAV 20170410  will display the box

            //this.axVSPrinter1.FontSize = (float)12;
            //this.axVSPrinter1.CurrentY = lngY + 70;
            //this.axVSPrinter1.MarginLeft = 10350;
            //this.axVSPrinter1.Table = "^1000; "; // JAV 20170410  will display the box

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;// JAV 20170410

            //this.axVSPrinter1.FontSize = (float)9;
            //this.axVSPrinter1.CurrentY = lngY2 + 70;
            //this.axVSPrinter1.MarginLeft = 2645;
            //this.axVSPrinter1.Table = "<2000; " + m_sArea;// JAV 20170410 Will display the result

            //this.axVSPrinter1.FontSize = (float)9;
            //this.axVSPrinter1.CurrentY = lngY + 70;
            //this.axVSPrinter1.MarginLeft = 6200;
            //this.axVSPrinter1.Table = "^1000; " + sTotEmp; // JAV 20170410 Will display the result

            //this.axVSPrinter1.FontSize = (float)9;
            //this.axVSPrinter1.CurrentY = lngY + 70;
            //this.axVSPrinter1.MarginLeft = 10350;
            //this.axVSPrinter1.Table = "^1000; " + m_sNoGender;  // JAV 20170410 Will display the result

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;// JAV 20170410

            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.FontSize = (float)9;
            //this.axVSPrinter1.CurrentY = lngY;
            ////this.axVSPrinter1.Table = "<2000|<1000|<3000|<750|<3500|<750;Business Area (in sqm.)|" + m_sArea + "|Total No. of Employees in Establishment:|" + sTotEmp + "|No. of Employees Residing within LGU:|" + m_sNoGender; // JAV 20170410 Adjust the spacing
            //this.axVSPrinter1.Table = "<2100|<1300|<2500|<1300|<2600|<1200;Business Area (in sqm.)||Total No. of Employees in Establishment:||No. of Employees Residing within LGU:||"; // JAV 20170410 Adjust the spacing
            #endregion
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<3666|<3666|<3666;Business Area (in sqm.): \n |Total No. of Employees in Establishment: \n |No. of Employees Residing within LGU: \n "; // JAV 20170410 Adjust the spacing


            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = "<11000;Note: Fill Up Only if Business Place is Rented";
            this.axVSPrinter1.FontBold = false;
            GetOwnerDetails(sLessor);
            string sLessorNm = string.Empty;
            sLessorNm = m_sOwnLn;
            if (m_sOwnFn.Trim() != "")
                sLessorNm += ", " + m_sOwnFn;
            if (m_sOwnMi.Trim() != "")
                sLessorNm += " " + m_sOwnMi;
            #region comments
            //this.axVSPrinter1.Table = "<11000;Lessor's Full Name:   " + sLessorNm;
            //this.axVSPrinter1.Table = "<11000;Lessor's Full Address:   " + m_sOwnHse + " " + m_sOwnStr + " " + m_sOwnBrgy + " " + m_sOwnMun + " " + m_sOwnProv;
            //this.axVSPrinter1.Table = "<11000;Lessor's Fill Telephone/Mobile No.:   " + m_sOwnTelNo;
            //this.axVSPrinter1.Table = "<5500|<5500;Lessor's Email Address:   " + m_sOwnEmailAdd + "|Monthly Rental:   " + sRent;
            #endregion

            //JARS 20170331
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontSize = (float)8;
            //this.axVSPrinter1.Table = "<1800|<5000|<1500|<2700;Name of Lessor:|" + sLessorNm + "|Monthly Rental:|" + sRent;
            this.axVSPrinter1.Table = "<11000;Lessor's Full Name:";
            this.axVSPrinter1.Table = "<11000;Lessor's Full Address:";
            this.axVSPrinter1.Table = "<11000;Lessor's Telephone/Mobile No.:";
            this.axVSPrinter1.Table = "<11000;Lessor's Email Address:";
            this.axVSPrinter1.Table = "<11000;Monthly Rental";

            //this.axVSPrinter1.Table = "<2000|<2800|<1400|<4800;House No./Bldg No.:|" + m_sOwnHse + "|Street:|" + m_sOwnStr;
            //this.axVSPrinter1.Table = "<1800|<3000|<1400|<1800|<3000;Subdivision:||Barangay:|" + m_sOwnBrgy + "|Contact Person: ";
            //this.axVSPrinter1.Table = "<1800|<3000|<1400|<1800|<3000;City/Mun:|" + m_sOwnMun + "|Province:|" + m_sOwnProv + "|Tel Nos: ";
            //this.axVSPrinter1.Table = "<1800|<3000|<1600|<1600|<3000;Telephone:|" + m_sOwnTelNo + "|Email Address:||Email Address: ";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = "<800|<10200;|3. BUSINESS ACTIVITY";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)9;
            //this.axVSPrinter1.Table = "^4000|^2500|^1500|^3000;||Capitalization|Gross/Sales Receipts (for Renewal)";
            //this.axVSPrinter1.Table = "^4000|^2500|^1500|^1500|^1500;Line of Business||(for New Business)|Essential|Non-Essential";
            object yy = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            this.axVSPrinter1.FontSize = (float)8;
            //this.axVSPrinter1.Table = "^4000|^2000|^2000;Line of Business|No. of Units| Capitalization (for New Business)";
            //this.axVSPrinter1.Table = "^3500|^2000|^2000;Line of Business|Capitalization \n(for New Business)|Previous Gross"; //JARS 20170331
            this.axVSPrinter1.Table = "^3500|^2000|^2000;Line of Business|No of Units|Capitalization \n(for New Business)"; //JARS 20170331
            this.axVSPrinter1.CurrentY = yy;
            this.axVSPrinter1.MarginLeft = 8000;
            this.axVSPrinter1.Table = "^3500;Gross Sales Reciepts (for Renewal)";
            this.axVSPrinter1.Table = "^1750|^1750;Essential|Non - Essential";
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            //OracleResultSet pRec = new OracleResultSet();


            

            string sCap = string.Empty;
            string sEss = string.Empty;
            string sNonEss = string.Empty;
            string sSupple = string.Empty;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            int iAddlBns = 0;
            string sPrevCap = string.Empty;
            //sPrevCap = GetPrevCapGross(m_sBIN, m_sBnsCode);
            sGross = "";
            if (m_sApplType == "NEW")
            {
                strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + string.Format("{0:#,##0.00}", sPrevCap) + "|" + sGross + "||" + sNonEss;
            }
            else
            {
                strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + string.Format("{0:#,##0.00}", sPrevCap) + "||" + sGross + "|" + sNonEss;
            }

            this.axVSPrinter1.Table = string.Format("<3500|^2000|^2000|^1750|^1750;{0}", strData);
            object xY = this.axVSPrinter1.CurrentY;
            iAddlBns++;
            sPrevCap = "";
            pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' order by bns_code_main";
            if (pRec.Execute())
            {

                while (pRec.Read())
                {
                    iAddlBns++;
                    m_sBnsCode = pRec.GetString("bns_code_main");
                    sPrevCap = GetPrevCapGross(m_sBIN, m_sBnsCode);

                    if (pRec.GetString("bns_stat").Trim() == "NEW")
                        sCap = string.Format("{0:#,###.00}", pRec.GetDouble("capital"));
                    else
                        sCap = string.Format("{0:#,###.00}", pRec.GetDouble("gross"));

                    if (Convert.ToDouble(sCap) == 0)
                        sCap = "";

                    sGross = "0";
                    if (ValidateEssential(m_sBnsCode))
                    {
                        sEss = sGross;
                        sNonEss = "0";
                    }
                    else
                    {
                        sEss = "0";
                        sNonEss = sGross;
                    }

                    if (Convert.ToDouble(sNonEss) == 0)
                        sNonEss = "";

                    if (Convert.ToDouble(sEss) == 0)
                        sEss = "";

                    if (iAddlBns > 5)
                    {
                        sSupple = "\n" + AppSettingsManager.GetBnsDesc(m_sBnsCode) + "||" + sCap + "|" + sEss + "|" + sNonEss;
                    }
                    else
                    {
                        strData = AppSettingsManager.GetBnsDesc(m_sBnsCode) + "|" + sPrevCap + "|" + sCap + "|" + sEss + "|" + sNonEss;
                        this.axVSPrinter1.Table = string.Format("<3500|^2000|^2000|^1750|^1750;{0}", strData);
                    }
                }

                if (iAddlBns == 0)
                    iAddlBns++;

                
                for (int ix = iAddlBns; ix <= 2; ix++)
                {
                    strData = "||||";
                    if (ix == 2)
                    {
                        xY = this.axVSPrinter1.CurrentY;
                    }
                    this.axVSPrinter1.Table = string.Format("<3500|^2000|^2000|^1750|^1750;{0}", strData);
                }

            }
            pRec.Close();

            if (Convert.ToInt32(xY) > 0)
            {
                //this.axVSPrinter1.Table = "<11000;Bootttt:";
                //return;
            }

            this.axVSPrinter1.CurrentY = xY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
            axVSPrinter1.Table = "<3500|^2000|^2000|^1750|^1750;||||";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            axVSPrinter1.Table = "<11000; \n \n \n \n \n \n \n \n \n \n \n \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = xY;
            axVSPrinter1.Table = "<3500|^2000|^2000|^1750|^1750;||||";
            axVSPrinter1.Table = "<3500|^2000|^2000|^1750|^1750;||||";
            strData = "I DECLARE UNDER PENALTY OF PERJURY that the foregoing information are true based on my personal knowledge and ";
            strData += "authentic records. Further, I agree to comply with the regulatory requirement and other deficiencies within 30 days from ";
            strData += "release of the business permit.";
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);  //JAV 20170410 Change the paragraph indention

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;

            //this.axVSPrinter1.Table = "<10000;Last Payment: " + sLastPaidYear + " " + sLastPaidQtr + " " + sLastBnsStat;

            //strData = "I DECLARE UNDER PENALTY OF PERJURY that the foregoing information are true based on my personal knowledge and ";
            //strData += "authentic records. Further, I agree to comply with the regulatory requirement and other deficiencies within 30 days from ";
            //strData += "release of the business permit.";
            ////strData += "and I am aware that non-compliance within the prescribed period would be sufficient ground ";
            ////strData += "for cancellation of my business permit and closure of my business establishments.";
            ////this.axVSPrinter1.Table = string.Format("^11000;{0}", strData);  //JAV 20170410 Change the paragraph indention
            //this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);  //JAV 20170410 Change the paragraph indention
            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.MarginLeft = 5000;
            
            //this.axVSPrinter1.DrawPicture(Resources.LubaoContact, "0.5in", "9.5in", "75%", "75%", 11, false);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^6000;_________________________________________________");
            this.axVSPrinter1.Table = string.Format("^6000;SIGNATURE OF APPLICANT/TAXPAYER OVER PRINTED NAME");
            this.axVSPrinter1.Paragraph = "";
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("^6000;________________________________________");
            this.axVSPrinter1.Table = string.Format("^6000;POSITION/TITLE");
            this.axVSPrinter1.MarginLeft = 500;

            //xY = this.axVSPrinter1.CurrentY; 
            //int aa = Convert.ToInt32(xY);
            //aa += 340;
            //xY = (object) aa;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.CurrentY = xY;
            //this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = lngY + 600;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "<11000;Application Form for Business Permit";
            
            this.axVSPrinter1.Table = "<11000;II. LGU SECTION ( Do Not Fill Up This Section )";

            this.axVSPrinter1.Table = "<800|<10200;|1. VERIFICATION OF DOCUMENTS";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "^4000|^4000|^1000|^1000|^1000;Description|Office/Agency|Yes|No|Not Needed";
            this.axVSPrinter1.FontSize = (float)8;
            
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Occupancy Permit(For New)|Office of the  Building Official|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Barangay Clearance(For Renewal)|Barangay|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Sanitary Permit/Health Clearance|City Health Office|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;City of Environmental Certificate|Bulacan Environmental and Natural Resources Office|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Market Clearance(For Stall Holders)|Malolos Public Market|||";
            this.axVSPrinter1.Table = "<4000|<4000|<1000|<1000|<1000;Valid Fire Safety Inspection Certificate|Bureau of Fire Protection|||";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = string.Format("<11000;Customer Feedback: Kamusta ang naging serbisyo ng BPLO sa inyo? Bilugan ang sagot.");
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)12;
            this.axVSPrinter1.CurrentY = lngY + 250;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<330|<1800|<330|<1300|<330|<1300|<330|<1300|<330|<3750;O||O||O||O||O|";
            this.axVSPrinter1.CurrentY = lngY + 300;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "<330|<1800|<330|<1300|<330|<1300|<330|<1300|<330|<3750;|Lubos na nasiyahan||Nasiyahan||Nakuntento||Di Nasiyahan||Lubos na nadismaya";
           
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.CurrentY = lngY + 1000;
            this.axVSPrinter1.Table = string.Format("<5500|<5500;Komentaryo/Mungkahi:________________________________|                                        ");
            this.axVSPrinter1.CurrentY = lngY + 1000;
            this.axVSPrinter1.Table = string.Format("^7200|<3800;|Verified by: BPLO");
            this.axVSPrinter1.Table = string.Format("^6500|^4500;|________________________________");
            
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.CurrentY = lngY + 1500;// AFM 20210616 previously 2000
            this.axVSPrinter1.Table = string.Format("<11000;Every risk is worth taking as long as it's for a good cause, and contributes to a good life");
            
            this.axVSPrinter1.Table = "<11000;You can register at " + AppSettingsManager.GetConfigValue("76") + " and apply online for renewal"; //MCR 20210616
            this.axVSPrinter1.FontItalic = true;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontItalic = false;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n \n \n \n \n \n \n \n";
            this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n \n \n \n \n \n";
           

            this.axVSPrinter1.NewPage(); //PAGE 2
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            #region not confirmed yet
            /**
            String sBrgyClearanceDate = "";
            String sBrgyClearanceBy = "";
            String sZoningClearanceDate = "";
            String sZoningClearanceBy = "";
            String sSanitaryClearanceDate = "";
            String sSanitaryClearanceBy = "";
            String sOccClearanceDate = "";
            String sOccClearanceBy = "";
            String sAnnualClearanceDate = "";
            String sAnnualClearanceBy = "";
            String sFireClearanceDate = "";
            String sFireClearanceBy = "";
            String sOtherDate = "";
            String sOtherBy = "";

            pRec.Query = "select * from Permit_Type where bin = '" + m_sBIN + "' and current_year = '" + sTaxYear + "'";
            if (pRec.Execute())
                while (pRec.Read())
                {
                    if (pRec.GetString("perm_type") == "Sanitary")
                    {
                        sSanitaryClearanceDate = "X";
                        sSanitaryClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "ZONING")
                    {
                        sZoningClearanceDate = pRec.GetString("issued_date");
                        sZoningClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "Annual Inspection" || pRec.GetString("perm_type") == "ANNUAL")
                    {
                        sAnnualClearanceDate = pRec.GetString("issued_date");
                        sAnnualClearanceBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                    else if (pRec.GetString("perm_type") == "WORKING")
                    {
                        sOtherDate = pRec.GetString("issued_date");
                        sOtherBy = AppSettingsManager.GetUserName(pRec.GetString("user_code"));
                    }
                }
            pRec.Close();
            **/
            #endregion

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^11000;LIST OF REQUIREMENTS \n ";
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "^11000;REQUIREMENTS FOR ASSESSMENTS \n ";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^3666|^3666|^3666;NEW \n |RENEWAL \n |RETIREMENT \n ");
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;DTI/SEC/CDA|Basis for Computing Tax|Original Business Permit/Official Receipt");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;Barangay Business Clearance|(BIR Return, Gross Receipts, Book of Sales, POS Report)|Business Plate");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;Occupancy Permit|Barangay Business Clearance|Barangay Business Closure");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;TCT/Contract of Lease(if renting)|Fire Safety Inspection Certificate|Affidavit/Broad Resolution of Closure");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;Basis for Computing Tax(Capital)|CNC(Certificate of non-coverage)|Basis for Computing Tax");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;CNC(Certificate of non-coverage)||");
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;CNC(Certificate of non-coverage)|SSS|Authorization Letter if Representative"); // RMC 20180104 added SSS, Pagibig, Philhealth in App form
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;Sworn Statement/Notarized|Pag-ibig|Photo Copy of Owners ID and Representative");   // RMC 20180104 added SSS, Pagibig, Philhealth in App form
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;Owner's ID|Philhealth|");    // RMC 20180104 added SSS, Pagibig, Philhealth in App form
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;|Authorization Letter if Representative|");    // RMC 20180109 additional requirements in application form
            this.axVSPrinter1.Table = string.Format("<3666|<3666|<3666;|Photo Copy of Owners ID and Representative|");    // RMC 20180109 additional requirements in application form
            
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = string.Format("^11000;STANDARD STEPS");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = string.Format("^5500|^5500;NEW & RENEWAL|RETIREMENT");
            this.axVSPrinter1.FontBold = false;
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.Table = string.Format("<800;Step 1: \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = "<5500; \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.MarginLeft = 1300;
            this.axVSPrinter1.Table = string.Format("<4700;Application Filing & verification-Submission of complete application form with attached documentary require and one-time verification. \n ");  
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 6010;
            this.axVSPrinter1.Table = string.Format("<800;Step 1: \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = "<5500; \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.MarginLeft = 6810;
            this.axVSPrinter1.Table = string.Format("<4700;Filing and Verification/Inspection of Application and Requirements \n  ");

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = string.Format("<800;Step 2: \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.Table = "<5500; \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.MarginLeft = 1300;
            this.axVSPrinter1.Table = string.Format("<4700;Assessment one-time assessment of taxes, fees, and charges \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 6010;
            this.axVSPrinter1.Table = string.Format("<800;Step 2: \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.Table = "<5500; \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.MarginLeft = 6810;
            this.axVSPrinter1.Table = string.Format("<4700;Payment \n ");

            
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = string.Format("<800;Step 3: \n \n \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.Table = "<5500; \n \n \n  ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.MarginLeft = 1300;
            this.axVSPrinter1.Table = string.Format("<4700;Pay and claim one-time payment of taxes, fees, and charges receipt of Official Receipt as proof of payment of taxes, fees, and charges imposed by the city/municipality and BFP in securing clearance. Business Permit and other regulatory permit and");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 6010;
            this.axVSPrinter1.Table = string.Format("<800;Step 3: \n \n \n ");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.Table = "<5500; \n \n \n  ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.CurrentY = lngY + 640;
            this.axVSPrinter1.MarginLeft = 6810;
            this.axVSPrinter1.Table = string.Format("<4700;Releasing \n \n \n ");


            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY + 1280;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.FontSize = (float)9;
            this.axVSPrinter1.Table = string.Format("^11000;STANDARD PROCESSING TIME \n ");
            this.axVSPrinter1.FontBold = false;

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = string.Format("^11000;");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = "^2750;New \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.Table = "^2750;Renewal \n ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.Table = "^3500;1-2 Days \n  ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 320;
            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.Table = "^3500;1 day \n  ";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);

            this.axVSPrinter1.CurrentY = lngY + 640;    
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n ";
            axVSPrinter1.DrawPicture(Properties.Resources.back_white, ".50in", "5.70in", "50%", "50%", 10, false);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 690;
            this.axVSPrinter1.MarginLeft = 1800;
            this.axVSPrinter1.FontSize = (float)8;
            this.axVSPrinter1.Table = string.Format("<2500;Republic of the Philippines");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 890;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.Table = string.Format("<2500;PROVINCE OF {0}", AppSettingsManager.GetConfigValue("08"));

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 890;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 8500;
            this.axVSPrinter1.Table = ">2500;Location Sketch of";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1050;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 8500;
            this.axVSPrinter1.Table = ">2500;Business Establishment";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1050;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 1800;
            this.axVSPrinter1.Table = string.Format("<2500;{0}", AppSettingsManager.GetConfigValue("02"));
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1210;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.Table = string.Format("<2500;www.facebook.com/bpldmalolos");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1370;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.Table = string.Format("<2500;bpld@maloloscity.gov.ph");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.Table = string.Format("<2500;www.malolos.gov.ph");
            this.axVSPrinter1.FontUnderline = false;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 6500;
            this.axVSPrinter1.Table = "<250; ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 7600;
            this.axVSPrinter1.Table = "<250; ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 9100;
            this.axVSPrinter1.Table = "<250; ";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 6750;
            this.axVSPrinter1.Table = "<500; NEW";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 7850;
            this.axVSPrinter1.Table = "<900; RENEWAL";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 1530;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 9350;
            this.axVSPrinter1.Table = "<900; CLOSURE";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY + 350;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "<11000; Business Name:";
            this.axVSPrinter1.Table = "<11000; Business Address:";
            this.axVSPrinter1.Table = "<11000; Owner's Name:";
            this.axVSPrinter1.Table = "<11000; Owner's Address:";

            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            this.axVSPrinter1.Table = "<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733|<733;\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n |\n ";
            lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 150;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "^5500|^5500;Sketched by: |Date";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.CurrentY = lngY + 500;
            this.axVSPrinter1.FontSize = (float)7;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "^5500|^5500;_________________________|_________________________";
            this.axVSPrinter1.Table = "^5500|^5500;Signature over Printed Name|";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            this.axVSPrinter1.CurrentY = lngY;
            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n ";

            //JARS 20171010 (S)
            if(m_bPrintSupp)
            {
                this.axVSPrinter1.NewPage();
                lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
                this.axVSPrinter1.CurrentY = lngY + 1000;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                this.axVSPrinter1.FontSize = (float)12;
                this.axVSPrinter1.Table = string.Format("^11000;SUPPLEMENTAL PAGE");
                //this.axVSPrinter1.Paragraph = "";
                lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                this.axVSPrinter1.CurrentY = lngY;
                this.axVSPrinter1.FontSize = (float)9;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = "<11000; ";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.CurrentY = lngY;
                this.axVSPrinter1.FontSize = (float)9;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = "<800|<10200;|3. BUSINESS ACTIVITY";
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.CurrentY = lngY + 220;
                this.axVSPrinter1.FontSize = (float)9;
                this.axVSPrinter1.Table = "^3500|^2000|^2000;Line of Business|No. of Units|Capitalization \n(for New Business)";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.CurrentY = lngY + 220;
                this.axVSPrinter1.FontSize = (float)9;
                this.axVSPrinter1.MarginLeft = 8000;
                this.axVSPrinter1.Table = "^3500;Gross Sales Reciepts (for Renewal)";
                this.axVSPrinter1.Table = "^1750|^1750;Essential|Non - Essential";
                lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.CurrentY = lngY;
                this.axVSPrinter1.FontSize = (float)9;
                this.axVSPrinter1.MarginLeft = 500;

                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
                this.axVSPrinter1.Table = "^3500|^2000|^2000|^1750|^1750;||||";
            }
            //JARS 20171010 (E)

            #region comments
            //lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.MarginLeft = 6000;
            //this.axVSPrinter1.Table = string.Format("^5000;Printed and verified by:");
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.FontSize = (float)10;
            //this.axVSPrinter1.Table = string.Format("^5000;________________________________");
            //this.axVSPrinter1.Table = string.Format("^5000;BPLO STAFF");
            //this.axVSPrinter1.FontItalic = true;
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            ////this.axVSPrinter1.Table = "<3000;*SEE ATTACHED SOA";
            //this.axVSPrinter1.FontItalic = false;
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.CurrentY = lngY;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            ////this.axVSPrinter1.Table = "<11000; \n \n \n \n \n \n \n";
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.FontSize = (float)9;
            //if (sSupple != "")
            //{
            //    this.axVSPrinter1.NewPage();
            //    this.axVSPrinter1.Table = string.Format("^10000;SUPPLEMENTAL PAGE");
            //    this.axVSPrinter1.Paragraph = "";

            //    this.axVSPrinter1.Table = "<10000;3. BUSINESS ACTIVITY";
            //    this.axVSPrinter1.FontBold = false;

            //    this.axVSPrinter1.Table = "^5000|^1500|^1500|^3000;||Capitalization|Gross/Sales Receipts (for Renewal)";
            //    this.axVSPrinter1.Table = "^5000|^1500|^1500|^1500|^1500;Line of Business|No. of Units|(for New Business)|Essential|Non-Essential";
            //    this.axVSPrinter1.Table = string.Format("<5000|<1500|<1500|<1500|<1500;{0}", sSupple);
            //}
            //this.axVSPrinter1.MarginLeft = 9000;
            ////this.axVSPrinter1.FontItalic = true;
            ////this.axVSPrinter1.FontBold = true;
            ////this.axVSPrinter1.Table = "<3000;*SEE ATTACHED SOA";
            ////this.axVSPrinter1.FontItalic = false;
            ////this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.MarginLeft = 500;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "<5500|>5500;      2.   ASSESSMENT OF APPLICABLE FEES|*SEE ATTACHED SOA";
            //this.axVSPrinter1.FontBold = false;

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "^3500|^2500|^2500|^2500;Local Taxes|Amount Due|Penalty/Surcharge|Total";
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Gross Sales Tax|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Tax on Storage for Combustible / Flammable or Explosive Substance|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Tax on Signboard / Billboards|||";
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "<11000;REGULATORY FEES AND CHARGES";
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Mayor's Permit Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Garbage Charges|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Delivery Truck / Vans Permit Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Sanitary Inspection Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Building Inspection Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Electrical Inspection Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Signboard / Billboard Renewal Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Signboard / Billboard Renewal Fee|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Storage and Sale of Combustible / Flammable or Explosive Substance|||";
            //this.axVSPrinter1.Table = "<3500|^2500|^2500|^2500;Others";
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = ">3500|^2500|^2500|^2500;TOTAL FEES for LGU";
            //this.axVSPrinter1.Table = ">3500|^2500|^2500|^2500;FIRE SAFETY INSPECTION FEE (10%)";
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            ////this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;

            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.Table = "^500|<2500|<4000|<4000;|Assessed By: MTO||FSIF Assessment Approved by: BFP|";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.Table = "<3500|<3000|<4500;____________________________||___________________________________";
            //this.axVSPrinter1.FontBold = false;

            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Table = "<11000;III.  CITY / MUNICIPALITY FIRE STATION SECTION";
            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.Table = "^500|>10000|^500;|DATE:____________________|";
            //this.axVSPrinter1.Table = "<11000;Application No.:_________________________";
            //this.axVSPrinter1.Table = "<11000; ";
            //this.axVSPrinter1.Table = "^500|<10000|^500;|(TO BE FILLED UP BY APPLICANT/OWNER)|";
            //this.axVSPrinter1.Table = "<11000; ";
            //this.axVSPrinter1.Table = "<11000;Name of Applicant:__________________________________________________________________";
            //this.axVSPrinter1.Table = "^500|<10000|^500; ";
            //this.axVSPrinter1.Table = "<11000;Name of Business:__________________________________________________________________";
            //this.axVSPrinter1.Table = "^500|<10000|^500; ";
            //this.axVSPrinter1.Table = "<5500|<5500;Total Floor Area:_________________________|Contact No._______________________________";
            //this.axVSPrinter1.Table = "^500|<10000|^500; ";
            //this.axVSPrinter1.Table = "<11000;Address of Establishment:____________________________________________________________";
            //this.axVSPrinter1.Table = "^500|<10000|^500; ";
            //this.axVSPrinter1.Table = "^500|<10000|^500; ";
            //this.axVSPrinter1.Table = "^500|<10000|^500;|__________________________________|";
            //this.axVSPrinter1.Table = "^500|<10000|^500;|           Signature of Applicant/Owner|";
            //this.axVSPrinter1.Table = "^500|<10000|^500;||";
            //lngY = Convert.ToInt64(axVSPrinter1.CurrentY);
            //this.axVSPrinter1.Table = "^500|<10000|^500;|Certified By:|";
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
            //this.axVSPrinter1.MarginLeft = 700;
            //this.axVSPrinter1.Table = "^250; ";
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.Table = "^500|<10000|^500;|Customer Relations Officer:|";
            //this.axVSPrinter1.Table = "^500|<10000|^500;|Time And Date Recieved: ______________________|";

            //this.axVSPrinter1.CurrentY = lngY;

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            //this.axVSPrinter1.MarginLeft = 6500;
            //this.axVSPrinter1.Table = "<2500|<2500;FIRE SAFETY INSPECTION \nFEE ASSESSMENT: \n |";
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.MarginLeft = 500;
            ////this.axVSPrinter1.FontItalic = true;
            ////this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.FontSize = (float)8.0;
            //this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.Table = "<11000;Important Notice: As per Section 12 of the Implementing Rules and Regulations of the Fire Code of 2008, certain establishments (e.g. building lessors, fire, earthquake, and explosion hazard insurance companies, and vendors of fire fighting equipment, appliances and devices) may be required to pay additional charges and fees other than the Fire Safety Inspection Fees. These Shall be collected during inspections or in another process to be communicated by representatives of the Bureau of Fire Protection (BFP).";
            ////this.axVSPrinter1.FontItalic = false;
            //this.axVSPrinter1.FontBold = false;
            #endregion
            this.axVSPrinter1.EndDoc();
        }


        private string GetPrevCapGross(string sBIN, string sBnsCode)
        {
            OracleResultSet pSet = new OracleResultSet();

            string sBnsStat = string.Empty;
            string sTaxYear = string.Empty;
            string sVAlue = string.Empty;

            double dGr = 0;
            double dPreGr = 0;
            double dAjGr = 0;
            double dFinalCapGr = 0;
            double dCap = 0;
            string sTmp = string.Empty;
            
            pSet.Query = "select * from businesses where bin = '" + sBIN + "'";
            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    sTaxYear = pSet.GetString("tax_year");
                    sBnsStat = pSet.GetString("bns_stat");
                    dFinalCapGr = pSet.GetDouble("gr_1");
                    dCap = pSet.GetDouble("capital");

                    if (sBnsStat == "NEW")
                    {
                        if (dCap == 0)
                            sTmp = "";
                        else
                            sTmp = string.Format("{0:#,###.00}", dCap);

                        sVAlue = "Prev Cap: " + sTmp;
                    }
                    else
                    {
                        if (dFinalCapGr == 0)
                            sTmp = "";
                        else
                            sTmp = string.Format("{0:#,###.00}", dFinalCapGr);

                        sVAlue = "Prev Gross: " + sTmp;
                    }
                }
            }
            pSet.Close();

            pSet.Query = "select * from bill_gross_info where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "'";
            pSet.Query += " and bns_code = '" + sBnsCode + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    dAjGr = pSet.GetDouble("adj_gross");
                    dGr = pSet.GetDouble("gross");
                    dPreGr = pSet.GetDouble("pre_gross");
                    dCap = pSet.GetDouble("capital");
                    sBnsStat = pSet.GetString("bns_stat");

                    if (sBnsStat == "NEW")
                    {
                        if (dCap == 0)
                            sTmp = "";
                        else
                            sTmp = string.Format("{0:#,###.00}", dCap);

                        sVAlue = "Prev Cap: " + sTmp;
                    }
                    else
                    {
                        if (dPreGr != 0)
                            dFinalCapGr = dPreGr + dAjGr;
                        else
                            dFinalCapGr = dGr + dAjGr;

                        if (dFinalCapGr == 0)
                            sTmp = "";
                        else
                            sTmp = string.Format("{0:#,###.00}", dFinalCapGr);

                        sVAlue = "Prev Gross: " + sTmp;
                    }
                }
            }
            pSet.Close();

            

            return sVAlue;
        }

        private void PrintRetirementBinan()  //JHMN 20170110 retirement or closure application
        {
            string sOwnCode = string.Empty;
            string sMainBnsCode = string.Empty;
            string sAddlBnsCode = string.Empty;
            string sMemoranda = string.Empty;
            string sData = "";
            string strCurrentDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
            iYAxis = 4300;

            if (MessageBox.Show("Use pre-printed form?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                CreateHeader();

            this.axVSPrinter1.CurrentY = 2600;
            this.axVSPrinter1.MarginLeft = 700;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontSize = (float)12.0;

            OracleResultSet pSet = new OracleResultSet();

            sOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN); //MCR 20170530 BIN-17-7371
            this.axVSPrinter1.Table = string.Format("<10000;BIN: {0}", m_sBIN);
            this.axVSPrinter1.Table = string.Format("<10000;Business Name: {0}", AppSettingsManager.GetBnsName(m_sBIN).Trim());
            this.axVSPrinter1.Table = string.Format("<10000;Business Address: {0}", AppSettingsManager.GetBnsAddress(m_sBIN).Trim());
            this.axVSPrinter1.Table = string.Format("<10000;Owner's Name: {0}", AppSettingsManager.GetBnsOwner(sOwnCode).Trim());
            this.axVSPrinter1.Table = string.Format("<10000;Owner's Address: {0}", AppSettingsManager.GetBnsOwnAdd(sOwnCode).Trim());
            //this.axVSPrinter1.Table = string.Format("^9000;___________________________________________________________________________________________");
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^4000|^2500|^1300|^2200;LINE OF BUSINESS|LAST CAPITAL/GROSS DECLARED|NO. OF UNITS|GROSS SALES/RECEIPT");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            pSet.Query = "select * from businesses where bin = '" + m_sBIN + "' ";  //and tax_year = '" + Convert.ToDateTime(strCurrentDate).ToString("yyyy") + "'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sMainBnsCode = pSet.GetString("bns_code").Trim();
                    string sTaxYear = string.Empty;
                    sTaxYear = pSet.GetString("tax_year");

                    if (pSet.GetString("bns_stat") == "REN")
                    {
                        dGrossCapital = pSet.GetDouble("capital");
                    }
                    else if (pSet.GetString("bns_stat") == "NEW")
                    {
                        dGrossCapital = pSet.GetDouble("gr_1");
                    }

                    pSet.Close();

                    pSet.Query = "select * from bill_gross_info where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and bns_code = '" + sMainBnsCode + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            if (pSet.GetString("bns_stat") == "REN")
                            {
                                dGrossCapital = pSet.GetDouble("capital");
                            }
                            else if (pSet.GetString("bns_stat") == "NEW")
                            {
                                dGrossCapital = pSet.GetDouble("gross");
                            }
                        }
                    pSet.Close();

                    this.axVSPrinter1.FontBold = false;
                    string sGrossCapital = string.Empty;

                    if (dGrossCapital == 0)
                        sGrossCapital = "";
                    else
                        sGrossCapital = string.Format("{0:#,###.00}", dGrossCapital);

                    this.axVSPrinter1.Table = string.Format("<4000|>2500|^1300|^2200;{0}|{1}", AppSettingsManager.GetBnsDesc(sMainBnsCode).Trim(), sGrossCapital);

                    //pSet.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + Convert.ToDateTime(strCurrentDate).ToString("yyyy") + "'";
                    pSet.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year  = '" + sTaxYear + "'";
                    if (pSet.Execute())
                        while (pSet.Read())
                        {
                            sMainBnsCode = pSet.GetString("bns_code_main");

                            if (pSet.GetString("bns_stat") == "REN")
                            {
                                dGrossCapital = pSet.GetDouble("capital");
                            }
                            else if (pSet.GetString("bns_stat") == "NEW")
                            {
                                dGrossCapital = pSet.GetDouble("gross");
                            }

                            if (dGrossCapital == 0)
                                sGrossCapital = "";
                            else
                                sGrossCapital = string.Format("{0:#,###.00}", dGrossCapital);

                            this.axVSPrinter1.Table = string.Format("<4000|>2500|^1300|^2200;;{0}|{1}", AppSettingsManager.GetBnsDesc(sMainBnsCode).Trim(), string.Format("{0:#,###.00}", dGrossCapital));
                        }
                    pSet.Close();

                    //this.axVSPrinter1.Table = string.Format("^9000;___________________________________________________________________________________________");

                }
            pSet.Close();

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            /*pSet.Query = "select * from retired_bns where bin = '" + m_sBIN + "' and tax_year = '" + Convert.ToDateTime(strCurrentDate).ToString("yyyy") + "'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sMemoranda = pSet.GetString("memoranda");
                }
            pSet.Close();
             */
            sMemoranda = m_sMemo;

            //this.axVSPrinter1.MarginLeft = 1400;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "<10000;;REASON FOR RETIREMENT / CLOSURE OF BUSINESS";

            //this.axVSPrinter1.MarginLeft = 1400;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<10000;;" + sMemoranda;
            //this.axVSPrinter1.Table = string.Format("^9000;___________________________________________________________________________________________");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            sData = "For corporation, only responsible person (President, Chief Acccountant and Corporate Secretary) should sign ";
            sData += "the form. In case of Liason Officer or any authorized representative, kindly present an authourization letter ";
            sData += "signed by the identified responsible person of the corporation. ";
            //this.axVSPrinter1.MarginLeft = 1400;
            this.axVSPrinter1.Table = string.Format("<10000;{0}", sData);

            //this.axVSPrinter1.MarginLeft = 6200;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)11.0;
            this.axVSPrinter1.Table = string.Format("<5500|^5500;|_________________________________________");
            this.axVSPrinter1.Table = string.Format("<5500|^5500;|SIGNATURE OF APPLICANT OVER PRINTED NAME");

            //this.axVSPrinter1.MarginLeft = 6200;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<5500|^5500;|_________________________________________");

            //this.axVSPrinter1.MarginLeft = 7500;
            this.axVSPrinter1.Table = string.Format("<5500|^4500;|POSITION/ TITLE");

            //this.axVSPrinter1.MarginLeft = 1400;
            //his.axVSPrinter1.Table = string.Format("^9000;___________________________________________________________________________________________");

            this.axVSPrinter1.FontSize = (float)12.0;

            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                sData = "SUBSCRIBED AND SWORN BEFORE ME THIS " + DateSuffix(Convert.ToDateTime(strCurrentDate).Day.ToString()) + " DAY OF " + Convert.ToDateTime(strCurrentDate).ToString("MMMM, yyyy").ToUpper() + " AT THE CITY HALL OF " + AppSettingsManager.GetConfigValue("02");
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                sData = "SUBSCRIBED AND SWORN BEFORE ME THIS " + DateSuffix(Convert.ToDateTime(strCurrentDate).Day.ToString()) + " DAY OF " + Convert.ToDateTime(strCurrentDate).ToString("MMMM, yyyy").ToUpper() + " AT THE MUNICIPAL HALL OF " + AppSettingsManager.GetConfigValue("02");
            //this.axVSPrinter1.MarginLeft = 1400;
            this.axVSPrinter1.FontSize = (float)11.0;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("^10000;{0}", sData);
            this.axVSPrinter1.Paragraph = "";

            sData = "APPROVED BY: ____________________      AFFIANT ____________________ ";
            this.axVSPrinter1.Table = string.Format("^10000;{0}", sData);

            //this.axVSPrinter1.MarginLeft = 1600;
            this.axVSPrinter1.Table = string.Format("<9000;;;DOC NO.______________");
            this.axVSPrinter1.Table = string.Format("<9000;PAGE NO._____________");
            this.axVSPrinter1.Table = string.Format("<9000;BOOK NO._____________");
            this.axVSPrinter1.Table = string.Format("<9000;SERIES OF 20____");

            //this.axVSPrinter1.MarginLeft = 7400;
            this.axVSPrinter1.Table = string.Format("<5500|^4500;|_________________________________________");

            //this.axVSPrinter1.MarginLeft = 7400;
            this.axVSPrinter1.Table = string.Format("<5500|^4500;|Administering Officer");

            //this.axVSPrinter1.MarginLeft = 1400;
            this.axVSPrinter1.FontSize = (float)12.0;
            //this.axVSPrinter1.Table = string.Format("^9000;___________________________________________________________________________________________");


            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            //this.axVSPrinter1.MarginLeft = 2500;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^5000;{0}", AppSettingsManager.GetConfigValue("03").Trim());

            //this.axVSPrinter1.MarginLeft = 2500;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = string.Format("^5000;{0}", "CITY CHIEF, Business Permit and Licensing Office");

            if (AuditTrail.AuditTrail.InsertTrail("AAE-PF", "retirement", m_sBIN) == 0)
            {
                MessageBox.Show("", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public static string DateSuffix(string day) // JHMN 20170116 added for retirement application
        {
            /*Math.DivRem(day, 10, out day);
            switch (day)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }*/

            string sLastNo = "";
            string sDayth = "";

            sLastNo = StringUtilities.StringUtilities.Right(day, 1);

            if (Convert.ToInt32(day) > 9 && Convert.ToInt32(day) < 20)
            {
                sDayth = day + "th";
            }
            else
            {
                switch (Convert.ToInt32(sLastNo))
                {

                    case 1: sDayth = day + "st"; break;
                    case 2: sDayth = day + "nd"; break;
                    case 3: sDayth = day + "rd"; break;
                    default: sDayth = day + "th"; break;
                }
            }

            return sDayth;
        }

        private void BusinessRecordView()
        {
            // RMC 20171127 transferred printing of business record view to vsprinter

            OracleResultSet pRec = new OracleResultSet();   // RMC 20110914 Modified Business record profile view
            BPLSAppSettingList sList = new BPLSAppSettingList();

            sList.ModuleCode = m_sModuleCode;
            sList.ReturnValueByBin = m_sBIN;

            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                this.axVSPrinter1.FontName = "Arial Narrow";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.FontSize = 10;
                this.axVSPrinter1.Table = string.Format("^11000;Republic of the Philippines");

                this.axVSPrinter1.FontBold = true;

                string sLGUName = ConfigurationAttributes.LGUName;

                string strProvinceName = AppSettingsManager.GetConfigValue("08");
                if (strProvinceName != string.Empty)
                    this.axVSPrinter1.Table = string.Format("^11000;PROVINCE OF " + strProvinceName);

                this.axVSPrinter1.Table = string.Format("^11000;" + sLGUName);
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.Table = string.Format("^11000;" + AppSettingsManager.GetConfigValue("41"));
                this.axVSPrinter1.Paragraph = "";

                if (m_sModuleCode == "NEW-APP-VIEW")   // RMC 20110311
                    this.axVSPrinter1.Table = string.Format("^11000;Business Information - Application: NEW");
                else if (m_sModuleCode == "REN-APP-VIEW")
                    this.axVSPrinter1.Table = string.Format("^11000;Business Information - Application: RENEWAL");
                else
                    this.axVSPrinter1.Table = string.Format("^11000;Business Information - Business Record");
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontUnderline = true;

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.FontName = "Arial";
                this.axVSPrinter1.FontSize = 12;
                this.axVSPrinter1.Table = string.Format("^11000;BUSINESS INFORMATION");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontUnderline = false;
                this.axVSPrinter1.FontSize = 10;

                string sTaxYear = sList.BPLSAppSettings[i].sTaxYear;
                string sPermitNo = sList.BPLSAppSettings[i].sPermitNo;
                string stelno = sList.BPLSAppSettings[i].sBnsTelNo;

                string sBnsNm = StringUtilities.StringUtilities.RemoveApostrophe(sList.BPLSAppSettings[i].sBnsNm);
                string sOrgKind = sList.BPLSAppSettings[i].sOrgnKind;
                string sMainBns = sList.BPLSAppSettings[i].sBnsCode;

                string sBnsAdd = AppSettingsManager.GetBnsAdd(m_sBIN, "");


                string sDateOperated = sList.BPLSAppSettings[i].sDTOperated;
                string sDTIRegNo = sList.BPLSAppSettings[i].sDTIRegNo;
                string sDTIRegDt = sList.BPLSAppSettings[i].sDTIRegDate;

                if (sDTIRegNo.Trim() == "" || sDTIRegDt == AppSettingsManager.GetSystemDate().ToShortDateString())
                    sDTIRegDt = "";

                string sBussPlate = "";
                string sPEZARegNo = "";
                string sPEZARegDt = "";
                pRec.Query = "select * from buss_plate where bin = '" + m_sBIN + "'";
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        sBussPlate = pRec.GetString("bns_plate");
                    }
                }
                pRec.Close();

                pRec.Query = "select * from boi_table where bin = '" + m_sBIN + "' and exempt_type = 'PEZA'";
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        sPEZARegNo = pRec.GetString("reg_no");
                        try
                        {
                            sPEZARegDt = pRec.GetDateTime("reg_dt").ToShortDateString();
                        }
                        catch
                        {
                            sPEZARegDt = "";
                        }

                    }

                }
                pRec.Close();


                sList.MainBns = sMainBns;
                sMainBns = sList.m_sMainBns;
                sMainBns = sList.m_sMainBns;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                
                this.axVSPrinter1.Table = string.Format("<2000|<3000|<1500|<1500|<1500|<2500;BIN:|{0}|Tax Year:|{1}|Date Operated:|{2}", m_sBIN, sTaxYear, sDateOperated);
                this.axVSPrinter1.Table = string.Format("<2000|<3000|<1500|<1500|<1000|<3000;Permit No.:|{0}|Permit Date:|{1}|Tel. No.:|{2}", sPermitNo, sTaxYear, "");
                                
                stelno = stelno.Replace('/', '\n');
                this.axVSPrinter1.Table = string.Format("<9000|<1000; |{0}", stelno);
                
                this.axVSPrinter1.Table = string.Format("<2000|<30000;Business Name:|{0}", sBnsNm);
                this.axVSPrinter1.Table = string.Format("<2000|<30000;Business Address:|{0}", sBnsAdd);
                this.axVSPrinter1.Table = string.Format("<2000|<3000|<2000|<5000;Orgn. Kind:|{0}|Business Plate:|{1}", sOrgKind, sBussPlate);

                string sCap = string.Format("{0:#,###.##}", Convert.ToDouble(sList.BPLSAppSettings[i].sCapital));
                string sGross = string.Format("{0:#,###.##}", Convert.ToDouble(sList.BPLSAppSettings[i].sGr1));
                string sBnsStat = sList.BPLSAppSettings[i].sBnsStat;

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = string.Format("<1000|<2000|<2000|<2000|<2000;|DTI Registration No.:|{0}|Registration Date:|{1}", sDTIRegNo, sDTIRegDt);
                this.axVSPrinter1.Table = string.Format("<1000|<2000|<2000|<2000|<2000;|PEZA Registration No.:|{0}|Registration Date:|{1}", sPEZARegNo, sPEZARegDt);

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = 12;
                this.axVSPrinter1.FontUnderline = true;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("^11000;OWNER'S INFORMATION");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontUnderline = false;
                this.axVSPrinter1.FontSize = 10;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                string sOwnCode = sList.BPLSAppSettings[i].sOwnCode;
                sList.OwnName = sOwnCode;
                string sOwnLn = sList.OwnNamesSetting[i].sLn;
                string sOwnFn = sList.OwnNamesSetting[i].sFn;
                string sOwnMi = sList.OwnNamesSetting[i].sMi;
                string sOwnAdd = AppSettingsManager.GetBnsOwnAdd(sOwnCode);

                this.axVSPrinter1.Table = string.Format("<2000|<9000;Last Name:|{0}", sOwnLn);
                this.axVSPrinter1.Table = string.Format("<2000|<3000|<1500|<1500|<1000|<3000;First Name:|{0}|Middle Initial:|{1}||", sOwnFn, sOwnMi);
                this.axVSPrinter1.Table = string.Format("<2000|<9000;Address:|{0}", sOwnAdd);

                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.FontSize = 12;
                this.axVSPrinter1.FontUnderline = true;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("^11000;LOCATION STATUS");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontUnderline = false;
                this.axVSPrinter1.FontSize = 10;
                this.axVSPrinter1.Paragraph = "";

                string sPlaceOwnership = sList.BPLSAppSettings[i].sPlaceOccupancy;

                string sPlaceOwnCode = "";

                if (sPlaceOwnership == "RENTED")
                    sPlaceOwnCode = sList.BPLSAppSettings[i].m_sBUSN_OWN;

                if (sPlaceOwnership == string.Empty)
                    sPlaceOwnership = "OWNED";

                if (sPlaceOwnCode == string.Empty)
                    sPlaceOwnCode = sList.BPLSAppSettings[i].m_sOWN_CODE;


                sList.OwnName = sPlaceOwnCode;
                string sPrevOwnLn = sList.OwnNamesSetting[i].sLn;
                string sPrevOwnFn = sList.OwnNamesSetting[i].sFn;
                string sPrevOwnMi = sList.OwnNamesSetting[i].sMi;
                string sPrevOwnAdd = AppSettingsManager.GetBnsOwnAdd(sPlaceOwnCode);

                string sMontlyRent = string.Format("{0:#,###.##}", Convert.ToDouble(sList.BPLSAppSettings[i].sRentLeaseMo));    // RMC 20110914
                string sNoEmp = sList.BPLSAppSettings[i].sNumEmployees;
                string sNumProf = sList.BPLSAppSettings[i].sNumProfessional;
                string sPayroll = sList.BPLSAppSettings[i].sAnnualWages;
                string sDelv = sList.BPLSAppSettings[i].sNumDelivVehicle;
                string sStorey = sList.BPLSAppSettings[i].sNumStorey;
                string sTotArea = sList.BPLSAppSettings[i].sTotFlrArea;
                string sNumMach = sList.BPLSAppSettings[i].sNumMachineries;
                string sGroundFlr = sList.BPLSAppSettings[i].sFlrArea;
                string sBldgVal = sList.BPLSAppSettings[i].sBldgVal;
                this.axVSPrinter1.Table = string.Format("<3000|<2000|<1500|<1500|<2000|<2000;Business Place Ownership:|{0}|Since When:|{1}|Monthly Rental:|{2}", sPlaceOwnership, sTaxYear, sMontlyRent);
                this.axVSPrinter1.Table = string.Format("<2000|<9000;Last Name:|{0}", sPrevOwnLn);
                this.axVSPrinter1.Table = string.Format("<2000|<3000|<1500|<1500|<1000|<3000;First Name:|{0}|Middle Initial:|{1}||", sPrevOwnFn, sPrevOwnMi);
                this.axVSPrinter1.Table = string.Format("<2000|<9000;Address:|{0}", sPrevOwnAdd);

                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.FontSize = 12;
                this.axVSPrinter1.FontUnderline = true;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("^11000;PREVIOUS OWNER INFORMATION");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontUnderline = false;
                this.axVSPrinter1.FontSize = 10;
                this.axVSPrinter1.Paragraph = "";
                string sPlaceOwnCode2 = sList.BPLSAppSettings[i].sPrevBnsOwn;


                if (sPlaceOwnCode2 == string.Empty)
                    sPlaceOwnCode2 = sList.BPLSAppSettings[i].m_sOWN_CODE;

                sList.OwnName = sPlaceOwnCode2;
                string sPrevOwnLn2 = sList.OwnNamesSetting[i].sLn;
                string sPrevOwnFn2 = sList.OwnNamesSetting[i].sFn;
                string sPrevOwnMi2 = sList.OwnNamesSetting[i].sMi;
                string sPrevOwnAdd2 = AppSettingsManager.GetBnsOwnAdd(sPlaceOwnCode2);

                this.axVSPrinter1.Table = string.Format("<2000|<9000;Last Name:|{0}", sPrevOwnLn2);
                this.axVSPrinter1.Table = string.Format("<2000|<3000|<1500|<1500|<1000|<3000;First Name:|{0}|Middle Initial:|{1}||", sPrevOwnFn2, sPrevOwnMi2);
                this.axVSPrinter1.Table = string.Format("<2000|<9000;Address:|{0}", sPrevOwnAdd2);

                int iCnt = 0;
                pRec.Query = "select count(*) from other_info where bin = '" + m_sBIN + "' and bns_code in ";
                pRec.Query += "(select bns_code from businesses where bin = '" + m_sBIN + "')";
                pRec.Query += " and tax_year = '" + sTaxYear + "'";
                pRec.Query += " and data <> 0";
                int.TryParse(pRec.ExecuteScalar(), out iCnt);

                if (iCnt > 0)
                {
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontSize = 12;
                    this.axVSPrinter1.FontUnderline = true;
                    this.axVSPrinter1.FontBold = true;
                    this.axVSPrinter1.Table = string.Format("^11000;ADDITIONAL INFORMATION");
                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.FontUnderline = false;
                    this.axVSPrinter1.FontSize = 10;
                    this.axVSPrinter1.Paragraph = "";


                    this.axVSPrinter1.Table = string.Format("^8000|^1000;Description|Value");

                    string sDefaultCode = "";
                    string sDefaultDesc = "";
                    string sValue = "";
                    pRec.Query = "select * from other_info where bin = '" + m_sBIN + "' and bns_code in ";
                    pRec.Query += "(select bns_code from businesses where bin = '" + m_sBIN + "')";
                    pRec.Query += " and tax_year = '" + sTaxYear + "'";
                    pRec.Query += " and data <> 0";
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            sDefaultCode = pRec.GetString("default_code");
                            sValue = string.Format("{0:###.00}", pRec.GetDouble("data"));

                            sDefaultDesc = GetDefaultDesc(sDefaultCode);

                            this.axVSPrinter1.Table = string.Format("<8000|>1000;{0}|{1}", sDefaultDesc, sValue);
                        }
                    }
                    pRec.Close();
                }

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = 12;
                this.axVSPrinter1.FontUnderline = true;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("^11000;LINE/S OF BUSINESS");
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontUnderline = false;
                this.axVSPrinter1.FontSize = 10;
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.Table = string.Format("^5000|^1000|^1500|^1500;Line of Business|Status|Capital|Gross");
                this.axVSPrinter1.Table = string.Format("<5000|^1000|>1500|>1500;(Main) {0}|{1}|{2}|{3}", sMainBns, sBnsStat, sCap, sGross);

                OracleResultSet result = new OracleResultSet();

                if (m_sModuleCode == "NEW-APP-VIEW" || m_sModuleCode == "REN-APP-VIEW")
                {
                    result.Query = "select * from addl_bns_que where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "'";
                }
                else
                {

                    result.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "'";
                }
                if (result.Execute())
                {
                    string sAddlBnsCode = "";
                    string sAddlBnsStat = "";
                    string sAddlGross = "";
                    string sAddlCapital = "";

                    while (result.Read())
                    {
                        sAddlBnsCode = result.GetString("bns_code_main").Trim();
                        sAddlBnsCode = AppSettingsManager.GetBnsDesc(sAddlBnsCode);
                        sAddlBnsStat = result.GetString("bns_stat").Trim();
                        sAddlGross = string.Format("{0:#,###.##}", result.GetDouble("gross"));
                        sAddlCapital = string.Format("{0:#,###.##}", result.GetDouble("capital"));

                        this.axVSPrinter1.Table = string.Format("<5000|^1000|>1500|>1500;{0}|{1}|{2}|{3}", sAddlBnsCode, sAddlBnsStat, sAddlCapital, sAddlGross);
                    }
                }
                result.Close();

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.Table = string.Format("<2000|=8000;Memoranda:|{0}", sList.BPLSAppSettings[i].m_sMEMORANDA);

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
                this.axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());
            }
        }


        //JHB 20180716 ADD BUSINESS PERMIT VIEW (s)
        private void PermitView() 
        {
            string sBnsName = "";
            string sBnsAddress = "";
            string sRecord = "";
            string sBnsStat = "";
            string sTaxYear = "";
            string sOwnName = "";
            string sDay = "";
            string sMonth = "";
            string sYear = "";

            string sPermitDt = "";
            string sPermitNo = "";
            string sOrgnKnd = "";

            Double sCapital1 = 0;
            Double sGr1 = 0;
            string sCapital = "";
            string sGr_1 = "";

            int iDay;

            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; //orPortrait

            OracleResultSet pRec = new OracleResultSet();
            pRec.Query = "select own_code from businesses where bin = '" + m_sBIN + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sOwnName = AppSettingsManager.GetBnsOwner(pRec.GetString("own_code"));
                }
            }
            pRec.Close();
         
            sBnsName = AppSettingsManager.GetBnsName(m_sBIN);
            sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);

            this.axVSPrinter1.StartDoc();
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.MarginLeft = 1500;
            this.axVSPrinter1.FontSize = (float)14;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^14000;BUSINESS PERMIT HISTORY"); 
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Table = "<10000; Owner's Name: " + sOwnName + " ";
            this.axVSPrinter1.Table = "<10000; Bin: " + m_sBIN + " ";
            this.axVSPrinter1.Table = "<10000; Business Name: " + sBnsName + " ";
            this.axVSPrinter1.Table = "<10000; Business Address: " + sBnsAddress + " ";

            this.axVSPrinter1.Table = "<11000; ";
            this.axVSPrinter1.Table = "<11000; ";

            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.MarginLeft = 700;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^2000|^2000|^2000|^2000|^2000|^2000|^2000; CAPITAL | GROSS | STATUS | TAX YEAR | ORIGIN KIND | PERMIT No. | PERMIT DATE");
            this.axVSPrinter1.FontBold = false;

            
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select distinct Tax_year,bns_nm,permit_dt, permit_no,bns_stat,orgn_kind, capital, gr_1 from businesses where  bin = '" + m_sBIN + "'";
            pSet.Query += " union all select distinct Tax_year,bns_nm,permit_dt, permit_no,bns_stat,orgn_kind, capital, gr_1 from  buss_hist where bin = '" + m_sBIN + "' order by tax_year ";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                   
                        sBnsName = pSet.GetString("bns_nm");
                        sBnsStat = pSet.GetString("bns_stat");
                        sTaxYear = pSet.GetString("tax_year");
                        sOrgnKnd = pSet.GetString("orgn_kind");

                        sCapital1 = pSet.GetDouble("capital");
                        sCapital = string.Format("{0:#,###.00}", sCapital1);
                        sGr1 = pSet.GetDouble("gr_1");
                        sGr_1 = string.Format("{0:#,###.00}", sGr1);

                        sPermitDt = pSet.GetDateTime("permit_dt").ToShortDateString();

                        sTaxYear = pSet.GetString("tax_year");
                        sPermitNo = pSet.GetString("permit_no");
                       
                   
                        this.axVSPrinter1.Table = "^2000|^2000|^2000|^2000|^2000|^2000|^2000;" + sCapital + "|" + sGr_1 + "|" + sBnsStat + "|" + sTaxYear + "|" + sOrgnKnd + "|" + sPermitNo + "|" + sPermitDt;
            
                }
            }
            pSet.Close();

            //this.axVSPrinter1.Table = "^2000|^2000|^2000|^2000|^2000|^2000|^2000;" + sCapital + "|" + sGr_1 + "|" + sBnsStat + "|" + sTaxYear + "|" + sOrgnKnd + "|" + sPermitNo + "|" + sPermitDt;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            //DateTime current = DateTime.Now;
            //sDay = current.ToString("dd");
            //iDay = Convert.ToInt32(sDay);

            //sDay = iDay.ToString();
            //sDay = ConvertDayInOrdinalForm(sDay);
            //sMonth = current.ToString("MMMM");
            //sYear = current.ToString("yyyy");
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "^2000|^5000;Printed by :|" + AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            axVSPrinter1.Table = "^2000|^5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        //JHB 20180716 BUSINESS PERMIT VIEW(e)

        private string GetDefaultDesc(string sCode)
        {
            // RMC 20171127 transferred printing of business record view to vsprinter

            OracleResultSet pSet = new OracleResultSet();
            string sDefaultDesc = "";

            pSet.Query = string.Format("select * from default_code where default_code = '{0}' and rev_year = '{1}'", sCode, ConfigurationAttributes.RevYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sDefaultDesc = pSet.GetString("default_desc").Trim();
                }
            }
            pSet.Close();

            return sDefaultDesc;
        }

        
    }
}