using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.SearchBusiness;
using Amellar.Common.ImageViewer;
using Amellar.Common.DeficientRecords;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Common.TransactionLog;

namespace Posting
{
    public partial class frmPosting : Form
    {
        string m_strBIN = string.Empty;
        string m_strOwnCode = string.Empty;
        string m_strDateOperated = string.Empty;
        public string m_sPostFormState = string.Empty;
        public string m_sSwTrans = string.Empty;
        string m_strLastQtrPaid = string.Empty;
        string m_sIsQtrly = string.Empty;
        string m_sNextQtrToPay = string.Empty;
        string m_sPaymentTerm = string.Empty;
        string m_sQtrPaid = string.Empty;
        string m_sNoOfQtr = string.Empty;
        string m_sModule = string.Empty;
        string m_sTempGenOR = string.Empty;
        string m_sMode = string.Empty;
        string m_sPaymentType = string.Empty;
        string m_sFile = string.Empty;
        string m_sCurrentTaxYear = string.Empty;
        frmDeficientRecords frmDeficientRecords = new frmDeficientRecords();
        private DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin

        struct PaytempStruct
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
        }
        bool b1st = false;
        bool b2nd = false;
        bool b3rd = false;
        bool b4th = false;
        bool m_bTaskManager = false;
        // blob
        protected frmImageList m_frmImageList;
        public static int m_intImageListInstance;
        // blob

        OracleResultSet pSetTaxYear = new OracleResultSet();
        OracleResultSet pSetCellValue = new OracleResultSet();

        public frmPosting()
        {
            InitializeComponent();

            // blob
            m_intImageListInstance = 0;
            m_frmImageList = new frmImageList();
            m_frmImageList.IsBuildUpPosting = true;
            // blob
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (m_sPostFormState == "ADD" || m_sPostFormState == "EDIT")
            {
                if (MessageBox.Show("Cancel current transaction?", "BPLS - Posting", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    AppSettingsManager.TaskManager(m_strBIN, m_sModule, "DELETE");
                    this.Dispose();
                }
            }
            else
                this.Dispose();
        }

        private void btnSearchBin_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            bin1.txtLGUCode.Text = AppSettingsManager.GetConfigObject("10");
            bin1.txtDistCode.Text = AppSettingsManager.GetConfigObject("11");

            if (btnSearchBin.Text == "Search &BIN")
            {
                btnSearchBin.Text = "Clear &BIN";
                m_strBIN = StringUtilities.HandleApostrophe(bin1.GetBin());
                if (bin1.txtTaxYear.Text != "" && bin1.txtBINSeries.Text != "")
                    OnlineSearch(m_strBIN);
                else
                {
                    frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                        m_strBIN = StringUtilities.HandleApostrophe(bin1.GetBin());
                        OnlineSearch(m_strBIN);
                    }
                    else
                    {
                        bin1.txtTaxYear.Text = "";
                        bin1.txtBINSeries.Text = "";
                        btnSearchBin.Text = "Search &BIN";
                        btnSave.Enabled = false;
                    }
                }
            }
            else
            {
                btnSave.Enabled = false;
                m_sNextQtrToPay = string.Empty;
                m_sQtrPaid = string.Empty;
                btnSearchBin.Text = "Search &BIN";
                cmbOrNo.Items.Clear();
                cmbOrNo.Text = "";
                txtMPNo.Text = "";
                dgvDues.Rows.Clear();
                bin1.txtTaxYear.Text = "";
                bin1.txtBINSeries.Text = "";
                bin1.txtTaxYear.Focus();

                bool bX = TaskMan.IsObjectLock(m_strBIN, m_sModule, "DELETE", "");    // RMC 20150430 added end-tasking in posting when bin was cleared

                m_sPaymentTerm = "F";
                m_sPaymentType = "CS";
                m_sMode = "POS";
                btnFull.Enabled = true;
                btnInstallment.Enabled = true;

                Control GB = null;
                foreach (Control g in this.Controls)
                {
                    if (g is GroupBox)
                    {
                        GB = g;
                        foreach (Control c in GB.Controls)
                        {
                            if (c is TextBox)
                                ((TextBox)c).Text = "";
                            else if (c is CheckBox)
                            {
                                ((CheckBox)c).Checked = false;
                                ((CheckBox)c).Enabled = false;
                            }
                        }
                    }
                }
            }
        }

        private void OnlineSearch(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            //REM MCR 20150129 (s)
            //result.Query = string.Format("select distinct * from pay_hist where bin = '{0}' order by  tax_year desc, qtr_paid desc", sBin);
            //if (result.Execute())
            //{
            //    if (result.Read())
            //    {
            //        if (result.GetString("data_mode") == "ONL" || result.GetString("data_mode") == "OFL")
            //        {
            //            MessageBox.Show("Cannot proceed, ONLINE/OFFLINE transanction is already made", "BPLS - Posting", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            result.Close();
            //            btnSave.Enabled = false;
            //            return;
            //        }
            //    }
            //}
            //result.Close();
            //REM MCR 20150129 (s)

            result.Query = string.Format("select * from businesses where bin = '{0}'", sBin);
            if (result.Execute())
            {
                if (result.Read())
                {
                    String tmpBIN;
                    tmpBIN = sBin;

                    if (m_bTaskManager)
                        sBin = "";

                    if (!AppSettingsManager.TaskManager(sBin, m_sModule, ""))
                    {
                        sBin = tmpBIN;
                        //string sTmpBin = string.Empty;
                        //sTmpBin = sBin;
                        m_strOwnCode = result.GetString("own_code").Trim();
                        txtBnsName.Text = result.GetString("bns_nm").Trim();
                        txtBnsCode.Text = result.GetString("bns_code").Trim();
                        txtStat.Text = result.GetString("bns_stat").Trim();
                        m_strDateOperated = result.GetDateTime("dt_operated").ToShortDateString();
                        txtMPNo.Text = result.GetString("permit_no").Trim();
                        txtBnsType.Text = AppSettingsManager.GetBnsDesc(txtBnsCode.Text.Trim());
                        txtTaxYear.Text = result.GetString("tax_year").Trim();
                        txtBnsAdd.Text = AppSettingsManager.GetBnsAdd(sBin.Trim(), "").Trim();
                        txtBnsOwner.Text = AppSettingsManager.GetBnsOwner(m_strOwnCode.Trim()).Trim();

                        // ALJ 05202003 (s) validation upon delete of business if has an applicastion/billing
                        if (m_sPostFormState == "DELETE")
                        {
                            result2.Query = string.Format(@"select distinct (bin) from taxdues where bin = '{0}' and tax_year <> '{1}'", sBin, txtTaxYear.Text.Trim());
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    sBin = "";
                                    MessageBox.Show("Cannot delete payment w/ existing application/billing", "BPLS - Posting", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    result2.Close();
                                    result.Close();
                                    return;
                                }
                            }
                            result2.Close();

                            if (txtStat.Text.Trim() == "NEW")
                            {
                                result2.Query = string.Format("select distinct (bin) from taxdues where bin = '{0}'", sBin); // CTS 09152003
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        sBin = "";
                                        MessageBox.Show("Cannot delete payment w/ existing application/billing", "BPLS - Posting", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        result2.Close();
                                        result.Close();
                                        return;
                                    }
                                }
                                result2.Close();
                            }
                        }
                        // ALJ 01022007 (s) validate NEW with existing Billing
                        if (m_sPostFormState == "ADD" || m_sPostFormState == "EDIT")
                        {
                            if (txtStat.Text.Trim() == "NEW")
                            {
                                result2.Query = string.Format("select distinct (bin) from taxdues where bin = '{0}'", sBin); // CTS 09152003
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        sBin = "";
                                        MessageBox.Show("Cannot add/edit payment w/ existing application/billing", "BPLS - Posting", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        result2.Close();
                                        result.Close();
                                        return;
                                    }
                                }
                                result2.Close();
                            }
                        }
                    }
                    else
                        sBin = "";

                    m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                }
                else
                {
                    sBin = "";
                    btnSearchBin.PerformClick();
                    return;
                }
            }
            result.Close();

            if (sBin.Trim() != "")
            {
                result.Query = string.Format("select * from partial_payer where bin = '{0}' and tax_year = '{1}'", sBin, txtTaxYear.Text.Trim()); // CTS 09152003
                if (result.Execute())
                    if (result.Read())
                        chkPartial.Checked = true;
                result.Close();

                btnDefTools.Enabled = true;

                result2.Query = "delete from pay_temp where bin = '" + sBin.Trim() + "'";
                if (result2.ExecuteNonQuery() != 0)
                {

                }
                result2.Close();
                dgvDues.Rows.Clear();

                if (m_sPostFormState == "ADD")
                {
                    m_bTaskManager = true;
                    PostSetup();
                }
                if (m_sPostFormState == "EDIT" || m_sPostFormState == "DELETE")
                {
                    m_bTaskManager = true;
                    PopulateORCombo();
                }

                ////Making the controls for debit_credit, visible or not (s)
                txtCreditLeft.Text = "0.00";
                txtDebitCredit.Text = "0.00";
                if (m_strOwnCode != "")
                {
                    //result.Query = string.Format("select * from dbcr_memo where bin = '{0}' and served = 'N'", sBin);
                    //result.Query = string.Format("select * from dbcr_memo where own_code = '{0}' and served = 'N'", m_strOwnCode); //JAV 20170712  change bin to own_code
                    result.Query = string.Format("select * from dbcr_memo where bin = '{0}' and served = 'N'", sBin); //JARS 20171010
                    if (result.Execute())
                        if (result.Read())
                        {
                            txtDebitCredit.Text = result.GetDouble("balance").ToString();
                            txtCreditLeft.Text = result.GetDouble("balance").ToString();
                        }
                    result.Close();
                }
                //Making the controls for debit_credit, visible or not (e)

                if (m_sPaymentTerm == "I" && !chk1st.Checked)
                    chkPartial.Enabled = false;
                else
                    chkPartial.Enabled = false;

                if (m_sPostFormState == "ADD")
                    txtOrNo.Focus();
                else
                    cmbOrNo.Focus();

                //(s)//LEO 02132003
                result.Query = string.Format("select distinct * from pay_hist where bin = '{0}' order by tax_year desc", sBin);
                txtMemo.Text = "";
                if (result.Execute())
                    if (result.Read())
                        txtMemo.Text = result.GetString("memo");
                result.Close();
                //(e)//LEO 02132003
            }
            // partial payer validations here

            if (m_sPostFormState == "ADD")
            {
                btnView.Enabled = true;
                btnSetAside.Enabled = false;
                btnAttach.Enabled = false;
            }
            if (m_sPostFormState == "EDIT" || m_sPostFormState == "DELETE")
            {
                btnView.Enabled = true;
                btnSetAside.Enabled = false;
            }
        }

        private void PopulateORCombo()
        {
            OracleResultSet pSet = new OracleResultSet();
            cmbOrNo.Items.Clear();
            pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode <> 'UNP' order by qtr_paid,or_date", bin1.GetBin(), txtTaxYear.Text.Trim());
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbOrNo.Items.Add(pSet.GetString("or_no"));
                }
            }

            // CompLessCredit(mv_sTotalTotalDue, mv_sCreditLeft, mv_sToBeCredited, &mv_sGrandTotalDue, &mv_sCreditLeft);
        }

        private void PostSetup()
        {
            DateTime dtPosted = DateTime.Parse(dtpOrDate.Text);
            DateTime dtTimePosted = DateTime.Parse(dtpOrDate.Text).ToLocalTime();
            string strCurrentYear = AppSettingsManager.GetSystemDate().Year.ToString();

            string sPaymentTerm = string.Empty;
            string sQtrPaid = string.Empty;
            string sQtrNo = string.Empty;

            OracleResultSet result = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();

            result.Query = "select distinct * from pay_hist where bin = '" + m_strBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "' and data_mode <> 'UNP' order by qtr_paid desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sPaymentTerm = result.GetString("payment_term").Trim();
                    sQtrPaid = result.GetString("qtr_paid").Trim();
                    sQtrNo = result.GetString("no_of_qtr").Trim();
                    m_strLastQtrPaid = sQtrPaid;

                    if (m_sIsQtrly == "Y")
                    {
                        if (txtStat.Text.Trim() != "NEW") // ALJ 07312003 consider RET in validation
                        {
                            if (sPaymentTerm == "F")
                            {
                                MessageBox.Show("Payment for the year considered complete", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                result.Close();
                                btnSave.Enabled = false;
                                result.Close();
                                return;
                            }
                            else
                            {
                                if (sQtrPaid == "4")
                                {
                                    MessageBox.Show("Payment for the year considered complete", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    result.Close();
                                    btnSave.Enabled = false;
                                    result.Close();
                                    return;
                                }
                                else
                                {
                                    int iQtrPaid = 0;
                                    int.TryParse(sQtrPaid, out iQtrPaid);
                                    iQtrPaid = iQtrPaid + 1;
                                    m_sNextQtrToPay = iQtrPaid.ToString().Trim();
                                    btnFull.Enabled = false;
                                    OnInstallment();
                                }
                            }
                        }
                        else
                        {
                            if (sQtrPaid == "4")
                            {
                                MessageBox.Show("Payment for the year considered complete", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                result.Close();
                                btnSave.Enabled = false;
                                result.Close();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (sPaymentTerm == "F")
                        {
                            //MCR 20150127 (s)
                            //if (AppSettingsManager.GetConfigValue("10") == "019")
                            //if(AppSettingsManager.GetConfigValue("68") == "Y")  // RMC 20170306 added config for posting of previous years payments 
                            if (AppSettingsManager.GetConfigValue("73") == "Y")  // AFM 20200124 changed config code
                            {
                                if (MessageBox.Show("Payment for the year considered complete, \ncontinue posting previous years?", "BPLS - Posting", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    txtTaxYear.Enabled = true;
                                    result1.Query = "select min(tax_year)-1 as tax_year from pay_hist where bin = '" + m_strBIN.Trim() + "'";
                                    if (result1.Execute())
                                    {
                                        if (result1.Read())
                                        {
                                            txtTaxYear.Text = result1.GetInt("tax_year").ToString();
                                            txtTaxYear.Focus();
                                        }
                                    }
                                    result1.Close();
                                }
                                else
                                { return; }
                                //MCR 20150127 (e)
                            }
                            else
                            {
                                MessageBox.Show("Payment for the year considered complete", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                btnSave.Enabled = false;
                                result.Close();
                                return;
                            }
                        }
                        else
                        {
                            if (sQtrPaid == "4")
                            {
                                //MCR 20150127 (s)
                                //if (AppSettingsManager.GetConfigValue("10") == "019")
                                if (AppSettingsManager.GetConfigValue("68") == "Y")  // RMC 20170306 added config for posting of previous years payments 
                                {
                                    if (MessageBox.Show("Payment for the year considered complete, \ncontinue posting previous years?", "BPLS - Posting", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        txtTaxYear.Enabled = true;
                                        result1.Query = "select min(tax_year)-1 as tax_year from pay_hist where bin = '" + m_strBIN.Trim() + "'";
                                        if (result1.Execute())
                                        {
                                            if (result1.Read())
                                            {
                                                txtTaxYear.Text = result1.GetInt("tax_year").ToString();
                                                txtTaxYear.Focus();
                                            }
                                        }
                                        result1.Close();
                                    }
                                    else
                                    { return; }
                                    //MCR 20150127 (e)
                                }
                                else
                                {
                                    MessageBox.Show("Payment for the year considered complete", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    btnSave.Enabled = false;
                                    result.Close();
                                    return;
                                }
                            }
                            else
                            {
                                //MCR 20150127 (s)
                                //if (AppSettingsManager.GetConfigValue("10") == "019")
                                if (AppSettingsManager.GetConfigValue("68") == "Y")  // RMC 20170306 added config for posting of previous years payments 
                                {
                                    if (MessageBox.Show("Payment for the year considered complete, \ncontinue posting previous years?", "BPLS - Posting", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        txtTaxYear.Enabled = true;
                                        result1.Query = "select min(tax_year)-1 as tax_year from pay_hist where bin = '" + m_strBIN.Trim() + "'";
                                        if (result1.Execute())
                                        {
                                            if (result1.Read())
                                            {
                                                txtTaxYear.Text = result1.GetInt("tax_year").ToString();
                                                txtTaxYear.Focus();
                                            }
                                        }
                                        result1.Close();
                                    }
                                    else
                                    { return; }
                                }
                                //MCR 20150127 (e)

                                int iQtrPaid = 0;
                                int.TryParse(sQtrPaid, out iQtrPaid);
                                iQtrPaid = iQtrPaid + 1;
                                m_sNextQtrToPay = iQtrPaid.ToString().Trim();
                                btnFull.Enabled = false;
                                OnInstallment();
                            }
                        }
                    }
                }
                else
                    OnFull();

                DisplayTaxAndDues();
            }
            result.Close();
        }

        private void DisplayTaxAndDues()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            bool bWithQDec = false;
            string sStartQtr;

            string sQtrToPay = string.Empty;
            string sBin = string.Empty;
            string sDueState = string.Empty;
            string sFeesCode = string.Empty;
            string sFeesDesc = string.Empty;
            string sQtr = string.Empty;
            string sTerm = string.Empty;
            string sTaxYear = string.Empty;

            if (m_sIsQtrly == "Y")
                sStartQtr = AppSettingsManager.GetQtr(m_strDateOperated);
            else
                sStartQtr = "F";

            if (txtStat.Text.Trim() == "NEW")
            {
                btnInstallment.Enabled = false; //MCR 20141211
                if (m_sPostFormState == "EDIT")
                    dgvDues.Rows.Clear();
                if (m_sPostFormState == "DELETE")
                    dgvDues.Rows.Clear();
                //MCR 20150127 (s)
                if (m_sPostFormState == "ADD" && txtTaxYear.Enabled == true)
                {
                    result2.Query = "delete from pay_temp where bin = '" + m_strBIN.Trim() + "'";
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();
                    dgvDues.Rows.Clear();
                }
                //MCR 20150127 (e)

                sBin = m_strBIN;
                sDueState = "N";
                sFeesCode = txtBnsCode.Text.Trim();
                sFeesDesc = "TAX ON " + AppSettingsManager.GetBnsDesc(sFeesCode.Trim());
                sFeesCode = "B" + sFeesCode;
                sQtr = sStartQtr;

                if (m_sPaymentTerm == "F")
                {
                    sQtrToPay = "F";
                    sTerm = "F";
                }
                else
                {
                    sDueState = "I"; //Q
                    sQtr = m_sQtrPaid;
                    sTerm = "I";//Q
                    sQtrToPay = m_sQtrPaid;
                }

                sTaxYear = txtTaxYear.Text.Trim();

                if (m_sPostFormState == "ADD")
                {
                    result.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, 0, 0, 0, 0, :6, :7, :8, ' ')";
                    result.AddParameter(":1", sBin.Trim());
                    result.AddParameter(":2", sTaxYear.Trim());
                    result.AddParameter(":3", sTerm.Trim());
                    result.AddParameter(":4", sQtr.Trim());
                    result.AddParameter(":5", sFeesDesc.Trim());
                    result.AddParameter(":6", sFeesCode.Trim());
                    result.AddParameter(":7", sDueState.Trim());
                    result.AddParameter(":8", sQtrToPay.Trim());
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                    result.Close();
                }
                else if (m_sPostFormState == "EDIT")
                {
                    result.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and fees_code = '" + sFeesCode.Trim() + "'";
                    if (result.Execute())
                    {
                        if (!result.Read()) //if not found
                        {
                            result2.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, '0', '0', '0', '0', :6, :7, :8, ' ')";
                            result2.AddParameter(":1", sBin.Trim());
                            result2.AddParameter(":2", sTaxYear.Trim());
                            result2.AddParameter(":3", sTerm.Trim());
                            result2.AddParameter(":4", sQtr.Trim());
                            result2.AddParameter(":5", sFeesDesc.Trim());
                            result2.AddParameter(":6", sFeesCode.Trim());
                            result2.AddParameter(":7", sDueState.Trim());
                            result2.AddParameter(":8", sQtrToPay.Trim());
                            if (result2.ExecuteNonQuery() != 0)
                            {

                            }
                            result2.Close();
                        }
                    }
                    result.Close();
                }

                // Addl Bns

                string sOtherBnsCode = string.Empty;
                string sOtherBnsDesc = string.Empty;

                result.Query = "select * from addl_bns where bin = '" + sBin.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "'";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        sOtherBnsCode = result.GetString("bns_code_main").Trim();
                        sOtherBnsDesc = "TAX ON " + AppSettingsManager.GetBnsDesc(sOtherBnsCode);
                        sOtherBnsCode = "B" + sOtherBnsCode;
                        if (m_sPostFormState == "ADD")
                        {
                            result.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, null, null, null, null, :6, :7, :8, ' ')";
                            result.AddParameter(":1", sBin.Trim());
                            result.AddParameter(":2", sTaxYear.Trim());
                            result.AddParameter(":3", sTerm.Trim());
                            result.AddParameter(":4", sQtr.Trim());
                            result.AddParameter(":5", sOtherBnsDesc.Trim());
                            result.AddParameter(":6", sOtherBnsCode.Trim());
                            result.AddParameter(":7", sDueState.Trim());
                            result.AddParameter(":8", sQtrToPay.Trim());
                            if (result.ExecuteNonQuery() != 0)
                            {

                            }
                        }
                        else if (m_sPostFormState == "EDIT")
                        {
                            result.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and fees_code = '" + sOtherBnsCode.Trim() + "'";
                            if (result.Execute())
                            {
                                if (!result.Read()) //if not found
                                {
                                    result2.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, '0', '0', '0', '0', :6, :7, :8, ' ')";
                                    result2.AddParameter(":1", sBin.Trim());
                                    result2.AddParameter(":2", sTaxYear.Trim());
                                    result2.AddParameter(":3", sTerm.Trim());
                                    result2.AddParameter(":4", sQtr.Trim());
                                    result2.AddParameter(":5", sOtherBnsDesc.Trim());
                                    result2.AddParameter(":6", sOtherBnsCode.Trim());
                                    result2.AddParameter(":7", sDueState.Trim());
                                    result2.AddParameter(":8", sQtrToPay.Trim());
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result2.Close();
                                }
                            }
                        }

                    }
                }
                result.Close();

                // end of addl bns

                OracleResultSet result3 = new OracleResultSet();
                //result.Query = "select * from tax_and_fees_table";
                result.Query = "select * from tax_and_fees_table where rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        sFeesCode = result.GetString("fees_code").Trim();
                        sFeesDesc = result.GetString("fees_desc").Trim();
                        if (m_sPostFormState == "ADD")
                        {
                            result2.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, 0, 0, 0, 0, :6, :7, :8, ' ')";
                            result2.AddParameter(":1", sBin.Trim());
                            result2.AddParameter(":2", sTaxYear.Trim());
                            result2.AddParameter(":3", sTerm.Trim());
                            result2.AddParameter(":4", sQtr.Trim());
                            result2.AddParameter(":5", sFeesDesc.Trim());
                            result2.AddParameter(":6", sFeesCode.Trim());
                            result2.AddParameter(":7", sDueState.Trim());
                            result2.AddParameter(":8", sQtrToPay.Trim());
                            if (result2.ExecuteNonQuery() != 0)
                            {

                            }
                            result2.Close();
                        }
                        else if (m_sPostFormState == "EDIT")
                        {
                            result3.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and fees_code = '" + sFeesCode.Trim() + "'";
                            if (result3.Execute())
                            {
                                if (!result3.Read()) //if not found
                                {
                                    result2.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, '0', '0', '0', '0', :6, :7, :8, ' ')";
                                    result2.AddParameter(":1", sBin.Trim());
                                    result2.AddParameter(":2", sTaxYear.Trim());
                                    result2.AddParameter(":3", sTerm.Trim());
                                    result2.AddParameter(":4", sQtr.Trim());
                                    result2.AddParameter(":5", sFeesDesc.Trim());
                                    result2.AddParameter(":6", sFeesCode.Trim());
                                    result2.AddParameter(":7", sDueState.Trim());
                                    result2.AddParameter(":8", sQtrToPay.Trim());
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result2.Close();
                                }
                            }
                            result3.Close();
                        }
                    }
                }
                result.Close();

                // for qtrly dec codes
            }
            else // REN
            {
                if (m_sPostFormState == "EDIT")
                    dgvDues.Rows.Clear();
                if (m_sPostFormState == "DELETE")
                    dgvDues.Rows.Clear();
                //MCR 20150127 (s)
                if (m_sPostFormState == "ADD" && txtTaxYear.Enabled == true)
                {
                    result2.Query = "delete from pay_temp where bin = '" + m_strBIN.Trim() + "'";
                    if (result2.ExecuteNonQuery() != 0)
                    {

                    }
                    result2.Close();
                    dgvDues.Rows.Clear();
                }
                //MCR 20150127 (e)

                sBin = m_strBIN;
                sDueState = "R";
                sFeesCode = txtBnsCode.Text.Trim();
                sFeesDesc = "TAX ON " + AppSettingsManager.GetBnsDesc(sFeesCode.Trim());
                sFeesCode = "B" + sFeesCode;
                sQtr = sStartQtr;

                if (m_sPaymentTerm == "F")
                {
                    sQtrToPay = "F";
                    sTerm = "F";
                }
                else
                {
                    if (m_sPostFormState == "ADD")
                    {
                        sQtrToPay = m_sNextQtrToPay;
                        sQtr = m_sNextQtrToPay;
                        sTerm = "I";
                    }
                    if (m_sPostFormState == "EDIT")
                    {
                        sQtrToPay = m_sNextQtrToPay;
                        sQtr = m_sQtrPaid;
                        sTerm = "I";
                    }

                }

                sTaxYear = txtTaxYear.Text.Trim();

                if (m_sPostFormState == "ADD")
                {
                    result.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, 0, 0, 0, 0, :6, :7, :8, ' ')";
                    result.AddParameter(":1", sBin.Trim());
                    result.AddParameter(":2", sTaxYear.Trim());
                    result.AddParameter(":3", sTerm.Trim());
                    result.AddParameter(":4", sQtr.Trim());
                    result.AddParameter(":5", sFeesDesc.Trim());
                    result.AddParameter(":6", sFeesCode.Trim());
                    result.AddParameter(":7", sDueState.Trim());
                    result.AddParameter(":8", sQtrToPay.Trim());
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                    result.Close();
                }
                else if (m_sPostFormState == "EDIT")
                {
                    result.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and fees_code = '" + sFeesCode.Trim() + "'";
                    if (result.Execute())
                    {
                        if (!result.Read()) //if not found
                        {
                            result2.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, '0', '0', '0', '0', :6, :7, :8, ' ')";
                            result2.AddParameter(":1", sBin.Trim());
                            result2.AddParameter(":2", sTaxYear.Trim());
                            result2.AddParameter(":3", sTerm.Trim());
                            result2.AddParameter(":4", sQtr.Trim());
                            result2.AddParameter(":5", sFeesDesc.Trim());
                            result2.AddParameter(":6", sFeesCode.Trim());
                            result2.AddParameter(":7", sDueState.Trim());
                            result2.AddParameter(":8", sQtrToPay.Trim());
                            if (result2.ExecuteNonQuery() != 0)
                            {

                            }
                            result2.Close();
                        }
                    }
                    result.Close();
                }

                // Addl Bns

                string sOtherBnsCode = string.Empty;
                string sOtherBnsDesc = string.Empty;

                result.Query = "select * from addl_bns where bin = '" + sBin.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "'";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        sOtherBnsCode = result.GetString("bns_code_main").Trim();
                        sOtherBnsDesc = "TAX ON " + AppSettingsManager.GetBnsDesc(sOtherBnsCode);
                        sOtherBnsCode = "B" + sOtherBnsCode;
                        if (m_sPostFormState == "ADD")
                        {
                            result.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, 0, 0, 0, 0, :6, :7, :8, ' ')";
                            result.AddParameter(":1", sBin.Trim());
                            result.AddParameter(":2", sTaxYear.Trim());
                            result.AddParameter(":3", sTerm.Trim());
                            result.AddParameter(":4", sQtr.Trim());
                            result.AddParameter(":5", sOtherBnsDesc.Trim());
                            result.AddParameter(":6", sOtherBnsCode.Trim());
                            result.AddParameter(":7", sDueState.Trim());
                            result.AddParameter(":8", sQtrToPay.Trim());
                            if (result.ExecuteNonQuery() != 0)
                            {

                            }
                        }
                        else if (m_sPostFormState == "EDIT")
                        {
                            result2.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and fees_code = '" + sOtherBnsCode.Trim() + "'";
                            if (result2.Execute())
                            {
                                if (!result2.Read()) //if not found
                                {
                                    result2.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, '0', '0', '0', '0', :6, :7, :8, ' ')";
                                    result2.AddParameter(":1", sBin.Trim());
                                    result2.AddParameter(":2", sTaxYear.Trim());
                                    result2.AddParameter(":3", sTerm.Trim());
                                    result2.AddParameter(":4", sQtr.Trim());
                                    result2.AddParameter(":5", sOtherBnsDesc.Trim());
                                    result2.AddParameter(":6", sOtherBnsCode.Trim());
                                    result2.AddParameter(":7", sDueState.Trim());
                                    result2.AddParameter(":8", sQtrToPay.Trim());
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }
                                }
                            }
                            result2.Close();
                        }
                    }
                }
                result.Close();

                // end of addl bns

                OracleResultSet result3 = new OracleResultSet();
                //result.Query = "select * from tax_and_fees_table";
                result.Query = "select * from tax_and_fees_table where rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        sFeesCode = result.GetString("fees_code").Trim();
                        sFeesDesc = result.GetString("fees_desc").Trim();
                        if (m_sPostFormState == "ADD")
                        {
                            result2.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, 0, 0, 0, 0, :6, :7, :8, ' ')";
                            result2.AddParameter(":1", sBin.Trim());
                            result2.AddParameter(":2", sTaxYear.Trim());
                            result2.AddParameter(":3", sTerm.Trim());
                            result2.AddParameter(":4", sQtr.Trim());
                            result2.AddParameter(":5", sFeesDesc.Trim());
                            result2.AddParameter(":6", sFeesCode.Trim());
                            result2.AddParameter(":7", sDueState.Trim());
                            result2.AddParameter(":8", sQtrToPay.Trim());
                            if (result2.ExecuteNonQuery() != 0)
                            {

                            }
                            result2.Close();
                        }
                        else if (m_sPostFormState == "EDIT")
                        {
                            result3.Query = "select * from pay_temp where bin = '" + sBin.Trim() + "' and fees_code = '" + sFeesCode.Trim() + "'";
                            if (result3.Execute())
                            {
                                if (!result3.Read()) //if not found
                                {
                                    result2.Query = "insert into pay_temp values (:1, :2, :3, :4, :5, '0', '0', '0', '0', :6, :7, :8, ' ')";
                                    result2.AddParameter(":1", sBin.Trim());
                                    result2.AddParameter(":2", sTaxYear.Trim());
                                    result2.AddParameter(":3", sTerm.Trim());
                                    result2.AddParameter(":4", sQtr.Trim());
                                    result2.AddParameter(":5", sFeesDesc.Trim());
                                    result2.AddParameter(":6", sFeesCode.Trim());
                                    result2.AddParameter(":7", sDueState.Trim());
                                    result2.AddParameter(":8", sQtrToPay.Trim());
                                    if (result2.ExecuteNonQuery() != 0)
                                    {

                                    }
                                    result2.Close();
                                }
                            }
                            result3.Close();
                        }
                    }
                }
                result.Close();
            }
            DisplayProper();
        }

        private void DisplayProper()
        {
            string sBin = string.Empty;
            string sTaxYear = string.Empty;
            string sQtr = string.Empty;
            string sTerm = string.Empty;
            string sFeesDesc = string.Empty;
            string sFeesDue = string.Empty;
            string sFeesSurch = string.Empty;
            string sFeesPen = string.Empty;
            string sFeesTotDue = string.Empty;
            string sFeesCode = string.Empty;
            string sDueState = string.Empty;
            string sQtrToPay = string.Empty;

            ////MOD MCR 20140410
            //if (txtStat.Text.Trim() == "NEW")
            //{
            //    btnFull.Enabled = false;
            //    btnInstallment.Enabled = false;
            //    //DeletePaidQtrNew(); 
            //}
            //else
            //{
            //    btnFull.Enabled = true;
            //    btnInstallment.Enabled = true;
            //}

            OracleResultSet result = new OracleResultSet();

            result.Query = "select * from pay_temp where bin = '" + m_strBIN.Trim() + "' and fees_code like 'B%'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sBin = result.GetString("bin").Trim();
                    sTaxYear = result.GetString("tax_year").Trim();
                    sQtr = result.GetString("qtr").Trim();
                    sTerm = result.GetString("term").Trim();
                    sFeesDesc = result.GetString("fees_desc").Trim();
                    sFeesDue = result.GetDouble("fees_due").ToString().Trim();
                    sFeesSurch = result.GetDouble("fees_surch").ToString().Trim();
                    sFeesPen = result.GetDouble("fees_pen").ToString().Trim();
                    sFeesTotDue = result.GetDouble("fees_totaldue").ToString().Trim();
                    sFeesCode = result.GetString("fees_code").Trim();
                    sDueState = result.GetString("due_state").Trim();
                    sQtrToPay = result.GetString("qtr_to_pay").Trim();

                    dgvDues.Rows.Add(sTaxYear, sTerm, sQtr, sFeesDesc, sFeesDue, sFeesSurch, sFeesPen, sFeesTotDue, sDueState, sQtrToPay, sFeesCode);

                }
            }
            result.Close();

            result.Query = "select * from pay_temp where bin = '" + m_strBIN.Trim() + "' and fees_code not like 'B%' order by fees_code";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sBin = result.GetString("bin").Trim();
                    sTaxYear = result.GetString("tax_year").Trim();
                    sQtr = result.GetString("qtr").Trim();
                    sTerm = result.GetString("term").Trim();
                    sFeesDesc = result.GetString("fees_desc").Trim();
                    sFeesDue = result.GetDouble("fees_due").ToString().Trim();
                    sFeesSurch = result.GetDouble("fees_surch").ToString().Trim();
                    sFeesPen = result.GetDouble("fees_pen").ToString().Trim();
                    sFeesTotDue = result.GetDouble("fees_totaldue").ToString().Trim();
                    sFeesCode = result.GetString("fees_code").Trim();
                    sDueState = result.GetString("due_state").Trim();
                    sQtrToPay = result.GetString("qtr_to_pay").Trim();

                    dgvDues.Rows.Add(sTaxYear, sTerm, sQtr, sFeesDesc, sFeesDue, sFeesSurch, sFeesPen, sFeesTotDue, sDueState, sQtrToPay, sFeesCode);

                }
            }
            result.Close();

        }

        private void DeletePaidQtrNew()
        {
            b1st = false;
            b2nd = false;
            b3rd = false;
            b4th = false;

            btnFull.Enabled = false;
            btnInstallment.Enabled = false;

            try
            {
                for (int x = 1; x <= Convert.ToInt16(m_strLastQtrPaid); x++)
                {
                    OracleResultSet result = new OracleResultSet();
                    result.Query = "delete from pay_temp where bin = '" + m_strBIN.Trim() + "'"; // check if it is okay even without qtr in query
                    result.ExecuteNonQuery();
                }
            }
            catch { }
        }

        private void frmPosting_Load(object sender, EventArgs e)
        {
            bin1.txtLGUCode.Text = AppSettingsManager.GetConfigObject("10");
            bin1.txtDistCode.Text = AppSettingsManager.GetConfigObject("11");
            dtpOrDate.Value = AppSettingsManager.GetSystemDate();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select is_qtrly from new_table where rev_year = '" + AppSettingsManager.GetConfigObject("07") + "'";
            if (result.Execute())
            {
                m_sIsQtrly = result.GetString("is_qtrly").Trim();
            }
            result.Close();

            if (m_sPostFormState == "ADD")
            {
                this.Text = "P O S T   P A Y M E N T - A D D";
                txtOrNo.BringToFront();
                m_sModule = "POSTING PAYMENT-ADD";
                ButtonControl(true);
                //btnView.Enabled = true;
                //btnDefTools.Enabled = true;
                this.LoadImageList();
            }
            else if (m_sPostFormState == "EDIT")
            {
                this.Text = "P O S T   P A Y M E N T - E D I T";
                cmbOrNo.BringToFront();
                m_sModule = "POSTING PAYMENT-EDIT";
                ButtonControl(true);
                btnSetAside.Enabled = false;
                btnView.Enabled = false;
                this.LoadImageList();
            }
            else if (m_sPostFormState == "DELETE")
            {
                this.Text = "P O S T   P A Y M E N T - D E L E T E";
                cmbOrNo.BringToFront();
                btnSave.Text = "&Delete";
                dgvDues.Enabled = false;
                m_sModule = "POSTING PAYMENT-DELETE";
                ButtonControl(false);
                btnDefTools.Enabled = true;
            }

            //SetCheckTerm(IDC_FULL);
            m_sPaymentTerm = "F";
            m_sMode = "POS";
            m_sPaymentType = "CS";
        }

        private void ButtonControl(bool blnEnable)
        {
            btnView.Enabled = blnEnable;
            btnAttach.Enabled = blnEnable;
            btnDefTools.Enabled = blnEnable;
            btnDetach.Enabled = blnEnable;
            //btnSetAside.Enabled = blnEnable;
            //btnReconcile.Enabled = blnEnable;
            //btnNoRecord.Enabled = blnEnable;
        }

        private void OnInstallment()
        {
            m_sPaymentTerm = "I";

            foreach (object r in this.GB2.Controls)
                if (r is CheckBox)
                {
                    ((CheckBox)r).Checked = false;
                    ((CheckBox)r).Enabled = true;
                }

            int iNextQtrToPay = 0;
            int.TryParse(m_sNextQtrToPay, out iNextQtrToPay);
            if (iNextQtrToPay > 1)
            {
                b1st = false;
                chk1st.Checked = false;
            }
            if (iNextQtrToPay > 2)
            {
                b2nd = false;
                chk2nd.Checked = false;
            }
            if (iNextQtrToPay > 3)
            {
                b3rd = false;
                chk3rd.Checked = false;
            }
            if (m_sNextQtrToPay == "2")
            {
                chk1st.Enabled = false;
                chk2nd.Checked = true;
                m_sQtrPaid = "2";
            }
            if (m_sNextQtrToPay == "3")
            {
                chk1st.Enabled = false;
                chk2nd.Enabled = false;
                chk3rd.Checked = true;
                m_sQtrPaid = "3";
            }
            if (m_sNextQtrToPay == "4")
            {
                chk1st.Enabled = false;
                chk2nd.Enabled = false;
                chk3rd.Enabled = false;
                chk4th.Checked = true;
                m_sQtrPaid = "4";
            }

            ChangeTermQtr();
        }

        private void ChangeTermQtr()
        {
            String sQuery;
            String sNewTerm, sNewQtr, sNewToPay;

            OracleResultSet pSet = new OracleResultSet();

            sNewTerm = m_sPaymentTerm;
            sNewQtr = m_sQtrPaid;
            if (m_sPaymentTerm == "F")
                sNewToPay = "F";
            else
                sNewToPay = m_sQtrPaid;

            for (int i = 0; i < dgvDues.RowCount; i++)
            {
                String sTaxYear, sBnsCode, sTerm, sQtr, sQtrToPay;
                sTaxYear = dgvDues.Rows[i].Cells[0].Value.ToString();
                sBnsCode = dgvDues.Rows[i].Cells[10].Value.ToString().Trim();
                sTerm = dgvDues.Rows[i].Cells[1].Value.ToString();
                sQtr = dgvDues.Rows[i].Cells[2].Value.ToString();
                sQtrToPay = dgvDues.Rows[i].Cells[9].Value.ToString();

                dgvDues.Rows[i].Cells[1].Value = sNewTerm;
                dgvDues.Rows[i].Cells[2].Value = sNewQtr;
                dgvDues.Rows[i].Cells[9].Value = sNewToPay;

                pSet.Query = string.Format("update pay_temp set term = '{0}',qtr = '{1}',qtr_to_pay = '{2}' where bin = '{3}' and tax_year = '{4}' and fees_code = '{5}' and term = '{6}' and qtr = '{7}' and qtr_to_pay = '{8}'", sNewTerm, sNewQtr, sNewToPay, bin1.GetBin(), sTaxYear, sBnsCode, sTerm, sQtr, sQtrToPay);
                pSet.ExecuteNonQuery();
                pSet.Close();
            }
        }

        private void OnFull()
        {
            btnFull.Enabled = true;
            m_sPaymentTerm = "F";
            m_sQtrPaid = "F";
            m_sNoOfQtr = "4";

            foreach (object r in this.GB2.Controls)
                if (r is CheckBox)
                {
                    ((CheckBox)r).Checked = false;
                    ((CheckBox)r).Enabled = false;
                }

            b1st = false;
            b2nd = false;
            b3rd = false;
            b4th = false;
            chkCash.Enabled = true;
            chkCheque.Enabled = true;
            //m_sNextQtrToPay = "";
            ChangeTermQtr();
        }

        private void SetNoOfQtr()
        {
            int iNoOfQtr = 0;

            if (chk1st.Checked)
                iNoOfQtr += 1;
            if (chk2nd.Checked)
                iNoOfQtr += 1;
            if (chk3rd.Checked)
                iNoOfQtr += 1;
            if (chk4th.Checked)
                iNoOfQtr += 1;

            m_sNoOfQtr = iNoOfQtr.ToString();
        }

        private void btnViewFiles_Click(object sender, EventArgs e)
        {
            // RMC 20111206 Added viewing of blob
            if (m_frmImageList.IsDisposed)
            {
                m_intImageListInstance = 0;
                m_frmImageList = new frmImageList();
                m_frmImageList.IsBuildUpPosting = true;
            }
            if (!m_frmImageList.IsDisposed && m_intImageListInstance == 0)
            {
                //if (m_frmImageList.ValidateImage(bin1.GetBin(), "C"))
                if (m_frmImageList.ValidateImage(bin1.GetBin(), AppSettingsManager.GetSystemType)) //MCR 20141209
                {
                    ImageInfo objImageInfo;
                    objImageInfo = new ImageInfo();

                    objImageInfo.TRN = bin1.GetBin();
                    //objImageInfo.System = "A";
                    objImageInfo.System = AppSettingsManager.GetSystemType; //MCR 20141209
                    m_frmImageList.isFortagging = false;
                    m_frmImageList.setImageInfo(objImageInfo);
                    m_frmImageList.Text = bin1.GetBin();
                    m_frmImageList.IsAutoDisplay = true;
                    m_frmImageList.Source = "VIEW";
                    m_frmImageList.Show();
                    m_intImageListInstance += 1;
                }
                else
                {
                    MessageBox.Show(string.Format("BIN {0} has no image", bin1.GetBin()));
                }

            }
        }

        private void btnAttachFiles_Click(object sender, EventArgs e)
        {
            this.LoadImageList(); // CJC 20130401         
        }

        private void LoadImageList()
        {
            if (m_frmImageList.IsDisposed)
            {
                m_intImageListInstance = 0;
                m_frmImageList = new frmImageList();
                m_frmImageList.IsBuildUpPosting = true;
            }
            if (!m_frmImageList.IsDisposed && m_intImageListInstance == 0)
            {
                ImageInfo objImageInfo;
                //objImageInfo = new ImageInfo("C", AppSettingsManager.SystemUser.UserCode);  // RMC 20111206
                objImageInfo = new ImageInfo(AppSettingsManager.GetSystemType, AppSettingsManager.SystemUser.UserCode);  // MCR 20141209

                m_frmImageList.Text = string.Format("Assigned Images - {0}", AppSettingsManager.SystemUser.UserCode);
                m_frmImageList.setImageInfo(objImageInfo);
                m_frmImageList.isFortagging = true;

                // RMC 20111206 Added attachment of blob image (s)
                if (m_sPostFormState == "ADD" || m_sPostFormState == "EDIT")
                    m_frmImageList.IsBuildUp = true;
                else
                    m_frmImageList.IsBuildUp = false;

                if (m_sPostFormState == "EDIT")
                    m_frmImageList.Source = "ATTACH";

                m_frmImageList.IsAutoDisplay = true;
                m_frmImageList.TopMost = true;
                m_frmImageList.Show();
                m_intImageListInstance += 1;
            }
        }

        private void SaveImage()
        {
            if (m_frmImageList.GetRecentImageID != 0)
            {
                string strImageFile = m_frmImageList.GetRecentImageFileNameDisplay;
                if (strImageFile != null && strImageFile != "")
                {
                    ImageInfo objImageInfo;
                    //objImageInfo = new ImageInfo(bin1.GetBin(), "A", AppSettingsManager.SystemUser.UserCode);   // RMC 20120119 corrected saving of image in Business records
                    //objImageInfo = new ImageInfo(m_strBIN, "C", AppSettingsManager.SystemUser.UserCode);   // MCR 20141203
                    objImageInfo = new ImageInfo(m_strBIN, AppSettingsManager.GetSystemType, AppSettingsManager.SystemUser.UserCode);   // MCR 20141209

                    if (!m_frmImageList.UpdateBlobImage(objImageInfo))
                    {
                        MessageBox.Show("Failed to attach image.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Failed to attach image.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

            }
            else
            {
                string strImageFile = m_frmImageList.GetRecentImageFileNameDisplay;
                string strImageName = System.IO.Path.GetFileName(strImageFile);

                if (strImageFile != "")
                {
                    if (MessageBox.Show(this, string.Format("Do you want to attach the image {0} to BIN {1}", strImageName, bin1.GetBin()), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        ImageTransation objTransaction = new ImageTransation();

                        if (!(objTransaction.InsertImage(bin1.GetBin(), strImageFile, "")))
                        {
                            MessageBox.Show("Failed to attach image.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        else
                        {
                            m_frmImageList.Close();
                            MessageBox.Show("Image attached.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }

        }

        private void btnDetachFiles_Click(object sender, EventArgs e)
        {
            if (m_frmImageList.GetRecentImageID == 0)
            {
                MessageBox.Show("View image to detach first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Do you want to detach file : " + m_frmImageList.GetRecentImageFileNameDisplay + " from BIN: " + bin1.GetBin() + ".", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (m_frmImageList.GetRecentImageID != 0)
                {
                    string strImageFile = m_frmImageList.GetRecentImageFileNameDisplay;
                    if (strImageFile != null && strImageFile != "")
                    {
                        ImageTransation objTransaction = new ImageTransation();
                        if (!(objTransaction.DetachImage(bin1.GetBin(), m_frmImageList.GetRecentImageID)))
                        {
                            MessageBox.Show("Failed to detach image.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        else
                        {
                            m_frmImageList.Close();
                            MessageBox.Show("Image detached", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                        MessageBox.Show("View image to detach first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("View image to detach first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void DetachImage()
        {
            ImageTransation objTransaction = new ImageTransation();
            if (!(objTransaction.DetachImage(bin1.GetBin())))
            {
                MessageBox.Show("Failed to detach image from this record.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                MessageBox.Show("Image was detached from this record.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnFull_Click(object sender, EventArgs e)
        {
            OnFull();
        }

        private void btnInstallment_Click(object sender, EventArgs e)
        {
            //SetCheckTerm(IDC_INSTALLMENT);
            OnInstallment();
        }

        private void btnDefTools_Click(object sender, EventArgs e)
        {
            frmDeficientRecords = new frmDeficientRecords();
            frmDeficientRecords.BIN = bin1.GetBin();
            frmDeficientRecords.ShowDialog();
        }

        private void btnReconcile_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reconcile the image to this record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /*		CString sFileName;
                        sFileName = m_sFile.Right(m_sFile.GetLength()-m_sFile.ReverseFind('\\')-1);
                        pApp->m_sFileDir = m_sFile.Left(m_sFile.GetLength()-sFileName.GetLength());
                        TransferReconciledRecord(m_sBIN);	
                */
                String sFile = m_sFile;
                //CToken tokenFile(sFile);
                //tokenFile.SetToken("|");  

                //while(tokenFile.MoreTokens() == 1)
                //{
                //m_sFile = tokenFile.GetNextToken();

                String sFileName;
                sFileName = m_sFile.Substring(m_sFile.Length - m_sFile.LastIndexOf('\\') - 1);
                //pApp->m_sFileDir = m_sFile.Substring(m_sFile.Length - sFileName.Length);
                TransferCorrectedRecords(m_strBIN);
                //}
            }
        }

        private void MakeSureDirectoryPathExists(String strPath, String sMainDir)
        {
            //char chdir[MAX_PATH];
            //strcpy(chdir, sMainDir);
            //while(_chdir(chdir)) 
            //{
            //    MessageBox("The Destination Folder cannot be accessed. Please check the network connection.",APP_NAME,MB_ICONSTOP);
            //    // JGR 12202004 VALIDATE DESTINATION FOLDER
            //    // START
            //    CBPLSColApp *pApp = (CBPLSColApp*)AfxGetApp();
            //    _RecordsetPtr pSet;
            //    CString sQuery;

            //    sQuery = "select * from directory_tbl";
            //    sQuery+= " where user_code = '" + pApp->sUser + "' and type_fld = 'COL'";
            //    pSet->Open(_bstr_t(sQuery),pApp->m_pConnection.GetInterfacePtr(),adOpenStatic,adLockReadOnly,adCmdText);	
            //    if (!pSet->adoEOF)
            //        strPath = pApp->GetStrVariant(pSet->GetCollect(_variant_t("ddir_folder")));
            //    pSet->Close();

            //    strcpy(chdir, strPath);
            //}

            String str; int loc;
            //loc = strPath.Find("\\", 0);
            //while(loc != -1) 
            //{
            //    str = strPath.Left(loc+1);
            //    strcpy(chdir, str);
            //    if(_chdir(chdir)) 
            //    {
            //        _mkdir(chdir);
            //    }
            //    loc = strPath.Find("\\", loc+1);
            //}
        }

        private void btnSetAside_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery, sSource, sServer = String.Empty, sFileNew, sFileType, sFileName, sCurrentDate, sCurrentTime, sCategory, sMainDir;
            DateTime odtDateTime;

            //int iResults = 0;
            DialogResult iResults;
            iResults = MessageBox.Show("Do you want to set aside this file as deficient record or for later reconciliation? \n Click YES for RECONCILIATION \n Click NO for DEFICIENT RECORD", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            switch (iResults)
            {
                case DialogResult.Cancel:
                    break;
                    return;
                case DialogResult.No:
                    {
                        String sFile = m_sFile;
                        //CToken tokenFile(sFile);
                        //tokenFile.SetToken("|");  

                        //while(tokenFile.MoreTokens() == 1)
                        //{
                        //m_sFile = tokenFile.GetNextToken();

                        sQuery = "select * from directory_tbl";
                        sQuery += " where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and type_fld = 'COL'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                sServer = pSet.GetString("ddir_folder");
                                sSource = pSet.GetString("sdir_folder");
                            }
                        pSet.Close();

                        sMainDir = sServer;
                        sServer = sServer + "\\COLL\\DEFRECORDS\\UNCAT\\";
                        MakeSureDirectoryPathExists(sServer, sMainDir);
                        sFileType = m_sFile.Substring(m_sFile.Length - m_sFile.LastIndexOf('.') - 1);
                        sFileName = m_sFile.Substring(m_sFile.Length - m_sFile.LastIndexOf('\\') - 1);
                        sFileNew = sServer + "\\" + sFileName;

                        //// JGR 12202004 VALIDATE DESTINATION FOLDER
                        //// START
                        //char chdir[MAX_PATH];
                        //strcpy(chdir, sMainDir);
                        //while(_chdir(chdir)) 
                        //{
                        //    MessageBox("The Destination Folder cannot be accessed. Please check the network connection.",APP_NAME,MB_ICONSTOP);

                        //    sQuery = "select * from directory_tbl";
                        //    sQuery+= " where user_code = '" + pApp->sUser + "' and type_fld = 'COL'";
                        //    pSet->Open(_bstr_t(sQuery),pApp->m_pConnection.GetInterfacePtr(),adOpenStatic,adLockReadOnly,adCmdText);	
                        //    if (!pSet->adoEOF)
                        //        sMainDir = pApp->GetStrVariant(pSet->GetCollect(_variant_t("ddir_folder")));
                        //    pSet->Close();

                        //    strcpy(chdir, sMainDir);
                        //}
                        //// JGR 12202004 VALIDATE DESTINATION FOLDER
                        //// END

                        //CFileFind finder;
                        //if (!finder.FindFile(sFileNew))
                        //{
                        //    CopyFile(m_sFile, sFileNew, 0);
                        //    DeleteFile(m_sFile);
                        //}
                        //else
                        //{
                        //    sFileNew = CreateTempSourceName(sServer,sFileType);
                        //    sFileNew = sServer + "\\" + sFileNew;

                        //    if (!finder.FindFile(sFileNew))
                        //    {
                        //        CopyFile(m_sFile, sFileNew, 0);
                        //        DeleteFile(m_sFile);
                        //    }
                        //}

                        odtDateTime = AppSettingsManager.GetSystemDate();
                        sCurrentDate = string.Format("{0}/{1}/{2}",
                                    odtDateTime.Month,
                                    odtDateTime.Day,
                                    odtDateTime.Year);
                        sCurrentTime = string.Format("{0}/{1}/{2}",
                                    odtDateTime.Hour,
                                    odtDateTime.Minute,
                                    odtDateTime.Second);

                        sQuery = "insert into image_login values (";
                        sQuery += "'" + AppSettingsManager.SystemUser.UserCode + "', 'U',";
                        sQuery += "'" + m_sFile + "',";
                        sQuery += "'" + sFileNew + "',";
                        sQuery += "'" + sCurrentDate + "',";
                        sQuery += "'" + sCurrentTime + "',";
                        sQuery += "'COL')";
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                        //}
                        MessageBox.Show("Image is moved to DEFICIENT RECORD folder", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // JGR 09302004 REMOVE CURRENTLY OPENED PROGRAM
                        //			char szName[100]="BravaReader.exe";//C:\Program Files\IGC\Brava! Reader\BravaReader.exe";   // Name of process to terminate
                        //char szName[100]; strcpy(szName, pApp->m_sViewer);		// JGR 06092007 USE PAPP VARIABLE FOR VIEWER
                        //int iVal = KILL_PROC_BY_NAME(szName);

                        //CListOfImages ListDlg;
                        //ListDlg.DoModal();
                        //if(ListDlg.m_bOk)
                        //    m_sFile = ListDlg.m_sFile;

                    }
                    break;
                case DialogResult.Yes:
                    {
                        String sFile = m_sFile;
                        //CToken tokenFile(sFile);
                        //tokenFile.SetToken("|");  

                        //while(tokenFile.MoreTokens() == 1)
                        //{
                        //m_sFile = tokenFile.GetNextToken();

                        //(s) JJP 09102004 REMOVE TAG ON DEFICIENCY IF SET ASIDE
                        String sQueryDef;
                        sQueryDef = string.Format("delete from def_records_tmp where rec_acct_no = '{0}'", AppSettingsManager.GenerateTmpAccountNo());
                        pSet.Query = sQueryDef;
                        pSet.ExecuteNonQuery();
                        //(e) JJP 09102004 REMOVE TAG ON DEFICIENCY IF SET ASIDE

                        sQuery = "select * from directory_tbl";
                        sQuery += " where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and type_fld = 'COLL'";
                        pSet.Query = sQuery;
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                sServer = pSet.GetString("ddir_folder");
                                sSource = pSet.GetString("sdir_folder");
                            }
                        pSet.Close();

                        sMainDir = sServer;
                        sServer = sServer + "\\COLL\\RECONCILE\\";
                        MakeSureDirectoryPathExists(sServer, sMainDir);
                        sFileType = m_sFile.Substring(m_sFile.Length - m_sFile.LastIndexOf('.') - 1);
                        sFileName = m_sFile.Substring(m_sFile.Length - m_sFile.LastIndexOf('\\') - 1);
                        sFileNew = sServer + "\\" + sFileName;

                        //// JGR 12202004 VALIDATE DESTINATION FOLDER
                        //// START
                        //char chdir[MAX_PATH];
                        //strcpy(chdir, sMainDir);
                        //while(_chdir(chdir)) 
                        //{
                        //    MessageBox("The Destination Folder cannot be accessed. Please check the network connection.",APP_NAME,MB_ICONSTOP);

                        //    sQuery = "select * from directory_tbl";
                        //    sQuery+= " where user_code = '" + pApp->sUser + "' and type_fld = 'COLL'";
                        //    pSet->Open(_bstr_t(sQuery),pApp->m_pConnection.GetInterfacePtr(),adOpenStatic,adLockReadOnly,adCmdText);	
                        //    if (!pSet->adoEOF)
                        //        sMainDir = pApp->GetStrVariant(pSet->GetCollect(_variant_t("ddir_folder")));
                        //    pSet->Close();

                        //    strcpy(chdir, sMainDir);
                        //}
                        //// JGR 12202004 VALIDATE DESTINATION FOLDER
                        //// END

                        //CFileFind finder;
                        //if (!finder.FindFile(sFileNew))
                        //{
                        //    CopyFile(m_sFile, sFileNew, 0);
                        //    DeleteFile(m_sFile);
                        //}
                        //else
                        //{
                        //    sFileNew = CreateTempSourceName(sServer,sFileType);
                        //    sFileNew = sServer + "\\" + sFileNew;

                        //    if (!finder.FindFile(sFileNew))
                        //    {
                        //        CopyFile(m_sFile, sFileNew, 0);
                        //        DeleteFile(m_sFile);
                        //    }
                        //}

                        odtDateTime = AppSettingsManager.GetSystemDate();
                        sCurrentDate = string.Format("{0}/{1}/{2}",
                                    odtDateTime.Month,
                                    odtDateTime.Day,
                                    odtDateTime.Year);
                        sCurrentTime = string.Format("{0}/{1}/{2}",
                                    odtDateTime.Hour,
                                    odtDateTime.Minute,
                                    odtDateTime.Second);

                        sQuery = "insert into image_login values (";
                        sQuery += "'" + AppSettingsManager.SystemUser.UserCode + "', 'R',";
                        sQuery += "'" + m_sFile + "',";
                        sQuery += "'" + sFileNew + "',";
                        sQuery += "'" + sCurrentDate + "',";
                        sQuery += "'" + sCurrentTime + "',";
                        sQuery += "'COL')";
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                        //}

                        MessageBox.Show("Image is moved to RECONCILED RECORD folder", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //            // JGR 09302004 REMOVE CURRENTLY OPENED PROGRAM
                        ////			char szName[100]="BravaReader.exe";//C:\Program Files\IGC\Brava! Reader\BravaReader.exe";   // Name of process to terminate
                        //            char szName[100]; strcpy(szName, pApp->m_sViewer);		// JGR 06092007 USE PAPP VARIABLE FOR VIEWER
                        //            int iVal = KILL_PROC_BY_NAME(szName);

                        //CListOfImages ListDlg;
                        //ListDlg.DoModal();
                        //if(ListDlg.m_bOk)
                        //    m_sFile = ListDlg.m_sFile;

                    }
                    break;
            }

            // JGR 09302004 REMOVE CURRENTLY OPENED PROGRAM
            //   char szName[100]="BravaReader.exe";//C:\Program Files\IGC\Brava! Reader\BravaReader.exe";   // Name of process to terminate
            //	int iVal = KILL_PROC_BY_NAME(szName);

        }

        private void btnNoRecord_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery, sSource, sServer, sFileNew = "", sFileType, sFileName, sCurrentDate, sCurrentTime, sCategory, sMainDir;
            DateTime odtDateTime;

            if (MessageBox.Show("Are you sure you want to move this image to No Record folder?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sQuery = "select * from directory_tbl";
                sQuery += " where user_code = '" + AppSettingsManager.SystemUser.UserCode + "' and type_fld = 'COLL'";
                pSet.Query = sQuery;

                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sServer = pSet.GetString("ddir_folder");
                        sSource = pSet.GetString("sdir_folder");
                    }
                pSet.Close();

                //sMainDir = sServer;
                //sServer = sServer + "\\COLL\\RECONCILE\\NO RECORD\\";
                //MakeSureDirectoryPathExists(sServer, sMainDir);
                //sFileType = m_sFile.Right(m_sFile.GetLength()-m_sFile.ReverseFind('.')-1);
                //sFileName = m_sFile.Right(m_sFile.GetLength()-m_sFile.ReverseFind('\\')-1);
                //sFileNew = sServer + "\\" + sFileName;

                //// JGR 12202004 VALIDATE DESTINATION FOLDER
                //// START
                //char chdir[MAX_PATH];
                //strcpy(chdir, sMainDir);
                //while(_chdir(chdir)) 
                //{
                //    MessageBox.Show("The Destination Folder cannot be accessed. Please check the network connection.",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Stop);

                //    sQuery = "select * from directory_tbl";
                //    sQuery+= " where user_code = '" + pApp->sUser + "' and type_fld = 'COLL'";
                //    pSet->Open(_bstr_t(sQuery),pApp->m_pConnection.GetInterfacePtr(),adOpenStatic,adLockReadOnly,adCmdText);	
                //    if (!pSet->adoEOF)
                //        sMainDir = pApp->GetStrVariant(pSet->GetCollect(_variant_t("ddir_folder")));
                //    pSet->Close();

                //    strcpy(chdir, sMainDir);
                //}
                //// JGR 12202004 VALIDATE DESTINATION FOLDER
                //// END

                //CFileFind finder;
                //if (!finder.FindFile(sFileNew))
                //{
                //    CopyFile(m_sFile, sFileNew, 0);
                //    DeleteFile(m_sFile);
                //}
                //else
                //{
                //    sFileNew = CreateTempSourceName(sServer,sFileType);
                //    sFileNew = sServer + "\\" + sFileNew;

                //    if (!finder.FindFile(sFileNew))
                //    {
                //        CopyFile(m_sFile, sFileNew, 0);
                //        DeleteFile(m_sFile);
                //    }
                //}

                odtDateTime = AppSettingsManager.GetSystemDate();
                sCurrentDate = string.Format("{0}/{1}/{2}",
                            odtDateTime.Month,
                            odtDateTime.Day,
                            odtDateTime.Year);
                sCurrentTime = string.Format("{0}/{1}/{2}",
                            odtDateTime.Hour,
                            odtDateTime.Minute,
                            odtDateTime.Second);

                sQuery = "insert into image_login values (";
                sQuery += "'" + AppSettingsManager.SystemUser.UserCode + "', 'R',";
                sQuery += "'" + m_sFile + "',";
                sQuery += "'" + sFileNew + "',";
                sQuery += "'" + sCurrentDate + "',";
                sQuery += "'" + sCurrentTime + "',";
                sQuery += "'COL')";

                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
            }
        }

        private void dgvDues_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            String m_sDue, m_sSurch, m_sInt, m_sAmtDue;
            double m_dDue, m_dSurch, m_dInt, m_dAmtDue;

            try
            {
                m_sDue = dgvDues.CurrentRow.Cells[4].Value.ToString();
                m_sSurch = dgvDues.CurrentRow.Cells[5].Value.ToString();
                m_sInt = dgvDues.CurrentRow.Cells[6].Value.ToString();
                m_dAmtDue = Convert.ToDouble(m_sDue) + Convert.ToDouble(m_sSurch) + Convert.ToDouble(m_sInt);
                dgvDues.CurrentRow.Cells[7].Value = m_dAmtDue;

                String sTaxYear, sBnsCode, sTerm, sQtr, sQtrToPay;
                sTaxYear = dgvDues.CurrentRow.Cells[0].Value.ToString();
                sBnsCode = dgvDues.CurrentRow.Cells[10].Value.ToString();
                sTerm = dgvDues.CurrentRow.Cells[1].Value.ToString();
                sQtr = dgvDues.CurrentRow.Cells[2].Value.ToString();
                sQtrToPay = dgvDues.CurrentRow.Cells[9].Value.ToString();

                pSetCellValue.Query = string.Format("update pay_temp set fees_due = '{0}',fees_surch = '{1}', fees_pen = '{2}', fees_totaldue = '{3}' where bin = '{4}' and tax_year = '{5}' and fees_code = '{6}' and term = '{7}' and qtr = '{8}' and qtr_to_pay = '{9}'", m_sDue, m_sSurch, m_sInt, m_dAmtDue, bin1.GetBin(), sTaxYear, sBnsCode, sTerm, sQtr, sQtrToPay);
                pSetCellValue.ExecuteNonQuery();

                m_dDue = 0;
                m_dSurch = 0;
                m_dInt = 0;
                m_dAmtDue = 0;

                for (int i = 0; i < dgvDues.RowCount; i++)
                {
                    m_sDue = dgvDues.Rows[i].Cells[4].Value.ToString();
                    m_sSurch = dgvDues.Rows[i].Cells[5].Value.ToString();
                    m_sInt = dgvDues.Rows[i].Cells[6].Value.ToString();
                    m_sAmtDue = dgvDues.Rows[i].Cells[7].Value.ToString();

                    m_dDue += Convert.ToDouble(m_sDue);
                    m_dSurch += Convert.ToDouble(m_sSurch);
                    m_dInt += Convert.ToDouble(m_sInt);
                    m_dAmtDue += Convert.ToDouble(m_sAmtDue);
                }

                txtGrandDue.Text = m_dDue.ToString("#,##0.00");
                txtGrandSurch.Text = m_dSurch.ToString("#,##0.00");
                txtGrandPen.Text = m_dInt.ToString("#,##0.00");
                txtGrandTotal.Text = m_dAmtDue.ToString("#,##0.00");
            }
            catch { }
        }

        private void dgvDues_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //unit amount
            decimal varDecimal;
            if (dgvDues.CurrentRow.Cells[4].Selected)
            {
                try
                {
                    if (dgvDues.CurrentRow.Cells[4].Value.ToString() != "")
                    {
                        if (!decimal.TryParse(dgvDues.CurrentRow.Cells[4].Value.ToString().Trim(), out varDecimal) || varDecimal < 0)
                        {
                            dgvDues.CurrentRow.Cells[4].Value = "0";
                        }
                    }
                }
                catch { }
            }
        }

        private void dgvDues_SelectionChanged(object sender, EventArgs e)
        {
            dgvDues.BeginEdit(true);
        }

        private void dgvDues_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvDues.IsCurrentCellDirty)
                dgvDues.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void radio_Click(object sender, EventArgs e)
        {
            if (chk1st.Checked && !chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
            {
                if (m_sNextQtrToPay == "")
                {
                    m_sNoOfQtr = "1";
                    m_sQtrPaid = "1";
                }
                chk2nd.Checked = false;
                chk3rd.Checked = false;
                chk4th.Checked = false;
            }
            else if (chk1st.Checked && chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
            {
                chk3rd.Checked = false;
                chk4th.Checked = false;

                if (m_sNextQtrToPay == "1" && !chk1st.Checked)
                    chk2nd.Checked = false;

                m_sQtrPaid = "2";
            }
            else if (chk1st.Checked && chk2nd.Checked && chk3rd.Checked && !chk4th.Checked)
            {
                if ((chk1st.Checked) && (!chk2nd.Checked))
                    chk1st.Checked = false;

                chk4th.Checked = false;

                if (m_sNextQtrToPay == "1" && !chk2nd.Checked)
                    chk3rd.Checked = false;
                if (m_sNextQtrToPay == "2" && !chk2nd.Checked)
                    chk3rd.Checked = false;

                m_sQtrPaid = "3";
            }
            else if (chk1st.Checked && chk2nd.Checked && chk3rd.Checked && chk4th.Checked)
            {
                if ((chk1st.Checked) && (!chk2nd.Checked))
                    chk1st.Checked = false;
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

                if (m_sNextQtrToPay == "1" && !chk3rd.Checked)
                    chk4th.Checked = false;
                if (m_sNextQtrToPay == "2" && !chk3rd.Checked)
                    chk4th.Checked = false;
                if (m_sNextQtrToPay == "3" && !chk3rd.Checked)
                    chk4th.Checked = false;
                m_sQtrPaid = "4";
            }
            else
            {
                //chk1st.Checked = false;
                //chk2nd.Checked = true;
                //chk3rd.Checked = false;
                //chk4th.Checked = false;
                //m_sQtrPaid = "2";

                if (chk3rd.Enabled == false)
                {
                    if (!chk1st.Checked && !chk2nd.Checked && !chk3rd.Checked && chk4th.Checked)
                    {
                        if ((chk1st.Checked) && (!chk2nd.Checked))
                            chk1st.Checked = false;
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

                        if (m_sNextQtrToPay == "1" && !chk3rd.Checked)
                            chk4th.Checked = false;
                        if (m_sNextQtrToPay == "2" && !chk3rd.Checked)
                            chk4th.Checked = false;
                        if (m_sNextQtrToPay == "3" && !chk3rd.Checked)
                            chk4th.Checked = false;
                        m_sQtrPaid = "4";
                    }
                    else
                    {
                        chk1st.Checked = false;
                        chk2nd.Checked = false;
                        chk3rd.Checked = false;
                        chk4th.Checked = false;
                        m_sQtrPaid = "";
                    }
                }
                else if (chk2nd.Enabled == false)
                {
                    if (!chk1st.Checked && !chk2nd.Checked && chk3rd.Checked && chk4th.Checked)
                    {
                        if ((chk1st.Checked) && (!chk2nd.Checked))
                            chk1st.Checked = false;
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

                        if (m_sNextQtrToPay == "1" && !chk3rd.Checked)
                            chk4th.Checked = false;
                        if (m_sNextQtrToPay == "2" && !chk3rd.Checked)
                            chk4th.Checked = false;
                        if (m_sNextQtrToPay == "3" && !chk3rd.Checked)
                            chk4th.Checked = false;
                        m_sQtrPaid = "4";
                    }
                    else //if (!chk1st.Checked && !chk2nd.Checked && chk3rd.Checked && !chk4th.Checked)
                    {
                        if ((chk1st.Checked) && (!chk2nd.Checked))
                            chk1st.Checked = false;

                        chk4th.Checked = false;

                        if (m_sNextQtrToPay == "1" && !chk2nd.Checked)
                            chk3rd.Checked = false;
                        if (m_sNextQtrToPay == "2" && !chk2nd.Checked)
                            chk3rd.Checked = false;

                        m_sQtrPaid = "3";
                    }
                }
                else if (chk1st.Enabled == false)
                {
                    if (!chk1st.Checked && chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
                    {
                        chk3rd.Checked = false;
                        chk4th.Checked = false;

                        if (m_sNextQtrToPay == "1" && !chk1st.Checked)
                            chk2nd.Checked = false;

                        m_sQtrPaid = "2";
                    }
                    else if (!chk1st.Checked && chk2nd.Checked && chk3rd.Checked && !chk4th.Checked)
                    {
                        if ((chk1st.Checked) && (!chk2nd.Checked))
                            chk1st.Checked = false;

                        chk4th.Checked = false;

                        if (m_sNextQtrToPay == "1" && !chk2nd.Checked)
                            chk3rd.Checked = false;
                        if (m_sNextQtrToPay == "2" && !chk2nd.Checked)
                            chk3rd.Checked = false;

                        m_sQtrPaid = "3";
                    }
                    else if (!chk1st.Checked && chk2nd.Checked && chk3rd.Checked && chk4th.Checked)
                    {
                        if ((chk1st.Checked) && (!chk2nd.Checked))
                            chk1st.Checked = false;
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

                        if (m_sNextQtrToPay == "1" && !chk3rd.Checked)
                            chk4th.Checked = false;
                        if (m_sNextQtrToPay == "2" && !chk3rd.Checked)
                            chk4th.Checked = false;
                        if (m_sNextQtrToPay == "3" && !chk3rd.Checked)
                            chk4th.Checked = false;
                        m_sQtrPaid = "4";
                    }
                    else
                    {
                        chk3rd.Checked = false;
                        chk4th.Checked = false;

                        if (m_sNextQtrToPay == "1" && !chk1st.Checked)
                            chk2nd.Checked = false;

                        m_sQtrPaid = "2";
                    }
                }

                else if ((!chk1st.Checked && chk2nd.Checked && chk3rd.Checked && chk4th.Checked) ||
                    (chk1st.Checked && !chk2nd.Checked && chk3rd.Checked && chk4th.Checked) ||
                    (chk1st.Checked && !chk2nd.Checked && chk3rd.Checked && !chk4th.Checked) ||
                    (chk1st.Checked && !chk2nd.Checked && !chk3rd.Checked && chk4th.Checked))
                {
                    if (m_sNextQtrToPay == "")
                    {
                        m_sNoOfQtr = "1";
                        m_sQtrPaid = "1";
                    }
                    chk2nd.Checked = false;
                    chk3rd.Checked = false;
                    chk4th.Checked = false;
                }
                else if (chk1st.Checked && chk2nd.Checked && !chk3rd.Checked && chk4th.Checked)
                {
                    chk3rd.Checked = false;
                    chk4th.Checked = false;

                    if (m_sNextQtrToPay == "1" && !chk1st.Checked)
                        chk2nd.Checked = false;

                    m_sQtrPaid = "2";
                }
                else
                {
                    chk1st.Checked = false;
                    chk2nd.Checked = false;
                    chk3rd.Checked = false;
                    chk4th.Checked = false;
                    m_sQtrPaid = "";
                }
            }

            ChangeTermQtr();
        }

        private void radio1st_Click(object sender, EventArgs e)
        {
            if (chk1st.Checked)
            {
                // ALJ 05152003 (s)to accept any qtr on initial add
                if (m_sNextQtrToPay == "")
                {
                    m_sNoOfQtr = "1";
                    m_sQtrPaid = "1";
                }
                // ALJ 05152003 (e)
                chk2nd.Checked = false;
                chk3rd.Checked = false;
                chk4th.Checked = false;
            }
            else
            {
                chk1st.Checked = true;
            }

            ChangeTermQtr();
        }

        private void radio2nd_Click(object sender, EventArgs e)
        {
            if (chk2nd.Checked)
            {
                chk3rd.Checked = false;
                chk4th.Checked = false;
            }
            else
            {
                if (!chk1st.Checked)
                    chk2nd.Checked = true;
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

            m_sQtrPaid = "2";
            ChangeTermQtr();
        }

        private void radio3rd_Click(object sender, EventArgs e)
        {
            if (chk3rd.Checked)
            {
                if ((chk1st.Checked) && (!chk2nd.Checked))
                {
                    chk1st.Checked = false;
                }

                chk4th.Checked = false;
            }
            else
            {
                if (!chk1st.Checked && !chk2nd.Checked)
                    chk3rd.Checked = true;
                else
                    chk4th.Checked = false;
            }

            if (m_sNextQtrToPay == "1" && !chk2nd.Checked)
            {
                chk3rd.Checked = false;
            }

            if (m_sNextQtrToPay == "2" && !chk2nd.Checked)
            {
                chk3rd.Checked = false;
            }

            m_sQtrPaid = "3";
            ChangeTermQtr();
        }

        private void radio4th_Click(object sender, EventArgs e)
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

            }
            else
            {
                if (!chk1st.Checked && !chk2nd.Checked && !chk3rd.Checked)
                    chk4th.Checked = true;
            }

            if (m_sNextQtrToPay == "1" && !chk3rd.Checked)
            {
                chk4th.Checked = false;
            }

            if (m_sNextQtrToPay == "2" && !chk3rd.Checked)
            {
                chk4th.Checked = false;
            }

            if (m_sNextQtrToPay == "3" && !chk3rd.Checked)
            {
                chk4th.Checked = false;
            }
            m_sQtrPaid = "4";
            ChangeTermQtr();
        }

        private string FeesTerm(String sFeesCode)
        {
            String sFeesTerm = "";
            OracleResultSet pSetTerm = new OracleResultSet();

            if (sFeesCode.Substring(0, 1) == "B")
            {
                sFeesTerm = "I";//Q
            }
            else
            {
                if (chkPartial.Checked == false)
                {
                    //string.Format("select * from tax_and_fees_table where fees_code like '%s'",sFeesCode);
                    //pSetTerm.Query = string.Format("select * from tax_and_fees_table where fees_code = '{0}' ", sFeesCode); // CTS 09152003
                    pSetTerm.Query = string.Format("select * from tax_and_fees_table where fees_code = '{0}' and rev_year = '{1}'", sFeesCode, AppSettingsManager.GetConfigValue("07")); // MCR 20141118
                    if (pSetTerm.Execute())
                        if (pSetTerm.Read())
                        {
                            sFeesTerm = pSetTerm.GetString("fees_term").Trim();
                        }
                    pSetTerm.Close();
                }
                else
                {
                    pSetTerm.Query = string.Format("select * from partial_fees where fees_code = '{0}'", sFeesCode); // CTS 09152003
                    if (pSetTerm.Execute())
                        if (pSetTerm.Read())
                        {
                            sFeesTerm = pSetTerm.GetString("fees_term").Trim();
                        }
                    pSetTerm.Close();
                }
            }

            return sFeesTerm;
        }

        private void TransferFile(String sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();

            String sDef = "";
            pSet.Query = "select rec_acct_no from def_records where rec_acct_no = '" + sBIN + "' and def_status = 'N'";
            if (pSet.Execute())
                if (pSet.Read())
                    sDef = "Y";
                else
                    sDef = "N";
            pSet.Close();

            //CBlobModule cBlob;
            //cBlob.m_sBIN = sBIN;
            //cBlob.LoadDocument(m_sFile, "",sDef);
        }

        private void TransferCorrectedRecords(String sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            String sQuery, sServer, sSource, sFile, sFileNew, sFileName, sFileType, sBrgy, sDist, sZone, sCurrentDate, sCurrentTime, sCategory, sDefStatus, sMainDir;

            //CBlobModule cBlob;
            //cBlob.m_sBIN = sBIN;

            if (m_sFile != String.Empty)
            {
                String sDef;
                sQuery = "select rec_acct_no from def_records";
                sQuery += " where rec_acct_no = '" + sBIN + "' and def_status = 'N'";

                pSet.Query = sQuery;
                if (pSet.Execute())
                    if (pSet.Read())
                        sDef = "Y";
                    else
                        sDef = "N";
                pSet.Close();

                sFile = m_sFile;
                //CToken tokenFile(sFile);
                //tokenFile.SetToken("|");  

                //while(tokenFile.MoreTokens() == 1)
                //{
                //    cBlob.LoadDocument(m_sFile, "",sDef);
                //}
            }
            //else
            //cBlob.UpdateRecordForDeficiency();
        }

        private void TransferReconciledRecord(String sBIN)
        {
            TransferFile(sBIN);
        }

        private int KILL_PROC_BY_NAME(char szToTerminate)
        {
            //// Created: 6/23/2000  (RK)
            //// Last modified: 3/10/2002  (RK)
            //// Please report any problems or bugs to kochhar@physiology.wisc.edu
            //// The latest version of this routine can be found at:
            ////     http://www.neurophys.wisc.edu/ravi/software/killproc/
            //// Terminate the process "szToTerminate" if it is currently running
            //// This works for Win/95/98/ME and also Win/NT/2000/XP
            //// The process name is case-insensitive, i.e. "notepad.exe" and "NOTEPAD.EXE"
            //// will both work (for szToTerminate)
            //// Return codes are as follows:
            ////   0   = Process was successfully terminated
            ////   603 = Process was not currently running
            ////   604 = No permission to terminate process
            ////   605 = Unable to load PSAPI.DLL
            ////   602 = Unable to terminate process for some other reason
            ////   606 = Unable to identify system type
            ////   607 = Unsupported OS
            ////   632 = Invalid process name
            ////   700 = Unable to get procedure address from PSAPI.DLL
            ////   701 = Unable to get process list, EnumProcesses failed
            ////   702 = Unable to load KERNEL32.DLL
            ////   703 = Unable to get procedure address from KERNEL32.DLL
            ////   704 = CreateToolhelp32Snapshot failed
            //// Change history:
            ////   modified 3/8/2002  - Borland-C compatible if BORLANDC is defined as
            ////                        suggested by Bob Christensen
            ////   modified 3/10/2002 - Removed memory leaks as suggested by
            ////					      Jonathan Richard-Brochu (handles to Proc and Snapshot
            ////                        were not getting closed properly in some cases)

            //    // RMC 20100819 Merged & modified blob from rpta (s)
            //    // MCV 20090608 <s> OVERRIDE THIS CODES Note: Not applicable
            //    CBPLSColApp *pApp = (CBPLSColApp*)AfxGetApp();
            //    CBlobModule2 cBlob;
            //    cBlob.KillProcessHandle(pApp->pInfoViewer);
            //    return 1;
            //    // MCV 20090608 <e> OVERRIDE THIS CODES Note: Not applicable
            //    // RMC 20100819 Merged & modified blob from rpta (e)


            //    BOOL bResult,bResultm;
            //    DWORD aiPID[1000],iCb=1000,iNumProc,iV2000=0;
            //    DWORD iCbneeded,i,iFound=0;
            //    char szName[MAX_PATH],szToTermUpper[MAX_PATH];
            //    HANDLE hProc,hSnapShot,hSnapShotm;
            //    OSVERSIONINFO osvi;
            //    HINSTANCE hInstLib;
            //    int iLen,iLenP,indx;
            //    HMODULE hMod;
            //    PROCESSENTRY32 procentry;      
            //    MODULEENTRY32 modentry;

            //    // Transfer Process name into "szToTermUpper" and
            //    // convert it to upper case
            //    iLenP=strlen(szToTerminate);
            //    if(iLenP<1 || iLenP>MAX_PATH) return 632;
            //    for(indx=0;indx<iLenP;indx++)
            //        szToTermUpper[indx]=toupper(szToTerminate[indx]);
            //    szToTermUpper[iLenP]=0;

            //     // PSAPI Function Pointers.
            //     BOOL (WINAPI *lpfEnumProcesses)( DWORD *, DWORD cb, DWORD * );
            //     BOOL (WINAPI *lpfEnumProcessModules)( HANDLE, HMODULE *,
            //        DWORD, LPDWORD );
            //     DWORD (WINAPI *lpfGetModuleBaseName)( HANDLE, HMODULE,
            //        LPTSTR, DWORD );

            //      // ToolHelp Function Pointers.
            //      HANDLE (WINAPI *lpfCreateToolhelp32Snapshot)(DWORD,DWORD) ;
            //      BOOL (WINAPI *lpfProcess32First)(HANDLE,LPPROCESSENTRY32) ;
            //      BOOL (WINAPI *lpfProcess32Next)(HANDLE,LPPROCESSENTRY32) ;
            //      BOOL (WINAPI *lpfModule32First)(HANDLE,LPMODULEENTRY32) ;
            //      BOOL (WINAPI *lpfModule32Next)(HANDLE,LPMODULEENTRY32) ;

            //    // First check what version of Windows we're in
            //    osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
            //    bResult=GetVersionEx(&osvi);
            //    if(!bResult)     // Unable to identify system version
            //        return 606;

            //    // At Present we only support Win/NT/2000/XP or Win/9x/ME
            //    if((osvi.dwPlatformId != VER_PLATFORM_WIN32_NT) &&
            //        (osvi.dwPlatformId != VER_PLATFORM_WIN32_WINDOWS))
            //        return 607;

            //    if(osvi.dwPlatformId==VER_PLATFORM_WIN32_NT)
            //    {
            //        // Win/NT or 2000 or XP

            //         // Load library and get the procedures explicitly. We do
            //         // this so that we don't have to worry about modules using
            //         // this code failing to load under Windows 9x, because
            //         // it can't resolve references to the PSAPI.DLL.
            //         hInstLib = LoadLibraryA("PSAPI.DLL");
            //         if(hInstLib == NULL)
            //            return 605;

            //         // Get procedure addresses.
            //         lpfEnumProcesses = (BOOL(WINAPI *)(DWORD *,DWORD,DWORD*))
            //            GetProcAddress( hInstLib, "EnumProcesses" ) ;
            //         lpfEnumProcessModules = (BOOL(WINAPI *)(HANDLE, HMODULE *,
            //            DWORD, LPDWORD)) GetProcAddress( hInstLib,
            //            "EnumProcessModules" ) ;
            //         lpfGetModuleBaseName =(DWORD (WINAPI *)(HANDLE, HMODULE,
            //            LPTSTR, DWORD )) GetProcAddress( hInstLib,
            //            "GetModuleBaseNameA" ) ;

            //         if(lpfEnumProcesses == NULL ||
            //            lpfEnumProcessModules == NULL ||
            //            lpfGetModuleBaseName == NULL)
            //            {
            //               FreeLibrary(hInstLib);
            //               return 700;
            //            }

            //        bResult=lpfEnumProcesses(aiPID,iCb,&iCbneeded);
            //        if(!bResult)
            //        {
            //            // Unable to get process list, EnumProcesses failed
            //            FreeLibrary(hInstLib);
            //            return 701;
            //        }

            //        // How many processes are there?
            //        iNumProc=iCbneeded/sizeof(DWORD);

            //        // Get and match the name of each process
            //        for(i=0;i<iNumProc;i++)
            //        {
            //            // Get the (module) name for this process

            //            strcpy(szName,"Unknown");
            //            // First, get a handle to the process
            //            hProc=OpenProcess(PROCESS_QUERY_INFORMATION|PROCESS_VM_READ,FALSE,
            //                aiPID[i]);
            //            // Now, get the process name
            //            if(hProc)
            //            {
            //               if(lpfEnumProcessModules(hProc,&hMod,sizeof(hMod),&iCbneeded) )
            //               {
            //                  iLen=lpfGetModuleBaseName(hProc,hMod,szName,MAX_PATH);
            //               }
            //            }
            //            CloseHandle(hProc);
            //            // We will match regardless of lower or upper case
            //#ifdef BORLANDC
            //            if(strcmp(strupr(szName),szToTermUpper)==0)
            //#else
            //            if(strcmp(_strupr(szName),szToTermUpper)==0)
            //#endif
            //            {
            //                // Process found, now terminate it
            //                iFound=1;
            //                // First open for termination
            //                hProc=OpenProcess(PROCESS_TERMINATE,FALSE,aiPID[i]);
            //                if(hProc)
            //                {
            //                    if(TerminateProcess(hProc,0))
            //                    {
            //                        // process terminated
            //                        CloseHandle(hProc);
            //                        FreeLibrary(hInstLib);
            //                        return 0;
            //                    }
            //                    else
            //                    {
            //                        // Unable to terminate process
            //                        CloseHandle(hProc);
            //                        FreeLibrary(hInstLib);
            //                        return 602;
            //                    }
            //                }
            //                else
            //                {
            //                    // Unable to open process for termination
            //                    FreeLibrary(hInstLib);
            //                    return 604;
            //                }
            //            }
            //        }
            //    }

            //    if(osvi.dwPlatformId==VER_PLATFORM_WIN32_WINDOWS)
            //    {
            //        // Win/95 or 98 or ME

            //        hInstLib = LoadLibraryA("Kernel32.DLL");
            //        if( hInstLib == NULL )
            //            return 702;

            //        // Get procedure addresses.
            //        // We are linking to these functions of Kernel32
            //        // explicitly, because otherwise a module using
            //        // this code would fail to load under Windows NT,
            //        // which does not have the Toolhelp32
            //        // functions in the Kernel 32.
            //        lpfCreateToolhelp32Snapshot=
            //            (HANDLE(WINAPI *)(DWORD,DWORD))
            //            GetProcAddress( hInstLib,
            //            "CreateToolhelp32Snapshot" ) ;
            //        lpfProcess32First=
            //            (BOOL(WINAPI *)(HANDLE,LPPROCESSENTRY32))
            //            GetProcAddress( hInstLib, "Process32First" ) ;
            //        lpfProcess32Next=
            //            (BOOL(WINAPI *)(HANDLE,LPPROCESSENTRY32))
            //            GetProcAddress( hInstLib, "Process32Next" ) ;
            //        lpfModule32First=
            //            (BOOL(WINAPI *)(HANDLE,LPMODULEENTRY32))
            //            GetProcAddress( hInstLib, "Module32First" ) ;
            //        lpfModule32Next=
            //            (BOOL(WINAPI *)(HANDLE,LPMODULEENTRY32))
            //            GetProcAddress( hInstLib, "Module32Next" ) ;
            //        if( lpfProcess32Next == NULL ||
            //            lpfProcess32First == NULL ||
            //            lpfModule32Next == NULL ||
            //            lpfModule32First == NULL ||
            //            lpfCreateToolhelp32Snapshot == NULL )
            //        {
            //            FreeLibrary(hInstLib);
            //            return 703;
            //        }

            //        // The Process32.. and Module32.. routines return names in all uppercase

            //        // Get a handle to a Toolhelp snapshot of all the systems processes.

            //        hSnapShot = lpfCreateToolhelp32Snapshot(
            //            TH32CS_SNAPPROCESS, 0 ) ;
            //        if( hSnapShot == INVALID_HANDLE_VALUE )
            //        {
            //            FreeLibrary(hInstLib);
            //            return 704;
            //        }

            //        // Get the first process' information.
            //        procentry.dwSize = sizeof(PROCESSENTRY32);
            //        bResult=lpfProcess32First(hSnapShot,&procentry);

            //        // While there are processes, keep looping and checking.
            //        while(bResult)
            //        {
            //            // Get a handle to a Toolhelp snapshot of this process.
            //            hSnapShotm = lpfCreateToolhelp32Snapshot(
            //                TH32CS_SNAPMODULE, procentry.th32ProcessID) ;
            //            if( hSnapShotm == INVALID_HANDLE_VALUE )
            //            {
            //                CloseHandle(hSnapShot);
            //                FreeLibrary(hInstLib);
            //                return 704;
            //            }
            //            // Get the module list for this process
            //            modentry.dwSize=sizeof(MODULEENTRY32);
            //            bResultm=lpfModule32First(hSnapShotm,&modentry);

            //            // While there are modules, keep looping and checking
            //            while(bResultm)
            //            {
            //                if(strcmp(modentry.szModule,szToTermUpper)==0)
            //                {
            //                    // Process found, now terminate it
            //                    iFound=1;
            //                    // First open for termination
            //                    hProc=OpenProcess(PROCESS_TERMINATE,FALSE,procentry.th32ProcessID);
            //                    if(hProc)
            //                    {
            //                        if(TerminateProcess(hProc,0))
            //                        {
            //                            // process terminated
            //                            CloseHandle(hSnapShotm);
            //                            CloseHandle(hSnapShot);
            //                            CloseHandle(hProc);
            //                            FreeLibrary(hInstLib);
            //                            return 0;
            //                        }
            //                        else
            //                        {
            //                            // Unable to terminate process
            //                            CloseHandle(hSnapShotm);
            //                            CloseHandle(hSnapShot);
            //                            CloseHandle(hProc);
            //                            FreeLibrary(hInstLib);
            //                            return 602;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        // Unable to open process for termination
            //                        CloseHandle(hSnapShotm);
            //                        CloseHandle(hSnapShot);
            //                        FreeLibrary(hInstLib);
            //                        return 604;
            //                    }
            //                }
            //                else
            //                {  // Look for next modules for this process
            //                    modentry.dwSize=sizeof(MODULEENTRY32);
            //                    bResultm=lpfModule32Next(hSnapShotm,&modentry);
            //                }
            //            }

            //            //Keep looking
            //            CloseHandle(hSnapShotm);
            //            procentry.dwSize = sizeof(PROCESSENTRY32);
            //            bResult = lpfProcess32Next(hSnapShot,&procentry);
            //        }
            //        CloseHandle(hSnapShot);
            //    }
            //    if(iFound==0)
            //    {
            //        FreeLibrary(hInstLib);
            //        return 603;
            //    }
            //    FreeLibrary(hInstLib);
            return 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool bAnotherQtr = false;
            if (m_strBIN.Trim() == string.Empty)
            {
                MessageBox.Show("Enter BIN First.");
                bin1.txtTaxYear.Focus();
                return;
            }

            String sQuery, sDatePosted, sOrNo, sTimePosted, m_sORDate;
            DateTime odtCurrentDate;
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            if (m_sPostFormState == "ADD")
                sOrNo = txtOrNo.Text.Trim();
            else
                sOrNo = cmbOrNo.Text.Trim();

            //(s) JJP 08242004 ENHANCED DEFICIENCY TOOLS
            if (AppSettingsManager.OpenValidation("OR NUMBER", m_strBIN) == false)
            {
                if (sOrNo.Trim() == "")
                {
                    MessageBox.Show("Enter OR No", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtOrNo.Focus();
                    return;
                }
            }
            else
            {
                sOrNo = AppSettingsManager.GenerateTempOR();
                if (sOrNo == "")
                    return;
            }
            //(e) JJP 08242004 ENHANCED DEFICIENCY TOOLS

            DateTime cdtCurrentDate;
            cdtCurrentDate = AppSettingsManager.GetSystemDate();
            if (dtpOrDate.Value > cdtCurrentDate)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // to validate if qtrs have values before saving (for REN installment)
            if (btnSave.Text != "&Delete")
            {
                if (txtStat.Text == "REN" && m_sPaymentTerm == "I")
                {
                    if (!chk1st.Checked && !chk2nd.Checked && !chk3rd.Checked && !chk4th.Checked)
                    {
                        MessageBox.Show("Quarter is required", "BPLS - Posting", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }

                if (!chkCash.Checked && !chkCheque.Checked)
                {
                    MessageBox.Show("Select Payment Type", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            String sButtonSave = btnSave.Text;

            odtCurrentDate = AppSettingsManager.GetSystemDate();
            sDatePosted = odtCurrentDate.ToString("MM/dd/yyyy");
            sTimePosted = odtCurrentDate.ToString("m:ss");

            if (sButtonSave == "&Save")
            {
                if ((txtGrandTotal.Text.Trim() == "" || txtGrandTotal.Text.Trim() == "0.00") && txtOrNo.Text.Substring(0, 4) != "TEMP")
                {
                    MessageBox.Show("Cannot save zero values for OR no: '" + txtOrNo.Text.Trim() + "'", "BPLS - Posting", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                SetNoOfQtr();

                if (MessageBox.Show("Save Payment?", "BPLS - Posting", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    String sTeller = " ";
                    try
                    {
                        sTeller = AppSettingsManager.TellerUser.UserCode;
                    }
                    catch { }

                    String m_sBin = m_strBIN;
                    if (m_sPostFormState == "ADD")
                    {
                        //					pApp->m_sTellerID = "";	// JGR 10092005 INITIALIZE TELLER ID

                        pSet.Query = string.Format("delete from pay_hist where bin = '{0}' and data_mode = 'UNP'", m_sBin);
                        pSet.ExecuteNonQuery();
                        pSet.Close();

                        if (txtStat.Text.Trim() == "NEW")
                        {
                            String sTaxYear, sDueState, sQtr, sQtrToPay;
                            pSet.Query = string.Format("select distinct(tax_year),due_state,qtr,qtr_to_pay from pay_temp where bin = '{0}' and fees_totaldue is not null", m_sBin);
                            if (pSet.Execute())
                            {
                                while (pSet.Read())
                                {
                                    m_sORDate = dtpOrDate.Value.ToString("MM/dd/yyyy");

                                    sTaxYear = pSet.GetString("tax_year").Trim();
                                    sDueState = pSet.GetString("due_state").Trim();
                                    sQtr = pSet.GetString("qtr").Trim();
                                    sQtrToPay = pSet.GetString("qtr_to_pay").Trim();

                                    if (sDueState == "Q")
                                        m_sPaymentTerm = "Q";
                                    #region comments
                                    //sQuery = "insert into pay_hist(or_no,bin,bns_stat,tax_year,qtr_paid,no_of_qtr,or_date,payment_term,data_mode,payment_type,date_posted,time_posted,bns_user,teller,memo) values ";
                                    //sQuery += "('" + sOrNo.Trim() + "', ";
                                    //sQuery += " '" + m_sBin + "', ";
                                    //sQuery += " '" + txtStat.Text.Trim() + "', ";
                                    //sQuery += " '" + sTaxYear + "', ";
                                    //sQuery += " '" + sQtr + "', ";
                                    //sQuery += " '" + m_sNoOfQtr + "', ";
                                    //sQuery += " to_date('" + m_sORDate + "','MM/dd/yyyy'), ";
                                    //sQuery += " '" + m_sPaymentTerm.Trim() + "', ";
                                    //sQuery += " '" + m_sMode + "', ";
                                    //sQuery += " '" + m_sPaymentType.Trim() + "', ";
                                    //sQuery += " to_date('" + sDatePosted + "','MM/dd/yyyy'), ";   //LEO 11202003
                                    //sQuery += " '" + sTimePosted + "', ";
                                    //sQuery += " '" + AppSettingsManager.SystemUser.UserCode + "', ";
                                    //sQuery += " '" + sTeller + "', ";
                                    //sQuery += " '" + txtMemo.Text.Trim() + "') ";
                                    #endregion

                                    //JARS 20170922 TEMPORARY TABLE FOR POS PAYMENTS
                                    sQuery = "insert into pos_temp (or_no,bin,bns_stat,tax_year,qtr_paid,no_of_qtr,or_date,payment_term,data_mode,payment_type,date_posted,time_posted,bns_user,teller,memo) values ";
                                    sQuery += "('" + sOrNo.Trim() + "', ";
                                    sQuery += " '" + m_sBin + "', ";
                                    sQuery += " '" + txtStat.Text.Trim() + "', ";
                                    sQuery += " '" + sTaxYear + "', ";
                                    sQuery += " '" + sQtr + "', ";
                                    sQuery += " '" + m_sNoOfQtr + "', ";
                                    sQuery += " to_date('" + m_sORDate + "','MM/dd/yyyy'), ";
                                    sQuery += " '" + m_sPaymentTerm.Trim() + "', ";
                                    sQuery += " '" + m_sMode + "', ";
                                    sQuery += " '" + m_sPaymentType.Trim() + "', ";
                                    sQuery += " to_date('" + sDatePosted + "','MM/dd/yyyy'), ";   //LEO 11202003
                                    sQuery += " '" + sTimePosted + "', ";
                                    sQuery += " '" + AppSettingsManager.SystemUser.UserCode + "', ";
                                    sQuery += " '" + sTeller + "', ";
                                    sQuery += " '" + txtMemo.Text.Trim() + "') ";


                                    pSet1.Query = sQuery;
                                    pSet1.ExecuteNonQuery();

                                    TransLog.UpdateLog(m_sBin, txtStat.Text.Trim(), sTaxYear, m_sPostFormState, m_dTransLogIn, AppSettingsManager.GetSystemDate());   // RMC 20170822 added transaction log feature for tracking of transactions per bin

                                    String sFeesCode, sFeesDue, sFeesSurch, sFeesPen, sFeesAmtDue;
                                    pSet1.Query = string.Format("select * from pay_temp where bin = '{0}' and tax_year = '{1}' and due_state = '{2}' and qtr = '{3}' and qtr_to_pay = '{4}'", m_sBin, sTaxYear, sDueState, sQtr, sQtrToPay);
                                    if (pSet1.Execute())
                                    {
                                        while (pSet1.Read())
                                        {
                                            String sBnsCodeOR;
                                            sFeesCode = pSet1.GetString("fees_code").Trim();
                                            if (sFeesCode.Substring(0, 1) == "B")
                                            {
                                                sBnsCodeOR = sFeesCode.Substring(1, sFeesCode.Length - 1);
                                                sFeesCode = "B";
                                            }
                                            else
                                            {
                                                sBnsCodeOR = txtBnsCode.Text.Trim();
                                            }

                                            sFeesDue = pSet1.GetDouble("fees_due").ToString();
                                            sFeesSurch = pSet1.GetDouble("fees_surch").ToString();
                                            sFeesPen = pSet1.GetDouble("fees_pen").ToString();
                                            sFeesAmtDue = pSet1.GetDouble("fees_totaldue").ToString();

                                            if (Convert.ToDouble(sFeesAmtDue) > 0) // ALJ 04252003 avoid saving record w/ zero values in or
                                            {
                                                #region comments
                                                //sQuery = "insert into or_table(or_no,fees_code,fees_due,fees_surch,fees_pen,fees_amtdue,qtr_paid,bns_code_main,tax_year) values ";
                                                //sQuery += "('" + sOrNo.Trim() + "', ";
                                                //sQuery += " substr('" + sFeesCode + "',1,2), ";
                                                //sQuery += sFeesDue + ", ";    // ALJ 11282003 fixed SQL
                                                //sQuery += sFeesSurch + ", ";  // ALJ 11282003 fixed SQL
                                                //sQuery += sFeesPen + ", ";    // ALJ 11282003 fixed SQL
                                                //sQuery += sFeesAmtDue + ", "; // ALJ 11282003 fixed SQL
                                                //sQuery += " '" + sQtr + "', ";
                                                //sQuery += " '" + sBnsCodeOR + "', ";
                                                //sQuery += " '" + txtTaxYear.Text.Trim() + "') ";
                                                #endregion
                                                //JARS 20170922 TEMPORARY TABLE FOR POS PAYMENTS
                                                sQuery = "insert into pos_or_temp (or_no,fees_code,fees_due,fees_surch,fees_pen,fees_amtdue,qtr_paid,bns_code_main,tax_year) values ";
                                                sQuery += "('" + sOrNo.Trim() + "', ";
                                                sQuery += " substr('" + sFeesCode + "',1,2), ";
                                                sQuery += sFeesDue + ", ";    // ALJ 11282003 fixed SQL
                                                sQuery += sFeesSurch + ", ";  // ALJ 11282003 fixed SQL
                                                sQuery += sFeesPen + ", ";    // ALJ 11282003 fixed SQL
                                                sQuery += sFeesAmtDue + ", "; // ALJ 11282003 fixed SQL
                                                sQuery += " '" + sQtr + "', ";
                                                sQuery += " '" + sBnsCodeOR + "', ";
                                                sQuery += " '" + txtTaxYear.Text.Trim() + "') ";
                                                pSet2.Query = sQuery;
                                                pSet2.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    pSet1.Close();

                                    if (sQtr == "4" || sQtr == "F")
                                    {
                                        pSet1.Query = "delete from payment_conflict where bin = '" + m_sBin + "'";
                                        pSet1.ExecuteNonQuery();
                                    }

                                }
                                AuditTrail.InsertTrail("CSP", "multiple table", m_sBin); //LEO 01292003  CSP - Collectiom Save Posting
                            }
                            pSet.Close();
                        }
                        else //"REN"
                        {
                            m_sORDate = dtpOrDate.Value.ToString("MM/dd/yyyy");

                            #region comments
                            //sQuery = "insert into pay_hist(or_no,bin,bns_stat,tax_year,qtr_paid,no_of_qtr,or_date,payment_term,data_mode,payment_type,date_posted,time_posted,bns_user,teller,memo) values ";	// JGR 09192005 Oracle Adjustment
                            //sQuery += "('" + sOrNo.Trim() + "', ";
                            //sQuery += " '" + m_sBin + "', ";
                            //sQuery += " '" + txtStat.Text.Trim() + "', ";
                            //sQuery += " '" + txtTaxYear.Text.Trim() + "', ";
                            //sQuery += " '" + m_sQtrPaid.Trim() + "', ";
                            //sQuery += " '" + m_sNoOfQtr + "', ";
                            //sQuery += " to_date('" + m_sORDate + "','MM/dd/yyyy'), ";
                            //sQuery += " '" + m_sPaymentTerm + "', ";
                            //sQuery += " '" + m_sMode + "', ";
                            //sQuery += " '" + m_sPaymentType + "', ";
                            //sQuery += " to_date('" + sDatePosted + "','MM/dd/yyyy'), ";
                            //sQuery += " '" + sTimePosted + "', ";
                            //sQuery += " '" + AppSettingsManager.SystemUser.UserCode + "', ";
                            //sQuery += " '" + sTeller + "', ";
                            //sQuery += " '" + txtMemo.Text.Trim() + "') ";
                            #endregion

                            //JARS 20170922 TEMPORARY TABLE FOR POS PAYMENTS
                            sQuery = "insert into pos_temp (or_no,bin,bns_stat,tax_year,qtr_paid,no_of_qtr,or_date,payment_term,data_mode,payment_type,date_posted,time_posted,bns_user,teller,memo) values ";	// JGR 09192005 Oracle Adjustment
                            sQuery += "('" + sOrNo.Trim() + "', ";
                            sQuery += " '" + m_sBin + "', ";
                            sQuery += " '" + txtStat.Text.Trim() + "', ";
                            sQuery += " '" + txtTaxYear.Text.Trim() + "', ";
                            sQuery += " '" + m_sQtrPaid.Trim() + "', ";
                            sQuery += " '" + m_sNoOfQtr + "', ";
                            sQuery += " to_date('" + m_sORDate + "','MM/dd/yyyy'), ";
                            sQuery += " '" + m_sPaymentTerm + "', ";
                            sQuery += " '" + m_sMode + "', ";
                            sQuery += " '" + m_sPaymentType + "', ";
                            sQuery += " to_date('" + sDatePosted + "','MM/dd/yyyy'), ";
                            sQuery += " '" + sTimePosted + "', ";
                            sQuery += " '" + AppSettingsManager.SystemUser.UserCode + "', ";
                            sQuery += " '" + sTeller + "', ";
                            sQuery += " '" + txtMemo.Text.Trim() + "') ";
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();

                            TransLog.UpdateLog(m_sBin, txtStat.Text.Trim(), txtTaxYear.Text.Trim(), m_sPostFormState, m_dTransLogIn, AppSettingsManager.GetSystemDate());   // RMC 20170822 added transaction log feature for tracking of transactions per bin

                            AuditTrail.InsertTrail("CSP", "pay_hist", m_sBin); //LEO 01292003  CSP - Collectiom Save Posting

                            String sFeesCode, sFeesDue, sFeesSurch, sFeesPen, sFeesAmtDue;
                            pSet.Query = string.Format("select * from pay_temp where bin = '{0}' and fees_totaldue is not null", m_sBin); // CTS 09152003
                            if (pSet.Execute())
                            {
                                pSet1.Query = string.Format("delete from taxdues where bin = '{0}'", m_sBin);  // CTS 09152003
                                pSet1.ExecuteNonQuery();

                                while (pSet.Read())
                                {
                                    String sBnsCodeOR;
                                    sFeesCode = pSet.GetString("fees_code").Trim();
                                    if (sFeesCode.Substring(0, 1) == "B")
                                    {
                                        sBnsCodeOR = sFeesCode.Substring(1, sFeesCode.Length - 1);
                                        sFeesCode = "B";
                                    }
                                    else
                                    {
                                        sBnsCodeOR = txtBnsCode.Text.Trim();
                                    }

                                    sFeesDue = pSet.GetDouble("fees_due").ToString();
                                    sFeesSurch = pSet.GetDouble("fees_surch").ToString();
                                    sFeesPen = pSet.GetDouble("fees_pen").ToString();
                                    sFeesAmtDue = pSet.GetDouble("fees_totaldue").ToString();

                                    if (Convert.ToDouble(sFeesAmtDue) > 0) // ALJ 04252003 avoid saving record w/ zero values in or
                                    {
                                        #region comments
                                        //sQuery = "insert into or_table(or_no,fees_code,fees_due,fees_surch,fees_pen,fees_amtdue,qtr_paid,bns_code_main,tax_year) values ";	// JJP 09192005 ORACLE ADJUSTMENT
                                        //sQuery += "('" + sOrNo.Trim() + "', ";
                                        //sQuery += " substr('" + sFeesCode + "',1,2), ";

                                        //// CTS 04122004 (s) set db engine
                                        //sQuery += sFeesDue + ", ";    // ALJ 11282003 fixed SQL
                                        //sQuery += sFeesSurch + ", ";  // ALJ 11282003 fixed SQL 
                                        //sQuery += sFeesPen + ", ";    // ALJ 11282003 fixed SQL
                                        //sQuery += sFeesAmtDue + ", "; // ALJ 11282003 fixed SQL
                                        //sQuery += " '" + m_sQtrPaid + "', ";
                                        //sQuery += " '" + sBnsCodeOR + "', ";
                                        //sQuery += " '" + txtTaxYear.Text.Trim() + "') ";
                                        #endregion
                                        //JARS 20170922 TEMPORARY TABLE FOR POS PAYMENTS
                                        sQuery = "insert into pos_or_temp (or_no,fees_code,fees_due,fees_surch,fees_pen,fees_amtdue,qtr_paid,bns_code_main,tax_year) values ";	// JJP 09192005 ORACLE ADJUSTMENT
                                        sQuery += "('" + sOrNo.Trim() + "', ";
                                        sQuery += " substr('" + sFeesCode + "',1,2), ";
                                        sQuery += sFeesDue + ", ";
                                        sQuery += sFeesSurch + ", ";
                                        sQuery += sFeesPen + ", ";
                                        sQuery += sFeesAmtDue + ", ";
                                        sQuery += " '" + m_sQtrPaid + "', ";
                                        sQuery += " '" + sBnsCodeOR + "', ";
                                        sQuery += " '" + txtTaxYear.Text.Trim() + "') ";
                                        pSet1.Query = sQuery;
                                        pSet1.ExecuteNonQuery();

                                    } // ALJ 04252003

                                    //inserting to taxdues (s) EMS 12082002
                                    String sBillQtrToPay = "0", sBillDueState;
                                    if (m_sPaymentTerm == "I" && m_sQtrPaid != "4")
                                    {
                                        switch (Convert.ToInt16(m_sQtrPaid))
                                        {
                                            case 1:
                                                {
                                                    sBillQtrToPay = "2";
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    sBillQtrToPay = "3";
                                                    break;
                                                }
                                            case 3:
                                                {
                                                    sBillQtrToPay = "4";
                                                    break;
                                                }
                                            default:
                                                {
                                                    break;
                                                }
                                        }

                                        sBillDueState = "R";
                                        String sOrigDue = "0", sFeesTerm;

                                        if (FeesTerm(sFeesCode) == "I")//Q MCR 20150112
                                        {
                                            switch (Convert.ToInt16(m_sNoOfQtr))
                                            {
                                                case 1:
                                                    {
                                                        sOrigDue = Convert.ToString(Convert.ToDouble(sFeesDue) * 4);
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        sOrigDue = Convert.ToString((Convert.ToDouble(sFeesDue) / 2) * 4);
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        sOrigDue = Convert.ToString((Convert.ToDouble(sFeesDue) / 3) * 4);
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        break;
                                                    }
                                            }


                                            if (Convert.ToDouble(sOrigDue) > 0)
                                            {
                                                sQuery = "insert into taxdues(bin,tax_year,qtr_to_pay,bns_code_main,tax_code,amount,due_state) values ";
                                                sQuery += "('" + m_sBin + "', ";
                                                sQuery += " '" + txtTaxYear.Text.Trim() + "', ";
                                                sQuery += " '" + sBillQtrToPay + "', ";
                                                sQuery += " '" + sBnsCodeOR + "', ";
                                                sQuery += " '" + sFeesCode + "', ";
                                                sQuery += sOrigDue + ", "; // ALJ 11282003 fixed SQL
                                                sQuery += " '" + sBillDueState + "') ";

                                                pSet1.Query = sQuery;
                                                pSet1.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }

                                // RMC 20120105 added updating of payment_conflict when year fully paid in installment basis (s)
                                if (m_sQtrPaid == "4" || m_sQtrPaid == "F")	// RMC 20120112 modified deletion in payment_conflict when qtr paid is full
                                {
                                    sQuery = "delete from payment_conflict where bin = '" + m_sBin + "'";
                                    pSet1.Query = sQuery;
                                    pSet1.ExecuteNonQuery();
                                }
                                // RMC 20120105 added updating of payment_conflict when year fully paid in installment basis (e)

                                AuditTrail.InsertTrail("CSP", "multiple table", m_sBin); //LEO 01292003  CSP - Collectiom Save Posting
                            }
                            pSet.Close();
                        }

                        if (dgvDues.CurrentRow.Cells[2].Value.ToString() == "F")
                        {
                            bool bX = TaskMan.IsObjectLock(m_sBin, m_sModule, "DELETE", "");  //LEO 05282003 Task Manager
                            m_sBin = "";
                            btnSearchBin.PerformClick();
                            m_sPaymentTerm = "F";
                            m_sPaymentType = "CS";
                            m_sMode = "POS";
                        }
                        else
                        {
                            m_bTaskManager = false;
                            if (Convert.ToInt16(dgvDues.CurrentRow.Cells[2].Value) < 4)
                            {
                                if (MessageBox.Show("Post another quarter", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    m_bTaskManager = true;
                                    dgvDues.Rows.Clear();

                                    txtOrNo.Text = "";
                                    txtMemo.Text = "";

                                    txtGrandDue.Text = "0.00";
                                    txtGrandSurch.Text = "0.00";
                                    txtGrandPen.Text = "0.00";
                                    txtGrandTotal.Text = "0.00";

                                    DeletePaidQtrNew();
                                    OnlineSearch(bin1.GetBin());
                                    txtOrNo.Focus();
                                    bAnotherQtr = true;
                                }
                                else
                                {
                                    bool bX = TaskMan.IsObjectLock(m_sBin, m_sModule, "DELETE", "");  //LEO 05282003 Task Manager
                                    m_sBin = "";
                                    btnSearchBin.PerformClick();
                                    dgvDues.Rows.Clear();
                                    bAnotherQtr = false;
                                }
                            }
                            else
                            {
                                bool bX = TaskMan.IsObjectLock(m_sBin, m_sModule, "DELETE", "");  //LEO 05282003 Task Manager
                                m_sBin = "";
                                btnSearchBin.PerformClick();
                                dgvDues.Rows.Clear();
                            }
                        }
                    }

                    if (m_sPostFormState == "EDIT")
                    {
                        String sQtr;
                        m_sORDate = dtpOrDate.Value.ToString("MM/dd/yyyy");
                        sTimePosted = dtpOrDate.Value.ToString("m:ss");
                        m_sCurrentTaxYear = dtpOrDate.Value.Year.ToString();

                        pSet.Query = string.Format("delete from or_table where or_no = '{0}' and tax_year = '{1}'", sOrNo, txtTaxYear.Text.Trim());
                        pSet.ExecuteNonQuery();

                        String sFeesCode, sFeesDue, sFeesSurch, sFeesPen, sFeesAmtDue;
                        pSet.Query = string.Format("select * from pay_temp where bin = '{0}'", m_sBin); // CTS 09152003
                        if (pSet.Execute())
                        {
                            while (pSet.Read())
                            {
                                String sBnsCodeOR;
                                sFeesCode = pSet.GetString("fees_code").Trim();
                                if (sFeesCode.Substring(0, 1) == "B")
                                {
                                    sBnsCodeOR = sFeesCode.Substring(1, sFeesCode.Length - 1);
                                    sFeesCode = "B";
                                }
                                else
                                    sBnsCodeOR = txtBnsCode.Text.Trim();	// RMC 20111214 modified saving of bns code in or_table (posting)

                                if (m_sPaymentTerm == "F")
                                {
                                    sQtr = "F";
                                }
                                else
                                {
                                    sQtr = m_sQtrPaid;
                                }

                                sFeesDue = pSet.GetDouble("fees_due").ToString();
                                sFeesSurch = pSet.GetDouble("fees_surch").ToString();
                                sFeesPen = pSet.GetDouble("fees_pen").ToString();
                                sFeesAmtDue = pSet.GetDouble("fees_totaldue").ToString();

                                if (Convert.ToDouble(sFeesAmtDue) > 0) // ALJ 04252003 avoid saving record w/ zero values in or
                                {
                                    sQuery = "insert into or_table(or_no,fees_code,fees_due,fees_surch,fees_pen,fees_amtdue,qtr_paid,bns_code_main,tax_year) values ";	// JJP 09192005 ORACLE ADJUSTMENT
                                    sQuery += "('" + sOrNo + "', ";
                                    sQuery += " substr('" + sFeesCode + "',1,2), ";

                                    // CTS 04122004 (s) set db engine
                                    sQuery += sFeesDue + ", ";    // ALJ 11282003 fixed SQL
                                    sQuery += sFeesSurch + ", ";  // ALJ 11282003 fixed SQL
                                    sQuery += sFeesPen + ", ";    // ALJ 11282003 fixed SQL
                                    sQuery += sFeesAmtDue + ", "; // ALJ 11282003 fixed SQL
                                    sQuery += " '" + sQtr + "', "; //Added by EMS 12152002
                                    sQuery += " '" + sBnsCodeOR + "', ";
                                    sQuery += " '" + txtTaxYear.Text.Trim() + "') ";
                                    pSet1.Query = sQuery;
                                    pSet1.ExecuteNonQuery();
                                    // GDE 20080826 insert into taxdues

                                    String sBillQtrToPay = String.Empty, sBillDueState;
                                    if (m_sPaymentTerm == "I" && m_sQtrPaid != "4")
                                    {
                                        switch (Convert.ToInt16(m_sQtrPaid))
                                        {
                                            case 1:
                                                {
                                                    sBillQtrToPay = "2";
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    sBillQtrToPay = "3";
                                                    break;
                                                }
                                            case 3:
                                                {
                                                    sBillQtrToPay = "4";
                                                    break;
                                                }
                                            default:
                                                {
                                                    break;
                                                }
                                        }
                                        sBillDueState = "R";
                                        String sOrigDue = "0", sFeesTerm;


                                        if (FeesTerm(sFeesCode) == "I")//Q MCR 20150112
                                        {
                                            switch (Convert.ToInt16(m_sNoOfQtr))
                                            {
                                                case 1:
                                                    {
                                                        sOrigDue = Convert.ToString(Convert.ToDouble(sFeesDue) * 4);
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        sOrigDue = Convert.ToString((Convert.ToDouble(sFeesDue) / 2) * 4);
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        sOrigDue = Convert.ToString((Convert.ToDouble(sFeesDue) / 3) * 4);
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        break;
                                                    }
                                            }

                                            // GDE 20080826 accept multi saving of posted record
                                            sQuery = "delete from taxdues where ";
                                            sQuery += "bin = '" + m_sBin + "' ";
                                            sQuery += "and tax_year = '" + txtTaxYear.Text.Trim() + "' ";
                                            sQuery += "and qtr_to_pay <= '" + sBillQtrToPay + "' ";	// RMC 20120110 corrected deletion of prev qtr taxdues when current qtr posted
                                            sQuery += "and bns_code_main = '" + sBnsCodeOR + "' ";
                                            sQuery += "and tax_code = '" + sFeesCode + "' ";
                                            sQuery += "and due_state = '" + sBillDueState + "'";
                                            pSet1.Query = sQuery;
                                            pSet1.ExecuteNonQuery();
                                            // GDE 20080826 accept multi saving of posted record

                                            if (Convert.ToDouble(sOrigDue) > 0)
                                            {
                                                sQuery = "insert into taxdues(bin,tax_year,qtr_to_pay,bns_code_main,tax_code,amount,due_state) values ";
                                                sQuery += "('" + m_sBin + "', ";
                                                sQuery += " '" + txtTaxYear.Text.Trim() + "', ";
                                                sQuery += " '" + sBillQtrToPay + "', ";
                                                sQuery += " '" + sBnsCodeOR + "', ";
                                                sQuery += " '" + sFeesCode + "', ";
                                                sQuery += sOrigDue + ", ";
                                                sQuery += " '" + sBillDueState + "') ";
                                                pSet1.Query = sQuery;
                                                pSet1.ExecuteNonQuery();
                                            }
                                        }
                                        // GDE 20080826 insert into taxdues


                                    } // ALJ 04252003
                                    else if ((m_sPaymentTerm == "I" && m_sQtrPaid == "4") || m_sQtrPaid == "F")	// RMC 20120112 modified deletion in payment_conflict when qtr paid is full
                                    {
                                        sBillDueState = "R";

                                        sQuery = "delete from taxdues where ";
                                        sQuery += "bin = '" + m_sBin + "' ";
                                        sQuery += "and tax_year = '" + txtTaxYear.Text.Trim() + "' ";
                                        sQuery += "and bns_code_main = '" + sBnsCodeOR + "' ";
                                        sQuery += "and tax_code = '" + sFeesCode + "' ";
                                        sQuery += "and due_state = '" + sBillDueState + "'";
                                        pSet1.Query = sQuery;
                                        pSet1.ExecuteNonQuery();

                                        // RMC 20120105 added updating of payment_conflict when year fully paid in installment basis (s)
                                        sQuery = "delete from payment_conflict where bin = '" + m_sBin + "'";
                                        pSet1.Query = sQuery;
                                        pSet1.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        pSet.Close();
                        //(s)//LEO 10022003 update pay_hist table

                        sQuery = string.Format("update pay_hist set or_date = to_date('{0}','MM/dd/yyyy'), memo = '{1}', date_posted = to_date('{2}','MM/dd/yyyy'), bns_user ='{3}' where or_no = '{4}' and bin = '{5}'", m_sORDate, txtMemo.Text.Trim(), sDatePosted, AppSettingsManager.SystemUser.UserCode, sOrNo, m_sBin); //LEO 11202003
                        pSet1.Query = sQuery;
                        pSet1.ExecuteNonQuery();
                        //(e)//LEO 10022003 update pay_hist table

                        TransLog.UpdateLog(m_sBin, txtStat.Text.Trim(), txtTaxYear.Text.Trim(), m_sPostFormState, m_dTransLogIn, AppSettingsManager.GetSystemDate());   // RMC 20170822 added transaction log feature for tracking of transactions per bin

                        if (m_sFile != "")
                            TransferFile(m_sBin);

                        bool bX = TaskMan.IsObjectLock(m_sBin, m_sModule, "DELETE", "");	// RMC 20110617 added end-tasking in posting-edit and delete
                    }

                    String sObject;										// JJP 12032004 AUDIT POSTING
                    sObject = m_sBin + " " + sOrNo + " " + txtTaxYear.Text.Trim();	// JJP 12032004 AUDIT POSTING

                    if (m_sSwTrans == "RECONCILE")
                        AuditTrail.InsertTrail("CPR", "multiple table", sObject);		// JJP 12032004 AUDIT POSTING
                    else
                        AuditTrail.InsertTrail("CUP", "multiple table", sObject);		// JJP 12032004 AUDIT POSTING

                    //(s) JJP Deficiency Tools
                    if (frmDeficientRecords.BIN != null)
                    {
                        frmDeficientRecords.SaveDeficientInfo(pSet);
                        btnDefTools.Enabled = false;
                    }

                    if (m_sPostFormState == "ADD")
                        TransferFile(m_sBin);	// JGR 08102004 TRANSFER IMAGES
                    else
                        if (m_sPostFormState == "EDIT")
                        {
                            if (m_sFile != "")
                                TransferFile(m_sBin);	// JGR 08102004 TRANSFER IMAGES

                            if (m_sSwTrans == "RECONCILE")
                                TransferReconciledRecord(m_sBin); // JGR 12152004 TRANSFER IMAGES OF RECONCILED RECORDS
                            else
                                TransferCorrectedRecords(m_sBin); // JGR 08102004 TRANSFER IMAGES OF CORREDTED RECORDS
                        }
                    m_sBin = "";

                } //Save Payment

                // JGR 11/08/2004 ASK IF POSTING NEW RECORD
                if (m_sPostFormState == "ADD")	// RMC 20110805 corrected message prompting after posting qtr payment
                {
                    MessageBox.Show("Posted Record is now ready for validation.");  // RMC 20171109 transferred message prompt after saving of temp posting
                    if (bAnotherQtr == false)	// RMC 20110805 corrected message prompting after posting qtr payment
                    {
                        SaveImage();
                        if (MessageBox.Show("Post new Record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            bool bX = TaskMan.IsObjectLock(m_strBIN, m_sModule, "DELETE", "");	// JJP 10032005 ORACLE ADJUSTMENT - QA
                            if (AppSettingsManager.GetConfigValue("39") == "Y")
                            {
                                this.LoadImageList();
                                m_sFile = "";
                                //    char szName[100]; strcpy(szName, pApp->m_sViewer);
                                //    int iVal = KILL_PROC_BY_NAME(szName);
                            }
                            else
                            {
                                // pFrm->PostRecordAdd();
                            }
                            //MessageBox.Show("Posted Record is now ready for validation."); //JARS 20170922    // RMC 20171109 transferred message prompt after saving of temp posting, put rem
                        }
                        else
                        {
                            bool bX = TaskMan.IsObjectLock(m_strBIN, m_sModule, "DELETE", "");	// RMC 20110617 
                            this.Dispose();
                            //CDialog::OnOK();
                            //char szName[100]="BravaReader.exe";//C:\Program Files\IGC\Brava! Reader\BravaReader.exe";   // Name of process to terminate
                            //int iVal = KILL_PROC_BY_NAME(szName);	
                        }
                    }
                }
                else
                {
                    SaveImage();
                    if (MessageBox.Show("Edit another record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bool bX = TaskMan.IsObjectLock(m_strBIN, m_sModule, "DELETE", ""); ;

                        btnSearchBin.PerformClick();
                        if (AppSettingsManager.GetConfigValue("39") == "Y")
                        {
                            this.LoadImageList();
                            m_sFile = "";
                            //char szName[100]; strcpy(szName, pApp->m_sViewer);
                            //int iVal = KILL_PROC_BY_NAME(szName);
                        }
                        else
                        {
                            // CDialog::OnOK();
                            // char szName[100]="BravaReader.exe";//C:\Program Files\IGC\Brava! Reader\BravaReader.exe";   // Name of process to terminate
                            // int iVal = KILL_PROC_BY_NAME(szName);
                        }
                    }
                    else
                    {
                        bool bX = TaskMan.IsObjectLock(m_strBIN, m_sModule, "DELETE", "");
                        this.Dispose();
                        //CDialog::OnOK();
                        //char szName[100]="BravaReader.exe";//C:\Program Files\IGC\Brava! Reader\BravaReader.exe";   // Name of process to terminate
                        //int iVal = KILL_PROC_BY_NAME(szName);	
                    }
                }

            }  //**********************
            else //sButtonSave == "&Delete"
            {
                String sLastOrNo = String.Empty;
                //pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' order by tax_year desc,qtr_paid desc", m_strBIN); // CTS 09152003
                pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", m_strBIN);  // RMC 20170127 modified validation in deleting posted payment
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sLastOrNo = pSet.GetString("or_no");
                        m_sNoOfQtr = pSet.GetString("no_of_qtr");
                    }
                    if (cmbOrNo.Text.Trim() != sLastOrNo)
                    {
                        MessageBox.Show("You can only delete the latest OR", "Posting", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        pSet.Close();
                        return;
                    }
                }
                pSet.Close();

                if (MessageBox.Show("Delete Payment?", "BPLS - Posting", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pSet.Query = string.Format("delete from pay_hist where or_no = '{0}' and tax_year = '{1}'", sOrNo, txtTaxYear.Text); // CTS 09152003
                    pSet.ExecuteNonQuery();

                    pSet.Query = string.Format("delete from or_table where or_no = '{0}' and tax_year = '{1}'", sOrNo, txtTaxYear.Text); // JGR 09192005 Oracle Adjustment
                    pSet.ExecuteNonQuery();

                    // ALJ 05162003 (s) to create update taxdues depending on the qtr deleted	   
                    String sQtrPaid, sTaxYear, sMode;
                    String sFeesCode, sFeesDue, sFeesSurch, sFeesPen, sFeesAmtDue, sBnsCodeOR; ;
                    String sBillQtrToPay, sBillDueState = String.Empty, sOrigDue = "0";

                    //pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' order by tax_year desc,qtr_paid desc", m_strBIN); // CTS 09152003
                    pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and data_mode <> 'UNP' order by tax_year desc,qtr_paid desc", m_strBIN);  // RMC 20170127 modified validation in deleting posted payment
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            sQtrPaid = pSet.GetString("qtr_paid");
                            sTaxYear = pSet.GetString("tax_year");
                            sBillQtrToPay = Convert.ToString(Convert.ToDouble(sQtrPaid) + 1);

                            pSet1.Query = string.Format("select * from pay_temp where bin = '{0}' and fees_totaldue is not null", m_strBIN); // CTS 09152003
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                {
                                    if ((m_sPaymentTerm == "I"))
                                    {
                                        pSet2.Query = string.Format("delete from taxdues where bin = '{0}'", m_strBIN); // CTS 09152003
                                        pSet2.ExecuteNonQuery();
                                        pSet2.Close();
                                        //deleting from taxdues before saving another
                                        while (pSet1.Read())
                                        {
                                            sFeesCode = pSet.GetString("fees_code");
                                            if (sFeesCode.Substring(0, 1) == "B")
                                            {
                                                sBnsCodeOR = sFeesCode.Substring(1, sFeesCode.Length);
                                                sFeesCode = "B";
                                            }
                                            else
                                            {
                                                sBnsCodeOR = txtBnsCode.Text.Trim();
                                            }
                                            sFeesDue = pSet.GetDouble("fees_due").ToString();
                                            if (txtStat.Text == "NEW")
                                                sBillDueState = "N";
                                            if (txtStat.Text == "REN")
                                                sBillDueState = "R";
                                            if (txtStat.Text == "RET")
                                                sBillDueState = "X";

                                            if (FeesTerm(sFeesCode) == "I")//Q MCR 20150112
                                            {
                                                switch (Convert.ToInt16(m_sNoOfQtr))
                                                {
                                                    case 1:
                                                        {
                                                            sOrigDue = Convert.ToString(Convert.ToDouble(sFeesDue) * 4);
                                                            break;
                                                        }
                                                    case 2:
                                                        {
                                                            sOrigDue = Convert.ToString((Convert.ToDouble(sFeesDue) / 2) * 4);
                                                            break;
                                                        }
                                                    case 3:
                                                        {
                                                            sOrigDue = Convert.ToString((Convert.ToDouble(sFeesDue) / 3) * 4);
                                                            break;
                                                        }
                                                    default:
                                                        {
                                                            break;
                                                        }
                                                }
                                                if (Convert.ToDouble(sOrigDue) > 0) // ALJ 04252003 to avoid saving zero values in taxdues
                                                {
                                                    sQuery = "insert into taxdues(bin,tax_year,qtr_to_pay,bns_code_main,tax_code,amount,due_state) values ";
                                                    sQuery += "('" + m_strBIN + "', ";
                                                    sQuery += " '" + txtTaxYear.Text + "', ";
                                                    sQuery += " '" + sBillQtrToPay + "', ";
                                                    sQuery += " '" + sBnsCodeOR + "', ";
                                                    sQuery += " '" + sFeesCode + "', ";
                                                    sQuery += sOrigDue + ", "; // ALJ 11282003 fixed SQL
                                                    sQuery += " '" + sBillDueState + "') ";
                                                    pSet2.Query = sQuery;
                                                    pSet2.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                            pSet1.Close();
                        }
                        pSet.Close();
                    }
                    else // no record in pay_hist insert "UNP"
                    {
                        sMode = "UNP";

                        sQuery = "insert into pay_hist(bin,bns_stat,tax_year,data_mode,bns_user) values ";	// JGR 09192005 Oracle Adjustment
                        sQuery += "('" + m_strBIN + "', ";
                        sQuery += " '" + StringUtilities.HandleApostrophe(txtStat.Text) + "', ";
                        sQuery += " '" + StringUtilities.HandleApostrophe(txtTaxYear.Text) + "', ";
                        sQuery += " '" + StringUtilities.HandleApostrophe(sMode) + "', ";
                        sQuery += " '" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode) + "') ";
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();

                        sQuery = string.Format("delete from taxdues where bin = '{0}' and tax_year = '{1}'", m_strBIN, txtTaxYear.Text);
                        pSet.Query = sQuery;
                        pSet.ExecuteNonQuery();
                    }
                    // ALJ 05162003 (s)

                    String sObject;										// JJP 12032004 AUDIT POSTING
                    sObject = m_strBIN + " " + sOrNo + " " + txtTaxYear.Text;	// JJP 12032004 AUDIT POSTING
                    AuditTrail.InsertTrail("CDP", "multiple table", sObject);		// JJP 12032004 AUDIT POSTING

                    TransLog.UpdateLog(m_strBIN, txtStat.Text.Trim(), txtTaxYear.Text.Trim(), m_sPostFormState, m_dTransLogIn, AppSettingsManager.GetSystemDate());   // RMC 20170822 added transaction log feature for tracking of transactions per bin

                    //ClearForm();
                    dgvDues.Rows.Clear();
                    PopulateORCombo();

                    btnDetach.PerformClick();
                    bool bX = TaskMan.IsObjectLock(m_strBIN, m_sModule, "DELETE", "");	// RMC 20110617 added end-tasking in posting-edit and delete
                    MessageBox.Show("Successfully deleted", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnClose.PerformClick();
                }
            }
            txtOrNo.Text = "";
            cmbOrNo.Items.Clear();
            bin1.txtTaxYear.Focus();
        }

        private void cmbOrNo_SelectedValueChanged(object sender, EventArgs e)
        {
            RetrieveInfo(cmbOrNo.Text.Trim());
            DisplayTaxAndDues();
            m_sTempGenOR = cmbOrNo.Text.Trim();  // JJP 08242004 ENHANCED DEFICIENCY TOOLS
            dgvDues_CellValueChanged(dgvDues, new DataGridViewCellEventArgs(1, 0));
            //ComputeTotal();
            CompLessCredit(txtGrandTotal.Text, txtCreditLeft.Text, txtDebitCredit.Text);

            chkCash.Enabled = true; // AST 20150427
            chkCheque.Enabled = true; // AST 20150427
        }

        private void CompLessCredit(String sTotalTotalDue, String sCreditLeft, String sToBeCredited)
        {
            String sGrandTotalDue = "0";
            if (Convert.ToDouble(sTotalTotalDue) > Convert.ToDouble(sToBeCredited))
            {
                sGrandTotalDue = string.Format("{0:#,##0.00}", Convert.ToDouble(sTotalTotalDue) - Convert.ToDouble(sToBeCredited));
                sCreditLeft = string.Format("{0:#,##0.00}", Convert.ToDouble(sCreditLeft) - Convert.ToDouble(sToBeCredited));
            }
            //else
            //{
            //	sGrandTotalDue = sTotalTotalDue;
            //}
            txtGrandTotal.Text = sGrandTotalDue;
            txtCreditLeft.Text = sCreditLeft;
        }

        private void RetrieveInfo(String sORNo)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            PaytempStruct p;

            String sTerm = String.Empty, sQtrPaid = String.Empty, sMemo = String.Empty, sORDate = String.Empty, sMode = String.Empty; // ALJ 05192003 sMode: to filter all "POS" mode
            String sNoofQtr = String.Empty;
            sQuery = string.Format("select distinct(payment_term),qtr_paid,no_of_qtr,memo,or_date,data_mode from pay_hist where bin = '{0}' and tax_year = '{1}' and (data_mode = 'POS' or data_mode = 'ONL' or data_mode = 'OFL') and or_no = '{2}'", m_strBIN, txtTaxYear.Text, sORNo); // JJP 09192005 ORACLE ADJUSTMENT
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sTerm = pSet.GetString("payment_term");
                    sQtrPaid = pSet.GetString("qtr_paid");
                    sNoofQtr = pSet.GetString("no_of_qtr");
                    sMemo = pSet.GetString("memo");
                    sORDate = pSet.GetDateTime("or_date").ToShortDateString();
                    sMode = pSet.GetString("data_mode");
                }
            pSet.Close();

            // ALJ 05192003 (s) sMode: to filter all "POS" mode
            if (sMode != "POS")
            {
                MessageBox.Show("Cannot EDIT or DELETE payments that undergone ONLINE or OFFLINE transaction", "BPLS - Posting", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                OnClearbin();
                return;
            }
            // ALJ 05192003 (e)

            if (sTerm == "F")
            {
                btnFull.Enabled = false;
                btnInstallment.Enabled = false;

                // ALJ 05/14/2003 disable when qtr when full (s)
                chk1st.Enabled = false;
                chk2nd.Enabled = false;
                chk3rd.Enabled = false;
                chk4th.Enabled = false;

                chk1st.Checked = false;
                chk2nd.Checked = false;
                chk3rd.Checked = false;
                chk4th.Checked = false;
                // ALJ 05/14/2003 (e)

                m_sPaymentTerm = "F";
            }
            else //sTerm == 'I'
            {
                //GetDlgItem(IDC_INSTALLMENT)->EnableWindow(1);
                btnFull.Enabled = false;
                btnInstallment.Enabled = false;
                m_sPaymentTerm = "I";

                //// ALJ 07092003 (s) testing
                //if (txtStat.Text == "NEW")
                //    m_sPaymentTerm = "Q";
                //// ALJ 07092003 (s) testing
                switch (Convert.ToInt32(sQtrPaid))
                {
                    case 1:
                        {
                            chk1st.Checked = true;
                            chk2nd.Checked = false;
                            chk3rd.Checked = false;
                            chk4th.Checked = false;

                            chk1st.Enabled = false;
                            chk2nd.Enabled = false;
                            chk3rd.Enabled = false;
                            chk4th.Enabled = false;
                            break;
                        }
                    case 2:
                        {
                            if (sNoofQtr == "2")
                            {
                                chk1st.Checked = true;
                                chk2nd.Checked = true;
                            }
                            else
                            {
                                chk1st.Checked = false;
                                chk2nd.Checked = true;
                            }

                            chk3rd.Checked = false;
                            chk4th.Checked = false;

                            chk1st.Enabled = false;
                            chk2nd.Enabled = false;
                            chk3rd.Enabled = false;
                            chk4th.Enabled = false;
                            break;
                        }
                    case 3:
                        {
                            if (sNoofQtr == "3")
                            {
                                chk1st.Checked = true;
                                chk2nd.Checked = true;
                                chk3rd.Checked = true;
                            }
                            else if (sNoofQtr == "2")
                            {
                                chk1st.Checked = false;
                                chk2nd.Checked = true;
                                chk3rd.Checked = true;
                            }
                            else
                            {
                                chk1st.Checked = false;
                                chk2nd.Checked = false;
                                chk3rd.Checked = true;
                            }
                            chk4th.Checked = false;

                            chk1st.Enabled = false;
                            chk2nd.Enabled = false;
                            chk3rd.Enabled = false;
                            chk4th.Enabled = false;
                            break;
                        }
                    case 4:
                        {
                            if (sNoofQtr == "4")
                            {
                                chk1st.Checked = true;
                                chk2nd.Checked = true;
                                chk3rd.Checked = true;
                                chk4th.Checked = true;
                            }
                            else if (sNoofQtr == "3")
                            {
                                chk1st.Checked = false;
                                chk2nd.Checked = true;
                                chk3rd.Checked = true;
                                chk4th.Checked = true;
                            }
                            else if (sNoofQtr == "2")
                            {
                                chk1st.Checked = false;
                                chk2nd.Checked = false;
                                chk3rd.Checked = true;
                                chk4th.Checked = true;
                            }
                            else
                            {
                                chk1st.Checked = false;
                                chk2nd.Checked = false;
                                chk3rd.Checked = false;
                                chk4th.Checked = true;
                            }

                            chk1st.Enabled = false;
                            chk2nd.Enabled = false;
                            chk3rd.Enabled = false;
                            chk4th.Enabled = false;
                            break;
                        }
                    default:
                        break;
                }
            }

            p.due_state = "";
            sQuery = string.Format("delete from pay_temp where bin = '{0}'", m_strBIN); // CTS 09152003
            pSet.Query = sQuery;
            pSet.ExecuteNonQuery();

            String sBnsCodeMain;
            sQuery = string.Format("select * from or_table where or_no = '{0}'", sORNo); // JGR 09192005 Oracle Adjustment
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    p.fees_code = pSet.GetString("fees_code");
                    p.fees_due = pSet.GetDouble("fees_due").ToString();
                    p.fees_pen = pSet.GetDouble("fees_pen").ToString();
                    p.fees_surch = pSet.GetDouble("fees_surch").ToString();
                    p.fees_totaldue = pSet.GetDouble("fees_amtdue").ToString();
                    p.qtr_to_pay = pSet.GetString("qtr_paid");
                    p.tax_year = pSet.GetString("tax_year");
                    sBnsCodeMain = pSet.GetString("bns_code_main");

                    p.bin = m_strBIN;
                    if (txtStat.Text == "NEW")
                    {
                        if (m_sPaymentTerm == "F") // ALJ 07092003 testing
                            p.due_state = "N";
                        else
                            p.due_state = "I";//Q
                    }
                    if (txtStat.Text == "REN")
                    {
                        p.due_state = "R";
                    }

                    if (txtStat.Text == "RET")
                    {
                        p.due_state = "X";
                    }

                    if (p.fees_code.Substring(0, 1) == "B")
                    {
                        p.fees_desc = AppSettingsManager.GetBnsDesc(sBnsCodeMain);
                        p.fees_code = "B" + sBnsCodeMain.Trim();
                        p.fees_desc = "TAX ON " + p.fees_desc; // ALJ 07092003 (s) testing
                    }
                    else  //if fees
                    {
                        p.fees_desc = AppSettingsManager.GetFeesDesc(p.fees_code);
                    }

                    if (m_sPaymentTerm == "F" && txtStat.Text != "NEW") // ALJ 07092003 && mv_sStatus != "NEW" testing
                    {
                        p.qtr = "F";
                    }
                    else //if m_sPaymentTerm = "I"
                    {
                        p.qtr = sQtrPaid;
                        m_sQtrPaid = sQtrPaid;
                    }
                    p.term = m_sPaymentTerm;

                    // ALJ 11242003 fixed SQL (s)
                    p.fees_due = Convert.ToDouble(p.fees_due).ToString("##0.00");
                    p.fees_surch = Convert.ToDouble(p.fees_surch).ToString("##0.00");
                    p.fees_pen = Convert.ToDouble(p.fees_pen).ToString("##0.00");
                    p.fees_totaldue = Convert.ToDouble(p.fees_totaldue).ToString("##0.00");
                    // ALJ 11242003 fixed SQL (e)

                    sQuery = "insert into pay_temp(bin,tax_year,term,qtr,fees_desc,fees_due,fees_surch,fees_pen,fees_totaldue,fees_code,due_state,qtr_to_pay) values ";
                    sQuery += "('" + p.bin + "', ";
                    sQuery += " '" + p.tax_year + "', ";
                    sQuery += " '" + p.term + "', ";
                    sQuery += " '" + p.qtr + "', ";
                    //sQuery += " '" + StringUtilities.Left(p.fees_desc, 40) + "', "; // ALJ 11282003 fixed SQL temporary solution // AST 20150414
                    // AST 20150414 Added this block(s)
                    if (p.fees_desc.Length > 40)
                        sQuery += " '" + StringUtilities.Left(StringUtilities.HandleApostrophe(p.fees_desc), 40) + "', ";
                    else
                        sQuery += " '" + StringUtilities.HandleApostrophe(p.fees_desc) + "', ";
                    // AST 20150414 Added this block(e)
                    sQuery += p.fees_due + ", ";      // ALJ 11242003 fixed SQL
                    sQuery += p.fees_surch + ", ";    // ALJ 11242003 fixed SQL
                    sQuery += p.fees_pen + ", ";      // ALJ 11242003 fixed SQL
                    sQuery += p.fees_totaldue + ", "; // ALJ 11242003 fixed SQL
                    sQuery += " '" + p.fees_code + "', ";
                    sQuery += " '" + p.due_state + "', ";
                    sQuery += " '" + p.qtr_to_pay + "') ";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
            }
            pSet.Close();

            txtMemo.Text = sMemo;
            dtpOrDate.Text = sORDate;
            dgvDues.Rows.Clear();
            DisplayProper();
        }

        private void OnClearbin()
        {
            //(s)//LEO 05282003 Task Manager
            bool bX = TaskMan.IsObjectLock(m_strBIN, m_sModule, "DELETE", "");	// RMC 20110617 added end-tasking in posting-edit and delete
            m_strBIN = "";
            m_bTaskManager = false;
            //(e)//LEO 05282003 Task Manager
            bin1.txtBINSeries.Text = "";
            bin1.txtTaxYear.Text = "";

            chkPartial.Enabled = false;
            txtCreditLeft.Text = "0.00";
            txtDebitCredit.Text = "0.00";
            txtGrandTotal.Text = "0.00";
            btnDefTools.Enabled = false;  // JJP Deficiency Tools
            btnView.Enabled = false;  // JJP 08112004 ENABLE DISABLE VIEW FILES
            btnSetAside.Enabled = false;   // JJP 08112004 ENABLE DISABLE SET ASIDE
            btnSearchBin.PerformClick();
            cmbOrNo.Items.Clear();// ALJ 04252003 reset OR Combo OnClearbin()
            bin1.txtTaxYear.Focus(); //Added by EMS 12262002
        }

        private void txtOrNo_Leave(object sender, EventArgs e)
        {
            if (m_sPostFormState == "ADD")
            {
                OracleResultSet pSet = new OracleResultSet();
                OracleResultSet pSet1 = new OracleResultSet();
                OracleResultSet pSet2 = new OracleResultSet();
                if (AppSettingsManager.OpenValidation("OR NUMBER", m_strBIN) == false)
                {
                    pSet.Query = string.Format("select distinct * from pay_hist where or_no = '{0}'", txtOrNo.Text.Trim());
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            String sDupWithBIN, sMsg;
                            sDupWithBIN = pSet.GetString("bin");
                            sMsg = "Duplicate OR No. with " + sDupWithBIN.Trim();
                            MessageBox.Show(sMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            txtOrNo.Text = String.Empty;
                            txtOrNo.Focus();
                            pSet.Close();
                            return;
                        }
                        else
                        {
                            // RMC 20171109 added additional validation of OR duplication in pos_temp (s)
                            pSet.Close();
                            pSet.Query = string.Format("select distinct * from pos_temp where or_no = '{0}'", txtOrNo.Text.Trim());
                            if (pSet.Execute())
                            if (pSet.Read())
                            {
                                String sDupWithBIN, sMsg;
                                sDupWithBIN = pSet.GetString("bin");
                                sMsg = "Duplicate OR No. with " + sDupWithBIN.Trim();
                                MessageBox.Show(sMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                txtOrNo.Text = String.Empty;
                                txtOrNo.Focus();
                                pSet.Close();
                                return;
                            }// RMC 20171109 added additional validation of OR duplication in pos_temp (e)
                            else
                            {
                                pSet1.Query = string.Format("select * from or_used where or_no = '{0}'", txtOrNo.Text.Trim());
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                    {
                                        if (AppSettingsManager.IfAutoGeneratedOR() == true) // RMC 20151002 corrected error in posting
                                            pSet2.Query = string.Format("select * from or_assigned where or_code = '{0}' and from_or_no >= '{1}' and to_or_no <= '{1}'", txtOrNo.Text.Trim().Substring(0, 3), txtOrNo.Text.Trim().Substring(txtOrNo.Text.Trim().Length - 6));
                                        else
                                            pSet2.Query = string.Format("select * from or_assigned where from_or_no >= '{0}' and to_or_no <= '{0}'", txtOrNo.Text.Trim()); // RMC 20151002 corrected error in posting                                     
                                        if (pSet2.Execute())
                                            if (pSet2.Read())
                                            {
                                                MessageBox.Show("OR has been used/assigned", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                                txtOrNo.Text = String.Empty;
                                                txtOrNo.Focus();
                                                pSet1.Close();
                                                pSet2.Close();
                                                return;
                                            }
                                        pSet2.Close();
                                    }
                                pSet1.Close();
                            }
                        }
                    pSet.Close();
                }
                else
                {
                    txtOrNo.Text = AppSettingsManager.GenerateTempOR();
                    if (txtOrNo.Text.Trim() == "")
                        return;
                }
            }
        }

        private void txtTaxYear_Leave(object sender, EventArgs e)
        {
            int iTmp = 0;
            int iTmpTaxYear =0;
            int.TryParse(txtTaxYear.Text.Trim(), out iTmp);

            pSetTaxYear.Query = @"select min(tax_year) as tax_year from pay_hist where bin = '" + m_strBIN.Trim() + "'";
            if (pSetTaxYear.Execute())
            {
                if (pSetTaxYear.Read())
                {
                    iTmpTaxYear = Convert.ToInt32(pSetTaxYear.GetString("tax_year"));

                    if (iTmp > iTmpTaxYear)
                    {
                        MessageBox.Show("Tax Year must be lest than " + pSetTaxYear.GetString("tax_year"), " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtTaxYear.Focus();
                        pSetTaxYear.Close();
                        btnSave.Enabled = false;
                        return;
                    }
                    else if (iTmp != (iTmpTaxYear - 1))
                    {
                        MessageBox.Show("Cannot skip dues!\nNo payment found on \nTax Year " + (iTmpTaxYear - 1), " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtTaxYear.Focus();
                        pSetTaxYear.Close();
                        btnSave.Enabled = false;
                        return;
                    }
                }
            }
            pSetTaxYear.Close();

            pSetTaxYear.Query = @"select tax_year From pay_hist where data_mode <> 'UNP' and bin = '" + m_strBIN.Trim() + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
            if (pSetTaxYear.Execute())
                if (pSetTaxYear.Read())
                {
                    MessageBox.Show("Payment Already Exist", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtTaxYear.Focus();
                    btnSave.Enabled = false;
                    return;
                }
                else
                    DisplayTaxAndDues();
            pSetTaxYear.Close();
            btnSave.Enabled = true;
        }
        
        /// <summary>
        /// AST 20150408 Added This Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOtherImages_Click(object sender, EventArgs e)
        {
            string strBIN = bin1.GetBin();

            if (!string.IsNullOrEmpty(strBIN.Trim()))
            {
                ImageInfo objImageInfo = new ImageInfo();
                frmImageList frmImageList = new frmImageList();

                objImageInfo.TRN = strBIN;
                objImageInfo.System = AppSettingsManager.GetSystemType;
                frmImageList.isFortagging = false;
                frmImageList.setImageInfo(objImageInfo);
                frmImageList.Text = strBIN;
                frmImageList.IsAutoDisplay = true;
                frmImageList.Source = "VIEW";
                frmImageList.IsViewOtherImages = true;
                frmImageList.Show(this);
            }
            else
            {
                MessageBox.Show("Please enter BIN first!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}