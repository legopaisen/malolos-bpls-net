
// RMC 20151222 Created GR module

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
using Amellar.Common.AppSettings;

namespace Amellar.Modules.Utilities
{
    public partial class frmRevYear : Form
    {
        private string m_sPrevRevYear = string.Empty;
        private string m_sRevYearTo = string.Empty;

        public frmRevYear()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Add")
            {
                btnAdd.Text = "Save";
                btnCancel.Text = "Cancel";
                btnEdit.Enabled = false;
                txtRevYear.ReadOnly = false;
                txtRevYearTo.ReadOnly = false;
                txtOrdinance.ReadOnly = false;
                txtRevYear.Focus();
            }
            else
            {
                if (!ValidateRevYear("Add"))
                {
                    btnTax.Enabled = false;
                    btnFees.Enabled = false;
                    btnExempt.Enabled = false;
                    btnDefault.Enabled = false;
                    
                    return;
                }

                if (txtRevYear.Text.Trim() == "" || txtRevYearTo.Text.Trim() == "")
                {
                    MessageBox.Show("Effectivity year/s are required","Revenue Year",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show("Save new Revenue Year?", "GR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    btnTax.Enabled = false;
                    btnFees.Enabled = false;
                    btnExempt.Enabled = false;
                    btnDefault.Enabled = false;
                    txtRevYear.ReadOnly = true;
                    txtRevYearTo.ReadOnly = true;
                    txtOrdinance.ReadOnly = true;

                    return;
                }
                else
                {
                    OracleResultSet pSet = new OracleResultSet();

                    pSet.Query = "insert into gr_year values (";
                    pSet.Query += "'" + StringUtilities.HandleApostrophe(txtRevYear.Text.Trim()) + "',";
                    pSet.Query += "'" + StringUtilities.HandleApostrophe(txtRevYearTo.Text.Trim()) + "',";
                    pSet.Query += "'" + StringUtilities.HandleApostrophe(txtOrdinance.Text.Trim()) + "')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }
                       
                    btnTax.Enabled = true;
                    btnFees.Enabled = true;
                    btnExempt.Enabled = true;
                    btnDefault.Enabled = true;

                    String sObj = txtRevYear.Text.Trim() + "-" + txtRevYearTo.Text.Trim();

                    if (AuditTrail.InsertTrail("AUTGR-RA", "gr_year", StringUtilities.HandleApostrophe(sObj)) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    GetPrevRevYear();
                }

                btnAdd.Text = "Add";
                btnCancel.Text = "Close";
                btnEdit.Enabled = true;

                txtRevYear.ReadOnly = true;
                txtRevYearTo.ReadOnly = true;
                txtOrdinance.ReadOnly = true;
                m_sRevYearTo = "";

                UpdateList();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Cancel")
            {
                btnAdd.Text = "Add";
                btnEdit.Text = "Edit";
                btnCancel.Text = "Close";
                
                txtRevYear.ReadOnly = true;
                txtRevYearTo.ReadOnly = true;
                txtOrdinance.ReadOnly = true;
                btnTax.Enabled = false;
                btnFees.Enabled = false;
                btnExempt.Enabled = false;
                btnDefault.Enabled = false;
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                m_sRevYearTo = "";
            }
            else
                this.Close();
        }

        private void frmRevYear_Load(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            m_sPrevRevYear = "";
            string sRevYearTo = "";
            int iTmp = 0;
            int.TryParse(ConfigurationAttributes.RevYear, out iTmp);
            iTmp = iTmp + 5;    // temp initial value to set year_to to 5 yrs, this can be editable
            sRevYearTo = string.Format("{0:####}",iTmp);

            pSet.Query = "select * from tabs where table_name = 'GR_YEAR'";
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = "create table gr_year(rev_year varchar2(4),rev_year_to varchar2(4),rev_ord varchar2(200))";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "insert into gr_year values (";
                    pSet.Query += "'" + ConfigurationAttributes.RevYear + "','" + sRevYearTo + "','')";   
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "INSERT INTO TRAIL_TABLE VALUES ('AUTGR-RA','ASS-UTIL-TABLES-GR-ADDED REVENUE YEAR')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "INSERT INTO TRAIL_TABLE VALUES ('AUTGR-RE','ASS-UTIL-TABLES-GR-EDITED REVENUE YEAR')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "INSERT INTO TRAIL_TABLE VALUES ('AUTGR-TA','ASS-UTIL-TABLES-GR-PRELOADED TAX VALUE')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "INSERT INTO TRAIL_TABLE VALUES ('AUTGR-FA','ASS-UTIL-TABLES-GR-PRELOADED FEES VALUE')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "INSERT INTO TRAIL_TABLE VALUES ('AUTGR-EA','ASS-UTIL-TABLES-GR-PRELOADED EXEMPTION VALUE')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "INSERT INTO TRAIL_TABLE VALUES ('AUTGR-DA','ASS-UTIL-TABLES-GR-PRELOADED SET DEFAULT VALUE')";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }
                }
            }
            pSet.Close();

            UpdateList();
        }

        private void UpdateList()
        {
            dgvRevYear.Columns.Clear();
            dgvRevYear.Columns.Add("YEAR", "YEAR FR");
            dgvRevYear.Columns.Add("YEARTO", "YEAR TO");
            dgvRevYear.Columns.Add("ORD", "ORDINANCE");

            dgvRevYear.RowHeadersVisible = false;
            dgvRevYear.Columns[0].Width = 100;
            dgvRevYear.Columns[1].Width = 100;
            dgvRevYear.Columns[2].Width = 200;

            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from gr_year order by rev_year";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dgvRevYear.Rows.Add(pSet.GetString("rev_year"), pSet.GetString("rev_year_to"), pSet.GetString("rev_ord"));
                }
            }
            pSet.Close();
        }

        private bool ValidateRevYear(string sTrans)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from gr_year where '" + StringUtilities.HandleApostrophe(txtRevYear.Text.Trim()) + "' between rev_year and rev_year_to";
            if (sTrans == "Edit")
                pSet.Query += " and rev_year_to <> '" + m_sRevYearTo + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("Revenue year already exists","GR",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return false;
                }
            }
            pSet.Close();

            return true;
        }

        private void btnTax_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pCmd = new OracleResultSet();

            if (MessageBox.Show("Preload tax values from previous revenue code?", "GR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sMess = string.Empty;
                int iCtr = 0;

                //checking
                pSet.Query = "select * from bns_table where fees_code = 'B' and rev_year = '" + txtRevYear.Text.Trim() + "' order by fees_code, bns_code";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Tax schedule for revenue year " + txtRevYear.Text.Trim() + " already been preloaded", "GR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pSet.Close();
                        btnTax.Enabled = false;
                        return;
                    }
                }
                pSet.Close();
                    
                //insertion
                
                pSet.Query = "select * from bns_table where fees_code = 'B' and rev_year = '" + m_sPrevRevYear + "' order by fees_code, bns_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into bns_table values ('B', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(pSet.GetString("bns_code")) + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(pSet.GetString("bns_desc")) + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(pSet.GetString("means")) + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;

                        if (AuditTrail.InsertTrail("AUTGR-TA", "multiple tables", StringUtilities.HandleApostrophe(pSet.GetString("bns_desc"))) == 0)
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess = "Business Type inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from btax_sched where rev_year = '" + m_sPrevRevYear + "' order by bns_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        
                        pCmd.Query = "insert into btax_sched values (";
                        pCmd.Query += "'" + pSet.GetString("bns_code") + "', ";
                        pCmd.Query += " " + pSet.GetDouble("gr_1") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("gr_2") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("ex_rate") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("plus_rate") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("amount") + ", ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "') ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Tax values inserted: " + iCtr.ToString();
                }
                
                iCtr = 0;
                pSet.Query = "select * from excess_sched  where rev_year = '" + m_sPrevRevYear + "' and fees_code = 'B' order by fees_code, bns_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into excess_sched values (";
                        pCmd.Query += "'" + pSet.GetString("fees_code") + "', ";
                        pCmd.Query += "'" + pSet.GetString("bns_code") + "', ";
                        pCmd.Query += "'" + pSet.GetDouble("excess_no") + "', ";
                        pCmd.Query += "'" + pSet.GetDouble("excess_amt") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "') ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Excess schedule inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from fix_sched where rev_year = '" + m_sPrevRevYear + "' order by bns_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into fix_sched values (";
                        pCmd.Query += "'" + pSet.GetString("BNS_CODE") + "',";
                        pCmd.Query += "'" + pSet.GetString("FIX_NAME") + "', ";
                        pCmd.Query += " " + pSet.GetInt("QTY1") + ", ";
                        pCmd.Query += " " + pSet.GetInt("QTY2") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("AREA1") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("AREA2") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("FIX_AMOUNT") + ", ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "') ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Fix schedule inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from new_table where rev_year = '" + m_sPrevRevYear + "' order by bns_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into new_table values (";
                        pCmd.Query += "'" + pSet.GetString("bns_code") + "', ";
                        pCmd.Query += " " + pSet.GetDouble("new_rate") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("min_tax") + ", ";
                        pCmd.Query += "'" + pSet.GetString("is_qtrly") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "') ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "New table inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from surch_sched where tax_fees_code like 'B%' and rev_year = '" + m_sPrevRevYear + "' order by tax_fees_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into surch_sched values (";
                        pCmd.Query += " " + pSet.GetDouble("SURCH_RATE") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("PEN_RATE") + ", ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "', ";
                        pCmd.Query += "'" + pSet.GetString("TAX_FEES_CODE") + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Surcharge value inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from discount_tbl where rev_year = '" + m_sPrevRevYear + "'";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into discount_tbl values (";
                        pCmd.Query += " " + pSet.GetDouble("DISCOUNT_RATE") + ", ";
                        pCmd.Query += "'" + pSet.GetString("TERM") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "') ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Discount value inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from minmax_tax_table where fees_code like 'B%' and rev_year = '" + m_sPrevRevYear + "' order by fees_code, bns_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into minmax_tax_table values (";
                        pCmd.Query += "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("BNS_CODE") + "', ";
                        pCmd.Query += " " + pSet.GetDouble("MIN_TAX") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("MAX_TAX") + ", ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "', ";
                        pCmd.Query += "'" + pSet.GetString("DATA_TYPE") + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Min/Max value inserted: " + iCtr.ToString();
                }

                if (sMess != "")
                {
                    MessageBox.Show(sMess,"GR",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    btnTax.Enabled = false;
                }
            }
        }

        private void GetPrevRevYear()
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from gr_year where rev_year <> '" + txtRevYear.Text.Trim() + "' order by rev_year desc";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sPrevRevYear = pSet.GetString("rev_year");
                }
            }
            pSet.Close();
            
        }

        private void btnFees_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pCmd = new OracleResultSet();

            if (MessageBox.Show("Preload fees & other charges from previous revenue code?", "GR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sMess = string.Empty;
                int iCtr = 0;

                //checking
                pSet.Query = "select * from tax_and_fees_table where rev_year = '" + txtRevYear.Text.Trim() + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Fees & Other Charges schedule for revenue year " + txtRevYear.Text.Trim() + " already been preloaded", "GR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pSet.Close();
                        btnFees.Enabled = false;
                        return;
                    }
                }
                pSet.Close();

                //insertion
                pSet.Query = "select * from tax_and_fees_table where rev_year = '" + m_sPrevRevYear + "' order by fees_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into tax_and_fees_table values (";
                        pCmd.Query += "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(pSet.GetString("FEES_DESC")) + "', ";
                        pCmd.Query += "'" + pSet.GetString("FEES_TYPE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("FEES_TERM") + "', ";
                        pCmd.Query += "'" + pSet.GetString("FEES_WITHSURCH") + "', ";
                        pCmd.Query += "'" + pSet.GetString("FEES_WITHPEN") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "')"; 
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;

                        if (AuditTrail.InsertTrail("AUTGR-FA", "multiple tables", StringUtilities.HandleApostrophe(pSet.GetString("FEES_DESC"))) == 0)
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess = "Fees inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from excess_sched where fees_code <> 'B' and rev_year = '" + m_sPrevRevYear + "'order by fees_code, bns_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into excess_sched values (";
                        pCmd.Query += "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("BNS_CODE") + "', ";
                        pCmd.Query += " " + pSet.GetDouble("EXCESS_NO") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("EXCESS_AMT") + ", ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Excess Schedule inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from bns_table where fees_code <> 'B' and rev_year = '" + m_sPrevRevYear + "' order by fees_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into bns_table values (";
                        pCmd.Query += "'" + pSet.GetString("fees_code") + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(pSet.GetString("bns_code")) + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(pSet.GetString("bns_desc")) + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(pSet.GetString("means")) + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Fees Type inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from fees_sched where rev_year = '" + m_sPrevRevYear + "' order by fees_code, bns_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into fees_sched values (";
                        pCmd.Query += "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("BNS_CODE") + "', ";
                        pCmd.Query += " " + pSet.GetInt("QTY1") + ", ";
                        pCmd.Query += " " + pSet.GetInt("QTY2") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("AREA1") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("AREA2") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("GR_1") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("GR_2") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("EX_RATE") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("PLUS_RATE") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("AMOUNT") + ", ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Fees schedule inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from rate_config_tbl where rev_year = '" + m_sPrevRevYear + "' order by fees_code";
                if(pSet.Execute())
                {
                    while(pSet.Read())
                    {
                        pCmd.Query = "insert into rate_config_tbl values (";
                        pCmd.Query += "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("DET_BUSS_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("CONFIG_CODE") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "', ";
                        pCmd.Query += "'" + pSet.GetString("CONFIG_SWITCH") + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Rate Config inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from rate_config_tbl_ref where rev_year = '" + m_sPrevRevYear + "' order by fees_code, det_buss_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into rate_config_tbl_ref values (";
                        pCmd.Query += "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("DET_BUSS_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("TAX_BASE_CODE") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Rate Config reference inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from surch_sched where tax_fees_code not like 'B%' and rev_year = '" +m_sPrevRevYear+"' order by tax_fees_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into surch_sched values (";
                        pCmd.Query += " " + pSet.GetDouble("SURCH_RATE") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("PEN_RATE") + ", ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "', ";
                        pCmd.Query += "'" + pSet.GetString("TAX_FEES_CODE") + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Surcharge schedule inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from minmax_tax_table where fees_code not like 'B%' and rev_year = '" + m_sPrevRevYear + "' order by fees_code, bns_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into minmax_tax_table values (";
                        pCmd.Query+= "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("BNS_CODE") + "', ";
                        pCmd.Query += " " + pSet.GetInt("MIN_TAX") + ", ";
                        pCmd.Query += " " + pSet.GetInt("MAX_TAX") + ", ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "', ";
                        pCmd.Query += "'" + pSet.GetString("DATA_TYPE") + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Min./Max schedule inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from qtr_fee_config where rev_year = '" + m_sPrevRevYear + "' order by fees_code";
                if (pSet.Execute())
                {
                    while(pSet.Read())
                    {
                        pCmd.Query = "insert into qtr_fee_config values (";
                        pCmd.Query += "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("DET_BUSS_CODE") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "', ";
                        pCmd.Query += "'" + pSet.GetString("DATA_TYPE") + "', ";
                        pCmd.Query += " " + pSet.GetDouble("GR_1") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("GR_2") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("AMOUNT") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("AMOUNT2") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("AMOUNT3") + ", ";
                        pCmd.Query += " " + pSet.GetDouble("AMOUNT4") + ", ";
                        pCmd.Query += "'" + pSet.GetString("COM_BUSS_CODE") + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Quarter configuration inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from default_others where rev_year = '" + m_sPrevRevYear + "' order by fees_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into default_others values (";
                        pCmd.Query += "'" + pSet.GetString("DEFAULT_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("DEFAULT_FEE") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "') ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Default inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from default_code where rev_year = '" + m_sPrevRevYear + "' order by default_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into default_code values (";
                        pCmd.Query += "'" + pSet.GetString("DEFAULT_CODE") + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(pSet.GetString("DEFAULT_DESC")) + "', ";
                        pCmd.Query += "'" + pSet.GetString("DATA_TYPE") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "') ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Default code inserted: " + iCtr.ToString();
                }

                iCtr = 0;
                pSet.Query = "select * from addl_sched where rev_year = '" + m_sPrevRevYear + "'";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into addl_sched values (";
                        pCmd.Query += "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("DATA_TYPE") + "', ";
                        pCmd.Query += " " + pSet.GetDouble("AMOUNT") + ", ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "') ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                        iCtr++;
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess += "\n" + "Addl sched inserted: " + iCtr.ToString();
                }
                

                if (sMess != "")
                {
                    MessageBox.Show(sMess, "GR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnFees.Enabled = false;
                }
            }
        }

        private void btnExempt_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pCmd = new OracleResultSet();

            if (MessageBox.Show("Preload exemption from previous revenue code?", "GR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sMess = string.Empty;
                int iCtr = 0;

                //checking
                pSet.Query = "select * from exempted_bns where rev_year = '" + txtRevYear.Text.Trim() + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Exemption for revenue year " + txtRevYear.Text.Trim() + " already been preloaded", "GR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pSet.Close();
                        btnExempt.Enabled = false;
                        return;
                    }
                }
                pSet.Close();

                pSet.Query = "select * from exempted_bns where rev_year = '" + m_sPrevRevYear + "' order by fees_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into exempted_bns values (";
                        pCmd.Query+= "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("BNS_CODE") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "') ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                        iCtr++;

                        string sObj = "F: " + pSet.GetString("FEES_CODE");
                        sObj += "/B: " + pSet.GetString("BNS_CODE");

                        if (AuditTrail.InsertTrail("AUTGR-EA", "exempted_bns", StringUtilities.HandleApostrophe(sObj)) == 0)
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess = "Exemption inserted: " + iCtr.ToString();
                }

                if (sMess != "")
                {
                    MessageBox.Show(sMess, "GR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnExempt.Enabled = false;
                }

                

            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pCmd = new OracleResultSet();

            if (MessageBox.Show("Preload Set Default from previous revenue code?", "GR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sMess = string.Empty;
                int iCtr = 0;

                //checking
                pSet.Query = "select * from default_table where rev_year = '" + txtRevYear.Text.Trim() + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Set Default for revenue year " + txtRevYear.Text.Trim() + " already been preloaded", "GR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pSet.Close();
                        btnDefault.Enabled = false;
                        return;
                    }
                }
                pSet.Close();

                pSet.Query = "select * from default_table where rev_year = '" + m_sPrevRevYear + "' order by fees_code, bns_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        pCmd.Query = "insert into default_table values (";
                        pCmd.Query += "'" + pSet.GetString("FEES_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("BNS_CODE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("DEFAULT_FEE") + "', ";
                        pCmd.Query += "'" + pSet.GetString("STAT_COVER") + "', ";
                        pCmd.Query += "'" + txtRevYear.Text.Trim() + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(pSet.GetString("REMARKS")) + "', ";
                        pCmd.Query += "'" + pSet.GetString("IS_DEFAULT") + "')";  
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                        iCtr++;

                        string sObj = "F: " + pSet.GetString("FEES_CODE");
                        sObj += "/B: " + pSet.GetString("BNS_CODE");
                        sObj += "/D: " + pSet.GetString("DEFAULT_FEE");
                        sObj += "/RY: " + txtRevYear.Text;

                        if (AuditTrail.InsertTrail("AUTGR-DA", "multiple tables", StringUtilities.HandleApostrophe(sObj)) == 0)
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                pSet.Close();

                if (iCtr > 0)
                {
                    sMess = "Set Default inserted: " + iCtr.ToString();
                }

                

                if (sMess != "")
                {
                    MessageBox.Show(sMess, "GR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnDefault.Enabled = false;
                }



            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit")
            {
                txtRevYearTo.ReadOnly = false;
                btnEdit.Text = "Update";
                btnAdd.Enabled = false;
                btnCancel.Text = "Cancel";
            }
            else
            {
                if (!ValidateRevYear("Edit"))
                    return;

                if (txtRevYear.Text.Trim() == "" || txtRevYearTo.Text.Trim() == "")
                {
                    MessageBox.Show("Effectivity year/s are required", "Revenue Year", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show("Save new Revenue Year?", "GR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet pSet = new OracleResultSet();

                    pSet.Query = "update gr_year set rev_year_to = '" + StringUtilities.HandleApostrophe(txtRevYearTo.Text.Trim()) + "', ";
                    pSet.Query += " rev_ord = '" + StringUtilities.HandleApostrophe(txtOrdinance.Text.Trim()) + "'";
                    pSet.Query += " where rev_year = '" + txtRevYear.Text + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    String sObj = txtRevYear.Text.Trim() + "-" + txtRevYearTo.Text.Trim();

                    if (AuditTrail.InsertTrail("AUTGR-RE", "gr_year", StringUtilities.HandleApostrophe(sObj)) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                txtRevYearTo.ReadOnly = true;
                btnEdit.Text = "Edit";
                btnAdd.Enabled = true;
                btnCancel.Text = "Close";
                m_sRevYearTo = "";
                UpdateList();
            }
        }

        private void dgvRevYear_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtRevYear.Text = dgvRevYear[0, e.RowIndex].Value.ToString();
            txtRevYearTo.Text = dgvRevYear[1, e.RowIndex].Value.ToString();
            txtOrdinance.Text = dgvRevYear[2, e.RowIndex].Value.ToString();
            m_sRevYearTo = txtRevYearTo.Text;
        }
    }
}