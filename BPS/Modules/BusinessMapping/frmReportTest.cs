// ALJ 20120629 put handle apostrophe (matching report by business name)
// RMC 20120328 modified Business mapping summary report
// RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Amellar.Common.DataConnector;
using Amellar.Common.BPLSApp;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.BusinessMapping
{
    
    public partial class frmReportTest : Form
    {
        
        private BPLSAppSettingList sList = new BPLSAppSettingList();
        private string m_sBrgy = "";
        private string m_sStreet = "";
        private string m_sOrderby = "";
        private string m_sArea = "";
        private string m_sReportName = "";
        private bool m_bBlank = false;
        private string m_sQuery = "";
        private string m_sReportSwitch = "";
        private string m_sFrom = string.Empty;
        private string m_sTo = string.Empty;
        private bool m_bInit = false;
        private string m_sMatch = "";
        
        public string Barangay
        {
            get { return m_sBrgy; }
            set { m_sBrgy = value; }
        }

        public string Street
        {
            get { return m_sStreet; }
            set { m_sStreet = value; }
        }

        public string Area
        {
            get { return m_sArea; }
            set { m_sArea = value; }
        }

        public string OrderBy
        {
            get { return m_sOrderby; }
            set { m_sOrderby = value; }
        }

        public bool BlankForm
        {
            get { return m_bBlank; }
            set { m_bBlank = value; }
        }

        public string ReportName
        {
            get { return m_sReportName; }
            set { m_sReportName = value; }
        }

        public string ReportSwitch
        {
            get { return m_sReportSwitch; }
            set { m_sReportSwitch = value; }
        }

        public string Query
        {
            get { return m_sQuery; }
            set { m_sQuery = value; }
        }

        public string DateFrom
        {
            get { return m_sFrom; }
            set { m_sFrom = value; }
        }

        public string DateTo
        {
            get { return m_sTo; }
            set { m_sTo = value; }
        }

        public string MatchFilter
        {
            get { return m_sMatch; }
            set { m_sMatch = value; }
        }

        public frmReportTest()
        {
            InitializeComponent();
        }

        private void frmReportTest_Load(object sender, EventArgs e)
        {
            m_bInit = true;
            
            this.ExportFile();
            

            if (m_sReportName == "Accomplishment")
            {
                //this.axVSPrinter1.PrintDialog;
                //axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);
                
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA3;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 1200;
                //this.axVSPrinter1.MarginTop = 360;
                this.axVSPrinter1.MarginTop = 720;
                //this.axVSPrinter1.MarginRight = 1100;
                //this.axVSPrinter1.MarginBottom = 360;
                this.axVSPrinter1.MarginBottom = 2000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

                CreateReportHeaders();
                if (m_bBlank)
                    PrintBlankForm();
                else
                    PrintForm();
            }
            else if (m_sReportName == "Summary Official")
            {
                //axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.MarginTop = 300;
                //this.axVSPrinter1.MarginRight = 1100;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                PrintSummaryReport();
            }
            else if (m_sReportName == "Unofficial")
            {
                //axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.MarginTop = 300;
                //this.axVSPrinter1.MarginRight = 1100;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                m_sReportSwitch = "Unencoded Businesses (with Permit)";
                PrintSummaryUnofficial();
            }
            else if(m_sReportName == "Not-Mapped")
            {
                //axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.MarginTop = 300;
                //this.axVSPrinter1.MarginRight = 1100;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                PrintSummaryNotMapped();
            }
            else if (m_sReportName == "Summary")
            {
                //axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                //this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                //this.axVSPrinter1.MarginLeft = 1000;
                this.axVSPrinter1.MarginLeft = 500; // RMC Modified Business mapping summary report. Added column for unofficial businesses that complied
                this.axVSPrinter1.MarginTop = 300;
                //this.axVSPrinter1.MarginRight = 1100;
                this.axVSPrinter1.MarginBottom = 800;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                PrintSummary();
            }
            else if (m_sReportName == "Summary Official Details")
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.MarginTop = 300;
                //this.axVSPrinter1.MarginRight = 1100;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                PrintSummaryDetailedReport();
            }
            else if (m_sReportName == "OFFICIAL BUSINESS" || m_sReportName == "UNOFFICIAL BUSINESS")
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

                this.axVSPrinter1.MarginLeft = 1000;
                this.axVSPrinter1.MarginTop = 300;
                //this.axVSPrinter1.MarginRight = 1100;
                this.axVSPrinter1.MarginBottom = 800;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                PrintEncodersReport();
            }
            else if (m_sReportName == "Print Matching")
            {
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.MarginTop = 300;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                PrintCleansingTool();
            }
            else if (m_sReportName == "MAPPED WITH PERMIT")
            {

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 700;
                this.axVSPrinter1.MarginTop = 300;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                //PrintHeaderMapped(m_sReportName, m_sBrgy);
                CreateReportHeaders();
                MappedReport(m_sReportName);
                
            }
            else if (m_sReportName == "MAPPED WITHOUT PERMIT")
            {

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 700;
                this.axVSPrinter1.MarginTop = 300;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                MappedReport(m_sReportName);
            }
            else if (m_sReportName == "MAPPED WITHOUT PERMIT2")
            {

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 700;
                this.axVSPrinter1.MarginTop = 300;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                MappedReport(m_sReportName);
            }
            else if (m_sReportName == "MAPPED WITH PERMIT AND NOTICE")
            {

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 700;
                this.axVSPrinter1.MarginTop = 300;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                MappedReport(m_sReportName);
            }
            else if (m_sReportName == "MAPPED WITHOUT PERMIT AND NOTICE")
            {

                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

                this.axVSPrinter1.MarginLeft = 700;
                this.axVSPrinter1.MarginTop = 300;
                this.axVSPrinter1.MarginBottom = 1000;

                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateReportHeaders();
                MappedReport(m_sReportName);
            }
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
            axVSPrinter1.ExportFile = "";
        }


        private void MappedReport(string sReportName)
        {
            OracleResultSet result = new OracleResultSet();
            int iCtr = 0;
            string sBin = string.Empty;
            string sTBin = string.Empty;
            string sLPin = string.Empty;
            string sBnsNm = string.Empty;
            string sBnsAdd = string.Empty;
            string sOwnName = string.Empty;
            string sOwnAdd = string.Empty;
            string sPermitNo = string.Empty;
            string sOrNo = string.Empty;
            string sOrDate = string.Empty;
            string sOrAmount = string.Empty;
            string sBnsType = string.Empty;
            string sOrgn = string.Empty;
            string strData = string.Empty;
            string sTrueBin = string.Empty;

            double dOrAmount = 0;
            double dTotAmtCollected = 0;
            if (sReportName == "MAPPED WITH PERMIT")
            {
                dTotAmtCollected = 0;
                int iCntWithPay = 0;
                int iCntWOPay = 0;
                result.Query = "select * from btm_temp_businesses where length(permit_no) = 10 and bns_brgy = '" + m_sBrgy.Trim() + "' order by bns_nm";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        iCtr += 1;
                        sBin = result.GetString("tbin").Trim();
                        sTrueBin = result.GetString("bin").Trim();
                        dOrAmount = AppSettingsManager.GetLastOrAmountMapped(result.GetString("bin").Trim());
                        dTotAmtCollected += dOrAmount;
                        if (dOrAmount > 0)
                            iCntWithPay += 1;
                        else
                            iCntWOPay += 1;
                            
                        //if (sBin == string.Empty)
                        //    sBin = "* " + result.GetString("bns_brgy");
                        strData = iCtr.ToString() + "|";
                        strData += sBin + "|";
                        strData += result.GetString("bns_nm").Trim() + "|";
                        strData += AppSettingsManager.GetMappedBnsAddress(sBin) + "|";
                        strData += AppSettingsManager.GetBnsOwner(result.GetString("own_code").Trim()) + "|";
                        strData += AppSettingsManager.GetBnsOwnAdd(result.GetString("own_code").Trim()) + "|";
                        strData += result.GetString("permit_no").Trim() + "|";
                        strData += AppSettingsManager.GetLastOrNoMapped(result.GetString("bin").Trim()) + "|";
                        strData += AppSettingsManager.GetLastOrDateMapped(result.GetString("bin").Trim()) + "|";
                        strData += string.Format("{0:#,###.00}", AppSettingsManager.GetLastOrAmountMapped(result.GetString("bin").Trim())) + ";";
                        this.axVSPrinter1.Table = string.Format(">400|<1700|<2500|<2500|<2200|<2500|<1100|<850|>850|>850;{0}", strData);
                    }
                }
                result.Close();
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Paragraph = "";
                strData = string.Format("{0:#,###}", iCntWithPay);
                this.axVSPrinter1.Table = string.Format("<10000; Businesses with Payments: {0}", strData);
                strData = string.Format("{0:#,###}", iCntWOPay);
                this.axVSPrinter1.Table = string.Format("<10000; Businesses without Payments: {0}", strData);
                strData = string.Format("{0:#,###}", iCntWithPay + iCntWOPay);
                this.axVSPrinter1.Table = string.Format("<10000; Total no. of businesses: {0}", strData);
                strData = string.Format("{0:#,###.#0}", dTotAmtCollected);
                this.axVSPrinter1.Table = string.Format("<10000; Total amount collected: {0}", strData);
            }
            else if (sReportName == "MAPPED WITHOUT PERMIT")
            {
                dTotAmtCollected = 0;
                int iCntWithPay = 0;
                int iCntWOPay = 0;
                //result.Query = "select * from btm_temp_businesses where permit_no = ' ' and bns_brgy = '" + m_sBrgy.Trim() + "' order by bns_nm";
                result.Query = "select * from btm_temp_businesses where permit_no is null and bns_brgy = '" + m_sBrgy.Trim() + "' order by bns_nm";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        iCtr += 1;
                        sBin = result.GetString("tbin").Trim();
                        sOrgn = result.GetString("orgn_kind").Trim();
                        if(sOrgn == "SINGLE PROPRIETORSHIP")
                            sOrgn = "SPROP";
                        if(sOrgn == "CORPORATION")
                            sOrgn = "CORP";
                        else
                            sOrgn = "PARTN";
                        dOrAmount = AppSettingsManager.GetLastOrAmountMapped(result.GetString("bin").Trim());
                        dTotAmtCollected += dOrAmount;
                        if (dOrAmount > 0)
                            iCntWithPay += 1;
                        else
                            iCntWOPay += 1;
                        strData = iCtr.ToString() + "|";
                        strData += AppSettingsManager.GetLandPin(sBin) + "|";
                        strData += sBin + "|";
                        strData += result.GetString("bns_nm").Trim() + "|";
                        strData += AppSettingsManager.GetMappedBnsAddress(result.GetString("tbin").Trim()) + "|";
                        strData += AppSettingsManager.GetBnsOwner(result.GetString("own_code").Trim()) + "|";
                        strData += AppSettingsManager.GetBnsOwnAdd(result.GetString("own_code").Trim()) + "|";
                        strData += AppSettingsManager.GetBnsDesc(result.GetString("bns_code").Trim()) + "|";
                        strData += sOrgn + ";";
                        this.axVSPrinter1.Table = string.Format(">500|<1500|<1500|<2500|<2500|<2500|<2500|<1000|<1100;{0}", strData);
                    }
                }
                result.Close();
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Paragraph = "";
                strData = string.Format("{0:#,###}", iCntWithPay);
                this.axVSPrinter1.Table = string.Format("<10000; Businesses with Payments: {0}", strData);
                strData = string.Format("{0:#,###}", iCntWOPay);
                this.axVSPrinter1.Table = string.Format("<10000; Businesses without Payments: {0}", strData);
                strData = string.Format("{0:#,###}", iCntWithPay + iCntWOPay);
                this.axVSPrinter1.Table = string.Format("<10000; Total no. of businesses: {0}", strData);
                strData = string.Format("{0:#,###.#0}", dTotAmtCollected);
                this.axVSPrinter1.Table = string.Format("<10000; Total amount collected: {0}", strData);
                
            }
            else if (sReportName == "MAPPED WITHOUT PERMIT2")
            {
                dTotAmtCollected = 0;
                int iCntWithPay = 0;
                int iCntWOPay = 0;
                //result.Query = "select * from btm_temp_businesses where permit_no = ' ' and bns_brgy = '" + m_sBrgy.Trim() + "' order by bns_nm";
                result.Query = "select * from btm_temp_businesses where permit_no is null and bns_brgy = '" + m_sBrgy.Trim() + "' and bin not in (select bin from business_que where tax_year = '" +DateTime.Now.Year.ToString()+ "') order by bns_nm";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        iCtr += 1;
                        sBin = result.GetString("tbin").Trim();
                        sOrgn = result.GetString("orgn_kind").Trim();
                        if (sOrgn == "SINGLE PROPRIETORSHIP")
                            sOrgn = "SPROP";
                        if (sOrgn == "CORPORATION")
                            sOrgn = "CORP";
                        else
                            sOrgn = "PARTN";
                        dOrAmount = 0;
                        dOrAmount = AppSettingsManager.GetLastOrAmountMapped(result.GetString("bin").Trim());
                        dTotAmtCollected += dOrAmount;
                        if (dOrAmount > 0)
                            iCntWithPay += 1;
                        else
                            iCntWOPay += 1;
                        strData = iCtr.ToString() + "|";
                        strData += AppSettingsManager.GetLandPin(sBin) + "|";
                        strData += sBin + "|";
                        strData += result.GetString("bns_nm").Trim() + "|";
                        strData += AppSettingsManager.GetMappedBnsAddress(result.GetString("tbin").Trim()) + "|";
                        strData += AppSettingsManager.GetBnsOwner(result.GetString("own_code").Trim()) + "|";
                        strData += AppSettingsManager.GetBnsOwnAdd(result.GetString("own_code").Trim()) + "|";
                        strData += AppSettingsManager.GetBnsDesc(result.GetString("bns_code").Trim()) + "|";
                        strData += sOrgn + ";";
                        this.axVSPrinter1.Table = string.Format(">500|<1500|<1500|<2500|<2500|<2500|<2500|<1000|<1100;{0}", strData);
                    }
                }
                result.Close();
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Paragraph = "";
                strData = string.Format("{0:#,###}", iCntWithPay);
                this.axVSPrinter1.Table = string.Format("<10000; Businesses with Payments: {0}", strData);
                strData = string.Format("{0:#,###}", iCntWOPay);
                this.axVSPrinter1.Table = string.Format("<10000; Businesses without Payments: {0}", strData);
                strData = string.Format("{0:#,###}", iCntWithPay + iCntWOPay);
                this.axVSPrinter1.Table = string.Format("<10000; Total no. of businesses: {0}", strData);
                strData = string.Format("{0:#,###.#0}", dTotAmtCollected);
                this.axVSPrinter1.Table = string.Format("<10000; Total amount collected: {0}", strData);

            }
            else if (sReportName == "MAPPED WITH PERMIT AND NOTICE")
            {
                dTotAmtCollected = 0;
                int iCntWithPay = 0;
                int iCntWOPay = 0;
                result.Query = "select * from btm_TEMP_businesses where length(permit_no) = 10 and bns_brgy = '" + m_sBrgy.Trim() + "' order by bns_nm";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        iCtr += 1;
                        sBin = result.GetString("tbin").Trim();
                        sBnsNm = result.GetString("bns_nm").Trim();

                        dOrAmount = AppSettingsManager.GetLastOrAmountMapped(result.GetString("bin").Trim());
                        dTotAmtCollected += dOrAmount;
                        if (dOrAmount > 0)
                            iCntWithPay += 1;
                        else
                            iCntWOPay += 1;

                        strData = iCtr.ToString() + "|";
                        strData += sBnsNm + "|";
                        strData += AppSettingsManager.GetMappedBnsAddress(sBin) + "|";
                        strData += AppSettingsManager.GetNoticeDate(sBin, "1") + "|";
                        strData += AppSettingsManager.GetNoticeDate(sBin, "2") + "|";
                        strData += AppSettingsManager.GetCDODate(sBin) + "|";
                        strData += AppSettingsManager.GetLastOrNoMapped(result.GetString("bin").Trim()) + "|";
                        strData += AppSettingsManager.GetLastOrDateMapped(result.GetString("bin").Trim()) + "|";
                        strData += string.Format("{0:#,###.#0}", AppSettingsManager.GetLastOrAmountMapped(result.GetString("bin").Trim())) + ";";
                        this.axVSPrinter1.Table = string.Format(">1000|<3750|<4750|<1000|<1000|<1100|<1000|<1000|>1000;{0}", strData);
                    }
                }
                result.Close();
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Paragraph = "";
                strData = string.Format("{0:#,###}", iCntWithPay);
                this.axVSPrinter1.Table = string.Format("<10000; Businesses with Payments: {0}", strData);
                strData = string.Format("{0:#,###}", iCntWOPay);
                this.axVSPrinter1.Table = string.Format("<10000; Businesses without Payments: {0}", strData);
                strData = string.Format("{0:#,###}", iCntWithPay + iCntWOPay);
                this.axVSPrinter1.Table = string.Format("<10000; Total no. of businesses: {0}", strData);
                strData = string.Format("{0:#,###.#0}", dTotAmtCollected);
                this.axVSPrinter1.Table = string.Format("<10000; Total amount collected: {0}", strData);
                
            }
            else if (sReportName == "MAPPED WITHOUT PERMIT AND NOTICE")
            {
                dTotAmtCollected = 0;
                double dAmount = 0;
                int iCntWithPay = 0;
                int iCntWOPay = 0;
                string sTBIN = string.Empty;
                result.Query = "select * from btm_TEMP_businesses where permit_no is null and bns_brgy = '" + m_sBrgy.Trim() + "' order by bns_nm";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        iCtr += 1;
                        sBin = result.GetString("tbin").Trim();
                        sTBIN = result.GetString("bin").Trim();
                        sBnsNm = result.GetString("bns_nm").Trim();
                        dAmount = AppSettingsManager.GetLastOrAmountMapped(sTBIN);
                        dTotAmtCollected += dAmount;
                        if (dAmount > 0)
                            iCntWithPay += 1;
                        else
                            iCntWOPay += 1;
                        strData = iCtr.ToString() + "|";
                        strData += sBnsNm + "|";
                        strData += AppSettingsManager.GetMappedBnsAddress(sBin) + "|";
                        strData += AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin)) + "|";
                        strData += AppSettingsManager.GetNoticeDate(sBin, "1") + "|";
                        strData += AppSettingsManager.GetNoticeDate(sBin, "2") + "|";
                        strData += AppSettingsManager.GetCDODate(sBin) + "|";
                        strData += AppSettingsManager.GetLastOrNoMapped(result.GetString("bin").Trim()) + "|";
                        strData += AppSettingsManager.GetLastOrDateMapped(result.GetString("bin").Trim()) + "|";
                        strData += string.Format("{0:#,##0.#0}", dAmount) + ";";
                        this.axVSPrinter1.Table = string.Format(">1000|<3250|<4750|<3250|<1000|<1000|<1100|<1000|<1000|>1000;{0}", strData);
                    }
                }
                result.Close();
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Paragraph = "";
                strData = string.Format("{0:#,###}", iCntWithPay);
                this.axVSPrinter1.Table = string.Format("<10000; Businesses with Payments: {0}", strData);
                strData = string.Format("{0:#,###}", iCntWOPay);
                this.axVSPrinter1.Table = string.Format("<10000; Businesses without Payments: {0}", strData);
                strData = string.Format("{0:#,###}", iCntWithPay + iCntWOPay);
                this.axVSPrinter1.Table = string.Format("<10000; Total no. of businesses: {0}", strData);
                strData = string.Format("{0:#,###.#0}", dTotAmtCollected);
                this.axVSPrinter1.Table = string.Format("<10000; Total amount collected: {0}", strData);
            }
            
        }

        private void CreateHeader()
        {
            string strProvinceName = string.Empty;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Table = "^21500;Republic of the Philippines";
            this.axVSPrinter1.FontBold = false;

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                this.axVSPrinter1.Table = string.Format("^21500;{0}", strProvinceName);
            this.axVSPrinter1.Table = string.Format("^21500;{0}", AppSettingsManager.GetConfigValue("09"));
            this.axVSPrinter1.Table = string.Format("^21500;{0}", AppSettingsManager.GetConfigValue("41"));
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^21500;TAX MAPPING DRIVE";
            this.axVSPrinter1.Table = "^21500;Daily Accomplishment Report";

            string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
            this.axVSPrinter1.Table = string.Format("^22500;{0}", sDate);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            //this.axVSPrinter1.Paragraph = "";

            string strData = string.Empty;
            strData = "Barangay: |" + m_sBrgy;
            this.axVSPrinter1.Table = string.Format("<3000|<9500;{0}", strData);
            strData = "Area of Assignment: |" + m_sArea;
            this.axVSPrinter1.Table = string.Format("<3000|<9500;{0}", strData);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            strData = "Land PIN|Bldg. Code|Old BIN|BIN|Name of Taxpayer/Company|Business Name|Line(s) of Business|Business Address\n(State exact Stall/Room No., Name of Building Occupied and Street)|Area|No. of storeys|CY 2011 Buss. Permt/CPLD|Date of Issue|If Business Place Rented/Leased|Remarks";
            this.axVSPrinter1.Table = string.Format("^1000|^1000|^1000|^1000|^2000|^2400|^2100|^2500|^600|^800|^1100|^1000|^2000|^1700;{0}", strData);
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
        }

        private void PrintForm()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            int iBussCnt = 0;
            int iRowCnt = 0;
            string sOldBin = "";
            string sBin = "";
            string sOwnCode = "";
            string sBnsName = "";
            string sBnsCode = "";
            string sBnsAddress = "";
            string sArea = "";
            string sStoreys = "";
            string sPermitNo = "";
            string sPermitDt = "";
            string sPlaceOwner = "";
            string sPlaceAdd = "";
            string sPlaceRent = "";
            string strData = "";
            string sBussCnt = "";
            string sBnsStreet = "";

            pRec.Query = "select * from businesses where bns_brgy = '" + m_sBrgy + "'";
            if (m_sStreet != "ALL")
                pRec.Query += " and " + m_sStreet + " ";
            pRec.Query += " and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            pRec.Query += " order by " + m_sOrderby + "";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iBussCnt++;
                    iRowCnt++;

                    sBin = pRec.GetString("bin").Trim();
                    sOwnCode = pRec.GetString("own_code").Trim();
                    sBnsName = pRec.GetString("bns_nm").Trim();
                    sBnsCode = pRec.GetString("bns_code").Trim();
                    sBnsAddress = AppSettingsManager.GetBnsAddress(sBin);
                    sArea = string.Format("{0:#,###.00}", pRec.GetDouble("flr_area"));
                    if (sArea == ".00")
                        sArea = "";
                    sStoreys = string.Format("{0:####}", pRec.GetInt("num_storeys"));
                    if (sStoreys == "0")
                        sStoreys = "";
                    sPermitNo = pRec.GetString("permit_no");
                    sPermitDt = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("permit_dt"));
                    sOldBin = GetOldBin(sBin);
                    sBnsStreet = pRec.GetString("bns_street");

                    if (pRec.GetString("place_occupancy").Trim() == "RENTED")
                    {
                        sPlaceOwner = pRec.GetString("busn_own");
                        sPlaceRent = string.Format("{0:#,####.00}", pRec.GetDouble("rent_lease_mo"));

                        sList.OwnName = sPlaceOwner;
                        for (int j = 0; j < sList.OwnNamesSetting.Count; j++)
                        {
                            sPlaceAdd = AppSettingsManager.GetBnsOwnAdd(sPlaceOwner);

                            sPlaceOwner = sList.OwnNamesSetting[j].sLn;
                            if (sList.OwnNamesSetting[j].sFn.Trim() != "")
                                sPlaceOwner += ", " + sList.OwnNamesSetting[j].sFn;
                            if (sList.OwnNamesSetting[j].sMi.Trim() != "")
                                sPlaceOwner += " " + sList.OwnNamesSetting[j].sMi;

                        }
                    }
                    else
                    {
                        sPlaceOwner = "";
                        sPlaceRent = "";
                        sPlaceAdd = "";
                    }

                    sBussCnt = string.Format("{0:###}", iBussCnt);
                    string sRentalPlace = "";
                    string sBussLine = "";

                    sRentalPlace = "Owner: " + sPlaceOwner + "\n";
                    sRentalPlace += "Address: " + sPlaceAdd + "\n";
                    sRentalPlace += "Rental: " + sPlaceRent;

                    sBussLine = GetLineOfBusiness(sBin);

                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                    strData = iBussCnt + ")||" + sOldBin + "|" + sBin + "|" + StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(sOwnCode));
                    strData += "|" + sBnsName + "|" + sBussLine;
                    strData += "|" + sBnsAddress + "|" + sArea + "|" + sStoreys + "|" + sPermitNo + "|" + sPermitDt;
                    strData += "|" + sRentalPlace + "|";
                    this.axVSPrinter1.Table = string.Format("<1000|<1000|<1000|<1000|<2000|<2400|<2400|<2500|^600|^800|<1100|<1000|<2700|<1700;{0}", strData);
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                    this.axVSPrinter1.Table = string.Format("<21200;Building Name:");
                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.Paragraph = "";
                 
                    // save print status
                    int iCnt = 0;
                    pSet.Query = "select count(*) from btm_print_status where bns_brgy = '" + m_sBrgy + "'";
                    pSet.Query += " and bns_street = '" + StringUtilities.HandleApostrophe(sBnsStreet) + "'";
                    int.TryParse(pSet.ExecuteScalar(), out iCnt);

                    if (iCnt == 0)
                    {
                        pSet.Query = "insert into btm_print_status values (";
                        pSet.Query += "'" + m_sBrgy + "', ";
                        pSet.Query += "'" + StringUtilities.HandleApostrophe(sBnsStreet) + "', 'Y')";
                        if (pSet.ExecuteNonQuery() == 0)
                        { }
                    }
                    
                    
                }

                
                // for last page
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = string.Format("<2500|<2000|<1000|<2000|<2000;Prepared and Submitted by:|||Validated by:|");
                this.axVSPrinter1.Table = string.Format("<2500|^2000|<1000|<2000|^2000;|Field Worker|||Secretariat");
                
            }
            pRec.Close();
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            if(!m_bInit)
                CreateReportHeaders();

            m_bInit = false;
            if (m_sReportName != "Accomplishment")
                axVSPrinter1.Footer = "TAX MAPPING DRIVE ||Page %d";
        }

        private string GetOldBin(string sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sOldBin = "";

            //pSet.Query = "select * from ref_no_tbl where bin = '" + sBIN + "'";
            pSet.Query = "select * from app_permit_no where bin = '" + sBIN + "'"; //MCR 20150114
            if (pSet.Execute())
            {
                if (pSet.Read())
                    //sOldBin = pSet.GetString("old_bin");
                    sOldBin = pSet.GetString("app_no"); //MCR 20150114
            }
            pSet.Close();

            return sOldBin;
        }

        private string GetLineOfBusiness(string sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBussLine = "";
            string sBnsCode = "";

            int iCnt = 1;
            string sCnt = "";
            sCnt = string.Format("{0:###}", iCnt) + ")";

            sBnsCode = AppSettingsManager.GetBnsCodeMain(sBIN);
            sBussLine = sCnt + " " + AppSettingsManager.GetBnsDesc(sBnsCode);

            pSet.Query = "select * from addl_bns where bin = '" + sBIN + "'";
            pSet.Query += " and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            if (pSet.Execute())
            {
                
                while (pSet.Read())
                {
                    iCnt++;
                    sCnt = string.Format("{0:###}", iCnt) + ")";
                    sBnsCode = pSet.GetString("bns_code_main");
                    sBussLine += "\n" + sCnt + " " + AppSettingsManager.GetBnsDesc(sBnsCode);
                }
            }
            pSet.Close();

            return sBussLine;
        }

        private void PrintBlankForm()
        {
            string strData = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            for (int i = 1; i <= 25; i++)
            {
                this.axVSPrinter1.FontSize = (float)14.0;
                strData = "|||||||||||||";
                this.axVSPrinter1.Table = string.Format("<1000|<1000|<1000|<1000|<2000|<2400|<2100|<2500|^600|^800|<1100|<1000|<2000|<1700;{0}", strData);
            }

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Table = string.Format("<4000|<2000|<1000|<4000|<2000;Prepared and Submitted by (field worker):|||Validated by (secretariat):|");
            //this.axVSPrinter1.Table = string.Format("<2500|^2000|<1000|<2000|^2000;|Field Worker|||Secretariat");
        }

        private void CreateHeaderSummary()
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
                this.axVSPrinter1.Table = string.Format("^16000;{0}", strProvinceName);
            this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("09"));
            this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("41"));
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = "^16000;TAX MAPPING DRIVE";
            if (m_sReportName == "Summary Official")
                this.axVSPrinter1.Table = "^16000;List of Deficient Businesses";
            else if (m_sReportName == "Unofficial")
                this.axVSPrinter1.Table = "^16000;List of Unofficial Businesses - " + m_sReportSwitch;
            else if (m_sReportName == "Not-Mapped")
                this.axVSPrinter1.Table = "^16000;List of Businesses not mapped";
            else if (m_sReportName == "Summary Official Details")
                this.axVSPrinter1.Table = "^16000;List of Deficient Businesses with Details";
            else if (m_sReportName == "Print Matching")
                this.axVSPrinter1.Table = "^16000;Business Mapping Cleansing Tool; Matching " + m_sMatch;

            string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
            this.axVSPrinter1.Table = string.Format("^16000;{0}", sDate);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.Paragraph = "";

            string strData = string.Empty;
            if (m_sReportName != "Print Matching")
                strData = "Barangay: |" + m_sBrgy;
            this.axVSPrinter1.Table = string.Format("<2000|<9500;{0}", strData);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = false;
            if (m_sReportName == "Summary Official")
            {
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                strData = "|With changes in:";
                this.axVSPrinter1.Table = string.Format("^7000|^9000;{0}", strData);
                strData = "BIN|Business Name|Business Address|Bns Name|Bns Address|Owner's Name|Ntr. of Bussiness|Line/s of Bussiness|Lessor's Name|Permit No.|Tax Year|Area|No. of Storeys";
                this.axVSPrinter1.Table = string.Format("^1500|^2500|^3000|^900|^900|^900|^900|^900|^900|^900|^900|^900|^900;{0}", strData);
                //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportName == "Unofficial")
            {
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                strData = "Land PIN|Bldg. Code|Temporary BIN\n /New BIN|Business Name|Business Address|Permit No.|Orgn. Kind|Owner's Name|Owner's Address";
                this.axVSPrinter1.Table = string.Format("^1500|^1300|^1700|^2500|^2500|^1000|^1500|^2000|^2000;{0}", strData);
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportName == "Not-Mapped")
            {
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                strData = "#|Old BIN|BIN|Business Name|Business Address|Permit No.|Owner's Name|Owner's Address";
                this.axVSPrinter1.Table = string.Format("^500|^1500|^1500|^2500|^3000|^1500|^2000|^3000;{0}", strData);
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportName == "Summary Official Details")
            {
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                strData = "BIN|Business Name|Changed Field|Current|New";
                this.axVSPrinter1.Table = string.Format("^1500|^3500|^2000|^4000|^4000;{0}", strData);
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Paragraph = "";
            }
            else if (m_sReportName == "Print Matching")
            {
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                strData = "BIN|Permit No.|Business Name|Business Address|Owner's Name|Owner's Address|Tax Year|Business Status";
                this.axVSPrinter1.Table = string.Format("^2000|^1500|^2500|^2500|^2500|^2500|^1000|^1000;{0}", strData);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }

            this.axVSPrinter1.FontSize = (float)8.0;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
        }

        private void CreateHeaderSummary2()
        {
            string strProvinceName = string.Empty;
            string strData = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Table = "^10000;Republic of the Philippines";
            this.axVSPrinter1.FontBold = false;

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                this.axVSPrinter1.Table = string.Format("^10000;{0}", strProvinceName);
            this.axVSPrinter1.Table = string.Format("^10000;{0}", AppSettingsManager.GetConfigValue("09"));
            this.axVSPrinter1.Table = string.Format("^10000;{0}", AppSettingsManager.GetConfigValue("41"));
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = true;
            if (m_sReportName == "OFFICIAL BUSINESS" || m_sReportName == "UNOFFICIAL BUSINESS")
            {
                this.axVSPrinter1.Table = "^9900;BUSINESS MAPPING ENCODER'S REPORT";
                this.axVSPrinter1.Table = string.Format("^10000;{0}", m_sReportName);
                this.axVSPrinter1.Paragraph = "";    
                strData = "Period covered " + m_sFrom + " to " + m_sTo;
                this.axVSPrinter1.Table = string.Format("^10000;{0}", strData);

                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.FontBold = false;

                this.axVSPrinter1.FontSize = (float)8.0;
                if (m_sReportName == "OFFICIAL BUSINESS")
                    strData = "DATE|BARANGAY|ENCODER|BIN|BUSINESS NAME";
                else
                    strData = "DATE|BARANGAY|ENCODER|TEMPORARY BIN|BUSINESS NAME";

                this.axVSPrinter1.Table = string.Format("^1000|^1500|^2400|^1500|^3500;{0}", strData);
                this.axVSPrinter1.Paragraph = "";
            }
            else
            {
                this.axVSPrinter1.Table = "^10000;TAX MAPPING DRIVE";
                this.axVSPrinter1.Table = "^10000;Summary Report";

                string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                this.axVSPrinter1.Table = string.Format("^10000;As of {0}", sDate);


                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.FontBold = false;
                
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
                /*strData = "|Official Businesses|Unofficial Businesses|Total";
                this.axVSPrinter1.Table = string.Format("^2000|^4700|^2200|^1000;{0}", strData);
                strData = "Barangay|Total Businesses|Mapped|Not Mapped|% Finished|w/ Permit|w/out Permit|% Inc./Dec.";
                this.axVSPrinter1.Table = string.Format("^2000|^1500|^1000|^1200|^1000|^1000|^1200|^1000;{0}", strData);*/
                //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                // RMC Modified Business mapping summary report. Added column for unofficial businesses that complied (s)
                this.axVSPrinter1.FontSize = (float)8.0;
                /*strData = "||Official Businesses|Unofficial Businesses|Total";
                this.axVSPrinter1.Table = string.Format("^1800|^1200|^3000|^4000|^1000;{0}", strData);
                strData = "Barangay|Total Businesses|Mapped|Not Mapped|% Finished|w/ Permit|w/out Permit|Which Applied for New/Renewal Permit|%|% Inc./Dec.";
                this.axVSPrinter1.Table = string.Format("^1800|^1200|^1000|^1000|^1000|^1000|^1000|^1300|^700|^1000;{0}", strData);
                // RMC Modified Business mapping summary report. Added column for unofficial businesses that complied (e)
                 */    // RMC 20120328 modified Business mapping summary report, put rem

                // RMC 20120328 modified Business mapping summary report (s)
                this.axVSPrinter1.MarginLeft = 700;
                strData = "Official Businesses||Unofficial Businesses|Summary"; // GDE 20120717
                //strData = "SUMMARY||Unofficial Businesses";
                //this.axVSPrinter1.Table = string.Format("^5000|^100|^5400;{0}", strData); // GDE 20120717
                //this.axVSPrinter1.Table = string.Format("^7500|^100|^7500;{0}", strData); // GDE 20120802 add no ren and amount
                this.axVSPrinter1.Table = string.Format("^6100|^100|^6900|^2000;{0}", strData);
                //strData = "Barangay|Total Businesses|Mapped|% Finished||Mapped w/ Permit|Applied for Renewal|%|Mapped w/out Permit|Applied for Permit (Discovery)|%";
                //strData = "Current Year Registration|Summary| |"; // GDE 20120802 add amount
                strData = "Current Year Registration|Summary| | | ";
                //this.axVSPrinter1.Table = string.Format("^1800|^1200|^1000|^1000|^100|^1000|^1000|^700|^1000|^1000|^700;{0}", strData); // GDE 20120717
                this.axVSPrinter1.Table = string.Format("^3200|^2900|^100|^6900|^2000;{0}", strData);
                // RMC 20120328 modified Business mapping summary report (e)
                // GDE 20120717 added
                //strData = "Barangay|REN|NEW|RET|Unrenewed Bns|Total Bns|Tax Mapped|% Finish||Discovery w/ Previous / Expired Permit (Not Recorded)|Applied for REN|%|Mapped w/o Permit (Discovery)|Applied for Permit (Discovery)|%"; // GDE 20120802 add amount
                strData = "Barangay|REN|NEW|RET|Unrenewed Bns|Total Bns|Tax Mapped|% Finish||Discovery w/ Previous / Expired Permit (Not Recorded)|Applied for REN|%|Mapped w/o Permit (Discovery)|Applied for Permit (Discovery)|%|With Payment|Amount Collected";
                this.axVSPrinter1.Table = string.Format("^1500|^568|^566|^566|^800|^650|^800|^650|^100|^1250|^1250|^950|^1250|^1250|^950|^700|^1300;{0}", strData);
                                                        
            }
        }

        private void PrintHeaderMapped(string sReportName, string sBrgy)
        {
            string strData = string.Empty;
            
            
            if (sReportName == "MAPPED WITH PERMIT")
            {
                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.Table = string.Format("^15350;LIST OF BUSINESSES MAPPED WITH PERMIT");
                strData = "#|BIN|BUSINESS NAME|BUSINESS ADDRESS|OWNER'S NAME|OWNER'S ADDRESS|PERMIT NO|O.R. NO.|O.R. DATE|AMOUNT";
                this.axVSPrinter1.Table = string.Format("^400|^1700|^2500|^2500|^2200|^2500|^1100|^850|^850|^850;{0}", strData);
            }
            else if (sReportName == "MAPPED WITHOUT PERMIT")
            {
                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.Table = string.Format("^15600;LIST OF BUSINESSES MAPPED WITHOUT PERMIT");
                strData = "#|LAND PIN|TBIN|BUSINESS NAME|BUSINESS ADDRESS|OWNER'S NAME|OWNER'S ADDRESS|BUSINESS TYPE|ORGN";
                this.axVSPrinter1.Table = string.Format("^500|^1500|^1500|^2500|^2500|^2500|^2500|^1000|^1100;{0}", strData);
            }
            else if (sReportName == "MAPPED WITHOUT PERMIT2")
            {
                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.Table = string.Format("^15600;LIST OF BUSINESSES MAPPED WITHOUT PERMIT");
                strData = "#|LAND PIN|TBIN|BUSINESS NAME|BUSINESS ADDRESS|OWNER'S NAME|OWNER'S ADDRESS|BUSINESS TYPE|ORGN";
                this.axVSPrinter1.Table = string.Format("^500|^1500|^1500|^2500|^2500|^2500|^2500|^1000|^1100;{0}", strData);
            }
            else if (sReportName == "MAPPED WITH PERMIT AND NOTICE")
            {
                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.Table = string.Format("^15600;LIST OF BUSINESSES MAPPED WITH PERMIT AND NOTICE");
                strData = "#|BUSINESS NAME|BUSINESS ADDRESS|1ST NOTICE|2ND NOTICE|TAG FOR CDO|OR NO|OR DATE|AMOUNT";
                this.axVSPrinter1.Table = string.Format("^1000|^3750|^4750|^1000|^1000|^1100|^1000|^1000|^1000;{0}", strData);
            }
            else if (sReportName == "MAPPED WITHOUT PERMIT AND NOTICE")
            {
                this.axVSPrinter1.MarginLeft = 500;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                this.axVSPrinter1.Table = string.Format("^18350;LIST OF BUSINESSES MAPPED WITHOUT PERMIT BUT WITH NOTICE");
                strData = "#|BUSINESS NAME|BUSINESS ADDRESS|OWNER'S NAME|1ST NOTICE|2ND NOTICE|TAG FOR CDO|OR NO|OR DATE|AMOUNT";
                this.axVSPrinter1.Table = string.Format("^1000|^3250|^4750|^3250|^1000|^1000|^1100|^1000|^1000|^1000;{0}", strData);
            }     
        }

        private void PrintSummaryReport()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            string sBIN = string.Empty;
            string sBnsName = string.Empty;
            string strData = string.Empty;
            string sApplType = string.Empty;
            string sBnsHouseNo = string.Empty;
            string sBnsStreet = string.Empty;
            string sBnsBrgy = string.Empty;
            string sBnsMun = string.Empty;
            string sBnsAddress = string.Empty;
            int iChBns = 0;
            int iChLoc = 0;
            int iChOwn = 0;
            int iChTyp = 0;
            int iChLin = 0;
            int iChLes = 0;
            int iChPmt = 0;
            int iChTYr = 0;
            int iChAre = 0;
            int iChStr = 0;
            int iCnt = 0;

            pRec.Query = m_sQuery;
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iCnt++;
                    sBIN = pRec.GetString("bin");
                    // view database build-up details
                    sBnsName = AppSettingsManager.GetBnsName(sBIN);
                    sBnsAddress = AppSettingsManager.GetBnsAddress(sBIN);

                    /*sBnsName = pRec.GetString("bns_nm");
                    sBnsHouseNo = pRec.GetString("bns_house_no").Trim();
                    sBnsStreet = pRec.GetString("bns_street").Trim();
                    sBnsBrgy = pRec.GetString("bns_brgy").Trim();
                    sBnsMun = pRec.GetString("bns_mun").Trim();

                    if (sBnsHouseNo == "." || sBnsHouseNo == "")
                        sBnsHouseNo = "";
                    else
                        sBnsHouseNo = sBnsHouseNo + " ";
                    if (sBnsStreet == "." || sBnsStreet == "")
                        sBnsStreet = "";
                    else
                        sBnsStreet = sBnsStreet + ", ";
                    if (sBnsBrgy == "." || sBnsBrgy == "")
                        sBnsBrgy = "";
                    else
                        sBnsBrgy = sBnsBrgy + ", ";
                    if (sBnsMun == "." || sBnsMun == "")
                        sBnsMun = "";

                    sBnsAddress = sBnsHouseNo + sBnsStreet + sBnsBrgy + sBnsMun;*/ 

                    pSet.Query = "select * from btm_update where bin = '" + sBIN + "'";
                    if (pSet.Execute())
                    {
                        
                        string sChBns = "";
                        string sChLoc = "";
                        string sChOwn = "";
                        string sChTyp = "";
                        string sChLin = "";
                        string sChLes = "";
                        string sChPmt = "";
                        string sChTYr = "";
                        string sChAre = "";
                        string sChStr = "";

                        while (pSet.Read())
                        {
                            
                            sApplType = pSet.GetString("appl_type");

                            if (sApplType == "CBNS")
                            {
                                sChBns = "Y";
                                iChBns++;
                            }
                            if (sApplType == "TLOC")
                            {
                                sChLoc = "Y";
                                iChLoc++;
                            }
                            if (sApplType == "TOWN")
                            {
                                sChOwn = "Y";
                                iChOwn++;
                            }
                            if (sApplType == "CTYP")
                            {
                                sChTyp = "Y";
                                iChTyp++;
                            }
                            if (sApplType == "ADDL")
                            {
                                sChLin = "Y";
                                iChLin++;
                            }
                            if (sApplType == "CBOW")
                            {
                                sChLes = "Y";
                                iChLes++;
                            }
                            if (sApplType == "CPMT")
                            {
                                sChPmt = "Y";
                                iChPmt++;
                            }
                            if (sApplType == "CTYR")
                            {
                                sChTYr = "Y";
                                iChTYr++;
                            }
                            if (sApplType == "AREA")
                            {
                                sChAre = "Y";
                                iChAre++;
                            }
                            if (sApplType == "CSTR")
                            {
                                sChStr = "Y";
                                iChStr++;
                            }
                        }

                        this.axVSPrinter1.FontSize = (float)8.0;
                        this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                        strData = sBIN + "|" + sBnsName + "|" + StringUtilities.RemoveOtherCharacters(sBnsAddress) + "|" + sChBns + "|" + sChLoc + "|" + sChOwn + "|" + sChTyp + "|" + sChLin + "|" + sChLes + "|" + sChPmt + "|" + sChTYr + "|" + sChAre + "|" + sChStr;
                        this.axVSPrinter1.Table = string.Format("<1500|<2500|<3000|^900|^900|^900|^900|^900|^900|^900|^900|^900|^900;{0}", strData);
                    }
                    pSet.Close();

                }

            }
            pRec.Close();

            this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            strData = "||Total|";
            strData += string.Format("{0:###}", iChBns) + "|";
            strData += string.Format("{0:###}", iChLoc) + "|";
            strData += string.Format("{0:###}", iChOwn) + "|";
            strData += string.Format("{0:###}", iChTyp) + "|";
            strData += string.Format("{0:###}", iChLin) + "|";
            strData += string.Format("{0:###}", iChLes) + "|";
            strData += string.Format("{0:###}", iChPmt) + "|";
            strData += string.Format("{0:###}", iChTYr) + "|";
            strData += string.Format("{0:###}", iChAre) + "|";
            strData += string.Format("{0:###}", iChStr);

            this.axVSPrinter1.Table = string.Format("<1500|<2500|<3000|^900|^900|^900|^900|^900|^900|^900|^900|^900|^900;{0}", strData);
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            strData = "Total Businesses Mapped in Brgy " + m_sBrgy + " with Changes -  " + string.Format("{0:###}", iCnt);
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);

            pRec.Query = "select count(*) from btm_businesses where bns_brgy = '" + m_sBrgy + "'";
            int.TryParse(pRec.ExecuteScalar(), out iCnt);

            strData = "Total Businesses Mapped in Brgy " + m_sBrgy + " -  " + string.Format("{0:###}", iCnt);
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);
            
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());
        }

        private void axVSPrinter1_BeforeFooter(object sender, EventArgs e)
        {
            this.axVSPrinter1.HdrFontName = "Arial Narrow";
            this.axVSPrinter1.HdrFontSize = (float)10.0;
        }

        private void PrintSummaryUnofficial()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBIN = "";
            string sBnsName = "";
            string sBnsHouseNo = "";
            string sBnsStreet = "";
            string sBnsBrgy = "";
            string sBnsMun = "";
            string sBnsAddress = "";
            string sPermitNo = "";
            string sOwnName = "";
            string sOwnAddress = "";
            string strData = "";
            string sLandPIN = "";
            string sOrgnKind = "";
            string sNewBIN = "";
            int iCnt = 0;

            for (int i = 1; i <= 2; i++)
            {
                if (i == 1)
                {
                    m_sReportSwitch = "Unencoded Businesses (with Permit)";

                    pRec.Query = "select * from btm_temp_businesses where bns_brgy = '" + m_sBrgy + "' ";
                    pRec.Query += " and trim(old_bin) is not null order by bns_nm";
                    iCnt = 0;
                }
                else
                {
                    m_sReportSwitch = "Undeclared Businesses (without Permit)";

                    axVSPrinter1.NewPage();

                    pRec.Query = "select * from btm_temp_businesses where bns_brgy = '" + m_sBrgy + "' ";
                    pRec.Query += " and trim(old_bin) is null order by bns_nm";
                    iCnt = 0;
                }
                if (pRec.Execute())
                {
                    while (pRec.Read())
                    {
                        iCnt++;
                        sBIN = pRec.GetString("tbin");
                        sBnsName = pRec.GetString("bns_nm");
                        sOrgnKind = pRec.GetString("orgn_kind");
                        sBnsHouseNo = pRec.GetString("bns_house_no").Trim();
                        sBnsStreet = pRec.GetString("bns_street").Trim();
                        sBnsBrgy = pRec.GetString("bns_brgy").Trim();
                        sBnsMun = pRec.GetString("bns_mun").Trim();
                        sPermitNo = pRec.GetString("permit_no");
                        sOwnName = pRec.GetString("own_code");
                        sOwnAddress = AppSettingsManager.GetBnsOwnAdd(sOwnName);
                        sOwnName = AppSettingsManager.GetBnsOwner(sOwnName);
                        sLandPIN = GetLandPIN(sBIN);
                        sNewBIN = pRec.GetString("bin");

                        if (sBnsHouseNo == "." || sBnsHouseNo == "")
                            sBnsHouseNo = "";
                        else
                            sBnsHouseNo = sBnsHouseNo + " ";
                        if (sBnsStreet == "." || sBnsStreet == "")
                            sBnsStreet = "";
                        else
                            sBnsStreet = sBnsStreet + ", ";
                        if (sBnsBrgy == "." || sBnsBrgy == "")
                            sBnsBrgy = "";
                        else
                            sBnsBrgy = sBnsBrgy + ", ";
                        if (sBnsMun == "." || sBnsMun == "")
                            sBnsMun = "";

                        sBnsAddress = sBnsHouseNo + sBnsStreet + sBnsBrgy + sBnsMun;

                        this.axVSPrinter1.FontSize = (float)8.0;
                        this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

                        sBIN = "T: " + sBIN;
                        if (sNewBIN.Trim() != "")
                            sBIN += "\nN:" + sNewBIN;
                        strData = sLandPIN + "|" + sBIN + "|" + sBnsName + "|" + StringUtilities.RemoveOtherCharacters(sBnsAddress) + "|" + sPermitNo + "|" + sOrgnKind + "|" + sOwnName + "|" + StringUtilities.RemoveOtherCharacters(sOwnAddress);
                        this.axVSPrinter1.Table = string.Format("<1500|<1300|<1700|<2500|<2500|<1000|<1500|<2000|<2000;{0}", strData);
                    }
                }
                pRec.Close();

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Paragraph = "";
                strData = "|Total count: " + string.Format("{0:###}", iCnt);
                this.axVSPrinter1.Table = string.Format("<11000|<4000;{0}", strData);

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";

                this.axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
                this.axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());
            }
        }

        private string GetLandPIN(string sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sLandPIN = "";

            pSet.Query = "select * from btm_gis_loc where bin = '" + sBIN + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sLandPIN = pSet.GetString("land_pin");
                    sLandPIN += "|" + pSet.GetString("bldg_code");
                }
            }
            pSet.Close();

            return sLandPIN;
        }

        private void PrintSummaryNotMapped()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBIN = "";
            string sBnsName = "";
            string sBnsHouseNo = "";
            string sBnsStreet = "";
            string sBnsBrgy = "";
            string sBnsMun = "";
            string sBnsAddress = "";
            string sPermitNo = "";
            string sOwnName = "";
            string sOwnAddress = "";
            string strData = "";
            string sOldBIN = "";
            int iCnt = 0;

            
            pRec.Query = "select * from businesses where bns_brgy = '" + m_sBrgy + "' ";
            //pRec.Query += " (select bin from btm_businesses) "; // GDE 20120716 as per jester
            pRec.Query += " and bin not in (select bin from btm_businesses where bns_brgy = '" + m_sBrgy.Trim() + "' union select bin from btm_temp_businesses where bns_brgy = '" + m_sBrgy.Trim() + "' and bin <> ' ')";
            pRec.Query += "order by bns_nm";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iCnt++;
                    sBIN = pRec.GetString("bin");
                    sBnsName = pRec.GetString("bns_nm");
                    sBnsHouseNo = pRec.GetString("bns_house_no").Trim();
                    sBnsStreet = pRec.GetString("bns_street").Trim();
                    sBnsBrgy = pRec.GetString("bns_brgy").Trim();
                    sBnsMun = pRec.GetString("bns_mun").Trim();
                    sPermitNo = pRec.GetString("permit_no");
                    sOwnName = pRec.GetString("own_code");
                    sOwnAddress = AppSettingsManager.GetBnsOwnAdd(sOwnName);
                    sOwnName = AppSettingsManager.GetBnsOwner(sOwnName);
                    sOldBIN = GetOldBin(sBIN);
                    
                    if (sBnsHouseNo == "." || sBnsHouseNo == "")
                        sBnsHouseNo = "";
                    else
                        sBnsHouseNo = sBnsHouseNo + " ";
                    if (sBnsStreet == "." || sBnsStreet == "")
                        sBnsStreet = "";
                    else
                        sBnsStreet = sBnsStreet + ", ";
                    if (sBnsBrgy == "." || sBnsBrgy == "")
                        sBnsBrgy = "";
                    else
                        sBnsBrgy = sBnsBrgy + ", ";
                    if (sBnsMun == "." || sBnsMun == "")
                        sBnsMun = "";

                    sBnsAddress = sBnsHouseNo + sBnsStreet + sBnsBrgy + sBnsMun;

                    this.axVSPrinter1.FontSize = (float)8.0;
                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                    strData = iCnt.ToString() + "|" + sOldBIN + "|" + sBIN + "|" + sBnsName + "|" + StringUtilities.RemoveOtherCharacters(sBnsAddress) + "|" + sPermitNo + "|" + sOwnName + "|" + StringUtilities.RemoveOtherCharacters(sOwnAddress);
                    this.axVSPrinter1.Table = string.Format(">500|<1500|<1500|<2500|<3000|<1500|<2000|<3000;{0}", strData);
                }
            }
            pRec.Close();

            int iTotBrgyBns = 0;
            //double dVariance = 0;

            pRec.Query = "select count(*) from businesses where bns_brgy = '" + m_sBrgy + "'";
            int.TryParse(pRec.ExecuteScalar(), out iTotBrgyBns);

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Paragraph = "";
            strData = "Total Businesses not Mapped in Brgy " + m_sBrgy + "-  " + string.Format("{0:###}", iCnt);
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);
            strData = "Total Businesses in Brgy " + m_sBrgy + "-  " + string.Format("{0:###}", iTotBrgyBns);
            this.axVSPrinter1.Table = string.Format("<11000;{0}", strData);
            
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());
        }

        private void PrintSummary()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            double dTotAmt = 0;
            int iTotBnsCnt = 0;
            double dTotRen = 0;
            double dTotNew = 0;
            double dTotRet = 0;
            double dTotUnrenewed = 0;
            string sBrgy = "";
            string sBrgyCode = "";
            string sSectCode = "";
            string strData = "";
            double dCntBns = 0;
            double dCntMapped = 0;
            double dCntUnoff = 0;
            double dCntUnoffNoPermit = 0;
            double dGCntBns = 0;
            double dGCntMapped = 0;
            double dGCntUnoff = 0;
            double dGCntUnoffNoPermit = 0;
            double dPercFin = 0;
            double dPercIncDec = 0;
            double dBussLinked = 0; //this will count unofficial businesses encoded by bptfo
            double dPercLinked = 0; // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
            double dGCntBussLinked = 0; // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
            double dBussLinkedWoutPermit = 0;   // RMC 20120328 modified Business mapping summary report
            double dGCntBussLinkedWoutPermit = 0;   // RMC 20120328 modified Business mapping summary report
            double dPercLinkedWoutPermit = 0;// RMC 20120328 modified Business mapping summary report
            double dBussSectLinkedWoutPermit = 0;

            pRec.Query = "select * from brgy order by brgy_code";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBrgy = pRec.GetString("brgy_nm").Trim();
                    sBrgyCode = pRec.GetString("brgy_code").Trim();

                    pSet.Query = "select count(*) from businesses where bns_brgy = '" + sBrgy + "'";
                    double.TryParse(pSet.ExecuteScalar(), out dCntBns);

                    pSet.Query = "select count(*) from businesses where bns_brgy = '" + sBrgy + "'";
                    //pSet.Query += " and bin in (select bin from btm_temp_businesses)";
                    pSet.Query += " and bin in (select bin from btm_temp_businesses where permit_no is not null)";  // RMC 20120328 modified Business mapping summary report
                    double.TryParse(pSet.ExecuteScalar(), out dBussLinked);

                    dGCntBussLinked += dBussLinked; // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied

                    // RMC 20120328 modified Business mapping summary report (s)
                    pSet.Query = "select count(*) from businesses where bns_brgy = '" + sBrgy + "'";
                    pSet.Query += " and bin in (select bin from btm_temp_businesses where permit_no is null)";
                    double.TryParse(pSet.ExecuteScalar(), out dBussLinkedWoutPermit);

                    dGCntBussLinkedWoutPermit += dBussLinkedWoutPermit;
                    // RMC 20120328 modified Business mapping summary report (e)

                    

                    //dCntBns = dCntBns - dBussLinked;  // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied

                    dGCntBns += dCntBns;

                    pSet.Query = "select count(*) from btm_businesses where bns_brgy = '" + sBrgy + "'";
                    double.TryParse(pSet.ExecuteScalar(), out dCntMapped);

                    // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied (s)
                    double dTemp = 0;
                    // GDE 20120720 temp comment
                    /*
                    pSet.Query = "select count(*) from BTM_TEMP_BUSINESSES where bns_brgy = '" + sBrgy + "'";
                    pSet.Query += " and trim(bin) is not null and bin not in ";
                    pSet.Query += "(SELECT BIN FROM BTM_BUSINESSES)";
                    double.TryParse(pSet.ExecuteScalar(), out dTemp);
                     */ 
                    // GDE 20120720 temp comment
                    dCntMapped += dTemp;
                    // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied (e)

                    dGCntMapped += dCntMapped;

                    pSet.Query = "select count(*) from btm_temp_businesses where bns_brgy = '" + sBrgy + "'";
                    pSet.Query += " and trim(old_bin) is not null";
                    double.TryParse(pSet.ExecuteScalar(), out dCntUnoff);

                    pSet.Query = "select count(*) from btm_temp_businesses where bns_brgy = '" + sBrgy + "'";
                    pSet.Query += " and trim(old_bin) is null";
                    double.TryParse(pSet.ExecuteScalar(), out dCntUnoffNoPermit);


                    dGCntUnoff += dCntUnoff;
                    dGCntUnoffNoPermit += dCntUnoffNoPermit;

                    dPercFin = 0;
                    dPercIncDec = 0;
                    try
                    {
                        dPercFin = (dCntMapped / dCntBns) * 100;
                    }
                    catch
                    {
                        dPercFin = 0;
                    }
                        //dPercIncDec = (((dCntMapped + dCntUnoff + dCntUnoffNoPermit) - dCntBns) / dCntBns) * 100;
                    try
                    {
                        dPercIncDec = (((dCntMapped + dCntUnoff + dCntUnoffNoPermit) - (dCntBns - dBussLinked)) / (dCntBns - dBussLinked)) * 100;   // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                    }
                    catch
                    {
                        dPercIncDec = 0;
                    }
                        //dPercLinked = dBussLinked / (dCntUnoff + dCntUnoffNoPermit) * 100;  // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                    try
                    {
                        dPercLinked = dBussLinked / dCntUnoff * 100;    // RMC 20120328 modified Business mapping summary report
                    }
                    catch
                    {
                        dPercLinked = 0;
                    }
                    try
                    {
                        dPercLinkedWoutPermit = dBussLinkedWoutPermit / dCntUnoffNoPermit * 100;    // RMC 20120328 modified Business mapping summary report
                    }
                    catch
                    {
                        dPercLinkedWoutPermit = 0;
                    }
                   

                    // GDE 20120717 added
                    double dRen = 0;
                    double dNew = 0;
                    double dRet = 0;
                    double dUnRenewed = 0;

                    pSet.Query = "select count(*) from businesses where bns_brgy = '" + sBrgy + "'";
                    pSet.Query += " and bns_stat = 'NEW'";
                    pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
                    double.TryParse(pSet.ExecuteScalar(), out dNew);

                    pSet.Query = "select count(*) from businesses where bns_brgy = '" + sBrgy + "'";
                    pSet.Query += " and bns_stat = 'REN'";
                    pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
                    double.TryParse(pSet.ExecuteScalar(), out dRen);

                    pSet.Query = "select count(*) from businesses where bns_brgy = '" + sBrgy + "'";
                    pSet.Query += " and bns_stat = 'RET'";
                    pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
                    double.TryParse(pSet.ExecuteScalar(), out dRet);

                    pSet.Query = "select count(*) from businesses where bns_brgy = '" + sBrgy + "'";
                    pSet.Query += " and tax_year < '"  + ConfigurationAttributes.CurrentYear + "'";
                    double.TryParse(pSet.ExecuteScalar(), out dUnRenewed);

                    dTotNew += dNew;
                    dTotRen += dRen;
                    dTotRet += dRet;
                    dTotUnrenewed += dUnRenewed;
                    // GDE 20120717 added
                    

                    // GDE 20120717 modified this report
                    //strData = sBrgy + "|";
                    //strData += string.Format("{0:#,###}", dCntBns) + "|";
                    //strData += string.Format("{0:#,###}", dCntMapped) + "|";
                    ////strData += string.Format("{0:#,###}", dCntBns-dCntMapped) + "|";  // RMC 20120328 modified Business mapping summary report, put rem
                    //strData += string.Format("{0:#,###.00}", dPercFin) + "||";
                    //strData += string.Format("{0:#,###}", dCntUnoff) + "|";
                    ///*strData += string.Format("{0:#,###}", dCntUnoffNoPermit) + "|";
                    ////strData += string.Format("{0:#,###.00}", dPercIncDec);
                    //strData += string.Format("{0:#,###}", dBussLinked);   // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                    //strData += "|" + string.Format("{0:#,###.00}", dPercLinked);    // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                    //strData += "|" + string.Format("{0:#,###.00}", dPercIncDec);    // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied    
                    //*/
                    //// RMC 20120328 modified Business mapping summary report, put rem

                    //// RMC 20120328 modified Business mapping summary report, put rem (s)
                    //strData += string.Format("{0:#,###}", dBussLinked);
                    //strData += "|" + string.Format("{0:#,###.00}", dPercLinked);
                    //strData += "|" + string.Format("{0:#,###}", dCntUnoffNoPermit);
                    //strData += "|" + string.Format("{0:#,###}", dBussLinkedWoutPermit);
                    //strData += "|" + string.Format("{0:#,###.00}", dPercLinkedWoutPermit);
                    //// RMC 20120328 modified Business mapping summary report, put rem (e)
                    // GDE 20120717 modified this report

                    if (dBussLinked < 0)
                        dBussLinked = 0;
                    if (dPercLinked < 0)
                        dPercLinked = 0;
                    if (dCntUnoffNoPermit < 0)
                        dCntUnoffNoPermit = 0;
                    if (dBussLinkedWoutPermit < 0)
                        dBussLinkedWoutPermit = 0;
                    if (dPercLinkedWoutPermit < 0)
                        dPercLinkedWoutPermit = 0;

                    int iBnsCount = 0;
                    iBnsCount = AppSettingsManager.GetCountBnsPaid(sBrgy);
                    double dBnsAmt = 0;
                    dBnsAmt = AppSettingsManager.GetAmountBnsPaid(sBrgy);

                    iTotBnsCnt = iTotBnsCnt + iBnsCount;
                    dTotAmt = dTotAmt + dBnsAmt;
                    strData = sBrgy + "|";
                    strData += string.Format("{0:#,###}", dRen) + "|";
                    strData += string.Format("{0:#,###}", dNew) + "|";
                    strData += string.Format("{0:#,###}", dRet) + "|";
                    strData += string.Format("{0:#,###}", dUnRenewed) + "|";
                    strData += string.Format("{0:#,###}", dCntBns) + "|";
                    strData += string.Format("{0:#,###}", dCntMapped) + "|";
                    strData += string.Format("{0:#,###.00}", dPercFin) + "||";
                    strData += string.Format("{0:#,###}", dCntUnoff) + "|";
                    strData += string.Format("{0:#,###}", dBussLinked);
                    strData += "|" + string.Format("{0:#,###.00}", dPercLinked);
                    strData += "|" + string.Format("{0:#,###}", dCntUnoffNoPermit);
                    strData += "|" + string.Format("{0:#,###}", dBussLinkedWoutPermit);
                    strData += "|" + string.Format("{0:#,###.00}", dPercLinkedWoutPermit);
                    strData += "|" + string.Format("{0:#,###}", iBnsCount);
                    strData += "|" + string.Format("{0:#,###.00}", dBnsAmt);


                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                    //this.axVSPrinter1.Table = string.Format("<2000|>1500|>1000|>1200|>1000|>1000|>1200|>1000;{0}", strData);
                    //this.axVSPrinter1.Table = string.Format("<1800|>1200|>1000|>1000|>1000|>1000|>1000|>1300|>700|>1000;{0}", strData);    // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                    //this.axVSPrinter1.Table = string.Format("<1800|>1200|>1000|>1000|^100|>1000|>1000|>700|>1000|>1000|>700;{0}", strData); // RMC 20120328 modified Business mapping summary report // GDE 20120717
                    //this.axVSPrinter1.Table = string.Format("<1800|>568|>566|>566|>1000|>1000|>1000|>1000|>100|>1250|>1250|>1250|>1250|>1250|>1250;{0}", strData); // GDE 20120802 add with payment and amount
                    this.axVSPrinter1.Table = string.Format("<1500|>568|>566|>566|>800|>650|>800|>650|>100|>1250|>1250|>950|>1250|>1250|>950|>700|>1300;{0}", strData);

                    if (m_sReportSwitch == "Section")
                    {
                        double dCntSectMapped = 0;
                        double dCntSectUnOff = 0;
                        double dCntSetcUnOffNoPermit = 0;
                        double dCntSectBussLinked = 0;  // RMC Modified Business mapping summary report. Added column for unofficial businesses that complied

                        if (dCntMapped != 0 || dCntUnoff != 0 || dCntUnoffNoPermit != 0)
                        {
                            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                            this.axVSPrinter1.Paragraph = "";
                            pRec2.Query = "select distinct substr(land_pin,12,3) from btm_gis_loc where substr(land_pin,8,3) = '" + sBrgyCode + "' order by substr(land_pin,12,3)";
                            if (pRec2.Execute())
                            {
                                while (pRec2.Read())
                                {
                                    sSectCode = pRec2.GetString(0);

                                    pSet.Query = "select count(*) from btm_businesses where bin in ";
                                    pSet.Query += "(select bin from btm_gis_loc where substr(land_pin,8,3) = '" + sBrgyCode + "' ";
                                    pSet.Query += "and substr(land_pin,12,3) = '" + sSectCode + "')";
                                    double.TryParse(pSet.ExecuteScalar(), out dCntSectMapped);

                                    pSet.Query = "select count(*) from btm_temp_businesses where tbin in ";
                                    pSet.Query += "(select bin from btm_gis_loc where substr(land_pin,8,3) = '" + sBrgyCode + "' ";
                                    pSet.Query += "and substr(land_pin,12,3) = '" + sSectCode + "') ";
                                    pSet.Query += "and trim(old_bin) is not null";
                                    double.TryParse(pSet.ExecuteScalar(), out dCntSectUnOff);

                                    pSet.Query = "select count(*) from btm_temp_businesses where tbin in ";
                                    pSet.Query += "(select bin from btm_gis_loc where substr(land_pin,8,3) = '" + sBrgyCode + "' ";
                                    pSet.Query += "and substr(land_pin,12,3) = '" + sSectCode + "') ";
                                    pSet.Query += "and trim(old_bin) is null";
                                    double.TryParse(pSet.ExecuteScalar(), out dCntSetcUnOffNoPermit);

                                    // RMC Modified Business mapping summary report. Added column for unofficial businesses that complied (s)
                                    pSet.Query = "select count(*) from btm_temp_businesses where tbin in ";
                                    pSet.Query += "(select bin from btm_gis_loc where substr(land_pin,8,3) = '" + sBrgyCode + "' ";
                                    pSet.Query += "and substr(land_pin,12,3) = '" + sSectCode + "') ";
                                    pSet.Query += "and trim(bin) is not null permit_no is not null";
                                    double.TryParse(pSet.ExecuteScalar(), out dCntSectBussLinked);
                                    // RMC Modified Business mapping summary report. Added column for unofficial businesses that complied (e)

                                    // RMC 20120328 modified Business mapping summary report (s)
                                    pSet.Query = "select count(*) from btm_temp_businesses where tbin in ";
                                    pSet.Query += "(select bin from btm_gis_loc where substr(land_pin,8,3) = '" + sBrgyCode + "' ";
                                    pSet.Query += "and substr(land_pin,12,3) = '" + sSectCode + "') ";
                                    pSet.Query += "and trim(bin) is not null permit_no is null ";
                                    double.TryParse(pSet.ExecuteScalar(), out dBussSectLinkedWoutPermit);

                                    // RMC 20120328 modified Business mapping summary report (e)

                                    this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                                    strData = "|Section " + sSectCode + "|";
                                    strData += string.Format("{0:#,###}", dCntSectMapped) + "|||";
                                    strData += string.Format("{0:#,###}", dCntSectUnOff) + "|";
                                    /*strData += string.Format("{0:#,###}", dCntSetcUnOffNoPermit) + "|";
                                    strData += string.Format("{0:#,###}", dCntSectBussLinked) + "||";*/
                                    // RMC 20120328 modified Business mapping summary report (s)
                                    strData += string.Format("{0:#,###}", dCntSectBussLinked) + "||";
                                    strData += string.Format("{0:#,###}", dCntSetcUnOffNoPermit) + "|";
                                    strData += string.Format("{0:#,###}", dBussSectLinkedWoutPermit) + "|";
                                    // RMC 20120328 modified Business mapping summary report (e)

                                    //this.axVSPrinter1.Table = string.Format("<2000|>1500|>1000|>1200|>1000|>1000|>1200|>1000;{0}", strData);
                                    //this.axVSPrinter1.Table = string.Format("<1800|>1200|>1000|>1000|>1000|>1000|>1000|>1300|>700|>1000;{0}", strData);    // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                                    this.axVSPrinter1.Table = string.Format("<1800|>1200|>1000|>1000|^100|>1000|>1000|>700|>1000|>1000|>700;{0}", strData); // RMC 20120328 modified Business mapping summary report

                                }
                                this.axVSPrinter1.Paragraph = "";
                            }
                            pRec2.Close();
                        }
                    }
                }

                this.axVSPrinter1.Paragraph = "";
                
                dPercFin = 0;
                dPercIncDec = 0;
                try
                {
                    dPercFin = (dGCntMapped / dGCntBns) * 100;
                    //dPercIncDec = (((dGCntMapped + dGCntUnoff + dGCntUnoffNoPermit) - dGCntBns) / dGCntBns) * 100;
                    dPercIncDec = (((dGCntMapped + dGCntUnoff + dGCntUnoffNoPermit) - (dGCntBns - dGCntBussLinked)) / (dGCntBns - dGCntBussLinked)) * 100;  // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                    dPercLinked = dGCntBussLinked / dGCntUnoff * 100;  // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                    dPercLinkedWoutPermit = dGCntBussLinkedWoutPermit / dGCntUnoffNoPermit * 100;   // RMC 20120328 modified Business mapping summary report

                }
                catch { }

                /*strData = "TOTAL|";
                strData += string.Format("{0:#,###}", dGCntBns) + "|";
                strData += string.Format("{0:#,###}", dGCntMapped) + "|";
                strData += string.Format("{0:#,###}", dGCntBns - dGCntMapped) + "|";
                strData += string.Format("{0:#,###.00}", dPercFin) + "|";
                strData += string.Format("{0:#,###}", dGCntUnoff) + "|";
                strData += string.Format("{0:#,###}", dGCntUnoffNoPermit) + "|";
                //strData += string.Format("{0:#,###.00}", dPercIncDec);
                strData += string.Format("{0:#,###}", dGCntBussLinked);   // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                strData += "|" + string.Format("{0:#,###.00}", dPercLinked);    // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                strData += "|" + string.Format("{0:#,###.00}", dPercIncDec);    // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                //this.axVSPrinter1.Table = string.Format("<2000|>1500|>1000|>1200|>1000|>1000|>1200|>1000;{0}", strData);
                this.axVSPrinter1.Table = string.Format("<1800|>1200|>1000|>1000|>1000|>1000|>1000|>1300|>700|>1000;{0}", strData);    // RMC 20120313 Modified Business mapping summary report. Added column for unofficial businesses that complied
                 */

                /*
                // RMC 20120328 modified Business mapping summary report (s)
                strData = "TOTAL|";
                strData += string.Format("{0:#,###}", dGCntBns) + "|";
                strData += string.Format("{0:#,###}", dGCntMapped) + "|";
                strData += string.Format("{0:#,###.00}", dPercFin) + "||";
                strData += string.Format("{0:#,###}", dGCntUnoff) + "|";
                strData += string.Format("{0:#,###}", dGCntBussLinked) + "|";   
                strData += string.Format("{0:#,###.00}", dPercLinked) + "|";   
                strData += string.Format("{0:#,###}", dGCntUnoffNoPermit) + "|";
                strData += string.Format("{0:#,###}", dGCntBussLinkedWoutPermit) + "|";
                strData += string.Format("{0:#,###.00}", dPercLinkedWoutPermit);   
                
                this.axVSPrinter1.Table = string.Format("<1800|>1200|>1000|>1000|^100|>1000|>1000|>700|>1000|>1000|>700;{0}", strData);
                // RMC 20120328 modified Business mapping summary report (e)
                 */

                // GDE 20120717 added
                strData = "TOTAL|";
                strData += string.Format("{0:#,###}", dTotRen) + "|";
                strData += string.Format("{0:#,###}", dTotNew) + "|";
                strData += string.Format("{0:#,###}", dTotRet) + "|";
                strData += string.Format("{0:#,###}", dTotUnrenewed) + "|";
                strData += string.Format("{0:#,###}", dGCntBns) + "|";
                strData += string.Format("{0:#,###}", dGCntMapped) + "|";
                strData += string.Format("{0:#,###.00}", dPercFin) + "||";
                strData += string.Format("{0:#,###}", dGCntUnoff) + "|";
                strData += string.Format("{0:#,###}", dGCntBussLinked);
                strData += "|" + string.Format("{0:#,###.00}", dPercLinked);
                strData += "|" + string.Format("{0:#,###}", dGCntUnoffNoPermit);
                strData += "|" + string.Format("{0:#,###}", dGCntBussLinkedWoutPermit);
                strData += "|" + string.Format("{0:#,###.00}", dPercLinkedWoutPermit);
                strData += "|" + string.Format("{0:#,###}", iTotBnsCnt);
                strData += "|" + string.Format("{0:#,###.00}", dTotAmt);
                this.axVSPrinter1.Table = string.Format("<1500|>568|>566|>566|>800|>650|>800|>650|>100|>1250|>1250|>950|>1250|>1250|>950|>700|>1300;{0}", strData);
                // GDE 20120717 added
            }
            pRec.Close();

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = string.Format("<1500|<3000|<1500|<3000;Printed by:|{0}|Date printed:|{1}", AppSettingsManager.SystemUser.UserCode, AppSettingsManager.GetCurrentDate());
        }

        private void PrintSummaryDetailedReport()
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
                        
            pRec.Query = m_sQuery;
            if (pRec.Execute())
            {
                while (pRec.Read())
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
                                sNew = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("permit_dt"));
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
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());
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
                    try
                    {
                        sPermitDt = string.Format("{0:MM/dd/yyyy}", result.GetDateTime(1));
                    }
                    catch
                    {
                        sPermitDt = "";
                    }
                }
            }
            result.Close();

            if (bNo)
                return sPermitNo;
            else
                return sPermitDt;
        }

        private void PrintEncodersReport()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sContent = "";
            string sEncoder = "";
            string sBrgy = "";
            string sBIN = "";
            string sBnsNm = "";
            string sDate = "";
            string sRecCnt = "";
            DateTime dDate;
            int iRecCnt = 0; 

            pRec.Query = m_sQuery;
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iRecCnt++;
                    sBrgy = pRec.GetString(0);
                    sEncoder = pRec.GetString(1);
                    sBIN = pRec.GetString(2);
                    sBnsNm = pRec.GetString(3);
                    dDate = pRec.GetDateTime(4);
                    sDate = string.Format("{0:MM/dd/yyyy}", dDate);

                    sEncoder = AppSettingsManager.GetUserName(sEncoder);

                    this.axVSPrinter1.FontSize = (float)8.0;
                    sContent = sDate + "|" + sBrgy + "|" + sEncoder + "|" + sBIN + "|" + sBnsNm;
                    this.axVSPrinter1.Table = string.Format("<1000|<1500|<2400|<1500|<3500;{0}", sContent);
                }

                sRecCnt = string.Format("{0:###}", iRecCnt);

                this.axVSPrinter1.Paragraph = "";
                sContent = "Total encoded records: " + sRecCnt;
                this.axVSPrinter1.Table = string.Format("<1100;{0}", sContent);
            }
            pRec.Close();

            this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());
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

        private void CreateReportHeaders()
        {
            if (m_sReportName == "Accomplishment")
                CreateHeader();
            else if (m_sReportName == "Summary" || m_sReportName == "OFFICIAL BUSINESS" || m_sReportName == "UNOFFICIAL BUSINESS")
                CreateHeaderSummary2();
            else
                CreateHeaderSummary();
            if (m_sReportName == "MAPPED WITH PERMIT")
                PrintHeaderMapped(m_sReportName, m_sBrgy);
            else if (m_sReportName == "MAPPED WITHOUT PERMIT")
                PrintHeaderMapped(m_sReportName, m_sBrgy);
            else if (m_sReportName == "MAPPED WITHOUT PERMIT2")
                PrintHeaderMapped(m_sReportName, m_sBrgy);
            else if (m_sReportName == "MAPPED WITH PERMIT AND NOTICE")
                PrintHeaderMapped(m_sReportName, m_sBrgy);
            else if (m_sReportName == "MAPPED WITHOUT PERMIT AND NOTICE")
                PrintHeaderMapped(m_sReportName, m_sBrgy);
        }

        private void PrintCleansingTool()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            string sBIN = "";
            string sPermitNo = "";
            string sBnsName = "";
            string sBnsAdd = "";
            string sOwnName = "";
            string sOwnAdd = "";
            string sTaxYear = "";
            string sBnsStat = "";
            string sOwnCode = "";
            string sContent = "";
            string sCnt = "";
            int iCnt = 0;
            int iCntUnOff = 0;

            pRec.Query = m_sQuery;
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iCnt++;
                    sBIN = pRec.GetString("bin");
                    sPermitNo = pRec.GetString("permit_no");
                    sBnsName = pRec.GetString("bns_nm");
                    sBnsAdd = AppSettingsManager.GetBnsAddress(sBIN);
                    sOwnCode = pRec.GetString("own_code");
                    sOwnName = AppSettingsManager.GetBnsOwner(sOwnCode);
                    sOwnAdd = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                    sTaxYear = pRec.GetString("tax_year");
                    sBnsStat = pRec.GetString("bns_stat");

                    this.axVSPrinter1.Paragraph = "";
                    this.axVSPrinter1.FontBold = true;
                    sCnt = string.Format("{0:###}", iCnt);

                    sContent = sCnt + ".)" + sBIN + "|" + sPermitNo + "|" + sBnsName + "|" + sBnsAdd + "|" + sOwnName + "|" + sOwnAdd + "|" + sTaxYear + "|" + sBnsStat;
                    this.axVSPrinter1.Table = string.Format("<2000|<1500|<2500|<2500|<2500|<2500|<1000|<1000;{0}", sContent);
                    this.axVSPrinter1.Paragraph = "";

                    this.axVSPrinter1.FontBold = false;
                    this.axVSPrinter1.Table = string.Format("<3000;Mapped un-official businesss/es");
                    this.axVSPrinter1.Paragraph = "";

                    if (m_sMatch == "by Permit No.")
                    {
                        pSet.Query = "select * from btm_temp_businesses where permit_no = '" + sPermitNo + "' and trim(bin) is null order by tbin";
                    }
                    else if (m_sMatch == "by Business Name")
                    {
                        //pSet.Query = "select * from btm_temp_businesses where bns_nm = '" + sBnsName + "' and trim(bin) is null order by tbin";
                        pSet.Query = "select * from btm_temp_businesses where bns_nm = '" + StringUtilities.HandleApostrophe(sBnsName) + "' and trim(bin) is null order by tbin"; // ALJ 20120629 put handle apostrophe (matching report by business name)
                        
                    }
                    else if (m_sMatch == "by Owner's Name")
                    {
                        pSet.Query = "select * from btm_temp_businesses where own_code = '" + sOwnCode + "' and trim(bin) is null order by tbin";
                    }
                    if (pSet.Execute())
                    {
                        string sAddress = "";
                        string sBnsHouseNo = "";
                        string sBnsStreet = "";
                        string sBnsBrgy = "";
                        string sBnsMun = "";

                        while (pSet.Read())
                        {
                            iCntUnOff++;
                            sBIN = pSet.GetString("tbin");
                            sPermitNo = pSet.GetString("permit_no");
                            sBnsName = pSet.GetString("bns_nm");
                            sOwnCode = pSet.GetString("own_code");
                            sBnsHouseNo = pSet.GetString("bns_house_no").Trim();
                            sBnsStreet = pSet.GetString("bns_street").Trim();
                            sBnsBrgy = pSet.GetString("bns_brgy").Trim();
                            sBnsMun = pSet.GetString("bns_mun").Trim();

                            if (sBnsHouseNo == "." || sBnsHouseNo == "")
                                sBnsHouseNo = "";
                            else
                                sBnsHouseNo = sBnsHouseNo + " ";
                            if (sBnsStreet == "." || sBnsStreet == "")
                                sBnsStreet = "";
                            else
                                sBnsStreet = sBnsStreet + ", ";
                            if (sBnsBrgy == "." || sBnsBrgy == "")
                                sBnsBrgy = "";
                            else
                                sBnsBrgy = sBnsBrgy + ", ";
                            if (sBnsMun == "." || sBnsMun == "")
                                sBnsMun = "";

                            sAddress = sBnsHouseNo + sBnsStreet + sBnsBrgy + sBnsMun;

                            sContent = sBIN + "|" + sPermitNo + "|" + sBnsName + "|" + sAddress + "|" + AppSettingsManager.GetBnsOwner(sOwnCode) + "|" + AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                            this.axVSPrinter1.Table = string.Format("<2000|<1500|<2500|<2500|<2500|<2500;{0}", sContent);
                        }
                    }
                    pSet.Close();
                }
            }
            pRec.Close();

            this.axVSPrinter1.Paragraph = "";

            sCnt = string.Format("{0:#,###}", iCntUnOff);

            this.axVSPrinter1.Table = string.Format("<3500|<2500|<2500|<2500|<2500;Total Un-official businesses not yet matched:| {0}", sCnt);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.Table = string.Format("<1500|<3000|<1500|<3000;Printed by:|{0}|Date printed:|{1}", AppSettingsManager.SystemUser.UserCode, AppSettingsManager.GetCurrentDate());
        }

        
    }
}