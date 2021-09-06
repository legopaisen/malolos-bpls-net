using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.SpecialOrdinances
{
    public partial class frmOrdinanceConfig : Form
    {
        public frmOrdinanceConfig()
        {
            InitializeComponent();
        }

        private void frmOrdinanceConfig_Load(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            cmbFees.Items.Clear();

            //pRec.Query = "select * from tax_and_fees_table where fees_type <> 'AD' order by fees_code ";
            pRec.Query = "select * from tax_and_fees_table where fees_type <> 'AD' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' order by fees_code "; //MCR 20141118
            if (pRec.Execute())
            {
                cmbFees.Items.Add("");
                while (pRec.Read())
                {
                    cmbFees.Items.Add(StringUtilities.HandleApostrophe(pRec.GetString("fees_desc").Trim()));
                }

                cmbFees.SelectedIndex = 0;
            }
            pRec.Close();

            rdoBoi.Checked = true;
            rdoFiscal.Checked = true;
            rdoTax.Checked = true;
            rdoRequired.Checked = true;
        }

        private void rdoBoi_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoBoi.Checked)
            {
                rdoPeza.Checked = false;
            }
        }

        private void rdoPeza_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoPeza.Checked)
            {
                rdoBoi.Checked = false;
            }
        }

        private void rdoFiscal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFiscal.Checked)
            {
                rdoGreen.Checked = false;
            }
        }

        private void rdoTax_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTax.Checked)
            {
                rdoFees.Checked = false;
                cmbFees.Enabled = false;
                cmbFees.SelectedIndex = 0;
            }
        }

        private void rdoFees_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFees.Checked)
            {
                rdoTax.Checked = false;
                cmbFees.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Add")
            {
                grpOrdinance.Enabled = true;
                grpExemption.Enabled = true;
                grpIncentive.Enabled = true;
                grpOtherConfig.Enabled = true;

                btnAdd.Text = "Save";
            }
            else
            {
                if (Validations())
                {
                }
            }
        }

        private bool Validations()
        {
            if (txtRate.Text.Trim() == "")
            {
                MessageBox.Show("Rate is required","Special Ordinances Config",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return false;
            }

            if (txtYrRange1.Text.Trim() == "")
            {
                MessageBox.Show("'Year Range from' is required", "Special Ordinances Config", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (txtYrRange2.Text.Trim() == "")
            {
                MessageBox.Show("'Year Range to' is required", "Special Ordinances Config", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }
    }
}