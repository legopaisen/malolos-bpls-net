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
using Amellar.Common.AuditTrail;
using Amellar.Common.TransactionLog;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmPrinting : Form
    {
        public frmPrinting()
        {
            InitializeComponent();
        }

        private string m_sIssuedDate = String.Empty;
        public string IssuedDate
        {
            set { m_sIssuedDate = value; }
        }
        private string m_sPermitNo = String.Empty;
        public string PermitNo
        {
            set { m_sPermitNo = value; }
        }
        private string m_sReportType = String.Empty;
        private string m_sBin = String.Empty;
        private string m_sTaxYear = String.Empty;
        public string BIN
        {
            set { m_sBin = value; }
        }
        public string ReportType
        {
            set { m_sReportType = value; }
        }
        public string TaxYear
        {
            set { m_sTaxYear = value; }
        }
        
        //MCR 20150326 (s)
        public string PermitType
        {
            set { m_sPermitType = value; }
        }
        private string m_sPermitType = String.Empty;
        //MCR 20150326 (e)

        private string m_sORNo = String.Empty;
        private string m_sORDate = String.Empty;
        private string m_sFeeAmount = String.Empty;

        //JHB 20190926 for Brgy Clearance (s)
        private string m_sIssuedOn = String.Empty; 
        public string IssuedOn
        {
            set { m_sIssuedOn = value; }
        }

        private string m_sTempBIN = string.Empty;
        public string TempBIN
        {
            get { return m_sTempBIN; }
            set { m_sTempBIN = value; }
        }
        //JHB 20190926 for Brgy Clearance (e)
        private bool brgy_reprint = false; //jhb 20200108
        public string ORNo
        {
            set { m_sORNo = value; }
        }
        public string ORDate
        {
            set { m_sORDate = value; }
        }
        public string FeeAmount
        {
            set { m_sFeeAmount = value; }
        }

        private string m_sBnsName = string.Empty;
        public string BnsName
        {
            set { m_sBnsName = value; }
        }

        private string m_sBnsAdd = string.Empty;
        public string BnsAdd
        {
            set { m_sBnsAdd = value; }
        }

        private string m_sBnsOwn = string.Empty;
        public string BnsOwn
        {
            set { m_sBnsOwn = value; }
        }

        //MCR 20150113 (s) Working Permit

        private string m_sEmpID = string.Empty;
        public string EmpID
        {
            set { m_sEmpID = value; }
        }

        private string m_sEmpLn = string.Empty; //JARS 20160118
        public string EmpLn
        {
            set { m_sEmpLn = value; }
        }

        private string m_sEmpFn = string.Empty; //JARS 20160118
        public string EmpFn
        {
            set { m_sEmpFn = value; }
        }
        //MCR 20150113 (e) Working Permit

        public string m_timeIN = string.Empty;
        public string timeIN
        {
            set { m_timeIN = value; }
        }
        public DateTime m_dTransLogIn = new DateTime();
        private DateTime m_dTransLogOut = AppSettingsManager.GetSystemDate();

        private void frmPrinting_Load(object sender, EventArgs e)
        {
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape


            if (m_sReportType == "Health")
            {
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                Health();
            }
            else if (m_sReportType == "Working Permit")
            {
                axVSPrinter1.MarginTop = 1080;
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                WorkingPermit();
            }
            else if (m_sReportType == "Sanitary" || m_sReportType == "Sanitary-Ext")
            {
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                //Sanitary();
               // SanitaryNew();  // RMC 20141222 modified permit printing
                SanitaryNew_revised();
            }
            else if (m_sReportType == "Zoning")
            {
                axVSPrinter1.MarginTop = 1080;
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                Zoning();
            }
            else if (m_sReportType == "Annual Inspection" || m_sReportType == "Annual Inspection-Ext")
            {
                axVSPrinter1.MarginTop = 1080;
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
               // AnnualInspection();
                AnnualInspection_NEW();
            }
            else if (m_sReportType == "SelectedHealth") //JARS 20160118
            {
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                SelectedHealth();
            }
           // axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
            //JHB 20191012 add for brgy clearance (s)
            else if (m_sReportType == "Barangay Clearance") 
            {
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Times New Roman";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                if (MessageBox.Show("Use pre-printed form?", "Baranagy Clearance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Barangay_Certificate();
                else
                   
                    Barangay_Certificate_new();
            }
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }//JHB 20191012 add for brgy clearance (e)

        private void Zoning()
        {
            OracleResultSet result = new OracleResultSet();

            string sORNo = string.Empty;
            string sFeeAmount = string.Empty;
            string sTCTNo = string.Empty;
            string sZoning = string.Empty;
            string sName = string.Empty;
            string sSPNo = string.Empty;
            string sMPNo = string.Empty;
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;

            // RMC 20150105 mods in permit printing (s)
            
            string sAdd = string.Empty;
            result.Query = "select * from emp_names where (bin = '" + m_sBin + "' or temp_bin = '" + m_sBin + "') and tax_year = '" + m_sTaxYear + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sAdd = result.GetString("bns_add");
                }
            }
            result.Close();
            // RMC 20150105 mods in permit printing (e)

            //result.Query = "Select * from zoning where bin ='" + m_sBin + "'";
            //result.Query += " and tax_year = '" + m_sTaxYear + "'";     // RMC 20150105 mods in permit printing

            //MCR 20150910 (s)
            String sQuery = "";
            sQuery = "select distinct * from ( ";
            sQuery += "select distinct z.BIN, z.TAX_YEAR, o.OWN_LN, o.OWN_FN, o.OWN_MI, z.TCT_NO, z.ZONING, z.SP_NO, z.MP_NO from zoning z ";
            sQuery += "inner join businesses B on B.bin = z.Bin ";
            sQuery += "inner join own_names O on o.Own_code = b.own_code ";
            sQuery += "union all ";
            sQuery += "select distinct z.BIN, z.TAX_YEAR, o.OWN_LN, o.OWN_FN, o.OWN_MI, z.TCT_NO, z.ZONING, z.SP_NO, z.MP_NO from zoning z ";
            sQuery += "inner join business_que B on B.bin = z.Bin ";
            sQuery += "inner join own_names O on o.Own_code = b.own_code ";
            sQuery += "union all ";
            sQuery += "select distinct BIN, TAX_YEAR, OWN_LN, OWN_FN, OWN_MI, TCT_NO, ZONING, SP_NO, MP_NO from zoning ";
            sQuery += "where bin not in (select bin from businesses) and bin not in (select bin from business_que) ";
            sQuery += ") where tax_year = '" + m_sTaxYear + "' and bin = '" + m_sBin + "' order by bin ";
            //MCR 20150910 (e)

            result.Query = sQuery;
            if (result.Execute())
            {
                if (result.Read()) //JARS 20160921 FROM WHILE TO IF
                {
                    try
                    {
                        sTCTNo = "TCT No. " + result.GetString("TCT_No");
                        sZoning = result.GetString("zoning");
                        sName = result.GetString("own_ln") + ", " + result.GetString("own_fn") + " " + result.GetString("own_mi");
                        sSPNo = result.GetString("sp_no");
                        sMPNo = result.GetString("mp_no");
                        sYear = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
                        sMonth = string.Format("{0:MMM}", AppSettingsManager.GetCurrentDate());
                        sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());

                        axVSPrinter1.MarginLeft = 750;
                        axVSPrinter1.SpaceBefore = 3500;
                        /*axVSPrinter1.FontSize = 14;
                        axVSPrinter1.Table = "<10|<4200|<3500|<3000;|This is to certify that a lot bearing|" + sTCTNo + "| located in Barangay";
                        axVSPrinter1.SpaceBefore = 50;
                        axVSPrinter1.Table = "<10|<11000|<3200;|San Roque Arbol, Lubao, Pampanga, is within the " + sZoning;
                        axVSPrinter1.SpaceBefore = 100;
                        axVSPrinter1.Table = "<10|<11000;|Municipality's Comprehensive Land User Plan approved by the Sangguniang";
                        axVSPrinter1.Table = "<10|<5000|<3500|<4000;|Panlalawigan through SP Resolution No.|" + sSPNo + "|and adopted by the";
                        axVSPrinter1.SpaceBefore = 70;
                        axVSPrinter1.Table = "<10|<7600|<3500;|Sangguniang Bayan of Lubao through Municipal Resolution No.|" + sMPNo;
                        axVSPrinter1.SpaceBefore = 200;
                        axVSPrinter1.Table = "<10|<4300|<3000|<700|<1200|<1300|<1800;|This certification is being issued to |" + sName + "  this |" + sDay + "th" + "| day of |" + sMonth + ". " + sYear;
                        axVSPrinter1.SpaceBefore = 100;
                        axVSPrinter1.Table = "<10|<8000;|for reference purposes only.";*/

                        // RMC 20150105 mods in permit printing (s)
                        axVSPrinter1.FontSize = 12;
                        //axVSPrinter1.TabIndex = 100;
                        //string sFormat = "=9000;";
                        string sData = string.Empty;

                        this.axVSPrinter1.IndentTab = "0.25in";
                        this.axVSPrinter1.IndentFirst = "0.25in";
                        sData = "This is to certify that a lot bearing " + sTCTNo + " located in " + sAdd + ", ";
                        sData += "Lubao, Pampanga, is within the " + sZoning + " Zone of Municipality's Comprehensive Land Use Plan approved by the Sangguniang ";
                        sData += "Panlalawigan through SP Resolution No. " + sSPNo + " and adopted by the ";
                        sData += "Sangguniang Bayan of Lubao through Municipal Resolution No. " + sMPNo + ".";
                        this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);
                        //axVSPrinter1.Table = sFormat + sData;
                        //axVSPrinter1.Paragraph = "";
                        //sFormat = "=9000;";
                        axVSPrinter1.SpaceBefore = 0;
                        this.axVSPrinter1.IndentTab = "0.25in";
                        this.axVSPrinter1.IndentFirst = "0.25in";
                        sData = "This certification is being issued to  " + sName + "  this " + ConvertDayInOrdinalForm(sDay) + " day of " + sMonth + ". " + sYear + " ";
                        sData += "for reference purposes only.";
                        this.axVSPrinter1.Table = string.Format("=10000;{0}", sData);
                        this.axVSPrinter1.IndentFirst = "0.0in";
                        // RMC 20150105 mods in permit printing (e)

                        axVSPrinter1.MarginLeft = 7000;
                        axVSPrinter1.SpaceBefore = 2000;
                        axVSPrinter1.Table = "<10|^4500;|" + AppSettingsManager.GetConfigValue("60");   //JARS 20160610 FROM 3000
                        axVSPrinter1.SpaceBefore = 0;
                        axVSPrinter1.Table = "<10|^4500;|MPDC/DZA"; //JARS 20160610 FROM 3000
                        axVSPrinter1.MarginLeft = 800;
                        axVSPrinter1.SpaceBefore = 1000;
                        //axVSPrinter1.Table = "<10|<4000;|O.R. No. _________"; // RMC 20150107 removed
                        axVSPrinter1.SpaceBefore = 0;
                        //axVSPrinter1.Table = "<10|<4000;|O.R. Date ________"; // RMC 20150107 removed
                        //axVSPrinter1.Table = "<10|<3500;|Fee Paid Php______"; // RMC 20150107 removed


                        //JHB 20210120 add date time (s)
                        axVSPrinter1.HdrFontName = "Arial";
                        axVSPrinter1.HdrFontSize = 7.0f;
                        axVSPrinter1.Footer += "Printed Date: " + AppSettingsManager.GetCurrentDate().ToString("yyyy-MM-dd hh:mm:ss");
                        //JHB 20210120 add date time (e)

                        //PermitRecord();
                    }
                    catch
                    {
                        sORNo = "";
                        sFeeAmount = "";
                        sTCTNo = "";
                        sZoning = "";
                        sName = "";
                        sSPNo = "";
                        sMPNo = "";
                    }
                }
            }
            string m_Stat = "";
            m_dTransLogIn = DateTime.Parse(m_timeIN);
            m_Stat = AppSettingsManager.GetBnsStat(m_sBin);
            //JHB 20210222 add time, insert to translog as per sir juli request (s)
            TransLog.UpdateLog(m_sBin, m_Stat, ConfigurationAttributes.CurrentYear, "Zoning", m_dTransLogIn, AppSettingsManager.GetSystemDate());
            //JHB 20210222 add time, insert to translog as per sir juli request (e)
        }

        private void Health()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rCount = new OracleResultSet();

            string sName = string.Empty;
            string sAge = string.Empty;
            string sSex = string.Empty;
            string sOccupation = string.Empty;
            string sPlaceOfWork = string.Empty;
            string sNationality = string.Empty;
            string sDate = string.Empty;
            string sExpiration = string.Empty;
            string sXRay = string.Empty;
            string sXRayResult = string.Empty;
            string sHepa = string.Empty;
            string sHepaResult = string.Empty;
            string sDrugTest = string.Empty;
            string sDrugTestResult = string.Empty;
            string sFeca = string.Empty;
            string sFecaResult = string.Empty;
            string sUrine = string.Empty;
            string sUrineResult = string.Empty;
            string sRegNo = string.Empty;
            int sCount = 0;
            int iCount = 0;

            //result.Query = "Select * from emp_names where bin ='" + m_sBin + "' and tax_year = '" + m_sTaxYear + "'";
            //rCount.Query = "Select count(*) as emp_count from emp_names where bin ='" + m_sBin + "' and tax_year = '" + m_sTaxYear + "'";
            result.Query = "Select * from emp_names where (bin ='" + m_sBin + "' or temp_bin = '" + m_sBin + "') and tax_year = '" + m_sTaxYear + "'";  // RMC 20141228 modified permit printing (lubao)
            rCount.Query = "Select count(*) as emp_count from emp_names where (bin ='" + m_sBin + "' or temp_bin = '" + m_sBin + "') and tax_year = '" + m_sTaxYear + "'";  // RMC 20141228 modified permit printing (lubao)
            int.TryParse(rCount.ExecuteScalar(), out sCount);

            if (result.Execute())
            {
                while (result.Read())
                {
                    try
                    {
                        sName = result.GetString("emp_ln") + ", " + result.GetString("emp_fn") + " " + result.GetString("emp_mi");
                        sAge = result.GetInt("emp_age").ToString();
                        sSex = result.GetString("emp_gender");
                        sOccupation = result.GetString("emp_occupation");
                        sPlaceOfWork = result.GetString("emp_place_of_work");
                        sNationality = result.GetString("emp_nationality");
                        sDate = result.GetString("emp_issuance_date");
                        sExpiration = result.GetString("emp_expiration_date");
                        sXRay = result.GetString("emp_xray_date");
                        sXRayResult = result.GetString("emp_xray_result");
                        sHepa = result.GetString("emp_hepab_date");
                        sHepaResult = result.GetString("emp_hepab_result");
                        sDrugTest = result.GetString("emp_drug_test_date");
                        sDrugTestResult = result.GetString("emp_drug_test_result");
                        sFeca = result.GetString("emp_fecalysis_date");
                        sFecaResult = result.GetString("emp_fecalysis_result");
                        sUrine = result.GetString("emp_urinalysis_date");
                        sUrineResult = result.GetString("emp_urinalysis_result");
                        sRegNo = result.GetString("emp_id");

                        //JARS 20170113 FROM <10 TO <15
                        axVSPrinter1.MarginLeft = 4560;
                        axVSPrinter1.Table = "<15|<5000;|";
                        axVSPrinter1.SpaceBefore = 300;//200
                        axVSPrinter1.FontSize = 8;
                        axVSPrinter1.Table = "<15|<5000;|" + sRegNo;
                        axVSPrinter1.MarginLeft = 510;
                        axVSPrinter1.SpaceBefore = 150; //70
                        axVSPrinter1.FontSize = 9;
                        axVSPrinter1.Table = "<15|<5000;|" + sName;
                        axVSPrinter1.SpaceBefore = 650; //580
                        axVSPrinter1.Table = "<15|^1700|<5000;|" + sAge + "|" + sSex;
                        axVSPrinter1.Table = "<15|<5000;|" + sOccupation;
                        axVSPrinter1.Table = "<15|<5000;|" + sPlaceOfWork;
                        axVSPrinter1.Table = "<15|<5000;|" + sNationality;
                        axVSPrinter1.SpaceBefore = 0;

                        iCount++;
                        if (iCount <= sCount)
                        {
                            axVSPrinter1.NewPage();
                            axVSPrinter1.MarginLeft = 500;
                            axVSPrinter1.Table = "<10|<5000;|";
                            axVSPrinter1.SpaceBefore = 350;
                            axVSPrinter1.Table = "<10|^2300|^3400;|" + sDate + "|" + sExpiration;
                            axVSPrinter1.SpaceBefore = 1200;
                            axVSPrinter1.Table = "<10|<3500|<3400;|" + sXRay + "|" + sXRayResult;
                            axVSPrinter1.SpaceBefore = 450;
                            axVSPrinter1.Table = "<10|<3500|<3400;|" + sHepa + "|" + sHepaResult;
                            axVSPrinter1.Table = "<10|<3500|<3400;|" + sDrugTest + "|" + sDrugTestResult;
                            axVSPrinter1.Table = "<10|<3500|<3400;|" + sFeca + "|" + sFecaResult;
                            axVSPrinter1.Table = "<10|<3500|<3400;|" + sUrine + "|" + sUrineResult;
                            axVSPrinter1.SpaceBefore = 0;
                            if (iCount < sCount)
                            {
                                axVSPrinter1.NewPage();
                            }
                        }
                    }
                    catch
                    {
                        sName = "";
                        sAge = "";
                        sSex = "";
                        sOccupation = "";
                        sPlaceOfWork = "";
                        sNationality = "";
                    }
                }
            }
            result.Close();
        }
        private void SelectedHealth() //JARS 20160118
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rCount = new OracleResultSet();

            string sName = string.Empty;
            string sAge = string.Empty;
            string sSex = string.Empty;
            string sOccupation = string.Empty;
            string sPlaceOfWork = string.Empty;
            string sNationality = string.Empty;
            string sDate = string.Empty;
            string sExpiration = string.Empty;
            string sXRay = string.Empty;
            string sXRayResult = string.Empty;
            string sHepa = string.Empty;
            string sHepaResult = string.Empty;
            string sDrugTest = string.Empty;
            string sDrugTestResult = string.Empty;
            string sFeca = string.Empty;
            string sFecaResult = string.Empty;
            string sUrine = string.Empty;
            string sUrineResult = string.Empty;
            string sRegNo = string.Empty;
            int sCount = 0;
            int iCount = 0;

            
            result.Query = "Select * from emp_names where (bin ='" + m_sBin + "' or temp_bin = '" + m_sBin + "') and tax_year = '" + m_sTaxYear + "' and emp_ln = '" + m_sEmpLn + "' and emp_fn = '" + m_sEmpFn + "'";
            
            if (result.Execute())
            {
                while (result.Read())
                {
                    try
                    {
                        sName = result.GetString("emp_ln") + ", " + result.GetString("emp_fn") + " " + result.GetString("emp_mi");
                        sAge = result.GetInt("emp_age").ToString();
                        sSex = result.GetString("emp_gender");
                        sOccupation = result.GetString("emp_occupation");
                        sPlaceOfWork = result.GetString("emp_place_of_work");
                        sNationality = result.GetString("emp_nationality");
                        sDate = result.GetString("emp_issuance_date");
                        sExpiration = result.GetString("emp_expiration_date");
                        sXRay = result.GetString("emp_xray_date");
                        sXRayResult = result.GetString("emp_xray_result");
                        sHepa = result.GetString("emp_hepab_date");
                        sHepaResult = result.GetString("emp_hepab_result");
                        sDrugTest = result.GetString("emp_drug_test_date");
                        sDrugTestResult = result.GetString("emp_drug_test_result");
                        sFeca = result.GetString("emp_fecalysis_date");
                        sFecaResult = result.GetString("emp_fecalysis_result");
                        sUrine = result.GetString("emp_urinalysis_date");
                        sUrineResult = result.GetString("emp_urinalysis_result");
                        sRegNo = result.GetString("emp_id");

                        axVSPrinter1.MarginLeft = 4560;
                        axVSPrinter1.Table = "<50|<5000;|";
                        axVSPrinter1.SpaceBefore = 300; //200
                        axVSPrinter1.FontSize = 8;
                        axVSPrinter1.Table = "<50|<5000;|" + sRegNo;
                        axVSPrinter1.MarginLeft = 510;
                        axVSPrinter1.SpaceBefore = 120;//70
                        axVSPrinter1.FontSize = 10;
                        axVSPrinter1.Table = "<50|<5000;|" + sName;
                        axVSPrinter1.SpaceBefore = 630;//580
                        axVSPrinter1.Table = "<50|<1700|<5000;|" + sAge + "|" + sSex;
                        axVSPrinter1.Table = "<50|<5000;|" + sOccupation;
                        axVSPrinter1.Table = "<50|<5000;|" + sPlaceOfWork;
                        axVSPrinter1.Table = "<50|<5000;|" + sNationality;
                        axVSPrinter1.SpaceBefore = 0;

                        axVSPrinter1.NewPage();
                        axVSPrinter1.MarginLeft = 500;
                        axVSPrinter1.Table = "<10|<5000;|";
                        axVSPrinter1.SpaceBefore = 350;
                        axVSPrinter1.Table = "<10|^2300|^3400;|" + sDate + "|" + sExpiration;
                        axVSPrinter1.SpaceBefore = 1200;
                        axVSPrinter1.Table = "<10|<3500|<3400;|" + sXRay + "|" + sXRayResult;
                        axVSPrinter1.SpaceBefore = 450;
                        axVSPrinter1.Table = "<10|<3500|<3400;|" + sHepa + "|" + sHepaResult;
                        axVSPrinter1.Table = "<10|<3500|<3400;|" + sDrugTest + "|" + sDrugTestResult;
                        axVSPrinter1.Table = "<10|<3500|<3400;|" + sFeca + "|" + sFecaResult;
                        axVSPrinter1.Table = "<10|<3500|<3400;|" + sUrine + "|" + sUrineResult;
                        axVSPrinter1.SpaceBefore = 0;
                            
                    }
                    catch
                    {
                        sName = "";
                        sAge = "";
                        sSex = "";
                        sOccupation = "";
                        sPlaceOfWork = "";
                        sNationality = "";
                    }
                }
            }
            result.Close();

            //JHB 20210222 add time, insert to translog as per sir juli request (s)
            string m_Stat = "";
            m_dTransLogIn = DateTime.Parse(m_timeIN);
            m_Stat = AppSettingsManager.GetBnsStat(m_sBin); 

           // if(!ValidateTransLog(sName,m_sBin,m_sTaxYear))-- for QA
            TransLog.UpdateLog(m_sBin, m_Stat, ConfigurationAttributes.CurrentYear, "Health Permit - " + sName + " ", m_dTransLogIn, AppSettingsManager.GetSystemDate());
            //JHB 20210222 add time, insert to translog as per sir juli request (e)
           
        }
        private void SanitaryNew()
        {
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sYear2 = string.Empty;

            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate());
            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
            sMonth = string.Format("{0:MMMM}", AppSettingsManager.GetCurrentDate());
            sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());

            axVSPrinter1.MarginLeft = 9000; 
            axVSPrinter1.SpaceBefore = 750; 
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Table = "<10|<5000;| PERMIT NO." + m_sPermitNo;

            axVSPrinter1.MarginLeft = 500; 
            axVSPrinter1.SpaceBefore = 2100;

            if (m_sBnsName.Length > 50)  
            {
                axVSPrinter1.FontSize = 18;  // MCR 20150121 mods in permit printing
            }
            else if (m_sBnsName.Length > 20)   // RMC 20150105 mods in permit printing
            {
                axVSPrinter1.FontSize = 24;  // RMC 20150105 mods in permit printing
                //axVSPrinter1.SpaceBefore = 1200;    // RMC 20150105 mods in permit printing
            }
            else
            {
                axVSPrinter1.FontSize = 32;
                //axVSPrinter1.SpaceBefore = 1800;    // RMC 20150105 mods in permit printing
            }

            axVSPrinter1.FontBold = true;

            axVSPrinter1.SpaceBefore = 1800;    // RMC 20150107
            long yGrid = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = 2476;

            if (m_sReportType == "Sanitary")
            {
                // RMC 20150107 (s)
                string sTmp = AppSettingsManager.GetDTIName(m_sBin).Trim();
                if (sTmp != "")
                    m_sBnsName = sTmp;
                // RMC 20150107 (e)
            }            
             
            axVSPrinter1.Table = "<10|^10000;|" + m_sBnsName;
            axVSPrinter1.MarginLeft = 3000; 
            //axVSPrinter1.SpaceBefore = 100; 
            
            axVSPrinter1.SpaceBefore = 1000;     // RMC 20150105 mods in permit printing
            
            //this.axVSPrinter1.CurrentY = 5032;  // RMC 20150105 mods in permit printing
            axVSPrinter1.FontSize = 14;
            axVSPrinter1.FontBold = false;
            yGrid = Convert.ToInt32(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = 5032;
            axVSPrinter1.Table = "<10|^8000;|" + m_sBnsAdd;

            axVSPrinter1.MarginLeft = 3000;
            axVSPrinter1.SpaceBefore = 300;
            //axVSPrinter1.SpaceBefore = 0;   // RMC 20150107
            //this.axVSPrinter1.CurrentY = 6368;  // RMC 20150105 mods in permit printing
            //yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
            //yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());    // RMC 20150113 corrections in sanitary, put rem
            this.axVSPrinter1.CurrentY = 6368;
            axVSPrinter1.Table = "<10|^6000;|" + m_sBnsOwn;
            
            //axVSPrinter1.MarginLeft = 5500;
            //axVSPrinter1.SpaceBefore = 750;
            axVSPrinter1.MarginLeft = 5550; // RMC 20150105 mods in permit printing
            axVSPrinter1.SpaceBefore = 900; // RMC 20150105 mods in permit printing
            //this.axVSPrinter1.CurrentY = 7004;  // RMC 20150105 mods in permit printing
            //yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
            axVSPrinter1.Table = "<10|<500;|" + sYear;

            //axVSPrinter1.MarginLeft = 4200;
            axVSPrinter1.MarginLeft = 4250; // RMC 20150105 mods in permit printing
            //axVSPrinter1.SpaceBefore = 330;
            axVSPrinter1.SpaceBefore = 360; // RMC 20150105 mods in permit printing
            //axVSPrinter1.Table = "<10|^500|^3850|^500;|" + sDay + "|" + sMonth + "|" + sYear;
            //this.axVSPrinter1.CurrentY = 8240;  // RMC 20150105 mods in permit printing
            //yGrid = Convert.ToInt32(axVSPrinter1.CurrentY.ToString());
            axVSPrinter1.Table = "<10|^500|^3950|^500;|" + sDay + "|" + sMonth + "|" + sYear;   // RMC 20150105 mods in permit printing

            axVSPrinter1.MarginLeft = 1800;
            axVSPrinter1.SpaceBefore = 2000;
            axVSPrinter1.FontSize = 32;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = "<10|<3000;|" + sYear2;

            axVSPrinter1.MarginLeft = 4800;
            axVSPrinter1.SpaceBefore = 100;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.FontBold = false;
            //axVSPrinter1.Table = "<10|<4000|<1300;| Recommending Approval: | Approved:";
            axVSPrinter1.Table = "<10|<100|^3000|^2800;||Recommending Approval: | Approved:";
            //DJAC-20150102 added o.r fields
            axVSPrinter1.MarginLeft = 1800;
            axVSPrinter1.SpaceBefore = 300;
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. No. _________|" + AppSettingsManager.GetConfigValue("58") + "|" + AppSettingsManager.GetConfigValue("59");
            axVSPrinter1.SpaceBefore = 0;
            axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. Date ________| OIC- SANITATION| Municipal Health Officer";
            //axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. Date. _________|" + AppSettingsManager.GetConfigRemarksValue("58") + "|" + AppSettingsManager.GetConfigObject("59");
            axVSPrinter1.Table = "<10|<2800;|Fee Paid Php______";




        }

        private void SanitaryNew_revised() //JHB 20190703 revise as per LGU Lubao request
        {
          //  this.axVSPrinter1.DrawPicture(Properties.Resources.SANITARY_PERMIT_letter_v3," -1.20in", "-0.0in", "82.0%", "64.0%", 10, false);

            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sYear2 = string.Empty;

            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate());
            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
            sMonth = string.Format("{0:MMMM}", AppSettingsManager.GetCurrentDate());
            sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());


            long yGrid = Convert.ToInt64(axVSPrinter1.CurrentY);
            axVSPrinter1.MarginLeft = 9000;
            this.axVSPrinter1.CurrentY = 2500;
           // axVSPrinter1.SpaceBefore = 750; 
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Table = "<10|<5000;| PERMIT NO." + m_sPermitNo;

            axVSPrinter1.MarginLeft = 500; 
            axVSPrinter1.SpaceBefore = 2100;

            if (m_sBnsName.Length > 50)  
            {
                axVSPrinter1.FontSize = 20; 
            }
            else if (m_sBnsName.Length > 20)   
            {
                axVSPrinter1.FontSize = 25;  
            }
            else
            {
                axVSPrinter1.FontSize = 25;
               
            }

            axVSPrinter1.FontBold = true;

            axVSPrinter1.SpaceBefore = 1800;    
            this.axVSPrinter1.CurrentY = 3400;

            if (m_sReportType == "Sanitary")
            {
            
                string sTmp = AppSettingsManager.GetDTIName(m_sBin).Trim();
                if (sTmp != "")
                    m_sBnsName = sTmp;
                
            }            
             
            axVSPrinter1.Table = "^11000;" + m_sBnsName;
            axVSPrinter1.MarginLeft = 3000; 

            
            axVSPrinter1.SpaceBefore = 1000;     
            //business details
            axVSPrinter1.FontSize = 13;
            axVSPrinter1.FontBold = false;
            yGrid = Convert.ToInt32(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = 5600;

            if (m_sBnsAdd.Length > 100 )
            {
                axVSPrinter1.FontSize = 13;
            }
            else if (m_sBnsAdd.Length > 20)
            {
                axVSPrinter1.FontSize = 13;
            }
            else
            {
                axVSPrinter1.FontSize = 13;

            }


            axVSPrinter1.Table = "<10|^8000;|" + m_sBnsAdd;
            axVSPrinter1.FontSize = 13;
            axVSPrinter1.MarginLeft = 3000;
            axVSPrinter1.SpaceBefore = 300;
            this.axVSPrinter1.CurrentY = 6800;
            axVSPrinter1.Table = "<10|^6000;|" + m_sBnsOwn;
            axVSPrinter1.MarginLeft = 6620; //6620
            axVSPrinter1.SpaceBefore = 1450; //1350
            axVSPrinter1.Table = "<10|<600;|" + sYear + " ,";

         
            axVSPrinter1.MarginLeft = 4950; 
            axVSPrinter1.SpaceBefore = 700;//670
            axVSPrinter1.Table = "<10|>500|>2000|>900|<800;|" + sDay + "|" + sMonth + "||" + sYear2;   


            axVSPrinter1.MarginLeft = 2700;
            this.axVSPrinter1.CurrentY = 10500;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.FontBold = false;

            axVSPrinter1.Table = "<10|<100|<3000|^1000|^1000|<2800;||Recommending Approval: ||| Approved:";
            axVSPrinter1.SpaceBefore = 500;
            axVSPrinter1.Table = "<10|<100|<3000|^500|^500|<4000;||" + AppSettingsManager.GetConfigValue("58") + "|||" + AppSettingsManager.GetConfigValue("59");
            axVSPrinter1.SpaceBefore = 0;
            axVSPrinter1.Table = "<10|<100|<3000|^500|^500|<4000;||" + AppSettingsManager.GetConfigValueRemarks("58") + "|||" + AppSettingsManager.GetConfigValueRemarks("59") + " ";

            //TAX YEAR
            axVSPrinter1.MarginLeft = 500;
            this.axVSPrinter1.CurrentY = 12800;
            axVSPrinter1.FontSize = 40;
            axVSPrinter1.FontBold = true;
            this.axVSPrinter1.TextColor = System.Drawing.Color.White;
            this.axVSPrinter1.DrawPicture(Properties.Resources.blue, "0.30in", "8.90in", "70%", "100%", 11, false); //JHB 20190801
            axVSPrinter1.Table = "<2000;" + sYear2;
           
            this.axVSPrinter1.TextColor = System.Drawing.Color.Black;
           
            //axVSPrinter1.MarginLeft = 1800;
            //axVSPrinter1.SpaceBefore = 300;
            //axVSPrinter1.FontSize = 12;
            //axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. No. _________|" + AppSettingsManager.GetConfigValue("58") + "|" + AppSettingsManager.GetConfigValue("59");
            //axVSPrinter1.SpaceBefore = 0;
            //axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. Date ________| OIC- SANITATION| Municipal Health Officer";
            ////axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. Date. _________|" + AppSettingsManager.GetConfigRemarksValue("58") + "|" + AppSettingsManager.GetConfigObject("59");
            //axVSPrinter1.Table = "<10|<2800;|Fee Paid Php______";
            //JHB 20210120 add date time (s)


            this.axVSPrinter1.CurrentY = 14700;
            axVSPrinter1.MarginBottom = 100;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.FontSize = 7.0f;
            this.axVSPrinter1.Table = "<2500;Printed Date: " + AppSettingsManager.GetCurrentDate().ToString("yyyy-MM-dd hh:mm:ss");

            string m_Stat = "";
            m_dTransLogIn = DateTime.Parse(m_timeIN);
            m_Stat = AppSettingsManager.GetBnsStat(m_sBin);
            //JHB 20210222 add time, insert to translog as per sir juli request (s)
            TransLog.UpdateLog(m_sBin, m_Stat, ConfigurationAttributes.CurrentYear, "Sanitary", m_dTransLogIn, AppSettingsManager.GetSystemDate());
            //JHB 20210222 add time, insert to translog as per sir juli request (e)


        }

        private void  Barangay_Certificate_new() //JHB 20191012 add for brgy clearance (s)
        {
           this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
           this.axVSPrinter1.DrawPicture(Properties.Resources.Lubaologo, "0.5in", "0.55in", "20%", "20%", 10, false); //JHB 20200124  municipal  logo
         //  this.axVSPrinter1.DrawPicture(Properties.Resources.balantacan, "7.0in", "0.55in", "20%", "20%", 10, false); //JHB 20200124 barangay logo
           this.axVSPrinter1.DrawPicture(Properties.Resources.lineTOP, "0.2in", "1.65in", "45%", "50%", 10, false); //JHB 20200124 topline
           this.axVSPrinter1.DrawPicture(Properties.Resources.lineDIAGONAL1, "2.5in", "1.74in", "200%", "69.0%", 10, false); //JHB 20200124 diagonal
           this.axVSPrinter1.DrawPicture(Properties.Resources.lineBOTTOM, "0.2in", "9.80in", "45%", "50%", 10, false); //JHB 20200124 bottom


            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet(); 
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sYear2 = string.Empty;
            string sBrgy = string.Empty;
            string sDate = string.Empty;
            string sStat = AppSettingsManager.GetBnsStat(m_sBin);
            string sMaritalStat =  AppSettingsManager.GetMaritalStat(m_sBin);
            string sTempBin = m_sTempBIN;
            string sOwnCode = AppSettingsManager.GetOwnCode(m_sBin);
            string sUserNm = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            DateTime sDateSave = AppSettingsManager.GetSystemDate();
           
            //Baranagy Officials
            string m_strBrgyCode = string.Empty;
            string m_strCapt = string.Empty; 
            string m_strTreas = string.Empty;
            string m_strSec = string.Empty;
            string m_strKwd1 = string.Empty;
            string m_strKwd2 = string.Empty;
            string m_strKwd3 = string.Empty;
            string m_strKwd4 = string.Empty;
            string m_strKwd5 = string.Empty;
            string m_strKwd6 = string.Empty;
            string m_strKwd7 = string.Empty;
            string m_strSk = string.Empty;
            //Baranagy Officials

            string DATE_CREATED = string.Format("{0:MM/dd/yyyy}", sDateSave);
            string strProvinceName = string.Empty;

            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate());
            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
            sMonth = string.Format("{0:MMMM}", AppSettingsManager.GetCurrentDate());
            sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());
            
            sBrgy = AppSettingsManager.GetBnsBrgy(m_sBin);
            sDate = sMonth + " " + sDay + ", " + sYear2;

            //header
          //  long yGrid = Convert.ToInt64(axVSPrinter1.CurrentY);
            this.axVSPrinter1.CurrentY = 900;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
           // this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Table = "^16000;Republic of the Philippines";
            this.axVSPrinter1.FontBold = false;

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValueRemarks("08") +" "+ strProvinceName);
            this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("09"));
            this.axVSPrinter1.Table = string.Format("^16000;{0}", AppSettingsManager.GetConfigValue("41"));

            //Barangay
            this.axVSPrinter1.CurrentY = 2000;
            this.axVSPrinter1.FontSize = (float)13.0;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = string.Format("^16000;BARANGAY {0}", sBrgy);
          
            //BARANGAY HEADER 
            this.axVSPrinter1.MarginLeft = 4000;
            this.axVSPrinter1.CurrentY = 2000;
            this.axVSPrinter1.SpaceBefore = 750;
            this.axVSPrinter1.FontSize = 18;
            this.axVSPrinter1.Table = "<10|^5000;|BARANGAY CLEARANCE";
            this.axVSPrinter1.FontBold = false;
            

            //DATE
            axVSPrinter1.MarginLeft = 4200;
            this.axVSPrinter1.CurrentY = 2900;
            axVSPrinter1.SpaceBefore = 750;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Table = "<10|>7500;|" + sDate;


            //BARANGAY OFFICIAL (s)
            result2.Query = "Select * from brgy where brgy_nm = '" + sBrgy + "' ";
            if (result2.Execute())
            {
                while (result2.Read())
                {
                    m_strBrgyCode = result2.GetString("BRGY_CODE");
                    m_strCapt = result2.GetString("BRGY_CAPT");
                    m_strTreas = result2.GetString("BRGY_TREAS");
                    m_strSec = result2.GetString("BRGY_SEC");
                    m_strKwd1 = result2.GetString("BRGY_KAGAWAD1");
                    m_strKwd2 = result2.GetString("BRGY_KAGAWAD2");
                    m_strKwd3 = result2.GetString("BRGY_KAGAWAD3");
                    m_strKwd4 = result2.GetString("BRGY_KAGAWAD4");
                    m_strKwd5 = result2.GetString("BRGY_KAGAWAD5");
                    m_strKwd6 = result2.GetString("BRGY_KAGAWAD6");
                    m_strKwd7 = result2.GetString("BRGY_KAGAWAD7");
                    m_strSk = result2.GetString("SK_CHAIR");
                }
            }
            result2.Close();
            //BARANGAY OFFICIAL (e)

            //001	BALANTACAN
            if (m_strBrgyCode == "001")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.BALANTACAN_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eBALANTACAN, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //002	BANCAL PUGAD
            if (m_strBrgyCode == "002")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.BANGCAL_PUGAD_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eBANCAL_PUGAD, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //003	BANCAL SINUBLI
            if (m_strBrgyCode == "003")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.BANGCAL_SINUBLI_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eBANCAL_SINUBALI, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //004	BARUYA
            if (m_strBrgyCode == "004")
            {
                 this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_RAFAEL_BARUYA_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false); 
                 this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_RAFAEL_BARUYA, "5.8in", "7.00in", "20%", "20%", 10, false); 
            }
            //005	CALANGAIN
            if (m_strBrgyCode == "005")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.CALANGAIN, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eCALANGAIN, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //006	CONCEPCION
            if (m_strBrgyCode == "006")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.CONCEPCION, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eCONCEPCION, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //007	DELA PAZ
            if (m_strBrgyCode == "007")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.DELA_PAZ, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eDELA_PAZ, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //008	DEL CARMEN
            if (m_strBrgyCode == "008")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.DEL_CARMEN, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eDEL_CARMEN, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //009	LOURDES
            if (m_strBrgyCode == "009")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.LOURDES, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eLOURDES, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //010	PRADO SIONGCO
            if (m_strBrgyCode == "010")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.PRADO_SIONGCO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.ePRADO_SIONGCO, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //011	REMEDIOS
            if (m_strBrgyCode == "011")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.REMEDIOS, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eREMEDIOS, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //012	SANTIAGO
            if (m_strBrgyCode == "012")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SANTIAGO_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSANTIAGO, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //013	SAN AGUSTIN
            if (m_strBrgyCode == "013")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_AGUSTIN, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_AGUSTIN, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //014	SAN ANTONIO
            if (m_strBrgyCode == "014")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_ANTONIO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_ANTONIO, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //015	SAN FRANCISCO
            if (m_strBrgyCode == "015")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.san_francisco_new_logo, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_FRANCISCO, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //016	SAN ISIDRO
            if (m_strBrgyCode == "016")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_ISIDRO_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_SAN_ISIDRO, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //017	SAN JUAN
            if (m_strBrgyCode == "017")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_JUAN_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_JUAN, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //018	SAN JOSE APUNAN
            if (m_strBrgyCode == "018")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_JOSE_APUNAN, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_JOSE_APUNAN, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //019	SAN JOSE GUMI
            if (m_strBrgyCode == "019")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.san_jose_gumi_logo, "7.0in", "0.55in", "3.5%", "3.5%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_JOSE_GUMI, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //020	SAN MATIAS
            if (m_strBrgyCode == "020")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.san_matias_LOGO, "7.0in", "0.55in", "15%", "15%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_MATIAS, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //021	SAN MIGUEL
            if (m_strBrgyCode == "021")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_MIGUEL_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_MIGUEL, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //022	SAN NICOLAS 1.0
            if (m_strBrgyCode == "022")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_NICOLAS1_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_NICOLAS1, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //023	SAN NICOLAS 2.0
            if (m_strBrgyCode == "023")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_NICOLAS2_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_NICOLAS2, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //024	SAN PABLO 1.0
            if (m_strBrgyCode == "024")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_PABLO_1_LOGO, "7.0in", "0.55in", "14%", "14%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_PABLO1, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //025	SAN PABLO 2.0
            if (m_strBrgyCode == "025")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_PABLO_2_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_PABLO2, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //026	SAN PEDRO PALCARANGAN
            if (m_strBrgyCode == "026")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_PEDRO_PALCARANGAN_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_PERDO_PALCARANGAN, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //027	SAN PEDRO SAUG
            if (m_strBrgyCode == "027")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_PEDRO_SAUG, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_PEDRO_SAUG, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //028	SAN ROQUE ARBOL
            if (m_strBrgyCode == "028")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_ROQUE_ARBOL, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_ROQUE_ARBOL, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //029	SAN ROQUE DAU 1.0
            if (m_strBrgyCode == "029")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_ROQUE_DAU_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_ROQUE_DAU, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //030	SAN ROQUE DAU 2.0
            if (m_strBrgyCode == "030")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.DON_IGNACIO_DIMSON, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eDON_IGNACIO_DIMSON_DAU_2, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //031	SAN VICENTE
            if (m_strBrgyCode == "031")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.SAN_VICENTE_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSAN_VICENTE, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //032	STA BARBARA
            if (m_strBrgyCode == "032")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STA_BARBARA_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTA_BARBARA, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //033	STA CATALINA
            if (m_strBrgyCode == "033")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STA_CATALINA_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTA_CATALINA, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //034	STA CRUZ
            if (m_strBrgyCode == "034")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STA_CRUZ_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTA_CRUZ, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //035	STA LUCIA
            if (m_strBrgyCode == "035")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STA_LUCIA_LOGO, "7.0in", "0.55in", "13.2%", "13.2%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTA_LUCIA, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //036	STA MARIA
            if (m_strBrgyCode == "036")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STA_MARIA_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTA_MARIA, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //037	STA MONICA
            if (m_strBrgyCode == "037")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STA_MONICA_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTA_MONICA, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //038	STA RITA
            if (m_strBrgyCode == "038")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STA_RITA_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTA_RITA, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //039	STA TEREZA 1.0
            if (m_strBrgyCode == "039")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STA_TERESA_1ST_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTA_TERESA_01, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //040	STA TEREZA 2.0
            if (m_strBrgyCode == "040")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STA_TEREZA_2_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTA_TERESA_02, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //041	STO. CRISTO
            if (m_strBrgyCode == "041")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STO_CRISTO_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTO_CRISTO, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //042	STO. DOMINGO
            if (m_strBrgyCode == "042")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STO_DOMINGO_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTO_DOMINGO, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //043	STO. NIÑO
            if (m_strBrgyCode == "043")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STO_NINO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTO_NINO, "5.8in", "7.00in", "20%", "20%", 10, false);
            }
            //044	STO. TOMAS
            if (m_strBrgyCode == "044")
            {
                this.axVSPrinter1.DrawPicture(Properties.Resources.STO_TOMAS_LOGO, "7.0in", "0.55in", "20%", "20%", 10, false);
                this.axVSPrinter1.DrawPicture(Properties.Resources.eSTO_TOMAS, "5.8in", "7.00in", "20%", "20%", 10, false);
            }

            //BARANGAY OFFICIAL HEADER(s)
            this.axVSPrinter1.MarginRight = 12900;
            this.axVSPrinter1.FontItalic = true;
            this.axVSPrinter1.TextColor = Color.DarkBlue;
            this.axVSPrinter1.FontSize = 9;
            this.axVSPrinter1.CurrentY = 2000; //2000
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strCapt;

            this.axVSPrinter1.CurrentY = 3050; //2900
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd1;

            this.axVSPrinter1.CurrentY = 4100; //3800
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd2;

            this.axVSPrinter1.CurrentY = 5150; //4700
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd3;

            this.axVSPrinter1.CurrentY = 6200; //5600
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd4;

            this.axVSPrinter1.CurrentY = 7250; //6500
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd5;

            this.axVSPrinter1.CurrentY = 8300; //7400
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd6;

            this.axVSPrinter1.CurrentY = 9350; //8300
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strKwd7;

            this.axVSPrinter1.CurrentY = 10400; //9200
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strSk;

            this.axVSPrinter1.CurrentY = 11450; //10100
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strTreas;

            this.axVSPrinter1.CurrentY = 12500; //11000
            this.axVSPrinter1.Table = "<500|<3000;|" + m_strSec;

            this.axVSPrinter1.FontItalic = false;
          
            this.axVSPrinter1.TextColor = Color.Black;

            this.axVSPrinter1.CurrentY = 2300; //2300
            this.axVSPrinter1.Table = "<500|<3000;|Punong Barangay";

            this.axVSPrinter1.CurrentY = 3350; //3200
            this.axVSPrinter1.Table = "<500|<3000;|Kagawad";
            
            this.axVSPrinter1.CurrentY = 4400; //4100
            this.axVSPrinter1.Table = "<500|<3000;|Kagawad";

            this.axVSPrinter1.CurrentY = 5450; //5000
            this.axVSPrinter1.Table = "<500|<3000;|Kagawad";

            this.axVSPrinter1.CurrentY = 6500; //5900
            this.axVSPrinter1.Table = "<500|<3000;|Kagawad";

            this.axVSPrinter1.CurrentY = 7550; //6800
            this.axVSPrinter1.Table = "<500|<3000;|Kagawad";

            this.axVSPrinter1.CurrentY = 8600; //7700
            this.axVSPrinter1.Table = "<500|<3000;|Kagawad";

            this.axVSPrinter1.CurrentY = 9650; //8600
            this.axVSPrinter1.Table = "<500|<3000;|Kagawad";

            this.axVSPrinter1.CurrentY = 10700; //9500
            this.axVSPrinter1.Table = "<500|<3000;|SK Chairman";

            this.axVSPrinter1.CurrentY = 11750; //10400
            this.axVSPrinter1.Table = "<500|<3000;|Barangay Treasurer";

            this.axVSPrinter1.CurrentY = 12800; //11300 
            this.axVSPrinter1.Table = "<500|<3000;|Barangay Secretary";
            //BARANGAY OFFICIAL HEADER(e)

            this.axVSPrinter1.MarginRight = 0;
           

            //OWNER (S)
            this.axVSPrinter1.CurrentY = 5000;
            this.axVSPrinter1.MarginLeft = 6000;
            this.axVSPrinter1.SpaceBefore = 300;
            this.axVSPrinter1.FontBold = true;

            if (m_sBnsOwn.Length > 100)
            {
                axVSPrinter1.FontSize = 8;
            }
            else if (m_sBnsOwn.Length > 50)
            {
                axVSPrinter1.FontSize = 9;
            }
            else
            {
                axVSPrinter1.FontSize = 12;
            }
            axVSPrinter1.Table = "<10|^5000;|" + m_sBnsOwn;
            //OWNER (E)


           //MARITAL STATUS (S)
           this.axVSPrinter1.CurrentY = 5600;
           this.axVSPrinter1.MarginLeft = 20;
           this.axVSPrinter1.FontSize = 11;
           this.axVSPrinter1.Table = "^1500;" + sMaritalStat + " ";

           //Punong Barangay signature
           this.axVSPrinter1.MarginLeft = 600;
           this.axVSPrinter1.CurrentY = 10300;
         //  this.axVSPrinter1.DrawPicture(Properties.Resources.LOURDES_eSIGNATURE, "5.8in", "7.00in", "20%", "20%", 10, false); //JHB 20200124 barangay logo
           this.axVSPrinter1.Table = "<500|^5100|^6000;|| " + m_strCapt;
          


            //OWNER'S ADDRESS  (S)
            this.axVSPrinter1.CurrentY = 6200;
            this.axVSPrinter1.MarginLeft = 500; //3800
           
            if (m_sBnsAdd.Length > 100)
            {
                this.axVSPrinter1.FontSize = 7;
            }
            else if (m_sBnsAdd.Length > 35)
            {
                this.axVSPrinter1.FontSize = 9;
            }
            else
            {
                this.axVSPrinter1.FontSize = 12;
            }
            this.axVSPrinter1.Table = "<10|<5000;|" + m_sBnsAdd; 
            //OWNER'S ADDRESS (E)

            //BUSINESS NAME  (S)
            string sBnsName = m_sBnsName.Trim();
           // string  sBnsName = StringUtilities.HandleApostrophe(m_sBnsName).Trim();
           // string sBnsName = StringUtilities.RemoveApostrophe(m_sBnsName).Trim(); 
            this.axVSPrinter1.CurrentY = 6800;
            this.axVSPrinter1.MarginLeft = 500;//3800
            if (sBnsName.Length > 100)
            {
                axVSPrinter1.FontSize = 7;
            }
            else if (sBnsName.Length > 35)
            {
                axVSPrinter1.FontSize = 8;
            }
            else
            {
                axVSPrinter1.FontSize = 10;
            }

          
              axVSPrinter1.Table = "<10|<5000;|" + sBnsName;

              this.axVSPrinter1.FontBold = false;
            //BUSINESS NAME (E)

            //PARAGRAPH (s)
              this.axVSPrinter1.MarginLeft = 1000;
              this.axVSPrinter1.CurrentY = 3690;
              this.axVSPrinter1.FontSize = 12;
              this.axVSPrinter1.Table = "<10|>7800;| Date ";
              this.axVSPrinter1.DrawLine(9000, 4000, 11500, 4000);

           

              this.axVSPrinter1.MarginLeft = 500;
              this.axVSPrinter1.CurrentY = 4200;
              this.axVSPrinter1.Table = "<10|<5000;|To Whom It may Concern: ";

              this.axVSPrinter1.MarginLeft = 600;
              this.axVSPrinter1.CurrentY = 5000;
              this.axVSPrinter1.Table = ">100|<5000;|This is to certify that "; //NAME OF OWNER
              this.axVSPrinter1.DrawLine(6000, 5600, 11500, 5600);

              this.axVSPrinter1.MarginLeft = 3500; //MARITAL STATUS
              this.axVSPrinter1.CurrentY = 5600;
              this.axVSPrinter1.Table = "<10|<8000;|of legal age,                              Filipino and resident of the address below";
              this.axVSPrinter1.CurrentY = 5600;
              this.axVSPrinter1.DrawLine(5500, 6200, 7000, 6200);

              this.axVSPrinter1.MarginLeft = 500; // ADDRESS
              this.axVSPrinter1.CurrentY = 6200;
              this.axVSPrinter1.Table = "<10|>10000;|is engaged to operate a";
              this.axVSPrinter1.DrawLine(3900, 6800, 9000, 6800);

              this.axVSPrinter1.MarginLeft = 500; // BUSINESS NAME
              this.axVSPrinter1.CurrentY = 6800;
              this.axVSPrinter1.Table = "<10|>9000;| in this Barangay.";
              this.axVSPrinter1.DrawLine(3900, 7400, 9000, 7400);

              this.axVSPrinter1.MarginLeft = 4600;
              this.axVSPrinter1.CurrentY = 7400;
              this.axVSPrinter1.Table = ">100|<9000;|  Issued in pursuance to SEC. 152 (C) Article Four, Chapter II Title I, ";
              this.axVSPrinter1.MarginLeft = 4600;
              this.axVSPrinter1.CurrentY = 8000;
              this.axVSPrinter1.Table = ">10|<9000;|Book II of the Local GOvernment Code of 1991, requiring a Barangay ";
              this.axVSPrinter1.MarginLeft = 4600;
              this.axVSPrinter1.CurrentY = 8600;
              this.axVSPrinter1.Table = ">10|<9000;|Business Clearance before the issuance of any license of permit for ";
              this.axVSPrinter1.MarginLeft = 4600;
              this.axVSPrinter1.CurrentY = 9200;
              this.axVSPrinter1.Table = ">10|<9000;|business or activity to be conducted in this Barangay. ";

              this.axVSPrinter1.MarginLeft = 600;
              this.axVSPrinter1.CurrentY = 10800;
              this.axVSPrinter1.Table = "<10|>8000;|Punong Barangay"; //Punong Baranagy
              this.axVSPrinter1.DrawLine(7500, 11000, 11000, 11000);

              this.axVSPrinter1.MarginLeft = 3500;
              this.axVSPrinter1.CurrentY = 12000;
              this.axVSPrinter1.Table = "<10|<8000;|Comm. Tax Cert. NO.";
              this.axVSPrinter1.DrawLine(6500, 12550, 8500, 12550);

              this.axVSPrinter1.MarginLeft = 3500;
              this.axVSPrinter1.CurrentY = 12400;
              this.axVSPrinter1.Table = "<10|<8000;|Issued on: ";
              this.axVSPrinter1.DrawLine(5400, 12950, 7000, 12950);

              this.axVSPrinter1.MarginLeft = 3500;
              this.axVSPrinter1.CurrentY = 12800;
              this.axVSPrinter1.Table = "<10|<8000;|Issued at: ";
              this.axVSPrinter1.DrawLine(5400, 13350, 7000, 13350);
            //PARAGRAPH (e)


            //OR DETAILS (S)
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 11;
            this.axVSPrinter1.CurrentY = 12000; //12000;
            this.axVSPrinter1.MarginLeft = 6300;
            this.axVSPrinter1.Table = "<10|<5000;| " + m_sORNo;
            this.axVSPrinter1.CurrentY = 12400; //12400;
            this.axVSPrinter1.MarginLeft = 3800;
            this.axVSPrinter1.Table = "<10|<5000;|" + m_sORDate;
            this.axVSPrinter1.CurrentY = 12800; //12900;
            this.axVSPrinter1.MarginLeft = 3800;
            this.axVSPrinter1.Table = "<10|<5000;|" + m_sIssuedOn;
            //axVSPrinter1.CurrentY = 12900;
            //axVSPrinter1.MarginLeft = 5200;
            //axVSPrinter1.Table = "<10|<5000;|O.R. Date: " + m_sFeeAmount;

            axVSPrinter1.FontBold = false;
            CheckifBrgyCleanExist(m_sBin);

            if (brgy_reprint == false)
            {

                result.Query = "Insert into BRGY_CLEARANCE(BIN, TAX_YEAR, TEMP_BIN, BNS_NM, BNS_STAT, OWN_CODE, OWN_NM, MARITAL_STAT, ";
                result.Query += "BNS_BRGY, CTC_NO, CTC_ISSUED_ON, CTC_ISSUED_AT, CTC_AMT, DATE_CREATED, BNS_USER) ";
                result.Query += "values('" + m_sBin + "', '" + sYear2 + "', '" + sTempBin + "', '" + StringUtilities.HandleApostrophe(sBnsName) + "', '" + sStat + "', '" + sOwnCode + "', '" + StringUtilities.HandleApostrophe(m_sBnsOwn) + "', '" + sMaritalStat + "', ";
                result.Query += "'" + sBrgy + "', '" + m_sORNo + "', '" + m_sIssuedOn + "', '" + m_sORDate + "', '" + m_sFeeAmount + "', '" + DATE_CREATED + "', '" + sUserNm + "')";
            }
            else
            {
                result.Query = "UPDATE BRGY_CLEARANCE SET CTC_NO = '" + m_sORNo + "',CTC_ISSUED_ON = '" + m_sIssuedOn + "', MARITAL_STAT = '" + sMaritalStat + "',BNS_BRGY = '" + sBrgy + "',OWN_CODE = '" + sOwnCode + "',BNS_STAT= '" + sStat + "', "; 
                result.Query += "CTC_ISSUED_AT = '" + m_sORDate + "', CTC_AMT = '" + m_sFeeAmount + "' WHERE BIN = '" + m_sBin + "' and tax_year = '" + sYear2 + "' ";
            
            }

            result.ExecuteNonQuery();
            if (!result.Commit())
            {
                result.Rollback();
            }
            result.Close();

            //FOOTER
            this.axVSPrinter1.MarginLeft = 3000;
            this.axVSPrinter1.HdrFontName = "Times New Roman";
            this.axVSPrinter1.HdrFontSize = 10.0f;
            this.axVSPrinter1.HdrFontItalic = true;
            this.axVSPrinter1.Footer = " NOTE: Not valid without the Barangay Dry Seal and with erasures. ";

            // RMC 20170822 added transaction log feature for tracking of transactions per bin (s)
            if (AuditTrail.InsertTrail("ABBC", "Barangay Clearance", "Print Barangay Clearance BIN: " + m_sBin + " for tax year " + m_sTaxYear) == 0)
            {
                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // RMC 20170822 added transaction log feature for tracking of transactions per bin (e)

        } //JHB 20191012 add for brgy clearance (e)

        private void Barangay_Certificate() //JHB 20191012 add for brgy clearance  clearance old(s)(s)
        {
            //   this.axVSPrinter1.DrawPicture(Properties.Resources.Untitled_1," 0.10in", "-2.5in", "33.0%", "46.8%", 10, false);
            OracleResultSet result = new OracleResultSet();
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sYear2 = string.Empty;
            string sBrgy = string.Empty;
            string sDate = string.Empty;
            string sStat = AppSettingsManager.GetBnsStat(m_sBin);
            string sMaritalStat = AppSettingsManager.GetMaritalStat(m_sBin);
            string sTempBin = m_sTempBIN;
            string sOwnCode = AppSettingsManager.GetOwnCode(m_sBin);
            string sUserNm = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            DateTime sDateSave = AppSettingsManager.GetSystemDate();
            string DATE_CREATED = string.Format("{0:MM/dd/yyyy}", sDateSave);


            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate());
            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
            sMonth = string.Format("{0:MMMM}", AppSettingsManager.GetCurrentDate());
            sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());

            sBrgy = AppSettingsManager.GetBnsBrgy(m_sBin);


            sDate = sMonth + " " + sDay + ", " + sYear2;
            long yGrid = Convert.ToInt64(axVSPrinter1.CurrentY);


            //DATE
            axVSPrinter1.MarginLeft = 3800;
            this.axVSPrinter1.CurrentY = 3000;
            axVSPrinter1.SpaceBefore = 750;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Table = "<10|>7500;|" + sDate;

            //OWNER (S)
            this.axVSPrinter1.CurrentY = 5000;
            axVSPrinter1.MarginLeft = 6000;
            axVSPrinter1.SpaceBefore = 300;

            if (m_sBnsOwn.Length > 100)
            {
                axVSPrinter1.FontSize = 8;
            }
            else if (m_sBnsOwn.Length > 50)
            {
                axVSPrinter1.FontSize = 9;
            }
            else
            {
                axVSPrinter1.FontSize = 12;
            }
            axVSPrinter1.Table = "<10|^5000;|" + m_sBnsOwn;
            //OWNER (E)


            //MARITAL STATUS (S)
            this.axVSPrinter1.CurrentY = 5600;
            axVSPrinter1.MarginLeft = 3400;
            axVSPrinter1.FontSize = 11;
            axVSPrinter1.Table = "<10|^5000;|" + sMaritalStat + ".";


            //OWNER'S ADDRESS  (S)
            this.axVSPrinter1.CurrentY = 6300;
            axVSPrinter1.MarginLeft = 3800;

            if (m_sBnsAdd.Length > 100)
            {
                axVSPrinter1.FontSize = 7;
            }
            else if (m_sBnsAdd.Length > 35)
            {
                axVSPrinter1.FontSize = 9;
            }
            else
            {
                axVSPrinter1.FontSize = 12;
            }
            axVSPrinter1.Table = "<10|<5000;|" + m_sBnsAdd;
            //OWNER'S ADDRESS (E)

            //BUSINESS NAME  (S)
            string sBnsName = m_sBnsName;
            //   string sBnsName = StringUtilities.HandleApostrophe(m_sBnsName).Trim();
            this.axVSPrinter1.CurrentY = 7000;
            axVSPrinter1.MarginLeft = 3800;
            if (sBnsName.Length > 100)
            {
                axVSPrinter1.FontSize = 7;
            }
            else if (sBnsName.Length > 35)
            {
                axVSPrinter1.FontSize = 8;
            }
            else
            {
                axVSPrinter1.FontSize = 10;
            }

            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = "<10|<5000;|" + sBnsName;
            //BUSINESS NAME (E)


            //OR DETAILS (S)

            axVSPrinter1.FontSize = 12;
            axVSPrinter1.CurrentY = 11800; //12000;//11700
            axVSPrinter1.MarginLeft = 6300;
            axVSPrinter1.Table = "<10|<5000;| " + m_sORNo;
            axVSPrinter1.CurrentY = 12280; //12400;//12200
            axVSPrinter1.MarginLeft = 5200;
            axVSPrinter1.Table = "<10|<5000;|" + m_sORDate;
            axVSPrinter1.CurrentY = 12680; //12900;//12700
            axVSPrinter1.MarginLeft = 5200;
            axVSPrinter1.Table = "<10|<5000;|" + m_sIssuedOn;
            //axVSPrinter1.CurrentY = 12900;
            //axVSPrinter1.MarginLeft = 5200;
            //axVSPrinter1.Table = "<10|<5000;|O.R. Date: " + m_sFeeAmount;
            CheckifBrgyCleanExist(m_sBin);

            if (brgy_reprint == false)
            {

                result.Query = "Insert into BRGY_CLEARANCE(BIN, TAX_YEAR, TEMP_BIN, BNS_NM, BNS_STAT, OWN_CODE, OWN_NM, MARITAL_STAT, ";
                result.Query += "BNS_BRGY, CTC_NO, CTC_ISSUED_ON, CTC_ISSUED_AT, CTC_AMT, DATE_CREATED, BNS_USER) ";
                result.Query += "values('" + m_sBin + "', '" + sYear2 + "', '" + sTempBin + "', '" + StringUtilities.HandleApostrophe(sBnsName).Trim() + "', '" + sStat + "', '" + sOwnCode + "', '" + StringUtilities.HandleApostrophe(m_sBnsOwn).Trim() + "', '" + sMaritalStat + "', ";
                result.Query += "'" + sBrgy + "', '" + m_sORNo + "', '" + m_sIssuedOn + "', '" + m_sORDate + "', '" + m_sFeeAmount + "', '" + DATE_CREATED + "', '" + sUserNm + "')";
            }
            else
            {
                result.Query = "UPDATE BRGY_CLEARANCE SET CTC_NO = '" + m_sORNo + "',CTC_ISSUED_ON = '" + m_sIssuedOn + "', MARITAL_STAT = '" + sMaritalStat + "',BNS_BRGY = '" + sBrgy + "',OWN_CODE = '" + sOwnCode + "',BNS_STAT= '" + sStat + "', ";
                result.Query += "CTC_ISSUED_AT = '" + m_sORDate + "', CTC_AMT = '" + m_sFeeAmount + "' WHERE BIN = '" + m_sBin + "' and tax_year = '" + sYear2 + "' ";

            }

            result.ExecuteNonQuery();
            if (!result.Commit())
            {
                result.Rollback();
            }
            result.Close();


            //axVSPrinter1.Table = "<10|<2000|<2000|O.R. No.: " + m_sORNo;
            //axVSPrinter1.Table = "<10|<2000|<2000|O.R. Date: " + m_sORDate;
            //axVSPrinter1.Table = "<10|<2000|<2000|O.R. Amount: " + m_sFeeAmount;
            //axVSPrinter1.SpaceBefore = 0;
            //axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. Date ________| OIC- SANITATION| Municipal Health Officer";
            ////axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. Date. _________|" + AppSettingsManager.GetConfigRemarksValue("58") + "|" + AppSettingsManager.GetConfigObject("59");
            //axVSPrinter1.Table = "<10|<2800;|Fee Paid Php______";

        } //JHB 20191012 add for brgy clearance old(s)



        private void Sanitary()
        {
            OracleResultSet result = new OracleResultSet();

            string sLocation = string.Empty;
            string sBNSName = string.Empty;
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sBIN = string.Empty;
            string sORNo = string.Empty;
            string sORDate = string.Empty;
            string sFeeAmount = string.Empty;

            result.Query = "Select * from businesses where bin ='" + m_sBin + "'";
            
            if (result.Execute())
            {
                while (result.Read())
                {
                    try
                    {
                        sYear = Convert.ToDateTime(m_sIssuedDate).Year.ToString();
                        sMonth = Convert.ToDateTime(m_sIssuedDate).ToString("MMMM");
                        sDay = Convert.ToDateTime(m_sIssuedDate).Day.ToString();
                        sLocation = result.GetString("bns_house_no")
                            + " " + result.GetString("bns_street")
                            + " " + result.GetString("bns_brgy")
                            + " " + result.GetString("bns_dist")
                            + " " + result.GetString("bns_zone")
                            + " " + result.GetString("bns_mun")
                            + " " + result.GetString("bns_zip")
                            + " " + result.GetString("bns_prov");
                        sBNSName = result.GetString("bns_nm");
                        sBIN = m_sBin;
                        sORNo = m_sORNo;
                        sORDate = m_sORDate;
                        sFeeAmount = m_sFeeAmount;

                        axVSPrinter1.FontSize = 7;
                        axVSPrinter1.MarginLeft = 4500;
                        axVSPrinter1.Table = "<10|<1600;| PERMIT NO." + m_sPermitNo;
                        axVSPrinter1.MarginLeft = 500;
                        axVSPrinter1.SpaceBefore = 1000;
                        axVSPrinter1.FontSize = 16;
                        axVSPrinter1.FontBold = true;
                        
                        axVSPrinter1.Table = "<10|^5500;|" + sBNSName;
                        axVSPrinter1.MarginLeft = 460;
                        axVSPrinter1.SpaceBefore = 500;
                        axVSPrinter1.FontSize = 7;
                        axVSPrinter1.FontBold = false;
                        
                        axVSPrinter1.Table = "<10|<700|<4700;|" + "located at|" + sLocation;
                        axVSPrinter1.SpaceBefore = 0;
                        axVSPrinter1.MarginLeft = 1170;
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                        axVSPrinter1.Table = "<4600; ";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                        axVSPrinter1.MarginLeft = 330;
                        axVSPrinter1.Table = "<10|<800|<3500|<2500;|" + "operated by|" + sBNSName + "|is hereby granted a";
                        axVSPrinter1.SpaceBefore = 0;
                        axVSPrinter1.MarginLeft = 1140;
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                        axVSPrinter1.Table = "<3500; ";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                        axVSPrinter1.MarginLeft = 2300;
                        axVSPrinter1.Table = "<10|<1600;| SANITARY PERMIT";
                        axVSPrinter1.MarginLeft = 330;
                        axVSPrinter1.SpaceBefore = 180;
                        axVSPrinter1.Table = "<10|^5200;|" + "This expires on the  31st of  December " + sYear + " unless sooner revoked when the interest of the public so requires.";
                        axVSPrinter1.MarginLeft = 1400;
                        axVSPrinter1.SpaceBefore = 20;
                        
                        axVSPrinter1.Table = "<10|^3000;|" + "Issued this  " + sDay + "  day of  " + sMonth + ", " + sYear + ".";
                        axVSPrinter1.MarginLeft = 700;
                        axVSPrinter1.SpaceBefore = 1150;
                        axVSPrinter1.FontSize = 16;
                        axVSPrinter1.FontBold = true;
                        
                        axVSPrinter1.Table = "<10|<5500;|" + sYear;
                        axVSPrinter1.MarginLeft = 2800;
                        axVSPrinter1.SpaceBefore = 100;
                        axVSPrinter1.FontSize = 6;
                        axVSPrinter1.FontBold = false;
                        axVSPrinter1.Table = "<10|<1800|<1600;| Recommending Approval: | Approved:";
                        axVSPrinter1.MarginLeft = 460;
                        axVSPrinter1.SpaceBefore = 20;
                        axVSPrinter1.Table = "<10|<850|<1800;|B.I.N.|" + sBIN;
                        axVSPrinter1.SpaceBefore = 0;
                        axVSPrinter1.Table = "<10|<850|<1560|^1450|^1800;|O.R. No.|" + sORNo + "|" + AppSettingsManager.GetConfigValue("58") + "|" + AppSettingsManager.GetConfigValue("59");
                        axVSPrinter1.Table = "<10|<850|<1560|^1450|^1800;|O.R. Date|" + sORDate + "| Sanitary Inspector IV | Municipal Health Officer";
                        axVSPrinter1.Table = "<10|<850|<1800;|Amount Paid|Php " + sFeeAmount;
                    }
                    catch
                    {
                        sLocation = "";
                        sBNSName = "";
                        sDay = "";
                        sMonth = "";
                        sYear = "";
                        sBIN = "";
                        sORNo = "";
                        sORDate = "";
                        sFeeAmount = "";
                    }
                }
            }
            result.Close();
        }

        private void AnnualInspection()
        {
            OracleResultSet result = new OracleResultSet();

            string sYear = string.Empty;
            string sORDate = string.Empty;
            string sFeeAmount = string.Empty;
            string sYear2 = string.Empty;
            string sName = string.Empty;
            string sCharOfOcc = string.Empty;
            string sCharGrp = string.Empty;
            string sLocation = string.Empty;
            string sCertOcc = string.Empty;
            string sDate = string.Empty;
            string sCertORNo = string.Empty;
            string sStallNo = string.Empty;

            int iCnt = 0;
            //result.Query = "select count(*) from sanitary_bldg_ext where bin = '" + m_sBin + "'";
            //int.TryParse(result.ExecuteScalar(), out iCnt);

            for (int ix = 0; ix <= iCnt; ix++)
            {
                result.Query = "Select * from annual_insp where bin ='" + m_sBin + "'";
                if (result.Execute())
                {

                    while (result.Read())
                    {
                        try
                        {
                            //sYear = m_sIssuedDate.ToString().Substring(m_sIssuedDate.Length - 2, 2);
                            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate()); //DJAC-20150102 change format
                            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
                            sORDate = m_sORDate;
                            sFeeAmount = m_sFeeAmount;
                            sName = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBin));

                            // RMC 20150117 (s)
                            if(sName.Trim() == "")
                                sName = result.GetString("own_ln") + ", " + result.GetString("own_fn") + " " + result.GetString("own_mi");
                            // RMC 20150117 (e)

                            if (m_sReportType == "Annual Inspection")
                                sCharOfOcc = result.GetString("char_occ");
                            else
                                sCharOfOcc = GetExtName("name");
                            sCharGrp = result.GetString("char_grp");
                            sLocation = result.GetString("location");
                            sCertOcc = result.GetString("cert_occ");
                            sDate = result.GetDateTime("cert_date").ToString("MM/dd/yyyy");
                            sCertORNo = result.GetString("cert_or_no");
                            //if (ix == 0)
                            if(m_sReportType =="Annual Inspection")
                                sStallNo = result.GetString("stall_no");
                            else
                                sStallNo = GetExtName("area");

                            axVSPrinter1.FontSize = 12;
                            axVSPrinter1.MarginLeft = 2000;
                            //axVSPrinter1.SpaceBefore = 2800;
                            //axVSPrinter1.Table = "<10|<4500|<6000;|" + "NO. _____________|DATE OF ISSUANCE: ____________________";

                            // RMC 20150105 mods in permit printing (s)
                            axVSPrinter1.SpaceBefore = 2900;

                            OracleResultSet pSetT = new OracleResultSet();
                            string sPermitNo = string.Empty;
                            string sDateIssued = string.Empty;

                            pSetT.Query = "select * from permit_type where bin = '" + m_sBin + "' and current_year = '" + sYear2 + "' ";
                            if (m_sReportType == "Annual Inspection")
                                pSetT.Query += " and perm_type = 'ANNUAL'";
                            else
                                pSetT.Query += " and perm_type = 'ANNUAL-EXT'";
                            if (pSetT.Execute())
                            {
                                if (pSetT.Read())
                                {
                                    sPermitNo = pSetT.GetString("current_year") + "-" + pSetT.GetString("permit_number");
                                    sDateIssued = pSetT.GetString("issued_date");
                                }
                            }

                            axVSPrinter1.Table = "<10|<4500|<6000;|" + "NO. " + sPermitNo + "|DATE OF ISSUANCE: " + sDateIssued;
                            // RMC 20150105 mods in permit printing (e)

                            //axVSPrinter1.FontSize = 10;
                            //axVSPrinter1.MarginLeft = 350;
                            //axVSPrinter1.SpaceBefore = 200;
                            //axVSPrinter1.Table = "<10|<6000;|This certification of Annual Inspection is issued/granted Pursuant to pertinent provision of Rule III of";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|the National Building Code PD 1096.";
                            //axVSPrinter1.FontSize = 12;

                            axVSPrinter1.MarginLeft = 1500;
                            axVSPrinter1.SpaceBefore = 1200;
                            if (sName.Length > 50)
                                axVSPrinter1.FontSize = 10;

                            axVSPrinter1.Table = "<10|<3500|<7000;|Name of Owner/Lessee: |" + sName;
                            axVSPrinter1.SpaceBefore = 0;
                            // RMC 20150107 (s)
                            string sTmp = AppSettingsManager.GetDTIName(m_sBin).Trim();
                            if (sTmp != "")
                                sCharOfOcc = sTmp;
                            // RMC 20150107 (e)
                            axVSPrinter1.Table = "<10|<3500|<7000;|Character of Occupancy: |" + sCharOfOcc; ;
                            axVSPrinter1.Table = "<10|<3500|<7000;|Group: |" + sCharGrp;
                            axVSPrinter1.Table = "<10|<3500|<5000;|Located at/Along: |" + sLocation;
                            axVSPrinter1.Table = "<10|<3500|<5000;|Under Certificate of Occupancy: |" + sCertOcc;
                            axVSPrinter1.Table = "<10|<3500|<5000;|Date: |" + sDate;
                            axVSPrinter1.Table = "<10|<3500|<5000;|With Official Receipt No.: |" + sCertORNo;
                            axVSPrinter1.Table = "<10|<3500|<5000;|Area/Stall No.: |" + sStallNo + " SQ.M";      // RMC 20150113 corrections in annual inspection entry of area

                            //axVSPrinter1.FontSize = 6;
                            //axVSPrinter1.MarginLeft = 350;
                            //axVSPrinter1.SpaceBefore = 180;
                            //axVSPrinter1.Table = "<10|<6000;|The Owner/Lessee shall properly maintain in the Building/Structure to enhance architectural well-";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|being structural stability, sanitation and fire-protective properties and shall not be occupied or used";
                            //axVSPrinter1.Table = "<10|<6000;|for the purpose other that its intended as stated above.";
                            //axVSPrinter1.SpaceBefore = 100;
                            //axVSPrinter1.Table = "<10|<6000;|No alteration/addition/repairs/new electronics or mechanical/plumbing/sanitary installation shall be";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|made thereon without a permit therefore.";
                            //axVSPrinter1.SpaceBefore = 100;
                            //axVSPrinter1.Table = "<10|<6000;|The designer is aware that the under Article 1723 of the Civil Code of the Philippines be is";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|responsible for damages if within fifteen(15) years from the completion of the structure. It should";
                            //axVSPrinter1.Table = "<10|<6000;|collapse due to defect in the plans of specifications of the structure to ensure that the condition";
                            //axVSPrinter1.Table = "<10|<6000;|under which the structure was designed are not being violated abused.";
                            //axVSPrinter1.SpaceBefore = 100;
                            //axVSPrinter1.Table = "<10|<6000;|A certified copy hereof shall be posted within the premises of the building and shall not be remove";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|without authority from the building official.";

                            axVSPrinter1.FontSize = 32;
                            axVSPrinter1.MarginLeft = 1800;
                            axVSPrinter1.SpaceBefore = 4650;
                            axVSPrinter1.FontBold = true;
                            axVSPrinter1.Table = "<10|<3000;|" + sYear2;
                            axVSPrinter1.SpaceBefore = 0;

                            axVSPrinter1.FontSize = 12;
                            axVSPrinter1.MarginLeft = 1800;
                            //axVSPrinter1.SpaceBefore = 300;
                            axVSPrinter1.SpaceBefore = 100;
                            axVSPrinter1.FontBold = false;
                            axVSPrinter1.Table = "<10|<4000|^5000;|O.R. No. _________|" + AppSettingsManager.GetConfigValue("61");
                            axVSPrinter1.SpaceBefore = 0;
                            axVSPrinter1.Table = "<10|<4000|^5000;|O.R. Date ________| Building Official";
                            axVSPrinter1.Table = "<10|<3500;|Fee Paid Php______";
                            //axVSPrinter1.FontSize = 7;
                            //axVSPrinter1.FontBold = false;
                            //axVSPrinter1.Table = "<10|<800|<1850|<1600;|Date Paid |" + sORDate + "|" + AppSettingsManager.GetConfigValue("61");
                            //axVSPrinter1.Table = "<10|<800|<1850|<1600;|Fee Paid |" + sFeeAmount + "|" + "Building Official";

                        }
                        catch
                        {
                            sYear = "";
                            sYear2 = "";
                            sORDate = "";
                            sFeeAmount = "";
                            sName = "";
                            sCharOfOcc = "";
                            sCharGrp = "";
                            sLocation = "";
                            sCertOcc = "";
                            sDate = "";
                            sCertORNo = "";
                            sStallNo = "";
                        }
                    }
                }
                result.Close();

                //axVSPrinter1.NewPage();
            }
        }
        private void AnnualInspection_NEW()//jhb 20190703
        {
            OracleResultSet result = new OracleResultSet();
           // this.axVSPrinter1.DrawPicture(Properties.Resources.Annual_Inspection_letter_v2_01, " -1.20in", "-0.0in", "82.0%", "64.0%", 10, false);
            
            string sYear = string.Empty;
            string sORDate = string.Empty;
            string sFeeAmount = string.Empty;
            string sYear2 = string.Empty;
            string sName = string.Empty;
            string sCharOfOcc = string.Empty;
            string sCharGrp = string.Empty;
            string sLocation = string.Empty;
            string sCertOcc = string.Empty;
            string sDate = string.Empty;
            string sCertORNo = string.Empty;
            string sStallNo = string.Empty;
            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
            int iCnt = 0;
            //result.Query = "select count(*) from sanitary_bldg_ext where bin = '" + m_sBin + "'";
            //int.TryParse(result.ExecuteScalar(), out iCnt);

            for (int ix = 0; ix <= iCnt; ix++)
            {
                result.Query = "Select * from annual_insp where bin ='" + m_sBin + "' and tax_year = '" + sYear2 + "' ";
                if (result.Execute())
                {

                    while (result.Read())
                    {
                        try
                        {
                            //sYear = m_sIssuedDate.ToString().Substring(m_sIssuedDate.Length - 2, 2);
                            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate()); //DJAC-20150102 change format
                          
                            sORDate = m_sORDate;
                            sFeeAmount = m_sFeeAmount;
                            sName = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBin));

                            // RMC 20150117 (s)
                            if (sName.Trim() == "")
                                sName = result.GetString("own_ln") + ", " + result.GetString("own_fn") + " " + result.GetString("own_mi");
                            // RMC 20150117 (e)

                            if (m_sReportType == "Annual Inspection")
                                sCharOfOcc = result.GetString("char_occ");
                            else
                                sCharOfOcc = GetExtName("name");
                            sCharGrp = result.GetString("char_grp");
                            sLocation = result.GetString("location");
                            sCertOcc = result.GetString("cert_occ");
                            sDate = result.GetDateTime("cert_date").ToString("MM/dd/yyyy");
                            sCertORNo = result.GetString("cert_or_no");
                            //if (ix == 0)
                            if (m_sReportType == "Annual Inspection")
                                sStallNo = result.GetString("stall_no");
                            else
                                sStallNo = GetExtName("area");

                            axVSPrinter1.FontSize = 12;
                            axVSPrinter1.MarginLeft = 3000;
                            //axVSPrinter1.SpaceBefore = 2800;
                            //axVSPrinter1.Table = "<10|<4500|<6000;|" + "NO. _____________|DATE OF ISSUANCE: ____________________";

                            // RMC 20150105 mods in permit printing (s)
                            axVSPrinter1.SpaceBefore = 1600;

                            OracleResultSet pSetT = new OracleResultSet();
                            string sPermitNo = string.Empty;
                            string sDateIssued = string.Empty;

                            pSetT.Query = "select * from permit_type where bin = '" + m_sBin + "' and current_year = '" + sYear2 + "' ";
                            if (m_sReportType == "Annual Inspection")
                                pSetT.Query += " and perm_type = 'ANNUAL'";
                            else
                                pSetT.Query += " and perm_type = 'ANNUAL-EXT'";
                            if (pSetT.Execute())
                            {
                                if (pSetT.Read())
                                {
                                    sPermitNo = pSetT.GetString("current_year") + "-" + pSetT.GetString("permit_number");
                                    sDateIssued = pSetT.GetString("issued_date");
                                }
                            }

                            axVSPrinter1.Table = "<500|<4000|<5000;|" + "NO. " + sPermitNo + "|DATE OF ISSUANCE: " + sDateIssued;
                            // RMC 20150105 mods in permit printing (e)

                            //axVSPrinter1.FontSize = 10;
                            //axVSPrinter1.MarginLeft = 350;
                            //axVSPrinter1.SpaceBefore = 200;
                            //axVSPrinter1.Table = "<10|<6000;|This certification of Annual Inspection is issued/granted Pursuant to pertinent provision of Rule III of";
                            //axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<6000;|the National Building Code PD 1096.";
                            //axVSPrinter1.FontSize = 12;

                          //  long yGrid = Convert.ToInt64(axVSPrinter1.CurrentY);
                           axVSPrinter1.MarginLeft = 1500;
                            this.axVSPrinter1.CurrentY = 3150; //3060
                            if (sName.Length > 50)
                                axVSPrinter1.FontSize = 10;

                            axVSPrinter1.Table = "<10|<3500|<7000;||" + sName; // axVSPrinter1.Table = "<10|<3500|<7000;||" + sName;
                            axVSPrinter1.SpaceBefore = 0;
                            string sTmp = AppSettingsManager.GetDTIName(m_sBin).Trim();
                            if (sTmp != "")
                                sCharOfOcc = sTmp;
                           
                            this.axVSPrinter1.CurrentY = 5150; //5000
                            axVSPrinter1.Table = "<10|<3500|<7000;||" + sCharOfOcc;
                            this.axVSPrinter1.CurrentY = 5520; //5400
                            axVSPrinter1.Table = "<10|<3500|<7000;||" + sCharGrp;
                            this.axVSPrinter1.CurrentY = 5800; //5700
                            axVSPrinter1.Table = "<10|<3500|<7000;||" + sLocation;
                            this.axVSPrinter1.CurrentY = 6150; //6000
                            axVSPrinter1.Table = "<10|<3500|<7000;||" + sCertOcc;
                            this.axVSPrinter1.CurrentY = 6520; //6400
                            axVSPrinter1.Table = "<10|<3500|<7000;||" + sDate;
                            this.axVSPrinter1.CurrentY = 6900; //6800
                            axVSPrinter1.Table = "<10|<3500|<7000;||" + sCertORNo;
                            this.axVSPrinter1.CurrentY = 7240; //7150
                            axVSPrinter1.Table = "<10|<3500|<7000;||" + sStallNo + " SQ.M";      // RMC 20150113 corrections in annual inspection entry of area


                            axVSPrinter1.FontSize = 40;
                            axVSPrinter1.MarginLeft = 800;
                            axVSPrinter1.SpaceBefore = 4650;
                            axVSPrinter1.FontBold = true;
                            this.axVSPrinter1.TextColor = System.Drawing.Color.White;
                            this.axVSPrinter1.DrawPicture(Properties.Resources.blue, "0.53in", "8.48in", "70%", "100%", 11, false); //JHB 20190801
                            axVSPrinter1.Table = "<2000;" + sYear2;
                            this.axVSPrinter1.TextColor = System.Drawing.Color.Black;
                            axVSPrinter1.SpaceBefore = 0;

                            axVSPrinter1.FontSize = 12;
                            axVSPrinter1.MarginLeft = 3500;
                            //axVSPrinter1.SpaceBefore = 300;
                            axVSPrinter1.SpaceBefore = 700;
                            axVSPrinter1.FontBold = false;
                            //axVSPrinter1.Table = "<10|<4000|^5000;|O.R. No. _________|" + AppSettingsManager.GetConfigValue("61");
                           // axVSPrinter1.SpaceBefore = 0;
                            //axVSPrinter1.Table = "<10|<4000|^5000;|O.R. Date ________| Building Official";
                            //axVSPrinter1.Table = "<10|<3500;|Fee Paid Php______";
                            //axVSPrinter1.FontSize = 7;
                            //axVSPrinter1.FontBold = false;
                            //axVSPrinter1.Table = "<10|<800|<1850|<1600;|Date Paid |" + sORDate + "|" + AppSettingsManager.GetConfigValue("61");
                            //axVSPrinter1.Table = "<10|<800|<1850|<1600;|Fee Paid |" + sFeeAmount + "|" + "Building Official";
                            axVSPrinter1.Table = "<10|<4000|<5000;||" + AppSettingsManager.GetConfigValue("61");
                            axVSPrinter1.SpaceBefore = 0;
                            axVSPrinter1.Table = "<10|<4000|<5000;|| Building Official";
                           
                           
                        }
                        catch
                        {
                            sYear = "";
                            sYear2 = "";
                            sORDate = "";
                            sFeeAmount = "";
                            sName = "";
                            sCharOfOcc = "";
                            sCharGrp = "";
                            sLocation = "";
                            sCertOcc = "";
                            sDate = "";
                            sCertORNo = "";
                            sStallNo = "";
                        }
                    }
                }
                result.Close();
              
                //JHB 20210120 add date time (s)
                axVSPrinter1.MarginLeft = 3500;
                axVSPrinter1.HdrFontName = "Arial";
                axVSPrinter1.HdrFontSize = 7.0f;
               
                axVSPrinter1.Footer = string.Format("{0}||{1}", "", "Printed  Date:" + AppSettingsManager.GetCurrentDate().ToString("yyyy-MM-dd hh:mm:ss"));
                //JHB 20210120 add date time (e)

                //axVSPrinter1.NewPage();
            }
        }
        #region//jhb 20200124 sample
        //private void Barangay_Certificate() //JHB 20191012 add for brgy clearance (s)
        //{
        //    //   this.axVSPrinter1.DrawPicture(Properties.Resources.Untitled_1," 0.10in", "-2.5in", "33.0%", "46.8%", 10, false);
        //    OracleResultSet result = new OracleResultSet();
        //    string sDay = string.Empty;
        //    string sMonth = string.Empty;
        //    string sYear = string.Empty;
        //    string sYear2 = string.Empty;
        //    string sBrgy = string.Empty;
        //    string sDate = string.Empty;
        //    string sStat = AppSettingsManager.GetBnsStat(m_sBin);
        //    string sMaritalStat = AppSettingsManager.GetMaritalStat(m_sBin);
        //    string sTempBin = m_sTempBIN;
        //    string sOwnCode = AppSettingsManager.GetOwnCode(m_sBin);
        //    string sUserNm = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
        //    DateTime sDateSave = AppSettingsManager.GetSystemDate();
        //    string DATE_CREATED = string.Format("{0:MM/dd/yyyy}", sDateSave);


        //    sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate());
        //    sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
        //    sMonth = string.Format("{0:MMMM}", AppSettingsManager.GetCurrentDate());
        //    sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());

        //    sBrgy = AppSettingsManager.GetBnsBrgy(m_sBin);


        //    sDate = sMonth + " " + sDay + ", " + sYear2;
        //    long yGrid = Convert.ToInt64(axVSPrinter1.CurrentY);


        //    //BARANGAY
        //    //axVSPrinter1.MarginLeft = 4000;
        //    //this.axVSPrinter1.CurrentY = 1000;
        //    //axVSPrinter1.SpaceBefore = 750;
        //    //axVSPrinter1.FontBold = true;
        //    //axVSPrinter1.FontSize = 12;
        //    //axVSPrinter1.Table = "<10|^5000;|BARANGAY " + sBrgy;
        //    //axVSPrinter1.FontBold = false;



        //    //DATE
        //    axVSPrinter1.MarginLeft = 3800;
        //    this.axVSPrinter1.CurrentY = 2900;
        //    axVSPrinter1.SpaceBefore = 750;
        //    axVSPrinter1.FontBold = true;
        //    axVSPrinter1.FontSize = 12;
        //    axVSPrinter1.Table = "<10|>7500;|" + sDate;

        //    //OWNER (S)
        //    this.axVSPrinter1.CurrentY = 5000;
        //    axVSPrinter1.MarginLeft = 6000;
        //    axVSPrinter1.SpaceBefore = 300;

        //    if (m_sBnsOwn.Length > 100)
        //    {
        //        axVSPrinter1.FontSize = 8;
        //    }
        //    else if (m_sBnsOwn.Length > 50)
        //    {
        //        axVSPrinter1.FontSize = 9;
        //    }
        //    else
        //    {
        //        axVSPrinter1.FontSize = 12;
        //    }
        //    axVSPrinter1.Table = "<10|^5000;|" + m_sBnsOwn;
        //    //OWNER (E)


        //    //MARITAL STATUS (S)
        //    this.axVSPrinter1.CurrentY = 5400;
        //    axVSPrinter1.MarginLeft = 3400;
        //    axVSPrinter1.FontSize = 11;
        //    axVSPrinter1.Table = "<10|^5000;|" + sMaritalStat + ".";


        //    //OWNER'S ADDRESS  (S)
        //    this.axVSPrinter1.CurrentY = 6300;
        //    axVSPrinter1.MarginLeft = 3800;

        //    if (m_sBnsAdd.Length > 100)
        //    {
        //        axVSPrinter1.FontSize = 7;
        //    }
        //    else if (m_sBnsAdd.Length > 35)
        //    {
        //        axVSPrinter1.FontSize = 9;
        //    }
        //    else
        //    {
        //        axVSPrinter1.FontSize = 12;
        //    }
        //    axVSPrinter1.Table = "<10|<5000;|" + m_sBnsAdd;
        //    //OWNER'S ADDRESS (E)

        //    //BUSINESS NAME  (S)


        //    string sBnsName = StringUtilities.HandleApostrophe(m_sBnsName).Trim();
        //    this.axVSPrinter1.CurrentY = 7000;
        //    axVSPrinter1.MarginLeft = 3800;
        //    if (sBnsName.Length > 100)
        //    {
        //        axVSPrinter1.FontSize = 7;
        //    }
        //    else if (sBnsName.Length > 35)
        //    {
        //        axVSPrinter1.FontSize = 8;
        //    }
        //    else
        //    {
        //        axVSPrinter1.FontSize = 10;
        //    }

        //    axVSPrinter1.FontBold = true;
        //    axVSPrinter1.Table = "<10|<5000;|" + sBnsName;
        //    //BUSINESS NAME (E)


        //    //axVSPrinter1.MarginLeft = 6620; //6620
        //    //axVSPrinter1.SpaceBefore = 1450; //1350
        //    //axVSPrinter1.Table = "<10|<600;|" + sYear + " ,";
        //    //axVSPrinter1.MarginLeft = 4950;
        //    //axVSPrinter1.SpaceBefore = 700;//670
        //    //axVSPrinter1.Table = "<10|>500|>2000|>900|<800;|" + sDay + "|" + sMonth + "||" + sYear2;


        //    //axVSPrinter1.MarginLeft = 2700;
        //    //this.axVSPrinter1.CurrentY = 1100;
        //    //axVSPrinter1.FontSize = 10;
        //    //axVSPrinter1.FontBold = false;

        //    //axVSPrinter1.Table = "<10|<100|<3000|^1000|^1000|<2800;||Recommending Approval: ||| Approved:";
        //    //axVSPrinter1.SpaceBefore = 500;
        //    //axVSPrinter1.Table = "<10|<100|<3000|^1000|^1000|<2800;||" + AppSettingsManager.GetConfigValue("58") + "|||" + AppSettingsManager.GetConfigValue("59");
        //    //axVSPrinter1.SpaceBefore = 0;
        //    //axVSPrinter1.Table = "<10|<100|<3000|^1000|^1000|<2800;||OIC- SANITATION|||Municipal Health Officer";

        //    //TAX YEAR
        //    //axVSPrinter1.MarginLeft = 500;
        //    //this.axVSPrinter1.CurrentY = 12800;
        //    //axVSPrinter1.FontSize = 40;
        //    //axVSPrinter1.FontBold = true;
        //    //this.axVSPrinter1.TextColor = System.Drawing.Color.White;
        //    //this.axVSPrinter1.DrawPicture(Properties.Resources.blue, "0.30in", "8.90in", "70%", "100%", 11, false); //JHB 20190801
        //    //axVSPrinter1.Table = "<2000;" + sYear2;

        //    //this.axVSPrinter1.TextColor = System.Drawing.Color.Black;

        //    //OR DETAILS (S)

        //    axVSPrinter1.FontSize = 12;
        //    axVSPrinter1.CurrentY = 11700; //12000;
        //    axVSPrinter1.MarginLeft = 6300;
        //    axVSPrinter1.Table = "<10|<5000;| " + m_sORNo;
        //    axVSPrinter1.CurrentY = 12200; //12400;
        //    axVSPrinter1.MarginLeft = 5200;
        //    axVSPrinter1.Table = "<10|<5000;|" + m_sORDate;
        //    axVSPrinter1.CurrentY = 12700; //12900;
        //    axVSPrinter1.MarginLeft = 5200;
        //    axVSPrinter1.Table = "<10|<5000;|" + m_sIssuedOn;
        //    //axVSPrinter1.CurrentY = 12900;
        //    //axVSPrinter1.MarginLeft = 5200;
        //    //axVSPrinter1.Table = "<10|<5000;|O.R. Date: " + m_sFeeAmount;
        //    CheckifBrgyCleanExist(m_sBin);

        //    if (brgy_reprint == false)
        //    {

        //        result.Query = "Insert into BRGY_CLEARANCE(BIN, TAX_YEAR, TEMP_BIN, BNS_NM, BNS_STAT, OWN_CODE, OWN_NM, MARITAL_STAT, ";
        //        result.Query += "BNS_BRGY, CTC_NO, CTC_ISSUED_ON, CTC_ISSUED_AT, CTC_AMT, DATE_CREATED, BNS_USER) ";
        //        result.Query += "values('" + m_sBin + "', '" + sYear2 + "', '" + sTempBin + "', '" + sBnsName + "', '" + sStat + "', '" + sOwnCode + "', '" + m_sBnsOwn + "', '" + sMaritalStat + "', ";
        //        result.Query += "'" + sBrgy + "', '" + m_sORNo + "', '" + m_sIssuedOn + "', '" + m_sORDate + "', '" + m_sFeeAmount + "', '" + DATE_CREATED + "', '" + sUserNm + "')";
        //    }
        //    else
        //    {
        //        result.Query = "UPDATE BRGY_CLEARANCE SET CTC_NO = '" + m_sORNo + "',CTC_ISSUED_ON = '" + m_sIssuedOn + "', MARITAL_STAT = '" + sMaritalStat + "',BNS_BRGY = '" + sBrgy + "',OWN_CODE = '" + sOwnCode + "',BNS_STAT= '" + sStat + "', ";
        //        result.Query += "CTC_ISSUED_AT = '" + m_sORDate + "', CTC_AMT = '" + m_sFeeAmount + "' WHERE BIN = '" + m_sBin + "' and tax_year = '" + sYear2 + "' ";

        //    }

        //    result.ExecuteNonQuery();
        //    if (!result.Commit())
        //    {
        //        result.Rollback();
        //    }
        //    result.Close();


        //    //axVSPrinter1.Table = "<10|<2000|<2000|O.R. No.: " + m_sORNo;
        //    //axVSPrinter1.Table = "<10|<2000|<2000|O.R. Date: " + m_sORDate;
        //    //axVSPrinter1.Table = "<10|<2000|<2000|O.R. Amount: " + m_sFeeAmount;
        //    //axVSPrinter1.SpaceBefore = 0;
        //    //axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. Date ________| OIC- SANITATION| Municipal Health Officer";
        //    ////axVSPrinter1.Table = "<10|<2800|^4000|^3000;|O.R. Date. _________|" + AppSettingsManager.GetConfigRemarksValue("58") + "|" + AppSettingsManager.GetConfigObject("59");
        //    //axVSPrinter1.Table = "<10|<2800;|Fee Paid Php______";

        //} //JHB 20191012 add for brgy clearance (s)

        #endregion//jhb 20200124 sample



        private void WorkingPermit()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            OracleResultSet rCount = new OracleResultSet();

            string m_sOccupation = string.Empty;
            string m_sPermitNo = string.Empty;
            string m_sPermitDate = string.Empty;
            string sPeriodCovered = string.Empty;
            string sTIN = string.Empty;
            string sCTCNo = string.Empty;
            string sIssuedOn = string.Empty;
            string sIssuedAt = string.Empty;
            string sApplicant = string.Empty;
            string sAddress = string.Empty;
            string sGender = string.Empty;
            string sDOB = string.Empty;
            string sJob = string.Empty;
            string sEmployer = string.Empty;
            string sBusinessAdd = string.Empty;

            int sCount = 0;
            int iCount = 0;

            m_sOccupation = "OWNER";

            result2.Query = "Select current_year, permit_number, issued_date from permit_working_number where bin ='" + m_sBin + "' and emp_id = '" + m_sEmpID + "'";
            if (result2.Execute())
            {
                while (result2.Read())
                {
                    m_sPermitNo = result2.GetString("current_year") + "-" + result2.GetString("permit_number");
                    m_sPermitDate = result2.GetDateTime("issued_date").ToString("MM/dd/yyyy");
                }
            }

            result.Query = @"select EMP_ID,EMP_TIN,EMP_CTC_NUMBER,EMP_CTC_ISSUED_ON,EMP_CTC_ISSUED_AT,
EMP_LN||', '||EMP_FN||' '||EMP_MI as EMP_NAME,EMP_ADDRESS,EMP_GENDER,EMP_DATE_OF_BIRTH,EMP_OCCUPATION,BIN,TAX_YEAR
from emp_names where (bin = '" + m_sBin + "' or temp_bin  = '" + m_sBin + "')";

            if (m_sPermitType == "ARCS")
                result.Query += " and emp_id = '" + m_sEmpID + "'";
            else
                result.Query += " and tax_year = '" + m_sTaxYear + "' and emp_id = '" + m_sEmpID + "'";

            //rCount.Query = "Select count(*) as emp_count from emp_names where bin ='" + m_sBin + "' and tax_year = '" + m_sTaxYear + "' and emp_occupation != '" + m_sOccupation + "'";
            //int.TryParse(rCount.ExecuteScalar(), out sCount);

            axVSPrinter1.FontSize = 8;
            if (result.Execute())
            {
                while (result.Read())
                {
                    sPeriodCovered = "December 31, " + AppSettingsManager.GetConfigValue("12"); //JARS 20180212 ADDED EXPIRATION DATE
                    sTIN = result.GetString("emp_tin");
                    sCTCNo = result.GetString("emp_ctc_number");
                    sIssuedOn = result.GetString("emp_ctc_issued_on");
                    sIssuedAt = result.GetString("emp_ctc_issued_at");

                    sApplicant = result.GetString("emp_name");
                    sAddress = result.GetString("emp_address");
                    sGender = result.GetString("emp_gender");
                    sDOB = result.GetString("emp_date_of_birth");
                    sJob = result.GetString("emp_occupation");
                    //JHB 20200702 add adjustment for  not registred business but required for working permit (s)
                    if (m_sPermitType == "ARCS")
                    {
                        result3.Query = "select bns_nm,bns_add from emp_names where bin = '" + m_sBin + "'";
                        if (result3.Execute())
                            if (result3.Read())
                            {
                                sEmployer = result3.GetString(0).Trim();
                                sBusinessAdd = result3.GetString(1).Trim();
                            }
                        result3.Close();
                    }//JHB 20200702 add adjustment for  not registred business but required for working permit (e)
                    else
                    {
                        sEmployer = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBin));
                        sBusinessAdd = AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(m_sBin));
                    }
                    axVSPrinter1.CurrentY = 1360;//1200
                    axVSPrinter1.MarginLeft = 2400;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + m_sORNo + "|" + m_sPermitNo;
                    axVSPrinter1.SpaceBefore = 160;//160
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + m_sORDate + "|" + m_sPermitDate;
                    axVSPrinter1.MarginLeft = 4000;
                    axVSPrinter1.Table = "<10|<3000;|" + sPeriodCovered;
                    axVSPrinter1.SpaceBefore = 0;

                    axVSPrinter1.MarginLeft = 2400;
                    this.axVSPrinter1.CurrentY = 2105;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + sTIN + "|" + sCTCNo;
                    this.axVSPrinter1.CurrentY = 2521;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + sIssuedOn + "|" + sIssuedAt;
                    axVSPrinter1.MarginLeft = 290;

                    this.axVSPrinter1.CurrentY = 3100;//2960
                    axVSPrinter1.MarginLeft = 1700;
                    axVSPrinter1.Table = "<10|<4600;|" + sApplicant;
                    axVSPrinter1.MarginLeft = 290;
                    this.axVSPrinter1.CurrentY = 3480;//3350
                    axVSPrinter1.Table = "<10|<5000;|" + sAddress;

                    this.axVSPrinter1.CurrentY = 4000;//3870
                    axVSPrinter1.Table = "<10|<1050|<1400|<2500;|" + sGender + "|" + sDOB + "|" + sJob;

                    this.axVSPrinter1.CurrentY = 4380;
                    axVSPrinter1.MarginLeft = 1700;
                    axVSPrinter1.Table = "<10|<4600;|" + sEmployer;
                    axVSPrinter1.MarginLeft = 290;
                    this.axVSPrinter1.CurrentY = 4820;
                    axVSPrinter1.Table = "<30|<5000;|" + sBusinessAdd;

                    axVSPrinter1.MarginLeft = 4100;
                    this.axVSPrinter1.CurrentY = 5420;
                    m_sTaxYear = ConfigurationAttributes.CurrentYear;// JHB 20190731 LUB-1910573 
                    axVSPrinter1.Table = "<10|<2000;|" + m_sTaxYear;
                    axVSPrinter1.SpaceBefore = 0;

                    iCount++;
                    if (iCount < sCount)
                    {
                        axVSPrinter1.NewPage();
                    }
                }
            }
            result.Close();
        }

        private void ToolPrint_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintQuality = VSPrinter7Lib.PrintQualitySettings.pqHigh;

            if (MessageBox.Show("Are you sure you want to print?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            axVSPrinter1.PrintDoc(1, 1, axVSPrinter1.PageCount);

            // RMC 20141222 modified permit printing (s)
            OracleResultSet pSet = new OracleResultSet();
            if (m_sReportType == "Zoning")
            {
                if (AuditTrail.InsertTrail("ABZC-P", "zoning", StringUtilities.HandleApostrophe(m_sBin)) == 0)
                {
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (m_sReportType == "Annual Inspection")
            {
                if (AuditTrail.InsertTrail("ABAIP-P", "annual_insp", StringUtilities.HandleApostrophe(m_sBin)) == 0)
                {
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (m_sReportType == "Sanitary")
            {
                if (AuditTrail.InsertTrail("ABSP-P", "sanitary", StringUtilities.HandleApostrophe(m_sBin)) == 0)
                {
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // RMC 20141222 modified permit printing (e)
        }

        private void toolSettingPageSetup_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPageSetup);
        }

        private void toolSettingPrintPage_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private string ConvertDayInOrdinalForm(string sDay)
        {
            // RMC 20150105 mods in permit printing
            string sLastNo = "";
            string sDayth = "";

            sLastNo = StringUtilities.Right(sDay, 1);

            switch (Convert.ToInt32(sLastNo))
            {
                case 1: sDayth = sDay + "st"; break;
                case 2: sDayth = sDay + "nd"; break;
                case 3: sDayth = sDay + "rd"; break;
                default: sDayth = sDay + "th"; break;
            }

            return sDayth;
        }

        private string GetExtName(string sSwitch)
        {
            OracleResultSet result = new OracleResultSet();
            string sExt = string.Empty;

            result.Query = "select * from sanitary_bldg_ext where bin = '" + m_sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    if(sSwitch == "name")
                        sExt = result.GetString("bns_nm");
                    else if (sSwitch == "area")
                    {
                        double dArea = 0;
                        double.TryParse(result.GetString("area"), out dArea);
                        sExt = string.Format("{0:#,###}", dArea);
                    }
                }
            }
            result.Close();

            return sExt;
        }

        private bool CheckifBrgyCleanExist(string sBin) //JHB 20191012 add for brgy clearance
        {
            OracleResultSet pSet = new OracleResultSet();
            

            pSet.Query = "select * from brgy_clearance where bin = '" + m_sBin + "' and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                   
                    brgy_reprint = true;
                }
            }

            pSet.Close();
            return true;
        }

        private void axVSPrinter1_StartDocEvent(object sender, EventArgs e)
        {

        }

         public static bool ValidateTransLog(string sName, string m_sBin, string m_sTaxYear)
        {
            OracleResultSet pTmp = new OracleResultSet();

            pTmp.Query = "select * from trans_log where bin = '" + m_sBin + "' and  tax_year = '" + m_sTaxYear + "' and trans_code like '%"+sName+"%' ";
            if (pTmp.Execute()) 
            {
                if (pTmp.Read())
                {
                    pTmp.Close();
                    return true;
                }
                else
                {
                    pTmp.Close();
                    return false;
                }
            }

            return true;
        }

        
    }
}