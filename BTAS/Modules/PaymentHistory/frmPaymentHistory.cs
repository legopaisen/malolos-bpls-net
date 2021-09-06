using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.SearchBusiness;
using Amellar.Common.Reports;
using Amellar.Modules.LiquidationReports;
using ComponentFactory.Krypton;

namespace Amellar.Modules.PaymentHistory
{
    public partial class frmPaymentHistory : Form
    {
        ReportClass rClass = new ReportClass();
        frmLiqReports rLiqReports = new frmLiqReports();

        // RMC 20170130 enabled and modified tagging of payment under protest (s)
        public string m_sRecTaxYear = string.Empty;
        public string m_sRecOR = string.Empty;
        public string m_sRecTerm = string.Empty;
        public string m_sRecQtr = string.Empty;
        // RMC 20170130 enabled and modified tagging of payment under protest (e)
        
        public frmPaymentHistory()
        {
            InitializeComponent();
        }

        private void frmPaymentHistory_Load(object sender, EventArgs e)
        {
            // RMC 20170130 enabled and modified tagging of payment under protest (s)
            if (AppSettingsManager.GetSystemType == "C")
            {
                btnEditProtest.Visible = true;
                chkPaymentProtest.Visible = true;
            }
            else
            {
                btnEditProtest.Visible = false;
                chkPaymentProtest.Visible = false;
            }
            // RMC 20170130 enabled and modified tagging of payment under protest (e)

            this.txtStartYear.Text = ClassCode.StartingYear();
            bin1.txtLGUCode.Text = AppSettingsManager.GetConfigObject("10");
            bin1.txtDistCode.Text = AppSettingsManager.GetConfigObject("11");
            bin1.txtTaxYear.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchBin();
        }

        private void SearchBin()
        {
            ClassCode cCode = new ClassCode(this);
            cCode.SearchBinLoad();
        }

        private void txtStartYear_TextChanged(object sender, EventArgs e)
        {
            ClassCode cCode = new ClassCode(this);
            if (txtStartYear.Text.Length < 4)
                dgvPaymentInfo.Rows.Clear();
            else
                cCode.LoadData(bin1.GetBin(), txtStartYear.Text.Trim());
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //rClass.PayHistPrint(bin1.GetBin());
            //rClass.PayHist2(bin1.GetBin());
            //rClass.PreviewDocu();
            //rLiqReports.ReportPaymentHist(bin1.GetBin());
            rLiqReports.ReportSwitch = "Payment History";
            rLiqReports.m_sBIN = bin1.GetBin();
            rLiqReports.ShowDialog();
            
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            //rClass.PayHistDetails(dgvPaymentInfo.SelectedRows[0].Cells[1].Value.ToString(), bin1.GetBin());
            //rClass.PayHistDetails(dgvPaymentInfo.SelectedRows[0].Cells[1].Value.ToString(), bin1.GetBin(), dgvPaymentInfo.SelectedRows[0].Cells[4].Value.ToString());
            //rClass.PayHistDetails(dgvPaymentInfo.SelectedRows[0].Cells[1].Value.ToString(), bin1.GetBin(), dgvPaymentInfo.SelectedRows[0].Cells[4].Value.ToString(), dgvPaymentInfo.SelectedRows[0].Cells[0].Value.ToString());
            //rClass.PreviewDocu();

            rLiqReports.m_sBIN = bin1.GetBin();
            rLiqReports.m_sTaxYear = dgvPaymentInfo.SelectedRows[0].Cells[0].Value.ToString();
            if (dgvPaymentInfo.SelectedRows[0].Cells[1].Value.ToString() != "") //MCR 20210708
                rLiqReports.ORNo = dgvPaymentInfo.SelectedRows[0].Cells[1].Value.ToString();
            else
                rLiqReports.ORNo = dgvPaymentInfo.SelectedRows[0].Cells[10].Value.ToString();
            rLiqReports.m_sQtrPAid = dgvPaymentInfo.SelectedRows[0].Cells[4].Value.ToString();
            rLiqReports.ReportSwitch = "PayHistDetails";
            rLiqReports.ShowDialog();
        }

        private void btnEditProtest_Click(object sender, EventArgs e)
        {
            // RMC 20170130 enabled and modified tagging of payment under protest
            if (btnEditProtest.Text == "&Edit")
            {
                btnEditProtest.Text = "&Update";
                chkPaymentProtest.Enabled = true;
            }
            else
            {
                if (MessageBox.Show("Update record?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (m_sRecOR == "" || m_sRecTaxYear == "")
                    {
                        MessageBox.Show("Select OR to tag first");
                        return;
                    }

                    ClassCode cCode = new ClassCode(this);
                    cCode.TagPaymentProtest(bin1.GetBin(), m_sRecOR, m_sRecTaxYear);
                }

                btnEditProtest.Text = "&Edit";
                chkPaymentProtest.Enabled = false;
            }
        }

        private void dgvPaymentInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // RMC 20170130 enabled and modified tagging of payment under protest

            try
            {
                m_sRecTaxYear = dgvPaymentInfo[0, e.RowIndex].Value.ToString().Trim();
                m_sRecOR = dgvPaymentInfo[1, e.RowIndex].Value.ToString().Trim();
                m_sRecTerm = dgvPaymentInfo[3, e.RowIndex].Value.ToString().Trim();
                m_sRecQtr = dgvPaymentInfo[4, e.RowIndex].Value.ToString().Trim();
                
            }
            catch { 
                m_sRecTaxYear = "";
                m_sRecOR = "";
                m_sRecTerm = "";
                m_sRecQtr = "";
            }

            ClassCode cCode = new ClassCode(this);
            cCode.LoadPaymentProtest(bin1.GetBin(), m_sRecOR, m_sRecTaxYear);
        }
    }
}