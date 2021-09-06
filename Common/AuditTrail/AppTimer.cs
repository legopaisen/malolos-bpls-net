using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Amellar.Common.AppSettings;

namespace Amellar.Common.AuditTrail
{

    public static class AppTimer
    {
        private static DateTime m_dtStartTime; //starting time
        private static DateTime m_dtEndTime; //ending time

        private static string m_strLogFileName = "SRE.log"; //default log name

        public static void StartTime()
        {
            m_dtStartTime = AppSettingsManager.GetSystemDate();
            m_dtEndTime = m_dtStartTime;
        }

        public static void EndTime(string strDescription)
        {
            m_dtEndTime = AppSettingsManager.GetSystemDate();
            try
            {
                TimeSpan tsEllapsedTime = m_dtEndTime - m_dtStartTime;
                StringBuilder strTrail = new StringBuilder();
                strTrail.Append(string.Format("{0:MM/dd/yyyy}|{0:HH:mm}", AppSettingsManager.GetSystemDate()));
                strTrail.Append(string.Format("|{0}|{1} {2}", AppSettingsManager.GetWorkstationName(), AppSettingsManager.SystemUser.UserCode,
                    AppSettingsManager.GetDistrictCode()));
                strTrail.Append(string.Format("|{0}", strDescription));
                strTrail.Append(string.Format("|{0:0.000000}\n", tsEllapsedTime.TotalSeconds));

                using (FileStream fileStream = new FileStream(m_strLogFileName, FileMode.Append,
                    FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.WriteLine(strTrail.ToString());
                    }
                }

            }
            catch { }

            //System.Windows.Forms.MessageBox.Show(string.Format("{0}.{1:#####0} s", tsEllapsedTime.Seconds, tsEllapsedTime.Milliseconds)); ;
        }

        

    }
}
