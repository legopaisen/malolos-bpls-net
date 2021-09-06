using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Amellar.BPLS.Billing
{
    public partial class frmApproveReturnMemo : Form
    {
        public frmApproveReturnMemo()
        {
            InitializeComponent();
        }

        public string Memo = string.Empty;
        public bool isOK = false;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Memo = txtMemo.Text.Trim();
            isOK = true;
            this.Close();
        }
    }
}