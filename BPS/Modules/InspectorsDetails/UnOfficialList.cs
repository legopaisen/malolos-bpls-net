using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Modules.InspectorsDetails
{
    public class UnOfficialList
    {
        protected frmUnOfficialList UnOfficialListFrm = null;

        public UnOfficialList(frmUnOfficialList Form)
        {
            this.UnOfficialListFrm = Form;
            
        }

        public virtual void FormLoad()
        {
        }

        public virtual void ViewList()
        {
        }

        public virtual void LoadInspector()
        {
        }

        public virtual void chkFilter_CheckStateChanged(object sender, EventArgs e)
        {
            
        }

        public virtual void chkInspected_CheckStateChanged(object sender, EventArgs e)
        {
        }

        public virtual void chkTaxMapped_CheckStateChanged(object sender, EventArgs e)
        {
        }

        public virtual void chkAll_CheckStateChanged(object sender, EventArgs e)
        {
        }

        public void ClearControls()
        {
            UnOfficialListFrm.txtBnsName.Text = "";
            UnOfficialListFrm.txtBnsAdd.Text = "";
            UnOfficialListFrm.txtOwnName.Text = "";
            UnOfficialListFrm.txtInspectionNo.Text = "";
            UnOfficialListFrm.cmbInspector.SelectedIndex = -1;
            UnOfficialListFrm.btnSearch.Text = "Search";
            UnOfficialListFrm.dgvList.Columns.Clear();
        }
    }
}
