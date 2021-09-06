using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using System.Windows.Forms;
using System.Data;

namespace Amellar.BPLS.Billing
{
    class Retirement:Billing
    {
        public Retirement(frmBilling Form)
            : base(Form)
        {
        }
        public bool isSaved = false;
        public override void ReturnValues()
        {
            OracleResultSet pSet = new OracleResultSet();
            bool bIsBque = false;
            
            if (!ValidApplication())
                return;

            pSet.Query = "select * from business_que where bin = '" + BillingForm.BIN + "' and bns_stat= 'RET'";
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {
                    MessageBox.Show("No retirement application found.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pSet.Close();
                    return;
                }
                else
                {
                    bIsBque = true;

                    m_sBaseDueQtr = ""; // initialize
                    m_sMaxDueQtr = ""; // initialize
                    

                    m_sDueState = "X";
                    m_sMainBnsStat = pSet.GetString("bns_stat"); 
                    BillingForm.Quarter = "1";
                    BillingForm.BillFlag = false;
                    BillingForm.TaxYear = pSet.GetString("tax_year");
                    BillingForm.BusinessName = pSet.GetString("bns_nm").Trim();
                    BillingForm.BusinessAddress = AppSettingsManager.GetBnsAddress(BillingForm.BIN); //MCR 20150406
                    m_sOwnCode = pSet.GetString("own_code").Trim();
                    BillingForm.OwnersName = LibraryClass.GetOwnerName(2, m_sOwnCode);
                    m_sMainBnsCode = pSet.GetString("bns_code").Trim();
                    m_fMainCapital = pSet.GetDouble("capital");
                    m_fMainGross = pSet.GetDouble("gr_1");
                    BillingForm.Status = m_sMainBnsStat;
                    BillingForm.BusinessCode = m_sMainBnsCode;
                    BillingForm.Capital = string.Format("{0:#,##0.00}", m_fMainCapital);
                    BillingForm.Gross = string.Format("{0:#,##0.00}", m_fMainGross);
                    GetMinMaxDueYear(BillingForm.BIN, out m_sBaseDueYear, out m_sMaxDueYear);
                    // RMC 20150112 mods in retirement billing (s)
                    if (m_sBaseDueYear == "")
                        m_sBaseDueYear = BillingForm.TaxYear; // defalut value
                    else
                        BillingForm.TaxYear = m_sBaseDueYear;   

                    if (m_sMaxDueYear == "")
                        m_sMaxDueYear = BillingForm.TaxYear; // defalut value
                    else
                        BillingForm.TaxYear = m_sMaxDueYear;
                    // RMC 20150112 mods in retirement billing (e)

                    GetMinMaxDueQtr(BillingForm.BIN, m_sBaseDueYear, out m_sBaseDueQtr, out m_sMaxDueQtr);
                    //m_sCurrentYear = m_sMaxDueYear;   // RMC 20170116 correction in RET billing, button save not enabled
                    GetClosureYear(pSet.GetString("tax_year")); // RMC 20150112 mods in retirement billing

                    // RMC 20170116 correction in RET billing, button save not enabled (s)
                    if(Convert.ToInt32(m_sCurrentYear) < Convert.ToInt32(m_sMaxDueYear))
                       m_sCurrentYear = m_sMaxDueYear;
                    // RMC 20170116 correction in RET billing, button save not enabled (e)

                    PopulateBnsType();
                    PopulateGrossInfo(BillingForm.Status, m_sDueState, BillingForm.TaxYear, BillingForm.Quarter, BillingForm.TaxYear, BillingForm.Quarter, "RET"); // insert initial values for bill_gross_info table // ALJ 20121212 add p_sModule to identify regular billing of rev-exam (fixed bug in wrong gross basis for tax computation)
                    LoadAddlInfo();
                    LoadGrossCapital();
                    AddlInfoReadOnly(true);
                    m_bIsInitLoad = false;
                    BillingForm.EnabledControls(true);
                    BillingForm.TaxYearTextBox.ReadOnly = false;
                }
            }
            pSet.Close();

            m_bIsInitLoad = false;
            // MCR 20150122 (s)
            if (Convert.ToInt32(BillingForm.TaxYear) < Convert.ToInt32(ConfigurationAttributes.RevYear) && isSaved == false)
            {
                frmModifyDues formModify = new frmModifyDues();
                formModify.BIN = BillingForm.BIN;
                if (BillingForm.SourceClass == "RetirementBilling")
                    formModify.BnsStat = "X";
                formModify.TaxYear = BillingForm.TaxYear;
                formModify.ShowDialog();
                if (formModify.DataSaved)
                {
                    isSaved = formModify.DataSaved;
                    ReturnValues();
                }
                else
                    BillingForm.Close();
            }
            // MCR 20150122 (e)
        }

        private bool ValidApplication()
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from retired_bns_temp  where bin = '" + BillingForm.BIN + "'";
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {
                    MessageBox.Show("No retirement application found.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pSet.Close();
                    return false;
                }
            }
            pSet.Close();

            pSet.Query = "select * from permit_update_appl where bin = '" + BillingForm.BIN + "' and data_mode = 'QUE'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("Cannot proceed. Record has Permit Update Application.\nProcced to Permit Update Transaction Billing Module", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pSet.Close();
                    return false;
                }
            }
            pSet.Close();

            return true;
        }

        private void GetClosureYear(string sTaxYear)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sCloseYear = "0";
            string sMaxPaidYear = "0";
            int iMaxPaidYear = 0;

            pSet.Query = "select * from retired_bns_temp where bin = '" + BillingForm.BIN + "' and tax_year >= '" + sTaxYear + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sCloseYear = string.Format("{0:yyyy}", pSet.GetDateTime("APPRVD_DATE"));
                }
            }
            pSet.Close();

            pSet.Query = "select max(tax_year) from pay_hist where bin = '" + BillingForm.BIN + "' and data_mode <> 'UNP'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sMaxPaidYear = pSet.GetString(0);

                    if (sMaxPaidYear.Trim() == "")  // RMC 20161213 include retirement billing in cancellation of billing (s)
                        sMaxPaidYear = "0";
                    else// RMC 20161213 include retirement billing in cancellation of billing (e)
                    {
                        int.TryParse(sMaxPaidYear, out iMaxPaidYear);
                        iMaxPaidYear = iMaxPaidYear + 1;
                        sMaxPaidYear = string.Format("{0:####}", iMaxPaidYear);
                    }
                }
            }
            pSet.Close();

            if (Convert.ToInt32(sCloseYear) >= Convert.ToInt32(sMaxPaidYear)) // < MCR 20150331 Retirement SOA not Enabled
            {
                if (sMaxPaidYear != "0")    // RMC 20161213 include retirement billing in cancellation of billing
                    sCloseYear = sMaxPaidYear;
            }

            m_sCurrentYear = sCloseYear;
        }
    }
}
