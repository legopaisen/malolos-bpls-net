////////////////////

// RMC 20110919 added validation to prevent logout
// RMC 20110810 added OLD BIN in search Engine
// RMC 20110725 branch link-up 
// RMC 20110414 Modified searching

////////////////////



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

namespace Amellar.Common.SearchBusiness
{
    public partial class frmSearchBusiness : Form
    {
        OracleResultSet pSet = new OracleResultSet();
        OracleResultSet pSet2 = new OracleResultSet();
        public string sBIN = string.Empty;
        private string m_strModule = string.Empty;
        public bool m_bOusiteManilaSW = false;  // RMC 20110725 
        public string sOwnCode = string.Empty; //MCR 20140721
        public string ModuleCode
        {
            get { return m_strModule; }
            set { m_strModule = value; }
        }

        public frmSearchBusiness()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            sBIN = "";  // RMC 20110810
            this.Close();
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            btnClearFields.Enabled = false;
            btnSearch.Enabled = true;
            ClearFields(0);
        }
        private void ClearFields(int iCmd)
        {
            if (iCmd == 0)
            {
                txtBnsAdd.Text = "";
                txtBnsName.Text = "";
                txtFName.Text = "";
                txtLName.Text = "";
                txtMI.Text = "";
                txtPermit.Text = "";
                txtTaxYear.Text = "";
                txtOldBin.Text = "";    // RMC 20110810
                txtPlate.Text = "";
                dgvResult.Rows.Clear();
                txtBnsName.Focus();
                sBIN = "";  // RMC 20170103 added refresh of grid values in search business
            }
            else if (iCmd == 1)
            {
                txtBnsAdd.Text = "";
                txtBnsName.Text = "";
                txtFName.Text = "";
                txtLName.Text = "";
                txtMI.Text = "";
                txtPermit.Text = "";
                txtTaxYear.Text = "";
                txtOldBin.Text = "";    // RMC 20110810
                txtPlate.Text = "";
                txtBnsName.Focus();
                sBIN = "";  // RMC 20170103 added refresh of grid values in search business
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnClearFields.Enabled = true;
            btnSearch.Enabled = false;
            
            dgvResult.Rows.Clear(); // RMC 20170103 added refresh of grid values in search business
            GetValues();    
        }

        private void GetValues()
         {
            OracleResultSet mSet = new OracleResultSet();
            string sMiddleName = ""; //JARS 20170904
            int iSpl = 0;
            string sBnsName = "", sLName = "", sFName = "", sMI = "", sTaxYear = "", sPermit = "", sOwnCode = "";
            if (txtBnsName.Text.Trim().Length < 1)
                sBnsName = "%";
            else
                sBnsName = txtBnsName.Text.Trim() + "%";    // RMC 20110414 added %

            if (txtLName.Text.Trim().Length < 1)
                sLName = "%";
            else
                sLName = txtLName.Text.Trim() + "%";    // RMC 20110414 added %

            if (txtFName.Text.Trim().Length < 1)
                sFName = "%";
            else
                sFName = txtFName.Text.Trim() + "%";    // RMC 20110414 added %

            if (txtMI.Text.Trim().Length < 1)
                sMI = "%";
            else
                sMI = txtMI.Text.Trim() + "%";    // RMC 20110414 added %

            if (txtTaxYear.Text.Trim().Length < 1)
                sTaxYear = "%";
            else
                sTaxYear = txtTaxYear.Text.Trim() + "%";    // RMC 20110414 added %

            if (txtPermit.Text.Trim().Length < 1)
                sPermit = "%";
            else
                sPermit = txtPermit.Text.Trim() + "%";    // RMC 20110414 added %
            #region comments
            /*pSet.Query = "select * from own_names where own_ln like '" + sLName + "' and own_fn like '" + sFName + "' and own_mi like '" + sMI + "' order by own_code asc";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sOwnCode = pSet.GetString("own_code").ToString();
                    sLName = pSet.GetString("own_ln").ToString();
                    sFName = pSet.GetString("own_fn").ToString();
                    sMI = pSet.GetString("own_MI").ToString();

                    if (sOwnCode.Trim().Length < 1)
                        sOwnCode = "%";

                    if (m_strModule == "NEW-APP-EDIT" || m_strModule == "NEW-APP-VIEW"
                        || m_strModule == "REN-APP-EDIT" || m_strModule == "REN-APP-VIEW"
                        || m_strModule == "CANCEL-APP")
                        pSet2.Query = "select * from business_que where bns_nm like '" + sBnsName + "' and own_code like '" + sOwnCode + "' and tax_year like '" + sTaxYear + "' and permit_no like '" + sPermit + "' order by bin asc";
                    else
                        pSet2.Query = "select * from businesses where bns_nm like '" + sBnsName + "' and own_code like '" + sOwnCode + "' and tax_year like '" + sTaxYear + "' and permit_no like '" + sPermit + "' order by bin asc";
                    if (pSet2.Execute())
                    {
                        while (pSet2.Read())
                        {
                            dgvResult.Rows.Add(pSet2.GetString("bin").Trim(), pSet2.GetString("bns_nm").Trim(),
                                pSet2.GetString("Permit_no").Trim(), pSet2.GetString("tax_year").Trim(),
                                sLName, sFName, sMI, sOwnCode);
                        }
                    }
                    pSet2.Close();
                }
            }
            pSet.Close();
            */
            // RMC 20110414 PUT REM
            #endregion
            // RMC 20110810 added OLD BIN in search Engine (s)
            if (txtOldBin.Text.Trim() != "")
            {
                string sBIN = "";
                //pSet.Query = "select * from ref_no_tbl where old_bin like '" + txtOldBin.Text.ToString() + "%'";
                pSet.Query = "select * from app_permit_no where app_no like '" + txtOldBin.Text.ToString() + "%'"; //MCR 20150114
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sBIN = pSet.GetString("bin");
                        sBnsName = AppSettingsManager.GetBnsName(sBIN);
                    }
                }
                pSet.Close();

            }

            // RMC 20110810 added OLD BIN in search Engine (s)
            if (txtPlate.Text.Trim() != "")
            {
                string sBIN = "";
                pSet.Query = "select * from buss_plate where bns_plate like '" + txtPlate.Text.ToString() + "%'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sBIN = pSet.GetString("bin");
                        sBnsName = AppSettingsManager.GetBnsName(sBIN);
                    }
                }
                pSet.Close();

            }
            // RMC 20110810 added OLD BIN in search Engine (e)

            // RMC 20110725 branch link-up (s)
            if (m_bOusiteManilaSW)
            {
                pSet.Query = "select * from main_ofc_tbl where";
                pSet.Query += " nvl(trim(bns_nm),' ') like '" + StringUtilities.StringUtilities.HandleApostrophe(sBnsName) + "'";   // RMC 20110808 ADDED HANDLE
                pSet.Query += " and nvl(trim(owner),' ') like '" + StringUtilities.StringUtilities.HandleApostrophe(sLName) + "'";  // RMC 20110808 ADDED HANDLE
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        dgvResult.Rows.Add(pSet.GetString("mid").Trim(), pSet.GetString("bns_nm").Trim(),
                            "", "", pSet.GetString("owner").Trim(), "", "", "");
                    }
                }
                pSet.Close();
                return;
            }
            // RMC 20110725 branch link-up (e)


            // RMC 20110414 Modified searching (s)
            pSet.Query = "select a.own_code, a.own_ln, a.own_fn, a.own_mi, ";
            pSet.Query += " b.bin, b.bns_nm, b.permit_no, b.tax_year ";
            //pSet.Query += ", c.bns_plate "; // RMC 20171122 added display of business plate in search engine  // RMC 20171122 added display of business plate in search engine  // RMC 20171128 correction in search business engine if buss plate is null, put rem
            pSet.Query += " from own_names a, ";
            if (m_strModule == "NEW-APP-EDIT" || m_strModule == "NEW-APP-VIEW"
               || m_strModule == "REN-APP-EDIT" || m_strModule == "REN-APP-VIEW"
               || m_strModule == "CANCEL-APP" || m_strModule == "BILLING")  // RMC 20150426 QA corrections
                pSet.Query += " business_que b ";
            else
                pSet.Query += " businesses b ";
            //pSet.Query += ", buss_plate c ";    // RMC 20171122 added display of business plate in search engine  // RMC 20171128 correction in search business engine if buss plate is null, put rem
            pSet.Query += " where nvl(trim(b.bns_nm),' ') like '" +  StringUtilities.StringUtilities.HandleApostrophe(sBnsName) + "'";  // RMC 20110808 ADDED HANDLE
            pSet.Query += " and nvl(trim(b.tax_year),' ') like '" + sTaxYear + "'";
            pSet.Query += " and nvl(trim(b.permit_no),' ') like '" + sPermit + "'";
            pSet.Query += " and nvl(trim(a.own_ln),' ') like '" + StringUtilities.StringUtilities.HandleApostrophe(sLName) + "'";   // RMC 20110808 ADDED HANDLE
            pSet.Query += " and nvl(trim(a.own_fn),' ') like '" + sFName + "'";
            pSet.Query += " and nvl(trim(a.own_mi),' ') like '" + sMI + "'";
            pSet.Query += " and a.own_code = b.own_code";
            //pSet.Query += " and b.bin = c.bin ";    // RMC 20171122 added display of business plate in search engine      // RMC 20171122 added display of business plate in search engine  // RMC 20171128 correction in search business engine if buss plate is null, put rem
            // RMC 20110725 (s)
            if (m_strModule == "NEW-APP-EDIT")
                pSet.Query += " and bns_stat = 'NEW'";
            if (m_strModule == "REN-APP-EDIT")
                pSet.Query += " and bns_stat = 'REN'";
            // RMC 20110725 (e)
            pSet.Query += " order by bin asc";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iSpl = 1;

                    //JARS 20170904 (S)
                    if(pSet.GetString("own_mi") != "")
                    {
                        mSet.Query = "select middle_name from own_profile where own_code = '"+ pSet.GetString("own_code") +"'";
                        if(mSet.Execute())
                        {
                            if (mSet.Read())
                            {
                                sMiddleName = mSet.GetString("middle_name");
                            }
                            else
                            {
                                sMiddleName = pSet.GetString("own_MI").Trim();
                            }
                        }
                        mSet.Close();
                    }
                    //dgvResult.Rows.Add(pSet.GetString("bin").Trim(), pSet.GetString("bns_nm").Trim(),
                    //    pSet.GetString("Permit_no").Trim(), pSet.GetString("tax_year").Trim(),
                    //    pSet.GetString("own_ln").Trim(), pSet.GetString("own_fn").Trim(),
                    //    pSet.GetString("own_MI").Trim(), pSet.GetString("own_code").Trim());

                    dgvResult.Rows.Add(pSet.GetString("bin").Trim(), pSet.GetString("bns_nm").Trim(),
                        pSet.GetString("Permit_no").Trim(), pSet.GetString("tax_year").Trim(),
                        pSet.GetString("own_ln").Trim(), pSet.GetString("own_fn").Trim(),
                        sMiddleName, pSet.GetString("own_code").Trim(),
                        //pSet.GetString("bns_plate"));   // RMC 20171122 added display of business plate in search engine, put rem
                        AppSettingsManager.GetBnsPlate(pSet.GetString("bin").Trim()));  // RMC 20171128 correction in search business engine if buss plate is null
                    //JARS 20170904 (E)
                }
               
            }
            else
            {
                

            }
            pSet2.Close();
            // RMC 20110414 Modified searching (e)  

            if (iSpl == 0)
            {
                
                pSet.Query = "select a.own_code, a.own_ln, a.own_fn, a.own_mi, ";
                pSet.Query += " b.bin, b.bns_nm, b.permit_no, b.tax_year ";
                pSet.Query += " from own_names a, ";
                pSet.Query += " spl_business_que b ";
                pSet.Query += " where nvl(trim(b.bns_nm),' ') like '" + StringUtilities.StringUtilities.HandleApostrophe(sBnsName) + "'";  // RMC 20110808 ADDED HANDLE
                pSet.Query += " and nvl(trim(b.tax_year),' ') like '" + sTaxYear + "'";
                pSet.Query += " and nvl(trim(b.permit_no),' ') like '" + sPermit + "'";
                pSet.Query += " and nvl(trim(a.own_ln),' ') like '" + StringUtilities.StringUtilities.HandleApostrophe(sLName) + "'";   // RMC 20110808 ADDED HANDLE
                pSet.Query += " and nvl(trim(a.own_fn),' ') like '" + sFName + "'";
                pSet.Query += " and nvl(trim(a.own_mi),' ') like '" + sMI + "'";
                pSet.Query += " and a.own_code = b.own_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        dgvResult.Rows.Add(pSet.GetString("bin").Trim(), pSet.GetString("bns_nm").Trim(),
                            pSet.GetString("Permit_no").Trim(), pSet.GetString("tax_year").Trim(),
                            pSet.GetString("own_ln").Trim(), pSet.GetString("own_fn").Trim(),
                            pSet.GetString("own_MI").Trim(), pSet.GetString("own_code").Trim());
                    }

                }
                pSet2.Close();
            }


            // RMC 20110810 (s)
            if (dgvResult.Rows.Count == 0)
            {
                MessageBox.Show("No record found.", "Search Engine", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnClearFields.Enabled = false;
                btnSearch.Enabled = true;
                ClearFields(0);
                return;
            }
            // RMC 20110810 (e)
            else
                dgvResult.Focus();  // RMC 20170103 added refresh of grid values in search business
        }

        private void dgvResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ClearFields(1);
            sBIN =  dgvResult.SelectedRows[0].Cells[0].Value.ToString();
            txtBnsName.Text = dgvResult.SelectedRows[0].Cells[1].Value.ToString();
            txtLName.Text = dgvResult.SelectedRows[0].Cells[4].Value.ToString();
            txtMI.Text = dgvResult.SelectedRows[0].Cells[6].Value.ToString();
            txtFName.Text = dgvResult.SelectedRows[0].Cells[5].Value.ToString();
            txtPermit.Text = dgvResult.SelectedRows[0].Cells[2].Value.ToString();
            txtTaxYear.Text = dgvResult.SelectedRows[0].Cells[3].Value.ToString();
            sOwnCode = dgvResult.SelectedRows[0].Cells[7].Value.ToString();// MCR 20140721

            // RMC 20110414 (s)
            txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(sBIN);
            // RMC 20110414 (e)
            txtPlate.Text = dgvResult.SelectedRows[0].Cells[8].Value.ToString();    // RMC 20171122 added display of business plate in search engine

            GetOldBIN(sBIN);    // RMC 20110810
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvResult.SelectedRows[0].Cells[0].Value != null)    // RMC 20110919 added validation to prevent logout
                {
                    sBIN = dgvResult.SelectedRows[0].Cells[0].Value.ToString();
                    sOwnCode = dgvResult.SelectedRows[0].Cells[7].Value.ToString();// MCR 20140721
                } 
                this.Close();
            }
            catch { return; }
        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ClearFields(1);
                sBIN = dgvResult.SelectedRows[0].Cells[0].Value.ToString();
                txtBnsName.Text = dgvResult.SelectedRows[0].Cells[1].Value.ToString();
                txtLName.Text = dgvResult.SelectedRows[0].Cells[4].Value.ToString();
                txtMI.Text = dgvResult.SelectedRows[0].Cells[6].Value.ToString();
                txtFName.Text = dgvResult.SelectedRows[0].Cells[5].Value.ToString();
                txtPermit.Text = dgvResult.SelectedRows[0].Cells[2].Value.ToString();
                txtTaxYear.Text = dgvResult.SelectedRows[0].Cells[3].Value.ToString();
                sOwnCode = dgvResult.SelectedRows[0].Cells[7].Value.ToString();// MCR 20140721

                // RMC 20110414 (s)
                txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(sBIN);
                // RMC 20110414 (e)
                txtPlate.Text = dgvResult.SelectedRows[0].Cells[8].Value.ToString();    // RMC 20171122 added display of business plate in search engine

                GetOldBIN(sBIN);    // RMC 20110810
            }
            catch { return; }
        }

        private void GetOldBIN(string sBIN)
        {
            // RMC 20110810

            OracleResultSet pRec = new OracleResultSet();

            //pRec.Query = "select * from ref_no_tbl where bin = '" + sBIN + "'";
            pRec.Query = "select * from app_permit_no where bin = '" + sBIN + "'"; //MCR 20150114
            if (pRec.Execute())
            {
                if (pRec.Read())
                    //txtOldBin.Text = pRec.GetString("old_bin"); //MCR 20150114
                    txtOldBin.Text = pRec.GetString("app_no");
                else
                    txtOldBin.Text = "";
            }
            pRec.Close();
        }

        private void frmSearchBusiness_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("test");
        }

        private void txtBnsName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // RMC 20161228 enable in search engine when 'Enter' was pressed
            
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                    btnSearch_Click(sender, e);
            
        }

        private void txtLName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // RMC 20161228 enable in search engine when 'Enter' was pressed
            
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                    btnSearch_Click(sender, e);
            
        }

        private void txtFName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // RMC 20161228 enable in search engine when 'Enter' was pressed
            
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                    btnSearch_Click(sender, e);
            
        }

        private void txtTaxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            // RMC 20161228 enable in search engine when 'Enter' was pressed
           
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                    btnSearch_Click(sender, e);
            
        }

       
        

        
    }
}