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

namespace Amellar.Modules.ApplicationRequirements
{
    public partial class frmAppRequirement : Form
    {
        private string m_sModuleCode = string.Empty;

        public frmAppRequirement()
        {
            InitializeComponent();
        }


        private void frmAppRequirement_Load(object sender, EventArgs e)
        {
            // RMC 20171124 Added checklist requirement specific to Malolos (s)
            /*if (AppSettingsManager.GetConfigValue("10") == "216")
            {
                this.chkMalolos.Location = new System.Drawing.Point(20, 88);
                chkMalolos.Visible = true;
            }
            else
                chkMalolos.Visible = false;
            // RMC 20171124 Added checklist requirement specific to Malolos (e)
             */
            // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA, put rem

            EnableControls(false);
            UpdateList();
        }

        private void UpdateList()
        {
            OracleResultSet pRec = new OracleResultSet();
            int iRow = 0;

            dgvList.Columns.Clear();

            dgvList.Columns.Add("CODE", "Code");
            dgvList.Columns.Add("DESC", "Description");
            /*dgvList.Columns.Add("STATUS", "Status");
            dgvList.Columns.Add("BNSCODE", "Nature of Business");
            dgvList.Columns.Add("ORGKIND", "Org. Kind");
            dgvList.Columns.Add("OTHERS", "Others...");
            dgvList.Columns.Add("OTHERVAR", " ");*/
            dgvList.Columns[0].Width = 50;
            dgvList.Columns[1].Width = 500;
            /*dgvList.Columns[2].Width = 70;
            dgvList.Columns[3].Width = 100;
            dgvList.Columns[4].Width = 70;
            dgvList.Columns[5].Width = 70;
            dgvList.Columns[6].Width = 70;*/
            dgvList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            /*dgvList.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;*/
            dgvList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            /*dgvList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvList.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvList.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvList.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvList.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;*/

            string sBnsCode = "";
            pRec.Query = "select distinct req_code, req_desc from REQUIREMENTS_CHKLIST order by req_code";
            if (pRec.Execute())
             {
                while (pRec.Read())
                {
                    dgvList.Rows.Add("");

                    dgvList[0, iRow].Value = pRec.GetString("req_code").Trim();
                    dgvList[1, iRow].Value = StringUtilities.RemoveApostrophe(pRec.GetString("req_desc").Trim());
                    /*dgvList[2, iRow].Value = pRec.GetString("bns_stat").Trim();

                    sBnsCode = pRec.GetString("bns_code").Trim();
                    dgvList[3, iRow].Value = AppSettingsManager.GetBnsDesc(sBnsCode);
                    dgvList[4, iRow].Value = pRec.GetString("bns_org").Trim();
                    dgvList[5, iRow].Value = pRec.GetString("other_desc").Trim();
                    dgvList[6, iRow].Value = pRec.GetString("other_var").Trim();*/

                    if (iRow == 0)
                        PopulateControls(iRow);
                    iRow++;
                }
            }
            pRec.Close();


        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            PopulateControls(e.RowIndex);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();
            
            if (btnSave.Text == "Add")
            {
                EnableControls(true);
                ClearControls();
                btnSave.Text = "Save";
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Text = "Cancel";
                GenerateCode();
                txtDesc.Focus();
                
            }
            else
            {
                if (!Validations())
                    return;

                if (MessageBox.Show("Save?", "Application Requirements", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SaveRecord("Add");
                    
                    MessageBox.Show("Record saved","Application Requirements",MessageBoxButtons.OK, MessageBoxIcon.Information);

                    EnableControls(false);
                    btnSave.Text = "Add";
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnCancel.Text = "Close";
                    UpdateList();
                }
            }
        }

        private void GenerateCode()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sCode = "";

            pRec.Query = "select * from REQUIREMENTS_CHKLIST order by req_code desc";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sCode = pRec.GetString("req_code");
                    sCode = string.Format("{0:00#}", Convert.ToInt32(sCode) + 1);
                }
                else
                    sCode = "001";
            }
            pRec.Close();

            txtCode.Text = sCode;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Cancel")
            {
                btnCancel.Text = "Close";
                btnSave.Text = "Add";
                btnEdit.Text = "Edit";
                txtDesc.ReadOnly = true;
                txtCode.Text = "";
                txtDesc.Text = "";
                btnSave.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                if (TaskMan.IsObjectLock("APP REQMT", "APP REQMT", "DELETE", "ASS"))
                {
                    OracleResultSet pCmd = new OracleResultSet();
                                        
                    pCmd.Query = "delete from REQUIREMENTS_CHKLIST_tmp";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    this.Close();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();
           
            if (btnEdit.Text == "Edit")
            {
                if (txtDesc.Text.Trim() == "")
                {
                    MessageBox.Show("Select requirement description to edit", "Application Requirements", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                EnableControls(true);
                btnEdit.Text = "Update";
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                txtDesc.Focus();
            }
            else
            {
                if (!Validations())
                    return;

                if (MessageBox.Show("Update?", "Application Requirements", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pCmd.Query = string.Format("delete from REQUIREMENTS_CHKLIST where req_code = '{0}'", txtCode.Text.Trim());
                    if(pCmd.ExecuteNonQuery() == 0)
                    {}

                    SaveRecord("Edit");

                    MessageBox.Show("Record updated", "Application Requirements", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    EnableControls(false);
                    btnEdit.Text = "Edit";
                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                    btnCancel.Text = "Close";
                    UpdateList();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pCmd = new OracleResultSet();

            m_sModuleCode = "AUAR-D";

            if (txtDesc.Text.Trim() == "")
            {
                MessageBox.Show("Select requirement description to delete", "Application Requirements", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            pRec.Query = string.Format("select count(*) from requirements_tbl where req_code = '{0}'", txtCode.Text.Trim());
            int iCnt = 0;

            int.TryParse(pRec.ExecuteScalar(), out iCnt);

            if (iCnt > 0)
            {
                MessageBox.Show("Deletion not allowed. Description already been used.", "Application Requirements",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Delete requirement?", "Application Requirements", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pCmd.Query = string.Format("delete from REQUIREMENTS_CHKLIST where req_code = '{0}'", txtCode.Text.Trim());
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                string strObj = "";
                strObj = "Deleted App. Rqmt: " + txtCode.Text.Trim() + "; Desc: " + txtDesc.Text.Trim(); ;

                if (AuditTrail.InsertTrail(m_sModuleCode, "requirements_chklist", StringUtilities.HandleApostrophe(strObj)) == 0)
                {
                    pCmd.Rollback();
                    pCmd.Close();
                    MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UpdateList();
            }
        }

        private void EnableControls(bool blnEnable)
        {
            chkRenewal.Enabled = blnEnable;
            chkNew.Enabled = blnEnable;
            txtDesc.ReadOnly = !blnEnable;
            grpBnsStat.Enabled = blnEnable;
            grpBnsNtr.Enabled = blnEnable;
            grpBnsOrg.Enabled = blnEnable;
            grpOther.Enabled = blnEnable;
            txtOthers.Enabled = blnEnable;  // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA
        }

        private void PopulateControls(int iRow)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pCmd = new OracleResultSet();

            string sNew = "";
            string sRenewal = "";

            try
            {
                ClearControls();

                txtCode.Text = dgvList[0, iRow].Value.ToString();
                txtDesc.Text = dgvList[1, iRow].Value.ToString();
                
                pRec.Query = string.Format("select * from REQUIREMENTS_CHKLIST where req_code = '{0}'", txtCode.Text);
                pRec.Query += " order by bns_stat, bns_code, bns_org, other_desc, other_var";
                if (pRec.Execute())
                {
                    pCmd.Query = string.Format("delete from REQUIREMENTS_CHKLIST_tmp where req_code = '{0}'", txtCode.Text);
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    string sBnsStat = "";
                    string sBnsCode = "";
                    string sOrgKind = "";
                    string sOtherDesc = "";
                    string sOtherVar = "";

                    while (pRec.Read())
                    {
                        sBnsStat = pRec.GetString("bns_stat");

                        if (sBnsStat == "NEW")
                            chkNew.Checked = true;
                        if (sBnsStat == "RENEWAL")
                            chkRenewal.Checked = true;

                        sBnsCode = pRec.GetString("bns_code");
                        if (sBnsCode == "ALL")
                        {
                            chkNtrAll.Checked = true;
                            chkNtrSpecific.Checked = false;
                        }
                        else
                        {
                            chkNtrAll.Checked = false;
                            chkNtrSpecific.Checked = true;

                            int iCnt = 0;
                            pCmd.Query = string.Format("select count(*) from REQUIREMENTS_CHKLIST_tmp where req_code = '{0}' and bns_code = '{1}'", txtCode.Text, sBnsCode);
                            int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                            if (iCnt == 0)
                            {
                                pCmd.Query = "insert into REQUIREMENTS_CHKLIST_tmp values (:1, :2)";
                                pCmd.AddParameter(":1", txtCode.Text);
                                pCmd.AddParameter(":2", sBnsCode);
                                if (pCmd.ExecuteNonQuery() == 0)
                                { }
                            }
                        }

                        sOrgKind = pRec.GetString("bns_org");
                        if (sOrgKind == "SINGLE")
                            chkOrgSingle.Checked = true;
                        if (sOrgKind == "PARTNERSHIP")
                            chkOrgPartner.Checked = true;
                        if (sOrgKind == "CORPORATION")
                            chkOrgCorp.Checked = true;
                        //if (sOrgKind == "COOPERATION")   // RMC 20170120 added cooperative orgn kind in Application reqmts module   
                        if (sOrgKind == "COOPERATIVE")  // RMC 20171124 correction in application requirement module
                            chkOrgCoop.Checked = true;  
                             

                        sOtherDesc = pRec.GetString("other_desc");
                        sOtherVar = string.Format("{0:##,###}", pRec.GetFloat("other_var"));

                        if (sOtherDesc == "GROSS")
                        {
                            chkOtherGross.Checked = true;
                            txtOtherGross.Text = sOtherVar;
                        }
                        else    // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA
                            if (sOtherDesc == "PEZA")
                                chkOtherPeza.Checked = true;
                            else    // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA
                                if (sOtherDesc == "BOI")
                                    chkOtherBoi.Checked = true;
                                else if (sOtherDesc != "")    // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA
                                {
                                    // RMC 20171124 Added checklist requirement specific to Malolos (s)
                                    //if (sOtherDesc == "MAPUMA")   // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA, put rem
                                    chkMalolos.Checked = true;
                                    txtOthers.Text = sOtherDesc;    // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA
                                }
                                else
                                    txtOthers.Text = "";
                        // RMC 20171124 Added checklist requirement specific to Malolos (e)
                    }
                }
                pRec.Close();

            }
            catch
            {
            }
        }

        private void chkNew_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkNtrAll_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkNtrAll.CheckState.ToString() == "Checked")
            {
                chkNtrSpecific.Checked = false;
            }
        }

        private void chkNtrSpecific_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkNtrSpecific.CheckState.ToString() == "Checked")
            {
                chkNtrAll.Checked = false;

                if (btnSave.Text == "Save" || btnEdit.Text == "Update")
                {
                    frmAppBnsNature frmAppBnsNature = new frmAppBnsNature();
                    frmAppBnsNature.ReqCode = txtCode.Text;
                    frmAppBnsNature.ShowDialog();
                }
            }
        }

        private void chkOtherGross_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkOtherGross.CheckState.ToString() == "Checked")
            {
                txtOtherGross.ReadOnly = false;
            }
            else
                txtOtherGross.ReadOnly = true;
        }

        private bool Validations()
        {
            OracleResultSet pRec = new OracleResultSet();

            if (chkNew.Checked == false && chkRenewal.Checked == false)
            {
                MessageBox.Show("Select Application status", "Application Requirements", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (chkNtrAll.Checked == false && chkNtrSpecific.Checked == false)
            {
                MessageBox.Show("Select Nature of Business", "Application Requirements", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (chkNtrSpecific.Checked == true)
            {
                int iCnt = 0;

                pRec.Query = string.Format("select count(*) from requirements_chklist_tmp where req_code = '{0}'",txtCode.Text.Trim());
                int.TryParse(pRec.ExecuteScalar(), out iCnt);

                if (iCnt == 0)
                {
                    MessageBox.Show("Specify Nature of Business", "Application Requirements", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            if (txtDesc.Text.Trim() == "")
            {
                MessageBox.Show("Specify description", "Application Requirements", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            //if (chkOrgSingle.Checked == false && chkOrgPartner.Checked == false && chkOrgCorp.Checked == false)
            if (chkOrgSingle.Checked == false && chkOrgPartner.Checked == false && chkOrgCorp.Checked == false && chkOrgCoop.Checked == false) // RMC 20170120 added cooperative orgn kind in Application reqmts module
            {
                MessageBox.Show("Select Organization kind", "Application Requirements", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            pRec.Query = string.Format("select * from requirements_chklist where req_desc = '{0}' and req_code <> '{1}'", StringUtilities.HandleApostrophe(txtDesc.Text.Trim()), txtCode.Text);
            if(pRec.Execute())
            {
                if(pRec.Read())
                {
                    MessageBox.Show("Description already exists","Application Requirements",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pRec.Close();
                    return false;
                }
            }
            pRec.Close();

            // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA (s)
            if (chkMalolos.Checked.ToString() == "Checked" && txtOthers.Text.Trim() == "")
            {
                MessageBox.Show("Other variable is required", "Application Requirements", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA (e)

            return true;
        }

        private void SaveRecord(string sTransaction)
        {
            OracleResultSet pCmd = new OracleResultSet();

            string sReqCode = "";
            string sReqDesc = "";

            List<string> m_lstBnsStat = new List<string>();
            List<string> m_lstOrgKind = new List<string>();
            List<string> m_lstBnsCode = new List<string>();
            List<string> m_lstOtherDesc = new List<string>();
            string sBnsStat = "";
            string sOrgKind = "";
            string sBnsCode = "";
            string sOtherDesc = "";
            string sOtherVar = "";

            sReqCode = txtCode.Text.Trim();
            sReqDesc = txtDesc.Text.Trim();

            // bns stat
            if (chkNew.Checked == true)
                m_lstBnsStat.Add("NEW");
            if (chkRenewal.Checked == true)
                m_lstBnsStat.Add("RENEWAL");

            // org kind
            if (chkOrgSingle.Checked == true)
                m_lstOrgKind.Add("SINGLE");
            if (chkOrgPartner.Checked == true)
                m_lstOrgKind.Add("PARTNERSHIP");
            if (chkOrgCorp.Checked == true)
                m_lstOrgKind.Add("CORPORATION");
            if (chkOrgCoop.Checked == true)
                m_lstOrgKind.Add("COOPERATIVE");

            // nature of business
            if (chkNtrAll.Checked == true)
                m_lstBnsCode.Add("ALL");
            else
            {
                OracleResultSet pRec = new OracleResultSet();

                pRec.Query = string.Format("select distinct bns_code from requirements_chklist_tmp where req_code = '{0}'", sReqCode);
                if (pRec.Execute())
                {
                    while (pRec.Read())
                    {
                        m_lstBnsCode.Add(pRec.GetString("bns_code"));
                    }
                }
                pRec.Close();
            }

            if (chkOtherGross.Checked == true)
                m_lstOtherDesc.Add("GROSS");
            if (chkOtherPeza.Checked == true)
                m_lstOtherDesc.Add("PEZA");
            if (chkOtherBoi.Checked == true)
                m_lstOtherDesc.Add("BOI");
            // RMC 20171124 Added checklist requirement specific to Malolos (s)
            if (chkMalolos.Checked == true)
                //m_lstOtherDesc.Add("MAPUMA");
                m_lstOtherDesc.Add(txtOthers.Text); // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA
            // RMC 20171124 Added checklist requirement specific to Malolos (e)

            for (int i = 0; i < m_lstBnsStat.Count; i++)
            {
                sBnsStat = m_lstBnsStat[i];

                for (int j = 0; j < m_lstOrgKind.Count; j++)
                {
                    sOrgKind = m_lstOrgKind[j];

                    for (int k = 0; k < m_lstBnsCode.Count; k++)
                    {
                        sBnsCode = m_lstBnsCode[k];

                        if (m_lstOtherDesc.Count == 0)
                        {
                            sOtherDesc = "";
                            sOtherVar = "0";

                            /*pCmd.Query = "insert into REQUIREMENTS_CHKLIST values (:1,:2,:3,:4,:5,:6,:7)";
                            pCmd.AddParameter(":1", txtCode.Text.Trim());
                            pCmd.AddParameter(":2", StringUtilities.HandleApostrophe(txtDesc.Text.Trim()));
                            pCmd.AddParameter(":3", sBnsStat);
                            pCmd.AddParameter(":4", sBnsCode);
                            pCmd.AddParameter(":5", sOrgKind);
                            pCmd.AddParameter(":6", sOtherDesc);
                            pCmd.AddParameter(":7", sOtherVar);*/
                            pCmd.Query = "insert into REQUIREMENTS_CHKLIST values (";
                            pCmd.Query += "'" + txtCode.Text.Trim() + "', ";
                            pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtDesc.Text.Trim()) + "', ";
                            pCmd.Query += "'" + sBnsStat + "', ";
                            pCmd.Query += "'" + sBnsCode + "', ";
                            pCmd.Query += "'" + sOrgKind + "', ";
                            pCmd.Query += "'" + sOtherDesc + "', ";
                            pCmd.Query += "'" + sOtherVar + "')";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }
                        }
                        else
                        {
                            sOtherDesc = "";
                            sOtherVar = "0";

                            for (int l = 0; l < m_lstOtherDesc.Count; l++)
                            {
                                sOtherDesc = m_lstOtherDesc[l];
                                if (sOtherDesc == "GROSS")
                                    sOtherVar = string.Format("{0:####}", Convert.ToDouble(txtOtherGross.Text.ToString()));
                                // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA (s)
                                else if (sOtherDesc == "PEZA" || sOtherDesc == "BOI")
                                { }
                                else
                                    sOtherDesc = txtOthers.Text;
                                // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA (e)

                                /*pCmd.Query = "insert into REQUIREMENTS_CHKLIST values (:1,:2,:3,:4,:5,:6,:7)";
                                pCmd.AddParameter(":1", txtCode.Text.Trim());
                                pCmd.AddParameter(":2", StringUtilities.HandleApostrophe(txtDesc.Text.Trim()));
                                pCmd.AddParameter(":3", sBnsStat);
                                pCmd.AddParameter(":4", sBnsCode);
                                pCmd.AddParameter(":5", sOrgKind);
                                pCmd.AddParameter(":6", sOtherDesc);
                                pCmd.AddParameter(":7", sOtherVar);*/
                                pCmd.Query = "insert into REQUIREMENTS_CHKLIST values (";
                                pCmd.Query += "'" + txtCode.Text.Trim() + "', ";
                                pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtDesc.Text.Trim()) + "', ";
                                pCmd.Query += "'" + sBnsStat + "', ";
                                pCmd.Query += "'" + sBnsCode + "', ";
                                pCmd.Query += "'" + sOrgKind + "', ";
                                pCmd.Query += "'" + sOtherDesc + "', ";
                                pCmd.Query += "'" + sOtherVar + "')";
                                if (pCmd.ExecuteNonQuery() == 0)
                                { }
                            }
                        }

                        

                        
                    }
                }
            }

            string strObj = "";
            if (sTransaction == "Add")
            {
                strObj = "Added App. Rqmt: " + txtCode.Text.Trim() + "; Desc: " + txtDesc.Text.Trim();
                m_sModuleCode = "AUAR-A";
            }
            else
            {
                strObj = "Edited App. Rqmt: " + txtCode.Text.Trim() + "; Desc: " + txtDesc.Text.Trim(); ;
                m_sModuleCode = "AUAR-E";
            }

            if (AuditTrail.InsertTrail(m_sModuleCode, "requirements_chklist", StringUtilities.HandleApostrophe(strObj)) == 0)
            {
                pCmd.Rollback();
                pCmd.Close();
                MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ClearControls()
        {
            txtCode.Text = "";
            txtDesc.Text = "";
            chkNew.Checked = false;
            chkRenewal.Checked = false;
            chkNtrAll.Checked = false;
            chkNtrSpecific.Checked = false;
            chkOrgSingle.Checked = false;
            chkOrgPartner.Checked = false;
            chkOrgCorp.Checked = false;
            chkOtherGross.Checked = false;
            txtOtherGross.Text = "";
            chkOtherPeza.Checked = false;
            chkOtherBoi.Checked = false;
            chkOrgCoop.Checked = false; // RMC 20170120 added cooperative orgn kind in Application reqmts module
            chkMalolos.Checked = false; // RMC 20171124 Added checklist requirement specific to Malolos
        }

        private void chkNtrSpecific_CheckedChanged(object sender, EventArgs e)
        {

        }

        

        private void chkMalolos_CheckStateChanged(object sender, EventArgs e)
        {
            // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA
            if (chkMalolos.CheckState.ToString() == "Checked")
            {
                txtOthers.ReadOnly = false;
            }
            else
                txtOthers.ReadOnly = true;
        }

        
    }
}