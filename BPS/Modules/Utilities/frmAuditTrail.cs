using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using BIN;

namespace Amellar.Modules.Utilities
{
    public partial class frmAuditTrail : Form
    {
        private AuditTrailBase TrailClass = null;
        private string m_strSource = string.Empty;
        private DateTime m_dtDateFrom;
        private DateTime m_dtDateTo;
        
        public frmAuditTrail()
        {
            InitializeComponent();
            
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private void frmAuditTrail_Load(object sender, EventArgs e)
        {
            TrailClass = new AuditTrailBase(this);

            chkBussRec.Checked = true;
        }

        private void chkBussRec_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.chkBussRec.CheckState.ToString() == "Checked")
            {
                m_strSource = "Buss Rec";
                this.chkUser.Checked = false;
                this.chkModule.Checked = false;
                this.chkUserTrans.Checked = false;
                this.FormLoad();
            }
        }

        private void FormLoad()
        {
            if (m_strSource == "Buss Rec")
                TrailClass = new AuditTrailRecord(this);
            else if (m_strSource == "User")
                TrailClass = new AuditTrailUser(this);
            else if (m_strSource == "Module")
                TrailClass = new AuditTrailModule(this);
            else if (m_strSource == "UserTrans")
                TrailClass = new AuditTrailUserTrans(this);

            this.TrailClass.FormLoad();
        }

        private void chkUser_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.chkUser.CheckState.ToString() == "Checked")
            {
                m_strSource = "User";
                this.chkBussRec.Checked = false;
                this.chkModule.Checked = false;
                this.chkUserTrans.Checked = false;
                this.FormLoad();
            }
        }

        private void chkModule_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.chkModule.CheckState.ToString() == "Checked")
            {
                m_strSource = "Module";
                this.chkBussRec.Checked = false;
                this.chkUser.Checked = false;
                this.cmbUser.Enabled = true;
                this.chkUserTrans.Checked = false;
                this.FormLoad();
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            if (dtpFrom.Value > dtpTo.Value)
            {
                MessageBox.Show("Invalid date", "Audit Trail", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtpFrom.Value = m_dtDateFrom;
                return;
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTo.Value < dtpFrom.Value)
            {
                MessageBox.Show("Invalid date", "Audit Trail", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtpTo.Value = m_dtDateTo;
                return;
            }
        }

        private void dtpFrom_Enter(object sender, EventArgs e)
        {
            m_dtDateFrom = dtpFrom.Value;
        }

        private void dtpTo_Enter(object sender, EventArgs e)
        {
            m_dtDateTo = dtpTo.Value;
        }

        private void btnSearchBIN_Click(object sender, EventArgs e)
        {
            TrailClass.SearchBin();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            TrailClass.Generate();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbUser_SelectedValueChanged(object sender, EventArgs e)
        {
            TrailClass.UserChange();
        }

        private void cmbUser_Leave(object sender, EventArgs e)
        {
            TrailClass.UserChange();
        }

        private void cmbModule_SelectedValueChanged(object sender, EventArgs e)
        {
            TrailClass.ModuleChange();
        }

        private void chkModule_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkUser_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void chkUserTrans_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.chkUserTrans.CheckState.ToString() == "Checked")
            {
                m_strSource = "UserTrans";
                this.chkBussRec.Checked = false;
                this.chkModule.Checked = false;
                this.chkUser.Checked = false;
                this.FormLoad();
            }
        }

        private void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }






        
    }
}