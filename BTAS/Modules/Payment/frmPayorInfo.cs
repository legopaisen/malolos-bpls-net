using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.Payment
{
    public partial class frmPayorInfo : Form
    {
        public frmPayorInfo()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string m_sOwnCode = "", m_sLN = "", m_sFN = "", m_sMi = "";

        private void TextControls()
        {
            //foreach (TextBox c in this.Controls)
            foreach (TextBox c in Controls)
            {
                ((TextBox)c).Text = String.Empty;
            }
        }

        private void frmPayorInfo_Load(object sender, EventArgs e)
        {
            TextControls();
            m_sOwnCode = "";
            m_sLN = "";
            m_sFN = "";
            m_sMi = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String sString;
            if (txtLN.Text.Trim() == "")
            {
                MessageBox.Show("Last Name required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtLN.Focus();
                return;
            }

            if (txtStreet.Text.Trim() == "")
            {
                MessageBox.Show("Street name required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtStreet.Focus();
                return;
            }

            if (txtMunCity.Text.Trim() == "")
            {
                MessageBox.Show("Municipal name required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtMunCity.Focus();
                return;
            }

            if (MessageBox.Show("Save payor's information?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m_sOwnCode = AppSettingsManager.EnlistOwner(txtLN.Text.Trim(), txtFN.Text.Trim(), txtMI.Text.Trim(), txtAddress.Text.Trim(), txtStreet.Text.Trim(), txtDist.Text.Trim(), txtZone.Text.Trim(), txtBrgy.Text.Trim(), txtMunCity.Text.Trim(), txtProv.Text.Trim(), "");
               
                m_sLN = txtLN.Text.Trim();
                m_sFN = txtFN.Text.Trim();
                m_sMi = txtMI.Text.Trim();

                MessageBox.Show("Payor's Info saved!", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}