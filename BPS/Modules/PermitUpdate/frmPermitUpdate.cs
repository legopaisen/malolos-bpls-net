using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.SearchBusiness;
using Amellar.Common.TaskManager;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Modules.AdditionalBusiness;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.PermitUpdate
{
    public partial class frmPermitUpdate : Form
    {
        OracleResultSet pSet = new OracleResultSet();
        frmSearchBusiness frmSearchBns = new frmSearchBusiness();
        
        private string m_strModule = string.Empty;
        private string m_sBnsCode = string.Empty;
        private string m_sNewBnsCode = string.Empty;
        private string m_sOwnCode = string.Empty;
        private string m_sNewOwnCode = string.Empty;
        private string m_sDateOperated = string.Empty;
        private bool m_bIsRenPUT = false;
        private bool m_bTransOwn = false;
        private bool m_bTransLoc = false;
        private bool m_bChClass = false;
        private bool m_boChBnsName = false;
        private bool m_bNewAddl = false;
        private bool m_bIsApplSave = false;
        private bool m_bWBilling = false;
        private bool m_bChOrg = false;  // RMC 20111004 added permit-update change of orgn kind
        private string sTrBIN = string.Empty;
        private string sTrAppCode = string.Empty;
        private string sTrBnsCode = string.Empty;
        private string sTrPrevOwnCode = string.Empty;
        private string sTrNewOwnCode = string.Empty;
        private string sTrOwnLn = string.Empty;
        private string sTrOwnFn = string.Empty;
        private string sTrOwnMi = string.Empty;
        private string sTrAddrNo = string.Empty;
        private string sTrAddrStreet = string.Empty;
        private string sTrAddrBrgy = string.Empty;
        private string sTrAddrZone = string.Empty;
        private string sTrAddrDist = string.Empty;
        private string sTrAddrMun = string.Empty;
        private string sTrAddrProv = string.Empty;
        private string sTrAppDate = string.Empty;
        private string sTaxYear = string.Empty;
        private string sQtrPaid = string.Empty;
        private string m_sPlaceOccupancy = string.Empty;
        public DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
        private string sTrDTINo = string.Empty; // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership
        private DateTime dTrDTIDt = AppSettingsManager.GetCurrentDate();    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership
        private string sTrMemo = string.Empty;  // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership

        public frmPermitUpdate()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
            m_strModule = "Permit Update Transaction";
            bin1.txtTaxYear.Focus();
            this.ActiveControl = bin1.txtTaxYear;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
                {
                    if (!TaskMan.IsObjectLock(bin1.GetBin(), "PERMIT_UPDATE", "ADD", "ASS"))
                    {
                        LoadValues();
                        m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    }
                    else
                    {
                        bin1.txtTaxYear.Text = "";
                        bin1.txtBINSeries.Text = "";

                    }
                }
                else
                {
                    frmSearchBns = new frmSearchBusiness();
                    //frmSearchBns.ModuleCode = m_sFormStatus;

                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                        if (!TaskMan.IsObjectLock(bin1.GetBin(), "PERMIT_UPDATE", "ADD", "ASS"))
                        {
                            LoadValues();
                            m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                        }
                        else
                        {
                            bin1.txtTaxYear.Text = "";
                            bin1.txtBINSeries.Text = "";

                        }
                    }
                }
            }
            else
            {
                ClearControls();
                btnSearch.Text = "Search";
                btnCancelApp.Enabled = false;
            }
        }

        private void LoadValues()
        {
            string sApplType = string.Empty;
            OracleResultSet result = new OracleResultSet();
            pSet.Query = string.Format("select * from business_que where bin = '{0}'", bin1.GetBin());
            pSet.Query+= string.Format(" and tax_year = '{0}' and bin in (select bin from businesses) ", ConfigurationAttributes.CurrentYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from business_que where bin = '{0}'", bin1.GetBin());
                    pSet.Query += string.Format(" and tax_year = '{0}' and bin in (select bin from businesses) ", ConfigurationAttributes.CurrentYear);

                    m_bIsRenPUT = true;
                }
                else
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from businesses where bin = '{0}'", bin1.GetBin());

                    m_bIsRenPUT = true;
                }
            }

            if (m_bIsRenPUT)
            {
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        txtTaxYear.Text = pSet.GetString("tax_year").Trim();
                        
                        result.Query = string.Format("select * from permit_update_appl where bin = '{0}'", bin1.GetBin());
                        result.Query += string.Format(" and tax_year = '{0}' and data_mode = 'QUE'", txtTaxYear.Text.ToString());
                        if (result.Execute())
                        {
                            if (!result.Read())
                            {
                                if (Convert.ToInt32(txtTaxYear.Text.ToString()) < Convert.ToInt32(ConfigurationAttributes.CurrentYear))
                                {
                                    MessageBox.Show("Cannot Proceed! Unrenewed Business.", m_strModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    ClearControls();
                                    btnSearch.Text = "Search";
                                    return;
                                }
                            }
                        }
                        result.Close();
                        

                       

                        m_sPlaceOccupancy = pSet.GetString("place_occupancy");
                        m_sBnsCode = pSet.GetString("bns_code");
                        m_sOwnCode = pSet.GetString("own_code");
                        txtBnsOldType.Text = AppSettingsManager.GetBnsDesc(m_sBnsCode);
                        txtBnsStat.Text = pSet.GetString("bns_stat");
                        txtBnsOldBnsName.Text = pSet.GetString("bns_nm");
                        txtBnsOldLocation.Text = AppSettingsManager.GetBnsAddress(bin1.GetBin());
                        txtBnsOldOwnName.Text = AppSettingsManager.GetBnsOwner(m_sOwnCode);
                        txtBnsOldOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(m_sOwnCode);
                        txtBnsOldOrg.Text = pSet.GetString("orgn_kind");    // RMC 20111004 added permit-update change of orgn kind

                        btnSearch.Text = "Clear";

                        pSet.Close();

                        pSet.Query = string.Format("select * from permit_update_appl where bin = '{0}'", bin1.GetBin());
                        pSet.Query += string.Format(" and tax_year = '{0}' and data_mode = 'QUE'", txtTaxYear.Text.ToString());
                        if (pSet.Execute())
                        {
                            while (pSet.Read())
                            {
                                sApplType = pSet.GetString("appl_type").Trim();

                                if (sApplType == "TOWN")
                                {
                                    m_sNewOwnCode = pSet.GetString("new_own_code").Trim();
                                    txtBnsNewOwnName.Text = AppSettingsManager.GetBnsOwner(m_sNewOwnCode);
                                    txtBnsNewOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(m_sNewOwnCode);
                                    m_bTransOwn = true;
                                }
                                if (sApplType == "TLOC")
                                {
                                    txtBnsNewLocation.Text = pSet.GetString("new_bns_loc").Trim();
                                    m_bTransLoc = true;
                                }
                                if (sApplType == "CTYP")
                                {
                                    m_sNewBnsCode = pSet.GetString("new_bns_code").Trim();
                                    txtBnsNewType.Text = AppSettingsManager.GetBnsDesc(m_sNewBnsCode);
                                    m_bChClass = true;
                                }
                                if (sApplType == "CBNS")
                                {
                                    txtBnsNewBnsName.Text = pSet.GetString("new_bns_name");
                                    m_boChBnsName = true;
                                }
                                if (sApplType == "ADDL")
                                    m_bNewAddl = true;

                                // RMC 20111004 added permit-update change of orgn kind (s)
                                if (sApplType == "CORG")
                                {
                                    txtBnsNewOrg.Text = pSet.GetString("new_orgn_kind");
                                    m_bChOrg = true;
                                }
                                // RMC 20111004 added permit-update change of orgn kind (e)

                                btnCancelApp.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Record not found.", m_strModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        ClearControls();
                        btnSearch.Text = "Search";
                        return;
                    }

                }
            }
            else
            {
                MessageBox.Show("Record not found.", m_strModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                ClearControls();
                btnSearch.Text = "Search";
                return;
            }
        }

        private void ClearControls()
        {
            // RMC 20111004 added permit-update change of orgn kind (s)
            if (TaskMan.IsObjectLock(bin1.GetBin(), "PERMIT_UPDATE", "DELETE", "ASS"))
            {
            }
            // RMC 20111004 added permit-update change of orgn kind (e)

            bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";
            txtBnsStat.Text = "";
            txtTaxYear.Text = "";
            dtpOperationStart.Value = AppSettingsManager.GetCurrentDate();
            txtBnsOldBnsName.Text = "";
            txtBnsNewBnsName.Text = "";
            txtBnsOldOwnName.Text = "";
            txtBnsOldOwnAdd.Text = "";
            txtBnsNewOwnName.Text = "";
            txtBnsNewOwnAdd.Text = "";
            txtBnsOldLocation.Text = "";
            txtBnsNewLocation.Text = "";
            txtBnsOldType.Text = "";
            txtBnsNewType.Text = "";
            txtBnsOldOrg.Text = ""; // RMC 20111004 added permit-update change of orgn kind
            txtBnsNewOrg.Text = ""; // RMC 20111004 added permit-update change of orgn kind
            bin1.txtTaxYear.Focus();
            btnSearch.Text = "Search";
        }

        private void btnAddlBns_Click(object sender, EventArgs e)
        {
            if (ValidateBin())
            {
                using (frmAdditionalBusiness frmAddlBns = new frmAdditionalBusiness())
                {
                    m_sDateOperated = string.Format("{0:MM/dd/yyyy}", dtpOperationStart.Value);

                    frmAddlBns.BIN = bin1.GetBin();
                    frmAddlBns.ApplSave = false;
                    frmAddlBns.txtMainBnsName.Text = txtBnsOldBnsName.Text.ToString();
                    frmAddlBns.txtBnsNature.Text = txtBnsOldType.Text.ToString();
                    frmAddlBns.txtBnsAdd.Text = txtBnsOldLocation.Text.ToString();
                    frmAddlBns.txtBnsOwner.Text = txtBnsOldOwnName.Text.ToString();
                    frmAddlBns.txtBnsOwnAdd.Text = txtBnsOldOwnAdd.Text.ToString();
                    frmAddlBns.txtTaxYear.Text = txtTaxYear.Text.Trim();
                    frmAddlBns.OperationStart = dtpOperationStart.Value;
                    frmAddlBns.txtBnsStat.Text = txtBnsStat.Text.ToString();
                    frmAddlBns.MainBnsDesc = txtBnsOldType.Text.ToString();
                    frmAddlBns.Qtr = AppSettingsManager.GetQtr(m_sDateOperated);
                    frmAddlBns.AddlFlag = true;

                    frmAddlBns.ShowDialog();

                    m_bIsApplSave = frmAddlBns.ApplSave;

                    if (m_bIsApplSave)
                    {
                        SavePermitUpdateAppl();

                        if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            TablesTransferPUT();
                            //this.Close();
                            FormClose();    // RMC 20111004
                        }
                    }
                    
                }
            }
        }

        private void btnApplyNewBnsName_Click(object sender, EventArgs e)
        {
            if (ValidateBin())
            {
                using (frmTransferApp TransferAppFrm = new frmTransferApp())
                {
                    TransferAppFrm.BIN = bin1.GetBin();
                    TransferAppFrm.ApplSave = false;
                    TransferAppFrm.Source = "ApplyNewBnsName";
                    TransferAppFrm.LogIn = m_dTransLogIn;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    TransferAppFrm.BnsStat = txtBnsStat.Text;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    TransferAppFrm.ShowDialog();

                    txtBnsNewBnsName.Text = TransferAppFrm.NewBnsName;
                    m_bIsApplSave = TransferAppFrm.ApplSave;

                    if (m_bIsApplSave)
                    {
                        SavePermitUpdateAppl();

                        //if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        if (!TransferAppFrm.ApplyBilling)    // RMC 20141125 mods in trailing permit-update transaction
                        {
                            TablesTransferPUT();
                            //this.Close();   
                            FormClose();    // RMC 20111004
                        }
                    }

                    LoadValues();
                }
                
            }
        }

        private bool ValidateBin()
        {
            DateTime dtpDate = dtpOperationStart.Value;

            if (bin1.GetBin().Length < 19)
            {
                MessageBox.Show("Enter BIN first.", m_strModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if(Convert.ToInt32(txtTaxYear.Text.ToString()) != Convert.ToInt32(dtpDate.Year))
            {
                MessageBox.Show("Invalid date operated.", m_strModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            pSet.Query = string.Format("select * from business_que where bin = '{0}' and bns_stat = 'RET'", bin1.GetBin());
            if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("Unable to transact retired business.", m_strModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                else
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from businesses where bin = '{0}' and bns_stat = 'RET'", bin1.GetBin());
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            MessageBox.Show("Unable to transact retired business.", m_strModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return false;
                        }
                    }
                    pSet.Close();
                }
            }
            
            return true;
        }

        private void btnApplyNewOwnName_Click(object sender, EventArgs e)
        {
            if (ValidateBin())
            {
                using (frmTransferApp TransferAppFrm = new frmTransferApp())
                {
                    TransferAppFrm.BIN = bin1.GetBin();
                    TransferAppFrm.ApplSave = false;
                    TransferAppFrm.Source = "ApplyNewOwnName";
                    TransferAppFrm.LogIn = m_dTransLogIn;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    TransferAppFrm.BnsStat = txtBnsStat.Text;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    TransferAppFrm.ShowDialog();

                    m_sNewOwnCode = TransferAppFrm.NewOwnCode;
                    txtBnsNewOwnName.Text = AppSettingsManager.GetBnsOwner(m_sNewOwnCode);
                    txtBnsNewOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(m_sNewOwnCode);
                    m_bIsApplSave = TransferAppFrm.ApplSave;

                    if (m_bIsApplSave)
                    {
                        SavePermitUpdateAppl();

                        //if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        if (!TransferAppFrm.ApplyBilling)    // RMC 20141125 mods in trailing permit-update transaction
                        {
                            TablesTransferPUT();
                            // this.Close();
                            FormClose();    // RMC 20111004
                        }
                    }

                    LoadValues();
                }
            }
        }

        private void btnApplyNewBnsLocation_Click(object sender, EventArgs e)
        {
            if (ValidateBin())
            {
                using (frmTransferApp TransferAppFrm = new frmTransferApp())
                {
                    TransferAppFrm.BIN = bin1.GetBin();
                    TransferAppFrm.ApplSave = false;
                    TransferAppFrm.Source = "ApplyNewLocation";
                    TransferAppFrm.LogIn = m_dTransLogIn;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    TransferAppFrm.BnsStat = txtBnsStat.Text;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    TransferAppFrm.ShowDialog();

                    txtBnsNewLocation.Text = TransferAppFrm.NewLocation;
                    m_bIsApplSave = TransferAppFrm.ApplSave;

                    if (m_bIsApplSave)
                    {
                        SavePermitUpdateAppl();

                        //if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        if (!TransferAppFrm.ApplyBilling)    // RMC 20141125 mods in trailing permit-update transaction
                        {
                            TablesTransferPUT();
                            // this.Close();
                            FormClose();    // RMC 20111004
                        }
                    }

                    LoadValues();
                }
            }
        }

        private void btnApplyNewBnsType_Click(object sender, EventArgs e)
        {
            /*pSet.Query = string.Format("select * from taxdues where bin = '{0}' and due_state <> 'P'", bin1.GetBin());
            if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("Settle account due first.", m_strModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

            }*/

            if (ValidateBin())
            {
                using (frmChangeType ChangeTypeFrm = new frmChangeType())
                {
                    ChangeTypeFrm.BIN = bin1.GetBin();
                    ChangeTypeFrm.TaxYear = txtTaxYear.Text.ToString().Trim();
                    ChangeTypeFrm.DateOperated = string.Format("{0:MM/dd/yyyy}", dtpOperationStart.Value);
                    ChangeTypeFrm.ApplSave = false;
                    ChangeTypeFrm.LogIn = m_dTransLogIn;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    ChangeTypeFrm.BnsStat = txtBnsStat.Text;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    ChangeTypeFrm.ShowDialog();

                    m_bIsApplSave = ChangeTypeFrm.ApplSave;

                    if (m_bIsApplSave)
                    {
                        SavePermitUpdateAppl();

                        //if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        if (!ChangeTypeFrm.ApplyBilling) // RMC 20141125 mods in trailing permit-update transaction
                        {
                            TablesTransferPUT();
                            // this.Close();
                            FormClose();    // RMC 20111004
                        }
                    }

                    LoadValues();
                }
            }
        }

        private void btnCancelApp_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel Application?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pSet.Query = string.Format("delete from transfer_table where bin = '{0}'", bin1.GetBin());
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from change_class_tbl where bin = '{0}'", bin1.GetBin());
                if (pSet.ExecuteNonQuery() == 0)
                { }

                //JHB 20181122 cancel bill_gross_info for addl_bns to avoid conflict in case of rev.exam billing(s)
                pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}'", bin1.GetBin());
                pSet.Query += " and bns_code in (select new_bns_code from permit_update_appl";
                pSet.Query += string.Format(" where bin = '{0}' and data_mode = 'QUE')", bin1.GetBin());
                if (pSet.ExecuteNonQuery() == 0)
                { }
                //JHB 20181122 cancel bill_gross_info for addl_bns to avoid conflict in case of rev.exam billing(e)


                pSet.Query = string.Format("delete from addl_bns_que where bin = '{0}'", bin1.GetBin());
                pSet.Query+= " and bns_code_main in (select new_bns_code from permit_update_appl";
                pSet.Query+= string.Format(" where bin = '{0}' and data_mode = 'QUE')", bin1.GetBin());
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from permit_update_appl where bin = '{0}' and data_mode = 'QUE'", bin1.GetBin());
                if (pSet.ExecuteNonQuery() == 0)
                { }
               


                pSet.Query = string.Format("delete from taxdues where bin = '{0}' and tax_year = '{1}'", bin1.GetBin(), txtTaxYear.Text.ToString());
                if (m_bIsRenPUT) //MCR 20140512 MAT-14-4982 old (!m_bIsRenPUT)
                    pSet.Query += " and due_state = 'P'";
                if (pSet.ExecuteNonQuery() == 0)
                { }

                MessageBox.Show("Application has been canceled.", "Permit Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearControls();

                if (AuditTrail.InsertTrail("AACPUT", "ASS-CANCEL APPLICATION-PUT", bin1.GetBin()) == 0)
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }	
        }

        private void SavePermitUpdateAppl()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sNewBnsCode = string.Empty;
            string sOldBnsCode = string.Empty;
            string sQtr = string.Empty;
            string sTransAppCode = string.Empty;
            string sApplType = string.Empty;
            string sPrevOwnCode = string.Empty;
            string sNewOwnCode = string.Empty;
            DateTime dtDateOperated;
            
            m_sDateOperated = string.Format("{0:MM/dd/yyyy}",dtpOperationStart.Value);
            dtDateOperated = Convert.ToDateTime(m_sDateOperated);
                      
            pSet.Query = string.Format("delete from permit_update_appl where bin = '{0}' and data_mode = 'QUE'", bin1.GetBin());
            if(pSet.ExecuteNonQuery() == 0)
            {}

            m_bNewAddl = false;
            m_bTransLoc = false;
            m_bTransOwn = false;
            m_bChClass = false;
            
            // ADDL
            pSet.Query = string.Format("select * from addl_bns_que where bin = '{0}'", bin1.GetBin());
            pSet.Query += " and bin not in (select bin from change_class_tbl";
            pSet.Query += string.Format(" where bin = '{0}' and is_main = 'N')", bin1.GetBin());
            if(pSet.Execute())
            {
                while(pSet.Read())
                {
                    sNewBnsCode = pSet.GetString("bns_code_main").Trim();
                    sQtr = pSet.GetString("qtr").Trim();
                    
                    if (sQtr != AppSettingsManager.GetQtr(m_sDateOperated))
                    {
                        sQtr = AppSettingsManager.GetQtr(m_sDateOperated);

                        pRec.Query = string.Format("update addl_bns_que set qtr = '{0}' where bin = '{1}'", sQtr, bin1.GetBin());
                        if(pRec.ExecuteNonQuery() == 0)
                        {}
                    }
                
                    pRec.Query = "insert into permit_update_appl(bin,tax_year,appl_type,new_bns_code,dt_operated,data_mode,permit_no,bns_user,dt_save) values (:1,:2,:3,:4,:5,:6,:7,:8,:9)";		
                    pRec.AddParameter(":1", bin1.GetBin());
                    pRec.AddParameter(":2", txtTaxYear.Text.ToString().Trim());
                    pRec.AddParameter(":3", "ADDL");
                    pRec.AddParameter(":4", sNewBnsCode);
                    pRec.AddParameter(":5", dtDateOperated);
                    pRec.AddParameter(":6", "QUE");
                    pRec.AddParameter(":7", "NONE");
                    pRec.AddParameter(":8", AppSettingsManager.SystemUser.UserCode);
                    pRec.AddParameter(":9", AppSettingsManager.GetCurrentDate());
                    if(pRec.ExecuteNonQuery() == 0)
                    {}

                    m_bNewAddl = true;
                }
            }
            pSet.Close();

            // CTYP
            pSet.Query = string.Format("select * from change_class_tbl where bin = '{0}'", bin1.GetBin());
            if(pSet.Execute())
            {
                while (pSet.Read())
                {
                    sOldBnsCode = pSet.GetString("old_bns_code").Trim();
                    sNewBnsCode = pSet.GetString("new_bns_code").Trim();
                    
                    pRec.Query = "insert into permit_update_appl(bin,tax_year,appl_type,old_bns_code,new_bns_code,dt_operated,data_mode,permit_no,bns_user,dt_save) values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10)";	
                    pRec.AddParameter(":1", bin1.GetBin());
                    pRec.AddParameter(":2", txtTaxYear.Text.ToString().Trim());
                    pRec.AddParameter(":3", "CTYP");
                    pRec.AddParameter(":4", sOldBnsCode);
                    pRec.AddParameter(":5", sNewBnsCode);
                    pRec.AddParameter(":6", dtDateOperated);
                    pRec.AddParameter(":7", "QUE");
                    pRec.AddParameter(":8", "NONE");
                    pRec.AddParameter(":9", AppSettingsManager.SystemUser.UserCode);
                    pRec.AddParameter(":10", AppSettingsManager.GetCurrentDate());
                    if(pRec.ExecuteNonQuery() == 0)
                    {}

                    m_bChClass = true;
                }
            }
            pSet.Close();
            
            pSet.Query = string.Format("select * from transfer_table where bin = '{0}'", bin1.GetBin());
            if(pSet.Execute())
            {
                while (pSet.Read())
                {
                    sTransAppCode = pSet.GetString("trans_app_code");

                    if (sTransAppCode == "TO")
                        sApplType = "TOWN";
                    else if (sTransAppCode == "TL")
                        sApplType = "TLOC";
                    else if (sTransAppCode == "CB")
                        sApplType = "CBNS";
                    else if (sTransAppCode == "CO") // RMC 20111004 added permit-update change of orgn kind
                        sApplType = "CORG"; // RMC 20111004 added permit-update change of orgn kind
                    
                    if (sApplType == "TOWN")
                    {
                        sPrevOwnCode = pSet.GetString("prev_own_code");
                        sNewOwnCode = pSet.GetString("new_own_code");

                        pRec.Query = "insert into permit_update_appl(bin,tax_year,appl_type,old_own_code,new_own_code,dt_operated,data_mode,permit_no,bns_user,dt_save) values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10)";
                        pRec.AddParameter(":1", bin1.GetBin());
                        pRec.AddParameter(":2", txtTaxYear.Text.ToString().Trim());
                        pRec.AddParameter(":3", sApplType);
                        pRec.AddParameter(":4", sPrevOwnCode);
                        pRec.AddParameter(":5", sNewOwnCode);
                        pRec.AddParameter(":6", dtDateOperated);
                        pRec.AddParameter(":7", "QUE");
                        pRec.AddParameter(":8", "NONE");
                        pRec.AddParameter(":9", AppSettingsManager.SystemUser.UserCode);
                        pRec.AddParameter(":10", AppSettingsManager.GetCurrentDate());
                        if(pRec.ExecuteNonQuery() == 0)
                        {}

                        m_bTransOwn = true;
                    }
                    if (sApplType == "TLOC")
                    {
                        pRec.Query = "insert into permit_update_appl(bin,tax_year,appl_type,old_bns_loc,new_bns_loc,dt_operated,data_mode,permit_no,bns_user,dt_save) values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10)";
                        pRec.AddParameter(":1", bin1.GetBin());
                        pRec.AddParameter(":2", txtTaxYear.Text.ToString().Trim());
                        pRec.AddParameter(":3", sApplType);
                        pRec.AddParameter(":4", txtBnsOldLocation.Text.ToString().Trim());
                        pRec.AddParameter(":5", txtBnsNewLocation.Text.ToString().Trim());
                        pRec.AddParameter(":6", dtDateOperated);
                        pRec.AddParameter(":7", "QUE");
                        pRec.AddParameter(":8", "NONE");
                        pRec.AddParameter(":9", AppSettingsManager.SystemUser.UserCode);
                        pRec.AddParameter(":10", AppSettingsManager.GetCurrentDate());
                        if(pRec.ExecuteNonQuery() == 0)
                        {}
                                           
                        m_bTransLoc = true;
                    }
                    if (sApplType == "CBNS")
                    {
                        pRec.Query = "insert into permit_update_appl(bin,tax_year,appl_type,dt_operated,data_mode,permit_no,bns_user,dt_save,new_bns_name,old_bns_name) values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10)";
                        pRec.AddParameter(":1", bin1.GetBin());
                        pRec.AddParameter(":2", txtTaxYear.Text.ToString().Trim());
                        pRec.AddParameter(":3", sApplType);
                        pRec.AddParameter(":4", dtDateOperated);
                        pRec.AddParameter(":5", "QUE");
                        pRec.AddParameter(":6", "NONE");
                        pRec.AddParameter(":7", AppSettingsManager.SystemUser.UserCode);
                        pRec.AddParameter(":8", AppSettingsManager.GetCurrentDate());
                        pRec.AddParameter(":9", StringUtilities.HandleApostrophe(txtBnsNewBnsName.Text.ToString().Trim()));
                        pRec.AddParameter(":10", StringUtilities.HandleApostrophe(txtBnsOldBnsName.Text.ToString().Trim()));
                        if(pRec.ExecuteNonQuery() == 0)
                        {}
                        m_bTransLoc = true;
                    }
                    // RMC 20111004 added permit-update change of orgn kind (s)
                    if (sApplType == "CORG")
                    {
                        pRec.Query = "insert into permit_update_appl(bin,tax_year,appl_type,dt_operated,data_mode,permit_no,bns_user,dt_save, new_orgn_kind, old_orgn_kind) values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10)";
                        pRec.AddParameter(":1", bin1.GetBin());
                        pRec.AddParameter(":2", txtTaxYear.Text.ToString().Trim());
                        pRec.AddParameter(":3", sApplType);
                        pRec.AddParameter(":4", dtDateOperated);
                        pRec.AddParameter(":5", "QUE");
                        pRec.AddParameter(":6", "NONE");
                        pRec.AddParameter(":7", AppSettingsManager.SystemUser.UserCode);
                        pRec.AddParameter(":8", AppSettingsManager.GetCurrentDate());
                        pRec.AddParameter(":9", txtBnsNewOrg.Text.ToString().Trim());
                        pRec.AddParameter(":10", txtBnsOldOrg.Text.ToString().Trim());
                        if (pRec.ExecuteNonQuery() == 0)
                        { }

                        m_bChOrg = true;
                    }
                    // RMC 20111004 added permit-update change of orgn kind (e)

                }
            }
            pSet.Close();

            
        }

        private void TablesTransferPUT()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            OracleResultSet pRecPUT = new OracleResultSet();
            OracleResultSet pRecBlast = new OracleResultSet();

            string sApplType = string.Empty;
            string sNewBnsCode = string.Empty;

            pRec.Query = "select * from permit_update_appl";
            pRec.Query += string.Format(" where bin = '{0}' and data_mode = 'QUE'", bin1.GetBin());
            if(pRec.Execute())
            {
                while(pRec.Read())
                {
                    sApplType = pRec.GetString("appl_type");
                    sNewBnsCode = pRec.GetString("new_bns_code");

                    if (sApplType == "ADDL")
                    {
                        pRec2.Query = "insert into addl_bns";
                        pRec2.Query += " select * from addl_bns_que";
                        pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                        pRec2.Query += string.Format(" and bns_code_main = '{0}'", sNewBnsCode);
                        if (pRec2.ExecuteNonQuery() == 0)
                        { }
                        
                        pRec2.Query = "delete from addl_bns_que";
                        pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                        pRec2.Query += string.Format(" and bns_code_main = '{0}'", sNewBnsCode);
                        if (pRec2.ExecuteNonQuery() == 0)
                        { }
                        
                    }
                    
                    if (sApplType == "CTYP")
                        SaveChangeClass(sNewBnsCode);
                    
                    if (sApplType == "TLOC")
                    {
                        if(GetTransTableValues(sApplType))
                        {
                            InsertTransHist(sApplType);
                            InsertBnsLocHist(sApplType);

                            pRec2.Query = "update businesses set";
                            pRec2.Query += string.Format(" bns_house_no = '{0}',", StringUtilities.HandleApostrophe(sTrAddrNo));
                            pRec2.Query += string.Format(" bns_street = '{0}',", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(sTrAddrStreet)));
                            pRec2.Query += string.Format(" bns_brgy = '{0}',", sTrAddrBrgy);
                            pRec2.Query += string.Format(" bns_zone = '{0}',", StringUtilities.SetEmptyToSpace(sTrAddrZone));
                            pRec2.Query += string.Format(" bns_dist = '{0}',", sTrAddrDist);
                            pRec2.Query += string.Format(" bns_mun = '{0}',", sTrAddrMun);
                            pRec2.Query += string.Format(" bns_prov = '{0}'", StringUtilities.SetEmptyToSpace(sTrAddrProv));
                            pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                            if (pRec2.ExecuteNonQuery() == 0)
                            { }
                        }

                    }

                    if (sApplType == "TOWN")
                    {
                        if(GetTransTableValues(sApplType))
                        {
                            string sDtSave;

                            pRecPUT.Query = "select * from permit_update_appl ";
                            pRecPUT.Query += string.Format("where bin = '{0}' and appl_type = '{1}'", sTrBIN, sApplType);
                            if(pRecPUT.Execute())
                            {
                                if (pRecPUT.Read())
                                {
                                    sTaxYear = pRecPUT.GetString("tax_year");
                                    sDtSave = string.Format("{0:MM/dd/yyyy}", pRecPUT.GetDateTime("dt_save"));
                                }
                            }
                            pRecPUT.Close();

                            pRecPUT.Query = string.Format("select distinct * from pay_hist where bin = '{0}' order by date_posted desc", sTrBIN);
                            if (pRecPUT.Execute())
                            {
                                if (pRecPUT.Read())
                                {
                                    sQtrPaid = pRecPUT.GetString("qtr_paid");
                                }
                            }
                            pRecPUT.Close();

                            InsertTransHist(sApplType);
                            InsertBnsLocHist(sApplType);
                                                        
                            // update businesses
                            if (m_sPlaceOccupancy == "OWNED")
                            {
                                pRec2.Query = "update businesses set";
                                pRec2.Query += string.Format(" own_code = '{0}',", sTrNewOwnCode);
                                pRec2.Query += string.Format(" busn_own = '{0}',", sTrNewOwnCode);
                                pRec2.Query += string.Format(" prev_bns_own = '{0}'", sTrPrevOwnCode);
                                //pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership, transferred
                            }
                            else
                            {
                                pRec2.Query = "update businesses set";
                                pRec2.Query += string.Format(" own_code = '{0}',", sTrNewOwnCode);
                                pRec2.Query += string.Format(" prev_bns_own = '{0}'", sTrPrevOwnCode);
                                //pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership, transferred
                            }

                            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (s)
                            if (sTrDTINo.Trim() != "")
                                pRec2.Query += string.Format(", dti_reg_no = '{0}', dti_reg_dt = to_date('{1}','MM/dd/yyyy')", sTrDTINo, dTrDTIDt.ToShortDateString());

                            if (sTrMemo.Trim() != "")
                                pRec2.Query += string.Format(", memoranda = '{0}'", sTrMemo);
                            pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (e)
                            
                            if (pRec2.ExecuteNonQuery() == 0)
                            { }
                        }
                        


                    }
                    
                    if (sApplType == "CBNS")
                    {
                        string sNewBnsName;

                        if (GetTransTableValues(sApplType))
                        {
                            InsertTransHist(sApplType);
                            InsertBnsLocHist(sApplType);

                            // update businesses
                            pRec2.Query = "update businesses set";
                            pRec2.Query += string.Format(" bns_nm = '{0}'", StringUtilities.HandleApostrophe(txtBnsNewBnsName.Text.ToString().Trim()));

                            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (s)
                            if (sTrDTINo.Trim() != "")
                                pRec2.Query += string.Format(", dti_reg_no = '{0}', dti_reg_dt = to_date('{1}','MM/dd/yyyy')", sTrDTINo, dTrDTIDt.ToShortDateString());

                            if (sTrMemo.Trim() != "")
                                pRec2.Query += string.Format(", memoranda = '{0}'", sTrMemo);
                            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (e)

                            pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                            if (pRec2.ExecuteNonQuery() == 0)
                            { }
                        }
                    }

                    // RMC 20111004 added permit-update change of orgn kind (s)
                    if (sApplType == "CORG")
                    {
                        if (GetTransTableValues(sApplType))
                        {
                            InsertTransHist(sApplType);
                            InsertBnsLocHist(sApplType);

                            // update businesses
                            pRec2.Query = "update businesses set";
                            pRec2.Query += string.Format(" orgn_kind = '{0}'", StringUtilities.HandleApostrophe(txtBnsNewOrg.Text.ToString().Trim()));
                            pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                            if (pRec2.ExecuteNonQuery() == 0)
                            { }
                        }
                    }
                    // RMC 20111004 added permit-update change of orgn kind (e)
                }

                pRec2.Query = "update permit_update_appl set data_mode = 'PAID',";
                pRec2.Query += " or_no = 'VOID'";
                pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                pRec2.Query += " and data_mode = 'QUE'";
                if (pRec2.ExecuteNonQuery() == 0)
                { }
                
            }
            pRec.Close();
        }

        private bool GetTransTableValues(string sApplType)
        {
            OracleResultSet pRecPUT = new OracleResultSet();
            sTrBIN = "";
            sTrAppCode = "";
            sTrBnsCode = "";
            sTrPrevOwnCode = "";
            sTrNewOwnCode = "";
            sTrOwnLn = "";
            sTrOwnFn = "";
            sTrOwnMi = "";
            sTrAddrNo = "";
            sTrAddrStreet = "";
            sTrAddrBrgy = "";
            sTrAddrZone = "";
            sTrAddrDist = "";
            sTrAddrMun = "";
            sTrAddrProv = "";
            sTrAppDate = "";
            sTaxYear = "";
            sQtrPaid = "";
            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (s)
            sTrDTINo = string.Empty; 
            dTrDTIDt = AppSettingsManager.GetCurrentDate();
            sTrMemo = string.Empty;
            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (e)

            pRecPUT.Query = "select * from transfer_table";
            pRecPUT.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
            if (sApplType == "TLOC")
                pRecPUT.Query += " and trans_app_code = 'TL'";
            if (sApplType == "TOWN")
                pRecPUT.Query += " and trans_app_code = 'TO'";
            if (sApplType == "CBNS")
                pRecPUT.Query += " and trans_app_code = 'CB'";
            if (sApplType == "CORG")    // RMC 20111004 added permit-update change of orgn kind
                pRecPUT.Query += " and trans_app_code = 'CO'";  // RMC 20111004 added permit-update change of orgn kind

            if (pRecPUT.Execute())
            {
                if (pRecPUT.Read())
                {
                    sTrBIN = pRecPUT.GetString("bin");
                    sTrAppCode = pRecPUT.GetString("trans_app_code");
                    sTrBnsCode = pRecPUT.GetString("bns_code");
                    sTrPrevOwnCode = pRecPUT.GetString("prev_own_code");
                    sTrNewOwnCode = pRecPUT.GetString("new_own_code");
                    sTrOwnLn = pRecPUT.GetString("own_ln");
                    sTrOwnFn = pRecPUT.GetString("own_fn");
                    sTrOwnMi = pRecPUT.GetString("own_mi");
                    sTrAddrNo = pRecPUT.GetString("addr_no");
                    sTrAddrStreet = pRecPUT.GetString("addr_street");
                    sTrAddrBrgy = pRecPUT.GetString("addr_brgy");
                    sTrAddrZone = pRecPUT.GetString("addr_zone");
                    sTrAddrDist = pRecPUT.GetString("addr_dist");
                    sTrAddrMun = pRecPUT.GetString("addr_mun");
                    sTrAddrProv = pRecPUT.GetString("addr_prov");
                    sTrAppDate = pRecPUT.GetString("app_date");
                }
                else
                    return false;
            }
            pRecPUT.Close();

            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (s)
            if (sApplType == "TOWN" || sApplType == "CBNS")
            {
                pRecPUT.Query = "select * from TRANS_OTHER_INFO_ADDL where bin = '" + bin1.GetBin() + "'";
                pRecPUT.Query += " and tax_year = '" + txtTaxYear.Text + "' and (default_code = 'DTINO' or default_code = 'DTIDT' or default_code = 'MEMO')";
                if (pRecPUT.Execute())
                {
                    while (pRecPUT.Read())
                    {
                        if(pRecPUT.GetString("default_code") == "DTINO")
                            sTrDTINo = pRecPUT.GetString("data");
                        if (pRecPUT.GetString("default_code") == "DTIDT")
                        {
                            try
                            {
                                dTrDTIDt = Convert.ToDateTime(pRecPUT.GetString("data"));
                            }
                            catch
                            {
                                dTrDTIDt = AppSettingsManager.GetCurrentDate();
                            }
                        }
                            
                        if (pRecPUT.GetString("default_code") == "MEMO")
                            sTrMemo = pRecPUT.GetString("data");
                    }
                }
                pRecPUT.Close();
            }
            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (e)

            return true;
        }

        private void InsertTransHist(string sApplType)
        {
            OracleResultSet pRec2 = new OracleResultSet();
                                                     
            pRec2.Query = "insert into transfer_hist(bin,trans_app_code,bns_code,prev_own_code,new_own_code,own_ln,Own_fn,own_mi,addr_no,addr_street,addr_brgy,addr_zone,addr_dist,addr_mun,addr_prov,app_date,trans_date,tax_year,qtr_paid,or_no) values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14,:15,:16,:17,:18,:19,:20)";
            pRec2.AddParameter(":1", sTrBIN);
            pRec2.AddParameter(":2", sTrAppCode);
            pRec2.AddParameter(":3", StringUtilities.SetEmptyToSpace(sTrBnsCode));
            pRec2.AddParameter(":4", StringUtilities.SetEmptyToSpace(sTrPrevOwnCode));
            pRec2.AddParameter(":5", StringUtilities.SetEmptyToSpace(sTrNewOwnCode));
            pRec2.AddParameter(":6", StringUtilities.HandleApostrophe(sTrOwnLn));
            pRec2.AddParameter(":7", StringUtilities.SetEmptyToSpace(sTrOwnFn));
            pRec2.AddParameter(":8", StringUtilities.SetEmptyToSpace(sTrOwnMi));
            pRec2.AddParameter(":9", StringUtilities.HandleApostrophe(sTrAddrNo));
            pRec2.AddParameter(":10", StringUtilities.HandleApostrophe(sTrAddrStreet));
            pRec2.AddParameter(":11", StringUtilities.SetEmptyToSpace(sTrAddrBrgy));
            pRec2.AddParameter(":12", StringUtilities.SetEmptyToSpace(sTrAddrZone));
            pRec2.AddParameter(":13", StringUtilities.SetEmptyToSpace(sTrAddrDist));
            pRec2.AddParameter(":14", StringUtilities.SetEmptyToSpace(sTrAddrMun));
            pRec2.AddParameter(":15", StringUtilities.SetEmptyToSpace(sTrAddrProv));
            pRec2.AddParameter(":16", sTrAppDate);
            pRec2.AddParameter(":17", StringUtilities.SetEmptyToSpace(sTrAppDate));
            pRec2.AddParameter(":18", StringUtilities.SetEmptyToSpace(sTaxYear));
            pRec2.AddParameter(":19", StringUtilities.SetEmptyToSpace(sQtrPaid));
            pRec2.AddParameter(":20", "VOID");
            if (pRec2.ExecuteNonQuery() == 0)
            { }

            pRec2.Query = "delete from transfer_table";
            pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
            if (sApplType == "TLOC")
                pRec2.Query += " and trans_app_code = 'TL'";
            if (sApplType == "TOWN")
                pRec2.Query += " and trans_app_code = 'TO'";
            if (sApplType == "CBNS")
                pRec2.Query += " and trans_app_code = 'CB'";
            if (sApplType == "CORG")    // RMC 20111004 added permit-update change of orgn kind
                pRec2.Query += " and trans_app_code = 'CO'";    // RMC 20111004 added permit-update change of orgn kind

            if (pRec2.ExecuteNonQuery() == 0)
            { }
        }

        private void InsertBnsLocHist(string sApplType)
        {
            OracleResultSet pRecBlast = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();

            string sLastOwnCode = string.Empty;
            string sLastBusnOwn = string.Empty;
            string sLastPrevBnsOwn = string.Empty; 
            string sLastAddrNo = string.Empty;
            string sLastAddrStreet = string.Empty;
            string sLastAddrBrgy = string.Empty;
            string sLastAddrZone = string.Empty;
            string sLastAddrDist = string.Empty;
            string sLastAddrMun = string.Empty;
            string sLastAddrProv = string.Empty;
            string sApplCode = string.Empty;

            if (sApplType == "TLOC")
                sApplCode = "TL";
            if (sApplType == "TOWN")
                sApplCode = "TO";
            if (sApplType == "CBNS")
                sApplCode = "CB";
            if (sApplType == "CORG")    // RMC 20111004 added permit-update change of orgn kind
                sApplCode = "CO";   // RMC 20111004 added permit-update change of orgn kind

            pRecBlast.Query = string.Format("select * from businesses where bin = '{0}'", bin1.GetBin());
            if (pRecBlast.Execute())
            {
                if (pRecBlast.Read())
                {
                    sLastOwnCode = pRecBlast.GetString("own_code");
                    sLastBusnOwn = pRecBlast.GetString("busn_own");
                    sLastPrevBnsOwn = pRecBlast.GetString("prev_bns_own");
                    sLastAddrNo = pRecBlast.GetString("bns_house_no");
                    sLastAddrStreet = pRecBlast.GetString("bns_street");
                    sLastAddrBrgy = pRecBlast.GetString("bns_brgy");
                    sLastAddrZone = pRecBlast.GetString("bns_zone");
                    sLastAddrDist = pRecBlast.GetString("bns_dist");
                    sLastAddrMun = pRecBlast.GetString("bns_mun");
                    sLastAddrProv = pRecBlast.GetString("bns_prov");

                    if (sApplType == "CBNS")
                    {
                        sLastAddrNo = pRecBlast.GetString("bns_nm");
                        sLastOwnCode = "";
                        sLastBusnOwn = "";
                        sLastPrevBnsOwn = "";
                        sLastAddrStreet = "";
                        sLastAddrBrgy = "";
                        sLastAddrZone = "";
                        sLastAddrDist = "";
                        sLastAddrMun = "";
                        sLastAddrProv = "";
                    }
                    // RMC 20111004 added permit-update change of orgn kind (s)
                    if (sApplType == "CORG")
                    {
                        sLastAddrNo = pRecBlast.GetString("orgn_kind");
                        sLastOwnCode = "";
                        sLastBusnOwn = "";
                        sLastPrevBnsOwn = "";
                        sLastAddrStreet = "";
                        sLastAddrBrgy = "";
                        sLastAddrZone = "";
                        sLastAddrDist = "";
                        sLastAddrMun = "";
                        sLastAddrProv = "";
                    }
                    // RMC 20111004 added permit-update change of orgn kind (e)

                    pRec2.Query = "insert into bns_loc_last values (:1,'VOID',:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12)";
                    pRec2.AddParameter(":1", bin1.GetBin());
                    pRec2.AddParameter(":2", sApplCode);
                    pRec2.AddParameter(":3", StringUtilities.SetEmptyToSpace(sLastOwnCode));
                    pRec2.AddParameter(":4", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(sLastBusnOwn)));
                    pRec2.AddParameter(":5", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(sLastPrevBnsOwn)));
                    pRec2.AddParameter(":6", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(sLastAddrNo)));
                    pRec2.AddParameter(":7", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(sLastAddrStreet)));
                    pRec2.AddParameter(":8", StringUtilities.SetEmptyToSpace(sLastAddrBrgy));
                    pRec2.AddParameter(":9", StringUtilities.SetEmptyToSpace(sLastAddrZone));
                    pRec2.AddParameter(":10", StringUtilities.SetEmptyToSpace(sLastAddrDist));
                    pRec2.AddParameter(":11", StringUtilities.SetEmptyToSpace(sLastAddrMun));
                    pRec2.AddParameter(":12", StringUtilities.SetEmptyToSpace(sLastAddrProv));
                    if (pRec2.ExecuteNonQuery() == 0)
                    { }

                }
            }
            pRecBlast.Close();
        }

        private void SaveChangeClass(string p_sNewBnsCode)
        {
            OracleResultSet pRecPUT = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            OracleResultSet pRecAddl = new OracleResultSet();
            double dAddlCapital = 0;
            double dAddlGross = 0;
            double dCcCapital = 0;
            string sCcOldBnsCode = string.Empty;
            string sCcStatus = string.Empty;
            string sCcIsMain = string.Empty;
            string sCcAppDate = string.Empty;
            string sAddlStatus = string.Empty;
            string sAddlQtr = string.Empty;

            pRecPUT.Query = "select * from change_class_tbl";
            pRecPUT.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
            pRecPUT.Query += string.Format(" and new_bns_code = '{0}'", p_sNewBnsCode);
            if(pRecPUT.Execute())
            {
                if (pRecPUT.Read())
                {
                    sCcOldBnsCode = pRecPUT.GetString("old_bns_code");
                    dCcCapital = pRecPUT.GetDouble("capital");
                    sCcStatus = pRecPUT.GetString("status");
                    sCcIsMain = pRecPUT.GetString("is_main");
                    sCcAppDate = string.Format("{0:MM/dd/yyyy}", pRecPUT.GetDateTime("app_date"));

                    if (sCcIsMain == "Y")
                    {
                        pRec2.Query = string.Format("update businesses set bns_code = '{0}'", p_sNewBnsCode);
                        pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                        if (pRec2.ExecuteNonQuery() == 0)
                        { }
                    }
                    else
                    {
                        pRec2.Query = "insert into addl_bns";
                        pRec2.Query += " select * from addl_bns_que";
                        pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                        pRec2.Query += string.Format(" and bns_code_main = '{0}'", p_sNewBnsCode);
                        if (pRec2.ExecuteNonQuery() == 0)
                        { }

                        pRecAddl.Query = "select * from addl_bns";
                        pRecAddl.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                        pRecAddl.Query += string.Format(" and bns_code_main = '{0}'", sCcOldBnsCode);
                        pRecAddl.Query += string.Format(" and tax_year = '{0}'", txtTaxYear.Text.Trim());
                        if (pRecAddl.Execute())
                        {
                            while (pRecAddl.Read())
                            {
                                dAddlCapital = pRecAddl.GetDouble("capital");
                                dAddlGross = pRecAddl.GetDouble("gross");
                                sAddlStatus = pRecAddl.GetString("bns_stat");
                                sAddlQtr = pRecAddl.GetString("qtr");

                                //pRec2.Query = "insert into addl_bns_hist values (:1,:2,:3,:4,:5,:6,:7,'VOID')";
                                //pRec2.Query = "insert into addl_bns_hist values (:1,:2,:3,:4,:5,:6,:7,'VOID','VOID')";  // RMC 20151005 mods in retirement module
                                pRec2.Query = "insert into addl_bns_hist values (:1,:2,:3,:4,:5,:6,:7,'VOID',0)";   // RMC 20180126 correction in PU application change of line of business
                                pRec2.AddParameter(":1", bin1.GetBin());
                                pRec2.AddParameter(":2", sCcOldBnsCode);
                                pRec2.AddParameter(":3", dAddlCapital);
                                pRec2.AddParameter(":4", dAddlGross);
                                pRec2.AddParameter(":5", txtTaxYear.Text.Trim());
                                pRec2.AddParameter(":6", sAddlStatus);
                                pRec2.AddParameter(":7", sAddlQtr);
                                if (pRec2.ExecuteNonQuery() == 0)
                                { }

                            }
                        }
                        pRecAddl.Close();

                        pRec2.Query = "delete from addl_bns";
                        pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                        pRec2.Query += string.Format(" and bns_code_main = '{0}'", sCcOldBnsCode);
                        pRec2.Query += string.Format(" and tax_year = '{0}'", txtTaxYear.Text.Trim());
                        if (pRec2.ExecuteNonQuery() == 0)
                        { }

                        pRec2.Query = "delete from addl_bns_que";
                        pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                        pRec2.Query += string.Format(" and bns_code_main = '{0}'", p_sNewBnsCode);
                        if (pRec2.ExecuteNonQuery() == 0)
                        { }
                    }

                    pRec2.Query = "insert into change_class_hist values (:1,:2,:3,:4,:5,:6,to_date(:7,'MM/dd/yyyy'),'VOID')";
                    pRec2.AddParameter(":1", bin1.GetBin());
                    pRec2.AddParameter(":2", sCcOldBnsCode);
                    pRec2.AddParameter(":3", p_sNewBnsCode);
                    pRec2.AddParameter(":4", dCcCapital);
                    pRec2.AddParameter(":5", sCcStatus);
                    pRec2.AddParameter(":6", sCcIsMain);
                    pRec2.AddParameter(":7", sCcAppDate);
                    if (pRec2.ExecuteNonQuery() == 0)
                    { }

                    pRec2.Query = "delete from change_class_tbl";
                    pRec2.Query += string.Format(" where bin = '{0}'", bin1.GetBin());
                    pRec2.Query += string.Format(" and new_bns_code = '{0}'", p_sNewBnsCode);
                    if (pRec2.ExecuteNonQuery() == 0)
                    { }

                }
            }
            pRecPUT.Close();
        }

        private void frmPermitUpdate_Load(object sender, EventArgs e)
        {
            dtpOperationStart.Value = AppSettingsManager.GetCurrentDate();  // RMC 20110809
        }

        private void btnApplyNewBnsOrg_Click(object sender, EventArgs e)
        {
            if (ValidateBin())
            {
                using (frmTransferApp TransferAppFrm = new frmTransferApp())
                {
                    TransferAppFrm.BIN = bin1.GetBin();
                    TransferAppFrm.ApplSave = false;
                    TransferAppFrm.Source = "ApplyNewOrgKind";
                    TransferAppFrm.LogIn = m_dTransLogIn;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    TransferAppFrm.BnsStat = txtBnsStat.Text;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    TransferAppFrm.ShowDialog();

                    txtBnsNewOrg.Text = TransferAppFrm.NewOrgnKind;
                    m_bIsApplSave = TransferAppFrm.ApplSave;

                    if (m_bIsApplSave)
                    {
                        SavePermitUpdateAppl();

                        //if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        if (!TransferAppFrm.ApplyBilling)    // RMC 20141125 mods in trailing permit-update transaction
                        {
                            TablesTransferPUT();
                            //this.Close();
                            FormClose();    // RMC 20111004
                        }
                    }

                    LoadValues();
                }
            }
        }

        private void FormClose()
        {
            // RMC 20111004 added permit-update change of orgn kind
            if (TaskMan.IsObjectLock(bin1.GetBin(), "PERMIT_UPDATE", "DELETE", "ASS"))
            {
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void containerWithShadow1_Load(object sender, EventArgs e)
        {

        }
    }
}