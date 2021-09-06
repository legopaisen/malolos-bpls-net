// AST 20140121 added business queue
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using Amellar.Common.EncryptUtilities;
using Amellar.Common.StringUtilities;
using Amellar.Common.DynamicProgressBar;
using System.Threading;

namespace BusinessRoll
{
    public partial class frmReport : Form
    {
        public frmReport()
        {
            InitializeComponent();
        }

        private void ExportFile() //MCR 20140618
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.DefaultExt = "html";
                dlg.Title = "Reports";
                dlg.Filter = "HTML Files (*.html;*.htm)|*.html|RTF Files (*.rtf)|*.rtf|Excel Files (*.xls)|*.xls";
                dlg.FilterIndex = 3;
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    axVSPrinter1.ExportFormat = VSPrinter7Lib.ExportFormatSettings.vpxPlainHTML;
                    axVSPrinter1.ExportFile = dlg.FileName;
                }
            }
        }
        
        string sSize = "";
        bool m_bInit = true;

        //MCR 20140709 (s)
        public frmProgress m_form; 
        public Thread m_objThread;
        public delegate void DifferentDelegate(object value, frmProgress.ProgressMode mode);
        public static void DoSomethingDifferent(object value, frmProgress.ProgressMode mode, DifferentDelegate threadFunction)
        {
            threadFunction(value, mode); // NOTE: invoked with a parameter
        }
        public void ThreadProcess()
        {
            using (m_form = new frmProgress())
            {

                m_form.TopMost = true;

                m_form.ShowDialog();
            }
        }
        //MCR 20140709 (e)

        public frmReport(ReportType ReportType, frmBusinessRoll frmBusinessRoll)
        {
            InitializeComponent();
            
            ExportFile();

            if (ReportType == ReportType.Barangay)
            {
                this.Text = "Report by Barangay";
                clsBusinessRoll.LoadByBarangay(this, frmBusinessRoll);
            }
            else if (ReportType == ReportType.Unrenewed)
            {
                this.Text = "Unrenewed";
                clsBusinessRoll.LoadUnrenewed(this, frmBusinessRoll);
            }
            else if (ReportType == ReportType.District)
            {
                this.Text = "Report by District";
                clsBusinessRoll.LoadByDistrict(this, frmBusinessRoll);
            }
            else if (ReportType == ReportType.LineOfBusiness)
            {
                this.Text = "Report by Line of Business";
                clsBusinessRoll.LoadByLineOfBusiness(this, frmBusinessRoll);
            }
            else if (ReportType == ReportType.Street)
            {
                this.Text = "Report by Street";
                clsBusinessRoll.LoadByStreet(this, frmBusinessRoll);
            }
            else if (ReportType == ReportType.TopGrosses)
            {
                this.Text = "Report by Top Grosses";
                clsBusinessRoll.LoadByTopGrosses(this, frmBusinessRoll);
            }
            else if (ReportType == ReportType.TopPayers)
            {
                this.Text = "Report by Top Payers";
                clsBusinessRoll.LoadByTopPayers(this, frmBusinessRoll);
            }
            else if (ReportType == ReportType.LeastGrosses)
            {
                this.Text = "Report by Least Grosses";
                clsBusinessRoll.LoadByLeaseGrosses(this, frmBusinessRoll);
            }
            else if (ReportType == ReportType.LeastPayers)
            {
                this.Text = "Report by Least Payers";
                clsBusinessRoll.LoadByLeastPayers(this, frmBusinessRoll);
            }
            else if (ReportType == ReportType.Name)
            {
                this.Text = "Report by Name";
                clsBusinessRoll.LoadByName(this, frmBusinessRoll);
            }
            else if (ReportType == ReportType.BusinessOnQueue)
            {
                this.Text = "Report by Business Queue";
                clsBusinessRoll.LoadByBusinessQueue(this, frmBusinessRoll);
            }
            else if(ReportType == ReportType.TopAssessed)
            {
                this.Text = "Report by Top Assessed Tax";
                clsBusinessRoll.LoadByTopAssessed(this, frmBusinessRoll);
            }

        }

        public enum ReportType
        {
            Barangay, 
            District,
            LineOfBusiness,
            Street,
            TopGrosses,
            TopPayers,
            LeastGrosses,
            LeastPayers,
            Name,
            BusinessOnQueue, // AST 20140121 added business queue
            Unrenewed, //MCR 20141110
            TopAssessed //JARS 20170720
        }

        private void frmReport_Load(object sender, EventArgs e)
        {

        }

        string _strBnsType = "";
        public string BusinessType
        {
            set { _strBnsType = value; }
        }

        string _strGross = "";
        public string Gross
        {
            set { _strGross = value; }
        }

        string _strTaxYear = "";
        public string TaxYear
        {
            set { _strTaxYear = value; }
        }

        string _strRepTitle = "";
        public string ReportTitle
        {
            set { _strRepTitle = value; }
        }

        public void CreateHeader()
        {
            int iYear = AppSettingsManager.GetCurrentDate().Year;
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterTop;
            axVSPrinter1.FontName = "Arial Narrow";
            axVSPrinter1.FontSize = 10.0f;

            if (this.Text == "Report by Barangay")
                sSize = "18300";
            else if (this.Text == "Report by District"
                || this.Text == "Report by Line of Business"
                || this.Text == "Report by Street")
                sSize = "18300";
            else if (this.Text == "Report by Top Grosses"
                || this.Text == "Report by Least Grosses")
                //sSize = "18500";
                sSize = "19000"; //JAV 20170823
            else if (this.Text == "Report by Top Payers" || this.Text == "Report by Top Assessed Tax")
                sSize = "18500";
            else if (this.Text == "Report by Least Payers"
                || this.Text == "Report by Business Queue"
                || this.Text == "Unrenewed")
                sSize = "17500";
            else if (this.Text == "Report by Name")
                sSize = "18000";

            axVSPrinter1.Table = string.Format("^{1};{0}", "Republic of the Philippines",sSize);
            String strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                //this.axVSPrinter1.Table = string.Format("^9000;{0}", strProvinceName);
                this.axVSPrinter1.Table = string.Format("^9000;{0}", "PROVINCE OF " + strProvinceName); // RMC 20150429 corrected reports
            axVSPrinter1.FontBold = true;
            //axVSPrinter1.Table = string.Format("^{1};{0}", AppSettingsManager.GetConfigValue("02"),sSize);
            axVSPrinter1.Table = string.Format("^{1};{0}", AppSettingsManager.GetConfigValue("09"), sSize); // RMC 20150429 corrected reports
            axVSPrinter1.FontBold = false;
            // MCR ADD 20140414 Province Name(s)
            this.axVSPrinter1.Table = string.Format("^{1};{0}", AppSettingsManager.GetConfigValue("41"),sSize);
            // MCR ADD 20140414(e)
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TextColor = Color.Blue;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.Table = string.Format("^{1};{0}", _strRepTitle,sSize);
            axVSPrinter1.TextColor = Color.Black;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.FontSize = 8.0f;
            axVSPrinter1.Table = string.Format("^{1};{0}", " As of " + AppSettingsManager.GetCurrentDate().ToString("MMMM dd, yyyy"),sSize);
            axVSPrinter1.Paragraph = "";

            axVSPrinter1.FontBold = true;
            if (this.Text == "Report by Barangay")
            {
                axVSPrinter1.Table = string.Format(">18300;{0}", "Page " + axVSPrinter1.PageCount); 
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                //axVSPrinter1.Table = string.Format("^1200|^2750|^2750|^2000|^1000|^1600|^1600|^1100|^1500|^2500;{0}", "|||||||||Business Permit");
                //axVSPrinter1.Table = string.Format("^1200|^2750|^2750|^2000|^1000|^1600|^1600|^1100|^1500|^1250|^1250;{0}", "Bin|Business Name|Owner's Name|Main Business|Status|Initial Capital|Gross Receipts|Date Operated|Contact No.|No.|Date"); // JAV 20170830

               // axVSPrinter1.Table = string.Format("^1200|^2200|^2200|^2000|<1500|^1000|^1600|^1600|^1100|^1200|^2500;{0}", "||||||||||Business Permit");
               // axVSPrinter1.Table = string.Format("^1200|^2200|^2200|^2000|<1500|^1000|^1600|^1600|^1100|^1200|^1250|^1250;{0}", "Bin|Business Name|Owner's Name|Main Business|DTI/SEC|Status|Initial Capital|Gross Receipts|Date Operated|Contact No.|No.|Date");
                //axVSPrinter1.Table = string.Format("^1200|^2000|^2000|^2000|^2000|<1500|^1000|^1600|^1600|^1100|^1200|^2500;{0}", "|||||||||||Business Permit");
                //axVSPrinter1.Table = string.Format("^1200|^2000|^2000|^2000|^2000|<1500|^1000|^1600|^1600|^1100|^1200|^1250|^1250;{0}", "Bin|Business Name|Business Address|Owner's Name|Main Business|DTI/SEC|Status|Initial Capital|Gross Receipts|Date Operated|Contact No.|No.|Date");
                axVSPrinter1.Table = string.Format("^1200|^1500|^2000|^1500|^1500|<1500|^1300|^1000|^1600|^1600|^1100|^1200|^1250|^1250|^500;{0}", "Bin|Business Name|Business Address|Owner's Name|Owner's Address|Main Business|DTI/SEC|Status|Initial Capital|Gross Receipts|Date Operated|Contact No.|Permit No.|Permit Date|No. of Employees"); // AFM 20200218 MAO-20-12359 added column for own address and no. of employees
                
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Unrenewed")
            {
                axVSPrinter1.Table = string.Format(">17500;{0}", "Page " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = string.Format("^2500|^4000|^4000|^3000|^1500|^1000|^1500;{0}", "Bin|Business Name|Owner's Name|Main Business|Status|Tax Year|Date Operated");
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Report by District")
            {
                axVSPrinter1.Table = string.Format(">17300;{0}", "Page " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = string.Format("^2000|^2300|^2300|^2000|^1000|^1600|^1600|^1600|^2500;{0}", "||||||||Business Permit");
                axVSPrinter1.Table = string.Format("^2000|^2300|^2300|^2000|^1000|^1600|^1600|^1600|^1250|^1250;{0}", "Bin|Business Name|Owner's Name|Main Business|Status|Initial Capital|Gross Receipts|Date Operated|No.|Date");
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Report by Line of Business")
            {
                axVSPrinter1.Table = string.Format(">17300;{0}", "Page " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                //axVSPrinter1.Table = string.Format("^2000|^2300|^2300|^2000|^1000|^1600|^1600|^1600|^2500;{0}", "||||||||Business Permit");
                //axVSPrinter1.Table = string.Format("^2000|^2300|^2300|^2000|^1000|^1600|^1600|^1600|^1250|^1250;{0}", "Bin|Business Name|Owner's Name|Main Business|Status|Initial Capital|Gross Receipts|Date Operated|No.|Date"); // JAV 20170830 
                /*axVSPrinter1.Table = string.Format("^2000|^2200|^2200|^2000|^1000|^1400|^1600|^1600|^1600|^2500;{0}", "|||||||||Business Permit");
                axVSPrinter1.Table = string.Format("^2000|^2200|^2200|^2000|^1000|^1400|^1600|^1600|^1600|^1250|^1250;{0}", "Bin|Business Name|Owner's Name|Main Business|DTI/SEC|Status|Initial Capital|Gross Receipts|Date Operated|No.|Date");
                  */
                // RMC 20171124 added contact no. in Business Roll by Line of Business (s)
                //axVSPrinter1.Table = string.Format("^1200|^2200|^2200|^2000|<1500|^1000|^1600|^1600|^1100|^1200|^2500;{0}", "||||||||||Business Permit");
                //axVSPrinter1.Table = string.Format("^1200|^2200|^2200|^2000|<1500|^1000|^1600|^1600|^1100|^1200|^1250|^1250;{0}", "Bin|Business Name|Owner's Name|Main Business|DTI/SEC|Status|Initial Capital|Gross Receipts|Date Operated|Contact No.|No.|Date");
                // RMC 20171124 added contact no. in Business Roll by Line of Business (e)
                //axVSPrinter1.Table = string.Format("^1200|^2000|^2000|^2000|^2000|<1500|^1000|^1600|^1600|^1100|^1200|^2500;{0}", "|||||||||||Business Permit");
                //axVSPrinter1.Table = string.Format("^1200|^2000|^2000|^2000|^2000|<1500|^1000|^1600|^1600|^1100|^1200|^1250|^1250;{0}", "Bin|Business Name|Business Address|Owner's Name|Main Business|DTI/SEC|Status|Initial Capital|Gross Receipts|Date Operated|Contact No.|No.|Date"); // jhb 20190424 revise as per LGU malolos request
                axVSPrinter1.Table = string.Format("^1200|^1500|^2000|^1500|^1500|^2000|^1000|^800|^1600|^1600|^1100|^1200|^1000|^1000|^500;{0}", "Bin|Business Name|Business Address|Owner's Name|Owner's Address|Main Business|DTI/SEC|Status|Initial Capital|Gross Receipts|Date Operated|Contact No.|Permit No.|Permit Date|No. of Employees"); // AFM 20200218 MAO-20-12359 added column for own address and no. of employees
            

                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Report by Street")
            {
                axVSPrinter1.Table = string.Format(">17300;{0}", "Page " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
              //  axVSPrinter1.Table = string.Format("^2000|^2500|^2500|^2000|^1000|^1600|^1600|^1600|^1250|^1250;{0}", "Bin|Business Name|Owner's Name|Main Business|Status|Initial Capital|Gross Receipts|Date Operated|PERMIT NUMBER|PERMIT DATE");
                axVSPrinter1.Table = string.Format("^2000|^2000|^2000|^1500|^1500|^1500|^1000|^1500|^1500|^1500|^1250|^1250|^500;{0}", "Bin|Business Name|Business Address|Owner's Name|Owner's Address|Main Business|Status|Initial Capital|Gross Receipts|Date Operated|Permit No.|Permit Date|No. Of Employees"); //AFM 20200218 MAO-20-12359
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Report by Top Grosses")
            {
                object objData = "BUSINESS TYPE: " + _strBnsType + "|";
                objData += "TOP: " + _strGross + " GROSSES" + "|";
                objData += "TAX YEAR: " + _strTaxYear + "|";
                objData += "Page: " + axVSPrinter1.PageCount;

                axVSPrinter1.Table = string.Format("<4625|^4625|^4625|>4625;{0}", objData);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                //axVSPrinter1.Table = string.Format("^2500|^2500|^2500|^2500|^2000|^1500|^1500|^1500|^1300|^700;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business|Gross Receipts|Date Operated|Permit Number|Permit Date|Tax Paid|Qtr");
                //axVSPrinter1.Table = string.Format("^2000|^2500|^2500|^2500|^1000|^2000|^1500|^1500|^1500|^1300|^700;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business|DTI/SEC|Gross Receipts|Date Operated|Permit Number|Permit Date|Tax Paid|Qtr"); //JAV 20170823
                axVSPrinter1.Table = string.Format("^1500|^1500|^1500|^1500|^1500|^1500|^1000|^2000|^1500|^1500|^1500|^1300|^700|^500;{0}", "Bin|Business Name|Business Address|Owner's Name|Owner's Address|Main Business|DTI/SEC|Gross Receipts|Date Operated|Permit Number|Permit Date|Tax Paid|Qtr|No. of Employees"); //AFM 20200219
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Report by Top Payers")
            {
                object objData = "BUSINESS TYPE: " + _strBnsType + "|";
                objData += "TOP: " + _strGross + "|";
                objData += "TAX YEAR: " + _strTaxYear + "|";
                objData += "Page: " + axVSPrinter1.PageCount;

                axVSPrinter1.Table = string.Format("<4350|^4350|^4350|>4350;{0}", objData);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                //^2500|^3000|^3000|^3000|^2000|^1500|^1500|^1500|^1000
                //axVSPrinter1.Table = string.Format("^2500|^3000|^3000|^3000|^1900|^1200|^1400|^1300|^1700;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business\n/Status/Orgn Kind|Gross Receipts\n/ Capital|Date\nOperated|Permit Number|Permit Date|Tax Paid");
                //axVSPrinter1.Table = string.Format("^1250|^2500|^2500|^2500|^1900|^1200|^1400|^1300|^1500|^700|^500;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business\n/Status/Orgn Kind|Gross Receipts\n/ Capital|Date\nOperated|Permit Number|Permit Date|Tax Paid|Payment Mode|No. of Qtrs");  // RMC 20150806 added mode of payment & no. of qtrs paid in Business Roll Top Payers report
                //axVSPrinter1.Table = string.Format("^2450|^2500|^2500|^2500|^1900|^1200|^1400|^1300|^1500;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business\n/Status/Orgn Kind|Gross Receipts\n/ Capital|Date\nOperated|Permit Number|Permit Date|Tax Paid"); // JAV 20170911
                axVSPrinter1.Table = string.Format("^2000|^1500|^1500|^1500|^1500|^2500|^1900|^1200|^1400|^1300|^1500|^500;{0}", "Bin|Business Name|Business Address|Owner's Name|Owner's Address|Main Business\n/Status/Orgn Kind|Gross Receipts\n/ Capital|Date\nOperated|Permit No.|Permit Date|Tax Paid|No. of Employees"); // AFM 20200219
                
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Report by Least Grosses")
            {
                object objData = "BUSINESS TYPE: " + _strBnsType + "|";
                objData += "TOP: " + _strGross + "|";
                objData += "TAX YEAR: " + _strTaxYear + "|";
                objData += "Page: " + axVSPrinter1.PageCount;

                axVSPrinter1.Table = string.Format("<4625|^4625|^4625|>4625;{0}", "BUSINESS TYPE: |" + "TOP: |" + "TAX YEAR: |" + "Page: " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                //axVSPrinter1.Table = string.Format("^2500|^2500|^2500|^2500|^2000|^1500|^1500|^1500|^1300|^700;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business|Gross Receipts|Date Operated|Permit Number|Permit Date|Tax Paid|Qtr"); // JAV 20170911
                //axVSPrinter1.Table = string.Format("^2100|^2300|^2300|^2200|^1300|^1800|^1500|^1500|^1500|^1300|^700;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business|DTI/SEC|Gross Receipts|Date Operated|Permit Number|Permit Date|Tax Paid|Qtr");
                axVSPrinter1.Table = string.Format("^1500|^1500|^1500|^1500|^1500|^2200|^1300|^1800|^1500|^1500|^1500|^1300|^700|^500;{0}", "Bin|Business Name|Business Address|Owner's Name|Owner's Address|Main Business|DTI/SEC|Gross Receipts|Date Operated|Permit No.|Permit Date|Tax Paid|Qtr|No. of Employees"); //AFM 20200219
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Report by Least Payers")
            {
                axVSPrinter1.Table = string.Format("<4375|^4375|^4375|>4375;{0}", "BUSINESS TYPE: |" + "TOP: |" + "TAX YEAR: |" + "Page: " + axVSPrinter1.PageCount);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                //axVSPrinter1.Table = string.Format("^2500|^2500|^2500|^2500|^2000|^1500|^1500|^1500|^1000;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business\n/Status/Orgn Kind|Gross Receipts\n/ Capital|Date Operated|Permit Number|Permit Date|Tax Paid");
                axVSPrinter1.Table = string.Format("^1500|^1500|^1500|^1500|^1500|^2500|^2000|^1500|^1500|^1500|^1000|^500;{0}", "Bin|Business Name|Business Address|Owner's Name|Owner's Address|Main Business\n/Status/Orgn Kind|Gross Receipts\n/ Capital|Date Operated|Permit No.|Permit Date|Tax Paid|No. of Employees"); //AFM 20200218
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Report by Name")
            {
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = string.Format("^2500|^2500|^2500|^2500|^2000|^1500|^1500|^1500|^1500;{0}", "Bin|Business Name|Owner's Name|Main Business|Status|Initial Capital|Gross Receipts|Date Operated|Date of Application");
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Report by Business Queue")
            {
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = string.Format("^2500|^2500|^2500|^2500|^2000|^1500|^1500|^1500|^1000|^1000;{0}", "Bin|Business Name|Owner's Name|Main Business|Status|Iniital Capital|Gross Receipts|Date Operated|Penalty|Billed Amount"); //AFM 20190725 new "Penalty" column
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (this.Text == "Report by Top Assessed Tax") //JARS 20170818 TOP ASSESSED HEADER
            {
                object objData = "BUSINESS TYPE: " + _strBnsType + "|";
                objData += "TOP: " + _strGross + "|";
                objData += "TAX YEAR: " + _strTaxYear + "|";
                objData += "Page: " + axVSPrinter1.PageCount;

                axVSPrinter1.Table = string.Format("<4350|^4350|^4350|>4350;{0}", objData);
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                //^2500|^3000|^3000|^3000|^2000|^1500|^1500|^1500|^1000
                //axVSPrinter1.Table = string.Format("^2500|^3000|^3000|^3000|^1900|^1200|^1400|^1300|^1700;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business\n/Status/Orgn Kind|Gross Receipts\n/ Capital|Date\nOperated|Permit Number|Permit Date|Tax Paid");
                //axVSPrinter1.Table = string.Format("^1250|^2500|^2500|^2500|^1900|^1200|^1400|^1300|^1500|^700|^500;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business\n/Status/Orgn Kind|Gross Receipts\n/ Capital|Date\nOperated|Permit Number|Permit Date|Assessed Tax|Payment Mode|No. of Qtrs");//JAV 20170824
                //axVSPrinter1.Table = string.Format("^1250|^2500|^2500|^2500|^1900|^1200|^1400|^1300|^1200|^1500;{0}", "Bin|Business Name Address|Owner's Name Address|Main Business\n/Status/Orgn Kind|Gross Receipts\n/ Capital|Date\nOperated|Permit Number|Permit Date|Tax Paid|Assessed Tax");//JAV 20170824
                axVSPrinter1.Table = string.Format("^1250|^1700|^1700|^1700|^1700|^1700|^1800|^1200|^1400|^1300|^1200|^1500|^500;{0}", "Bin|Business Name|Business Address|Owner's Name|Owner's Address|Main Business\n/Status/Orgn Kind|Gross Receipts\n/ Capital|Date\nOperated|Permit No.|Permit Date|Tax Paid|Assessed Tax|No. of Employees");//AFM 20200218
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            axVSPrinter1.FontBold = false;
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            if (!m_bInit)
                CreateHeader();

            m_bInit = false;

            axVSPrinter1.FontBold = false;
            String strUser = "";
            try { strUser = AppSettingsManager.SystemUser.UserCode; }
            catch { strUser = "SYS_PROG"; }

            axVSPrinter1.HdrFontName = "Arial";
            axVSPrinter1.HdrFontSize = 8.0f;
            axVSPrinter1.Footer = "PRINTED BY: " + strUser + "\nDate Printed: " + AppSettingsManager.GetCurrentDate().ToString("dd MMMM, yyyy hh:mm:ss");

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
