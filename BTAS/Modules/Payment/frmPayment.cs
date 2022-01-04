using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.BPLSApp;
using ComponentFactory.Krypton.Toolkit;
using CashTender;
using Amellar.Common.Reports;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using System.Collections;
using Amellar.Common.SearchBusiness;
using Amellar.Modules.PaymentHistory;
using Amellar.Common.SOA;
using Amellar.Common.AppSettings;
using Amellar.Modules.LiquidationReports;
using Amellar.Common.TransactionLog;

namespace Amellar.Modules.Payment
{
    public partial class frmPayment : Form
    {
        string m_sRevYear = string.Empty;
        string m_sIsQtrly = string.Empty;
        bool m_bRetVoidSurchPen = false;
        public bool bCompromise = false;
        bool m_bExistTaxPU = false;
        public bool bGetTransTrigger = false;
        public bool bInitialDP = false;
        public bool bRefCheckNo = false;
        public bool bTaxDueFound = false;
        bool m_bAttachedExcessAmount = false;
        bool m_bExistTaxDue = false;
        string m_sNextQtrToPay = string.Empty;
        string m_sMultiPay = string.Empty;
        string m_sPaymentType = string.Empty;
        public string m_sPaymentMode = string.Empty;
        public string m_sBIN = string.Empty; //MCR 20140227 MOD public
        public string m_sPaymentTerm = string.Empty;
        bool m_bIsCompromise = false;
        bool m_bIsDisCompromise = false;
        double m_dTotalDue = 0, m_dTotalSurcharge = 0, m_dTotalPenalty = 0, m_dTotalTotalDue = 0;
        double m_dTotalDue1 = 0, m_dTotalSurcharge1 = 0, m_dTotalPenalty1 = 0, m_dTotalTotalDue1 = 0;
        string m_sStatus = string.Empty;
        double dDue = 0, dPen = 0, dSurch = 0, dTot = 0;

        string m_sProv, m_sMun, m_sDist, m_sZone, m_sBrgy, m_sStreet, m_sAddrNo, m_sCreditLeft = "0"; //MCR  20140724
        string m_sTimePosted = "", m_sPostedDate = ""; //MCR 20140724

        string p_bin = string.Empty, p_tax_year = string.Empty, p_term = string.Empty, p_qtr = string.Empty, p_fees_desc = string.Empty;
        string p_fees_due = string.Empty, p_fees_surch = string.Empty, p_fees_pen = string.Empty, p_fees_totaldue = string.Empty;
        string p_fees_code = string.Empty, p_due_state = string.Empty, p_qtr_to_pay = string.Empty;
        string m_sFullPartial = string.Empty;
        string m_sOwnCode = string.Empty;
        bool m_bIsRenPUT = false;
        string m_sPrevOwnCode = string.Empty;
        string m_sCheckOwnerCode = string.Empty;
        string m_sOldBnsName = string.Empty;
        public string m_sDtOperated = string.Empty; //MCR 20140227 MOD public
        string m_sPrevBnsLoc = string.Empty;
        string m_sCompPaySw = string.Empty;
        string m_sQtr1 = string.Empty;
        string m_sTerm = string.Empty;
        int m_iNextQtrToPay = 0;
        string m_strORNo = string.Empty;
        string m_sCurrentTaxYear = string.Empty;
        string m_sPreviousOR = string.Empty;
        //MCR 20140227 (s)
        public string m_sMonthlyCutoffDate = "";
        public string m_sInitialDueDate = "";
        public double m_dPenRate = 0;
        public double m_dSurchRate = 0;
        public DateTime mv_cdtORDate;
        public bool m_bPartial = false;
        //MCR 20140227 (e)
        string m_sTellerOR = string.Empty;
        int m_iMultiTaxYear = 0;

        //MCR 20140811 (s)
        string m_sDiscountedTotal = "0";
        string m_sCheckChange = "0";
        //MCR 20140811 (e)

        string m_sTaxCreditAmount = string.Empty;

        //MCR 20140721 (s)
        double m_dCreditLeft = 0;
        bool m_bDiscQtr = false, bGuarantor = false, m_blnIsFull = false;
        string m_sNewOwnCode = string.Empty, m_sPlaceOccupancy = string.Empty, m_sRefCheckNo = string.Empty;
        //MCR 20140721 (e)

        string m_sQtr = string.Empty;
        string m_sFlag = "", m_sToBeDebited = "0.00", m_sDebitCredit = "0.00", m_sCCChkAmt = "0.00", m_sCCCashAmt = "0.00";

        //MCR 20140722 (s)
        string m_sNewWithQtr = string.Empty, m_sAddlCompPen = string.Empty, m_sDateToPay = string.Empty;
        bool m_bEnableTaxDue = false, m_bEnableTaxPU = false;
        double m_dFeesAmt = 0, m_dFeesPen = 0, m_dFeesSurch = 0, m_dFeesDue = 0;
        int iAgreePayTerm = 0, m_iNoPaySeq = 0, m_iTermToPayComp = 0;
        //MCR 20140722 (e)

        bool m_bMaxOR = false; //MCR 20140826
        private int m_iORNo = 0;    // RMC 20151229 merged multiple OR use from Mati

        private DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
        // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (s)
        string m_sTrDTINo = string.Empty;
        DateTime m_dTrDTIDt = AppSettingsManager.GetCurrentDate();
        string m_sTrMemo = string.Empty;
        // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (e)
        double dDeclaredGross = 0;

        public frmPayment()
        {
            InitializeComponent();
        }

        private void GetLastTransaction(string sTeller)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from teller_cashtender where teller = '" + sTeller.Trim() + "' order by datetime_save desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    txtLastBin.Text = result.GetString("bin").Trim();
                    txtLastChange.Text = string.Format("{0:###0.00}", result.GetDouble("change_amt"));
                    txtLastChange.SelectAll();
                    txtLastChange.Copy();
                }
                else
                {
                    txtLastChange.Text = "0.00";
                    txtLastBin.Text = "NONE";
                    //txtTeller.Text = "UNKNOWN TELLER";
                }
            }
            result.Close();
            txtTeller.Text = sTeller.Trim();

        }

        private void frmPayment_Load(object sender, EventArgs e)
        {
            m_sCurrentTaxYear = dtpORDate.Value.Year.ToString();
            bin.txtLGUCode.Text = AppSettingsManager.GetConfigObject("10");
            bin.txtDistCode.Text = AppSettingsManager.GetConfigObject("11");
            GetLastTransaction(AppSettingsManager.objTeller.UserCode);
            m_sMonthlyCutoffDate = AppSettingsManager.GetConfigValue("14");
            CheckPaymentMode();
            bin.txtTaxYear.Focus();
            txtLastChange.SelectAll();
            txtLastChange.Copy();
            /*if (m_iSwRE == 1 || m_iSwRE == 2)
                chkAdj.Enabled = true;
            else*/
            chkAdj.Enabled = false;
            dtpORDate.Text = AppSettingsManager.GetSystemDate().ToShortDateString();    // RMC 20151229 merged multiple OR use from Mati
        }

        private void CheckPaymentMode()
        {
            if (m_sPaymentMode != "SOA")
            {
                txtORNo.Text = GetCurrentOR(AppSettingsManager.objTeller.UserCode);
                if (m_sPaymentMode == "ONL")
                {
                    dtpORDate.Enabled = false;
                    dgvTaxFees.ReadOnly = true; // RMC 20180125 enabled editing of dues in Offline payment
                }
                else
                {
                    dtpORDate.Enabled = true;
                    dgvTaxFees.ReadOnly = false;    // RMC 20180125 enabled editing of dues in Offline payment
                }
            }
            else
                txtORNo.Text = string.Empty;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sString = "", sChkNo = "";

            if (m_sOwnCode != "")
            {
                if (MessageBox.Show("Are you sure you want to cancel this transaction?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    AppSettingsManager.TaskManager(m_sBIN, "ONLINE PAYMENT", "DELETE");
                    sString = "select chk_no from chk_tbl_temp where or_no = '" + txtORNo.Text + "'";
                    pSet.Query = sString;
                    if (pSet.Execute())
                        if (pSet.Read())
                            sChkNo = pSet.GetString("chk_no");
                    pSet.Close();

                    sString = "delete from chk_tbl_temp where or_no = '" + txtORNo.Text + "'";
                    pSet.Query = sString;
                    pSet.ExecuteNonQuery();

                    sString = "delete from multi_check_pay where chk_no = '" + sChkNo + "' and used_sw = 'N'";
                    pSet.Query = sString;
                    pSet.ExecuteNonQuery();
                }
                else
                    return;
            }
            else
            {
                sString = "select chk_no from chk_tbl_temp where or_no = '" + txtORNo.Text + "'";
                pSet.Query = sString;
                if (pSet.Execute())
                    if (pSet.Read())
                        sChkNo = pSet.GetString("chk_no");
                pSet.Close();

                sString = "delete from chk_tbl_temp where or_no = '" + txtORNo.Text + "'";
                pSet.Query = sString;
                pSet.ExecuteNonQuery();

                sString = "delete from multi_check_pay where chk_no = '" + sChkNo + "' and used_sw = 'N'";
                pSet.Query = sString;
                pSet.ExecuteNonQuery();
            }

            this.Close();
        }

        private void btnSearchBin_Click(object sender, EventArgs e)
        {
           if (btnSearchBin.Text.Trim() == "Search BIN")// && bin.txtTaxYear.Text.Trim() != string.Empty && bin.txtBINSeries.Text.Trim() != string.Empty
            {
                bin.txtTaxYear.Enabled = false;
                bin.txtBINSeries.Enabled = false;
                //btnSearchBin.Text = "Clear BIN";
                //OnLineSearch();
                OnSearch();

                // RMC 20141020 printing of OR (s)
                if (bin.txtTaxYear.Text.Trim() != string.Empty && bin.txtBINSeries.Text.Trim() != string.Empty)
                {
                    btnSearchBin.Text = "Clear BIN";
                    
                }
                // RMC 20141020 printing of OR (e)

                txtTaxYear.Focus();
                txtBnsCode.Focus();
            }
            else
            {
                if ((bin.txtTaxYear.Text.Trim() == string.Empty || bin.txtBINSeries.Text.Trim() == string.Empty)
                    && bin.txtTaxYear.Enabled == true)
                {
                    MessageBox.Show("No Business to be search");
                    return;
                }

                if (btnSearchBin.Text != "Search BIN")
                    if (txtTaxYear.Text.Trim() != "")
                        AppSettingsManager.TaskManager(m_sBIN, "ONLINE PAYMENT", "DELETE");

                m_sProv = "";
                m_sMun = "";
                m_sDist = "";
                m_sZone = "";
                m_sBrgy = "";
                m_sStreet = "";
                m_sAddrNo = "";
                m_sNewOwnCode = "";
                m_sPrevOwnCode = "";
                m_iORNo = 0;    // RMC 20151229 merged multiple OR use from Mati

                btnAccept.Enabled = false;

                bin.txtTaxYear.Enabled = true;
                bin.txtBINSeries.Enabled = true;
                btnSearchBin.Text = "Search BIN";
                CleanMe();
                OnCheck();
                OnCash();
            }
        }

        private void OnLineSearch()
        {
            string sTaxYear = string.Empty;
            int iCountBin = 0;
            bool bMultiBns = false;
            m_sMultiPay = "N";
            m_sPaymentType = "CS";
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            if (bin.txtTaxYear.Text.Trim() == string.Empty || bin.txtBINSeries.Text.Trim() == string.Empty)
            {

            }
            else
            {
                m_sBIN = bin.GetBin();
                result.Query = "select count(bin) as iCount from businesses where own_code in (select own_code from businesses where bin = '" + m_sBIN.Trim() + "')";
                if (result.Execute() && iCountBin > 0)
                {
                    if (result.Read())
                        iCountBin = result.GetInt("iCount");
                    if (iCountBin > 1 && m_sPaymentMode != "ONL")
                        bMultiBns = true;
                    else
                        bMultiBns = false;

                }
                result.Close();

                if (m_sPaymentMode == "ONL" || m_sPaymentMode == "OFL")
                {
                    result.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {

                        }
                        else
                        {
                            result.Close();
                            result.Query = "select * from businesses where bin = '" + m_sBIN.Trim() + "'";
                            {
                                if (result.Execute())
                                {
                                    if (result.Read())
                                    {
                                        dgvTaxFees.Rows.Clear();
                                        result2.Query = "select * from treasurers_module where bin = '" + m_sBIN.Trim() + "' and (action = '0' or action = '1') and tax_year <= '" + AppSettingsManager.GetCurrentDate().Year + "'";
                                        if (result2.Execute())
                                        {
                                            if (result2.Read())
                                            {
                                                MessageBox.Show("SOA is subject for verification.");
                                                return;
                                            }
                                        }
                                        result2.Close();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("NO BUSINESS FOUND.");
                                    // put the search dialog here
                                }
                            }
                        }

                    }
                    result.Close();


                }
                else
                {
                    OnFull();
                    BPLSAppSettingList sList = new BPLSAppSettingList();
                    sList.ReturnValueByBin = m_sBIN;
                    for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
                    {
                        txtBnsName.Text = sList.BPLSAppSettings[i].sBnsNm;
                        txtBnsAddress.Text = AppSettingsManager.GetBnsAddress(m_sBIN).Trim();
                        txtBnsOwner.Text = AppSettingsManager.GetBnsOwner(sList.BPLSAppSettings[i].sOwnCode);
                        txtBnsStat.Text = sList.BPLSAppSettings[i].sBnsStat;
                    }
                }
            }


            result.Query = "select * from bounce_chk_rec where bin = '" + m_sBIN.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    MessageBox.Show("Cannot proceed. \nThis record has been paid under a bounced check...");
                    return;
                }
            }
            result.Close();

            result.Query = "select distinct bin from closure_tagging where bin = '" + m_sBIN.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    MessageBox.Show("Record was tagged for closure. Cannot continue.");
                    return;
                }
            }
            result.Close();

            string sHRUser, sHRDate, sHRRemarks, sMess;
            result.Query = "select * from hold_records where bin = '" + m_sBIN.Trim() + "' and status = 'HOLD'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sHRUser = result.GetString("user_code");
                    sHRDate = result.GetString("dt_save");
                    sHRRemarks = result.GetString("remarks");
                    sMess = "Cannot continue! This record is currently on hold.\nUser Code: " + sHRUser + "  Date: " + sHRDate + "\nRemarks: " + sHRRemarks;
                    MessageBox.Show(sMess);
                    return;

                }
            }
            result.Close();

            if (m_sPaymentMode == "ONL" || m_sPaymentMode == "OFL")
            {
                // task manager
                result.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        m_sOwnCode = result.GetString("own_code").Trim();
                        result2.Query = "select * from permit_update_appl where bin = '" + m_sBIN.Trim() + "' and appl_type = 'TOWN' and data_mode = 'QUE'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                m_sPrevOwnCode = m_sOwnCode;
                                m_sOwnCode = result2.GetString("new_own_code").Trim();
                                m_bIsRenPUT = true;
                            }
                        }
                        result2.Close();

                        result2.Query = "select * from permit_update_appl where bin = '" + m_sBIN.Trim() + "' and appl_type = 'ADDL' and data_mode = 'QUE'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                m_bIsRenPUT = true;
                            }
                        }
                        result2.Close();

                        m_sCheckOwnerCode = m_sOwnCode;
                        txtBnsName.Text = result.GetString("bns_nm").Trim();
                        txtBnsCode.Text = result.GetString("bns_code").Trim();

                        result2.Query = "select * from permit_update_appl where bin = '" + m_sBIN.Trim() + "' and appl_type = 'CBNS' and data_mode = 'QUE'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {

                                txtBnsName.Text = result2.GetString("new_bns_nm").Trim();
                                m_sOldBnsName = result2.GetString("old_bns_nm").Trim();
                                m_bIsRenPUT = true;
                            }
                        }
                        result2.Close();


                        result2.Query = "select * from permit_update_appl where bin = '" + m_sBIN.Trim() + "' and appl_type = 'CTYP' and data_mode = 'QUE'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                result2.Close();
                                result2.Query = "select new_bns_code from change_class_tbl where bin = '" + m_sBIN.Trim() + "' and is_main = 'Y'";
                                if (result2.Read())
                                {
                                    txtBnsCode.Text = result2.GetString("new_bns_code").Trim();
                                    m_bIsRenPUT = true;
                                }
                            }
                        }
                        result2.Close();

                        txtBnsStat.Text = result.GetString("bns_stat").Trim();
                        txtTaxYear.Text = result.GetString("tax_year").Trim();
                        m_sDtOperated = result.GetDateTime("dt_operated").ToString("MM/dd/yyyy");


                        // RET001
                        if (txtBnsStat.Text.Trim() == "RET")
                        {
                            result2.Query = "select distinct bin from pay_hist where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    m_bRetVoidSurchPen = true;
                                }
                            }
                            result2.Close();
                        }

                        txtBnsAddress.Text = AppSettingsManager.GetBnsAdd(m_sBIN.Trim(), "");
                        txtBnsOwner.Text = AppSettingsManager.GetBnsOwner(m_sOwnCode);
                        txtBnsType.Text = AppSettingsManager.GetBnsDesc(AppSettingsManager.GetBnsCodeMain(m_sBIN.Trim()));
                        txtBnsCode.Text = AppSettingsManager.GetBnsCodeMain(m_sBIN.Trim());
                    }
                    else
                    {
                        OracleResultSet result3 = new OracleResultSet();
                        result2.Query = "select own_code, bns_nm,  bns_code, bns_stat, tax_year, dt_operated from businesses where bin = '" + m_sBIN.Trim() + "'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {

                                m_sOwnCode = result2.GetString("own_code").Trim();
                                result3.Query = "select * from permit_update_appl where bin = '" + m_sBIN.Trim() + "' and appl_type = 'TOWN' and data_mode = 'QUE'";
                                if (result3.Execute())
                                {
                                    if (result3.Read())
                                    {
                                        m_sPrevOwnCode = m_sOwnCode;
                                        m_sOwnCode = result3.GetString("new_own_code").Trim();
                                        m_bIsRenPUT = true;
                                    }
                                }
                                result3.Close();
                                // m_sPlaceOccupancy insert here

                                m_sCheckOwnerCode = m_sOwnCode;
                                txtBnsName.Text = result2.GetString("bns_nm").Trim();
                                txtBnsCode.Text = result2.GetString("bns_code").Trim();

                                result3.Query = "select * from permit_update_appl where bin = '" + m_sBIN.Trim() + "' and appl_type = 'CBNS' and data_mode = 'QUE'";
                                if (result3.Execute())
                                {
                                    if (result3.Read())
                                    {
                                        txtBnsName.Text = string.Empty;
                                        txtBnsName.Text = result3.GetString("new_bns_name").Trim();
                                        m_sOldBnsName = result3.GetString("old_bns_name").Trim();
                                        m_bIsRenPUT = true;
                                    }
                                }
                                result3.Close();


                                result3.Query = "select * from permit_update_appl where bin = '" + m_sBIN.Trim() + "' and appl_type = 'CTYP' and data_mode = 'QUE'";
                                if (result3.Execute())
                                {
                                    if (result3.Read())
                                    {
                                        result3.Close();
                                        result3.Query = "select new_bns_code from change_class_tbl where bin = '" + m_sBIN.Trim() + "' and is_main = 'Y'";
                                        if (result3.Read())
                                        {
                                            txtBnsCode.Text = result3.GetString("new_bns_code").Trim();
                                            m_bIsRenPUT = true;
                                        }
                                    }
                                }
                                result3.Close();

                                txtBnsStat.Text = result2.GetString("bns_stat").Trim();
                                txtTaxYear.Text = result2.GetString("tax_year").Trim();

                                if (txtBnsStat.Text.Trim() == "NEW")
                                {
                                    result3.Query = "select * from taxdues where bin = '" + m_sBIN.Trim() + "' order by tax_year desc";
                                    if (result3.Execute())
                                    {
                                        if (result3.Read())
                                        {
                                            string sTempYear = string.Empty;
                                            sTempYear = result3.GetString("tax_year");
                                            if (int.Parse(sTempYear) > int.Parse(txtTaxYear.Text.Trim()))
                                                txtBnsStat.Text = "REN";

                                        }
                                    }
                                    result3.Close();
                                }
                                m_sDtOperated = result2.GetDateTime("dt_operated").ToString("MM/dd/yyyy");
                                sTaxYear = txtTaxYear.Text.Trim();

                            }
                            else
                            {
                                MessageBox.Show("ONLINE PAYMENT");
                                dgvTaxFees.Rows.Clear();
                                return;
                            }
                            //result2.Close();
                            result3.Query = "select * from partial_payer where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                            if (result3.Execute())
                            {
                                if (result3.Read())
                                {

                                    MessageBox.Show("CHECK PARTIAL CHECK BOX");
                                    // partial = true;
                                }
                            }
                            result3.Close();


                        }
                        result2.Close();

                        m_sStatus = txtBnsStat.Text.Trim();
                        txtBnsAddress.Text = AppSettingsManager.GetBnsAdd(m_sBIN.Trim(), "");

                        result2.Query = "select new_bns_loc from permit_update_appl where bin = '" + m_sBIN.Trim() + "'and appl_type = 'TLOC' and data_mode = 'QUE'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                m_sPrevBnsLoc = txtBnsAddress.Text.Trim();
                                txtBnsAddress.Text = result2.GetString("new_bns_loc").Trim();
                                result2.Close();

                                result2.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "'";
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        m_bIsRenPUT = true;
                                    }
                                    else
                                        m_bIsRenPUT = false;
                                }
                            }
                        }
                        result2.Close();

                        txtBnsOwner.Text = AppSettingsManager.GetBnsOwner(m_sOwnCode.Trim());
                        txtBnsType.Text = AppSettingsManager.GetBnsDesc(AppSettingsManager.GetBnsCodeMain(m_sBIN.Trim()));

                        if (txtBnsStat.Text == "RET")
                            result2.Query = "select * from taxdues where bin = '" + m_sBIN.Trim() + "' and due_state = 'X' order by tax_year asc";
                        else
                            result2.Query = "select * from taxdues where bin = '" + m_sBIN.Trim() + "' order by tax_year asc";

                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {

                            }
                            else
                            {
                                result2.Close();
                                result2.Query = "select * from compromise_tbl where bin = '" + m_sBIN.Trim() + "'";
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        bCompromise = true;
                                    }
                                    else
                                    {
                                        if (int.Parse(sTaxYear) < AppSettingsManager.GetSystemDate().Year)
                                        {
                                            MessageBox.Show("Unrenewed Business or not yet billed.");
                                            return;
                                            dgvTaxFees.Rows.Clear();
                                        }
                                    }
                                }

                            }

                        }
                        result2.Close();

                        if (bCompromise == false)
                        {
                            if (txtBnsStat.Text == "RET")
                                result2.Query = "select * from taxdues where bin = '" + m_sBIN.Trim() + "' and due_state = 'X' order by tax_year desc";
                            else
                                result2.Query = "select * from taxdues where bin = '" + m_sBIN.Trim() + "' order by tax_year desc";
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                    txtTaxYear.Text = result2.GetString("tax_year").Trim();
                            }
                            result2.Close();

                        }

                        result2.Query = "delete from pay_temp where bin = '" + m_sBIN.Trim() + "'";
                        if (result2.ExecuteNonQuery() != 0)
                        {

                        }
                        result2.Close();

                        result2.Query = "delete from pay_temp_bns where bin = '" + m_sBIN.Trim() + "'";
                        if (result2.ExecuteNonQuery() != 0)
                        {

                        }
                        result2.Close();

                        dgvTaxFees.Rows.Clear();

                        result2.Query = "select * from compromise_due where bin = '" + m_sBIN.Trim() + "'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                m_bIsCompromise = true;
                                m_sCompPaySw = result2.GetString("pay_sw").Trim();
                            }
                            else
                                m_bIsCompromise = false;
                        }
                        result2.Close();

                    }
                    result.Close();

                    ComputeTaxAndFees(m_sBIN.Trim());

                }
            }

            result.Query = "select * from waive_tbl where bin = '" + m_sBIN.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sFullPartial = result.GetString("full_partial");
                    m_sQtr = result.GetString("qtr");

                }
            }
            result.Close();
        }

        private void OnSearch() //MCR 20140722
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            String sQuery, sTaxYear;
            bool bMultiBns = false;
            String sTransAppCode, sLName, sFName, sMI, sAppName = "";
            
            bInitialDP = true;

            bGuarantor = false;
            m_sMultiPay = "N";
            m_sRefCheckNo = "";

            bCompromise = false;
            m_bIsCompromise = false;
            m_bDiscQtr = false;

            m_sPaymentType = "CS";

            sTaxYear = txtTaxYear.Text;
            //  mv_sDiscountedTotal = "0.00";
            // (s) ALJ 03292005 for MANILA
            chk1st.Checked = false;
            chk2nd.Checked = false;
            chk3rd.Checked = false;
            chk4th.Checked = false;
            // (s) ALJ 03292005 for MANILA
            m_bIsRenPUT = false;

            // RMC 20141020 printing of OR (s)
            if (bin.txtTaxYear.Text.Trim() == string.Empty && bin.txtBINSeries.Text.Trim() == string.Empty)
            {
                frmSearchBusiness frmSearchBusiness = new frmSearchBusiness();
                frmSearchBusiness.ShowDialog();
                if (frmSearchBusiness.sBIN.Trim() != String.Empty)
                {
                    m_sBIN = StringUtilities.HandleApostrophe(frmSearchBusiness.sBIN);
                    bin.txtLGUCode.Text = frmSearchBusiness.sBIN.Substring(0, 3);
                    bin.txtDistCode.Text = frmSearchBusiness.sBIN.Substring(4, 2);
                    bin.txtTaxYear.Text = frmSearchBusiness.sBIN.Substring(7, 4);
                    bin.txtBINSeries.Text = frmSearchBusiness.sBIN.Substring(12, 7);
                    m_sOwnCode = frmSearchBusiness.sOwnCode;
                }
            }

            if (bin.txtTaxYear.Text.Trim() == string.Empty && bin.txtBINSeries.Text.Trim() == string.Empty)
                return;
            // RMC 20141020 printing of OR (e)

            m_sBIN = StringUtilities.HandleApostrophe(bin.GetBin());

            ////AFM 20210628 validation for soa approval (s)
            //string sStatus = string.Empty;
            //pSet.Query = "select status from soa_monitoring where bin = '" + m_sBIN + "' and (status = 'RETURNED' or status = 'PENDING')";
            //if (pSet.Execute())
            //    if (pSet.Read())
            //    {
            //        sStatus = pSet.GetString("status");
            //        if (sStatus == "PENDING")
            //        {
            //            MessageBox.Show("BIN's SOA is currently PENDING for approval!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            return;
            //        }
            //        if (sStatus == "RETURNED")
            //        {
            //            MessageBox.Show("BIN's SOA was RETURNED!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            return;
            //        }
            //    }
            ////AFM 20210628 validation for soa approval (e)

            //MCR 20140902 REM SPLBNS (s)
            String sQuerySplBns;
            bool blnIsSplBns = false;
            sQuerySplBns = string.Format("select * from spl_business_que where bin = '{0}' ", m_sBIN);
            pSet.Query = sQuerySplBns;
            if (pSet.Execute())
                if (pSet.Read())
                    blnIsSplBns = true;
            pSet.Close(); //MCR 20140902 REM SPLBNS (e) */

            int iCountBin = 0;

            sQuery = "select count(*) as bin_count from businesses where own_code in (select own_code from businesses where bin = '" + m_sBIN + "')";
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    iCountBin = pSet.GetInt("bin_count");
                    if (iCountBin > 1 && m_sPaymentMode != "ONL")
                        bMultiBns = true;
                    else
                        bMultiBns = false;
                }
            pSet.Close();

            if (blnIsSplBns == true)
                bMultiBns = false;

            if (AppSettingsManager.blnIsConflict(m_sBIN))
            {
                MessageBox.Show("Conflict Bin", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (AppSettingsManager.GrossMonitoring(m_sBIN))
            {
                MessageBox.Show("Gross is subject for approval.", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (AppSettingsManager.IsUnderTreasModule(m_sBIN))
            {
                MessageBox.Show("SOA is for verification.", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            String sQueryReAss = "";
            sQueryReAss = string.Format("select * from REASS_WATCH_LIST where bin = '{0}' and is_tag = '1'", m_sBIN);
            pSet.Query = sQueryReAss;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    MessageBox.Show("Please proceed to BPTFO for re-assessment.", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            pSet.Close();

            if (m_sPaymentMode == "ONL" || m_sPaymentMode == "OFL")
            {
                ///* MCR 20140902 REMOVE SPLBNS (s)
                if (blnIsSplBns == true) 
                sQuery = "select * from spl_business_que where bin = '" + m_sBIN + "'";
                //MCR 20140902 REMOVE SPLBNS (e) */
                else
                sQuery = "select * from business_que where bin = '" + m_sBIN + "'"; //
                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (!pSet.Read())
                    {
                        pSet.Close();
                        // RMC 20150102 enabled special business (s)
                        if (blnIsSplBns == true)
                            sQuery = "select * from spl_business_que where bin = '" + m_sBIN + "'"; // RMC 20150102 enabled special business (e)
                        else
                            sQuery = "select * from businesses where bin = '" + m_sBIN + "'"; //
                        pSet.Query = sQuery;
                        if (pSet.Execute())
                            if (!pSet.Read())
                            {
                                frmSearchBusiness frmSearchBusiness = new frmSearchBusiness();
                                frmSearchBusiness.ShowDialog();
                                if (frmSearchBusiness.sBIN.Trim() != String.Empty)
                                {
                                    m_sBIN = StringUtilities.HandleApostrophe(frmSearchBusiness.sBIN);
                                    bin.txtLGUCode.Text = frmSearchBusiness.sBIN.Substring(0, 3);
                                    bin.txtDistCode.Text = frmSearchBusiness.sBIN.Substring(4, 2);
                                    bin.txtTaxYear.Text = frmSearchBusiness.sBIN.Substring(7, 4);
                                    //bin.txtBINSeries.Text = frmSearchBusiness.sBIN.Substring(11, 7);
                                    bin.txtBINSeries.Text = frmSearchBusiness.sBIN.Substring(12, 7);    // RMC 20141020 printing of OR
                                    m_sOwnCode = frmSearchBusiness.sOwnCode;
                                }
                                else
                                {
                                    CleanMe();
                                    bin.txtTaxYear.Focus();
                                    return;
                                }
                            }
                    }
                pSet.Close();

                sQuery = string.Format("select * from treasurers_module where bin = '{0}' and (action = '0' or action = '1') and tax_year <= '{1}'", m_sBIN, AppSettingsManager.GetSystemDate().Year);
                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        MessageBox.Show("SOA is subject for verification.", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                pSet.Close();
            }
            else
            {
                if (!AppSettingsManager.TaskManager(m_sBIN, "ONLINE PAYMENT", " "))		// JJP 12082004 Fixed Bugs TaskManager
                {
                    OnFull();

                    ///*MCR 20140902 REMOVE SPLBNS (s)
                    if (blnIsSplBns == true)
                        sQuery = "select * from spl_business_que where bin = '" + m_sBIN + "'";
                    //MCR 20140902 REMOVE SPLBNS (e) */
                    else
                    sQuery = "select * from business_que where bin = '" + m_sBIN + "'";
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            txtBnsName.Text = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm").Trim());
                            txtTaxYear.Text = pSet.GetString("tax_year").Trim();
                            txtBnsStat.Text = pSet.GetString("bns_stat").Trim();
                        }
                        else
                        {
                            pSet.Close();
                            if (blnIsSplBns == true)
                                sQuery = "select * from spl_business_que where bin = '" + m_sBIN + "'";
                            else
                                sQuery = "select * from businesses where bin = '" + m_sBIN + "'";
                            if (pSet.Execute())
                                if (pSet.Read())
                                {
                                    txtBnsName.Text = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm").Trim());
                                    txtTaxYear.Text = pSet.GetString("tax_year").Trim();
                                    txtBnsStat.Text = pSet.GetString("bns_stat").Trim();
                                }
                        }
                    pSet.Close();

                    if (AppSettingsManager.IsForSoaPrint(m_sBIN, txtTaxYear.Text))
                    { }
                    else
                    {
                        MessageBox.Show("Cannot generate SOA / Or Accept Payment.", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    sQuery = string.Format("select transfer_table.* from transfer_table,trans_fees_table where transfer_table.bin = trans_fees_table.bin and transfer_table.trans_app_code = trans_fees_table.trans_app_code and trans_fees_table.tax_year = '{0}' and transfer_table.bin = '{1}'", txtTaxYear.Text.Trim(), m_sBIN);
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            while (pSet.Read())
                            {
                                sTransAppCode = pSet.GetString("trans_app_code").Trim();

                                if (sTransAppCode == "TO")
                                {
                                    sAppName += "Transfer of Ownership";
                                    sLName = StringUtilities.HandleApostrophe(pSet.GetString("own_ln").Trim());
                                    sFName = StringUtilities.HandleApostrophe(pSet.GetString("own_fn").Trim());
                                    sMI = StringUtilities.HandleApostrophe(pSet.GetString("own_mi").Trim());
                                    m_sPrevOwnCode = pSet.GetString("prev_own_code").Trim();
                                    m_sNewOwnCode = pSet.GetString("new_own_code").Trim();
                                    m_sOwnCode = m_sNewOwnCode;

                                    txtBnsOwner.Text = sLName + ", " + sFName + " " + sMI;
                                }
                                else if (sTransAppCode == "TL")
                                {
                                    sAppName += "Transfer of Location";
                                    m_sAddrNo = StringUtilities.HandleApostrophe(pSet.GetString("addr_no").Trim());
                                    m_sStreet = StringUtilities.HandleApostrophe(pSet.GetString("addr_street").Trim());
                                    m_sBrgy = StringUtilities.HandleApostrophe(pSet.GetString("addr_brgy").Trim());
                                    m_sZone = StringUtilities.HandleApostrophe(pSet.GetString("addr_zone").Trim());
                                    m_sDist = StringUtilities.HandleApostrophe(pSet.GetString("addr_dist").Trim());
                                    m_sMun = StringUtilities.HandleApostrophe(pSet.GetString("addr_mun").Trim());
                                    m_sProv = StringUtilities.HandleApostrophe(pSet.GetString("addr_prov").Trim());

                                    txtBnsAddress.Text = m_sAddrNo + " " + m_sStreet + ", " + m_sBrgy + " " + m_sZone + " " + m_sDist + " " + m_sMun + " " + m_sProv;
                                }
                                else if (sTransAppCode == "TC")
                                {
                                    sAppName += "Change of Classification";
                                    txtBnsCode.Text = pSet.GetString("bns_code").Trim();

                                    sQuery = "select bns_desc from bns_table where bns_code = '" + txtBnsCode.Text.Trim() + "'";
                                    pSet1.Query = sQuery;
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                            txtBnsType.Text = StringUtilities.HandleApostrophe(pSet1.GetString("bns_desc").Trim());
                                    pSet1.Close();
                                }
                            }
                            ///* MCR 20140902 REMOVE SPLBNS (s)
                            if (blnIsSplBns == true)
                                sQuery = "select * from spl_business_que where bin = '" + m_sBIN + "'";
                              //  MCR 20140902 REMOVE SPLBNS (e)*/
                            else
                            sQuery = "select * from business_que where bin = '" + m_sBIN + "'";
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                {
                                    if (txtBnsOwner.Text.Trim() == String.Empty)
                                    {
                                        m_sNewOwnCode = pSet1.GetString("own_code").Trim();
                                        m_sOwnCode = m_sNewOwnCode;
                                        m_sCheckOwnerCode = m_sOwnCode;

                                        sQuery = string.Format("select own_ln,own_fn,own_mi from own_names where own_code = '{0}'", m_sNewOwnCode);
                                        pSet2.Query = sQuery;
                                        if (pSet2.Execute())
                                            if (pSet2.Read())
                                            {
                                                sLName = StringUtilities.HandleApostrophe(pSet2.GetString("own_ln").Trim());
                                                sFName = StringUtilities.HandleApostrophe(pSet2.GetString("own_fn").Trim());
                                                sMI = StringUtilities.HandleApostrophe(pSet2.GetString("own_mi").Trim());
                                                txtBnsName.Text = sLName + ", " + sFName + " " + sMI;
                                            }
                                        pSet2.Close();
                                    }

                                    if (txtBnsAddress.Text.Trim() == String.Empty)
                                    {
                                        m_sAddrNo = StringUtilities.HandleApostrophe(pSet.GetString("bns_house_no").Trim());
                                        m_sStreet = StringUtilities.HandleApostrophe(pSet.GetString("bns_street").Trim());
                                        m_sBrgy = StringUtilities.HandleApostrophe(pSet.GetString("bns_brgy").Trim());
                                        m_sZone = StringUtilities.HandleApostrophe(pSet.GetString("bns_zone").Trim());
                                        m_sDist = StringUtilities.HandleApostrophe(pSet.GetString("bns_dist").Trim());
                                        m_sMun = StringUtilities.HandleApostrophe(pSet.GetString("bns_mun").Trim());
                                        m_sProv = StringUtilities.HandleApostrophe(pSet.GetString("bns_prov").Trim());
                                        txtBnsAddress.Text = m_sAddrNo + " " + m_sStreet + ", " + m_sBrgy + " " + m_sZone + " " + m_sDist + " " + m_sMun + " " + m_sProv;
                                    }

                                    if (txtBnsCode.Text.Trim() == String.Empty)
                                    {
                                        txtBnsCode.Text = pSet1.GetString("bns_code").Trim();
                                        sQuery = "select bns_desc from bns_table where bns_code = '" + txtBnsCode.Text.Trim() + "' and fees_code = 'B' and rev_year = '" + AppSettingsManager.GetConfigValue("07").Trim() + "'";
                                        if (pSet2.Execute())
                                            if (pSet2.Read())
                                                txtBnsAddress.Text = StringUtilities.HandleApostrophe(pSet2.GetString("bns_desc").Trim());
                                        pSet2.Close();
                                    }
                                }
                                else
                                {
                                    pSet1.Close();
                                    if (blnIsSplBns == true)
                                        sQuery = "select * from spl_business_que where bin = '" + m_sBIN + "'";
                                    else
                                        sQuery = "select * from businesses where bin = '" + m_sBIN + "'";
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                        {
                                            if (txtBnsOwner.Text.Trim() == String.Empty)
                                            {
                                                m_sNewOwnCode = pSet1.GetString("own_code").Trim();
                                                m_sOwnCode = m_sNewOwnCode;
                                                m_sCheckOwnerCode = m_sOwnCode;

                                                sQuery = string.Format("select own_ln,own_fn,own_mi from own_names where own_code = '{0}'", m_sNewOwnCode);
                                                pSet2.Query = sQuery;
                                                if (pSet2.Execute())
                                                    if (pSet2.Read())
                                                    {
                                                        sLName = StringUtilities.HandleApostrophe(pSet2.GetString("own_ln").Trim());
                                                        sFName = StringUtilities.HandleApostrophe(pSet2.GetString("own_fn").Trim());
                                                        sMI = StringUtilities.HandleApostrophe(pSet2.GetString("own_mi").Trim());
                                                        txtBnsName.Text = sLName + ", " + sFName + " " + sMI;
                                                    }
                                                pSet2.Close();
                                            }

                                            if (txtBnsAddress.Text.Trim() == String.Empty)
                                            {
                                                m_sAddrNo = StringUtilities.HandleApostrophe(pSet.GetString("bns_house_no").Trim());
                                                m_sStreet = StringUtilities.HandleApostrophe(pSet.GetString("bns_street").Trim());
                                                m_sBrgy = StringUtilities.HandleApostrophe(pSet.GetString("bns_brgy").Trim());
                                                m_sZone = StringUtilities.HandleApostrophe(pSet.GetString("bns_zone").Trim());
                                                m_sDist = StringUtilities.HandleApostrophe(pSet.GetString("bns_dist").Trim());
                                                m_sMun = StringUtilities.HandleApostrophe(pSet.GetString("bns_mun").Trim());
                                                m_sProv = StringUtilities.HandleApostrophe(pSet.GetString("bns_prov").Trim());
                                                txtBnsAddress.Text = m_sAddrNo + " " + m_sStreet + ", " + m_sBrgy + " " + m_sZone + " " + m_sDist + " " + m_sMun + " " + m_sProv;
                                            }

                                            if (txtBnsCode.Text.Trim() == String.Empty)
                                            {
                                                txtBnsCode.Text = pSet1.GetString("bns_code").Trim();
                                                sQuery = "select bns_desc from bns_table where bns_code = '" + txtBnsCode.Text.Trim() + "' and fees_code = 'B' and rev_year = '" + AppSettingsManager.GetConfigValue("07").Trim() + "'";
                                                if (pSet2.Execute())
                                                    if (pSet2.Read())
                                                        txtBnsAddress.Text = StringUtilities.HandleApostrophe(pSet2.GetString("bns_desc").Trim());
                                                pSet2.Close();
                                            }
                                        }

                                }
                            pSet1.Close();

                            LoadBills();
                            ComputeTotal();
                            chkCash.Enabled = true;
                            chkCheque.Enabled = true;
                            chkFull.Enabled = true;
                            if (m_sPaymentMode != "OFL")
                                btnAccept.Enabled = true;
                            else
                            {
                                bool bIsGrandtedSOA = false;
                                bool bIsGrantedPayment = false;
                                sQuery = string.Format("select * from sys_rights where usr_code = '{0}' and usr_rights = 'CTSOA-OFL'", AppSettingsManager.TellerUser.UserCode);
                                pSet1.Query = sQuery;
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                        bIsGrandtedSOA = true;
                                pSet1.Close();

                                sQuery = string.Format("select * from sys_rights where usr_code = '{0}' and usr_rights = 'CTPAY-OFL'", AppSettingsManager.TellerUser.UserCode);
                                pSet1.Query = sQuery;
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                        bIsGrantedPayment = true;
                                pSet1.Close();

                                if (bIsGrantedPayment)
                                    btnAccept.Enabled = true;
                                else
                                    btnAccept.Enabled = false;
                            }
                        }
                        else
                        {
                            txtBnsName.Text = "";
                            txtBnsAddress.Text = "";
                            txtBnsOwner.Text = "";
                            txtBnsType.Text = "";
                            txtBnsCode.Text = "";
                            txtBnsStat.Text = "";
                            txtTaxYear.Text = "";
                            chkCash.Enabled = false;
                            chkCheque.Enabled = false;
                            chkFull.Enabled = false;
                            chkInstallment.Enabled = false;
                            btnAccept.Enabled = false;
                            AppSettingsManager.TaskManager(m_sBIN, "ONLINE PAYMENT", "DELETE");
                            MessageBox.Show("No Pending Application or not yet Billed.", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    pSet.Close();
                }
            }

            sQuery = "select * from bounce_chk_rec where bin = '" + m_sBIN + "'";
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    MessageBox.Show("Cannot proceed. \nThis record has been paid under a bounced check...", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            pSet.Close();

            String sQueryTrans;
            sQueryTrans = string.Format("select distinct bin from closure_tagging where bin = '{0}'", m_sBIN);
            pSet.Query = sQueryTrans;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    MessageBox.Show("Record was tagged for closure. Cannot continue.", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            pSet.Close();

            String sHRUser, sHRDate, sHRRemarks, sMess;
            sQuery = string.Format("select * from hold_records where bin = '{0}' and status = 'HOLD'", m_sBIN);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sHRUser = pSet.GetString("user_code").Trim();
                    sHRDate = pSet.GetString("dt_save").Trim();
                    sHRRemarks = pSet.GetString("remarks").Trim();
                    sMess = "Cannot continue! This record is currently on hold.\nUser Code: " + sHRUser + "  Date: " + sHRDate + "\nRemarks: " + sHRRemarks;
                    MessageBox.Show(sMess, " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            pSet.Close();

            ///*  MCR 20140902 REM SPLBNS (s)
            blnIsSplBns = false;
            sQuerySplBns = string.Format("select * from spl_business_que where bin = '{0}' ", m_sBIN);
            pSet1.Query = sQuerySplBns;
            if (pSet1.Execute())
                if (pSet1.Read())
                    blnIsSplBns = true;
            pSet1.Close();  //MCR 20140902 REM SPLBNS (e) */


            if (m_sPaymentMode == "ONL" || m_sPaymentMode == "OFL")
            {
                if (!AppSettingsManager.TaskManager(m_sBIN, "ONLINE PAYMENT", " "))
                {
                    m_bRetVoidSurchPen = false;

                    if (blnIsSplBns == true) //MCR 20140902 REM SPLBNS (s)
                        sQuery = string.Format("select * from spl_business_que where bin = '{0}'", m_sBIN);
                    else
                    sQuery = string.Format("select * from business_que where bin = '{0}'", m_sBIN);

                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            m_sOwnCode = pSet.GetString("own_code").Trim();
                            sQuery = "select new_own_code from permit_update_appl where bin = '" + m_sBIN + "' and appl_type = 'TOWN' and data_mode = 'QUE'";
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                {
                                    m_sPrevOwnCode = m_sOwnCode;
                                    m_sOwnCode = pSet1.GetString("new_own_code").Trim();
                                    m_bIsRenPUT = true;
                                }
                            pSet1.Close();

                            sQuery = "select * from permit_update_appl where bin = '" + m_sBIN + "' and appl_type = 'ADDL' and data_mode = 'QUE'";
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                    m_bIsRenPUT = true;
                            pSet1.Close();

                            m_sCheckOwnerCode = m_sOwnCode;
                            txtBnsName.Text = pSet.GetString("bns_nm").Trim();
                            txtBnsCode.Text = pSet.GetString("bns_code").Trim();

                            sQuery = "select * from permit_update_appl where bin = '" + m_sBIN + "' and appl_type = 'CBNS' and data_mode = 'QUE'";
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                {
                                    txtBnsName.Text = pSet1.GetString("new_bns_name").Trim();
                                    m_sOldBnsName = pSet1.GetString("old_bns_name").Trim();
                                    m_bIsRenPUT = true;
                                }
                            pSet1.Close();

                            sQuery = "select * from permit_update_appl where bin = '" + m_sBIN + "' and appl_type = 'CTYP' and data_mode = 'QUE'";
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                {
                                    pSet1.Close();
                                    sQuery = "select new_bns_code from change_class_tbl where bin = '" + m_sBIN + "' and is_main = 'Y'";
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                        {
                                            txtBnsCode.Text = pSet1.GetString("new_bns_code").Trim();
                                            m_bIsRenPUT = true;
                                        }
                                }
                            pSet1.Close();

                            txtBnsStat.Text = pSet.GetString("bns_stat").Trim();
                            txtTaxYear.Text = pSet.GetString("tax_year").Trim();
                            sTaxYear = txtTaxYear.Text;
                            m_sDtOperated = pSet.GetDateTime("dt_operated").ToString("MM/dd/yyyy");

                            if (txtBnsStat.Text == "RET")
                            {
                                sQuery = string.Format("select distinct bin from pay_hist where bin = '{0}' and tax_year = '{1}'", m_sBIN, txtTaxYear.Text.Trim());
                                pSet1.Query = sQuery;
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                        m_bRetVoidSurchPen = true;
                                pSet1.Close();
                            }
                        }
                        else
                        {
                            pSet.Close();

                            if (blnIsSplBns == true) //MCR 20140902 REM SPLBNS (s)
                                sQuery = string.Format("select * from spl_business_que where bin = '{0}'", m_sBIN);
                            else
                            sQuery = string.Format("select * from businesses where bin = '{0}'", m_sBIN);

                            pSet.Query = sQuery;
                            if (pSet.Execute())
                                if (pSet.Read())
                                {
                                    m_sOwnCode = pSet.GetString("own_code").Trim();
                                    sQuery = "select new_own_code from permit_update_appl where bin = '" + m_sBIN + "' and appl_type = 'TOWN' and data_mode = 'QUE'";
                                    pSet1.Query = sQuery;
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                        {
                                            m_sPrevOwnCode = m_sOwnCode;
                                            m_sOwnCode = pSet1.GetString("new_own_code").Trim();
                                        }
                                    pSet1.Close();

                                    m_sPlaceOccupancy = pSet.GetString("place_occupancy").Trim();

                                    m_sCheckOwnerCode = m_sOwnCode;
                                    txtBnsName.Text = pSet.GetString("bns_nm").Trim();
                                    txtBnsCode.Text = pSet.GetString("bns_code").Trim();

                                    sQuery = "select * from permit_update_appl where bin = '" + m_sBIN + "' and appl_type = 'CBNS' and data_mode = 'QUE'";
                                    pSet1.Query = sQuery;
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                        {
                                            txtBnsName.Text = pSet1.GetString("new_bns_name").Trim();
                                            m_sOldBnsName = pSet1.GetString("old_bns_name").Trim();
                                            m_bIsRenPUT = true;
                                        }
                                    pSet1.Close();

                                    sQuery = "select * from permit_update_appl where bin = '" + m_sBIN + "' and appl_type = 'CTYP' and data_mode = 'QUE'";
                                    pSet1.Query = sQuery;
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                        {
                                            pSet1.Close();
                                            sQuery = "select new_bns_code from change_class_tbl where bin = '" + m_sBIN + "' and is_main = 'Y'";
                                            pSet1.Query = sQuery;
                                            if (pSet1.Execute())
                                                if (pSet1.Read())
                                                    txtBnsCode.Text = pSet1.GetString("new_bns_code").Trim();
                                        }
                                    pSet1.Close();

                                    txtBnsStat.Text = pSet.GetString("bns_stat").Trim();
                                    txtTaxYear.Text = pSet.GetString("tax_year").Trim();

                                    if (txtBnsStat.Text == "NEW")
                                    {
                                        sQuery = "select * from taxdues where bin = '" + m_sBIN + "' order by tax_year desc";
                                        pSet1.Query = sQuery;
                                        if (pSet1.Execute())
                                            if (pSet1.Read())
                                            {
                                                String sTmpTaxyear;
                                                sTmpTaxyear = pSet1.GetString("tax_year");
                                                if (Convert.ToInt32(sTmpTaxyear) > Convert.ToInt32(txtTaxYear.Text))
                                                    txtBnsStat.Text = "REN";
                                            }
                                        pSet1.Close();
                                    }

                                    m_sDtOperated = pSet.GetDateTime("dt_operated").ToString("MM/dd/yyyy");
                                    sTaxYear = txtTaxYear.Text;
                                }
                                else
                                {
                                    MessageBox.Show("Business does not exist", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    bin.txtTaxYear.Focus();
                                    CleanMe();
                                    AppSettingsManager.TaskManager(m_sBIN, "ONLINE PAYMENT", "DELETE");
                                    return;
                                }

                            //Partial
                            sQuery = string.Format("select * from partial_payer where bin = '{0}' and tax_year = '{1}'", m_sBIN, txtTaxYear.Text.Trim());
                            pSet.Query = sQuery;
                            if (pSet.Execute())
                                if (pSet.Read())
                                    chkPartial.Checked = true;

                        }
                    pSet.Close();

                    m_sStatus = txtBnsStat.Text.Trim();

                    txtBnsAddress.Text = AppSettingsManager.GetBnsAddress(m_sBIN);

                    sQuery = "select new_bns_loc from permit_update_appl where bin = '" + m_sBIN + "' and appl_type = 'TLOC' and data_mode = 'QUE'";
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            m_sPrevBnsLoc = txtBnsAddress.Text;
                            sQuery = string.Format("select * from business_que where bin = '{0}'", m_sBIN);
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                            {
                                if (pSet1.Read())
                                    m_bIsRenPUT = true;
                                else
                                    m_bIsRenPUT = false;
                            }
                            else
                                m_bIsRenPUT = false;

                            pSet1.Close();
                        }
                    pSet.Close();

                    txtBnsOwner.Text = AppSettingsManager.GetBnsOwner(m_sOwnCode);
                    txtBnsType.Text = AppSettingsManager.GetBnsDesc(txtBnsCode.Text);
                    if (m_sStatus == "RET")
                        sQuery = string.Format("select * from taxdues where bin = '{0}' and due_state = 'X' order by tax_year desc", m_sBIN);
                    else
                        sQuery = string.Format("select * from taxdues where bin = '{0}' order by tax_year desc", m_sBIN);

                    pSet.Query = sQuery;
                    if (pSet.Execute())
                    {
                        if (!pSet.Read())
                        {
                            pSet.Close();
                            sQuery = "select * from compromise_tbl where bin = '" + m_sBIN + "'";
                            if (pSet.Execute())
                                if (!pSet.Read())
                                {
                                    if (Convert.ToInt32(sTaxYear) < dtpORDate.Value.Year)
                                    {
                                        MessageBox.Show("Unrenewed Business or not yet billed", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        bin.txtTaxYear.Focus();
                                        //CleanMe();
                                        return;
                                    }

                                    MessageBox.Show("No Taxdues for the said BIN", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    bin.txtTaxYear.Focus();
                                    //CleanMe();
                                    return;
                                }
                                else
                                    bCompromise = true;
                        }

                        if (bCompromise == false)
                            txtTaxYear.Text = pSet.GetString("tax_year");
                    }
                    pSet.Close();

                    sQuery = string.Format("delete from pay_temp where bin = '{0}'", m_sBIN); // CTS 09152003
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    sQuery = string.Format("delete from pay_temp_bns where bin = '{0}'", m_sBIN); // CTS 09152003
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    dgvTaxFees.Rows.Clear();

                    sQuery = "select * from compromise_due where bin = '" + m_sBIN + "'";
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            m_sCompPaySw = pSet.GetString("pay_sw").Trim();
                            m_bIsCompromise = true;
                        }
                        else
                            m_bIsCompromise = false;
                    pSet.Close();

                    ComputeTaxAndFees(m_sBIN);
                }
            }

            txtTotDues.Text = "0.00";
            txtCreditLeft.Text = "0.00";
            txtToTaxCredit.Text = "0.00";
            chkTCredit.Enabled = false;

            if (m_sOwnCode != "")
            {
                // RMC 20141127 QA Tarlac ver (s)
                //if (AppSettingsManager.GetConfigValue("10") == "017")
                //    sQuery = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N' and multi_pay = 'N'", m_sOwnCode);
                //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                //    sQuery = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N' and multi_pay = 'N'", m_sOwnCode);
                //else// RMC 20141127 QA Tarlac ver (e)
                    sQuery = string.Format("select * from dbcr_memo where bin = '{0}' and served = 'N' and multi_pay = 'N'", m_sBIN);// GDE 20110811 //JARS 20171010
                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        m_dCreditLeft = pSet.GetDouble("balance");
                        txtCreditLeft.Text = m_dCreditLeft.ToString();
                    }
                pSet.Close();
            }

            bool bDeducted = false;

            //Discount
            if ((Convert.ToDouble(m_sDiscountedTotal) <= Convert.ToDouble(txtCreditLeft.Text)) && (Convert.ToDouble(txtCreditLeft.Text) != 0))
            {
                bDeducted = true;
                txtToTaxCredit.ReadOnly = true;
                txtToTaxCredit.Text = Convert.ToDouble(m_sDiscountedTotal).ToString("#,##0.00");
            }

            if (Convert.ToDouble(txtCreditLeft.Text) > 0.00 && (Convert.ToDouble(m_sDiscountedTotal) > Convert.ToDouble(txtCreditLeft.Text)))
            {
                if (bMultiBns)
                {
                    bDeducted = false;
                    txtToTaxCredit.Text = "0.00";
                }
                else
                {
                    bDeducted = true;
                    txtToTaxCredit.Text = Convert.ToDouble(txtCreditLeft.Text).ToString("#,##0.00");
                }

            }

            CompLessCredit(m_sDiscountedTotal, m_sCreditLeft, txtToTaxCredit.Text);


            if (m_sPaymentTerm == "I" && !chk1st.Checked)
                chkPartial.Enabled = false;
            else
                chkPartial.Enabled = false;

            chkCash.Checked = false;
            chkCheque.Checked = false;
            chkTCredit.Enabled = false;
            chkTCredit.Checked = false;

            if (bDeducted == true)
            {
                chkTCredit.Enabled = true;
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

            sQuery = string.Format("select * from waive_tbl where bin = '{0}'", m_sBIN);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    m_sFullPartial = pSet.GetString("full_partial");
                    m_sQtr = pSet.GetString("qtr");
                }
                else
                {
                    m_sFullPartial = "";
                    m_sQtr = "";
                }
            pSet.Close();

            int iQtr;
            sQuery = string.Format("select * from taxdues where bin = '{0}' and due_state <> 'X'", m_sBIN);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    iQtr = Convert.ToInt32(pSet.GetString("qtr_to_pay"));

                    if (iQtr > 1)
                    {
                        pSet.Close();

                        if (blnIsSplBns == true) //MCR 20140902 REM SPLBNS (s)
                            sQuery = string.Format("select * from spl_business_que where bin = '{0}'", m_sBIN);
                        else
                        sQuery = string.Format("select * from businesses where bin = '{0}'", m_sBIN);
                        pSet.Query = sQuery;
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                if (txtBnsStat.Text.ToString().Trim() == "") // RMC 20171003 corrected bns stat in payment transaction
                                    txtBnsStat.Text = pSet.GetString("bns_stat");
                            }
                    }
                }
            pSet.Close();

            sQuery = string.Format("select * from retired_bns_temp where bin = '{0}'", m_sBIN);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    chkRetirement.Enabled = true;
                    chkRetirement.Checked = true;
                    OnRetirement();

                    // RMC 20150501 QA BTAS (s)
                    //chkInstallment.Enabled = false;   // RMC 20170228 correction in SOA of RET, put rem
                    // RMC 20150501 QA BTAS (e)
                }
                else
                    chkRetirement.Enabled = false;
            pSet.Close();

            sQuery = string.Format("select * from taxdues where bin = '{0}' and due_state = 'A'", m_sBIN);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    chkAdj.Checked = true;
                    chkAdj.Enabled = false;
                    chkCharges.Checked = true;
                    chkCharges.Enabled = true;
                    OnCheckAdjLate();
                }
                else
                {
                    chkAdj.Checked = false;
                    chkAdj.Enabled = false;
                    chkCharges.Checked = false;
                    chkCharges.Enabled = false;
                }
            pSet.Close();

            sQuery = string.Format("select * from taxdues where bin = '{0}' and (due_state = 'R' or due_state = 'N') ", m_sBIN);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    if(pSet.GetString("due_state") == "N")  // RMC 20141217 adjustments
                    {
                        chkFull.Enabled = false;
                        chkInstallment.Enabled = false;
                    }
                                        
                }
            pSet.Close();

            if (m_bIsCompromise == true && bCompromise == false)
            {
                chkFull.Enabled = true;
                chkInstallment.Enabled = true;
            }

            if (txtBnsStat.Text == "NEW" && AppSettingsManager.GetConfigValue("15").ToUpper() == "Y")
            {
                sQuery = string.Format("select * from pay_hist where bin = '{0}' and bns_stat = 'NEW'", m_sBIN);
                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        OnInstallmentNew(m_sBIN);
                        chkFull.Enabled = false;
                    }
                pSet.Close();
            }

            if (m_sPaymentMode == "OFL")
            {
                //OFLPaymentCondition(); MCR 20140902 REM This Code Doesn't Exist on Antipolo Version
            }

            m_blnIsFull = true;
            sQuery = string.Format("select * from pay_hist where bin = '{0}' and tax_year = '{1}'", m_sBIN, txtTaxYear.Text);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    m_blnIsFull = false;
            pSet.Close();

            ////MCR 20140723 (s)
            //if (chkInstallment.Checked)
            //    OnInstallmentNew(m_sBIN);
            //else if (chkFull.Checked)
            //    OnFull();
            ////MCR 20140723 (e)

            m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
        }

        private void OFLPaymentCondition()//MCR 20140902
        {
            bool bIsGrandtedSOA = false;
            bool bIsGrantedPayment = false;
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = string.Format("select * from sys_rights where usr_code = '{0}' and usr_rights = 'CTSOA-OFL'", AppSettingsManager.TellerUser.UserCode);
            if (pSet.Execute())
                if (pSet.Read())
                    bIsGrandtedSOA = true;
            pSet.Close();

            pSet.Query = string.Format("select * from sys_rights where usr_code = '{0}' and usr_rights = 'CTPAY-OFL'", AppSettingsManager.TellerUser.UserCode);
            if (pSet.Execute())
                if (pSet.Read())
                    bIsGrantedPayment = true;
            pSet.Close();

            if (bIsGrantedPayment)
                btnAccept.Enabled = true;
            else
                btnAccept.Enabled = false;
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

            m_sDiscountedTotal = string.Format("{0}", m_dTotalTotalDue - Convert.ToDouble(txtDiscountAmount.Text));
        }

        public void ComputeTaxAndFees(string sBin)
        {
            m_sRevYear = AppSettingsManager.GetConfigObject("07");
            m_sIsQtrly = "N";
            DateTime dtSystemDate = DateTime.Now;
            DateTime dtDateOperated = DateTime.Now;
            string sQtrToPay = string.Empty;
            string sBnsCodeMain, sOldTaxYear, sNewtaxYear, sTaxYear;
            string sCurrentYear, sToday, sDateOperated = string.Empty, sYearOperated = string.Empty;
            dtSystemDate = AppSettingsManager.GetSystemDate();
            sToday = string.Format("{0:MM/dd/yyyy}", dtSystemDate);
            sToday = string.Format("{0:MM/dd/yyyy}", dtpORDate.Value);      // RMC 20170131 recompute dues displayed when date was changed in Offline payment
            sCurrentYear = ConfigurationAttributes.CurrentYear;

            TaxDuesStruct t;
            PayTempStruct p;
            PenRate pr;

            pr.p_dSurchRate1 = 0;
            pr.p_dSurchRate2 = 0;
            pr.p_dSurchRate3 = 0;
            pr.p_dSurchRate4 = 0;
            pr.p_dPenRate1 = 0;
            pr.p_dPenRate2 = 0;
            pr.p_dPenRate3 = 0;
            pr.p_dPenRate4 = 0;
            pr.p_SurchQuart = 0;
            pr.p_PenQuart = 0;

            t.tax_year = "";
            t.qtr_to_pay = "";
            t.due_state = "";

            if (txtBnsStat.Text.Trim() == "REN")
                m_sIsQtrly = "Y";

            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();

            result.Query = "select count(distinct tax_year) as iCount from taxdues where bin = '" + sBin + "'";
            if (result.Execute())
                if (result.Read())
                    m_iMultiTaxYear = result.GetInt("iCount");
            result.Close();

            if (m_iMultiTaxYear > 1)
                txtTaxYear.ReadOnly = false;
            else
                txtTaxYear.ReadOnly = true;

            result.Query = "select * from businesses where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    dtDateOperated = result.GetDateTime("dt_operated");
                    sDateOperated = string.Format("{0:MM/dd/yyyy}", dtDateOperated);
                    sYearOperated = sDateOperated.Substring(6, 4);
                }
                else
                {
                    result.Close();
                    result.Query = "select * from business_que where bin = '" + sBin + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            dtDateOperated = result.GetDateTime("dt_operated");
                            sDateOperated = string.Format("{0:MM/dd/yyyy}", dtDateOperated);
                            sYearOperated = sDateOperated.Substring(6, 4);
                        }
                    }
                }
            }
            result.Close();

            // RMC 20150112 mods in retirement billing (s)
            if (txtBnsStat.Text.Trim() == "RET")
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'X' order by tax_year asc";
            else
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' order by tax_year DESC";
            if (result.Execute())
            {
                if (result.Read())
                {
                    if (AppSettingsManager.GetConfigObject("71") != "Y") //AFM 20191129 MAO-19-11416 partial payment
                    {
                        txtTaxYear.Text = result.GetString("tax_year").Trim();
                    }
                    //sDueState = result.GetString("due_state");
                    if (txtBnsStat.Text.Trim() == "RET")   // RMC 20150108
                        m_sNextQtrToPay = result.GetString("QTR_TO_PAY");
                }
            }
            result.Close();
            // RMC 20150112 mods in retirement billing (e)

            result.Query = "delete from pay_temp where bin = '" + sBin.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            result.Query = "delete from pay_temp_bns where bin = '" + sBin.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            //// RMC 20141105 Modified Compromise Agreement Module (s)
            //int iIsCompromise = 0;
            //result.Query = "select * from compromise_due ";
            //result.Query += "where bin = '" + sBin.Trim() + "'";
            //if (result.Execute())
            //{
            //    if (result.Read())
            //    {
            //        iIsCompromise = 1;
            //    }
            //    else
            //        iIsCompromise = 0;
            //}
            //result.Close();
            //// RMC 20141105 Modified Compromise Agreement Module (e)

            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            //plsqlCmd.ProcedureName = "compute_tax_and_fees";
            plsqlCmd.ProcedureName = "compute_tax_and_fees_new";    // RMC 20170725 configured penalty computation, merged from Binan
            plsqlCmd.ParamValue = sBin;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sToday;
            plsqlCmd.AddParameter("m_sOrDate", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sCurrentYear;
            plsqlCmd.AddParameter("m_sCurrentTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtBnsStat.Text;
            plsqlCmd.AddParameter("m_sStatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtTaxYear.Text;
            plsqlCmd.AddParameter("m_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_iMultiTaxYear;
            plsqlCmd.AddParameter("m_iMultiTaxYear", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sRevYear;
            plsqlCmd.AddParameter("m_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sDateOperated;
            plsqlCmd.AddParameter("m_sDateOperated", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sYearOperated;
            plsqlCmd.AddParameter("m_sYearOperated", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sIsQtrly;
            plsqlCmd.AddParameter("m_sQtrly", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sNextQtrToPay;
            plsqlCmd.AddParameter("m_sNxtQtrToPay", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Output);
            //plsqlCmd.ParamValue = iIsCompromise;    // RMC 20141105 Modified Compromise Agreement Module
            //plsqlCmd.AddParameter("m_iCompromise", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Output);    // RMC 20141105 Modified Compromise Agreement Module
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
            }
            plsqlCmd.Close();
            m_sQtr1 = "F";
            m_sTerm = "F";
            bool bEntered = false;

            #region Hide
            // for displaying dues
            if (txtBnsStat.Text.Trim() == "RET")
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'X' and tax_year <= '" + txtTaxYear.Text.Trim() + "'";
                
            else
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state <> 'P' and tax_year <= '" + txtTaxYear.Text.Trim() + "' order by tax_year desc ,qtr_to_pay desc ,due_state desc";
               // result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state <> 'X' and tax_year <= '" + txtTaxYear.Text.Trim() + "' order by tax_year desc ,qtr_to_pay desc ,due_state desc"; // RMC 20181113 corrected SOA of Permit-update billing //JHB 20181120
                if (result.Execute())
            {
                m_bEnableTaxDue = true;
                while (result.Read())
                //if (result.Read())
                {
                    bEntered = true;
                    sQtrToPay = result.GetString("qtr_to_pay").Trim();
                    //if (sQtrToPay != "1" && m_iMultiTaxYear != 1)
                    if (sQtrToPay != "1" && m_iMultiTaxYear == 1)
                    {
                        m_sNextQtrToPay = sQtrToPay;
                        chkInstallment_Click(chkInstallment, null); //OnInstallmentNew(sBin);
                    }
                    else
                        if (sQtrToPay != "1" && m_iMultiTaxYear != 1)
                        {
                            result2.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'A'  and bin in (select bin from taxdues where due_state = 'X')";
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    m_sNextQtrToPay = sQtrToPay;
                                    //OnInstallmentNew(sBin);
                                }
                            }
                            result2.Close();
                        }
                        else
                        {
                            m_sNextQtrToPay = sQtrToPay;
                            chk1st.Checked = false;
                            chk2nd.Checked = false;
                            chk3rd.Checked = false;
                            chk4th.Checked = false;
                            chkFull.Checked = true;
                            chkInstallment.Checked = false;
                            chkFull_Click(chkFull, null); //DisplayDuesNew(sBin, "F", "", "", "");
                        }
                    
                    if (m_iMultiTaxYear != 1 && int.Parse(txtTaxYear.Text.Trim()) <= AppSettingsManager.GetCurrentDate().Year)
                    {
                        m_sNextQtrToPay = sQtrToPay;
                        chk1st.Checked = false;
                        chk2nd.Checked = false;
                        chk3rd.Checked = false;
                        chk4th.Checked = false;
                        chkFull.Checked = true;
                        chkInstallment.Checked = false;
                        chkFull_Click(chkFull, null); //DisplayDuesNew(sBin, "F", "", "", "");
                    }
                    else
                    {
                        if (int.Parse(txtTaxYear.Text.Trim()) < AppSettingsManager.GetCurrentDate().Year)
                        {
                            m_sNextQtrToPay = sQtrToPay;
                            chk1st.Checked = false;
                            chk2nd.Checked = false;
                            chk3rd.Checked = false;
                            chk4th.Checked = false;
                            chkFull.Checked = true;
                            chkInstallment.Checked = false;
                            chkFull_Click(chkFull, null); //DisplayDuesNew(sBin, "F", "", "", "");
                            m_sPaymentTerm = "F";
                        }
                        else
                        {
                            if (txtBnsStat.Text != "NEW" && sQtrToPay != "1")
                            {
                                m_sNextQtrToPay = sQtrToPay;
                                chkFull.Checked = false;
                                chkInstallment.Checked = true;
                                chkInstallment_Click(chkInstallment, null); //OnInstallmentNew(sBin);
                            }
                            else
                            {
                                chk1st.Checked = false;
                                chk2nd.Checked = false;
                                chk3rd.Checked = false;
                                chk4th.Checked = false;
                                chkFull.Checked = true;
                                chkInstallment.Checked = false;
                                chkFull_Click(chkFull, null); //OnFull(); // GDE 20110412 added for testing
                            }
                        }
                    }

                }
            }
            result.Close();

            // GDE 20110414 added for testing (s){
            if (bEntered == false)
            {
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'P' and tax_year = '" + txtTaxYear.Text.Trim() + "' order by tax_year desc ,qtr_to_pay desc ,due_state desc";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sQtrToPay = result.GetString("qtr_to_pay").Trim();
                        if (sQtrToPay != "1" && m_iMultiTaxYear != 1)
                        {
                            m_sNextQtrToPay = sQtrToPay;
                            chkInstallment_Click(chkInstallment, null); //OnInstallmentNew(sBin);
                        }
                        else
                            if (sQtrToPay != "1" && m_iNextQtrToPay != 1)
                            {
                                result2.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'A'  and bin in (select bin from taxdues where due_state = 'X')";
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        m_sNextQtrToPay = sQtrToPay;
                                        chkInstallment_Click(chkInstallment, null); //OnInstallmentNew(sBin);
                                    }
                                }
                                result2.Close();
                            }
                            else
                            {
                                m_sNextQtrToPay = sQtrToPay;
                                chk1st.Checked = false;
                                chk2nd.Checked = false;
                                chk3rd.Checked = false;
                                chk4th.Checked = false;
                                chkFull.Checked = true;
                                chkInstallment.Checked = false;
                                chkFull_Click(chkFull, null); //DisplayDuesNew(sBin, "F", "", "", "");
                            }

                        if (m_iMultiTaxYear != 1 && int.Parse(txtTaxYear.Text.Trim()) <= AppSettingsManager.GetCurrentDate().Year)
                        {
                            m_sNextQtrToPay = sQtrToPay;
                            chk1st.Checked = false;
                            chk2nd.Checked = false;
                            chk3rd.Checked = false;
                            chk4th.Checked = false;
                            chkFull.Checked = true;
                            chkInstallment.Checked = false;
                            chkFull_Click(chkFull, null); //DisplayDuesNew(sBin, "F", "", "", "");
                        }
                        else
                        {
                            if (int.Parse(txtTaxYear.Text.Trim()) < AppSettingsManager.GetCurrentDate().Year)
                            {
                                m_sNextQtrToPay = sQtrToPay;
                                chk1st.Checked = false;
                                chk2nd.Checked = false;
                                chk3rd.Checked = false;
                                chk4th.Checked = false;
                                chkFull.Checked = true;
                                chkInstallment.Checked = false;
                                chkFull_Click(chkFull, null); //DisplayDuesNew(sBin, "F", "", "", "");
                                m_sPaymentTerm = "F";
                            }
                            else
                            {
                                if (txtBnsStat.Text != "NEW" && sQtrToPay != "1")
                                {
                                    m_sNextQtrToPay = sQtrToPay;
                                    chkFull.Checked = false;
                                    chkInstallment.Checked = true;
                                    chkInstallment_Click(chkInstallment, null); //OnInstallmentNew(sBin);
                                }
                                else
                                {
                                    chk1st.Checked = false;
                                    chk2nd.Checked = false;
                                    chk3rd.Checked = false;
                                    chk4th.Checked = false;
                                    chkFull.Checked = true;
                                    chkInstallment.Checked = false;
                                    chkFull_Click(chkFull, null); //OnFull(); // GDE 20110412 added for testing
                                }
                            }
                        }
                    }
                }
                result.Close();
            }
            #endregion

            // GDE 20110414 added for testing (e)}
        } //MOD MCR 20140227/20140722

        //MCR 20140723 (s)
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
            mv_cdtORDate = dtpORDate.Value;

            bool boRetStatus = false;
            pSet.Query = "select max(tax_year) as max_yr from pay_hist where bin = '" + m_sBIN + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sMaxYear = pSet.GetString("max_yr");
            pSet.Close();

            if (sDueState == "X")
            {
                pSet.Query = "select * from retired_bns_temp where bin ='" + m_sBIN + "' and tax_year = '" + sMaxYear + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        boRetStatus = true;
                pSet.Close();
            }

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
            for (int dYear = Convert.ToInt32(sTaxYear); dYear <= Convert.ToInt32(m_sCurrentTaxYear); dYear++)
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

                if (mv_cdtORDate > Jan || m_sORDate == sJan || (((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("01/20/" + sBaseYear) || m_sORDate == ("01/20/" + sBaseYear)) && mv_cdtORDate > Dec) || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("01/20/" + sBaseYear) || m_sORDate == ("01/20/" + sBaseYear)) && Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")))))
                {

                    if (m_sORDate == sJan || (mv_cdtORDate < Jan && mv_cdtORDate.Month == Jan.Month && mv_cdtORDate.Year == Jan.Year))
                    {

                        if (sBaseYear != sTaxYear)
                        {

                        }
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
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
                    if (m_sORDate == sFeb || (mv_cdtORDate < Feb && mv_cdtORDate.Month == Feb.Month && mv_cdtORDate.Year == Feb.Year))
                    {

                        if (sBaseYear != sTaxYear)
                        {

                        }
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
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
                    if (m_sORDate == sMar || (mv_cdtORDate < Mar && mv_cdtORDate.Month == Mar.Month && mv_cdtORDate.Year == Mar.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
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

                if (mv_cdtORDate > Apr || m_sORDate == sApr || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("04/20/" + sBaseYear) || m_sORDate == ("04/20/" + sBaseYear)) && Convert.ToInt32(sQtrPaid) <= 1 && Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))))
                {
                    if (m_sORDate == sApr || (mv_cdtORDate < Apr && mv_cdtORDate.Month == Apr.Month && mv_cdtORDate.Year == Apr.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate2 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;

                            if ((IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "1") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate2 = dPenRate1;

                            if (sBaseYear != sTaxYear)
                            {
                                dPenRate3 += m_dPenRate;
                                dPenRate4 += m_dPenRate;
                            }
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
                    if (m_sORDate == sMay || (mv_cdtORDate < May && mv_cdtORDate.Month == May.Month && mv_cdtORDate.Year == May.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            if ((IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "1") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate2 = dPenRate1;
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
                    if (m_sORDate == sJun || (mv_cdtORDate < Jun && mv_cdtORDate.Month == Jun.Month && mv_cdtORDate.Year == Jun.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            if ((IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "1") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate2 = dPenRate1;
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

                if (mv_cdtORDate > Jul || m_sORDate == sJul || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("07/20/" + sBaseYear) || m_sORDate == ("07/20/" + sBaseYear)) && Convert.ToInt32(sQtrPaid) <= 2 && Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))))
                {
                    if (m_sORDate == sJul || (mv_cdtORDate < Jul && mv_cdtORDate.Month == Jul.Month && mv_cdtORDate.Year == Jul.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate3 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;

                            if ((IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "2") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate3 = dPenRate2;

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
                    if (m_sORDate == sAug || (mv_cdtORDate < Aug && mv_cdtORDate.Month == Aug.Month && mv_cdtORDate.Year == Aug.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            if ((IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "2") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate3 = dPenRate2;
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
                    if (m_sORDate == sSep || (mv_cdtORDate < Sep && mv_cdtORDate.Month == Sep.Month && mv_cdtORDate.Year == Sep.Year))
                    {

                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;

                            if ((IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "2") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate3 = dPenRate2;

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


                if (mv_cdtORDate > Oct || m_sORDate == sOct || ((Convert.ToDateTime(m_sORDate) > Convert.ToDateTime("10/20/" + sBaseYear) || m_sORDate == ("10/20/" + sBaseYear)) && Convert.ToInt32(sQtrPaid) <= 3 && Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))))
                {
                    if (m_sORDate == sOct || (mv_cdtORDate < Oct && mv_cdtORDate.Month == Oct.Month && mv_cdtORDate.Year == Oct.Year))
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dSurchRate4 = m_dSurchRate;
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                            if ((IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "3") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate4 = dPenRate3;
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
                    if (m_sORDate == sNov || (mv_cdtORDate < Nov && mv_cdtORDate.Month == Nov.Month && mv_cdtORDate.Year == Nov.Year))
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                            if ((IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "3") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate4 = dPenRate3;
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
                    if (m_sORDate == sDec || (mv_cdtORDate < Dec && mv_cdtORDate.Month == Dec.Month && mv_cdtORDate.Year == Dec.Year))
                    {
                    }
                    else
                    {
                        if (sDueState == "X" || sDueState == "R" || (sDueState == "X" && (Convert.ToInt32(sTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")) || Convert.ToInt32(sMaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12"))) && boRetStatus == false) || sDueState == "A")
                        {
                            dPenRate1 += m_dPenRate;
                            dPenRate2 += m_dPenRate;
                            dPenRate3 += m_dPenRate;
                            dPenRate4 += m_dPenRate;
                            if ((IsPaidOnLastQtr(m_sBIN, sTaxYear, "", "3") == true) && (AppSettingsManager.GetConfigValue("51") == "2"))
                                dPenRate4 = dPenRate3;
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

        private void SaveToPayTemp(String p_sBin, String p_sTaxYear, String p_sTerm, String p_sQtr, String p_sFeesDesc, String p_sFeesDue, String p_sFeesSurch, String p_sFeesPen, String p_sFeesTotal, String p_sFeesCode, String p_sDueState, String p_sQtrToPay, String p_sFeesSort)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            sQuery = "insert into pay_temp(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,fees_code_sort) values ";
            sQuery += "('" + StringUtilities.HandleApostrophe(p_sBin) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sTaxYear) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sTerm) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sQtr) + "', ";
            sQuery += " '" + StringUtilities.Left(StringUtilities.HandleApostrophe(p_sFeesDesc), 40) + "', "; // ALJ 11202003 fixed SQL temporary solution
            sQuery += p_sFeesDue + ", ";
            sQuery += p_sFeesSurch + ", ";
            sQuery += p_sFeesPen + ", ";
            sQuery += p_sFeesTotal + ", ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sFeesCode) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sDueState) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sQtrToPay) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sFeesSort) + "')";
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

        private void SaveToPayTempBns(String p_sBin, String p_sTaxYear, String p_sTerm, String p_sQtr, String p_sFeesDesc, String p_sFeesDue, String p_sFeesSurch, String p_sFeesPen, String p_sFeesTotal, String p_sFeesCode, String p_sDueState, String p_sQtrToPay, String p_sFeesSort, String p_sBnsCodeMain)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery = "";
            sQuery = "insert into pay_temp_bns(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay,fees_code_sort,bns_code_main) values ";
            sQuery += "('" + StringUtilities.HandleApostrophe(p_sBin) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sTaxYear) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sTerm) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sQtr) + "', ";
            sQuery += " '" + StringUtilities.Left(StringUtilities.HandleApostrophe(p_sFeesDesc), 40) + "', ";
            sQuery += p_sFeesDue + ", ";
            sQuery += p_sFeesSurch + ", ";
            sQuery += p_sFeesPen + ", ";
            sQuery += p_sFeesTotal + ", ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sFeesCode) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sDueState) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sQtrToPay) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sFeesSort) + "', ";
            sQuery += " '" + StringUtilities.HandleApostrophe(p_sBnsCodeMain) + "')";
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();
        }

        private string FeesPen(String sTaxYear, String sTempPen, String sQtr)
        {
            String sQuery, sPen, sPenTemp;
            OracleResultSet pSet = new OracleResultSet();

            sPen = sTempPen;
            if (sQtr == "2")
            {
                pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '2' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sPen = "0.00";
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '1' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                pSet.Close();
                                pSet.Query = "select sum(fees_pen) as fees_pen from pay_temp where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and qtr = '1' and term = 'I'";
                                if (pSet.Execute())
                                    if (pSet.Read())
                                    {
                                        double fPenTemp = 0;
                                        sPenTemp = pSet.GetDouble("fees_pen").ToString();
                                        fPenTemp = pSet.GetDouble("fees_pen");
                                        if (fPenTemp >= 0)
                                            sPen = "0.00";
                                    }
                            }
                    }
                pSet.Close();
            }

            if (sQtr == "3")
            {
                pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '3' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sPen = "0.00";
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '2' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                pSet.Close();
                                pSet.Query = "select sum(fees_pen) as fees_pen from pay_temp where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and qtr = '2' and term = 'I'";
                                if (pSet.Execute())
                                    if (pSet.Read())
                                    {
                                        double fPenTemp = 0;
                                        sPenTemp = pSet.GetDouble("fees_pen").ToString();
                                        fPenTemp = pSet.GetDouble("fees_pen");
                                        if (fPenTemp >= 0)
                                            sPen = "0.00";
                                    }
                            }
                    }
                pSet.Close();
            }

            if (sQtr == "4")
            {
                pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '4' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sPen = "0.00";
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '3' and payment_term = 'I' and bin = '" + m_sBIN + "'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                pSet.Close();
                                pSet.Query = sQuery = "select sum(fees_pen) as fees_pen from pay_temp where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and qtr = '3' and term = 'I'";
                                if (pSet.Execute())
                                    if (pSet.Read())
                                    {
                                        double fPenTemp = 0;
                                        sPenTemp = pSet.GetDouble("fees_pen").ToString();
                                        fPenTemp = pSet.GetDouble("fees_pen");
                                        if (fPenTemp >= 0)
                                            sPen = "0.00";
                                    }
                            }
                    }
                pSet.Close();
            }
            return sPen;
        }

        private string FeesSurch(String sTaxYear, String sDueTemp, double dSurch, string sQtr, string sTaxCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sSurch, sSurchTemp, sLastQtrpaid = "";

            sSurch = MathUtilities.RoundValue(Convert.ToDouble(Convert.ToDouble(sDueTemp) * dSurch), 2, MidpointRounding.AwayFromZero).ToString();

            pSet.Query = "select max(qtr_paid) as qtr_paid from pay_hist where tax_year = '" + sTaxYear + "' and payment_term = 'I' and bin = '" + m_sBIN + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sLastQtrpaid = pSet.GetString("qtr_paid");
            pSet.Close();

            if (sQtr == "2")
            {
                pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '2' and payment_term = 'I' and bin = '" + m_sBIN + "' and data_mode != 'POS'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sSurch = "0.00";
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid > '" + sLastQtrpaid + "' and payment_term = 'I' and bin = '" + m_sBIN + "' and data_mode = 'POS'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                pSet.Close();
                                pSet.Query = "select sum(fees_surch) as fees_surch from pay_temp where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and qtr > '" + sLastQtrpaid + "' and term = 'I' and fees_code like '" + sTaxCode + "%'";
                                if (pSet.Execute())
                                    if (pSet.Read())
                                    {
                                        double fSurch = 0;
                                        sSurchTemp = pSet.GetDouble("fees_surch").ToString();
                                        fSurch = pSet.GetDouble("fees_surch");
                                        if (fSurch > 0)
                                            sSurch = "0.00";

                                        if (sSurch != "0.00")
                                        {
                                            pSet.Close();
                                            pSet.Query = "select sum(fees_surch) as fees_surch from or_table where or_no in ( select or_no from pay_hist where bin = '" + m_sBIN + "'  and tax_year = '" + sTaxYear + "') and tax_year = '" + sTaxYear + "'";
                                            if (pSet.Execute())
                                                if (pSet.Read())
                                                {
                                                    fSurch = 0;
                                                    sSurchTemp = pSet.GetDouble("fees_surch").ToString();
                                                    fSurch = pSet.GetDouble("fees_surch");

                                                    if (fSurch > 0)
                                                        sSurch = "0.00";
                                                }
                                        }
                                    }
                            }

                    }
                pSet.Close();
            }

            if (sQtr == "3")
            {
                pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '3' and payment_term = 'I' and bin = '" + m_sBIN + "' and data_mode != 'POS'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sSurch = "0.00";
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid > '" + sLastQtrpaid + "' and payment_term = 'I' and bin = '" + m_sBIN + "' and data_mode = 'POS'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                pSet.Close();
                                pSet.Query = "select sum(fees_surch) as fees_surch from pay_temp where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and qtr > '" + sLastQtrpaid + "' and term = 'I' and fees_code like '" + sTaxCode + "%'";
                                if (pSet.Execute())
                                    if (pSet.Read())
                                    {
                                        double fSurch = 0;
                                        sSurchTemp = pSet.GetDouble("fees_surch").ToString();
                                        fSurch = pSet.GetDouble("fees_surch");
                                        if (fSurch > 0)
                                            sSurch = "0.00";

                                        if (sSurch != "0.00")
                                        {
                                            pSet.Close();
                                            pSet.Query = "select sum(fees_surch) as fees_surch from or_table where or_no in ( select or_no from pay_hist where bin = '" + m_sBIN + "'  and tax_year = '" + sTaxYear + "') and tax_year = '" + sTaxYear + "'"; // CJC 20121127
                                            if (pSet.Execute())
                                                if (pSet.Read())
                                                {
                                                    fSurch = 0;
                                                    sSurchTemp = pSet.GetDouble("fees_surch").ToString();
                                                    fSurch = pSet.GetDouble("fees_surch");
                                                    if (fSurch > 0)
                                                        sSurch = "0.00";
                                                }
                                        }
                                    }
                            }
                    }
                pSet.Close();
            }

            if (sQtr == "4")
            {
                pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid = '4' and payment_term = 'I' and bin = '" + m_sBIN + "' and data_mode != 'POS'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sSurch = "0.00";
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select * from pay_hist where tax_year = '" + sTaxYear + "' and qtr_paid > '" + sLastQtrpaid + "' and payment_term = 'I' and bin = '" + m_sBIN + "' and data_mode = 'POS'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                pSet.Close();
                                pSet.Query = "select sum(fees_surch) as fees_surch from pay_temp where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "' and qtr > '" + sLastQtrpaid + "' and term = 'I' and fees_code like '" + sTaxCode + "%'";
                                if (pSet.Execute())
                                    if (pSet.Read())
                                    {
                                        double fSurch = 0;
                                        sSurchTemp = pSet.GetDouble("fees_surch").ToString();
                                        fSurch = pSet.GetDouble("fees_surch");
                                        if (fSurch > 0)
                                            sSurch = "0.00";

                                        if (sSurch != "0.00")
                                        {
                                            pSet.Close();
                                            pSet.Query = "select sum(fees_surch) as fees_surch from or_table where or_no in ( select or_no from pay_hist where bin = '" + m_sBIN + "'  and tax_year = '" + sTaxYear + "') and tax_year = '" + sTaxYear + "'";
                                            if (pSet.Execute())
                                                if (pSet.Read())
                                                {
                                                    fSurch = 0;
                                                    sSurchTemp = pSet.GetDouble("fees_surch").ToString();
                                                    fSurch = pSet.GetDouble("fees_surch");

                                                    if (fSurch > 0)
                                                        sSurch = "0.00";
                                                }
                                        }
                                    }
                            }
                    }

            }
            return sSurch;
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

            pSet.Close();
            return false;
        }

        private void OnCash()
        {
            bool bCashAnsOrig;
            String sQuery;
            bCashAnsOrig = chkCash.Checked;

            if (chkCash.Checked == true)
            {
                if ((chkTCredit.Checked == true || chkTCredit.Checked == false) && chkCheque.Checked == true)
                {
                    MessageBox.Show("Tag cash payment type first,if u want to use \ncash/check or cash/check/tax credit payment type", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    chkCash.Checked = false;
                    return;
                }
            }

            CheckPaymentType();

            if (chkTCredit.Checked == true && chkCash.Checked == true)
                txtTotDues.Text = txtGrandTotal.Text;
            else
                txtTotDues.Text = "0.00";

            if (chkCash.Checked == false && (chkTCredit.Checked == true || chkCheque.Checked == true))
                txtTotDues.Text = "0.00";

            if (bCashAnsOrig == true && chkCheque.Checked == true)
            {
                OracleResultSet pSet = new OracleResultSet();
                sQuery = "delete from chk_tbl_temp where or_no = '" + txtORNo.Text.Trim() + "' and cash_amt <> 0.00";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
            }
        }

        private void OnCheck()
        {
            CheckPaymentType();

            if (chkCheque.Checked == true)
            {
                btnChequeInfo.Visible = true;
                btnChequeInfo.PerformClick();
                if (bGuarantor == true)
                    chkCash.Enabled = false;
            }
            else
            {
                String sQuery;
                OracleResultSet pSet = new OracleResultSet();
                OracleResultSet pSet1 = new OracleResultSet();
                pSet.Query = "select * from chk_tbl_temp where or_no = '" + txtORNo.Text.Trim() + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        String sChkNo;
                        pSet.Close();

                        pSet.Query = "select distinct chk_no from chk_tbl_temp where or_no = '" + txtORNo.Text.Trim() + "'";
                        if (pSet.Execute())
                        {
                            while (pSet.Read())
                            {
                                sChkNo = pSet.GetString("chk_no");

                                pSet1.Query = "select * from multi_check_pay where used_sw = 'N' and chk_no = '" + sChkNo + "'";
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                        sQuery = "delete from multi_check_pay where used_sw = 'N' and chk_no = '" + sChkNo + "'";
                                pSet1.Close();
                            }
                        }
                        pSet.Close();

                        sQuery = "delete from chk_tbl_temp where rtrim(or_no) = '" + txtORNo.Text + "'";
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                    }

                btnChequeInfo.Visible = false;
                chkCash.Enabled = true;
                m_sToBeDebited = "0.00";
            }
        }
        //MCR 20140723 (e)

        private string GetCurrentOR(string sTellerCode)
        {
            int iCurrOr = 0;
            string sCurrentOr = string.Empty;
            string sCurrentOrCode = string.Empty;
            string sORSeries = string.Empty;
            string sCurrOr = string.Empty;  // RMC 20150501 QA BTAS
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from or_current where teller = '" + sTellerCode.Trim() + "'";
            if (result.Execute())
                if (result.Read())
                {
                    //iCurrOr = result.GetInt("cur_or_no");
                    //sCurrOr = result.GetInt("cur_or_no").ToString(); //JARS 20170713
                    //sCurrOr = result.GetInt("cur_or_no").ToString("0000000"); // JAV 20170717

                    sCurrOr = result.GetString("cur_or_no"); //JARS 20170726 FINAL ADJUSTMENTS, ALTERED TABLE OR_CURRENT

                    //sCurrentOrCode = result.GetString("or_code");
                    // RMC 20141127 QA Tarlac ver (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                    {
                        sORSeries = result.GetString("or_series"); //MCR 20140826
                        sCurrentOrCode = result.GetString("or_code");   // RMC 20150501 QA BTAS
                    }
                    // RMC 20141127 QA Tarlac ver (e)
                }
            result.Close();
            /*
            sCurrentOr = string.Format("{0:0###0}", sCurrentOr.Trim());
            sCurrentOr = sCurrentOrCode + "-" sCurrentOr;
            */
            // RMC 20141127 QA Tarlac ver (s)
            if (AppSettingsManager.GetConfigValue("56") == "Y")
            //    sCurrentOr = iCurrOr.ToString() + sORSeries;
                sCurrentOr = sCurrOr + sORSeries;   // RMC 20150501 QA BTAS
            else // RMC 20141127 QA Tarlac ver (e)
                //sCurrentOr = sORSeries + iCurrOr.ToString();
                sCurrentOr = sORSeries + sCurrOr;   // RMC 20150501 QA BTAS
            
            return sCurrentOr;
        }

        private void LoadBills() //MCR 20140722
        {
            String sQuery, sFeesCode, sFeesDesc = "", sFees, sTransAppCode, sTransCount;
            double dFeesDue, dFeesSurch, dFeesPen, dFeesAmtDue;
            int iTransCount = 0;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            pSet.Query = "Select * from trans_fees_table where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sFeesCode = pSet.GetString("fees_code");
                    dFeesDue = pSet.GetDouble("fees_due");
                    dFeesSurch = pSet.GetDouble("fees_surch");
                    dFeesPen = pSet.GetDouble("fees_pen");
                    dFeesAmtDue = pSet.GetDouble("fees_amtdue");
                    sTransAppCode = pSet.GetString("trans_app_code");

                    pSet1.Query = string.Format("select fees_desc from tax_and_fees_table where fees_code = '{0}'", sFeesCode);
                    if (pSet1.Execute())
                        if (pSet1.Read())
                            sFeesDesc = pSet.GetString("fees_desc");
                    pSet1.Close();

                    sFees = sTransAppCode + "-" + sFeesDesc;

                    pSet1.Query = string.Format("select count(*) as trans_count from pay_hist where bin = '{0}' and tax_year = '{1}' and qtr_paid like 'T%%'", m_sBIN, txtTaxYear.Text);
                    int.TryParse(pSet1.ExecuteScalar(), out iTransCount);
                    iTransCount++;
                    sTransCount = iTransCount.ToString();
                    pSet1.Close();

                    sTransCount = "T" + sTransCount;

                    dgvTaxFees.Rows.Add(txtTaxYear.Text.Trim(), "F", sTransCount, sFees, dFeesDue.ToString("##0.00"), dFeesSurch.ToString("##0.00"), dFeesPen.ToString("##0.00"), dFeesAmtDue.ToString("##0.00"), "", "", "");
                }
            }
            pSet.Close();

            ComputeTotal();
            btnChequeInfo.Visible = true;
        }

        private void OnInstallment(string sBin)
        {
            chkFull.Checked = false;
            chkInstallment.Checked = true;
            m_sPaymentTerm = "I";
            //if (m_sNextQtrToPay.Trim() == string.Empty)
            if (m_sNextQtrToPay.Trim() == string.Empty || m_sNextQtrToPay == "0")   // RMC 20160113 corrections in SOA error if Installemnt
                m_sNextQtrToPay = "1";

            chk1st.Checked = true;

            int iNextQtrToPay = 0;
            int.TryParse(m_sNextQtrToPay, out iNextQtrToPay);

            if (iNextQtrToPay > 1)
            {
                chk1st.Enabled = false;
                chk1st.Checked = false;
            }
            if (iNextQtrToPay > 2)
            {
                chk2nd.Enabled = false;
                chk2nd.Checked = false;
            }
            if (iNextQtrToPay > 3)
            {
                chk3rd.Enabled = false;
                chk3rd.Checked = false;
            }

            if (m_sNextQtrToPay == "1")
                chk1st.Checked = true;
            if (m_sNextQtrToPay == "2")
                chk2nd.Checked = true;
            if (m_sNextQtrToPay == "3")
                chk3rd.Checked = true;
            if (m_sNextQtrToPay == "4")
                chk4th.Checked = true;
            m_bExistTaxPU = false;

            DisplayDuesNew(sBin, m_sNextQtrToPay, "", "", "");
        }

        private void OnFull()
        {
            if (chkAdj.Checked)
                OnCheckAdj();
            SetCheckTerm(chkFull);

            m_sPaymentTerm = "F";
            String sTmpTaxCredit = txtToTaxCredit.Text;
            dgvTaxFees.Rows.Clear();
            txtToTaxCredit.Text = sTmpTaxCredit;

            if (bTaxDueFound == false)
                txtCompromise.Text = "0.00";

            txtCompromise.Text = "";

            if (m_bIsCompromise == true)
            {
                m_bIsDisCompromise = true;
                txtCompromise.Enabled = true;
            }
            else
            {
                m_bIsDisCompromise = false;
                txtCompromise.Enabled = false;
            }

            m_sNextQtrToPay = "1";
            m_bExistTaxPU = false;

            if (m_sPaymentMode == "ONL" || m_sPaymentMode == "OFL")
            {

                //DisplayDuesNew(m_sBIN.Trim(), "F", "", "", "");
                // RMC 20150112 mods in retirement billing (s)
                if (chkAdj.Checked == false)
                {
                    DisplayDuesNew(m_sBIN.Trim(), "F", "", "", "");  // RMC 20170228 correction in SOA of RET
                    if (txtBnsStat.Text.Trim() == "RET")
                        DisplayDuesNew(m_sBIN.Trim(), "X", "", "", "");
                    /*else 
                        DisplayDuesNew(m_sBIN.Trim(), "F", "", "", "");*/
                    // RMC 20170228 correction in SOA of RET, put rem
                }
                else
                {
                    DisplayDuesNew(m_sBIN.Trim(), "A", "", "", "");
                    DisplayDuesNew(m_sBIN.Trim(), "F", "", "", "");
                }
                // RMC 20150112 mods in retirement billing (e)
            }

            chkCash.Checked = false;
            chkCheque.Checked = false;

            m_bDiscQtr = false;

            if (m_sNewWithQtr == "Y" && m_sNewWithQtr != "4")
                chkInstallment.Enabled = true;

            if (m_sPaymentMode == "OFL")
            {
                //OFLPaymentCondition(); MCR 20140902 REM This Code Doesn't Exist on Antipolo Version
            }
        }

        //MCR 20140722 (s)
        private void OnInstallmentNew(string sBin)
        {
            SetCheckTerm(chkInstallment);
            m_sPaymentTerm = "I";

            String sTmpTaxCredit = txtToTaxCredit.Text;
            dgvTaxFees.Rows.Clear();
            txtToTaxCredit.Text = sTmpTaxCredit;

            OracleResultSet pSet = new OracleResultSet();
            DateTime sDueJan = new DateTime(), sDueFeb, sDueMar, sDueApr = new DateTime(), sDueMay, sDueJun, sDueJul = new DateTime(), sDueAug, sDueSep, sDueOct = new DateTime(), sDueNov, sDueDec;
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

            DateTime sDateNow = dtpORDate.Value;
            String sQtr1 = "";
            String sQtr2 = "";
            String sQtr3 = "";
            String sQtr4 = "";
            /*int sTaxYear = 0;
            sTaxYear = dtpORDate.Value.Year;*/

            // RMC 20160113 corrections in SOA error if Installemnt (s)
            string sTaxYear = "";
            sTaxYear = string.Format("{0:####}",dtpORDate.Value.Year);
            if(sTaxYear == "")
                sTaxYear = txtTaxYear.Text;
            // RMC 20160113 corrections in SOA error if Installemnt (e)

            // RMC 20141217 adjustments, change all convert.ToInt32 to toint32

            try
            {
                //if (Convert.ToInt32(m_sNextQtrToPay) < 4 || Convert.ToInt32(txtTaxYear.Text) < sTaxYear)
                //if (Convert.ToInt32(m_sNextQtrToPay) < 4 || Convert.ToInt32(txtTaxYear.Text) < Convert.ToInt32(sTaxYear))   // RMC 20141217 adjustments
                if (Convert.ToInt32(m_sNextQtrToPay) < 4 && Convert.ToInt32(txtTaxYear.Text) < Convert.ToInt32(sTaxYear))   // RMC 20180125 set default next qtr to pay for installment
                    m_sNextQtrToPay = "";  
            }
            catch
            {
                m_sNextQtrToPay = "";  
            }

            if (m_sNextQtrToPay == "")
            {
                //if (sDateNow >= sDueJan || Convert.ToInt32(txtTaxYear.Text) < sTaxYear)
                if (sDateNow >= sDueJan || Convert.ToInt32(txtTaxYear.Text) < Convert.ToInt32(sTaxYear))    // RMC 20141217 adjustments
                {
                    m_sNextQtrToPay = "1";
                    sQtr1 = "1";
                }
                //if (sDateNow >= sDueApr || Convert.ToInt32(txtTaxYear.Text) < sTaxYear)
                if (sDateNow >= sDueApr || Convert.ToInt32(txtTaxYear.Text) < Convert.ToInt32(sTaxYear))    // RMC 20141217 adjustments
                {
                    m_sNextQtrToPay = "2";
                    sQtr2 = "2";
                }
                //if (sDateNow >= sDueJul || Convert.ToInt32(txtTaxYear.Text) < sTaxYear)
                if (sDateNow >= sDueJul || Convert.ToInt32(txtTaxYear.Text) < Convert.ToInt32(sTaxYear))    // RMC 20141217 adjustments
                {
                    m_sNextQtrToPay = "3";
                    sQtr3 = "3";
                }
                //if (sDateNow >= sDueOct || Convert.ToInt32(txtTaxYear.Text) < sTaxYear)
                if (sDateNow >= sDueOct || Convert.ToInt32(txtTaxYear.Text) < Convert.ToInt32(sTaxYear))    // RMC 20141217 adjustments
                {
                    m_sNextQtrToPay = "4";
                    sQtr4 = "4";
                }
            }

            chk1st.Checked = true;
            chk2nd.Checked = true;
            chk3rd.Checked = true;
            chk4th.Checked = true;

            chk1st.Enabled = true;
            chk2nd.Enabled = true;
            chk3rd.Enabled = true;
            chk4th.Enabled = true;

            // RMC 20141217 adjustments (s)
            if (m_sNextQtrToPay == "")
                m_sNextQtrToPay = "1";
            // RMC 20141217 adjustments (e)

            string sCurrDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());    // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin

            if (Convert.ToInt32(m_sNextQtrToPay) >= 1)
            {
                chk1st.Enabled = false;
                chk1st.Checked = false;

                // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin (s)
                if (AppSettingsManager.GetQtr(sCurrDate) == "1")
                {
                    chk1st.Enabled = true;
                    chk1st.Checked = true;
                }
                // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin (e)
            }
            if (Convert.ToInt32(m_sNextQtrToPay) >= 2)
            {
                chk2nd.Enabled = false;
                chk2nd.Checked = false;

                // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin (s)
                if (AppSettingsManager.GetQtr(sCurrDate) == "2")
                {
                    chk2nd.Enabled = true;
                    chk2nd.Checked = true;
                }
                // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin (e)
            }
            if (Convert.ToInt32(m_sNextQtrToPay) >= 3)
            {
                chk3rd.Enabled = false;
                chk3rd.Checked = false;

                // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin (s)
                if (AppSettingsManager.GetQtr(sCurrDate) == "3")
                {
                    chk3rd.Enabled = true;
                    chk3rd.Checked = true;
                }
                // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin (e)
            }
            if (Convert.ToInt32(m_sNextQtrToPay) == 4)
            {
                chk4th.Enabled = false;
                chk4th.Checked = false;

                // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin (s)
                if (AppSettingsManager.GetQtr(sCurrDate) == "4")
                {
                    chk4th.Enabled = true;
                    chk4th.Checked = true;
                }
                // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin (e)
            }

            if (m_sNextQtrToPay == "1")
            {
                chk1st.Checked = true;
                chk2nd.Checked = false;
                chk3rd.Checked = false;
                chk4th.Checked = false;
                sQtr1 = "1";
            }
            if (m_sNextQtrToPay == "2")
            {
                chk1st.Checked = true;
                chk2nd.Checked = true;
                chk3rd.Checked = false;
                chk4th.Checked = false;
                sQtr1 = "1"; sQtr2 = "2";
            }
            if (m_sNextQtrToPay == "3")
            {
                chk1st.Checked = true;
                chk2nd.Checked = true;
                chk3rd.Checked = true;
                chk4th.Checked = false;
                sQtr1 = "1"; sQtr2 = "2"; sQtr3 = "3";
            }
            if (m_sNextQtrToPay == "4")
            {
                chk1st.Checked = true;
                chk2nd.Checked = true;
                chk3rd.Checked = true;
                chk4th.Checked = true;
                sQtr1 = "1"; sQtr2 = "2"; sQtr3 = "3"; sQtr4 = "4";
            }

            //if (bTaxDueFound == false)
            //{
            //    chk1st.Enabled = false;
            //    chk2nd.Enabled = false;
            //    chk3rd.Enabled = false;
            //    chk4th.Enabled = false;

            //    chk1st.Checked = false;
            //    chk2nd.Checked = false;
            //    chk3rd.Checked = false;
            //    chk4th.Checked = false;
            //}

            txtCompromise.Text = "";

            m_bIsDisCompromise = false;
            if (m_sCompPaySw != "N")
                txtCompromise.Enabled = false;
            else
                m_bIsDisCompromise = true;
            m_bExistTaxPU = false;

            if (m_sNextQtrToPay == "1")
                DisplayDuesNew(m_sBIN, "1", "", "", "");
            else if (m_sNextQtrToPay == "2")
                DisplayDuesNew(m_sBIN, sQtr1, sQtr2, "", "");
            else if (m_sNextQtrToPay == "3")
                DisplayDuesNew(m_sBIN, sQtr1, sQtr2, sQtr3, "");
            else if (m_sNextQtrToPay == "4")
                DisplayDuesNew(m_sBIN, sQtr1, sQtr2, sQtr3, sQtr4);
            else
                DisplayDuesNew(m_sBIN, sQtr1, sQtr2, sQtr3, sQtr4);

            chkCash.Checked = false;
            chkCheque.Checked = false;

            if (chkAdj.Checked)
                OnCheckAdj();

            if (m_bIsCompromise == true)
            {
                chkFull.Enabled = true;
                chkInstallment.Enabled = true;
            }

            if (m_sPaymentMode == "OFL")
            {
                //OFLPaymentCondition(); MCR 20140902 REM This Code Doesn't Exist on Antipolo Version
            }
        }

        private double GetCompromiseInterest(String p_sDateToPay, String p_sORDate, double p_dPenRate)
        {
            if (p_sDateToPay.Trim() == "")
            {
                MessageBox.Show("WARNING! Schedule of payments has not been set.\nInterest will not be computed in case of late payment.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            String sQuery;
            int dTaxYear, dCurrTaxYear;
            double dInterest = 0;

            String sJan, sFeb, sMar, sApr, sMay, sJun, sJul, sAug, sSep, sOct, sNov, sDec;
            DateTime Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec;
            DateTime odtDateToPay, odtORDate;

            odtDateToPay = Convert.ToDateTime(p_sDateToPay);
            dTaxYear = odtDateToPay.Year;

            odtORDate = dtpORDate.Value;
            dCurrTaxYear = odtORDate.Year;

            DateTime sDueJan = new DateTime(), sDueFeb = new DateTime(), sDueMar = new DateTime(), sDueApr = new DateTime(), sDueMay = new DateTime(), sDueJun = new DateTime(), sDueJul = new DateTime(), sDueAug = new DateTime(), sDueSep = new DateTime(), sDueOct = new DateTime(), sDueNov = new DateTime(), sDueDec = new DateTime();

            sQuery = string.Format("select * from  due_dates where due_year = '{0}'", AppSettingsManager.GetSystemDate().Year);
            pSet.Query = sQuery;
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

            String sBaseYear;
            for (int dYear = dTaxYear; dYear <= dCurrTaxYear; dYear++)
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

                if (odtORDate > Jan && odtDateToPay <= Jan)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > Feb && odtDateToPay <= Feb)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > Mar && odtDateToPay <= Mar)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > Apr && odtDateToPay <= Apr)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > May && odtDateToPay <= May)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > Jun && odtDateToPay <= Jun)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > Jul && odtDateToPay <= Jul)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > Aug && odtDateToPay <= Aug)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > Sep && odtDateToPay <= Sep)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > Oct && odtDateToPay <= Oct)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > Nov && odtDateToPay <= Nov)
                    dInterest = dInterest + p_dPenRate;
                if (odtORDate > Dec && odtDateToPay <= Dec)
                    dInterest = dInterest + p_dPenRate;
            }
            return dInterest;
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

                        m_dFeesAmt = m_dFeesDue + m_dFeesSurch + m_dFeesPen;

                        sQuery = string.Format("select dt_to_pay from comp_duedate where bin = '{0}' and no_to_pay = '{1}'", sBIN, m_iTermToPayComp);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                                m_sDateToPay = pSet.GetDateTime("dt_to_pay").ToString("MM/dd/yyyy");
                        pSet1.Close();

                        if (sPaySw == "N")
                        {
                            bInitialDP = false;
                            sDesc = "COMPROMISE AGREEMENT (INITIAL PAYMENT)";

                            while (pSet.Read())
                            {
                                String sFeesDue, sFeesSurch, sFeesPen;
                                sFeesDue = Convert.ToDouble(pSet.GetDouble("fdue") * 0.25).ToString("##0.00");
                                sFeesSurch = Convert.ToDouble(pSet.GetDouble("fsurch") * 0.25).ToString("##0.00");
                                sFeesPen = Convert.ToDouble(pSet.GetDouble("fpen") * 0.25).ToString("##0.00");

                                m_dFeesDue += pSet.GetDouble("fdue");
                                m_dFeesSurch += pSet.GetDouble("fsurch");
                                m_dFeesPen += pSet.GetDouble("fpen");
                                m_dFeesAmt = m_dFeesDue + m_dFeesSurch + m_dFeesPen;

                                dTmpDue += Convert.ToDouble(sFeesDue);
                                dTmpSurch += Convert.ToDouble(sFeesSurch);
                                dTmpPen += Convert.ToDouble(sFeesPen);

                                dRowTotal = Convert.ToDouble(sFeesDue) + Convert.ToDouble(sFeesSurch) + Convert.ToDouble(sFeesPen);
                                sRowCompInt = Convert.ToDouble(dRowTotal * GetCompromiseInterest(m_sDateToPay, dtpORDate.Text, m_dPenRate)).ToString();
                                dTmpRowCompPen = dTmpRowCompPen + Convert.ToDouble(sRowCompInt);
                            }
                            dTmpTotal = dTmpDue + dTmpSurch + dTmpPen;

                            dgvTaxFees.Rows.Add("", "F", "F", sDesc, dTmpDue.ToString("##0.00"), dTmpSurch.ToString("##0.00"), Convert.ToDouble(dTmpPen + Convert.ToDouble(m_sAddlCompPen)).ToString("##0.00"), Convert.ToDouble(dTmpTotal + Convert.ToDouble(m_sAddlCompPen)).ToString("##0.00"), "CD", "", "");

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

                                String sCompDesc = string.Format(" {0} of {1}", iTermToPay, iNoPaySeq);
                                sDesc += sCompDesc;

                                dTmpRowCompPen = 0;
                                while (pSet.Read())
                                {

                                    String sFeesDue, sFeesSurch, sFeesPen;
                                    sFeesDue = Convert.ToDouble(pSet.GetDouble("fdue") / Convert.ToDouble(iNoPaySeq)).ToString("##0.00");
                                    sFeesSurch = Convert.ToDouble(pSet.GetDouble("fsurch") / Convert.ToDouble(iNoPaySeq)).ToString("##0.00");
                                    sFeesPen = Convert.ToDouble(pSet.GetDouble("fpen") / Convert.ToDouble(iNoPaySeq)).ToString("##0.00");

                                    m_dFeesDue += Convert.ToDouble(sFeesDue);
                                    m_dFeesSurch += Convert.ToDouble(sFeesSurch);
                                    m_dFeesPen += Convert.ToDouble(sFeesPen);
                                    m_dFeesAmt = m_dFeesDue + m_dFeesSurch + m_dFeesPen;

                                    dRowTotal = Convert.ToDouble(sFeesDue) + Convert.ToDouble(sFeesSurch) + Convert.ToDouble(sFeesPen);
                                    sRowCompInt = Convert.ToDouble(dRowTotal * GetCompromiseInterest(m_sDateToPay, dtpORDate.Text, m_dPenRate)).ToString();
                                    dTmpRowCompPen = dTmpRowCompPen + Convert.ToDouble(sRowCompInt);
                                }
                            }
                            m_sAddlCompPen = dTmpRowCompPen.ToString();

                            dgvTaxFees.Rows.Add("", sPayType, sString, sDesc, m_dFeesDue.ToString("##0.00"), m_dFeesSurch.ToString("##0.00"), Convert.ToDouble(m_dFeesPen + Convert.ToDouble(m_sAddlCompPen)).ToString("##0.00"), Convert.ToDouble(m_dFeesAmt + Convert.ToDouble(m_sAddlCompPen)).ToString("##0.00"), "C", "", "");
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

                m_bIsCompromise = true;
            }
            else
                m_bIsCompromise = false;

            if (dgvTaxFees.RowCount == 1)
            {
                String sTmpDueState;
                sTmpDueState = dgvTaxFees.Rows[1].Cells[8].Value.ToString();
                if (sTmpDueState.Substring(0, 1) == "C")
                    chkInstallment.Enabled = false;
            }
        }

        private void DisplayDuesNew(string sBin, string sQtrToPay, string sQtrToPay2, string sQtrToPay3, string sQtrToPay4)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            String sQuery = "";
            String sRange1 = "", sRange2 = "";

            String sQtrPaid = "";

            if (sQtrToPay != "A" && sQtrToPay != "F" && m_iMultiTaxYear == 1 && sQtrToPay != "X") // MCR 20143005 include Adj
            {

                // AST 20160113 (s)
                //sQuery = "select qtr_paid from pay_hist where payment_term = 'I' and bin = '" + sBin + "' order by tax_year desc, qtr_paid desc";
                //sQuery = string.Format("select qtr_paid from pay_hist where payment_term = 'I' and bin = '{0}' and tax_year = '{1}' ", sBin, txtTaxYear.Text);
                //sQuery += "order by tax_year desc, qtr_paid desc";
                // AST 20160113 (e)

                //sQuery = "select qtr_paid from pay_hist where payment_term = 'I' and bin = '" + sBin + "' order by tax_year desc, qtr_paid desc";
                // RMC 20160113 corrections in SOA error if Installemnt (s)
                sQuery = "select qtr_paid from pay_hist where (payment_term = 'I' or payment_term <> 'I') and bin = '" + sBin + "' ";   // RMC 20180117 correction in SOA and Payment
                sQuery += " and (qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'Y' and qtr_paid <> 'X')";
                sQuery += " and tax_year >= (select max(tax_year) from pay_hist where bin = '" + sBin + "') order by tax_year desc, qtr_paid desc";
                // RMC 20160113 corrections in SOA error if Installemnt (e)

                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sQtrPaid = pSet.GetString("qtr_paid");
                        // RMC 20180117 correction in SOA and Payment (s)
                        if (sQtrPaid == "F")
                            sQtrPaid = "4";
                        // RMC 20180117 correction in SOA and Payment (e)
                        if (Convert.ToInt32(sQtrPaid) == 1)
                            sQtrToPay = "";
                        if (Convert.ToInt32(sQtrPaid) == 2)
                        {
                            sQtrToPay = "";
                            sQtrToPay2 = "";
                        }
                        if (Convert.ToInt32(sQtrPaid) == 3)
                        {
                            sQtrToPay = "";
                            sQtrToPay2 = "";
                            sQtrToPay3 = "";
                        }
                    }
                pSet.Close();
            }

            PayTempStruct p;
            p.due_state = "";
            p.qtr_to_pay = "";
            m_bExistTaxDue = false;

            if (sQtrToPay2 == "" && sQtrToPay3 == "" && sQtrToPay4 == "")
            {
                if (sQtrToPay == "F" && Convert.ToInt32(m_sNextQtrToPay) > 1 && txtBnsStat.Text != "NEW" && m_iMultiTaxYear == 1)
                    sQtrToPay = "";

                if (sQtrToPay == "A" || sQtrToPay == "X")
                {
                    if (sQtrToPay == "X")
                        //sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state = 'X' and qtr_to_pay = 'F' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);
                        //sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state = 'X' and (qtr_to_pay = 'F' or qtr_to_pay = '1') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);    // RMC 20150108
                        //sQuery = string.Format("select * from pay_temp where bin = '{0}' and ((due_state = 'X' and (qtr_to_pay = 'F' or qtr_to_pay = '1')) or (due_state <> 'X' and qtr_to_pay = 'F')) order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);  // RMC 20170119 correction in SOA of RET with previous year delinq
                       // sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state = 'X' and (qtr_to_pay = 'F' or qtr_to_pay = '1') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);    // RMC 20170228 correction in SOA of RET
                        sQuery = string.Format("select distinct a.* from pay_temp a where bin = '{0}' and due_state = 'X' and (qtr_to_pay = 'F' or qtr_to_pay = '1') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);    // RMC 20180111 CORRECTION IN SOA DOUBLE DISPLAY
                    else
                        sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state = '{1}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay);
                }
                else
                {
                    // RMC 20180108 correction in SOA for current year installment and with prev year delinq (s)
                    if (m_iMultiTaxYear > 1)
                    {
                        sQuery = "select * from pay_temp where bin = '" + sBin + "' and due_state <> 'P' and due_state <> 'X' and due_state <> 'A' ";
                        sQuery += " and ((qtr_to_pay = '" + sQtrToPay + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "') or ";
                        sQuery += " (qtr_to_pay = 'F' and tax_year < '" + ConfigurationAttributes.CurrentYear + "'))";

                    }// RMC 20180108 correction in SOA for current year installment and with prev year delinq (e)
                    else
                    sQuery = "select * from pay_temp where bin = '" + sBin + "' and due_state <> 'P' and due_state <> 'X' and due_state <> 'A' and qtr_to_pay = '" + sQtrToPay + "' ";
                    /*if (sBin == "192-00-2011-0001708")
                        sQuery += " and qtr <> '4' ";*/
                    // RMC 20141202 deleted hard-coded bin lgu code
                    sQuery += " order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                }
                sRange1 = "1";
            }
            else
            {
                sRange1 = "1";
                if (sQtrToPay2 != "" && sQtrToPay3 == "" && sQtrToPay4 == "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('{1}','{2}') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay, sQtrToPay2);
                    sRange2 = "2";
                }
                if (sQtrToPay2 != "" && sQtrToPay3 != "" && sQtrToPay4 == "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('{1}','{2}','{3}') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay, sQtrToPay2, sQtrToPay3);
                    sRange2 = "3";
                }
                if (sQtrToPay2 != "" && sQtrToPay3 != "" && sQtrToPay4 != "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('{1}','{2}','{3}','{4}') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay, sQtrToPay2, sQtrToPay3, sQtrToPay4);
                    sRange2 = "4";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 != "" && sQtrToPay4 == "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('{1}') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay3);
                    sRange1 = "3";
                    sRange2 = "";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 != "" && sQtrToPay4 != "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('{1}','{2}') order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay3, sQtrToPay4);
                    sRange1 = "3";
                    sRange2 = "4";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 == "" && sQtrToPay4 != "")
                {
                    sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay = '{1}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin, sQtrToPay4);
                    sRange1 = "4";
                    sRange2 = "";
                }

                // RMC 20180108 correction in SOA for current year installment and with prev year delinq (s)
                // this will disregard query above
                //note: only current year can be paid in installment
                sQuery = string.Format("select * from pay_temp where bin = '{0}' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and ", sBin);
                sQuery += string.Format(" ((qtr_to_pay in ('{0}','{1}','{2}','{3}') and tax_year = '" + ConfigurationAttributes.CurrentYear + "') or ", sQtrToPay, sQtrToPay2, sQtrToPay3, sQtrToPay4);
                sQuery += " (qtr_to_pay = 'F' and tax_year < '" + ConfigurationAttributes.CurrentYear + "'))";
                sQuery += " order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                // RMC 20180108 correction in SOA for current year installment and with prev year delinq (e) 
            }

            double dTotalDiscountAmount;
            dTotalDiscountAmount = 0;
            int iSwDiscounted = 0;

            /* Discount
            if (sQtrToPay == "F" &&  AppSettingsManager.OnCheckIfDiscounted(dtpORDate.ToString("MM/dd/yyyy")) == true && txtBnsStat.Text.Trim() != "NEW")	// JJP 12142004 FIXED BUGS IN PRACTICE SESSIONS - DISCOUNT // ALJ 02142005 change function definition/declaration to accomodate OFL payment
                iSwDiscounted = 1;*/

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    m_bExistTaxDue = true;
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

                    //AFM 20200728 MAO-20-13358 aligned with View SOA module to display 3 decimals (s)
                    p.fees_due = pSet.GetDouble("fees_due").ToString("##0.000");
                    p.fees_surch = pSet.GetDouble("fees_surch").ToString("##0.000");
                    p.fees_pen = pSet.GetDouble("fees_pen").ToString("##0.000");
                    p.fees_totaldue = pSet.GetDouble("fees_totaldue").ToString("##0.000");
                    //AFM 20200728 MAO-20-13358 aligned with View SOA module to display 3 decimals (e)
                   
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
                    else if ((m_bDiscQtr == true || m_sNextQtrToPay == "4") && pApp->OnCheckIfDiscounted(m_sORDate) == true && txtBnsStat.Text.Trim() != "NEW")
                    {
                        _RecordsetPtr pRec;
                        pRec.CreateInstance(__uuidof(Recordset));
                        CString sDiscountRoundOff;
                        double dDiscountResult = 0;
                        if (((p.fees_code.Substring(0,1) == 'B' && p.fees_code != "B2203") || p.fees_code.Left(2) == "13") && p.tax_year == m_sCurrentTaxYear && p.due_state != "A" && p.due_state != "X" && p.due_state != "P")		// CTS 01042005 fix bugs on discounting for DELIVERY VEHICLE	// JJP 01262006 Do not include if ADJ, retirement or permit update
                        {
                            CString sQtrPaid;
                            int iRemQtr = 0;
                            sQuery = string.Format("select max(qtr_paid) as qtr_paid from pay_hist where bin = '%s' and tax_year = (select max(tax_year) from pay_hist where bin = '%s')", m_sBIN, m_sBIN);
                            pRec->Open(_bstr_t(sQuery), pApp->m_pConnection.GetInterfacePtr(), adOpenStatic, adLockReadOnly, adCmdText);
                            if (!pRec->adoEOF)
                                sQtrPaid = pApp->GetStrVariant(pRec->GetCollect("qtr_paid"));
                            pRec->Close();
                            if (sQtrPaid == "" || sQtrPaid == "F")
                                sQtrPaid = "1";
                            iRemQtr = 4 - Convert.ToInt32(sQtrPaid);

                            dDiscountResult = atof(p.fees_due) * 4 * m_dDiscountRate / iRemQtr;
                            sDiscountRoundOff.Format("%0.2f", dDiscountResult);
                            sDiscountedAmount.Format("%0.2f", atof(p.fees_due) - atof(sDiscountRoundOff));
                            dTotalDiscountAmount = dTotalDiscountAmount + atof(sDiscountRoundOff);
                            mc_vsfaFlexPayment.SetTextMatrix((mc_vsfaFlexPayment.GetRows()) - 1, 11, sDiscountedAmount);
                        }
                        else
                            mc_vsfaFlexPayment.SetTextMatrix((mc_vsfaFlexPayment.GetRows()) - 1, 11, "0.00");
                    }
                    */
                }
            }
            pSet.Close();

            if (m_bExistTaxPU == false)
            {
                // RMC 20170216 added btax qtrly payment if with permit-update transaction (s)
                if (AppSettingsManager.GetConfigValue("10") == "243")
                {
                    // RMC 20170404 additional validation in SOA and Payment if with new addl bns (s)
                    if (ValidatePUAddlBns(sBin))
                    {
                        sQtrToPay = "";
                        sQtrToPay2 = "";
                        sQtrToPay3 = "";
                        sQtrToPay4 = "";
                    }
                    // RMC 20170404 additional validation in SOA and Payment if with new addl bns (e)

                    sQuery = "select * from  pay_temp where bin = '" + sBin + "' and due_state = 'P' ";
                    //sQuery += " and qtr_to_pay in ('" + sQtrToPay + "','" + sQtrToPay2 + "','" + sQtrToPay3 + "','" + sQtrToPay4 + "') ";
                    //sQuery += " and qtr_to_pay in ('" + sQtrToPay + "','" + sQtrToPay2 + "','" + sQtrToPay3 + "','" + sQtrToPay4 + "','F') ";   // RMC 20170223 corrected in SOA permit update transaction not displayed   
                    // RMC 20170224 corrected error in SOA of permit-update (s)
                    if (sQtrToPay == "" && sQtrToPay2 == "" && sQtrToPay3 == "" && sQtrToPay4 == "")
                        sQuery += " and qtr_to_pay in ('F') ";
                    else
                        sQuery += " and qtr_to_pay in ('" + sQtrToPay + "','" + sQtrToPay2 + "','" + sQtrToPay3 + "','" + sQtrToPay4 + "') ";
                    // RMC 20170224 corrected error in SOA of permit-update (e)

                    sQuery += " order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                }
                else// RMC 20170216 added btax qtrly payment if with permit-update transaction (e)
                    sQuery = string.Format("select distinct * from pay_temp where bin = '{0}' and due_state = 'P' and qtr_to_pay = 'F' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBin);
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        m_bExistTaxPU = true;
                        iSwDiscounted = 0;
                        chkFull.Enabled = true;

                        p.bin = pSet.GetString("bin");
                        p.tax_year = pSet.GetString("tax_year");
                        p.term = pSet.GetString("term");
                        if (p.term == "I")
                            p.term = "Q";
                        p.qtr = pSet.GetString("qtr");
                        if (p.qtr == "F")
                            p.qtr = "Y";
                        p.fees_desc = pSet.GetString("fees_desc");
                        p.fees_due = MathUtilities.RoundValue(pSet.GetDouble("fees_due"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                        p.fees_surch = MathUtilities.RoundValue(pSet.GetDouble("fees_surch"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                        p.fees_pen = MathUtilities.RoundValue(pSet.GetDouble("fees_pen"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                        p.fees_totaldue = MathUtilities.RoundValue(pSet.GetDouble("fees_totaldue"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                        p.fees_code = pSet.GetString("fees_code");
                        p.due_state = pSet.GetString("due_state");
                        p.qtr_to_pay = pSet.GetString("qtr_to_pay");

                        dgvTaxFees.Rows.Add(p.tax_year, p.term, p.qtr, p.fees_desc, p.fees_due, p.fees_surch, p.fees_pen, p.fees_totaldue, p.due_state, p.qtr_to_pay, p.fees_code);

                        /* Discount
                        String sDiscountedAmount;
                        if (iSwDiscounted == 1)
                        {
                            CString sTestFeesCode;
                            CString sDiscountRoundOff;	// CTS 01182005 fix bugs on 0.01 difference
                            sTestFeesCode = p.fees_code;
                            double dDiscountResult = 0; // CTS 01182005 fix bugs on 0.01 difference
                            if (((p.fees_code.Substring(0,1) == 'B' && p.fees_code != "B2203") || p.fees_code.Left(2) == "13") && p.tax_year == m_sCurrentTaxYear && p.due_state != "A" && p.due_state != "X" && p.due_state != "P")		// CTS 01042005 fix bugs on discounting for DELIVERY VEHICLE	// JJP 01262006 Do not include if ADJ, retirement or permit update
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
                        //(s) RTL 10182005 discount in installment
                        else if ((m_bDiscQtr == true || m_sNextQtrToPay == "4") && pApp->OnCheckIfDiscounted(m_sORDate) == true && txtBnsStat.Text.Trim() != "NEW")
                        {
                            _RecordsetPtr pRec;
                            pRec.CreateInstance(__uuidof(Recordset));
                            CString sDiscountRoundOff;
                            double dDiscountResult = 0;
                            if (((p.fees_code.Substring(0,1) == 'B' && p.fees_code != "B2203") || p.fees_code.Left(2) == "13") && p.tax_year == m_sCurrentTaxYear && p.due_state != "A" && p.due_state != "X" && p.due_state != "P")		// CTS 01042005 fix bugs on discounting for DELIVERY VEHICLE	// JJP 01262006 Do not include if ADJ, retirement or permit update
                            {
                                CString sQtrPaid;
                                int iRemQtr = 0;
                                sQuery = string.Format("select max(qtr_paid) as qtr_paid from pay_hist where bin = '%s' and tax_year = (select max(tax_year) from pay_hist where bin = '%s')", m_sBIN, m_sBIN);
                                pRec->Open(_bstr_t(sQuery), pApp->m_pConnection.GetInterfacePtr(), adOpenStatic, adLockReadOnly, adCmdText);
                                if (!pRec->adoEOF)
                                    sQtrPaid = pApp->GetStrVariant(pRec->GetCollect("qtr_paid"));
                                pRec->Close();
                                if (sQtrPaid == "" || sQtrPaid == "F")
                                    sQtrPaid = "1";
                                iRemQtr = 4 - Convert.ToInt32(sQtrPaid);

                                dDiscountResult = atof(p.fees_due) * 4 * m_dDiscountRate / iRemQtr;
                                sDiscountRoundOff.Format("%0.2f", dDiscountResult);
                                sDiscountedAmount.Format("%0.2f", atof(p.fees_due) - atof(sDiscountRoundOff));
                                dTotalDiscountAmount = dTotalDiscountAmount + atof(sDiscountRoundOff);
                                mc_vsfaFlexPayment.SetTextMatrix((mc_vsfaFlexPayment.GetRows()) - 1, 11, sDiscountedAmount);
                            }
                            else
                                mc_vsfaFlexPayment.SetTextMatrix((mc_vsfaFlexPayment.GetRows()) - 1, 11, "0.00");
                        }
                        */
                    }

                }
                pSet.Close();
            }

            if (m_bIsDisCompromise == true || m_sCompPaySw == "N")
                OnCompromise(sBin, sRange1, sRange2);

            iSwDiscounted = 0;
            if (sQtrToPay != "A")
                txtDiscountAmount.Text = dTotalDiscountAmount.ToString();

            sQuery = string.Format("select * from pay_hist where bin = '{0}' and bns_stat = 'NEW'", sBin);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (!pSet.Read() && AppSettingsManager.GetConfigValue("15").ToUpper() == "Y")
                {
                    if (sQtrToPay != "F" && sQtrToPay != "1" && m_sStatus == "NEW")
                    {
                        sQuery = string.Format("select * from  pay_temp where bin = '{0}' and due_state = 'N' and qtr_to_pay = 'F' and fees_code not like 'B%%' order by fees_code_sort", sBin);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            while (pSet1.Read())
                            {
                                p.bin = pSet.GetString("bin");
                                p.tax_year = pSet.GetString("tax_year");
                                p.term = pSet.GetString("term");
                                p.qtr = pSet.GetString("qtr");
                                p.fees_desc = pSet.GetString("fees_desc");
                                p.fees_due = MathUtilities.RoundValue(pSet.GetDouble("fees_due"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                                p.fees_surch = MathUtilities.RoundValue(pSet.GetDouble("fees_surch"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                                p.fees_pen = MathUtilities.RoundValue(pSet.GetDouble("fees_pen"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                                p.fees_totaldue = MathUtilities.RoundValue(pSet.GetDouble("fees_totaldue"), 2, MidpointRounding.AwayFromZero).ToString("##0.00");
                                p.fees_code = pSet.GetString("fees_code");
                                p.due_state = pSet.GetString("due_state");
                                p.qtr_to_pay = pSet.GetString("qtr_to_pay");

                                dgvTaxFees.Rows.Add(p.tax_year, p.term, p.qtr, p.fees_desc, p.fees_surch, p.fees_pen, p.fees_totaldue, p.qtr_to_pay, p.fees_code);
                            }
                        }
                        pSet1.Close();
                    }
                }
            pSet.Close();

            ComputeTotal();
            SetControls(true);
            txtCompromise.Enabled = false;

            if (p.qtr_to_pay != "1" && m_iMultiTaxYear == 1)
            {
                chkFull.Enabled = false;
                chkInstallment.Enabled = true;
            }
            if (m_iMultiTaxYear != 1 && Convert.ToInt32(txtTaxYear.Text) < Convert.ToInt32(m_sCurrentTaxYear))
            {
                chkFull.Enabled = true;
                chkInstallment.Enabled = false;
            }
            if (p.due_state == "N" || p.due_state == "Q")
            {
                chkFull.Enabled = true;
                chkInstallment.Enabled = false;
            }

            if (m_bEnableTaxPU == true || txtBnsStat.Text == "NEW")
            {
                chkFull.Enabled = true;
                chkInstallment.Enabled = false;
                if (m_bEnableTaxDue == true && txtBnsStat.Text != "NEW")
                    chkInstallment.Enabled = true;
            }

            txtGrandTotal.Text = m_sDiscountedTotal;
            DeductTaxCredit();
            CompLessCredit(m_sDiscountedTotal, m_sCreditLeft, txtToTaxCredit.Text);

        }
        //MCR 20140722 (e)

        private void DeductTaxCredit()
        {
            String sQuery;
            OracleResultSet pSet = new OracleResultSet();

            txtTotDues.Text = "0.00";
            txtCreditLeft.Text = "0.00";
            txtToTaxCredit.Text = "0.00";
            chkTCredit.Enabled = false;

            if (m_sOwnCode != "")
            {
                // RMC 20141127 QA Tarlac ver (s)
                //if (AppSettingsManager.GetConfigValue("10") == "017")
                //    pSet.Query = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N' and multi_pay = 'N'", m_sOwnCode);
                //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                //    pSet.Query = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N' and multi_pay = 'N'", m_sOwnCode);
                //else// RMC 20141127 QA Tarlac ver (e)
                    pSet.Query = string.Format("select * from dbcr_memo where bin = '{0}' and served = 'N' and multi_pay = 'N'", m_sBIN); //JARS 20171010
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        m_sCreditLeft = pSet.GetDouble("balance").ToString("#,##0.00");
                        txtCreditLeft.Text = m_sCreditLeft;
                    }
                pSet.Close();

            }
            txtCreditLeft.Text = m_sCreditLeft;

            //Discount
            bool bDeducted = false;
            if ((Convert.ToDouble(m_sDiscountedTotal) <= Convert.ToDouble(txtCreditLeft.Text)) && Convert.ToDouble(m_sDiscountedTotal) > 0)
            {
                bDeducted = true;
                txtToTaxCredit.ReadOnly = true;
                txtToTaxCredit.Text = m_sDiscountedTotal;
            }
            txtToTaxCredit.Text = Convert.ToDouble(m_sDiscountedTotal).ToString("#,##0.00");

            if (Convert.ToDouble(txtCreditLeft.Text) > 0.00 && Convert.ToDouble(m_sDiscountedTotal) > Convert.ToDouble(txtCreditLeft.Text))
            {
                bDeducted = true;
                txtToTaxCredit.Text = m_sCreditLeft;
            }
            txtToTaxCredit.Text = Convert.ToDouble(txtCreditLeft.Text).ToString("#,##0.00");

            chkCash.Checked = false;
            chkCheque.Checked = false;
            chkTCredit.Checked = false;

            if (bDeducted == true)
            {
                chkTCredit.Checked = true;
                chkTCredit.Enabled = true;
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

        private void DebitTaxCredit(String sOwnCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;

            String sRefChkNo, sCMNo, sCredit, sDebit, sBalance, sServe, sMemo;
            // RMC 20141127 QA Tarlac ver (s)
            //if (AppSettingsManager.GetConfigValue("10") == "017")
            //    pSet.Query = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N' and multi_pay = 'N'", sOwnCode);
            //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
            //   pSet.Query = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N' and multi_pay = 'N'", sOwnCode);
            //else// RMC 20141127 QA Tarlac ver (e)
                pSet.Query = string.Format("select * from dbcr_memo where bin = '{0}' and served = 'N' and multi_pay = 'N'", m_sBIN); //JARS 20171010
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sRefChkNo = pSet.GetString("chk_no");
                    sBalance = pSet.GetDouble("balance").ToString();

                    if (Convert.ToDouble(sBalance) > 0)
                    {
                        sCredit = "0";

                        sDebit = Convert.ToDouble(txtToTaxCredit.Text).ToString("##0.00");
                        sBalance = Convert.ToDouble(Convert.ToDouble(sCredit) - Convert.ToDouble(sDebit)).ToString("##0.00");

                        sServe = "Y";
                        sMemo = "DEBITED THRU PAYMENT MADE HAVING OR_NO " + txtORNo.Text;
                        sQuery = "insert into dbcr_memo values "; //JARS 20171010
                        // RMC 20141127 QA Tarlac ver (s)
                        //if (AppSettingsManager.GetConfigValue("10") == "017")
                        //    sQuery += "('" + sOwnCode + "', ";
                        //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                        //    sQuery += "('" + sOwnCode + "', ";
                        //else// RMC 20141127 QA Tarlac ver (e)
                            sQuery += "('" + m_sBIN + "', "; //JARS 20171010
                        sQuery += " '" + txtORNo.Text + "', ";
                        sQuery += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyy") + "','MM/dd/yyyy'), ";
                        sQuery += sDebit + ", ";
                        sQuery += Convert.ToDouble(sCredit).ToString("##0.00") + ", ";
                        sQuery += sBalance + ", ";
                        sQuery += " '" + sMemo + "','',";
                        sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                        sQuery += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyy") + "','MM/dd/yyyy'), ";
                        sQuery += "to_date('" + dtpORDate.Value.ToString("hh:mm") + "','hh:mm'), ";
                        sQuery += "'" + sServe + "',";
                        sQuery += "'" + sRefChkNo + "',";
                        sQuery += "'N','0')";
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();

                        // RMC 20141127 QA Tarlac ver (s)
                        //if (AppSettingsManager.GetConfigValue("10") == "017")
                        //    pSet.Query = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}' and served = 'N' and multi_pay = 'N'", sOwnCode);
                        //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                        //    pSet.Query = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}' and served = 'N' and multi_pay = 'N'", sOwnCode);
                        //else// RMC 20141127 QA Tarlac ver (e)
                            pSet.Query = string.Format("update dbcr_memo set served = 'Y' where bin = '{0}' and served = 'N' and multi_pay = 'N'", m_sBIN); //JARS 20171010
                        pSet.ExecuteNonQuery();

                        sDebit = "0";

                        sBalance = Convert.ToDouble(sDebit).ToString("##0.00");

                        sServe = "Y";
                        sMemo = "REMAINING BALANCE AFTER PAYMENT W/ DEBIT HAVING OR " + txtORNo.Text;
                        sQuery = "insert into dbcr_memo values "; //JARS 20171010
                        // RMC 20141127 QA Tarlac ver (s)
                        //if (AppSettingsManager.GetConfigValue("10") == "017")
                        //    sQuery += "('" + sOwnCode + "', ";
                        //else if (AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                        //    sQuery += "('" + sOwnCode + "', ";
                        //else// RMC 20141127 QA Tarlac ver (e)
                            sQuery += "('" + m_sBIN + "', "; //JARS 20171010
                        sQuery += " '" + txtORNo.Text.Trim() + "', ";
                        sQuery += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyy") + "','MM/dd/yyyy'), ";
                        sQuery += Convert.ToDouble(sDebit).ToString("##0.00") + ", ";
                        sQuery += Convert.ToDouble(sCredit).ToString("##0.00") + ", ";
                        sQuery += sBalance + ", ";
                        sQuery += " '" + sMemo + "', '',";
                        sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                        sQuery += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyy") + "','MM/dd/yyyy'), ";
                        sQuery += "to_date('" + dtpORDate.Value.ToString("hh:mm") + "','hh:mm'), ";
                        sQuery += "'" + sServe + "',";
                        sQuery += "'" + sRefChkNo + "',";
                        sQuery += "'N','0')";
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                    }
                }
            pSet.Close();
        }

        private bool IsPaidOnLastQtr(String sBin, String sTaxYear, String sBnsCode, String sQtr)
        {
            bool bResult = false;
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from pay_hist where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and qtr_paid = '" + sQtr + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    bResult = true;
            pSet.Close();
            return bResult;
        }

        private void DisplayDues(string sBin, string sQtrToPay, string sQtrToPay2, string sQtrToPay3, string sQtrToPay4)
        {
            dgvTaxFees.Rows.Clear();
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            m_dTotalTotalDue = 0;
            m_dTotalSurcharge = 0;
            m_dTotalPenalty = 0;
            m_dTotalDue = 0;

            m_dTotalDue1 = 0;
            m_dTotalSurcharge1 = 0;
            m_dTotalPenalty1 = 0;
            m_dTotalTotalDue1 = 0;
            string sRange1 = string.Empty;
            string sRange2 = string.Empty;

            if (m_sNextQtrToPay.Trim() == string.Empty)
                m_sNextQtrToPay = "1";

            if (sQtrToPay2.Trim() == string.Empty && sQtrToPay3.Trim() == string.Empty && sQtrToPay4.Trim() == string.Empty)
            {
                if (sQtrToPay == "F" && int.Parse(m_sNextQtrToPay) > 1 && txtBnsStat.Text.Trim() != "NEW")
                    sQtrToPay = string.Empty;
                if (sQtrToPay == "A" || sQtrToPay == "X")
                {
                    result.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and due_state = '" + sQtrToPay.Trim() + "'  order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    if (sQtrToPay == "X")
                        result.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and due_state = 'X' and qtr_to_pay = 'F'  order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                }
                else
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'X' and due_state <> 'A' and qtr_to_pay = '" + sQtrToPay.Trim() + "' order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                sRange1 = "1";
            }
            else
            {
                sRange1 = "1";
                if (sQtrToPay2 != "" && sQtrToPay3 == "" && sQtrToPay4 == "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('" + sQtrToPay.Trim() + "','" + sQtrToPay2.Trim() + "') order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange2 = "2";
                }
                if (sQtrToPay2 != "" && sQtrToPay3 != "" && sQtrToPay4 == "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('" + sQtrToPay.Trim() + "','" + sQtrToPay2.Trim() + "','" + sQtrToPay3.Trim() + "') order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange2 = "3";
                }
                if (sQtrToPay2 != "" && sQtrToPay3 != "" && sQtrToPay4 != "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('" + sQtrToPay.Trim() + "','" + sQtrToPay2.Trim() + "','" + sQtrToPay3.Trim() + "','" + sQtrToPay4.Trim() + "') order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange2 = "4";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 != "" && sQtrToPay4 == "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay  = '" + sQtrToPay3.Trim() + "' order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange1 = "3";
                    sRange2 = "";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 != "" && sQtrToPay4 != "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay in ('" + sQtrToPay3.Trim() + "','" + sQtrToPay4.Trim() + "') order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange1 = "3";
                    sRange2 = "4";
                }
                if (sQtrToPay2 == "" && sQtrToPay3 == "" && sQtrToPay4 != "")
                {
                    result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state <> 'P' and due_state <> 'A' and due_state <> 'X' and qtr_to_pay = '" + sQtrToPay4.Trim() + "' order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                    sRange1 = "4";
                    sRange2 = "";
                }

            }

            // codes for discount
            if (result.Execute())
            {
                m_bExistTaxDue = true;
                while (result.Read())
                {
                    p_bin = result.GetString("bin").Trim();
                    p_tax_year = result.GetString("tax_year").Trim();
                    p_term = result.GetString("term").Trim();
                    p_qtr = result.GetString("qtr").Trim();
                    if (p_term == "I")
                        p_term = "Q";
                    if (p_qtr == "F")
                        p_qtr = "Y";

                    p_fees_desc = result.GetString("fees_desc").Trim();
                    p_fees_due = result.GetDouble("fees_due").ToString();
                    p_fees_surch = result.GetDouble("fees_surch").ToString();
                    p_fees_pen = result.GetDouble("fees_pen").ToString();
                    p_fees_totaldue = result.GetDouble("fees_totaldue").ToString();
                    p_fees_code = result.GetString("fees_code").Trim();
                    p_due_state = result.GetString("due_state").Trim();
                    p_qtr_to_pay = result.GetString("qtr_to_pay").Trim();

                    //                    dgvTaxFees.Rows.Add(p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);

                    double.TryParse(p_fees_due, out m_dTotalDue);
                    double.TryParse(p_fees_surch, out m_dTotalSurcharge);
                    double.TryParse(p_fees_pen, out m_dTotalPenalty);
                    double.TryParse(p_fees_totaldue, out m_dTotalTotalDue);

                    m_dTotalDue1 = m_dTotalDue1 + m_dTotalDue;
                    m_dTotalSurcharge1 = m_dTotalSurcharge1 + m_dTotalSurcharge;
                    m_dTotalPenalty1 = m_dTotalPenalty1 + m_dTotalPenalty;
                    m_dTotalTotalDue1 = m_dTotalTotalDue1 + m_dTotalTotalDue;

                    // GDE 20101103 for decimal places (s){
                    double.TryParse(p_fees_due.ToString(), out dDue);
                    p_fees_due = string.Format("{0:0.00}", (double)dDue);
                    double.TryParse(p_fees_surch.ToString(), out dSurch);
                    p_fees_surch = string.Format("{0:0.00}", (double)dSurch);
                    double.TryParse(p_fees_pen.ToString(), out dPen);
                    p_fees_pen = string.Format("{0:0.00}", (double)dPen);
                    double.TryParse(p_fees_totaldue.ToString(), out dTot);
                    p_fees_totaldue = string.Format("{0:0.00}", (double)dTot);
                    // GDE 20101103 for decimal places (e)}

                    dgvTaxFees.Rows.Add(p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);
                }
            }
            result.Close();

            // permit update codes

            if (m_bExistTaxPU == false)
            {
                result.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state = 'P' and qtr_to_pay = 'F' order by tax_year,qtr_to_pay,qtr,fees_code_sort";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        m_bExistTaxPU = true;
                        p_bin = result.GetString("bin").Trim();
                        p_tax_year = result.GetString("tax_year").Trim();
                        p_term = result.GetString("term").Trim();
                        p_qtr = result.GetString("qtr").Trim();
                        if (p_term == "I")
                            p_term = "Q";
                        if (p_qtr == "F")
                            p_qtr = "Y";

                        p_fees_desc = result.GetString("fees_desc").Trim();
                        p_fees_due = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_due").ToString()));
                        p_fees_surch = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_surch").ToString()));
                        p_fees_pen = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_pen").ToString()));
                        p_fees_totaldue = string.Format("{0:#,##0.00}", double.Parse(result.GetDouble("fees_totaldue").ToString()));
                        p_fees_code = result.GetString("fees_code").Trim();
                        p_due_state = result.GetString("due_state").Trim();
                        p_qtr_to_pay = result.GetString("qtr_to_pay").Trim();

                        double.TryParse(p_fees_due, out m_dTotalDue);
                        double.TryParse(p_fees_surch, out m_dTotalSurcharge);
                        double.TryParse(p_fees_pen, out m_dTotalPenalty);
                        double.TryParse(p_fees_totaldue, out m_dTotalTotalDue);

                        m_dTotalDue1 = m_dTotalDue1 + m_dTotalDue;
                        m_dTotalSurcharge1 = m_dTotalSurcharge1 + m_dTotalSurcharge;
                        m_dTotalPenalty1 = m_dTotalPenalty1 + m_dTotalPenalty;
                        m_dTotalTotalDue1 = m_dTotalTotalDue1 + m_dTotalTotalDue;

                        // GDE 20101103 for decimal places (s){
                        double.TryParse(p_fees_due.ToString(), out dDue);
                        p_fees_due = string.Format("{0:0.00}", (double)dDue);
                        double.TryParse(p_fees_surch.ToString(), out dSurch);
                        p_fees_surch = string.Format("{0:0.00}", (double)dSurch);
                        double.TryParse(p_fees_pen.ToString(), out dPen);
                        p_fees_pen = string.Format("{0:0.00}", (double)dPen);
                        double.TryParse(p_fees_totaldue.ToString(), out dTot);
                        p_fees_totaldue = string.Format("{0:0.00}", (double)dTot);
                        // GDE 20101103 for decimal places (e)}

                        dgvTaxFees.Rows.Add(p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);
                    }
                }
                result.Close();
            }

            //(s) RTL 11302005 for installment in NEW

            result.Query = "select * from  pay_hist where bin = '" + sBin.Trim() + "' and bns_stat = 'NEW'";
            if (result.Execute()/* insert condition for qtrly dec of new */)
            {
                if (result.Read())
                {
                    if (sQtrToPay != "F" && sQtrToPay != "1" && txtBnsStat.Text.Trim() == "NEW")
                    {
                        result2.Query = "select * from  pay_temp where bin = '" + sBin.Trim() + "' and due_state = 'N' and qtr_to_pay = 'F' and fees_code not like 'B%' order by fees_code_sort";
                        if (result2.Execute())
                        {
                            while (result2.Read())
                            {
                                p_bin = result.GetString("bin").Trim();
                                p_tax_year = result.GetString("tax_year").Trim();
                                p_term = result.GetString("term").Trim();
                                p_qtr = result.GetString("qtr").Trim();

                                if (p_term == "I")
                                    p_term = "Q";
                                if (p_qtr == "F")
                                    p_qtr = "Y";

                                p_fees_desc = result.GetString("fees_desc").Trim();
                                p_fees_due = result.GetDouble("fees_due").ToString();
                                p_fees_surch = result.GetDouble("fees_surch").ToString();
                                p_fees_pen = result.GetDouble("fees_pen").ToString();
                                p_fees_totaldue = result.GetDouble("fees_totaldue").ToString();
                                p_fees_code = result.GetString("fees_code").Trim();
                                p_due_state = result.GetString("due_state").Trim();
                                p_qtr_to_pay = result.GetString("qtr_to_pay").Trim();


                                double.TryParse(p_fees_due, out m_dTotalDue);
                                double.TryParse(p_fees_surch, out m_dTotalSurcharge);
                                double.TryParse(p_fees_pen, out m_dTotalPenalty);
                                double.TryParse(p_fees_totaldue, out m_dTotalTotalDue);

                                m_dTotalDue1 = m_dTotalDue1 + m_dTotalDue;
                                m_dTotalSurcharge1 = m_dTotalSurcharge1 + m_dTotalSurcharge;
                                m_dTotalPenalty1 = m_dTotalPenalty1 + m_dTotalPenalty;
                                m_dTotalTotalDue1 = m_dTotalTotalDue1 + m_dTotalTotalDue;

                                // GDE 20101103 for decimal places (s){
                                double.TryParse(p_fees_due.ToString(), out dDue);
                                p_fees_due = string.Format("{0:0.00}", (double)dDue);
                                double.TryParse(p_fees_surch.ToString(), out dSurch);
                                p_fees_surch = string.Format("{0:0.00}", (double)dSurch);
                                double.TryParse(p_fees_pen.ToString(), out dPen);
                                p_fees_pen = string.Format("{0:0.00}", (double)dPen);
                                double.TryParse(p_fees_totaldue.ToString(), out dTot);
                                p_fees_totaldue = string.Format("{0:0.00}", (double)dTot);
                                // GDE 20101103 for decimal places (e)}

                                dgvTaxFees.Rows.Add(p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);
                            }
                        }
                        result2.Close();

                    }
                }
            }
            result.Close();

            txtTotDue.Text = string.Format("{0:#,##0.00}", m_dTotalDue1);
            txtTotSurch.Text = string.Format("{0:#,##0.00}", m_dTotalSurcharge1);
            txtTotPen.Text = string.Format("{0:#,##0.00}", m_dTotalPenalty1);
            txtTotTotDue.Text = string.Format("{0:#,##0.00}", m_dTotalTotalDue1);

            // controlling control codes here

        }

        private void OnCheckAdj()
        {
            DisplayDuesNew(m_sBIN.Trim(), "A", "", "", "");
        }

        private void OnCheckRetirement()
        {
            MessageBox.Show("RETIREMENT");
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

        private void txtLastChange_TextChanged(object sender, EventArgs e)
        {
            txtLastChange.SelectAll();
            txtLastChange.Copy();
        }

        //MCR 20140724 (s)
        private void SavePaymentDues()
        {
            
            string sPayTerm = string.Empty;
            string sTaxYear = string.Empty, sDueState = string.Empty, sQtr = string.Empty;
            string sQtrToPay = string.Empty, sQtrToPay2 = string.Empty, sQtrToPay3 = string.Empty, sQtrToPay4 = string.Empty, sCtr = string.Empty;
            string sBnsStat = string.Empty;
            int iNextOrNo = 0;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            result.Query = "delete from pay_hist where bin = '" + m_sBIN.Trim() + "' and data_mode = 'UNP'";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();

            string sFATaxYearPrev = string.Empty, sFATaxYear = string.Empty, sFATerm = string.Empty, sFAQtr = string.Empty, sFADue = string.Empty, sFASurch = string.Empty, sFAPen = string.Empty, sFATotal = string.Empty, sFADueStatePrev = string.Empty, sFADueState = string.Empty, sFAQtrToPayPrev = string.Empty, sFAQtrToPay = string.Empty, sFAFeesCode = string.Empty, sFADiscounted = string.Empty;
            string sFeesCode = string.Empty, sFeesDue = string.Empty, sFeesSurch = string.Empty, sFeesPen = string.Empty, sFeesAmtDue = string.Empty, sBnsCodeOR = string.Empty;

            // RMC 20151229 merged multiple OR use from Mati (s)
            string sORNo = string.Empty;    
            string sTmpORNo = string.Empty;    
            sTmpORNo = txtORNo.Text;
            int iMultOrCnt = 0;
            string sTmpFeeCode = string.Empty;
            // RMC 20151229 merged multiple OR use from Mati (e)
            string strTmpFeesCode = string.Empty; // AST 20160125
            string sTellerOr = string.Empty; //JARS 20170823
            // RMC 20160503 correction in saving payment using multiple OR (s)
            int iPayHistOrNo = 0;
            int iPayHistCnt = 0;
            int.TryParse(txtORNo.Text.Trim(), out iPayHistOrNo);
            // RMC 20160503 correction in saving payment using multiple OR (e)

            for (int i = 0; i <= dgvTaxFees.Rows.Count - 1; i++)
            {
                sFATaxYear = dgvTaxFees.Rows[i].Cells[0].Value.ToString();
                sFATerm = dgvTaxFees.Rows[i].Cells[1].Value.ToString();
                sFAQtr = dgvTaxFees.Rows[i].Cells[2].Value.ToString();
                sFADue = dgvTaxFees.Rows[i].Cells[4].Value.ToString();
                sFASurch = dgvTaxFees.Rows[i].Cells[5].Value.ToString();
                sFAPen = dgvTaxFees.Rows[i].Cells[6].Value.ToString();
                sFATotal = dgvTaxFees.Rows[i].Cells[7].Value.ToString();
                sFADueState = dgvTaxFees.Rows[i].Cells[8].Value.ToString();
                sFAQtrToPay = dgvTaxFees.Rows[i].Cells[9].Value.ToString();
                sFAFeesCode = dgvTaxFees.Rows[i].Cells[10].Value.ToString();
                //sFADiscounted = dgvTaxFees.Rows[i].Cells[11].Value.ToString(); // try to look for remedies

                // RMC 20160512 addl correction use of multiple OR (s)
                // Note: insertion to or_table is per row, so counting should be per row also
                iMultOrCnt++;
                iPayHistCnt++;
                // RMC 20160512 addl correction use of multiple OR (e)

                /*
                // RMC 20151229 merged multiple OR use from Mati (s)
                if (sFAFeesCode.Substring(0, 1) == "B")
                {
                    // AST 20160125 correction in display dues (s)
                    //iMultOrCnt++;
                    if (strTmpFeesCode != sFAFeesCode)
                    {
                        strTmpFeesCode = sFAFeesCode;
                        iMultOrCnt++;
                        iPayHistCnt++;  // RMC 20160503 correction in saving payment using multiple OR
                    }
                    // AST 20160125 correction in display dues (e)
                }
                else
                {
                    if (sTmpFeeCode != sFAFeesCode.Substring(0, 2))
                    {
                        sTmpFeeCode = sFAFeesCode.Substring(0, 2);
                        iMultOrCnt++;
                        iPayHistCnt++;  // RMC 20160503 correction in saving payment using multiple OR
                        
                    }
                }
                // RMC 20151229 merged multiple OR use from Mati (e)
                */
                // RMC 20160512 addl correction use of multiple OR, put rem

                sDueState = sFADueState;

                if (sFATaxYear != AppSettingsManager.GetSystemDate().Year.ToString())								//delinquent year
                {
                    if (sFAQtrToPay == "1" && sFAQtr == "F")
                    {
                        sFATerm = "F";
                        sFAQtrToPay = "F";
                    }
                    //else if(sFAQtrToPay == "1" && sFAQtr != "F")


                    else if (sFAQtrToPay != "F" && (int.Parse(sFAQtrToPay) >= 1 && sFAQtr != "F"))		// JGR 01152007 IF PAY TERM IS INSTALLMENT,  PAY_TERM SHOULD BE I AND QTR TO PAY IS THE DISPLAYED QTR
                    {
                        sFATerm = "I";
                        sFAQtrToPay = sFAQtr;
                    }
                    else
                    {
                        sFATerm = "F";
                        sFAQtrToPay = "F";
                    }
                }

                // RMC 20140909 Migration QA
                if (sFAQtr == "X")
                    sFAQtrToPay = "X";
                // RMC 20140909 Migration QA

                string sPayHistOrNo = string.Empty; // RMC 20170106 correction in saving OR no. in multiple OR use
                sPayHistOrNo = string.Format("{0:0######}", iPayHistOrNo);
                //if(m_iORNo <= 1)
                //{
                    //m_sTellerOR = sPayHistOrNo;
                //}

                //JHB 20181203 merge from LAL-LO Rev-Adjustment (S)
                //JARS 20181004 (S) ATTEMPT AT SAVING DECLARED GROSS IN PAY_HIST TABLE, TO HAVE A RECORD ON WHAT GROSS WAS DECLARED ON THAT YEAR
                //JARS 20181129 MEETING COMCLUSION, DECLARED GROSS IN PAY_HIST IS NOW OBSOLETE, WILL NOW TRY TO PLACE IT IN OR_TABLE
                //WITH NEW COLUMN
                string sBnsDesc = "";
                if (AppSettingsManager.GetConfigObject("10") == "243" || AppSettingsManager.GetConfigObject("10") == "216")
                {
                    if (sFAFeesCode.Substring(0, 1) == "B")
                    {
                        dDeclaredGross = GetBillGrossInfo(m_sBIN, sFATaxYear, sFAFeesCode.Substring(1, sFAFeesCode.Length - 1), sDueState);
                    }

                    if (sDueState == "A") //JARS 20181005 UPDATE BILL_GROSS_INFO SET GROSS TO ADJ_GROSS AND SET ADJ_GROSS TO ZERO, TO BE READY FOR ANOTHER ADJUSTMENT
                    {
                        //JARS 20181122 ADDED ADJ_GROSS = 0, FORGOT THAT PART
                        result.Query = "update bill_gross_info set gross = adj_gross, adj_gross = 0 where bin = '" + m_sBIN + "' and tax_year = '" + sFATaxYear + "' and bns_code = '" + sBnsDesc + "'";
                        if (result.ExecuteNonQuery() != 0)
                        { }
                        result.Close();

                        //JARS 20181008 UPDATE TAXDUES_HIST, SO CAN BE FETCHED BY COMPUTE_ADJUSTMENT FOR SECOND ADJUSTMENT
                        result.Query = "select * from taxdues_dup where bin = '" + m_sBIN + "' and tax_year = '" + sFATaxYear + "' and due_state = 'A' order by tax_code";
                        if (result.Execute())
                        {
                            while (result.Read())
                            {
                                result2.Query = "update taxdues_hist set amount = '" + result.GetDouble("amount") + "' where bin = '" + m_sBIN + "' and tax_code = '" + result.GetString("tax_code") + "' and tax_year = '" + sFATaxYear + "'";
                                if (result2.ExecuteNonQuery() != 0)
                                { }
                            }
                            result.Close();
                        }
                    }
                }
                //JARS 20181004 (E)
                //JHB 20181203 merge from LAL-LO Rev-Adjustment (e)

                if ((sFATaxYear != sFATaxYearPrev || sFADueState != sFADueStatePrev || sFAQtrToPay != sFAQtrToPayPrev) && sFADueState.Substring(0, 1) != "C")
                {
                    // RMC 20160503 correction in saving payment using multiple OR (s)
                    if (m_iORNo > 1 && iPayHistCnt >= 14)
                    {
                        iPayHistOrNo++;
                        //iPayHistCnt = 0;
                        iPayHistCnt = 1;    // RMC 20180102 correction in user of multiple OR
                    }
                    //string sPayHistOrNo = string.Empty;   // RMC 20170106 correction in saving OR no. in multiple OR use, put rem
                    //sPayHistOrNo = string.Format("{0:#######}", iPayHistOrNo);
                    sPayHistOrNo = string.Format("{0:0######}", iPayHistOrNo);
                    // RMC 20160503 correction in saving payment using multiple OR (e)

                    if (sFAQtrToPay != "F")
                        sFATerm = "I";
                    else
                        sFATerm = "F";

                    if (sFADueState == "Q" && sFAQtrToPay != "F")				//quarterly dec
                        sFATerm = "Q";

                    if (sFADueState == "P")										//permit update
                    {
                        // RMC 20170216 added btax qtrly payment if with permit-update transaction (s)
                        if (AppSettingsManager.GetConfigValue("10") == "243")    
                        {
                            if (sFAQtrToPay == "F")
                            {
                                sFATerm = "F";
                                sFAQtrToPay = "P";
                            }
                            else
                            {
                                txtMemo.Text = "PERMIT-UPDATE TRANS";
                            }
                        }
                        else// RMC 20170216 added btax qtrly payment if with permit-update transaction (e)
                        {
                            sFATerm = "F";
                            sFAQtrToPay = "P";
                        }
                    }

                    if (sFAQtr == "A")											//adjustment
                    {
                        sFATerm = "F";
                        sFAQtrToPay = "A";
                    }

                    if (sFAQtr == "X")											//retirement
                    {
                        sFATerm = "F";
                        sFAQtrToPay = "X";
                    }

                    /*
                    // AST 20160120 Added to avoid duplicate insertion on tbl pay_hist (s)
                    result.Query = string.Format("delete from pay_hist where or_no = '{0}' and teller = '{1}'", txtORNo.Text.Trim(), txtTeller.Text);
                    result.ExecuteNonQuery();
                    // AST 20160120 Added to avoid duplicate insertion on tbl pay_hist (e)
                    */
                    // RMC 20160503 correction in saving payment using multiple OR, put rem

                    // RMC 20170217 added checking to avoid duplicate entries in pay_hist (s)
                    OracleResultSet pCheck = new OracleResultSet();
                    int iCheck = 0;

                    pCheck.Query = "select * from pay_hist where or_no = '" + sPayHistOrNo + "' and bin = '" + m_sBIN + "' ";
                    pCheck.Query += "and bns_stat = '" + txtBnsStat.Text.Trim() + "' and tax_year = '" + sFATaxYear + "' ";
                    pCheck.Query += "and qtr_paid = '" + sFAQtrToPay + "' ";
                    if (pCheck.Execute())
                    {
                        if (pCheck.Read())
                            iCheck++;
                    }
                    pCheck.Close();
                    // RMC 20170217 added checking to avoid duplicate entries in pay_hist (e)

                    if (iCheck == 0)    // RMC 20170217 added checking to avoid duplicate entries in pay_hist 
                    {
                        //m_sTellerOR = sPayHistOrNo; //JARS 20170823
                        string sOrDate = string.Empty;
                        string sDatePosted = string.Empty;
                        string sTimePosted = string.Empty;
                        sOrDate = string.Format("{0:MM/dd/yyyy}", dtpORDate.Value);
                        sDatePosted = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetSystemDate());
                        sTimePosted = string.Format("{0:HH:mm}", AppSettingsManager.GetSystemDate());
                        result.Query = "insert into pay_hist(or_no,bin,bns_stat,tax_year,qtr_paid,or_date,payment_term,data_mode,payment_type,date_posted,time_posted,bns_user,teller,memo) values ";
                        //result.Query += "('" + txtORNo.Text.Trim() + "', ";
                        result.Query += "('" + sPayHistOrNo + "', ";    // RMC 20160503 correction in saving payment using multiple OR
                        result.Query += " '" + m_sBIN + "', ";
                        result.Query += " '" + txtBnsStat.Text.Trim() + "', ";
                        result.Query += " '" + sFATaxYear + "', ";
                        result.Query += " '" + sFAQtrToPay + "', ";
                        result.Query += "to_date('" + sOrDate + "','MM/dd/yyyy'), ";
                        result.Query += " '" + sFATerm + "', ";
                        result.Query += " '" + m_sPaymentMode + "', ";
                        result.Query += " '" + m_sPaymentType + "', ";
                        result.Query += "to_date('" + sDatePosted + "','MM/dd/yyyy'), ";
                        result.Query += " '" + sTimePosted + "', ";
                        result.Query += " '" + txtTeller.Text.Trim() + "', ";
                        result.Query += " '" + txtTeller.Text.Trim() + "', ";
                        //result.Query += " '" + string.Empty + "') ";
                        result.Query += " '" + StringUtilities.HandleApostrophe(txtMemo.Text) + "') ";  // RMC 20151229 merged multiple OR use from Mati
                        if (result.ExecuteNonQuery() != 0)
                        {

                        }
                        result.Close();

                        TransLog.UpdateLog(m_sBIN, txtBnsStat.Text.Trim(), sFATaxYear, "CAP", m_dTransLogIn, AppSettingsManager.GetSystemDate());   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    }
                    #region comments
                    // RMC 20151229 merged multiple OR use from Mati (s)
                    /*if (m_iORNo > 1)
                    {
                        int iTmpOr = 0;
                        int.TryParse(txtORNo.Text, out iTmpOr);

                        for (int ix = 1; ix < m_iORNo; ix++)
                        {
                            iTmpOr++;
                            sORNo = string.Format("{0:#######}", iTmpOr);

                            
                            // AST 20160120 Added to avoid duplicate insertion on tbl pay_hist (s)
                            result.Query = string.Format("delete from pay_hist where or_no = '{0}' and teller = '{1}'", sORNo, txtTeller.Text);
                            result.ExecuteNonQuery();
                            // AST 20160120 Added to avoid duplicate insertion on tbl pay_hist (e)
                            
                            

                            result.Query = "insert into pay_hist(or_no,bin,bns_stat,tax_year,qtr_paid,or_date,payment_term,data_mode,payment_type,date_posted,time_posted,bns_user,teller,memo) values ";
                            result.Query += "('" + sORNo + "', ";
                            result.Query += " '" + m_sBIN + "', ";
                            result.Query += " '" + txtBnsStat.Text.Trim() + "', ";
                            result.Query += " '" + sFATaxYear + "', ";
                            result.Query += " '" + sFAQtrToPay + "', ";
                            result.Query += "to_date('" + sOrDate + "','MM/dd/yyyy'), ";
                            result.Query += " '" + sFATerm + "', ";
                            result.Query += " '" + m_sPaymentMode + "', ";
                            result.Query += " '" + m_sPaymentType + "', ";
                            result.Query += "to_date('" + sDatePosted + "','MM/dd/yyyy'), ";
                            result.Query += " '" + sTimePosted + "', ";
                            result.Query += " '" + txtTeller.Text.Trim() + "', ";
                            result.Query += " '" + txtTeller.Text.Trim() + "', ";
                            //result.Query += " '" + string.Empty + "') ";
                            result.Query += " '" + StringUtilities.HandleApostrophe(txtMemo.Text) + "') ";  // RMC 20151229 merged multiple OR use from Mati
                            if (result.ExecuteNonQuery() != 0)
                            {

                            }
                            result.Close();
                        }
                    }*/
                    // RMC 20160503 correction in saving payment using multiple OR, put rem
                    // RMC 20151229 merged multiple OR use from Mati (e)
                    #endregion
                    sFATaxYearPrev = sFATaxYear;
                    sFADueStatePrev = sFADueState;
                    sFAQtrToPayPrev = sFAQtrToPay;
                }
                else
                {
                    // RMC 20170106 added validation/insertion of multiple OR in pay_hist (s)
                    if (m_iORNo > 1)
                    {
                        OracleResultSet pCheck = new OracleResultSet();
                        if (m_iORNo > 1 && iPayHistCnt >= 14)
                        {
                            iPayHistOrNo++;
                            iPayHistCnt = 0;
                            //if (Convert.ToInt32(sTmpORNo) >= iPayHistOrNo)
                            //{
                            //    iPayHistOrNo++;
                            //    m_sTellerOR += iPayHistOrNo.ToString() + "-"; //JARS 20170823
                            //}
                        }
                        sPayHistOrNo = string.Format("{0:0######}", iPayHistOrNo);
                        pCheck.Query = "select * from pay_hist where or_no = '" + sPayHistOrNo + "'";
                        if (pCheck.Execute())
                        {
                            if (!pCheck.Read())
                            {
                                string sOrDate = string.Empty;
                                string sDatePosted = string.Empty;
                                string sTimePosted = string.Empty;
                                sOrDate = string.Format("{0:MM/dd/yyyy}", dtpORDate.Value);
                                sDatePosted = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetSystemDate());
                                sTimePosted = string.Format("{0:HH:mm}", AppSettingsManager.GetSystemDate());
                                result.Query = "insert into pay_hist(or_no,bin,bns_stat,tax_year,qtr_paid,or_date,payment_term,data_mode,payment_type,date_posted,time_posted,bns_user,teller,memo) values ";
                                result.Query += "('" + sPayHistOrNo + "', ";
                                result.Query += " '" + m_sBIN + "', ";
                                result.Query += " '" + txtBnsStat.Text.Trim() + "', ";
                                result.Query += " '" + sFATaxYear + "', ";
                                result.Query += " '" + sFAQtrToPay + "', ";
                                result.Query += "to_date('" + sOrDate + "','MM/dd/yyyy'), ";
                                result.Query += " '" + sFATerm + "', ";
                                result.Query += " '" + m_sPaymentMode + "', ";
                                result.Query += " '" + m_sPaymentType + "', ";
                                result.Query += "to_date('" + sDatePosted + "','MM/dd/yyyy'), ";
                                result.Query += " '" + sTimePosted + "', ";
                                result.Query += " '" + txtTeller.Text.Trim() + "', ";
                                result.Query += " '" + txtTeller.Text.Trim() + "', ";
                                result.Query += " '" + StringUtilities.HandleApostrophe(txtMemo.Text) + "') ";
                                if (result.ExecuteNonQuery() != 0)
                                {

                                }
                                result.Close();
                            }
                        }
                        pCheck.Close();

                    }
                }
                // RMC 20170106 added validation/insertion of multiple OR in pay_hist (e)

                if (sFAFeesCode.Substring(0, 1) == "B")
                {
                    sBnsCodeOR = sFAFeesCode.Substring(1, sFAFeesCode.Length - 1);
                    sFAFeesCode = "B";
                }
                else
                {
                    sBnsCodeOR = sFAFeesCode.Substring(3, sFAFeesCode.Length - 3);
                }
                sFeesCode = sFAFeesCode;

                if (sFADiscounted.Trim() == string.Empty)
                    sFADiscounted = "0";
                if (double.Parse(sFADiscounted) > 0)
                {
                    sFeesDue = sFADiscounted;
                    sFeesSurch = "0.00";
                    sFeesPen = "0.00";
                    sFeesAmtDue = sFADiscounted;
                }
                else
                {
                    sFeesDue = sFADue;
                    sFeesSurch = sFASurch;
                    sFeesPen = sFAPen;
                    sFeesAmtDue = sFATotal;
                }


                // RMC 20151229 merged multiple OR use from Mati (s)
                //if (iMultOrCnt == 14)
                //if (iMultOrCnt > 14)

                // RMC 20160119 corrections in use of multiple OR (s)
                if (iMultOrCnt > 14)    // RMC 20160119 corrections in use of multiple OR
                {
                    int iTmpOr = 0;
                    int.TryParse(sTmpORNo, out iTmpOr);

                    iTmpOr += 1;
                    //sTmpORNo = string.Format("{0:#######}", iTmpOr);
                    sTmpORNo = string.Format("{0:0######}", iTmpOr);    // RMC 20170106 correction in saving OR no. in multiple OR use

                    //iMultOrCnt = 0;
                    iMultOrCnt = 1; // RMC 20180102 correction in user of multiple OR
                }
                // RMC 20151229 merged multiple OR use from Mati (e)

                // RMC 20160119 corrections in use of multiple OR (e)


                 // result.Query = "insert into or_table(or_no,fees_code,fees_due,fees_surch,fees_pen,fees_amtdue,qtr_paid,bns_code_main,tax_year) values ";
                //result.Query += "('" + txtORNo.Text + "','";

                //JHB 2018120 MEGERE FROM LAL_LO //JARS 20181129 (S)
                result.Query = "insert into or_table(or_no,fees_code,fees_due,fees_surch,fees_pen,fees_amtdue,qtr_paid,bns_code_main,tax_year, declared_gross_cap) values ";
                result.Query += "('" + sTmpORNo + "','";    // RMC 20151229 merged multiple OR use from Mati
                if (sFeesCode == "B")
                    result.Query += sFeesCode + "', ";
                else
                    result.Query += sFeesCode.Substring(0, 2) + "', ";
                result.Query += double.Parse(sFeesDue).ToString() + ", ";
                result.Query += double.Parse(sFeesSurch).ToString() + ", ";
                result.Query += double.Parse(sFeesPen).ToString() + ", ";
                result.Query += double.Parse(sFeesAmtDue).ToString() + ", ";
                result.Query += " '" + sFAQtrToPay + "', ";
                result.Query += " '" + sBnsCodeOR + "', ";
                result.Query += " '" + sFATaxYear + "', ";
                if (sFeesCode == "B") //JARS 20181129
                {
                    result.Query += "'" + dDeclaredGross + "')";
                }
                else
                {
                    result.Query += "'')";
                }
                result.Query += "";
                //JHB 2018120 MEGERE FROM LAL_LO //JARS 20181129 (E)
                if (result.ExecuteNonQuery() != 0)
                {

                }
                else 
                {
                    MessageBox.Show("");
                }
                result.Close();

                // RMC 20151229 merged multiple OR use from Mati (s)
                //if (iMultOrCnt == 14)
                //if (iMultOrCnt > 14)
                //{
                //    int iTmpOr = 0;
                //    int.TryParse(sTmpORNo, out iTmpOr);
                /*if (iMultOrCnt > 14)    // RMC 20160119 corrections in use of multiple OR
                {
                    int iTmpOr = 0;
                    int.TryParse(sTmpORNo, out iTmpOr);

                //    iTmpOr += 1;
                //    sTmpORNo = string.Format("{0:#######}", iTmpOr);

                //    iMultOrCnt = 0;
                //}
                    iMultOrCnt = 0;
                }*/
                // RMC 20160119 corrections in use of multiple OR, put rem
                // RMC 20151229 merged multiple OR use from Mati (e)
            }


            if (chk1st.Checked == true)
                sQtrToPay = "1";
            if (chk2nd.Checked == true)
                sQtrToPay = "2";
            if (chk3rd.Checked == true)
                sQtrToPay = "3";
            if (chk4th.Checked == true)
                sQtrToPay = "4";

            result.Query = string.Format("select * from taxdues_hist where bin = '{0}' and tax_year = '{1}'", m_sBIN, txtTaxYear.Text);
            if (result.Execute())
            {
                if (!result.Read())
                {
                    result.Close();
                    result.Query = string.Format("insert into taxdues_hist select * from taxdues where bin = '{0}'", m_sBIN);
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                }
            }
            result.Close();

            string sTBIN, sTTaxYear, sTQtrToPay, sTBnsCodeMain, sTTaxCode, sTAmount, sTDueState, sTORNo;

            bool boCompOnly = false;
            string sTmpDueState;
            // pls check this
            sTmpDueState = sFADueState;
            if (sTmpDueState.Substring(0, 1) == "C")
                boCompOnly = true;

            result.Query = "select * from taxdues where bin = '" + m_sBIN.Trim() + "'";
            if (m_bExistTaxPU == true && m_sPaymentTerm == "F")
                result.Query += " and due_state = 'P'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sTBIN = result.GetString("bin").Trim();
                    sTTaxYear = result.GetString("tax_year").Trim();
                    sTQtrToPay = result.GetString("qtr_to_pay").Trim();
                    sTBnsCodeMain = result.GetString("bns_code_main").Trim();
                    sTTaxCode = result.GetString("tax_code").Trim();
                    sTAmount = result.GetDouble("amount").ToString();
                    sTDueState = result.GetString("due_state").Trim();
                    sTORNo = txtORNo.Text.Trim();

                    result2.Query = "insert into taxdues_last(bin,tax_year,qtr_to_pay,bns_code_main,tax_code,amount,due_state,or_no) values ";
                    result2.Query += "('" + sTBIN + "', ";
                    result2.Query += " '" + sTTaxYear + "', ";
                    result2.Query += " '" + sTQtrToPay + "', ";
                    result2.Query += " '" + sTBnsCodeMain + "', ";
                    result2.Query += " '" + sTTaxCode + "', ";
                    result2.Query += sTAmount + ", ";
                    result2.Query += " '" + sTDueState + "', ";
                    result2.Query += " '" + sTORNo + "') ";
                    if (result2.ExecuteNonQuery() != 0)
                    {
                    }
                    result2.Close();
                }
            }
            result.Close();

            string sBillNo, sBillDate, sBillUser, sGracePeriod, sReceivedDate, sReceivedBy;
            result.Query = "select * from bill_no where bin = '" + m_sBIN.Trim() + "'";
            if (m_bExistTaxPU == true && m_sPaymentTerm == "F")
                result.Query += " and due_state = 'P'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sTBIN = result.GetString("bin").Trim();
                    sTTaxYear = result.GetString("tax_year").Trim();
                    sTBnsCodeMain = result.GetString("bns_code_main").Trim();
                    sBillNo = result.GetString("bill_no").Trim();
                    sBillDate = result.GetDateTime("bill_date").ToShortDateString();
                    sBillUser = result.GetString("bill_user").Trim();
                    sGracePeriod = result.GetInt("grace_period").ToString();
                    sReceivedDate = result.GetDateTime("received_date").ToShortDateString();
                    sReceivedBy = result.GetString("received_by").Trim();
                    sTQtrToPay = result.GetString("qtr").Trim();
                    sTDueState = result.GetString("due_state").Trim();
                    sTORNo = txtORNo.Text.Trim();

                    sBillDate = string.Format("{0:MM/dd/yyyy}", DateTime.Parse(sBillDate));
                    sReceivedDate = string.Format("{0:MM/dd/yyyy}", DateTime.Parse(sReceivedDate));
                    result2.Query = "insert into bill_no_last values ";
                    result2.Query += "('" + sTBIN + "', ";
                    result2.Query += " '" + sTTaxYear + "', ";
                    result2.Query += " '" + sTBnsCodeMain + "', ";
                    result2.Query += " '" + sBillNo + "', ";
                    result2.Query += "to_date('" + sBillDate + "','MM/dd/yyyy'), ";
                    result2.Query += " '" + sBillUser + "', ";
                    result2.Query += sGracePeriod + ", ";
                    result2.Query += "to_date('" + sReceivedDate + "','MM/dd/yyyy'), ";
                    result2.Query += " '" + sReceivedBy + "', ";
                    result2.Query += " '" + sTQtrToPay + "', ";
                    result2.Query += " '" + sTDueState + "', ";
                    result2.Query += " '" + sTORNo + "') ";
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();
                }
            }
            result.Close();


            //AFM 20200508 check if multipay (s)
            result.Query = string.Format("select chk_no from chk_tbl_temp where chk_no in (select chk_no from multi_check_pay) and or_no = '{0}'", txtORNo.Text);
            if (result.Execute())
                if (result.Read())
                    bGuarantor = true;
            result.Close();
            //AFM 20200508 check if multipay (e)

            string sChkNo = "";
            result.Query = "select * from chk_tbl_temp where or_no = '" + txtORNo.Text.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sChkNo = result.GetString("chk_no").Trim();

                    // GUARANTOR ASAP
                    if (bGuarantor == true)
                    {
                        result2.Query = string.Format("update multi_check_pay set used_sw = 'Y' where chk_no = '{0}'", sChkNo);
                        result2.ExecuteNonQuery();
                    }

                    result2.Query = "insert into chk_tbl select * from chk_tbl_temp where or_no = '" + txtORNo.Text.Trim() + "'";
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();

                    result2.Query = "delete from chk_tbl_temp where or_no = '" + txtORNo.Text.Trim() + "'";
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();
                }


            }
            result.Close();

            string sCheckNo, sQuery = "";
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            if (m_sFlag == "Y")
            {
                pSet.Query = string.Format("select * from chk_tbl where or_no = '{0}'", txtORNo.Text.Trim());
                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        sCheckNo = pSet.GetString("chk_no").Trim();
                        pSet1.Query = string.Format("select * from dbcr_memo where chk_no = '{0}' and served = 'N'", sCheckNo); //JARS 20171010
                        if (pSet1.Execute())
                            if (pSet1.Read())
                            {
                                pSet1.Query = string.Format("update dbcr_memo set served = 'Y' where chk_no = '{0}' and served = 'N'", sCheckNo); //JARS 20171010
                                pSet1.ExecuteNonQuery();
                            }
                            else
                            {
                                String sDebit, sCredit, sBalance, sMemo, sServe;

                                String m_sOWNCODE;
                                m_sOWNCODE = m_sOwnCode;

                                if (m_sOwnCode != m_sCheckOwnerCode && m_sCheckOwnerCode != "")
                                    m_sOWNCODE = m_sCheckOwnerCode;
                                sDebit = "0";
                                sCredit = m_sDebitCredit;
                                sBalance = m_sDebitCredit;

                                sServe = "N";
                                sMemo = "CREDITED IN EXCESS TO CHECK PAYMENT/S HAVING OR_NO " + txtORNo.Text.Trim();
                                sQuery = "insert into dbcr_memo values "; //JARS 20171010
                                // RMC 20141127 QA Tarlac ver (s)
                                //if (AppSettingsManager.GetConfigValue("10") == "017")
                                //   sQuery += "('" + m_sOWNCODE + "', ";
                                //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                                //    sQuery += "('" + m_sOWNCODE + "', ";
                                //else// RMC 20141127 QA Tarlac ver (e)
                                    sQuery += "('" + m_sBIN + "', ";
                                sQuery += " '" + txtORNo.Text.Trim() + "', ";
                                string sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                                sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                                sQuery += Convert.ToDouble(sDebit).ToString("##0.00") + ", ";
                                sQuery += Convert.ToDouble(sCredit).ToString("##0.00") + ", ";
                                sQuery += Convert.ToDouble(sBalance).ToString("##0.00") + ", ";
                                sQuery += " '" + sMemo.Trim() + "', ";
                                sQuery += "' ',";
                                sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                                sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                                sTextFormat = dtpORDate.Value.ToString("HH:m");
                                sQuery += "'" + sTextFormat + "',";
                                sQuery += "'" + sServe + "',";
                                sQuery += "'" + sCheckNo.Trim() + "',";
                                sQuery += "'" + m_sMultiPay + "','0')";
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();

                                pSet1.Query = string.Format("update dbcr_memo set served = 'Y' where chk_no = '{0}' and served = 'N'", sCheckNo); //JARS 20171010
                                pSet1.ExecuteNonQuery();

                                sDebit = "0";

                                if (Convert.ToDouble(sBalance) == 0)
                                    sServe = "Y";
                                else
                                    sServe = "N";

                                sMemo = "REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR " + txtORNo.Text.Trim();
                                sQuery = "insert into dbcr_memo values "; //JARS 20171010
                                // RMC 20141127 QA Tarlac ver (s)
                                //if (AppSettingsManager.GetConfigValue("10") == "017")
                                //    sQuery += "('" + m_sOWNCODE + "', ";
                                //else if(AppSettingsManager.GetConfigValue("10") == "216")
                                //    sQuery += "('" + m_sOWNCODE + "', ";
                                //else// RMC 20141127 QA Tarlac ver (e)
                                    sQuery += "('" + m_sBIN + "', ";
                                sQuery += " '" + txtORNo.Text.Trim() + "', ";
                                sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                                sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                                sQuery += Convert.ToDouble(sDebit).ToString("##0.00") + ", ";
                                sQuery += Convert.ToDouble(sCredit).ToString("##0.00") + ", ";
                                sQuery += Convert.ToDouble(sBalance).ToString("##0.00") + ", ";
                                sQuery += " '" + sMemo.Trim() + "', ";
                                sQuery += "' ',";
                                sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                                sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                                sTextFormat = dtpORDate.Value.ToString("HH:m");
                                sQuery += "'" + sTextFormat + "',";
                                sQuery += "'" + sServe + "',";
                                sQuery += "'" + sCheckNo + "',";
                                sQuery += "'" + m_sMultiPay + "','0')";
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();

                            }
                        pSet1.Close();
                    }
                pSet.Close();
            }
            else
            {
                if (Convert.ToDouble(txtToTaxCredit.Text) > 0.00 || Convert.ToDouble(m_sToBeDebited) > 0.00)
                {
                    if (Convert.ToDouble(txtToTaxCredit.Text) > 0.00 && bGuarantor == true)
                        DebitTaxCredit(m_sBIN);

                    String m_sOWNCODE;
                    m_sOWNCODE = m_sOwnCode;

                    if (m_sOwnCode != m_sCheckOwnerCode && m_sCheckOwnerCode != "")
                        m_sOWNCODE = m_sCheckOwnerCode;

                    if (Convert.ToDouble(m_sToBeDebited) > 0.00)
                        txtToTaxCredit.Text = m_sToBeDebited;

                    if (bGuarantor == true && bRefCheckNo == true)
                    {
                        // RMC 20141127 QA Tarlac ver (s)
                        //if (AppSettingsManager.GetConfigValue("10") == "017")
                        //    pSet.Query = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N'", m_sOWNCODE);
                        //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                        //    pSet.Query = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N'", m_sOWNCODE);
                        //else// RMC 20141127 QA Tarlac ver (e)
                            pSet.Query = string.Format("select * from dbcr_memo where bin = '{0}' and served = 'N'", m_sBIN); //JARS 20171010
                        if (pSet.Execute())
                            if (pSet.Read())
                                m_sCreditLeft = pSet.GetDouble("balance").ToString();
                        pSet.Close();
                    }

                    pSet.Query = string.Format("select * from multi_check_pay where chk_no in (select chk_no from chk_tbl where or_no = '{0}')", txtORNo.Text);
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            m_sRefCheckNo = pSet.GetString("chk_no").Trim();
                            m_sMultiPay = "Y";
                        }
                    pSet.Close();

                    // RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    pSet.Query = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}' and served = 'N'", m_sOWNCODE);
                    //else if(AppSettingsManager.GetConfigValue("10") == "216")
                    //    pSet.Query = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}' and served = 'N'", m_sOWNCODE);
                    //else// RMC 20141127 QA Tarlac ver (E)
                        pSet.Query = string.Format("update dbcr_memo set served = 'Y' where bin = '{0}' and served = 'N'", m_sBIN); //JARS 20171010
                    pSet.ExecuteNonQuery();

                    String sDebit, sCredit, sBalance, sMemo, sServe;
                    if (m_sPaymentType == "CC" && bGuarantor == true && bRefCheckNo == true)
                        sDebit = m_sCreditLeft;
                    else
                        sDebit = txtToTaxCredit.Text;
                    sCredit = "0";

                    sBalance = Convert.ToDouble(Convert.ToDouble(sCredit) - Convert.ToDouble(sDebit)).ToString("##0.00");

                    sServe = "Y";
                    sMemo = "DEBITED THRU PAYMENT MADE HAVING OR_NO " + txtORNo.Text.Trim();
                    sQuery = "insert into dbcr_memo values "; //JARS 20171010
                    // RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    sQuery += "('" + m_sOWNCODE + "', ";
                    //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                    //    sQuery += "('" + m_sOWNCODE + "', ";
                    //else// RMC 20141127 QA Tarlac ver (e)
                        sQuery += "('" + m_sBIN + "', ";
                    sQuery += " '" + txtORNo.Text + "', ";
                    string sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sQuery += Convert.ToDouble(sDebit).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sCredit).ToString("##0.00") + ", ";
                    sQuery += sBalance + ", ";
                    sQuery += " '" + StringUtilities.HandleApostrophe(sMemo.Trim()) + "', ";
                    sQuery += "' ',";
                    sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sTextFormat = dtpORDate.Value.ToString("HH:m");
                    sQuery += "'" + sTextFormat + "',";
                    sQuery += "'" + sServe + "',";
                    sQuery += "'" + m_sRefCheckNo + "',";
                    sQuery += "'" + m_sMultiPay + "','0')";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    // RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    pSet.Query = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}'", m_sOWNCODE);
                    //else if (AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                    //    pSet.Query = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}'", m_sOWNCODE);
                    //else// RMC 20141127 QA Tarlac ver (e)
                        pSet.Query = string.Format("update dbcr_memo set served = 'Y' where bin = '{0}'", m_sBIN); //JARS 20171010
                    pSet.ExecuteNonQuery();

                    sDebit = "0";

                    if (m_sPaymentType == "CC" && bGuarantor == true && bRefCheckNo == true)
                        sCredit = "0";
                    else
                        sCredit = Convert.ToDouble(Convert.ToDouble(m_sCreditLeft) - Convert.ToDouble(txtToTaxCredit.Text)).ToString();

                    sBalance = Convert.ToDouble(Convert.ToDouble(sCredit) - Convert.ToDouble(sDebit)).ToString();
                    if (Convert.ToDouble(sBalance) == 0)
                        sServe = "Y";
                    else
                        sServe = "N";

                    sMemo = "REMAINING BALANCE AFTER PAYMENT W/ DEBIT HAVING OR " + txtORNo.Text;
                    sQuery = "insert into dbcr_memo values "; //JARS 20171010
                    // RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    sQuery += "('" + m_sOWNCODE + "', ";
                    //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                    //    sQuery += "('" + m_sOWNCODE + "', ";
                    //else// RMC 20141127 QA Tarlac ver (e)
                        sQuery += "('" + m_sBIN + "', ";
                    sQuery += " '" + txtORNo.Text + "', ";
                    sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sQuery += sDebit + ", ";
                    sQuery += sCredit + ", ";
                    sQuery += sBalance + ", ";
                    sQuery += " '" + StringUtilities.HandleApostrophe(sMemo.Trim()) + "', ";
                    sQuery += "' ',";
                    sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sTextFormat = dtpORDate.Value.ToString("HH:m");
                    sQuery += "'" + sTextFormat + "',";
                    sQuery += "'" + sServe + "',";
                    sQuery += "'" + m_sRefCheckNo + "',";
                    sQuery += "'" + m_sMultiPay + "','0')";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    String sIrregType, sCashAmt, sCheckAmt, sCreditAmt, sDebitAmt;
                    String sPCashAmt, sPChkAmt, sPRemAmt, sPStatus;
                    sIrregType = "CQ-DR";
                    sCheckAmt = "0.00";
                    sCashAmt = "0.00";
                    sCreditAmt = "0.00";
                    sDebitAmt = txtToTaxCredit.Text;
                    sChkNo = "";
                    sPCashAmt = "0";
                    sPChkAmt = "0";
                    sPRemAmt = "0";
                    sPStatus = ".";

                    sQuery = "insert into irreg_payment(bin,or_no,or_date,irreg_type,cash_amt,check_amt,credit_amt,debit_amt,chk_no,part_cash_amt,part_chk_amt,part_rem_amt,part_stat) values ";
                    sQuery += "('" + m_sBIN + "', ";
                    sQuery += " '" + txtORNo.Text + "', ";
                    sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sQuery += " '" + sIrregType + "', ";
                    sQuery += Convert.ToDouble(sCashAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sCheckAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sCreditAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sDebitAmt).ToString("##0.00") + ", ";
                    sQuery += " '" + sChkNo + "', ";
                    sQuery += Convert.ToDouble(sPCashAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sPChkAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sPRemAmt).ToString("##0.00") + ", ";
                    sQuery += " '" + sPStatus + "') ";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
                ////********************************

                if (Convert.ToDouble(m_sDebitCredit) > 0.00 && chkCheque.Checked == true)
                {
                    if (bGuarantor == true)
                        m_sMultiPay = "Y";
                    else
                        m_sMultiPay = "N";

                    if (bGuarantor == true && bRefCheckNo == false)
                    {
                        pSet.Query = string.Format("select chk_no from chk_tbl where chk_no in (select chk_no from multi_check_pay) and or_no = '{0}'", txtORNo.Text);
                        if (pSet.Execute())
                            if (pSet.Read())
                                sChkNo = pSet.GetString("chk_no").Trim();
                        pSet.Close();

                        pSet.Query = string.Format("update multi_check_pay set used_sw = 'Y' where chk_no = '{0}'", sChkNo);
                        pSet.ExecuteNonQuery();
                    }
                    if (m_sOwnCode == m_sCheckOwnerCode)
                        MessageBox.Show("About to create credit memo", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    String m_sOWNCODE;
                    m_sOWNCODE = m_sOwnCode;

                    if (m_sOwnCode != m_sCheckOwnerCode && m_sCheckOwnerCode != "")
                        m_sOWNCODE = m_sCheckOwnerCode;

                    String sDebit, sCredit, sBalance, sMemo, sServe;
                    sDebit = "0";
                    sCredit = m_sDebitCredit;
                    sBalance = m_sDebitCredit;

                    sServe = "N";
                    sMemo = "CREDITED IN EXCESS TO CHECK PAYMENT/S HAVING OR_NO " + txtORNo.Text;
                    sQuery = "insert into dbcr_memo values "; //JARS 20171010
                    // RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    sQuery += "('" + m_sOWNCODE + "', ";
                    //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                    //    sQuery += "('" + m_sOWNCODE + "', ";
                    //else// RMC 20141127 QA Tarlac ver (e)
                        sQuery += "('" + m_sBIN + "', ";
                    sQuery += " '" + txtORNo.Text + "', ";
                    string sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sQuery += Convert.ToDouble(sDebit).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sCredit).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sBalance).ToString("##0.00") + ", ";
                    sQuery += " '" + sMemo.Trim() + "', ";
                    sQuery += "' ',";
                    sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sTextFormat = dtpORDate.Value.ToString("HH:m");
                    sQuery += "'" + sTextFormat + "',";
                    sQuery += "'" + sServe + "',";
                    sQuery += "'" + sChkNo.Trim() + "',";
                    sQuery += "'" + m_sMultiPay + "','0')";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    // RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    pSet.Query = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}'", m_sOWNCODE);
                    //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                    //    pSet.Query = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}'", m_sOWNCODE);
                    //else// RMC 20141127 QA Tarlac ver (e)
                    pSet.Query = string.Format("update dbcr_memo set served = 'Y' where bin = '{0}'", m_sBIN); //JARS 20171010
                    pSet.ExecuteNonQuery();

                    sDebit = "0";
                    sBalance = Convert.ToDouble(Convert.ToDouble(sCredit) - Convert.ToDouble(sDebit)).ToString();
                    if (Convert.ToDouble(sBalance) == 0)
                        sServe = "Y";
                    else
                        sServe = "N";

                    sMemo = "REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR " + txtORNo.Text;
                    sQuery = "insert into dbcr_memo values "; //JARS 20171010
                    // RMC 20141127 QA Tarlac ver (s)
                    //if (AppSettingsManager.GetConfigValue("10") == "017")
                    //    sQuery += "('" + m_sOWNCODE + "', ";
                    //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                    //    sQuery += "('" + m_sOWNCODE + "', ";
                    //else// RMC 20141127 QA Tarlac ver (e)
                        sQuery += "('" + m_sBIN + "', ";
                    sQuery += " '" + txtORNo.Text + "', ";
                    sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sQuery += Convert.ToDouble(sDebit).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sCredit).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sBalance).ToString("##0.00") + ", ";
                    sQuery += " '" + sMemo.Trim() + "', ";
                    sQuery += "' ',";
                    sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sTextFormat = dtpORDate.Value.ToString("HH:m");
                    sQuery += "'" + sTextFormat + "',";
                    sQuery += "'" + sServe + "',";
                    sQuery += "'" + sChkNo + "',";
                    sQuery += "'" + m_sMultiPay + "','0')";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    String sIrregType, sCashAmt, sCheckAmt, sCreditAmt, sDebitAmt;
                    String sPCashAmt, sPChkAmt, sPRemAmt, sPStatus;
                    sIrregType = "CQ-CR";
                    sCheckAmt = "0.00";
                    sCashAmt = "0.00";
                    sCreditAmt = m_sDebitCredit;
                    sDebitAmt = "0.00";
                    sChkNo = "";
                    sPCashAmt = "0";
                    sPChkAmt = "0";
                    sPRemAmt = "0";
                    sPStatus = ".";

                    sQuery = "insert into irreg_payment(bin,or_no,or_date,irreg_type,cash_amt,check_amt,credit_amt,debit_amt,chk_no,part_cash_amt,part_chk_amt,part_rem_amt,part_stat) values ";
                    sQuery += "('" + m_sBIN + "', ";
                    sQuery += " '" + txtORNo.Text + "', ";
                    sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sQuery += " '" + sIrregType + "', ";
                    sQuery += Convert.ToDouble(sCashAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sCheckAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sCreditAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sDebitAmt).ToString("##0.00") + ", ";
                    sQuery += " '" + sChkNo + "', ";
                    sQuery += Convert.ToDouble(sPCashAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sPChkAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sPRemAmt).ToString("##0.00") + ", ";
                    sQuery += " '" + sPStatus.Trim() + "') ";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }

                if (m_sPaymentType == "CC")
                {
                    //Saving to irreg_payment (s)
                    String sCashAmt;  //JHB 20180607 (s)
                    sCashAmt = m_sCCCashAmt;

                   // String sIrregType, sPCashAmt, sPChkAmt, sPRemAmt, sPStatus, sCheckAmt, sCreditAmt, sDebitAmt;
                   /*sIrregType = "CC";
                    sCheckAmt = m_sCCChkAmt;
                    sCashAmt = m_sCCCashAmt;
                    sCreditAmt = "0.00";
                    sDebitAmt = "0.00";
                    sChkNo = "";
                    sPCashAmt = "0";
                    sPChkAmt = "0";
                    sPRemAmt = "0";
                    sPStatus = ".";   --  */

                    sQuery = "update chk_tbl set cash_amt  = '" + Convert.ToDouble(sCashAmt).ToString("##0.00") +"' ";
                    sQuery += "where or_no = '" + txtORNo.Text.Trim() + "' ";  //JHB 20180607 (e) display cash amt in RCD report w/ CC payment 
                    /*sQuery = "insert into irreg_payment(bin,or_no,or_date,irreg_type,cash_amt,check_amt,credit_amt,debit_amt,chk_no,part_cash_amt,part_chk_amt,part_rem_amt,part_stat) values ";
                    sQuery += "('" + m_sBIN + "', ";
                    sQuery += " '" + txtORNo.Text.Trim() + "', ";
                    string sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                    sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'), ";
                    sQuery += " '" + sIrregType + "', ";
                    sQuery += Convert.ToDouble(sCashAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sCheckAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sCreditAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sDebitAmt).ToString("##0.00") + ", ";
                    sQuery += " '" + sChkNo + "', ";
                    sQuery += Convert.ToDouble(sPCashAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sPChkAmt).ToString("##0.00") + ", ";
                    sQuery += Convert.ToDouble(sPRemAmt).ToString("##0.00") + ", ";
                    sQuery += " '" + sPStatus + "') ";  */

                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
            }

            // gde 20110216 skip codes 539 lines

            // RMC 20150102 enabled special business (s)
            String sQuerySplBns;
            bool blnIsSplBns = false;
            sQuerySplBns = string.Format("select * from spl_business_que where bin = '{0}' ", m_sBIN);
            result.Query = sQuerySplBns;
            if (result.Execute())
                if (result.Read())
                    blnIsSplBns = true;
            result.Close();
            // RMC 20150102 enabled special business (e)

            if (sDueState == "R" || sDueState == "C")
                sBnsStat = "REN";
            else
            {
                if (sDueState == "X")
                    sBnsStat = "RET";
                //else
                else if (sDueState == "N")	// RMC 20150907 corrections in updating bns stat if with rev exam adj, merged from Calamba
                    sBnsStat = "NEW";
                else
                    sBnsStat = "REN";	// RMC 20150907 corrections in updating bns stat if with rev exam adj, merged from Calamba
            }

            string sTempTaxYear = string.Empty;
            sTempTaxYear = string.Format("{0:0}", int.Parse(txtTaxYear.Text.Trim()) - 1);
            if (blnIsSplBns == true)
                result.Query = "select * from spl_business_que where bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
            else
                result.Query = "select * from businesses where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'"; // try lang
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();
                    if (blnIsSplBns == true)
                        result.Query = "select * from spl_business_que where bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                    else
                        //result.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'"; // try lang
                        result.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year >= '" + txtTaxYear.Text.Trim() + "'"; // try lang   // RMC 20170119 correction in saving retirement of delinquent after payment 
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            if (blnIsSplBns == true)
                                result2.Query = "select * from spl_business_que where bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                            else
                            //result2.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'"; //try lang
                                result2.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year >= '" + txtTaxYear.Text.Trim() + "'";  // RMC 20170119 correction in saving retirement of delinquent after payment 
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    sBnsStat = result2.GetString("bns_stat");
                                }
                            }
                            result2.Close();

                            if (sBnsStat == "RET" && chkRetirement.Checked == true)
                            {
                                // GDE 20110216 
                                SaveRetirement(); 
                                result2.Query = "delete from business_que where bin = '" + m_sBIN.Trim() + "'";
                                if (result2.ExecuteNonQuery() != 0)
                                {

                                }
                                result2.Close();
                            }
                            else if (sBnsStat != "RET")
                            {
                                if (blnIsSplBns == true)
                                {
                                    result2.Query = "insert into buss_hist select * from spl_businesses where bin = '" + m_sBIN + "'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }

                                    result2.Query = "delete from spl_businesses where bin = '"+m_sBIN+"'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }

                                    result2.Query = "insert into spl_businesses select * from spl_business_que where bin = '" + m_sBIN + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }

                                    result2.Query = "delete from spl_business_que where bin = '" + m_sBIN.Trim() + "'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }
                                }
                                else
                                {
                                    result2.Query = "insert into buss_hist select * from businesses where bin = '" + m_sBIN.Trim() + "'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result2.Close();
                                    result2.Query = "delete from businesses where bin = '" + m_sBIN.Trim() + "'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result2.Close();
                                    //result2.Query = "insert into businesses select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'"; // try lang
                                    //result2.Query = "insert into businesses select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year => '" + txtTaxYear.Text.Trim() + "'";   // RMC 20170119 correction in saving retirement of delinquent after payment 
                                    result2.Query = "insert into businesses select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year >= '" + txtTaxYear.Text.Trim() + "'";  //AFM 20200611 fixed proper operator >=
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result2.Close();
                                    result2.Query = "delete from business_que where bin = '" + m_sBIN.Trim() + "'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result2.Close();
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Close();
                    //result.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'"; // try lang
                    result.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year >= '" + txtTaxYear.Text.Trim() + "'";   // RMC 20170301 corrections in retirement application
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            //result2.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'"; // try lang
                            result2.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year >= '" + txtTaxYear.Text.Trim() + "'"; // RMC 20170301 corrections in retirement application
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    sBnsStat = result2.GetString("bns_stat");
                                }
                            }
                            result2.Close();

                            if (sBnsStat == "RET")
                            {
                                result2.Query = "select * from retired_bns_temp where bin = '" + m_sBIN.Trim() + "'";
                                //result2.Query += " and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                                //result2.Query += " and tax_year >= '" + txtTaxYear.Text.Trim() + "'";   // RMC 20170116 correction in payment of retirement application with delinq   // RMC 20170119 correction in saving retirement of delinquent after payment, put rem
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        SaveRetirement();

                                        // RMC 20170301 corrections in retirement application (s)
                                        result2.Query = "delete from business_que where bin = '" + m_sBIN.Trim() + "'";
                                        if (result2.ExecuteNonQuery() != 0)
                                        {

                                        }
                                        result2.Close();
                                        // RMC 20170301 corrections in retirement application (e)
                                    }
                                   
                                }
                                result2.Close();
                            }
                            else
                            {
                                result2.Query = "insert into buss_hist select * from businesses where bin = '" + m_sBIN.Trim() + "'";
                                if (result2.ExecuteNonQuery() != 0)
                                {

                                }
                                result2.Close();

                                result2.Query = "delete from businesses where bin = '" + m_sBIN.Trim() + "'";
                                if (result2.ExecuteNonQuery() != 0)
                                {

                                }
                                result2.Close();
                                result2.Query = "insert into businesses select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'"; // try lang
                                if (result2.ExecuteNonQuery() != 0)
                                {

                                }
                                result2.Close();

                                result2.Query = "select * from retired_bns_temp where bin = '" + m_sBIN.Trim() + "'";
                                //result2.Query += " and tax_year = '" + txtTaxYear.Text.Trim() + "'"; // try lang
                                //result2.Query += " and tax_year >= '" + txtTaxYear.Text.Trim() + "'";   // RMC 20170116 correction in payment of retirement application with delinq   // RMC 20170119 correction in saving retirement of delinquent after payment, put rem
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        // GDE 20110216 
                                        SaveRetirement();
                                    }
                                }
                                result2.Close();
                            }

                            result2.Query = "delete from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'"; // try lang
                            if (result2.ExecuteNonQuery() != 0)
                            {

                            }
                        }
                        else
                        {
                            // RMC 20150129 (s)
                            // for discovery delinq
                            result2.Query = "select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year < '" + txtTaxYear.Text.Trim() + "' and bns_stat = 'NEW'";
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    result2.Close();

                                    result2.Query = "insert into buss_hist select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year < '" + txtTaxYear.Text.Trim() + "' and bns_stat = 'NEW'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }

                                    result2.Query = "insert into businesses select * from business_que where bin = '" + m_sBIN.Trim() + "' and tax_year < '" + txtTaxYear.Text.Trim() + "' and bns_stat = 'NEW'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }

                                    result2.Query = "update businesses set tax_year = '" + txtTaxYear.Text.Trim() + "', bns_stat = 'REN' where bin = '" + m_sBIN.Trim() + "'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }

                                    result2.Query = "delete from business_que where bin = '" + m_sBIN.Trim() + "'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }

                                    // RMC 20170119 correction in saving retirement of delinquent after payment (s)
                                    result2.Query = "select * from retired_bns_temp where bin = '" + m_sBIN.Trim() + "'";
                                    if (result2.Execute())
                                    {
                                        if (result2.Read())
                                        {
                                            SaveRetirement();
                                        }
                                    }
                                    result2.Close();
                                    // RMC 20170119 correction in saving retirement of delinquent after payment (e)
                                }
                                else
                                {
                                    result2.Close();
                             // RMC 20150129 (e)
                                    result2.Query = "insert into buss_hist select * from businesses where bin = '" + m_sBIN.Trim() + "'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }

                                    result2.Query = "update businesses set tax_year = '" + txtTaxYear.Text.Trim() + "', bns_stat = '" + sBnsStat + "' where bin = '" + m_sBIN.Trim() + "'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }

                                    // RMC 20180126 additional checking for retirement saving in Payment (s)
                                    if (sBnsStat == "RET")
                                    {
                                        result2.Query = "select * from retired_bns_temp where bin = '" + m_sBIN.Trim() + "'";
                                        if (result2.Execute())
                                        {
                                            if (result2.Read())
                                            {
                                                SaveRetirement();
                                            }
                                        }
                                        result2.Close();
                                    }
                                    // RMC 20180126 additional checking for retirement saving in Payment (e)
                                }
                            }
                            result2.Close();
                        } ////// 
                    }
                }

            }
            result.Close();

            // GDE 20110414 try lng
            if (sDueState == "P")
                sBnsStat = "REN";
            // GDE 20110414 try lng

            if (m_bExistTaxPU == true || m_bIsRenPUT == true) // ALJ 01032006 PUT for REN App -  || m_bIsRenPUT == TRUE
            {
                TablesTransferPUT(); // ALJ 01082006 PERMIT UPDATE TRAN tables transfer
            }
            InsertAddlBns();// RMC 20170110 added insertion of NEW addl line of business included in REN application

            if (txtBnsStat.Text == "RET")
            {
                int iQtr = 0;

                result.Query = "select * from taxdues where bin = '" + m_sBIN.Trim() + "' and due_state <> 'X'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        iQtr = Convert.ToInt32(result.GetString("qtr_to_pay")); // check this pls gde
                        if (iQtr > 1)
                            m_sPaymentTerm = "I";
                    }

                }
                result.Close();
            }

            if (m_sPaymentTerm == "F")
            {
                //JHB 20181221 merge from BIÑAN JARS 20180801 ATTEMPT AT DELETING ONLY TAXDUES (ISSUE WITH PAYING REVENUE ADJUSTMENT FIRST AND THEN PAYING INSTALLMENT FEES)
                #region comments
                //result.Query = "delete from taxdues where bin = '" + m_sBIN.Trim() + "' and tax_year <= '" + txtTaxYear.Text.Trim() + "'";
                //if (m_bExistTaxPU == true)
                //{
                //    result.Query += " and due_state = 'P'";
                //}
                //if (chkAdj.Checked == true)
                //    result.Query += " and due_state <> 'A'";
                //if (int.Parse(m_sNextQtrToPay) > 1 && sBnsStat != "NEW")
                //    result.Query += " and qtr_to_pay = 1";
                //if (result.ExecuteNonQuery() != 0)
                //{

                //}
                //result.Close();
                #endregion
                //JARS 20180914 NEW WAY OF DELETING TAXDUES (IF ADJUSTMENT, DELETE ONLY ADJUSTMENT, ETC...)
                result.Query = "delete from taxdues where bin = '" + m_sBIN + "' and tax_year <= '" + txtTaxYear.Text.Trim() + "'";
                result.Query += "and qtr_to_pay >= '1'";
                if (chkAdj.Checked)
                {
                    result.Query += " and due_state in ('A')"; //IF THE BIN HAS REMAINING DUES (INSTALLMENT) AND YOU ONLY TRANSACT THE REV ADJUSTMENT (FULL) 
                }
                if (result.ExecuteNonQuery() != 0)
                { }
                //JHB 20181221 megre from BIÑAN JARS 20180914 (E)



                result.Query = "delete from bill_no where bin = '" + m_sBIN.Trim() + "' and tax_year <= '" + txtTaxYear.Text.Trim() + "'";
                if (m_bExistTaxPU == true)
                {
                    result.Query += " and due_state = 'P'";
                }
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();

                chkRetirement.Checked = false;

            }
            else
            {
                //if (m_bExistTaxPU == true)
                if (m_bExistTaxPU == true && AppSettingsManager.GetConfigValue("10") != "243")  // RMC 20170216 added btax qtrly payment if with permit-update transaction 
                {
                    result.Query = "delete from taxdues where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                    result.Query += " and due_state = 'P'";
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                    result.Close();

                    result.Query = "delete from bill_no where bin = '" + m_sBIN.Trim() + "' and tax_year <= '" + txtTaxYear.Text.Trim() + "' and due_state = 'P'";
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                    result.Close();
                }

                

                string sTaxDueYear;
                result.Query = "select * from taxdues where bin = '" + m_sBIN.Trim() + "' and tax_year <= '" + txtTaxYear.Text.Trim() + "' order by tax_year,qtr_to_pay";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        sTaxDueYear = result.GetString("tax_year").Trim();
                        if (sTaxDueYear != AppSettingsManager.GetSystemDate().Year.ToString() && AppSettingsManager.MultiTaxYear(m_sBIN) > 1)
                        {
                            AppSettingsManager.DeleteTaxDues(m_sBIN.Trim(), sTaxDueYear.Trim());
                            AppSettingsManager.DeleteBillNo(m_sBIN.Trim(), sTaxDueYear.Trim());
                        }
                        else
                        {
                            if (m_sFullPartial == "F")
                                AppSettingsManager.DeleteTaxDues(m_sBIN.Trim(), sTaxDueYear.Trim());
                            else
                            {
                                if (sQtrToPay == "1")
                                    AppSettingsManager.UpdateTaxDues("2", m_sBIN, sTaxDueYear);
                                if (sQtrToPay == "2")
                                    AppSettingsManager.UpdateTaxDues("3", m_sBIN, sTaxDueYear);
                                if (sQtrToPay == "3")
                                    AppSettingsManager.UpdateTaxDues("4", m_sBIN, sTaxDueYear);
                                if (sQtrToPay == "4")
                                {
                                    result2.Query = "delete from taxdues where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + sTaxDueYear.Trim() + "' and due_state <> 'X'";
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result2.Close();

                                    result2.Query = "select * from taxdues where bin = '" + m_sBIN.Trim() + "'";
                                    if (result2.Execute())
                                    {
                                        if (result2.Read())
                                        {
                                            AppSettingsManager.DeleteBillNo(m_sBIN.Trim(), sTaxDueYear.Trim());
                                            AppSettingsManager.DeletePartialPayer(m_sBIN.Trim(), sTaxDueYear.Trim());
                                        }
                                    }
                                    result2.Close();
                                }

                                // RMC 20170216 added btax qtrly payment if with permit-update transaction (s)
                                if (m_bExistTaxPU == true && AppSettingsManager.GetConfigValue("10") == "243")
                                {
                                    result.Query = "update taxdues set due_state = 'R' where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + sTaxDueYear + "'";
                                    if (result.ExecuteNonQuery() != 0)
                                    { }
                                }
                                // RMC 20170216 added btax qtrly payment if with permit-update transaction (e)
                            }
                        }

                        if (chkRetirement.Checked == true)
                        {
                            //result2.Query = "delete from taxdues where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + sTaxDueYear.Trim() + "' and due_state = 'X'";
                            result2.Query = "delete from taxdues where bin = '" + m_sBIN.Trim() + "' and due_state = 'X'";  // RMC 20170301 corrections in retirement application
                            if (result2.ExecuteNonQuery() != 0)
                            {

                            }
                            result2.Close();

                            OracleResultSet result3 = new OracleResultSet();
                            string sBnsCodeMain = string.Empty;
                            result2.Query = "select * from retired_bns where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + sTaxDueYear.Trim() + "'";
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    sBnsCodeMain = result2.GetString("bns_code_main").Trim();
                                    result3.Query = "delete from taxdues where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + sTaxDueYear.Trim() + "' and bns_code_main = '" + sBnsCodeMain.Trim() + "'";
                                    if (result3.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result3.Close();
                                }
                            }
                            result2.Close();
                        }

                    }
                }
                result.Close();

                chkRetirement.Checked = false;
            }
            // saving in or_used (S){
            string orusedOrDate = string.Empty;
            orusedOrDate = string.Format("{0:dd-MMM-yy}", dtpORDate.Value);

            // RMC 20141127 QA Tarlac ver (s)
            if (AppSettingsManager.GetConfigValue("56") == "Y")
            {
                result.Query = "insert into or_used(or_series, or_no,teller,trn_date) values ";
                result.Query += "('" + StringUtilities.Right(txtORNo.Text.Trim(), 2) + "', ";
                result.Query += "'" + StringUtilities.Left(txtORNo.Text.Trim(),txtORNo.Text.Trim().Length - 2) + "', ";
            }// RMC 20141127 QA Tarlac ver (e)
            else
            {
                result.Query = "insert into or_used(or_no,teller,trn_date) values ";
                result.Query += "('" + txtORNo.Text.Trim() + "', ";
            }
            result.Query += " '" + txtTeller.Text.Trim() + "', ";
            
            // AST 20160112 (s)
            //result.Query += " '" + orusedOrDate + "') ";
            DateTime dtTimeHolder = new DateTime();
            DateTime.TryParse(orusedOrDate, out dtTimeHolder);
            result.Query += " to_date('" + dtTimeHolder.ToShortDateString() + "', 'MM/dd/yyyy')) ";
            // AST 20160112 (e)

            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();
            // saving in or_used (E)}

            // RMC 20151229 merged multiple OR use from Mati (s)
            sORNo = txtORNo.Text;
            if (m_iORNo > 1)
            {
                int iTmpOr = 0;
                int.TryParse(txtORNo.Text, out iTmpOr);

                for (int ix = 1; ix < m_iORNo; ix++)
                {
                    iTmpOr++;
                    //sORNo = string.Format("{0:#######}", iTmpOr);
                    sORNo = string.Format("{0:0######}", iTmpOr);   // RMC 20170106 added editing of prev gross/cap in Billing module

                    result.Query = "insert into or_used(or_no,teller,trn_date) values ";
                    result.Query += "('" + sORNo.Trim() + "', ";
                    result.Query += " '" + txtTeller.Text.Trim() + "', ";
                    // AST 20160112 (s)
                    //result.Query += " '" + orusedOrDate + "') ";
                    result.Query += " to_date('" + dtTimeHolder.ToShortDateString() + "', 'MM/dd/yyyy')) ";
                    // AST 20160112 (e)

                    if (result.ExecuteNonQuery() != 0)
                    {}
                    result.Close();
                }
            }
            m_sPreviousOR = sORNo;
            // RMC 20151229 merged multiple OR use from Mati (e)
            //m_sPreviousOR = txtORNo.Text.Trim();  // RMC 20151229 merged multiple OR use from Mati, put rem
            string sNextORNo = string.Empty, sToORNo = string.Empty;
            string sORCode = string.Empty, sCurrOR = string.Empty;
            string sORSeries = string.Empty; //MCR 20140826

            int iOR_Cnt = 0;

            result.Query = "select * from or_current where teller = '" + txtTeller.Text.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    /*sNextORNo = result.GetInt("cur_or_no").ToString();
                    sToORNo = result.GetInt("to_or_no").ToString();*/

                    // RMC 20150501 QA BTAS (s)
                    sNextORNo = result.GetString("cur_or_no");
                    sToORNo = result.GetString("to_or_no");
                    // RMC 20150501 QA BTAS (e)

                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                        sORSeries = result.GetString("or_series");

                    if (AppSettingsManager.IfAutoGeneratedOR() == true)
                    {
                        sORCode = result.GetString("or_code");
                        /*iOR_Cnt = sNextORNo.Length;
                        switch (iOR_Cnt)
                        {
                            case 1:
                                sCurrOR = "00000" + sNextORNo;
                                break;
                            case 2:
                                sCurrOR = "0000" + sNextORNo;
                                break;
                            case 3:
                                sCurrOR = "000" + sNextORNo;
                                break;
                            case 4:
                                sCurrOR = "00" + sNextORNo;
                                break;
                            case 5:
                                sCurrOR = "0" + sNextORNo;
                                break;
                        }*/

                        sCurrOR = sNextORNo;    // RMC 20150501 QA BTAS
                        sCurrOR = sORCode + "-" + sCurrOR;
                    }
                }
            }
            result.Close();

            while (true)
            {
                if (AppSettingsManager.IfAutoGeneratedOR() == true)
                {
                    result.Query = "select * from or_used where or_no = '" + sORSeries.Trim() + sCurrOR.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            iNextOrNo = int.Parse(sNextORNo) + 1;
                            /*sNextORNo = iNextOrNo.ToString();
                            iOR_Cnt = sNextORNo.Length;
                            switch (iOR_Cnt)
                            {
                                case 1:
                                    sCurrOR = "00000" + sNextORNo;
                                    break;
                                case 2:
                                    sCurrOR = "0000" + sNextORNo;
                                    break;
                                case 3:
                                    sCurrOR = "000" + sNextORNo;
                                    break;
                                case 4:
                                    sCurrOR = "00" + sNextORNo;
                                    break;
                                case 5:
                                    sCurrOR = "0" + sNextORNo;
                                    break;
                            }*/

                            sNextORNo = string.Format("{0:000000#}", iNextOrNo);    // RMC 20150501 QA BTAS
                            sCurrOR = sORCode + "-" + sCurrOR;
                        }
                        else
                            break;
                    }
                    result.Close();
                }
                else
                {
                    //result.Query = "select * from or_used where or_no = '" + sORSeries + sNextORNo + "'";
                    // RMC 20141127 QA Tarlac ver (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                        result.Query = "select * from or_used where or_no = '" + sNextORNo + "' and or_series = '" + sORSeries + "'";
                    else
                        result.Query = "select * from or_used where or_no = '" + sNextORNo + "'";
                    // RMC 20141127 QA Tarlac ver (e)
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            iNextOrNo = int.Parse(sNextORNo) + 1;
                            //sNextORNo = iNextOrNo.ToString();
                            sNextORNo = string.Format("{0:000000#}", iNextOrNo);    // RMC 20150501 QA BTAS
                        }
                        else
                            break;
                    }
                    result.Close();

                }
            }

            if (int.Parse(sNextORNo) <= int.Parse(sToORNo))
            {
                result.Query = "update or_current set cur_or_no = '" + sNextORNo + "' where teller = '" + txtTeller.Text.Trim() + "'"; //MCR 20140826 iNextOrNo changed into sNextORNo
                // RMC 20141127 QA Tarlac ver (s)
                if (AppSettingsManager.GetConfigValue("56") == "Y")
                    result.Query += " and or_series = '" + sORSeries + "'";
                // RMC 20141127 QA Tarlac ver (e)
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();
            }
            else
            {
                //result.Query = "insert into or_assigned_hist select * from or_assigned where teller = '" + txtTeller.Text.Trim() + "'";
                result.Query = "insert into or_assigned_hist(teller,from_or_no, to_or_no,trn_date,assigned_by,form_type,or_id,date_assigned) select teller,from_or_no, to_or_no,trn_date,assigned_by,form_type,oa_or_id,date_assigned from or_assigned where teller = '"+ AppSettingsManager.TellerUser.UserCode +"'"; // CTS 09152003 // JAA 20190325 added column names
               
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();

                result.Query = "delete from or_assigned where teller = '" + txtTeller.Text.Trim() + "'";
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();

                result.Query = "delete from or_current where teller = '" + txtTeller.Text.Trim() + "'";
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();

                MessageBox.Show("Maximum OR No. assigned has been reached!");
                m_bMaxOR = true;
                //this.Close(); // RMC 20140910 temp

            }

            if (m_bAttachedExcessAmount)
                AttachedExcessAmount(dtpORDate.Value.ToString("MM/dd/yyyy"), txtORNo.Text.Trim());

            if (chkAdj.Checked)
                InitializeREtables();

            if (m_bIsDisCompromise == true)
                SaveCompromise(bInitialDP);

            //txtORNo.Text = GetCurrentOR(AppSettingsManager.TellerUser.UserCode);  // RMC 20140909 Migration QA, put rem
        }

        private void SaveTransferDues()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();
            String sQuery, sTransAppCode, sTransCount = "0", sFeesCode = "", sFeesDue, sFeesSurch, sFeesPen, sFeesAmtDue, sFees;
            int iTransCount, iCount;

            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();
            String m_sPostedDate, m_sORDate, m_sTimePosted;

            m_sPostedDate = cdtToday.ToString("MM/dd/yyyy");

            m_sORDate = dtpORDate.Value.ToString("MM/dd/yyyy");
            //m_sTimePosted = dtpORDate.Value.ToShortTimeString();
            m_sTimePosted = string.Format("{0:HH:mm}", cdtToday);    // RMC 20140909 Migration QA

            sQuery = string.Format("select count(*) as trans_count from pay_hist where bin = '{0}' and tax_year = '{1}' and qtr_paid like 'T%%'", m_sBIN, txtTaxYear.Text.Trim());
            result.Query = sQuery;
            if (result.Execute())
                if (result.Read())
                {
                    iTransCount = result.GetInt("trans_count");
                    iTransCount++;
                    sTransCount = iTransCount.ToString();
                }
            result.Close();

            sTransCount = "T" + sTransCount;

            sQuery = "insert into pay_hist(or_no,bin,bns_stat,tax_year,qtr_paid,or_date,payment_term,data_mode,payment_type,date_posted,time_posted,bns_user,teller,memo) values ";	// JGR 09192005 Oracle Adjustment
            sQuery += "('" + txtORNo.Text.Trim() + "', ";
            sQuery += " '" + m_sBIN + "', ";
            sQuery += " '" + txtBnsStat.Text.Trim() + "', ";
            sQuery += " '" + txtTaxYear.Text.Trim() + "', ";
            sQuery += " '" + sTransCount + "', ";
            sQuery += " '" + m_sORDate + "', ";
            sQuery += " 'F', ";
            sQuery += " '" + m_sPaymentMode + "', ";
            sQuery += " '" + m_sPaymentType + "', ";
            sQuery += " '" + m_sPostedDate + "', ";	// RMC 20140107 corrected or date saving in offline payment
            sQuery += " '" + m_sTimePosted + "', ";
            sQuery += " '" + AppSettingsManager.SystemUser.UserName + "', ";
            sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
            sQuery += " '" + txtMemo.Text.Trim() + "') ";
            result.Query = sQuery;
            result.ExecuteNonQuery();

            TransLog.UpdateLog(m_sBIN, txtBnsStat.Text.Trim(), txtTaxYear.Text.Trim(), "CAP", m_dTransLogIn, AppSettingsManager.GetSystemDate());   // RMC 20170822 added transaction log feature for tracking of transactions per bin

            for (int iRow = 1; iRow < dgvTaxFees.RowCount; iRow++)
            {
                sFeesDue = dgvTaxFees.Rows[iRow].Cells[4].Value.ToString();
                sFeesSurch = dgvTaxFees.Rows[iRow].Cells[5].Value.ToString();
                sFeesPen = dgvTaxFees.Rows[iRow].Cells[6].Value.ToString();
                sFeesAmtDue = dgvTaxFees.Rows[iRow].Cells[7].Value.ToString();
                sTransAppCode = dgvTaxFees.Rows[iRow].Cells[3].Value.ToString().Substring(2);
                sFees = dgvTaxFees.Rows[iRow].Cells[3].Value.ToString().Substring(2, dgvTaxFees.Rows[iRow].Cells[3].Value.ToString().Length - 2);

                sQuery = string.Format("select fees_code from tax_and_fees_table where fees_desc = '{0}'", StringUtilities.HandleApostrophe(sFees));
                result.Query = sQuery;
                if (result.Execute())
                    if (result.Read())
                        sFeesCode = result.GetString("fees_code");
                result.Close();

                sQuery = "insert into `or`(or_no,fees_code,fees_due,fees_surch,fees_pen,fees_amtdue,qtr_paid,bns_code_main,tax_year) values ";
                sQuery += "('" + txtORNo.Text.Trim() + "', ";
                sQuery += " substr('" + sFeesCode + "',1,2), ";
                sQuery += Convert.ToDouble(sFeesDue).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sFeesSurch).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sFeesPen).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sFeesAmtDue).ToString("##0.00") + ", ";
                sQuery += " '" + sTransCount + "', ";
                sQuery += " '" + txtBnsCode.Text.Trim() + "', ";
                sQuery += " '" + txtTaxYear.Text.Trim() + "') ";
                result.Query = sQuery;
                result.ExecuteNonQuery();

                sQuery = string.Format("insert into trans_fees_hist values('{0}','{1}','{2:##0.00}','0.00','0.00','{3:##0.00}','{4}','{5}','{6}','{7}')", m_sBIN, sFeesCode, sFeesDue, sFeesAmtDue, txtTaxYear.Text.Trim(), m_sORDate, sTransAppCode, sTransCount);
                result.Query = sQuery;
                result.ExecuteNonQuery();
            }

            // ---------- SAVING TO HISTORY TABLES ---------- //
            String sPrevOwnCode, sNewOwnCode, sOldOwn = "", sAppDate;
            String sOwnLn = "", sOwnFn = "", sOwnMi = "", sOwnHouseNo = "", sOwnStreet = "", sOwnDist = "", sOwnZone = "", sOwnBrgy = "", sOwnMun = "", sOwnProv = "";
            String sBnsHouseNo = "", sBnsStreet = "", sBnsMun = "", sBnsDist = "", sBnsZone = "", sBnsBrgy = "", sBnsProv = "", sBnsCode = "";

            sQuery = string.Format("select transfer_table.* from transfer_table,trans_fees_table where transfer_table.bin = trans_fees_table.bin and transfer_table.trans_app_code = trans_fees_table.trans_app_code and trans_fees_table.tax_year = '{0}' and transfer_table.bin = '{1}'", txtTaxYear.Text, m_sBIN);
            result.Query = sQuery;
            if (result.Execute())
            {
                while (result.Read())
                {
                    sTransAppCode = result.GetString("trans_app_code");
                    sAppDate = result.GetString("app_date");

                    if (sTransAppCode == "TO")
                    {
                        sPrevOwnCode = result.GetString("prev_own_code");
                        sNewOwnCode = result.GetString("new_own_code");

                        sQuery = string.Format("select * from own_names where own_code = '{0}'", sPrevOwnCode);
                        result1.Query = sQuery;
                        if (result1.Execute())
                        {
                            if (result1.Read())
                            {
                                sOwnLn = StringUtilities.HandleApostrophe(result1.GetString("own_ln").Trim());
                                sOwnFn = StringUtilities.HandleApostrophe(result1.GetString("own_fn").Trim());
                                sOwnMi = StringUtilities.HandleApostrophe(result1.GetString("own_mi").Trim());
                                sOwnHouseNo = StringUtilities.HandleApostrophe(result1.GetString("own_house_no").Trim());
                                sOwnStreet = StringUtilities.HandleApostrophe(result1.GetString("own_street").Trim());
                                sOwnDist = StringUtilities.HandleApostrophe(result1.GetString("own_dist").Trim());
                                sOwnZone = StringUtilities.HandleApostrophe(result1.GetString("own_zone").Trim());
                                sOwnBrgy = StringUtilities.HandleApostrophe(result1.GetString("own_brgy").Trim());
                                sOwnMun = StringUtilities.HandleApostrophe(result1.GetString("own_mun").Trim());
                                sOwnProv = StringUtilities.HandleApostrophe(result1.GetString("own_prov").Trim());
                            }
                        }
                        result1.Close();

                        sQuery = string.Format("select prev_bns_own from businesses where bin = '{0}'", m_sBIN);
                        result1.Query = sQuery;
                        if (result1.Execute())
                            if (result1.Read())
                                sOldOwn = StringUtilities.HandleApostrophe(result1.GetString("prev_bns_own"));
                        result1.Close();

                        sQuery = string.Format(@"insert into transfer_hist values('{0}','{1}',' ','{2}','{3}','{4}','{5}','{6}','{7}',
                                            '{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')",
                                        m_sBIN, sTransAppCode, sOldOwn, sPrevOwnCode, sOwnLn, sOwnFn, sOwnMi,
                                        sOwnHouseNo, sOwnStreet, sOwnBrgy, sOwnZone, sOwnDist, sOwnMun, sOwnProv, sAppDate, m_sORDate, txtTaxYear.Text, sTransCount);
                        result1.Query = sQuery;
                        result1.ExecuteNonQuery();
                    }
                    else if (sTransAppCode == "TL")
                    {
                        sQuery = string.Format("select * from businesses where bin = '{0}'", m_sBIN);
                        result1.Query = sQuery;
                        if (result1.Execute())
                        {
                            if (result1.Read())
                            {
                                sBnsHouseNo = StringUtilities.HandleApostrophe(result1.GetString("bns_house_no").Trim());
                                sBnsStreet = StringUtilities.HandleApostrophe(result1.GetString("bns_street").Trim());
                                sBnsMun = StringUtilities.HandleApostrophe(result1.GetString("bns_mun").Trim());
                                sBnsDist = StringUtilities.HandleApostrophe(result1.GetString("bns_dist").Trim());
                                sBnsZone = StringUtilities.HandleApostrophe(result1.GetString("bns_zone").Trim());
                                sBnsBrgy = StringUtilities.HandleApostrophe(result1.GetString("bns_brgy").Trim());
                                sBnsProv = StringUtilities.HandleApostrophe(result1.GetString("bns_prov").Trim());
                            }
                        }
                        result1.Close();

                        sQuery = string.Format(@"insert into transfer_hist values('{0}','{1}',' ',' ',' ',' ',' ',' ','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                                        m_sBIN, sTransAppCode, sBnsHouseNo, sBnsStreet, sBnsBrgy, sBnsZone, sBnsDist, sBnsMun, sBnsProv, sAppDate, m_sORDate, txtTaxYear.Text, sTransCount);
                        result1.Query = sQuery;
                        result1.ExecuteNonQuery();
                    }
                    else if (sTransAppCode == "TC")
                    {
                        sQuery = string.Format("select bns_code from businesses where bin = '{0}'", m_sBIN);
                        result1.Query = sQuery;
                        if (result1.Execute())
                            if (result1.Read())
                                sBnsCode = result1.GetString("bns_code");
                        result1.Close();

                        sQuery = string.Format(@"insert into transfer_hist values('{0}','{1}','{2}',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','{3}','{4}','{5}','{6}')",
                                        m_sBIN, sTransAppCode, sBnsCode, sAppDate, m_sORDate, txtTaxYear.Text, sTransCount);
                        result1.Query = sQuery;
                        result1.ExecuteNonQuery();
                    }

                    sQuery = string.Format("delete from transfer_table where bin = '{0}' and trans_app_code = '{1}'", m_sBIN, sTransAppCode);
                    result1.Query = sQuery;
                    result1.ExecuteNonQuery();

                    sQuery = string.Format("delete from bill_no where bin = '{0}' and tax_year = '{1}' and qtr = '{2}' and due_state ='{3}'", m_sBIN, txtTaxYear.Text, sTransCount, sTransAppCode);
                    result1.Query = sQuery;
                    result1.ExecuteNonQuery();
                }
            }
            result.Close();

            sQuery = string.Format("delete from trans_info_hist where bin = '{0}'", m_sBIN);
            result.Query = sQuery;
            result.ExecuteNonQuery();

            sQuery = string.Format("delete from other_fees_hist where bin = '{0}'", m_sBIN);
            result.Query = sQuery;
            result.ExecuteNonQuery();

            sQuery = string.Format("insert into trans_info_hist select * from other_info where bin = '{0}' and tax_year = '{1}'", m_sBIN, txtTaxYear.Text);
            result.Query = sQuery;
            result.ExecuteNonQuery();

            sQuery = string.Format("insert into other_fees_hist select * from other_fees_table where bin = '{0}'", m_sBIN);
            result.Query = sQuery;
            result.ExecuteNonQuery();
            // ---------- SAVING TO HISTORY TABLES ---------- //


            // ---------- UPDATING BUSINESS/OTHER_INFO RECORD ---------- //
            sQuery = string.Format("select * from businesses where bin = '{0}' and own_code = '{1}'", m_sBIN, m_sNewOwnCode);
            result.Query = sQuery;
            if (result.Execute())
                if (result.Read())
                {
                    sQuery = string.Format(@"update businesses set bns_house_no = '{0}',bns_street = '{1}',bns_mun = '{2}',bns_dist = '{3}',
                                bns_zone = '{4}',bns_brgy = '{5}',bns_prov = '{6}',bns_code = '{7}' where bin = '{8}'",
                                    txtBnsAddress.Text.Trim(), m_sStreet, m_sMun, m_sDist, m_sZone, m_sBrgy, m_sProv, txtBnsCode.Text.Trim(), m_sBIN);
                    result.Query = sQuery;
                    result.ExecuteNonQuery();
                }
                else
                {
                    sQuery = string.Format(@"update businesses set bns_house_no = '{0}',bns_street = '{1}',bns_mun = '{2}',bns_dist = '{3}',
                                bns_zone = '{4}',bns_brgy = '{5}',bns_prov = '{6}',bns_code = '{7}',own_code = '{8}',prev_bns_own = '{9}' where bin = '{10}'",
                                    txtBnsAddress.Text.Trim(), m_sStreet, m_sMun, m_sDist, m_sZone, m_sBrgy, m_sProv, txtBnsCode.Text.Trim(), m_sNewOwnCode, m_sPrevOwnCode, m_sBIN);
                    result.Query = sQuery;
                    result.ExecuteNonQuery();
                }
            result.Close();

            sQuery = string.Format("select * from trans_other_info where bin = '{0}'", m_sBIN);
            result.Query = sQuery;
            if (result.Execute())
                if (result.Read())
                {
                    sQuery = string.Format("delete from other_info where bin = '{0}' and tax_year = '{1}'", m_sBIN, txtTaxYear.Text);
                    result.Query = sQuery;
                    result.ExecuteNonQuery();

                    sQuery = string.Format("insert into other_info select * from trans_other_info where bin = '{0}'", m_sBIN);
                    result.Query = sQuery;
                    result.ExecuteNonQuery();
                }
            result.Close();
            // ---------- UPDATING BUSINESS/OTHER_INFO RECORD ---------- //



            // ---------- DELETING IN NECCESSARY TABLE ---------- //
            sQuery = string.Format("delete from trans_fees_table where bin = '{0}'", m_sBIN);
            result.Query = sQuery;
            result.ExecuteNonQuery();

            sQuery = string.Format("delete from trans_other_info where bin = '{0}'", m_sBIN);
            result.Query = sQuery;
            result.ExecuteNonQuery();

            sQuery = string.Format("delete from other_fees_table where bin = '{0}'", m_sBIN);
            result.Query = sQuery;
            result.ExecuteNonQuery();
            // ---------- DELETING IN NECCESSARY TABLE ---------- //


            // ---------- FOR CHECK ----------- //
            sQuery = string.Format("select * from chk_tbl_temp where or_no = '{0}'", txtORNo.Text);
            result.Query = sQuery;
            if (result.Execute())
                if (result.Read())
                {
                    sQuery = string.Format("insert into chk_tbl select * from chk_tbl_temp");
                    result.Query = sQuery;
                    result.ExecuteNonQuery();

                    sQuery = string.Format("delete from chk_tbl_temp where or_no = '{0}'", txtORNo.Text.Trim());
                    result.Query = sQuery;
                    result.ExecuteNonQuery();
                }
            result.Close();

            if (Convert.ToDouble(txtToTaxCredit.Text.Trim()) > 0)
            {
                // RMC 20141127 QA Tarlac ver (s)
                //if (AppSettingsManager.GetConfigValue("10") == "017")
                //    sQuery = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}' and served = 'N'", AppSettingsManager.GetBnsOwnCode(m_sBIN));
                //else if (AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                //    sQuery = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}' and served = 'N'", AppSettingsManager.GetBnsOwnCode(m_sBIN));
                //else// RMC 20141127 QA Tarlac ver (e)
                    sQuery = string.Format("update dbcr_memo set served = 'Y' where bin = '{0}' and served = 'N'", m_sBIN); //JARS 20171010
                result.Query = sQuery;
                result.ExecuteNonQuery();

                String sDebit, sCredit, sBalance, sMemo, sServe;
                sDebit = txtToTaxCredit.Text;
                sCredit = "0";

                sBalance = Convert.ToDouble(Convert.ToDouble(sCredit) - Convert.ToDouble(sDebit)).ToString();
                sServe = "Y";
                sMemo = "DEBITED THRU PAYMENT MADE HAVING OR_NO " + txtORNo.Text;
                // RMC 20141127 QA Tarlac ver (s)
                //if (AppSettingsManager.GetConfigValue("10") == "017")
                //{
                //    sQuery = "insert into dbcr_memo(own_code,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values ";
                //    sQuery += "('" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "', ";
                //}// RMC 20141127 QA Tarlac ver (e)
                //else if (AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                //{
                //    sQuery = "insert into dbcr_memo(own_code,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values ";
                //    sQuery += "('" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "', ";
                //}
                //else
                //{
                    sQuery = "insert into dbcr_memo(bin,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values "; //JARS 20171010
                    sQuery += "('" + m_sBIN + "', ";
                //}
                sQuery += " '" + txtORNo.Text + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += sDebit + ", ";   // ALJ 11242003 fixed SQL 
                sQuery += sCredit + ", ";  // ALJ 11242003 fixed SQL
                sQuery += sBalance + ", "; // ALJ 11242003 fixed SQL
                sQuery += " '" + sMemo + "', ";
                sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += " '" + m_sTimePosted + "', ";
                sQuery += " '" + sServe + "','0') ";
                result.Query = sQuery;
                result.ExecuteNonQuery();

                //Added by EMS 01302003 (s)
                // RMC 20141127 QA Tarlac ver (s)
                //if (AppSettingsManager.GetConfigValue("10") == "017")
                //    sQuery = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}'", AppSettingsManager.GetBnsOwnCode(m_sBIN));
                //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                //    sQuery = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}'", AppSettingsManager.GetBnsOwnCode(m_sBIN));
                //else// RMC 20141127 QA Tarlac ver (e)
                    sQuery = string.Format("update dbcr_memo set served = 'Y' where bin = '{0}'", m_sBIN); //JARS 20171010
                result.Query = sQuery;
                result.ExecuteNonQuery();

                sDebit = "0";
                sCredit = Convert.ToDouble(Convert.ToDouble(txtCreditLeft.Text) - Convert.ToDouble(txtToTaxCredit.Text)).ToString();
                sBalance = Convert.ToDouble(Convert.ToDouble(sCredit) - Convert.ToDouble(sDebit)).ToString();
                if (Convert.ToInt32(sBalance) == 0)
                    sServe = "Y";
                else
                    sServe = "N";
                sMemo = "REMAINING BALANCE AFTER PAYMENT W/ DEBIT HAVING OR " + txtORNo.Text;
                // RMC 20141127 QA Tarlac ver (s)
                //if (AppSettingsManager.GetConfigValue("10") == "017")
                //{
                //    sQuery = "insert into dbcr_memo(own_code,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values ";
                //    sQuery += "('" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "', ";
                //}// RMC 20141127 QA Tarlac ver (e)
                //else if (AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                //{
                //    sQuery = "insert into dbcr_memo(own_code,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values ";
                //    sQuery += "('" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "', ";
                //}
                //else
                //{
                    sQuery = "insert into dbcr_memo(bin,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values "; //JARS 20171010
                    sQuery += "('" + m_sBIN + "', ";
                //}
                sQuery += " '" + txtORNo.Text + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += sDebit + ", ";   // ALJ 11242003 fixed SQL
                sQuery += sCredit + ", ";  // ALJ 11242003 fixed SQL 
                sQuery += sBalance + ", "; // ALJ 11242003 fixed SQL
                sQuery += " '" + sMemo + "', ";
                sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += " '" + m_sTimePosted + "', ";
                sQuery += " '" + sServe + "','0') ";
                result.Query = sQuery;
                result.ExecuteNonQuery();

                //Saving to irreg_payment (s)
                String sIrregType, sCashAmt, sCheckAmt, sCreditAmt, sDebitAmt, sChkNo;
                String sPCashAmt, sPChkAmt, sPRemAmt, sPStatus;
                sIrregType = "CQ-DR";
                sCheckAmt = "0.00";
                sCashAmt = "0.00";
                sCreditAmt = "0.00";
                sDebitAmt = txtToTaxCredit.Text;
                sChkNo = "";
                sPCashAmt = "0";
                sPChkAmt = "0";
                sPRemAmt = "0";
                sPStatus = ".";

                sQuery = "insert into irreg_payment(bin,or_no,or_date,irreg_type,cash_amt,check_amt,credit_amt,debit_amt,chk_no,part_cash_amt,part_chk_amt,part_rem_amt,part_stat) values ";
                sQuery += "('" + m_sBIN + "', ";
                sQuery += " '" + txtORNo.Text + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += " '" + sIrregType + "', ";
                sQuery += Convert.ToDouble(sCashAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sCheckAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sCreditAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sDebitAmt).ToString("##0.00") + ", ";
                sQuery += " '" + sChkNo + "', ";
                sQuery += Convert.ToDouble(sPCashAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sPChkAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sPRemAmt).ToString("##0.00") + ", ";
                sQuery += " '" + sPStatus + "') ";
                result.Query = sQuery;
                result.ExecuteNonQuery();
            }

            if (Convert.ToDouble(m_sDebitCredit) > 1)
            {
                MessageBox.Show("About to create credit memo", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                String sDebit, sCredit, sBalance, sMemo, sServe;
                sDebit = "0";
                sCredit = m_sDebitCredit;
                sBalance = m_sDebitCredit;

                sServe = "N";
                sMemo = "CREDITED IN EXCESS TO CHECK PAYMENT/S HAVING OR_NO " + txtORNo.Text.Trim();

                // RMC 20141127 QA Tarlac ver (s)
                //if (AppSettingsManager.GetConfigValue("10") == "017")
                //{
                //    sQuery = "insert into dbcr_memo(own_code,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values ";
                //    sQuery += "('" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "', ";
                //}// RMC 20141127 QA Tarlac ver (e)
                //else if (AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                //{
                //    sQuery = "insert into dbcr_memo(own_code,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values ";
                //    sQuery += "('" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "', ";
                //}
                //else
                //{
                    sQuery = "insert into dbcr_memo(bin,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values "; //JARS 20171010
                    sQuery += "('" + m_sBIN + "', ";
                //}
                sQuery += " '" + txtORNo.Text.Trim() + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += Convert.ToDouble(sDebit).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sCredit).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sBalance).ToString("##0.00") + ", ";
                sQuery += " '" + sMemo + "', ";
                sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += " '" + m_sTimePosted + "', ";
                sQuery += " '" + sServe + "','0') ";
                result.Query = sQuery;
                result.ExecuteNonQuery();

                // RMC 20141127 QA Tarlac ver (s)
                //if (AppSettingsManager.GetConfigValue("10") == "017")
                //    sQuery = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}'", AppSettingsManager.GetBnsOwnCode(m_sBIN));
                //else if(AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                //    sQuery = string.Format("update dbcr_memo set served = 'Y' where own_code = '{0}'", AppSettingsManager.GetBnsOwnCode(m_sBIN));
                //else// RMC 20141127 QA Tarlac ver (e)
                    sQuery = string.Format("update dbcr_memo set served = 'Y' where bin = '{0}'", m_sBIN); //JARS 20171010
                result.Query = sQuery;
                result.ExecuteNonQuery();

                sDebit = "0";

                if (Convert.ToDouble(sBalance) == 0)
                    sServe = "Y";
                else
                    sServe = "N";

                sMemo = "REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR " + txtORNo.Text.Trim();
                // RMC 20141127 QA Tarlac ver (s)
                //if (AppSettingsManager.GetConfigValue("10") == "017" || AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                //{
                //    sQuery = "insert into dbcr_memo(own_code,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values ";
                //    sQuery += "('" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "', ";
                //}// RMC 20141127 QA Tarlac ver (e)
                //else
                //{
                    sQuery = "insert into dbcr_memo (bin,or_no,or_date,debit,credit,balance,memo,teller,date_created,time_created,served,stat) values ";
                    sQuery += "('" + m_sBIN + "', "; //JARS 20171010
                //}
                sQuery += " '" + txtORNo.Text.Trim() + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += Convert.ToDouble(sDebit).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sCredit).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sBalance).ToString("##0.00") + ", ";
                sQuery += " '" + sMemo + "', ";
                sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += " '" + m_sTimePosted + "', ";
                sQuery += " '" + sServe + "','0') ";
                result.Query = sQuery;
                result.ExecuteNonQuery();

                String sIrregType, sCashAmt, sCheckAmt, sCreditAmt, sDebitAmt, sChkNo;
                String sPCashAmt, sPChkAmt, sPRemAmt, sPStatus;
                sIrregType = "CQ-CR";
                sCheckAmt = "0.00";
                sCashAmt = "0.00";
                sCreditAmt = m_sDebitCredit;
                sDebitAmt = "0.00";
                sChkNo = "";
                sPCashAmt = "0";
                sPChkAmt = "0";
                sPRemAmt = "0";
                sPStatus = ".";

                sQuery = "insert into irreg_payment(bin,or_no,or_date,irreg_type,cash_amt,check_amt,credit_amt,debit_amt,chk_no,part_cash_amt,part_chk_amt,part_rem_amt,part_stat) values ";
                sQuery += "('" + m_sBIN + "', ";
                sQuery += " '" + txtORNo.Text.Trim() + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += " '" + sIrregType + "', ";
                sQuery += Convert.ToDouble(sCashAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sCheckAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sCreditAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sDebitAmt).ToString("##0.00") + ", ";
                sQuery += " '" + sChkNo + "', ";
                sQuery += Convert.ToDouble(sPCashAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sPChkAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sPRemAmt).ToString("##0.00") + ", ";
                sQuery += " '" + sPStatus + "') ";
                result.Query = sQuery;
                result.ExecuteNonQuery();
            }

            if (m_sPaymentType == "CC")
            {
                String sIrregType, sCashAmt, sCheckAmt, sCreditAmt, sDebitAmt, sChkNo;
                String sPCashAmt, sPChkAmt, sPRemAmt, sPStatus;
                sIrregType = "CC";
                sCheckAmt = m_sCCChkAmt;
                sCashAmt = m_sCCCashAmt;
                sCreditAmt = "0.00";
                sDebitAmt = "0.00";
                sChkNo = "";
                sPCashAmt = "0";
                sPChkAmt = "0";
                sPRemAmt = "0";
                sPStatus = ".";

                sQuery = "insert into irreg_payment(bin,or_no,or_date,irreg_type,cash_amt,check_amt,credit_amt,debit_amt,chk_no,part_cash_amt,part_chk_amt,part_rem_amt,part_stat) values ";
                sQuery += "('" + m_sBIN + "', ";
                sQuery += " '" + txtORNo.Text + "', ";
                sQuery += " '" + m_sORDate + "', ";
                sQuery += " '" + sIrregType + "', ";
                sQuery += Convert.ToDouble(sCashAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sCheckAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sCreditAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sDebitAmt).ToString("##0.00") + ", ";
                sQuery += " '" + sChkNo + "', ";
                sQuery += Convert.ToDouble(sPCashAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sPChkAmt).ToString("##0.00") + ", ";
                sQuery += Convert.ToDouble(sPRemAmt).ToString("##0.00") + ", ";
                sQuery += " '" + sPStatus + "') ";
                result.Query = sQuery;
                result.ExecuteNonQuery();
            }
            // ---------- FOR CHECK ----------- //

            // ---------- FOR OR ---------- //
            // RMC 20141127 QA Tarlac ver (s)
            if (AppSettingsManager.GetConfigValue("56") == "Y")
            {
                result.Query = "insert into or_used(or_series, or_no,teller,trn_date) values ";
                result.Query += "('" + StringUtilities.Right(txtORNo.Text.Trim(), 2) + "', ";
                result.Query += "'" + StringUtilities.Left(txtORNo.Text.Trim(),txtORNo.Text.Trim().Length - 2) + "', ";
            }// RMC 20141127 QA Tarlac ver (e)
            else
            {
                sQuery = "insert into or_used(or_no,teller,trn_date) values ";
                sQuery += "('" + txtORNo.Text + "', ";
            }
            sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
            sQuery += " '" + m_sORDate + "') ";
            result.Query = sQuery;
            result.ExecuteNonQuery();

            m_sPreviousOR = txtORNo.Text;
            String sNextORNo = "", sToORNo = "";

            String sORCode = "", sCurrOR = "";
            int iOR_Cnt;
            string sORSeries = string.Empty;    // RMC 20141127 QA Tarlac ver 

            //sQuery = string.Format("select cur_or_no,to_or_no,or_code from or_current where teller = '{0}'", AppSettingsManager.TellerUser.UserCode);
            sQuery = string.Format("select * from or_current where teller = '{0}'", AppSettingsManager.TellerUser.UserCode);    // RMC 20141127 QA Tarlac ver
            result.Query = sQuery;
            if (result.Execute())
                if (result.Read())
                {
                    /*sNextORNo = result.GetInt("cur_or_no").ToString();
                    sToORNo = result.GetInt("to_or_no").ToString();*/

                    // RMC 20150501 QA BTAS (s)
                    sNextORNo = result.GetString("cur_or_no");
                    sToORNo = result.GetString("to_or_no");
                    // RMC 20150501 QA BTAS (e)

                    // RMC 20141127 QA Tarlac ver (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                        sORSeries = result.GetString("or_series");
                    // RMC 20141127 QA Tarlac ver (e)

                    if (AppSettingsManager.IfAutoGeneratedOR() == true)
                    {
                        sORCode = result.GetString("or_code");
                        iOR_Cnt = sNextORNo.Length;

                        switch (iOR_Cnt)
                        {
                            case 1:
                                sCurrOR = "00000" + sNextORNo;
                                break;
                            case 2:
                                sCurrOR = "0000" + sNextORNo;
                                break;
                            case 3:
                                sCurrOR = "000" + sNextORNo;
                                break;
                            case 4:
                                sCurrOR = "00" + sNextORNo;
                                break;
                            case 5:
                                sCurrOR = "0" + sNextORNo;
                                break;
                        }
                        sCurrOR = sORCode + "-" + sCurrOR;
                    }
                }
            result.Close();

            while (true)
            {
                if (AppSettingsManager.IfAutoGeneratedOR() == true)
                {
                    sQuery = string.Format("select * from or_used where or_no = '{0}'", sCurrOR);  // RTL 12022004
                    
                    result.Query = sQuery;
                    if (result.Execute())
                        if (!result.Read())
                        {
                            sNextORNo = Convert.ToInt32(Convert.ToInt32(sNextORNo) + 1).ToString();
                            iOR_Cnt = sNextORNo.Length;

                            switch (iOR_Cnt)
                            {
                                case 1:
                                    sCurrOR = "00000" + sNextORNo;
                                    break;
                                case 2:
                                    sCurrOR = "0000" + sNextORNo;
                                    break;
                                case 3:
                                    sCurrOR = "000" + sNextORNo;
                                    break;
                                case 4:
                                    sCurrOR = "00" + sNextORNo;
                                    break;
                                case 5:
                                    sCurrOR = "0" + sNextORNo;
                                    break;
                            }
                            sCurrOR = sORCode + "-" + sCurrOR;
                        }
                        else
                            break;
                }
                else
                {
                    sQuery = string.Format("select * from or_used where or_no = '{0}'", Convert.ToInt32(sNextORNo)); // ALJ 11242003 fixed SQL  
                    // RMC 20141127 QA Tarlac ver (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                        sQuery += " and or_series = '" + sORSeries + "'";
                    // RMC 20141127 QA Tarlac ver (e)
                    result.Query = sQuery;
                    if (result.Execute())
                        if (!result.Read())
                            sNextORNo = Convert.ToInt32((sNextORNo) + 1).ToString();
                        else
                            break;
                }
            }

            if (Convert.ToInt32(sNextORNo) <= Convert.ToInt32(sToORNo))
            {
                sQuery = string.Format("update or_current set cur_or_no = '{0}' where teller = '{1}'", Convert.ToInt32(sNextORNo), AppSettingsManager.TellerUser.UserCode); // CTS 09152003
                // RMC 20141127 QA Tarlac ver (s)
                if (AppSettingsManager.GetConfigValue("56") == "Y")
                    sQuery += " and or_series = '" + sORSeries + "'";
                // RMC 20141127 QA Tarlac ver (e)
                result.Query = sQuery;
                result.ExecuteNonQuery();
            }
            else
            {
                //sQuery = string.Format("insert into or_assigned_hist select * from or_assigned where teller = '{0}'", AppSettingsManager.TellerUser.UserCode); // CTS 09152003
                sQuery = string.Format("insert into or_assigned_hist (teller,from_or_no, to_or_no,trn_date,assigned_by,form_type,or_id,date_assigned) select teller,from_or_no, to_or_no,trn_date,assigned_by,form_type,oa_or_id,date_assigned from or_assigned where teller = '{0}'", AppSettingsManager.TellerUser.UserCode); // CTS 09152003 // JAA 20190325 added column names
                result.Query = sQuery;
                result.ExecuteNonQuery();

                sQuery = string.Format("delete from or_assigned where teller = '{0}'", AppSettingsManager.TellerUser.UserCode); // CTS 09152003
                result.Query = sQuery;
                result.ExecuteNonQuery();

                sQuery = string.Format("delete from or_current where teller = '{0}'", AppSettingsManager.TellerUser.UserCode); // CTS 09152003
                result.Query = sQuery;
                result.ExecuteNonQuery();


                MessageBox.Show("Maximum OR No. assigned has been reached!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                m_bMaxOR = true;
            }
            //txtORNo.Text = GetCurrentOR(AppSettingsManager.TellerUser.UserCode);  // RMC 20140909 Migration QA, put rem
            // ---------- FOR OR ---------- //
        }

        private void SaveRetirement()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            String sQuery = "", sFullPartial = "", sQtr = "";

            // RMC 20160113 corrections in updating bns_stat in Retirement transaction (s)
            string sBussTaxYear = "";
            sQuery = "select tax_year from businesses where bin = '" + m_sBIN + "'";
            pSet1.Query = sQuery;
            if (pSet1.Execute())
            {
                if (pSet1.Read())
                    sBussTaxYear = pSet1.GetString("tax_year");
            }
            pSet1.Close();
            // RMC 20160113 corrections in updating bns_stat in Retirement transaction (e)

            pSet.Query = string.Format("select * from waive_tbl where bin = '{0}'", m_sBIN);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sFullPartial = pSet.GetString("full_partial").Trim();
                    sQtr = pSet.GetString("qtr").Trim();
                }
            pSet.Close();

            pSet.Query = string.Format("update retired_bns_temp set or_no = '{0}' where bin = '{1}'", txtORNo.Text, m_sBIN);
            pSet.ExecuteNonQuery();

            //pSet.Query = string.Format("insert into retired_bns (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,memoranda,main,apprvd_date,app_no,or_no) select * from retired_bns_temp where bin = '{0}'", m_sBIN);
            pSet.Query = string.Format("insert into retired_bns (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,memoranda,main,apprvd_date,app_no,or_no,bill_no) select * from retired_bns_temp where bin = '{0}'", m_sBIN); //AFM 20211222
            pSet.ExecuteNonQuery();

            pSet.Query = string.Format("delete from retired_bns_temp where bin = '{0}'", m_sBIN);
            pSet.ExecuteNonQuery();

            String sRetNo = "";
            pSet.Query = string.Format("select * from retired_bns where retirement_no is not null order by retirement_no desc");
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sRetNo = pSet.GetInt("retirement_no").ToString();
                    sRetNo = Convert.ToInt32(pSet.GetInt("retirement_no") + 1).ToString();
                }
            pSet.Close();

            pSet.Query = string.Format("update retired_bns set retirement_no = '{0}' where bin = '{1}' and or_no = '{2}'", sRetNo, m_sBIN, txtORNo.Text);
            pSet.ExecuteNonQuery();

            if (sFullPartial == "F")
            {
                if (Convert.ToInt32(txtTaxYear.Text) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12").Trim()))
                {
                    pSet.Query = string.Format("insert into buss_hist select * from businesses where bin = '{0}'", m_sBIN);
                    pSet.ExecuteNonQuery();
                }

                pSet.Query = string.Format("delete from businesses where bin = '{0}'", m_sBIN);
                pSet.ExecuteNonQuery();

                pSet.Query = string.Format("insert into businesses select * from business_que where bin = '{0}'", m_sBIN);
                pSet.ExecuteNonQuery();

                String sBussCode;
                string sRetGross = "";  string sRetQtr = "";	// RMC 20160113 corrections in updating bns_stat in Retirement transaction
                pSet.Query = string.Format("select * from retired_bns where bin ='{0}' and qtr = 'F' and main = 'N'", m_sBIN);
                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        sBussCode = pSet.GetString("bns_code_main");
                        sRetGross = string.Format("{0:####}", pSet.GetDouble("gross"));	// RMC 20160113 corrections in updating bns_stat in Retirement transaction
                        sRetQtr = string.Format("{0:####}", pSet.GetDouble("qtr"));	// RMC 20160113 corrections in updating bns_stat in Retirement transaction	

                        if (Convert.ToInt32(txtTaxYear.Text) == Convert.ToInt32(AppSettingsManager.GetConfigValue("12")))
                        {
                            pSet1.Query = string.Format(@"insert into addl_bns_hist (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,prev_gross) select * from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", m_sBIN, txtTaxYear.Text, sBussCode);   // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1
                            pSet1.ExecuteNonQuery();    // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1

                            pSet1.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", m_sBIN, txtTaxYear.Text, sBussCode);   // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1
                            pSet1.ExecuteNonQuery();    // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1
                        }
                        pSet1.Query = string.Format("delete from tax_and_fees where bin = '{0}' and bns_code_main = '{1}'", m_sBIN, sBussCode); // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1
                        pSet1.ExecuteNonQuery();    // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1

                        // RMC 20160113 corrections in updating bns_stat in Retirement transaction (s)
                        if (Convert.ToInt32(sBussTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")))
                        {
                            pSet1.Query = "insert into addl_bns (BIN, BNS_CODE_MAIN,CAPITAL,GROSS,TAX_YEAR, BNS_STAT,QTR) ";
                            pSet1.Query += "(select BIN, BNS_CODE_MAIN, CAPITAL, '" + sRetGross + "', '" + AppSettingsManager.GetConfigValue("12") + "', 'RET', '" + sRetQtr + "' ";
                            pSet1.Query += " from addl_bns where bin = '" + m_sBIN + "' and tax_year =  '" + sBussTaxYear + "' and bns_code_main = '" + sBussCode + "')";
                            if (pSet1.ExecuteNonQuery() == 0)
                            { }
                        }
                        else
                        {
                            pSet1.Query = "update addl_bns set bns_stat = 'RET' where bin = '" + m_sBIN + "' and tax_year = '" + sBussTaxYear + "' and bns_code_main = '" + sBussCode + "'";
                            if (pSet1.ExecuteNonQuery() == 0)
                            { }
                        }
                        // RMC 20160113 corrections in updating bns_stat in Retirement transaction (e)
                    }
                pSet.Close();

                string.Format("select * from retired_bns where bin ='{0}' and qtr = 'F' and main = 'Y'", m_sBIN);
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sBussCode = pSet.GetString("bns_code_main");
                        string.Format("delete from tax_and_fees where bin = '{0}' and bns_code_main = '{1}'", m_sBIN, sBussCode);
                    }
                pSet.Close();
            }
            else
            {
                String sBussCode;
                string sRetGross = ""; string sRetQtr = "";	// RMC 20160113 corrections in updating bns_stat in Retirement transaction
                if (sQtr.Trim() == "")
                    pSet.Query = string.Format("select * from retired_bns where bin ='{0}' and retirement_no = '{1}' and main = 'N'", m_sBIN, sRetNo);
                else
                    pSet.Query = string.Format("select * from retired_bns where bin ='{0}' and qtr = '{1}' and main = 'N'", m_sBIN, sQtr);
                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        sBussCode = pSet.GetString("bns_code_main");
                        sRetGross = string.Format("{0:####}", pSet.GetDouble("gross"));	// RMC 20160113 corrections in updating bns_stat in Retirement transaction
                        sRetQtr = string.Format("{0:####}", pSet.GetDouble("qtr"));	// RMC 20160113 corrections in updating bns_stat in Retirement transaction	

                        if (Convert.ToInt32(txtTaxYear.Text) == Convert.ToInt32(AppSettingsManager.GetConfigValue("12")))
                        {
                            pSet1.Query = string.Format("insert into addl_bns_hist (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,prev_gross) select * from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", m_sBIN, txtTaxYear.Text, sBussCode);    // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1
                            pSet1.ExecuteNonQuery();    // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1

                            pSet1.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", m_sBIN, txtTaxYear.Text, sBussCode);   // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1
                            pSet1.ExecuteNonQuery();    // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1
                        }

                        pSet1.Query = string.Format("delete from tax_and_fees where bin = '{0}' and bns_code_main = '{1}'", m_sBIN, sBussCode); // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1
                        pSet1.ExecuteNonQuery();    // RMC 20160113 corrections in updating bns_stat in Retirement transaction, changed pSet to pSet1

                        // RMC 20160113 corrections in updating bns_stat in Retirement transaction (s)
                        if (Convert.ToInt32(sBussTaxYear) < Convert.ToInt32(AppSettingsManager.GetConfigValue("12")))
                        {
                            pSet1.Query = "insert into addl_bns (BIN, BNS_CODE_MAIN,CAPITAL,GROSS,TAX_YEAR, BNS_STAT,QTR) ";
                            pSet1.Query += "(select BIN, BNS_CODE_MAIN, CAPITAL, '" + sRetGross + "', '" + AppSettingsManager.GetConfigValue("12") + "', 'RET', '" + sRetQtr + "' ";
                            pSet1.Query += " from addl_bns where bin = '" + m_sBIN + "' and tax_year =  '" + sBussTaxYear + "' and bns_code_main = '" + sBussCode + "')";
                            if (pSet1.ExecuteNonQuery() == 0)
                            { }
                        }
                        else
                        {
                            pSet1.Query = "update addl_bns set bns_stat = 'RET' where bin = '" + m_sBIN + "' and tax_year = '" + sBussTaxYear + "' and bns_code_main = '" + sBussCode + "'";
                            if (pSet1.ExecuteNonQuery() == 0)
                            { }
                        }
                        // RMC 20160113 corrections in updating bns_stat in Retirement transaction (e)
                    }
                pSet.Close();

                if (sQtr == "")
                    pSet.Query = string.Format("select * from retired_bns where bin ='{0}' and retirement_no = '{1}' and main = 'Y'", m_sBIN, sRetNo);
                else
                    pSet.Query = string.Format("select * from retired_bns where bin ='{0}' and qtr = '{1}' and main = 'Y'", m_sBIN, sQtr);
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        pSet.Close();
                        pSet.Query = string.Format("select * from addl_bns where bin ='{0}' order by tax_year desc", m_sBIN);
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                String sAddlTY;
                                sBussCode = pSet.GetString("bns_code_main");
                                sAddlTY = pSet.GetString("tax_year");

                                pSet1.Query = string.Format("update businesses set bns_code = '{0}' where bin ='{1}'", sBussCode, m_sBIN);
                                pSet1.ExecuteNonQuery();

                                pSet1.Query = string.Format("insert into addl_bns_hist (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,prev_gross) select * from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", m_sBIN, sAddlTY, sBussCode);
                                pSet1.ExecuteNonQuery();

                                pSet1.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", m_sBIN, sAddlTY, sBussCode);
                                pSet1.ExecuteNonQuery();

                                pSet1.Query = string.Format("delete from tax_and_fees where bin = '{0}' and bns_code_main = '{1}'", m_sBIN, sBussCode);
                                pSet1.ExecuteNonQuery();

                                //pSet1.Query = string.Format("update businesses set bns_code = '{0}' where bin ='{1}'", sBussCode, m_sBIN);  // RMC 20160119 changed pSet to pSet1
                                //pSet1.ExecuteNonQuery();    // RMC 20160119 changed pSet to pSet1
                                //pSet1.Query = string.Format("insert into addl_bns_hist (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,prev_gross) select * from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", m_sBIN, sAddlTY, sBussCode);    // RMC 20160119 changed pSet to pSet1
                                //pSet1.ExecuteNonQuery();    // RMC 20160119 changed pSet to pSet1
                                //pSet1.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", m_sBIN, sAddlTY, sBussCode);   // RMC 20160119 changed pSet to pSet1
                                //pSet1.ExecuteNonQuery();    // RMC 20160119 changed pSet to pSet1
                                //pSet1.Query = string.Format("delete from tax_and_fees where bin = '{0}' and bns_code_main = '{1}'", m_sBIN, sBussCode); // RMC 20160119 changed pSet to pSet1
                                //pSet1.ExecuteNonQuery();    // RMC 20160119 changed pSet to pSet1

                            }
                    }
                pSet.Close();
            }
        }

        private void AttachedExcessAmount(String p_sOrDate, String p_sOrNo)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            m_sTimePosted = string.Format("{0:HH:m}", dtpORDate.Value);

            String sQuery;
            // RMC 20141127 QA Tarlac ver (s)
            //if (AppSettingsManager.GetConfigValue("10") == "017" || AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
            //    pSet.Query = string.Format("update dbcr_memo set own_code = '" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "', multi_pay = 'N' where or_date = to_date('" + p_sOrDate + "','MM/dd/yyyy') and or_no = '" + p_sOrNo + "' and memo like 'REMAINING%%' and served = 'N'");
            //else// RMC 20141127 QA Tarlac ver (e)
                pSet.Query = string.Format("update dbcr_memo set bin = '" + m_sBIN + "', multi_pay = 'N' where or_date = to_date('" + p_sOrDate + "','MM/dd/yyyy') and or_no = '" + p_sOrNo + "' and memo like 'REMAINING%%' and served = 'N'"); //JARS 20171010
            pSet.ExecuteNonQuery();

            String sRefChkNo, m_sBalance, sMemo;
            String sBankCode = string.Empty; //AFM 20200623
            double dChkAmtTotal = 0, dChkMultiAmt = 0;
            //pSet.Query = string.Format("select chk_no from chk_tbl where or_no = '{0}'", p_sOrNo);
            pSet.Query = string.Format("select * from chk_tbl where or_no = '{0}'", p_sOrNo); //AFM 20200623 changed to all
            if (pSet.Execute())
                while (pSet.Read())
                {
                    sRefChkNo = pSet.GetString("chk_no");
                    sBankCode = pSet.GetString("bank_code");

                    //pSet1.Query = string.Format("select * from multi_check_pay where chk_no = '{0}'", sRefChkNo);
                    pSet1.Query = string.Format("select * from multi_check_pay where chk_no = '{0}' and bank_code = '{1}'", sRefChkNo, sBankCode);
                    if (pSet1.Execute())
                        if (pSet1.Read())
                        {
                            dChkMultiAmt = pSet1.GetDouble("chk_amt");
                            pSet1.Close();

                            //pSet1.Query = string.Format("select sum(chk_amt) from chk_tbl where chk_no = '{0}'", sRefChkNo);
                            pSet1.Query = string.Format("select sum(chk_amt) from chk_tbl where chk_no = '{0}' and bank_code = '{1}'", sRefChkNo, sBankCode); //AFM 20200612 added bank
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                {
                                    dChkAmtTotal = pSet1.GetDouble(0);
                                    dChkAmtTotal = dChkMultiAmt - dChkAmtTotal;

                                    if (dChkAmtTotal > 0)
                                    {
                                        m_sBalance = dChkAmtTotal.ToString("##0.00");

                                        sMemo = "REMAINING BALANCE AFTER PAYMENT W/ DEBIT HAVING OR " + p_sOrNo;
                                        sQuery = "insert into dbcr_memo values "; //JARS 20171010
                                        // RMC 20141127 QA Tarlac ver (s)
                                        //if (AppSettingsManager.GetConfigValue("10") == "017" || AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                                        //    sQuery += "('" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "', ";
                                        //else// RMC 20141127 QA Tarlac ver (e)
                                            sQuery += "('" + m_sBIN + "', ";
                                        sQuery += " '" + p_sOrNo + "', ";
                                        sQuery += "to_date('" + p_sOrDate + "','MM/dd/yyyy'), ";
                                        sQuery += " 0,";
                                        sQuery += m_sBalance + ", ";
                                        sQuery += m_sBalance + ", ";
                                        sQuery += " '" + StringUtilities.HandleApostrophe(sMemo) + "', '', ";
                                        sQuery += " '" + AppSettingsManager.TellerUser.UserCode + "', ";
                                        string sTextFormat = dtpORDate.Value.ToString("MM/dd/yyyy");
                                        sQuery += "to_date('" + sTextFormat + "','MM/dd/yyyy'),";
                                        sQuery += "'" + m_sTimePosted + "', ";
                                        sQuery += "'N',";
                                        sQuery += "'" + sRefChkNo + "',";
                                        sQuery += "'N','0')";
                                        pSet1.Query = sQuery;
                                        pSet1.ExecuteNonQuery();
                                        break;
                                    }
                                }
                            pSet1.Close();
                        }
                    pSet1.Close();
                }
            pSet.Close();
        }

        private void InitializeREtables()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = string.Format("delete from taxdues where bin = '{0}' and due_state = 'A'", m_sBIN);
            pSet.ExecuteNonQuery();
        }

        private void SaveCompromise(bool bInitDP)
        {
            double dFeesDue = 0, dFeesSurch = 0, dFeesPen = 0, dFeesTotal = 0;
            double dTmpFeesDue = 0, dTmpFeesSurch = 0, dTmpFeesPen = 0, dTmpFeesTotal = 0;
            String sQuery = "", sString = "", sYear = "", sTempYear = "", sFeesCode = "", sFormat = "", sBnsCode = "", sPayType = "";

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            int iTermToPay = 0, iLoop = 0, iTmpToPay = 0, iCompPay = 0;
            int iRow = 0, iTotalRow = 0;
            bool bCompDueExist = false;

            DateTime cdtToday;
            cdtToday = AppSettingsManager.GetSystemDate();
            m_sPostedDate = cdtToday.ToString("MM/dd/yyyy");
            m_sTimePosted = string.Format("{0:HH:mm}", cdtToday);    // RMC 20140909 Migration QA

            sQuery = "select * from compromise_due where bin = '" + m_sBIN + "'";
            sQuery += " order by tax_year";
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (bInitDP == false)
                {
                    while (pSet.Read())
                    {
                        sBnsCode = pSet.GetString("bns_code");
                        sFeesCode = pSet.GetString("fees_code");
                        sYear = pSet.GetString("tax_year");

                        iTermToPay = pSet.GetInt("term_to_pay");

                        iTmpToPay = GetCompTermToPay(m_sBIN, iTermToPay);
                        dTmpFeesDue = pSet.GetDouble("fees_due");
                        dTmpFeesSurch = pSet.GetDouble("fees_surch");
                        dTmpFeesPen = pSet.GetDouble("fees_pen");
                        dTmpFeesTotal = pSet.GetDouble("fees_total");

                        String sFeesDue, sFeesSurch, sFeesPen;
                        sFeesDue = Convert.ToDouble(dTmpFeesDue * 0.25).ToString();
                        sFeesSurch = Convert.ToDouble(dTmpFeesSurch * 0.25).ToString();
                        sFeesPen = Convert.ToDouble(dTmpFeesPen * 0.25).ToString();

                        dFeesDue = dTmpFeesDue - Convert.ToDouble(sFeesDue);
                        dFeesSurch = dTmpFeesSurch - Convert.ToDouble(sFeesSurch);
                        dFeesPen = dTmpFeesPen - Convert.ToDouble(sFeesPen);

                        dFeesTotal = dFeesDue + dFeesSurch + dFeesPen;

                        sString = "update compromise_due set";
                        sString += " fees_due = ";
                        sString += dFeesDue.ToString("##0.00") + ",";
                        sString += " fees_surch = ";
                        sString += dFeesSurch.ToString("##0.00") + ",";
                        sString += " fees_pen = ";
                        sString += dFeesPen.ToString("##0.00") + ",";
                        sString += " fees_total = ";
                        sString += dFeesTotal.ToString("##0.00") + ",";
                        sString += " pay_sw = 'Y',";
                        sString += " term_to_pay = ";
                        sString += iTmpToPay;
                        sString += " where bin = '" + m_sBIN + "'";
                        sString += " and fees_code = '" + sFeesCode + "'";
                        sString += " and tax_year  = '" + sYear + "'";
                        sString += " and fees_total = ";
                        sString += dTmpFeesTotal.ToString("##0.00");
                        pSet.Query = sString;
                        pSet.ExecuteNonQuery();

                        if (sTempYear == "" || sTempYear != sYear)
                        {
                            sTempYear = sYear;
                            sString = "insert into comp_payhist values (";
                            sString += "'" + txtORNo.Text + "',";
                            sString += "'" + m_sBIN + "',";
                            sString += "'" + sYear + "',";
                            sString += "'F',";
                            sString += "'0',";
                            sString += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),";
                            sString += "'F',";
                            sString += "'" + m_sPaymentMode + "',";
                            sString += "'" + m_sPaymentType + "',";
                            /*sString += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),";
                            sString += "to_date('" + dtpORDate.Value.ToString("hh:mm") + "','hh:mm'),";*/
                            sString += "to_date('" + m_sPostedDate + "','MM/dd/yyyy'),";    // RMC 20140909 Migration QA
                            sString += "'" + m_sTimePosted + "',"; // RMC 20140909 Migration QA
                            sString += "'" + AppSettingsManager.SystemUser.UserCode + "',";
                            sString += "'" + AppSettingsManager.TellerUser.UserCode + "',";
                            sString += "'" + txtMemo.Text.Trim() + "')";
                            pSet.Query = sString;
                            pSet.ExecuteNonQuery();

                            sString = "insert into pay_hist values (";
                            sString += "'" + txtORNo.Text + "',";
                            sString += "'" + m_sBIN + "',";
                            sString += "'" + txtBnsStat.Text + "',";
                            sString += "'" + sYear + "',";
                            sString += "'F',";
                            sString += "'0',";
                            sString += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),";
                            sString += "'F',";
                            sString += "'" + m_sPaymentMode + "',";
                            sString += "'" + m_sPaymentType + "',";
                            /*sString += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),";
                            sString += "to_date('" + dtpORDate.Value.ToString("hh:mm") + "','hh:mm'),";*/
                            sString += "to_date('" + m_sPostedDate + "','MM/dd/yyyy'),";    // RMC 20140909 Migration QA
                            sString += "'" + m_sTimePosted + "',"; // RMC 20140909 Migration QA
                            sString += "'" + AppSettingsManager.SystemUser.UserCode + "',";
                            sString += "'" + AppSettingsManager.TellerUser.UserCode + "',";
                            sString += "'COMPROMISE AGREEMENT',null,null)";
                            pSet.Query = sString;
                            pSet.ExecuteNonQuery();

                            TransLog.UpdateLog(m_sBIN, txtBnsStat.Text, sYear, "CAP", m_dTransLogIn, AppSettingsManager.GetSystemDate());   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                        }

                        dFeesDue = dTmpFeesDue - dFeesDue;
                        dFeesSurch = dTmpFeesSurch - dFeesSurch;
                        dFeesPen = dTmpFeesPen - dFeesPen;
                        dFeesTotal = dFeesDue + dFeesSurch + dFeesPen;

                        m_sAddlCompPen = Convert.ToDouble(dFeesTotal * GetCompromiseInterest(m_sDateToPay, dtpORDate.Value.ToString("MM/dd/yyyy"), m_dPenRate)).ToString();

                        if (sFeesCode.Substring(0, 1) == "B")
                            sFeesCode = "B";

                        sString = "insert into or_table(or_no,fees_code,fees_due,fees_surch,fees_pen,fees_amtdue,qtr_paid,bns_code_main,tax_year) values ";
                        sString += "('" + txtORNo.Text + "', ";
                        sString += "substr('" + sFeesCode + "',1,2), ";
                        sString += dFeesDue.ToString("##0.00") + ", ";
                        sString += dFeesSurch.ToString("##0.00") + ", ";
                        sString += Convert.ToDouble(dFeesPen + Convert.ToDouble(m_sAddlCompPen)).ToString("##0.00") + ", ";
                        sString += Convert.ToDouble(dFeesTotal + Convert.ToDouble(m_sAddlCompPen)).ToString("##0.00") + ", ";
                        sString += " 'I', ";
                        sString += " '" + sBnsCode + "', ";
                        sString += " '" + sYear + "') ";
                        pSet.Query = sString;
                        pSet.ExecuteNonQuery();
                    }
                }

                if (dgvTaxFees.Rows[dgvTaxFees.RowCount - 1].Cells[8].Value.ToString() == "C")
                    bCompDueExist = true;

                if (bCompDueExist == true)
                {

                    if (txtCompromise.Text.Trim() == "0" || txtCompromise.Text.Trim() == "")
                        txtCompromise.Text = "1";

                    iCompPay = Convert.ToInt32(txtCompromise.Text);
                    sPayType = "I";

                    sQuery = "select * from compromise_due where bin = '" + m_sBIN + "'";
                    sQuery += " order by tax_year";
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        for (iLoop = 1; iLoop <= iCompPay; iLoop++)
                        {
                            while (pSet1.Read())
                            {
                                dTmpFeesDue = pSet1.GetDouble("fees_due");
                                dTmpFeesSurch = pSet1.GetDouble("fees_surch");
                                dTmpFeesPen = pSet1.GetDouble("fees_pen");
                                dTmpFeesTotal = dTmpFeesDue + dTmpFeesSurch + dTmpFeesPen;

                                sBnsCode = pSet1.GetString("bns_code");
                                sFeesCode = pSet1.GetString("fees_code");
                                sYear = pSet1.GetString("tax_year");

                                iTermToPay = pSet1.GetInt("term_to_pay");
                                if (iLoop > 1)
                                    iTermToPay = iTmpToPay;
                                iTmpToPay = GetCompTermToPay(m_sBIN, iTermToPay);

                                sString = "update compromise_due set";
                                sString += " term_to_pay = ";
                                sString += iTmpToPay;
                                sString += " where bin = '" + m_sBIN + "'";
                                sString += " and fees_code = '" + sFeesCode + "'";
                                sString += " and tax_year  = '" + sYear + "'";
                                pSet1.Query = sString;
                                pSet1.ExecuteNonQuery();

                                if (sTempYear == "" || sTempYear != sYear)
                                {
                                    sTempYear = sYear;
                                    sString = "insert into comp_payhist values (";
                                    sString += "'" + txtORNo.Text + "',";
                                    sString += "'" + m_sBIN + "',";
                                    sString += "'" + sYear + "',";
                                    sString += "'" + iCompPay + "',";
                                    sString += "'" + iTermToPay + "',";
                                    sString += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),";
                                    sString += "'" + sPayType + "',";
                                    sString += "'" + m_sPaymentMode + "',";
                                    sString += "'" + m_sPaymentType + "',";
                                    /*sString += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),";
                                    sString += "to_date('" + dtpORDate.Value.ToString("hh:mm") + "','hh:mm'),";*/
                                    sString += "to_date('" + m_sPostedDate + "','MM/dd/yyyy'),";    // RMC 20140909 Migration QA
                                    sString += "'" + m_sTimePosted + "',"; // RMC 20140909 Migration QA
                                    sString += "'" + AppSettingsManager.SystemUser.UserCode + "',";
                                    sString += "'" + AppSettingsManager.TellerUser.UserCode + "',";
                                    sString += "'" + txtMemo.Text.Trim() + "')";
                                    pSet1.Query = sString;
                                    pSet1.ExecuteNonQuery();

                                    sString = "insert into pay_hist values (";
                                    sString += "'" + txtORNo.Text + "',";
                                    sString += "'" + m_sBIN + "',";
                                    sString += "'" + txtBnsStat.Text.Trim() + "',";
                                    sString += "'" + sYear + "',";
                                    sString += "'" + iCompPay + "',";
                                    sString += "'" + iTermToPay + "',";
                                    sString += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),";
                                    sString += "'" + sPayType + "',";
                                    sString += "'" + m_sPaymentMode + "',";
                                    sString += "'" + m_sPaymentType + "',";
                                    /*sString += "to_date('" + dtpORDate.Value.ToString("MM/dd/yyyy") + "','MM/dd/yyyy'),";
                                    sString += "to_date('" + dtpORDate.Value.ToString("hh:mm") + "','hh:mm'),";*/
                                    sString += "to_date('" + m_sPostedDate + "','MM/dd/yyyy'),";    // RMC 20140909 Migration QA
                                    sString += "'" + m_sTimePosted + "',"; // RMC 20140909 Migration QA
                                    sString += "'" + AppSettingsManager.SystemUser.UserCode + "',";
                                    sString += "'" + AppSettingsManager.TellerUser.UserCode + "',";
                                    sString += "'COMPROMISE AGREEMENT',null,null)";
                                    pSet1.Query = sString;
                                    pSet1.ExecuteNonQuery();

                                    TransLog.UpdateLog(m_sBIN, txtBnsStat.Text.Trim(), sYear, "CAP", m_dTransLogIn, AppSettingsManager.GetSystemDate());   // RMC 20170822 added transaction log feature for tracking of transactions per bin
                                }

                                String sFeesDue, sFeesSurch, sFeesPen;
                                sFeesDue = Convert.ToDouble(dTmpFeesDue / Convert.ToDouble(iAgreePayTerm)).ToString("##0.00");
                                sFeesSurch = Convert.ToDouble(dTmpFeesSurch / Convert.ToDouble(iAgreePayTerm)).ToString("##0.00");
                                sFeesPen = Convert.ToDouble(dTmpFeesPen / Convert.ToDouble(iAgreePayTerm)).ToString("##0.00");

                                dFeesDue = Convert.ToDouble(sFeesDue);
                                dFeesSurch = Convert.ToDouble(sFeesSurch);
                                dFeesPen = Convert.ToDouble(sFeesPen);

                                dFeesTotal = dFeesDue + dFeesSurch + dFeesPen;
                                m_sAddlCompPen = Convert.ToDouble(dFeesTotal * GetCompromiseInterest(m_sDateToPay, dtpORDate.Value.ToString("MM/dd/yyyy"), m_dPenRate)).ToString();

                                if (sFeesCode.Substring(0, 1) == "B")
                                    sFeesCode = "B";

                                sString = "insert into or_table(or_no,fees_code,fees_due,fees_surch,fees_pen,fees_amtdue,qtr_paid,bns_code_main,tax_year) values ";	// JGR 09202005 Oracle Adjustment
                                sString += "('" + txtORNo.Text + "', ";
                                sString += "substr('" + sFeesCode + "',1,2), ";
                                sString += dFeesDue + ", ";
                                sString += dFeesSurch + ", ";
                                sString += dFeesPen + Convert.ToDouble(m_sAddlCompPen) + ", ";
                                sString += dFeesTotal + Convert.ToDouble(m_sAddlCompPen) + ", ";
                                sString += " '" + iCompPay + "',";
                                sString += " '" + sBnsCode + "', ";
                                sString += " '" + sYear + "') ";
                                pSet1.Query = sString;
                                pSet1.ExecuteNonQuery();
                            }
                            pSet1.Close();
                        }


                    sQuery = "select * from compromise_due where bin = '" + m_sBIN + "'";
                    sQuery += " order by tax_year";
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        while (pSet1.Read())
                        {
                            sQuery = "insert into comp_due_last values (";
                            sQuery += "'" + m_sBIN + "',";
                            sQuery += "'" + pSet1.GetString("tax_year") + "',";
                            sQuery += "'" + pSet1.GetString("bns_code") + "',";
                            sQuery += "'" + pSet1.GetString("fees_code") + "',";
                            sQuery += "'" + pSet1.GetDouble("fees_due").ToString("##0.00") + "',";
                            sQuery += "'" + pSet1.GetDouble("fees_surch").ToString("##0.00") + "',";
                            sQuery += "'" + pSet1.GetDouble("fees_pen").ToString("##0.00") + "',";
                            sQuery += "'" + pSet1.GetDouble("fees_total").ToString("##0.00") + "',";
                            sQuery += "'" + pSet1.GetString("due_state") + "',";
                            sQuery += "'" + pSet1.GetInt("term_to_pay") + "',";
                            sQuery += "'" + pSet1.GetString("pay_sw") + "',";
                            sQuery += "'" + txtORNo.Text + "')";
                            pSet1.Query = sQuery;
                            pSet1.ExecuteNonQuery();
                        }
                    pSet1.Close();

                    if (iTmpToPay == iAgreePayTerm)
                    {
                        pSet.Query = "insert into compromise_hist select * from compromise_tbl where bin = '" + m_sBIN + "'";
                        pSet.ExecuteNonQuery();

                        pSet.Query = "delete from compromise_tbl where bin = '" + m_sBIN + "'";
                        pSet.ExecuteNonQuery();

                        pSet.Query = "delete from compromise_due where bin = '" + m_sBIN + "'";
                        pSet.ExecuteNonQuery();
                    }
                }
            }
            pSet.Close();
        }

        private int GetCompTermToPay(String p_sBin, int p_iTermToPay)
        {
            bool bFlag = true;
            OracleResultSet pSet = new OracleResultSet();
            p_iTermToPay = p_iTermToPay + 1;
            while (bFlag)
            {
                pSet.Query = string.Format("select * from comp_payhist where bin = '{0}' and pay_no = {1}", p_sBin, p_iTermToPay);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                        p_iTermToPay = p_iTermToPay + 1;
                    else
                        bFlag = false;
                }
                else
                    bFlag = false;
            }
            return p_iTermToPay;
        }
        //MCR 20140724 (e)

        private void SaveChangeClass(String p_sNewBnsCode)
        {
            String sQuery, sCcOldBnsCode, sCcCapital, sCcStatus, sCcIsMain, sCcAppDate;
            String sAddlCapital, sAddlGross, sAddlStatus, sAddlQtr;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            sQuery = "select * from change_class_tbl";
            sQuery += " where bin = '" + m_sBIN + "'";
            sQuery += " and new_bns_code = '" + p_sNewBnsCode + "'";
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sCcOldBnsCode = pSet.GetString("old_bns_code");
                    sCcCapital = pSet.GetDouble("capital").ToString();
                    sCcStatus = pSet.GetString("status");
                    sCcIsMain = pSet.GetString("is_main");
                    sCcAppDate = pSet.GetDateTime("app_date").ToString("MM/dd/yyyy");

                    if (sCcIsMain == "Y")
                    {
                        sQuery = "update businesses set bns_code = '" + p_sNewBnsCode + "'";
                        sQuery += " where bin = '" + m_sBIN + "'";
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                    }
                    else
                    {
                        sQuery = "insert into addl_bns";
                        sQuery += " select * from addl_bns_que";
                        sQuery += " where bin = '" + m_sBIN + "'";
                        sQuery += " and bns_code_main = '" + p_sNewBnsCode + "'";
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();

                        String sAddlPrevGross = "";

                        sQuery = "select * from addl_bns";
                        sQuery += " where bin = '" + m_sBIN + "'";
                        sQuery += " and bns_code_main = '" + sCcOldBnsCode + "'";
                        sQuery += " and tax_year = '" + txtTaxYear.Text + "'";
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            while (pSet1.Read())
                            {
                                sAddlCapital = pSet1.GetDouble("capital").ToString();
                                sAddlGross = pSet1.GetDouble("gross").ToString();
                                sAddlStatus = pSet1.GetString("bns_stat");
                                sAddlQtr = pSet1.GetString("qtr");
                                sAddlPrevGross = pSet1.GetDouble("prev_gross").ToString();

                                sQuery = "insert into addl_bns_hist values";
                                sQuery += " ('" + m_sBIN + "',";
                                sQuery += " '" + sCcOldBnsCode + "',";
                                sQuery += Convert.ToDouble(sAddlCapital).ToString("##0.00") + ",";
                                sQuery += Convert.ToDouble(sAddlGross).ToString("##0.00") + ",";
                                sQuery += " '" + txtTaxYear.Text + "',";
                                sQuery += " '" + sAddlStatus + "',";
                                sQuery += " '" + sAddlQtr + "',";
                                sQuery += " '" + txtORNo.Text + "', ";
                                sQuery += Convert.ToDouble(sAddlPrevGross).ToString("##0.00") + ")";
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();
                            }
                        pSet1.Close();

                        sQuery = "delete from addl_bns";
                        sQuery += " where bin = '" + m_sBIN + "'";
                        sQuery += " and bns_code_main = '" + sCcOldBnsCode + "'";
                        sQuery += " and tax_year = '" + txtTaxYear.Text + "'";
                        pSet1.Query = sQuery;
                        pSet1.ExecuteNonQuery();

                        sQuery = "delete from addl_bns_que";
                        sQuery += " where bin = '" + m_sBIN + "'";
                        sQuery += " and bns_code_main = '" + p_sNewBnsCode + "'";
                        pSet1.Query = sQuery;
                        pSet1.ExecuteNonQuery();
                    }

                    sQuery = "insert into change_class_hist values";
                    sQuery += " ('" + m_sBIN + "',";
                    sQuery += " '" + sCcOldBnsCode + "',";
                    sQuery += " '" + p_sNewBnsCode + "',";
                    sQuery += sCcCapital + ",";
                    sQuery += " '" + sCcStatus + "',";
                    sQuery += " '" + sCcIsMain + "',";
                    //sQuery += " '" + sCcAppDate + "',";
                    sQuery += " to_date('" + sCcAppDate + "','MM/dd/yyyy'),";   // RMC 20170217 corrected error in payment of bin with change in buss type transaction
                    sQuery += " '" + txtORNo.Text + "')";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    sQuery = "delete from change_class_tbl";
                    sQuery += " where bin = '" + m_sBIN + "'";
                    sQuery += " and new_bns_code = '" + p_sNewBnsCode + "'";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
            pSet.Close();
        }

        private void txtLastChange_Click(object sender, EventArgs e)
        {
            txtLastChange.SelectAll();
            txtLastChange.Copy();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            //OnAcceptPayment();
            OnAcceptPaymentNew();

            if (m_bMaxOR == true)
                btnSearchBin.Enabled = false;
        }

        private void OnAcceptPayment()
        {
            double dCCChkAmt = 0;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();

            if (m_bIsDisCompromise == true)
            {
                if (Convert.ToDouble(txtCompromise.Text) < 0 || Convert.ToDouble(txtCompromise.Text) > m_iNoPaySeq)
                {
                    MessageBox.Show("Invalid Compromise Due to Pay.", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCompromise.Focus();
                    return;
                }
            }

            if (chkCash.Checked == false && chkCheque.Checked == false && chkTCredit.Checked == false)
            {
                MessageBox.Show("Payment type required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            String sQuery, m_sOrNo;

            m_sOrNo = txtORNo.Text.Trim();
            result.Query = string.Format("select distinct or_no from pay_hist where or_no = '{0}'", txtORNo.Text.Trim());
            if (result.Execute())
                if (result.Read())
                {
                    MessageBox.Show("Duplicate OR no.", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    result.Close();
                    return;
                }
            result.Close();

            //(s) RTL 11252005 for auto or non-auto generated or
            if (AppSettingsManager.IfAutoGeneratedOR() != true)
            {
                //result.Query = string.Format("select * from or_current where {0} between from_or_no and to_or_no and teller = '{1}'", Convert.ToInt32(txtORNo.Text.Trim().Substring(2)), AppSettingsManager.TellerUser.UserCode);
                // RMC 20141127 QA Tarlac ver (s)
                if (AppSettingsManager.GetConfigValue("56") == "Y")
                {
                    string sOrNo = StringUtilities.Left(txtORNo.Text.Trim(),txtORNo.Text.Trim().Length - 2);
                    string sOrSeries = StringUtilities.Right(txtORNo.Text.Trim(),2);

                    result.Query = "select * from or_current where "+ sOrNo +" between from_or_no and to_or_no ";
                    result.Query += " and teller = '" + AppSettingsManager.TellerUser.UserCode + "'";
                    result.Query += " and or_series = '" + sOrSeries + "'";
                }// RMC 20141127 QA Tarlac ver (e)
                else
                    result.Query = string.Format("select * from or_current where {0} between from_or_no and to_or_no and teller = '{1}'", Convert.ToInt32(txtORNo.Text.Trim()), AppSettingsManager.TellerUser.UserCode);

                if (result.Execute())
                    if (!result.Read())
                    {
                        MessageBox.Show("OR No. is not included in the OR declaration ...", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        result.Close();
                        return;
                    }
                result.Close();
            }

            
            // RMC 20141127 QA Tarlac ver (s)
            if (AppSettingsManager.GetConfigValue("56") == "Y")
                result.Query = string.Format("select * from or_used where or_no = '{0}' and or_series = '{1}'", StringUtilities.Left(txtORNo.Text.Trim(),txtORNo.Text.Trim().Length - 2), StringUtilities.Right(txtORNo.Text.Trim(),2));
            else// RMC 20141127 QA Tarlac ver (e)
                result.Query = string.Format("select * from or_used where or_no = '{0}'", txtORNo.Text.Trim());
            if (result.Execute())
                if (result.Read())
                {
                    MessageBox.Show("OR No. already used! Please use other OR...", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    result.Close();
                    return;
                }
            result.Close();

            sQuery = "select * from chk_tbl_temp where or_no = '" + txtORNo.Text.Trim() + "'";
            result.Query = sQuery;
            if (chkCheque.Checked == true)
            {
                if (result.Execute())
                    if (!result.Read())
                    {
                        MessageBox.Show("Check info not yet entered!", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        result.Close();
                        return;
                    }
                result.Close();
            }

            // CTS 11252004 add validation to tax credit
            if (chkTCredit.Checked == true && Convert.ToInt32(txtToTaxCredit.Text.Trim()) == 0)
            {
                if (MessageBox.Show("Zero amount is used for tax credit \nContinue saving?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    txtToTaxCredit.Focus();
                    return;
                }
            }
            // CTS 11252004 add validation to tax credit

            // CTS 11302004 add validation for tax credit
            if (m_sPaymentType == "")
            {
                MessageBox.Show("Payment type required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // CTS 11302004 add validation for tax credit*/ // CTS 12172004 consolidation of validation for tax credit

            // CTS 12172004 consolidated validation of tax credit

            if (chkTCredit.Checked == true)
            {
                if (chkCash.Checked == true && Convert.ToInt32(txtTotDues.Text.Trim()) == 0)
                {
                    MessageBox.Show("Cash amount required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (chkCash.Checked == true && Convert.ToInt32(txtTotDues.Text.Trim()) > 0.00)
                {
                    if (chkCheque.Checked == false)
                    {
                        String sTotalAmount = "0.00";										// ALJ 12272004 BUGS ON CONVERTION
                        sTotalAmount = Convert.ToDouble(Convert.ToDouble(txtTotDues.Text) + Convert.ToDouble(txtToTaxCredit.Text)).ToString();
                        if (Convert.ToInt32(txtDiscountAmount.Text.Trim()) > Convert.ToInt32(sTotalAmount))					// ALJ 12272004 BUGS ON CONVERTION
                        {
                            MessageBox.Show("Insufficient fund for the payment due!", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (Convert.ToInt32(txtDiscountAmount.Text.Trim()) < Convert.ToInt32(sTotalAmount))					// ALJ 12272004 BUGS ON CONVERTION
                        {
                            MessageBox.Show("Amount paid must be equal to payment due! \nyou are using cash & tax credit payment", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                if (Convert.ToDouble(txtGrandTotal.Text) > 0.00)
                {
                    if (chkCheque.Checked == false && chkCash.Checked == false && chkTCredit.Checked == false)
                    {
                        MessageBox.Show("Insufficient fund for the payment due!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        if (chkCheque.Checked == false && chkCash.Checked == false && chkTCredit.Checked == true)
                        {
                            if (Convert.ToDouble(txtGrandTotal.Text) > Convert.ToDouble(txtToTaxCredit.Text))
                            {
                                MessageBox.Show("Insufficient fund for the payment due!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                        }
                    }

                    if (chkCheque.Checked == true)
                    {
                        double dChkAmt = 0, dCashAmt = 0, dTotalAmount = 0;

                        sQuery = "select sum(chk_amt) as chk_amount, sum(cash_amt) as cash_amount";
                        sQuery += " from chk_tbl_temp where or_no = '" + txtORNo.Text.Trim() + "'";
                        result.Query = sQuery;
                        if (result.Execute())
                            if (result.Read())
                            {
                                dChkAmt = result.GetDouble("chk_amount");
                                dCashAmt = result.GetDouble("cash_amount");
                            }
                        result.Close();

                        dTotalAmount = dChkAmt + dCashAmt;

                        String sTmpdTotalAmoun = dTotalAmount.ToString();

                        if (Convert.ToDouble(txtGrandTotal.Text.Trim()) > Convert.ToDouble(sTmpdTotalAmoun))		// JJP 02062006 Fixing Online
                        {
                            MessageBox.Show("Insufficient fund for the payment due!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                }
            }

            if ((txtMemo.Text.Trim() == "" || txtMemo.Text.Trim().Length == 0) && m_sPaymentMode == "OFL")
            {
                MessageBox.Show("Memoranda required for Offline-Payment!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtMemo.Focus();
                return;
            }

            if (MessageBox.Show("Accept Payment?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m_sOrNo = txtORNo.Text.Trim(); // PASS TO TEMP
                CheckPaymentType();

                if (m_sPaymentMode != "TRANS")
                {
                    SavePaymentDues();
                    UpdateWarrant(m_sOwnCode, m_sBIN);
                }
                else
                {
                    SaveTransferDues();
                }

                /*
                if (m_iSwRE == 1 || m_iSwRE == 2)
                    AuditTrail.InsertTrail("CAP-RE", "multiple table", m_sBIN);
                else
                */
                AuditTrail.InsertTrail("CAP", "multiple table", m_sBIN);

                string sCashReceive = string.Empty;
                string sCashChange = string.Empty;
                string sChange = string.Empty;
                sCashReceive = "0.00";
                sChange = "0.00";
                frmCashTender fCashTender = new frmCashTender();
                fCashTender.lblDueAmount.Text = this.txtGrandTotal.Text.Trim();
                fCashTender.m_sOrigAmtDue = m_sDiscountedTotal;
                fCashTender.lblChange.Text = "0.00";

                if (chkTCredit.Checked == false)
                {
                    if (Convert.ToDouble(txtCreditLeft.Text) > Convert.ToDouble(m_sDiscountedTotal))
                    {
                        fCashTender.m_sTaxCreditAmount = Convert.ToDouble(txtCreditLeft.Text).ToString("##0.00");
                        sChange = string.Format("{0:##0.00}", Convert.ToDouble(txtCreditLeft.Text) - Convert.ToDouble(txtToTaxCredit.Text));
                    }
                    else
                        fCashTender.m_sTaxCreditAmount = Convert.ToDouble(txtToTaxCredit.Text).ToString("##0.00");
                }
                else
                {
                    fCashTender.m_sTaxCreditAmount = "0";
                }

                if (chkCheque.Checked == true)
                {
                    result.Query = "select * from dbcr_memo where or_no = '" + txtORNo.Text.Trim() + "'"; //JARS 20171010
                    result.Query += " and multi_pay = 'N' and balance <> '0.00'";
                    result.Query += " and memo like 'REMAIN%'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sChange = result.GetDouble("balance").ToString();
                            if (m_bAttachedExcessAmount == true)
                            {
                                dCCChkAmt = double.Parse(m_sCCChkAmt) + double.Parse(sChange);
                                m_sCCChkAmt = dCCChkAmt.ToString("#,##0.00");
                            }
                        }
                    }
                    result.Close();

                    fCashTender.txtCashAmount.Text = m_sCCCashAmt;
                    fCashTender.txtCheckAmount.Text = m_sCCChkAmt;
                }

                fCashTender.m_sExcessChange = sChange;
                m_sCheckChange = sChange;
                fCashTender.m_sBIN = m_sBIN.Trim();
                fCashTender.m_sOrNO = txtORNo.Text.Trim();
                fCashTender.m_sPaymentType = m_sPaymentType;
                fCashTender.m_sPaymentMode = m_sPaymentMode; //MCR 20140812
                fCashTender.txtBalance.Text = txtTotTotDue.Text;

                fCashTender.ShowDialog();
                sCashReceive = fCashTender.txtCashTendered.Text.Trim();
                sCashChange = fCashTender.lblChange.Text.Trim();

                if (sCashReceive == string.Empty)
                    sCashReceive = "0";

                OracleResultSet pSet = new OracleResultSet();
                sQuery = "insert into teller_cashtender values (";
                sQuery += "'" + AppSettingsManager.TellerUser.UserCode + "',";
                sQuery += "'" + m_sBIN + "',";
                sQuery += Convert.ToDouble(txtGrandTotal.Text).ToString("##0.00") + ",";
                if (m_sCCChkAmt.Trim() == "")
                    m_sCCChkAmt = "0.00";
                sQuery += Convert.ToDouble(m_sCCChkAmt).ToString("##0.00") + ",";
                sQuery += Convert.ToDouble(sCashReceive).ToString("##0.00") + ",";
                if (sCashChange.Trim() == "")
                    sCashChange = "0.00";
                sQuery += Convert.ToDouble(sCashChange).ToString("##0.00") + ",";
                sQuery += "to_date('" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy HH:mm:ss") + "','MM/dd/yyyy HH24:MI:SS'))";
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();

                GetLastTransaction(txtTeller.Text.Trim());
                ClearSOA();
                //ReportClass rClass = new ReportClass();
                //rClass.OfficialReceipt(txtORNo.Text.Trim(), m_sBIN, m_sPaymentTerm, m_sQtr, txtBnsStat.Text.Trim());
                dgvTaxFees.Rows.Clear();
                CheckPaymentMode();
                btnSearchBin.PerformClick();
                bin.txtTaxYear.Focus();
            }
            else
            {
                MessageBox.Show("NOT ACCEPT");
            }
        }

        private void OnAcceptPaymentNew()   // RMC 20140909 Migration QA -- modified this function
        {
            double dCCChkAmt = 0;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();

            try
            {
                if (m_bIsDisCompromise == true)
                {
                    if (Convert.ToDouble(txtCompromise.Text) < 0 || Convert.ToDouble(txtCompromise.Text) > m_iNoPaySeq)
                    {
                        MessageBox.Show("Invalid Compromise Due to Pay.", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtCompromise.Focus();
                        return;
                    }
                }

                if (chkCash.Checked == false && chkCheque.Checked == false && chkTCredit.Checked == false)
                {
                    MessageBox.Show("Payment type required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                String sQuery, m_sOrNo;

                m_sOrNo = txtORNo.Text.Trim();
                result.Query = string.Format("select distinct or_no from pay_hist where or_no = '{0}'", txtORNo.Text.Trim());
                if (result.Execute())
                    if (result.Read())
                    {
                        MessageBox.Show("Duplicate OR no.", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        result.Close();
                        return;
                    }
                result.Close();

                //(s) RTL 11252005 for auto or non-auto generated or
                if (AppSettingsManager.IfAutoGeneratedOR() != true)
                {
                    //result.Query = string.Format("select * from or_current where {0} between from_or_no and to_or_no and teller = '{1}'", Convert.ToInt32(txtORNo.Text.Trim().Substring(2)), AppSettingsManager.TellerUser.UserCode);
                    // RMC 20141127 QA Tarlac ver (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                    {
                        string sOrNo = StringUtilities.Left(txtORNo.Text.Trim(), txtORNo.Text.Trim().Length - 2);
                        string sOrSeries = StringUtilities.Right(txtORNo.Text.Trim(), 2);

                        result.Query = "select * from or_current where " + sOrNo + " between from_or_no and to_or_no ";
                        result.Query += " and teller = '" + AppSettingsManager.TellerUser.UserCode + "'";
                        result.Query += " and or_series = '" + sOrSeries + "'";
                    }
                    else
                    {
                        result.Query = "select * from or_current where " + txtORNo.Text.Trim() + " between from_or_no and to_or_no ";
                        result.Query += " and teller = '" + AppSettingsManager.TellerUser.UserCode + "'";
                    }
                    // RMC 20141127 QA Tarlac ver (e)
                    if (result.Execute())
                        if (!result.Read())
                        {
                            MessageBox.Show("OR No. is not included in the OR declaration ...", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            result.Close();
                            return;
                        }
                    result.Close();
                }

                // RMC 20141127 QA Tarlac ver (s)
                if (AppSettingsManager.GetConfigValue("56") == "Y")
                    result.Query = string.Format("select * from or_used where or_no = '{0}' and or_series = '{1}'", StringUtilities.Left(txtORNo.Text.Trim(), txtORNo.Text.Trim().Length - 2), StringUtilities.Right(txtORNo.Text.Trim(), 2));
                else// RMC 20141127 QA Tarlac ver (e)
                    result.Query = string.Format("select * from or_used where or_no = '{0}'", txtORNo.Text.Trim());
                if (result.Execute())
                    if (result.Read())
                    {
                        MessageBox.Show("OR No. already used! Please use other OR...", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        result.Close();
                        return;
                    }
                result.Close();

                sQuery = "select * from chk_tbl_temp where or_no = '" + txtORNo.Text.Trim() + "'";
                result.Query = sQuery;
                if (chkCheque.Checked == true)
                {
                    if (result.Execute())
                        if (!result.Read())
                        {
                            MessageBox.Show("Check info not yet entered!", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            result.Close();
                            return;
                        }
                    result.Close();
                }

                // CTS 11252004 add validation to tax credit
                //if (chkTCredit.Checked == true && Convert.ToInt32(txtToTaxCredit.Text.Trim()) == 0)
                if (chkTCredit.Checked == true && Convert.ToDouble(txtToTaxCredit.Text.Trim()) == 0)    // RMC 20140909 Migration QA
                {
                    if (MessageBox.Show("Zero amount is used for tax credit \nContinue saving?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        txtToTaxCredit.Focus();
                        return;
                    }
                }
                // CTS 11252004 add validation to tax credit


                // CTS 11302004 add validation for tax credit
                if (m_sPaymentType == "")
                {
                    MessageBox.Show("Payment type required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                #region comments
                // CTS 11302004 add validation for tax credit*/ // CTS 12172004 consolidation of validation for tax credit

                // CTS 12172004 consolidated validation of tax credit

                //if (chkTCredit.Checked == true)   // RMC 20170119 added validation in check payment if check amount entered is less than the tax due, put rem
                {
                    //if (chkCash.Checked == true && Convert.ToInt32(txtTotDues.Text.Trim()) == 0)
                    /*if (chkCash.Checked == true && Convert.ToDouble(txtTotDues.Text.Trim()) == 0)   // RMC 20141203 corrected error in online payment
                    {
                        MessageBox.Show("Cash amount required!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }*/
                    // RMC 20170119 added validation in check payment if check amount entered is less than the tax due, put rem

                    //if (chkCash.Checked == true && Convert.ToInt32(txtTotDues.Text.Trim()) > 0.00)
                    /*if (chkCash.Checked == true && Convert.ToDouble(txtTotDues.Text.Trim()) > 0.00)    // RMC 20141203 corrected error in online payment
                    {
                        if (chkCheque.Checked == false)
                        {
                            String sTotalAmount = "0.00";										// ALJ 12272004 BUGS ON CONVERTION
                            sTotalAmount = Convert.ToDouble(Convert.ToDouble(txtTotDues.Text) + Convert.ToDouble(txtToTaxCredit.Text)).ToString();
                            //if (Convert.ToInt32(txtDiscountAmount.Text.Trim()) > Convert.ToInt32(sTotalAmount))					// ALJ 12272004 BUGS ON CONVERTION
                            if (Convert.ToDouble(txtDiscountAmount.Text.Trim()) > Convert.ToDouble(sTotalAmount))   // RMC 20141203 corrected error in online payment
                            {
                                MessageBox.Show("Insufficient fund for the payment due!", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            //if (Convert.ToInt32(txtDiscountAmount.Text.Trim()) < Convert.ToInt32(sTotalAmount))					// ALJ 12272004 BUGS ON CONVERTION
                            if (Convert.ToDouble(txtDiscountAmount.Text.Trim()) < Convert.ToDouble(sTotalAmount))// RMC 20141203 corrected error in online payment
                            {
                                MessageBox.Show("Amount paid must be equal to payment due! \nyou are using cash & tax credit payment", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }*/
                    // RMC 20141203 corrected error in online payment, transferred
                #endregion
                    if (Convert.ToDouble(txtGrandTotal.Text) > 0.00)
                    {
                        if (chkCheque.Checked == false && chkCash.Checked == false && chkTCredit.Checked == false)
                        {
                            MessageBox.Show("Insufficient fund for the payment due!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        else
                        {
                            if (chkCheque.Checked == false && chkCash.Checked == false && chkTCredit.Checked == true)
                            {
                                if (Convert.ToDouble(txtGrandTotal.Text) > Convert.ToDouble(txtToTaxCredit.Text))
                                {
                                    MessageBox.Show("Insufficient fund for the payment due!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }
                            }
                        }

                        /*if (chkCheque.Checked == true)
                        {
                            double dChkAmt = 0, dCashAmt = 0, dTotalAmount = 0;

                            sQuery = "select sum(chk_amt) as chk_amount, sum(cash_amt) as cash_amount";
                            sQuery += " from chk_tbl_temp where or_no = '" + txtORNo.Text.Trim() + "'";
                            result.Query = sQuery;
                            if (result.Execute())
                                if (result.Read())
                                {
                                    dChkAmt = result.GetDouble("chk_amount");
                                    dCashAmt = result.GetDouble("cash_amount");
                                }
                            result.Close();

                            dTotalAmount = dChkAmt + dCashAmt;

                            String sTmpdTotalAmoun = dTotalAmount.ToString();

                            // RMC 20151209 corrections in cash, cheque, tax credit payment combination (s)
                            // returned this block {
                            if (Convert.ToDouble(txtGrandTotal.Text.Trim()) > Convert.ToDouble(sTmpdTotalAmoun))		// JJP 02062006 Fixing Online //JARS 20151202 HAD TO COMMENT OUT. For Transaciton Combination of Tax Credit, Cash and Cheque Payment. 
                            {
                                MessageBox.Show("Insufficient fund for the payment due!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            // }
                            // RMC 20151209 corrections in cash, cheque, tax credit payment combination (e)
                        }*/
                        // RMC 20170119 added validation in check payment if check amount entered is less than the tax due, put rem
                    }
                }

                if ((txtMemo.Text.Trim() == "" || txtMemo.Text.Trim().Length == 0) && m_sPaymentMode == "OFL")
                {
                    MessageBox.Show("Memoranda required for Offline-Payment!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtMemo.Focus();
                    return;
                }

                // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin (s)
                double dTmpTotDue = 0;
                double.TryParse(txtTotTotDue.Text.ToString(), out dTmpTotDue);
                if (dTmpTotDue == 0)
                {
                    MessageBox.Show("No amount to pay.\nPlease check.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                // RMC 20151111 enable unchecking/checking of current quarter in Online payment even if the Business was delinquent - requested by Tumauini thru Jeslin (e)

                // RMC 20151229 merged multiple OR use from Mati (s)
                OracleResultSet pOR = new OracleResultSet();
                int iRow = 0;

                /*pOR.Query = "select count(distinct tax_code), tax_year from taxdues where bin = '" + m_sBIN + "' and tax_code <> 'B' group by tax_year";
                if (pOR.Execute())
                {
                    //if (pOR.Read()) // AST 20160125
                    while (pOR.Read())
                        iRow += pOR.GetInt(0);
                }
                pOR.Close();

                //pOR.Query = "select count(*), tax_year from taxdues where bin = '" + m_sBIN + "' and tax_code = 'B' group by tax_year"; // AST 20160125 rem
                pOR.Query = "select count(distinct bns_code_main) from taxdues where bin = '" + m_sBIN + "' and tax_code = 'B' "; // AST 20160125
                if (pOR.Execute())
                {
                    //if (pOR.Read()) // AST 20160125
                    while (pOR.Read())
                        iRow += pOR.GetInt(0);
                }
                pOR.Close();*/
                // RMC 20160503 correction in saving payment using multiple OR, put rem


                iRow = dgvTaxFees.Rows.Count;   // RMC 20160503 correction in saving payment using multiple OR

                m_iORNo = (int)iRow / 14;                
                int iRem = iRow % 14;                 
                string sORNo = string.Empty;
                int iORCount = 0;
                double dDebitExtra = 0.00;

                if (iRem > 0)
                    m_iORNo += 1;

                if (m_iORNo > 1)
                {
                    sORNo = string.Format("{0:#######}", m_iORNo);

                    if (MessageBox.Show("This transaction requires " + sORNo + " O.R.s.\nContinue?", "Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    else
                    {
                        //compute remaining teller OR
                        if (m_iORNo > GetRemainingOR(AppSettingsManager.TellerUser.UserCode))
                        {
                            MessageBox.Show("Insufficient O.R. form available", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }

                        txtMemo.Text = "MULTIPLE O.R.: " + txtORNo.Text;

                        int iTmpOr = 0;
                        int.TryParse(txtORNo.Text, out iTmpOr);

                        for (int ix = 1; ix < m_iORNo; ix++)
                        {
                            iTmpOr++;
                            //sORNo = string.Format("{0:#######}", iTmpOr);
                            sORNo = string.Format("{0:0######}", iTmpOr);   // RMC 20170106 correction in saving OR no. in multiple OR use
                            txtMemo.Text += ", " + sORNo;
                        }


                    }
                }
                else
                {
                    m_iORNo = 1;
                }
                // RMC 20151229 merged multiple OR use from Mati (e)

                // RMC 20141020 printing of OR (s)
                //if (MessageBox.Show("O.R. form no. " + txtORNo.Text + " ready?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) // AST 20150427 Reqt'd to change prompt msg.

                // RMC 20151229 merged multiple OR use from Mati (s)
                if (m_iORNo > 1)
                {
                    string sTmpOR = txtMemo.Text;
                    sTmpOR = sTmpOR.Replace("MULTIPLE O.R.: ", "");
                    m_sTellerOR = sTmpOR; //JARS 20170823
                    if (MessageBox.Show("O.R. numbers " + sTmpOR + " ready?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }// RMC 20151229 merged multiple OR use from Mati (e)
                else
                {
                    if (MessageBox.Show("O.R. number " + txtORNo.Text + " ready?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    m_sTellerOR = txtORNo.Text; //JARS 20170824
                }
                // RMC 20141020 printing of OR (e)


                if (MessageBox.Show("Accept Payment?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m_sOrNo = txtORNo.Text.Trim(); // PASS TO TEMP
                    CheckPaymentType();

                    bool blnForApproval = false;
                    if (CheckLatestPayment()) // if true, bin is subject to approval
                        blnForApproval = true;

                    string sCashReceive = string.Empty;
                    string sCashChange = string.Empty;
                    string sChange = string.Empty;
                    sCashReceive = "0.00";
                    sChange = "0.00";
                    frmCashTender fCashTender = new frmCashTender();
                    fCashTender.lblDueAmount.Text = this.txtGrandTotal.Text.Trim();
                    fCashTender.m_sOrigAmtDue = m_sDiscountedTotal;
                    fCashTender.lblChange.Text = "0.00";

                    if (chkTCredit.Checked == false)
                    {
                        if (Convert.ToDouble(txtCreditLeft.Text) > Convert.ToDouble(m_sDiscountedTotal))
                        {
                            fCashTender.m_sTaxCreditAmount = Convert.ToDouble(txtCreditLeft.Text).ToString("##0.00");
                            sChange = string.Format("{0:##0.00}", Convert.ToDouble(txtCreditLeft.Text) - Convert.ToDouble(txtToTaxCredit.Text));
                        }
                        else
                            fCashTender.m_sTaxCreditAmount = Convert.ToDouble(txtToTaxCredit.Text).ToString("##0.00");
                    }
                    else
                    {
                        fCashTender.m_sTaxCreditAmount = "0";
                    }

                    if (chkCheque.Checked == true)
                    {
                        fCashTender.txtCashAmount.Text = m_sCCCashAmt;
                        fCashTender.txtCheckAmount.Text = m_sCCChkAmt;

                        // RMC 20170119 added validation in check payment if check amount entered is less than the tax due (s)
                        if (Convert.ToDouble(m_sCCCashAmt) > 0)
                        {
                            chkCash.Checked = true;
                            CheckPaymentType();
                        }
                        // RMC 20170119 added validation in check payment if check amount entered is less than the tax due (e)
                    }

                    //fCashTender.m_sExcessChange = sChange;
                    //
                    fCashTender.m_sBIN = m_sBIN.Trim();
                    fCashTender.m_sOrNO = txtORNo.Text.Trim();
                    fCashTender.m_sPaymentType = m_sPaymentType;
                    fCashTender.m_sPaymentMode = m_sPaymentMode; //MCR 20140812
                    fCashTender.txtBalance.Text = txtTotTotDue.Text;

                    fCashTender.ShowDialog();

                    if (fCashTender.m_bCancel)
                    {
                        return;
                    }

                    sCashReceive = fCashTender.txtCashTendered.Text.Trim();
                    sCashChange = fCashTender.lblChange.Text.Trim();

                    if (sCashReceive == string.Empty)
                        sCashReceive = "0";

                    // RMC 20141203 corrected error in online payment (s)
                    // transferred
                    if (chkCash.Checked == true && Convert.ToDouble(txtTotDues.Text.Trim()) > 0.00)    // RMC 20141203 corrected error in online payment
                    {
                        if (chkCheque.Checked == false)
                        {
                            String sTotalAmount = "0.00";										// ALJ 12272004 BUGS ON CONVERTION
                            sTotalAmount = Convert.ToDouble(Convert.ToDouble(txtTotDues.Text) + Convert.ToDouble(txtToTaxCredit.Text)).ToString();

                            /*if (Convert.ToInt32(txtDiscountAmount.Text.Trim()) > Convert.ToInt32(sTotalAmount))					// ALJ 12272004 BUGS ON CONVERTION
                            {
                                MessageBox.Show("Insufficient fund for the payment due!", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }*/
                            // RMC 20141203 corrected error in online payment, put rem

                            //if (Convert.ToInt32(txtDiscountAmount.Text.Trim()) < Convert.ToInt32(sTotalAmount))					// ALJ 12272004 BUGS ON CONVERTION
                            if ((Convert.ToDouble(sCashReceive) - Convert.ToDouble(sCashChange) + Convert.ToDouble(txtToTaxCredit.Text)) < Convert.ToDouble(sTotalAmount))// RMC 20141203 corrected error in online payment
                            {
                                MessageBox.Show("Amount paid must be equal to payment due! \nYou are using cash & tax credit payment", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                    // RMC 20141203 corrected error in online payment (e)


                    if (m_sPaymentMode != "TRANS")
                    {
                        SavePaymentDues();
                        UpdateWarrant(m_sOwnCode, m_sBIN);
                    }
                    else
                    {
                        SaveTransferDues();
                    }

                    if (chkCheque.Checked == true)
                    {
                        result.Query = "select * from dbcr_memo where or_no = '" + txtORNo.Text.Trim() + "'"; //JARS 20171010
                        result.Query += " and multi_pay = 'N' and balance <> '0.00'";
                        result.Query += " and memo like 'REMAIN%'";
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                sChange = result.GetDouble("balance").ToString();
                                if (m_bAttachedExcessAmount == true)
                                {
                                    dCCChkAmt = double.Parse(m_sCCChkAmt) + double.Parse(sChange);
                                    //m_sCCChkAmt = dCCChkAmt.ToString("#,##0.00");
                                }
                            }
                        }
                        result.Close();
                    }

                    OracleResultSet pSet = new OracleResultSet();
                    sQuery = "insert into teller_cashtender values (";
                    sQuery += "'" + AppSettingsManager.TellerUser.UserCode + "',";
                    sQuery += "'" + m_sBIN + "',";
                    sQuery += Convert.ToDouble(txtGrandTotal.Text).ToString("##0.00") + ",";
                    if (m_sCCChkAmt.Trim() == "")
                        m_sCCChkAmt = "0.00";
                    sQuery += Convert.ToDouble(m_sCCChkAmt).ToString("##0.00") + ",";
                    sQuery += Convert.ToDouble(sCashReceive).ToString("##0.00") + ",";
                    if (sCashChange.Trim() == "")
                        sCashChange = "0.00";
                    sQuery += Convert.ToDouble(sCashChange).ToString("##0.00") + ",";
                    sQuery += "to_date('" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy HH:mm:ss") + "','MM/dd/yyyy HH24:MI:SS'))";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();

                    m_sCheckChange = sChange;
                    double fAmountDueT = 0;
                    double fExcessAmountT = 0;
                    double fCheckAmountT = 0;
                    double fTaxCreditAmountT = 0;
                    double.TryParse(fCashTender.m_sAmountDue, out fAmountDueT); 
                    double.TryParse(sChange, out fExcessAmountT);
                    double.TryParse(m_sCCChkAmt, out fCheckAmountT);
                    double.TryParse(fCashTender.m_sTaxCreditAmount, out fTaxCreditAmountT);

                    //JARS 20170823 (s)
                    if (m_sPaymentType.Trim() == "CQ" || m_sPaymentType.Trim() == "CQTC" || m_sPaymentType.Trim() == "TC")
                    {
                        double.TryParse(fCashTender.m_sExcessChange, out fExcessAmountT); //JARS 20170905
                        AppSettingsManager.TellerTransaction(AppSettingsManager.TellerUser.UserCode, m_sPaymentMode, m_sBIN, m_sTellerOR, fAmountDueT, fTaxCreditAmountT, fCheckAmountT, Convert.ToDouble(0), fExcessAmountT, m_sPaymentType);
                    }
                    else
                    {
                        double fCashRenderedT = 0;
                        double.TryParse(fCashTender.txtCashTendered.Text, out fCashRenderedT);
                        double.TryParse(fCashTender.m_sExcessChange, out fExcessAmountT);
                        AppSettingsManager.TellerTransaction(AppSettingsManager.TellerUser.UserCode, m_sPaymentMode, m_sBIN, m_sTellerOR, fAmountDueT, fTaxCreditAmountT, fCheckAmountT, fCashRenderedT, fExcessAmountT, m_sPaymentType);
                    }
                    //JARS 20170823 (e)
                    #region comments
                    //if (m_sPaymentType.Trim() == "CQ" || m_sPaymentType.Trim() == "CQTC" || m_sPaymentType.Trim() == "TC")
                    //    AppSettingsManager.TellerTransaction(AppSettingsManager.TellerUser.UserCode, m_sPaymentMode, m_sBIN, txtORNo.Text.Trim(), fAmountDueT, fTaxCreditAmountT, fCheckAmountT, Convert.ToDouble(0), fExcessAmountT, m_sPaymentType);
                    //else
                    //{
                    //    double fCashRenderedT = 0;
                    //    double.TryParse(fCashTender.txtCashTendered.Text, out fCashRenderedT);
                    //    double.TryParse(fCashTender.m_sExcessChange, out fExcessAmountT);
                    //    AppSettingsManager.TellerTransaction(AppSettingsManager.TellerUser.UserCode, m_sPaymentMode, m_sBIN, txtORNo.Text.Trim(), fAmountDueT, fTaxCreditAmountT, fCheckAmountT, fCashRenderedT, fExcessAmountT, m_sPaymentType);
                    //}
                    #endregion

                    AuditTrail.InsertTrail("CAP", "multiple table", m_sBIN);
                    
                    //MCR 20210708 (s)
                    if (AppSettingsManager.GetConfigValue("75") == "Y")
                    {
                        String sValue = "", sRefNo = "";
                        sRefNo = GetSOARefNo(m_sBIN);
                        pSet = new OracleResultSet();
                        pSet.Query = "select BO_VOID_REF_NO('" + sRefNo + "') from dual";
                        if (pSet.Execute())
                            if (pSet.Read())
                                sValue = pSet.GetString(0);
                        pSet.Close();

                        //MCR 20210927 (s)
                        String sBillNo = AppSettingsManager.GetBillNoAndDate(bin.GetBin(), txtTaxYear.Text, txtBnsCode.Text, 0);
                        pSet.Query = "select * from business_que where bin = '" + bin.GetBin() + "' and bns_user = 'SYS_PROG'"; // to check if from online registration
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                pSet = new OracleResultSet();
                                sQuery = "insert into multisys_ref values (";
                                sQuery += "'" + sBillNo + "',";
                                sQuery += "'" + sRefNo + "',";
                                sQuery += fAmountDueT + ",";
                                sQuery += 1 + ",";
                                sQuery += "to_date('" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy HH:mm:ss") + "','MM/dd/yyyy HH24:MI:SS'))";
                                pSet.Query = sQuery;
                                pSet.ExecuteNonQuery();
                            }
                        pSet.Close();

                        pSet.Query = "select BO_SEND_PAID_NOTIF('" + bin.GetBin() + "'," + fAmountDueT + ",1,'" + AppSettingsManager.TellerUser.UserCode + "') from dual";
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                                sValue = pSet.GetString(0);
                        }
                        pSet.Close();

                        //MCR 20210927 (s)
                    }
                    //MCR 20210708 (e)

                    MessageBox.Show("Payment saved", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information); // RMC 20140909 Migration QA

                    PrintOR();  // RMC 20141020 printing of OR

                    txtORNo.Text = GetCurrentOR(AppSettingsManager.TellerUser.UserCode);  // RMC 20140909 Migration QA

                    GetLastTransaction(txtTeller.Text.Trim());

                    if(m_sStatus == "N") //AFM 20211207 REQUESTED CTC ON SOA - delete after payment
                    {
                        pSet.Query = "DELETE FROM CTC_TABLE WHERE BIN = '" + m_sBIN + "'";
                        if (pSet.ExecuteNonQuery() == 0)
                        { }
                    }

                    //AFM 20211217 MAO-21-16197	PAID BUSINESS APPROVAL BEFORE APPROVAL OF MAYOR (s)
                    if (blnForApproval && txtBnsStat.Text != "RET")
                    {
                        pSet.Query = "INSERT INTO BUSINESS_APPROVAL VALUES(";
                        pSet.Query += "'" + bin.GetBin() + "',";
                        pSet.Query += "'" + txtTaxYear.Text + "',";
                        pSet.Query += "'" + AppSettingsManager.GetBnsCodeMain(bin.GetBin()) + "',";
                        pSet.Query += "'PENDING',";
                        pSet.Query += "'',";
                        pSet.Query += "'',";
                        pSet.Query += "'',";
                        pSet.Query += "'')";
                        if (pSet.ExecuteNonQuery() == 0)
                        { }
                    }
                    
                    //AFM 20211217 MAO-21-16197	PAID BUSINESS APPROVAL BEFORE APPROVAL OF MAYOR (e)

                    ClearSOA();
                    //ReportClass rClass = new ReportClass();
                    //rClass.OfficialReceipt(txtORNo.Text.Trim(), m_sBIN, m_sPaymentTerm, m_sQtr, txtBnsStat.Text.Trim());
                    dgvTaxFees.Rows.Clear();
                    CheckPaymentMode();
                    btnSearchBin.PerformClick();
                    bin.txtTaxYear.Focus();
                }
                else
                {
                    MessageBox.Show("NOT ACCEPT");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Payment", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckLatestPayment() //AFM 20211220 (in relation to paid business approval) check if already paid at least 1st quarter for current year
        {
            int cnt = 0;
            OracleResultSet res = new OracleResultSet();
            if (txtTaxYear.Text != AppSettingsManager.GetSystemDate().Year.ToString()) // for discovery; will not save to approval until paid for latest year
                return false;
            res.Query = "select count(*) from pay_hist where bin = '" + bin.GetBin() + "' and tax_year = '" + AppSettingsManager.GetSystemDate().Year.ToString() + "' and (qtr_paid = 'F' OR qtr_paid = '1')";
            int.TryParse(res.ExecuteScalar(), out cnt);
            if (cnt == 0)
                return true;
            else
                return false;
        }

        private string GetSOARefNo(string sBin)
        {
            String sValue = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select refno from soa_monitoring where bin = '" + sBin + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sValue = pSet.GetString(0);
            pSet.Close();

            return sValue;
        }

        private void ClearSOA()
        {
            bool bWatch = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from billing_tagging  where bin = '" + m_sBIN.Trim() + "' and report = 'SOA' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bWatch = true;
            }
            result.Close();

            if (bWatch == true)
            {
                result.Query = "delete from billing_tagging where bin = '" + m_sBIN.Trim() + "' and report = 'SOA' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();
            }
        }

        private void txtCreditLeft_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkFull_Click(object sender, EventArgs e)
        {
            chkFull.Checked = true;
            chkInstallment.Checked = false;
            chk1st.Checked = false;
            chk2nd.Checked = false;
            chk3rd.Checked = false;
            chk4th.Checked = false;

            if (chkFull.Checked == true)
            {
                dgvTaxFees.Rows.Clear();
                chk1st.Enabled = false;
                chk2nd.Enabled = false;
                chk3rd.Enabled = false;
                chk4th.Enabled = false;

                chk1st.Checked = false;
                chk2nd.Checked = false;
                chk3rd.Checked = false;
                chk4th.Checked = false;
                OnFull();
            }
        }

        private void chkInstallment_Click(object sender, EventArgs e)
        {
            chkFull.Checked = false;
            chkInstallment.Checked = true;

            if (chkInstallment.Checked == true)
            {
                dgvTaxFees.Rows.Clear();    // RMC 20160719 correction in payment module double loading of adj values
                chkInstallment.Checked = true;
                chkFull.Checked = false;
                chk1st.Enabled = true;
                chk2nd.Enabled = true;
                chk3rd.Enabled = true;
                chk4th.Enabled = true;
                m_sTerm = "I";
                OnInstallmentNew(m_sBIN);
            }
        }

        /*
        private void chk1st_Click(object sender, EventArgs e)
        {
            if (chk1st.Checked == true)
            {
                chk2nd.Checked = false;
                chk3rd.Checked = false;
                chk4th.Checked = false;
                //chkInstallment.Checked = true;
                //chkFull.Checked = false;
            }
            else
            {
                if (chkRetirement.Checked == true || chkAdj.Checked == true)
                {
                    chkFull.Checked = true;
                    chkInstallment.Checked = false;
                }
                else
                    chk1st.Checked = true;
            }

            string sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4;
            if (chk1st.Checked == true)
                sQToBePaid1 = "1";
            else
                sQToBePaid1 = string.Empty;

            if (chk2nd.Checked == true)
                sQToBePaid2 = "2";
            else
                sQToBePaid2 = string.Empty;

            if (chk3rd.Checked == true)
                sQToBePaid3 = "3";
            else
                sQToBePaid3 = string.Empty;

            if (chk4th.Checked == true)
                sQToBePaid4 = "4";
            else
                sQToBePaid4 = string.Empty;

            dgvTaxFees.Rows.Clear();
            DisplayDues(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            if (chkAdj.Checked == true)
                OnCheckAdj();
            if (chkRetirement.Checked == true)
                OnCheckRetirement();
        }

        private void chk2nd_Click(object sender, EventArgs e)
        {
            if (chk2nd.Checked == true)
            {
                chk3rd.Checked = false;
                chk4th.Checked = false;
                //chkInstallment.Checked = true;
                //chkFull.Checked = false;
            }
            else
            {
                if (chk1st.Checked == false)
                {
                    if (chkRetirement.Checked == true || chkAdj.Checked == true)
                    {
                        chkFull.Checked = true;
                        chkInstallment.Checked = false;
                    }
                    else
                        chk2nd.Checked = true;
                }
                else
                {
                    chk3rd.Checked = false;
                    chk4th.Checked = false;
                }

            }

            string sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4;
            if (chk1st.Checked == true)
                sQToBePaid1 = "1";
            else
                sQToBePaid1 = string.Empty;

            if (chk2nd.Checked == true)
                sQToBePaid2 = "2";
            else
                sQToBePaid2 = string.Empty;

            if (chk3rd.Checked == true)
                sQToBePaid3 = "3";
            else
                sQToBePaid3 = string.Empty;

            if (chk4th.Checked == true)
                sQToBePaid4 = "4";
            else
                sQToBePaid4 = string.Empty;

            dgvTaxFees.Rows.Clear();
            DisplayDues(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            if (chkAdj.Checked == true)
                OnCheckAdj();
            if (chkRetirement.Checked == true)
                OnCheckRetirement();
        }

        private void chk3rd_Click(object sender, EventArgs e)
        {
            if (chk3rd.Checked == true)
            {
                chk4th.Checked = false;
                //chkInstallment.Checked = true;
                //chkFull.Checked = false;
            }
            else
            {
                if (chk1st.Checked == false && chk2nd.Checked == false)
                {
                    if (chkRetirement.Checked == true || chkAdj.Checked == true)
                    {
                        chkFull.Checked = true;
                        chkInstallment.Checked = false;
                    }
                    else
                        chk3rd.Checked = true;
                }
                else
                {
                    chk4th.Checked = false;
                }

            }

            string sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4;
            if (chk1st.Checked == true)
                sQToBePaid1 = "1";
            else
                sQToBePaid1 = string.Empty;

            if (chk2nd.Checked == true)
                sQToBePaid2 = "2";
            else
                sQToBePaid2 = string.Empty;

            if (chk3rd.Checked == true)
                sQToBePaid3 = "3";
            else
                sQToBePaid3 = string.Empty;

            if (chk4th.Checked == true)
                sQToBePaid4 = "4";
            else
                sQToBePaid4 = string.Empty;

            dgvTaxFees.Rows.Clear();
            DisplayDues(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            if (chkAdj.Checked == true)
                OnCheckAdj();
            if (chkRetirement.Checked == true)
                OnCheckRetirement();
        }

        private void chk4th_Click(object sender, EventArgs e)
        {
            if (chk4th.Checked == true)
            {
                if ((chk1st.Checked == true) && (chk2nd.Checked == false))
                {
                    chk1st.Checked = false;
                }
                if ((chk1st.Checked == true) && (chk3rd.Checked == false))
                {
                    chk1st.Checked = false;
                    chk2nd.Checked = false;
                }
                if ((chk2nd.Checked == true) && (chk3rd.Checked == false))
                {
                    chk3rd.Checked = false;
                    chk2nd.Checked = false;
                }
            }
            else
            {
                if (chk1st.Checked == false && chk2nd.Checked == false && chk3rd.Checked == false)
                {
                    if (chkAdj.Checked == true || chkRetirement.Checked == true)
                    {
                        chkFull.Checked = true;
                        chkInstallment.Checked = false;
                    }
                    else
                    {
                        chk4th.Checked = true;
                    }
                }
            }

            if (m_sNextQtrToPay == "1" && chk3rd.Checked == false)
            {
                chk4th.Checked = false;
            }

            if (m_sNextQtrToPay == "2" && chk3rd.Checked == false)
            {
                chk4th.Checked = false;
            }

            if (m_sNextQtrToPay == "3" && chk3rd.Checked == false)
            {
                chk4th.Checked = false;
            }

            string sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4;
            if (chk1st.Checked == true)
                sQToBePaid1 = "1";
            else
                sQToBePaid1 = string.Empty;

            if (chk2nd.Checked == true)
                sQToBePaid2 = "2";
            else
                sQToBePaid2 = string.Empty;

            if (chk3rd.Checked == true)
                sQToBePaid3 = "3";
            else
                sQToBePaid3 = string.Empty;

            if (chk4th.Checked == true)
                sQToBePaid4 = "4";
            else
                sQToBePaid4 = string.Empty;

            dgvTaxFees.Rows.Clear();
            DisplayDues(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            if (chkAdj.Checked == true)
                OnCheckAdj();
            if (chkRetirement.Checked == true)
                OnCheckRetirement();
        }
        */

        //(s) 02282014 MCR

        public bool FeeQtrlyCheck(string p_sBIN, String p_sTaxYear, String p_sFeeCode, String p_sBnsCode)
        {
            // ALJ 12232005 chk if paid qtrly -- (due to change of fee/tax configuration from I to F)
            String sQuery;

            //if (p_sFeeCode == "03" || (p_sFeeCode.Left(1) == 'B' && p_sBnsCode.Left(2) == "22")) // GARBAGE/Delivery Van temporary hard coded -- revise code in the future
            //	if (p_sFeeCode == "03" || (p_sFeeCode.Left(1) == 'B' && p_sBnsCode.Left(3) == "B22")) // GARBAGE/Delivery Van temporary hard coded -- revise code in the future		// JJP 03142006 Correct 2nd quarter payment - do not include delivery vehicle
            /*	// RTL 03082007 put rem
            if (p_sFeeCode == "03" || (p_sFeeCode.Left(1) == 'B' && p_sBnsCode.Left(2) == "22" && DeliveryVehicleQtrPaid(p_sBIN,p_sTaxYear,p_sFeeCode,p_sBnsCode) == true)) // GARBAGE/Delivery Van temporary hard coded -- revise code in the future		// JGR 12082006 IF FEES IS FOR DELIVERY VEHICLE AND HAS QTRLY DELINQUENTS, DISPLAY IN SOA
            {
                if (p_sFeeCode.Left(1) == 'B')
                    p_sFeeCode = 'B';

                sQuery = "select a.or_no, a.qtr_paid, a.payment_term from pay_hist a, or_table b";
                sQuery+= " where bin = '"+p_sBIN+"'";
                sQuery+= " and a.or_no = b.or_no";
                sQuery+= " and a.tax_year = b.tax_year";
                sQuery+= " and a.tax_year = '"+p_sTaxYear+"'";
                sQuery+= " and payment_term = 'I'";
                sQuery+= " and fees_code = '"+p_sFeeCode+"'";
                sQuery+= " and bns_code_main = '"+p_sBnsCode+"'";

                if(pApp->RecordFound(sQuery))
                    return TRUE;
                else
                    return FALSE;
            }
            else
            */
            return false;
        }

        public bool IsTaxFull(string p_sBnsCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;

            // ALJ 12232005 chk if paid qtrly -- (due to change of fee/tax configuration from I to F)
            sQuery = "select * from btax_full_table where bns_code like '" + p_sBnsCode.Substring(0, 2) + "%%'";
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    return true;
                }
                pSet.Close();
            }

            return false;
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
                    sFeesTerm = "";		// ALJ 12232005 chk if paid qtrly -- (due to change of fee/tax configuration from I to F)
                }
                else
                {
                    if (m_bPartial == false)
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
        //(e) 02282014 MCR

        private void TablesTransferPUT()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            string sApplType = string.Empty;
            string sNewBnsCode = string.Empty;
            string sTrBIN = string.Empty;
            string sTrAppCode = string.Empty;
            string sTrBnsCode = string.Empty;
            string sTrPrevOwnCode = string.Empty;
            string sTrNewOwnCode = string.Empty;
            string sTrOwnLn = string.Empty;
            string sTrOwnFn = string.Empty;
            string sTrOwnMi = string.Empty;
            string sTrAddrNo = string.Empty;
            string sTrAddrStreet = string.Empty;
            string sTrAddrBrgy = string.Empty;
            string sTrAddrZone = string.Empty;
            string sTrAddrDist = string.Empty;
            string sTrAddrMun = string.Empty;
            string sTrAddrProv = string.Empty;
            string sTrAppDate = string.Empty;
            string sQuery = string.Empty;

            result.Query = "select * from permit_update_appl where bin = '" + m_sBIN.Trim() + "' and data_mode = 'QUE'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sApplType = result.GetString("appl_type").Trim();
                    sNewBnsCode = result.GetString("new_bns_code").Trim();

                    if (sApplType == "ADDL")
                    {
                        result2.Query = "insert into addl_bns select * from addl_bns_que where bin = '" + m_sBIN.Trim() + "' and bns_code_main = '" + sNewBnsCode + "'";
                        if (result2.ExecuteNonQuery() != 0)
                        {

                        }
                        result2.Close();

                        result2.Query = "delete from addl_bns_que where bin = '" + m_sBIN + "' and bns_code_main = '" + sNewBnsCode + "'";
                        if (result2.ExecuteNonQuery() != 0)
                        {

                        }
                        result2.Close();
                    }
                    if (sApplType == "CTYP")
                    {
                        SaveChangeClass(sNewBnsCode);
                    }
                    if (sApplType == "TLOC")
                    {
                        result2.Query = "select * from transfer_table where bin = '" + m_sBIN.Trim() + "' and trans_app_code = 'TL'";
                        if (result2.Execute())
                        {
                            if (result2.Read())
                            {
                                sTrBIN = result2.GetString("bin").Trim();
                                sTrAppCode = result2.GetString("trans_app_code").Trim(); // GDE 20110414 dito ako natapos
                                sTrBnsCode = result2.GetString("bns_code").Trim();
                                sTrPrevOwnCode = result2.GetString("prev_own_code").Trim();
                                sTrNewOwnCode = result2.GetString("new_own_code").Trim();
                                sTrOwnLn = result2.GetString("own_ln").Trim();
                                sTrOwnFn = result2.GetString("own_fn").Trim();
                                sTrOwnMi = result2.GetString("own_mi").Trim();
                                sTrAddrNo = result2.GetString("addr_no").Trim();
                                sTrAddrStreet = result2.GetString("addr_street").Trim();
                                sTrAddrBrgy = result2.GetString("addr_brgy").Trim();
                                sTrAddrZone = result2.GetString("addr_zone").Trim();
                                sTrAddrDist = result2.GetString("addr_dist").Trim();
                                sTrAddrMun = result2.GetString("addr_mun").Trim();
                                sTrAddrProv = result2.GetString("addr_prov").Trim();
                                sTrAppDate = result2.GetString("app_date").Trim();

                                sQuery = "insert into transfer_hist(bin,trans_app_code,bns_code,prev_own_code,new_own_code,own_ln,Own_fn,own_mi,addr_no,addr_street,addr_brgy,addr_zone,addr_dist,addr_mun,addr_prov,app_date,or_no) values";
                                sQuery += "('" + sTrBIN + "',";
                                sQuery += "'" + sTrAppCode + "',";
                                sQuery += "'" + sTrBnsCode + "',";
                                sQuery += "'" + sTrPrevOwnCode + "',";
                                sQuery += "'" + sTrNewOwnCode + "',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sTrOwnLn) + "',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sTrOwnFn) + "',";
                                sQuery += "'" + sTrOwnMi + "',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sTrAddrNo) + "',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sTrAddrStreet) + "',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sTrAddrBrgy) + "',";
                                sQuery += "'" + sTrAddrZone + "',";
                                sQuery += "'" + sTrAddrDist + "',";
                                sQuery += "'" + sTrAddrMun + "',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sTrAddrProv) + "',";
                                sQuery += "'" + sTrAppDate + "',";
                                sQuery += "'" + txtORNo.Text.Trim() + "')";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                sQuery = "delete from transfer_table where bin = '" + m_sBIN + "'";
                                sQuery += " and trans_app_code = 'TL'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                String sLastOwnCode, sLastBusnOwn, sLastPrevBnsOwn, sLastAddrNo, sLastAddrStreet, sLastAddrBrgy, sLastAddrZone, sLastAddrDist, sLastAddrMun, sLastAddrProv;
                                sQuery = "select * from businesses";
                                sQuery += " where bin = '" + m_sBIN + "'";
                                result3.Query = sQuery;
                                if (result3.Execute())
                                    if (result3.Read())
                                    {
                                        sLastOwnCode = result3.GetString("own_code").Trim();
                                        sLastBusnOwn = result3.GetString("busn_own").Trim();
                                        sLastPrevBnsOwn = result3.GetString("prev_bns_own").Trim();
                                        sLastAddrNo = result3.GetString("bns_house_no").Trim();
                                        sLastAddrStreet = result3.GetString("bns_street").Trim();
                                        sLastAddrBrgy = result3.GetString("bns_brgy").Trim();
                                        sLastAddrZone = result3.GetString("bns_zone").Trim();
                                        sLastAddrDist = result3.GetString("bns_dist").Trim();
                                        sLastAddrMun = result3.GetString("bns_mun").Trim();
                                        sLastAddrProv = result3.GetString("bns_prov").Trim();

                                        sQuery = "insert into bns_loc_last values";
                                        sQuery += " ('" + m_sBIN + "',";
                                        sQuery += "'" + txtORNo.Text.Trim() + "',";
                                        sQuery += "'TL',";
                                        sQuery += "'" + sLastOwnCode + "',";
                                        sQuery += "'" + sLastBusnOwn + "',";
                                        sQuery += "'" + sLastPrevBnsOwn + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastAddrNo) + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastAddrStreet) + "',";
                                        sQuery += "'" + sLastAddrBrgy + "',";
                                        sQuery += "'" + sLastAddrZone + "',";
                                        sQuery += "'" + sLastAddrDist + "',";
                                        sQuery += "'" + sLastAddrMun + "',";
                                        sQuery += "'" + sLastAddrProv + "')";
                                        result3.Query = sQuery;
                                        result3.ExecuteNonQuery();
                                    }
                                result3.Close();

                                sQuery = "update businesses set";
                                sQuery += " bns_house_no = '" + StringUtilities.HandleApostrophe(StringUtilities.Left(sTrAddrNo, 30)) + "',";
                                sQuery += " bns_street = '" + StringUtilities.HandleApostrophe(StringUtilities.Left(sTrAddrStreet, 40)) + "',";
                                sQuery += " bns_brgy = '" + sTrAddrBrgy + "',";
                                sQuery += " bns_zone = '" + sTrAddrZone + "',";
                                sQuery += " bns_dist = '" + sTrAddrDist + "',";
                                sQuery += " bns_mun = '" + sTrAddrMun + "',";
                                sQuery += " bns_prov = '" + sTrAddrProv + "'";
                                sQuery += " where bin = '" + m_sBIN + "'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();
                            }
                        }
                        result2.Close();
                    }

                    if (sApplType == "TOWN")
                    {
                        sQuery = "select * from transfer_table where bin = '" + m_sBIN + "'";
                        sQuery += " and trans_app_code = 'TO'";
                        result2.Query = sQuery;
                        if (result2.Execute())
                            if (result2.Read())
                            {
                                sTrBIN = result2.GetString("bin").Trim();
                                sTrAppCode = result2.GetString("trans_app_code").Trim();
                                sTrBnsCode = result2.GetString("bns_code").Trim();
                                sTrPrevOwnCode = result2.GetString("prev_own_code").Trim();
                                sTrNewOwnCode = result2.GetString("new_own_code").Trim();
                                sTrOwnLn = result2.GetString("own_ln").Trim();
                                sTrOwnFn = result2.GetString("own_fn").Trim();
                                sTrOwnMi = result2.GetString("own_mi").Trim();
                                sTrAddrNo = result2.GetString("addr_no").Trim();
                                sTrAddrStreet = result2.GetString("addr_street").Trim();
                                sTrAddrBrgy = result2.GetString("addr_brgy").Trim();
                                sTrAddrZone = result2.GetString("addr_zone").Trim();
                                sTrAddrDist = result2.GetString("addr_dist").Trim();
                                sTrAddrMun = result2.GetString("addr_mun").Trim();
                                sTrAddrProv = result2.GetString("addr_prov").Trim();
                                sTrAppDate = result2.GetString("app_date").Trim();

                                String sTaxYear = "", sDtSave, sQtrPaid = "";
                                sQuery = "select * from permit_update_appl where bin = '" + sTrBIN + "' and appl_type = '" + sApplType + "'";
                                result3.Query = sQuery;
                                if (result3.Execute())
                                    if (result3.Read())
                                    {
                                        sTaxYear = result3.GetString("tax_year");
                                        sDtSave = result3.GetDateTime("dt_save").ToString("MM/dd/yyyy");
                                    }
                                result3.Close();

                                sQuery = "select * from pay_hist where bin = '" + sTrBIN + "' order by date_posted desc";
                                result3.Query = sQuery;
                                if (result3.Execute())
                                    if (result3.Read())
                                    {
                                        sQtrPaid = result3.GetString("qtr_paid");
                                    }
                                result3.Close();

                                sQuery = "insert into transfer_hist(bin,trans_app_code,bns_code,prev_own_code,new_own_code,own_ln,Own_fn,own_mi,addr_no,addr_street,addr_brgy,addr_zone,addr_dist,addr_mun,addr_prov,app_date,trans_date,tax_year,qtr_paid,or_no) values";
                                sQuery += "('" + sTrBIN + "',";
                                sQuery += "'" + sTrAppCode + "',";
                                sQuery += "'" + sTrBnsCode + "',";
                                sQuery += "'" + sTrPrevOwnCode + "',";
                                sQuery += "'" + sTrNewOwnCode + "',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sTrOwnLn) + "',";
                                sQuery += "'" + sTrOwnFn + "',";
                                sQuery += "'" + sTrOwnMi + "',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sTrAddrNo) + "',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sTrAddrStreet) + "',";
                                sQuery += "'" + sTrAddrBrgy + "',";
                                sQuery += "'" + sTrAddrZone + "',";
                                sQuery += "'" + sTrAddrDist + "',";
                                sQuery += "'" + sTrAddrMun + "',";
                                sQuery += "'" + sTrAddrProv + "',";
                                sQuery += "'" + sTrAppDate + "',";
                                sQuery += "'" + sTrAppDate + "',";
                                sQuery += "'" + sTaxYear + "',";
                                sQuery += "'" + sQtrPaid + "',";
                                sQuery += "'" + txtORNo.Text.Trim() + "')";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                sQuery = "delete from transfer_table where bin = '" + m_sBIN + "'";
                                sQuery += " and trans_app_code = 'TO'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                String sLastOwnCode, sLastBusnOwn, sLastPrevBnsOwn, sLastAddrNo, sLastAddrStreet, sLastAddrBrgy, sLastAddrZone, sLastAddrDist, sLastAddrMun, sLastAddrProv;
                                sQuery = "select * from businesses where bin = '" + m_sBIN + "'";
                                result3.Query = sQuery;
                                if (result3.Execute())
                                    if (result3.Read())
                                    {
                                        sLastOwnCode = result3.GetString("own_code").Trim();
                                        sLastBusnOwn = result3.GetString("busn_own").Trim();
                                        sLastPrevBnsOwn = result3.GetString("prev_bns_own").Trim();
                                        sLastAddrNo = result3.GetString("bns_house_no").Trim();
                                        sLastAddrStreet = result3.GetString("bns_street").Trim();
                                        sLastAddrBrgy = result3.GetString("bns_brgy").Trim();
                                        sLastAddrZone = result3.GetString("bns_zone").Trim();
                                        sLastAddrDist = result3.GetString("bns_dist").Trim();
                                        sLastAddrMun = result3.GetString("bns_mun").Trim();
                                        sLastAddrProv = result3.GetString("bns_prov").Trim();

                                        sQuery = "insert into bns_loc_last values";
                                        sQuery += " ('" + m_sBIN + "',";
                                        sQuery += "'" + txtORNo.Text + "',";
                                        sQuery += "'TO',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastOwnCode) + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastBusnOwn) + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastPrevBnsOwn) + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastAddrNo) + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastAddrStreet) + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastAddrBrgy) + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastAddrZone) + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastAddrDist) + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastAddrMun) + "',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastAddrProv) + "')";
                                        result3.Query = sQuery;
                                        result3.ExecuteNonQuery();
                                    }
                                result3.Close();

                                GetTransTableValues(m_sBIN);    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership

                                if (m_sPlaceOccupancy == "OWNED")
                                {
                                    sQuery = "update businesses set";
                                    sQuery += " own_code = '" + sTrNewOwnCode + "',";
                                    sQuery += " busn_own = '" + sTrNewOwnCode + "',";
                                    sQuery += " prev_bns_own = '" + sTrPrevOwnCode + "'";
                                    //sQuery += " where bin = '" + m_sBIN + "'";    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership, transferred
                                }
                                else
                                {
                                    sQuery = "update businesses set";
                                    sQuery += " own_code = '" + sTrNewOwnCode + "',";
                                    sQuery += " prev_bns_own = '" + sTrPrevOwnCode + "'";
                                    //sQuery += " where bin = '" + m_sBIN + "'";    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership, transferred
                                }

                                // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (s)
                                if (m_sTrDTINo.Trim() != "")
                                    sQuery += string.Format(", dti_reg_no = '{0}', dti_reg_dt = to_date('{1}','MM/dd/yyyy')", m_sTrDTINo, m_dTrDTIDt.ToShortDateString());

                                if (m_sTrMemo.Trim() != "")
                                    sQuery += string.Format(", memoranda = '{0}'", m_sTrMemo);
                                sQuery += " where bin = '" + m_sBIN + "'";
                                // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (e)

                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();
                            }
                        result2.Close();
                    }
                    if (sApplType == "CBNS")
                    {
                        String sNewBnsName;
                        sQuery = "select * from transfer_table where bin = '" + m_sBIN + "'";
                        sQuery += " and trans_app_code = 'CB'";
                        result2.Query = sQuery;
                        if (result2.Execute())
                            if (result2.Read())
                            {
                                sTrBIN = result2.GetString("bin").Trim();
                                sTrAppCode = result2.GetString("trans_app_code").Trim();
                                sTrOwnLn = result2.GetString("own_ln").Trim();
                                sTrAppDate = result2.GetString("app_date").Trim();

                                sQuery = "insert into transfer_hist(bin,trans_app_code,bns_code,prev_own_code,new_own_code,own_ln,Own_fn,own_mi,addr_no,addr_street,addr_brgy,addr_zone,addr_dist,addr_mun,addr_prov,app_date,trans_date,or_no) values";
                                sQuery += "('" + sTrBIN + "',";
                                sQuery += "'" + sTrAppCode + "',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sTrOwnLn) + "',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "'" + sTrAppDate + "',";
                                sQuery += "to_date('" + dtpORDate.Text + "','MM/dd/yyyy'),";
                                sQuery += "'" + txtORNo.Text.Trim() + "')";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                sQuery = "delete from transfer_table where bin = '" + m_sBIN + "'";
                                sQuery += " and trans_app_code = 'CB'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                String sLastBnsName;
                                sQuery = "select * from businesses where bin = '" + m_sBIN + "'";
                                result3.Query = sQuery;
                                if (result3.Execute())
                                    if (result3.Read())
                                    {
                                        sLastBnsName = result3.GetString("bns_nm");

                                        sQuery = "insert into bns_loc_last values";
                                        sQuery += " ('" + m_sBIN + "',";
                                        sQuery += "'" + txtORNo.Text.Trim() + "',";
                                        sQuery += "'CB',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastBnsName) + "',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "' ')";
                                        result3.Query = sQuery;
                                        result3.ExecuteNonQuery();
                                    }
                                result3.Close();

                                GetTransTableValues(m_sBIN);    // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership

                                sQuery = "update businesses set bns_nm = '" + StringUtilities.HandleApostrophe(txtBnsName.Text.Trim()) + "'";
                                // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (s)
                                if (m_sTrDTINo.Trim() != "")
                                    sQuery += string.Format(", dti_reg_no = '{0}', dti_reg_dt = to_date('{1}','MM/dd/yyyy')", m_sTrDTINo, m_dTrDTIDt.ToShortDateString());

                                if (m_sTrMemo.Trim() != "")
                                    sQuery += string.Format(", memoranda = '{0}'", m_sTrMemo);
                                // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership (e)

                                sQuery += " where bin = '" + m_sBIN + "'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();
                            }
                        result2.Close();
                    }

                    if (sApplType == "CORG")
                    {
                        String sNewOrgnKind;
                        sQuery = "select * from transfer_table where bin = '" + m_sBIN + "'";
                        sQuery += " and trans_app_code = 'CO'";
                        result2.Query = sQuery;
                        if (result2.Execute())
                            if (result2.Read())
                            {
                                sTrBIN = result2.GetString("bin");
                                sNewOrgnKind = result2.GetString("own_ln");
                                sTrAppCode = result2.GetString("trans_app_code");
                                sTrAppDate = result2.GetString("app_date");

                                sQuery = "insert into transfer_hist(bin,trans_app_code,bns_code,prev_own_code,new_own_code,own_ln,Own_fn,own_mi,addr_no,addr_street,addr_brgy,addr_zone,addr_dist,addr_mun,addr_prov,app_date,trans_date,or_no) values";
                                sQuery += "('" + sTrBIN + "',";
                                sQuery += "'" + sTrAppCode + "',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "'" + StringUtilities.HandleApostrophe(sNewOrgnKind) + "',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "' ',";
                                sQuery += "'" + sTrAppDate + "',";
                                sQuery += "to_date('" + dtpORDate.Text + "','MM/dd/yyyy'),";
                                sQuery += "'" + txtORNo.Text.Trim() + "')";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                sQuery = "delete from transfer_table where bin = '" + m_sBIN + "'";
                                sQuery += " and trans_app_code = 'CO'";
                                result2.Query = sQuery;
                                result2.ExecuteNonQuery();

                                String sLastOrgnKind;
                                sQuery = "select * from businesses where bin = '" + m_sBIN + "'";
                                result3.Query = sQuery;
                                if (result3.Execute())
                                    if (result3.Read())
                                    {
                                        sLastOrgnKind = result3.GetString("orgn_kind");

                                        sQuery = "insert into bns_loc_last values";
                                        sQuery += " ('" + m_sBIN + "',";
                                        sQuery += "'" + txtORNo.Text.Trim() + "',";
                                        sQuery += "'CO',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "'" + StringUtilities.HandleApostrophe(sLastOrgnKind) + "',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "' ',";
                                        sQuery += "' ')";
                                        result3.Query = sQuery;
                                        result3.ExecuteNonQuery();
                                    }
                                result3.Close();

                                sQuery = "update businesses set";
                                sQuery += " orgn_kind = '" + StringUtilities.HandleApostrophe(sNewOrgnKind) + "'";
                                sQuery += " where bin = '" + m_sBIN + "'";
                                result3.Query = sQuery;
                                result3.ExecuteNonQuery();
                            }
                        result2.Close();
                    }

                    sQuery = "update permit_update_appl set data_mode = 'PAID', or_no = '" + txtORNo.Text.Trim() + "'";
                    sQuery += " where bin = '" + m_sBIN + "' and data_mode = 'QUE'";
                    result.Query = sQuery;
                    result.ExecuteNonQuery();
                }
            }
            result.Close();
        }

        //MCR 20140721 (s)
        private void btnChequeInfo_Click(object sender, EventArgs e)
        {
            String sLN, sFN, sMI;
            ArrayList arr_Name = new ArrayList();

            arr_Name = GetOwnNames2(m_sOwnCode);
            sLN = arr_Name[0].ToString();
            sFN = arr_Name[1].ToString();
            sMI = arr_Name[2].ToString();

            frmPaymentMultiCheck frmPaymentMultiCheck = new frmPaymentMultiCheck();
            m_sDebitCredit = "0.00";
            m_sCCChkAmt = "0.00";   // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination
            m_sCCCashAmt = "0.00";  // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination
            if ((chkCheque.Checked) && m_sOwnCode != "")
            {
                //if (chkCash.Check == true && chkTCredit.Check == true)
                //    UpdateData(1);

                frmPaymentMultiCheck.OwnCode = m_sOwnCode;
                frmPaymentMultiCheck.ORNo = txtORNo.Text;
                frmPaymentMultiCheck.BIN = m_sBIN;
                frmPaymentMultiCheck.AcctLN = sLN;
                frmPaymentMultiCheck.AcctFN = sFN;
                frmPaymentMultiCheck.AcctMI = sMI;
                frmPaymentMultiCheck.ORDate = dtpORDate.Value.ToString("MM/dd/yyyy");

                frmPaymentMultiCheck.TotalTaxDue = txtGrandTotal.Text;
                frmPaymentMultiCheck.PaymentType = m_sPaymentType;

                // RMC 20170210 corrected error in additional payment when using multiple check transaction (s)
                if (chkCash.Checked == false)
                    frmPaymentMultiCheck.txtTotCashAmount.Text = "0";
                // RMC 20170210 corrected error in additional payment when using multiple check transaction (e)

                frmPaymentMultiCheck.ShowDialog();
                m_sPaymentType = frmPaymentMultiCheck.PaymentType;

                if (m_sPaymentType == "CC")
                {
                    chkCash.Checked = true;
                }

                m_sDebitCredit = frmPaymentMultiCheck.TotalDBCR;
                m_sCCChkAmt = frmPaymentMultiCheck.TotalChkAmount;
                m_sCCCashAmt = frmPaymentMultiCheck.TotalCashAmount;

                // RMC 20161209 correction in payment if check is less than the total due (s)
                if (chkTCredit.Checked == false && (Convert.ToDouble(m_sCCCashAmt) == 0 && Convert.ToDouble(m_sDebitCredit) != 0))
                {
                    if (Convert.ToDouble(frmPaymentMultiCheck.TotalTaxDue) < Convert.ToDouble(m_sCCChkAmt))
                    {   
                        // RMC 20170113 correction in credit memo creation from online payment
                    }
                    else
                    {
                        m_sCCCashAmt = m_sDebitCredit;
                        m_sDebitCredit = "0";
                        m_sPaymentType = "CC";
                        chkCash.Checked = true;
                    }
                }
                // RMC 20161209 correction in payment if check is less than the total due (e)

                m_sFlag = frmPaymentMultiCheck.FlagAddlCheck;
                m_bAttachedExcessAmount = frmPaymentMultiCheck.chkAttached.Checked;

                if (m_bAttachedExcessAmount == true)
                {
                    txtCreditLeft.Text = Convert.ToDouble(Convert.ToDouble(txtCreditLeft.Text) + Convert.ToDouble(frmPaymentMultiCheck.TotalRemBalance)).ToString("#,#00.00");
                }

                m_sCheckOwnerCode = "";
                m_sCheckOwnerCode = frmPaymentMultiCheck.OwnCode;
                bGuarantor = frmPaymentMultiCheck.bMultiPayFound;
                bRefCheckNo = frmPaymentMultiCheck.bRefCheck;

                if (m_sPaymentType != frmPaymentMultiCheck.PaymentType && bGuarantor == true && bRefCheckNo == true)
                {
                    if (frmPaymentMultiCheck.PaymentType == "CC")
                    {
                        chkCash.Enabled = true;
                        chkCash.Checked = true;
                    }
                    m_sPaymentType = frmPaymentMultiCheck.PaymentType;
                    btnChequeInfo.Visible = false;
                }

                if (bRefCheckNo == true)
                    m_sToBeDebited = frmPaymentMultiCheck.TotalTaxDue;

                if (chkCash.Checked == true && chkTCredit.Checked == true)
                {
                    if (Convert.ToDouble(frmPaymentMultiCheck.TotalCashAmount) > 0.00)
                        txtTotDues.Text = frmPaymentMultiCheck.TotalCashAmount;
                }
            }
        }

        private ArrayList GetOwnNames2(String sOwnCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            ArrayList arr_Name = new ArrayList();

            sQuery = string.Format("select * from own_names where own_code = '{0}'", sOwnCode); // CTS 09152003
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    arr_Name.Add(pSet.GetString("own_ln"));
                    arr_Name.Add(pSet.GetString("own_fn"));
                    arr_Name.Add(pSet.GetString("own_mi"));
                }
            pSet.Close();
            return arr_Name;
        }

        private void OnRetirement()
        {
            m_bExistTaxPU = false;
            if (chkRetirement.Checked)
            {
                if (m_sFullPartial == "F")
                    txtBnsStat.Text = "RET";
                m_sNextQtrToPay = "1";

                if (m_sFullPartial == "F")
                {
                    dgvTaxFees.Rows.Clear();

                    // RMC 20170228 correction in SOA of RET (s)
                    if (chkInstallment.Checked)
                    {
                        if(chk1st.Checked)
                            DisplayDuesNew(m_sBIN, "1", "", "", "");
                        if(chk2nd.Checked)
                            DisplayDuesNew(m_sBIN, "", "2", "", "");
                        if(chk3rd.Checked)
                            DisplayDuesNew(m_sBIN, "", "", "3", "");
                        if(chk4th.Checked)
                            DisplayDuesNew(m_sBIN, "", "", "", "4");
                    }// RMC 20170228 correction in SOA of RET (e)
                    else
                    {
                        if (m_sNextQtrToPay == "1")
                            DisplayDuesNew(m_sBIN, "1", "2", "3", "4");
                        else if (m_sNextQtrToPay == "2")
                            DisplayDuesNew(m_sBIN, "", "2", "3", "4");
                        else if (m_sNextQtrToPay == "3")
                            DisplayDuesNew(m_sBIN, "", "", "3", "4");
                        else if (m_sNextQtrToPay == "4")
                            DisplayDuesNew(m_sBIN, "", "", "", "4");
                    }

                    /*SetCheckTerm(chkFull);    
                    chk1st.Checked = false;
                    chk2nd.Checked = false;
                    chk3rd.Checked = false;
                    chk4th.Checked = false;
                    chkFull.Enabled = false;
                    chkInstallment.Enabled = false;
                    chk1st.Enabled = false;
                    chk2nd.Enabled = false;
                    chk3rd.Enabled = false;
                    chk4th.Enabled = false;*/
                    // RMC 20170228 correction in SOA of RET, put rem
                }
                else
                {
                    String sQuery, sBnsCodeMain;
                    OracleResultSet pSet = new OracleResultSet();

                    dgvTaxFees.Rows.Clear(); //MCR 20150120 remove duplicate
                    sQuery = string.Format("select bns_code_main from retired_bns_temp where bin = '{0}' and main = 'Y' and qtr = '{1}'", m_sBIN, m_sQtr);
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sBnsCodeMain = pSet.GetString("bns_code_main");

                            if (chk1st.Checked && !chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "2", "3", "4");
                            else if (!chk1st.Checked && chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "3", "4");
                            else if (chk1st.Checked && chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "3", "4");
                            else if (!chk1st.Checked && chk2nd.Checked && chk3rd.Checked && !chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "", "4");
                            else if (!chk1st.Checked && !chk2nd.Checked && chk3rd.Checked && !chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "", "4");
                            else if (!chk1st.Checked && chk2nd.Checked && chk3rd.Checked && chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "", "");
                            if (!chk1st.Checked && !chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
                            {
                                if (m_sNextQtrToPay == "1")
                                    DisplayDuesBns(m_sBIN, sBnsCodeMain, "1", "2", "3", "4");
                                else if (m_sNextQtrToPay == "2")
                                    DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "2", "3", "4");
                                else if (m_sNextQtrToPay == "3")
                                    DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "3", "4");
                                else if (m_sNextQtrToPay == "4")
                                    DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "", "4");
                            }
                        }
                    pSet.Close();

                    sQuery = string.Format("select bns_code_main from addl_bns_que where bin = '{0}' and bns_stat = 'RET'", m_sBIN);
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                    {
                        while (pSet.Read())
                        {
                            sBnsCodeMain = pSet.GetString("bns_code_main");

                            if (chk1st.Checked && !chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "2", "3", "4");
                            else if (!chk1st.Checked && chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "3", "4");
                            else if (chk1st.Checked && chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "3", "4");
                            else if (!chk1st.Checked && chk2nd.Checked && chk3rd.Checked && !chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "", "4");
                            else if (!chk1st.Checked && !chk2nd.Checked && chk3rd.Checked && !chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "", "4");
                            else if (!chk1st.Checked && chk2nd.Checked && chk3rd.Checked && chk4th.Checked)
                                DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "", "");
                            if (!chk1st.Checked && !chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
                            {
                                if (m_sNextQtrToPay == "1")
                                    DisplayDuesBns(m_sBIN, sBnsCodeMain, "1", "2", "3", "4");
                                else if (m_sNextQtrToPay == "2")
                                    DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "2", "3", "4");
                                else if (m_sNextQtrToPay == "3")
                                    DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "3", "4");
                                else if (m_sNextQtrToPay == "4")
                                    DisplayDuesBns(m_sBIN, sBnsCodeMain, "", "", "", "4");
                            }
                        }
                    }
                    pSet.Close();
                }

                DisplayDuesNew(m_sBIN, "X", "", "", "");
            }
            else
            {
                OracleResultSet pSet = new OracleResultSet();
                String sQuery;

                sQuery = string.Format("select bns_stat from business_que where bin = '{0}' and bns_stat <> 'RET'", m_sBIN);
                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                        txtBnsStat.Text = pSet.GetString("bns_stat");
                    else
                    {
                        pSet.Close();
                        sQuery = string.Format("select bns_stat from businesses where bin = '{0}'", m_sBIN);
                        pSet.Query = sQuery;
                        if (pSet.Execute())
                            if (pSet.Read())
                                txtBnsStat.Text = pSet.GetString("bns_stat");
                    }
                pSet.Close();

                dgvTaxFees.Rows.Clear();

                if (chk4th.Checked == true)
                    On4thQtr();
                else if (chk3rd.Checked == true)
                    On3rdQtr();
                else if (chk2nd.Checked == true)
                    On2ndQtr();
                else if (chk1st.Checked == true)
                    On1stQtr();
                else
                    OnFull();
            }
            if (chkAdj.Checked == true)
                OnCheckAdj();
        }

        private void OnCheckAdjLate()
        {
            String sQuery;
            OracleResultSet pSet = new OracleResultSet();

            bool bOldValueTC;
            bOldValueTC = chkTCredit.Checked;

            dgvTaxFees.Rows.Clear();

            sQuery = string.Format("delete from pay_temp where bin = '{0}'", m_sBIN);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();

            sQuery = string.Format("delete from pay_temp_bns where bin = '{0}'", m_sBIN);
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();

            ComputeTaxAndFees(m_sBIN);

            chkTCredit.Checked = bOldValueTC;

            if (chkCharges.Checked)
            {
                chkAdj.Checked = true;
                dgvTaxFees.Rows.Clear();
                OnCheckAdj(); //MCR 20150116
            }
            else
                chkAdj.Checked = false;

            if (m_sPaymentMode == "OFL")
            {
                //OFLPaymentCondition(); MCR 20140902 REM This Code Doesn't Exist on Antipolo Version
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

        struct PayTempStruct
        {
            public string bin;
            public string tax_year;
            public string term;
            public string qtr;
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

        private void DisplayDuesBns(String sBIN, String sBnsCode, String sQtrToPay, String sQtrToPay2, String sQtrToPay3, String sQtrToPay4)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery = "";

            PayTempStruct p;

            if (sQtrToPay2 == "" && sQtrToPay3 == "" && sQtrToPay4 == "")
                sQuery = string.Format("select * from  pay_temp_bns where bin = '{0}' and qtr_to_pay = '{1}' and bns_code_main = '{2}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBIN, sQtrToPay, sBnsCode);
            else
            {
                if (sQtrToPay2 != "" && sQtrToPay3 == "" && sQtrToPay4 == "")
                    sQuery = string.Format("select * from  pay_temp_bns where bin = '{0}' and qtr_to_pay in ('{1}','{2}') and due_state <> 'X' and bns_code_main = '{3}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBIN, sQtrToPay, sQtrToPay2, sBnsCode);
                if (sQtrToPay2 != "" && sQtrToPay3 != "" && sQtrToPay4 == "")
                    sQuery = string.Format("select * from  pay_temp_bns where bin = '{0}' and qtr_to_pay in ('{1}','{1}','{2}') and due_state <> 'X' and bns_code_main = '{3}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBIN, sQtrToPay, sQtrToPay2, sQtrToPay3, sBnsCode);
                if (sQtrToPay2 != "" && sQtrToPay3 != "" && sQtrToPay4 != "")
                    sQuery = string.Format("select * from  pay_temp_bns where bin = '{0}' and qtr_to_pay in ('{1}','{1}','{2}','{3}') and due_state <> 'X' and bns_code_main = '{4}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBIN, sQtrToPay, sQtrToPay2, sQtrToPay3, sQtrToPay4, sBnsCode);
                if (sQtrToPay2 == "" && sQtrToPay3 != "" && sQtrToPay4 == "")
                    sQuery = string.Format("select * from  pay_temp_bns where bin = '{0}' and qtr_to_pay in ('{1}') and due_state <> 'X' and bns_code_main = '{2}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBIN, sQtrToPay3, sBnsCode);
                if (sQtrToPay2 == "" && sQtrToPay3 != "" && sQtrToPay4 != "")
                    sQuery = string.Format("select * from  pay_temp_bns where bin = '{0}' and qtr_to_pay in ('{1}','{2}') and due_state <> 'X' and bns_code_main = '{3}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBIN, sQtrToPay3, sQtrToPay4, sBnsCode);
                if (sQtrToPay2 == "" && sQtrToPay3 == "" && sQtrToPay4 != "")
                    sQuery = string.Format("select * from  pay_temp_bns where bin = '{0}' and trim(qtr_to_pay) like '{1}' and due_state <> 'X' and bns_code_main = '{2}' order by tax_year,qtr_to_pay,qtr,fees_code_sort", sBIN, sQtrToPay4, sBnsCode);
            }

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    p.bin = pSet.GetString("bin");
                    p.tax_year = pSet.GetString("tax_year");
                    p.term = pSet.GetString("term");
                    p.qtr = pSet.GetString("qtr");
                    p.fees_desc = pSet.GetString("fees_desc");
                    p.fees_due = pSet.GetDouble("fees_due").ToString("##0.00");
                    p.fees_surch = pSet.GetDouble("fees_surch").ToString("##0.00");
                    p.fees_pen = pSet.GetDouble("fees_pen").ToString("##0.00");
                    p.fees_totaldue = pSet.GetDouble("fees_totaldue").ToString("##0.00");
                    p.fees_code = pSet.GetString("fees_code");
                    p.due_state = pSet.GetString("due_state");
                    p.qtr_to_pay = pSet.GetString("qtr_to_pay");

                    double.TryParse(p_fees_due.ToString(), out dDue);
                    p_fees_due = string.Format("{0:0.00}", (double)dDue);
                    double.TryParse(p_fees_surch.ToString(), out dSurch);
                    p_fees_surch = string.Format("{0:0.00}", (double)dSurch);
                    double.TryParse(p_fees_pen.ToString(), out dPen);
                    p_fees_pen = string.Format("{0:0.00}", (double)dPen);
                    double.TryParse(p_fees_totaldue.ToString(), out dTot);
                    p_fees_totaldue = string.Format("{0:0.00}", (double)dTot);

                    dgvTaxFees.Rows.Add(p.tax_year, p.term, p.qtr, p.fees_desc, p.fees_due, p.fees_surch, p.fees_pen, p.fees_totaldue, p.due_state, p.qtr_to_pay, p.fees_code);
                }
            }
            pSet.Close();

            ComputeTotal();
            SetControls(true);
        }

        private void SetControls(bool nFlag)
        {
            chkFull.Enabled = nFlag;
            chkInstallment.Enabled = nFlag;
            chkCash.Enabled = nFlag;
            chkCheque.Enabled = nFlag;
            btnChequeInfo.Enabled = nFlag;
            dgvTaxFees.Enabled = nFlag;
            btnAccept.Enabled = nFlag;
            txtCompromise.Enabled = nFlag;

            if (m_sPaymentMode == "SOA")
            {
                chkCash.Enabled = false;
                chkCheque.Enabled = false;
                btnChequeInfo.Visible = false;
                txtCompromise.Enabled = false;
            }
        }

        private void On1stQtr()
        {
            if (chk1st.Checked)
            {
                chk2nd.Checked = false;
                chk3rd.Checked = false;
                chk4th.Checked = false;
                SetCheckTerm(chkInstallment);
            }
            else
            {
                if (chkAdj.Checked || chkRetirement.Checked)
                {
                    chk1st.Checked = false;
                    SetCheckTerm(chkFull);
                }
                else
                    chk1st.Checked = true;
            }

            String sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4;
            if (chk1st.Checked)
                sQToBePaid1 = "1";
            else
                sQToBePaid1 = "";
            if (chk2nd.Checked)
                sQToBePaid2 = "2";
            else
                sQToBePaid2 = "";
            if (chk3rd.Checked)
                sQToBePaid3 = "3";
            else
                sQToBePaid3 = "";
            if (chk4th.Checked)
                sQToBePaid4 = "4";
            else
                sQToBePaid4 = "";

            String sTmpTaxCredit;
            sTmpTaxCredit = txtToTaxCredit.Text;
            dgvTaxFees.Rows.Clear();
            txtToTaxCredit.Text = sTmpTaxCredit;

            m_bExistTaxPU = false;

            /*
            // RMC 20150112 mods in retirement billing (s)
            if(chkAdj.Checked == true)
                DisplayDuesNew(m_sBIN, "A", "", "", "");
            // RMC 20150112 mods in retirement billing (e)
             */
            // RMC 20160719 correction in payment module double loading of adj values, put rem

            DisplayDuesNew(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            if (chkAdj.Checked)
                OnCheckAdj();

            if (chkRetirement.Checked)
                OnRetirement();

            m_bDiscQtr = false;

            if (m_sPaymentMode == "OFL")
            {
                //OFLPaymentCondition(); MCR 20140902 REM This Code Doesn't Exist on Antipolo Version
            }
        }

        private void On2ndQtr()
        {
            if (chk2nd.Checked)
            {
                chk3rd.Checked = false;
                chk4th.Checked = false;
                SetCheckTerm(chkInstallment);
            }
            else
            {
                if (!chk1st.Checked)
                {
                    if ((chkAdj.Checked || chkRetirement.Checked) && chk3rd.Checked == false)
                    {
                        chk2nd.Checked = false;
                        SetCheckTerm(chkFull);
                    }
                    else
                        chk2nd.Checked = true;
                }
                else
                {
                    chk3rd.Checked = false;
                    chk4th.Checked = false;
                }
            }


            if (m_sNextQtrToPay == "1" && !chk1st.Checked)
            {
                chk2nd.Checked = false;
            }


            String sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4;
            if (chk1st.Checked)
                sQToBePaid1 = "1";
            else
                sQToBePaid1 = "";
            if (chk2nd.Checked)
                sQToBePaid2 = "2";
            else
                sQToBePaid2 = "";
            if (chk3rd.Checked)
                sQToBePaid3 = "3";
            else
                sQToBePaid3 = "";
            if (chk4th.Checked)
                sQToBePaid4 = "4";
            else
                sQToBePaid4 = "";

            String sTmpTaxCredit;
            sTmpTaxCredit = txtToTaxCredit.Text;

            dgvTaxFees.Rows.Clear();
            txtToTaxCredit.Text = sTmpTaxCredit;

            m_bExistTaxPU = false;

            /*
            // RMC 20150112 mods in retirement billing (s)
            if (chkAdj.Checked == true)
                DisplayDuesNew(m_sBIN, "A", "", "", "");
            // RMC 20150112 mods in retirement billing (e)
             */
            // RMC 20160719 correction in payment module double loading of adj values, put rem

            DisplayDuesNew(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            // Discount
            if (Convert.ToDouble(m_sCreditLeft) > 0.00)
            {
                DeductTaxCredit();
                if (Convert.ToDouble(m_sDiscountedTotal) <= Convert.ToDouble(m_sCreditLeft))
                {
                    txtToTaxCredit.ReadOnly = true;
                    txtToTaxCredit.Text = Convert.ToDouble(m_sDiscountedTotal).ToString("#,##.00");
                }

                if (Convert.ToDouble(m_sCreditLeft) > 0.00 && (Convert.ToDouble(m_sDiscountedTotal) > Convert.ToDouble(m_sCreditLeft)))
                {
                    txtToTaxCredit.Text = m_sCreditLeft;
                }

                CompLessCredit(m_sDiscountedTotal, m_sCreditLeft, txtToTaxCredit.Text);
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

            if (chkAdj.Checked)
                OnCheckAdj();

            if (chkRetirement.Checked)
                OnRetirement();
            m_bDiscQtr = false;

            if (m_sPaymentMode == "OFL")
            {
                //OFLPaymentCondition(); MCR 20140902 REM This Code Doesn't Exist on Antipolo Version
            }
        }

        private void On3rdQtr()
        {
            if (chk3rd.Checked)
            {
                chk4th.Checked = false;
                SetCheckTerm(chkInstallment);
            }
            else
            {
                if (!chk1st.Checked && !chk2nd.Checked)
                {
                    if (chkAdj.Checked || chkRetirement.Checked)
                    {
                        chk3rd.Checked = false;
                        SetCheckTerm(chkFull);
                    }
                    else
                        chk3rd.Checked = true;
                }
                else
                    chk4th.Checked = false;
            }

            if (m_sNextQtrToPay == "1" && !chk2nd.Checked)
                chk3rd.Checked = false;

            if (m_sNextQtrToPay == "2" && !chk2nd.Checked)
                chk3rd.Checked = false;

            String sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4;
            if (chk1st.Checked)
                sQToBePaid1 = "1";
            else
                sQToBePaid1 = "";
            if (chk2nd.Checked)
                sQToBePaid2 = "2";
            else
                sQToBePaid2 = "";
            if (chk3rd.Checked)
                sQToBePaid3 = "3";
            else
                sQToBePaid3 = "";
            if (chk4th.Checked)
                sQToBePaid4 = "4";
            else
                sQToBePaid4 = "";

            String sTmpTaxCredit;
            sTmpTaxCredit = txtToTaxCredit.Text;
            dgvTaxFees.Rows.Clear();
            txtToTaxCredit.Text = sTmpTaxCredit;
            m_bExistTaxPU = false;

          /*  // RMC 20150112 mods in retirement billing (s)
            if (chkAdj.Checked == true)
                DisplayDuesNew(m_sBIN, "A", "", "", "");
            // RMC 20150112 mods in retirement billing (e)
            */
            // RMC 20160719 correction in payment module double loading of adj values, put rem

            DisplayDuesNew(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            if (chkRetirement.Checked)
                OnRetirement();

            //Discount
            if (Convert.ToDouble(m_sCreditLeft) > 0.00)
            {
                DeductTaxCredit();
                if (Convert.ToDouble(m_sDiscountedTotal) <= Convert.ToDouble(m_sCreditLeft))
                {
                    txtToTaxCredit.ReadOnly = true;
                    txtToTaxCredit.Text = Convert.ToDouble(m_sDiscountedTotal).ToString("#,##.00");
                }

                if (Convert.ToDouble(m_sCreditLeft) > 0.00 && (Convert.ToDouble(m_sDiscountedTotal) > Convert.ToDouble(m_sCreditLeft)))
                {
                    txtToTaxCredit.Text = m_sCreditLeft;
                }

                CompLessCredit(m_sDiscountedTotal, m_sCreditLeft, txtToTaxCredit.Text);
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

            if (chkAdj.Checked)
                OnCheckAdj();
            m_bDiscQtr = false;

            if (m_sPaymentMode == "OFL")
            {
                //OFLPaymentCondition(); MCR 20140902 REM This Code Doesn't Exist on Antipolo Version
            }
        }

        private void On4thQtr()
        {
            if (chk4th.Checked)
            {
                if ((chk1st.Checked) && (!chk2nd.Checked))
                {
                    chk1st.Checked = false;
                }
                if ((chk1st.Checked) && (!chk3rd.Checked))
                {
                    chk1st.Checked = false;
                    chk2nd.Checked = false;
                }
                if ((chk2nd.Checked) && (!chk3rd.Checked))
                {
                    chk2nd.Checked = false;
                    chk3rd.Checked = false;
                }
                SetCheckTerm(chkInstallment);

                if (chk4th.Checked == true && AppSettingsManager.OnCheckIfDiscounted(dtpORDate.Value.ToString("MM/dd/yyyy")) == true && txtBnsStat.Text != "NEW")
                    m_bDiscQtr = true;
            }
            else
            {
                if (!chk1st.Checked && !chk2nd.Checked && !chk3rd.Checked)
                {
                    if (chkAdj.Checked || chkRetirement.Checked)
                    {
                        chk4th.Checked = false;
                        SetCheckTerm(chkFull);
                    }
                    else
                        chk4th.Checked = true;
                }
                m_bDiscQtr = false;
            }

            if (m_sNextQtrToPay == "1" && !chk3rd.Checked)
                chk4th.Checked = false;

            if (m_sNextQtrToPay == "2" && !chk3rd.Checked)
                chk4th.Checked = false;

            if (m_sNextQtrToPay == "3" && !chk3rd.Checked)
                chk4th.Checked = false;

            String sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4;
            if (chk1st.Checked)
                sQToBePaid1 = "1";
            else
                sQToBePaid1 = "";
            if (chk2nd.Checked)
                sQToBePaid2 = "2";
            else
                sQToBePaid2 = "";
            if (chk3rd.Checked)
                sQToBePaid3 = "3";
            else
                sQToBePaid3 = "";
            if (chk4th.Checked)
                sQToBePaid4 = "4";
            else
                sQToBePaid4 = "";

            String sTmpTaxCredit;
            sTmpTaxCredit = txtToTaxCredit.Text;
            dgvTaxFees.Rows.Clear();
            txtToTaxCredit.Text = sTmpTaxCredit;
            m_bExistTaxPU = false;

            /*// RMC 20150112 mods in retirement billing (s)
            if (chkAdj.Checked == true)
                DisplayDuesNew(m_sBIN, "A", "", "", "");
            // RMC 20150112 mods in retirement billing (e)
             */
            // RMC 20160719 correction in payment module double loading of adj values, put rem

            DisplayDuesNew(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            if (chkRetirement.Checked)
                OnRetirement();

            //Discount
            if (Convert.ToDouble(m_sCreditLeft) > 0.00)
            {
                DeductTaxCredit();
                if (Convert.ToDouble(m_sDiscountedTotal) <= Convert.ToDouble(m_sCreditLeft))
                {
                    txtToTaxCredit.ReadOnly = true;
                    txtToTaxCredit.Text = Convert.ToDouble(m_sDiscountedTotal).ToString("#,##.00");
                }

                if (Convert.ToDouble(m_sCreditLeft) > 0.00 && (Convert.ToDouble(m_sDiscountedTotal) > Convert.ToDouble(m_sCreditLeft)))
                {
                    txtToTaxCredit.Text = m_sCreditLeft;
                }

                CompLessCredit(m_sDiscountedTotal, m_sCreditLeft, txtToTaxCredit.Text);
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

            if (chkAdj.Checked)
                OnCheckAdj();

            if (chk1st.Checked == true && chk4th.Checked == true && m_blnIsFull) // CJC 20131011
            {
                chk1st.Checked = false;
                chk2nd.Checked = false;
                chk3rd.Checked = false;
                chk4th.Checked = false;
                chk4th.Enabled = false;
                OnFull();
            }

            if (m_sPaymentMode == "OFL")
            {
                //OFLPaymentCondition(); MCR 20140902 REM This Code Doesn't Exist on Antipolo Version
            }
        }

        private void dgvTaxFees_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (bin.Enabled == false) //MCR 20140811
            if (bin.txtTaxYear.Text.Trim() != "" && bin.txtBINSeries.Text.Trim() != "")  // RMC 20180125 enabled editing of dues in Offline payment
                ComputeTotal();
        }

        private void chk1st_Click(object sender, EventArgs e)
        {
            On1stQtr();
        }

        private void chk2nd_Click(object sender, EventArgs e)
        {
            On2ndQtr();
        }

        private void chk3rd_Click(object sender, EventArgs e)
        {
            On3rdQtr();
        }

        private void chk4th_Click(object sender, EventArgs e)
        {
            On4thQtr();
        }

        private void chkCash_Click(object sender, EventArgs e)
        {
            OnCash();
        }

        private void chkCheque_Click(object sender, EventArgs e)
        {
            OnCheck();
        }

        private void chkTCredit_Click(object sender, EventArgs e)
        {
            CheckPaymentType();

            // RMC 20170117 correction in Payment module if tax credit is unchecked (s)
            if (chkTCredit.Checked == false)
            {
                txtCreditLeft.Text = "0";
                txtToTaxCredit.Text = "0";
                m_sCreditLeft = "0";
                CompLessCredit(txtTotTotDue.Text,m_sCreditLeft, txtToTaxCredit.Text);
            }
            // RMC 20170117 correction in Payment module if tax credit is unchecked (e)
        }

        //MCR 20140721 (e)

        private void CleanMe()
        {
            bin.txtTaxYear.Text = string.Empty;
            bin.txtBINSeries.Text = string.Empty;
            //            txtORNo.Text = string.Empty;
            txtBnsName.Text = string.Empty;
            txtCompromise.Enabled = false;
            chkPartial.Checked = false;
            chkPartial.Enabled = false;
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
            m_sToBeDebited = "0.00";
            m_sCreditLeft = "0.00"; // RMC 20150505 QA reports
            txtGrandTotal.Text = string.Empty;
            chkFull.Checked = false;
            chkInstallment.Checked = false;
            chk1st.Checked = false;
            chk2nd.Checked = false;
            chk3rd.Checked = false;
            chk4th.Checked = false;
            chkCash.Checked = false;
            chkCheque.Checked = false;
            chkCharges.Checked = false;
            chkAdj.Checked = false;
            chkRetirement.Checked = false;
            chkTCredit.Checked = false;
            dgvTaxFees.Rows.Clear();
            m_sBIN = string.Empty;
            bin.txtTaxYear.Focus();

            // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination (s)
            m_sDebitCredit = "0.00";
            m_sCCChkAmt = "0.00";   
            m_sCCCashAmt = "0.00";  
            // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination (e)

            m_iORNo = 0;    // RMC 20151229 merged multiple OR use from Mati
            txtMemo.Text = "";  // RMC 20151229 merged multiple OR use from Mati
        }

        //MCR 20140811 (s)
        private void CompLessCredit(String sTotalTotalDue, String sCreditLeft, String sToBeCredited)
        {
            String sGrandTotalDue;
            if (Convert.ToDouble(sTotalTotalDue) > Convert.ToDouble(sToBeCredited))
            {
                sGrandTotalDue = Convert.ToDouble(Convert.ToDouble(sTotalTotalDue) - Convert.ToDouble(sToBeCredited)).ToString("#,##0.00");
                sCreditLeft = Convert.ToDouble(Convert.ToDouble(sCreditLeft) - Convert.ToDouble(sToBeCredited)).ToString("#,##0.00");
            }
            else
                sGrandTotalDue = Convert.ToDouble(Convert.ToDouble(sTotalTotalDue) - Convert.ToDouble(sToBeCredited)).ToString("#,##0.00");

            if (Convert.ToDouble(sGrandTotalDue) < 0)
                sGrandTotalDue = "0.00";

            txtGrandTotal.Text = sGrandTotalDue;
            txtCreditLeft.Text = sCreditLeft;
        }

        private void txtToTaxCredit_Leave(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtToTaxCredit.Text) > Convert.ToDouble(txtTotTotDue.Text))
            {
                MessageBox.Show("Amount must be less than the total amount due", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtToTaxCredit.Focus();
                //return;
            }
            else
            {
                if (Convert.ToDouble(txtToTaxCredit.Text) > Convert.ToDouble(m_sCreditLeft))
                {
                    MessageBox.Show("Amount must be less than the total credit left", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtToTaxCredit.Focus();
                    //return;
                }
                else
                {
                    CompLessCredit(m_sDiscountedTotal, m_sCreditLeft, txtToTaxCredit.Text);
                }
            }
        }

        private void txtCompromise_Leave(object sender, EventArgs e)
        {
            double dFeesDue = 0, dFeesSurch = 0, dFeesPen = 0, dFeesTotal = 0;
            String sDesc = "", sString = "", sPayType = "I";
            int iRow = 0, iTotalRow = 0;

            if (txtCompromise.Text.Trim() == "")
                txtCompromise.Text = "0";

            if (Convert.ToDouble(txtCompromise.Text) < 0)
                txtCompromise.Text = "0";

            if (Convert.ToDouble(txtCompromise.Text) > m_iNoPaySeq)
                txtCompromise.Text = m_iNoPaySeq.ToString();

            if (bTaxDueFound == false)
            {
                chkCheque.Checked = true;
                chkCash.Checked = false;
            }

            if (Convert.ToDouble(txtCompromise.Text) >= 0 && Convert.ToDouble(txtCompromise.Text) <= Convert.ToDouble(m_iNoPaySeq))
            {
                OnCompromise(m_sBIN, "1", "");

                if (Convert.ToDouble(txtCompromise.Text) > 1 && Convert.ToDouble(m_sAddlCompPen) > 0)
                {
                    MessageBox.Show("Compromise due has an additional penalty. Only one payment is allowed", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCompromise.Text = "";
                    return;
                }

                if (Convert.ToDouble(txtCompromise.Text) >= 1) // ALJ 08112006
                {
                    //if (Convert.ToDouble(m_sAddlCompPen) > 0 && dgvTaxFees.Rows[dgvTaxFees.RowCount-1].Cells[8].Value.ToString() == "CD") // "CD" means already displayed the 25% down payment
                    if (Convert.ToDouble(m_sAddlCompPen) > 0 && dgvTaxFees.Rows[dgvTaxFees.RowCount].Cells[8].Value.ToString() == "CD") // "CD" means already displayed the 25% down payment
                    {
                        MessageBox.Show("Compromise due has an additional penalty. Only one payment is allowed", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtCompromise.Text = "0"; // initial down payment
                        return;
                    }

                    dFeesDue = m_dFeesDue * Convert.ToDouble(txtCompromise.Text);
                    dFeesSurch = m_dFeesSurch * Convert.ToDouble(txtCompromise.Text);
                    dFeesPen = m_dFeesPen * Convert.ToDouble(txtCompromise.Text);
                    dFeesTotal = dFeesDue + dFeesSurch + dFeesPen;

                    sDesc = "COMPROMISE AGREEMENT";

                    if (Convert.ToDouble(txtCompromise.Text) > 1)
                        sDesc += "(" + txtCompromise.Text + " payments)";

                    else if (Convert.ToDouble(txtCompromise.Text) == 1)
                    {
                        if (m_iTermToPayComp == 0)
                            m_iTermToPayComp = 1;
                        sDesc = string.Format("COMPROMISE AGREEMENT (MONTHLY) {0} of {1}", m_iTermToPayComp, m_iNoPaySeq);
                    }

                    //iTotalRow = dgvTaxFees.RowCount-1;
                    iTotalRow = dgvTaxFees.RowCount;

                    if (dgvTaxFees.Rows[iTotalRow].Cells[8].Value.ToString() == "C")
                        dgvTaxFees.Rows.RemoveAt(iTotalRow);

                    dgvTaxFees.Rows.Add("", sPayType, sString, sDesc, dFeesDue, dFeesSurch, dFeesPen + Convert.ToDouble(m_sAddlCompPen), dFeesTotal + Convert.ToDouble(m_sAddlCompPen), "C", "", "");
                }

                ComputeTotal();
                txtGrandTotal.Text = m_sDiscountedTotal;
                CompLessCredit(txtTotTotDue.Text, m_sCreditLeft, txtToTaxCredit.Text);
                return;
            }
        }

        private void txtCompromise_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtTaxYear_Leave(object sender, EventArgs e)
        {
            if (txtTaxYear.ReadOnly == false)
            {
                OracleResultSet result = new OracleResultSet();
                if (txtTaxYear.Text.Trim().Length == 4)
                {
                    result.Query = "delete from pay_temp where bin = '" + m_sBIN.Trim() + "'";
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                    result.Close();

                    dgvTaxFees.Rows.Clear();

                    result.Query = "select * from taxdues where bin = '" + m_sBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {

                        }
                        else
                        {
                            MessageBox.Show("No taxdues for the said tax year.");
                            return;
                        }
                    }
                    result.Close();

                    ComputeTaxAndFees(m_sBIN.Trim());

                    m_sCreditLeft = "0.00";
                    txtToTaxCredit.Text = "0.00";
                    if (m_sOwnCode.Trim() != "")
                    {
                        // RMC 20141127 QA Tarlac ver (s)
                        //if (AppSettingsManager.GetConfigValue("10") == "017" || AppSettingsManager.GetConfigValue("10") == "216") //JARS 20170713
                        //    result.Query = "select * from dbcr_memo where own_code = '" + AppSettingsManager.GetBnsOwnCode(m_sBIN) + "' and served = 'N' and multi_pay = 'N'";
                        //else// RMC 20141127 QA Tarlac ver (e)
                            result.Query = "select * from dbcr_memo where bin = '" + m_sBIN + "' and served = 'N' and multi_pay = 'N'"; //JARS 20171010
                        if (result.Execute())
                            if (result.Read())
                            {
                                txtToTaxCredit.Text = result.GetDouble("balance").ToString("#,##0.00");
                                txtCreditLeft.Text = result.GetDouble("balance").ToString("#,##0.00");
                                m_sCreditLeft = result.GetDouble("balance").ToString("#,##0.00");
                            }
                        result.Close();
                    }

                    if (Convert.ToDouble(txtTotTotDue.Text) <= Convert.ToDouble(txtCreditLeft.Text))
                    {
                        txtToTaxCredit.Text = txtTotTotDue.Text;
                    }

                    CompLessCredit(m_sDiscountedTotal, m_sCreditLeft, txtToTaxCredit.Text);

                    if (m_sPaymentTerm == "I" && !chk1st.Checked)
                        chkPartial.Enabled = false;
                    else
                        chkPartial.Enabled = false;
                }
            }
        }

        //MCR 20140811(e)

        private void UpdateWarrant(String p_sOwnCode, String p_sBin)
        {
            /*
            OracleResultSet pSet = new OracleResultSet();
            pSet.CreateNewConnectionRPTA();
            OracleResultSet pSet1 = new OracleResultSet();
            String sRptaOwnCode = "";

            if (AppSettingsManager.GetConfigValue("0") == "Y")//("RPTA")
            {
                pSet.Query = string.Format("select rpta_own_code from rpta_bpls_own where bpls_own_code = '{0}'", p_sOwnCode);
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sRptaOwnCode = pSet.GetString("rpta_own_code").Trim();
                        pSet.Query = string.Format("update bpls_levy_tag set delq_settled = 'Y', settled_by = '{0}' where rpta_own_code = '{1}'", AppSettingsManager.SystemUser.UserName, sRptaOwnCode);
                        pSet.ExecuteNonQuery();
                    }
                pSet.Close();

                pSet1.Query = string.Format("delete from bpls_levy where bpls_own_code = '{0}' and bin = '{1}'", p_sOwnCode, p_sBin);
                pSet1.ExecuteNonQuery();

                pSet1.Query = string.Format("delete from bpls_delq_table where bpls_own_code = '{0}' and bin = '{1}'", p_sOwnCode, p_sBin);
                pSet1.ExecuteNonQuery();
            }*/

        }

        private void chkCash_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dtpORDate_Leave(object sender, EventArgs e)
        {
            // RMC 20170131 recompute dues displayed when date was changed in Offline payment
            ComputeTaxAndFees(bin.GetBin());

        }

        //MCR 20140812

        private void PrintOR()
        {
            // RMC 20141020 printing of OR
            frmLiqReports frmliqreports = new frmLiqReports();
            //frmliqreports.ReportSwitch = "ReprintOR"; // AST 20160125 rem
            frmliqreports.ReportSwitch = "PrintOR"; // AST 20160125
            frmliqreports.DataGridView = dgvTaxFees;
            frmliqreports.formUse = "Not";

            frmliqreports.m_sBIN = bin.GetBin();
            frmliqreports.m_sTaxYear = txtTaxYear.Text;
            frmliqreports.m_sStatus = txtBnsStat.Text;

            frmliqreports.BCode = txtBnsCode.Text;
            frmliqreports.ORDate = dtpORDate.Value.ToShortDateString();
            frmliqreports.ORNo = txtORNo.Text;

            frmliqreports.bnsAddress = txtBnsAddress.Text;
            frmliqreports.bnsName = txtBnsName.Text;
            frmliqreports.bnsOwner = txtBnsOwner.Text;
            frmliqreports.m_sBrgy = AppSettingsManager.GetBnsBrgy(m_sBIN);
            frmliqreports.ownCode = m_sOwnCode;
            frmliqreports.term = "Installment";

            frmliqreports.Teller = txtTeller.Text;
            frmliqreports.PaymentType = m_sPaymentType;
            double dToBeCredited = 0;
            double.TryParse(txtToTaxCredit.Text.ToString(), out dToBeCredited);
            frmliqreports.ToBeCredited = dToBeCredited;


            if (chkFull.Checked == true)
            {
                frmliqreports.term = "Full";
            }

            //frmliqreports.formUse = "Reprinted";
            frmliqreports.ShowDialog();

        }

        private int GetRemainingOR(string sTeller)
        {
            // RMC 20151229 merged multiple OR use from Mati
            OracleResultSet pRec = new OracleResultSet();

            string sCurrOR, sORTo;

            int iRemOR = 0;

            pRec.Query = "select * from or_current where teller = '" + sTeller + "'";
            if(pRec.Execute())
            {
                if (pRec.Read())
                {
                    sCurrOR = pRec.GetString("cur_or_no");
                    sORTo = pRec.GetString("to_or_no");

                    iRemOR = Convert.ToInt32(sORTo) - Convert.ToInt32(sCurrOR) + 1;
                }
            }
            pRec.Close();

            return iRemOR;
        }

        private void chkCheque_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void InsertAddlBns()
        {
            // RMC 20170110 added insertion of NEW addl line of business included in REN application
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();
            string sBnsCode = string.Empty;


            //AFM 20200312 merged from LUNA - JAA 20200108 merged from binan LUN-20-11811(s)
            //pSet.Query = "select * from addl_bns_que where bin = '" + m_sBIN.Trim() + "' ";
            //pSet.Query += "and bns_stat = 'NEW' and tax_year = '" + txtTaxYear.Text.Trim() + "'";

            pSet.Query = "select * from addl_bns_que where bin = '" + m_sBIN.Trim() + "' ";
            pSet.Query += "and tax_year = '" + txtTaxYear.Text.Trim() + "'"; //MCR 20180109 removed bns_stat
            //AFM 20200312 merged from LUNA - JAA 20200108 merged from binan LUN-20-11811(e)

            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sBnsCode = pSet.GetString("bns_code_main");

                    result.Query = "select * from addl_bns where bin = '" + m_sBIN.Trim() + "' ";
                    result.Query += " and bns_code_main = '" + sBnsCode + "' ";
                    result.Query += " and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                    if (result.Execute())
                    {
                        if (!result.Read())
                        {
                            result.Close();

                            result.Query = "insert into addl_bns ";
                            result.Query += "select * from addl_bns_que where bin = '" + m_sBIN.Trim() + "' ";
                            result.Query += " and bns_code_main = '" + sBnsCode + "' ";
                            result.Query += " and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                            if (result.ExecuteNonQuery() == 0)
                            { }

                            result.Query = "delete from addl_bns_que where bin = '" + m_sBIN.Trim() + "' ";
                            result.Query += " and bns_code_main = '" + sBnsCode + "' ";
                            result.Query += " and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                            if (result.ExecuteNonQuery() == 0)
                            { }

                        }
                    }
                    result.Close();
                }
            }
            pSet.Close();
        }

        private void chkTCredit_CheckedChanged(object sender, EventArgs e)
        {

        }

        private bool ValidatePUAddlBns(string sBin)
        {
            // RMC 20170404 additional validation in SOA and Payment if with new addl bns
            OracleResultSet pSetPU = new OracleResultSet();
            string sFeesCode = string.Empty;
            string sTaxYear = string.Empty;

            pSetPU.Query = "select * from  pay_temp where bin = '" + sBin + "' and due_state = 'P' and fees_code like 'B%'";
            if (pSetPU.Execute())
            {
                if (pSetPU.Read())
                {
                    sFeesCode = pSetPU.GetString("fees_code");
                    sTaxYear = pSetPU.GetString("tax_year");
                }
            }
            pSetPU.Close();

            if (sFeesCode != "")
            {
                sFeesCode = sFeesCode.Substring(1, sFeesCode.Length - 1);

                pSetPU.Query = "select * from permit_update_appl where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "'";
                pSetPU.Query += " and new_bns_code = '" + sFeesCode + "' and appl_type = 'ADDL'";
                if (pSetPU.Execute())
                {
                    if (pSetPU.Read())
                    {
                        pSetPU.Close();
                        return true;
                    }
                }
                pSetPU.Close();
            }

            return false;
        }

        private void GetTransTableValues(string sBIN)
        {
            // RMC 20171122 added capturing of new DTI no., DTI date, and Memoranda in Permit-Update transfer of ownership
            OracleResultSet pRecPUT = new OracleResultSet();

            m_sTrDTINo = string.Empty;
            m_dTrDTIDt = AppSettingsManager.GetCurrentDate();
            m_sTrMemo = string.Empty;

            pRecPUT.Query = "select * from TRANS_OTHER_INFO_ADDL where bin = '" + sBIN + "'";
            pRecPUT.Query += " and tax_year = '" + txtTaxYear.Text + "' and (default_code = 'DTINO' or default_code = 'DTIDT' or default_code = 'MEMO')";
            if (pRecPUT.Execute())
            {
                while (pRecPUT.Read())
                {
                    if (pRecPUT.GetString("default_code") == "DTINO")
                        m_sTrDTINo = pRecPUT.GetString("data");
                    if (pRecPUT.GetString("default_code") == "DTIDT")
                    {
                        try
                        {
                            m_dTrDTIDt = Convert.ToDateTime(pRecPUT.GetString("data"));
                        }
                        catch
                        {
                            m_dTrDTIDt = AppSettingsManager.GetCurrentDate();
                        }
                    }

                    if (pRecPUT.GetString("default_code") == "MEMO")
                        m_sTrMemo = pRecPUT.GetString("data");
                }
            }
            pRecPUT.Close();
        }

       //JHB 201812103 merge form Lal-lo update for revExam JARS 20181129
        private double GetBillGrossInfo(string sBIN, string sTaxYear, string sBnsCode, string sDueState)
        {
            OracleResultSet result = new OracleResultSet();
            double dGrossCap = 0;
            //sBnsCode = sFAFeesCode.Substring(1, sFAFeesCode.Length - 1);
            

            result.Query = "select * from bill_gross_info where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "' and bns_code = '"+ sBnsCode +"' and bns_code in ";
            result.Query += "(select bns_code from business_que where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "' union select bns_code from businesses where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "')";
            if (result.Execute())
            {
                if (result.Read())
                {
                    if (txtBnsStat.Text == "NEW")
                    {
                        dGrossCap = result.GetDouble("capital");
                    }
                    else
                    {
                        if (sDueState == "A")
                        {
                            dGrossCap = result.GetDouble("adj_gross");
                        }
                        else
                        {
                            dGrossCap = result.GetDouble("gross");
                        }
                    }
                }
            }
            result.Close();
            return dGrossCap;
        }
    }
}




    
