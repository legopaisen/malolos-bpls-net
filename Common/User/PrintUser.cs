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

namespace Amellar.Common.User
{
    public class PrintUser
    {
        private VSPrinterEmuModel model;
        private VSPrinterEmuModel pageHeaderModel;
        private string m_strSource = string.Empty;
        private string m_strReportName = string.Empty;
        private string m_strQuery = string.Empty;
        
        public string Source
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public string ReportName
        {
            get { return m_strReportName; }
            set { m_strReportName = value; }
        }

        public string ReportQuery
        {
            get { return m_strQuery; }
            set { m_strQuery = value; }
        }

        public PrintUser()
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

            if (m_strSource == "1")
                PrintList();
            else
                PrintRights();

            pageHeaderModel.PageCount = model.PageCount;
            PrintReportPreview();
        }

        private void PrintList()
        {
            OracleResultSet result = new OracleResultSet();
            string strData = string.Empty;
            string strUserLN = string.Empty;
            string strUserFN = string.Empty;
            string strUserMI = string.Empty;
            string strUserName = string.Empty;

            //CreateHeader();
            //CreatePageHeader();
            
            result.Query = "select * from sys_users order by usr_code";
            if (result.Execute())
            {
                while (result.Read())
                {
                    strUserName = "";
                    strData = result.GetString("usr_code");
                    strUserLN = result.GetString("usr_ln").Trim();
                    strUserFN = result.GetString("usr_fn").Trim();
                    strUserMI = result.GetString("usr_mi").Trim();

                    strUserName = StringUtilities.PersonName.ToPersonName(strUserLN, strUserFN, strUserMI, "L", "F L", "F M. L");

                    strData += "|" + strUserName;
                    strData += "|" + result.GetString("usr_pos");
                    strData += "|" + result.GetString("usr_div");

                    model.SetTable(string.Format("<1500|<5000|<3000|<2000;{0}", strData));
                    
                }
            }
            result.Close();

            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));

        }

        private void PrintRights()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet recUser = new OracleResultSet();

            CreatePageHeader();

            string strContent = string.Empty;
            string strUsrCode = string.Empty;
            string strTmpUsr = string.Empty;

            result.Query = m_strQuery;
            if(result.Execute())
            {
                while (result.Read())
                {
                    strUsrCode = result.GetString("usr_code");

                    string strUserLN = string.Empty;
                    string strUserFN = string.Empty;
                    string strUserMI = string.Empty;
                    string strUserName = string.Empty;
                    string strUsrPos = string.Empty;
                    string strUsrDiv = string.Empty;

                    recUser.Query = "select * from sys_users";
                    recUser.Query += " where usr_code = '" + strUsrCode + "'";
                    if (recUser.Execute())
                    {
                        if (recUser.Read())
                        {
                            strUsrPos = recUser.GetString("usr_pos").Trim();
                            strUsrDiv = recUser.GetString("usr_div").Trim();
                            strUserLN = recUser.GetString("usr_ln").Trim();
                            strUserFN = recUser.GetString("usr_fn").Trim();
                            strUserMI = recUser.GetString("usr_mi").Trim();

                            strUserName = StringUtilities.PersonName.ToPersonName(strUserLN, strUserFN, strUserMI, "L", "F L", "F M. L");
                        }
                    }
                    recUser.Close();

                    if (strUsrCode != strTmpUsr)
                    {
                        model.SetParagraph("");
                        model.SetFontBold(1);
                        strContent  = "User :|" + strUsrCode + "|";
                        strContent += " |";
                        strContent += "User Name :|";
                        strContent += strUserName;
                        model.SetTable(string.Format("<1500|<1500|<1000|<2000|<5500;{0}", strContent));
                        strContent  = "||";
                        strContent += " |";
                        strContent += "Position/Division :|";
                        strContent += strUsrPos + "/" + strUsrDiv;
                        model.SetTable(string.Format("<1500|<1500|<1000|<2000|<5500;{0}", strContent));
                        model.SetFontBold(0);
                        model.SetParagraph("");
                        model.SetTable("<11500;RIGHTS");
                        model.SetParagraph("");
                        strContent = "MODULE CODE|MODULE DESCRIPTION";
                        model.SetFontUnderline(1);
                        model.SetTable(string.Format("<2000|<9500;{0}", strContent));
                        model.SetFontUnderline(0);
                        model.SetParagraph("");
                        strTmpUsr = strUsrCode;

                    }

                    strContent  = result.GetString("usr_rights");
                    strContent += "|";
                    strContent += result.GetString("right_desc");
                    model.SetTable(string.Format("<2000|<9500;{0}", strContent));
                }
            }
            result.Close();

            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));
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

            //model.SetParagraph(ConfigurationAttributes.OfficeName);   // RMC 20110809 deleted office name in User's report list
            model.SetParagraph(string.Empty);

            model.SetParagraph(m_strReportName);
            model.SetParagraph(string.Empty);

            model.SetFontBold(0);
            model.SetTextAlign(0);
            model.SetTable(string.Empty);
            model.SetFontSize(8);

            //lngYY2 = model.GetCurrentY();

            string strData = string.Empty;

            if (m_strSource == "1")
            {
                model.SetFontUnderline(1);
                strData = "USER CODE|USER NAME|POSITION|DIVISION";
                model.SetTable(string.Format("^1500|^5000|^3000|^2000;{0}", strData));
                model.SetTable(string.Empty);
                model.SetFontUnderline(0);
            }

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

            if (m_strSource == "1")
            {
                pageHeaderModel.SetFontUnderline(1);
                strData = "USER CODE|USER NAME|POSITION|DIVISION";
                pageHeaderModel.SetTable(string.Format("^1500|^5000|^3000|^2000;{0}", strData));
                pageHeaderModel.SetTable(string.Empty);
                pageHeaderModel.SetFontUnderline(0);
            }
            

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
