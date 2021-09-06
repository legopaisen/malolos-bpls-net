//JHMN 20170103 temporary certificate for business application without mayors permit number

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.SearchBusiness;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.Reports;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmApplication : Form
    {
        public string m_sTaxYear = string.Empty;
        
        public frmApplication()
        {
            InitializeComponent();
        }

        private void frmApplication_Load(object sender, EventArgs e)
        {
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;  
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
            bin1.txtTaxYear.Focus();

            btnPrint.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                btnSearch.Text = "Clear";
                string sBin = string.Empty;
                string sBnsPermitNo = string.Empty;

                if (bin1.txtTaxYear.Text == "" || bin1.txtBINSeries.Text == "")
                {
                    frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                    frmSearchBns.ModuleCode = "";
                    frmSearchBns.ShowDialog();

                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                    }
                    else
                    {
                        bin1.txtTaxYear.Text = "";
                        bin1.txtBINSeries.Text = "";
                    }
                }
                
                sBin = bin1.GetBin();

                OracleResultSet result = new OracleResultSet();
                result.Query = string.Format("SELECT * FROM businesses WHERE bin = '" + sBin + "' AND tax_year = '" + ConfigurationAttributes.CurrentYear + "'");
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sBnsPermitNo = result.GetString("permit_no");

                        if (sBnsPermitNo.Trim() == "")
                        {
                            this.LoadInfo(sBin.Trim());
                            result.Close();

                            btnPrint.Enabled = true;   
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Selected business already has permit. Please print business permit instead", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            btnPrint.Enabled = false;
                            bin1.txtBINSeries.Text = "";
                            bin1.txtTaxYear.Text = "";

                            txtBNSAdd.Text = string.Empty;
                            txtBNSName.Text = string.Empty;
                            txtBNSOwner.Text = string.Empty;
                            bin1.txtTaxYear.Focus();
                            btnSearch.Text = "Search";
                            return;
                        }
                    }
                }
                result.Close();

                MessageBox.Show("Selected business has no record of application for the current year.", "Application", MessageBoxButtons.OK, MessageBoxIcon.Stop);   // RMC 20151005 mods in retirement module
                bin1.txtBINSeries.Text = "";
                bin1.txtTaxYear.Text = "";

                txtBNSAdd.Text = string.Empty;
                txtBNSName.Text = string.Empty;
                txtBNSOwner.Text = string.Empty;
                bin1.txtTaxYear.Focus();
                btnSearch.Text = "Search";
                btnPrint.Enabled = false;   
                return;
            }
            else
            {
                btnSearch.Text = "Search";
                bin1.txtBINSeries.Text = "";
                bin1.txtTaxYear.Text = "";

                txtBNSAdd.Text = string.Empty;
                txtBNSName.Text = string.Empty;
                txtBNSOwner.Text = string.Empty;
                bin1.txtTaxYear.Focus();

                btnPrint.Enabled = false;  
            }
        }

        private void LoadInfo(string sBin)
        {
            txtBNSName.Text = AppSettingsManager.GetBnsName(sBin);
            txtBNSAdd.Text = AppSettingsManager.GetBnsAddress(sBin);
            txtBNSOwner.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));
        }

        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {   
            frmBussReport form = new frmBussReport();
            form.BIN = bin1.GetBin();
            form.ReportSwitch = "WithApplication";
            form.ShowDialog();

            btnPrint.Enabled = false;
        }
    }
}