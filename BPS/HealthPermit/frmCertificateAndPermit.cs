using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.SearchBusiness;
using Amellar.Common.AuditTrail;
using Amellar.Modules.BusinessReports;
using Amellar.Common.TransactionLog;



namespace Amellar.Modules.HealthPermit
{
    public partial class frmCertificateAndPermit : Form
    {
        public frmCertificateAndPermit()
        {
            InitializeComponent();
        }
        

        private string m_sCertPermType = String.Empty;
        public string CertificatePermitType
        {
            set { m_sCertPermType = value; }
        }
        private string m_sBIN = String.Empty;
        private string m_sIssuedDate = String.Empty;
        private string m_sPermitNumber = String.Empty;

        private string m_sORNO = String.Empty;
        private string m_sFeeAmount = String.Empty;
        private string m_sORDate = String.Empty;
        private string m_sIssuedon = String.Empty;

   
       
        string TimeIN = string.Empty;

        private void frmCertificateAndPermit_Load(object sender, EventArgs e)
        {
            bin1.Focus();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;

            if (m_sCertPermType == "Zoning")
                this.Text = "Zoning Certification";
            else if (m_sCertPermType == "Annual Inspection")
                this.Text = "Annual Building Inspection Certification";
            else if (m_sCertPermType == "Sanitary")
                this.Text = "Sanitary Permit";
            //JHB 20191217 add Barangay Clearance (s)
            else if (m_sCertPermType == "Barangay Clearance")
            {
                this.Text = "Barangay Clearance";
               // rdoRenewal.Text = "NEW/REN";
               // rdoNew.Text = "TEMP.BIN";
                btnPrint2.Visible = false;
            }
            //JHB 20191217 add Barangay Clearance (e)
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckofPermitRecord()
        {
            m_sIssuedDate = "";
            #region GetNextSeries
            OracleResultSet result = new OracleResultSet();
            String sQuery = String.Empty;
            
            result.Query = "select coalesce(max(to_number(permit_number)),0)+1 as permit_number from permit_type where perm_type = '" + m_sCertPermType + "' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    if (m_sCertPermType == "Sanitary")
                       // m_sPermitNumber = AppSettingsManager.GetConfigValue("12") + "-" + result.GetInt(0).ToString("0000");
                        m_sPermitNumber = result.GetInt(0).ToString("0000");
                    if (m_sCertPermType == "Annual Inspection") 
                        m_sPermitNumber = result.GetInt(0).ToString("0000"); 
                    if (m_sCertPermType == "Barangay Clearance") 
                        m_sPermitNumber = result.GetInt(0).ToString("0000");
                }
            }
            result.Close();
            #endregion
            bool Hasrecord = false;
            #region CheckExist
            //result.Query = "select * from permit_type where bin = '" + m_sBIN + "' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "' and perm_type = 'Sanitary'"; 
            result.Query = "select * from permit_type where bin = '" + m_sBIN + "' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "' and perm_type = '" + m_sCertPermType + "' "; 
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sPermitNumber = result.GetString(0) + "-" + result.GetString(2);
                    m_sIssuedDate = result.GetString(3);
                    
                }
            }
            int intCount = 0;
            int.TryParse(result.ExecuteScalar(), out intCount);
            if (intCount == 0)
                Hasrecord = false;
            else
                Hasrecord = true;


            result.Close();
            #endregion
            #region Saving 

             if (Hasrecord == false)
            {
                string sCurrentYear = AppSettingsManager.GetConfigValue("12");
                string sPermType = m_sCertPermType;
               // string sPermitNumber = m_sPermitNumber.Substring(5);
                string sPermitNumber = m_sPermitNumber;
                string sIssuedDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                string sUserCode = AppSettingsManager.SystemUser.UserCode;
                string s_mBin = m_sBIN;

                result.Query = "insert into permit_type (current_year,perm_type,permit_number,issued_date,user_code,bin) values('" + sCurrentYear + "', '" + sPermType + "', '" + sPermitNumber + "', '" + sIssuedDate + "', '" + sUserCode + "', '" + s_mBin + "')";
                result.ExecuteNonQuery();
            }
            #endregion
        }

     
        

        private bool CheckifPaid(string sFormat)
        {
            OracleResultSet pSet = new OracleResultSet();
            if (sFormat == "Sanitary")
            {
                pSet.Query = @"select fees_amtdue,P.or_no,P.or_date from or_table OT 
                inner join tax_and_fees_table TFT on TFT.fees_code = OT.fees_codes
                inner join pay_hist P on P.or_no = OT.or_no
                where fees_desc like '%SANITARY PERMIT%' and P.bin = '" + m_sBIN + "' and P.data_mode <> 'UNP' and P.tax_year = '" + AppSettingsManager.GetConfigValue("07") + "'";
            }
            else if (sFormat == "Annual")
            {
                pSet.Query = @"select fees_amtdue,P.or_no,P.or_date from or_table OT 
                inner join tax_and_fees_table TFT on TFT.fees_code = OT.fees_code
                inner join pay_hist P on P.or_no = OT.or_no
                where fees_desc like '%ANNUAL%' and P.bin = '" + m_sBIN + "' and P.data_mode <> 'UNP' and P.tax_year = '" + AppSettingsManager.GetConfigValue("07") + "'";
            }
            else if (sFormat == "Zoning")
            {
                pSet.Query = @"select fees_amtdue,P.or_no,P.or_date from or_table OT 
                inner join tax_and_fees_table TFT on TFT.fees_code = OT.fees_code
                inner join pay_hist P on P.or_no = OT.or_no
                where fees_desc like '%ZONING%' and P.bin = '" + m_sBIN + "' and P.data_mode <> 'UNP' and P.tax_year = '" + AppSettingsManager.GetConfigValue("07") + "'";
            }
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sFeeAmount = pSet.GetInt(0).ToString();
                    m_sORNO = pSet.GetString(1);
                    m_sORDate = pSet.GetDateTime(2).ToString("MM/dd/yyyy");
                }
                else
                {
                    MessageBox.Show("Not yet Paid", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            pSet.Close();
            return true;
        }

        private void btnPrint_Click(object sender, EventArgs e)//JHB 201912 brgy clearance
        {

           // AuditTrail.InsertTrail("ABBC", "Barangay Clearance", "PRINT " + m_sCertPermType + " bin: " + m_sBIN + " ");
            AuditTrail.InsertTrail("ABBC",Text, "PRINT " + m_sCertPermType + " bin: " + m_sBIN + " ");
            TimeIN = AppSettingsManager.GetSystemDate().ToString();  
                if (txtBNSName.Text.Trim() == "")
                {
                    MessageBox.Show("Select BIN first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (rdoRenewal.Checked == true)
                {
                    if (bin1.txtTaxYear.Text.ToString() == "" || bin1.txtBINSeries.Text.ToString() == "")
                    {
                        MessageBox.Show("Select BIN first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    m_sBIN = bin1.GetBin();
                }

                if (rdoNew.Checked == true)
                {
                    if (txtTmpBin.Text.Trim() == "")
                    {
                        MessageBox.Show("Select temporary BIN first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    m_sBIN = txtTmpBin.Text.Trim();
                }
                // RMC 20141222 modified permit printing (e)

            

                CheckofPermitRecord();// generate permit number

                string sBnsStat = "";
                OracleResultSet pSet = new OracleResultSet(); //JARS 20170120 TO CHECK IF RETIRED
                pSet.Query = "select bns_stat from businesses where bin = '" + m_sBIN + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sBnsStat = pSet.GetString(0);
                pSet.Close();
                if (sBnsStat == "RET")
                {
                    if (MessageBox.Show(m_sBIN + " is RETIRED, Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        btnSearch.Text = "Search";  // RMC 20150105 mods in permit printing
                        ClearControls();
                        return;
                    }

                }
                //JHB 20191217 bgy clearance (s)
                if (this.Text == "Barangay Clearance")
                {

                    frmCertPayments formP = new frmCertPayments();
                    formP.ReportSwitch = m_sCertPermType;
                 

                    //JHB 20200108 has record (s)

                    OracleResultSet pSet1 = new OracleResultSet();
                    pSet1.Query = "select * from brgy_clearance where bin = '" + m_sBIN + "' and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
                    if (pSet1.Execute())
                    {
                        if (pSet1.Read())
                        {
                            string cert = pSet1.GetString("CTC_ISSUED_ON");
                            formP.dtpDate.Text = pSet1.GetString("CTC_ISSUED_AT");
                            formP.txtOrNo.Text = pSet1.GetString("CTC_NO");
                            formP.txtAmount.Text = pSet1.GetString("CTC_AMT");
                            formP.txtIssuedON.Text = pSet1.GetString("CTC_ISSUED_ON");
                        }

                        else
                        {
                            if (MessageBox.Show("Add new Record for Barangay Clearance for Business Permit?", "Barangay Clearance for Business Permit Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return;
                            }
                        }
                    }

                    pSet1.Close();

                    //JHB 20200108 has record (e)
                    formP.ShowDialog();


                    m_sORDate = string.Format("{0:MM/dd/yyyy}", formP.dtpDate.Value);
                    m_sORNO = formP.txtOrNo.Text.Trim();
                    m_sFeeAmount = formP.txtAmount.Text.Trim();
                    m_sIssuedon = formP.txtIssuedON.Text.Trim();

                    if (formP.Closed)
                    {
                        MessageBox.Show("Transaction cancelled", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }

              
            
                frmPrinting frmPrinting = new frmPrinting();
                if (m_sIssuedDate == "")
                    m_sIssuedDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyy");
                //frmPrinting.IssuedDate = m_sIssuedDate;   // RMC 20141222 modified permit printing
                frmPrinting.PermitNo = m_sPermitNumber;
                frmPrinting.BIN = m_sBIN;
                frmPrinting.TaxYear = AppSettingsManager.GetConfigValue("12");  // RMC 20141222 modified permit printing
                frmPrinting.ReportType = m_sCertPermType;
                frmPrinting.BnsName = txtBNSName.Text;  // RMC 20141222 modified permit printing
                frmPrinting.BnsAdd = txtBNSAdd.Text;    // RMC 20141222 modified permit printing
                frmPrinting.BnsOwn = txtBNSOwner.Text;  // RMC 20141222 modified permit printing

                //JHB 20191217 bgy clearance (s)
                if (this.Text == "Barangay Clearance") 
                {
                frmPrinting.ORDate = m_sORDate;   
                frmPrinting.ORNo = m_sORNO;   
                frmPrinting.FeeAmount = m_sFeeAmount; 
                frmPrinting.IssuedOn = m_sIssuedon;
                frmPrinting.TempBIN = txtTmpBin.Text.Trim();
                }
                //JHB 20191217 bgy clearance (e)
                frmPrinting.m_timeIN = TimeIN;

                frmPrinting.ShowDialog();
           
        }

        private void LoadInfo(string sBin)
        {
            txtBNSName.Text = AppSettingsManager.GetBnsName(sBin);
            txtBNSAdd.Text = AppSettingsManager.GetBnsAddress(sBin);
            txtBNSOwner.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));

            // RMC 20150112 added printing of sanitary for extension bldg (s)
            if (txtBNSName.Text.Trim() != "")
                btnPrint2.Enabled = true;
            // RMC 20150112 added printing of sanitary for extension bldg (e)

        }

        private bool IsRetireBns(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from retired_bns where bin = :1";
            result.AddParameter(":1", sBin.Trim());
            if (result.Execute())
            {
                if (result.Read())
                {
                    if (result.GetString(5) == "RET")
                    {
                        MessageBox.Show("Business is already retired!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }
            }
            result.Close();
            return true;
        }

        private bool IsOnBnsses(string sBin, int iFrmt)
        {
            OracleResultSet result = new OracleResultSet();
            String sQuery = String.Empty;
            if (iFrmt == 0) //NEW (ZONING)
                sQuery = "select * from businesses where bin = '" + sBin.Trim() + "' and bns_stat = 'NEW'";
            else if (iFrmt == 1) //NEW AND REN (ANNUAL AND SANITARY)
                sQuery = "select * from businesses where bin = '" + sBin.Trim() + "' and bns_stat != 'RET'";
            result.Query = sQuery;
            if (result.Execute())
            {
                if (!result.Read())
                {
                    MessageBox.Show("Record not found!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            result.Close();
            return true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "&Search")
            {
                btnSearch.Text = "&Clear";
                string sBin = string.Empty;
                //if (bin1.GetBin() == string.Empty)
                if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "") // RMC 20141222 modified permit printing
                {
                    frmSearchBusiness fSearch = new frmSearchBusiness();
                    fSearch.ShowDialog();
                    sBin = fSearch.sBIN;
                }
                else
                    sBin = bin1.GetBin();

                LoadInfo(sBin.Trim());
                m_sBIN = sBin;
            }
            else
            {
                btnSearch.Text = "&Search";
                txtBNSAdd.Text = string.Empty;
                txtBNSName.Text = string.Empty;
                txtBNSOwner.Text = string.Empty;
                bin1.Focus();
                m_sBIN = "";
                bin1.txtTaxYear.Clear();
                bin1.txtBINSeries.Clear();
            }
        }

        private void rdoRenewal_CheckedChanged(object sender, EventArgs e)
        {
            // RMC 20141222 modified permit printing
            if (rdoRenewal.Checked == true)
            {
                rdoNew.Checked = false;
                btnSearch.Enabled = true;
            }
            else
            {
                btnSearch.Enabled = false;
            }
        }

        private void rdoNew_CheckedChanged(object sender, EventArgs e)
        {
            // RMC 20141222 modified permit printing
            if (rdoNew.Checked == true)
            {
                rdoRenewal.Checked = false;
                btnSearchTmp.Enabled = true;
            }
            else
            {
                btnSearchTmp.Enabled = false;
            }
        }

        private void btnSearchTmp_Click(object sender, EventArgs e)
        {
            // RMC 20141222 modified permit printing
            if (btnSearchTmp.Text == "Search")  // RMC 20150102 mods in permit
            {
                btnSearchTmp.Text = "Clear";    // RMC 20150102 mods in permit
                frmSearchTmp form = new frmSearchTmp();
                form.Permit = "Sanitary";   // RMC 20150102 mods in permit
                form.TaxYear = AppSettingsManager.GetConfigValue("12");
                form.ShowDialog();

                txtTmpBin.Text = form.BIN;
                txtBNSOwner.Text = form.LastName + ", " + form.FirstName;
                if (form.MI.Trim() != "")
                    txtBNSOwner.Text += " " + form.MI + ".";

                OracleResultSet pSet = new OracleResultSet();
                

                //pSet.Query = "select * from emp_names where bin = '" + txtTmpBin.Text + "'";
                // RMC 20150102 mods in permit (s)
                m_sBIN = txtTmpBin.Text;
                pSet.Query = "select * from emp_names where (bin = '" + m_sBIN + "' or temp_bin = '" + m_sBIN + "')";
                // RMC 20150102 mods in permit (e)
                pSet.Query += " and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
                //pSet.Query += " and emp_occupation = 'OWNER'";  // RMC 20150102 mods in permit    // RMC 20150117
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        txtBNSName.Text = pSet.GetString("bns_nm");
                        txtBNSAdd.Text = pSet.GetString("bns_add");

                        // RMC 20150107 (s)
                        txtDTIBnsName.Text = AppSettingsManager.GetDTIName(m_sBIN); 
                        if (txtDTIBnsName.Text == "")
                            txtDTIBnsName.Text = txtBNSName.Text;
                        // RMC 20150107 (e)

                        btnPrint2.Enabled = true;   // RMC 20150112 added printing of sanitary for extension bldg

                        /*
                        // RMC 20150102 mods in permit (s)
                        string sLn = ""; string sFn = ""; string sMi = ""; string sName = "";

                        sLn = pSet.GetString("emp_ln").Trim();
                        sFn = pSet.GetString("emp_fn").Trim();
                        sMi = pSet.GetString("emp_mi").Trim();

                        if (sFn.Trim() == ".")
                            sFn = string.Empty;
                        if (sMi.Trim() == ".")
                            sMi = string.Empty;
                        
                        if (sMi.Trim() == string.Empty)
                            sName = sFn + " " + sLn;
                        else
                            sName = sFn + " " + sMi + ". " + sLn;

                        if (sFn.Trim() == string.Empty && sMi.Trim() == string.Empty)
                            sName = sLn;

                        txtBNSOwner.Text = sName;
                        // RMC 20150102 mods in permit (e)
                         */ // RMC 20150117
                    }
                }
                pSet.Close();

                // RMC 20150117 (s)
                // get owner's name
                string sLn = ""; string sFn = ""; string sMi = ""; string sName = "";

                sLn = AppSettingsManager.GetBnsOwnerLastName(AppSettingsManager.GetBnsOwnCode(m_sBIN));
                sFn = AppSettingsManager.GetBnsOwnerFirstName(AppSettingsManager.GetBnsOwnCode(m_sBIN));
                sMi = AppSettingsManager.GetBnsOwnerMiName(AppSettingsManager.GetBnsOwnCode(m_sBIN));

                if (sLn.Trim() == "")
                {
                    pSet.Query = "select * from emp_names where (bin = '" + m_sBIN + "' or temp_bin = '" + m_sBIN + "')";
                    pSet.Query += " and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
                    pSet.Query += " and emp_occupation = 'OWNER'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            sLn = pSet.GetString("emp_ln").Trim();
                            sFn = pSet.GetString("emp_fn").Trim();
                            sMi = pSet.GetString("emp_mi").Trim();
                        }
                        else
                            sLn = txtDTIBnsName.Text;
                    }
                    pSet.Close();
                }

                if (sFn.Trim() == ".")
                    sFn = string.Empty;
                if (sMi.Trim() == ".")
                    sMi = string.Empty;

                if (sMi.Trim() == string.Empty)
                    sName = sFn + " " + sLn;
                else
                    sName = sFn + " " + sMi + ". " + sLn;

                if (sFn.Trim() == string.Empty && sMi.Trim() == string.Empty)
                    sName = sLn;
                txtBNSOwner.Text = sName;

                // RMC 20150117 (e)

            }
            else
            {
                // RMC 20150102 mods in permit (s)
                btnSearchTmp.Text = "Search";
                ClearControls();
                // RMC 20150102 mods in permit (e)
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            // RMC 20141222 modified permit printing
            //if (btnSearch.Text == "Search BIN")
            if (btnSearch.Text == "Search") // RMC 20150105 mods in permit printing
            {
                //btnSearch.Text = "Clear BIN";
                btnSearch.Text = "Clear";   // RMC 20150105 mods in permit printing

                if (bin1.txtTaxYear.Text == "" || bin1.txtBINSeries.Text == "")
                {
                    frmSearchBusiness fSearch = new frmSearchBusiness();
                    fSearch.ShowDialog();
                    /*m_sBIN = fSearch.sBIN;
                    bin1.txtTaxYear.Text = m_sBIN.Substring(7, 4);
                    bin1.txtBINSeries.Text = m_sBIN.Substring(12, 7);*/

                    // RMC 20150102 mods in permit (s)
                    if (fSearch.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = fSearch.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = fSearch.sBIN.Substring(12, 7).ToString();
                        m_sBIN = bin1.GetBin();
                    }
                    // RMC 20150102 mods in permit (e)
                }
                else
                    m_sBIN = bin1.GetBin();

                LoadInfo(m_sBIN);
                // RMC 20150107 (s)
                txtDTIBnsName.Text = AppSettingsManager.GetDTIName(m_sBIN);
                if (txtDTIBnsName.Text == "")
                    txtDTIBnsName.Text = txtBNSName.Text;
                // RMC 20150107 (e)

                //LoadInfo(m_sBIN);
                //// RMC 20150107 (s)
                //txtDTIBnsName.Text = AppSettingsManager.GetDTIName(m_sBIN); 
                //if (txtDTIBnsName.Text == "")
                //    txtDTIBnsName.Text = txtBNSName.Text;
                //// RMC 20150107 (e)
            }
            else
            {
                //btnSearch.Text = "Search BIN";
                btnSearch.Text = "Search";  // RMC 20150105 mods in permit printing
                ClearControls();
            }
        }

        private void ClearControls()
        {
            txtBNSName.Text = "";
            txtBNSAdd.Text = "";
            txtBNSOwner.Text = "";
            bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";
            txtTmpBin.Text = "";
            txtDTIBnsName.Text = "";
            m_sBIN = "";    // RMC 20150102 mods in permit
            btnPrint2.Enabled = false;  // RMC 20150112 added printing of sanitary for extension bldg
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            frmPrintExt form = new frmPrintExt();
            form.BIN = m_sBIN;
            form.BnsName = txtBNSName.Text;
            form.BnsAdd = txtBNSAdd.Text;
            form.Owner = txtBNSOwner.Text;
            form.ShowDialog();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}