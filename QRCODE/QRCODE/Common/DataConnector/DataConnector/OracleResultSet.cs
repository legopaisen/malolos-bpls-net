////////////////////

// RMC 20111206 create blob connection
// RMC 20110908 create GIS connection

////////////////////


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using System.Windows.Forms;

namespace Amellar.Common.DataConnector
{
    /// <summary>
    /// This class encapsulates 
    /// <remarks>
    /// Will later add transaction support
    /// </remarks>
    /// </summary>
    public class OracleResultSet
    {
        protected OracleCommand m_cmdCommand;
        protected OracleDataReader m_drDataReader;

        private string m_strErrorDescription;

        private OracleTransaction m_trTransaction;
        private bool m_blnHasTransaction;
        private bool m_blnIsRpadIgnore;  //RDO 062408 
        OracleDataConnector objOracleDataConn;  // RMC 20110908 create GIS connection
        
        //JVL added this class inside the OracleResult class(s)
        public OracleResultSet(int intBlob)
        {
            if (intBlob == 1)
                this.CreateInstance(DataConnectorManager.Instance.BlobConnection);
            m_blnHasTransaction = false;
            m_blnIsRpadIgnore = false; //RDO 062408 
            //m_blnIsRpadIgnore = true;  //RDO 062408 
        }
        //JVL (e)


        public OracleResultSet()
        {
            this.CreateInstance(DataConnectorManager.Instance.Connection);
            m_blnHasTransaction = false;
            m_blnIsRpadIgnore = false;  //RDO 062408 
            //m_blnIsRpadIgnore = true;  //RDO 062408 
        }

               
        public OracleResultSet(OracleConnection cncConnection)
        {
            this.CreateInstance(cncConnection);
        }

        public OracleResultSet(OracleConnection cncConnection, string strQuery)
        {
            this.CreateInstance(cncConnection);
            m_cmdCommand.CommandText = strQuery;
            //m_strQuery = strQuery;
        }

        public OracleResultSet(string strQuery)
        {
            this.CreateInstance(DataConnectorManager.Instance.Connection);
            m_cmdCommand.CommandText = strQuery;
            //m_strQuery = strQuery;
        }

        public bool IgnoreRpad
        {
            get { return m_blnIsRpadIgnore; }
            set { m_blnIsRpadIgnore = value; }
        }

        public bool Transaction
        {
            get { return m_blnHasTransaction; }
            set
            {
                m_blnHasTransaction = value;
                if (m_blnHasTransaction)
                {
                    try
                    {
                        m_trTransaction = DataConnectorManager.Instance.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    }
                    catch
                    {
                        m_blnHasTransaction = false;
                    }
                }
                else
                {
                    try
                    {
                        m_trTransaction.Dispose();
                    }
                    catch { }
                }
            }
        }

        public void Close()
        {
            //not yet tested
            if (m_blnHasTransaction)
            {
                try
                {
                    m_trTransaction.Dispose();
                }
                catch { }
            }
            try
            {
                m_drDataReader.Dispose();
            }
            catch { }
            try
            {
                m_cmdCommand.Dispose();
            }
            catch { }
            DataConnectorManager.Instance.CloseConnection();
            GC.Collect(); // CJC 20130628
        }

        ~OracleResultSet()
        {
            this.Close();
        }

        public void CreateInstance(OracleConnection cncConnection)
        {
            m_cmdCommand = new OracleCommand();
            m_cmdCommand.Connection = cncConnection;
          
        }

 
        public string ErrorDescription
        {
            get { return m_strErrorDescription; }
        }

        /// <summary>
        /// Property that sets query retaining values of parameters
        /// </summary>
        public string QueryText
        {
            set { m_cmdCommand.CommandText = value; }
        }


        public string Query
        {
            get 
            {
                return m_cmdCommand.CommandText;
                //return m_strQuery; 
            }
            set
            {
                m_cmdCommand.Parameters.Clear();
                //m_strQuery = value;
                //m_cmdCommand.CommandText = m_strQuery;
                string strQuery = value;
                if (m_blnIsRpadIgnore)
                {
                    int intIndex1 = 0;
                    int intIndex2 = 0;
                    int intIndex3 = 0;
                    while ((intIndex1 = strQuery.ToUpper().IndexOf(" RPAD(")) != -1)
                    {
                        intIndex2 = strQuery.IndexOf(",", intIndex1);
                        if (intIndex2 == -1)
                            break;
                        intIndex3 = strQuery.IndexOf(")", intIndex2);
                        if (intIndex3 == -1)
                            break;

                        strQuery = string.Format("{0} {1} {2}", strQuery.Substring(0, intIndex1),
                            strQuery.Substring(intIndex1 + 6, intIndex2 - intIndex1 - 6), strQuery.Substring(intIndex3 + 1));
                    }
                }
                m_cmdCommand.CommandText = strQuery;


            }
        }

        public bool Read()
        {
                return m_drDataReader.Read();
        }

        //public void AddParameter
        //char, date, double, number, boolean
        //should catch invalid casting or invalid column id
        public int GetOrdinal(string strColumn)
        {
            return m_drDataReader.GetOrdinal(strColumn);
        }


        public char GetChar(string strColumn)
        {
            return this.GetChar(this.GetOrdinal(strColumn));
        }
        
        public char GetChar(int intColumn)
        {
            return m_drDataReader.GetChar(intColumn);
        }

        public decimal GetDecimal(string strColumn)
        {
            return this.GetDecimal(this.GetOrdinal(strColumn));
        }

        public decimal GetDecimal(int intColumn)
        {
            return m_drDataReader.GetDecimal(intColumn);
        }

        public int GetInt(string strColumn)
        {
            return this.GetInt(this.GetOrdinal(strColumn));
        }
        
        public int GetInt(int intColumn)
        {
            // RMC 20110311 added try & catch
            try
            {
                return m_drDataReader.GetOracleDecimal(intColumn).ToInt32();
            }
            catch
            {
                return 0;
            }
        }

        public DateTime GetDateTime(string strColumn)
        {
            try
            {
                return this.GetDateTime(this.GetOrdinal(strColumn));
            }
            catch (System.Exception ex)
            {
                return DateTime.Now;
            }
            
        }

        public DateTime GetDateTime(int intColumn)
        {
             return m_drDataReader.GetDateTime(intColumn);
        }

        public bool GetBoolean(string strColumn)
        {
            return this.GetBoolean(this.GetOrdinal(strColumn));
        }
        public bool GetBoolean(int intColumn)
        {
            return m_drDataReader.GetBoolean(intColumn);
        }
        
        public double GetFloat(string strColumn)
        {
            return this.GetFloat(this.GetOrdinal(strColumn));
        }

        public double GetFloat(int intColumn)
        {
            return m_drDataReader.GetOracleDecimal(intColumn).ToSingle();
        }

        public double GetDouble(string strColumn)
        {
            return this.GetDouble(this.GetOrdinal(strColumn));
        }

        public double GetDouble(int intColumn)
        {
            // RMC 20110311 added try & catch
            try
            {
                return m_drDataReader.GetOracleDecimal(intColumn).ToDouble();
            }
            catch
            {
                return 0;
            }
        }

        public string GetString(string strColumn)
        {
            return this.GetString(this.GetOrdinal(strColumn));
        }

        public string GetString(int intColumn)
        {
            try
            {
                return m_drDataReader.GetString(intColumn);
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// JVL 20080513 blob inmplementation
        /// </summary>
        /// <param name="strColumn"></param>
        /// <returns></returns>
        public byte[] GetBlob(string strColumn)
        {
            return this.GetBlob(this.GetOrdinal(strColumn));
        }

        /// <summary>
        /// JVL 20080513 blob inmplementation
        /// </summary>
        /// <param name="intColunm"></param>
        /// <returns></returns>
        public byte[] GetBlob(int intColunm)
        {
            try { return m_drDataReader.GetOracleBlob(intColunm).Value; }
            catch { return null; }
        }



        /*
        public long GetBytes(int intColumn, long lngFieldOffset, byte[] bytBuffer, 
            int intBufferOffset, int intLength)
        {
            return m_drDataReader.GetBytes(intColumn, lngFieldOffset, bytBuffer, intBufferOffset,
                intLength);
        }
         */

        //parameter passing should include Direction as well

        public void AddParameter(string strParameterName, object obj)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = strParameterName;
            param.Value = obj;
            m_cmdCommand.Parameters.Add(param);
        }

        public void AddParameter(string strParameterName, object obj, int intSize)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = strParameterName;
            param.Value = obj;
            param.Size = intSize;
            m_cmdCommand.Parameters.Add(param);
        }

        /*
        public void AddIntParameter(string strParameterName, int intValue)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = strParameterName;
            param.OracleDbType = OracleDbType.Int32;
            param.Value = intValue;
            m_cmdCommand.Parameters.Add(param);
        }

        public void AddStringParameter(string strParameterName, int intSize, string strValue)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = strParameterName;
            param.OracleDbType = OracleDbType.Varchar2;
            param.Value = strValue;
            param.Size = intSize;
            m_cmdCommand.Parameters.Add(param);
        }

        public void AddStringParameter(string strParameterName, string strValue)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = strParameterName;
            param.OracleDbType = OracleDbType.Varchar2;
            param.Value = strValue;
            m_cmdCommand.Parameters.Add(param);
        }
        */

        /// <summary>
        /// This method returns scalar
        /// </summary>
        /// <returns></returns>
        public string ExecuteScalar()
        {
            try
            {
                return m_cmdCommand.ExecuteScalar().ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool Commit()
        {
            if (m_blnHasTransaction)
            {
                try
                {
                    m_trTransaction.Commit();
                }
                catch (Exception e)
                {
                    m_strErrorDescription = e.Message;
                    return false;
                }
            }
            return true;
        }

        public bool Rollback()
        {
            if (m_blnHasTransaction)
            {
                try
                {
                    m_trTransaction.Rollback();
                }
                catch (Exception e)
                {
                    m_strErrorDescription = e.Message;
                    return false;
                }
            }
            return true;
        }

        public string QueryToString()
        {
            string strQuery = string.Empty;
            string strKey = string.Empty;
            string strValue = string.Empty;
            strQuery = m_cmdCommand.CommandText;

            for (int i = m_cmdCommand.Parameters.Count - 1; i >= 0; i--)
            {
                strKey = m_cmdCommand.Parameters[i].ParameterName;
                strValue = string.Empty;
                if (m_cmdCommand.Parameters[i].Value.GetType() == typeof(DateTime))
                    strValue = string.Format("TO_DATE('{0:MM/dd/yyyy}', 'MM/dd/yyyy')", (DateTime)m_cmdCommand.Parameters[i].Value);
                else if (m_cmdCommand.Parameters[i].Value.GetType() == typeof(float))
                    strValue = string.Format("{0:0.00}", (float) m_cmdCommand.Parameters[i].Value);
                else if (m_cmdCommand.Parameters[i].Value.GetType() == typeof(double)) 
                    strValue = string.Format("{0:0.00}", (double) m_cmdCommand.Parameters[i].Value);
                else if (m_cmdCommand.Parameters[i].Value.GetType() == typeof(int))
                    strValue = string.Format("{0}", (int)m_cmdCommand.Parameters[i].Value);
                else //if (m_cmdCommand.Parameters[i].Value.GetType() == typeof(string))
                    strValue = string.Format("'{0}'", m_cmdCommand.Parameters[i].Value.ToString());
                strQuery = strQuery.Replace(strKey, strValue);
            }

            return strQuery;
        }

        public int ExecuteNonQuery()
        {
            return m_cmdCommand.ExecuteNonQuery();
        }

        public bool Execute()
        {
            try
            {
                m_drDataReader = m_cmdCommand.ExecuteReader();
            }
            catch (OracleException ex)
            {
               m_strErrorDescription = ex.Message.ToString();
               MessageBox.Show(m_strErrorDescription); // ALJ 20090701 Prompt error message and exit application
               Application.Exit(); // ALJ 20090701 Prompt error message and exit application
               return false;
            }
            //catch (Exception ex)


            return true;
        }

        // RMC 20110908 create GIS connection (s)
        public void CreateNewConnection()
        {
            int intPort = 0;
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["GisPort"], out intPort);
            objOracleDataConn = new OracleDataConnector(
                       System.Configuration.ConfigurationManager.AppSettings["GisHost"],
                       intPort,
                       System.Configuration.ConfigurationManager.AppSettings["GisServiceName"],
                       System.Configuration.ConfigurationManager.AppSettings["GisUserId"],
                       System.Configuration.ConfigurationManager.AppSettings["GisPassword"]
                                            );

            objOracleDataConn.OpenConnection();

            this.CreateInstance(objOracleDataConn.Connection);
        }
        // RMC 20110908 create GIS connection (e)

        // RMC 20111206 create blob connection (s)
        public void CreateBlobConnection()
        {
            int intPort = 0;
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["BlobPort"], out intPort);
            objOracleDataConn = new OracleDataConnector(
                       System.Configuration.ConfigurationManager.AppSettings["BlobHost"],
                       intPort,
                       System.Configuration.ConfigurationManager.AppSettings["BlobServiceName"],
                       System.Configuration.ConfigurationManager.AppSettings["BlobUserId"],
                       System.Configuration.ConfigurationManager.AppSettings["BlobPassword"]
                                            );

            objOracleDataConn.OpenConnection();

            this.CreateInstance(objOracleDataConn.Connection);
        }
        // RMC 20111206 create blob connection (e)

        // GDE create RPTA connection
        public void CreateRPTAConnection()
        {
            int intPort = 0;
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["RptaPort"], out intPort);
            objOracleDataConn = new OracleDataConnector(
                       System.Configuration.ConfigurationManager.AppSettings["RptaHost"],
                       intPort,
                       System.Configuration.ConfigurationManager.AppSettings["RptaServiceName"],
                       System.Configuration.ConfigurationManager.AppSettings["RptaUserId"],
                       System.Configuration.ConfigurationManager.AppSettings["RptaPassword"]
                                            );

            objOracleDataConn.OpenConnection();

            this.CreateInstance(objOracleDataConn.Connection);
        }

    }
}
