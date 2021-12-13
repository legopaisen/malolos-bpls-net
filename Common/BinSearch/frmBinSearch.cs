
// RMC 20120418 modified re-printing of business info if with existing application
// RMC 20120117 disable printing of application form if record on-hold
// RMC 20120109 trap error if bns code is empty before printing application form
// RMC 20111227 Added validation in printing renewal form
// RMC 20111214 Separate menu for Application form printing
// RMC 20111214 added viewing of Blob in BinSearch
// RMC 20110311

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.Reports;
using Amellar.Common.SearchBusiness;
using Amellar.Common.BPLSApp;
using Amellar.Common.ImageViewer;
using Amellar.Common.frmBns_Rec;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using Amellar.Modules.HealthPermit;

namespace Amellar.Common.BinSearch
{
    public partial class frmBinSearch : Form
    {
        private bool bIsRePrint = false;
        private string m_strModule = string.Empty;  // RMC 20110311
        // blob
        protected frmImageList m_frmImageList;
        public static int m_intImageListInstance;
        // blob
        protected frmBusinessRecord m_frmBussRecord;    // RMC 20111214 Separate menu for Application form printing
        protected bool m_bAdvancePrinting = false;      // RMC 20161228 adjust tax year in form for advance printing

        public string ModuleCode
        {
            get { return m_strModule; }
            set { m_strModule = value; }
        }   // RMC 20110311

        public frmBinSearch()
        {
       
           InitializeComponent();
           // blob
           m_intImageListInstance = 0;
           m_frmImageList = new frmImageList();
           m_frmImageList.IsBuildUpPosting = true;
           // blob
        }

        private void frmBinSearch_Load(object sender, EventArgs e)
        {
            //bin1.GetLGUCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.LGUCode);
            //bin1.GetDistCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.DistCode);
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;  // RMC 20110311 
            bin1.GetDistCode = ConfigurationAttributes.DistCode;    // RMC 20110311 

            if (m_strModule == "REN-APP-FORM")
            {
                kryptonHeader1.Text = "Application Form";
                btnBnsInfo.Text = "Print form";
            }
            else if (m_strModule == "APP-TRAIL")
            {
                btnBnsInfo.Text = "Approval Trail";
                btn_viewPermit.Enabled = false;
                btnViewDocu.Enabled = false;
            }
            else
            {
                kryptonHeader1.Text = "Search BIN";
                btnBnsInfo.Text = "Business Info";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_frmImageList.Close();
            this.Close();
        }

        private void AppPermitNo(out string sAppDate, out string sAppNo)//MCR 20150108
        {
            sAppNo = "";
            sAppDate = "";
            int iAppNo = 0;
            OracleResultSet pSet = new OracleResultSet();
            //If Exist
            pSet.Query = "select * from app_permit_no where bin = '" + bin1.GetBin() + "' and year = '" + AppSettingsManager.GetSystemDate().Year + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sAppNo = pSet.GetString(1);
                    sAppDate = pSet.GetString(3);
                }
                else  //JARS 20170825
                {
                    pSet.Close();
                    pSet.Query = "select count(*) as app from (select distinct(app_no) from app_permit_no)";
                    if(pSet.Execute())
                    {
                        if(pSet.Read())
                        {
                            iAppNo = pSet.GetInt("app");
                            iAppNo++;
                            sAppNo = iAppNo.ToString().PadLeft(5,'0');

                            sAppNo = AppSettingsManager.GetSystemDate().Year + "-" + sAppNo;
                        }
                    }
                    pSet.Close();
                    pSet.Query = "select save_tm from business_que where bin = '"+ bin1.GetBin() +"'";
                    if(pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            sAppDate = pSet.GetDateTime("save_tm").ToString("mm/dd/yyyy");
                        }
                        else
                        {
                            
                            //pSet.Close();
                            //pSet.Query = "select save_tm from businesses where bin = '" + bin1.GetBin() + "'";
                            //if(pSet.Execute())
                            //{
                            //    if(pSet.Read())
                            //    {
                            //        sAppDate = pSet.GetDateTime("save_tm").ToString("mm/dd/yyyy");
                            //    }
                            //}
                            sAppDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                        }
                    }
                    pSet.Close();
                }
            }
            pSet.Close();

            //if (sAppNo == "")
            //{
            //    //Get MaxSeries
            //    pSet.Query = "select coalesce(max(to_number(substr(app_no,6,10))),0) + 1 as Series from app_permit_no where year = '" + AppSettingsManager.GetSystemDate().Year + "'";
            //    if (pSet.Execute())
            //        if (pSet.Read())
            //            sAppNo = AppSettingsManager.GetSystemDate().Year.ToString() + "-" + pSet.GetInt(0).ToString("00000");
            //    pSet.Close();
            //}
            
            ////Save if not exist
            //if (sAppDate == "")
            //{
            //    pSet.Query = @"insert into app_permit_no values ('" + AppSettingsManager.GetSystemDate().Year.ToString() + "','" + sAppNo + "','" + bin1.GetBin().Trim() + "','" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy") + "')";
            //    pSet.ExecuteNonQuery();
            //}
        }

        private void btnBnsInfo_Click(object sender, EventArgs e)
        {
            // RMC 20111214 Separate menu for Application form printing (s)
            if (m_strModule == "REN-APP-FORM")
            {
                if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
                {
                    //if (IsAlreadyPrint()) // GDE 20130108 added additional prompt if application form already been printed.
                    //{
                    if (ValidateBIN())
                    {
                        string sAppDate = "";
                        string sAppNo = "";
                        AppPermitNo(out sAppDate, out sAppNo);

                        frmAppForm PrintAppForm = new frmAppForm();
                        PrintAppForm.BIN = bin1.GetBin();
                        PrintAppForm.AppNo = sAppNo; //MCR 20150108
                        PrintAppForm.AppDate = sAppDate;//MCR 20150108
                        PrintAppForm.ApplType = AppSettingsManager.GetBnsStatonBnsQue(bin1.GetBin().Trim());
                        PrintAppForm.bIsRePrint = bIsRePrint;
                        PrintAppForm.m_bAdvancePrintForm = m_bAdvancePrinting;   // RMC 20161228 adjust tax year in form for advance printing
                        PrintAppForm.ShowDialog();
                    }
                    //}
                }
            }   // RMC 20111214 Separate menu for Application form printing (e)
            //JARS 20170320 (s) CERTIFICATION OF STATUS
            else if(m_strModule == "CERT-STAT")
            {
                frmAppForm PrintAppForm = new frmAppForm();
                PrintAppForm.ApplType = "CERT-STAT";
                PrintAppForm.BIN = bin1.GetBin();
                PrintAppForm.ShowDialog();
            }
            //JARS 20170320
            else if (m_strModule == "APP-TRAIL")
            {
                frmPrinting listform = new frmPrinting();
                listform.ReportType = "ApprovalTrail";
                listform.BIN = bin1.GetBin();
                listform.TaxYear = ConfigurationAttributes.CurrentYear;
                listform.BnsName = AppSettingsManager.GetBnsName(bin1.GetBin());
                listform.BnsAdd = AppSettingsManager.GetBnsAddress(bin1.GetBin());
                listform.ShowDialog();
            }
            else
            {
                ReportClass rClass = new ReportClass();
                string sBIN;
                sBIN = bin1.GetBin();

                // RMC 20120418 modified re-printing of business info if with existing application (s)
                if (!ValidateTransaction())
                    return;
                // RMC 20120418 modified re-printing of business info if with existing application (e)

                // RMC 20110311 (s)
                BPLSAppSettingList sList = new BPLSAppSettingList();
                sList.ModuleCode = m_strModule;
                sList.ReturnValueByBin = sBIN;
                if (sList.BPLSAppSettings.Count == 0)
                {
                    MessageBox.Show("Record not found.", "Business Records View", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                // RMC 20110311 (e)

                /*rClass.iReport = 1;
                rClass.ModuleCode = m_strModule;
                rClass.BusinessRecord(sBIN);
                rClass.PreviewDocu();*/
                // RMC 20171127 transferred printing of business record view to vsprinter, put rem

                // RMC 20171127 transferred printing of business record view to vsprinter (s)
                frmAppForm PrintAppForm = new frmAppForm();
                PrintAppForm.BIN = bin1.GetBin();
                PrintAppForm.ModuleCode = m_strModule;
                PrintAppForm.ApplType = "RECORD-VIEW";
                PrintAppForm.ShowDialog();
                // RMC 20171127 transferred printing of business record view to vsprinter (e)
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // RMC 20110311 
            frmSearchBusiness frmSearchBns = new frmSearchBusiness();
            frmSearchBns.ModuleCode = m_strModule;
            frmSearchBns.ShowDialog();

            if (frmSearchBns.sBIN.Length > 1)
            {
                bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
            }
            else
            {
                bin1.txtTaxYear.Text = "";
                bin1.txtBINSeries.Text = "";
            }
        }

        private void btnViewDocu_Click(object sender, EventArgs e)
        {
            // RMC 20111214 added viewing of Blob in BinSearch
            if (m_frmImageList.IsDisposed)
            {
                m_intImageListInstance = 0;
                m_frmImageList = new frmImageList();
                m_frmImageList.IsBuildUpPosting = true;
            }
            if (!m_frmImageList.IsDisposed && m_intImageListInstance == 0)
            {
                //if (m_frmImageList.ValidateImage(bin1.GetBin(), "A"))
                if (m_frmImageList.ValidateImage(bin1.GetBin(), AppSettingsManager.GetSystemType)) //MCR 20141209
                {
                    ImageInfo objImageInfo;
                    objImageInfo = new ImageInfo();

                    objImageInfo.TRN = bin1.GetBin();
                    //objImageInfo.System = "A";
                    objImageInfo.System = AppSettingsManager.GetSystemType; //MCR 20141209
                    m_frmImageList.isFortagging = false;
                    m_frmImageList.setImageInfo(objImageInfo);
                    m_frmImageList.Text = bin1.GetBin();
                    m_frmImageList.IsAutoDisplay = true;
                    m_frmImageList.Source = "VIEW";
                    m_frmImageList.Show();
                    m_intImageListInstance += 1;
                }
                else
                {

                    MessageBox.Show(string.Format("BIN {0} has no image", bin1.GetBin()));
                }

            }
        }

        private bool IsAlreadyPrint()
        {
            OracleResultSet result = new OracleResultSet();
            bool bResult = false;
            result.Query = "select * from APPL_FORM_PRINT where bin = :1 and tax_year = :2";
            result.AddParameter(":1", bin1.GetBin());
            result.AddParameter(":2", ConfigurationAttributes.CurrentYear);
            if (result.Execute())
            {
                if (result.Read())
                    bResult = false;
                else
                    bResult = true;
            }
            result.Close();

            if (bResult == false)
            {
                if (MessageBox.Show("Do you want to re-print this form?", "Re-print Application Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bResult = true;
                }
                else
                    bResult = false;
            }

            if (bResult)
            {
                result.Query = "delete from APPL_FORM_PRINT where bin = :1 and tax_year = :2";
                result.AddParameter(":1", bin1.GetBin());
                result.AddParameter(":2", ConfigurationAttributes.CurrentYear);
                if (result.ExecuteNonQuery() != 0)
                { }
                result.Close();


                result.Query = "insert into APPL_FORM_PRINT values (:1, :2)";
                result.AddParameter(":1", bin1.GetBin());
                result.AddParameter(":2", ConfigurationAttributes.CurrentYear);
                if (result.ExecuteNonQuery() != 0)
                { }
                result.Close();
            }

            return bResult;
        }

        private bool ValidateBIN()
        {
            // RMC 20111214 Separate menu for Application form printing
            m_frmBussRecord = new frmBusinessRecord();
            m_bAdvancePrinting = false;      // RMC 20161228 adjust tax year in form for advance printing

            /*
            // RMC 20161129 customized application form for Binan (s)
            if (AppSettingsManager.GetConfigValue("10") == "243")
            {
                if (!m_frmBussRecord.DupCheck(string.Format("business_que where bin = '{0}'", bin1.GetBin())))
                {
                    MessageBox.Show("No application found.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            // RMC 20161129 customized application form for Binan (e)
             */
            // RMC 20161208 removed printing of application after renewal application based on training

            //MCR 20111121 (s)
            string sValue = AppSettingsManager.GetNigViolist(bin1.GetBin());
            if (sValue != "")
                MessageBox.Show("Record was tagged in negative list\n\n" + sValue, "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //MCR 20111121 (e)

            if (!m_frmBussRecord.DupCheck(string.Format("business_que where bin = '{0}'", bin1.GetBin())))//businesses
            {
                if (!m_frmBussRecord.DupCheck(string.Format("businesses where bin = '{0}'", bin1.GetBin())))
                {
                    MessageBox.Show("No record found.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            if (m_frmBussRecord.DupCheck(string.Format("business_que where bin = '{0}' and bns_stat = 'RET'", bin1.GetBin())))//businesses
            {
                MessageBox.Show("Business already been retired.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (m_frmBussRecord.DupCheck(string.Format("businesses where bin = '{0}' and bns_stat = 'RET'", bin1.GetBin())))
            {
                MessageBox.Show("Business already been retired.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (m_frmBussRecord.DupCheck(string.Format("business_que where bin = '{0}' and tax_year = '{1}'", bin1.GetBin(), ConfigurationAttributes.CurrentYear)))//businesses
            {
                MessageBox.Show("BIN already been applied for current year.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //return false; // GDE 20121227
                if (MessageBox.Show("Do you want to re-print this form?", "Re-print Application Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bIsRePrint = true;
                    return true;

                }
                else
                {
                    // RMC 20161228 adjust tax year in form for advance printing (s)
                    if (MessageBox.Show("Print form in advance?", "Re-print Application Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bIsRePrint = false;
                        m_bAdvancePrinting = true;      // RMC 20161228 adjust tax year in form for advance printing
                        return true;
                    }// RMC 20161228 adjust tax year in form for advance printing (e)
                    else
                    {
                        bIsRePrint = false;
                        return false;
                    }
                }
            }
            else if (m_frmBussRecord.DupCheck(string.Format("businesses where bin = '{0}' and tax_year = '{1}'", bin1.GetBin(), ConfigurationAttributes.CurrentYear)))//businesses
            {
                MessageBox.Show("BIN already been applied for current year.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //return false; // GDE 20121227
                if (MessageBox.Show("Do you want to re-print this form?", "Re-print Application Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bIsRePrint = true;
                    return true;

                }
                else
                {
                    // RMC 20161228 adjust tax year in form for advance printing (s)
                    if (MessageBox.Show("Print form in advance?", "Re-print Application Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bIsRePrint = false;
                        m_bAdvancePrinting = true;      // RMC 20161228 adjust tax year in form for advance printing
                        return true;
                    }// RMC 20161228 adjust tax year in form for advance printing (e)
                    else
                    {
                        bIsRePrint = false;
                        return false;
                    }
                }
            }

            if (m_frmBussRecord.DupCheck(string.Format("business_que where bin = '{0}' and tax_year = '{1}' and bns_stat = 'REN'", bin1.GetBin(), ConfigurationAttributes.CurrentYear)))
            {
                MessageBox.Show("BIN already been applied for renewal.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (MessageBox.Show("Do you want to re-print this form?", "Re-print Application Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bIsRePrint = true;
                    return true;
                    
                }
                else
                {
                    // RMC 20161228 adjust tax year in form for advance printing (s)
                    if (MessageBox.Show("Print form in advance?", "Re-print Application Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bIsRePrint = false;
                        m_bAdvancePrinting = true;      // RMC 20161228 adjust tax year in form for advance printing
                        return true;
                    }// RMC 20161228 adjust tax year in form for advance printing (e)
                    else
                    {
                        bIsRePrint = false;
                        return false;
                    }
                }
            }

            if (m_frmBussRecord.DupCheck(string.Format("business_que where bin = '{0}' and bns_stat = 'RET'", bin1.GetBin())))
            {
                MessageBox.Show("Has a pending Retirement Application.Please check it first.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (m_frmBussRecord.DupCheck(string.Format("businesses where bin = '{0}' and bns_stat = 'RET'", bin1.GetBin())))
            {
                MessageBox.Show("Has a pending Retirement Application.Please check it first.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (m_frmBussRecord.DupCheck(string.Format("pay_hist where bin = '{0}' and data_mode = 'UNP'", bin1.GetBin())))
            {
                MessageBox.Show("Cannot proceed to application, incomplete payments detected.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (m_frmBussRecord.DupCheck(string.Format("official_closure_tagging where bin = '{0}'", bin1.GetBin())))
            {
                MessageBox.Show("Record was tagged for closure. Cannot continue.", "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (m_frmBussRecord.DupCheck(string.Format("permit_update_appl where bin = '{0}' and data_mode = 'QUE'", bin1.GetBin())))
            {
                MessageBox.Show("Business has Permit Update Application.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            // RMC 20111227 Added validation in printing renewal form (s)
            if (m_frmBussRecord.DupCheck(string.Format("taxdues where bin = '{0}' and tax_year < '{1}'", bin1.GetBin(), ConfigurationAttributes.CurrentYear)))
            {
                if (MessageBox.Show("Has pending tax dues. Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    MessageBox.Show("Settle account due first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                
            }

            if (m_frmBussRecord.DupCheck(string.Format("pay_hist where bin = '{0}' and data_mode = 'UNP'", bin1.GetBin())))
            {
                if (MessageBox.Show("Has pending tax dues. Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    MessageBox.Show("Cannot proceed to application, incomplete payments detected.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            if (m_frmBussRecord.DupCheck(string.Format("payment_conflict where bin = '{0}'", bin1.GetBin())))
            {
                if (MessageBox.Show("Has payments conflict. Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    MessageBox.Show("Cannot proceed to application, incomplete payments detected.\nVerify at TAC.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            // RMC 20111227 Added validation in printing renewal form (e)

            // RMC 20120109 trap error if bns code is empty before printing application form (s)
            if (m_frmBussRecord.DupCheck(string.Format("businesses where bin = '{0}' and trim(bns_code) is null", bin1.GetBin())))
            {
                MessageBox.Show("Please update business type fist", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            // RMC 20120109 trap error if bns code is empty before printing application form (e)

            // RMC 20120117 disable printing of application form if record on-hold (s)
            OracleResultSet pSet = new OracleResultSet();
            string strHRUser = "";
            string strHRDate = "";
            string strHRRemarks = "";
            string strMess = "";
            pSet.Query = string.Format("select * from hold_records where bin = '{0}' and status = 'HOLD'", bin1.GetBin());
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    strHRUser = pSet.GetString("user_code");
                    strHRDate = pSet.GetString("dt_save");
                    strHRRemarks = pSet.GetString("remarks");
                    pSet.Close();
                    strMess = "Cannot print Application form! This record is currently on hold.\nUser Code: " + strHRUser + "  Date: " + strHRDate + "\nRemarks: " + strHRRemarks;
                    MessageBox.Show(strMess, "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            // RMC 20120117 disable printing of application form if record on-hold (e)

            return true;
        }

        private bool ValidateTransaction()
        {
            // RMC 20120418 modified re-printing of business info if with existing application

            OracleResultSet pRecord = new OracleResultSet();
            string sBnsStat = "";


            pRecord.Query = "select * from business_que where bin = '" + bin1.GetBin() + "'";
            if (pRecord.Execute())
            {
                if (pRecord.Read())
                {
                    sBnsStat = pRecord.GetString("bns_stat");

                    if (sBnsStat == "REN")
                        sBnsStat = "RENEWAL";

                    if (sBnsStat == "RENEWAL" && m_strModule == "NEW-APP-VIEW")
                    {
                        MessageBox.Show("BIN has existing " + sBnsStat + " application.\nView business record under Application Menu-" + sBnsStat + "", "Business Records View", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                    if (sBnsStat == "NEW" && m_strModule == "REN-APP-VIEW")
                    {
                        MessageBox.Show("BIN has existing " + sBnsStat + " application.\nView business record under Application Menu-" + sBnsStat + "", "Business Records View", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }

                    if (m_strModule != "NEW-APP-VIEW" && m_strModule != "REN-APP-VIEW")
                    {
                        MessageBox.Show("BIN has existing " + sBnsStat + " application.\nView business record under Application Menu-" + sBnsStat + "", "Business Records View", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }
                else
                    return true;
            }
            pRecord.Close();

            

            return true;
        }

        private void btn_viewPermit_Click(object sender, EventArgs e)
        {
        if (m_strModule == "REN-APP-FORM")
            {
                if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
                {
                    if (ValidateBIN())
                    {
                        string sAppDate = "";
                        string sAppNo = "";
                        AppPermitNo(out sAppDate, out sAppNo);

                        frmAppForm PrintAppForm = new frmAppForm();
                        PrintAppForm.BIN = bin1.GetBin();
                        PrintAppForm.AppNo = sAppNo; 
                        PrintAppForm.AppDate = sAppDate;
                        PrintAppForm.ApplType = AppSettingsManager.GetBnsStatonBnsQue(bin1.GetBin().Trim());
                        PrintAppForm.bIsRePrint = bIsRePrint;
                        PrintAppForm.m_bAdvancePrintForm = m_bAdvancePrinting;   
                        PrintAppForm.ShowDialog();
                    }
                   
                }
            }   

            else if (m_strModule == "CERT-STAT")
            {
                frmAppForm PrintAppForm = new frmAppForm();
                PrintAppForm.ApplType = "CERT-STAT";
                PrintAppForm.BIN = bin1.GetBin();
                PrintAppForm.ShowDialog();
            }
              else
            {
                ReportClass rClass = new ReportClass();
                string sBIN;
                sBIN = bin1.GetBin();

                if (!ValidateTransaction())
                    return;
                BPLSAppSettingList sList = new BPLSAppSettingList();
                sList.ModuleCode = m_strModule;
                sList.ReturnValueByBin = sBIN;
                if (sList.BPLSAppSettings.Count == 0)
                {
                    MessageBox.Show("Record not found.", "Business Records View", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            
                frmAppForm PrintAppForm = new frmAppForm();
                PrintAppForm.BIN = bin1.GetBin();
                PrintAppForm.ModuleCode = m_strModule;
                PrintAppForm.ApplType = "PERMIT-VIEW";
                PrintAppForm.ShowDialog();
              
            }
        }
    }
}