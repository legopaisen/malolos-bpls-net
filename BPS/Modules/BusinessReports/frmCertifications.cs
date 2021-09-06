//JHMN 20170103 different options of certificate

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmCertifications : Form
    {
        public frmCertifications()
        {
            InitializeComponent();
        }

        private void btnBusOwnership_Click(object sender, EventArgs e) //JHMN 20170103 Business Ownership button
        {
            using (frmCertification CertificationFrm = new frmCertification())
            {
                CertificationFrm.ShowDialog();
            }
        }

        private void btnRetirement_Click(object sender, EventArgs e) //JHMN 20170103 Retirement button
        {
            if (AppSettingsManager.Granted("ARCR"))    //JHB 20181114 add access grants
            {

                using (frmRetCert frmRetCert = new frmRetCert())
                {
                    frmRetCert.ShowDialog();
                }
            }
        }

        private void btnApplication_Click(object sender, EventArgs e) //JHMN 20170103 Application button
        {
            if (AppSettingsManager.Granted("ARCWAP"))    //JHB 20181114 add access grants
            {

                using (frmApplication frmApplication = new frmApplication())
                {
                    frmApplication.ShowDialog();
                }
            }
        }

        private void btnNoBusiness_Click(object sender, EventArgs e)//JHMN 20170103 No Business button
        {
            if (AppSettingsManager.Granted("ARCNR"))    //JHB 20181114 add access grants
            {
                using (frmNoBusiness frmNoBusiness = new frmNoBusiness())
                {
                    frmNoBusiness.ShowDialog();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) //JHMN 20170103 Cancel button
        {
            this.Close();
        }

        private void frmCertifications_Load(object sender, EventArgs e)
        {

        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (frmCertification frmCertification  = new frmCertification())
            {
                frmCertification.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ARCP"))    //JHB 20181114 add access grants
            {
                using (frmBussCert frmBussCert = new frmBussCert())
                {
                    //frmVerifyBusinesses.ShowDialog();
                    frmBussCert.ReportSwitch = "WithBuss";  // RMC 20171128 adjustment in certificate of status and With Business
                    frmBussCert.ShowDialog();
                }
            }
        }

        private void btnCertBussPermit_Click(object sender, EventArgs e) //JARS 20170915
        {
            if (AppSettingsManager.Granted("ARCBR"))    //JHB 20181114 add access grants
            {

                using (frmBussCert frmBussCert = new frmBussCert())
                {
                    frmBussCert.ReportSwitch = "WithBussPermit";
                    frmBussCert.ShowDialog();
                }
            }
        }

        private void btnCertStatus_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ARS"))    //JHB 20181114 add access grants
            {
                // RMC 20171121 transferred and modified Certificate of status
                using (frmBussCert frmBussCert = new frmBussCert())
                {
                    frmBussCert.ReportSwitch = "CERT-STAT";
                    frmBussCert.ShowDialog();
                }
            }
        }
    }
}