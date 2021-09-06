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
namespace BPLSBilling
{
    public partial class frmRetCert : Form
    {
        public string m_sTaxYear = string.Empty;    // RMC 20151005 mods in retirement module

        public frmRetCert()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                btnSearch.Text = "Clear";
                string sBin = string.Empty;
                if (txtBINYr.Text.Trim() == string.Empty && txtBINSerial.Text.Trim() == string.Empty)
                {
                    frmSearchBusiness fSearch = new frmSearchBusiness();
                    fSearch.ShowDialog();
                    sBin = fSearch.sBIN;
                }
                else
                    sBin = txtLGUCode.Text.Trim() + "-" + txtDISTCode.Text.Trim() + "-" + txtBINYr.Text.Trim() + "-" + txtBINSerial.Text.Trim();

                IsRetireBns(sBin);
                if (txtORNo.Text.Trim() != "")
                {
                    LoadInfo(sBin.Trim());
                    btnPrint.Enabled = true;   // RMC 20151005 mods in retirement module
                }
                else
                {
                    // AST 20150427 Added this block (s)
                    OracleResultSet result = new OracleResultSet();
                    result.Query = string.Format("SELECT * FROM Retired_Bns WHERE BIN = '{0}'", sBin);
                    if (result.Execute())
                    {
                        if (result.Read()) 
                        {
                            this.LoadInfo(sBin.Trim());
                            result.Close();

                            btnPrint.Enabled = true;   // RMC 20151005 mods in retirement module
                            return;
                        }
                    }
                    result.Close();
                    // AST 20150427 Added this block (e)

                    //MessageBox.Show("Selected business has no record of retirement.");
                    MessageBox.Show("Selected business has no record of retirement.","Retirement",MessageBoxButtons.OK, MessageBoxIcon.Stop);   // RMC 20151005 mods in retirement module
                    txtBINYr.Text = string.Empty;
                    txtBINSerial.Text = string.Empty;
                    btnSearch.Text = "Search";
                    btnPrint.Enabled = false;   // RMC 20151005 mods in retirement module
                    return;
                }
            }
            else
            {
                btnSearch.Text = "Search";

                txtBINYr.Text = string.Empty;
                txtBINSerial.Text = string.Empty;
                txtBNSAdd.Text = string.Empty;
                txtBNSName.Text = string.Empty;
                txtBNSOwner.Text = string.Empty;
                txtORNo.Text = string.Empty;
                dtpIssuedDate.Value = AppSettingsManager.GetSystemDate();
                dtpCeasedDate.Value = AppSettingsManager.GetSystemDate();
                txtBINYr.Focus();
                btnPrint.Enabled = false;   // RMC 20151005 mods in retirement module
            }
        }

        private void LoadInfo(string sBin)
        {
            txtBNSName.Text = AppSettingsManager.GetBnsName(sBin);
            txtBNSAdd.Text = AppSettingsManager.GetBnsAddress(sBin);
            txtBNSOwner.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));

            txtORNo.ReadOnly = false;   // RMC 20150429 corrected reports 
        }

        private void frmRetCert_Load(object sender, EventArgs e)
        {
            txtBINYr.Focus();
            txtLGUCode.Text = ConfigurationAttributes.LGUCode;
            txtDISTCode.Text = ConfigurationAttributes.DistCode;

            dtpCeasedDate.Value = AppSettingsManager.GetSystemDate();
            dtpIssuedDate.Value = AppSettingsManager.GetSystemDate();

            btnPrint.Enabled = false;   // RMC 20151005 mods in retirement module
        }

        private void txtBINYr_Leave(object sender, EventArgs e)
        {
            txtBINSerial.Focus();
        }

        private void txtBINSerial_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtBINSerial.Text.Trim() == "")
                    return;

                txtBINSerial.Text = string.Format("{0:0000000}", int.Parse(txtBINSerial.Text.Trim()));
            }
            catch
            {
                MessageBox.Show("Invalid BIN.");
                return;
            }
        }

        private void txtBINYr_TextChanged(object sender, EventArgs e)
        {
            if (txtBINYr.Text.Length == 4)
                txtBINSerial.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string sBin = string.Empty;
            sBin = txtLGUCode.Text.Trim() + "-" + txtDISTCode.Text.Trim() + "-" + txtBINYr.Text.Trim() + "-" + txtBINSerial.Text.Trim();

            if (dtpCeasedDate.Value.ToString("MM/dd/yyyy") == AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy"))
                if (MessageBox.Show("Warning: Ceased Date is set to current date\nproceed anyway?", "Certification of Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

            // RMC 20151005 mods in retirement module (s)
            bool bFull = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from waive_tbl where bin = '" + sBin + "'";
            if(result.Execute())
            {
                if(result.Read())
                {
                    if(result.GetString("full_partial") == "F")
                        bFull = true;
                    else
                        bFull = false;
                }
            }
            result.Close();
            // RMC 20151005 mods in retirement module (e)

            ReportClass rClass = new ReportClass();
            rClass.sOrNo = txtORNo.Text;
            rClass.sCeasedDate = dtpCeasedDate.Value.ToShortDateString();
            rClass.sIssuedDate = dtpIssuedDate.Value.ToShortDateString();
            rClass.sFPTaxYear = m_sTaxYear; // RMC 20151005 mods in retirement module
            rClass.m_bFullRetirement = bFull;
            rClass.RetCertLUBAO(sBin);
            rClass.PreviewDocu();

            btnPrint.Enabled = false;   // RMC 20151005 mods in retirement module
        }

        private void IsRetireBns(string sBin)
        {
            string sTaxYear = "";

            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from businesses where bin = '" + sBin + "' and bns_stat = 'RET'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sTaxYear = result.GetString("tax_year");
                }
                else
                {
                    // RMC 20151005 mods in retirement module (s)
                    result.Close();

                    result.Query = "select * from retired_bns where bin = '" + sBin + "' and bns_stat = 'RET' order by tax_year desc";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sTaxYear = result.GetString("tax_year");
                        }
                    }
                    // RMC 20151005 mods in retirement module (e)
                }
            }
            result.Close();

            result.Query = "select * from pay_hist where bin = :1 and tax_year = :2 and bns_stat = 'RET'";
            result.AddParameter(":1", sBin.Trim());
            result.AddParameter(":2", sTaxYear);
            if (result.Execute())
                if (result.Read())
                    txtORNo.Text = result.GetString("or_no");
            result.Close();

            m_sTaxYear = sTaxYear;  // RMC 20151005 mods in retirement module
        }

        private void dtpORDate_Leave(object sender, EventArgs e)
        {
            dtpChecker(dtpCeasedDate);
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;
        }

        private void txtORNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dtpChecker(DateTimePicker dtp)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtp.Value.Date > cdtToday.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtp.Value = AppSettingsManager.GetSystemDate();
            }
        }

        private void dtpCeasedDate_ValueChanged(object sender, EventArgs e)
        {
            dtpChecker(dtpCeasedDate);
        }

        private void dtpIssuedDate_ValueChanged(object sender, EventArgs e)
        {
            dtpChecker(dtpIssuedDate);
        }
    }
}