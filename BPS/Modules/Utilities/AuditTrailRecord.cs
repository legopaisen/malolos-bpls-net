

// RMC 20110908 added '%' before object variable to correct printing of audit trail by record

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.SearchBusiness;
using Amellar.Common.DataConnector;


namespace Amellar.Modules.Utilities
{
    public class AuditTrailRecord:AuditTrailBase
    {
        OracleResultSet result = new OracleResultSet();

        public AuditTrailRecord(frmAuditTrail Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            AuditTrailFrm.bin1.Enabled = true;
            AuditTrailFrm.btnSearchBIN.Enabled = true;
            AuditTrailFrm.cmbUser.Enabled = false;
            AuditTrailFrm.txtLastName.Enabled = false;
            AuditTrailFrm.txtFirstName.Enabled = false;
            AuditTrailFrm.txtMI.Enabled = false;
            AuditTrailFrm.cmbModule.Enabled = false;

           // AuditTrailFrm.bin1.txtTaxYear.Text = "";
           // AuditTrailFrm.bin1.txtBINSeries.Text = "";
            AuditTrailFrm.cmbUser.Text = "";
            AuditTrailFrm.txtLastName.Text = "";
            AuditTrailFrm.txtFirstName.Text = "";
            AuditTrailFrm.txtMI.Text = "";
            AuditTrailFrm.cmbModule.Text = "";

        }

        public override void SearchBin()
        {
            frmSearchBusiness frmSearchBns = new frmSearchBusiness();

            frmSearchBns.ShowDialog();
            if (frmSearchBns.sBIN.Length > 1)
            {
                //AuditTrailFrm.bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                //AuditTrailFrm.bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
            }
        }

        public override void Generate()
        {
            int intCount = 0;
            string strDateFrom = string.Format("{0:MM/dd/yyyy}", AuditTrailFrm.dtpFrom.Value);
            string strDateTo = string.Format("{0:MM/dd/yyyy}", AuditTrailFrm.dtpTo.Value);

            result.Query = "select count(*) as iCount from a_trail ";
            result.Query += " where rtrim(object) like '%" + AuditTrailFrm.bin1.GetBin() + "%' ";   // RMC 20110908 added '%' before object variable to correct printing of audit trail by record
            result.Query += string.Format(" and trunc(a_trail.tdatetime) >= to_date('{0}','MM/dd/yyyy')",strDateFrom);
            result.Query += string.Format(" and trunc(a_trail.tdatetime) <= to_date('{0}','MM/dd/yyyy')", strDateTo);
            int.TryParse(result.ExecuteScalar().ToString(), out intCount);

            if (intCount == 0)
            {
                MessageBox.Show("No record found", "Audit Trail", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;            
            }

            result.Query = "select * from a_trail ";
            result.Query += " where rtrim(object) like '%" + AuditTrailFrm.bin1.GetBin() + "%' ";   // RMC 20110908 added '%' before object variable to correct printing of audit trail by record
            result.Query += string.Format(" and trunc(a_trail.tdatetime) >= to_date('{0}','MM/dd/yyyy')", strDateFrom);
            result.Query += string.Format(" and trunc(a_trail.tdatetime) <= to_date('{0}','MM/dd/yyyy')", strDateTo);
            result.Query += " order by tdatetime";

            PrintOtherReports PrintClass = new PrintOtherReports();
            PrintClass = new PrintOtherReports();
            PrintClass.ReportName = "AUDIT TRAIL REPORTS";
            PrintClass.ReportName += "\n\n";
            PrintClass.ReportName += "Users Log by Business Records";
            PrintClass.ReportName += "\n";
            PrintClass.ReportName += strDateFrom + " to " + strDateTo;
            PrintClass.Source = "3";
            PrintClass.ReportQuery = result.Query.ToString();
            PrintClass.BIN = AuditTrailFrm.bin1.GetBin();
            PrintClass.FormLoad();
        }
    }
}
