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
    public partial class frmEmpGender : Form
    {
        public frmEmpGender()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            frmReport form = new frmReport();
            form.ReportName = "LIST OF BUSINESSES WITH EMPLOYEE GENDER";
            form.Data1 = cmbStatus.Text;
            form.DateFrom = dtpOrDateFr.Value;
            form.DateTo = dtpOrDateTo.Value;
            form.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEmpGender_Load(object sender, EventArgs e)
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("ALL");
            cmbStatus.Items.Add("NEW");
            cmbStatus.Items.Add("RENEWAL");
            cmbStatus.Items.Add("RETIRED");
            cmbStatus.Text = "ALL";
        }
    }
}