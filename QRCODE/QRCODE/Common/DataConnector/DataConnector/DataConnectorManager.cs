using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using Oracle.DataAccess.Client;

namespace Amellar.Common.DataConnector
{
    ///<summary>
    ///This singleton class handles database connection for entire application.
    ///</summary>
    ///<remarks>
    ///used singleton pattern - fourth version as suggested (thread-safe)
    ///see http://www.yoda.arachsys.com/csharp/singleton.html 
    ///</remarks>
    public sealed class DataConnectorManager
    {
        static readonly DataConnectorManager instance = new DataConnectorManager();

        //temporarily locked to oracle connection only
        //should use factory pattern
        //private static DataConnector m_objDataConnector;
        private static OracleDataConnector m_objDataConnector;

        //sets connection as null when CloseConnection function is called
        private static bool m_blnIsConnectionAlwaysOpen;

        private static OracleDataConnector m_objDataBlobConnector; //JVL added blob connection


        DataConnectorManager()
        {
            m_blnIsConnectionAlwaysOpen = true;
        }

        private void CreateInstance()
        {
            m_objDataConnector = new OracleDataConnector();
            
            m_objDataConnector.Host = ConfigurationManager.AppSettings["Host"];
            int intPort = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["Port"], out intPort))
            {
                m_objDataConnector.Port = intPort;
            }
            m_objDataConnector.ServiceName = ConfigurationManager.AppSettings["ServiceName"];
            m_objDataConnector.UserId = ConfigurationManager.AppSettings["UserId"];
            m_objDataConnector.Password = ConfigurationManager.AppSettings["Password"];
        }

        public bool IsConnectionAlwaysOpen
        {
            get { return m_blnIsConnectionAlwaysOpen; }
            set { m_blnIsConnectionAlwaysOpen = value; }
        }

        public bool OpenConnection()
        {
            if (m_objDataConnector == null)
            {
                CreateInstance();
            }
            return m_objDataConnector.OpenConnection();
        }

        public string ErrorDescription
        {
            get
            {
                if (m_objDataConnector != null)
                {
                    return m_objDataConnector.ErrorDescription;
                }
                return String.Empty;
            }
        }

        public bool CloseConnection()
        {
            if (m_objDataConnector != null)
            {
                if (!m_blnIsConnectionAlwaysOpen)
                {
                    if (m_objDataConnector.CloseConnection())
                    {
                        //if (!m_blnIsConnectionAlwaysOpen)
                        //{
                        m_objDataConnector = null;
                        //}
                    }
                }
                return true;
            }
            return false;
        }

        /*
        public OracleDataConnector DataConnector
        {
            get
            {
                if (m_objDataConnector == null)
                {
                    CreateInstance();
                }
                return m_objDataConnector;
            }
        }
        */

        public OracleConnection Connection
        {
            get
            {
                if (m_objDataConnector == null)
                {
                    CreateInstance();
                    m_objDataConnector.OpenConnection(); //fix
                }
                return m_objDataConnector.Connection;
            }
        }


        #region //JVL added this method for Blob
        //JVL (s)
        /// <summary>
        /// 
        /// </summary>
        public OracleConnection BlobConnection
        {
            get
            {
                if (m_objDataBlobConnector == null)
                {
                    BlobCreateInstance();
                    m_objDataBlobConnector.OpenConnection(); //fix
                }
                return m_objDataBlobConnector.Connection;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void BlobCreateInstance()
        {
            m_objDataBlobConnector = new OracleDataConnector();
            //m_objDataConnector = new OracleDataConnector();
            //m_objDataBlobConnector = m_objDataConnector;

            m_objDataBlobConnector.Host = ConfigurationManager.AppSettings["BlobHost"];
            int intPort = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["BlobPort"], out intPort))
            {
                m_objDataBlobConnector.Port = intPort;
            }
            m_objDataBlobConnector.ServiceName = ConfigurationManager.AppSettings["BlobServiceName"];
            m_objDataBlobConnector.UserId = ConfigurationManager.AppSettings["BlobUserId"];
            m_objDataBlobConnector.Password = ConfigurationManager.AppSettings["BlobPassword"];
        }

        // JVL (e)
        #endregion



        public static DataConnectorManager Instance
        {
            get
            {
                return instance;
            }
        }

        
    }
}
