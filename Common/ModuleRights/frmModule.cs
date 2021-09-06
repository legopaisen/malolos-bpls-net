using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Common.ModuleRights
{
    public partial class frmModule : Form
    {
        private PrintModule PrintClass = null;
        private string m_sSource = string.Empty;
        private string m_sQuery = string.Empty;
        private string m_sTempRights = string.Empty;

        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }

        public frmModule()
        {
            InitializeComponent();
        }

        private void frmModule_Load(object sender, EventArgs e)
        {
            this.EnableControls(false);
            this.UpdateList();
            this.dgvList.CurrentCell = this.dgvList[0,0];
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Cancel")
            {
                this.btnNew.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.txtRights.Text = "";
                this.txtDesc.Text = "";
                this.txtID.Text = "";
                this.EnableControls(false);
                this.btnClose.Text = "Close";
                this.btnNew.Text = "New";
                this.btnEdit.Text = "Edit";
                int intRow = this.dgvList.SelectedCells[0].RowIndex;
                this.dgvList.CurrentCell = this.dgvList[0, intRow];
                this.SelectModule(intRow);
            }
            else
                this.Close();
        }

        private void dgvList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.SelectModule(e.RowIndex);
        }

        private void dgvList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.SelectModule(e.RowIndex);
        }

        private void SelectModule(int intRow)
        {
            this.btnNew.Text = "New";
            this.btnEdit.Text = "Edit";
            this.EnableControls(false);

            if (intRow >= 0)
            {
                txtRights.Text = dgvList[0, intRow].Value.ToString().Trim();
                txtDesc.Text = dgvList[1, intRow].Value.ToString().Trim();
                txtID.Text = dgvList[2, intRow].Value.ToString().Trim();

                m_sTempRights = txtRights.Text;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (btnNew.Text == "New")
            {
                this.txtRights.Text = "";
                this.txtDesc.Text = "";
                this.txtID.Text = "";
                this.btnNew.Text = "Save";
                this.btnClose.Text = "Cancel";
                this.btnEdit.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = false;
                this.EnableControls(true);
            }
            else
            {
                if (!Validation())
                    return;

                result.Query = string.Format("select * from sys_module where trim(usr_rights) = '{0}'", txtRights.Text.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("Rights already exists, please check.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        result.Close();
                        return;
                    }
                }

                if (MessageBox.Show("Save record?", m_sSource, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                OracleResultSet pSet = new OracleResultSet();

                pSet.Query = "insert into sys_module (usr_rights, right_desc, ctrl_id) values (:1, :2, :3)";
                pSet.AddParameter(":1", txtRights.Text.Trim());
                pSet.AddParameter(":2", txtDesc.Text.Trim());
                pSet.AddParameter(":3", txtID.Text.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show("Failed to save module.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.UpdateList();
                this.btnNew.Text = "New";
                this.btnClose.Text = "Close";
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.EnableControls(false);
            }
        }

        private void UpdateList()
        {
            if (m_sSource == "BPS")
                m_sQuery = "select usr_rights \" Rights\", right_desc \" Description\", ctrl_id \" Control ID\" from sys_module where substr(usr_rights,1,1) = 'A' order by usr_rights";    // RMC 20110902 added ordering in module rights
            else
                m_sQuery = "select usr_rights \" Rights\", right_desc \" Description\", ctrl_id \" Control ID\" from sys_module where substr(usr_rights,1,1) = 'C' order by usr_rights";    // RMC 20110902 added ordering in module rights

            DataGridViewOracleResultSet dsUser = new DataGridViewOracleResultSet(dgvList, m_sQuery, 0, 0);
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 70;
            dgvList.Columns[1].Width = 250;
            dgvList.Columns[2].Width = 90;
            dgvList.Refresh();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            
            if (!Validation())
                return;

            if (btnEdit.Text == "Edit")
            {
                this.EnableControls(true);
                this.btnNew.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = false;
                this.btnEdit.Text = "Update";
                this.btnClose.Text = "Cancel";
            }
            else
            {
                result.Query = string.Format("select * from sys_module where trim(usr_rights) = '{0}'", m_sTempRights);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        // check if the Module Code is changed
                        if (txtRights.Text.Trim() != m_sTempRights)
                        {
                            if (MessageBox.Show("You have attempted to change the Module Code for this record.\nThis is not advisable.  Do this only when it is highly needed!\nDo you wish to continue?", m_sSource, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                txtRights.Focus();
                                return;
                            }
                            else
                            {
                                result.Close();

                                result.Query = string.Format("select usr_rights from sys_module where usr_rights = '{0}'", txtRights.Text.Trim());
                                if (result.Execute())
                                {
                                    if (result.Read())
                                    {
                                        MessageBox.Show("Rights: " + txtRights.Text + " already exists.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        txtRights.Focus();
                                        return;
                                    }
                                }
                                result.Close();

                            }
                        }

                        result.Query = string.Format("update sys_module set usr_rights = :1, right_desc = :2, ctrl_id = :3 where usr_rights = '{0}'", m_sTempRights);
                        result.AddParameter(":1", txtRights.Text.Trim());
                        result.AddParameter(":2", txtDesc.Text.Trim());
                        result.AddParameter(":3", txtID.Text.Trim());
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Cannot update record for Rights: " + txtRights.Text.Trim() + ".\n It is no longer found in the database.\nDo you want to add the new record for this Control ID instead?", m_sSource, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.btnNew.Text = "Save";
                            this.btnNew.Enabled = true;
                            this.btnEdit.Enabled = false;
                        }
                    }
                }
                result.Close();

                this.EnableControls(false);
                this.btnNew.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.btnClose.Text = "Close";
                int intRow = this.dgvList.SelectedCells[0].RowIndex;

                this.UpdateList();
                
                this.dgvList.CurrentCell = this.dgvList[0, intRow];
                this.SelectModule(intRow);
            }
            
                
                
        }

        private bool Validation()
        {
            if (txtRights.Text.Trim() == "")
            {
                MessageBox.Show("Rights field is required, please check.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (txtDesc.Text.Trim() == "")
            {
                MessageBox.Show("Description field is required, please check.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private void EnableControls(bool blnEnable)
        {
            this.txtRights.Enabled = blnEnable;
            this.txtDesc.Enabled = blnEnable;
            this.txtID.Enabled = blnEnable;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (txtRights.Text.Trim() == "")
            {
                MessageBox.Show("Select record to delete.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("All selected items will be permanently DELETED.  Please confirm.", m_sSource, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                result.Query = string.Format("Select * from a_trail where mod_code = '{0}'", txtRights.Text.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("This module has been used. Deletion not allowed.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                result.Close();


                result.Query = string.Format("delete from sys_module where usr_rights = '{0}'", txtRights.Text.Trim());
                if (result.ExecuteNonQuery() == 0)
                { }
                
                UpdateList();
                this.txtRights.Text = "";
                this.txtDesc.Text = "";
                this.txtID.Text = "";
                this.EnableControls(false);
            }		
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            PrintClass = new PrintModule();
            PrintClass.ReportName = "List of BPLS System Modules";
            PrintClass.FormLoad();
        }

    }
}