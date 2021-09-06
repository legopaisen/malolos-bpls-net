
// RMC 20110831 adjusted column size for default code

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.Utilities
{
    public partial class frmDefaultCodes : Form
    {
        private string m_strRevYear = string.Empty;
        private string m_strFeesCode = string.Empty;
	    private string m_strDesc = string.Empty;
	    private string m_strBnsCode = string.Empty;
        private string m_strType = string.Empty;
        private string m_strDefaultCode = string.Empty;
        private int m_intSwForm;
        private int m_intRow = 0;
        private int m_intRowDefault = 0;    // RMC 20111104 Added delete button in Default codes module

        public string RevYear
        {
            get { return m_strRevYear; }
            set { m_strRevYear = value; }
        }

        public string FeesCode
        {
            get { return m_strFeesCode; }
            set { m_strFeesCode = value; }
        }

        public string FeesDesc
        {
            get { return m_strDesc; }
            set { m_strDesc = value; }
        }

        public string BnsCode
        {
            get { return m_strBnsCode; }
            set { m_strBnsCode = value; }
        }

        public int Switch
        {
            get { return m_intSwForm; }
            set { m_intSwForm = value; }
        }

        public string CodeType
        {
            get { return m_strType; }
            set { m_strType = value; }
        }

        public frmDefaultCodes()
        {
            InitializeComponent();
        }

        private void frmDefaultCodes_Load(object sender, EventArgs e)
        {
            m_intRow = 0;
            m_intRowDefault = 0;    // RMC 20111104 Added delete button in Default codes module

            this.LoadCodeCombo();
            this.LoadDefaultCodes();
            dgvDefault2.Columns[0].ReadOnly = true;         
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Cancel")
            {
                if (this.btnAdd2.Text == "Save")
                    dgvDefault2.Rows.RemoveAt(m_intRow);

                this.btnAdd2.Text = "Add";
                this.btnEdit2.Text = "Edit";
                this.btnCancel.Text = "Exit";

                this.btnAdd2.Enabled = true;
                this.btnEdit2.Enabled = true;
                this.btnDelete2.Enabled = true;
                this.dgvDefault2.ReadOnly = true;
            }
            else
                Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (this.btnSave.Text == "Save")
            {
                if(dgvDefault[0,0].Value.ToString() == "")
                {
                    MessageBox.Show("Please specify default code", "Default Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show("Save Changes?", "Default Code", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        result.Query = string.Format("delete from default_others where fees_code = '{0}' and default_fee = '{1}' and rev_year = '{2}'", m_strFeesCode, m_strBnsCode, m_strRevYear);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        result.Query = "insert into default_others (default_code, fees_code, default_fee, rev_year) values (:1,:2,:3,:4)";
                        result.AddParameter(":1", dgvDefault[0,0].Value.ToString());
                        result.AddParameter(":2", m_strFeesCode);
                        result.AddParameter(":3", m_strBnsCode);
                        result.AddParameter(":4", m_strRevYear);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                }
            }
            /*else //Delete
            {
            }*/
        }

        private void btnAdd2_Click(object sender, EventArgs e)
        {
            bool bSw = false;
            
            if(this.btnAdd2.Text == "Add")
            {
                this.GenerateDefaultCode();

                dgvDefault2.Rows.Add("");
                m_intRow = dgvDefault2.Rows.Count - 1;

                dgvDefault2[0, m_intRow].Value = m_strDefaultCode;
                dgvDefault2[1, m_intRow].Value = m_strDesc;
                dgvDefault2[2, m_intRow].Value = m_strType;

                for (int iRow = 0; iRow < m_intRow; iRow++ )
                {
                    if (m_intRow != iRow)
                    {
                        if(dgvDefault2[1,m_intRow].Value.ToString() == dgvDefault2[1,iRow].Value.ToString())
                        {
                            MessageBox.Show("Default Description already exists.", "Default Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvDefault2.Rows.RemoveAt(m_intRow);
                            return;
                        }
                    }
                }

                this.btnAdd2.Text = "Save";
                this.btnEdit2.Enabled = false;
                this.btnDelete2.Enabled = false;
                this.btnCancel.Text = "Cancel";
                this.dgvDefault2.ReadOnly = false;
            }
            else
            {
                if (dgvDefault2[0, m_intRow].Value.ToString() != "" && dgvDefault2[1, m_intRow].Value.ToString() != "")
                {
                    OracleResultSet result = new OracleResultSet();

                    string strDesc = string.Empty;
                    strDesc = dgvDefault2[1, m_intRow].Value.ToString().Trim().ToUpper();

                    result.Query = "insert into default_code (default_code, default_desc, data_type, rev_year) values (:1,:2,:3,:4)";
                    result.AddParameter(":1", dgvDefault2[0, m_intRow].Value.ToString().Trim());
                    result.AddParameter(":2", strDesc);
                    result.AddParameter(":3", dgvDefault2[2, m_intRow].Value.ToString().Trim());
                    result.AddParameter(":4", m_strRevYear);
                    if(result.ExecuteNonQuery() == 0)
                    {
                    }
                }

                this.LoadCodeCombo();
                this.LoadDefaultCodes();

                this.btnAdd2.Text = "Add";
                this.btnEdit2.Enabled = true;
                this.btnDelete2.Enabled = true;
                this.btnCancel.Text = "Exit";
                this.dgvDefault2.ReadOnly = true;
            }
        }

        private void GenerateDefaultCode()
        {
            OracleResultSet result = new OracleResultSet();
            int intCount = 0;

            result.Query = string.Format("select count(*) from default_code where rev_year = '{0}'", m_strRevYear);
            int.TryParse(result.ExecuteScalar().ToString(), out intCount);

            m_strDefaultCode = string.Format("{0:0000}", intCount+1);
            
        }

        private void LoadDefaultCodes()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();

            string strDefaultCode = string.Empty;
            string strDefaultDesc = string.Empty;
            string strType = string.Empty;
            int intRow = 0;

            dgvDefault2.Rows.Clear();
            result.Query = string.Format("select * from default_code where rev_year = '{0}' order by default_code", m_strRevYear);
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgvDefault2.Rows.Add("");
                    strDefaultCode = result.GetString("default_code").ToString();
                    strDefaultDesc = result.GetString("default_desc").ToString();
                    strType = result.GetString("data_type").ToString();

                    dgvDefault2[0, intRow].Value = strDefaultCode;
                    dgvDefault2[1, intRow].Value = strDefaultDesc;
                    dgvDefault2[2, intRow].Value = strType;

                    intRow++;
                }

                //dgvDefault.Rows.Clear();
                result2.Query = string.Format("select * from default_others where fees_code = '{0}' and default_fee = '{1}' and rev_year = '{2}'", m_strFeesCode, m_strBnsCode, m_strRevYear);

                dgvDefault.Rows.Add("");
                dgvDefault[1, 0].Value = m_strFeesCode;
                dgvDefault[2, 0].Value = m_strBnsCode;

                if (result2.Execute())
                {
                    if (result2.Read())
                    {
                        dgvDefault[0, 0].Value = result2.GetString("default_code");
                    }
                }
                result2.Close();

                if (m_intSwForm == 0)
                {
                    this.btnSave.Text = "Save";
                    this.dgvDefault.Enabled = true;
                    this.dgvDefault2.ReadOnly = false;

                }
            }
            result.Close();
        }

        private void btnEdit2_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (this.btnEdit2.Text == "Edit")
            {
                this.btnSave.Enabled = false;
                this.btnEdit2.Text = "Update";
                this.btnCancel.Text = "Cancel";
                this.btnAdd2.Enabled = false;
                this.btnDelete2.Enabled = false;
                this.dgvDefault2.ReadOnly = false;
            }
            else
            {
                result.Query = string.Format("delete from default_code where rev_year = '{0}'", m_strRevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                int intRow = dgvDefault2.Rows.Count;

                for (int x = 0; x < intRow; x++)
                {
                    if (dgvDefault2[0, x].Value.ToString() != "" && dgvDefault2[1, x].Value.ToString() != "")
                    {
                        string strDesc = string.Empty;
                        strDesc = dgvDefault2[1, x].Value.ToString().Trim().ToUpper();

                        result.Query = "insert into default_code (default_code, default_desc, data_type, rev_year) values (:1,:2,:3,:4)";
                        result.AddParameter(":1", dgvDefault2[0, x].Value.ToString().Trim());
                        result.AddParameter(":2", strDesc);
                        result.AddParameter(":3", dgvDefault2[2, x].Value.ToString().Trim());
                        result.AddParameter(":4", m_strRevYear);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                }

                this.btnSave.Enabled = true;

                this.btnSave.Enabled = true;
                this.btnEdit2.Text = "Edit";
                this.btnCancel.Text = "Exit";
                this.btnAdd2.Enabled = true;
                this.btnDelete2.Enabled = true;
                this.dgvDefault2.ReadOnly = true;

                this.LoadCodeCombo();
                this.LoadDefaultCodes();
            }
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            
            if (MessageBox.Show("All selected record(s) will be deleted. \nContinue anyway?", "Default Code", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                for (int x = m_intRow; x <= m_intRow; x++)
                {
                    string strCode = dgvDefault2[0, m_intRow].Value.ToString();

                    result.Query = string.Format("select * from default_others where default_code = '{0}' and rev_year = '{1}'", strCode, m_strRevYear);
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            MessageBox.Show("Can't delete default values with existing record", "Default Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            result.Close();

                            result.Query = string.Format("delete from default_code where default_code = '{0}' and rev_year = '{1}'", strCode, m_strRevYear);
                            if (result.ExecuteNonQuery() == 0)
                            {
                            }
                        }
                    }
                }
                this.LoadDefaultCodes();   
            }
        }

        private void dgvDefault2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_intRow = e.RowIndex;
        }

        private void LoadCodeCombo()
        {
            OracleResultSet result = new OracleResultSet();

            DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
            dgvDefault.Columns.Clear();
            dgvDefault.Columns.Add("FEESCODE", "Fees Code");
            dgvDefault.Columns.Add("BNSCODe", "Business Code");
            
            comboBox.HeaderCell.Value = "Code";
            dgvDefault.Columns.Insert(0, comboBox);
            
            comboBox.Items.Clear();

            //result.Query = "select * from default_code order by default_code"; REM 20141110
            result.Query = "select * from default_code where rev_year = '" + m_strRevYear + "' order by default_code";
            if (result.Execute())
            {
                while (result.Read())
                {
                    comboBox.Items.AddRange(result.GetString("default_code"));
                }
            }
            result.Read();


            dgvDefault.Columns[0].Width = 80;  // RMC 20110831 adjusted column size for default code
            dgvDefault.Columns[1].Width = 80;
            dgvDefault.Columns[2].Width = 120;

        }

        private void dgvDefault2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string strDesc = string.Empty;
            string strRowDesc = string.Empty;

            strDesc = dgvDefault2[1, e.RowIndex].Value.ToString().Trim();

            for (int iCtr = 0; iCtr < dgvDefault2.Rows.Count; iCtr++)
            {
                if (iCtr != e.RowIndex)
                {
                    strRowDesc = dgvDefault2[1, iCtr].Value.ToString().Trim();

                    if (strRowDesc == strDesc)
                    {
                        MessageBox.Show("Duplicate description", "Default Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvDefault2[1, iCtr].Value = m_strDesc;
                        return;

                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // RMC 20111104 Added delete button in Default codes module

            OracleResultSet result = new OracleResultSet();
            string sDefaultCode = "";
            
            try
            {
                sDefaultCode = dgvDefault[0, m_intRowDefault].Value.ToString();
            }
            catch
            {
                sDefaultCode = "";
            }

            if (sDefaultCode == "")
            {
                MessageBox.Show("Select default code to delete","Default Code",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Delete default code " + sDefaultCode + "?", "Default Code", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                result.Query = string.Format("delete from default_others where fees_code = '{0}' and default_fee = '{1}' and rev_year = '{2}' and default_code = '{3}'", m_strFeesCode, m_strBnsCode, m_strRevYear, sDefaultCode);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                this.LoadCodeCombo();
                this.LoadDefaultCodes();
                MessageBox.Show("Default code deleted","Default code",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void dgvDefault_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // RMC 20111104 Added delete button in Default codes module
            m_intRowDefault = e.RowIndex;
        }
    }
}