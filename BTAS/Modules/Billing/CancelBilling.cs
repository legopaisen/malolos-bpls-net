//#######################################
// Modifications: 
// ALJ 20130116 revert gross/capital of businesses/hist, addl_bns 
// ALJ 20121212 add p_sModule to identify regular billing of rev-exam (fixed bug in wrong gross basis for tax computation)
// RMC 20120117 added deletion of rev-exam billing in Cancel Billing transaction
// RMC 20120117 delete record in gross_monitoring when billing was cancelled
// RMC 20120113 delete record in treasurers module when billing was cancelled
// RMC 20120112 added end-tasking in cancel billing 
// ALJ 20110907 re-assessment validation
// RMC 20110826 added validation if billing exists
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
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;

namespace Amellar.BPLS.Billing
{
    /// <summary>
    /// RMC 20110807 created CancelBilling module
    /// 
    /// </summary>
    class CancelBilling : Billing
    {
        public CancelBilling(frmBilling Form)
            : base(Form)
        {
        }

        /*public override void ReturnValues()
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = string.Format("select * from business_que where bin = '{0}'", BillingForm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from business_que where bin = '{0}'", BillingForm.BIN);
                }
                else
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from businesses where bin = '{0}'", BillingForm.BIN);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            pSet.Close();
                            pSet.Query = string.Format("select * from businesses where bin = '{0}'", BillingForm.BIN);
                        }
                        else
                        {
                            pSet.Close();
                            MessageBox.Show("Record not found.");
                            return;
                        }
                    }
                }
            }
            
            
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sMainBnsCode = pSet.GetString("bns_code").Trim();
                    m_sMainBnsStat = pSet.GetString("bns_stat");
                    m_fMainCapital = pSet.GetDouble("capital");
                    m_fMainGross = pSet.GetDouble("gr_1");

                    BillingForm.TaxYear = pSet.GetString("tax_year");
                    BillingForm.BusinessName = pSet.GetString("bns_nm").Trim();
                    m_sOwnCode = pSet.GetString("own_code").Trim();
                    BillingForm.OwnersName = LibraryClass.GetOwnerName(2, m_sOwnCode);
                    BillingForm.Status = m_sMainBnsStat;
                    BillingForm.BusinessCode = m_sMainBnsCode;
                    BillingForm.Capital = string.Format("{0:#,##0.00}", m_fMainCapital);
                    BillingForm.Gross = string.Format("{0:#,##0.00}", m_fMainGross);

                    //m_sDtOperated = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("dt_operated"));
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
                }
            }
            pSet.Close();

            UpdateList();
        }*/

        private bool bAdjustmentCancel = false; //JARS 20181004

        public override void CancelBillUpdateList()
        {
            // RMC 20160512 correction in cancel billing (s)
            if (m_sBaseDueYear == "" && m_sMaxDueYear == "")
                return;
            // RMC 20160512 correction in cancel billing (e)

            OracleResultSet pSet = new OracleResultSet();
            double fAmount;
            string sFeesDesc;
            string sFeesCode;
            BillingForm.btnEditAddlInfo.Enabled = false;
            BillingForm.btnRetrieveBilling.Enabled = false;
            BillingForm.btnSave.Enabled = true;

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
                    if (fAmount > 0)
                        BillingForm.TaxAndFees.Rows.Add(true, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "AF");
                    else
                        BillingForm.TaxAndFees.Rows.Add(false, sFeesDesc, string.Format("{0:#,##0.00}", fAmount), sFeesCode, "AF");
                }
            }
            pSet.Close();
        }

        public override void Save()
        {
            OracleResultSet pSet = new OracleResultSet();

            string strBin;
            string strYear;
            bAdjustmentCancel = false;
            strBin = BillingForm.BIN;
            //strYear = BillingForm.TaxYear; // ALJ 20110907 re-assessment validation put "//"
            strYear = m_sBaseDueYear; // ALJ 20110907 re-assessment validation

            if (MessageBox.Show("Cancel this billing?", "CANCEL BILLING", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CancelRevExamBilling(strBin);   // RMC 20120117 added deletion of rev-exam billing in Cancel Billing transaction
                RevertDuesAmount(strBin, strYear);//JHB 20181203 megere from LAL-LO JARS 20181121

                if (!bAdjustmentCancel) //JARS 20181004
                {
                    // (s) ALJ 20110907 re-assessment validation
                    pSet.Query = string.Format("delete from ass_bill_gross_info where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                    //pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr = '1'))";   // RMC 20160517 revised cancel billing
                    pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr = '1') or due_state = 'X')";    // RMC 20161213 include retirement billing in cancellation of billing
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from ass_taxdues where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                    //pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr_to_pay = '1'))";    // RMC 20160517 revised cancel billing
                    pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr_to_pay = '1') or due_state = 'X')"; // RMC 20161213 include retirement billing in cancellation of billing     
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    pSet.Query = string.Format("delete from reass_bill_gross_info where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                    //pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr = '1'))";   // RMC 20160517 revised cancel billing
                    pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr = '1') or due_state = 'X')";   // RMC 20161213 include retirement billing in cancellation of billing
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    pSet.Query = string.Format("delete from reass_taxdues where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                    //pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr_to_pay = '1'))";    // RMC 20160517 revised cancel billing
                    pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr_to_pay = '1') or due_state = 'X')";    // RMC 20161213 include retirement billing in cancellation of billing
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    // (e) ALJ 20110907 re-assessment validation


                    pSet.Query = string.Format("delete from taxdues where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">=" 
                    //pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr_to_pay = '1'))";    // RMC 20160517 revised cancel billing
                    pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr_to_pay = '1') or due_state = 'X')";    // RMC 20161213 include retirement billing in cancellation of billing
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from taxdues_hist where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">="
                    //pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr_to_pay = '1'))";    // RMC 20160517 revised cancel billing
                    pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr_to_pay = '1') or due_state = 'X')";    // RMC 20161213 include retirement billing in cancellation of billing
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_no where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">="
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_hist where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">="
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from btax where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">="
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from tax_and_fees where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">="
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }


                    pSet.Query = string.Format("delete from addl where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">="
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from fire_tax where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">="
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from reg_table where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);  // ALJ 20110907 re-assessment validation- put ">="
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">="
                    //pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr = '1'))";   // RMC 20160517 revised cancel billing
                    pSet.Query += " and ((due_state = 'P') or (due_state = 'N') or (due_state = 'R' and qtr = '1') or due_state = 'X')";   // RMC 20161213 include retirement billing in cancellation of billing
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    // RMC 20120113 delete record in treasurers module when billing was cancelled (s)
                    pSet.Query = string.Format("delete from treasurers_module where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from treasurers_module_tmp where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    // RMC 20120113 delete record in treasurers module when billing was cancelled (e)

                    // RMC 20120117 delete record in gross_monitoring when billing was cancelled (s)
                    pSet.Query = string.Format("delete from gross_monitoring where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    // RMC 20120117 delete record in gross_monitoring when billing was cancelled (e)


                    // RMC 20160517 additional clean-up in cancel billing (s)
                    OracleResultSet pTmp = new OracleResultSet();
                    string sTmpTaxYear = string.Empty;
                    string sTmpQtrToPay = string.Empty;

                    pTmp.Query = "select * from taxdues where bin = '" + strBin + "' and tax_year not in ";
                    pTmp.Query += " (select tax_year from pay_hist where bin = '" + strBin + "') ";
                    pTmp.Query += " and due_state = 'R' and qtr_to_pay > '1' ";
                    if (pTmp.Execute())
                    {
                        if (pTmp.Read())
                        {
                            sTmpTaxYear = pTmp.GetString("tax_year");
                            sTmpQtrToPay = pTmp.GetString("qtr_to_pay");

                            pSet.Query = "delete from ass_bill_gross_info where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            pSet.Query += " and due_state = 'R' and qtr = '" + sTmpQtrToPay + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from ass_taxdues where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            pSet.Query += " and due_state = 'R' and qtr_to_pay = '" + sTmpQtrToPay + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from reass_bill_gross_info where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            pSet.Query += " and due_state = 'R' and qtr = '" + sTmpQtrToPay + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from reass_taxdues where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            pSet.Query += " and due_state = 'R' and qtr_to_pay = '" + sTmpQtrToPay + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from taxdues where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            pSet.Query += " and due_state = 'R' and qtr_to_pay = '" + sTmpQtrToPay + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from taxdues_hist where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            pSet.Query += " and due_state = 'R' and qtr_to_pay = '" + sTmpQtrToPay + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from bill_no where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from bill_hist where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from btax where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from tax_and_fees where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from addl where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from fire_tax where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from reg_table where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from bill_gross_info where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            pSet.Query += " and due_state = 'R' and qtr = '" + sTmpQtrToPay + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from treasurers_module where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from treasurers_module_tmp where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = "delete from gross_monitoring where bin = '" + strBin + "' and tax_year = '" + sTmpTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            {
                            }
                        }
                    }
                    pTmp.Close();
                    // RMC 20160517 additional clean-up in cancel billing (e)
                }

                //MCR 20210803 (s)
                OracleResultSet pCmd = new OracleResultSet();
                pCmd.Query = "insert into soa_monitoring_hist (select bin,status,memo,refno,sysdate from soa_monitoring where bin = :1)";
                pCmd.AddParameter(":1", strBin);
                pCmd.ExecuteNonQuery();

                pCmd.Query = "delete from soa_monitoring where bin = :1";
                pCmd.AddParameter(":1", strBin);
                pCmd.ExecuteNonQuery();

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
                //MCR 20210803(e)

                pCmd.Close();
                pSet.Close();

                string strObj = strBin.Trim() + "/" + strYear;
                if (AuditTrail.InsertTrail("AICB", "taxdues", StringUtilities.HandleApostrophe(strObj)) == 0)
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Billing of BIN :" + strBin + " has been cancelled.", "CANCEL BILLING", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // RMC 20120112 added end-tasking in cancel billing (s)
                if (TaskMan.IsObjectLock(strBin, "BILLING", "DELETE", "COL"))
                {
                }
                // RMC 20120112 added end-tasking in cancel billing (e)

                BillingForm.Close();    // RMC 20110826 added auto-close after saving 
            }
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

        // RMC 20110826 added validation if billing exists (s)
        public override bool ValidBilling()
        {
            OracleResultSet pSet = new OracleResultSet();

            string strBin;
            string strYear;

            strBin = BillingForm.BIN;
            strYear = BillingForm.TaxYear;

            // RMC 20120116 added re-capturing of current year in Cancel Billing (s)
            if (m_sCurrentYear == "")
                m_sCurrentYear = ConfigurationAttributes.CurrentYear;
            // RMC 20120116 added re-capturing of current year in Cancel Billing (e)

            //pSet.Query = string.Format("select * from taxdues where bin = '{0}' and tax_year >= '{1}'", strBin, m_sCurrentYear); // GDE 20120327 try less than
            pSet.Query = string.Format("select * from taxdues where bin = '{0}' and tax_year <= '{1}'", strBin, m_sCurrentYear);
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

                    // RMC 20120117 added deletion of rev-exam billing in Cancel Billing transaction (s)
                    pSet.Query = string.Format("select * from taxdues where bin = '{0}' and due_state = 'A'", strBin);
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
                            // RMC 20120117 added deletion of rev-exam billing in Cancel Billing transaction (e)

                            MessageBox.Show("No record found.", "Cancel Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return false;
                        }
                    }
                }
            }

            return true;

        }

        private void CancelRevExamBilling(string strBin)
        {
            // RMC 20120117 added deletion of rev-exam billing in Cancel Billing transaction
            OracleResultSet pRevExam = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();
            string sRevExamYear = "";

            pRevExam.Query = "select distinct tax_year from taxdues where bin = '" + strBin + "'";
           // pRevExam.Query += " and due_state = 'A'";
            pRevExam.Query += " and due_state <> 'R'"; ;//JHB 20181122 cancel bill if due_state is not 'R'
            if (pRevExam.Execute())
            {
                while (pRevExam.Read())
                {
                    sRevExamYear = pRevExam.GetString(0);

                    RevertGrossCapital(strBin, sRevExamYear); // ALJ 20130116 revert gross/capital of businesses/hist, addl_bns 
                    
                    pSet.Query = string.Format("delete from ass_bill_gross_info where bin = '{0}' and tax_year = '{1}' ", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    pSet.Query = string.Format("delete from ass_taxdues where bin = '{0}' and tax_year = '{1}' and due_state = 'A'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    pSet.Query = string.Format("delete from reass_bill_gross_info where bin = '{0}' and tax_year = '{1}' ", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    pSet.Query = string.Format("delete from reass_taxdues where bin = '{0}' and tax_year = '{1}' and due_state = 'A'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from taxdues where bin = '{0}' and tax_year = '{1}' and due_state = 'A'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from taxdues_hist where bin = '{0}' and tax_year = '{1}' and due_state = 'A'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_no where bin = '{0}' and tax_year = '{1}'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_hist where bin = '{0}' and tax_year = '{1}'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from btax where bin = '{0}' and tax_year = '{1}'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from tax_and_fees where bin = '{0}' and tax_year = '{1}'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }


                    pSet.Query = string.Format("delete from addl where bin = '{0}' and tax_year = '{1}'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from fire_tax where bin = '{0}' and tax_year = '{1}'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from reg_table where bin = '{0}' and tax_year = '{1}'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    //JHB 20181122 add cancel bill if due_state is 'X', save to taxdues_hist (s)
                    pSet.Query = string.Format("insert into taxdues_hist select * from taxdues where bin = '{0}' and due_state = 'X'", strBin,sRevExamYear);  
                    if (pSet.ExecuteNonQuery() == 0)
                    { 
                    }
                    pSet.Query = string.Format("delete from taxdues where bin = '{0}' and due_state = 'X'", strBin,sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    
                    //JHB 20181122 add cancel bill if due_state is 'X', save to taxdues_hist (e)

                    //JARS 20181004 (S) AVOID DELETING, INSTEAD REVERT ADJ_GROSS TO 0
                    //pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}' and tax_year = '{1}'", strBin, sRevExamYear);
                    //if (pSet.ExecuteNonQuery() == 0)
                    //{
                    //}
                    pSet.Query = string.Format("update bill_gross_info set adj_gross = 0 where bin = '{0}' and tax_year = '{1}'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    bAdjustmentCancel = true;
                }
            }
            pRevExam.Close();
        }

        /// <summary>
        /// ALJ 20130116 revert gross/capital of businesses/hist, addl_bns 
        /// </summary>
        /// <param name="strBin"></param>
        private void RevertGrossCapital(string strBin, string strTaxYear)
        {
            OracleResultSet pRecTaxDue = new OracleResultSet();
            OracleResultSet pRecBillGross = new OracleResultSet();
            OracleResultSet pCmd = new OracleResultSet();

            string strBnsCode, strQtr, strBnsStat;
            double dGross, dCapital, dAdjGross;
            pRecTaxDue.Query = "select qtr_to_pay, bns_code_main from taxdues where bin = '" + strBin + "'";
            pRecTaxDue.Query += " and tax_year = '" + strTaxYear + "' and due_state = 'A'";
            if (pRecTaxDue.Execute())
            {
                while (pRecTaxDue.Read())
                {
                    strQtr = pRecTaxDue.GetString("qtr_to_pay");
                    strBnsCode = pRecTaxDue.GetString("bns_code_main").Trim();
                    pRecBillGross.Query = "select bns_stat, capital, gross, adj_gross from bill_gross_info"; 
                    pRecBillGross.Query+= " where bin = '"+strBin+"' and tax_year = '"+strTaxYear+"'";
                    pRecBillGross.Query += " and bns_code = '"+strBnsCode+"' and qtr = '"+strQtr+"'";
                    if (pRecBillGross.Execute())
                    {
                        if (pRecBillGross.Read())
                        {
                            strBnsStat = pRecBillGross.GetString("bns_stat");
                            dCapital = pRecBillGross.GetDouble("capital");
                            dGross = pRecBillGross.GetDouble("gross");
                            dAdjGross = pRecBillGross.GetDouble("adj_gross");

                            if (dAdjGross > 0) 
                            {
                                pCmd.Query = string.Format("UPDATE businesses SET gr_1 = {0}, capital = {1} WHERE bin = '{2}' AND tax_year = '{3}' and bns_code = '{4}' and bns_stat = '{5}'", dGross, dCapital, strBin, strTaxYear, strBnsCode, strBnsStat);
                                pCmd.ExecuteNonQuery();
                                pCmd.Query = string.Format("UPDATE buss_hist SET gr_1 = {0}, capital = {1} WHERE bin = '{2}' AND tax_year = '{3}' and bns_code = '{4}' and bns_stat = '{5}'", dGross, dCapital, strBin, strTaxYear, strBnsCode, strBnsStat);
                                pCmd.ExecuteNonQuery();
                                pCmd.Query = string.Format("UPDATE addl_bns SET gross = {0}, capital = {1} WHERE bin = '{2}' AND tax_year = '{3}' and bns_code_main = '{4}' and bns_stat = '{5}' and qtr = '{6}'", dGross, dCapital, strBin, strTaxYear, strBnsCode, strBnsStat, strQtr);
                                pCmd.ExecuteNonQuery();

                            }
                        }
                    }
                }
                

            }
            pCmd.Close();
            pRecBillGross.Close();
            pRecTaxDue.Close();
        }
        //JHB 20181203 merge from LAL-LO "REV ADJUSTMENT" (S)
        public static void RevertDuesAmount(string strBin, string strTaxYear)
        {
            //JARS 20181121 ADDED THIS FUNCTION, TO REVERT REMAINING DUES TO THE AMOUNT BEFORE ADJUSTMENT
            OracleResultSet rHist = new OracleResultSet();
            OracleResultSet rDues = new OracleResultSet();

            rHist.Query = "select * from taxdues_hist where bin = '" + strBin + "' and tax_year = '" + strTaxYear + "' order by tax_code";

            if (rHist.Execute())
            {
                while (rHist.Read())
                {
                    rDues.Query = "update taxdues set amount = '" + rHist.GetDouble("amount") + "' where bin = '" + strBin + "' and tax_code = '" + rHist.GetString("tax_code") + "' and due_state = 'R'";
                    rDues.ExecuteNonQuery();
                }
            }
            rHist.Close();
        }
        //JHB 20181203 merge from LAL-LO "REV ADJUSTMENT" (E)


        public override void ReturnValues()
        {
            // RMC 20120117 added deletion of rev-exam billing in Cancel Billing transaction
            OracleResultSet pSet = new OracleResultSet();
            m_bIsInitLoad = true;
            //m_iSwRE = 1; // set revenue exam to 1
            pSet.Query = "SELECT * FROM businesses WHERE bin = :1";
            pSet.AddParameter(":1", BillingForm.BIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    /*OracleResultSet pSetPay = new OracleResultSet();
                    pSetPay.Query = "SELECT DISTINCT(bin) FROM pay_hist WHERE bin = :1 AND data_mode <> 'UNP'";
                    pSetPay.AddParameter(":1", BillingForm.BIN);
                    if (pSetPay.Execute())
                    {
                        if (!pSetPay.Read())
                        {
                            MessageBox.Show("Record Not Posted", "Revenue Examination", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            pSetPay.Close();
                            return;
                        }
                    }
                    pSetPay.Close();*/  // RMC 20161213 removed validation if without payment in cancelling retirement billing
                }
                else
                {
                    pSet.Close();

                    pSet.Query = "SELECT * FROM business_que WHERE bin = :1";
                    pSet.AddParameter(":1", BillingForm.BIN);
                    if (pSet.Execute())
                    {
                        if (!pSet.Read())
                        {
                            pSet.Close();
                            MessageBox.Show("Record Not Found", "Cancel Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                m_sMainBnsStat = pSet.GetString("bns_stat");
                /*if (m_sMainBnsStat != "REN")
                {
                    pSet.Close();
                    MessageBox.Show("New Business. Not valid for revenue examination", "Revenue Examination", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }*/

                if (m_sMainBnsStat == "RET") //JARS 20180522
                {
                    m_sDueState = "X";
                }
                else
                {
                    m_sDueState = "R";
                }

                m_sDueState = "R";
                BillingForm.BillFlag = false;
                BillingForm.Quarter = "1";
                BillingForm.TaxYear = pSet.GetString("tax_year");
                BillingForm.BusinessName = pSet.GetString("bns_nm").Trim();
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

                // RMC 20150120
                if (m_sBaseDueYear == "" && m_sMaxDueYear == "")
                {
                    MessageBox.Show("Record Not Found", "Cancel Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // RMC 20150120

                // RMC 20150112 mods in retirement billing (s)
                if(m_sBaseDueYear != "")
                    BillingForm.TaxYear = m_sBaseDueYear;
                // RMC 20150112 mods in retirement billing (e)

                m_sCurrentYear = m_sMaxDueYear;
                PopulateBnsType();
                PopulateGrossInfo(BillingForm.Status, m_sDueState, BillingForm.TaxYear, BillingForm.Quarter, BillingForm.TaxYear, BillingForm.Quarter, "CAN"); // insert initial values for bill_gross_info table // ALJ 20121212 add p_sModule to identify regular billing of rev-exam (fixed bug in wrong gross basis for tax computation)
                LoadAddlInfo();
                LoadGrossCapital();
                AddlInfoReadOnly(true);
                m_bIsInitLoad = false;
                BillingForm.EnabledControls(true);
                BillingForm.TaxYearTextBox.ReadOnly = false;


            }
            pSet.Close();
            m_bIsInitLoad = false;
        }

        protected override void GetMinMaxDueYear(string p_sBIN, out string o_sMinDueYear, out string o_sMaxDueYear)
        {
            OracleResultSet pSet = new OracleResultSet();
            //MCR 20150325 Cheking
            bool bPrmtUpdateApp = false;
            bool bBnsQueApp = false;
            pSet.Query = "select * from Permit_Update_Appl where bin = '" + p_sBIN + "'";
            pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";  // RMC 20180117 correction in cancel billing to cater taxdues already paid
            if (pSet.Execute())
                if (pSet.Read())
                    bPrmtUpdateApp = true;
            pSet.Close();

            pSet.Query = "select * from business_que where bin = '" + p_sBIN + "'";
            pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";  // RMC 20180117 correction in cancel billing to cater taxdues already paid
            if (pSet.Execute())
                if (pSet.Read())
                    bBnsQueApp = true;
            pSet.Close();

            // RMC 20180117 correction in cancel billing to cater taxdues already paid (s)
            // ADD CLEAN-UP OF TAXDUES HERE
            string sTmpTy = "";
            string sTmpQtr = "";
            string sTmpQtrPd = "";
            int iQtr = 0;
            OracleResultSet pCmd = new OracleResultSet();
            
            pSet.Query = "select * from pay_hist where bin = '" + p_sBIN + "' and tax_year in ";
            pSet.Query += "(select max(tax_year) from pay_hist where bin = '" + p_sBIN + "')";
            pSet.Query += " order by qtr_paid desc, no_of_qtr desc";
            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    sTmpTy = pSet.GetString("tax_year");
                    if (pSet.GetString("qtr_paid") == "F")
                    {
                        pCmd.Query = "delete from taxdues where bin = '" + p_sBIN + "'";
                        pCmd.Query += " and tax_year = '" + sTmpTy + "'";
                        pCmd.Query += " and due_state = 'R'";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                    }
                    else
                    {
                        sTmpQtr = pSet.GetString("no_of_qtr");
                        if (sTmpQtr == "" || sTmpQtr == null)
                            sTmpQtr = "1";

                        sTmpQtrPd = pSet.GetString("qtr_paid");

                        
                        if  (sTmpQtrPd == "" || sTmpQtrPd == null)
                            sTmpQtrPd = "1";
                        //JHB 20181121 add checking if sTmpQtrPd is 'A' or 'P' (s)
                        if (sTmpQtrPd == "A" || sTmpQtrPd == "P")
                        {
                           
                        }//JHB 20181121 add checking if sTmpQtrPd is 'A' or 'P' (e)
                        else 
                        {
                            iQtr = Convert.ToInt32(sTmpQtrPd) + Convert.ToInt32(sTmpQtr) - 1;
                            string sQtrPaid = string.Empty;
                            sQtrPaid = string.Format("{0:##}", iQtr);

                            pCmd.Query = "delete from taxdues where bin = '" + p_sBIN + "'";
                            pCmd.Query += " and tax_year = '" + sTmpTy + "'";
                            pCmd.Query += " and qtr_to_pay <= '" + sQtrPaid + "'";
                            pCmd.Query += " and due_state = 'R'";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }

                        }
                        
                    }
                }
            }
            pSet.Close();
            // RMC 20180117 correction in cancel billing to cater taxdues already paid (e)


            if (bPrmtUpdateApp == true && bBnsQueApp == false)
            {
                pSet.Query = "SELECT MIN(tax_year) AS min_year, MAX(tax_year) AS max_year FROM taxdues WHERE bin = :1";
                pSet.Query += " and due_state <> 'R'";  // RMC 20160512 correction in cancel billing
            }
            else
            {
                pSet.Query = "SELECT MIN(tax_year) AS min_year, MAX(tax_year) AS max_year FROM taxdues WHERE bin = :1";
                pSet.Query += " and tax_year not in (select tax_year from pay_hist where bin = '" + p_sBIN + "' and bns_stat <> 'RET')";  // RMC 20150120
            }

            // RMC 20150112 mods in retirement billing
            o_sMinDueYear = string.Empty;
            o_sMaxDueYear = string.Empty;
            //pSet.Query = "SELECT MIN(tax_year) AS min_year, MAX(tax_year) AS max_year FROM taxdues WHERE bin = :1";
            //pSet.Query += " and tax_year not in (select tax_year from pay_hist where bin = '" + p_sBIN + "' and bns_stat <> 'RET')";  // RMC 20150120
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

            //JARS 20181019 IMPORTED FROM BIÑAN BPLS
            // RMC 20170920 added additional validation in Cancel billing if with Adjustment (s)

            if (o_sMinDueYear == "" && o_sMaxDueYear == "")
            {
                //pSet.Query = "SELECT MIN(tax_year) AS min_year, MAX(tax_year) AS max_year,  FROM taxdues WHERE bin = :1";
               // pSet.Query += " and due_state = 'A'";

                pSet.Query = "SELECT MIN(tax_year) AS min_year, MAX(tax_year) AS max_year,due_state FROM taxdues  ";//JHB 20181122 cancel bill if due_state is not 'R'
                pSet.Query += "WHERE bin = :1 and due_state <> 'R' group by tax_year, due_state ";

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

        
            // RMC 20170920 added additional validation in Cancel billing if with Adjustment (e)
        }
    }
}


