using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.Utilities
{
    public partial class frmDefaultValuesSet : Form
    {
        DataTable dataMainBns = new DataTable("MainBns");
        DataTable dataFees = new DataTable("Fees"); 

        private string m_strRevYear = string.Empty;

        public frmDefaultValuesSet()
        {
            InitializeComponent();
        }

        private void frmDefaultValuesSet_Load(object sender, EventArgs e)
        {
            this.GetRevYear();
            this.LoadMainBnsType();
            this.LoadFees();
        }

        private void LoadMainBnsType()
        {
            OracleResultSet result = new OracleResultSet();
            dataMainBns.Columns.Clear();
            dataMainBns.Columns.Add("Buss Code", typeof(String));
            dataMainBns.Columns.Add("Buss Desc", typeof(String));

            result.Query = string.Format("select bns_desc,bns_code from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '{0}' order by bns_code", m_strRevYear);
            if (result.Execute())
            {
                while (result.Read())
                {
                    dataMainBns.Rows.Add(new String[] { result.GetString("bns_code"), result.GetString("bns_desc") });
                }
            }
            result.Close();

            cmbMainBnsType.DataSource = dataMainBns;
            cmbMainBnsType.DisplayMember = "Buss Desc";
            cmbMainBnsType.ValueMember = "Buss Desc";
            cmbMainBnsType.SelectedIndex = 0;
        }

        private void LoadFees()
        {
            OracleResultSet result = new OracleResultSet();

            dataFees.Columns.Clear();
            dataFees.Columns.Add("Fees Code", typeof(String));
            dataFees.Columns.Add("Fees Desc", typeof(String));

            result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}' order by fees_code", m_strRevYear);
            if (result.Execute())
            {
                while (result.Read())
                {
                    dataFees.Rows.Add(new String[] { result.GetString("fees_code"), result.GetString("fees_desc") });
                }

            }
            result.Close();

            cmbFees.DataSource = dataFees;
            cmbFees.DisplayMember = "Fees Desc";
            cmbFees.ValueMember = "Fees Desc";
            cmbFees.SelectedIndex = 0;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            string strBnsCode = string.Empty;
            int intCount = 0;

            strBnsCode = txtBnsCode.Text.ToString().Trim();
            strBnsCode += txtBnsCode2.Text.ToString().Trim();

            result.Query = string.Format("select count(*) from bns_table where fees_code = 'B' "
                + "and bns_code like '{0}%%' and rev_year = '{1}'", strBnsCode, m_strRevYear);

            int.TryParse(result.ExecuteScalar(), out intCount);

            if (intCount == 0)
            {
                MessageBox.Show("Record not found.", "Default Values", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                result.Query = "";

                return;
            }

            using (frmDefaultValues frmDefaultValues = new frmDefaultValues())
            {
                frmDefaultValues.RevYear = m_strRevYear;
                frmDefaultValues.BnsCode = strBnsCode;
                frmDefaultValues.FeesCode = ((DataRowView)this.cmbFees.SelectedItem)["Fees Code"].ToString();
                frmDefaultValues.Text = "Default Value: " + cmbFees.Text.ToString();
                frmDefaultValues.ShowDialog();
            }
        }

        private void btnResetDefault_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            OracleResultSet result4 = new OracleResultSet();
            OracleResultSet result5 = new OracleResultSet();

            string strFeesCode = string.Empty;
            string strCode = string.Empty;
            string strBnsCode = string.Empty;

            if (MessageBox.Show("Initialize Default Values?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //result.Query = "delete from default_table";
                result.Query = "delete from default_table where rev_year = '" + m_strRevYear + "'";  // RMC 20180130 correction in set default initializing values
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = string.Format("select fees_code from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}'", m_strRevYear);
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        strFeesCode = result.GetString("fees_code");

                        result2.Query = string.Format("select * from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '{0}' order by bns_code", m_strRevYear);
                        if (result2.Execute())
                        {
                            while (result2.Read())
                            {
                                strCode = result2.GetString("bns_code");

                                StringBuilder strQuery = new StringBuilder();

                                result3.Query = string.Format("select * from bns_table where fees_code = 'B' and trim(bns_code) like '{0}%%' and rev_year = '{1}' order by bns_code", strCode, m_strRevYear);
                                if (result3.Execute())
                                {
                                    if (result3.Read())
                                    {
                                        strQuery.Append(string.Format("select * from bns_table where fees_code = 'B' and trim(bns_code) like '{0}%%' and rev_year = '{1}' order by bns_code", strCode, m_strRevYear));
                                    }
                                    else
                                    {

                                        strQuery.Append(string.Format("select * from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and trim(bns_code) like '{0}%%' and rev_year = '{1}' order by bns_code", strCode, m_strRevYear));
                                    }
                                }
                                result3.Close();

                                result3.Query = strQuery.ToString();
                                if (result3.Execute())
                                {
                                    while (result3.Read())
                                    {
                                        strBnsCode = result3.GetString("bns_code");

                                        int intCount = 0;
                                        string strDefaultFee = string.Empty;
                                        string strIsDefault = string.Empty;

                                        result4.Query = string.Format("select count(*) from bns_table where fees_code = '{0}' and trim(bns_code) like '{1}%%' and rev_year = '{2}' order by fees_code,bns_code", strFeesCode, strCode, m_strRevYear);
                                        int.TryParse(result4.ExecuteScalar(), out intCount);

                                        result4.Query = string.Format("select * from bns_table where fees_code = '{0}' and trim(bns_code) like '{1}%%' and rev_year = '{2}' order by fees_code,bns_code", strFeesCode, strCode, m_strRevYear);
                                        if (result4.Execute())
                                        {
                                            while (result4.Read())
                                            {
                                                strDefaultFee = result4.GetString("bns_code");

                                                if (intCount == 1)
                                                    strIsDefault = "Y";
                                                else
                                                    strIsDefault = "N";

                                                result5.Query = "Insert into default_table (fees_code, bns_code, default_fee, stat_cover, rev_year, remarks, is_default) values (:1,:2,:3,:4,:5,:6,:7)";
                                                result5.AddParameter(":1", strFeesCode);
                                                result5.AddParameter(":2", strBnsCode);
                                                result5.AddParameter(":3", strDefaultFee);
                                                result5.AddParameter(":4", "NEW");
                                                result5.AddParameter(":5", m_strRevYear);
                                                result5.AddParameter(":6", "");
                                                result5.AddParameter(":7", strIsDefault);
                                                if (result5.ExecuteNonQuery() == 0)
                                                {
                                                }

                                                result5.Query = "Insert into default_table (fees_code, bns_code, default_fee, stat_cover, rev_year, remarks, is_default) values (:1,:2,:3,:4,:5,:6,:7)";
                                                result5.AddParameter(":1", strFeesCode);
                                                result5.AddParameter(":2", strBnsCode);
                                                result5.AddParameter(":3", strDefaultFee);
                                                result5.AddParameter(":4", "REN");
                                                result5.AddParameter(":5", m_strRevYear);
                                                result5.AddParameter(":6", "");
                                                result5.AddParameter(":7", strIsDefault);
                                                if (result5.ExecuteNonQuery() == 0)
                                                {
                                                }
                                            }
                                        }
                                        result4.Close();
                                    }
                                }
                                result3.Close();
                            }
                        }
                        result2.Close();
                    }
                }
                result.Close();

                //if (AuditTrail.InsertTrail("MULTI-CODE", "default_table", "RESET DEFAULT") == 0)
                if (AuditTrail.InsertTrail("MULTI-CODE", "default_table", "RESET DEFAULT " + m_strRevYear) == 0)    // RMC 20180130 correction in set default initializing values 
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Default values has been reset.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmbMainBnsType_SelectedValueChanged(object sender, EventArgs e)
        {
            txtBnsCode.Text = ((DataRowView)this.cmbMainBnsType.SelectedItem)["Buss Code"].ToString();
            txtBnsCode2.Text = "";
        }

        private void frmDefaultValuesSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("test");
        }
    }
}