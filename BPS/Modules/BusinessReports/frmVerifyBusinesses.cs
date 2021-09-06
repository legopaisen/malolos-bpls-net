// JHMN 20170130 verify businesses
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Amellar.Common.DataConnector;
using Amellar.Common.Tools;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmVerifyBusinesses : Form
    {
        public frmVerifyBusinesses()
        {
            InitializeComponent();
        }

        ArrayList arr_TaxYear = new ArrayList();
        public string p_sMNTaxYear = string.Empty, p_sMXTaxYear = string.Empty;

        private void frmVerifyBusinesses_Load(object sender, EventArgs e)
        {
            addSelection();
            loadSelection();
        }

        public void addSelection()
        {
            OracleResultSet pSet = new OracleResultSet();
            arr_TaxYear.Clear();
            pSet.Query = string.Format("SELECT MAX(tax_year), MIN(tax_year) FROM treasurers_module ORDER BY tax_year DESC");
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    p_sMXTaxYear = pSet.GetString(0).Trim();
                    p_sMNTaxYear = pSet.GetString(1).Trim();
                }
            }
            pSet.Close();

            pSet.Query = string.Format("SELECT distinct(tax_year) FROM treasurers_module ORDER BY tax_year DESC");
            if (pSet.Execute())
            {
                arr_TaxYear.Add("ALL");
                while (pSet.Read())
                {
                    arr_TaxYear.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();
        }

        public void loadSelection()
        {
            foreach (Object obj in arr_TaxYear)
            {
                string m_sTaxYear = (string)obj;
                cmbTaxYear.Items.Add(m_sTaxYear);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbTaxYear.Text.Trim() == "")
            {
                MessageBox.Show("Please select tax year.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmReport frmReport = new frmReport();
            //frmReport.ReportName = "VERIFY BUSINESSES";
            frmReport.ReportName = "VERIFIED BUSINESSES";   // RMC 20170221 addl mods in report of Verified businesses
            if(cmbTaxYear.Text.Trim() == "ALL")
                frmReport.TaxYear = p_sMNTaxYear + " - " + p_sMXTaxYear;
            else
                frmReport.TaxYear = cmbTaxYear.Text.Trim();
            frmReport.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}