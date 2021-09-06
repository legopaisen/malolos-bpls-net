using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmTellerReportofCollections : Form
    {
        public frmTellerReportofCollections()
        {
            InitializeComponent();
        }

        bool m_boSwitch = false;
        ArrayList m_sAssignedOrData = new ArrayList();
        ArrayList m_sAssignedOrQty = new ArrayList();

        ArrayList m_sUsedOrData = new ArrayList();
        ArrayList m_sUsedOrQty = new ArrayList();
        ArrayList m_sAmountCollected = new ArrayList();

        string m_sRCD_OR_From = "";
        string m_sRCD_OR_To = "";
        string m_sTotalOR = "";
        string m_sDebit = "";
        string m_sCredit = "";

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void frnTellerReportofCollections_Load(object sender, EventArgs e)
        {
            LoadTeller();
            dtpORDate.Value = AppSettingsManager.GetSystemDate();
            m_boSwitch = false;
        }

        private void LoadTeller()
        {
            String sQuery = string.Format("select teller, ln from tellers order by teller");
            cmbTeller.Items.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbTeller.Items.Add(pSet.GetString("teller"));
                }
            }
            pSet.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private static string GetAmountCollected(string p_sToOr)
        {
            String sQuery, sAmount = "";

            // CTS 03292004 (s) set db engine
            sQuery = string.Format("select sum(fees_amtdue) as amount from or_table where or_no = '{0}'", p_sToOr.Trim());	// JGR 09192005 Oracle Adjustment
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;

            if (pSet.Execute())
                if (pSet.Read())
                    sAmount = pSet.GetDouble("amount").ToString();
            pSet.Close();

            double dDebit = 0, dCredit = 0, m_dTotalDrCr = 0;

            sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no = '{0}' and memo not like 'REMAINING%%'", p_sToOr.Trim()); //JARS 20171010
            pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    dDebit = pSet.GetDouble("xdebit");
                    dCredit = pSet.GetDouble("xcredit");
                    m_dTotalDrCr += (dCredit - dDebit);
                }
            }
            pSet.Close();

            return sAmount;
        }

        private void GetCollections()
        {
            string sQuery;
            string m_sTotalCash = "", m_sTotalCheck = "", m_sDispDebit = "", m_sDispCredit = "", m_sDispTaxCredit = "", m_sDebit = "", m_sCredit = "", m_sTotalCollected = "";
            string sORDate = string.Format("{0:MM/dd/yyyy}", dtpORDate.Value);  // RMC 20150617

            if (m_boSwitch == true) //JARS 20171010
                //sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}') and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
                //sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}') and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", dtpORDate.Value, cmbTeller.Text, txtORFrom.Text, txtORTo.Text);  // RMC 20150501 QA BTAS
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}') and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", sORDate, cmbTeller.Text, txtORFrom.Text, txtORTo.Text);  // RMC 20150617
            else
                //sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}') and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);
                //sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}') and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", dtpORDate.Value, cmbTeller.Text);  // RMC 20150501 QA BTAS
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}') and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", sORDate, cmbTeller.Text);  // RMC 20150617

            double dDebit = 0;
            double dCredit = 0;
            double dTotalDrCr = 0;
            double m_dTotalDbCr = 0;
            double dTotalPayment = 0;
            double dTotalCheck = 0;

            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    dDebit = pSet.GetDouble("xdebit");
                    dCredit = pSet.GetDouble("xcredit");
                    dTotalDrCr = (dCredit - dDebit);
                    m_sDebit = string.Format("{0:##0.00}", dDebit);
                    m_sCredit = string.Format("{0:##0.00}", dCredit);
                    m_dTotalDbCr = dTotalDrCr;
                }
            }
            pSet.Close();

            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no = '{3}') and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}') and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            dDebit = 0;
            dCredit = 0;
            dTotalDrCr = 0;
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    dDebit = pSet.GetDouble("xdebit");
                    dCredit = pSet.GetDouble("xcredit");
                    m_sDebit = string.Format("{0:##0.00}", dDebit);
                    m_sCredit = string.Format("{0:##0.00}", dCredit);
                }
            }
            pSet.Close();

            if (m_boSwitch == true)
                sQuery = string.Format("select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}')", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}')", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment = pSet.GetDouble("amount");
            pSet.Close();

            if (m_boSwitch == true)
                sQuery = string.Format("select sum(chk_amt) as amount, sum(cash_amt) as cash_amt from chk_tbl where or_date = to_date('{0}','MM/dd/yyyy') and  teller = '{1}' and or_no >= '{2}' and or_no <= '{3}'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(chk_amt) as amount, sum(cash_amt) as cash_amt from chk_tbl where or_date = to_date('{0}','MM/dd/yyyy') and  teller = '{1}'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalCheck = pSet.GetDouble("amount");
            pSet.Close();

            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(balance) as balance from dbcr_memo where multi_pay = 'Y' and or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}' and memo not like 'REMAINING%%'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(balance) as balance from dbcr_memo where multi_pay = 'Y' and or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and memo not like 'REMAINING%%'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalCheck += +pSet.GetDouble("balance");
            pSet.Close();

            if (m_boSwitch == true)
                sQuery = string.Format("select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}' and payment_type = 'CS') and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and payment_type = 'CS' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}'))", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment = pSet.GetDouble("amount");
            pSet.Close();

            m_sTotalCash = string.Format("{0:##0.00}", dTotalPayment);

            double dTotalPayment2 = 0, dTotalDebit = 0;

            if (m_boSwitch == true)
                sQuery = string.Format("select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}' and payment_type = 'CSTC') and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and payment_type = 'CSTC' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}'))", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment2 = pSet.GetDouble("amount");
            pSet.Close();

            m_sTotalCash = string.Format("{0:##0.00}", dTotalPayment2);

            if (m_boSwitch == true)
                sQuery = string.Format("select sum(cash_amt) as cash_amount from chk_tbl where or_no in (select or_no from pay_hist where payment_type = 'CCTC' and teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy') and or_no >= '{2}' and or_no <= '{3}')", cmbTeller.Text, dtpORDate.Value.ToShortDateString(), txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(cash_amt) as cash_amount from chk_tbl where or_no in (select or_no from pay_hist where payment_type = 'CCTC' and teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy'))", cmbTeller.Text, dtpORDate.Value.ToShortDateString());

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment2 += pSet.GetDouble("cash_amount");
            pSet.Close();

            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(debit) as amount from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}' and payment_type = 'CSTC') and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(debit) as amount from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and payment_type = 'CSTC') and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment2 = dTotalPayment2 - pSet.GetDouble("amount");
            pSet.Close();

            m_sTotalCash = string.Format("{0:##0.00}", dTotalPayment2);

            //For Payment_type = "CC"
            double dTotalPayment3 = 0, dChkAmt = 0;
            if (m_boSwitch == true)
                sQuery = string.Format("select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}' and payment_type = 'CC') and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and payment_type = 'CC' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}'))", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment3 = pSet.GetDouble("amount");
            pSet.Close();

            if (m_boSwitch == true)
                sQuery = string.Format("select sum(chk_amt) as chk_amount from chk_tbl where or_no in (select or_no from pay_hist where payment_type = 'CC' and teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy') and or_no >= '{2}' and or_no <= '{3}')", cmbTeller.Text, dtpORDate.Value.ToShortDateString(), txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(chk_amt) as chk_amount from chk_tbl where or_no in (select or_no from pay_hist where payment_type = 'CC' and teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy') AND OR_NO NOT IN (SELECT OR_NO FROM RCD_REMIT))", cmbTeller.Text, dtpORDate.Value.ToShortDateString());

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dChkAmt = pSet.GetDouble("chk_amount");
            pSet.Close();

            double dAddCashAmt = 0;

            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}') and memo not like 'REMAINING%%' and memo not like 'REVERS%%' and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}')) and memo not like 'REMAINING%%' and memo not like 'REVERS%%'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalDebit = pSet.GetDouble("xdebit");
            pSet.Close();

            dTotalPayment3 = dTotalPayment3 - dChkAmt;
            dTotalPayment += dTotalPayment2 + dTotalPayment3 + dAddCashAmt;
            m_sTotalCash = string.Format("{0:##0.00}", dTotalPayment);

            double dDiscount = 0;
            String sDiscount = "";
            if (m_boSwitch == true)
                sQuery = string.Format("select sum(discount_amt) as discount from discount_trans a,pay_hist b where a.or_no = b.or_no and or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and b.or_no >= '{2}' and b.or_no <= '{3}' and (payment_type = 'CS' or payment_type = 'CSTC') and b.or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(discount_amt) as discount from discount_trans a,pay_hist b where a.or_no = b.or_no and or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and (payment_type = 'CS' or payment_type = 'CSTC') and b.or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dDiscount = pSet.GetDouble("discount");
            pSet.Close();

            sDiscount = string.Format("{0:##0.00}", dDiscount); //Final Cash Amount
            dTotalPayment = dTotalPayment - dDiscount;
            m_sTotalCash = string.Format("{0:##0.00}", dTotalPayment); //Final Cash Amount

            double dTotalCheck2 = 0;
            if (m_boSwitch == true)
                sQuery = string.Format("select sum(chk_amt) as amount, sum(cash_amt) as cash_amt from chk_tbl where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}' and chk_no not in (select chk_no from multi_check_pay) and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(chk_amt) as amount, sum(cash_amt) as cash_amt from chk_tbl where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}'and chk_no not in (select chk_no from multi_check_pay) and or_no in (select or_no from pay_hist where teller = '{1}' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}'))", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalCheck = pSet.GetDouble("amount");
            pSet.Close();
            m_sTotalCheck = string.Format("{0:##0.00}", dTotalCheck);

            if (m_boSwitch == true)
                sQuery = string.Format("select sum(chk_amt) as amount from multi_check_pay where chk_no in (select chk_no from chk_tbl where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}' and or_no not in (select or_no from rcd_remit)) and trunc(datetime_fld) = to_date('{0}','MM/dd/yyyy') ", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(chk_amt) as amount from multi_check_pay where chk_no in (select chk_no from chk_tbl where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no in (select or_no from pay_hist where teller = '{1}' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}'))) and trunc(datetime_fld) = to_date('{0}','MM/dd/yyyy') ", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalCheck2 = pSet.GetDouble("amount");
            pSet.Close();

            dTotalCheck += dTotalCheck2;
            m_sTotalCheck = string.Format("{0:##0.00}", dTotalCheck);

            double dTDebit = 0, dTCredit = 0, dTDebitS = 0, dTDebitM = 0;
            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}') and memo not like 'REMAINING%%'  and memo not like 'REVERS%%' and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}'  and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}')) and memo not like 'REMAINING%%'  and memo not like 'REVERS%%'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    dTDebit = pSet.GetDouble("xdebit");
                    dTCredit = pSet.GetDouble("xcredit");
                }
            pSet.Close();

            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}' and memo not like 'REMAINING%%' and multi_pay = 'N' and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and memo not like 'REMAINING%%' and multi_pay = 'N' and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTDebitS = pSet.GetDouble("xdebit");
            pSet.Close();

            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}' and memo not like 'REMAINING%%' and multi_pay = 'Y' and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and memo not like 'REMAINING%%' and multi_pay = 'Y' and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dTDebitM = pSet.GetDouble("xdebit");
            pSet.Close();

            double dDDebit = 0, dDCredit = 0;
            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}') and memo not like 'REMAINING%%'  and memo not like 'REVERS%%'  and multi_pay = 'N' and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(debit) as xdebit, sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}'  and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}')) and memo not like 'REMAINING%%'  and memo not like 'REVERS%%'  and multi_pay = 'N'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    dDDebit = pSet.GetDouble("xdebit");
                    dDCredit = pSet.GetDouble("xcredit");
                    m_sDispDebit = string.Format("{0:##0.00}", dDDebit);
                    m_sDispCredit = string.Format("{0:##0.00}", dDCredit);
                }
            pSet.Close();

            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(debit) as xdebit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}') and memo like 'DEBIT%%'  and memo not like 'REVERS%%'  and multi_pay = 'N' and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(debit) as xdebit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}')) and memo like 'DEBIT%%'  and memo not like 'REVERS%%' and multi_pay = 'N'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dDDebit = pSet.GetDouble("xdebit");
            pSet.Close();

            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select * from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}') and memo like 'REMAINING%%' and memo not like 'REVERS%%' and multi_pay = 'N' and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select * from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}')) and memo like 'REMAINING%%'  and memo not like 'REVERS%%' and multi_pay = 'N'", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    string sORNo = pSet.GetString("or_no");
                    double dAddDebit = pSet.GetDouble("credit");

                    sQuery = string.Format("select * from chk_tbl where or_no = '{0}'", sORNo);
                    OracleResultSet pSet1 = new OracleResultSet();
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                    {
                        if (pSet1.Read())
                        {
                            dDDebit += dAddDebit;
                            m_sDispDebit = string.Format("{0:##0.00}", dDDebit);
                        }
                    }
                    pSet1.Close();
                }
            pSet.Close();

            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}') and memo  like 'REMAINING%%'  and multi_pay = 'N'  and chk_no in (select chk_no from chk_tbl where or_date = to_date('{0}','MM/dd/yyyy')) and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}'  and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}')) and memo  like 'REMAINING%%'  and multi_pay = 'N'  and chk_no in (select chk_no from chk_tbl where or_date = to_date('{0}','MM/dd/yyyy'))", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    dTCredit = pSet.GetDouble("xcredit");
                    m_sDispCredit = string.Format("{0:##0.00}", dTCredit);
                }
            pSet.Close();

            double dTCCredit = 0;
            if (m_boSwitch == true) //JARS 20171010
                sQuery = string.Format("select sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}') and memo  like 'REMAINING%%'  and multi_pay = 'N'  and chk_no not in (select chk_no from chk_tbl where or_date = to_date('{0}','MM/dd/yyyy')) and or_no not in (select or_no from rcd_remit)", dtpORDate.Value.ToShortDateString(), cmbTeller.Text, txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format("select sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where or_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}'  and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{1}')) and memo  like 'REMAINING%%'  and multi_pay = 'N'  and chk_no not in (select chk_no from chk_tbl where or_date = to_date('{0}','MM/dd/yyyy'))", dtpORDate.Value.ToShortDateString(), cmbTeller.Text);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    dTCCredit = pSet.GetDouble("xcredit");
                    m_sDispTaxCredit = string.Format("{0:##0.00}", dTCCredit);
                }
            pSet.Close();

            m_sDebit = string.Format("{0:##0.00}", dTDebit);
            m_sCredit = string.Format("{0:##0.00}", dTCredit);

            double dTotalCollected;
            dTotalCollected = (dTotalPayment + dTotalCheck);
            m_sTotalCollected = string.Format("{0:##0.00}", dTotalCollected);
        }

        private int LoadAssignedOr()
        {
            String sQuery, sFromOr, sToOr;
            int iCtr = 0, iRecordCount = 0;
            Boolean bSw = false;

            OracleResultSet pSet = new OracleResultSet();
            sQuery = string.Format("select * from or_assigned where trn_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' order by from_or_no", dtpORDate.Value.ToShortDateString(), StringUtilities.HandleApostrophe(cmbTeller.Text));
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                bSw = true;
                while (pSet.Read())
                {
                    /*sFromOr = pSet.GetInt("from_or_no").ToString();
                    sToOr = pSet.GetInt("to_or_no").ToString();*/

                    // RMC 20150501 QA BTAS (s)
                    sFromOr = pSet.GetString("from_or_no").ToString();
                    sToOr = pSet.GetString("to_or_no").ToString();
                    // RMC 20150501 QA BTAS (e)

                    iRecordCount = (Convert.ToInt32(sToOr) - Convert.ToInt32(sFromOr)) + 1;
                    m_sAssignedOrData.Add(sFromOr + " - " + sToOr);
                    m_sAssignedOrQty.Add(iRecordCount);
                    iCtr++;
                }
            }
            pSet.Close();

            if (bSw)
                iCtr++;

            return iCtr;
        }

        private int LoadUsedOr()
        {
            String sQuery, sFromOr = "", sToOr = "";
            int iCtr = 0, iFromOr = 0, iToOr = 0, iRecordCount = 0, iTmpOr = 0;
            Boolean bSw = false;
            double dAmount = 0;

            if (m_boSwitch == true)
                sQuery = string.Format("select * from or_used where trn_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' and or_no >= '{2}' and or_no <= '{3}' order by or_no", dtpORDate.Value.ToShortDateString(), StringUtilities.HandleApostrophe(cmbTeller.Text), StringUtilities.HandleApostrophe(txtORFrom.Text), StringUtilities.HandleApostrophe(txtORTo.Text));
            else
                sQuery = string.Format("select * from or_used where trn_date = to_date('{0}','MM/dd/yyyy') and teller = '{1}' order by or_no", dtpORDate.Value.ToShortDateString(), StringUtilities.HandleApostrophe(cmbTeller.Text));

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    bSw = true;
                    //sFromOr = pSet.GetInt("or_no").ToString().Trim(); //JARS 20170825 FROM STRING TO INT
                    sFromOr = pSet.GetString("or_no"); //AFM 20200124 new datatype
                    //sFromOr = pSet.GetInt("or_no").ToString().Trim(); // AST 20150427     // RMC 20150501 QA BTAS, put rem
                    //iFromOr = Convert.ToInt32(sFromOr.Substring(2)); //Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427 remove this line
                    iFromOr = Convert.ToInt32(sFromOr); // AST 20150427
                    sToOr = sFromOr;
                    //iToOr = Convert.ToInt32(sFromOr.Substring(2)); //Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427
                    iToOr = Convert.ToInt32(sFromOr); // AST 20150427

                    m_sUsedOrData.Add(sFromOr + " - " + sToOr);
                    m_sUsedOrQty.Add("1");
                    m_sAmountCollected.Add(Convert.ToDouble(GetAmountCollected(sToOr)).ToString("#,##0.00"));

                    //iRecordCount = (Convert.ToInt32(sToOr.Substring(2)) - Convert.ToInt32(sFromOr.Substring(2))) + 1; //Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427 remove this line
                    iRecordCount = (Convert.ToInt32(sToOr) - Convert.ToInt32(sFromOr)) + 1; //Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427
                    while (pSet.Read())
                    {
                        iToOr++;
                        //sToOr = pSet.GetInt("or_no").ToString().Trim(); //JARS 20170825 FROM STRING TO INT
                        sToOr = pSet.GetString("or_no"); //AFM 20200124 new datatype
                        //sToOr = pSet.GetInt("or_no").ToString().Trim(); // AST 20150427   // RMC 20150501 QA BTAS, put rem
                        //iTmpOr = Convert.ToInt32(sToOr.Substring(2));//Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427 remove this line
                        iTmpOr = Convert.ToInt32(sToOr); // AST 20150427

                        if (iToOr == iTmpOr)
                        {
                            String sToOr2;
                            sToOr2 = iTmpOr.ToString();
                            m_sUsedOrData[iCtr] = (sFromOr + " - " + sToOr2);
                            //iRecordCount = (Convert.ToInt32(sToOr.Substring(2)) - Convert.ToInt32(sFromOr.Substring(2))) + 1;//Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427 remove this line
                            iRecordCount = (Convert.ToInt32(sToOr) - Convert.ToInt32(sFromOr)) + 1;//Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427
                            m_sUsedOrQty[iCtr] = iRecordCount;
                        }
                        else
                        {
                            iCtr++;
                            sFromOr = sToOr;
                            sToOr = sFromOr;
                            //iFromOr = Convert.ToInt32(sFromOr.Substring(2));//Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427 remove this line
                            iFromOr = Convert.ToInt32(sFromOr);//Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427
                            //iToOr = Convert.ToInt32(sToOr.Substring(2));//Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427 remove this line
                            iToOr = Convert.ToInt32(sToOr);//Substring(sFromOr.Length - 6) MCR 20140826 // ASt 20150427

                            String sToOr2;
                            sToOr2 = iTmpOr.ToString();
                            m_sUsedOrData.Add(sFromOr + " - " + sToOr2);

                            //iRecordCount = (Convert.ToInt32(sToOr.Substring(2)) - Convert.ToInt32(sFromOr.Substring(2))) + 1;//Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427 remove this line
                            iRecordCount = (Convert.ToInt32(sToOr) - Convert.ToInt32(sFromOr)) + 1;//Substring(sFromOr.Length - 6) MCR 20140826 // AST 20150427
                            m_sUsedOrQty.Add(iRecordCount);
                        }
                    }
                }
            pSet.Close();

            m_sRCD_OR_From = sFromOr;
            m_sRCD_OR_To = sToOr;

            m_sTotalOR = iRecordCount.ToString();

            if (bSw)
                iCtr++;

            return iCtr;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            m_sAssignedOrData.Clear();
            m_sAssignedOrQty.Clear();
            m_sUsedOrData.Clear();
            m_sUsedOrQty.Clear();
            m_sAmountCollected.Clear();

            String m_sDispCredit = "0.00", m_sTotalCollected = "0.00", m_sTotalCash = "0.00", m_sTotalCheck = "0.00", m_sDispTaxCredit = "0.00";
            String sSeries = "", sDenominationTotal, sMonth, sYear, sDay, sTeller;
            String sBillsData, sTotalDebitCredit, sDate = "", sBlank, sBalance, sTotalRemit;
            //String sCurrencyPapers[8],sCurrencyCoins[8];
            int iCtr, iCtrAssignedOr, iCtrUsedOr;
            DateTime cdtDate;
            String sSeries2 = "";

            if (txtORFrom.Text.Trim() != String.Empty && txtORTo.Text.Trim() != String.Empty)
                m_boSwitch = true;
            else
                m_boSwitch = false;

            iCtrAssignedOr = LoadAssignedOr();
            iCtrUsedOr = LoadUsedOr();

            GetCollections();

            int iHigh = iCtrAssignedOr;

            if (iHigh < iCtrUsedOr)
                iHigh = iCtrUsedOr;

            for (iCtr = 0; iCtr < iHigh; iCtr++)
            {
                try
                {
                    if (m_sAssignedOrQty.Count == 0)
                    {
                        m_sAssignedOrQty.Add("");
                        m_sAssignedOrData.Add("");
                    }
                    if (m_sUsedOrQty.Count == 0)
                    {
                        m_sUsedOrQty.Add("");
                        m_sUsedOrData.Add("");
                        m_sAmountCollected.Add("");
                    }

                    if (iCtr == 0)
                    {
                        sSeries = ">611|<1222|>611|<1222|>611|<2222|>611|>1790|<265|>611|<1222;";
                        sSeries2 = ">611|<1222|>611|<1222|>611|<2222|>611|>1790|<265|>611|<1222;";
                    }

                    String sSpace = " ";
                    String aaa, bbb;
                    aaa = m_sRCD_OR_From + " -";
                    bbb = m_sRCD_OR_To + "  ";
                    sSeries += "||||" + sSpace + "|" + sSpace + "|" + m_sUsedOrQty[iCtr] + "|" + aaa + "|||";
                    sSeries2 += "||||" + sSpace + "|" + sSpace + "|" + sSpace + "|" + bbb + "|||";
                }
                catch { }
            }

            String sQuery;
            //sQty = m_sUsedOrQty[iCtr-1].ToString();

            double dDebit = 0, dCredit = 0;
            String sEspasyo, sTotalDeno, sTotalCollection, sTotalDebit, sTotalCredit, sDebitLabel = "", sCreditLabel = "";
            String sTotalCashAmt, sTotalCheckAmt;
            sEspasyo = " ";

            sTotalCollection = ">2500|>2500;";
            sTotalDebit = ">4000|>2000;";
            sTotalCredit = ">4000|>2000;";
            sTotalCashAmt = ">4000|>2000;";
            sTotalCheckAmt = ">4000|>2000;";

            OracleResultSet pSet = new OracleResultSet();
             //JARS 20171010
            sQuery = "select sum(debit) as debit from dbcr_memo where memo like 'DEBITED THRU PAYMENT MADE HAVING OR_NO%' and or_no between '" + m_sRCD_OR_From + "' and '" + m_sRCD_OR_To + "' and served = 'Y' and multi_pay = 'N'";
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dDebit = pSet.GetDouble("debit");
            pSet.Close();

            m_sDebit = dDebit.ToString("##0.00");

            sQuery = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no between '" + m_sRCD_OR_From + "' and '" + m_sRCD_OR_To + "' and served = 'N' and multi_pay = 'N'";
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dCredit = pSet.GetDouble("balance");
            pSet.Close();

            m_sCredit = dCredit.ToString("##0.00");

            String sTotalCollectionLabel, sTotalCashLabel, sTotalCheckLabel;
            sTotalCollectionLabel = "TOTAL COLLECTION :";
            sTotalCashLabel = "TOTAL CASH: ";
            sTotalCheckLabel = "TOTAL CHECK: ";
            sTotalCollection += sTotalCollectionLabel + "|" + Convert.ToDouble(m_sTotalCollected).ToString("#,##0.00");
            sTotalDebit += "LESS: TAX CREDIT|(" + Convert.ToDouble(m_sDebit).ToString("#,##0.00") + ");";//Added by EMS 01022005 // GDE 20120503
            sTotalCredit += "PLUS: EXCESS OF CHECK|" + Convert.ToDouble(m_sCredit).ToString("#,##0.00");//Added by EMS 01022005  // GDE 20120503
            sTotalCashAmt += sTotalCashLabel + "|" + Convert.ToDouble(m_sTotalCash).ToString("#,##0.00");//Added by EMS 01022005
            sTotalCheckAmt += sTotalCheckLabel + "|" + Convert.ToDouble(m_sTotalCheck).ToString("#,##0.00");//Added by EMS 01022005

            String sTaxCredit;
            double dTaxCredit = 0;

            dTaxCredit = Convert.ToDouble(m_sDispCredit);
            sTaxCredit = dTaxCredit.ToString("#,##0.00");
            sTotalCredit += sCreditLabel + "|" + Convert.ToDouble(sTaxCredit).ToString("#,##0.00") + ";";

            String sContentFees;
            String sFeesDesc, sFeesAmt, sFeesCode;
            double dTotFees = 0;
            sContentFees = "^2000|<4000|>2000;";

            // for business tax
            // ENUMERATE BUSINESS TAX IN REPORT

            if (m_boSwitch == true)
                sQuery = string.Format(@"select fees_code, SUBSTR(BNS_CODE_MAIN,1,2), sum(fees_amtdue) from
	or_table where or_no in (select unique or_no from pay_hist where teller = '{0}'
	and or_date = to_date('{1}','MM/dd/yyyy') and or_no between '{2}' and '{3}') and	fees_code = 'B' group by fees_code, 
		SUBSTR(BNS_CODE_MAIN,1,2) order by fees_code, SUBSTR(BNS_CODE_MAIN,1,2)", cmbTeller.Text, dtpORDate.Value.ToShortDateString(), m_sRCD_OR_From, m_sRCD_OR_To);

            else
                sQuery = string.Format(@"select fees_code, SUBSTR(BNS_CODE_MAIN,1,2), sum(fees_amtdue) from
		or_table where or_no in (select unique or_no from pay_hist where teller = '{0}'
		and or_date = to_date('{1}','MM/dd/yyyy')) and	fees_code = 'B' group by fees_code,
		SUBSTR(BNS_CODE_MAIN,1,2) order by fees_code, SUBSTR(BNS_CODE_MAIN,1,2)", cmbTeller.Text, dtpORDate.Value.ToShortDateString());

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                sFeesDesc = "BUSINESS TAX";
                sFeesCode = "B";
                sContentFees += sFeesCode + "|";
                sContentFees += sFeesDesc + "|;;";

                while (pSet.Read())
                {
                    sFeesAmt = pSet.GetDouble(2).ToString();
                    sFeesCode = pSet.GetString(1);

                    sQuery = string.Format("select bns_desc from bns_table where bns_code = '{0}'", sFeesCode);
                    OracleResultSet pSet1 = new OracleResultSet();
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        if (pSet1.Read())
                            sFeesDesc = StringUtilities.HandleApostrophe(pSet1.GetString(0).Trim());
                    pSet1.Close();

                    sFeesCode = "B" + sFeesCode;
                    dTotFees = dTotFees + Convert.ToDouble(sFeesAmt);

                    sContentFees += "         " + sFeesCode + "|";
                    sContentFees += "         " + sFeesDesc + "|";
                    sContentFees += Convert.ToDouble(sFeesAmt).ToString("#,##0.00") + ";";
                }

                sContentFees += ";";
            }
            pSet.Close();

            double dFees = 0, dAmtDue = 0; int iNoEmp = 0;

            if (m_boSwitch == true)
                sQuery = string.Format(@"select a.fees_desc, b.fees_code, sum(b.fees_amtdue) from
			tax_and_fees_table a, or_table b where or_no in (select distinct or_no from
			pay_hist where teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy') and or_no >= '{2}' and or_no <= '{3}') and
			a.fees_code = b.fees_code group by a.fees_desc, b.fees_code order by b.fees_code", cmbTeller.Text, dtpORDate.Value.ToShortDateString(), txtORFrom.Text, txtORTo.Text);
            else
                sQuery = string.Format(@"select a.fees_desc, b.fees_code, sum(b.fees_amtdue) from
			tax_and_fees_table a, or_table b where or_no in (select distinct or_no from
			pay_hist where teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy')) and
			a.fees_code = b.fees_code group by a.fees_desc, b.fees_code order by b.fees_code", cmbTeller.Text, dtpORDate.Value.ToShortDateString());

            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    sFeesDesc = pSet.GetString(0);
                    sFeesCode = pSet.GetString(1);
                    sFeesAmt = pSet.GetDouble(2).ToString();

                    dTotFees += Convert.ToDouble(sFeesAmt);
                    sFeesAmt = Convert.ToDouble(sFeesAmt).ToString("#,##0.00");

                    sContentFees += sFeesCode + "|";
                    sContentFees += sFeesDesc + "|";
                    sContentFees += sFeesAmt + ";;";
                }
            pSet.Close();

            //sTotalDebitCredit.Format("%0.2f", m_dTotalDrCr);
            //sTotalDebitCredit = pApp->PutCommas(pApp->TrimAll(sTotalDebitCredit));

            //sTotalRemit.Format("%0.2f", atof(pApp->RemoveCommas(m_sTotalRemit)) + atof(pApp->RemoveCommas(m_sTotalCheck)));
            //sBalance.Format("%0.2f", atof(m_sTotalCollection) - atof(sTotalRemit));

            sTeller = AppSettingsManager.GetTeller(cmbTeller.Text, 0);

            //(s) Out in VSPrinter
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontSize = 8;
            this.axVSPrinter1.FontSize = 92;

            this.axVSPrinter1.MarginLeft = 600; //1000
            this.axVSPrinter1.MarginTop = 1000;
            this.axVSPrinter1.MarginBottom = 2000;

            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterTop;
            this.axVSPrinter1.FontSize = 8;
            this.axVSPrinter1.Paragraph = "Republic of the Philippines";

            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.FontBold = true;
            //this.axVSPrinter1.Paragraph = AppSettingsManager.GetConfigValue("02");

            // RMC 20150501 QA BTAS (s)
            string sProv = AppSettingsManager.GetConfigValue("08");

            if (sProv.Trim() != "")
                this.axVSPrinter1.Paragraph = "PROVINCE OF " + sProv;
            this.axVSPrinter1.Paragraph = AppSettingsManager.GetConfigValue("09");
            // RMC 20150501 QA BTAS (e)

            this.axVSPrinter1.FontSize = 8;
            this.axVSPrinter1.FontBold = false;

            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                this.axVSPrinter1.Paragraph = "Office of the City Treasurer";
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                this.axVSPrinter1.Paragraph = "Office of the Municipal Treasurer";

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Paragraph = ("TELLER'S REPORT OF COLLECTIONS BY REVENUE ACCOUNT");

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontSize = 8;
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = ("As of " + dtpORDate.Value.ToShortDateString());
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontSize = (10);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            String sHeader;
            sHeader = "^2000|^4000|^2000;ACCOUNT CODE|REVENUE ACCOUNT|TOTAL COLLECTIONS;";
            object y = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = (sHeader);

            this.axVSPrinter1.MarginLeft = 8600;
            sHeader = "^3000;OFFICIAL RECEIPTS;";//3000
            this.axVSPrinter1.CurrentY = y;
            this.axVSPrinter1.Table = (sHeader);
            sHeader = "^1000|^2000;Qty|Series No.;";//1000|^2000
            this.axVSPrinter1.Table = (sHeader);

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.MarginLeft = 600;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = 8;

            y = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.Table = (sContentFees);
            object yy = this.axVSPrinter1.CurrentY;
            this.axVSPrinter1.MarginLeft = 8600;

            this.axVSPrinter1.CurrentY = y;

            for (int iii = 0; iii < m_sUsedOrQty.Count; iii++)
            {
                sHeader = "^1500|<2000;" + m_sUsedOrQty[iii].ToString() + "|" + m_sUsedOrData[iii].ToString();
                this.axVSPrinter1.Table = (sHeader);
            }

            this.axVSPrinter1.CurrentY = yy;
            this.axVSPrinter1.MarginLeft = 2600;
            this.axVSPrinter1.Paragraph = "";

            String sTotFees;
            sTotFees = dTotFees.ToString("#,##0.00");
            sContentFees = ">4000|>2000;TOTAL COLLECTIONS:|" + sTotFees + ";";
            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = 8;
            this.axVSPrinter1.Table = (sContentFees);

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = (sTotalDebit);
            this.axVSPrinter1.Table = (sTotalCredit);
            this.axVSPrinter1.Table = (">4000|>2000;EXCESS OF TAX CREDIT|" + m_sDispTaxCredit + ";");

            this.axVSPrinter1.Paragraph = "";
            double dTotAmt = 0;
            dTotAmt = Convert.ToDouble(sTotFees) - Convert.ToDouble(m_sDebit) + Convert.ToDouble(m_sCredit);	// JGR 01212005
            String sTotCollect;
            sTotCollect = dTotAmt.ToString("#,##0.00");
            this.axVSPrinter1.Table = (sTotCollect);
            sContentFees = "^4000|>2000;NET COLLECTIONS|" + sTotCollect + ";";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = (sContentFees);

            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = false;

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 10;
            this.axVSPrinter1.Table = (sContentFees);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.FontSize = 8;

            sHeader = "<5000|<3000;Submitted By:|Date:;";
            this.axVSPrinter1.Table = (sHeader);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            DateTime odtDate;
            odtDate = AppSettingsManager.GetSystemDate();

            sHeader = "<5000|<3000;" + sTeller + "|" + sDate + ";";
            this.axVSPrinter1.Table = (sHeader);
            sHeader = "<5000|<3000|;TELLER|;";
            this.axVSPrinter1.Table = (sHeader);

            this.axVSPrinter1.Paragraph = "";

            sHeader = "<5000|<3000;Verified By:|Date:;";
            this.axVSPrinter1.Table = (sHeader);

            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";

            sHeader = "<5000|<3000;(Designation)|" + odtDate.Date + ";";
            this.axVSPrinter1.Table = (sHeader);

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // RMC 20150501 QA BTAS
            cmbTeller.Focus();
            btnGenerate_Click(sender, e);
        }
    }
}