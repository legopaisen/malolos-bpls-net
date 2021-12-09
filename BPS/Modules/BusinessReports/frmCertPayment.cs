
// RMC 20171114 added capturing of certification payment

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmCertPayment : Form
    {
        private string m_sOrDate = string.Empty;
        private bool m_bCancel = false;
        private string m_sReportSwitch = string.Empty;

        public string ORNo
        {
            get { return txtOrNo.Text; }
        }

        public string Amount
        {
            get { return txtAmount.Text; }
        }

        public string ORDate
        {
            get { return m_sOrDate; }
        }
        public bool Closed
        {
            get { return m_bCancel; }
        }
        public string ReportSwitch
        {
            get { return m_sReportSwitch; }
            set { m_sReportSwitch = value; }
        }


        public frmCertPayment()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtOrNo.Text.Trim() == "")
            {
                MessageBox.Show("O.R. no. is required","Certification",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            if (m_sReportSwitch == "Barangay Clearance" && txtIssuedON.Text.Trim() == "")
            {
                MessageBox.Show("Issued on is required", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            double dAmount = 0;
            double.TryParse(txtAmount.Text, out dAmount);
            if (m_sReportSwitch == "Barangay Clearance")
            { }

            if (dAmount <= 0 )
            {
                if (m_sReportSwitch == "WithBuss" || m_sReportSwitch == "CERT-STAT" ||
                    m_sReportSwitch == "Retirement")  // RMC 20171128 allow certifcation printing of With Business and Status even without certification payment
                {
                    if (MessageBox.Show("Payment amount is zero. Continue?", "Certification", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        MessageBox.Show("Payment amount is required", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Payment amount is required", "Certification", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            

            m_sOrDate = string.Format("{0:MM/dd/yyyy}",dtpDate.Value);

            this.Close();
        }

        private void frmCertPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel transaction?", "Certification", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m_bCancel = true;
                this.Close();
            }
            else
                m_bCancel = false;
        }

        private void frmCertPayment_Load(object sender, EventArgs e)
        {
            if (m_sReportSwitch == "Barangay Clearance")
            {
                lblAcctName.Text = "CTC No.: ";
                label1.Text = "Issued On : ";
                // txtAmount.Text = "0.00";
            }
            else
            {
                lblissuedOn.Visible = false;
                txtIssuedON.Visible = false;
            }
        }
    }
}