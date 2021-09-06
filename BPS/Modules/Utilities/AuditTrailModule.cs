using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.Utilities
{
    public class AuditTrailModule:AuditTrailBase
    {
        DataTable dataModules = new DataTable("Modules");
        string m_strModuleCode = string.Empty;

        public AuditTrailModule(frmAuditTrail Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            AuditTrailFrm.bin1.Enabled = false;
            AuditTrailFrm.btnSearchBIN.Enabled = false;
            AuditTrailFrm.cmbUser.Enabled = true;
            AuditTrailFrm.txtLastName.Enabled = false;
            AuditTrailFrm.txtFirstName.Enabled = false;
            AuditTrailFrm.txtMI.Enabled = false;
            AuditTrailFrm.cmbModule.Enabled = true;

            //AuditTrailFrm.bin1.txtTaxYear.Text = "";
            //AuditTrailFrm.bin1.txtBINSeries.Text = "";
            //AuditTrailFrm.cmbUser.Text = "";
            AuditTrailFrm.txtLastName.Text = "";
            AuditTrailFrm.txtFirstName.Text = "";
            AuditTrailFrm.txtMI.Text = "";
            AuditTrailFrm.cmbModule.Text = "";
            LoadUserCode();
            LoadModules();
        }

        public override void ModuleChange()
        {
            //string strUserCode = string.Empty;

            try
            {
                m_strModuleCode = ((DataRowView)this.AuditTrailFrm.cmbModule.SelectedItem)["User Rights"].ToString().Trim();
            }
            catch
            {
                m_strModuleCode = "";
            }

        }

        private void LoadModules()
        {
            OracleResultSet result = new OracleResultSet();
            int intCount = 0;

            AuditTrailFrm.cmbModule.Text = "";

            dataModules.Columns.Clear();
            dataModules.Columns.Add("User Rights", typeof(String));
            dataModules.Columns.Add("User Desc", typeof(String));

            //result.Query = "select count(*) from sys_module"; // GDE 20130219 change sys_mode to trail_table
            result.Query = "select count(*) from trail_table";

            int.TryParse(result.ExecuteScalar(), out intCount);

            if (intCount == 0)
            {
                MessageBox.Show("Record not found.", "Default Values", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                result.Query = "";
            }
            else if (intCount > 1)
            {
                //result.Query = "select trim(usr_rights), right_desc from sys_module order by usr_rights"; // GDE 20130219 change sys_mode to trail_table
                result.Query = "select trim(usr_rights), right_desc from trail_table order by usr_rights";
            }

            if (result.Execute())
            {
                while (result.Read())
                {
                    dataModules.Rows.Add(new String[] { result.GetString(0), StringUtilities.RemoveApostrophe(result.GetString(1)) });
                }
            }
            result.Close();

            AuditTrailFrm.cmbModule.DataSource = dataModules;
            AuditTrailFrm.cmbModule.DisplayMember = "User Desc";
            AuditTrailFrm.cmbModule.ValueMember = "User Desc";
            AuditTrailFrm.cmbModule.SelectedIndex = -1;
        }

        private void LoadUserCode()
        {
            int intCount = 0;

            AuditTrailFrm.cmbUser.Text = "";
            OracleResultSet result = new OracleResultSet();
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
                result.Query = "select trim(usr_code), trim(usr_code) from  sys_users order by usr_code";
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

        public override void Generate()
        {
            OracleResultSet result = new OracleResultSet();

            int intCount = 0;
            string strDateFrom = string.Format("{0:MM/dd/yyyy}", AuditTrailFrm.dtpFrom.Value);
            string strDateTo = string.Format("{0:MM/dd/yyyy}", AuditTrailFrm.dtpTo.Value);
            string strUserCode = AuditTrailFrm.cmbUser.Text.Trim();
            if (strUserCode.Trim() == string.Empty)
                strUserCode = "%";

            // RMC 20111007 Corrected audit trail by module (s)
            if (m_strModuleCode == "")
            {
                MessageBox.Show("Specify module first", "Audit Trail", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // RMC 20111007 Corrected audit trail by module (e)

            result.Query = "select count(*) as iCount from a_trail ";
            result.Query += string.Format(" where rtrim(mod_code) like '{0}%'", m_strModuleCode);
            result.Query += string.Format(" and trunc(a_trail.tdatetime) >= to_date('{0}','MM/dd/yyyy')", strDateFrom);
            result.Query += string.Format(" and trunc(a_trail.tdatetime) <= to_date('{0}','MM/dd/yyyy')", strDateTo);
            result.Query += string.Format(" and usr_code like '{0}%'", strUserCode);
            int.TryParse(result.ExecuteScalar().ToString(), out intCount);

            if (intCount == 0)
            {
                MessageBox.Show("No record found", "Audit Trail", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            result.Query = "select * from a_trail ";
            result.Query += string.Format(" where rtrim(mod_code) like '{0}%'", m_strModuleCode);
            result.Query += string.Format(" and trunc(a_trail.tdatetime) >= to_date('{0}','MM/dd/yyyy')", strDateFrom);
            result.Query += string.Format(" and trunc(a_trail.tdatetime) <= to_date('{0}','MM/dd/yyyy')", strDateTo);
            result.Query += string.Format(" and usr_code like '{0}%'", strUserCode);
            result.Query += " order by tdatetime";

            PrintOtherReports PrintClass = new PrintOtherReports();
            PrintClass = new PrintOtherReports();
            PrintClass.ReportName = "AUDIT TRAIL REPORTS";
            PrintClass.ReportName += "\n\n";
            PrintClass.ReportName += "Users Log by Transaction";
            PrintClass.ReportName += "\n";
            PrintClass.ReportName += strDateFrom + " to " + strDateTo;
            PrintClass.Source = "5";
            PrintClass.ReportQuery = result.Query.ToString();
            PrintClass.BIN = m_strModuleCode;
            PrintClass.FormLoad();
        }

    }
}
