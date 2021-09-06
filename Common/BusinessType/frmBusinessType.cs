
// RMC 20111214 added validation in search business types
// RMC 20110311

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.BusinessType
{
    public partial class frmBusinessType : Form
    {
        OracleResultSet pSet = new OracleResultSet();
        public string m_sBnsDescription;
        bool bFormStyle = false;
        public string m_strBnsCode;
        
        public frmBusinessType()
        {
            InitializeComponent();
            LoadBnsType();
            
        }

        private void dgvSubCat_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtBnsCode.Text = string.Empty;
            txtBnsDesc.Text = string.Empty;
            try
            {
                txtBnsCode.Text = dgvSubCat.SelectedRows[0].Cells[0].Value.ToString();
                txtBnsDesc.Text = dgvSubCat.SelectedRows[0].Cells[1].Value.ToString();

                
            }
            catch
            {
                txtBnsCode.Text = "";
                txtBnsDesc.Text = "";
            }
        }

        public void SetFormStyle(bool bStyle)
        {
            if(bStyle == true)
            {
                this.grpMainBnsType.Visible = true;
                this.grpCodeDesc.Visible = false;
            }
            else if(bStyle == false)
            {
                this.grpCodeDesc.Visible = true;
                this.grpMainBnsType.Visible = false;
            }

            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try //JARS 20170822
            {
                m_sBnsDescription = dgvSubCat.SelectedRows[0].Cells[1].Value.ToString();
                m_strBnsCode = dgvSubCat.SelectedRows[0].Cells[0].Value.ToString();

                // RMC 20111214 added validation in search business types (s)
                if (!ValidateBnsCode(m_strBnsCode))
                {
                    m_sBnsDescription = "";
                    m_strBnsCode = "";
                    return;
                }
                // RMC 20111214 added validation in search business types (e)
            }
            catch
            {
                m_sBnsDescription = "";
                m_strBnsCode = "";
            }
            this.Close();
        }
        public void LoadBnsType()
        {
            OracleResultSet pSet = new OracleResultSet();
            //pSet.Query = "SELECT BNS_DESC FROM BNS_TABLE WHERE FEES_CODE = 'B' AND MEANS = 'G' ORDER BY BNS_CODE ASC";

            //pSet.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '{0}' and bns_code not in (select bns_code from bns_table where (bns_code like '10%' or bns_code like '11%' or bns_code like '12%' or bns_code like '13%' or bns_code like '14%' or bns_code like '15%' ) and fees_code = 'B') order by bns_code", AppSettingsManager.GetConfigValue("07"));
            pSet.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '{0}' order by bns_code", AppSettingsManager.GetConfigValue("07"));
            if(pSet.Execute())
            {
                while(pSet.Read())
                {
                    cmbMainBnsType.Items.Add(pSet.GetString("bns_desc").Trim());
                }
            }
            pSet.Close();
        }

        private void cmbMainBnsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dgvSubCat.Columns.Clear();
            dgvSubCat.Rows.Clear();
            OracleResultSet subCat = new OracleResultSet();
            string sCode, sMainDesc;
            sMainDesc = cmbMainBnsType.Text.Trim();
            pSet.Query = "SELECT bns_code FROM BNS_TABLE WHERE TRIM(BNS_DESC) = '" + sMainDesc + "'";   // AND MEANS = 'G'";    // RMC 20110311
            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    sCode = pSet.GetString("bns_code").Trim();
                    subCat.Query = "SELECT * FROM BNS_TABLE WHERE BNS_CODE LIKE '" + sCode + "%' AND BNS_CODE <> :1 AND FEES_CODE = 'B' AND REV_YEAR = :2 order by bns_code"; //MCR 20141117 added rev_year
                    subCat.AddParameter(":1", sCode);
                    subCat.AddParameter(":2", AppSettingsManager.GetConfigValue("07"));
                    if(subCat.Execute())
                    {
                        while(subCat.Read())
                        {
                            dgvSubCat.Rows.Add(subCat.GetString("bns_code").Trim(), subCat.GetString("bns_desc").Trim());
                        }
                    }
                    subCat.Close();
                }
            }
            pSet.Close();
        }

        private void btnSearchCode_Click(object sender, EventArgs e)
        {
            if (txtBnsCode.Text.Trim() == "" && txtBnsDesc.Text.Trim() == "")
            {
                MessageBox.Show("No data to be search");
                return;
            }
            else
            {
                dgvSubCat.Rows.Clear();
                SearchCodeDesc();
            }
        }

        public void SearchCodeDesc()
        {
            string sCode, sBnsDesc;
            if(txtBnsCode.Text.Trim() == "")
                sCode = "%%";
            else
                sCode = txtBnsCode.Text.Trim();

            if(txtBnsDesc.Text.Trim() == "")
                sBnsDesc = "%%";
            else
                sBnsDesc = txtBnsDesc.Text.Trim();

            pSet.Query = "SELECT BNS_CODE, BNS_DESC FROM BNS_TABLE WHERE TRIM(BNS_CODE) LIKE '" + sCode + "%' and TRIM(BNS_DESC) LIKE '" + sBnsDesc + "%' AND TRIM(FEES_CODE) = 'B' AND LENGTH(TRIM(BNS_CODE)) > 2 order by bns_code";
            if(pSet.Execute())
            {
               // if(pSet.Read())
               // {
                    while(pSet.Read() == true)
                    {
                        dgvSubCat.Rows.Add(pSet.GetString("bns_code").Trim(), pSet.GetString("bns_desc").Trim());
                    }
               // }
            }
            pSet.Close();
        }

        public void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                m_sBnsDescription = dgvSubCat.SelectedRows[0].Cells[1].Value.ToString();
                m_strBnsCode = dgvSubCat.SelectedRows[0].Cells[0].Value.ToString();

                // RMC 20111214 added validation in search business types (s)
                if (!ValidateBnsCode(m_strBnsCode))
                {
                    m_sBnsDescription = "";
                    m_strBnsCode = "";
                    return;
                }
                // RMC 20111214 added validation in search business types (e)
            }
            catch
            {
                m_sBnsDescription = "";
                m_strBnsCode = "";
            }
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBnsDesc.Text = string.Empty;
            txtBnsCode.Text = string.Empty;
            dgvSubCat.Rows.Clear();
        }

        private void frmBusinessType_Load(object sender, EventArgs e)
        {

        }

        private void dgvSubCat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtBnsCode.Text = string.Empty;
                txtBnsDesc.Text = string.Empty;
                txtBnsCode.Text = dgvSubCat.SelectedRows[0].Cells[0].Value.ToString();
                txtBnsDesc.Text = dgvSubCat.SelectedRows[0].Cells[1].Value.ToString();

                // RMC 20111214 added validation in search business types (s)
                if(!ValidateBnsCode(txtBnsCode.Text.Trim()))
                {
                    txtBnsCode.Text = "";
                    txtBnsDesc.Text = "";
                }
                // RMC 20111214 added validation in search business types (e)
            }
            catch
            {
            }
        }

        
        private bool ValidateBnsCode(string sBnsCode)
        {
            // validate if sub bns code have 3rd level of code, 2nd level should not be selected
            // RMC 20111214 added validation in search business types

            OracleResultSet result = new OracleResultSet();

            if (sBnsCode.Trim().Length == 2) //JARS 20171027
            {
                result.Query = "select * from bns_table where fees_code = 'B' ";
                result.Query += " and bns_code like '" + sBnsCode + "%'";
                result.Query += " and length(trim(bns_code)) > 4";
                result.Query += " and rev_year = '" + ConfigurationAttributes.RevYear + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        result.Close();
                        MessageBox.Show("Please select detailed business description for code "+sBnsCode+".", "Business Types", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                    else
                    {
                        result.Close();
                        return true;
                    }
                }
            }
            return true;
        }
    }
}