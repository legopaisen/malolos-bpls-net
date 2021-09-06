using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Modules.BusinessReports;

namespace BPLSBilling
{
    public partial class frmBnsOwnBday : Form
    {
        public frmBnsOwnBday()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            frmBussReport dlg = new frmBussReport();
            dlg.ReportSwitch = "ListBnsOwnBday";
            dlg.m_sMonth = cmbMonth.SelectedIndex.ToString("00");
            if (cmbMonth.SelectedIndex == 0)
                dlg.ReportName = "List of Business Owner's Birthday";
            else
                dlg.ReportName = "List of Business Owner's Birthday in the Month of " + cmbMonth.Text.Trim();
            dlg.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}