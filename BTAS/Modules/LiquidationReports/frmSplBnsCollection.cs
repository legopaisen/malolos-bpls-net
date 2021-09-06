using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;
using System.Collections;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmSplBnsCollection : Form
    {
        public frmSplBnsCollection()
        {
            InitializeComponent();
        }

        int iRowIndex = 0;
        int iCheck = 0;
        string m_sReportName = string.Empty;

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            m_sReportName = "COLLECTION OF SPECIAL BUSINESSES";

            if (iCheck == 0)
            {
                //MessageBox.Show("The first 8 Regulatory Fees will be displayed in individual column.\nThe rest will be automatically consolidated under OTHER FEES.", "No Regulatory Fees Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("The first 6 Regulatory Fees will be displayed in individual column.\nThe rest will be automatically consolidated under OTHER FEES.", "No Regulatory Fees Selected", MessageBoxButtons.OK, MessageBoxIcon.Information); // RMC 20140909 Migration QA
                PrepareDefaultFees();
            }
            else
                PrepareSelectedFees();

            if (AppSettingsManager.GenerateInfo(m_sReportName))
            {
                OracleResultSet pSet = new OracleResultSet();

                pSet.Query = string.Format("DELETE FROM REPORT_ABSTRACT WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.ExecuteNonQuery();
                pSet.Close();

                pSet.Query = string.Format("DELETE FROM GEN_INFO WHERE REPORT_NAME = '{0}'", m_sReportName);
                pSet.ExecuteNonQuery();
                pSet.Close();

                pSet.Query = string.Format("INSERT INTO GEN_INFO VALUES('{0}',sysdate,'{1}',{2},'COL')", m_sReportName, AppSettingsManager.SystemUser.UserCode, 1);
                pSet.ExecuteNonQuery();
                pSet.Close();

                frmLiqReports dlg = new frmLiqReports();
                dlg.ReportSwitch = "SplBnsCollection";
                dlg.ReportTitle = m_sReportName;
                dlg.DateFrom = dtpFrom.Value;
                dlg.DateTo = dtpTo.Value;
                dlg.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSplBnsCollection_Load(object sender, EventArgs e)
        {
            LoadFees();

            dtpFrom.Text = AppSettingsManager.GetSystemDate().ToShortDateString();
            dtpTo.Text = AppSettingsManager.GetSystemDate().ToShortDateString();
        }

        private void PrepareDefaultFees()
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = "delete from BTAX_REP_ABSTRACT where user_code = :1 and report_name = :2";
            result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
            result.AddParameter(":2", m_sReportName);
            if (result.ExecuteNonQuery() != 0)
            { }
            result.Close();

            //for (int i = 0; i < 8; i++)
            for (int i = 0; i < 6; i++) // RMC 20140909 Migration QA
            {
                result.Query = "insert into BTAX_REP_ABSTRACT values (:1, :2, :3)";
                result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
                result.AddParameter(":2", dgvFees.Rows[i].Cells[1].Value.ToString());
                result.AddParameter(":3", m_sReportName);
                if (result.ExecuteNonQuery() != 0)
                { }
                result.Close();
            }
        }

        private void PrepareSelectedFees()
        {
            OracleResultSet result = new OracleResultSet();
            int iCnt = 0;
            result.Query = "delete from BTAX_REP_ABSTRACT where user_code = :1 and report_name = :2";
            result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
            result.AddParameter(":2", m_sReportName);
            if (result.ExecuteNonQuery() != 0)
            { }
            result.Close();

            for (int i = 0; i < dgvFees.RowCount; i++)
            {
                //if ((bool)dgvFees.Rows[i].Cells[0].Value == true && iCnt < 8)
                if ((bool)dgvFees.Rows[i].Cells[0].Value == true && iCnt < 6)   // RMC 20140909 Migration QA
                {
                    iCnt++;
                    result.Query = "insert into BTAX_REP_ABSTRACT values (:1, :2, :3)";
                    result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
                    result.AddParameter(":2", dgvFees.Rows[i].Cells[1].Value.ToString());
                    result.AddParameter(":3", m_sReportName);
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();
                }
            }

        }

        private void dgvFees_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            iRowIndex = 0;
            if (e.RowIndex != -1)
            {
                iRowIndex = e.RowIndex;
                iCheck = 0;

                for (int i = 0; i < dgvFees.RowCount; i++)
                {
                    if ((bool)dgvFees.Rows[i].Cells[0].Value == true)
                        iCheck++;

                    if (iCheck > 6)
                    {
                        MessageBox.Show("Maximum number of individual column for abstract report reached.");
                        dgvFees.Rows[iRowIndex].Cells[0].Value = false;
                        iRowIndex = 0;
                        return;
                    }
                }


            }
        }

        private void LoadFees()
        {
            dgvFees.Rows.Clear();
            OracleResultSet result = new OracleResultSet();

            result.Query = "select * from tax_and_fees_table order by fees_code";
            if (result.Execute())
                while (result.Read())
                    dgvFees.Rows.Add(false, result.GetString("fees_code"), result.GetString("fees_desc"));
            result.Close();
        }

    }
}