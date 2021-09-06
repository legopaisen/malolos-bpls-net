//#######################################
// Modifications:   
// ALJ 20130219 NEW Discovery - delinquent
// ALJ 20120309 Rev exam for new
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
using Amellar.Modules.ApplicationRequirements;
using Amellar.Modules.PaymentHistory;

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

        private bool m_bIsNewDiscovery = false; // ALJ 20130219 NEW Discovery - delinquent
        public string m_sBrgyCode = string.Empty;
        private DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
        private string m_sOrgnKind = string.Empty;  // RMC 20180131 added validation of specific checklist in Billing

        public frmBilling()
        {
            InitializeComponent();
            dgvTaxFees.Rows.Clear();
            dgvAddlInfo.Rows.Clear();
            bin.GetLGUCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.LGUCode);
            bin.GetDistCode = AppSettingsManager.GetConfigObject("11");
            m_objNumberValidator = new MoneyTextValidator();
            m_objNumberValidator2 = new SimpleTextValidator();
            m_objNumberValidator2.SetIntCharacterSet();


        }

        // (s) ALJ 20130219 NEW Discovery - delinquent
        public bool NewDiscovery
        {
            get { return m_bIsNewDiscovery; }
            set { m_bIsNewDiscovery = value; }
        }
        // (e) ALJ 20130219 NEW Discovery - delinquent

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

        public string BusinessAddress
        {
            get { return txtBusinessAdd.Text; }
            set { txtBusinessAdd.Text = value; }
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

        public string OrgnKind  // RMC 20180131 added validation of specific checklist in Billing
        {
            get { return this.m_sOrgnKind; }
            set { this.m_sOrgnKind = value; }
        }
            

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
                btnDelqDues.Visible = true; //MCR 20150120
                //MCR 20170529 (s)
                //chkTagReass.Visible = true; //JARS 20171003 MALOLOS
                chkTagReass.Visible = false; //JARS 20171003 MALOLOS
                chkTagReass.Checked = false;
                //MCR 20170529 (e)
            }
            else if (this.SourceClass == "RevExam")
            {
                BillingClass = new RevExam(this);
                //this.lblGross.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //lblGross.Text = "New-Gross";
                lblGross.Text = "New-Gross/\nCapital"; // ALJ 20120309 Rev exam for new
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
            else if (this.SourceClass == "RetirementBilling")   // RMC 20140708 added retirement billing
            {
                BillingClass = new Retirement(this);
                txtTaxYear.ReadOnly = true;
            }

            // RMC 20110807 created CancelBilling module (e)

            BillingClass.m_bIsRenPUT = false;   // RMC 20140725 Added Permit update billing
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.blnIsConflict(bin.GetBin()))
            {
                MessageBox.Show("Conflict Bin");
                ClearControls();    // RMC 20120109 insert bin conflict validation in Billing search
                btnSearch.Text = "&Search";  // RMC 20120109 insert bin conflict validation in Billing search
                btnDelqDues.Enabled = false; //MCR 20150120
                return;
            }

            if (CheckforHOLD() == true) //WEB MCR 20210707
                MessageBox.Show("Cannot continue! This record is currently on hold in online application., ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

            SearchClick();  // RMC 20110414      
            // transferred all codes to SearchClick()            
        }

        private bool CheckforHOLD() //ON WEB MCR 20210707
        {
            int iCount = 0;
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select count(*) as iCnt from hold_records_web where bin = '" + bin.GetBin() + "'";
            int.TryParse(pSet.ExecuteScalar(), out iCount);

            if (iCount == 0)
                return false;
            else
                return true;
        }

        private void ClearControls()
        {
            // RMC 20111220 added object locking in billing (s)
            //if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "DELETE", "COL"))
            if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "DELETE", "ASS")) // RMC 20150430 billing was transferred to BPS
            {
            }
            // RMC 20111220 added object locking in billing (e)

            chkTagReass.Checked = false;
            chkTagReass.Enabled = false;
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
            txtBusinessAdd.Text = string.Empty; //MCR 20150123
            txtPrevGrossCapital.Text = string.Empty; //MCR 20150123
            m_sBrgyCode = string.Empty; //MCR 20140929  // RMC 20150806 merged exemption by brgy from Mati
            btnPrevGrossCapital.Enabled = false;   // RMC 20170106 added editing of prev gross/cap in Billing module
        }

        private void BillingRetrieve()
        {
            // RMC 20150108 (S)
            if (this.SourceClass == "Billing")
            {
                BillingClass = new Billing(this);
            }
            else if (this.SourceClass == "RevExam")
            {
                BillingClass = new RevExam(this);
            }
            else if (this.SourceClass == "CancelBilling")
            {
                BillingClass = new CancelBilling(this);
            }
            else if (this.SourceClass == "PermitUpdate")
            {
                BillingClass = new PermitUpdate(this);

            }
            else if (this.SourceClass == "RetirementBilling")
            {
                BillingClass = new Retirement(this);
            }
            // RMC 20150108 (E)  

            // RMC 20110826 added validation if billing exists (s)
            if (!BillingClass.ValidBilling())
                return;
            // RMC 20110826 added validation if billing exists (e)

            // Billing BillingClass = new Billing(this);  

            BillingClass.ReturnValues();

            if (this.SourceClass != "CancelBilling")    // RMC 20150120
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
                //if (cmbBnsType.SelectedIndex == 0) // Note: add filter. not applicable if Permit update - ADDL 
                if (cmbBnsType.SelectedIndex == 0 && this.SourceClass != "PermitUpdate")    // RMC 20150325 modified billing for permit update application
                    m_bIsMain = true;
                else
                    m_bIsMain = false;
                BillingClass.ReLoadAddlInfo();
                BillingClass.RetrieveBilling();
                PrevGrossCapital(); //MCR 20150123
            }
        }

        private void btnRetrieveBilling_Click(object sender, EventArgs e)
        { 
            //peb 20191218 (s) getting the barangay value
            OracleResultSet result = new OracleResultSet();
            result.Query = "select bns_brgy from businesses where bin ='" + bin.m_sBIN + "'";
            if (result.Execute())
            {
                if (result.Read())
                    brgy = result.GetString("bns_brgy");
            }

            result.Close();
            if (brgy == null)//peb 20191227
            {
                result.Query = "select bns_brgy from business_que where bin ='" + bin.m_sBIN + "'";
                if (result.Execute())
                {
                    if (result.Read())
                        brgy = result.GetString("bns_brgy");
                }
                result.Close();
            }
            //peb 20191218 (e)

            BillingClass.RetrieveBilling();
          
            ExemptionByBrgyLoad();//MCR 20140930 Exempted by brgy   // RMC 20150806 merged exemption by brgy from Mati
            //BrgyClearanceFee();//PEB20191217
        }

        public string brgy;
        private void BrgyClearanceFee()//peb 20191217
        {
            OracleResultSet result = new OracleResultSet();
            double amount=0;
            result.Query = "select SUM(AMOUNT) as amount from barangay_addl_sched where brgy_nm='" + brgy + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    if (result.GetInt("AMOUNT") != 0)
                    {
                        amount = amount + result.GetInt("amount");
                        //dgvTaxFees.Rows.Add(true, "BARANGAY CLEARANCE FEE", String.Format("{0:0.00}", result.GetInt("AMOUNT")), "99", "TF");
                        dgvTaxFees.Rows.Add(false, "BARANGAY CLEARANCE FEE", String.Format("{0:0.00}", 0), "50", "TF");
                    }
                    else
                    {
                        dgvTaxFees.Rows.Add(false, "BARANGAY CLEARANCE FEE", result.GetInt("amount").ToString("N0"), "50", "TF");
                    }
                } 
            }
            result.Close();
            amount += double.Parse(Total);
            Total = String.Format("{0:0.00}", amount); 
            frmBillFee frm = new frmBillFee();
            frm.brgy = brgy;
            frm.moduleswitch = "BARANGAY CLEARANCE FEE";
        }

        private void ExemptionByBrgyLoad() //MCR 20140930 Exempted by brgy  // RMC 20150806 merged exemption by brgy from Mati
        {
            OracleResultSet pSet = new OracleResultSet();
            for (int i = 0; i < dgvTaxFees.RowCount; i++)
            {
                pSet.Query = "SELECT * FROM tax_and_fees_exempted_brgy where fees_code = :1 AND brgy_code = :2 AND rev_year = :3";
                pSet.AddParameter(":1", dgvTaxFees.Rows[i].Cells[3].Value.ToString());
                pSet.AddParameter(":2", m_sBrgyCode);
                pSet.AddParameter(":3", AppSettingsManager.GetConfigValue("07"));
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        //dgvTaxFees.Rows[i].Cells[0].Value = false;
                        dgvTaxFees_CellContentClick(dgvTaxFees, new DataGridViewCellEventArgs(0, i));
                    }
                pSet.Close();
            }
        }

        private void dgvTaxFees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
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
                    /*iFRIndex = dgvTaxFees.Rows.Count - 1; // index of fire tax (last row)
                    fAmount = BillingClass.UpdateFireTax();
                    if (fAmount > 0) // ALJ 20100115
                        dgvTaxFees.Rows[iFRIndex].Cells[0].Value = true;
                    else
                        dgvTaxFees.Rows[iFRIndex].Cells[0].Value = false;
                    dgvTaxFees.Rows[iFRIndex].Cells[2].Value = string.Format("{0:#,##0.00}", fAmount);
                    BillingClass.UpdateBillTable(fAmount, "AF", "02"); // update fire

                    */  // RMC 20170105 removed updating of fire tax in billing
                }
                {
                    // RMC 20181121 Customized Fire Tax Fee computation, enabled (s)
                    iFRIndex = dgvTaxFees.Rows.Count - 1; // index of fire tax (last row)
                    if (sFeesCode == "99")  // RMC 20181123 correction in Billing if Fire tax not enabled
                    {
                        Boolean bTemp = (bool)dgvTaxFees.Rows[e.RowIndex].Cells[0].Value;
                        if (bTemp == true)
                            fAmount = BillingClass.UpdateFireTax();

                        if (fAmount > 0) // ALJ 20100115
                        {
                            dgvTaxFees.Rows[iFRIndex].Cells[0].Value = true;
                        }
                        else
                        {
                            dgvTaxFees.Rows[iFRIndex].Cells[0].Value = false;
                        }

                        dgvTaxFees.Rows[iFRIndex].Cells[2].Value = string.Format("{0:#,##0.00}", fAmount);
                        //BillingClass.UpdateBillTable(fAmount, "AF", "02"); // update fire
                        BillingClass.UpdateBillTable(fAmount, "AF", "99");  // RMC 20181121 Customized Fire Tax Fee computation
                        // RMC 20170105 removed updating of fire tax in billing
                    }
                    // RMC 20181121 Customized Fire Tax Fee computation, enabled (e)
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

            double dTotal = 0;
            double.TryParse(txtTotal.Text.ToString(), out dTotal);

            string feesdesc = dgvTaxFees.Rows[dgvTaxFees.Rows.Count - 1].Cells[1].Value.ToString();
            string check = dgvTaxFees.Rows[dgvTaxFees.Rows.Count - 1].Cells[2].Value.ToString();
            if (check == "0.00" && feesdesc == "BARANGAY CLEARANCE FEE")
            {
                MessageBox.Show("The Barangay Clearance fee has no amount");
            }

            if (BillingClass.IsBillCreated) 
            {
                dgvTaxFees.Rows.Clear();
                txtTotal.Text = "0";
                string sUnBilledBnsType = "";

                /*if (txtStatus.Text == "RET")    // RMC 20150108
                    btnViewSOA.Enabled = true;
                else    */
                // RMC 20150112 mods in retirement billing, put rem
               /* {
                    // (s) ALJ 20110907 re-assessment validation
                    //if (txtTaxYear.Text.ToString() == BillingClass.CurrentYear)// || m_bIsNewDiscovery == true) // ALJ 20130219 NEW Discovery - delinquent -  '|| m_bIsNewDiscovery == true'    // RMC 20150129
                    if (txtTaxYear.Text.ToString() == BillingClass.CurrentYear || m_bIsNewDiscovery == true)    // RMC 20161109 merged discovery-delinq
                        btnViewSOA.Enabled = true;
                    else
                        btnViewSOA.Enabled = false;
                    // (e) ALJ 20110907 re-assessment validation
                }*/ // RMC 20180117 correction in validation of unbilled line of business if auto-renewal, put rem

                if (cmbBnsType.Items.Count > 1)
                    sUnBilledBnsType = BillingClass.UnBilledBusinessType();
                if (sUnBilledBnsType == "")
                {
                    // Compare Taxses
                    //if (txtStatus.Text.ToString() == "REN") // ALJ 20110907 re-assessment validation - put remark, to cater any status  

                    // RMC 20150528 enabled treas module (s)
                    /* REM MCR 20141222 Lubao version*/
                    if (GetPrevStat() == "REN" || GetPrevStat() == "NEW")  // RMC 20120113 modified comparing of taxes if REN only //AFM 20200514 added condition for NEW since warning doesn't prompt if new capital is lower in rev adjustment
                    { 
                        if (this.Status == "RET")   // RMC 20170110 removed compare tax for RET
                        { }
                        else
                            BillingClass.CompareTaxes();
                    }
                    // RMC 20150528 enabled treas module (e)
                    

                    // RMC 20180117 correction in validation of unbilled line of business if auto-renewal (s)
                    if (txtTaxYear.Text.ToString() == BillingClass.CurrentYear || m_bIsNewDiscovery == true)    // RMC 20161109 merged discovery-delinq
                        btnViewSOA.Enabled = true;
                    else
                        btnViewSOA.Enabled = false;
                    // RMC 20180117 correction in validation of unbilled line of business if auto-renewal (e)
                }
                else
                {
                    btnViewSOA.Enabled = false; // RMC 20180117 correction in validation of unbilled line of business if auto-renewal
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

                //MCR 20210610 (s)
                if (AppSettingsManager.GetConfigValue("75") == "Y")
                {
                    string sQtrValid = "", sValue = "";
                    DateTime dtQtrValid;

                    sQtrValid = AppSettingsManager.ValidUntil(bin.GetBin(), ConfigurationAttributes.CurrentYear);
                    dtQtrValid = DateTime.Parse(sQtrValid);

                    OracleResultSet pSet = new OracleResultSet();
                    pSet.Query = "select BO_SEND_BILLED_NOTIF('" + bin.GetBin() + "'," + dTotal + ",to_date('" + dtQtrValid.ToString("MM/dd/yyyy") + "','MM/dd/yyyy')) from dual";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                            sValue = pSet.GetString(0);
                    }
                    pSet.Close();
                }

                if (btnViewSOA.Enabled == true)
                {
                    OracleResultSet pSet = new OracleResultSet();
                    pSet.Query = "delete from soa_tbl where bin = '" + bin.GetBin() + "'";
                    if (pSet.ExecuteNonQuery() != 0)
                    {
                    }
                    pSet.Close();
                }
                //MCR 20210610 (e)
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
            // RMC 20170106 added editing of prev gross/cap in Billing module (s)
            txtPrevGrossCapital.ReadOnly = true;
            btnPrevGrossCapital.Enabled = true;
            btnPrevGrossCapital.Text = "Edit";
            // RMC 20170106 added editing of prev gross/cap in Billing module (e)
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
            btnPrevGrossCapital.Enabled = p_blnEnabled;   // RMC 20170106 added editing of prev gross/cap in Billing module
        }

        private void ValidateTaxYearQtr()
        {
            // RMC 20140708 added retirement billing (s)
            if (this.SourceClass == "RetirementBilling")
                return;
            // RMC 20140708 added retirement billing (e)

            if (txtTaxYear.Text.ToString().Length == 4 && BillingClass.IsInitialLoad == false)
            {
                btnEditAddlInfo.Enabled = true; // ALJ 20110321
                btnRetrieveBilling.Enabled = true;
                btnPrevGrossCapital.Enabled = true;   // RMC 20170106 added editing of prev gross/cap in Billing module
                int iTaxYear, iBaseTaxYear, iCurrentYear;
                int.TryParse(txtTaxYear.Text.ToString(), out iTaxYear);
                int.TryParse(BillingClass.BaseTaxYear, out iBaseTaxYear);
                int.TryParse(BillingClass.CurrentYear, out iCurrentYear);
                if (iTaxYear < iBaseTaxYear || iTaxYear > iCurrentYear)
                {
                    btnEditAddlInfo.Enabled = false; // ALJ 20110321
                    btnRetrieveBilling.Enabled = false;
                    btnPrevGrossCapital.Enabled = false;   // RMC 20170106 added editing of prev gross/cap in Billing module
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
                        btnPrevGrossCapital.Enabled = false;   // RMC 20170106 added editing of prev gross/cap in Billing module
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
                btnPrevGrossCapital.Enabled = false;   // RMC 20170106 added editing of prev gross/cap in Billing module
                dgvTaxFees.Rows.Clear();
                txtTotal.Text = "0";
            }
        }

        private void ValidateQtrDec()
        {

            if (txtStatus.Text.ToString() == "NEW")
            {
                if (txtQtr.Text.ToString().Length == 1 && BillingClass.IsInitialLoad == false
                    && BillingClass.m_bPermitUpdateFlag == false)   // RMC 20160517 correction in permit-update billing with renewal application
                {

                    btnEditAddlInfo.Enabled = true; // ALJ 20110321
                    btnRetrieveBilling.Enabled = true;
                    btnPrevGrossCapital.Enabled = true;   // RMC 20170106 added editing of prev gross/cap in Billing module
                    int iQtr, iBaseDueQtr, iMaxDueQtr;
                    int.TryParse(txtQtr.Text.ToString(), out iQtr);
                    int.TryParse(BillingClass.BaseDueQtr, out iBaseDueQtr);
                    int.TryParse(BillingClass.MaxDueQtr, out iMaxDueQtr);
                    
                    if (iQtr < iBaseDueQtr || iQtr > iMaxDueQtr)
                    {
                        btnEditAddlInfo.Enabled = false; // ALJ 20110321
                        btnRetrieveBilling.Enabled = false;
                        btnPrevGrossCapital.Enabled = false;   // RMC 20170106 added editing of prev gross/cap in Billing module
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
                            btnPrevGrossCapital.Enabled = false;   // RMC 20170106 added editing of prev gross/cap in Billing module
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
                    btnPrevGrossCapital.Enabled = false;   // RMC 20170106 added editing of prev gross/cap in Billing module
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
            {
                btnSave.Enabled = true;   
            }
            else
            {
                if (m_sSource == "RetirementBilling")
                {
                    if (dgvTaxFees.RowCount > 0)
                    {
                        MessageBox.Show("Amount Due is 0", "Retirement Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnSave.Enabled = false;
                        btnViewSOA.Enabled = true;
                    }
                    else
                        btnViewSOA.Enabled = false;
                }
                else
                    btnSave.Enabled = false;
            }
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

            // RMC 20150811 added validation in editing presumptive gross in Billing based on entered pre-gross in utilities (s)
            if (txtPreGross.Text.Trim() != "")
            {
                double dGr1 = 0;
                double dGr2 = 0;
                double dPreGross = 0;
                double.TryParse(txtPreGross.Text, out dPreGross);

                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = "select * from pre_gross_tbl where bns_code = '" + txtBnsCode.Text.Substring(0, 2) + "' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        dGr1 = pSet.GetDouble("gr_1");
                        dGr2 = pSet.GetDouble("gr_2");

                        if (dPreGross < dGr1 || dPreGross > dGr2)
                        {
                            MessageBox.Show("Presumptive gross is invalid. Please check presumptive gross table in Utilities.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            txtPreGross.Text = "";
                            return;
                        }
                    }
                }
                pSet.Close();
            }
            // RMC 20150811 added validation in editing presumptive gross in Billing based on entered pre-gross in utilities (e)
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
                    //if (this.SourceClass == "CancelBilling")    // RMC 20140110 disable validation of save billing first for cancel billing transaction
                    if (this.SourceClass == "CancelBilling" || this.Status == "RET")    // RMC 20140708 added retirement billing     
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        MessageBox.Show("Save billing first for " + sUnBilledBnsType, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                e.Cancel = false;

                // RMC 20140103 added end-tasking if Billing form closed (s)
                //if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "DELETE", "COL"))
                if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "DELETE", "ASS")) // RMC 20150430 billing was transferred to BPS
                {
                }
                // RMC 20140103 added end-tasking if Billing form closed (e)
            }


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
            //if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "DELETE", "COL"))
            if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "DELETE", "ASS")) // RMC 20150430 billing was transferred to BPS
            {
            }
            // RMC 20111220 added object locking in billing (e)

            this.Close();
        }

        private void btnViewSOA_Click(object sender, EventArgs e)
        {
            if (CheckUnBilled()) // ALJ 20110321
            {
                // RMC 20170117 added additional validation in SOA of subject for verification (s)
                if (AppSettingsManager.TreasurerModule(this.m_sBIN))
                {
                    MessageBox.Show("SOA is subject for verification.");
                    return;
                }
                // RMC 20170117 added additional validation in SOA of subject for verification (e)

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

                        // RMC 20160512 correction in SOA of PermitUpdate if with existing dues (s)
                        OracleResultSet pTmpValid = new OracleResultSet();
                        bool bRen = false;
                        pTmpValid.Query = "select * from taxdues where bin = '"+ bin.GetBin()+"' and due_state = 'R'";
                        if(pTmpValid.Execute())
                        {
                            if(pTmpValid.Read())
                            {
                                bRen = true;
                            }
                        }
                        pTmpValid.Close();

                        if (this.SourceClass == "PermitUpdate" && bRen)
                        {
                            fSOA.txtStat.Text = "REN";
                        }// RMC 20160512 correction in SOA of PermitUpdate if with existing dues (e)
                        else
                        {
                            fSOA.txtStat.Text = this.txtStatus.Text;
                        }
                        fSOA.GetData(this.m_sBIN);
                        fSOA.m_dTransLogIn = m_dTransLogIn; // RMC 20170822 added transaction log feature for tracking of transactions per bin
                        fSOA.ShowDialog();

                        // RMC 20161215 reset billing after printing of SOA (s)
                        ClearControls();    
                        btnSearch.Text = "&Search";
                        // RMC 20161215 reset billing after printing of SOA (e)
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
                else if (this.Status == "RET" && !BillingClass.BnsWithRetirementApp(AppSettingsManager.GetBnsCodeByDesc(sUnBilledBnsType)))  // RMC 20140708 added retirement billing
                { 
                }
                else
                {
                    
                    //if (txtTaxYear.Text.ToString() == BillingClass.CurrentYear) //JARS 20170809 so that it won't prompt every instance
                    //{

                    //}
                    //MessageBox.Show("Save billing first for " + sUnBilledBnsType, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //cmbBnsType.Focus();
                    //return false;

                    // RMC 20180117 correction in validation of unbilled line of business if auto-renewal (s).
                    //if (this.SourceClass == "CancelBilling" || this.Status == "RET")
                    if (this.SourceClass == "CancelBilling" || this.Status == "RET" || BillingClass.m_bIsNoSale) // RMC 20180125 enable soa if other line of business has no operation/sale
                    { }
                    else
                    {
                        MessageBox.Show("Save billing first for " + sUnBilledBnsType, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                    // RMC 20180117 correction in validation of unbilled line of business if auto-renewal (e)
                }
            }
            return true;

        }


        // RMC 20110414 (s)
        private void SearchClick()
        {
            if (btnSearch.Text == "&Search")
            {
                string sRet = string.Empty;
                OracleResultSet result1 = new OracleResultSet();
                result1.Query = "select * from businesses where bin='" + bin.GetBin() + "' and bns_stat='RET'";
                if (result1.Execute())
                {
                    if (result1.Read())
                    {
                        sRet = "This Business has already been retired.";
                        MessageBox.Show(sRet);
                        ClearControls();
                        btnSearch.Text = "&Search";
                        btnDelqDues.Enabled = false; //MCR 20150120
                        return;
                    }
                }
                result1.Close();

                // RMC 20171123 added validation in Billing if with tagged violation (s)
                if(InspectionTool.ViolationsTool.IsViolation(bin.GetBin()))
                {
                    MessageBox.Show("BIN has been tagged for a violation.\nBilling not allowed.","Billing",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    ClearControls();
                    btnSearch.Text = "&Search";
                    return;
                }
                // RMC 20171123 added validation in Billing if with tagged violation (e)

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

                //AFM MAO-19-11511 20191211 (s)
                string sValue = AppSettingsManager.GetNigViolist(bin.GetBin());
                if (sValue != "")
                    MessageBox.Show("Record was tagged in negative list\n\n" + sValue, "Application Form", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //AFM MAO-19-11511 20191211 (e)

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
                    //if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "ADD", "COL"))
                    if (TaskMan.IsObjectLock(bin.GetBin(), "BILLING", "ADD", "ASS"))    // RMC 20150430 billing was transferred to BPS
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
                    if (this.SourceClass == "Billing")  // RMC 20150428 
                        fSearchBns.ModuleCode = "BILLING";  // RMC 20150426 QA corrections

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
                            btnDelqDues.Enabled = false; //MCR 20150120
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

                // RMC 20160222 added validation in billing if billing exists, force user to cancel billing first due to acceleration clause - Tumauini (s)
                if (this.SourceClass == "Billing")
                {
                    BillingClass.IsInitialLoad = true;
                    if (!ValidBilling_Acceleration())
                        return;
                }
                // RMC 20160222 added validation in billing if billing exists, force user to cancel billing first due to acceleration clause - Tumauini (e)

                BillingRetrieve();
                PrevGrossCapital();

                if (this.SourceClass == "RevExam") //AFM 20200108 fixed display of business address
                    this.BusinessAddress = AppSettingsManager.GetBnsAddress(this.BIN);

                // RMC 20180131 added validation of specific checklist in Billing (s)
                if (this.SourceClass == "Billing")
                {
                    string sDesc = AppRequirement.Checklist(ConfigurationAttributes.CurrentYear,bin.GetBin(),txtStatus.Text.ToString(),txtBnsCode.Text.ToString(),m_sOrgnKind,"SUBD");
                    if (sDesc != "")
                    {
                        MessageBox.Show(sDesc + " is required ","Billing",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
                // RMC 20180131 added validation of specific checklist in Billing (e)

                if (bin.GetTaxYear != string.Empty || bin.GetBINSeries != string.Empty)
                {
                    chkTagReass.Enabled = false ; //MCR 20170529
                    chkTagReass.Enabled = true; //MCR 20170529
                    btnDelqDues.Enabled = true; //MCR 20150120
                    btnSearch.Text = "&Clear";
                }

                m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
            }
            else
            {
                if (CheckUnBilled()) // ALJ 20110321    // RMC 20180125 enabled validation of unbilled on Clear button
                {
                    ClearControls();
                    btnDelqDues.Enabled = false; //MCR 20150120
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
                //if (!AppSettingsManager.Granted("AIBB")) // not granted to assess/bill (ASS-BILLING-BILL-BILLING)
                if (!AppSettingsManager.Granted("AIB")) // not granted to assess/bill (ASS-BILLING-BILL-BILLING) //JARS 20170629 TO MATCH WITH SYS_MODULES TABLE
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
                /*if (txtStatus.Text == "RET")    // RMC 20150108
                    btnViewSOA.Enabled = true;
                else*/
                // RMC 20150112 mods in retirement billing, put rem
                {
                    if (txtTaxYear.Text.ToString() == BillingClass.CurrentYear)
                        btnViewSOA.Enabled = true;
                    else
                        btnViewSOA.Enabled = false;
                }
            }
            // (e) ALJ 20110907 re-assessment validation

            //if (this.SourceClass != "PermitUpdate") // RMC 20170112 disable validation of rebill access for permit update billing
            if (this.SourceClass != "PermitUpdate" && this.SourceClass != "RevExam" && this.SourceClass != "RetirementBilling") // RMC 20170123 disable validation of rebill for retirement and revexam billing
            {
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
            }
            return true;
            // (e) ALJ 20110907
        }

        protected void GrossMonitoring()
        {
            // RMC 20111227 added Gross monitoring module for gross >= 200000
            OracleResultSet pGross = new OracleResultSet();
            double dGross = 0;
            double dGrossMonitor = 1000000000000;
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

        private void btnDelqDues_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select distinct tax_year from taxdues where tax_year <= '2013' and bin = '" + bin.GetBin().Trim() + "' order by tax_year asc";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    frmModifyDues formModify = new frmModifyDues();
                    formModify.BIN = bin.GetBin().Trim();
                    formModify.TaxYear = pSet.GetString(0);
                    formModify.Type = "View";
                    formModify.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No Record Found", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            pSet.Close();
        }

        private void PrevGrossCapital() //MCR 20150123
        {
            if (txtStatus.Text == "")
            {
                lblCaption.Text = "Capital/Gross";
                txtPrevGrossCapital.Text = "";
            }
            else
            {
                int iTmpValue = 0;
                string sTmpLabel = "";
                AppSettingsManager.GetPrevCapitalorGross(m_sBIN, txtTaxYear.Text.Trim(), txtBnsCode.Text.Trim(), out iTmpValue, out sTmpLabel);
                txtPrevGrossCapital.Text = iTmpValue.ToString("#,##0.00");
                lblCaption.Text = sTmpLabel;
            }
        }

        private bool ValidBilling_Acceleration()
        {
            // RMC 20160222 added validation in billing if billing exists, force user to cancel billing first due to acceleration clause - Tumauini

            if (BillingClass.IsInitialLoad)
            {
                OracleResultSet pSet = new OracleResultSet();

                try
                {
                    string sEntryYear = string.Empty;
                    string sEffYear = string.Empty;

                    pSet.Query = "select * from spl_ord_tbl order by entry_date desc";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            sEntryYear = string.Format("{0:yyyy}", pSet.GetDateTime("entry_date"));
                            sEffYear = pSet.GetString("eff_year");
                        }
                    }
                    pSet.Close();

                    pSet.Query = "select * from taxdues where bin = '" + m_sBIN + "' and tax_year >= '" + sEffYear + "' ";
                    pSet.Query += "and qtr_to_pay = '1'";
                    pSet.Query += " and '" + sEntryYear + "' = '" + AppSettingsManager.GetConfigValue("12") + "'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            pSet.Close();

                            MessageBox.Show("Billing exists. \nPlease cancel billing first and re-bill for application of acceleration clause.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;

                        }
                    }
                    pSet.Close();
                }
                catch
                {
                    return true;
                }
            }

            BillingClass.IsInitialLoad = false;


            return true;
        }

        private void btnPrevGrossCapital_Click(object sender, EventArgs e)
        {
            // RMC 20170106 added editing of prev gross/cap in Billing module

            if (btnPrevGrossCapital.Text.ToString() == "Edit")
            {
                btnPrevGrossCapital.Text = "Update";
                txtPrevGrossCapital.ReadOnly = false;

                btnCancelAddlInfo.Enabled = true;
                txtTaxYear.ReadOnly = true;
                dgvTaxFees.Enabled = false;
                btnRetrieveBilling.Enabled = false;
                btnSave.Enabled = false;
                btnEditAddlInfo.Enabled = false;
            }
            else
            {
                btnPrevGrossCapital.Text = "Edit";
                txtPrevGrossCapital.ReadOnly = true;

                btnCancelAddlInfo.Enabled = false;
                txtTaxYear.ReadOnly = false;
                dgvTaxFees.Enabled = true;
                btnRetrieveBilling.Enabled = true;
                btnSave.Enabled = true;
                btnEditAddlInfo.Enabled = true;
                
                // save here
                //AppSettingsManager.GetPrevCapitalorGross(m_sBIN, txtTaxYear.Text.Trim(), txtBnsCode.Text.Trim(), out iTmpValue, out sTmpLabel);
                OracleResultSet pCmd = new OracleResultSet();
                string sTaxYear = string.Empty;
                string sBnsStat = string.Empty;
                    
                sTaxYear = Convert.ToDouble((Convert.ToInt32(txtTaxYear.Text.Trim().ToString()) - 1)).ToString();
                sBnsStat = AppSettingsManager.GetPrevBnsStat(m_sBIN);
                double dPrevGC = 0;
                double.TryParse(txtPrevGrossCapital.Text.ToString(), out dPrevGC);

                if(sBnsStat == "NEW")
                {
                    pCmd.Query = "update buss_hist set capital = "+dPrevGC+" ";
                    pCmd.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code = '" + txtBnsCode.Text.Trim() + "'";
                    if(pCmd.ExecuteNonQuery() == 0)
                    {}

                    pCmd.Query = "update businesses set capital = "+dPrevGC+" ";
                    pCmd.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code = '" + txtBnsCode.Text.Trim() + "'";
                    if(pCmd.ExecuteNonQuery() == 0)
                    {}

                    pCmd.Query = "update bill_gross_info set capital = " + dPrevGC + " ";
                    pCmd.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code = '" + txtBnsCode.Text.Trim() + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    pCmd.Query = "update addl_bns set capital = " + dPrevGC + " ";
                    pCmd.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code_main = '" + txtBnsCode.Text.Trim() + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    if (AuditTrail.InsertTrail("AIB-EPC", "businesses", m_sBIN + "/" + sTaxYear + "/" + txtBnsCode.Text.Trim()) == 0)
                    {
                        pCmd.Rollback();
                        pCmd.Close();
                        MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    pCmd.Query = "update buss_hist set gr_1 = "+dPrevGC+" ";
                    pCmd.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code = '" + txtBnsCode.Text.Trim() + "'";
                    if(pCmd.ExecuteNonQuery() == 0)
                    {}

                    pCmd.Query = "update businesses set gr_1 = "+dPrevGC+" ";
                    pCmd.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code = '" + txtBnsCode.Text.Trim() + "'";
                    if(pCmd.ExecuteNonQuery() == 0)
                    {}

                    pCmd.Query = "update bill_gross_info set gross = " + dPrevGC + " ";
                    pCmd.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code = '" + txtBnsCode.Text.Trim() + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    pCmd.Query = "update addl_bns set gross = " + dPrevGC + " ";
                    pCmd.Query += "where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code_main = '" + txtBnsCode.Text.Trim() + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    if (AuditTrail.InsertTrail("AIB-EPG", "businesses", m_sBIN + "/" + sTaxYear + "/" + txtBnsCode.Text.Trim()) == 0)
                    {
                        pCmd.Rollback();
                        pCmd.Close();
                        MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                PrevGrossCapital();

                
            }
        }

        public void ValidatePYCNC()
        {
            // RMC 20180125 added validation of CNC for previous year
            OracleResultSet pRec = new OracleResultSet();
            string sYear = string.Empty;
            string sFeesCode = AppSettingsManager.GetFeesCodeByDesc("CNC","FS");
            if(sFeesCode == "")
                sFeesCode = AppSettingsManager.GetFeesCodeByDesc("CNC", "AD");
            int iPYear = Convert.ToInt32(ConfigurationAttributes.CurrentYear) - 1;
            sYear = string.Format("{0:####}", iPYear);

            if (txtStatus.Text == "REN")
            {
                pRec.Query = "select * from or_table where fees_code = '" + sFeesCode + "' and tax_year = '" + sYear + "'";
                pRec.Query += " and or_no in (select or_no from pay_hist where bin = '" + bin.GetBin() + "' and tax_year = '" + sYear + "')";
                if (pRec.Execute())
                {
                    if (!pRec.Read())
                    {
                        MessageBox.Show("Please bill CNC of previous year", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string sOrNo = string.Empty;
                        sOrNo = pRec.GetString("or_no");

                        MessageBox.Show("Previous year CNC OR No: " + sOrNo, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                pRec.Close();
            }
        }

        private void kryptonHeader1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtGross_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnPayHist_Click(object sender, EventArgs e)
        {
            //AFM 20191209 MAO-19-11502
            //if (AppSettingsManager.Granted("CCPH"))
            if (AppSettingsManager.Granted("ARPH")) //AFM 20200109
            {
                frmPaymentHistory frmPayHist = new frmPaymentHistory();
                frmPayHist.ShowDialog();
            }
        }

    }
}