using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amellar.Common.DataConnector;
using System.Windows.Forms;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.RCD
{
    abstract class Payor
    {
        public abstract List<string> ORSeries();
        public abstract List<string> Name();
        public abstract List<string> FeeType();
        public abstract List<double> Amount();
    }

    class LoadPayor : Payor
    {
        List<string> sORSeries = new List<string>();
        List<string> sName = new List<string>();
        List<string> sFeeType = new List<string>();
        List<double> dAmount = new List<double>();


        public LoadPayor(string sTeller, DateTime dtDate, string sSwitch, string sRemitNo)
        {

            OracleResultSet rs = new OracleResultSet();
            rs.Query = "select sum(a.due), a.surcharge, a.form_type, a.or_no, a.acct_code, b.fees_desc from payment a, major_fees b ";
            rs.Query += "where a.teller = '" + sTeller.Trim() + "' and a.date_paid = to_date('" + dtDate.ToShortDateString() + "', 'MM/dd/yyyy') ";
            rs.Query += "and a.trn_type != 'POS' ";
            rs.Query += "and b.fees_code = substr(a.fee_type, 1, 2) ";
            if (sSwitch == "New")
                rs.Query += "and a.or_no not in (select or_no from rcd_remit) ";
            else
                rs.Query += "and a.or_no in (select or_no from rcd_remit where rcd_series = '" + sRemitNo + "' and teller = '" + sTeller.Trim() + "') ";
            rs.Query += "group by a.surcharge, a.form_type, a.or_no, a.acct_code, b.fees_desc ";
            rs.Query += "order by a.form_type asc ";

            //rs.Query = "SELECT A.OR_NO, B.ACCT_LN, B.ACCT_FN, B.ACCT_MI, C.FEES_DESC, SUM(A.DUE)+SUM(A.SURCHARGE) FROM PAYMENT A, ACCOUNT B, MAJOR_FEES C ";
            //rs.Query += "WHERE A.TELLER = '" + sTeller.Trim() + "' ";
            //rs.Query += "AND A.DATE_PAID = to_date('" + dtDate.ToShortDateString() + "','MM/dd/yyyy') ";
            //rs.Query += "AND A.OR_NO BETWEEN '" + result.GetString(0) + "' AND '" + result.GetString(1) + "' ";
            //rs.Query += "AND A.TRN_TYPE != 'POS'  AND A.OR_NO NOT IN (SELECT OR_NO FROM RCD_REMIT) ";
            //rs.Query += "AND B.ACCT_CODE = A.ACCT_CODE ";
            //rs.Query += "AND C.FEES_CODE = SUBSTR(A.FEE_TYPE, 1, 2) ";
            //rs.Query += "GROUP BY A.OR_NO, B.ACCT_LN, B.ACCT_FN, B.ACCT_MI, C.FEES_DESC ";
            //rs.Query += "ORDER BY A.OR_NO, C.FEES_DESC ASC";
            if (rs.Execute())
            {
                while (rs.Read())
                {
                    string sPayor = "";//AppSettingsManager.get("AcctName", rs.GetString(4));
                    //if (rs.GetString(3).Trim().Length != 0)
                    //    sPayor += " " + rs.GetString(3) + ".";

                    sORSeries.Add(rs.GetString(3));
                    sName.Add(sPayor);
                    sFeeType.Add(rs.GetString(5));
                    
                    if (rs.GetString(2) == "FORM 51")
                        dAmount.Add(rs.GetDouble(0) + rs.GetDouble(1));
                    else
                        dAmount.Add(rs.GetDouble(0));
                }
            }
            rs.Close();
        }

        public override List<string> ORSeries()
        {
            //throw new NotImplementedException();
            return sORSeries;
        }

        public override List<string> Name()
        {
            //throw new NotImplementedException();
            return sName;
        }

        public override List<string> FeeType()
        {
            //throw new NotImplementedException();
            return sFeeType;
        }

        public override List<double> Amount()
        {
            //throw new NotImplementedException();
            return dAmount;
        }
    }
}
