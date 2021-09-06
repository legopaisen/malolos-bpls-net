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

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmListofOR : Form
    {
        public frmListofOR()
        {
            InitializeComponent();
        }

        public string BIN
        {
            set { m_sBin = value; }
        }
        public string ORNumber
        {
            get { return m_sORno; }
        }
        private string m_sBin = string.Empty;
        private string m_sORno = string.Empty;
        public bool m_bOk = false;

        private void frmListofOR_Load(object sender, EventArgs e)
        {
            txtBIN.Text = m_sBin;
            PopulateFields();
        }

        private void PopulateFields()
        {
            OracleResultSet pSet = new OracleResultSet();
            dgvResult.Rows.Clear();
            pSet.Query = string.Format("select or_no,tax_year,qtr_paid,or_date,payment_type from pay_hist where bin = '{0}' order by tax_year,qtr_paid,or_date", StringUtilities.HandleApostrophe(m_sBin));
            if (pSet.Execute())
                while (pSet.Read())
                    dgvResult.Rows.Add(pSet.GetString("or_no"), pSet.GetString("tax_year"), pSet.GetString("qtr_paid"), pSet.GetDateTime("or_date").ToString("MM/dd/yyyy"), pSet.GetString("payment_type"));
            pSet.Close();

            string sOwnCode = string.Empty;
            pSet.Query = string.Format("select * from businesses where bin = '{0}'", StringUtilities.HandleApostrophe(m_sBin));
            if (pSet.Execute())
                while (pSet.Read())
                {
                    txtBnsName.Text = StringUtilities.RemoveApostrophe(pSet.GetString("bns_nm")).Trim();
                    sOwnCode = pSet.GetString("own_code");
                }
            pSet.Close();

            pSet.Query = string.Format("select * from own_names where own_code = '{0}'", sOwnCode);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    txtLName.Text = StringUtilities.RemoveApostrophe(pSet.GetString("own_ln")).Trim();
                    txtFName.Text = StringUtilities.RemoveApostrophe(pSet.GetString("own_fn")).Trim();
                    txtMI.Text = StringUtilities.RemoveApostrophe(pSet.GetString("own_mi")).Trim();
                }
            pSet.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (m_sORno.Trim() == "")
            {
                MessageBox.Show("Please select an item from the list.", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            m_bOk = true;
            this.Close();
        }

        private void dgvResult_SelectionChanged(object sender, EventArgs e)
        {
            m_sORno = dgvResult.CurrentRow.Cells[0].Value.ToString().Trim();
        }
    }
}