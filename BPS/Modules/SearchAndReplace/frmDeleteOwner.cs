

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;

namespace Amellar.BPLS.SearchAndReplace
{
    public partial class frmDeleteOwner : Form
    {
        
        public frmDeleteOwner()
        {
            InitializeComponent();
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();
            int iCnt = 0;
            int iTotalCnt = 0;
            int iGTotalCnt = 0;
            string sOwnCode = "";
            string sGTotalCnt = "";

            if (MessageBox.Show("Delete owner accounts not used?", "Delete Owner", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            pRec.Query = "select * from own_names order by own_code";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sOwnCode = pRec.GetString("own_code").Trim();

                    iTotalCnt = 0;

                    //	BNS_LOC_LAST	LAST_BUSN_OWN
                    //	BNS_LOC_LAST	LAST_OWN_CODE
                    //	BNS_LOC_LAST	LAST_PREV_BNS_OWN
                    
                    pCmd.Query = string.Format("select count(*) from BNS_LOC_LAST where LAST_BUSN_OWN = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BNS_LOC_LAST where LAST_OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BNS_LOC_LAST where LAST_PREV_BNS_OWN = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	BPLS_DELQ_TABLE	BPLS_OWN_CODE
                    pCmd.Query = string.Format("select count(*) from BPLS_DELQ_TABLE where BPLS_OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	BPLS_LEVY	BPLS_OWN_CODE
                    //	BPLS_LEVY	RPTA_OWN_CODE
                    pCmd.Query = string.Format("select count(*) from BPLS_LEVY where BPLS_OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BPLS_LEVY where RPTA_OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	BUSINESSES	BUSN_OWN
                    //	BUSINESSES	OWN_CODE
                    //	BUSINESSES	PREV_BNS_OWN

                    pCmd.Query = string.Format("select count(*) from BUSINESSES where BUSN_OWN = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BUSINESSES where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BUSINESSES where PREV_BNS_OWN = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	BUSINESS_QUE	BUSN_OWN
                    //	BUSINESS_QUE	OWN_CODE
                    //	BUSINESS_QUE	PREV_BNS_OWN

                    pCmd.Query = string.Format("select count(*) from BUSINESS_QUE where BUSN_OWN = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BUSINESS_QUE where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BUSINESS_QUE where PREV_BNS_OWN = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	BUSINESS_TMP	BUSN_OWN
                    //	BUSINESS_TMP	OWN_CODE
                    //	BUSINESS_TMP	PREV_BNS_OWN

                    pCmd.Query = string.Format("select count(*) from BUSINESS_TMP where BUSN_OWN = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BUSINESS_TMP where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BUSINESS_TMP where PREV_BNS_OWN = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	BUSS_HIST	BUSN_OWN
                    //	BUSS_HIST	OWN_CODE
                    //	BUSS_HIST	PREV_BNS_OWN

                    pCmd.Query = string.Format("select count(*) from BUSS_HIST where BUSN_OWN = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BUSS_HIST where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from BUSS_HIST where PREV_BNS_OWN = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	CONFISCATE	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from CONFISCATE where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	DBCR_MEMO	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from DBCR_MEMO where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	DISTRAINT_HIST	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from DISTRAINT_HIST where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	DISTRAINT_PROPERTY	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from DISTRAINT_PROPERTY where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //  LEVY	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from LEVY where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	MAIN_OFC_TBL	MID
                    pCmd.Query = string.Format("select count(*) from MAIN_OFC_TBL where MID = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	MULTI_CHECK_HIST	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from MULTI_CHECK_HIST where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	MULTI_CHECK_PAY	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from MULTI_CHECK_PAY where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	NOREC_CLOSURE	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from NOREC_CLOSURE where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	NOTICE_SENT	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from NOTICE_SENT where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	NOTICE_SENT_HIST	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from NOTICE_SENT where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;
                    
                    //	OWN_PROFILE	OWN_CODE
                    /*pCmd.Query = string.Format("select count(*) from OWN_PROFILE where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);*/
                    // RMC 20151021 put rem, own profile record can be deleted as long as the owner has no business record

                    iTotalCnt = iTotalCnt + iCnt;

                    //	PERMIT_UPDATE_APPL	NEW_OWN_CODE
                    pCmd.Query = string.Format("select count(*) from PERMIT_UPDATE_APPL where NEW_OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	PERMIT_UPDATE_APPL	OLD_OWN_CODE
                    pCmd.Query = string.Format("select count(*) from PERMIT_UPDATE_APPL where OLD_OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	REP_WARRANT_LEVY	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from REP_WARRANT_LEVY where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	TMP_ADVNOTICE	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from TMP_ADVNOTICE where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	TMP_DBCR	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from TMP_DBCR where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	TRANSFER_HIST	NEW_OWN_CODE
                    //	TRANSFER_HIST	PREV_OWN_CODE
                    pCmd.Query = string.Format("select count(*) from TRANSFER_HIST where NEW_OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from TRANSFER_HIST where PREV_OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	TRANSFER_TABLE	NEW_OWN_CODE
                    //	TRANSFER_TABLE	PREV_OWN_CODE
                    pCmd.Query = string.Format("select count(*) from TRANSFER_TABLE where NEW_OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    pCmd.Query = string.Format("select count(*) from TRANSFER_TABLE where PREV_OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;

                    //	UNOFFICIAL_INFO_TBL	OWN_CODE
                    pCmd.Query = string.Format("select count(*) from UNOFFICIAL_INFO_TBL where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    //business mapping
                    pCmd.Query = string.Format("select count(*) from btm_businesses where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    pCmd.Query = string.Format("select count(*) from btm_temp_businesses where OWN_CODE = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    pCmd.Query = string.Format("select count(*) from btm_businesses where busn_own = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    pCmd.Query = string.Format("select count(*) from btm_temp_businesses where busn_own = '{0}'", sOwnCode);
                    int.TryParse(pCmd.ExecuteScalar(), out iCnt);

                    iTotalCnt = iTotalCnt + iCnt;


                    if (iTotalCnt == 0)
                    {
                        pCmd.Query = string.Format("insert into own_names_hist (select * from own_names where own_code = '{0}')", sOwnCode);
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        pCmd.Query = string.Format("delete from own_names where own_code = '{0}'", sOwnCode);
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        iGTotalCnt++;
                    }
                }

                if (iGTotalCnt > 0)
                {
                    sGTotalCnt = string.Format("{0:###}", iGTotalCnt);

                    MessageBox.Show("DONE \nTotal records deleted: " + sGTotalCnt);

                    if (AuditTrail.InsertTrail("ABSROQ", "own_names", "Delete Owner module") == 0)
                    {
                        pCmd.Rollback();
                        pCmd.Close();
                        MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("No record deleted");
                    this.Close();
                }
            }
        }

        
        
    }
}