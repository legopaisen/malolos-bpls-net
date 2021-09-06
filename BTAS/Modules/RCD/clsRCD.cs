using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amellar.Common.DataConnector;
using System.Windows.Forms;

namespace Amellar.Modules.RCD
{
    class Common
    {
        
    }

    #region A.Collections
    //abstract class A
    //{
    //    public abstract List<string> FormType();
    //    public abstract List<int> ORNo();
    //    public abstract List<string> ORFrom();
    //    public abstract List<string> ORTo();
    //    public abstract List<double> Amount();
    //}

    //class Collections : A
    //{
    //    List<string> sFormType = new List<string>();
    //    List<int> nORNo = new List<int>();
    //    List<string> sORFr = new List<string>();
    //    List<string> sORTo = new List<string>();
    //    List<double> dAmount = new List<double>();

    //    public Collections(string sTeller, DateTime dtDate, string sSwitch, int nRemitNo)
    //    {
    //        OracleResultSet result = new OracleResultSet();
    //        result.Query = "SELECT distinct FROM_OR_NO, TO_OR_NO FROM OR_INV";
    //        try
    //        {
    //            if (result.Execute())
    //            {
    //                while (result.Read())
    //                {
    //                    OracleResultSet rs = new OracleResultSet();
    //                    rs.Query = "SELECT SUM(A.DUE), A.SURCHARGE, A.FORM_TYPE, B.FEES_DESC FROM PAYMENT A, MAJOR_FEES B ";
    //                    rs.Query += "WHERE A.TELLER = '" + sTeller.Trim() + "' ";
    //                    rs.Query += "AND A.DATE_PAID = to_date('" + dtDate.ToShortDateString() + "', 'MM/dd/yyyy') ";
    //                    rs.Query += "AND A.OR_NO BETWEEN '" + result.GetString(0) + "' AND '" + result.GetString(1) + "' ";
    //                    rs.Query += "AND A.TRN_TYPE != 'POS' ";
    //                    if (sSwitch == "New")
    //                        rs.Query += "AND A.OR_NO NOT IN (SELECT OR_NO FROM RCD_REMIT) ";
    //                    else
    //                        rs.Query += "AND A.OR_NO IN (SELECT OR_NO FROM RCD_REMIT WHERE RCD_SERIES = '" + nRemitNo + "' AND TELLER = '" + sTeller.Trim() + "' and or_no between '" + result.GetString(0) + "' and '" + result.GetString(1) + "') ";
    //                    rs.Query += "AND B.FEES_CODE = SUBSTR(A.FEE_TYPE, 1, 2) ";
    //                    rs.Query += "GROUP BY A.SURCHARGE, A.FORM_TYPE, B.FEES_DESC";
    //                    if (rs.Execute())
    //                    {
    //                        while (rs.Read())
    //                        {
    //                            // AST 20131004 Modify formtype depends on feesdesc(s)
    //                            //if (rs.GetString(3).Trim().Contains("STALL RENTAL") || rs.GetString(3).Trim().Contains("VACATED STALL")
    //                                //|| rs.GetString(3).Trim().Contains("PRIOR YEARS DELINQUENCY"))
    //                                sFormType.Add(rs.GetString(2));
    //                            //else
    //                                //sFormType.Add(rs.GetString(3));
    //                            nORNo.Add(int.Parse(result.GetString(1)) - int.Parse(result.GetString(0)) + 1);
    //                            sORFr.Add(result.GetString(0));
    //                            sORTo.Add(result.GetString(1));
    //                            dAmount.Add(rs.GetDouble(0) + rs.GetDouble(1));
    //                        }
    //                    }
    //                    rs.Close();

    //                }
    //            }
    //            result.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadFeeType", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        }

    //        //ADD COLLECTION OF CASH TICKET
    //        result.Query = "SELECT * FROM DCT_USED WHERE COLLECTOR = '" + sTeller.Trim() + "' ";
    //        result.Query += "AND TRN_DATE = to_date('" + dtDate.ToShortDateString() + "','MM/dd/yyyy')";
    //        try
    //        {
    //            if (result.Execute())
    //            {
    //                while (result.Read())
    //                {
    //                    sFormType.Add("Cash Ticket");
    //                    nORNo.Add(result.GetInt("NO_PCS"));
    //                    sORFr.Add(result.GetString("SERIES_FR"));
    //                    sORTo.Add(result.GetString("SERIES_TO"));
    //                    dAmount.Add(result.GetDouble("AMOUNT") * result.GetInt("NO_PCS"));
    //                }
    //            }
    //            result.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadFeeType/Cashticket", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        }
    //    }

    //    public override List<string> FormType()
    //    {
    //        //throw new NotImplementedException();
    //        return sFormType;
    //    }

    //    public override List<int> ORNo()
    //    {
    //        //throw new NotImplementedException();
    //        return nORNo;
    //    }

    //    public override List<string> ORFrom()
    //    {
    //        //throw new NotImplementedException();
    //        return sORFr;
    //    }

    //    public override List<string> ORTo()
    //    {
    //        //throw new NotImplementedException();
    //        return sORTo;
    //    }
       
    //    public override List<double> Amount()
    //    {
    //        //throw new NotImplementedException();
    //        return dAmount;
    //    }

    //}

    //abstract class TaxCredit
    //{
    //    public abstract double Credit();
    //    public abstract double Debit();
    //}

    //class LoadTaxCredit : TaxCredit
    //{
    //    double m_dCredit = 0;
    //    double m_dDebit = 0;

    //    public LoadTaxCredit(string sTeller, DateTime dtDate, List<string> sORFr, List<string> sORTo)
    //    {
    //        string sFR = "";
    //        string sTo = "";
    //        for (int i = 0; i != sORFr.Count; i++)
    //        {
    //            if (sFR == sORFr[i].ToString() && sTo == sORTo[i].ToString())
    //            {
    //                OracleResultSet result = new OracleResultSet();
    //                result.Query = "SELECT SUM(CREDIT), SUM(DEBIT) FROM CREDITS ";
    //                result.Query += "WHERE OR_NO IN (SELECT OR_NO FROM PAYMENT WHERE DATE_PAID = to_date('" + dtDate.ToShortDateString() + "','MM/dd/yyyy') ";
    //                result.Query += "AND TELLER = '" + sTeller.Trim() + "' ";
    //                result.Query += "AND OR_NO BETWEEN '" + sORFr[i] + "' AND '" + sORTo[i] + "') ";
    //                try
    //                {
    //                    if (result.Execute())
    //                    {
    //                        while (result.Read())
    //                        {
    //                            m_dCredit += result.GetDouble(0);
    //                            m_dDebit += result.GetDouble(1);
    //                        }
    //                    }
    //                    result.Close();
    //                }
    //                catch (Exception ex)
    //                {
    //                    MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadTaxCredit", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //                }
    //                sFR = sORFr[i].ToString();
    //                sTo = sORTo[i].ToString();
    //            }
    //        }
    //    }

    //    public override double Credit()
    //    {
    //        //throw new NotImplementedException();
    //        return m_dCredit;
    //    }

    //    public override double Debit()
    //    {
    //        //throw new NotImplementedException();
    //        return m_dDebit;
    //    }
    //}

    #endregion

    #region B.Remittance/Deposits
    //class Remittance
    //{
    //    public static string GetTellerName(string sTellerCode)
    //    {
    //        OracleResultSet result = new OracleResultSet();
    //        result.Query = "SELECT TELLER_FN, TELLER_MI, TELLER_LN FROM TELLERS WHERE TELLER_CODE = '" + sTellerCode.Trim() + "'";
    //        try
    //        {
    //            if (result.Execute())
    //            {
    //                while (result.Read())
    //                {
    //                    string sMI = result.GetString(1);
    //                    if (sMI.Trim() != string.Empty)
    //                        sMI = sMI.Insert(1, ".");
    //                    return result.GetString(0).Trim() + " " + sMI + " " + result.GetString(2).Trim();
    //                }
    //            }
    //            result.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err GetTellerName", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        }
    //        return "";
    //    }

    //    public static string GetBankName(string sTeller, DateTime dtDatePaid, List<string> sORFr, List<string> sORTo)
    //    {
    //        string sBankName = "";
    //        for (int i = 0; i != sORFr.Count; i++)
    //        {
    //            OracleResultSet result = new OracleResultSet();
    //            result.Query = "SELECT SUM(A.AMOUNT), C.BANK_NM FROM CASH_CHECK_PAYMENT A, CHECK_TBL B, BANK_TABLE C ";
    //            result.Query += "WHERE A.TELLER = '" + sTeller.Trim() + "' ";
    //            result.Query += "AND A.DATE_PAID = to_date('" + dtDatePaid.ToShortDateString() + "','MM/dd/yyyy') ";
    //            result.Query += "AND A.FEE_TYPE = 'CK'";
    //            result.Query += "AND A.OR_NO between '" + sORFr[i] + "' AND '" + sORTo[i] + "' ";
    //            result.Query += "AND B.OR_NO = A.OR_NO ";
    //            result.Query += "AND B.TELLER = A.TELLER ";
    //            result.Query += "AND B.DATE_ISSUED = A.DATE_PAID ";
    //            result.Query += "GROUP BY C.BANK_NM";
    //            try
    //            {
    //                if (result.Execute())
    //                {
    //                    while (result.Read())
    //                    {
    //                        sBankName += result.GetString(1).Trim() + ", ";
    //                    }
    //                    result.Close();
    //                }
    //                if (sBankName.Trim().Length != 0)
    //                    sBankName = sBankName.Remove(sBankName.Length - 2, 2); // remove the comma
    //            }
    //            catch (Exception ex)
    //            {
    //                MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err GetBankName", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            }
    //        }
    //        return sBankName;
    //    }

    //    public static double LoadCash(string sTeller, DateTime dtDatePaid, List<string> sORFr, List<string> sORTo)
    //    {
    //        double dCash = 0;
    //        string sFr = "";
    //        string sTo = "";
    //        for (int i = 0; i != sORFr.Count; i++)
    //        {
    //            if (sFr != sORFr[i].ToString() && sTo != sORTo[i].ToString())
    //            {
    //                OracleResultSet rSet = new OracleResultSet();
    //                rSet.Query = "SELECT SUM(AMOUNT) FROM CASH_CHECK_PAYMENT ";
    //                rSet.Query += "WHERE TELLER = '" + sTeller.Trim() + "' ";
    //                rSet.Query += "AND DATE_PAID = to_date('" + dtDatePaid.ToShortDateString() + "','MM/dd/yyyy') ";
    //                rSet.Query += "AND FEE_TYPE = 'CS' ";
    //                rSet.Query += "AND OR_NO BETWEEN '" + sORFr[i] + "' AND '" + sORTo[i] + "'";
    //                try
    //                {
    //                    if (rSet.Execute())
    //                    {
    //                        while (rSet.Read())
    //                        {
    //                            dCash += rSet.GetDouble(0);
    //                        }
    //                        rSet.Close();
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadCash", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //                }
    //                sFr = sORFr[i].ToString();
    //                sTo = sORTo[i].ToString();
    //            }
    //        }
    //        return dCash;
    //    }

    //    public static double LoadCheck(string sTeller, DateTime dtDatePaid, List<string> sORFr, List<string> sORTo)
    //    {
    //        double dCheck = 0;
    //        string sFr = "";
    //        string sTo = "";
    //        for (int i = 0; i != sORFr.Count; i++)
    //        {
    //            if (sFr != sORFr[i].ToString() && sTo != sORTo[i].ToString())
    //            {
    //                OracleResultSet result = new OracleResultSet();
    //                result.Query = "SELECT SUM(A.AMOUNT), C.BANK_NM FROM CASH_CHECK_PAYMENT A, CHECK_TBL B, BANK_TABLE C ";
    //                result.Query += "WHERE A.TELLER = '" + sTeller.Trim() + "' ";
    //                result.Query += "AND A.DATE_PAID = to_date('" + dtDatePaid.ToShortDateString() + "','MM/dd/yyyy') ";
    //                result.Query += "AND A.FEE_TYPE = 'CK'";
    //                result.Query += "AND A.OR_NO between '" + sORFr[i] + "' AND '" + sORTo[i] + "' ";
    //                result.Query += "AND B.OR_NO = A.OR_NO ";
    //                result.Query += "AND B.TELLER = A.TELLER ";
    //                result.Query += "AND B.DATE_ISSUED = A.DATE_PAID ";
    //                result.Query += "GROUP BY C.BANK_NM";
    //                try
    //                {
    //                    if (result.Execute())
    //                    {
    //                        while (result.Read())
    //                        {
    //                            dCheck += result.GetDouble(0);
    //                        }
    //                        result.Close();
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //                }
    //                sFr = sORFr[i].ToString();
    //                sTo = sORTo[i].ToString();
    //            }
    //        }
    //        return dCheck;
    //    }
    //}
    #endregion

    #region C.Accountability for Accountable form
    //abstract class C
    //{
    //    public abstract List<int> BeginningQty();
    //    public abstract List<string> BeginningORFrom();
    //    public abstract List<string> BeginningORTo();

    //    public abstract List<int> IssuedQty();
    //    public abstract List<string> IssuedORFrom();
    //    public abstract List<string> IssuedORTo();

    //    public abstract List<int> EndingQty();
    //    public abstract List<string> EndingORFrom();
    //    public abstract List<string> EndingORTo();
    //}

    //class Accountability : C
    //{
    //    string m_sTeller = string.Empty;
    //    DateTime m_dtDateTrans = new DateTime();
    //    List<string> m_sFormType = new List<string>();
    //    List<string> m_sORFrom = new List<string>();
    //    List<string> m_sORTo = new List<string>();

    //    //Collection
    //    List<string> sBeginningORFrom = new List<string>();
    //    List<string> sBeginningORTo = new List<string>();
    //    List<int> iBeginningQty = new List<int>();

    //    List<string> sIssuedORFrom = new List<string>();
    //    List<string> sIssuedORTo = new List<string>();
    //    List<int> nIssuedQty = new List<int>();

    //    List<string> sEndingORFrom = new List<string>();
    //    List<string> sEndingORTo = new List<string>();
    //    List<int> iEndingQty = new List<int>();

    //    string sType = string.Empty;
    //    public string AccountabilityType
    //    {
    //        get { return sType; }
    //        set { sType = value; }
    //    }

    //    public Accountability(string sTeller, DateTime dtDateTrans, List<string> sFormType, List<string> sORFr, List<string> sORTo)
    //    {
    //        m_sTeller = sTeller;
    //        m_dtDateTrans = dtDateTrans;
    //        m_sFormType = sFormType;
    //        m_sORFrom = sORFr;
    //        m_sORTo = sORTo;
    //    }

    //    public void GetAccountability()
    //    {
    //        for (int i = 0; i != m_sORFrom.Count; i++)
    //        {
    //            OracleResultSet result = new OracleResultSet();
    //            result.Query = "SELECT MIN(B.OR_NO), MIN(A.TO_OR_NO), MAX(B.OR_NO) FROM OR_RETURNED A, OR_USED B ";
    //            result.Query += "WHERE A.TELLER_CODE = '" + m_sTeller.Trim() + "' ";
    //            result.Query += "AND A.TRN_DATE = to_date('" + m_dtDateTrans.ToShortDateString() + "','MM/dd/yyyy') ";
    //            result.Query += "AND A.FROM_OR_NO BETWEEN '" + m_sORFrom[i] + "' AND '" + m_sORTo[i] + "' ";
    //            result.Query += "AND B.TELLER = '" + m_sTeller.Trim() + "' ";
    //            result.Query += "AND B.TRN_DATE = to_date('" + m_dtDateTrans.ToShortDateString() + "','MM/dd/yyyy') ";
    //            result.Query += "AND B.OR_NO BETWEEN '" + m_sORFrom[i] + "' AND '" + m_sORTo[i] + "' ";
    //            try
    //            {
    //                if (m_sFormType[i].ToString().Trim() != "")
    //                    result.Query += "and b.form_type = '" + m_sFormType[i].ToString() + "' ";
    //            }
    //            catch { }
    //            result.Query += "ORDER BY A.FROM_OR_NO ASC";
    //            try
    //            {
    //                if (result.Execute())
    //                {
    //                    while (result.Read())
    //                    {
    //                        sBeginningORFrom.Add(result.GetString(0).Trim());
    //                        sBeginningORTo.Add(result.GetString(1).Trim());
    //                        iBeginningQty.Add(int.Parse(result.GetString(1).Trim()) - int.Parse(result.GetString(0).Trim()) + 1);

    //                        sIssuedORFrom.Add(result.GetString(0).Trim());
    //                        sIssuedORTo.Add(result.GetString(2).Trim());
    //                        nIssuedQty.Add(int.Parse(result.GetString(2).Trim()) - int.Parse(result.GetString(0).Trim()) + 1);

    //                        sEndingORFrom.Add((int.Parse(result.GetString(2).Trim()) + 1).ToString("0000000"));
    //                        sEndingORTo.Add(result.GetString(1).Trim());
    //                        iEndingQty.Add(int.Parse(result.GetString(1).Trim()) - int.Parse((int.Parse(result.GetString(2).Trim()) + 1).ToString("0000000")) + 1);
    //                    }
    //                }
    //                result.Close();
    //            }
    //            catch (Exception ex)
    //            {
    //                MessageBox.Show(ex.Message + "\nPlease contact your software support team",
    //                    "Err GetAccountability", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            }

    //            //ADD COLLECTION OF CASH TICKET
    //            result.Query = "SELECT * FROM DCT_USED WHERE COLLECTOR = '" + m_sTeller.Trim() + "' ";
    //            result.Query += "AND TRN_DATE = to_date('" + m_dtDateTrans.ToShortDateString() + "','MM/dd/yyyy')";
    //            try
    //            {
    //                if (result.Execute())
    //                {
    //                    while (result.Read())
    //                    {
    //                        OracleResultSet rs = new OracleResultSet();
    //                        rs.Query = "SELECT * FROM DCT_INV WHERE AMOUNTPC = '" + result.GetFloat("AMOUNT") + "'";
    //                        if (rs.Execute())
    //                        {
    //                            while (rs.Read())
    //                            {
    //                                sBeginningORFrom.Add(rs.GetString("SERIES_FR"));
    //                                sBeginningORTo.Add(rs.GetString("SERIES_TO"));
    //                                iBeginningQty.Add(rs.GetInt("NOOFPCS"));

    //                                sBeginningORFrom.Add(result.GetString("SERIES_FR"));
    //                                sBeginningORTo.Add(result.GetString("SERIES_TO"));
    //                                iBeginningQty.Add(result.GetInt("NO_PCS"));

    //                                sBeginningORFrom.Add(int.Parse(result.GetString("SERIES_TO") + 1).ToString("0000000"));
    //                                sBeginningORTo.Add(rs.GetString("SERIES_TO"));
    //                                iBeginningQty.Add(int.Parse(rs.GetString("SERIES_TO")) - int.Parse(result.GetString("SERIES_TO") + 1));
    //                            }
    //                        }
    //                        rs.Close();
    //                    }
    //                }
    //                result.Close();
    //            }
    //            catch (Exception ex)
    //            {
    //                MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadFeeType/Cashticket", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            }
    //        }
    //    }

    //    public override List<int> BeginningQty()
    //    {
    //        return iBeginningQty;
    //    }

    //    public override List<string> BeginningORFrom()
    //    {
    //        return sBeginningORFrom;
    //    }

    //    public override List<string> BeginningORTo()
    //    {
    //        return sBeginningORTo;
    //    }

    //    public override List<int> IssuedQty()
    //    {
    //        return nIssuedQty;
    //    }

    //    public override List<string> IssuedORFrom()
    //    {
    //        return sIssuedORFrom;
    //    }

    //    public override List<string> IssuedORTo()
    //    {
    //        return sIssuedORTo;
    //    }

    //    public override List<int> EndingQty()
    //    {
    //        return iEndingQty;
    //    }

    //    public override List<string> EndingORFrom()
    //    {
    //        return sEndingORFrom;
    //    }

    //    public override List<string> EndingORTo()
    //    {
    //        return sEndingORTo;
    //    }
    //}
    #endregion

    #region D.Summary of Remittance, Collection, and Deposits
    //abstract class D
    //{
    //    public abstract List<int> CheckNo();
    //    public abstract List<string> Payee();
    //    public abstract List<double> Amount();
    //}

    //class CollectionSummary : D
    //{
    //    List<int> nCheckNo = new List<int>();
    //    List<string> sPayee = new List<string>();
    //    List<double> dAmount = new List<double>();

    //    public CollectionSummary(string sTeller, DateTime dtDateTrans, List<string> sORFr, List<string> sORTo)
    //    {
    //        for (int i = 0; i != sORFr.Count; i++)
    //        {
    //            OracleResultSet result = new OracleResultSet();
    //            result.Query = "SELECT A.CHECK_NO, A.CHECK_AMOUNT, B.ACCT_FN, B.ACCT_MI, B.ACCT_LN FROM CHECK_TBL A, ACCOUNT B ";
    //            result.Query += "WHERE A.TELLER = '" + sTeller.Trim() + "' ";
    //            result.Query += "AND A.CHECK_DATE_ISSUED = to_date('" + dtDateTrans.ToShortDateString() + "','MM/dd/yyyy') ";
    //            result.Query += "AND A.ACCT_CODE = B.ACCT_CODE ";
    //            result.Query += "AND A.OR_NO BETWEEN '" + sORFr[i] + "' AND '" + sORTo[i] + "' ";
    //            try
    //            {
    //                if (result.Execute())
    //                {
    //                    while (result.Read())
    //                    {
    //                        nCheckNo.Add(int.Parse(result.GetString(0).Trim()));
    //                        sPayee.Add(result.GetString(2).Trim() + " " + result.GetString(4).Trim());
    //                        dAmount.Add(result.GetDouble(1));
    //                    }
    //                }
    //                result.Close();
    //            }
    //            catch (Exception ex)
    //            {
    //                MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err CollectionSummary", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            }
    //        }
    //    }
    //    public override List<int> CheckNo()
    //    {
    //        //throw new NotImplementedException();
    //        return nCheckNo;
    //    }

    //    public override List<string> Payee()
    //    {
    //        //throw new NotImplementedException();
    //        return sPayee;
    //    }

    //    public override List<double> Amount()
    //    {
    //        //throw new NotImplementedException();
    //        return dAmount;
    //    }
    //}
    #endregion

    #region LoadCollection
    //abstract class ORInventory
    //{
    //    //public abstract string FormType();
    //    //public abstract string ORFrom();
    //    //public abstract string ORTo();
    //    //public abstract int ORNo();
    //    //public abstract double TotalCollection();
    //    public abstract List<string> FormType();
    //    public abstract List<string> ORFrom();
    //    public abstract List<string> ORTo();
    //    public abstract List<int> ORNo();
    //    public abstract List<double> TotalCollection();
    //}

    //class LoadCollection : ORInventory
    //{
    //    List<string> m_sFormType = new List<string>();
    //    List<string> m_sORFr =new List<string>();
    //    List<string> m_sORTo = new List<string>();
    //    List<int> m_nORNo = new List<int>();
    //    List<double> m_dTotalCollection = new List<double>();

    //    bool m_blnHasRecord;
    //    public bool HasRecord
    //    {
    //        get { return m_blnHasRecord; }
    //    }

    //    public LoadCollection(string sTeller, DateTime dtDate, string sSwitch, int nRemitNo)
    //    {
    //        OracleResultSet result = new OracleResultSet();
    //        result.Query = "SELECT DISTINCT FROM_OR_NO, TO_OR_NO FROM OR_INV";
    //        if (result.Execute())
    //        {
    //            while (result.Read())
    //            {
    //                OracleResultSet rs = new OracleResultSet();
    //                rs.Query = "SELECT FORM_TYPE, SUM(DUE)+SUM(SURCHARGE) FROM PAYMENT ";
    //                rs.Query += "WHERE TELLER = '" + sTeller.Trim() + "' ";
    //                rs.Query += "AND DATE_PAID = to_date('" + dtDate.ToShortDateString() + "', 'MM/dd/yyyy') ";
    //                rs.Query += "AND OR_NO BETWEEN " + result.GetString(0) + " AND " + result.GetString(1) + " ";
    //                rs.Query += "AND TRN_TYPE != 'POS' ";
    //                if (sSwitch == "New")
    //                    rs.Query += "AND OR_NO NOT IN (SELECT OR_NO FROM RCD_REMIT) ";
    //                else //for reprint
    //                    rs.Query += "AND OR_NO IN (SELECT OR_NO FROM RCD_REMIT WHERE RCD_SERIES = '" + nRemitNo + "' AND TELLER = '" + sTeller.Trim() + "' and or_no between '" + result.GetString(0) + "' and '" + result.GetString(1) + "') ";
    //                rs.Query += "GROUP BY FORM_TYPE order by form_type asc";
    //                try
    //                {
    //                    if (rs.Execute())
    //                    {
    //                        while (rs.Read())
    //                        {
    //                            string strTmpFormType = rs.GetString(0).Trim();//tmp
    //                            m_blnHasRecord = true;
    //                            m_sFormType.Add(rs.GetString(0).Trim());
    //                            m_sORFr.Add(result.GetString(0).Trim());
    //                            m_sORTo.Add(result.GetString(1).Trim());
    //                            m_nORNo.Add(int.Parse(result.GetString(1)) - int.Parse(result.GetString(0)) + 1);
    //                            m_dTotalCollection.Add(rs.GetDouble(1));
    //                        }
    //                    }
    //                    rs.Close();
    //                }
    //                catch (Exception ex)
    //                {
    //                    MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadCollection", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //                }
    //            }
    //        }
    //        result.Close();
    //    }

    //    public override List<string> FormType()
    //    {
    //        //throw new NotImplementedException();
    //        return m_sFormType;
    //    }

    //    public override List<string> ORFrom()
    //    {
    //        //throw new NotImplementedException();
    //        return m_sORFr;
    //    }

    //    public override List<string> ORTo()
    //    {
    //        //throw new NotImplementedException();
    //        return m_sORTo;
    //    }

    //    public override List<int> ORNo()
    //    {
    //        //throw new NotImplementedException();
    //        return m_nORNo;
    //    }

    //    public override List<double> TotalCollection()
    //    {
    //        //throw new NotImplementedException();
    //        return m_dTotalCollection;
    //    }
    //}
    #endregion

    #region Payor Info
    //abstract class Payor
    //{
    //    public abstract List<string> ORSeries();
    //    public abstract List<string> Name();
    //    public abstract List<string> FeeType();
    //    public abstract List<double> Amount();
    //}

    //class LoadPayor : Payor
    //{
    //    List<string> sORSeries = new List<string>();
    //    List<string> sName = new List<string>();
    //    List<string> sFeeType = new List<string>();
    //    List<double> nAmount = new List<double>();

    //    public LoadPayor(string sTeller, DateTime dtDate)
    //    {
    //        OracleResultSet result = new OracleResultSet();
    //        result.Query = "SELECT FROM_OR_NO, TO_OR_NO FROM OR_INV";
    //        try
    //        {
    //            if (result.Execute())
    //            {
    //                while (result.Read())
    //                {
    //                    OracleResultSet rs = new OracleResultSet();
    //                    rs.Query = "SELECT  A.OR_NO, B.ACCT_LN, B.ACCT_FN, B.ACCT_MI, C.FEES_DESC, SUM(A.DUE)+SUM(A.SURCHARGE) FROM PAYMENT A, ACCOUNT B, MAJOR_FEES C ";
    //                    rs.Query += "WHERE A.TELLER = '" + sTeller.Trim() + "' ";
    //                    rs.Query += "AND A.DATE_PAID = to_date('" + dtDate.ToShortDateString() + "','MM/dd/yyyy') ";
    //                    rs.Query += "AND A.OR_NO BETWEEN '" + result.GetString(0) + "' AND '" + result.GetString(1) + "' ";
    //                    rs.Query += "AND A.TRN_TYPE != 'POS'  AND A.OR_NO NOT IN (SELECT OR_NO FROM RCD_REMIT) ";
    //                    rs.Query += "AND B.ACCT_CODE = A.ACCT_CODE ";
    //                    rs.Query += "AND C.FEES_CODE = SUBSTR(A.FEE_TYPE, 1, 2) ";
    //                    rs.Query += "GROUP BY A.OR_NO, B.ACCT_LN, B.ACCT_FN, B.ACCT_MI, C.FEES_DESC ";
    //                    rs.Query += "ORDER BY A.OR_NO, C.FEES_DESC ASC";
    //                    if (rs.Execute())
    //                    {
    //                        while (rs.Read())
    //                        {
    //                            sORSeries.Add(rs.GetString(0));
    //                            string sPayor = rs.GetString(1) + " " + rs.GetString(2);
    //                            if (rs.GetString(3).Trim().Length != 0)
    //                                sPayor += " " + rs.GetString(3) + ".";
    //                            sName.Add(sPayor);
    //                            sFeeType.Add(rs.GetString(4));
    //                            nAmount.Add(rs.GetDouble(5));
    //                        }
    //                    }
    //                    rs.Close();
    //                }
    //            }
    //            result.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadPayor", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        }
    //    }

    //    public override List<string> ORSeries()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override List<string> Name()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override List<string> FeeType()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override List<double> Amount()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    #endregion


}
