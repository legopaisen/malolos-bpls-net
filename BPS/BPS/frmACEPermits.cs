using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Modules.BusinessPermit;
using Amellar.Common.AppSettings;
using Amellar.Modules.HealthPermit;

namespace BPLSBilling
{
    public partial class frmACEPermits : Form
    {
        public frmACEPermits()
        {
            InitializeComponent();
        }

        private void btnRetCert_Click(object sender, EventArgs e)
        {
            frmRetCert frmRetCert = new frmRetCert();
            frmRetCert.ShowDialog();
        }

        private void btnBnsPermit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABMP"))   // RMC 20110808
            {
                using (frmBusinessPermit frmBusinessPermit = new frmBusinessPermit())
                {
                    frmBusinessPermit.ShowDialog();
                }
            }
        }

        private void btnSanitaryPermit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABSP")) // RMC 20141211 QA Health Permit module
            {
                frmCertificateAndPermit frmCertificateAndPermit = new frmCertificateAndPermit();
                frmCertificateAndPermit.CertificatePermitType = "Sanitary";
                frmCertificateAndPermit.ShowDialog();
            }
        }

        private void btnZoning_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABZC")) // RMC 20141211 QA Health Permit module
            {
                //frmCertificateAndPermit frmCertificateAndPermit = new frmCertificateAndPermit();
                //frmCertificateAndPermit.CertificatePermitType = "Zoning";
                frmZoningPermit frmCertificateAndPermit = new frmZoningPermit();    // RMC 20141222 modified permit printing
                frmCertificateAndPermit.ShowDialog();
            }
        }

        private void btnAnnlInsp_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABAIP")) // RMC 20141211 QA Health Permit module
            {
                //frmCertificateAndPermit frmCertificateAndPermit = new frmCertificateAndPermit();
                //frmCertificateAndPermit.CertificatePermitType = "Annual Inspection";
                frmAnnualInspection frmCertificateAndPermit = new frmAnnualInspection();    // RMC 20141222 modified permit printing
                frmCertificateAndPermit.ShowDialog();
            }
        }

        private void btnHealthPermit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABHC")) // RMC 20141211 QA Health Permit module
            {
                frmHealthPermit frmHealthPermit = new frmHealthPermit();
                frmHealthPermit.ShowDialog();
            }
        }

        private void btnSplPermit_Click(object sender, EventArgs e)
        {
            frmSpecialPermit fSpecial = new frmSpecialPermit();
            fSpecial.ShowDialog();
        }

        private void btnWorkingPermit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABHC")) //MCR 20150113 reused the Userlevel of Health
            {
                frmWorking frmWorking = new frmWorking();
                frmWorking.ShowDialog();
            }
        }
    }
}