using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Modules.BusinessMapping;
namespace BPLSBilling
{
    public partial class frmBrgyFilter : Form
    {
        public string m_sReportName = string.Empty;
        public string m_strBrgy = string.Empty;
        public frmBrgyFilter()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_strBrgy = string.Empty;
            this.Close();
        }

        private void frmBrgyFilter_Load(object sender, EventArgs e)
        {
            LoadBrgy();
        }

        private void LoadBrgy()
        {
            OracleResultSet result = new OracleResultSet();
            cmbBrgy.Items.Clear();
            result.Query = "select * from brgy order by brgy_nm";
            if (result.Execute())
            {
                while (result.Read())
                    cmbBrgy.Items.Add(result.GetString("brgy_nm"));
            }
            result.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            m_strBrgy = cmbBrgy.Text.Trim();
            //this.Close();
            using (frmReportTest frmReport = new frmReportTest())
            {
                //if(m_sReportName == "MAPPED WITH PERMIT")
                frmReport.ReportName = m_sReportName;
                frmReport.Barangay = m_strBrgy.Trim();
                frmReport.ReportSwitch = "";
                frmReport.ShowDialog();
            }
        }
    }
}