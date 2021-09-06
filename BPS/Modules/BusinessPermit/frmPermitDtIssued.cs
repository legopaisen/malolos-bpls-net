
// RMC 20140129 Added module to input permit date issuance before printing of Permit


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.BusinessPermit
{
    public partial class frmPermitDtIssued : Form
    {
        private string m_sBIN = "";
        private string m_sTaxYear = "";
        private DateTime m_dtPermitDate = DateTime.Now;

        public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; }
        }

        public string TaxYear
        {
            get { return m_sTaxYear; }
            set { m_sTaxYear = value; }
        }

        public DateTime PermitDate
        {
            get { return m_dtPermitDate; }
            set { m_dtPermitDate = value; }
        }

        public frmPermitDtIssued()
        {
            InitializeComponent();
        }

        private void frmPermitDtIssued_Load(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select distinct * from pay_hist where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "'";
            pSet.Query += " order by or_date ";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    dtpPayDate.Value = pSet.GetDateTime("or_date");
                }
            }
            pSet.Close();

            
        }
                
        private void btnOk_Click(object sender, EventArgs e)
        {
            string sPermitDate = string.Empty;
            string sPayDate = string.Empty;
            DateTime dtTmpPermitDate;
            DateTime dtTmpPayDate;

            sPermitDate = string.Format("{0:MM/dd/yyyy}", dtpPermitDate.Value);
            sPayDate = string.Format("{0:MM/dd/yyyy}", dtpPayDate.Value);

            DateTime.TryParse(sPermitDate, out dtTmpPermitDate);
            DateTime.TryParse(sPayDate, out dtTmpPayDate);

            if (dtTmpPermitDate < dtTmpPayDate)
            {
                MessageBox.Show("Permit issuance date should not be lower than the payment date.", "Business Permits", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtpPermitDate.Value = AppSettingsManager.GetCurrentDate();
                return;
            }

            m_dtPermitDate = dtpPermitDate.Value;
            this.Close();
        }
    }
}