using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.Reports
{
    public partial class frmNTRC : Form
    {
        public frmNTRC()
        {
            InitializeComponent();
        }

        private string m_sBnsCode = string.Empty;
        private string m_sBnsType = string.Empty;


        private void frmNTRC_Load(object sender, EventArgs e)
        {
            LoadMain();
        }

        private void txtTaxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }

        }

        private void LoadMain()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select bns_desc from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '"+ AppSettingsManager.GetConfigValue("07") +"' order by bns_code";

            if(pSet.Execute())
            {
                while(pSet.Read())
                {
                    cmbBnsType.Items.Add(pSet.GetString("bns_desc").Trim());
                }
            }
            pSet.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbBnsType.Text == "")
            {
                MessageBox.Show("Please Select Business Type.");
                return;
            }

            if (txtTaxYear.Text == "")
            {
                MessageBox.Show("Please Enter Tax Year.");
                return;
            }

            m_sBnsType = cmbBnsType.Text.Trim();

            ReportClass rClass = new ReportClass();
            rClass.m_sBusinessCode = m_sBnsCode;
            rClass.m_sBusinessDesc = cmbBnsType.Text.Trim();
            rClass.m_sTaxYear = txtTaxYear.Text.Trim();
            rClass.NTRC();
            rClass.PreviewDocu();

        }

        private void cmbBnsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select bns_code from bns_table where bns_desc = '"+cmbBnsType.Text.Trim()+"'";

            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    m_sBnsCode = pSet.GetString("bns_code");
                }
            }
            pSet.Close();
        }
    }
}