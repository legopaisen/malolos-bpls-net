using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;

namespace Amellar.BPLS.SearchAndReplace
{
    public class BINReplaceAcct:SearchAndReplace
    {
        
        public BINReplaceAcct(frmSearchAndReplace Form)
            : base(Form)
        {
        }

        public override void Update()
        {
            string strBin = SearchAndReplaceFrm.bin1.GetBin();
            string strOwnCode1 = SearchAndReplaceFrm.txtOwnCode.Text.Trim();
            string strOwnCode2 = SearchAndReplaceFrm.txtOwnCode1.Text.Trim();

            if (strBin.Length < 19)
            {
                MessageBox.Show("BIN to be replaced required.", "Search & Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                        
            if (SearchAndReplaceFrm.btnSearch.Text == "Search")
            {
                MessageBox.Show("BIN to be replaced required.", "Search & Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (strOwnCode2 == "")
            {
                MessageBox.Show("New owner code required.", "Search & Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Replace owner code of BIN " + strBin + " with " + strOwnCode2 + "?", "Search and Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pSet.Query = string.Format("update businesses set own_code = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where bin = '{0}'", strBin.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("update business_que set own_code = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where bin = '{0}'", strBin.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("update buss_hist set own_code = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where bin = '{0}'", strBin.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                MessageBox.Show("Owner's code of BIN: "+strBin.Trim()+" successfully replaced.", "Search and Replace", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string strObject = "";

                strObject = strBin + " Replaced own code from " + strOwnCode1;
                strObject += " to " + strOwnCode2;

                if (AuditTrail.InsertTrail("ABSRBR", "businesses", StringUtilities.HandleApostrophe(strObject)) == 0)
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (TaskMan.IsObjectLock(SearchAndReplaceFrm.bin1.GetBin(), "S&R", "DELETE", "ASS"))
                {
                }

                this.ClearFields(true,true);
                
            }
        }
    }
}
