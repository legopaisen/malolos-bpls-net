using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.SearchUser
{
    public partial class frmSearchUser : Form
    {
        public frmSearchUser()
        {
            InitializeComponent();
        }

        private string m_sUserCode = String.Empty;

        public string UserCode
        {
            get { return m_sUserCode; }
            set { m_sUserCode = value; }
        }

        private void frmSearchUser_Load(object sender, EventArgs e)
        {
            LoadPosition();
        }

        private void LoadPosition()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select distinct usr_pos from sys_users order by usr_pos";
            if (pSet.Execute())
                while (pSet.Read())
                {
                    cmbPosition.Items.Add(StringUtilities.StringUtilities.RemoveApostrophe(pSet.GetString(0).Trim()));
                }
            pSet.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_sUserCode = "ALL";
            this.Close();
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            dgvResult.Rows.Clear();
            cmbPosition.ResetText();

            foreach (object c in this.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Text = "";
            }
        }

        private void UpdateList()
        {
            String sQuery, sLName, sFName, sMI, sPosition;

            sLName = StringUtilities.StringUtilities.HandleApostrophe(AppSettingsManager.ForSearch(txtLName.Text));
            sFName = StringUtilities.StringUtilities.HandleApostrophe(AppSettingsManager.ForSearch(txtFName.Text));
            sMI = StringUtilities.StringUtilities.HandleApostrophe(AppSettingsManager.ForSearch(txtMI.Text));
            sPosition = StringUtilities.StringUtilities.HandleApostrophe(AppSettingsManager.ForSearch(cmbPosition.Text));

            sQuery = "select usr_code,usr_ln,usr_fn,usr_mi,usr_pos ";
            sQuery += "from sys_users where nvl(trim(usr_ln),' ') like '" + sLName + "' ";
            sQuery += "and nvl(trim(usr_fn),' ') like '" + sFName + "' ";
            sQuery += "and nvl(trim(usr_mi),' ') like '" + sMI + "' ";
            sQuery += "and nvl(trim(usr_pos),' ') like '" + sPosition + "'";
            
            dgvResult.Rows.Clear();

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    dgvResult.Rows.Add(StringUtilities.StringUtilities.RemoveApostrophe(pSet.GetString(0).Trim()), StringUtilities.StringUtilities.RemoveApostrophe(pSet.GetString(1).Trim()), StringUtilities.StringUtilities.RemoveApostrophe(pSet.GetString(2).Trim()), StringUtilities.StringUtilities.RemoveApostrophe(pSet.GetString(3).Trim()), StringUtilities.StringUtilities.RemoveApostrophe(pSet.GetString(4).Trim()));
                }
            pSet.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtFName.Text == "" && txtLName.Text == "" && txtMI.Text == "" && cmbPosition.Text == "")
            {
                MessageBox.Show("No Search Criteria!", "Record Search Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                UpdateList();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtUserCode.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Select a user", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                m_sUserCode = txtUserCode.Text;
                this.Close();
            }
        }

        private void dgvResult_SelectionChanged(object sender, EventArgs e)
        {
            txtUserCode.Text = dgvResult.CurrentRow.Cells[0].Value.ToString();
            txtLName.Text = dgvResult.CurrentRow.Cells[1].Value.ToString();
            txtFName.Text = dgvResult.CurrentRow.Cells[2].Value.ToString();
            txtMI.Text = dgvResult.CurrentRow.Cells[3].Value.ToString();
            cmbPosition.Text = dgvResult.CurrentRow.Cells[4].Value.ToString();
        }
    }
}