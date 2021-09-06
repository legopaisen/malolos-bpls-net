using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.ImageViewer
{
    public class ImageInfo
    {
        //need to implement for future udates if needed
        private string m_strTRN;        

        public string TRN
        {
            get { return m_strTRN; }
            set
            {
                if (value != null && value != "")
                {
                    try
                    {
                        
                    }
                    catch
                    { }
                }
                m_strTRN = value;
            }
        }

        private string m_strSysType;
        public string System
        {
            get { return m_strSysType; }
            set { m_strSysType = value; }
        }

        
        private string m_strUserCode;
        public string UserCode
        {
            get { return m_strUserCode; }
            set { m_strUserCode = value; }
        }


        private int m_intID;
        public int ID
        {
            get { return m_intID; }
            set { m_intID = value; }
        }        

        public ImageInfo()
        {
            m_strTRN = string.Empty;
            m_strSysType = string.Empty;
            m_strUserCode = string.Empty;            
        }

        public ImageInfo(string strTRN, string strSystemType, string strUserCode)
        {
            m_strTRN = strTRN.Trim();
            m_strSysType = strSystemType;
            m_strUserCode = strUserCode;
        }

        public ImageInfo(string strSystemType, string strUserCode)
        {
            m_strSysType = strSystemType;
            m_strUserCode = strUserCode;            
        }



        private string m_strImageName;
        public string ImageName
        {
            get { return m_strImageName; }
            set { m_strImageName = value; }
        }

        public ImageInfo(int strID)
        {
            m_intID = strID;            
        }       

        //Set the property for faas (s)
        public void setFaasInfo(string strPin)
        {
            /*OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("select * from faas_view where pin = '{0}'", strPin);

            if (result.Execute())
            {
                if (result.Read())
                {
                    m_strOwnCode = result.GetString("own_code").Trim();
                    m_strPrevPin = result.GetString("prev_pin").Trim();
                    m_strArpn = result.GetString("arpn").Trim();
                    m_strPrevArpn = result.GetString("prev_arp").Trim();
                    m_strSurveyNo = result.GetString("survey_no").Trim();
                    m_strLotNumber = result.GetString("lot_no").Trim();
                    m_strCadNumber = result.GetString("oct_no").Trim();
                    m_strBlockNumber = result.GetString("blk_no").Trim();
                    m_strActualUse = result.GetString("act_use").Trim();

                    this.Pin = strPin;
                }
            }

            result.Close();*/
           
        }        

        public int getDocCode
        {
            get
            {
                if (m_strTRN != string.Empty && m_strTRN != null)
                    return getDocCodeMethod(m_strTRN);
                else
                    return 1;

            }
        }



        private int getDocCodeMethod(string strTRN)
        {
            try
            {
                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                /*result.CreateNewInstance(0);

                if (!result.isConnected)
                    return 1;*/

                // RMC 20150226 adjustment in blob configuration (s)
                if (AppSettingsManager.GetBlobImageConfig() == "F")
                    result.Query = string.Format("select DECODE(max(doc_code) + 1, NULL, 1, max(doc_code) + 1) from docblob_twopage where bin = '{0}' and sys_type = '{1}'", strTRN, AppSettingsManager.GetSystemType);
                else// RMC 20150226 adjustment in blob configuration (e)
                    result.Query = string.Format("select DECODE(max(doc_code) + 1, NULL, 1, max(doc_code) + 1) from docblob_tbl where bin = '{0}' and sys_type = '{1}'", strTRN, AppSettingsManager.GetSystemType);
                int intDocCode = 1;
                int.TryParse(result.ExecuteScalar(), out intDocCode);


                result.Close();
                return intDocCode;
            }
            catch
            {
                return 1;
            }

        }



            /*

       PIN         VARCHAR2(29)       Y                         
DOC_CODE    VARCHAR2(100)      Y                         
DOC_TYPE    VARCHAR2(50)       Y                         
DOC_EXTN    VARCHAR2(2000)     Y                         
DIST_CODE   VARCHAR2(2)        Y                         
FILE_NM     VARCHAR2(100)      Y                         
FILE_PATH   VARCHAR2(200)      Y                         
BRGY_NAME   VARCHAR2(200)      Y                         
SECT_NAME   VARCHAR2(200)      Y                         
KIND        VARCHAR2(20)       Y                         
DEFICIENT   VARCHAR2(3)        Y                         
SYS_TYPE    VARCHAR2(1)        Y                         
UPLOAD_DT   DATE               Y                         
UPLOAD_TM   VARCHAR2(20)       Y                         
UPLOAD_BY   VARCHAR2(50)       Y                         
DOCFILE     BLOB               Y                         
ENCODER     VARCHAR2(100)      Y                         
ID          NUMBER             Y                         
SHA1HASH    VARCHAR2(128 CHAR) Y                         
STATUS      VARCHAR2(20)       Y                         
ISCANCELLED NUMBER(38)                  0                
ISPENDING   NUMBER(38)                  0                
ISREJECTED  NUMBER(38)                  0                
 */






    }
}
