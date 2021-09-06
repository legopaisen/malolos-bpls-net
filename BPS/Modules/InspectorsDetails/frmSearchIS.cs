using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmSearchIS : Form
    {
        private string m_sISNum = string.Empty;

        public string ISNum
        {
            get { return m_sISNum; }
            set { m_sISNum = value; }
        }

        public frmSearchIS()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sOwnName = string.Empty;
            string sBnsName = string.Empty;
            
            dgvResult.Columns.Clear();
            dgvResult.Columns.Add("IS", "IS#");
            dgvResult.Columns.Add("NAME", "Business Name");
            dgvResult.Columns.Add("LOC", "Location");
            dgvResult.Columns.Add("OWNER", "Owner's Name");
            dgvResult.RowHeadersVisible = false;
            dgvResult.Columns[0].Width = 80;
            dgvResult.Columns[1].Width = 150;
            dgvResult.Columns[2].Width = 150;
            dgvResult.Columns[3].Width = 150;
            dgvResult.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvResult.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvResult.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvResult.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            sOwnName = txtName.Text.Trim() + "%%";
            sBnsName = txtBnsName.Text.Trim() + "%%";

            pSet.Query = "select a.is_number, a.bns_nm, a.bns_house_no || ' ' || a.bns_street || ' ' ";
            pSet.Query += "|| a.bns_brgy || ' ' || a.bns_dist || ' ' || a.bns_mun, b.own_ln || ' ' || b.own_fn || ' ' || b.own_mi ";
            pSet.Query += "from unofficial_info_tbl a, own_names b where trim(a.own_code) = trim(b.own_code) ";
            pSet.Query += " and trim(a.bns_nm) like '" + sBnsName + "' ";
            pSet.Query += " and trim(b.own_ln) like '" + sOwnName + "' ";
            pSet.Query += " order by a.bns_nm";
            if (pSet.Execute())
            {
                int iRow = 0;

                while (pSet.Read())
                {
                    dgvResult.Rows.Add("");
                    dgvResult[0, iRow].Value = pSet.GetString(0).Trim();
                    dgvResult[1, iRow].Value = pSet.GetString(1).Trim();
                    dgvResult[2, iRow].Value = pSet.GetString(2).Trim();
                    dgvResult[3, iRow].Value = pSet.GetString(3).Trim();
                    iRow++;
                }
            }
            pSet.Close();
        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_sISNum = dgvResult[0, e.RowIndex].Value.ToString().Trim();
            txtBnsName.Text = dgvResult[1, e.RowIndex].Value.ToString().Trim();
            txtBnsAdd.Text = dgvResult[2, e.RowIndex].Value.ToString().Trim();
            txtName.Text = dgvResult[3, e.RowIndex].Value.ToString().Trim();
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            m_sISNum = "";
            txtBnsName.Text = "";
            txtBnsAdd.Text = "";
            txtName.Text = "";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_sISNum = "";
            this.Close();
        }

        
    }
}