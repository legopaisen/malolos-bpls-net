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
using ComponentFactory.Krypton.Toolkit;

namespace ReturnDeclareOR
{
    public partial class frmORDeclaration : Form
    {
        private string m_sUser = "";

        public frmORDeclaration()
        {
            InitializeComponent();
        }

        OracleResultSet result = new OracleResultSet();
        string m_strFormType = string.Empty;
        string m_strTellerCode = string.Empty;
        string m_strFrom = string.Empty;
        string m_strTo = string.Empty;
        Boolean m_blnComboSelected;
        
        private void frmDeclaration_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AppSettingsManager.SystemUser.UserCode.Trim()))
                m_sUser = "SYS_PROG";

            this.GetTellerCode();
            this.LoadList();
        }

        private void GetTellerCode()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT Teller FROM Tellers";
            if (result.Execute())
            {
                cboTellerCode.Items.Clear();
                while (result.Read()) 
                {
                    cboTellerCode.Items.Add(result.GetString(0));
                }
            }
            result.Close();
        }

        private void LoadList()
        {
            OracleResultSet result = new OracleResultSet();
            DateTime dtDateAssigned = new DateTime();
            string strTellerCode;
            string strFrom;
            string strTo;
            string strAssignedBy;
            string strDecSeries;
            string strFormType;
            string strCurOrNo;

            result.Query = "select or_assigned.*, or_current.cur_or_no from or_assigned,or_current where ";
            result.Query += "trim(or_assigned.from_or_no) = trim(or_current.from_or_no) and ";
            result.Query += "trim(or_assigned.to_or_no) = trim(or_current.to_or_no) and ";
            result.Query += "or_assigned.form_type = or_current.form_type ";
            if (result.Execute())
            {
                dgvList.Rows.Clear();
                while (result.Read())
                {
                    strTellerCode = result.GetString("TELLER_CODE");
                    strFrom = result.GetString(1);
                    strTo = result.GetString("TO_OR_NO");
                    dtDateAssigned = result.GetDateTime("DATE_ASSIGNED");
                    strAssignedBy = result.GetString("ASSIGNED_BY");
                    strDecSeries = result.GetString("DEC_SERIES");
                    strFormType = result.GetString("FORM_TYPE");
                    strCurOrNo = result.GetString("cur_or_no");
                    dgvList.Rows.Add(strTellerCode, strFormType, strFrom, strTo, dtDateAssigned.ToShortDateString(), strAssignedBy, strDecSeries, strCurOrNo);    // RMC 20131003 Added display of OR current in OR declaration module
                }
            }
            result.Close();

            dgvList.ClearSelection();
        }

        private void GetAvailableOR()
        {            
            string sFromORNum = string.Empty;
            string sToORNum = string.Empty;
            result = new OracleResultSet();
            result.Query = "SELECT FROM_OR_NO, TO_OR_NO FROM OR_INV WHERE FORM_TYPE = ";
            //result.Query += " and to_or_no not in (select to_or_no from or_assigned WHERE FORM_TYPE = '" + cboFormType.Text + "') ";   // RMC 20130605 corrections in OR declaration
            result.Query += " order by from_or_no ";  // RMC 20130522 corrections in OR declaration
            if (result.Execute())
            {
                dgvAvailableOR.Rows.Clear();
                while (result.Read())
                {
                    sFromORNum = result.GetString("FROM_OR_NO").Trim();
                    sToORNum = result.GetString("TO_OR_NO").Trim();
                    bool bCanFill;
                    //for (int i = 0; i != dataGridView1.RowCount; i++) 
                    //{
                    //    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == sFromORNum && dataGridView1.Rows[i].Cells[1].Value.ToString() == sToORNum)
                    //        bCanFill = false;
                    //}

                    // RMC 20130522 corrections in OR declaration (s)
                    ValidateORUsed(sFromORNum, sToORNum, out sFromORNum, out bCanFill);
                    // RMC 20130522 corrections in OR declaration (e)

                    if (bCanFill == true)
                        dgvAvailableOR.Rows.Add(sFromORNum, sToORNum);
                }
                result.Close();
            }
            if (dgvAvailableOR.RowCount > 0)
            {
                pnlAvailableOR.ForeColor = Color.Black;
                //pnlAvailableOR.Text = "Available OR Series for " + cboFormType.Text;
            }
            else
            {
                pnlAvailableOR.ForeColor = Color.Red;
                //pnlAvailableOR.Text = "No available OR Series for " + cboFormType.Text;
            }
        }

        private void btnDeclare_Click(object sender, EventArgs e)
        {
            if (ValidateFields() == true)
            {
                if (CheckValidORSeries() == true)
                {
                    if (MessageBox.Show("Are you sure you want to declare " + "" + " to teller " + cboTellerCode.Text + " with OR Series From " + txtFrom.Text + " To " + txtTo.Text + "", "Amellar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
                    {
                        if (cboTellerCode.Text.Trim() == "")
                        {
                            MessageBox.Show("Please select teller", "Teller", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }

                        string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                        result = new OracleResultSet();
                        result.Query = "INSERT INTO OR_ASSIGNED (TELLER_CODE, FROM_OR_NO, TO_OR_NO, DATE_ASSIGNED,assigned_by,FORM_TYPE)VALUES('";
                        result.Query += cboTellerCode.Text.Trim() + "', '" + txtFrom.Text.Trim() + "', '" + txtTo.Text.Trim() + "',";
                        result.Query += "to_date('" + sDate + "','MM/dd/yyyy'),'" + m_sUser + "',";
                        result.Query += "'" + "" + "')";
                        result.ExecuteNonQuery();
                        
                        // RMC 20130520 corrections in or declaration (s)
                        result.Query = "insert into or_current values (";
                        result.Query += "'" + cboTellerCode.Text.Trim() + "', '" + txtFrom.Text.Trim() + "', '" + txtTo.Text.Trim() + "',";
                        result.Query += "'" + txtFrom.Text.Trim() + "', '', '" + "" + "', '" + AppSettingsManager.GetCurrentDate().Year + "')"; // AST 20131003 Add saving of year in or_current
                        result.ExecuteNonQuery();
                        // RMC 20130520 corrections in or declaration (e)

                        // RMC 20130516 added trailing (s)
                        string strObject = string.Empty;
                        strObject = "Teller:" + cboTellerCode.Text.Trim() + "/";
                        strObject += "From:" + txtFrom.Text.Trim() + "/";
                        strObject += "To:" + txtTo.Text.Trim() + "/";

                        //if (AuditTrail.InsertTrail(result, m_sUser, "DROOO -D", "or_assigned", strObject) == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // RMC 20130516 added trailing (e)

                        this.LoadList();
                        ExpandInterface(false);
                        pnlAvailableOR.Text = "Available OR Series for ";
                        txtFrom.Enabled = false;
                        txtTo.Enabled = false;
                        btnDeclare.Enabled = false;
                        dgvList.Enabled = true; // AST 20130927 enable dgView
                        btnClose.Text = "&Close";
                    }
                }
            }
        }

        private bool ValidateFields() 
        {
            bool bCanDeclare = true;
            //if (cboFormType.SelectedIndex < 0)
            {
                MessageBox.Show("Please specify Form Type", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //cboFormType.DroppedDown = true;
                bCanDeclare = false;
            }

            if (cboTellerCode.Text.Trim() == "")
            {
                MessageBox.Show("Please Select Teller Code", "Select Teller", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cboTellerCode.DroppedDown = true;
            }

            if (txtFrom.Text.Length == 0 && txtTo.Text.Length == 0) 
            {
                MessageBox.Show("Please specify OR Series from the available list above", "Amellar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                bCanDeclare = false;
                dgvAvailableOR.Focus();
            }
            return bCanDeclare;
        }

        private bool CheckValidORSeries()
        {
            bool bAcceptable = false;
            string sFromORNum = string.Empty;
            string sToORNum = string.Empty;
            int iFromORNum = 0;
            int iToORNum = 0;
            result = new OracleResultSet();
            result.Query = "SELECT FROM_OR_NO, TO_OR_NO FROM OR_INVentory WHERE FORM_TYPE = '" + "" + "' ";
            //m_Set.Query += "AND SUBSTR(DATE_CREATED, 8, 2) = '" + sCurrYear + "'"; // AST 20131003 Add filtering of O.R. per Year
            if (result.Execute())
            {
                while (result.Read())
                {
                    sFromORNum = result.GetString("FROM_OR_NO");
                    sToORNum = result.GetString("TO_OR_NO");
                    iFromORNum = Convert.ToInt32(sFromORNum);
                    iToORNum = Convert.ToInt32(sToORNum);
                    if (Convert.ToInt32(txtFrom.Text) >= iFromORNum && Convert.ToInt32(txtFrom.Text) <= iToORNum // AST 20140205 Added equal to iToORNum
                        && Convert.ToInt32(txtTo.Text) <= iToORNum && Convert.ToInt32(txtTo.Text) >= iFromORNum) // AST 20140205 Added equal to iFromORNum
                    {
                        bAcceptable = true;
                    }
                }
                result.Close();
            }
                if (bAcceptable == false)
                    MessageBox.Show("Your OR Series is invalid", "Amellar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return bAcceptable;
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //m_blnComboSelected = false;
            //cboFormType.Text = Convert.ToString(dgvList.CurrentRow.Cells[1].Value);
            cboTellerCode.Text = Convert.ToString(dgvList.CurrentRow.Cells[0].Value);
            txtFrom.Text = Convert.ToString(dgvList.CurrentRow.Cells[2].Value);
            txtTo.Text = Convert.ToString(dgvList.CurrentRow.Cells[3].Value);
            txtCurrent.Text = Convert.ToString(dgvList.CurrentRow.Cells[7].Value);  // RMC 20131003 Added display of OR current in OR declaration module
            result.Query = @"SELECT TELLER_LN, TELLER_FN, TELLER_MI FROM TELLERS WHERE TELLER_CODE = 
            '" + cboTellerCode.Text + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    txtFName.Text = result.GetString("TELLER_FN");
                    txtLName.Text = result.GetString("TELLER_LN");
                    txtMI.Text = result.GetString("TELLER_MI");
                }
                result.Close();
            }
            btnEdit.Enabled = true;
            btnReturn.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "&Cancel")
            {
                groupBox1.Enabled = true;
                btnEdit.Text = "&Edit";
                btnClose.Text = "&Close";
                txtFrom.Enabled = false;
                txtTo.Enabled = false;
                //cboFormType.Enabled = true;
                dgvList.Enabled = true;

                ExpandInterface(false);
                pnlAvailableOR.Text = "Available OR Series for ";
                txtFrom.Enabled = false;
                txtTo.Enabled = false;
                btnDeclare.Enabled = false;
                txtFrom.Clear();
                txtTo.Clear();
                pnlTellerInfo.Enabled = true;
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to close " + this.Text + "?", "Amellar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    this.Close();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to return this Series of O.R.?", "Return", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                result = new OracleResultSet();
                result.Query = "DELETE FROM OR_ASSIGNED WHERE FORM_TYPE = '" + "" + "'";
                // RMC 20130522 corrected returning of OR (s)
                result.Query += " and to_or_no = '" + txtTo.Text.Trim() + "'";
                result.Query += " and teller_code = '" + cboTellerCode.Text.Trim() + "' ";
                //m_Set.Query += "and substr(date_assigned, 8, 2) = '" + sCurrYear + "'"; // AST 20131003 Add filtering of O.R. per Year
                result.ExecuteNonQuery();

                result.Query = "DELETE FROM OR_CURRENT WHERE FORM_TYPE = '" + "" + "'";
                result.Query += " and to_or_no = '" + txtTo.Text.Trim() + "'";
                result.Query += " and teller_code = '" + cboTellerCode.Text.Trim() + "' ";
                //m_Set.Query += "and substr(year, 3, 2) = '" + sCurrYear + "'"; // AST 20131003 Add filtering of O.R. per Year
                result.ExecuteNonQuery();

                string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                //result.Query = "INSERT INTO OR_RETURNED (TELLER_CODE, FROM_OR_NO, TO_OR_NO, TRN_DATE, RETURNED_BY, DEC_SERIES)VALUES('";
                result.Query = "INSERT INTO OR_RETURNED (TELLER_CODE, FROM_OR_NO, TO_OR_NO, TRN_DATE, RETURNED_BY )VALUES('"; // JAA 20190212 remove dec_series
                result.Query += cboTellerCode.Text + "', '";
                result.Query += txtFrom.Text + "', '";
                result.Query += txtTo.Text + "',";
                result.Query += "to_date('" + sDate + "','MM/dd/yyyy'),'";
                result.Query += m_sUser + "', '')";
                result.ExecuteNonQuery();
                // RMC 20130522 corrected returning of OR (e)

                // RMC 20130516 added trailing (s)
                string strObject = string.Empty;
                strObject = "Teller:" + cboTellerCode.Text.Trim() + "/";
                strObject += "From:" + txtFrom.Text.Trim() + "/";
                strObject += "To:" + txtTo.Text.Trim() + "/";

                //if (AuditTrail.InsertTrail(result, m_sUser, "DROOO -R", "or_assigned", strObject) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // RMC 20130516 added trailing (e)


                cboTellerCode.Text = string.Empty;
                txtFName.Clear();
                txtLName.Clear();
                txtMI.Clear();
                txtFrom.Clear();
                txtTo.Clear();
                LoadList();

                btnReturn.Enabled = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //cboFormType.Text = string.Empty;  // RMC 20131002 clear all fields when form type was changed in OR declaration module
            cboTellerCode.Text = string.Empty;
            txtFName.Clear();
            txtLName.Clear();
            txtMI.Clear();
            txtFrom.Clear();
            txtTo.Clear();
            btnDeclare.Enabled = false;
            txtFrom.Enabled = false;
            txtTo.Enabled = false;
            btnEdit.Enabled = false;
            ExpandInterface(false);
            btnClose.Text = "&Close";
            dgvList.Enabled = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            List<string> CurrentRecord = new List<string>();
            List<string> NewRecord = new List<string>();
            if (btnEdit.Text == "&Edit")
            {
                string strQuery = "SELECT FORM_TYPE, TELLER FROM OR_USED ";
                strQuery += "WHERE FORM_TYPE = '" + "" + "' ";
                strQuery += "AND TELLER = '" + cboTellerCode.Text + "' ";
                //strQuery += "AND SUBSTR(TRN_DATE, 8, 2) = '" + sCurrYear + "'"; // AST 20131003 Add filtering of O.R. per Year
                //if (AppSettingsManager.HasTransaction(strQuery))
                  //  return;

                btnEdit.Text = "&Update";
                btnClose.Text = "&Cancel";
                ExpandInterface(true);
                btnDeclare.Enabled = false;
                btnEdit.Enabled = false;
                pnlTellerInfo.Enabled = false;
                dgvList.Enabled = false;
                //m_strFormType = cboFormType.Text.Trim();
                m_strTellerCode = cboTellerCode.Text.Trim();
                m_strFrom = txtFrom.Text.Trim();
                m_strTo = txtTo.Text.Trim();
                txtTo.Clear();
                txtFrom.Clear();
                txtFrom.Focus();
                GetAvailableOR();
                CurrentRecord.Clear();
                CurrentRecord.Add(m_strFrom);
                CurrentRecord.Add(m_strTo);
            }
            else
            {
                CurrentRecord.Clear();
                NewRecord.Add(txtFrom.Text.Trim());
                NewRecord.Add(txtTo.Text.Trim());
                //if (!AppSettingsManager.HasChanges(CurrentRecord, CurrentRecord))
                    //return;

                //if (!AppSettingsManager.Confirmation(AppSettingsManager.ConfirmButton.Edit))
                  //  return;
                
                result.Query = "UPDATE OR_ASSIGNED SET ";
                result.Query += "FROM_OR_NO = '" + txtFrom.Text + "',";
                result.Query += "TO_OR_NO = '" + txtTo.Text + "' ";
                result.Query += "WHERE FORM_TYPE = '" + "" + "' ";
                result.Query += "AND TELLER_CODE = '" + cboTellerCode.Text + "'";
                result.ExecuteNonQuery();

                // AST 20140424 Added Update in or_current (s)
                result.Query = "UPDATE OR_CURRENT SET ";
                result.Query += "FROM_OR_NO = '" + txtFrom.Text + "',";
                result.Query += "TO_OR_NO = '" + txtTo.Text + "' ";
                result.Query += "WHERE FORM_TYPE = '" + "" + "' ";
                result.Query += "AND TELLER_CODE = '" + cboTellerCode.Text + "'";
                result.ExecuteNonQuery();
                // AST 20140424 Added Update in or_current (e)

                // RMC 20130516 added trailing (s)
                string strObject = string.Empty;
                strObject = "Teller:" + cboTellerCode.Text.Trim() + "/";
                strObject += "From:" + m_strFrom + " to " + txtFrom.Text.Trim() + "/";
                strObject += "To:" + m_strTo + " to " + txtTo.Text.Trim() + "/";

                //if (AuditTrail.InsertTrail(result, m_sUser, "DROOO -E", "or_assigned", strObject) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // RMC 20130516 added trailing (e)

                btnEdit.Text = "&Edit";
                btnClose.Text = "&Close";
                btnEdit.Enabled = false;
                dgvList.Enabled = true;
                //cboFormType.Enabled = true;
                txtFrom.Enabled = false;
                txtTo.Enabled = false;
                groupBox1.Enabled = true;
                ExpandInterface(false);
                LoadList();
                MessageBox.Show("O.R. Series for " + cboTellerCode.Text + " Updated Successfully\nTeller:" + cboTellerCode.Text +
                    "\nFrom:" + m_strFrom + " to " + txtFrom.Text +
                    "\nTo:" + m_strTo + " to " + txtTo.Text + "", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnClear.PerformClick();
                dgvAvailableOR.Rows.Clear();
                dgvList.ClearSelection();
            }
        }

        private void dgvListORNum_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // AST 20140121 Added validation for null data
            if (dgvAvailableOR.RowCount > 0)
            {
                txtFrom.Text = dgvAvailableOR.CurrentRow.Cells[0].Value.ToString();
                txtTo.Text = dgvAvailableOR.CurrentRow.Cells[1].Value.ToString();
                if (btnEdit.Text == "&Update")
                    btnEdit.Enabled = true;
                else
                    btnDeclare.Enabled = true;
            }
        }

        private void cboTellerCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        // RMC 20130522 corrections in OR declaration (s)
        private void ValidateORUsed(string p_sFromORNum, string p_sToORNum, out string p_sORFr, out bool p_bInsertData)
        {
            OracleResultSet p_Set = new OracleResultSet();
            string sTmpORNo = string.Empty;
            string sStrFormat = string.Empty;
            int iStrLen = 0;
            int iORFr = 0;

            bool bInsertData = false;
            string sORFr = string.Empty;
            int iLen = p_sFromORNum.Length;

            p_Set.Query = "select * from or_used, or_inv where or_used.or_no between or_inv.from_or_no and or_inv.to_or_no ";
            p_Set.Query += " and or_inv.from_or_no = '" + p_sFromORNum + "' and or_inv.to_or_no = '" + p_sToORNum + "'";
            p_Set.Query += " and length(or_no) = " + iLen + " ";    // RMC 20130605 corrections in OR declaration
            p_Set.Query += " and or_used.form_type = or_inv.form_type"; // RMC 20130913 corrections in OR declaration
            p_Set.Query += " and or_inv.FORM_TYPE = '" + "" + "'";   // RMC 20131002 corrected wrong available OR series in OR Declaration
            //p_Set.Query += " and substr(or_used.trn_date, 8, 2) = '" + sCurrYear + "'"; // AST 20131003 Add filtering of O.R. per Year
            //p_Set.Query += " and substr(or_inv.date_created, 8, 2) = '" + sCurrYear + "'"; // AST 20131003 Add filtering of O.R. per Year
            p_Set.Query += " order by or_used.or_no desc";
            if (p_Set.Execute())
            {
                if (p_Set.Read())
                {
                    sTmpORNo = p_Set.GetString("or_no");
                    iORFr = Convert.ToInt32(sTmpORNo) + 1;

                    iStrLen = sTmpORNo.Length;

                    sStrFormat = "";
                    for (int i = 1; i <= iStrLen; i++)
                    {
                        sStrFormat += "0";
                    }

                    if (sTmpORNo == p_sToORNum)
                    {
                        bInsertData = false;
                        sORFr = "";
                    }
                    else
                    {
                        bInsertData = true;
                        sORFr = string.Format("{0:" + sStrFormat + "}", iORFr);
                    }
                }
                else
                {
                    bInsertData = true;
                    sORFr = p_sFromORNum;
                }
            }
            p_Set.Close();

            p_bInsertData = bInsertData;
            p_sORFr = sORFr;
           
        }
        // RMC 20130522 corrections in OR declaration (e)

        private void cboFormType_SelectedValueChanged(object sender, EventArgs e)
        {
            cboTellerCode.Enabled = true;
            btnClear.PerformClick();
        }

        private void cboTellerCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (m_blnComboSelected == false)
                //return;
            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT teller_ln, teller_fn, teller_mi FROM tellers ";
            result.Query += "WHERE teller_code = '" + cboTellerCode.Text.Trim() + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    txtLName.Text = result.GetString("TELLER_LN").Trim();
                    txtFName.Text = result.GetString("TELLER_FN").Trim();
                    txtMI.Text = result.GetString("TELLER_MI").Trim();
                }
            }

            string sFormType = string.Empty;
            result.Query = "SELECT FROM_OR_NO, TO_OR_NO FROM OR_ASSIGNED ";
            result.Query += "WHERE TELLER_CODE = '" + cboTellerCode.Text.Trim() + "' ";
            result.Query += "AND FORM_TYPE = '" + "" + "' ";
            //m_Set.Query += "AND SUBSTR(DATE_ASSIGNED, 8, 2) = '" + sCurrYear + "'"; // AST 20131003 Add filtering of O.R. per Year
            if (result.Execute())
            {
                if (result.Read())
                {
                    txtFrom.Text = result.GetString("FROM_OR_NO");
                    txtTo.Text = result.GetString("TO_OR_NO");

                    ExpandInterface(false);
                    txtFrom.ReadOnly = true;
                    txtTo.ReadOnly = true;
                    dgvList.Enabled = true;
                    btnDeclare.Enabled = false;
                    btnEdit.Enabled = true;
                    btnReturn.Enabled = true;
                }
                else
                {
                    ExpandInterface(true);
                    GetAvailableOR();
                    txtFrom.Clear();
                    txtTo.Clear();
                    btnClose.Text = "&Cancel";
                    dgvList.Enabled = false;
                    dgvList.ClearSelection();
                    btnDeclare.Enabled = false;
                    btnEdit.Enabled = false;
                    btnReturn.Enabled = false;
                }
            }
            result.Close();            
        }

        private void GetTellerInfo()
        {
            
        }

        private void ExpandInterface(bool ExpandInterface)
        {
            if (ExpandInterface)
            {
                if (dgvAvailableOR.RowCount == 0)
                {
                    //MessageBox.Show("No Available O.R. for " + cboFormType.Text + "", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return;
                }
                this.Size = new Size(476, 686);
                pnlORSeries.Location = new Point(12, 520);
                pnlAvailableOR.Location = new Point(13, 325);
            }
            else
            {
                this.Size = new Size(466, 483);
                pnlORSeries.Location = new Point(12, 325);
                pnlAvailableOR.Location = new Point(12, 457);
            }
            this.CenterToScreen();
        }

        private void cboTellerCode_DropDown(object sender, EventArgs e)
        {
            //m_blnComboSelected = true;
        }
    }
}
