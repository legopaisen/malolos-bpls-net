using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amellar.Common.DataConnector;
using System.Windows.Forms;

namespace Amellar.Modules.RCD
{
    class Remittance
    {
        public static string GetTellerName(string sTellerCode)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT FN, MI, LN FROM TELLERS WHERE TELLER = '" + sTellerCode.Trim() + "'";
            try
            {
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        string sMI = result.GetString(1);
                        if (sMI.Trim() != string.Empty)
                            sMI = sMI.Insert(1, ".");
                        return result.GetString(0).Trim() + " " + sMI + " " + result.GetString(2).Trim();
                    }
                }
                result.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err GetTellerName", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "";
        }

        public static string GetBankName(string sTeller, DateTime dtDatePaid, List<string> sORFr, List<string> sORTo, List<string> sFormType)
        {
            string sBankName = "";
            for (int i = 0; i != sORFr.Count; i++)
            {
                OracleResultSet result = new OracleResultSet();
                result.Query = "SELECT SUM(A.AMOUNT), C.BANK_NM FROM CASH_CHECK_PAYMENT A, CHECK_TBL B, BANK_TABLE C ";
                result.Query += "WHERE A.TELLER = '" + sTeller.Trim() + "' ";
                result.Query += "AND A.DATE_PAID = to_date('" + dtDatePaid.ToShortDateString() + "','MM/dd/yyyy') ";
                result.Query += "AND A.FEE_TYPE = 'CK'";
                result.Query += "AND A.OR_NO between '" + sORFr[i] + "' AND '" + sORTo[i] + "' ";
                result.Query += "AND A.FORM_TYPE = '" + sFormType[i] + "'";
                result.Query += "AND B.OR_NO = A.OR_NO ";
                result.Query += "AND B.TELLER = A.TELLER ";
                result.Query += "AND B.DATE_ISSUED = A.DATE_PAID ";
                result.Query += "GROUP BY C.BANK_NM";
                try
                {
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            sBankName += result.GetString(1).Trim() + ", ";
                        }
                        result.Close();
                    }
                    if (sBankName.Trim().Length != 0)
                        sBankName = sBankName.Remove(sBankName.Length - 2, 2); // remove the comma
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err GetBankName", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return sBankName;
        }

        public static double LoadCash(string sTeller, DateTime dtDatePaid, List<string> sORFr, List<string> sORTo, List<string> sFormType)
        {
            double dCash = 0;
            string sFr = "";
            string sTo = "";
            for (int i = 0; i != sORFr.Count; i++)
            {
                if (sFr != sORFr[i].ToString() && sTo != sORTo[i].ToString())
                {
                    OracleResultSet rSet = new OracleResultSet();
                    rSet.Query = "SELECT SUM(AMOUNT) FROM CASH_CHECK_PAYMENT ";
                    rSet.Query += "WHERE TELLER = '" + sTeller.Trim() + "' ";
                    rSet.Query += "AND DATE_PAID = to_date('" + dtDatePaid.ToShortDateString() + "','MM/dd/yyyy') ";
                    rSet.Query += "AND FEE_TYPE = 'CS' ";
                    rSet.Query += "AND OR_NO BETWEEN '" + sORFr[i] + "' AND '" + sORTo[i] + "' ";
                    rSet.Query += "and form_type = '" + sFormType[i] + "'";
                    try
                    {
                        if (rSet.Execute())
                        {
                            while (rSet.Read())
                            {
                                //tmp
                                double d = rSet.GetDouble(0);
                                dCash += rSet.GetDouble(0);
                            }
                            rSet.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadCash", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sFr = sORFr[i].ToString();
                    sTo = sORTo[i].ToString();
                }
            }
            return dCash;
        }

        public static double LoadCheck(string sTeller, DateTime dtDatePaid, List<string> sORFr, List<string> sORTo, List<string> sFormType)
        {
            double dCheck = 0;
            string sFr = "";
            string sTo = "";
            for (int i = 0; i != sORFr.Count; i++)
            {
                if (sFr != sORFr[i].ToString() && sTo != sORTo[i].ToString())
                {
                    OracleResultSet result = new OracleResultSet();
                    result.Query = "SELECT SUM(A.AMOUNT), C.BANK_NM FROM CASH_CHECK_PAYMENT A, CHECK_TBL B, BANK_TABLE C ";
                    result.Query += "WHERE A.TELLER = '" + sTeller.Trim() + "' ";
                    result.Query += "AND A.DATE_PAID = to_date('" + dtDatePaid.ToShortDateString() + "','MM/dd/yyyy') ";
                    result.Query += "AND A.FEE_TYPE = 'CK'";
                    result.Query += "AND A.OR_NO between '" + sORFr[i] + "' AND '" + sORTo[i] + "' ";
                    result.Query += "AND A.FORM_TYPE = '" + sFormType[i] + "'";
                    result.Query += "AND B.OR_NO = A.OR_NO ";
                    result.Query += "AND B.TELLER = A.TELLER ";
                    result.Query += "AND B.DATE_ISSUED = A.DATE_PAID ";
                    result.Query += "GROUP BY C.BANK_NM";
                    try
                    {
                        if (result.Execute())
                        {
                            while (result.Read())
                            {
                                dCheck += result.GetDouble(0);
                            }
                            result.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sFr = sORFr[i].ToString();
                    sTo = sORTo[i].ToString();
                }
            }
            return dCheck;
        }
    }
}
