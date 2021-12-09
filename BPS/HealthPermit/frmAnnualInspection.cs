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
using Amellar.Common.SearchBusiness;
using Amellar.Common.AuditTrail;
using Amellar.Common.TransactionLog;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmAnnualInspection : Form
    {
        
        private string m_sCertPermType = string.Empty;
        
        private string m_sBIN = string.Empty;

        string TimeIN = string.Empty;


        public frmAnnualInspection()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
            bin1.txtTaxYear.ReadOnly = true;
            bin1.txtBINSeries.ReadOnly = true;
        }

        String m_sPermitNumber = "";
        String m_sIssuedDate = "";

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
                if (rdoRenewal.Checked == true)
                {
                    if (bin1.txtTaxYear.Text.ToString() == "" || bin1.txtBINSeries.Text.ToString() == "")
                    {
                        MessageBox.Show("Select BIN first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    m_sBIN = bin1.GetBin();
                }

                if (rdoNew.Checked == true)
                {
                    if (txtTmpBin.Text.Trim() == "")
                    {
                        MessageBox.Show("Select temporary BIN first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    m_sBIN = txtTmpBin.Text.Trim();
                }

                // RMC 20150113 corrections in annual inspection entry of area (s)
                double dArea = 0;
                double.TryParse(txtStallNo.Text, out dArea);

                if (dArea == 0)
                {
                    MessageBox.Show("Invalid area. Please enter numeric only.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                // RMC 20150113 corrections in annual inspection entry of area (e)

                // RMC 20141228 modified permit printing (lubao) (s)
                if (txtGrp.Text.Trim() == "" || txtLoc.Text.Trim() == "" || txtCertOcc.Text.Trim() == "" || txtORNo.Text.Trim() == "" || txtStallNo.Text.Trim() == "")
                {
                    MessageBox.Show("All fields are required.", this.Text,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return;
                }
                // RMC 20141228 modified permit printing (lubao) (e)

                OracleResultSet pSet = new OracleResultSet();
                int iCnt = 0;
                pSet.Query = "select count(*) from annual_insp where bin = '" + m_sBIN + "'";
                pSet.Query += " and tax_year = '" + txtTaxYear.Text + "'";
                int.TryParse(pSet.ExecuteScalar(), out iCnt);

                if (iCnt > 0)
                {
                    MessageBox.Show("Permit already issued to business owner", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show("Save record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pSet.Query = "insert into annual_insp values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14)";
                    pSet.AddParameter(":1", txtTaxYear.Text);
                    pSet.AddParameter(":2", m_sBIN);
                    pSet.AddParameter(":3", StringUtilities.HandleApostrophe(txtLastName.Text.Trim()));
                    pSet.AddParameter(":4", StringUtilities.HandleApostrophe(txtFirstName.Text.Trim()));
                    pSet.AddParameter(":5", StringUtilities.HandleApostrophe(txtMiddleInitial.Text.Trim()));
                    pSet.AddParameter(":6", StringUtilities.HandleApostrophe(txtCharOcc.Text.Trim()));
                    pSet.AddParameter(":7", StringUtilities.HandleApostrophe(txtGrp.Text.Trim()));
                    pSet.AddParameter(":8", StringUtilities.HandleApostrophe(txtLoc.Text.Trim()));
                    pSet.AddParameter(":9", StringUtilities.HandleApostrophe(txtCertOcc.Text.Trim()));
                    pSet.AddParameter(":10", dtpDate.Value);
                    pSet.AddParameter(":11", StringUtilities.HandleApostrophe(txtORNo.Text.Trim()));
                    pSet.AddParameter(":12", StringUtilities.HandleApostrophe(txtStallNo.Text.Trim()));
                    pSet.AddParameter(":13", AppSettingsManager.SystemUser.UserCode);
                    pSet.AddParameter(":14", AppSettingsManager.GetCurrentDate());
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    UpdateOtherInfo("ADD");  // RMC 20150104 addl mods

                    string sObj = "BIN: " + m_sBIN + "/Own Name: " + txtLastName.Text + ", " + txtFirstName.Text + " " + txtMiddleInitial.Text;
                    if (AuditTrail.InsertTrail("ABAIP-A", "annual_insp", StringUtilities.HandleApostrophe(sObj)) == 0)
                    {
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Record saved", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateList();

                    PermitRecord(); // RMC 20150105 mods in permit printing     
                }

                btnExt.Enabled = true;
                EnableControls(false);
                btnNew.Text = "Add";
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnPrint.Enabled = true;
                btnClose.Enabled = true;
                btnSearchTmp.Text = "Search";   // RMC 20141228 modified permit printing (lubao)
                btnSearch.Text = "Search";  // RMC 20141228 modified permit printing (lubao)
                //ClearControls();    // RMC 20150102 mods in permit    // RMC 20150113 corrections in annual inspection entry of area
            }
           
        }

        private void frmAnnualInspection_Load(object sender, EventArgs e)
        {
            txtTaxYear.Text = AppSettingsManager.GetConfigValue("12");
            UpdateList();
        }

        private void UpdateList()
        {
            dgvList.Columns.Clear();
            dgvList.Columns.Add("1", "BIN");
            dgvList.Columns.Add("2", "Tax Year");
            dgvList.Columns.Add("3", "Last Name");
            dgvList.Columns.Add("4", "First Name");
            dgvList.Columns.Add("5", "M.I.");
            dgvList.Columns.Add("6", "Character of Occ.");
            dgvList.Columns.Add("7", "Group");
            dgvList.Columns.Add("8", "Location");
            dgvList.Columns.Add("9", "Certificate of Occ.");
            dgvList.Columns.Add("10", "Date");
            dgvList.Columns.Add("11", "O.R. No.");
            dgvList.Columns.Add("12", "Area/Stall No.");
            dgvList.Columns[0].Width = 100;
            dgvList.Columns[1].Width = 80;
            dgvList.Columns[2].Width = 100;
            dgvList.Columns[3].Width = 100;
            dgvList.Columns[4].Width = 20;


            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select BIN, TAX_YEAR, OWN_LN, OWN_FN, OWN_MI, CHAR_OCC, CHAR_GRP, LOCATION, CERT_OCC, CERT_DATE, CERT_OR_NO,STALL_NO from annual_insp where tax_year = '" + txtTaxYear.Text + "' order by bin";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    // RMC 20150117 (s)
                    string sLN = ""; string sFN = ""; string sMI = "";

                    sLN = AppSettingsManager.GetBnsOwnerLastName(AppSettingsManager.GetBnsOwnCode(pSet.GetString(0)));
                    sFN = AppSettingsManager.GetBnsOwnerFirstName(AppSettingsManager.GetBnsOwnCode(pSet.GetString(0)));
                    sMI = AppSettingsManager.GetBnsOwnerMiName(AppSettingsManager.GetBnsOwnCode(pSet.GetString(0)));

                    if (sLN.Trim() == "")
                    {
                        sLN = pSet.GetString(2);
                        sFN = pSet.GetString(3);
                        sMI = pSet.GetString(4);
                    }

                    dgvList.Rows.Add(pSet.GetString(0), pSet.GetString(1), sLN, sFN, sMI,
                        pSet.GetString(5), pSet.GetString(6), pSet.GetString(7), pSet.GetString(8),
                        pSet.GetDateTime(9), pSet.GetString(10), pSet.GetString(11));

                    // RMC 20150117 (e)


                    /*dgvList.Rows.Add(pSet.GetString(0), pSet.GetString(1),
                        AppSettingsManager.GetBnsOwnerLastName(AppSettingsManager.GetBnsOwnCode(pSet.GetString(0))),
                        AppSettingsManager.GetBnsOwnerFirstName(AppSettingsManager.GetBnsOwnCode(pSet.GetString(0))),
                        AppSettingsManager.GetBnsOwnerMiName(AppSettingsManager.GetBnsOwnCode(pSet.GetString(0))),
                        pSet.GetString(5), pSet.GetString(6), pSet.GetString(7), pSet.GetString(8),
                        pSet.GetDateTime(9), pSet.GetString(10), pSet.GetString(11));
                     */
                }
            }
            pSet.Close();
        }

        private void EnableControls(bool blnEnable)
        {
            //txtCharOcc.ReadOnly = !blnEnable; // RMC 20141228 modified permit printing (lubao)
            txtGrp.ReadOnly = !blnEnable;
            txtLoc.ReadOnly = !blnEnable;
            txtCertOcc.ReadOnly = !blnEnable;
            dtpDate.Enabled = blnEnable;
            txtORNo.ReadOnly = !blnEnable;
            txtStallNo.ReadOnly = !blnEnable;
            rdoNew.Enabled = blnEnable;
            rdoRenewal.Enabled = blnEnable;
        }

        private void ClearControls()
        {
            if (TaskMan.IsObjectLock(m_sBIN, "ANNUAL INSPECTION PERMIT", "DELETE", "ASS"))  // RMC 20141228 modified permit printing (lubao)
            {
            }

            m_sBIN = "";
            bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";
            txtTmpBin.Text = "";
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMiddleInitial.Text = "";
            txtCharOcc.Text = "";
            txtGrp.Text = "";
            txtLoc.Text = "";
            txtCertOcc.Text = "";
            dtpDate.Value = AppSettingsManager.GetCurrentDate();
            txtORNo.Text = "";
            txtStallNo.Text = "";
            //rdoNew.Checked = false;   // RMC 20150117
            //rdoRenewal.Checked = false;   // RMC 20150117
            txtDTIName.Text = "";
        }

        private void rdoNew_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNew.Checked == true)
            {
                rdoRenewal.Checked = false;
                btnSearchTmp.Enabled = true;
                ClearControls();   // RMC 20150117
            }
            else
            {
                btnSearchTmp.Enabled = false;
            }
        }

        private void rdoRenewal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRenewal.Checked == true)
            {
                rdoNew.Checked = false;
                btnSearch.Enabled = true;
                ClearControls();   // RMC 20150117
            }
            else
            {
                btnSearch.Enabled = false;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (m_sBIN == "")
            {
                MessageBox.Show("Select data to edit first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            rdoRenewal.Enabled = false;
            rdoNew.Enabled = false;

            if (btnEdit.Text == "Edit")
            {
                EnableControls(true);

                btnEdit.Text = "Update";
                btnNew.Enabled = false;
                btnDelete.Enabled = false;
                btnPrint.Enabled = false;
                btnClose.Enabled = false;
                btnSearch.Enabled = false;  // RMC 20150102 mods in permit
                btnSearchTmp.Enabled = false;   // RMC 20150102 mods in permit
            }
            else
            {
                // RMC 20141228 modified permit printing (lubao) (s)
                if (txtGrp.Text.Trim() == "" || txtLoc.Text.Trim() == "" || txtCertOcc.Text.Trim() == "" || txtORNo.Text.Trim() == "" || txtStallNo.Text.Trim() == "")
                {
                    MessageBox.Show("All fields are required.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                // RMC 20141228 modified permit printing (lubao) (e)

                // RMC 20150113 corrections in annual inspection entry of area (s)
                double dArea = 0;
                double.TryParse(txtStallNo.Text, out dArea);

                if (dArea == 0)
                {
                    MessageBox.Show("Invalid area. Please enter numeric only.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                // RMC 20150113 corrections in annual inspection entry of area (e)

                if (MessageBox.Show("Update record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet pSet2 = new OracleResultSet();
                    OracleResultSet pSet = new OracleResultSet();

                    string sObj = string.Empty;

                    pSet2.Query = "select * from annual_insp where bin = '" + m_sBIN + "'";
                    if (pSet2.Execute())
                    {
                        if (pSet2.Read())
                        {
                            pSet.Query = "update annual_insp set ";
                            pSet.Query += "char_occ = '" + StringUtilities.HandleApostrophe(txtCharOcc.Text.Trim()) + "', ";
                            pSet.Query += "char_grp = '" + StringUtilities.HandleApostrophe(txtGrp.Text.Trim()) + "', ";
                            pSet.Query += "location = '" + StringUtilities.HandleApostrophe(txtLoc.Text.Trim()) + "', ";
                            pSet.Query += "cert_occ = '" + StringUtilities.HandleApostrophe(txtCertOcc.Text.Trim()) + "', ";
                            pSet.Query += "cert_date = :1 ,";
                            pSet.Query += "cert_or_no = '" + StringUtilities.HandleApostrophe(txtORNo.Text.Trim()) + "', ";
                            pSet.Query += "stall_no = '" + StringUtilities.HandleApostrophe(txtStallNo.Text.Trim()) + "', ";
                            pSet.Query += "bns_user = '" + AppSettingsManager.SystemUser.UserCode + "', ";
                            pSet.Query += "entry_date = :2";
                            pSet.Query += " where bin = '" + m_sBIN + "'";
                            pSet.AddParameter(":1", dtpDate.Value);
                            pSet.AddParameter(":2", AppSettingsManager.GetCurrentDate());
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            UpdateOtherInfo("ADD");  // RMC 20150104 addl mods

                            sObj = "BIN: " + m_sBIN;
                            if (pSet2.GetString("char_occ") != txtCharOcc.Text.Trim())
                                sObj += "/Char Occ: " + pSet2.GetString("char_occ") + " to " + txtCharOcc.Text.Trim();
                            if (pSet2.GetString("char_grp") != txtGrp.Text.Trim())
                                sObj += "/Grp: " + pSet2.GetString("char_grp") + " to " + txtGrp.Text.Trim();
                            if (pSet2.GetString("location") != txtLoc.Text.Trim())
                                sObj += "/Location: " + pSet2.GetString("location") + " to " + txtLoc.Text.Trim();
                            if (pSet2.GetString("cert_occ") != txtCertOcc.Text.Trim())
                                sObj += "/Cert Occ: " + pSet2.GetString("cert_occ") + " to " + txtCertOcc.Text.Trim();
                            if (pSet2.GetDateTime("cert_date") != dtpDate.Value)
                                sObj += "/Date: " + pSet2.GetDateTime("cert_date") + " to " + dtpDate.Value;
                            if (pSet2.GetString("cert_or_no") != txtORNo.Text.Trim())
                                sObj += "/OR No.: " + pSet2.GetString("cert_or_no") + " to " + txtORNo.Text.Trim();
                            if (pSet2.GetString("stall_no") != txtStallNo.Text.Trim())
                                sObj += "/Stall No.: " + pSet2.GetString("stall_no") + " to " + txtStallNo.Text.Trim();

                            if (AuditTrail.InsertTrail("ABAIP-E", "annual_insp", StringUtilities.HandleApostrophe(sObj)) == 0)
                            {
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    pSet2.Close();

                    MessageBox.Show("Record updated", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateList();
                    PermitRecord(); //RMC 20150116
                }

                btnExt.Enabled = true;
                btnEdit.Text = "Edit";
                btnNew.Enabled = true;
                btnDelete.Enabled = true;
                btnPrint.Enabled = true;
                btnClose.Enabled = true;
                EnableControls(false);
                btnSearchTmp.Text = "Search";   // RMC 20141228 modified permit printing (lubao)
                btnSearch.Text = "Search";  // RMC 20141228 modified permit printing (lubao)
                btnSearch.Enabled = true;  // RMC 20150102 mods in permit
                btnSearchTmp.Enabled = true;   // RMC 20150102 mods in permit
                //ClearControls();    // RMC 20150102 mods in permit    // RMC 20150113 corrections in annual inspection entry of area
            }
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                m_sBIN = dgvList[0, e.RowIndex].Value.ToString();
                txtTaxYear.Text = dgvList[1, e.RowIndex].Value.ToString();
                txtLastName.Text = dgvList[2, e.RowIndex].Value.ToString();
                txtFirstName.Text = dgvList[3, e.RowIndex].Value.ToString();
                txtMiddleInitial.Text = dgvList[4, e.RowIndex].Value.ToString();
                txtCharOcc.Text = StringUtilities.RemoveApostrophe(dgvList[5, e.RowIndex].Value.ToString());    // RMC 20150102 mods in permit
                txtGrp.Text = dgvList[6, e.RowIndex].Value.ToString();
                txtLoc.Text = dgvList[7, e.RowIndex].Value.ToString();
                txtCertOcc.Text = dgvList[8, e.RowIndex].Value.ToString();
                dtpDate.Text = dgvList[9, e.RowIndex].Value.ToString();
                txtORNo.Text = dgvList[10, e.RowIndex].Value.ToString();
                txtStallNo.Text = dgvList[11, e.RowIndex].Value.ToString();
                txtDTIName.Text = AppSettingsManager.GetDTIName(m_sBIN);    // RMC 20150107

                OracleResultSet pSet = new OracleResultSet();

                //pSet.Query = "select * from businesses where bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";    // RMC 20150102 mods in permit, put rem
                pSet.Query = "select * from business_que where bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";    // RMC 20150102 mods in permit
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        if (pSet.GetString("bns_stat") == "NEW")
                        {
                            rdoNew.Checked = true;
                            txtTmpBin.Text = m_sBIN;
                            bin1.txtTaxYear.Text = "";  // RMC 20150102 mods in permit
                            bin1.txtBINSeries.Text = "";    // RMC 20150102 mods in permit
                        }
                        else
                        {
                            rdoRenewal.Checked = true;
                            bin1.txtTaxYear.Text = m_sBIN.Substring(7, 4);  //019-08-2014-0000010
                            bin1.txtBINSeries.Text = m_sBIN.Substring(12, 7);
                            txtTmpBin.Text = "";    // RMC 20150102 mods in permit
                        }
                            
                    }
                    else
                    {
                        pSet.Close();
                        //pSet.Query = "select * from business_que where bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                        pSet.Query = "select * from businesses where bin = '" + m_sBIN + "'";    // RMC 20150102 mods in permit
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                //if (pSet.GetString("bns_stat") == "NEW")
                                // RMC 20150102 mods in permit (s)
                                string sTaxYear = txtTaxYear.Text;
                                if (pSet.GetString("bns_stat") == "NEW" && pSet.GetString("tax_year") == sTaxYear)
                                // RMC 20150102 mods in permit (e)
                                {
                                    rdoNew.Checked = true;
                                    txtTmpBin.Text = m_sBIN;
                                    bin1.txtTaxYear.Text = "";  // RMC 20150102 mods in permit
                                    bin1.txtBINSeries.Text = "";    // RMC 20150102 mods in permit
                                }
                                else
                                {
                                    rdoRenewal.Checked = true;
                                    bin1.txtTaxYear.Text = m_sBIN.Substring(7, 4);  //019-08-2014-0000010
                                    bin1.txtBINSeries.Text = m_sBIN.Substring(12, 7);
                                    txtTmpBin.Text = "";    // RMC 20150102 mods in permit
                                }
                            }
                            else
                            {
                                pSet.Close();
                                pSet.Query = "select * from emp_names where temp_bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                                if (pSet.Execute())
                                {
                                    if (pSet.Read())
                                    {
                                        rdoNew.Checked = true;
                                        txtTmpBin.Text = m_sBIN;
                                        bin1.txtTaxYear.Text = "";  // RMC 20150102 mods in permit
                                        bin1.txtBINSeries.Text = "";    // RMC 20150102 mods in permit
                                    }
                                }
                                pSet.Close();
                            }
                        }
                        pSet.Close();
                    }
                }
                pSet.Close();
            }
            catch
            {
                m_sBIN = "";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                btnSearch.Text = "Clear";

                if (bin1.txtTaxYear.Text == "" || bin1.txtBINSeries.Text == "")
                {
                    frmSearchBusiness fSearch = new frmSearchBusiness();
                    fSearch.ShowDialog();
                    if (fSearch.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = fSearch.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = fSearch.sBIN.Substring(12, 7).ToString();
                        m_sBIN = bin1.GetBin();
                    }
                }
                else
                    m_sBIN = bin1.GetBin();

                // RMC 20141228 modified permit printing (lubao) (s)
                if (!TaskMan.IsObjectLock(m_sBIN, "ANNUAL INSPECTION PERMIT", "ADD", "ASS"))
                {
                    LoadInfo(m_sBIN);
                }
                else
                {
                    m_sBIN = "";
                    bin1.txtTaxYear.Text = "";
                    bin1.txtBINSeries.Text = "";
                }
                // RMC 20141228 modified permit printing (lubao) (e)

                

            }
            else
            {
                btnSearch.Text = "Search";
                ClearControls();
            }
        }

        private void LoadInfo(string sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();

            string sOwnCode = AppSettingsManager.GetBnsOwnCode(sBIN);

            pSet.Query = "select * from own_names where own_code = '" + sOwnCode + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtLastName.Text = pSet.GetString("own_ln");
                    txtFirstName.Text = pSet.GetString("own_fn");
                    txtMiddleInitial.Text = pSet.GetString("own_mi");
                }
            }
            pSet.Close();

            txtCharOcc.Text = AppSettingsManager.GetBnsName(sBIN);  // RMC 20141228 modified permit printing (lubao)
            txtLoc.Text = AppSettingsManager.GetBnsAddress(sBIN);
            // RMC 20150107 (S)
            txtDTIName.Text = AppSettingsManager.GetDTIName(sBIN);
            if (txtDTIName.Text == "")
                txtDTIName.Text = txtCharOcc.Text;
            // RMC 20150107 (E)
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
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (TaskMan.IsObjectLock(m_sBIN, "ANNUAL INSPECTION PERMIT", "DELETE", "ASS"))  // RMC 20141228 modified permit printing (lubao)
            {
            }

            this.Close();
        }

        private void btnSearchTmp_Click(object sender, EventArgs e)
        {
            if (btnSearchTmp.Text == "Search")  // RMC 20141228 modified permit printing (lubao)
            {
                btnSearchTmp.Text = "Clear";    // RMC 20141228 modified permit printing (lubao)
                frmSearchTmp form = new frmSearchTmp();
                form.Permit = "Annual Inspection";  // RMC 20150102 mods in permit
                form.TaxYear = txtTaxYear.Text;
                form.ShowDialog();

                txtTmpBin.Text = form.BIN;
                txtLastName.Text = form.LastName;
                txtFirstName.Text = form.FirstName;
                txtMiddleInitial.Text = form.MI;
                txtCharOcc.Text = form.BnsName; // RMC 20141228 modified permit printing (lubao)
                txtLoc.Text = form.BnsAdd;  // RMC 20141228 modified permit printing (lubao)
                /*txtLoc.Text = AppSettingsManager.GetConfigValue("02");
                if (AppSettingsManager.GetConfigValue("08").Trim() != "")
                    txtLoc.Text += ", " + AppSettingsManager.GetConfigValue("08");*/

                // RMC 20141228 modified permit printing (lubao) (s)

                m_sBIN = txtTmpBin.Text;

                // RMC 20150107 (S)
                txtDTIName.Text = AppSettingsManager.GetDTIName(m_sBIN);
                if (txtDTIName.Text == "")
                    txtDTIName.Text = txtCharOcc.Text;
                // RMC 20150107 (E)

                // RMC 20150117 (s)
                if (txtLastName.Text.Trim() == "")
                {
                    txtLastName.Text = txtDTIName.Text;
                }
                // RMC 20150117 (e)

                if (!TaskMan.IsObjectLock(m_sBIN, "ANNUAL INSPECTION PERMIT", "ADD", "ASS"))
                {

                }
                else
                {
                    m_sBIN = "";
                    txtTmpBin.Text = "";
                }
                // RMC 20141228 modified permit printing (lubao) (e)
            }
            else
            {
                // RMC 20141228 modified permit printing (lubao) (s)
                btnSearchTmp.Text = "Search";
                ClearControls();
                // RMC 20141228 modified permit printing (lubao) (e)
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PermitRecord(); //MCR 20150116
            frmPrinting form = new frmPrinting();
            form.ReportType = "Annual Inspection";
            form.BIN = m_sBIN;
            frmPrinting frmPrinting = new frmPrinting();
            //if (m_sIssuedDate == "")
            //    m_sIssuedDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyy");
            //frmPrinting.IssuedDate = m_sIssuedDate;   // RMC 20141222 modified permit printing
            form.PermitNo = m_sPermitNumber;
            form.ShowDialog();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (m_sBIN == "")
            {
                MessageBox.Show("Select data to delete first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            OracleResultSet pSet = new OracleResultSet();
            int iCnt = 0;
            //check if already printed
            pSet.Query = "select count(*) from a_trail where object like '%" + m_sBIN + "%'";
            pSet.Query += " and mod_code = 'ABAIP-P'";
            int.TryParse(pSet.ExecuteScalar(), out iCnt);

            if (iCnt > 0)
            {
                if (MessageBox.Show("Annual Inspection certificate already been printed for the selected record.\nContinue deleting?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            if (MessageBox.Show("Delete record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet pCmd = new OracleResultSet();
                string sObj = string.Empty;

                pCmd.Query = "delete from annual_insp where bin = '" + m_sBIN + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                UpdateOtherInfo("DELETE");  // RMC 20150104 addl mods

                sObj = "BIN: " + m_sBIN;
                sObj += "/Char Occ: " + txtCharOcc.Text.Trim();
                sObj += "/Grp: " + txtGrp.Text.Trim();
                sObj += "/Location: " + txtLoc.Text.Trim();
                sObj += "/Cert Occ: " + txtCertOcc.Text.Trim();
                sObj += "/Date: " + dtpDate.Value;
                sObj += "/OR No.: " + txtORNo.Text.Trim();
                sObj += "/Stall No.: " + txtStallNo.Text.Trim();

                if (AuditTrail.InsertTrail("ABAIP-D", "annual_insp", StringUtilities.HandleApostrophe(sObj)) == 0)
                {
                    MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Record deleted", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateList();
            }
        }

        private void txtTmpBin_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateOtherInfo(string sTransaction)
        {
            // RMC 20150104 addl mods
            OracleResultSet pCmd = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();
            string sOtherInfoCode = string.Empty;
            string sDataType = string.Empty;
            double dArea = 0;

            double.TryParse(txtStallNo.Text, out dArea);

            string sMainBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);

            pCmd.Query = "select * from default_code where rev_year = '" + ConfigurationAttributes.RevYear + "'";
            pCmd.Query += " and default_desc like '%METERS%'";  //NUMBER OF SQ METERS
            if (pCmd.Execute())
            {
                if (pCmd.Read())
                {
                    sOtherInfoCode = pCmd.GetString("default_code");
                    sDataType = pCmd.GetString("data_type");
                }
            }
            pCmd.Close();

            if (sTransaction == "DELETE")
            {
                pCmd.Query = "delete from other_info where bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text + "' and default_code = '" + sOtherInfoCode + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }
            else
            {
                int iCnt = 0;
                result.Query = "select count(*) from other_info where bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text + "' and default_code = '" + sOtherInfoCode + "'";
                int.TryParse(result.ExecuteScalar(), out iCnt);

                if (sOtherInfoCode != "" && sDataType != "" && dArea != 0)
                {   
                    if (iCnt > 0)
                    {
                        pCmd.Query = "update other_info set data = " + dArea + " where bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text + "' and default_code = '" + sOtherInfoCode + "'";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                    }
                    else
                    {
                        pCmd.Query = "INSERT INTO other_info VALUES (:1,:2,:3,:4,:5,:6,:7)";
                        pCmd.AddParameter(":1", m_sBIN);
                        pCmd.AddParameter(":2", txtTaxYear.Text);
                        pCmd.AddParameter(":3", sMainBnsCode);
                        pCmd.AddParameter(":4", sOtherInfoCode);
                        pCmd.AddParameter(":5", sDataType);
                        pCmd.AddParameter(":6", dArea);
                        pCmd.AddParameter(":7", ConfigurationAttributes.RevYear);
                        pCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void PermitRecord()
        {
            // RMC 20150105 mods in permit printing
            String m_sPermitNumber = "";
            #region GetNextSeries
            OracleResultSet result = new OracleResultSet();
            String sQuery = String.Empty;
            result.Query = "select coalesce(max(to_number(permit_number)),0)+1 as permit_number from permit_type where perm_type = 'ANNUAL' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            if (result.Execute())
            {
                if (result.Read())
                    m_sPermitNumber = AppSettingsManager.GetConfigValue("12") + "-" + result.GetInt(0).ToString("0000");
            }
            result.Close();
            #endregion

            bool Hasrecord = false;
            #region CheckExist
            result.Query = "select * from permit_type where bin = '" + m_sBIN + "' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "' and perm_type = 'ANNUAL'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sPermitNumber = result.GetString(2);
                    m_sIssuedDate = result.GetString(3);
                }
            }
            result.Close();
            #endregion
             int intCount = 0;
            int.TryParse(result.ExecuteScalar(), out intCount);
            if (intCount == 0)
                Hasrecord = false;
            else
                Hasrecord = true;


            result.Close();
          

           


            #region Saving

        //    if (m_sIssuedDate == "")
              if (Hasrecord == false)
            {
                string sCurrentYear = AppSettingsManager.GetConfigValue("12");
                string sPermType = "ANNUAL";
                string sPermitNumber = m_sPermitNumber.Substring(5);
                string sIssuedDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                string sUserCode = AppSettingsManager.SystemUser.UserCode;
                string s_mBin = m_sBIN;

                result.Query = "insert into permit_type (current_year,perm_type,permit_number,issued_date,user_code,bin) values('" + sCurrentYear + "', '" + sPermType + "', '" + sPermitNumber + "', '" + sIssuedDate + "', '" + sUserCode + "', '" + s_mBin + "')";
                result.ExecuteNonQuery();
            }
            #endregion
        }

        private void btnExt_Click(object sender, EventArgs e)
        {
            frmAnnualExt form = new frmAnnualExt();
            form.BIN = m_sBIN;
            form.BnsName = txtCharOcc.Text;
            form.ShowDialog();
        }
    }
}