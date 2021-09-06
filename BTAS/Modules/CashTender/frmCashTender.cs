using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace CashTender
{
    public partial class frmCashTender : Form
    {
        public string m_sAmountDue = string.Empty;
        public string m_sTaxCreditAmount = string.Empty;
        public string m_sExcessChange = string.Empty;
        public string m_sBIN = string.Empty;
        public string m_sPaymentMode = string.Empty;
        public string m_sOrNO = string.Empty;
        string m_sChange = string.Empty;
        public string m_sOrigAmtDue = string.Empty;
        public string m_sPaymentType = string.Empty;
        public bool m_bCancel = false;  // RMC 20140909 Migration QA

        public frmCashTender()
        {
            InitializeComponent();
        }

        private void frmCashTender_Load(object sender, EventArgs e)
        {
            // RMC 20140909 Migration QA (s)
            m_bCancel = false;
            double dDue = 0;
            double dCheck = 0;
            double dCash = 0;
            double dBalance = 0;
            double.TryParse(lblDueAmount.Text.ToString(), out dDue);
            double.TryParse(txtCheckAmount.Text.ToString(), out dCheck);
            double.TryParse(txtCashAmount.Text.ToString(), out dCash);
            dBalance = dDue - (dCheck + dCash);
            Math.Round(dBalance, 2); //JARS 20170831
            txtBalance.Text = string.Format("{0: #,##0.00}", dBalance);
            // RMC 20140909 Migration QA (e)

            if (m_sPaymentType.Trim() == "CQ" || m_sPaymentType.Trim() == "CQTC" || m_sPaymentType.Trim() == "TC") //MCR 20140902 CQ
            {
                m_sAmountDue = lblDueAmount.Text.Trim();    // RMC 20140909 Migration QA
                /*double fAmountDue = Convert.ToDouble(lblDueAmount.Text.Trim());
                double fExcessAmount = Convert.ToDouble(m_sExcessChange);
                AppSettingsManager.TellerTransaction(AppSettingsManager.TellerUser.UserCode, m_sPaymentMode, m_sBIN, m_sOrNO, fAmountDue, Convert.ToDouble(m_sTaxCreditAmount), Convert.ToDouble(txtCheckAmount.Text), Convert.ToDouble(0), fExcessAmount, m_sPaymentType);*/   // RMC 20140909 Migration QA, put rem
                this.Close();
            }
            
            btnOk.Enabled = false;  // RMC 20141203 corrected error in online payment
            txtCashTendered.Focus(); // AST 20150427
            txtCashTendered.SelectAll(); // AST 20150427
        }

        private void txtCashTendered_TextChanged(object sender, EventArgs e)
        {
            double dTrueChange = 0;
            string sTrueChange = string.Empty;

            //MCR 20140826
            if (txtCashTendered.Text.Trim() == "" || txtCashTendered.Text.Trim() == ".")
                txtCashTendered.Text = "0.00";

            if (txtCashAmount.Text == "0.00")
            {
                if (txtBalance.Text == "0.00")
                    txtCashTendered.ReadOnly = true;
                else
                    dTrueChange = double.Parse(txtCashTendered.Text.Trim()) - double.Parse(txtBalance.Text.Trim());
            }
            else
                dTrueChange = double.Parse(txtCashTendered.Text.Trim()) - double.Parse(txtCashAmount.Text.Trim());

            sTrueChange = dTrueChange.ToString();
            lblChange.Text = string.Format("{0: #,##0.00}", dTrueChange);
            m_sChange = sTrueChange;

            // RMC 20141203 corrected error in online payment (s)
            if (dTrueChange >= 0)
                btnOk.Enabled = true;
            else
                btnOk.Enabled = false;
            // RMC 20141203 corrected error in online payment (e)
        }

        private void txtCashTendered_Leave(object sender, EventArgs e)
        {
            double dTrueChange = 0;
            string sTrueChange = string.Empty;

            //MCR 20140826
            if (txtCashTendered.Text.Trim() == "" || txtCashTendered.Text.Trim() == ".")
                txtCashTendered.Text = "0.00";

            if (txtCashAmount.Text == "0.00")
            {
                if (txtBalance.Text == "0.00")
                    txtCashTendered.ReadOnly = true;
                else
                    dTrueChange = double.Parse(txtCashTendered.Text.Trim()) - double.Parse(txtBalance.Text.Trim());
            }
            else
                dTrueChange = double.Parse(txtCashTendered.Text.Trim()) - double.Parse(txtCashAmount.Text.Trim());

            sTrueChange = dTrueChange.ToString();
            lblChange.Text = string.Format("{0: #,##0.00}", dTrueChange);
            m_sChange = sTrueChange;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            
            // RMC 20140909 Migration QA (s)
            double dCashTenderd = 0;
            double.TryParse(txtCashTendered.Text.ToString(), out dCashTenderd);
            if (dCashTenderd == 0)
            {
                MessageBox.Show("Invalid amount", "Cash Tender", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtCashTendered.Focus();
                return;
            }
            // RMC 20140909 Migration QA (e)

            if (lblChange.Text.Trim() == string.Empty || double.Parse(lblChange.Text.Trim()) < 0)
            {
                if (MessageBox.Show("Total change is " + lblChange.Text.Trim() + " . Continue Closing?\nWARNING!: THIS WILL REFLECT AS LAST CHANGE OF THE TRANSACTION", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (m_sTaxCreditAmount.Trim() == string.Empty)
                        m_sTaxCreditAmount = "0";
                    if (m_sExcessChange.Trim() == string.Empty)
                        m_sExcessChange = "0";

                    double fAmountDue = double.Parse(lblDueAmount.Text.Trim());
                    double fTaxCredit = double.Parse(m_sTaxCreditAmount);
                    double fExcessAmount = double.Parse(m_sExcessChange);
                    double fOrigAmtDue = double.Parse(m_sOrigAmtDue);
                    //double fOrigAmtDue = double.Parse(lblDueAmount.Text.Trim()); // change this to total discounted total

                    if (fTaxCredit > 0)
                    {
                        if (fOrigAmtDue > fTaxCredit)
                            fAmountDue = fOrigAmtDue;
                        else
                        {
                            //result.Query = "select debit from dbcr_memo where memo like 'DEBIT%' and or_no = '" + m_sOrNO + "'"; // GDE 20111021
                            result.Query = "select credit from dbcr_memo where memo like 'CREDIT%' and or_no = '" + m_sOrNO + "'"; //JARS 20170905 //JARS 20171010
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    fAmountDue = result.GetDouble("credit"); //JARS 20170905 CREDIT
                                    fExcessAmount = fTaxCredit - fAmountDue;
                                }
                            }
                            result.Close();
                        }
                    }
                    if (fExcessAmount <= 0)
                        fExcessAmount = double.Parse(m_sChange);

                    m_sExcessChange = fExcessAmount.ToString();  // RMC 20140909 Migration QA
                    m_sAmountDue = lblDueAmount.Text; //MCR 20141023
                    //AppSettingsManager.TellerTransaction(AppSettingsManager.TellerUser.UserCode, m_sPaymentMode, m_sBIN, m_sOrNO, fAmountDue, Convert.ToDouble(m_sTaxCreditAmount), Convert.ToDouble(txtCheckAmount.Text), Convert.ToDouble(txtCashTendered.Text), fExcessAmount, m_sPaymentType);	// JJP 08242005 New Teller Transaction  // RMC 20140909 Migration QA, put rem
                }
            }
            else
            {
                if (m_sTaxCreditAmount.Trim() == string.Empty)
                    m_sTaxCreditAmount = "0";
                if (m_sExcessChange.Trim() == string.Empty)
                    m_sExcessChange = "0";

                double fAmountDue = double.Parse(lblDueAmount.Text.Trim());
                double fTaxCredit = double.Parse(m_sTaxCreditAmount);
                double fExcessAmount = double.Parse(m_sExcessChange);
                double fOrigAmtDue = double.Parse(m_sOrigAmtDue);
                //double fOrigAmtDue = double.Parse(lblDueAmount.Text.Trim()); // change this to total discounted total
                if (fTaxCredit > 0)
                {
                    if (fOrigAmtDue > fTaxCredit)
                        fAmountDue = fOrigAmtDue;
                    else
                    {
                        //result.Query = "select debit from dbcr_memo where memo like 'DEBIT%' and or_no = '" + m_sOrNO + "'"; // GDE 20111021
                        result.Query = "select credit from dbcr_memo where memo like 'CREDIT%' and or_no = '" + m_sOrNO + "'"; //JARS 20170905 //JARS 20171010
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                fAmountDue = result.GetDouble("credit"); //JARS 20170905 CREDIT
                                fExcessAmount = fTaxCredit - fAmountDue;
                            }
                        }
                        result.Close();
                    }
                }
                if (fExcessAmount <= 0)
                    fExcessAmount = double.Parse(m_sChange);

                m_sExcessChange = fExcessAmount.ToString();  // RMC 20140909 Migration QA
                m_sAmountDue = lblDueAmount.Text; //MCR 20141023
                //AppSettingsManager.TellerTransaction(AppSettingsManager.TellerUser.UserCode, m_sPaymentMode, m_sBIN, m_sOrNO, fAmountDue, Convert.ToDouble(m_sTaxCreditAmount), Convert.ToDouble(txtCheckAmount.Text), Convert.ToDouble(txtCashTendered.Text), fExcessAmount, m_sPaymentType);	// JJP 08242005 New Teller Transaction  // RMC 20140909 Migration QA, put rem
            }

            this.Close();
        }

        private void txtCashTendered_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // RMC 20140909 Migration QA
            if (MessageBox.Show("Cancel Cash Tender?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m_bCancel = true;
                this.Close();
            }
            else
                return;
        }

        
    }
}