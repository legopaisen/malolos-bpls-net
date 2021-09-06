using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.SearchOwner;

namespace Amellar.Modules.PermitUpdate
{
    public partial class frmTransferApp : Form
    {
        private TransferApp TransferAppClass = null;

        private string m_strSource = string.Empty;
        private string m_strTransaction = string.Empty;
        private string m_sBin = string.Empty;
        private string m_sNewBnsName = string.Empty;
        private string m_sNewOwnCode = string.Empty;
        private string m_sNewLoc = string.Empty;
        private string m_sTaxYear = string.Empty;
        private bool m_bIsApplSave = false;
        private string m_sNewOrgKind = string.Empty;    // RMC 20111004 added permit-update change of orgn kind
        private bool m_bApplyBilling = true;   // RMC 20141125 mods in trailing permit-update transaction
        private DateTime m_dLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
        private string m_sBnsStat = string.Empty;   // RMC 20170822 added transaction log feature for tracking of transactions per bin

        public string Source
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public string BIN
        {
            get { return m_sBin; }
            set { m_sBin = value; }
        }

        public string NewBnsName
        {
            get { return m_sNewBnsName; }
            set { m_sNewBnsName = value; }
        }

        public string NewOwnCode
        {
            get { return m_sNewOwnCode; }
            set { m_sNewOwnCode = value; }
        }

        public string NewLocation
        {
            get { return m_sNewLoc; }
            set { m_sNewLoc = value; }
        }

        public string NewOrgnKind   // RMC 20111004 added permit-update change of orgn kind
        {
            get { return m_sNewOrgKind; }
            set { m_sNewOrgKind = value; }
        }

        public bool ApplSave
        {
            get { return m_bIsApplSave; }
            set { m_bIsApplSave = value; }
        }

        public string TaxYear
        {
            get { return m_sTaxYear; }
            set { m_sTaxYear = value; }
        }

        public bool ApplyBilling    // RMC 20141125 mods in trailing permit-update transaction
        {
            get { return m_bApplyBilling; }
            set { m_bApplyBilling = value; }
        }

        public DateTime LogIn   // RMC 20170822 added transaction log feature for tracking of transactions per bin
        {
            get { return m_dLogIn; }
            set { m_dLogIn = value; }
        }

        public string BnsStat   // RMC 20170822 added transaction log feature for tracking of transactions per bin
        {
            get { return m_sBnsStat; }
            set { m_sBnsStat = value; }
        }

        public frmTransferApp()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
            bin2.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin2.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private void btnOldSearch_Click(object sender, EventArgs e)
        {
            if (btnOldSearch.Text == "Clear")
                ClearPrevious();
            else
            {
            }
        }

        public void ClearPrevious()
        {
            /*bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";*/
            txtOldBnsStat.Text = "";
            txtOldLastName.Text = "";
            txtOldFirstName.Text = "";
            txtOldMI.Text = "";
            txtOldAdd.Text = "";
            txtOldStreet.Text = "";
            txtOldBrgy.Text = "";
            txtOldDistrict.Text = "";
            txtOldCity.Text = "";
            txtOldProv.Text = "";
        }

        public void ClearNew()
        {
            /*bin2.txtTaxYear.Text = "";
            bin2.txtBINSeries.Text = "";*/
            txtNewBnsStat.Text = "";
            txtNewLastName.Text = "";
            txtNewFirstName.Text = "";
            txtNewMI.Text = "";
            txtNewAdd.Text = "";
            txtNewStreet.Text = "";
            txtNewBrgy.Text = "";
            txtNewDistrict.Text = "";
            txtNewCity.Text = "";
            txtNewProv.Text = "";
        }

        private void btnNewSearch_Click(object sender, EventArgs e)
        {
            TransferAppClass.SearchNew();
        }

        private void frmTransferApp_Load(object sender, EventArgs e)
        {
            bin1.txtTaxYear.Text = m_sBin.Substring(7, 4);
            bin1.txtBINSeries.Text = m_sBin.Substring(12, 7);

            bin2.txtTaxYear.Text = m_sBin.Substring(7, 4);
            bin2.txtBINSeries.Text = m_sBin.Substring(12, 7);

            if (m_strSource == "ApplyNewBnsName")
            {
                this.Text = "Change of Business Name";  // RMC 20111004 added permit-update change of orgn kind
                TransferAppClass = new AppNewBnsName(this);
                m_strTransaction = "Change of Business Name";
            }
            else if (m_strSource == "ApplyNewOwnName")
            {
                this.Text = "Transfer of Ownership";    // RMC 20111004 added permit-update change of orgn kind
                TransferAppClass = new AppNewOwnName(this);
                m_strTransaction = "Transfer of Ownership";
            }
            else if (m_strSource == "ApplyNewLocation")
            {
                this.Text = "Change of Location";   // RMC 20111004 added permit-update change of orgn kind
                TransferAppClass = new AppNewLocation(this);
                m_strTransaction = "Change of Location";
            }
            else if (m_strSource == "ApplyNewOrgKind")
            {
                this.Text = "Change of Organization Kind";  // RMC 20111004 added permit-update change of orgn kind
                TransferAppClass = new AppNewOrgKind(this);
                m_strTransaction = "Change of Organization Kind";
            }

            m_bIsApplSave = false;

            TransferAppClass.TransactionName = m_strTransaction;
            TransferAppClass.FormLoad();
        }

        private void lblNewTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TransferAppClass.Save();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Close")
                this.Close();
            else
                TransferAppClass.Close();
        }

        private void dgvOtherInfo_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 3 && btnSave.Text == "Update")
            if (e.ColumnIndex == 3 && (btnSave.Text == "Update" || btnSave.Text == "Save")) // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership 
                dgvOtherInfo.ReadOnly = false;
            else
                dgvOtherInfo.ReadOnly = true;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        
    }
}