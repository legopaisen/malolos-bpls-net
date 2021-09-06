using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.TransactionLog;

namespace Amellar.Modules.PermitUpdate
{
    public class AppNewBnsName:TransferApp
    {
        OracleResultSet pSet = new OracleResultSet();
        private string m_sInitialPUTValue = string.Empty;

        public AppNewBnsName(frmTransferApp Form): base(Form)
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
            // RMC 20111004 added permit-update change of orgn kind (s)
            TransferAppFrm.lblNewOrg.Visible = false;
            TransferAppFrm.cmbNewOrg.Visible = false;
            // RMC 20111004 added permit-update change of orgn kind (e)
            LoadValues();
            LoadOtherInfo();
        }

        private void LoadValues()
        {
            m_sInitialPUTValue = "";    // RMC 20111004 added trailing of Permit-udpate applications 

            pSet.Query = "select bns_nm,bns_house_no,bns_street,bns_mun,bns_dist,bns_zone,bns_brgy,bns_prov,tax_year,business_que.bns_code,bns_desc from business_que,bns_table";
            pSet.Query += string.Format(" where business_que.bns_code = bns_table.bns_code and fees_code = 'B' and bin = '{0}'", TransferAppFrm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = "select bns_nm,bns_house_no,bns_street,bns_mun,bns_dist,bns_zone,bns_brgy,bns_prov,tax_year,business_que.bns_code,bns_desc,bns_zip from business_que,bns_table";   // RMC 20110803 added bns_zip
                    pSet.Query += string.Format(" where business_que.bns_code = bns_table.bns_code and fees_code = 'B' and bin = '{0}'", TransferAppFrm.BIN);   
                }
                else
                {
                    pSet.Close();

                    pSet.Query = "select bns_nm,bns_house_no,bns_street,bns_mun,bns_dist,bns_zone,bns_brgy,bns_prov,tax_year,businesses.bns_code,bns_desc,bns_zip from businesses,bns_table";   // RMC 20110803 added bns_zip
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

            TransferAppFrm.txtNewLastName.ReadOnly = false;
            TransferAppFrm.dgvOtherInfo.Enabled = true;

            pSet.Query = string.Format("select * from permit_update_appl where bin = '{0}' and appl_type = 'CBNS' and data_mode = 'QUE'", TransferAppFrm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from transfer_table where bin = '{0}' and trans_app_code = 'CB'", TransferAppFrm.BIN);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            TransferAppFrm.txtNewLastName.Text = pSet.GetString("own_ln");
                            TransferAppFrm.btnSave.Text = "Edit";
                            TransferAppFrm.txtNewLastName.ReadOnly = true;
                            m_sInitialPUTValue = TransferAppFrm.txtNewLastName.Text;    // RMC 20111004 added trailing of Permit-udpate applications
//                            TransferAppFrm.dgvOtherInfo.Enabled = false;
                        }
                    }
                    pSet.Close();
                }
            }

            

        }

        public override void Save()
        {
            if (TransferAppFrm.txtOldLastName.Text.ToString().Trim() ==
                TransferAppFrm.txtNewLastName.Text.ToString().Trim())
            {
                MessageBox.Show("Business name not changed.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                TransferAppFrm.txtNewLastName.Focus();
                return;
            }

            if(TransferAppFrm.txtNewLastName.Text.ToString().Trim() == "")
            {
                MessageBox.Show("New business name required.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                TransferAppFrm.txtNewLastName.Focus();
                return;
            }
            else
            {
                if (TransferAppFrm.btnSave.Text == "Save")
                {
                    if (MessageBox.Show("Save application for Change of Business Name?", m_strTransaction, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SaveOtherInfo();
                        SaveApplication("Save");

                        MessageBox.Show("Application for change of business name saved successfully.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TransferAppFrm.txtNewLastName.ReadOnly = true;
                        TransferAppFrm.dgvOtherInfo.Enabled = false;
                        TransferAppFrm.btnSave.Text = "Edit";
                        TransferAppFrm.NewBnsName = TransferAppFrm.txtNewLastName.Text.ToString().Trim();
                        TransferAppFrm.ApplSave = true;

                        // RMC 20111004 added trailing of Permit-udpate applications (s)
                        string sObj = TransferAppFrm.BIN + "/PUT: Buss Name to " + TransferAppFrm.txtNewLastName.Text.ToString().Trim();
                        // RMC 20141125 mods in trailing permit-update transaction (s)
                        if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            TransferAppFrm.ApplyBilling = false;

                            TransLog.UpdateLog(TransferAppFrm.BIN, TransferAppFrm.BnsStat, TransferAppFrm.TaxYear, "AAAPUT-NB", TransferAppFrm.LogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

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
                else if (TransferAppFrm.btnSave.Text == "Edit")
                {
                    TransferAppFrm.txtNewLastName.ReadOnly = false;
                    TransferAppFrm.dgvOtherInfo.Enabled = true;
                    TransferAppFrm.btnSave.Text = "Update";
                    TransferAppFrm.btnClose.Text = "Cancel";
                }
                else if (TransferAppFrm.btnSave.Text == "Update")
                {
                    if (MessageBox.Show("Update application for Change of Business Name?", m_strTransaction, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SaveOtherInfo();
                        SaveApplication("Update");

                        MessageBox.Show("Application for change of business name updated.", m_strTransaction, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TransferAppFrm.txtNewLastName.ReadOnly = true;
                        TransferAppFrm.dgvOtherInfo.Enabled = false;
                        TransferAppFrm.btnSave.Text = "Edit";
                        TransferAppFrm.NewBnsName = TransferAppFrm.txtNewLastName.Text.ToString().Trim();
                        TransferAppFrm.ApplSave = true;
                        TransferAppFrm.btnSave.Text = "Edit";
                        TransferAppFrm.btnClose.Text = "Close";

                        // RMC 20111004 added trailing of Permit-udpate applications (s)
                        string sObj = TransferAppFrm.BIN + "/PUT: Buss Name fr " + m_sInitialPUTValue + " to " + TransferAppFrm.txtNewLastName.Text.ToString().Trim();
                        // RMC 20141125 mods in trailing permit-update transaction (s)
                        if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            TransferAppFrm.ApplyBilling = false;
                            TransLog.UpdateLog(TransferAppFrm.BIN, TransferAppFrm.BnsStat, TransferAppFrm.TaxYear, "AAEPUT-NB", TransferAppFrm.LogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

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
                            TransLog.UpdateLog(TransferAppFrm.BIN, TransferAppFrm.BnsStat, TransferAppFrm.TaxYear, "AAEPUT", TransferAppFrm.LogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

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

        private void LoadOtherInfo()
        {
            OracleResultSet pRec = new OracleResultSet();
            string strBnsCode = string.Empty;
            string sDefaultCode = string.Empty;
            string sDataType = string.Empty;
            
            TransferAppFrm.dgvOtherInfo.Columns.Clear();
            TransferAppFrm.dgvOtherInfo.Columns.Add("CODE", "Code");
            TransferAppFrm.dgvOtherInfo.Columns.Add("DESC", "Description");
            TransferAppFrm.dgvOtherInfo.Columns.Add("TYPE", "Type");
            TransferAppFrm.dgvOtherInfo.Columns.Add("VALUE", "Value");
            TransferAppFrm.dgvOtherInfo.RowHeadersVisible = false;
            TransferAppFrm.dgvOtherInfo.Columns[0].Width = 80;
            TransferAppFrm.dgvOtherInfo.Columns[1].Width = 200;
            TransferAppFrm.dgvOtherInfo.Columns[2].Width = 80;
            TransferAppFrm.dgvOtherInfo.Columns[3].Width = 80;
            TransferAppFrm.dgvOtherInfo.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight; 

            strBnsCode = StringUtilities.Left(m_strBnsCode,2);

            int x = 0;  // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update change of business name
            pSet.Query = string.Format("select distinct(default_code) from default_others where rev_year = '{0}' order by default_code",ConfigurationAttributes.RevYear);
            if(pSet.Execute())        	
	        {
                //int x = 0;// RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update change of business name, put rem
                double d;

                while(pSet.Read())
	            {
		            TransferAppFrm.dgvOtherInfo.Rows.Add("");
			        sDefaultCode = pSet.GetString("default_code");

                    TransferAppFrm.dgvOtherInfo[0,x].Value = sDefaultCode;
			        
                    pRec.Query = string.Format("select * from default_code where default_code = '{0}' and rev_year = '{1}'",sDefaultCode,ConfigurationAttributes.RevYear);
			        if(pRec.Execute())
                    {
                        if(pRec.Read())
                        {
                            TransferAppFrm.dgvOtherInfo[1,x].Value = pRec.GetString("default_desc").Trim();
				            TransferAppFrm.dgvOtherInfo[2,x].Value = pRec.GetString("data_type").Trim();
			            }
                    }
                    pRec.Close();
			
        			sDataType = TransferAppFrm.dgvOtherInfo[2,x].Value.ToString().Trim();

                    pRec.Query = string.Format("select data,data_type from trans_other_info where bin = '{0}' and tax_year = '{1}' and bns_code = '{2}'", TransferAppFrm.BIN, ConfigurationAttributes.CurrentYear, m_strBnsCode);
                    pRec.Query+= string.Format(" and default_code = '{0}' and data_type = '{1}' and rev_year = '{2}'", sDefaultCode,sDataType,ConfigurationAttributes.RevYear);
                    if(pRec.Execute())
                    {
                        if(pRec.Read())
			    		{
							sDataType = pRec.GetString("data_type");	
                            TransferAppFrm.dgvOtherInfo[3,x].Value = pRec.GetDouble("data");
                        }
			            else
			            {
                            pRec.Close();
				            
            				pRec.Query = string.Format("select data,data_type, tax_year from trans_other_info where bin = '{0}'", TransferAppFrm.BIN); 
                            pRec.Query+= string.Format(" and default_code = '{0}' and data_type = '{1}' and rev_year = '{2}' order by tax_year desc", sDefaultCode,sDataType,ConfigurationAttributes.RevYear);
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                {
                                    sDataType = pRec.GetString("data_type");
                                    TransferAppFrm.dgvOtherInfo[3, x].Value = pRec.GetDouble("data");
                                }
                                else
                                {
                                    pRec.Close();

                                    pRec.Query = string.Format("select data,data_type from other_info where bin = '{0}' and tax_year = '{1}' and bns_code = '{2}'", TransferAppFrm.BIN, ConfigurationAttributes.CurrentYear, m_strBnsCode);
                                    pRec.Query += string.Format(" and default_code = '{0}' and data_type = '{1}' and rev_year = '{2}'", sDefaultCode, sDataType, ConfigurationAttributes.RevYear);
                                    if (pRec.Execute())
                                    {
                                        if (pRec.Read())
                                        {
                                            sDataType = pRec.GetString("data_type");
                                            TransferAppFrm.dgvOtherInfo[3, x].Value = pRec.GetDouble("data");
                                        }
                                        else
                                        {
                                            pRec.Close();

                                            pRec.Query = string.Format("select data,data_type, tax_year from other_info where bin = '{0}'", TransferAppFrm.BIN);
                                            pRec.Query += string.Format(" and default_code = '{0}' and data_type = '{1}' and rev_year = '{2}' order by tax_year desc", sDefaultCode, sDataType, ConfigurationAttributes.RevYear);
                                            if (pRec.Execute())
                                            {
                                                if (pRec.Read())
                                                {
                                                    sDataType = pRec.GetString("data_type");
                                                    TransferAppFrm.dgvOtherInfo[3, x].Value = pRec.GetDouble("data");
                                                }
                                                else
                                                    TransferAppFrm.dgvOtherInfo[3, x].Value = "0";
                                            }
                                            pRec.Close();
                                        }
                                    }
                                    pRec.Close();
                                }
                                
                            }
                            pRec.Close();
                        }
                        
                    }
                    pRec.Close();

                    string sValue = string.Empty;
                    try
                    {
                        d = Convert.ToDouble(TransferAppFrm.dgvOtherInfo[3, x].Value.ToString());
                    }
                    catch
                    {
                        d = 0;
                    }

			        if(sDataType == "A" || sDataType == "AR" || sDataType == "RR")
				        sValue = string.Format("{0:##.00}",d);
			        else 
				        sValue = string.Format("{0:##0}",d);

			        TransferAppFrm.dgvOtherInfo[3, x].Value = sValue;
                    x++;

				}
			}
            pSet.Close();

            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update change of business name (s)
            TransferAppFrm.dgvOtherInfo.Rows.Add("");
            TransferAppFrm.dgvOtherInfo[0, x].Value = "DTINO";
            TransferAppFrm.dgvOtherInfo[1, x].Value = "DTI/SEC NO.";
            TransferAppFrm.dgvOtherInfo[2, x].Value = " ";
            TransferAppFrm.dgvOtherInfo.Rows.Add("");
           
            TransferAppFrm.dgvOtherInfo[0, x+1].Value = "DTIDT";
            TransferAppFrm.dgvOtherInfo[1, x+1].Value = "DTI/SEC DATE (MM/DD/YYYY)";
            TransferAppFrm.dgvOtherInfo[2, x+1].Value = " ";
            TransferAppFrm.dgvOtherInfo.Rows.Add("");
            
            TransferAppFrm.dgvOtherInfo[0, x+2].Value = "MEMO";
            TransferAppFrm.dgvOtherInfo[1, x+2].Value = "MEMORANDA";
            TransferAppFrm.dgvOtherInfo[2, x+2].Value = " ";

            pRec.Query = string.Format("select * from TRANS_OTHER_INFO_ADDL where bin = '{0}' and tax_year = '{1}' and (default_code = 'DTINO' or default_code = 'DTIDT' or default_code = 'MEMO')", TransferAppFrm.BIN, ConfigurationAttributes.CurrentYear);
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    if (pRec.GetString("default_code") == "DTINO")
                        TransferAppFrm.dgvOtherInfo[3, x].Value = pRec.GetString("data");
                    if (pRec.GetString("default_code") == "DTIDT")
                        TransferAppFrm.dgvOtherInfo[3, x + 1].Value = pRec.GetString("data");
                    if (pRec.GetString("default_code") == "MEMO")
                        TransferAppFrm.dgvOtherInfo[3, x + 2].Value = pRec.GetString("data");
                }
            }
            pRec.Close();

            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update change of business name (e)
			
        }

        private void SaveApplication(string strTransaction)
        {
            if (strTransaction == "Save")
            {
                pSet.Query = "insert into transfer_table values (:1,'CB',' ',' ',' ',:2,' ',' ',' ',' ',' ',' ',' ',' ',' ',:3)";
                pSet.AddParameter(":1", TransferAppFrm.BIN);
                pSet.AddParameter(":2", StringUtilities.HandleApostrophe(TransferAppFrm.txtNewLastName.Text.ToString().Trim()));
                pSet.AddParameter(":3", string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate()));
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }
            else
            {

                pSet.Query = "update transfer_table set ";
                pSet.Query += string.Format("own_ln = '{0}', ",StringUtilities.HandleApostrophe(TransferAppFrm.txtNewLastName.Text.ToString().Trim()));
                pSet.Query += string.Format("app_date = '{0}'", string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate()));
                pSet.Query += string.Format(" where bin = '{0}' and trans_app_code = 'CB'", TransferAppFrm.BIN);
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }
        }

        private void SaveOtherInfo()
        {
            //Note: delete Edit/Update Other info button
            string strBnsCode = string.Empty;
            string sCode = string.Empty;
            string sType = string.Empty;
            string sValue = string.Empty;
            double dValue = 0;

            strBnsCode = StringUtilities.Left(m_strBnsCode,2);
                
            pSet.Query = string.Format("delete from trans_other_info where bin = '{0}' and tax_year = '{1}' and bns_code = '{2}'", TransferAppFrm.BIN, ConfigurationAttributes.CurrentYear, strBnsCode);
            if(pSet.ExecuteNonQuery() == 0)
            {}
            // RMC 20171129 corrected error in permit update (s)
            pSet.Query = string.Format("delete from TRANS_OTHER_INFO_ADDL where bin = '{0}' and tax_year = '{1}'", TransferAppFrm.BIN, ConfigurationAttributes.CurrentYear);
            if (pSet.ExecuteNonQuery() == 0)
            { }
            // RMC 20171129 corrected error in permit update (e)

            for (int iRow = 0; iRow < TransferAppFrm.dgvOtherInfo.Rows.Count; iRow++)
            {
                try
                {
                    sCode = TransferAppFrm.dgvOtherInfo[0, iRow].Value.ToString().Trim();
                    sType = TransferAppFrm.dgvOtherInfo[2, iRow].Value.ToString().Trim();
                    sValue = TransferAppFrm.dgvOtherInfo[3, iRow].Value.ToString().Trim();

                    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update change of business name (s)
                    if (sCode == "DTINO" || sCode == "DTIDT" || sCode == "MEMO")
                    {
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
                    else// RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update change of business name (e)
                    {
                        dValue = Convert.ToDouble(sValue);

                        if (dValue > 0)
                        {
                            if (sType == "A" || sType == "AR" || sType == "RR")
                                sValue = string.Format("{0:##.00}", dValue);
                            else
                                sValue = string.Format("{0:##0}", dValue);

                            pSet.Query = "insert into trans_other_info values(:1,:2,:3,:4,:5,:6,:7)";
                            pSet.AddParameter(":1", TransferAppFrm.BIN);
                            pSet.AddParameter(":2", ConfigurationAttributes.CurrentYear);
                            pSet.AddParameter(":3", m_strBnsCode);
                            pSet.AddParameter(":4", sCode);
                            pSet.AddParameter(":5", sType);
                            pSet.AddParameter(":6", sValue);
                            pSet.AddParameter(":7", ConfigurationAttributes.RevYear);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                        }
                    }
                }
                catch { }
            }

            TransferAppFrm.dgvOtherInfo.Enabled = false;

        }

        public override void Close()
        {
            LoadValues();
            LoadOtherInfo();
            TransferAppFrm.txtNewLastName.ReadOnly = true;
            TransferAppFrm.dgvOtherInfo.Enabled = false;
            TransferAppFrm.btnClose.Text = "Close";
            
        }

       
    }
}
