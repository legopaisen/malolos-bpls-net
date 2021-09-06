
// RMC 20120104 corrected saving of users

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.LogIn;
using Amellar.Common.AuditTrail;

namespace Amellar.Common.User
{
    public partial class frmUser : Form
    {
        private string m_sSource = string.Empty;
        private string m_sQuery = string.Empty;
        private string m_strTempPassword = string.Empty;
        private bool blnValidateUserCode = false;
        
        private TabControl tabMain = new TabControl();
        private TabPage tabReports = new TabPage("Reports");
        private TabPage tabUtilities = new TabPage("Utilities");
        private TabPage tabBussRec = new TabPage("Business Records");
        private TabPage tabApplication = new TabPage("Application");
        private TabPage tabBilling = new TabPage("Billing");
        private TabPage tabPayment = new TabPage("Payment Records");
        private TabPage tabTransactions = new TabPage("Transactions");
        private DataGridView dgvListBussRec = new DataGridView();
        private DataGridView dgvListApplication = new DataGridView();
        private DataGridView dgvListBilling = new DataGridView();
        private DataGridView dgvListPayment = new DataGridView();
        private DataGridView dgvListTransactions = new DataGridView();
        private DataGridView dgvListReports = new DataGridView();
        private DataGridView dgvListUtilities = new DataGridView();
        

        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }

        public frmUser()
        {
            InitializeComponent();
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            this.EnableControls(false);

            this.ClearControls();
            
            dgvUserList.AllowUserToResizeRows = false;
            
            //draw tabs
            this.Controls.Add(tabMain);
            
            if (m_sSource == "BPS")  //billing
            {
                //add tabs to main tab
                tabMain.Controls.Add(tabBussRec);
                tabMain.Controls.Add(tabApplication);
                tabMain.Controls.Add(tabBilling); // RMC 20140923 merged from Antipolo

                //add controls to tabs
                tabBussRec.Controls.Add(dgvListBussRec);
                tabApplication.Controls.Add(dgvListApplication);
                tabBilling.Controls.Add(dgvListBilling);  // RMC 20140923 merged from Antipolo
                
            }
            else
            {   //collection

                //add tabs to main tab
                tabMain.Controls.Add(tabPayment);
                tabMain.Controls.Add(tabTransactions);
                //tabMain.Controls.Add(tabBilling);   // RMC 20140923 merged from Antipolo

                //add controls to tabs
                tabPayment.Controls.Add(dgvListPayment);
                tabTransactions.Controls.Add(dgvListTransactions);
                //tabBilling.Controls.Add(dgvListBilling);    // RMC 20140923 merged from Antipolo

            }
            
            tabMain.Controls.Add(tabReports);
            tabMain.Controls.Add(tabUtilities);
            tabReports.Controls.Add(dgvListReports);
            tabUtilities.Controls.Add(dgvListUtilities);

            // enable tabs using SelectedTab from last tab to 1st tab
            this.tabMain.SelectedTab = this.tabUtilities;
            this.tabMain.SelectedTab = this.tabReports;

            if (m_sSource == "BPS")
            {
                this.tabMain.SelectedTab = this.tabBilling;   // RMC 20140923 merged from Antipolo
                this.tabMain.SelectedTab = this.tabApplication;
                this.tabMain.SelectedTab = this.tabBussRec;
            }
            else
            {
                //this.tabMain.SelectedTab = this.tabBilling; // RMC 20140923 merged from Antipolo
                this.tabMain.SelectedTab = this.tabPayment;
                this.tabMain.SelectedTab = this.tabTransactions;
            }

            tabMain.Location = new Point(315, 290);
            tabMain.Size = new Size(330, 216);

            this.UpdatePermissionList();
            this.UpdateUserList();
            this.dgvUserList.CurrentCell = this.dgvUserList[0, 0];
            
        }
                
        private void UpdateUserList()
        {
            //m_sQuery = "select distinct(usr_code) \" User Code\", usr_ln \" Last Name\", usr_fn \" First Name\", usr_mi \" MI\", usr_pos \" Position\", usr_div \"Division\", usr_memo \"Memo\", usr_pwd "
            //       + " from sys_users order by usr_ln,usr_fn,usr_mi,usr_code";

            //AFM 20200129 MAO-20-11973 new query to not include old users with whitespace to prevent duplication upon updating user
            m_sQuery = "select usr_code \" User Code\", usr_ln \" Last Name\", usr_fn \" First Name\", usr_mi \" MI\", usr_pos \" Position\", usr_div \"Division\", usr_memo \"Memo\", usr_pwd "
                   + " FROM sys_users a WHERE a.usr_code "
                   + " NOT LIKE trim(a.usr_code) || '% %' "
                   + " OR trim(a.usr_code) in (select distinct(trim(b.usr_code)) "
                   + " from sys_users b group by trim(b.usr_code) "
                   + " HAVING COUNT (*) = 1) order by usr_ln,usr_fn,usr_mi,usr_code";

            DataGridViewOracleResultSet dsUser = new DataGridViewOracleResultSet(dgvUserList, m_sQuery, 0, 0);
            dgvUserList.RowHeadersVisible = false;
            dgvUserList.Columns[7].Visible = false;
            dgvUserList.Refresh();
            
        }

        private void UpdatePermissionList()
        {
            if (m_sSource == "BPS")
            {
                m_sQuery = "select right_desc \"Module Permissions\" from sys_module where rtrim(usr_rights) like 'AB%%'";
                // RMC 20150807 hide all modules not applicable to enterprise edition of BPLS in Users module (s)
                if (AppSettings.AppSettingsManager.GetConfigValue("66") == "ENTERPRISE")
                    m_sQuery += " and (usr_rights <> 'ABAIP' and usr_rights <> 'ABHC' and usr_rights <> 'ABM' and usr_rights <> 'ABM-U-ND' and usr_rights <> 'ABSP' and usr_rights <> 'ABZC')";
                // RMC 20150807 hide all modules not applicable to enterprise edition of BPLS in Users module (e)
                this.LoadPermissionList(m_sQuery, dgvListBussRec);

                m_sQuery = "select right_desc \"Module Permissions\" from sys_module where rtrim(usr_rights) like 'AA%%'";
                this.LoadPermissionList(m_sQuery, dgvListApplication);

                m_sQuery = "select right_desc \"Module Permissions\" from sys_module where rtrim(usr_rights) like 'AI%%'";
                this.LoadPermissionList(m_sQuery, dgvListBilling);
                // RMC 20140923 merged from Antipolo   

                m_sQuery = "select right_desc \"Module Permissions\" from sys_module where rtrim(usr_rights) like 'AR%%'";
                this.LoadPermissionList(m_sQuery, dgvListReports);

                m_sQuery = "select right_desc \"Module Permissions\" from sys_module where rtrim(usr_rights) like 'AU%%'";
                m_sQuery += " and rtrim(usr_rights) <> 'AUTM'"; // RMC 20140923 merged from Antipolo
                this.LoadPermissionList(m_sQuery, dgvListUtilities);
            }
            else
            {
                m_sQuery = "select right_desc \"Module Permissions\" from sys_module where rtrim(usr_rights) like 'CR%%'";
                this.LoadPermissionList(m_sQuery, dgvListPayment);

                m_sQuery = "select right_desc \"Module Permissions\" from sys_module where rtrim(usr_rights) like 'CT%%'";
                this.LoadPermissionList(m_sQuery, dgvListTransactions);

                /*m_sQuery = "select right_desc \"Module Permissions\" from sys_module where rtrim(usr_rights) like 'AI%%'";
                this.LoadPermissionList(m_sQuery, dgvListBilling);*/    // RMC 20140923 merged from Antipolo

                m_sQuery = "select right_desc \"Module Permissions\" from sys_module where rtrim(usr_rights) like 'CC%%'";
                this.LoadPermissionList(m_sQuery, dgvListReports);

                m_sQuery = "select right_desc \"Module Permissions\" from sys_module where rtrim(usr_rights) like 'CU%%'";
                m_sQuery += " or rtrim(usr_rights) = 'AUTM'"; // RMC 20140923 merged from Antipolo
                this.LoadPermissionList(m_sQuery, dgvListUtilities);
            }
        }

        private void LoadPermissionList(string strQuery, DataGridView dgvList)
        {
            dgvList.Columns.Add(new DataGridViewCheckBoxColumn());

            DataGridViewOracleResultSet dsList = new DataGridViewOracleResultSet(dgvList, strQuery, 0, 0);
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 30;
            dgvList.Columns[1].Width = 270;
            dgvList.Columns[1].ReadOnly = true;
            dgvList.Refresh();

            dgvList.Size = new Size(320, 190);
            dgvList.AllowUserToResizeRows = false;
        }

        private void LoadUserRights()
        {
            if (m_sSource == "BPS")
            {
                m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and rtrim(usr_rights) like 'AB%%'", txtUserCode.Text.Trim());
                // RMC 20150807 hide all modules not applicable to enterprise edition of BPLS in Users module (s)
                if (AppSettings.AppSettingsManager.GetConfigValue("66") == "ENTERPRISE")
                    m_sQuery += " and (usr_rights <> 'ABAIP' and usr_rights <> 'ABHC' and usr_rights <> 'ABM' and usr_rights <> 'ABM-U-ND' and usr_rights <> 'ABSP' and usr_rights <> 'ABZC')";
                // RMC 20150807 hide all modules not applicable to enterprise edition of BPLS in Users module (e)
                this.LoadRights(m_sQuery, dgvListBussRec);

                m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and rtrim(usr_rights) like 'AA%%'", txtUserCode.Text.Trim());
                this.LoadRights(m_sQuery, dgvListApplication);

                m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and rtrim(usr_rights) like 'AI%%'", txtUserCode.Text.Trim());
                this.LoadRights(m_sQuery, dgvListBilling);
                // RMC 20140923 merged from Antipolo

                m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and rtrim(usr_rights) like 'AR%%'", txtUserCode.Text.Trim());
                this.LoadRights(m_sQuery, dgvListReports);

                m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and rtrim(usr_rights) like 'AU%%'", txtUserCode.Text.Trim());
                m_sQuery += " and rtrim(usr_rights) <> 'AUTM'"; // RMC 20140923 merged from Antipolo
                this.LoadRights(m_sQuery, dgvListUtilities);
            }
            else
            {
                m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and rtrim(usr_rights) like 'CR%%'", txtUserCode.Text.Trim());
                this.LoadRights(m_sQuery, dgvListPayment);

                m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and rtrim(usr_rights) like 'CT%%'", txtUserCode.Text.Trim());
                this.LoadRights(m_sQuery, dgvListTransactions); 

                /*m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and rtrim(usr_rights) like 'AI%%'", txtUserCode.Text.Trim());
                this.LoadRights(m_sQuery, dgvListBilling);*/  // RMC 20140923 merged from Antipolo

                m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and rtrim(usr_rights) like 'CC%%'", txtUserCode.Text.Trim());
                this.LoadRights(m_sQuery, dgvListReports);

                /*m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and rtrim(usr_rights) like 'CU%%'", txtUserCode.Text.Trim());
                m_sQuery += " or rtrim(usr_rights) = 'AUTM'"; */  // RMC 20140923 merged from Antipolo

                // RMC 20170117 correction in granting rights wherein treasurer's module was grandted to all users (s)
                m_sQuery = string.Format("select usr_rights from sys_rights where usr_code = '{0}' and (rtrim(usr_rights) like 'CU%%'", txtUserCode.Text.Trim());
                m_sQuery += " or rtrim(usr_rights) = 'AUTM')";
                // RMC 20170117 correction in granting rights wherein treasurer's module was grandted to all users (e)
                this.LoadRights(m_sQuery, dgvListUtilities);
            }
        }

        private void LoadRights(string strQuery, DataGridView dgvRights)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();

            for (int j = 0; j <= dgvRights.Rows.Count - 1; j++)
            {
                dgvRights[0, j].Value = false;
            }
            
            string strRights = string.Empty;
            
            pSet.Query = strQuery.ToString();
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    strRights = pSet.GetString("usr_rights");

                    string strDesc = string.Empty;

                    pRec.Query = string.Format("select right_desc from sys_module where usr_rights = '{0}'", strRights);
                    if(pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            strDesc = pRec.GetString("right_desc").Trim();

                            for (int j = 0; j <= dgvRights.Rows.Count - 1; j++)
                            {
                                string strTemp = dgvRights[1,j].Value.ToString().Trim();
                                if (strDesc == strTemp)
                                    dgvRights[0, j].Value = true;
                            }
                            
                        }
                    }
                    pRec.Close();
                }
            }
            pSet.Close();

            dgvRights.RefreshEdit();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (btnNew.Text == "New")
            {
                this.ClearControls();
                this.EnableControls(true);
                this.btnEdit.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnClose.Enabled = false;
                this.btnNew.Text = "Save";
                this.btnRevoke_Click(sender, e);
            }
            else
            {
                blnValidateUserCode = true;

                if (!this.Validation())
                    return;

                EncryptPass encrypt = new EncryptPass();
                string strPassword = encrypt.EncryptPassword(txtPassword.Text.Trim());

                if (MessageBox.Show("Save record?", m_sSource, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                this.SaveUser(strPassword);

                if (AuditTrail.AuditTrail.InsertTrail("CUAU", "sys_rights", StringUtilities.StringUtilities.HandleApostrophe(txtUserCode.Text.Trim())) == 0)
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Record saved.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.UpdateUserList();
                
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnClose.Enabled = true;
                this.EnableControls(false);
                this.btnNew.Text = "New";
               
            }
        }

        private void ClearControls()
        {
            this.txtUserCode.Text = "";
            this.txtLastName.Text = "";
            this.txtFirstName.Text = "";
            this.txtMI.Text = "";
            this.txtPosition.Text = "";
            this.txtPassword.Text = "";
            this.txtConfirmPw.Text = "";

            cmbDivision.Items.Clear();
            cmbDivision.Items.Add("");
            /*cmbDivision.Items.Add("BPDO");
            cmbDivision.Items.Add("BPDO TACO");
            cmbDivision.Items.Add("CASH");
            cmbDivision.Items.Add("CASH TACO");
            cmbDivision.Items.Add("LICENSE");
            cmbDivision.Items.Add("LICENSE TACO");
            cmbDivision.Items.Add("MPS");
            cmbDivision.Items.Add("MPS TACO");
            cmbDivision.Items.Add("TAC");
            cmbDivision.Items.Add("OTHERS");*/
            // RMC 20150226 modified user group in user module (s)
            cmbDivision.Items.Add("SDD");
            cmbDivision.Items.Add("PM");
            cmbDivision.Items.Add("DTSD");
            cmbDivision.Items.Add("DATA ENCODERS");
            cmbDivision.Items.Add("DATA CONTROLLERS");
            cmbDivision.Items.Add("LICENSE");
            cmbDivision.Items.Add("TREASURY");
            // RMC 20150226 modified user group in user module (e)
            cmbDivision.Items.Add("ENGINEERING");   // RMC 20170322 added module Eng'g Tool for business, this is LGU-Specific and not configurable (Binan)
            cmbDivision.Items.Add("BPLO"); // MCR 20191126
            cmbDivision.Items.Add("ZONING"); // MCR 20191126
            cmbDivision.Items.Add("SANITARY"); // MCR 20191126
            cmbDivision.Items.Add("BFP"); // MCR 20191126
            cmbDivision.Items.Add("BENRO"); // MCR 20191126
            cmbDivision.Items.Add("CENRO"); // MCR 20191126
            cmbDivision.Items.Add("CHO"); // MCR 20191126
            cmbDivision.Items.Add("PESO"); // AFM 20191212 MAO-19-11583
            cmbDivision.Items.Add("MAPUMA"); // AFM 20191212 MAO-19-11716
           
            cmbDivision.SelectedIndex = -1;
        }

        private void EnableControls(bool blnEnable)
        {
            this.txtUserCode.Enabled = blnEnable;
            this.txtLastName.Enabled = blnEnable;
            this.txtFirstName.Enabled = blnEnable;
            this.txtMI.Enabled = blnEnable;
            this.txtPosition.Enabled = blnEnable;
            this.txtPassword.Enabled = blnEnable;
            this.txtConfirmPw.Enabled = blnEnable;
            this.cmbDivision.Enabled = blnEnable;
            this.btnGrantAll.Enabled = blnEnable;
            this.btnRevoke.Enabled = blnEnable;

            if (m_sSource == "BPS")
            {
                this.dgvListBussRec.Enabled = blnEnable;
                this.dgvListApplication.Enabled = blnEnable;
                this.dgvListBilling.Enabled = blnEnable;  // RMC 20140923 merged from Antipolo
            }
            else
            {
                this.dgvListTransactions.Enabled = blnEnable;
                this.dgvListPayment.Enabled = blnEnable;
                //this.dgvListBilling.Enabled = blnEnable;    // RMC 20140923 merged from Antipolo
            }

            this.dgvListReports.Enabled = blnEnable;
            this.dgvListUtilities.Enabled = blnEnable;
        }

        private bool Validation()
        {
            OracleResultSet pSet = new OracleResultSet();

            if (txtUserCode.Text.Trim() == "")
            {
                MessageBox.Show("User Code field is required, please check.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (txtLastName.Text.Trim() == "")
            {
                MessageBox.Show("Last Name field is required, please check.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (txtPassword.Text.Trim() != txtConfirmPw.Text.Trim())
            {
                MessageBox.Show("Password entries do not match, please check.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (cmbDivision.Text.Trim() == "")
            {
                MessageBox.Show("Division field is required, please check.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (blnValidateUserCode)
            {
                if (txtPassword.Text.Trim() == "")
                {
                    MessageBox.Show("Password field is required, please check.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

                pSet.Query = string.Format("select usr_code from sys_users where usr_code = '{0}'", txtUserCode.Text.Trim());
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("User Code " + txtUserCode.Text.Trim() + " already exists.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }
                pSet.Close();
            }

            return true;
        }

        private void SaveUserRights(DataGridView dgvRights)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();

            string strDesc = string.Empty;

            for (int j = 0; j <= dgvRights.Rows.Count - 1; j++)
            {
                if ((bool)dgvRights[0, j].Value)
                {
                    strDesc = dgvRights[1,j].Value.ToString().Trim();

                    pSet.Query = string.Format("select usr_rights from sys_module where right_desc = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(strDesc));
                    if(pSet.Execute())
                    {
                        string strRights = string.Empty;

                        if(pSet.Read())
                        {
                            strRights = pSet.GetString("usr_rights").Trim();
                            
                            result.Query = "insert into sys_rights (usr_code, usr_rights) values (:1, :2)";
                            result.AddParameter(":1", txtUserCode.Text.Trim().ToUpper());
                            result.AddParameter(":2", strRights);

                            if (result.ExecuteNonQuery() == 0)
                            {
                                result.Rollback();
                                result.Close();
                                MessageBox.Show("Failed to save rights.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                        }
                        
                    }
                    pSet.Close();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (btnEdit.Text == "Edit")
            {
                this.EnableControls(true);
                this.btnNew.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnClose.Enabled = false;
                this.btnEdit.Text = "Update";
                this.txtUserCode.Enabled = false;
                                
            }
            else
            {
                blnValidateUserCode = false;

                if (!this.Validation())
                    return;

                string strPassword = string.Empty;

                if (txtPassword.Text.Trim() != "")
                {
                    if (txtPassword.Text.Trim() != m_strTempPassword)
                    {
                        EncryptPass encrypt = new EncryptPass();
                        strPassword = encrypt.EncryptPassword(txtPassword.Text.Trim());
                    }
                }
                else
                    strPassword = m_strTempPassword;

                // RMC 20120104 corrected saving of users (s)
                if (MessageBox.Show("Update record?", m_sSource, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                // RMC 20120104 corrected saving of users (e)

                result.Query = string.Format("delete from sys_users where usr_code = '{0}'",txtUserCode.Text.Trim());
                if (result.ExecuteNonQuery() == 0)
                {}

                result.Query = string.Format("delete from sys_rights where usr_code = '{0}'", txtUserCode.Text.Trim());
                // RMC 20110801 (s)
                /*if (m_sSource == "BPS")
                {
                    result.Query += string.Format(" and usr_rights like 'A%'");
                    result.Query += string.Format(" and usr_rights not like 'AI%%'");   // RMC 20141217 adjustments
                }
                else
                {
                    result.Query += string.Format(" and usr_rights like 'C%'");
                    result.Query += string.Format(" and usr_rights like 'AI%%'");// RMC 20141217 adjustments
                }
                // RMC 20110801 (e)*/
                // RMC 20150112 mods in retirement billing (s)
                if (m_sSource == "BPS")
                {
                    //result.Query += " and (usr_rights like 'A%' and usr_rights not like 'AI%%')";  
                    //result.Query += " and (usr_rights like 'A%')";  
                    result.Query += " and (usr_rights like 'A%' AND usr_rights <> 'AUTM')"; // RMC 20170117 correction in granting rights wherein treasurer's module was grandted to all users     
                }
                else
                {
                    //result.Query += " and (usr_rights like 'C%' or usr_rights like 'AI%%')";
                    //result.Query += " and (usr_rights like 'C%')";
                    result.Query += " and (usr_rights like 'C%' or usr_rights = 'AUTM')";   // RMC 20170117 correction in granting rights wherein treasurer's module was grandted to all users
                }
                // RMC 20150112 mods in retirement billing (e)
                if (result.ExecuteNonQuery() == 0)
                { }

                /*if (MessageBox.Show("Update record?", m_sSource, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;*/   // RMC 20120104 corrected saving of users

                this.SaveUser(strPassword);

                MessageBox.Show("Record updated.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (AuditTrail.AuditTrail.InsertTrail("CUAU", "sys_rights", StringUtilities.StringUtilities.HandleApostrophe(txtUserCode.Text.Trim())) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.UpdateUserList();

                this.EnableControls(false);
                this.btnNew.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnClose.Enabled = true;
                this.btnEdit.Text = "Edit";
                             
            }
        }

        private void SaveUser(string strPassword)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "insert into sys_users (usr_code, usr_pwd, usr_ln, usr_fn, usr_mi, usr_pos, usr_div) values (:1, :2, :3, :4, :5, :6, :7)";
            pSet.AddParameter(":1", txtUserCode.Text.Trim().ToUpper());
            pSet.AddParameter(":2", strPassword);
            pSet.AddParameter(":3", txtLastName.Text.Trim().ToUpper());
            pSet.AddParameter(":4", txtFirstName.Text.Trim().ToUpper());
            pSet.AddParameter(":5", txtMI.Text.Trim().ToUpper());
            pSet.AddParameter(":6", txtPosition.Text.Trim().ToUpper());
            pSet.AddParameter(":7", cmbDivision.Text.Trim().ToUpper());
            if (pSet.ExecuteNonQuery() == 0)
            {
                pSet.Rollback();
                pSet.Close();
                MessageBox.Show("Failed to save user.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (m_sSource == "BPS")
            {
                this.SaveUserRights(dgvListBussRec);    // for Business Records rights
                this.SaveUserRights(dgvListApplication);    // for Application rights
                this.SaveUserRights(dgvListBilling);    // for Billing rights // RMC 20140923 merged from Antipolo
                this.SaveUserRights(dgvListReports);    // for Reports rights
                this.SaveUserRights(dgvListUtilities);  // for Utilities rights
            }
            else
            {
                this.SaveUserRights(dgvListPayment);
                this.SaveUserRights(dgvListTransactions);
                //this.SaveUserRights(dgvListBilling);    // for Billing rights   // RMC 20140923 merged from Antipolo 
                this.SaveUserRights(dgvListReports);    // for Reports rights
                this.SaveUserRights(dgvListUtilities);  // for Utilities rights
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            string strUserCode = string.Empty;

            strUserCode = txtUserCode.Text.Trim();

            if (strUserCode == "")
            {
                MessageBox.Show("Select record to delete.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("All selected items will be permanently DELETED.  Please confirm.", m_sSource, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //prevent deletion of user with transaction
                result.Query = string.Format("Select * from a_trail where usr_code = '{0}' AND mod_code <> 'AULI'", strUserCode);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("User has transaction/s. Deletion not allowed.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                result.Close();

                result.Query = string.Format("delete from sys_users where usr_code = '{0}'",strUserCode);
                if(result.ExecuteNonQuery() == 0)
                {}

                result.Query = string.Format("delete from sys_rights where usr_code = '{0}'",strUserCode);
                // RMC 20110801 (s)
                /*if (m_sSource == "BPS")
                {
                    result.Query += string.Format(" and usr_rights like 'A%'");
                    result.Query += string.Format(" and usr_rights not like 'AI%%'");   // RMC 20141217 adjustments
                }
                else
                {
                    result.Query += string.Format(" and usr_rights like 'C%'");
                    result.Query += string.Format(" and usr_rights like 'AI%%'");// RMC 20141217 adjustments
                }*/
                // RMC 20150112 mods in retirement billing (s)
                if (m_sSource == "BPS")
                {
                   //result.Query += " and (usr_rights like 'A%' and usr_rights not like 'AI%%')";
                    result.Query += " and (usr_rights like 'A%')";
                }
                else
                {
                    //result.Query += " and (usr_rights like 'C%' or usr_rights like 'AI%%')";
                    result.Query += " and (usr_rights like 'C%')";
                }
                // RMC 20150112 mods in retirement billing (e)
                // RMC 20110801 (e)
                if(result.ExecuteNonQuery() == 0)
                {}

                if (AuditTrail.AuditTrail.InsertTrail("CUDU", "sys_rights", StringUtilities.StringUtilities.HandleApostrophe(txtUserCode.Text.Trim())) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Record deleted.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.UpdateUserList();
                this.ClearControls();
                this.EnableControls(false);


                
            }		
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            this.btnNew.Text = "New";
            this.btnEdit.Text = "Edit";
            this.ClearControls();
            this.EnableControls(false);
            this.btnNew.Enabled = true;
            this.btnEdit.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnClose.Enabled = true;
            this.dgvUserList.Enabled = true;
            this.btnRevoke_Click(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUserList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.SelectUser(e.RowIndex);
        }

        private void dgvUserList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.SelectUser(e.RowIndex);
        }

        private void SelectUser(int intRow)
        {
            this.btnNew.Text = "New";
            this.btnEdit.Text = "Edit";
            this.ClearControls();
            this.EnableControls(false);

            if (intRow >= 0)
            {
                this.txtUserCode.Text = dgvUserList[0, intRow].Value.ToString().Trim();
                this.txtLastName.Text = dgvUserList[1, intRow].Value.ToString().Trim();
                this.txtFirstName.Text = dgvUserList[2, intRow].Value.ToString().Trim();
                this.txtMI.Text = dgvUserList[3, intRow].Value.ToString().Trim();
                this.txtPosition.Text = dgvUserList[4, intRow].Value.ToString().Trim();
                this.cmbDivision.Text = dgvUserList[5, intRow].Value.ToString().Trim();
                this.m_strTempPassword = dgvUserList[7, intRow].Value.ToString().Trim();

                this.LoadUserRights();
            }
        }

        private void dgvUserList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.SelectUser(e.RowIndex);
            
        }

        private void btnGrantAll_Click(object sender, EventArgs e)
        {
            if (m_sSource == "BPS")
            {
                this.SelectState(true, dgvListBussRec);    // for Business Records rights
                this.SelectState(true, dgvListApplication);    // for Application rights
                this.SelectState(true, dgvListBilling);    // for Billing rights  // RMC 20140923 merged from Antipolo
                this.SelectState(true, dgvListReports);    // for Reports rights
                this.SelectState(true, dgvListUtilities);  // for Utilities rights
            }
            else
            {
                this.SelectState(true, dgvListPayment);
                this.SelectState(true, dgvListTransactions);
                //this.SelectState(true, dgvListBilling);    // for Billing rights    // RMC 20140923 merged from Antipolo
                this.SelectState(true, dgvListReports);    // for Reports rights
                this.SelectState(true, dgvListUtilities);  // for Utilities rights
            }
            
        }

        private void SelectState(bool blnSelectAll, DataGridView dgvRights)
        {
            //dgvRights.CancelEdit();

            for (int j = 0; j <= dgvRights.Rows.Count - 1; j++)
            {
                if(blnSelectAll)
                    dgvRights[0, j].Value = true;
                else
                    dgvRights[0, j].Value = false;
            }

            dgvRights.RefreshEdit();
        }

        private void btnRevoke_Click(object sender, EventArgs e)
        {
            if (m_sSource == "BPS")
            {
                this.SelectState(false, dgvListBussRec);    // for Business Records rights
                this.SelectState(false, dgvListApplication);    // for Application rights
                this.SelectState(false, dgvListBilling);    // for Billing rights // RMC 20140923 merged from Antipolo
                this.SelectState(false, dgvListReports);    // for Reports rights
                this.SelectState(false, dgvListUtilities);  // for Utilities rights
            }
            else
            {
                this.SelectState(false, dgvListPayment);
                this.SelectState(false, dgvListTransactions);
                //this.SelectState(false, dgvListBilling);    // for Billing rights   // RMC 20140923 merged from Antipolo
                this.SelectState(false, dgvListReports);    // for Reports rights
                this.SelectState(false, dgvListUtilities);  // for Utilities rights
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.TextLength < 6)
            {
                MessageBox.Show("Password minimun of 6 characters.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            using (frmPrintUser frmPrintUser = new frmPrintUser())
            {
                frmPrintUser.ShowDialog();
            }
        }

        private void dgvUserList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}