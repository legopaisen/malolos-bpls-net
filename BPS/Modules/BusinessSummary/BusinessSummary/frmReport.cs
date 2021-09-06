using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;

namespace BusinessSummary
{
    public partial class frmReport : Form
    {
        public frmReport()
        {
            InitializeComponent();
        }

        string m_strUser = "";
        string m_strRegYear = "";
        string sSize = "";
        bool m_bInit = true;

        public frmReport(ReportType ReportType, String RegYear)
        {
            InitializeComponent();

            m_strRegYear = RegYear;
            
            ExportFile();

            if (ReportType.By_Barangay == ReportType)
            {
                this.Text = "SUMMARY OF BUSINESSES BY BARANGAY";
                clsBusinessSummary.LoadByBarangay(this, RegYear);
            }
            else if (ReportType.By_District == ReportType)
            {
                this.Text = "SUMMARY OF BUSINESSES BY DISTRICT";
                clsBusinessSummary.LoadByDistrict(this, RegYear);
            }
            else if (ReportType.By_Line_Of_Business == ReportType)
            {
                this.Text = "SUMMARY OF BUSINESSES BY LINE OF BUSINESS";
                clsBusinessSummary.LoadByLineOfBusiness(this, RegYear);
            }
            else if (ReportType.By_Org_Kind == ReportType)
            {
                this.Text = "SUMMARY OF BUSINESSES BY ORGANIZATION KIND";
                clsBusinessSummary.LoadByOrgKind(this, RegYear);
            }
            else if (ReportType.By_Gross_Receipts == ReportType)
            {
                this.Text = "SUMMARY OF BUSINESSES BY GROSS RECEIPTS";
                clsBusinessSummary.LoadByGrossReceipt(this, RegYear);
            }
            else if (ReportType.By_Initial_Capital == ReportType)
            {
                this.Text = "SUMMARY OF BUSINESSES BY INITIAL CAPITAL";
                clsBusinessSummary.LoadByInitCapital(this, RegYear);
            }
        }

        public enum ReportType
        {
            By_Barangay,
            By_District,
            By_Line_Of_Business,
            By_Org_Kind,
            By_Gross_Receipts,
            By_Initial_Capital
        }

        private void ExportFile()
        {
            //MCR 20140618 (s) 
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
            //MCR 20140618 (e)
        }

        private void frmReport_Load(object sender, EventArgs e)
        {

        }

        public void CreateHeader() //MCR 20140618
        {
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterTop;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.FontSize = 10f;
            if (this.Text == "SUMMARY OF BUSINESSES BY BARANGAY"
                || this.Text == "SUMMARY OF BUSINESSES BY DISTRICT"
                || this.Text == "SUMMARY OF BUSINESSES BY LINE OF BUSINESS")
                sSize = "16000";
            else if (this.Text == "SUMMARY OF BUSINESSES BY ORGANIZATION KIND")
                sSize = "17500";
            else if (this.Text == "SUMMARY OF BUSINESSES BY GROSS RECEIPTS")
                sSize = "16500";
            else if (this.Text == "SUMMARY OF BUSINESSES BY INITIAL CAPITAL")
                sSize = "19100";

            //MOD MCR 20140618 (s)
            axVSPrinter1.Table = String.Format("^{1};{0}", "Republic of the Philippines", sSize);
            //axVSPrinter1.Table = String.Format("^{1};{0}", AppSettingsManager.GetConfigValue("08"), sSize);   // RMC 20150429 corrected reports, put rem
            // RMC 20150429 corrected reports (s)
            if(AppSettingsManager.GetConfigValue("01") == "MUNICIPALITY")
                axVSPrinter1.Table = String.Format("^{1};{0}", "PROVINCE OF " + AppSettingsManager.GetConfigValue("08"), sSize);
            // RMC 20150429 corrected reports (e)
            axVSPrinter1.FontBold = true;
            //axVSPrinter1.Table = String.Format("^{1};{0}", AppSettingsManager.GetConfigValue("02"), sSize);   // RMC 20150429 corrected reports, put rem
            axVSPrinter1.Table = String.Format("^{1};{0}", AppSettingsManager.GetConfigValue("09"), sSize); // RMC 20150429 corrected reports 
            axVSPrinter1.FontBold = false;
            axVSPrinter1.Table = String.Format("^{1};{0}", "Business Permit and Licensing Office", sSize);
            axVSPrinter1.Paragraph = String.Empty;
            axVSPrinter1.Table = String.Format("^{1};{0}", this.Text, sSize);
            axVSPrinter1.FontSize = 12f;
            axVSPrinter1.FontSize = 8.0f;
            if (m_strRegYear.Trim() != "")
                axVSPrinter1.Table = String.Format("^{1};{0}", "Based on " + m_strRegYear + " Registration", sSize);
            else
                axVSPrinter1.Table = String.Format("^{1};{0}", "Based on Records", sSize);
            axVSPrinter1.Paragraph = String.Empty;
            axVSPrinter1.Table = String.Format("^{1};{0}", "As of " + AppSettingsManager.GetCurrentDate().ToString("dd MMMM yyyy"), sSize);
            //MOD MCR 20140618 (e)

            axVSPrinter1.FontBold = true;
            if (this.Text == "SUMMARY OF BUSINESSES BY BARANGAY")
            {
                axVSPrinter1.Table = String.Format(">16000;{0}", "Page " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = String.Format("^3000|^3000|^5000|^5000;{0}", "|New|Renewal|Retired;");
                axVSPrinter1.Table = String.Format("^3000|^1000|^2000|^2000|^3000|^2000|^3000;{0}", "Barangay|No.|Initial Capital|No.|Declared Gross Receipts|No.|Declared Gross Receipts;");
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "SUMMARY OF BUSINESSES BY DISTRICT")
            {
                axVSPrinter1.Table = String.Format(">16000;{0}", "Page " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = String.Format("^2000|^3000|^5500|^5500;{0}", "|New|Renewal|Retired;");
                axVSPrinter1.Table = String.Format("^2000|^1000|^2000|^1000|^2250|^2250|^1000|^2250|^2250;{0}", "District|No.|Initial Capital|No.|Declared Gross Receipts|Presumptive Gross Receipts|No.|Declared Gross Receipts|Presumptive Gross Receipts;");
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "SUMMARY OF BUSINESSES BY LINE OF BUSINESS")
            {
                axVSPrinter1.Table = String.Format(">16000;{0}", "Page " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = String.Format("^3000|^3000|^5000|^5000;{0}", "|New|Renewal|Retired;");
                axVSPrinter1.Table = String.Format("^3000|^1000|^2000|^2000|^3000|^2000|^3000;{0}", "Line of Business|No.|Initial Capital|No.|Declared Gross Receipts|No.|Declared Gross Receipts;");
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "SUMMARY OF BUSINESSES BY ORGANIZATION KIND")
            {
                axVSPrinter1.Table = String.Format(">17500;{0}", "Page " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = String.Format("^1500|^4000|^4000|^4000|^4000;{0}", "|Single Proprietorship|Partnership|Corporation|Cooperative;");
                axVSPrinter1.Table = String.Format("^1500|^1000|^1500|^1500|^1000|^1500|^1500|^1000|^1500|^1500|^1000|^1500|^1500;{0}", "Barangay|No.|Initial Capital|Declared GR|No.|Initial Capital|Declared GR|No.|Initial Capital|Declared GR|No.|Initial Capital|Declared GR;");
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "SUMMARY OF BUSINESSES BY GROSS RECEIPTS")
            {
                axVSPrinter1.Table = String.Format(">16500;{0}", "Page " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = String.Format("^1500|^3000|^3000|^3000|^3000|^3000;{0}", "|Below 100,000|100,000 to < 1M|1m to < 20M|20M and Above|Total;");
                axVSPrinter1.Table = String.Format("^1500|^1000|^2000|^1000|^2000|^1000|^2000|^1000|^2000|^1000|^2000;{0}", "Barangay|No.|Declared|No.|Declared|No.|Declared|No.|Declared|No.|Declared;");
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "SUMMARY OF BUSINESSES BY INITIAL CAPITAL")
            {
                //tmp 19100
                //tmp 1500|800|1400
                axVSPrinter1.Table = String.Format(">19100;{0}", "Page " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = String.Format("^1500|^2200|^2200|^2200|^2200|^2200|^2200|^2200|^2200;{0}", "|Below 100,000|100,000 to < 500,000|500,000 to < 1M|1M to < 5M|5M to < 10M|10M to < 20M|20M and Above|Total;");
                axVSPrinter1.Table = String.Format("^1500|^800|^1400|^800|^1400|^800|^1400|^800|^1400|^800|^1400|^800|^1400|^800|^1400|^800|^1400;{0}", "Barangay|No.|Initial Capital|No.|Initial Capital|No.|Initial Capital|No.|Initial Capital|No.|Initial Capital|No.|Initial Capital|No.|Initial Capital|No.|Initial Capital;");
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            if (!m_bInit)
                CreateHeader();

            m_bInit = false;

            axVSPrinter1.FontBold = false;
            try { m_strUser = AppSettingsManager.SystemUser.UserCode; }
            catch { m_strUser = "SYS_PROG"; }

            axVSPrinter1.HdrFontName = "Arial";
            axVSPrinter1.HdrFontSize = 8.0f;
            axVSPrinter1.Footer = "Printed by: " + m_strUser + "\n";
            axVSPrinter1.Footer += "Printed date: " + AppSettingsManager.GetCurrentDate().ToString("yyyy-MM-dd hh:mm:ss");
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
