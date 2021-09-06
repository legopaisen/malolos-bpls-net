//#######################################
// Modifications: 

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

        public override void CancelBillUpdateList()
        {
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

            strBin = BillingForm.BIN;
            //strYear = BillingForm.TaxYear; // ALJ 20110907 re-assessment validation put "//"
            strYear = m_sBaseDueYear; // ALJ 20110907 re-assessment validation

            if (MessageBox.Show("Cancel this billing?", "CANCEL BILLING", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // (s) ALJ 20110907 re-assessment validation
                pSet.Query = string.Format("delete from ass_bill_gross_info where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }
                pSet.Query = string.Format("delete from ass_taxdues where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }
                pSet.Query = string.Format("delete from reass_bill_gross_info where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }
                pSet.Query = string.Format("delete from reass_taxdues where bin = '{0}' and tax_year >= '{1}'", strBin, strYear);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }
                // (e) ALJ 20110907 re-assessment validation


                pSet.Query = string.Format("delete from taxdues where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">=" 
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("delete from taxdues_hist where bin = '{0}' and tax_year >= '{1}'", strBin, strYear); // ALJ 20110907 re-assessment validation- put ">="
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

                CancelRevExamBilling(strBin);   // RMC 20120117 added deletion of rev-exam billing in Cancel Billing transaction

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
            pRevExam.Query += " and due_state = 'A'";
            if (pRevExam.Execute())
            {
                while (pRevExam.Read())
                {
                    sRevExamYear = pRevExam.GetString(0);

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

                    pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}' and tax_year = '{1}'", strBin, sRevExamYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                }
            }
            pRevExam.Close();
        }

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
                    OracleResultSet pSetPay = new OracleResultSet();
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
                    pSetPay.Close();
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
                            MessageBox.Show("Record Not Found", "Revenue Examination", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                m_sCurrentYear = m_sMaxDueYear;
                PopulateBnsType();
                PopulateGrossInfo(BillingForm.Status, m_sDueState, BillingForm.TaxYear, BillingForm.Quarter, BillingForm.TaxYear, BillingForm.Quarter, "CAN"); // insert initial values for bill_gross_info table
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
    }
}


