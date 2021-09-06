
// ALJ 20130219 NEW Discovery - delinquent
// RMC 20120119 corrected saving of image in Business records
// RMC 20120104 enable buss plate in new app
// RMC 20111220 transferred validation of business if business-mapped in AppSettingsManager

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.BIN;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.BPLSApp;
using Amellar.Common.BusinessType;
using Amellar.Common.SearchOwner;
using Amellar.Common.PrintUtilities;
using System.Drawing.Printing;
using Amellar.Common.SearchBusiness;
using ComponentFactory.Krypton.Toolkit;
using ComponentFactory.Krypton.Ribbon;
using Amellar.Common.AuditTrail;
using Amellar.Common.DeficientRecords;
using Amellar.Modules.AdditionalBusiness;
using Amellar.Common.AddlInfo;
using Amellar.BPLS.Billing;
using Amellar.Common.TextValidation;
using Amellar.Modules.ApplicationRequirements;
using Amellar.Modules.OwnerProfile;
using Amellar.Common.ImageViewer;

namespace Amellar.Common.frmBns_Rec
{
    public partial class frmBusinessRecord : Form
    {
        private Object sender; //GMC 20150819 BtnAppNew Object in BPS frmMainForm
        private EventArgs e; //GMC 20150819 BtnAppNew EventArgs in BPS frmMainForm
        private bool bAddNewApp = false;
        protected Library LibraryClass = new Library();        
        private VSPrinterEmuModel model;
        public string m_sFormStatus = string.Empty;
        bool bState = false;
        bool loadFinished = false; // JAV 20170928
        public string m_sTaxYear;
        OracleResultSet pSet = new OracleResultSet();
        frmSearchBusiness frmSearchBns = new frmSearchBusiness();
        private string m_sBuss_Own_Code = string.Empty;
        private string m_sBuss_BusnCode = string.Empty;
        private string m_sBuss_Prev_BnsOwn = string.Empty;
        public string m_strBnsCode = string.Empty;
        public string m_strTmpBnsCode = string.Empty;
        public string m_strTempBIN = string.Empty;
        private string m_strPrevBnsStat = string.Empty;
        private string m_strCapital = string.Empty;
        private string m_strGross = string.Empty;
        public string m_sOrNo = string.Empty;
        public string m_sCancelReason = string.Empty;
        public string m_sCancelDate = string.Empty;
        public string m_sCancelBy = string.Empty;
        public string m_sBnsUser = string.Empty;
        public string m_sOfcType = string.Empty;
        public string m_sMotherBin = string.Empty;
        public string m_sRadioMain = string.Empty;
        public string m_sBnsZip = string.Empty;
        private bool m_blnInitialLoad = true;    
        private string m_strTaxYear = string.Empty;
        private bool m_bSwEnableSave = false;
        private bool m_bSwQue = false;
        public bool m_bBnsNmTrigger = true;
        public string m_strTempGross = string.Empty;
        public string m_strTmpBnsName = string.Empty;
        public string m_sZCNo = string.Empty;
        private MoneyTextValidator m_objMoneyValidator;
        frmBilling BillingForm = new frmBilling();
        private int m_iBranch = 0;  // RMC 20110725
        public string m_strTempCapital = string.Empty;  // RMC 20110725
        public string m_strOperationStart;  // RMC 20110725
        public string m_strInspectionNo = string.Empty; // RMC 20110816
        public bool m_bOwnProfileEdited = false; //JARS 20170807
        public string m_sTmpBuss_Own_Code = string.Empty;
        public string m_sTmpBuss_BusnCode = string.Empty;
        public string m_sTmpBuss_Prev_BnsOwn = string.Empty;
        // blob
        protected frmImageList m_frmImageList;
        public static int m_intImageListInstance;
        // blob
        //MCR 20141222 (s)
        private bool m_bIsNewDiscovery = false;
        public bool NewDiscovery
        {
            get { return m_bIsNewDiscovery; }
            set { m_bIsNewDiscovery = value; }
        }
        //MCR 20141222 (e)
        public bool m_bAddNew = false;

        public KryptonButton ButtonBnsOwnSearch
        {
            get { return this.btnBnsOwnSearch; }
            set { this.btnBnsOwnSearch = value; }
        }

        public string GetPage1OwnCode
        {
            get { return m_sBuss_Own_Code; }
            set { m_sBuss_Own_Code = value; }
        }

        public string GetPage2OwnCode
        {
            get { return m_sBuss_BusnCode; }
            set { m_sBuss_BusnCode = value; }
        }

        public string GetPage3OwnCode
        {
            get { return m_sBuss_Prev_BnsOwn; }
            set { m_sBuss_Prev_BnsOwn = value; }
        }

        bool bPage1 = true;
        public int m_iState = 0;
        public DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin

        public void FormState(int iState)
        {

            if (iState == 0)
            {
                // BussRecAdd
                bin1.Enabled = false;
                m_iState = 0;
            }
            else if (iState == 1)
            {
                // BussRecEdit
                //txtBnsAddNo.Enabled = false;
                m_iState = 1;
            }
            else if (iState == 2)
            {
                // BussRecDelete
                //txtBnsAddNo.Enabled = false;
                m_iState = 2;
            }
        }

        public frmBusinessRecord()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
            txtLGUCode.Text = bin1.GetLGUCode;
            txtDistCode.Text = bin1.GetDistCode;
            LoadBrgy();
            btnAddlBns.Enabled = false;
            m_objMoneyValidator = new MoneyTextValidator();
            
            // blob
            m_intImageListInstance = 0;
            m_frmImageList = new frmImageList();
            m_frmImageList.IsBuildUpPosting = true;
            // blob
        }

        private void frmBusinessRecord_Load(object sender, EventArgs e)
        {
            // RMC 20150425 disabled viewing of controls for business mapping in business record module (s)
            OracleResultSet pGisRec = new OracleResultSet();
            try
            {
                pGisRec.CreateNewConnectionGIS();
            }
            catch
            {
                chkGISLink.Visible = false;
                lblPIN.Visible = false;
                cmbLandPIN.Visible = false;
                lblBldgCode.Visible = false;
                cmbBldgCode.Visible = false;
                lblBldgName.Visible = false;
                txtBldgName.Visible = false;
            }
            // RMC 20150425 disabled viewing of controls for business mapping in business record module (e)

            m_strTempBIN = "";
            m_strTmpBnsCode = "";
            m_strBnsCode = "";
            m_strTempGross = "";
            m_strTmpBnsName = "";
            m_strTempCapital = "";  // RMC 20110725
            m_strOperationStart = "";  // RMC 20110725
            txtBnsMun.Text = AppSettingsManager.GetConfigValue("02");
            txtBnsProv.Text = AppSettingsManager.GetConfigValue("08");
            txtOwnMun.Text = AppSettingsManager.GetConfigValue("02");
            txtOwnProv.Text = AppSettingsManager.GetConfigValue("08");
            txtPINMun.Text = AppSettingsManager.GetConfigValue("02");
            txtPINProv.Text = AppSettingsManager.GetConfigValue("08");
            dtpMPDate.Value = AppSettingsManager.GetCurrentDate();  // RMC 20110725
            //dtpDTIIssued.Value = AppSettingsManager.GetCurrentDate();  // RMC 20110725 // AST 20150325 Req'd by Gob
            //dtpSSSIssued.Value = AppSettingsManager.GetCurrentDate();  // RMC 20110725 // AST 20150325 Req'd by Gob
            //dtpCTCNoIssued.Value = AppSettingsManager.GetCurrentDate();  // RMC 20110725 // AST 20150325 Req'd by Gob
            //dtpTINNoIssued.Value = AppSettingsManager.GetCurrentDate();  // RMC 20110725 // AST 20150325 Req'd by Gob
            dtpOperationStart.Value = AppSettingsManager.GetCurrentDate();  // RMC 20110725
            dtpApplicationDate.Value = AppSettingsManager.GetCurrentDate(); // RMC 20110809
            btnAddNew.Enabled = false;  // RMC 20111006 Added 'Add New' button in Business Records-Add module
            btnForm.Enabled = false;
            txtTaxYear.Text = "";   // RMC 20161208 reset tax year value in business record

            // RMC 20150104 addl mods (s)
            if (m_strInspectionNo != "")
                m_strTempBIN = m_strInspectionNo;
            // RMC 20150104 addl mods (e)

            if (m_sFormStatus == "BUSS-ADD-NEW")
            {
                bin1.txtTaxYear.ReadOnly = true;
                bin1.txtBINSeries.ReadOnly = true;
                btnSearchBIN.Enabled = false;
                txtTaxYear.Focus();
                this.ActiveControl = txtTaxYear;
                this.dtpMPDate.Enabled = true;
                btnAddNew.Text = "Add another record";
                btnViewFiles.Enabled = true;   // RMC 20111207

                // RMC 20111128 consider Business mapping - Unencoded in Business Records-Add (s)
                if (m_strInspectionNo != "")
                {
                    LoadValues();
                } // RMC 20150114 enabled
                // RMC 20141222 modified permit printing, put rem
                // RMC 20111128 consider Business mapping - Unencoded in Business Records-Add (e)
                this.LoadImageList(); // CJC 20130401
            }

            if (m_sFormStatus == "BUSS-EDIT")
            {
                bin1.Enabled = true;
                btnSearchBIN.Enabled = true;
                bin1.txtTaxYear.Focus();
                this.ActiveControl = bin1.txtTaxYear;
                BackCodeClass backCode = new BackCodeClass(this);
                backCode.ManageControls(1, false);
                backCode.ManageControls(2, false);
                backCode.ManageControls(3, false);
                //txtCurrentGross.ReadOnly = false;
                btnSave.Text = "Update";
                this.dtpMPDate.Enabled = true;
                btnAddNew.Text = "Edit another record";
                btnViewFiles.Enabled = true;   // RMC 20111207

                
            }

            //JARS 20170822
            if(AppSettingsManager.GetConfigValue("10") == "216")
            {
                txtBnsProv.Visible = false;
                label13.Visible = false;
            }

            if (m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")
            {
                txtOldBin.ReadOnly = true; //MCR 20150114
                bin1.Enabled = false;
                btnSearchBIN.Enabled = false;
                //txtTaxYear.Enabled = false;
                txtTaxYear.Text = ConfigurationAttributes.CurrentYear;
                txtTaxYear.ReadOnly = true;
                txtMPNo.ReadOnly = true;
                txtBussPlate.ReadOnly = true;   // RMC 20110819 added capturing/viewing of Business Plate
                txtBussPlate.Enabled = false;
                txtBnsName.Focus();
                this.ActiveControl = txtBnsName;
                cmbBnsStat.Text = "NEW";
                cmbBnsStat.Enabled = false;
                txtPrevGross.ReadOnly = true;
                txtCurrentGross.ReadOnly = true;
                btnCheckList.Enabled = true;
                btnAddNew.Text = "Add another record";
                btnViewFiles.Enabled = true;   // RMC 20111207
                txtBussPlate.ReadOnly = false;   // RMC 20120104 enable buss plate in new app

                
                //MCR 20141222 (s)
                if (m_sFormStatus == "NEW-APP") 
                {
                    int iTaxYear; 
                    int.TryParse(ConfigurationAttributes.CurrentYear, out iTaxYear);
                    if (m_bIsNewDiscovery == true)
                    {
                        txtTaxYear.Text = string.Format("{0:0000}", iTaxYear - 1);
                        txtTaxYear.ReadOnly = false;
                    }
                }
                //MCR 20141222 (e)

                // RMC 20110816 (s)
                if (m_strInspectionNo != "")
                {
                    LoadValues();
                }
                // RMC 20110816 (e)
            }

            if (m_sFormStatus == "NEW-APP-EDIT" || m_sFormStatus == "SPL-APP-EDIT")
            {
                bin1.Enabled = true;
                btnSearchBIN.Enabled = true;
                bin1.txtTaxYear.Focus();
                this.ActiveControl = bin1.txtTaxYear;
                BackCodeClass backCode = new BackCodeClass(this);
                backCode.ManageControls(1, false);
                backCode.ManageControls(2, false);
                backCode.ManageControls(3, false);
                cmbBnsStat.Enabled = false;
                txtPrevGross.ReadOnly = true;
                txtCurrentGross.ReadOnly = true;
                btnSave.Text = "Update";
                //btnCheckList.Visible = true;    // RMC 20110906
                btnCheckList.Enabled = true;    // RMC 20110906
                btnAddNew.Text = "Edit another record";
                btnViewFiles.Enabled = true;   // RMC 20111207

                string sTmpDate = "";
                string sTmpBin = "";
                AppPermitNo(out sTmpDate, out sTmpBin, "NEW"); //MCR 20150114
                txtOldBin.ReadOnly = true; //MCR 20150114
            }

            if (m_sFormStatus == "REN-APP")
            {
                txtOldBin.ReadOnly = true; //MCR 20150114
                bin1.Enabled = true;
                btnSearchBIN.Enabled = true;
                bin1.txtTaxYear.Focus();
                this.ActiveControl = bin1.txtTaxYear;
                BackCodeClass backCode = new BackCodeClass(this);
                backCode.ManageControls(1, false);
                backCode.ManageControls(2, false);
                //backCode.ManageControls(3, false);    // RMC 20110809
                txtCurrentGross.ReadOnly = false;
                btnAddlBnsInfo.Enabled = true;
                btnSave.Text = "Save";
                cmbBnsStat.Enabled = false; // RMC 20110824     
                btnCheckList.Enabled = true;    // RMC 20110906
                btnAddNew.Text = "Add another record";
                btnViewFiles.Enabled = true;   // RMC 20111207
            }

            if (m_sFormStatus == "REN-APP-EDIT")
            {
                bin1.Enabled = true;
                btnSearchBIN.Enabled = true;
                bin1.txtTaxYear.Focus();
                this.ActiveControl = bin1.txtTaxYear;
                BackCodeClass backCode = new BackCodeClass(this);
                backCode.ManageControls(1, false);
                backCode.ManageControls(2, false);
                backCode.ManageControls(3, false);
                txtCurrentGross.ReadOnly = false;
                txtMPNo.ReadOnly = true;
                txtTaxYear.ReadOnly = true;
                cmbBnsStat.Enabled = false;
                txtInitialCap.ReadOnly = true;
                btnSave.Text = "Update";
                //btnCheckList.Visible = true;    // RMC 20110906
                btnCheckList.Enabled = true;    // RMC 20110906
                btnAddNew.Text = "Edit another record";
                btnViewFiles.Enabled = true;   // RMC 20111207

                string sTmpDate = "";
                string sTmpBin = "";
                AppPermitNo(out sTmpDate, out sTmpBin, "REN"); //MCR 20150114
                txtOldBin.ReadOnly = true; //MCR 20150114
            }


            if (m_sFormStatus == "BUSS-DELETE")
            {
                bin1.Enabled = true;
                btnSearchBIN.Enabled = true;
                bin1.txtTaxYear.Focus();
                this.ActiveControl = bin1.txtTaxYear;
                BackCodeClass backCode = new BackCodeClass(this);
                backCode.ManageControls(1, false);
                backCode.ManageControls(2, false);
                backCode.ManageControls(3, false);
                btnSave.Text = "Delete";
                btnViewFiles.Enabled = true;   // RMC 20111207

                btnDetachFiles.Enabled = false;
                btnAttachFiles.Enabled = false;
            }

            if (m_sFormStatus == "BUSS-UPDATE")
            {
                bin1.Enabled = true;
                btnSearchBIN.Enabled = true;
                bin1.txtTaxYear.Focus();
                this.ActiveControl = bin1.txtTaxYear;
                BackCodeClass backCode = new BackCodeClass(this);
                backCode.ManageControls(1, false);
                backCode.ManageControls(2, false);
                backCode.ManageControls(3, false);
                txtCurrentGross.ReadOnly = false;
                btnSave.Text = "Update";
                btnViewFiles.Enabled = true;   // RMC 20111207

            }

            if (m_sFormStatus == "BUSS-CANCEL-UPDATE" || m_sFormStatus == "CANCEL-APP")
            {
                bin1.Enabled = true;
                btnSearchBIN.Enabled = true;
                bin1.txtTaxYear.Focus();
                this.ActiveControl = bin1.txtTaxYear;
                BackCodeClass backCode = new BackCodeClass(this);
                backCode.ManageControls(1, false);
                backCode.ManageControls(2, false);
                backCode.ManageControls(3, false);
                //txtCurrentGross.ReadOnly = false;
                txtCurrentGross.ReadOnly = true;    // RMC 20110809
                btnBnsType.Enabled = false; // RMC 20110809
                btnAddlBns.Enabled = false; // RMC 20110809
                btnSave.Text = "Void";
                btnViewFiles.Enabled = true;   // RMC 20111207
                btnDetachFiles.Enabled = false;
                btnAttachFiles.Enabled = false;
            }

            timerBnsRec.Enabled = true;
            if(cmbPlaceBnsStat.Text == "")  // RMC 20111128
                cmbPlaceBnsStat.Text = "OWNED";
        }

        private void AppPermitNo(out string sAppDate, out string sAppNo, string sType)//MCR 20150114
        {
            sAppNo = "";
            sAppDate = "";
            OracleResultSet pSet = new OracleResultSet();
            //If Exist
            if (sType == "NEW")
                pSet.Query = "select * from app_permit_no_new where bin = '" + bin1.GetBin() + "' and year = '" + AppSettingsManager.GetSystemDate().Year + "'";
            else
                pSet.Query = "select * from app_permit_no where bin = '" + bin1.GetBin() + "' and year = '" + AppSettingsManager.GetSystemDate().Year + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sAppNo = pSet.GetString(1);
                    sAppDate = pSet.GetString(3);
                }
            }
            pSet.Close();

            if (sAppNo == "")
            {
                //Get MaxSeries
                if (sType == "NEW")
                    pSet.Query = "select coalesce(max(to_number(substr(app_no,6,10))),0) + 1 as Series from app_permit_no_new where year = '" + AppSettingsManager.GetSystemDate().Year + "'";
                else
                    pSet.Query = "select coalesce(max(to_number(substr(app_no,6,10))),0) + 1 as Series from app_permit_no where year = '" + AppSettingsManager.GetSystemDate().Year + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sAppNo = AppSettingsManager.GetSystemDate().Year + "-" + pSet.GetInt(0).ToString("00000");
                pSet.Close();
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        public void btnSearchBIN_Click(object sender, EventArgs e)
        {
            //LoadValues(bin1.GetBin()); // GDE 20100222
            BackCodeClass backCode = new BackCodeClass(this);

            if (btnSearchBIN.Text == "Search Bin")
            {
                // GDE added (s){
                if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
                {
                    if (!TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "ADD", "ASS"))
                    {
                        // RMC 20171115 merged validation of violation in renewal application from Binan (s)
                        //MCR 20171006 BIN-17-7858 (s)
                        if (m_sFormStatus == "REN-APP")
                        {
                            if (CheckifTaggedViolation())
                            {
                                if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
                                {
                                }

                                bin1.txtTaxYear.ReadOnly = false;
                                bin1.txtBINSeries.ReadOnly = false;
                                bin1.txtTaxYear.Text = "";
                                bin1.txtBINSeries.Text = "";

                                backCode.ClearControls(1);
                                backCode.ClearControls(2);
                                btnSearchBIN.Text = "Search Bin";
                                return;
                            }
                        }
                        //MCR 20171006 BIN-17-7858 (e)
                        // RMC 20171115 merged validation of violation in renewal application from Binan (e)

                        LoadValues(bin1.GetBin());
                        btnAddlBns.Enabled = true;
                        m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    }
                    else
                    {
                        bin1.txtTaxYear.Text = "";
                        bin1.txtBINSeries.Text = "";
                        backCode.ClearControls(1);
                        btnAddlBns.Enabled = false;
                        m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    }
                }
                else
                {
                    frmSearchBns = new frmSearchBusiness();
                    frmSearchBns.ModuleCode = m_sFormStatus;

                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                        if (!TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "ADD", "ASS"))
                        {
                            LoadValues(bin1.GetBin());
                            btnAddlBns.Enabled = true;
                        }
                        else
                        {
                            bin1.txtTaxYear.Text = "";
                            bin1.txtBINSeries.Text = "";
                            backCode.ClearControls(1);
                            btnAddlBns.Enabled = false;
                        }
                    }
                }
                // GDE added (e)}

                if (bin1.txtTaxYear.Text != "" && bin1.txtBINSeries.Text != "")
                {
                    bin1.txtTaxYear.ReadOnly = true;
                    bin1.txtBINSeries.ReadOnly = true;
                }

                // RMC 20110824 (s)
                if (m_sFormStatus == "REN-APP")
                    txtMPNo.Text = "";
                // RMC 20110824 (e)
                
            }
            else
            {
                // RMC 20141217 adjustments (s)
                OracleResultSet pCmd = new OracleResultSet();
                pCmd.Query = "delete from addl_info_tmp where bin = '" + bin1.GetBin() + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
                // RMC 20141217 adjustments (e)

                if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
                {
                }

                bin1.txtTaxYear.ReadOnly = false;
                bin1.txtBINSeries.ReadOnly = false;
                bin1.txtTaxYear.Text = "";
                bin1.txtBINSeries.Text = "";
                
                backCode.ClearControls(1);
                backCode.ClearControls(2);
                btnSearchBIN.Text = "Search Bin";

                if (m_sFormStatus == "BUSS-EDIT" || m_sFormStatus == "REN-APP"
                    || m_sFormStatus == "REN-APP" || m_sFormStatus == "BUSS-DELETE"
                    || m_sFormStatus == "BUSS-UPDATE" || m_sFormStatus == "BUSS-CANCEL-UPDATE" 
                    || m_sFormStatus == "CANCEL-APP")
                {
                    backCode.ManageControls(1, false);
                    backCode.ManageControls(2, false);
                    backCode.ManageControls(3, false);
                }

                if (m_sFormStatus == "BUSS-EDIT" || m_sFormStatus == "NEW-APP-EDIT" || m_sFormStatus == "SPL-APP-EDIT"
                    || m_sFormStatus == "REN-APP" || m_sFormStatus == "REN-APP"
                    || m_sFormStatus == "BUSS-DELETE" || m_sFormStatus == "BUSS-CANCEL-UPDATE"
                    || m_sFormStatus == "CANCEL-APP")
                    bin1.txtTaxYear.Focus();

                if (m_sFormStatus == "BUSS-ADD-NEW")
                    txtTaxYear.Focus();

                if (m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")
                    txtBnsName.Focus();

                m_blnInitialLoad = true;
                btnSave.Enabled = false;
                btnCheckList.Enabled = false;    // RMC 20110906 Added saving of Application requirements in Business Records
                btnBTMDeficiency.Enabled = false;
                btnForm.Enabled = false;
                LoadLandPIN(cmbBnsBrgy.Text.ToString()); 
                m_frmImageList.Close();
            }

            if (m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP"
                || m_sFormStatus == "NEW-APP-EDIT" || m_sFormStatus == "SPL-APP-EDIT"
                || m_sFormStatus == "REN-APP" || m_sFormStatus == "REN-APP-EDIT")
                txtOldBin.ReadOnly = true;

        }


        private void LoadBrgy()
        {

            pSet.Query = "SELECT DISTINCT(TRIM(BRGY_NM)) FROM BRGY  ORDER BY TRIM(BRGY_NM) ASC";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbBnsBrgy.Items.Add(pSet.GetString(0).Trim());
                    cmbOwnBrgy.Items.Add(pSet.GetString(0).Trim());
                    cmbPINBrgy.Items.Add(pSet.GetString(0).Trim());
                    cmbBnsOwnBrgy.Items.Add(pSet.GetString(0).Trim());
                    cmbPrevOwnBrgy.Items.Add(pSet.GetString(0).Trim());

                }
            }
            pSet.Close();
        }

        private void LoadValues(string sBin)
        {
            BackCodeClass backCode = new BackCodeClass(this);
            backCode.ReturnValuesByBin(sBin);
            
            m_blnInitialLoad = false;
            if (!this.ValidateRecord(sBin))
            {
                // RMC 20110809 
                if (TaskMan.IsObjectLock(sBin, m_sFormStatus, "DELETE", "ASS"))
                {
                }
                // RMC 20110809

                bin1.txtTaxYear.Text = "";
                bin1.txtBINSeries.Text = "";
                backCode.ClearControls(1);
                btnAddlBns.Enabled = false;
                btnSearchBIN.Text = "Search Bin";
                btnSave.Enabled = false;

                return;
            }
            else
            {
                m_strTmpBnsName = txtBnsName.Text.Trim();
                //btnSave.Enabled = true;
                btnSearchBIN.Text = "Clear Fields";
                m_strCapital = string.Format("{0:#,##.00}", Convert.ToDouble(txtInitialCap.Text));
                m_strGross = string.Format("{0:#,##.00}", Convert.ToDouble(txtCurrentGross.Text));

                if (m_sFormStatus == "BUSS-EDIT" || m_sFormStatus == "SPL-APP-EDIT" || m_sFormStatus == "REN-APP-EDIT" || m_sFormStatus == "NEW-APP-EDIT")
                {
                    backCode.ManageControls(1, true);
                    backCode.ManageControls(2, true);
                    backCode.ManageControls(3, true);

                    if (m_sFormStatus == "NEW-APP-EDIT")
                    {
                        txtTaxYear.ReadOnly = true;
                        txtMPNo.ReadOnly = true;
                        dtpMPDate.Enabled = false;
                        //txtBussPlate.ReadOnly = true;   // RMC 20110819 added capturing/viewing of Business Plate
                        txtBussPlate.ReadOnly = false;  // RMC 20120104 enable buss plate in new app
                    }
                }

                if (m_sFormStatus == "REN-APP-EDIT" || m_sFormStatus == "REN-APP")
                {
                    txtBnsType.ReadOnly = true;
                    btnBnsType.Enabled = false;
                    btnBnsTypeAll.Enabled = false;
                }
                else
                {
                    if (CheckPayment(bin1.GetBin(), AppSettingsManager.GetConfigValue("12")))
                    {
                        txtBnsType.ReadOnly = true;
                        btnBnsType.Enabled = false;
                        btnBnsTypeAll.Enabled = false;
                    }
                    else
                    {
                        txtBnsType.ReadOnly = false;
                        btnBnsType.Enabled = true;
                        btnBnsTypeAll.Enabled = true;
                    }
                }

                /*if (CheckPayment(bin1.GetBin(), txtTaxYear.Text.Trim()))
                {
                    txtInitialCap.ReadOnly = true;
                    txtCurrentGross.ReadOnly = true;
                }
                else
                {
                    txtInitialCap.ReadOnly = false;
                    txtCurrentGross.ReadOnly = false;
                }*/

                if (m_sFormStatus == "REN-APP")
                    btnForm.Enabled = true;
                else
                    btnForm.Enabled = false;

                LoadGisValues(bin1.GetBin());

                
                
            }
        }

        private void bin1_KeyPress(object sender, KeyPressEventArgs e)
        {
            bin1.GetLGUCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.LGUCode);
            bin1.GetDistCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.DistCode);
        }

        private void btnBnsType_Click(object sender, EventArgs e)
        {
            frmBusinessType frmBnsType = new frmBusinessType();
            frmBnsType.SetFormStyle(true);
            frmBnsType.ShowDialog();
            txtBnsType.Text = frmBnsType.m_sBnsDescription;
            m_strBnsCode = frmBnsType.m_strBnsCode;
        }

        private void btnBnsTypeAll_Click(object sender, EventArgs e)
        {
            frmBusinessType frmBnsType = new frmBusinessType();
            frmBnsType.SetFormStyle(false);
            frmBnsType.ShowDialog();
            txtBnsType.Text = frmBnsType.m_sBnsDescription;
            m_strBnsCode = frmBnsType.m_strBnsCode;
        }

        private void txtTaxYear_Leave(object sender, EventArgs e)
        {
            if (m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")
                txtMPYear.Text = "";
            else
                txtMPYear.Text = txtTaxYear.Text;
        }

        private void txtMPNo_Leave(object sender, EventArgs e)
        {
            int iCount = 0;
            iCount = txtMPNo.TextLength;

            switch (iCount)
            {
                case 1:
                    {
                        txtMPNo.Text = "0000" + txtMPNo.Text;
                        break;
                    }
                case 2:
                    {
                        txtMPNo.Text = "000" + txtMPNo.Text;
                        break;
                    }
                case 3:
                    {
                        txtMPNo.Text = "00" + txtMPNo.Text;
                        break;
                    }
                case 4:
                    {
                        txtMPNo.Text = "0" + txtMPNo.Text;
                        break;
                    }
                case 5:
                    {
                        txtMPNo.Text = txtMPNo.Text;
                        break;
                    }
            }

            if (m_sFormStatus == "BUSS-ADD-NEW" || m_sFormStatus == "BUSS-EDIT")    // RMC 20111007 Modified validation of duplicate permit no
            {
                if(txtMPNo.Text.Trim() != "")
                {
                    string strMP = string.Empty;
                    string strRecBin = string.Empty;

                    strMP = txtMPYear.Text.Trim() + "-" + txtMPNo.Text.Trim();

                    pSet.Query = string.Format("select * from businesses where permit_no = '{0}'", strMP);
                    // RMC 20111007 Modified validation of duplicate permit no (s)
                    if (bin1.GetBin().Length == 19) 
                        pSet.Query += string.Format(" and bin <> '{0}'", bin1.GetBin());
                    // RMC 20111007 Modified validation of duplicate permit no (e)
                    if(pSet.Execute())
                    {
                        if(pSet.Read())
                        {
                            strRecBin = pSet.GetString("bin");

                            /*if (MessageBox.Show("Duplicate Permit Number with BIN: " + strRecBin + ".\n Continue Anyway?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                txtMPNo.Text = "";
                                txtMPNo.Focus();
                            }
                            */ // RMC 20170130 block record if duplicate permit no. detected

                            // RMC 20170130 block record if duplicate permit no. detected (s)
                            MessageBox.Show("Duplicate Permit Number with BIN: " + strRecBin + ".", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            txtMPNo.Text = "";
                            txtMPNo.Focus();
                            return;
                            // RMC 20170130 block record if duplicate permit no. detected (e)
                        }
                    }
                    pSet.Close();
                    
                }
            }

        }



        private void tabBnsRec_SelectedIndexChanged(object sender, EventArgs e)
        {

            BackCodeClass backCode = new BackCodeClass(this);
            BPLSAppSettingList sList = new BPLSAppSettingList();
            string sTabTag = string.Empty;
            //sTabTag = tabBnsRec.SelectedTab.Tag.ToString();

            /* sTabTag = 0 - Page 1;
             * sTabTag = 1 - Page 2;
             * sTabTag = 2 - Page 3;*/

            // AST 20150311 Added this block (s)
            bool blnExecute = false;
            if (sender.Equals(btnBack))
            {
                if (tabBnsRec.SelectedTab == tabPage3)
                {
                    sTabTag = "1";
                }
                else if (tabBnsRec.SelectedTab == tabPage2)
                {
                    sTabTag = "0";
                }
            }
            else if (sender.Equals(btnNext))
            {
                if (tabBnsRec.SelectedTab == tabPage1)
                {
                    sTabTag = "1";
                }
                else if (tabBnsRec.SelectedTab == tabPage2)
                {
                    sTabTag = "2";
                }
            }
            else
            {
                sTabTag = tabBnsRec.SelectedTab.Tag.ToString();
            }

            if (blnExecute)
                return;
            // AST 20150311 Added this block (e)

            if (sTabTag == "1" || sTabTag == "2")
            {
                if (m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP" || m_sFormStatus == "BUSS-ADD-NEW"
                    || m_sFormStatus == "NEW-APP-EDIT" || m_sFormStatus == "SPL-APP-EDIT" || m_sFormStatus == "REN-APP"
                    || m_sFormStatus == "REN-APP-EDIT" || m_sFormStatus == "CANCEL-APP")
                {
                    backCode.PlaceOccupancy = sTabTag;
                    backCode.PageOneCheck(m_sFormStatus);
                }
                else
                {
                    backCode.PlaceOccupancy = sTabTag;
                    backCode.PageOneCheck("");
                }
                if (backCode.m_sPage1 == "FALSE")
                {
                    tabBnsRec.SelectTab(tabPage1);
                    MessageBox.Show(backCode.m_sMessage, "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    backCode.m_sMessage = string.Empty;
                    return;
                }

                if (sTabTag == "1" || sTabTag == "2")
                {
                    backCode.PageTwoCheck();

                    if (backCode.m_sPage2 == "FALSE")
                    {
                        tabBnsRec.SelectTab(tabPage2);
                        MessageBox.Show(backCode.m_sMessage, "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        backCode.m_sMessage = string.Empty;
                        return;
                    }
                }

                if (sTabTag == "2")
                {
                    backCode.PageThreeCheck(false);
                }

                //btnSave.Enabled = true;
            }

            // AST 20150311 Added this block (s)
            if (sTabTag == "0" && tabBnsRec.SelectedTab != tabPage1)
            {
                blnExecute = true;
                tabBnsRec.SelectTab(tabPage1);
            }
            else if (sTabTag == "1" && tabBnsRec.SelectedTab != tabPage2)
            {
                blnExecute = true;
                tabBnsRec.SelectTab(tabPage2);
            }
            else if (sTabTag == "2" && tabBnsRec.SelectedTab != tabPage3)
            {
                blnExecute = true;
                tabBnsRec.SelectTab(tabPage3);
            }
            // AST 20150311 Added this block (e)

            // AST 20150312 Change Caption of Button "Next" in Page 3 to Close (s)            
            if (sTabTag == "2")
            {
                btnNext.Visible = false;
                btnClose.Visible = true;
            }
            else
            {
                btnNext.Visible = true;
                btnClose.Visible = false;
            }
            // AST 20150312 Change Caption of Button "Next" in Page 3 to Close (e)
        }


        private void btnSearchOwner_Click(object sender, EventArgs e)
        {
            frmSearchOwner SearchOwner = new frmSearchOwner();
            //SearchOwner.m_sPageWatch = "";
            SearchOwner.m_sPageWatch = "PAGE2"; // RMC 20150807 modified searching of owners in Business records
            SearchOwner.ShowDialog();

            txtOwnCode.Text = SearchOwner.m_strOwnCode;
            txtOwnLn.Text = SearchOwner.m_sOwnLn;
            txtOwnFn.Text = SearchOwner.m_sOwnFn;
            txtOwnMi.Text = SearchOwner.m_sOwnMi;
            txtOwnAddNo.Text = SearchOwner.m_sOwnAdd;
            txtOwnStreet.Text = SearchOwner.m_sOwnStreet;
            cmbOwnBrgy.Text = SearchOwner.m_sOwnBrgy;
            cmbOwnDist.Text = SearchOwner.m_sOwnDist;
            txtOwnMun.Text = SearchOwner.m_sOwnMun;
            txtOwnProv.Text = SearchOwner.m_sOwnProv;
            txtOwnZip.Text = SearchOwner.m_sOwnZip;

            
        }

        private void txtBnsType_Leave(object sender, EventArgs e)
        {
            if (txtBnsType.ReadOnly == false)
            {
                if (txtBnsType.Text.ToString().Trim() != "")    // RMC 20110803
                {
                    BackCodeClass backCode = new BackCodeClass(this);
                    //m_strBnsCode = txtBnsType.Text.ToString().Trim(); //?? 

                    //txtBnsType.Text = backCode.GetBnsDesc(txtBnsType.Text.Trim()); // ?? 
                    txtBnsType.Text = backCode.GetBnsDesc(m_strBnsCode); // AST 20150410 Added This Line
                }
            }
        }



        private void btnClosedBns_Click(object sender, EventArgs e)
        {
        }

        private void cmbPlaceBnsStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            BackCodeClass backCode = new BackCodeClass(this);
            if (cmbPlaceBnsStat.Text.Trim() == "OWNED")
            {
                backCode.ManageControls(2, false);
                backCode.PageTwoCheck();
            }
            else
            {
                backCode.ManageControls(2, true);
                backCode.ClearControls(2);
            }

        }

        private void dtpMPDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void timerBnsRec_Tick(object sender, EventArgs e)
        {
            if (bState)
            {
                if (this.Opacity > .10)
                {
                    timerBnsRec.Enabled = true;
                    this.Opacity = this.Opacity - .10;
                }
                else
                {
                    this.Opacity = 0;
                    timerBnsRec.Enabled = false;

                    
                    this.Close();
                }
            }
            else
            {
                if (this.Opacity < 1.0)
                    this.Opacity = this.Opacity + .10;
                else
                    timerBnsRec.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
        }

        
        private void btnSearchLandPIN_Click(object sender, EventArgs e)
        {

        }//GMC 20150819 Accept Object and eventargs from BPS mainform(s)
        public void ObjectPass(Object sender,EventArgs e) {
            this.sender = sender;
            this.e = e;
        }//GMC 20150819 Accept Object and eventargs from BPS mainform(e)
        private void btnSave_Click(object sender, EventArgs e)
        {
            BackCodeClass backClass = new BackCodeClass(this);

            backClass.PageOneCheck(m_sFormStatus);
            backClass.PageTwoCheck();
            backClass.PageThreeCheck(true);

            if (backClass.m_sPage1 == "FALSE")
            {
                tabBnsRec.SelectTab(tabPage1);
                MessageBox.Show(backClass.m_sMessage, "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                backClass.m_sMessage = string.Empty;
                return;
            }

            if (backClass.m_sPage2 == "FALSE")
            {
                tabBnsRec.SelectTab(tabPage2);
                MessageBox.Show(backClass.m_sMessage, "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                backClass.m_sMessage = string.Empty;
                return;
            }

            if (backClass.m_sPage3 == "FALSE")
            {
                tabBnsRec.SelectTab(tabPage3);
                MessageBox.Show(backClass.m_sMessage, "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                backClass.m_sMessage = string.Empty;
                return;
            }

            if (m_sFormStatus == "NEW-APP")
            {
                if (MessageBox.Show("Save this record?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backClass.AppNewSave();
                    SaveImage();
                    /*
                    //GMC 20150819 Prompt to add new application again(s)
                    if (MessageBox.Show("Add New Application Again?", "New Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bAddNewApp = true;
                        this.Dispose();
                        this.Close();
                    }
                    else
                        this.Close();
                    //GMC 20150819 Prompt to add new application again(e)
                     */
                    // RMC 20161218 removed continuous application option, causes error
                }
               // btnAddNew.Visible = true; pendign
               // btnAddNew.Text = "Add New"; pending
            }
            else if (m_sFormStatus == "SPL-APP")
            {
                if (MessageBox.Show("Save this record?", "Special Business", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backClass.AppNewSave();
                    SaveImage();
                }
                // btnAddNew.Visible = true; pending
                // btnAddNew.Text = "Add New"; pending
            }
            else if (m_sFormStatus == "NEW-APP-EDIT")
            {
                if (MessageBox.Show("Update this record?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backClass.AppNewUpdate();
                    SaveImage();
                }
            }
            else if (m_sFormStatus == "SPL-APP-EDIT")
            {
                if (MessageBox.Show("Update this record?", "Special Business", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backClass.AppNewUpdate();
                    SaveImage(); 
                }
            }
            else if (m_sFormStatus == "BUSS-ADD-NEW")
            {
                if (MessageBox.Show("Save this record?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backClass.BussSave();
                    SaveImage();

                    m_frmImageList.Close(); // CJC 20130401
                }
            }
            else if (m_sFormStatus == "REN-APP" || m_sFormStatus == "REN-APP-EDIT") //JARS 20171023 ON SITE: INCLUDED RENEWAL APPLICATION EDIT
            {
                //if (MessageBox.Show("Save renewal application?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // JAV 20170928 DTI Expiration
                //{ // JAV 20170928 DTI Expiration
                try //JARS 20171019 QA FIX FOR INPUT STRING NOT IN A CORRECT FORMAT MESSAGE
                {
                    DateTime dtDateNow = DateTime.Now; // JAV 20170928 DTI Expiration
                    //DateExpiration(dtpDTIIssued.Value, dtDateNow); // JAV 20170928 DTI Expiration
                    DateExpiration(dtpDTIIssued.Value,AppSettingsManager.GetCurrentDate()); // RMC 20171116 correction in validating DTI expiration
                }
                catch
                {
                    //MessageBox.Show("Please input in your SYSTEM CONFIG the expiration of DTI registrations", "Application", MessageBoxButtons.OK, MessageBoxIcon.Error); // RMC 20171116 correction in validating DTI expiration, transferred
                    //this.Close();
                }
                #region comments
                //DateTime dtDateNow = DateTime.Now; // JAV 20170928 DTI Expiration
                    //DateExpiration(dtpDTIIssued.Value, dtDateNow); // JAV 20170928 DTI Expiration

                   //backClass.AppRenSave(); // JAV 20170928 DTI Expiration
                    //SaveImage();// JAV 20170928 DTI Expiration

                    /*
                   // RMC 20161129 customized application form for Binan (s)

                   if (MessageBox.Show("Print Application Form?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                   {
                       string sAppDate = "";
                       string sAppNo = "";
                       AppPermitNo(out sAppDate, out sAppNo, "REN");

                       frmAppForm PrintAppForm = new frmAppForm();
                       PrintAppForm.BIN = bin1.GetBin();
                       PrintAppForm.AppNo = sAppNo;
                       PrintAppForm.AppDate = sAppDate;
                       PrintAppForm.ApplType = AppSettingsManager.GetBnsStatonBnsQue(bin1.GetBin().Trim());
                       PrintAppForm.bIsRePrint = false;
                       PrintAppForm.ShowDialog();
                   }
                  
                   // RMC 20161129 customized application form for Binan (e)
                     */ // RMC 20161208 removed printing of application after renewal application based on training

                    
                   //GMC 20150819 Prompt to renew application again(s)
                   /*if (MessageBox.Show("Renew Application Again?", "Renew Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                   {
                       bAddNewApp = true;
                       this.Dispose();
                       this.Close();
                   }
                   else
                       this.Close();
                   //GMC 20150819 Prompt to renew application again(e)
                    */
                // RMC 20161218 removed continuous application option, causes error
                #endregion
                /*if (CheckAddlBns())
                       btnAddlBns_Click(sender, e);*/
                // RMC 20180103 corrected error in Renewal application of additional line, put rem

                   
                //}
            }
            else if (m_sFormStatus == "REN-APP-EDIT")
            {
                if (MessageBox.Show("Save renewal application?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backClass.AppRenUpdate();
                    SaveImage(); 

                    if (CheckAddlBns())
                        btnAddlBns_Click(sender, e);
                }
            }
            else if (m_sFormStatus == "BUSS-EDIT")
            {
                if (MessageBox.Show("Update this record?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backClass.BussUpdate();
                    SaveImage(); 
                }
            }
            else if (m_sFormStatus == "BUSS-DELETE")
            {
                if (MessageBox.Show("Delete this record?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backClass.BussDelete();
                    DetachImage();

                    if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
                        this.Close();
                }
            }
            else if (m_sFormStatus == "BUSS-UPDATE")
            {
                if (MessageBox.Show("Update this record?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backClass.UpdateRecord();
                    SaveImage(); // MCR 20140320
                }
            }
            else if (m_sFormStatus == "BUSS-CANCEL-UPDATE")
            {
                if (MessageBox.Show("Cancel this record?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    backClass.CancelUpdate();
            }
            else if (m_sFormStatus == "CANCEL-APP")
            {
                if (MessageBox.Show("Cancel this record?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    backClass.CancelApplication();
                    /*
                    //GMC 20150824 Prompt Cancel Application Again(s)
                    if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
                    {
                        if (MessageBox.Show("Cancel Application Again?", "Cancel Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            bAddNewApp = true;
                            this.Dispose();
                            this.Close();
                        }
                        else
                            this.Close();
                    }
                        
                    //GMC 20150824 Prompt Cancel Application Again(e)*/ // RMC 20161204 temp remove because this creates error
                }
            }

            btnSearchBIN.Enabled = false;   // RMC 20110816
            if (m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP" || m_sFormStatus == "NEW-APP-EDIT" || m_sFormStatus == "SPL-APP-EDIT" || m_sFormStatus == "REN-APP" || m_sFormStatus == "REN-APP-EDIT")
                btnCheckList.Enabled = true;    // RMC 20110906 Added saving of Application requirements in Business Records
            else
                btnCheckList.Enabled = false;

            btnForm.Enabled = false;
            
            // JAV 20170928
            //DateTime dtDateNow = DateTime.Now;
            //DateExpiration(dtpDTIIssued.Value, dtDateNow);

            // AST 20150410 Added this block (s)
            //if (MessageBox.Show("Do you want to transact another?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    this.Close();
            //}
            // AST 20150410 Added this block (e)
        }

        private void btnBnsOwnSearch_Click(object sender, EventArgs e)
        {
            frmSearchOwner SearchOwner = new frmSearchOwner();
            SearchOwner.m_sPageWatch = "PAGE2";
            SearchOwner.ShowDialog();

            txtBnsOwnCode.Text = SearchOwner.m_strOwnCode;
            txtBnsOwnLn.Text = SearchOwner.m_sOwnLn;
            txtBnsOwnFn.Text = SearchOwner.m_sOwnFn;
            txtBnsOwnMi.Text = SearchOwner.m_sOwnMi;
            txtBnsOwnAddNo.Text = SearchOwner.m_sOwnAdd;
            txtBnsOwnMun.Text = SearchOwner.m_sOwnMun;
            txtBnsOwnProv.Text = SearchOwner.m_sOwnProv;
            txtBnsOwnStreet.Text = SearchOwner.m_sOwnStreet;
            cmbBnsOwnBrgy.Text = SearchOwner.m_sOwnBrgy;
            cmbBnsOwnDist.Text = SearchOwner.m_sOwnDist;
            txtBnsOwnZip.Text = SearchOwner.m_sOwnZip;
        }

        private void txtBnsAddNo_Leave(object sender, EventArgs e)
        {
            if (m_sFormStatus == "BUSS-ADD-NEW" || m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")    // RMC 20170127 limit auto-copying of Business Address to Owner address and location address for newly created record only
            {
                if (txtOwnAddNo.Text.ToString().Trim() == "")
                    txtOwnAddNo.Text = txtBnsAddNo.Text.ToString().Trim();

                txtPINAddNo.Text = txtBnsAddNo.Text.ToString().Trim();
            }
        }

        private void txtBnsStreet_Leave(object sender, EventArgs e)
        {
            if (m_sFormStatus == "BUSS-ADD-NEW" || m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")    // RMC 20170127 limit auto-copying of Business Address to Owner address and location address for newly created record only
            {
                if (txtOwnStreet.Text.ToString().Trim() == "")
                    txtOwnStreet.Text = txtBnsStreet.Text.ToString().Trim();

                txtPINStreet.Text = txtBnsStreet.Text.ToString().Trim();
            }
        }

        private void cmbBnsBrgy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_sFormStatus == "BUSS-ADD-NEW" || m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")    // RMC 20170127 limit auto-copying of Business Address to Owner address and location address for newly created record only
            {
                if (cmbOwnBrgy.Text.ToString() == "")
                {
                    cmbOwnBrgy.Text = cmbBnsBrgy.Text.ToString();
                }

                LoadLandPIN(cmbBnsBrgy.Text.ToString());
            }
        }

        private void btnAddlBnsInfo_Click(object sender, EventArgs e)
        {
            if (CheckPayment(bin1.GetBin(), txtTaxYear.Text.Trim()))
            {
                MessageBox.Show("You cannot edit the additional information of this business.\nThis business has an existing payment for the tax year.", "Edit Additional Info", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            using (frmAddlInfo frmGrid = new frmAddlInfo())
            {
                string strTaxYear = string.Empty;

                /*if (m_sFormStatus == "REN-APP" || m_sFormStatus == "BUSS-UPDATE")
                    strTaxYear = string.Format("{0:000#}",Convert.ToInt32(txtTaxYear.Text.ToString().Trim())-1);
                else*/  // RMC 20110810
                    strTaxYear = txtTaxYear.Text.ToString().Trim();

                frmGrid.SourceClass = "AddlInfo";
                frmGrid.BnsCode = m_strBnsCode;
                frmGrid.TaxYear = strTaxYear;
                frmGrid.RevYear = AppSettingsManager.GetConfigValue("07");

                if (m_sFormStatus == "BUSS-ADD-NEW" || m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")
                {
                    // RMC 20150102 mods in permit (s)
                    if (m_strInspectionNo != "")
                        m_strTempBIN = m_strInspectionNo;
                    // RMC 20150102 mods in permit (e)

                    if (frmGrid.BIN == "" && m_strTempBIN != "")
                    {
                        frmGrid.BIN = m_strTempBIN;
                        frmGrid.TempBIN = m_strTempBIN;
                    }
                }
                else
                    frmGrid.BIN = bin1.GetBin();

                frmGrid.ShowDialog();

                if (m_sFormStatus == "BUSS-ADD-NEW" || m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")
                    m_strTempBIN = frmGrid.TempBIN;

                // RMC 20110810 added capturing of addl info value to business record (s)
                txtNoEmp.Text = frmGrid.EmployeeNo;
                txtGroundArea.Text = frmGrid.BusinessArea;
                txtNoDelivery.Text = frmGrid.VehicleNo;
                // RMC 20110810 added capturing of addl info value to business record (e)
            }
        }

        private void btnAddlBns_Click(object sender, EventArgs e)
        {
            using (frmAdditionalBusiness frmAddlBns = new frmAdditionalBusiness())
            {
                frmAddlBns.BIN = bin1.GetBin();
                frmAddlBns.txtMainBnsName.Text = txtBnsName.Text.Trim();
                frmAddlBns.txtBnsNature.Text = txtBnsType.Text.Trim();
                frmAddlBns.txtBnsAdd.Text = txtBnsAddNo.Text.Trim() + " " + txtBnsStreet.Text.Trim() + " " + cmbBnsBrgy.Text.Trim() + " " + txtBnsMun.Text.Trim() + " " + cmbBnsDist.Text.Trim() + " " + txtBnsProv.Text.Trim();
                frmAddlBns.txtBnsOwner.Text = txtOwnLn.Text.Trim() + ", " + txtOwnFn.Text.Trim() + " " + txtOwnMi.Text.Trim() + ".";
                frmAddlBns.txtBnsOwnAdd.Text = txtOwnAddNo.Text.Trim() + " " + txtOwnStreet.Text.Trim() + " " + cmbOwnBrgy.Text.Trim() + " " + cmbOwnDist.Text.Trim() + " " + txtOwnMun.Text.Trim() + " " + txtOwnProv.Text.Trim() + " " + txtOwnZip.Text.Trim();
                frmAddlBns.txtTaxYear.Text = txtTaxYear.Text.Trim();
                frmAddlBns.OperationStart = dtpOperationStart.Value;
                frmAddlBns.txtBnsStat.Text = StringUtilities.StringUtilities.Left(cmbBnsStat.Text.Trim(),3);
                frmAddlBns.MainBnsDesc = txtBnsType.Text.Trim();
                frmAddlBns.TransCode = m_sFormStatus;   // RMC 20110809
                // RMC 20170109 enable adding of new buss in renewal application (s)
                if (m_sFormStatus == "REN-APP-EDIT" || m_sFormStatus == "REN-APP" || m_sFormStatus == "NEW-APP" || m_sFormStatus == "NEW-APP-EDIT")
                    frmAddlBns.AddlFlag = true;
                else
                    frmAddlBns.AddlFlag = false;
                // RMC 20170109 enable adding of new buss in renewal application (e)
                frmAddlBns.ShowDialog();
            }
        }

        private void txtTaxYear_TextChanged(object sender, EventArgs e)
        {
            if (txtTaxYear.Text.ToString().Length == 4)
            {
                this.txtTaxYear_Leave(sender, e);
                txtMPNo.Focus();
            }
        }

        private void btnPrevOwnSearch_Click(object sender, EventArgs e)
        {
            frmSearchOwner SearchOwner = new frmSearchOwner();
            //SearchOwner.m_sPageWatch = "";
            SearchOwner.m_sPageWatch = "PAGE2"; // RMC 20150807 modified searching of owners in Business records
            SearchOwner.ShowDialog();
               
            txtPrevOwnCode.Text = SearchOwner.m_strOwnCode;
            txtPrevOwnAddNo.Text = SearchOwner.m_sOwnAdd;
            txtPrevOwnLn.Text = SearchOwner.m_sOwnLn;
            txtPrevOwnFn.Text = SearchOwner.m_sOwnFn;
            txtPrevOwnMi.Text = SearchOwner.m_sOwnMi;
            txtPrevOwnMun.Text = SearchOwner.m_sOwnMun;
            txtPrevOwnProv.Text = SearchOwner.m_sOwnProv;
            txtPrevOwnStreet.Text = SearchOwner.m_sOwnStreet;
            cmbPrevOwnBrgy.Text = SearchOwner.m_sOwnBrgy;
            cmbPrevOwnDist.Text = SearchOwner.m_sOwnDist;
            
        }

        private void cmbBnsStat_SelectedValueChanged(object sender, EventArgs e)
        {
            string strMessage = "";
            bool blnAns = true;

            /*txtInitialCap.Text = "0.00";
            txtCurrentGross.Text = "0.00";*/
            
            if(m_strCapital == "")
                m_strCapital = txtInitialCap.Text;
            if (m_strGross == "")
                m_strGross = txtCurrentGross.Text;

            if (m_sFormStatus == "BUSS-EDIT" && !m_blnInitialLoad)
            {
                pSet.Query = "select distinct * from pay_hist where bin = '" + bin1.GetBin() + "' and data_mode <> 'UNP'";
                strMessage = "Unable to edit Business Status! Delete existing payment first...";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show(strMessage, "Business Record - EDIT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbBnsStat.Text = m_strPrevBnsStat;
                        cmbBnsStat.Enabled = false;
                        dtpOperationStart.Focus();
                        blnAns = false;
                    }
                }
            }


            if (blnAns)
            {
                if (cmbBnsStat.Text.ToString() == "NEW")
                {
                    //String.Format("{0:MM/dd/yyyy}",Date)); 
                    dtpOperationStart.Value = Convert.ToDateTime(dtpMPDate.Text);
                    txtInitialCap.Text = m_strCapital;
                    txtInitialCap.ReadOnly = false;
                    txtCurrentGross.ReadOnly = true;

                }

                if (cmbBnsStat.Text.ToString() == "RENEWAL" || cmbBnsStat.Text.ToString() == "RETIRED")
                {
                    int intTaxYear = Convert.ToInt32(txtTaxYear.Text.ToString()) - 1;
                    string strTaxYear = string.Format("{0}{1}","01/01/",intTaxYear);

                    dtpOperationStart.Value = Convert.ToDateTime(strTaxYear);
                    txtCurrentGross.Text = m_strGross;
                    txtCurrentGross.ReadOnly = false;
                    txtInitialCap.ReadOnly = true;

                    //// RMC 20150108 enable entry of capital for capturing in soa of previous capital
                    //if ((m_sFormStatus == "BUSS-ADD-NEW" || m_sFormStatus == "BUSS-EDIT") && cmbBnsStat.Text.ToString() == "RENEWAL")
                    //{
                    //    txtInitialCap.ReadOnly = false;
                    //}
                    //// RMC 20150108 enable entry of capital for capturing in soa of previous capital
                }
            }
            
        }

        private void cmbBnsStat_Enter(object sender, EventArgs e)
        {
            m_strPrevBnsStat = cmbBnsStat.Text.ToString();
        }

        // RMC 20110725 (s)
        private void rbtnMain_CheckedChanged(object sender, EventArgs e)
        {
            btnLinkUp.Enabled = false;
            //rbtnMain.Checked = false;
            m_sRadioMain = "MAIN";
            m_iBranch = 0;

            if (m_sOfcType == "BRANCH")
            {
                if (MessageBox.Show("Business is tagged as Branch office of BIN:" + m_sMotherBin + ".\nChange tagging to main branch?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (MessageBox.Show("Link to main branch, BIN:" + m_sMotherBin + " will be removed. Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        m_sRadioMain = "MAIN";
                        rbtnMain.Checked = false;
                        btnLinkUp.Enabled = false;
                    }
                    else
                    {
                        m_sRadioMain = "BRANCH";
                        rbtnMain.Checked = true;
                        btnLinkUp.Enabled = true;
                    }

                }
                else
                {
                    m_sRadioMain = "BRANCH";
                    rbtnMain.Checked = true;
                    btnLinkUp.Enabled = true;
                }
            }
        }
        // RMC 20110725 (e)

        private bool CheckPayment(string strBin, string strTaxYear)
        {
            // RMC 20140124 adjustment in updating other info in Buss-Update (s)
            if (m_sFormStatus == "BUSS-UPDATE")
                return false;
            // RMC 20140124 adjustment in updating other info in Buss-Update (e)

            //pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode <> 'UNP'", strBin, strTaxYear);
            pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and bns_stat <> 'RET'  and (data_mode = 'ONL' or data_mode = 'OFL')", strBin, strTaxYear);   // RMC 20110810
            if (pSet.Execute())
            {
                if (pSet.Read())
                    return true;
                else
                    return false;
            }

            

            return true;
        }

        private void btnDefTools_Click(object sender, EventArgs e)
        {
            using (frmDeficientRecords frmDefRecord = new frmDeficientRecords())
            {
                if (bin1.GetBin().Length < 19)
                    frmDefRecord.BIN = "";
                else
                    frmDefRecord.BIN = bin1.GetBin();
                frmDefRecord.ShowDialog();
            }
        }

        private bool ValidateRecord(string strBIN)
        {
            // for C++ reference
            //  B1 = BUSS_EDIT
            //  B2 = BUSS_DELETE
            //  B3 = BUSS_VIEW
            //  B4 = UPDATE REC
            //  B5 = CANCEL UPDATE
            //  N1 = NEW EDIT
            //  R1 = REN ADD
            //  R2 = REN EDIT
            //  R3 = RETIRED
            //  C1 = CANCEL APPLICATION

            if (txtBnsName.Text.Trim() == "" && cmbBnsOrgnKind.Text.Trim() == String.Empty) //MCR 20140916 SPL-BNS added cmbBnsOrgnKind
            {
                MessageBox.Show("No record found.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                if (m_sFormStatus == "BUSS-EDIT" || m_sFormStatus == "NEW-APP-EDIT"
                    || m_sFormStatus == "REN-APP" || m_sFormStatus == "REN-APP"
                    || m_sFormStatus == "BUSS-DELETE" || m_sFormStatus == "BUSS-CANCEL-UPDATE"
                    || m_sFormStatus == "CANCEL-APP")
                    bin1.txtTaxYear.Focus();

                if (m_sFormStatus == "BUSS-ADD-NEW")
                    txtTaxYear.Focus();

                //if (m_sFormStatus == "NEW-APP")
                if (m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")
                    txtBnsName.Focus();

                return false;
            }

            if (m_sFormStatus == "REN-APP")//R1
            {
                string strBnsStat = string.Empty;
	            string strBusinessYear = string.Empty;
	            string strBnsCode = string.Empty;
                string strBussYear = string.Empty;
                string strYear = string.Empty;
                string strTaxYear = string.Empty;
		        string strLastQtr = string.Empty;
                int intQue = 0;
                
                strYear = string.Format("{0:000#}", Convert.ToInt32(ConfigurationAttributes.CurrentYear) - 1);

                if (this.DupCheck(string.Format("top_grosspayer_tbl where bin = '{0}' and tax_year = '{1}'", strBIN, strYear)))
                {
                    if (!Granted.Grant("AARAMPS"))
                    {
                        MessageBox.Show("This record belongs to the Top Gross/Payer For " + strYear + ".", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }
                else
                {
                    if (Granted.Grant("AARAMPS") && !Granted.Grant("AARAREGMPS"))
                    {
                        MessageBox.Show("This record does not belong to the Top Gross/Payer For " + strYear + ".", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }

                if (this.DupCheck(string.Format("business_que where bin = '{0}' and bns_stat = 'RET'", strBIN)))
                {
                    MessageBox.Show("Has a pending Retirement Application.Please check it first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

                strYear = ConfigurationAttributes.CurrentYear;

                if (this.DupCheck(string.Format("business_que where bin = '{0}'", strBIN))) // RMC 20110809
                    pSet.Query = string.Format("select * from business_que where bin = '{0}'", strBIN); // RMC 20110809
                else
                    pSet.Query = string.Format("select * from businesses where bin = '{0}'", strBIN);
                if(pSet.Execute())
		        {
                    if(pSet.Read())
		    		{
			            strBnsStat = pSet.GetString("bns_stat");
			            strBusinessYear = pSet.GetString("tax_year");
			            strBnsCode = pSet.GetString("bns_code");
		            }
                }
		        pSet.Close();

		        strBussYear = strBusinessYear; 

		        m_strTaxYear = string.Format("{0:000#}",Convert.ToInt32(strBusinessYear) + 1);  
    
                pSet.Query = string.Format("select * from reg_table where bin = '{0}' and tax_year >= '{1}' order by tax_year desc",strBIN,m_strTaxYear);
			    if(pSet.Execute())
                {
                    if(pSet.Read())
				    {
			            strBnsStat = "REN";
			            strBusinessYear = pSet.GetString("tax_year");
			            m_strTaxYear = string.Format("{0:000#}",Convert.ToInt32(strBusinessYear) + 1); 
                    }
                }
		        pSet.Close();

		        pSet.Query = string.Format("select tax_year, due_state from taxdues where bin = '{0}' order by 1,2", strBIN);
		        if(pSet.Execute())
                {
                    while(pSet.Read())
                    {
                        strBnsStat = pSet.GetString(1);
                    }

                    if (strBnsStat == "R")
				        strBnsStat = "REN";

			        if (strBnsStat == "N")
				        strBnsStat = "NEW";
        			
			        if (strBnsStat == "X")
				        strBnsStat = "RET";		
                }
                pSet.Close();
		
		        pSet.Query = string.Format("select * from businesses where bin = '{0}'", strBIN);
				pSet.Query+= string.Format(" and tax_year = '{0}' and substr(memoranda,1,13) = 'UPDATE RECORD'", strBussYear);
				if(pSet.Execute())
                {
                    if(pSet.Read())
                    {
                        pSet.Close();
		                pSet.Query = string.Format("select distinct(tax_year) from pay_hist where bin = '{0}' and tax_year = '{1}'", strBIN,strBussYear);
                        if(pSet.Execute())
                        {
                            if(pSet.Read())
                            {
                            }
                            else
                            {
                        		MessageBox.Show("No payment found. Settle account due first.","Business Records",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                				return false;
			                }
                        }
                        pSet.Close();
                    }
                }
		
                if(strBussYear == ConfigurationAttributes.CurrentYear)
                {
                    MessageBox.Show("BIN already been applied.", "Business Records",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return false;
                }

                pSet.Query = string.Format("select distinct(bin) from taxdues where bin = '{0}' and tax_year < '{1}' ",strBIN, strYear);
				if(pSet.Execute())
				{
                    if (pSet.Read())
                    {
                        if (MessageBox.Show("Has pending tax dues. Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            MessageBox.Show("Settle account due first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return false;
                        }
                    }
                }
                pSet.Close();

                if (this.DupCheck(string.Format("pay_hist where bin = '{0}' and data_mode = 'UNP'", strBIN)))
                {
                    //MessageBox.Show("Cannot proceed to application, incomplete payments detected.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MessageBox.Show("Cannot proceed to application, incomplete payments detected.\nPlease verify record.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);  // RMC 20150526
                    return false;
                }

                // 2011 posted payments with conflict in payment mode - Calamba (1-time only)
                if (this.DupCheck(string.Format("payment_conflict where bin = '{0}'", strBIN)))
                {
                    //MessageBox.Show("Cannot proceed to application, incomplete payments detected.\nVerify at TAC.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MessageBox.Show("Cannot proceed to application, incomplete payments detected.\nPlease verify record.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);  // RMC 20150526
                    return false;
                }
                // 2011 posted payments with conflict in payment mode - Calamba (1-time only)
                
                if (strBnsStat == "RET")
		        {
			        MessageBox.Show("Business already been retired.","Business Records",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return false;
		        }
                                
                if (strBnsStat == "NEW" && LibraryClass.IsQtrlyDec(strBnsCode,AppSettingsManager.GetConfigValue("07")) == "Y")		
		        {
                    pSet.Query = string.Format("select tax_year,qtr_paid from pay_hist where bin = '{0}' order by 1,2", strBIN);
                    if(pSet.Execute())
                    {
                        while(pSet.Read())
                        {
            				strTaxYear = pSet.GetString("tax_year");
				            strLastQtr = pSet.GetString("qtr_paid");
                        }
                    }
                    pSet.Close();
			
			        if (strLastQtr != "4")
			        {
				        pSet.Query = string.Format("select tax_year,qtr_to_pay from taxdues where bin = '{0}' order by 1,2 desc", strBIN);
				        strTaxYear = "";
				        strLastQtr = "";
                        if(pSet.Execute())
                        {
                            if(pSet.Read())
                            {
                                strTaxYear = pSet.GetString("tax_year");
					            strLastQtr = pSet.GetString("qtr_to_pay");
                            }

                            if ((Convert.ToInt32(strTaxYear) < Convert.ToInt32(ConfigurationAttributes.CurrentYear)) && (strLastQtr != "4") && WithOutLatestPayment(strBIN))
                            {
                                if (MessageBox.Show("Previous year delinquency detected...\n\nContinue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // RMC 20150112 mods in retirement billing
                                {
                                    /*if (Granted.Grant("AIB"))   // RMC 20150112 mods in retirement billing
                                    {
                                        //if (MessageBox.Show("Previous year delinquency detected...\n\nProceed with billing?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
                                        if (MessageBox.Show("Proceed with billing?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // RMC 20150112 mods in retirement billing
                                        {
                                            if (Granted.Grant("AIB"))
                                            {
                                                // RMC 20131219 added end tasking befor calling billing in application (s)
                                                if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
                                                {
                                                }
                                                // RMC 20131219 added end tasking befor calling billing in application (e)

                                                BillingForm.SourceClass = "Billing";
                                                BillingForm.BIN = bin1.GetBin();
                                                //BillingForm.SearchButton.Enabled = false;
                                                //BillingForm.TaxYear = txtTaxYear.Text.ToString();
                                                BillingForm.Text = "Billing";
                                                BillingForm.ShowDialog();
                                                BillingForm.Dispose();
                                            }

                                            // RMC 20110810 (s)
                                            if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
                                            {
                                            }
                                            // RMC 20110810 (e)

                                            this.Close();


                                        }*/
                                    // RMC 20161220 removed proceeding to billing if bin has delinquency in application, put rem
                                    /*else
                                    {
                                        MessageBox.Show("Bill previous quarter first for quarterly declaration.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }*/
                                    // RMC 20150112 mods in retirement billing, put rem
                                    //} // RMC 20161220 removed proceeding to billing if bin has delinquency in application, put rem
                                }
                                else// RMC 20161220 removed proceeding to billing if bin has delinquency in application
                                    this.Close();// RMC 20161220 removed proceeding to billing if bin has delinquency in application
                            }
                        }
                    }
                }

                int intConvert = 0;
		        pSet.Query = string.Format("select count(*) from pay_hist where bin = '{0}'", strBIN);
                pSet.Query += string.Format(" and tax_year = (select max(tax_year) from pay_hist where bin = '{0}') and tax_year < '{1}'", strBIN, Convert.ToInt32(strYear) - 1);
                int.TryParse(pSet.ExecuteScalar().ToString(), out intConvert);
				
	            if (intConvert > 0)
		        {
                    if (MessageBox.Show("Warning: Record has delinquent payments. Continue application?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			    	{
                        MessageBox.Show("Settle account due first.","Business Records",MessageBoxButtons.OK,MessageBoxIcon.Stop);
				        return false;
                    }
                }
			
                intConvert = Convert.ToInt32(strBussYear);
		        
                if ((intConvert + 1) != Convert.ToInt32(strYear))
		        {
                    OracleResultSet pRec = new OracleResultSet();
                    string strOrigYear = string.Empty;
                    string strConvert = string.Empty;

			        //pSet.Query = string.Format("select distinct(tax_year) from taxdues where bin = '{0}' order by tax_year", strBIN);
                    pSet.Query = string.Format("select distinct(tax_year) from taxdues where bin = '{0}' order by tax_year desc", strBIN);  // RMC 20140113
		            if(pSet.Execute())
                    {
                        if(pSet.Read())
                        {
                            pSet.Close();
                            if (pSet.Execute())
                            {
                                while (pSet.Read())
                                {
                                    //get last record
                                    strTaxYear = pSet.GetString("tax_year");
                                }

                                pRec.Query = string.Format("select tax_year from business_que where bin = '{0}'", strBIN);
                                if (pRec.Execute())
                                {
                                    if (pRec.Read())
                                    {
                                        strOrigYear = pRec.GetString("tax_year");
                                    }
                                    else
                                    {
                                        pRec.Close();
                                        pRec.Query = string.Format("select tax_year from businesses where bin = '{0}'", strBIN);
                                        if (pRec.Execute())
                                        {
                                            if (pRec.Read())
                                                strOrigYear = pRec.GetString("tax_year");
                                        }
                                        pRec.Close();
                                    }
                                }

                                if (Convert.ToInt32(strOrigYear) >= Convert.ToInt32(ConfigurationAttributes.CurrentYear))
                                {
                                    MessageBox.Show("BIN already been applied.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return false;
                                }

                                
                                if (strBnsStat == "NEW")
                                {
                                    pRec.Query = "select max(qtr_paid) as cnt_qpaid from pay_hist";
                                    pRec.Query += string.Format(" where bin = '{0}' and tax_year = '{1}'", strBIN, strYear);
                                    if (pRec.Execute())
                                    {
                                        if (pRec.Read())
                                            strConvert = pRec.GetString("cnt_qpaid");
                                    }
                                    pRec.Close();

                                    if (strConvert == "4" || strConvert == "F")
                                    {
                                        strBnsStat = "REN";
                                    }

                                    if (Convert.ToInt32(strOrigYear) < Convert.ToInt32(strTaxYear))
                                    {
                                        strBnsStat = "REN";
                                    }
                                }
                                else
                                {
                                    strLastQtr = "1";
                                }
                            }
                            
                        }
                        else
			    		{
                            intQue = 0;
				            pRec.Query = string.Format("select tax_year from business_que where bin = '{0}'", strBIN);
				            if(pRec.Execute())
                            {
                                if(pRec.Read())
                                {
					                intQue = 1;
				                }
				                else
				                {
                                    pRec.Close();
					
                                    pRec.Query = string.Format("select tax_year from businesses where bin = '{0}'", strBIN);
					                if(pRec.Execute())
                                    {
                                        if(pRec.Read())
					                        strTaxYear = pRec.GetString("tax_year");
						            }
                                    pRec.Close();
					
					                if (strBnsStat == "NEW") 
					                {
						                pRec.Query = string.Format("select max(qtr_paid) as cnt_qpaid from pay_hist where bin = '{0}' and tax_year = '{1}'", strBIN, strTaxYear);
                                        if(pRec.Execute())
                                        {
                                            if(pRec.Read())
        						                strConvert = pRec.GetString("cnt_qpaid");
                                        }
                                        pRec.Close();

                                        if (strConvert == "4" || strConvert == "F") 
						                {
							                strBnsStat = "REN";
						                }
					                }
					                intQue = 0;
				                }
			                }
                        }
                    }
                    pSet.Close();

                    if(strTaxYear == "") //JARS 20170831
                    {
                        strTaxYear = (Convert.ToInt32(strBussYear) + 1).ToString();
                    }

                    if(Convert.ToInt32(strTaxYear) < Convert.ToInt32(strBussYear))
			    	{
                        string strTempYr = string.Empty;
                        
				        if (strTaxYear.Trim() == "")
                            strTempYr = strBussYear;
				        else
                            strTempYr = strTaxYear;

                        MessageBox.Show("Last renewal occur last " + strTempYr, "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
    
                        if(!(Convert.ToInt32(strTaxYear) == Convert.ToInt32(strBussYear)) && (intQue == 0) && (strBnsStat != "NEW"))
				        {
                            MessageBox.Show("Pending assessment found, proceed to Bill Previous module.","Business Records",MessageBoxButtons.OK,MessageBoxIcon.Stop);
        					return false;
				        }
			        }
                }

                // CLOSURE
                if (this.DupCheck(string.Format("official_closure_tagging where bin = '{0}'", strBIN)))
                {
                    MessageBox.Show("Record was tagged for closure. Cannot continue.","Business Records",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

                // HOLD RECORDS
                string strHRUser, strHRDate, strHRRemarks, strMess;

                pSet.Query = string.Format("select * from hold_records where bin = '{0}' and status = 'HOLD'", strBIN);
                if(pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        strHRUser = pSet.GetString("user_code");
                        strHRDate = pSet.GetString("dt_save");
                        strHRRemarks = pSet.GetString("remarks");
                        strMess = "Cannot continue! This record is currently on hold.\nUser Code: " + strHRUser + "  Date: " + strHRDate + "\nRemarks: " + strHRRemarks;
                        MessageBox.Show(strMess, "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }
                pSet.Close();

                if (strBnsStat != "NEW" && Granted.Grant("AIB"))
                {
                    if ((Convert.ToInt32(m_strTaxYear) < Convert.ToInt32(ConfigurationAttributes.CurrentYear)) && WithOutLatestPayment(strBIN))
                    {
                        if (MessageBox.Show("Previous year delinquency detected...\n\nContinue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // RMC 20150112 mods in retirement billing
                        {
                            #region comments
                            /*if (Granted.Grant("AIB"))   // RMC 20150112 mods in retirement billing
                            {
                                //if (MessageBox.Show("Previous year delinquency detected...\n\nProceed with billing?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                if (MessageBox.Show("Proceed with billing?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // RMC 20150112 mods in retirement billing
                                {
                                    if (Granted.Grant("AIB"))
                                    {
                                        // RMC 20131219 added end tasking befor calling billing in application (s)
                                        if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
                                        {
                                        }
                                        // RMC 20131219 added end tasking befor calling billing in application (e)

                                        BillingForm.SourceClass = "Billing";
                                        BillingForm.Text = "Billing";
                                        BillingForm.BIN = bin1.GetBin();
                                        //BillingForm.SearchButton.Enabled = false;
                                        //BillingForm.TaxYear = txtTaxYear.Text.ToString();
                                        BillingForm.Text = "Billing";
                                        BillingForm.ShowDialog();
                                        //BillingForm.Dispose();    // RMC 20150817 corrections in application and billing of delinquent business

                                    }

                                    // RMC 20110810 (s)
                                    if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
                                    {
                                    }
                                    // RMC 20110810 (e)
                                    this.Close();


                                }*/
                            // RMC 20161220 removed proceeding to billing if bin has delinquency in application, put rem
                            /*else
                            {
                                MessageBox.Show("Bill previous year delinquency first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }*/
                            // RMC 20150112 mods in retirement billing, put rem
                            //} // RMC 20161220 removed proceeding to billing if bin has delinquency in application, put rem
                            #endregion
                        }
                        else// RMC 20161220 removed proceeding to billing if bin has delinquency in application
                            this.Close();// RMC 20161220 removed proceeding to billing if bin has delinquency in application
                    }
                    else
                    {
                        if (Convert.ToInt32(m_strTaxYear) >= Convert.ToInt32(ConfigurationAttributes.CurrentYear))
                            m_bSwQue = true;
                    }

                    if (m_bSwQue)
                    {
                        if (this.DupCheck(string.Format("business_que where bin = '{0}' and tax_year = '{1}'", strBIN, m_strTaxYear)))
                        {
                            //using (frmBilling BillingForm = new frmBilling())
                            {
                                BillingForm.SourceClass = "Billing";
                                BillingForm.BIN = strBIN;
                                //BillingForm.SearchButton.Enabled = false;
                                //BillingForm.TaxYear = m_strTaxYear;
                                BillingForm.Text = "Billing";
                                BillingForm.ShowDialog();
                                BillingForm.Dispose();
                            }

                            // RMC 20110810 (s)
                            if (TaskMan.IsObjectLock(strBIN, m_sFormStatus, "DELETE", "ASS"))
                            {
                            }
                            // RMC 20110810 (e)

                            this.Close();
                        }
                    }
                }
            }

            if (m_sFormStatus == "REN-APP-EDIT" || m_sFormStatus == "NEW-APP-EDIT" || m_sFormStatus == "CANCEL-APP")//R2 N1 C1
            {
                if (this.DupCheck(string.Format("business_que where bin = '{0}' and bns_stat = 'RET'", strBIN)))
                {
                    MessageBox.Show("Business has a pending Retirement Application.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

                string sTmpStat = string.Empty;

                if (m_sFormStatus == "REN-APP-EDIT") 
                    sTmpStat = "REN";
                if (m_sFormStatus == "NEW-APP-EDIT")
                    sTmpStat = "NEW";

                if (sTmpStat != string.Empty)
                {
                    if (!this.DupCheck(string.Format("business_que where bin = '{0}' and bns_stat = '{1}'", strBIN, sTmpStat)))
                    {
                        MessageBox.Show("Business has no pending application.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }
                else
                {
                    if (!this.DupCheck(string.Format("business_que where bin = '{0}'", strBIN)))
                    {
                        MessageBox.Show("Business has no pending application.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }

                /*if (this.DupCheck(string.Format("permit_update_appl where bin = '{0}' and data_mode = 'QUE'", strBIN)))
                {
                    MessageBox.Show("Business has Permit Update Application.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }*/
                // RMC 20180118 added cancellation of permit update appl queued on prev year, put rem

                // RMC 20180118 added cancellation of permit update appl queued on prev year (s)
                if (m_sFormStatus == "CANCEL-APP")
                {
                    if (this.DupCheck(string.Format("permit_update_appl where bin = '{0}' and data_mode = 'QUE'", strBIN)))
                    {
                        if (MessageBox.Show("Cancellation of application will also cancel permit update application.\nProceed?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            return false;

                    }
                }
                // RMC 20180118 added cancellation of permit update appl queued on prev year (e)

                
            }

            // RMC 20110809 Added validation (s)
            if (m_sFormStatus == "BUSS-EDIT" || m_sFormStatus == "BUSS-UPDATE")
            {
                if (this.DupCheck(string.Format("business_que where bin = '{0}'", strBIN)))
                {
                    MessageBox.Show("Editing/Updating not allowed, business has pending application.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            if (m_sFormStatus == "BUSS-DELETE")
            {
                if (this.DupCheck(string.Format("business_que where bin = '{0}'", strBIN)))
                {
                    MessageBox.Show("Deletion not allowed, business has pending application.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            // RMC 20110809 Added validation (e)

            if (m_sFormStatus == "BUSS-UPDATE")//B4
            {
                int intCount = 0;
                pSet.Query = string.Format("select count(*) from pay_hist where bin = '{0}'", strBIN);
                pSet.Query += " and tax_year = (select max(tax_year) from pay_hist)";
                pSet.Query += string.Format(" and tax_year < '{0}' - 1", ConfigurationAttributes.CurrentYear);
                int.TryParse(pSet.ExecuteScalar().ToString(), out intCount);

                if (intCount > 0)
                {
                    MessageBox.Show("Pay previous year delinquent first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;

                }

                if (this.DupCheck("taxdues where bin = '" + strBIN + "'"))
                {
                    if (!this.DupCheck("business_que where bin = '" + strBIN + "'"))
                    {
                        MessageBox.Show("Unpaid dues found....Settle due first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                if (this.DupCheck("business_que where bin = '" + strBIN + "'"))
                {
                    if (MessageBox.Show("Existing Application/billing found! Continue Update?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;

                }

                if (this.DupCheck("businesses where bin = '" + strBIN + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "'"))
                {
                    MessageBox.Show("Record updated up to current year.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                pSet.Query = "select distinct * from pay_hist where tax_year in (";
                pSet.Query += string.Format("select tax_year from businesses where bin = '{0}') and bin = '{0}'", strBIN);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                    }
                    else
                    {
                        MessageBox.Show("No payment found. Settle account due first,", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                pSet.Close();

                pSet.Query = string.Format("select bin from businesses where bin = '{0}'", strBIN);
                pSet.Query += string.Format(" and substr(memoranda,1,13) = 'UPDATE RECORD' and tax_year = '{0}'", ConfigurationAttributes.CurrentYear);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Business record already been updated.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                pSet.Close();
            }

            if (m_sFormStatus == "BUSS-CANCEL-UPDATE")//B5
            {
                string strConvert = "";

                pSet.Query = string.Format("select bin from businesses where bin = '{0}'", strBIN);
                pSet.Query += " and substr(memoranda,1,13) = 'UPDATE RECORD'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                        strConvert = pSet.GetString(0).Trim();
                }
                pSet.Close();

                if (strConvert == "")
                {
                    MessageBox.Show("Record not found.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            
            // validate business mapping changes
            if (m_sFormStatus == "REN-APP" || m_sFormStatus == "REN-APP-EDIT")
            {
                //if (ValidateMapping(strBIN))
                if (AppSettingsManager.ValidateMapping(strBIN))  // RMC 20111220 transferred validation of business if business-mapped in AppSettingsManager
                {
                    if (MessageBox.Show("Deficiencies discovered during business mapping drive.\nContinue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;
                    else
                    {
                        btnBTMDeficiency.Enabled = true;
                        PrintDeficiency();
                    }
                }

                
            }


            return true;
        }

        //private bool DupCheck(string strQuery)
        public bool DupCheck(string strQuery)   // RMC 20111214 changed to public
        {
            int intCount = 0;

            pSet.Query = string.Format("select * from {0}", strQuery);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    // RMC 20140110 ADDED CHECKING IF PREV YEAR UNDER INCENTIVE (S)
                    if (strQuery.Contains("pay_hist where bin"))
                    {
                        pSet.Close();
                        string sExemptYear = string.Empty;
                        int iExemptYear = 0;
                        int iCurrYear = 0;
                        int.TryParse(AppSettingsManager.GetConfigValue("12"), out iCurrYear);

                        pSet.Query = "select * from boi_table where bin = '" + bin1.GetBin() + "'";
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                sExemptYear = string.Format("{0:yyyy}", pSet.GetDateTime("dateto"));
                                int.TryParse(sExemptYear, out iExemptYear);
                                if (iExemptYear >= iCurrYear)
                                    return false;
                                else
                                {
                                    if (iExemptYear == iCurrYear - 1)
                                        return false;
                                    else
                                        return true;
                                }
                            }
                            else
                                return true;
                        }
                    }
                    else
                        // RMC 20140110 ADDED CHECKING IF PREV YEAR UNDER INCENTIVE (E)
                        return true;
                    
                }
                else
                    return false;
            }
            pSet.Close();
                       
            return true;
        }

        private void cmbOwnBrgy_Leave(object sender, EventArgs e)
        {
            string strBrgy = cmbOwnBrgy.Text.ToUpper();
            cmbOwnBrgy.Text = strBrgy;
        }

        private void cmbPINBrgy_Leave(object sender, EventArgs e)
        {
            string strBrgy = cmbPINBrgy.Text.ToUpper();
            cmbPINBrgy.Text = strBrgy;
        }

        private bool WithOutLatestPayment(string strBin)
        {
            string strLatestTaxYearPaid = string.Empty;
            string strLatestQtrPaid = string.Empty;
            string strPaymentTerm = string.Empty;
            
            DateTime dtCurrent = AppSettingsManager.GetSystemDate();
            int intCurrentYear = dtCurrent.Year;

            pSet.Query = string.Format("select tax_year,qtr_paid,payment_term from pay_hist where bin = '{0}' order by tax_year desc, qtr_paid desc", strBin);
            if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    strLatestTaxYearPaid = pSet.GetString("tax_year");
                    strLatestQtrPaid = pSet.GetString("qtr_paid");
                    strPaymentTerm = pSet.GetString("payment_term");
                }
            }
            pSet.Close();
            
            //if (intCurrentYear - 1 == Convert.ToInt32(strLatestTaxYearPaid) && (strLatestQtrPaid == "F" || strLatestQtrPaid == "4" || strPaymentTerm == "I"))
            // RMC 20171227 QA corrections in application (s)
            int iLatestTaxYearPaid = 0;
            int.TryParse(strLatestTaxYearPaid, out iLatestTaxYearPaid);
            if (intCurrentYear - 1 == iLatestTaxYearPaid && (strLatestQtrPaid == "F" || strLatestQtrPaid == "4" || strPaymentTerm == "I"))
            // RMC 20171227 QA corrections in application (e)
            {
                m_strTaxYear = string.Format("{0:000#", intCurrentYear);
                m_bSwEnableSave = true;
                return false;
            }
            else
                return true;
        }

        private void txtInitialCap_TextChanged(object sender, EventArgs e)
        {
            m_objMoneyValidator.HandleTextChange(sender, 10);

            if(!m_blnInitialLoad)
            {
                if (CheckPayment(bin1.GetBin(), txtTaxYear.Text.Trim()))
                {
                    MessageBox.Show("You cannot edit the capital.\nThe business has an exisiting payment for the tax year.", "Edit Capital", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtInitialCap.Text = m_strCapital;
                    return;
                }
            }
        }

        private void txtPrevGross_TextChanged(object sender, EventArgs e)
        {
            m_objMoneyValidator.HandleTextChange(sender, 10);
        }

        private void txtCurrentGross_TextChanged(object sender, EventArgs e)
        {
            if (m_sFormStatus == "SPL-APP" || m_sFormStatus == "SPL-APP-EDIT")
            {
                txtCurrentGross.Text = "0.00";
                return;
            }

            m_objMoneyValidator.HandleTextChange(sender, 10);

            if (!m_blnInitialLoad)
            {
                if (CheckPayment(bin1.GetBin(), txtTaxYear.Text.Trim()))
                {
                    MessageBox.Show("You cannot edit the gross.\nThe business has an exisiting payment for the tax year.", "Edit Gross", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCurrentGross.Text = m_strGross;
                    return;
                }
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void txtOwnCode_TextChanged(object sender, EventArgs e)
        {
            btnOwnProfile.Enabled = !string.IsNullOrEmpty(txtOwnCode.Text.Trim()); // AST 20150413 
        }

        private void label69_Click(object sender, EventArgs e)
        {

        }

        private void txtBnsType_TextChanged(object sender, EventArgs e)
        {
            
        }

        
        private void txtMonthlyRental_Leave(object sender, EventArgs e)
        {
            try
            {
                txtMonthlyRental.Text = string.Format("{0:#,###.00}", Convert.ToDouble(txtMonthlyRental.Text.ToString()));
            }
            catch { }

        }

        private void txtMonthlyRental_TextChanged(object sender, EventArgs e)
        {
            m_objMoneyValidator.HandleTextChange(sender, 10);
        }

        private void txtInitialCap_Leave(object sender, EventArgs e)
        {
            try
            {
                txtInitialCap.Text = string.Format("{0:#,###.00}", Convert.ToDouble(txtInitialCap.Text.ToString()));
            }
            catch { }
        }

        private void txtCurrentGross_Leave(object sender, EventArgs e)
        {
            try
            {
                txtCurrentGross.Text = string.Format("{0:#,###.00}", Convert.ToDouble(txtCurrentGross.Text.ToString()));
            }
            catch { }
        }

        private void txtPrevGross_Leave(object sender, EventArgs e)
        {
            try
            {
                txtPrevGross.Text = string.Format("{0:#,###.00}", Convert.ToDouble(txtPrevGross.Text.ToString()));
            }
            catch { }
        }

        private void btnSetBOI_Click(object sender, EventArgs e)
        {
            /*using (frmTagging frmTagging = new frmTagging())
            {
                frmTagging.BIN = bin1.GetBin();
                frmTagging.ShowDialog();
            }*/
        }

        private bool CheckAddlBns()
        {
            // RMC 20110825 modified pop-up of module in renewal-edit
            OracleResultSet pCheck = new OracleResultSet();

            pCheck.Query = string.Format("select * from addl_bns where bin = '{0}'", bin1.GetBin());
            if (pCheck.Execute())
            {
                if (pCheck.Read())
                {
                    pCheck.Close();

                    if (m_sFormStatus == "REN-APP-EDIT")
                    {
                        pCheck.Query = string.Format("select * from addl_bns where bin = '{0}'", bin1.GetBin());
                        pCheck.Query += " and tax_year = '" + txtTaxYear.Text + "'";
                        if (pCheck.Execute())
                        {
                            if (pCheck.Read())
                            {
                                pCheck.Close();
                                return false;
                            }
                            else
                            {
                                pCheck.Close();
                                return true;
                            }
                        }
                        pCheck.Close();
                    }
                    
                    return true;
                }
                else
                {
                    pCheck.Close();
                    return false;
                }
            }

            return true;
        }

        // RMC 20110725 (s)
        private void btnLinkUp_Click(object sender, EventArgs e)
        {
            frmApplicationForm frmMainBranch = new frmApplicationForm();
            //frmMainBranch.bin1.txtTaxYear.Text = bin1.GetBin().Substring(7, 4).ToString();
            //frmMainBranch.bin1.txtBINSeries.Text = bin1.GetBin().Substring(12, 7).ToString();
            frmMainBranch.m_sBranchBIN = bin1.GetBin();
            frmMainBranch.ShowDialog();

            m_sOfcType = frmMainBranch.m_sOfcType;
            m_sMotherBin = frmMainBranch.m_sBIN;
        }

        
        private void chkConsolidatedGross_CheckStateChanged(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            if (chkConsolidatedGross.CheckState.ToString() == "Checked")
            {
                rbtnMain.Enabled = true;
                rbtnBranch.Enabled = true;
            }
            else
            {
                rbtnMain.Enabled = false;
                rbtnBranch.Enabled = false;

                if (m_sOfcType == "BRANCH")
                {
                    if (MessageBox.Show("Link to main branch, BIN:" + m_sMotherBin + " will be removed. Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        chkConsolidatedGross.Checked = false;
                        rbtnMain.Enabled = false;
                        rbtnBranch.Enabled = false;
                    }
                    else
                    {
                        chkConsolidatedGross.Checked = true;
                        rbtnMain.Enabled = true;
                        rbtnBranch.Enabled = true;
                    }

                }
                if (m_sOfcType == "MAIN")
                {
                    pRec.Query = string.Format("select * from consol_gr where mother_bin = '{0}' and ofc_type <> 'SINGLE'", bin1.GetBin());
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            m_iBranch++;
                        }
                    }
                    pRec.Close();

                    if (m_iBranch > 1)
                    {
                        MessageBox.Show("You have to cancel all branches connected first.", "BPS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        chkConsolidatedGross.Checked = true;
                        rbtnMain.Enabled = true;
                        rbtnBranch.Enabled = true;
                    }
                    else
                    {
                        if (m_iBranch == 1)
                        {
                            chkConsolidatedGross.Checked = false;
                            rbtnMain.Enabled = false;
                            rbtnBranch.Enabled = false;

                        }
                    }
                }
            }

        }

        private void rbtnBranch_CheckedChanged(object sender, EventArgs e)
        {
            btnLinkUp.Enabled = true;
            //rbtnMain.Checked = true;
            m_iBranch = 0;

            if (m_sOfcType == "MAIN")
            {
                if (MessageBox.Show("Business is tagged as Main office.\nDo you want to tag this record as branch ofc?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet pRec = new OracleResultSet();

                    pRec.Query = string.Format("select * from consol_gr where mother_bin = '{0}' and ofc_type <> 'SINGLE'", bin1.GetBin());
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            m_iBranch++;
                        }
                    }
                    pRec.Close();
                    
                    if (m_iBranch > 1)
                    {
                        MessageBox.Show("You have to cancel all branches connected first.", "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        rbtnMain.Checked = false;
                        m_sRadioMain = "MAIN";
                        btnLinkUp.Enabled = false;

                    }
                    else
                        if (m_iBranch == 1)
                        {
                            rbtnMain.Checked = false;
                            m_sRadioMain = "BRANCH";
                            btnLinkUp.Enabled = true;
                        }
                }
                else
                {
                    m_sRadioMain = "MAIN";
                    rbtnMain.Checked = false;
                    btnLinkUp.Enabled = false;
                }
            }
        }
        // RMC 20110725 (e)

        private void txtCTCNo_Leave(object sender, EventArgs e)
        {
            // RMC 20110810 Added validation of CTC no. for duplication
            OracleResultSet pRec = new OracleResultSet();
            string sTmpBIN = "";
            

            if (this.DupCheck(string.Format("businesses where ctc_no = '{0}' and bin <> '{1}'", txtCTCNo.Text.Trim(), bin1.GetBin())))
            {
                pRec.Query = string.Format("select * from businesses where ctc_no = '{0}' and bin <> '{1}'", txtCTCNo.Text.Trim(), bin1.GetBin());
            }
            else
            {
                pRec.Query = string.Format("select * from business_que where ctc_no = '{0}' and bin <> '{1}'", txtCTCNo.Text.Trim(), bin1.GetBin());
            }

            if(pRec.Execute())
            {
                if(pRec.Read())
                {
                    sTmpBIN = pRec.GetString("bin");
                    // RMC 20150806 Allow duplication of CTC no. in Page 3 for same Business Owner (s)
                    string sTmpOwnCode = "";    
                    sTmpOwnCode = pRec.GetString("own_code");

                    if (sTmpOwnCode == txtOwnCode.Text.Trim())
                    { } // RMC 20150806 Allow duplication of CTC no. in Page 3 for same Business Owner (e)
                    else
                    {
                        MessageBox.Show("Duplicate CTC no. with BIN : " + sTmpBIN, "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtCTCNo.Text = "";
                        txtCTCNo.Focus();
                        pRec.Close();
                        return;
                    }
                }
            }
            pRec.Close();
                            
        }

        private void LoadValues()
        {
            // RMC 20110816
            // RMC 20111128 consider Business mapping - Unencoded in Business Records-Add

            OracleResultSet pRec = new OracleResultSet();
            string sPermitNo = "";
            string sBnsCode = "";
            string sBusnOwn = "";
            bool bTaxMapped = false;
            int iCnt = 0;

            // RMC 20141222 modified permit printing (s)
            if (AppSettingsManager.GetConfigValue("10") == "019")//lubao
            {
                LoadValuesZoning();

            }// RMC 20141222 modified permit printing (e)
            else
            {
                if (m_sFormStatus == "BUSS-ADD-NEW")
                {
                    pRec.Query = "select * from btm_temp_businesses where tbin = '" + m_strInspectionNo + "'";
                    bTaxMapped = true;
                }
                else //if (m_sFormStatus == "NEW-APP")
                {
                    pRec.Query = "select count(*) from unofficial_info_tbl where trim(bin_settled) is null and is_number = '" + m_strInspectionNo + "'";
                    int.TryParse(pRec.ExecuteScalar(), out iCnt);

                    if (iCnt == 0)
                    {
                        pRec.Query = "select count(*) from btm_temp_businesses where tbin = '" + m_strInspectionNo + "' and trim(old_bin) is null";
                        int.TryParse(pRec.ExecuteScalar(), out iCnt);

                        if (iCnt > 0)
                        {
                            pRec.Query = "select * from btm_temp_businesses where tbin = '" + m_strInspectionNo + "' and trim(old_bin) is null";
                            bTaxMapped = true;
                        }
                    }
                    else
                    {
                        pRec.Query = "select * from unofficial_info_tbl where trim(bin_settled) is null and is_number = '" + m_strInspectionNo + "'";
                        bTaxMapped = false;
                    }
                }
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        txtBnsName.Text = StringUtilities.StringUtilities.RemoveApostrophe(pRec.GetString("bns_nm").Trim());
                        txtBnsAddNo.Text = pRec.GetString("bns_house_no").Trim();
                        txtBnsStreet.Text = pRec.GetString("bns_street").Trim();
                        cmbBnsBrgy.Text = pRec.GetString("bns_brgy").Trim();
                        txtBnsZone.Text = pRec.GetString("bns_zone").Trim();
                        txtBnsMun.Text = pRec.GetString("bns_mun").Trim();
                        txtOwnCode.Text = pRec.GetString("own_code").Trim();

                        BPLSAppSettingList sList = new BPLSAppSettingList();

                        sList.OwnName = txtOwnCode.Text;

                        for (int j = 0; j < sList.OwnNamesSetting.Count; j++)
                        {
                            txtOwnCode.Text = sList.OwnNamesSetting[j].sOwnerCode;
                            txtOwnLn.Text = sList.OwnNamesSetting[j].sLn;
                            txtOwnFn.Text = sList.OwnNamesSetting[j].sFn;
                            txtOwnMi.Text = sList.OwnNamesSetting[j].sMi;
                            txtOwnAddNo.Text = sList.OwnNamesSetting[j].sOwnHouseNo;
                            txtOwnStreet.Text = sList.OwnNamesSetting[j].sOwnStreet;
                            cmbOwnBrgy.Text = sList.OwnNamesSetting[j].sOwnBrgy;
                            cmbOwnDist.Text = sList.OwnNamesSetting[j].sOwnDist;
                            txtOwnMun.Text = sList.OwnNamesSetting[j].sOwnMun;
                            txtOwnProv.Text = sList.OwnNamesSetting[j].sOwnProv;
                            txtOwnZip.Text = sList.OwnNamesSetting[j].sOwnZip;
                        }

                        if (bTaxMapped)
                        {
                            sPermitNo = pRec.GetString("permit_no");
                            if (sPermitNo != "")
                            {
                                txtTaxYear.Text = sPermitNo.Substring(0, 4);
                                txtMPYear.Text = sPermitNo.Substring(0, 4);
                                txtMPNo.Text = sPermitNo.Substring(5, 5);
                            }
                            dtpMPDate.Value = pRec.GetDateTime("permit_dt");
                            cmbBnsOrgnKind.Text = pRec.GetString("orgn_kind");
                            sBnsCode = pRec.GetString("bns_code");
                            m_strBnsCode = sBnsCode;
                            m_strTmpBnsCode = sBnsCode;
                            txtBnsType.Text = AppSettingsManager.GetBnsDesc(sBnsCode);
                            sBusnOwn = pRec.GetString("busn_own");
                            txtMonthlyRental.Text = string.Format("{0:#,###.00}", pRec.GetDouble("rent_lease_mo"));
                            cmbPlaceBnsStat.Text = pRec.GetString("place_occupancy");

                            sList.OwnName = sBusnOwn;
                            for (int j = 0; j < sList.OwnNamesSetting.Count; j++)
                            {
                                txtBnsOwnCode.Text = sList.OwnNamesSetting[j].sOwnerCode;
                                txtBnsOwnLn.Text = sList.OwnNamesSetting[j].sLn;
                                txtBnsOwnFn.Text = sList.OwnNamesSetting[j].sFn;
                                txtBnsOwnMi.Text = sList.OwnNamesSetting[j].sMi;
                                txtBnsOwnAddNo.Text = sList.OwnNamesSetting[j].sOwnHouseNo;
                                txtBnsOwnStreet.Text = sList.OwnNamesSetting[j].sOwnStreet;
                                cmbBnsOwnBrgy.Text = sList.OwnNamesSetting[j].sOwnBrgy;
                                cmbBnsOwnDist.Text = sList.OwnNamesSetting[j].sOwnDist;
                                txtBnsOwnMun.Text = sList.OwnNamesSetting[j].sOwnMun;
                                txtBnsOwnProv.Text = sList.OwnNamesSetting[j].sOwnProv;
                                txtBnsOwnZip.Text = sList.OwnNamesSetting[j].sOwnZip;
                            }

                            sList.OwnName = txtOwnCode.Text;
                            for (int j = 0; j < sList.OwnNamesSetting.Count; j++)
                            {
                                txtPrevOwnCode.Text = sList.OwnNamesSetting[j].sOwnerCode;
                                txtPrevOwnLn.Text = sList.OwnNamesSetting[j].sLn;
                                txtPrevOwnFn.Text = sList.OwnNamesSetting[j].sFn;
                                txtPrevOwnMi.Text = sList.OwnNamesSetting[j].sMi;
                                txtPrevOwnAddNo.Text = sList.OwnNamesSetting[j].sOwnHouseNo;
                                txtPrevOwnStreet.Text = sList.OwnNamesSetting[j].sOwnStreet;
                                cmbPrevOwnBrgy.Text = sList.OwnNamesSetting[j].sOwnBrgy;
                                cmbPrevOwnDist.Text = sList.OwnNamesSetting[j].sOwnDist;
                                txtPrevOwnMun.Text = sList.OwnNamesSetting[j].sOwnMun;
                                txtPrevOwnProv.Text = sList.OwnNamesSetting[j].sOwnProv;
                                txtPrevOwnZip.Text = sList.OwnNamesSetting[j].sOwnZip;
                            }
                        }
                    }
                }
                pRec.Close();


                LoadGisValues(m_strInspectionNo);
            }
        }

        private void btnCheckList_Click(object sender, EventArgs e)
        {
            // RMC 20110906 Added saving of Application requirements in Business Records

            if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
            {
                using (frmAppCheckList frmAppCheckList = new frmAppCheckList())
                {
                    frmAppCheckList.BIN = bin1.GetBin();
                    frmAppCheckList.TaxYear = txtTaxYear.Text;
                    frmAppCheckList.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Select BIN first.","Business Records",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            // RMC 20111006 Added 'Add New' button in Business Records-Add module

            bState = true;
            this.Opacity = 1.0;
            timerBnsRec.Enabled = true;

            if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
            {
            }

            m_bAddNew = true;

            m_frmImageList.Close(); // CJC 20130401
            

            BackCodeClass backCode = new BackCodeClass(this);

            if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
            {
            }

            // RMC 20161121 corrections in adding new record button (s)
            bState = true;
            this.Opacity = 1.0;
            timerBnsRec.Enabled = true;

            OracleResultSet pCmd = new OracleResultSet();
            pCmd.Query = "delete from addl_info_tmp where bin = '" + bin1.GetBin() + "'";
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            this.Dispose();
            this.Close();
            // RMC 20161121 corrections in adding new record button (e)
            
            /*
            bin1.txtTaxYear.ReadOnly = false;
            bin1.txtBINSeries.ReadOnly = false;
            bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";

            backCode.ClearControls(1);
            backCode.ClearControls(2);
            backCode.ClearControls(3);
            btnSearchBIN.Text = "Search Bin";

            if (m_sFormStatus == "BUSS-EDIT" || m_sFormStatus == "REN-APP"
                || m_sFormStatus == "REN-APP" || m_sFormStatus == "BUSS-DELETE"
                || m_sFormStatus == "BUSS-UPDATE" || m_sFormStatus == "BUSS-CANCEL-UPDATE"
                || m_sFormStatus == "CANCEL-APP")
            {
                backCode.ManageControls(1, false);
                backCode.ManageControls(2, false);
                backCode.ManageControls(3, false);
            }

            if (m_sFormStatus == "BUSS-EDIT" || m_sFormStatus == "NEW-APP-EDIT"
                || m_sFormStatus == "REN-APP" || m_sFormStatus == "REN-APP"
                || m_sFormStatus == "BUSS-DELETE" || m_sFormStatus == "BUSS-CANCEL-UPDATE"
                || m_sFormStatus == "CANCEL-APP")
                bin1.txtTaxYear.Focus();

            if (m_sFormStatus == "BUSS-ADD-NEW")
                txtTaxYear.Focus();

            if (m_sFormStatus == "NEW-APP")
                txtBnsName.Focus();

            m_blnInitialLoad = true;
            btnSave.Enabled = false;
            btnCheckList.Enabled = false;

            frmBusinessRecord_Load(sender, e);

            this.tabBnsRec.TabIndex = 0;
            tabBnsRec.SelectTab(tabPage1);
            */
        }

        private void txtOwnLn_Leave(object sender, EventArgs e)
        {
            // RMC 20111011 added pop-up of same own names upon entry of last name (user-request)

            if (txtOwnLn.Text.Trim() != "")
            {
                frmSearchOwner SearchOwner = new frmSearchOwner();
                //SearchOwner.m_sPageWatch = "";
                SearchOwner.m_sPageWatch = "PAGE2"; // RMC 20111128
                SearchOwner.OwnLastName = txtOwnLn.Text.Trim();
                SearchOwner.GetValues();    // RMC 20111128
                if (!SearchOwner.m_bNoRecordFound)   // RMC 20111128
                    SearchOwner.ShowDialog();

                if (SearchOwner.m_strOwnCode != "")
                {
                    txtOwnCode.Text = SearchOwner.m_strOwnCode;
                    txtOwnLn.Text = SearchOwner.m_sOwnLn;
                    txtOwnFn.Text = SearchOwner.m_sOwnFn;
                    txtOwnMi.Text = SearchOwner.m_sOwnMi;
                    txtOwnAddNo.Text = SearchOwner.m_sOwnAdd;
                    txtOwnStreet.Text = SearchOwner.m_sOwnStreet;
                    cmbOwnBrgy.Text = SearchOwner.m_sOwnBrgy;
                    cmbOwnDist.Text = SearchOwner.m_sOwnDist;
                    txtOwnMun.Text = SearchOwner.m_sOwnMun;
                    txtOwnProv.Text = SearchOwner.m_sOwnProv;
                    txtOwnZip.Text = SearchOwner.m_sOwnZip;
                    
                }
                else
                {
                    txtOwnCode.Text = "";
                    //txtOwnLn.Text = "";
                    txtOwnFn.Text = "";
                    txtOwnMi.Text = "";
                    txtOwnAddNo.Text = "";
                    txtOwnStreet.Text = "";
                    cmbOwnBrgy.Text = "";
                    cmbOwnDist.Text = "";
                    txtOwnMun.Text = "";
                    txtOwnProv.Text = "";
                    txtOwnZip.Text = "";
                    
                }
            }
        }

        private void PrintApplicationForm()
        {
            // RMC 20111013 Added printing of application form
        }

        private void btnSearchBldgPIN_Click(object sender, EventArgs e)
        {

        }

        private void btnBTMDeficiency_Click(object sender, EventArgs e)
        {
            PrintDeficiency();
        }

        private void PrintDeficiency()
        {
            frmAppForm PrintClass = new frmAppForm();
            PrintClass.ApplType = "Business Mapping Deficiency";
            PrintClass.BIN = bin1.GetBin();
            PrintClass.ShowDialog();
        }

        /*public bool ValidateMapping(string strBIN)
        {
            OracleResultSet pTmp = new OracleResultSet();

            pTmp.Query = "select * from btm_update where bin = '" + strBIN + "' and trim(def_settled) is null";
            if (pTmp.Execute())
            {
                if (pTmp.Read())
                {
                    pTmp.Close();
                    return true;
                }
                else
                {
                    pTmp.Close();
                    return false;
                }
            }

            return true;
        }*/
        // RMC 20111220 transferred validation of business if business-mapped in AppSettingsManager

        private void btnForm_Click(object sender, EventArgs e)
        {
            if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
            {
                frmAppForm PrintAppForm = new frmAppForm();
                PrintAppForm.BIN = bin1.GetBin();
                if (m_sFormStatus == "REN-APP" || m_sFormStatus == "REN-APP-EDIT")
                    PrintAppForm.ApplType = "REN";
                PrintAppForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select BIN first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }

        private void btnReconcile_Click(object sender, EventArgs e)
        {

        }

        private void LoadLandPIN(string sBrgyName)
        {
            try
            {
                OracleResultSet result = new OracleResultSet();
                OracleResultSet pGisRec = new OracleResultSet();
                pGisRec.CreateNewConnectionGIS();
                string sBrgyCode = "";

                result.Query = string.Format("select * from brgy where brgy_nm = '{0}'", sBrgyName);
                if (result.Execute())
                {
                    if (result.Read())
                        sBrgyCode = result.GetString("brgy_code");
                }
                result.Close();

                cmbLandPIN.Items.Clear();
                pGisRec.Query = "select distinct pin from GIS_BUSINESS_LOCATION where substr(pin,8,3) = '" + sBrgyCode + "' order by pin";
                if (pGisRec.Execute())
                {
                    cmbLandPIN.Items.Add("");
                    while (pGisRec.Read())
                    {
                        cmbLandPIN.Items.Add(pGisRec.GetString(0));
                    }
                }
                pGisRec.Close();
            }
            catch { }

        }

        private void LoadBldgCode(string sLandPIN)
        {
            try
            {
                OracleResultSet pGisRec = new OracleResultSet();
                pGisRec.CreateNewConnectionGIS();

                cmbBldgCode.Items.Clear();
                pGisRec.Query = "select * from gis_business_location where pin = '" + sLandPIN + "' order by bldg_code";
                if (pGisRec.Execute())
                {
                    cmbBldgCode.Items.Add("");

                    while (pGisRec.Read())
                    {
                        cmbBldgCode.Items.Add(pGisRec.GetString("bldg_code"));
                    }
                }
                pGisRec.Close();
            }
            catch { }
        }

        private void LoadGisValues(string sBIN)
        {
            OracleResultSet result = new OracleResultSet();
            //cmbLandPIN.Enabled = true;
            //cmbBldgCode.Enabled = true;
            
            result.Query = "select * from btm_gis_loc where bin = '" + sBIN + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    cmbLandPIN.Text = result.GetString("land_pin");

                    if (cmbLandPIN.Text.ToString() != "")
                    {
                        cmbBldgCode.Text = result.GetString("bldg_code");

                        cmbLandPIN.Enabled = false;
                        cmbBldgCode.Enabled = false;

                        chkGISLink.Enabled = false;
                    }
                }
                else
                {
                    if (Granted.Grant("ABM"))
                        chkGISLink.Enabled = true;
                    else
                        chkGISLink.Enabled = false;
                }
            }
            result.Close();
        }

        private void cmbBldgCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBldgName(cmbLandPIN.Text.ToString(), cmbBldgCode.Text.ToString());
        }

        private void cmbLandPIN_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadBldgCode(cmbLandPIN.Text.Trim());
        }

        private void LoadBldgName(string sLandPIN, string sBldgCode)
        {
            try
            {
                OracleResultSet pGisRec = new OracleResultSet();
                pGisRec.CreateNewConnectionGIS();

                pGisRec.Query = "select * from gis_business_location where pin = '" + sLandPIN + "' and bldg_code = '" + sBldgCode + "'";
                if (pGisRec.Execute())
                {
                    if (pGisRec.Read())
                    {
                        txtBldgName.Text = pGisRec.GetString("bldg_name");
                    }
                }
                pGisRec.Close();
            }
            catch { }
        }

        private void btnOwnProfile_Click(object sender, EventArgs e)
        {
            if (txtOwnCode.Text == string.Empty)
                txtOwnCode.Text = AppSettingsManager.EnlistOwner(txtOwnLn.Text.Trim(), txtOwnFn.Text.Trim(), txtOwnMi.Text.Trim(), txtOwnAddNo.Text.Trim(), txtOwnStreet.Text.Trim(), cmbOwnDist.Text.Trim().ToUpper(), "", cmbOwnBrgy.Text.Trim().ToUpper(), txtOwnMun.Text.Trim(), txtOwnProv.Text.Trim(), txtOwnZip.Text.Trim());
            
            m_bOwnProfileEdited = true; //JARS 20170807
            frmOwnerProfile frmOwnProfileClass = new frmOwnerProfile();
            frmOwnProfileClass.OwnCode = txtOwnCode.Text.Trim();
            frmOwnProfileClass.ShowDialog();
            m_sTmpBuss_Own_Code = frmOwnProfileClass.OwnCode;
        }

        private void btnBnsOwnProfile_Click(object sender, EventArgs e)
        {
            if (txtBnsOwnCode.Text == string.Empty)
              txtBnsOwnCode.Text = AppSettingsManager.EnlistOwner(txtBnsOwnLn.Text.Trim(), txtBnsOwnFn.Text.Trim(), txtBnsOwnMi.Text.Trim(), txtBnsOwnAddNo.Text.Trim(), txtBnsOwnStreet.Text.Trim(), cmbBnsDist.Text.Trim().ToUpper(), "", cmbBnsOwnBrgy.Text.Trim().ToUpper(), txtBnsOwnMun.Text.Trim(), txtBnsOwnProv.Text.Trim(), txtBnsOwnZip.Text.Trim());

            frmOwnerProfile frmOwnProfileClass = new frmOwnerProfile();
            frmOwnProfileClass.OwnCode = txtBnsOwnCode.Text.Trim();
            frmOwnProfileClass.ShowDialog();
            m_sTmpBuss_BusnCode = frmOwnProfileClass.OwnCode;
        }

        private void btnPrevOwnProfile_Click(object sender, EventArgs e)
        {
            frmOwnerProfile frmOwnProfileClass = new frmOwnerProfile();
            frmOwnProfileClass.OwnCode = txtPrevOwnCode.Text.Trim();
            frmOwnProfileClass.ShowDialog();
            m_sTmpBuss_Prev_BnsOwn = frmOwnProfileClass.OwnCode;
            
        }

        private void chkGISLink_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkGISLink.CheckState.ToString() == "Checked")
            {
                cmbLandPIN.Enabled = true;
                cmbBldgCode.Enabled = true;
            }
            else
            {
                cmbLandPIN.Enabled = false;
                cmbBldgCode.Enabled = false;

                cmbLandPIN.Text = "";
                cmbBldgCode.Text = "";
                txtBldgName.Text = "";
            }
        }

        private void btnViewFiles_Click(object sender, EventArgs e)
        {
            // RMC 20111206 Added viewing of blob

            if (m_frmImageList.IsDisposed)
            {
                m_intImageListInstance = 0;
                m_frmImageList = new frmImageList();
                m_frmImageList.IsBuildUpPosting = true;
            }
            if (!m_frmImageList.IsDisposed && m_intImageListInstance == 0)
            {
                //if (m_frmImageList.ValidateImage(bin1.GetBin(), "A"))
                if (m_frmImageList.ValidateImage(bin1.GetBin(), AppSettingsManager.GetSystemType)) //MCR 20141209
                {
                    ImageInfo objImageInfo;
                    objImageInfo = new ImageInfo();

                    objImageInfo.TRN = bin1.GetBin();
                    //objImageInfo.System = "A"; 
                    objImageInfo.System = AppSettingsManager.GetSystemType; //MCR 20121209
                    m_frmImageList.isFortagging = false;
                    m_frmImageList.setImageInfo(objImageInfo);
                    m_frmImageList.Text = bin1.GetBin();
                    m_frmImageList.IsAutoDisplay = true;
                    m_frmImageList.Source = "VIEW";
                    m_frmImageList.Show();
                    m_intImageListInstance += 1;
                }
                else
                {

                    MessageBox.Show(string.Format("BIN {0} has no image", bin1.GetBin()));
                }

            }
        }

        private void btnAttachFiles_Click(object sender, EventArgs e)
        {
            this.LoadImageList(); // CJC 20130401         
        }

        private void LoadImageList()
        {
            // RMC 20111206 Added attachment of blob image

            if (m_frmImageList.IsDisposed)
            {
                m_intImageListInstance = 0;
                m_frmImageList = new frmImageList();
                m_frmImageList.IsBuildUpPosting = true;
            }
            if (!m_frmImageList.IsDisposed && m_intImageListInstance == 0)
            {
                ImageInfo objImageInfo;
                //objImageInfo = new ImageInfo("A", AppSettingsManager.SystemUser.UserCode);  // RMC 20111206
                objImageInfo = new ImageInfo(AppSettingsManager.GetSystemType, AppSettingsManager.SystemUser.UserCode);  // MCR 20141209

                //JVL20100107(s)
                /*if (state == PostingState.Add)
                {
                    objImageInfo = new ImageInfo("A", AppSettingsManager.SystemUser.UserCode);
                }
                else if (state == PostingState.Edit)
                {
                    //objImageInfo = new ImageInfo();
                    //objImageInfo.System = "T";
                    objImageInfo = new ImageInfo("A", AppSettingsManager.SystemUser.UserCode);
                    m_blnIsEditAttach = true;
                }
                else //modify this condition if you need to add different scenario for posting delete or posting view
                {
                    objImageInfo = new ImageInfo("A", AppSettingsManager.SystemUser.UserCode);
                }
                //JVL20100107(e)*/
                // RMC 20111206 put rem

                m_frmImageList.Text = string.Format("Assigned Images - {0}", AppSettingsManager.SystemUser.UserCode);
                m_frmImageList.setImageInfo(objImageInfo);
                m_frmImageList.isFortagging = true;

                //m_frmImageList.TopMost = true;
                //m_frmImageList.IsBuildUp = true;
                // RMC 20111206 Added attachment of blob image (s)
                if (m_sFormStatus == "BUSS-ADD-NEW" || m_sFormStatus == "BUSS-EDIT")
                    m_frmImageList.IsBuildUp = true;
                else
                    m_frmImageList.IsBuildUp = false;
                m_frmImageList.Source = "ATTACH";
                // RMC 20111206 Added attachment of blob image (e)

                m_frmImageList.IsAutoDisplay = true;
                //m_frmImageList.Show(this.ApplicationFrm);
                m_frmImageList.TopMost = true; // CJC 20130401
                //m_frmImageList.Show();
                m_frmImageList.Show();
                m_intImageListInstance += 1;
            }
            else 
            {
                // AST 20150316 Added This Block (s)
                ImageInfo objImageInfo;
                objImageInfo = new ImageInfo(AppSettingsManager.GetSystemType, AppSettingsManager.SystemUser.UserCode);  // MCR 20141209
                m_frmImageList.Text = string.Format("Assigned Images - {0}", AppSettingsManager.SystemUser.UserCode);
                m_frmImageList.setImageInfo(objImageInfo);
                m_frmImageList.isFortagging = true;
                
                if (m_sFormStatus == "BUSS-ADD-NEW" || m_sFormStatus == "BUSS-EDIT")
                    m_frmImageList.IsBuildUp = true;
                else
                    m_frmImageList.IsBuildUp = false;
                m_frmImageList.Source = "ATTACH";

                m_frmImageList.IsAutoDisplay = true;
                m_frmImageList.TopMost = true;
                m_frmImageList.ShowDialog();
                m_intImageListInstance += 1;
                // AST 20150316 Added This Block (e)
            }
        }

        private void SaveImage()
        {
            
            if (m_frmImageList.GetRecentImageID != 0)   // image already in database (used in build-up)
            {
                string strImageFile = m_frmImageList.GetRecentImageFileNameDisplay;
                if (strImageFile != null && strImageFile != "")
                {
                    ImageInfo objImageInfo;
                    //objImageInfo = new ImageInfo(bin1.GetBin(), "A", AppSettingsManager.SystemUser.UserCode);   // RMC 20120119 corrected saving of image in Business records
                    objImageInfo = new ImageInfo(bin1.GetBin(), AppSettingsManager.GetSystemType, AppSettingsManager.SystemUser.UserCode);   // MCR 20141209

                    if (!m_frmImageList.UpdateBlobImage(objImageInfo))
                    {
                        //pSet.Rollback();
                        //pSet.Close();
                        MessageBox.Show("Failed to attach image.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                else
                {
                    //pSet.Rollback();
                    //pSet.Close();
                    MessageBox.Show("Failed to attach image.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

            }
            else
            {
                // for browsed local file to be inserted in database
                string strImageFile = m_frmImageList.GetRecentImageFileNameDisplay;
                string strImageName = System.IO.Path.GetFileName(strImageFile);

                if (strImageFile != "")
                {
                    if (MessageBox.Show(this, string.Format("Do you want to attach the image {0} to BIN {1}", strImageName, bin1.GetBin()), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        ImageTransation objTransaction = new ImageTransation();

                        if (!(objTransaction.InsertImage(bin1.GetBin(), strImageFile, cmbBnsBrgy.Text.Trim())))
                        {
                            MessageBox.Show("Failed to attach image.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        else
                        {

                            m_frmImageList.Close();
                            MessageBox.Show("Image attached.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
             
        }

        private void btnDetachFiles_Click(object sender, EventArgs e)
        {
            if (m_frmImageList.GetRecentImageID == 0)
            {
                MessageBox.Show("View image to detach first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Do you want to detach file : " + m_frmImageList.GetRecentImageFileNameDisplay + " from BIN: " + bin1.GetBin() + ".", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (m_frmImageList.GetRecentImageID != 0)
                {
                    string strImageFile = m_frmImageList.GetRecentImageFileNameDisplay;
                    if (strImageFile != null && strImageFile != "")
                    {
                        ImageTransation objTransaction = new ImageTransation();
                        objTransaction.FileName = strImageFile; // AST 20150430
                        if (!(objTransaction.DetachImage(bin1.GetBin(), m_frmImageList.GetRecentImageID)))
                        {
                            MessageBox.Show("Failed to detach image.","Business Records",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                            return;
                        }
                        else
                        {

                            m_frmImageList.Close();
                            MessageBox.Show("Image detached","Business Records",MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        

                    }
                    else
                        MessageBox.Show("View image to detach first.","Business Records",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("View image to detach first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void DetachImage()
        {
            ImageTransation objTransaction = new ImageTransation();
            if (!(objTransaction.DetachImage(bin1.GetBin())))
            {
                MessageBox.Show("Failed to detach image from this record.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                MessageBox.Show("Image was detached from this record.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void frmBusinessRecord_Enter(object sender, EventArgs e)
        {
            
        }

        private void frmBusinessRecord_Click(object sender, EventArgs e)
        {
            
        }

        private void frmBusinessRecord_Shown(object sender, EventArgs e)
        {
            m_frmImageList.TopMost = false; // CJC 20130401
        }

        private void frmBusinessRecord_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void btnOtherBnsInfo_Click(object sender, EventArgs e)
        {
            frmOtherBnsInfo form = new frmOtherBnsInfo();
            if (m_sFormStatus == "BUSS-ADD-NEW" || m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")
            {
                // RMC 20150102 mods in permit (s)
                if (m_strInspectionNo != "")
                    m_strTempBIN = m_strInspectionNo;
                // RMC 20150102 mods in permit (e)

                // RMC 20161121 corrected other business info not saved (s)
                if (m_strTempBIN == "")
                {
                    DateTime dtCurrent = AppSettingsManager.GetSystemDate();
                    dtCurrent.Year.ToString();
                    m_strTempBIN = string.Format("{0:0000#}-{1:00#}-{2:00#}-{3:00#}:{4:00#}:{5:00#}", dtCurrent.Year.ToString(), dtCurrent.Month.ToString(), dtCurrent.Day.ToString(), dtCurrent.Hour.ToString(), dtCurrent.Minute.ToString(), dtCurrent.Second.ToString());
                }
                // RMC 20161121 corrected other business info not saved (e)

                if (form.BIN == "" && m_strTempBIN != "")
                {
                    form.BIN = m_strTempBIN;
                    
                }
            }
            else
                form.BIN = bin1.GetBin();
            
            form.ShowDialog();
        }

        private void LoadValuesZoning()
        {
            // RMC 20141228 modified permit printing (lubao)
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select * from emp_names where temp_bin = '" + m_strInspectionNo + "'";
            if(pRec.Execute())
            {
                if(pRec.Read())
                {
                    /*txtOwnLn.Text = pRec.GetString("emp_ln");
                    txtOwnFn.Text = pRec.GetString("emp_fn");
                    txtOwnMi.Text = pRec.GetString("emp_mi");*/ // RMC 20150117
                    
                    txtBnsName.Text = StringUtilities.StringUtilities.RemoveApostrophe(pRec.GetString("bns_nm").Trim());
                    txtBnsAddNo.Text = pRec.GetString("bns_add").Trim();
                }
            }
            pRec.Close();

            // RMC 20150117 (S)
            string sLN = ""; string sFN = ""; string sMI = ""; string sOwnCode = "";
            pRec.Query = "select * from emp_names where temp_bin = '" + m_strInspectionNo + "'";
            pRec.Query += " and emp_occupation = 'OWNER'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sLN = pRec.GetString("emp_ln").Trim();
                    sFN = pRec.GetString("emp_fn").Trim();
                    sMI = pRec.GetString("emp_mi").Trim();

                    pRec.Close();

                    pRec.Query = "select * from own_names where trim(own_ln) = '" + sLN + "'";
                    if (sFN != "")
                        pRec.Query += " and trim(own_fn) = '" + sFN + "'";
                    if (sMI != "")
                        pRec.Query += " and trim(own_mi) = '" + sMI + "'";
                    pRec.Query += " order by own_code desc";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sOwnCode = pRec.GetString("own_code");

                            BPLSAppSettingList sOwnList = new BPLSAppSettingList();
                            sOwnList.OwnName = sOwnCode;

                            for (int j = 0; j < sOwnList.OwnNamesSetting.Count; j++)
                            {
                                txtOwnCode.Text = sOwnList.OwnNamesSetting[j].sOwnerCode;
                                txtOwnLn.Text = sOwnList.OwnNamesSetting[j].sLn;
                                txtOwnFn.Text = sOwnList.OwnNamesSetting[j].sFn;
                                txtOwnMi.Text = sOwnList.OwnNamesSetting[j].sMi;
                                txtOwnAddNo.Text = sOwnList.OwnNamesSetting[j].sOwnHouseNo;
                                txtOwnStreet.Text = sOwnList.OwnNamesSetting[j].sOwnStreet;
                                cmbOwnBrgy.Text = sOwnList.OwnNamesSetting[j].sOwnBrgy;
                                cmbOwnDist.Text = sOwnList.OwnNamesSetting[j].sOwnDist;
                                txtOwnMun.Text = sOwnList.OwnNamesSetting[j].sOwnMun;
                                txtOwnProv.Text = sOwnList.OwnNamesSetting[j].sOwnProv;
                                txtOwnZip.Text = sOwnList.OwnNamesSetting[j].sOwnZip;
                            }
                        }
                        else
                        {
                            txtOwnLn.Text = sLN;
                            txtOwnFn.Text = sFN;
                            txtOwnMi.Text = sMI;

                        }
                    }
                    pRec.Close();
                }
                else
                {
                    txtOwnLn.Text = "";
                    txtOwnFn.Text = "";
                    txtOwnMi.Text = "";
                }
            }
            pRec.Close();
            // RMC 20150117 (E)

            pRec.Query = "delete from addl_info_tmp where bin = '" + m_strInspectionNo + "'";
            if (pRec.ExecuteNonQuery() == 0)
            { }

            pRec.Query = "insert into addl_info_tmp ";
            pRec.Query += "select * from addl_info where bin = '" + m_strInspectionNo + "'";
            if (pRec.ExecuteNonQuery() == 0)
            { }
             
        }


        private void frmBusinessRecord_FormClosed_1(object sender, FormClosedEventArgs e)
        {//GMC 20150820 Perform Parent Click Button New Application(s)
            if (bAddNewApp)
            {
                bAddNewApp = false;
                ((KryptonRibbonGroupButton)this.sender).PerformClick();
            }
        }//GMC 20150820 Perform Parent Click Button New Application(e)
        private void frmBusinessRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            // AST 20150312 remove this line below
            /*if (MessageBox.Show("Are you sure you want to close this module?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;*/
            //if (!bAddNewApp)//GMC 20150819 If this form is not reopen again
            if (!bAddNewApp && !m_bAddNew)   // RMC 20161121 corrections in adding new record button
            {
                if (MessageBox.Show("Are you sure you want to close this module?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // AST 20150312
                {
                    bState = true;
                    this.Opacity = 1.0;
                    timerBnsRec.Enabled = true;

                    // RMC 20141217 adjustments (s)
                    OracleResultSet pCmd = new OracleResultSet();
                    pCmd.Query = "delete from addl_info_tmp where bin = '" + bin1.GetBin() + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }
                    // RMC 20141217 adjustments (e)

                    if (TaskMan.IsObjectLock(bin1.GetBin(), m_sFormStatus, "DELETE", "ASS"))
                    {
                    }

                    m_bAddNew = false;
                    m_frmImageList.Close();


                    //// AST 20150325 Added Kill of Image view (s)                
                    //foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcessesByName("frmDatabaseInfo.ActiveForm.Name", Environment.MachineName))
                    //{
                    //    proc.Kill();
                    //}
                    //// AST 20150325 Added Kill of Image view (e)
                }
                else
                {
                    e.Cancel = true; // AST 20150312
                }
            }
        }
        public void ReOpen() {
            this.Show();
        }
        
        /// <summary>
        /// AST 20150312 Added this event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBnsOwnLn_Leave(object sender, EventArgs e)
        {
            // RMC 20150807 modified searching of owners in Business records
            if (txtBnsOwnLn.Text.Trim() != "")
            {
                frmSearchOwner SearchOwner = new frmSearchOwner();
                SearchOwner.m_sPageWatch = "PAGE2";
                SearchOwner.OwnLastName = txtBnsOwnLn.Text.Trim();
                SearchOwner.GetValues();    
                if (!SearchOwner.m_bNoRecordFound)   
                    SearchOwner.ShowDialog();

                if (SearchOwner.m_strOwnCode != "")
                {
                    txtBnsOwnCode.Text = SearchOwner.m_strOwnCode;
                    txtBnsOwnLn.Text = SearchOwner.m_sOwnLn;
                    txtBnsOwnFn.Text = SearchOwner.m_sOwnFn;
                    txtBnsOwnMi.Text = SearchOwner.m_sOwnMi;
                    txtBnsOwnAddNo.Text = SearchOwner.m_sOwnAdd;
                    txtBnsOwnStreet.Text = SearchOwner.m_sOwnStreet;
                    cmbBnsOwnBrgy.Text = SearchOwner.m_sOwnBrgy;
                    cmbBnsOwnDist.Text = SearchOwner.m_sOwnDist;
                    txtBnsOwnMun.Text = SearchOwner.m_sOwnMun;
                    txtBnsOwnProv.Text = SearchOwner.m_sOwnProv;
                    txtBnsOwnZip.Text = SearchOwner.m_sOwnZip;

                }
                else
                {
                    txtBnsOwnCode.Text = "";
                    txtBnsOwnFn.Text = "";
                    txtBnsOwnMi.Text = "";
                    txtBnsOwnAddNo.Text = "";
                    txtBnsOwnStreet.Text = "";
                    cmbBnsOwnBrgy.Text = "";
                    cmbBnsOwnDist.Text = "";
                    txtBnsOwnMun.Text = "";
                    txtBnsOwnProv.Text = "";
                    txtBnsOwnZip.Text = "";

                }
            }
        }

        private void txtPrevOwnLn_Leave(object sender, EventArgs e)
        {
            
            // RMC 20150807 modified searching of owners in Business records
            if (txtPrevOwnLn.Text.Trim() != "")
            {
                frmSearchOwner SearchOwner = new frmSearchOwner();
                SearchOwner.m_sPageWatch = "PAGE2";
                SearchOwner.OwnLastName = txtPrevOwnLn.Text.Trim();
                SearchOwner.GetValues();    
                if (!SearchOwner.m_bNoRecordFound)   
                    SearchOwner.ShowDialog();

                if (SearchOwner.m_strOwnCode != "")
                {
                    txtPrevOwnCode.Text = SearchOwner.m_strOwnCode;
                    txtPrevOwnLn.Text = SearchOwner.m_sOwnLn;
                    txtPrevOwnFn.Text = SearchOwner.m_sOwnFn;
                    txtPrevOwnMi.Text = SearchOwner.m_sOwnMi;
                    txtPrevOwnAddNo.Text = SearchOwner.m_sOwnAdd;
                    txtPrevOwnStreet.Text = SearchOwner.m_sOwnStreet;
                    cmbPrevOwnBrgy.Text = SearchOwner.m_sOwnBrgy;
                    cmbPrevOwnDist.Text = SearchOwner.m_sOwnDist;
                    txtPrevOwnMun.Text = SearchOwner.m_sOwnMun;
                    txtPrevOwnProv.Text = SearchOwner.m_sOwnProv;
                    txtPrevOwnZip.Text = SearchOwner.m_sOwnZip;

                }
                else
                {
                    txtPrevOwnCode.Text = "";
                    txtPrevOwnFn.Text = "";
                    txtPrevOwnMi.Text = "";
                    txtPrevOwnAddNo.Text = "";
                    txtPrevOwnStreet.Text = "";
                    cmbPrevOwnBrgy.Text = "";
                    cmbPrevOwnDist.Text = "";
                    txtPrevOwnMun.Text = "";
                    txtPrevOwnProv.Text = "";
                    txtPrevOwnZip.Text = "";
                }
            }
        }

        private void containerWithShadow1_Load(object sender, EventArgs e)
        {

        }
        // JAV 20170927 (s) // JAV 20170928 DTI Expiration
        //private void dtpDTIIssued_ValueChanged(object sender, EventArgs e)
        //{
            
        //    DateTime dtFrom = dtpDTIIssued.Value;
        //    DateTime dtTo = DateTime.Now;
            

        //    DateExpiration(dtFrom, dtTo);
        //}

        public void DateExpiration(DateTime d1, DateTime d2) 
        {
            try
            {
                BackCodeClass backClass = new BackCodeClass(this);
                DateTime expiry = d1.AddYears(int.Parse(AppSettingsManager.GetConfigObject("43"))); //JARS 20171023 ON SITE: CHANGED TO VALUE 43 //CHANGED TO OBJECT

                //MessageBox.Show("Current date is " + d2.ToString()); for checking the current date 
                //MessageBox.Show("Expiry date is " + expiry.ToString()); for checking the expiry date

                if (expiry < d2)
                {
                    if (MessageBox.Show("DTI Permit is already Expired do you want to continue?", "DTI/SEC", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (MessageBox.Show("Save renewal application?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            backClass.AppRenSave();
                            SaveImage();

                            if (CheckAddlBns())
                                btnAddlBns_Click(sender, e);
                        }
                    }
                    else
                        return;
                }
                else
                {
                    if (MessageBox.Show("DTI Permit is not Expired!", "DTI/SEC", MessageBoxButtons.OK, MessageBoxIcon.None) == DialogResult.OK)
                    {
                        if (MessageBox.Show("Save renewal application?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            backClass.AppRenSave();
                            SaveImage();

                            // RMC 20180103 corrected error in Renewal application of additional line (s)
                            if (CheckAddlBns())
                                btnAddlBns_Click(sender, e);
                            // RMC 20180103 corrected error in Renewal application of additional line (e)
                        }
                    }
                }

                //return;
            }
            catch
            {
                // RMC 20171116 correction in validating DTI expiration, transferred from btnSave_Click()
                MessageBox.Show("Please input in your SYSTEM CONFIG the expiration of DTI registrations", "Application", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                return;
                // RMC 20171116 correction in validating DTI expiration, transferred from btnSave_Click()
            }
        }
        // JAV 20170927 (e) // JAV 20170928 DTI Expiration

        private bool CheckifTaggedViolation()    
        {
            String sValueViolation = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select distinct VT.VIOLATION_DESC From VIOLATION_TABLE VT  left join violations VS on VS.VIOLATION_CODE = VT.VIOLATION_CODE where VS.Bin = '" + bin1.GetBin().Trim() + "'";
            if (pSet.Execute())
                while (pSet.Read())
                    sValueViolation += pSet.GetString(0) + "\n";
            pSet.Close();

            if (sValueViolation.Length > 0)
                sValueViolation = sValueViolation.Remove(sValueViolation.ToString().Length - 1, 1).Trim();

            if (sValueViolation.Length > 0)
            {
                MessageBox.Show("BIN has been tagged for a violation of\n" + sValueViolation, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
                return false;
        }

        

        
    }

}