
// RMC 20120329 Modifications in Notice of violation

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmOfficialTagging : Form
    {
        private OfficialTagging TaggingClass = null;
        protected string m_sBin = string.Empty;
        protected string m_sWatcher = string.Empty;
        protected string m_sIns = string.Empty;
        protected string m_sISNum = string.Empty;
        protected string m_sSource = string.Empty;
        string sIsNo = string.Empty;
        public string m_sBnsNm = string.Empty;  // GDE 20120416 test
        public string m_sBnsType = string.Empty; // MCR 20140515

        public string Watcher
        {
            get { return m_sWatcher; }
            set { m_sWatcher = value; }
        }

        public string Inspector
        {
            get { return m_sIns; }
            set { m_sIns = value; }
        }

        public string Bin
        {
            get { return m_sBin; }
            set { m_sBin = value; }
        }

        public string ISNum
        {
            get { return m_sISNum; }
            set { m_sISNum = value; }
        }

        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }

        public string BnsNm // GDE 20120416 test
        {
            get { return m_sBnsNm; }
            set { m_sBnsNm = value; }
        }

        public frmOfficialTagging()
        {
            InitializeComponent();
        }

        private void frmOfficialTagging_Load(object sender, EventArgs e)
        {
            if (this.Source == "DEFICIENT")
            {
                TaggingClass = new OfficialTaggingDeficient(this);
                this.Text = "Official Business";
            }
            // RMC 20120222 added printing of notice for un-official business in business mapping (s)
            else if (this.Source == "Business Mapping")
            {
                TaggingClass = new OfficialTaggingBM(this);
                this.Text = "Business Mapping";
            }
            // RMC 20120222 added printing of notice for un-official business in business mapping (e)
            else
            {
                TaggingClass = new OfficialTaggingUnofficial(this);
                this.Text = "Unofficial Business";
            }

            TaggingClass.Watcher = m_sWatcher;
            TaggingClass.Inspector = m_sIns;
            TaggingClass.Bin = m_sBin;
            TaggingClass.ISNum = m_sISNum;
            
            TaggingClass.FormLoad();
            
        }

        private void chkWNotice_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkWClosure.Checked)
                btnTagClosure.Enabled = true;
            else
                btnTagClosure.Enabled = false;

            TaggingClass.CheckWNotice();
        }

        private void chkWONotice_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkWClosure.Checked)
                btnTagClosure.Enabled = true;
            else
                btnTagClosure.Enabled = false;

            TaggingClass.CheckWONotice();

        }

        private void chkWTag_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkWClosure.Checked)
                btnTagClosure.Enabled = true;
            else
                btnTagClosure.Enabled = false;

            TaggingClass.CheckWTag();
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            TaggingClass.ButtonIssueNotice();
        }

        private void dgvBnsInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        

        private void btnSend_Click(object sender, EventArgs e)
        {
            TaggingClass.ButtonSendNotice();
        }

        private void dtnDelete_Click(object sender, EventArgs e)
        {
            TaggingClass.ButtonDeleteNotice();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            TaggingClass.ButtonGenerate();
        }

        private void dgvBnsInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                TaggingClass.CellClickBnsInfo(e.RowIndex);
                sIsNo = dgvBnsInfo.Rows[e.RowIndex].Cells[0].Value.ToString(); // GDE 20120430
            }
        }

        private void chkWNotice_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            TaggingClass.ButtonSearch();
        }

        private void chkW2ndNotice_CheckStateChanged(object sender, EventArgs e)
        {
            TaggingClass.CheckW2ndNotice(); // RMC 20120329 Modifications in Notice of violation
        }

        private void chkWClosure_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkWClosure.Checked)
                btnTagClosure.Enabled = true;
            else
                btnTagClosure.Enabled = false;

            if (chkWClosure.CheckState.ToString() == "Checked")
            {
                chkWONotice.Checked = false;
                chkWTag.Checked = false;
                chkWNotice.Checked = false;
            }

            TaggingClass.CheckWClosureNotice(); // RMC 20120329 Modifications in Notice of violation
            
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            
            TaggingClass.ButtonPrintList(); // RMC 20120329 Modifications in Notice of violation
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            TaggingClass.ButtonClear(); // RMC 20120417 Modified Final notice format
        }

        private void dgvBnsInfo_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            TaggingClass.CellRowEnter(e.RowIndex);
        }

        private void btnTagClosure_Click(object sender, EventArgs e)
        {
            if (!AppSettingsManager.bIsTagged(sIsNo.Trim()))
            {
                // GDE temporary for tagging (s){
                frmMemo fMemo = new frmMemo();
                fMemo.ShowDialog();
                string sMemo = string.Empty;
                sMemo = fMemo.txtMemo.Text.Trim();
                DateTime dDate = new DateTime();
                dDate = DateTime.Now;

                OracleResultSet pSet = new OracleResultSet();
                if (this.Text == "Official Business")
                    pSet.Query = "insert into official_closure_tagging values (:1,:2,:3,:4)";
                else
                    pSet.Query = "insert into norec_closure_tagging values (:1,:2,:3,:4)";
                pSet.AddParameter(":1", sIsNo);
                pSet.AddParameter(":2", dDate);
                pSet.AddParameter(":3", AppSettingsManager.SystemUser.UserCode);
                pSet.AddParameter(":4", sMemo);
                if (pSet.ExecuteNonQuery() != 0)
                { }
                pSet.Close();

                MessageBox.Show("Tagging Saved.");
                if (this.Text == "Official Business")
                {
                    string sOwnCode = string.Empty;
                    sOwnCode = AppSettingsManager.GetBnsOwnCode(sIsNo);

                    pSet.Query = "insert into tagged_bns values (:1)";
                    pSet.AddParameter(":1", sOwnCode);
                    if (pSet.ExecuteNonQuery() == 0)
                    { }
                    pSet.Close();
                }
                TaggingClass.CheckWClosureNotice();
                // GDE temporary for tagging (e)}
            }
            else
            {
                MessageBox.Show("Business already been tagged for CDO.","CDO Tagging",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            
        }

        private void chkWClosure_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkWONotice_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkWTag_CheckedChanged(object sender, EventArgs e)
        {

        }

        

    }
}