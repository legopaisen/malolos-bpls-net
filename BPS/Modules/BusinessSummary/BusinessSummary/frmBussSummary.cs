using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;

namespace BusinessSummary
{
    public partial class frmBussSummary : Form
    {
        public frmBussSummary()
        {
            InitializeComponent();
        }

        string m_strSwitch = "";
        public string Switch
        {
            set { m_strSwitch = value; }
        }

        private void frmBussSummary_Load(object sender, EventArgs e)
        {
            //Tmp
            //m_strSwitch = "BasedOnRecords";
            //m_strSwitch = "BasedOnYearRegistration";

            if (m_strSwitch == "BasedOnYearRegistration")
                txtYear.Enabled = true;
            else
                txtYear.Enabled = false;

            // RMC 20150429 corrected reports (s)
            if (AppSettingsManager.GetConfigValue("01") == "MUNICIPALITY")
                rbDist.Enabled = false;
            // RMC 20150429 corrected reports (e)
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            frmReport frmReport = null;

            string strRegYear = "";

            if (m_strSwitch == "BasedOnYearRegistration")
            {
                if (txtYear.Text.Trim() != "" && txtYear.Text.Trim().Length == 4)
                    strRegYear = txtYear.Text;
                else
                {// RMC 20150520 corrections in reports (s)
                    MessageBox.Show("Year of registration is required", "Management Reports", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }// RMC 20150520 corrections in reports (e)
            }

            if (rbBarangay.Checked == true)
                frmReport = new frmReport(frmReport.ReportType.By_Barangay, strRegYear);
            else if (rbDist.Checked == true)
                frmReport = new frmReport(frmReport.ReportType.By_District, strRegYear);
            else if (rbLineBusiness.Checked == true)
                frmReport = new frmReport(frmReport.ReportType.By_Line_Of_Business, strRegYear);             
            else if (rbOrgKind.Checked == true)
                frmReport = new frmReport(frmReport.ReportType.By_Org_Kind, strRegYear);                            
            else if (rbGrossRange.Checked == true)
                frmReport = new frmReport(frmReport.ReportType.By_Gross_Receipts, strRegYear);
            else if (rbCapitalRange.Checked == true)
                frmReport = new frmReport(frmReport.ReportType.By_Initial_Capital, strRegYear);

            frmReport.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbLineBusiness_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chkIncludeSub_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
