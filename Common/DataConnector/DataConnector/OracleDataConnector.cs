using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Amellar.Common.EncryptUtilities;

namespace Amellar.Common.DataConnector
{
    class OracleDataConnector:DataConnector
    {
        private const string c_strConnectionStringFormatTns = "Data Source={0};User Id={1};Password={2};{3}";
        private const string c_strConnectionStringFormat = "Data Source=(DESCRIPTION="
             + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))"
             + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));"
             + "User Id={3};Password={4};{5}";

        private string m_strAlias;
        private string m_strUserId;
        private string m_strPassword;
        private string m_strHost;
        private int m_intPort;
        private string m_strServiceName;

        //additional settings for connection pooling, connection timeout, etc.
        private string m_strAdditionalAttributes; 

        private bool m_blnIsAlias;

        private OracleConnection m_cnConnection;

        public OracleDataConnector()
        {
            this.AdditionalAttributes = "";
        }

        public OracleDataConnector(string strAlias, string strUserId, string strPassword)
        {
            this.Alias = strAlias;
            this.UserId = strUserId;
            this.Password = strPassword;
            this.Host = "";
            this.Port = 1521;
            this.ServiceName = "";
            m_blnIsAlias = true;
            this.AdditionalAttributes = "";            
        }

        public OracleDataConnector(string strHost, int intPort, string strServiceName, string strUserId, string strPassword)
        {
            this.Host = strHost;
            this.Port = intPort;
            this.ServiceName = strServiceName;
            this.UserId = strUserId;
            this.Password = strPassword;
            this.Alias = "";
            m_blnIsAlias = false;
            this.AdditionalAttributes = "";            
        }

        ~OracleDataConnector()
        {
            CloseConnection();
        }

        public OracleConnection Connection
        {
            get { return m_cnConnection; }
        }

        public string Alias
        {
            set
            {
                m_blnIsAlias = true;
                m_strAlias = value;
            }
        }

        public string UserId
        {
            set 
            { 
                m_strUserId = value; 
            }
        }

        public string Password
        {
            set 
            { 
                m_strPassword = value;
            }
        }

        public string Host
        {
            set
            {
                m_blnIsAlias = false;
                m_strHost = value;
            }
        }

        public int Port
        {
            set
            {
                m_blnIsAlias = false;
                m_intPort = value;
            }
        }

        public string ServiceName
        {
            set
            {
                m_blnIsAlias = false;
                m_strServiceName = value;
            }
        }

        public string AdditionalAttributes
        {
            set
            {
                m_strAdditionalAttributes = value;
            }
        }

        public override bool OpenConnection()
        {
            if (m_strUserId.Substring(m_strUserId.Length - 1) == "t")
            {
                System.Windows.Forms.MessageBox.Show("You are connected in test database!");
            }

            Encryption decrypt = new Encryption();
            if (m_blnIsAlias)
            {
                m_strConnectionString = string.Format(c_strConnectionStringFormatTns,
                    m_strAlias, m_strUserId, decrypt.DecryptString(m_strPassword), m_strAdditionalAttributes);
                    //m_strPassword, m_strAdditionalAttributes);
            }
            else
            {
                m_strConnectionString = string.Format(c_strConnectionStringFormat,
                    m_strHost, m_intPort, m_strServiceName,
                    m_strUserId, decrypt.DecryptString(m_strPassword), m_strAdditionalAttributes);
                    //m_strPassword, m_strAdditionalAttributes);
            }
            // alj (s)
            /*
            string str, encript;
            str = "baM40";
            encript = decrypt.EncryptString(str);
            str = "baT24";
            encript = decrypt.EncryptString(str);
             */
            // alj (e)
            try
            {
                if (m_cnConnection != null)
                {
                    try
                    {
                        m_cnConnection.Close();
                    }
                    catch { }
                    try
                    {
                        m_cnConnection.Dispose();
                    }
                    catch { }

                }
                m_cnConnection = new OracleConnection();
                m_cnConnection.ConnectionString = m_strConnectionString;

                m_cnConnection.Open();
            }
            catch (OracleException ex)
            {
                if (ex != null)
                {
                    switch (ex.Number)
                    {
                        case 12514:
                        case 12154:
                            m_strErrorDescription = "Database is unvailable.";
                            break;
                        default:
                            m_strErrorDescription = "Database error: " + ex.Message.ToString();
                            break;
                    }
                }
                return false;
            }
            catch (Exception ex) //not really necessary
            {
                m_strErrorDescription = ex.Message.ToString();
                return false;
            }
            
            return true;
        }

        public override bool CloseConnection()
        {
            if (m_cnConnection == null)
            {
                return true;
            }
            try
            {
                m_cnConnection.Close();
                m_cnConnection.Dispose();
            }
            catch (Exception ex)
            {
                m_strErrorDescription = ex.Message.ToString();
                return false;
            }
            return true;
        }

    }
}
