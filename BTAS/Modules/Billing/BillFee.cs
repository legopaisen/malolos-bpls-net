//#######################################
// Modifications:  
// ALJ 20110812 Message Prompt from bill_fee_sub proc if no schedule found of no data in other info              
// ALJ 20110727 for quarterly fee (qtr_fee_config)                
//#######################################
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Amellar.Common.DataConnector;

namespace Amellar.BPLS.Billing
{
    /// <summary>
    /// ALJ 20090723
    /// Class for Billing of Fees subcategories
    /// </summary>
    public class BillFee:Fee
    {
        public BillFee(frmBillFee Form):base(Form)
        {
        }
        public override void UpdateList()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBnsDescSub, sBnsCodeSub, sStatCover;
            double fAmount = 0;
            BillFeeForm.Fees.Columns.Clear();
            BillFeeForm.Fees.Columns.Add(new DataGridViewCheckBoxColumn());
            BillFeeForm.Fees.Columns.Add("Business Type", "BusinessType");
            BillFeeForm.Fees.Columns.Add("Code", "Code");
            BillFeeForm.Fees.Columns.Add("Amount", "Amount");
            BillFeeForm.Fees.Columns[0].Width = 20;
            BillFeeForm.Fees.Columns[0].ReadOnly = false;
            BillFeeForm.Fees.Columns[1].Width = 300;
            BillFeeForm.Fees.Columns[1].ReadOnly = true;
            BillFeeForm.Fees.Columns[2].Width = 80;
            BillFeeForm.Fees.Columns[2].ReadOnly = true;
            BillFeeForm.Fees.Columns[3].Width = 100;
            BillFeeForm.Fees.Columns[3].ReadOnly = true;
            BillFeeForm.Fees.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            BillFeeForm.Fees.Rows.Clear();
            if (BillFeeForm.Status == "REN" || BillFeeForm.Status == "RET")
                sStatCover = "REN";
            else
                sStatCover = "NEW";
            //pSet.Query = "SELECT bns_desc_sub, bns_code_sub, amount FROM fee_table WHERE bin = :1 AND bns_code_main = :2 AND fees_code = :3 ORDER BY bns_code_sub";
            pSet.Query = "SELECT a.bns_desc, a.bns_code FROM bns_table a, default_table b WHERE a.fees_code = b.fees_code AND a.bns_code = b.default_fee AND a.rev_year = b.rev_year AND a.fees_code = :1 AND b.bns_code = :2 AND b.stat_cover = :3 AND b.rev_year = :4 ORDER BY a.bns_code";
            pSet.AddParameter(":1", BillFeeForm.FeesCode);
            pSet.AddParameter(":2", BillFeeForm.BusinessCode);
            pSet.AddParameter(":3", sStatCover);
            pSet.AddParameter(":4", BillFeeForm.RevisionYear);
            if (pSet.Execute())
            {
                OracleResultSet pSetFeeTable = new OracleResultSet();
                while (pSet.Read())
                {
                    sBnsDescSub = pSet.GetString("bns_desc").Trim();
                    sBnsCodeSub = pSet.GetString("bns_code").Trim();
                    pSetFeeTable.Query = "SELECT amount FROM fee_table WHERE bin = :1 AND fees_code = :2 AND bns_code_main = :3 AND bns_code_sub = :4";
                    pSetFeeTable.AddParameter(":1", BillFeeForm.BIN);
                    pSetFeeTable.AddParameter(":2", BillFeeForm.FeesCode);
                    pSetFeeTable.AddParameter(":3", BillFeeForm.BusinessCode);
                    pSetFeeTable.AddParameter(":4", sBnsCodeSub);
                    if (pSetFeeTable.Execute())
                    {
                        if (pSetFeeTable.Read())
                        {
                            fAmount = pSetFeeTable.GetDouble("amount");
                            if (fAmount > 0)
                                BillFeeForm.Fees.Rows.Add(true, sBnsDescSub, sBnsCodeSub, string.Format("{0:#,##0.00}", fAmount));
                            else
                                BillFeeForm.Fees.Rows.Add(false, sBnsDescSub, sBnsCodeSub, string.Format("{0:#,##0.00}", fAmount));
                        }
                        else
                            BillFeeForm.Fees.Rows.Add(false, sBnsDescSub, sBnsCodeSub, string.Format("{0:#,##0.00}", 0));
                    }
                    pSetFeeTable.Close();   // RMC 20141127 QA Tarlac ver

                }
                //pSetFeeTable.Close();// RMC 20141127 QA Tarlac ver, put rem
            }   
            pSet.Close();
        }

        public override double Compute(string p_sCode)
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            string sMessagePrompt = string.Empty; // ALJ 20110812 Message Prompt from bill_fee_sub proc if no schedule found of no data in other info 
            double dAmount = 0;
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "bill_fee_sub";
            plsqlCmd.ParamValue = BillFeeForm.BIN;  
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillFeeForm.BusinessCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillFeeForm.TaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillFeeForm.Status;
            plsqlCmd.AddParameter("p_sSTatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillFeeForm.Quarter; // ALJ 20110727 for quarterly fee (qtr_fee_config)
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input); // ALJ 20110727 for quarterly fee (qtr_fee_config)
            plsqlCmd.ParamValue = BillFeeForm.FeesCode;
            plsqlCmd.AddParameter("p_sFeesCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sCode;
            plsqlCmd.AddParameter("p_sDefaultFee", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillFeeForm.Gross;
            plsqlCmd.AddParameter("p_fGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillFeeForm.Capital;
            plsqlCmd.AddParameter("p_fCapital", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillFeeForm.RevisionYear;
            plsqlCmd.AddParameter("p_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = dAmount;
            plsqlCmd.AddParameter("o_fDefaultAmount", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Output);
            // (s) ALJ 20110812 Message Prompt from bill_fee_sub proc if no schedule found of no data in other info 
            plsqlCmd.ParamValue = string.Empty;
            plsqlCmd.AddParameter("o_sMessagePrompt", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Output, 100);
            // (e) ALJ 20110812 Message Prompt from bill_fee_sub proc if no schedule found of no data in other info 
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
                plsqlCmd.Close();

            }
            else
            {
                double.TryParse(plsqlCmd.ReturnValue("o_fDefaultAmount").ToString(), out dAmount);
                // (s) ALJ 20110812 Message Prompt from bill_fee_sub proc if no schedule found of no data in other info 
                sMessagePrompt = plsqlCmd.ReturnValue("o_sMessagePrompt").ToString();
                if (sMessagePrompt != "null")
                {
                    MessageBox.Show(sMessagePrompt, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // (s) ALJ 20110812 Message Prompt from bill_fee_sub proc if no schedule found of no data in other info 

            }
            plsqlCmd.Close();

            // RMC 20160109 customized special ordinance module for Tumauini (s)
            plsqlCmd = new OraclePLSQL();
            plsqlCmd.ProcedureName = "COMPUTE_ACCELERATION_AMT";
            plsqlCmd.ParamValue = BillFeeForm.RevisionYear;
            plsqlCmd.AddParameter("p_RevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillFeeForm.TaxYear;
            plsqlCmd.AddParameter("p_TaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = "F";
            plsqlCmd.AddParameter("p_FeeCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillFeeForm.FeesCode;
            plsqlCmd.AddParameter("p_TaxFeesCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = dAmount;
            plsqlCmd.AddParameter("p_Amount", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = BillFeeForm.BIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = dAmount;
            plsqlCmd.AddParameter("p_fResult", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Output);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
                plsqlCmd.Close();
            }
            double.TryParse(plsqlCmd.ReturnValue("p_fResult").ToString(), out dAmount);
            // RMC 20160109 customized special ordinance module for Tumauini (e)

            return dAmount;

        }


        public override void UpdateRows()
        {
            OracleResultSet pCmd = new OracleResultSet();
            double fAmount;
            string sCode;
            int iRows, i;
            iRows = BillFeeForm.Fees.Rows.Count;
            for (i = 0; i < iRows; ++i)
            {
                sCode = BillFeeForm.Fees.Rows[i].Cells[2].Value.ToString();
                double.TryParse(BillFeeForm.Fees.Rows[i].Cells[3].Value.ToString(), out fAmount);
                pCmd.Query = "UPDATE fee_table SET amount = :1 WHERE bin = :2 AND bns_code_main = :3 AND fees_code = :4 AND bns_code_sub = :5";
                pCmd.AddParameter(":1", fAmount);
                pCmd.AddParameter(":2", BillFeeForm.BIN);
                pCmd.AddParameter(":3", BillFeeForm.BusinessCode);
                pCmd.AddParameter(":4", BillFeeForm.FeesCode);
                pCmd.AddParameter(":5", sCode);
                pCmd.ExecuteNonQuery();
                
            }
        }         

    }
}


