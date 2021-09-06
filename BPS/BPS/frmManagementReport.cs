using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Modules.BusinessReports;
using BusinessRoll;
using BusinessSummary;
using Amellar.Modules.OwnerProfile;
using Amellar.Modules.Retirement;

namespace BPLSBilling
{
    public partial class frmManagementReport : Form
    {

        public frmManagementReport()
        {
            InitializeComponent();
        }

        private void frmManagementReport_Load(object sender, EventArgs e)
        {
            
        }

        private void btnListofBns_Click(object sender, EventArgs e)
        {
            frmBussList frmBussList = new frmBussList();
            frmBussList.ShowDialog();
        }

        private void btnBnsRoll_Click(object sender, EventArgs e)
        {
            //frmBusinessRoll frmBusinessRoll = new frmBusinessRoll();
            //frmBusinessRoll.BusinessQueue = false;
            //frmBusinessRoll.ShowDialog();
            //JARS 20180515 (S)
            frmBussinessRollCategory frm = new frmBussinessRollCategory();
            frm.ShowDialog();
            //JARS 20180515 (E)
        }

        private void btnRetirement_Click(object sender, EventArgs e)
        {
            frmRetirementReport frmRetirementReport = new frmRetirementReport();
            frmRetirementReport.ShowDialog();
        }

        private void btnOwnership_Click(object sender, EventArgs e)
        {
            frmOwnershipRecord frmOwnershipRecord = new frmOwnershipRecord();
            frmOwnershipRecord.ShowDialog();
        }

        private void btnBnsQueApp_Click(object sender, EventArgs e)
        {
            frmBusinessRoll frmBusinessRoll = new frmBusinessRoll();
            frmBusinessRoll.BusinessQueue = true;
            frmBusinessRoll.ShowDialog();
        }

    }
}