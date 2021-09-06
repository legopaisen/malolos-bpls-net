using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.Utilities
{
    public class AuditTrailUser : AuditTrailBase
    {
        OracleResultSet result = new OracleResultSet();
        string m_strUserCode = string.Empty;

        public AuditTrailUser(frmAuditTrail Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            AuditTrailFrm.bin1.Enabled = false;
            AuditTrailFrm.btnSearchBIN.Enabled = false;
            AuditTrailFrm.cmbUser.Enabled = true;
            AuditTrailFrm.txtLastName.Enabled = true;
            AuditTrailFrm.txtFirstName.Enabled = true;
            AuditTrailFrm.txtMI.Enabled = true;
            AuditTrailFrm.cmbModule.Enabled = false;

            //AuditTrailFrm.bin1.txtTaxYear.Text = "";
            //AuditTrailFrm.bin1.txtBINSeries.Text = "";
            AuditTrailFrm.cmbUser.Text = "";
            AuditTrailFrm.txtLastName.Text = "";
            AuditTrailFrm.txtFirstName.Text = "";
            AuditTrailFrm.txtMI.Text = "";
            AuditTrailFrm.cmbModule.Text = "";

            LoadUserCode();
        }

        private void LoadUserCode()
        {
            int intCount = 0;

            AuditTrailFrm.cmbUser.Text = "";

            DataTable dataUsers = new DataTable("Users");
            dataUsers.Columns.Clear();
            dataUsers.Columns.Add("User Code", typeof(String));
            dataUsers.Columns.Add("User Name", typeof(String));
            
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
                    dataUsers.Rows.Add(new String[] { result.GetString(0), StringUtilities.RemoveApostrophe(result.GetString(1)) });
                }
            }
            result.Close();

            AuditTrailFrm.cmbUser.DataSource = dataUsers;
            AuditTrailFrm.cmbUser.DisplayMember = "User Name";
            AuditTrailFrm.cmbUser.ValueMember = "User Name";
            AuditTrailFrm.cmbUser.SelectedIndex = -1;

        }

        public override void UserChange()
        {
            //string strUserCode = string.Empty;

            try
            {
                m_strUserCode = ((DataRowView)this.AuditTrailFrm.cmbUser.SelectedItem)["User Code"].ToString().Trim();
            }
            catch
            {
                m_strUserCode = "";
            }

            if (AuditTrailFrm.cmbUser.Text.Trim() != "")
            {

                result.Query = string.Format("select * from sys_users where usr_code = '{0}'", m_strUserCode);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        AuditTrailFrm.txtLastName.Text = result.GetString("usr_ln");
                        AuditTrailFrm.txtFirstName.Text = result.GetString("usr_fn");
                        AuditTrailFrm.txtMI.Text = result.GetString("usr_mi");
                        
                    }
                    else
                    {
                        if (m_strUserCode != "ALL")
                        {
                            MessageBox.Show("No record found", "Audit Trail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                result.Close();
            }
            else
            {
                AuditTrailFrm.txtLastName.Text = "";
                AuditTrailFrm.txtFirstName.Text = "";
                AuditTrailFrm.txtMI.Text = "";
            }
        }

        public override void Generate()
        {
            int intCount = 0;
            string strDateFrom = string.Format("{0:MM/dd/yyyy}", AuditTrailFrm.dtpFrom.Value);
            string strDateTo = string.Format("{0:MM/dd/yyyy}", AuditTrailFrm.dtpTo.Value);

            result.Query = "select count(*) as iCount from a_trail ";
            result.Query += string.Format(" where rtrim(usr_code) like '{0}%'", m_strUserCode);
            result.Query += string.Format(" and trunc(a_trail.tdatetime) >= to_date('{0}','MM/dd/yyyy')", strDateFrom);
            result.Query += string.Format(" and trunc(a_trail.tdatetime) <= to_date('{0}','MM/dd/yyyy')", strDateTo);
            int.TryParse(result.ExecuteScalar().ToString(), out intCount);

            if (intCount == 0)
            {
                MessageBox.Show("No record found", "Audit Trail", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            result.Query = "select * from a_trail ";
            result.Query += string.Format(" where rtrim(usr_code) like '{0}%'", m_strUserCode);
            result.Query += string.Format(" and trunc(a_trail.tdatetime) >= to_date('{0}','MM/dd/yyyy')", strDateFrom);
            result.Query += string.Format(" and trunc(a_trail.tdatetime) <= to_date('{0}','MM/dd/yyyy')", strDateTo);
            result.Query += " order by tdatetime";

            PrintOtherReports PrintClass = new PrintOtherReports();
            PrintClass = new PrintOtherReports();
            PrintClass.ReportName = "AUDIT TRAIL REPORTS";
            PrintClass.ReportName += "\n\n";
            PrintClass.ReportName += "Users Log by User";
            PrintClass.ReportName += "\n";
            PrintClass.ReportName += strDateFrom + " to " + strDateTo;
            PrintClass.Source = "4";
            PrintClass.ReportQuery = result.Query.ToString();
            PrintClass.BIN = m_strUserCode;
            PrintClass.FormLoad();
        }

        
    }
}
