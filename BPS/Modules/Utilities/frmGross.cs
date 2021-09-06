// JARS 20150811 CREATED PRESUMPTIVE GROSS TABLE 
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
    public partial class frmGross : Form
    {
        
        private string m_sQuery = string.Empty;
        private string n_sQuery = string.Empty;
        private string o_sQuery = string.Empty;
        private double gr1, gr2;
        private string gr1string;
        private string toBeEdited;

        //private DataGridView dgvPreGross = new DataGridView();
        public frmGross()
        {
            InitializeComponent();
        }

        
        
        private void frmGross_Load(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();
            /*pRec.Query = "select distinct(rev_year) from BNS_TABLE WHERE FEES_CODE = 'B' AND length(BNS_CODE) = 2 ";
            pRec.Execute();
            pRec.Read();
            string revYear = pRec.GetString("rev_year").Trim();
            txtDistRev.Text = revYear;*/
            // RMC 20150811 QA Presumptive gross module, put rem

            txtDistRev.Text = AppSettingsManager.GetConfigValue("07");  // RMC 20150811 QA Presumptive gross module
            
            gr1string = txtGross1.Text;
            this.txtGross1.Text = string.Format("{0:0.00}", 0);

            this.txtGross2.Text = string.Format("{0:0.00}", 0);
            
            /*cmbBnsDesc.Items.Add("A) MANUFACTURERS...");
            cmbBnsDesc.Items.Add("B) WHOLESALERS...");
            cmbBnsDesc.Items.Add("C1) MANUFACTURERS (ESSENTIALS)...");
            cmbBnsDesc.Items.Add("C2) WHOLESALERS (ESSENTIALS)...");
            cmbBnsDesc.Items.Add("C3) RETAILERS (ESSENTIALS)...");
            cmbBnsDesc.Items.Add("D) RETAILERS...");
            cmbBnsDesc.Items.Add("E) CONTRATORS...");
            cmbBnsDesc.Items.Add("F) OTHER CONTRACTORS...");
            cmbBnsDesc.Items.Add("G) BANKS & OTHER FINANCIAL INSTITUTIONS...");
            cmbBnsDesc.Items.Add("H) OTHER SVCS... ");
            cmbBnsDesc.Items.Add("TAX-EXEMPT BUSINESSES...");
            cmbBnsDesc.Items.Add("PEDDLERS...");*/
            // RMC 20150811 QA Presumptive gross module, put rem

            // RMC 20150811 QA Presumptive gross module (s)
            cmbBnsDesc.Items.Clear();
            pRec.Query = "select * from bns_table where fees_code = 'B' and length(bns_code) = 2 ";
            pRec.Query += " and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' order by bns_code";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    cmbBnsDesc.Items.Add(pRec.GetString("bns_desc"));
                }
            }
            pRec.Close();
            UpdateList();
            // RMC 20150811 QA Presumptive gross module (e)

            /*m_sQuery = "select BNS_DESC,GR_1,GR_2,REV_YEAR from pre_gross_tbl";
            DataGridViewOracleResultSet dgvPre  = new DataGridViewOracleResultSet(dgvPreGross, m_sQuery, 0, 0);
              */
            // RMC 20150811 QA Presumptive gross module, put rem

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            gr1 = Convert.ToDouble(txtGross1.Text);
            gr2 = Convert.ToDouble(txtGross2.Text);
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();
            if (btnAdd.Text == "Add")
            {
                btnAdd.Text = "Save";
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnClose.Text = "Cancel";
                txtGross1.Enabled = true;
                txtGross2.Enabled = true;
                cmbBnsDesc.Enabled = true;
            }
            else
            {
                if (cmbBnsDesc.Text != "" || txtGross1.Text != "" || txtGross2.Text != "")
                {
                    if (txtGross1.Text != "0.00" || txtGross2.Text != "0.00")
                    {
                        if (gr2 < gr1)
                        {
                            MessageBox.Show("Gross 2 cannot be lesser than gross 1");
                        }
                        else
                        {
                            
                            /*pRec.Query = "select BNS_CODE from BNS_TABLE WHERE BNS_DESC LIKE '" + cmbBnsDesc.Text + "%' AND length(BNS_CODE) = 2 ";
                            pRec.Execute();
                            pRec.Read();
                                string code = pRec.GetString("bns_code");*/
                            // RMC 20150811 QA Presumptive gross module, put rem

                            // RMC 20150811 QA Presumptive gross module (s)
                            string code = AppSettingsManager.GetBnsCodeByDesc(cmbBnsDesc.Text);

                            pRec.Query = "select * from pre_gross_tbl where bns_code = '" + code + "' and rev_year = '" + txtDistRev.Text + "'";
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                {
                                    pRec.Close();
                                    MessageBox.Show("Business type already been added","Presumptive Gross",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                                    return;
                                }
                            }
                            pRec.Close();

                            double dGr1 = 0;
                            double dGr2 = 0;
                            double.TryParse(txtGross1.Text, out dGr1);
                            double.TryParse(txtGross2.Text, out dGr2);

                            string revYear = txtDistRev.Text;
                            pSet.Query = "insert into pre_gross_tbl(BNS_CODE, BNS_DESC, GR_1, GR_2, REV_YEAR) values(:1, :2, :3, :4, :5)";
                            pSet.AddParameter(":1", code);
                            pSet.AddParameter(":2", cmbBnsDesc.Text);
                            pSet.AddParameter(":3", dGr1);
                            pSet.AddParameter(":4", dGr2);
                            pSet.AddParameter(":5", revYear);
                            pSet.ExecuteNonQuery();
                            // RMC 20150811 QA Presumptive gross module (e)

                            //m_sQuery = "select BNS_DESC,GR_1,GR_2,REV_YEAR from pre_gross_tbl";
                            MessageBox.Show("Add Successful");
                            //DataGridViewOracleResultSet dgvPre = new DataGridViewOracleResultSet(dgvPreGross, m_sQuery, 0, 0);

                            // RMC 20150811 QA Presumptive gross module (s)
                            UpdateList();
                            string sObject = txtDistRev.Text + ": " + code + ": Gross: " + dGr1 + "-" + dGr2;

                            if (AuditTrail.InsertTrail("AUTPG-A", "pre_gross_tbl", sObject) == 0)
                            {
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            // RMC 20150811 QA Presumptive gross module (e)

                            
                            btnAdd.Text = "Add";
                            txtGross1.Text = "0.00";
                            txtGross2.Text = "0.00";
                            cmbBnsDesc.Text = "";
                            cmbBnsDesc.Enabled = false;
                            txtGross1.Enabled = false;
                            txtGross2.Enabled = false;
                            btnEdit.Enabled = true;
                            btnDelete.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Warning! Do not leave any fields empty");
                        cmbBnsDesc.Text = "";
                        txtGross1.Text = "0.00";
                        txtGross2.Text = "0.00";
                        txtGross1.Enabled = false;
                        txtGross2.Enabled = false;
                        btnAdd.Text = "Add";
                        cmbBnsDesc.Enabled = false;
                    }
                    
                }
                else
                {
                    MessageBox.Show("Warning! Do not leave any fields empty");
                    cmbBnsDesc.Text = "";
                    txtGross1.Text = "0.00";
                    txtGross2.Text = "0.00";
                    txtGross1.Enabled = false;
                    txtGross2.Enabled = false;
                    btnAdd.Text = "Add";
                    cmbBnsDesc.Enabled = false;
                }
            }
            
        }

        private void txtGross1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtGross2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        private void dgvPreGross_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnClose.Text = "Cancel";
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            cmbBnsDesc.Text = dgvPreGross.SelectedRows[0].Cells[0].Value.ToString();
            string desc = dgvPreGross.SelectedRows[0].Cells[0].Value.ToString();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select BNS_CODE from BNS_TABLE WHERE BNS_DESC LIKE '" + desc + "%' AND length(BNS_CODE) = 2 ";
            pSet.Execute();
            pSet.Read();
            string code = pSet.GetString("bns_code");
            txtToBeEdited.Text = code;
            btnEdit.Enabled = true;
            txtGross1.Text = dgvPreGross.SelectedRows[0].Cells[1].Value.ToString();
            txtGross2.Text = dgvPreGross.SelectedRows[0].Cells[2].Value.ToString();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();
            if (btnEdit.Text == "Edit")
            {
                btnEdit.Text = "Save";
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnClose.Text = "Cancel";
                txtGross1.Enabled = true;
                txtGross2.Enabled = true;
                cmbBnsDesc.Enabled = true;
            }
            else
            {
                if (cmbBnsDesc.Text != "" || txtGross1.Text != "0.00" || txtGross2.Text != "0.00")
                {
                    if (txtGross1.Text != "" || txtGross2.Text != "")
                    {
                        if (gr2 < gr1)
                        {
                            //MessageBox.Show("Gross 2 cannot be lesser than gross 1"); // RMC 20150811 QA Presumptive gross module, put rem
                            // RMC 20150811 QA Presumptive gross module (s)
                            MessageBox.Show("Invalid Gross Range","Presumptive Gross Table",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                            return;
                            // RMC 20150811 QA Presumptive gross module (e)
                        }
                        else
                        {
                            /*pRec.Query = "select BNS_CODE from BNS_TABLE WHERE BNS_DESC LIKE '" + cmbBnsDesc.Text + "%' AND length(BNS_CODE) = 2 ";
                            pRec.Execute();
                            pRec.Read();
                            string code = pRec.GetString("bns_code");
                            string revYear = txtDistRev.Text;
                            pSet.Query = "update pre_gross_tbl SET gr_1 = '" + txtGross1.Text + "', gr_2 = '" + txtGross2.Text + "', rev_year = '" + revYear + "' WHERE bns_desc = '" + cmbBnsDesc.Text + "'";*/

                            // RMC 20150811 QA Presumptive gross module (s)
                            string sCode = AppSettingsManager.GetBnsCodeByDesc(cmbBnsDesc.Text);

                            double dGr1 = 0;
                            double dGr2 = 0;
                            double.TryParse(txtGross1.Text, out dGr1);
                            double.TryParse(txtGross2.Text, out dGr2);

                            pSet.Query = "update pre_gross_tbl SET gr_1 = '" + dGr1 + "', gr_2 = '" + dGr2 + "' WHERE bns_code = '" + sCode + "' and rev_year = '" + txtDistRev.Text + "'";
                            // RMC 20150811 QA Presumptive gross module (e)
                            pSet.Execute();

                            //m_sQuery = "select BNS_DESC,GR_1,GR_2,REV_YEAR from pre_gross_tbl";
                            MessageBox.Show("Edit Successful");
                            //DataGridViewOracleResultSet dgvPre = new DataGridViewOracleResultSet(dgvPreGross, m_sQuery, 0, 0);

                            // RMC 20150811 QA Presumptive gross module (s)
                            UpdateList();
                            string sObject = txtDistRev.Text + ": " + sCode + ": Gross: " + dGr1 + "-" + dGr2;

                            if (AuditTrail.InsertTrail("AUTPG-E", "pre_gross_tbl", sObject) == 0)
                            {
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            // RMC 20150811 QA Presumptive gross module (e)

                            btnEdit.Text = "Edit";
                            txtGross1.Text = "0.00";
                            txtGross2.Text = "0.00";
                            cmbBnsDesc.Text = "";
                            txtToBeEdited.Text = "";
                            cmbBnsDesc.Enabled = false;
                            txtGross1.Enabled = false;
                            txtGross2.Enabled = false;
                            btnAdd.Enabled = true;
                            btnDelete.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Warning! Do not leave any fields empty");
                        cmbBnsDesc.Text = "";
                        txtGross1.Text = "0.00";
                        txtGross2.Text = "0.00";
                        txtGross1.Enabled = false;
                        txtGross2.Enabled = false;
                        btnEdit.Text = "Edit";
                        cmbBnsDesc.Enabled = false;
                    }

                    
                }
                else
                {
                    MessageBox.Show("Warning! Do not leave any fields empty");
                    cmbBnsDesc.Text = "";
                    txtGross1.Text = "0.00";
                    txtGross2.Text = "0.00";
                    txtGross1.Enabled = false;
                    txtGross2.Enabled = false;
                    btnEdit.Text = "Edit";
                    cmbBnsDesc.Enabled = false;
                }
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            if (cmbBnsDesc.Text == "")
            {
                MessageBox.Show("Nothing to delete");
            }
            else
            {
                // RMC 20150811 QA Presumptive gross module (s)
                double dGr1 = 0;
                double dGr2 = 0;
                double.TryParse(txtGross1.Text, out dGr1);
                double.TryParse(txtGross2.Text, out dGr2);

                string sCode = AppSettingsManager.GetBnsCodeByDesc(cmbBnsDesc.Text);
                pSet.Query = "delete from pre_gross_tbl WHERE bns_code = '" + sCode + "'";
                // RMC 20150811 QA Presumptive gross module (e)

                //pSet.Query = "delete from pre_gross_tbl WHERE bns_desc = '" + cmbBnsDesc.Text + "'";
                pSet.Execute();
                //m_sQuery = "select BNS_DESC,GR_1,GR_2,REV_YEAR from pre_gross_tbl order by bns_code";
                

                //DataGridViewOracleResultSet dgvPre = new DataGridViewOracleResultSet(dgvPreGross, m_sQuery, 0, 0);
                MessageBox.Show("Delete Successful");

                // RMC 20150811 QA Presumptive gross module (s)
                UpdateList();
                string sObject = txtDistRev.Text + ": " + sCode + ": Gross: " + dGr1 + "-" + dGr2;

                if (AuditTrail.InsertTrail("AUTPG-D", "pre_gross_tbl", sObject) == 0)
                {
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // RMC 20150811 QA Presumptive gross module (e)

                btnAdd.Enabled = true;
                btnEdit.Enabled = false;
                
                
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Cancel")
            {
                cmbBnsDesc.Text = "";
                this.txtGross1.Text = string.Format("{0:0.00}", 0);

                this.txtGross2.Text = string.Format("{0:0.00}", 0);
                btnAdd.Enabled = true;
                btnAdd.Text = "Add";
                btnEdit.Text = "Edit";
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnClose.Text = "Close";
                txtGross1.Enabled = false;
                txtGross2.Enabled = false;
                cmbBnsDesc.Enabled = false;
                txtToBeEdited.Text = "";
            }
            else
            {
                this.Close();
            }
        }

        private void txtGross1_Leave(object sender, EventArgs e)
        {
            // RMC 20150811 QA Presumptive gross module
            double dTmp = 0;
            double.TryParse(txtGross1.Text, out dTmp);
            txtGross1.Text = string.Format("{0:#,###.00}", dTmp);
        }

        private void txtGross2_Leave(object sender, EventArgs e)
        {
            // RMC 20150811 QA Presumptive gross module
            double dTmp = 0;
            double.TryParse(txtGross2.Text, out dTmp);
            txtGross2.Text = string.Format("{0:#,###.00}", dTmp);
        }

        private void UpdateList()
        {
            // RMC 20150811 QA Presumptive gross module
            OracleResultSet pSet = new OracleResultSet();

            dgvPreGross.Columns.Clear();
            dgvPreGross.Columns.Add("BNS", "BUSINESS TYPE");
            dgvPreGross.Columns.Add("GR1", "GROSS 1");
            dgvPreGross.Columns.Add("GR2", "GROSS 2");
            dgvPreGross.Columns.Add("REVYEAR", "REVENUE YEAR");
            
            dgvPreGross.RowHeadersVisible = false;
            dgvPreGross.Columns[0].Width = 150;
            dgvPreGross.Columns[1].Width = 100;
            dgvPreGross.Columns[2].Width = 100;
            dgvPreGross.Columns[3].Width = 80;
            
            dgvPreGross.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvPreGross.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            pSet.Query = "select BNS_DESC,GR_1,GR_2,REV_YEAR from pre_gross_tbl";
            pSet.Query += " where rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dgvPreGross.Rows.Add(pSet.GetString(0), string.Format("{0:#,###.00}",pSet.GetDouble(1)), 
                        string.Format("{0:#,###.00}",pSet.GetFloat(2)), pSet.GetString(3));
                }
            }
            pSet.Close();
        }
    }
}