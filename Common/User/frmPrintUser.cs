using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.User
{
    public partial class frmPrintUser : Form
    {
        private PrintUser PrintClass = null;
        DataTable dataUsers = new DataTable("Users");
        DataTable dataModules = new DataTable("Modules");
        bool m_blnChangeCombo = false;

        public frmPrintUser()
        {
            InitializeComponent();
        }

        private void frmPrintUser_Load(object sender, EventArgs e)
        {
            LoadUsers();
            LoadModules();

            m_blnChangeCombo = true;
            chkUsers.Checked = true;
        }

        private void chkUsers_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkUsers.CheckState.ToString() == "Checked")
            {
                chkRights.Checked = false;
                cmbUser.Enabled = false;
                cmbModule.Enabled = false;
                cmbUser.SelectedIndex = 0;
                cmbModule.SelectedIndex = 0;
            }
        }

        private void chkRights_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkRights.CheckState.ToString() == "Checked")
            {
                chkUsers.Checked = false;
                cmbUser.Enabled = true;
                cmbModule.Enabled = true;
            }
        }

        private void LoadUsers()
        {
            OracleResultSet result = new OracleResultSet();
            int intCount = 0;

            dataUsers.Columns.Clear();
            dataUsers.Columns.Add("User Code", typeof(String));
            dataUsers.Columns.Add("User Name", typeof(String));

            dataUsers.Rows.Add(new String[] { "ALL", "ALL" });

            result.Query = "select count(*) from sys_users";

            int.TryParse(result.ExecuteScalar(), out intCount);

            if (intCount == 0)
            {
                MessageBox.Show("Record not found.", "Default Values", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                result.Query = "";
            }
            else if (intCount > 1)
            {
                result.Query = "select trim(usr_code), trim(usr_ln)||', '||trim(usr_fn)||' '||trim(usr_mi) from  sys_users order by usr_code";
            }
            
            if (result.Execute())
            {
                while (result.Read())
                {
                    dataUsers.Rows.Add(new String[] { result.GetString(0), StringUtilities.StringUtilities.RemoveApostrophe(result.GetString(1)) });
                }
            }
            result.Close();

            cmbUser.DataSource = dataUsers;
            cmbUser.DisplayMember = "User Code";
            cmbUser.ValueMember = "User Code";
            cmbUser.SelectedIndex = 0;
        }

        private void LoadModules()
        {
            OracleResultSet result = new OracleResultSet();
            int intCount = 0;

            dataModules.Columns.Clear();
            dataModules.Columns.Add("User Rights", typeof(String));
            dataModules.Columns.Add("User Desc", typeof(String));

            dataModules.Rows.Add(new String[] { "ALL", "ALL" });

            result.Query = "select count(*) from sys_module";

            int.TryParse(result.ExecuteScalar(), out intCount);

            if (intCount == 0)
            {
                MessageBox.Show("Record not found.", "Default Values", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                result.Query = "";
            }
            else if (intCount > 1)
            {
                result.Query = "select trim(usr_rights), right_desc from sys_module order by usr_rights";
            }

            if (result.Execute())
            {
                while (result.Read())
                {
                    dataModules.Rows.Add(new String[] { result.GetString(0), StringUtilities.StringUtilities.RemoveApostrophe(result.GetString(1)) });
                }
            }
            result.Close();

            cmbModule.DataSource = dataModules;
            cmbModule.DisplayMember = "User Rights";
            cmbModule.ValueMember = "User Rights";
            cmbModule.SelectedIndex = 0;
        }

        private void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(m_blnChangeCombo)
                cmbModule.SelectedIndex = 0;

            if (chkUsers.CheckState.ToString() == "Checked")
                cmbModule.Enabled = false;
            else
            {
                if (cmbUser.Text != "ALL")
                    cmbModule.Enabled = false;
                else
                    cmbModule.Enabled = true;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            PrintClass = new PrintUser();
            
            if(chkUsers.Checked == true)
            {
                PrintClass.Source = "1";
                PrintClass.ReportName = "List of BPLS System Users";

		    }
            else
		    {
                if (cmbUser.Text.ToString().Trim() == "ALL" && cmbModule.Text.ToString().Trim() == "ALL")
			    {
				    result.Query = "select a.usr_code, a.usr_rights,b.right_desc from sys_rights a, sys_module b";
				    result.Query+= " where a.usr_rights = b.usr_rights";
				    result.Query+= " order by usr_code,a.usr_rights"; 
			    }
			    else if (cmbUser.Text.ToString().Trim() == "ALL" && cmbModule.Text.ToString().Trim() != "ALL")
			    {
				    result.Query = "select a.usr_code, a.usr_rights,b.right_desc from sys_rights a, sys_module b";
				    result.Query+= " where a.usr_rights = b.usr_rights";
				    result.Query+= " and a.usr_rights = '" +cmbModule.Text.ToString().Trim()+ "'";
				    result.Query+= " order by usr_code,a.usr_rights"; 
			    }
			    else if (cmbUser.Text.ToString().Trim() != "ALL" && cmbModule.Text.ToString().Trim() == "ALL") 
			    {
				    result.Query = "select a.usr_code, a.usr_rights,b.right_desc from sys_rights a, sys_module b";
				    result.Query+= " where a.usr_rights = b.usr_rights";
				    result.Query+= " and a.usr_code = '"+ cmbUser.Text.ToString().Trim() +"'";
				    result.Query+= " order by usr_code,a.usr_rights";
			    }

                PrintClass.Source = "2";
                PrintClass.ReportName = "List of BPLS System Rights";
                PrintClass.ReportQuery = result.Query.ToString();
		    }

		    PrintClass.FormLoad();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkUsers_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}