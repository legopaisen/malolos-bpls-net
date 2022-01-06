//JHMN 20170104 switch of retirement certificate printing

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
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmRetCert : Form
    {
        public string m_sTaxYear = string.Empty;
        private bool m_bReprint = false;    // RMC 20171123 enable capturing of certification payment if retirement certificate is reprinted

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
                        txtDueTo.Text = string.Empty;
                        txtNotary.Text = string.Empty;
                        txtDoc.Text = string.Empty;
                        txtPage.Text = string.Empty;
                        txtBook.Text = string.Empty;
                    }
                }

                IsRetireBns(bin1.GetBin());
                if (txtORNo.Text.Trim() != "")
                {
                    LoadInfo(bin1.GetBin());
                    btnPrint.Enabled = true;   
                }
                else
                {
                    OracleResultSet result = new OracleResultSet();
                    result.Query = string.Format("SELECT * FROM Retired_Bns WHERE BIN = '{0}'", bin1.GetBin());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            this.LoadInfo(bin1.GetBin());
                            result.Close();

                            btnPrint.Enabled = true;
                            return;
                        }
                        // RMC 20180122 addl validation in printing retirement cert (s)
                        else
                        {
                            result.Close();
                            result.Query = string.Format("SELECT * FROM businesses WHERE BIN = '{0}' and bns_stat = 'RET'", bin1.GetBin());
                            result.Query += " and bin in (select bin from pay_hist where bns_stat = 'RET')";
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    this.LoadInfo(bin1.GetBin());
                                    result.Close();

                                    btnPrint.Enabled = true;
                                    return;
                                }
                            }
                        }
                        // RMC 20180122 addl validation in printing retirement cert (e)
                    }
                    result.Close();
                    
                    MessageBox.Show("Selected business has no record of retirement.","Retirement",MessageBoxButtons.OK, MessageBoxIcon.Stop);   // RMC 20151005 mods in retirement module
                    bin1.txtBINSeries.Text = string.Empty;
                    bin1.txtTaxYear.Text = string.Empty;
                    btnSearch.Text = "Search";
                    bin1.txtBINSeries.Text = string.Empty;
                    bin1.txtTaxYear.Text = string.Empty;
                    txtBNSAdd.Text = string.Empty;
                    txtBNSName.Text = string.Empty;
                    txtBNSOwner.Text = string.Empty;
                    txtORNo.Text = string.Empty;
                    dtpIssuedDate.Value = AppSettingsManager.GetSystemDate();
                    dtpCeasedDate.Value = AppSettingsManager.GetSystemDate();
                    bin1.txtTaxYear.Focus();
                    btnPrint.Enabled = false;
                    txtDueTo.Text = string.Empty;
                    txtNotary.Text = string.Empty;
                    txtDoc.Text = string.Empty;
                    txtPage.Text = string.Empty;
                    txtBook.Text = string.Empty;
                    return;
                }
            }
            else
            {
                btnSearch.Text = "Search";

                bin1.txtBINSeries.Text = string.Empty;
                bin1.txtTaxYear.Text = string.Empty;
                txtBNSAdd.Text = string.Empty;
                txtBNSName.Text = string.Empty;
                txtBNSOwner.Text = string.Empty;
                txtORNo.Text = string.Empty;
                dtpIssuedDate.Value = AppSettingsManager.GetSystemDate();
                dtpCeasedDate.Value = AppSettingsManager.GetSystemDate();
                bin1.txtTaxYear.Focus();
                btnPrint.Enabled = true;
                txtDueTo.Text = string.Empty;
                txtNotary.Text = string.Empty;
                txtDoc.Text = string.Empty;
                txtPage.Text = string.Empty;
                txtBook.Text = string.Empty;
                cmbInspector.Text = ""; // RMC 20171117 added saving of retirement certificate details (s)
                rdbCorp.Checked = false;
                rdbIndividual.Checked = false;  // RMC 20171117 added saving of retirement certificate details (e)

                m_bReprint = false;    // RMC 20171123 enable capturing of certification payment if retirement certificate is reprinted
            }
        }

        private void LoadInfo(string sBin)
        {
            txtBNSName.Text = AppSettingsManager.GetBnsName(sBin);
            txtBNSAdd.Text = AppSettingsManager.GetBnsAddress(sBin);
            txtBNSOwner.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));

            txtORNo.ReadOnly = false;   // RMC 20150429 corrected reports 
            m_bReprint = false;    // RMC 20171123 enable capturing of certification payment if retirement certificate is reprinted

            // RMC 20171117 added saving of retirement certificate details (s)
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select * from ret_info_cert where bin = '" + bin1.GetBin() + "' and tax_year = '" + m_sTaxYear + "'";
            pRec.Query += " order by updated_dt desc";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    txtDoc.Text = pRec.GetString("doc_no");
                    txtPage.Text = pRec.GetString("page_no");
                    txtBook.Text = pRec.GetString("book_no");
                    txtNotary.Text = pRec.GetString("notary");
                    txtDueTo.Text = pRec.GetString("due_to");
                    cmbInspector.Text = pRec.GetString("inspector");
                    if (pRec.GetString("cert_op").Trim() == "1")
                    {
                        rdbIndividual.Checked = true;
                        rdbCorp.Checked = false;
                    }
                    else
                    {
                        rdbIndividual.Checked = false;
                        rdbCorp.Checked = true;
                    }

                    m_bReprint = true;    // RMC 20171123 enable capturing of certification payment if retirement certificate is reprinted
  
                }
            }
            pRec.Close();
            // RMC 20171117 added saving of retirement certificate details (e)
        }

        private void frmRetCert_Load(object sender, EventArgs e)
        {
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;  
            bin1.GetDistCode = ConfigurationAttributes.DistCode;

            bin1.txtTaxYear.Focus();

            dtpCeasedDate.Value = AppSettingsManager.GetSystemDate();
            dtpIssuedDate.Value = AppSettingsManager.GetSystemDate();

            btnPrint.Enabled = false;   // RMC 20151005 mods in retirement module
            LoadInspector(); //JARS 20170823
        }


        private void LoadInspector()
        {
            OracleResultSet pRec = new OracleResultSet();
            cmbInspector.Items.Add("");
            pRec.Query = "select * from inspector";
            if(pRec.Execute())
            {
                while(pRec.Read())
                {
                    cmbInspector.Items.Add(pRec.GetString("inspector_code"));
                }
            }
            pRec.Close();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string sCertificationOption = "";
            bool bFull = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from waive_tbl where bin = '" + bin1.GetBin() +"'";
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


            if(rdbCorp.Checked)
            {
                sCertificationOption = "Secretary's Certificate";
            }
            else if(rdbIndividual.Checked)
            {
                sCertificationOption = "Affidavit of Closure";
            }

            // RMC 20171117 added saving of retirement certificate details (s)
            result.Query = "select * from ret_info_cert where bin = '" + bin1.GetBin() + "'";
            result.Query += " and doc_no = '" +txtDoc.Text.Trim() + "'";
            result.Query += " and page_no = '" + txtPage.Text.Trim() + "'";
            result.Query += " and book_no = '" + txtBook.Text.Trim() + "'";
            if(result.Execute())
            {
                if(result.Read())
                {
                    result.Close();
                }
                else
                {
                    result.Close();
                    result.Query = "insert into ret_info_cert values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11)";
                    result.AddParameter(":1",  bin1.GetBin());
                    result.AddParameter(":2", m_sTaxYear);
                    result.AddParameter(":3",StringUtilities.HandleApostrophe(txtDoc.Text.Trim()));
                    result.AddParameter(":4",StringUtilities.HandleApostrophe(txtPage.Text.Trim()));
                    result.AddParameter(":5",StringUtilities.HandleApostrophe(txtBook.Text.Trim()));
                    result.AddParameter(":6",StringUtilities.HandleApostrophe(txtNotary.Text.Trim()));
                    result.AddParameter(":7",StringUtilities.HandleApostrophe(txtDueTo.Text.Trim()));
                    result.AddParameter(":8",StringUtilities.HandleApostrophe(cmbInspector.Text.Trim()));
                    if (rdbIndividual.Checked)
                        result.AddParameter(":9", '1');
                    else
                        result.AddParameter(":9", '2');
                    result.AddParameter(":10",AppSettingsManager.SystemUser.UserCode);
                    result.AddParameter(":11", AppSettingsManager.GetCurrentDate());
                    if (result.ExecuteNonQuery() == 0)
                    { }
                }
            }
             
            // RMC 20171117 added saving of retirement certificate details (e)

            // RMC 20171123 enable capturing of certification payment if retirement certificate is reprinted (s)
            string sOrNo = string.Empty;
            string sOrDt = string.Empty;
            string sOrAmount = string.Empty;

            if (m_bReprint)
            {
                frmCertPayment formP = new frmCertPayment();
                formP.ReportSwitch = "Retirement";
                formP.ShowDialog();

                if (formP.Closed)
                {
                    MessageBox.Show("Transaction cancelled", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (formP.ORNo.Trim() == "" || formP.ORNo.Trim() == ".")
                {
                    m_bReprint = false;
                }
                else
                {
                    sOrNo = formP.ORNo;
                    sOrDt = formP.ORDate;
                    sOrAmount = formP.Amount;
                }

            }
            // RMC 20171123 enable capturing of certification payment if retirement certificate is reprinted (e)


            frmBussReport form = new frmBussReport();
            form.BIN = bin1.GetBin();
            form.ReportSwitch = "Retirement";
            //JARS 20170823 (S)
            form.DocNo = txtDoc.Text.Trim();
            form.PageNo = txtPage.Text.Trim();
            form.BookNo = txtBook.Text.Trim();
            form.Notary = txtNotary.Text.Trim();
            form.DueTo = txtDueTo.Text.Trim();
            form.Inspector = cmbInspector.Text.Trim();
            form.CertificationOption = sCertificationOption;
            // RMC 20171123 enable capturing of certification payment if retirement certificate is reprinted (s)
            form.ORNo = sOrNo;
            form.ORDate = sOrDt;
            form.ORAmount = sOrAmount;
            // RMC 20171123 enable capturing of certification payment if retirement certificate is reprinted (e)
            form.SeriesYr = txtSeriesYr.Text.Trim(); //AFM 20220106 MAO-22-16337


            
            form.ShowDialog();

            //btnPrint.Enabled = false;   // RMC 20151005 mods in retirement module
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
                    result.Close();

                    result.Query = "select * from retired_bns where bin = '" + sBin + "' and bns_stat = 'RET' order by tax_year desc";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sTaxYear = result.GetString("tax_year");
                        }
                    }
                    
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

            m_sTaxYear = sTaxYear;  
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

        private void txtSeriesYr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}