using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.LogIn;

namespace Amellar.Modules.InspectionTool
{
    public partial class frmInspector : Form
    {
        private string m_sTmpInspectorCode = string.Empty;

        public frmInspector()
        {
            InitializeComponent();
        }

        private void frmInspector_Load(object sender, EventArgs e)
        {
            m_sTmpInspectorCode = "";
            UpdateList();
        }

        private void EnableControls(bool blnEnable)
        {
            txtInsCode.ReadOnly = !blnEnable;
            txtLastName.ReadOnly = !blnEnable;
            txtFirstName.ReadOnly = !blnEnable;
            txtMI.ReadOnly = !blnEnable;
            txtPosition.ReadOnly = !blnEnable;
            txtPassword.ReadOnly = !blnEnable;
            txtConfirmPass.ReadOnly = !blnEnable;
            
        }

        private void ClearControls()
        {
            txtInsCode.Text = "";
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMI.Text = "";
            txtPosition.Text = "";
            txtPassword.Text = "";
            txtConfirmPass.Text = "";
        }

        private void UpdateList()
        {
            string sQuery = string.Empty;

            sQuery = "select distinct(inspector_code) \" Code\", inspector_ln \" Last Name\", inspector_fn \" First Name\", inspector_mi \" MI\", position \" Position\" from inspector order by inspector_code";

            DataGridViewOracleResultSet dsUser = new DataGridViewOracleResultSet(dgvInspectors, sQuery, 0, 0);
            dgvInspectors.RowHeadersVisible = false;
            dgvInspectors.Columns[0].Width = 120;
            dgvInspectors.Columns[1].Width = 150;
            dgvInspectors.Columns[2].Width = 100;
            dgvInspectors.Columns[3].Width = 50;
            dgvInspectors.Columns[4].Width = 120;
            dgvInspectors.Refresh();


        }

        private void dgvInspectors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadValues(e.RowIndex);
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Close")
                this.Close();
            else
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnAdd.Text = "Add";
                btnEdit.Text = "Edit";
                btnClose.Text = "Close";

                ClearControls();
                EnableControls(false);
                dgvInspectors.Enabled = true;
                UpdateList();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (btnAdd.Text == "Add")
            {
                btnAdd.Text = "Save";
                btnClose.Text = "Cancel";
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                ClearControls();
                EnableControls(true);
                dgvInspectors.Enabled = false;
                txtInsCode.Focus();
            }
            else
            {
                if (!ValidateControls())
                    return;

                pSet.Query = string.Format("select * from inspector where inspector_code = '{0}'", txtInsCode.Text.Trim());
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Inspector code already exists.", "Inspector", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtInsCode.Focus();
                        return;
                    }
                    else
                    {
                        pSet.Close();

                        EncryptPass encrypt = new EncryptPass();
                        string strPassword = encrypt.EncryptPassword(txtPassword.Text.Trim());

                        if (MessageBox.Show("Save?", "Inspector", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            pSet.Query = "insert into inspector values (:1,:2,:3,:4,:5,:6)";
                            pSet.AddParameter(":1", txtInsCode.Text.Trim());
                            pSet.AddParameter(":2", txtLastName.Text.Trim());
                            pSet.AddParameter(":3", txtFirstName.Text.Trim());
                            pSet.AddParameter(":4", txtMI.Text.Trim());
                            pSet.AddParameter(":5", strPassword);
                            pSet.AddParameter(":6", txtPosition.Text.Trim());
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            MessageBox.Show("New record has been saved.", "Inspector", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            UpdateList();
                            EnableControls(false);

                            btnAdd.Text = "Add";
                            btnClose.Text = "Close";
                            btnEdit.Enabled = true;
                            btnDelete.Enabled = true;
                            dgvInspectors.Enabled = true;
                        }
                    }
                }
            }

        }

        private bool ValidateControls()
        {
            if (txtInsCode.Text.Trim() == "" || txtLastName.Text.Trim() == "" ||
                txtFirstName.Text.Trim() == "" || txtMI.Text.Trim() == "" ||
                txtPosition.Text.Trim() == "" || txtPassword.Text.Trim() == "" || txtConfirmPass.Text.Trim() == "")
            {
                MessageBox.Show("All fields are required.","Inspector", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (txtPassword.Text.Trim() != txtConfirmPass.Text.Trim())
            {
                MessageBox.Show("Incorect Password.", "Inspector", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (btnEdit.Text == "Edit")
            {
                btnEdit.Text = "Update";
                btnClose.Text = "Cancel";
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                EnableControls(true);
                dgvInspectors.Enabled = false;
                txtInsCode.Focus();
            }
            else
            {
                if (!ValidateControls())
                    return;

                pSet.Query = string.Format("select * from inspector where inspector_code = '{0}' and inspector_code <> '{1}'", txtInsCode.Text.Trim(), m_sTmpInspectorCode);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Inspector code already exists.", "Inspector", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtInsCode.Focus();
                        return;
                    }
                    else
                    {
                        pSet.Close();

                        EncryptPass encrypt = new EncryptPass();
                        string strPassword = encrypt.EncryptPassword(txtPassword.Text.Trim());

                        if (MessageBox.Show("Update?", "Inspector", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            pSet.Query = string.Format("update inspector set inspector_code = '{0}', ", txtInsCode.Text.Trim());
                            pSet.Query+= string.Format("inspector_ln = '{0}', inspector_fn = '{1}', ", txtLastName.Text.Trim(), txtFirstName.Text.Trim());
                            pSet.Query+= string.Format("inspector_mi = '{0}', password = '{1}', ", txtMI.Text.Trim(), strPassword);
                            pSet.Query+= string.Format("position = '{0}' where inspector_code = '{1}'", txtPosition.Text.Trim(), m_sTmpInspectorCode);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            MessageBox.Show("Record has been updated.", "Inspector", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            btnEdit.Text = "Edit";
                            btnClose.Text = "Close";
                            btnAdd.Enabled = true;
                            btnDelete.Enabled = true;
                            EnableControls(false);
                            dgvInspectors.Enabled = true;

                            UpdateList();
                        }
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (txtInsCode.Text.Trim() == "")
            {
                MessageBox.Show("Select record to delete.","Inspector", MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Delete?", "Inspector", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pSet.Query = string.Format("delete from inspector where inspector_code = '{0}'", txtInsCode.Text.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                { }

                MessageBox.Show("Record has been deleted.", "Inspector", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateList();
                ClearControls();                                
                
            }
        }

        private void dgvInspectors_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            LoadValues(e.RowIndex);
        }

        private void LoadValues(int iRow)
        {
            try
            {
                txtInsCode.Text = dgvInspectors[0, iRow].Value.ToString().Trim();
                txtLastName.Text = dgvInspectors[1, iRow].Value.ToString().Trim();
                txtFirstName.Text = dgvInspectors[2, iRow].Value.ToString().Trim();
                txtMI.Text = dgvInspectors[3, iRow].Value.ToString().Trim();
                txtPosition.Text = dgvInspectors[4, iRow].Value.ToString().Trim();
                m_sTmpInspectorCode = txtInsCode.Text.Trim();
            }
            catch
            {
            }
        }

        private void dgvInspectors_KeyDown(object sender, KeyEventArgs e)
        {
            int iRow = 0;
            iRow = dgvInspectors.SelectedCells[0].RowIndex;

            LoadValues(iRow);
        }
    }
}