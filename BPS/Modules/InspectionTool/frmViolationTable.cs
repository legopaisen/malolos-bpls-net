using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.InspectionTool
{
    public partial class frmViolationTable : Form
    {
        public frmViolationTable()
        {
            InitializeComponent();
        }

        private void frmViolationTable_Load(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            string sQuery = string.Empty;

            sQuery = "select distinct(violation_code) \" Code\", violation_desc \" Violation Description\", reference \" Reference\" from violation_table order by violation_code";

            DataGridViewOracleResultSet dsUser = new DataGridViewOracleResultSet(dgvList, sQuery, 0, 0);
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 120;
            dgvList.Columns[1].Width = 250;
            dgvList.Columns[2].Width = 150;
            dgvList.Refresh();
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
                dgvList.Enabled = false;
                
                txtRef.Focus();
            }
            else
            {
                if (!Validate())
                    return;

                if (MessageBox.Show("Save record?", "Violation Table", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    GenerateCodeSeries();
                    pSet.Query = "insert into violation_table values (:1,:2,:3)";
                    pSet.AddParameter(":1", txtCode.Text.Trim());
                    pSet.AddParameter(":2", txtDesc.Text.Trim());
                    pSet.AddParameter(":3", txtRef.Text.Trim());
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("New record has been saved.", "Violation Table", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnAdd.Text = "Add";
                    btnClose.Text = "Close";
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    ClearControls();
                    EnableControls(false);
                    dgvList.Enabled = true;
                    UpdateList();
                }
            }
        }

        private void ClearControls()
        {
            txtCode.Text = "";
            txtDesc.Text = "";
            txtRef.Text = "";
        }

        private void EnableControls(bool blnEnable)
        {
            txtDesc.ReadOnly = !blnEnable;
            txtRef.ReadOnly = !blnEnable;
        }

        private bool Validate()
        {
            //if (txtCode.Text.Trim() == "" || txtDesc.Text.Trim() == "" || txtRef.Text.Trim() == "")
            if (txtDesc.Text.Trim() == "" || txtRef.Text.Trim() == "")  // RMC 20150117
            {
                MessageBox.Show("All fields required.", "Violation Table", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private void GenerateCodeSeries()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iCount = 0;

            pSet.Query = "select count(*) from violation_table";
            int.TryParse(pSet.ExecuteScalar().ToString(), out iCount);

            iCount++;

            txtCode.Text = string.Format("{0:0000000#}", iCount);

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
                dgvList.Enabled = false;
                txtRef.Focus();
            }
            else
            {
                if (!Validate())
                    return;

                if (MessageBox.Show("Do you want to save changes?", "Violation Table", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pSet.Query = string.Format("update violation_table set violation_desc = '{0}', reference = '{1}' where violation_code = '{2}'", txtDesc.Text.Trim(), txtRef.Text.Trim(), txtCode.Text.Trim());
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("Record updated.","Violation Table",MessageBoxButtons.OK,MessageBoxIcon.Information);

                    btnEdit.Text = "Edit";
                    btnClose.Text = "Close";
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    EnableControls(false);
                    ClearControls();
                    dgvList.Enabled = true;
                    UpdateList();
                }

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (txtCode.Text.Trim() == "")
            {
                MessageBox.Show("Select record to delete.", "Violation Table", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Do you want to delete "+txtCode.Text.Trim()+ "?", "Violation Table", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pSet.Query = string.Format("delete from violation_table where violation_code = '{0}'", txtCode.Text.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                { }

                MessageBox.Show("Record deleted.","Violation Table", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UpdateList();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Close")
                this.Close();
            else
            {
                ClearControls();
                EnableControls(false);
                btnAdd.Text = "Add";
                btnEdit.Text = "Edit";
                btnClose.Text = "Close";
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

            }
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadList(e.RowIndex);
        }

        private void LoadList(int iRow)
        {
            try
            {
                txtCode.Text = dgvList[0, iRow].Value.ToString().Trim();
                txtDesc.Text = dgvList[1, iRow].Value.ToString().Trim();
                txtRef.Text = dgvList[2, iRow].Value.ToString().Trim();
            }
            catch
            {
            }

        }

        private void dgvList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            LoadList(e.RowIndex);
        }

        private void dgvList_KeyDown(object sender, KeyEventArgs e)
        {
            int iRow = 0;
            iRow = dgvList.SelectedCells[0].RowIndex;

            LoadList(iRow);
        }

        private void containerWithShadow2_Load(object sender, EventArgs e)
        {

        }
    }
}