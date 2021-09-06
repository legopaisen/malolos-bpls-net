using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using ComponentFactory.Krypton.Toolkit;

namespace Amellar.Modules.Utilities
{
    public partial class frmDefaultValues : Form
    {
        DataTable dataTable = new DataTable("Businesses");        
        
        private string m_strMainBnsCode = string.Empty;
        private string m_strFeesCode = string.Empty;
        private string m_strRevYear = string.Empty;
        bool m_blnChangeCombo = false;

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

        public string BnsCode
        {
            get { return m_strMainBnsCode; }
            set { m_strMainBnsCode = value; }
        }

        public frmDefaultValues()
        {
            InitializeComponent();
        }

        private void frmDefaultValues_Load(object sender, EventArgs e)
        {
            this.LoadBnsTypeCombo();           
            this.SetTable(4);
            this.SetTable(0);
            rdoNew.Checked = true;

            m_blnChangeCombo = true;
            UpdateListDefaultValues();
        }

        private void UpdateListDefaultValues()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();

            string strBnsCode = string.Empty;
            string strBnsDesc = string.Empty;
            string strBnsTypeCode = string.Empty;
            string strStatCover = string.Empty;
            int intRow = 0;

            dgvBnsFee.Columns.Clear();
            //dgvBnsFee.Columns.Add(new DataGridViewCheckBoxColumn());
            //dgvBnsFee.Columns.Add(new DataGridViewCheckBoxColumn());

            //btnAdd.Enabled = false;
            btnClose.Text = "Close";

            DataGridViewCheckBoxColumn column1 = new DataGridViewCheckBoxColumn();
            {
                column1.HeaderText = "X";
            }

            DataGridViewCheckBoxColumn column2 = new DataGridViewCheckBoxColumn();
            {
                column2.HeaderText = "Y";
            }

            dgvBnsFee.Columns.Insert(0, column1);
            dgvBnsFee.Columns.Insert(1, column2);
            dgvBnsFee.Columns.Add("BNSCODE", "Code");
            dgvBnsFee.Columns.Add("BNSDESC", "Business Description");
            dgvBnsFee.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBnsFee.RowHeadersVisible = false;
            dgvBnsFee.Columns[0].Width = 30;
            dgvBnsFee.Columns[1].Width = 30;
            dgvBnsFee.Columns[2].Width = 70;
            dgvBnsFee.Columns[3].Width = 300;

            strBnsCode = StringUtilities.Left(m_strMainBnsCode, 2);

            try
            {
                result.Query = string.Format("select bns_code,bns_desc from bns_table where fees_code = '{0}'"
                    + " and bns_code like '{1}%%' and rev_year = '{2}' order by bns_code", m_strFeesCode, strBnsCode, m_strRevYear);
                if (result.Execute())
                {
                    strBnsTypeCode = txtBnsTypeCode.Text.Trim();

                    if (rdoNew.Checked)
                        strStatCover = "NEW";
                    else if (rdoRenewal.Checked)
                        strStatCover = "REN";
                    else
                        strStatCover = "RET";

                    while (result.Read())
                    {
                        strBnsCode = result.GetString("bns_code");
                        strBnsDesc = result.GetString("bns_desc");

                        dgvBnsFee.Rows.Add("");
                        dgvBnsFee[2, intRow].Value = strBnsCode;
                        dgvBnsFee[3, intRow].Value = StringUtilities.RemoveApostrophe(strBnsDesc);

                        result2.Query = string.Format("select is_default from tmp_default where fees_code = '{0}' and bns_code = '{1}' "
                            + "and default_fee = '{2}' and rev_year = '{3}' and stat_cover = '{4}'", m_strFeesCode, strBnsTypeCode, strBnsCode, m_strRevYear, strStatCover);
                        if (result2.Execute())
                        {
                            string strCheck = string.Empty;

                            if (result2.Read())
                            {
                                strCheck = result2.GetString("is_default");

                                if (strCheck == "Y")
                                {
                                    dgvBnsFee[0, intRow].Value = true;
                                    dgvBnsFee[1, intRow].Value = true;
                                }
                                else
                                {
                                    dgvBnsFee[0, intRow].Value = true;
                                    dgvBnsFee[1, intRow].Value = false;

                                }

                                //btnAdd.Enabled = true;
                                //btnClose.Text = "Cancel";
                            }
                            else
                            {
                                dgvBnsFee[0, intRow].Value = false;
                                dgvBnsFee[1, intRow].Value = false;

                            }
                        }
                        result2.Close();

                        intRow++;
                    }
                }
                result.Close();
            }
            catch
            {
                MessageBox.Show("error in updatelistdefaultvalues");
            }
             
        }

        private void LoadBnsTypeCombo()
        {
            OracleResultSet result = new OracleResultSet();
            int intCount = 0;

            dataTable.Columns.Clear();
            dataTable.Columns.Add("Buss Code", typeof(String));
            dataTable.Columns.Add("Buss Desc", typeof(String));
                        		
            result.Query = string.Format("select count(*) from bns_table where fees_code = 'B' "
                + "and bns_code like '{0}%%' and rev_year = '{1}'", m_strMainBnsCode, m_strRevYear);

            int.TryParse(result.ExecuteScalar(), out intCount);

            if (intCount == 0)
            {
                MessageBox.Show("Record not found.", "Default Values", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                result.Query = "";
            }
            else if (intCount > 1)
            {
                result.Query = string.Format("select * from bns_table where fees_code = 'B' "
                    + "and bns_code like '{0}%%' and rev_year = '{1}' and length(bns_code) > 2 order by bns_code", m_strMainBnsCode, m_strRevYear);
            }
            else
            {
                result.Query = string.Format("select * from bns_table where fees_code = 'B' "
                    + "and bns_code like '{0}%%' and rev_year = '{1}' and length(bns_code) > 2 order by bns_code", m_strMainBnsCode, m_strRevYear);
            }
            if (result.Execute())
            {
                while (result.Read())
                {
                    dataTable.Rows.Add(new String[] { result.GetString("bns_code"), StringUtilities.RemoveApostrophe(result.GetString("bns_desc")) });
                    //cmbBnsType.Items.Add(result.GetString("bns_code"), result.GetString("bns_desc"));
                }
            }
            result.Close();

            cmbBnsType.DataSource = dataTable;
            cmbBnsType.DisplayMember = "Buss Desc";
            cmbBnsType.ValueMember = "Buss Desc";
            cmbBnsType.SelectedIndex = 0;
            
        }

        private void SetTable(int intAction)
        {
            //  0 = Create TMP records from default_table and tmp_default
            //  1 = Delete TMP records from default_table
            //	2 = Update Default Values from tmp_default to default_table
            //	3 = Set the default_table to its original data
            //	4 = Delete TMP records from tmp_default
            
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();

            string strBnsCode = string.Empty;
	        string strDefaultFee = string.Empty;
		    string strStatCover = string.Empty;
			string strIsDefault = string.Empty;

	        if(intAction == 0)
		    {
			    result.Query = string.Format("select * from default_table where fees_code = '{0}' and bns_code like '{1}%%' "
                    + "and rev_year = '{2}' order by bns_code,default_fee", m_strFeesCode, m_strMainBnsCode, m_strRevYear);
                if(result.Execute())
                {
                    while(result.Read())
                    {
                        strBnsCode    = result.GetString("bns_code");
				        strDefaultFee = result.GetString("default_fee");
					    strStatCover  = result.GetString("stat_cover");
					    strIsDefault  = result.GetString("is_default");
					
                        result2.Query = string.Format("delete from default_table where fees_code = '{0}' "
                            + "and bns_code = '{1}' and default_fee = '{2}' and stat_cover = '{3}' "
                            + "and rev_year = '{4}'",m_strFeesCode,strBnsCode,strDefaultFee,strStatCover,m_strRevYear);
                        if(result2.ExecuteNonQuery() == 0)
                        {
                        }
					
                        result2.Query = "insert into default_table (fees_code, bns_code, default_fee, stat_cover, rev_year, remarks, is_default) "
                            + " values(:1,:2,:3,:4,:5,:6,:7) ";
                        result2.AddParameter(":1",m_strFeesCode);
                        result2.AddParameter(":2",strBnsCode);
                        result2.AddParameter(":3",strDefaultFee);
                        result2.AddParameter(":4",strStatCover);
                        result2.AddParameter(":5",m_strRevYear);
                        result2.AddParameter(":6","TMP");
                        result2.AddParameter(":7",strIsDefault);
                        if(result2.ExecuteNonQuery() == 0)
                        {
                        }

					    result2.Query = string.Format("delete from tmp_default where fees_code = '{0}' and bns_code = '{1}' "
                            + "and default_fee = '{2}'  and stat_cover = '{3}' and rev_year = '{4}'",m_strFeesCode,strBnsCode,strDefaultFee,strStatCover,m_strRevYear);
					    if(result2.ExecuteNonQuery() == 0)
                        {
                        }

                        result2.Query = "insert into tmp_default (fees_code, bns_code, default_fee, stat_cover, rev_year, is_default) "
                             + " values(:1,:2,:3,:4,:5,:6) ";
                        result2.AddParameter(":1", m_strFeesCode);
                        result2.AddParameter(":2", strBnsCode);
                        result2.AddParameter(":3", strDefaultFee);
                        result2.AddParameter(":4", strStatCover);
                        result2.AddParameter(":5", m_strRevYear);
                        result2.AddParameter(":6", strIsDefault);
                        if(result2.ExecuteNonQuery() == 0)
                        {
                        }
					}
				}
                result.Close();
			}
            else if (intAction == 1)
			{
			    result.Query = "delete from default_table where remarks = 'TMP'";
			    if(result.ExecuteNonQuery() == 0)
                {
                }
			}
		    else if (intAction == 2)
		    {
                result.Query = "delete from tmp_default_bckup";
			    if(result.ExecuteNonQuery() == 0)
                {
                }

                if (AuditTrail.InsertTrail("MULTI-CODE", "tmp_default_bckup", "DEFAULT VALUES") == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
			    
                result.Query = "insert into default_table (fees_code, bns_code, default_fee, stat_cover, rev_year, is_default) "
                    + " select fees_code,bns_code,default_fee,stat_cover,rev_year,is_default from tmp_default";
                if(result.ExecuteNonQuery() == 0)
                {
                }

                if (AuditTrail.InsertTrail("MULTI-CODE", "default_table", "DEFAULT VALUES") == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
				
			    result.Query = "insert into tmp_default_bckup(fees_code,bns_code,default_fee,stat_cover,rev_year,is_default) "
                    + " select fees_code,bns_code,default_fee,stat_cover,rev_year,is_default from default_table";
			    if(result.ExecuteNonQuery() == 0)
                {
                }

                if (AuditTrail.InsertTrail("MULTI-CODE", "tmp_default_bckup", "DEFAULT VALUES") == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
			   
    		}
		    else if (intAction == 3)
		    {
			    result.Query = "update default_table set remarks = '' where remarks = 'TMP'";
                if(result.ExecuteNonQuery() == 0)
                {
                }
			}			
            else if (intAction == 4)
			{
                result.Query = "delete from tmp_default";
			    if(result.ExecuteNonQuery() == 0)
                {
                }

			    result.Query = "delete from tmp_default_bckup";
			    if(result.ExecuteNonQuery() == 0)
                {
                }

                if (AuditTrail.InsertTrail("MULTI-CODE", "tmp_default_bckup", "DEFAULT VALUES") == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
			    
                result.Query = "insert into tmp_default_bckup(fees_code,bns_code,default_fee,stat_cover,rev_year,is_default) "
                    + "select fees_code,bns_code,default_fee,stat_cover,rev_year,is_default from default_table";
			    if(result.ExecuteNonQuery() == 0)
                {
                }

                if (AuditTrail.InsertTrail("MULTI-CODE", "tmp_default_bckup", "DEFAULT VALUES") == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
			    
			}
	    }

        private void rdoNew_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNew.Checked)
            {
                rdoRenewal.Checked = false;
                rdoRetired.Checked = false;

                if(m_blnChangeCombo)
                    this.UpdateListDefaultValues();
            }
        }

        private void rdoRenewal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRenewal.Checked)
            {
                rdoNew.Checked = false;
                rdoRetired.Checked = false;
                if (m_blnChangeCombo)
                    this.UpdateListDefaultValues();
            }
        }

        private void rdoRetired_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRetired.Checked)
            {
                rdoNew.Checked = false;
                rdoRenewal.Checked = false;
                if (m_blnChangeCombo)
                    this.UpdateListDefaultValues();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("Update changes?", "Default Values", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.SetTable(2);
                this.SetTable(1);
                //btnAdd.Enabled = false;
                btnClose.Text = "Close";

                MessageBox.Show("Default values updated.", "Default Values", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.SetTable(4);
                this.SetTable(0);
                UpdateListDefaultValues();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Cancel")
            {
                if (MessageBox.Show("Cancel changes?", "Default Values", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //this.LoadBnsTypeCombo();
                    this.SetTable(4);
                    this.SetTable(0);

                    rdoNew.Checked = true;
                    //btnAdd.Enabled = false;
                    btnClose.Text = "Close";
                    //m_blnChangeCombo = true;
                }
            }
            else
            {
                this.SetTable(3);
                this.SetTable(4);
                this.SetTable(0);
                this.SetTable(3);
                this.SetTable(4);
                this.Close();
            }
        }

        private void btnCheckAllX_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dgvBnsFee.Rows.Count - 1; i++)
            {
                dgvBnsFee[0, i].Value = true;
            }

            btnClose.Text = "Cancel";
            //btnAdd.Enabled = true;

            this.InsertTempValues();
        }

        private void btnUnCheckAllX_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dgvBnsFee.Rows.Count - 1; i++)
            {
                dgvBnsFee[0, i].Value = false;
                dgvBnsFee[1, i].Value = false;
            }

            btnClose.Text = "Close";
            //btnAdd.Enabled = false;

            this.InsertTempValues();
        }

        private void btnCheckAllY_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dgvBnsFee.Rows.Count - 1; i++)
            {
                dgvBnsFee[0, i].Value = true;
                dgvBnsFee[1, i].Value = true;
            }

            btnClose.Text = "Cancel";
            //btnAdd.Enabled = true;

            this.InsertTempValues();
        }

        private void btnUnCheckAllY_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dgvBnsFee.Rows.Count - 1; i++)
            {
                dgvBnsFee[1, i].Value = false;
            }

            this.InsertTempValues();
        }

        private void cmbBnsType_SelectedValueChanged(object sender, EventArgs e)
        {
            txtBnsTypeCode.Text = ((DataRowView)this.cmbBnsType.SelectedItem)["Buss Code"].ToString().Trim();

            if (m_blnChangeCombo)
            {
                this.UpdateListDefaultValues();
            }
        }

        private void dgvBnsFee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    if ((bool)dgvBnsFee[e.ColumnIndex, e.RowIndex].Value)
                    {
                        dgvBnsFee[0, e.RowIndex].Value = false;
                        dgvBnsFee[1, e.RowIndex].Value = false;
                    }
                    else
                    {
                        dgvBnsFee[0, e.RowIndex].Value = true;
                    }

                }

                if (e.ColumnIndex == 1)
                {

                    if ((bool)dgvBnsFee[e.ColumnIndex, e.RowIndex].Value)
                    {
                        dgvBnsFee[1, e.RowIndex].Value = false;
                    }
                    else
                    {
                        dgvBnsFee[0, e.RowIndex].Value = true;
                        dgvBnsFee[1, e.RowIndex].Value = true;
                    }
                }

                if (e.ColumnIndex < 2)
                    this.InsertTempValues();
            }
            catch
            {
            }

            bool blnRowTrue = false;
            for (int i = 0; i <= dgvBnsFee.Rows.Count - 1; i++)
            {
                if ((bool)dgvBnsFee[0, i].Value)
                {
                    blnRowTrue = true;
                    break;
                }

                if ((bool)dgvBnsFee[1, i].Value)
                {
                    blnRowTrue = true;
                    break;
                }
            }

            if (blnRowTrue)
            {
                btnClose.Text = "Cancel";
                //btnAdd.Enabled = true;
            }
            else
            {
                btnClose.Text = "Close";
                //btnAdd.Enabled = false;
            }
        }

        private void btnApplyToAll_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            string strStatCover = string.Empty;
            string strBnsCode = string.Empty;
            string strDefaultFeesCode = string.Empty;

            //m_blnChangeCombo = false;

            if (rdoNew.Checked)
                strStatCover = "New";
            else if (rdoRenewal.Checked)
                strStatCover = "Renewal";
            else
                strStatCover = "Retired";

            if (MessageBox.Show("Update all " + strStatCover + " Businesses?", "Default Values", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (rdoNew.Checked)
                    strStatCover = "NEW";
                else if (rdoRenewal.Checked)
                    strStatCover = "REN";
                else
                    strStatCover = "RET";

                result.Query = string.Format("delete from tmp_default where fees_code = '{0}' and stat_cover = '{1}' and rev_year = '{2}' and bns_code like '{3}%%'", m_strFeesCode, strStatCover, m_strRevYear, m_strMainBnsCode);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                for (int iCtrFlex = 0; iCtrFlex <= dgvBnsFee.Rows.Count - 1; iCtrFlex++)
                {
                    strDefaultFeesCode = dgvBnsFee[2, iCtrFlex].Value.ToString();
                    bool blnIsDefault = (bool)dgvBnsFee[1, iCtrFlex].Value;
                    bool blnIsAvailable = (bool)dgvBnsFee[0, iCtrFlex].Value;

                    result.Query = string.Format("select * from bns_table where fees_code = 'B' "
                        + "and bns_code like '{0}%%' and rev_year = '{1}' and length(bns_code) > 2 order by bns_code", m_strMainBnsCode, m_strRevYear);
                    if(result.Execute())
                    {
                        while (result.Read())
                        {
                            strBnsCode = result.GetString("bns_code");

                            if (blnIsDefault == true)
                            {
                                result.Query = "insert into tmp_default (fees_code, bns_code, default_fee, stat_cover, rev_year, is_default) values (:1,:2,:3,:4,:5,:6) ";
                                result.AddParameter(":1", m_strFeesCode);
                                result.AddParameter(":2", strBnsCode);
                                result.AddParameter(":3", strDefaultFeesCode);
                                result.AddParameter(":4", strStatCover);
                                result.AddParameter(":5", m_strRevYear);
                                result.AddParameter(":6", "Y");
                                if (result.ExecuteNonQuery() == 0)
                                {
                                }

                            }
                            else
                            {
                                if (blnIsAvailable == true)
                                {
                                    result.Query = "insert into tmp_default (fees_code, bns_code, default_fee, stat_cover, rev_year, is_default) values (:1,:2,:3,:4,:5,:6) ";
                                    result.AddParameter(":1", m_strFeesCode);
                                    result.AddParameter(":2", strBnsCode);
                                    result.AddParameter(":3", strDefaultFeesCode);
                                    result.AddParameter(":4", strStatCover);
                                    result.AddParameter(":5", m_strRevYear);
                                    result.AddParameter(":6", "N");
                                    if (result.ExecuteNonQuery() == 0)
                                    {
                                    }

                                }
                            }
                        }
                    }
                    result.Close();
                }

            }
        }

        private void InsertTempValues()
        {
            OracleResultSet result = new OracleResultSet();
            string strDefaultFee = string.Empty;
            string strIsDefault = string.Empty;
            string strStatCover = string.Empty;
            string strBnsCode = string.Empty;

            // insert into temp tables
            for (int i = 0; i <= dgvBnsFee.Rows.Count - 1; i++)
            {
                strBnsCode = ((DataRowView)this.cmbBnsType.SelectedItem)["Buss Code"].ToString();

                strDefaultFee = dgvBnsFee[2, i].Value.ToString();

                if ((bool) dgvBnsFee[1, i].Value)
                    strIsDefault = "Y";
                else
                    strIsDefault = "N";

                if (rdoNew.Checked)
                    strStatCover = "NEW";
                else if (rdoRenewal.Checked)
                    strStatCover = "REN";
                else
                    strStatCover = "RET";
                
                result.Query = string.Format("delete from tmp_default where fees_code = '{0}' and bns_code = '{1}' "
                    + "and default_fee = '{2}' and stat_cover = '{3}' and rev_year = '{4}'", m_strFeesCode, strBnsCode, strDefaultFee, strStatCover, m_strRevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                if ((bool)dgvBnsFee[0, i].Value)
                {
                    result.Query = "insert into tmp_default (fees_code, bns_code, default_fee, stat_cover, rev_year, is_default) values (:1,:2,:3,:4,:5,:6) ";
                    result.AddParameter(":1", m_strFeesCode);
                    result.AddParameter(":2", strBnsCode);
                    result.AddParameter(":3", strDefaultFee);
                    result.AddParameter(":4", strStatCover);
                    result.AddParameter(":5", m_strRevYear);
                    result.AddParameter(":6", strIsDefault);
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }
                }
            }
        }

        
        

    }
}