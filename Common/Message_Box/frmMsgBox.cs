
// RMC 20111226 added system code in Common:Message_Box

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.Message_Box;
using Amellar.Common.DataConnector; // RMC 20110809
using Amellar.Common.AppSettings;   // RMC 20110809

namespace Amellar.Common.Message_Box
{
    public partial class frmMsgBox : Form
    {
        public string m_sMsg;
        public string m_slbl;
        public string m_sModCode;   // RMC 20111226 added system code in Common:Message_Box
               
        public frmMsgBox()
        {
            InitializeComponent();
        }

        private void frmMsgBox_Load(object sender, EventArgs e)
        {
            frmX(m_sMsg, m_slbl);
        }

        public void frmX(String m_sMsg, String m_slblMsg)
        {
            if (m_sMsg == "EXIT")
            {
                lblMsg.Text = m_slblMsg;
                picQuestion.Visible = true;
                picInfo.Visible = false;
                picCritical.Visible = false;
                btnYes.Visible = true;
                btnNo.Visible = true;
            }
            else if(m_sMsg == "CRITICAL")
            {
                lblMsg.Text = m_slblMsg;
                picQuestion.Visible = false;
                picInfo.Visible = false;
                picCritical.Visible = true;
                btnYes.Visible = false;
                btnNo.Visible = true;
                btnNo.Text = "Ok";
            }
            else
            {
                lblMsg.Text = "GODFREY";
            }
            
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            if (m_sMsg == "EXIT")
            {
                // RMC 20110809 Added deletion of user login trail (s)
                OracleResultSet pCmd = new OracleResultSet();

                //pCmd.Query = "delete from a_trail where mod_code = 'ALI' and usr_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                pCmd.Query = "delete from a_trail where mod_code = '"+m_sModCode+"' and usr_code = '" + AppSettingsManager.SystemUser.UserCode + "'";  // RMC 20111226 added system code in Common:Message_Box
                if (pCmd.ExecuteNonQuery() == 0)
                { }
                // RMC 20110809 Added deletion of user login trail (e)

                Application.Exit();
            }
        }

 
        
    }
}