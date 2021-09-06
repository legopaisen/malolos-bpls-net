

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.InspectorsDetails
{
    public class InsDetails
    {
        protected frmInspectorDetails DetailsFrm = null;
                
        public InsDetails(frmInspectorDetails Form)
        {
            this.DetailsFrm = Form;
        }

        public void InitialFormLoad()
        {
        }

        public virtual void FormLoad()
        {
        }

        protected void ClearControls()
        {
            DetailsFrm.ClearControls();
        }

        protected void EnableControls(bool blnEnable)
        {
            DetailsFrm.EnableControls(blnEnable);
        }

        public virtual void CellClick(int iCol, int iRow)
        {
        }

        public virtual void InspectorsCellClick(int iCol, int iRow)
        {
        }

        public virtual void Search()
        {
        }

        public virtual void Add()
        {
        }

        public virtual void Edit()
        {
        }

        public virtual void Delete()
        {
        }

        public virtual void Print()
        {
        }

        public virtual void Violation()
        {
        }

        public virtual void Tag()
        {
        }

        public virtual void Untag()
        {
        }

        public virtual void IssueNotice()
        {
        }

        public virtual void RefreshList()
        {
        }

        public virtual void ValidateISNo()
        {
        }

        
    }
}
