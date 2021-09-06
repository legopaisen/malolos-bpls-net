// JJR 20150811 CREATED MODULE FOR APPROVER TABLE MODIFIED

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.Utilities
{
    public partial class frmApprover : Form
    {
        private string m_sSource = string.Empty;
        private string m_sQuery = string.Empty;
        private string m_sQuery2 = string.Empty;
        public string s_UserCode = string.Empty;
        public string s_UserCode2 = string.Empty;
        public string s_GroupType = string.Empty;

        public frmApprover()
        {
            InitializeComponent();
        }
        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }

        private void dgvUserList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            s_UserCode = dgvUserList[0, e.RowIndex].Value.ToString().Trim();
        }

        private void dgvUserList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            s_UserCode = dgvUserList[0, e.RowIndex].Value.ToString().Trim();
        }

        private void dgvUserList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            s_UserCode = dgvUserList[0, e.RowIndex].Value.ToString().Trim();
        }

        private void frmApprover_Load(object sender, EventArgs e)
        {
            UpdateUserList();
            UserGroups();
        }

        private void UpdateUserList()
        {
            m_sQuery = "select distinct(usr_code) \" User Code\", usr_ln \" Last Name\", usr_fn \" First Name\", usr_mi \" MI\", usr_pos \" Position\", usr_div \"Division\", usr_memo \"Memo\", usr_pwd "
                   + " from sys_users order by usr_ln,usr_fn,usr_mi,usr_code";

            DataGridViewOracleResultSet dsUser = new DataGridViewOracleResultSet(dgvUserList, m_sQuery, 0, 0);
            dgvUserList.RowHeadersVisible = false;
            dgvUserList.Columns[7].Visible = false;
            dgvUserList.Refresh();

        }


        private void UserGroups()
        {
            m_sQuery2 = "select approver_tbl.usr_group \"User Group\",approver_tbl.usr_code \" User Code\",sys_users.usr_ln \" Last Name\",sys_users.usr_fn \" First Name\",sys_users.usr_mi \" MI \""
            + "FROM sys_users ,approver_tbl WHERE trim(sys_users.usr_code)=approver_tbl.usr_code ORDER BY sys_users.usr_ln";    // RMC 20171129 added trim in usr_code (malolos still in CHAR)
            DataGridViewOracleResultSet dsUser2 = new DataGridViewOracleResultSet(dgvUserGroup, m_sQuery2, 0, 0);
            dgvUserGroup.RowHeadersVisible = false;
            dgvUserGroup.Columns[0].Width = 100;
            dgvUserGroup.Columns[2].Width = 270;
            dgvUserGroup.Columns[3].Width = 270;
            dgvUserGroup.Columns[4].Width = 50;
            dgvUserGroup.Columns[1].Visible = false;
            dgvUserGroup.Refresh();
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        private int match()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select *from approver_tbl WHERE usr_code='"+ s_UserCode + "'";
            pSet.Execute();
            if (pSet.Read())
            {
                return 1;
            }
            else
            {
                return 0;

            }

        }
        private void AddUserGroup()
        {
            if (m_sQuery ==string.Empty)
            {
                //MessageBox.Show("Select user to be add on user group.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("Select user to be added.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);  // RMC 20150811 QA Approving Officer module
               
            }
            else if (cmbgroup.Text == "")
            {
                //MessageBox.Show("Select user group type on combobox.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("Group type is required.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);   // RMC 20150811 QA Approving Officer module
                cmbgroup.Focus();
            }
            else if (match() > 0)
            {
                //MessageBox.Show("User are already in the group.");
                MessageBox.Show("Selected user already in the list of Approving Officers.");  // RMC 20150811 QA Approving Officer module
            }
            else
            {
                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = "insert into approver_tbl(usr_code,usr_group) values (:1, :2)";
                pSet.AddParameter(":1", s_UserCode);
                pSet.AddParameter(":2", cmbgroup.Text.ToUpper());
                pSet.ExecuteNonQuery();
                //MessageBox.Show("User Group successfully Added");

                // RMC 20150811 QA Approving Officer module (s)
                if (AuditTrail.InsertTrail("CUAO-A", "approver_tbl", "User Code: " + s_UserCode) == 0)
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                                
                MessageBox.Show("User successfully added");
                // RMC 20150811 QA Approving Officer module (e)

                UserGroups();
                btnAdd.Text = "&Add";
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnClose.Text = "&Close";
            }
        }
        
        private void dgvUserList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            s_UserCode = dgvUserList[0, e.RowIndex].Value.ToString().Trim();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text.Trim() == "&Close")
            {
                this.Close();
            }
            else if(btnEdit.Text.Trim()=="&Edit" && btnAdd.Text.Trim()=="&Add"){
                this.Close();
            }
            else
            {
                if (btnAdd.Text.Trim() == "&Save")
                {
                    btnClose.Text = "&Close";
                    btnAdd.Text = "&Add";
                    btnDelete.Enabled = true;
                    btnEdit.Enabled = true;
                }
                else if (btnEdit.Text.Trim() == "&Update")
                {
                    btnClose.Text = "&Close";
                    btnEdit.Text = "&Edit";
                    btnDelete.Enabled = true;
                    btnAdd.Enabled = true;
                }
            }
            
        }

        private void dgvUserList_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            s_UserCode = dgvUserList[0, e.RowIndex].Value.ToString().Trim();
        }

        private void dgvUserList_CellMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            s_UserCode = dgvUserList[0, e.RowIndex].Value.ToString().Trim();
        }

        private void dgvUserList_CellMouseDoubleClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            s_UserCode = dgvUserList[0, e.RowIndex].Value.ToString().Trim();
        }

        private void cmbgroup_DropDown(object sender, EventArgs e)
        {
            cmbgroup.Items.Clear();
            cmbgroup.Items.Add("LICENSE");
            cmbgroup.Items.Add("TREASURER");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text.Trim() == "&Add")
            {
                btnAdd.Text = "&Save";
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnClose.Text = "&Cancel";
            }
            else
            {
                AddUserGroup();
            }
        }

        private void EditUserGroup()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "UPDATE approver_tbl SET usr_group='" + cmbgroup.Text.ToUpper() + "' WHERE usr_code='" + s_UserCode2 + "'";
            result.Execute();

            // RMC 20150811 QA Approving Officer module (s)
            if (AuditTrail.InsertTrail("CUAO-E", "approver_tbl", "User Code: " + s_UserCode) == 0)
            {
                result.Rollback();
                result.Close();
                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // RMC 20150811 QA Approving Officer module (e)

            MessageBox.Show("Record successfully updated.");
            cmbgroup.Text = "";
            btnEdit.Text = "&Edit";
            btnClose.Text = "&Close";
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            UserGroups();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            if (s_UserCode2 == string.Empty)
            {
                MessageBox.Show("Select user from the user group to delete.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                if (MessageBox.Show("Do you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // RMC 20150811 QA Approving Officer module (s)
                    // validate first before deleting
                    result.Query = "select * from a_trail where mod_code = 'AUTM-A' and usr_code = '" + s_UserCode2 + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            MessageBox.Show("Cannot delete user, transaction found", "Approving Officer", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    result.Close();
                    // RMC 20150811 QA Approving Officer module (e)

                    result.Query = "delete from approver_tbl where usr_code='" + s_UserCode2 + "'";
                    result.ExecuteNonQuery();

                    // RMC 20150811 QA Approving Officer module (s)
                    if (AuditTrail.InsertTrail("CUAO-D", "approver_tbl", "User Code: " + s_UserCode) == 0)
                    {
                        result.Rollback();
                        result.Close();
                        MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // RMC 20150811 QA Approving Officer module (e)

                    MessageBox.Show("Record succesfully deleted");
                    UserGroups();
                }
                else
                {

                }
            }
        }

        private void dgvUserGroup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            s_UserCode2 = dgvUserGroup[1, e.RowIndex].Value.ToString().Trim();
            s_GroupType = dgvUserGroup[0, e.RowIndex].Value.ToString().Trim();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (s_UserCode2 == string.Empty)
            {
                MessageBox.Show("Select user group to be edit.", m_sSource, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                if (btnEdit.Text.Trim() == "&Edit")
                {
                    cmbgroup.Text = s_GroupType;
                    btnEdit.Text = "&Update";
                    btnClose.Text = "&Cancel";
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    EditUserGroup();
                }
            }
        }

        private void dgvUserGroup_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            s_UserCode2 = dgvUserGroup[1, e.RowIndex].Value.ToString().Trim();
            s_GroupType = dgvUserGroup[0, e.RowIndex].Value.ToString().Trim();
        }

        private void dgvUserGroup_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            s_UserCode2 = dgvUserGroup[1, e.RowIndex].Value.ToString().Trim();
            s_GroupType = dgvUserGroup[0, e.RowIndex].Value.ToString().Trim();
        }

        private void dgvUserGroup_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            s_UserCode2 = dgvUserGroup[1, e.RowIndex].Value.ToString().Trim();
            s_GroupType = dgvUserGroup[0, e.RowIndex].Value.ToString().Trim();
        }

        private void dgvUserGroup_KeyDown(object sender, KeyEventArgs e)
        {
            //s_UserCode2 = dgvUserGroup[1, e.RowIndex].Value.ToString().Trim();
            //s_GroupType = dgvUserGroup[0, e.RowIndex].Value.ToString().Trim();
        }

        private void dgvUserList_KeyDown(object sender, KeyEventArgs e)
        {
            //s_UserCode = dgvUserList[0, e.RowIndex].Value.ToString().Trim();
        }
    }
}