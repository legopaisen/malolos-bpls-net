using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;


namespace Amellar.BPLS.SearchAndReplace
{
    public class SearchAndReplace
    {
        protected frmSearchAndReplace SearchAndReplaceFrm = null;
        protected OracleResultSet pSet = new OracleResultSet();
                
        public SearchAndReplace(frmSearchAndReplace Form)
        {
            this.SearchAndReplaceFrm = Form;
            
        }

        public virtual void Update()
        {
            
        }

        public void ClearFields(bool blnOld, bool blnNew)
        {
            if (blnOld) // RMC 20110414
            {
                SearchAndReplaceFrm.bin1.txtTaxYear.Text = "";
                SearchAndReplaceFrm.bin1.txtBINSeries.Text = "";
                SearchAndReplaceFrm.txtLn.Text = "";
                SearchAndReplaceFrm.txtFn.Text = "";
                SearchAndReplaceFrm.txtMi.Text = "";
                SearchAndReplaceFrm.txtAdd.Text = "";
                SearchAndReplaceFrm.txtBrgy.Text = "";
                SearchAndReplaceFrm.txtSt.Text = "";
                SearchAndReplaceFrm.txtDist.Text = "";
                SearchAndReplaceFrm.txtMun.Text = "";
                SearchAndReplaceFrm.txtZone.Text = "";
                SearchAndReplaceFrm.txtOwnCode.Text = "";
                SearchAndReplaceFrm.txtZip.Text = ""; // RMC 20110414
                SearchAndReplaceFrm.btnSearch.Text = "Search";  // RMC 20110725
            }

            //if (blnOld) // RMC 20110414
            if (blnNew) // RMC 20110414
            {
                SearchAndReplaceFrm.bin2.txtTaxYear.Text = "";
                SearchAndReplaceFrm.bin2.txtBINSeries.Text = "";
                SearchAndReplaceFrm.txtLn1.Text = "";
                SearchAndReplaceFrm.txtFn1.Text = "";
                SearchAndReplaceFrm.txtMi1.Text = "";
                SearchAndReplaceFrm.txtAdd1.Text = "";
                SearchAndReplaceFrm.txtBrgy1.Text = "";
                SearchAndReplaceFrm.txtSt1.Text = "";
                SearchAndReplaceFrm.txtDist1.Text = "";
                SearchAndReplaceFrm.txtMun1.Text = "";
                SearchAndReplaceFrm.txtZone1.Text = "";
                SearchAndReplaceFrm.txtOwnCode1.Text = "";
                SearchAndReplaceFrm.txtZip1.Text = ""; // RMC 20110414
                SearchAndReplaceFrm.btnSearch1.Text = "Search";  // RMC 20110725
            }
        }

        
    }
}
