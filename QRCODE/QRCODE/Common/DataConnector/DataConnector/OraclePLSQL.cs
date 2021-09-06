using System;
using System.Collections.Generic;
using System.Text;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Amellar.Common.DataConnector
{
    public class OraclePLSQL
    {

        private OracleTransaction m_plsqlTrTransaction;
        private List<OracleParameter> m_paramList;
        private OracleParameter m_oracParam;
        private OracleCommand m_oracCommand;
        private OracleDataReader m_oracDataReader;
        public string m_strError;
        private bool m_isTransactional;
        List<int> intRefCursor;


        public enum OracleDbTypes
        {
            Blob, Varchar2, Int32, Double, Date
        }

        //public OracleDbTypes OracleTypes
        // {
        //     get { return oracTypes; }

        //}
        OracleDbTypes oracTypes;




        public OraclePLSQL()
        {
            oracTypes = new OracleDbTypes();

            m_isTransactional = false;
            m_paramList = new List<OracleParameter>();
            m_strProcName = string.Empty;
            objParamValue = null;
            intRefCursor = new List<int>();
        }



        public string ProcedureName
        {
            get { return m_strProcName; }
            set { m_strProcName = value; }
        }
        private string m_strProcName;

        public void AddParameter(string paramCode, OracleDbTypes oracTypes, System.Data.ParameterDirection paramDirection)
        {

            int size = -1;
            this.AddParameter(paramCode, oracTypes, paramDirection, size);
        }




        /// <summary>
        /// optional property is ParamSize, required if input parameter ParamValue
        /// </summary>
        /// <param name="paramCode"></param>
        /// <param name="?"></param>
        /// 
        public void AddParameter(string paramCode, OracleDbTypes oracTypes, System.Data.ParameterDirection paramDirection, int size)
        {
            OracleDbType oracType = new OracleDbType();
            switch (oracTypes)
            {
                case OracleDbTypes.Blob:
                    { oracType = OracleDbType.Blob; break; }
                case OracleDbTypes.Varchar2:
                    { oracType = OracleDbType.Varchar2; break; }
                case OracleDbTypes.Double:
                    { oracType = OracleDbType.Double; break; }
                case OracleDbTypes.Int32:
                    { oracType = OracleDbType.Int32; break; }
                case OracleDbTypes.Date:
                    { oracType = OracleDbType.Date; break; }
                default:
                    { System.Windows.Forms.MessageBox.Show("Oracle Db types needs to add!!!"); break; }
            }

            m_oracCommand = new OracleCommand(this.ProcedureName, DataConnectorManager.Instance.Connection);
            m_oracCommand.CommandType = System.Data.CommandType.StoredProcedure;

            m_oracParam = new OracleParameter(paramCode, oracType);
            m_oracParam.Direction = paramDirection;

            if (size != -1)
                m_oracParam.Size = size;

            if (paramDirection == System.Data.ParameterDirection.Input)
            {
                m_oracParam.Value = this.ParamValue;
            }
            else if (paramDirection == System.Data.ParameterDirection.Output)
            {
                if (oracType == OracleDbType.RefCursor)
                {
                    intRefCursor.Add(m_paramList.Count);
                }
                else
                {

                }

            }
            else if (paramDirection == System.Data.ParameterDirection.ReturnValue)
            { }
            else if (paramDirection == System.Data.ParameterDirection.InputOutput)
            { }

            m_paramList.Add(m_oracParam);


            //            m_oracCommand.Parameters.Add(m_oracParam);
            //     m_oracCommand.ExecuteNonQuery();
            //   m_oracDataReader = ((OracleRefCursor)m_oracParam.Value).GetDataReader();
            //            while (m_oracDataReader.Read())
            //            {
            //                string ths = string.Format("Last Name : {0}", m_oracDataReader[0].ToString());
            //                string rf = string.Format("First Name: {0}", m_oracDataReader[1].ToString());
            //            }


        }

        public void Close()
        {

            if (m_isTransactional)
            {
                try
                {
                    m_plsqlTrTransaction.Dispose();
                }
                catch { }
            }
            try
            {
                m_oracDataReader.Dispose();
            }
            catch { }
            try
            {
                m_oracCommand.Dispose();
            }
            catch { }


            DataConnectorManager.Instance.CloseConnection();
        }

        ~OraclePLSQL()
        {
            this.Close();
        }


        public void ClearParameter()
        {
            m_paramList.Clear();
            intRefCursor.Clear();
        }



        public int ExecuteNonQuery()
        {
            for (int x = 0; x <= m_paramList.Count - 1; x++)
            {
                m_oracCommand.Parameters.Add(m_paramList[x]);
            }
            // m_paramList.Clear();
            return m_oracCommand.ExecuteNonQuery();
        }



        public bool Execute()
        {
            try
            {
                for (int x = 0; x <= intRefCursor.Count + 1; x++)
                {
                    if (intRefCursor.Count > x)
                    {
                        try
                        {
                            m_oracDataReader = ((OracleRefCursor)m_paramList[intRefCursor[x]].Value).GetDataReader();
                            intRefCursor.Remove(intRefCursor[x]);
                            break;
                            // intRefCursor.Remove(x);
                            // m_oracDataReader = m_oracCommand.ExecuteReader();
                        }
                        catch
                        { }
                    }
                }
            }
            catch (OracleException ex)
            {
                m_strError = ex.Message.ToString();
                return false;
            }
            return true;
        }

        public bool Read()
        {
            return m_oracDataReader.Read();
        }

        public object ParamValue
        {
            get { return objParamValue; }
            set { objParamValue = value; }
        }
        private object objParamValue;




        /// <summary>
        /// Returned parameter as object
        /// </summary>
        public object ReturnValue(int intParamIndex)
        {
            //m_oracCommand.Parameters.Add(m_paramList[x].ParameterName);
            return m_oracCommand.Parameters[m_paramList[intParamIndex].ParameterName].Value;

            //   get {return m_oracCommand.Parameters[m_ParamCode].Value; }
        }

        public object ReturnValue(string strName)
        {
            /*
                       if (this.GetParamIndex(strName) == -1)
                       {
                           m_strError = "Can\'t continue no output index.";
                           return m_strError;
                       }
                       else*/
            return m_oracCommand.Parameters[strName].Value;
        }

        private int GetParamIndex(string strName)
        {
            for (int x = 0; x <= m_paramList.Count - 1; x++)
            {
                if (m_paramList[x].ParameterName == strName)
                {
                    return x;
                }
            }
            return -1;
        }







        #region // This code is same as OracleResultSet Class so it needs to be merge

        public bool Commit()
        {
            if (m_isTransactional)
            {
                try
                {
                    m_plsqlTrTransaction.Commit();
                }
                catch (Exception e)
                {
                    m_strError = e.Message;
                    return false;
                }
            }
            return true;
        }

        public bool Rollback()
        {
            if (m_isTransactional)
            {
                try
                {
                    m_plsqlTrTransaction.Rollback();
                }
                catch (Exception e)
                {
                    m_strError = e.Message;
                    return false;
                }
            }
            return true;
        }
        /*
        public bool Read()
        {
            try
            {
                return m_oracDataReader.Read();
            }
            catch (OracleException ex)
            { 
                m_strError = ex.Message;
                return false;
            }

        }
        */

        public bool Transaction
        {
            get { return m_isTransactional; }
            set
            {
                m_isTransactional = value;
                if (m_isTransactional)
                {
                    try
                    {
                        m_plsqlTrTransaction = DataConnectorManager.Instance.Connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    }
                    catch
                    {
                        m_isTransactional = false;
                    }
                }
                else
                {
                    try
                    {
                        m_plsqlTrTransaction.Dispose();
                    }
                    catch { }
                }
            }
        }


        public string Error
        {
            get { return m_strError; }
        }

        ////////////
        //public void AddParameter
        //char, date, double, number, boolean
        //should catch invalid casting or invalid column id
        public int GetOrdinal(string strColumn)
        {
            return m_oracDataReader.GetOrdinal(strColumn);
        }


        public char GetChar(string strColumn)
        {
            return this.GetChar(this.GetOrdinal(strColumn));
        }

        public char GetChar(int intColumn)
        {
            return m_oracDataReader.GetChar(intColumn);
        }

        public decimal GetDecimal(string strColumn)
        {
            return this.GetDecimal(this.GetOrdinal(strColumn));
        }

        public decimal GetDecimal(int intColumn)
        {
            return m_oracDataReader.GetDecimal(intColumn);
        }

        public int GetInt(string strColumn)
        {
            return this.GetInt(this.GetOrdinal(strColumn));
        }

        public int GetInt(int intColumn)
        {
            return m_oracDataReader.GetOracleDecimal(intColumn).ToInt32();
        }

        public DateTime GetDateTime(string strColumn)
        {
            return this.GetDateTime(this.GetOrdinal(strColumn));
        }

        public DateTime GetDateTime(int intColumn)
        {
            return m_oracDataReader.GetDateTime(intColumn);
        }

        public bool GetBoolean(string strColumn)
        {
            return this.GetBoolean(this.GetOrdinal(strColumn));
        }
        public bool GetBoolean(int intColumn)
        {
            return m_oracDataReader.GetBoolean(intColumn);
        }

        public double GetFloat(string strColumn)
        {
            return this.GetFloat(this.GetOrdinal(strColumn));
        }

        public double GetFloat(int intColumn)
        {
            return m_oracDataReader.GetOracleDecimal(intColumn).ToSingle();
        }

        public double GetDouble(string strColumn)
        {
            return this.GetDouble(this.GetOrdinal(strColumn));
        }

        public double GetDouble(int intColumn)
        {
            return m_oracDataReader.GetOracleDecimal(intColumn).ToDouble();
        }

        public string GetString(string strColumn)
        {
            return this.GetString(this.GetOrdinal(strColumn));
        }

        public string GetString(int intColumn)
        {
            try
            {
                return m_oracDataReader.GetString(intColumn);
            }
            catch
            {
                return string.Empty;
            }
        }


        #endregion



    }
}
