using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BIN;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;
using Amellar.Common.SearchBusiness;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmReprintOR : Form
    {
        public frmReprintOR()
        {
            InitializeComponent();
        }

        bool bCompromise = false, bInitialDP = false;
        string m_sPaymentType = string.Empty, m_sPaymentMode = string.Empty, m_sPaymentTerm = string.Empty;
        DateTime m_sDtOperated;
        string m_sOwnCode = string.Empty, m_sBIN = string.Empty, sTaxYear = string.Empty, m_sIsQtrly = string.Empty;
        int iAgreePayTerm = 0, m_iNoPaySeq = 0, m_iTermToPayComp = 0;
        double m_dDiscountRate = 0, m_dFeesAmt = 0, m_dFeesPen = 0, m_dFeesSurch = 0, m_dFeesDue = 0;
        string m_sDateToPay = string.Empty, m_sMonthlyCutoffDate = "", m_sInitialDueDate = "";
        double m_dPenRate = 0, m_dSurchRate = 0;
        DateTime mv_cdtORDate;

        string m_sCurrentTaxYear = string.Empty, m_sNewWithQtr = string.Empty;
        struct PayTempStruct
        {
            public string bin;
            public string tax_year;
            public string term;
            public string qtr;
            public string bns_code;
            public string fees_desc;
            public string fees_due;
            public string fees_surch;
            public string fees_pen;
            public string fees_totaldue;
            public string fees_code;
            public string due_state;
            public string qtr_to_pay;
            public string fees_code_sort;
        }

        struct TaxDuesStruct
        {
            public string bin;
            public string tax_year;
            public string qtr_to_pay;
            public string tax_code;
            public string amount;
            public string due_state;
        }

        struct PenRate
        {
            public double p_dSurchRate1;
            public double p_dSurchRate2;
            public double p_dSurchRate3;
            public double p_dSurchRate4;
            public double p_dPenRate1;
            public double p_dPenRate2;
            public double p_dPenRate3;
            public double p_dPenRate4;
            public double p_SurchQuart;
            public double p_PenQuart;
        }

        private void frmReprintOR_Load(object sender, EventArgs e)
        {
            bin1.txtLGUCode.Text = AppSettingsManager.GetConfigValue("10");
            bin1.txtDistCode.Text = AppSettingsManager.GetConfigValue("11");

            OracleResultSet pSet = new OracleResultSet();
            String sQuery = "";
            bCompromise = false;
            m_sPaymentTerm = "CS";

            pSet.Query = string.Format("select * from discount_tbl where rev_year = '{0}'", AppSettingsManager.GetConfigValue("07"));
            if (pSet.Execute())
                if (pSet.Read())
                    m_dDiscountRate = pSet.GetDouble("discount_rate");
            pSet.Close();
        }

        private void btnSearchBin_Click(object sender, EventArgs e)
        {
            String sQuery;
            String sTransAppCode, sLName, sFName, sMI, sAppName;

            if (btnSearchBin.Text == "Search BIN")
            {
                OracleResultSet pSet = new OracleResultSet();
                bInitialDP = true;
                btnSearchBin.Text = "Clear BIN";
                m_sPaymentType = "CS";
                if (bin1.txtTaxYear.Text.Trim() == string.Empty && bin1.txtBINSeries.Text.Trim() == string.Empty)
                {
                    frmSearchBusiness dlg = new frmSearchBusiness();
                    dlg.ShowDialog();
                    //if (dlg.sBIN.ToString().Trim() != String.Empty)
                    if (dlg.sBIN.Length > 1)   // RMC 20141020 printing of OR
                    {
                        /*m_sBIN = StringUtilities.HandleApostrophe(dlg.sBIN);
                        bin1.txtTaxYear.Text = m_sBIN.Substring(7, 4);
                        bin1.txtBINSeries.Text = m_sBIN.Substring(7);*/
                        // RMC 20141020 printing of OR (s)
                        bin1.txtTaxYear.Text = dlg.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = dlg.sBIN.Substring(12, 7).ToString();
                        m_sBIN = StringUtilities.HandleApostrophe(bin1.GetBin());
                        // RMC 20141020 printing of OR (e)
                        m_sOwnCode = dlg.sOwnCode;

                        frmListofOR ORdlg = new frmListofOR();
                        ORdlg.BIN = m_sBIN;
                        ORdlg.ShowDialog();
                        if (ORdlg.m_bOk)
                            txtORNo.Text = ORdlg.ORNumber.Trim();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    m_sBIN = StringUtilities.HandleApostrophe(bin1.GetBin());
                    frmListofOR ORdlg = new frmListofOR();
                    ORdlg.BIN = bin1.GetBin();
                    ORdlg.ShowDialog();
                    if (ORdlg.m_bOk)
                        txtORNo.Text = ORdlg.ORNumber.Trim();
                }

                ORSearch();

                txtTotDues.Text = "0.00";
                txtCreditLeft.Text = "0.00";
                txtToTaxCredit.Text = "0.00";
                txtGrandTotal.Text = "0.00";
                chkTCredit.Enabled = false;

                if (m_sOwnCode != "")
                {
                    pSet.Query = string.Format("select * from dbcr_memo where bin = '{0}' and memo = 'DEBITED THRU PAYMENT MADE HAVING OR_NO {1}'", m_sBIN, txtORNo.Text.Trim()); //JARS 20171010
                    //pSet.Query = string.Format("select * from dbcr_memo where own_code = '{0}' and memo = 'DEBITED THRU PAYMENT MADE HAVING OR_NO {1}'", m_sOwnCode, txtORNo.Text.Trim()); //JARS 20170711
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            txtCreditLeft.Text = pSet.GetDouble("debit").ToString("#,##0.00");
                        }
                    pSet.Close();
                }

                bool bDeducted = false;
                if (txtTotTotDue.Text == "")
                    txtTotTotDue.Text = "0";

                txtDiscountAmount.Text = txtTotTotDue.Text; // RMC 20141020 printing of OR, disabled txtDiscountAmount setting of values

                if (Convert.ToDouble(txtDiscountAmount.Text) <= Convert.ToDouble(txtCreditLeft.Text))
                {
                    bDeducted = true;
                    txtToTaxCredit.Text = txtDiscountAmount.Text;
                }

                if (Convert.ToDouble(txtCreditLeft.Text) > 0.00 && (Convert.ToDouble(txtDiscountAmount.Text) > Convert.ToDouble(txtCreditLeft.Text)))
                {
                    bDeducted = true;
                    txtToTaxCredit.Text = txtCreditLeft.Text;
                }

                CompLessCredit(txtDiscountAmount.Text, txtCreditLeft.Text, txtToTaxCredit.Text);

                chkPartial.Enabled = false;

                if (bDeducted == true)
                {
                    chkTCredit.Checked = true;
                    if (Convert.ToDouble(txtGrandTotal.Text) == 0.00)
                    {
                        chkCash.Enabled = false;
                        chkCheque.Enabled = false;
                    }
                    else
                    {
                        chkCash.Enabled = true;
                        chkCheque.Enabled = true;
                    }
                }
            }
            else
            {
                btnSearchBin.Text = "Search BIN";
                CleanMe();
            }
        }

        private void OnCompromise(String sBIN, String sRange1, String sRange2)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            String sQuery = "", sPaySw = "", sDesc = "", sString = "", sPayType = "";
            int iNoPaySeq = 0, iRange1 = 0, iRange2 = 0, iTermToPay = 0;
            double dFeesDue = 0, dFeesSurch = 0, dFeesPen = 0, dFeesTotal = 0;
            double dTmpDue = 0, dTmpSurch = 0, dTmpPen = 0, dTmpTotal = 0;
            int iRows, iTotalRows = 0;

            double dRowTotal = 0;
            double dTmpRowCompPen = 0;
            String sRowCompInt;

            iAgreePayTerm = 0;
            m_dFeesDue = 0;
            m_dFeesSurch = 0;
            m_dFeesPen = 0;
            m_dFeesAmt = 0;

            iTotalRows = dgvTaxFees.RowCount;

            if (iTotalRows > 0)
                for (iRows = 0; iRows < iTotalRows; )
                {
                    if (dgvTaxFees.Rows[iRows].Cells[8].Value.ToString().Substring(0, 1) == "C")
                    {
                        dgvTaxFees.Rows.RemoveAt(iRows);
                        iTotalRows--;
                    }
                    else
                        iRows++;
                }

            sQuery = "select * from compromise_tbl where bin = '" + sBIN + "'";
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    iNoPaySeq = pSet.GetInt("no_pay_seq");
            pSet.Close();

            if (iNoPaySeq > 0)
            {
                sQuery = "select fees_due as fdue,fees_surch as fsurch,fees_pen as fpen,";
                sQuery += " fees_total as ftotal, pay_sw, term_to_pay";
                sQuery += " from compromise_due where bin = '" + sBIN + "'";

                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        iTermToPay = pSet.GetInt("term_to_pay");
                        m_iTermToPayComp = iTermToPay;
                        sPaySw = pSet.GetString("pay_sw");

                        String sFeesDue, sFeesSurch, sFeesPen;
                        sFeesDue = Convert.ToDouble(pSet.GetDouble("fdue") * 0.25).ToString("##0.00");
                        sFeesSurch = Convert.ToDouble(pSet.GetDouble("fsurch") * 0.25).ToString("##0.00");
                        sFeesPen = Convert.ToDouble(pSet.GetDouble("fpen") * 0.25).ToString("##0.00");

                        m_dFeesAmt = Convert.ToDouble(sFeesDue) + Convert.ToDouble(sFeesSurch) + Convert.ToDouble(sFeesPen);

                        if (sPaySw == "N")
                        {
                            bInitialDP = false;
                            sDesc = "COMPROMISE AGREEMENT (INITIAL PAYMENT)";

                            m_dFeesDue += Convert.ToDouble(sFeesDue);
                            m_dFeesSurch += Convert.ToDouble(sFeesSurch);
                            m_dFeesPen += Convert.ToDouble(sFeesPen);
                            m_dFeesAmt = m_dFeesDue + m_dFeesSurch + m_dFeesPen;

                            dTmpDue += Convert.ToDouble(sFeesDue);
                            dTmpSurch += Convert.ToDouble(sFeesSurch);
                            dTmpPen += Convert.ToDouble(sFeesPen);
                            dTmpTotal = Convert.ToDouble(sFeesDue) + Convert.ToDouble(sFeesSurch) + Convert.ToDouble(sFeesPen);

                            dgvTaxFees.Rows.Add("", "F", "F", sDesc, dTmpDue.ToString("##0.00"), dTmpSurch.ToString("##0.00"), Convert.ToDouble(dTmpPen).ToString("##0.00"), Convert.ToDouble(dTmpTotal).ToString("##0.00"), "CD", "", "");

                            m_dFeesDue = (m_dFeesDue - dTmpDue);
                            m_dFeesSurch = (m_dFeesSurch - dTmpSurch);
                            m_dFeesPen = (m_dFeesPen - dTmpPen);
                            m_dFeesAmt = (m_dFeesAmt - dTmpTotal);
                        }

                        if (bInitialDP == true)
                        {
                            sString = "";
                            if (sRange1 == "F")
                            {
                                double dxTmpDue = 0, dxTmpSurch = 0, dxTmpPen = 0, dxTmpTotal = 0;
                                sPayType = "F";
                                sDesc = "COMPROMISE AGREEMENT (REMAINING)";

                                dxTmpDue = (m_dFeesDue / Convert.ToDouble(iNoPaySeq)) - Convert.ToDouble(iTermToPay + 1);
                                dxTmpSurch = (m_dFeesSurch / Convert.ToDouble(iNoPaySeq)) - Convert.ToDouble(iTermToPay + 1);
                                dxTmpPen = (m_dFeesPen / Convert.ToDouble(iNoPaySeq)) - Convert.ToDouble(iTermToPay + 1);
                                dxTmpTotal = (m_dFeesAmt / Convert.ToDouble(iNoPaySeq)) - Convert.ToDouble(iTermToPay + 1);

                                m_dFeesDue = m_dFeesDue - dxTmpDue;
                                m_dFeesSurch = m_dFeesSurch - dxTmpSurch;
                                m_dFeesPen = m_dFeesPen - dxTmpPen;
                                m_dFeesAmt = m_dFeesAmt - dxTmpTotal;
                            }
                            else
                            {
                                if (txtCompromise.Enabled == true && (txtCompromise.Text == "0" || txtCompromise.Text == "0.00"))
                                    txtCompromise.Text = "1";
                                sPayType = "I";
                                sDesc = "COMPROMISE AGREEMENT (MONTHLY)";
                                m_dFeesDue = m_dFeesDue / Convert.ToDouble(iNoPaySeq);
                                m_dFeesSurch = m_dFeesSurch / Convert.ToDouble(iNoPaySeq);
                                m_dFeesPen = m_dFeesPen / Convert.ToDouble(iNoPaySeq);
                                m_dFeesAmt = m_dFeesDue + m_dFeesSurch + m_dFeesPen;
                            }
                            dgvTaxFees.Rows.Add("", sPayType, sString, sDesc, m_dFeesDue.ToString("##0.00"), m_dFeesSurch.ToString("##0.00"), Convert.ToDouble(m_dFeesPen).ToString("##0.00"), Convert.ToDouble(m_dFeesAmt).ToString("##0.00"), "C", "", "");
                        }
                        else
                        {
                            m_dFeesDue = m_dFeesDue / Convert.ToDouble(iNoPaySeq);
                            m_dFeesSurch = m_dFeesSurch / Convert.ToDouble(iNoPaySeq);
                            m_dFeesPen = m_dFeesPen / Convert.ToDouble(iNoPaySeq);
                            m_dFeesAmt = m_dFeesDue + m_dFeesSurch + m_dFeesPen;
                        }

                        iAgreePayTerm = iNoPaySeq;
                        m_iNoPaySeq = iNoPaySeq;
                    }
            }
        }


        private void CheckPaymentType()
        {
            m_sPaymentType = string.Empty;

            if (chkCash.Checked == true)
                m_sPaymentType = "CS";

            if (chkCheque.Checked == true)
                if (m_sPaymentType == string.Empty)
                    m_sPaymentType = "CQ";
                else
                    m_sPaymentType = "CC";

            if (chkTCredit.Checked == true)
                if (m_sPaymentType != string.Empty)
                    m_sPaymentType = m_sPaymentType + "TC";
                else
                    m_sPaymentType = "TC";
        }

        private void ORSearch()
        {
            string sTransAppCode, sLName, sFName, sMI, sAppName;
            OracleResultSet pSet = new OracleResultSet();

            int iNoQtrPaid = 0;

            bInitialDP = true;

            pSet.Query = string.Format("select * from businesses where bin = '{0}'", m_sBIN);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    m_sOwnCode = pSet.GetString("own_code");
                    txtBnsName.Text = pSet.GetString("bns_nm");
                    txtBnsCode.Text = pSet.GetString("bns_code");
                    txtBnsStat.Text = pSet.GetString("bns_stat");
                    txtTaxYear.Text = pSet.GetString("tax_year");
                    m_sDtOperated = pSet.GetDateTime("dt_operated");
                    sTaxYear = txtTaxYear.Text;
                }
                else
                {
                    MessageBox.Show("Business does not exist", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    bin1.txtTaxYear.Focus();
                    CleanMe();
                    AppSettingsManager.TaskManager(m_sBIN, "ONLINE PAYMENT", "DELETE");
                    return;
                }
            pSet.Close();

            pSet.Query = string.Format("select * from partial_payer where bin = '{0}' and tax_year = '{1}'", m_sBIN, txtTaxYear.Text);
            if (pSet.Execute())
                if (pSet.Read())
                    chkPartial.Checked = true;
            pSet.Close();

            txtBnsAddress.Text = AppSettingsManager.GetBnsAddress(m_sBIN);
            txtBnsOwner.Text = AppSettingsManager.GetBnsOwner(m_sOwnCode);
            txtBnsType.Text = AppSettingsManager.GetBnsDesc(txtBnsCode.Text.Trim());

            pSet.Query = string.Format("select * from pay_hist where or_no = '{0}'", txtORNo.Text.Trim());

            int iTmp = 0;
            string sTmp = "";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    chkFull.Enabled = false;
                    chkInstallment.Enabled = false;
                    chkCash.Enabled = false;
                    chkCheque.Enabled = false;
                    chk1st.Enabled = false;
                    chk2nd.Enabled = false;
                    chk3rd.Enabled = false;
                    chk4th.Enabled = false;

                    dtpORDate.Value = pSet.GetDateTime("or_date");
                    m_sPaymentTerm = pSet.GetString("payment_term");
                    m_sPaymentType = pSet.GetString("payment_type");
                    txtTeller.Text = pSet.GetString("teller");
                    m_sIsQtrly = pSet.GetString("qtr_paid");
                    txtMemo.Text = pSet.GetString("memo");
                    m_sPaymentMode = pSet.GetString("data_mode");
                    sTmp = pSet.GetString("no_of_qtr");

                    if (int.TryParse(sTmp, out iTmp) == false)
                        iNoQtrPaid = 0;
                    else
                        iNoQtrPaid = Convert.ToInt32(sTmp);

                    if (m_sPaymentTerm == "F")
                        SetCheckTerm(chkFull);
                    else
                        SetCheckTerm(chkInstallment);

                    SetCheckQtr(iNoQtrPaid);

                    if (m_sPaymentTerm == "F")
                    {
                        chk1st.Checked = true;
                        chk2nd.Checked = true;
                        chk3rd.Checked = true;
                        chk4th.Checked = true;
                    }

                    if (m_sPaymentType == "CS")
                    {
                        chkCash.Checked = true;
                        chkCheque.Checked = false;
                    }
                    else if (m_sPaymentType == "CQ")
                    {
                        chkCash.Checked = false;
                        chkCheque.Checked = true;
                    }
                    else //CC
                    {
                        chkCash.Checked = true;
                        chkCheque.Checked = true;
                    }
                }
            pSet.Close();

            ComputeTaxAndFees();
            DisplayDuesNew(txtORNo.Text.Trim(), m_sIsQtrly);
            btnPrint.Enabled = true;
        }

        private void SetCheckQtr(int iNoQtrPaid)
        {
            chk1st.Checked = false;
            chk2nd.Checked = false;
            chk3rd.Checked = false;
            chk4th.Checked = false;

            if (iNoQtrPaid > 1)
            {
                if (iNoQtrPaid == 2)
                {
                    if (m_sIsQtrly == "4")
                    {
                        chk3rd.Checked = true;
                        chk4th.Checked = true;
                    }
                    else
                        if (m_sIsQtrly == "3")
                        {
                            chk2nd.Checked = true;
                            chk3rd.Checked = true;
                        }
                    if (m_sIsQtrly == "2")
                    {
                        chk1st.Checked = true;
                        chk2nd.Checked = true;
                    }

                }
                else
                    if (iNoQtrPaid == 3)
                    {
                        if (m_sIsQtrly == "4")
                        {
                            chk2nd.Checked = true;
                            chk3rd.Checked = true;
                            chk4th.Checked = true;
                        }
                        else
                            if (m_sIsQtrly == "3")
                            {
                                chk1st.Checked = true;
                                chk2nd.Checked = true;
                                chk3rd.Checked = true;
                            }
                    }

            }
            else
            {
                if (m_sIsQtrly == "1")
                    chk1st.Checked = true;
                else
                    if (m_sIsQtrly == "2")
                        chk2nd.Checked = true;
                    else
                        if (m_sIsQtrly == "3")
                            chk3rd.Checked = true;
                        else
                            if (m_sIsQtrly == "4")
                                chk4th.Checked = true;
            }

        }

        private void SetCheckTerm(CheckBox chk)
        {
            if (chk == chkInstallment)
            {
                chkInstallment.Checked = true;
                chkFull.Checked = false;
            }
            else if (chk == chkFull)
            {
                chkInstallment.Checked = false;
                chkFull.Checked = true;
            }
            else
            {
                chkInstallment.Checked = false;
                chkFull.Checked = false;
            }
        }

        private bool WithPayment(String sBIN, String sTaxYear)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = string.Format("select * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode <> 'UNP' and qtr_paid <> 'X'", sBIN, sTaxYear);
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
                    return false;
                }
            }
            else
            {
                pSet.Close();
                return false;
            }
        }

        private void chkTCredit_CheckedChanged(object sender, EventArgs e)
        {
            CheckPaymentType();
        }

        private void chkCash_CheckedChanged(object sender, EventArgs e)
        {
            CheckPaymentType();
        }

        private void chkCheque_CheckedChanged(object sender, EventArgs e)
        {
            CheckPaymentType();
        }

        private void txtTaxYear_TextChanged(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (txtTaxYear.Text.Length == 4)
            {
                pSet.Query = string.Format("delete from pay_temp where bin = '{0}'", m_sBIN);
                pSet.ExecuteNonQuery();
                dgvTaxFees.Rows.Clear();

                pSet.Query = string.Format("select * from taxdues where bin = '{0}' and tax_year = '{1}'", m_sBIN, txtTaxYear.Text);
                if (pSet.Execute())
                    if (!pSet.Read())
                    {
                        MessageBox.Show("No Taxdues for the said Tax Year", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtTaxYear.Focus();
                        pSet.Close();
                        return;
                    }
                pSet.Close();

                ComputeTaxAndFees();

                txtCreditLeft.Text = "0.00";
                txtToTaxCredit.Text = "0.00";

                if (m_sOwnCode != "")
                {
                    pSet.Query = string.Format("select * from dbcr_memo where bin = '{0}' and served = 'N'", m_sBIN); //JARS 20171010
                    //pSet.Query = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N'", m_sOwnCode);
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            txtToTaxCredit.Text = pSet.GetDouble("balance").ToString("#,##0.00");
                            txtCreditLeft.Text = pSet.GetDouble("balance").ToString("#,##0.00");
                        }
                    pSet.Close();
                }

                if (Convert.ToDouble(txtGrandTotal.Text) <= Convert.ToDouble(txtCreditLeft.Text))
                    txtToTaxCredit.Text = txtGrandTotal.Text;

                CompLessCredit(txtDiscountAmount.Text, txtCreditLeft.Text, txtToTaxCredit.Text);

                chkPartial.Enabled = false;
            }
        }

        private void CompLessCredit(String sTotalTotalDue, String sCreditLeft, String sToBeCredited)
        {
            String sGrandTotalDue = "0";
            /*if (Convert.ToDouble(sTotalTotalDue) > Convert.ToDouble(sToBeCredited))
            {
                sGrandTotalDue = Convert.ToDouble(Convert.ToDouble(sTotalTotalDue) - Convert.ToDouble(sToBeCredited)).ToString("#,##0.00");
                sCreditLeft = Convert.ToDouble(Convert.ToDouble(sCreditLeft) - Convert.ToDouble(sToBeCredited)).ToString("#,##0.00");
            }
            else
                sGrandTotalDue = Convert.ToDouble(Convert.ToDouble(sTotalTotalDue) - Convert.ToDouble(sToBeCredited)).ToString("#,##0.00");

            if (Convert.ToDouble(sGrandTotalDue) < 0)
                sGrandTotalDue = "0.00";
                */
            // RMC 20141020 printing of OR, put rem convert.toDouble causes error for null parameters

            // RMC 20141020 printing of OR (s)
            double dTotalTotalDue = 0;
            double dToBeCredited = 0;
            double dCreditLeft = 0;
            double dGrandTotalDue = 0;
            double.TryParse(sTotalTotalDue, out dTotalTotalDue);
            double.TryParse(sToBeCredited, out dToBeCredited);
            double.TryParse(sCreditLeft, out dCreditLeft);

            if (dTotalTotalDue > dToBeCredited)
            {
                dGrandTotalDue = dTotalTotalDue - dToBeCredited;
                dCreditLeft = dCreditLeft - dToBeCredited;
                if (dCreditLeft > 0)
                    sCreditLeft = string.Format("{0:#,###.00}", dCreditLeft);
                else
                    sCreditLeft = "0.00";
            }
            else
                dGrandTotalDue = dTotalTotalDue - dToBeCredited;

            if (dGrandTotalDue <= 0)
                sGrandTotalDue = "0.00";
            else
                sGrandTotalDue = string.Format("{0:#,###.00}", dGrandTotalDue);
            // RMC 20141020 printing of OR (e)

            txtGrandTotal.Text = sGrandTotalDue;
            txtCreditLeft.Text = sCreditLeft;
        }

        public string FeesTerm(String sFeesCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            String sFeesTerm = "";

            try
            {
                if (sFeesCode.Substring(0, 1) == "B")
                {
                    //sFeesTerm = "Q";  // ALJ 12232005 put "//" function for reg fees only   
                    sFeesTerm = "Q";		// ALJ 12232005 chk if paid qtrly -- (due to change of fee/tax configuration from I to F)
                }
                else
                {
                    if (chkPartial.Checked == false)
                    {
                        pSet.Query = string.Format("select * from tax_and_fees_table where fees_code = '{0}'", sFeesCode); // CTS 09152003
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                sFeesTerm = pSet.GetString("fees_term").Trim();
                            }
                        pSet.Close();
                    }
                    else
                    {
                        pSet.Query = string.Format("select * from partial_fees where fees_code = '{0}'", sFeesCode); // CTS 09152003
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                sFeesTerm = pSet.GetString("fees_term").Trim();
                            }
                        pSet.Close();
                    }
                }
            }
            catch { }
            return sFeesTerm;
        }

        public string FeesWithPen(String sFeesCode)
        {
            OracleResultSet pSet = new OracleResultSet();

            String sQuery;
            String sFeesWithPen = "";

            try
            {
                if (sFeesCode.Substring(0, 1) == "B")
                {
                    sFeesWithPen = "Y";
                }
                else
                {
                    pSet.Query = string.Format("select * from tax_and_fees_table where fees_code = '{0}'", sFeesCode); // CTS 09152003
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sFeesWithPen = pSet.GetString("fees_withpen").Trim();
                        }
                    pSet.Close();
                }
            }
            catch { }
            return sFeesWithPen;
        }

        public string FeesWithSurch(String sFeesCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            String sFeesWithSurch = "";

            try
            {
                if (sFeesCode.Substring(0, 1) == "B")
                {
                    sFeesWithSurch = "Y";
                }
                else
                {
                    pSet.Query = string.Format("select * from tax_and_fees_table where fees_code = '{0}'", sFeesCode); // CTS 09152003
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sFeesWithSurch = pSet.GetString("fees_withsurch").Trim();
                        }
                    pSet.Close();
                }
            }
            catch { }
            return sFeesWithSurch;
        }

        private void DisplayDuesNew(String sORNo, String sTerm)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            String sQuery = "";
            String sRange1 = "", sRange2 = "";

            String sQtrPaid = "";

            PayTempStruct p;
            p.due_state = "";
            p.qtr_to_pay = "";

            double dTotalDiscountAmount;
            dTotalDiscountAmount = 0;
            int iSwDiscounted = 0;
            sQuery = string.Format("select * from pay_temp where bin = '{0}' and qtr_to_pay = 'P' order by tax_year,qtr_to_pay,qtr,fees_code_sort", m_sBIN);

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    p.bin = pSet.GetString("bin");
                    p.tax_year = pSet.GetString("tax_year");
                    p.term = pSet.GetString("term");
                    if (p.term == "I")
                        p.term = "Q";
                    p.qtr = pSet.GetString("qtr");
                    if (p.qtr == "F")
                        p.qtr = "Y";
                    p.fees_desc = pSet.GetString("fees_desc");
                    //p.fees_due = MathUtilities.RoundValue(pSet.GetDouble("fees_due"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                    //p.fees_surch = MathUtilities.RoundValue(pSet.GetDouble("fees_surch"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                    //p.fees_pen = MathUtilities.RoundValue(pSet.GetDouble("fees_pen"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                    //p.fees_totaldue = MathUtilities.RoundValue(pSet.GetDouble("fees_totaldue"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");

                    //AFM 20200730 MAO-20-13358 aligned with View SOA module to display 3 decimals (s)
                    p.fees_due = pSet.GetDouble("fees_due").ToString("##0.000");
                    p.fees_surch = pSet.GetDouble("fees_surch").ToString("##0.000");
                    p.fees_pen = pSet.GetDouble("fees_pen").ToString("##0.000");
                    p.fees_totaldue = pSet.GetDouble("fees_totaldue").ToString("##0.000");
                    //AFM 20200730 MAO-20-13358 aligned with View SOA module to display 3 decimals (e)

                    p.fees_code = pSet.GetString("fees_code");
                    p.due_state = pSet.GetString("due_state");
                    p.qtr_to_pay = pSet.GetString("qtr_to_pay");

                    dgvTaxFees.Rows.Add(p.tax_year, p.term, p.qtr, p.fees_desc, p.fees_due, p.fees_surch, p.fees_pen, p.fees_totaldue, p.due_state, p.qtr_to_pay, p.fees_code);

                    /* Discount
                    String sDiscountedAmount;
                    if (iSwDiscounted == 1)
                    {
                        String sTestFeesCode = p.fees_code;
                        String sDiscountRoundOff;
                        double dDiscountResult = 0;
                        if (((p.fees_code.Substring(0,1) == "B" && p.fees_code != "B2203") || p.fees_code.Left(2) == "13") && p.tax_year == m_sCurrentTaxYear && p.due_state != "A" && p.due_state != "X" && p.due_state != "P")
                        {
                            dDiscountResult = atof(p.fees_due) * m_dDiscountRate;	// CTS 01182005 fix bugs on 0.01 difference
                            sDiscountRoundOff.Format("%0.2f", dDiscountResult); // CTS 01182005 fix bugs on 0.01 difference
                            sDiscountedAmount.Format("%0.2f", atof(p.fees_due) - atof(sDiscountRoundOff));	// CTS 01182005 fix bugs on 0.01 difference
                            dTotalDiscountAmount = dTotalDiscountAmount + atof(sDiscountRoundOff); // CTS 01182005 fix bugs on 0.01 difference
                            mc_vsfaFlexPayment.SetTextMatrix((mc_vsfaFlexPayment.GetRows()) - 1, 11, sDiscountedAmount);
                        }
                        else
                            mc_vsfaFlexPayment.SetTextMatrix((mc_vsfaFlexPayment.GetRows()) - 1, 11, "0.00");
                    }
                    */
                }
            }
            pSet.Close();

            iSwDiscounted = 0;
            ComputeTotal();
            txtDiscountAmount.Text = dTotalDiscountAmount.ToString();

            txtGrandTotal.Text = txtDiscountAmount.Text;
            CompLessCredit(txtDiscountAmount.Text, txtCreditLeft.Text, txtToTaxCredit.Text);
        }

        private void ComputeTotal()
        {
            double m_dTotalDue, m_dTotalSurcharge, m_dTotalPenalty, m_dTotalTotalDue;

            m_dTotalDue = 0;
            m_dTotalSurcharge = 0;
            m_dTotalPenalty = 0;
            m_dTotalTotalDue = 0;

            if (dgvTaxFees.Rows.Count > 0)
                for (int i = 0; i < dgvTaxFees.RowCount; i++)
                {
                    m_dTotalDue += Convert.ToDouble(dgvTaxFees.Rows[i].Cells[4].Value);
                    m_dTotalSurcharge += Convert.ToDouble(dgvTaxFees.Rows[i].Cells[5].Value);
                    m_dTotalPenalty += Convert.ToDouble(dgvTaxFees.Rows[i].Cells[6].Value);
                    m_dTotalTotalDue += Convert.ToDouble(dgvTaxFees.Rows[i].Cells[7].Value);
                }


            txtTotDue.Text = m_dTotalDue.ToString("#,##0.00");
            txtTotSurch.Text = m_dTotalSurcharge.ToString("#,##0.00");
            txtTotPen.Text = m_dTotalPenalty.ToString("#,##0.00");
            txtTotTotDue.Text = m_dTotalTotalDue.ToString("#,##0.00");

            txtGrandTotal.Text = m_dTotalTotalDue.ToString("#,##0.00");

            /*
            (s) JJP 10142004 DISCOUNT FOR MANILA
            m_sDiscountedTotalDue.Format("%0.2f", (float)m_dTotalDue - atof(mv_sDiscount));
            m_sDiscountedSurcharge = m_dTotalSurcharge;
            m_sDiscountedPenalty = m_dTotalPenalty;
            (e) JJP 10142004 DISCOUNT FOR MANILA
            */

            //txtDiscountAmount.Text = string.Format("{0}", m_dTotalTotalDue - Convert.ToDouble(txtDiscountAmount.Text));
            txtDiscountAmount.Text = txtTotTotDue.Text; // RMC 20141020 printing of OR, disabled txtDiscountAmount setting of values
        }

        private void ComputeTaxAndFees()
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;

            TaxDuesStruct t;
            PayTempStruct p;
            String sBnsCodeMain;

            pSet.Query = string.Format("delete from pay_temp where bin = '{0}' and qtr_to_pay = 'P'", m_sBIN);
            pSet.ExecuteNonQuery();

            pSet.Query = string.Format("select * from or_table where or_no = '{0}' order by fees_code", txtORNo.Text);
            if (pSet.Execute())
            {
                p.bin = m_sBIN;
                p.term = m_sPaymentTerm;
                while (pSet.Read())
                {
                    p.tax_year = pSet.GetString("tax_year");
                    p.qtr = pSet.GetString("qtr_paid");
                    p.fees_code = pSet.GetString("fees_code");
                    p.fees_due = pSet.GetDouble("fees_due").ToString();
                    p.fees_surch = pSet.GetDouble("fees_surch").ToString();
                    p.fees_pen = pSet.GetDouble("fees_pen").ToString();
                    p.fees_totaldue = pSet.GetDouble("fees_amtdue").ToString();
                    p.bns_code = pSet.GetString("bns_code_main");

                    if (m_sPaymentTerm == "I")
                        p.term = FeesTerm(p.fees_code);

                    if (p.fees_code.Substring(0, 1) == "B")
                    {
                        p.fees_code = p.fees_code.Trim() + p.bns_code.Trim();
                        p.fees_desc = AppSettingsManager.GetBnsDesc(p.bns_code);
                        p.fees_desc = "TAX ON " + p.fees_desc;
                        p.fees_code_sort = "00";
                    }
                    else
                    {
                        if (p.fees_code == "01")
                        {
                            p.fees_desc = AppSettingsManager.GetFeesDesc(p.fees_code);
                            p.fees_code = p.fees_code + "-" + p.bns_code;
                            p.fees_desc = p.fees_desc + "(" + p.bns_code + ")";
                            p.fees_code_sort = p.fees_code;
                        }
                        else
                        {
                            p.fees_desc = AppSettingsManager.GetFeesDesc(p.fees_code);
                            p.fees_code_sort = p.fees_code;
                        }
                    }
                    p.due_state = "";
                    p.qtr_to_pay = "P";

                    sQuery = "insert into pay_temp(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,fees_code_sort) values ";
                    sQuery += "('" + StringUtilities.HandleApostrophe(p.bin) + "', ";
                    sQuery += " '" + StringUtilities.HandleApostrophe(p.tax_year) + "', ";
                    sQuery += " '" + StringUtilities.HandleApostrophe(p.term) + "', ";
                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr) + "', ";
                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                    sQuery += Convert.ToDouble(p.fees_due).ToString("#00.00") + ", ";
                    sQuery += Convert.ToDouble(p.fees_surch).ToString("#00.00") + ", ";
                    sQuery += Convert.ToDouble(p.fees_pen).ToString("#00.00") + ", ";
                    sQuery += Convert.ToDouble(p.fees_totaldue).ToString("#00.00") + ", ";
                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code) + "', ";
                    sQuery += " '" + StringUtilities.HandleApostrophe(p.due_state) + "', ";
                    sQuery += " '" + StringUtilities.HandleApostrophe(p.qtr_to_pay) + "', ";
                    sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_code_sort) + "')";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
            }
            pSet.Close();
        }

        public void ComputePenRate(String sTaxYear, String sQtr, String sDueState
            , double p_dSurchRate1, double p_dSurchRate2, double p_dSurchRate3, double p_dSurchRate4
            , double p_dPenRate1, double p_dPenRate2, double p_dPenRate3, double p_dPenRate4
            , double p_SurchQuart, double p_PenQuart)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            String sJan, sFeb, sMar, sApr, sMay, sJun, sJul, sAug, sSep, sOct, sNov, sDec;
            DateTime Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec;
            double dSurchRate1, dSurchRate2, dSurchRate3, dSurchRate4;
            double dPenRate1, dPenRate2, dPenRate3, dPenRate4;
            double SurchQuart, PenQuart;
            DateTime cdtDtOperated, cdtDtO1st, cdtDtO2nd, cdtDtO3rd, cdtDtO4th, cdtQtrDate;
            String sDtOperatedYear;
            dSurchRate1 = 0;
            dSurchRate2 = 0;
            dSurchRate3 = 0;
            dSurchRate4 = 0;
            dPenRate1 = 0;
            dPenRate2 = 0;
            dPenRate3 = 0;
            dPenRate4 = 0;
            SurchQuart = 0;
            PenQuart = 0;

            PenRate pr;

            String sMaxYear = "", m_sORDate;
            m_sORDate = dtpORDate.Value.ToString("MM/dd/yyyy");

            bool boRetStatus = false;
            pSet.Query = "select max(tax_year) as max_yr from pay_hist where bin = '" + m_sBIN + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sMaxYear = pSet.GetString("max_yr");
            pSet.Close();

            DateTime sDueJan = new DateTime(), sDueFeb = new DateTime(), sDueMar = new DateTime(), sDueApr = new DateTime(), sDueMay = new DateTime(), sDueJun = new DateTime(), sDueJul = new DateTime(), sDueAug = new DateTime(), sDueSep = new DateTime(), sDueOct = new DateTime(), sDueNov = new DateTime(), sDueDec = new DateTime();
            pSet.Query = string.Format("select * from due_dates where due_year = '{0}'", AppSettingsManager.GetConfigValue("12"));
            if (pSet.Execute())
                while (pSet.Read())
                {
                    String sCode = pSet.GetString("due_code").Trim();
                    if (sCode == "01")
                        sDueJan = pSet.GetDateTime("due_date");
                    else if (sCode == "02")
                        sDueFeb = pSet.GetDateTime("due_date");
                    else if (sCode == "03")
                        sDueMar = pSet.GetDateTime("due_date");
                    else if (sCode == "04")
                        sDueApr = pSet.GetDateTime("due_date");
                    else if (sCode == "05")
                        sDueMay = pSet.GetDateTime("due_date");
                    else if (sCode == "06")
                        sDueJun = pSet.GetDateTime("due_date");
                    else if (sCode == "07")
                        sDueJul = pSet.GetDateTime("due_date");
                    else if (sCode == "08")
                        sDueAug = pSet.GetDateTime("due_date");
                    else if (sCode == "09")
                        sDueSep = pSet.GetDateTime("due_date");
                    else if (sCode == "10")
                        sDueOct = pSet.GetDateTime("due_date");
                    else if (sCode == "11")
                        sDueNov = pSet.GetDateTime("due_date");
                    else if (sCode == "12")
                        sDueDec = pSet.GetDateTime("due_date");
                }
            pSet.Close();

            if (sDueState == "N")
            {
                m_sNewWithQtr = AppSettingsManager.GetConfigValue("15").Trim();

                if (m_sNewWithQtr == "N")
                {

                }
            }

            String sQtrPaid = "";
            pSet.Query = "select * from pay_hist where bin = '" + m_sBIN + "' order by tax_year desc, qtr_paid desc";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sQtrPaid = pSet.GetString("qtr_paid");
                    if (sQtrPaid == "F")
                        sQtrPaid = "4";
                }
            pSet.Close();

            m_sCurrentTaxYear = dtpORDate.Value.Year.ToString();
            cdtDtOperated = Convert.ToDateTime(m_sDtOperated);
            sDtOperatedYear = sTaxYear;

            String sDtO1st, sDtO2nd, sDtO3rd, sDtO4th;

            String sEndingDay;
            sEndingDay = m_sMonthlyCutoffDate;
            if (Convert.ToInt32(m_sMonthlyCutoffDate) > 30)
                sEndingDay = "30";

            sDtO1st = sDueJan.ToString("MM/dd/").Substring(0, 6) + sDtOperatedYear;
            sDtO2nd = sDueApr.ToString("MM/dd/").Substring(0, 6) + sDtOperatedYear;
            sDtO3rd = sDueJul.ToString("MM/dd/").Substring(0, 6) + sDtOperatedYear;
            sDtO4th = sDueOct.ToString("MM/dd/").Substring(0, 6) + sDtOperatedYear;

            cdtDtO1st = Convert.ToDateTime(sDtO1st);
            cdtDtO2nd = Convert.ToDateTime(sDtO2nd);
            cdtDtO3rd = Convert.ToDateTime(sDtO3rd);
            cdtDtO4th = Convert.ToDateTime(sDtO4th);
            cdtQtrDate = new DateTime();

            if (sQtr == "1")
                cdtQtrDate = Convert.ToDateTime(sDtO1st);
            if (sQtr == "2")
                cdtQtrDate = Convert.ToDateTime(sDtO2nd);
            if (sQtr == "3")
                cdtQtrDate = Convert.ToDateTime(sDtO3rd);
            if (sQtr == "4")
                cdtQtrDate = Convert.ToDateTime(sDtO4th);

            String sBaseYear;
            for (int dYear = Convert.ToInt16(sTaxYear); dYear <= Convert.ToInt16(m_sCurrentTaxYear); dYear++)
            {
                sBaseYear = dYear.ToString();

                sJan = sDueJan.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sFeb = sDueFeb.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sMar = sDueMar.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sApr = sDueApr.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sMay = sDueMay.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sJun = sDueJun.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sJul = sDueJul.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sAug = sDueAug.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sSep = sDueSep.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sOct = sDueOct.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sNov = sDueNov.ToString("MM/dd/").Substring(0, 6) + sBaseYear;
                sDec = sDueDec.ToString("MM/dd/").Substring(0, 6) + sBaseYear;

                Jan = Convert.ToDateTime(sJan);
                Feb = Convert.ToDateTime(sFeb);
                Mar = Convert.ToDateTime(sMar);
                Apr = Convert.ToDateTime(sApr);
                May = Convert.ToDateTime(sMay);
                Jun = Convert.ToDateTime(sJun);
                Jul = Convert.ToDateTime(sJul);
                Aug = Convert.ToDateTime(sAug);
                Sep = Convert.ToDateTime(sSep);
                Oct = Convert.ToDateTime(sOct);
                Nov = Convert.ToDateTime(sNov);
                Dec = Convert.ToDateTime(sDec);

                if (sDueState == "Q")
                {
                    if (sQtr == "1")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO1st);
                        cdtDtOperated = Convert.ToDateTime(sDtO1st);
                    }
                    if (sQtr == "2")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO2nd);
                        cdtDtOperated = Convert.ToDateTime(sDtO2nd);
                    }
                    if (sQtr == "3")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO3rd);
                        cdtDtOperated = Convert.ToDateTime(sDtO3rd);
                    }
                    if (sQtr == "4")
                    {
                        cdtQtrDate = Convert.ToDateTime(sDtO4th);
                        cdtDtOperated = Convert.ToDateTime(sDtO4th);
                    }
                }

                mv_cdtORDate = dtpORDate.Value;
                if (mv_cdtORDate > Jan || m_sORDate == sJan)
                {

                    if (m_sORDate == sJan)
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dSurchRate1 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate2 += m_dPenRate;
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Jan && cdtQtrDate <= Jan)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (mv_cdtORDate > Feb || m_sORDate == sFeb)
                {
                    if (m_sORDate == sFeb)
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dPenRate1 = dPenRate1 + m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate2 += m_dPenRate;
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Feb && cdtQtrDate <= Feb)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (mv_cdtORDate > Mar || m_sORDate == sMar)
                {
                    if (m_sORDate == sMar)
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dPenRate1 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate2 += m_dPenRate;
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Mar && cdtQtrDate <= Mar)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (mv_cdtORDate > Apr || m_sORDate == sApr)
                {
                    if (m_sORDate == sApr)
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dSurchRate2 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Apr && cdtQtrDate <= Apr)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (mv_cdtORDate > May || m_sORDate == sMay)
                {
                    if (m_sORDate == sMay)
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= May && cdtQtrDate <= May)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (mv_cdtORDate > Jun || m_sORDate == sJun)
                {
                    if (m_sORDate == sJun)
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Jun && cdtQtrDate <= Jun)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (mv_cdtORDate > Jul || m_sORDate == sJul)
                {
                    if (m_sORDate == sJul)
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dSurchRate3 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;

                            if (sBaseYear != sTaxYear)
                                dPenRate4 += m_dPenRate;
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Jul && cdtQtrDate <= Jul)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }


                if (mv_cdtORDate > Aug || m_sORDate == sAug)
                {
                    if (m_sORDate == sAug)
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            if (sBaseYear != sTaxYear)
                                dPenRate4 += m_dPenRate;
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Aug && cdtQtrDate <= Aug)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (mv_cdtORDate > Sep || m_sORDate == sSep)
                {
                    if (m_sORDate == sSep)
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;

                            if (sBaseYear != sTaxYear)
                                dPenRate4 += m_dPenRate;
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Sep && cdtQtrDate <= Sep)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }


                if (mv_cdtORDate > Oct || m_sORDate == sOct)
                {
                    if (m_sORDate == sOct)
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt16(sTaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt16(sMaxYear) < Convert.ToInt16(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate4 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Oct && cdtQtrDate <= Oct)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (mv_cdtORDate > Nov || m_sORDate == sNov)
                {
                    if (m_sORDate == sNov)
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Nov && cdtQtrDate <= Nov)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (mv_cdtORDate > Dec || m_sORDate == sDec)
                {
                    if (m_sORDate == sDec)
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                        }
                        if (sDueState == "Q")
                        {
                            if (cdtDtOperated <= Dec && cdtQtrDate <= Dec)
                            {
                                SurchQuart = m_dSurchRate;
                                PenQuart += m_dPenRate;
                            }
                        }
                    }
                }

                if (dPenRate1 > 0.72)
                    dPenRate1 = 0.72;
                if (dPenRate2 > 0.72)
                    dPenRate2 = 0.72;
                if (dPenRate3 > 0.72)
                    dPenRate3 = 0.72;
                if (dPenRate4 > 0.72)
                    dPenRate4 = 0.72;
                if (PenQuart > 0.72)
                    PenQuart = 0.72;

                pr.p_dSurchRate1 = dSurchRate1;
                pr.p_dSurchRate2 = dSurchRate2;
                pr.p_dSurchRate3 = dSurchRate3;
                pr.p_dSurchRate4 = dSurchRate4;
                pr.p_dPenRate1 = dPenRate1;
                pr.p_dPenRate2 = dPenRate2;
                pr.p_dPenRate3 = dPenRate3;
                pr.p_dPenRate4 = dPenRate4;
                pr.p_SurchQuart = SurchQuart;
                pr.p_PenQuart = PenQuart;
            }
        }

        private void CleanMe()
        {
            iAgreePayTerm = 0;
            txtCompromise.Text = "0";
            bin1.txtTaxYear.Text = string.Empty;
            bin1.txtBINSeries.Text = string.Empty;
            txtORNo.Text = string.Empty;

            m_sMonthlyCutoffDate = "";
            m_sInitialDueDate = "";
            m_dPenRate = 0;
            m_dSurchRate = 0;

            btnPrint.Enabled = false;
            txtBnsName.Text = string.Empty;
            txtCompromise.Enabled = false;
            chkPartial.Checked = false;
            chkPartial.Enabled = false;

            SetCheckTerm(chkFull);
            m_sPaymentTerm = "F";
            m_sPaymentType = "CS";

            txtBnsAddress.Text = string.Empty;
            txtBnsOwner.Text = string.Empty;
            txtBnsType.Text = string.Empty;
            txtBnsCode.Text = string.Empty;
            txtBnsStat.Text = string.Empty;
            txtTaxYear.Text = string.Empty;
            txtTotDue.Text = string.Empty;
            txtTotSurch.Text = string.Empty;
            txtTotPen.Text = string.Empty;
            txtTotTotDue.Text = string.Empty;
            txtTotDues.Text = string.Empty;
            txtCreditLeft.Text = string.Empty;
            txtToTaxCredit.Text = string.Empty;
            txtGrandTotal.Text = string.Empty;
            chkFull.Checked = false;
            chkInstallment.Checked = false;
            chk1st.Checked = false;
            chk2nd.Checked = false;
            chk3rd.Checked = false;
            chk4th.Checked = false;
            chkCash.Checked = false;
            chkCheque.Checked = false;
            chkTCredit.Checked = false;
            dgvTaxFees.Rows.Clear();
            m_sBIN = string.Empty;
            bin1.txtTaxYear.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery, sORPrinter;

            if (txtReason.Text.Trim() == "")
            {
                MessageBox.Show("Please enter reason for reprinting.", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                pSet.Query = string.Format("insert into or_reprint values ('{0}','{1}','{2}',sysdate,'{3}')", m_sBIN, txtORNo.Text, txtReason.Text.ToUpper(), AppSettingsManager.SystemUser.UserCode);  // RMC 20140909 Migration QA, added toupper in reason 
                pSet.ExecuteNonQuery();

                AuditTrail.InsertTrail("CRRO", "multiple table", m_sBIN + "/" + txtORNo.Text.Trim());

                CheckPaymentType();
                frmLiqReports frmliqreports = new frmLiqReports();
                frmliqreports.m_bReprintOR = true;
                frmliqreports.ReportSwitch = "ReprintOR";
                frmliqreports.DataGridView = dgvTaxFees;
                frmliqreports.m_sBrgy = AppSettingsManager.GetBnsBrgy(m_sBIN);
                //frmliqreports.m_sBIN = bin1.GetBin();
                frmliqreports.formUse = "Not";

                //frmliqreports.m_sBIN = "021-02-" + bin1.GetBINSeries.ToString(); // Edit JMBC 20141001 reason couldn't get the correct bin number
                frmliqreports.m_sBIN = bin1.GetBin();   // RMC 20141020 printing of OR
                frmliqreports.m_sTaxYear = txtTaxYear.Text;
                frmliqreports.m_sStatus = txtBnsStat.Text;
                //JMBC 20140926
                frmliqreports.BCode = txtBnsCode.Text;
                frmliqreports.ORDate = dtpORDate.Value.ToShortDateString();
                frmliqreports.ORNo = txtORNo.Text;
                //JMBC 20141001
                frmliqreports.bnsAddress = txtBnsAddress.Text;
                frmliqreports.bnsName = txtBnsName.Text;
                frmliqreports.bnsOwner = txtBnsOwner.Text;
                frmliqreports.ownCode = m_sOwnCode;
                frmliqreports.term = "Installment";
                // RMC 20141020 printing of OR (s)
                frmliqreports.Teller = txtTeller.Text;   
                frmliqreports.PaymentType = m_sPaymentType; 
                double dToBeCredited = 0;
                double.TryParse(txtToTaxCredit.Text.ToString(), out dToBeCredited);    
                frmliqreports.ToBeCredited = dToBeCredited;
                // RMC 20141020 printing of OR (e)
                 
                if (chkFull.Checked == true)
                {
                    frmliqreports.term = "Full";
                }
                
                //if (MessageBox.Show("Do you want to use pre-printed Form?", "BPLS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)    // RMC 20141020 printing of OR
                //    frmliqreports.formUse = "Reprinted";

                frmliqreports.ShowDialog();
            }
        }

        private void dgvTaxFees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}