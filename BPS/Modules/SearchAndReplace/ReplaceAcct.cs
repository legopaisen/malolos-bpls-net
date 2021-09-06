using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;

namespace Amellar.BPLS.SearchAndReplace
{
    class ReplaceAcct:SearchAndReplace
    {
        public ReplaceAcct(frmSearchAndReplace Form)
            : base(Form)
        {
        }

        public override void Update()
        {
            string strOwnCode1 = SearchAndReplaceFrm.txtOwnCode.Text.Trim();
            string strOwnCode2 = SearchAndReplaceFrm.txtOwnCode1.Text.Trim();

            if (strOwnCode1 == "")
            {
                MessageBox.Show("Owner's Code to be replaced required.", "Search and Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SearchAndReplaceFrm.btnSearch.Text == "Search")
            {
                MessageBox.Show("Owner's information required!", "Search and Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (strOwnCode2 == "")
            {
                MessageBox.Show("New owner code required.", "Search & Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Replace owner's code " + strOwnCode1 + " with " + strOwnCode2 + "?", "Search and Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pSet.Query = string.Format("update businesses set own_code = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where own_code = '{0}'", strOwnCode1);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("update business_que set own_code = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where own_code = '{0}'", strOwnCode1);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("update buss_hist set own_code = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where own_code = '{0}'", strOwnCode1);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                // RMC 20150808 modified search and replace accounts (s)
                pSet.Query = string.Format("update businesses set busn_own = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where busn_own = '{0}'", strOwnCode1);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("update business_que set busn_own = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where busn_own = '{0}'", strOwnCode1);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("update buss_hist set busn_own = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where busn_own = '{0}'", strOwnCode1);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("update businesses set prev_bns_own = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where prev_bns_own = '{0}'", strOwnCode1);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("update business_que set prev_bns_own = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where prev_bns_own = '{0}'", strOwnCode1);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("update buss_hist set prev_bns_own = '{0}'", strOwnCode2);
                pSet.Query += string.Format("where prev_bns_own = '{0}'", strOwnCode1);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("select * from OWN_PROFILE where own_code = '{0}'", strOwnCode2);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        pSet.Close();
                        pSet.Query = string.Format("delete from OWN_PROFILE ");
                        pSet.Query += string.Format("where own_code = '{0}'", strOwnCode1);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                    else
                    {
                        pSet.Close();
                        pSet.Query = string.Format("update OWN_PROFILE set own_code = '{0}'", strOwnCode2);
                        pSet.Query += string.Format("where own_code = '{0}'", strOwnCode1);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                }
                pSet.Close();


                
                // RMC 20150808 modified search and replace accounts (e)

                MessageBox.Show("Owner code successfully replaced.", "Search and Replace", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string strObject = "";

                strObject = " Replaced own code " + strOwnCode1 + " to " + strOwnCode2;

                if (AuditTrail.InsertTrail("ABSROR", "businesses", StringUtilities.HandleApostrophe(strObject)) == 0)
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.ClearFields(true,true);
            }
        }
    }
}
