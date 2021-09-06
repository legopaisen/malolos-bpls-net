using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Amellar.Common.DataConnector;

namespace Amellar.BPLS.Billing
{
    /// <summary>
    /// ALJ 20090728
    /// Class for Billing of Fire Tax (PD1185)
    /// </summary>
    public class BillFire:Fee
    {
        public BillFire(frmBillFee Form):base(Form)
        {
        }
        public override void UpdateList()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sFeesDesc, sFeesCode;
            double fAmount;

            BillFeeForm.Fees.Columns.Clear();
            BillFeeForm.Fees.Columns.Add(new DataGridViewCheckBoxColumn());
            BillFeeForm.Fees.Columns.Add("FeesDesc", "Taxes/Fees/Additional Charges");
            BillFeeForm.Fees.Columns.Add("FeesCode", "Fees Code");
            BillFeeForm.Fees.Columns.Add("Amount", "Amount");
            BillFeeForm.Fees.Columns[0].Width = 20;
            BillFeeForm.Fees.Columns[0].ReadOnly = false;
            BillFeeForm.Fees.Columns[1].Width = 350;
            BillFeeForm.Fees.Columns[1].ReadOnly = true;
            BillFeeForm.Fees.Columns[2].Width = 80;
            BillFeeForm.Fees.Columns[2].ReadOnly = true;
            BillFeeForm.Fees.Columns[2].Visible = false; // hide in ADDL Charges
            BillFeeForm.Fees.Columns[3].Width = 100;
            BillFeeForm.Fees.Columns[3].ReadOnly = true;
            BillFeeForm.Fees.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            BillFeeForm.Fees.Rows.Clear();
            pSet.Query = "SELECT bns_desc, fees_code, qty, amount FROM addl_table WHERE bin = :1 AND bns_code_main = :2 AND data_type = 'FR' ORDER BY fees_code";
            pSet.AddParameter(":1", BillFeeForm.BIN);
            pSet.AddParameter(":2", BillFeeForm.BusinessCode);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    fAmount = 0;
                    sFeesDesc = pSet.GetString("bns_desc").Trim();
                    sFeesCode = pSet.GetString("fees_code").Trim();
                    fAmount = pSet.GetDouble("amount");
                    if (fAmount > 0)
                        BillFeeForm.Fees.Rows.Add(true, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount));
                    else
                        BillFeeForm.Fees.Rows.Add(false, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount));
                }
            }
            pSet.Close();
        }

        public override double Compute(string p_sCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            double fRate = 0;
            double dAmount = 0, fAmount = 0;
            // get fire tax rate
            pSet.Query = "SELECT fees_rate FROM fire_tax_tag WHERE fees_code = :1";
            pSet.AddParameter(":1", p_sCode);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    fRate = pSet.GetDouble("fees_rate");
                    fRate = fRate / 100;
                }
            }
            // get amount 
            pSet.Query = "SELECT amount FROM bill_table WHERE bin = :1 AND bns_code_main = :2 AND fees_code = :3 AND fees_type = 'TF'";
            pSet.AddParameter(":1", BillFeeForm.BIN);
            pSet.AddParameter(":2", BillFeeForm.BusinessCode);
            pSet.AddParameter(":3", p_sCode);
            if (pSet.Execute())
            {
                if (pSet.Read())
                    fAmount = pSet.GetDouble("amount");
                else
                {
                    pSet.Query = "SELECT amount FROM addl_table WHERE bin = :1 AND bns_code_main = :2 AND fees_code = :3 AND data_type = 'AD'";
                    pSet.AddParameter(":1", BillFeeForm.BIN);
                    pSet.AddParameter(":2", BillFeeForm.BusinessCode);
                    pSet.AddParameter(":3", p_sCode);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                            fAmount = pSet.GetDouble("amount");

                    } 
                }

            }
            pSet.Close();
            dAmount = fAmount * fRate;
            return dAmount;

        }


        public override void UpdateRows()
        {
            OracleResultSet pCmd = new OracleResultSet();
            double fAmount;
            int iRows, i;
            string sFeesCode;
            iRows = BillFeeForm.Fees.Rows.Count;
            for (i = 0; i < iRows; ++i)
            {
                sFeesCode = BillFeeForm.Fees.Rows[i].Cells[2].Value.ToString();
                double.TryParse(BillFeeForm.Fees.Rows[i].Cells[3].Value.ToString(), out fAmount);
                pCmd.Query = "UPDATE addl_table SET amount = :1 WHERE bin = :2 AND bns_code_main = :3 AND fees_code = :4 AND data_type = 'FR'";
                pCmd.AddParameter(":1", fAmount);
                pCmd.AddParameter(":2", BillFeeForm.BIN);
                pCmd.AddParameter(":3", BillFeeForm.BusinessCode);
                pCmd.AddParameter(":4", sFeesCode);
                pCmd.ExecuteNonQuery();

            }
        }

 

    }
}


