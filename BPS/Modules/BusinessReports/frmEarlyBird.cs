using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmEarlyBird : Form
    {
        public frmEarlyBird()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (txtTop.Text.Trim() == "" || txtTaxYear.Text.Trim() == "")
            {
                MessageBox.Show("Fill out all the textbox first");
                return;
            }

            frmBussReport fBussReport = new frmBussReport();
            fBussReport.ReportSwitch = "ListOfEarlyBird";
            fBussReport.ReportName = "LIST OF EARLY BIRD TAXPAYER";
            fBussReport.m_sTop = txtTop.Text;
            fBussReport.m_sTaxYear = txtTaxYear.Text;
            fBussReport.ShowDialog();
        }

        private void frmEarlyBird_Load(object sender, EventArgs e)
        {
            txtTop.Text = "";
            txtTaxYear.Text = "";
        }
    }
}