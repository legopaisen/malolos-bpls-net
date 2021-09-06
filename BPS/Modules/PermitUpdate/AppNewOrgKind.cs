using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.PermitUpdate
{
    public class AppNewOrgKind:TransferApp
    {
        OracleResultSet pSet = new OracleResultSet();
        private string m_sInitialPUTValue = string.Empty;

        public AppNewOrgKind(frmTransferApp Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            TransferAppFrm.lblOldTitle.Text = "Previous Business Name";
            TransferAppFrm.lblOldLastName.Text = "Bns Name";
            TransferAppFrm.lblOldFirstName.Text = "Bns Type";

            TransferAppFrm.lblNewTitle.Text = "New Business Name";
            TransferAppFrm.lblNewLastName.Text = "Bns Name";
            TransferAppFrm.lblNewFirstName.Text = "Bns Type";

            TransferAppFrm.ClearPrevious();
            TransferAppFrm.ClearNew();

            TransferAppFrm.bin1.txtTaxYear.ReadOnly = true;
            TransferAppFrm.bin1.txtBINSeries.ReadOnly = true;
            TransferAppFrm.bin2.txtTaxYear.ReadOnly = true;
            TransferAppFrm.bin2.txtBINSeries.ReadOnly = true;
            TransferAppFrm.lblNewOrg.Visible = true;
            TransferAppFrm.cmbNewOrg.Visible = true;
            TransferAppFrm.cmbNewOrg.Enabled = true;

            LoadValues();
            
        }

        private void LoadValues()
        {
            m_sInitialPUTValue = "";

            pSet.Query = "select bns_nm,bns_house_no,bns_street,bns_mun,bns_dist,bns_zone,bns_brgy,bns_prov,tax_year,business_que.bns_code,bns_desc from business_que,bns_table";
            pSet.Query += string.Format(" where business_que.bns_code = bns_table.bns_code and fees_code = 'B' and bin = '{0}'", TransferAppFrm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = "select bns_nm,bns_house_no,bns_street,bns_mun,bns_dist,bns_zone,bns_brgy,bns_prov,tax_year,business_que.bns_code,bns_desc,bns_zip, orgn_kind from business_que,bns_table";   // RMC 20110803 added bns_zip
                    pSet.Query += string.Format(" where business_que.bns_code = bns_table.bns_code and fees_code = 'B' and bin = '{0}'", TransferAppFrm.BIN);   
                }
                else
                {
                    pSet.Close();

                    pSet.Query = "select bns_nm,bns_house_no,bns_street,bns_mun,bns_dist,bns_zone,bns_brgy,bns_prov,tax_year,businesses.bns_code,bns_desc,bns_zip, orgn_kind from businesses,bns_table";   // RMC 20110803 added bns_zip
                    pSet.Query += string.Format(" where businesses.bns_code = bns_table.bns_code and fees_code = 'B' and bin = '{0}'", TransferAppFrm.BIN);
                }
            }
            if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    TransferAppFrm.txtOldLastName.Text = pSet.GetString("bns_nm");
                    TransferAppFrm.txtOldFirstName.Text = pSet.GetString("bns_desc");
                    TransferAppFrm.txtOldAdd.Text = pSet.GetString("bns_house_no");
                    TransferAppFrm.txtOldStreet.Text = pSet.GetString("bns_street");
                    TransferAppFrm.txtOldBrgy.Text = pSet.GetString("bns_brgy");
                    TransferAppFrm.txtOldZone.Text = pSet.GetString("bns_zone");
                    TransferAppFrm.txtOldDistrict.Text = pSet.GetString("bns_dist");
                    TransferAppFrm.txtOldCity.Text = pSet.GetString("bns_mun");
                    TransferAppFrm.txtOldProv.Text = pSet.GetString("bns_prov");
                    TransferAppFrm.TaxYear = pSet.GetString("tax_year");
                    TransferAppFrm.txtOldZip.Text = pSet.GetString("bns_zip");
                    TransferAppFrm.txtOldOrg.Text = pSet.GetString("orgn_kind");

                    m_strBnsCode = pSet.GetString("bns_code");

                    TransferAppFrm.txtNewLastName.Text = TransferAppFrm.txtOldLastName.Text;
                    TransferAppFrm.txtNewFirstName.Text = TransferAppFrm.txtOldFirstName.Text;
                    TransferAppFrm.txtNewAdd.Text = TransferAppFrm.txtOldAdd.Text;
                    TransferAppFrm.txtNewStreet.Text = TransferAppFrm.txtOldStreet.Text;
                    TransferAppFrm.txtNewBrgy.Text = TransferAppFrm.txtOldBrgy.Text;
                    TransferAppFrm.txtNewZone.Text = TransferAppFrm.txtOldZone.Text;
                    TransferAppFrm.txtNewDistrict.Text = TransferAppFrm.txtOldDistrict.Text;
                    TransferAppFrm.txtNewCity.Text = TransferAppFrm.txtOldCity.Text;
                    TransferAppFrm.txtNewProv.Text = TransferAppFrm.txtOldProv.Text;
                    TransferAppFrm.txtNewZip.Text = TransferAppFrm.txtOldZip.Text;
                    TransferAppFrm.cmbNewOrg.Text = TransferAppFrm.txtOldOrg.Text;


                }
                else
                {
                    MessageBox.Show("Record not found.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    TransferAppFrm.ClearPrevious();
                    TransferAppFrm.ClearNew();
                    return;
                }
                    
            }
            pSet.Close();

           
            pSet.Query = string.Format("select * from permit_update_appl where bin = '{0}' and appl_type = 'CORG' and data_mode = 'QUE'", TransferAppFrm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from transfer_table where bin = '{0}' and trans_app_code = 'CO'", TransferAppFrm.BIN);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            TransferAppFrm.cmbNewOrg.Text = pSet.GetString("own_ln");
                            TransferAppFrm.btnSave.Text = "Edit";
                            TransferAppFrm.cmbNewOrg.Enabled = false;

                            m_sInitialPUTValue = pSet.GetString("own_ln");
                        }
                    }
                    pSet.Close();
                }
            }

            

        }

        public override void Save()
        {
            if(TransferAppFrm.txtOldOrg.Text.ToString().Trim() == 
                TransferAppFrm.cmbNewOrg.Text.ToString().Trim())
            {
                MessageBox.Show("Organization kind not changed.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                TransferAppFrm.cmbNewOrg.Focus();
                return;
            }

            if (TransferAppFrm.cmbNewOrg.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Organization kind required.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                TransferAppFrm.txtNewLastName.Focus();
                return;
            }
            else
            {
                if (TransferAppFrm.btnSave.Text == "Save")
                {
                    if (MessageBox.Show("Save application for Change of Organization Kind?", m_strTransaction, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SaveApplication("Save");

                        MessageBox.Show("Application for change of organization kind saved successfully.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TransferAppFrm.cmbNewOrg.Enabled = false;
                        TransferAppFrm.dgvOtherInfo.Enabled = false;
                        TransferAppFrm.btnSave.Text = "Edit";
                        TransferAppFrm.NewOrgnKind = TransferAppFrm.cmbNewOrg.Text.ToString().Trim();
                        TransferAppFrm.ApplSave = true;

                        string sObj = TransferAppFrm.BIN + "/PUT: Orgn Kind to " + TransferAppFrm.cmbNewOrg.Text.ToString().Trim();
                        // RMC 20141125 mods in trailing permit-update transaction (s)
                        if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            TransferAppFrm.ApplyBilling = false;
                            if (AuditTrail.InsertTrail("AAAPUT-NB", "transfer_table", StringUtilities.HandleApostrophe(sObj)) == 0)
                            {
                                pSet.Rollback();
                                pSet.Close();
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }// RMC 20141125 mods in trailing permit-update transaction (e)
                        else
                        {
                            TransferAppFrm.ApplyBilling = true; // RMC 20141125 mods in trailing permit-update transaction
                            if (AuditTrail.InsertTrail("AAAPUT", "transfer_table", StringUtilities.HandleApostrophe(sObj)) == 0)
                            {
                                pSet.Rollback();
                                pSet.Close();
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        

                    }
                }
                else if (TransferAppFrm.btnSave.Text == "Edit")
                {
                    TransferAppFrm.cmbNewOrg.Enabled = true;
                    TransferAppFrm.btnSave.Text = "Update";
                    TransferAppFrm.btnClose.Text = "Cancel";
                }
                else if (TransferAppFrm.btnSave.Text == "Update")
                {
                    if (m_sInitialPUTValue == TransferAppFrm.cmbNewOrg.Text.ToString().Trim())
                    {
                        MessageBox.Show("Organization kind not changed.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        TransferAppFrm.cmbNewOrg.Focus();
                        return;
                    }

                    if (MessageBox.Show("Update application for Change of Organization kind?", m_strTransaction, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SaveApplication("Update");

                        MessageBox.Show("Application for change of organization kind updated.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TransferAppFrm.cmbNewOrg.Enabled = false;
                        TransferAppFrm.dgvOtherInfo.Enabled = false;
                        TransferAppFrm.btnSave.Text = "Edit";
                        TransferAppFrm.NewOrgnKind = TransferAppFrm.cmbNewOrg.Text.ToString().Trim();
                        TransferAppFrm.ApplSave = true;
                        TransferAppFrm.btnSave.Text = "Edit";
                        TransferAppFrm.btnClose.Text = "Close";

                        string sObj = TransferAppFrm.BIN + "/PUT: Orgn Kind fr " + m_sInitialPUTValue + " to " + TransferAppFrm.cmbNewOrg.Text.ToString().Trim();
                        // RMC 20141125 mods in trailing permit-update transaction (s)
                        if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            TransferAppFrm.ApplyBilling = false;
                            if (AuditTrail.InsertTrail("AAEPUT-NB", "transfer_table", StringUtilities.HandleApostrophe(sObj)) == 0)
                            {
                                pSet.Rollback();
                                pSet.Close();
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }// RMC 20141125 mods in trailing permit-update transaction (e)
                        else
                        {
                            TransferAppFrm.ApplyBilling = true; // RMC 20141125 mods in trailing permit-update transaction
                            if (AuditTrail.InsertTrail("AAEPUT", "transfer_table", StringUtilities.HandleApostrophe(sObj)) == 0)
                            {
                                pSet.Rollback();
                                pSet.Close();
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
            }
        }

        

        private void SaveApplication(string strTransaction)
        {
            if (strTransaction == "Save")
            {
                pSet.Query = "insert into transfer_table values (:1,'CO',' ',' ',' ',:2,' ',' ',' ',' ',' ',' ',' ',' ',' ',:3)";
                pSet.AddParameter(":1", TransferAppFrm.BIN);
                pSet.AddParameter(":2", StringUtilities.HandleApostrophe(TransferAppFrm.cmbNewOrg.Text.ToString().Trim()));
                pSet.AddParameter(":3", string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate()));
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }
            else
            {

                pSet.Query = "update transfer_table set ";
                pSet.Query += string.Format("own_ln = '{0}', ",StringUtilities.HandleApostrophe(TransferAppFrm.cmbNewOrg.Text.ToString().Trim()));
                pSet.Query += string.Format("app_date = '{0}'", string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate()));
                pSet.Query += string.Format(" where bin = '{0}' and trans_app_code = 'CO'", TransferAppFrm.BIN);
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }
        }

        
        public override void Close()
        {
            LoadValues();
            TransferAppFrm.cmbNewOrg.Enabled = true;
            TransferAppFrm.btnClose.Text = "Close";
            
        }

       
    }
}
