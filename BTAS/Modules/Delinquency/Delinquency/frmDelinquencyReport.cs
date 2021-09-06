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
using System.Threading;
using Amellar.Common.DynamicProgressBar;

namespace Amellar.Modules.Delinquency
{
    public partial class frmDelinquencyReport : Form
    {
        public frmDelinquencyReport()
        {
            InitializeComponent();
        }

        private string m_sQuery = String.Empty;
        private string m_sReportName = String.Empty;
        public string Query
        {
            set { m_sQuery = value; }
        }
        public string ReportName
        {
            set { m_sReportName = value; }
        }
        int m_iPage = 0;

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
        //MCR 20140714 (e)

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

        private bool m_bInit = true;

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDelinquencyReport_Load(object sender, EventArgs e)
        {
            if (m_sReportName.Contains("UNRENEWED"))
            {
                ExportFile();
                m_bInit = false;
                this.axVSPrinter1.MarginLeft = 700;
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; //pprFanfoldUS
                this.axVSPrinter1.MarginBottom = 700;
                this.axVSPrinter1.MarginTop = 700;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

                UnrenewedBusiness();
            }
            else if (m_sReportName.Contains("SUMMARY"))
            {
                ExportFile();
                this.axVSPrinter1.MarginLeft = 400;
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; //pprFanfoldUS
                this.axVSPrinter1.MarginBottom = 700;
                this.axVSPrinter1.MarginTop = 700;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                CreateHeader();
                SummaryDelinquent();
            }
            else if (m_sReportName.Contains("LIST"))
            {
                m_bInit = false;
                this.axVSPrinter1.MarginLeft = 400;
                this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; //pprFanfoldUS
                this.axVSPrinter1.MarginBottom = 700;
                this.axVSPrinter1.MarginTop = 700;
                this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
                this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

                ListDelinquent();
            }
        }

        private void ListDelinquent()
        {
            String sRank = "", sDataGroup = "", sDataGroup2 = "", sBin = "", sBnsName = "", sBnsAdd = "", sOwnName = "", sOwnAdd = "", sBnsCode = "", sOrNo = "", sOrDate = "", sPaymentTerm = "", sTaxYear = "", sQtrPaid = "", sCutoffDate = "", sTempDataGroup = "";
            double dGross = 0, dCapital = 0, dBtax = 0, dFeesCharges = 0, dSurchInt = 0, dTotal = 0;
            double dSubTotalBtax = 0, dSubTotalFeesCharges = 0, dSubTotalSurchInt = 0, dSubTotalTotal = 0;
            double dTotalBtax = 0, dTotalFeesCharges = 0, dTotalSurchInt = 0, dTotalTotal = 0;
            int iCount = 0;
            String sHeader = "", sHeaderColumn = "";
            bool m_bEntered = false;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            DoSomethingDifferent(frmListDelinquency.intCountSaved, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = m_sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sHeader = "<1700|<3000|<3000|^900|>1300|>1300|>1300|>1400|>1400|^1000|^1000|^550|^600|^550;";
                    iCount += 1;

                    if (m_sReportName.Contains("TOP"))
                    {
                        sRank = pSet.GetString("MAX");
                        sHeader = "^700|" + sHeader;
                    }

                    sDataGroup = pSet.GetString("group1");
                    sDataGroup2 = pSet.GetString("group2");
                    sBin = pSet.GetString("bin");
                    sBnsName = pSet.GetString("bns_nm");
                    sBnsAdd = pSet.GetString("bns_address");
                    sOwnName = pSet.GetString("own_nm");
                    sOwnAdd = pSet.GetString("own_address");
                    sBnsCode = pSet.GetString("bns_code");
                    dGross = pSet.GetDouble("gross");
                    dCapital = pSet.GetDouble("capital");
                    dBtax = pSet.GetDouble("btax");
                    dFeesCharges = pSet.GetDouble("fees_charges");
                    dSurchInt = pSet.GetDouble("surch_int");
                    sOrNo = pSet.GetString("or_no");
                    sOrDate = pSet.GetDateTime("or_date").ToShortDateString();
                    sPaymentTerm = pSet.GetString("payment_term");
                    sTaxYear = pSet.GetString("tax_year");
                    sQtrPaid = pSet.GetString("qtr_paid");
                    sCutoffDate = pSet.GetString("cut_off_dt");

                    dTotal = dBtax + dFeesCharges + dSurchInt;

                    dTotalBtax += dBtax;
                    dTotalFeesCharges += dFeesCharges;
                    dTotalSurchInt += dSurchInt;
                    dTotalTotal += dTotal;

                    if (sTempDataGroup != sDataGroup)
                    {
                        if (m_bEntered == true)
                        {
                            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;

                            if (m_sReportName.Contains("TOP"))
                                this.axVSPrinter1.Table = sHeader + "|||||Sub Total|" + dSubTotalBtax.ToString("#,##0.00") + "|" + dSubTotalFeesCharges.ToString("#,##0.00") + "|" + dSubTotalSurchInt.ToString("#,##0.00") + "|" + dSubTotalTotal.ToString("#,##0.00") + "||||";
                            else
                                this.axVSPrinter1.Table = sHeader + "||||Sub Total|" + dSubTotalBtax.ToString("#,##0.00") + "|" + dSubTotalFeesCharges.ToString("#,##0.00") + "|" + dSubTotalSurchInt.ToString("#,##0.00") + "|" + dSubTotalTotal.ToString("#,##0.00") + "||||";
                            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                            this.axVSPrinter1.Paragraph = "";
                        }

                        dSubTotalBtax = 0;
                        dSubTotalFeesCharges = 0;
                        dSubTotalSurchInt = 0;
                        dSubTotalTotal = 0;
                        this.axVSPrinter1.FontBold = true;
                        this.axVSPrinter1.FontSize = 10;
                        this.axVSPrinter1.Table = "<5000;" + sDataGroup;
                        this.axVSPrinter1.FontSize = 8;
                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.Paragraph = "";

                        if (m_sReportName.Contains("TOP"))
                            sHeaderColumn = sHeader + Convert.ToInt32(sDataGroup2).ToString("#,##0") + "|";
                        else
                            sHeaderColumn = sHeader;

                        if (sDataGroup2 == "NEW")
                            this.axVSPrinter1.Table = sHeaderColumn + sBin + "|" + sBnsName + "\n" + sBnsAdd + "|" + sOwnName + "\n" + sOwnAdd + "|" + sBnsCode + "\n" + sDataGroup2 + "|" + dCapital.ToString("#,##0.00") + "|" + dBtax.ToString("#,##0.00") + "|" + dFeesCharges.ToString("#,##0.00") + "|" + dSurchInt.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sPaymentTerm + "|" + sTaxYear + "|" + sQtrPaid;
                        else // RET AND REN
                            this.axVSPrinter1.Table = sHeaderColumn + sBin + "|" + sBnsName + "\n" + sBnsAdd + "|" + sOwnName + "\n" + sOwnAdd + "|" + sBnsCode + "\n" + sDataGroup2 + "|" + dGross.ToString("#,##0.00") + "|" + dBtax.ToString("#,##0.00") + "|" + dFeesCharges.ToString("#,##0.00") + "|" + dSurchInt.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sPaymentTerm + "|" + sTaxYear + "|" + sQtrPaid;

                        this.axVSPrinter1.Paragraph = "";

                        sTempDataGroup = sDataGroup;
                        m_bEntered = true;
                    }
                    else
                    {
                        if (m_sReportName.Contains("TOP"))
                            sHeaderColumn = sHeader + sDataGroup2 + "|";
                        else
                            sHeaderColumn = sHeader;
                        if (sDataGroup2 == "NEW")
                            this.axVSPrinter1.Table = sHeaderColumn + sBin + "|" + sBnsName + "\n" + sBnsAdd + "|" + sOwnName + "\n" + sOwnAdd + "|" + sBnsCode + "\n" + sDataGroup2 + "|" + dCapital.ToString("#,##0.00") + "|" + dBtax.ToString("#,##0.00") + "|" + dFeesCharges.ToString("#,##0.00") + "|" + dSurchInt.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sPaymentTerm + "|" + sTaxYear + "|" + sQtrPaid;
                        else // RET AND REN
                            this.axVSPrinter1.Table = sHeaderColumn + sBin + "|" + sBnsName + "\n" + sBnsAdd + "|" + sOwnName + "\n" + sOwnAdd + "|" + sBnsCode + "\n" + sDataGroup2 + "|" + dGross.ToString("#,##0.00")  + "|" + dBtax.ToString("#,##0.00") + "|" + dFeesCharges.ToString("#,##0.00") + "|" + dSurchInt.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00") + "|" + sOrNo + "|" + sOrDate + "|" + sPaymentTerm + "|" + sTaxYear + "|" + sQtrPaid;
                        this.axVSPrinter1.Paragraph = "";
                    }

                    dSubTotalBtax += dBtax;
                    dSubTotalFeesCharges += dFeesCharges;
                    dSubTotalSurchInt += dSurchInt;
                    dSubTotalTotal += dTotal;
                    DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(frmListDelinquency.intCountSaved)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                }
            }
            pSet.Close();
            
            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            if (m_bEntered == true)
            {
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;

                if (m_sReportName.Contains("TOP"))
                    this.axVSPrinter1.Table = sHeader + "|||||Sub Total|" + dSubTotalBtax.ToString("#,##0.00") + "|" + dSubTotalFeesCharges.ToString("#,##0.00") + "|" + dSubTotalSurchInt.ToString("#,##0.00") + "|" + dSubTotalTotal.ToString("#,##0.00") + "||||";
                else
                    this.axVSPrinter1.Table = sHeader + "||||Sub Total|" + dSubTotalBtax.ToString("#,##0.00") + "|" + dSubTotalFeesCharges.ToString("#,##0.00") + "|" + dSubTotalSurchInt.ToString("#,##0.00") + "|" + dSubTotalTotal.ToString("#,##0.00") + "||||";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Paragraph = "";
            }

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            if (m_sReportName.Contains("TOP"))
                this.axVSPrinter1.Table = sHeader + "|||||Total|" + dTotalBtax.ToString("#,##0.00") + "|" + dTotalFeesCharges.ToString("#,##0.00") + "|" + dTotalSurchInt.ToString("#,##0.00") + "|" + dTotalTotal.ToString("#,##0.00") + "||||";
            else
                this.axVSPrinter1.Table = sHeader + "||||Total|" + dTotalBtax.ToString("#,##0.00") + "|" + dTotalFeesCharges.ToString("#,##0.00") + "|" + dTotalSurchInt.ToString("#,##0.00") + "|" + dTotalTotal.ToString("#,##0.00") + "||||";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<3000; Total Number of " + iCount.ToString("#,##0");
            this.axVSPrinter1.Table = "<1700|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<1700|<5000;Date printed :|" + AppSettingsManager.MonthsInWords(AppSettingsManager.GetSystemDate());
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void SummaryDelinquent()
        {
            String sDataGroup = "", sCutOfDate = "";
            double dRenGross = 0, dNewCap = 0, dRetGross = 0, dTotalAmount = 0;
            int iRenNo = 0, iNewno = 0, iRetNo = 0, iTotalNo = 0;
            double dGrandRenGross = 0, dGrandNewCap = 0, dGrandRetGross = 0, dGrandTotalAmount = 0;
            int iGrandRenNo = 0, iGrandNewno = 0, iGrandRetNo = 0, iGrandTotalNo = 0;

            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();
            Thread.Sleep(500);
            DoSomethingDifferent(frmSummaryDelinquency.intCountSaved, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = m_sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    sDataGroup = pSet.GetString(1);
                    iRenNo = pSet.GetInt(2);
                    dRenGross = pSet.GetDouble(3);
                    iNewno = pSet.GetInt(4);
                    dNewCap = pSet.GetDouble(5);
                    iRetNo = pSet.GetInt(6);
                    dRetGross = pSet.GetDouble(7);
                    sCutOfDate = pSet.GetString(8);
                    iTotalNo = iRenNo + iNewno + iRetNo;
                    dTotalAmount = dRenGross + dRetGross + dNewCap;

                    dGrandNewCap += dNewCap;
                    dGrandRenGross += dRenGross;
                    dGrandRetGross += dRetGross;
                    dGrandTotalAmount += dTotalAmount;

                    iGrandNewno += iNewno;
                    iGrandRenNo += iRenNo;
                    iGrandRetNo += iRetNo;
                    iGrandTotalNo += iTotalNo;

                    this.axVSPrinter1.Table = "<1700|>700|>1700|>700|>1700|>700|>1700|>700|>1700;" + sDataGroup + "|" + iRenNo.ToString("#,##0") + "|" + dRenGross.ToString("#,##0.00") + "|" + iNewno.ToString("#,##0") + "|" + dNewCap.ToString("#,##0.00") + "|" + iRetNo.ToString("#,##0") + "|" + dRetGross.ToString("#,##0.00") + "|" + iTotalNo.ToString("#,##0") + "|" + dTotalAmount.ToString("#,##0.00");
                    this.axVSPrinter1.Paragraph = "";

                    DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(frmSummaryDelinquency.intCountSaved)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            pSet.Close();
            
            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);

            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            this.axVSPrinter1.Table = ">1700|>700|>1700|>700|>1700|>700|>1700|>700|>1700; Total|" + iGrandRenNo.ToString("#,##0") + "|" + dGrandRenGross.ToString("#,##0.00") + "|" + iGrandNewno.ToString("#,##0") + "|" + dGrandNewCap.ToString("#,##0.00") + "|" + iGrandRetNo.ToString("#,##0") + "|" + dGrandRetGross.ToString("#,##0.00") + "|" + iGrandTotalNo.ToString("#,##0") + "|" + dGrandTotalAmount.ToString("#,##0.00");
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Table = "<1700|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<1700|<5000;Date printed :|" + AppSettingsManager.MonthsInWords(AppSettingsManager.GetSystemDate());
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void UnrenewedBusiness()
        {
            String sDataGroup = "", sBin = "", sBnsName = "", sBnsAdd = "", sOwnName = "", sOwnAdd = "", sBnsStat = "", sLastYear = "", sLastDate = "", sLastOr = "", sPayType = "", sQtrPaid = "", sCurrentUser = "", sReportName = "";
            String sTempDataGroup = " ";
            int iCount = 0, iTotalCount = 0, iDistCount = 0;
            int iCtr = 0, iProgressCtr = 0;

            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = m_sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    frmUnrenewedBusiness.DoSomethingDifferent(frmUnrenewedBusiness.intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmUnrenewedBusiness.m_form.ProgressBarWork);
                    frmUnrenewedBusiness.intCountIncreament += 1;

                    sDataGroup = pSet.GetString(1);
                    iCount = pSet.GetInt(2);
                    sBin = pSet.GetString(3);
                    sBnsName = pSet.GetString(4);
                    sBnsAdd = pSet.GetString(5);
                    sOwnName = pSet.GetString(6);
                    sOwnAdd = pSet.GetString(7);
                    sBnsStat = pSet.GetString(8);
                    sLastYear = pSet.GetString(9);
                    sLastDate = pSet.GetString(10);
                    sLastOr = pSet.GetString(11);
                    sPayType = pSet.GetString(12);
                    sQtrPaid = pSet.GetString(13);

                    if (m_sReportName.Contains("DISTRICT") && sTempDataGroup == " ")
                    {
                        this.axVSPrinter1.FontBold = true;
                        this.axVSPrinter1.Table = "<2200|<5000|<5000; District Name|" + sDataGroup;
                        this.axVSPrinter1.FontBold = false;
                        sTempDataGroup = "";
                    }

                    if (sTempDataGroup != sDataGroup)
                    {
                        this.axVSPrinter1.FontBold = true;
                        if (m_sReportName.Contains("MAIN BUSINESS"))
                            this.axVSPrinter1.Table = "<2200|<5000|<5000; Business Description|" + sDataGroup + "| Total " + iCount;
                        else if (m_sReportName.Contains("BARANGAY"))
                            this.axVSPrinter1.Table = "<2200|<5000|<5000; Barangay Name|" + sDataGroup + "| Total " + iCount;

                        this.axVSPrinter1.FontBold = false;
                        this.axVSPrinter1.Paragraph = "";
                        this.axVSPrinter1.Table = "<2200|<5000|<5000|^900|^1300|^1300|^1300|^1000|^900;" + sBin + "|" + sBnsName + "\n" + sBnsAdd + "|" + sOwnName + "\n" + sOwnAdd + "|" + sBnsStat + "|" + sLastYear + "|" + sLastDate + "|" + sLastOr + "|" + sPayType + "|" + sQtrPaid;
                        this.axVSPrinter1.Paragraph = "";
                        sTempDataGroup = sDataGroup;
                        iTotalCount += iCount;
                    }
                    else
                    {
                        iDistCount += 1;
                        this.axVSPrinter1.Table = "<2200|<5000|<5000|^900|^1300|^1300|^1300|^1000|^900;" + sBin + "|" + sBnsName + "\n" + sBnsAdd + "|" + sOwnName + "\n" + sOwnAdd + "|" + sBnsStat + "|" + sLastYear + "|" + sLastDate + "|" + sLastOr + "|" + sPayType + "|" + sQtrPaid;
                        this.axVSPrinter1.Paragraph = "";
                    }                               //1    2     3     4    5     6      7     8    9 

                    frmUnrenewedBusiness.DoSomethingDifferent(string.Format("{0:#0} %", (Convert.ToDouble(frmUnrenewedBusiness.intCountIncreament) / Convert.ToDouble(frmUnrenewedBusiness.intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmUnrenewedBusiness.m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            pSet.Close();

            frmUnrenewedBusiness.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmUnrenewedBusiness.m_form.ProgressBarWork);
            Thread.Sleep(10);

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = false;
            if (m_sReportName.Contains("DISTRICT"))
                //this.axVSPrinter1.Table = "<12200|<1000;Total Record(s) Retrive :|" + iDistCount.ToString("#,##0");
                this.axVSPrinter1.Table = "<12200|<1000;Total Record(s) Retrieve :|" + iDistCount.ToString("#,##0");    // RMC 20170607 corrected spelling in message prompt
            else
                //this.axVSPrinter1.Table = "<2200|<1000;Total Record(s) Retrive :|" + iTotalCount.ToString("#,##0");
                this.axVSPrinter1.Table = "<2200|<1000;Total Record(s) Retrieve :|" + iTotalCount.ToString("#,##0"); // RMC 20170607 corrected spelling in message prompt
            this.axVSPrinter1.Table = "<2200|<1000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
            this.axVSPrinter1.Table = "<2200|<1000;Date printed :|" + AppSettingsManager.MonthsInWords(AppSettingsManager.GetSystemDate());
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void CreateHeader()
        {
            if (m_sReportName.Contains("UNRENEWED"))
            {
                string strProvinceName = string.Empty;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontName = "Arial Narrow";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.FontSize = (float)10.0;
                this.axVSPrinter1.Table = "^18900;Republic of the Philippines";
                this.axVSPrinter1.FontBold = false;

                strProvinceName = AppSettingsManager.GetConfigValue("08");
                if (strProvinceName != string.Empty)
                    this.axVSPrinter1.Table = string.Format("^18900;{0}", strProvinceName);
                this.axVSPrinter1.Table = string.Format("^18900;{0}", AppSettingsManager.GetConfigValue("09"));

                this.axVSPrinter1.FontName = "Arial";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = "^18900;" + m_sReportName;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^18900;As of " + AppSettingsManager.GetSystemDate().ToString("MMMM dd, yyyy"));
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                //this.axVSPrinter1.Table = ("^2200|^5000|^5000|^900|^1300|^1300|^1300|^1000|^900;Bin|Business Name/ Address|Owner'S Name/ Address|Status|Last Year Of|Last Date Of Payment|Or Number|Payment\nType|Qtr\nPaid");
                this.axVSPrinter1.Table = ("^2200|^5000|^5000|^900|^1300|^1300|^1300|^1000|^900;Bin|Business Name/ Address|Owner'S Name/ Address|Status|Last Year Renewed|Last Date Of Payment|Or Number|Payment\nType|Qtr\nPaid"); // RMC 20170607 modified column header in Unrenewd business report
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
            }
            else if (m_sReportName.Contains("SUMMARY"))
            {
                String sHeader = "^1700|^700|^1700|^700|^1700|^700|^1700|^700|^1700;";

                m_iPage += 1;
                String sHeader2 = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontName = "Arial Narrow";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.FontSize = (float)10.0;
                this.axVSPrinter1.Table = "^11300;Republic of the Philippines";
                this.axVSPrinter1.FontBold = false;
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader2 = "^11300;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader2 = "^11300;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = sHeader2;

                this.axVSPrinter1.FontName = "Arial";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = "^11300;" + m_sReportName;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("<5000|>6300;As of " + AppSettingsManager.GetSystemDate().ToString("MMMM dd, yyyy") + "| Page" + m_iPage);
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBox;
                this.axVSPrinter1.Table = ("^1700|^2400|^2400|^2400|^2400; |R E N E W A L|N E W|R E T I R E D|T O T A L");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                if (m_sReportName.Contains("BARANGAY"))
                    sHeader += "Barangay";
                else if (m_sReportName.Contains("DISTRICT"))
                    sHeader += "District";
                else if (m_sReportName.Contains("MAIN BUSINESS"))
                    sHeader += "Business Types";
                else if (m_sReportName.Contains("OWNERSHIP KIND"))
                    sHeader += "Organization Kind";
                sHeader += "|No.|Gross Receipts|No.|Capital|No.|Gross Receipts|No.|Gross Capital";

                this.axVSPrinter1.Table = sHeader;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
            }
            else if (m_sReportName.Contains("LIST"))
            {
                String sHeader = "^1700|^3000|^3000|^900|^1300|^1300|^1300|^1400|^1400|^1000|^1000|^550|^600|^550;";
                String sHeader2 = "Bin|Business Name/ Address|Owner's Name/ Address|Code\nStatus|Gross/\nCapital|Business Tax|Fees|Surch & Interest|Total|Or Number|Or Date|Term|Year|Qtr";
                String sHeaderColumn = "";
                String sHeader3 = "";
                if (m_sReportName.Contains("TOP"))
                {
                    sHeader = "^700|" + sHeader;
                    sHeader2 = "Rank|" + sHeader2;
                }

                sHeaderColumn = sHeader + sHeader2;

                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontName = "Arial Narrow";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.FontSize = (float)10.0;
                this.axVSPrinter1.Table = "^10600;Republic of the Philippines";
                if (AppSettingsManager.GetConfigObject("01") == "CITY")
                    sHeader3 = "^10600;Office of the City Treasurer";
                else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                    sHeader3 = "^10600;Office of the Municipal Treasurer";
                this.axVSPrinter1.Table = sHeader3;
                this.axVSPrinter1.FontName = "Arial";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = "^10600;" + m_sReportName;
                this.axVSPrinter1.FontBold = false;
                this.axVSPrinter1.FontSize = 8;
                this.axVSPrinter1.Table = ("^10600;As of " + AppSettingsManager.GetSystemDate().ToString("MMMM dd, yyyy"));
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.Table = sHeaderColumn;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = false;
            }
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            if (!m_bInit)
                CreateHeader();

            m_bInit = false;
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
    }
}