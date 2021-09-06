using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.RPTA.Classes.Bank;
using Amellar.Common.SearchOwner;

namespace Amellar.Modules.Payment
{
    public partial class frmPaymentMultiCheck : Form
    {
        public frmPaymentMultiCheck()
        {
            InitializeComponent();
        }

        private string m_sAcctFN = "";
        private string m_sAcctLN = "";
        private string m_sAcctMI = "";
        private string m_sAcctNo = "";
        private string m_sBankBranch = "";
        private string m_sBankNm = "";
        private string m_sChkAmt = ""; // ALJ 01262005 remove display of "0.00"
        private string m_sCashAmt = ""; // ALJ 01262005
        private string m_sBankCode = "";
        private string m_sChkNo = "";
        private bool m_bGuarantor = false;
        
        public string AcctFN
        {
            set { m_sAcctFN = value; }
        }
        public string AcctLN
        {
            set { m_sAcctLN = value; }
        }
        public string AcctMI
        {
            set { m_sAcctMI = value; }
        }
        public string AcctNo
        {
            set { m_sAcctNo = value; }
        }
        public string BankBranch
        {
            set { m_sBankBranch = value; }
        }
        public string BankName
        {
            set { m_sBankNm = value; }
        }
        public string BankCode
        {
            set { m_sBankCode = value; }
        }
        public string ChkCode
        {
            set { m_sChkNo = value; }
        }
        public string ChkAmount
        {
            set { m_sChkAmt = value; }
        }
        public string CashAmount
        {
            set { m_sChkAmt = value; }
        }
        public bool Guarantor
        {
            set { m_bGuarantor = value; }
        }

       
        string sCheckType = "";
        private string m_sFlagAddlCheck = "";
        private string m_sCheckAmount = "";
        public string FlagAddlCheck
        {
            get { return m_sFlagAddlCheck; }
            set { m_sFlagAddlCheck = value; }
        }
        private string m_sOwnCode = "";
        private string sTempOwnCode = "";
        private string m_sORNo = "";
        private string m_sORDate = "";
        public string ORNo
        {
            set { m_sORNo = value; }
        }
        public string ORDate
        {
            set { m_sORDate = value; }
        }
        private string m_sBin = "";
        public string BIN
        {
            set { m_sBin = value; }
        }
        private string m_sPaymentType = "";
        public string PaymentType
        {
            get { return m_sPaymentType; }
            set { m_sPaymentType = value; }
        }
        private string sTempChkNo = "";

        private string m_sTotalTaxDue = "";
        public string TotalTaxDue
        {
            get { return m_sTotalTaxDue; }
            set { m_sTotalTaxDue = value; }
        }

        private string m_sTotalChkAmount = "";
        public string TotalChkAmount
        {
            get { return m_sTotalChkAmount; }
            set { m_sTotalChkAmount = value; }
        }

        private string m_sTotalCashAmount = "";
        public string TotalCashAmount
        {
            get { return m_sTotalCashAmount; }
            set { m_sTotalCashAmount = value; }
        }

        private string m_sTotalDBCR = "";
        public string TotalDBCR
        {
            get { return m_sTotalDBCR; }
            set { m_sTotalDBCR = value; }
        }

        private string m_sTotalRemBalance = "";
        public string TotalRemBalance
        {
            get { return m_sTotalRemBalance; }
            set { m_sTotalRemBalance = value; }
        }

        public string OwnCode
        {
            get { return m_sOwnCode; }
            set { m_sOwnCode = value; }
        }
        public bool bMultiPayFound = false;
        public bool bInsufficient = false;
        public bool bRefCheck = false;

        private void frmPaymentMultiCheck_Load(object sender, EventArgs e)
        {
            dtpChkDate.Value = AppSettingsManager.GetSystemDate();
            dtpChkType.Value = AppSettingsManager.GetSystemDate();

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            String sHead, sWidth;
            String sQuery, sItem;

            btnAddOwner.Enabled = false;
            btnAcctNo.Enabled = false;
            btnChkNo.Enabled = false;
            sCheckType = "";

            txtChkInfoBalance.Text = "0.00";
            txtChkInfoTotAmountColl.Text = "0.00";
            txtChkInfoAmount.Text = "0.00";
            
            txtTotTaxDue.Text = m_sTotalTaxDue;

            DeacControls();

            chkAttached.Checked = false;
            bMultiPayFound = false;
            bRefCheck = false;
            chkRef.Checked = false;
            sTempOwnCode = m_sOwnCode;

            m_sFlagAddlCheck = "N";

            UpdateList("");
            if (dgView.RowCount != 0)
            {
                UpdateControlValues();

                String sMultiPay = "", sChkNo = "";

                sQuery = "select * from chk_tbl_temp where or_no = '" + m_sORNo + "'";
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sChkNo = pSet.GetString("chk_no").Trim();

                        sQuery = "select * from multi_check_pay where chk_no = '" + sChkNo + "' and used_sw = 'Y'";
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                            {
                                pSet1.Close();
                                sQuery = "select own_code from multi_check_pay where chk_no = '" + sChkNo + "' and used_sw = 'Y'";
                                pSet1.Query = sQuery;
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                        //sTempOwnCode = pSet1.GetString("own_code").Trim();
                                        sTempOwnCode = pSet1.GetString("acct_no").Trim();   // RMC 20170210 corrected error in additional payment when using multiple check transaction
                                pSet1.Close();

                                bMultiPayFound = true;
                                bRefCheck = true;
                                chkRef.Checked = true;
                                //if (pSet->GetRecordCount() - 1 > 1) // CTS 01092005 fix bugs on move last record
                                //    pSet.MoveLast();
                            }
                        pSet1.Close();

                        sQuery = "select * from multi_check_pay where chk_no = '" + sChkNo + "' and used_sw = 'N'";
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                            {
                                pSet1.Close();
                                sQuery = "select own_code from multi_check_pay where chk_no = '" + sChkNo + "' and used_sw = 'N'";
                                pSet1.Query = sQuery;
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                        //sTempOwnCode = pSet1.GetString("own_code").Trim();
                                        //sTempOwnCode = pSet1.GetString("acct_no").Trim();   // RMC 20170210 corrected error in additional payment when using multiple check transaction
                                        sTempOwnCode = pSet1.GetString("own_code").Trim(); //AFM 20200428 corrected
                                pSet1.Close();

                                bMultiPayFound = true;
                            }

                        //if (pSet->GetRecordCount() - 1 > 1) // CTS 01092005 fix bugs on move last record
                        //    pSet->MoveLast();
                    }
                }
                pSet.Close();

                if (bMultiPayFound == true)
                {
                    btnAdd.Enabled = false;
                    if (bRefCheck == true)
                    {
                        btnEdit.Enabled = false; 
                        chkRef.Checked = true;
                    }
                }
            }
        }

        private void DeacControls()
        {
            rdoChk1.Checked = false;
            rdoChk2.Checked = false;
            rdoChk3.Checked = false;

            chkGuarantor.Checked = false;
            chkRef.Checked = false;
            btnAddOwner.Enabled = false;
            btnAcctNo.Enabled = false;
            btnChkNo.Enabled = false;
            chkRef.Enabled = false;

            rdoChk1.Enabled = false;
            rdoChk2.Enabled = false;
            rdoChk3.Enabled = false;
            chkGuarantor.Enabled = false;

            btnFind.Enabled = false;
            txtChkNo.Enabled = false;
            txtChkCashAmount.Enabled = false;
            txtChkAmount.Enabled = false;
            dtpChkDate.Enabled = false;
            txtAcctNo.Enabled = false;
            txtLN.Enabled = false;
            txtFN.Enabled = false;
            txtMI.Enabled = false;

            if (dgView.RowCount == 0)
            {
                bMultiPayFound = false;
                bRefCheck = false; 
            }
        }

        private void Initialize()
        {
            txtFN.Text = "";
            txtLN.Text = "";
            txtMI.Text = "";
            txtAcctNo.Text = "";
            txtBankBranch.Text = "";
            txtBankName.Text = "";
            txtChkAmount.Text = "";
            txtChkCashAmount.Text = "";
            txtBankCode.Text = "";
            txtChkNo.Text = "";
            dtpChkDate.Value = AppSettingsManager.GetSystemDate();
            dtpChkType.Value = AppSettingsManager.GetSystemDate();

            m_bGuarantor = false;
        }

        private void AcControls()
        {
            rdoChk1.Enabled = true;
            rdoChk2.Enabled = true;
            rdoChk3.Enabled = true;
            chkGuarantor.Enabled = true;

            btnFind.Enabled = true;
            txtChkNo.Enabled = true;
            txtChkAmount.Enabled = true;
            dtpChkDate.Enabled = true;
            txtAcctNo.Enabled = true;
            txtLN.Enabled = true;
            txtFN.Enabled = true;
            txtMI.Enabled = true;

            txtChkAmount.ReadOnly = false;
            txtChkCashAmount.ReadOnly = false;
            txtAcctNo.ReadOnly = false;
            txtLN.ReadOnly = false;
            txtFN.ReadOnly = false;
            txtMI.ReadOnly = false;
        }

        private void UpdateList(String sQuery)
        {
            String sChkType = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;

            if (sQuery != "")
            {
                String sOwnCode = "", sChkNo = "", sChkDate = "", sBankCode = "", sAcctNo = "", sAcctLn, sAcctFn, sAcctMI, sOrNo = "", sOrDate = "", sTeller = "", sDateAccepted = "";
                double dChkAmt = 0, dChkCash = 0;

                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sOwnCode = pSet.GetString("sOwnCode").Trim();
                        sChkNo = pSet.GetString("chk_no").Trim();
                        sChkDate = pSet.GetString("chk_date").Trim();
                        dChkAmt = pSet.GetDouble("chk_amt");
                        dChkCash = pSet.GetDouble("cash_amt");
                        sBankCode = pSet.GetString("bank_code").Trim();
                        sAcctNo = pSet.GetString("acct_no").Trim();
                    }
                pSet.Close();

                if (sOwnCode != "")
                {
                    sQuery = "select * from own_names where own_code = '" + sOwnCode + "'";
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sAcctLn = pSet.GetString("own_ln").Trim();
                            sAcctFn = pSet.GetString("own_fn").Trim();
                            sAcctMI = pSet.GetString("own_mi").Trim();

                            sQuery = "insert into chk_tbl_temp(chk_no,chk_date,chk_type,chk_amt,cash_amt,bank_code,acct_no,acct_ln,acct_fn,acct_mi,or_no,or_date,teller,dt_accepted) values ";
                            sQuery += "('" + StringUtilities.HandleApostrophe(sChkNo) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(sChkDate) + "', ";
                            sQuery += "'" + sCheckType + "', ";
                            sQuery += dChkAmt.ToString("###.00") + ", ";
                            sQuery += dChkCash.ToString("###.00") + ", ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(sBankCode) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(sAcctNo) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(StringUtilities.Left(sAcctLn, 50)) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(StringUtilities.Left(sAcctFn, 50)) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(sAcctMI.Trim()) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(m_sORNo) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(m_sORDate) + "', ";
                            sQuery += " '" + StringUtilities.HandleApostrophe(AppSettingsManager.TellerUser.UserCode) + "', ";
                            sQuery += " '" + dtpChkType.Value.ToString("MM/dd/yyyy") + "') ";
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();
                        }
                    pSet.Close();
                }
            }

            dgView.Rows.Clear();
            sQuery = string.Format("select a.chk_no,to_char(a.chk_amt) as chk_amt,b.bank_nm,b.bank_branch,a.acct_no,a.bank_code,a.cash_amt,a.chk_type from chk_tbl_temp a,bank_table b where a.bank_code = b.bank_code and a.or_no = '{0}'", m_sORNo);
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    dgView.Rows.Add(pSet.GetString("chk_no").Trim(), pSet.GetString("chk_amt").Trim(), pSet.GetString("bank_nm").Trim(), pSet.GetString("bank_branch").Trim(), pSet.GetString("acct_no").Trim(), pSet.GetString("bank_code").Trim(), pSet.GetDouble("cash_amt"), pSet.GetString("chk_type").Trim());
                }
            pSet.Read();

            dgView.ClearSelection(); //MCR 20140724
        }

        private void UpdateControlValues()
        {
            String sQuery;
            OracleResultSet pSet = new OracleResultSet();

            if (dgView.RowCount != 0)
            {
                m_sCheckAmount = dgView.CurrentRow.Cells[1].Value.ToString();
                txtBankCode.Text = dgView.CurrentRow.Cells[5].Value.ToString();
                txtChkNo.Text = dgView.CurrentRow.Cells[0].Value.ToString();
                if (txtBankCode.Text != "")
                {
                    sQuery = string.Format("select * from  bank_table where bank_code = '{0}'", txtBankCode.Text);
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            txtBankName.Text = pSet.GetString("bank_nm").Trim();
                            txtBankBranch.Text = pSet.GetString("bank_branch").Trim();
                        }
                    pSet.Close();

                    sQuery = string.Format("select * from chk_tbl_temp where chk_no = '{0}'", txtChkNo.Text);
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            String sChkType; // CTS 12032004 add check type
                            txtAcctNo.Text = pSet.GetString("acct_no").Trim();
                            txtLN.Text = pSet.GetString("acct_ln").Trim();
                            txtFN.Text = pSet.GetString("acct_fn").Trim();
                            txtMI.Text = pSet.GetString("acct_mi").Trim();
                            txtChkAmount.Text = pSet.GetDouble("chk_amt").ToString("##0.00");
                            txtChkCashAmount.Text = pSet.GetDouble("cash_amt").ToString("##0.00");
                            dtpChkDate.Value = pSet.GetDateTime("chk_date");
                            sChkType = pSet.GetString("chk_type");

                            if (sChkType == "MC")
                            {
                                rdoChk1.Checked = true;
                                rdoChk2.Checked = false;
                                rdoChk3.Checked = false;
                            }

                            if (sChkType == "CC")
                            {
                                rdoChk1.Checked = false;
                                rdoChk2.Checked = true;
                                rdoChk3.Checked = false;
                            }

                            if (sChkType == "PC")
                            {
                                rdoChk1.Checked = false;
                                rdoChk2.Checked = false;
                                rdoChk3.Checked = true;
                            }

                            //sQuery = "select * from multi_check_pay where chk_no = '" + txtChkNo.Text + "'";
                            sQuery = "select * from multi_check_pay where chk_no = '" + txtChkNo.Text + "' and bank_code = '" + txtBankCode.Text + "'"; //AFM 20200601
                            pSet.Query = sQuery;
                            if (pSet.Execute())
                                if (pSet.Read())
                                {
                                    bMultiPayFound = true;
                                    chkGuarantor.Enabled = true;
                                    chkGuarantor.Checked = true;
                                }
                            pSet.Close();

                            //sQuery = "select * from multi_check_pay where chk_no = '" + txtChkNo.Text + "' and used_sw = 'Y'";
                            sQuery = "select * from multi_check_pay where chk_no = '" + txtChkNo.Text + "' and used_sw = 'Y' and bank_code = '" + txtBankCode.Text + "'"; //AFM 20200601
                            pSet.Query = sQuery;
                            if (pSet.Execute())
                                if (pSet.Read())
                                {
                                    chkRef.Enabled = true;
                                    chkRef.Checked = true;
                                    bRefCheck = true;
                                    chkRef.Checked = true;
                                }
                            pSet.Close();
                        }
                    pSet.Close();
                }
            }

            ComputeTotalChkAmt();
            GetRemainChkBalance();
        }

        private void ComputeTotalChkAmt()
        {
            double dChkAmt = 0, dCashAmt = 0;
            double dCashChkAmt = 0;

            txtTotChkAmount.Text = "0.00";
            txtTotCashAmount.Text = "0.00";
            txtDBCRAmout.Text = "0.00";

            for (int i = 0; i < dgView.RowCount; i++)
            {
                dChkAmt += Convert.ToDouble(dgView.Rows[i].Cells[1].Value);
                dCashAmt += Convert.ToDouble(dgView.Rows[i].Cells[6].Value);
            }

            txtTotCashAmount.Text = dCashAmt.ToString("#,##0.00");
            txtTotChkAmount.Text = dChkAmt.ToString("#,##0.00"); 

            dCashChkAmt = dChkAmt + dCashAmt;

            if (Convert.ToDouble(txtTotTaxDue.Text) > dCashChkAmt)
            {
                lblDBCR.Text = "Debit Amount";
                txtDBCRAmout.Text = Convert.ToDouble(Convert.ToDouble(txtTotTaxDue.Text) - (dChkAmt + dCashAmt)).ToString("#,##0.00");
            }

            if (Convert.ToDouble(txtTotTaxDue.Text) < dCashChkAmt)
            {
                lblDBCR.Text = "Credit Amount";
                txtDBCRAmout.Text = Convert.ToDouble((dChkAmt + dCashAmt) - Convert.ToDouble(txtTotTaxDue.Text)).ToString("#,##0.00");
            }

            if (Convert.ToDouble(txtDBCRAmout.Text) == 0.00 && lblDBCR.Text == "Debit Amount")
            {
                lblDBCR.Text = "Debit/Credit Amount";
            }

            if (dgView.RowCount == 0)
            {
                lblDBCR.Text = "Debit/Credit Amount";
                txtDBCRAmout.Text = "0.00";
            }
        }

        private void FocusCurrentValue() // MCR 20140904 Select Current Value
        {
            string sTemp = txtChkNo.Text.Trim();
            for (int i = 0; i != dgView.RowCount; i++)
            {
                if (dgView.Rows[i].Cells[0].Value.ToString().Trim() == sTemp)
                {
                    dgView.Rows[i].Selected = true;
                    dgView.CurrentCell = dgView.Rows[i].Cells[0];
                    dgView_SelectionChanged(dgView, new DataGridViewCellEventArgs(0, i));
                    break;
                }
            }
        }

        private void GetRemainChkBalance()
        {
            String sQuery, sChkNo, sBankCode;
            double dTotalChkAmt = 0, dTotalCollected = 0, dRemaining = 0;
            double dTotalChkAmtTmp = 0, dTotalCollectedTmp = 0; // ALJ 12292004
            double dTotalAmtUsed = 0; //JHB 20190121 merge from LAL-LO JARS 20180115

            OracleResultSet pSet = new OracleResultSet();

            if (bMultiPayFound == true)
            {
                for (int i = 0; i < dgView.RowCount; i++)
                {
                    sChkNo = dgView.Rows[i].Cells[0].Value.ToString();
                    sBankCode = dgView.Rows[i].Cells[5].Value.ToString();
                    sQuery = "select * from multi_check_pay where chk_no = '" + sChkNo + "' and bank_code = '" + txtBankCode.Text.Trim() + "'"; //AFM 20200603 added bank code
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                            dTotalChkAmtTmp = pSet.GetDouble("chk_amt");
                    pSet.Close();
                    dTotalChkAmt += dTotalChkAmtTmp;

                    //sQuery = "select sum(chk_amt) as chk_amount from chk_tbl where chk_no = '" + sChkNo + "'";
                    sQuery = "select sum(chk_amt) as chk_amount from chk_tbl where chk_no = '" + sChkNo + "' and bank_code = '" + sBankCode + "'"; //AFM 20200428 added bank code to avoid same check diff bank
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                            dTotalCollectedTmp = pSet.GetDouble("chk_amount");
                    pSet.Close();
                    dTotalCollected += dTotalCollectedTmp;
                }

                for (int i = 0; i < dgView.RowCount; i++)
                {
                    sChkNo = dgView.Rows[i].Cells[0].Value.ToString();
                    sQuery = "select * from multi_check_pay where chk_no = '" + sChkNo + "' and bank_code = '" + txtBankCode.Text.Trim() + "'"; //AFM 20200603 added bank code
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                            dRemaining = dTotalChkAmt - (dTotalCollected + Convert.ToDouble(dgView.Rows[i].Cells[1].Value));
                    pSet.Close();
                }
                //JHB 20190121 merge from LAL-LO JARS 20180115 (S)
                if (dTotalCollected <= 0) //JARS 20180115 TO SEE TOTAL CHECK AMOUNT, AMOUNT USED, CURRENT BALANCE WHEN PAYING FOR MULTIPLE BUSINESSES
                {
                    sChkNo = txtChkNo.Text.ToString();
                    sBankCode = txtBankCode.Text.ToString();
                    //sQuery = "select sum(chk_amt) as chk_amt from chk_tbl where chk_no = '" + sChkNo + "'";
                    sQuery = "select sum(chk_amt) as chk_amt from chk_tbl where chk_no = '" + sChkNo + "' and bank_code = '" + sBankCode + "'"; //AFM 20200428 added bank code to avoid same check diff bank
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            dTotalAmtUsed = pSet.GetDouble("chk_amt");
                        }
                    pSet.Close();

                    //sQuery = "select * from multi_check_pay where chk_no = '" + sChkNo + "'";
                    sQuery = "select * from multi_check_pay where chk_no = '" + sChkNo + "' and bank_code = '" + sBankCode + "'"; //AFM 20200601 added bank code for same check diff bank
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            dTotalChkAmt = pSet.GetDouble("chk_amt");
                            if (txtChkAmount.Text != "")
                            {
                                dRemaining = dTotalChkAmt - Convert.ToDouble(txtChkAmount.Text) - dTotalAmtUsed;
                            }
                        }
                    pSet.Close();

                    dTotalCollected = dTotalAmtUsed;
                }
                //JHB 20190121 merge from LAL-LO JARS 20180115 (E)

                txtChkInfoBalance.Text = dRemaining.ToString("#,##0.00");
                txtChkInfoTotAmountColl.Text = dTotalCollected.ToString("#,##0.00");
                txtChkInfoAmount.Text = dTotalChkAmt.ToString("#,##0.00");
            }
        }

        private void RefreshCheckType()
        {
            if (rdoChk1.Checked == true)
                sCheckType = "MC";
            else if (rdoChk2.Checked == true)
                sCheckType = "CC";
            else if (rdoChk3.Checked == true)
                sCheckType = "PC";
        }

        private void rdoChk1_Click(object sender, EventArgs e)
        {
            sCheckType = "CC";
            //sCheckType = "MC";
        }

        private void rdoChk2_Click(object sender, EventArgs e)
        {
            sCheckType = "PC";
            //sCheckType = "CC";
        }

        private void rdoChk3_Click(object sender, EventArgs e)
        {
            sCheckType = "MC";
            //sCheckType = "PC";
        }

        private void dtpChkType_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpChkType.Value.Date > cdtToday.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpChkType.Value = AppSettingsManager.GetSystemDate();
            }
        }

        private void dtpChkDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpChkDate.Value.Date > cdtToday.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpChkDate.Value = AppSettingsManager.GetSystemDate();
            }
        }

        private void chkGuarantor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGuarantor.Checked == true)
            {
                chkGuarantor.Enabled = true;
                bMultiPayFound = true;
                txtLN.ReadOnly = true;
                txtFN.ReadOnly = true;
                txtMI.ReadOnly = true;
                btnAddOwner.Enabled = true;
                btnAcctNo.Enabled = true;
                chkRef.Enabled = true;
            }
            else
            {
                chkRef.Checked = false;
                txtChkCashAmount.ReadOnly = false;
                txtChkAmount.ReadOnly = false;

                dtpChkDate.Enabled = true;
                rdoChk1.Enabled = true;
                rdoChk2.Enabled = true;
                rdoChk3.Enabled = true;

                OnChkChknoref();

                chkGuarantor.Checked = false;
                bMultiPayFound = false;
                bRefCheck = false;
                txtLN.ReadOnly = false;
                txtFN.ReadOnly = false;
                txtMI.ReadOnly = false;
                btnAddOwner.Enabled = false;
                btnAcctNo.Enabled = false;
                chkRef.Enabled = false;
            }
        }

        private void chkRef_CheckedChanged(object sender, EventArgs e)
        {
            OnChkChknoref();
        }

        private void OnChkChknoref()
        {
            txtBankCode.Text = "";
            txtBankBranch.Text = "";
            txtBankName.Text = "";

            //REM MCR 20150123 (s)
            //txtFN.Text = "";
            //txtLN.Text = "";
            //txtMI.Text = "";
            //txtAcctNo.Text = "";
            //REM MCR 20150123 (e)
            txtChkNo.Text = "";
            txtChkAmount.Text = "";
            txtChkCashAmount.Text = "";

            if (chkRef.Checked == true)
            {
                btnChkNo.Enabled = true;
                btnAcctNo.Enabled = false;
                btnAddOwner.Enabled = false;
                btnFind.Enabled = false;
                txtAcctNo.ReadOnly = true;
                txtLN.ReadOnly = true;
                txtFN.ReadOnly = true;
                txtMI.ReadOnly = true;
                txtChkAmount.ReadOnly = true;
                txtChkCashAmount.ReadOnly = true;
                rdoChk1.Enabled = false;
                rdoChk2.Enabled = false;
                rdoChk3.Enabled = false;
                dtpChkDate.Enabled = false;
                bRefCheck = true;
            }
            else
            {
                btnChkNo.Enabled = false;
                btnAcctNo.Enabled = true;
                btnAddOwner.Enabled = true;
                btnFind.Enabled = true;
                txtAcctNo.ReadOnly = false;
                txtLN.ReadOnly = false;
                txtFN.ReadOnly = false;
                txtMI.ReadOnly = false;
                txtChkAmount.ReadOnly = false;
                txtChkCashAmount.ReadOnly = false;
                rdoChk1.Enabled = true;
                rdoChk2.Enabled = true;
                rdoChk3.Enabled = true;
                dtpChkDate.Enabled = true;
                bRefCheck = false;
            }
        }

        private void dgView_SelectionChanged(object sender, EventArgs e)
        {
            if (dgView.RowCount > 0)
                UpdateControlValues();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            
            frmBankSearch frmBank = new frmBankSearch();
            frmBank.ShowDialog();
            txtBankCode.Text = frmBank.BankCode.Trim();
            txtBankName.Text = frmBank.BankName.Trim();
            txtBankBranch.Text = frmBank.BankBranch.Trim();

            if (txtLN.Text == "")
            {
                txtLN.Text = m_sAcctLN;
                txtFN.Text = m_sAcctFN;
                txtMI.Text = m_sAcctMI;
            }
        }

        private void btnAcctNo_Click(object sender, EventArgs e)
        {
            frmSearchOwner frmSearchOwner = new frmSearchOwner();
            frmSearchOwner.ShowDialog();

            if (frmSearchOwner.m_strOwnCode != String.Empty)
            {
                sTempOwnCode = frmSearchOwner.m_strOwnCode; // CTS 12212004 temporary catching the own_code
                txtLN.Text = frmSearchOwner.m_sOwnLn;
                txtFN.Text = frmSearchOwner.m_sOwnFn;
                txtMI.Text = frmSearchOwner.m_sOwnMi;
            }
        }

        private void btnAddOwner_Click(object sender, EventArgs e)
        {
            frmPayorInfo frmPayorInfo = new frmPayorInfo();
            frmPayorInfo.ShowDialog();
            if (frmPayorInfo.m_sOwnCode != String.Empty)
            {
                sTempOwnCode = frmPayorInfo.m_sOwnCode;
                txtLN.Text = frmPayorInfo.m_sLN;
                txtFN.Text = frmPayorInfo.m_sFN;
                txtMI.Text = frmPayorInfo.m_sMi;
            }
        }

        private void txtChkNo_Leave(object sender, EventArgs e)
        {
            String sString;
            OracleResultSet pSet = new OracleResultSet();

            if (chkRef.Checked == false && txtChkNo.Text.Trim() != "")
            {
                if (txtChkNo.Text.Trim() != sTempChkNo)
                {
                    sString = "select * from chk_tbl where chk_no = '" + txtChkNo.Text.Trim() + "' and bank_code = '"+ txtBankCode.Text.Trim() +"'"; //AFM 20200117 added bank code in validation to determine same chk no
                    pSet.Query = sString;
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            MessageBox.Show("Check number already been used! \nYou're not using multiple business payment", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            txtChkNo.Text = "";
                            txtChkNo.Focus();
                            return;
                        }
                    }
                    pSet.Close();   // RMC 20140909 Migration QA

                    sString = "select * from chk_tbl_temp where chk_no = '" + txtChkNo.Text.Trim() + "'";
                    pSet.Query = sString;
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            MessageBox.Show("Check number already been used! \nYou're not using multiple business payment", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            txtChkNo.Text = "";
                            txtChkNo.Focus();
                            return;
                        }
                    }
                    pSet.Close();   // RMC 20140909 Migration QA
                }
            }
        }

        private bool Doppelganger() //AFM 20200513 lists all chk with same no but different banks
        {
            List<string> listBankCodes = new List<string>();
            int i = 0;
            OracleResultSet result = new OracleResultSet();
            frmBankSearch frmbanksearch = new frmBankSearch();
            result.Query = "select distinct(bank_code) as bank_code from multi_check_pay where chk_no = '" + txtChkNo.Text + "'";
            if(result.Execute())
                while(result.Read())
                {
                    listBankCodes.Add(result.GetString("bank_code"));
                    i++;
                }
            result.Close();
            if(i == 1)
                return false;
            else
            {
                MessageBox.Show("Found " + i + " cheques with same cheque no. from different banks \nPlease select one.", "Same Cheque Found!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                frmbanksearch.SearchMode = "DUPLICATES";
                frmbanksearch.BankCodesList = listBankCodes.ToArray();
                frmbanksearch.ShowDialog();
                m_sBankCode = frmbanksearch.BankCode;
                return true;
            }

        }
        private void btnChkNo_Click(object sender, EventArgs e)
        {
            String sQuery = "", sString = "", sOwnCode = "";
            double dChkAmt = 0;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            m_sFlagAddlCheck = "N";
            if (txtChkNo.Text.Trim() == "")
            {
                MessageBox.Show("Check number required!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if(chkRef.Checked == true) //AFM 20200513 set query if has duplicates
            {
                if(Doppelganger())
                    sQuery = "select * from multi_check_pay where chk_no = '" + txtChkNo.Text + "' and bank_code = '" + m_sBankCode + "'";
                else
                    sQuery = "select * from multi_check_pay where chk_no = '" + txtChkNo.Text + "'";


            }
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sCheckType = pSet.GetString("chk_type").Trim();
                    dtpChkDate.Value = pSet.GetDateTime("chk_date");
                    txtAcctNo.Text = pSet.GetString("acct_no").Trim();
                    txtBankCode.Text = pSet.GetString("bank_code").Trim();
                    sOwnCode = pSet.GetString("own_code").Trim();
                    //sTempOwnCode = sOwnCode;
                    sTempOwnCode = txtAcctNo.Text;  // RMC 20170210 corrected error in additional payment when using multiple check transaction
                    //}   // RMC 20140909 Migration QA, put rem
                    //pSet.Close(); // RMC 20140909 Migration QA, put rem

                    if (sCheckType == "MC")
                        rdoChk1.Checked = true;
                    if (sCheckType == "CC")
                        rdoChk2.Checked = true;
                    if (sCheckType == "PC")
                        rdoChk3.Checked = true;

                    pSet.Close();   // RMC 20140909 Migration QA
                    sQuery = "select * from own_names where own_code in (select own_code from businesses where bin = '" + sOwnCode + "')";
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            txtLN.Text = pSet.GetString("own_ln").Trim();
                            txtFN.Text = pSet.GetString("own_fn").Trim();
                            txtMI.Text = pSet.GetString("own_mi").Trim();
                        }
                        else
                        {
                            pSet.Close();
                            sQuery = "select * from own_names where own_code in (select own_code from business_que where bin = '" + sOwnCode + "')";
                            pSet.Query = sQuery;
                            if (pSet.Execute())
                                if (pSet.Read())
                                {
                                    txtLN.Text = pSet.GetString("own_ln").Trim();
                                    txtFN.Text = pSet.GetString("own_fn").Trim();
                                    txtMI.Text = pSet.GetString("own_mi").Trim();
                                }
                        }
                    pSet.Close();

                    if (txtBankCode.Text.Trim() != "")
                    {
                        sQuery = "select * from bank_table where bank_code = '" + txtBankCode.Text.Trim() + "'";
                        pSet.Query = sQuery;
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                txtBankName.Text = pSet.GetString("bank_nm").Trim();
                                txtBankBranch.Text = pSet.GetString("bank_branch").Trim();
                            }
                        pSet.Close();
                    }

                    sQuery = "select sum(chk_amt) as chk_amt from chk_tbl where chk_no = '" + txtChkNo.Text.Trim() + "' and bank_code = '" + txtBankCode.Text.Trim() + "'";	// JGR 02022006 GET CHECK BALANCE FROM CHECK_TBL AGAINST MULTI_CHECK_PAY //AFM 20200428 added bank code
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            double dChkAmt2 = 0;
                            dChkAmt2 = pSet.GetDouble("chk_amt");

                            double dMultiChkAmt = 0;
                            sQuery = "select * from multi_check_pay where chk_no = '" + txtChkNo.Text.Trim() + "' and bank_code = '" + txtBankCode.Text.Trim() + "'"; //AFM 20200603 added bank code
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                    dMultiChkAmt = pSet1.GetDouble("chk_amt");
                            pSet1.Close();

                            dChkAmt2 = dMultiChkAmt - dChkAmt2;

                            bool blnRead = false;
                            double dExcessAmt = 0;
                            sQuery = "select * from dbcr_memo where chk_no = '" + txtChkNo.Text.Trim() + "' and multi_pay = 'N' and served = 'N'"; // JGR 02142006 IF SERVED IS N AND CHK_NO IS EITHER IN MULTI_CHECK_PAY OR NOT  and chk_no not in (select chk_no from multi_check_pay)";	// JJP 02102006 QA NEW MULTIPLE //JARS 20171010
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                {
                                    dExcessAmt = pSet1.GetDouble("balance");
                                    blnRead = true; //AFM 20200428
                                }
                            pSet1.Close();

                            dChkAmt2 = dChkAmt2 - dExcessAmt;

                            if (dChkAmt2 <= 0 && blnRead == true) //AFM 20200428 added condition if query above DID NOT return null to bypass multi-payment
                            {
                                MessageBox.Show("This Check has 0 balance", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                pSet.Close();
                                return;
                            }

                            double dChkAmtList = 0;
                            for (int i = 0; i < dgView.RowCount; i++)
                            {
                                dChkAmtList += Convert.ToDouble(dgView.Rows[i].Cells[1].Value);
                            }

                            dChkAmt = dChkAmt2 + dChkAmtList;
                            double dTotalTaxDue = Convert.ToDouble(txtTotTaxDue.Text);
                            String sChkAmt = dChkAmt.ToString();

                            // AST 20160122 fixed minor bug/s (s)
                            string strTmp = dChkAmt.ToString();
                            double.TryParse(strTmp, out dChkAmt);
                            strTmp = dTotalTaxDue.ToString();
                            //double.TryParse(strTmp, out dChkAmt);
                            double.TryParse(strTmp, out dTotalTaxDue);  // RMC 20170210 corrected error in additional payment when using multiple check transaction
                            // AST 20160122 fixed minor bug/s (e)

                            if (dChkAmt >= dTotalTaxDue)
                            {
                                txtTotTaxDue.Text = Convert.ToDouble(dTotalTaxDue - dChkAmtList).ToString();
                                txtChkAmount.Text = txtTotTaxDue.Text;

                            }
                            else
                            {
                                String sInsuBal = Convert.ToDouble(dTotalTaxDue - dChkAmt).ToString();
                                if (MessageBox.Show("Insufficient fund to settle dues!\nWould you like to add another payment?\n[AMOUNT: " + sInsuBal + "]", "Check Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    frmAdditionalPayment frmAdditionalPayment = new frmAdditionalPayment();
                                    frmAdditionalPayment.ShowDialog();
                                    if (frmAdditionalPayment.m_sPaymentType != "")
                                    {
                                        bInsufficient = true;
                                        String sPayType = frmAdditionalPayment.m_sPaymentType;
                                        if (sPayType == "CS")
                                        {
                                            txtChkAmount.Text = dChkAmt2.ToString();
                                            m_sPaymentType = "CC";
                                            btnAdd.PerformClick();
                                            btnExit.PerformClick();
                                        }
                                        else
                                        {
                                            txtChkAmount.Text = dChkAmt2.ToString();
                                            btnAdd.PerformClick();
                                            DeacControls();
                                            bMultiPayFound = false;
                                            bRefCheck = false;
                                            chkRef.Checked = false;
                                            btnAdd.Text = "&Add";
                                            btnExit.Text = "Ex&it";
                                            btnAdd.Enabled = true;
                                            dgView.Enabled = true;
                                            btnEdit.Enabled = true;
                                            btnDelete.Enabled = true;

                                            if (bMultiPayFound == true)
                                                txtDBCRAmout.Text = txtChkInfoBalance.Text;

                                            if (bMultiPayFound == true && bRefCheck == true)
                                            {
                                                txtDBCRAmout.Text = "0.00";
                                                chkRef.Checked = true;
                                            }

                                            if (sTempOwnCode != "")
                                                m_sOwnCode = sTempOwnCode;
                                            m_sFlagAddlCheck = "Y";
                                        }
                                    }
                                    else
                                        txtChkNo.Focus();
                                }
                                else
                                    txtChkNo.Focus();
                            }
                        }
                    // pSet.Close(); // RMC 20140909 Migration QA, put rem
                }   // RMC 20140909 Migration QA
            }   // RMC 20140909 Migration QA
        }

        private void txtChkNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;*/
            // RMC 20140909 Migration QA, put rem
        }

        private void chkAttached_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAttached.Checked)
            {
                MessageBox.Show("Are you sure this is your last transaction?\nThe excess amount of this check will be credited to the current record.", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                m_sFlagAddlCheck = "Y"; //AFM 20200605
                chkAttached.Checked = true;
            }
            else
            {
                chkAttached.Checked = false;
                m_sFlagAddlCheck = "N"; //AFM 20200605
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            String sQuery;
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            if (btnAdd.Text.Trim() == "&Add")
            {
                Initialize();

                rdoChk1.Checked = false;
                rdoChk2.Checked = false;
                rdoChk3.Checked = false;

                txtAcctNo.Text = m_sOwnCode;
                txtLN.Text = m_sAcctLN;
                txtFN.Text = m_sAcctFN;
                txtMI.Text = m_sAcctMI;

                btnAcctNo.Enabled = true;
                btnAddOwner.Enabled = true;

                btnAdd.Text = "&Save";
                btnExit.Text = "&Cancel";

                AcControls();
                dgView.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;

                if (bMultiPayFound == true)
                    chkGuarantor.Enabled = false;
            }
            else //Save
            {
                RefreshCheckType();

                if (txtAcctNo.Text.Trim() == "")
                {
                    MessageBox.Show("You must enter an account no!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtAcctNo.Focus();
                    return;
                }
                if (txtLN.Text.Trim() == "")
                {
                    MessageBox.Show("You must enter an account name!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtLN.Focus();
                    return;
                }
                if (txtChkNo.Text.Trim() == "")
                {
                    MessageBox.Show("You must enter a check number!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtChkNo.Focus();
                    return;
                }
                if (txtChkAmount.Text.Trim() == "")
                {
                    MessageBox.Show("You must enter check amount!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtChkAmount.Focus();
                    return;
                }

                if (txtBankCode.Text.Trim() == "")
                {
                    MessageBox.Show("Bank code required!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    btnFind.Focus();
                    return;
                }

                if (sCheckType.Trim() == "")
                {
                    MessageBox.Show("Choose check type first!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (chkGuarantor.Checked == false && m_sPaymentType.Contains("CC"))
                {
                    double dAmount = 0;
                    dAmount = Convert.ToDouble(txtChkAmount.Text) + Convert.ToDouble(txtTotChkAmount.Text);

                    if (dAmount >= Convert.ToDouble(txtTotTaxDue.Text))
                    {
                        MessageBox.Show("Total check amount can't be higher or equal to amount due! \nYou are using cash/check payment!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtChkAmount.Focus();
                        return;
                    }
                }

                //if (chkGuarantor.Checked == true && chkRef.Checked == false &&
                //   ((Convert.ToDouble(txtChkAmount.Text) + Convert.ToDouble(txtTotChkAmount.Text)) < Convert.ToDouble(txtTotTaxDue.Text))) // ALJ 12292005
                if (chkGuarantor.Checked == true && chkRef.Checked == false &&
                   ((Convert.ToDouble(txtChkAmount.Text) + Convert.ToDouble(txtTotChkAmount.Text)) <= Convert.ToDouble(txtTotTaxDue.Text))) // AFM 20200520 less than or equal
                {
                    MessageBox.Show("Check amount must be higher than amount due! \nYou are using multi-business payment!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtChkAmount.Focus();
                    return;
                }

                if (Convert.ToDouble(txtChkAmount.Text) <= 0.00)
                {
                    MessageBox.Show("Check amount required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtChkAmount.Focus();
                    return;
                }

                if (chkGuarantor.Checked == true && chkRef.Checked == false)
                {
                    sQuery = "insert into multi_check_pay values";
                    //sQuery += "('" + StringUtilities.HandleApostrophe(m_sBin) + "',";
                    sQuery += "('" + StringUtilities.HandleApostrophe(m_sOwnCode) + "',"; //JARS 20170728
                    sQuery += "'" + StringUtilities.HandleApostrophe(txtChkNo.Text.Trim()) + "',";
                    sQuery += "to_date('" + StringUtilities.HandleApostrophe(dtpChkDate.Value.ToString("MM/dd/yyyy")) + "','MM/dd/yyyy'),";
                    sQuery += "'" + sCheckType + "',";
                    sQuery += txtChkAmount.Text + ", ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(txtBankCode.Text.Trim()) + "',";
                    sQuery += "'" + StringUtilities.HandleApostrophe(txtAcctNo.Text.Trim()) + "',";
                    sQuery += "sysdate,";
                    sQuery += "'" + AppSettingsManager.TellerUser.UserCode + "',";
                    sQuery += "'N')";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    bMultiPayFound = true;
                    txtChkAmount.Text = Convert.ToDouble(Convert.ToDouble(txtTotTaxDue.Text) - Convert.ToDouble(txtTotChkAmount.Text)).ToString();
                }

                sQuery = "insert into chk_tbl_temp(chk_no,chk_date,chk_type,chk_amt,cash_amt,bank_code,acct_no,acct_ln,acct_fn,acct_mi,or_no,or_date,teller,dt_accepted) values ";
                sQuery += "('" + StringUtilities.HandleApostrophe(txtChkNo.Text.Trim()) + "',";
                sQuery += "to_date('" + dtpChkDate.Value.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),";
                sQuery += "'" + sCheckType + "',";
                sQuery += "'" + txtChkAmount.Text + "',";
                sQuery += "'" + txtChkCashAmount.Text + "',";
                sQuery += "'" + StringUtilities.HandleApostrophe(txtBankCode.Text) + "',";
                sQuery += "'" + StringUtilities.HandleApostrophe(txtAcctNo.Text) + "',";
                sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(txtLN.Text.Trim(), 50)) + "',";
                sQuery += "'" + StringUtilities.HandleApostrophe(StringUtilities.Left(txtFN.Text.Trim(), 20)) + "',";
                sQuery += "'" + StringUtilities.HandleApostrophe(txtMI.Text.Trim()) + "',";
                sQuery += "'" + StringUtilities.HandleApostrophe(m_sORNo) + "',";
                sQuery += "to_date('" + StringUtilities.HandleApostrophe(m_sORDate) + "','MM/dd/yyyy'),";
                sQuery += "'" + StringUtilities.HandleApostrophe(AppSettingsManager.TellerUser.UserCode) + "',";
                sQuery += "to_date('" + StringUtilities.HandleApostrophe(dtpChkDate.Value.ToString("MM/dd/yyyy")) + "','MM/dd/yyyy'))";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();

                String sObject;
                sObject = StringUtilities.HandleApostrophe(txtLN.Text.Trim()) + ", " + StringUtilities.HandleApostrophe(txtFN.Text.Trim()) + " " + StringUtilities.HandleApostrophe(txtMI.Text.Trim()) + ".";
                AuditTrail.InsertTrail("CSCI", "multiple table", sObject);

                UpdateList("");
                ComputeTotalChkAmt();
                GetRemainChkBalance();

                if (chkGuarantor.Checked == true && chkRef.Checked == true && Convert.ToDouble(txtChkInfoBalance.Text) > 0.00)
                {
                    chkAttached.Enabled = true;
                    this.chkAttached.ForeColor = System.Drawing.Color.Red;  // RMC 20150505 QA reports
                }
                else
                {
                    chkAttached.Enabled = false;
                    this.chkAttached.ForeColor = System.Drawing.Color.Black;    // RMC 20150505 QA reports
                }

                Initialize();

                btnAdd.Text = "&Add";
                btnExit.Text = "E&xit";

                if (bMultiPayFound == true)
                {
                    btnAdd.Enabled = false;
                    if (bRefCheck == true)
                    {
                        btnEdit.Enabled = false;
                        chkRef.Checked = true;
                    }
                }
                else
                {
                    txtChkInfoBalance.Text = "0.00";
                    txtChkInfoTotAmountColl.Text = "0.00";
                    txtChkInfoAmount.Text = "0.00";
                }

                DeacControls();
                dgView.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                //FocusCurrentValue(); // MCR 20140904 Select Current Value
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "&Edit")
            {
                if (txtAcctNo.Text.Trim() != "")
                {
                    AcControls();
                    sTempChkNo = txtChkNo.Text.Trim();
                    btnEdit.Text = "&Update";
                    btnExit.Text = "&Cancel";
                    dgView.Enabled = false;
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;

                    if (dgView.RowCount >= 1 && bMultiPayFound == true)
                        chkGuarantor.Enabled = false;

                    if (chkGuarantor.Checked == true)
                        txtChkAmount.Text = Convert.ToDouble(txtChkInfoAmount.Text).ToString("#00.00");
                }
                else
                {
                    if (dgView.RowCount > 0)
                        MessageBox.Show("Select an item in the list first!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    else
                        MessageBox.Show("No items in the list!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    btnEdit.Focus();
                }
            }
            else //Update
            {
                RefreshCheckType();
                String sQuery;
                OracleResultSet pSet = new OracleResultSet();
                OracleResultSet pSet1 = new OracleResultSet();
                OracleResultSet pSet2 = new OracleResultSet();
                String sOldChkNo;
                DateTime cdtToday = new DateTime();
                cdtToday = AppSettingsManager.GetSystemDate();

                //Validations (s)
                if (txtAcctNo.Text.Trim() == "")
                {
                    MessageBox.Show("You must enter an account no", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtAcctNo.Focus();
                    return;
                }
                if (txtLN.Text.Trim() == "")
                {
                    MessageBox.Show("You must enter an account name", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtLN.Focus();
                    return;
                }
                if (txtChkNo.Text.Trim() == "")
                {
                    MessageBox.Show("You must enter a check no", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtChkNo.Focus();
                    return;
                }
                if (txtChkAmount.Text.Trim() == "")
                {
                    MessageBox.Show("You must enter check amount", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtChkAmount.Focus();
                    return;
                }

                if (txtBankCode.Text.Trim() == "")
                {
                    MessageBox.Show("Bank code required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    btnFind.Focus();
                    return;
                }

                if (sCheckType.Trim() == "")
                {
                    MessageBox.Show("Choose check type first!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (chkGuarantor.Checked == false && m_sPaymentType.Contains("CC"))
                {
                    double dAmount = 0;
                    dAmount = Convert.ToDouble(txtChkAmount.Text) + Convert.ToDouble(txtTotChkAmount.Text);

                    if (dAmount >= Convert.ToDouble(txtTotTaxDue.Text))
                    {
                        MessageBox.Show("Total check amount can't be higher or equal to amount due! \nYou are using cash/check payment!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtChkAmount.Focus();
                        return;
                    }
                }

                if (chkGuarantor.Checked == true && chkRef.Checked == false && (Convert.ToDouble(txtChkAmount.Text) <= Convert.ToDouble(txtTotTaxDue.Text)))
                {
                    MessageBox.Show("Check amount must be higher than amount due! \nYou are using multi-business payment!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtChkAmount.Focus();
                    return;
                }

                if (Convert.ToDouble(txtChkAmount.Text) <= 0.00)
                {
                    MessageBox.Show("Check amount required!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtChkAmount.Focus();
                    return;
                }

                sQuery = "delete from multi_check_pay where chk_no = '" + sTempChkNo + "' and used_sw = 'N'";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();

                if (chkGuarantor.Checked == true && chkRef.Checked == false)
                {
                    sQuery = "insert into multi_check_pay values";
                    //sQuery += "('" + StringUtilities.HandleApostrophe(m_sBin) + "',";
                    sQuery += "('" + StringUtilities.HandleApostrophe(m_sOwnCode) + "',"; //JARS 20170728
                    sQuery += "'" + StringUtilities.HandleApostrophe(txtChkNo.Text) + "',";
                    sQuery += "to_date('" + StringUtilities.HandleApostrophe(dtpChkDate.Value.ToString("MM/dd/yyyy")) + "','MM/dd/yyyy'),";
                    sQuery += "'" + sCheckType + "',";
                    sQuery += txtChkAmount.Text + ", ";
                    sQuery += "'" + StringUtilities.HandleApostrophe(txtBankCode.Text) + "',";
                    sQuery += "'" + StringUtilities.HandleApostrophe(txtAcctNo.Text) + "',";
                    sQuery += "sysdate,";
                    sQuery += "'" + StringUtilities.HandleApostrophe(AppSettingsManager.TellerUser.UserCode) + "', ";
                    sQuery += "'N')";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    txtChkAmount.Text = txtTotTaxDue.Text;
                }

                sOldChkNo = txtChkNo.Text.Trim();
                sQuery = string.Format("delete from chk_tbl_temp where chk_no = '{0}'", sTempChkNo); // CTS 09152003
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();

                sQuery = "insert into chk_tbl_temp(chk_no,chk_date,chk_type,chk_amt,cash_amt,bank_code,acct_no,acct_ln,acct_fn,acct_mi,or_no,or_date,teller,dt_accepted) values ";
                sQuery += "('" + StringUtilities.HandleApostrophe(txtChkNo.Text.Trim()) + "', ";
                sQuery += "to_date('" + StringUtilities.HandleApostrophe(dtpChkDate.Value.ToString("MM/dd/yyyy")) + "','MM/dd/yyyy'),";
                sQuery += "'" + sCheckType + "', ";
                sQuery += Convert.ToDouble(txtChkAmount.Text).ToString("#00.00") + ", ";
                sQuery += Convert.ToDouble(txtChkCashAmount.Text).ToString("#00.00") + ", ";
                sQuery += " '" + StringUtilities.HandleApostrophe(txtBankCode.Text.Trim()) + "', ";
                sQuery += " '" + StringUtilities.HandleApostrophe(txtAcctNo.Text) + "', ";
                sQuery += " '" + StringUtilities.HandleApostrophe(StringUtilities.Left(txtLN.Text.Trim(), 50)) + "', ";
                sQuery += " '" + StringUtilities.HandleApostrophe(StringUtilities.Left(txtFN.Text.Trim(), 50)) + "', ";
                sQuery += " '" + StringUtilities.HandleApostrophe(txtMI.Text.Trim()) + "', ";
                sQuery += " '" + StringUtilities.HandleApostrophe(m_sORNo) + "', ";
                sQuery += "to_date('" + StringUtilities.HandleApostrophe(m_sORDate) + "','MM/dd/yyyy'),";
                sQuery += " '" + StringUtilities.HandleApostrophe(AppSettingsManager.TellerUser.UserCode) + "', ";
                sQuery += "to_date('" + StringUtilities.HandleApostrophe(dtpChkType.Value.ToString("MM/dd/yyyy")) + "','MM/dd/yyyy')) ";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();

                String sObject;
                sObject = StringUtilities.HandleApostrophe(txtLN.Text.Trim()) + ", " + StringUtilities.HandleApostrophe(txtFN.Text.Trim()) + " " + StringUtilities.HandleApostrophe(txtMI.Text.Trim()) + ".";
                AuditTrail.InsertTrail("CECI", "multiple table", sObject);

                UpdateList("");
                ComputeTotalChkAmt();
                GetRemainChkBalance();
                Initialize();

                btnEdit.Text = "&Edit";
                btnExit.Text = "E&xit";
                if (bMultiPayFound == true)
                {
                    btnAdd.Enabled = false;
                    if (bRefCheck == true)
                    {
                        btnEdit.Enabled = false;
                        chkRef.Checked = true;
                    }
                }
                else
                {
                    txtChkInfoBalance.Text = "0.00";
                    txtChkInfoTotAmountColl.Text = "0.00";
                    txtChkInfoAmount.Text = "0.00";
                }

                DeacControls();
                dgView.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                //FocusCurrentValue(); // MCR 20140904 Select Current Value
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            String sQuery;
            OracleResultSet pSet = new OracleResultSet();

            if (txtChkNo.Text.Trim() == "")
            {
                if (dgView.RowCount > 0)
                    MessageBox.Show("Select an item in the list first!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else
                    MessageBox.Show("No items in the list!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btnEdit.Focus();
            }
            else
            {
                if (MessageBox.Show("Delete this check info?", "Payment Check", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sQuery = string.Format("delete from chk_tbl_temp where chk_no = '{0}'", txtChkNo.Text.Trim());
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    sQuery = "select * from multi_check_pay where chk_no = '" + txtChkNo.Text.Trim() + "' and used_sw = 'N'";
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sQuery = "delete from multi_check_pay where chk_no = '" + txtChkNo.Text.Trim() + "' and used_sw = 'N'";
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();
                        }
                    pSet.Close();

                    AuditTrail.InsertTrail("CDCI", "multiple table", txtChkNo.Text.Trim());
                    UpdateList("");
                    Initialize();
                    ComputeTotalChkAmt();
                    if (dgView.RowCount == 0)
                    {
                        btnAdd.Enabled = true;
                        btnEdit.Enabled = true;
                        bMultiPayFound = false;
                        txtChkInfoBalance.Text = "0.00";
                        txtChkInfoTotAmountColl.Text = "0.00";
                        txtChkInfoAmount.Text = "0.00";
                        sTempOwnCode = m_sOwnCode;
                    }

                }
            }
        }

       

        private void btnExit_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (btnExit.Text == "&Cancel")
            {
                dgView.ClearSelection();
                if (btnAdd.Text == "&Save")
                {
                    DeacControls();
                    btnAdd.Text = "&Add";
                    btnExit.Text = "Ex&it";

                    dgView.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }
                if (btnEdit.Text == "&Update")
                {
                    DeacControls();
                    btnEdit.Text = "&Edit";
                    btnExit.Text = "Ex&it";

                    dgView.Enabled = true;
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                }
                Initialize();

                if (bMultiPayFound == true)
                {
                    btnAdd.Enabled = false;
                    if (chkRef.Checked == true)
                        btnEdit.Enabled = false;
                }
            }
            else //Exit
            {
                String sText, sRightCashAmt, sCheckNo;
                int iCnt = 0;

                //if (m_sPaymentType == "CC")
                if (m_sPaymentType == "CC" || m_sPaymentType == "CCTC") // RMC 20151209 corrections in cash, cheque, tax credit payment combination
                    iCnt = 0;
                else
                    iCnt = 1; //iCnt = Convert.ToInt16(m_sPaymentType.Contains("CC"));

                if (iCnt == 0 && Convert.ToDouble(txtTotCashAmount.Text) == 0.00)
                {
                    if (Convert.ToDouble(txtDBCRAmout.Text) >= 0.00 && lblDBCR.Text.Trim() != "Debit Amount" && lblDBCR.Text.Trim() != "Debit/Credit Amount")
                    {
                        MessageBox.Show("Total check amount must be lower than total tax due! \nYou are using Cash/Check combination!", "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    lblDBCR.Text = "Debit/Credit amount";
                    iCnt = dgView.RowCount;

                    if (iCnt > 0)
                    {
                        sCheckNo = "";
                        for (int i = 0; i <= dgView.RowCount; i++)
                        {
                            sCheckNo = dgView.Rows[i].Cells[0].Value.ToString();
                            if (sCheckNo != "")
                                i = dgView.RowCount + 2;
                        }

                        sRightCashAmt = Convert.ToDouble(Convert.ToDouble(txtTotTaxDue.Text) - Convert.ToDouble(txtTotChkAmount.Text)).ToString();
                        txtTotCashAmount.Text = sRightCashAmt;

                        sText = "update chk_tbl_temp set cash_amt = " + sRightCashAmt + " where chk_no = '" + sCheckNo + "'";
                        pSet.Query = sText;
                        pSet.ExecuteNonQuery();

                        MessageBox.Show("Cash amount will be " + sRightCashAmt, "Payment Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtDBCRAmout.Text = "0.00";
                    }
                }

                if (lblDBCR.Text == "Debit Amount")
                {
                    sRightCashAmt = Convert.ToDouble(Convert.ToDouble(txtTotTaxDue.Text) - (Convert.ToDouble(txtTotChkAmount.Text) + Convert.ToDouble(txtTotCashAmount.Text))).ToString();
                    

                    String sMsg;
                    sMsg = "Total cash amount should  be " + sRightCashAmt;
                    MessageBox.Show(sMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return;
                    this.Close();
                }

                if (bMultiPayFound == true)
                    txtDBCRAmout.Text = txtChkInfoBalance.Text;

                if (bMultiPayFound == true && bRefCheck == true)
                {
                    txtDBCRAmout.Text = "0.00";
                    chkRef.Checked = true;
                }
                if (sTempOwnCode != "")
                    m_sOwnCode = sTempOwnCode;

                String sTest;
                sTest = m_sPaymentType;

                m_sTotalCashAmount = txtTotCashAmount.Text;
                m_sTotalChkAmount = txtTotChkAmount.Text;
                m_sTotalDBCR = txtDBCRAmout.Text;
                m_sTotalRemBalance = txtChkInfoBalance.Text;
                
                this.Close();
            }
        }

        private void txtChkAmount_Leave(object sender, EventArgs e)
        {
            // RMC 20140909 Migration QA
            double dAmt = 0;
            double.TryParse(txtChkAmount.Text.ToString(), out dAmt);

            if (dAmt == 0 && txtBankCode.Text.Trim() != "")
            {
                MessageBox.Show("Invalid amount", "Check", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtChkAmount.Focus();
                return;
            }
        }

        /// <summary>
        /// AST 20150427 Added This Event to prevent closing of 
        /// payment if we don't have bank yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBankMaintenance_Click(object sender, EventArgs e)
        {
            using (NewBankModule.frmBank FormBank = new NewBankModule.frmBank())
            {
                FormBank.ShowDialog();   
            }
        }

        private void txtChkNo_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void txtChkNo_TextChanged(object sender, EventArgs e)
        { 
            //AFM 20200117 validation of bank
            if (txtBankCode.Text.Trim() == "")
            {
                if (txtChkNo.Text.Trim() != "")
                {
                    if(chkRef.Checked != true)
                    {
                    MessageBox.Show("Select Bank first!");
                    txtChkNo.Text = "";
                    return;
                    }
                }
            }
        }
    }
}