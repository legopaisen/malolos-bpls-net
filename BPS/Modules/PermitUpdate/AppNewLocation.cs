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
    public class AppNewLocation : TransferApp
    {
        OracleResultSet pSet = new OracleResultSet();
        private string m_sInitialPUTValue = string.Empty;   // RMC 20111004 added trailing of Permit-udpate applications                 
        
        public AppNewLocation(frmTransferApp Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            TransferAppFrm.lblOldTitle.Text = "Previous Location";
            TransferAppFrm.lblOldLastName.Text = "Bns Name";
            TransferAppFrm.lblOldFirstName.Text = "Bns Type";

            TransferAppFrm.lblNewTitle.Text = "New Location";
            TransferAppFrm.lblNewLastName.Text = "Bns Name";
            TransferAppFrm.lblNewFirstName.Text = "Bns Type";

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

            TransferAppFrm.cmbNewBrgy.Visible = true;
            TransferAppFrm.cmbNewDist.Visible = true;
            TransferAppFrm.txtNewBrgy.Visible = false;
            TransferAppFrm.txtNewDistrict.Visible = false;

            // RMC 20111004 added permit-update change of orgn kind (s)
            TransferAppFrm.lblNewOrg.Visible = false;
            TransferAppFrm.cmbNewOrg.Visible = false;
            // RMC 20111004 added permit-update change of orgn kind (e)

            LoadValues();
        }

        private void LoadValues()
        {
            TransferAppFrm.txtNewCity.Text = AppSettingsManager.GetConfigValue("02");
            m_sInitialPUTValue = "";    // RMC 20111004 added trailing of Permit-udpate applications 

            pSet.Query = "select distinct brgy_nm from brgy order by brgy_nm";
            if (pSet.Execute())
            {
                TransferAppFrm.cmbNewBrgy.Items.Add(" ");

                while (pSet.Read())
                {
                    TransferAppFrm.cmbNewBrgy.Items.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();

            pSet.Query = "select distinct dist_nm from brgy order by dist_nm";
            if (pSet.Execute())
            {
                TransferAppFrm.cmbNewDist.Items.Add(" ");

                while (pSet.Read())
                {
                    TransferAppFrm.cmbNewDist.Items.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();

            pSet.Query = "select bns_nm,bns_house_no,bns_street,bns_mun,bns_dist,bns_zone,bns_brgy,bns_prov,tax_year,business_que.bns_code,bns_desc from business_que,bns_table";
            pSet.Query += string.Format(" where business_que.bns_code = bns_table.bns_code and fees_code = 'B' and bin = '{0}'", TransferAppFrm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = "select bns_nm,bns_house_no,bns_street,bns_mun,bns_dist,bns_zone,bns_brgy,bns_prov,tax_year,business_que.bns_code,bns_desc, bns_zip from business_que,bns_table";  // RMC 20110803 added bns_zip
                    pSet.Query += string.Format(" where business_que.bns_code = bns_table.bns_code and fees_code = 'B' and bin = '{0}'", TransferAppFrm.BIN);
                }
                else
                {
                    pSet.Close();

                    pSet.Query = "select bns_nm,bns_house_no,bns_street,bns_mun,bns_dist,bns_zone,bns_brgy,bns_prov,tax_year,businesses.bns_code,bns_desc, bns_zip from businesses,bns_table";  // RMC 20110803 added bns_zip
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
                    TransferAppFrm.txtOldBrgy.Text = pSet.GetString("bns_brgy");
                    TransferAppFrm.txtOldStreet.Text = pSet.GetString("bns_street");
                    TransferAppFrm.txtOldDistrict.Text = pSet.GetString("bns_dist");
                    TransferAppFrm.txtOldZone.Text = pSet.GetString("bns_zone");
                    TransferAppFrm.txtOldCity.Text = pSet.GetString("bns_mun");
                    TransferAppFrm.txtOldProv.Text = pSet.GetString("bns_prov");
                    TransferAppFrm.TaxYear = pSet.GetString("tax_year");
                    TransferAppFrm.txtOldZip.Text = pSet.GetString("bns_zip");

                    m_strBnsCode = pSet.GetString("bns_code");

                    TransferAppFrm.txtNewLastName.Text = pSet.GetString("bns_nm");
                    TransferAppFrm.txtNewFirstName.Text = pSet.GetString("bns_desc");
                }
            }
            pSet.Close();

            pSet.Query = string.Format("select * from permit_update_appl where bin = '{0}' and appl_type = 'TLOC' and data_mode = 'QUE'", TransferAppFrm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = string.Format("select * from transfer_table where bin = '{0}' and trans_app_code = 'TL'", TransferAppFrm.BIN);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            TransferAppFrm.txtNewAdd.Text = pSet.GetString("addr_no");
                            TransferAppFrm.txtNewBrgy.Text = pSet.GetString("addr_brgy");
                            TransferAppFrm.txtNewStreet.Text = pSet.GetString("addr_street");
                            TransferAppFrm.txtNewDistrict.Text = pSet.GetString("addr_dist");
                            TransferAppFrm.txtNewZone.Text = pSet.GetString("addr_zone");
                            TransferAppFrm.txtNewCity.Text = pSet.GetString("addr_mun");
                            TransferAppFrm.txtNewProv.Text = pSet.GetString("addr_prov");

                            TransferAppFrm.NewLocation = GetNewLocation();

                            m_sInitialPUTValue = TransferAppFrm.NewLocation;    // RMC 20111004 added trailing of Permit-udpate applications 

                            TransferAppFrm.btnSave.Text = "Edit";
                            EnableControls(false);
                        }
                    }
                    pSet.Close();
                }
                else
                    EnableControls(true);
            }
        }

        private void EnableControls(bool blnEnable)
        {
            TransferAppFrm.txtNewAdd.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewStreet.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewBrgy.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewZone.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewDistrict.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewCity.ReadOnly = !blnEnable;
            TransferAppFrm.txtNewProv.ReadOnly = !blnEnable;
        }

        public override void Close()
        {
            LoadValues();
            TransferAppFrm.btnClose.Text = "Close";
            EnableControls(false);
        }

        public override void Save()
        {
            if (TransferAppFrm.btnSave.Text == "Edit")
            {
                EnableControls(true);
                TransferAppFrm.btnSave.Text = "Update";
                TransferAppFrm.btnClose.Text = "Cancel";
                TransferAppFrm.btnNewSearch.Enabled = true;
            }
            else
            {
                if (TransferAppFrm.txtOldAdd.Text.Trim() == TransferAppFrm.txtNewAdd.Text.Trim()
                    && TransferAppFrm.txtOldStreet.Text.Trim() == TransferAppFrm.txtNewStreet.Text.Trim()
                    && TransferAppFrm.txtOldBrgy.Text.Trim() == TransferAppFrm.txtNewBrgy.Text.Trim()
                    && TransferAppFrm.txtOldZone.Text.Trim() == TransferAppFrm.txtNewZone.Text.Trim()
                    && TransferAppFrm.txtOldDistrict.Text.Trim() == TransferAppFrm.txtNewDistrict.Text.Trim()
                    && TransferAppFrm.txtOldCity.Text.Trim() == TransferAppFrm.txtNewCity.Text.Trim()
                    && TransferAppFrm.txtOldProv.Text.Trim() == TransferAppFrm.txtNewProv.Text.Trim())
                {
                    MessageBox.Show("You are transferring the same address. Please check the fields.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if((AppSettingsManager.GetConfigValue("24") == "Y" && TransferAppFrm.txtNewZone.Text.Trim() == "")
                    || (AppSettingsManager.GetConfigValue("23") == "Y" && TransferAppFrm.cmbNewDist.Text.Trim() == "")
                    || TransferAppFrm.cmbNewBrgy.Text.Trim() == "" || TransferAppFrm.txtNewCity.Text.Trim() == "")
                {
                    MessageBox.Show("Fill-up all necessary fields", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TransferAppFrm.txtNewAdd.Focus();
                }
                else
                {
                    if (TransferAppFrm.btnSave.Text == "Save")
                    {
                        if (MessageBox.Show("Save application for Transfer of Business Location?", m_strTransaction, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            SaveApplication("Save");
                            
                            MessageBox.Show("Application for Transfer of Business Location saved successfully.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            EnableControls(false);
                            TransferAppFrm.ApplSave = true;
                            TransferAppFrm.btnClose.Text = "Close";
                            TransferAppFrm.btnSave.Text = "Edit";
                            TransferAppFrm.NewLocation = GetNewLocation();

                            // RMC 20111004 added trailing of Permit-udpate applications (s)
                            string sObj = TransferAppFrm.BIN + "/PUT: Buss Location to " + TransferAppFrm.NewLocation;
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
                        if (MessageBox.Show("Update application for Transfer of Location?", m_strTransaction, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            SaveApplication("Update");
                            
                            MessageBox.Show("Application for Transfer of Business Location updated.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            EnableControls(false);
                            TransferAppFrm.ApplSave = true;
                            TransferAppFrm.btnClose.Text = "Close";
                            TransferAppFrm.btnSave.Text = "Edit";
                            TransferAppFrm.NewLocation = GetNewLocation();

                            // RMC 20111004 added trailing of Permit-udpate applications (s)
                            string sObj = TransferAppFrm.BIN + "/PUT: Buss Location fr " + m_sInitialPUTValue + " to " + TransferAppFrm.NewLocation;
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

        private string GetNewLocation()
        {
            string sHouseNo, sStreet, sMun, sBrgy, sZone, sDistrict, sProv, sBnsAddress;
            string sNewLocation = string.Empty;

            if (TransferAppFrm.txtNewAdd.Text.Trim() == "." || TransferAppFrm.txtNewAdd.Text.Trim() == "")
                sHouseNo = "";
            else
                sHouseNo = TransferAppFrm.txtNewAdd.Text.Trim() + " ";
            if (TransferAppFrm.txtNewStreet.Text.Trim() == "." || TransferAppFrm.txtNewStreet.Text.Trim() == "")
                sStreet = "";
            else
                sStreet = TransferAppFrm.txtNewStreet.Text.Trim() + ", ";
            if (TransferAppFrm.txtNewBrgy.Text.Trim() == "." || TransferAppFrm.txtNewBrgy.Text.Trim() == "")
                sBrgy = "";
            else
                sBrgy = TransferAppFrm.txtNewBrgy.Text.Trim() + ", ";
            if (TransferAppFrm.txtNewProv.Text.Trim() == "." || TransferAppFrm.txtNewProv.Text.Trim() == "")
                sProv = "";
            else
                sProv = TransferAppFrm.txtNewProv.Text.Trim();
            if (TransferAppFrm.txtNewZone.Text.Trim() == "." || TransferAppFrm.txtNewZone.Text.Trim() == "ZONE" || TransferAppFrm.txtNewZone.Text.Trim() == "")
                sZone = "";
            else
                sZone = "ZONE " + TransferAppFrm.txtNewZone.Text.Trim() + " ";
            if (TransferAppFrm.txtNewDistrict.Text.Trim() == "." || TransferAppFrm.txtNewDistrict.Text.Trim() == "")
                sDistrict = "";
            else
                sDistrict = TransferAppFrm.txtNewDistrict.Text.Trim() + ", ";
            if (TransferAppFrm.txtNewCity.Text.Trim() == "." || TransferAppFrm.txtNewCity.Text.Trim() == "")
                sMun = "";
            else
                sMun = TransferAppFrm.txtNewCity.Text.Trim();

            sNewLocation = sHouseNo + sStreet + sBrgy + sZone + sDistrict + sMun;

            return sNewLocation;
        }

        private void SaveApplication(string strTransaction)
        {
            if (strTransaction == "Save")
            {
                pSet.Query = "insert into transfer_table values (:1,'TL',' ',' ',' ',' ',' ',' ',:2,:3,:4,:5,:6,:7,:8,:9)";
                pSet.AddParameter(":1", TransferAppFrm.BIN);
                pSet.AddParameter(":2", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewAdd.Text.Trim())));
                pSet.AddParameter(":3", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewStreet.Text.Trim())));
                pSet.AddParameter(":4", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.cmbNewBrgy.Text.Trim())));
                pSet.AddParameter(":5", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewZone.Text.Trim())));
                pSet.AddParameter(":6", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.cmbNewDist.Text.Trim())));
                pSet.AddParameter(":7", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewCity.Text.Trim())));
                pSet.AddParameter(":8", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewProv.Text.Trim())));
                pSet.AddParameter(":9", string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate()));
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }
            else
            {
                pSet.Query = "update transfer_table set ";
                pSet.Query += "addr_no = :1, addr_street = :2, addr_brgy = :3, addr_zone = :4, ";
                pSet.Query += "addr_dist = :5, addr_mun = :6, addr_prov = :7, app_date = :8";
                pSet.Query += string.Format(" where bin = '{0}' and trans_app_code = 'TL'", TransferAppFrm.BIN);
                pSet.AddParameter(":1", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewAdd.Text.Trim())));
                pSet.AddParameter(":2", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewStreet.Text.Trim())));
                pSet.AddParameter(":3", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.cmbNewBrgy.Text.Trim())));
                pSet.AddParameter(":4", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewZone.Text.Trim())));
                pSet.AddParameter(":5", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.cmbNewDist.Text.Trim())));
                pSet.AddParameter(":6", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewCity.Text.Trim())));
                pSet.AddParameter(":7", StringUtilities.SetEmptyToSpace(StringUtilities.HandleApostrophe(TransferAppFrm.txtNewProv.Text.Trim())));
                pSet.AddParameter(":8", string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate()));
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }
        }
    }
}
