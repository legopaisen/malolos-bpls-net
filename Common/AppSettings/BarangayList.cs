using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.AppSettings
{
    //@author R.D.Ong
    public class BarangayList
    {
        private string m_strDistrictCode;
        private List<string> m_lstDistrictCodes;
        private List<string> m_lstBarangayCodes;
        private List<string> m_lstBarangayNames;

        public BarangayList()
        {
            m_strDistrictCode = string.Empty;
            m_lstDistrictCodes = new List<string>();
            m_lstBarangayCodes = new List<string>();
            m_lstBarangayNames = new List<string>();
        }

        public void GetBarangayList(string strDistrictCode, bool blnIsOrderByName)
        {
            m_lstDistrictCodes.Clear();
            m_lstBarangayCodes.Clear();
            m_lstBarangayNames.Clear();

            string strTmpDistrictCode = AppSettingsManager.GetDistrictCode();
            OracleResultSet result = new OracleResultSet();
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append("SELECT dist_code, brgy_code, brgy_nm FROM brgy WHERE 1=1 order by brgy_code asc");

            if (strTmpDistrictCode == "00" && strDistrictCode == "")
            {
                m_strDistrictCode = "";
            }
            else if (strTmpDistrictCode == "00")
            {
                m_strDistrictCode = strDistrictCode;
                strQuery.Append(string.Format(" AND dist_code = '{0}'", strDistrictCode));
            }
            else
            {
                m_strDistrictCode = strTmpDistrictCode;
                strQuery.Append(string.Format(" AND dist_code = '{0}'", strTmpDistrictCode));
            }
            if (blnIsOrderByName)
                strQuery.Append(" ORDER BY dist_code, brgy_nm");
            else
                strQuery.Append(" ORDER BY dist_code, brgy_code");
            result.Query = strQuery.ToString();
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_lstDistrictCodes.Add(result.GetString("dist_code").Trim());
                    m_lstBarangayCodes.Add(result.GetString("brgy_code").Trim());
                    m_lstBarangayNames.Add(result.GetString("brgy_nm").Trim());
                }
            }

            result.Close();
        }

        public List<string> DistrictCodes
        {
            get { return m_lstDistrictCodes; } 
        }

        public List<string> BarangayCodes
        {
            get { return m_lstBarangayCodes; }
        }

        public List<string> BarangayNames
        {
            get { return m_lstBarangayNames; }
        }

        public string DistrictCode
        {
            get { return m_strDistrictCode; }
        }

        public string GetBarangayName(string strDistrictCode, string strBarangayCode)
        {
            string strBarangayName = string.Empty;
            for (int i = 0; i < m_lstBarangayCodes.Count; i++)
            {
                if (m_lstDistrictCodes[i] == strDistrictCode &&
                    m_lstBarangayCodes[i] == strBarangayCode)
                {
                    strBarangayName = m_lstBarangayNames[i];
                    break;
                }
            }
            return strBarangayName;
        }

        public string GetBarangayCode(string strDistrictCode, string strBarangayName)
        {
            string strBarangayCode = string.Empty;
            for (int i = 0; i < m_lstDistrictCodes.Count; i++)
            {
                if (m_lstDistrictCodes[i] == strDistrictCode &&
                    m_lstBarangayNames[i] == strBarangayName)
                {
                    strBarangayCode = m_lstBarangayCodes[i];
                    break;
                }
            }
            return strBarangayCode;
        }

        public string GetBarangayCode(int intIndex)
        {
            if (m_lstBarangayCodes.Count == 1 && intIndex == 0)
                return m_lstBarangayCodes[0];
            else if (intIndex > 0 && intIndex < m_lstDistrictCodes.Count)
                return m_lstBarangayCodes[intIndex];
            return string.Empty;

        }
    }
}
