//#######################################
// Modifications: 
// ALJ 20120309 Rev exam for new
//#######################################
using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using System.Windows.Forms;
using System.Data;

namespace Amellar.BPLS.Billing
{
    class RevExam:Billing
    {
        public RevExam(frmBilling Form): base(Form)
        {
        }
        public override void ReturnValues()
        {
            OracleResultSet pSet = new OracleResultSet();
            m_bIsInitLoad = true;
            m_iSwRE = 1; // set revenue exam to 1
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
                    MessageBox.Show("Record Not Found", "Revenue Examination", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                m_sMainBnsStat = pSet.GetString("bns_stat");              

                m_sDtOperated = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("dt_operated"));
                
                // (s) ALJ 20120309 Rev exam for new
                if (m_sMainBnsStat != "REN")
                {
                    if (m_sMainBnsStat == "NEW")
                    {
                        BillingForm.Quarter = LibraryClass.GetQtr(pSet.GetDateTime("dt_operated"));
                        m_sDueState = "N";
                        MessageBox.Show("You are about to re-compute taxes and fees new business.", "Revenue Examination", MessageBoxButtons.OK, MessageBoxIcon.Warning); // ALJ 20120309 Rev axam for NEW
                    }
                    else
                    {
                        pSet.Close();
                        MessageBox.Show("Retired Business. Not valid for revenue examination", "Revenue Examination", MessageBoxButtons.OK, MessageBoxIcon.Warning); // ALJ 20120309 Rev axam for NEW - put "//
                        return;
                    }


                }
                else
                {
                    m_sDueState = "R";
                    BillingForm.Quarter = "1";
                }
                // (e) ALJ 20120309 Rev exam for new
               
                BillingForm.BillFlag = false;
                // BillingForm.Quarter = "1"; // ALJ 20120309 Rev exam for new - put "//
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
                PopulateGrossInfo(BillingForm.Status, m_sDueState, BillingForm.TaxYear, BillingForm.Quarter, BillingForm.TaxYear, BillingForm.Quarter, "ADJ"); // insert initial values for bill_gross_info table
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

        protected override void AddlInfoReadOnly(bool p_blnEnabled)
        {
            BillingForm.AdditionalInformation.Columns[3].ReadOnly = p_blnEnabled;
            BillingForm.AdjGrossTextBox.ReadOnly = p_blnEnabled;
            BillingForm.VATGrossTextBox.ReadOnly = p_blnEnabled;
            if (BillingForm.Status == "NEW") // ALJ 20120309 Rev exam 
            {
                BillingForm.VATGrossTextBox.ReadOnly = true; // NEW always true for rev exam
            }
            BillingForm.CapitalTextBox.ReadOnly = true; // ALJ 20120309 Rev exam  -- NEW Original Capital always true for rev exam
            BillingForm.GrossTextBox.ReadOnly = true; // ALJ 20120309 Rev exam REN Original Gross always true for rev exam

                
        }

        /// <summary>
        /// ALJ 20121212
        /// Warning: upon click Apply, this function will update the gross of businesses table even without billing
        /// Pending: if the revenue adjustment does not continue find a way to revert the gross receipt. 
        /// </summary>
        protected override void UpdateBussGross()
        {
            OracleResultSet pCmd = new OracleResultSet();
            double dGrCap;

            double.TryParse(BillingForm.AdjGross, out dGrCap); // ALJ ALJ 20120309 Rev exam for new - put it here
            if (BillingForm.MainBusiness)
            {
                // (s) ALJ 20120309 Rev exam for new
                if (BillingForm.Status != "NEW")
                {
                // (e) ALJ 20120309 Rev exam for new
                    pCmd.Query = "UPDATE businesses SET gr_1 = :1 WHERE bin = :2 AND tax_year = :3";
                // (s) ALJ 20120309 Rev exam for new
                }
                else
                {
                    pCmd.Query = "UPDATE businesses SET capital = :1 WHERE bin =:2 AND tax_year = :3";
                }
                // (e) ALJ 20120309 Rev exam for new
                pCmd.AddParameter(":1", dGrCap);
                pCmd.AddParameter(":2", BillingForm.BIN);
                pCmd.AddParameter(":3", BillingForm.TaxYear);
                pCmd.ExecuteNonQuery();
            }
            else
            {
                // (s) ALJ 20120309 Rev exam for new
                if (BillingForm.Status != "NEW")
                {
                    // (e) ALJ 20120309 Rev exam for new
                    pCmd.Query = "UPDATE addl_bns SET gross = :1 WHERE bin = :2 AND bns_code_main = :3 AND tax_year = :4 AND qtr = :5";
                // (s) ALJ 20120309 Rev exam for new
                }
                else
                {
                    pCmd.Query = "UPDATE addl_bns SET capital = :1 WHERE bin = :2 AND bns_code_main = :3 AND tax_year = :4 AND qtr = :5"; 
                }
                // (e) ALJ 20120309 Rev exam for new
                pCmd.AddParameter(":1", dGrCap);
                pCmd.AddParameter(":2", BillingForm.BIN);
                pCmd.AddParameter(":3", BillingForm.BusinessCode);
                pCmd.AddParameter(":4", BillingForm.TaxYear);
                pCmd.AddParameter(":5", BillingForm.Quarter);
                pCmd.ExecuteNonQuery();

            }
            pCmd.Close();
            


        }

        protected override void GetMinMaxDueYear(string p_sBIN, out string o_sMinDueYear, out string o_sMaxDueYear)
        {
            o_sMinDueYear = string.Empty;
            o_sMaxDueYear = string.Empty;
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "SELECT MIN(tax_year) AS min_year, MAX(tax_year) AS max_year FROM pay_hist WHERE bin = :1";
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

        protected override void ComputeAdjustment()
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            double dTaxCreditAmount = 0;
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "compute_adjustment";
            plsqlCmd.ParamValue = BillingForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillingForm.Quarter;
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sOwnCode;
            plsqlCmd.AddParameter("p_sOwnCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sRevYear;
            plsqlCmd.AddParameter("p_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = AppSettingsManager.SystemUser.UserCode;
            plsqlCmd.AddParameter("p_sUser", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = dTaxCreditAmount;
            plsqlCmd.AddParameter("o_fTaxCreditAdj", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Output);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
                plsqlCmd.Close();

            }
            else
            {
                double.TryParse(plsqlCmd.ReturnValue("o_fTaxCreditAdj").ToString(), out dTaxCreditAmount);

            }
            plsqlCmd.Close();
            if (dTaxCreditAmount > 0)
            {
                MessageBox.Show("Warning! System detects an over payment amounting P" + string.Format("{0:#,##0.00}", dTaxCreditAmount), "Revenue Examination", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

    }
}
