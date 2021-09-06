using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Modules.Payment
{
    public partial class frmAdditionalPayment : Form
    {
        public frmAdditionalPayment()
        {
            InitializeComponent();
        }

        public string m_sPaymentType = "";

        private void frmAdditionalPayment_Load(object sender, EventArgs e)
        {
            m_sPaymentType = "";
        }

        private void rdoCash_Click(object sender, EventArgs e)
        {
            m_sPaymentType = "CS";
        }

        private void rdoCheck_Click(object sender, EventArgs e)
        {
            m_sPaymentType = "CQ";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_sPaymentType = "";
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool bCash = false;
            bool bCheck = false;

            bCash = rdoCash.Checked;
            bCheck = rdoCheck.Checked;

            if (rdoCash.Checked == true)
                m_sPaymentType = "CS";
            else if (rdoCheck.Checked == true)
                m_sPaymentType = "CQ";

            if (bCash == bCheck && bCash == false)
            {
                MessageBox.Show("Choose one payment type first!", "Additional Payment", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Close();
        }

    }
}