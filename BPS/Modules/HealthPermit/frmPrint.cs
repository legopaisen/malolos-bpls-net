using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.ContainerWithShadow;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;
using Amellar.Common.SearchBusiness;
using Amellar.Common.TaskManager;
using Amellar.Modules.AdditionalBusiness;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmPrint : Form
    {
        public frmPrint()
        {
            InitializeComponent();
        }

        private string m_sReportType = string.Empty;
        private string m_sBin = string.Empty;
        private string m_sTaxYear = string.Empty;

        public string BIN
        {
            set { m_sBin = value; }
        }
        public string TaxYear
        {
            set { m_sTaxYear = value; }
        }

        private void btnHealth_Click(object sender, EventArgs e)
        {
            m_sReportType = "Health";
            frmPrinting frmPrinting = new frmPrinting();
            frmPrinting.ReportType = m_sReportType;
            frmPrinting.BIN = m_sBin;
            frmPrinting.TaxYear = m_sTaxYear;
            frmPrinting.ShowDialog();
        }

        private void btnWork_Click(object sender, EventArgs e)
        {
            m_sReportType = "Working Permit";
            frmPrinting frmPrinting = new frmPrinting();
            frmPrinting.ReportType = m_sReportType;
            frmPrinting.BIN = m_sBin;
            frmPrinting.TaxYear = m_sTaxYear;
            frmPrinting.ShowDialog();
        }
    }
}