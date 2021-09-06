using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.Utilities
{
    public partial class frmExemptedBuss : Form
    {
        private string m_strRevYear = string.Empty;

        CheckBox checkboxHeader = new CheckBox();

        public frmExemptedBuss()
        {
            InitializeComponent();
        }

        private void frmExemptedBuss_Load(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            btnCancel.Text = "Close";

            this.GetRevYear();
            this.LoadTaxFee();
            this.UpdateList();
            this.SetExempted();
        }

        private void UpdateList()
        {
            OracleResultSet result = new OracleResultSet();
            string strBnsCode = string.Empty;
            string strBnsDesc = string.Empty;
            int intRow = 0;

            dgvBussList.Columns.Clear();
            dgvBussList.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvBussList.Columns.Add("BNSCODE", "Code");
            dgvBussList.Columns.Add("BNSDESC", "Business Type");
            dgvBussList.Columns[0].Width = 30;
            dgvBussList.Columns[1].Width = 40;
            dgvBussList.Columns[2].Width = 290;

            result.Query = string.Format("select bns_code,bns_desc from bns_table where fees_code = 'B' "
                + "and length(trim(bns_code)) = 2 and rev_year = '{0}' order by bns_code", m_strRevYear);
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgvBussList.Rows.Add("");
                    strBnsCode = result.GetString(0).Trim();
                    strBnsDesc = result.GetString(1).Trim();
                    dgvBussList[0, intRow].Value = false;
                    dgvBussList[1, intRow].Value = strBnsCode;
                    dgvBussList[2, intRow].Value = strBnsDesc;

                    intRow++;
                }
            }
            result.Close();
			
        }

        private void SetExempted()
        {
            OracleResultSet result = new OracleResultSet();
            string strCode = string.Empty;
            int intCtr = 0;
            
            if (txtCode.Text == "B")
                result.Query = string.Format("select bns_code from exempted_bns where fees_code = 'B'");
            else
                result.Query = string.Format("select bns_code from exempted_bns where fees_code like '{0}%%'", txtCode.Text);
            if (result.Execute())
            {
                while (result.Read())
                {
                    intCtr++;
                    strCode = StringUtilities.Left(result.GetString(0), 2);

                    for (int intRow = 0; intRow <= dgvBussList.Rows.Count - 1; intRow++)
                    {
                        if (strCode == dgvBussList[1, intRow].Value.ToString().Trim())
                            dgvBussList[0, intRow].Value = true;
                    }
                }
            }
            result.Close();

            for (int intRow = 0; intRow <= dgvBussList.Rows.Count - 1; intRow++)
            {
                if ((bool)dgvBussList[0, intRow].Value)
                {
                    this.OnChangeRowCol(intRow);
                    break;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Close")
                this.Close();
            else
            {
                if (MessageBox.Show("Cancel changes?", "Exempted Business", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int intRow = 0;
                    intRow = dgvBussList.SelectedCells[0].RowIndex;

                    this.OnChangeRowCol(intRow);
                    btnUpdate.Enabled = false;
                    btnCancel.Text = "Close";
                }
            }
        }

        private void dgvBussList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //this.ValidateBuss();

                if (e.ColumnIndex == 0)
                {
                    if ((bool)dgvBussList[0, e.RowIndex].Value)
                    {
                        dgvBussList[0, e.RowIndex].Value = false;
                    }
                    else
                    {
                        dgvBussList[0, e.RowIndex].Value = true;
                    }

                }

                this.OnChangeRowCol(e.RowIndex);
                
            }
            catch
            {
            }
        }

        private void UpdateSubCatList(string strMainBussCode)
        {
            OracleResultSet result = new OracleResultSet();
            string strBnsCode = string.Empty;
            string strBnsDesc = string.Empty;
            int intRow = 0;

            dgvSubCatList.Controls.Remove(checkboxHeader);
            this.dgvSubCatList.Columns.Clear();
            
            DataGridViewCheckBoxColumn column1 = new DataGridViewCheckBoxColumn();
            column1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            dgvSubCatList.Columns.Insert(0, column1);
            this.dgvSubCatList.Columns.Add("BNSCODE", "Code");
            this.dgvSubCatList.Columns.Add("BNSDESC", "Business Type");
            this.dgvSubCatList.Columns[0].Width = 30;
            this.dgvSubCatList.Columns[1].Width = 40;
            this.dgvSubCatList.Columns[2].Width = 290;

            // add checkbox header
            Rectangle rect = dgvSubCatList.GetCellDisplayRectangle(0, -1, true);
            // set checkbox header to center of header cell. +1 pixel to position correctly.
            rect.X = rect.Location.X + (rect.Width / 4);

            //CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.Checked = false;
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);

            dgvSubCatList.Controls.Add(checkboxHeader);
            
            result.Query = string.Format("select bns_code, bns_desc from bns_table where fees_code = 'B' "
                + "and bns_code <> '{0}' and bns_code like '{1}%' order by bns_code", strMainBussCode, strMainBussCode);
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgvSubCatList.Rows.Add("");
                    strBnsCode = result.GetString(0).Trim();
                    strBnsDesc = result.GetString(1).Trim();

                    dgvSubCatList[0, intRow].Value = false;
                    dgvSubCatList[1, intRow].Value = strBnsCode;
                    dgvSubCatList[2, intRow].Value = strBnsDesc;
                    intRow++;
                }
            }
            result.Close();
        }

        private void SetExemptedSubCat(string strMainBussCode)
        {
            OracleResultSet result = new OracleResultSet();
            string strBnsCode = string.Empty;

            result.Query = string.Format("select bns_code from exempted_bns where bns_code like '{0}%%' "
                + "and fees_code = '{1}'", strMainBussCode, txtCode.Text);
            if (result.Execute())
            {
                while(result.Read())
                {
                    strBnsCode = result.GetString("bns_code").Trim();

                    for (int intRow = 0; intRow <= dgvSubCatList.Rows.Count - 1; intRow++)
                    {
                        if (strBnsCode == dgvSubCatList[1, intRow].Value.ToString().Trim())
                            dgvSubCatList[0, intRow].Value = true;
                    }
                    
                }
            }
            result.Close();
        }

        private void dgvBussList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.OnChangeRowCol(e.RowIndex);
        }

        private void OnChangeRowCol(int intRowIndex)
        {
            try
            {
                if ((bool)dgvBussList[0, intRowIndex].Value)
                {
                    btnUpdate.Enabled = true;
                    btnCancel.Text = "Cancel";
                    txtSubTaxFee.Text = dgvBussList[2, intRowIndex].Value.ToString().Trim();
                    this.UpdateSubCatList(dgvBussList[1, intRowIndex].Value.ToString().Trim());
                    this.SetExemptedSubCat(dgvBussList[1, intRowIndex].Value.ToString().Trim());
                }
                else
                {
                    btnUpdate.Enabled = false;
                    btnCancel.Text = "Close";
                    txtSubTaxFee.Text = "";
                    dgvSubCatList.Controls.Remove(checkboxHeader);
                    this.dgvSubCatList.Columns.Clear();
                }
            }
            catch
            {
            }
        }


        private void ValidateBuss()
        {
            OracleResultSet result = new OracleResultSet();
            string strBnsCode = string.Empty;

            for (int i = 0; i <= dgvBussList.Rows.Count - 1; i++)
            {
                if ((bool)dgvBussList[0, i].Value)
                {
                    strBnsCode = dgvBussList[1, i].Value.ToString();

                    result.Query = string.Format("select bns_code from exempted_bns where bns_code like '{0}%%' "
                        + "and fees_code = '{1}'", strBnsCode, txtCode.Text);
                    if (result.Execute())
                    {
                        if (result.Read())
                            dgvBussList[0, i].Value = true;
                        else
                            dgvBussList[0, i].Value = false;
                    }
                    result.Close();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string strMainBussCode = string.Empty;
            string strCode = string.Empty;
            int intRow = 0;
            intRow = dgvBussList.SelectedCells[0].RowIndex;

            strMainBussCode = dgvBussList[1, intRow].Value.ToString().Trim();

            string strMsgBox = string.Empty;

            strMsgBox = cmbBussType.Text.ToString();
            strMsgBox += ": " + txtSubTaxFee.Text.ToString();

            if (MessageBox.Show("Save changes in Exempted Business schedule of \n" + strMsgBox + "?", "Exempted Business", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
	        {
	    		result.Query = string.Format("delete from exempted_bns where fees_code = '{0}' "
                    + "and bns_code like '{1}%%'",txtCode.Text, strMainBussCode);
                if (result.ExecuteNonQuery() == 0)
                {
                }
		
                for(int i = 0; i <= dgvSubCatList.Rows.Count - 1; i++)
		    	{
			        if ((bool) dgvSubCatList[0,i].Value)
			        {
				        strCode = dgvSubCatList[1,i].Value.ToString().Trim();
      				
                        result2.Query = "insert into exempted_bns (fees_code, bns_code, rev_year) values (:1,:2,:3)";
                        result2.AddParameter(":1", txtCode.Text.ToString().Trim());
                        result2.AddParameter(":2", strCode);
                        result2.AddParameter(":3", m_strRevYear);
				        if(result2.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                }

                MessageBox.Show("Exempted Business schedule of \n" + strMsgBox + "\nhas been saved.", "Exempted Business", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.UpdateList();
                this.SetExempted();
	        }
	        else
	        {
                this.SetExemptedSubCat(strMainBussCode);
	        }
        }

        private void dgvSubCatList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    if ((bool)dgvSubCatList[0, e.RowIndex].Value)
                    {
                        dgvSubCatList[0, e.RowIndex].Value = false;
                    }
                    else
                    {
                        dgvSubCatList[0, e.RowIndex].Value = true;
                        btnUpdate.Enabled = true;
                        btnCancel.Text = "Cancel";
                    }

                }

            }
            catch
            {
            }
        }
                
        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSubCatList.RowCount; i++)
            {
                dgvSubCatList[0, i].Value = ((CheckBox)dgvSubCatList.Controls.Find("checkboxHeader", true)[0]).Checked;
            }
            dgvSubCatList.EndEdit();
        }

        private void LoadTaxFee()
        {
            OracleResultSet result = new OracleResultSet();
            int intCount = 0;

            DataTable dataTable = new DataTable("TaxFee");

            dataTable.Columns.Clear();
            dataTable.Columns.Add("Buss Code", typeof(String));
            dataTable.Columns.Add("Buss Desc", typeof(String));

            dataTable.Rows.Add(new String[] {"B","BUSINESS TAX" });

            result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}' order by fees_code", m_strRevYear);
            if (result.Execute())
            {
                while (result.Read())
                {
                    dataTable.Rows.Add(new String[] { result.GetString("fees_code"), result.GetString("fees_desc") });
                }
            }
            result.Close();

            cmbBussType.DataSource = dataTable;
            cmbBussType.DisplayMember = "Buss Desc";
            cmbBussType.ValueMember = "Buss Desc";
            cmbBussType.SelectedIndex = 0;
            
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

        private void cmbBussType_SelectedValueChanged(object sender, EventArgs e)
        {
            txtCode.Text = ((DataRowView)this.cmbBussType.SelectedItem)["Buss Code"].ToString();

            dgvSubCatList.Controls.Remove(checkboxHeader);
            this.dgvSubCatList.Columns.Clear();

            this.UpdateList();
            this.SetExempted();           
        }
    }
}