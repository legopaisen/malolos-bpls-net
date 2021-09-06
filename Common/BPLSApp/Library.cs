using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.BPLSApp
{
    /// <summary>
    /// ALJ 20090701
    /// library for most commonly use functions
    /// </summary>
    public class Library
    {
        public Library()
        {
 
        }
        /// <summary>
        /// function to get qtr (1,2,3,4) based on date
        /// </summary>
        /// <returns></returns>
        public string GetQtr(DateTime p_sDate)
        {
            int iMonth;
            string sQtr;
            
            iMonth = p_sDate.Month;
            if (iMonth < 4)
                sQtr = "1";
            else if (iMonth < 7)
                sQtr = "2";
            else if (iMonth < 10)
                sQtr = "3";
            else
                sQtr = "4";

            return sQtr;
        }

        /// <summary>
        /// function of getting owners name with different format
        /// [1 = FN MI LN],[2 = LN FN MI']
        /// </summary>
        /// <returns></returns>
        public string GetOwnerName(int p_iFormat, string p_sOwnCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sOwnLN, sOwnFN, sOwnMI, sOwnersName;
            pSet.Query = "SELECT own_ln, own_fn, own_mi FROM own_names WHERE own_code = :1";
            pSet.AddParameter(":1", p_sOwnCode);
            sOwnersName = "";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sOwnLN = pSet.GetString("own_ln").Trim();
                    sOwnFN = pSet.GetString("own_fn").Trim();
                    sOwnMI = pSet.GetString("own_mi").Trim();
                    switch (p_iFormat)
                    {
                        case 1:
                            {
                                sOwnersName = sOwnFN;
                                if (sOwnMI != "" && sOwnMI != ".")
                                    sOwnersName = sOwnersName + " " + sOwnMI + ".";
                                sOwnersName = sOwnersName + " " + sOwnLN;
                                break;
                           }
                        case 2:
                            {
                                sOwnersName = sOwnFN;
                                if (sOwnMI != "" && sOwnMI != ".")
                                    sOwnersName = sOwnersName + " " + sOwnMI + ".";
                                if (sOwnFN == "")
                                    sOwnersName = sOwnLN;
                                else
                                    sOwnersName = sOwnLN + ", " + sOwnersName;
                                break;
                            }
                    }
                }
            }
            pSet.Close();
            return sOwnersName;
        }

        public string GetBnsDesc(string p_sFeesCode, string p_sBnsCode, string p_sRevYear)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBnsDesc = "";
            pSet.Query = "SELECT get_bns_desc(:1,:2,:3) AS bns_desc FROM DUAL";
            pSet.AddParameter(":1", p_sFeesCode);
            pSet.AddParameter(":2", p_sBnsCode);
            pSet.AddParameter(":3", p_sRevYear);
            if (pSet.Execute())
            {
                if(pSet.Read())
                {
                    sBnsDesc = pSet.GetString("bns_desc").Trim();
                }
            }
            pSet.Close();
            return sBnsDesc;
        }

        

        public string IsQtrlyDec(string p_sBnsCode, string p_sRevYear)
        {
            OracleResultSet pSet = new OracleResultSet();
	        string sData;
            sData = string.Empty;
            pSet.Query = "select is_qtrly from new_table where substr(bns_code,1,2) = :1 and rev_year = :2"; // ALJ 20110321 substr(bns_code,1,2)
            pSet.AddParameter(":1", p_sBnsCode.Substring(0,2));
            pSet.AddParameter(":2", p_sRevYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sData = pSet.GetString("is_qtrly").Trim();
                }
                else
                    sData = "N";    // RMC 20180108 correction in Billing of previously NEW and exempt in tax
            }
            pSet.Close();
	        return sData;
        }

        /// <summary>
        /// ALJ 20100616 Audit Trail
        /// </summary>
        /// <param name="MCode"></param>
        /// <param name="ATable"></param>
        /// <param name="DObject"></param>
        public void AuditTrail(string p_sMCode, string p_sATable, string p_sObject)
        {
            OracleResultSet pSet = new OracleResultSet();
            
            pSet.Query = "insert into a_trail values(:1, sysdate, :2, :3, :4, :5)";
            pSet.AddParameter(":1", AppSettingsManager.SystemUser.UserCode);
            pSet.AddParameter(":2", p_sMCode);
            pSet.AddParameter(":3", p_sATable);
            pSet.AddParameter(":4", AppSettingsManager.GetWorkstationName());
            pSet.AddParameter(":5", p_sObject);
            pSet.ExecuteNonQuery();

            pSet.Query = "select * from trail_table where usr_rights = :1";
            pSet.AddParameter(":1", p_sMCode);
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {
                    MessageBox.Show("Module code "+p_sMCode+" not in trail table.", "Audit Trail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            pSet.Close();

        }
 
    }

}
