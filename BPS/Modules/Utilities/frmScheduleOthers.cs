using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.Utilities
{
    public partial class frmScheduleOthers : Form
    {
        private string m_strRevYear = string.Empty;
        private string m_strPrevValue = string.Empty;
        private bool m_bSwFeesCode = false;

        private string _moduletype;
        public string moduletype
        {
            get { return _moduletype; }
            set
            {
                _moduletype = value;
            }
        }

        public frmScheduleOthers()
        {
            InitializeComponent();
        }

        private void frmScheduleOthers_Load(object sender, EventArgs e)
        { //peb 20191204 (s)
            m_strRevYear = AppSettingsManager.GetConfigObject("07");
            OracleResultSet result = new OracleResultSet();
            if (moduletype == "Brgy")
            {
                this.Text = "Brangay Clearance Charges"; //JHB 20191229
                this.Size = new Size(900, 402); //peb 20191227
                dgvListOthers.Width += 200; //peb 20191227
                containerWithShadow2.Width += 200; //peb 20191227
                result.Query = "select distinct(brgy_nm) from brgy order by brgy_nm asc";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        cmbBrgy.Items.Add(result.GetString("brgy_nm"));
                    }
                }
                result.Close();

                foreach (Control c in panel1.Controls)
                {
                    c.Visible = false;
                }
                cmbBrgy.Visible = true;
                brgycode.Visible = true;
                panel2.Visible = true;
                panel2.BringToFront();
                btnEdit.Enabled = false;
                //lbl1.Visible = true;
                //lbl2.Visible=true;
                //lbl3.Visible=true;
                //lblHeader.Visible=true;
                
            }
               
            else
            {
                cmbBrgy.Visible = false;
                brgycode.Visible = false;
                btned.Visible = false;
                btnEdit.Enabled = true;
                this.chkAddl.Checked = true;
                
                dgvListOthers.ReadOnly = false;
                this.EnableControls();

             
                //(e)     
            }
            //m_strRevYear = AppSettingsManager.GetConfigObject("07");
            //this.chkAddl.Checked = true;
            //this.EnableControls();
        }

        private void EnableControls()
        {
            if (this.chkAddl.Checked == true)
            {
                txtRate.Enabled = false;
                chkRate.Enabled = false;
                chkQtr.Enabled = false;
                chkSurch.Enabled = false;
                chkInt.Enabled = false;
                chkRate.Checked = false;    //initialize
                chkQtr.Checked = false;    //initialize
                chkSurch.Checked = false;    //initialize
                chkInt.Checked = false;    //initialize
                btnEdit.Enabled = false;
                txtMinFee.Enabled = false;
            }
            else if (this.chkFire.Checked == true)
            {
                txtMinFee.Enabled = true;
                txtRate.Enabled = true;
                chkRate.Enabled = true;
                chkQtr.Enabled = true;
                chkSurch.Enabled = true;
                chkInt.Enabled = true;
                chkRate.Checked = false;    //initialize
                chkQtr.Checked = false;    //initialize
                chkSurch.Checked = false;    //initialize
                chkInt.Checked = false;    //initialize
                btnEdit.Enabled = false;
            }
            else if (this.chkCTC.Checked == true)
            {
                txtMinFee.Enabled = false;
                txtRate.Enabled = true;
                chkRate.Enabled = true;
                chkQtr.Enabled = true;
                chkSurch.Enabled = false;
                chkInt.Enabled = false;
                chkRate.Checked = false;    //initialize
                chkQtr.Checked = false;    //initialize
                chkSurch.Checked = false;    //initialize
                chkInt.Checked = false;    //initialize
                btnEdit.Enabled = false;
            }
        }

        private void LoadList()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string strCode = string.Empty;
            string strDesc = string.Empty;
            string strTerm = string.Empty;
            string strSurch = string.Empty;
            string strPen = string.Empty;
            string strType = string.Empty;
            double dblAmount = 0;

            m_bSwFeesCode = true;
            if (this.chkAddl.Checked == true)
            {
                dgvListOthers.Columns.Clear();
                dgvListOthers.Columns.Add("CODE", "Code");
                dgvListOthers.Columns.Add("NAME", "Name");
                //dgvListOthers.Columns.Add("TYPE", "Type");
                //dgvListOthers.Columns.Add("TERM", "Term");
                //dgvListOthers.Columns.Add("SURCH", "Surch");
                //dgvListOthers.Columns.Add("INT", "Int");
                dgvListOthers.Columns.Add("AMT", "Amount");

                DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
                comboBox.HeaderCell.Value = "Type";
                dgvListOthers.Columns.Insert(2, comboBox);
                comboBox.Items.AddRange("F", "Q", "O");

                DataGridViewComboBoxColumn comboBox2 = new DataGridViewComboBoxColumn();
                comboBox2.HeaderCell.Value = "Term";
                dgvListOthers.Columns.Insert(3, comboBox2);
                comboBox2.Items.AddRange("Q", "F");

                DataGridViewComboBoxColumn comboBox3 = new DataGridViewComboBoxColumn();
                comboBox3.HeaderCell.Value = "Surch";
                dgvListOthers.Columns.Insert(4, comboBox3);
                comboBox3.Items.AddRange("Y", "N");

                DataGridViewComboBoxColumn comboBox4 = new DataGridViewComboBoxColumn();
                comboBox4.HeaderCell.Value = "Int";
                dgvListOthers.Columns.Insert(5, comboBox4);
                comboBox4.Items.AddRange("Y", "N");

                dgvListOthers.RowHeadersVisible = false;
                dgvListOthers.Columns[0].Width = 40;
                dgvListOthers.Columns[1].Width = 200;
                dgvListOthers.Columns[2].Width = 40;
                dgvListOthers.Columns[3].Width = 40;
                dgvListOthers.Columns[4].Width = 40;
                dgvListOthers.Columns[5].Width = 40;
                dgvListOthers.Columns[6].Width = 80;

                dgvListOthers.Columns[0].ReadOnly = true;
                dgvListOthers.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

                result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'AD' and rev_year = '{0}' order by fees_code", m_strRevYear);
                if (result.Execute())
                {
                    int intRow = 0;
                    while (result.Read())
                    {
                        dgvListOthers.Rows.Add("");

                        strCode = result.GetString("fees_code").Trim();
                        strDesc = result.GetString("fees_desc").Trim();
                        strTerm = result.GetString("fees_term").Trim();
                        strSurch = result.GetString("fees_withsurch").Trim();
                        strPen = result.GetString("fees_withpen").Trim();

                        strType = "";
                        dblAmount = 0;

                        result2.Query = string.Format("select * from addl_sched where fees_code = '{0}' and rev_year = '{1}'", strCode, m_strRevYear);
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                strType = result2.GetString("data_type");
                                if (strType != "O")
                                    dblAmount = result2.GetDouble("amount");
                            }
                        }
                        result2.Close();

                        dgvListOthers[0, intRow].Value = strCode;
                        dgvListOthers[1, intRow].Value = strDesc;
                        dgvListOthers[2, intRow].Value = strType;
                        dgvListOthers[3, intRow].Value = strTerm;
                        dgvListOthers[4, intRow].Value = strSurch;
                        dgvListOthers[5, intRow].Value = strPen;
                        if (strType == "O")
                            dgvListOthers[6, intRow].Value = "";
                        else
                            dgvListOthers[6, intRow].Value = string.Format("{0:#0.00}", dblAmount);

                        intRow++;
                    }
                    dgvListOthers.Rows.Add("");
                }
                result.Close();
            }
            else if (this.chkFire.Checked == true)
            {
                dgvListOthers.Columns.Clear();
                dgvListOthers.Columns.Add(new DataGridViewCheckBoxColumn());
                dgvListOthers.Columns.Add("FIRE", "Fire Safety Inspection Fee");

                dgvListOthers.RowHeadersVisible = false;
                dgvListOthers.Columns[0].Width = 30;
                dgvListOthers.Columns[1].Width = 400;

                result.Query = "delete from fire_tax_table";
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = "insert into fire_tax_table (description) values (:1)";
                result.AddParameter(":1", AppSettingsManager.GetConfigValue("17"));
                if(result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = string.Format("insert into fire_tax_table (select distinct(fees_desc) from tax_and_fees_table where rev_year = '{0}' and fees_type <> 'FR')", m_strRevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = "select * from fire_tax_table order by description";
                if (result.Execute())
                {
                    int intRow = 0;
                    while (result.Read())
                    {
                        dgvListOthers.Rows.Add("");
                        dgvListOthers[0, intRow].Value = false;
                        dgvListOthers[1, intRow].Value = result.GetString("description");

                        intRow++;
                    }
                }
                result.Close();

                this.LoadCheckList();

            }
            else if (this.chkCTC.Checked == true)
            {
                dgvListOthers.Visible = false;
            }

        }

        private void dgvListOthers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                e.Value = e.Value.ToString().ToUpper();
                e.FormattingApplied = true;
            }
        }

        private void brgyloadlist()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string strCode = string.Empty;
            string strDesc = string.Empty;
            string strTerm = string.Empty;
            string strSurch = string.Empty;
            string strPen = string.Empty;
            string strType = string.Empty;
            double dblAmount = 0;

            m_bSwFeesCode = true;
            if (this.chkAddl.Checked != true)
            {
                dgvListOthers.Columns.Clear();
                dgvListOthers.Columns.Add("CODE", "Code");
                dgvListOthers.Columns.Add("NAME", "Name");
                //dgvListOthers.Columns.Add("TYPE", "Type");
                //dgvListOthers.Columns.Add("TERM", "Term");
                //dgvListOthers.Columns.Add("SURCH", "Surch");
                //dgvListOthers.Columns.Add("INT", "Int");
                dgvListOthers.Columns.Add("AMT", "Amount");

                DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
                comboBox.HeaderCell.Value = "Type";
                dgvListOthers.Columns.Insert(2, comboBox);
                comboBox.Items.AddRange("F", "Q", "O");

                DataGridViewComboBoxColumn comboBox2 = new DataGridViewComboBoxColumn();
                comboBox2.HeaderCell.Value = "Term";
                dgvListOthers.Columns.Insert(3, comboBox2);
                comboBox2.Items.AddRange("Q", "F");

                DataGridViewComboBoxColumn comboBox3 = new DataGridViewComboBoxColumn();
                comboBox3.HeaderCell.Value = "Surch";
                dgvListOthers.Columns.Insert(4, comboBox3);
                comboBox3.Items.AddRange("Y", "N");

                DataGridViewComboBoxColumn comboBox4 = new DataGridViewComboBoxColumn();
                comboBox4.HeaderCell.Value = "Int";
                dgvListOthers.Columns.Insert(5, comboBox4);
                comboBox4.Items.AddRange("Y", "N");

                dgvListOthers.RowHeadersVisible = false;
                dgvListOthers.Columns[0].Width = 40;
                dgvListOthers.Columns[1].Width = 400;
                dgvListOthers.Columns[2].Width = 40;
                dgvListOthers.Columns[3].Width = 40;
                dgvListOthers.Columns[4].Width = 40;
                dgvListOthers.Columns[5].Width = 40;
                dgvListOthers.Columns[6].Width = 80;

                dgvListOthers.Columns[0].ReadOnly = true;
                dgvListOthers.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvListOthers.MultiSelect = false; //AFM 20191223
               
                result.Query = "select * from barangay_taxandfeestable where   rev_year = '" +m_strRevYear+ "' and BARANGAY = '" + cmbBrgy.Text + "' order by fees_code";

                if (result.Execute())
                {
                    int intRow = 0;
                    while (result.Read())
                    {
                        dgvListOthers.Rows.Add("");

                        strCode = string.Format("{0:D2}", result.GetString("fees_code"));//(result.GetString("fees_code")).ToString("D2");
                        strDesc = result.GetString("fees_desc").Trim();
                        strTerm = result.GetString("fees_term").Trim();
                        strSurch = result.GetString("fees_withsurch").Trim();
                        strPen = result.GetString("fees_withpen").Trim();

                        strType = "";
                        dblAmount = 0;

                        // result2.Query = string.Format("select * from barangay_addl_sched where fees_code = '{0}' and rev_year = '{1}'", strCode, m_strRevYear);
                        result2.Query = string.Format("select * from barangay_addl_sched where fees_code = '{0}' and rev_year = '{1}' and brgy_nm = '" + cmbBrgy.Text + "'", strCode, m_strRevYear);
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                strType = result2.GetString("data_type");
                                if (strType != "O")
                                    dblAmount = result2.GetDouble("amount");
                            }
                        }
                        result2.Close();

                        dgvListOthers[0, intRow].Value = (int.Parse(strCode)).ToString("D2");
                        dgvListOthers[1, intRow].Value = strDesc;
                        dgvListOthers[2, intRow].Value = strType;
                        dgvListOthers[3, intRow].Value = strTerm;
                        dgvListOthers[4, intRow].Value = strSurch;
                        dgvListOthers[5, intRow].Value = strPen;
                        if (strType == "O")
                            dgvListOthers[6, intRow].Value = "";
                        else
                            dgvListOthers[6, intRow].Value = string.Format("{0:#0.00}", dblAmount);

                        intRow++;

                    }
                    dgvListOthers.Rows.Add("");
                }
                result.Close();
            }
            else
            {
                #region
                //dgvListOthers.Columns.Clear();
                //dgvListOthers.Columns.Add(new DataGridViewCheckBoxColumn());
                //dgvListOthers.Columns.Add("FIRE", "Fire Safety Inspection Fee");

                //dgvListOthers.RowHeadersVisible = false;
                //dgvListOthers.Columns[0].Width = 30;
                //dgvListOthers.Columns[1].Width = 400;

                //result.Query = "delete from fire_tax_table";
                //if (result.ExecuteNonQuery() == 0)
                //{
                //}

                //result.Query = "insert into fire_tax_table (description) values (:1)";
                //result.AddParameter(":1", AppSettingsManager.GetConfigValue("17"));
                //if (result.ExecuteNonQuery() == 0)
                //{
                //}

                //result.Query = string.Format("insert into fire_tax_table (select distinct(fees_desc) from tax_and_fees_table where rev_year = '{0}' and fees_type <> 'FR')", m_strRevYear);
                //if (result.ExecuteNonQuery() == 0)
                //{
                //}

                //result.Query = "select * from fire_tax_table order by description";
                //if (result.Execute())
                //{
                //    int intRow = 0;
                //    while (result.Read())
                //    {
                //        dgvListOthers.Rows.Add("");
                //        dgvListOthers[0, intRow].Value = false;
                //        dgvListOthers[1, intRow].Value = result.GetString("description");

                //        intRow++;
                //    }
                //}
                //result.Close();

                //this.LoadCheckList();
                #endregion
                this.LoadCheckList();

            }
        }

        private void dgvListOthers_CellClick(object sender, DataGridViewCellEventArgs e)
           {
            this.lblHeader.Visible = false;
            this.lbl1.Visible = false;
            this.lbl2.Visible = false;
            this.lbl3.Visible = false;
            string input = string.Empty;//JHB 20191229
            if (this.chkAddl.Checked == true)
            {
                if (e.ColumnIndex == 2)
                {
                    this.lblHeader.Text = "T Y P E";
                    this.lbl1.Text = "F - Fixed Amount";
                    this.lbl2.Text = "Q - Quantity";
                    this.lbl3.Text = "O - Others";
                    this.lblHeader.Visible = true;
                    this.lbl1.Visible = true;
                    this.lbl2.Visible = true;
                    this.lbl3.Visible = true;
                }
                else if (e.ColumnIndex == 3)
                {
                    this.lblHeader.Text = "T E R M";
                    this.lbl1.Text = "F - Full Payment";
                    this.lbl2.Text = "Q - Quarterly";

                    this.lblHeader.Visible = true;
                    this.lbl1.Visible = true;
                    this.lbl2.Visible = true;
                    this.lbl3.Visible = false;
                }
                else if (e.ColumnIndex == 4)
                {
                    this.lblHeader.Text = "SURCHARGE";
                    this.lbl1.Text = "Y - with Surcharge";
                    this.lbl2.Text = "N - w/o Surcharge";
                    this.lblHeader.Visible = true;
                    this.lbl1.Visible = true;
                    this.lbl2.Visible = true;
                    this.lbl3.Visible = false;
                }
                else if (e.ColumnIndex == 5)
                {
                    this.lblHeader.Text = "INTEREST";
                    this.lbl1.Text = "Y - with Interest";
                    this.lbl2.Text = "N - w/o Interest";
                    this.lblHeader.Visible = true;
                    this.lbl1.Visible = true;
                    this.lbl2.Visible = true;
                    this.lbl3.Visible = false;

                }
            }
            #region  //peb 20191210(s)
            else if (moduletype == "Brgy")
            {
                if (btnEdit.Enabled == false)
                {
                    
                    dgvListOthers.ReadOnly = true;

                    return;
                }
                else
                {
                    dgvListOthers.ReadOnly = false;
                }
                //string input = "";


                
                for (int i = 0; i < dgvListOthers.Rows.Count; i++)
                {

                    input = Convert.ToString(dgvListOthers.Rows[i].Cells[1].Value);
                    char[] str = input.Trim().ToCharArray();


                    foreach (char item in str)
                    {
                        if (item.ToString() == "," && item.ToString() == "!" && item.ToString() == "/" && item.ToString() == "-" && item.ToString() == ":")
                            break;
                        if (char.IsSymbol(item))
                        {
                            MessageBox.Show("Special Characters are not accepted");
                            return;

                        }
                    }
                }

                if (e.ColumnIndex == 2)
                {
                    this.lblHeader.Text = "T Y P E";
                    this.lbl1.Text = "F - Fixed Amount";
                    this.lbl2.Text = "Q - Quantity";
                    this.lbl3.Text = "O - Others";
                    this.lblHeader.Visible = true;
                    this.lbl1.Visible = true;
                    this.lbl2.Visible = true;
                    this.lbl3.Visible = true;
                }
                else if (e.ColumnIndex == 3)
                {
                    this.lblHeader.Text = "T E R M";
                    this.lbl1.Text = "F - Full Payment";
                    this.lbl2.Text = "Q - Quarterly";

                    this.lblHeader.Visible = true;
                    this.lbl1.Visible = true;
                    this.lbl2.Visible = true;
                    this.lbl3.Visible = false;
                }
                else if (e.ColumnIndex == 4)
                {
                    this.lblHeader.Text = "SURCHARGE";
                    this.lbl1.Text = "Y - with Surcharge";
                    this.lbl2.Text = "N - w/o Surcharge";
                    this.lblHeader.Visible = true;
                    this.lbl1.Visible = true;
                    this.lbl2.Visible = true;
                    this.lbl3.Visible = false;
                }
                else if (e.ColumnIndex == 5)
                {
                    this.lblHeader.Text = "INTEREST";
                    this.lbl1.Text = "Y - with Interest";
                    this.lbl2.Text = "N - w/o Interest";
                    this.lblHeader.Visible = true;
                    this.lbl1.Visible = true;
                    this.lbl2.Visible = true;
                    this.lbl3.Visible = false;

                }

            }
            #endregion //peb 20191210(e)
            else
            {
                this.btnClose.Text = "&Cancel";
                this.btnEdit.Enabled = true;

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "&Close")
                this.Close();
            else
            {
                if (moduletype != "Brgy")
                {
                    if (MessageBox.Show("Cancel changes?", "Other Charges", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.LoadList();
                        this.btnClose.Text = "&Close";
                    }
                }
                else
                {
                    this.brgyloadlist();
                    this.btnClose.Text = "&Close";
                }
            }
        } 
         
        private void dgvListOthers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
           {
              string input = "";
            if (moduletype == "Brgy")
            {
                #region check Special Characters


                string specialChar = @"|#$%&=?»«@£§€{}.;<>_";
                for (int i = 0; i < dgvListOthers.Rows.Count; i++)
                {

                    input = Convert.ToString(dgvListOthers.Rows[i].Cells[1].Value);
                    char[] str = input.Trim().ToCharArray();


                    foreach (char item in str)
                    {
                        if (item.ToString() == "," && item.ToString() == "!" && item.ToString() == "/" && item.ToString() == "-" && item.ToString() == ":")
                            break;
                        // AFM 20200102 removed
                        //if (item.ToString() == "'")
                        //{
                        //    MessageBox.Show("Special Characters are not accepted");
                        //    return;
                        //}
                        if (char.IsSymbol(item))
                        {
                            MessageBox.Show("Special Characters are not accepted");
                            return;

                        }
                    }
                }



                #endregion check Special Characters

                int lastfees;

                OracleResultSet result = new OracleResultSet();

                if (chkAddl.Checked == true)
                {
                    if (dgvListOthers[e.ColumnIndex, e.RowIndex].Value.ToString() != "")
                    {
                        this.btnClose.Text = "&Cancel";
                        this.btnEdit.Enabled = true;
                    }
                    else
                    {
                        this.btnClose.Text = "&Close";
                        this.btnEdit.Enabled = false;
                    }
                }

                string strValue = string.Empty;

                if (e.ColumnIndex == 6)
                {
                    strValue = dgvListOthers[6, e.RowIndex].Value.ToString();

                    try
                    {
                        strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));
                    }
                    catch
                    {
                        MessageBox.Show("Error in Field", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dgvListOthers[6, e.RowIndex].Value = m_strPrevValue;
                        return;
                    }

                    try
                    {
                        if (dgvListOthers[2, e.RowIndex].Value.ToString().Trim() == "O")
                            dgvListOthers[6, e.RowIndex].Value = "";
                        else
                            dgvListOthers[6, e.RowIndex].Value = strValue;
                    }
                    catch
                    {
                    }

                }

                if (dgvListOthers[e.ColumnIndex, e.RowIndex].Value.ToString().Trim() != "")
                {
                    if (e.ColumnIndex == 1)
                    {
                        strValue = dgvListOthers[1, e.RowIndex].Value.ToString().Trim();
                        dgvListOthers[1, e.RowIndex].Value = strValue.ToUpper();

                      

                        for (int intCtr = 0; intCtr <= dgvListOthers.Rows.Count - 1; intCtr++)
                        {
                            if (e.RowIndex != intCtr)
                            {
                                if (dgvListOthers[1, intCtr].Value != null)
                                {
                                    if (dgvListOthers[1, e.RowIndex].Value.ToString().Trim() ==
                                        dgvListOthers[1, intCtr].Value.ToString().Trim())
                                    {
                                        MessageBox.Show("Fee Description Already Exist.", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        dgvListOthers[1, e.RowIndex].Value = "";
                                    }
                                }
                             
                            }
                        }
                    }
                }
                try
                {

                    if (e.ColumnIndex == 2 && dgvListOthers[2, e.RowIndex].Value.ToString() == "O")
                        dgvListOthers[6, e.RowIndex].Value = "";

                    if (e.ColumnIndex == 6 && dgvListOthers[2, e.RowIndex].Value.ToString() == "O")
                        dgvListOthers[6, e.RowIndex].Value = "";
                }
                catch
                {
                }

                int intFeesCode = 0;
                string strFeesCode = "";

                #region barangay_taxandfeestable //JHB 20191229 modify saving of barangay_taxandfeestable
                if (dgvListOthers[0, e.RowIndex].Value.ToString() == "" && (e.ColumnIndex == 6 || e.ColumnIndex == 5))
                {
                    if (m_bSwFeesCode)
                    {
                        m_bSwFeesCode = false;
                        result.Query = string.Format("select * from barangay_taxandfeestable where rev_year = '{0}' and brgy_code='"+ brgycode.Text+"' order by fees_code desc", m_strRevYear);
                        if (result.Execute())
                        {
                            if (result.Read())
                                strFeesCode = result.GetString("fees_code");
                            else
                               
                               strFeesCode = "0";

                            int.TryParse(strFeesCode, out intFeesCode);
                        }
                        result.Close();
                    }

                    if (e.RowIndex == dgvListOthers.Rows.Count - 1)
                        intFeesCode++;

                    strFeesCode = string.Format("{0:00}", intFeesCode);

                    if (e.ColumnIndex == 5 && dgvListOthers[2, e.RowIndex].Value.ToString() == "0")
                        dgvListOthers[0, e.RowIndex].Value = strFeesCode;
                    else
                        dgvListOthers[0, e.RowIndex].Value = strFeesCode;

                    if (strValue != "")
                    {
                        if (e.ColumnIndex == 5 && dgvListOthers[2, e.RowIndex].Value.ToString() == "0")
                            dgvListOthers.Rows.Add("");
                        else
                            dgvListOthers.Rows.Add("");
                    }
                }
                #endregion barangay_taxandfeestable //JHB 20191229 modify saving of barangay_taxandfeestable



            }
            else
            #region tax_and_fees_table
            {

                
                OracleResultSet result = new OracleResultSet();

                if (chkAddl.Checked == true)
                {
                    if (dgvListOthers[e.ColumnIndex, e.RowIndex].Value.ToString() != "")
                    {
                        this.btnClose.Text = "&Cancel";
                        this.btnEdit.Enabled = true;
                    }
                    else
                    {
                        this.btnClose.Text = "&Close";
                        this.btnEdit.Enabled = false;
                    }
                }

                string strValue = string.Empty;

                if (e.ColumnIndex == 6)
                {
                    strValue = dgvListOthers[6, e.RowIndex].Value.ToString();

                    try
                    {
                        strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));
                    }
                    catch
                    {
                        MessageBox.Show("Error in Field", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dgvListOthers[6, e.RowIndex].Value = m_strPrevValue;
                        return;
                    }

                    try
                    {
                        if (dgvListOthers[2, e.RowIndex].Value.ToString().Trim() == "O")
                            dgvListOthers[6, e.RowIndex].Value = "";
                        else
                            dgvListOthers[6, e.RowIndex].Value = strValue;
                    }
                    catch
                    {
                    }

                }

                if (dgvListOthers[e.ColumnIndex, e.RowIndex].Value.ToString().Trim() != "")
                {
                    if (e.ColumnIndex == 1)
                    {
                        strValue = dgvListOthers[1, e.RowIndex].Value.ToString().Trim();
                        dgvListOthers[1, e.RowIndex].Value = strValue.ToUpper();

                        result.Query = string.Format("select * from tax_and_fees_table where fees_desc = '{0}' and rev_year = '{1}'", strValue, m_strRevYear);
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                MessageBox.Show("Fee Description Already Exist.", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dgvListOthers[1, e.RowIndex].Value = "";
                            }
                        }
                        result.Close();

                        for (int intCtr = 0; intCtr <= dgvListOthers.Rows.Count - 1; intCtr++)
                        {
                            if (e.RowIndex != intCtr)
                            {
                                if (dgvListOthers[1, intCtr].Value != null)
                                {
                                    if (dgvListOthers[1, e.RowIndex].Value.ToString().Trim() ==
                                        dgvListOthers[1, intCtr].Value.ToString().Trim())
                                    {
                                        MessageBox.Show("Fee Description Already Exist.", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        dgvListOthers[1, e.RowIndex].Value = "";
                                    }
                                }
                                else
                                    dgvListOthers.Rows.RemoveAt(intCtr);
                            }
                        }
                    }
                }
                try
                {

                    if (e.ColumnIndex == 2 && dgvListOthers[2, e.RowIndex].Value.ToString() == "O")
                        dgvListOthers[6, e.RowIndex].Value = "";

                    if (e.ColumnIndex == 6 && dgvListOthers[2, e.RowIndex].Value.ToString() == "O")
                        dgvListOthers[6, e.RowIndex].Value = "";
                }
                catch
                {
                }

                int intFeesCode = 0;
                string strFeesCode = "";

                if (dgvListOthers[0, e.RowIndex].Value.ToString() == "" && (e.ColumnIndex == 6 || e.ColumnIndex == 5))
                {
                    if (m_bSwFeesCode)
                    {
                        m_bSwFeesCode = false;
                        result.Query = string.Format("select * from tax_and_fees_table where rev_year = '{0}' order by fees_code desc", m_strRevYear);
                        if (result.Execute())
                        {
                            if (result.Read())
                                strFeesCode = result.GetString("fees_code");
                            else
                                strFeesCode = "0";

                            int.TryParse(strFeesCode, out intFeesCode);

                        }
                        result.Close();
                    }

                    if (e.RowIndex == dgvListOthers.Rows.Count - 1)
                        intFeesCode++;

                    strFeesCode = string.Format("{0:00}", intFeesCode);

                    if (e.ColumnIndex == 5 && dgvListOthers[2, e.RowIndex].Value.ToString() == "0")
                        dgvListOthers[0, e.RowIndex].Value = strFeesCode;
                    else
                        dgvListOthers[0, e.RowIndex].Value = strFeesCode;

                    if (strValue != "")
                    {
                        if (e.ColumnIndex == 5 && dgvListOthers[2, e.RowIndex].Value.ToString() == "0")
                            dgvListOthers.Rows.Add("");
                        else
                            dgvListOthers.Rows.Add("");
                    }
                }

            }
                #endregion tax_and_fees_table
        }
            
    
        private void savebrgyChrgs()//peb20191212
        {
        
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();

            try
            {
                for (int intRow = 0; intRow <= dgvListOthers.Rows.Count - 1; intRow++)
                {
                    if (dgvListOthers[0, intRow].Value == null || dgvListOthers[0, intRow].Value.ToString() == "")
                        dgvListOthers.Rows.RemoveAt(intRow);
                }

                for (int intRow = 0; intRow <= dgvListOthers.Rows.Count - 1; intRow++)
                {
                    if (dgvListOthers[2, intRow].Value.ToString().Trim() == "O")
                    {
                        if (dgvListOthers[0, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[1, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[2, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[3, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[4, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[5, intRow].Value.ToString().Trim() != "")
                        {
                            continue;
                        }
                        else
                        {
                            MessageBox.Show("Incomplete data.", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    else
                    {
                        if (dgvListOthers[0, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[1, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[2, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[3, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[4, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[5, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[6, intRow].Value.ToString().Trim() != "")
                        {
                            continue;
                        }
                        else
                        {
                            MessageBox.Show("Incomplete data.", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }

                    }
                }

                for (int intRow = 0; intRow <= dgvListOthers.Rows.Count - 1; intRow++)
                {
                    string strAmount = string.Empty;
                    double dblAmount = 0;

                    try
                    {
                        strAmount = dgvListOthers[6, intRow].Value.ToString().Trim();
                    }
                    catch
                    {
                        strAmount = "0";
                    }

                    double.TryParse(strAmount, out dblAmount);
                    strAmount = string.Format("{0:##.00}", dblAmount);

                    if (dgvListOthers[2, intRow].Value.ToString().Trim() == "O")
                        strAmount = "";

                    if (dgvListOthers[0, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[1, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[2, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[3, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[4, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[5, intRow].Value.ToString().Trim() != "")
                    {
                        result.Query = "insert into barangay_taxandfeestable (fees_code, fees_desc, fees_type, fees_term, fees_withsurch, fees_withpen, rev_year,BARANGAY,brgy_code) values (:1,:2,:3,:4,:5,:6,:7,:8,:9)";
                        result.AddParameter(":1", dgvListOthers[0, intRow].Value.ToString().Trim());
                        result.AddParameter(":2", StringUtilities.HandleApostrophe(dgvListOthers[1, intRow].Value.ToString().Trim()));
                        result.AddParameter(":3", dgvListOthers[2, intRow].Value.ToString().Trim());
                        result.AddParameter(":4", dgvListOthers[3, intRow].Value.ToString().Trim());
                        result.AddParameter(":5", dgvListOthers[4, intRow].Value.ToString().Trim());
                        result.AddParameter(":6", dgvListOthers[5, intRow].Value.ToString().Trim());
                        result.AddParameter(":7", m_strRevYear);
                        result.AddParameter(":8", cmbBrgy.Text.Trim());
                        result.AddParameter(":9", brgycode.Text);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        result.Query = "insert into barangay_addl_sched (fees_code, data_type, amount, rev_year,brgy_nm) values(:1,:2,:3,:4,:5)";
                        result.AddParameter(":1", dgvListOthers[0, intRow].Value.ToString().Trim());
                        result.AddParameter(":2", dgvListOthers[2, intRow].Value.ToString().Trim());
                        result.AddParameter(":3", dgvListOthers[6, intRow].Value);
                        result.AddParameter(":4", m_strRevYear);
                        result.AddParameter(":5", cmbBrgy.Text);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }


                    }
                }

                //peb20191227(s)
                for (int intRow = 0; intRow <= dgvListOthers.Rows.Count - 1; intRow++)
                {
                    string strAmount = string.Empty;
                    double dblAmount = 0;

                    try
                    {
                        strAmount = dgvListOthers[6, intRow].Value.ToString().Trim();
                    }
                    catch
                    {
                        strAmount = "0";
                    }

                    double.TryParse(strAmount, out dblAmount);
                    strAmount = string.Format("{0:##.00}", dblAmount);

                    if (dgvListOthers[2, intRow].Value.ToString().Trim() == "O")
                        strAmount = "";

                    if (dgvListOthers[0, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[1, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[2, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[3, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[4, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[5, intRow].Value.ToString().Trim() != "")
                    {
                        result.Query = "insert into barangay_taxandfeestable (fees_code, fees_desc, fees_type, fees_term, fees_withsurch, fees_withpen, rev_year,BARANGAY,brgy_code) values (:1,:2,:3,:4,:5,:6,:7,:8,:9)";
                        result.AddParameter(":1", dgvListOthers[0, intRow].Value.ToString().Trim());
                        result.AddParameter(":2", StringUtilities.HandleApostrophe(dgvListOthers[1, intRow].Value.ToString().Trim()));
                        result.AddParameter(":3", dgvListOthers[2, intRow].Value.ToString().Trim());
                        result.AddParameter(":4", dgvListOthers[3, intRow].Value.ToString().Trim());
                        result.AddParameter(":5", dgvListOthers[4, intRow].Value.ToString().Trim());
                        result.AddParameter(":6", dgvListOthers[5, intRow].Value.ToString().Trim());
                        result.AddParameter(":7", m_strRevYear);
                        result.AddParameter(":8", cmbBrgy.Text.Trim());
                        result.AddParameter(":9", brgycode.Text);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        result.Query = "insert into barangay_addl_sched (fees_code, data_type, amount, rev_year,brgy_nm) values(:1,:2,:3,:4,:5)";
                        result.AddParameter(":1", dgvListOthers[0, intRow].Value.ToString().Trim());
                        result.AddParameter(":2", dgvListOthers[2, intRow].Value.ToString().Trim());
                        result.AddParameter(":3", dgvListOthers[6, intRow].Value);
                        result.AddParameter(":4", m_strRevYear);
                        result.AddParameter(":5", cmbBrgy.Text);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                }
                //peb20191227(e)


                result2.Query = "select * from barangay_taxandfeestable where barangay='" + cmbBrgy.Text.ToUpper() + "'";
                if (result2.Execute())
                {
                    while (result2.Read())
                    {

                     result.Query = string.Format("delete from barangay_taxandfeestable where FEES_CODE = '"+result2.GetString("FEES_CODE")+"' and barangay = '{0}'", cmbBrgy.Text);
                      if (result.ExecuteNonQuery() == 0)
                     {
                     }

                        result.Query = string.Format("delete from barangay_addl_sched where FEES_CODE = '" + result2.GetString("FEES_CODE") + "' and brgy_nm = '{0}'", cmbBrgy.Text);
                         if (result.ExecuteNonQuery() == 0)
                         {
                         }
                     }
                }   
            result2.Close();
                for (int intRow = 0; intRow <= dgvListOthers.Rows.Count - 1; intRow++)
                {
                    string strAmount = string.Empty;
                    double dblAmount = 0;

                    try
                    {
                        strAmount = dgvListOthers[6, intRow].Value.ToString().Trim();
                    }
                    catch
                    {
                        strAmount = "0";
                    }

                    double.TryParse(strAmount, out dblAmount);
                    strAmount = string.Format("{0:##.00}", dblAmount);

                    if (dgvListOthers[2, intRow].Value.ToString().Trim() == "O")
                        strAmount = "";

                    if (dgvListOthers[0, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[1, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[2, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[3, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[4, intRow].Value.ToString().Trim() != "" && 
                        dgvListOthers[5, intRow].Value.ToString().Trim() != "")
                    {
                        result.Query = "insert into barangay_taxandfeestable (fees_code, fees_desc, fees_type, fees_term, fees_withsurch, fees_withpen, rev_year,BARANGAY,brgy_code) values (:1,:2,:3,:4,:5,:6,:7,:8,:9)";
                        result.AddParameter(":1", dgvListOthers[0, intRow].Value.ToString().Trim());
                        result.AddParameter(":2", StringUtilities.HandleApostrophe(dgvListOthers[1, intRow].Value.ToString().Trim()));
                        result.AddParameter(":3", dgvListOthers[2, intRow].Value.ToString().Trim());
                        result.AddParameter(":4", dgvListOthers[3, intRow].Value.ToString().Trim());
                        result.AddParameter(":5", dgvListOthers[4, intRow].Value.ToString().Trim());
                        result.AddParameter(":6", dgvListOthers[5, intRow].Value.ToString().Trim());
                        result.AddParameter(":7", m_strRevYear);
                        result.AddParameter(":8", cmbBrgy.Text.Trim());
                        result.AddParameter(":9", brgycode.Text);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        result.Query = "insert into barangay_addl_sched (fees_code, data_type, amount, rev_year,brgy_nm) values(:1,:2,:3,:4,:5)";
                        result.AddParameter(":1", dgvListOthers[0, intRow].Value.ToString().Trim());
                        result.AddParameter(":2", dgvListOthers[2, intRow].Value.ToString().Trim());
                        result.AddParameter(":3", dgvListOthers[6, intRow].Value);
                        result.AddParameter(":4", m_strRevYear);
                        result.AddParameter(":5", cmbBrgy.Text);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        string strObj = dgvListOthers[0, intRow].Value.ToString().Trim();

                        if (AuditTrail.InsertTrail("AUTA", "addl_sched/tax_and_fees_table", StringUtilities.HandleApostrophe(strObj)) == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                this.brgyloadlist();
             
                this.btnClose.Text = "&Close";

               // this.btnEdit.Enabled = false;

              //  btned.Enabled = false;

            }
            catch
            {
                MessageBox.Show("Incomplete data.", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)//peb 20191204
        {
            if (moduletype == "Brgy")
            {
                if (MessageBox.Show("Save Schedule for Additional Charges?", "Other Charges", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    this.savebrgyChrgs();
                else
                {

                }
            }
            else
            {

                if (chkAddl.Checked == true)
                {
                    if (MessageBox.Show("Save Schedule for Additional Charges?", "Other Charges", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        this.SaveAddlChrgs();
                }
                else
                {
                    if (MessageBox.Show("Save New Schedule for Fire Safety Inspection Fee?", "Other Charges", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        this.SaveCheckList();

                }
                
            }
            //dgvListOthers.Enabled = false;
            btned.Enabled = true;
            btnEdit.Enabled = false;
          
            
        }

        private void SaveAddlChrgs()
        {
            OracleResultSet result = new OracleResultSet();

            try
            {
                for (int intRow = 0; intRow <= dgvListOthers.Rows.Count - 1; intRow++)
                {
                    if (dgvListOthers[0, intRow].Value == null || dgvListOthers[0, intRow].Value.ToString() == "")
                        dgvListOthers.Rows.RemoveAt(intRow);
                }

                for (int intRow = 0; intRow <= dgvListOthers.Rows.Count - 1; intRow++)
                {
                    if (dgvListOthers[2, intRow].Value.ToString().Trim() == "O")
                    {
                        if (dgvListOthers[0, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[1, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[2, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[3, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[4, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[5, intRow].Value.ToString().Trim() != "")
                        {
                            continue;
                        }
                        else
                        {
                            MessageBox.Show("Incomplete data.", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    else
                    {
                        if (dgvListOthers[0, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[1, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[2, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[3, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[4, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[5, intRow].Value.ToString().Trim() != "" &&
                            dgvListOthers[6, intRow].Value.ToString().Trim() != "")
                        {
                            continue;
                        }
                        else
                        {
                            MessageBox.Show("Incomplete data.", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }

                    }
                }

                result.Query = string.Format("delete from tax_and_fees_table where fees_type = 'AD' and rev_year = '{0}'", m_strRevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = string.Format("delete from addl_sched where rev_year = '{0}'", m_strRevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                for (int intRow = 0; intRow <= dgvListOthers.Rows.Count - 1; intRow++)
                {
                    string strAmount = string.Empty;
                    double dblAmount = 0;

                    try
                    {
                        strAmount = dgvListOthers[6, intRow].Value.ToString().Trim();
                    }
                    catch
                    {
                        strAmount = "0";
                    }

                    double.TryParse(strAmount, out dblAmount);
                    strAmount = string.Format("{0:##.00}", dblAmount);

                    if (dgvListOthers[2, intRow].Value.ToString().Trim() == "O")
                        strAmount = "";

                    if (dgvListOthers[0, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[1, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[2, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[3, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[4, intRow].Value.ToString().Trim() != "" &&
                        dgvListOthers[5, intRow].Value.ToString().Trim() != "")
                    {
                        result.Query = "insert into tax_and_fees_table (fees_code, fees_desc, fees_type, fees_term, fees_withsurch, fees_withpen, rev_year) values (:1,:2,:3,:4,:5,:6,:7) ";
                        result.AddParameter(":1", dgvListOthers[0, intRow].Value.ToString().Trim());
                        result.AddParameter(":2", StringUtilities.HandleApostrophe(dgvListOthers[1, intRow].Value.ToString().Trim()));
                        result.AddParameter(":3", "AD");
                        result.AddParameter(":4", dgvListOthers[3, intRow].Value.ToString().Trim());
                        result.AddParameter(":5", dgvListOthers[4, intRow].Value.ToString().Trim());
                        result.AddParameter(":6", dgvListOthers[5, intRow].Value.ToString().Trim());
                        result.AddParameter(":7", m_strRevYear);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        result.Query = "insert into addl_sched (fees_code, data_type, amount, rev_year) values(:1,:2,:3,:4)";
                        result.AddParameter(":1", dgvListOthers[0, intRow].Value.ToString().Trim());
                        result.AddParameter(":2", dgvListOthers[2, intRow].Value.ToString().Trim());
                        result.AddParameter(":3", strAmount);
                        result.AddParameter(":4", m_strRevYear);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        string strObj = dgvListOthers[0, intRow].Value.ToString().Trim();

                        if (AuditTrail.InsertTrail("AUTA", "addl_sched/tax_and_fees_table", StringUtilities.HandleApostrophe(strObj)) == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                this.LoadList();
                this.btnClose.Text = "&Close";

                this.btnEdit.Enabled = false;

                btned.Enabled = false;

            }
            catch
            {
                MessageBox.Show("Incomplete data.", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
		}

        private void dgvListOthers_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string input = "";
           
            string specialChar = @"|#$%&=?»«@£§€{};<>_"; 
            for (int i = 0; i < dgvListOthers.Rows.Count; i++)
            {
                input = Convert.ToString(dgvListOthers.Rows[i].Cells[1].Value);
                char[] str = input.Trim().ToCharArray();
               

                foreach (char item in str)
                {
                    if (item.ToString() == "," && item.ToString() == "!" && item.ToString() == "/" && item.ToString() == "-" && item.ToString() == ":")
                        break;
                    // AFM 20200102 removed
                    //if (item.ToString() == "'")
                    //{
                    //    MessageBox.Show("Special Characters are not accepted");
                    //    return;
                    //}
                    if (char.IsSymbol(item))
                    {
                        MessageBox.Show("Special Characters are not accepted");
                        return;

                    }
                }
            }
            
            

            OracleResultSet result = new OracleResultSet();

            m_strPrevValue = "";

            try
            {
                if (e.ColumnIndex == 6)
                {
                    m_strPrevValue = dgvListOthers[6, e.RowIndex].Value.ToString();
                }

                
            }
            catch { }

         /*   int lastRow = dgvListOthers.Rows.Count - 1;
            if (dgvListOthers.Rows[lastRow].Cells[2].Value == null)
            {
                dgvListOthers.Rows.Add();
            }
            */
          

        }

        private void chkAddl_CheckStateChanged(object sender, EventArgs e)
        {


            if (btnClose.Text == "&Cancel")
            {
                MessageBox.Show("Finish transaction first", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.chkAddl.Checked = false;
                return;
            }

            if (this.chkAddl.CheckState.ToString() == "Checked")
            {
                this.chkFire.Checked = false;
                this.checkBox1.Checked = false;
                this.EnableControls();
                this.LoadList();
            }
        }

        private void chkFire_CheckStateChanged(object sender, EventArgs e)
        {
            if (btnClose.Text == "&Cancel")
            {
                MessageBox.Show("Finish transaction first", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.chkFire.Checked = false;
                return;
            }

            if (this.chkFire.CheckState.ToString() == "Checked")
            {
                this.chkAddl.Checked = false;
                this.checkBox1.Checked = false;
                this.EnableControls();
                this.LoadList();
            }
        }

        private void LoadCheckList()
        {
            if (moduletype != "Brgy")
            {
                OracleResultSet result = new OracleResultSet();
                OracleResultSet result2 = new OracleResultSet();

                string strFeesTerm = string.Empty;
                string strSurch = string.Empty;
                string strPen = string.Empty;

                result.Query = string.Format("select * from tax_and_fees_table where fees_desc = 'FIRE SAFETY INSP FEE' and rev_year = '{0}'", m_strRevYear);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        strFeesTerm = result.GetString("fees_term");
                        strSurch = result.GetString("fees_withsurch");
                        strPen = result.GetString("fees_withpen");
                    }
                    else
                    {
                        strFeesTerm = "F";
                        strSurch = "Y";
                        strPen = "N";
                    }
                }
                result.Close();

                if (strFeesTerm == "Q")
                    chkQtr.Checked = true;
                else
                    chkQtr.Checked = false;

                if (strSurch == "Y")
                    chkSurch.Checked = true;
                else
                    chkSurch.Checked = false;

                if (strPen == "Y")
                    chkInt.Checked = true;
                else
                    chkInt.Checked = false;

                result.Query = string.Format("select * from fire_tax_tag where rev_year = '{0}'", m_strRevYear);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        chkRate.Checked = true;

                        string strValue = string.Empty;

                        for (int intRow = 0; intRow <= dgvListOthers.Rows.Count - 1; intRow++)
                        {
                            strValue = dgvListOthers[1, intRow].Value.ToString().Trim();

                            result2.Query = string.Format("select * from fire_tax_tag where fees_desc = '{0}' and rev_year = '{1}'", StringUtilities.HandleApostrophe(strValue), m_strRevYear);
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    dgvListOthers[0, intRow].Value = true;
                                    txtRate.Text = string.Format("{0:##.00}", result2.GetDouble("fees_rate"));
                                    txtMinFee.Text = string.Format("{0:##.00}", result2.GetDouble("amount")); //AFM 20191209
                                }
                                else
                                    dgvListOthers[0, intRow].Value = false;
                            }
                            result2.Close();
                        }

                    }
                    else
                    {
                        chkRate.Checked = false;
                        txtRate.Enabled = false;

                    }
                }
                result.Close();

                if (chkRate.Checked == true)
                {
                    txtRate.Enabled = true;
                }
                else
                {
                    txtRate.Enabled = false;
                }
            }
        }

        private void chkRate_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkRate.CheckState.ToString() == "Checked")
            {
                txtRate.Enabled = true;
            }
        }

        private void dgvListOthers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.DeleteRow(dgvListOthers.SelectedCells[0].RowIndex);
            }
            
        }

        private void dgvListOthers_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                /*case MouseButtons.Left:
                    MessageBox.Show(this, "Left Button Click");
                    break;*/
                case MouseButtons.Right:
                    DataGridView grid = sender as DataGridView;
                    ContextMenuStrip menu = new ContextMenuStrip();
                    menu.Items.Add("Delete", null, new EventHandler(contextMenuStrip1_Click));
                    Point pt = grid.PointToClient(Control.MousePosition);
                    menu.Show(dgvListOthers, pt.X, pt.Y);
                    break;
                /*case MouseButtons.Middle:
                    break;*/
                default:
                    break;
            }
            
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            this.DeleteRow(dgvListOthers.SelectedCells[0].RowIndex);
        }

        private void DeleteRow(int intRow)
        {
            string strDesc = dgvListOthers[1, intRow].Value.ToString();

            if (this.chkAddl.Checked == true)
            {
                if (MessageBox.Show("Delete entire row of "+ strDesc +"?", "Other Charges", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dgvListOthers.Rows.RemoveAt(intRow);
                    this.btnClose.Text = "&Cancel";
                    this.btnEdit.Enabled = true;
                }
            }
        }

        private void SaveCheckList()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string strValue = string.Empty;
            string tmpFeesCode = string.Empty;
            string strFeesTerm = string.Empty;
            string strSurcharge = string.Empty;
            string strPenalty = string.Empty;
            string strFeesDesc = string.Empty;

            if(chkQtr.Checked == true)
		        strFeesTerm = "Q";
	        else
		        strFeesTerm = "F";

            if(chkSurch.Checked == true)
		        strSurcharge = "Y";
	        else
		        strSurcharge = "N";

            if(chkInt.Checked == true)
		        strPenalty = "Y";
	        else
		        strPenalty = "N";

            try
            {
                strValue = txtRate.Text.ToString();
                strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));
            }
            catch
            {
                MessageBox.Show("Invalid Rate Base", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtRate.Text = "0";
                return;
            }

            bool swFireTax = false;
		    bool swClose_pRSet = false;

            string strRate = txtRate.Text.ToString();
            strRate = string.Format("{0:##0.00}", Convert.ToDouble(strRate));

            //AFM 20191209 MAO-19-11500 (s)
            string strAmount = txtMinFee.Text.ToString();
            strAmount = string.Format("{0:##0.00}", Convert.ToDouble(strAmount));
            //AFM 20191209 MAO-19-11500 (e)

            result.Query = string.Format("select * from fire_tax_tag where rev_year = '{0}'", m_strRevYear);
		    if(result.Execute())
		    {
                if(result.Read())
				{
                    result2.Query = string.Format("delete from fire_tax_tag where rev_year = '{0}'", m_strRevYear);
                    if(result2.ExecuteNonQuery() == 0)
                    {
                    }
				
                    for(int intRow = 0; intRow <= dgvListOthers.Rows.Count-1; intRow++)
					{
				        if((bool) dgvListOthers[0,intRow].Value)
						{
						    strFeesDesc = dgvListOthers[1,intRow].Value.ToString().Trim();

                            if(strFeesDesc == AppSettingsManager.GetConfigValue("17"))
						        tmpFeesCode = "B";
						    else
						    {
						        result2.Query = string.Format("select fees_code from tax_and_fees_table where fees_desc = '{0}' and rev_year = '{1}'", StringUtilities.HandleApostrophe(strFeesDesc), m_strRevYear);
							    if(result2.Execute())
							    {
                                    if(result2.Read())
							        {
							            tmpFeesCode = result2.GetString("fees_code");
                                    }
                                }
                                result2.Close();
                            }

						    result2.Query = "insert into fire_tax_tag (fees_code, fees_desc, fees_rate, rev_year, amount) values (:1,:2,:3,:4,:5)";
                            result2.AddParameter(":1", tmpFeesCode);
                            result2.AddParameter(":2", StringUtilities.HandleApostrophe(strFeesDesc));
                            result2.AddParameter(":3", strRate);
                            result2.AddParameter(":4", m_strRevYear);
                            result2.AddParameter(":5", strAmount); //AFM 20191209 MAO-19-11500
                            if(result2.ExecuteNonQuery() == 0)
                            {
                            }

						    swFireTax = true;

						    if (AuditTrail.InsertTrail("AUTFT", "fire_tax_tag/tax_and_fees_tabl", StringUtilities.HandleApostrophe(tmpFeesCode)) == 0)
                            {
                                result2.Rollback();
                                result2.Close();
                                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
						
					}
			    }
			    else
			    {
                    if (chkRate.Checked == true)
				    {
					    for(int intRow = 0; intRow <= dgvListOthers.Rows.Count-1; intRow++)
					    {
                            strFeesDesc = string.Empty;
                            
                            if((bool) dgvListOthers[0,intRow].Value)
						    {
                                strFeesDesc = dgvListOthers[1,intRow].Value.ToString().Trim();

                                if(strFeesDesc == AppSettingsManager.GetConfigValue("17"))
							        tmpFeesCode = "B";
							    else
							    {
                                    result2.Query = string.Format("select fees_code from tax_and_fees_table where fees_desc = '{0}' and rev_year = '{1}'", StringUtilities.HandleApostrophe(strFeesDesc), m_strRevYear);
								    if(result2.Execute())
								    {
                                        if(result2.Read())
								        {
								            tmpFeesCode = result2.GetString("fees_code");
                                        }
                                    }
                                    result2.Close();
							    }
    							
							    result2.Query = "insert into fire_tax_tag (fees_code, fees_desc, fees_rate, rev_year, amount) values (:1,:2,:3,:4,:5)";
                                result2.AddParameter(":1", tmpFeesCode);
                                result2.AddParameter(":2", StringUtilities.HandleApostrophe(strFeesDesc));
                                result2.AddParameter(":3", strRate);
                                result2.AddParameter(":4", m_strRevYear);
                                result2.AddParameter(":5", strAmount); //AFM 20191209 MAO-19-11500
                                if(result2.ExecuteNonQuery() == 0)
                                {
                                }
							    
                                swFireTax = true;

							    if (AuditTrail.InsertTrail("AUTFT", "fire_tax_tag/tax_and_fees_tabl", StringUtilities.HandleApostrophe(tmpFeesCode)) == 0)
                                {
                                    result2.Rollback();
                                    result2.Close();
                                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
					    }
				    }
                }
			}
            result.Close();
		
			result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FR' and rev_year = '{0}'", m_strRevYear);
		    if(result.Execute())
		    {
                if(result.Read())
				{
                    if (chkRate.Checked == false)
			        {
				        result2.Query = string.Format("delete from tax_and_fees_table where fees_type = 'FR' and rev_year = '{0}'", m_strRevYear);
                        if(result2.ExecuteNonQuery() == 0)
                        {
                        }
				        
				        result2.Query= string.Format("delete from fire_tax_tag where rev_year = '{0}'", m_strRevYear);
                        if(result2.ExecuteNonQuery() == 0)
                        {
                        }

				        swClose_pRSet = false;

                        if (AuditTrail.InsertTrail("AUTFT-D", "fire_tax_tag/tax_and_fees_tabl", StringUtilities.HandleApostrophe(tmpFeesCode)) == 0)
                        {
                            result2.Rollback();
                            result2.Close();
                            MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
				        
			        }
			        else
			        {
				        result2.Query = string.Format("delete from fire_tax_tag where rev_year = '{0}'", m_strRevYear);
				        if(result2.ExecuteNonQuery() == 0)
                        {
                        }

				        for(int intRow = 0; intRow <= dgvListOthers.Rows.Count-1; intRow++)
					    {
                            strFeesDesc = string.Empty;
                            
                            if((bool) dgvListOthers[0,intRow].Value)
					        {
                                strFeesDesc = dgvListOthers[1, intRow].Value.ToString().Trim();

						        if(strFeesDesc == AppSettingsManager.GetConfigValue("17"))
							        tmpFeesCode = "B";
						        else
						        {
							        //result2.Query = string.Format("select fees_code from tax_and_fees_table where fees_desc = '{0}' and rev_year = '{1}'", strFeesDesc, m_strRevYear);
							        result2.Query = string.Format("select fees_code from tax_and_fees_table where fees_desc = '{0}' and rev_year = '{1}'", StringUtilities.HandleApostrophe(strFeesDesc), m_strRevYear);
								    if(result2.Execute())
								    {
                                        if(result2.Read())
								        {
								            tmpFeesCode = result2.GetString("fees_code");
                                        }
                                    }
                                    result2.Close();
						        }

						        result2.Query = "insert into fire_tax_tag (fees_code, fees_desc, fees_rate, rev_year, amount) values (:1,:2,:3,:4,:5)";
                                result2.AddParameter(":1", tmpFeesCode);
                                result2.AddParameter(":2", StringUtilities.HandleApostrophe(strFeesDesc));
                                result2.AddParameter(":3", strRate);
                                result2.AddParameter(":4", m_strRevYear);
                                result2.AddParameter(":5", strAmount); //AFM 20191209 MAO-19-11500
                                if(result2.ExecuteNonQuery() == 0)
                                {
                                }

						        if (AuditTrail.InsertTrail("AUTFT", "fire_tax_tag/tax_and_fees_tabl", StringUtilities.HandleApostrophe(tmpFeesCode)) == 0)
                                {
                                    result2.Rollback();
                                    result2.Close();
                                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
					    }
				
				        swClose_pRSet = true;

				        result2.Query = "update tax_and_fees_table set fees_term = :1, fees_withsurch = :2, fees_withpen = :3 where fees_type = 'FR' and rev_year = :4";
                        result2.AddParameter(":1", strFeesTerm);
                        result2.AddParameter(":2", strSurcharge);
                        result2.AddParameter(":3", strPenalty);
                        result2.AddParameter(":4", m_strRevYear);
                        if(result2.ExecuteNonQuery() == 0)
                        {
                        }
                    }   
				     
                }
                else
                {
			        if (chkRate.Checked == true)
			        {
				        result2.Query = string.Format("select fees_code from tax_and_fees_table where rev_year = '{0}' order by fees_code desc", m_strRevYear);
				        if(result2.Execute())
                        {
				            if(result2.Read())
					            tmpFeesCode = result2.GetString("fees_code");
				            else
					            tmpFeesCode = "0";
                        }
                        result2.Close();

				        int iFeesCode = Convert.ToInt32(tmpFeesCode) + 1;
				        tmpFeesCode = string.Format("{0:##0}", iFeesCode);
				
				        if(swFireTax)
				        {
					        result2.Query = "insert into tax_and_fees_table (fees_code, fees_desc, fees_type, fees_term, fees_withsurch, fees_withpen, rev_year) values (:1,:2,:3,:4,:5,:6,:7)";
                            result2.AddParameter(":1", tmpFeesCode);
                            result2.AddParameter(":2", "FIRE SAFETY INSP FEE");
                            result2.AddParameter(":3", "FR");
                            result2.AddParameter(":4", strFeesTerm);
                            result2.AddParameter(":5", strSurcharge);
                            result2.AddParameter(":6", strPenalty);
                            result2.AddParameter(":7", m_strRevYear);
						    if(result2.ExecuteNonQuery() == 0)
                            {
                            }

                            if (AuditTrail.InsertTrail("AUTFT", "fire_tax_tag/tax_and_fees_tabl", StringUtilities.HandleApostrophe(tmpFeesCode)) == 0)
                            {
                                result.Rollback();
                                result.Close();
                                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                        }
                    }
				}
			}
            result.Close();
		

		    result.Query = string.Format("select * from fire_tax_tag where rev_year = '{0}'", m_strRevYear);
		    if(result.Execute())
            {
                if(result.Read())
                {
                }
                else
		        {
			        result2.Query = string.Format("delete from tax_and_fees_table where fees_type = 'FR' and rev_year = '{0}'", m_strRevYear);
			        if(result2.ExecuteNonQuery() == 0)
                    {
                    }
                }
            }

            this.LoadList();
            this.btnClose.Text = "&Close";
            this.btnEdit.Enabled = false;
		}


        private void chkQtr_CheckedChanged(object sender, EventArgs e)
        {
        
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbBrgy_SelectedIndexChanged(object sender, EventArgs e)//peb 20191204
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string strCode = string.Empty;
            string strDesc = string.Empty;
            string strTerm = string.Empty;
            string strSurch = string.Empty;
            string strPen = string.Empty;
            string strType = string.Empty;
            double dblAmount = 0;

            m_bSwFeesCode = true;
            if (this.chkAddl.Checked == true)
            {
                dgvListOthers.Columns.Clear();
                dgvListOthers.Columns.Add("CODE", "Code");
                dgvListOthers.Columns.Add("NAME", "Name");
                //dgvListOthers.Columns.Add("TYPE", "Type");
                //dgvListOthers.Columns.Add("TERM", "Term");
                //dgvListOthers.Columns.Add("SURCH", "Surch");
                //dgvListOthers.Columns.Add("INT", "Int");
                dgvListOthers.Columns.Add("AMT", "Amount");

                DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
                comboBox.HeaderCell.Value = "Type";
                dgvListOthers.Columns.Insert(2, comboBox);
                comboBox.Items.AddRange("F", "Q", "O");

                DataGridViewComboBoxColumn comboBox2 = new DataGridViewComboBoxColumn();
                comboBox2.HeaderCell.Value = "Term";
                dgvListOthers.Columns.Insert(3, comboBox2);
                comboBox2.Items.AddRange("Q", "F");

                DataGridViewComboBoxColumn comboBox3 = new DataGridViewComboBoxColumn();
                comboBox3.HeaderCell.Value = "Surch";
                dgvListOthers.Columns.Insert(4, comboBox3);
                comboBox3.Items.AddRange("Y", "N");

                DataGridViewComboBoxColumn comboBox4 = new DataGridViewComboBoxColumn();
                comboBox4.HeaderCell.Value = "Int";
                dgvListOthers.Columns.Insert(5, comboBox4);
                comboBox4.Items.AddRange("Y", "N");

                dgvListOthers.RowHeadersVisible = false;
                dgvListOthers.Columns[0].Width = 40;
                dgvListOthers.Columns[1].Width = 200;
                dgvListOthers.Columns[2].Width = 40;
                dgvListOthers.Columns[3].Width = 40;
                dgvListOthers.Columns[4].Width = 40;
                dgvListOthers.Columns[5].Width = 40;
                dgvListOthers.Columns[6].Width = 80;

                dgvListOthers.Columns[0].ReadOnly = true;
                dgvListOthers.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                dgvListOthers.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                brgyloadlist();

                result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'AD' and rev_year = '{0}' order by fees_code", m_strRevYear);
                if (result.Execute())
                {
                    int intRow = 0;
                    while (result.Read())
                    {
                        dgvListOthers.Rows.Add("");

                        strCode = result.GetString("fees_code").Trim();
                        strDesc = result.GetString("fees_desc").Trim();
                        strTerm = result.GetString("fees_term").Trim();
                        strSurch = result.GetString("fees_withsurch").Trim();
                        strPen = result.GetString("fees_withpen").Trim();

                        strType = "";
                        dblAmount = 0;

                        result2.Query = string.Format("select * from addl_sched where fees_code = '{0}' and rev_year = '{1}'", strCode, m_strRevYear);
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                strType = result2.GetString("data_type");
                                if (strType != "O")
                                    dblAmount = result2.GetDouble("amount");
                            }
                        }
                        result2.Close();

                        dgvListOthers[0, intRow].Value = strCode;
                        dgvListOthers[1, intRow].Value = strDesc;
                        dgvListOthers[2, intRow].Value = strType;
                        dgvListOthers[3, intRow].Value = strTerm;
                        dgvListOthers[4, intRow].Value = strSurch;
                        dgvListOthers[5, intRow].Value = strPen;
                        if (strType == "O")
                            dgvListOthers[6, intRow].Value = "";
                        else
                            dgvListOthers[6, intRow].Value = string.Format("{0:#0.00}", dblAmount);

                        intRow++;
                    }
                    dgvListOthers.Rows.Add("");
                }
                result.Close();

            }
            else
            {
                //dgvListOthers.Columns.Clear();
                //dgvListOthers.Columns.Add(new DataGridViewCheckBoxColumn());
                //dgvListOthers.Columns.Add("FIRE", "Fire Safety Inspection Fee");

                //dgvListOthers.RowHeadersVisible = false;
                //dgvListOthers.Columns[0].Width = 30;
                //dgvListOthers.Columns[1].Width = 400;

                //result.Query = "delete from fire_tax_table";
                //if (result.ExecuteNonQuery() == 0)
                //{
                //}

                //result.Query = "insert into fire_tax_table (description) values (:1)";
                //result.AddParameter(":1", AppSettingsManager.GetConfigValue("17"));
                //if (result.ExecuteNonQuery() == 0)
                //{
                //}

                //result.Query = string.Format("insert into fire_tax_table (select distinct(fees_desc) from tax_and_fees_table where rev_year = '{0}' and fees_type <> 'FR')", m_strRevYear);
                //if (result.ExecuteNonQuery() == 0)
                //{
                //}

                //result.Query = "select * from fire_tax_table order by description";
                //if (result.Execute())
                //{
                //    int intRow = 0;
                //    while (result.Read())
                //    {
                //        dgvListOthers.Rows.Add("");
                //        dgvListOthers[0, intRow].Value = false;
                //        dgvListOthers[1, intRow].Value = result.GetString("description");

                //        intRow++;
                //    }
                //}
                //result.Close();

                this.LoadCheckList();
            }
        }

        private void cmbBrgy_SelectedValueChanged(object sender, EventArgs e)//peb 20191204
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select brgy_code from brgy where brgy_nm='" + cmbBrgy.Text.ToUpper() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    brgycode.Text = result.GetString("brgy_code");
                }
            }
            result.Close();

            btned.Enabled = true;
            brgyloadlist();
          //  dgvListOthers.Enabled = true;
        }

        private void frmScheduleOthers_Click(object sender, EventArgs e)
        {

        }

        private void btnedclick(object sender, EventArgs e) //peb 20191204
        { 
              // dgvListOthers.Enabled = true;
               btnEdit.Enabled =true;
               dgvListOthers.ReadOnly = false;
                return;
            
        }
        private void txtMinFee_Enter(object sender, EventArgs e)
        {
            this.btnEdit.Enabled = true;
            this.btnClose.Text = "&Cancel";
        }

        private void txtMinFee_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkCTC_CheckedChanged(object sender, EventArgs e)
        {
            if (btnClose.Text == "&Cancel")
            {
                MessageBox.Show("Finish transaction first", "Other Charges", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.checkBox1.Checked = false;
                return;
            }

            if (this.checkBox1.Checked == true)
            {
                this.chkFire.Checked = false;
                this.chkAddl.Checked = false;
                this.EnableControls();
                this.LoadList();
            }
        }
        
    }
}