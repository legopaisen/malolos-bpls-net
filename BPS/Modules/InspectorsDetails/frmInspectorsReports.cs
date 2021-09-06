
// RMC 20171116 added violation report

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using System.Globalization;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmInspectorsReports : Form
    {
        private string m_sQuery = string.Empty;

        public string Query
        {
            get { return m_sQuery; }
            set { m_sQuery = value; }
        }

        public frmInspectorsReports()
        {
            InitializeComponent();
        }

        private void frmInspectorsReports_Load(object sender, EventArgs e)
        {
            this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;

            this.axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.MarginRight = 500;
            this.axVSPrinter1.MarginTop = 700;
            this.axVSPrinter1.MarginBottom = 300;

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            PrintViolationReport();

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void PrintViolationReport()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec1 = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            string sDataRow = "";
            string sBin = "";
            string sDate = "";
            string sViolationCode = "";
            string sUntagBy = "";
            string sBnsName = "";
            string sBnsAddr = "";
            string sViolation = "";


            pSet.Query = m_sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    //sInspectorCode = pSet.GetString("inspector_code").Trim();
                    sBin = pSet.GetString("bin").Trim();
                    sDate = pSet.GetString("trans_date");
                    sViolationCode = pSet.GetString("violation_code");
                    sUntagBy = pSet.GetString("untag_by");
                    sDate = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("trans_date"));

                    sBnsName = AppSettingsManager.GetBnsName(sBin);
                    sBnsAddr = AppSettingsManager.GetBnsAddress(sBin);

                    sDataRow = sBin + "|";
                    sDataRow += sBnsName + "|";

                    sViolation = Violations.GetViolationDesc(sViolationCode);
                    
                    if (sUntagBy.Trim() == "")
                        sDataRow += sViolation + "||";
                    else
                        sDataRow += "|" + sViolation + "|";

                    sDataRow += sUntagBy;

                    this.axVSPrinter1.Table = string.Format("<2000|<3000|<2000|<2000|<1500;{0}", sDataRow);
                    
                }
            }
            pSet.Close();

            
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            string strProvinceName = string.Empty;
            string strData = string.Empty;

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Table = "^11000;Republic of the Philippines";
            this.axVSPrinter1.FontBold = false;

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                this.axVSPrinter1.Table = string.Format("^11000;{0}", "Province of " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strProvinceName.ToLower()));
            this.axVSPrinter1.Table = string.Format("^11000;{0}", AppSettingsManager.GetConfigValue("09"));
            this.axVSPrinter1.Table = string.Format("^11000;{0}", AppSettingsManager.GetConfigValue("41"));

            if (ConfigurationAttributes.LGUCode == "216")
            {
                this.axVSPrinter1.Table = string.Format("^11000;{0}", "OFFICE OF THE " + AppSettingsManager.GetConfigValue("01") + " ADMINISTRATOR");
                this.axVSPrinter1.Table = string.Format("^11000;{0}", "BUSINESS PERMIT AND LICENSING DIVISION");
            }

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^11000;VIOLATION REPORT");

            this.axVSPrinter1.Paragraph = "";
            strData = "BIN|BUSINESS NAME|TAGGED VIOLATION|UNTAGGED VIOLATION|UNTAG BY";
            this.axVSPrinter1.Table = string.Format("^2000|^3000|^2000|^2000|^1500;{0}", strData);
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = "";
        }
    }
}