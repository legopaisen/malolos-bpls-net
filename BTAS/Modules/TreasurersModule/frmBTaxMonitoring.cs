
// RMC 20111227 added Gross monitoring module for gross >= 200000
// RMC 20111221 added Btax Monitoring module


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.SOA;
using Amellar.Common.AuditTrail;
using Amellar.Common.LogIn;

namespace Amellar.BPLS.TreasurersModule
{
    public partial class frmBTaxMonitoring : Form
    {
        public SystemUser m_objSystemUser;

        // RMC 20111227 added Gross monitoring module for gross >= 200000 (s)
        private Monitoring MonitoringClass = null;  
        private string m_strSource = string.Empty;
        private string m_strMemo = string.Empty; //JARS 20170922

        public string Memo //JARS 20170922
        {
            get { return m_strMemo; }
            set { m_strMemo = value; }
        }
        public string SourceClass
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }
        // RMC 20111227 added Gross monitoring module for gross >= 200000 (e)

        public frmBTaxMonitoring()
        {
            InitializeComponent();
            m_objSystemUser = new SystemUser();
            bin1.GetLGUCode = AppSettingsManager.GetConfigObject("10"); //MCR 20140904
            bin1.GetDistCode = AppSettingsManager.GetConfigObject("11"); //MCR 20140904
        }

        private void frmBTaxMonitoring_Load(object sender, EventArgs e)
        {
            if (this.SourceClass == "Tax")
            {
                MonitoringClass = new MonitoringTax(this);
                
            }
            else if (this.SourceClass == "PostPayment") //JARS 20170922
            {
                MonitoringClass = new MonitoringPost(this);
                txtOrNo.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                txtMemo.Visible = true;
                btnViewMemo.Visible = false;
                btnViewSOA.Visible = false;
                btnReject.Visible = true;
                this.Text = "Post Payment Approval";
            }
            else
            {
                MonitoringClass = new MonitoringGross(this);
            }

            MonitoringClass.UpdateList();
        }

        

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadValues(e.RowIndex);
        }

        public void LoadValues(int iRow)
        {
            string sBIN = "";
            string sOwnCode = "";

            try
            {
                if (this.SourceClass == "PostPayment") //JARS 20170922
                {
                    sBIN = dgvList[0, iRow].Value.ToString().Trim();
                    //192-00-2011-0003513
                    bin1.txtTaxYear.Text = sBIN.Substring(7, 4);
                    bin1.txtBINSeries.Text = sBIN.Substring(12, 7);

                    txtBnsName.Text = AppSettingsManager.GetBnsName(sBIN);
                    txtBnsAddress.Text = AppSettingsManager.GetBnsAddress(sBIN);
                    sOwnCode = AppSettingsManager.GetOwnCode(sBIN);
                    txtOwnName.Text = AppSettingsManager.GetBnsOwner(sOwnCode);
                    txtOwnAddress.Text = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                    txtTaxYear.Text = dgvList[3, iRow].Value.ToString().Trim();
                    txtOrNo.Text = dgvList[1, iRow].Value.ToString().Trim();
                    txtMemo.Text = dgvList[8, iRow].Value.ToString().Trim();
                }
                else
                {
                    sBIN = dgvList[0, iRow].Value.ToString().Trim();
                    //192-00-2011-0003513
                    bin1.txtTaxYear.Text = sBIN.Substring(7, 4);
                    bin1.txtBINSeries.Text = sBIN.Substring(12, 7);

                    txtBnsName.Text = AppSettingsManager.GetBnsName(sBIN);
                    txtBnsAddress.Text = AppSettingsManager.GetBnsAddress(sBIN);
                    sOwnCode = AppSettingsManager.GetOwnCode(sBIN);
                    txtOwnName.Text = AppSettingsManager.GetBnsOwner(sOwnCode);
                    txtOwnAddress.Text = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                    txtTaxYear.Text = dgvList[8, iRow].Value.ToString().Trim();
                }
            }
            catch { }   // RMC 20120112 ADDED try & catch in frmBTaxMonitoring
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            MonitoringClass.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            MonitoringClass.Search();
        }

        private void btnViewSOA_Click(object sender, EventArgs e)
        {
            if (txtTaxYear.Text.Trim() != "")
            {
                frmSOA fSOA = new frmSOA();
                fSOA.iFormState = 0;
                fSOA.sBIN = bin1.GetBin();
                fSOA.txtTaxYear.Text = txtTaxYear.Text.ToString().Trim();
                fSOA.txtStat.Text = "REN";
                fSOA.GetData(bin1.GetBin());
                fSOA.ShowDialog();
            }
        }
        private void btnApprove_Click(object sender, EventArgs e)
        {
            MonitoringClass.Approve();
            MonitoringClass.Clear(); //JARS 20170922     
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            MonitoringClass.Return();

            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnViewMemo_Click(object sender, EventArgs e)
        {
            // RMC 20120103 added capturing of memoranda in approving btax
            if (txtTaxYear.Text.Trim() != "")
            {
                frmMemoranda MemoFrm = new frmMemoranda();
                MemoFrm.m_sQuery = "select memoranda from treasurers_module where bin = '" + bin1.GetBin() + "'";
                MemoFrm.m_sQuery += " and tax_year = '" + txtTaxYear.Text + "'";
                MemoFrm.ShowDialog();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            MonitoringClass.Refresh();
        }

        private void frameWithShadow2_Load(object sender, EventArgs e)
        {

        }

        private void btnBilling_Click(object sender, EventArgs e)
        {

        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            MonitoringClass.Return();
        }

        
    }
}