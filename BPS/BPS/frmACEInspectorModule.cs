using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Modules.InspectionTool;
using Amellar.Modules.InspectorsDetails;

namespace BPLSBilling
{
    public partial class frmACEInspectorModule : Form
    {
        public frmACEInspectorModule()
        {
            InitializeComponent();
        }

        private void btnInspector_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUI"))
            {
                using (frmInspector frmInspector = new frmInspector())
                {
                    frmInspector.ShowDialog();
                }
            }
        }

        private void btnUnofBns_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABUB"))
            {
                using (frmInspectorDetails frmInspectorDetails = new frmInspectorDetails())
                {
                    frmInspectorDetails.Source = "Unofficial";
                    frmInspectorDetails.ShowDialog();
                }
            }
        }

        private void btnDefBns_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABID"))
            {
                using (frmInspectorDetails frmInspectorDetails = new frmInspectorDetails())
                {
                    frmInspectorDetails.Source = "Deficient";
                    frmInspectorDetails.ShowDialog();
                }
            }
        }

        private void btnViolation_Click(object sender, EventArgs e)
        {
            // RMC 20150117
            if (AppSettingsManager.Granted("AUV"))
            {
                using (frmViolationTable frmViolation = new frmViolationTable())
                {
                    frmViolation.ShowDialog();
                }
            }
        }
    }
}