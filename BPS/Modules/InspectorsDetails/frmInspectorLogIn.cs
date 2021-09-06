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


namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmInspectorLogIn : Form
    {
        //private SystemUser m_objSystemUser; // ALJ 20100119
        public string m_sUserCode;
        private string m_sPassword;

        public frmInspectorLogIn()
        {
            InitializeComponent();
         //   m_objSystemUser = new SystemUser(); // ALJ 20100119
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            EncryptPass asas = new EncryptPass();
            txtPassword.Text = asas.EncryptPassword(txtPassword.Text);
            OracleResultSet result = new OracleResultSet();

            result.Query = "select * from inspector where inspector_code = '" + StringUtilities.HandleApostrophe(txtUserCode.Text) + "' and password = '" + StringUtilities.HandleApostrophe(txtPassword.Text) + "'";
            if(result.Execute())
            {
                if(result.Read())
                {
                    m_sUserCode = txtUserCode.Text;
                    //AppSettingsManager.g_objSystemUser = m_objSystemUser; // ALJ 20100119
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
            
            
             
          

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
          //  m_objSystemUser.Clear(); // ALJ 20100119
            m_sUserCode = "";
            m_sPassword = "";
            this.Close();
            
        }

        private void txtUserCode_TextChanged(object sender, EventArgs e)
        {
            string sLn;
            string sFn;
            string sMi;
            string sPos;
            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT * FROM inspector WHERE inspector_CODE = '" + txtUserCode.Text + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sLn = result.GetString("inspector_ln").Trim();
                    sFn = result.GetString("inspector_fn").Trim();
                    sMi = result.GetString("inspector_mi").Trim();
                    sPos = result.GetString("position").Trim();

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
        }
    }
}