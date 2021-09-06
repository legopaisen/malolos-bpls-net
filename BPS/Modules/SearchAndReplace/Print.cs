using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using Amellar.Common.PrintUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.BPLS.SearchAndReplace
{
    public class Print
    {
        private VSPrinterEmuModel model;
        private VSPrinterEmuModel pageHeaderModel;
        OracleResultSet pSet = new OracleResultSet();
        private string m_strQuery = string.Empty;
        private bool m_bPrintBIN = false;
        private bool m_bPrintPrevOwn = false;
        private bool m_bPrintOwnPlace = false;  // RMC 20111013 added additional column in Owner's query report
        private long y = 0;
        
        public string Query
        {
            get { return m_strQuery; }
            set { m_strQuery = value; }
        }

        public bool PrintBIN
        {
            get { return m_bPrintBIN; }
            set { m_bPrintBIN = value; }
        }

        public bool PrintPrevOwnEntry
        {
            get { return m_bPrintPrevOwn; }
            set { m_bPrintPrevOwn = value; }
        }

        public bool PrintOwnPlaceEntry
        {
            get { return m_bPrintOwnPlace; }
            set { m_bPrintOwnPlace = value; }
        }

        public Print()
        {
            model = new VSPrinterEmuModel();
            model.LeftMargin = 50;
            model.TopMargin = 50;
            model.MaxY = 1100 - 50;

            pageHeaderModel = new VSPrinterEmuModel();
            pageHeaderModel.LeftMargin = 50;
            pageHeaderModel.TopMargin = 50;
            pageHeaderModel.MaxY = 1100 - 50;
        }

        public void FormLoad()
        {
            PrintReport();
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

            model.SetParagraph(AppSettingsManager.GetConfigValue("41"));    // RMC 20110811
            model.SetParagraph(string.Empty);

            model.SetParagraph("OWNER'S QUERY");

            model.SetParagraph(string.Empty);

            model.SetFontBold(0);
            model.SetTextAlign(0);
            model.SetTable(string.Empty);
            model.SetFontSize(8);

            string strData = string.Empty;

            model.SetFontBold(1);
            // RMC 20111013 added additional column in Owner's query report (s)
            strData = "OWN CODE|OWNER'S NAME||NO. OF|NO. OF|NO. OF PREVIOUS|NO. OF OWNED";
            model.SetTable(string.Format("^1000|^5000|^200|^1000|^1000|^1500|^1500;{0}", strData));
            strData = "|||BUSINESS|LESSEE|OWNER ENTRY|PLACE ENTRY";
            model.SetTable(string.Format("^1000|^5000|^200|^1000|^1000|^1500|^1500;{0}", strData));
            // RMC 20111013 added additional column in Owner's query report (e)
            model.SetTable(string.Empty);
            model.SetFontBold(0);

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

            //pageHeaderModel.SetParagraph(ConfigurationAttributes.OfficeName);
            pageHeaderModel.SetParagraph(AppSettingsManager.GetConfigValue("41"));    // RMC 20110811
            pageHeaderModel.SetParagraph(string.Empty);

            pageHeaderModel.SetParagraph("OWNER'S QUERY");

            pageHeaderModel.SetParagraph(string.Empty);

            pageHeaderModel.SetFontBold(0);
            pageHeaderModel.SetTextAlign(0);
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetFontSize(8);

            string strData;

            pageHeaderModel.SetFontBold(1);
            // RMC 20111013 added additional column in Owner's query report (s)
            strData = "OWN CODE|OWNER'S NAME||NO. OF|NO. OF|NO. OF PREVIOUS|NO. OF OWNED";
            pageHeaderModel.SetTable(string.Format("^1000|^5000|^200|^1000|^1000|^1500|^1500;{0}", strData));
            strData = "|||BUSINESS|LESSEE|OWNER ENTRY|PLACE ENTRY";
            pageHeaderModel.SetTable(string.Format("^1000|^5000|^200|^1000|^1000|^1500|^1500;{0}", strData));
            // RMC 20111013 added additional column in Owner's query report (e)
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetFontBold(0);

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



        private void PrintReport()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sDataRow = "";
            string sOwnCode = "";
            string sLastName = "";
            string sFirstName = "";
            string sMI = "";
            string sOwnAddress = "";
            string sOwnName = "";
            string sCount = "";

            CreateHeader();
            CreatePageHeader();

            pSet.Query = m_strQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sOwnCode = pSet.GetString("own_code").Trim();
			        sLastName = StringUtilities.HandleApostrophe(pSet.GetString("own_ln").Trim());
			        sFirstName = StringUtilities.HandleApostrophe(pSet.GetString("own_fn").Trim());
                    sMI = pSet.GetString("own_mi").Trim();

                    if (sMI == "" || sMI == ".")
                        sMI = "";
                    else
                        sMI = sMI + ".";
                    if (sFirstName == "" || sFirstName == ".")
                        sFirstName = "";
                    else
                        sFirstName = sFirstName + " ";
                    if (sLastName == "" || sLastName == ".")
                        sLastName = "";
                    else
                        sLastName = sLastName + ", ";

                    //pApp->m_bOwnerQuery = true;
                    AppSettingsManager.m_bOwnerQuery = true;
                    sOwnAddress = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                    sOwnName = sLastName + sFirstName + sMI;

                    int iCnt = 0;
                    pRec.Query = string.Format("select count(*) from businesses where own_code = '{0}'", sOwnCode);
                    int.TryParse(pRec.ExecuteScalar(), out iCnt);
                    sCount = string.Format("{0:##}", iCnt);
                    if (sCount == "")
                        sCount = "0";

                    sDataRow = sOwnCode + "|";
                    sDataRow += sOwnName + "||";
                    sDataRow += sCount + "|";

                    pRec.Query = string.Format("select count(*) from businesses where busn_own = '{0}' and place_occupancy = 'RENTED'", sOwnCode);
                    int.TryParse(pRec.ExecuteScalar(), out iCnt);
                    sCount = string.Format("{0:##}", iCnt);

                    if (sCount == "")
                        sCount = "0";
                    sDataRow += sCount + "|";

                    pRec.Query = string.Format("select count(*) from businesses where prev_bns_own = '{0}'", sOwnCode);
                    int.TryParse(pRec.ExecuteScalar(), out iCnt);
                    sCount = string.Format("{0:##}", iCnt);

                    if (sCount == "")
                        sCount = "0";
                    sDataRow += sCount;

                    // RMC 20111013 added additional column in Owner's query report (s)
                    pRec.Query = string.Format("select count(*) from businesses where busn_own = '{0}' and own_code <> busn_own and place_occupancy = 'OWNED'", sOwnCode);
                    int.TryParse(pRec.ExecuteScalar(), out iCnt);
                    sCount = string.Format("{0:##}", iCnt);
                    if (sCount == "")
                        sCount = "0";
                    sDataRow += "|" + sCount;
                    // RMC 20111013 added additional column in Owner's query report (e)

                    model.SetTable(string.Format("<1000|=5000|^200|^1000|^1000|^1500|^1500;{0}", sDataRow));
                    model.SetTable(string.Format("<1000|=5000;{0}", "|  " + sOwnAddress));

                    y = 0;
                    if (m_bPrintBIN)
                    {
                        PrintLesseeBIN(sOwnCode);
                    }

                    if (m_bPrintPrevOwn)
                    {
                        PrintPrevOwn(sOwnCode);
                    }

                    if (m_bPrintOwnPlace)
                    {
                        PrintOwnPlace(sOwnCode);
                    }
                }
            }
            pSet.Close();

            AppSettingsManager.m_bOwnerQuery = true;

            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));
        }

        private void PrintLesseeBIN(string sOwnCode)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBIN = "";
            int iLesseeCnt = 0;

            pRec.Query = string.Format("select count(*) from businesses where busn_own = '{0}' and place_occupancy = 'RENTED'", sOwnCode);
            int.TryParse(pRec.ExecuteScalar(), out iLesseeCnt);

            if(iLesseeCnt > 0)
                y = model.GetCurrentY();
            
            pRec.Query = string.Format("select * from businesses where busn_own = '{0}' and place_occupancy = 'RENTED' order by bin", sOwnCode);
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBIN = pRec.GetString("bin");

                    model.SetTable(string.Format("<1000|=5000|^200|^1000|<2000;{0}", "||||" + sBIN));
                }
                model.SetParagraph("");
            }
            pRec.Close();

        }

        private void PrintPrevOwn(string sOwnCode)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBIN = "";
            int iLesseeCnt = 0;

            pRec.Query = string.Format("select count(*) from businesses where prev_bns_own = '{0}' and own_code <> prev_bns_own", sOwnCode);
            int.TryParse(pRec.ExecuteScalar(), out iLesseeCnt);

          //  if(iLesseeCnt > 0 && y > 0)
          //      model.SetCurrentY(y);

            pRec.Query = string.Format("select * from businesses where prev_bns_own = '{0}' and own_code <> prev_bns_own order by bin", sOwnCode);
            if(pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBIN = pRec.GetString("bin");

                    model.SetTable(string.Format("<1000|=5000|^200|^1000|^1000|<2000;{0}", "|||||" + sBIN));
                }
                model.SetParagraph("");
            }
            pRec.Close();
                    


        }

        private void PrintOwnPlace(string sOwnCode)
        {
            // RMC 20111013 added additional column in Owner's query report

            OracleResultSet pRec = new OracleResultSet();
            string sBIN = "";
            int iLesseeCnt = 0;

            pRec.Query = string.Format("select count(*) from businesses where busn_own = '{0}' and own_code <> busn_own and place_occupancy = 'OWNED'", sOwnCode);
            int.TryParse(pRec.ExecuteScalar(), out iLesseeCnt);

            //  if(iLesseeCnt > 0 && y > 0)
            //      model.SetCurrentY(y);

            pRec.Query = string.Format("select * from businesses where busn_own = '{0}' and own_code <> busn_own and place_occupancy = 'OWNED' order by bin", sOwnCode);
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBIN = pRec.GetString("bin");

                    model.SetTable(string.Format("<1000|=5000|^200|^1000|^1000|^1000|<2000;{0}", "||||||" + sBIN));
                }
                model.SetParagraph("");
            }
            pRec.Close();
        }
            

    }
}
