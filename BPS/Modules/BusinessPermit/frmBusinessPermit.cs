
// RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ
// RMC 20120130 disable generation/printing of permit for hold records 
// RMC 20120124 enable generation of permit for Peddler but no printing
// RMC 20120124 added printing of remarks in Permit (user-request)
// RMC 20120117 disable generation of permit for PEDDLERS
// RMC 20111229 transferred enabling of checklist button
// RMC 20111219 added mods in generation of MP series
// RMC 20110908 Corrected error changed pSet to pRec
// RMC 20110905 added validation of requirements in generating permit
// RMC 20110826 added validation plate no.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.SearchBusiness;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Modules.ApplicationRequirements;
using Amellar.Common.TransactionLog;

namespace Amellar.Modules.BusinessPermit
{
    public partial class frmBusinessPermit : Form
    {
        //OracleResultSet pSet = new OracleResultSet(); // RMC 20110908 Corrected error changed
        private PrintPermit PrintClass = null;
        private int m_iMPSwitch;
        private string m_sMainPermitNo = string.Empty;
        private string m_sFirstOwnCode = string.Empty;
        private string m_sMode = string.Empty;
        private string m_sReportName = string.Empty;
        private double m_dAmountPaid = 0;
        private bool m_bIsPUT = false;
        private bool m_bIsRenPUT = false;
        private bool m_bNewAddl = false;
        private bool m_bChClass = false;
        private bool m_bTransOwn = false;
        private bool m_bTransLoc = false;
        private string m_sOrgKind = string.Empty;
        private string m_sInitMemo = string.Empty;
        private string m_sTmpBnsName = string.Empty;
        private DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
        private string m_sTempList = string.Empty;
        private string m_sTblSource = string.Empty;

        public frmBusinessPermit()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private void frmBusinessPermit_Load(object sender, EventArgs e)
        {
            m_iMPSwitch = 1;
            btnUpdate.Enabled = false;
            btnGenerate.Enabled = false;
            //btnCancel.Text = "Cancel";
            bin1.txtTaxYear.Focus();
            this.ActiveControl = bin1.txtTaxYear;
            ClearGrid();
            btnCheckList.Visible = false;  // RMC 20110905

            // RMC 20170104 hide control of cert of registation in Permit module for Binan (s)
            if (AppSettingsManager.GetConfigValue("10") == "243")
                btnPrint.Visible = false;
            // RMC 20170104 hide control of cert of registation in Permit module for Binan (e)
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet(); // RMC 20110908 Corrected error changed
            OracleResultSet pSet2 = new OracleResultSet();

            frmSearchBusiness frmSearchBns = new frmSearchBusiness();

            if (btnSearch.Text == "Clear")
            {
                ClearControls();
                this.btnCancel.Text = "Close";
            }
            else
            {
                if (bin1.txtTaxYear.Text.Trim() == "" && bin1.txtBINSeries.Text.Trim() == "")
                {
                    frmSearchBns.ShowDialog();

                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                        RetrieveRecords();
                    }
                }
                else
                {
                    pSet.Query = string.Format("select * from businesses where bin = '{0}'", bin1.GetBin());
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            RetrieveRecords();
                        }
                        else if (!pSet.Read())
                        {
                            m_sTblSource = "QUE";
                            pSet2.Query = string.Format("select * from business_que where bin = '{0}'", bin1.GetBin()); //AFM 20201006 allow viewing of bns permit for unpaid
                            if(pSet2.Execute())
                                if(pSet2.Read())
                                    RetrieveRecords();
                        }
                        else
                        {
                            frmSearchBns.ShowDialog();

                            if (frmSearchBns.sBIN.Length > 1)
                            {
                                bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                                bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                                RetrieveRecords();
                            }
                            else
                            {
                                bin1.txtTaxYear.Text = "";
                                bin1.txtBINSeries.Text = "";
                            }
                        }
                    }
                    pSet.Close();
                    pSet2.Close();

                }
            }
        }

        private void RetrieveRecords()
        {
            OracleResultSet pSet = new OracleResultSet(); // RMC 20110908 Corrected error changed
            OracleResultSet pSet2 = new OracleResultSet();

            string sOwnCode = string.Empty;
            string sMPDate = string.Empty;
            DateTime dtpMPDate;

            m_sOrgKind = "";

            m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
            

            pSet.Query = string.Format("select * from businesses where bin = '{0}'", bin1.GetBin());
            if (pSet.Execute())
            {
                if (pSet.Read())
	            {
                    txtBnsName.Text = pSet.GetString("bns_nm").Trim();
                    sOwnCode = pSet.GetString("own_code").Trim();
                    cmbPermitNo.Items.Add(pSet.GetString("permit_no").Trim());
			        m_sMainPermitNo = cmbPermitNo.Text;
                    txtTaxYear.Text = pSet.GetString("tax_year").Trim();
                    txtTelNo.Text = pSet.GetString("bns_telno").Trim();
                    txtEmployee.Text = string.Format("{0:##}", pSet.GetInt("num_employees"));
                    txtBnsStat.Text = pSet.GetString("bns_stat").Trim();
                    txtBnsCode.Text = pSet.GetString("bns_code").Trim();
                    txtRemarks.Text = pSet.GetString("memoranda").Trim();
                    
                    dtpMPDate = pSet.GetDateTime("permit_dt");
			        txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(bin1.GetBin());
			        txtOwnName.Text = AppSettingsManager.GetBnsOwner(sOwnCode);
			        txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
			        m_sFirstOwnCode = sOwnCode;
                    cmbPermitNo.Enabled = true;
                    //cmbPermitNo.SelectedIndex = 0;    // RMC 20120228 modified printing of permit with change of line of business and payment adjustment
                    m_sOrgKind = pSet.GetString("orgn_kind");

                    if (m_sOrgKind == "SINGLE PROPRIETORSHIP")
                        m_sOrgKind = "SINGLE";
                    
                    btnSearch.Text = "Clear";
                    btnCheckList.Visible = true;  //AFM 20200106  statement location moved moved for checlist button malolos ver.
                    if(cmbPermitNo.Text.Trim() == "" && Convert.ToInt32(txtTaxYear.Text.ToString()) < Convert.ToInt32(ConfigurationAttributes.CurrentYear))
        			{
				        MessageBox.Show("Current year payment record not found.","Business Permit", MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        return;
			        }

                    // RMC 20110822 (s)
                    if (Convert.ToInt32(txtTaxYear.Text.ToString()) < Convert.ToInt32(ConfigurationAttributes.CurrentYear))
                    {
                        MessageBox.Show("Current year payment record not found.", "Business Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    // RMC 20110822 (e)

                    //btnCheckList.Visible = true;    // RMC 20111229 transferred enabling of checklist button   //AFM 20200106 moved statement location
                }

                else if (!pSet.Read())
                {
                    pSet2.Query = string.Format("select * from business_que where bin = '{0}'", bin1.GetBin()); //AFM 20201006 allow viewing of bns permit for unpaid
                    if(pSet2.Execute())
                        if (pSet2.Read())
                        {
                            txtBnsName.Text = pSet2.GetString("bns_nm").Trim();
                            sOwnCode = pSet2.GetString("own_code").Trim();
                            cmbPermitNo.Items.Add(pSet2.GetString("permit_no").Trim());
                            m_sMainPermitNo = cmbPermitNo.Text;
                            txtTaxYear.Text = pSet2.GetString("tax_year").Trim();
                            txtTelNo.Text = pSet2.GetString("bns_telno").Trim();
                            txtEmployee.Text = string.Format("{0:##}", pSet2.GetInt("num_employees"));
                            txtBnsStat.Text = pSet2.GetString("bns_stat").Trim();
                            txtBnsCode.Text = pSet2.GetString("bns_code").Trim();
                            txtRemarks.Text = pSet2.GetString("memoranda").Trim();

                            dtpMPDate = pSet2.GetDateTime("permit_dt");
                            txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(bin1.GetBin());
                            txtOwnName.Text = AppSettingsManager.GetBnsOwner(sOwnCode);
                            txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                            m_sFirstOwnCode = sOwnCode;
                            cmbPermitNo.Enabled = true;
                            //cmbPermitNo.SelectedIndex = 0;    // RMC 20120228 modified printing of permit with change of line of business and payment adjustment
                            m_sOrgKind = pSet2.GetString("orgn_kind");

                            if (m_sOrgKind == "SINGLE PROPRIETORSHIP")
                                m_sOrgKind = "SINGLE";

                            btnSearch.Text = "Clear";
                            btnCheckList.Visible = true; //AFM 20200106 view checklist even in unpaid. requested by malolos
                        }
                }


                else
                {
                    MessageBox.Show("No Record Found.", "Business Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
			}
            pSet.Close();
            pSet2.Close();

            // RMC 20140106 corrected viewing of no. of employees in permit (s)
           /* string sDefaultCode = string.Empty;

            pSet.Query = "select * from default_code where default_desc like '%OF WORKERS%'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                    sDefaultCode = pSet.GetString("default_code").Trim();
            }
            pSet.Close();
            // RMC 20140106 corrected viewing of no. of employees in permit (e)

            //pSet.Query = string.Format("select * from other_info where bin = '{0}' and tax_year = '{1}' and default_code = '0015'", bin1.GetBin(), txtTaxYear.Text.ToString().Trim());
            pSet.Query = string.Format("select * from other_info where bin = '{0}' and tax_year = '{1}' and default_code = '{2}' and bns_code = '{3}'", bin1.GetBin(), txtTaxYear.Text.ToString().Trim(), sDefaultCode, txtBnsCode.Text);  // RMC 20140106 corrected viewing of no. of employees in permit
            if (pSet.Execute())
            {
                if (pSet.Read())
                    txtEmployee.Text = string.Format("{0:#0}", pSet.GetInt("data"));
	            else
		            txtEmployee.Text = "0";
            }
            pSet.Close();*/
            // RMC 20140114 modified getting of num of employees, put rem

            // RMC 20140114 modified getting of num of employees (s)
            frmPermitNew PrintForm = new frmPermitNew();
            txtEmployee.Text = PrintForm.GetNumEmployees(bin1.GetBin(), "", txtTaxYear.Text);
            // RMC 20140114 modified getting of num of employees (e)
	
            /*if(StringUtilities.Left(txtRemarks.Text.ToString().Trim(),12) != "AS PER LEGAL" 
                && StringUtilities.Left(txtRemarks.Text.ToString().Trim(),7) != "NATURE:")
                txtRemarks.Text = "";
            */
            // RMC 20120124 added printing of remarks in Permit (user-request)

            pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by qtr_paid desc", bin1.GetBin(), txtTaxYear.Text.ToString().Trim()); // RMC 20110823 added desc in ordering
            if (pSet.Execute())
            {
                if (pSet.Read())
	            {
                    dtpORDate.Value = pSet.GetDateTime("or_date");
                    txtOrNo.Text = pSet.GetString("or_no").Trim();
                    m_sMode = pSet.GetString("data_mode").Trim();
                }
            }
            pSet.Close();

            cmbPermitNo.SelectedIndex = 0;  // RMC 20120228 modified printing of permit with change of line of business and payment adjustment

            pSet.Query = string.Format("select sum(fees_amtdue) as amount from or_table where or_no = '{0}'", txtOrNo.Text.ToString().Trim());
            if (pSet.Execute())
	        {
                if (pSet.Read())
                    m_dAmountPaid = pSet.GetDouble("amount");
            }
            pSet.Close();

            pSet.Query = string.Format("select bns_desc from bns_table where bns_code = '{0}' and fees_code = 'B' and rev_year = '{1}'", StringUtilities.SetEmptyToSpace(txtBnsCode.Text.ToString().Trim()), ConfigurationAttributes.RevYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtBnsDesc.Text = pSet.GetString("bns_desc").Trim();
                }
            }
            pSet.Close();

            // RMC 20120117 disable generation of permit for PEDDLERS (s)
            /*if (txtBnsDesc.Text.Trim() == "PEDDLERS" || txtBnsDesc.Text.Trim() == "PEDDLER")
            {
               MessageBox.Show("Cannot generate Permit if Business Type is 'PEDDLER'","Business Permit",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                ClearControls();
                return;
            }*/ // RMC 20120124 enable generation of permit for Peddler but no printing
            // RMC 20120117 disable generation of permit for PEDDLERS (e)

            pSet.Query = string.Format("select * from buss_plate where bin = '{0}'", bin1.GetBin());
            if (pSet.Execute())
            {
                if (pSet.Read())
                    txtPlate.Text = pSet.GetString("bns_plate").Trim();
            }
            pSet.Close();

            // RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ (S)
            pSet.Query = "select * from dti_bns_name where bin = '" + bin1.GetBin() + "' order by saved_tm desc";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtDTIBnsName.Text = pSet.GetString("dti_bns_nm");
                    m_sTmpBnsName = txtDTIBnsName.Text;
                }
                else
                    m_sTmpBnsName = "";
            }
            pSet.Close();
            // RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ (E)
	
            txtNationality.Text = "FILIPINO";
            
	        UpdatePermitNoList();

            EnableControls(true);
	
            if(cmbPermitNo.Text.ToString().Trim() == "" || cmbPermitNo.Text.ToString().Trim() == "NONE")
            {
                string strTmp = "12/31/" + ConfigurationAttributes.CurrentYear;
                dtpLicDate.Value = Convert.ToDateTime(strTmp);
                dtpPermitDate.Value = AppSettingsManager.GetCurrentDate();
                dtpPermitDate.Enabled = true;
				m_iMPSwitch = 1;
		
                if(AppSettingsManager.GetConfigValue("25") != "Y")
        		{
                    btnGenerate.Text = "Print";
			    }
		        else
		        {
                    btnGenerate.Text = "Generate";
		        }

                txtAppNo.ReadOnly = false;
                btnGenerate.Enabled = true;
                btnUpdate.Enabled = true;
                btnCancel.Text = "Close";
		
	        }
	        else
	        {
		        OnLoadOldGeneratedMP();
                txtAppNo.ReadOnly = false;
                btnGenerate.Enabled = true;
                btnUpdate.Enabled = true;
                btnGenerate.Text = "Print";
                btnCancel.Text = "Close";
		    }
	    
        }

        private void EnableControls(bool blnEnable)
        {
            dtpAppDate.Enabled = blnEnable;
            dtpLicDate.Enabled = blnEnable;
            txtRemarks.ReadOnly = true;    // RMC 20120124
            txtNationality.ReadOnly = !blnEnable;
            txtLicNo.ReadOnly = !blnEnable;
            txtOwnSSS.ReadOnly = !blnEnable;
            txtAppNo.ReadOnly = !blnEnable;
            txtOwnTin.ReadOnly = !blnEnable;
            //txtPlate.ReadOnly = !blnEnable;
            
        }

        private void ClearControls()
        {
            this.bin1.txtTaxYear.Text = "";
            this.bin1.txtBINSeries.Text = "";
            this.cmbPermitNo.Text = "";
            this.txtBnsName.Text = "";
            this.txtBnsAdd.Text = "";
            this.txtTaxYear.Text = "";
            this.txtTelNo.Text = "";
            this.txtEmployee.Text = "";
            this.txtBnsStat.Text = "";
            this.txtBnsCode.Text = "";
            this.txtBnsDesc.Text = "";
            this.txtOrNo.Text = "";
            this.txtAppNo.Text = "";
            this.txtLicNo.Text = "";
            this.txtPlate.Text = "";
            this.txtOwnName.Text = "";
            this.txtOwnAdd.Text = "";
            this.dtpORDate.Value = AppSettingsManager.GetCurrentDate();
            this.dtpPermitDate.Value = AppSettingsManager.GetCurrentDate();
            this.dtpAppDate.Value = AppSettingsManager.GetCurrentDate();
            this.dtpLicDate.Value = AppSettingsManager.GetCurrentDate();
            this.txtNationality.Text = "";
            this.txtOwnTin.Text = "";
            this.txtOwnSSS.Text = "";
            this.txtRemarks.Text = "";
            this.cmbPermitNo.Items.Clear();
            this.btnGenerate.Enabled = false;
            //btnCancel.Text = "Cancel";
            btnCancel.Text = "Close";   // RMC 20120117 disable generation of permit for PEDDLERS
            this.btnSearch.Text = "Search";
            this.ClearGrid();
            bin1.txtTaxYear.Focus();
            this.ActiveControl = bin1.txtTaxYear;
            btnCheckList.Visible = false;  // RMC 20110905
            txtDTIBnsName.Text = "";    // RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ 
            txtDTIBnsName.ReadOnly = true;  // RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ 
        }

        private void UpdatePermitNoList()
        {
            OracleResultSet pSet = new OracleResultSet();   // RMC 20110908 Corrected error changed pSet to pRec

            string sMpNo = string.Empty;

            pSet.Query = "select distinct permit_no from permit_update_appl";
            pSet.Query+= string.Format(" where bin = '{0}' and tax_year = '{1}' and data_mode = 'PAID'", bin1.GetBin(), ConfigurationAttributes.CurrentYear);
            if(pSet.Execute())
            {
                while (pSet.Read())
                {
                    sMpNo = pSet.GetString("permit_no").Trim();
                    if (sMpNo != "NONE")
                        cmbPermitNo.Items.Add(sMpNo);
                    else
                        cmbPermitNo.Items.Add("Generate ...");

                }
                
            }
            pSet.Close();

            int iCtr = 0;
            pSet.Query = "select count(distinct permit_no) from permit_update_appl";
            pSet.Query += string.Format(" where bin = '{0}' and tax_year = '{1}' and data_mode = 'PAID'", bin1.GetBin(), ConfigurationAttributes.CurrentYear);
            int.TryParse(pSet.ExecuteScalar().ToString(), out iCtr);
            if(iCtr > 0)
                cmbPermitNo.SelectedIndex = 0;
            
        }

        private void OnLoadOldGeneratedMP()
        {
            OracleResultSet pSet = new OracleResultSet();   // RMC 20110908 Corrected error changed pSet to pRec

            pSet.Query = string.Format("select * from rep_mp where bin = '{0}' and tax_year = '{1}' and permit_no = '{2}'", bin1.GetBin(),txtTaxYear.Text.ToString().Trim(), StringUtilities.SetEmptyToSpace(cmbPermitNo.Text.ToString().Trim()));
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtAppNo.Text = pSet.GetString("appl_no").Trim();
                    dtpAppDate.Value = pSet.GetDateTime("appl_dt");
                    txtLicNo.Text = pSet.GetString("ml_no").Trim();
                    dtpLicDate.Value = pSet.GetDateTime("ml_dt");
                    txtNationality.Text = pSet.GetString("nationality").Trim();
                    txtOwnTin.Text = pSet.GetString("tin_no").Trim();
                    txtOwnSSS.Text = pSet.GetString("sss_no").Trim();
                    dtpPermitDate.Value = pSet.GetDateTime("permit_dt");

                    m_iMPSwitch = 1;

                    EnableControls(true);
                    dtpPermitDate.Enabled = false;
                }
                else
                    m_iMPSwitch = 0;
            }
            pSet.Close();
	
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();   // RMC 20110908 Corrected error changed pSet to pRec

            if (btnUpdate.Text == "Update")
            {
                if (!ValidatePlateNo(txtPlate.Text.ToString().Trim(), bin1.GetBin()))
                {
                    string sBin = GetDuplicatePlate(txtPlate.Text.ToString());
                    MessageBox.Show("Business Plate no. already issued to BIN: " + sBin, "Business Permits", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPlate.Text = "";
                    txtPlate.Focus();
                    return;
                }

                pSet.Query = string.Format("select * from buss_plate where bin = '{0}'", bin1.GetBin());
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        pSet.Close();

                        pSet.Query = string.Format("update buss_plate set bns_plate = '{0}' where bin = '{1}'", txtPlate.Text.ToString().Trim(), bin1.GetBin());
                        if (pSet.ExecuteNonQuery() == 0)
                        { }
                    }
                    else
                    {
                        pSet.Close();

                        pSet.Query = "insert into buss_plate values (:1,:2)";
                        pSet.AddParameter(":1", bin1.GetBin());
                        pSet.AddParameter(":2", txtPlate.Text.ToString().Trim());
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                    }
                }

                MessageBox.Show("Business Plate is updated.", "Business Permit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPlate.ReadOnly = true;
                btnUpdate.Text = "Edit";

            }
            else
            {
                txtPlate.ReadOnly = false;
                btnUpdate.Text = "Update";
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();   // RMC 20110908 Corrected error changed pSet to pRec

            string sMPDt = "";
            m_sReportName = "BUSINESS PERMIT";
            string sOrgKind = AppSettingsManager.GetBnsOrgKind(bin1.GetBin());

            //MCR 20150119 (s) remove plate
            //if (sOrgKind != "COOPERATIVE" && sOrgKind != "CORPORATION")
            //    if (btnUpdate.Text == "Update")
            //    {
            //        MessageBox.Show("Update business plate no. first.", "Business Permits", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        return;
            //    }
            //MCR 20150119 (e) remove plate

            //AFM 20200106 DISALLOW GENERATION IF BIN IS UNPAID
            if (m_sTblSource == "QUE")
            {
                MessageBox.Show("Business not yet paid!", "Business Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ (s)
            if (btnEditBnsName.Text == "Update Business Name")
            {
                MessageBox.Show("Update business name first", "Business Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ (e)

            // RMC 20120124 added printing of remarks in Permit (user-request) (s)
            if (btnEditRemarks.Text == "Update")
            {
                MessageBox.Show("Update remarks first.", "Business Permits", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // RMC 20120124 added printing of remarks in Permit (user-request) (e)

            //MCR 20150119 (s) remove plate
            //if (sOrgKind != "COOPERATIVE" && sOrgKind != "CORPORATION")
            //    if (!ValidateIfWithPlateNo())
            //        return;
            //MCR 20150119 (e) remove plate

            // RMC 20120130 disable generation/printing of permit for hold records (s)
            if (!ValidateHoldRecord())
                return;
            // RMC 20120130 disable generation/printing of permit for hold records (e)

            // RMC 20170126 disable printing of permit if business already retired (s)
            if (txtBnsStat.Text == "RET")
            {
                MessageBox.Show("Business already retired","Business Permit",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            // RMC 20170126 disable printing of permit if business already retired (e)

            bool bPrintTempPermit = false;  // RMC 20171110 added configurable option to print temporary permit - requested by Malolos

            //MCR 20111121 (s)
            string sValue = AppSettingsManager.GetNigViolist(bin1.GetBin());
            if (sValue != "")
            {
                MessageBox.Show("Record was tagged in negative list\n\n" + sValue, "Business Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //MCR 20111121 (e)

            // RMC 20110905 added validation of requirements in generating permit (s)
            if (!AppRequirement.Checklist(txtTaxYear.Text.Trim(), bin1.GetBin(), txtBnsStat.Text.Trim(), txtBnsCode.Text.Trim(), m_sOrgKind))
            {
                //MessageBox.Show("Requirements not yet complete.","Businees Permit",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //btnCheckList.Visible = true;   // RMC 20110905    // RMC 20111229 transferred enabling of checklist button

                // RMC 20171110 added configurable option to print temporary permit - requested by Malolos (s)
                if (AppSettingsManager.GetConfigValueByDescription("ENABLE PRINTING OF TEMPORARY PERMIT") == "Y")
                {
                    if (MessageBox.Show("Requirements not yet complete.\n Print Temporary Permit?", "Temporary Permit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)   
                        return;
                    else
                        bPrintTempPermit = true;
                }// RMC 20171110 added configurable option to print temporary permit - requested by Malolos (e)
                else
                {
                    if (MessageBox.Show("Requirements not yet complete. Continue?", "Business Permit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)   // RMC 20120102 by-pass reqmts checklist in printing permit - ms Berna
                        return;
                }
            }
            // RMC 20110905 added validation of requirements in generating permit (e)

            // RMC 20171110 added configurable option to print temporary permit - requested by Malolos (s)
            if (bPrintTempPermit)
            {
                using (frmAppCheckList frmAppCheckList = new frmAppCheckList())
                {
                    frmAppCheckList.BIN = bin1.GetBin();
                    frmAppCheckList.TaxYear = txtTaxYear.Text;
                    frmAppCheckList.Source = "TEMP PERMIT";
                    frmAppCheckList.TempPermit = true;
                    frmAppCheckList.ShowDialog();
                    m_sTempList = frmAppCheckList.TempList;
                }

                PrintTempPermit();
            }// RMC 20171110 added configurable option to print temporary permit - requested by Malolos (e)
            else
            {
                
                if (btnGenerate.Text == "Generate")
                {
                    if (m_bIsPUT && !m_bIsRenPUT)
                        GeneratePUT();
                    else
                        GenerateMP();
                }

                if (btnGenerate.Text == "Print")
                {
                    string sPermitNo = string.Empty;
                    string sTaxYear = string.Empty;

                    sPermitNo = cmbPermitNo.Text.ToString().Trim();
                    sTaxYear = txtTaxYear.Text.ToString().Trim();

                    // RMC 20120124 enable generation of permit for Peddler but no printing (s)
                    if (txtBnsDesc.Text.Trim() == "PEDDLERS" || txtBnsDesc.Text.Trim() == "PEDDLER")
                    {
                        MessageBox.Show("Cannot print Permit if Business Type is 'PEDDLER'", "Business Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        ClearControls();
                        return;
                    }
                    // RMC 20120124 enable generation of permit for Peddler but no printing (e)

                    if (sPermitNo == "")
                    {
                        MessageBox.Show("Select Permit Number", "Business Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        if (StringUtilities.Left(sPermitNo, 5) != sTaxYear + "-")
                            cmbPermitNo.Text = sTaxYear + "-" + sPermitNo;

                        sPermitNo = cmbPermitNo.Text.ToString().Trim();

                        /*pSet.Query = string.Format("select * from businesses where bin = '{0}' and permit_no = '{1}'", bin1.GetBin(), sPermitNo);
                        if(pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                sMPDt = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

                                pSet.Query = string.Format("update businesses set permit_no = '{0}',permit_dt = to_date('{1}','MM/dd/yyyy') where bin = '{2}'", sPermitNo, sMPDt, bin1.GetBin());
                                if (pSet.ExecuteNonQuery() == 0)
                                { }
                            }
                        }
                        pSet.Close();*/
                        // do not update permit date    // RMC 20110803

                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                            PrintMP();
                    }
                }
            }
        }

        private void PrintMP()
        {
            OracleResultSet pSet = new OracleResultSet();   // RMC 20110908 Corrected error changed pSet to pRec

            string sPermitDt, sApplDt, sMtDt, sMayorName, sOrDt;

            sPermitDt = string.Format("{0:MM/dd/yyyy}", dtpPermitDate.Value);
            sApplDt = string.Format("{0:MM/dd/yyyy}", dtpAppDate.Value);
            sMtDt = string.Format("{0:MM/dd/yyyy}", dtpLicDate.Value);
            sOrDt = string.Format("{0:MM/dd/yyyy}", dtpORDate.Value);
            sMayorName = ""; //for future daw

            m_iMPSwitch = 1;

            if (m_iMPSwitch == 1)
            {
                pSet.Query = string.Format("delete from rep_mp where bin = '{0}' and tax_year = '{1}' and permit_no = '{2}'", bin1.GetBin(), txtTaxYear.Text.ToString(), cmbPermitNo.Text.ToString());
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from gen_info where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = "insert into gen_info values (:1, :2, :3, :4, :5)";
                pSet.AddParameter(":1", m_sReportName);
                pSet.AddParameter(":2", AppSettingsManager.GetCurrentDate());
                pSet.AddParameter(":3", AppSettingsManager.SystemUser.UserCode);
                pSet.AddParameter(":4", "1");
                pSet.AddParameter(":5", "ASS");
                if (pSet.ExecuteNonQuery() == 0)
                { }

                if (txtEmployee.Text.Trim() == string.Empty)
                    txtEmployee.Text = "0";

                pSet.Query = "insert into rep_mp values(";
                pSet.Query += "'" + AppSettingsManager.GetConfigValue("09") + "', ";
                pSet.Query += "'" + bin1.GetBin() + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtBnsName.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtBnsAdd.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtTelNo.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtBnsStat.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtTaxYear.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtBnsCode.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtBnsDesc.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(cmbPermitNo.Text.ToString().Trim())) + "',";
                pSet.Query += " to_date('" + sPermitDt + "','MM/dd/yyyy'), ";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtAppNo.Text.ToString().Trim())) + "',";
                pSet.Query += " to_date('" + sApplDt + "','MM/dd/yyyy'),";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtLicNo.Text.ToString().Trim())) + "',";
                pSet.Query += " to_date('" + sMtDt + "','MM/dd/yyyy'),";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtOrNo.Text.ToString().Trim())) + "',";
                pSet.Query += " to_date('" + sOrDt + "','MM/dd/yyyy'),";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtOwnName.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtOwnAdd.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtNationality.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtOwnTin.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtOwnSSS.Text.ToString().Trim())) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtEmployee.Text.ToString().Trim())) + "', ";

                string sRowValue = string.Empty;
                for (int iRow = 0; iRow < 10; iRow++)
                {
                    try
                    {
                        sRowValue = dgvList[0, iRow].Value.ToString().Trim();
                    }
                    catch
                    {
                        sRowValue = "";
                    }

                    pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(sRowValue)) + "',";
                }

                for (int iRow = 0; iRow < 10; iRow++)
                {
                    try
                    {
                        sRowValue = string.Format("{0:###.00}", Convert.ToDouble(dgvList[1, iRow].Value.ToString().Trim()));
                    }
                    catch
                    {
                        sRowValue = "0.00";
                    }

                    pSet.Query += "'" + sRowValue + "',";
                }

                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtRemarks.Text.ToString().Trim())) + "', ";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(sMayorName)) + "',";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(AppSettingsManager.SystemUser.UserName)) + "', ";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(AppSettingsManager.SystemUser.UserCode)) + "', ";
                pSet.Query += "'" + StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(m_sReportName)) + "') ";
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("update gen_info set switch = 0 where report_name = '{0}' and system = 'ASS' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }

            DateTime dtpTagDate;
            string sYear = string.Empty;

	    	pSet.Query = string.Format("select * from billing_tagging where bin = '{0}' and report = 'PERMIT' and tax_year = '{1}'",bin1.GetBin(),txtTaxYear.Text.ToString().Trim());
	        if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    dtpTagDate = pSet.GetDateTime("tdatetime");
                    sYear = string.Format("{0:yyyy}", dtpTagDate);

                    if (sYear == ConfigurationAttributes.CurrentYear)
                        PreviewMP();
                    else
                    {
                        PreviewMP();

                        pSet.Close();

                        pSet.Query = string.Format("delete from billing_tagging where bin = '{0}' and tax_year = '{1}' and report = 'PERMIT'", bin1.GetBin(), txtTaxYear.Text.ToString().Trim());
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        pSet.Query = "insert into billing_tagging values (:1,:2,:3,:4,:5)";
                        pSet.AddParameter(":1", bin1.GetBin());
                        pSet.AddParameter(":2", AppSettingsManager.GetCurrentDate());
                        pSet.AddParameter(":3", AppSettingsManager.SystemUser.UserCode);
                        pSet.AddParameter(":4", "PERMIT");
                        pSet.AddParameter(":5", txtTaxYear.Text.ToString().Trim());
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        

                    }
                }
                else
                {
                    PreviewMP();

                    pSet.Query = "insert into billing_tagging values (:1,:2,:3,:4,:5)";
                    pSet.AddParameter(":1", bin1.GetBin());
                    pSet.AddParameter(":2", AppSettingsManager.GetCurrentDate());
                    pSet.AddParameter(":3", AppSettingsManager.SystemUser.UserCode);
                    pSet.AddParameter(":4", "PERMIT");
                    pSet.AddParameter(":5", txtTaxYear.Text.ToString().Trim());
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    

                }
            }
            pSet.Close();
	
        }

        private void PreviewMP()
        {
            /*PrintClass = new PrintPermit();
            PrintClass.BIN = bin1.GetBin();
            PrintClass.ReportName = "Mayor's Permit";
            PrintClass.TaxYear = txtTaxYear.Text.ToString().Trim();
            PrintClass.PermitNumber = cmbPermitNo.Text.ToString().Trim();
            PrintClass.FormLoad();*/

            // RMC 20140129 Added module to input permit date issuance before printing of Permit (s)
            frmPermitDtIssued form = new frmPermitDtIssued();
            form.BIN = bin1.GetBin();
            form.TaxYear = txtTaxYear.Text.ToString().Trim();
            form.ShowDialog();
            // RMC 20140129 Added module to input permit date issuance before printing of Permit (e)

            frmPermitNew PrintForm = new frmPermitNew();
            PrintForm.ReportSwitch = "Permit";
            PrintForm.BIN = bin1.GetBin();
            PrintForm.PermitNo = cmbPermitNo.Text.ToString().Trim();
            PrintForm.TaxYear = txtTaxYear.Text.ToString().Trim();
            
            PrintForm.NoticeDate = form.PermitDate; // RMC 20140129 Added module to input permit date issuance before printing of Permit
            PrintForm.OwnCode = m_sFirstOwnCode;
            PrintForm.ShowDialog();
        }

        private void GeneratePUT()
        {
            OracleResultSet pSet = new OracleResultSet();   // RMC 20110908 Corrected error changed pSet to pRec

            string sMPSerial = "";
            string sMPNo = "";
            string sDupBIN = "";
            string sMPDt = "";

            pSet.Query = string.Format("select * from put_series");
	        if(pSet.Execute())
            {
                if(pSet.Read())
		            sMPSerial = pSet.GetString("mp_no");
		        else
			        sMPSerial = "0";
            }
	        pSet.Close();

	        bool bAns = true;
            while (bAns)
            {
                sMPSerial = string.Format("{0:##}", Convert.ToInt32(sMPSerial) + 1);

                pSet.Query = string.Format("select * from put_series where mp_no >= {0}", Convert.ToInt32(sMPSerial));
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sMPSerial = pSet.GetString("mp_no");
                    }
                    else
                    {
                        pSet.Close();

                        pSet.Query = string.Format("update put_series set mp_no = '{0}'", sMPSerial);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        sMPDt = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                        sMPNo = m_sMainPermitNo + "-" + sMPSerial;

                        pSet.Query = string.Format("select bin from permit_update_appl where permit_no = '{0}'", sMPNo);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                sDupBIN = pSet.GetString("bin");
                                //MessageBox.Show("Duplicate Permit Number with BIN: " + sDupBIN, "Business Permits", MessageBoxButtons.OK, MessageBoxIcon.Information);    ?????
                            }
                            else
                            {
                                pSet.Close();

                                pSet.Query = string.Format("update permit_update_appl set permit_no = '{0}',permit_dt = to_date('{1}','MM/DD/YYYY') where bin = '{2}' and permit_no = 'NONE'", sMPNo, sMPDt, bin1.GetBin());
                                if (pSet.ExecuteNonQuery() == 0)
                                { }

                                bAns = false;
                            }
                        }
                        pSet.Close();
                    }
                }
            }

            int iCol = 0;

            iCol = cmbPermitNo.Items.Count;
            cmbPermitNo.Items.Add(sMPNo);
            cmbPermitNo.SelectedIndex = iCol;
            btnGenerate.Enabled = false;

            // RMC 20110801 (S)
            //if (AuditTrail.InsertTrail("ABBP", "put_series", "Generate Permit for " + bin1.GetBin()) == 0)
            if (AuditTrail.InsertTrail("ABBP", "put_series", "Generate Permit of BIN:  " + bin1.GetBin() + " for tax year " + txtTaxYear.Text.Trim()) == 0) // RMC 20110906
            {
                pSet.Rollback();
                pSet.Close();
                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // RMC 20110801 (E)
            PrintMP();
        }

        private void GenerateMP()
        {
            OracleResultSet pSet = new OracleResultSet();   // RMC 20110908 Corrected error changed pSet to pRec

            string sMPSerial = "";
            string sMPNo = "";
            string sDupBIN = "";
            string sMPDt = "";

            pSet.Query = string.Format("select * from mp_series where tax_year = '{0}'", AppSettingsManager.GetConfigValue("12"));  // RMC 20111129 modified initialization of serials
            if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    sMPSerial = pSet.GetString("mp_no");
                    pSet.Close();
                }
                else
                {
                    sMPSerial = "0";
                    pSet.Close();

                    // RMC 20111219 added mods in generation of MP series (s)
                    pSet.Query = "insert into mp_series values (";
                    pSet.Query += "'"+sMPSerial+"','" + AppSettingsManager.GetConfigValue("12") + "')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }
                    // RMC 20111219 added mods in generation of MP series (e)
                }

            }
	        //pSet.Close();

	    	bool bAns = true;
	        while (bAns)
	        {
		        sMPSerial = string.Format("{0:##}", Convert.ToInt32(sMPSerial) + 1);

                pSet.Query = string.Format("select mp_no from mp_series where mp_no >= {0} and tax_year = '{1}'", Convert.ToInt32(sMPSerial), AppSettingsManager.GetConfigValue("12")); // RMC 20111129 modified initialization of serials
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sMPSerial = pSet.GetString("mp_no");
                    }
                    else
                    {
                        pSet.Close();

                        pSet.Query = string.Format("update mp_series set mp_no = '{0}' where tax_year = '{1}'", sMPSerial, AppSettingsManager.GetConfigValue("12"));    // RMC 20111129 modified initialization of serials
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        sMPSerial = string.Format("{0:0000#}", Convert.ToInt32(sMPSerial));

                        sMPDt = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                        sMPNo = string.Format("{0:####}-{1}", AppSettingsManager.GetCurrentDate().Year, sMPSerial);

                        pSet.Close();

                        pSet.Query = string.Format("select bin from businesses where permit_no = '{0}'", sMPNo);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                sDupBIN = pSet.GetString("bin");
                                //MessageBox.Show("Duplicate Permit Number with BIN: " + sDupBIN);

                            }
                            else
                            {
                                pSet.Close();
                                pSet.Query = string.Format("update businesses set permit_no = '{0}',permit_dt = to_date('{1}','MM/DD/YYYY') where bin = '{2}'", sMPNo, sMPDt, bin1.GetBin());
                                if (pSet.ExecuteNonQuery() == 0)
                                { }

                                if (m_bIsRenPUT)
                                {
                                    pSet.Query = string.Format("update permit_update_appl set permit_no = '{0}',permit_dt = to_date('{1}','MM/DD/YYYY') where bin = '{2}' and permit_no = 'NONE'", sMPNo, sMPDt, bin1.GetBin());
                                    if (pSet.ExecuteNonQuery() == 0)
                                    { }
                                }

                                bAns = false;
                            }
                        }

                    }
                }
            }

            int iCol = 0;

            iCol = cmbPermitNo.Items.Count;
            cmbPermitNo.Items.Add(sMPNo);
            cmbPermitNo.SelectedIndex = iCol;
            btnGenerate.Enabled = false;

            // RMC 20110801 (S)
            //if (AuditTrail.InsertTrail("ABBP", "mp_series", "Generate Permit for " + bin1.GetBin()) == 0)

            TransLog.UpdateLog(bin1.GetBin(), txtBnsStat.Text, txtTaxYear.Text.Trim(), "ABBP", m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

            if (AuditTrail.InsertTrail("ABBP", "mp_series", "Generate Permit of BIN: " + bin1.GetBin() + "for tax year " + txtTaxYear.Text.Trim()) == 0)    // RMC 20110906
            {
                pSet.Rollback();
                pSet.Close();
                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // RMC 20110801 (E)

            PrintMP();
        }

        private void cmbPermitNo_SelectedValueChanged(object sender, EventArgs e)
        {
            PopulatePermitFees();
        }

        private void PopulatePermitFees()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();   // RMC 20110908 Corrected error changed pSet to pRec

            int intRow = 0;
            string sPopFeesCode = string.Empty;
            string sPopAmount = string.Empty;
            string sPopBnsCodeMain = string.Empty;
            string sPopBnsDesc = string.Empty;

            this.ClearGrid();

            pSet.Query = string.Format("select fees_code from tax_and_fees_table where rtrim(fees_code) = '01' and rev_year = '{0}'",ConfigurationAttributes.RevYear);
			if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    sPopFeesCode = pSet.GetString("fees_code").Trim();
                }
            }
            pSet.Close();

            pSet.Query = string.Format("select * from tax_and_fees where fees_code = '{0}' and bin = '{1}' and tax_year = '{2}'",sPopFeesCode,bin1.GetBin(), txtTaxYear.Text.ToString().Trim());
	        if(pSet.Execute())
	        {
                if(pSet.Read())
	            {
		            int iDescCtr = 0; 
		    
		            string sTmp = "";
		            
                    if (cmbPermitNo.Items.Count > 0)
		            {
			            if (m_bNewAddl || m_bChClass)
			            {
				            sTmp = " and bns_code_main in (select new_bns_code from permit_update_appl";
				            sTmp+= string.Format(" where bin = '{0}' and permit_no = '{1}'", bin1.GetBin(), cmbPermitNo.Text.ToString().Trim());
			            }
			            if (m_bTransOwn || m_bTransLoc)
			            {
				            sTmp = "";

			                if (m_bChClass)
				            {
					            sTmp = " and bns_code_main not in (select old_bns_code from permit_update_appl";
					            sTmp+= string.Format(" where bin = '{0}' and appl_type = 'CTYP' and permit_no = '{1}')", bin1.GetBin(), cmbPermitNo.Text.ToString().Trim());
				            }
			            }
			        }
		            else
		            {
				        sTmp = " and bns_code_main not in (select new_bns_code from permit_update_appl";
				        sTmp+= string.Format(" where bin = '{0}' and permit_no <> '{1}'", bin1.GetBin(), cmbPermitNo.Text.ToString().Trim());
                        sTmp+= " and (appl_type = 'ADDL' or appl_type = 'CTYP'))"; 
                    }
			
            		if (m_bIsRenPUT)
			            sTmp = "";
		
                    intRow = dgvList.Rows.Count;

		            pRec.Query = "select bns_code_main, sum(amount) as amount from tax_and_fees a, bns_table b";
		            pRec.Query+= string.Format(" where a.fees_code = '{0}'", sPopFeesCode);
		            pRec.Query+= " and a.fees_code = b.fees_code";
		            pRec.Query+= " and bns_code_sub = bns_code";
		            pRec.Query+= string.Format(" and bin = '{0}' and tax_year = '{1}'", bin1.GetBin(), txtTaxYear.Text.ToString().Trim());
        	        pRec.Query+= " and substr(rtrim(bns_desc),1,9) <> 'SIGNBOARD'";
			        pRec.Query+= " and bns_desc <> 'APPLICATION FEE'";
		            pRec.Query+= " and bns_desc <> 'COLD STORAGE'"; 
		            pRec.Query+= " and bns_desc <> 'REFRIGERATING CASES'"; 
		            pRec.Query+= sTmp; 
		            pRec.Query+= " group by bns_code_main";
		            if(pRec.Execute())
                    {
                        while(pRec.Read())
		                {
			                sPopAmount = string.Format("{0:#,###.00}", pRec.GetDouble("amount"));
			                sPopBnsCodeMain =  pRec.GetString("bns_code_main");
                			sPopBnsDesc = " " + sPopBnsCodeMain + " PERMIT FEE";
		
                            dgvList.Rows.Add("");
			                dgvList[0,intRow].Value = sPopBnsDesc;
			                dgvList[1,intRow].Value = sPopAmount;
                            intRow++;
                        }
                    }
                    pRec.Close();
			
		            pRec.Query = "select bns_desc, sum(amount) as amount from tax_and_fees a, bns_table b";
		            pRec.Query+= string.Format(" where a.fees_code = '{0}'", sPopFeesCode);
		            pRec.Query+= " and a.fees_code = b.fees_code";
		            pRec.Query+= " and bns_code_sub = bns_code";
		            pRec.Query+= string.Format(" and bin = '{0}' and tax_year = '{1}'", bin1.GetBin(), txtTaxYear.Text.ToString().Trim());
        			pRec.Query+= " and (substr(rtrim(bns_desc),1,9) = 'SIGNBOARD'";
					pRec.Query+= " or bns_desc = 'APPLICATION FEE'"; 
		            pRec.Query+= " or bns_desc = 'COLD STORAGE'"; 
		            pRec.Query+= " or bns_desc = 'REFRIGERATING CASES')"; 
		            pRec.Query+= sTmp; 
		            pRec.Query+= " group by bns_desc";
		            if(pRec.Execute())
                    {
                        while(pRec.Read())
		                {
                            sPopAmount = string.Format("{0:#,###.00}", pRec.GetDouble("amount"));
			                sPopBnsDesc = pRec.GetString("bns_desc");
                			
                            dgvList.Rows.Add("");
			                dgvList[0,intRow].Value = sPopBnsDesc;
			                dgvList[1,intRow].Value = sPopAmount;
                            intRow++;

                        }
                    }
                    pRec.Close();
		    	}
	            else 
	            {
		            pRec.Query = "select bns_desc,fees_amtdue from bns_table,or_table";
                    pRec.Query+= string.Format(" where or_no = '{0}' and or_table.fees_code = '{1}'", txtOrNo.Text.ToString().Trim(), sPopFeesCode);
				    pRec.Query+= string.Format(" and bns_table.fees_code = or_table.fees_code and bns_code_main = '{0}'", StringUtilities.SetEmptyToSpace(txtBnsCode.Text.ToString().Trim()));
					pRec.Query+= " and bns_code_main = bns_code";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            if (m_sMode == "POS")
                                sPopBnsDesc = "BUSINESS PERMIT";
                            else
                                sPopBnsDesc = pRec.GetString("bns_desc");

                            sPopAmount = string.Format("{0:#,###.00}", pRec.GetDouble("fees_amtdue"));

                            dgvList.Rows.Add("");
                            dgvList[0, intRow].Value = sPopBnsDesc;
                            dgvList[1, intRow].Value = sPopAmount;
                            intRow++;
                        }
                    }
                    pRec.Close();
                }
            }
            pSet.Close();
		
        }

        private void ClearGrid()
        {
            dgvList.Columns.Clear();
            dgvList.Columns.Add("FEE", "PERMIT FEE(S)");
            dgvList.Columns.Add("AMT", "AMOUNT");
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 300;
            dgvList.Columns[1].Width = 100;
            dgvList.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintClass = new PrintPermit();
            PrintClass.BIN = bin1.GetBin();
            PrintClass.ReportName = "Certificate of Registration";
            PrintClass.PermitNumber = cmbPermitNo.Text.ToString().Trim();
            PrintClass.FormLoad();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Cancel")
            {
                ClearControls();
            }
            else
                this.Close();
        }

        private bool ValidatePlateNo(string sPlateNo, string sBin)
        {
            OracleResultSet result = new OracleResultSet();

            //result.Query = string.Format("select * from buss_plate where bns_plate = '{0}' and bin <> '{1}'", sPlateNo, sBin);
            result.Query = string.Format("select * from buss_plate where bin in (select bin from businesses where bns_stat <> 'RET') and bns_plate = '{0}' and bin <> '{1}'", sPlateNo, sBin);
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();
                    return false;
                }
                else
                {
                    result.Close();
                    return true;
                }
            }
            result.Close();

            return true;

        }

        private string GetDuplicatePlate(string sPlateNo)
        {
            OracleResultSet result = new OracleResultSet();
            string sBin = "";

            result.Query = string.Format("select * from buss_plate where bns_plate = '{0}' and bin <> '{1}'", sPlateNo, bin1.GetBin());
            if(result.Execute())
            {
                if(result.Read())
                {
                    sBin = result.GetString("bin");
                }
            }
            result.Close();

            return sBin;
        }

        private bool ValidateIfWithPlateNo()
        {
            OracleResultSet result = new OracleResultSet();

            if (AppSettingsManager.GetConfigValue("10") == "232")   // RMC 20140102 BUSINESS PLATE NOT REQUIRED IN MATI
            { }
            else
            {
                if (txtPlate.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Business plate no. required.", "Business Permits", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPlate.Focus();
                    return false;
                }
            }

            return true;
            
        }

        private void btnCheckList_Click(object sender, EventArgs e)
        {
            if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
            {
                using (frmAppCheckList frmAppCheckList = new frmAppCheckList())
                {
                    frmAppCheckList.BIN = bin1.GetBin();
                    frmAppCheckList.TaxYear = txtTaxYear.Text;
                    frmAppCheckList.TempPermit = false;
                    frmAppCheckList.Source = "PERMIT";
                    frmAppCheckList.WatTable = m_sTblSource; //AFM 20200106 requested by malolos. allow viewing of unpaid bin
                    frmAppCheckList.ShowDialog();
                }
            }
        }

        private void btnEditRemarks_Click(object sender, EventArgs e)
        {
            // RMC 20120124 added printing of remarks in Permit (user-request) 

            if (btnEditRemarks.Text == "Edit")
            {
                btnEditRemarks.Text = "Update";
                txtRemarks.ReadOnly = false;
            }
            else
            {
                if (MessageBox.Show("Update remarks?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet pCmd = new OracleResultSet();

                    pCmd.Query = "update businesses set memoranda = '" + StringUtilities.HandleApostrophe(txtRemarks.Text.Trim()) + "'";
                    pCmd.Query += " where bin = '" + bin1.GetBin() + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                }
                
                btnEditRemarks.Text = "Edit";
                txtRemarks.ReadOnly = true;
            }
        }

        private bool ValidateHoldRecord()
        {
            // RMC 20120130 disable generation/printing of permit for hold records 
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
                    strMess = "Cannot Generate/Print Permit! This record is currently on hold.\nUser Code: " + strHRUser + "  Date: " + strHRDate + "\nRemarks: " + strHRRemarks;
                    MessageBox.Show(strMess, "Business Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            pSet.Close();

            return true;
        }

        private void btnEditBnsName_Click(object sender, EventArgs e)
        {
            // RMC 20120328 Added capturing of dti business name in permit printing - user-request thru ALJ 
            string sQuery = "";
            OracleResultSet pCmd = new OracleResultSet();

            if (btnEditBnsName.Text == "Edit Business Name")
            {
                txtDTIBnsName.ReadOnly = false;
                btnEditBnsName.Text = "Update Business Name";
            }
            else
            {
                string sObj = "";

                if (txtDTIBnsName.Text.Trim() != "")
                {
                    if (MessageBox.Show("Save business name?\nPlease note: this edited business name is for printing of Permit only.", "Business Permit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sDate = "";

                        pCmd.Query = "delete from dti_bns_name where bin = '" + bin1.GetBin() + "'";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        pCmd.Query = "insert into dti_bns_name values (";
                        pCmd.Query += "'" + bin1.GetBin() + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtDTIBnsName.Text) + "', ";
                        pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                        sDate = Convert.ToString(AppSettingsManager.GetCurrentDate());
                        pCmd.Query += " to_date('" + sDate + "', 'MM/dd/yyyy hh:mi:ss am')) ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        sObj = bin1.GetBin() + ": Edited Permit Business Name to " + txtDTIBnsName.Text;

                        if (AuditTrail.InsertTrail("ABBP-EBN", "dti_bns_name", sObj) == 0)
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        txtDTIBnsName.Text = m_sTmpBnsName;
                    }
                }
                else
                {
                    pCmd.Query = "delete from dti_bns_name where bin = '" + bin1.GetBin() + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    if (m_sTmpBnsName != "")
                    {
                        sObj = bin1.GetBin() + ": Deleted edited Permit Business Name - " + m_sTmpBnsName;

                        if (AuditTrail.InsertTrail("ABBP-EBN", "dti_bns_name", sObj) == 0)
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                txtDTIBnsName.ReadOnly = true;
                btnEditBnsName.Text = "Edit Business Name";
            }
        }

        private void containerWithShadow1_Load(object sender, EventArgs e)
        {

        }

        private void PrintTempPermit()
        {
            // RMC 20171110 added configurable option to print temporary permit - requested by Malolos

            OracleResultSet pSet = new OracleResultSet();
            string sTmpNo = string.Empty;
            string sValidity = string.Empty;
            DateTime dtValidity = AppSettingsManager.GetCurrentDate();

            //validation
            pSet.Query = "select * from permit_temp where bin = '" + bin1.GetBin() + "' and tax_year = '" + txtTaxYear.Text + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sTmpNo =  pSet.GetString("temp_permit_no");
                    dtValidity = pSet.GetDateTime("validity_date");

                    if (MessageBox.Show("Temporary Permit No. '" + sTmpNo + "' has been issued to this record.\n Would you like to re-print Temporary Permit?", "Temporary Permit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }
            }
            pSet.Close();

            //generate temp permit
            if (sTmpNo == "")
            {
                int iTmpNo = 0;
                pSet.Query = "select * from permit_temp where tax_year = '" + txtTaxYear.Text + "' order by temp_permit_no desc";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        int.TryParse(pSet.GetString("temp_permit_no"), out iTmpNo);
                        iTmpNo++;
                    }
                    else
                        iTmpNo = 1;
                }
                pSet.Close();

                sTmpNo = string.Format("{0:0####}", iTmpNo);

                //save to update first to avoid duplication of permit_no
                pSet.Query = "insert into permit_temp (bin, tax_year, temp_permit_no, printed_dt, printed_by) values (:1,:2,:3,:4,:5) ";
                pSet.AddParameter(":1", bin1.GetBin());
                pSet.AddParameter(":2", txtTaxYear.Text);
                pSet.AddParameter(":3", sTmpNo);
                pSet.AddParameter(":4", AppSettingsManager.GetCurrentDate());
                pSet.AddParameter(":5", AppSettingsManager.SystemUser.UserCode);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                frmPermitDtIssued form = new frmPermitDtIssued();
                form.BIN = bin1.GetBin();
                form.TaxYear = txtTaxYear.Text.ToString().Trim();
                form.label2.Text = "Validity Date:";
                form.ShowDialog();

                sValidity = string.Format("{0:MM/dd/yyyy}", form.PermitDate);

                pSet.Query = "update permit_temp set validity_date = to_date('" + sValidity + "','MM/dd/yyyy')";
                pSet.Query += " where bin = '" + bin1.GetBin() + "' and tax_year = '" + txtTaxYear.Text + "'";
                if (pSet.ExecuteNonQuery() == 0)
                { }

                dtValidity = form.PermitDate;
            }
            else
            {
                frmPermitDtIssued form = new frmPermitDtIssued();
                form.BIN = bin1.GetBin();
                form.TaxYear = txtTaxYear.Text.ToString().Trim();
                form.label2.Text = "Validity Date:";
                form.ShowDialog();

                sValidity = string.Format("{0:MM/dd/yyyy}", form.PermitDate);

                pSet.Query = "update permit_temp set validity_date = to_date('" + sValidity + "','MM/dd/yyyy')";
                pSet.Query += " where bin = '" + bin1.GetBin() + "' and tax_year = '" + txtTaxYear.Text + "'";
                if (pSet.ExecuteNonQuery() == 0)
                { }

                dtValidity = form.PermitDate;
            }

            frmPermitNew PrintForm = new frmPermitNew();
            PrintForm.ReportSwitch = "Temporary Permit";
            PrintForm.BIN = bin1.GetBin();
            PrintForm.PermitNo = sTmpNo;
            PrintForm.TaxYear = txtTaxYear.Text.ToString().Trim();
            PrintForm.NoticeDate = dtValidity; 
            PrintForm.OwnCode = m_sFirstOwnCode;
            PrintForm.TempList = m_sTempList;
            PrintForm.ShowDialog();


        }

    }
}