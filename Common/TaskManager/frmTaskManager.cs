using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.TaskManager
{
    public partial class frmTaskManager : Form
    {
        private string m_sSource = string.Empty;
        private string m_strObject = string.Empty;
        private string m_strUser = string.Empty;
        private string m_strModeCode = string.Empty;
        private string m_strDetails = string.Empty;

        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }
        
        public frmTaskManager()
        {
            InitializeComponent();
        }

        private void frmTaskManager_Load(object sender, EventArgs e)
        {
            this.UpdateList();
            
        }

        private void UpdateList()
        {
            OracleResultSet result = new OracleResultSet();

            dgvList.Columns.Clear();
            dgvList.Columns.Add("OBJECT", "Object");
            dgvList.Columns.Add("DATE", "Date");
            dgvList.Columns.Add("USER", "User");
            dgvList.Columns.Add("SYSTEM", "System");
            dgvList.Columns.Add("DETAILS", "Details");
            
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 120;
            dgvList.Columns[1].Width = 180;
            dgvList.Columns[2].Width = 90;
            dgvList.Columns[3].Width = 50;
            dgvList.Columns[4].Width = 10;
            dgvList.Columns[4].Visible = false;

            dgvList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvList.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvList.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            txtDetails.Text = "DETAILS:";

            string strObject = string.Empty;
            string strDate = string.Empty;
            string strUsrCode = string.Empty;
            string strSystem = string.Empty;
            string strDetails = string.Empty;

            int intRow = 0;

            //result.Query = "select object,sys_date,user_code,system, details from module_info order by object";
            result.Query = "select object,sys_date,user_code,system, details from module_info where system = '" + m_sSource + "' order by object"; // RMC 20110805
            if (result.Execute())
            {
                while (result.Read())
                {
                    strObject = result.GetString(0);
                    strDate = string.Format("{0:MM/dd/yyyy HH:mm}",result.GetDateTime(1));
                    strUsrCode = result.GetString(2);
                    strSystem = result.GetString(3);
                    strDetails = result.GetString(4);

                    dgvList.Rows.Add("");
                    dgvList[0, intRow].Value = strObject;
                    dgvList[1, intRow].Value = strDate;
                    dgvList[2, intRow].Value = strUsrCode;
                    dgvList[3, intRow].Value = strSystem;
                    dgvList[4, intRow].Value = strDetails;

                    intRow++;
                }
            }
            result.Close();
            
            //result.Query = "select work_station,tdatetime,usr_code,mod_code from a_trail where mod_code = 'ALI' or mod_code = 'CLI' order by usr_code";
            // RMC 20110805 (s)
            if(m_sSource == "ASS")
                result.Query = "select work_station,tdatetime,usr_code,mod_code from a_trail where mod_code = 'ALI' order by usr_code";
            else
                result.Query = "select work_station,tdatetime,usr_code,mod_code from a_trail where mod_code = 'CLI' order by usr_code";
            // RMC 20110805 (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    strObject = result.GetString(0);
                    strDate = string.Format("{0:MM/dd/yyyy HH:mm}", result.GetDateTime(1));
                    strUsrCode = result.GetString(2);
                    strSystem = result.GetString(3);
                    strDetails = "";

                    dgvList.Rows.Add("");
                    dgvList[0, intRow].Value = strObject;
                    dgvList[1, intRow].Value = strDate;
                    dgvList[2, intRow].Value = strUsrCode;
                    dgvList[3, intRow].Value = strSystem;
                    dgvList[4, intRow].Value = strDetails;
                    intRow++;
                }
            }
            result.Close();

            btnEndTask.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string strUserCode = string.Empty;

            try
            {
                m_strObject = dgvList[0, e.RowIndex].Value.ToString().Trim();
                m_strUser = dgvList[2, e.RowIndex].Value.ToString().Trim();
                m_strModeCode = dgvList[3, e.RowIndex].Value.ToString().Trim();
                m_strDetails = dgvList[4, e.RowIndex].Value.ToString().Trim();

                txtDetails.Text = "DETAILS: " + m_strDetails;

                if (Granted.Grant("AUTTM-E"))
                    btnEndTask.Enabled = true;
                else
                {
                    if (m_strUser == AppSettingsManager.SystemUser.UserCode)
                        btnEndTask.Enabled = true;
                    else
                        btnEndTask.Enabled = false;

                }
            }
            catch { }
        }

        private void btnEndTask_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = "delete from module_info where object = :1 and user_code = :2 and details = :3";
            result.AddParameter(":1", m_strObject);
            result.AddParameter(":2", m_strUser);
            result.AddParameter(":3", m_strDetails);
            if (result.ExecuteNonQuery() == 0)
            {
            }
            
            //result.Query = "delete from a_trail where usr_code = :1 and mod_code = :2";
            result.Query = "delete from a_trail where work_station = :1 and usr_code = :2 and mod_code = :3"; //MCR 20150107
            result.AddParameter(":1", m_strObject);
            result.AddParameter(":2", m_strUser);
            result.AddParameter(":3", m_strModeCode);
            if (result.ExecuteNonQuery() == 0)
            {
            }

            string strObject = m_strObject + "/" + m_strUser + "/" + txtDetails.Text.ToUpper().Trim();

            // RMC 20151021 changed task manager module code (s)
            string sModCode = string.Empty;

            if (AppSettingsManager.GetSystemType == "C")
                sModCode = "CUTTM-E";
            else
                sModCode = "AUTTM-E";
            // RMC 20151021 changed task manager module code (e)

            //if (AuditTrail.AuditTrail.InsertTrail("AUTTM-E", "module_info", StringUtilities.StringUtilities.HandleApostrophe(strObject)) == 0)
            if (AuditTrail.AuditTrail.InsertTrail(sModCode, "module_info", StringUtilities.StringUtilities.HandleApostrophe(strObject)) == 0)   // RMC 20151021 changed task manager module code
            {
                result.Rollback();
                result.Close();
                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            this.UpdateList();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.UpdateList();
        }

        
    }
}