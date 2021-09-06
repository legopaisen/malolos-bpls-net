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
using Amellar.Common.AuditTrail;
using Amellar.Modules.BusinessReports;

namespace Amellar.Modules.Retirement
{
    public partial class frmRetirementReport : Form
    {
        public frmRetirementReport()
        {
            InitializeComponent();
        }
        
        private string m_sReportName = String.Empty;

        private bool m_bCheckApp = false;

        private void frmRetirementReport_Load(object sender, EventArgs e)
        {
            LoadBrgy();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (chkAppDate.Checked == true)
                m_bCheckApp = true;
            else
                m_bCheckApp = false;

            m_sReportName = "Retirement Report";

            frmBussReport dlg = new frmBussReport();
            dlg.ReportSwitch = "RetirementReport";//39;
            dlg.ReportName = m_sReportName;
            dlg.BrgyName = cmbBrgy.Text;
            dlg.RetireFrom = dtpFrom.Text;
            dlg.RetireTo = dtpTo.Text;
            dlg.CheckApproved = m_bCheckApp;
            dlg.ShowDialog();
        }

        private void LoadBrgy()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "SELECT DISTINCT(TRIM(BRGY_NM)) FROM BRGY  ORDER BY TRIM(BRGY_NM) ASC";
            if (pSet.Execute())
            {
                cmbBrgy.Items.Add("ALL");
                while (pSet.Read())
                {
                    cmbBrgy.Items.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();
        }

        private void chkAppDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAppDate.Checked == true)
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
            }
            else
            {
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
            }
        }

        private void cmbBrgy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}