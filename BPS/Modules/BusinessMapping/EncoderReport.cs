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

namespace Amellar.Modules.BusinessMapping
{
    public class EncoderReport
    {
        private VSPrinterEmuModel model;
        private VSPrinterEmuModel pageHeaderModel;
        private string m_sQuery = string.Empty;
        private string m_sFrom = string.Empty;
        private string m_sTo = string.Empty;
        private bool m_bOfficial = true;
        private string m_sReport = string.Empty;

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

        public bool Official
        {
            get { return m_bOfficial; }
            set { m_bOfficial = value; }
        }

        public string ReportName
        {
            get { return m_sReport; }
            set { m_sReport = value; }
        }

        public EncoderReport()
        {
            model = new VSPrinterEmuModel();
            model.LeftMargin = 20;
            model.TopMargin = 50;
            model.MaxY = 1100 - 200;

            pageHeaderModel = new VSPrinterEmuModel();
            pageHeaderModel.LeftMargin = 20;
            pageHeaderModel.TopMargin = 50;
            pageHeaderModel.MaxY = 1100 - 200;
        }

        public void FormLoad()
        {
            CreateHeader();
            CreatePageHeader();

            Print();

            pageHeaderModel.PageCount = model.PageCount;
            PrintReportPreview();
        }

        private void Print()
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

            //CreateHeader();
            //CreatePageHeader();

            //select bns_brgy, bns_user, bin, bns_nm 
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

                    sContent = sDate + "|" + sBrgy + "|" + sEncoder + "|" + sBIN + "|" + sBnsNm;
                    model.SetTable(string.Format("<1000|<1000|~3500|<1500|~4500;{0}", sContent));
                }

                sRecCnt = string.Format("{0:###}", iRecCnt);

                model.SetParagraph("");
                sContent = "Total encoded records: " + sRecCnt;
                model.SetTable(string.Format("<1100;{0}", sContent));
            }
            pRec.Close();

            model.SetFontSize(8);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));
        }

        private void PrintReportPreview()
        {
            model.MaxY = 1100;
            VSPrinterEmuDocument doc = new VSPrinterEmuDocument();
            doc.Model = model;

            doc.PageHeaderModel = pageHeaderModel;
            doc.Model.Reset();

            doc.DefaultPageSettings.Margins.Top = 50;
            doc.DefaultPageSettings.Margins.Left = 50;
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

            model.SetParagraph(AppSettingsManager.GetConfigValue("41"));    
            model.SetParagraph("");

            model.SetParagraph("BUSINESS MAPPING ENCODER'S REPORT");
            model.SetParagraph(m_sReport);
            model.SetParagraph("");
            string strData = "";
            strData = "Period covered " + m_sFrom + " to " + m_sTo;
            model.SetParagraph(strData);
            
            model.SetFontBold(0);
            model.SetTextAlign(0);
            model.SetParagraph("");
            model.SetFontSize(8);

            model.SetFontUnderline(1);
            if (m_bOfficial)
            {
                strData = "DATE|BARANGAY|ENCODER|BIN|BUSINESS NAME";
            }
            else
            {
                strData = "DATE|BARANGAY|ENCODER|TEMPORARY BIN|BUSINESS NAME";
            }

            model.SetTable(string.Format("^1000|^1000|^3500|^1500|^4500;{0}", strData));
            
            
            model.SetParagraph("");
            model.SetFontUnderline(0);


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

            pageHeaderModel.SetParagraph(AppSettingsManager.GetConfigValue("41"));    
            pageHeaderModel.SetParagraph("");

            pageHeaderModel.SetParagraph("BUSINESS MAPPING ENCODER'S REPORT");
            pageHeaderModel.SetParagraph(m_sReport);
            pageHeaderModel.SetParagraph("");
            string strData = "";
            strData = "Period covered " + m_sFrom + " to " + m_sTo;
            pageHeaderModel.SetTable(string.Format("^10500;{0}", strData));

            pageHeaderModel.SetFontBold(0);
            pageHeaderModel.SetTextAlign(0);
            pageHeaderModel.SetParagraph("");
            pageHeaderModel.SetFontSize(8);

            pageHeaderModel.SetFontUnderline(1);
            if (m_bOfficial)
            {
                strData = "DATE|BARANGAY|ENCODER|BIN|BUSINESS NAME";
            }
            else
            {
                strData = "DATE|BARANGAY|ENCODER|TEMPORARY BIN|BUSINESS NAME";
            }
            pageHeaderModel.SetTable(string.Format("^1000|^1000|^3500|^1500|^4500;{0}", strData));
            pageHeaderModel.SetParagraph("");
            pageHeaderModel.SetFontUnderline(0);

        }
             
    }
}
