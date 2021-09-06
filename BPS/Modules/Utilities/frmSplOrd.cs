
// RMC 20160109 customized special ordinance module for Tumauini

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.Utilities
{
    public partial class frmSplOrd : Form
    {
        DataTable dataTable = new DataTable("Businesses");
        DataTable dataTableFees = new DataTable("Fees");
        string m_sTaxCode = string.Empty;
        string m_sFeesCode = string.Empty;

        public frmSplOrd()
        {
            InitializeComponent();
        }

        
        private void frmSplOrd_Load(object sender, EventArgs e)
        {
            /*
         table spl_ord_tbl:
         fees_type - FR|FA
         fees_means - INC|DEC
         fees_term - A|Q|M 
         */

            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from tabs where table_name = 'SPL_ORD_TBL'";
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = "create table spl_ord_tbl(rev_year varchar2(4),eff_year varchar2(4),fees_code varchar2(10),fees_type varchar2(2),fees_means varchar2(3),fees_term varchar2(1),value_fld float,entry_date date)";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "INSERT INTO TRAIL_TABLE VALUES ('AUTSO-A','ASS-UTIL-TABLES-SPECIAL ORDINANCE-ADDED')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "INSERT INTO TRAIL_TABLE VALUES ('AUTSO-E','ASS-UTIL-TABLES-SPECIAL ORDINANCE-EDITED')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "INSERT INTO TRAIL_TABLE VALUES ('AUTSO-D','ASS-UTIL-TABLES-SPECIAL ORDINANCE-DELETED')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    
                }
            }
            pSet.Close();


            pSet.Query = "select rev_year from gr_year order by rev_year";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbRevYear.Items.Add(pSet.GetString(0));
                }
            }
            pSet.Close();

            cmbRevYear.SelectedIndex = 0;

            UpdateList();

        }

        private void cmbRevYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_sTaxCode = "";
            m_sFeesCode = "";
            LoadTax();
            LoadFees();
        }

        private void LoadTax()
        {
            OracleResultSet pSet = new OracleResultSet();

            dataTable.Columns.Clear();
            dataTable.Columns.Add("Buss Code", typeof(String));
            dataTable.Columns.Add("Buss Desc", typeof(String));

            dataTable.Rows.Add(new String[] {"00","ALL"});
            pSet.Query = "select * from bns_table where fees_code = 'B' and length(bns_code) = 2 and rev_year = '" + cmbRevYear.Text + "' order by bns_code";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dataTable.Rows.Add(new String[] { pSet.GetString("bns_code"), StringUtilities.RemoveApostrophe(pSet.GetString("bns_desc")) });
                }
            }
            pSet.Close();

            cmbTax.DataSource = dataTable;
            cmbTax.DisplayMember = "Buss Desc";
            cmbTax.ValueMember = "Buss Desc";
            cmbTax.SelectedIndex = 0;
        }

        private void cmbTax_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_sTaxCode = ((DataRowView)this.cmbTax.SelectedItem)["Buss Code"].ToString().Trim();
            }
            catch
            {
                m_sTaxCode = "";
            }
        }

        private void LoadFees()
        {
            OracleResultSet pSet = new OracleResultSet();

            dataTableFees.Columns.Clear();
            dataTableFees.Columns.Add("Fees Code", typeof(String));
            dataTableFees.Columns.Add("Fees Desc", typeof(String));

            dataTableFees.Rows.Add(new String[] { "00", "ALL" });
            pSet.Query = "select * from tax_and_fees_table where rev_year = '" + cmbRevYear.Text + "' order by fees_code";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dataTableFees.Rows.Add(new String[] { pSet.GetString("fees_code"), StringUtilities.RemoveApostrophe(pSet.GetString("fees_desc")) });
                }
            }
            pSet.Close();

            cmbFees.DataSource = dataTableFees;
            cmbFees.DisplayMember = "Fees Desc";
            cmbFees.ValueMember = "Fees Desc";
            cmbFees.SelectedIndex = 0;
        }

        private void cmbFees_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_sFeesCode = ((DataRowView)this.cmbFees.SelectedItem)["Fees Code"].ToString().Trim();
            }
            catch
            {
                m_sFeesCode = "";
            }
        }

        private bool Validation()
        {
            if (cmbRevYear.Text.Trim() == "" || txtEffYear.Text.Trim() == "")
            {
                MessageBox.Show("Revenue year and effectivity year is required.", "Special Ordinance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (chkTax.Checked && m_sTaxCode == "")
            {
                MessageBox.Show("Business line description value is required", "Special Ordinance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (chkFees.Checked && m_sFeesCode == "")
            {
                MessageBox.Show("Regulatory fee description is required", "Special Ordinance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (rdoRate.Checked == false && rdoAmt.Checked == false)
            {
                MessageBox.Show("Rate or Amount is required.", "Special Ordinance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (rdoRate.Checked && txtRate.Text.Trim() == "")
            {
                MessageBox.Show("Rate value is required.", "Special Ordinance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (rdoAmt.Checked && txtAmt.Text.Trim() == "")
            {
                MessageBox.Show("Amount value is required.", "Special Ordinance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (rdoInc.Checked == false && rdoDec.Checked == false)
            {
                MessageBox.Show("Increase or Decrease configuration is required.", "Special Ordinance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (rdoAnnual.Checked == false && rdoQuarter.Checked == false && rdoMonth.Checked == false)
            {
                MessageBox.Show("Term is required.", "Special Ordinance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            
            int iYear = 0; int iRevYear = 0;
            int.TryParse(txtEffYear.Text.Trim(), out iYear);
            int.TryParse(cmbRevYear.Text, out iRevYear);
            
            if (iYear < iRevYear)
            {
                MessageBox.Show("Invalid effectivity year.", "Special Ordinance", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private void EnableControls(bool blnEnable)
        {
            cmbRevYear.Enabled = blnEnable;
            txtEffYear.ReadOnly = !blnEnable;
            cmbTax.Enabled = blnEnable;
            cmbFees.Enabled = blnEnable;
            chkTax.Enabled = blnEnable;
            chkFees.Enabled = blnEnable;
            rdoAmt.Enabled = blnEnable;
            rdoRate.Enabled = blnEnable;
            rdoInc.Enabled = blnEnable;
            rdoDec.Enabled = blnEnable;
            rdoAnnual.Enabled = blnEnable;
            rdoQuarter.Enabled = blnEnable;
            rdoMonth.Enabled = blnEnable;
        }

        private void ClearControls()
        {
            m_sTaxCode = "";
            m_sFeesCode = "";
            txtEffYear.Text = "";
            txtRate.Text = "";
            txtAmt.Text = "";

            rdoAmt.Checked = false;
            rdoRate.Checked = false;
            rdoInc.Checked = false;
            rdoDec.Checked = false;
            rdoAnnual.Checked = false;
            rdoQuarter.Checked = false;
            rdoMonth.Checked = false;
            chkTax.Checked = false;
            chkFees.Checked = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Add")
            {
                btnAdd.Text = "Save";
                btnCancel.Text = "Cancel";
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                EnableControls(true);
                ClearControls();
                cmbRevYear.Focus();
            }
            else
            {
                if (!Validation())
                    return;

                // additional validation
                OracleResultSet pSet = new OracleResultSet();

                pSet.Query = "select * from spl_ord_tbl where rev_year = '" + cmbRevYear.Text + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Revenue year "+cmbRevYear.Text+" already tagged in special ordinance./nPlease check list.","Special Ordinance",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        pSet.Close();
                        return;
                    }
                }
                pSet.Close();


                if (MessageBox.Show("Save?", "Special Ordinance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sFeesCode = string.Empty;
                    string sFeesType = string.Empty;
                    string sFeesMeans = string.Empty;
                    string sFeesTerm = string.Empty;
                    double dValue = 0;
                    string sValue = string.Empty;

                    int iCntInsert = 0;
                    for (int i = 1; i <= 2; i++)
                    {
                        bool bInsert = false;

                        if (i == 1)
                        {
                            if (chkTax.Checked)
                            {
                                sFeesCode = "B" + m_sTaxCode;
                                bInsert = true;
                            }
                            else
                                bInsert = false;
                        }
                        else
                        {
                            if (chkFees.Checked)
                            {
                                sFeesCode = "F" + m_sFeesCode;
                                bInsert = true;
                            }
                            else
                                bInsert = false;
                        }

                        if (bInsert)
                        {
                            if (rdoRate.Checked)
                            {
                                sFeesType = "FR";
                                double.TryParse(txtRate.Text, out dValue);
                            }
                            else
                            {
                                sFeesType = "FA";
                                double.TryParse(txtAmt.Text, out dValue);
                            }
                            sValue = string.Format("{0:###.00}", dValue);

                            if (rdoInc.Checked)
                                sFeesMeans = "INC";
                            else
                                sFeesMeans = "DEC";

                            if (rdoAnnual.Checked)
                                sFeesTerm = "A";
                            else if (rdoQuarter.Checked)
                                sFeesTerm = "Q";
                            else
                                sFeesTerm = "M";

                            pSet.Query = "insert into spl_ord_tbl values (:1, :2, :3, :4, :5, :6, :7, :8)";
                            pSet.AddParameter(":1", cmbRevYear.Text);
                            pSet.AddParameter(":2", txtEffYear.Text.Trim());
                            pSet.AddParameter(":3", sFeesCode);
                            pSet.AddParameter(":4", sFeesType);
                            pSet.AddParameter(":5", sFeesMeans);
                            pSet.AddParameter(":6", sFeesTerm);
                            pSet.AddParameter(":7", sValue);
                            pSet.AddParameter(":8", AppSettingsManager.GetSystemDate());
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            string strObject = "TY:" + cmbRevYear.Text + "/EY: " + txtEffYear.Text.Trim() + "/Fee: " + sFeesCode;
                            strObject += "/Type: " + sFeesType + "/Means: " + sFeesMeans + "/Term: " + sFeesTerm + "/Value: " + sValue;

                            if (AuditTrail.InsertTrail("AUTSO-A", "spl_ord_tbl", StringUtilities.HandleApostrophe(strObject)) == 0)
                            {
                                pSet.Rollback();
                                pSet.Close();
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            iCntInsert++;
                        }
                    }
                    
                    if(iCntInsert > 0)
                        MessageBox.Show("Record saved","Special Ordinance",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    else
                        MessageBox.Show("No record saved", "Special Ordinance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                UpdateList();
                btnAdd.Text = "Add";
                btnCancel.Text = "Close";
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                EnableControls(false);
                ClearControls();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Cancel")
            {
                EnableControls(false);
                ClearControls();
                btnCancel.Text = "Close";
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }

        private void rdoRate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRate.Checked)
            {
                rdoAmt.Checked = false;
                txtRate.ReadOnly = false;
                txtRate.Focus();
            }
            else
            {
                txtRate.ReadOnly = true;
                txtRate.Text = "";
            }
        }

        private void rdoAmt_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAmt.Checked)
            {
                rdoRate.Checked = false;
                txtAmt.ReadOnly = false;
                txtAmt.Focus();
            }
            else
            {
                txtAmt.ReadOnly = true;
                txtAmt.Text = "";
            }
        }

        private void rdoInc_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoInc.Checked)
            {
                rdoDec.Checked = false;
            }
        }

        private void rdoDec_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDec.Checked)
            {
                rdoInc.Checked = false;
            }
        }

        private void rdoAnnual_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAnnual.Checked)
            {
                rdoQuarter.Checked = false;
                rdoMonth.Checked = false;
            }
        }

        private void rdoQuarter_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoQuarter.Checked)
            {
                rdoAnnual.Checked = false;
                rdoMonth.Checked = false;
            }
        }

        private void rdoMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMonth.Checked)
            {
                rdoAnnual.Checked = false;
                rdoQuarter.Checked = false;
            }
        }

        private void UpdateList()
        {
            dgvRevYear.Columns.Clear();
            dgvRevYear.Columns.Add("REVYEAR", "REV. YEAR");
            dgvRevYear.Columns.Add("EFFYEAR", "EFF. YEAR");
            dgvRevYear.Columns.Add("FEE", "APPLIED TO");
            dgvRevYear.Columns.Add("TYPE", "TYPE");
            dgvRevYear.Columns.Add("MEANS", "COMPUTATION");
            dgvRevYear.Columns.Add("TERM", "TERM");
            dgvRevYear.Columns.Add("VALUE", "VALUE");

            dgvRevYear.RowHeadersVisible = false;
            dgvRevYear.Columns[0].Width = 100;
            dgvRevYear.Columns[1].Width = 100;
            dgvRevYear.Columns[2].Width = 200;

            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from spl_ord_tbl order by rev_year";
            if (pSet.Execute())
            {
                string sFeeCode = string.Empty;
                string sFeeDesc = string.Empty;

                while (pSet.Read())
                {
                    sFeeCode = pSet.GetString("fees_code");
                    if (sFeeCode.Substring(0, 1) == "B")
                    {
                        sFeeDesc = AppSettingsManager.GetBnsDesc(sFeeCode.Substring(1, 2));
                        if(sFeeDesc == "")
                            sFeeDesc = "ALL BUSINESS LINE";
                    }
                    else
                    {
                        sFeeDesc = AppSettingsManager.GetFeesDesc(sFeeCode.Substring(1, 2));
                        if (sFeeDesc == "")
                            sFeeDesc = "ALL REGULATORY FEES";
                    }

                    dgvRevYear.Rows.Add(pSet.GetString("rev_year"),
                        pSet.GetString("eff_year"), sFeeDesc,
                        pSet.GetString("fees_type"),
                        pSet.GetString("fees_means"),
                        pSet.GetString("fees_term"),
                        pSet.GetDouble("value_fld"));
                }
            }
            pSet.Close();
        }
    }
}