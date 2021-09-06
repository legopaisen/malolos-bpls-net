using System;
using System.Collections.Generic;
using System.Text;


namespace Amellar.Common.DataConnector
{
    /// <summary>
    /// This base class is a Connector entity bean. Will be useful 
    /// when multiple database engine is supported.
    /// </summary>
    public class DataConnector
    {
        protected string m_strConnectionString;
        protected string m_strErrorDescription;
        string m_sUser;

        public string ErrorDescription
        {
            get { return m_strErrorDescription; }
        }

        public virtual bool OpenConnection()
        {
            return true;
        }

        public virtual bool CloseConnection()
        {
            return true;
        }

   
       
    }
}
