using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.PrintUtilities;
using Amellar.Common.AppSettings;

namespace Amellar.Common.ModuleRights
{
    public class PrintModule
    {
        private VSPrinterEmuModel model;
        private VSPrinterEmuModel pageHeaderModel;
        private string m_strReportName = string.Empty;

        public string ReportName
        {
            get { return m_strReportName; }
            set { m_strReportName = value; }
        }

        public PrintModule()
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
            CreateHeader();
            CreatePageHeader();

            PrintList();
            
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

            //model.SetParagraph(ConfigurationAttributes.OfficeName);   // RMC 20110809 deleted
            model.SetParagraph(string.Empty);

            model.SetParagraph(m_strReportName);
            model.SetParagraph(string.Empty);

            model.SetFontBold(0);
            model.SetTextAlign(0);
            model.SetTable(string.Empty);
            model.SetFontSize(8);

            //lngYY2 = model.GetCurrentY();

            string strData = string.Empty;

            model.SetFontUnderline(1);
            strData = "MODULE CODE|MODULE DESCRIPTION";
            model.SetTable(string.Format("<2000|<9500;{0}", strData));
            model.SetTable(string.Empty);
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

            //pageHeaderModel.SetParagraph(ConfigurationAttributes.OfficeName); // RMC 20110809 deleted
            pageHeaderModel.SetParagraph(string.Empty);

            pageHeaderModel.SetParagraph(m_strReportName);
            pageHeaderModel.SetParagraph(string.Empty);

            pageHeaderModel.SetFontBold(0);
            pageHeaderModel.SetTextAlign(0);
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetFontSize(8);


            string strData;

            pageHeaderModel.SetFontUnderline(1);
            strData = "MODULE CODE|MODULE DESCRIPTION";
            pageHeaderModel.SetTable(string.Format("<2000|<9500;{0}", strData));
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetFontUnderline(0);
        }

        private void PrintList()
        {
            OracleResultSet result = new OracleResultSet();
            string strData = string.Empty;
            
            result.Query = "select *  from sys_module order by usr_rights";
            if(result.Execute())
            {
                while (result.Read())
                {
                    strData = result.GetString("usr_rights") + "|";
                    strData+=  result.GetString("right_desc");
                    model.SetTable(string.Format("<2000|<9500;{0}", strData));
                }
            }
            result.Close();

            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

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

    }
}
