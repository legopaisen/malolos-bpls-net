using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace NewBankModule
{
    public partial class frmBank : Form
    {
        string m_sBnsCode = string.Empty;
        public frmBank()
        {
            InitializeComponent();
        }

        private void InitialControlState()
        {
            btnAdd.Text = "Add";
            btnEdit.Text = "Edit";
            btnClose.Text = "Close";
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            txtBankBranch.ReadOnly = true;
            txtBnkName.ReadOnly = true;
            txtBnkName.Text = string.Empty;
            txtBankBranch.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtAddress.ReadOnly = true;
            dgvBanks.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text.Trim() == "Close")
                this.Close();
            else
            {
                InitialControlState();
            }
        }

        private string GetBankCode()
        {
            string sResult = string.Empty;
            int iCode = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select max(bank_code) as bank_code from bank_table";
            int.TryParse(result.ExecuteScalar(), out iCode);

            sResult = string.Format("{0:000}", iCode + 1);
            return sResult;
        }

        private void Loadbank()
        {
            dgvBanks.Rows.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from bank_table order by bank_nm";
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgvBanks.Rows.Add(result.GetString("bank_code").Trim(), result.GetString("BANK_NM").Trim(), result.GetString("BANK_BRANCH").Trim(), result.GetString("BANK_ADD").Trim());
                }
            }
            result.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string sBnkCode = string.Empty;
            if (btnAdd.Text.Trim() == "Add")
            {
                btnAdd.Text = "Save";
                btnClose.Text = "Cancel";
                btnEdit.Enabled = false;
                txtBankBranch.ReadOnly = false;
                txtBnkName.ReadOnly = false;
                txtBnkName.Text = string.Empty;
                txtBankBranch.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtAddress.ReadOnly = false;
                txtBnkName.Focus();
                dgvBanks.Enabled = false;
            }
            else
            {
                OracleResultSet result = new OracleResultSet();

                // RMC 20150707 corrected saving of Bank table with incomplete details (s)
                if (txtBnkName.Text.Trim() == "" || txtBankBranch.Text.Trim() == "" || txtAddress.Text.Trim() == "")
                {
                    MessageBox.Show("Fill all the Empty Fields.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // RMC 20150707 corrected saving of Bank table with incomplete details (e)

                if (MessageBox.Show("Do you want to save this bank?","Saving Bank",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sBnkCode = GetBankCode();
                    result.Query = "insert into bank_table values (:1, :2, :3, :4)";
                    result.AddParameter(":1", sBnkCode.Trim());
                    result.AddParameter(":2", txtBnkName.Text.Trim());
                    result.AddParameter(":3", txtBankBranch.Text.Trim());
                    result.AddParameter(":4", txtAddress.Text.Trim());
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();

                    MessageBox.Show("Bank saved.");
                    Loadbank();
                    InitialControlState();
                }
            }
        }

        private void dgvBanks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                m_sBnsCode = dgvBanks.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtBnkName.Text = dgvBanks.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtBankBranch.Text = dgvBanks.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtAddress.Text = dgvBanks.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit")
            {
                btnAdd.Enabled = false;
                btnEdit.Text = "Update";
                btnClose.Text = "Cancel";
                txtAddress.ReadOnly = false;
                txtBankBranch.ReadOnly = false;
                txtBnkName.ReadOnly = false;
                dgvBanks.Enabled = false;
            }
            else
            {
                OracleResultSet result = new OracleResultSet();
                if (MessageBox.Show("Do you want to update this bank?", "Updating Bank", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    result.Query = "delete from bank_table where bank_code = :1";
                    result.AddParameter(":1", m_sBnsCode);
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();

                    result.Query = "insert into bank_table values (:1, :2, :3, :4)";
                    result.AddParameter(":1", m_sBnsCode.Trim());
                    result.AddParameter(":2", txtBnkName.Text.Trim());
                    result.AddParameter(":3", txtBankBranch.Text.Trim());
                    result.AddParameter(":4", txtAddress.Text.Trim());
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();

                    MessageBox.Show("Bank saved.");
                    Loadbank();
                    InitialControlState();
                }
            }
        }

        private void frmBank_Load(object sender, EventArgs e)
        {
            Loadbank(); 
        }
    }
}