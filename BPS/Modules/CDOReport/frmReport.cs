using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.Reports;
using Amellar.Modules.BusinessReports;

namespace CDOReport
{
    public partial class frmReport : Form
    {
        public bool bOfficial = false;
        public bool bUnOfficial = false;
        frmBussReport fBussReport = new frmBussReport();
        public string sBrgy = string.Empty;
        public string sUser = string.Empty;
        public DateTime dtFrom = DateTime.Now;
        public DateTime dtTo = DateTime.Now;

        ReportClass rClass = new ReportClass();
        public string m_sQuery;
        OracleResultSet result = new OracleResultSet();
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            ExecuteQueryCDO(m_sQuery);
        }

        private void ExecuteQueryCDO(string sQuery)
        {
            string sBin = string.Empty;
            dgvCDO.Rows.Clear();
            result.Query = m_sQuery;
            if (result.Execute())
            {
                while (result.Read())
                {
                    if (result.GetString("bin").Trim() != string.Empty && AppSettingsManager.GetBnsName(result.GetString("bin").Trim()) != string.Empty)
                    {
                        if(!AppSettingsManager.bIsPrinted(result.GetString("bin").Trim()))
                            dgvCDO.Rows.Add(true, result.GetString("bin").Trim(), AppSettingsManager.GetBnsName(result.GetString("bin").Trim()), AppSettingsManager.GetBnsAdd(result.GetString("bin").Trim(), ""), AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(result.GetString("bin").Trim())), string.Format("{0:dd-MMM-yyyy}", result.GetDateTime("tdatetime")));
                        else
                            dgvCDO.Rows.Add(false, result.GetString("bin").Trim(), AppSettingsManager.GetBnsName(result.GetString("bin").Trim()), AppSettingsManager.GetBnsAdd(result.GetString("bin").Trim(), ""), AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(result.GetString("bin").Trim())), string.Format("{0:dd-MMM-yyyy}", result.GetDateTime("tdatetime")));
                    }
                }
            }
            result.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //result.Query = "delete from PRINTED_CDO_UNOF where sys_user = '" + AppSettingsManager.SystemUser.UserCode.Trim() + "'";
            //if (result.ExecuteNonQuery() != 0)
            //{ }
            //result.Close();

            result.Query = "delete from CDO_REPORT where sys_user = '" + AppSettingsManager.SystemUser.UserCode.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            { }
            result.Close();


            for (int i = 0; i < dgvCDO.RowCount; i++)
            {
                if ((bool)dgvCDO.Rows[i].Cells[0].Value)
                {
                    result.Query = "insert into PRINTED_CDO_UNOF values (:1,:2,:3)";
                    result.AddParameter(":1", dgvCDO.Rows[i].Cells[1].Value.ToString());
                    result.AddParameter(":2", AppSettingsManager.SystemUser.UserCode.Trim());
                    result.AddParameter(":3", DateTime.Now);
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();

                    result.Query = "insert into CDO_REPORT values (:1, :2, :3, :4, :5, :6)";
                    result.AddParameter(":1", dgvCDO.Rows[i].Cells[1].Value.ToString());
                    result.AddParameter(":2", AppSettingsManager.GetBnsName(dgvCDO.Rows[i].Cells[1].Value.ToString()));
                    result.AddParameter(":3", AppSettingsManager.GetBnsAdd(dgvCDO.Rows[i].Cells[1].Value.ToString(), ""));
                    result.AddParameter(":4", AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(dgvCDO.Rows[i].Cells[1].Value.ToString())));
                    result.AddParameter(":5", dgvCDO.Rows[i].Cells[5].Value.ToString());
                    result.AddParameter(":6", AppSettingsManager.SystemUser.UserCode);
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();

                }
            }
            frmBussReport fBussReport = new frmBussReport();
            fBussReport.ReportSwitch = "CDO Report";
            fBussReport.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ExecuteQueryCDO(m_sQuery);
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                for (int i = 0; i < dgvCDO.Rows.Count; i++)
                {
                    dgvCDO.Rows[i].Cells[0].Value = true;
                }
            }
            else
                ExecuteQueryCDO(m_sQuery);
        }

        //private void LoadReports()
    }
}