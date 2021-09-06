using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.Tools;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmGender : Form // CJC 20130429
    {
        public frmGender()
        {
            InitializeComponent();
        }

        private void frmGender_Load(object sender, EventArgs e)
        {
            cmbGender.Items.Clear();
            cmbGender.Items.Add("ALL");
            cmbGender.Items.Add("MALE");
            cmbGender.Items.Add("FEMALE");
            cmbGender.Text = "ALL";

            cmbStatus.Items.Clear();
            //cmbStatus.Items.Add("ALL");
            cmbStatus.Items.Add("NEW");
            cmbStatus.Items.Add("RENEWAL");
            cmbStatus.Items.Add("RETIRED");
            cmbStatus.Text = "NEW";
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            frmReport report = new frmReport();
            report.ReportName = "LIST OF BUSINESSES BY GENDER";
            report.Data1 = cmbStatus.Text;
            report.Data2 = cmbGender.Text;
            report.DateFrom = dtpOrDateFr.Value;
            report.DateTo = dtpOrDateTo.Value;
            report.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}