using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using Amellar.Common.EncryptUtilities;
using Amellar.Common.StringUtilities;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace BusinessRoll
{
    class DataAccess
    {
        public static List<String> BarangayList()
        {
            List<String> strBarangay = new List<String>();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from brgy order by brgy_nm asc";
            if (result.Execute())
            {
                strBarangay.Add("ALL");
                while (result.Read())
                {
                    strBarangay.Add(result.GetString("brgy_nm").Trim());
                }
            }
            result.Close();

            return strBarangay;
        }

        public static List<string> DistrictList()
        {
            List<string> strDistrict = new List<string>();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select distinct dist_nm from brgy order by dist_nm";
            if (result.Execute())
            {
                strDistrict.Add("ALL");
                while (result.Read())
                {
                    strDistrict.Add(result.GetString("dist_nm").Trim());
                }
            }
            result.Close();

            //if (lstDistrict.Count == 0)
                //lstDistrict.Add(" ");

            return strDistrict;
        }

        public static void BusinessTypeList(out List<String> BnsCode, out List<String> BnsDesc)
        {
            List<String> strBnsCode = new List<String>();
            List<String> strBnsDesc = new List<String>();
            OracleResultSet result = new OracleResultSet();
            //result.Query = "select bns_desc, bns_code from bns_table where fees_code = 'B' and length(rtrim(bns_code)) = 2 and rev_year = '1993' order by bns_code ";
            result.Query = "select bns_desc, bns_code from bns_table where fees_code = 'B' and length(rtrim(bns_code)) = 2 and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' order by bns_code ";  // RMC 20150429 corrected reports
            if (result.Execute())
            {
                strBnsCode.Add("");
                strBnsDesc.Add("ALL");
                while (result.Read())
                {
                    strBnsCode.Add(result.GetString("bns_code").Trim());
                    strBnsDesc.Add(result.GetString("bns_desc").Trim());
                }
            }
            result.Close();
            BnsCode = strBnsCode;
            BnsDesc = strBnsDesc;
        }

        public static void BusinessNatureList(String BnsCode, out List<String> outBnsCode, out List<String> BnsDesc)
        {
            List<String> strBnsCode = new List<String>();
            List<String> strBnsDesc = new List<String>();
            OracleResultSet result = new OracleResultSet();
            //result.Query = "select bns_desc,bns_code from bns_table where fees_code = 'B' and length(rtrim(bns_code)) > 3 and rev_year = '1993' and rtrim(bns_code) like '" + BnsCode + "%%%' order by bns_code ";
            result.Query = "select bns_desc,bns_code from bns_table where fees_code = 'B' and length(rtrim(bns_code)) > 3 and rev_year = '"+AppSettingsManager.GetConfigValue("07") + "' and rtrim(bns_code) like '" + BnsCode + "%%%' order by bns_code ";  // RMC 20150429 corrected reports
            if (result.Execute())
            {
                strBnsCode.Add("");
                strBnsDesc.Add("ALL");
                while (result.Read())
                {
                    strBnsCode.Add(result.GetString("bns_code").Trim());
                    strBnsDesc.Add(result.GetString("bns_desc").Trim());
                }
            }
            result.Close();
            outBnsCode = strBnsCode;
            BnsDesc = strBnsDesc;

        }
    }
}
