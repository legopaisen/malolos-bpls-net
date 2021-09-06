using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Modules.LiquidationReports;
using Amellar.Common.Reports;
using Amellar.Modules.RCD;
using Amellar.Common.AppSettings;

namespace BTAS
{
    public partial class frmLiquidation : Form
    {
        public frmLiquidation()
        {
            InitializeComponent();
        }

        private void btnTellerTrans_Click(object sender, EventArgs e)
        {
            frmTellerTransaction frmTellerTransaction = new frmTellerTransaction();
            frmTellerTransaction.ReportSwitch = 5; //teller transaction
            frmTellerTransaction.ShowDialog();
        }

        private void btnTellerRecCollect_Click(object sender, EventArgs e)
        {
            frmTellerReportofCollections frmTellerReportofCollections = new frmTellerReportofCollections();
            frmTellerReportofCollections.ShowDialog();
        }

        private void btnAbsChk_Click(object sender, EventArgs e)
        {
            frmAbstractChecks frmAbstractChecks = new frmAbstractChecks();
            frmAbstractChecks.ReportSwitch = 3; // Check
            frmAbstractChecks.ShowDialog();
        }

        private void btnAbsCollect_Click(object sender, EventArgs e)
        {
            frmAbstractCollect frmAbstractCollect = new frmAbstractCollect();
            frmAbstractCollect.ReportSwitch = 1; // Abstract of Collection
            frmAbstractCollect.ShowDialog();
        }

        private void btnAbsPstOR_Click(object sender, EventArgs e)
        {
            frmAbstractCollect frmAbstractCollect = new frmAbstractCollect();
            frmAbstractCollect.ReportSwitch = 2; // Abstract of Posted OR
            frmAbstractCollect.ShowDialog();
        }

        private void btnRCD_Click(object sender, EventArgs e)
        {
            frmRCD frmRCD = new frmRCD();
            frmRCD.ShowDialog();
        }

        private void btnReprintRCD_Click(object sender, EventArgs e)
        {
            frmReprint frmReprint = new frmReprint();
            frmReprint.ShowDialog();
        }

        private void btnDCreport_Click(object sender, EventArgs e)
        {
            frmDebitCredit frmDebitCredit = new frmDebitCredit();
            frmDebitCredit.ShowDialog();
        }

        private void btnLstCancChk_Click(object sender, EventArgs e)
        {
            frmTellerTransaction frmTellerTransaction = new frmTellerTransaction();
            frmTellerTransaction.ShowDialog();
        }

        private void btnAbsCancOR_Click(object sender, EventArgs e)
        {
            frmAbstractOfCancelledOR frmAbstractOfCancelledOR = new frmAbstractOfCancelledOR();
            frmAbstractOfCancelledOR.AbstractReportFormat = 1;
            frmAbstractOfCancelledOR.ShowDialog();
        }

        private void btnAbsDMemo_Click(object sender, EventArgs e)
        {
            frmAbstractOfCancelledOR frmAbstractOfCancelledOR = new frmAbstractOfCancelledOR();
            frmAbstractOfCancelledOR.AbstractReportFormat = 3;
            frmAbstractOfCancelledOR.ShowDialog();
        }

        private void btnAbsCMemo_Click(object sender, EventArgs e)
        {
            btnAbsCMemo.PerformDropDown();
        }

        private void kryptonContextMenuItem1_Click(object sender, EventArgs e)
        {
            frmAbstractOfCancelledOR frmAbstractOfCancelledOR = new frmAbstractOfCancelledOR();
            frmAbstractOfCancelledOR.AbstractReportFormat = 2; //Credit
            frmAbstractOfCancelledOR.ShowDialog();
        }

        private void kryptonContextMenuItem2_Click(object sender, EventArgs e)
        {

            frmAbstractOfCancelledOR frmAbstractOfCancelledOR = new frmAbstractOfCancelledOR();
            frmAbstractOfCancelledOR.AbstractReportFormat = 4; //Tax Credit
            frmAbstractOfCancelledOR.ShowDialog();
        }

        private void frmLiquidation_Load(object sender, EventArgs e)
        {

        }

        private void kryptonHeaderGroup1_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLORCD_Click(object sender, EventArgs e)
        {
            // RMC 20150504 QA corrections 
            if (AppSettingsManager.Granted("CCLRA"))
            {
                frmTellerTransaction form = new frmTellerTransaction();
                form.ReportSwitch = 6;
                form.ShowDialog();
            }
        }

        private void btnAbsCollectBrgy_Click(object sender, EventArgs e) //AFM 20200204 MAO-20-12234
        {
            frmAbstractCollect frmAbstractCollect = new frmAbstractCollect();
            frmAbstractCollect.ReportSwitch = 3;
            frmAbstractCollect.ShowDialog();
        }

    }
}