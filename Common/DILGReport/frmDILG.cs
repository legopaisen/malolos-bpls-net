using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.DILGReport
{
    public partial class frmDILG : Form
    {
        public frmDILG()
        {
            InitializeComponent();
        }

        private void frmDILG_Load(object sender, EventArgs e)
        {
            txtTaxYear.Text = AppSettingsManager.GetCurrentDate().Year.ToString();
            txtTaxYear.Focus();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            DILGReportPreview template = new DILGReportPreview();            
            template.PreviewReport(dtFrom.Value, dtTo.Value, txtTaxYear.Text, chkSurch.Checked, false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtTaxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void dtFrom_Leave(object sender, EventArgs e)
        {
            if (dtFrom.Value > dtTo.Value)
                dtFrom.Value = dtTo.Value;
        }

        private void dtTo_Leave(object sender, EventArgs e)
        {   
            if (dtFrom.Value > dtTo.Value)
                dtTo.Value = dtFrom.Value;
        }
    }
}