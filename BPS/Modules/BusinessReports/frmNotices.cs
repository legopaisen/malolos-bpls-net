using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.SearchBusiness;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmNotices : Form
    {
        public frmNotices()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private void frmNotices_Load(object sender, EventArgs e)
        {
            rdoRenewal.Checked = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sOwnCode = "";

            if (btnSearch.Text == "Clear")
            {
                ClearControls();
                bin1.txtTaxYear.Focus();
            }
            else
            {
                if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
                {
                    if (ValidateBIN(bin1.GetBin()))
                    {
                        txtBnsName.Text = AppSettingsManager.GetBnsName(bin1.GetBin());
                        sOwnCode = AppSettingsManager.GetBnsOwnCode(bin1.GetBin());
                        txtBnsOwn.Text = AppSettingsManager.GetBnsOwner(sOwnCode);
                        btnSearch.Text = "Clear";
                    }
                }
                else
                {
                    frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                    frmSearchBns.ModuleCode = "";

                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();

                        if (ValidateBIN(bin1.GetBin()))
                        {
                            txtBnsName.Text = AppSettingsManager.GetBnsName(bin1.GetBin());
                            sOwnCode = AppSettingsManager.GetBnsOwnCode(bin1.GetBin());
                            txtBnsOwn.Text = AppSettingsManager.GetBnsOwner(sOwnCode);
                            btnSearch.Text = "Clear";
                        }
                    }
                }
            }
        }

        private bool ValidateBIN(string sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sTaxYear = "";

            if (sBIN.Length < 19)
                return false;

            if (rdoRenewal.Checked)
            {
                bool m_bBnsQue = false; //MCR 20150903 checker for validation
                pSet.Query = "select * from business_que where bin = '" + sBIN + "' and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sTaxYear = pSet.GetString("tax_year");
                        m_bBnsQue = true;
                        pSet.Close();
                    }
                    else
                    {
                        pSet.Close();

                        pSet.Query = "select * from businesses where bin = '" + sBIN + "'";
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                sTaxYear = pSet.GetString("tax_year"); 
                                m_bBnsQue = false;
                            }
                            else
                            {
                                pSet.Close();
                                MessageBox.Show("No record found.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                btnSearch.Text = "Clear";
                                return false;
                            }
                        }
                        pSet.Close();
                    }
                }

                if (m_bBnsQue == true)
                {
                    if (Convert.ToInt32(sTaxYear) <= Convert.ToInt32(AppSettingsManager.GetConfigValue("12")))
                        return true;
                    else
                    {
                        MessageBox.Show("BIN is not delinquent.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        ClearControls();
                        return false;
                    }
                }
                else
                {
                    if (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")))
                        return true;
                    else
                    {
                        MessageBox.Show("BIN is not delinquent.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        ClearControls();
                        return false;
                    }
                }
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearControls()
        {
            bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";
            txtBnsName.Text = "";
            txtBnsOwn.Text = "";
            btnSearch.Text = "Search";
        }

        private void rdoRenewal_CheckedChanged(object sender, EventArgs e)
        {
            ClearControls();
            
        }

        private void rdoReassess_CheckedChanged(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmBussReport frmBussReport = new frmBussReport();

            if (txtBnsName.Text.Trim() == "")
            {
                MessageBox.Show("Specify business name first.","Notice",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (rdoRenewal.Checked)
            {
                frmBussReport.ReportSwitch = "Renewal";
            }
            else if (rdoReassess.Checked)
            {
                frmBussReport.ReportSwitch = "Reassess";
            }
            else if(rdbNoticeClosure.Checked)
            {
                frmBussReport.ReportSwitch = "Closure";
            }
            frmBussReport.BIN = bin1.GetBin();
            frmBussReport.ShowDialog();
        }
    }
}