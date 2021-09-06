using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.ApplicationRequirements
{
    public static class AppRequirement
    {
        public static bool Checklist(string sTaxYear, string sBIN, string sStatus, string sBnsCode, string sOrgKind)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pCheckList = new OracleResultSet();
            string sReqCode = "";
            string sOtherDesc = "";
            string sOtherVar = "";

            pCheckList.Query = "select * from REQUIREMENTS_CHKLIST ";
            if (sStatus == "NEW")
            {
                pCheckList.Query += " where bns_stat = 'NEW'";
            }
            else
            {
                pCheckList.Query += " where bns_stat = 'RENEWAL'";
            }
            pCheckList.Query += string.Format(" and (bns_code = 'ALL' or bns_code = '{0}') ", sBnsCode);
            pCheckList.Query += string.Format(" and bns_org = '{0}'", sOrgKind);
            pCheckList.Query += " order by req_code";
            if (pCheckList.Execute())
            {
                while (pCheckList.Read())
                {
                    sReqCode = pCheckList.GetString("req_code");
                    sOtherDesc = pCheckList.GetString("other_desc");
                    sOtherVar = string.Format("{0:###.00}", pCheckList.GetDouble("other_var"));
                    if (sOtherVar == "")
                        sOtherVar = "0";

                    if (sOtherDesc == "")
                    {
                        pRec.Query = string.Format("select * from requirements_tbl where bin = '{0}' and tax_year = '{1}' and req_code = '{2}'", sBIN, sTaxYear, sReqCode);
                        if (pRec.Execute())
                        {
                            if (pRec.Read())
                            {
                            }
                            else
                            {
                                pCheckList.Close();
                                pRec.Close();
                                return false;
                            }
                        }
                        pRec.Close();
                    }
                    else
                    {
                        if (sOtherDesc == "GROSS")
                        {
                            double dGross = 0;

                            pRec.Query = string.Format("select gr_1 from businesses where bin = '{0}' and tax_year = '{1}'", sBIN, sTaxYear);
                            double.TryParse(pRec.ExecuteScalar(), out dGross);

                            pRec.Query = string.Format("select sum(gross) from addl_bns where bin = '{0}' and tax_year = '{1}'", sBIN, sTaxYear);
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                {
                                    dGross += pRec.GetDouble(0);
                                }
                            }
                            pRec.Close();

                            if (dGross >= Convert.ToDouble(sOtherVar))
                            {
                                pRec.Query = string.Format("select * from requirements_tbl where bin = '{0}' and tax_year = '{1}' and req_code = '{2}'", sBIN, sTaxYear, sReqCode);
                                if (pRec.Execute())
                                {
                                    if (pRec.Read())
                                    {
                                    }
                                    else
                                    {
                                        pCheckList.Close();
                                        pRec.Close();
                                        return false;
                                    }
                                }
                                pRec.Close();
                            }
                        }

                        if (sOtherDesc == "PEZA" || sOtherDesc == "BOI")
                        {
                            pRec.Query = string.Format("select * from boi_table where bin = '{0}' and exempt_type = '{1}'", sBIN, sOtherDesc);
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                {
                                    pRec.Close();

                                    pRec.Query = string.Format("select * from requirements_tbl where bin = '{0}' and tax_year = '{1}' and req_code = '{2}'", sBIN, sTaxYear, sReqCode);
                                    if (pRec.Execute())
                                    {
                                        if (pRec.Read())
                                        {
                                        }
                                        else
                                        {
                                            pCheckList.Close();
                                            pRec.Close();
                                            return false;
                                        }
                                    }
                                    pRec.Close();
                                }
                            }
                            pRec.Close();
                        }

                        // RMC 20171124 Added checklist requirement specific to Malolos (s)
                        bool bTagged = false;   // note: MALOLOS PUBLIC MARKET is indicated in bns street
                        //if (sOtherDesc == "MAPUMA")
                        if (sOtherDesc == "MAPUMA" || sOtherDesc == "SUBD" || sOtherDesc == "SUBDIVISION")  // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA
                        {
                            // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA (s)
                            if (sOtherDesc == "SUBD" || sOtherDesc == "SUBDIVISION")
                                pRec.Query = string.Format("select * from businesses where bin = '{0}' and tax_year = '{1}' and bns_street like '%SUBD%'", sBIN, sTaxYear);
                            else
                            // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA (e)
                            pRec.Query = string.Format("select * from businesses where bin = '{0}' and tax_year = '{1}' and (bns_street like '%MALOLOS PUBLIC MARKET%' or bns_street like '%MAPUMA%')", sBIN, sTaxYear);
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                {
                                    bTagged = true;
                                }
                            }
                            pRec.Close();

                            if (bTagged)
                            {
                                pRec.Query = string.Format("select * from requirements_tbl where bin = '{0}' and tax_year = '{1}' and req_code = '{2}'", sBIN, sTaxYear, sReqCode);
                                if (pRec.Execute())
                                {
                                    if (pRec.Read())
                                    {
                                    }
                                    else
                                    {
                                        pCheckList.Close();
                                        pRec.Close();
                                        return false;
                                    }
                                }
                                pRec.Close();
                            }
                        }
                        // RMC 20171124 Added checklist requirement specific to Malolos (e)
                    }
                }
            }
            pCheckList.Close();

            return true;
        }

        public static string Checklist(string sTaxYear, string sBIN, string sStatus, string sBnsCode, string sOrgKind, string sChecklistDesc)
        {
            // RMC 20180131 added validation of specific checklist in Billing
            OracleResultSet pCheckList = new OracleResultSet();
            string sCode = string.Empty;
            string sDesc = string.Empty;
            if (sOrgKind == "SINGLE PROPRIETORSHIP")
                sOrgKind = "SINGLE";

            pCheckList.Query = "select * from REQUIREMENTS_CHKLIST ";
            if (sStatus == "NEW")
            {
                pCheckList.Query += " where bns_stat = 'NEW'";
            }
            else
            {
                pCheckList.Query += " where bns_stat = 'RENEWAL'";
            }
            pCheckList.Query += string.Format(" and (bns_code = 'ALL' or bns_code = '{0}') ", sBnsCode);
            pCheckList.Query += string.Format(" and bns_org = '{0}'", sOrgKind);
            pCheckList.Query += string.Format(" and other_desc like '%{0}%'", sChecklistDesc);
            if (pCheckList.Execute())
            {
                if (pCheckList.Read())
                {
                    sCode = pCheckList.GetString("req_code");
                    sDesc = pCheckList.GetString("req_desc");
                }
            }
            pCheckList.Close();

            if (sCode != string.Empty || sCode.Trim() == "")
            {
                OracleResultSet pSet = new OracleResultSet();

                pCheckList.Query = "select * from requirements_tbl where req_code = '" + sCode + "' and bin = '" + sBIN + "'";
                if (pCheckList.Execute())
                {
                    if (!pCheckList.Read())
                    {
                        pSet.Query = "select * from business_que where bin = '" + sBIN + "' and bns_street like '%SUBD%'";
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            { }
                            else
                                sDesc = "";
                        }
                    }
                    else
                        sDesc = "";
                }
                pCheckList.Close();
            }

            return sDesc;
        }
    }
}
