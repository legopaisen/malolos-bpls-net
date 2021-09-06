//#######################################
// Modifications: 
// ALJ 20130219 NEW Discovery - delinquent 
// ALJ 20130115 add due_state <> 'A' to avoid billing of rev adj in regular billing
// ALJ 20120309 Rev exam for new
// RMC 20120119 disable validation of zero gross for NEW business in Billing
// RMC 20120118 modified billing for no operation/no gross
// RMC 20120111 added validation to accept bns with no computed tax if exempted in billing
// ALJ 20120109 Message Prompt from bill_btax proc if no schedule found of no data in other info 
// RMC 20120103 modified billing for delinquent years
// ALJ 20111227 save to for SOA Printing
// ALJ 20110907 re-assessment validation   
// ALJ 20110805 Check if has permit update or retirement application      
// ALJ 20110802 Special Tax Exemption (PEZA, BOI)
// ALJ 20110725 lgu specific fee
// RMC 20110414 Changed declaration of AddlInfo             
// ALJ 20100708 QTR DEC  
//#######################################
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.ComponentModel;
using Amellar.Common.BPLSApp;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AddlInfo;
using Amellar.Common.StringUtilities;


namespace Amellar.BPLS.Billing
{
    /// <summary>
    /// ALJ 20090701
    /// Class for billing of taxes and fees
    /// </summary>
    public class Billing
    {
        protected AddlInfo AddlInfoClass = new AddlInfo();   // RMC 20110414
        protected frmBilling BillingForm = null;
        protected Library LibraryClass = new Library();

        private string m_sLatestApplTaxYear;
        protected string m_sOwnCode;
        protected string m_sMainBnsCode;
        protected string m_sMainBnsStat;
        protected string m_sDueState;
        private string m_sIsQtrly;
        protected string m_sRevYear;
        protected int m_iSwRE;
        protected string m_sCurrentYear;
        protected string m_sCurrentQtr;
        protected double m_fMainGross;
        protected double m_fMainCapital;
        protected string m_sDtOperated; // ALJ 20120309 Rev exam for new - changed to protected
        //private string m_sPUTApplType;
        public string m_sPUTApplType;   // RMC 20150108
        private string m_sGrossParam;
        private string m_sCapitalParam; // ALJ 20120309 Rev exam for new
        public bool m_bPermitUpdateFlag = false;    // RMC 20150325 modified billing for permit update application, initialized
        public bool m_bIsRenPUT;
        public bool m_bIsNoSale;    // RMC 20180125 enable soa if other line of business has no operation/sale, change private to public
        private bool m_bIsReg;
        private bool m_bIsBillOK = true;
        private bool m_bIsBillCreated = false;
        protected bool m_bIsInitLoad = true;
        protected string m_sBaseDueYear, m_sBaseDueQtr, m_sMaxDueYear, m_sMaxDueQtr;
        private string m_sHoldTaxYear = string.Empty;
        private string m_sHoldQtr = string.Empty;
        private string m_sHoldBnsCode = string.Empty;
        // CJC 2011 (s)
        protected bool m_blnNewAddl = false;
        protected bool m_blnTransLoc = false;
        protected bool m_blnTransOwn = false;
        protected bool m_blnChClass = false;
        protected bool m_blnChBnsName = false;
        protected bool m_blnChOrgKind = false;   // RMC 20140725 Added Permit update billing
        // CJC 2011 (e)

        // RMC 20140725 Added Permit update billing (s)
        protected string m_sPrevOwnCode = string.Empty;
        protected string m_sNewBnsLoc = string.Empty;
        protected string m_sPrevBnsLoc = string.Empty;
        protected string m_sOldBnsName = string.Empty;
        // RMC 20140725 Added Permit update billing (e)

        //public bool IsBillOK { get { return m_bIsBillOK; } }
        public bool IsBillCreated { get { return m_bIsBillCreated; } }
        public string BaseTaxYear { get { return m_sBaseDueYear; } }
        public string BaseDueQtr { get { return m_sBaseDueQtr; } }
        public string MaxDueQtr { get { return m_sMaxDueQtr; } } // ALJ 20110309
        public string IsQtrly { get { return m_sIsQtrly; } }
        public string CurrentYear { get { return m_sCurrentYear; } }
        public string CurrentQtr { get { return m_sCurrentQtr; } }
        //public bool IsInitialLoad { get { return m_bIsInitLoad; } }   // RMC 20160222 added validation in billing if billing exists, force user to cancel billing first  due to acceleration clause - Tumauini, put rem
        // RMC 20160222 added validation in billing if billing exists, force user to cancel billing first  due to acceleration clause - Tumauini (s)
        public bool IsInitialLoad 
        { 
            get { return m_bIsInitLoad; }
            set { m_bIsInitLoad = value; }
        }
        // RMC 20160222 added validation in billing if billing exists, force user to cancel billing first  due to acceleration clause - Tumauini (e)
        public int IsRevExam { get { return m_iSwRE; } }

        // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
        // AST 20150112 I include key word "Tmp" in variable to avoid accidentally overriding of value.
        private bool m_HasUnsettledQtr = false; // AST 20150112
        private string m_strTmpTaxYear = string.Empty; // AST 20150112
        private string m_strTmpQtr = string.Empty; // AST 20150112
        private string m_strTmpBnsStat = string.Empty; // AST 20150112
        private string m_strTmpQtrToPay = string.Empty; // AST 20150112
        public bool g_RemoveBusinessQue = false; // AST 20150114
        // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez(e)

        public Billing(frmBilling Form)
        {
            this.BillingForm = Form;
            m_sRevYear = ConfigurationAttributes.RevYear;
            m_sCurrentYear = ConfigurationAttributes.CurrentYear;
            m_sCurrentQtr = LibraryClass.GetQtr(AppSettingsManager.GetSystemDate());

        }

        public virtual void ReturnValues()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iBaseDueYear, iCurrentYear, iCurrentQtr, iMaxDueYear, iTaxYear, iMaxDueQtr, iBaseDueQtr, iQtr; // ALJ 20110309 iBaseDueQtr
            bool bIsBque = false;
            // /*test*/  bool bHasDueQtr = false; // ALJ 20110321 for QTR DEC
            m_bIsInitLoad = true;
            m_iSwRE = 0; // set revenue exam to 0 or regular billing;

            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
            // AST 20150112 For fixing billing issues when prev. tax-year qtr doesn't paid for qtrl'y deck(s)
            m_HasUnsettledQtr = this.ValidateQuarterPaid(BillingForm.BIN);
            // AST 20150112 For fixing billing issues when prev. tax-year qtr doesn't paid for qtrl'y deck(e)
            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (e)

            ///*MCR 20140902 REM SPLBNS
            bool blnIsSplBns = false;
            pSet.Query = "select * from spl_business_que where bin  = :1";
            pSet.AddParameter(":1", BillingForm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    blnIsSplBns = true;
                }
                else
                {
                    pSet.Close();
                    pSet.Query = "select * from spl_businesses where bin  = :1";
                    pSet.AddParameter(":1", BillingForm.BIN);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        { blnIsSplBns = true; }
                    }
                }
            }
            pSet.Close(); //MCR 20140902 REM SPLBNS 

            BillingForm.NewDiscovery = false;  // ALJ 20130219 NEW Discovery - delinquent
            if (!ValidApplication())
                return;

            if(blnIsSplBns) //MCR 20140902 REM SPLBNS (s)
                pSet.Query = "SELECT * FROM spl_business_que WHERE bin = :1";
            else

            pSet.Query = "SELECT * FROM business_que WHERE bin = :1";
            pSet.AddParameter(":1", BillingForm.BIN);
            if (pSet.Execute())
            {
                if (!pSet.Read())
                    bIsBque = false;
                else
                    bIsBque = true;
            }
            //if (!bIsBque)
            if (!bIsBque || m_HasUnsettledQtr) // AST 20150112 added HasUnsettledQtr    // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez
            {
                pSet.Query = "SELECT * FROM businesses WHERE bin = :1";
                pSet.AddParameter(":1", BillingForm.BIN);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        //if (AppSettingsManager.GetConfigValue("27") == "N") //MCR 20141017
                        //{
                            OracleResultSet pSetPay = new OracleResultSet();
                            pSetPay.Query = "SELECT DISTINCT(bin) FROM pay_hist WHERE bin = :1 AND data_mode <> 'UNP'";
                            pSetPay.AddParameter(":1", BillingForm.BIN);
                            if (pSetPay.Execute())
                            {
                                if (!pSetPay.Read())
                                {
                                    MessageBox.Show("Record Not Posted", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    pSetPay.Close();
                                    return;
                                }
                            }
                            pSetPay.Close();
                        //}
                    }
                    else
                    {
                        pSet.Close();
                        MessageBox.Show("Record Not Found", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }



            BillingForm.BillFlag = false;

            BillingForm.TaxYear = pSet.GetString("tax_year");
            m_sLatestApplTaxYear = BillingForm.TaxYear;
            BillingForm.BusinessName = pSet.GetString("bns_nm").Trim();
            BillingForm.BusinessAddress = AppSettingsManager.GetBnsAddress(BillingForm.BIN);
            m_sOwnCode = pSet.GetString("own_code").Trim();
            BillingForm.OwnersName = LibraryClass.GetOwnerName(2, m_sOwnCode);
            BillingForm.m_sBrgyCode = AppSettingsManager.GetBrgyCode(pSet.GetString("bns_brgy")); //MCR 20140929    // RMC 20150806 merged exemption by brgy from Mati

            m_sMainBnsCode = pSet.GetString("bns_code").Trim();
            m_sMainBnsStat = pSet.GetString("bns_stat");
            m_fMainCapital = pSet.GetDouble("capital");
            m_fMainGross = pSet.GetDouble("gr_1");
            BillingForm.BusinessCode = m_sMainBnsCode;
            BillingForm.Status = m_sMainBnsStat;
            BillingForm.Capital = string.Format("{0:#,##0.00}", m_fMainCapital);
            BillingForm.Gross = string.Format("{0:#,##0.00}", m_fMainGross);
            BillingForm.OrgnKind = pSet.GetString("orgn_kind"); // RMC 20180131 added validation of specific checklist in Billing

            m_sDtOperated = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("dt_operated"));
            if (BillingForm.Status == "NEW")
            {
                // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
                // AST 20150112 (s)
                if (m_HasUnsettledQtr)
                {
                    BillingForm.Quarter = m_strTmpQtrToPay;
                    m_sDueState = m_strTmpBnsStat.Substring(0, 1);
                }
                // AST 20150112 (e)
                // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (e)
                {
                    BillingForm.Quarter = LibraryClass.GetQtr(pSet.GetDateTime("dt_operated"));
                    m_sDueState = "N";// temporary
                }
            }
            else
            {
                // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
                // AST 20150112 (s)
                if (m_HasUnsettledQtr)
                {
                    BillingForm.Quarter = m_strTmpQtrToPay;
                    m_sDueState = m_strTmpBnsStat.Substring(0, 1);
                }
                // AST 20150112 (e)
                // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (e)
                else
                {
                    BillingForm.Quarter = "1";
                    m_sDueState = "R";// temporary 
                }
            }
            m_sPUTApplType = string.Empty;

            /*int.TryParse(BillingForm.TaxYear, out iTaxYear);
            int.TryParse(BillingForm.Quarter, out iQtr);*/  // RMC 20150112 mods in retirement billing, put rem
            int.TryParse(m_sCurrentYear, out iCurrentYear);
            int.TryParse(m_sCurrentQtr, out iCurrentQtr);



            // get then base due year and maximum due year
            m_sBaseDueYear = ""; // initialize
            m_sMaxDueYear = ""; // initialize
            GetMinMaxDueYear(BillingForm.BIN, out m_sBaseDueYear, out m_sMaxDueYear);
            if (m_sBaseDueYear == "")
                m_sBaseDueYear = BillingForm.TaxYear; // defalut value
            else
                BillingForm.TaxYear = m_sBaseDueYear;   // RMC 20150112 mods in retirement billing
            
            //BillingForm.Status = 

            if (m_sMaxDueYear == "")
                m_sMaxDueYear = BillingForm.TaxYear; // defalut value
            else
                BillingForm.TaxYear = m_sMaxDueYear;   // RMC 20150112 mods in retirement billing

            BillingForm.Status = DiscoveryDelinquentStat(BillingForm.Status);   // RMC 20150129
            int.TryParse(m_sMaxDueYear, out iMaxDueYear);
            //

            // Get the base due qtr and maximun due qtr
            // (s) ALJ 20100709 QTR DEC
            m_sBaseDueQtr = ""; // initialize
            m_sMaxDueQtr = ""; // initialize
            GetMinMaxDueQtr(BillingForm.BIN, m_sBaseDueYear, out m_sBaseDueQtr, out m_sMaxDueQtr);

            // RMC 20150112 mods in retirement billing (s)
            if (m_sBaseDueQtr != "")
                BillingForm.Quarter = m_sBaseDueQtr;
            int.TryParse(BillingForm.TaxYear, out iTaxYear);
            int.TryParse(BillingForm.Quarter, out iQtr);
            // RMC 20150112 mods in retirement billing (e)

            // get latest qtr if has payment and corect due state for QTR DEC
            if (BillingForm.Status == "NEW" && m_sIsQtrly == "Y")
            {
                OracleResultSet pSetQtr = new OracleResultSet();
                string sMaxQtr;
                sMaxQtr = string.Empty;
                pSetQtr.Query = "select max(qtr_paid) as max_qtr from pay_hist where bin = :1 and tax_year = :2";
                pSetQtr.AddParameter(":1", BillingForm.BIN);
                pSetQtr.AddParameter(":2", m_sBaseDueYear);
                if (pSetQtr.Execute())
                {
                    if (pSetQtr.Read())
                        sMaxQtr = pSetQtr.GetString("max_qtr");
                    if (sMaxQtr != string.Empty)
                    {
                        // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
                        // AST 20150112 (s)
                        // This is to get the latest qtr from pay_hist or billing_hist depends-
                        // on output of ValidateQtrPaid method.
                        if (m_HasUnsettledQtr)
                        {
                            sMaxQtr = m_strTmpQtrToPay;
                        }
                        // AST 20150112 (e)
                        else
                            sMaxQtr = AppSettingsManager.GetQtr(string.Format("{0:MM/dd/yyyy}", pSetQtr.GetDateTime("or_date")));
                        // CJC 20130412 (e)  
                        // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (e)

                        BillingForm.Quarter = sMaxQtr;
                        if (m_sBaseDueQtr == string.Empty && sMaxQtr != "4")
                        {
                            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
                            if (m_HasUnsettledQtr)
                            {
                                int.TryParse(sMaxQtr, out iMaxDueQtr);
                                m_sBaseDueQtr = sMaxQtr;
                            }// RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (e)
                            else
                            {
                                int.TryParse(sMaxQtr, out iMaxDueQtr);
                                m_sBaseDueQtr = string.Format("{0:0}", iMaxDueQtr + 1);
                                // /*test*/ bHasDueQtr = true; // ALJ 20110321
                            }
                        }// RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
                        else if (sMaxQtr == "4" && !m_HasUnsettledQtr) // AST 20150112 added m_HasUnsettledQtr
                            m_sBaseDueYear = string.Format("{0:0000}", Convert.ToInt32(m_sBaseDueYear) + 1); // CJC 20130111 QTR DEC
                        // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (e)
                        
                        m_sDueState = "Q";
                    }
                    else
                    {
                        if (m_sBaseDueQtr != string.Empty)
                        {
                            if (m_sBaseDueQtr == BillingForm.Quarter)
                                m_sDueState = "N";
                            else
                                m_sDueState = "Q";
                            // /*test*/ bHasDueQtr = true; // ALJ 20110321
                        }
                    }
                }
                pSetQtr.Close();
            }
            if (m_sBaseDueQtr == "")
                m_sBaseDueQtr = BillingForm.Quarter;
            if (m_sMaxDueQtr == "")
                m_sMaxDueQtr = BillingForm.Quarter;
            int.TryParse(m_sMaxDueQtr, out iMaxDueQtr);
            int.TryParse(m_sBaseDueQtr, out iBaseDueQtr); // ??
            //


            // (e) ALJ 20100709 QTR DEC
            /* // ALJ 20110907 remove the ff codes not needed due update in getminmaxdueyear 
            if (!bIsBque)
            {
                if (BillingForm.Status == "NEW" && m_sIsQtrly == "Y")
                {
                    // nothing to do;
                }
                else
                {
                    m_sBaseDueYear = string.Format("{0:0000}", iTaxYear + 1);
                }
            }*/
            int.TryParse(m_sBaseDueYear, out iBaseDueYear); // ??         


            /*if (iMaxDueYear >= iTaxYear)
            {
                BillingForm.TaxYear = m_sMaxDueYear;
                BillingForm.Quarter = m_sMaxDueQtr;
            }*/

            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
            //JDB 20141121 merged from Rod CJC 20140128 (s)
            if (iMaxDueYear > iTaxYear && (BillingForm.Status == "NEW" && m_sIsQtrly == "Y"))
            {
                BillingForm.TaxYear = string.Format("{0:0000}", iMaxDueYear + 1);
                BillingForm.Status = "REN";
                m_sDueState = "R";
                BillingForm.Quarter = "1";
                m_sMaxDueQtr = "1";
            }
            else if (iMaxDueYear >= iTaxYear)
            {
                // AST 20150114
                if (m_HasUnsettledQtr)
                {
                    m_sMaxDueYear = m_strTmpTaxYear;
                    m_sMaxDueQtr = m_strTmpQtrToPay;
                }

                //if (BillingForm.Status != "RET") //JARS 20170913
                if (BillingForm.Status == "NEW" && BillingForm.Status == "RET") //JARS 20170913
                {
                    BillingForm.TaxYear = m_sMaxDueYear;
                    BillingForm.Quarter = iCurrentQtr.ToString();
                    //BillingForm.Quarter = m_sMaxDueQtr;
                }
                else
                {
                    BillingForm.TaxYear = m_sMaxDueYear;
                    BillingForm.Quarter = m_sMaxDueQtr;
                }
            }
            //JDB 20141121 merged from Rod CJC 20140128 (e)
            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (e)

            PopulateBnsType();
            // RMC 20171123 enabled special business feature (s)
            if(blnIsSplBns) //MCR 20140902 REM SPLBNS
                SplPopulateGrossInfo(BillingForm.Status, m_sDueState, m_sMaxDueYear, m_sMaxDueQtr, BillingForm.TaxYear, BillingForm.Quarter, "REG"); // insert initial values for bill_gross_info table
            else// RMC 20171123 enabled special business feature (e)
            PopulateGrossInfo(BillingForm.Status, m_sDueState, m_sMaxDueYear, m_sMaxDueQtr, BillingForm.TaxYear, BillingForm.Quarter, "REG"); // insert initial values for bill_gross_info table
            LoadAddlInfo();
            LoadGrossCapital();

            if (!bIsBque || (BillingForm.Status == "NEW" && m_sIsQtrly == "Y")) // ALJ 20100708 QTR DEC || (BillingForm.Status == "NEW" && m_sIsQtrly == "Y")
            {
                // Note: pending qtr dec - max qtr + 1;
                // (s) ALJ 20100708 QTR DEC
                if (BillingForm.Status == "NEW" && m_sIsQtrly == "Y" && (iMaxDueQtr + 1 <= 4)) // ALJ 20110321
                {
                    // /*test*/ if (bHasDueQtr) // ALJ 20110321
                    // /*test*/ {
                    int.TryParse(BillingForm.TaxYear, out iTaxYear);
                    int.TryParse(BillingForm.Quarter, out iQtr);
                    if (iTaxYear == iCurrentYear)
                    {
                        if (iQtr < iCurrentQtr)
                        {
                            BillingForm.Quarter = string.Format("{0:0}", iMaxDueQtr + 1);
                            m_sDueState = "Q";
                        }
                    }
                    else
                    {
                        // AST 20150113 (s)
                        if (m_HasUnsettledQtr)
                        {
                            BillingForm.Quarter = m_strTmpQtrToPay;
                            m_sDueState = "Q";
                        }
                        // AST 20150113 (e)
                        else
                        {
                            BillingForm.Quarter = string.Format("{0:0}", iMaxDueQtr + 1);
                            m_sDueState = "Q";
                        }
                    }
                    // /*test*/ }

                }

                else
                {
                    //if (iMaxDueYear < iCurrentYear) // ALJ 20110309
                    if (iMaxDueYear < iCurrentYear && !m_HasUnsettledQtr) // ALJ 20110309 // AST 20150112 added m_HasUnsettledQtr   // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez
                    {
                        // (e) ALJ 20100708 QTR DEC
                        BillingForm.TaxYear = string.Format("{0:0000}", iMaxDueYear + 1);
                        BillingForm.Status = "REN";
                        m_sDueState = "R";
                        BillingForm.Quarter = "1";
                    }
                } // ALJ 20100708 QTR DEC
                UpdateOtherInfo();
                //UpdateBillGrossInfo();
                PopulateGrossInfo(BillingForm.Status, m_sDueState, m_sMaxDueYear, m_sMaxDueQtr, BillingForm.TaxYear, BillingForm.Quarter, "REG"); // insert initial values for bill_gross_info table
                if (BillingForm.Status == "NEW")
                    m_sMaxDueQtr = BillingForm.Quarter.ToString(); // ALJ 20110321


            }


            AddlInfoReadOnly(true);


            /* remove the ff code
            if (m_sBaseDueYear != BillingForm.TaxYear)
            {
                BillingForm.TaxYear = m_sBaseDueYear;
                if (BillingForm.Status == "NEW" && m_sIsQtrly == "Y")
                {
                    m_sBaseDueQtr = GetMinDueQtr(m_sBaseDueYear);
                    BillingForm.Quarter = m_sBaseDueQtr;
                }
                UpdateOtherInfo();
                UpdateBillGrossInfo();


            }
            */

            if (iBaseDueYear < iCurrentYear)
            {
                BillingForm.TaxYearTextBox.ReadOnly = false;
                BillingForm.TaxYearTextBox.Focus();
            }
            else
            {
                BillingForm.TaxYearTextBox.ReadOnly = true;
            }

            // (s) ALJ 20101907 QTR DEC
            if (BillingForm.Status == "NEW" && m_sIsQtrly == "Y")
            {
                BillingForm.QuarterTextBox.ReadOnly = false;
                BillingForm.QuarterTextBox.Focus();
            }
            // (e) ALJ 20101907 QTR DEC 

            pSet.Close();
            m_bIsInitLoad = false;
            BillingForm.EnabledControls(true);

            // RMC 20150117 (s)
            if (Convert.ToInt32(BillingForm.TaxYear) < Convert.ToInt32(ConfigurationAttributes.RevYear))
            {
                frmModifyDues formModify = new frmModifyDues();
                formModify.BIN = BillingForm.BIN;
                if (BillingForm.SourceClass == "Billing")
                    formModify.BnsStat = "R";
                formModify.TaxYear = BillingForm.TaxYear;
                formModify.ShowDialog();
                if (formModify.DataSaved)
                    ReturnValues();
                else
                    BillingForm.Close();
            }
            // RMC 20150117 (e)

            BillingForm.ValidatePYCNC();    // RMC 20180125 added validation of CNC for previous year
        }

        /// <summary>
        /// ALJ 20100216
        /// Function for auto-renewal application
        /// In C++ version this is the AutoApplication() function
        /// </summary>
        private bool ValidApplication()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBnsCode, sTaxYear, sQtr;
            string sNewBnsStat; // ALJ 20130219 NEW Discovery - delinquent
            int iTaxYear, iCurrentYear, iQtr, iCurrentQtr; // ALJ 20110309 iCurrentQtr

            // (s) ALJ 20110805 Check if has permit update or retirement application 
            // check is has permit update application
            pSet.Query = "SELECT * FROM permit_update_appl WHERE bin = :1 AND data_mode = 'QUE'";
            //pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";  // RMC 20180117 correction in billing if with pending prev year PU transaction    // RMC 20180118 added cancellation of permit update appl queued on prev year, put rem
            pSet.Query += " order by tax_year ";    // RMC 20180118 added cancellation of permit update appl queued on prev year
            pSet.AddParameter(":1", BillingForm.BIN);
            // Note: Do not add filter for tax year BillingForm.TaxYear is NULL at this point
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    /*MessageBox.Show("Cannot proceed. Record has Permit Update Application.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //MessageBox.Show("Procced to Permit Update Transaction Billing Module.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Proceed to Permit Update Transaction Billing Module.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);   // RMC 20110807 corrected spelling of proceed
                     */
                    // RMC 20180118 added cancellation of permit update appl queued on prev year, put rem

                    // RMC 20180118 added cancellation of permit update appl queued on prev year (s)
                    string sMsg = string.Empty;
                    if (pSet.GetString("tax_year") != ConfigurationAttributes.CurrentYear)
                    {
                        sMsg = "Cannot proceed.\nRecord has Permit Update Application for tax year " + pSet.GetString("tax_year") + ".";
                        sMsg += "\nProceed to Cancel Application";
                    }
                    else
                    {
                        sMsg = "Cannot proceed. Record has Permit Update Application.";
                        sMsg += "\nProceed to Permit Update Transaction Billing Module.";
                    }
                    MessageBox.Show(sMsg, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    // RMC 20180118 added cancellation of permit update appl queued on prev year (e)

                    pSet.Close();
                    return false;
                }
            }

            // check is has retirement application
            pSet.Query = "SELECT * FROM retired_bns_temp WHERE bin = :1";
            pSet.AddParameter(":1", BillingForm.BIN);
            // Note: Do not add filter for tax year BillingForm.TaxYear is NULL at this point
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("Cannot proceed. Record has Retirement Application.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //MessageBox.Show("Procced to Retirement Billing Module.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Proceed to Retirement Billing Module.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);  // RMC 20110807 corrected spelling of proceed
                    pSet.Close();
                    return false;
                }
            }
            // (e) ALJ 20110805



            bool blnIsSplBns = false;
            ///* MCR 20140902 REM SPLBNS (s)
            pSet.Query = "select * from spl_business_que where bin  = :1";
            pSet.AddParameter(":1", BillingForm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    blnIsSplBns = true;
                }
                else
                {
                    pSet.Close();
                    pSet.Query = "select * from spl_businesses where bin  = :1";
                    pSet.AddParameter(":1", BillingForm.BIN);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        { blnIsSplBns = true; }
                    }
                }
            }
            pSet.Close(); //MCR 20140902 REM SPLBNS (e)*/

            sQtr = "1";
            if(blnIsSplBns)
                pSet.Query = "SELECT * FROM spl_business_que WHERE bin = :1";
            else
            pSet.Query = "SELECT * FROM business_que WHERE bin = :1";
            pSet.AddParameter(":1", BillingForm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sTaxYear = pSet.GetString("tax_year");
                    sBnsCode = pSet.GetString("bns_code").Trim();
                    sNewBnsStat = pSet.GetString("bns_stat"); // ALJ 20130219 NEW Discovery - delinquent
                    m_sIsQtrly = LibraryClass.IsQtrlyDec(sBnsCode, m_sRevYear);
                    int.TryParse(sTaxYear, out iTaxYear);
                    int.TryParse(m_sCurrentYear, out iCurrentYear);
                    int.TryParse(m_sCurrentQtr, out iCurrentQtr); // ALJ 20110309
                    //if ((iTaxYear < iCurrentYear) && (pSet.GetDateTime("save_tm").Year != iCurrentYear)) //MCR 2015 01 27 added save_tm != CY
                    if (iTaxYear < iCurrentYear)    // RMC 20150129 verify prev mods from MJ
                    {
                        // (s) ALJ 20130219 NEW Discovery - delinquent
                        if (sNewBnsStat == "NEW")
                        {
                            BillingForm.NewDiscovery = true;
                            OracleResultSet pRecNewDisc = new OracleResultSet();
                            pRecNewDisc.Query = "SELECT * FROM taxdues WHERE bin = :1";
                            pRecNewDisc.AddParameter(":1", BillingForm.BIN);
                            if (pRecNewDisc.Execute())
                            {
                                if (pRecNewDisc.Read())
                                {
                                    /*MessageBox.Show("Warning! Has pending tax and fees dues for NEW application for tax year " + sTaxYear + ".\n Pay/settle first before renewal or proceed for re-billing.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return true;*/
                                    // RMC 20150129
                                }
                                else 
                                {
                                    MessageBox.Show("Has pending NEW application for tax year " + sTaxYear + ".", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return true;
                                }
                            }
                            pRecNewDisc.Close();
                        }
                        else
                        {
                        // (e) ALJ 20130219 NEW Discovery - delinquent
                            if (MessageBox.Show("Previous year application detected!\nVoid application and proceed to current year renewal?", "Billing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                pSet.Query = "delete from business_que where bin = :1";
                                pSet.AddParameter(":1", BillingForm.BIN);
                                pSet.ExecuteNonQuery();
                            }
                            else
                            {
                                pSet.Close();
                                return false;
                            }
                        } // ALJ 20130219 NEW Discovery - delinquent
                    }
                    else
                    {
                        pSet.Close();
                        return true;
                    }

                }

                OracleResultSet pSetBuss = new OracleResultSet();
                string sBnsStat;

                pSetBuss.Query = "SELECT * FROM businesses WHERE bin = :1 and bns_stat <> 'RET'";
                pSetBuss.AddParameter(":1", BillingForm.BIN);
                if (pSetBuss.Execute())
                {
                    if (pSetBuss.Read())
                    {
                        //if (AppSettingsManager.GetConfigValue("27") == "N") //MCR 20141017
                        //{
                            // RMC 20111212 added validation in Billing (s)
                            OracleResultSet pSetPay = new OracleResultSet();
                            pSetPay.Query = "SELECT DISTINCT(bin) FROM pay_hist WHERE bin = :1 AND data_mode <> 'UNP'";
                            pSetPay.AddParameter(":1", BillingForm.BIN);
                            if (pSetPay.Execute())
                            {
                                if (!pSetPay.Read())
                                {
                                    MessageBox.Show("Record Not Posted", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    pSetPay.Close();
                                    return false;
                                }
                            }
                            pSetPay.Close();
                            // RMC 20111212 added validation in Billing (e)
                        //}

                        sBnsCode = pSetBuss.GetString("bns_code").Trim();
                        sBnsStat = pSetBuss.GetString("bns_stat").Trim();
                        m_sIsQtrly = LibraryClass.IsQtrlyDec(sBnsCode, m_sRevYear);

                        sTaxYear = pSetBuss.GetString("tax_year");
                        int.TryParse(sTaxYear, out iTaxYear);
                        int.TryParse(m_sCurrentYear, out iCurrentYear);
                        int.TryParse(m_sCurrentQtr, out iCurrentQtr); // ALJ 20110309

                        iQtr = 4; // for RENEWAL and non-qtr dec .. default is 4 .. for NEW and with qtr dec check paym_hist... for validation of payment already exist 

                        if (sBnsStat == "NEW" && m_sIsQtrly == "Y")
                        {
                            pSet.Query = "select max(qtr_paid) as qtr from pay_hist where bin = :1 and tax_year = :2";
                            pSet.AddParameter(":1", BillingForm.BIN);
                            pSet.AddParameter(":2", sTaxYear);
                            if (pSet.Execute())
                            {
                                if (pSet.Read())
                                {
                                    sQtr = pSet.GetString("qtr");
                                    int.TryParse(sQtr, out iQtr);
                                }
                            }
                        }

                        if ((iTaxYear + 1) > iCurrentYear && iQtr == 4)
                        {

                            MessageBox.Show("Cannot bill, Payment already exists.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            pSet.Close();
                            pSetBuss.Close();
                            return false;
                        }



                        // check for taxdues - latest dues
                        OracleResultSet pSetDues = new OracleResultSet();
                        string sTmpTaxduesTY = "", sTmpTaxduesQtr = ""; // ALJ 20110321 set ""
                        int iTmpTaxduesTY = 0;
                        int iTmpTaxduesQtr = 0;
                        //pSetDues.Query = "select distinct tax_year from taxdues where bin = :1 and due_state <> 'A' order by tax_year desc";
                        pSetDues.Query = "select tax_year, qtr_to_pay from taxdues where bin = :1 and due_state <> 'A' order by tax_year desc, qtr_to_pay desc";
                        pSetDues.AddParameter(":1", BillingForm.BIN);
                        if (pSetDues.Execute())
                        {
                            if (pSetDues.Read())
                            {
                                sTmpTaxduesTY = pSetDues.GetString("tax_year");
                                sTmpTaxduesQtr = pSetDues.GetString("qtr_to_pay");
                                int.TryParse(sTmpTaxduesTY, out iTmpTaxduesTY);
                                int.TryParse(sTmpTaxduesQtr, out iTmpTaxduesQtr);
                            }
                        }
                        pSetDues.Close();

                        // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
                        // AST 20150114 (s)
                        // with delinquent year/s || // with multiple years billing created
                        if (m_strTmpBnsStat == "NEW") // AST 20150217
                        {
                            int intBilledQtr = 0;
                            intBilledQtr = AppSettingsManager.GetLastBilledQtr(BillingForm.BIN);

                            if (intBilledQtr < 4 && m_sIsQtrly == "Y") // ASt 20150226 Added && m_sIsQtrly == "Y"
                                return true; // proceed to billing no need to insert data in business_que 
                        }
                        // AST 20150114 remove this block
                        //if ((iTaxYear + 1) < iCurrentYear) // with delinquent year/s || // with multiple years billing created
                        //{

                        //    if ((iTmpTaxduesTY + 1) < iCurrentYear)
                        //    {
                        //        pSetBuss.Close();
                        //        return true; // proceed to billing no need to insert data in business_que 
                        //    }
                        //}
                        // AST 20150114 (e)
                        // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (e)

                        if (iTmpTaxduesTY == iTaxYear) // due year = business tax year .. meaning there's no multiple years billing created.
                        {
                            MessageBox.Show("Has Pending Taxdues!", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        // ALJ 20110321 (s) QTR DEC
                        if (sBnsStat == "NEW" && m_sIsQtrly == "Y")
                        {
                            if (iTaxYear == iCurrentYear)
                            {
                                if (iQtr == iCurrentQtr)
                                {
                                    MessageBox.Show("Cannot bill. Payment already exists for this quarter.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return false;
                                }
                                return true;
                            }
                            if (iTaxYear < iCurrentYear && iQtr < 4 && iTmpTaxduesQtr < 4)
                            {
                                MessageBox.Show("Previous year/s New-quarterly delinquency detected!", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return true;
                            }
                        }
                        // ALJ 20110321 e) QTR DEC

                        if (ConfigurationAttributes.AutoApplication == "Y")
                        {
                            /*if (MessageBox.Show("Apply for renewal?", "Billing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                if (sTmpTaxduesTY != "")
                                    sTaxYear = sTmpTaxduesTY;
                                if (sTmpTaxduesQtr != "")
                                    sQtr = sTmpTaxduesQtr;
                                m_sDueState = "R"; // ALJ 20110321
                                AutoApplication(sTaxYear, sQtr, m_sCurrentYear, m_sDueState);
                            }
                            else
                            {
                                pSetBuss.Close();
                                return false;
                            }*/
                            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez

                            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
                            // AST 20150113 (s)
                            if (m_sIsQtrly == "Y") // AST 20150226 Added this condition
                            {
                                // AST 20150114 Check if BIN can renew
                                if (!AppSettingsManager.ValidateTaxYearAndQtrPaid(BillingForm.BIN, sTaxYear))
                                    return false;
                            }

                            if (sTmpTaxduesTY != "")
                                sTaxYear = sTmpTaxduesTY;
                            if (sTmpTaxduesQtr != "")
                                sQtr = sTmpTaxduesQtr;
                            m_sDueState = "R"; // ALJ 20110321

                            int intTaxYearToBill = 0;
                            string strTaxYearToBill = string.Empty; // AST 20150226
                            g_RemoveBusinessQue = true; // AST 20150114


                            if (!String.IsNullOrEmpty(m_strTmpTaxYear.Trim()))
                            {
                                int.TryParse(m_strTmpTaxYear, out intTaxYearToBill);
                                intTaxYearToBill++;
                                strTaxYearToBill = intTaxYearToBill.ToString();
                            }
                            else
                            {
                                // AST 20150220 Added Year Correction in AutoAppl'n (s)
                                int.TryParse(sTaxYear, out intTaxYearToBill);
                                intTaxYearToBill++;

                                if (intTaxYearToBill > int.Parse(m_sCurrentYear))
                                {
                                    strTaxYearToBill = m_sCurrentYear;
                                }
                                else if (intTaxYearToBill <= int.Parse(m_sCurrentYear))
                                {
                                    strTaxYearToBill = intTaxYearToBill.ToString();
                                }

                                // AST 20150220 Added Year Correction in AutoAppl'n (e)
                            }
                            // AST 20150113 (e)

                            if (MessageBox.Show(string.Format("Apply for {0} renewal?", strTaxYearToBill), "Billing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                //AutoApplication(sTaxYear, sQtr, m_sCurrentYear, m_sDueState);
                                AutoApplication(sTaxYear, sQtr, strTaxYearToBill, m_sDueState);
                            }
                            else
                            {
                                pSetBuss.Close();
                                BillingForm.SearchButton.Text = "&Clear"; // AST 20150114
                                BillingForm.SearchButton.PerformClick(); // AST 20150114
                                return false;
                            }
                            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (e)

                            
                        }
                        else
                        {
                            MessageBox.Show("Current year application required!\nProceed to application renewal.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                }
                pSetBuss.Close();
            }
            pSet.Close();
            return true;
        }

        private void AutoApplication(string p_sTaxYear, string p_sQtr, string p_sCurrentYear, string p_sDueState)
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "auto_application";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBin", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sTaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sQtr;
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sCurrentYear;
            plsqlCmd.AddParameter("p_sCurrYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = AppSettingsManager.SystemUser.UserCode;
            plsqlCmd.AddParameter("p_sUser", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sRevYear;
            plsqlCmd.AddParameter("p_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sDueState;
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
            }
            plsqlCmd.Close();


        }

        protected void PopulateGrossInfo(string p_sBnsStat, string p_sDueState, string p_sBaseYear, string p_sBaseQtr, string p_sPopYear, string p_sPopQtr, string p_sModule)
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "pop_billgross_info";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBin", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sBnsStat;
            plsqlCmd.AddParameter("p_sBnsStat", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sDueState;
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sBaseYear;
            plsqlCmd.AddParameter("p_sBaseYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sBaseQtr;
            plsqlCmd.AddParameter("p_sBaseQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sPopYear;
            plsqlCmd.AddParameter("p_sPopYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sPopQtr;
            plsqlCmd.AddParameter("p_sPopQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sModule;
            plsqlCmd.AddParameter("p_sModule", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
            }
            plsqlCmd.Close();

        }
        protected void SplPopulateGrossInfo(string p_sBnsStat, string p_sDueState, string p_sBaseYear, string p_sBaseQtr, string p_sPopYear, string p_sPopQtr, string p_sModule)
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "spl_pop_billgross_info";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBin", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sBnsStat;
            plsqlCmd.AddParameter("p_sBnsStat", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sDueState;
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sBaseYear;
            plsqlCmd.AddParameter("p_sBaseYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sBaseQtr;
            plsqlCmd.AddParameter("p_sBaseQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sPopYear;
            plsqlCmd.AddParameter("p_sPopYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sPopQtr;
            plsqlCmd.AddParameter("p_sPopQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sModule;
            plsqlCmd.AddParameter("p_sModule", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
            }
            plsqlCmd.Close();

        }
        protected virtual void GetMinMaxDueYear(string p_sBIN, out string o_sMinDueYear, out string o_sMaxDueYear)
        {
            o_sMinDueYear = string.Empty;
            o_sMaxDueYear = string.Empty;
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            pSet.Query = "SELECT MIN(tax_year) AS min_year, MAX(tax_year) AS max_year FROM taxdues WHERE bin = :1 and due_state <> :2"; // ALJ 20130115 add due_state <> 'A' to avoid billing of rev adj in regular billing
            // RMC 20150112 mods in retirement billing (s)
            if (BillingForm.Status == "RET")
                pSet.Query += " and due_state = 'X'";
            // RMC 20150112 mods in retirement billing (e)
            pSet.AddParameter(":1", p_sBIN);
            pSet.AddParameter(":2", "A"); // ALJ 20130115 add due_state <> 'A' to avoid billing of rev adj in regular billing
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    o_sMinDueYear = pSet.GetString("min_year");
                    o_sMaxDueYear = pSet.GetString("max_year");

                    // RMC 20150112 mods in retirement billing (s)
                    // auto-increment of tax year after saving of billing
                    if (o_sMinDueYear.Trim() != "")
                    {
                        //MCR 20150112 (s)
                        int iTmpMinDueYear = 0;
                        int iTmpMaxDueYear = 0;
                        iTmpMinDueYear = int.Parse(o_sMinDueYear);
                        iTmpMaxDueYear = int.Parse(o_sMaxDueYear);
                        o_sMinDueYear = string.Format("{0:0000}", int.Parse(o_sMinDueYear) + 1);
                        o_sMaxDueYear = string.Format("{0:0000}", int.Parse(o_sMaxDueYear) + 1);

                        if (iTmpMaxDueYear >= AppSettingsManager.GetSystemDate().Year
                            && iTmpMinDueYear >= AppSettingsManager.GetSystemDate().Year)
                        {
                            o_sMinDueYear = iTmpMinDueYear.ToString();
                            o_sMaxDueYear = iTmpMaxDueYear.ToString();
                        }
                        //MCR 20150112 (e)

                        // RMC 20160222 correction creating billing for advance tax year (s)
                        if (Convert.ToInt32(o_sMinDueYear) > AppSettingsManager.GetSystemDate().Year)
                            o_sMinDueYear = iTmpMinDueYear.ToString();

                        if (Convert.ToInt32(o_sMaxDueYear) > AppSettingsManager.GetSystemDate().Year)
                            o_sMaxDueYear = iTmpMaxDueYear.ToString();
                        // RMC 20160222 correction creating billing for advance tax year (e)
                                              

                        int iMinDue = 0;
                        int iMaxDue = 0;
                        int.TryParse(o_sMinDueYear, out iMinDue);
                        int.TryParse(o_sMaxDueYear, out iMaxDue);

                        if (iMaxDue < iMinDue)
                            o_sMaxDueYear = o_sMinDueYear;
                    }
                    // RMC 20150112 mods in retirement billing (e)
                }
            }
            // (s) ALJ 20110907 get the next year as base year if has remaining qtr dues
            //pSet.Query = "select distinct * from pay_hist WHERE bin = :1 AND tax_year =  :2 AND bns_stat <> 'NEW'";

            //if (m_sMainBnsStat == "NEW" || m_sMainBnsStat == "RET")    // RMC 20140110 corrected cancel billing of new application  // RMC 20140708 added retirement billing
            //if (m_sMainBnsStat == "NEW")    // RMC 20150112 mods in retirement billing
            if (m_sMainBnsStat == "NEW" && BillingForm.NewDiscovery)    // RMC 20150921 corrections in billing if user has no access in re-bill
            {
                // RMC 20150129 (s)
                if (BillingForm.NewDiscovery)
                {
                    pSet.Query = "select tax_year from business_que where bin = '" + p_sBIN + "'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            o_sMinDueYear = pSet.GetString(0);
                           
                        }
                    }
                    pSet.Close();
                }
                // RMC 20150129 (e)
            }
            else
            {
                pSet.Query = "select distinct * from pay_hist WHERE bin = :1 AND tax_year = :2 and bns_stat <> 'RET'";    // RMC 20120103 modified billing for delinquent years
                pSet.AddParameter(":1", p_sBIN);
                pSet.AddParameter(":2", o_sMinDueYear);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        o_sMinDueYear = string.Format("{0:0000}", int.Parse(o_sMinDueYear) + 1);
                    }
                    else
                    {
                        pSet.Close();

                        // RMC 20120103 modified billing for delinquent years (s)
                        pSet.Query = "select max(tax_year) from pay_hist where bin = :1 and data_mode <> 'UNP' and bns_stat <> 'RET'"; // GDE add data mode //MCR 20150129 added bns_stat
                        pSet.AddParameter(":1", p_sBIN);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                // RMC 20160222 correction creating billing for advance tax year - merged from Angono (s)
                                // RMC 20160127 correction in billing with auto-application (s)
                                int iTmpMinDueYear = 0;
                                int.TryParse(o_sMinDueYear, out iTmpMinDueYear);
                                int iTmpMaxTY = 0;
                                int.TryParse(pSet.GetString(0), out iTmpMaxTY);

                                if (iTmpMinDueYear < iTmpMaxTY)
                                // RMC 20160127 correction in billing with auto-application (e)
                                // RMC 20160222 correction creating billing for advance tax year - merged from Angono (e)
                                {
                                    o_sMinDueYear = pSet.GetString(0);

                                    if (BillingForm.Status == "RET")
                                    {
                                        if (o_sMinDueYear == AppSettingsManager.GetSystemDate().Year.ToString())
                                            o_sMinDueYear = string.Format("{0:0000}", Convert.ToInt32(pSet.GetString(0))); //MCR 20150216
                                        else
                                            o_sMinDueYear = string.Format("{0:0000}", Convert.ToInt32(pSet.GetString(0)) + 1);
                                    }
                                    else if (o_sMinDueYear != "")
                                        o_sMinDueYear = string.Format("{0:0000}", Convert.ToInt32(pSet.GetString(0)) + 1);
                                }
                            }
                        }
                        // RMC 20120103 modified billing for delinquent years (e)
                    }
                }
                pSet.Close();
            }
            // RMC 20150921 corrections in billing if user has no access in re-bill (s)
                
            if(o_sMinDueYear!=""){ //GMC 20150930 Correction. Input string was not in a correct format
                if (Convert.ToInt32(o_sMinDueYear) > Convert.ToInt32(ConfigurationAttributes.CurrentYear))
                    o_sMinDueYear = ConfigurationAttributes.CurrentYear;
            }

            if (o_sMaxDueYear == "")
                o_sMaxDueYear = o_sMinDueYear;
            // RMC 20150921 corrections in billing if user has no access in re-bill (e)
        }
        protected void GetMinMaxDueQtr(string p_sBIN, string p_sBaseDueYear, out string o_sMinDueQtr, out string o_sMaxDueQtr)
        {
            o_sMinDueQtr = string.Empty;
            o_sMaxDueQtr = string.Empty;
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "SELECT MIN(qtr_to_pay) AS min_qtr, MAX(qtr_to_pay) AS max_qtr FROM taxdues WHERE bin = :1 and tax_year = :2";
            pSet.AddParameter(":1", p_sBIN);
            pSet.AddParameter(":2", p_sBaseDueYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    o_sMinDueQtr = pSet.GetString("min_qtr");
                    o_sMaxDueQtr = pSet.GetString("max_qtr");
                }
            }
            pSet.Close();
        }
        private string GetMinDueYear()
        {
            OracleResultSet pset = new OracleResultSet();
            OracleResultSet psetBns = new OracleResultSet();
            string sMinDueYear = "", sBussYear = "";
            int iMinDueYear;

            psetBns.Query = "select tax_year from businesses where bin = :1";
            psetBns.AddParameter(":1", BillingForm.BIN);

            if (psetBns.Execute())
            {
                if (psetBns.Read())
                {
                    sBussYear = psetBns.GetString("tax_year");
                }
            }
            psetBns.Close();

            pset.Query = "select tax_year from taxdues where bin = :1 order by tax_year";
            pset.AddParameter(":1", BillingForm.BIN);
            if (pset.Execute())
            {
                if (pset.Read())
                {
                    sMinDueYear = pset.GetString("tax_year");
                    if (sBussYear == sMinDueYear)
                    {
                        //if (BillingForm.Status == "NEW" && m_sIsQtrly == "Y")
                        if ((BillingForm.Status == "NEW" && m_sIsQtrly == "Y") || BillingForm.Status == "RET")  // RMC 20140708 added retirement billing  
                        {
                            // null -- nothing to do 
                        }
                        else
                        {
                            int.TryParse(sMinDueYear, out iMinDueYear);
                            sMinDueYear = string.Format("{0:0000}", iMinDueYear + 1);
                        }
                    }
                }
                else
                {
                    //if (BillingForm.Status == "NEW" && m_sIsQtrly == "Y")
                    if ((BillingForm.Status == "NEW" && m_sIsQtrly == "Y") || BillingForm.Status == "RET")  // RMC 20140708 added retirement billing  
                    {
                        // null -- nothing to do 
                    }
                    else
                    {
                        if (sBussYear != "")
                        {
                            int.TryParse(sBussYear, out iMinDueYear);
                            sMinDueYear = string.Format("{0:0000}", iMinDueYear + 1);
                        }
                    }
                }

            }
            pset.Close();
            return sMinDueYear;
        }

        private string GetMinDueQtr(string p_sMinDueYear)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sMinDueQtr = "";
            pSet.Query = "select max(qtr_to_pay) as qtr from taxdues where bin = :1 and tax_year = :2";
            pSet.AddParameter(":1", BillingForm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sMinDueQtr = pSet.GetString("qtr");
                }
            }
            return sMinDueQtr;

        }

        public bool HasDues(string p_sBin, string p_sBnsStat, string p_sBnsCode, string p_sTaxYear, string p_sQtr)
        {
            OracleResultSet pSet = new OracleResultSet();
            bool bHasDues = false;
            if (p_sBnsStat != "NEW")
                pSet.Query = "SELECT  * FROM taxdues WHERE bin = :1 AND bns_code_main = :2 AND tax_year = :3";
            else
                pSet.Query = "SELECT  * FROM taxdues WHERE bin = :1 AND bns_code_main = :2 AND tax_year = :3 AND qtr_to_pay = :4";
            pSet.AddParameter(":1", p_sBin);
            pSet.AddParameter(":2", p_sBnsCode);
            pSet.AddParameter(":3", p_sTaxYear);
            if (p_sBnsStat == "NEW")
                pSet.AddParameter(":4", p_sQtr);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    bHasDues = true;
                }
                else
                    bHasDues = false;
            }
            pSet.Close();
            return bHasDues;

        }


        protected void PopulateBnsType()
        {
            OracleResultSet pSet = new OracleResultSet();
            DataTable dataTable = new DataTable("bns_table");
            string sBnsCode;
            dataTable.Columns.Add("bns_code", typeof(String));
            dataTable.Columns.Add("bns_desc", typeof(String));

            //BillingForm.cmbBnsType.Items.Clear();
            //BillingForm.BusinessTypes.Items.Clear();

            /* dataTable.Rows.Add(new String[] { m_sMainBnsCode, LibraryClass.GetBnsDesc("B", m_sMainBnsCode, m_sRevYear) });

             bool bIsEntered = false;
             //pSet.Query = "SELECT bns_code_main FROM addl_bns WHERE bin = :1 AND tax_year = :2 and qtr = :3";

             pSet.Query = "SELECT distinct bns_code_main FROM addl_bns WHERE bin = :1 AND tax_year >= :2 and qtr = :3";   // RMC 20150126 MCR 20150210 distinct
             pSet.AddParameter(":1", BillingForm.BIN);
             pSet.AddParameter(":2", BillingForm.TaxYear);
             pSet.AddParameter(":3", BillingForm.Quarter);

             if (pSet.Execute())
             {
                 while (pSet.Read())
                 {
                     sBnsCode = pSet.GetString("bns_code_main").Trim();
                     dataTable.Rows.Add(new String[] { sBnsCode, LibraryClass.GetBnsDesc("B", sBnsCode, m_sRevYear) });
                     bIsEntered = true;
                 }
             }
             pSet.Close();


             if (bIsEntered == false)
             {
                 // RMC 20150107 (S)
                 pSet.Query = "SELECT bns_code_main FROM addl_bns_que WHERE bin = :1 AND tax_year = :2 and qtr = :3";
                 pSet.AddParameter(":1", BillingForm.BIN);
                 pSet.AddParameter(":2", BillingForm.TaxYear);
                 pSet.AddParameter(":3", BillingForm.Quarter);

                 if (pSet.Execute())
                 {
                     while (pSet.Read())
                     {
                         sBnsCode = pSet.GetString("bns_code_main").Trim();
                         dataTable.Rows.Add(new String[] { sBnsCode, LibraryClass.GetBnsDesc("B", sBnsCode, m_sRevYear) });
                         bIsEntered = true;
                     }
                 }
             }
             pSet.Close();
             // RMC 20150107 (E)

             if (bIsEntered == false)
             {
                 // MCR 20150121 (S)
                 //pSet.Query = "SELECT bns_code_main FROM retired_bns_temp WHERE bin = :1 AND to_char(apprvd_date, 'YYYY') = :2 AND main = :3";
                 pSet.Query = "SELECT bns_code_main FROM retired_bns_temp WHERE bin = :1 AND tax_year = :2 AND main = :3"; //REM MCR 20150127
                 pSet.AddParameter(":1", BillingForm.BIN);
                 pSet.AddParameter(":2", BillingForm.TaxYear);
                 pSet.AddParameter(":3", "N");
                 if (pSet.Execute())
                 {
                     while (pSet.Read())
                     {
                         sBnsCode = pSet.GetString("bns_code_main").Trim();
                         dataTable.Rows.Add(new String[] { sBnsCode, LibraryClass.GetBnsDesc("B", sBnsCode, m_sRevYear) });
                     }
                 }
                 pSet.Close();
                 // MCR 20150121 (E)
             }*/
            // RMC 20150325 modified billing for permit update application, put rem

            // RMC 20150325 modified billing for permit update application (s)
            if (m_bPermitUpdateFlag == true && m_blnNewAddl && m_sDueState == "P")
            {
                // no renewal application included
                pSet.Query = "select distinct new_bns_code from permit_update_appl where ";
                pSet.Query += " bin = '" + BillingForm.BIN + "' and tax_year = '" + BillingForm.TaxYear + "'";
                pSet.Query += " and appl_type = 'ADDL' and data_mode = 'QUE'";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sBnsCode = pSet.GetString(0).Trim();
                        dataTable.Rows.Add(new String[] { sBnsCode, LibraryClass.GetBnsDesc("B", sBnsCode, m_sRevYear) });
                    }
                }
            }
            else
            {
                // RMC 20151109 Corrections in billing of retirement application (s)
                if (m_sDueState == "X")
                {
                    pSet.Query += "SELECT bns_code_main FROM retired_bns_temp WHERE bin = '" + BillingForm.BIN + "'";
                    //pSet.Query += " AND tax_year = '" + BillingForm.TaxYear + "'";    // RMC 20170109 corrected error in retirement billing, put rem
                }// RMC 20151109 Corrections in billing of retirement application (e)
                else
                {
                    dataTable.Rows.Add(new String[] { m_sMainBnsCode, LibraryClass.GetBnsDesc("B", m_sMainBnsCode, m_sRevYear) });

                    pSet.Query = "SELECT distinct bns_code_main FROM addl_bns WHERE bin = '" + BillingForm.BIN + "' ";
                    pSet.Query += "AND tax_year >= '" + BillingForm.TaxYear + "' and qtr = '" + BillingForm.Quarter + "'";
                    pSet.Query += " and bns_stat <> 'RET' ";    // RMC 20160113 corrections in Billing
                    pSet.Query += " UNION ";
                    pSet.Query += "SELECT bns_code_main FROM addl_bns_que WHERE bin = '" + BillingForm.BIN + "' ";
                    //pSet.Query += "AND tax_year = '" + BillingForm.TaxYear + "' and qtr = '" + BillingForm.Quarter + "'";
                    //pSet.Query += "AND tax_year = '" + BillingForm.TaxYear + "' and qtr >= '" + BillingForm.Quarter + "'";   // RMC 20160517 correction in permit-update billing with renewal application
                    pSet.Query += "AND tax_year >= '" + BillingForm.TaxYear + "' and qtr >= '" + BillingForm.Quarter + "'";   //JARS 20170808
                    pSet.Query += " and bns_stat <> 'RET' ";    // RMC 20160113 corrections in Billing
                    pSet.Query += " UNION ";
                    pSet.Query += "SELECT bns_code_main FROM retired_bns_temp WHERE bin = '" + BillingForm.BIN + "'";
                    pSet.Query += " AND tax_year = '" + BillingForm.TaxYear + "' AND main = 'N'";
                }
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sBnsCode = pSet.GetString(0).Trim();
                        dataTable.Rows.Add(new String[] { sBnsCode, LibraryClass.GetBnsDesc("B", sBnsCode, m_sRevYear) });
                    }
                }
                pSet.Close();
            }
            // RMC 20150325 modified billing for permit update application (e)

            try
            {
                BillingForm.BusinessTypes.DataSource = dataTable;
                BillingForm.BusinessTypes.DisplayMember = "bns_desc";
                BillingForm.BusinessTypes.ValueMember = "bns_code";
                BillingForm.BusinessTypes.SelectedIndex = 0;
                // RMC 20150325 modified billing for permit update application (s)
                if (m_bPermitUpdateFlag == true && m_blnNewAddl && m_sDueState == "P")
                    BillingForm.MainBusiness = false;
                else// RMC 20150325 modified billing for permit update application (e)
                    BillingForm.MainBusiness = true;
                BillingForm.BusinessCode = BillingForm.BusinessTypes.SelectedValue.ToString();
            }
            catch {
                // RMC 20170109 corrected error in retirement billing (s)
                if (m_sDueState == "X")
                {
                    MessageBox.Show("Please check retirement date","Billing",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                // RMC 20170109 corrected error in retirement billing (e)
            }


        }


        protected void LoadAddlInfo()
        {
            //AddlInfo.AddlInfo AddlInfoClass = new AddlInfo.AddlInfo();    // RMC 20110414 put rem
            AddlInfoClass = new AddlInfo();   // RMC 20110414
            AddlInfoClass.BIN = BillingForm.BIN;
            AddlInfoClass.TaxYear = BillingForm.TaxYear;
            if (BillingForm.MainBusiness)
                AddlInfoClass.BusinessCode = m_sMainBnsCode;
            else
                AddlInfoClass.BusinessCode = BillingForm.BusinessCode;


            BillingForm.AdditionalInformation.Columns.Clear();
            BillingForm.AdditionalInformation.DataSource = AddlInfoClass.GetAddlInfo();
            BillingForm.AdditionalInformation.Columns[0].Width = 50;
            BillingForm.AdditionalInformation.Columns[1].Width = 250;
            BillingForm.AdditionalInformation.Columns[2].Width = 35;
            BillingForm.AdditionalInformation.Columns[3].Width = 50;
            BillingForm.AdditionalInformation.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            BillingForm.AdditionalInformation.Columns[0].Visible = false;
            BillingForm.AdditionalInformation.Columns[1].ReadOnly = true;
            BillingForm.AdditionalInformation.Columns[2].ReadOnly = true;


        }

        protected void LoadGrossCapital()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            double dGrCap;
            pSet.Query = "SELECT * FROM bill_gross_info WHERE bin = :1 AND tax_year = :2 AND qtr = :3 AND bns_code = :4";
            pSet.AddParameter(":1", BillingForm.BIN);
            pSet.AddParameter(":2", BillingForm.TaxYear);
            pSet.AddParameter(":3", BillingForm.Quarter);
            pSet.AddParameter(":4", BillingForm.BusinessCode);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    BillingForm.BusinessCode = pSet.GetString("bns_code");
                    // RMC 20140708 added retirement billing (s)
                    if (BillingForm.Status == "RET")
                    {
                        //BillingForm.Status = ;    //value already set at ReturnValues
                        //m_sDueState = ;           //value already set at ReturnValues
                        // RMC 20140708 added retirement billing (e)
                    }
                    else
                    {
                        // RMC 20150129 (s)
                        if (BillingForm.NewDiscovery && pSet.GetString("bns_stat") == "NEW")
                        {
                            if (BillingForm.Status == "REN")
                            {
                                // do not change stat retrieved from DiscoveryDelinquentStat()
                                BillingForm.Quarter = "1";
                                m_sDueState = "R";
                            }
                            else
                            {
                                BillingForm.Status = pSet.GetString("bns_stat");
                                m_sDueState = pSet.GetString("due_state");
                            }
                        }// RMC 20150129 (e)
                        else
                        {
                            BillingForm.Status = pSet.GetString("bns_stat");
                            if (m_bPermitUpdateFlag && !m_bIsRenPUT)    // RMC 20140725 Added Permit update billing
                            {
                                //due state value already retrieved at PertmiUpdate.ReturnValues()
                            }
                            else
                                m_sDueState = pSet.GetString("due_state");    // RMC 20140708 added retirement billing
                        }
                    }
                    if (BillingForm.Status == "RET") //AFM 20200527 for retiring business with 1st quarter payment for current year. Will get gross from retired_bns_temp
                    {
                        pSet2.Query = "SELECT * FROM retired_bns_temp WHERE bin = :1 AND tax_year = :2 AND bns_code_main = :3 AND bns_stat = 'RET'";
                        pSet2.AddParameter(":1", BillingForm.BIN);
                        pSet2.AddParameter(":2", BillingForm.TaxYear);
                        pSet2.AddParameter(":3", BillingForm.BusinessCode);

                        if(pSet2.Execute())
                            if(pSet2.Read())
                            {
                                //BillingForm.Capital = string.Format("{0:#,##0.00}", pSet2.GetDouble("capital"));
                                //m_sCapitalParam = BillingForm.Capital;

                                BillingForm.Gross = string.Format("{0:#,##0.00}", pSet2.GetDouble("gross"));
                                m_sGrossParam = BillingForm.Gross;

                                BillingForm.PreGross = "0";
                                double.TryParse(BillingForm.PreGross, out dGrCap);

                                BillingForm.AdjGross = "0";
                                double.TryParse(BillingForm.AdjGross, out dGrCap);

                                BillingForm.VATGross = "0";
                            }
                        pSet2.Close();
                    }
                    else
                    {
                        BillingForm.Capital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
                        //////////
                        m_sCapitalParam = BillingForm.Capital; // // ALJ 20120309 Rev exam for new -- assign value for m_sCapitalParam - basis for billing 
                        //////////
                        BillingForm.Gross = string.Format("{0:#,##0.00}", pSet.GetDouble("gross"));
                        //////////
                        m_sGrossParam = BillingForm.Gross; // assign value for m_sGrossParam - basis for billing 
                        //////////
                        BillingForm.PreGross = string.Format("{0:#,##0.00}", pSet.GetDouble("pre_gross"));
                        double.TryParse(BillingForm.PreGross, out dGrCap);
                        //////////
                        if (dGrCap > 0) // assign value for m_sGrossParam pre gross will prevail - basis for billing 
                            m_sGrossParam = BillingForm.PreGross;
                        //////////
                        BillingForm.AdjGross = string.Format("{0:#,##0.00}", pSet.GetDouble("adj_gross"));
                        double.TryParse(BillingForm.AdjGross, out dGrCap);
                        //////////
                        if (dGrCap > 0) // assign value for m_sGrossParam Adj Gross will prevail - basis for billing 
                        // (s) ALJ 20120309 Rev exam for new
                        {
                            if (BillingForm.Status == "NEW")
                                m_sCapitalParam = BillingForm.AdjGross;
                            else
                                // (e) ALJ 20120309 Rev exam for new
                                m_sGrossParam = BillingForm.AdjGross;
                        } // ALJ 20120309 Rev exam for new
                        //////////
                        BillingForm.VATGross = string.Format("{0:#,##0.00}", pSet.GetDouble("vat_gross"));


                        //m_sDueState = pSet.GetString("due_state");    // RMC 20140708 added retirement billing, put rem
                    }
                }
                else
                {
                    // special code for qtr dec delinquent assume that has current year renewal and attempting to edit prev years New
                    pSet.Query = "SELECT * FROM bill_gross_info WHERE bin = :1 AND tax_year = :2 AND qtr = :3 AND bns_code = :4 and bns_stat = :5";
                    pSet.AddParameter(":1", BillingForm.BIN);
                    pSet.AddParameter(":2", BillingForm.TaxYear);
                    pSet.AddParameter(":3", "4");
                    pSet.AddParameter(":4", BillingForm.BusinessCode);
                    pSet.AddParameter(":5", "NEW");
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            BillingForm.BusinessCode = pSet.GetString("bns_code");
                            // RMC 20140708 added retirement billing (s)
                            if (BillingForm.Status == "RET")
                            {
                                //BillingForm.Status = ;    //value already set at ReturnValues
                                //m_sDueState = ;           //value already set at ReturnValues
                                // RMC 20140708 added retirement billing (e)
                            }
                            else
                            {
                                BillingForm.Status = pSet.GetString("bns_stat");
                                m_sDueState = pSet.GetString("due_state");
                            }
                            BillingForm.Capital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
                            m_sCapitalParam = BillingForm.Capital; // ALJ 20120309 Rev exam for new - assign value for m_sCapitalParam - basis for billing 
                            BillingForm.Gross = string.Format("{0:#,##0.00}", pSet.GetDouble("gross"));
                            //////////
                            m_sGrossParam = BillingForm.Gross; // assign value for m_sGrossParam - basis for billing 
                            //////////
                            BillingForm.PreGross = string.Format("{0:#,##0.00}", pSet.GetDouble("pre_gross"));
                            double.TryParse(BillingForm.PreGross, out dGrCap);
                            //////////
                            if (dGrCap > 0) // assign value for m_sGrossParam pre gross will prevail - basis for billing 
                                m_sGrossParam = BillingForm.PreGross;
                            //////////
                            BillingForm.AdjGross = string.Format("{0:#,##0.00}", pSet.GetDouble("adj_gross"));
                            double.TryParse(BillingForm.AdjGross, out dGrCap);
                            //////////
                            if (dGrCap > 0) // assign value for m_sGrossParam Adj Gross will prevail - basis for billing 
                            // (s) ALJ 20120309 Rev exam for new
                            {
                                if (BillingForm.Status == "NEW")
                                    m_sCapitalParam = BillingForm.AdjGross;
                                else
                                    // (e) ALJ 20120309 Rev exam for new
                                    m_sGrossParam = BillingForm.AdjGross;
                            } // ALJ 20120309 Rev exam for new
                            //////////
                            BillingForm.VATGross = string.Format("{0:#,##0.00}", pSet.GetDouble("vat_gross"));

                            //m_sDueState = pSet.GetString("due_state");  // RMC 20140708 added retirement billing, put rem

                            BillingForm.Quarter = "4";
                            BillingForm.QuarterTextBox.ReadOnly = false;
                        }
                        else
                        {

                            //
                            pSet.Query = "SELECT * FROM bill_gross_info WHERE bin = :1 AND tax_year = :2 AND qtr = :3 AND bns_code = :4 and bns_stat = :5";
                            pSet.AddParameter(":1", BillingForm.BIN);
                            pSet.AddParameter(":2", BillingForm.TaxYear);
                            pSet.AddParameter(":3", "1");
                            pSet.AddParameter(":4", BillingForm.BusinessCode);
                            pSet.AddParameter(":5", "REN");
                            if (pSet.Execute())
                            {
                                if (pSet.Read())
                                {
                                    BillingForm.BusinessCode = pSet.GetString("bns_code");
                                    // RMC 20140708 added retirement billing (s)
                                    if (BillingForm.Status == "RET")
                                    {
                                        //BillingForm.Status = ;    //value already set at ReturnValues
                                        //m_sDueState = ;           //value already set at ReturnValues
                                        // RMC 20140708 added retirement billing (e)
                                    }
                                    else
                                    {
                                        BillingForm.Status = pSet.GetString("bns_stat");
                                        m_sDueState = pSet.GetString("due_state");
                                    }
                                    BillingForm.Capital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
                                    m_sCapitalParam = BillingForm.Capital; // ALJ 20120309 Rev exam for new - assign value for m_sCapitalParam - basis for billing 
                                    BillingForm.Gross = string.Format("{0:#,##0.00}", pSet.GetDouble("gross"));
                                    //////////
                                    m_sGrossParam = BillingForm.Gross; // assign value for m_sGrossParam - basis for billing 
                                    //////////
                                    BillingForm.PreGross = string.Format("{0:#,##0.00}", pSet.GetDouble("pre_gross"));
                                    double.TryParse(BillingForm.PreGross, out dGrCap);
                                    //////////
                                    if (dGrCap > 0) // assign value for m_sGrossParam pre gross will prevail - basis for billing 
                                        m_sGrossParam = BillingForm.PreGross;
                                    //////////
                                    BillingForm.AdjGross = string.Format("{0:#,##0.00}", pSet.GetDouble("adj_gross"));
                                    double.TryParse(BillingForm.AdjGross, out dGrCap);
                                    //////////
                                    if (dGrCap > 0) // assign value for m_sGrossParam Adj Gross will prevail - basis for billing 
                                    // (s) ALJ 20120309 Rev exam for new
                                    {
                                        if (BillingForm.Status == "NEW")
                                            m_sCapitalParam = BillingForm.AdjGross;
                                        else
                                            // (e) ALJ 20120309 Rev exam for new
                                            m_sGrossParam = BillingForm.AdjGross;
                                    } // ALJ 20120309 Rev exam for new
                                    //////////
                                    BillingForm.VATGross = string.Format("{0:#,##0.00}", pSet.GetDouble("vat_gross"));

                                    //m_sDueState = pSet.GetString("due_state");  // RMC 20140708 added retirement billing, put rem

                                    BillingForm.Quarter = "1";
                                    BillingForm.QuarterTextBox.ReadOnly = true;
                                }
                                // RMC 20160517 correction in permit-update billing with renewal application (s)
                                else
                                {
                                    // this is redundant to PermitUpdate.REturnValues -- fix this soon
                                    if (m_bPermitUpdateFlag && m_blnNewAddl)
                                    {
                                        pSet.Query = "select * from addl_bns_que where bin = :1 AND tax_year = :2 AND bns_code_main = :3";
                                        pSet.AddParameter(":1", BillingForm.BIN);
                                        pSet.AddParameter(":2", BillingForm.TaxYear);
                                        pSet.AddParameter(":3", BillingForm.BusinessCode);
                                        if (pSet.Execute())
                                        {
                                            if (pSet.Read())
                                            {
                                                BillingForm.Quarter = pSet.GetString("qtr");
                                                BillingForm.Status = pSet.GetString("bns_stat");
                                                BillingForm.Capital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
                                                BillingForm.Gross = string.Format("{0:#,##0.00}", 0);
                                                m_sCapitalParam = BillingForm.Capital;
                                                m_sGrossParam = BillingForm.Gross;

                                                BillingForm.Quarter = pSet.GetString("qtr");
                                                BillingForm.QuarterTextBox.ReadOnly = false;
                                                m_sDueState = "N";

                                                UpdateBillGrossInfo();
                                            }
                                        }
                                    }

                                    
                                }
                               /* else
                                {
                                    pSet.Query = "SELECT * FROM bill_gross_info WHERE bin = :1 AND tax_year = :2 AND bns_code = :3 order by qtr";
                                    pSet.AddParameter(":1", BillingForm.BIN);
                                    pSet.AddParameter(":2", BillingForm.TaxYear);
                                    pSet.AddParameter(":3", BillingForm.BusinessCode);
                                    if (pSet.Execute())
                                    {
                                        if (pSet.Read())
                                        {
                                            BillingForm.BusinessCode = pSet.GetString("bns_code");
                                            BillingForm.Status = pSet.GetString("bns_stat");
                                            m_sDueState = pSet.GetString("due_state");
                                            if (BillingForm.Status == "NEW")
                                            {
                                                BillingForm.Capital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
                                                BillingForm.Gross = string.Format("{0:#,##0.00}", 0);
                                            }
                                            else
                                            {
                                                BillingForm.Capital = string.Format("{0:#,##0.00}", 0);
                                                BillingForm.Gross = string.Format("{0:#,##0.00}", pSet.GetDouble("gross"));
                                            }

                                            m_sCapitalParam = BillingForm.Capital;
                                            m_sGrossParam = BillingForm.Gross;

                                            BillingForm.Quarter = pSet.GetString("qtr");
                                            BillingForm.QuarterTextBox.ReadOnly = false;
                                        }
                                    }
                                    
                                }
                                // RMC 20160517 correction in permit-update billing with renewal application (e)
                                */
                            }
                        }
                        //
                    }


                }

                /* // remove this code... replaced by populategrossinfo
                else
                {
                    // get the initial value of gross and capital from businesses, business_que and addl_bns
                    if (BillingForm.MainBusiness) 
                    {
                        BillingForm.BusinessCode = m_sMainBnsCode;
                        BillingForm.Status = m_sMainBnsStat;
                        BillingForm.Gross = string.Format("{0:#,##0.00}", m_fMainGross);
                        BillingForm.Capital = string.Format("{0:#,##0.00}", m_fMainCapital);
                    }
                    else
                    {
                        pSet.Query = "SELECT bns_stat, capital, gross FROM addl_bns WHERE bin = :1 AND bns_code_main = :2 AND tax_year = :3 AND qtr = :4";
                        pSet.AddParameter(":1", BillingForm.BIN);
                        pSet.AddParameter(":2", BillingForm.BusinessCode);
                        pSet.AddParameter(":3", BillingForm.TaxYear);
                        pSet.AddParameter(":4", BillingForm.Quarter);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                BillingForm.Status = pSet.GetString("bns_stat");
                                BillingForm.Capital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
                                BillingForm.Gross = string.Format("{0:#,##0.00}", pSet.GetDouble("gross"));
                            }
                        }

                    }
                    
                    // UpdateBillGrossInfo(); // insert initial values to bill_gross_info
                }
                */
                m_sIsQtrly = LibraryClass.IsQtrlyDec(BillingForm.BusinessCode, m_sRevYear);
            }
            pSet.Close();
        }

        public void EditAddlInfo()
        {
            AddlInfoReadOnly(false);
        }

        public void ApplyAddlInfo()
        {
            UpdateOtherInfo();
            RemoveDuplicateBillGrossInfo(); //AFM 20200609
            UpdateBillGrossInfo();
            UpdateBussGross();
        }

        public void ReLoadAddlInfo()
        {
            LoadAddlInfo();
            LoadGrossCapital();
            AddlInfoReadOnly(true);

        }

        private void RemoveDuplicateBillGrossInfo() //AFM 20200609 remove duplicates in bill_gross_info table that causes PL error
        {
            OracleResultSet result = new OracleResultSet();
            //result.Query = "DELETE FROM BILL_GROSS_INFO B1 ";
            //result.Query += "WHERE B1.bin = '"+ BillingForm.BIN + "' and B1.tax_year = '"+ BillingForm.TaxYear +"' and B1.qtr = '"+ BillingForm.Quarter +"' and B1.bns_code = '"+ BillingForm.BusinessCode +"' and B1.due_state = '"+ m_sDueState +"' "; //specific condition to make sure it's a duplicate
            //result.Query += "AND (SCN_TO_TIMESTAMP(ORA_ROWSCN)) <> "; //not equal to most recent record
            //result.Query += " (SELECT MAX(SCN_TO_TIMESTAMP(ORA_ROWSCN)) FROM BILL_GROSS_INFO B where b.bin = '" + BillingForm.BIN + "' and b.tax_year = '" + BillingForm.TaxYear + "' and b.qtr = '" + BillingForm.Quarter + "' and b.bns_code = '" + BillingForm.BusinessCode + "' and b.due_state = '" + m_sDueState + "')";

            //AFM 20210125 new fail-safe query to delete duplicates preventing pl error (above code works but not on client)
            result.Query = "DELETE FROM bill_gross_info a where rowid > (select min(rowid) from bill_gross_info b where a.bin = b.bin and a.tax_year = b.tax_year and a.bns_code = b.bns_code and a.qtr = b.qtr) and bin = '" + BillingForm.BIN + "' and tax_year = '" + BillingForm.TaxYear + "'";            

            if(result.ExecuteNonQuery() == 0)
            {
            }
            result.Close();
        }

        private void UpdateOtherInfo()
        {
            DataTable dataTable = new DataTable();
            string sDefaultCode, sDefaultDesc, sDataType, sUnit;
            int i, iRows;
            bool bIsReset = false;
            //AddlInfo.AddlInfo AddlInfoClass = new AddlInfo.AddlInfo();    // RMC 20110414 put rem
            AddlInfoClass = new AddlInfo();   // RMC 20110414

            AddlInfoClass.BIN = BillingForm.BIN;
            AddlInfoClass.TaxYear = BillingForm.TaxYear;
            if (BillingForm.MainBusiness)
                AddlInfoClass.BusinessCode = m_sMainBnsCode;
            else
                AddlInfoClass.BusinessCode = BillingForm.BusinessCode;
            sUnit = string.Empty;
            dataTable.Columns.Add("Code", typeof(String));
            dataTable.Columns.Add("Description", typeof(String));
            dataTable.Columns.Add("Type", typeof(String));
            dataTable.Columns.Add("Unit", typeof(String));
            iRows = BillingForm.AdditionalInformation.Rows.Count;
            for (i = 0; i < iRows; ++i)
            {
                sDefaultCode = BillingForm.AdditionalInformation.Rows[i].Cells[0].Value.ToString();
                sDefaultDesc = BillingForm.AdditionalInformation.Rows[i].Cells[1].Value.ToString();
                sDataType = BillingForm.AdditionalInformation.Rows[i].Cells[2].Value.ToString();
                sUnit = BillingForm.AdditionalInformation.Rows[i].Cells[3].Value.ToString();
                dataTable.Rows.Add(sDefaultCode, sDefaultDesc, sDataType, sUnit);
                if (ResetTaxAndFees(sDefaultCode, sUnit) == 1)
                    bIsReset = true;

                if (AppSettingsManager.GetConfigValue("10") == "019")    // RMC 20161216 limit update of capital when asset size was updated to Lubao version only
                {
                    // RMC 20150104 update capital when asset size was edited (s)
                    double dTmp = 0;
                    if (sDefaultDesc == "ASSET SIZE")
                    {
                        double.TryParse(sUnit, out dTmp);
                        if (dTmp != 0)
                        {
                            BillingForm.Capital = string.Format("{0:#,###.00}", dTmp);
                            UpdateBussCapital();
                        }
                    }
                    // RMC 20150104 update capital when asset size was edited (e)
                }
            }
            AddlInfoClass.UpdateAddlInfo(dataTable);

            if (bIsReset)
            {
                DelBilling();
                //DeleteBillDues(); // remove this.. PL for deletion
                m_bIsBillOK = false;
                m_sHoldBnsCode = BillingForm.BusinessCode;
                m_sHoldTaxYear = BillingForm.TaxYear;
                m_sHoldQtr = BillingForm.Quarter;
            }
            AddlInfoReadOnly(true);
        }

        /// <summary>
        /// ALJ 20100210
        /// Created this function to delete specific fees_code from tax_and_fees affected by editing other_info
        /// </summary>
        /// <param name="p_sDefaultCode"></param>
        /// <param name="p_sUnit"></param>
        private int ResetTaxAndFees(string p_sDefaultCode, string p_sUnit)
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            int iIsReset = 0;
            double dData;
            double.TryParse(p_sUnit, out dData);
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "reset_tax_fees";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sDefaultCode;
            plsqlCmd.AddParameter("p_sDefaultCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = dData;
            plsqlCmd.AddParameter("p_fData", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sRevYear;
            plsqlCmd.AddParameter("p_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = iIsReset;
            plsqlCmd.AddParameter("o_iIsReset", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Output);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
                plsqlCmd.Rollback();
            else
                int.TryParse(plsqlCmd.ReturnValue("o_iIsReset").ToString(), out iIsReset);
            plsqlCmd.Close();
            return iIsReset;

        }

        /// <summary>
        /// ALJ 20100211
        /// creates this function to delete dues affected by changes in addl_info
        /// </summary>
        private void DeleteBillDues()
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "delete_bill_dues";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.Quarter; ;
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
                plsqlCmd.Rollback();
            plsqlCmd.Close();
        }

        /// <summary>
        /// ALJ 20100205
        /// Created this function to save capital, gross , pre gross, adj gross and vat gross as billing parameter
        /// bill_gross_info table is a replacement for reg_table and declared_gross tables 
        /// </summary>
        private void UpdateBillGrossInfo()
        {
            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (s)
            // AST 20150115 (s)
            if (BillingForm.Status == "NEW" && m_HasUnsettledQtr)
            {
                int intQtr = 0;
                int.TryParse(BillingForm.Quarter, out intQtr);
                if (intQtr == 1)
                    m_sDueState = BillingForm.Status.Substring(0, 1);
                else if (intQtr >= 2 && intQtr <= 4)
                    m_sDueState = "Q";
            }
            // AST 20150115 (e)
            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez (e)

            OraclePLSQL plsqlCmd = new OraclePLSQL();
            double dGrCap;
            int iIsReset = 0;
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "update_billgross_info";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.Quarter;
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.Status;
            plsqlCmd.AddParameter("p_sStatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sDueState;
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            double.TryParse(BillingForm.Capital, out dGrCap);
            //////////
            m_sCapitalParam = BillingForm.Capital; // assign value for m_sCapitalParam - basis for billing 
            //////////
            plsqlCmd.ParamValue = dGrCap;
            plsqlCmd.AddParameter("p_fCapital", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            double.TryParse(BillingForm.Gross, out dGrCap);
            //////////
            m_sGrossParam = BillingForm.Gross; // assign value for m_sGrossParam - basis for billing 
            //////////
            plsqlCmd.ParamValue = dGrCap;
            plsqlCmd.AddParameter("p_fGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            double.TryParse(BillingForm.PreGross, out dGrCap);
            //////////
            if (dGrCap > 0) // assign value for m_sGrossParam pre gross will prevail - basis for billing 
                m_sGrossParam = BillingForm.PreGross;
            //////////
            plsqlCmd.ParamValue = dGrCap;
            plsqlCmd.AddParameter("p_fPreGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            double.TryParse(BillingForm.AdjGross, out dGrCap);
            //////////
            if (dGrCap > 0) // assign value for m_sGrossParam Adj Gross will prevail - basis for billing 
            // (s) ALJ 20120309 Rev exam for new
            {
                if (BillingForm.Status == "NEW")
                    m_sCapitalParam = BillingForm.AdjGross;
                else
                    // (e)ALJ 20120309 Rev exam for new
                    m_sGrossParam = BillingForm.AdjGross;
            } // ALJ 20120309 Rev exam for new
            //////////
            plsqlCmd.ParamValue = dGrCap;
            plsqlCmd.AddParameter("p_fAdjGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            double.TryParse(BillingForm.VATGross, out dGrCap);
            plsqlCmd.ParamValue = dGrCap;
            plsqlCmd.AddParameter("p_fVatGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = iIsReset;
            plsqlCmd.AddParameter("o_iIsReset", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Output);

            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
                plsqlCmd.Rollback();
            else
                int.TryParse(plsqlCmd.ReturnValue("o_iIsReset").ToString(), out iIsReset);
            plsqlCmd.Close();
            if (iIsReset == 1)
            {
                DelBilling();
                //DeleteBillDues(); // remove this.. PL for deletion
                m_bIsBillOK = false;
                m_sHoldBnsCode = BillingForm.BusinessCode;
                m_sHoldTaxYear = BillingForm.TaxYear;
                m_sHoldQtr = BillingForm.Quarter;

            }


            /*
            OracleResultSet pCmd = new OracleResultSet();
            double dGrCap;
            pCmd.Query = "DELETE FROM bill_gross_info WHERE bin = :1 AND tax_year = :2 AND qtr = :3 AND bns_code = :4";
            pCmd.AddParameter(":1", BillingForm.BIN);
            pCmd.AddParameter(":2", BillingForm.TaxYear);
            pCmd.AddParameter(":3", BillingForm.Quarter);
            pCmd.AddParameter(":4", BillingForm.BusinessCode);
            pCmd.ExecuteNonQuery();
            pCmd.Query = "INSERT INTO bill_gross_info VALUES (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10)";
            pCmd.AddParameter(":1", BillingForm.BIN);
            pCmd.AddParameter(":2", BillingForm.TaxYear);
            pCmd.AddParameter(":3", BillingForm.Quarter);
            pCmd.AddParameter(":4", BillingForm.BusinessCode);
            pCmd.AddParameter(":5", BillingForm.Status);
            double.TryParse(BillingForm.Capital, out dGrCap);
            pCmd.AddParameter(":6", dGrCap);
            double.TryParse(BillingForm.Gross, out dGrCap);
            pCmd.AddParameter(":7", dGrCap);
            double.TryParse(BillingForm.PreGross, out dGrCap);
            pCmd.AddParameter(":8", dGrCap);
            double.TryParse(BillingForm.AdjGross, out dGrCap);
            pCmd.AddParameter(":9", dGrCap);
            double.TryParse(BillingForm.VATGross, out dGrCap);
            pCmd.AddParameter(":10", dGrCap);
            pCmd.ExecuteNonQuery();


            pCmd.Close();
          */


        }

        protected virtual void UpdateBussGross()
        {
            OracleResultSet pCmd = new OracleResultSet();
            double dGrCap;
            if (BillingForm.MainBusiness)
            {
                //if (m_iSwRE == 1 || m_iSwRE == 2) Note: For Revenue Examination see RevExam Class UpadateGross Function
                //{ }
                //else
                //{
                if (BillingForm.Status != "NEW")
                {
                    double.TryParse(BillingForm.Gross, out dGrCap);
                    pCmd.Query = "UPDATE business_que SET gr_1 = :1 WHERE bin = :2 AND tax_year = :3";
                    pCmd.AddParameter(":1", dGrCap);
                    pCmd.AddParameter(":2", BillingForm.BIN);
                    pCmd.AddParameter(":3", BillingForm.TaxYear);
                    pCmd.ExecuteNonQuery();
                }
                else
                {
                    if (m_sDueState == "N")
                    {
                        double.TryParse(BillingForm.Capital, out dGrCap);
                        pCmd.Query = "UPDATE business_que SET capital = :1 WHERE bin =:2 AND tax_year = :3";
                        pCmd.AddParameter(":1", dGrCap);
                        pCmd.AddParameter(":2", BillingForm.BIN);
                        pCmd.AddParameter(":3", BillingForm.TaxYear);
                        pCmd.ExecuteNonQuery();


                    }
                }
                //{
            }
            else
            {
                pCmd.Query = "UPDATE addl_bns SET capital = :1, gross = :2 WHERE bin = :3 AND bns_code_main = :4 AND tax_year = :5 AND qtr = :6";
                double.TryParse(BillingForm.Capital, out dGrCap);
                pCmd.AddParameter(":1", dGrCap);
                double.TryParse(BillingForm.Gross, out dGrCap);
                pCmd.AddParameter(":2", dGrCap);
                pCmd.AddParameter(":3", BillingForm.BIN);
                pCmd.AddParameter(":4", BillingForm.BusinessCode);
                pCmd.AddParameter(":5", BillingForm.TaxYear);
                pCmd.AddParameter(":6", BillingForm.Quarter);
                pCmd.ExecuteNonQuery();

            }
            pCmd.Close();


        }

        protected virtual void AddlInfoReadOnly(bool p_blnEnabled)
        {
            BillingForm.AdditionalInformation.Columns[3].ReadOnly = p_blnEnabled;
            if (BillingForm.Status == "NEW" && m_sDueState == "N")
                BillingForm.CapitalTextBox.ReadOnly = p_blnEnabled;
            else
            {
                BillingForm.GrossTextBox.ReadOnly = p_blnEnabled;
                BillingForm.PreGrossTextBox.ReadOnly = p_blnEnabled;
                BillingForm.AdjGrossTextBox.ReadOnly = p_blnEnabled;
                BillingForm.VATGrossTextBox.ReadOnly = p_blnEnabled;
            }
        }
        public void RetrieveBilling()
        {
            if (BillingForm.IsBillingAccessOK()) // ALJ 20110907 re-assessment validation
            {
                // RMC 20120118 modified billing for no operation/no gross (s)
                // CALAMBA  
                m_bIsNoSale = false;
                double dGross = 0;
                double.TryParse(m_sGrossParam, out dGross);
                //if (dGross == 0 && m_sMainBnsStat != "NEW") // RMC 20120119 disable validation of zero gross for NEW business in Billing
                if (dGross == 0 && m_sMainBnsStat != "NEW" && BillingForm.Status != "NEW") // RMC 20150108
                {
                    if (MessageBox.Show("Zero business tax detected.\nContinue?", "Billing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        m_bIsNoSale = true;

                        LibraryClass.AuditTrail("AIB-MS-ZT", "multi-table", BillingForm.BIN + " TY" + BillingForm.TaxYear);

                        //* yes -atreail bin - 
                    }
                    else
                    {
                        m_bIsNoSale = false;
                        return;
                    }
                }
                // CALAMBA  
                // RMC 20120118 modified billing for no operation/no gross (e)

                InitBillTable();
                UpdateList();
                BillingForm.BillFlag = true;    
            } // ALJ 20110907 re-assessment validation
        }

        private void InitBillTable()
        {
            if (m_bPermitUpdateFlag == false)   // RMC 20150325 modified billing for permit update application
            {
                // RMC 20150108 (S)
                if (BillingForm.Status == "NEW")
                    m_sDueState = "N";
                // RMC 20150108 (E)
            }

            OraclePLSQL plsqlCmd = new OraclePLSQL();
            double dGross, dCapital, dTotal, dGrandTotal;
            double.TryParse(m_sGrossParam, out dGross);
            //double.TryParse(BillingForm.Capital, out dCapital); // ALJ 20120309 Rev exam for new - put "//"
            double.TryParse(m_sCapitalParam, out dCapital); // ALJ 20120309 Rev exam for new
            string sBillScope;
            dTotal = 0.00;
            dGrandTotal = 0.00;
            if (BillingForm.MainBusiness)
                sBillScope = "MAIN";
            else
                sBillScope = "ADDL";
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "init_bill_table3";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.Status;
            plsqlCmd.AddParameter("p_sStatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sDueState;
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.Quarter;
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sDtOperated;
            plsqlCmd.AddParameter("p_sDateOperated", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = dGross;
            plsqlCmd.AddParameter("p_fGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = dCapital;
            plsqlCmd.AddParameter("p_fCapital", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sRevYear;
            plsqlCmd.AddParameter("p_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_iSwRE;
            plsqlCmd.AddParameter("p_iSwRE", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            //plsqlCmd.ParamValue = 0; // temporary hardcoded
            plsqlCmd.ParamValue = m_bPermitUpdateFlag;  // RMC 20140725 Added Permit update billing
            plsqlCmd.AddParameter("p_bPermitUpdateFlag", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            //plsqlCmd.ParamValue = 0; // temporary hardcoded
            plsqlCmd.ParamValue = m_bIsRenPUT;  // RMC 20140725 Added Permit update billing
            plsqlCmd.AddParameter("p_bIsRenPUT", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sPUTApplType;
            plsqlCmd.AddParameter("p_sPUTApplType", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = "BUSINESS TAX"; // temporary hardcoded
            plsqlCmd.AddParameter("p_sTaxHeader", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            // plsqlCmd.ParamValue = ""; // temporary hardcoded
            // plsqlCmd.AddParameter("p_sFireTaxCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sIsQtrly; // ALJ 20110321
            plsqlCmd.AddParameter("p_sIsQtrly", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = ConfigurationAttributes.LGUCode;
            plsqlCmd.AddParameter("p_sLGUCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sBillScope;
            plsqlCmd.AddParameter("p_sBillScope", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = dTotal;
            plsqlCmd.AddParameter("o_fTotal", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Output);
            plsqlCmd.ParamValue = dGrandTotal;
            plsqlCmd.AddParameter("o_fGrandTotal", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Output);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
                plsqlCmd.Close();
            }
            else
            {
                double.TryParse(plsqlCmd.ReturnValue("o_fGrandTotal").ToString(), out dGrandTotal);
            }
            plsqlCmd.Close();
            BillingForm.Total = string.Format("{0:#,##0.00}", dGrandTotal);

        }

        private void UpdateList()
        {
            OracleResultSet pSet = new OracleResultSet();
            double fAmount;
            string sFeesDesc;
            string sFeesCode;
            BillingForm.TaxAndFees.Rows.Clear();
            // taxes and fees
            pSet.Query = "SELECT fees_desc, amount, fees_code FROM bill_table WHERE bin = :1 AND bns_code_main = :2 AND fees_type = 'TF' ORDER BY fees_code";
            pSet.AddParameter(":1", BillingForm.BIN);
            pSet.AddParameter(":2", BillingForm.BusinessCode);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sFeesDesc = pSet.GetString("fees_desc").Trim();
                    fAmount = pSet.GetDouble("amount");
                    sFeesCode = pSet.GetString("fees_code");

                    /*
                    // RMC 20150115 migrated modify dues module to cater delinquent years not covered by the current revenue code (s)
                    double dTmpAmount = 0;

                    GetPrevTaxPaid(sFeesCode, BillingForm.BusinessCode, out dTmpAmount);
                    if (dTmpAmount != 0)
                        fAmount = dTmpAmount;
                    // RMC 20150115 migrated modify dues module to cater delinquent years not covered by the current revenue code (e)
                    */ // RMC 20150117 put rem

                    //AFM 20191227 MAO-19-11726
                      String GetMainbnsCode = AppSettingsManager.GetBnsCodeMain(BillingForm.BIN);
                    if (sFeesCode == "25" && GetMainbnsCode != BillingForm.BusinessCode) //change fees code according to schedule of fees
                        continue;
                    //AFM 20191227 MAO-19-11726

                    if (fAmount > 0)
                        BillingForm.TaxAndFees.Rows.Add(true, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "TF");
                    else
                        BillingForm.TaxAndFees.Rows.Add(false, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "TF");
                }
            }
            // addl charges and fire tax
            pSet.Query = "SELECT fees_desc, amount, fees_code FROM bill_table WHERE bin = :1 AND bns_code_main = :2 AND fees_type = 'AF' ORDER BY fees_code";
            pSet.AddParameter(":1", BillingForm.BIN);
            pSet.AddParameter(":2", BillingForm.BusinessCode);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sFeesDesc = pSet.GetString("fees_desc").Trim();
                    fAmount = pSet.GetDouble("amount");
                    sFeesCode = pSet.GetString("fees_code");

                    if (BillingForm.SourceClass != "RevExam") //JARS 20171006
                    {
                        if (fAmount > 0)
                            BillingForm.TaxAndFees.Rows.Add(true, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "AF");
                        else
                            BillingForm.TaxAndFees.Rows.Add(false, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "AF");
                    }
                    else
                    {
                        //AFM 20200109 removed amount set to zero -malolos ver.
                        //fAmount = 0;
                        //BillingForm.TaxAndFees.Rows.Add(false, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "AF");
                        if (fAmount > 0) //AFM 20200109 added
                            BillingForm.TaxAndFees.Rows.Add(true, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "AF");
                        else
                            BillingForm.TaxAndFees.Rows.Add(false, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "AF");
                    }

                }
            }
           
            pSet.Close();
           // BillingForm.TaxAndFees.Rows.Add(false, "BARANGAY CLEARANCE FEE ", string.Format("{0:#,##0.00}", 0), "24", "AF");//PEB 20191205

        }

        public double BillBtax()
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            //OracleResultSet pSet = new OracleResultSet(); // ALJ 20120109 put remark
            string sMessagePrompt = string.Empty; // ALJ 20120109 Message Prompt from bill_btax proc if no schedule found of no data in other info 
            double dConvert = 0;
            double dBtaxAmt = 0;

            // (s) ALJ 20120109 Message Prompt from bill_btax proc if no schedule found of no data in other info 
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "bill_btax";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.Status;
            plsqlCmd.AddParameter("p_sSTatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sDueState;
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            double.TryParse(m_sGrossParam, out dConvert);
            plsqlCmd.ParamValue = dConvert;
            plsqlCmd.AddParameter("p_fGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            //double.TryParse(BillingForm.Capital, out dConvert); // ALJ 20120309 Rev exam for new - put "//"
            double.TryParse(m_sCapitalParam, out dConvert); // ALJ 20120309 Rev exam for new
            plsqlCmd.ParamValue = dConvert;
            plsqlCmd.AddParameter("p_fCapital", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sRevYear;
            plsqlCmd.AddParameter("p_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = dBtaxAmt;
            plsqlCmd.AddParameter("o_fAmount", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Output);
            plsqlCmd.ParamValue = string.Empty;
            plsqlCmd.AddParameter("o_sMessagePrompt", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Output, 100);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
                plsqlCmd.Close();

            }
            else
            {
                double.TryParse(plsqlCmd.ReturnValue("o_fAmount").ToString(), out dBtaxAmt);
                sMessagePrompt = plsqlCmd.ReturnValue("o_sMessagePrompt").ToString();
                if (sMessagePrompt != "null")
                {
                    MessageBox.Show(sMessagePrompt, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            plsqlCmd.Close();
            return dBtaxAmt;
            // (e) ALJ 20120109 Message Prompt from bill_btax proc if no schedule found of no data in other info 

            /*  // ALJ 20120109 put remarks
            pSet.Query = "select bill_btax(:1,:2,:3,:4,:5,:6,:7,:8) as amount from dual";
            pSet.AddParameter(":1",BillingForm.BIN);
            pSet.AddParameter(":2", BillingForm.BusinessCode);
            pSet.AddParameter(":3", BillingForm.TaxYear);
            pSet.AddParameter(":4", BillingForm.Status);
            pSet.AddParameter(":5", m_sDueState);
            double.TryParse(m_sGrossParam, out dConvert);
            pSet.AddParameter(":6", dConvert);
            double.TryParse(BillingForm.Capital, out dConvert);
            pSet.AddParameter(":7", dConvert);
            pSet.AddParameter(":8", m_sRevYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                { 
                   dBtaxAmt = pSet.GetDouble("amount");
                }
            }
            pSet.Close();
            return dBtaxAmt;
             */
            // ALJ 20120109 put remarks
        }

        public double BillFees(string p_sFeesCode, string p_sFeesDesc, string p_sFeesType)
        {
            double dFeesAmt, dGross, dCapital;
            double.TryParse(m_sGrossParam, out dGross);
            //double.TryParse(BillingForm.Capital, out dCapital); // ALJ 20120309 Rev exam for new - put "//"
            double.TryParse(m_sCapitalParam, out dCapital); // ALJ 20120309 Rev exam for new
            frmBillFee BillFeeForm = new frmBillFee();
            switch (p_sFeesType)
            {
                case "TF": // tax and fees
                    {
                        BillFeeForm.SourceClass = new BillFee(BillFeeForm);
                        break;
                    }
                case "AF":
                    {
                        if (p_sFeesCode == "01") // addl charges
                            BillFeeForm.SourceClass = new BillAddl(BillFeeForm);
                        else if (p_sFeesCode == "24")
                            BillFeeForm.SourceClass = new BillAddl(BillFeeForm);
                        else if (p_sFeesCode == "50")
                            BillFeeForm.SourceClass = new BillBrgy(BillFeeForm);
                        else // "02" fire tax
                            BillFeeForm.SourceClass = new BillFire(BillFeeForm);
                        break;
                    }
            }
            
            BillFeeForm.BIN = BillingForm.BIN;
            BillFeeForm.Status = BillingForm.Status;
            BillFeeForm.Capital = dCapital;
            BillFeeForm.Gross = dGross;
            BillFeeForm.BusinessCode = BillingForm.BusinessCode;
            BillFeeForm.FeesCode = p_sFeesCode;
            BillFeeForm.Text = p_sFeesDesc; // set header
            if (p_sFeesDesc == "BARANGAY CLEARANCE FEE")//peb 20191218
            {
                OracleResultSet result = new OracleResultSet();

                frmBillFee frm = new frmBillFee();
                string queryresult = "";
                result.Query = "select bns_brgy from businesses where bin ='" + BillingForm.BIN + "'";
                if (result.Execute())
                {
                    if (result.Read())
                        queryresult = result.GetString("bns_brgy");
                     
                }
                result.Close();
                if (queryresult == "") //peb20191227
                {
                    result.Query = " select bns_brgy from business_que where bin ='" + BillingForm.BIN + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                            queryresult = result.GetString("bns_brgy");

                    }
                    result.Close();
                }

                //result.Query = "select SUM(AMOUNT) as amount from barangay_addl_sched where brgy_nm='" + queryresult + "'";
                //if (result.Execute())
                //{
                //    while (result.Read())
                //    {

                //       // BillFeeForm.Fees.Add(false, "BARANGAY CLEARANCE FEE", result.GetInt("amount").ToString("N0"), "26", "TF");
                //    }
                //}
                //result.Close();
               
              // frm.moduleswitch = "BARANGAY CLEARANCE FEE";
               BillFeeForm.moduleswitch = "BARANGAY CLEARANCE FEE";
               BillFeeForm.brgy = queryresult;
               frm.brgy = queryresult;
            }
            BillFeeForm.TaxYear = BillingForm.TaxYear;
            BillFeeForm.Quarter = BillingForm.Quarter;
            BillFeeForm.RevisionYear = m_sRevYear;
            BillFeeForm.ShowDialog();
            dFeesAmt = BillFeeForm.TotalDue;
            // (s) ALJ 20110725 lgu specific fee
            if (ConfigurationAttributes.LGUCode == "192") // CALAMBA LGU specific Fee (Garbage)
            {
                if (p_sFeesCode == "02") // Garbage fee for CALAMBA
                {
                    dFeesAmt = LGUSpecificFee(p_sFeesCode, dFeesAmt);
                }
            }
            // (e) ALJ 20110725
            return dFeesAmt;
        }

        public void UpdateBillTable(double p_fAmount, string p_sType, string p_sFeesCode)
        {
            OracleResultSet pCmd = new OracleResultSet();
            pCmd.Query = "UPDATE bill_table SET amount = " + p_fAmount + " WHERE bin = '" + BillingForm.BIN + "' and bns_code_main = '" + BillingForm.BusinessCode + "' AND fees_code = '" + p_sFeesCode + "' AND fees_type = '" + p_sType + "'";
            //pCmd.AddParameter(":1", p_fAmount);
            //pCmd.AddParameter(":2", BillingForm.BIN);
            //pCmd.AddParameter(":3", BillingForm.BusinessCode);
            //pCmd.AddParameter(":4", p_sFeesCode);
            //pCmd.AddParameter(":5", p_sType);
          //  pCmd.ExecuteNonQuery(); peb20191220
            if (pCmd.Execute())
            {

            }
            else
            {
             // pCmd.Query="insert into bill_table values('" +BillingForm.BIN+"', '" +BillingForm.BusinessCode+"','" +BillingForm.BusinessCode+"','"+p_sType+"', '"+p_sFeesCode+"','"+p_sType+"' ) "
            }
            pCmd.Close();
        }

        public double UpdateFireTax()
        {
            //AFM 20210610 added from binan ver. (s)
            String sBillScope = "";
            if (BillingForm.MainBusiness)
                sBillScope = "MAIN";
            else
                sBillScope = "ADDL";
            //AFM 20210610 added from binan ver. (e)

            OraclePLSQL plsqlCmd = new OraclePLSQL();
            double fFireTotal = 0;
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "update_fire_tax_table";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sRevYear;
            plsqlCmd.AddParameter("p_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sBillScope;
            plsqlCmd.AddParameter("p_sBillScope", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = fFireTotal;
            plsqlCmd.AddParameter("o_fFireTax", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Output);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
                plsqlCmd.Close();

            }
            else
            {
                double.TryParse(plsqlCmd.ReturnValue("o_fFireTax").ToString(), out fFireTotal);

            }
            plsqlCmd.Close();
            return fFireTotal;

        }

        public double Total()
        {

            double fTotal, fAmount;
            fTotal = 0.00;
            int iRows, i;
            iRows = BillingForm.TaxAndFees.Rows.Count;
            for (i = 0; i < iRows; ++i)
            {
                double.TryParse(BillingForm.TaxAndFees.Rows[i].Cells[2].Value.ToString(), out fAmount);
                fTotal = fTotal + fAmount;
            }

            return fTotal;
        }

        //public void Save()
        public virtual void Save() // RMC 20110807 created CancelBilling module (changed to public virtual)
        {
            /*if (m_sGrossParam == "0.00" && m_sMainBnsStat == "REN")
            {
                MessageBox.Show("Gross Amount is required", BillingForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }*/ // RMC 20150106

            OracleResultSet pCmd = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();
            DateTime dtSystemDate = DateTime.Now;
            // testing of required fee and exempted fee
            bool bBillOK = false;
            double fAmount, dGross;
            int iRows, i, iBillSerial;
            string sFeesCode, sFeesDesc, sFeesType, sBillNo, sToday;
            m_bIsBillCreated = false;
            iRows = BillingForm.TaxAndFees.Rows.Count;
            sFeesDesc = string.Empty;
            for (i = 0; i < iRows; ++i)
            {
                double.TryParse(BillingForm.TaxAndFees.Rows[i].Cells[2].Value.ToString(), out fAmount);
                sFeesCode = BillingForm.TaxAndFees.Rows[i].Cells[3].Value.ToString();
                sFeesDesc = BillingForm.TaxAndFees.Rows[i].Cells[1].Value.ToString().Trim(); // AJ 20090115 change cell index
                sFeesType = BillingForm.TaxAndFees.Rows[i].Cells[4].Value.ToString().Trim(); // AJ 20090115
                //if (fAmount > 0 || BnsIsExempted(sFeesCode, BillingForm.BusinessCode) || sFeesType == "AF" || sFeesCode == "16") // AJ 20090115 || sFeesType == 'AF' // GDE 20101110 temporary add code 16 for testing only
                /*if (fAmount > 0 || BnsIsExempted(sFeesCode, BillingForm.BusinessCode) || sFeesType == "AF"
                    || BillingForm.Status == "RET")*/
                // RMC 20140708 added retirement billing, deleted sFeesCode == "16" added by // GDE 20101110 temporary add code 16 for testing only
                // RMC 20140725 Added Permit update billing (s)
                if (fAmount > 0 || BnsIsExempted(sFeesCode, BillingForm.BusinessCode)
                    || (sFeesType == "AF" && (!m_bIsRenPUT && !m_blnNewAddl && !m_blnTransLoc && !m_blnTransOwn && !m_blnChClass && !m_blnChBnsName && !m_blnChOrgKind))
                    || BillingForm.Status == "RET")
                // RMC 20140725 Added Permit update billing (e)
                {
                    bBillOK = true;
                }
                else
                {
                    if (ValidateAddlSched(BillingForm.BusinessCode, sFeesDesc)) // RMC 20140109 validation for garbage added in other charges table - Mati
                    {
                        bBillOK = true;
                    }
                    else
                    {
                        if (ValidatePermitUpdateTrans(BillingForm.BusinessCode, sFeesDesc)) // RMC 20140725 Added Permit update billing
                        {
                            bBillOK = true;
                        }
                        else
                        {
                            bBillOK = false;
                            break;
                        }
                    }
                }
            }

            if (!bBillOK)
            {
                MessageBox.Show(sFeesDesc + " is required.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else // Saving
            {
                // RMC 20171128 added validation in Billing if with ECC, should unbill CNC (s)
                if (!ValidateLguSpecificTagging(BillingForm.BIN, "ECC"))
                {
                    MessageBox.Show("BIN already has ECC. \nPlease remove CNC billing", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                // RMC 20171128 added validation in Billing if with ECC, should unbill CNC (e)

                DelBilling();
                if (m_bIsNoSale)
                {
                    // Note: pending treatment for no sale or non-operation
                }
                if (m_sDueState == "Q" || m_bIsReg)
                {
                    // Note: reg_table will be disabled when fully migrated to C# - delete the ff codes in the future
                    pCmd.Query = "delete from reg_table where bin = :1 and tax_year = :2 and qtr = :3";
                    pCmd.AddParameter(":1", BillingForm.BIN);
                    pCmd.AddParameter(":1", BillingForm.TaxYear);
                    pCmd.AddParameter(":1", BillingForm.Quarter);
                    pCmd.ExecuteNonQuery();
                    pCmd.Query = "insert into reg_table values (:1,:2,:3,:4,:5,:6,:7,:8)";
                    pCmd.AddParameter(":1", BillingForm.BIN);
                    pCmd.AddParameter(":2", BillingForm.TaxYear);
                    pCmd.AddParameter(":3", BillingForm.Quarter);
                    double.TryParse(BillingForm.Gross, out dGross);
                    pCmd.AddParameter(":4", dGross);
                    pCmd.AddParameter(":5", 0); // floor area set to zero not neccessary
                    pCmd.AddParameter(":6", 0); // no. of employee set to zero not neccessary
                    pCmd.AddParameter(":7", 0); // no. of vehicle set to zero not neccessary
                    pCmd.AddParameter(":8", 0); // no. of machinery set to zero not neccessary
                    pCmd.ExecuteNonQuery();
                }

                int iCurrentYear, iTaxYear;
                int.TryParse(m_sCurrentYear, out iCurrentYear);
                int.TryParse(BillingForm.TaxYear, out iTaxYear);
                if ((m_sDueState == "Q" || iTaxYear < iCurrentYear) && m_iSwRE == 0 && !BillingForm.MainBusiness)
                {
                    pCmd.Query = "delete from addl_bns where bin = :1 and tax_year = :2 and qtr = :3 and bns_code_main = :4 and bns_stat = :5";
                    pCmd.AddParameter(":1", BillingForm.BIN);
                    pCmd.AddParameter(":2", BillingForm.TaxYear);
                    pCmd.AddParameter(":3", BillingForm.Quarter);
                    pCmd.AddParameter(":4", BillingForm.BusinessCode);
                    pCmd.AddParameter(":5", BillingForm.Status);
                    pCmd.ExecuteNonQuery();
                    pCmd.Query = "insert into addl_bns values (:1,:2,:3,:4,:5,:6,:7, :8)";
                    pCmd.AddParameter(":1", BillingForm.BIN);
                    pCmd.AddParameter(":2", BillingForm.BusinessCode);
                    pCmd.AddParameter(":3", 0);
                    double.TryParse(BillingForm.Gross, out dGross);
                    pCmd.AddParameter(":4", dGross);
                    pCmd.AddParameter(":5", BillingForm.TaxYear);
                    pCmd.AddParameter(":6", BillingForm.Status);
                    pCmd.AddParameter(":7", BillingForm.Quarter);
                    pCmd.AddParameter(":8", null);
                    pCmd.ExecuteNonQuery();
                }

                for (i = 0; i < iRows; ++i)
                {
                    sFeesCode = BillingForm.TaxAndFees.Rows[i].Cells[3].Value.ToString();
                    sFeesType = BillingForm.TaxAndFees.Rows[i].Cells[4].Value.ToString();
                    double.TryParse(BillingForm.TaxAndFees.Rows[i].Cells[2].Value.ToString(), out fAmount);

                    if (m_iSwRE == 1)    // RMC 20170224 for rev-exam insert all fees in taxdues_dup even if amount is 0 to compute over payment - Binan (s)
                    {
                        InsertDues(sFeesType, sFeesCode, fAmount);
                    }// RMC 20170224 for rev-exam insert all fees in taxdues_dup even if amount is 0 to compute over payment - Binan (e)
                    else
                    {
                        if (fAmount > 0)
                        {
                            InsertDues(sFeesType, sFeesCode, fAmount);
                        }
                     }
                }
                // insert into bill_no and bill_hist 
                // set bill_serial
                sBillNo = string.Empty;
                dtSystemDate = AppSettingsManager.GetSystemDate();
                sToday = string.Format("{0:MM/dd/yyyy}", dtSystemDate);
                iBillSerial = 0;
                pSet.Query = "select bill_no_seq.nextval as bill_no from dual";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        iBillSerial = pSet.GetInt("bill_no");
                        sBillNo = sToday.Substring(0, 2) + sToday.Substring(6, 4) + "-" + string.Format("{0:00000}", iBillSerial);
                    }
                }
                if (iBillSerial > 0)
                {
                    pCmd.Query = "insert into bill_no values(:1,:2,:3,:4,to_date(:5,'mm/dd/yyyy'),:6,:7,to_date(:8,'mm/dd/yyyy'),:9,:10,:11)";
                    pCmd.AddParameter(":1", BillingForm.BIN);
                    pCmd.AddParameter(":2", BillingForm.TaxYear);
                    pCmd.AddParameter(":3", BillingForm.BusinessCode);
                    pCmd.AddParameter(":4", sBillNo);
                    pCmd.AddParameter(":5", sToday);
                    pCmd.AddParameter(":6", AppSettingsManager.SystemUser.UserCode);
                    //pCmd.AddParameter(":6", "SYS_PROG"); // temporary hardcoded
                    pCmd.AddParameter(":7", 0); // grace period set to 0
                    pCmd.AddParameter(":8", sToday);
                    pCmd.AddParameter(":9", string.Empty); // received by set to empty
                    pCmd.AddParameter(":10", BillingForm.Quarter);
                    pCmd.AddParameter(":11", m_sDueState);
                    pCmd.ExecuteNonQuery();

                    pCmd.Query = "insert into bill_hist values(:1,:2,:3,:4,to_date(:5,'mm/dd/yyyy'),:6,:7,to_date(:8,'mm/dd/yyyy'),:9,:10,:11)";
                    pCmd.AddParameter(":1", BillingForm.BIN);
                    pCmd.AddParameter(":2", BillingForm.TaxYear);
                    pCmd.AddParameter(":3", BillingForm.BusinessCode);
                    pCmd.AddParameter(":4", sBillNo);
                    pCmd.AddParameter(":5", sToday);
                    pCmd.AddParameter(":6", AppSettingsManager.SystemUser.UserCode);
                    //pCmd.AddParameter(":6", "SYS_PROG"); // temporary hardcoded
                    pCmd.AddParameter(":7", 0); // grace period set to 0
                    pCmd.AddParameter(":8", sToday);
                    pCmd.AddParameter(":9", string.Empty); // received by set to empty
                    pCmd.AddParameter(":10", BillingForm.Quarter);
                    pCmd.AddParameter(":11", m_sDueState);
                    pCmd.ExecuteNonQuery();

                }
                else
                {
                    MessageBox.Show("NO Serial for Bill NO. Contact your System Developer", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pCmd.Close();
                    pSet.Close();
                    return;
                }

                //MCR 20170529 (s)
                if (BillingForm.chkTagReass.Checked == true)
                {
                    if (!isTAGGED(BillingForm.BIN, BillingForm.TaxYear))
                    {
                        pCmd.Query = "insert into REASS_WATCH_LIST values(:1,:2,1,:3)";
                        pCmd.AddParameter(":1", BillingForm.BIN);
                        pCmd.AddParameter(":2", BillingForm.TaxYear);
                        pCmd.AddParameter(":3", AppSettingsManager.GetBnsName(BillingForm.BIN));
                        pCmd.ExecuteNonQuery();
                    }
                    BillingForm.chkTagReass.Checked = false;
                }
                //MCR 20170529 (e)
                double dAmount = 0;
                string sObject = "";
                sObject = BillingForm.BIN + " TY" + BillingForm.TaxYear;
                //JARS 20180925 (S)
                if (BillingForm.Status == "REN")
                {
                    double.TryParse(BillingForm.Gross, out dAmount);
                    sObject = sObject + " GROSS:" + dAmount.ToString("#,##0.00");
                }
                else if (BillingForm.Status == "NEW")
                {
                    double.TryParse(BillingForm.Capital, out dAmount);
                    sObject = sObject + " CAPITAL:" + dAmount.ToString("#,##0.00");
                }
                //JARS 20180925 (E)
                if (m_iSwRE == 1 || m_iSwRE == 2)
                    LibraryClass.AuditTrail("AIB-MS-RE", "multi-table", sObject);
                else
                    LibraryClass.AuditTrail("AIB-MS", "multi-table", sObject);
                MessageBox.Show("Billing created.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);


                ComputeAdjustment();
                m_bIsBillCreated = true;
                if (m_bIsBillOK == false && BillingForm.TaxYear == m_sHoldTaxYear && BillingForm.Quarter == m_sHoldQtr && BillingForm.BusinessCode == m_sHoldBnsCode)
                {
                    m_bIsBillOK = true;
                }
            }
            
            //MCR 20210710 (s)
            pCmd.Query = "insert into soa_monitoring_hist (select bin,status,memo,refno,sysdate from soa_monitoring where bin = :1)";
            pCmd.AddParameter(":1", BillingForm.BIN);
            pCmd.ExecuteNonQuery();
            //MCR 20210710 (s)

            //AFM 20210625 soa monitoring - new module (s)
            pCmd.Query = "delete from soa_monitoring where bin = :1";
            pCmd.AddParameter(":1", BillingForm.BIN);
            pCmd.ExecuteNonQuery();

            pCmd.Query = "insert into soa_monitoring values(:1,:2,:3,:4)";
            pCmd.AddParameter(":1", BillingForm.BIN);
            pCmd.AddParameter(":2", "PENDING");
            pCmd.AddParameter(":3", "");
            pCmd.AddParameter(":4", "");
            pCmd.ExecuteNonQuery();
            //AFM 20210625 soa monitoring - new module (s)

            //MCR 20210708 (s)
            if (AppSettingsManager.GetConfigValue("75") == "Y") 
            {
                String sValue = "", sRefNo = "";
                sRefNo = GetSOARefNo(BillingForm.BIN);
                pSet = new OracleResultSet();
                pSet.Query = "select BO_VOID_REF_NO('" + sRefNo + "') from dual";
                if (pSet.Execute())
                    if (pSet.Read())
                        sValue = pSet.GetString(0);
                pSet.Close();
            }
            //MCR 20210708 (e)
            pCmd.Close();
            pSet.Close();
        }

        private string GetSOARefNo(string sBin)
        {
            String sValue = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select refno from soa_monitoring where bin = '" + sBin + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sValue = pSet.GetString(0);
            pSet.Close();

            return sValue;
        }

        private bool BnsIsExempted(string p_sFeesCode, string p_sBnsCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            if (p_sFeesCode == "00")
            {
                // (s) ALJ 20110802 Special Tax Exemption (PEZA, BOI)
                string sDateExempt, sExemptType;
                if (BillingForm.Status == "NEW")
                    sDateExempt = m_sDtOperated;
                else
                    sDateExempt = "01/01/" + BillingForm.TaxYear;
                pSet.Query = "SELECT * FROM boi_table WHERE bin = :1 AND datefrom <= to_date(:2, 'MM/dd/yyyy') AND dateto >= to_date(:3, 'MM/dd/yyyy')";
                pSet.AddParameter(":1", BillingForm.BIN);
                pSet.AddParameter(":2", sDateExempt);
                pSet.AddParameter(":3", sDateExempt);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sExemptType = pSet.GetString("exempt_type");
                        MessageBox.Show("Tax exempt to due " + sExemptType, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        pSet.Close();
                        return true;
                    }
                }

                // (e) ALJ 20110802
                p_sFeesCode = "B";              
                pSet.Query = "SELECT * FROM consol_gr WHERE bin = :1 AND ofc_type = 'BRANCH'";
                pSet.AddParameter(":1", BillingForm.BIN);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Gross Receipts was consolidated to main branch.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return true;
                    }
                }
                if (m_bIsNoSale)
                    return true;

            }

            // RMC 20150806 merged exemption by brgy from Mati (s)
            //MCR 20140929 Exempted by brgy (s)
            pSet.Query = "SELECT * FROM tax_and_fees_exempted_brgy where fees_code = :1 AND brgy_code = :2 AND rev_year = :3";
            pSet.AddParameter(":1", p_sFeesCode);
            pSet.AddParameter(":2", BillingForm.m_sBrgyCode);
            pSet.AddParameter(":3", m_sRevYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();
                    return true;
                }
            }
            pSet.Close();
            //MCR 20140929 Exempted by brgy (e)
            // RMC 20150806 merged exemption by brgy from Mati (e)

            pSet.Query = "SELECT * FROM exempted_bns WHERE fees_code = :1 AND bns_code = :2 AND rev_year = :3";
            pSet.AddParameter(":1", p_sFeesCode);
            //pSet.AddParameter(":2", p_sBnsCode.Substring(0,2));
            pSet.AddParameter(":2", p_sBnsCode);    // RMC 20110414
            pSet.AddParameter(":3", m_sRevYear);

            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close(); // ALJ 20110802
                    return true;
                }
                else
                {
                    if (!BillingForm.MainBusiness && p_sFeesCode != "B")
                    {
                        pSet.Close(); // ALJ 20110802
                        return true;
                    }
                    if (BillingForm.Status == "RET" && p_sFeesCode != "B")
                    {
                        pSet.Close(); // ALJ 20110802
                        return true;
                    }
                    if (m_sDueState == "Q" && p_sFeesCode != "B")
                    {
                        pSet.Close(); // ALJ 20110802
                        return true;
                    }
                    if (m_bPermitUpdateFlag == true && p_sFeesCode != "01" && m_bIsRenPUT == false)
                    {
                        pSet.Close(); // ALJ 20110802
                        return true;
                    }
                    pSet.Close(); // ALJ 20110802
                    return false;
                }
            }
            pSet.Close();
            // Note: Pending Consiladated gross with the same main business code
            return true;
        }

        //MCR 20170529 (s)
        private bool isTAGGED(string sBin, string sTaxYear)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * From REASS_WATCH_LIST where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and is_tag = 1";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    pSet.Close();
                    return true;
                }
            pSet.Close();
            return false;
        }
        //MCR 20170529 (e)

        private void DelBilling()
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "del_billing";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sDueState;
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.Quarter;
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_iSwRE;
            plsqlCmd.AddParameter("p_iSwRE", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();

            }
            plsqlCmd.Close();
        }

        private void InsertDues(string p_sFeesType, string p_sFeesCode, double p_fAmount)
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "insert_dues";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sDueState;
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.Quarter;
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sRevYear;
            plsqlCmd.AddParameter("p_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_iSwRE;
            plsqlCmd.AddParameter("p_iSwRE", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sFeesType;
            plsqlCmd.AddParameter("p_sFeesType", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sFeesCode;
            plsqlCmd.AddParameter("p_sFeesCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_fAmount;
            plsqlCmd.AddParameter("p_fAmount", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();

            }
            plsqlCmd.Close();
        }

        public string UnBilledBusinessType()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBnsCode = "", sBnsDesc = "";
            pSet.Query = "select unbilled_bns_code(:1,:2,:3,:4) as bns_code from dual";
            pSet.AddParameter(":1", BillingForm.BIN);
            pSet.AddParameter(":2", BillingForm.TaxYear);
            pSet.AddParameter(":3", BillingForm.Quarter);
            // RMC 20150325 modified billing for permit update application (s)
            if (m_bPermitUpdateFlag == true)
            {
                int iSw = 9;    // temp switch code for put transaction
                pSet.AddParameter(":4", iSw);
            }// RMC 20150325 modified billing for permit update application (e)
            else
                pSet.AddParameter(":4", m_iSwRE);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sBnsCode = pSet.GetString("bns_code").Trim();
                }
            }
            pSet.Close();
            if (sBnsCode != "")
            {
                sBnsDesc = LibraryClass.GetBnsDesc("B", sBnsCode, m_sRevYear);
            }
            else
            {
                if (!m_bIsBillOK)
                {
                    sBnsDesc = LibraryClass.GetBnsDesc("B", m_sHoldBnsCode, m_sRevYear);
                }
            }

            // RMC 20120111 added validation to accept bns with no computed tax if exempted in billing (s)
            /* if (BnsIsExempted(sBnsCode))
                 sBnsDesc = "";
             // RMC 20120111 added validation to accept bns with no computed tax if exempted in billing (e)
             */
            // rmc 20120118 put rem

            return sBnsDesc;
        }

        protected virtual void ComputeAdjustment()
        {
            // see RevEsxam.cs
        }

        public void CompareTaxes()
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            string sParameter = string.Empty;
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "compare_taxes";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_iSwRE;
            plsqlCmd.AddParameter("p_iSwRE", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            plsqlCmd.ParamValue = AppSettingsManager.SystemUser.UserCode;
            plsqlCmd.AddParameter("p_sUser", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = string.Empty;
            plsqlCmd.AddParameter("o_sParameter", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Output, 100);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
                plsqlCmd.Close();

            }
            else
            {
                sParameter = plsqlCmd.ReturnValue("o_sParameter").ToString();

            }
            plsqlCmd.Close();
            if (sParameter == "LOWER")
            {
                MessageBox.Show("Warning! Previous tax paid is higher than current tax due", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            // (s) ALJ 20110907 re-assessment validation
            else
            {
                if (sParameter == "RE-BILL(LOWER)")
                {
                    MessageBox.Show("Warning! Previous tax assessment is higher than current tax due", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            // (e) ALJ 20110907 re-assessment validation


        }

        /// <summary>
        /// ALJ 20110725 lgu specific fee
        /// Function created to compute fee which computation is specific or unique to LGU
        /// </summary>
        /// <param name="p_sFeesCode"></param>
        /// <returns></returns>
        protected double LGUSpecificFee(string p_sFeesCode, double p_dFeesAmt)
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            double dAmount = 0;
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "lgu_spec_fee";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sFeesCode;
            plsqlCmd.AddParameter("p_sFeesCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_dFeesAmt;
            plsqlCmd.AddParameter("p_fAmount", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = dAmount;
            plsqlCmd.AddParameter("o_fAmount", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Output);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
                plsqlCmd.Close();

            }
            else
            {
                double.TryParse(plsqlCmd.ReturnValue("o_fAmount").ToString(), out dAmount);

            }
            plsqlCmd.Close();
            return dAmount;
        }

        // RMC 20110807 created CancelBilling module (s)
        public virtual void CancelBillUpdateList()
        {

        }
        // RMC 20110807 created CancelBilling module (e)

        /// <summary>
        /// ALJ 20110907 re-assessment validation 
        /// </summary>
        public bool SaveForSOA()
        {
            // RMC 20150528 enabled treas module (s)
            //REM MCR 20141222 LUBAO VERSION
            if (AppSettingsManager.TreasurerModule(BillingForm.BIN))
            {
                MessageBox.Show("Billing is subject for verification.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            // RMC 20150528 enabled treas module (e)

            //if (MessageBox.Show("Proceed to SOA printing?", "Billing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            if (true)   // RMC 20180105 remove too much prompts in billing/soa requested by Malolos ralph
            {
                // (s) ALJ 20111227 save to for SOA Printing
                if (AppSettingsManager.SaveForSoa(BillingForm.BIN, m_sBaseDueYear, AppSettingsManager.SystemUser.UserCode))
                {
                    //MessageBox.Show("Billing is now ready for SOA viewing and printing.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);   // RMC 20180105 remove too much prompts in billing/soa requested by Malolos ralph
                    return true;
                }
                else
                {
                    MessageBox.Show("Save billing first.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

                // (e) ALJ 20111227 save to for SOA Printing

                /* // ALJ 20111227 save to for SOA Printing -- Put remarks, created function in appsetting
                OracleResultSet pCmd = new OracleResultSet();
                pCmd.Query = "DELETE FROM ass_taxdues WHERE bin = :1 and tax_year >= :2 and tax_year <= :3";
                pCmd.AddParameter(":1", BillingForm.BIN);
                pCmd.AddParameter(":2", m_sBaseDueYear);
                pCmd.AddParameter(":3", m_sCurrentYear);
                pCmd.ExecuteNonQuery();
                pCmd.Query = "DELETE FROM ass_bill_gross_info WHERE bin = :1 and tax_year >= :2 and tax_year <= :3";
                pCmd.AddParameter(":1", BillingForm.BIN);
                pCmd.AddParameter(":2", m_sBaseDueYear);
                pCmd.AddParameter(":3", m_sCurrentYear);
                pCmd.ExecuteNonQuery();

                // Note: Saving should be in this sequence: save to ass_taxdues first then ass_bill_gross_info 
                pCmd.Query = "INSERT INTO ass_taxdues SELECT * FROM taxdues a WHERE bin = :1 AND tax_year >= :2 AND tax_year <= :3";
                pCmd.AddParameter(":1", BillingForm.BIN);
                pCmd.AddParameter(":2", m_sBaseDueYear);
                pCmd.AddParameter(":3", m_sCurrentYear);

                // check and prompt user if zero (0) row/s affected
                if (pCmd.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("Save billing first.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }



                pCmd.Query = "INSERT INTO ass_bill_gross_info SELECT a.*, '" + AppSettingsManager.SystemUser.UserCode + "' as usr_code FROM bill_gross_info a WHERE bin = :1 AND tax_year >= :2 AND tax_year <= :3";
                pCmd.AddParameter(":1", BillingForm.BIN);
                pCmd.AddParameter(":2", m_sBaseDueYear);
                pCmd.AddParameter(":3", m_sCurrentYear);
                pCmd.ExecuteNonQuery();


                MessageBox.Show("Billing is now ready for SOA viewing and printing.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;*/
            }
            else
                return false;
        }

        // RMC 20110826 added validation if billing exists (s)
        public virtual bool ValidBilling()
        {
            return true;  
        }
        // RMC 20110826 added validation if billing exists (e)

        public bool BnsIsExempted(string p_sBnsCode)
        {
            // RMC 20120111 added validation to accept bns with no computed tax if exempted in billing
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from exempted_bns where bns_code = '" + p_sBnsCode + "'";
            pSet.Query += " and fees_code = 'B'";   // RMC 20131226
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();
                    return true;
                }
                else
                {
                    pSet.Close();
                    return false;
                }
            }
            return true;
        }

        public bool BnsWithRetirementApp(string p_sBnsCode)
        {
            // RMC 20140708 added retirement billing
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from retired_bns_temp where bns_code_main = '" + p_sBnsCode + "'";
            pSet.Query += " and bin = '" + BillingForm.BIN + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();
                    return true;
                }
                else
                {
                    pSet.Close();
                    return false;
                }
            }
            return true;
        }

        private bool ValidateAddlSched(string p_sBnsCode, string p_sFeesDesc)
        {
            // RMC 20140109 validation for garbage added in other charges table - Mati
            OracleResultSet pRec = new OracleResultSet();
            string sCode = string.Empty;

            //pRec.Query = "select * from tax_and_fees_table where fees_desc like '" + p_sFeesDesc + "%' and fees_type = 'AD'";
            //pRec.Query = "select * from tax_and_fees_table where fees_desc like '" + p_sFeesDesc + "%' and fees_type = 'AD' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118
            pRec.Query = "select * from tax_and_fees_table where fees_desc like '" + StringUtilities.HandleApostrophe(p_sFeesDesc) + "%' and fees_type = 'AD' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'";    // RMC 20150426 QA corrections
            
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sCode = pRec.GetString("fees_code");
                    pRec.Close();

                    //pRec.Query = "select * from tax_and_fees_table where fees_type <> 'AD' and fees_desc like '" + p_sFeesDesc + "%'";
                    //pRec.Query = "select * from tax_and_fees_table where fees_type <> 'AD' and fees_desc like '" + p_sFeesDesc + "%' and and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118
                    //pRec.Query = "select * from tax_and_fees_table where fees_type <> 'AD' and fees_desc like '" + StringUtilities.HandleApostrophe(p_sFeesDesc) + "%' and and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; // RMC 20150426 QA corrections
                    pRec.Query = "select * from tax_and_fees_table where fees_type <> 'AD' and fees_desc like '" + StringUtilities.HandleApostrophe(p_sFeesDesc) + "%' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; // RMC 20160506 fixed missing expression error in Billing

                    if (pRec.Execute())
                    {
                        if (!pRec.Read()) 
                            sCode = "";
                    }
                }
            }
            pRec.Close();

            if (sCode != "")
            {
                pRec.Query = "select * from addl_table where fees_code = '" + sCode + "' and bns_code_main = '" + p_sBnsCode + "' and bin = '" + BillingForm.BIN + "' and amount <> 0";
                if (pRec.Execute())
                {
                    if (!pRec.Read())
                    {
                        pRec.Close();
                        return false;
                    }
                }
            }
            else
                return false;

            return true;

        }

        private bool ValidatePermitUpdateTrans(string p_sBnsCode, string p_sFeesDesc)
        {
            // RMC 20140725 Added Permit update billing
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            string sCode = string.Empty;

            if (BillingForm.SourceClass != "PermitUpdate")
                return false;

            /*if (m_bIsRenPUT)
                return false;*/
            // RMC 20150108

            if (!m_blnNewAddl && !m_blnTransLoc && !m_blnTransOwn && !m_blnChClass && !m_blnChBnsName && !m_blnChOrgKind)
                return false;

            string sTmpFeesDesc = string.Empty;

            int iCnt = 0;
            pRec.Query = "select * from tax_and_fees_table where fees_type = 'AD' and ";
            pRec.Query += "(fees_desc like 'ADDITIONAL%BUSINESS' or fees_desc like '%LOCATION%'";
            pRec.Query += " or fees_desc like '%OWNERSHIP%' or fees_desc like '%CLASS%'";
            pRec.Query += " or fees_desc like '%NATURE%'";
            pRec.Query += " or fees_desc like '%BUSINESS NAME' or fees_desc like '%KIND%')";
            pRec.Query += " and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sCode = pRec.GetString("fees_code");

                    pRec2.Query = "select * from addl_table where fees_code = '" + sCode + "' and bns_code_main = '" + p_sBnsCode + "' and bin = '" + BillingForm.BIN + "' and amount <> 0";
                    if (pRec2.Execute())
                    {
                        if (pRec2.Read()) //pRec.Read() MOD MCR 20150204
                        {
                            iCnt++;
                        }
                    }
                }
            }
            pRec.Close();

            if (sCode != "") // RMC 20150108
            {
                if (iCnt == 0)
                    return false;
            }

            return true;
        }

        protected virtual void UpdateBussCapital()
        {
            // RMC 20150104 update capital when asset size was edited

            OracleResultSet pCmd = new OracleResultSet();
            double dGrCap;
            double.TryParse(BillingForm.Capital, out dGrCap);
            pCmd.Query = "UPDATE business_que SET capital = :1 WHERE bin =:2 AND tax_year = :3";
            pCmd.AddParameter(":1", dGrCap);
            pCmd.AddParameter(":2", BillingForm.BIN);
            pCmd.AddParameter(":3", BillingForm.TaxYear);
            pCmd.ExecuteNonQuery();
        }

        private void GetPrevTaxPaid(string sFeesCode, string sBnsCode, out double p_dAmount)
        {
            // RMC 20150115 migrated modify dues module to cater delinquent years not covered by the current revenue code
            OracleResultSet pSet = new OracleResultSet();

            p_dAmount = 0;
            int iRevYear = 0;
            int iTaxYear = 0;
            string sOrNo = string.Empty;
            string sPrevYear = string.Empty;

            int.TryParse(ConfigurationAttributes.RevYear, out iRevYear);
            int.TryParse(BillingForm.TaxYear, out iTaxYear);

            if (iTaxYear < iRevYear)
            {
                pSet.Query = "select * from pay_hist where bin = '" + BillingForm.BIN + "' and tax_year < '"+ConfigurationAttributes.RevYear+"' order by tax_year desc";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sOrNo = pSet.GetString("or_no");
                        sPrevYear = pSet.GetString("tax_year");
                    }
                }
                pSet.Close();

                if(sFeesCode == "00")
                    pSet.Query = "select * from or_table where or_no = '" + sOrNo + "' and fees_code = 'B' and bns_code_main = '" + sBnsCode + "' and tax_year = '" + sPrevYear + "'";
                else
                    pSet.Query = "select * from or_table where or_no = '" + sOrNo + "' and fees_code = '" + sFeesCode + "' and tax_year = '" + sPrevYear + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        p_dAmount = pSet.GetDouble("fees_due");
                    }
                }
                pSet.Close();
            }
        }

        private string DiscoveryDelinquentStat(string sBnsStat)
        {
            // RMC 20150129
            OracleResultSet pTmp = new OracleResultSet();
            string sStat = "";
            string sTaxYear = "";

            if(BillingForm.NewDiscovery)
            {
                pTmp.Query = "select tax_year from business_que where bin = '" + BillingForm.BIN + "'";
                if(pTmp.Execute())
                {
                    if(pTmp.Read())
                        sTaxYear = pTmp.GetString(0).Trim();
                }
                pTmp.Close();

                //check if year of operation already billed 
                if (sTaxYear != "")
                {
                    pTmp.Query = "select * from taxdues where bin = '" + BillingForm.BIN + "'";
                    pTmp.Query += " and tax_year = '" + sTaxYear + "'";
                    if (pTmp.Execute())
                    {
                        if (pTmp.Read())
                        {
                            sStat = "REN";
                        }
                        else
                            sStat = "NEW";
                    }
                    pTmp.Close();
                }
                else
                    sStat = "NEW";
            }
            else
            {
                sStat = sBnsStat;
            }

            return sStat;
        }

        /// <summary>
        /// @Added by: AST 20150112
        /// For fixing billing issues when prev. tax-year qtr doesn't paid.
        /// This is other validation for Qtrl'y deck for New & Ren
        /// </summary>
        /// <param name="BIN"></param>
        /// <returns></returns>
        private bool ValidateQuarterPaid(string BIN)
        {

            m_strTmpBnsStat = AppSettingsManager.GetBnsStat(BIN);
            if (m_strTmpBnsStat != "NEW")
                return false;

            string strQtrPaid = string.Empty;
            int intQtrPaid = 0;
            int intQtrToBePay = 0;
            int intCurrentYear = AppSettingsManager.GetCurrentDate().Year;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();
            int intCouter = 0;
            int intQtrFromBilling = 0; // holder of qtr from billing_hist.
            int intTaxYearFromBilling = 0; // holder of taxyear from billing_hist.

            // Get the qtr based on OR date.
            string strGetQtr = @"case 
            when qtr_paid = 'F' and (to_char(or_date, 'MM') between '01' and '03') then '1' 
            when qtr_paid = 'F' and (to_char(or_date, 'MM') between '04' and '06') then '2' 
            when qtr_paid = 'F' and (to_char(or_date, 'MM') between '07' and '09') then '3' 
            when qtr_paid = 'F' and (to_char(or_date, 'MM') between '10' and '12') then '4' 
            else Qtr_Paid 
            end as Qtr_Paid ";

            // Get Last Qtr paid.
            // I'd use order by to get the correct last qtr paid.
            result.Query = string.Format("select or_no, bin, bns_stat, tax_year, {0},", strGetQtr);
            result.Query += "or_date, payment_term, data_mode, payment_type, date_posted ";
            result.Query += string.Format("from pay_hist where bin = '{0}' ", BIN);
            result.Query += string.Format("and tax_year <= '{0}' ", intCurrentYear);
            result.Query += "order by tax_year, qtr_paid";
            if (result.Execute())
            {
                // We'll use while loop until found the last record for qtr_paid.
                while (result.Read())
                {
                    strQtrPaid = result.GetString("Qtr_Paid");
                    m_strTmpQtr = result.GetString("Qtr_Paid");
                    m_strTmpTaxYear = result.GetString("Tax_Year");
                    //m_strTmpBnsStat = result.GetString("Bns_Stat");
                }
            }
            result.Close();

            char chrQtrPaid;
            char.TryParse(strQtrPaid, out chrQtrPaid);

            // Check in billing_hist.
            // if Billing Qtr is > pay_hist Qtr then use Qtr from billing.
            // Otherwise use updated Qtr from pay_hist.
            if (char.IsDigit(chrQtrPaid))
            {
                result.Query = string.Format("SELECT Qtr, Tax_Year FROM Bill_Hist WHERE Bin = '{0}' ORDER BY Tax_Year, Qtr", BIN);
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        int.TryParse(result.GetString("Qtr"), out intQtrFromBilling);
                        int.TryParse(result.GetString("Tax_Year"), out intTaxYearFromBilling);
                    }
                }
                result.Close();

                int.TryParse(strQtrPaid, out intQtrPaid);

                if (intTaxYearFromBilling > int.Parse(m_strTmpTaxYear))
                {
                    strQtrPaid = intQtrFromBilling.ToString();
                    m_strTmpQtr = intQtrFromBilling.ToString();
                    m_strTmpTaxYear = intTaxYearFromBilling.ToString();
                    char.TryParse(strQtrPaid, out chrQtrPaid); //Re-call output.
                }
                else if (intTaxYearFromBilling == int.Parse(m_strTmpTaxYear))
                {
                    if (intQtrFromBilling > intQtrPaid)
                    {
                        if (m_strTmpBnsStat == "NEW" && !VerifyifQTRisAlreadyPaid(intQtrFromBilling, BIN))
                        {

                        }
                        else
                        {
                            strQtrPaid = intQtrFromBilling.ToString();
                            m_strTmpQtr = intQtrFromBilling.ToString();
                            m_strTmpTaxYear = intTaxYearFromBilling.ToString();
                            char.TryParse(strQtrPaid, out chrQtrPaid); //Re-call output.
                        }
                    }
                }
            }

            // Check if it has payment/Qtr Paid
            if (!string.IsNullOrEmpty(strQtrPaid) && char.IsDigit(chrQtrPaid))
            {
                int.TryParse(strQtrPaid, out intQtrPaid);

                // Disregarded 4th Qtr.
                if (intQtrPaid == 4)
                    return false;
                // otherwise Continue, it means Qtr 1,2,3.

                intQtrToBePay = intQtrPaid + 1; //Increment by 1 to set the quarter to pay
                m_strTmpQtrToPay = intQtrToBePay.ToString();

                // AST 20150226 Added for Exempted bns. (s)
                string sBnsCode = string.Empty;
                sBnsCode = AppSettingsManager.GetBnsCodeMain(BIN);
                m_sIsQtrly = LibraryClass.IsQtrlyDec(sBnsCode, m_sRevYear);
                if (m_sIsQtrly == "N")
                {
                    return false;
                }
                // AST 20150226 Added for Exempted bns. (e)

                BillingForm.BIN = BIN;
                BillingForm.BusinessCode = "";
                BillingForm.TaxYear = m_strTmpTaxYear;
                BillingForm.Status = m_strTmpBnsStat;
                m_sDueState = m_strTmpBnsStat.Substring(0, 1);
                BillingForm.Quarter = m_strTmpQtrToPay;

                //// Check Taxdues..
                //// I didn't set another parameter because, the system needs to set the correct record to the table taxdues in the other part of the project.
                //result.Query = string.Format("SELECT * FROM Taxdues WHERE Bin = '{0}' AND Tax_Year <= '{1}'", BIN, intCurrentYear);
                //int.TryParse(result.ExecuteScalar(), out intCouter);
                //if (intCouter > 0) // Check if it has record
                //{
                //    if (result.Execute())
                //    {
                //        if (result.Read())
                //        {
                //            // Set Qtr to be pay
                //            rs.Query = string.Format("UPDATE Taxdues SET Qtr_To_Pay = '{0}' WHERE Bin = '{1}'", intQtrToBePay, BIN);
                //            if (rs.ExecuteNonQuery() == 0) { }
                //        }
                //    }
                //    result.Close();
                //}
                //else
                //{ // Get Values on history
                //    result.Query = string.Format("SELECT * FROM Taxdues_Hist WHERE Bin = '{0}' AND Tax_Year <= '{1}'", BIN, intCurrentYear);
                //    if (result.Execute())
                //    {
                //        if (result.Read())
                //        {
                //            //rs.Query = string.Format("DELETE FROM Taxdues WHERE Bin = '{0}'", BIN);
                //            //if (rs.ExecuteNonQuery() == 0) { }

                //            rs.Query = string.Format("INSERT INTO Taxdues VALUES({0})", result.Query);
                //            if (rs.ExecuteNonQuery() == 0) { }

                //            // Set Qtr to be pay
                //            rs.Query = string.Format("UPDATE Taxdues SET Qtr_To_Pay = '{0}' WHERE Bin = '{1}'", intQtrToBePay, BIN);
                //            if (rs.ExecuteNonQuery() == 0) { }
                //        }
                //    }
                //    result.Close();
                //}

                // return true because, it will prevent the user to start renewal and prompt-
                // the user to pay the remaining dues in prev qtrs.
                return true;
            }




            return false;
        }

        private bool VerifyifQTRisAlreadyPaid(int p_intQtrFromBilling, string p_sBin) //MCR 20150729
        {
            // RMC 20150817 corrections in application and billing of delinquent business, merged mods fr Rodriguez
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select qtr_paid from pay_hist where bin = '" + p_sBin + "' and qtr_paid = '" + p_intQtrFromBilling + "'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    pSet.Close();
                    return true;
                }
            pSet.Close();

            return false;
        }

        private bool ValidateLguSpecificTagging(string p_sBin, string sTagCode)
        {
            // RMC 20171128 added validation in Billing if with ECC, should unbill CNC
            // customized for Malolos
             OracleResultSet pSet = new OracleResultSet();

            if (sTagCode == "ECC")
            {
                pSet.Query = "select * from ecc_tagging where bin = '" + p_sBin + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        // if with ECC, should not bill CNC

                        pSet.Close();

                        int iCnt = 0;
                        pSet.Query = "select count(*) from addl_table where bin = '" + p_sBin + "' and amount > 0 and fees_code in ";
                        pSet.Query += "(select fees_code from tax_and_fees_table where fees_type = 'AD' and fees_desc = 'CNC')";
                        int.TryParse(pSet.ExecuteScalar(), out iCnt);

                        if (iCnt > 0)
                        {
                            return false;
                        }
                    }
                }
                pSet.Close();
                
            }
            return true;

        }

    }

}
