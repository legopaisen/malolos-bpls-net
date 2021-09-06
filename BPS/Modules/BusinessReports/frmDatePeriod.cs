using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmDatePeriod : Form
    {
        public frmDatePeriod()
        {
            InitializeComponent();
        }

        private bool m_bIsOk = false;
        private string m_sDatefrom = String.Empty;
        private string m_sDateto = String.Empty;
        private string m_sReportModule = string.Empty; //AFM 20200121

        public string ReportModule
        {
            get { return m_sReportModule; }
            set { m_sReportModule = value; }
        } 

        public bool IsOK
        {
            get { return m_bIsOk; }
            set { m_bIsOk = value; }
        }
        public string Datefrom
        {
            get { return m_sDatefrom; }
            set { m_sDatefrom = value; }
        }
        public string Dateto
        {
            get { return m_sDateto; }
            set { m_sDateto = value; }
        }

        private void frmDatePeriod_Load(object sender, EventArgs e)
        {
            if (m_sReportModule != "EngineeringReport" && m_sReportModule != "SanitaryReport")
            {
                dtpFrom.Value = Convert.ToDateTime(m_sDatefrom);
                dtpTo.Value = Convert.ToDateTime(m_sDateto);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_bIsOk = false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date) //AFM 20200130 fixed conflict in seconds
            {
                m_bIsOk = false;
                MessageBox.Show("Invalid Date.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            m_sDatefrom = string.Format("{0}/{1}/{2}", dtpFrom.Value.Month, dtpFrom.Value.Day, dtpFrom.Value.Year);
            m_sDateto = string.Format("{0}/{1}/{2}", dtpTo.Value.Month, dtpTo.Value.Day, dtpTo.Value.Year);
            m_bIsOk = true;

            if (ReportModule == "EngineeringReport") //AFM 20200121
            {
                frmBussReport frmbussreport = new frmBussReport();
                frmbussreport.m_dFrom = Convert.ToDateTime(m_sDatefrom);
                frmbussreport.m_dTo = Convert.ToDateTime(m_sDateto);
                frmbussreport.ReportSwitch = "EngineeringReport";
                frmbussreport.ReportName = "Engineering Clearance Report";
                frmbussreport.ShowDialog();
            }
            if (ReportModule == "SanitaryReport") //AFM 20200130
            {
                frmBussReport frmbussreport = new frmBussReport();
                frmbussreport.m_dFrom = Convert.ToDateTime(m_sDatefrom);
                frmbussreport.m_dTo = Convert.ToDateTime(m_sDateto);
                frmbussreport.ReportSwitch = "SanitaryReport";
                frmbussreport.ReportName = "Sanitary Clearance Report";
                frmbussreport.ShowDialog();
            }

            this.Close();
        }

    }
}