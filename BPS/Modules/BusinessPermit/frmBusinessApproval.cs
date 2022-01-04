using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

//AFM 20211217 MAO-21-16197	PAID BUSINESS APPROVAL BEFORE APPROVAL OF MAYOR
namespace Amellar.Modules.BusinessPermit
{
    public partial class frmBusinessApproval : Form
    {

        private string m_sBin = string.Empty;

        private string m_sModule;

        public string Module
        {
            get { return m_sModule; }
            set { m_sModule = value; }
        }

	

        public frmBusinessApproval()
        {
            InitializeComponent();
            bin2.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin2.GetDistCode = ConfigurationAttributes.DistCode;
            bin2.txtTaxYear.Focus();
        }

        private void txtBnsName_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmBusinessApproval_Load(object sender, EventArgs e)
        {
            if (m_sModule == "MAYOR APPROVAL")
            {
                this.Text = "Mayor's Approval";
                rdoALL.Visible = false;
                rdoApproved.Visible = false;
                rdoPending.Visible = false;

                this.btnApprove.Size = new System.Drawing.Size(185, 25);
                this.btnApprove.Location = new System.Drawing.Point(697, 380);
                btnApprove.Text = "Approve Business Permit";

                this.btnGenerate.Location = new System.Drawing.Point(168, 102);
            }

            ClearControls();
        }

        private void PopulateGrid()
        {
            dgvList.Rows.Clear();

            txtBnsName.Text = "";
            txtOwner.Text = "";
            txtTaxYear.Text = "";
            rdoApproved.Checked = false;
            rdoPending.Checked = false;
            rdoALL.Checked = true;
            btnApprove.Enabled = true;

            string sStatus = string.Empty;
            OracleResultSet res = new OracleResultSet();

            if (rdoPending.Checked == true)
                sStatus = "PENDING";
            else if (rdoApproved.Checked == true || m_sModule == "MAYOR APPROVAL")
                sStatus = "APPROVED";
            else
                sStatus = "%%";

            string sBnsCode = string.Empty;
            string sOwner = string.Empty;
            string sBin = string.Empty;
            string sTaxYear = string.Empty;
            res.Query = "select distinct bs.bin, bs.bns_nm, bs.bns_stat, bs.own_code, bs.bns_code, bs.tax_year, ba.status, ph.or_date, ba.approved_by from businesses bs, business_approval ba, pay_hist ph ";
            res.Query += " where bs.bin = ba.bin and bs.bin = ph.bin and ba.tax_year = ph.tax_year ";
            res.Query += " and EXTRACT(YEAR FROM TO_DATE(ph.or_date, 'DD-MON-RR')) like '%" + txtTaxYear.Text + "%' ";
            res.Query += " and ba.status like '" + sStatus + "' ";
            if(m_sModule == "MAYOR APPROVAL")
                res.Query += " and ba.mayor_approved IS NULL ";
            res.Query += " and bs.bin like '%" + m_sBin + "%' ";
            if(res.Execute())
                while (res.Read())
                {
                    sOwner = AppSettingsManager.GetBnsOwner(res.GetString("own_code"));
                    sBin = res.GetString("bin");
                    sTaxYear = res.GetString("tax_year");
                    dgvList.Rows.Add(res.GetString("bin"), res.GetString("bns_nm"), res.GetString("bns_code"), res.GetString("bns_stat"), res.GetString("tax_year"), sOwner, res.GetString("status"), res.GetDateTime("or_date").ToShortDateString(), res.GetString("approved_by"));
                }
            res.Close();
        }

        private void ClearControls()
        {
            m_sBin = "";
            txtBnsName.Text = "";
            bin2.txtTaxYear.Text = "";
            bin2.txtBINSeries.Text = "";
            txtOwner.Text = "";
            txtTaxYear.Text = "";

            dgvList.Rows.Clear();
            rdoApproved.Checked = false;
            rdoPending.Checked = false;

            //initialize
            rdoALL.Checked = true;
            btnApprove.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            OracleResultSet res = new OracleResultSet();
            if (string.IsNullOrEmpty(m_sBin))
            {
                MessageBox.Show("Select record to approve!");
                return;
            }
            if(btnApprove.Text == "Approve")
            {
                res.Query = "UPDATE BUSINESS_APPROVAL SET STATUS = 'APPROVED', DATE_APPROVED = '" + string.Format("{0:dd-MMM-yy}", AppSettingsManager.GetSystemDate()) + "', APPROVED_BY = '" + AppSettingsManager.SystemUser.UserCode + "' WHERE BIN = '" + m_sBin + "' AND STATUS = 'PENDING'";
                if (res.ExecuteNonQuery() == 0)
                { }
                ClearControls();
                PopulateGrid();
            }
            else // mayor's approval
            {
                res.Query = "UPDATE BUSINESS_APPROVAL SET MAYOR_APPROVED = 'YES', DATE_APPROVED_MAYOR = '" + string.Format("{0:dd-MMM-yy}", AppSettingsManager.GetSystemDate()) + "', APPROVED_BY = '" + AppSettingsManager.SystemUser.UserCode + "' WHERE BIN = '" + m_sBin + "' AND STATUS = 'APPROVED'";
                if (res.ExecuteNonQuery() == 0)
                { }
                ClearControls();
                PopulateGrid();
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if ((m_sModule == "BUSINESS APPROVAL" && (rdoALL.Checked == true || rdoApproved.Checked == true || rdoPending.Checked == true)) 
                || m_sModule == "MAYOR APPROVAL")
            {
                ClearControls();
                PopulateGrid();
            }
                
            else
                MessageBox.Show("Select filter!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            m_sBin = bin2.GetBin();
            if (!string.IsNullOrEmpty(m_sBin))
            {
                PopulateGrid();
            }
        }

        private void rdoALL_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string sBIN = string.Empty;
            try
            {
                sBIN = dgvList[0, e.RowIndex].Value.ToString();
            }
            catch { }

            if (!string.IsNullOrEmpty(sBIN))
            {
                bin2.txtTaxYear.Text = sBIN.Substring(7, 4).ToString();
                bin2.txtBINSeries.Text = sBIN.Substring(12, 7).ToString();

                m_sBin = bin2.GetBin();
            }

            try
            {
                txtBnsName.Text = dgvList[1, e.RowIndex].Value.ToString();
            }
            catch { }

            try
            {
                txtOwner.Text = dgvList[5, e.RowIndex].Value.ToString();
            }
            catch { }

            try
            {
                string sApproved = string.Empty;
                sApproved = dgvList[6, e.RowIndex].Value.ToString();

                if (sApproved == "APPROVED" && m_sModule == "BUSINESS APPROVAL")
                    btnApprove.Enabled = false;
                else
                    btnApprove.Enabled = true;
            }
            catch { }
        }

        private void rdoPending_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}