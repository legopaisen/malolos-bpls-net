//#######################################
// Modifications:          
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
        private string m_sDtOperated;
        private string m_sPUTApplType;
        private string m_sGrossParam;
        private bool m_bPermitUpdateFlag;
        private bool m_bIsRenPUT;
        private bool m_bIsNoSale;
        private bool m_bIsReg;
        private bool m_bIsBillOK = true;
        private bool m_bIsBillCreated = false;
        protected bool m_bIsInitLoad = true;
        protected string m_sBaseDueYear, m_sBaseDueQtr, m_sMaxDueYear, m_sMaxDueQtr;
        private string m_sHoldTaxYear = string.Empty;
        private string m_sHoldQtr = string.Empty;
        private string m_sHoldBnsCode = string.Empty;

        //public bool IsBillOK { get { return m_bIsBillOK; } }
        public bool IsBillCreated { get { return m_bIsBillCreated; } }
        public string BaseTaxYear{ get { return m_sBaseDueYear; } }
        public string BaseDueQtr{ get { return m_sBaseDueQtr; } }
        public string MaxDueQtr { get { return m_sMaxDueQtr; } } // ALJ 20110309
        public string IsQtrly { get { return m_sIsQtrly; } }
        public string CurrentYear { get { return m_sCurrentYear; } }
        public string CurrentQtr { get { return m_sCurrentQtr; } }
        public bool IsInitialLoad { get { return m_bIsInitLoad; } }
        public int IsRevExam { get { return m_iSwRE; } }
        
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

            if (!ValidApplication())
                return;
            pSet.Query = "SELECT * FROM business_que WHERE bin = :1";
            pSet.AddParameter(":1", BillingForm.BIN);
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {  
                    bIsBque = false;
                }
                else
                {
                    bIsBque = true;
                }
             }
             if (!bIsBque)
             {
                 pSet.Query = "SELECT * FROM businesses WHERE bin = :1";
                 pSet.AddParameter(":1", BillingForm.BIN);
                 if (pSet.Execute())
                 {
                     if (pSet.Read())
                     {
                         OracleResultSet pSetPay = new OracleResultSet();
                         pSetPay.Query = "SELECT DISTINCT(bin) FROM pay_hist WHERE bin = :1 AND data_mode <> 'UNP'";
                         pSetPay.AddParameter(":1",BillingForm.BIN);
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
            m_sOwnCode = pSet.GetString("own_code").Trim();
            BillingForm.OwnersName = LibraryClass.GetOwnerName(2, m_sOwnCode);

            m_sMainBnsCode = pSet.GetString("bns_code").Trim();
            m_sMainBnsStat = pSet.GetString("bns_stat");
            m_fMainCapital = pSet.GetDouble("capital");
            m_fMainGross = pSet.GetDouble("gr_1");
            BillingForm.Status = m_sMainBnsStat;
            BillingForm.BusinessCode = m_sMainBnsCode;
            BillingForm.Capital = string.Format("{0:#,##0.00}", m_fMainCapital);
            BillingForm.Gross = string.Format("{0:#,##0.00}", m_fMainGross);

            m_sDtOperated = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("dt_operated"));
            if (BillingForm.Status == "NEW")
            {
                BillingForm.Quarter = LibraryClass.GetQtr(pSet.GetDateTime("dt_operated"));
                m_sDueState = "N";// temporary
            }
            else
            {
                BillingForm.Quarter = "1";
                m_sDueState = "R";// temporary 
            }
            m_sPUTApplType = string.Empty;

            int.TryParse(BillingForm.TaxYear, out iTaxYear);
            int.TryParse(BillingForm.Quarter, out iQtr);
            int.TryParse(m_sCurrentYear, out iCurrentYear);
            int.TryParse(m_sCurrentQtr, out iCurrentQtr);



            // get then base due year and maximum due year
            m_sBaseDueYear = ""; // initialize
            m_sMaxDueYear = ""; // initialize
            GetMinMaxDueYear(BillingForm.BIN, out m_sBaseDueYear, out m_sMaxDueYear);
            if (m_sBaseDueYear == "")
                m_sBaseDueYear = BillingForm.TaxYear; // defalut value
            if (m_sMaxDueYear == "")
                m_sMaxDueYear = BillingForm.TaxYear; // defalut value
            int.TryParse(m_sMaxDueYear, out iMaxDueYear);
            //

            // Get the base due qtr and maximun due qtr
            // (s) ALJ 20100709 QTR DEC
            m_sBaseDueQtr = ""; // initialize
            m_sMaxDueQtr = ""; // initialize
            GetMinMaxDueQtr(BillingForm.BIN, m_sBaseDueYear, out m_sBaseDueQtr, out m_sMaxDueQtr);
            
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
                        BillingForm.Quarter = sMaxQtr;
                        if (m_sBaseDueQtr == string.Empty && sMaxQtr != "4")
                        {
                            int.TryParse(sMaxQtr, out iMaxDueQtr);
                            m_sBaseDueQtr = string.Format("{0:0}", iMaxDueQtr + 1);
                            // /*test*/ bHasDueQtr = true; // ALJ 20110321

                        }
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
            }
            int.TryParse(m_sBaseDueYear, out iBaseDueYear); // ??         

            
            if (iMaxDueYear >= iTaxYear)
            {
                BillingForm.TaxYear = m_sMaxDueYear;
                BillingForm.Quarter = m_sMaxDueQtr;
            }            
            
            
            PopulateBnsType();
            PopulateGrossInfo(BillingForm.Status, m_sDueState, m_sMaxDueYear, m_sMaxDueQtr, BillingForm.TaxYear, BillingForm.Quarter); // insert initial values for bill_gross_info table
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
                            BillingForm.Quarter = string.Format("{0:0}", iMaxDueQtr + 1);
                            m_sDueState = "Q";
                        }
                    // /*test*/ }

                }

                else
                {
                    if (iMaxDueYear < iCurrentYear) // ALJ 20110309
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
                PopulateGrossInfo(BillingForm.Status, m_sDueState, m_sMaxDueYear, m_sMaxDueQtr, BillingForm.TaxYear, BillingForm.Quarter); // insert initial values for bill_gross_info table
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
            int iTaxYear, iCurrentYear, iQtr, iCurrentQtr; // ALJ 20110309 iCurrentQtr
            sQtr = "1";
            pSet.Query = "SELECT * FROM business_que WHERE bin = :1";
            pSet.AddParameter(":1", BillingForm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    
                    sTaxYear = pSet.GetString("tax_year");
                    sBnsCode = pSet.GetString("bns_code").Trim();
                    m_sIsQtrly = LibraryClass.IsQtrlyDec(sBnsCode, m_sRevYear);
                    int.TryParse(sTaxYear, out iTaxYear);
                    int.TryParse(m_sCurrentYear, out iCurrentYear);
                    int.TryParse(m_sCurrentQtr, out iCurrentQtr); // ALJ 20110309
                    if (iTaxYear < iCurrentYear)
                    {
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
                    }
                    else
                    {
                        pSet.Close();
                        return true;
                    }

                }

                OracleResultSet pSetBuss = new OracleResultSet();
                string  sBnsStat;
                
                pSetBuss.Query = "SELECT * FROM businesses WHERE bin = :1 and bns_stat <> 'RET'";
                pSetBuss.AddParameter(":1", BillingForm.BIN);
                if (pSetBuss.Execute())
                {
                    if (pSetBuss.Read())
                    {
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

                        if ((iTaxYear + 1) < iCurrentYear) // with delinquent year/s || // with multiple years billing created
                        {
                            
                            if ((iTmpTaxduesTY + 1) < iCurrentYear)
                            {
                                pSetBuss.Close();
                                return true; // proceed to billing no need to insert data in business_que 
                            }
                        }

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

                            if (MessageBox.Show("Apply for renewal?", "Billing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                            }
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

        protected void PopulateGrossInfo(string p_sBnsStat, string p_sDueState, string p_sBaseYear, string p_sBaseQtr, string p_sPopYear, string p_sPopQtr)
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
            pSet.Query = "SELECT MIN(tax_year) AS min_year, MAX(tax_year) AS max_year FROM taxdues WHERE bin = :1";
            pSet.AddParameter(":1", p_sBIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    o_sMinDueYear = pSet.GetString("min_year");
                    o_sMaxDueYear = pSet.GetString("max_year");
                }
            }
            pSet.Close();
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
                        if (BillingForm.Status == "NEW" && m_sIsQtrly == "Y")
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
                    if (BillingForm.Status == "NEW" && m_sIsQtrly == "Y")
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
            dataTable.Columns.Add("bns_code",typeof(String));
            dataTable.Columns.Add("bns_desc", typeof(String));

            //BillingForm.cmbBnsType.Items.Clear();
            //BillingForm.BusinessTypes.Items.Clear();
            dataTable.Rows.Add(new String[] { m_sMainBnsCode, LibraryClass.GetBnsDesc("B", m_sMainBnsCode, m_sRevYear)});       
            
            pSet.Query = "SELECT bns_code_main FROM addl_bns WHERE bin = :1 AND tax_year = :2 and qtr = :3";
            pSet.AddParameter(":1", BillingForm.BIN);
            pSet.AddParameter(":2", BillingForm.TaxYear);
            pSet.AddParameter(":3", BillingForm.Quarter);

            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sBnsCode = pSet.GetString("bns_code_main").Trim();
                    dataTable.Rows.Add(new String[] { sBnsCode, LibraryClass.GetBnsDesc("B", sBnsCode, m_sRevYear) });
                }
            }
            pSet.Close();        

            BillingForm.BusinessTypes.DataSource = dataTable;
            BillingForm.BusinessTypes.DisplayMember = "bns_desc";
            BillingForm.BusinessTypes.ValueMember = "bns_code";
            BillingForm.BusinessTypes.SelectedIndex = 0;
            BillingForm.MainBusiness = true;
            BillingForm.BusinessCode = BillingForm.BusinessTypes.SelectedValue.ToString();
            
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
                    BillingForm.Status = pSet.GetString("bns_stat");
                    BillingForm.Capital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
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
                        m_sGrossParam = BillingForm.AdjGross;
                    //////////
                    BillingForm.VATGross = string.Format("{0:#,##0.00}", pSet.GetDouble("vat_gross"));
                    m_sDueState = pSet.GetString("due_state");
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
                            BillingForm.Status = pSet.GetString("bns_stat");
                            BillingForm.Capital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
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
                                m_sGrossParam = BillingForm.AdjGross;
                            //////////
                            BillingForm.VATGross = string.Format("{0:#,##0.00}", pSet.GetDouble("vat_gross"));
                            m_sDueState = pSet.GetString("due_state");

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
                                    BillingForm.Status = pSet.GetString("bns_stat");
                                    BillingForm.Capital = string.Format("{0:#,##0.00}", pSet.GetDouble("capital"));
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
                                        m_sGrossParam = BillingForm.AdjGross;
                                    //////////
                                    BillingForm.VATGross = string.Format("{0:#,##0.00}", pSet.GetDouble("vat_gross"));
                                    m_sDueState = pSet.GetString("due_state");

                                    BillingForm.Quarter = "1";
                                    BillingForm.QuarterTextBox.ReadOnly = true;
                                }
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
            UpdateBillGrossInfo();
            UpdateBussGross();
        }

        public void ReLoadAddlInfo()
        {
            LoadAddlInfo();
            LoadGrossCapital();
            AddlInfoReadOnly(true);
            
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
                sDataType    = BillingForm.AdditionalInformation.Rows[i].Cells[2].Value.ToString();
                sUnit        = BillingForm.AdditionalInformation.Rows[i].Cells[3].Value.ToString();
                dataTable.Rows.Add(sDefaultCode, sDefaultDesc, sDataType, sUnit);
                if (ResetTaxAndFees(sDefaultCode, sUnit) == 1)
                    bIsReset = true;
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
                m_sGrossParam = BillingForm.AdjGross;
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
            InitBillTable();
            UpdateList();
            BillingForm.BillFlag = true;
        }

        private void InitBillTable()
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            double dGross, dCapital, dTotal, dGrandTotal;
            double.TryParse(m_sGrossParam, out dGross);
            double.TryParse(BillingForm.Capital, out dCapital);
            string sBillScope;
            dTotal = 0.00;
            dGrandTotal = 0.00;
            if (BillingForm.MainBusiness)
                sBillScope = "MAIN";
            else
                sBillScope = "ADDL";
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "init_bill_table";
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
            plsqlCmd.ParamValue = 0; // temporary hardcoded
            plsqlCmd.AddParameter("p_bPermitUpdateFlag", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            plsqlCmd.ParamValue = 0; // temporary hardcoded
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
            pSet.AddParameter(":1",BillingForm.BIN);
            pSet.AddParameter(":2", BillingForm.BusinessCode);
            if (pSet.Execute())
            {
                while(pSet.Read())
                {
                    sFeesDesc = pSet.GetString("fees_desc").Trim();
                    fAmount = pSet.GetDouble("amount");
                    sFeesCode = pSet.GetString("fees_code");
                    if (fAmount > 0)
                        BillingForm.TaxAndFees.Rows.Add(true,sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "TF");
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
                    if (fAmount > 0)
                        BillingForm.TaxAndFees.Rows.Add(true, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "AF");
                    else
                        BillingForm.TaxAndFees.Rows.Add(false, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "AF");
                }
            }
            pSet.Close();

        }

        public double BillBtax()
        {
            OracleResultSet pSet = new OracleResultSet();
            double dConvert = 0;
            double dBtaxAmt = 0;
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
        }

        public double BillFees(string p_sFeesCode, string p_sFeesDesc, string p_sFeesType)
        {
            double dFeesAmt, dGross, dCapital;
            double.TryParse(m_sGrossParam, out dGross);
            double.TryParse(BillingForm.Capital, out dCapital);
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
            BillFeeForm.TaxYear = BillingForm.TaxYear;
            BillFeeForm.Quarter = BillingForm.Quarter;
            BillFeeForm.RevisionYear = m_sRevYear;
            BillFeeForm.ShowDialog();
            dFeesAmt = BillFeeForm.TotalDue;
            return dFeesAmt;
        }

        public void UpdateBillTable(double p_fAmount, string p_sType, string p_sFeesCode)
        {
            OracleResultSet pCmd = new OracleResultSet();
            pCmd.Query = "UPDATE bill_table SET amount = :1 WHERE bin = :2 and bns_code_main = :3 AND fees_code = :4 AND fees_type = :5";
            pCmd.AddParameter(":1", p_fAmount);
            pCmd.AddParameter(":2", BillingForm.BIN);
            pCmd.AddParameter(":3", BillingForm.BusinessCode);
            pCmd.AddParameter(":4", p_sFeesCode);
            pCmd.AddParameter(":5", p_sType);
            pCmd.ExecuteNonQuery();
            pCmd.Close();
        }

        public double UpdateFireTax()
        {
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

        public void Save()
        {
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
                if (fAmount > 0 || BnsIsExempted(sFeesCode, BillingForm.BusinessCode) || sFeesType == "AF" || sFeesCode == "16") // AJ 20090115 || sFeesType == 'AF' // GDE 20101110 temporary add code 16 for testing only
                {
                    bBillOK = true;
                }
                else
                {
                    bBillOK = false;                    
                    break;
                }
            }
            if (!bBillOK)
            {
                MessageBox.Show(sFeesDesc + " is required.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else // Saving
            {
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
                    pCmd.AddParameter(":1",  BillingForm.BIN);
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
                    pCmd.Query = "insert into addl_bns values (:1,:2,:3,:4,:5,:6,:7)";
                    pCmd.AddParameter(":1", BillingForm.BIN);
                    pCmd.AddParameter(":2", BillingForm.BusinessCode);
                    pCmd.AddParameter(":3", 0);
                    double.TryParse(BillingForm.Gross, out dGross);
                    pCmd.AddParameter(":4", dGross);
                    pCmd.AddParameter(":5", BillingForm.TaxYear);
                    pCmd.AddParameter(":6", BillingForm.Status);
                    pCmd.AddParameter(":7", BillingForm.Quarter);
                    pCmd.ExecuteNonQuery();
                }
                
                for (i = 0; i < iRows; ++i)
                {
                    sFeesCode = BillingForm.TaxAndFees.Rows[i].Cells[3].Value.ToString();
                    sFeesType = BillingForm.TaxAndFees.Rows[i].Cells[4].Value.ToString();
                    double.TryParse(BillingForm.TaxAndFees.Rows[i].Cells[2].Value.ToString(), out fAmount);
                    if (fAmount > 0)
                    {
                        InsertDues(sFeesType, sFeesCode, fAmount);
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
                        sBillNo = sToday.Substring(0,2) + sToday.Substring(6,4) +"-"+ string.Format("{0:00000}",iBillSerial);
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

                if (m_iSwRE == 1 || m_iSwRE == 2)
                    LibraryClass.AuditTrail("AIB-MS-RE", "multi-table", BillingForm.BIN + " TY" + BillingForm.TaxYear);
                else
                    LibraryClass.AuditTrail("AIB-MS", "multi-table", BillingForm.BIN + " TY" + BillingForm.TaxYear);
                MessageBox.Show("Billing created.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ComputeAdjustment();  
                m_bIsBillCreated = true;
                if (m_bIsBillOK == false && BillingForm.TaxYear == m_sHoldTaxYear && BillingForm.Quarter == m_sHoldQtr && BillingForm.BusinessCode == m_sHoldBnsCode)
                {
                    m_bIsBillOK = true;
                }
            }

            pCmd.Close();
            pSet.Close();
        }

        private bool BnsIsExempted(string p_sFeesCode, string p_sBnsCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            if (p_sFeesCode == "00")
            {
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
            pSet.Query = "SELECT * FROM exempted_bns WHERE fees_code = :1 AND bns_code = :2 AND rev_year = :3";
            pSet.AddParameter(":1", p_sFeesCode);
            //pSet.AddParameter(":2", p_sBnsCode.Substring(0,2));
            pSet.AddParameter(":2", p_sBnsCode);    // RMC 20110414
            pSet.AddParameter(":3", m_sRevYear);

            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    return true;
                }
                else
                {
                    if (!BillingForm.MainBusiness && p_sFeesCode != "B") return true;
                    if (BillingForm.Status == "RET" && p_sFeesCode != "B") return true;
                    if (m_sDueState == "Q" && p_sFeesCode != "B") return true;
                    if (m_bPermitUpdateFlag == true && p_sFeesCode != "01" && m_bIsRenPUT == false) return true;
                    return false;
                }
            }
            pSet.Close();
            // Note: Pending Consiladated gross with the same main business code
            return true;
        }

 

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
            plsqlCmd.AddParameter("o_sParameter", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Output,100);
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
        }

    }



}
