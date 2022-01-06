using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.LogIn;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.Payment
{
    public partial class frmCancelPayment : Form
    {
        string m_strTaxYear = string.Empty;
        string m_strBnsStat = string.Empty;
        string m_strPlaceOccupancy = string.Empty;
        string strBin = string.Empty;
        frmLogIn fLogIn;
        string m_strReason = string.Empty;

        public frmCancelPayment()
        {
            InitializeComponent();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {

            OnProceed();
            btnInsufficientFund.Checked = true;
        }

        private void OnProceed()
        {
            if (txtORNo.Text.Trim() == string.Empty)
            {
                //MessageBox.Show("Enter OR No. to be cancelled first");
                MessageBox.Show("Enter OR No. to be cancel first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            bool bComp = false;
            string strTeller = string.Empty;
            string strOwnCode = string.Empty;
            string strLastOR = string.Empty;
            DateTime dtDate1 = AppSettingsManager.GetSystemDate();
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();

            string strMode = string.Empty;
            // AST 20160112 (s)
            //result.Query = "select a.* from pay_hist a, or_table b where a.or_no = b.or_no and or_date = '" + string.Format("{0:dd-MMM-yyyy}", dtDate1) + "' and a.or_no = '" + txtORNo.Text.Trim() + "'";
            result.Query = string.Format("select a.* from pay_hist a, or_table b where a.or_no = b.or_no and or_date = to_date('{0}', 'MM/dd/yyyy') and a.or_no = '{1}'", dtDate1.ToShortDateString(), txtORNo.Text.Trim());
            // AST 20160112 (e)
            if (result.Execute())
            {
                if (result.Read())
                { // <-
                    strTeller = result.GetString("teller").Trim();
                    //result2.Query = "select * from partial_remit where teller = '" + strTeller.Trim() + "' and '" + txtORNo.Text.Trim() + "' between or_from and or_to";
                    // RMC 20140909 Migration QA (s)
                    result2.Query = "select * from partial_remit where teller = '" + strTeller.Trim() + "' and ";
                    if (AppSettingsManager.GetConfigValue("10") == "021")
                        //result2.Query += "to_number(substr('" + txtORNo.Text.Trim() + "',3,20) between or_from and or_to"; MOD MCR 20141022
                        result2.Query += "substr('" + txtORNo.Text.Trim() + "',3,20) between or_from and or_to"; //MOD MCR 20141022
                    else
                        result2.Query += "'" + txtORNo.Text.Trim() + "' between or_from and or_to";
                    // RMC 20140909 Migration QA (e)
                    if (result2.Execute())
                    {
                        if (result2.Read())
                        {
                            MessageBox.Show("Cannot cancel. Payments have been remitted", "Cancel Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    result2.Close();
                } 
                else
                {
                    if (!AppSettingsManager.Granted("CTCPNC"))
                    {
                        MessageBox.Show("Cannot cancel not current dated OR");
                        ClearForm();
                        return;
                    }

                }
            }
            result.Close();

            // GDE 20110613
            result2.Query = "select a.*, b.data_mode from businesses a, pay_hist b where a.bin = b.bin and b.or_no = '" + txtORNo.Text.Trim() + "'";
            if (result2.Execute())
            {
                if (result2.Read())
                {
                    strBin = result2.GetString("bin").Trim();
                    strOwnCode = result2.GetString("own_code").Trim();
                    txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(strBin);
                    txtBnsName.Text = result2.GetString("bns_nm").Trim();

                    m_strTaxYear = result2.GetString("tax_year").Trim();
                    m_strPlaceOccupancy = result2.GetString("place_occupancy").Trim();
                    strMode = result2.GetString("data_mode").Trim();
                    m_strBnsStat = result2.GetString("bns_stat").Trim();

                }
                else
                {
                    MessageBox.Show("No own_code retrieved from businesses table.");
                }

                if (strMode == "POS")
                {
                    MessageBox.Show("Cannot cancel posted payments", "Cancel Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                result3.Query = "select * from own_names where own_code = '" + strOwnCode.Trim() + "'";
                if (result3.Execute())
                {
                    if (result3.Read())
                    {
                        txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(strOwnCode).Trim();
                        txtOwnName.Text = AppSettingsManager.GetBnsOwner(strOwnCode).Trim();
                    }
                }
                result3.Close();
            }
            result2.Close();

            ComputeMainItems();

            result2.Query = "select distinct * from pay_hist where bin = '" + strBin.Trim() + "' order by tax_year desc, or_date desc, time_posted desc, qtr_paid desc"; //AFM 20220106 added time_posted order
            result2.Query += ", or_no desc";    // RMC 20151229 merged multiple OR use from Mati
            if (result2.Execute())
            {
                if (result2.Read())
                {
                    strLastOR = result2.GetString("or_no").Trim();
                }
            }
            result2.Close();

            if (strLastOR.Trim() != txtORNo.Text.Trim())
            {
                result2.Query = "select * from comp_payhist where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
                if (result2.Execute())
                {
                    if (result2.Read())
                        bComp = true;
                }
                result2.Close();

                if (AppSettingsManager.Granted("CTCPNC") == true)
                {
                    MessageBox.Show("You can only cancel the latest OR");
                    ClearForm();
                    return;
                }

                if (bComp == false)
                {
                    MessageBox.Show("You can only cancel not latest OR with compromise!");
                    ClearForm();
                    return;
                }
            }

        }

        private void ClearForm()
        {
            txtORNo.Text = string.Empty;
            txtOthers.Text = string.Empty;
            txtBnsAdd.Text = string.Empty;
            txtBnsName.Text = string.Empty;
            txtBTax.Text = string.Empty;
            txtFees.Text = string.Empty;
            txtOwnAdd.Text = string.Empty;
            txtOwnName.Text = string.Empty;
            txtPen.Text = string.Empty;
            txtSurch.Text = string.Empty;
            txtTotal.Text = string.Empty;
            txtOthers.ReadOnly = true;  // RMC 20140909 Migration QA

            txtORNo.Focus();
        }

        private void ComputeMainItems()
        {
            string strTotalTax = "0";
            string strTotalFees = "0";
            string strTotalSurch = "0";
            string strTotalInt = "0";
            string strTotalTotal = "0";

            double dTax = 0;
            double dFees = 0;
            double dSurch = 0;
            double dInt = 0;
            double dTotal = 0;

            

            OracleResultSet result = new OracleResultSet();
            result.Query = "select fees_due from or_table where or_no = '" + txtORNo.Text.Trim() + "' and fees_code like 'B%'"; // AST 20160125 rem
            //result.Query = "select fees_amtdue from or_table where or_no = '" + txtORNo.Text.Trim() + "' and fees_code like 'B%'"; // AST 20160125
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all ";
            result.Query += " select fees_due from or_table where trim(or_no) not like '" + txtORNo.Text.Trim() + "' and substr(trim(fees_code),1,1) = 'B'"; // AST 20160125 rem
            //result.Query += " select fees_amtdue from or_table where trim(or_no) not like '" + txtORNo.Text.Trim() + "' and substr(trim(fees_code),1,1) = 'B'"; // AST 20160125 
            result.Query += " and or_no in (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if(result.Execute())
            {
                while(result.Read())
                {
                    dTax = result.GetDouble("fees_due");
                    strTotalTax = string.Format("{0:#,##0.0#}", double.Parse(strTotalTax) + dTax);
                }
                txtBTax.Text = strTotalTax;
            }
            result.Close();

            result.Query = "select fees_due from or_table where or_no = '" + txtORNo.Text.Trim() + "' and fees_code <> 'B'"; // AST 20160125 rem
            //result.Query = "select fees_amtdue from or_table where or_no = '" + txtORNo.Text.Trim() + "' and fees_code <> 'B'"; // AST 20160125
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all";
            result.Query += " select fees_due from or_table where trim(or_no) not like '" + txtORNo.Text.Trim() + "' and substr(trim(fees_code),1,1) <> 'B'"; // AST 20160125 rem
            //result.Query += " select fees_amtdue from or_table where trim(or_no) not like '" + txtORNo.Text.Trim() + "' and substr(trim(fees_code),1,1) <> 'B'"; // AST 20160125
            result.Query += " and or_no in (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)

            if (result.Execute())
            {
                while (result.Read())
                {
                    dFees = result.GetDouble("fees_due");
                    strTotalFees = string.Format("{0:#,##0.00}", double.Parse(strTotalFees) + dFees);
                }
                txtFees.Text = strTotalFees;
            }
            result.Close();

            double dblTmpSurch = 0; // AST 20160125
            double dblTmpPen = 0; // AST 20160125

            result.Query = "select sum(fees_surch) as fSurch, sum(fees_pen) as fPen from or_table where or_no = '" + txtORNo.Text.Trim() + "'";
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all ";
            result.Query += " select fees_surch,fees_pen from or_table where or_no in (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if (result.Execute())
            {
                //if (result.Read())
                while (result.Read())
                {
                    try
                    {
                        dSurch = result.GetDouble("fSurch");
                    }
                    catch (System.Exception ex)
                    {
                        dSurch = 0;
                    }

                    try
                    {
                        dInt = result.GetDouble("fPen");
                    }
                    catch (System.Exception ex)
                    {
                        dInt = 0;
                    }
                    
                    strTotalSurch = string.Format("{0:#,##0.00}", dSurch);
                    strTotalInt = string.Format("{0:#,##0.00}", dInt);

                    dblTmpSurch += dSurch;
                    dblTmpPen += dInt;
                }
                //txtSurch.Text = strTotalSurch; // AST 20160125 rem
                //txtPen.Text = strTotalInt; // AST 20160125 rem
            }
            result.Close();

            // AFM 20200721 MAO-20-13350 (s)
            result.Query = "select balance from dbcr_memo where or_no = '" + txtORNo.Text.Trim() + "' and memo like 'DEBITED THRU PAYMENT MADE HAVING OR_NO%'";
            if(result.Execute())
                if(result.Read())
                    txtTaxCredit.Text = string.Format("{0:#,##0.00}", result.GetDouble("balance") * -1);
            result.Close();
            // AFM 20200721 MAO-20-13350 (e)

            txtSurch.Text = string.Format("{0:#,##0.00}", dblTmpSurch); // AST 20160125
            txtPen.Text = string.Format("{0:#,##0.00}", dblTmpPen); // AST 20160125

            txtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(txtBTax.Text.Trim()) + double.Parse(txtFees.Text.Trim()) + double.Parse(txtSurch.Text.Trim()) + double.Parse(txtPen.Text.Trim()));
            try
            {
                txtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(txtTotal.Text.Trim()) - double.Parse(txtTaxCredit.Text.Trim())); // AFM 20200721 MAO-20-13350
            }
            catch { }
        }

        private void btnCancelPayment_Click(object sender, EventArgs e)
        {
            if (btnOther.Checked == true)
            {
                if (txtOthers.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("You must enter a reason for cancellation");
                    txtOthers.Focus();
                    return;
                }
                else
                    m_strReason = txtOthers.Text.Trim();
            }

            CancelPayment();
        }

        private void CancelPayment()
        {
            OracleResultSet result = new OracleResultSet();
            if(txtORNo.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Enter O.R. No. to be cancelled.", "Cancel Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtORNo.Focus();
                return;
            }

            // RMC 20151229 merged multiple OR use from Mati (s)
		    string sORNo = string.Empty;
		    result.Query+= " select distinct or_no from pay_hist where or_no in ";
		    result.Query+= " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%"+txtORNo.Text.Trim()+"%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sORNo += result.GetString(0) + "\n";
                }
            }
            result.Close();
		
		    if(sORNo != "")
		    {
                if (MessageBox.Show("Cancelling this O.R. will also cancel the following O.R.:\n" + sORNo + "\ndue to Multiple O.R. used during transaction.\nContinue?", "Cancel Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				    return;
		    }
		    // RMC 20151229 merged multiple OR use from Mati (e)

            if(MessageBox.Show("Do you really want to cancel this Payment?","Cancel Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strUserCode = string.Empty;
                fLogIn = new frmLogIn();
                fLogIn.ShowDialog();
                strUserCode = fLogIn.m_sUserCode.Trim();
                if (strUserCode.Trim() != string.Empty)
                {
                    if (AppSettingsManager.Granted("CTCPA") == false)
                    {
                        MessageBox.Show("Access Denied.");
                        return;
                    }
                    else
                    {
                        //pApp->AuditTrail("CTCPA", "trail_table/a_trail", pApp->sUser + "/" + mv_sORNo); 
                    }
                }
                else
                {
                    return;
                }

                result.Query = "select * from transfer_table where bin = '" + strBin.Trim() + "'";
                if(result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("Has pending application for Transfer");
                        return;
                    }
                    else
                    {

                        //AFM 20211220 remove mayor's approval (s)
                        OracleResultSet res2 = new OracleResultSet();
                        string sBin = string.Empty;
                        string sTaxYear = string.Empty;
                        string sQtr = string.Empty;
                        result.Query = "select * from pay_hist where or_no = '" + txtORNo.Text + "'";
                        if (result.Execute())
                            if (result.Read())
                            {
                                sBin = result.GetString("bin");
                                sQtr = result.GetString("qtr_paid");
                                sTaxYear = result.GetString("tax_year");
                            }
                        result.Close();

                        if (sQtr == "1")
                        {
                            result.Query = "DELETE FROM BUSINESS_APPROVAL WHERE BIN = '" + sBin + "' AND TAX_YEAR = '" + sTaxYear + "'";
                            if (result.ExecuteNonQuery() != 0)
                            { }
                            result.Close();
                        }
                        //AFM 20211220 remove mayor's approval (e)

                        CancelBusRecPayment();
                    }
                }
                result.Close();

                //result.Query = "update teller_transaction set transaction_code = 'CNL', dt_save = '" + string.Format("{0:dd-MMM-yyyy}", AppSettingsManager.GetSystemDate()) + "' where or_no = '" + txtORNo.Text.Trim() + "'"; // AST 20160120 changed date format
                result.Query = string.Format("update teller_transaction set transaction_code = 'CNL', dt_save = to_date('{0}', 'MM/dd/yyyy') where or_no = '{1}'", AppSettingsManager.GetSystemDate().ToShortDateString(), txtORNo.Text.Trim()); // AST 20160120 changed date format
                if(result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();

                // RMC 20151229 merged multiple OR use from Mati (s)
                //result.Query = "update teller_transaction set transaction_code  = 'CNL', dt_save = '" + string.Format("{0:dd-MMM-yyyy}", AppSettingsManager.GetSystemDate()) + "' "; // AST 20160120 changed date format
                result.Query = string.Format("update teller_transaction set transaction_code  = 'CNL', dt_save = to_date('{0}', 'MM/dd/yyyy') ", AppSettingsManager.GetSystemDate().ToShortDateString()); // AST 20160120 changed date format
                result.Query += " where or_no in ";
                result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                if (result.ExecuteNonQuery() != 0)
                {}
                result.Close();
                // RMC 20151229 merged multiple OR use from Mati (e)


                AuditTrail.InsertTrail("CTCP", "multiple tables", txtORNo.Text.Trim() + "/" + strUserCode); //AFM 20220106

                //pApp->AuditTrail("CTCP", "multiple tables", mv_sORNo);
                MessageBox.Show("Payments Cancelled.");
                ClearForm();
                return;
                
            }
            else
            {
                ClearForm();
                return;
            }
        }

        private void CancelBusRecPayment()
        {
            string sMsg = string.Empty;
            string sIsQuarterlyDec = string.Empty;
            string sTBIN = string.Empty;
            string sTTaxYear = string.Empty;
            string sTQtrToPay = string.Empty;
            string sTBnsCodeMain = string.Empty;
            string sTTaxCode = string.Empty;
            string sTAmount = "0";
            string sTDueState = string.Empty;
            string sTORNo = string.Empty;
            double dAmount = 0;

            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            result.Query = "select is_qtrly from new_table where rev_year = '" + AppSettingsManager.GetConfigObject("07") + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sIsQuarterlyDec = result.GetString("is_qtrly").Trim();
                }
            }
            result.Close();

            result.Query = "select * from taxdues_last where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all select * from taxdues_last where trim(bin) = '" + strBin.Trim() + "' and or_no in ";
            result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if (result.Execute())
            {
                if (result.Read())
                {
                    result2.Query = "delete from taxdues where bin = '" + strBin.Trim() + "'";
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();
                }
            }
            result.Close();

            result.Query = "select * from taxdues_last where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all select * from taxdues_last where trim(bin) = '" + strBin.Trim() + "' and or_no in ";
            result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    sTBIN = result.GetString("bin").Trim();
                    sTTaxYear = result.GetString("tax_year").Trim();
                    sTQtrToPay = result.GetString("qtr_to_pay").Trim();
                    sTBnsCodeMain = result.GetString("bns_code_main").Trim();
                    sTTaxCode = result.GetString("tax_code").Trim();
                    dAmount = result.GetDouble("amount");
                    sTDueState = result.GetString("due_state").Trim();

                    result2.Query = "insert into taxdues(bin,tax_year,qtr_to_pay,bns_code_main,tax_code,amount,due_state) values ";
                    result2.Query += "('" + sTBIN + "', ";
                    result2.Query += " '" + sTTaxYear + "', ";
                    result2.Query += " '" + sTQtrToPay + "', ";
                    result2.Query += " '" + sTBnsCodeMain + "', ";
                    result2.Query += " '" + sTTaxCode + "', ";
                    result2.Query += dAmount + ", ";
                    result2.Query += " '" + sTDueState + "') ";
                    if (result2.ExecuteNonQuery() != 0)
                    {
                    }
                    result2.Close();
                }
            }
            result.Close();

            string sBillNo = string.Empty;
            string sBillDate = string.Empty;
            string sBillUser = string.Empty;
            string sGracePeriod = string.Empty;
            string sReceivedDate = string.Empty;
            string sReceivedBy = string.Empty;
            int iCount = 0;
            result = new OracleResultSet();

            result.Query = "select * from bill_no_last where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all select * from bill_no_last where bin = '" + strBin.Trim() + "' and or_no in ";
            result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if (result.Execute())
            {
                if (result.Read())
                {
                    result2.Query = "delete from bill_no where bin = '" + strBin.Trim() + "'";
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();
                }
                else
                {
                    string sBillBin = string.Empty;
                    string sBillTaxYear = string.Empty;
                    string sBillPaymentTerm = string.Empty;
                    string sBillQtrPaid = string.Empty;
                    string sBillBnsStat = string.Empty;
                    string sBillDueState = string.Empty;

                    //result2.Query = "select distinct * from pay_hist where or_no = '" + txtORNo.Text.Trim() + "' order by tax_year, qtr_paid";
                    // RMC 20151229 merged multiple OR use from Mati (s)
                    result2.Query = "select bin,tax_year,payment_term,qtr_paid,bns_stat from pay_hist where or_no = '" + txtORNo.Text.Trim() + "'";
                    result2.Query += " union all select bin,tax_year,payment_term,qtr_paid,bns_stat from pay_hist where or_no in ";
                    result2.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                    // RMC 20151229 merged multiple OR use from Mati (e)
                    if (result2.Execute())
                    {
                        while (result2.Read())
                        {
                            sBillBin = result2.GetString("bin").Trim();
                            sBillTaxYear = result2.GetString("tax_year").Trim();
                            sBillPaymentTerm = result2.GetString("payment_term").Trim();
                            sBillQtrPaid = result2.GetString("qtr_paid").Trim();
                            sBillBnsStat = result2.GetString("bns_stat").Trim();

                            if (sBillBnsStat == "NEW")
                            {
                                if (sBillPaymentTerm == "F")
                                    sBillDueState = "N";
                                else
                                    sBillDueState = "Q";
                            }

                            if (sBillBnsStat == "REN")
                            {
                                sBillDueState = "R";
                                if (sBillPaymentTerm == "F")
                                    sBillQtrPaid = "1";
                            }

                            if (sBillBnsStat == "RET")
                            {
                                sBillDueState = "X";
                                if (sBillPaymentTerm == "F")
                                    sBillQtrPaid = "1";
                            }

                            if (sBillDueState == "N" && sIsQuarterlyDec == "N")
                            {
                                result3.Query = "delete from bill_no where bin = '" + sBillBin.Trim() + "' and tax_year = '" + sBillTaxYear.Trim() + "' and due_state = '" + sBillDueState.Trim() + "'";
                                if (result3.ExecuteNonQuery() != 0)
                                {

                                }
                                result3.Close();

                                result3.Query = "insert into bill_no select * from bill_hist where bin = '" + sBillBin.Trim() + "' and tax_year = '" + sBillTaxYear.Trim() + "' and due_state = '" + sBillDueState.Trim() + "' and qtr = '" + sBillQtrPaid.Trim() + "'";
                                if (result3.ExecuteNonQuery() != 0)
                                {

                                }
                                result3.Close();

                            }
                            else
                            {
                                result3.Query = "delete from bill_no where bin = '" + sBillBin.Trim() + "' and tax_year = '" + sBillTaxYear.Trim() + "' and due_state = '" + sBillDueState.Trim() + "'";
                                if (result3.ExecuteNonQuery() != 0)
                                {

                                }
                                result3.Close();

                                result3.Query = "insert into bill_no select * from bill_hist where bin = '" + sBillBin.Trim() + "' and tax_year = '" + sBillTaxYear.Trim() + "' and due_state = '" + sBillDueState.Trim() + "' and qtr = '" + sBillQtrPaid.Trim() + "'";
                                if (result3.ExecuteNonQuery() != 0)
                                {

                                }
                                result3.Close();
                            }


                        }
                    }
                    result2.Close();
                }
            }
            result.Close();

            result.Query = "select * from bill_no_last where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all select * from bill_no_last where bin = '" + strBin.Trim() + "' and or_no in ";
            result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    sTBIN = result.GetString("bin").Trim();
                    sTTaxYear = result.GetString("tax_year").Trim();
                    sTQtrToPay = result.GetString("qtr").Trim();
                    sTBnsCodeMain = result.GetString("bns_code_main").Trim();
                    sTDueState = result.GetString("due_state").Trim();
                    sBillNo = result.GetString("bill_no").Trim();
                    sBillDate = result.GetDateTime("bill_date").ToShortDateString();
                    sBillUser = result.GetString("bill_user").Trim();
                    sGracePeriod = result.GetInt("grace_period").ToString();
                    sReceivedDate = result.GetDateTime("received_date").ToShortDateString();
                    sReceivedBy = result.GetString("received_by").Trim();

                    sBillDate = string.Format("{0:dd-MMM-yy}", DateTime.Parse(sBillDate));
                    sReceivedDate = string.Format("{0:dd-MMM-yy}", DateTime.Parse(sReceivedDate));
                    result2.Query = "insert into bill_no values ";
                    result2.Query += "('" + sTBIN + "', ";
                    result2.Query += " '" + sTTaxYear + "', ";
                    result2.Query += " '" + sTBnsCodeMain + "', ";
                    result2.Query += " '" + sBillNo + "', ";
                    //result2.Query += " '" + DateTime.Parse(sBillDate).ToShortDateString() + "', "; // AST 20160118 rem
                    result2.Query += " to_date('" + DateTime.Parse(sBillDate).ToShortDateString() + "', 'MM/dd/yyyy'), "; // AST 20160118 
                    result2.Query += " '" + sBillUser + "', ";
                    result2.Query += sGracePeriod + ", ";
                    //result2.Query += " '" + DateTime.Parse(sReceivedDate).ToShortDateString() + "', ";
                    result2.Query += " to_date('" + DateTime.Parse(sReceivedDate).ToShortDateString() + "', 'MM/dd/yyyy'), ";
                    result2.Query += " '" + sReceivedBy + "', ";
                    result2.Query += " '" + sTQtrToPay + "', ";
                    result2.Query += " '" + sTDueState + "')";
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();
                }
            }
            result.Close();


            result.Query = "insert into cancelled_payment select distinct * from pay_hist where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all select * from pay_hist where trim(bin) = '" + strBin.Trim() + "' and or_no in ";
            result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            if (m_strReason.Trim() == string.Empty)
                m_strReason = "INSUFFICIENT FUND";

            result.Query = "update cancelled_payment set memo = '" + StringUtilities.HandleApostrophe(m_strReason.Trim()) + "' where bin = '" + strBin.Trim() + "'"; // RMC 20151229 merged multiple OR use from Mati, added handelapostrophe
            result.Query += " and or_no = '" + txtORNo.Text.Trim() + "'";	// RMC 20151229 merged multiple OR use from Mati
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query = "update cancelled_payment set memo = '" + StringUtilities.HandleApostrophe(m_strReason.Trim()) + "' where bin = '" + strBin.Trim() + "'";
            result.Query += " and or_no in (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();
            // RMC 20151229 merged multiple OR use from Mati (e)

            // AFM 20200129 merged from LUNA (s)
            // JAA 20200129 for multiple OR (s) 
            OracleResultSet res = new OracleResultSet();
            res.Query = "select * from pay_hist where bin = '" + strBin.Trim() + "' and memo like '%" + txtORNo.Text.Trim() + "%' order by tax_year desc";
            if (res.Execute())
            {
                if (res.Read())
                {
                    result.Query = "delete from pay_hist where bin = '" + strBin.Trim() + "' and memo like '%" + txtORNo.Text.Trim() + "%'";
                    result.ExecuteNonQuery();
                    result.Close();
                }
                // JAA 20200129 for multiple OR (e)
                else
                {
                    result.Query = "delete from pay_hist where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                    result.Close();
                }
            }
            // AFM 20200129 merged from LUNA (e)

            //result.Query = "delete from pay_hist where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
            //if (result.ExecuteNonQuery() != 0)
            //{

            //}
            //result.Close();

            result.Query = "insert into cancelled_or select * from or_table where or_no = '" + txtORNo.Text.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            result.Query = "delete from or_table where or_no = '" + txtORNo.Text.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query = "insert into cancelled_or select * from or_table where or_no in ";
            result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            result.Query = "delete from or_table where or_no in ";
            result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();
            // RMC 20151229 merged multiple OR use from Mati (e) 

            int iCompTermToPay = 0;
            int iCompTermToPayLast = 0;

            #region create comp_due_last table
            /*
            result.Query = "select * from comp_payhist where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "' order by pay_no";
            if (result.Execute())
            {
                if (result.Read())
                {
                    // GDE 20110413
                    result2.Query = "select * from comp_due_last where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
                    if(result2.Execute())
                    {
                        if(result2.Read())
                        {
                            iCompTermToPayLast = result2.GetInt("term_to_pay");
                            iCompTermToPay = iCompTermToPayLast;

                            result3.Query = "select * from compromise_due where bin = '" + strBin.Trim() + "'";
                            if(result3.Execute())
                            {
                                if(result3.Read())
                                {
                                    iCompTermToPay = result3.GetInt("term_to_pay");
                                    result3.Close();

                                    result3.Query = "delete from compromise_due where bin = '" + strBin.Trim() + "'";
                                    if(result3.ExecuteNonQuery() != 0)
                                    {

                                    }
                                }
                                else
                                {
                                    result3.Close();
                                    result3.Query = "insert into compromise_due select * from compromise_hist";
                                    if(result3.ExecuteNonQuery() != 0)
                                    {

                                    }
                                }
                            }
                            result3.Close();

                            result3.Query = "insert into compromise_due select * from comp_due_last where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
                            if(result3.ExecuteNonQuery() != 0)
                            {

                            }
                            result3.Close();

                            if(iCompTermToPay < iCompTermToPayLast)
                            {
                                result3.Query = "update compromise_due set term_to_pay = '" + iCompTermToPay + "' where bin = '" + strBin.Trim() + "'";
                                if(result3.ExecuteNonQuery() != 0)
                                {

                                }
                                result3.Close();
                            }
                        }
                    }
                    result2.Close();
                    // GDE 20110413

                    MessageBox.Show("Please create comp_due_last table.");
                }
                else
                {
                    // GDE 20110414 ask sir allan about this table (s){
                    result2.Query = "select * from compromise_due where bin = '" + strBin.Trim() + "'";
                    if(result2.Execute())
                    {
                        if(result2.Read())
                        {
                            string sFormat = string.Empty;
                            int iTerm = 0;

                            iTerm = result2.GetInt("pay_no");

                            if(iTerm == 0)
                            {
                                result3.Query = "update compromise_due set term_to_pay = '" + iTerm + "', pay_sw = 'N' where bin = '" + strBin.Trim() + "'";
                                if(result3.ExecuteNonQuery() != 0)
                                {

                                }
                                result3.Close();
                            }
                            else
                            {
                                result3.Query = "update compromise_due set term_to_pay = '" + iTerm + "' where bin = '" + strBin.Trim() + "'";
                                if(result3.ExecuteNonQuery() != 0)
                                {

                                }
                                result3.Close();
                            }
                        }
                        else
                        {
                            int iPay = 0;
                            result3.Query = "select * from comp_due_hist where bin = '" + strBin.Trim() + "'";
                            if(result3.Execute())
                            {
                                if(result3.Read())
                                {
                                    result3.Close();

                                    result3.Query = "select * from compromise_hist where bin = '" + strBin.Trim() + "'";
                                    if(result3.Execute())
                                    {
                                        if(result3.Read())
                                        {
                                            iPay = result3.GetInt("no_pay_seq");
                                        }
                                    }
                                    result3.Close();

                                    result3.Query = "insert into compromise_tbl select * from compromise_hist where bin = '" + strBin.Trim() + "'";
                                    if(result3.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result3.Close();
                                    
                                    result3.Query = "insert into compromise_due select * from comp_due_hist where bin = '" + strBin.Trim() + "'";
                                    if(result3.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result3.Close();

                                    result3.Query = "update compromise_due set term_to_pay = '" + iPay + "' where bin = '" + strBin.Trim() + "'";
                                    if(result3.ExecuteNonQuery() != 0)
                                    {

                                    }
                                }
                            }
                            result3.Close();
                        }
                    }
                    result2.Close();
                    // GDE 20110414 ask sir allan about this table (e)
                }

                result2.Query = "delete from comp_payhist where bin = '" + strBin.Trim() + "' and or_no = '" + txtORNo.Text.Trim() + "'";
                if (result2.ExecuteNonQuery() != 0)
                {

                }
                result2.Close();
            }
            result.Close();
            */
            #endregion

            result.Query = "insert into cancelled_chk select * from chk_tbl where or_no = '" + txtORNo.Text.Trim() + "'";
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all select * from chk_tbl where or_no in ";
            result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            //string[] sArrMultChkNo = new string[];
            //string sArrChkNo = string.Empty;
            //string sArrChkNo2 = string.Empty;
            List<string> chkList = new List<string>();
            //int i = 0;
            result.Query = "select chk_no from chk_tbl where chk_no in (select distinct chk_no from multi_check_pay) and or_no = '" + txtORNo.Text.Trim() + "'";
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all select chk_no from chk_tbl where chk_no in (select distinct chk_no from multi_check_pay) and or_no in ";
            result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    chkList.Add(result.GetString("chk_no").Trim());
                }
            }
            result.Close();

            result.Query = "delete from chk_tbl where or_no = '" + txtORNo.Text.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query = "delete from chk_tbl where or_no in ";
            result.Query += " (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();
            // RMC 20151229 merged multiple OR use from Mati (e)



            int iChkCount = 0;
            iChkCount = chkList.Count;
            int iChkFound = 0;

            if (iChkCount > 0)
            {
                for (int i = 0; i < iChkCount; ++i)
                {
                    result.Query = "select count(*) as ChkCnt from chk_tbl where chk_no = '" + chkList[i].ToString() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            iChkFound = result.GetInt("ChkCnt");
                        }
                        if (iChkFound == 0)
                        {
                            result2.Query = "insert into multi_check_hist select * from multi_check_pay where chk_no = '" + chkList[i].ToString() + "'"; //AFM 20200117 multi_check_list CORRECTION in table name
                            if (result2.ExecuteNonQuery() != 0)
                            {

                            }
                            result2.Close();

                            result2.Query = "delete from multi_check_pay where chk_no = '" + chkList[i].ToString() + "'";
                            if (result2.ExecuteNonQuery() != 0)
                            {

                            }
                            result2.Close();
                        }
                    }
                    result.Close();
                }
            }

            result.Query = "delete from bounce_chk_rec where or_no = '" + txtORNo.Text.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query = "delete from bounce_chk_rec where or_no in ";
            result.Query += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();
            // RMC 20151229 merged multiple OR use from Mati (e)

            string sLastHistYear = string.Empty;
            result.Query = "select * from buss_hist where bin = '" + strBin.Trim() + "' order by tax_year desc";
            if (result.Execute())
            {
                if (result.Read())
                    sLastHistYear = result.GetString("tax_year").Trim();
            }
            result.Close();

            result.Query = "select distinct * from pay_hist where bin = '" + strBin.Trim() + "' and tax_year = '" + m_strTaxYear.Trim() + "' and bns_stat = '" + m_strBnsStat.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {

                }
                else
                {
                    result2.Query = "select * from business_que where bin = '" + strBin.Trim() + "'";
                    if (result2.Execute())
                    {
                        if (result2.Read())
                        {

                        }
                        else
                        {
                            result3.Query = "insert into business_que select * from businesses where bin = '" + strBin.Trim() + "'";
                            if (result3.ExecuteNonQuery() != 0)
                            {

                            }
                            result3.Close();

                            result3.Query = "update business_que set permit_no = '" + string.Empty + "' where bin = '" + strBin.Trim() + "'"; // GDE 20110614 added
                            if (result3.ExecuteNonQuery() != 0)
                            {

                            }
                            result3.Close();
                        }
                    }
                    result2.Close();

                    result2.Query = "delete from businesses where bin = '" + strBin.Trim() + "' and tax_year = '" + sTTaxYear.Trim() + "'"; // GDE 20110614 add tax_year
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();

                    if (sLastHistYear.Trim() != string.Empty)
                    {
                        try
                        {
                            result2.Query = "insert into businesses select * from buss_hist where bin = '" + strBin.Trim() + "'  and tax_year = '" + sLastHistYear.Trim() + "'";
                            if (result2.ExecuteNonQuery() != 0)
                            {

                            }
                            result2.Close();
                        }
                        catch { }

                        result2.Query = "delete from buss_hist where bin = '" + strBin.Trim() + "' and tax_year = '" + sLastHistYear.Trim() + "'";
                        if (result2.ExecuteNonQuery() != 0)
                        {

                        }
                        result2.Close();
                    }
                }
            }
            result.Close();


            string sRetBin = string.Empty;
            string sRetBnsCode = string.Empty;
            string sRetQtr = string.Empty;
            string sRetMain = string.Empty;
            string sOldBnsStat = string.Empty;
            string sOldTaxYear = string.Empty;

            result.Query = "select * from retired_bns where or_no = '" + txtORNo.Text.Trim() + "'";
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += " union all select * from retired_bns where or_no in ";
            result.Query += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    sRetBin = result.GetString("bin").Trim();
                    sRetQtr = result.GetString("qtr").Trim();
                    sRetBnsCode = result.GetString("bns_code_main").Trim();
                    sRetMain = result.GetString("main").Trim();

                    if (sRetMain == "Y")
                    {
                        result2.Query = "select distinct * from pay_hist where bin = '" + strBin.Trim() + "' and bns_stat <> 'RET' order by tax_year desc";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                sOldTaxYear = result2.GetString("tax_year").Trim();
                                sOldBnsStat = result2.GetString("bns_stat").Trim();

                            }
                        }
                        result2.Close();

                        result2.Query = "update businesses set bns_code = '" + sRetBnsCode.Trim() + "', bns_stat = '" + sOldBnsStat.Trim() + "', tax_year = '" + sOldTaxYear.Trim() + "' where bin = '" + strBin.Trim() + "'";
                        if (result2.ExecuteNonQuery() != 0)
                        {

                        }
                        result2.Close();
                    }
                    else
                    {
                        //result2.Query = "insert into addl_bns select bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr from addl_bns_hist where bin = '" + strBin.Trim() + "' and bns_code_main = '" + sRetBnsCode.Trim() + "'";
                        result2.Query = "insert into addl_bns select bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr, prev_gross from addl_bns_hist where bin = '" + strBin.Trim() + "' and bns_code_main = '" + sRetBnsCode.Trim() + "'";  // RMC 20110823
                        if (result2.ExecuteNonQuery() != 0)
                        {

                        }
                        result2.Close();

                        result2.Query = "delete from addl_bns_hist where bin = '" + strBin.Trim() + "' and bns_code_main = '" + sRetBnsCode.Trim() + "'";
                        if (result2.ExecuteNonQuery() != 0)
                        {

                        }
                        result2.Close();
                    }

                    result2.Query = "insert into retired_bns_temp select bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,memoranda,main,apprvd_date,app_no,or_no from retired_bns where bin = '" + strBin.Trim() + "' and bns_code_main = '" + sRetBnsCode.Trim() + "'";
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();

                    result2.Query = "delete from retired_bns where bin = '" + strBin.Trim() + "' and bns_code_main = '" + sRetBnsCode.Trim() + "'";
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();
                }
            }
            result.Close();

            #region CREATE permit_update_appl table
            string sQuery;
            result.Query = "select * from permit_update_appl where or_no = '" + txtORNo.Text.Trim() + "'";
            // RMC 20151229 merged multiple OR use from Mati (s)
            result.Query += "union all select * from permit_update_appl where or_no in ";
            result.Query += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            if(result.Execute())
            {
                string sBin, sApplType, sNewBnsCode, sTaxYear, sOldOwnCode, sNewOwnCode,sOrNo;
                bool bIsRenPUT = false; 

                while (result.Read())
                {
                    sBin = result.GetString("bin");
                    sTaxYear = result.GetString("tax_year");
                    sApplType = result.GetString("appl_type");
                    sNewBnsCode = result.GetString("new_bns_code");
                    sOrNo = result.GetString("or_no");  // RMC 20151229 merged multiple OR use from Mati

                    if (AppSettingsManager.bIsSplBns(sBin) == true)
                        result2.Query = "select * from spl_business_que where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "'";
                    else
                        result2.Query = "select * from business_que where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "'";

                    if (result2.Execute())
                    {
                        if (result2.Read())
                            bIsRenPUT = true;
                        else
                            bIsRenPUT = false;
                    }
                    result2.Close();
                    

                    if (sApplType == "ADDL")
                    {
                        result2.Query = "insert into addl_bns_que";
                        result2.Query += " select * from addl_bns where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and bns_code_main = '" + sNewBnsCode + "'";
                        result2.ExecuteNonQuery();
                        result2.Close();

                        result2.Query = "delete from addl_bns where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and bns_code_main = '" + sNewBnsCode + "'";
                        result2.ExecuteNonQuery();
                        result2.Close();

                    }
                    if (sApplType == "TLOC")
                    {
                        result2.Query = "insert into transfer_table";
                        result2.Query += " select bin,trans_app_code,bns_code,prev_own_code,new_own_code,own_ln,own_fn,own_mi,";
                        result2.Query += "addr_no,addr_street,addr_brgy,addr_zone,addr_dist,addr_mun,addr_prov,app_date";
                        result2.Query += " from transfer_hist where or_no ='" + txtORNo.Text.Trim() + "' and trans_app_code = 'TL'";
                        result2.ExecuteNonQuery();
                        result2.Close();

                        result2.Query = "delete from transfer_hist where or_no = '" + txtORNo.Text.Trim() + "' and trans_app_code = 'TL'";
                        result2.ExecuteNonQuery();
                        result2.Close();

                        result2.Query = "delete from transfer_hist where or_no in ";
                        result2.Query += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        result2.Query += " and trans_app_code = 'TL'";
                        result2.ExecuteNonQuery();
                        result2.Close();

                        String sLastOwnCode, sLastBusnOwn, sLastPrevBnsOwn, sLastAddrNo, sLastAddrStreet, sLastAddrBrgy, sLastAddrZone, sLastAddrDist, sLastAddrMun, sLastAddrProv;
                        sQuery = " select * from bns_loc_last where or_no = '" + txtORNo.Text.Trim() + "' and trans_app_code = 'TL'";
                        // RMC 20151229 merged multiple OR use from Mati (s)
                        sQuery += " union all select * from bns_loc_last where or_no in ";
                        sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        sQuery += " and trans_app_code = 'TL'";
                        // RMC 20151229 merged multiple OR use from Mati (e)
                        result2.Query = sQuery;
                        if (result2.Execute())
                            if (result2.Read())
                            {
                                sLastAddrNo = result2.GetString("last_addr_no").Trim();
                                sLastAddrStreet = result2.GetString("last_addr_street").Trim();
                                sLastAddrBrgy = result2.GetString("last_brgy").Trim();
                                sLastAddrZone = result2.GetString("last_zone").Trim();
                                sLastAddrDist = result2.GetString("last_dist").Trim();
                                sLastAddrMun = result2.GetString("last_mun").Trim();
                                sLastAddrProv = result2.GetString("last_prov").Trim();

                                if (AppSettingsManager.bIsSplBns(sBin) == true)
                                    sQuery = "update spl_businesses set";
                                else
                                    sQuery = "update businesses set";
                                if (bIsRenPUT == true)
                                    if (AppSettingsManager.bIsSplBns(strBin) == true)
                                        sQuery = "update spl_business_que set";
                                    else
                                        sQuery = "update business_que set";

                                sQuery += " bns_house_no = '" + StringUtilities.HandleApostrophe(sLastAddrNo) + "',";
                                sQuery += " bns_street = '" + StringUtilities.HandleApostrophe(sLastAddrStreet) + "',";
                                sQuery += " bns_brgy = '" + StringUtilities.HandleApostrophe(sLastAddrBrgy) + "',";
                                sQuery += " bns_zone = '" + StringUtilities.HandleApostrophe(sLastAddrZone) + "',";
                                sQuery += " bns_dist = '" + StringUtilities.HandleApostrophe(sLastAddrDist) + "',";
                                sQuery += " bns_mun = '" + StringUtilities.HandleApostrophe(sLastAddrMun) + "',";
                                sQuery += " bns_prov = '" + StringUtilities.HandleApostrophe(sLastAddrProv) + "'";
                                sQuery += " where bin = '" + sBin + "'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                sQuery = "delete from bns_loc_last where or_no = '" + txtORNo.Text.Trim() + "' and trans_app_code = 'TL'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                sQuery = "delete from bns_loc_last where or_no in ";
                                sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                                sQuery += "and trans_app_code = 'TL'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();
                            }
                        result2.Close();
                    }
                    if (sApplType == "TOWN")
                    {
                        sQuery = "insert into transfer_table";
                        sQuery += " select bin,trans_app_code,bns_code,prev_own_code,new_own_code,own_ln,own_fn,own_mi,";
                        sQuery += "addr_no,addr_street,addr_brgy,addr_zone,addr_dist,addr_mun,addr_prov,app_date";
                        sQuery += " from transfer_hist where or_no ='" + txtORNo.Text.Trim() + "' and trans_app_code = 'TO'";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();

                        sQuery = "delete from transfer_hist where or_no = '" + txtORNo.Text.Trim() + "' and trans_app_code = 'TO'";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();

                        // RMC 20151229 merged multiple OR use from Mati (s)
                        sQuery = "insert into transfer_table";
                        sQuery += " select bin,trans_app_code,bns_code,prev_own_code,new_own_code,own_ln,own_fn,own_mi,";
                        sQuery += "addr_no,addr_street,addr_brgy,addr_zone,addr_dist,addr_mun,addr_prov,app_date";
                        sQuery += " from transfer_hist where trans_app_code = 'TO' and or_no in ";
                        sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();

                        sQuery = "delete from transfer_hist where trans_app_code = 'TO' and or_no in ";
                        sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();
                        // RMC 20151229 merged multiple OR use from Mati (e)

                        String sLastOwnCode, sLastBusnOwn, sLastPrevBnsOwn, sLastAddrNo, sLastAddrStreet, sLastAddrBrgy, sLastAddrZone, sLastAddrDist, sLastAddrMun, sLastAddrProv;
                        sQuery = " select * from bns_loc_last where or_no = '" + txtORNo.Text.Trim() + "' and trans_app_code = 'TO'";
                        // RMC 20151229 merged multiple OR use from Mati (s)
                        sQuery += " union all select * from bns_loc_last where trans_app_code = 'TO' and or_no in ";
                        sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        // RMC 20151229 merged multiple OR use from Mati (e)
                        result2.Query = sQuery;
                        if (result2.Execute())
                            if (result2.Read())
                            {
                                sLastOwnCode = result2.GetString("last_own_code").Trim();
                                sLastBusnOwn = result2.GetString("last_busn_own").Trim();
                                sLastPrevBnsOwn = result2.GetString("last_prev_bns_own").Trim();

                                if (AppSettingsManager.bIsSplBns(sBin) == true)
                                    sQuery = "update spl_businesses set";
                                else
                                    sQuery = "update businesses set";
                                if (bIsRenPUT == true)
                                    if (AppSettingsManager.bIsSplBns(sBin) == true)
                                        sQuery = "update spl_business_que set";
                                    else
                                        sQuery = "update business_que set";

                                sQuery += " own_code = '" + sLastOwnCode + "',";
                                sQuery += " busn_own = '" + sLastBusnOwn + "',";
                                sQuery += " prev_bns_own = '" + sLastPrevBnsOwn + "'";
                                sQuery += " where bin = '" + sBin + "'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                sQuery = "delete from bns_loc_last where or_no = '" + txtORNo.Text.Trim() + "' and trans_app_code = 'TO'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                // RMC 20151229 merged multiple OR use from Mati (s)
                                sQuery = "delete from bns_loc_last where trans_app_code = 'TO' and or_no in ";
                                sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();
                                // RMC 20151229 merged multiple OR use from Mati (e)
                            }
                        result2.Close();
                    }
                    if (sApplType == "CTYP")
                    {
                        // (s) ALJ 07012005 PERMIT UPDATE TRAN

                        String sIsMain, sBinCtyp, sOldBnsCode, sNewBnsType;
                        sQuery = "select * from change_class_hist";
                        sQuery += " where or_no = '" + txtORNo.Text.Trim() + "'";
                        // RMC 20151229 merged multiple OR use from Mati (s)
                        sQuery += " union all select * from change_class_hist where or_no in ";
                        sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        // RMC 20151229 merged multiple OR use from Mati (e)
                        result2.Query = sQuery;
                        if (result2.Execute())
                            while (result2.Read())
                            {
                                sBinCtyp = result2.GetString("bin").Trim();
                                sIsMain = result2.GetString("is_main").Trim();
                                sOldBnsCode = result2.GetString("old_bns_code").Trim();
                                sNewBnsType = result2.GetString("new_bns_code").Trim();
                                if (sIsMain == "Y")
                                {
                                    sQuery = "update businesses set bns_code = '" + sOldBnsCode + "'";
                                    if (bIsRenPUT == true)
                                        sQuery = "update business_que set";
                                    sQuery += " where bin = '" + sBinCtyp + "'";
                                    result2.Query = sQuery;
                                    result2.ExecuteNonQuery();
                                }
                                else // == 'N'
                                {
                                    sQuery = "insert into addl_bns_que";
                                    sQuery += " select * from addl_bns where bin = '" + sBinCtyp + "' and bns_code_main = '" + sNewBnsType + "'";
                                    result2.Query = sQuery;
                                    result2.ExecuteNonQuery();

                                    sQuery = "delete from addl_bns";
                                    sQuery += " where bin = '" + sBinCtyp + "' and bns_code_main = '" + sNewBnsType + "'";
                                    result2.Query = sQuery;
                                    result2.ExecuteNonQuery();

                                    sQuery = "insert into addl_bns";
                                    sQuery += " select bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr from addl_bns_hist";
                                    sQuery += " where bin = '" + sBinCtyp + "' and or_no = '" + txtORNo.Text + "'";
                                    // RMC 20151229 merged multiple OR use from Mati (s)
                                    sQuery += " union all select bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr from addl_bns_hist";
                                    sQuery += " where bin = '" + sBinCtyp + "' and or_no in ";
                                    sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text + "%' and or_no <> '" + txtORNo.Text + "')";
                                    // RMC 20151229 merged multiple OR use from Mati (e)
                                    result2.Query = sQuery;
                                    result2.ExecuteNonQuery();

                                    sQuery = "delete from addl_bns_hist";
                                    sQuery += " where bin = '" + sBinCtyp + "' and or_no = '" + txtORNo.Text + "'";
                                    result2.Query = sQuery;
                                    result2.ExecuteNonQuery();

                                    // RMC 20151229 merged multiple OR use from Mati (s)
                                    sQuery = "delete from addl_bns_hist";
                                    sQuery += " where bin = '" + sBinCtyp + "' and or_no in ";
                                    sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text + "%' and or_no <> '" + txtORNo.Text + "')";
                                    result2.Query = sQuery;
                                    result2.ExecuteNonQuery();
                                    // RMC 20151229 merged multiple OR use from Mati (e)
                                }
                            }
                        result2.Close();

                        sQuery = "insert into change_class_tbl";
                        sQuery += " select bin, old_bns_code, new_bns_code, capital, status, is_main, app_date";
                        sQuery += " from change_class_hist";
                        sQuery += " where or_no ='" + txtORNo.Text.Trim() + "'";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();

                        sQuery = "delete from change_class_hist where or_no = '" + txtORNo.Text.Trim() + "'";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();

                        // RMC 20151229 merged multiple OR use from Mati (s)
                        sQuery = "insert into change_class_tbl";
                        sQuery += " select bin, old_bns_code, new_bns_code, capital, status, is_main, app_date";
                        sQuery += " from change_class_hist";
                        sQuery += " where or_no in ";
                        sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();

                        sQuery = "delete from change_class_hist where or_no in ";
                        sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();
                        // RMC 20151229 merged multiple OR use from Mati (e)
                    }

                    //(s) RTL 02212006 for new business name
                    if (sApplType == "CBNS")
                    {
                        sQuery = "insert into transfer_table";
                        sQuery += " select bin,trans_app_code,bns_code,prev_own_code,new_own_code,own_ln,own_fn,own_mi,";
                        sQuery += "addr_no,addr_street,addr_brgy,addr_zone,addr_dist,addr_mun,addr_prov,app_date";
                        sQuery += " from transfer_hist where or_no ='" + txtORNo.Text.Trim() + "' and trans_app_code = 'CB'";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();

                        sQuery = "delete from transfer_hist where or_no = '" + txtORNo.Text.Trim() + "' and trans_app_code = 'CB'";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();

                        // RMC 20151229 merged multiple OR use from Mati (s)
                        sQuery = "insert into transfer_table";
                        sQuery += " select bin,trans_app_code,bns_code,prev_own_code,new_own_code,own_ln,own_fn,own_mi,";
                        sQuery += "addr_no,addr_street,addr_brgy,addr_zone,addr_dist,addr_mun,addr_prov,app_date";
                        sQuery += " from transfer_hist where trans_app_code = 'CB' and or_no in ";
                        sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();

                        sQuery = "delete from transfer_hist where trans_app_code = 'CB' and or_no in ";
                        sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();
                        // RMC 20151229 merged multiple OR use from Mati (e)

                        String sLastBnsName;
                        sQuery = " select * from bns_loc_last where or_no = '" + txtORNo.Text.Trim() + "' and trans_app_code = 'CB'";
                        // RMC 20151229 merged multiple OR use from Mati (s)
                        sQuery += " union all ";
                        sQuery = " select * from bns_loc_last where trans_app_code = 'CB' and or_no in ";
                        sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                        // RMC 20151229 merged multiple OR use from Mati (e)
                        result2.Query = sQuery;
                        if (result2.Execute())
                            if (result2.Read())
                            {
                                sLastBnsName = result2.GetString("last_addr_no");

                                if (AppSettingsManager.bIsSplBns(sBin) == true)
                                    sQuery = "update spl_businesses set";
                                else
                                    sQuery = "update businesses set";
                                if (bIsRenPUT == true)
                                    if (AppSettingsManager.bIsSplBns(sBin) == true)
                                        sQuery = "update spl_business_que set";
                                    else
                                        sQuery = "update business_que set";

                                sQuery += " bns_nm = '" + sLastBnsName + "'";
                                sQuery += " where bin = '" + sBin + "'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                sQuery = "delete from bns_loc_last where or_no = '" + txtORNo.Text.Trim() + "' and trans_app_code = 'CB'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                // RMC 20151229 merged multiple OR use from Mati (s)
                                sQuery = "delete from bns_loc_last where trans_app_code = 'CB' and or_no in ";
                                sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();
                                // RMC 20151229 merged multiple OR use from Mati (e)

                            }
                        result2.Close();
                    }
                }

                sQuery = "update permit_update_appl set data_mode = 'QUE', or_no = ' ', permit_no = 'NONE'";
                sQuery += " where or_no = '" + txtORNo.Text.Trim() + "'";
                sQuery += " and data_mode = 'PAID'";
                result2.Query = sQuery;
                result2.ExecuteNonQuery();

                // RMC 20151229 merged multiple OR use from Mati (s)
                sQuery = "update permit_update_appl set data_mode = 'QUE', or_no = ' ', permit_no = 'NONE'";
                sQuery += " where or_no in ";
                sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
                sQuery += " and data_mode = 'PAID'";
                result2.Query = sQuery;
                result2.ExecuteNonQuery();

                // RMC 20151229 merged multiple OR use from Mati (e)

                MessageBox.Show("Warning! Permit has been cancelled.", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            result.Close();
            
            #endregion

            String sDebit, sCredit, sBalance, sMemo, sServe, sORDate;
            String sCancelDate, sCancelTime;
            sCancelDate = dtDate.Value.ToString("MM/dd/yyyy");
            sCancelTime = dtDate.Value.ToString("HH:m");
            string sOwnCode;
            string sTmpORNo = string.Empty;
            sQuery = string.Format("select * from dbcr_memo where or_no = '{0}' and memo like 'CREDIT%%'", txtORNo.Text.Trim()); //JARS 20171010
            // RMC 20151229 merged multiple OR use from Mati (s)
            sQuery += " union all ";
            sQuery += " select * from dbcr_memo where memo like 'CREDIT%%' and or_no in "; //JARS 20171010
            sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            result.Query = sQuery;
            if (result.Execute())
                if (result.Read())
                {
                    String sChkNo = "", sMultiPay = "";
                    sCredit = result.GetDouble("credit").ToString();
                    sORDate = result.GetDateTime("or_date").ToString("MM/dd/yyyy");
                    sChkNo = result.GetString("chk_no").Trim();
                    sMultiPay = result.GetString("multi_pay").Trim();
                    sTmpORNo = result.GetString("or_no");	// RMC 20151229 merged multiple OR use from Mati
                    //sOwnCode = result.GetString("own_code");
                    strBin = result.GetString("bin");

                    //// RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    sOwnCode = result.GetString("own_code");
                    //// RMC 20141127 QA Tarlac ver (e)

                    ////JARS 20170713 FOR MALOLOS BPLS.NET
                    //if (AppSettingsManager.GetConfigValue("10") == "216")
                        //sOwnCode = result.GetString("own_code");

                    String sCreditLeft = "0";
                    //sQuery = string.Format("select * from dbcr_memo where or_no = '{0}' and served = 'N'", txtORNo.Text.Trim());
                    sQuery = string.Format("select * from dbcr_memo where or_no = '{0}' and served = 'N'", sTmpORNo); //JARS 20171010
                    result2.Query = sQuery;
                    if (result2.Execute())
                        if (result2.Read())
                        {
                            sCreditLeft = result2.GetDouble("balance").ToString();
                        }
                    result2.Close();

                    sDebit = sCredit;
                    sCredit = "0";
                    sBalance = Convert.ToDouble(Convert.ToDouble(sCredit) - Convert.ToDouble(sDebit)).ToString();
                    sServe = "Y";

                    // RMC 20141127 QA Tarlac ver (s)
                    if (AppSettingsManager.GetConfigValue("10") == "017")
                        strBin = AppSettingsManager.GetBnsOwnCode(strBin);
                    // RMC 20141127 QA Tarlac ver (e)

                    //sMemo = "REVERSE ENTRY AS CANCELLATION TO OR NO " + txtORNo.Text.Trim();
                    sMemo = "REVERSE ENTRY AS CANCELLATION TO OR NO " + sTmpORNo;   // RMC 20151229 merged multiple OR use from Mati
                    sQuery = "insert into dbcr_memo values "; //JARS 20171010
                    sQuery += "('" + strBin + "', ";
                    //sQuery += "('" + sOwnCode + "', "; //JARS 20170713 ADJUSTMENT FOR MALOLOS DATABASE
                    //sQuery += " '" + txtORNo.Text.Trim() + "', ";
                    sQuery += " '" + sTmpORNo + "', ";  // RMC 20151229 merged multiple OR use from Mati
                    sQuery += " to_date('" + DateTime.Parse(sORDate).ToShortDateString() + "','MM/dd/yyyy'), ";
                    sQuery += sDebit + ", ";
                    sQuery += sCredit + ", ";
                    sQuery += sBalance + ", ";
                    sQuery += " '" + sMemo + "', ";
                    sQuery += "' ',";
                    sQuery += " '" + AppSettingsManager.SystemUser.UserCode + "', ";
                    sQuery += " to_date('" + DateTime.Parse(sCancelDate).ToShortDateString() + "','MM/dd/yyyy'), ";
                    sQuery += " '" + sCancelTime + "', ";
                    sQuery += "'" + sServe + "',";
                    sQuery += "'" + sChkNo + "',";
                    sQuery += "'" + sMultiPay + "','0')";

                    if (sMultiPay == "N")
                    {
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();
                    }

                    //Added by EMS 01302003 (s)
                    //sMsg = "Credit transaction made by OR " + txtORNo.Text.Trim() + " has been reversed to debit transaction";
                    sMsg = "Credit transaction made by OR " + sTmpORNo + " has been reversed to debit transaction";  // RMC 20151229 merged multiple OR use from Mati
                    MessageBox.Show(sMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Added by EMS 01302003 (e)

                    // RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    sQuery = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}' and served = 'N'", strBin);
                    //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713 FOR MALOLOS BPLS.NET
                    //    sQuery = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}' and served = 'N'", sOwnCode);
                    //else// RMC 20141127 QA Tarlac ver (e)
                        sQuery = string.Format("update dbcr_memo set served = 'Y' where bin = '{0}' and served = 'N'", strBin); //JARS 20171010
                    result2.Query = sQuery;
                    result2.ExecuteNonQuery();

                    sBalance = Convert.ToDouble(Convert.ToDouble(sCreditLeft) - Convert.ToDouble(sDebit)).ToString();
                    if (Convert.ToDouble(sBalance) < 0)
                    {
                        sDebit = sBalance;
                        sCredit = "0";
                    }
                    else
                    {
                        sCredit = sBalance;
                        sDebit = "0";
                    }
                    sServe = "N";

                    if (sMultiPay == "Y")
                        sServe = "Y";
                    //sMemo = "REMAINING BALANCE AFTER CANCELLATION OF OR " + txtORNo.Text.Trim();
                    sMemo = "REMAINING BALANCE AFTER CANCELLATION OF OR " + sTmpORNo;    // RMC 20151229 merged multiple OR use from Mati
                    sQuery = "insert into dbcr_memo values "; //JARS 20171010
                    sQuery += "('" + strBin + "', ";
                    //sQuery += "('" + sOwnCode + "', "; //JARS 20170713 FOR MALOLOS BPLS.NET
                    //sQuery += " '" + txtORNo.Text.Trim() + "', ";
                    sQuery += " '" + sTmpORNo + "', ";   // RMC 20151229 merged multiple OR use from Mati
                    sQuery += " to_date('" + DateTime.Parse(sORDate).ToShortDateString() + "','MM/dd/yyyy'), ";
                    sQuery += sDebit + ", ";
                    sQuery += sCredit + ", ";
                    sQuery += sBalance + ", ";
                    sQuery += " '" + sMemo + "', ";
                    sQuery += "' ',";
                    sQuery += " '" + AppSettingsManager.SystemUser.UserCode + "', ";
                    sQuery += " to_date('" + DateTime.Parse(sCancelDate).ToShortDateString() + "','MM/dd/yyyy'), ";
                    sQuery += " '" + sCancelTime + "', ";
                    sQuery += "'" + sServe + "',";
                    sQuery += "'" + sChkNo + "',";
                    sQuery += "'" + sMultiPay + "','0')";

                    if (sMultiPay == "N")	// JGR 02022006 DO NOT INSERT ENTRY FOR MULTIPLE TRANS
                    {
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();
                    }
                }
            result.Close();

            sQuery = string.Format("select * from dbcr_memo where or_no = '{0}' and memo like 'DEBIT%%'", txtORNo.Text.Trim()); // CTS 09152003 c/o JJP //JARS 20171010
            // RMC 20151229 merged multiple OR use from Mati (s)
            sQuery += " union all";
            sQuery += " select * from dbcr_memo where memo like 'DEBIT%%' and or_no in "; //JARS 20171010
            sQuery += "(select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text.Trim() + "%' and or_no <> '" + txtORNo.Text.Trim() + "')";
            // RMC 20151229 merged multiple OR use from Mati (e)
            result.Query = sQuery;
            if (result.Execute())
                if (result.Read())
                {
                    String sChkNo = "", sMultiPay = ""; // CTS 12042004 add chk_no, multi_pay field
                    //sOwnCode = result.GetString("own_code");
                    strBin = result.GetString("bin");
                    sDebit = result.GetDouble("debit").ToString();
                    sORDate = result.GetDateTime("or_date").ToString("MM/dd/yyyy");
                    sChkNo = result.GetString("chk_no").Trim();
                    sMultiPay = result.GetString("multi_pay").Trim();

                    sTmpORNo = result.GetString("or_no");	// RMC 20151229 merged multiple OR use from Mati

                    String sCreditLeft = "0";
                    // RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    sQuery = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N'", strBin);
                    //else if(AppSettingsManager.GetConfigValue("10") == "216")
                    //    sQuery = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N'", sOwnCode);
                    //else// RMC 20141127 QA Tarlac ver (e)
                        sQuery = string.Format("select * from dbcr_memo where bin = '{0}' and served = 'N'", strBin); //JARS 20171010
                    result2.Query = sQuery;
                    if (result2.Execute())
                        if (result2.Read())
                        {
                            sCreditLeft = result2.GetDouble("balance").ToString();
                        }
                    result2.Close();

                    sCredit = sDebit;
                    sDebit = "0";
                    sBalance = Convert.ToDouble(Convert.ToDouble(sCredit) - Convert.ToDouble(sDebit)).ToString();
                    sServe = "Y";
                    //sMemo = "REVERSE ENTRY AS CANCELLATION TO OR NO " + txtORNo.Text.Trim();
                    sMemo = "REVERSE ENTRY AS CANCELLATION TO OR NO " + sTmpORNo;    // RMC 20151229 merged multiple OR use from Mati
                    sQuery = "insert into dbcr_memo values "; //JARS 20171010
                    sQuery += "('" + strBin + "', ";
                    //sQuery += "('" + sOwnCode + "', "; //JARS 20170713
                    //sQuery += " '" + txtORNo.Text.Trim() + "', ";
                    sQuery += " '" + sTmpORNo + "', ";  // RMC 20151229 merged multiple OR use from Mati
                    //sQuery += " '" + sORDate + "', ";
                    sQuery += " to_date('" + DateTime.Parse(sORDate).ToShortDateString() + "','MM/dd/yyyy'), "; // RMC 20150204 corrected error in cancel payment if with tax credit
                    sQuery += sDebit + ", ";
                    sQuery += sCredit + ", ";
                    sQuery += sBalance + ", ";
                    sQuery += " '" + sMemo + "', ";
                    sQuery += "' ',";
                    sQuery += " '" + AppSettingsManager.SystemUser.UserCode + "', ";
                    //sQuery += " '" + sCancelDate + "', ";
                    sQuery += " to_date('" + DateTime.Parse(sCancelDate).ToShortDateString() + "', 'MM/dd/yyyy'), ";    // RMC 20150204 corrected error in cancel payment if with tax credit
                    sQuery += " '" + sCancelTime + "', ";
                    sQuery += "'" + sServe + "',";
                    sQuery += "'" + sChkNo + "',";
                    sQuery += "'" + sMultiPay + "','0')";

                    if (sMultiPay == "N")	// JGR 02022006 DO NOT INSERT ENTRY FOR MULTIPLE TRANS
                    {
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();
                    }

                    //sMsg = "Debit transaction made by OR " + txtORNo.Text.Trim() + " has been reversed to credit transaction";
                    sMsg = "Debit transaction made by OR " + sTmpORNo + " has been reversed to credit transaction";  // RMC 20151229 merged multiple OR use from Mati
                    MessageBox.Show(sMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    sQuery = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}' and served = 'N'", strBin);
                    //else if (AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                    //    sQuery = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}' and served = 'N'", sOwnCode);
                    //else// RMC 20141127 QA Tarlac ver (e)
                        sQuery = string.Format("update dbcr_memo set served = 'Y' where bin = '{0}' and served = 'N'", strBin); //JARS 20171010
                    result2.Query = sQuery;
                    result2.ExecuteNonQuery();

                    sBalance = Convert.ToDouble(Convert.ToDouble(sCreditLeft) + Convert.ToDouble(sCredit)).ToString();
                    if (Convert.ToDouble(sBalance) < 0)
                    {
                        sDebit = sBalance;
                        sCredit = "0";
                    }
                    else
                    {
                        sCredit = sBalance;
                        sDebit = "0";
                    }
                    sServe = "N";

                    if (sMultiPay == "Y")
                        sServe = "Y";
                    //sMemo = "REMAINING BALANCE AFTER CANCELLATION OF OR " + txtORNo.Text.Trim();
                    sMemo = "REMAINING BALANCE AFTER CANCELLATION OF OR " + sTmpORNo;    // RMC 20151229 merged multiple OR use from Mati
                    sQuery = "insert into dbcr_memo values "; //JARS 20171010
                    sQuery += "('" + strBin + "', ";
                    //sQuery += "('" + sOwnCode + "', "; //JARS 20170713
                    //sQuery += " '" + txtORNo.Text.Trim() + "', ";
                    sQuery += " '" + sTmpORNo + "', ";   // RMC 20151229 merged multiple OR use from Mati
                    //sQuery += " '" + sORDate + "', ";
                    sQuery += " to_date('" + DateTime.Parse(sORDate).ToShortDateString() + "','MM/dd/yyyy'), "; // RMC 20150204 corrected error in cancel payment if with tax credit
                    sQuery += sDebit + ", ";
                    sQuery += sCredit + ", ";
                    sQuery += sBalance + ", ";
                    sQuery += " '" + sMemo + "', ";
                    sQuery += "' ',";
                    sQuery += " '" + AppSettingsManager.SystemUser.UserCode + "', ";
                    //sQuery += " '" + sCancelDate + "', ";
                    sQuery += " to_date('" + DateTime.Parse(sCancelDate).ToShortDateString() + "', 'MM/dd/yyyy'), ";    // RMC 20150204 corrected error in cancel payment if with tax credit
                    sQuery += " '" + sCancelTime + "', ";
                    sQuery += "'" + sServe + "',";
                    sQuery += "'" + sChkNo + "',";
                    sQuery += "'" + sMultiPay + "','0')";

                    if (sMultiPay == "N")
                    {
                        result2.Query = sQuery;
                        result2.ExecuteNonQuery();
                    }
                }
            result.Close();

            //sQuery = string.Format("select * from dbcr_memo where or_no = '{0}' and memo like 'REMAIN%%' and multi_pay = 'N'", txtORNo.Text.Trim());
            sQuery = string.Format("select * from dbcr_memo where or_no = '{0}' and memo like 'REMAIN%%' and multi_pay = 'N'", sTmpORNo);   // RMC 20151229 merged multiple OR use from Mati //JARS 20171010
            sQuery += " and chk_no in (select chk_no from multi_check_pay where used_sw = 'Y')";
            result.Query = sQuery;
            if (result.Execute())
                while (result.Read())
                {
                    String sChkNo = result.GetString("chk_no");

                    sQuery = "update dbcr_memo set "; //JARS 20171010
                    sQuery += "served = 'Y', multi_pay = 'Y' ";
                    //sQuery += "where or_no = '" + txtORNo.Text.Trim() + "' and memo like 'REMAIN%%' and served = 'N'";
                    sQuery += "where or_no = '" + sTmpORNo + "' and memo like 'REMAIN%%' and served = 'N'"; // RMC 20151229 merged multiple OR use from Mati
                    sQuery += " and chk_no = '" + sChkNo + "' ";
                    sQuery += " and chk_no in (select chk_no from multi_check_pay where used_sw = 'Y')";
                    result2.Query = sQuery;
                    result2.ExecuteNonQuery();
                    result2.Close();
                }
            result.Close();

            // RMC 20151229 merged multiple OR use from Mati (s)
            result2.Query = "delete from pay_hist where trim(bin) = '" + strBin.Trim()  + "' ";
            result2.Query += " and or_no in (select or_no from pay_hist where memo like 'MULTIPLE O.R.%" + txtORNo.Text + "%' and or_no <> '" + txtORNo.Text + "')";
            result2.ExecuteNonQuery();
            result2.Close();
            // RMC 20151229 merged multiple OR use from Mati (e)
        }

        private void frmCancelPayment_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInsufficientFund_Click(object sender, EventArgs e)
        {
            m_strReason = "INSUFFICIENT FUND";
            txtOthers.Text = string.Empty;
            txtOthers.ReadOnly = true;
        }

        private void btnCancelledCheck_Click(object sender, EventArgs e)
        {
            m_strReason = "CANCELLED CHECK";
            txtOthers.Text = string.Empty;
            txtOthers.ReadOnly = true;
        }

        private void btnWrongAcnt_Click(object sender, EventArgs e)
        {
            m_strReason = "WRONG ENTRY OF ACCT NO";
            txtOthers.Text = string.Empty;
            txtOthers.ReadOnly = true;
        }

        private void btnOther_Click(object sender, EventArgs e)
        {
            m_strReason = string.Empty;
            txtOthers.ReadOnly = false;
            txtOthers.Focus();
        }

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtDate.Value.Date > cdtToday.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtDate.Value = AppSettingsManager.GetSystemDate();
            }
        }

    }
}