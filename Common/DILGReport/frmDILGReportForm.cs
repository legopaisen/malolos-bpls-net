using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.DynamicProgressBar;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Amellar.Modules.DILGReport
{
    public partial class frmDILGReportForm : Form
    {
        public string m_sBnsStat;
        public DateTime m_dtDateFr = AppSettingsManager.GetCurrentDate();
        public DateTime m_dtDateTo = AppSettingsManager.GetCurrentDate();
        private bool m_bInit = true;
        TimeSpan tsTimeConsumed;

        //JARS 20180316 (S) IMPORT FROM OTHER BPLS
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
        //JARS 20180316 (E)

        public frmDILGReportForm()
        {
            InitializeComponent();
        }

        private void DoSomethingDifferent(object intCountIncreament, object p, object p_3)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void CreateHeader() //JARS 20180508
        {
            string strProvinceName = string.Empty;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Paragraph = "Republic of the Philippines";

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty && !strProvinceName.Contains("PROVINCE"))
                strProvinceName = "Province of " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strProvinceName.ToLower());
            if (strProvinceName != string.Empty)
                this.axVSPrinter1.Paragraph = strProvinceName;
            this.axVSPrinter1.Paragraph = AppSettingsManager.GetConfigValue("09");

            this.axVSPrinter1.Paragraph = AppSettingsManager.GetConfigValue("41");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "BUSINESS PERMIT TRANSACTION LOG";
            string strData = "";

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = (float)8.0;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            //strData = "|Business Name|Address|Line/s of Business|Status|Application|Billing/SOA|Payment|Issue|Release|Time Consumed|Released By|Received By";
            //strData = "BIN / Business Name|Address|Line/s of Business|Status|Application|Payment|Generate Permit|Release Permit|Time|Permit";
            //this.axVSPrinter1.Table = string.Format("^2500|^1500|^1500|^1000|^3000|^2250|^2250|^2250|^1000|^2000;{0}", strData);
            //strData = "||||No.|Date|In|Out|Date|In|Out|Date|In|Out|Date|In|Out|Consumed|Released By|Received By";
           // this.axVSPrinter1.Table = string.Format("^2500|^1500|^1500|^1000|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^1000|^1000|^1000;{0}", strData);

            //strData = "BIN / Business Name|Address|Line/s of Business|Status|Application|Billing/SOA|Payment|Generate Permit|Release Permit|Time|Permit";  //JHB 20180521
            //this.axVSPrinter1.Table = string.Format("^2500|^1500|^1500|^800|^3000|^2250|^2250|^2250|^2250|^1000|^2000;{0}", strData);
           // strData = "||||No.|Date|In|Out|Date|In|Out|Date|In|Out|Date|In|Out|Date|In|Out|Consumed|Released By|Received By";
           // this.axVSPrinter1.Table = string.Format("^2500|^1500|^1500|^800|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^1000|^1000|^1000;{0}", strData);
            

            strData = "BIN / Business Name|Line/s of Business|Status|Application|Billing/SOA|Payment|Generate Permit|Release Permit|Time|Permit"; //JHB 20190108 modify display 
            this.axVSPrinter1.Table = string.Format("^2300|^1500|^650|^3000|^2250|^2250|^2250|^2250|^1000|^2000;{0}", strData);  //JHB 20190108 modify display 
            strData = "Address|||No.|Date|In|Out|Date|In|Out|Date|In|Out|Date|In|Out|Date|In|Out|Consumed|Released By|Received By";  //JHB 20190108 modify display 
            this.axVSPrinter1.Table = string.Format("^2300|^1500|^650|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^1000|^1000|^1000;{0}", strData);  //JHB 20190108 modify display 

            this.axVSPrinter1.Paragraph = "";
        }

        private void PrintForm()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            string sQuery, strData, sFormat, sBIN, sTaxYear;
            string sDtIn, sDtOut;
            string sDtFr = string.Format("{0:MM/dd/yyyy}", m_dtDateFr);
            string sDtTo = string.Format("{0:MM/dd/yyyy}", m_dtDateTo);
            string sPrevBin = string.Empty;

            //JARS 20180509
            #region Progress
            m_objThread = new Thread(this.ThreadProcess);
            m_objThread.Start();

            Thread.Sleep(500);

            int intCount = 0;
            int intCountIncreament = 0;

            //AFM 20190712 applied proper query for progress bar (s)
            //sQuery = "select count(*) from trans_log ";
            sQuery = "select count(*) from (select distinct bin,tax_year,app_stat from trans_log ";
            sQuery += " where trunc(trans_in) between to_date('" + sDtFr + "','MM/dd/yyyy') and ";
            sQuery += " to_date('" + sDtTo + "','MM/dd/yyyy')";
            if (m_sBnsStat != "ALL")
            {
                sQuery += " and app_stat = '" + m_sBnsStat + "' ";
                sQuery += " and (bin in (select bin from businesses where bns_stat = '" + m_sBnsStat + "') or bin in (select bin from business_que where bns_stat = '" + m_sBnsStat + "'))) ";
            }
            else
                sQuery += ")";
            //sQuery += " and trans_code like '%APP' order by trans_in, bin";
            //AFM 20190712 applied proper query for progress bar (e)


            //select count(*) from (select distinct bin,tax_year,app_stat from trans_log  where trunc(trans_in) between to_date('04/01/2019','MM/dd/yyyy') and  
            //to_date('05/01/2019','MM/dd/yyyy') and app_stat = 'NEW' and (bin in (select bin from businesses where bns_stat = 'NEW') or bin in (select bin from business_que where bns_stat = 'NEW'))  order by bin);

            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out intCount);

            DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
            #endregion
            m_sBnsStat = m_sBnsStat.Substring(0, 3); //JHB 20190103 update stat for ren and ret
            
            //sFormat = "<500|<2000|<1500|<1500|<1000|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|<1000|<1000|<1000;";
            //sFormat = "<500|<2000|<1500|<1500|<800|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|<1000|<1000|<1000;";

            sFormat = "<400|<1900|<1500|<650|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|<1000|<1000|<1000;";

            //if (m_sBnsStat == "RENEWAL")
            //{
            //    m_sBnsStat = "REN";
            //}
            //if (m_sBnsStat == "RETIRED")
            //{
            //    m_sBnsStat = "RET";
            //}

            //pRec.Query = "select bin, trans_in, trans_out, tax_year, app_stat from trans_log ";
            //pRec.Query += " where trunc(trans_in) between to_date('" + sDtFr + "','MM/dd/yyyy') and ";
            //pRec.Query += " to_date('" + sDtTo + "','MM/dd/yyyy')";
            //if (m_sBnsStat != "ALL")
            //   pRec.Query += " and app_stat = '" + m_sBnsStat + "' ";
            //pRec.Query += " and trans_code like '%APP' order by trans_in, bin";
            //pRec.Query += "";

            //JARS 20180509 (S) FIX FOR MISSING DATA, NO TRANS_CODE FOR APPLICATION BUT HAS TRANS_CODE FOR SOA AND PAYMENT
            //JHB 2019110 revise Display info for trans app
            pRec.Query = "select distinct bin,tax_year,app_stat from trans_log ";
            pRec.Query += " where trunc(trans_in) between to_date('" + sDtFr + "','MM/dd/yyyy') and ";
            pRec.Query += " to_date('" + sDtTo + "','MM/dd/yyyy')";
            if (m_sBnsStat != "ALL")
            {
                pRec.Query += " and app_stat = '" + m_sBnsStat + "' and";
                pRec.Query += " (bin in (select bin from businesses where bns_stat = '" + m_sBnsStat + "') or bin in (select bin from business_que where bns_stat = '" + m_sBnsStat + "')) ";
            }
            else
            {
                 pRec.Query += " ";
            }
            pRec.Query += " order by bin";
            //JARS 20180509 (E)

            if (pRec.Execute())
            {
                int iRecCnt = 0;
                string sRecCnt = string.Empty;
                while (pRec.Read())
                {
                    sBIN = pRec.GetString("bin");
                    if(sBIN != sPrevBin)
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        iRecCnt++;
                        tsTimeConsumed = new TimeSpan();

                        //sDtIn = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime(1)); 
                        //sDtOut = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime(2));
                        sTaxYear = pRec.GetString("tax_year");
                        m_sBnsStat = pRec.GetString("app_stat");

                        sRecCnt = string.Format("{0:###}", iRecCnt);

                        strData = sRecCnt + "|" + sBIN + " / " + AppSettingsManager.GetBnsName(sBIN) + " / " + AppSettingsManager.GetBnsAddress(sBIN) + "|";
                        //strData+= AppSettingsManager.GetBnsAddress(sBIN) + "|";
                        strData += GetBnsLine(sBIN, sTaxYear) + "|";
                        strData += m_sBnsStat;

                        //strData += "|" + GetAppNo(sBIN, sTaxYear, m_sBnsStat, sDtIn, sDtOut);
                        ////application
                        //strData += GetData(m_sBnsStat, sDtIn, sDtOut, sBIN, "APP");
                        ////soa
                        ////strData += GetData(m_sBnsStat, sDtIn, sDtOut, sBIN, "SOA");  
                        ////payment
                        //strData += GetData(m_sBnsStat, sDtIn, sDtOut, sBIN, "CAP");
                        ////issue permit
                        //strData += GetData(m_sBnsStat, sDtIn, sDtOut, sBIN, "ABBP");
                        ////release

                        //JARS 20180509
                        strData += "|" + GetAppNo(sBIN, sTaxYear, m_sBnsStat, sDtFr, sDtTo);

                        //application
                        strData += GetData(m_sBnsStat, sDtFr, sDtTo, sBIN, "APP");
                        //soa
                        strData += GetData(m_sBnsStat, sDtFr, sDtTo, sBIN, "SOA");  //JHB 20180521 as per BPLD request
                        //payment
                        strData += GetData(m_sBnsStat, sDtFr, sDtTo, sBIN, "CAP");
                        //issue permit
                        strData += GetData(m_sBnsStat, sDtFr, sDtTo, sBIN, "ABBP");
                        //release


                        string sDate = string.Empty, sTimeIn = string.Empty, sTimeOut = string.Empty, sReleasedBy = string.Empty,
                            sReceivedBy = string.Empty, sTimeConsumed = string.Empty;

                        pSet.Query = "select * from permit_monitoring where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "'";
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                sDate = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("recd_dt"));
                                sTimeIn = pSet.GetString("recd_tm");
                                sTimeOut = sTimeIn;
                                sReleasedBy = pSet.GetString("released_by");
                                sReceivedBy = pSet.GetString("recd_by");

                            }
                        }
                        pSet.Close();

                        sTimeConsumed = string.Format("{0:hh:mm:ss}", tsTimeConsumed);

                        strData += "|" + sDate + "|" + sTimeIn + "|" + sTimeOut + "|" + sTimeConsumed + "|" + sReleasedBy + "|" + sReceivedBy;

                        this.axVSPrinter1.Table = sFormat + strData;
                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);

                        sPrevBin = pRec.GetString("bin");
                    }
                }
                //JARS 20180508 (S)
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontSize = 10;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                this.axVSPrinter1.Paragraph = "Total Number of Businesses: " + iRecCnt;
                this.axVSPrinter1.FontBold = false;
                //JARS 20180508 (E)
            }
            pRec.Close();
            DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
            Thread.Sleep(10);
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            //JARS 20180508
            if (!m_bInit)
                CreateHeader();

            m_bInit = false;
            #region comments
            //string strProvinceName = string.Empty;
            //this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            //this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.FontSize = (float)10.0;
            //this.axVSPrinter1.Paragraph = "Republic of the Philippines";

            //strProvinceName = AppSettingsManager.GetConfigValue("08");
            //if (strProvinceName != string.Empty && !strProvinceName.Contains("PROVINCE"))
            //    strProvinceName = "Province of " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strProvinceName.ToLower());
            //if (strProvinceName != string.Empty)
            //    this.axVSPrinter1.Paragraph = strProvinceName;
            //this.axVSPrinter1.Paragraph = AppSettingsManager.GetConfigValue("09");

            //this.axVSPrinter1.Paragraph = AppSettingsManager.GetConfigValue("41");
            //this.axVSPrinter1.Paragraph = "";

            //this.axVSPrinter1.Paragraph = "BUSINESS PERMIT TRANSACTION LOG";
            //string strData = "";

            //this.axVSPrinter1.FontBold = false;
            //this.axVSPrinter1.FontSize = (float)8.0;
            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            ////strData = "|Business Name|Address|Line/s of Business|Status|Application|Billing/SOA|Payment|Issue|Release|Time Consumed|Released By|Received By";
            //strData = "BIN / Business Name|Address|Line/s of Business|Status|Application|Payment|Generate Permit|Release Permit|Time|Permit";
            //this.axVSPrinter1.Table = string.Format("^2500|^1500|^1500|^1000|^3000|^2250|^2250|^2250|^1000|^2000;{0}", strData);
            //strData = "||||No.|Date|In|Out|Date|In|Out|Date|In|Out|Date|In|Out|Consumed|Released By|Received By";
            //this.axVSPrinter1.Table = string.Format("^2500|^1500|^1500|^1000|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^750|^1000|^1000|^1000;{0}", strData);
            //this.axVSPrinter1.Paragraph = "";
            #endregion
        }

        private string GetData(string sBnsStat, string sDtIn, string sDtOut, string sBIN, string sTransCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sData = string.Empty;
            string sDate = string.Empty;
            string sTimeIn = string.Empty;
            string sTimeOut = string.Empty;
            string sTimeHr = "0";
            string sTimeMn = "0";
            string sTimeSc = "0";

            pSet.Query = "select * from trans_log where app_stat = '" + sBnsStat + "' and trunc(trans_in) between to_date('" + sDtIn + "','MM/dd/yyyy') ";
            pSet.Query += " and to_date('" + sDtOut + "','MM/dd/yyyy') ";
            pSet.Query += " and bin = '" + sBIN + "' and trans_code like '%" + sTransCode + "'";

            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sDate = pSet.GetDateTime("trans_in").ToShortDateString();
                    sTimeIn = pSet.GetDateTime("trans_in").ToLongTimeString();
                    sTimeOut = pSet.GetDateTime("trans_out").ToLongTimeString();

                    tsTimeConsumed += DateTime.Parse(sTimeIn).Subtract(DateTime.Parse(sTimeOut)).Duration();
                }
                sData += "|" + sDate + "|" + sTimeIn + "|" + sTimeOut;
                
            }
            pSet.Close();

            return sData;
        }

        private void frmDILGReportForm_Load(object sender, EventArgs e)
        {
            this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.MarginTop = 500;
            this.axVSPrinter1.MarginBottom = 500;
            ExportFile();
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            CreateHeader();
            this.PrintForm();

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private string GetAppNo(string sBIN, string sTaxYear,string sBnsStat, string sDateFr, string sDateTo)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sAppNo = string.Empty;

            if (sBnsStat == "REN")
            {
                pSet.Query = "select * from app_permit_no where to_date(issued_date,'MM/dd/yyyy') between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy')";
                pSet.Query += " and bin = '" + sBIN + "' and year = '" + sTaxYear + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                        sAppNo = pSet.GetString("app_no");
                }
                pSet.Close();
            }
            if (sBnsStat == "NEW")
            {
                pSet.Query = "select * from app_permit_no_new where to_date(issued_date,'MM/dd/yyyy') between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy')";
                pSet.Query += " and bin = '" + sBIN + "' and year = '" + sTaxYear + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                        sAppNo = pSet.GetString("app_no");
                }
                pSet.Close();
            }

            /*if (sBnsStat == "RET")
            {
                pSet.Query = "select * from app_permit_no_ret where to_date(issued_date,'MM/dd/yyyy') between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy')";
                pSet.Query += " and bin = '" + sBIN + "' and year = '" + sTaxYear + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                        sAppNo = pSet.GetString("app_no");
                }
                pSet.Close();
            }*/

            return sAppNo;
        }

        private string GetBnsLine(string sBIN, string sTaxYear)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBnsLine = string.Empty;
            string sBnsCode = string.Empty;

            pRec.Query = "select * from businesses where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sBnsCode = pRec.GetString("bns_code");
                    sBnsLine = AppSettingsManager.GetBnsDesc(sBnsCode);
                }
                else
                {
                    pRec.Close();
                    pRec.Query = "select * from business_que where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "'";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sBnsCode = pRec.GetString("bns_code");
                            sBnsLine = AppSettingsManager.GetBnsDesc(sBnsCode);
                        }
                    }
                }
            }
            pRec.Close();

            pRec.Query = "select * from addl_bns where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "'";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBnsCode = pRec.GetString("bns_code_main");
                    sBnsLine += "/" + AppSettingsManager.GetBnsDesc(sBnsLine);
                }
            }
            pRec.Close();

            return sBnsLine;
        }

        private void ExportFile()
        {
            string strCurrentDirectory = Directory.GetCurrentDirectory();
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.DefaultExt = "html";
                dlg.Title = "Reports";
                dlg.Filter = "HTML Files (*.html;*.htm)|*.html|Excel Files (*.xls)|*.xls";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.axVSPrinter1.ExportFormat = VSPrinter7Lib.ExportFormatSettings.vpxPlainHTML;
                    this.axVSPrinter1.ExportFile = dlg.FileName;
                }
            }
        }

        private void axVSPrinter1_StartDocEvent(object sender, EventArgs e)
        {

        }
    }
}