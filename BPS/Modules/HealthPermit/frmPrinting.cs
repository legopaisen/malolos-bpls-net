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

        private string m_sORNo = String.Empty;
        private string m_sORDate = String.Empty;
        private string m_sFeeAmount = String.Empty;
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

        //MCR 20150113 (e) Working Permit

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
                SanitaryNew();  // RMC 20141222 modified permit printing
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
                AnnualInspection();
            }

            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

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

            result.Query = "Select * from zoning where bin ='" + m_sBin + "'";
            result.Query += " and tax_year = '" + m_sTaxYear + "'";     // RMC 20150105 mods in permit printing
            if (result.Execute())
            {
                while (result.Read())
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
                        //sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());
                        sDay = string.Format("{0}", AppSettingsManager.GetCurrentDate().Day);

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
                        axVSPrinter1.Table = "<10|^3000;|" + AppSettingsManager.GetConfigValue("60");
                        axVSPrinter1.SpaceBefore = 0;
                        axVSPrinter1.Table = "<10|^3000;|MPDC/DZA";
                        axVSPrinter1.MarginLeft = 800;
                        axVSPrinter1.SpaceBefore = 1000;
                        //axVSPrinter1.Table = "<10|<4000;|O.R. No. _________"; // RMC 20150107 removed
                        axVSPrinter1.SpaceBefore = 0;
                        //axVSPrinter1.Table = "<10|<4000;|O.R. Date ________"; // RMC 20150107 removed
                        //axVSPrinter1.Table = "<10|<3500;|Fee Paid Php______"; // RMC 20150107 removed
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

                        axVSPrinter1.MarginLeft = 4560;
                        axVSPrinter1.Table = "<10|<5000;|";
                        axVSPrinter1.SpaceBefore = 200;
                        axVSPrinter1.FontSize = 8;
                        axVSPrinter1.Table = "<10|<5000;|" + sRegNo;
                        axVSPrinter1.MarginLeft = 510;
                        axVSPrinter1.SpaceBefore = 70;
                        axVSPrinter1.FontSize = 12;
                        axVSPrinter1.Table = "<10|<5000;|" + sName;
                        axVSPrinter1.SpaceBefore = 580;
                        axVSPrinter1.Table = "<10|<1700|<5000;|" + sAge + "|" + sSex;
                        axVSPrinter1.Table = "<10|<5000;|" + sOccupation;
                        axVSPrinter1.Table = "<10|<5000;|" + sPlaceOfWork;
                        axVSPrinter1.Table = "<10|<5000;|" + sNationality;
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

        private void SanitaryNew()
        {
            string sDay = string.Empty;
            string sMonth = string.Empty;
            string sYear = string.Empty;
            string sYear2 = string.Empty;

            sYear = string.Format("{0:yy}", AppSettingsManager.GetCurrentDate());
            sYear2 = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
            sMonth = string.Format("{0:MMMM}", AppSettingsManager.GetCurrentDate());
            //sDay = string.Format("{0:dd}", AppSettingsManager.GetCurrentDate());
            sDay = string.Format("{0}", AppSettingsManager.GetCurrentDate().Day);

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
            axVSPrinter1.Table = "<10|<3000|<1300;| Recommending Approval: | Approved:";

            //DJAC-20150102 added o.r fields
            axVSPrinter1.MarginLeft = 1800;
            axVSPrinter1.SpaceBefore = 300;
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Table = "<10|<2800|^3000|^3000;|O.R. No. _________|" + AppSettingsManager.GetConfigValue("58") + "|" + AppSettingsManager.GetConfigValue("59");
            axVSPrinter1.SpaceBefore = 0;
            axVSPrinter1.Table = "<10|<2800|^3000|^3000;|O.R. Date ________| Sanitary Inspector IV | Municipal Health Officer";
            axVSPrinter1.Table = "<10|<2800;|Fee Paid Php______";
        }

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

        private void WorkingPermit()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
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
from emp_names where (bin = '" + m_sBin + "' or temp_bin  = '" + m_sBin + "') and tax_year = '" + m_sTaxYear + "' and emp_id = '" + m_sEmpID + "'";

            //rCount.Query = "Select count(*) as emp_count from emp_names where bin ='" + m_sBin + "' and tax_year = '" + m_sTaxYear + "' and emp_occupation != '" + m_sOccupation + "'";
            //int.TryParse(rCount.ExecuteScalar(), out sCount);

            axVSPrinter1.FontSize = 8;
            if (result.Execute())
            {
                while (result.Read())
                {
                    sPeriodCovered = "";
                    sTIN = result.GetString("emp_tin");
                    sCTCNo = result.GetString("emp_ctc_number");
                    sIssuedOn = result.GetString("emp_ctc_issued_on");
                    sIssuedAt = result.GetString("emp_ctc_issued_at");

                    sApplicant = result.GetString("emp_name");
                    sAddress = result.GetString("emp_address");
                    sGender = result.GetString("emp_gender");
                    sDOB = result.GetString("emp_date_of_birth");
                    sJob = result.GetString("emp_occupation");
                    sEmployer = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBin));
                    sBusinessAdd = AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(m_sBin));

                    axVSPrinter1.CurrentY = 1200;
                    axVSPrinter1.MarginLeft = 2600;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + m_sORNo + "|" + m_sPermitNo;
                    axVSPrinter1.SpaceBefore = 160;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + m_sORDate + "|" + m_sPermitDate;
                    axVSPrinter1.Table = "<10|<3000;|" + sPeriodCovered;
                    axVSPrinter1.SpaceBefore = 0;

                    axVSPrinter1.MarginLeft = 2400;
                    this.axVSPrinter1.CurrentY = 2105;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + sTIN + "|" + sCTCNo;
                    this.axVSPrinter1.CurrentY = 2521;
                    axVSPrinter1.Table = "<10|<2000|<2000;|" + sIssuedOn + "|" + sIssuedAt;
                    axVSPrinter1.MarginLeft = 290;

                    this.axVSPrinter1.CurrentY = 2960;
                    axVSPrinter1.MarginLeft = 1700;
                    axVSPrinter1.Table = "<10|<4600;|" + sApplicant;
                    axVSPrinter1.MarginLeft = 290;
                    this.axVSPrinter1.CurrentY = 3350;
                    axVSPrinter1.Table = "<10|<5000;|" + sAddress;

                    this.axVSPrinter1.CurrentY = 3870;
                    axVSPrinter1.Table = "<10|<1050|<1400|<2500;|" + sGender + "|" + sDOB + "|" + sJob;

                    this.axVSPrinter1.CurrentY = 4260;
                    axVSPrinter1.MarginLeft = 1700;
                    axVSPrinter1.Table = "<10|<4600;|" + sEmployer;
                    axVSPrinter1.MarginLeft = 290;
                    this.axVSPrinter1.CurrentY = 4610;
                    axVSPrinter1.Table = "<10|<5000;|" + sBusinessAdd;

                    axVSPrinter1.MarginLeft = 4120;
                    this.axVSPrinter1.CurrentY = 5053;
                    axVSPrinter1.Table = "<10|<2000;|" + m_sTaxYear;
                    axVSPrinter1.SpaceBefore = 0;

                    //iCount++;
                    //if (iCount < sCount)
                    //{
                    //    axVSPrinter1.NewPage();
                    //}
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

            sLastNo = StringUtilities.Right(sDay, 1);

            if (Convert.ToInt32(sDay) > 9 && Convert.ToInt32(sDay) < 20)
            {
                sDayth = sDay + "th";
            }
            else
            {
                switch (Convert.ToInt32(sLastNo))
                {
                    case 1: sDayth = sDay + "st"; break;
                    case 2: sDayth = sDay + "nd"; break;
                    case 3: sDayth = sDay + "rd"; break;
                    default: sDayth = sDay + "th"; break;
                }
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

        
    }
}