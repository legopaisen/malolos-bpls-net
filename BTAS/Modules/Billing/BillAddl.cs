using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Amellar.Common.DataConnector;

namespace Amellar.BPLS.Billing
{
    /// <summary>
    /// ALJ 20090727
    /// Class for billing of Addl charges
    /// </summary>
    public class BillAddl:Fee
    {
        private string m_sType;
        private double m_fPerQtyAmt;

        public BillAddl(frmBillFee Form):base(Form)
        {
        }


        public override void UpdateList()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet(); //JARS 20171005
            string sFeesDesc, sFeesCode;
            double fAmount;
            int iQty; // addl charges
            BillFeeForm.Fees.Columns.Clear();
            BillFeeForm.Fees.Columns.Add(new DataGridViewCheckBoxColumn());
            BillFeeForm.Fees.Columns.Add("FeesDesc", "Additional Charges");
            BillFeeForm.Fees.Columns.Add("FeesCode", "Fees Code");
            BillFeeForm.Fees.Columns.Add("Amount", "Amount");
            BillFeeForm.Fees.Columns.Add("Qty", "Qty");
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
            BillFeeForm.Fees.Columns[4].Width = 80; // Addl Charges
            BillFeeForm.Fees.Columns[4].ReadOnly = true; // Addl Charges
            BillFeeForm.Fees.Columns[4].Visible = false;  // Addl Charges          
            BillFeeForm.Fees.Rows.Clear();
            if (BillFeeForm.moduleswitch == "BARANGAY CLEARANCE FEE")// peb 20191219
            {
            }
            pSet.Query = "SELECT bns_desc, fees_code, qty, amount FROM addl_table WHERE bin = :1 AND bns_code_main = :2 AND data_type = 'AD' ORDER BY fees_code";
            pSet.AddParameter(":1", BillFeeForm.BIN);
            pSet.AddParameter(":2", BillFeeForm.BusinessCode);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    fAmount = 0;
                    iQty = 0;
                    sFeesDesc = pSet.GetString("bns_desc").Trim();
                    sFeesCode = pSet.GetString("fees_code").Trim();
                    iQty = pSet.GetInt("qty");
                    fAmount = pSet.GetDouble("amount");

                    //JARS 20171005
                    pSet2.Query = "select * from pay_hist where bin = '" + BillFeeForm.BIN + "' and or_no in (select or_no from or_table where fees_code = '" + sFeesCode + "' and tax_year = '" + BillFeeForm.TaxYear + "')"; //JARS 20171005
                    if(pSet2.Execute())
                    {
                        if (pSet2.Read())
                        {
                            fAmount = 0;
                            BillFeeForm.Fees.Rows.Add(false, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount), string.Format("{0:#,###}", iQty));
                        }
                        else
                        {
                            if (fAmount > 0)
                                BillFeeForm.Fees.Rows.Add(true, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount), string.Format("{0:#,###}", iQty));
                            else
                                BillFeeForm.Fees.Rows.Add(false, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount), string.Format("{0:#,###}", iQty));
                        }
                    }
                    pSet2.Close();
                }
            }
            pSet.Close();
        }

        /* Revised codes...
        public override void UpdateList()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sFeesDesc, sFeesCode;
            double fAmount = 0;
            int iQty; // addl charges
            BillFeeForm.Fees.Columns.Clear();
            BillFeeForm.Fees.Columns.Add(new DataGridViewCheckBoxColumn());
            BillFeeForm.Fees.Columns.Add("Additional Charges", "FeesDesc");
            BillFeeForm.Fees.Columns.Add("Fees Code", "FeesCode");
            BillFeeForm.Fees.Columns.Add("Amount", "Amount");
            BillFeeForm.Fees.Columns.Add("Qty", "Qty");
            BillFeeForm.Fees.Columns[0].Width = 20;
            BillFeeForm.Fees.Columns[0].ReadOnly = false;
            BillFeeForm.Fees.Columns[1].Width = 300;
            BillFeeForm.Fees.Columns[1].ReadOnly = true;
            BillFeeForm.Fees.Columns[2].Width = 80;
            BillFeeForm.Fees.Columns[2].ReadOnly = true;
            BillFeeForm.Fees.Columns[2].Visible = false; // hide in ADDL Charges
            BillFeeForm.Fees.Columns[3].Width = 100;
            BillFeeForm.Fees.Columns[3].ReadOnly = true;
            BillFeeForm.Fees.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            BillFeeForm.Fees.Columns[4].Width = 80; // Addl Charges
            BillFeeForm.Fees.Columns[4].ReadOnly = true; // Addl Charges
            BillFeeForm.Fees.Columns[4].Visible = false;  // Addl Charges          
            BillFeeForm.Fees.Rows.Clear();
            pSet.Query = "SELECT fees_code,fees_desc FROM tax_and_fees_table WHERE fees_type = 'AD' AND rev_year = :1 ORDER BY fees_code";
            pSet.AddParameter(":1", BillFeeForm.RevisionYear);
            if (pSet.Execute())
            {
                OracleResultSet pSetAddl = new OracleResultSet();
                OracleResultSet pSetAddlTable = new OracleResultSet();
                OracleResultSet pCmd = new OracleResultSet();
                while (pSet.Read())
                {
                    sFeesDesc = pSet.GetString("fees_desc").Trim();
                    sFeesCode = pSet.GetString("fees_code").Trim();
                    pSetAddl.Query = "SELECT qty, amount FROM addl WHERE bin = :1 AND fees_code = :2 AND bns_code_main = :3 AND tax_year = :4 AND qtr = :5";
                    pSetAddl.AddParameter(":1", BillFeeForm.BIN);
                    pSetAddl.AddParameter(":2", sFeesCode);
                    pSetAddl.AddParameter(":3", BillFeeForm.BusinessCode);
                    pSetAddl.AddParameter(":4", BillFeeForm.TaxYear);
                    pSetAddl.AddParameter(":5", BillFeeForm.Quarter);

                    pSetAddlTable.Query = "SELECT qty, amount FROM addl_table WHERE bin = :1 AND fees_code = :2 AND bns_code_main = :3 AND data_type = 'AD'";
                    pSetAddlTable.AddParameter(":1", BillFeeForm.BIN);
                    pSetAddlTable.AddParameter(":2", sFeesCode);
                    pSetAddlTable.AddParameter(":3", BillFeeForm.BusinessCode);

                    

                    if (pSetAddl.Execute() && pSetAddlTable.Execute())
                    {
                        iQty = 0;
                        fAmount = 0;
                        if (pSetAddl.Read())
                        {
                            iQty = pSetAddl.GetInt("qty");
                            fAmount = pSetAddl.GetDouble("amount");
                            if (fAmount > 0)
                                BillFeeForm.Fees.Rows.Add(true, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount), string.Format("{0:#,###}", iQty));
                            else
                                BillFeeForm.Fees.Rows.Add(false, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount), string.Format("{0:#,###}", iQty));

                            // insert in addl_table if does not exist
                            if (!pSetAddlTable.Read())
                            {
                                pCmd.Query = "INSERT INTO addl_table VALUES (:1,:2,:3,:4,:5,:6,'AD')";
                                pCmd.AddParameter(":1", BillFeeForm.BIN);
                                pCmd.AddParameter(":2", BillFeeForm.BusinessCode);
                                pCmd.AddParameter(":3", sFeesCode);
                                pCmd.AddParameter(":4", sFeesDesc);
                                pCmd.AddParameter(":5", iQty);
                                pCmd.AddParameter(":6", fAmount);
                                pCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            if (pSetAddlTable.Read())
                            {
                                iQty = pSetAddlTable.GetInt("qty");
                                fAmount = pSetAddlTable.GetDouble("amount");
                                if (fAmount > 0)
                                    BillFeeForm.Fees.Rows.Add(true, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount), string.Format("{0:#,###}", iQty));
                                else
                                    BillFeeForm.Fees.Rows.Add(false, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount), string.Format("{0:#,###}", iQty));
                            }
                            else
                            {
                                BillFeeForm.Fees.Rows.Add(false, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount), string.Format("{0:#,###}", iQty));
                                // insert in addl_table if does not exist
                                pCmd.Query = "INSERT INTO addl_table VALUES (:1,:2,:3,:4,:5,:6,'AD')";
                                pCmd.AddParameter(":1", BillFeeForm.BIN);
                                pCmd.AddParameter(":2", BillFeeForm.BusinessCode);
                                pCmd.AddParameter(":3", sFeesCode);
                                pCmd.AddParameter(":4", sFeesDesc);
                                pCmd.AddParameter(":5", iQty);
                                pCmd.AddParameter(":6", fAmount);
                                pCmd.ExecuteNonQuery();

                            }
                        }


                    }
                }
                pSetAddl.Close();
                pSetAddlTable.Close();
            }   
            pSet.Close();
        }
        */

        public override double Compute(string p_sCode)
        {

            OracleResultSet pSet = new OracleResultSet();
            double dAmount = 0;
            pSet.Query = "SELECT data_type, amount FROM addl_sched WHERE fees_code = :1 AND rev_year = :2";
            pSet.AddParameter(":1", p_sCode);
            pSet.AddParameter(":2", BillFeeForm.RevisionYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sType = pSet.GetString("data_type").Trim();
                    dAmount = pSet.GetDouble("amount");
                    if (m_sType == "Q")
                    {
                        m_fPerQtyAmt = dAmount;
                        dAmount = 0;
                        BillFeeForm.Fees.Enabled = false;
                        BillFeeForm.LabelInput.Text = "Enter Quantity";
                        BillFeeForm.LabelInput.Visible = true;
                        BillFeeForm.Quantity.Visible = true;
                        BillFeeForm.Amount.Visible = false;
                        BillFeeForm.ButtonCompute.Visible = true;
                        BillFeeForm.Quantity.Focus();
                    }
                    else
                    {
                        if (m_sType == "O")
                        {
                            dAmount = 0;
                            BillFeeForm.Fees.Enabled = false;
                            BillFeeForm.LabelInput.Text = "Enter Amount";
                            BillFeeForm.LabelInput.Visible = true;
                            BillFeeForm.Quantity.Visible = false;
                            BillFeeForm.Amount.Visible = true;
                            BillFeeForm.ButtonCompute.Visible = true;
                            BillFeeForm.Amount.Focus();
                            
                        }
                        else
                        {
                            BillFeeForm.Fees.Enabled = true;
                            BillFeeForm.LabelInput.Visible = false;
                            BillFeeForm.Quantity.Visible = false;
                            BillFeeForm.Amount.Visible = false;
                            BillFeeForm.ButtonCompute.Visible = false;
                            if (dAmount == 0)
                                MessageBox.Show("Schedule not found.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            pSet.Close();
            return dAmount;

        }

        public override double OnButtonCompute()
        {
            double dAmount = 0;
            switch (m_sType)
            {
                case "Q":
                    {
                        int iInput = 0;
                        int.TryParse(BillFeeForm.Quantity.Text, out iInput);
                        dAmount = iInput * m_fPerQtyAmt;
                        BillFeeForm.Amount.Text = "0";
                        break;
                    }
                case "O":
                    {
                        double.TryParse(BillFeeForm.Amount.Text, out dAmount);
                        BillFeeForm.Quantity.Text = "0";
                        break;
                    }
            }
            if (dAmount > 0)
                BillFeeForm.Fees.Rows[BillFeeForm.RowIndex].Cells[0].Value = true;
            BillFeeForm.Fees.Enabled = true;
            
                //BillFeeForm.Fees.CancelEdit();
            BillFeeForm.LabelInput.Visible = false;
            BillFeeForm.Quantity.Visible = false;
            BillFeeForm.Amount.Visible = false;
            BillFeeForm.ButtonCompute.Visible = false;
            return dAmount;
        }


        public override void UpdateRows()
        {
            OracleResultSet pCmd = new OracleResultSet();
            double fAmount;
            string sFeesCode;
            int iRows, i, iQty;
            iRows = BillFeeForm.Fees.Rows.Count;
            for (i = 0; i < iRows; ++i)
            {
                sFeesCode = BillFeeForm.Fees.Rows[i].Cells[2].Value.ToString();
                int.TryParse(BillFeeForm.Fees.Rows[i].Cells[4].Value.ToString(), out iQty);
                double.TryParse(BillFeeForm.Fees.Rows[i].Cells[3].Value.ToString(), out fAmount);
                pCmd.Query = "UPDATE addl_table SET amount = :1, qty = :2 WHERE bin = :3 AND bns_code_main = :4 AND fees_code = :5 AND data_type = 'AD'";
                pCmd.AddParameter(":1", fAmount);
                pCmd.AddParameter(":2", iQty);
                pCmd.AddParameter(":3", BillFeeForm.BIN);
                pCmd.AddParameter(":4", BillFeeForm.BusinessCode);
                pCmd.AddParameter(":5", sFeesCode);
                pCmd.ExecuteNonQuery();
                
            }
        }

    }
}


