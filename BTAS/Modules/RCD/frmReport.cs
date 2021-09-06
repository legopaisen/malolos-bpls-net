using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;
using System.Collections;
using ComponentFactory.Krypton.Toolkit;
using Amellar.Common.LogIn;

namespace Amellar.Modules.RCD
{
    public partial class frmReport : KryptonForm
    {
        // RMC 20140909 Migration QA (s)
        static List<string> m_lstCheckNo = new List<string>();
        static List<string> m_lstBank = new List<string>();
        static List<double> m_lstCheckAmt = new List<double>();
        static string m_sRCDSeries = string.Empty;
        // RMC 20140909 Migration QA (e)

        List<string> lstORLast = new List<string>();    // RMC 20170110 add sub-total per booklet in rcd per OR

        public string m_sOrDate = string.Empty; // RMC 20150106
        private bool m_bNewPage = false;    // RMC 20170117 correction in RCD page 2 table header
        
        public frmReport()
        {
            InitializeComponent();
        }

        OracleResultSet m_Set = new OracleResultSet();
        string m_sUser = "";
        Hashtable htMonth = new Hashtable();

        private void frmReport_Load(object sender, EventArgs e)
        {
            try { m_sUser = AppSettingsManager.SystemUser.UserCode; }
            catch { m_sUser = "SYS_PRO"; }

            if (m_sSwitch != "New")
            {
                btnRemit.Visible = false;
                btnPrint.Text = "Re-print";
            }
            
            if(m_sSwitch == "New")
            {
                btnPrint.Enabled = false;
            }
            m_bNewPage = false; // RMC 20170117 correction in RCD page 2 table header 
            LoadPrint();
        }

        DateTime m_dtDateTrans = new DateTime();
        public DateTime DateTrans
        {
            set { m_dtDateTrans = value; }
        }

        private double m_dCashAmount = 0;
        private double m_dCheckAmount = 0;
        private double m_dTotalCollect = 0;

        bool m_bPrintOnce = false;

        string m_sTellerCode = "";
        public string TellerCode
        {
            set { m_sTellerCode = value; }
        }

        string m_sRCDNo = "";
        public string RCDNo
        {
            get { return m_sRCDNo; }    // RMC 20140909 Migration QA
            set { m_sRCDNo = value; }
        }

        List<string> m_sFormType = new List<string>();
        public List<string> FormType
        {
            set { m_sFormType = value; }
        }

        List<string> m_sORFr = new List<string>();
        public List<string> ORFrom
        {
            set { m_sORFr = value; }
        }

        List<string> m_sORTo = new List<string>();
        public List<string> ORTo
        {
            set { m_sORTo = value; }
        }

        string m_sSwitch = "";
        public string Switch
        {
            set { m_sSwitch = value; }
        }

        List<string> m_lstDenominations = new List<string>();
        public List<string> Denominations
        {
            get { return m_lstDenominations; }
            set { m_lstDenominations = value; }
        }

        List<string> m_lstDenominationsQty = new List<string>();
        public List<string> DenominationsQty
        {
            get { return m_lstDenominationsQty; }
            set { m_lstDenominationsQty = value; }
        }

        List<string> m_lstDenominationsAmt = new List<string>();
        public List<string> DenominationsAmt
        {
            get { return m_lstDenominationsAmt; }
            set { m_lstDenominationsAmt = value; }
        }

        //public static double GetAmount(string From, string To)
        //public static double GetAmount(int From, int To, string ORSeries, string sTmp) // RMC 20140909 Migration QA
        public static double GetAmount(int iFrom, int iTo, string ORSeries, string sTmp)
        {
            try
            {
                double dAmount = 0;
                OracleResultSet result = new OracleResultSet();

                // RMC 20170117 correction in OR Series beginnin in RCD (s)
                string From = string.Empty;
                string To = string.Empty;

                //if (iFrom.ToString().Length < 7) //JARS 20170726 NOT APPLICABLE IN MALOLOS BPLS.NET
                //    From = string.Format("{0:0######}", iFrom);
                //if(iTo.ToString().Length < 7)
                //    To = string.Format("{0:0######}", iTo);
                // RMC 20170117 correction in OR Series beginnin in RCD (e)

                //JARS 20170727
                From = string.Format("{0:0######}", iFrom);
                To = string.Format("{0:0######}", iTo);

                // RMC 20140909 Migration QA (s)
                if (ORSeries.Trim() != "" && AppSettingsManager.GetConfigValue("10") == "021")  // RMC 20141203 corrected error in online payment
                {
                    result.Query = "select nvl(sum(fees_amtdue),0) as tot_fee from or_table where substr(or_no,3,20) ";
                    result.Query += string.Format("between '{0}' and '{1}' and or_no like '{2}%' ", From, To, ORSeries);
                }// RMC 20140909 Migration QA (e)
                else
                {
                    // RMC 20141203 corrected error in online payment (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                        result.Query = string.Format("select nvl(sum(fees_amtdue),0) as tot_fee from or_table where to_number(or_no) between '{0}{2}' and '{1}{2}' ", From, To, ORSeries);
                    else// RMC 20141203 corrected error in online payment (e)
                        //result.Query = string.Format("select nvl(sum(fees_amtdue),0) as tot_fee from or_table where to_number(or_no) between '{0}' and '{1}' ", From, To);
                        result.Query = string.Format("select nvl(sum(fees_amtdue),0) as tot_fee from or_table where or_no between '{0}' and '{1}' ", From, To); // RMC 20150630 corrected conflict if OR no. with letter in posting
                }
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        dAmount = result.GetDouble("tot_fee");
                    }
                }
                result.Close();
                return dAmount;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
            
        }

        //public static double GetCashAmount(string From, string To, string sTeller)
        //public static double GetCashAmount(string sTeller)  // RMC 20140909 Migration QA
        public static double GetCashAmount(string sTeller, string sOrDate)  // RMC 20150106
        {
            double dAmount = 0;
            double dTotalPayment = 0, dTotalPayment2 = 0, dTotalPayment3 = 0, dChkAmt = 0;

            OracleResultSet pSet = new OracleResultSet();
            //pSet.Query = "select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where teller = '" + sTeller + "' and payment_type = 'CS' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + sTeller + "')  and or_date > to_date('12/08/2013','MM/dd/yyyy'))";
            // RMC 20140909 Migration QA (s)
            pSet.Query = "select sum(fees_amtdue) as amount from or_table where or_no in ";
            pSet.Query+= "(select or_no from pay_hist where teller = '" + sTeller + "' and payment_type = 'CS' and data_mode <> 'POS' "; //MCR 20150129 add datamode
            pSet.Query += " and or_date <= TO_DATE('"+sOrDate+"','MM/dd/yyyy')";   // RMC 20150106
            pSet.Query+= "and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + sTeller + "'))";
            // RMC 20140909 Migration QA (e)
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment = pSet.GetDouble("amount");
            pSet.Close();

            //pSet.Query = "select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where teller = '" + sTeller + "' and payment_type = 'CSTC' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + sTeller + "')  and or_date > to_date('12/08/2013','MM/dd/yyyy'))";
            // RMC 20140909 Migration QA (s)
            pSet.Query = "select sum(fees_amtdue) as amount from or_table where or_no in ";
            pSet.Query += "(select or_no from pay_hist where teller = '" + sTeller + "' and payment_type = 'CSTC' and data_mode <> 'POS' and "; //MCR 20150129 add datamode
            pSet.Query += "or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + sTeller + "'))";
            // RMC 20140909 Migration QA (e)
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment2 = pSet.GetDouble("amount");
            pSet.Close();

            //pSet.Query = "select sum(cash_amt) as cash_amount from chk_tbl where or_no in (select or_no from pay_hist where payment_type = 'CCTC' and teller = '" + sTeller + "' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + sTeller + "')  and or_date > to_date('12/08/2013','MM/dd/yyyy'))";
            // RMC 20140909 Migration QA (s)
            pSet.Query = "select sum(cash_amt) as cash_amount from chk_tbl where or_no in ";
            pSet.Query += "(select or_no from pay_hist where payment_type = 'CCTC' and teller = '" + sTeller + "' and data_mode <> 'POS' and "; //MCR 20150129 add datamode
            pSet.Query += "or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + sTeller + "'))";
            // RMC 20140909 Migration QA (e)
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment2 += pSet.GetDouble("cash_amount");
            pSet.Close();

            pSet.Query = "select sum(debit) as amount from dbcr_memo where or_no in "; //JARS 20171010
            pSet.Query += "(select or_no from pay_hist where teller = '" + sTeller + "' and payment_type = 'CSTC' and data_mode <> 'POS' "; //MCR 20150129 add datamode
            //pSet.Query += "and or_no not in (select or_no from rcd_remit where  or_no = pay_hist.or_no and teller = '" + sTeller + "')  and or_date > to_date('12/08/2013','MM/dd/yyyy')) ";
            // RMC 20140909 Migration QA (s)
            pSet.Query += "and or_no not in (select or_no from rcd_remit where  or_no = pay_hist.or_no and teller = '" + sTeller + "')) ";
            // RMC 20140909 Migration QA (e)
            pSet.Query += "and memo not like 'REMAINING%%' and memo not like 'REVERS%%'";
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment2 = dTotalPayment2 - pSet.GetDouble("amount");
            pSet.Close();

            //pSet.Query = "select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where teller = '" + sTeller + "' and payment_type = 'CC' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + sTeller + "')  and or_date > to_date('12/08/2013','MM/dd/yyyy'))";
            // RMC 20140909 Migration QA (s)
            pSet.Query = "select sum(fees_amtdue) as amount from or_table where or_no in ";
            pSet.Query += "(select or_no from pay_hist where teller = '" + sTeller + "' and payment_type = 'CC' and data_mode <> 'POS' and "; //MCR 20150129 add datamode
            pSet.Query+= "or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + sTeller + "'))";
            // RMC 20140909 Migration QA (e)
            if (pSet.Execute())
                if (pSet.Read())
                    dTotalPayment3 = pSet.GetDouble("amount");
            pSet.Close();

            //pSet.Query = "select sum(chk_amt) as chk_amount from chk_tbl where or_no in (select or_no from pay_hist where teller = '" + sTeller + "' and payment_type = 'CC' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + sTeller + "')  and or_date > to_date('12/08/2013','MM/dd/yyyy'))";
            // RMC 20140909 Migration QA (s)
            pSet.Query = "select sum(chk_amt) as chk_amount from chk_tbl where or_no in ";
            pSet.Query += "(select or_no from pay_hist where teller = '" + sTeller + "' and payment_type = 'CC' and data_mode <> 'POS' and "; //MCR 20150129 add datamode
            pSet.Query+= "or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + sTeller + "'))";    
            // RMC 20140909 Migration QA (e)
            if (pSet.Execute())
                if (pSet.Read())
                    dChkAmt = pSet.GetDouble("chk_amount");
            pSet.Close();

            dTotalPayment3 = dTotalPayment3 - dChkAmt;
            dAmount = dTotalPayment + dTotalPayment2 + dTotalPayment3;

            return dAmount;
        }


        public static double GetTotalAmount(string Teller)
        {
            double dTotalAmt = 0;
            try
            {
                OracleResultSet result = new OracleResultSet();
                result.Query = "select sum(fees_amtdue) as tot_fee from or_table where or_no in ";
                result.Query += string.Format("(select or_no from pay_hist where teller = '{0}' and or_no not in ", Teller);
                result.Query += string.Format("(select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '{0}'))", Teller);
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        double.TryParse(result.GetString("tot_fee"), out dTotalAmt);
                    }
                }
                result.Close();
            }
            catch { }
            return dTotalAmt;
        }

        //public static double GetCheckAmount(string strTeller,string sSwitch)
        public static double GetCheckAmount(string strTeller, string sSwitch, string sOrDate)   // RMC 20150106
        {
            // RMC 20140909 Migration QA
            OracleResultSet pSet = new OracleResultSet();
            double dTotCheckAmt = 0;
            double dCheckAmt = 0;
            string sBank = string.Empty;
            m_lstCheckNo = new List<string>();
            m_lstBank = new List<string>();
            m_lstCheckAmt = new List<double>();

            pSet.Query = "select * from chk_tbl where teller = '" + strTeller + "' and chk_no not in ";
            pSet.Query+= "(select chk_no from multi_check_pay) and or_no in ";
            pSet.Query += "(select or_no from pay_hist where teller = '" + strTeller + "' and data_mode <> 'POS'"; //MCR 20150129 add datamode

            if (sSwitch == "New")
            {
                pSet.Query += " and or_date <= TO_DATE('" + sOrDate + "','MM/dd/yyyy')";   // RMC 20150106
                pSet.Query += " and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "')";
            }
            else
                pSet.Query += " and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + m_sRCDSeries + "')";
            pSet.Query += " ) order by or_no";

            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dCheckAmt = pSet.GetDouble("chk_amt");
                    dTotCheckAmt += dCheckAmt;

                    m_lstCheckNo.Add(pSet.GetString("chk_no"));
                    m_lstBank.Add(AppSettingsManager.GetBankBranch(pSet.GetString("bank_code"))); //REM  MCR 20150115   // RMC 20161213 mods in RCD, returned
                    //m_lstBank.Add(pSet.GetString("bank_code"));   // RMC 20161213 mods in RCD, put rem
                    m_lstCheckAmt.Add(dCheckAmt);
                }
            }
            pSet.Close();
//guide
            //pSet.Query = "select * from multi_check_pay where chk_no in ";
            pSet.Query = "select distinct(bank_code), chk_no, chk_amt from multi_check_pay where chk_no in "; //AFM 20200602 specified select to distinct checks with same banks and diff accounts
            pSet.Query += "(select chk_no from chk_tbl where teller = '" + strTeller + "' and multi_check_pay.bank_code = chk_tbl.bank_code and or_no in "; // JAA 20200615 added bank code
            pSet.Query += "(select or_no from pay_hist where teller = '" + strTeller + "' and data_mode <> 'POS' and "; //MCR 20150129 add datamode
            if (sSwitch == "New")
                pSet.Query+= "or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "')))";
            else
                pSet.Query += "or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + m_sRCDSeries + "')))";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dCheckAmt = pSet.GetDouble("chk_amt");
                    dTotCheckAmt += dCheckAmt;

                    m_lstCheckNo.Add(pSet.GetString("chk_no"));
                    m_lstBank.Add(AppSettingsManager.GetBankBranch(pSet.GetString("bank_code"))); //REM MCR 20150115    // RMC 20161213 mods in RCD, returned
                    //m_lstBank.Add(pSet.GetString("bank_code"));   // RMC 20161213 mods in RCD, put rem
                    m_lstCheckAmt.Add(dCheckAmt);
                }
            }
            pSet.Close();

            return dTotCheckAmt;
        }
        

        /*public static double GetCheckAmount(string From, string To, out string CheckNo, out string Bank)
        {
            double dCheckAmt = 0;
            string strCheckNo = "";
            string strBankCode = "";
            string strBankNm = "";
            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("select * from chk_tbl where or_no between '{0}' and '{1}'", From, To);
            if (result.Execute())
            {
                while (result.Read())
                {
                    dCheckAmt = result.GetDouble("chk_amt");
                    strCheckNo = result.GetString("chk_no");
                    strBankCode = result.GetString("bank_code");
                }
            }
            result.Close();

            result.Query = "select bank_nm from bank_table where bank_code = '" + strBankCode + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBankNm = result.GetString("bank_nm");
                }
            }
            result.Close();

            CheckNo = strCheckNo; 
            Bank = strBankNm;
            return dCheckAmt;
        }*/ // RMC 20140909 Migration QA

        public static double GetTaxCredit(string Teller)
        {
            double dTaxCredit = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select sum(fees_amtdue) as tot_fee from or_table where or_no in ";
            result.Query += "(select or_no from pay_hist where teller = '" + Teller + "' and or_no not in ";
            result.Query += "(select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + Teller + "') and payment_type = 'TC')";
            if (result.Execute())
            {
                while (result.Read())
                {
                    double.TryParse(result.GetString("tot_fee"), out dTaxCredit);
                }
            }
            result.Close();
            return dTaxCredit;
        }

        //MCR 20140826 (s)
        public static double GetCredit(string strTeller)
        {
            double dCredit = 0;
            OracleResultSet pSet = new OracleResultSet();
            //pSet.Query = "select sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where teller = '" + strTeller + "' and or_no in (select or_no from pay_hist where teller = '" + strTeller + "' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "'))) and memo not like 'REMAINING%%'  and memo not like 'REVERS%%'  and multi_pay = 'N' and or_date > to_date('12/08/2013','MM/dd/yyyy')";
            pSet.Query = "select sum(credit) as xcredit from dbcr_memo where or_no in (select or_no from pay_hist where teller = '" + strTeller + "' "; //JARS 20171010
            pSet.Query += " and (data_mode = 'ONL' or data_mode = 'OFL')";  // RMC 20150617
            pSet.Query += " and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "')) and memo  like 'REMAINING%%'  and multi_pay = 'N'  and chk_no in (select chk_no from chk_tbl where teller = '" + strTeller + "')";   // RMC 20140909 Migration QA
            
            if (pSet.Execute())
                if (pSet.Read())
                    dCredit = pSet.GetDouble("xcredit");
            pSet.Close();
            return dCredit;
        }

        public static double GetDebit(string strTeller)
        {
            double dDebit = 0;
            OracleResultSet pSet = new OracleResultSet();
            //pSet.Query = "select sum(debit) as xdebit from dbcr_memo where or_no in (select or_no from pay_hist where teller = '" + strTeller + "' and or_no in (select or_no from pay_hist where teller = '" + strTeller + "' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "'))) and memo not like 'REMAINING%%'  and memo not like 'REVERS%%'  and multi_pay = 'N' and or_date > to_date('12/08/2013','MM/dd/yyyy')";
            pSet.Query = "select sum(debit) as xdebit from dbcr_memo where or_no in (select or_no from pay_hist where teller = '" + strTeller + "' "; //JARS 20171010
            pSet.Query += " and (data_mode = 'ONL' or data_mode = 'OFL')";  // RMC 20150617
            pSet.Query += " and or_no in (select or_no from pay_hist where teller = '" + strTeller + "' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "'))) and memo not like 'REMAINING%%'  and memo not like 'REVERS%%'  and multi_pay = 'N'";  // RMC 20140909 Migration QA
            if (pSet.Execute())
                if (pSet.Read())
                    dDebit = pSet.GetDouble("xdebit");
            pSet.Close();
            return dDebit;
        }

        public static double GetDebit(string strTeller, string sOrNo)
        {
            // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination
            double dDebit = 0;
            OracleResultSet pSet = new OracleResultSet();
            
            pSet.Query = "select sum(debit) as xdebit from dbcr_memo where or_no in (select or_no from pay_hist where teller = '" + strTeller + "' "; //JARS 20171010
            pSet.Query += " and (data_mode = 'ONL' or data_mode = 'OFL')";  
            pSet.Query += " and or_no in (select or_no from pay_hist where teller = '" + strTeller + "' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "'))) and memo not like 'REMAINING%%'  and memo not like 'REVERS%%'  and multi_pay = 'N'";  // RMC 20140909 Migration QA
            pSet.Query += " and or_no = '" + sOrNo + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    dDebit = pSet.GetDouble("xdebit");
            pSet.Close();
            return dDebit;
        }

        public static double GetAmount(string strTeller)
        {
            string sQuery = "";
            double dAmount = 0;
            OracleResultSet pSet = new OracleResultSet();
            //sQuery = "select sum(fees_amtdue) as tot_fee from or_table where or_no in (select or_no from pay_hist where teller = '" + strTeller + "'";
            sQuery = "select sum(fees_amtdue) as tot_fee from or_table where EXISTS (select or_no from pay_hist where OR_NO = OR_TABLE.OR_NO AND teller = '" + strTeller + "'"; // AFM 20200618 changed condition for faster query
            //sQuery += " and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "') and or_date > to_date('11/20/2012','MM/dd/yyyy'))";
            sQuery += " and (data_mode = 'ONL' or data_mode = 'OFL')";  // RMC 20150617
            //sQuery += " and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "'))"; // RMC 20140909 Migration QA
            sQuery += " and NOT EXISTS (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "'))"; // AFM 20200618 changed condition for faster query
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dAmount = pSet.GetDouble("tot_fee");
            pSet.Close();
            return dAmount;
        }

        public static double GetTotalCashAmt(string strTeller)
        {
            double dTotamount = 0;
            OracleResultSet pSet = new OracleResultSet();
            /*pSet.Query = "select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where teller = '" + strTeller + "' and payment_type = 'CS' and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "')  and or_date > to_date('12/08/2013','MM/dd/yyyy'))";
            if (pSet.Execute())
                if (pSet.Read())
                    dTotamount = pSet.GetDouble("amount");
            pSet.Close();*/

            // RMC 20140909 Migration QA (s)
            string sOrNo = string.Empty;
            string sPayType = string.Empty;
            OracleResultSet pSet2 = new OracleResultSet();
            dTotamount = 0;
            
            
            /*payment types:
            CS - cash
            CQ - Cheque
            CC - cash and cheque
            CSTC - cash and tax credit
            CQTC - cheque and tax credit
            CCTC - cash, cheque and tax credit*/

            //pSet.Query = "select distinct(or_no), payment_type from pay_hist where (payment_type = 'CS' or payment_type = 'CC') and or_no not in ";
            //pSet.Query = "select distinct(or_no), payment_type from pay_hist where (payment_type = 'CS' or payment_type = 'CC' or payment_type like 'CC%' or payment_type like 'CS%') and or_no not in ";  // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination
            pSet.Query = "select distinct(or_no), payment_type from pay_hist where (payment_type = 'CS' or payment_type = 'CC' or payment_type like 'CC%' or payment_type like 'CS%') and NOT EXISTS ";  // AFM 20200618 changed condition for faster query
            pSet.Query += "(select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "') ";
            pSet.Query += "and teller = '" + strTeller + "'";
            pSet.Query += " and (data_mode = 'ONL' or data_mode = 'OFL')";  // RMC 20150617
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sOrNo = pSet.GetString(0);
                    sPayType = pSet.GetString(1).Trim();

                    //if (sPayType == "CS")
                    if (sPayType == "CS" || sPayType == "CSTC") // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination
                    {
                        pSet2.Query = "select sum(fees_amtdue) as tot_fees from or_table where or_no = '" + sOrNo + "'";
                        if (pSet2.Execute())
                        {
                            if (pSet2.Read())
                                dTotamount += pSet2.GetDouble("tot_fees");
                        }
                        pSet2.Close();

                        // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination (s)
                        if (sPayType == "CSTC")
                            dTotamount -= GetDebit(strTeller, sOrNo);
                        // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination (e)
                    }
                    else
                    {
                        pSet2.Query = "select sum(cash_amt) as tot_csh from chk_tbl where or_no = '" + sOrNo + "' and teller = '" + strTeller + "'";
                        if (pSet2.Execute())
                        {
                            if (pSet2.Read())
                            {
                                dTotamount += pSet2.GetDouble("tot_csh");
                            }
                        }
                        pSet2.Close();
                    }
                }
            }
            pSet.Close();
            // RMC 20140909 Migration QA (e)

            return dTotamount;
        }

        public static double GetExcessCheck(string strTeller)
        {
            string sQuery = "";
            double dExcessCheck = 0;
            OracleResultSet pSet = new OracleResultSet(); //JARS 20171010
            sQuery = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where teller = '" + strTeller + "'";
            //sQuery += " and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "') and or_date > to_date('11/20/2012','MM/dd/yyyy'))  and served = 'N' and multi_pay = 'N'";
            sQuery += " and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "')) and served = 'N' and multi_pay = 'N'";   // RMC 20140909 Migration QA
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dExcessCheck = pSet.GetDouble("balance");
            pSet.Close();
            return dExcessCheck;
        }

        public static double GetAppliedTC(string strTeller)
        {
            string sQuery = "";
            double dAppliedTC = 0;
            OracleResultSet pSet = new OracleResultSet();
            sQuery = "select sum(fees_amtdue) as tot_fee from or_table where or_no in (select or_no from pay_hist where teller = '" + strTeller + "'";
            //sQuery += " and or_date > to_date('11/20/2012','MM/dd/yyyy') and  or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "')";
            sQuery += " and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "')";    // RMC 20140909 Migration QA
            sQuery += " and payment_type = 'TC')";
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dAppliedTC = pSet.GetDouble("tot_fee");
            pSet.Close();
            return dAppliedTC;
        }

        public static double GetAmount(string strTeller, string strRCD, string sTemp)
        {
            string sQuery = "";
            double dAmount = 0;
            OracleResultSet pSet = new OracleResultSet();
            sQuery = "select sum(fees_amtdue) as tot_fee from or_table where or_no in (select or_no from pay_hist where teller = '" + strTeller + "'";
            //sQuery += " and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + strRCD + "') and or_date > to_date('11/20/2012','MM/dd/yyyy'))";
            sQuery += " and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + strRCD + "') and data_mode <> 'POS')";  // RMC 20140909 Migration QA
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dAmount = pSet.GetDouble("tot_fee");
            pSet.Close();
            return dAmount;
        }

        public static double GetTotalCashAmt(string strTeller, string strRCD)
        {
            double dTotamount = 0;
            OracleResultSet pSet = new OracleResultSet();
            /*pSet.Query = "select sum(fees_amtdue) as amount from or_table where or_no in (select or_no from pay_hist where teller = '" + strTeller + "' and payment_type = 'CS' and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + strRCD + "')  and or_date > to_date('12/08/2013','MM/dd/yyyy'))";
            if (pSet.Execute())
                if (pSet.Read())
                    dTotamount = pSet.GetDouble("amount");
            pSet.Close();*/

            // RMC 20140909 Migration QA (s)
            string sOrNo = string.Empty;
            string sPayType = string.Empty;
            OracleResultSet pSet2 = new OracleResultSet();
            dTotamount = 0;

            /*payment types:
            CS - cash
            CQ - Cheque
            CC - cash and cheque
            CSTC - cash and tax credit
            CQTC - cheque and tax credit
            CCTC - cash, cheque and tax credit*/

            //pSet.Query = "select distinct(or_no), payment_type from pay_hist where (payment_type = 'CS' or payment_type = 'CC') and or_no in ";
            pSet.Query = "select distinct(or_no), payment_type from pay_hist where (payment_type = 'CS' or payment_type = 'CC' or payment_type like 'CC%' or payment_type like 'CS%') and or_no in ";   // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination
            pSet.Query += "(select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + strRCD + "') ";
            pSet.Query += "and teller = '" + strTeller + "' and data_mode <> 'POS'"; //MCR 20150130 added data_mode
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sOrNo = pSet.GetString(0);
                    sPayType = pSet.GetString(1).Trim();

                    //if (sPayType == "CS")
                    if (sPayType == "CS" || sPayType == "CSTC") // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination
                    {
                        pSet2.Query = "select sum(fees_amtdue) as tot_fees from or_table where or_no = '" + sOrNo + "'";
                        if (pSet2.Execute())
                        {
                            if (pSet2.Read())
                                dTotamount += pSet2.GetDouble("tot_fees");
                        }
                        pSet2.Close();

                        // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination (s)
                        if (sPayType == "CSTC")
                            dTotamount -= GetAppliedTC(strTeller, strRCD, sOrNo);
                        // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination (e)
                    }
                    else
                    {
                        pSet2.Query = "select sum(cash_amt) as tot_csh from chk_tbl where or_no = '" + sOrNo + "' and teller = '" + strTeller + "'";
                        if (pSet2.Execute())
                        {
                            if (pSet2.Read())
                            {
                                dTotamount += pSet2.GetDouble("tot_csh");
                            }
                        }
                        pSet2.Close();
                    }
                }
            }
            pSet.Close();
            // RMC 20140909 Migration QA (e)

            return dTotamount;
        }

        public static double GetExcessCheck(string strTeller, string strRCD)
        {
            string sQuery = "";
            double dExcessCheck = 0;
            OracleResultSet pSet = new OracleResultSet();
            /*sQuery = "select sum(balance) as balance from dbcr_memo where memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%' and or_no in (select or_no from pay_hist where teller = '" + strTeller + "'";
            sQuery += " and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + strRCD + "') and or_date > to_date('11/20/2012','MM/dd/yyyy'))  and served = 'N' and multi_pay = 'N'";*/
            pSet.Query = "select sum(credit) as balance from dbcr_memo where or_no in (select or_no from pay_hist where teller = '" + strTeller + "' "; //JARS 20171010
            pSet.Query += " and (data_mode = 'ONL' or data_mode = 'OFL')";  // RMC 20150617
            pSet.Query += " and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + strRCD + "')) and memo  like 'REMAINING%%'  and multi_pay = 'N'  and chk_no in (select chk_no from chk_tbl where teller = '" + strTeller + "')";     // RMC 20140909 Migration QA
            if (pSet.Execute())
                if (pSet.Read())
                    dExcessCheck = pSet.GetDouble("balance");
            pSet.Close();
            return dExcessCheck;
        }

        public static double GetAppliedTC(string strTeller, string strRCD)
        {
            string sQuery = "";
            double dAppliedTC = 0;
            OracleResultSet pSet = new OracleResultSet();
            /*sQuery = "select sum(fees_amtdue) as tot_fee from or_table where or_no in (select or_no from pay_hist where teller = '" + strTeller + "'";
            //sQuery += " and or_date > to_date('11/20/2012','MM/dd/yyyy') and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + strRCD + "')";
            sQuery += " and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + strRCD + "')";   // RMC 20140909 Migration QA
            sQuery += " and payment_type = 'TC')";*/

            pSet.Query = "select sum(debit) as tot_fee from dbcr_memo where or_no in (select or_no from pay_hist where teller = '" + strTeller + "' "; //JARS 20171010
            pSet.Query += " and (data_mode = 'ONL' or data_mode = 'OFL')";  // RMC 20150617
            pSet.Query += " and or_no in (select or_no from pay_hist where teller = '" + strTeller + "' and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + strRCD + "'))) and memo not like 'REMAINING%%'  and memo not like 'REVERS%%'  and multi_pay = 'N'";  // RMC 20140909 Migration QA
            if (pSet.Execute())
                if (pSet.Read())
                    dAppliedTC = pSet.GetDouble("tot_fee");
            pSet.Close();
            return dAppliedTC;
        }

        //MCR 20140826 (e)

        public static double GetAppliedTC(string strTeller, string strRCD, string sORNo)
        {
            // RMC 20151209 corrected rcd display of cash in cash, cheque, tax credit payment combination
            string sQuery = "";
            double dAppliedTC = 0;
            OracleResultSet pSet = new OracleResultSet();
            
            pSet.Query = "select sum(debit) as tot_fee from dbcr_memo where or_no in (select or_no from pay_hist where teller = '" + strTeller + "' "; //JARS 20171010
            pSet.Query += " and (data_mode = 'ONL' or data_mode = 'OFL')";  
            pSet.Query += " and or_no in (select or_no from pay_hist where teller = '" + strTeller + "' and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + strTeller + "' and rcd_series = '" + strRCD + "'))) and memo not like 'REMAINING%%'  and memo not like 'REVERS%%'  and multi_pay = 'N'";
            pSet.Query += " and or_no = '" + sORNo + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    dAppliedTC = pSet.GetDouble("tot_fee");
            pSet.Close();
            return dAppliedTC;
        }

        public string GetRCDSeries()
        {
            int iRCDSeries = 0;
            string strRCDSeries = "";
            OracleResultSet result = new OracleResultSet();
            if (m_sSwitch == "New")
            {
                //result.Query = "select rcd_series from rcd_series";
                // RMC 20150107 (s)
                string sRCDCode = string.Empty;
                result.Query = "select * from rcd_series";  
                result.Query += " where teller = '" + m_sTellerCode + "'";  
                // RMC 20150107 (e)

                // AFM 20200122 merged from luna version (s)
                // JAA 20191004 added LUN-19-10952 - reset to 1 after a year (s) 
                OracleResultSet rs = new OracleResultSet();
                string sYr = string.Empty;
                sYr = string.Format("{0:yyyy}", AppSettingsManager.GetCurrentDate());
                rs.Query = "select nvl(max(rcd_series),0) as rcd_series from rcd_series where year = '" + sYr + "'";
                rs.Query += "and teller = '" + m_sTellerCode + "'";
                // JAA 20191004 added LUN-19-10952  - reset to 1 after a year (e) 
                // AFM 20200122 merged from luna version (e)

                if (result.Execute())
                {
                    if (result.Read())
                    {
                        int.TryParse(result.GetString("rcd_series"), out iRCDSeries);
                        iRCDSeries++;
                        //strRCDSeries = iRCDSeries.ToString("0000000000");
                        strRCDSeries = iRCDSeries.ToString("0000000");  // RMC 20150501 QA BTAS
                        sRCDCode = result.GetString("rcd_code");    // RMC 20150107
                    }
                    else
                    {
                        // RMC 20141203 corrected error in online payment (s)
                        iRCDSeries = 1;
                        //strRCDSeries = iRCDSeries.ToString("0000000000");
                        strRCDSeries = iRCDSeries.ToString("0000000");   // RMC 20150501 QA BTAS
                        // RMC 20141203 corrected error in online payment (e)
                        sRCDCode = result.GetString("rcd_code");    // RMC 20150107

                        // AFM 20200122 merged from luna version (s)
                        // JAA 20190109 added saving in rcd series (s) 
                        string sMonth2 = string.Format("{0:MM}", AppSettingsManager.GetCurrentDate());
                        OracleResultSet pSetBpls = new OracleResultSet();
                        pSetBpls.Query = "insert into rcd_series(rcd_series,teller,rcd_code,month,year) values ";
                        pSetBpls.Query += "('" + iRCDSeries + "', ";
                        pSetBpls.Query += "'" + m_sTellerCode + "', ";
                        pSetBpls.Query += "'" + sRCDCode + "', '" + sMonth2 + "', ";
                        pSetBpls.Query += "'" + sYr + "' )"; // JAA 20191004 added Year for LUN-19-10952
                        if (pSetBpls.ExecuteNonQuery() == 0)
                        { }
                        // JAA 20190109 added saving in rcd series (e)
                        // AFM 20200122 merged from luna version (e)
                    }
                }
                result.Close();

                // RMC 20140909 Migration QA (s)
                result.Query = "update rcd_series set rcd_series = " + iRCDSeries + "";
                result.Query += " where teller = '" + m_sTellerCode + "'";  // RMC 20150107
                if (result.ExecuteNonQuery() == 0)
                { }
                // RMC 20140909 Migration QA (e)

                // RMC 20150107
                string sYear = StringUtilities.Right(ConfigurationAttributes.CurrentYear,2);
                string sMonth = string.Format("{0:MM}", AppSettingsManager.GetCurrentDate());
                strRCDSeries = sRCDCode + "-" + sYear + "-" + sMonth + "-" + strRCDSeries;
                // RMC 20150107
            }
            else
            {
                result.Query = "select * from partial_remit where rcd_series = '" + m_sRCDNo + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        int.TryParse(result.GetString("rcd_series"), out iRCDSeries);
                        iRCDSeries++;
                        //strRCDSeries = iRCDSeries.ToString("0000000000");
                        strRCDSeries = iRCDSeries.ToString("0000000");   // RMC 20150501 QA BTAS
                    }
                }
                result.Close();
            }
            return strRCDSeries;
        }

        //public static string GetTeller()
        public string GetTeller()
        {
            // rmc 20150107
            string strTeller = string.Empty;
            strTeller = Remittance.GetTellerName(m_sTellerCode);
            // rmc 20150107

            /*OracleResultSet result = new OracleResultSet();
            result.Query = "select ln, fn, mi from tellers";
            if (result.Execute())
            {
                while (result.Read())
                {
                    strTeller = result.GetString("fn") + " ";
                    strTeller += result.GetString("mi") + ". ";
                    strTeller += result.GetString("ln");
                }
            }
            result.Close();*/

            return strTeller;
        }

        private bool ValidateORTo(string strORFr, string strORTo, string strORMax)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select from_or_no, to_or_no from or_assigned_hist where from_or_no = '" + strORFr + "' and from_or_no <= '" + strORTo + "' and to_or_no = '" + strORMax + "' ";
            result.Query += "and teller not in (select teller from rcd_remit where teller = '" + m_sTellerCode + "' and or_no ";
            result.Query += "between or_assigned_hist.from_or_no and or_assigned_hist.to_or_no ) and teller = '" + m_sTellerCode + "' order by from_or_no asc";
            if (result.Execute())
            {
                while (result.Read())
                {
                    result.Close();
                    return true;
                }
            }
            result.Close();
            return false;
        }

        private void LoadPrint()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();
            OracleResultSet rs2 = new OracleResultSet();
            OracleResultSet rs3 = new OracleResultSet();

            List<String> lstORFr = new List<String>();
            List<String> lstORTo = new List<String>();

            string strORFr = "";
            string strORTo = "";
            string strORSeries = ""; //MCR 20140820
            string strCheckNo = "";
            string strCheckNoTmp = "";
            string strBank = "";
            double dCheckAmount = 0;
            double dCashAmount = 0;
            double dAmount = 0;
            double dTotalAmount = 0;
            double dTaxCredit = 0;
            double dExcessCheck = 0;
            double dAppTC = 0;
            int iMinOR = 0;
            int iMaxOR = 0;
            string strMinOR = "";
            string strMaxOR = "";
            int iORQty = 0;
            string strTmpORTO = "";
            string strTmpORFROM = "";
            string sOrSeriesQuery = ""; //JARS 20171016
            int iTmpQty = 0;
            int iTmpORFr = 0;
            int iTmpORTo = 0;

            List<string> lstCheckNo = new List<string>();
            List<string> lstBank = new List<string>();
            List<double> lstCheckAmt = new List<double>();

            List<string> strReceiptsQty = new List<string>();
            List<string> strReceiptsFr_OR_No = new List<string>();
            List<string> strReceiptsTo_OR_No = new List<string>();

            List<string> strIssuedQty = new List<string>();
            List<string> strIssuedFr_OR_No = new List<string>();
            List<string> strIssuedTo_OR_No = new List<string>();

            List<string> strEndingQty = new List<string>();
            List<string> strEndingFr_OR_No = new List<string>();
            List<string> strEndingTo_OR_No = new List<string>();

            GetMonthString();
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            axVSPrinter1.MarginLeft = 1000;
            axVSPrinter1.MarginRight = 1000;
            axVSPrinter1.MarginTop = 500;
            axVSPrinter1.MarginBottom = 500;
            axVSPrinter1.StartDoc();

            //axVSPrinter1.Paragraph = "";
            axVSPrinter1.FontBold = true;

            axVSPrinter1.FontSize = (float)10;
            axVSPrinter1.Table = string.Format("<11000;{0}", "A.COLLECTION");
            axVSPrinter1.FontBold = false;

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            axVSPrinter1.Table = string.Format("<5500|<5500;{0}", "X 1. FOR COLLECTORS|2. FOR LIQUIDATION OFFICERS/TREASURER");

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColumns;
            axVSPrinter1.Table = string.Format("^0|^2200|^2200|^4400|^2200|0;{0}", "|NAME OF | NO. OF | INCLUSIVE SERIAL NUMBERS||");

            //(widthleft, heightleft, widthright, heightright)
            /*if (m_sSwitch == "New")
                axVSPrinter1.DrawLine(4900, 2600, 9300, 2600); // Add line on inclusive serial numbers
            else
                axVSPrinter1.DrawLine(4900, 2900, 9300, 2900); */// Add line on inclusive serial numbers
            //axVSPrinter1.DrawLine(4900, 2750, 9300, 2750);  // RMC 20150106
            //axVSPrinter1.DrawLine(4900, 3250, 9300, 3250);  // RMC 20150501 QA BTAS
            axVSPrinter1.Table = string.Format("^0|^2200|^2200|^2200|^2200|^2200|^0;{0}", "|ACCOUNTABLE | O.R. | FROM | TO | AMOUNT |");

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColumns;
            axVSPrinter1.Table = string.Format("^0|^2200|^2200|^2200|^2200|^2200|^0;{0}", "|FORMS | ISSUED | | | ");
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

            axVSPrinter1.FontSize = (float)8;
            //axVSPrinter1.FontSize = (float)10;  // RMC 20150107
           
            int iRowCtr = 0;    // RMC 20150501 QA BTAS
            if (m_sSwitch == "New")
            {
                //MCR 20140820
                /*esult.Query = "select or_assigned_hist.* from or_assigned_hist,pay_hist where pay_hist.or_no between or_assigned_hist.or_series||or_assigned_hist.from_or_no and or_assigned_hist.or_series||or_assigned_hist.to_or_no ";
                result.Query += string.Format("and or_assigned_hist.trn_date >= pay_hist.or_date and or_assigned_hist.teller ");
                result.Query += string.Format("not in (select teller from rcd_remit where teller = '{0}' and or_no between or_assigned_hist.or_series||or_assigned_hist.from_or_no and or_assigned_hist.or_series||or_assigned_hist.to_or_no) ", m_sTellerCode);
                result.Query += string.Format("and or_assigned_hist.teller = '{0}' order by or_assigned_hist.from_or_no asc", m_sTellerCode);*/
                // RMC 20140909 Migration QA (s)
                m_sRCDSeries = "";
                //result.Query = "select distinct from_or_no, to_or_no";
                result.Query = "select from_or_no, to_or_no ";   // RMC 20150915 corrections in print of RCD
                if (AppSettingsManager.GetConfigValue("10") == "021" || AppSettingsManager.GetConfigValue("56") == "Y") // RMC 20141203 corrected error in online payment
                    result.Query += ", or_series as or_series ";
                else
                    result.Query += ", null as or_series "; 
                result.Query += "from or_assigned_hist where teller not in (select teller from rcd_remit ";
                result.Query += "where teller = '"+m_sTellerCode+"' ";
                if (AppSettingsManager.GetConfigValue("10") == "021")
                    result.Query += "and to_number(substr(or_no,3,20)) between or_assigned_hist.from_or_no and or_assigned_hist.to_or_no and substr(or_no,1,2) = or_assigned_hist.or_series ";
                else
                {
                    // RMC 20141203 corrected error in online payment (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                        result.Query += "and or_no between or_assigned_hist.from_or_no||or_series and or_assigned_hist.to_or_no||or_series ";
                    // RMC 20141203 corrected error in online payment (e)
                    else
                        result.Query += "and or_no between or_assigned_hist.from_or_no and or_assigned_hist.to_or_no ";
                }
                result.Query += "and or_no in (select or_no from pay_hist where teller = '"+m_sTellerCode+"')) and teller = '"+m_sTellerCode+"' ";
                result.Query += "and (from_or_no,to_or_no) not in (select or_no,or_no from cancelled_payment where or_no between or_assigned_hist.from_or_no and or_assigned_hist.to_or_no and teller = '"+ m_sTellerCode +"') ";

                if (AppSettingsManager.GetConfigValue("56") == "Y") // RMC 20141217 adjustments
                    result.Query += "group by or_series order by or_series";
                else
                    result.Query += " order by from_or_no,to_or_no";
                // RMC 20140909 Migration QA (e)
            }
            else
            {
                #region comments
                /*result.Query = "select from_or_no,to_or_no,or_series from or_assigned_hist oah, partial_remit pr";
                result.Query += string.Format(" where oah.teller = '{0}' and pr.or_from between oah.or_series||oah.from_or_no and oah.or_series||oah.to_or_no", m_sTellerCode);*/
                // RMC 20140909 Migration QA (s)
                /*if (AppSettingsManager.GetConfigValue("10") == "021")
                    result.Query = "select to_number(substr(or_from,3,20)) as from_or_no, to_number(substr(or_to,3,20)) as to_or_no, substr(or_from,1,2) as or_series ";
                else
                    result.Query = "select or_from as from_or_no, or_to as to_or_no, null as or_series ";
                result.Query += "from partial_remit where rcd_series = '" + m_sRCDNo + "' order by or_series, from_or_no asc";*/

                //result.Query = "select min(from_or_no) as from_or_no, max(to_or_no) as to_or_no";
              /*  result.Query = "select distinct from_or_no, to_or_no"; //MCR 20150109
                if (AppSettingsManager.GetConfigValue("10") == "021" || AppSettingsManager.GetConfigValue("56") == "Y") // RMC 20141203 corrected error in online payment
                    result.Query += ", or_series as or_series ";
                else
                    result.Query += ", null as or_series ";
                // RMC 20141203 corrected error in online payment (s)
                if(AppSettingsManager.GetConfigValue("56") == "Y") 
                    //result.Query += "from or_assigned_hist where from_or_no||or_series in ";
                    result.Query += "from or_assigned_hist where from_or_no||or_series between "; //MCR 20150109
                // RMC 20141203 corrected error in online payment (e)
                else
                    //result.Query += "from or_assigned_hist where from_or_no in ";
                    result.Query += "from or_assigned_hist where from_or_no between "; //MCR 20150109
                if (AppSettingsManager.GetConfigValue("10") == "021")
                    result.Query += "(select to_number(substr(or_from,3,20)) from partial_remit where rcd_series = '" + m_sRCDNo + "' and or_series = substr(or_from,1,2)) ";
                else
                {
                    // RMC 20141203 corrected error in online payment (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                    {
                        result.Query += "(select or_from from partial_remit where rcd_series = '" + m_sRCDNo + "') and";
                        result.Query += "(select or_to from partial_remit where rcd_series = '" + m_sRCDNo + "') ";//MCR 20150109
                    }
                    // RMC 20141203 corrected error in online payment (e)
                    else
                    {
                        result.Query += "(select to_number(or_from) from partial_remit where rcd_series = '" + m_sRCDNo + "') and ";
                        result.Query += "(select to_number(or_to) from partial_remit where rcd_series = '" + m_sRCDNo + "')"; //MCR 20150109
                    }
                }

                if (m_sRCDNo == "05A-15-01-0000000015") //MCR 20150128
                {
                    //MCR 20150127 (s) RCD
                    result.Query += " and (from_or_no in (select or_no from rcd_remit where rcd_series = '" + m_sRCDNo + "')";
                    result.Query += " and to_or_no in (select or_no from rcd_remit where rcd_series = '" + m_sRCDNo + "')) ";
                    //MCR 20150127 (s) RCD
                }
                if (AppSettingsManager.GetConfigValue("56") == "Y") // RMC 20141217 adjustments
                    result.Query += "group by or_series order by from_or_no asc";
                else
                    result.Query += "order by from_or_no,to_or_no";
                m_sRCDSeries = m_sRCDNo;
                // RMC 20140909 Migration QA (e)
               */
                // RMC 20150915 corrections in re-print of RCD, put rem
                #endregion
                // RMC 20150915 corrections in re-print of RCD (s)
                result.Query = "select or_from as from_or_no, or_to as to_or_no, null as or_series from partial_remit ";
                result.Query += " where rcd_series = '" + m_sRCDNo + "'";
                result.Query += " order by from_or_no";
                m_sRCDSeries = m_sRCDNo;
                // RMC 20150915 corrections in re-print of RCD (e)

            }
            sOrSeriesQuery = result.Query; //JARS 20171016 
            // RMC 20150504 QA corrections (s)
            OracleResultSet pCmd = new OracleResultSet();
            pCmd.Query = "delete from rcd_remit_tmp where teller_code = '" + m_sTellerCode + "'";
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            lstORLast = new List<string>(); // RMC 20170110 add sub-total per booklet in rcd per OR

            if (result.Execute())
            {
                while (result.Read())
                {
                    //strORFr = result.GetString("from_or_no").ToString().Trim();
                    //strORTo = result.GetString("to_or_no").ToString().Trim();
                    //if (m_sSwitch == "New")
                    //{
                    //    strORFr = result.GetInt("from_or_no").ToString().Trim(); //JARS 20171003
                    //    strORTo = result.GetInt("to_or_no").ToString().Trim();
                    //}
                    //else
                    //{
                        strORFr = result.GetString("from_or_no").ToString().Trim(); //JARS 20171003 
                        strORTo = result.GetString("to_or_no").ToString().Trim();
                        if (strORFr.Length < 7) //JHB 20180605 (s) in order to print and re-print
                        {
                            strORFr = "00" + strORFr;
                            strORTo = "00" + strORTo;
                        }//JHB 20180605(e) 
                    //}
                        result2.Query = "select * from pay_hist where or_no between '"+ strORFr +"' and '"+ strORTo +"' and teller = '"+ m_sTellerCode +"'";
                    if(result2.Execute())
                    {
                        while(result2.Read())
                        {
                            InsertTmp(strORFr, strORTo, "FORM 51");
                        }
                    }
                    result2.Close();
                }
            }
            result.Close();

            result.Query = "select * from RCD_REMIT_TMP where TELLER_CODE = '" + m_sTellerCode + "' order by from_or_no";
            // RMC 20150504 QA corrections (e)

            if (result.Execute())
            {
                string sORWatch = string.Empty;
                string sTmpORWatch = string.Empty;
                while (result.Read())
                {
                    /*strORFr = result.GetInt("from_or_no").ToString().Trim();
                    strORTo = result.GetInt("to_or_no").ToString().Trim();*/

                    // RMC 20150501 QA BTAS (s)
                    strORFr = result.GetString("from_or_no").ToString().Trim();
                    strORTo = result.GetString("to_or_no").ToString().Trim();
                    // RMC 20150501 QA BTAS (e)

                    // RMC 20170117 correction in OR Series beginnin in RCD (s)
                    strReceiptsFr_OR_No.Add(strORFr);
                    strReceiptsTo_OR_No.Add(strORTo);
                    int iTmpQtyOR = 0;
                    iTmpQtyOR = Convert.ToInt32(strORTo) - Convert.ToInt32(strORFr) + 1;
                    strReceiptsQty.Add(iTmpQtyOR.ToString());
                    // RMC 20170117 correction in OR Series beginnin in RCD (e)


                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                        strORSeries = result.GetString("or_series").Trim();
                    else
                        strORSeries = "";

                    /*rs.Query = "select to_char(max(or_no)) as maxor, to_char(min(or_no)) as minor from or_used ";
                    rs.Query += string.Format("where or_no between '{3}{0}' and '{3}{1}' and teller = '{2}' and teller not in ", strORFr, strORTo, m_sTellerCode, strORSeries);
                    rs.Query += "(select teller from partial_remit where or_used.or_no between partial_remit.or_from and partial_remit.or_to and teller = '" + m_sTellerCode + "') ";
                    rs.Query += "having to_char(Max(or_no)) is not null"; //ADD MCR 20140626*/

                    // RMC 20140909 Migration QA (s)

                    // RMC 20141203 corrected error in online payment (s)
                    if (AppSettingsManager.GetConfigValue("10") == "021")
                    {
                        sORWatch = strORSeries;
                        sORWatch = sORWatch.Trim() + strORFr;
                    }
                    else
                    {
                        if (AppSettingsManager.GetConfigValue("56") == "Y")
                        {
                            sORWatch = strORFr + strORSeries;
                        }
                        else
                        {
                            sORWatch = strORFr;
                        }
                    }
                    // RMC 20141203 corrected error in online payment (e) 

                    if (sORWatch != sTmpORWatch) //to avoid duplication
                    {
                        sTmpORWatch = sORWatch;
                        int iTmpIssuedCtr = 0;  // RMC 20170119 correction in RCD error where OR issued was not used

                        if (strORSeries.Trim() != "" && AppSettingsManager.GetConfigValue("10") == "021")  // RMC 20141203 corrected error in online payment
                        {
                            // this is for Antipolo version wherein they used OR series code bec of duplication of OR series (error by printing press)
                            rs.Query = "select to_char(max(to_number(substr(or_no,3,20)))) as maxor, to_char(min(to_number(substr(or_no,3,20)))) as minor from or_used ";
                            rs.Query += "where to_number(substr(or_no,3,20)) between '" + strORFr + "' and '" + strORTo + "' and or_no like '" + strORSeries + "%' ";
                            if (m_sSwitch == "New")
                                rs.Query += "and teller = '" + m_sTellerCode + "' and trim(teller) not in ";
                            else
                                rs.Query += "and teller = '" + m_sTellerCode + "' and trim(teller) in ";
                            rs.Query += "(select trim(teller) from partial_remit where to_number(substr(or_used.or_no,3,20)) ";
                            rs.Query += "between to_number(substr(partial_remit.or_from,3,20)) and to_number(substr(partial_remit.or_to,3,20)) ";
                            rs.Query += "and substr(partial_remit.or_from,1,2) = '" + strORSeries + "' and teller = '" + m_sTellerCode + "') ";
                            rs.Query += "having to_char(Max(or_no)) is not null";
                        }
                        else
                        {
                            rs.Query = "select  to_char(max(to_number(or_no))) as maxor, to_char(min(to_number(or_no))) as minor from or_used "; //AFM 20200124 changed datatype
                            if (m_sSwitch == "New")
                                rs.Query += string.Format("where or_no between '{0}' and '{1}' and teller = '{2}' and teller not in ", strORFr, strORTo, m_sTellerCode);
                            else
                                rs.Query += string.Format("where or_no between '{0}' and '{1}' and teller = '{2}' and teller in ", strORFr, strORTo, m_sTellerCode);
                            // RMC 20141203 corrected error in online payment (s)
                            if (AppSettingsManager.GetConfigValue("56") == "Y")
                            {
                                //rs.Query += "(select teller from partial_remit where or_used.or_no||or_series between partial_remit.or_from and partial_remit.or_to and teller = '" + m_sTellerCode + "') ";
                                rs.Query += "(select teller from partial_remit where or_used.or_no||or_series between partial_remit.or_from and partial_remit.or_to and teller = '" + m_sTellerCode + "' and rcd_series = '" + m_sRCDNo + "') ";
                                rs.Query += " and or_series = '" + strORSeries + "'";
                            }// RMC 20141203 corrected error in online payment (e)
                            else
                                //rs.Query += "(select teller from partial_remit where or_used.or_no between partial_remit.or_from and partial_remit.or_to and teller = '" + m_sTellerCode + "') ";
                                rs.Query += "(select teller from partial_remit where or_used.or_no between partial_remit.or_from and partial_remit.or_to and teller = '" + m_sTellerCode + "' and rcd_series = '" + m_sRCDNo + "') ";
                            
                            rs.Query += "and or_no not in (select or_no from cancelled_payment where or_no between '"+ strORFr+"' and '"+ strORTo +"')"; //JARS 20171016
                            rs.Query += "having to_char(Max(or_no)) is not null";
                        }
                        // RMC 20140909 Migration QA (e)
                        if (rs.Execute())
                        {
                            while (rs.Read())
                            {
                                iTmpIssuedCtr++;  // RMC 20170119 correction in RCD error where OR issued was not used

                                //if (strTmpORTO != strORTo) //MOD MCR 20150115
                                if (strTmpORTO != strORTo || (strTmpORTO == strORTo && strTmpORFROM != strORFr)) //MOD MCR 20150115
                                {
                                    strMinOR = rs.GetString("minor").Trim();
                                    strMaxOR = rs.GetString("maxor").Trim();

                                    int.TryParse(strMinOR, out iMinOR);
                                    int.TryParse(strMaxOR, out iMaxOR);

                                    //MCR 20140826
                                    /*if (iMinOR == 0)
                                        iMinOR = Convert.ToInt32(strMinOR.Substring(2));
                                    if (iMaxOR == 0)
                                        iMaxOR = Convert.ToInt32(strMaxOR.Substring(2));
                                    */
                                    // RMC 20140909 Migration QA (s)
                                    if (iMinOR == 0)
                                        int.TryParse(strMinOR, out iMinOR);
                                    if (iMaxOR == 0)
                                        int.TryParse(strMaxOR, out iMaxOR);
                                    // RMC 20140909 Migration QA (e)

                                    iORQty = (iMaxOR - iMinOR) + 1;

                                    //dAmount = GetAmount(strMinOR, strMaxOR);

                                    // RMC 20140909 Migration QA (s)
                                    dAmount = GetAmount(iMinOR, iMaxOR, strORSeries, "");
                                    //dCheckAmount = GetCheckAmount(m_sTellerCode,m_sSwitch);
                                    dCheckAmount = GetCheckAmount(m_sTellerCode, m_sSwitch, m_sOrDate); // RMC 20150106
                                    // RMC 20140909 Migration QA (e)

                                    /*GetCheckAmount(strMinOR, strMaxOR, out strCheckNo, out strBank);
                                    //ValidateORTo(strMaxOR, strORTo, strORTo)
                                    if (strCheckNo != strCheckNoTmp && strCheckNo != "")
                                    {
                                        dCheckAmount += GetCheckAmount(strMinOR, strMaxOR, out strCheckNo, out strBank);
                                        lstCheckNo.Add(strCheckNo.Trim());
                                        lstBank.Add(strBank.Trim());
                                        lstCheckAmt.Add(dCheckAmount);
                                    }
                                    strCheckNoTmp = strCheckNo;
                                    */
                                    // RMC 20140909 Migration QA, put rem

                                    if (strORSeries.Trim() != "")   // RMC 20140909 Migration QA
                                    {
                                        if (AppSettingsManager.GetConfigValue("10") == "021")   // RMC 20141203 corrected error in online payment
                                            //axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|>2200;{0}|{1}|{2}|{3}|{4:#,###.00}", "LICENSE", iORQty, strORSeries.Trim() + strMinOR, strORSeries.Trim() + strMaxOR, dAmount); // RMC 20140909 Migration QA
                                            axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|>2200;{0}|{1}|{2}|{3}|{4:#,###.00}", "FORM 51", iORQty, strORSeries.Trim() + strMinOR, strORSeries.Trim() + strMaxOR, dAmount); // RMC 20140909 Migration QA
                                        else
                                            //axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|>2200;{0}|{1}|{2}|{3}|{4:#,###.00}", "LICENSE", iORQty, strMinOR + strORSeries.Trim(), strMaxOR + strORSeries.Trim(), dAmount); // RMC 20141203 corrected error in online payment
                                            axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|>2200;{0}|{1}|{2}|{3}|{4:#,###.00}", "FORM 51", iORQty, strMinOR + strORSeries.Trim(), strMaxOR + strORSeries.Trim(), dAmount); // RMC 20141203 corrected error in online payment
                                    }
                                    else

                                        if (strMinOR.Length < 7 || strMinOR.Length < 7) //JHB 20180605 (s) add "00" in or display
                                         {
                                             strMinOR = "00" + strMinOR;
                                             strMaxOR = "00" + strMaxOR;
                                         }//JHB 20180605(e) 

                                        //axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|>2200;{0}|{1}|{2}|{3}|{4:#,###.00}", "LICENSE", iORQty, strMinOR, strMaxOR, dAmount);
                                        axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|>2200;{0}|{1}|{2}|{3}|{4:#,###.00}", "FORM 51", iORQty, strMinOR, strMaxOR, dAmount);

                                    iRowCtr++;  // RMC 20150501 QA BTAS

                                    //Get Receipts Inclusive Serial Nos
                                    int.TryParse(strORFr, out iTmpORFr);
                                    int.TryParse(strORTo, out iTmpORTo);

                                    // RMC 20141203 corrected error in online payment (s)
                                    if (iMinOR > iTmpORFr)
                                    {
                                        iTmpORFr = iMinOR;
                                        strORFr = iTmpORFr.ToString();
                                    }
                                    // RMC 20141203 corrected error in online payment (e)
                                    iTmpQty = (iTmpORTo - iTmpORFr) + 1;

                                    /*strReceiptsQty.Add(iTmpQty.ToString());

                                    if (strORSeries.Trim() != "")   // RMC 20140909 Migration QA (s)
                                    {
                                        if (AppSettingsManager.GetConfigValue("10") == "021")   // RMC 20141203 corrected error in online payment
                                        {
                                            strReceiptsFr_OR_No.Add(strORSeries.Trim() + strORFr);
                                            strReceiptsTo_OR_No.Add(strORSeries.Trim() + strORTo);
                                        }
                                        else
                                        {
                                            // RMC 20141203 corrected error in online payment (s)
                                            strReceiptsFr_OR_No.Add(strORFr + strORSeries.Trim());
                                            strReceiptsTo_OR_No.Add(strORTo + strORSeries.Trim());
                                            // RMC 20141203 corrected error in online payment (e)
                                        }
                                    }// RMC 20140909 Migration QA (e)
                                    else
                                    {
                                        strReceiptsFr_OR_No.Add(strORFr);
                                        strReceiptsTo_OR_No.Add(strORTo);
                                    }
                                    //Get Receipts Inclusive Serial Nos
                                    */
                                    // RMC 20170117 correction in OR Series beginnin in RCD, put rem

                                    //Get Issued Inclusive Serial Nos                                
                                    strIssuedQty.Add(iORQty.ToString());
                                    if (strORSeries.Trim() != "")   // RMC 20140909 Migration QA (s)
                                    {
                                        if (AppSettingsManager.GetConfigValue("10") == "021")   // RMC 20141203 corrected error in online payment
                                        {
                                            strIssuedFr_OR_No.Add(strORSeries.Trim() + strMinOR);
                                            strIssuedTo_OR_No.Add(strORSeries.Trim() + strMaxOR);
                                        }
                                        else
                                        {
                                            // RMC 20141203 corrected error in online payment (s)
                                            strIssuedFr_OR_No.Add(strMinOR + strORSeries.Trim());
                                            strIssuedTo_OR_No.Add(strMaxOR + strORSeries.Trim());
                                            // RMC 20141203 corrected error in online payment (e)
                                        }
                                    }// RMC 20140909 Migration QA (e)
                                    else
                                    {
                                        strIssuedFr_OR_No.Add(strMinOR);
                                        strIssuedTo_OR_No.Add(strMaxOR);
                                    }
                                    //Get Issued Inclusive Serial Nos 


                                    //Get Ending Balance Inclusive Serial Nos                                
                                    //int.TryParse(strMaxOR.Substring(2), out iTmpORFr);//MCR 20140901 Remove String

                                    

                                    int.TryParse(strMaxOR, out iTmpORFr);   // RMC 20140909 Migration QA
                                    iTmpORFr++;
                                    int.TryParse(strORTo, out iTmpORTo);
                                    iTmpQty = (iTmpORTo - iTmpORFr) + 1;

                                    // RMC 20150504 QA corrections (s)
                                    string sTmpORFr = string.Format("{0:000000#}", iTmpORFr);
                                    string sTmpORTo = string.Format("{0:000000#}", iTmpORTo);
                                    // RMC 20150504 QA corrections (e)

                                    // RMC 20151230 corrected OR ending balance in RCD (s)
                                    if (m_sSwitch != "New")
                                    {
                                        sTmpORTo = GetEndingOR(sTmpORTo, m_sTellerCode);
                                        iTmpORTo = Convert.ToInt32(sTmpORTo);
                                        iTmpQty = (iTmpORTo - iTmpORFr) + 1;
                                        //iTmpQty = (iTmpORFr - iTmpORTo) + 1; //JARS 20170725 QTY BECOMES NEGATIVE
                                    }
                                    // RMC 20151230 corrected OR ending balance in RCD (e)
                                    
                                    if (iTmpQty == 0)
                                    {
                                        strEndingFr_OR_No.Add("XXX");
                                        strEndingTo_OR_No.Add("XXX");
                                        strEndingQty.Add(iTmpQty.ToString());
                                    }
                                    else
                                    {
                                        if (strORSeries.Trim() != "")   // RMC 20140909 Migration QA (s)
                                        {
                                            if (AppSettingsManager.GetConfigValue("10") == "021")   // RMC 20141203 corrected error in online payment
                                            {
                                                /*strEndingFr_OR_No.Add(strORSeries.Trim() + iTmpORFr.ToString());
                                                strEndingTo_OR_No.Add(strORSeries.Trim() + iTmpORTo.ToString());*/
                                                // RMC 20150504 QA corrections (s)
                                                strEndingFr_OR_No.Add(strORSeries.Trim() + sTmpORFr);
                                                strEndingTo_OR_No.Add(strORSeries.Trim() + sTmpORTo);
                                                // RMC 20150504 QA corrections (e)
                                            }
                                            else
                                            {
                                                /*
                                                // RMC 20141203 corrected error in online payment (s)
                                                strEndingFr_OR_No.Add(iTmpORFr.ToString() + strORSeries.Trim());
                                                strEndingTo_OR_No.Add(iTmpORTo.ToString() + strORSeries.Trim());
                                                // RMC 20141203 corrected error in online payment (e)
                                                 */
                                                // RMC 20150504 QA corrections (s)
                                                strEndingFr_OR_No.Add(sTmpORFr + strORSeries.Trim());
                                                strEndingTo_OR_No.Add(sTmpORTo + strORSeries.Trim());
                                                // RMC 20150504 QA corrections (e)
                                            }
                                        } // RMC 20140909 Migration QA (e)
                                        else
                                        {
                                            /*strEndingFr_OR_No.Add(iTmpORFr.ToString());
                                            strEndingTo_OR_No.Add(iTmpORTo.ToString());*/

                                            // RMC 20150504 QA corrections (s)
                                            strEndingFr_OR_No.Add(sTmpORFr.ToString());
                                            strEndingTo_OR_No.Add(sTmpORTo.ToString());
                                            // RMC 20150504 QA corrections (e)
                                        }
                                        strEndingQty.Add(iTmpQty.ToString());
                                    }
                                    //Get Ending Balance Inclusive Serial Nos                                
                                }
                                strTmpORTO = strORTo;
                                strTmpORFROM = strORFr;
                            }

                            // RMC 20170119 correction in RCD error where OR issued was not used (s)
                            if (iTmpIssuedCtr == 0)
                            {
                                strIssuedFr_OR_No.Add("XXX");
                                strIssuedTo_OR_No.Add("XXX");
                                strIssuedQty.Add("0");

                                int.TryParse(strORFr, out iTmpORFr);
                                int.TryParse(strORTo, out iTmpORTo);

                                iTmpIssuedCtr = iTmpORTo - iTmpORFr + 1;
                                strEndingFr_OR_No.Add(strORFr);
                                strEndingTo_OR_No.Add(strORTo);
                                strEndingQty.Add(iTmpIssuedCtr.ToString());
                            }
                            // RMC 20170119 correction in RCD error where OR issued was not used (e)
                        }
                        rs.Close();
                    }
                }
            }
            result.Close();
            //}
            //else
            //{
            //    axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|>2200;{0}|{1}|{2}|{3}|{4:#,###.00}", "LICENSE", iORQty, strMinOR, strMaxOR, dAmount);
            //}

            /*axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|>2200;{0}|{1}|{2}|{3}|{4}", " ", " ", " ", " ", " ");
            axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|>2200;{0}|{1}|{2}|{3}|{4}", " ", " ", " ", " ", " ");*/

            // RMC 20150501 QA BTAS (s)
            for (int i = iRowCtr; i <= 5; i++)
            {
                axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|>2200;{0}|{1}|{2}|{3}|{4}", " ", " ", " ", " ", " ");
            }
            // RMC 20150501 QA BTAS (e)

            //dTotalAmount = GetTotalAmount(m_sTellerCode);
            //dTaxCredit = GetTaxCredit(m_sTellerCode);
            //dCashAmount = dTotalAmount - dCheckAmount;

            if (m_sSwitch == "New")
            {
                dCashAmount = GetTotalCashAmt(m_sTellerCode);
                dAmount = GetAmount(m_sTellerCode);
                /*dExcessCheck = GetExcessCheck(m_sTellerCode); 
                dAppTC = GetAppliedTC(m_sTellerCode);*/
                // RMC 20140909 Migration QA, put rem

                dExcessCheck = GetCredit(m_sTellerCode);    // RMC 20140909 Migration QA
                dAppTC = GetDebit(m_sTellerCode);   // RMC 20140909 Migration QA
            }
            else
            {
                dCashAmount = GetTotalCashAmt(m_sTellerCode, m_sRCDNo);
                dAmount = GetAmount(m_sTellerCode, m_sRCDNo, "");
                dExcessCheck = GetExcessCheck(m_sTellerCode, m_sRCDNo);
                dAppTC = GetAppliedTC(m_sTellerCode, m_sRCDNo);
            }

            //dTotalAmount = dAmount;
            dTotalAmount = dAmount + dExcessCheck - dAppTC; // RMC 20140909 Migration QA

            double dActualCol = 0;
            double dGrossCol = 0;
            //dActualCol = (dAmount + dAppTC);
            dActualCol = dAmount;   // RMC 20140909 Migration QA
            dGrossCol = ((dAmount - dAppTC) + dExcessCheck);

            //MCR 20140827 (s)
            m_dCheckAmount = dCheckAmount;
            m_dCashAmount = dCashAmount;
            m_dTotalCollect = dGrossCol;
            //MCR 20140827 (e)

            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = string.Format(">8800|>2200;{0}|{1:#,###.00}", "TOTAL", dActualCol);
            axVSPrinter1.Table = string.Format(">8800|>2200;{0}|{1:#,###.00}", "LESS APPLIED TAX CREDIT", dAppTC);
            axVSPrinter1.Table = string.Format(">8800|>2200;{0}|{1:#,###.00}", "PLUS EXCESS OF CHECK", dExcessCheck);
            axVSPrinter1.Table = string.Format(">8800|>2200;{0}|{1:#,###.00}", "GRAND TOTAL", dGrossCol);
            axVSPrinter1.FontBold = false;

            axVSPrinter1.Table = string.Format("<6600|^2200|^2200;{0}", "B. REMITTANCE AND DEPOSITS");

            axVSPrinter1.Table = string.Format("<6600|^2200|^2200;{0}", "ACCOUNTABLE OFFICER/BANK", "REFERENCE", "AMOUNT");

            axVSPrinter1.Table = string.Format("<6600|>2200|>2200;{0}|{1}|{2:#,###.00}", " ", "CHECK:", dCheckAmount);
            axVSPrinter1.Table = string.Format("<6600|>2200|>2200;{0}|{1}|{2:#,###.00}", " ", "CASH:", dCashAmount);

            axVSPrinter1.Table = string.Format("<6600|>2200|>2200;{0}|{1}|{2}", " ", " ", " ");

            double fDenomination = 0;
            double fDenominationQty = 0;
            double fDenominationAmt = 0;
            double fDenominationAmtTotal = 0;

            // RMC 20140909 Migration QA (s)
            if (m_sSwitch != "New")
            {
                PopulateDenomination();
            }
            // RMC 20140909 Migration QA (e)

            for (int i = 0; i != m_lstDenominations.Count; i++)
            {
                double.TryParse(m_lstDenominations[i], out fDenomination);
                double.TryParse(m_lstDenominationsQty[i], out fDenominationQty);
                double.TryParse(m_lstDenominationsAmt[i], out fDenominationAmt);

                axVSPrinter1.Table = string.Format(">6600|>2200|>2200;{0}|{1}|{2:#,###.00}", fDenomination + " x " + fDenominationQty, "", fDenominationAmt);
                fDenominationAmtTotal += fDenominationAmt;
            }
            fDenominationAmtTotal += dCheckAmount;  // RMC 20141203 corrected error in online payment
            axVSPrinter1.Table = string.Format(">8800|>2200;{0}|{1:#,###.00}", "TOTAL", fDenominationAmtTotal);

            axVSPrinter1.Table = string.Format("<11000;{0}", "C. ACCOUNTABILITY FOR ACCOUNTABLE FORMS");

            /*axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|^2200;{0}|{1}|{2}|{3}|{4}", "", "BEGINNING BALANCE", "RECEIPT", "ISSUED", "ENDING BALANCE");

            axVSPrinter1.Table = string.Format("^2200|^2200|^2200|^2200|^2200;{0}|{1}|{2}|{3}|{4}", "NAME OF FORM", "INCLUSIVE SERIAL NOS", "INCLUSIVE SERIAL NOS", "INCLUSIVE SERIAL NOS", "INCLUSIVE SERIAL NOS");

            axVSPrinter1.Table = string.Format("^2200|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
                "", "QTY", "FROM", "TO", "QTY", "FROM", "TO", "QTY", "FROM", "TO", "QTY", "FROM", "TO");*/

            // RMC 20150504 QA corrections (s)
            axVSPrinter1.Table = string.Format("^1400|^2400|^2400|^2400|^2400;{0}|{1}|{2}|{3}|{4}", "", "BEGINNING BALANCE", "RECEIPT", "ISSUED", "ENDING BALANCE");

            axVSPrinter1.Table = string.Format("^1400|^2400|^2400|^2400|^2400;{0}|{1}|{2}|{3}|{4}", "NAME OF FORM", "INCLUSIVE SERIAL NOS", "INCLUSIVE SERIAL NOS", "INCLUSIVE SERIAL NOS", "INCLUSIVE SERIAL NOS");

            axVSPrinter1.Table = string.Format("^1400|^600|^900|^900|^600|^900|^900|^600|^900|^900|^600|^900|^900;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
                "", "QTY", "FROM", "TO", "QTY", "FROM", "TO", "QTY", "FROM", "TO", "QTY", "FROM", "TO");
            // RMC 20150504 QA corrections (e)

            iRowCtr = 0;    // RMC 20150501 QA BTAS
            for (int i = 0; i != strReceiptsQty.Count; i++)
            {
                if (i == 0) // For Beginning Balance
                {
                    //axVSPrinter1.Table = string.Format("^2200|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
                    axVSPrinter1.Table = string.Format("^1400|^600|^900|^900|^600|^900|^900|^600|^900|^900|^600|^900|^900;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",  // RMC 20150504 QA corrections
                        //"LICENSE", strReceiptsQty[i], strReceiptsFr_OR_No[i], strReceiptsTo_OR_No[i],
                        //"FORM 51", strReceiptsQty[i], strReceiptsFr_OR_No[i], strReceiptsTo_OR_No[i],"", "", "",strIssuedQty[i], strIssuedFr_OR_No[i], strIssuedTo_OR_No[i],strEndingQty[i], strEndingFr_OR_No[i], strEndingTo_OR_No[i]);
                        "FORM 51", strReceiptsQty[i], strReceiptsFr_OR_No[i], strReceiptsTo_OR_No[i],"", "", "",strIssuedQty[i], strIssuedFr_OR_No[i], strIssuedTo_OR_No[i],strEndingQty[i], strEndingFr_OR_No[i], strEndingTo_OR_No[i]);
                }
                else
                {
                    //axVSPrinter1.Table = string.Format("^2200|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
                    axVSPrinter1.Table = string.Format("^1400|^600|^900|^900|^600|^900|^900|^600|^900|^900|^600|^900|^900;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",    // RMC 20150504 QA corrections
                        //"LICENSE", "", "", "",
                        "FORM 51", "", "", "",
                        strReceiptsQty[i], strReceiptsFr_OR_No[i], strReceiptsTo_OR_No[i],
                        strIssuedQty[i], strIssuedFr_OR_No[i], strIssuedTo_OR_No[i],
                        strEndingQty[i], strEndingFr_OR_No[i], strEndingTo_OR_No[i]);
                }

                iRowCtr = i;    // RMC 20150501 QA BTAS
            }

            // RMC 20150501 QA BTAS (s)
            for (int ix = iRowCtr; ix <= 5; ix++)
            {
                //axVSPrinter1.Table = string.Format("^2200|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33|^733.33;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
                axVSPrinter1.Table = string.Format("^1400|^600|^900|^900|^600|^900|^900|^600|^900|^900|^600|^900|^900;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",  // RMC 20150504 QA corrections
                "", "", "", "", "", "", "", "", "", "", "", "", "");
            }
            // RMC 20150501 QA BTAS (e)
            //MCR 20150115
            //axVSPrinter1.Paragraph = "";
            //axVSPrinter1.Paragraph = "";

            axVSPrinter1.Table = string.Format("<11000;{0}", "D. SUMMARY OF COLLECTION AND REMITTANCES/DEPOSITS");

            axVSPrinter1.Table = string.Format("^5500|^5500;{0}|{1}", "BEGINNING BALANCE", "LIST OF CHECKS");

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColumns;
            axVSPrinter1.Table = string.Format("^0|<5500|^1833.33|^1833.33|^1833.33|^0;{0}|{1}|{2}|{3}|{4}|{5}", "", "ADD: COLLECTIONS:", "CHECK NO", "BANK", "AMOUNT", "");

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            object objCurrentY = axVSPrinter1.CurrentY;
            object objCurrentMarginL = axVSPrinter1.MarginLeft;

            axVSPrinter1.MarginLeft = "6000";
            /*for (int i = 0; i != lstCheckNo.Count; i++)
            {
                axVSPrinter1.Table = string.Format("^1833.33|^1833.33|^1833.33;{0}|{1}|{2:#,###.00}", lstCheckNo[i], lstBank[i], lstCheckAmt[i]);
            }
            if (lstCheckNo.Count == 0)
            {
                axVSPrinter1.Table = string.Format("^1833.33|^1833.33|^1833.33;{0}|{1}|{2}", " ", " ", " ");
            }
             */

            // RMC 20140909 Migration QA (s)
            bool bCheckSupple = false;
            int iChkRow = 0;
            if (m_lstCheckNo.Count > 5)
                bCheckSupple = true;

            for (int i = 0; i != m_lstCheckNo.Count; i++)   
            {
                axVSPrinter1.Table = string.Format("^1833.33|^1833.33|^1833.33;{0}|{1}|{2:#,###.00}", m_lstCheckNo[i], m_lstBank[i], m_lstCheckAmt[i]);

                if (i == 4 && bCheckSupple)
                {
                    iChkRow = i+1;
                    break;
                }
            }

            if (m_lstCheckNo.Count == 0)
            {
                axVSPrinter1.Table = string.Format("^1833.33|^1833.33|^1833.33;{0}|{1}|{2}", " ", " ", " ");
            }
            // RMC 20140909 Migration QA (e)

            

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColumns;
            axVSPrinter1.CurrentY = objCurrentY;
            axVSPrinter1.MarginLeft = objCurrentMarginL;
            axVSPrinter1.Table = string.Format("^0|<5500|<5500|^0;{0}|{1}|{2}", " ", " ", " ", " ");
            axVSPrinter1.Table = string.Format("^0|<5500|<5500|^0;{0}|{1}|{2}", " ", " ", " ", " ");
            axVSPrinter1.Table = string.Format("^0|<5500|<5500|^0;{0}|{1}|{2}", " ", " ", " ", " ");
            //axVSPrinter1.Table = string.Format("^0|<5500|<5500|^0;{0}|{1}|{2}", " ", " ", " ", " ");

            object objTotalCashCurrentY = axVSPrinter1.CurrentY;
            axVSPrinter1.Table = string.Format("^0|>5500|>5500|^0;{0}|{1,-70}|{2}", " ", "TOTAL CASH:", " ", " ");

            object objTotalCheckCurrentY = axVSPrinter1.CurrentY;
            axVSPrinter1.Table = string.Format("^0|>5500|>5500|^0;{0}|{1,-70}|{2}", " ", "TOTAL CHECK:", " ", " ");

            object objTotalNetCurrentY = axVSPrinter1.CurrentY;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = string.Format("^0|>5500|>5500|^0;{0}|{1,-80}|{2}", " ", "TOTAL NET COLLECTION:", " ", " ");
            axVSPrinter1.FontBold = false;

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.CurrentY = objTotalCashCurrentY;

            axVSPrinter1.MarginLeft = "3000";
            /*axVSPrinter1.Table = string.Format(">2000;{0:#,###.00}", dCashAmount - dAppTC); //MCR 20140826
            axVSPrinter1.Table = string.Format(">2000;{0:#,###.00}", dCheckAmount - dExcessCheck); //MCR 20140826*/
            // RMC 20140909 Migration QA (s)
            axVSPrinter1.Table = string.Format(">2000;{0:#,###.00}", dCashAmount); 
            axVSPrinter1.Table = string.Format(">2000;{0:#,###.00}", dCheckAmount);
            // RMC 20140909 Migration QA (e)

            axVSPrinter1.Table = string.Format(">2000;{0:#,###.00}", dTotalAmount);

            axVSPrinter1.MarginLeft = 500;
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            //axVSPrinter1.Table = string.Format("^0|^11000|^0;{0}|{1}|{2}", " ", " ", " ");

            axVSPrinter1.Table = string.Format("^0|<11000|^0;{0}|{1,-100}{2}|{3}", " ", "CERTIFICATION", "VERIFICATION AND ACKNOWLEDGEMENT", " ");

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
            //object objCertCurrentY = axVSPrinter1.CurrentY;
            //axVSPrinter1.Table = string.Format("<0|<5500;{0}|{1}", " ", "               I hereby certify that the foregoing reports of\ncollection and deposits and accountability for accountable\nforms is true and correct.\n\n\n");

            //string strTotalAmt = string.Format("{0:#,###.00}", dTotalAmount);
            //axVSPrinter1.CurrentY = objCertCurrentY;
            //axVSPrinter1.MarginLeft = 6000;
            //axVSPrinter1.Table = string.Format("^5500|^0;{0}{1}|{2}", "                     I HEREBY CERTIFY that the foregoing report of collection\n has been verified and acknowledge receipt of Php ", strTotalAmt + "\n\n\n\n", " ");
           
            string strTotalAmt = string.Format("{0:#,###.00}", dTotalAmount);
            object objCertCurrentY = axVSPrinter1.CurrentY;
            axVSPrinter1.Table = string.Format("<0|<5500|<0|<5500;{0}|{1}||{2}", " ", "               I hereby certify that the foregoing reports of\ncollection and deposits and accountability for accountable\nforms is true and correct.\n\n\n"
                ,"                     I HEREBY CERTIFY that the foregoing report of collection\n has been verified and acknowledge receipt of Php " + strTotalAmt + ";;;;;");

            object objTellerCurrentY = axVSPrinter1.CurrentY;
            //axVSPrinter1.FontUnderline = true;
            //axVSPrinter1.MarginLeft = 500;
            //axVSPrinter1.Table = string.Format("^0|^0;{0}|{1}", " ", " ");

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //axVSPrinter1.CurrentY = Convert.ToInt64(objTellerCurrentY) - 200; // RMC 20150501 QA BTAS, put rem
            axVSPrinter1.CurrentY = Convert.ToInt64(objTellerCurrentY) - 800; // RMC 20150501 QA BTAS
            axVSPrinter1.MarginLeft = 1000;
            //axVSPrinter1.Table = string.Format("<2000|<1000|<2000|<1000|<2000|<1000|<1000|<500|^0|>0;{0}||{1}||{2}||{3}| | ", GetTeller().ToUpper(), 
            /*axVSPrinter1.Table = string.Format("<2500|<500|<2000|<1000|<2000|<1000|<1000|<500|^0|>0;{0}||{1}||{2}||{3}| | ", GetTeller().ToUpper(),     // RMC 20150504 QA corrections    
                AppSettingsManager.GetCurrentDate().ToShortDateString(), AppSettingsManager.GetConfigValue("44"), "                       ");*/

            // RMC 20150707 added filtering of RCD date in re-print RCD (s)
            //axVSPrinter1.Table = string.Format("<2500|<500|<2000|<1000|<2500|<500|<1000|<700|^0|>0;{0}||{1}||{2}||{3}| | ", GetTeller().ToUpper(), m_dtDateTrans.ToString("MM/dd/yyyy"), AppSettingsManager.GetConfigValue("05"), m_dtDateTrans.ToString("MM/dd/yyyy"));//GMC 20150923 Municipal Treasurer
            axVSPrinter1.Table = string.Format("<2500|<500|<2000|<1000|<2500|<500|<2000|<700|^0|>0;{0}||{1}||{2}||{3}| | ", GetTeller().ToUpper(), m_dtDateTrans.ToString("MM/dd/yyyy"), AppSettingsManager.GetConfigValue("05"), m_dtDateTrans.ToString("MM/dd/yyyy"));   // RMC 20180201 adjust printing of date in RCD
            // RMC 20150707 added filtering of RCD date in re-print RCD (e)

            //REM MCR20150116 (s)
            //axVSPrinter1.CurrentY = objTellerCurrentY;
            //axVSPrinter1.MarginLeft = 4000;
            //axVSPrinter1.Table = string.Format("<2000;{0}", AppSettingsManager.GetCurrentDate().ToShortDateString());

            //axVSPrinter1.CurrentY = objTellerCurrentY;
            //axVSPrinter1.MarginLeft = 7000;
            //axVSPrinter1.Table = string.Format("<2000;{0}", AppSettingsManager.GetConfigValue("44"));

            //axVSPrinter1.CurrentY = objTellerCurrentY;
            //axVSPrinter1.MarginLeft = 10000;
            //axVSPrinter1.Table = string.Format("<1000;{0}", "                       ");
            //axVSPrinter1.FontUnderline = false;

            //axVSPrinter1.CurrentY = objTellerCurrentY;
            //axVSPrinter1.MarginLeft = 11500;
            //axVSPrinter1.Table = string.Format("^0|>0;{0}|{1}", " ", " ");
            //REM MCR20150116 (e)

            //Description
            objTellerCurrentY = axVSPrinter1.CurrentY;
            axVSPrinter1.FontUnderline = true;
            axVSPrinter1.MarginLeft = 500;
            axVSPrinter1.Table = string.Format("^0|^0;{0}|{1}", " ", " ");

            axVSPrinter1.FontUnderline = false;
            axVSPrinter1.CurrentY = objTellerCurrentY;
            axVSPrinter1.MarginLeft = 1000;
            //axVSPrinter1.Table = string.Format("<2000|<1000|<2000|<1000|<2000|<1000|<1000|<500|^0|>0;{0}||{1}||{2}||{3}| | ", "  Name and Signature", "    Date",
            axVSPrinter1.Table = string.Format("<2500|<500|<2000|<1000|<2000|<1000|<1000|<500|^0|>0;{0}||{1}||{2}||{3}| | ", "  Name and Signature", "    Date",    // RMC 20150504 QA corrections
                "Name and Signature", "      Date");
            
            //REM MCR20150116 (s)
            //axVSPrinter1.CurrentY = objTellerCurrentY;
            //axVSPrinter1.MarginLeft = 4000;
            //axVSPrinter1.Table = string.Format("<2000;{0}", "    Date");

            //axVSPrinter1.CurrentY = objTellerCurrentY;
            //axVSPrinter1.MarginLeft = 7000;
            //axVSPrinter1.Table = string.Format("<2000;{0}", "        "); //AppSettingsManager.GetConfigValue("45")

            //axVSPrinter1.CurrentY = objTellerCurrentY;
            //axVSPrinter1.MarginLeft = 10000;
            //axVSPrinter1.Table = string.Format("<1000;{0}", "      Date");
            //axVSPrinter1.FontUnderline = false;

            //axVSPrinter1.CurrentY = objTellerCurrentY;
            //axVSPrinter1.MarginLeft = 11500;
            //axVSPrinter1.Table = string.Format("^0|>0;{0}|{1}", " ", " ");
            //REM MCR20150116 (e)

            //Next Line
            objTellerCurrentY = axVSPrinter1.CurrentY;
            axVSPrinter1.MarginLeft = 500;
            axVSPrinter1.Table = string.Format("^0|^0;{0}|{1}", " ", " ");

            axVSPrinter1.CurrentY = objTellerCurrentY;
            axVSPrinter1.MarginLeft = 1000;
            if (AppSettingsManager.GetConfigValue("01") == "CITY") // RMC 20161213 mods in RCD (s)
                axVSPrinter1.Table = string.Format("<6000|<2000;{0}|{1}", "  Accountable Officer", "City Treasurer");
            else// RMC 20161213 mods in RCD (e)
                axVSPrinter1.Table = string.Format("<6000|<2000;{0}|{1}", "  Accountable Officer","Municipal Treasurer"); //GMC 20150923 Municipal Treasurer

            //axVSPrinter1.CurrentY = objTellerCurrentY;
            //axVSPrinter1.MarginLeft = 11500;
            //axVSPrinter1.Table = string.Format("^0|>11000;{0}|{1}", " ", " ");

            //axVSPrinter1.MarginLeft = 500;
            //axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            //axVSPrinter1.Table = string.Format("^11000;{0}", " ");

            //axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBottom;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";    // RMC 20150501 QA BTAS
            axVSPrinter1.Paragraph = "Print Date: " + AppSettingsManager.GetCurrentDate().ToString("MMMM dd, yyyy");
            //axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            // RMC 20140909 Migration QA (s)
            if (bCheckSupple)
            {
                axVSPrinter1.NewPage();
                axVSPrinter1.FontSize = (float)10;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                axVSPrinter1.Table = string.Format("^0|<7000;|SUPPLEMENTAL PAGE FOR LIST OF CHECKS");
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;

                axVSPrinter1.Table = string.Format("^1833.33|^3000|^1833.33;CHECK NO|BANK|AMOUNT");

                for (int i = iChkRow; i != m_lstCheckNo.Count; i++)
                {
                    axVSPrinter1.Table = string.Format("<1833.33|<3000|>1833.33;{0}|{1}|{2:#,###.00}", m_lstCheckNo[i], m_lstBank[i], m_lstCheckAmt[i]);
                }
            }
            // RMC 20140909 Migration QA (E)

            //Next Page

            m_bNewPage = true; // RMC 20170117 correction in RCD page 2 table header 
            axVSPrinter1.NewPage();

            /*axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "OFFICIAL RECEIPT/TICKET - SERIAL NO.\n(FROM - TO)", "PAYOR", "PARTICULARS/\nDESCRIPTION", "AMOUNT IN\nPHP");
            axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
            axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
             */
            // RMC 20170117 correction in RCD page 2 table header, put rem

            List<string> lstORNo = new List<string>();
            List<string> lstPayor = new List<string>();
            List<string> lstAmount = new List<string>();
            

            axVSPrinter1.FontSize = (float)10;
            Payor(out lstORNo, out lstPayor, out lstAmount);

            // RMC 20170110 add sub-total per booklet in rcd per OR (s)
            double dSubTotal = 0;
            double dTmpAmt = 0;
            string sTmpLastOr = string.Empty;
            int iOrCnt = 0;
            try
            {
                sTmpLastOr = lstORLast[iOrCnt].ToString();

            }
            catch (Exception ex)
            { }
            // RMC 20170110 add sub-total per booklet in rcd per OR (e)

            for (int i = 0; i != lstORNo.Count; i++)
            {
                //axVSPrinter1.Table = string.Format("^2400|<4000|^2600|>2000;{0}|{1}|{2}|{3}", lstORNo[i], lstPayor[i], "LICENSE", lstAmount[i]);
                /*if (i == 35)
                {
                    axVSPrinter1.NewPage();
                    axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                    axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "OFFICIAL RECEIPT/TICKET - SERIAL NO.\n(FROM - TO)", "PAYOR", "PARTICULARS/\nDESCRIPTION", "AMOUNT IN\nPHP");
                    axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
                    axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
                }*/
                // RMC 20170117 correction in RCD page 2 table header, put rem
                axVSPrinter1.Table = string.Format("^2400|<4000|^2600|>2000;{0}|{1}|{2}|{3}", lstORNo[i], lstPayor[i], "FORM 51", lstAmount[i]); //go here

                // RMC 20170110 add sub-total per booklet in rcd per OR (S)
                double.TryParse(lstAmount[i].ToString(), out dTmpAmt);
                dSubTotal += dTmpAmt;
                if(sTmpLastOr == lstORNo[i].ToString())
                {
                    axVSPrinter1.FontBold = true;
                    axVSPrinter1.Table = string.Format("^2400|^4000|^2600|>2000;{0}|{1}|{2}|{3:#,###.00}", "", "", "SUB-TOTAL", dSubTotal);
                    axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
                    dSubTotal = 0;
                    if (iOrCnt < lstORLast.Count)
                    {
                        iOrCnt++;
                        try
                        {
                            sTmpLastOr = lstORLast[iOrCnt].ToString();
                        }
                        catch
                        {
                            sTmpLastOr = "";
                        }
                    }
                    axVSPrinter1.FontBold = false;
                }
                // RMC 20170110 add sub-total per booklet in rcd per OR (E)
            }

            // RMC 20170110 add sub-total per booklet in rcd per OR (s)
            if (dSubTotal > 0)
            {
                axVSPrinter1.FontBold = true;
                axVSPrinter1.Table = string.Format("^2400|^4000|^2600|>2000;{0}|{1}|{2}|{3:#,###.00}", "", "", "SUB-TOTAL", dSubTotal);
                axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
                axVSPrinter1.FontBold = false;
            }
            // RMC 20170110 add sub-total per booklet in rcd per OR (e)

            axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");

            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = string.Format("^2400|^4000|^2600|>2000;{0}|{1}|{2}|{3:#,###.00}", "", "", "TOTAL", dTotalAmount);
            axVSPrinter1.FontBold = false;

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            for (int i = 0; i != 2; i++) // 20
            {
                axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
            }

            //JARS 20170905 (S)
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "CANCELLED PAYMENTS", "PAYOR", "PARTICULARS/\nDESCRIPTION", "AMOUNT IN\nPHP");
            axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
            #region comments
            //for (int i = 0; i != lstORNo.Count; i++)
            //{
            //    #region comments
            //    //axVSPrinter1.Table = string.Format("^2400|<4000|^2600|>2000;{0}|{1}|{2}|{3}", lstORNo[i], lstPayor[i], "LICENSE", lstAmount[i]);
            //    /*if (i == 35)
            //    {
            //        axVSPrinter1.NewPage();
            //        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            //        axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "OFFICIAL RECEIPT/TICKET - SERIAL NO.\n(FROM - TO)", "PAYOR", "PARTICULARS/\nDESCRIPTION", "AMOUNT IN\nPHP");
            //        axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
            //        axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
            //    }*/
            //    #endregion
            //    //rs.Query = "select * from cancelled_payment where or_no = '"+ lstORNo[i] +"'";m_sRCDNo
            //    rs.Query = "select * from cancelled_payment where or_no between (select or_from from partial_remit where rcd_series = '"+ m_sRCDNo +"') and (select or_to from partial_remit where rcd_series = '"+ m_sRCDNo +"')";
            //    rs.
            //    if(rs.Execute())
            //    {
            //        while(rs.Read())
            //        {
            //            axVSPrinter1.Table = string.Format("^2400|<4000|^2600|>2000;{0}|{1}|{2}|{3}", rs.GetString("or_no"), lstPayor[i], "FORM 51", lstAmount[i]);
            //            double.TryParse(lstAmount[i].ToString(), out dTmpAmt);
            //            dSubTotal += dTmpAmt;
            //        }
            //    }
            //    // RMC 20170117 correction in RCD page 2 table header, put rem
            //    //axVSPrinter1.Table = string.Format("^2400|<4000|^2600|>2000;{0}|{1}|{2}|{3}", lstORNo[i], lstPayor[i], "FORM 51", lstAmount[i]);

            //    // RMC 20170110 add sub-total per booklet in rcd per OR (S)
            //    //double.TryParse(lstAmount[i].ToString(), out dTmpAmt);
            //    //dSubTotal += dTmpAmt;
            //    if (sTmpLastOr == lstORNo[i].ToString())
            //    {
            //        axVSPrinter1.FontBold = true;
            //        axVSPrinter1.Table = string.Format("^2400|^4000|^2600|>2000;{0}|{1}|{2}|{3:#,###.00}", "", "", "SUB-TOTAL", dSubTotal);
            //        axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
            //        dSubTotal = 0;
            //        if (iOrCnt < lstORLast.Count)
            //        {
            //            iOrCnt++;
            //            try
            //            {
            //                sTmpLastOr = lstORLast[iOrCnt].ToString();
            //            }
            //            catch
            //            {
            //                sTmpLastOr = "";
            //            }
            //        }
            //        axVSPrinter1.FontBold = false;
            //    }
            //    // RMC 20170110 add sub-total per booklet in rcd per OR (E)
            //}
            #endregion
            
            //JARS 20171010 (S) QA FOR CANCELLED PAYMENTS IN RCD
            double dCancTotal = 0;
            if (m_sSwitch == "New")
            {
                rs.Query = "select * from rcd_remit_tmp where teller_code = '"+m_sTellerCode+"'";
                if(rs.Execute())
                {
                    while(rs.Read())
                    {
                        rs2.Query = "select a.or_no,b.bns_nm,sum(c.fees_amtdue) as amount from cancelled_payment a left join businesses b on a.bin = b.bin left join cancelled_or c on a.or_no = c.or_no ";
                        rs2.Query += "where  ";
                        rs2.Query += "a.or_no between '"+rs.GetString("from_or_no")+"' and '"+ rs.GetString("to_or_no") +"' group by a.or_no,b.bns_nm";
                        if (rs2.Execute())
                        {
                            while(rs2.Read())
                            {
                                axVSPrinter1.Table = string.Format("^2400|<4000|^2600|>2000;{0}|{1}|{2}|{3}", rs2.GetString("or_no"), rs2.GetString("bns_nm"), "FORM 51", rs2.GetDouble("amount").ToString("#,##0.00"));
                                double.TryParse(rs2.GetDouble("amount").ToString(), out dTmpAmt);
                                dCancTotal += dTmpAmt;
                            }
                        }
                        rs2.Close();
                    }
                }
            }
            else
            {
                rs.Query = "select or_from,or_to from partial_remit where rcd_series = '"+ m_sRCDNo +"'";
                //rs.Query = "select a.or_no,b.bns_nm,sum(c.fees_amtdue) as amount from cancelled_payment a left join businesses b on a.bin = b.bin left join cancelled_or c on a.or_no = c.or_no ";
                //rs.Query += "where  ";
                //rs.Query += "a.or_no between (select or_from from partial_remit where rcd_series = '" + m_sRCDNo + "') ";
                //rs.Query += "and (select or_to from partial_remit where rcd_series = '" + m_sRCDNo + "') group by a.or_no,b.bns_nm";
                if (rs.Execute())
                {
                    while (rs.Read())
                    {
                        rs2.Query = "select a.or_no,b.bns_nm,sum(c.fees_amtdue) as amount from cancelled_payment a left join businesses b on a.bin = b.bin left join cancelled_or c on a.or_no = c.or_no ";
                        rs2.Query += "where  ";
                        rs2.Query += "a.or_no between '"+rs.GetString("or_from")+"' and '"+ rs.GetString("or_to") +"'";
                        rs2.Query += "group by a.or_no,b.bns_nm";

                        if(rs2.Execute())
                        {
                            while (rs2.Read())
                            {
                                axVSPrinter1.Table = string.Format("^2400|<4000|^2600|>2000;{0}|{1}|{2}|{3}", rs2.GetString("or_no"), rs2.GetString("bns_nm"), "FORM 51", rs2.GetDouble("amount").ToString("#,##0.00"));
                                double.TryParse(rs2.GetDouble("amount").ToString(), out dTmpAmt);
                                dCancTotal += dTmpAmt;
                            }
                        }
                        //axVSPrinter1.Table = string.Format("^2400|<4000|^2600|>2000;{0}|{1}|{2}|{3}", rs.GetString("or_no"), rs.GetString("bns_nm"), "FORM 51", rs.GetDouble("amount").ToString("#,##0.00"));
                        //double.TryParse(rs.GetDouble("amount").ToString(), out dTmpAmt);
                        //dCancTotal += dTmpAmt;
                    }
                }
                rs2.Close();
            }

            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = string.Format("^2400|^4000|^2600|>2000;{0}|{1}|{2}|{3:#,###.00}", "", "", "TOTAL", dCancTotal);
            axVSPrinter1.FontBold = false;

            rs.Close();
            //JARS 20171010 (E)
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //JARS 20170905 (E)


            axVSPrinter1.FontSize = (float)12;
            //axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.Table = string.Format(">9800; ");
            axVSPrinter1.Table = string.Format(">9800; ");
            axVSPrinter1.Table = string.Format(">9800; ");

            string strCurrentYTmp = axVSPrinter1.CurrentY.ToString();
            double dCurrentYTmp = 0;
            double.TryParse(strCurrentYTmp, out dCurrentYTmp);
            dCurrentYTmp = dCurrentYTmp + 300;
            axVSPrinter1.DrawLine(7300, dCurrentYTmp, 10600, dCurrentYTmp);

            axVSPrinter1.Table = string.Format(">9800;{0}", GetTeller());
            axVSPrinter1.Table = string.Format(">9800;{0}", "Name and Signature");
            axVSPrinter1.Table = string.Format(">9800;{0}", "Accountable Officer");

            
            

            axVSPrinter1.EndDoc();
        }

        private void Payor(out List<string> ORNo, out List<string> Payor, out List<string> Amount)
        {
            string strPayor = "";
            string strORNo = "";
            string strBIN = "";
            string strOwnCode = "";
            double dAmount = 0;
            List<string> lstORNo = new List<string>();
            List<string> lstPayor = new List<string>();
            List<string> lstAmount = new List<string>();
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();

            if (m_sSwitch == "New")
                //result.Query = "select distinct(or_no) from pay_hist where or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + m_sTellerCode + "') and teller = '" + m_sTellerCode + "' and data_mode <> 'POS'"; //MCR 20150129 added data_mode
                result.Query = "select distinct(or_no) from pay_hist where NOT EXISTS (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + m_sTellerCode + "') and teller = '" + m_sTellerCode + "' and data_mode <> 'POS'"; // AFM 20200618 changed condition for faster query
            else
                //result.Query = "select distinct(or_no) from pay_hist where or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + m_sTellerCode + "' and rcd_series = '" + m_sRCDNo + "') and teller = '" + m_sTellerCode + "' and data_mode <> 'POS'"; //MCR 20150129 added data_mode
                result.Query = "select distinct(or_no) from pay_hist where EXISTS (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + m_sTellerCode + "' and rcd_series = '" + m_sRCDNo + "') and teller = '" + m_sTellerCode + "' and data_mode <> 'POS'"; // AFM 20200618 changed condition for faster query
            result.Query += " order by or_no";  // RMC 20140909 Migration QA
            if (result.Execute())
            {
                while (result.Read())
                {
                    strORNo = result.GetString("or_no");
                    lstORNo.Add(strORNo);
                    string sPayType = string.Empty; // RMC 20140909 Migration QA

                    rs.Query = "select distinct * from pay_hist where or_no = '" + strORNo + "'";
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            strBIN = rs.GetString("bin");
                            sPayType = rs.GetString("payment_type").Trim(); // RMC 20140909 Migration QA
                        }
                    }
                    rs.Close();

                    rs.Query = "select * from businesses where bin = '" + strBIN + "'";
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            strOwnCode = rs.GetString("own_code");
                        }
                    }
                    rs.Close();

                    if (strOwnCode == "")
                    {
                        rs.Query = "select * from business_que where bin = '" + strBIN + "'";
                        if (rs.Execute())
                        {
                            while (rs.Read())
                            {
                                strOwnCode = rs.GetString("own_code");
                            }
                        }
                        rs.Close();
                    }

                    rs.Query = "select own_ln,own_fn,own_mi from own_names where own_code = '" + strOwnCode + "'";
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            strPayor = rs.GetString("own_fn") + " ";
                            strPayor += rs.GetString("own_mi") + ". ";
                            strPayor += rs.GetString("own_ln");
                            lstPayor.Add(strPayor);
                        }
                    }
                    rs.Close();

                    rs.Query = "select sum(fees_amtdue) as tot_fees from or_table where or_no = '" + strORNo + "'";
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            dAmount = rs.GetDouble("tot_fees");
                            /*string strAmtTmp = string.Format("{0:#,###.00}", dAmount);
                            lstAmount.Add(strAmtTmp);*/
                            // RMC 20140909 Migration QA, put rem
                        }
                    }
                    rs.Close();

                    // RMC 20140909 Migration QA (s)
                    double dTmp = 0;
                    rs.Query= "select sum(debit) as debit from dbcr_memo where memo like 'DEBITED THRU PAYMENT MADE HAVING OR_NO%' "; //JARS 20171010
                    rs.Query += " and teller = '" + m_sTellerCode + "' and served = 'Y' and multi_pay = 'N' ";
                    rs.Query += " and or_no in (select or_no from pay_hist where teller = '" + m_sTellerCode + "'";
                    if (m_sSwitch == "New")
                        rs.Query += " and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + m_sTellerCode + "'))";
                    else
                        rs.Query += " and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + m_sTellerCode + "' and rcd_series = '" + m_sRCDNo + "'))";
                    rs.Query += " and or_no = '" + strORNo + "'";
                    if (rs.Execute())
                    {
                        if (rs.Read())
                        {
                            dTmp = rs.GetDouble("debit");
                            dAmount = dAmount - dTmp;
                        }
                    }
                    rs.Close();
						
                    /*rs.Query = "select sum(credit) as credit from dbcr_memo where (memo like 'REMAINING BALANCE AFTER PAYMENT W/ DEBIT HAVING%'";	
				    rs.Query+= " or memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%')";*/
                    rs.Query = "select sum(credit) as credit from dbcr_memo where "; //JARS 20171010
                    rs.Query += " memo like 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR%'";
                    rs.Query += " and teller = '" + m_sTellerCode + "' and served = 'N' and multi_pay = 'N' ";
                    rs.Query += " and or_no in (select or_no from pay_hist where teller = '" + m_sTellerCode + "'";
                    if (m_sSwitch == "New")
				        rs.Query+= " and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + m_sTellerCode + "'))";
                    else
                        rs.Query+= " and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + m_sTellerCode + "' and rcd_series = '" + m_sRCDNo + "'))";
                    rs.Query += " and or_no = '" + strORNo + "'";
                    if (rs.Execute())
                    {
                        if (rs.Read())
                        {
                            dTmp = rs.GetDouble("credit");
                            dAmount = dAmount + dTmp;
                        }
                    }
                    rs.Close();

                    rs.Query = "select sum(credit) as credit from dbcr_memo where "; //JARS 20171010
                    rs.Query += " memo like 'REMAINING BALANCE AFTER PAYMENT W/ DEBIT HAVING%'";
                    rs.Query += " and teller = '" + m_sTellerCode + "' and served = 'N' and multi_pay = 'N' ";
                    rs.Query += " and or_no in (select or_no from pay_hist where teller = '" + m_sTellerCode + "'";
                    if (m_sSwitch == "New")
                        rs.Query += " and or_no not in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + m_sTellerCode + "'))";
                    else
                        rs.Query += " and or_no in (select or_no from rcd_remit where or_no = pay_hist.or_no and teller = '" + m_sTellerCode + "' and rcd_series = '" + m_sRCDNo + "'))";
                    rs.Query += " and or_no = '" + strORNo + "'";
                    if (rs.Execute())
                    {
                        if (rs.Read())
                        {
                            if (sPayType == "TC")
                                dTmp = 0;
                            else
                                dTmp = rs.GetDouble("credit");
                            dAmount = dAmount + dTmp;
                        }
                    }
                    rs.Close();

                    string strAmtTmp = string.Format("{0:#,###.00}", dAmount);
                    lstAmount.Add(strAmtTmp);
                    // RMC 20140909 Migration QA (e)
                }
            }
            result.Close();

            ORNo = lstORNo;
            Payor = lstPayor;
            Amount = lstAmount;
        }

        private void PayorInfoTmp() // Tmp
        {
            axVSPrinter1.NewPage();
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.Table = string.Format("^10000;{0}", "Republic of the Philippines");
            axVSPrinter1.Table = string.Format("^10000;{0}", "PROVINCE OF " + AppSettingsManager.GetConfigValue("PROV"));

            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = string.Format("^10000;{0}", AppSettingsManager.GetConfigValue("LGUN"));
            axVSPrinter1.FontBold = false;

            axVSPrinter1.Paragraph = "";

            axVSPrinter1.Table = string.Format("^10000;{0}", AppSettingsManager.GetConfigValue("MKTN"));

            //axVSPrinter1.Table = string.Format("^10000;{0}", "REPORTS OF COLLECTION AND DEPOSITS");

            axVSPrinter1.Table = string.Format("^10000;{0}", "RCD Per O.R.");
            axVSPrinter1.Paragraph = "";

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
            LoadPayor loadpayor = new LoadPayor(m_sTellerCode, m_dtDateTrans, m_sSwitch, m_sRCDNo);
            axVSPrinter1.Table = string.Format("^2500|^2500|^2500|^2500;{0}", "O.R. No.|Payor|Fee Type|Amount;");
            string strData = "";
            for (int i = 0; i != loadpayor.ORSeries().Count; i++)
            {
                strData = loadpayor.ORSeries()[i].ToString() + "|";
                strData += loadpayor.Name()[i].ToString() + "|";
                strData += loadpayor.FeeType()[i].ToString() + "|";
                strData += string.Format("{0:#,###.00}", loadpayor.Amount()[i]) + ";";
                axVSPrinter1.Table = string.Format("^2500|<2500|<2500|>2500;{0}", strData);
            }
        }

        private void GetMonthString()
        {
            htMonth.Clear();
            htMonth.Add(1, "January");
            htMonth.Add(2, "February");
            htMonth.Add(3, "March");
            htMonth.Add(4, "April");
            htMonth.Add(5, "May");
            htMonth.Add(6, "June");
            htMonth.Add(7, "July");
            htMonth.Add(8, "August");
            htMonth.Add(9, "September");
            htMonth.Add(10, "October");
            htMonth.Add(11, "November");
            htMonth.Add(12, "December");
        }

        private void btnRemit_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            String sQuery = "", sRCDSeries, sErrRemitTo = "";
            String sTeller = m_sTellerCode;

            string m_s1000 = "0", m_s500 = "0", m_s200 = "0", m_s100 = "0", m_s50 = "0", m_s20 = "0", m_s10 = "0";
            string m_s10Coins = "0", m_s5Coins = "0", m_s1Coins = "0", m_s50Cents = "0", m_s25Cents = "0", m_s10Cents = "0", m_s5Cents = "0", m_s1Cents = "0";

            //sRCDSeries = "05-" + GetRCDSeries();

            if (MessageBox.Show("Are you sure you want to remit this collection?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //sRCDSeries = "05-" + GetRCDSeries();    // RMC 20140909 Migration QA
                sRCDSeries = GetRCDSeries();    // RMC 20150107
                m_sRCDSeries = sRCDSeries;  // RMC 20140909 Migration QA
                m_sRCDNo = sRCDSeries;  // RMC 20140909 Migration QA

                //Added by EMS 01022005 (s)
                string sTemp;
                String sCurrentUser = AppSettingsManager.SystemUser.UserCode;
                frmLogIn frmLogIn = new frmLogIn();
                frmLogIn.ShowDialog();
                sTemp = frmLogIn.m_objSystemUser.UserCode;
                if (sTemp.Trim() != "")
                {
                    if (AppSettingsManager.Granted("CCLDA"))
                    {
                        AuditTrail.InsertTrail("CCLDA", "trail_table/a_trail", AppSettingsManager.SystemUser.UserCode + "/" + sTeller);
                        AppSettingsManager.SystemUser.UserCode = sCurrentUser;
                    }
                    else
                    {
                        AppSettingsManager.SystemUser.UserCode = sCurrentUser;
                        return;
                    }
                }
                else
                {
                    AppSettingsManager.SystemUser.UserCode = sCurrentUser;
                    return;
                }

                String sOldOrFrom = "", sOldOrTo = "", sOrRemitTo = ""; 
                String sQry = "", sQryTo = "";
                String sRemitSeries = "";

                /*sQry = "select or_series, from_or_no, to_or_no from or_assigned_hist where or_series || from_or_no not in ";
                sQry += "(select or_from from partial_remit where teller = '" + m_sTellerCode + "' and or_date <= to_date('" + m_dtDateTrans.ToString("MM/dd/yyyy") + "','MM/dd/yyyy')) ";
                sQry += "and trn_date <= to_date('" + m_dtDateTrans.ToString("MM/dd/yyyy") + "','MM/dd/yyyy') and teller = '" + m_sTellerCode + "'"; // MOD > to < date
                pSet.Query = sQry;*/

                // RMC 20140909 Migration QA (s)
                //pSet.Query = "select min(from_or_no) as from_or_no, max(to_or_no) as to_or_no";
                pSet.Query = "select from_or_no as from_or_no, to_or_no as to_or_no";   // RMC 20150915 corrections in print of RCD
                if (AppSettingsManager.GetConfigValue("10") == "021" || AppSettingsManager.GetConfigValue("56") == "Y") // RMC 20141203 corrected error in online payment
                    pSet.Query += ", or_series as or_series ";
                else
                    pSet.Query += ", null as or_series ";
                pSet.Query += "from or_assigned_hist where teller not in (select teller from rcd_remit ";
                pSet.Query += "where teller = '" + m_sTellerCode + "' ";
                if (AppSettingsManager.GetConfigValue("10") == "021")
                    pSet.Query += "and to_number(substr(or_no,3,20)) between or_assigned_hist.from_or_no and or_assigned_hist.to_or_no and substr(or_no,1,2) = or_assigned_hist.or_series ";
                else
                {
                    if (AppSettingsManager.GetConfigValue("56") == "Y") // RMC 20141203 corrected error in online payment
                        pSet.Query += "and or_no between or_assigned_hist.from_or_no||or_assigned_hist.or_series and or_assigned_hist.to_or_no||or_assigned_hist.or_series ";
                    else
                        pSet.Query += "and or_no between or_assigned_hist.from_or_no and or_assigned_hist.to_or_no ";
                }
                pSet.Query += "and or_no in (select or_no from pay_hist where teller = '" + m_sTellerCode + "')) and teller = '" + m_sTellerCode + "' ";
                pSet.Query += "and (from_or_no,to_or_no) not in (select or_no,or_no from cancelled_payment where or_no between or_assigned_hist.from_or_no and or_assigned_hist.to_or_no and teller = '" + m_sTellerCode + "') ";
                if (AppSettingsManager.GetConfigValue("56") == "Y")     // RMC 20141217 adjustments
                    pSet.Query += "group by or_series order by or_series";
                pSet.Query += " order by from_or_no";   // RMC 20150915 corrections in print of RCD
                // RMC 20140909 Migration QA (e)
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        /*sOldOrFrom = pSet.GetInt("from_or_no").ToString().Trim();
                        sOldOrTo = pSet.GetInt("to_or_no").ToString().Trim();*/
                        // RMC 20150501 QA BTAS (s)
                        //sOldOrFrom = pSet.GetString("from_or_no").ToString().Trim();
                        //sOldOrTo = pSet.GetString("to_or_no").ToString().Trim();
                        // RMC 20150501 QA BTAS (e)
                        sOldOrFrom = pSet.GetString("from_or_no").Trim(); //JARS 20171003
                        sOldOrTo = pSet.GetString("to_or_no").Trim();
                        if (AppSettingsManager.GetConfigValue("56") == "Y")
                            sRemitSeries = pSet.GetString("or_series").Trim();
                        else
                            sRemitSeries = "";

                        /*pSet2.Query = "select to_char(max(or_no)) as maxor, to_char(min(or_no)) as minor from or_used ";
                        pSet2.Query += string.Format("where or_no between '{0}' and '{1}' and teller = '{2}' and teller not in ", sRemitSeries + sOldOrFrom, sRemitSeries + sOldOrTo, m_sTellerCode);
                        pSet2.Query += "(select teller from partial_remit where or_used.or_no between partial_remit.or_from and partial_remit.or_to and teller = '" + m_sTellerCode + "') ";
                        pSet2.Query += "having to_char(Max(or_no)) is not null"; //ADD MCR 20140626*/

                        // RMC 20140909 Migration QA (s)
                        //if (sRemitSeries.Trim() != "")
                        if (sRemitSeries.Trim() != "" && AppSettingsManager.GetConfigValue("10") == "021")  // RMC 20141203 corrected error in online payment
                        {
                            // this is for Antipolo version wherein they used OR series code bec of duplication of OR series (error by printing press)
                            pSet2.Query = "select to_char(max(to_number(substr(or_no,3,20)))) as maxor, to_char(min(to_number(substr(or_no,3,20)))) as minor from or_used ";
                            pSet2.Query += "where to_number(substr(or_no,3,20)) between '" + sOldOrFrom + "' and '" + sOldOrTo + "' and or_no like '" + sRemitSeries + "%' ";
                            pSet2.Query += "and teller = '" + m_sTellerCode + "' and teller not in ";
                            pSet2.Query += "(select teller from partial_remit where to_number(substr(or_used.or_no,3,20)) ";
                            pSet2.Query += "between to_number(substr(partial_remit.or_from,3,20)) and to_number(substr(partial_remit.or_to,3,20)) ";
                            pSet2.Query += "and partial_remit.or_from like '" + sRemitSeries + "%' and teller = '" + m_sTellerCode + "') ";
                            pSet2.Query += "having to_char(Max(or_no)) is not null";
                        }
                        else
                        {
                            pSet2.Query = "select to_char(max(to_number(or_no))) as maxor, to_char(min(to_number(or_no))) as minor from or_used "; //AFM 20200124 changed datatype
                            // RMC 20141203 corrected error in online payment (s)
                            if (AppSettingsManager.GetConfigValue("56") == "Y")
                            {
                                pSet2.Query += string.Format("where or_no between '{0}' and '{1}' and teller = '{2}' and or_series = '{3}' and teller not in ", sOldOrFrom, sOldOrTo, m_sTellerCode, sRemitSeries);
                                pSet2.Query += "(select teller from partial_remit where or_used.or_no||or_series between partial_remit.or_from and partial_remit.or_to and teller = '" + m_sTellerCode + "') ";
                            }
                            else    // RMC 20141203 corrected error in online payment (e)
                            {
                                pSet2.Query += string.Format("where or_no between '{0}' and '{1}' and teller = '{2}' and teller not in ", sOldOrFrom, sOldOrTo, m_sTellerCode);
                                pSet2.Query += "(select teller from partial_remit where or_used.or_no between partial_remit.or_from and partial_remit.or_to and teller = '" + m_sTellerCode + "') ";
                            }
                            pSet2.Query += "having to_char(Max(to_number(or_no))) is not null"; //AFM 20200124 changed datatype
                        }
                        // RMC 20140909 Migration QA (e)
                        if (pSet2.Execute())
                            if (pSet2.Read())
                            {
                                // RMC 20140909 Migration QA (s)
                                sOldOrFrom = pSet2.GetString("minor");
                                sOldOrTo = pSet2.GetString("maxor");
                                // RMC 20140909 Migration QA (e)

                                if (sRemitSeries != "") 
                                {
                                    if (AppSettingsManager.GetConfigValue("10") == "021")  // RMC 20141203 corrected error in online payment
                                    {
                                        sOldOrFrom = sRemitSeries + sOldOrFrom;
                                        sOldOrTo = sRemitSeries + sOldOrTo;
                                    }// RMC 20141203 corrected error in online payment (s)
                                    else if (AppSettingsManager.GetConfigValue("56") == "Y")
                                    {
                                        sOldOrFrom = sOldOrFrom + sRemitSeries;
                                        sOldOrTo = sOldOrTo + sRemitSeries;
                                    }
                                    // RMC 20141203 corrected error in online payment (e)
                                }

                                /*sQryTo = "select max(or_no) as or_no_remit from or_used where teller = '" + m_sTellerCode + "' ";
                                sQryTo += "and or_no in(select or_no from or_assigned_hist where or_no between '" + sOldOrFrom + "' ";
                                sQryTo += "and '" + sOldOrTo + "')";
                                pSet2.Query = sQryTo;
                                if (pSet2.Execute())
                                    if (pSet2.Read())
                                        sOrRemitTo = pSet2.GetString("or_no_remit").Trim();
                                pSet2.Close();*/

                                sOrRemitTo = sOldOrTo;  // RMC 20140909 Migration QA
                                

                                for (int i = 0; i < m_lstDenominations.Count; i++)
                                {
                                    if (m_lstDenominations[i].ToString() == "1,000.00")
                                        m_s1000 = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == "500.00")
                                        m_s500 = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == "200.00")
                                        m_s200 = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == "100.00")
                                        m_s100 = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == "50.00")
                                        m_s50 = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == "20.00")
                                        m_s20 = m_lstDenominationsQty[i].ToString();
                                    //else if (m_lstDenominations[i].ToString() == "10.00")
                                    //    m_s10 =  m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == "10.00")
                                        m_s10Coins = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == "5.00")
                                        m_s5Coins = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == "1.00")
                                        m_s1Coins = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == ".50")
                                        m_s50Cents = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == ".25")
                                        m_s25Cents = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == ".10")
                                        m_s10Cents = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == ".05")
                                        m_s5Cents = m_lstDenominationsQty[i].ToString();
                                    else if (m_lstDenominations[i].ToString() == ".01")
                                        m_s1Cents = m_lstDenominationsQty[i].ToString();
                                }

                                //sQuery = string.Format(@"insert into partial_remit values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}',to_date('{22}','MM/dd/yyyy HH24:MI:SS'),to_date('{23}','MM/dd/yyyy HH24:MI:SS'))",
                                sQuery = string.Format(@"insert into partial_remit(teller,rcd_series, p1000, p500, p200, p100, p50, P20,P10,C10, C5,C1, C005, C0025, C001,C0005, C0001,TOTAL_CASH, TOTAL_CHECK,TOTAL_COLLECTION,OR_FROM,OR_TO,OR_DATE,DT_SAVE) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}',to_date('{22}','MM/dd/yyyy HH24:MI:SS'),to_date('{23}','MM/dd/yyyy HH24:MI:SS'))", // JAA 20190212 added column names
                                m_sTellerCode, sRCDSeries,
                                m_s1000, m_s500, m_s200,
                                m_s100, m_s50, m_s20,
                                m_s10, m_s10Coins, m_s5Coins,
                                m_s1Coins, m_s50Cents, m_s25Cents,
                                m_s10Cents, m_s5Cents, m_s1Cents,
                                m_dCashAmount, m_dCheckAmount, m_dTotalCollect,
                                //sOldOrFrom, sOrRemitTo,
                                sOldOrFrom, sOldOrTo,   // RMC 20140909 Migration QA
                                m_dtDateTrans.ToString("MM/dd/yyyy HH:mm:ss"), AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy HH:mm:ss"));
                                pSet1.Query = sQuery;
                                pSet1.ExecuteNonQuery();

                            }
                        pSet2.Close();
                    }
                }
                pSet.Close();

                String sORNo;

                sQuery = string.Format("select unique(a.or_no) as or_number from or_table a, pay_hist b where b.teller = '{0}' and  a.or_no = b.or_no and b.or_no not in (select or_no from rcd_remit where teller = '{0}' and or_no = b.or_no)", m_sTellerCode);	// JGR 09192005 Oracle Adjustment // GDE 20090226 add teller filter in rcd_remit
                pSet.Query = sQuery;
                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        sORNo = pSet.GetString("or_number");
                        sQuery = string.Format("insert into rcd_remit values('{0}','{1}','{2}')", sRCDSeries, m_sTellerCode, sORNo);
                        pSet1.Query = sQuery;
                        pSet1.ExecuteNonQuery();
                    }
                pSet.Close();

                if (sErrRemitTo.Trim() != "")
                {
                    sQuery = "select * from rcd_remit where or_no = '" + sErrRemitTo + "' and teller = '" + m_sTellerCode + "'";
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (!pSet.Read())
                        {
                            sQuery = string.Format("insert into rcd_remit values('{0}','{1}','{2}')", sRCDSeries, m_sTellerCode, sErrRemitTo);
                            pSet1.Query = sQuery;
                            pSet1.ExecuteNonQuery();
                        }
                    pSet.Close();
                }

                //MessageBox.Show("Remittance Successfully", "Remit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Collection Remitted", "Remit", MessageBoxButtons.OK, MessageBoxIcon.Information);  // RMC 20140909 Migration QA
                this.Close();

            }

            btnPrint.Enabled = true; //JARS 20170831
        }

        private void Trail(OracleResultSet RSet, string sUser, string sModCode, string sTable, string sObject)
        {
            if (AuditTrail.InsertTrail(sModCode, sTable, sObject) == 0)
            {
                m_Set.Rollback();
                m_Set.Close();
                MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to print?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            axVSPrinter1.PrintDoc(1, 1, axVSPrinter1.PageCount);
            if (btnPrint.Text == "&Print")
                Trail(m_Set, m_sUser, "RCD-P", "Payment", "Print: RCD");
            else
                Trail(m_Set, m_sUser, "RCD-RP", "Payment", "Re-print: RCD");
        }

        private void btnRemit_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remit?", "Remit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            OracleResultSet result = new OracleResultSet();
            result.Query = "Select distinct(or_no), date_paid from payment where trim(teller) = '" + m_sTellerCode + "' ";
            result.Query += "and date_paid between TO_DATE('" + m_dtDateTrans.ToShortDateString() + "','MM/dd/yyyy') ";
            result.Query += "and TO_DATE('" + AppSettingsManager.GetCurrentDate().ToShortDateString() + "', 'MM/dd/yyyy') ";
            result.Query += "and trn_type <> 'POS' ";
            result.Query += "and or_no not in (select or_no from rcd_remit) ";
            result.Query += "order by or_no asc";
            if (result.Execute())
            {
                int iRCDNo = GetRCDNum();
                while (result.Read())
                {
                    OracleResultSet rs = new OracleResultSet();
                    rs.Query = "insert into rcd_remit (rcd_series, teller, or_no, or_dt, remit_dt) values(";
                    rs.Query += "" + iRCDNo + ",";
                    rs.Query += "'" + m_sTellerCode + "',";
                    rs.Query += "'" + result.GetString(0) + "',";
                    rs.Query += "to_date('" + result.GetDateTime(1).ToShortDateString() + "', 'MM/dd/yyyy'),";
                    rs.Query += "to_date('" + AppSettingsManager.GetCurrentDate().ToShortDateString() + "', 'MM/dd/yyyy'))";
                    rs.ExecuteNonQuery();
                }
                MessageBox.Show("Remittance Successfully", "Remit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private int GetRCDNum()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select REMIT_SEQ.nextval as rcd_no from dual";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    return pSet.GetInt(0);
                }
            }

            return 0;
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            axVSPrinter1.MarginTop = 500;
            axVSPrinter1.MarginBottom = 500;
            axVSPrinter1.MarginLeft = 500;
            axVSPrinter1.MarginRight = 500;

            axVSPrinter1.FontSize = (float)12;
            
            axVSPrinter1.FontName = "ARIAL";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.MarginLeft = 1000;
            axVSPrinter1.MarginRight = 1000;

            //if (m_sSwitch != "New")// && m_bPrintOnce == false)
            if (m_sSwitch != "New" && m_sSwitch != "Print-New") // RMC 20140909 Migration QA
            {
                axVSPrinter1.Table = string.Format("<10000;{0}", "RE-PRINTED RCD");
                m_bPrintOnce = true;
            }

            axVSPrinter1.Table = string.Format("^10000;{0}", "Republic of the Philippines");
            //axVSPrinter1.Table = string.Format("^10000;{0}", "PROVINCE OF " + AppSettingsManager.GetConfigValue("PROV"));
            if(AppSettingsManager.GetConfigValue("08") != "")
                axVSPrinter1.Table = string.Format("^10000;{0}", "PROVINCE OF " + AppSettingsManager.GetConfigValue("08"));
            axVSPrinter1.FontBold = true;
            //axVSPrinter1.Table = string.Format("^10000;{0}", AppSettingsManager.GetConfigValue("02"));
            axVSPrinter1.Table = string.Format("^10000;{0}", AppSettingsManager.GetConfigValue("09"));
            axVSPrinter1.FontBold = false;
            axVSPrinter1.FontUnderline = true;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = string.Format("^10000;{0}", "REPORTS OF COLLECTION AND REMITTANCE");
            axVSPrinter1.FontBold = false;
            axVSPrinter1.FontUnderline = false;
            //axVSPrinter1.Paragraph = "";
            axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
            axVSPrinter1.MarginLeft = 500;
            axVSPrinter1.MarginRight = 500;

            axVSPrinter1.FontSize = (float)10;  // MCR 20140115
            if (m_sRCDNo.Trim() != "")
            {
                OracleResultSet pSet = new OracleResultSet();
                string sDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                DateTime dtDate;
                if (AppSettingsManager.GetConfigValue("56") == "Y")
                {
                    //pSet.Query = "select distinct trim(pr.dt_save) from or_assigned_hist oah, partial_remit pr";
                    pSet.Query = "select distinct (pr.dt_save) from or_assigned_hist oah, partial_remit pr";    // RMC 20150707 added filtering of RCD date in re-print RCD
                    pSet.Query += string.Format(" where oah.teller = '{0}' and pr.or_from between oah.or_series||oah.from_or_no and oah.or_series||oah.to_or_no", m_sTellerCode);
                }
                else
                {
                    //pSet.Query = "select distinct trim(pr.dt_save) from or_assigned_hist oah, partial_remit pr";
                    pSet.Query = "select distinct (pr.dt_save) from or_assigned_hist oah, partial_remit pr"; // RMC 20150707 added filtering of RCD date in re-print RCD
                    pSet.Query += string.Format(" where oah.teller = '{0}' and pr.or_from between oah.from_or_no and oah.to_or_no", m_sTellerCode);
                }
                pSet.Query += " and rcd_series = '" + m_sRCDNo + "'";   // RMC 20150707 added filtering of RCD date in re-print RCD
                if (pSet.Execute())
                {
                    if (pSet.Read())
                        sDate = pSet.GetDateTime("dt_save").ToString("MM/dd/yyyy");
                }
                pSet.Close();

                m_dtDateTrans = Convert.ToDateTime(sDate);  // RMC 20150707 added filtering of RCD date in re-print RCD

                axVSPrinter1.Table = string.Format("<5500|>5500;{0}", "FUND: General Fund|DATE:  " + sDate + "");
                //axVSPrinter1.Table = string.Format("<7000|>4000;{0}", "Name of Accountable Officer:  " + Remittance.GetTellerName(m_sTellerCode) + "|REPORT NO.:05-" + m_sRCDNo);
                axVSPrinter1.Table = string.Format("<7000|>4000;{0}", "Name of Accountable Officer:  " + Remittance.GetTellerName(m_sTellerCode) + "|REPORT NO.:" + m_sRCDNo);  // RMC 20140909 Migration QA
            }
            else
            {
                axVSPrinter1.Table = string.Format("<5500|>5500;{0}", "FUND: General Fund|DATE:  " + m_dtDateTrans.ToString("MM/dd/yyyy") + "");
                //axVSPrinter1.Table = string.Format("<7000|>4000;{0}", "Name of Accountable Officer:  " + Remittance.GetTellerName(m_sTellerCode) + "|REPORT NO.:05-" + GetRCDSeries());
                axVSPrinter1.Table = string.Format("<7000|>4000;{0}", "Name of Accountable Officer:  " + Remittance.GetTellerName(m_sTellerCode) + "|REPORT NO.:"); // RMC 20140909 Migration QA
            }

            axVSPrinter1.FontSize = (float)10;  // RMC 20150107

            // RMC 20170117 correction in RCD page 2 table header (s)
            if (m_bNewPage)
            {
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "OFFICIAL RECEIPT/TICKET - SERIAL NO.\n(FROM - TO)", "PAYOR", "PARTICULARS/\nDESCRIPTION", "AMOUNT IN\nPHP");
                axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
                axVSPrinter1.Table = string.Format("^2400|^4000|^2600|^2000;{0}|{1}|{2}|{3}", "", "", "", "");
                axVSPrinter1.FontSize = (float)10;
            }
            // RMC 20170117 correction in RCD page 2 table header (e)
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // RMC 20140909 Migration QA
            if (m_sSwitch == "New" && m_sRCDSeries.Trim() == "")
            {
                if (MessageBox.Show("Collection not yet remitted. Continue closing?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                else
                    this.Close();
            }
            else
                this.Close();
        }

        private void PopulateDenomination()
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from partial_remit where teller = '" + m_sTellerCode + "' and rcd_series = '" + m_sRCDSeries + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    //paper P1000  P500  P200  P100  P50  P20  P10
                    //coins C10  C5  C1  C005  C0025  C001  C0005  C0001
                    int P1000 = 0; int P500 = 0; int P200 = 0; int P100 = 0; int P50 = 0; int P20 = 0; int P10 = 0;
                    int C10 = 0; int C5 = 0; int C1 = 0; int C005 = 0; int C0025 = 0; int C001 = 0; int C0005 = 0; int C0001 = 0;

                    P1000 = pSet.GetInt("P1000");
                    P500 = pSet.GetInt("P500");
                    P200 = pSet.GetInt("P200");
                    P100 = pSet.GetInt("P100");
                    P50 = pSet.GetInt("P50");
                    P20 = pSet.GetInt("P20");
                    P10 = pSet.GetInt("P10");

                    C10 = pSet.GetInt("C10");
                    C5 = pSet.GetInt("C5");
                    C1 = pSet.GetInt("C1");
                    C005 = pSet.GetInt("C005");
                    C0025 = pSet.GetInt("C0025");
                    C001 = pSet.GetInt("C001");
                    C0005 = pSet.GetInt("C0005");
                    C0001 = pSet.GetInt("C0001");

                    double dAmt = 0;
                    string sAmt = string.Empty;
                    if (P1000 != 0)
                    {
                        m_lstDenominations.Add("1000");
                        m_lstDenominationsQty.Add(P1000.ToString());
                        dAmt = 1000 * P1000;
                        sAmt = string.Format("{0:#,###.00}",dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (P500 != 0)
                    {
                        m_lstDenominations.Add("500");
                        m_lstDenominationsQty.Add(P500.ToString());
                        dAmt = 500 * P500;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (P200 != 0)
                    {
                        m_lstDenominations.Add("200");
                        m_lstDenominationsQty.Add(P200.ToString());
                        dAmt = 200 * P200;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (P100 != 0)
                    {
                        m_lstDenominations.Add("100");
                        m_lstDenominationsQty.Add(P100.ToString());
                        dAmt = 100 * P100;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (P50 != 0)
                    {
                        m_lstDenominations.Add("50");
                        m_lstDenominationsQty.Add(P50.ToString());
                        dAmt = 50 * P50;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (P20 != 0)
                    {
                        m_lstDenominations.Add("20");
                        m_lstDenominationsQty.Add(P20.ToString());
                        dAmt = 20 * P20;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (P10 != 0)
                    {
                        m_lstDenominations.Add("10");
                        m_lstDenominationsQty.Add(P10.ToString());
                        dAmt = 10 * P10;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }
                    
                    if (C10 != 0)
                    {
                        m_lstDenominations.Add("10");
                        m_lstDenominationsQty.Add(C10.ToString());
                        dAmt = 10 * C10;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }
                    
                    if (C5 != 0)
                    {
                        m_lstDenominations.Add("5");
                        m_lstDenominationsQty.Add(C5.ToString());
                        dAmt = 5 * C5;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (C1 != 0)
                    {
                        m_lstDenominations.Add("1");
                        m_lstDenominationsQty.Add(C1.ToString());
                        dAmt = 1 * C1;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (C005 != 0)
                    {
                        m_lstDenominations.Add(".50");
                        m_lstDenominationsQty.Add(C005.ToString());
                        dAmt = .50 * C005;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (C0025 != 0)
                    {
                        m_lstDenominations.Add(".25");
                        m_lstDenominationsQty.Add(C0025.ToString());
                        dAmt = .25 * C0025;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (C001 != 0)
                    {
                        m_lstDenominations.Add(".10");
                        m_lstDenominationsQty.Add(C001.ToString());
                        dAmt = .10 * C001;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (C0001 != 0)
                    {
                        m_lstDenominations.Add(".01");
                        m_lstDenominationsQty.Add(C0001.ToString());
                        dAmt = .01 * C0001;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }

                    if (C0005 != 0)
                    {
                        m_lstDenominations.Add(".05");
                        m_lstDenominationsQty.Add(C0005.ToString());
                        dAmt = .05 * C0005;
                        sAmt = string.Format("{0:#,###.00}", dAmt);
                        m_lstDenominationsAmt.Add(sAmt);
                    }
                }
            }
            pSet.Close();

        }

        private void InsertTmp(string sOrFr, string sOrTo, string sFormType)
        {
            // RMC 20150504 QA corrections 
            OracleResultSet pTRec = new OracleResultSet();
            OracleResultSet pCmd = new OracleResultSet();

            pTRec.Query = "select * from rcd_remit_tmp where teller_code = '" + m_sTellerCode + "'";
            pTRec.Query += " and '" + sOrFr + "' between from_or_no and to_or_no";
            pTRec.Query += " and form_type = '" + sFormType + "'";
	        if(pTRec.Execute())
            {
                if (!pTRec.Read())   // not found
                {
                    // RMC 20170117 correction in OR Series beginnin in RCD (s)
                    OracleResultSet pTmp = new OracleResultSet();

                    pTmp.Query = "select * from or_assigned_hist where teller = '" + m_sTellerCode + "'";
                    pTmp.Query += " and '" + sOrTo + "' between from_or_no and to_or_no and trn_date <= to_date('" + m_dtDateTrans.ToString("MM/dd/yyyy") + "','MM/dd/yyyy')";
                    pTmp.Query += " order by trn_date desc";
                    if (pTmp.Execute())
                    {
                        if (pTmp.Read())
                        {
                            if (Convert.ToInt32(sOrTo) < Convert.ToInt32(pTmp.GetString("to_or_no")))
                                sOrTo = pTmp.GetString("to_or_no");
                            ////JARS 20170712 (S)
                            //if (Convert.ToInt32(sOrTo) < pTmp.GetInt("to_or_no"))
                            //    sOrTo = pTmp.GetInt("to_or_no").ToString();
                            ////JARS 20170712 (E)
                        }
                    }
                    pTmp.Close();
                    // RMC 20170117 correction in OR Series beginnin in RCD (e)

                    pCmd.Query = "insert into rcd_remit_tmp values (";
                    pCmd.Query += "'" + m_sTellerCode + "', ";
                    pCmd.Query += "'" + sOrFr + "', ";
                    pCmd.Query += "'" + sOrTo + "', ";
                    pCmd.Query += "'" + sFormType + "')";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    

                    lstORLast.Add(sOrTo);   // RMC 20170110 add sub-total per booklet in rcd per OR
                }
            }
            pTRec.Close();
        }

        private string GetEndingOR(string sORNo, string sTeller)
        {
            // RMC 20151230 corrected OR ending balance in RCD
            OracleResultSet pOR = new OracleResultSet();
            string sEndOR = string.Empty;
            
            pOR.Query = "select * from or_assigned_hist where teller = '" + sTeller + "'";
            pOR.Query += " and '" + sORNo + "' between  from_or_no and to_or_no order by to_or_no desc";
            if (pOR.Execute())
            {
                if (pOR.Read())
                    sEndOR = pOR.GetString("to_or_no");
                    //sEndOR = pOR.GetInt("to_or_no").ToString(); //JARS 20170712
            }
            pOR.Close();

            return sEndOR;
        }

    }
}
