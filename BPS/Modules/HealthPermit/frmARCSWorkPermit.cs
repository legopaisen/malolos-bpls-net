using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmARCSPermits : Form
    {
        public frmARCSPermits()
        {
            InitializeComponent();
        }

        private void dtgRecord_SelectionChanged(object sender, EventArgs e)
        {
            lblORNValue.Text = dtgRecord.CurrentRow.Cells[0].Value.ToString();
            lblORDValue.Text = dtgRecord.CurrentRow.Cells[1].Value.ToString();
            lblFeesDValue.Text = dtgRecord.CurrentRow.Cells[2].Value.ToString();
        }

        private void frmARCSWorkPermit_Load(object sender, EventArgs e)
        {
            LoadFees();
        }

        private void LoadFees()
        {
            dtgRecord.Rows.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.CreateNewConnectionARCS();
            pSet.Query = "Select Pi.or_no,Pi.or_date,S.Fees_Desc From Payments_Info Pi Left Join Subcategories S On S.Fees_Code = Pi.Fees_Code Where (S.Fees_Code Like '34%' or  S.Fees_Code Like '19%') and or_no like '" + txtFilter.Text.Trim() + "%' Order By PI.Or_date desc"; //JHB 20210126 replace or_no with or_date
            if (pSet.Execute())
                while (pSet.Read())
                    dtgRecord.Rows.Add(pSet.GetString(0), pSet.GetDateTime(1).ToString("MM/dd/yyyy"), pSet.GetString(2));
            pSet.Close();

            if (dtgRecord.RowCount <= 0)
            {
                lblFeesDValue.Text = "";
                lblORDValue.Text = "";
                lblORNValue.Text = "";
            }
        }

        private void btnHealth_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABHC"))  //MCR 20150325 reused for arcs permit
            {
                frmHealthPermit frmHealthPermit = new frmHealthPermit();
                frmHealthPermit.m_sOrNo = lblORNValue.Text.Trim();
                frmHealthPermit.m_sOrDate = lblORDValue.Text.Trim();
                frmHealthPermit.PermitType = "ARCS";
                frmHealthPermit.ShowDialog();
            }
        }

        private void btnWorkPermit_Click(object sender, EventArgs e)
        {
            if (!ValidateHealth())
            {
                MessageBox.Show("Create Health Permit First!", "ARCS - Permits", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AppSettingsManager.Granted("ABHC")) //MCR 20150325 reused for arcs permit
            {
                frmWorking frmWorking = new frmWorking();
                frmWorking.m_sOrNo = lblORNValue.Text.Trim();
                frmWorking.m_sOrDate = lblORDValue.Text.Trim();
                frmWorking.PermitType = "ARCS";
                frmWorking.ShowDialog();
            }
        }

        private bool ValidateHealth()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * From emp_names where bin = '" + lblORNValue.Text.Trim() + "'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    pSet.Close();
                    return true;
                }
            pSet.Close();

            return false;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            LoadFees();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}