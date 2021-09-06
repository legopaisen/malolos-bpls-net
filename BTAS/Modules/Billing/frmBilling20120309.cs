//#######################################
// Modifications:   
// RMC 20120113 modified comparing of taxes if REN only
// RMC 20120110 disable locking of gross higher than 300,000 (by treas)
// RMC 20120109 transferred record-locking in Billing
// RMC 20120109 corrected log-out in Billing-search
// RMC 20120109 insert bin conflict validation in Billing search
// RMC 20111227 added Gross monitoring module for gross >= 200000
// RMC 20111220 added object locking in billing
// RMC 20111220 added updating of business mapped in Billing-autoapplication
// RMC 20111219 added validation of business mapping deficiencies in Billing
// ALJ 20110907 re-assessment validation                                          
// RMC 20110414 added set in BIN
// ALJ 20110216 dgvTaxFees.RefreshEdit()     
// RMC 20110826 added validation if billing exists
// GDE 20110801 add SeachBusiness Module under SearchClick()            
//#######################################

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using BIN;
using Amellar.Common.TextValidation;
using Amellar.BPLS.TreasurersModule;
using Amellar.Common.SOA;
using Amellar.Common.SearchBusiness;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;

namespace Amellar.BPLS.Billing
{
    /// <summary>
    /// ALJ 20090701
    /// Billing form
    /// </summary>

    public partial class frmBilling : Form
    {
        frmSOA fSOA;
        private string m_sBIN;
        private bool m_bIsMain = true;
        private bool m_bBillFlag = false;
        private bool m_bIsReturn = false;
        private string m_sSource;
        private MoneyTextValidator m_objNumberValidator;
        private SimpleTextValidator m_objNumberValidator2;
        Billing BillingClass = null;

        public frmBilling()
        {
            InitializeComponent();
            dgvTaxFees.Rows.Clear();
            dgvAddlInfo.Rows.Clear();
            bin.GetLGUCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.LGUCode);
            bin.GetDistCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.DistCode);
            m_objNumberValidator = new MoneyTextValidator();
            m_objNumberValidator2 = new SimpleTextValidator();
            m_objNumberValidator2.SetIntCharacterSet();


        }

        public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; } // RMC 20110414 added set in BIN
        }
        public string SourceClass
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }

        public bool MainBusiness
        {
            get { return m_bIsMain; }
            set { m_bIsMain = value; }
        }

        public bool BillFlag
        {
            get { return m_bBillFlag; }
            set { m_bBillFlag = value; }
        }

        public string TaxYear
        {
            get { return txtTaxYear.Text; }
            set { txtTaxYear.Text = value; }
        }

        public TextBox TaxYearTextBox
        {
            get { return this.txtTaxYear; }
            set { this.txtTaxYear = value; }
        }

        public string Quarter
        {
            get { return txtQtr.Text; }
            set { txtQtr.Text = value; }
        }

        public TextBox QuarterTextBox
        {
            get { return this.txtQtr; }
            set { this.txtQtr = value; }
        }

        public string BusinessName
        {
            get { return txtBusinessName.Text; }
            set { txtBusinessName.Text = value; }
        }

        public string OwnersName
        {
            get { return txtOwnersName.Text; }
            set { txtOwnersName.Text = value; }
        }

        public string Status
        {
            get { return txtStatus.Text; }
            set { txtStatus.Text = value; }
        }

        public string BusinessCode
        {
            get { return txtBnsCode.Text; }
            set { txtBnsCode.Text = value; }
        }

        public string Capital
        {
            get { return txtCapital.Text; }
            set { txtCapital.Text = value; }
        }

        public TextBox CapitalTextBox
        {
            get { return this.txtCapital; }
            set { this.txtCapital = value; }
        }

        public string Gross
        {
            get { return txtGross.Text; }
            set { txtGross.Text = value; }
        }

        public TextBox GrossTextBox
        {
            get { return this.txtGross; }
            set { this.txtGross = value; }
        }

        public string PreGross
        {
            get { return txtPreGross.Text; }
            set { txtPreGross.Text = value; }
        }

        public TextBox PreGrossTextBox
        {
            get { return this.txtPreGross; }
            set { this.txtPreGross = value; }
        }

        public string AdjGross
        {
            get { return txtAdjGross.Text; }
            set { txtAdjGross.Text = value; }
        }

        public TextBox AdjGrossTextBox
        {
            get { return this.txtAdjGross; }
            set { this.txtAdjGross = value; }
        }

        public string VATGross
        {
            get { return txtVATGross.Text; }
            set { txtVATGross.Text = value; }
        }

        public TextBox VATGrossTextBox
        {
            get { return this.txtVATGross; }
            set { this.txtVATGross = value; }
        }

        public string Total
        {
            get { return txtTotal.Text; }
            set { txtTotal.Text = value; }
        }

        public ComboBox BusinessTypes
        {
            get { return this.cmbBnsType; }
            set { this.cmbBnsType = value; }
        }

        public DataGridView AdditionalInformation
        {
            get { return this.dgvAddlInfo; }
            set { this.dgvAddlInfo = value; }
        }

        public DataGridView TaxAndFees
        {
            get { return this.dgvTaxFees; }
            set { this.dgvTaxFees = value; }
        }

        // RMC 20110414 (S)
        public Button SearchButton
        {
            get { return this.btnSearch; }
            set { this.btnSearch = value; }
        }
        // RMC 20110414 (E)

        private void frmBilling_Load(object sender, EventArgs e)
        {
            if (this.SourceClass == "Billing")
            {
                BillingClass = new Billing(this);

                // RMC 20110414
                if (m_sBIN != "" && m_sBIN != null)
                {
                    bin.txtTaxYear.Text = m_sBIN.Substring(7, 4);
                    bin.txtBINSeries.Text = m_sBIN.Substring(12, 7);
                    SearchClick();
                    txtTaxYear.ReadOnly = true;
                    bin.txtTaxYear.ReadOnly = true;
                    bin.txtBINSeries.ReadOnly = true;
                    btnSearch.Enabled = false;
                }
                // RMC 20110414
            }
            else if (this.SourceClass == "RevExam")
            {
                BillingClass = new RevExam(this);
                //this.lblGross.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblGross.Text = "New-Gross";
                lblGross.ForeColor = System.Drawing.Color.Red;
                txtAdjGross.Visible = true;
                txtPreGross.Visible = false;
            }
            else if (this.SourceClass == "CancelBilling")   // RMC 20110807 created CancelBilling module (s)
            {
                BillingClass = new CancelBilling(this);
                btnSave.Text = "Cancel Billing";

            }
            else if (this.SourceClass == "PermitUpdate") // CJC 2011
            {
                BillingClass = new PermitUpdate(this);

            }

            // RMC 20110807 created CancelBilling module (e)
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.blnIsConflict(bin.GetBin()))
            {
                MessageBox.Show("Conflict Bin");
                ClearControls();    // RMC 20120109 insert bin conflict validation in Billing search
                btnSearch.Text = "&Search";  // RMC 20120109 insert bin conflict validation in Billing search
                return;
            }




            SearchClick();  // RMC 20110414      
            // transferred all codes to SearchClick()            
        }

        private void ClearControls()
        {
            // RMC 20111220 added object locking in billing (s)
            if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "DELETE", "COL"))
            {
            }
            // RMC 20111220 added object locking in billing (e)

            m_sBIN = string.Empty;
            bin.GetTaxYear = string.Empty;
            bin.GetBINSeries = string.Empty;
            txtTaxYear.Text = string.Empty;
            txtTaxYear.ReadOnly = true;
            txtQtr.Text = string.Empty;
            txtQtr.ReadOnly = true;
            txtBusinessName.Text = string.Empty;
            txtOwnersName.Text = string.Empty;
            cmbBnsType.ValueMember = null; ;
            cmbBnsType.DataSource = null;
            txtBnsCode.Text = string.Empty;
            txtStatus.Text = string.Empty;
            dgvTaxFees.Rows.Clear();
            txtCapital.Text = string.Empty;
            txtCapital.ReadOnly = true;
            txtGross.Text = string.Empty;
            txtGross.ReadOnly = true;
            txtAdjGross.Text = string.Empty;
            txtAdjGross.ReadOnly = true;
            txtPreGross.Text = string.Empty;
            txtPreGross.ReadOnly = true;
            txtVATGross.Text = string.Empty;
            txtVATGross.ReadOnly = true;
            dgvAddlInfo.DataSource = null;
            btnEditAddlInfo.Enabled = false;
            btnCancelAddlInfo.Enabled = false;
            btnRetrieveBilling.Enabled = false;
            btnSave.Enabled = false;
            btnViewSOA.Enabled = false;
            btnBusinessAgent.Enabled = false;
        }

        private void BillingRetrieve()
        {
            // RMC 20110826 added validation if billing exists (s)
            if (!BillingClass.ValidBilling())
                return;
            // RMC 20110826 added validation if billing exists (e)

            // Billing BillingClass = new Billing(this);  
            BillingClass.ReturnValues();

            if (IsBillingAccessOK()) { }; // ALJ 20110907 re-assessment validation     

            BillingClass.CancelBillUpdateList();  // RMC 20110807 created CancelBilling module
        }

        private void cmbBnsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBnsType.ValueMember != "")
            {
                btnEditAddlInfo.Text = "&Edit";
                btnCancelAddlInfo.Enabled = false;
                txtBnsCode.Text = cmbBnsType.SelectedValue.ToString();
                if (cmbBnsType.SelectedIndex == 0) // Note: add filter. not applicable if Permit update - ADDL 
                    m_bIsMain = true;
                else
                    m_bIsMain = false;
                BillingClass.ReLoadAddlInfo();
                BillingClass.RetrieveBilling();
            }



        }

        private void btnRetrieveBilling_Click(object sender, EventArgs e)
        {
            BillingClass.RetrieveBilling();
        }


        private void dgvTaxFees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {

                bool blnIsChecked = false;
                string sFeesCode, sFeesDesc, sFeesType;
                double fAmount = 0;
                int iFRIndex;
                // get the current checkbox status
                blnIsChecked = (bool)dgvTaxFees.Rows[e.RowIndex].Cells[0].Value;

                sFeesType = dgvTaxFees.Rows[e.RowIndex].Cells[4].Value.ToString();
                sFeesCode = dgvTaxFees.Rows[e.RowIndex].Cells[3].Value.ToString();
                sFeesDesc = dgvTaxFees.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (!blnIsChecked) // compute and checked
                {
                    dgvTaxFees.Rows[e.RowIndex].Cells[0].Value = true;
                    if (sFeesType == "TF") // Tax And Fees
                    {
                        if (sFeesCode == "00") // business tax
                        {
                            fAmount = BillingClass.BillBtax();
                        }
                        else // Fees
                        {
                            fAmount = BillingClass.BillFees(sFeesCode, sFeesDesc, sFeesType);
                        }
                    }
                    else // Additional Charges and Fire Tax
                    {
                        fAmount = BillingClass.BillFees(sFeesCode, sFeesDesc, sFeesType);
                    }

                    dgvTaxFees.Rows[e.RowIndex].Cells[2].Value = string.Format("{0:#,##0.00}", fAmount);

                    if (fAmount == 0)
                    {
                        dgvTaxFees.CancelEdit();
                        dgvTaxFees.Rows[e.RowIndex].Cells[0].Value = false;
                        dgvTaxFees.RefreshEdit(); // ALJ 20110216 dgvTaxFees.RefreshEdit()
                    }

                }
                else // un-compute and unchecked
                {
                    dgvTaxFees.Rows[e.RowIndex].Cells[2].Value = "0.00";
                    dgvTaxFees.Rows[e.RowIndex].Cells[0].Value = false;
                }

                BillingClass.UpdateBillTable(fAmount, sFeesType, sFeesCode);

                if (sFeesType == "AF" && sFeesCode == "02") // fire tax billing
                {
                    // skip when fire tax row is being click - no need to update/refresh fire tax
                }
                else
                {
                    iFRIndex = dgvTaxFees.Rows.Count - 1; // index of fire tax (last row)
                    fAmount = BillingClass.UpdateFireTax();
                    if (fAmount > 0) // ALJ 20100115
                        dgvTaxFees.Rows[iFRIndex].Cells[0].Value = true;
                    else
                        dgvTaxFees.Rows[iFRIndex].Cells[0].Value = false;
                    dgvTaxFees.Rows[iFRIndex].Cells[2].Value = string.Format("{0:#,##0.00}", fAmount);
                    BillingClass.UpdateBillTable(fAmount, "AF", "02"); // update fire


                }
                this.txtTotal.Text = string.Format("{0:#,##0.00}", BillingClass.Total());
            }
        }

        private void dgvTaxFees_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dgvTaxFees_CellContentClick(sender, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BillingClass.Save();
            if (BillingClass.IsBillCreated)
            {
                dgvTaxFees.Rows.Clear();
                txtTotal.Text = "0";
                string sUnBilledBnsType = "";

                // (s) ALJ 20110907 re-assessment validation
                if (txtTaxYear.Text.ToString() == BillingClass.CurrentYear)
                    btnViewSOA.Enabled = true;
                else
                    btnViewSOA.Enabled = false;
                // (e) ALJ 20110907 re-assessment validation

                if (cmbBnsType.Items.Count > 1)
                    sUnBilledBnsType = BillingClass.UnBilledBusinessType();
                if (sUnBilledBnsType == "")
                {
                    // Compare Taxses
                    //if (txtStatus.Text.ToString() == "REN") // ALJ 20110907 re-assessment validation - put remark, to cater any status  
                    if (GetPrevStat() == "REN")  // RMC 20120113 modified comparing of taxes if REN only
                    {
                        BillingClass.CompareTaxes();
                    }
                }
                // (s) NEW QTR DEC
                if (txtStatus.Text.ToString() == "NEW" && BillingClass.IsQtrly == "Y" && txtQtr.Text.ToString() != "4")
                {
                    if (sUnBilledBnsType == "")
                        BillingRetrieve();
                }
                // (e) NEW QTR DEC
                else
                {
                    if (txtTaxYear.Text.ToString() != BillingClass.CurrentYear)
                    {
                        //string sUnBilledBnsType = "";
                        //if (cmbBnsType.Items.Count > 1)
                        //    sUnBilledBnsType = BillingClass.UnBilledBusinessType();
                        if (sUnBilledBnsType == "")
                            BillingRetrieve();
                        //else
                        // MessageBox.Show("Save billing first for " + sUnBilledBnsType, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                //                btnViewSOA.Enabled = true; // GDE 20110809 as per Mam Rachel

                // RMC 20111220 added updating of business mapped in Billing-autoapplication (s)
                if (AppSettingsManager.ValidateMapping(bin.GetBin()))
                {
                    OracleResultSet pCmd = new OracleResultSet();

                    string sTmpDate = "";
                    sTmpDate = Convert.ToString(AppSettingsManager.GetCurrentDate());

                    pCmd.Query = "update btm_update set def_settled = 'Y', ";
                    pCmd.Query += "settled_by = '" + AppSettingsManager.SystemUser.UserCode + "', ";
                    pCmd.Query += "settled_dt = to_date('" + sTmpDate + "', 'MM/dd/yyyy hh:mi:ss am') ";
                    pCmd.Query += "where bin = '" + bin.GetBin() + "' and trim(def_settled) is null";
                    pCmd.AddParameter(":1", AppSettingsManager.GetCurrentDate());
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }
                }
                // RMC 20111220 added updating of business mapped in Billing-autoapplication (e)


            }
        }

        private void btnEditAddlInfo_Click(object sender, EventArgs e)
        {
            if (IsBillingAccessOK()) // ALJ 20110907 re-assessment validation
            {

                if (btnEditAddlInfo.Text.ToString() == "&Edit")
                {
                    btnEditAddlInfo.Text = "&Apply";
                    btnCancelAddlInfo.Enabled = true;
                    txtTaxYear.ReadOnly = true;
                    dgvTaxFees.Enabled = false;
                    btnRetrieveBilling.Enabled = false;
                    btnSave.Enabled = false;
                    if (txtStatus.Text == "NEW")
                    {
                        txtCapital.ReadOnly = false;
                        txtGross.ReadOnly = true;
                    }
                    else
                    {
                        txtGross.ReadOnly = false;
                        txtCapital.ReadOnly = true;
                    }

                    BillingClass.EditAddlInfo(); // GDE 20110809 disabled as per ma'am rachel

                }
                else // apply changes
                {
                    btnEditAddlInfo.Text = "&Edit";
                    btnCancelAddlInfo.Enabled = false;
                    txtTaxYear.ReadOnly = false;
                    dgvTaxFees.Enabled = true;
                    btnRetrieveBilling.Enabled = true;
                    btnSave.Enabled = true;
                    BillingClass.ApplyAddlInfo();
                    BillingClass.RetrieveBilling();
                }
            } // ALJ 20110907 re-assessment validation

        }

        private void btnCancelAddlInfo_Click(object sender, EventArgs e)
        {
            double dTotal;
            btnEditAddlInfo.Text = "&Edit";
            btnCancelAddlInfo.Enabled = false;
            txtTaxYear.ReadOnly = false;
            dgvTaxFees.Enabled = true;
            btnRetrieveBilling.Enabled = true;
            double.TryParse(txtTotal.Text.ToString(), out dTotal);
            if (dTotal > 0)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
            BillingClass.ReLoadAddlInfo();
        }
        /// <summary>
        /// ALJ 20100128 Function to enable and disable billing form controls
        /// p_blnEnabled is false - initial controls enabled value based on design 
        /// </summary>
        /// <param name="p_blnEnabled"></param>
        public void EnabledControls(bool p_blnEnabled)
        {
            btnEditAddlInfo.Enabled = p_blnEnabled;
            btnRetrieveBilling.Enabled = p_blnEnabled;
            btnBusinessAgent.Enabled = p_blnEnabled;
            if (!p_blnEnabled) // if set to false
            {
                btnSave.Enabled = p_blnEnabled;
                btnViewSOA.Enabled = p_blnEnabled;
            }
        }

        private void ValidateTaxYearQtr()
        {

            if (txtTaxYear.Text.ToString().Length == 4 && BillingClass.IsInitialLoad == false)
            {
                btnEditAddlInfo.Enabled = true; // ALJ 20110321
                btnRetrieveBilling.Enabled = true;
                int iTaxYear, iBaseTaxYear, iCurrentYear;
                int.TryParse(txtTaxYear.Text.ToString(), out iTaxYear);
                int.TryParse(BillingClass.BaseTaxYear, out iBaseTaxYear);
                int.TryParse(BillingClass.CurrentYear, out iCurrentYear);
                if (iTaxYear < iBaseTaxYear || iTaxYear > iCurrentYear)
                {
                    btnEditAddlInfo.Enabled = false; // ALJ 20110321
                    btnRetrieveBilling.Enabled = false;
                    MessageBox.Show("Invalid Tax Year! Nothing to be Billed.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTaxYear.Focus();
                    return;
                }
                if (iTaxYear > iBaseTaxYear && BillingClass.IsRevExam == 0) // check skip billing (tax year)
                {
                    // check if prev year has been billed.
                    string sTaxYear, sQtr;
                    sQtr = txtQtr.Text.ToString(); // temporary // add validation if qtr dec
                    sTaxYear = string.Format("{0:0000}", iTaxYear - 1);
                    if (!BillingClass.HasDues(m_sBIN, txtStatus.Text.ToString(), txtBnsCode.Text.ToString(), sTaxYear, sQtr))
                    {
                        btnEditAddlInfo.Enabled = false; // ALJ 20110321
                        btnRetrieveBilling.Enabled = false;
                        MessageBox.Show("Invalid! Cannot skip dues.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTaxYear.Focus();
                        return;
                    }
                }
                // Note: pending checking skip billing for qtrs
                BillingClass.ReLoadAddlInfo();
            }
            else
            {
                btnEditAddlInfo.Enabled = false; // ALJ 20110321
                btnRetrieveBilling.Enabled = false;
                dgvTaxFees.Rows.Clear();
                txtTotal.Text = "0";
            }
        }

        private void ValidateQtrDec()
        {

            if (txtStatus.Text.ToString() == "NEW")
            {
                if (txtQtr.Text.ToString().Length == 1 && BillingClass.IsInitialLoad == false)
                {

                    btnEditAddlInfo.Enabled = true; // ALJ 20110321
                    btnRetrieveBilling.Enabled = true;
                    int iQtr, iBaseDueQtr, iMaxDueQtr;
                    int.TryParse(txtQtr.Text.ToString(), out iQtr);
                    int.TryParse(BillingClass.BaseDueQtr, out iBaseDueQtr);
                    int.TryParse(BillingClass.MaxDueQtr, out iMaxDueQtr);
                    if (iQtr < iBaseDueQtr || iQtr > iMaxDueQtr)
                    {
                        btnEditAddlInfo.Enabled = false; // ALJ 20110321
                        btnRetrieveBilling.Enabled = false;
                        MessageBox.Show("Invalid quarter! Nothing to be Billed.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtQtr.Focus();
                        return;
                    }
                    if (iQtr > iBaseDueQtr && BillingClass.IsRevExam == 0) // check skip billing (Qtr)
                    {
                        // check if prev year has been billed.
                        string sTaxYear, sQtr;
                        sTaxYear = txtTaxYear.Text.ToString(); // temporary // add validation if qtr dec
                        sQtr = string.Format("{0:0}", iQtr - 1);
                        if (!BillingClass.HasDues(m_sBIN, txtStatus.Text.ToString(), txtBnsCode.Text.ToString(), sTaxYear, sQtr))
                        {
                            btnEditAddlInfo.Enabled = false; // ALJ 20110321
                            btnRetrieveBilling.Enabled = false;
                            MessageBox.Show("Invalid! Cannot skip dues.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtQtr.Focus();
                            return;
                        }
                    }
                    // Note: pending checking skip billing for qtrs
                    BillingClass.ReLoadAddlInfo();
                }
                else
                {
                    btnEditAddlInfo.Enabled = false; // ALJ 20110321
                    btnRetrieveBilling.Enabled = false;
                    dgvTaxFees.Rows.Clear();
                    txtTotal.Text = "0";
                }
            }
        }

        private void ValidateTaxYearQtr2()
        {
            /*

            if (m_bIsReturn)
            {
                txtTaxYear.Focus();
                m_bIsReturn = false;
                return;
            }
            if (txtTaxYear.Text.ToString().Length == 4 && BillingClass.IsInitialLoad == false)
            {
                btnRetrieveBilling.Enabled = true;
                int iTaxYear, iBaseTaxYear, iCurrentYear;
                int.TryParse(txtTaxYear.Text.ToString(), out iTaxYear);
                int.TryParse(BillingClass.BaseTaxYear, out iBaseTaxYear);
                int.TryParse(BillingClass.CurrentYear, out iCurrentYear);
                if (iTaxYear < iBaseTaxYear || iTaxYear > iCurrentYear)
                {
                    btnRetrieveBilling.Enabled = false;
                    MessageBox.Show("Invalid Tax Year! Nothing to be Billed.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    m_bIsReturn = true;
                    txtTaxYear.Undo(); // this line will triger txtTaxYear_TextChanged, m_bIsReturn should be set to true;
                    txtTaxYear.Focus();
                    if (txtTaxYear.Text.ToString().Length != 4)
                        btnRetrieveBilling.Enabled = false;
                    m_bIsReturn = false; // set to false before exit/return
                    return;
                }
                if (iTaxYear > iBaseTaxYear) // check skip billing (tax year)
                {
                    // check if prev year has been billed.
                    string sTaxYear, sQtr;
                    sQtr = txtQtr.Text.ToString(); // temporary // add validation if qtr dec
                    sTaxYear = string.Format("{0:0000}", iTaxYear - 1);
                    if (!BillingClass.HasDues(m_sBIN, txtBnsCode.Text.ToString(), sTaxYear, sQtr))
                    {
                        btnRetrieveBilling.Enabled = false;
                        MessageBox.Show("Invalid! Cannot skip dues.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        m_bIsReturn = true; 
                        txtTaxYear.Undo(); // this line will triger txtTaxYear_TextChanged, m_bIsReturn should be set to true;  
                        txtTaxYear.Focus();
                        if (txtTaxYear.Text.ToString().Length != 4)
                            btnRetrieveBilling.Enabled = false;
                        m_bIsReturn = false; // set to false before exit/return
                        return;
                    }
                }
                // Note: pending checking skip billing for qtrs
                BillingClass.ReLoadAddlInfo();
            }
            else
            {
                btnRetrieveBilling.Enabled = false;
            }
        */
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            double dTotal;
            double.TryParse(txtTotal.Text.ToString(), out dTotal);
            if (dTotal > 0)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }

        private void txtTaxYear_TextChanged(object sender, EventArgs e)
        {
            ValidateTaxYearQtr();
        }

        private void txtTaxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            m_objNumberValidator2.HandleKeyPressEvent(sender, e, 4);
        }

        private void txtTaxYear_Leave(object sender, EventArgs e)
        {
            m_objNumberValidator2.HandleIntFormat(sender, "{0:0000}", 4);
            ValidateTaxYearQtr();
        }

        private void txtQtr_TextChanged(object sender, EventArgs e)
        {
            ValidateQtrDec(); // ALJ 20110309
        }

        private void txtQtr_KeyPress(object sender, KeyPressEventArgs e)
        {
            m_objNumberValidator2.HandleKeyPressEvent(sender, e, 1);
        }

        private void txtQtr_Leave(object sender, EventArgs e)
        {
            m_objNumberValidator2.HandleIntFormat(sender, "{0:0}", 1);
            ValidateQtrDec(); // ALJ 20110309        
        }
        private void txtCapital_KeyPress(object sender, KeyPressEventArgs e)
        {
            m_objNumberValidator.HandleKeyPressEvent(sender, e);
        }

        private void txtCapital_Leave(object sender, EventArgs e)
        {
            m_objNumberValidator.HandleFormat(sender);
        }

        private void txtGross_KeyPress(object sender, KeyPressEventArgs e)
        {
            m_objNumberValidator.HandleKeyPressEvent(sender, e);
        }

        private void txtGross_Leave(object sender, EventArgs e)
        {
            m_objNumberValidator.HandleFormat(sender);
        }

        private void txtAdjGross_KeyPress(object sender, KeyPressEventArgs e)
        {
            m_objNumberValidator.HandleKeyPressEvent(sender, e);
        }

        private void txtAdjGross_Leave(object sender, EventArgs e)
        {
            m_objNumberValidator.HandleFormat(sender);
        }

        private void txtPreGross_KeyPress(object sender, KeyPressEventArgs e)
        {
            m_objNumberValidator.HandleKeyPressEvent(sender, e);
        }

        private void txtPreGross_Leave(object sender, EventArgs e)
        {
            m_objNumberValidator.HandleFormat(sender);
        }

        private void txtVATGross_KeyPress(object sender, KeyPressEventArgs e)
        {
            m_objNumberValidator.HandleKeyPressEvent(sender, e);
        }

        private void txtVATGross_Leave(object sender, EventArgs e)
        {
            m_objNumberValidator.HandleFormat(sender);
        }

        private void dgvAddlInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int iRows;
            iRows = dgvAddlInfo.Rows.Count;
            if (iRows > 0)
                if (e.ColumnIndex == 3)
                {
                    string sData, sType;
                    sType = dgvAddlInfo.Rows[e.RowIndex].Cells[2].Value.ToString();
                    sData = dgvAddlInfo.Rows[e.RowIndex].Cells[3].Value.ToString();
                    sType = sType.Trim();
                    sData = sData.Trim();
                    if (sData != "")
                    {
                        if (sType[0] == 'Q')
                        {
                            int iValid;
                            if (!int.TryParse(sData, out iValid))
                            {
                                MessageBox.Show(sData + " is invalid entry.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dgvAddlInfo.Rows[e.RowIndex].Cells[3].Value = "";
                            }
                        }
                        else
                        {
                            double dValid;
                            if (!double.TryParse(sData, out dValid))
                            {
                                MessageBox.Show(sData + " is invalid entry.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dgvAddlInfo.Rows[e.RowIndex].Cells[3].Value = "";
                            }
                        }
                    }
                }
        }

        private void frmBilling_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (!BillingClass.IsBillOK)
            //{
            //    MessageBox.Show("Save billing first.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}
        }

        private void frmBilling_FormClosing(object sender, FormClosingEventArgs e)
        {
            string sUnBilledBnsType;
            sUnBilledBnsType = BillingClass.UnBilledBusinessType();
            if (sUnBilledBnsType != "")
            {
                if (BillingClass.BnsIsExempted(AppSettingsManager.GetBnsCodeByDesc(sUnBilledBnsType)))    // RMC 20120120
                {
                    e.Cancel = false;
                }
                else
                {
                    MessageBox.Show("Save billing first for " + sUnBilledBnsType, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    e.Cancel = true;
                }
            }
            else
                e.Cancel = false;

            /*
            if (!BillingClass.IsBillOK)
            {
                MessageBox.Show("Save billing first.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.Cancel = true;
            }
            else
                e.Cancel = false;
             */
        }

        private void txtTaxYear_Enter(object sender, EventArgs e)
        {
            if (CheckUnBilled()) { } // ALJ 20110321
            /*
            string sUnBilledBnsType;
            sUnBilledBnsType = BillingClass.UnBilledBusinessType();
            if (sUnBilledBnsType != "")
            {
                MessageBox.Show("Save billing first for " + sUnBilledBnsType, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cmbBnsType.Focus();
            }
             */

        }

        private void txtQtr_Enter(object sender, EventArgs e)
        {
            if (CheckUnBilled()) { } // ALJ 20110321
            /*
            string sUnBilledBnsType;
            sUnBilledBnsType = BillingClass.UnBilledBusinessType();
            if (sUnBilledBnsType != "")
            {
                MessageBox.Show("Save billing first for " + sUnBilledBnsType, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cmbBnsType.Focus();
            }
             */

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // RMC 20111220 added object locking in billing (s)
            if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "DELETE", "COL"))
            {
            }
            // RMC 20111220 added object locking in billing (e)

            this.Close();
        }

        private void btnViewSOA_Click(object sender, EventArgs e)
        {
            if (CheckUnBilled()) // ALJ 20110321
            {
                // (s) ALJ 20110907 re-assessment validation
                if (btnViewSOA.Text == "&For SOA")
                {
                    if (AppSettingsManager.Granted("AIBB")) // (ASS-BILLING-BILL-BILLING)
                    {
                        // save for soa printing
                        if (BillingClass.SaveForSOA())
                        {
                            dgvTaxFees.Rows.Clear();
                            btnViewSOA.Text = "&View SOA";

                            GrossMonitoring();  // RMC 20111227 added Gross monitoring module for gross >= 200000
                        }
                    }
                }
                else
                {
                    if (AppSettingsManager.Granted("AIBVS")) // (ASS-BILLING-VIEW SOA)
                    {

                        // (e) ALJ 20110907 re-assessment validation
                        // Check access rights
                        fSOA = new frmSOA();
                        fSOA.iFormState = 0;
                        fSOA.sBIN = this.m_sBIN;
                        fSOA.txtTaxYear.Text = this.txtTaxYear.Text;
                        fSOA.txtStat.Text = this.txtStatus.Text;
                        fSOA.GetData(this.m_sBIN);
                        fSOA.ShowDialog();
                    } // ALJ 20110907 re-assessment validation
                } // ALJ 20110907 re-assessment validation
            }
        }

        /// <summary>
        /// ALJ 20110321
        /// Function to check if all business types have been billed
        /// </summary>
        private bool CheckUnBilled()
        {
            string sUnBilledBnsType;
            sUnBilledBnsType = BillingClass.UnBilledBusinessType();
            if (sUnBilledBnsType != "")
            {
                if (BillingClass.BnsIsExempted(AppSettingsManager.GetBnsCodeByDesc(sUnBilledBnsType)))    // RMC 20120120
                {

                }
                else
                {
                    MessageBox.Show("Save billing first for " + sUnBilledBnsType, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cmbBnsType.Focus();
                    return false;
                }
            }
            return true;

        }


        // RMC 20110414 (s)
        private void SearchClick()
        {
            if (btnSearch.Text == "&Search")
            {

                // RMC 20111219 added validation of business mapping deficiencies in Billing (s)
                /* if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "ADD", "COL"))
                 {
                    
                     return;
                 }*/
                // RMC 20120109 transferred record-locking in Billing

                if (AppSettingsManager.ValidateMapping(bin.GetBin()))
                {
                    if (MessageBox.Show("Deficiencies discovered during business mapping drive.\nContinue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        MessageBox.Show("Please validate this record at " + AppSettingsManager.GetConfigValue("41") + " before Billing.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                }
                // RMC 20111219 added validation of business mapping deficiencies in Billing (e)

                m_sBIN = bin.GetBin();
                if (SourceClass == "Billing")
                {
                    // (s) check if has pending for re-billing from treas module
                    frmForReBilling ForReBillingForm = new frmForReBilling();
                    ForReBillingForm.BIN = m_sBIN;
                    if (!ForReBillingForm.CheckRecordForReBill() && ForReBillingForm.BIN != string.Empty) // test if bin not for re billing.. then check if has pending records for re-billing
                    {
                        ForReBillingForm.UserCode = AppSettingsManager.SystemUser.UserCode;
                        if (ForReBillingForm.WithPendingBilling())
                        {
                            ForReBillingForm.ShowDialog();
                            if (ForReBillingForm.BIN != string.Empty)
                            {
                                bin.GetLGUCode = ForReBillingForm.BIN.Substring(0, 3);
                                bin.GetDistCode = ForReBillingForm.BIN.Substring(4, 2);
                                bin.GetTaxYear = ForReBillingForm.BIN.Substring(7, 4);
                                bin.GetBINSeries = ForReBillingForm.BIN.Substring(12, 7);
                            }
                        }
                    }
                    else
                    {
                        // clear ForReBillingForm.BIN if empty or refuse to  re-bill BIN
                        if (ForReBillingForm.BIN == string.Empty)
                        {
                            bin.GetTaxYear = string.Empty;
                            bin.GetBINSeries = string.Empty;
                            ForReBillingForm.Dispose();
                            bin.Focus();
                            return;
                        }
                    }
                    ForReBillingForm.Dispose();
                    // (e) check if has pending for re-billing from treas module
                }

                // RMC 20120109 transferred record-locking in Billing (s)
                if (bin.GetTaxYear != "" || bin.GetBINSeries != "")
                {
                    if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "ADD", "COL"))
                    {
                        return;
                    }
                }
                // RMC 20120109 transferred record-locking in Billing (e)


                if (bin.GetTaxYear == string.Empty || bin.GetBINSeries == string.Empty)
                {
                    string sBinSearch = string.Empty;
                    // call search engine
                    //MessageBox.Show("Call Search Emgine");
                    // GDE 20110801 add Search Business Module
                    frmSearchBusiness fSearchBns = new frmSearchBusiness();

                    fSearchBns.ShowDialog();
                    // RMC 20120109 corrected log-out in Billing-search (s)
                    if (fSearchBns.sBIN == "")
                        return;
                    else
                    {
                        if (AppSettingsManager.blnIsConflict(fSearchBns.sBIN))
                        {
                            MessageBox.Show("Conflict Bin");
                            ClearControls();
                            btnSearch.Text = "&Search";
                            return;
                        }
                    }
                    // RMC 20120109 corrected log-out in Billing-search (e)

                    sBinSearch = fSearchBns.dgvResult.SelectedRows[0].Cells[0].Value.ToString();
                    bin.txtTaxYear.Text = sBinSearch.Substring(7, 4);
                    bin.txtBINSeries.Text = sBinSearch.Substring(12, 7);
                    // GDE 20110801 add Search Business Module

                }
                m_sBIN = bin.GetBin(); // refresh BIN

                BillingRetrieve();


                if (bin.GetTaxYear != string.Empty || bin.GetBINSeries != string.Empty)
                    btnSearch.Text = "&Clear";
            }
            else
            {
                if (CheckUnBilled()) // ALJ 20110321
                {
                    ClearControls();
                    btnSearch.Text = "&Search";
                }
            }
        }
        // RMC 20110414 (e)

        /// <summary>
        /// ALJ 20110907 re-assessment validation
        /// check if has the appropriate access rights for billing or re billing is OK
        /// </summary>
        /// <returns></returns>
        public bool IsBillingAccessOK()
        {
            // (s) ALJ 20110907 re-assessment validation
            if (AppSettingsManager.IsForSoaPrint(m_sBIN, txtTaxYear.Text.ToString()))
            {
                if (!AppSettingsManager.Granted("AIRB")) // not granted to re-assess/re-bill (ASS-BILLING-REBIL)
                {
                    MessageBox.Show("Billing is ready for SOA. You do not have access to re-bill/cancel bill.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ClearControls();
                    btnSearch.Text = "&Search";
                    return false;
                }
                btnViewSOA.Text = "&View SOA";

            }
            else
            {
                if (!AppSettingsManager.Granted("AIBB")) // not granted to assess/bill (ASS-BILLING-BILL-BILLING)
                {
                    MessageBox.Show("You do not have access to bill or not yet ready for SOA.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ClearControls();
                    btnSearch.Text = "&Search";
                    return false;
                }
                btnViewSOA.Text = "&For SOA";

            }

            // (s) ALJ 20110907 re-assessment validation
            if (btnViewSOA.Text == "&View SOA")
            {
                if (txtTaxYear.Text.ToString() == BillingClass.CurrentYear)
                    btnViewSOA.Enabled = true;
                else
                    btnViewSOA.Enabled = false;
            }
            // (e) ALJ 20110907 re-assessment validation

            if (AppSettingsManager.TreasurerModule(m_sBIN, txtTaxYear.Text.ToString()) == "2")
            {
                if (!AppSettingsManager.Granted("AUTM")) // not granted (ASS-UTIL-TREASURERS MODULE)
                {
                    MessageBox.Show("Records already approved. Cannot re-bill.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ClearControls();
                    btnSearch.Text = "&Search";
                    return false;
                }

            }
            return true;
            // (e) ALJ 20110907
        }

        protected void GrossMonitoring()
        {
            // RMC 20111227 added Gross monitoring module for gross >= 200000
            OracleResultSet pGross = new OracleResultSet();
            double dGross = 0;
            double dGrossMonitor = 300000;
            double dPrevGross = 0;
            string sPrevTaxYear = string.Format("{0:####}", Convert.ToInt32(txtTaxYear.Text.ToString()) - 1);

            pGross.Query = "select sum(gross) from bill_gross_info where bin = '" + m_sBIN + "'";
            pGross.Query += " and tax_year = '" + txtTaxYear.Text + "'";
            if (pGross.Execute())
            {
                if (pGross.Read())
                {
                    dGross = pGross.GetDouble(0);
                    //dGross = pGross.GetInt(0);
                }
            }
            pGross.Close();


            if (dGross >= dGrossMonitor)
            {
                pGross.Query = "select * from businesses where bin = '" + m_sBIN + "' and tax_year = '" + sPrevTaxYear + "'";
                if (pGross.Execute())
                {
                    if (pGross.Read())
                        dPrevGross = pGross.GetDouble("gr_1");
                    else
                    {
                        pGross.Close();

                        pGross.Query = "select * from buss_hist where bin = '" + m_sBIN + "' and tax_year = '" + sPrevTaxYear + "'";
                        if (pGross.Execute())
                        {
                            if (pGross.Read())
                                dPrevGross = pGross.GetDouble("gr_1");
                        }
                    }
                }
                pGross.Close();

                pGross.Query = "select sum(gross) from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + sPrevTaxYear + "'";
                if (pGross.Execute())
                {
                    if (pGross.Read())
                        dPrevGross += pGross.GetDouble(0);
                }
                pGross.Close();

                string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

                pGross.Query = "delete from gross_monitoring where ";
                pGross.Query += " bin = '" + m_sBIN + "' and ";
                pGross.Query += " tax_year = '" + txtTaxYear.Text + "' ";
                pGross.Query += " and action = '0'";
                if (pGross.ExecuteNonQuery() == 0)
                { }

                pGross.Query = "insert into gross_monitoring values (";
                pGross.Query += "'" + m_sBIN + "', 'GROSS', '0',";
                //pGross.Query += "'" + m_sBIN + "', 'GROSS', '1',";  // RMC 20120110 disable locking of gross higher than 300,000 (by treas)
                pGross.Query += "'" + txtTaxYear.Text + "', ";
                pGross.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                pGross.Query += "to_date('" + sDate + "','MM/dd/yyyy'), ";
                pGross.Query += "to_date('" + sDate + "','MM/dd/yyyy'), ";
                pGross.Query += " " + dPrevGross + ",";
                pGross.Query += " " + dGross + ", ";
                pGross.Query += "'NO', '')";
                if (pGross.ExecuteNonQuery() == 0)
                { }


            }


        }

        private string GetPrevStat()
        {
            // RMC 20120113 modified comparing of taxes if REN only
            OracleResultSet pStat = new OracleResultSet();
            string sPrevStat = "";

            pStat.Query = "select * from businesses where bin = '" + bin.GetBin() + "'";
            if (pStat.Execute())
            {
                if (pStat.Read())
                    sPrevStat = pStat.GetString("bns_stat");
            }
            pStat.Close();

            return sPrevStat;
        }

    }
}