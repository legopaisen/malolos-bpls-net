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

namespace TellerSetup
{
    public partial class frmTellerSetup : Form
    {
        public string TellerCode    // RMC 20150504 QA corrections
        {
            get { return txtTellerCode.Text; }
            set { txtTellerCode.Text = value; }
        }

        public frmTellerSetup()
        {
            InitializeComponent();
        }

        private void frmTellerSetup_Load(object sender, EventArgs e)
        {
            LoadTeller();
        }

        private void LoadTeller()
        {
            lblCounter.Text = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from tellers order by ln, fn, mi, teller";
            if (result.Execute())
            {
                while (result.Read())
                    //dgvTellerList.Rows.Add(result.GetString("teller"), result.GetString("ln"), result.GetString("fn"), result.GetString("mi"), result.GetString("position"), result.GetString("or_code"), result.GetString("rcd_code"));    // RMC 20150126
                      dgvTellerList.Rows.Add(result.GetString("teller"), result.GetString("ln"), result.GetString("fn"), result.GetString("mi"), result.GetString("position"), result.GetString("rcd_code"));    // RMC 20150126
            
            }
            result.Close();

            lblCounter.Text = dgvTellerList.Rows.Count.ToString() + " Record/s were found.";
        }

        private void dgvTellerList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTellerList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex != -1)
            {
                FillControl(e.RowIndex);
            }

        }

        private void FillControl(int i)
        {
            txtTellerCode.Text = dgvTellerList.Rows[i].Cells[0].Value.ToString();
            txtLastName.Text = dgvTellerList.Rows[i].Cells[1].Value.ToString();
            txtFName.Text = dgvTellerList.Rows[i].Cells[2].Value.ToString();
            txtMi.Text = dgvTellerList.Rows[i].Cells[3].Value.ToString();
            txtPosition.Text = dgvTellerList.Rows[i].Cells[4].Value.ToString();
            //txtORCode.Text = dgvTellerList.Rows[i].Cells[5].Value.ToString(); // JAV 20170717
            //txtRcdCode.Text = dgvTellerList.Rows[i].Cells[6].Value.ToString();  // RMC 20150126
            txtRcdCode.Text = dgvTellerList.Rows[i].Cells[5].Value.ToString();
        }

        private void dgvTellerList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTellerList_Click(object sender, EventArgs e)
        {
            txtTellerCode.Text = dgvTellerList.SelectedRows[0].Cells[0].Value.ToString();
            txtLastName.Text = dgvTellerList.SelectedRows[0].Cells[1].Value.ToString();
            txtFName.Text = dgvTellerList.SelectedRows[0].Cells[2].Value.ToString();
            txtMi.Text = dgvTellerList.SelectedRows[0].Cells[3].Value.ToString();
            txtPosition.Text = dgvTellerList.SelectedRows[0].Cells[4].Value.ToString();
            //txtORCode.Text = dgvTellerList.SelectedRows[0].Cells[5].Value.ToString();
            //txtRcdCode.Text = dgvTellerList.Rows[0].Cells[6].Value.ToString();  // RMC 20150126
            //txtRcdCode.Text = dgvTellerList.SelectedRows[0].Cells[6].Value.ToString();  // RMC 20150622 Corrections in teller set-up
            txtRcdCode.Text = dgvTellerList.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void CleanMe()
        {
            dgvTellerList.Rows.Clear();
            LoadTeller();
            txtTellerCode.Text = string.Empty;
            txtORCode.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtFName.Text = string.Empty;
            txtMi.Text = string.Empty;
            txtPosition.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtRcdCode.Text = string.Empty; // RMC 20150107

            txtTellerCode.Focus();
            
        }

        private void ButtonControl(bool blnEnable)
        {
            //MCR 20140324
            foreach (object c in this.Controls)
            {
                if (c is ComponentFactory.Krypton.Toolkit.KryptonButton)
                    ((ComponentFactory.Krypton.Toolkit.KryptonButton)c).Enabled = blnEnable;
            }
        }

        private void TextControl(bool blnEnable)
        {
            //MCR 20140324
            foreach (object c in this.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Enabled = blnEnable;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text.Trim() == "&Add")
            {
                btnAdd.Text = "&Save";
                btnClose.Text = "&Cancel";
                CleanMe();
                dgvTellerList.Enabled = false;
                //MCR 20140324 (s)
                ButtonControl(false);
                TextControl(true);
                btnAdd.Enabled = true;
                btnClose.Enabled = true;
                //MCR 20140324 (e)
            }
            else
            {
                // RMC 20150622 Corrections in teller set-up (s)
                if (!ValidateRCDCode())
                    return;
                // RMC 20150622 Corrections in teller set-up (e)

                if (txtTellerCode.Text.Trim() == string.Empty || txtLastName.Text.Trim() == string.Empty || txtPassword.Text.Trim() == string.Empty
                    || txtRcdCode.Text.Trim() == string.Empty)  // RMC 20150107
                {
                    MessageBox.Show("Please supply necessary details.");
                    return;
                }
                else
                {
                    if (txtPassword.Text.Trim() == txtConfirmPassword.Text.Trim())
                    {
                        if(txtRcdCode.Text.Length > 3) //JARS 20170831
                        {
                            MessageBox.Show("RCD Code too long, must only be 3 characters long");
                            return;
                        }
                        EncryptPass encPass = new EncryptPass();
                        txtPassword.Text = encPass.EncryptPassword(txtPassword.Text.Trim());
                        OracleResultSet result = new OracleResultSet();

                        // RMC 20150107 
                        result.Query = "insert into rcd_series values ('0',";
                        result.Query += "'" + txtTellerCode.Text.Trim() + "', ";
                        result.Query += "'" + txtRcdCode.Text.Trim() + "',";
                        result.Query += "'" + "0" + "')";
                        if (result.ExecuteNonQuery() == 0)
                        { }
                        result = new OracleResultSet();
                        // RMC 20150107 

                        //result.Query = "insert into tellers values ('";
                        result.Query = "insert into tellers(teller,ln,fn,mi,position, pswd, rcd_code) values ('"; // JAA 20190212 added column names
                        result.Query += txtTellerCode.Text.Trim() + "','";
                        result.Query += txtLastName.Text.Trim() + "','";
                        result.Query += txtFName.Text.Trim() + "','";
                        result.Query += txtMi.Text.Trim() + "','";
                        result.Query += txtPosition.Text.Trim() + "','";
                        result.Query += StringUtilities.HandleApostrophe(txtPassword.Text.Trim()) + "','";
                        //result.Query += txtORCode.Text.Trim() + "', '";
                        result.Query += txtRcdCode.Text.Trim() + "')";  // RMC 20150126
                        if (result.ExecuteNonQuery() != 0)
                        {
                            MessageBox.Show("Teller successfully saved.");
                            //CleanMe();
                            dgvTellerList.Enabled = true;
                        }
                        result.Close();

                        // RMC 20150811 added trail in tellers module (s)
                        if (AuditTrail.InsertTrail("STT-A", "tellers", "Teller Code: " + txtTellerCode.Text.Trim()) == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        CleanMe();
                        // RMC 20150811 added trail in tellers module (e)
                        
                    }
                    else
                    {
                        MessageBox.Show("Password did not matched.");
                        return;
                    }
                }
                btnClose.PerformClick(); //MCR 20140324
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dgvTellerList.Enabled = false;
            if (MessageBox.Show("Are you sure you want to delete " + dgvTellerList.SelectedRows[0].Cells[0].Value.ToString() + "'s record?", "Delete Teller", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet result = new OracleResultSet();
                // RMC 20150811 added validation in deleting teller (s)
                result.Query = "select * from pay_hist where trim(teller) = '" + txtTellerCode.Text.Trim() + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("Cannot delete teller, transaction found","Teller Set-up",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        result.Close();
                        // RMC 20150811 added validation in deleting teller (e)

                        result.Query = "delete from tellers where teller = '" + txtTellerCode.Text.Trim() + "' and ln = '" + txtLastName.Text.Trim() + "' and fn = '" + txtFName.Text.Trim() + "' and position = '" + txtPosition.Text.Trim() + "'";
                        if (result.ExecuteNonQuery() != 0)
                        {
                            MessageBox.Show("Teller successfully deleted.");
                            //CleanMe();
                            dgvTellerList.Enabled = true;
                            //return;
                        }
                        result.Close();

                        // RMC 20150811 added trail in tellers module (s)
                        if (AuditTrail.InsertTrail("STT-D", "tellers", "Teller Code: " + txtTellerCode.Text.Trim()) == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        CleanMe();
                        // RMC 20150811 added trail in tellers module (e)
                    }
                }
            }
            else
            {
                CleanMe();
                dgvTellerList.Enabled = true;
                return;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //MCR 20140324
            if (btnEdit.Text == "&Edit")
            {
                btnEdit.Text = "&Update";
                btnClose.Text = "&Cancel";
                dgvTellerList.Enabled = false;
                ButtonControl(false);
                TextControl(true);
                txtTellerCode.Enabled = false;
                btnEdit.Enabled = true;
                btnClose.Enabled = true;
            }
            else
            {
                // RMC 20150622 Corrections in teller set-up (s)
                if (!ValidateRCDCode())
                    return;
                // RMC 20150622 Corrections in teller set-up (e)

                if (txtTellerCode.Text.Trim() == string.Empty || txtLastName.Text.Trim() == string.Empty || txtPassword.Text.Trim() == string.Empty
                    || txtRcdCode.Text.Trim() == string.Empty || txtMi.Text.Trim() == string.Empty)  // RMC 20150126 //JARS 20170825
                {
                    MessageBox.Show("Please supply necessary details.");
                    return;
                }
                else
                {
                    if (txtPassword.Text.Trim() == txtConfirmPassword.Text.Trim())
                    {
                        EncryptPass encPass = new EncryptPass();
                        txtPassword.Text = encPass.EncryptPassword(txtPassword.Text.Trim());
                        OracleResultSet result = new OracleResultSet();

                        // RMC 20150107 
                        int iCnt = 0;
                        result.Query = "select count(*) from rcd_series where teller = '" + txtTellerCode.Text.Trim() + "'";
                        int.TryParse(result.ExecuteScalar(), out iCnt);
                        if (iCnt == 0)
                        {
                            result.Close();
                            result.Query = "insert into rcd_series values ('0',";
                            result.Query += "'" + txtTellerCode.Text.Trim() + "', ";
                            result.Query += "'" + txtRcdCode.Text.Trim() + "', ";
                            result.Query += "'0')"; //JARS 20170824
                            if (result.ExecuteNonQuery() == 0)
                            { }
                        }
                        else
                        {
                            result.Close();
                            result.Query = "update rcd_series set rcd_code = '" + txtRcdCode.Text.Trim() + "'";
                            result.Query += " where teller = '" + txtTellerCode.Text.Trim() + "'";
                            if (result.ExecuteNonQuery() == 0)
                            { }
                        }
                        result = new OracleResultSet();
                        // RMC 20150107 

                        result.Query = "update tellers set LN=:LNvalue, FN=:FNvalue, MI=:MIvalue, POSITION=:POSvalue, PSWD=:PSWDvalue, RCD_CODE=:RCDvalue where TELLER = :TellerCode";
                        result.AddParameter(":LNvalue", txtLastName.Text.Trim());
                        result.AddParameter(":FNvalue", txtFName.Text.Trim());
                        result.AddParameter(":MIvalue", txtMi.Text.Trim());
                        result.AddParameter(":POSvalue", txtPosition.Text.Trim());
                        result.AddParameter(":PSWDvalue", txtPassword.Text.Trim());
                        result.AddParameter(":RCDvalue", txtRcdCode.Text.Trim());    // RMC 20150126
                        result.AddParameter(":TellerCode", txtTellerCode.Text.Trim());
                        

                        if (result.ExecuteNonQuery() != 0)
                        {
                            MessageBox.Show("Teller successfully saved.");
                            //CleanMe();
                            dgvTellerList.Enabled = true;
                        }
                        result.Close();

                        // RMC 20150811 added trail in tellers module (s)
                        if (AuditTrail.InsertTrail("STT-E", "tellers", "Teller Code: " + txtTellerCode.Text.Trim()) == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        CleanMe();
                        // RMC 20150811 added trail in tellers module (e)
                    }
                    else
                    {
                        MessageBox.Show("Password did not matched.");
                        return;
                    }
                }
                btnClose.PerformClick(); //MCR 20140324
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //MCR 20140324 
            if (btnClose.Text == "C&lose")
                this.Close();
            else
            {
                CleanMe();
                ButtonControl(true);
                TextControl(false);
                btnEdit.Text = "&Edit";
                btnAdd.Text = "&Add";
                btnClose.Text = "C&lose";
            }
        }

        private void txtRcdCode_Leave(object sender, EventArgs e)
        {
            // RMC 20150107
            /*OracleResultSet result = new OracleResultSet();

            result.Query = "select * from rcd_series where rcd_code = '" + txtRcdCode.Text.Trim() + "'";
            result.Query += " and teller <> '" + txtTellerCode.Text.Trim() + "'";   // RMC 20150126
            if(result.Execute())
            {
                if(result.Read())
                {
                    MessageBox.Show("RCD code already in use",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    txtRcdCode.Text = "";
                    return;
                }
            }
            result.Close();*/
            // RMC 20150622 Corrections in teller set-up, put rem

            // RMC 20150622 Corrections in teller set-up (s)
            if (!ValidateRCDCode())
                return;
            // RMC 20150622 Corrections in teller set-up (e)
        }

        private bool ValidateRCDCode()
        {
            // RMC 20150622 Corrections in teller set-up
            OracleResultSet result = new OracleResultSet();

            result.Query = "select * from rcd_series where rcd_code = '" + txtRcdCode.Text.Trim() + "'";
            result.Query += " and teller <> '" + txtTellerCode.Text.Trim() + "'";   
            if (result.Execute())
            {
                if (result.Read())
                {
                    MessageBox.Show("RCD code already in use", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtRcdCode.Text = "";
                    return false;
                }
            }
            result.Close();

            return true;
        }
    }
}
