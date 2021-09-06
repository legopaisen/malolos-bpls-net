using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.SearchOwner;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.PermitUpdate
{
    public class AppNewOwnName : TransferApp
    {
        OracleResultSet pSet = new OracleResultSet();
        private string m_sInitialPUTValue = string.Empty;   // RMC 20111004 added trailing of Permit-udpate applications                 

        public AppNewOwnName(frmTransferApp Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            TransferAppFrm.lblOldTitle.Text = "Previous Owner";
            TransferAppFrm.lblOldLastName.Text = "Last Name";
            TransferAppFrm.lblOldFirstName.Text = "First Name";

            TransferAppFrm.lblNewTitle.Text = "New Owner";
            TransferAppFrm.lblNewLastName.Text = "Last Name";
            TransferAppFrm.lblNewFirstName.Text = "First Name";

            TransferAppFrm.lblOldMI.Visible = true;
            TransferAppFrm.txtOldMI.Visible = true;
            TransferAppFrm.lblNewMI.Visible = true;
            TransferAppFrm.txtNewMI.Visible = true;

            TransferAppFrm.ClearPrevious();
            TransferAppFrm.ClearNew();

            TransferAppFrm.bin1.txtTaxYear.ReadOnly = true;
            TransferAppFrm.bin1.txtBINSeries.ReadOnly = true;
            TransferAppFrm.bin2.txtTaxYear.ReadOnly = true;
            TransferAppFrm.bin2.txtBINSeries.ReadOnly = true;
            TransferAppFrm.bin2.txtTaxYear.Text = "";
            TransferAppFrm.bin2.txtBINSeries.Text = "";

            TransferAppFrm.btnNewSearch.Visible = true;

            // RMC 20111004 added permit-update change of orgn kind (s)
            TransferAppFrm.lblNewOrg.Visible = false;
            TransferAppFrm.cmbNewOrg.Visible = false;
            // RMC 20111004 added permit-update change of orgn kind (e)

            
            LoadValues();
            LoadOtherInfo();    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership 

            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (s)
            TransferAppFrm.dgvOtherInfo.Enabled = true;
            TransferAppFrm.dgvOtherInfo.ReadOnly = false;
            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (e)
            
        }

        private void EnableControls(bool blnEnable)
        {
            TransferAppFrm.txtNewLastName.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewFirstName.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewMI.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewAdd.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewStreet.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewBrgy.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewZone.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewDistrict.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewCity.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewProv.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewZip.ReadOnly = !blnEnable; // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership 
        }

        private void LoadValues()
        {
            m_sInitialPUTValue = "";

            pSet.Query = "select own_names.own_code,own_ln,own_fn,own_mi,own_house_no,own_street,own_brgy,own_dist,own_zone,own_mun,own_prov,tax_year,bns_code from own_names,business_que";
            pSet.Query += string.Format(" where own_names.own_code = business_que.own_code and bin = '{0}'", TransferAppFrm.BIN);
            pSet.Query += string.Format(" and tax_year = '{0}'", ConfigurationAttributes.CurrentYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = "select own_names.own_code,own_ln,own_fn,own_mi,own_house_no,own_street,own_brgy,own_dist,own_zone,own_mun,own_prov,tax_year,bns_code, own_zip from own_names,business_que";   // RMC 20110803 added own_zip
                    pSet.Query += string.Format(" where own_names.own_code = business_que.own_code and bin = '{0}'", TransferAppFrm.BIN);
                    pSet.Query += string.Format(" and tax_year = '{0}'", ConfigurationAttributes.CurrentYear);
                }
                else
                {
                    pSet.Close();

                    pSet.Query = "select own_names.own_code,own_ln,own_fn,own_mi,own_house_no,own_street,own_brgy,own_dist,own_zone,own_mun,own_prov,tax_year,bns_code, own_zip from own_names,businesses"; // RMC 20110803 added own_zip
                    pSet.Query += string.Format(" where own_names.own_code = businesses.own_code and bin = '{0}'", TransferAppFrm.BIN);
                }
            }

            if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_strOldOwnCode = pSet.GetString("own_code");
                    TransferAppFrm.txtOldLastName.Text = pSet.GetString("own_ln");
                    TransferAppFrm.txtOldFirstName.Text = pSet.GetString("own_fn");
                    TransferAppFrm.txtOldMI.Text = pSet.GetString("own_mi");
                    TransferAppFrm.txtOldAdd.Text = pSet.GetString("own_house_no");
                    TransferAppFrm.txtOldBrgy.Text = pSet.GetString("own_brgy");
                    TransferAppFrm.txtOldStreet.Text = pSet.GetString("own_street");
                    TransferAppFrm.txtOldDistrict.Text = pSet.GetString("own_dist");
                    TransferAppFrm.txtOldZone.Text = pSet.GetString("own_zone");
                    TransferAppFrm.txtOldCity.Text = pSet.GetString("own_mun");
                    TransferAppFrm.txtOldProv.Text = pSet.GetString("own_prov");
                    TransferAppFrm.TaxYear = pSet.GetString("tax_year");
                    TransferAppFrm.txtOldZip.Text = pSet.GetString("own_zip");
                    m_strBnsCode = pSet.GetString("bns_code");
                }
            }
            pSet.Close();

            pSet.Query = string.Format("select * from permit_update_appl where bin = '{0}' and appl_type = 'TOWN' and data_mode = 'QUE'", TransferAppFrm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = string.Format("select * from transfer_table where bin = '{0}' and trans_app_code = 'TO'", TransferAppFrm.BIN);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            TransferAppFrm.NewOwnCode = pSet.GetString("new_own_code");
                            TransferAppFrm.txtNewLastName.Text = pSet.GetString("own_ln");
                            TransferAppFrm.txtNewFirstName.Text = pSet.GetString("own_fn");
                            TransferAppFrm.txtNewMI.Text = pSet.GetString("own_mi");
                            TransferAppFrm.txtNewAdd.Text = pSet.GetString("addr_no");
                            TransferAppFrm.txtNewBrgy.Text = pSet.GetString("addr_brgy");
                            TransferAppFrm.txtNewStreet.Text = pSet.GetString("addr_street");
                            TransferAppFrm.txtNewDistrict.Text = pSet.GetString("addr_dist");
                            TransferAppFrm.txtNewZone.Text = pSet.GetString("addr_zone");
                            TransferAppFrm.txtNewCity.Text = pSet.GetString("addr_mun");
                            TransferAppFrm.txtNewProv.Text = pSet.GetString("addr_prov");
                            TransferAppFrm.btnSave.Text = "Edit";
                            TransferAppFrm.btnNewSearch.Text = "Clear";
                            TransferAppFrm.btnNewSearch.Enabled = false;
                            EnableControls(false);

                            m_sInitialPUTValue = pSet.GetString("new_own_code");    // RMC 20111004 added trailing of Permit-udpate applications 
                        }
                    }
                    pSet.Close();
                }
                else
                    EnableControls(true);
            }
        }

        public override void Save()
        {
            string chkBin = string.Empty;

            if (TransferAppFrm.btnSave.Text == "Edit")
            {
                EnableControls(true);
                TransferAppFrm.btnSave.Text = "Update";
                TransferAppFrm.btnClose.Text = "Cancel";
                TransferAppFrm.btnNewSearch.Enabled = true;
            }
            else
            {
                if (TransferAppFrm.txtOldLastName.Text.ToString().Trim() == TransferAppFrm.txtNewLastName.Text.ToString().Trim()
                    && TransferAppFrm.txtOldFirstName.Text.ToString().Trim() == TransferAppFrm.txtNewFirstName.Text.ToString().Trim()
                    && TransferAppFrm.txtOldMI.Text.ToString().Trim() == TransferAppFrm.txtNewMI.Text.ToString().Trim())
                {
                    MessageBox.Show("You are transferring the same owner. Please check the fields.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                pSet.Query = "select distinct bin from taxdues";
                pSet.Query += string.Format(" where bin = '{0}'", TransferAppFrm.BIN);
                pSet.Query += string.Format(" and tax_year <= '{0}'", string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate()));
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        chkBin = pSet.GetString("bin").Trim();
                    }
                }
                pSet.Close();

                if (chkBin != "")
                {
                    if (MessageBox.Show("Warning! Unsettled dues found.\n Continue saving?", m_strTransaction, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }

                if (TransferAppFrm.txtNewLastName.Text.ToString().Trim() == "" || TransferAppFrm.txtNewStreet.Text.ToString().Trim() == ""
                    || TransferAppFrm.txtNewBrgy.Text.ToString().Trim() == "" || TransferAppFrm.txtNewCity.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Fill-up all necessary fields", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TransferAppFrm.txtNewLastName.Focus();
                }
                else
                {
                    //TransferAppFrm.NewOwnCode = AppSettingsManager.EnlistOwner(TransferAppFrm.txtNewLastName.Text.Trim(), TransferAppFrm.txtNewFirstName.Text.Trim(), TransferAppFrm.txtNewMI.Text.Trim(), TransferAppFrm.txtNewAdd.Text.Trim(), TransferAppFrm.txtNewStreet.Text.Trim(), TransferAppFrm.txtNewDistrict.Text.Trim(), TransferAppFrm.txtNewZone.Text.Trim(), TransferAppFrm.txtNewBrgy.Text.Trim(), TransferAppFrm.txtNewCity.Text.Trim(), TransferAppFrm.txtNewProv.Text.Trim(), "");
                    TransferAppFrm.NewOwnCode = AppSettingsManager.EnlistOwner(TransferAppFrm.txtNewLastName.Text.Trim(), TransferAppFrm.txtNewFirstName.Text.Trim(), TransferAppFrm.txtNewMI.Text.Trim(), TransferAppFrm.txtNewAdd.Text.Trim(), TransferAppFrm.txtNewStreet.Text.Trim(), TransferAppFrm.txtNewDistrict.Text.Trim(), TransferAppFrm.txtNewZone.Text.Trim(), TransferAppFrm.txtNewBrgy.Text.Trim(), TransferAppFrm.txtNewCity.Text.Trim(), TransferAppFrm.txtNewProv.Text.Trim(), TransferAppFrm.txtNewZip.Text.Trim()); // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership 

                    if (TransferAppFrm.btnSave.Text == "Save")
                    {
                        if (MessageBox.Show("Save application for Transfer of Ownership?", m_strTransaction, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            SaveOtherInfo();    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership 
                            SaveApplication("Save");

                            MessageBox.Show("Application for Transfer of Ownership saved successfully!", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            EnableControls(false);
                            TransferAppFrm.ApplSave = true;
                            TransferAppFrm.btnSave.Text = "Edit";

                            // RMC 20111004 added trailing of Permit-udpate applications (s)
                            string sObj = TransferAppFrm.BIN + "/PUT: Own Code to " + TransferAppFrm.NewOwnCode;
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
                            // RMC 20111004 added trailing of Permit-udpate applications (e)
                        }
                    }

                    else if (TransferAppFrm.btnSave.Text == "Update")
                    {
                        if (MessageBox.Show("Update application for Transfer of Ownership?", m_strTransaction, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            SaveOtherInfo();    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership 
                            SaveApplication("Update");

                            MessageBox.Show("Application for Transfer of Ownership updated!", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            EnableControls(false);

                            TransferAppFrm.ApplSave = true;
                            TransferAppFrm.btnSave.Text = "Edit";
                            TransferAppFrm.btnClose.Text = "Close";

                            // RMC 20111004 added trailing of Permit-udpate applications (s)
                            string sObj = TransferAppFrm.BIN + "/PUT: Own Code fr " + m_sInitialPUTValue + " to " + TransferAppFrm.NewOwnCode;
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
                            // RMC 20111004 added trailing of Permit-udpate applications (e)
                        }
                    }
                }
            }
        }

        private void SaveApplication(string strTransaction)
        {
            if (strTransaction == "Save")
            {
                pSet.Query = "insert into transfer_table values (:1,'TO',' ',:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14)";
                pSet.AddParameter(":1", TransferAppFrm.BIN);
                pSet.AddParameter(":2", m_strOldOwnCode);
                pSet.AddParameter(":3", TransferAppFrm.NewOwnCode);
                pSet.AddParameter(":4", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewLastName.Text.ToString().Trim())));
                pSet.AddParameter(":5", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewFirstName.Text.ToString().Trim())));
                pSet.AddParameter(":6", StringUtilities.SetEmptyToSpace(TransferAppFrm.txtNewMI.Text.ToString().Trim()));
                pSet.AddParameter(":7", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewAdd.Text.ToString().Trim())));
                pSet.AddParameter(":8", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewStreet.Text.ToString().Trim())));
                pSet.AddParameter(":9", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewBrgy.Text.ToString().Trim())));
                pSet.AddParameter(":10", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewZone.Text.ToString().Trim())));
                pSet.AddParameter(":11", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewDistrict.Text.ToString().Trim())));
                pSet.AddParameter(":12", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewCity.Text.ToString().Trim())));
                pSet.AddParameter(":13", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewProv.Text.ToString().Trim())));
                pSet.AddParameter(":14", string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate()));
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }
            else
            {
                pSet.Query = "update transfer_table set new_own_code = :1, own_ln = :2, ";
                pSet.Query += "own_fn = :3, own_mi = :4, addr_no = :5, addr_street = :6, ";
                pSet.Query += "addr_brgy = :7, addr_zone = :8, addr_dist = :9, addr_mun = :10, ";
                pSet.Query += "addr_prov = :11, app_date = :12 ";
                pSet.Query += string.Format("where bin = '{0}' and trans_app_code = 'TO'", TransferAppFrm.BIN);
                pSet.AddParameter(":1", TransferAppFrm.NewOwnCode);
                pSet.AddParameter(":2", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewLastName.Text.ToString().Trim())));
                pSet.AddParameter(":3", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewFirstName.Text.ToString().Trim())));
                pSet.AddParameter(":4", StringUtilities.SetEmptyToSpace(TransferAppFrm.txtNewMI.Text.ToString().Trim()));
                pSet.AddParameter(":5", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewAdd.Text.ToString().Trim())));
                pSet.AddParameter(":6", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewStreet.Text.ToString().Trim())));
                pSet.AddParameter(":7", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewBrgy.Text.ToString().Trim())));
                pSet.AddParameter(":8", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewZone.Text.ToString().Trim())));
                pSet.AddParameter(":9", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewDistrict.Text.ToString().Trim())));
                pSet.AddParameter(":10", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewCity.Text.ToString().Trim())));
                pSet.AddParameter(":11", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewProv.Text.ToString().Trim())));
                pSet.AddParameter(":12", string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate()));
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }
        }

        public override void SearchNew()
        {
            if (TransferAppFrm.btnNewSearch.Text == "Clear")
            {
                TransferAppFrm.ClearNew();
                TransferAppFrm.btnNewSearch.Text = "Search";
                EnableControls(true);
            }
            else
            {
                frmSearchOwner SearchOwner = new frmSearchOwner();
                SearchOwner.ShowDialog();
                TransferAppFrm.txtNewLastName.Text = SearchOwner.m_sOwnLn;
                TransferAppFrm.txtNewFirstName.Text = SearchOwner.m_sOwnFn;
                TransferAppFrm.txtNewMI.Text = SearchOwner.m_sOwnMi;
                TransferAppFrm.txtNewAdd.Text = SearchOwner.m_sOwnAdd;
                TransferAppFrm.txtNewStreet.Text = SearchOwner.m_sOwnStreet;
                TransferAppFrm.txtNewBrgy.Text = SearchOwner.m_sOwnBrgy;
                TransferAppFrm.txtNewDistrict.Text = SearchOwner.m_sOwnDist;
                TransferAppFrm.txtNewCity.Text = SearchOwner.m_sOwnMun;
                TransferAppFrm.txtNewProv.Text = SearchOwner.m_sOwnProv;
                TransferAppFrm.btnNewSearch.Text = "Clear";
                EnableControls(false);
            }
        }

        public override void Close()
        {
            LoadValues();
            LoadOtherInfo();    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership 
            TransferAppFrm.btnClose.Text = "Close";
            EnableControls(false);
        }

        private void LoadOtherInfo()
        {
            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership

            OracleResultSet pRec = new OracleResultSet();
            string strBnsCode = string.Empty;
            string sDefaultCode = string.Empty;
            string sDataType = string.Empty;

            TransferAppFrm.dgvOtherInfo.Columns.Clear();
            TransferAppFrm.dgvOtherInfo.Columns.Add("CODE", "Code");
            TransferAppFrm.dgvOtherInfo.Columns.Add("DESC", "Description");
            TransferAppFrm.dgvOtherInfo.Columns.Add("TYPE", "");
            TransferAppFrm.dgvOtherInfo.Columns.Add("VALUE", "Value");
            TransferAppFrm.dgvOtherInfo.RowHeadersVisible = false;
            TransferAppFrm.dgvOtherInfo.Columns[0].Width = 80;
            TransferAppFrm.dgvOtherInfo.Columns[1].Width = 160;
            TransferAppFrm.dgvOtherInfo.Columns[2].Width = 0;
            TransferAppFrm.dgvOtherInfo.Columns[3].Width = 200;
            TransferAppFrm.dgvOtherInfo.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            TransferAppFrm.dgvOtherInfo.Columns[2].Visible = false;

            TransferAppFrm.dgvOtherInfo.Rows.Add("");
            TransferAppFrm.dgvOtherInfo[0, 0].Value = "DTINO";
            TransferAppFrm.dgvOtherInfo[1, 0].Value = "DTI NO.";
            TransferAppFrm.dgvOtherInfo.Rows.Add("");
            TransferAppFrm.dgvOtherInfo[0, 1].Value = "DTIDT";
            TransferAppFrm.dgvOtherInfo[1, 1].Value = "DTI DATE (MM/DD/YYYY)";
            TransferAppFrm.dgvOtherInfo.Rows.Add("");
            TransferAppFrm.dgvOtherInfo[0, 2].Value = "MEMO";
            TransferAppFrm.dgvOtherInfo[1, 2].Value = "MEMORANDA";

            

            pRec.Query = string.Format("select * from TRANS_OTHER_INFO_ADDL where bin = '{0}' and tax_year = '{1}' and (default_code = 'DTINO' or default_code = 'DTIDT' or default_code = 'MEMO')", TransferAppFrm.BIN, ConfigurationAttributes.CurrentYear);
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    if (pRec.GetString("default_code") == "DTINO")
                        TransferAppFrm.dgvOtherInfo[3, 0].Value = pRec.GetString("data");
                    if (pRec.GetString("default_code") == "DTIDT")
                        TransferAppFrm.dgvOtherInfo[3, 1].Value = pRec.GetString("data");
                    if (pRec.GetString("default_code") == "MEMO")
                        TransferAppFrm.dgvOtherInfo[3, 2].Value = pRec.GetString("data");
                }
            }
            pRec.Close();
        }

        private void SaveOtherInfo()
        {
            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership

            string strBnsCode = string.Empty;
            string sCode = string.Empty;
            string sType = string.Empty;
            string sValue = string.Empty;
            double dValue = 0;

            strBnsCode = StringUtilities.Left(m_strBnsCode, 2);

            pSet.Query = string.Format("delete from TRANS_OTHER_INFO_ADDL where bin = '{0}' and tax_year = '{1}' and (default_code = 'DTINO' or default_code = 'DTIDT' or default_code = 'MEMO')", TransferAppFrm.BIN, ConfigurationAttributes.CurrentYear);
            if (pSet.ExecuteNonQuery() == 0)
            { }

            for (int iRow = 0; iRow < TransferAppFrm.dgvOtherInfo.Rows.Count; iRow++)
            {
                try
                {
                    sCode = TransferAppFrm.dgvOtherInfo[0, iRow].Value.ToString().Trim();
                    sValue = TransferAppFrm.dgvOtherInfo[3, iRow].Value.ToString().Trim();

                    if (sValue != "")
                    {
                        pSet.Query = "insert into TRANS_OTHER_INFO_ADDL values(:1,:2,:3,:4,:5)";
                        pSet.AddParameter(":1", TransferAppFrm.BIN);
                        pSet.AddParameter(":2", ConfigurationAttributes.CurrentYear);
                        pSet.AddParameter(":3", sCode);
                        pSet.AddParameter(":4", StringUtilities.HandleApostrophe(sValue));
                        pSet.AddParameter(":5", ConfigurationAttributes.RevYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                    }
                }
                catch { }   // RMC 20171227 correction in saving permit-update application if row has null value
            }

            TransferAppFrm.dgvOtherInfo.Enabled = false;

        }
    }
}
