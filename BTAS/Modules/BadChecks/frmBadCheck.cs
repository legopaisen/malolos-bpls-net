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

namespace Amellar.Modules.BadChecks
{
    public partial class frmBadCheck : Form
    {
        private bool m_bIsBadChk = false;
        private string m_sReason = string.Empty;
        private string m_sBankCode = string.Empty;
        private string m_sChkAmt = string.Empty;
        private string m_sChkType = string.Empty;
        private string m_sChkDate = string.Empty;
        private string m_sOrNo = string.Empty;

        public frmBadCheck()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                if (txtChkNo.Text.Trim() == "")
                {
                    frmSearchChkNo form = new frmSearchChkNo();
                    form.ShowDialog();

                    if(form.CheckNo != "")
                    {
                        txtChkNo.Text = form.CheckNo;
                    }
                }
                
                if(txtChkNo.Text.Trim() != "")
                {
                    OracleResultSet set = new OracleResultSet();
                    string sQuery = string.Empty;
                    
                    bool bRecordFound = false;

                    set.Query = "select * from bounce_chk_info where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text.Trim()) + "'";
                    if (set.Execute())
                    {
                        if (set.Read())
                        {
                            m_bIsBadChk = true;
                            bRecordFound = true;

                            btnRemove.Enabled = true;
                            btnRemoveAll.Enabled = true;

                            sQuery = set.Query.ToString();
                        }
                        else
                        {
                            m_bIsBadChk = false;

                            btnRemove.Enabled = false;
                            btnRemoveAll.Enabled = false;

                            set.Close();

                            set.Query = "select * from chk_tbl where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text.Trim()) + "'";
                            if (set.Execute())
                            {
                                if (set.Read())
                                { 
                                    bRecordFound = true;
                                    sQuery = set.Query.ToString();
                                }
                                else
                                {
                                    set.Close();
                                    set.Query = "select * from cancelled_chk where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text.Trim()) + "'";
                                    if(set.Execute())
                                    {
                                        if(set.Read())
                                        {
                                            bRecordFound = true;
                                            sQuery = set.Query.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if(bRecordFound)
                    {
                        set.Query = sQuery;
                        if (set.Execute())
                        {
                            if (set.Read())
                            {
                                m_sBankCode = set.GetString("bank_code");
                                txtBankName.Text = AppSettingsManager.GetBankBranch(m_sBankCode);
                                dtpChkDate.Text = dtpChkDate.Text;
                                txtLastName.Text = set.GetString("acct_ln").Trim();
                                txtFirstName.Text = set.GetString("acct_fn").Trim();
                                txtMI.Text = set.GetString("acct_mi").Trim();
                                txtAcctNo.Text = set.GetString("acct_no").Trim();
                                dtpChkDate.Value = set.GetDateTime("chk_date");
                                txtChkAmt.Text = string.Format("{0:#,###.00}", set.GetDouble("chk_amt"));

                                m_sChkType = set.GetString("chk_type");
                                if (m_sChkType == "CC")
                                    rdoChk1.Checked = true;
                                else if (m_sChkType == "PC")
                                    rdoChk2.Checked = true;
                                else if (m_sChkType == "MC")
                                    rdoChk3.Checked = true;

                                OracleResultSet set2 = new OracleResultSet();
                                set2.Query = "select * from multi_check_pay where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text.Trim()) + "'";
                                if (set2.Execute())
                                {
                                    if (set2.Read())
                                    {
                                        txtChkAmt.Text = string.Format("{0:#,###.00}", set2.GetDouble("chk_amt"));
                                        m_sBankCode = set2.GetString("bank_code");
                                        txtBankName.Text = AppSettingsManager.GetBankBranch(m_sBankCode);
                                    }
                                }
                                set2.Close();

                                if (m_bIsBadChk == true)
                                {
                                    m_sReason = set.GetString("reason");
                                    if (m_sReason == "AC")
                                        rdoReason1.Checked = true;
                                    if (m_sReason == "DAIF")
                                        rdoReason2.Checked = true;
                                    if (m_sReason == "DAUD")
                                        rdoReason3.Checked = true;
                                    if (m_sReason == "SPO")
                                        rdoReason4.Checked = true;

                                    UpdateListBC();
                                    btnInsert.Text = "Remove from Block List";
                                }
                                else
                                {
                                    UpdateList();

                                    rdoReason1.Enabled = true;
                                    rdoReason2.Enabled = true;
                                    rdoReason3.Enabled = true;
                                    rdoReason4.Enabled = true;
                                }

                                btnEditChk.Enabled = true;
                                btnInsert.Enabled = true;

                                btnSearch.Text = "Clear";
                            }
                        }
                        set.Close();
                    }
                    else
                    {
                        MessageBox.Show("No record found.", "Bad Checks", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    
                }
            }
            else
            {
                ClearControls();
                txtChkNo.Text = "";
                btnSearch.Text = "Search";
            }
        }

        private void ClearControls()
        {
            txtChkNo.Focus();
            
            m_sOrNo = "";
            m_sBankCode = "";
            m_sChkType = "";
            m_sReason = "";
            txtBankName.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtMI.Text = "";
            txtAcctNo.Text = "";
            txtChkAmt.Text = "";
            dtpChkDate.Value = AppSettingsManager.GetCurrentDate();
            txtBIN.Text = "";
            txtBnsName.Text = "";
            txtBnsAdd.Text = "";
            txtOwnName.Text = "";
            txtOwnAdd.Text = "";

            dgvList.Columns.Clear();

            rdoChk1.Checked = false;
            rdoChk2.Checked = false;
            rdoChk3.Checked = false;

            rdoReason1.Checked = false;
            rdoReason2.Checked = false;
            rdoReason3.Checked = false;
            rdoReason4.Checked = false;

            rdoReason1.Enabled = false;
            rdoReason2.Enabled = false;
            rdoReason3.Enabled = false;
            rdoReason4.Enabled = false;

            btnEditChk.Enabled = false;
            btnInsert.Enabled = false;

            btnRemove.Enabled = false;
            btnRemoveAll.Enabled = false;
            
            btnInsert.Text = "Insert Account owner to Block List";
            btnSearch.Text = "Search";

        }

        private void rdoChk1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoChk1.Checked == true)
            {
                m_sChkType = "CC";  //cashier's check
            }
        }

        private void rdoChk2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoChk2.Checked == true)
            {
                m_sChkType = "PC";  //personal check
            }
        }

        private void rdoChk3_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoChk3.Checked == true)
            {
                m_sChkType = "MC";  //manager's check
            }
        }

        private void UpdateListBC()
        {
            OracleResultSet set = new OracleResultSet();

            dgvList.Columns.Clear();
            dgvList.Columns.Add("BIN", "BIN");
            dgvList.Columns.Add("ORNO", "OR NO");
            dgvList.Columns.Add("BNSNM", "BUSINESS NAME");
            dgvList.Columns.Add("TAXYEAR", "TAX YEAR");
            dgvList.Columns.Add("TERM", "PAY TERM");
            dgvList.Columns.Add("QTR", "QTR PAID");
            dgvList.Columns.Add("PERMIT", "PERMIT NO");
            dgvList.Columns.Add("STAT", "STATUS");
            dgvList.Columns.Add("OWNCODE", "OWN CODE");

            set.Query = "select a.bin,a.or_no,b.bns_nm,b.tax_year,a.payment_term,a.qtr_paid,b.permit_no,b.bns_stat,b.own_code ";
	        set.Query+= "from pay_hist a, businesses b ";
	        set.Query+= "where a.bin = b.bin ";
	        set.Query+= "and a.or_no in (select or_no from bounce_chk_rec where chk_no = '"+ StringUtilities.HandleApostrophe(txtChkNo.Text.Trim()) +"')";
            if (set.Execute())
            {
                while (set.Read())
                {
                    dgvList.Rows.Add(set.GetString(0).Trim(), set.GetString(1).Trim(), set.GetString(2).Trim(), set.GetString(3).Trim(),
                        set.GetString(4).Trim(), set.GetString(5).Trim(), set.GetString(6).Trim(), set.GetString(7).Trim(), set.GetString(8).Trim());
                }
            }
            set.Close();
            
        }

        private void UpdateList()
        {
            OracleResultSet set = new OracleResultSet();

            dgvList.Columns.Clear();
            dgvList.Columns.Add("BIN", "BIN");
            dgvList.Columns.Add("ORNO", "OR NO");
            dgvList.Columns.Add("BNSNM", "BUSINESS NAME");
            dgvList.Columns.Add("TAXYEAR", "TAX YEAR");
            dgvList.Columns.Add("TERM", "PAY TERM");
            dgvList.Columns.Add("QTR", "QTR PAID");
            dgvList.Columns.Add("PERMIT", "PERMIT NO");
            dgvList.Columns.Add("STAT", "STATUS");
            dgvList.Columns.Add("OWNCODE", "OWN CODE");
            dgvList.Columns[4].Width = 80;
            dgvList.Columns[5].Width = 80;
            
            set.Query = "select a.bin,a.or_no,b.bns_nm,b.tax_year,a.payment_term,a.qtr_paid,b.permit_no,b.bns_stat,b.own_code ";
            set.Query += "from pay_hist a, businesses b ";
            set.Query += "where a.bin = b.bin ";
            set.Query += "and a.or_no in (select or_no from chk_tbl where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text.Trim()) + "')";
            if (set.Execute())
            {
                while (set.Read())
                {
                    dgvList.Rows.Add(set.GetString(0).Trim(), set.GetString(1).Trim(), set.GetString(2).Trim(), set.GetString(3).Trim(),
                        set.GetString(4).Trim(), set.GetString(5).Trim(), set.GetString(6).Trim(), set.GetString(7).Trim(), set.GetString(8).Trim());
                }
            }
            set.Close();
            
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            OracleResultSet cmd = new OracleResultSet();

            if (btnInsert.Text == "Insert Account owner to Block List")
            {
                if (rdoReason1.Checked == true || rdoReason2.Checked == true || rdoReason3.Checked == true || rdoReason4.Checked == true)
                {
                    if (MessageBox.Show("Are you sure you want to insert this record in block list?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if(rdoReason1.Checked == true)
                            m_sReason = "AC";
                        else if(rdoReason2.Checked == true)
                            m_sReason = "DAIF";
                        else if(rdoReason3.Checked == true)    
                            m_sReason = "DAUD";
                        else
                            m_sReason = "SPO";

                        double dChkAmt = 0;
                        double.TryParse(txtChkAmt.Text, out dChkAmt);

                        cmd.Query = "insert into bounce_chk_info values ";
                        cmd.Query += "('" + StringUtilities.HandleApostrophe(txtChkNo.Text.Trim()) + "', ";
                        cmd.Query += " to_date('"+dtpChkDate.Text.ToString()+"', 'MM/dd/yyyy'), " ;
                        cmd.Query += " '" + m_sChkType + "', ";
                        cmd.Query += " " + dChkAmt + ", ";
                        cmd.Query += " '" + m_sBankCode + "', ";
                        cmd.Query += " '" + txtAcctNo.Text + "', ";
                        cmd.Query += " '" + StringUtilities.HandleApostrophe(txtLastName.Text) + "', ";
                        cmd.Query += " '" + StringUtilities.HandleApostrophe(txtFirstName.Text) + "', ";
                        cmd.Query += " '" + txtMI.Text + "', ";
                        cmd.Query += " '" + m_sReason + "')";
                        if (cmd.ExecuteNonQuery() == 0)
                        { }

                        string strObj = "Acct. No.: " + txtAcctNo.Text + "/chk no.: " + txtChkNo.Text.Trim() + "/Reason: " + m_sReason;

                        if (AuditTrail.InsertTrail("CPBC-BL-ADD", "bounce_chk_info", StringUtilities.HandleApostrophe(strObj)) == 0)
                        {
                            cmd.Rollback();
                            cmd.Close();
                            MessageBox.Show(cmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        
                        PopBounceRec();
                        MessageBox.Show("Record Inserted.", "Bad Checks", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        m_bIsBadChk = true;

                        btnInsert.Text = "Remove Account owner from Block List";

                        btnEditChk.Enabled = false;
                        rdoReason1.Enabled = false;
                        rdoReason2.Enabled = false;
                        rdoReason3.Enabled = false;
                        rdoReason4.Enabled = false;

                    }

                }
                else
                {
                    MessageBox.Show("Please select reason of bad check tagging. ", "Bad Checks", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to remove this record from block list?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd.Query = "delete from bounce_chk_info";
                    cmd.Query += " where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text) + "'";
                    if (cmd.ExecuteNonQuery() == 0)
                    { }

                    string strObj = "Acct. No.: " + txtAcctNo.Text + "/chk no.: " + txtChkNo.Text.Trim();

                    if (AuditTrail.InsertTrail("CPBC-BL-DEL", "bounce_chk_info", StringUtilities.HandleApostrophe(strObj)) == 0)
                    {
                        cmd.Rollback();
                        cmd.Close();
                        MessageBox.Show(cmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    OracleResultSet set = new OracleResultSet();
                    string sBIN = string.Empty;
                    string sOrNo = string.Empty;

                    set.Query = "select * from bounce_chk_rec";
                    set.Query += " where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text) + "'";
                    if (set.Execute())
                    {
                        while (set.Read())
                        {
                            sBIN = set.GetString("bin");
                            sOrNo = set.GetString("or_no");

                            cmd.Query = "delete from bounce_chk_rec";
                            cmd.Query += " where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text) + "'";
                            cmd.Query += " and bin = '" + sBIN + "' and or_no = '" + sOrNo + "'";
                            if (cmd.ExecuteNonQuery() == 0)
                            { }

                            strObj = "BIN: " + sBIN + "/OR no.: " + sOrNo + "/chk no.: " + txtChkNo.Text.Trim();
                            if (AuditTrail.InsertTrail("CPBC-RB-DEL", "bounce_chk_rec", StringUtilities.HandleApostrophe(strObj)) == 0)
                            {
                                cmd.Rollback();
                                cmd.Close();
                                MessageBox.Show(cmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    set.Close();

                    //clean-up
                    cmd.Query = "delete from bounce_chk_rec";
                    cmd.Query += " where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text) + "'";
                    if (cmd.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("Record removed.", "Bad Checks", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearControls();

                }
            }
        }

        private void PopBounceRec()
        {
            OracleResultSet cmd = new OracleResultSet();
            string sBIN = string.Empty;
            string sOrNo = string.Empty;
            string sTmpBIN = string.Empty;
            string sTmpOrNo = string.Empty;

            for(int i = 0; i < dgvList.Rows.Count; i++)
            {
                sBIN = dgvList[0, i].Value.ToString();
                sOrNo = dgvList[1, i].Value.ToString();

                if (sTmpBIN != sBIN || sTmpOrNo != sOrNo)
                {
                    // insert same BIN and OR no once only (this is for records with qtrly payments) to prevent multiple entries
                    cmd.Query = "insert into bounce_chk_rec values ";
                    cmd.Query += "('" + sBIN + "', ";
                    cmd.Query += " '" + sOrNo + "', ";
                    cmd.Query += " '" + StringUtilities.HandleApostrophe(txtChkNo.Text) + "')";
                    if (cmd.ExecuteNonQuery() == 0)
                    { }

                    sTmpBIN = sBIN;
                    sTmpOrNo = sOrNo;

                    string strObj = "BIN: " + sBIN + "/OR no.: " + sOrNo + "/chk no.: " + txtChkNo.Text.Trim();

                    if (AuditTrail.InsertTrail("CPBC-RB-ADD", "bounce_chk_rec", StringUtilities.HandleApostrophe(strObj)) == 0)
                    {
                        cmd.Rollback();
                        cmd.Close();
                        MessageBox.Show(cmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            OracleResultSet cmd = new OracleResultSet();

            if (txtBIN.Text.Trim() == "")
            {
                MessageBox.Show("Select business to remove from list first","",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Remove this business record from the list of restricted businesses?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cmd.Query = "delete from bounce_chk_rec ";
                cmd.Query += "where bin = '" + txtBIN.Text.Trim() + "' ";
                cmd.Query += "and or_no = '" + m_sOrNo + "'";
                if (cmd.ExecuteNonQuery() == 0)
                { }

                string strObj = "BIN: " + txtBIN.Text.Trim() + "/OR no.: " + m_sOrNo + "/chk no.: " + txtChkNo.Text.Trim();
                if (AuditTrail.InsertTrail("CPBC-RB-DEL", "bounce_chk_rec", StringUtilities.HandleApostrophe(strObj)) == 0)
                {
                    cmd.Rollback();
                    cmd.Close();
                    MessageBox.Show(cmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Record removed.", "Bad Checks", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UpdateListBC();
                txtBnsAdd.Text = "";
                txtOwnName.Text = "";
                txtOwnAdd.Text = "";
                txtBIN.Text = "";
                txtBnsName.Text = "";
                m_sOrNo = "";
            }		
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_sOrNo = "";

            if(dgvList.Rows.Count > 0)
            {
                if (m_bIsBadChk == true)
                {
                    btnRemove.Enabled = true;
                    btnRemoveAll.Enabled = true;
                }
                else
                {
                    btnRemove.Enabled = false;
                    btnRemoveAll.Enabled = false;
                }

                try
                {
                    txtBIN.Text = dgvList[0, e.RowIndex].Value.ToString();
                    m_sOrNo = dgvList[1, e.RowIndex].Value.ToString();
                    txtBnsName.Text = dgvList[2, e.RowIndex].Value.ToString();
                    txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(dgvList[0, e.RowIndex].Value.ToString());
                    txtOwnName.Text = AppSettingsManager.GetBnsOwner(dgvList[8, e.RowIndex].Value.ToString());
                    txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(dgvList[8, e.RowIndex].Value.ToString());
                }
                catch
                {
                    txtBIN.Text = "";
                    m_sOrNo = "";
                    txtBnsAdd.Text = "";
                    txtOwnName.Text = "";
                    txtOwnAdd.Text = "";
                    txtBnsName.Text = "";
                }
            }
            else
            {
                btnRemove.Enabled = false;
                btnRemoveAll.Enabled = false;

                txtBnsAdd.Text = "";
                txtOwnName.Text = "";
                txtOwnAdd.Text = "";

            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            OracleResultSet cmd = new OracleResultSet();

            // Add rights here

            if (MessageBox.Show("Are you sure you want to remove all records of this account from the list of restricted businesses?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet set = new OracleResultSet();
                string sBIN = string.Empty;
                string sOrNo = string.Empty;

                set.Query = "select * from bounce_chk_rec";
                set.Query += " where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text) + "'";
                if (set.Execute())
                {
                    while (set.Read())
                    {
                        sBIN = set.GetString("bin");
                        sOrNo = set.GetString("or_no");

                        cmd.Query = "delete from bounce_chk_rec";
                        cmd.Query += " where chk_no = '" + StringUtilities.HandleApostrophe(txtChkNo.Text) + "'";
                        cmd.Query += " and bin = '" + sBIN + "' and or_no = '" + sOrNo + "'";
                        if (cmd.ExecuteNonQuery() == 0)
                        { }

                        string strObj = "BIN: " + sBIN + "/OR no.: " + sOrNo + "/chk no.: " + txtChkNo.Text.Trim();
                        if (AuditTrail.InsertTrail("CPBC-RB-DEL", "bounce_chk_rec", StringUtilities.HandleApostrophe(strObj)) == 0)
                        {
                            cmd.Rollback();
                            cmd.Close();
                            MessageBox.Show(cmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                set.Close();

                //clean-up
                cmd.Query = "delete from bounce_chk_rec ";
                cmd.Query += "where chk_no = '" + txtChkNo.Text + "' ";
                if (cmd.ExecuteNonQuery() == 0)
                { }

                UpdateListBC();
                txtBnsAdd.Text = "";
                txtOwnName.Text = "";
                txtOwnAdd.Text = "";
                txtBIN.Text = "";
                txtBnsName.Text = "";
                m_sOrNo = "";

            }	
        }

        private void frmBadCheck_Load(object sender, EventArgs e)
        {
            txtChkNo.Focus();
        }
    }
}