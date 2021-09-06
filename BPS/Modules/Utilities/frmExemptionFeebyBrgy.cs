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

namespace Amellar.Modules.Utilities
{
    public partial class frmExemptionFeebyBrgy : Form
    {
        public frmExemptionFeebyBrgy()
        {
            InitializeComponent();
        }

        String m_strRevYear = string.Empty;
        String m_strTempSelectedValue = string.Empty;

        private void frmExemptionFeebyBrgy_Load(object sender, EventArgs e)
        {
            GetRevYear();
            LoadBrgy();
            LoadFees();
        }

        private void LoadFees()
        {
            OracleResultSet result = new OracleResultSet();
            //result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}' order by fees_code", m_strRevYear);  
            //result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}' and fees_code = '04'", m_strRevYear);

            // RMC 20150806 merged exemption by brgy from Mati (s)
            // lock to garbage only
            result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}' and fees_desc like '%GARBAGE%'", m_strRevYear);
            // RMC 20150806 merged exemption by brgy from Mati (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgvFees.Rows.Add(result.GetString("fees_desc"), result.GetString("fees_code"));
                }
            }
            result.Close();
        }

        private void LoadBrgy()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from brgy";
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgvBrgy.Rows.Add(0, result.GetString("brgy_nm"), result.GetString("brgy_code"));
                }
            }
            result.Close();
        }

        private void ResetBrgyValue()
        {
            for (int i = 0; i < dgvBrgy.RowCount; i++)
                dgvBrgy.Rows[i].Cells[0].Value = false;
        }

        private void UpdateBrgyValue()
        {
            ResetBrgyValue();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from tax_and_fees_exempted_brgy where fees_code = '" + dgvFees.CurrentRow.Cells[1].Value.ToString() + "' and rev_year = '" + m_strRevYear + "'";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    for (int i = 0; i < dgvBrgy.RowCount; i++)
                    {
                        if (pSet.GetString(1).Trim() == dgvBrgy.Rows[i].Cells[2].Value.ToString())
                        {
                            dgvBrgy.Rows[i].Cells[0].Value = true;
                            break;
                        }
                    }
                }
            }
            pSet.Close();
        }

        private void SelectCurrentValue()
        {
            for (int i = 0; i != dgvFees.RowCount; i++)
            {
                if (dgvFees.Rows[i].Cells[1].Value.ToString() == m_strTempSelectedValue)
                {
                    dgvFees.Rows[i].Selected = true;
                    dgvFees.CurrentCell = dgvFees.Rows[i].Cells[0];
                    dgvFees_SelectionChanged(dgvFees, new DataGridViewCellEventArgs(1, i));
                    break;
                }
            }
        }

        private void GetRevYear()
        {
            OracleResultSet result = new OracleResultSet();
            m_strRevYear = "";

            result.Query = "select * from config where code = '07'";
            if (result.Execute())
            {
                if (result.Read())
                    m_strRevYear = result.GetString("object").Trim();
            }
            result.Close();

        }

        private void cmbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dgvFees_SelectionChanged(object sender, EventArgs e)
        {
            UpdateBrgyValue();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Cancel")
            {
                btnEdit.Text = "Edit";
                btnClose.Text = "Close";
                dgvFees.Enabled = true;
                dgvBrgy.ReadOnly = true;
                SelectCurrentValue();
            }
            else
            {
                this.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit")
            {
                btnEdit.Text = "Update";
                btnClose.Text = "Cancel";
                dgvBrgy.ReadOnly = false;
                dgvFees.Enabled = false;
                m_strTempSelectedValue = dgvFees.CurrentRow.Cells[1].Value.ToString();
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to Update \n" + dgvFees.CurrentRow.Cells[0].Value.ToString() + "?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet pSet = new OracleResultSet();

                    pSet.Query = "Delete from tax_and_fees_exempted_brgy where fees_code = '" + dgvFees.CurrentRow.Cells[1].Value.ToString() + "' and rev_year = '" + m_strRevYear + "'";
                    pSet.ExecuteNonQuery();

                    for (int i = 0; i < dgvBrgy.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dgvBrgy.Rows[i].Cells[0].Value) == 1)
                        {
                            pSet.Query = "insert into tax_and_fees_exempted_brgy values (:1,:2,:3)";
                            pSet.AddParameter(":1", dgvFees.CurrentRow.Cells[1].Value.ToString());
                            pSet.AddParameter(":2", dgvBrgy.Rows[i].Cells[2].Value.ToString());
                            pSet.AddParameter(":3", m_strRevYear);
                            pSet.ExecuteNonQuery();
                        }
                    }

                    if (AuditTrail.InsertTrail("MULTI-CODE", "tax_and_fees_exempted_brgy", "EXEMPTION FEE BY BRGY") == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Update Successful!", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnClose.PerformClick();
                }
            }
        }

    }
}