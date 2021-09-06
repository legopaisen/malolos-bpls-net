using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.SearchOwner;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.SearchBusiness;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmBussCert : Form
    {
        private string m_sReportSwitch = string.Empty;
        string sOwnCode = string.Empty;
        

        public string ReportSwitch
        {
            get { return m_sReportSwitch; }
            set { m_sReportSwitch = value; }
        }

        public frmBussCert()
        {
            InitializeComponent();
        }

        private void frmBussCert_Load(object sender, EventArgs e)
        {
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
            bin1.txtTaxYear.Focus();

            bin1.txtTaxYear.Enabled = true;
            bin1.txtBINSeries.Enabled = true;
            btnSearch.Enabled = true;
            rdoNoRecord.Checked = true;

            txtLastName.Enabled = false;
            txtFirstName.Enabled = false;
            txtMI.Enabled = false;
            txtAddress.Enabled = false;
            txtAddress.Enabled = false;

            //if (ReportSwitch == "WithBussPermit") 
            //    label5.Text = "         Due to:";

            // RMC 20171114 corrections in certification printing (s)
            if (ReportSwitch == "WithBussPermit")
                this.Text = "Certificate of Business Permit";
            else if (ReportSwitch == "CERT-STAT")// RMC 20171121 transferred and modified Certificate of status
                this.Text = "Certificate of Business Status";
            else
                this.Text = "Business Certification";
            // RMC 20171114 corrections in certification printing (e)
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
                // RMC 20171128 adjustment in certificate of status and With Business (s)
                if (m_sReportSwitch == "CERT-STAT" || m_sReportSwitch == "WithBuss")
                    result.Query = string.Format("SELECT * FROM businesses WHERE bin = '" + sBin + "'");
                else// RMC 20171128 adjustment in certificate of status and With Business (e)
                    result.Query = string.Format("SELECT * FROM businesses WHERE bin = '" + sBin + "' AND tax_year = '" + ConfigurationAttributes.CurrentYear + "'");
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sBnsPermitNo = result.GetString("permit_no");
                        sOwnCode = result.GetString("own_code");


                        if (sOwnCode.Trim() != "")    
                        {
                            this.LoadInfo(sOwnCode.Trim(), sBin.Trim());
                            result.Close();

                            btnPrint.Enabled = true;
                            return;
                        }
                        //if (sBnsPermitNo.Trim() == "")
                        //{
                        //    this.LoadInfo(sOwnCode.Trim(), sBin.Trim());
                        //    result.Close();

                        //    btnPrint.Enabled = true;
                        //    return;
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Selected business already has permit. Please print business permit instead", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //    btnPrint.Enabled = false;
                        //    bin1.txtBINSeries.Text = "";
                        //    bin1.txtTaxYear.Text = "";

                        //    txtLastName.Text = string.Empty;
                        //    txtFirstName.Text = string.Empty;
                        //    txtMI.Text = string.Empty;
                        //    txtAddress.Text = string.Empty;
                        //    bin1.txtTaxYear.Focus();
                        //    btnSearch.Text = "Search";
                        //    return;
                        //}
                    }
                }
                result.Close();

                MessageBox.Show("Selected business has no record of application for the current year.", "Application", MessageBoxButtons.OK, MessageBoxIcon.Stop);   // RMC 20151005 mods in retirement module
                bin1.txtBINSeries.Text = "";
                bin1.txtTaxYear.Text = "";

                txtLastName.Text = string.Empty;
                txtFirstName.Text = string.Empty;
                txtMI.Text = string.Empty;
                txtAddress.Text = string.Empty;
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

                txtLastName.Text = string.Empty;
                txtFirstName.Text = string.Empty;
                txtMI.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtRequestedBy.Text = string.Empty;
                bin1.txtTaxYear.Focus();

                btnPrint.Enabled = false;  
            }
        }

        private void LoadInfo(string sOwnCode, string sBin)
        {
            txtLastName.Text = AppSettingsManager.GetBnsOwnerLastName(sOwnCode);
            txtFirstName.Text = AppSettingsManager.GetBnsOwnerFirstName(sOwnCode);
            txtMI.Text = AppSettingsManager.GetBnsOwnerMiName(sOwnCode);
            txtAddress.Text = AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(sBin));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoWithRecord_Click(object sender, EventArgs e)
        {
            if (rdoWithRecord.Checked == true)  
            {
                bin1.txtTaxYear.Enabled = true;
                bin1.txtBINSeries.Enabled = true;
                btnSearch.Enabled = true;
            }
        }

        private void rdoNoRecord_Click(object sender, EventArgs e)
        {
            if (rdoNoRecord.Checked == true)
            {
                bin1.txtTaxYear.Enabled = false;
                bin1.txtBINSeries.Enabled = false;
                btnSearch.Enabled = false;
                bin1.txtTaxYear.Text = "";
                bin1.txtBINSeries.Text = "";
                btnSearch.Text = "Search";
                txtAddress.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtMI.Text = "";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmBussReport frmBussReports = new frmBussReport();

            if (txtRequestedBy.Text.Trim() == "" || txtPurpose.Text.Trim() == "") //JHB 20180511
            {
                MessageBox.Show("Complete all the input fields first", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // RMC 20171114 added capturing of certification payment (s)
            frmCertPayment formP = new frmCertPayment();
            formP.ReportSwitch = m_sReportSwitch;
            formP.ShowDialog();

            if (formP.Closed)
            {
                MessageBox.Show("Transaction cancelled", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // RMC 20171114 added capturing of certification payment (e)

            frmBussReports.OwnLastName = txtLastName.Text.Trim();
            frmBussReports.OwnFirstName = txtFirstName.Text.Trim();
            frmBussReports.OwnMI = txtMI.Text.Trim();
            frmBussReports.OwnAddress = txtAddress.Text.Trim();
            frmBussReports.RequestedBy = txtRequestedBy.Text.Trim();
            frmBussReports.OwnCode = sOwnCode;
            frmBussReports.RequestedBy = txtRequestedBy.Text.Trim();
            frmBussReports.Purpose = txtPurpose.Text.Trim(); //JHB 20180511
          
            frmBussReports.sMode = "With Record";
            if (m_sReportSwitch == "WithBussPermit") //JARS 20170915
            {
                frmBussReports.ReportSwitch = "WithBussPermit";
                frmBussReports.BIN = bin1.GetBin();
            }
            // RMC 20171121 transferred and modified Certificate of status (s)
            else if (m_sReportSwitch == "CERT-STAT")
            {
                frmBussReports.ReportSwitch = "CertStat";
                frmBussReports.BIN = bin1.GetBin();
            }
            // RMC 20171121 transferred and modified Certificate of status (e)
            else
            {
                frmBussReports.ReportSwitch = "BussCertificate";

            }

            // RMC 20171114 added capturing of certification payment (s)
            frmBussReports.ORNo = formP.ORNo;
            frmBussReports.ORDate = formP.ORDate;
            frmBussReports.ORAmount = formP.Amount;
            // RMC 20171114 added capturing of certification payment (e)

            frmBussReports.ShowDialog();
        }

    }
}