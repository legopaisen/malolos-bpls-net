using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using ComponentFactory.Krypton.Toolkit;

namespace Amellar.Modules.Utilities
{
    public partial class frmRangeConfig : Form
    {
        private string m_strFeesCode = string.Empty;
        private string m_strFeesDesc = string.Empty;
        private string m_strCode = string.Empty;
        private string m_strTmpFeesDesc = string.Empty;

        public string RevYear
        {
            get { return txtRevYear.Text; }
            set { txtRevYear.Text = value; }
        }

        public string FeesCode
        {
            get { return txtFeesCode.Text; }
            set { txtFeesCode.Text = value; }
        }

        public string BnsCode
        {
            get { return txtBnsCode.Text; }
            set { txtBnsCode.Text = value; }
        }

        public string CodeType
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }

        public string FeesDesc
        {
            get { return m_strTmpFeesDesc; }
            set { m_strTmpFeesDesc = value; }
        }

        public frmRangeConfig()
        {
            InitializeComponent();
        }

        private void frmRangeConfig_Load(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            string strConfigCode = string.Empty;
            string strConfigSwitch = string.Empty;

            

            result.Query = string.Format("select * from rate_config_tbl_tmp where fees_code = '{0}' and det_buss_code = '{1}'", txtFeesCode.Text.Trim(), txtBnsCode.Text.Trim());
            if (result.Execute())
            {
                if (result.Read())
                {
                    strConfigCode = result.GetString("config_code");
                    strConfigSwitch = result.GetString("config_switch");
                }
            }
            result.Close();

            dgvFees.Enabled = true;

            if (strConfigCode == "02")
                chkTaxPaid.Checked = true;
            else if (strConfigCode == "03")
                chkTaxDue.Checked = true;
            else
                chkGross.Checked = true;
            

            if (strConfigSwitch == "1")
                chkSwitch.Checked = true;
            
        }

        private void UpdateList()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            int intRow = 0;
            string strFeesCode = string.Empty;

            dgvFees.Columns.Clear();
            dgvFees.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvFees.Columns.Add("BNSCODE", "Code");
            dgvFees.Columns.Add("BNSDESC", "Description");
            dgvFees.RowHeadersVisible = false;
            dgvFees.Columns[0].Width = 40;
            dgvFees.Columns[1].Width = 70;
            dgvFees.Columns[2].Width = 200;

            dgvFees.Rows.Add("");
            dgvFees[0, intRow].Value = false;
            dgvFees[1, intRow].Value = "00";
            dgvFees[2, intRow].Value = "BUSINESS TAX";

            result.Query = string.Format("select * from rate_config_tbl_ref_tmp where fees_code = '{0}' "
                + " and det_buss_code = '{1}' and tax_base_code = '00'", txtFeesCode.Text.Trim(), txtBnsCode.Text.Trim());
            if (result.Execute())
            {
                if (result.Read())
                {
                    dgvFees[0, intRow].Value = true;
                }
            }
            result.Close();

            result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}' order by fees_code", txtRevYear.Text.Trim());
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgvFees.Rows.Add("");
                    intRow++;
                    strFeesCode = result.GetString("fees_code");

                    dgvFees[0, intRow].Value = false;
                    dgvFees[1, intRow].Value = strFeesCode;
                    dgvFees[2, intRow].Value = result.GetString("fees_desc");

                    result2.Query = string.Format("select * from rate_config_tbl_ref_tmp where fees_code = '{0}' "
                        + " and det_buss_code = '{1}' and tax_base_code = '{2}'", txtFeesCode.Text.Trim(), txtBnsCode.Text.Trim(), strFeesCode);
                    if (result2.Execute())
                    {
                        if (result2.Read())
                        {
                            dgvFees[0, intRow].Value = true;
                        }
                    }
                    result2.Close();
                }
            }
            result.Close();
        }

        private void lblFeesCode_Click(object sender, EventArgs e)
        {

        }

        private void dgvFees_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            m_strFeesCode = "";
            m_strFeesDesc = "";

            if (dgvFees[e.ColumnIndex, e.RowIndex].Value != null)
            {
                if (e.ColumnIndex == 1)
                    m_strFeesCode = dgvFees[e.ColumnIndex, e.RowIndex].Value.ToString();

                if (e.ColumnIndex == 2)
                    m_strFeesDesc = dgvFees[e.ColumnIndex, e.RowIndex].Value.ToString();
            }

        }

        private void dgvFees_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                MessageBox.Show("Fees Code not editable.", "Range Config", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dgvFees[e.ColumnIndex, e.RowIndex].Value = m_strFeesCode;
                return;
            }

            if (e.ColumnIndex == 2)
            {
                MessageBox.Show("Fees description not editable.", "Range Config", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dgvFees[e.ColumnIndex, e.RowIndex].Value = m_strFeesDesc;
                return;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            /*
	        table: rate_config_tbl
        	
	        fees_code varchar2(2)
	        det_buss_code varchar2(4)
	        config_code varchar2(2)
		        - 01 - Based on Gross/Capital
		        - 02 - Based on Tax Paid
		        - 03 - Based on Tax Due
	        rev_year varchar2(4)
	        config_switch varchar2(1)
		        - 0 - add values
	            - 1 - which ever is higher

	        table: rate_config_tbl_ref

	        fees_code varchar2(2)
	        det_buss_code varchar2(4)
	        tax_base_code varchar2(2)
		        - tax_and_fees_table (same code)
	        rev_year varchar2(4)

	        */

            OracleResultSet result = new OracleResultSet();
            string strConfigCode = string.Empty;
            string strTmpSwitch = string.Empty;

            if (chkGross.Checked)
                strConfigCode = "01";
            else if (chkTaxPaid.Checked)
                strConfigCode = "02";
            else
                strConfigCode = "03";
            

            if(chkSwitch.Checked)
                strTmpSwitch = "1";
            else
                strTmpSwitch = "0";

            if (MessageBox.Show("Save changes?", "Range Config", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                result.Query = string.Format("delete from rate_config_tbl_tmp where fees_code = '{0}' and det_buss_code = '{1}'", txtFeesCode.Text.Trim(), txtBnsCode.Text.Trim());
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = string.Format("delete from rate_config_tbl_ref_tmp where fees_code = '{0}' and det_buss_code = '{1}'", txtFeesCode.Text.Trim(), txtBnsCode.Text.Trim());
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = "insert into rate_config_tbl_tmp (fees_code, det_buss_code, config_code, rev_year, config_switch) values (:1, :2, :3, :4, :5) ";
                result.AddParameter(":1",txtFeesCode.Text.Trim());
                result.AddParameter(":2",txtBnsCode.Text.Trim());
                result.AddParameter(":3",strConfigCode);
                result.AddParameter(":4",txtRevYear.Text.Trim());
                result.AddParameter(":5",strTmpSwitch);
                if (result.ExecuteNonQuery() == 0)
                {
                }
                
                if(chkTaxPaid.Checked || chkTaxDue.Checked)
                {
                    int intCtr = 0;

                    for (int y = 0; y <= dgvFees.Rows.Count - 1; y++)
                    {
                        if ((bool)dgvFees[0, y].Value)
                        {
                            intCtr++;
                            result.Query = "insert into rate_config_tbl_ref_tmp (fees_code, det_buss_code, tax_base_code, rev_year) values (:1,:2,:3,:4)";
                            result.AddParameter(":1",txtFeesCode.Text.Trim());
                            result.AddParameter(":2",txtBnsCode.Text.Trim());
                            result.AddParameter(":3",dgvFees[1,y].Value.ToString());
                            result.AddParameter(":4",txtRevYear.Text.Trim());
                            if (result.ExecuteNonQuery() == 0)
                            {
                            }
                        }
                    }

                    if (intCtr == 0)
                    {
                        MessageBox.Show("Check at least one fee.", "Range Config", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        result.Query = string.Format("delete from rate_config_tbl_tmp where fees_code = '{0}' and det_buss_code = '{1}'", txtFeesCode.Text.Trim(), txtBnsCode.Text.Trim());
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        result.Query = string.Format("delete from rate_config_tbl_ref_tmp where fees_code = '{0}' and det_buss_code = '{1}'", txtFeesCode.Text.Trim(), txtBnsCode.Text.Trim());
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }
                        return;
                    }
                }

            }

            this.Close();
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkGross_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.chkGross.CheckState.ToString() == "Checked")
            {
                this.chkTaxPaid.Checked = false;
                this.chkTaxDue.Checked = false;
                dgvFees.Columns.Clear();
            }
        }

        private void chkTaxPaid_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.chkTaxPaid.CheckState.ToString() == "Checked")
            {
                this.chkGross.Checked = false;
                this.chkTaxDue.Checked = false;
                this.UpdateList();
            }
        }

        private void chkTaxDue_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.chkTaxDue.CheckState.ToString() == "Checked")
            {
                this.chkTaxPaid.Checked = false;
                this.chkGross.Checked = false;
                this.UpdateList();
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            using (frmDefaultCodes frmDefaultCodes = new frmDefaultCodes())
            {
                frmDefaultCodes.FeesCode = txtFeesCode.Text.ToString();
                frmDefaultCodes.BnsCode = txtBnsCode.Text.ToString();
                frmDefaultCodes.FeesDesc = m_strTmpFeesDesc;
                frmDefaultCodes.CodeType = m_strCode;
                frmDefaultCodes.RevYear = txtRevYear.Text.ToString();
                frmDefaultCodes.Switch = 0;
                frmDefaultCodes.ShowDialog();
                
            }
        }

       
    }
}