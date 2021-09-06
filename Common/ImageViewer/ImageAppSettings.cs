using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.ImageViewer
{
    public class ImageAppSettings
    {
        public static bool IsViewOtherImages; // AST 20150408

        public static int getScannedImagePerPage()
        {
            try
            {
                int intScannedImagePerPage = 2; //default 
                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                /*result.CreateNewInstance(0);
                if (result.isConnected)*/

                // RMC 20150226 adjustment in blob configuration (s)
                if (AppSettingsManager.GetBlobImageConfig() == "F")
                    result.Query = "SELECT increment_by FROM user_sequences WHERE sequence_name = 'DBLOB_SEQ_TWO'";
                else// RMC 20150226 adjustment in blob configuration (e)
                {
                    if (IsViewOtherImages) // AST 20150408
                        result.Query = "SELECT increment_by FROM user_sequences WHERE sequence_name = 'DBLOB_SEQ_TWO'";
                    else
                        result.Query = "SELECT increment_by FROM user_sequences WHERE sequence_name = 'DOCBLOB_SEQ'";
                }
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            intScannedImagePerPage = result.GetInt(0);
                        }
                    }
                    result.Close();

                return intScannedImagePerPage;
            }
            catch
            {
                return 2;
            }
        }

        //determines if loggins is enabled
        public static bool isLoggingEnabled()
        {
            return true;
        }
    }
}
