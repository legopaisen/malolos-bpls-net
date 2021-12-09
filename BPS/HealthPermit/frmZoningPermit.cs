using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Common.SearchBusiness;
using Amellar.Common.TransactionLog;


namespace Amellar.Modules.HealthPermit
{
    public partial class frmZoningPermit : Form
    {
        private string m_sPermitNumber = string.Empty;
        private string m_sCertPermType = string.Empty;
        private string m_sIssuedDate = string.Empty;
        private bool bRenewal = false; //JARS 20170111
        private string m_sTmpBIN = string.Empty;
        private string m_sBIN = string.Empty;

        
        string TimeIN = string.Empty;

        public frmZoningPermit()
        {
            InitializeComponent();
        }

        private void frmZoningPermit_Load(object sender, EventArgs e)
        {
            txtSP.Text = "308 dated Oct. 20, 2003"; // this is editable
            txtMP.Text = "27 series of 2003";   // this is editable
            txtTaxYear.Text = AppSettingsManager.GetConfigValue("12");
            UpdateList();
            btnSearchTmp.Enabled = true; //JARS 20160222
            btnSearchRen.Enabled = true;
        }

        private void UpdateList()
        {
            dgvList.Columns.Clear();
            dgvList.Columns.Add("1", "BIN"); // RMC 20150102 mods in permit
            dgvList.Columns.Add("2","Tax Year");
            dgvList.Columns.Add("3","Last Name");
            dgvList.Columns.Add("4","First Name");
            dgvList.Columns.Add("5","M.I.");
            dgvList.Columns.Add("6","TCT No.");
            dgvList.Columns.Add("7","Zoning");
            dgvList.Columns.Add("8","SP No.");
            dgvList.Columns.Add("9","MP No.");
            dgvList.Columns.Add("10", "BIN");
            dgvList.Columns[0].Visible = false;
            dgvList.Columns[1].Width = 80;
            dgvList.Columns[2].Width = 100;
            dgvList.Columns[3].Width = 100;
            dgvList.Columns[4].Width = 20;
            

            OracleResultSet pSet = new OracleResultSet();
            String sQuery = "";
            //pSet.Query = "select BIN, TAX_YEAR, OWN_LN, OWN_FN, OWN_MI, TCT_NO, ZONING, SP_NO, MP_NO from zoning where tax_year = '" + txtTaxYear.Text + "' order by bin";
            
            sQuery = "select distinct * from ( ";
            sQuery += "select distinct z.BIN, z.TAX_YEAR, o.OWN_LN, o.OWN_FN, o.OWN_MI, z.TCT_NO, z.ZONING, z.SP_NO, z.MP_NO from zoning z ";
            sQuery += "inner join businesses B on B.bin = z.Bin ";
            sQuery += "inner join own_names O on o.Own_code = b.own_code ";
            sQuery += "union all ";
            sQuery += "select distinct z.BIN, z.TAX_YEAR, o.OWN_LN, o.OWN_FN, o.OWN_MI, z.TCT_NO, z.ZONING, z.SP_NO, z.MP_NO from zoning z ";
            sQuery += "inner join business_que B on B.bin = z.Bin ";
            sQuery += "inner join own_names O on o.Own_code = b.own_code ";
            sQuery += "union all ";
            sQuery += "select distinct BIN, TAX_YEAR, OWN_LN, OWN_FN, OWN_MI, TCT_NO, ZONING, SP_NO, MP_NO from zoning ";
            sQuery += "where bin not in (select bin from businesses) and bin not in (select bin from business_que) ";
            sQuery += ") where tax_year = '" + txtTaxYear.Text + "' order by bin ";

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dgvList.Rows.Add(pSet.GetString(0), pSet.GetString(1), pSet.GetString(2), 
                        pSet.GetString(3), pSet.GetString(4), pSet.GetString(5),
                        pSet.GetString(6), pSet.GetString(7), pSet.GetString(8), pSet.GetString(0));
                }
            }
            pSet.Close();
        }

        private void EnableControls(bool blnEnable)
        {
            /*txtLastName.ReadOnly = !blnEnable;
            txtFirstName.ReadOnly = !blnEnable;
            txtMiddleInitial.ReadOnly = !blnEnable;*/   // RMC 20150102 mods in permit, put rem
            btnSearchTmp.Enabled = blnEnable;   // RMC 20150102 mods in permit
            txtTCTNo.ReadOnly = !blnEnable;
            cboZoning.Enabled = blnEnable;  // RMC 20150102 mods in permit
            txtSP.ReadOnly = !blnEnable;
            txtMP.ReadOnly = !blnEnable;
            cboZoning.Enabled = blnEnable;  // RMC 20150102 mods in permit
        }

        private void ClearControls()
        {
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMiddleInitial.Text = "";
            txtTCTNo.Text = "";
            cboZoning.Text = "";
            m_sTmpBIN = "";
            m_sBIN = "";
            txtTmpBin.Text = "";    // RMC 20150102 mods in permit

            if (TaskMan.IsObjectLock(txtTmpBin.Text, "ZONING PERMIT", "DELETE", "ASS"))  // RMC 20150102 mods in permit
            {
            }

        }

        private void PermitRecord()
        {
            String m_sPermitNumber = "";
            #region GetNextSeries
            OracleResultSet result = new OracleResultSet();
            String sQuery = String.Empty;
            result.Query = "select coalesce(max(to_number(permit_number)),0)+1 as permit_number from permit_type where perm_type = 'ZONING' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            if (result.Execute())
            {
                if (result.Read())
                    m_sPermitNumber = AppSettingsManager.GetConfigValue("12") + "-" + result.GetInt(0).ToString("0000");
            }
            result.Close();
            #endregion

            #region CheckExist
            result.Query = "select * from permit_type where bin = '" + m_sTmpBIN + "' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "' and perm_type = 'ZONING'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sPermitNumber = result.GetString(2);
                    m_sIssuedDate = result.GetString(3);
                }
                else
                    m_sIssuedDate = ""; //MCR 20150218
            }
            result.Close();
            #endregion
            #region Saving

            if (m_sIssuedDate == "")
            {
                string sCurrentYear = AppSettingsManager.GetConfigValue("12");
                string sPermType = "ZONING";
                string sPermitNumber = m_sPermitNumber.Substring(5);
                string sIssuedDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                string sUserCode = AppSettingsManager.SystemUser.UserCode;
                string s_mBin = m_sTmpBIN;

                result.Query = "insert into permit_type (current_year,perm_type,permit_number,issued_date,user_code,bin) values('" + sCurrentYear + "', '" + sPermType + "', '" + sPermitNumber + "', '" + sIssuedDate + "', '" + sUserCode + "', '" + s_mBin + "')";
                result.ExecuteNonQuery();
            }
            #endregion
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            
            TimeIN = AppSettingsManager.GetSystemDate().ToString();
          
            if (btnNew.Text == "Add")
            {
                EnableControls(true);
                ClearControls();
                btnNew.Text = "Save";
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnPrint.Enabled = false;
                btnClose.Enabled = false;
                txtLastName.Focus();
            }
            else
            {
                OracleResultSet pSet = new OracleResultSet();
                if (txtLastName.Text.Trim().ToString() == "" || txtFirstName.Text.Trim().ToString() == "" || txtMiddleInitial.Text.Trim().ToString() == "" ||
                    txtTCTNo.Text.Trim().ToString() == "" || cboZoning.Text.Trim().ToString() == "" || txtSP.Text.Trim().ToString() == "" || txtMP.Text.Trim().ToString() == "")
                {
                    MessageBox.Show("Complete all information first",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return;
                }

                int iCnt = 0;

                pSet.Query = "select count(*) from zoning where own_ln = '" + txtLastName.Text.Trim() + "'";
                pSet.Query += " and own_fn = '" + txtFirstName.Text.Trim() + "'";
                pSet.Query += " and own_mi = '" + txtMiddleInitial.Text.Trim() + "'";
                pSet.Query += " and bin = '" + m_sBIN + "'";
                int.TryParse(pSet.ExecuteScalar(), out iCnt);

                if (iCnt > 0)
                {
                    MessageBox.Show("Permit already issued to business owner",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show("Save record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //string sTmpBin = GenTempBin();
                    string sTmpBin = txtTmpBin.Text;    // RMC 20150102 mods in permit

                    pSet.Query = "insert into zoning (tax_year, bin, own_ln, own_fn, own_mi, tct_no, zoning, sp_no, mp_no, bns_user, entry_date)";
                    pSet.Query += " values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11)";
                    pSet.AddParameter(":1",txtTaxYear.Text);
                    pSet.AddParameter(":2", sTmpBin);
                    pSet.AddParameter(":3", StringUtilities.HandleApostrophe(txtLastName.Text.Trim()));
                    pSet.AddParameter(":4", StringUtilities.HandleApostrophe(txtFirstName.Text.Trim()));
                    pSet.AddParameter(":5", StringUtilities.HandleApostrophe(txtMiddleInitial.Text.Trim()));
                    pSet.AddParameter(":6", StringUtilities.HandleApostrophe(txtTCTNo.Text.Trim()));
                    pSet.AddParameter(":7", StringUtilities.HandleApostrophe(cboZoning.Text.Trim()));
                    pSet.AddParameter(":8", StringUtilities.HandleApostrophe(txtSP.Text.Trim()));
                    pSet.AddParameter(":9", StringUtilities.HandleApostrophe(txtMP.Text.Trim()));
                    pSet.AddParameter(":10", StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));
                    pSet.AddParameter(":11", AppSettingsManager.GetCurrentDate());
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    string sObj = "Temp BIN: " + sTmpBin + "/Own Name: " + txtLastName.Text + ", " + txtFirstName.Text + " " + txtMiddleInitial.Text + "/TCT: " + txtTCTNo.Text.Trim();
                    if (AuditTrail.InsertTrail("ABZC-A", "zoning", StringUtilities.HandleApostrophe(sObj)) == 0)
                    {
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Record saved.\nTemporary BIN: " +sTmpBin, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateList();
                    m_sTmpBIN = sTmpBin;
                    bRenewal = false;
                    PermitRecord(); // RMC 20150105 mods in permit printing
                }

                //PermitRecord();

                EnableControls(false);
                btnNew.Text = "Add";
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnPrint.Enabled = true;
                btnClose.Enabled = true;
            }
            
        }

        private string GenTempBin()
        {
            OracleResultSet pCmd = new OracleResultSet();
            int iSeries = 0;
            string sTmpBIN = string.Empty;

            pCmd.Query = "select * from zoning where tax_year = '" + txtTaxYear.Text + "' order by temp_bin desc";
            if (pCmd.Execute())
            {
                if (pCmd.Read())
                {
                    sTmpBIN = pCmd.GetString("temp_bin");
                    int.TryParse(StringUtilities.Right(sTmpBIN,7), out iSeries);
                    iSeries++;
                }
                else
                {
                    iSeries = 1;
                }
            }
            pCmd.Close();

            sTmpBIN = string.Format("{0}-{1:000000#}",txtTaxYear.Text,iSeries);
            return sTmpBIN;
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Discard changes?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClearControls();
                EnableControls(false);
                btnNew.Text = "Add";
                btnEdit.Text = "Edit";
                btnNew.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnPrint.Enabled = true;
                btnClose.Enabled = true;
                btnSearchTmp.Text = "Search";   // RMC 20150102 mods in permit
                btnSearchRen.Text = "Search Renewal"; //JARS
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (TaskMan.IsObjectLock(txtTmpBin.Text, "ZONING PERMIT", "DELETE", "ASS"))  // RMC 20150102 mods in permit
            {
            }

            this.Close();
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                m_sTmpBIN = dgvList[0, e.RowIndex].Value.ToString();
                txtTaxYear.Text = dgvList[1, e.RowIndex].Value.ToString();
                txtLastName.Text = dgvList[2, e.RowIndex].Value.ToString();
                txtFirstName.Text = dgvList[3, e.RowIndex].Value.ToString();
                txtMiddleInitial.Text = dgvList[4, e.RowIndex].Value.ToString();
                txtTCTNo.Text = dgvList[5, e.RowIndex].Value.ToString();
                cboZoning.Text = dgvList[6, e.RowIndex].Value.ToString();
                txtSP.Text = dgvList[7, e.RowIndex].Value.ToString();
                txtMP.Text = dgvList[8, e.RowIndex].Value.ToString();
                m_sBIN = dgvList[9, e.RowIndex].Value.ToString();
                txtTmpBin.Text = m_sTmpBIN; // RMC 20150102 mods in permit
            }
            catch {
                m_sTmpBIN = "";
                m_sBIN = "";
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if(m_sTmpBIN == "")
            {
                MessageBox.Show("Select record to edit first",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            /*if (m_sBIN.Trim() != "")
            {
                MessageBox.Show("Selected record has been applied.\nEditing not allowed.\nCancel application of BIN: " + m_sBIN + " first if you will edit selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }*/            // RMC 20150102 mods in permit

            if (btnEdit.Text == "Edit")
            {
                EnableControls(true);
                btnEdit.Text = "Update";
                btnNew.Enabled = false;
                btnDelete.Enabled = false;
                btnPrint.Enabled = false;
                btnClose.Enabled = false;
                btnSearchTmp.Enabled = false;
                btnSearchRen.Enabled = false;
            }
            else
            {
                //string  BnsOrgKind = 
                if (AppSettingsManager.GetBnsOrgKind(m_sTmpBIN) == "CORPORATION")

                { }
                else
                {
                    if (txtLastName.Text.Trim().ToString() == "" || txtFirstName.Text.Trim().ToString() == "" || txtMiddleInitial.Text.Trim().ToString() == "" ||
                        txtTCTNo.Text.Trim().ToString() == "" || cboZoning.Text.Trim().ToString() == "" || txtSP.Text.Trim().ToString() == "" || txtMP.Text.Trim().ToString() == "")
                    {
                        MessageBox.Show("Complete all information first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }

                int iCnt = 0;

                pSet.Query = "select count(*) from zoning where own_ln = '" + txtLastName.Text.Trim() + "'";
                pSet.Query += " and own_fn = '" + txtFirstName.Text.Trim() + "'";
                pSet.Query += " and own_mi = '" + txtMiddleInitial.Text.Trim() + "'";
                pSet.Query += " and temp_bin <> '" + m_sTmpBIN + "'";
                int.TryParse(pSet.ExecuteScalar(), out iCnt);

                if (iCnt > 0)
                {
                    MessageBox.Show("Permit already issued to business owner", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                //check if already printed
                pSet.Query = "select count(*) from a_trail where object like '%" + m_sTmpBIN + "%'";
                pSet.Query += " and mod_code = 'ABZC-P'"; //check code
                int.TryParse(pSet.ExecuteScalar(), out iCnt);

                if (iCnt > 0)
                {
                    if (MessageBox.Show("Zoning certificate already been printed for the selected record.\nContinue updating?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                if (MessageBox.Show("Update record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet pSet2 = new OracleResultSet();
                    string sObj = string.Empty;

                    pSet2.Query = "select * from zoning where bin = '" + m_sTmpBIN + "'";
                    if (pSet2.Execute())
                    {
                        if (pSet2.Read())
                        {
                            pSet.Query = "update zoning set ";
                            pSet.Query += "own_ln = '" + StringUtilities.HandleApostrophe(txtLastName.Text.Trim()) + "', ";
                            pSet.Query += "own_fn = '" + StringUtilities.HandleApostrophe(txtFirstName.Text.Trim()) + "', ";
                            pSet.Query += "own_mi = '" + StringUtilities.HandleApostrophe(txtMiddleInitial.Text.Trim()) + "', ";
                            pSet.Query += "tct_no = '" + StringUtilities.HandleApostrophe(txtTCTNo.Text.Trim()) + "', ";
                            pSet.Query += "zoning= '" + StringUtilities.HandleApostrophe(cboZoning.Text.Trim()) + "', ";
                            pSet.Query += "sp_no = '" + StringUtilities.HandleApostrophe(txtSP.Text.Trim()) + "', ";
                            pSet.Query += "mp_no = '" + StringUtilities.HandleApostrophe(txtMP.Text.Trim()) + "', ";
                            pSet.Query += "bns_user = '" + AppSettingsManager.SystemUser.UserCode + "', ";
                            pSet.Query += "entry_date = :1";
                            //pSet.Query += " where temp_bin = '" + m_sTmpBIN + "'";
                            pSet.Query += " where bin = '" + m_sTmpBIN + "'"; //JARS 20160921
                            pSet.AddParameter(":1", AppSettingsManager.GetCurrentDate());
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            sObj = "Temp BIN: " + m_sTmpBIN;
                            if (pSet2.GetString("own_ln") != txtLastName.Text.Trim())
                                sObj += "/Last Name: " + pSet2.GetString("own_ln") + " to " + txtLastName.Text.Trim();
                            if (pSet2.GetString("own_fn") != txtFirstName.Text.Trim())
                                sObj += "/First Name: " + pSet2.GetString("own_fn") + " to " + txtFirstName.Text.Trim();
                            if (pSet2.GetString("own_mi") != txtMiddleInitial.Text.Trim())
                                sObj += "/M.I.: " + pSet2.GetString("own_mi") + " to " + txtMiddleInitial.Text.Trim();
                            if (pSet2.GetString("tct_no") != txtTCTNo.Text.Trim())
                                sObj += "/TCT: " + pSet2.GetString("tct_no") + " to " + txtTCTNo.Text.Trim();
                            if (pSet2.GetString("zoning") != cboZoning.Text.Trim())
                                sObj += "/Zoning: " + pSet2.GetString("zoning") + " to " + cboZoning.Text.Trim();
                            if (pSet2.GetString("sp_no") != txtSP.Text.Trim())
                                sObj += "/SP No.: " + pSet2.GetString("sp_no") + " to " + txtSP.Text.Trim();
                            if (pSet2.GetString("mp_no") != txtMP.Text.Trim())
                                sObj += "/MP No.: " + pSet2.GetString("mp_no") + " to " + txtMP.Text.Trim();

                            if (AuditTrail.InsertTrail("ABZC-E", "zoning", StringUtilities.HandleApostrophe(sObj)) == 0)
                            {
                                MessageBox.Show(pSet2.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    pSet2.Close();

                    MessageBox.Show("Record updated", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateList();
                    
                }

                btnEdit.Text = "Edit";
                btnNew.Enabled = true;
                btnDelete.Enabled = true;
                btnPrint.Enabled = true;
                btnClose.Enabled = true;
                EnableControls(false);
            }

            PermitRecord();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (m_sTmpBIN == "")
            {
                MessageBox.Show("Select data to delete first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            OracleResultSet pSet = new OracleResultSet();
            int iCnt = 0;
            //check if already printed
            pSet.Query = "select count(*) from a_trail where object like '%" + m_sTmpBIN + "%'";
            pSet.Query += " and mod_code = 'ABZC-P'";
            int.TryParse(pSet.ExecuteScalar(), out iCnt);

            if (iCnt > 0)
            {
                if (MessageBox.Show("Zoning certificate already been printed for the selected record.\nContinue deleting?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            if (MessageBox.Show("Delete record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet pCmd = new OracleResultSet();
                string sObj = string.Empty;

                //pCmd.Query = "delete from zoning where temp_bin = '" + m_sTmpBIN + "'";
                pCmd.Query = "delete from zoning where bin = '" + m_sTmpBIN + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                sObj = "Temp BIN: " + m_sTmpBIN;
                sObj += "/Last Name:" + txtLastName.Text.Trim();
                sObj += "/First Name: " + txtFirstName.Text.Trim();
                sObj += "/M.I.: " + txtMiddleInitial.Text.Trim();
                sObj += "/TCT: " + txtTCTNo.Text.Trim();
                sObj += "/Zoning: " + cboZoning.Text.Trim();
                sObj += "/SP No.: " + txtSP.Text.Trim();
                sObj += "/MP No.: " + txtMP.Text.Trim();

                if (AuditTrail.InsertTrail("ABZC-D", "zoning", StringUtilities.HandleApostrophe(sObj)) == 0)
                {
                    MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Record deleted", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateList();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
       {
            frmPrinting form = new frmPrinting();
            form.ReportType = "Zoning";
            form.BIN = txtTmpBin.Text; //JARS 20160610
            //form.BIN = m_sTmpBIN;
            form.timeIN = TimeIN;
         
            form.TaxYear = AppSettingsManager.GetConfigValue("12");
            bRenewal = false;
            form.ShowDialog();      

        }

        private void btnSearchTmp_Click(object sender, EventArgs e)
        {
            // RMC 20150102 mods in permit
            if (btnSearchTmp.Text == "Search")
            {
                btnSearchTmp.Text = "Clear";
                frmSearchTmp form = new frmSearchTmp();
                form.Permit = "Zoning";
                form.TaxYear = txtTaxYear.Text;
                form.ShowDialog();
                txtTmpBin.Text = form.BIN;
                m_sTmpBIN = form.BIN;

                //if (!TaskMan.IsObjectLock(txtTmpBin.Text, "ZONING PERMIT", "ADD", "ASS"))
                //{
                    txtLastName.Text = form.LastName;
                    txtFirstName.Text = form.FirstName;
                    txtMiddleInitial.Text = form.MI;
                //}
                //else
                //{
                //    ClearControls();
                //}

            }
            else
            {
                btnSearchTmp.Text = "Search";
                ClearControls();
            }
        }

        private void kryptonButton1_Click(object sender, EventArgs e) //JARS 20170112
        {
            if (MessageBox.Show("Print Zoning Permit for a Renewed Business?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (btnSearchRen.Text == "Search Renewal")
                {
                    bRenewal = true;
                    btnSearchRen.Text = "Clear";
                    frmSearchBusiness form = new frmSearchBusiness();
                    //form.Permit = "Zoning";
                    //form.TaxYear = txtTaxYear.Text;
                    form.ShowDialog();
                    txtTmpBin.Text = form.sBIN;
                    m_sTmpBIN = form.sBIN;

                    txtLastName.Text = form.sLastName;
                    txtFirstName.Text = form.sFirstName;
                    txtMiddleInitial.Text = form.sMiddleInitial;

                    //if (!TaskMan.IsObjectLock(txtTmpBin.Text, "ZONING PERMIT", "ADD", "ASS"))
                    //{
                    //    txtLastName.Text = form.LastName;
                    //    txtFirstName.Text = form.FirstName;
                    //    txtMiddleInitial.Text = form.MI;
                    //}
                    //else
                    //{
                    //    ClearControls();
                    //}

                }
                else
                {
                    bRenewal = false;
                    btnSearchRen.Text = "Search Renewal";
                    ClearControls();
                }
            }
            else
            {
                return;
            }

        }
        
    }
}