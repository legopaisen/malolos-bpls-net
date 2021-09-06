using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Amellar.Common.DataConnector
{
    public static class BlobTools
    {
        /// <summary>
        /// From Database to a File it will create a file in the specified strPath withd filename
        /// </summary>
        /// <param name="strPath">string path name with filename of file to be created</param>
        /// <param name="byteArray">byte[] of the blob data in the data base result.getBlob(0)</param>
        /// <returns>return true if file is save in the given path</returns>
        public static bool hasSaveBlobFile(string strPath, byte[] byteArray)
        {
            try
            {
                FileStream fileStream = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// It convert the files in to byte[] and can be used as parameter in blob fields
        /// </summary>
        /// <param name="strPath">string path of the files with filename</param>
        /// <returns>return byte[] or oracle blob type data</returns>
        public static byte[] ReadBlobFile(string strPath)
        {
            try
            {
                FileStream fileStream = new FileStream(strPath, FileMode.Open, FileAccess.Read);
                // Create a byte array of file stream length
                byte[] blobData = new byte[fileStream.Length];
                //Read block of bytes from stream into the byte array
                fileStream.Read(blobData, 0, System.Convert.ToInt32(fileStream.Length));
                fileStream.Close();
                return blobData;
            }
            catch
            { return null; }
        }

        /*
            OracleResultSet re = new OracleResultSet();
            re.Query = "insert into blob_sample values(:1, :2, :3)";
            re.AddParameter(":1", 1);
            re.AddParameter(":2", BlobTools.ReadBlobFile("c:\\sample\\sample.jpg"));
            re.AddParameter(":3", "c:\\sample");
            re.ExecuteNonQuery();
            
                        
            re.Query = "select blob_picture from blob_sample where id = 1";
            
            if (re.Execute())
            {
                if (re.Read())
                {
                    BlobTools.hasSaveBlobFile("c:\\sample\\sample1.jpg", re.GetBlob(0));                    
                }
            }

            re.Query = "update blob_sample set blob_picture = :1 where id = 1";
            re.AddParameter(":1", BlobTools.ReadBlobFile("c:\\sample\\test.gif"));
            re.ExecuteNonQuery();

            re.Query = "select blob_picture from blob_sample where id = 1";

            if (re.Execute())
            {
                if (re.Read())
                {
                    Amellar.Common.DataConnector. BlobTools.hasSaveBlobFile("c:\\sample\\test.jpg", re.GetBlob(0));
                }
            }
        */

    }
}
