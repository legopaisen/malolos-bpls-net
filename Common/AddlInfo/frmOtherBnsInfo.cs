
// RMC 20140104 Capturing of gender of employee


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Common.AddlInfo
{
    public partial class frmOtherBnsInfo : Form
    {
        private string m_sBIN = string.Empty;

        public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; }
        }

        public frmOtherBnsInfo()
        {
            InitializeComponent();
        }

        private void frmOtherBnsInfo_Load(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from addl_info_tbl order by addl_code";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbDesc.Items.Add(pSet.GetString("addl_desc"));
                }
            }
            pSet.Close();
                        
            UpdateList();
            cmbDesc.Enabled = false;
        }
                
        private void UpdateList()
        {
            OracleResultSet pSet = new OracleResultSet();

            dgvList.Rows.Clear();
            dgvList.Columns.Clear();

            dgvList.Columns.Add("Code", "Code");
            dgvList.Columns.Add("Desc", "Description");
            dgvList.Columns.Add("Value", "Value");

            pSet.Query = "select * from addl_info_tmp where bin = '" + m_sBIN + "' order by addl_code";
            if (pSet.Execute())
            {
                int iCnt = 0;
                while (pSet.Read())
                {
                    dgvList.Rows.Add("");
                    dgvList[0, iCnt].Value = pSet.GetString("addl_code");
                    dgvList[1, iCnt].Value = GetAddlDesc(pSet.GetString("addl_code"));
                    dgvList[2, iCnt].Value = pSet.GetString("value");
                    iCnt++;
                }
            }
            pSet.Close();

        }

        private void cmbDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from addl_info_tbl where addl_desc = '" + cmbDesc.Text + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                    txtCode.Text = pSet.GetString("addl_code");
            }
            pSet.Close();

        }

        private string GetAddlDesc(string sAddlCode)
        {
            string sAddlDesc = string.Empty;

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from addl_info_tbl where addl_code = '" + sAddlCode + "'";
            if(pSet.Execute())
            {
                if(pSet.Read())
                    sAddlDesc = pSet.GetString("addl_desc");
            }
            pSet.Close();

            return sAddlDesc;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();

            if (btnAdd.Text == "Add")
            {
                cmbDesc.Enabled = true;
                txtValue.ReadOnly = false;
                btnAdd.Text = "Save";
                txtValue.Text = "";
                btnDelete.Enabled = false;
            }
            else
            {
                if (txtValue.Text.Trim() == "")
                {
                    MessageBox.Show("Complete information before saving","Other Business Information",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return;
                }
                if (txtCode.Text == "")
                {
                    MessageBox.Show("Complete information before saving", "Other Business Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                pCmd.Query = "select * from addl_info_tmp where bin = '" + m_sBIN + "'";
                pCmd.Query += " and addl_code = '" + txtCode.Text.ToString() + "'";
                if (pCmd.Execute())
                {
                    if (pCmd.Read())
                    {
                        MessageBox.Show("" + cmbDesc.Text.ToString() + " already added", "Other Business Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        pCmd.Close();
                        pCmd.Query = "insert into addl_info_tmp values (";
                        pCmd.Query += "'" + m_sBIN + "', ";
                        pCmd.Query += "'" + txtCode.Text.ToString() + "', ";
                        pCmd.Query += "'" + txtValue.Text.ToString() + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                    }
                }
                pCmd.Close();
                
                UpdateList();

                btnAdd.Text = "Add";
                cmbDesc.Enabled = false;
                txtValue.ReadOnly = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtCode.Text = dgvList[0, e.RowIndex].Value.ToString();
                cmbDesc.Text = dgvList[1, e.RowIndex].Value.ToString();
                txtValue.Text = dgvList[2, e.RowIndex].Value.ToString();

                if (txtCode.Text != "" && btnAdd.Text == "Add")
                    btnDelete.Enabled = true;
            }
            catch { }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();

            if (txtCode.Text != "" && txtValue.Text != "")
            {
                if (MessageBox.Show("Delete '" + cmbDesc.Text + "'?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pCmd.Query = "delete from addl_info_tmp where bin = '" + m_sBIN + "'";
                    pCmd.Query += " and addl_code = '" + txtCode.Text.ToString() + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    UpdateList();
                }
            }
        }
    }
}