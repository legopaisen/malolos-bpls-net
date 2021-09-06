using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace Amellar.Common.AppSettings
{
    public class SystemUser:User
    {
        private string m_strPosition;
        private string m_strMemo;
        private string m_strSystemCode;

        public SystemUser()
        {
            this.Clear();
        }

        /// <summary>
        /// ALJ 20100119 load sys user's details for BPLS
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <returns></returns>
        public bool Load(string strUserCode)
        {
            this.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("SELECT usr_ln, usr_fn, usr_mi, usr_pos FROM sys_users WHERE trim(usr_code) = '{0}'", //JARS 20170614 ADDED TRIM()
                StringUtilities.StringUtilities.HandleApostrophe(strUserCode)); // RMC 20110725 added handleapostrophe
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_strUserCode = strUserCode;
                    m_strLastName = result.GetString("usr_ln").Trim();
                    m_strFirstName = result.GetString("usr_fn").Trim();
                    m_strMI = result.GetString("usr_mi").Trim();
                    m_strPosition = result.GetString("usr_pos").Trim();
                    return true;
                }
            }
            return false;
        }

        public bool Load(string strUserCode, string strSystemCode)
        {
            this.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("SELECT usr_ln, usr_fn, usr_mi, usr_pos, usr_memo FROM sys_users WHERE trim(usr_code) = '{0}' AND sys_code = '{1}'",
                StringUtilities.StringUtilities.HandleApostrophe(strUserCode), strSystemCode);  // RMC 20110725 added handleapostrophe
            /*
            result.Query = "SELECT usr_ln, usr_fn, usr_mi, usr_pos, usr_memo FROM sys_users WHERE usr_code = RPAD(:1, 20) AND sys_code = :2";
            result.AddParameter(":1", strUserCode);
            result.AddParameter(":2", strSystemCode);
            */
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_strUserCode = strUserCode;
                    m_strLastName = result.GetString("usr_ln").Trim();
                    m_strFirstName = result.GetString("usr_fn").Trim();
                    m_strMI = result.GetString("usr_mi").Trim();
                    m_strPosition = result.GetString("usr_pos").Trim();
                    m_strMemo = result.GetString("usr_memo").Trim();
                    //m_strPassword = result.GetString("usr_pwd").Trim();
                    m_strSystemCode = strSystemCode;
                    return true;
                }
            }
            return false;
        }

        public string Position
        {
            get { return m_strPosition; }
        }

        public bool Authenticate(string strPassword)
        {
            Amellar.Common.EncryptUtilities.Encrypt enc = new Amellar.Common.EncryptUtilities.Encrypt();
            string strEncrypted = enc.EncryptPassword(strPassword);


            if (strPassword != string.Empty)
            {
                OracleResultSet result = new OracleResultSet();
                //use trim to capture ñ character //JVL mal
                result.Query = "SELECT COUNT(*) FROM sys_users WHERE trim(usr_code) = :1 AND sys_code = :2 AND trim(usr_pwd) = :3";
                result.AddParameter(":1", StringUtilities.StringUtilities.HandleApostrophe(m_strUserCode)); // RMC 20110725 added handleapostrophe
                result.AddParameter(":2", m_strSystemCode);

                result.AddParameter(":3", StringUtilities.StringUtilities.HandleApostrophe(strPassword)); // RMC 20110725 added handleapostrophe
                //result.AddParameter(":3", strEncrypted);

                int intCount = 0;
                int.TryParse(result.ExecuteScalar().ToString(), out intCount);
                if (intCount != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override void Clear()
        {
            base.Clear();
            m_strPosition = string.Empty;
            m_strMemo = string.Empty;
            m_strSystemCode = string.Empty;
        }
    }
}
