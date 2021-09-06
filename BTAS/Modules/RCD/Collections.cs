using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amellar.Common.DataConnector;
using System.Windows.Forms;

namespace Amellar.Modules.RCD
{
    abstract class Collections
    {
        public abstract List<string> FormType();
        public abstract List<int> ORNo();
        public abstract List<string> ORFrom();
        public abstract List<string> ORTo();
        public abstract List<double> Amount();
    }

    class LoadCollection : Collections
    {
        List<string> sFormType = new List<string>();
        List<int> nORNo = new List<int>();
        List<string> sORFr = new List<string>();
        List<string> sORTo = new List<string>();
        List<double> dAmount = new List<double>();
        bool m_blnHasRecord;

        public bool HasRecord
        {
            get { return m_blnHasRecord; }
        }
        public LoadCollection(string sTeller, DateTime dtDate, string sSwitch, string nRemitNo)
        {
            int iORNo = 0;
            OracleResultSet rs = new OracleResultSet();
            rs.Query = "select sum(due), surcharge, form_type, or_no from payment ";
            rs.Query += "where teller = '" + sTeller.Trim() + "' and date_paid = to_date('" + dtDate.ToShortDateString() + "', 'MM/dd/yyyy') ";
            rs.Query += "and trn_type != 'POS' ";
            if (sSwitch == "New")
                rs.Query += "and or_no not in (select or_no from rcd_remit) ";
            else
                rs.Query += "and or_no in (select or_no from rcd_remit where rcd_series = '" + nRemitNo + "' and teller = '" + sTeller.Trim() + "') ";
            rs.Query += "GROUP BY surcharge, form_type, or_no ";
            rs.Query += "order by form_type asc ";
            if (rs.Execute())
            {
                while (rs.Read())
                {
                    OracleResultSet result = new OracleResultSet();
                    m_blnHasRecord = true;
                    sFormType.Add(rs.GetString(2));
                    iORNo = int.Parse(rs.GetString(3));
                    if (rs.GetString(2).Trim() != "")
                    {
                        if (rs.GetString(2).Trim() == "FORM 51")
                            dAmount.Add(rs.GetDouble(0) + rs.GetDouble(1));
                        else
                            dAmount.Add(rs.GetDouble(0));
                    }

                    result.Query = "select distinct from_or_no, to_or_no from or_inv where form_type = '" + rs.GetString("form_type") + "'";
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            int iFr = int.Parse(result.GetString(0));
                            int iTo = int.Parse(result.GetString(1));

                            if (iORNo >= iFr && iORNo <= iTo)
                            {
                                nORNo.Add(int.Parse(result.GetString(1)) - int.Parse(result.GetString(0)) + 1);
                                sORFr.Add(result.GetString(0));
                                sORTo.Add(result.GetString(1));
                            }
                        }
                    }
                }
            }
            rs.Close();

            //ADD COLLECTION OF CASH TICKET
            rs.Query = "SELECT * FROM DCT_USED WHERE COLLECTOR = '" + sTeller.Trim() + "' ";
            rs.Query += "AND TRN_DATE = to_date('" + dtDate.ToShortDateString() + "','MM/dd/yyyy')";
            try
            {
                if (rs.Execute())
                {
                    while (rs.Read())
                    {
                        sFormType.Add("Cash Ticket");
                        nORNo.Add(rs.GetInt("NO_PCS"));
                        sORFr.Add(rs.GetString("SERIES_FR"));
                        sORTo.Add(rs.GetString("SERIES_TO"));
                        dAmount.Add(rs.GetDouble("AMOUNT") * rs.GetInt("NO_PCS"));
                    }
                }
                rs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadFeeType/Cashticket", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override List<string> FormType()
        {
            //throw new NotImplementedException();
            return sFormType;
        }

        public override List<int> ORNo()
        {
            //throw new NotImplementedException();
            return nORNo;
        }

        public override List<string> ORFrom()
        {
            //throw new NotImplementedException();
            return sORFr;
        }

        public override List<string> ORTo()
        {
            //throw new NotImplementedException();
            return sORTo;
        }

        public override List<double> Amount()
        {
            //throw new NotImplementedException();
            return dAmount;
        }

    }

    abstract class TaxCredit
    {
        public abstract double Credit();
        public abstract double Debit();
    }

    class LoadTaxCredit : TaxCredit
    {
        double m_dCredit = 0;
        double m_dDebit = 0;

        public LoadTaxCredit(string sTeller, DateTime dtDate, List<string> sORFr, List<string> sORTo, List<string> sFormType)
        {
            string sFR = "";
            string sTo = "";
            for (int i = 0; i != sORFr.Count; i++)
            {
                if (sFR == sORFr[i].ToString() && sTo == sORTo[i].ToString())
                {
                    OracleResultSet result = new OracleResultSet();
                    result.Query = "SELECT SUM(CREDIT), SUM(DEBIT) FROM CREDITS ";
                    result.Query += "WHERE OR_NO IN (SELECT OR_NO FROM PAYMENT WHERE DATE_PAID = to_date('" + dtDate.ToShortDateString() + "','MM/dd/yyyy') ";
                    result.Query += "AND TELLER = '" + sTeller.Trim() + "' ";
                    result.Query += "AND OR_NO BETWEEN '" + sORFr[i] + "' AND '" + sORTo[i] + "'";
                    result.Query += "AND FORM_TYPE = '" + sFormType[i] + "')";
                    try
                    {
                        if (result.Execute())
                        {
                            while (result.Read())
                            {
                                m_dCredit += result.GetDouble(0);
                                m_dDebit += result.GetDouble(1);
                            }
                        }
                        result.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadTaxCredit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sFR = sORFr[i].ToString();
                    sTo = sORTo[i].ToString();
                }
            }
        }

        public override double Credit()
        {
            //throw new NotImplementedException();
            return m_dCredit;
        }

        public override double Debit()
        {
            //throw new NotImplementedException();
            return m_dDebit;
        }
    }

    abstract class CollectionSummary
    {
        public abstract List<int> CheckNo();
        public abstract List<string> Payee();
        public abstract List<double> Amount();
    }

    class LoadSummary : CollectionSummary
    {
        List<int> nCheckNo = new List<int>();
        List<string> sPayee = new List<string>();
        List<double> dAmount = new List<double>();

        public LoadSummary(string sTeller, DateTime dtDateTrans, List<string> sORFr, List<string> sORTo)
        {
            for (int i = 0; i != sORFr.Count; i++)
            {
                OracleResultSet result = new OracleResultSet();
                result.Query = "SELECT A.CHECK_NO, A.CHECK_AMOUNT, B.ACCT_FN, B.ACCT_MI, B.ACCT_LN FROM CHECK_TBL A, ACCOUNT B ";
                result.Query += "WHERE A.TELLER = '" + sTeller.Trim() + "' ";
                result.Query += "AND A.CHECK_DATE_ISSUED = to_date('" + dtDateTrans.ToShortDateString() + "','MM/dd/yyyy') ";
                result.Query += "AND A.ACCT_CODE = B.ACCT_CODE ";
                result.Query += "AND A.OR_NO BETWEEN '" + sORFr[i] + "' AND '" + sORTo[i] + "' ";
                try
                {
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            nCheckNo.Add(int.Parse(result.GetString(0).Trim()));
                            sPayee.Add(result.GetString(2).Trim() + " " + result.GetString(4).Trim());
                            dAmount.Add(result.GetDouble(1));
                        }
                    }
                    result.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err CollectionSummary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public override List<int> CheckNo()
        {
            //throw new NotImplementedException();
            return nCheckNo;
        }

        public override List<string> Payee()
        {
            //throw new NotImplementedException();
            return sPayee;
        }

        public override List<double> Amount()
        {
            //throw new NotImplementedException();
            return dAmount;
        }
    }
}
