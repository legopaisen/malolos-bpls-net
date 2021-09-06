using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Amellar.Common.AppSettings
{
    public class BailOutInfo
    {

        public string ErrorDescription
        {
            get { return m_strErrorDescription; }
            set { m_strErrorDescription = value; }
        }
        private string m_strErrorDescription;

        public string Workstation
        {
            get { return m_strWorkStation; }
            set { m_strWorkStation = value; }
        }
        private string m_strWorkStation;



        public string ModuleCode //for further info we can query the user_lock table for module code or object it access
        {
            get { return m_strModuleCode; }
            set { m_strModuleCode = value; }
        }
        private string m_strModuleCode;


        public string User
        {
            get { return m_strUser; }
            set { m_strUser = value; }
        }
        private string m_strUser;

        public string Teller
        {
            get { return m_strTeller; }
            set { m_strTeller = value; }
        }
        private string m_strTeller;

        public DateTime DateTimeBailOut
        {
            get { return m_dtDateTimeBailOut; }
            set { m_dtDateTimeBailOut = value; }
        }
        private DateTime m_dtDateTimeBailOut;

        
        public BailOutInfo()
        {
            m_strModuleCode = "";
            m_strTeller = "";
            m_strUser = "";
            m_strWorkStation = "";
        }


        public static class Start
        {
            public static bool BailOut(BailOutInfo info)
            {
                string strCurrentDir = string.Format(@"{0}\{1}.LOG", Directory.GetCurrentDirectory(), "LOGS-OUT");
                StreamWriter write = new StreamWriter(strCurrentDir, true);
                write.WriteLine(info.DateTimeBailOut.ToShortDateString());
                if (info.Workstation != "")
                    write.WriteLine(info.Workstation);
                if (info.User != "")
                    write.WriteLine(info.User);
                if (info.Teller != "")
                    write.WriteLine(info.Teller);
                if (info.ModuleCode != "")
                    write.WriteLine(info.ModuleCode);
                if (info.ErrorDescription != "")
                    write.WriteLine(info.ErrorDescription);
                write.Close();
                return true;
            }
        }
    }

}
