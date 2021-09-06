
// RMC 20120221 added count of printed certifications in Certifications module
// RMC 20120220 Changed searching in Certification 


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
    public partial class frmCertification : Form
    {
        private string m_sReportSwitch = string.Empty;
        
        public string ReportSwitch
        {
            get { return m_sReportSwitch; }
            set { m_sReportSwitch = value; }
        }


        public frmCertification()
        {
            InitializeComponent();
        }

        private void frmCertification_Load(object sender, EventArgs e)
        {
            rdoNoRecord.Checked = true;
            rdoWithRecord.Checked = false;

            // RMC 20120221 added count of printed certifications in Certifications module (s)
            OracleResultSet pRec = new OracleResultSet();
            int iCnt = 0;

            pRec.Query = "select count(*) from a_trail where (mod_code = 'ARCWB' or mod_code = 'ARCNB')";
            int.TryParse(pRec.ExecuteScalar(), out iCnt);

            lblCount.Text = string.Format("{0:##}",iCnt);
            // RMC 20120221 added count of printed certifications in Certifications module (e)
        }

        private void rdoNoRecord_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoRecord.Checked)
            {
                
            }
        }

        private void rdoWithRecord_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoWithRecord.Checked)
            {
                
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                /*    frmSearchOwner SearchOwner = new frmSearchOwner();
                    SearchOwner.m_sPageWatch = "";
                    SearchOwner.ShowDialog();

                    txtCode.Text = SearchOwner.m_strOwnCode;

                    if (txtCode.Text.Trim() != "")
                    {
                        txtLastName.Text = SearchOwner.m_sOwnLn;
                        txtFirstName.Text = SearchOwner.m_sOwnFn;
                        txtMI.Text = SearchOwner.m_sOwnMi;
                        txtAddress.Text = AppSettingsManager.GetBnsOwnAdd(txtCode.Text);
                        btnSearch.Text = "Clear";
                    }
                    else
                    {
                        ClearControls();

                    }
                */
                // RMC 20120220 Changed searching in Certification 

                // RMC 20120220 Changed searching in Certification (s)
                frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                frmSearchBns.ModuleCode = "";
                frmSearchBns.ShowDialog();

                if (frmSearchBns.sBIN.Length > 1)
                {
                    txtCode.Text = AppSettingsManager.GetBnsOwnCode(frmSearchBns.sBIN.Trim());

                    if (txtCode.Text.Trim() != "")
                    {
                        SetBnsOwner(txtCode.Text.ToString().Trim());
                        txtAddress.Text = AppSettingsManager.GetBnsOwnAdd(txtCode.Text);
                        btnSearch.Text = "Clear";
                    }
                    else
                    {
                        ClearControls();
                    }
                }
                else
                {
                    ClearControls();
                }
                // RMC 20120220 Changed searching in Certification (e)

                /*if (rdoNoRecord.Checked)
                {
                    if (!ValidateRecord())
                    {
                        MessageBox.Show("Owner is a registered business owner", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        ClearControls();
                        return;
                    }
                }*/

            }
            else
            {
                ClearControls();

            }
        }

        private void EnableControls(bool blnEnable)
        {
            txtLastName.ReadOnly = !blnEnable;
            txtFirstName.ReadOnly = !blnEnable;
            txtMI.ReadOnly = !blnEnable;
            txtAddress.ReadOnly = !blnEnable;
            
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmBussReport frmReport = new frmBussReport();

            if ( txtRequestedBy.Text.Trim() == "" || txtPurpose.Text.Trim() == "") //JHB 20180511
            {
                MessageBox.Show("Complete all the input fields first", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (txtLastName.Text.Trim() == "")
            {
                MessageBox.Show("No record to print","Certification",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            if (txtRequestedBy.Text.Trim() == "")
            {
                MessageBox.Show("'Requested by' required", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            /*if (rdoNoRecord.Checked)
            {
                if (!ValidateRecord())
                {
                    MessageBox.Show("Owner is a registered business owner", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                frmReport.OwnLastName = txtLastName.Text.Trim();
                frmReport.OwnFirstName = txtFirstName.Text.Trim();
                frmReport.OwnMI = txtMI.Text.Trim();
                frmReport.OwnAddress = txtAddress.Text.Trim();
                frmReport.OwnCode = "";
            }
            else
            {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Please select owner code from list of registered businesses", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                       
                frmReport.OwnCode = txtCode.Text.Trim();
                frmReport.OwnLastName = txtLastName.Text.Trim();
                frmReport.OwnFirstName = txtFirstName.Text.Trim();
                frmReport.OwnMI = txtMI.Text.Trim();
                frmReport.OwnAddress = txtAddress.Text.Trim();
            }*/

            if (!ValidateRecord())
            {
                MessageBox.Show("Owner is a registered business owner", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }

            frmReport.OwnCode = txtCode.Text.Trim();
            frmReport.OwnLastName = txtLastName.Text.Trim();
            frmReport.OwnFirstName = txtFirstName.Text.Trim();
            frmReport.OwnMI = txtMI.Text.Trim();
            frmReport.OwnAddress = txtAddress.Text.Trim();
            frmReport.RequestedBy = txtRequestedBy.Text.Trim();
            frmReport.Purpose = txtPurpose.Text.Trim();
            frmReport.ReportSwitch = "Certification";
            frmReport.ShowDialog();

            ClearControls();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateRecord()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sOwnCode = "";

            pRec.Query = "select * from own_names where trim(own_ln) = '" + txtLastName.Text.Trim() + "'";
            if(txtFirstName.Text.Trim() != "")
                pRec.Query += " and trim(own_fn) = '" + txtFirstName.Text.Trim() + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    //sOwnCode = pRec.GetString("own_code"); // AST 20150429
                    //txtCode.Text = sOwnCode; // AST 20150429
                }
            }
            pRec.Close();

            if (sOwnCode != "")
            {
                pRec.Query = "select * from businesses where own_code = '" + sOwnCode + "'";
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        pRec.Close();
                        return false;
                    }
                }
                pRec.Close();
            }

            return true;
        }

        private void ClearControls()
        {
            txtCode.Text = "";
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMI.Text = "";
            txtAddress.Text = "";
            txtRequestedBy.Text = "";
            btnSearch.Text = "Search";
        }

        private void SetBnsOwner(string sOwnCode)
        {
            // RMC 20120220 Changed searching in Certification 
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from own_names where own_code = '" + sOwnCode + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtLastName.Text = pSet.GetString("own_ln");
                    txtFirstName.Text = pSet.GetString("own_fn");
                    txtMI.Text = pSet.GetString("own_mi");
                }
            }
            pSet.Close();

        }
    }
}