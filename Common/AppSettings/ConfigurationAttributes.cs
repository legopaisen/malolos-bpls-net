using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.AppSettings
{
    /// <summary>
    /// This is just an entity bean of commonly used configuration settings
    /// </summary>
    public static class ConfigurationAttributes
    {
        public static string ChiefTaxDivision
        {
            get { return AppSettingsManager.GetConfigValue("115").Trim(); }
        }
        // GDE 20090209 for LGU Code (s){
        public static string LGUCode
        {
            get { return AppSettingsManager.GetConfigObject("10").Trim(); }
        }
        // GDE 20090209 for LGU Code (e)}
        // GDE 20090209 for Dist Code (s){
        public static string DistCode
        {
            get { return AppSettingsManager.GetConfigObject("11").Trim(); }
        }
        // GDE 20090209 for Dist Code (e)}

        // GDE 20090220 for LGU Name(s){
        public static string LGUName
        {
            get { return AppSettingsManager.GetConfigObject("09").Trim(); }
        }
        // GDE 20090220 for LGU Name(e)}

        public static string RevYear
        {
            get { return AppSettingsManager.GetConfigObject("07").Trim(); } // ALJ 20090703 REVISION YEAR
        }

        public static string CurrentYear
        {
            get { return AppSettingsManager.GetConfigObject("12").Trim(); } // ALJ 20100217 CURRENT YEAR
        }

        public static string AutoApplication
        {
            get { return AppSettingsManager.GetConfigObject("27").Trim(); } // ALJ 20100218 AUTO-APPLICATION
        }

        public static bool HasAdjustmentFeature
        {
            get { return AppSettingsManager.GetConfigValueByDescription("HAS ADJUSTMENT FEATURE") == "Y"; }
        }

        public static bool HasAdjustments
        {
            get { return AppSettingsManager.GetConfigValueByDescription("APPLY ADJUSTMENT").Trim() == "Y"; }
        }

        //public static string LGUName
        //{
        //    get { return AppSettingsManager.GetConfigValueByDescription("LGU NAME"); }
        //}

        public static string ProvinceName
        {
            get { return AppSettingsManager.GetConfigValueByDescription("PROVINCE"); }
        }

        public static string Version
        {
            get { return AppSettingsManager.GetConfigValue("60"); }
        }

        public static bool HasAttributeEnableAssessorApprovalModule
        {
            get { return AppSettingsManager.GetConfigValue("117") == "Y"; }
        }

        public static string AssessorOfficeName
        {
            get
            {
                string strVersion = ConfigurationAttributes.Version;
                string strAssessorOfficeName = string.Empty;

                if (strVersion == "P")
                    strAssessorOfficeName = "Provincial";
                else if (strVersion == "CC" || strVersion == "C")
                    strAssessorOfficeName = "City";
                else if (strVersion == "PM" || strVersion == "P")
                    strAssessorOfficeName = "Municipality";

                return string.Format("{0} Assessor's Office", strAssessorOfficeName);
            }
        }

        //RDO 021508 (s) flag that enables program features
        public static bool IsShowExtendedPins
        {
            //get { return true; }
            get
            {
                //RDO 031208 (s) get from configuration
                if (AppSettingsManager.GetConfigValueByDescription("SHOW EXTENDED PINS").Trim() == "Y")
                    return true;
                //RDO 031208 (e) get from configuration
                return false;
            }
        }


        //JVL (s)
        /// <summary>
        /// it only disable / enable the display but the actual comutation and saving of 
        /// JVLarion but has conflict if LGU has BOTH fire tax and idle land tax is enable it need to customized
        /// </summary>
        public static bool HasFireTax
        {
            get
            {
                if (AppSettingsManager.GetConfigValueByDescription("HAS FIRE TAX").Trim() == "Y")
                    return true;
                return false;
            }
        }

        //JVL (s)
        /// <summary>
        /// it only disable / enable the display but the actual comutation and saving of 
        /// JVLarion but has conflict if LGU has BOTH fire tax and idle land tax is enable it need to customized
        /// </summary>
        public static bool HasIdleLandTax
        {
            get
            {
                if (AppSettingsManager.GetConfigValueByDescription("HAS IDLE LAND TAX").Trim() == "Y")
                    return true;
                return false;
            }
        }


        //JVL (s)

        /*
        public static bool IsAllowMultipleOR
        {
            get
            {
                if (AppSettingsManager.IsDebugMode)
                    return true;
                return false;
            }
        }
        //RDO 021508 (e) flag that enables program features
        */

        //RDO 0707208 (s) actually returns the opposite value
        public static bool IsManualDeclaration
        {
            get
            {
                bool blnManualDeclaration = false;
                string strConfigValue = AppSettingsManager.GetConfigValueByDescription("MANUAL DECLARATION OF OR").Trim();
                if (strConfigValue == "N")
                    blnManualDeclaration = true;

                return blnManualDeclaration;
            }
        }

        public static string TreasurerName
        {
            get
            {
                string strVersion = AppSettingsManager.GetConfigValue("60");
                string strTreasurerName = string.Empty;
                if (strVersion == "P" || strVersion == "CC" || strVersion == "C")
                    strTreasurerName = AppSettingsManager.GetConfigValue("08");
                else if (strVersion == "PM")
                    strTreasurerName = AppSettingsManager.GetConfigValue("97");
                else
                    strTreasurerName = AppSettingsManager.GetConfigValueByDescription("TREASURER");

                return strTreasurerName;
            }
        }

        public static string TreasurerPositionName
        {
            get
            {
                string strVersion = AppSettingsManager.GetConfigValue("60");
                string strTreasurerNamePosition = string.Empty;
                if (strVersion == "P")
                    strTreasurerNamePosition = "Provincial Treasurer";
                else if (strVersion == "PM")
                    strTreasurerNamePosition = "Municipal Treasurer";
                else if (strVersion == "CC" || strVersion == "C")
                    strTreasurerNamePosition = "City Treasurer";

                return strTreasurerNamePosition;
            }
        }

        public static string OfficeName
        {
            get
            {
                // RMC 20110414 Modified
                string strVersion = AppSettingsManager.GetConfigValue("01").Trim();
                string strOfficeName = string.Empty;

                // RMC 20110809 enabled
                if (strVersion == "CITY")
                    strOfficeName = "Office of the City Treasurer";
                else if (strVersion == "PROVINCE")
                    strOfficeName = "Office Of The Provincial Treasurer";
                else
                    strOfficeName = "Office Of The Municipal Treasurer";
                // RMC 20110809 enabled 

                //strOfficeName = "Business Permits and Tricycle Franchising Office";
                return strOfficeName;
            }
        }

        /*
        public static string JurisdictionName
        {
            get
            {
                string strVersion = string.Empty;
                string strJurisdictionName = string.Empty;
                strVersion = AppSettingsManager.GetConfigValue("60");
                if (strVersion == "P")
                    strJurisdictionName = "Provincial";
                else if (strVersion == "CC" || strVersion == "C")
                    strJurisdictionName = "City";
                else if (strVersion == "PM" || strVersion == "P")
                    strJurisdictionName = "Municipality";

                return strJurisdictionName;
            }
        }
        */


    }
}
