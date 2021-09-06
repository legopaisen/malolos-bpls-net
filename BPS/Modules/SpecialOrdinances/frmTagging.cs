using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.SearchBusiness;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.SpecialOrdinances
{
    public partial class frmTagging : Form
    {
        frmSearchBusiness frmSearchBns = new frmSearchBusiness();
        private string m_sBin = string.Empty;
        private string m_sSwitch = string.Empty;
        private string m_sOrdinance = string.Empty; // RMC 20161227 modified BOI/PEZA exemption desc
        
        public string BIN
        {
            get { return m_sBin; }
            set { m_sBin = value; }
        }

        public string Switch
        {
            get { return m_sSwitch; }
            set { m_sSwitch = value; }
        }

        public frmTagging()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private void frmTagging_Load(object sender, EventArgs e)
        {
            chkTagging.Checked = true;

            if (m_sSwitch == "BUSS-REC")
                chkReport.Visible = false;

            /*cmbOrdinance.Items.Add("PEZA");
            cmbOrdinance.Items.Add("BOI");*/    // RMC 20161227 modified BOI/PEZA exemption desc

            // RMC 20161227 modified BOI/PEZA exemption desc (s)
            cmbOrdinance.Items.Add("BOI (PIONEER FIRM)");
            cmbOrdinance.Items.Add("BOI (NON-PIONEER FIRM)");
            cmbOrdinance.Items.Add("BOI (EXPANDING FIRM)");
            cmbOrdinance.Items.Add("PEZA (RA-7916)");
            cmbOrdinance.Items.Add("REGIONAL/AREA HQ/OFFICE (RA-8756)");
            // RMC 20161227 modified BOI/PEZA exemption desc (e)

            cmbOrdinance.SelectedIndex = 0;

            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            //SearchBin();      // RMC 20161227 modified BOI/PEZA exemption desc, put rem
        }

        private void chkTagging_CheckStateChanged(object sender, EventArgs e)
        {
            if(chkTagging.CheckState.ToString() == "Checked")
            {
                bin1.Enabled = true;
                btnDelete.Enabled = true;
                btnSave.Enabled = true;
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                btnSearchBIN.Enabled = true;
                btnGenerate.Enabled = false;
                chkReport.Checked = false;
                bin1.txtTaxYear.Focus();
                
            }
            
        }

        private void chkReport_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkReport.CheckState.ToString() == "Checked")
            {
                bin1.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                btnSearchBIN.Enabled = false;
                btnGenerate.Enabled = true;
                chkTagging.Checked = false;
                btnGenerate.Focus();
                txtRegNo.ReadOnly = true;    // RMC 20110914 added reg_no and reg_dt in boi tagging module
                dtpRegDate.Enabled = false;  // RMC 20110914 added reg_no and reg_dt in boi tagging module
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbOrdinance.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Select ordinance", "Special Ordinances", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Print " + cmbOrdinance.Text.Trim() + "?", "Special Ordinances", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            // add printing select module
            using (frmReport frmReport = new frmReport())
            {
                frmReport.Switch = cmbOrdinance.Text.Trim();
                frmReport.ShowDialog();
            }
        }

        private void btnSearchBIN_Click(object sender, EventArgs e)
        {
            if (btnSearchBIN.Text == "Search Bin")
            {
                if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
                {
                    m_sBin = bin1.GetBin();
                    SearchBin();
                }
                else
                {
                    frmSearchBns = new frmSearchBusiness();
                    frmSearchBns.ModuleCode = "";

                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();

                        m_sBin = bin1.GetBin();
                        SearchBin();

                    }
                }
            }
            else
            {
                ClearControls();
                bin1.txtTaxYear.Focus();
            }
        }

        private void SearchBin()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            string sOwnCode = string.Empty;
            string sBoiBin = string.Empty;
            string sBoiUser = string.Empty;
            string sBoiDate = string.Empty;

            pRec.Query = string.Format("Select * from businesses where bin = '{0}'", m_sBin);
            if(pRec.Execute())
            {
                if (pRec.Read())
                { 
                    pRec.Close();
                    pRec.Query = string.Format("Select * from businesses where bin = '{0}'", m_sBin);
                }
                else
                {
                    pRec.Close();
                    pRec.Query = string.Format("Select * from business_que where bin = '{0}'", m_sBin);
                    
                }
            }

            if(pRec.Execute())
            {
                if(pRec.Read())
                {
                    btnSave.Enabled = true;
                    txtBnsName.Text = pRec.GetString("bns_nm").Trim();
                    sOwnCode = pRec.GetString("own_code").Trim();
                    txtOwner.Text = AppSettingsManager.GetBnsOwner(sOwnCode);
		
                    pSet.Query = string.Format("select * from boi_table where bin = '{0}'", m_sBin);
                    if(pSet.Execute())
                    {
                        if(pSet.Read())
                        {
                            sBoiBin = pSet.GetString("bin");
				            dtpFrom.Value = pSet.GetDateTime("datefrom");
				            dtpTo.Value = pSet.GetDateTime("dateto");
				            sBoiUser = pSet.GetString("user_code");
				            sBoiDate = pSet.GetString("date_save");
                            cmbOrdinance.Text = pSet.GetString("exempt_type").Trim();

                            // RMC 20110914 (S)
                            dtpRegDate.Value = pSet.GetDateTime("reg_dt");
                            txtRegNo.Text = pSet.GetString("reg_no");
                            // RMC 20110914 (e)

                            // RMC 20161227 modified BOI/PEZA exemption desc (s)
                            if (cmbOrdinance.Text.ToString() == "BOI (PIONEER FIRM)")
                                m_sOrdinance = "BOI-PF";
                            else if (cmbOrdinance.Text.ToString() == "BOI (NON-PIONEER FIRM)")
                                m_sOrdinance = "BOI-NPF";
                            else if (cmbOrdinance.Text.ToString() == "BOI (EXPANDING FIRM)")
                                m_sOrdinance = "BOI-EF";
                            else if (cmbOrdinance.Text.ToString() == "PEZA (RA-7916)")
                                m_sOrdinance = "PEZA-RA-7916";
                            else if (cmbOrdinance.Text.ToString() == "REGIONAL/AREA HQ/OFFICE (RA-8756)")
                                m_sOrdinance = "REG-RA-8756";
                            else
                                m_sOrdinance = "";
                            // RMC 20161227 modified BOI/PEZA exemption desc (e)

							btnSave.Text = "Update";
                            btnDelete.Enabled = true;
			            }
			            else
			            {
				            btnSave.Text = "Save";
                            btnDelete.Enabled = false;

			            }
                    }
                    pSet.Close();
			
                    btnSearchBIN.Text = "Clear Bin";
                    txtRegNo.ReadOnly = false;    // RMC 20110914 added reg_no and reg_dt in boi tagging module
                    dtpRegDate.Enabled = true;  // RMC 20110914 added reg_no and reg_dt in boi tagging module
                }
            }
		    pRec.Close();
        }

        private void cmbOrdinance_SelectedIndexChanged(object sender, EventArgs e)
        {
            // RMC 20161227 modified BOI/PEZA exemption desc (s)
            if (cmbOrdinance.Text.ToString() == "BOI (PIONEER FIRM)")
                m_sOrdinance = "BOI-PF";
            else if (cmbOrdinance.Text.ToString() == "BOI (NON-PIONEER FIRM)")
                m_sOrdinance = "BOI-NPF";
            else if (cmbOrdinance.Text.ToString() == "BOI (EXPANDING FIRM)")
                m_sOrdinance = "BOI-EF";
            else if (cmbOrdinance.Text.ToString() == "PEZA (RA-7916)")
                m_sOrdinance = "PEZA-RA-7916";
            else if (cmbOrdinance.Text.ToString() == "REGIONAL/AREA HQ/OFFICE (RA-8756)")
                m_sOrdinance = "REG-RA-8756";
            else
                m_sOrdinance = "";
            
            // RMC 20161227 modified BOI/PEZA exemption desc (e)

            // SearchBin();  // RMC 20161227 modified BOI/PEZA exemption desc, put rem
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            if (bin1.txtTaxYear.Text == "" || bin1.txtBINSeries.Text == "")
            {
                MessageBox.Show("Select BIN to untag", "Special Ordinances", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            m_sBin = bin1.GetBin();

            if (txtBnsName.Text.Trim() == "")
            {
                MessageBox.Show("Select record to delete","Special Ordinances",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Delete tag of BIN:" + m_sBin + " under " + cmbOrdinance.Text.Trim() + "?", "Special Ordinances", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // boi_table_hist
                pRec.Query = "insert into boi_table_hist ";
                pRec.Query += string.Format("select * from boi_table where bin = '{0}' ", m_sBin);
                pRec.Query += string.Format(" and exempt_type = '{0}'", cmbOrdinance.Text.Trim());
                if (pRec.ExecuteNonQuery() == 0)
                { }
                
                // boi_table
                pRec.Query = string.Format("delete from boi_table where bin = '{0}'", m_sBin);
                pRec.Query += string.Format(" and exempt_type = '{0}'", cmbOrdinance.Text.Trim());
                if (pRec.ExecuteNonQuery() == 0)
                { }

                MessageBox.Show("Tag deleted.", "Special Ordinances", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sDateFr = string.Empty;
                string sDateTo = string.Empty;

                sDateFr = string.Format("0:MM/dd/yyyy}", dtpFrom.Value);
                sDateTo = string.Format("0:MM/dd/yyyy}", dtpTo.Value);

                string strObj = "Un-Tagged: " + m_sBin + "/" + sDateFr + " to " + sDateTo;

                //if (AuditTrail.InsertTrail(cmbOrdinance.Text.Trim(), "boi_table", StringUtilities.HandleApostrophe(strObj)) == 0)
                if (AuditTrail.InsertTrail(m_sOrdinance, "boi_table", StringUtilities.HandleApostrophe(strObj)) == 0)   // RMC 20161227 modified BOI/PEZA exemption desc
                {
                    pRec.Rollback();
                    pRec.Close();
                    MessageBox.Show(pRec.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ClearControls();
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
            }

        }

        private void ClearControls()
        {
            btnSearchBIN.Text = "Search Bin";
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";
            txtBnsName.Text = "";
            txtOwner.Text = "";
            txtRegNo.Text = "";    // RMC 20110914 added reg_no and reg_dt in boi tagging module
            dtpRegDate.Value = AppSettingsManager.GetCurrentDate();  // RMC 20110914 added reg_no and reg_dt in boi tagging module
            txtRegNo.ReadOnly = true;    // RMC 20110914 added reg_no and reg_dt in boi tagging module
            dtpRegDate.Enabled = false;  // RMC 20110914 added reg_no and reg_dt in boi tagging module

            dtpFrom.Value = AppSettingsManager.GetCurrentDate();
            dtpTo.Value = AppSettingsManager.GetCurrentDate();
            m_sOrdinance = "";  // RMC 20161227 modified BOI/PEZA exemption desc
            m_sBin = "";    // RMC 20161227 modified BOI/PEZA exemption desc

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBnsType = string.Empty;
            string sDateFrom = string.Empty;
            string sDateTo = string.Empty;
            string sCurrentDate = string.Empty;
            string sRegDate = string.Empty;

            sDateFrom = string.Format("{0:MM/dd/yyyy}", dtpFrom.Value);
            sDateTo = string.Format("{0:MM/dd/yyyy}", dtpTo.Value);
            sRegDate = string.Format("{0:MM/dd/yyyy}", dtpRegDate.Value);   // RMC 20110914

            sCurrentDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", AppSettingsManager.GetCurrentDate());

            if (bin1.txtTaxYear.Text == "" || bin1.txtBINSeries.Text == "")
            {
                MessageBox.Show("Select BIN to tag", "Special Ordinances", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // RMC 20110914 (S)
            if (txtRegNo.Text.Trim() == "")
            {
                MessageBox.Show("Registration number required.","Speacial Ordinances",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            
            }
            // RMC 20110914 (E)

            pSet.Query = "select * from boi_table where reg_no = '" + txtRegNo.Text.Trim() + "' and bin <> '" + bin1.GetBin() + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("Registration number already used by BIN : " + pSet.GetString("bin"), "Special Ordinances",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pSet.Close();
                    return;
                }
                else
                    pSet.Close();
            }
            

            m_sBin = bin1.GetBin();

            if (btnSave.Text == "Save")
            {
                if (MessageBox.Show("Tag BIN:" + m_sBin + " under " + cmbOrdinance.Text.Trim() + "?", "Special Ordinances", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                pSet.Query = string.Format("select bns_code from businesses where bin = '{0}'", m_sBin);
                if(pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sBnsType = AppSettingsManager.GetBnsDesc(pSet.GetString("bns_code"));
                    }
                }
                pSet.Close();

                pSet.Query = "insert into boi_table values (:1,:2,:3,:4,:5,:6,:7,:8,:9)";
                pSet.AddParameter(":1", m_sBin);
                pSet.AddParameter(":2", DateTime.Parse(sDateFrom));
                pSet.AddParameter(":3", DateTime.Parse(sDateTo));
                pSet.AddParameter(":4", sCurrentDate);
                pSet.AddParameter(":5", AppSettingsManager.SystemUser.UserCode);
                pSet.AddParameter(":6", sBnsType);
                pSet.AddParameter(":7", cmbOrdinance.Text.Trim());
                pSet.AddParameter(":8", StringUtilities.HandleApostrophe(txtRegNo.Text.Trim()));
                pSet.AddParameter(":9", DateTime.Parse(sRegDate));
                if(pSet.ExecuteNonQuery() == 0)
                {}

                MessageBox.Show("Data saved.", "Special Ordinances",MessageBoxButtons.OK, MessageBoxIcon.Information);

                string strObj = "Tagged: " + m_sBin + "/" + sDateFrom + " to " + sDateTo;

                //if (AuditTrail.InsertTrail(cmbOrdinance.Text.Trim(), "boi_table", StringUtilities.HandleApostrophe(strObj)) == 0)
                if (AuditTrail.InsertTrail(m_sOrdinance, "boi_table", StringUtilities.HandleApostrophe(strObj)) == 0)   // RMC 20161227 modified BOI/PEZA exemption desc
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //SearchBin();
                
            }
            else if (btnSave.Text == "Update")
            {
                if (MessageBox.Show("Update BIN:" + m_sBin + " under " + cmbOrdinance.Text.Trim() + "?", "Special Ordinances", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                pSet.Query = "insert into boi_table_hist ";
                pSet.Query += string.Format("select * from boi_table where bin = '{0}'", m_sBin);
                pSet.Query += string.Format(" and exempt_type = '{0}'", cmbOrdinance.Text.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from boi_table where bin = '{0}'", m_sBin);
                pSet.Query += string.Format(" and exempt_type = '{0}'", cmbOrdinance.Text.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("select bns_code from businesses where bin = '{0}'", m_sBin);
                if(pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sBnsType = AppSettingsManager.GetBnsDesc(pSet.GetString("bns_code"));
                    }
                }
                pSet.Close();

                pSet.Query = "insert into boi_table values (:1,:2,:3,:4,:5,:6,:7,:8,:9)";
                pSet.AddParameter(":1", m_sBin);
                pSet.AddParameter(":2", DateTime.Parse(sDateFrom));
                pSet.AddParameter(":3", DateTime.Parse(sDateTo));
                pSet.AddParameter(":4", sCurrentDate);
                pSet.AddParameter(":5", AppSettingsManager.SystemUser.UserCode);
                pSet.AddParameter(":6", sBnsType);
                pSet.AddParameter(":7", cmbOrdinance.Text.Trim());
                pSet.AddParameter(":8", StringUtilities.HandleApostrophe(txtRegNo.Text.Trim()));
                pSet.AddParameter(":9", DateTime.Parse(sRegDate));
                if(pSet.ExecuteNonQuery() == 0)
                {}

                MessageBox.Show("Data updated.", "Special Ordinances",MessageBoxButtons.OK, MessageBoxIcon.Information);

                string strObj = "Edited: " + m_sBin + "/" + sDateFrom + " to " + sDateTo;

                //if (AuditTrail.InsertTrail(cmbOrdinance.Text.Trim(), "boi_table", StringUtilities.HandleApostrophe(strObj)) == 0)
                if (AuditTrail.InsertTrail(m_sOrdinance, "boi_table", StringUtilities.HandleApostrophe(strObj)) == 0)   // RMC 20161227 modified BOI/PEZA exemption desc
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                 
            }

            ClearControls();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}