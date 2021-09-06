using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace BusinessSummary
{
    class DataAccess
    {
        public static List<string> DistrictList()
        {
            List<string> lstDistrict = new List<string>();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select distinct dist_nm from brgy order by dist_nm";
            if (result.Execute())
            {
                while (result.Read())
                {
                    lstDistrict.Add(result.GetString("dist_nm").Trim());
                }
            }
            result.Close();

            if (lstDistrict.Count == 0)
                lstDistrict.Add(" ");

            return lstDistrict;
        }

        public static List<string> BarangayList(String District)
        {
            List<string> lstBarangay = new List<string>();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select brgy_nm from brgy ";

            //if (District != "")
            if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                result.Query += "where dist_nm = '" + District + "' ";

            result.Query += "order by brgy_code";
            if (result.Execute())
            {
                while (result.Read())
                {
                    lstBarangay.Add(result.GetString("brgy_nm").Trim());
                }
            }

            result.Close();

            return lstBarangay;
        }
        //AFM 20191205 MAO-19-10958(s)
        private static bool HasSubCat(string BnsCode, bool HasSub)
        {
            int iBns = 0;
            int iCnt = 0;
            iBns = BnsCode.Length;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select count(*) as COUNT from bns_table where  fees_code = 'B' and bns_code like '" + BnsCode + "%' and length(bns_code) > 4";
            int.TryParse(result.ExecuteScalar(), out iCnt);
            if (BnsCode.Length == 4)
            {
                if (iCnt > 1)
                {
                    HasSub = true;
                    return HasSub;
                }
                else
                {
                    HasSub = false;
                    return HasSub;
                }
            }
            return false;
        }
        //AFM 20191205 MAO-19-10958(e)

        public static int BusinessCount(String Brgy, String District, String Status, String BnsCode, String OrgnKind, String TaxYear, Boolean BasedOnRegistration)
        {
          
            OracleResultSet result = new OracleResultSet();
            int iBnsCount = 0;

            bool hasSub = false;
            hasSub = HasSubCat(BnsCode, false); //AFM 20191205 MAO-19-10958

            result.Query = "select count(*) as bns_count from businesses where 1=1 ";
            
            if (Status != "")
                result.Query += "and bns_stat = '" + Status + "' ";

            if (Brgy != "")
                result.Query += "and bns_brgy = '" + Brgy + "' ";

            if (AppSettingsManager.GetConfigValue("01") != "MUNICIPALITY")
            {
                //if (District != "")
                if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                    result.Query += "and bns_dist = '" + District + "' ";
            }

            if (BnsCode != "")
                if(hasSub == true)
                    result.Query += "and bns_code like '" + BnsCode + "' "; //AFM 20191205 MAO-19-10958
                else
                    result.Query += "and bns_code like '" + BnsCode + "%' ";

            if (OrgnKind != "")
                result.Query += "and orgn_kind = '" + OrgnKind + "' ";

            if (TaxYear != "")
                result.Query += "and tax_year = '" + TaxYear + "' ";

            if (result.Execute())
            {
                while (result.Read())
                {
                    iBnsCount = result.GetInt("bns_count");
                }
            }
            result.Close();

            if (BasedOnRegistration)
            {
                result.Query = "select count(*) as bns_count from buss_hist where 1=1 ";

                if (Status != "")
                    result.Query += "and bns_stat = '" + Status + "' ";

                if (Brgy != "")
                    result.Query += "and bns_brgy = '" + Brgy + "' ";

                //if (District != "")
                if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                    result.Query += "and bns_dist = '" + District + "' ";

                if (BnsCode != "")
                    if (hasSub == true)
                        result.Query += "and bns_code like '" + BnsCode + "' "; //AFM 20191206 MAO-19-10958
                    else
                        result.Query += "and bns_code like '" + BnsCode + "%' ";

                if (OrgnKind != "")
                    result.Query += "and orgn_kind = '" + OrgnKind + "' ";

                if (TaxYear != "")
                {
                    result.Query += "and tax_year = '" + TaxYear + "' ";

                    result.Query += " and bin not in (select bin from businesses where tax_year = '" + TaxYear + "')";   // RMC 20170227 correction in Summary of Business
                }
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        iBnsCount += result.GetInt("bns_count");
                    }
                }
                result.Close();
            }

            return iBnsCount;
        }

        public static double Capital(String Brgy, String District, String Status, String BnsCode, String OrgnKind, String BIN, String TaxYear, Boolean BasedOnRegistration)
        {
            bool hasSub = false;
            hasSub = HasSubCat(BnsCode, false); //AFM 20191205 MAO-19-10958
            OracleResultSet result = new OracleResultSet();
            double dCapital = 0.00;
            result.Query = "select sum(capital) as capital_mn from businesses where 1=1 ";

            if (Brgy != "")
                result.Query += "and bns_brgy = '" + Brgy + "' ";

            if (AppSettingsManager.GetConfigValue("01") != "MUNICIPALITY") 
            {
                //if (District != "")
                if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                    result.Query += "and bns_dist = '" + District + "' ";
            }

            if (Status != "")
                result.Query += "and bns_stat = '" + Status + "' ";

            if (BnsCode != "")
                if (hasSub == true)
                    result.Query += "and bns_code like '" + BnsCode + "' ";
                else
                    result.Query += "and bns_code like '" + BnsCode + "%' ";

            if (OrgnKind != "")
                result.Query += "and orgn_kind = '" + OrgnKind + "' ";

            if (TaxYear != "")
                result.Query += "and tax_year = '" + TaxYear + "' ";

            if (BIN != "")
            {
                result.Query = "select sum(capital) as capital_mn from addl_bns where bin = '" + BIN + "' and tax_year = '" + TaxYear + "' and bns_stat = 'NEW'";    
            }

            if (result.Execute())
            {
                while (result.Read())
                {
                    dCapital = result.GetDouble("capital_mn");
                }
            }
            result.Close();

            if (BasedOnRegistration)
            {
                result.Query = "select sum(capital) as capital_mn from buss_hist where 1=1 ";

                if (Brgy != "")
                    result.Query += "and bns_brgy = '" + Brgy + "' ";

                //if (District != "")
                if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                    result.Query += "and bns_dist = '" + District + "' ";

                if (Status != "")
                    result.Query += "and bns_stat = '" + Status + "' ";

                if (BnsCode != "")
                    if (hasSub == true)
                        result.Query += "and bns_code like '" + BnsCode + "' "; //AFM 20191206 MAO-19-10958
                    else
                        result.Query += "and bns_code like '" + BnsCode + "%' ";

                if (OrgnKind != "")
                    result.Query += "and orgn_kind = '" + OrgnKind + "' ";

                if (TaxYear != "")
                {
                    result.Query += "and tax_year = '" + TaxYear + "' ";
                    result.Query += " and bin not in (select bin from businesses where tax_year = '" + TaxYear + "')";   // RMC 20170227 correction in Summary of Business
                }

                if (BIN != "")
                {
                    //result.Query = "select sum(capital) as capital_mn from addl_bns where bin = '" + BIN + "' and tax_year = '" + TaxYear + "' and bns_stat = 'NEW'";
                    result.Query = "select sum(capital) as capital_mn from addl_bns_hist where bin = '" + BIN + "' and tax_year = '" + TaxYear + "' and bns_stat = 'NEW'";   // RMC 20171114 correction in Management report total of capital
                }

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        dCapital += result.GetDouble("capital_mn");
                    }
                }
                result.Close();
            }

            return dCapital;
        }

        public static double CapitalAddl(String Brgy, String District, String Status, String BnsCode, String OrgnKind, String TaxYear, Boolean BasedOnRegistration)
        {
            bool hasSub = false;
            hasSub = HasSubCat(BnsCode, false); //AFM 20191205 MAO-19-10958
            OracleResultSet result = new OracleResultSet();
            double dCapitalAddl = 0.00;
            result.Query = "select sum(a.capital) as capital_addl from addl_bns a, businesses b ";
            result.Query += "where a.bin = b.bin and a.bns_stat = b.bns_stat and a.tax_year = b.tax_year ";

            if (Brgy != "")
                result.Query += "and b.bns_brgy = '" + Brgy + "' ";

            if (AppSettingsManager.GetConfigValue("01") != "MUNICIPALITY")
            {
                //if (District != "")
                if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                    result.Query += "and b.bns_dist = '" + District + "' ";
            }

            if (Status != "")
                result.Query += "and b.bns_stat = '" + Status + "' ";

            if (BnsCode != "")
                if (hasSub == true)
                    result.Query += "and a.bns_code_main like '" + BnsCode + "' ";
                else
                    result.Query += "and a.bns_code_main like '" + BnsCode + "%' ";

            if (OrgnKind != "")
                result.Query += "and b.orgn_kind = '" + OrgnKind + "' ";

            if (TaxYear != "")
                result.Query += "and a.tax_year = '" + TaxYear + "' and b.tax_year = '" + TaxYear + "' ";


            
            if (result.Execute())
            {
                while (result.Read())
                {
                    dCapitalAddl = result.GetDouble("capital_addl");
                }
            }
            result.Close();

            if (BasedOnRegistration)
            {
                result.Query = "select sum(a.capital) as capital_addl from addl_bns a, buss_hist b ";
                result.Query += "where a.bin = b.bin and a.bns_stat = b.bns_stat and a.tax_year = b.tax_year ";

                if (Brgy != "")
                    result.Query += "and b.bns_brgy = '" + Brgy + "' ";

                //if (District != "")
                if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                    result.Query += "and b.bns_dist = '" + District + "' ";

                if (Status != "")
                    result.Query += "and b.bns_stat = '" + Status + "' ";

                if (BnsCode != "")
                    if (hasSub == true)
                        result.Query += "and a.bns_code_main like '" + BnsCode + "' "; //AFM 20191206 MAO-19-10958
                    else
                        result.Query += "and a.bns_code_main like '" + BnsCode + "%' ";

                if (OrgnKind != "")
                    result.Query += "and b.orgn_kind = '" + OrgnKind + "' ";

                if (TaxYear != "")
                {
                    result.Query += "and a.tax_year = '" + TaxYear + "' and b.tax_year = '" + TaxYear + "' ";
                    result.Query += " and b.bin not in (select bin from businesses where tax_year = '" + TaxYear + "')";   // RMC 20170227 correction in Summary of Business
                }

                

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        dCapitalAddl += result.GetDouble("capital_addl");
                    }
                }
                result.Close();
            }

            return dCapitalAddl;
        }

        public static double Gross(String Brgy, String District, String Status, String BnsCode, String OrgnKind, String TaxYear, Boolean BasedOnRegistration)
        {
            bool hasSub = false;
            hasSub = HasSubCat(BnsCode, false); //AFM 20191205 MAO-19-10958
            OracleResultSet result = new OracleResultSet();
            double dGross = 0.00;
            result.Query = "select sum(gr_1) as gross_mn from businesses where 1=1 ";

            if (Status != "")
            {
                if (OrgnKind != "")
                    result.Query += "and bns_stat <> 'NEW' ";
                else
                    result.Query += "and bns_stat = '" + Status + "' ";
            }

            if (Brgy != "")
                result.Query += "and bns_brgy = '" + Brgy + "' ";

            if (AppSettingsManager.GetConfigValue("01") != "MUNICIPALITY")
            {
                //if (District != "")
                if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                    result.Query += "and bns_dist = '" + District + "' ";
            }

            if (BnsCode != "")
                if (hasSub == true)
                    result.Query += "and bns_code like '" + BnsCode + "' "; //AFM 20191205 MAO-19-10958
                else
                    result.Query += "and bns_code like '" + BnsCode + "%' "; 

            if (OrgnKind != "")
                result.Query += "and orgn_kind = '" + OrgnKind + "' ";

            if (TaxYear != "")
                result.Query += "and tax_year = '" + TaxYear + "' ";

            if (result.Execute())
            {
                while (result.Read())
                {
                    dGross = result.GetDouble("gross_mn");
                }
            }
            result.Close();

            if (BasedOnRegistration)
            {
                result.Query = "select sum(gr_1) as gross_mn from buss_hist where 1=1 ";

                if (Status != "")
                {
                    if (OrgnKind != "")
                        result.Query += "and bns_stat <> 'NEW' ";
                    else
                        result.Query += "and bns_stat = '" + Status + "' ";
                }

                if (Brgy != "")
                    result.Query += "and bns_brgy = '" + Brgy + "' ";

                if (AppSettingsManager.GetConfigValue("01") != "MUNICIPALITY")  // RMC 20150520 corrections in reports
                {
                    //if (District != "")
                    if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                        result.Query += "and bns_dist = '" + District + "' ";
                }

                if (BnsCode != "")
                    if (hasSub == true)
                        result.Query += "and bns_code like '" + BnsCode + "' "; //AFM 20191206 MAO-19-10958
                    else
                        result.Query += "and bns_code like '" + BnsCode + "%' ";

                if (OrgnKind != "")
                    result.Query += "and orgn_kind = '" + OrgnKind + "' ";

                if (TaxYear != "")
                {
                    result.Query += "and tax_year = '" + TaxYear + "' ";
                    result.Query += " and bin not in (select bin from businesses where tax_year = '" + TaxYear + "')";   // RMC 20170227 correction in Summary of Business
                }

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        dGross += result.GetDouble("gross_mn");
                    }
                }
                result.Close();
            }

            return dGross;
        }

        public static double GrossAddl(String Brgy, String District, String Status, String BnsCode, String OrgnKind, String BIN, String TaxYear, Boolean BasedOnRegistration)
        {
            bool hasSub = false;
            hasSub = HasSubCat(BnsCode, false); //AFM 20191205 MAO-19-10958
            OracleResultSet result = new OracleResultSet();
            double dGrossAddl = 0.00;
            result.Query = "select sum(a.gross) from addl_bns a, businesses b where 1=1 ";
            
            result.Query += "and a.bin = b.bin and a.bns_stat = b.bns_stat and a.tax_year = b.tax_year ";            

            if (Status != "")
            {
                if (OrgnKind != "")
                    result.Query += "and a.bns_stat <> 'NEW' ";
                else
                    result.Query += "and a.bns_stat = '" + Status + "' ";
            }

            if (Brgy != "")
                result.Query += "and b.bns_brgy = '" + Brgy + "' ";

            if (AppSettingsManager.GetConfigValue("01") != "MUNICIPALITY")
            {
                //if (District != "")
                if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                    result.Query += "and b.bns_dist = '" + District + "' ";
            }

            if (BnsCode != "")
                if (hasSub == true)
                    result.Query += "and a.bns_code_main like '" + BnsCode + "' "; //AFM 20191205 MAO-19-10958
                else
                    result.Query += "and a.bns_code_main like '" + BnsCode + "%' ";

            if (OrgnKind != "")
                result.Query += "and b.orgn_kind = '" + OrgnKind + "' ";

            if (TaxYear != "")
                result.Query += "and a.tax_year = '" + TaxYear + "' and b.tax_year = '" + TaxYear + "' ";

            if (BIN != "")
            {
                result.Query = "select sum(gross) from addl_bns where 1=1 and bin = '" + BIN + "' and tax_year = '" + TaxYear + "' and bns_stat = 'REN'";
            }

            if (result.Execute())
            {
                while (result.Read())
                {
                    dGrossAddl = result.GetDouble(0);
                }
            }
            result.Close();

            if (BasedOnRegistration)
            {
                result.Query = "select sum(a.gross) from addl_bns a, buss_hist b where 1=1 ";

                result.Query += "and a.bin = b.bin and a.bns_stat = b.bns_stat and a.tax_year = b.tax_year ";

                if (Status != "")
                {
                    if (OrgnKind != "")
                        result.Query += "and a.bns_stat <> 'NEW' ";
                    else
                        result.Query += "and a.bns_stat = '" + Status + "' ";
                }

                if (Brgy != "")
                    result.Query += "and b.bns_brgy = '" + Brgy + "' ";

                if (AppSettingsManager.GetConfigValue("01") != "MUNICIPALITY")  // RMC 20150520 corrections in reports
                {
                    //if (District != "")
                    if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                        result.Query += "and b.bns_dist = '" + District + "' ";
                }

                if (BnsCode != "")
                    if (hasSub == true)
                        result.Query += "and a.bns_code_main like '" + BnsCode + "' "; //AFM 20191206 MAO-19-10958
                    else
                        result.Query += "and a.bns_code_main like '" + BnsCode + "%' ";

                if (OrgnKind != "")
                    result.Query += "and b.orgn_kind = '" + OrgnKind + "' ";

                if (TaxYear != "")
                {
                    result.Query += "and a.tax_year = '" + TaxYear + "' and b.tax_year = '" + TaxYear + "' ";
                    result.Query += " and b.bin not in (select bin from businesses where tax_year = '" + TaxYear + "')";   // RMC 20170227 correction in Summary of Business
                }

                if (BIN != "")
                {
                    //result.Query = "select sum(gross) from addl_bns where 1=1 and bin = '" + BIN + "' and tax_year = '" + TaxYear + "' and bns_stat = 'REN'";
                    result.Query = "select sum(gross) from addl_bns_hist where 1=1 and bin = '" + BIN + "' and tax_year = '" + TaxYear + "' and bns_stat = 'REN'";  // RMC 20150520 corrections in reports
                }

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        dGrossAddl += result.GetDouble(0);
                    }
                }
                result.Close();
            }

            return dGrossAddl;
        }

        public static double PreGR(String Brgy, String District, String Status, String BnsCode, String OrgnKind, String BIN, String TaxYear, Boolean BasedOnRegistration)
        {
            bool hasSub = false;
            hasSub = HasSubCat(BnsCode, false); //AFM 20191205 MAO-19-10958
            OracleResultSet result = new OracleResultSet();
            double PreGR = 0.00;
            result.Query = "select sum(presumptive_gr) as pre_gr from declared_gross where 1=1 ";

            if (BIN != "")
            {
                result.Query += "and bin = '" + BIN + "' and tax_year = '" + TaxYear + "' ";
            }
            else
            {
                result.Query += "and bin in (select bin from businesses where 1=1 ";                
            }

            if (Status != "")
            {
                if (OrgnKind != "")
                    result.Query += "and bns_stat <> 'NEW' ";
                else
                    result.Query += "and bns_stat = '" + Status + "' ";
            }

            if (Brgy != "")
                result.Query += "and bns_brgy = '" + Brgy + "' ";

            if (AppSettingsManager.GetConfigValue("01") != "MUNICIPALITY")
            {
                //if (District != "")
                if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                    result.Query += "and bns_dist = '" + District + "' ";
            }

            if (OrgnKind != "")
                result.Query += "and orgn_kind = '" + OrgnKind + "' ";

            if (TaxYear != "")
                result.Query += "and tax_year = '" + TaxYear + "' ";

            if (BIN == "")
                result.Query += ") ";

            if (TaxYear != "")
                result.Query += "and tax_year = '" + TaxYear + "' ";

            if (BnsCode != "")
                if (hasSub == true)
                    result.Query += "and bns_code like '" + BnsCode + "' "; //AFM 20191205 MAO-19-10958
                else
                    result.Query += "and bns_code like '" + BnsCode + "%' ";

            if (result.Execute())
            {
                while (result.Read())
                {
                    PreGR = result.GetDouble("pre_gr");
                }
            }
            result.Close();

            if (BasedOnRegistration)
            {
                result.Query = "select sum(presumptive_gr) as pre_gr from declared_gross where 1=1 ";

                if (BIN != "")
                {
                    result.Query += "and bin = '" + BIN + "' and tax_year = '" + TaxYear + "' ";
                }
                else
                {
                    result.Query += "and bin in (select bin from buss_hist where 1=1 ";
                }

                if (Status != "")
                {
                    if (OrgnKind != "")
                        result.Query += "and bns_stat <> 'NEW' ";
                    else
                        result.Query += "and bns_stat = '" + Status + "' ";
                }

                if (Brgy != "")
                    result.Query += "and bns_brgy = '" + Brgy + "' ";

                if (AppSettingsManager.GetConfigValue("01") != "MUNICIPALITY")
                {
                    //if (District != "")
                    if (District.Trim() != "")  // RMC 20170227 correction in Summary of Business
                        result.Query += "and bns_dist = '" + District + "' ";
                }

                if (OrgnKind != "")
                    result.Query += "and orgn_kind = '" + OrgnKind + "' ";

                if (TaxYear != "")
                    result.Query += "and tax_year = '" + TaxYear + "' ";

                if (BIN == "")
                    result.Query += ") ";

                if (TaxYear != "")
                {
                    result.Query += "and tax_year = '" + TaxYear + "' ";
                    result.Query += " and bin not in (select bin from businesses where tax_year = '" + TaxYear + "')";   // RMC 20170227 correction in Summary of Business
                }

                if (BnsCode != "")
                    if (hasSub == true)
                        result.Query += "and bns_code like '" + BnsCode + "' "; //AFM 20191206 MAO-19-10958
                    else
                        result.Query += "and bns_code like '" + BnsCode + "%' ";

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        PreGR += result.GetDouble("pre_gr");
                    }
                }
                result.Close();
            }

            return PreGR;
        }

        public static void BusinessTable(String FeesCode, String RevYear, out List<String> BnsCode, out List<String> BnsDesc, bool includeSub)
        {
            OracleResultSet result = new OracleResultSet();
            List<String> strBnsCode = new List<string>();
            List<String> strBnsDesc = new List<string>();
            if (includeSub == true) //AFM 20191204 MAO-19-10958
                result.Query = "select * from bns_table where 1=1";
            else
                result.Query = "select * from bns_table where 1=1 and length(rtrim(bns_code)) = 2"; // TEST  and length(rtrim(bns_code)) = 2

            if (FeesCode != "")
                result.Query += "and fees_code = '" + FeesCode + "' ";
            else
                result.Query += "and fees_code = 'B' ";

            /*if (RevYear != "")
                result.Query += "and rev_year = '" + RevYear + "' ";
            else
                result.Query += "and rev_year = '1993' ";*/ // RMC 20150429 corrected reports

            result.Query += "and rev_year = '" + AppSettingsManager.GetConfigValue("07")+ "' ";    // RMC 20150429 corrected reports

            if (includeSub == true)
                result.Query += "order by bns_code";
            else
                result.Query += "order by bns_desc";

            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsCode.Add(result.GetString("bns_code"));
                    strBnsDesc.Add(result.GetString("bns_desc"));
                }
            }
            result.Close();

            BnsCode = strBnsCode;
            BnsDesc = strBnsDesc;

        }

        public static int RetireNo(String TaxYear)
        {
            OracleResultSet result = new OracleResultSet();
            int iRetireNo = 0;
            result.Query = "select count(*) as retire_cnt from businesses where bns_stat = 'RET' ";

            // RMC 20171114 correction in Management report, do not include presumptive gr in total gr, enabled (s)
            if (TaxYear != "")
                result.Query += "and tax_year = '" + TaxYear + "' ";
            // RMC 20171114 correction in Management report, do not include presumptive gr in total gr, enabled (e)

            if (result.Execute())
            {
                while (result.Read()) 
                {
                    iRetireNo = result.GetInt(0);
                }
            }
            result.Close();

            return iRetireNo;
        }

        public static void Businesses(String Brgy, String District, String Status, out List<String> BIN, out List<String> outTaxYear, out List<double> GR, out List<double> Capital, String TaxYear, Boolean BasedOnRegistration)
        {
            List<string> lstBIN = new List<string>();
            List<string> lstTaxYear = new List<string>();
            List<double> lstGR = new List<double>();
            List<double> lstCapital = new List<double>();

            OracleResultSet result = new OracleResultSet();

            // RMC 20150429 corrected reports (s)
            //if(District == AppSettingsManager.GetConfigValue("02"))
            if (District == AppSettingsManager.GetConfigValue("02") || District.Trim() == "" || District == null)   // RMC 20171114 correction in Management report, do not include presumptive gr in total gr
                result.Query = "select * from businesses where bns_stat = '" + Status + "' and bns_brgy = '" + Brgy + "' ";
            else// RMC 20150429 corrected reports (e)
                result.Query = "select * from businesses where bns_stat = '" + Status + "' and bns_brgy = '" + Brgy + "' and bns_dist = '" + District + "' ";

            if (TaxYear != "")
                result.Query += "and tax_year = '" + TaxYear + "' ";

            if (result.Execute())
            {
                while (result.Read())
                {
                    lstBIN.Add(result.GetString("bin"));
                    lstTaxYear.Add(result.GetString("tax_year"));
                    lstGR.Add(result.GetDouble("gr_1"));
                    lstCapital.Add(result.GetDouble("capital"));
                }
            }
            result.Close();

            if (BasedOnRegistration)
            {
                // RMC 20150429 corrected reports (s)
                //if (District == AppSettingsManager.GetConfigValue("02"))
                if (District == AppSettingsManager.GetConfigValue("02") || District.Trim() == "" || District == null)   // RMC 20171114 correction in Management report, do not include presumptive gr in total gr
                    result.Query = "select * from buss_hist where bns_stat = '" + Status + "' and bns_brgy = '" + Brgy + "' ";
                else// RMC 20150429 corrected reports (e)
                    result.Query = "select * from buss_hist where bns_stat = '" + Status + "' and bns_brgy = '" + Brgy + "' and bns_dist = '" + District + "' ";

                if (TaxYear != "")
                {
                    result.Query += "and tax_year = '" + TaxYear + "' ";
                    result.Query += " and bin not in (select bin from businesses where tax_year = '" + TaxYear + "')";   // RMC 20170227 correction in Summary of Business
                }

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        lstBIN.Add(result.GetString("bin"));
                        lstTaxYear.Add(result.GetString("tax_year"));
                        lstGR.Add(result.GetDouble("gr_1"));
                        lstCapital.Add(result.GetDouble("capital"));
                    }
                }
                result.Close();
            }

            BIN = lstBIN;
            outTaxYear = lstTaxYear;
            GR = lstGR;
            Capital = lstCapital;
        }
    }
}
