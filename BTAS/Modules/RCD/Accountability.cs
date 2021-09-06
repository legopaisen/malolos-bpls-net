using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amellar.Common.DataConnector;
using System.Windows.Forms;

namespace Amellar.Modules.RCD
{
    abstract class Accountability
    {
        public abstract List<int> BeginningQty();
        public abstract List<string> BeginningORFrom();
        public abstract List<string> BeginningORTo();

        public abstract List<int> IssuedQty();
        public abstract List<string> IssuedORFrom();
        public abstract List<string> IssuedORTo();

        public abstract List<int> EndingQty();
        public abstract List<string> EndingORFrom();
        public abstract List<string> EndingORTo();
    }

    class LoadAccountability : Accountability
    {
        string m_sTeller = string.Empty;
        DateTime m_dtDateTrans = new DateTime();
        List<string> m_sFormType = new List<string>();
        List<string> m_sORFrom = new List<string>();
        List<string> m_sORTo = new List<string>();

        //Collection
        List<string> sBeginningORFrom = new List<string>();
        List<string> sBeginningORTo = new List<string>();
        List<int> iBeginningQty = new List<int>();

        List<string> sIssuedORFrom = new List<string>();
        List<string> sIssuedORTo = new List<string>();
        List<int> nIssuedQty = new List<int>();

        List<string> sEndingORFrom = new List<string>();
        List<string> sEndingORTo = new List<string>();
        List<int> iEndingQty = new List<int>();

        string sType = string.Empty;
        public string AccountabilityType
        {
            get { return sType; }
            set { sType = value; }
        }

        public LoadAccountability(string sTeller, DateTime dtDateTrans, List<string> sFormType, List<string> sORFr, List<string> sORTo)
        {
            m_sTeller = sTeller;
            m_dtDateTrans = dtDateTrans;
            m_sFormType = sFormType;
            m_sORFrom = sORFr;
            m_sORTo = sORTo;
        }

        public void GetAccountability()
        {
            for (int i = 0; i != m_sORFrom.Count; i++)
            {
                OracleResultSet result = new OracleResultSet();
                result.Query = "SELECT MIN(B.OR_NO), MIN(A.TO_OR_NO), MAX(to_number(B.OR_NO)) FROM OR_RETURNED A, OR_USED B "; //AFM 20200124 changed to_number due to changed datatype
                result.Query += "WHERE A.TELLER_CODE = '" + m_sTeller.Trim() + "' ";
                result.Query += "AND A.TRN_DATE = to_date('" + m_dtDateTrans.ToShortDateString() + "','MM/dd/yyyy') ";
                result.Query += "AND A.FROM_OR_NO BETWEEN '" + m_sORFrom[i] + "' AND '" + m_sORTo[i] + "' ";
                result.Query += "AND B.TELLER = '" + m_sTeller.Trim() + "' ";
                result.Query += "AND B.TRN_DATE = to_date('" + m_dtDateTrans.ToShortDateString() + "','MM/dd/yyyy') ";
                result.Query += "AND B.OR_NO BETWEEN '" + m_sORFrom[i] + "' AND '" + m_sORTo[i] + "' ";
                try
                {
                    if (m_sFormType[i].ToString().Trim() != "")
                        result.Query += "and b.form_type = '" + m_sFormType[i].ToString() + "' ";
                }
                catch { }
                result.Query += "ORDER BY A.FROM_OR_NO ASC";
                try
                {
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            sBeginningORFrom.Add(result.GetString(0).Trim());
                            sBeginningORTo.Add(result.GetString(1).Trim());
                            iBeginningQty.Add(int.Parse(result.GetString(1).Trim()) - int.Parse(result.GetString(0).Trim()) + 1);

                            sIssuedORFrom.Add(result.GetString(0).Trim());
                            sIssuedORTo.Add(result.GetString(2).Trim());
                            nIssuedQty.Add(int.Parse(result.GetString(2).Trim()) - int.Parse(result.GetString(0).Trim()) + 1);

                            sEndingORFrom.Add((int.Parse(result.GetString(2).Trim()) + 1).ToString("0000000"));
                            sEndingORTo.Add(result.GetString(1).Trim());
                            iEndingQty.Add(int.Parse(result.GetString(1).Trim()) - int.Parse((int.Parse(result.GetString(2).Trim()) + 1).ToString("0000000")) + 1);
                        }
                    }
                    result.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\nPlease contact your software support team",
                        "Err GetAccountability", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //ADD COLLECTION OF CASH TICKET
                result.Query = "SELECT * FROM DCT_USED WHERE COLLECTOR = '" + m_sTeller.Trim() + "' ";
                result.Query += "AND TRN_DATE = to_date('" + m_dtDateTrans.ToShortDateString() + "','MM/dd/yyyy')";
                try
                {
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            OracleResultSet rs = new OracleResultSet();
                            rs.Query = "SELECT * FROM DCT_INV WHERE AMOUNTPC = '" + result.GetFloat("AMOUNT") + "'";
                            if (rs.Execute())
                            {
                                while (rs.Read())
                                {
                                    sBeginningORFrom.Add(rs.GetString("SERIES_FR"));
                                    sBeginningORTo.Add(rs.GetString("SERIES_TO"));
                                    iBeginningQty.Add(rs.GetInt("NOOFPCS"));

                                    sBeginningORFrom.Add(result.GetString("SERIES_FR"));
                                    sBeginningORTo.Add(result.GetString("SERIES_TO"));
                                    iBeginningQty.Add(result.GetInt("NO_PCS"));

                                    sBeginningORFrom.Add(int.Parse(result.GetString("SERIES_TO") + 1).ToString("0000000"));
                                    sBeginningORTo.Add(rs.GetString("SERIES_TO"));
                                    iBeginningQty.Add(int.Parse(rs.GetString("SERIES_TO")) - int.Parse(result.GetString("SERIES_TO") + 1));
                                }
                            }
                            rs.Close();
                        }
                    }
                    result.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err LoadFeeType/Cashticket", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public override List<int> BeginningQty()
        {
            return iBeginningQty;
        }

        public override List<string> BeginningORFrom()
        {
            return sBeginningORFrom;
        }

        public override List<string> BeginningORTo()
        {
            return sBeginningORTo;
        }

        public override List<int> IssuedQty()
        {
            return nIssuedQty;
        }

        public override List<string> IssuedORFrom()
        {
            return sIssuedORFrom;
        }

        public override List<string> IssuedORTo()
        {
            return sIssuedORTo;
        }

        public override List<int> EndingQty()
        {
            return iEndingQty;
        }

        public override List<string> EndingORFrom()
        {
            return sEndingORFrom;
        }

        public override List<string> EndingORTo()
        {
            return sEndingORTo;
        }
    }
}
