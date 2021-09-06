using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmEPSClearance : Form //JARS 20190122
    {
        public frmEPSClearance()
        {
            InitializeComponent();
        }

        private string m_sClearanceMode = ""; //JARS 20190130 TO USE THIS MODULE FOR ENGINEERING AND ZONING CLEARANCE.
        public string ClearanceMode
        {
            get { return m_sClearanceMode; }
            set { m_sClearanceMode = value; }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            frmBussReport report = new frmBussReport();
            if (m_sClearanceMode == "Engineering")
            {
                report.ReportSwitch = "ListEngineeringClearance";
                report.ReportName = "LIST OF ENGINEERING CLEARANCE";
            }
            else if (m_sClearanceMode == "Zoning")
            {
                report.ReportSwitch = "ListZoningClearance";
                report.ReportName = "LIST OF LOCATIONAL CLEARANCE";
            }
            report.m_dFrom = dtpFrom.Value;
            report.m_dTo = dtpTo.Value;
            report.ShowDialog();
        }

        private void frmEPSClearance_Load(object sender, EventArgs e)
        {
            if(m_sClearanceMode == "Engineering")
            {
                this.Text = "List of Engineering Clearance";
            }
            else if (m_sClearanceMode == "Zoning")
            {
                this.Text = "List of Zoning Clearance";
            }
        }
    }
}