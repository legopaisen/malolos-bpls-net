using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.Utilities
{
    public partial class frmBarangay : Form
    {
        private PrintOtherReports PrintClass = null;
        string m_strBrgyCode = string.Empty;
        string m_strBrgy = string.Empty;
        string m_strZone = string.Empty;
        string m_strDistCode = string.Empty;
        string m_strDist = string.Empty;

        public frmBarangay()
        {
            InitializeComponent();
        }

        private void frmBarangay_Load(object sender, EventArgs e)
        {
            this.UpdateList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Cancel")
            {
                this.btnNew.Text = "Add";
                this.btnEdit.Text = "Edit";
                this.btnClose.Text = "Close";
                this.EnableControls(false);
                this.btnNew.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.dgvList.Enabled = true;
                this.ClearControls();
            }
            else
                this.Close();
        }

        private void UpdateList()
        {
            string strQuery = string.Empty;
                        
            strQuery = "select brgy_code \" Brgy Code\", brgy_nm \" Brgy Name\", zone \" Zone\", dist_code \" Dist Code\", dist_nm \"Dist Name\" from brgy order by brgy_code asc";

            DataGridViewOracleResultSet dsUser = new DataGridViewOracleResultSet(dgvList, strQuery, 0, 0);
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 50;
            dgvList.Columns[1].Width = 150;
            dgvList.Columns[2].Width = 100;
            dgvList.Columns[3].Width = 50;
            dgvList.Columns[4].Width = 150;
            dgvList.Refresh();
        }

        private void SelectList(int intRow)
        {
            if (intRow >= 0)
            {
                txtBrgyCode.Text = dgvList[0, intRow].Value.ToString().Trim();
                txtBrgy.Text = dgvList[1, intRow].Value.ToString().Trim();
                txtZone.Text = dgvList[2, intRow].Value.ToString().Trim();
                txtDistCode.Text = dgvList[3, intRow].Value.ToString().Trim();
                txtDist.Text = dgvList[4, intRow].Value.ToString().Trim();

                m_strBrgyCode = txtBrgyCode.Text;
                m_strBrgy = txtBrgy.Text;
                m_strZone = txtZone.Text;
                m_strDistCode = txtDistCode.Text;
                m_strDist = txtDist.Text;
            }
        }

        private void dgvList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.SelectList(e.RowIndex);
        }

        private void dgvList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.SelectList(e.RowIndex);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (btnNew.Text == "Add")
            {
                this.ClearControls();
                this.EnableControls(true);
                this.btnNew.Text = "Save";
                this.btnEdit.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = false;
                this.btnClose.Text = "Cancel";
                this.dgvList.Enabled = false;
                //this.txtBrgyCode.Focus();
                this.GenerateBrgyCode();
                this.txtBrgy.Focus();
            }
            else
            {
                if(txtBrgy.Text.Trim() == "" || txtBrgyCode.Text.Trim() == "")
		        {	
                    MessageBox.Show("Please Supply Sufficient Data","Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
		        }
		
                if ((txtDist.Text.Trim() == "" || txtDistCode.Text.Trim() == "") && AppSettingsManager.GetConfigObject("23") == "Y")
		        {
			        MessageBox.Show("Please Supply Sufficient Data","Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			        return;
		        }

                if(txtZone.Text.Trim() == "" && AppSettingsManager.GetConfigObject("24") == "Y")
				{
			        MessageBox.Show("Please Supply Sufficient Data","Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			        return;
		        }
		
                result.Query = string.Format("select * from brgy where brgy_code ='{0}'",txtBrgyCode.Text.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        string strTempBrgyCode = result.GetString("brgy_code");
                        string strTempBrgy = result.GetString("brgy_nm");
                        string strTempDistCode = result.GetString("dist_code");

                        MessageBox.Show("Brgy Code: " + strTempBrgyCode + " and Brgy Name: " + strTempBrgy + "and District Code: " + strTempDistCode + " already exists.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        result.Close();

                        result.Query = "insert into brgy (brgy_code, brgy_nm, zone, dist_code, dist_nm) values (:1,:2,:3,:4,:5)";
                        result.AddParameter(":1", txtBrgyCode.Text.Trim());
                        result.AddParameter(":2", txtBrgy.Text.Trim());
                        result.AddParameter(":3", txtZone.Text.Trim());
                        result.AddParameter(":4", txtDistCode.Text.Trim());
                        result.AddParameter(":5", StringUtilities.SetEmptyToSpace(txtDist.Text.Trim()));
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        string strObj = txtBrgyCode.Text.Trim() + "-" + txtBrgy.Text.Trim() + "=" + txtDistCode.Text.Trim();
                        
                        if (AuditTrail.InsertTrail("AUTB-A", "brgy", StringUtilities.HandleApostrophe(strObj)) == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                result.Close();

                this.UpdateList();
	
                this.btnNew.Text = "Add";
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.btnClose.Text = "Close";
                this.dgvList.Enabled = true;
                this.EnableControls(false);
            }
        }

        private void ClearControls()
        {
            txtBrgyCode.Text = "";
            txtBrgy.Text = "";
            txtZone.Text = "";
            txtDistCode.Text = "";
            txtDist.Text = "";
        }

        private void EnableControls(bool blnEnable)
        {
            //txtBrgyCode.ReadOnly = !blnEnable;
            txtBrgy.ReadOnly = !blnEnable;

            if (AppSettingsManager.GetConfigObject("23") == "N")
            {
                //txtDistCode.ReadOnly = true;
                txtDist.ReadOnly = true;
            }
            else
            {
                //txtDistCode.ReadOnly = !blnEnable;
                txtDist.ReadOnly = !blnEnable;
            }

            if (AppSettingsManager.GetConfigObject("24") == "N")
                txtZone.ReadOnly = true;
            else
                txtZone.ReadOnly = !blnEnable;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (btnEdit.Text == "Edit")
            {
                if (txtBrgyCode.Text.Trim() == "" || txtBrgy.Text.Trim() == "")
                {
                    MessageBox.Show("No record selected. Please select record to edit.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                this.EnableControls(true);
                this.dgvList.Enabled = false;
                this.btnNew.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = false;
                this.btnClose.Text = "Cancel";
                this.btnEdit.Text = "Update";
            }
            else
            {
                result.Query = string.Format("select * from brgy where brgy_code = '{0}' and brgy_code <> '{1}'", txtBrgyCode.Text.Trim(), m_strBrgyCode);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("Duplicate Barangay Code.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                result.Close();

                if (txtBrgy.Text.Trim() == "" || txtBrgyCode.Text.Trim() == "")
                {
                    MessageBox.Show("Please Supply Sufficient Data", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if ((txtDist.Text.Trim() == "" || txtDistCode.Text.Trim() == "") && AppSettingsManager.GetConfigObject("23") == "Y")
                {
                    MessageBox.Show("Please Supply Sufficient Data", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (txtZone.Text.Trim() == "" && AppSettingsManager.GetConfigObject("24") == "Y")
                {
                    MessageBox.Show("Please Supply Sufficient Data", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                result.Query = string.Format("select * from brgy where brgy_nm = '{0}' and brgy_nm <> '{1}'", txtBrgy.Text.Trim(), m_strBrgy);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("Duplicate Barangay Name.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                result.Close();

                StringBuilder sQuery = new StringBuilder();

                sQuery.Append(string.Format("update brgy set brgy_code = '{0}', ", txtBrgyCode.Text.Trim()));
                if (AppSettingsManager.GetConfigObject("24") == "Y")
                    sQuery.Append(string.Format("zone = '{0}', ", txtZone.Text.Trim()));
                if (AppSettingsManager.GetConfigObject("23") == "Y")
                {
                    sQuery.Append(string.Format("dist_code = '{0}', ", txtDistCode.Text.Trim()));
                    sQuery.Append(string.Format("dist_nm = '{0}', ", StringUtilities.SetEmptyToSpace(txtDist.Text.Trim())));
                }
                sQuery.Append(string.Format("brgy_nm = '{0}' ", txtBrgy.Text.Trim()));
                sQuery.Append(string.Format("where brgy_code = '{0}' ", m_strBrgyCode));
                sQuery.Append(string.Format("and brgy_nm = '{0}' ", m_strBrgy));
                if (AppSettingsManager.GetConfigObject("24") == "Y")
                    sQuery.Append(string.Format("and zone = '{0}' ", m_strZone));
                if (AppSettingsManager.GetConfigObject("23") == "Y")
                {
                    sQuery.Append(string.Format("and dist_code = '{0}'", m_strDistCode));
                    sQuery.Append(string.Format("and dist_nm = '{0}'", m_strDist));
                }

                result.Query = sQuery.ToString();
                if (result.ExecuteNonQuery() == 0)
                {
                }

                string strObj = txtBrgyCode.Text.Trim() + "-" + txtBrgy.Text.Trim() + "=" + txtDistCode.Text.Trim();

                if (AuditTrail.InsertTrail("AUTB-U", "brgy", StringUtilities.HandleApostrophe(strObj)) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.UpdateList();
                this.EnableControls(false);
                this.dgvList.Enabled = true;
                this.btnNew.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.btnClose.Text = "Close";
                this.btnEdit.Text = "Edit";
            }        
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (txtBrgyCode.Text.Trim() == "" || txtBrgy.Text.Trim() == "")
            {
                MessageBox.Show("No record selected. Please select record to delete.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
	    	else 
            {
                if (MessageBox.Show("The Barangay "+txtBrgy.Text+" will be permanently DELETED.\nPlease confirm.", "Barangay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
                    StringBuilder sQuery = new StringBuilder();

                    sQuery.Append(string.Format("delete from brgy where brgy_code = '{0}' and brgy_nm = '{1}' ", txtBrgyCode.Text.Trim(), txtBrgy.Text.Trim()));
                    if(AppSettingsManager.GetConfigObject("23") == "Y")
                        sQuery.Append(string.Format("and dist_code = '{0}'", txtDistCode.Text.Trim()));
                    result.Query = sQuery.ToString();
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                    string strObj = txtBrgyCode.Text.Trim() + "-" + txtBrgy.Text.Trim() + "=" + txtDistCode.Text.Trim();

                    if (AuditTrail.InsertTrail("AUTB-D", "brgy", StringUtilities.HandleApostrophe(strObj)) == 0)
                    {
                        result.Rollback();
                        result.Close();
                        MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.UpdateList();
                    this.ClearControls();
			    }
	        }
        }

        private void GenerateBrgyCode()
        {
            OracleResultSet result = new OracleResultSet();
            string strCode = string.Empty;
            int intCode = 0;

            result.Query = "select * from brgy order by brgy_code desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    strCode = result.GetString("brgy_code");
                }
                else
                    strCode = "0";
            }
            result.Close();

            try
            {
                intCode = Convert.ToInt32(strCode) + 1;
                txtBrgyCode.Text = string.Format("{0:00#}", intCode);
            }
            catch
            {
                MessageBox.Show("Invalid barangay code.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtBrgyCode.Text = "";
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            /*PrintClass = new PrintOtherReports();
            PrintClass.ReportName = "List of Barangay";
            PrintClass.Source = "1";
            PrintClass.FormLoad();*/

            // RMC 20150520 corrections in reports (s)
            frmReportSched form = new frmReportSched();
            form.Source = "4";
            form.ReportName = "List of Barangay";
            form.ShowDialog();
            // RMC 20150520 corrections in reports (e)
        }
    }
}