//////////////////////////


// RMC 20110831 Enabled own_code searching in Replace Owner Accts
// RMC 20110831 corrected search and replace edit owner enlisting of new owner 
//////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.SearchBusiness;
using Amellar.Common.AuditTrail;
using Amellar.Common.SearchOwner;
using Amellar.Common.StringUtilities;
	
//Modified project by RMC 20110330

namespace Amellar.BPLS.SearchAndReplace
{
    public partial class frmSearchAndReplace : Form
    {
        SearchAndReplaceClass cSNR = new SearchAndReplaceClass();
        OracleResultSet result = new OracleResultSet();
        frmSearchBusiness frmSearchBns = new frmSearchBusiness();
        private SearchAndReplace SearchAndReplaceClass = null;
        public string m_sTrailDetail = string.Empty;

        /*
         * window states
         * 0 = Edit Owner's info by BIN searching
         * 1 = Replace owner of BIN searched
         * 2 = Replace owner of BINs attached to own_code searched
         
         */

        public int iWindowState = 0;
        public bool bControlState = false;
        //public string m_strOwnCode1, m_strOwnCode2;

        public frmSearchAndReplace()
        {
            InitializeComponent();

            //bin1.GetLGUCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.LGUCode);
            //bin1.GetDistCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.DistCode);

            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;

            bin2.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin2.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sButtonState = string.Empty;

            sButtonState = btnSearch.Text.Trim().ToString();
            if (sButtonState == "Clear")
            {
                btnSearch.Text = "Search";
                SearchAndReplaceClass.ClearFields(true,false);
                WindowState(0, false);
                //WindowState(0);

                if (TaskMan.IsObjectLock(bin1.GetBin(), m_sTrailDetail, "DELETE", "ASS"))
                {
                }
            }
            else
            {
                if (iWindowState == 2)
                {
                    // RMC 20110831 Enabled own_code searching in Replace Owner Accts
                    if (txtOwnCode.Text.Trim() != "")
                    {
                        GetValues(txtOwnCode.Text.Trim(), true);
                    }
                    else // RMC 20110831 Enabled own_code searching in Replace Owner Accts
                    {
                        frmSearchOwner SearchOwner = new frmSearchOwner();
                        SearchOwner.m_sPageWatch = "PAGE2"; // RMC 20150808 modified search and replace accounts
                        SearchOwner.ShowDialog();
                        txtLn.Text = SearchOwner.m_sOwnLn;
                        txtFn.Text = SearchOwner.m_sOwnFn;
                        txtMi.Text = SearchOwner.m_sOwnMi;
                        txtAdd.Text = SearchOwner.m_sOwnAdd;
                        txtSt.Text = SearchOwner.m_sOwnStreet;
                        txtBrgy.Text = SearchOwner.m_sOwnBrgy;
                        txtMun.Text = SearchOwner.m_sOwnMun;
                        txtProv.Text = SearchOwner.m_sOwnProv;
                        txtOwnCode.Text = SearchOwner.m_strOwnCode.Trim();
                        txtZip.Text = SearchOwner.m_sOwnZip.Trim(); // RMC 20110414
                    }

                    if (txtOwnCode.Text != "")
                    {
                        btnSearch.Text = "Clear";
                        WindowState(1, true);   // RMC 20110725
                    }
                }
                else
                {
                    // RMC 20110831 Enabled own_code searching in Replace Owner Accts
                    if (txtOwnCode.Text.Trim() != "")
                    {
                        GetValues(txtOwnCode.Text.Trim(), true);
                    }
                    else // RMC 20110831 Enabled own_code searching in Replace Owner Accts
                    {
                        if (bin1.txtTaxYear.Text == "" || bin1.txtBINSeries.Text == "")
                        {
                            frmSearchBns = new frmSearchBusiness();
                            frmSearchBns.ShowDialog();
                            if (frmSearchBns.sBIN.Length > 1)
                            {
                                bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                                bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();

                                if (!TaskMan.IsObjectLock(bin1.GetBin(), m_sTrailDetail, "ADD", "ASS"))
                                {
                                    GetValues(bin1.GetBin());
                                }
                                else
                                {
                                    bin1.txtTaxYear.Text = "";
                                    bin1.txtBINSeries.Text = "";
                                    SearchAndReplaceClass.ClearFields(true, false);
                                }
                            }
                        }
                        else
                            GetValues(bin1.GetBin());
                        //WindowState(0, true);
                    }

                    if (bin1.txtTaxYear.Text != "" && bin1.txtBINSeries.Text != "")
                        btnSearch.Text = "Clear";
                }
            }
        }

        private void GetValues(string sBIN)
        {
            cSNR.EditOwner(sBIN);
            if (cSNR.bState)
            {
                txtOwnCode.Text = cSNR.m_sOwnCode;
                txtAdd.Text = cSNR.m_sAdd;
                txtLn.Text = cSNR.m_sLN;
                txtFn.Text = cSNR.m_sFN;
                txtMi.Text = cSNR.m_sMI;
                txtBrgy.Text = cSNR.m_sBrgy;
                txtMun.Text = cSNR.m_sMun;
                txtProv.Text = cSNR.m_sProv;
                txtZone.Text = cSNR.m_sZone;
                txtSt.Text = cSNR.m_sStreet;
                txtZip.Text = cSNR.m_sZip;  // RMC 20110414

                if (iWindowState == 3)
                    WindowState(3, true);
                else if (iWindowState == 1) // RMC 20110725
                    WindowState(1, true);   // RMC 20110725
                else
                    WindowState(0, true);
                
            }
            else
            {
                MessageBox.Show("No File Found.");
                SearchAndReplaceClass.ClearFields(true,true);
                bin1.txtTaxYear.Focus();
                btnSearch.Text = "Search";
                WindowState(0, false);
                //WindowState(0);
            }

        }

        

        private void frmSearchAndReplace_Load(object sender, EventArgs e)
        {
            SearchAndReplaceClass = new SearchAndReplace(this);

            if (iWindowState == 0)
                SearchAndReplaceClass = new EditAcct(this);
            else if (iWindowState == 1)
                SearchAndReplaceClass = new BINReplaceAcct(this);
            else if (iWindowState == 2)
                SearchAndReplaceClass = new ReplaceAcct(this);
            
            WindowState(iWindowState);
            
        }

        private void WindowState(int iState, bool bControlState)
        {
            if (iState == 0)
            {
                txtAdd.Enabled = bControlState;
                //bin1.Enabled = bControlState;
                txtLn.Enabled = bControlState;
                txtFn.Enabled = bControlState;
                txtMi.Enabled = bControlState;
                txtBrgy.Enabled = bControlState;
                txtSt.Enabled = bControlState;
                txtZone.Enabled = bControlState;
                txtMun.Enabled = bControlState;
                txtProv.Enabled = bControlState;
                txtZip.Enabled = bControlState; // RMC 20110414
            }
            if (iState == 1)
            {
                txtAdd1.Enabled = bControlState;
                //bin2.Enabled = bControlState;
                txtLn1.Enabled = bControlState;
                txtFn1.Enabled = bControlState;
                txtMi1.Enabled = bControlState;
                txtBrgy1.Enabled = bControlState;
                txtSt1.Enabled = bControlState;
                txtZone1.Enabled = bControlState;
                txtMun1.Enabled = bControlState;
                txtProv1.Enabled = bControlState;
                btnSearch1.Enabled = bControlState;
                txtZip1.Enabled = bControlState; // RMC 20110414
            }
            
            
        }
        private void WindowState(int iState)
        {

            if (iState == 0)
            {
                bin1.Enabled = true;
                btnSearch1.Enabled = false;
                txtOwnCode.Enabled = true;    // RMC 20110831 Enabled own_code searching in Replace Owner Accts
            }
            if (iState == 1)
            {
                bin1.Enabled = true;
                bin2.Enabled = true;
            }
            if (iState == 2)
            {
                bin1.Enabled = false;
                txtOwnCode.Enabled = true;    // RMC 20110831 Enabled own_code searching in Replace Owner Accts
                txtOwnCode1.Enabled = true; // RMC 20110831 Enabled own_code searching in Replace Owner Accts
            }
            if (iState == 3)
            {
                bin1.Enabled = true;
                bin1.txtTaxYear.Focus();
                this.ActiveControl = bin1.txtTaxYear;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (iWindowState == 0)
            {
                // RMC 20110831 corrected search and replace edit owner enlisting of new owner (s)
                if (txtLn.Text.Trim() == "")
                {
                    MessageBox.Show("Last name required.", "Search and Replace", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                // RMC 20110831 corrected search and replace edit owner enlisting of new owner (e)

                //cSNR.m_sBIN = bin1.GetBin().ToString();
                cSNR.m_sBIN = txtOwnCode.Text.Trim();   // RMC 20110902 changed, pass owncode instead of bin                
                cSNR.m_sLN = txtLn.Text.Trim();   // RMC 20110831 added trim
                cSNR.m_sFN = txtFn.Text.Trim();   // RMC 20110831 added trim
                cSNR.m_sMI = txtMi.Text.Trim();   // RMC 20110831 added trim
                cSNR.m_sAdd = txtAdd.Text.Trim();   // RMC 20110831 added trim
                cSNR.m_sStreet = txtSt.Text.Trim();   // RMC 20110831 added trim
                cSNR.m_sBrgy = txtBrgy.Text.Trim();   // RMC 20110831 added trim
                cSNR.m_sZone = txtZone.Text.Trim();   // RMC 20110831 added trim
                cSNR.m_sDist = txtDist.Text.Trim(); // AST 20150417
                cSNR.m_sMun = txtMun.Text.Trim();   // RMC 20110831 added trim
                cSNR.m_sProv = txtProv.Text.Trim();   // RMC 20110831 added trim                                
                cSNR.m_sZip = txtZip.Text.Trim();   // RMC 20110831 added trim
                cSNR.m_sOwnCode = txtOwnCode.Text.Trim();   // RMC 20110902 
                cSNR.UpdateOwner();

                string strObject = "";

                strObject = "Edited owner information of code" + txtOwnCode.Text.Trim();

                if (AuditTrail.InsertTrail("ABSROE", "own_names", StringUtilities.HandleApostrophe(strObject)) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (TaskMan.IsObjectLock(bin1.GetBin(), "S&R", "DELETE", "ASS"))
                {
                }

                this.Close();
            }
            else
            {
                // RMC 20110725 (S)
                if (txtOwnCode1.Text.Trim() == "")
                {
                    txtOwnCode1.Text = AppSettingsManager.EnlistOwner(txtLn1.Text.Trim(), txtFn1.Text.Trim(),txtMi1.Text.Trim(),txtAdd1.Text.Trim(),txtSt1.Text.Trim(),txtDist1.Text.Trim(), txtZone1.Text.Trim(),txtBrgy1.Text.Trim(),txtMun1.Text.Trim(),txtProv1.Text.Trim(),txtZip1.Text.Trim());
                }
                // RMC 20110725 (E)



                SearchAndReplaceClass.Update();
                                
            }
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            string sButtonState = string.Empty;

            sButtonState = btnSearch1.Text.Trim().ToString();
            if (sButtonState == "Clear")
            {
                btnSearch1.Text = "Search";
                SearchAndReplaceClass.ClearFields(false,true);
                WindowState(0, false);
                //WindowState(0);
            }
            else
            {
                // RMC 20110831 Enabled own_code searching in Replace Owner Accts
                if (txtOwnCode.Text.Trim() != "")
                {
                    GetValues(txtOwnCode1.Text.Trim(), false);
                }
                else // RMC 20110831 Enabled own_code searching in Replace Owner Accts
                {
                    frmSearchOwner SearchOwner = new frmSearchOwner();
                    SearchOwner.m_sPageWatch = "PAGE2"; // RMC 20150808 modified search and replace accounts
                    SearchOwner.ShowDialog();
                    txtLn1.Text = SearchOwner.m_sOwnLn;
                    txtFn1.Text = SearchOwner.m_sOwnFn;
                    txtMi1.Text = SearchOwner.m_sOwnMi;
                    txtAdd1.Text = SearchOwner.m_sOwnAdd;
                    txtSt1.Text = SearchOwner.m_sOwnStreet;
                    txtBrgy1.Text = SearchOwner.m_sOwnBrgy;
                    txtMun1.Text = SearchOwner.m_sOwnMun;
                    txtProv1.Text = SearchOwner.m_sOwnProv;
                    txtOwnCode1.Text = SearchOwner.m_strOwnCode.Trim();
                    txtZip1.Text = SearchOwner.m_sOwnZip.Trim();    // RMC 20110414

                    if (txtOwnCode1.Text != "")
                        btnSearch1.Text = "Clear";
                }
            }
        }

        private void GetValues(string sOwnCode, bool bSwitch)
        {
            // RMC 20110831 Enabled own_code searching in Replace Owner Accts

            OracleResultSet pOwnCode = new OracleResultSet();

            pOwnCode.Query = "select * from own_names where own_code = '" + sOwnCode.Trim() + "'";
            /*pOwnCode.Query += "and (own_code in (select own_code from businesses) ";
            pOwnCode.Query += "or own_code in (select own_code from business_que) ";
            pOwnCode.Query += "or own_code in (select own_code from btm_temp_businesses) "; // GDE 20120507 added as per eric
            pOwnCode.Query += "or own_code in (select own_code from btm_businesses)) "; // GDE 20120507 added as per eric
             */ // RMC 20150808 modified search and replace accounts, put rem
            if (pOwnCode.Execute())
            {
                if (pOwnCode.Read())
                {
                    if (bSwitch)
                    {
                        txtLn.Text = pOwnCode.GetString("own_ln");
                        txtFn.Text = pOwnCode.GetString("own_fn");
                        txtMi.Text = pOwnCode.GetString("own_mi");
                        txtAdd.Text = pOwnCode.GetString("own_house_no");
                        txtSt.Text = pOwnCode.GetString("own_street");
                        txtBrgy.Text = pOwnCode.GetString("own_brgy");
                        txtMun.Text = pOwnCode.GetString("own_mun");
                        txtProv.Text = pOwnCode.GetString("own_prov");
                        txtZone.Text = pOwnCode.GetString("own_zone");
                        txtDist.Text = pOwnCode.GetString("own_dist");
                        txtZip.Text = pOwnCode.GetString("own_zip");
                        btnSearch.Text = "Clear";

                        if (iWindowState == 2)
                            WindowState(1, true);
                        else if (iWindowState == 0)
                            WindowState(0, true);
                    }
                    else
                    {
                        txtLn1.Text = pOwnCode.GetString("own_ln");
                        txtFn1.Text = pOwnCode.GetString("own_fn");
                        txtMi1.Text = pOwnCode.GetString("own_mi");
                        txtAdd1.Text = pOwnCode.GetString("own_house_no");
                        txtSt1.Text = pOwnCode.GetString("own_street");
                        txtBrgy1.Text = pOwnCode.GetString("own_brgy");
                        txtMun1.Text = pOwnCode.GetString("own_mun");
                        txtProv1.Text = pOwnCode.GetString("own_prov");
                        txtZone1.Text = pOwnCode.GetString("own_zone");
                        txtDist1.Text = pOwnCode.GetString("own_dist");

                        btnSearch1.Text = "Clear";
                    }

                    
                }
                else
                {
                    MessageBox.Show("No record found.", "Search and Replace", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    if (bSwitch)
                        SearchAndReplaceClass.ClearFields(true, false);
                    else
                        SearchAndReplaceClass.ClearFields(false, true);
                    return;
                }
            }
            pOwnCode.Close();

        }

        /// <summary>
        /// AST 20150417 Added this event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSearchAndReplace_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to close?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                btnSearch.Text = "Clear";
                btnSearch.PerformClick();
            }
        }
        
    }
}