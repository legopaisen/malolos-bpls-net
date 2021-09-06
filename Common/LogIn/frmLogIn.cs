
// RMC 20111215 added display of system code where the user is currently logged in log-in validation
// RMC 20110809 Added validation of user multiple logged-in in different workstation

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.Message_Box;
using Amellar.Common.AppSettings; // ALJ 20100119
using Amellar.Common.StringUtilities;

namespace Amellar.Common.LogIn
{
    public partial class frmLogIn : Form
    {
        public SystemUser m_objSystemUser; // ALJ 20100119
        public string m_sUserCode;
        private string m_sPassword;
        public string sFormState = "LOGIN";
        public Teller objTeller;
                
        public frmLogIn()
        {
            InitializeComponent();
            m_objSystemUser = new SystemUser(); // ALJ 20100119
            objTeller = new Teller();
            
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            EncryptPass asas = new EncryptPass();
            txtPassword.Text = asas.EncryptPassword(txtPassword.Text);
            OracleResultSet result = new OracleResultSet();
            if (sFormState == "LOGIN")
                result.Query = "SELECT * FROM SYS_USERS WHERE trim(USR_PWD) = '" + StringUtilities.StringUtilities.HandleApostrophe(txtPassword.Text) + "' AND trim(USR_CODE) = '" + txtUserCode.Text + "'";    // RMC 20110414 added handle apostrophe
            else
                result.Query = "SELECT * FROM tellers WHERE trim(PSWD) = '" + StringUtilities.StringUtilities.HandleApostrophe(txtPassword.Text) + "' AND trim(teller) = '" + txtUserCode.Text + "'";   // RMC 20110414 added handle apostrophe
            if(result.Execute())
            {
                if(result.Read())
                {
                    m_sUserCode = txtUserCode.Text;
                    if (sFormState == "LOGIN")
                    {
                        if (!ValidateLogIN())   // RMC 20110809 Added validation of user multiple logged-in in different workstation
                            m_sUserCode = "";
                        else
                            AppSettingsManager.g_objSystemUser = m_objSystemUser; // ALJ 20100119
                    }
                    else
                        AppSettingsManager.objTeller = objTeller; // ALJ 20100119
                    this.Close();
                }
                else
                {
                    //MessageBox.Show("Invalid username and/or password");
                    frmMsgBox msgbox = new frmMsgBox();
                    string lblMsg = "Invalid Username and/or Password";
                    msgbox.m_slbl = lblMsg;
                    msgbox.m_sMsg = "CRITICAL";
                    msgbox.ShowDialog();
                    txtUserCode.Focus();
                    this.txtUserCode.SelectAll();

                }
            }

        }

        private void txtUserCode_Leave(object sender, EventArgs e)
        {
            /* // ALJ 20100119 put remarks original code of godfrey
            string sLn;
            string sFn;
            string sMi;
            string sPos;
            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT * FROM SYS_USERS WHERE USR_CODE = '" + txtUserCode.Text + "'";
            if(result.Execute())
            {
                if (result.Read())
                {
                    sLn = result.GetString("usr_ln").Trim();
                    sFn = result.GetString("usr_fn").Trim();
                    sMi = result.GetString("usr_mi").Trim();
                    sPos = result.GetString("usr_pos").Trim();

                    if (sMi.Length > 0)
                        sMi = sMi + ".";

                    txtFullname.Text = sLn + ", " + sFn + " " + sMi;
                    txtDesignation.Text = sPos;
                }
                else
                {
                    txtFullname.Text = "UNKNOWN USER";
                    txtDesignation.Text = "UNKNOWN DESIGNATION";
                }
            }
             */
            // (s) ALJ 20100119 replaced the codes above as fallows..
            if(sFormState == "LOGIN") 
            {
                if (m_objSystemUser.Load(txtUserCode.Text))
                {
                    txtFullname.Text = m_objSystemUser.UserName;
                    txtDesignation.Text = m_objSystemUser.Position;
                }
                else
                {
                    txtFullname.Text = "UNKNOWN USER";
                    txtDesignation.Text = "UNKNOWN DESIGNATION";
                }
                // (e) ALJ 20100119
            }
            else
            {
                if (objTeller.Load(txtUserCode.Text.Trim()))
                {
                    txtFullname.Text = objTeller.UserName;
                    txtDesignation.Text = objTeller.Position;
                }
                else
                {
                    txtFullname.Text = "UNKNOWN USER";
                    txtDesignation.Text = "UNKNOWN DESIGNATION";
                }
            }

            

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_objSystemUser.Clear(); // ALJ 20100119
            m_sUserCode = "";
            m_sPassword = "";
            this.Close();
            
        }

        private void frmLogIn_Load(object sender, EventArgs e)
        {

        }

        private bool ValidateLogIN()
        {
            // RMC 20110809 Added validation of user multiple logged-in in different workstation

            OracleResultSet result = new OracleResultSet();
            string sWorkStation = "";
            string sSysCode = "";

            if (m_sUserCode == "SYS_PROG")
                return true;

            result.Query = "select * from a_trail where usr_code = '" + m_sUserCode + "'";
            result.Query += " and (mod_code = 'ALI' or mod_code = 'CLI')";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sWorkStation = result.GetString("work_station").Trim();

                    // RMC 20111215 added display of system code where the user is currently logged in log-in validation (s)
                    sSysCode = result.GetString("mod_code").Trim();
                    if (sSysCode == "ALI")
                        sSysCode = "BPS";
                    else
                        sSysCode = "BTAS";
                    // RMC 20111215 added display of system code where the user is currently logged in log-in validation (e)

                    // AST 20150413 Temp Disable for Build-up (s)
                    //if (sWorkStation != AppSettingsManager.GetWorkstationName().Trim())
                    //{
                    //    //MessageBox.Show("You are already logged-in at workstation: " + sWorkStation, "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //    MessageBox.Show("You are already logged-in at workstation: " + sWorkStation, sSysCode, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //    result.Close();

                    //    return false;
                    //}
                    // AST 20150413 Temp Disable for Build-up (e)
                }
            }
            result.Close();
                       

            return true;
            

        }
    }
}