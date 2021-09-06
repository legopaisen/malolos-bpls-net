using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using System.IO;

namespace Amellar.Common.ImageViewer
{
    public class ImageTransation
    {
        public ImageTransation()
        { 
            
        }

        // AST 20150430 (s)
        private string m_strFileName = string.Empty;
        public string FileName
        {
            set { m_strFileName = value; }
        }
        // AST 20150430 (e)

        /// <summary>
        /// Use as for update blob build-up
        /// </summary>
        /// <returns>boolean when successfully attach if false it rollback</returns>
        public bool UpdateImage(ImageInfo objInfo)
        {
            try
            {
                OracleResultSet resultExt = new OracleResultSet();
                resultExt.Transaction = true;
                StringBuilder strObject = new StringBuilder();
                strObject.Length = 0;
                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                /*result.CreateNewInstance(0);
                if (!result.isConnected)
                    return false;*/

                result.Transaction = true;
                int intCount = 0;

                DateTime dtCurrentDateTime = AppSettingsManager.GetCurrentDate();
                int intId = objInfo.ID;
                string strDocCode = string.Format("{0:00#}", objInfo.getDocCode);

                // AST 20150415 remove loop temporary for work around daw po(s)
                //for (int i = 0; i < ImageAppSettings.getScannedImagePerPage(); i++)
                //{
                    // AST 20150415 remove condition as gob request (s)
                    // RMC 20150226 adjustment in blob configuration (s)
                    //if (AppSettingsManager.GetBlobImageConfig() == "F")
                    //    result.Query = "update docblob_twopage set iscancelled = 0, ispending = 0, isrejected = 0, bin = :1, doc_code = :2, dist_code = :3, kind = :4, deficient = :5, upload_dt = :6, upload_tm = :7, upload_by = :8, status = :9 where id = :10 and encoder = :11 and sys_type = :12";
                    //else// RMC 20150226 adjustment in blob configuration (e)
                        result.Query = "update docblob_tbl set iscancelled = 0, ispending = 0, isrejected = 0, bin = :1, doc_code = :2, dist_code = :3, kind = :4, deficient = :5, upload_dt = :6, upload_tm = :7, upload_by = :8, status = :9 where id = :10 and encoder = :11 and sys_type = :12";
                    // AST 20150415 remove condition as gob request (e)
                    result.AddParameter(":1", objInfo.TRN);
                    result.AddParameter(":2", strDocCode);
                    result.AddParameter(":3", string.Empty);
                    result.AddParameter(":4", string.Empty);
                    result.AddParameter(":5", string.Empty); //temporary but need to table base on select * from DELQ_TBL
                    result.AddParameter(":6", dtCurrentDateTime);
                    result.AddParameter(":7", string.Format("{0:hh:mm:ss}", dtCurrentDateTime));
                    result.AddParameter(":8", AppSettingsManager.SystemUser.UserCode);
                    result.AddParameter(":9", string.Empty);
                    //result.AddParameter(":10", intId + i);
                    result.AddParameter(":10", intId);
                    result.AddParameter(":11", AppSettingsManager.SystemUser.UserCode);
                    result.AddParameter(":12", objInfo.System);
                    result.ExecuteNonQuery();

                    //strObject.Append(string.Format("{0} - {1} ", (intId + i), objInfo.TRN.Substring(7)));
                    strObject.Append(string.Format("{0} - {1} ", intId, objInfo.TRN.Substring(7)));

                //}
                    // AST 20150415 remove loop temporary for work around daw po(e)

                //insert table match_hist
                //insert audtrail
                //insert int database of faas for more info like match_hist

                /*if (AuditTrail.AuditTrail.InsertTrail(resultExt, AppSettingsManager.SystemUser.UserCode, string.Empty,
                    "COFAY", "DOCBLOB_SAMPLE, MATCHED_ADDL_INFO", strObject.ToString(), "TAG MATCHED") == 0)
                {
                    result.Rollback();
                    result.Close();

                    resultExt.Rollback();
                    resultExt.Close();

                    return false;
                }*/



                if (!result.Commit() || !resultExt.Commit())
                {
                    result.Rollback();
                    result.Close();


                    resultExt.Rollback();
                    resultExt.Close();

                    return false;
                }


                result.Close();
                resultExt.Close();



                return true;
            }
            catch
            {
                return false;
            }
        }   
        
        /// <summary>
        /// Use for tagging Unmatch
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="strUserCode"></param>
        /// <returns></returns>
        /*public bool UpdateImage(int intIdParam, string strUserCode, string strSysType, Amellar.RPTA.Classes.DeficientRecords.frmInformationPopUp info)
        {

            OracleResultSet resultExt = new OracleResultSet();
            resultExt.Transaction = true;
            int intCount = 0;
            StringBuilder strObject = new StringBuilder();
            strObject.Length = 0;

            OracleResultSet result = new OracleResultSet();
            result.CreateNewInstance(0);
            if (!result.isConnected)
                return false;

            result.Transaction = true;

            DateTime dtCurrentDateTime = AppSettingsManager.GetCurrentDate();
            int intId = intIdParam;
           
          

            for (int i = 0; i < AppSettings.getScannedImagePerPage(); i++)
            {
                result.Query = "update docblob_sample set iscancelled = 0, ispending = 0, isrejected = 0, deficient = :1, upload_dt = :2, upload_tm = :3, upload_by = :4 where id = :5 and encoder = :6 and sys_type = :7";
                result.AddParameter(":1", "002"); //temporary but need to table base on select * from DELQ_TBL
                result.AddParameter(":2", dtCurrentDateTime);
                result.AddParameter(":3", string.Format("{0:hh:mm:ss}", dtCurrentDateTime));
                result.AddParameter(":4", strUserCode);
                result.AddParameter(":5", intId + i);
                result.AddParameter(":6", strUserCode);
                result.AddParameter(":7", strSysType);
                result.ExecuteNonQuery();


                result.Query = "select count(*) from MATCHED_ADDL_INFO where id = :1 and sys_type = 'C'";
                result.AddParameter(":1", intId + i);
                
                intCount = 0;
                int.TryParse(result.ExecuteScalar(), out intCount);
                if (intCount != 0)
                {
                    result.QueryText = "delete from MATCHED_ADDL_INFO where id = :1 and sys_type = 'C'";
                    result.ExecuteNonQuery();
                }

                result.Query = "insert into MATCHED_ADDL_INFO values(:1, :2, :3, :4, :5, :6, :7, :8, :9)";
                result.AddParameter(":1", intId + i);
                result.AddParameter(":2", info.Pin);
                result.AddParameter(":3", info.Arpn);
                result.AddParameter(":4", strSysType);
                result.AddParameter(":5", " ");
                result.AddParameter(":6", "002");
                result.AddParameter(":7", "UNMATCHED");
                result.AddParameter(":8", strUserCode);
                result.AddParameter(":9", dtCurrentDateTime);
                result.ExecuteNonQuery();




                //insert in database
                resultExt.Query = "select count(*) from MATCHED_ADDL_INFO where id = :1 and sys_type = 'C'";
                resultExt.AddParameter(":1", intId + i);
               
                intCount = 0;
                int.TryParse(resultExt.ExecuteScalar(), out intCount);
                if (intCount != 0)
                {
                    resultExt.QueryText = "delete from MATCHED_ADDL_INFO where id = :1 and sys_type = 'C'";
                    resultExt.ExecuteNonQuery();
                }

                resultExt.Query = "insert into MATCHED_ADDL_INFO values(:1, :2, :3, :4, :5, :6, :7, :8, :9)";
                resultExt.AddParameter(":1", intId + i);
                resultExt.AddParameter(":2", info.Pin);
                resultExt.AddParameter(":3", info.Arpn);
                resultExt.AddParameter(":4", strSysType);
                resultExt.AddParameter(":5", " ");
                resultExt.AddParameter(":6", "002");
                resultExt.AddParameter(":7", string.Format("{0} {1}", "UNMATCHED", info.OtherInfo));
                resultExt.AddParameter(":8", strUserCode);
                resultExt.AddParameter(":9", dtCurrentDateTime);
                resultExt.ExecuteNonQuery();


                strObject.Append(string.Format("{0} ", intId + i));


            }

            //insert table match_hist
            //insert audtrail
            //insert int database of faas for more info like match_hist

            if (AuditTrail.AuditTrail.InsertTrail(resultExt, AppSettingsManager.SystemUser.UserCode, string.Empty,
                "COFAY", "DOCBLOB_SAMPLE, MATCHED_ADDL_INFO", strObject.ToString(), "TAG UNMATCHED") == 0)
            {
                result.Rollback();
                result.Close();

                resultExt.Rollback();
                resultExt.Close();

                return false;
            }



            if (!result.Commit() || !resultExt.Commit())
            {
                result.Rollback();
                result.Close();


                resultExt.Rollback();
                resultExt.Close();

                return false;
            }


            result.Close();
            resultExt.Close();
            



            return true;

        
        }
        */



        /// <summary>
        /// Use for other deficient
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="strUserCode"></param>
        /// <returns></returns>
        public bool UpdateImage(int intIdParam, string strUserCode, string strSysType, string strDeficient)
        {
            try
            {
                int intPending = 0;
                int intRejected = 0;

                if (strDeficient == "PENDING")
                    intPending = 1;
                else if (strDeficient == "REJECTED")
                    intRejected = 1;
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please tag again this TRN.");
                    return false;
                }


                OracleResultSet resultExt = new OracleResultSet();
                resultExt.Transaction = true;
                int intCount = 0;
                StringBuilder strObject = new StringBuilder();
                strObject.Length = 0;

                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                /*result.CreateNewInstance(0);

                if (!result.isConnected)
                    return false;*/

                result.Transaction = true;

                DateTime dtCurrentDateTime = AppSettingsManager.GetCurrentDate();
                int intId = intIdParam;



                // AST 20150415 remove loop temporary for work around daw po(s)
                //for (int i = 0; i < ImageAppSettings.getScannedImagePerPage(); i++)
                //{
                    // AST 20150415 remove condition as gob request (s)
                    // RMC 20150226 adjustment in blob configuration (s)
                    //if (AppSettingsManager.GetBlobImageConfig() == "F")
                    //    result.Query = "update docblob_twopage set iscancelled = :1, ispending = :2, isrejected = :3, upload_dt = :4, upload_tm = :5 where id = :6 and encoder = :7 and sys_type = :8";
                    //else// RMC 20150226 adjustment in blob configuration (e)
                        result.Query = "update docblob_tbl set iscancelled = :1, ispending = :2, isrejected = :3, upload_dt = :4, upload_tm = :5 where id = :6 and encoder = :7 and sys_type = :8";
                    // AST 20150415 remove condition as gob request (e)
                    result.AddParameter(":1", 0);
                    result.AddParameter(":2", intPending);
                    result.AddParameter(":3", intRejected);
                    result.AddParameter(":4", dtCurrentDateTime);
                    result.AddParameter(":5", string.Format("{0:hh:mm:ss}", dtCurrentDateTime));
                    //result.AddParameter(":6", intId + i);
                    result.AddParameter(":6", intId);
                    result.AddParameter(":7", strUserCode);
                    result.AddParameter(":8", strSysType);
                    result.ExecuteNonQuery();

                    //strObject.Append(string.Format("{0} ", intId + i));
                    strObject.Append(string.Format("{0} ", intId));
                //}
                // AST 20150415 remove loop temporary for work around daw po(e)
                /*if (AuditTrail.AuditTrail.InsertTrail(resultExt, AppSettingsManager.SystemUser.UserCode, string.Empty,
                    "COFAY", "DOCBLOB_SAMPLE, MATCHED_ADDL_INFO", strObject.ToString(), strDeficient) == 0)
                {
                    result.Rollback();
                    result.Close();

                    resultExt.Rollback();
                    resultExt.Close();

                    return false;
                }*/

                if (!result.Commit() || !resultExt.Commit())
                {
                    result.Rollback();
                    result.Close();


                    resultExt.Rollback();
                    resultExt.Close();

                    return false;
                }


                result.Close();
                resultExt.Close();

                return true;

            }
            catch
            {
                return false;
            }
        }

        // RMC 20091211 Added function to detach image attached to deleted posted payment (s)
        public bool DetachImage(string m_sTRN)
        {
            try
            {
                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                /*result.CreateNewInstance(0);

                if (!result.isConnected)
                    return false;*/

                result.Transaction = true;

                // RMC 20150226 adjustment in blob configuration (s)
                string sBlobConfig = AppSettingsManager.GetBlobImageConfig();
                // AST 20150415 remove condition as gob request (s)
                //if(sBlobConfig == "F")
                //    result.Query = "select count(*) from docblob_twopage where bin = :1";
                //else// RMC 20150226 adjustment in blob configuration (e)
                if (m_strFileName.Contains("BPLS ASSESSMENT - APPLICATION")) // AST 20150430
                    result.Query = "select count(*) from docblob_twopage where bin = :1";
                else
                    result.Query = "select count(*) from docblob_tbl where bin = :1";
                // AST 20150415 remove condition as gob request (e)
                result.AddParameter(":1", m_sTRN);
                int intCount = 0;
                int.TryParse(result.ExecuteScalar(), out intCount);
                if (intCount != 0)
                {

                    // AST 20150415 remove loop temporary for work around daw po(s)
                    //for (int i = 0; i < ImageAppSettings.getScannedImagePerPage(); i++)
                    //{
                        // AST 20150415 remove condition as gob request (s)
                        // RMC 20150226 adjustment in blob configuration (s)
                        //if (sBlobConfig == "F")
                        //    result.Query = string.Format("update docblob_twopage set bin = '', deficient = '', status = '', encoder =:1 where bin = :2 and sys_type = '{0}'", AppSettingsManager.GetSystemType);
                        //else// RMC 20150226 adjustment in blob configuration (e)
                    if (m_strFileName.Contains("BPLS ASSESSMENT - APPLICATION")) // AST 20150430
                        result.Query = string.Format("update docblob_twopage set bin = '', deficient = '', status = '', encoder =:1 where bin = :2 and sys_type = '{0}'", AppSettingsManager.GetSystemType);
                    else
                        result.Query = string.Format("update docblob_tbl set bin = '', deficient = '', status = '', encoder =:1 where bin = :2 and sys_type = '{0}'", AppSettingsManager.GetSystemType);
                        // AST 20150415 remove condition as gob request (e)

                        result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode);
                        result.AddParameter(":2", m_sTRN);
                        result.ExecuteNonQuery();
                    //}
                    // AST 20150415 remove loop temporary for work around daw po(e)

                    if (!result.Commit())
                    {
                        result.Rollback();
                        result.Close();
                        return false;
                    }
                    result.Close();
                }
                else
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
        // RMC 20091211 Added function to detach image attached to deleted posted payment (e)

        public bool DetachImage(string m_sTRN, int intID)
        {
            try
            {
                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                // result.CreateNewInstance(0);
                result.Transaction = true;

                // RMC 20150226 adjustment in blob configuration (s)
                string sBlobConfig = AppSettingsManager.GetBlobImageConfig();
                // AST 20150415 remove condition as gob request (s)
                //if (sBlobConfig == "F")
                //    result.Query = "select count(*) from docblob_twopage where bin = :1 and (id = :2 or id = :3)";
                //else// RMC 20150226 adjustment in blob configuration (e)
                if (m_strFileName.Contains("BPLS ASSESSMENT - APPLICATION")) // AST 20150430
                    result.Query = "select count(*) from docblob_twopage where bin = :1 and (id = :2)";
                else
                    result.Query = "select count(*) from docblob_tbl where bin = :1 and (id = :2)";
                // AST 20150415 remove condition as gob request (e)
                result.AddParameter(":1", m_sTRN);
                result.AddParameter(":2", intID);
                // AST 20150415 remove condition as gob request (s)
                //if (sBlobConfig == "F")
                //    result.AddParameter(":3", intID + 1);
                // AST 20150415 remove condition as gob request (e)
                int intCount = 0;
                int.TryParse(result.ExecuteScalar(), out intCount);
                if (intCount != 0)
                {
                    // AST 20150415 remove loop temporary for work around daw po(s)
                    //for (int i = 0; i < ImageAppSettings.getScannedImagePerPage(); i++)
                    //{
                        // AST 20150415 remove condition as gob request (s)
                        // RMC 20150226 adjustment in blob configuration (s)
                        //if (sBlobConfig == "F")
                        //    result.Query = string.Format("update docblob_twopage set bin = '', deficient = '', status = '', encoder =:1 where bin = :2 and id = :3 and sys_type = '{0}'", AppSettingsManager.GetSystemType);
                        //else// RMC 20150226 adjustment in blob configuration (e)

                    if (m_strFileName.Contains("BPLS ASSESSMENT - APPLICATION")) // AST 20150430
                        result.Query = string.Format("update docblob_twopage set bin = '', deficient = '', status = '', encoder =:1 where bin = :2 and id = :3 and sys_type = '{0}'", AppSettingsManager.GetSystemType);
                    else
                        result.Query = string.Format("update docblob_tbl set bin = '', deficient = '', status = '', encoder =:1 where bin = :2 and id = :3 and sys_type = '{0}'", AppSettingsManager.GetSystemType);
                        // AST 20150415 remove condition as gob request (e)

                        result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode);
                        result.AddParameter(":2", m_sTRN);
                        //result.AddParameter(":2", intID + i);
                        result.AddParameter(":2", intID);
                        result.ExecuteNonQuery();
                    //}
                    // AST 20150415 remove loop temporary for work around daw po(e)

                    if (!result.Commit())
                    {
                        result.Rollback();
                        result.Close();
                        return false;
                    }
                    result.Close();
                }
                else
                    return false;

                return true;
            }
            catch { return false; }
        }

        public bool InsertImage(string sBIN, string sFileDirectory, string sBrgy)
        {
            // RMC 20111206
            try
            {
                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                result.Transaction = true;

                int intSequence = 0;
                // RMC 20150226 adjustment in blob configuration (s)
                string sBlobConfig = AppSettingsManager.GetBlobImageConfig();
                // AST 20150415 remove condition as gob request (s)
                //if (sBlobConfig == "F")
                //    result.Query = "SELECT dblob_seq_two.nextval FROM dual"; //hardcoded
                //else// RMC 20150226 adjustment in blob configuration (e)
                    result.Query = "SELECT docblob_seq.nextval FROM dual"; //hardcoded
                // AST 20150415 remove condition as gob request (e)
                if (result.Execute())
                {
                    if (result.Read())
                        intSequence = result.GetInt(0);
                }

                string sTmp = "";
                string sFileName = System.IO.Path.GetFileName(sFileDirectory);
                string sFileExtTmp = System.IO.Path.GetExtension(sFileDirectory);
                string sFileExt = "";
                int iLen = sFileExtTmp.Length;

                byte[] blobData = null;

                using (FileStream fileStream = new FileStream(sFileDirectory, FileMode.Open, FileAccess.Read))
                {
                    blobData = new byte[fileStream.Length];
                    fileStream.Read(blobData, 0, System.Convert.ToInt32(fileStream.Length));
                    fileStream.Close();
                }

                sFileExt = sFileExtTmp.Substring(1, iLen - 1).ToUpper();

                // RMC 20150226 adjustment in blob configuration (s)
                if (sBlobConfig == "F")
                    result.Query = "insert into docblob_twopage values ('" + sBIN + "', ";
                else// RMC 20150226 adjustment in blob configuration (e)
                    result.Query = "insert into docblob_tbl values ('" + sBIN + "', ";
                result.Query += "'001', 'F', '" + sFileExt + "', ";
                result.Query += "'', '" + sFileName.ToUpper() + "', '" + sFileDirectory + "', '', ";
                result.Query += "'" + sBrgy + "','','N','A', ";
                result.Query += "sysdate, to_char(sysdate, 'HH24:MI:SS'), ";
                result.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', :1, ";
                result.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                result.Query += " " + intSequence + ", ";
                result.Query += "'', '', 0,0,0,to_char(sysdate, 'YYYY')) ";

                using (MemoryStream stream = new MemoryStream())
                {
                    result.AddParameter(":1", blobData);


                    if (result.ExecuteNonQuery() == 0)
                    {
                        result.Rollback();
                        result.Close();
                        stream.Close();
                        return false;
                    }
                    stream.Close();
                }

                if (!result.Commit())
                {
                    result.Rollback();
                    result.Close();
                    return false;
                }

                return true;
            }
            catch { return false; }
        }
        
    }
}
