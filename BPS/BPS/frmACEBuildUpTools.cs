using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.BPLS.SearchAndReplace;
using Amellar.Common.AppSettings;

namespace BPLSBilling
{
    public partial class frmACEBuildUpTools : Form
    {
        public frmACEBuildUpTools()
        {
            InitializeComponent();
        }

        private void btnBinOwner_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABSRBR"))    // RMC 20110809
            {
                using (frmSearchAndReplace frmSNR = new frmSearchAndReplace())
                {
                    frmSNR.Text = "Replace owner of BIN";
                    frmSNR.iWindowState = 1;
                    frmSNR.bControlState = true;
                    frmSNR.m_sTrailDetail = "S&R";
                    frmSNR.ShowDialog();
                }
            }
        }

        private void btnReplaceOwner_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABSROR"))    // RMC 20110809
            {
                using (frmSearchAndReplace frmSNR = new frmSearchAndReplace())
                {
                    frmSNR.Text = "Replace owner codes";
                    frmSNR.iWindowState = 2;
                    frmSNR.bControlState = true;
                    frmSNR.m_sTrailDetail = "S&R";
                    frmSNR.ShowDialog();
                }
            }
        }

        private void btnEditOwner_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABSROE"))    // RMC 20110809
            {
                using (frmSearchAndReplace frmSNR = new frmSearchAndReplace())
                {
                    frmSNR.Text = "Edit owner's Information";
                    frmSNR.iWindowState = 0;
                    frmSNR.bControlState = true;
                    frmSNR.m_sTrailDetail = "S&R";
                    frmSNR.ShowDialog();
                }
            }
        }

        private void btnQueryOwner_Click(object sender, EventArgs e)
        {
            // RMC 20110831
            if (AppSettingsManager.Granted("ABSROQ"))
            {
                using (frmOwnerQuery frmOwnerQuery = new frmOwnerQuery())
                {
                    frmOwnerQuery.ShowDialog();
                }
            }
        }

        private void btnDeleteOwner_Click(object sender, EventArgs e)
        {
            // RMC 20110901
            if (AppSettingsManager.Granted("ABSROQ"))
            {
                using (frmDeleteOwner frmDeleteOwner = new frmDeleteOwner())
                {
                    frmDeleteOwner.ShowDialog();
                }
            }
        }
    }
}