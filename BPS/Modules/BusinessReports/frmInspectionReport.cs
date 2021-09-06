using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.BIN;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.SearchBusiness;
using Amellar.Modules.BusinessReports;

namespace InspectionTool
{
    public partial class frmInspectionReport : Form //JARS 20170911
    {
        public frmInspectionReport()
        {
            InitializeComponent();
        }

        private string m_sBIN = string.Empty;
        private string m_sInsCode = string.Empty;
        private string m_strModule = string.Empty;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (bin1.txtTaxYear.Text == "" || bin1.txtBINSeries.Text == "")
            {
                frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                frmSearchBns.ModuleCode = m_strModule;
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
                    txtBNSName.Text = string.Empty;
                    txtBNSOwner.Text = string.Empty;
                    txtOwnAdd.Text = string.Empty;
                    txtBnsAddress.Text = string.Empty;
                    cmbInspector.Text = string.Empty;
                    txtInspectorName.Text = string.Empty;
                    txtContactPerson.Text = string.Empty;
                    txtContactNumber.Text = string.Empty;
                    txtPosition.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                }
            }
            m_sBIN = bin1.GetBin();
            LoadInfo();
            txtContactNumber.Enabled = true;
            txtContactPerson.Enabled = true;
            txtPosition.Enabled = true;
            txtEmail.Enabled = true;
        }

        private void frmInspectionReport_Load(object sender, EventArgs e)
        {
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
            bin1.txtTaxYear.Focus();
            LoadInspector();
        }

        private void LoadInspector()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select inspector_code from inspector order by inspector_code";
            if(pSet.Execute())
            {
                cmbInspector.Items.Add("");
                while(pSet.Read())
                {
                    cmbInspector.Items.Add(pSet.GetString("inspector_code"));
                }
            }
            pSet.Close();
        }
        private void LoadInfo()
        {
            OracleResultSet pSet = new OracleResultSet();

            txtBNSName.Text = AppSettingsManager.GetBnsName(m_sBIN);
            txtBNSOwner.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBIN));
            txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(m_sBIN), true);
            txtBnsAddress.Text = AppSettingsManager.GetBnsAdd(m_sBIN, "");
            txtContactPerson.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBIN));
            pSet.Query = "select tel_no,email_add from own_profile where own_code = '" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "'";

            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    if (pSet.GetString("tel_no") == "")
                    {
                        txtContactNumber.Text = "";
                    }
                    else
                    {
                        txtContactNumber.Text = pSet.GetString("tel_no");
                    }
                    if (pSet.GetString("email_add") == "")
                    {
                        txtEmail.Text = "";
                    }
                    else
                    {
                        txtEmail.Text = pSet.GetString("email_add");
                    }
                }
            }
            pSet.Close();

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if(txtBNSName.Text == "")
            {
                MessageBox.Show("Nothing to Generate");
                return;
            }

            if(txtContactPerson.Text == "")
            {
                MessageBox.Show("Please Input Contact Person");
                return;
            }

            if(txtContactNumber.Text == "")
            {
                MessageBox.Show("Please Input Contact Number");
                return;
            }

            if(txtPosition.Text == "")
            {
                MessageBox.Show("Please Input Person's Position");
                return;
            }

            if(txtInspectorName.Text == "")
            {
                MessageBox.Show("Please Select Inspector");
            }


            frmBussReport frm = new frmBussReport();
            frm.ReportSwitch = "InspectionReport";
            frm.BIN = bin1.GetBin();
            frm.InspectorCode = m_sInsCode;
            frm.ContactNumber = txtContactNumber.Text.Trim();
            frm.ContactPerson = txtContactPerson.Text.Trim();
            frm.Position = txtPosition.Text.Trim();
            frm.Email = txtEmail.Text.Trim();

            frm.ShowDialog();

        }

        private void cmbInspector_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            m_sInsCode = cmbInspector.Text.Trim();
            string sInsFN = string.Empty;
            string sInsLN = string.Empty;
            string sInsMI = string.Empty;

            if(cmbInspector.Text != "")
            {
                pSet.Query = "select inspector_ln,inspector_fn,inspector_mi from inspector where inspector_code = '" + m_sInsCode + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sInsFN = pSet.GetString("inspector_fn");
                        sInsLN = pSet.GetString("inspector_ln");
                        sInsMI = pSet.GetString("inspector_mi");
                    }
                }
                pSet.Close();

                txtInspectorName.Text = sInsLN + ", " + sInsFN + " " + sInsMI;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBNSName.Text = string.Empty;
            txtBNSOwner.Text = string.Empty;
            txtOwnAdd.Text = string.Empty;
            txtBnsAddress.Text = string.Empty;
            cmbInspector.Text = string.Empty;
            txtInspectorName.Text = string.Empty;
            txtContactPerson.Text = string.Empty;
            txtContactNumber.Text = string.Empty;
            txtPosition.Text = string.Empty;
            txtEmail.Text = string.Empty;

            txtContactNumber.Enabled = false;
            txtContactPerson.Enabled = false;
            txtPosition.Enabled = false;
            txtEmail.Enabled = false;

            bin1.txtTaxYear.Text = "";  // RMC 20171122 added clear of bin when 'Clear' button was clicked in Inspection Report module
            bin1.txtBINSeries.Text = "";    // RMC 20171122 added clear of bin when 'Clear' button was clicked in Inspection Report module
        }
    }
}