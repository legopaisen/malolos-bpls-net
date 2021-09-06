

// RMC 20120418 added searching by address in Business mappin printing of notice
// GDE 20120416 test
// RMC 20120329 Modifications in Notice of violation


using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Modules.InspectorsDetails
{
    public class OfficialTagging
    {
        protected frmOfficialTagging TaggingFrm = null;
        protected PrintInspection PrintClass = null; 
        protected string m_sBin = string.Empty;
        protected string m_sWatcher = string.Empty;
        protected string m_sIns = string.Empty;
        protected string m_sISNum = string.Empty;
        protected string m_sBnsNm = string.Empty;   // GDE 20120416 test
        protected string m_sBnsAdd = string.Empty;  // RMC 20120418 added searching by address in Business mappin printing of notice
        protected string m_sOwnName = string.Empty; // RMC 20120418 added searching by address in Business mappin printing of notice

        public string Watcher
        {
            get { return m_sWatcher; }
            set { m_sWatcher = value; }
        }

        public string Inspector
        {
            get { return m_sIns; }
            set { m_sIns = value; }
        }

        public string Bin
        {
            get { return m_sBin; }
            set { m_sBin = value; }
        }

        public string ISNum
        {
            get { return m_sISNum; }
            set { m_sISNum = value; }
        }

        public OfficialTagging(frmOfficialTagging Form)
        {
            this.TaggingFrm = Form;
        }

        public virtual void FormLoad()
        {
        }

        public virtual void OneByOne()
        {
        }

        public virtual void OnWithNotice()
        {
        }

        public virtual void OnWith2ndNotice()   // RMC 20120329 Modifications in Notice of violation
        {
        }

        public virtual void OnWClosureNotice()  // RMC 20120329 Modifications in Notice of violation
        {
        }

        public virtual void OnWithoutNotice()
        {
        }

        public virtual void OnWithTag()
        {
        }

        public virtual void ButtonIssueNotice()
        {
        }

        public virtual void ButtonSendNotice()
        {
        }

        public virtual void ButtonDeleteNotice()
        {
        }

        public virtual void ButtonGenerate()
        {
        }

        public virtual void ButtonSearch()
        {
        }

        public virtual void CheckWNotice()
        {
        }

        public virtual void CheckW2ndNotice()   // RMC 20120329 Modifications in Notice of violation
        {
        }

        public virtual void CheckWClosureNotice()   // RMC 20120329 Modifications in Notice of violation
        {
           
        }

        public virtual void CheckWONotice()
        {
        }

        public virtual void CheckWTag()
        {
        }

        public virtual void CellClickBnsInfo(int iRow)
        {
        }

        public virtual void ButtonPrintList()   // RMC 20120329 Modifications in Notice of violation
        {
        }

        public virtual void ButtonClear()   // RMC 20120417 Modified Final notice format
        {
        }

        public virtual void CellRowEnter(int iRow)  // RMC 20120418 added searching by address in Business mappin printing of notice
        {
        }
    }
}
