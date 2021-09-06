using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.AppSettings
{
    public class User
    {
        protected string m_strUserCode;
        protected string m_strLastName;
        protected string m_strFirstName;
        protected string m_strMI;

        private string m_strPassword;

        public User()
        {
            this.Clear();
        }

        public User(string strUserCode, string strLastName, string strFirstName,
            string strMI)
        {
            this.Clear();
            m_strUserCode = strUserCode;
            m_strLastName = strLastName;
            m_strFirstName = strFirstName;
            m_strMI = strMI;
        }


        public virtual void Clear()
        {
            m_strUserCode = string.Empty;
            m_strLastName = string.Empty;
            m_strFirstName = string.Empty;
            m_strMI = string.Empty;

            m_strPassword = string.Empty;
        }

        public string UserCode
        {
            set { m_strUserCode = value; }
            get { return m_strUserCode; }
        }

        public bool IsEmpty
        {
            get { return (m_strUserCode == string.Empty); }
        }

        public string UserName
        {
            // ALJ 20100119 enable this function
            get
            {
                return StringUtilities.PersonName.ToPersonName(m_strLastName, m_strFirstName,
                    m_strMI, "L", "F L", "F M. L");
            }
        }
         

    }
}
