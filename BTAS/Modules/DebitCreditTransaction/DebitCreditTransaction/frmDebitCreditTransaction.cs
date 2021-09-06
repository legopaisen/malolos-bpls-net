using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.SearchBusiness;
using Amellar.Common.StringUtilities;
using Amellar.Common.LogIn;
using Amellar.Common.BPLSApp;

namespace Amellar.Modules.DebitCreditTransaction
{
    public partial class frmDebitCreditTransaction : Form
    {
        public frmDebitCreditTransaction()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

        protected Library LibraryClass = new Library();
        private string m_sOwnCode = "";

        private void frmDebitCreditTransaction_Load(object sender, EventArgs e)
        {
            dtpORDate.Value = AppSettingsManager.GetSystemDate();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
            {
                LoadValues(bin1.GetBin());
            }
            else
            {
                frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                frmSearchBns.ShowDialog();

                if (frmSearchBns.sBIN.Length > 1)
                {
                    bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                    bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                    LoadValues(bin1.GetBin());
                }
            }
        }

        private void LoadValues(String sBin)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = @"select BIN,BNS_NM,OWN_CODE from businesses where bin = '" + StringUtilities.HandleApostrophe(sBin) + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtBnsName.Text = pSet.GetString(1);
                    m_sOwnCode = pSet.GetString(2);
                    txtTaxPayrName.Text = AppSettingsManager.GetBnsOwner(m_sOwnCode);
                    txtTaxPayrAdd.Text = AppSettingsManager.GetBnsOwnAdd(m_sOwnCode);
                    rdoCredit.Enabled = true;
                    rdoDebit.Enabled = true;
                }
                /*else
                {
                    MessageBox.Show("No record found!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    bin1.txtTaxYear.Text = "";
                    bin1.txtBINSeries.Text = "";
                    bin1.txtTaxYear.Focus();
                    rdoCredit.Enabled = false;
                    rdoDebit.Enabled = false;
                }*/
                // RMC 20160105 Adding tax credit for NEW business in business_que, put rem
                // RMC 20160105 Adding tax credit for NEW business in business_que (s)
                else
                {
                    if (MessageBox.Show("Record not found in business record?\nContinue searching in application?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        pSet.Close();
                        return;
                    }

                    pSet.Close();
                    pSet.Query = @"select BIN,BNS_NM,OWN_CODE from business_que where bin = '" + StringUtilities.HandleApostrophe(sBin) + "'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            txtBnsName.Text = pSet.GetString(1);
                            m_sOwnCode = pSet.GetString(2);
                            txtTaxPayrName.Text = AppSettingsManager.GetBnsOwner(m_sOwnCode);
                            txtTaxPayrAdd.Text = AppSettingsManager.GetBnsOwnAdd(m_sOwnCode);
                            rdoCredit.Enabled = true;
                            rdoDebit.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("No record found!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            bin1.txtTaxYear.Text = "";
                            bin1.txtBINSeries.Text = "";
                            bin1.txtTaxYear.Focus();
                            rdoCredit.Enabled = false;
                            rdoDebit.Enabled = false;
                        }
                    }
                    

                }
                // RMC 20160105 Adding tax credit for NEW business in business_que (s)
                
            }
            pSet.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rdoCredit.Enabled = false;
            rdoDebit.Enabled = false;
            rdoCredit.Checked = false;
            rdoDebit.Checked = false;
            
            bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";

            foreach (object c in this.Controls)
                if (c is TextBox)
                    ((TextBox)c).Text = "";

            dtpORDate.Value = AppSettingsManager.GetSystemDate();
            txtDbAmount.ReadOnly = true;
            txtCdAmount.ReadOnly = true;
            txtDbAmount.Text = "0.00";
            txtCdAmount.Text = "0.00";
            label8.Text = "Balance";

            bin1.txtTaxYear.Focus();
        }

        private void rdoDebit_Click(object sender, EventArgs e)
        {
            txtDbAmount.Text = "0.00";
            txtCdAmount.Text = "0.00";
            txtDbAmount.ReadOnly = false;
            txtCdAmount.ReadOnly = true;
            label8.Text = "Balance (Debit)";
        }

        private void rdoCredit_Click(object sender, EventArgs e)
        {
            txtDbAmount.Text = "0.00";
            txtCdAmount.Text = "0.00";
            txtDbAmount.ReadOnly = true;
            txtCdAmount.ReadOnly = false;
            label8.Text = "Balance (Credit)";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDbAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDbAmount_TextChanged(object sender, EventArgs e)
        {
            if (rdoCredit.Enabled != false && rdoDebit.Enabled != false)
                ComputeBalance();
        }

        private void ComputeBalance()
        {
            double dResult = 0;
            
            // AST 20160201 (s)
            double dblDebitAmt = 0;
            double dblCreditAmt = 0;
            double.TryParse(txtDbAmount.Text, out dblDebitAmt);
            double.TryParse(txtCdAmount.Text, out dblCreditAmt);
            // AST 20160201 (e)

            //if (Convert.ToDouble(txtDbAmount.Text) > Convert.ToDouble(txtCdAmount.Text)) // AST 20160201 rem
            if (dblDebitAmt > dblCreditAmt) // AST 20160201
            {
                //dResult = Convert.ToDouble(txtDbAmount.Text) - Convert.ToDouble(txtCdAmount.Text);
                dResult = dblDebitAmt - dblCreditAmt;
                dResult = dResult * -1;
            }
            //if (Convert.ToDouble(txtCdAmount.Text) > Convert.ToDouble(txtDbAmount.Text)) // AST 20160201 rem
            else if (dblCreditAmt > dblDebitAmt) // AST 20160201
            {
                dResult = Convert.ToDouble(txtCdAmount.Text) - Convert.ToDouble(txtDbAmount.Text);
            }
            //if (Convert.ToDouble(txtCdAmount.Text) == Convert.ToDouble(txtDbAmount.Text) &&
            //    (Convert.ToDouble(txtCdAmount.Text) != 0.00 && Convert.ToDouble(txtDbAmount.Text) != 0.00)) // AST 20160201 rem
            else if (dblCreditAmt == dblDebitAmt && dblCreditAmt != 0 && dblDebitAmt != 0) // AST 20160201 
            {
                dResult = 0.00;
            }

            if (dResult < 0) // AST 20160203 
                dResult = 0;

            txtBalance.Text = dResult.ToString("#,##0.00");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String sQuery = "", sTextFormat = "", m_sUser = "";
            DateTime cdtToday;
            cdtToday = AppSettingsManager.GetSystemDate();

            OracleResultSet pSet = new OracleResultSet();

            double dDebit = 0, dCredit = 0, dBalance = 0;
            double dTempDebit = 0, dTempCredit = 0, dTempBalance = 0;

            // AST 20160122 (s)
            double dblDebit = 0;
            double dblCredit = 0;
            double dblBalance = 0;
            // AST 20160122 (e)

            String sBin = "";
            sBin = bin1.GetBin();

            if (Convert.ToDouble(txtDbAmount.Text) == 0.00 && Convert.ToDouble(txtCdAmount.Text) == 0.00)
            {
                MessageBox.Show("No amount detected for Debit amount & Credit amount! ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtDbAmount.Focus();
                return;
            }

            if (txtChkNo.Text.Trim() == "" && txtORNo.Text.Trim() == "")
            {
                MessageBox.Show("No reference number detected!\nReference number can be OR Number or \nCheck Number", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtChkNo.Focus();
                return;
            }

            if (txtMemo.Text.Trim() == "")
            {
                MessageBox.Show("Memoranda required!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtMemo.Focus();
                return;
            }

            if (dtpORDate.Value >= Convert.ToDateTime(cdtToday))
            {
                MessageBox.Show("OR Date must not be greater or equal to current date !", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtpORDate.Focus();
                return;
            }

            if (MessageBox.Show("Do you want to save this info for debit/credit?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                frmLogIn frmLogIn = new frmLogIn();
                frmLogIn.sFormState = "LOGIN";
                frmLogIn.ShowDialog();
                
                if (frmLogIn.m_sUserCode != "")
                {
                    m_sUser = frmLogIn.m_sUserCode;

                    sQuery = "select * from dbcr_memo where bin = '" + StringUtilities.HandleApostrophe(sBin) + "'"; //JARS 20171010
                    //sQuery = "select * from dbcr_memo where own_code = '" + AppSettingsManager.GetOwnCode(sBin) + "'"; //JARS 20170713
                    sQuery += " and served = 'N'";

                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            dDebit = pSet.GetDouble("debit");
                            dTempDebit = dDebit;
                            dCredit = pSet.GetDouble("credit");
                            dTempCredit = dCredit;
                            dBalance = pSet.GetDouble("balance");
                            dTempBalance = dBalance;

                            // AST 20160122 (s)
                            double.TryParse(txtDbAmount.Text, out dblDebit);
                            double.TryParse(txtCdAmount.Text, out dblCredit);
                            double.TryParse(txtBalance.Text, out dblBalance);
                            // AST 20160122 (e)

                            sQuery = "insert into dbcr_memo values"; //JARS 20171010
                            sQuery += "('" + sBin + "', ";
                            //sQuery += "('" + AppSettingsManager.GetOwnCode(sBin) + "', "; //JARS 20170713
                            sQuery += "'" + txtORNo.Text.Trim() + "',";

                            // AST 20160122 (s)
                            //sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                            //sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'),";
                            sQuery += "to_date('" + dtpORDate.Value.ToShortDateString() + "','MM/dd/yyyy'),";

                            //sQuery += Convert.ToDouble(txtDbAmount.Text) + ",";
                            //sQuery += Convert.ToDouble(txtCdAmount.Text) + ",";
                            //sQuery += Convert.ToDouble(txtBalance.Text) + ",";
                            sQuery += dblDebit + ",";
                            sQuery += dblCredit + ",";
                            sQuery += dblBalance + ",";
                            

                            sQuery += "'" + txtMemo.Text + "',";
                            sQuery += "' ',";
                            sQuery += "'" + m_sUser + "',";
                            //sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'),";
                            sQuery += "to_date('" + dtpORDate.Value.ToShortDateString() + "','MM/dd/yyyy'),";
                            // AST 20160122 (e)
                            sTextFormat = dtpORDate.Value.ToString("HH:m");
                            sQuery += "'" + sTextFormat + "',";                            
                            sQuery += "'N',";
                            sQuery += "'" + txtChkNo.Text + "',";
                            sQuery += "'N','1')";

                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();

                            sQuery = "update dbcr_memo set served = 'Y' where bin = '" + StringUtilities.HandleApostrophe(sBin) + "'"; //JARS 20171010
                            //sQuery = "update dbcr_memo set served = 'Y' where own_code = '" + AppSettingsManager.GetOwnCode(sBin) + "'"; //JARS 20170713
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();


                            //dTempDebit += Convert.ToDouble(txtDbAmount.Text);
                            //dTempCredit += Convert.ToDouble(txtCdAmount.Text);
                            dTempDebit += dblDebit;
                            dTempCredit += dblCredit;
                            dTempBalance = dTempCredit - dTempDebit;

                            double.TryParse(txtBalance.Text, out dblBalance);

                            //if (Convert.ToDouble(txtBalance.Text) > 0.00)
                            if (dblBalance > 0)
                            {
                                txtCdAmount.Text = txtBalance.Text;
                                txtDbAmount.Text = "0.00";
                            }
                            else
                            {

                                //if (Convert.ToDouble(txtBalance.Text) < 0.00)
                                if (dblBalance < 0)
                                {
                                    //txtDbAmount.Text = Convert.ToDouble(Convert.ToDouble(txtBalance.Text) * -1).ToString(); // AST 20160201 rem
                                    txtDbAmount.Text = string.Format("{0}", dblBalance * -1); // AST 20160201
                                    //txtCdAmount.Text = ""; // AST 20160201 rem
                                    txtCdAmount.Text = "0.00";  // AST 20160201
                                }
                                else
                                {
                                    txtDbAmount.Text = "0.00";
                                    txtCdAmount.Text = "0.00";
                                }
                            }

                            sQuery = "insert into dbcr_memo values"; //JARS 20171010
                            sQuery += "('" + sBin + "', ";
                            //sQuery += "('" + AppSettingsManager.GetOwnCode(sBin) + "', "; //JARS 20170713
                            sQuery += "'" + txtORNo.Text + "',";
                            sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                            sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'),";
                            // AST 20160201 (s)
                            //sQuery += Convert.ToDouble(txtDbAmount.Text) + ",";
                            //sQuery += Convert.ToDouble(txtCdAmount.Text) + ",";
                            //sQuery += Convert.ToDouble(txtBalance.Text) + ",";
                            sQuery += dblDebit + ",";
                            sQuery += dblCredit + ",";
                            sQuery += dblBalance + ",";
                            // AST 20160201 (e)
                            sQuery += "'REMAINING BALANCE AFTER MERGING OF ALL TAX CREDIT/DEBIT',";
                            sQuery += "' ',";
                            sQuery += "'" + m_sUser + "',";
                            sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'),";
                            sTextFormat = dtpORDate.Value.ToString("HH:m");
                            sQuery += "'" + sTextFormat + "',";
                            sQuery += "'N',";
                            sQuery += "'" + txtChkNo.Text + "',";
                            sQuery += "'N','1')";
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();
                        }
                        else
                        {
                            // AST 20160201 (s)
                            double.TryParse(txtDbAmount.Text, out dblDebit);
                            double.TryParse(txtCdAmount.Text, out dblCredit);
                            double.TryParse(txtBalance.Text, out dblBalance);
                            // AST 20160201 (e)

                            sQuery = "insert into dbcr_memo values"; //JARS 20171010
                            sQuery += "('" + sBin + "',";
                            //sQuery += "('" + AppSettingsManager.GetOwnCode(sBin) + "', "; //JARS 20170713
                            sQuery += "'" + txtORNo.Text + "',";
                            sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                            sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'),";
                            // AST 20160201 (s)
                            //sQuery += Convert.ToDouble(txtDbAmount.Text) + ",";
                            //sQuery += Convert.ToDouble(txtCdAmount.Text) + ",";
                            //sQuery += Convert.ToDouble(txtBalance.Text) + ",";
                            sQuery += dblDebit + ",";
                            sQuery += dblCredit + ",";
                            sQuery += dblBalance + ",";
                            // AST 20160201 (e)
                            sQuery += "'" + txtMemo.Text + "',";
                            sQuery += "' ',";
                            sQuery += "'" + m_sUser + "',";
                            sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'),";
                            sTextFormat = dtpORDate.Value.ToString("HH:m");
                            sQuery += "'" + sTextFormat + "',";
                            sQuery += "'N',";
                            sQuery += "'" + txtChkNo.Text + "',";
                            sQuery += "'N','1')";

                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();

                            sQuery = "update dbcr_memo set served = 'Y' where bin = '" + sBin + "'"; //JARS 20171010
                            //sQuery = "update dbcr_memo set served = 'Y' where own_code = '" + AppSettingsManager.GetOwnCode(sBin) + "'"; //JARS 20170713

                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();

                            // AST 20160201 (s)
                            //dTempDebit += Convert.ToDouble(txtDbAmount.Text);
                            //dTempCredit += Convert.ToDouble(txtCdAmount.Text);
                            dTempDebit += dblDebit;
                            dTempCredit += dblCredit;
                            // AST 20160201 (e)
                            dTempBalance = dTempCredit - dTempDebit;

                            sQuery = "insert into dbcr_memo values"; //JARS 20171010
                            sQuery += "('" + sBin + "', ";
                            //sQuery += "('" + AppSettingsManager.GetOwnCode(sBin) + "', "; //JARS 20170713
                            sQuery += "'" + txtORNo.Text.Trim() + "',";
                            sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                            sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'),";
                            // AST 20160201 (s)
                            //sQuery += Convert.ToDouble(txtDbAmount.Text) + ",";
                            //sQuery += Convert.ToDouble(txtCdAmount.Text) + ",";
                            //sQuery += Convert.ToDouble(txtBalance.Text) + ",";
                            sQuery += dblDebit + ",";
                            sQuery += dblCredit + ",";
                            sQuery += dblBalance + ",";
                            // AST 20160201 (e)
                            sQuery += "'REMAINING BALANCE AFTER ADDING OF TAX CREDIT',";
                            sQuery += "' ',";
                            sQuery += "'" + m_sUser + "',";
                            sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'),";
                            sTextFormat = dtpORDate.Value.ToString("HH:m");
                            sQuery += "'" + sTextFormat + "',";
                            sQuery += "'N',";
                            sQuery += "'" + txtChkNo.Text.Trim() + "',";
                            sQuery += "'N','1')";

                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();

                        }
                    pSet.Close();

                    sQuery = string.Format("update tmp_dbcr set served = 'Y' where own_code = '{0}'", m_sOwnCode);
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    // RMC 20170126 add creation of SOA for Rev-Exam with over payment (s)
                    pSet.Query = "delete from taxdues where bin = '" + sBin + "' and amount < 0";
                    pSet.ExecuteNonQuery();
                    // RMC 20170126 add creation of SOA for Rev-Exam with over payment (e)

                    LibraryClass.AuditTrail("CPDC", "dbcr_memo", sBin + "/" + txtORNo.Text.Trim() + "/" + txtChkNo.Text.Trim());
                    MessageBox.Show("Debit Credit successfully saved!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnClear.PerformClick();
                    btnSave.Enabled = false;
                }
                else
                {
                    MessageBox.Show("User validation failed!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSave.Focus();
                }
            }
        }

    }
}