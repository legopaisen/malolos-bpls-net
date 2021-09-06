using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
namespace NewPayment
{
    public partial class frmPayments : Form
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
        string m_sBIN = string.Empty;
        public string m_sPaymentTerm = string.Empty;
        bool m_bIsCompromise = false;
        bool m_bIsDisCompromise = false;
        double m_dTotalDue = 0, m_dTotalSurcharge = 0, m_dTotalPenalty = 0, m_dTotalTotalDue = 0;
        double m_dTotalDue1 = 0, m_dTotalSurcharge1 = 0, m_dTotalPenalty1 = 0, m_dTotalTotalDue1 = 0;
        string m_sStatus = string.Empty;
        double dDue = 0, dPen = 0, dSurch = 0, dTot = 0;
        string m_sCCChkAmt = string.Empty;

        string p_bin = string.Empty, p_tax_year = string.Empty, p_term = string.Empty, p_qtr = string.Empty, p_fees_desc = string.Empty;
        string p_fees_due = string.Empty, p_fees_surch = string.Empty, p_fees_pen = string.Empty, p_fees_totaldue = string.Empty;
        string p_fees_code = string.Empty, p_due_state = string.Empty, p_qtr_to_pay = string.Empty;
        string m_sFullPartial = string.Empty;
        string m_sOwnCode = string.Empty;
        bool m_bIsRenPUT = false;
        string m_sPrevOwnCode = string.Empty;
        string m_sCheckOwnerCode = string.Empty;
        string m_sOldBnsName = string.Empty;
        string m_sDtOperated = string.Empty;
        string m_sPrevBnsLoc = string.Empty;
        string m_sCompPaySw = string.Empty;
        string m_sQtr1 = string.Empty;
        string m_sTerm = string.Empty;
        int m_iNextQtrToPay = 0;
        string m_strORNo = string.Empty;
        string m_sCurrentTaxYear = string.Empty;
        string m_sPreviousOR = string.Empty;
        int m_iMultiTaxYear = 0;
        string m_sQtr = string.Empty;
        string m_sTaxCreditAmount = string.Empty;
        public frmPayments()
        {
            InitializeComponent();
        }

        private void btnLoadBIN_Click(object sender, EventArgs e)
        {
            if (btnLoadBIN.Text == "Load")
            {
                if (bin1.txtTaxYear.Text != string.Empty && bin1.txtBINSeries.Text != string.Empty)
                {
                    SearchBIN(bin1.GetBin());
                    btnLoadBIN.Text = "Clear";
                }

            }
            else
            {
                dgvParticulars.Rows.Clear();
                dgvSubTotals.Rows.Clear();
                dgvTotal.Rows.Clear();
                btnLoadBIN.Text = "Load";
                txtOwnName.Text = string.Empty;
                txtBnsAdd.Text = string.Empty;
                txtMemo.Text = string.Empty;
                txtOwnCode.Text = string.Empty;
                txtCash.Text = string.Empty;
                txtCheck.Text = string.Empty;
                txtCrediMemo.Text = string.Empty;
                txtPartial.Text = string.Empty;
                txtTaxYear.Text = string.Empty;
                txtBnsStat.Text = string.Empty;
                chk1st.Checked = false;
                chk2nd.Checked = false;
                chk3rd.Checked = false;
                chk4th.Checked = false;
                chkCash.Checked = false;
                chkCheck.Checked = false;
                chkFull.Checked = false;
                chkInstallment.Checked = false;
                chkPartial.Checked = false;
                chkReceived.Checked = false;
                btnSearchOwnCode.Text = "Search";
                bin1.txtTaxYear.Text = string.Empty;
                bin1.txtBINSeries.Text = string.Empty;
                bin1.txtTaxYear.Focus();
            }

        }

        private void SearchBIN(string sBin)
        {
            m_sBIN = sBin;
            int iBinCount = 0;
            bool blnIsMultiBns = false;
            OracleResultSet result = new OracleResultSet();
            if (sBin.Substring(7, 4) == string.Empty || sBin.Substring(12, 7) == string.Empty)
            {
                MessageBox.Show("NO BIN");
            }
            else
            {
                result.Query = "select count(bin)as iCount from businesses where own_code in (select own_code from businesses where bin = :1)";
                result.AddParameter(":1", sBin.Trim());
                if(result.Execute())
                    if(result.Read())
                        iBinCount = result.GetInt("iCount");
                result.Close();

                if(iBinCount > 1)
                    blnIsMultiBns = true;
                else
                    blnIsMultiBns = false;
                
                CheckBnsStatus(sBin.Trim());




            }
        }

        private void CheckBnsStatus(string sBin)
        {
            string sTaxYear = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from business_que where bin = :1";
            result.AddParameter(":1", sBin.Trim());
            if(result.Execute())
            {
                if (result.Read())
                { }
                else
                {
                    result.Close();
                    result.Query = "select * from businesses where bin = :1";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            result.Close();
                            result.Query = "select * from treasurers_module where bin = :1 and (action = '0' or action = '1') and tax_year <= :2";
                            result.AddParameter(":1", sBin.Trim());
                            result.AddParameter(":2", AppSettingsManager.GetCurrentDate().Year.ToString());
                            if(result.Execute())
                                if(result.Read())
                                {
                                    MessageBox.Show("SOA is subject for verification.");
                                    return;
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

            // FOR BOUNCE CHECK
            result.Query = "select * from bounce_chk_rec where bin = :1";
            result.AddParameter(":1", sBin.Trim());
            if (result.Execute())
            {
                if (result.Read())
                {
                    MessageBox.Show("Cannot proceed. \nThis record has been paid under a bounced check...");
                    return;
                }
            }
            result.Close();

            // FOR CLOSURE TAGGING
            result.Query = "select distinct bin from closure_tagging where bin = :1";
            result.AddParameter(":1", sBin.Trim());
            if (result.Execute())
            {
                if (result.Read())
                {
                    MessageBox.Show("Record was tagged for closure. Cannot continue.");
                    return;
                }
            }
            result.Close();

            // FOR BUSINESS ON-HOLD
            string sHRUser, sHRDate, sHRRemarks, sMess;
            result.Query = "select * from hold_records where bin = :1 and status = 'HOLD'";
            result.AddParameter(":1", sBin.Trim());
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

            result.Query = "select * from business_que where bin = :1";
            result.AddParameter(":1", sBin.Trim());
            if (result.Execute())
            {
                if (result.Read())
                {
                    txtBnsStat.Text = result.GetString("bns_stat").Trim();
                    txtTaxYear.Text = result.GetString("tax_year").Trim();
                    m_sOwnCode = result.GetString("own_code").Trim();
                    result.Close();
                    result.Query = "select * from permit_update_appl where bin = :1 and appl_type = 'TOWN' and data_mode = 'QUE'";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            m_sPrevOwnCode = m_sOwnCode;
                            m_sOwnCode = result.GetString("new_own_code").Trim();
                            m_bIsRenPUT = true;
                        }
                    }
                    result.Close();

                    result.Query = "select * from permit_update_appl where bin = :1 and appl_type = 'ADDL' and data_mode = 'QUE'";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            m_bIsRenPUT = true;
                        }
                    }
                    result.Close();

                    m_sCheckOwnerCode = m_sOwnCode;

                    // BUSINESS NAME
                    // BUSINESS CODE

                    result.Query = "select * from permit_update_appl where bin = :1 and appl_type = 'CBNS' and data_mode = 'QUE'";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {

                            m_sOldBnsName = result.GetString("old_bns_nm").Trim();
                            m_bIsRenPUT = true;
                        }
                    }
                    result.Close();

                    result.Query = "select * from permit_update_appl where bin = '" + sBin.Trim() + "' and appl_type = 'CTYP' and data_mode = 'QUE'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            result.Close();
                            result.Query = "select new_bns_code from change_class_tbl where bin = '" + sBin.Trim() + "' and is_main = 'Y'";
                            if (result.Read())
                            {
                                txtBnsCode.Text = result.GetString("new_bns_code").Trim();
                                m_bIsRenPUT = true;
                            }
                        }
                    }
                    result.Close();

                    

                    result.Query = "select * from business_que where bin = :1";
                    result.AddParameter(":1", sBin.Trim());
                    if(result.Execute())
                        if(result.Read())
                            m_sDtOperated = result.GetString("dt_operated").Trim();
                    result.Close();

                    // ADD CODES FOR RETIREMENT RET001

                    txtBnsAdd.Text = AppSettingsManager.GetBnsAdd(sBin.Trim());
                    txtOwnName.Text = AppSettingsManager.GetBnsOwner(m_sOwnCode);

                    // BNS TYPe
                    // BNS CODE
                }
                else
                {
                    result.Close();
                    result.Query = "select * from businesses where bin = :1";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            txtBnsStat.Text = result.GetString("bns_stat").Trim();
                            txtTaxYear.Text = result.GetString("tax_year").Trim();
                            txtBnsCode.Text = result.GetString("bns_code").Trim();
                            m_sOwnCode = result.GetString("own_code").Trim();
                            result.Close();
                            result.Query = "select * from permit_update_appl where bin = :1 and appl_type = 'TOWN' and data_mode = 'QUE'";
                            result.AddParameter(":1", sBin.Trim());
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    m_sPrevOwnCode = m_sOwnCode;
                                    m_sOwnCode = result.GetString("new_own_code").Trim();
                                    m_bIsRenPUT = true;
                                }
                            }
                            result.Close();

                            // m_sPlaceOccupancy insert here

                            m_sCheckOwnerCode = m_sOwnCode;
                            //txtBnsName.Text = result.GetString("bns_nm").Trim();
//                            txtBnsCode.Text = result2.GetString("bns_code").Trim();

                            result.Query = "select * from permit_update_appl where bin = :1 and appl_type = 'CBNS' and data_mode = 'QUE'";
                            result.AddParameter(":1", sBin.Trim());
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    //txtBnsName.Text = string.Empty;
                                    //txtBnsName.Text = result.GetString("new_bns_name").Trim();
                                    m_sOldBnsName = result.GetString("old_bns_name").Trim();
                                    m_bIsRenPUT = true;
                                }
                            }
                            result.Close();

                            result.Query = "select * from permit_update_appl where bin = :1 and appl_type = 'CTYP' and data_mode = 'QUE'";
                            result.AddParameter(":1", sBin.Trim());
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    result.Close();
                                    result.Query = "select new_bns_code from change_class_tbl where bin = :1 and is_main = 'Y'";
                                    result.AddParameter(":1", sBin.Trim());
                                    if (result.Read())
                                    {
                                        txtBnsCode.Text = result.GetString("new_bns_code").Trim();
                                        m_bIsRenPUT = true;
                                    }
                                }
                            }
                            result.Close();

                            


                            string sBnsStat = string.Empty;

                            result.Query = "select bns_stat from businesses where bin = :1";
                            result.AddParameter(":1", sBin.Trim());
                            if (result.Execute())
                                if (result.Read())
                                    sBnsStat = result.GetString("bns_stat").Trim();
                            result.Close();

                            if (sBnsStat == "NEW")
                            {
                                result.Query = "select * from taxdues where bin = :1";
                                result.AddParameter(":1", sBin.Trim());
                                if (result.Execute())
                                {
                                    if (result.Read())
                                    {
                                        string sTempYear = string.Empty;
                                        sTempYear = result.GetString("tax_year");
                                        if (int.Parse(sTempYear) > int.Parse(txtTaxYear.Text.Trim()))
                                            sBnsStat = "REN";
                                    }
                                }
                                result.Close();
                            }

                            //m_sDtOperated = result2.GetDateTime("dt_operated").ToShortDateString();
                            sTaxYear = txtTaxYear.Text.Trim();
                        }
                        else
                        {
                            MessageBox.Show("ONLINE PAYMENT");
                            //dgvTaxFees.Rows.Clear();
                            return;
                        }

                    }
                    result.Close();

                    result.Query = "select * from partial_payer where bin = :1 and tax_year = :2";
                    result.AddParameter(":1", sBin.Trim());
                    result.AddParameter(":2", txtTaxYear.Text.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {

                            MessageBox.Show("CHECK PARTIAL CHECK BOX");
                            // partial = true;
                        }
                    }
                    result.Close();

                    m_sStatus = txtBnsStat.Text.Trim();
                    txtBnsAdd.Text = AppSettingsManager.GetBnsAdd(sBin.Trim());

                    result.Query = "select new_bns_loc from permit_update_appl where bin = :1 and appl_type = 'TLOC' and data_mode = 'QUE'";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            m_sPrevBnsLoc = txtBnsAdd.Text.Trim();
                            txtBnsAdd.Text = result.GetString("new_bns_loc").Trim();
                            result.Close();

                            result.Query = "select * from business_que where bin = :1";
                            result.AddParameter(":1", sBin.Trim());
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    m_bIsRenPUT = true;
                                }
                                else
                                    m_bIsRenPUT = false;
                            }
                        }
                    }
                    result.Close();

                    txtOwnName.Text = AppSettingsManager.GetBnsOwner(m_sOwnCode.Trim());
                    //txtBnsType.Text = AppSettingsManager.GetBnsDesc(AppSettingsManager.GetBnsCodeMain(m_sBIN.Trim()));

                    if (txtBnsStat.Text.Trim() == "RET")
                        result.Query = "select * from taxdues where bin = :1 and due_state = 'X' order by tax_year";
                    else
                        result.Query = "select * from taxdues where bin = :1 order by tax_year";

                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {

                        }
                        else
                        {
                            result.Close();
                            result.Query = "select * from compromise_tbl where bin = :1";
                            result.AddParameter(":1", sBin.Trim());
                            if (result.Execute())
                            {
                                if (result.Read())
                                    bCompromise = true;
                                else
                                {
                                    if (int.Parse(txtTaxYear.Text.Trim()) < AppSettingsManager.GetSystemDate().Year)
                                    {
                                        MessageBox.Show("Unrenewed Business or not yet billed.");
                                        return;
                                        //dgvTaxFees.Rows.Clear();
                                    }
                                }
                            }


                        }
                    }
                    result.Close();

                    if (bCompromise == false)
                    {
                        if (txtBnsStat.Text == "RET")
                            result.Query = "select * from taxdues where bin = :1 and due_state = 'X' order by tax_year desc";
                        else
                            result.Query = "select * from taxdues where bin = :1 order by tax_year desc";
                        result.AddParameter(":1", sBin.Trim());
                        if (result.Execute())
                        {
                            if (result.Read())
                                txtTaxYear.Text = result.GetString("tax_year").Trim();
                        }
                        result.Close();

                    }

                    result.Query = "delete from pay_temp where bin = :1";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                    result.Close();

                    result.Query = "delete from pay_temp_bns where bin = :1";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                    result.Close();

                    //dgvTaxFees.Rows.Clear();

                    result.Query = "select * from compromise_due where bin = :1";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            m_bIsCompromise = true;
                            m_sCompPaySw = result.GetString("pay_sw").Trim();
                        }
                        else
                            m_bIsCompromise = false;
                    }
                    result.Close();

                }
            }
            result.Close();
            ComputeTaxAndFees(sBin.Trim());

            result.Query = "select * from waive_tbl where bin = :1";
            result.AddParameter(":1", sBin.Trim());
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

        private void ComputeTaxAndFees(string sBin)
        {
            m_sRevYear = AppSettingsManager.GetConfigObject("07");
            m_sIsQtrly = "N";
            DateTime dtSystemDate = DateTime.Now;
            DateTime dtDateOperated = DateTime.Now;
            string sQtrToPay = string.Empty;
            string sCurrentYear, sToday, sDateOperated = string.Empty, sYearOperated = string.Empty;
            dtSystemDate = AppSettingsManager.GetSystemDate();
            sToday = string.Format("{0:MM/dd/yyyy}", dtSystemDate);
            sCurrentYear = ConfigurationAttributes.CurrentYear;

            if (txtBnsStat.Text.Trim() == "REN")
                m_sIsQtrly = "Y";

            int iMultiYear = 0;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            result.Query = "select count(distinct tax_year) as iCount from taxdues where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    iMultiYear = result.GetInt("iCount");
                }
            }
            result.Close();



            if (iMultiYear > 1)
                txtTaxYear.ReadOnly = true;
            else
                txtTaxYear.ReadOnly = false;

            // insert permit update transaction here

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

            // try
            /*
            if (txtBnsStat.Text.Trim() == "RET")
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'X' order by tax_year asc";
            else
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' order by tax_year DESC";
            if (result.Execute())
            {
                if (result.Read())
                {
                    txtTaxYear.Text = result.GetString("tax_year").Trim();
                }
            }
            result.Close();
             */
            //try

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


            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "compute_tax_and_fees";
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
            plsqlCmd.ParamValue = iMultiYear;
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
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();

            }
            plsqlCmd.Close();
            m_sQtr1 = "F";
            m_sTerm = "F";


            // for displaying dues
            if (txtBnsStat.Text.Trim() == "RET")
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'X' and tax_year <= '" + txtTaxYear.Text.Trim() + "'";
            else
                result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state <> 'P' and tax_year <= '" + txtTaxYear.Text.Trim() + "' order by tax_year desc ,qtr_to_pay desc ,due_state desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sQtrToPay = result.GetString("qtr_to_pay").Trim();
                    if (sQtrToPay != "1" && iMultiYear != 1)
                    {
                        m_sNextQtrToPay = sQtrToPay;
                        OnInstallment(sBin);
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
                                    OnInstallment(sBin);
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
                            
                            DisplayDues(sBin, "F", "", "", "");
                        }

                    if (iMultiYear != 1 && int.Parse(txtTaxYear.Text.Trim()) <= AppSettingsManager.GetCurrentDate().Year)
                    {
                        m_sNextQtrToPay = sQtrToPay;
                        chk1st.Checked = false;
                        chk2nd.Checked = false;
                        chk3rd.Checked = false;
                        chk4th.Checked = false;
                        chkFull.Checked = true;
                        chkInstallment.Checked = false;
                        
                        DisplayDues(sBin, "F", "", "", "");
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
                           
                            DisplayDues(sBin, "F", "", "", "");
                            m_sPaymentTerm = "F";
                        }
                        else
                        {
                            if (txtBnsStat.Text != "NEW" && sQtrToPay != "1")
                            {
                                m_sNextQtrToPay = sQtrToPay;
                                chkFull.Checked = false;
                                chkInstallment.Checked = true;
                                OnInstallment(sBin);
                            }
                            else
                            {
                                chk1st.Checked = false;
                                chk2nd.Checked = false;
                                chk3rd.Checked = false;
                                chk4th.Checked = false;
                                chkFull.Checked = true;
                                chkInstallment.Checked = false;
                                OnFull(sBin); // GDE 20110412 added for testing
                            }

                        }

                    }

                }
                // GDE 20110414 added for testing (s){
                else
                {
                    result.Close();
                    result.Query = "select * from taxdues where bin = '" + sBin.Trim() + "' and due_state = 'P' and tax_year = '" + txtTaxYear.Text.Trim() + "' order by tax_year desc ,qtr_to_pay desc ,due_state desc";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sQtrToPay = result.GetString("qtr_to_pay").Trim();
                            if (sQtrToPay != "1" && iMultiYear != 1)
                            {
                                m_sNextQtrToPay = sQtrToPay;
                                OnInstallment(sBin);
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
                                            OnInstallment(sBin);
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
                                    
                                    DisplayDues(sBin, "F", "", "", "");
                                }

                            if (iMultiYear != 1 && int.Parse(txtTaxYear.Text.Trim()) <= AppSettingsManager.GetCurrentDate().Year)
                            {
                                m_sNextQtrToPay = sQtrToPay;
                                chk1st.Checked = false;
                                chk2nd.Checked = false;
                                chk3rd.Checked = false;
                                chk4th.Checked = false;
                                chkFull.Checked = true;
                                chkInstallment.Checked = false;
                                
                                DisplayDues(sBin, "F", "", "", "");
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
                                    
                                    DisplayDues(sBin, "F", "", "", "");
                                    m_sPaymentTerm = "F";
                                }
                                else
                                {
                                    if (txtBnsStat.Text != "NEW" && sQtrToPay != "1")
                                    {
                                        m_sNextQtrToPay = sQtrToPay;
                                        chkFull.Checked = false;
                                        chkInstallment.Checked = true;
                                        OnInstallment(sBin);
                                    }
                                    else
                                    {
                                        chk1st.Checked = false;
                                        chk2nd.Checked = false;
                                        chk3rd.Checked = false;
                                        chk4th.Checked = false;
                                        chkFull.Checked = true;
                                        chkInstallment.Checked = false;
                                        OnFull(sBin); // GDE 20110412 added for testing
                                    }

                                }

                            }

                        }
                    }
                }
                // GDE 20110414 added for testing (e)}
            }
            result.Close();
        }

        private void OnInstallment(string sBin)
        {
            chkFull.Checked = false;
            chkInstallment.Checked = true;
            m_sPaymentTerm = "I";
            if (m_sNextQtrToPay.Trim() == string.Empty)
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

            DisplayDues(sBin, m_sNextQtrToPay, "", "", "");

        }

        private void OnFull(string sBin)
        {
            chkFull.Checked = true;
            chkInstallment.Checked = false;
            m_sPaymentTerm = "F";
            string sTempTaxCredit = string.Empty;
            dgvParticulars.Rows.Clear();

            //sTempTaxCredit = txtToTaxCredit.Text.Trim();

            if (m_bIsCompromise)
            {
                m_bIsDisCompromise = true;
                //txtCompromise.ReadOnly = true;
            }
            else
            {
                m_bIsDisCompromise = false;
                //txtCompromise.ReadOnly = false;
            }
            m_sNextQtrToPay = "1";
            m_bExistTaxPU = false;
            //if (m_sPaymentMode != "ONL" && m_sPaymentMode != "OFL")
            QtrSelect frmQtr = new QtrSelect();
            frmQtr.strBin = sBin.Trim();
            frmQtr.ShowDialog();
            DisplayDues(sBin.Trim(), "F", "", "", "");

            /* // add this code in the later part of migration
             * 	// CTS 11102004 add compromise
	            //mc_eNoCompromise.EnableWindow(0); // ALJ 07312006 PUT "//" -- display/not to display compromise switch 
	            //OnKillfocusNumCompromise();
	            // CTS 11102004 add compromise
	            // CTS 12142004 fix bugs on auto check of cash
	            mc_bCash.SetCheck(FALSE);
	            mc_bCheck.SetCheck(FALSE);
	            // CTS 12142004 fix bugs on auto check of cash

	            m_bDiscQtr = false;	// RTL 10182005 discount in installment

	            //(s) RTL 11302005 for installment in NEW
	            if (m_sNewWithQtr == "Y" && m_sNewWithQtr != "4")
		            GetDlgItem(IDC_INSTALLMENT)->EnableWindow(1);
	            //(e) RTL 11302005 for installment in NEW

             */
        }

        private void DisplayDues(string sBin, string sQtrToPay, string sQtrToPay2, string sQtrToPay3, string sQtrToPay4)
        {


            dgvParticulars.Rows.Clear();
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

                    // GDE 20101103 for decimal places (e)}

                    dgvParticulars.Rows.Add(sBin, p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);


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
                        // GDE 20101103 for decimal places (e)}

                        dgvParticulars.Rows.Add(sBin, p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);
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

                                // GDE 20101103 for decimal places (e)}

                                dgvParticulars.Rows.Add(sBin, p_tax_year, p_term, p_qtr, p_fees_desc, p_fees_due, p_fees_surch, p_fees_pen, p_fees_totaldue, p_due_state, p_qtr_to_pay, p_fees_code);
                            }
                        }
                        result2.Close();

                    }
                }
            }
            result.Close();

            //txtTotDue.Text = string.Format("{0:#,##0.00}", m_dTotalDue1);
            //txtTotSurch.Text = string.Format("{0:#,##0.00}", m_dTotalSurcharge1);
            //txtTotPen.Text = string.Format("{0:#,##0.00}", m_dTotalPenalty1);
            //txtTotTotDue.Text = string.Format("{0:#,##0.00}", m_dTotalTotalDue1);

            // controlling control codes here

            // for sub total and total grid
            double dTotDue = 0;
            double dTotSurch = 0;
            double dTotPen = 0;
            double dTotTotal = 0;

            dgvSubTotals.Rows.Clear();
            for(int i = 0; i < dgvParticulars.RowCount; i++)
            {
                dTotDue = dTotDue + double.Parse(dgvParticulars.Rows[i].Cells[5].Value.ToString());
                dTotSurch = dTotSurch + double.Parse(dgvParticulars.Rows[i].Cells[6].Value.ToString());
                dTotPen = dTotPen + double.Parse(dgvParticulars.Rows[i].Cells[7].Value.ToString());
                dTotTotal = dTotTotal + double.Parse(dgvParticulars.Rows[i].Cells[8].Value.ToString());                               
            }
            dgvSubTotals.Rows.Add("TOTAL", string.Format("{0:#,##0.#0}", dTotDue), string.Format("{0:#,##0.#0}", dTotSurch), string.Format("{0:#,##0.#0}", dTotPen), string.Format("{0:#,##0.#0}", dTotTotal));

            // for sub total and total grid

        }

        private void frmPayments_Load(object sender, EventArgs e)
        {
            bin1.txtLGUCode.Text = AppSettingsManager.GetConfigObject("10");
            bin1.txtDistCode.Text = AppSettingsManager.GetConfigObject("11");
            
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
                    //if (chkRetirement.Checked == true || chkAdj.Checked == true)
                    //{
                    //    chkFull.Checked = true;
                    //    chkInstallment.Checked = false;
                   //}
                    //else
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

            dgvParticulars.Rows.Clear();
            DisplayDues(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);
            /*
            if (chkAdj.Checked == true)
                OnCheckAdj();
            if (chkRetirement.Checked == true)
                OnCheckRetirement();
             */

            // for sub total and total grid
            double dTotDue = 0;
            double dTotSurch = 0;
            double dTotPen = 0;
            double dTotTotal = 0;

            dgvSubTotals.Rows.Clear();
            for (int i = 0; i < dgvParticulars.RowCount; i++)
            {
                dTotDue = dTotDue + double.Parse(dgvParticulars.Rows[i].Cells[5].Value.ToString());
                dTotSurch = dTotSurch + double.Parse(dgvParticulars.Rows[i].Cells[6].Value.ToString());
                dTotPen = dTotPen + double.Parse(dgvParticulars.Rows[i].Cells[7].Value.ToString());
                dTotTotal = dTotTotal + double.Parse(dgvParticulars.Rows[i].Cells[8].Value.ToString());
            }
            dgvSubTotals.Rows.Add("TOTAL", string.Format("{0:#,##0.#0}", dTotDue), string.Format("{0:#,##0.#0}", dTotSurch), string.Format("{0:#,##0.#0}", dTotPen), string.Format("{0:#,##0.#0}", dTotTotal));

            // for sub total and total grid
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
                    //if (chkRetirement.Checked == true || chkAdj.Checked == true)
                    //{
                    //    chkFull.Checked = true;
                    //    chkInstallment.Checked = false;
                    //}
                   // else
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

            dgvParticulars.Rows.Clear();
            DisplayDues(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            /*
            if (chkAdj.Checked == true)
                OnCheckAdj();
            if (chkRetirement.Checked == true)
                OnCheckRetirement();
             */

            // for sub total and total grid
            double dTotDue = 0;
            double dTotSurch = 0;
            double dTotPen = 0;
            double dTotTotal = 0;

            dgvSubTotals.Rows.Clear();
            for (int i = 0; i < dgvParticulars.RowCount; i++)
            {
                dTotDue = dTotDue + double.Parse(dgvParticulars.Rows[i].Cells[5].Value.ToString());
                dTotSurch = dTotSurch + double.Parse(dgvParticulars.Rows[i].Cells[6].Value.ToString());
                dTotPen = dTotPen + double.Parse(dgvParticulars.Rows[i].Cells[7].Value.ToString());
                dTotTotal = dTotTotal + double.Parse(dgvParticulars.Rows[i].Cells[8].Value.ToString());
            }
            dgvSubTotals.Rows.Add("TOTAL", string.Format("{0:#,##0.#0}", dTotDue), string.Format("{0:#,##0.#0}", dTotSurch), string.Format("{0:#,##0.#0}", dTotPen), string.Format("{0:#,##0.#0}", dTotTotal));

            // for sub total and total grid
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
                    //if (chkAdj.Checked == true || chkRetirement.Checked == true)
                    //{
                    //    chkFull.Checked = true;
                    //    chkInstallment.Checked = false;
                    //}
                   // else
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

            dgvParticulars.Rows.Clear();
            DisplayDues(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            /*
            if (chkAdj.Checked == true)
                OnCheckAdj();
            if (chkRetirement.Checked == true)
                OnCheckRetirement();
             */

            // for sub total and total grid
            double dTotDue = 0;
            double dTotSurch = 0;
            double dTotPen = 0;
            double dTotTotal = 0;

            dgvSubTotals.Rows.Clear();
            for (int i = 0; i < dgvParticulars.RowCount; i++)
            {
                dTotDue = dTotDue + double.Parse(dgvParticulars.Rows[i].Cells[5].Value.ToString());
                dTotSurch = dTotSurch + double.Parse(dgvParticulars.Rows[i].Cells[6].Value.ToString());
                dTotPen = dTotPen + double.Parse(dgvParticulars.Rows[i].Cells[7].Value.ToString());
                dTotTotal = dTotTotal + double.Parse(dgvParticulars.Rows[i].Cells[8].Value.ToString());
            }

            dgvSubTotals.Rows.Add("TOTAL", string.Format("{0:#,##0.#0}", dTotDue), string.Format("{0:#,##0.#0}", dTotSurch), string.Format("{0:#,##0.#0}", dTotPen), string.Format("{0:#,##0.#0}", dTotTotal));

            // for sub total and total grid
        }

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
                //if (chkRetirement.Checked == true || chkAdj.Checked == true)
                //{
                //    chkFull.Checked = true;
                //    chkInstallment.Checked = false;
                //}
                //else
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

            dgvParticulars.Rows.Clear();
            DisplayDues(m_sBIN, sQToBePaid1, sQToBePaid2, sQToBePaid3, sQToBePaid4);

            /*
            if (chkAdj.Checked == true)
                OnCheckAdj();
            if (chkRetirement.Checked == true)
                OnCheckRetirement();
             */

            // for sub total and total grid
            double dTotDue = 0;
            double dTotSurch = 0;
            double dTotPen = 0;
            double dTotTotal = 0;

            dgvSubTotals.Rows.Clear();
            for (int i = 0; i < dgvParticulars.RowCount; i++)
            {
                dTotDue = dTotDue + double.Parse(dgvParticulars.Rows[i].Cells[5].Value.ToString());
                dTotSurch = dTotSurch + double.Parse(dgvParticulars.Rows[i].Cells[6].Value.ToString());
                dTotPen = dTotPen + double.Parse(dgvParticulars.Rows[i].Cells[7].Value.ToString());
                dTotTotal = dTotTotal + double.Parse(dgvParticulars.Rows[i].Cells[8].Value.ToString());
            }
            dgvSubTotals.Rows.Add("TOTAL", string.Format("{0:#,##0.#0}", dTotDue), string.Format("{0:#,##0.#0}", dTotSurch), string.Format("{0:#,##0.#0}", dTotPen), string.Format("{0:#,##0.#0}", dTotTotal));

            // for sub total and total grid
        }

        private void chk2nd_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chk3rd_CheckedChanged(object sender, EventArgs e)
        {

        }


    }
}