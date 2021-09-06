// ALJ 20130219 NEW Discovery - delinquent
// RMC 20120320 Added deletiong in buss_plate in cancel application
// RMC 20120316 corrected deletion of billing tables when application was edited
// RMC 20120314 added validation of buss plate in business records
// RMC 20120207 modifications in business mapping
// RMC 20120127 delete record in billing-related tables in cancel appilication transaction
// RMC 20120126 added deletion in ass_taxdues when application cancelled
// RMC 20120116 added handleapostrophe in memoranda businesses
// RMC 20120103 added modification in saving other_info
// RMC 20111221 added validation of duplicate entries in other_info table
// RMC 20111220 transferred validation of business if business-mapped in AppSettingsManager
// RMC 20111216 consider business-mapped-unofficial in cancel application transaction
// RMC 20111128 consider Business mapping - unencoded/undeclared in Business Records-Add/Applications-New
// RMC 20111014 modified query commit in Business Records transactions
// RMC 20111012 added BIN in auto-tagging of unknown lessor 
// RMC 20111007 Modified validation of duplicate permit no
// RMC 20111007 corrected enabling/disabling of save button in Business records module
// RMC 20110906 Added saving of Application requirements in Business Records
// RMC 20110901 Added validation if Lessor is a registered business
// RMC 20110831 added validation if with addl_info
// RMC 20110819 added capturing/viewing of Business Plate
// RMC 20110725 added toupper to dist & brgy

using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.BPLSApp;
using Amellar.Common.DataConnector;
using Amellar.Common.BusinessType;
using Amellar.Common.AppSettings;
using System.Windows.Forms;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;
using System.Data;

namespace Amellar.Common.frmBns_Rec
{
    class BackCodeClass
    {
        bool bAns = true;
        public string m_sPage1, m_sMessage, m_sBnsDesc, m_sPage2, m_sPage3;
        string m_sBinSerial = string.Empty;
        frmBusinessType frmBnsType = new frmBusinessType();
        frmBusinessRecord pageCheck = null;
        private SystemUser m_objSystemUser;
        BPLSAppSettingList sList = new BPLSAppSettingList();
        OracleResultSet pSet = new OracleResultSet();
        OracleResultSet result = new OracleResultSet();
        private string m_strModuleCode = string.Empty;
        private string m_strTableUpdated = string.Empty;
        BarangayList brgyList = new BarangayList();

        public BackCodeClass(frmBusinessRecord pageCheck)
        {
            this.pageCheck = pageCheck;
        }

        public string sTaxYear
        {
            get { return pageCheck.txtTaxYear.Text; }
        }


        public string sMP
        {
            get { return pageCheck.txtMPNo.Text; }
        }

        public string sBnsName
        {
            get { return pageCheck.txtBnsName.Text; }
        }

        private string m_sPlaceOccupancy = "";
        public string PlaceOccupancy
        {
            set { m_sPlaceOccupancy = value; }
        }

        
        private void GenerateSerial()
        {

        }
        public void PageOneCheck(string sFrmStat)
        {
            DateTime dtCurrent = AppSettingsManager.GetSystemDate();
            int intCurrentYear = dtCurrent.Year;

            if (sTaxYear.Trim().Length == 0 && (sFrmStat != "NEW-APP" || sFrmStat != "SPL-APP"))
            {
                m_sPage1 = "FALSE";
                pageCheck.txtTaxYear.Focus();
                m_sMessage = "Tax Year Required";
                return;
            }

            //if (!pageCheck.m_bOwnProfileEdited) //JARS 20170807 ADDED VALIDATION TO ENSURE POPULATION OF OWNER PROFILES TABLE
            //{
            //    if (sFrmStat != "NEW-APP-EDIT" && sFrmStat != "REN-APP-EDIT")
            //    {
            //        OracleResultSet pOwn = new OracleResultSet();
            //        pOwn.Query = "select * from own_profile where own_code = '" + pageCheck.txtBnsOwnCode.Text + "'";
            //        if (pOwn.Execute())
            //        {
            //            if (pOwn.Read())
            //            {
            //                m_sMessage = "Warning: Please verify that the Owner's ADDITIONAL INFO is updated.";
            //            }
            //            else
            //            {
            //                m_sMessage = "Warning: Owner's ADDITIONAL INFO does not exist, please fill up ADDITIONAL OWNER'S INFO in page ONE.";
            //            }
            //        }
            //        pOwn.Close();
            //        MessageBox.Show(m_sMessage, "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //}

            // (s) ALJ 20130219 NEW Discovery - delinquent
            if (pageCheck.NewDiscovery)
            {
                if (Convert.ToInt32(sTaxYear.Trim()) >= intCurrentYear)
                {
                    bAns = false;
                    m_sPage1 = "FALSE";
                    m_sMessage = "Invalid Tax Year! Should be less than current year.";
                    pageCheck.txtTaxYear.Focus();
                    return;
                }
                String sDtOperated = string.Format("{0:##}/{1:##}/{2:####}", 1, 1, pageCheck.txtTaxYear.Text);
                pageCheck.dtpOperationStart.Value = DateTime.Parse(sDtOperated);
            }
            // (e) ALJ 20130219 NEW Discovery - delinquent

            if ((Convert.ToInt32(sTaxYear.Trim()) > intCurrentYear) ||
                (sTaxYear.Trim().Length < 4 && (sFrmStat != "NEW-APP" || sFrmStat != "SPL-APP")))
            {
                bAns = false;
                m_sPage1 = "FALSE";
                m_sMessage = "Invalid Tax Year";
                pageCheck.txtTaxYear.Focus();
                return;
            }

            if (sFrmStat != "NEW-APP" && sFrmStat != "SPL-APP" && sFrmStat != "NEW-APP-EDIT" && sFrmStat != "SPL-APP-EDIT"
                && sFrmStat != "REN-APP" && sFrmStat != "REN-APP-EDIT"
                && sFrmStat != "CANCEL-APP" && sFrmStat != "BUSS-UPDATE")
            {
                if ((bAns = sList.StringCheck(sMP)) == false)
                {
                    m_sPage1 = "FALSE";
                    pageCheck.txtMPNo.Focus();
                    m_sMessage = "Mayor's Permit Required";
                    return;
                }

                DateTime odtPermit = DateTime.Parse(pageCheck.dtpMPDate.Text);

                if (Convert.ToInt32(pageCheck.txtMPYear.Text) != odtPermit.Year)
                {
                    bAns = false;
                    m_sPage1 = "FALSE";
                    m_sMessage = "Permit date must tally with Permit no.";
                    pageCheck.dtpMPDate.Focus();
                    return;
                }
            }

            if (!pageCheck.m_sFormStatus.Contains("SPL-APP")) //MCR 20140916 SPL-BNS
                if ((bAns = sList.StringCheck(sBnsName)) == false)
                {
                    m_sPage1 = "FALSE";
                    pageCheck.txtBnsName.Focus();
                    m_sMessage = "Business Name Required";
                    return;
                }

            OracleResultSet pRec = new OracleResultSet();

            if (pageCheck.m_strTmpBnsName != pageCheck.txtBnsName.Text.Trim())
                pageCheck.m_bBnsNmTrigger = true;

            if (sFrmStat == "REN-APP-ADD")
            {
                pageCheck.m_bBnsNmTrigger = false;
            }

            pRec.Query = "select * from businesses";
            pRec.Query += string.Format(" where bns_nm = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsName.Text.Trim()));
            pRec.Query += string.Format(" and bin <> '{0}'", pageCheck.bin1.GetBin());
            if (pRec.Execute())
            {
                if (pRec.Read() && pageCheck.m_bBnsNmTrigger)
                {
                    MessageBox.Show("Warning: Double business name", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    using (frmDouble_Buss frmDoubleBuss = new frmDouble_Buss())
                    {
                        frmDoubleBuss.txtBussName.Text = pageCheck.txtBnsName.Text.Trim();
                        frmDoubleBuss.ShowDialog();

                        if (frmDoubleBuss.txtBussAdd.Text.Trim() != "")
                        {
                            pageCheck.txtBnsAddNo.Text = frmDoubleBuss.m_sAdd;
                            pageCheck.txtBnsStreet.Text = frmDoubleBuss.m_sStreet;
                            pageCheck.cmbBnsBrgy.Text = frmDoubleBuss.m_sBrgy;
                        }
                    }

                    if (MessageBox.Show("Do you want to continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        pageCheck.txtBnsName.Focus();
                        m_sPage1 = "FALSE";
                        return;
                    }
                    else
                    {
                        pageCheck.m_strTmpBnsName = pageCheck.txtBnsName.Text.Trim();
                        pageCheck.m_bBnsNmTrigger = false;
                    }
                }
            }
            pRec.Close();

            if ((bAns = sList.StringCheck(pageCheck.txtBnsStreet.Text)) == false)
            {
                m_sPage1 = "FALSE";
                pageCheck.txtBnsStreet.Focus();
                m_sMessage = "Business Street Required";
                return;
            }
            if ((bAns = sList.StringCheck(pageCheck.cmbBnsBrgy.Text)) == false)
            {
                m_sPage1 = "FALSE";
                pageCheck.cmbBnsBrgy.Focus();
                m_sMessage = "Business Barangay Required";
                return;
            }
            if ((bAns = sList.StringCheck(pageCheck.txtBnsMun.Text.Trim())) == false)
            {
                m_sPage1 = "FALSE";
                pageCheck.txtBnsMun.Focus();
                m_sMessage = "Business Municipality/City Required";
                return;
            }
            if ((bAns = sList.StringCheck(pageCheck.cmbBnsOrgnKind.Text)) == false)
            {
                m_sPage1 = "FALSE";
                pageCheck.cmbBnsOrgnKind.Focus();
                m_sMessage = "Business Organization Required";
                return;
            }
            if ((bAns = sList.StringCheck(pageCheck.txtBnsType.Text)) == false)
            {
                m_sPage1 = "FALSE";
                pageCheck.txtBnsType.Focus();
                m_sMessage = "Business Type Required";
                return;
            }
            if ((bAns = sList.StringCheck(pageCheck.txtOwnLn.Text)) == false)
            {
                m_sPage1 = "FALSE";
                pageCheck.txtOwnLn.Focus();
                m_sMessage = "Owner's Last Name Required";
                return;
            }

            /*if (AppSettingsManager.bEnlistOwner(pageCheck.txtOwnLn.Text.Trim(), pageCheck.txtOwnFn.Text.Trim(), pageCheck.txtOwnAddNo.Text.Trim(), pageCheck.txtOwnStreet.Text.Trim(), pageCheck.cmbOwnBrgy.Text.Trim(), pageCheck.txtOwnMun.Text.Trim(), pageCheck.txtOwnProv.Text.Trim()))
                EnlistNewOwner();*/

            if (m_sPlaceOccupancy != "OWNED")
            {
                pageCheck.GetPage1OwnCode = AppSettingsManager.EnlistOwner(pageCheck.txtOwnLn.Text.Trim(), pageCheck.txtOwnFn.Text.Trim(), pageCheck.txtOwnMi.Text.Trim(), pageCheck.txtOwnAddNo.Text.Trim(), pageCheck.txtOwnStreet.Text.Trim(), pageCheck.cmbOwnDist.Text.Trim().ToUpper(), "", pageCheck.cmbOwnBrgy.Text.Trim().ToUpper(), pageCheck.txtOwnMun.Text.Trim(), pageCheck.txtOwnProv.Text.Trim(), pageCheck.txtOwnZip.Text.Trim());    // RMC 20110725 added toupper to dist & brgy
                pageCheck.txtOwnCode.Text = pageCheck.GetPage1OwnCode;
                UpdateOwnProfileCode(pageCheck.m_sTmpBuss_Own_Code, pageCheck.txtOwnCode.Text);
            }

            if (pageCheck.GetPage1OwnCode == "")
            {
                m_sPage1 = "FALSE";
                m_sMessage = "Owner's code required";
                return;
            }

            if (OnCheckIfRetiredBns() && (sFrmStat == "NEW-APP" || sFrmStat == "NEW-APP-EDIT" || sFrmStat == "SPL-APP" || sFrmStat == "SPL-APP-EDIT"))
            {
                m_sPage1 = "FALSE";
                m_sMessage = "Business already retired.";
                return;
            }

            if (!ValidateNewApplication(sFrmStat))
            {
                m_sPage1 = "FALSE";
                return;
            }

            if (!ValidateGIS(sFrmStat))
            {
                m_sPage1 = "FALSE";
                return;
            }


            pRec.Query = "select * from unofficial_info_tbl where trim(is_number) in ";
            pRec.Query += "(select trim(is_number) from norec_closure_tagging) ";
            pRec.Query += string.Format("and own_code = '{0}'", pageCheck.txtOwnCode.Text.Trim());
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    if (MessageBox.Show("One or more business of this owner was tagged for closure.\nDo you want to continue this application?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        m_sPage1 = "FALSE";
                        return;
                    }
                }
            }
            pRec.Close();

            pRec.Query = "select a.bin from businesses a,official_closure_tagging b ";
            pRec.Query += string.Format("where a.bin = b.bin and own_code = '{0}'", pageCheck.txtOwnCode.Text.Trim());
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    if (MessageBox.Show("One or more business of this owner was tagged for closure.\nDo you want to continue this application?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        m_sPage1 = "FALSE";
                        return;
                    }
                }
            }
            pRec.Close();

            if (sFrmStat == "BUSS-EDIT")
            {
                DateTime dtOperationStart = pageCheck.dtpOperationStart.Value;

                if (dtOperationStart.Year > Convert.ToInt32(pageCheck.txtTaxYear.Text.ToString()))
                {
                    string sConvert = "";

                    sConvert = string.Format("{0:##}/{1:##}/{2:####}", dtOperationStart.Month, dtOperationStart.Day, pageCheck.txtTaxYear.Text);

                    pageCheck.dtpOperationStart.Value = DateTime.Parse(sConvert);
                }
            }

            // RMC 20111007 Modified validation of duplicate permit no (s)
            if (sFrmStat == "BUSS-ADD-NEW" || sFrmStat == "BUSS-EDIT")
            {
                string strMP = "";
                string strRecBin = "";
                strMP = pageCheck.txtMPYear.Text.Trim() + "-" + pageCheck.txtMPNo.Text.Trim();

                pRec.Query = string.Format("select * from businesses where permit_no = '{0}'", strMP);
                if (pageCheck.bin1.GetBin().Length == 19)
                {
                    pRec.Query += string.Format(" and bin <> '{0}'", pageCheck.bin1.GetBin());
                }
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        strRecBin = pRec.GetString("bin");

                        
                        /*if (MessageBox.Show("Duplicate Permit Number with BIN: " + strRecBin + ".\n Continue Anyway?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            pageCheck.txtMPNo.Text = "";
                            pageCheck.txtMPNo.Focus();
                            pRec.Close();
                            m_sPage1 = "FALSE";
                            return;
                        }*/ // RMC 20170130 block record if duplicate permit no. detected

                        // RMC 20170130 block record if duplicate permit no. detected (s)
                        MessageBox.Show("Duplicate Permit Number with BIN: " + strRecBin + ".", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        pageCheck.txtMPNo.Text = "";
                        pageCheck.txtMPNo.Focus();
                        m_sPage1 = "FALSE";
                        return;
                        // RMC 20170130 block record if duplicate permit no. detected (e)
                    }

                }
                pRec.Close();


            }
            // RMC 20111007 Modified validation of duplicate permit no (e)

            // RMC 20120314 added validation of buss plate in business records (s)
            if (!ValidatePlateNo(pageCheck.txtBussPlate.Text.ToString().Trim(), pageCheck.bin1.GetBin()))
            {
                string sBin = GetDuplicatePlate(pageCheck.txtBussPlate.Text.ToString());
                m_sMessage = "Business Plate no. already issued to BIN: " + sBin + ".\nPlease check and edit business plate no.";
                m_sPage1 = "FALSE";
                pageCheck.txtBussPlate.Text = "";
                pageCheck.txtBussPlate.Focus();
                return;
            }
            // RMC 20120314 added validation of buss plate in business records (e)

            // RMC 20150615 (S)
            if (pageCheck.m_strBnsCode.Trim() == "")
            {
                MessageBox.Show("Please select business type","Business Records",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            // RMC 20150615 (E)

            pageCheck.btnSave.Enabled = false; // RMC 20111007 corrected enabling/disabling of save button in Business records module
        }

        public void PageTwoCheck()
        {
            //if (pageCheck.m_iState == 0 || pageCheck.cmbPlaceBnsStat.Text.Trim() == "OWNED")
            //if (pageCheck.m_iState == 0)  // RMC 20110901 put rem
            {
                if (pageCheck.cmbPlaceBnsStat.Text.Trim() == "OWNED")
                {
                    pageCheck.cmbPlaceBnsStat.Text = "OWNED";
                    pageCheck.txtBnsOwnAddNo.Text = pageCheck.txtOwnAddNo.Text;
                    pageCheck.txtBnsOwnCode.Text = pageCheck.txtOwnCode.Text;
                    pageCheck.txtBnsOwnLn.Text = pageCheck.txtOwnLn.Text;
                    pageCheck.txtBnsOwnFn.Text = pageCheck.txtOwnFn.Text;
                    pageCheck.txtBnsOwnMi.Text = pageCheck.txtOwnMi.Text;
                    pageCheck.txtBnsOwnMun.Text = pageCheck.txtOwnMun.Text;
                    pageCheck.txtBnsOwnProv.Text = pageCheck.txtOwnProv.Text;
                    pageCheck.txtBnsOwnStreet.Text = pageCheck.txtOwnStreet.Text;
                    pageCheck.cmbBnsOwnBrgy.Text = pageCheck.cmbOwnBrgy.Text;
                    pageCheck.cmbBnsOwnDist.Text = pageCheck.cmbOwnDist.Text;
                    pageCheck.txtBnsOwnZip.Text = pageCheck.txtOwnZip.Text;
                    ManageControls(2, true);

                    pageCheck.GetPage2OwnCode = pageCheck.GetPage1OwnCode;
                }
                else
                {
                    if ((bAns = sList.StringCheck(pageCheck.txtOwnLn.Text)) == false)
                    {
                        m_sPage2 = "FALSE";
                        pageCheck.txtOwnLn.Focus();
                        m_sMessage = "Business place owner's last name required!";
                        return;
                    }

                    //if (pageCheck.cmbPlaceBnsStat.Text.Trim() == "RENTED")
                    pageCheck.GetPage2OwnCode = AppSettingsManager.EnlistOwner(pageCheck.txtBnsOwnLn.Text.Trim(),
                        pageCheck.txtBnsOwnFn.Text.Trim(), pageCheck.txtBnsOwnMi.Text.Trim(),
                        pageCheck.txtBnsOwnAddNo.Text.Trim(), pageCheck.txtBnsOwnStreet.Text.Trim(),
                        pageCheck.cmbBnsDist.Text.Trim().ToUpper(), "", pageCheck.cmbBnsOwnBrgy.Text.Trim().ToUpper(),
                        pageCheck.txtBnsOwnMun.Text.Trim(), pageCheck.txtBnsOwnProv.Text.Trim(), pageCheck.txtBnsOwnZip.Text.Trim());    // RMC 20110725 added toupper to dist & brgy
                    pageCheck.txtBnsOwnCode.Text = pageCheck.GetPage2OwnCode;   // RMC 20110901 Added validation if Lessor is a registered business
                    UpdateOwnProfileCode(pageCheck.m_sTmpBuss_BusnCode, pageCheck.txtBnsOwnCode.Text);
                }

            }

            if (pageCheck.GetPage2OwnCode == "")
            {
                m_sPage2 = "FALSE";
                m_sMessage = "Business place owner's code required!";
                return;
            }

            if (pageCheck.m_sFormStatus != "BUSS-EDIT") // RMC 20140108
            {
                if (AppSettingsManager.GetConfigValue("65") == "Y") // AST 20150325 Added This Condition
                {
                    // RMC 20110831 added validation if with addl_info (s)
                    if (!ValidateAddlInfo())
                    {
                        m_sPage2 = "FALSE";
                        //m_sMessage = "Additional information required.";
                        m_sMessage = "Please update Additional information.";   // RMC 20111221 added validation of duplicate entries in other_info table
                        return;
                    }
                    // RMC 20110831 added validation if with addl_info (e)
                }
            }

            // RMC 20140104 Capturing of gender of employee (s)
            if (pageCheck.m_sFormStatus != "CANCEL-APP" && pageCheck.m_sFormStatus != "BUSS-UPDATE" 
                && pageCheck.m_sFormStatus != "BUSS-EDIT" && pageCheck.m_sFormStatus == "BUSS-ADD-NEW") // RMC 20150306 disabled validation of other business info for adding business record
            {
                if (AppSettingsManager.GetConfigValue("65") == "Y") // AST 20150325 Added This Condition
                {
                    if (!ValidateOtherInfo())
                    {
                        m_sPage2 = "FALSE";
                        return;
                    }
                }
            }
            // RMC 20140104 Capturing of gender of employee (e)

            pageCheck.btnSave.Enabled = false; // RMC 20111007 corrected enabling/disabling of save button in Business records module            
        }

        public void PageThreeCheck(bool blnValidate)
        {
            pageCheck.btnSave.Enabled = true;   // RMC 20111007 corrected enabling/disabling of save button in Business records module

            //if (pageCheck.m_iState == 0)  // RMC 20110901 put rem
            {
                pageCheck.txtPrevOwnAddNo.Text = pageCheck.txtOwnAddNo.Text;
                pageCheck.txtPrevOwnCode.Text = pageCheck.txtOwnCode.Text;
                pageCheck.txtPrevOwnLn.Text = pageCheck.txtOwnLn.Text;
                pageCheck.txtPrevOwnFn.Text = pageCheck.txtOwnFn.Text;
                pageCheck.txtPrevOwnMi.Text = pageCheck.txtOwnMi.Text;
                pageCheck.txtPrevOwnMun.Text = pageCheck.txtOwnMun.Text;
                pageCheck.txtPrevOwnProv.Text = pageCheck.txtOwnProv.Text;
                pageCheck.txtPrevOwnStreet.Text = pageCheck.txtOwnStreet.Text;
                pageCheck.cmbPrevOwnBrgy.Text = pageCheck.cmbOwnBrgy.Text;
                pageCheck.cmbPrevOwnDist.Text = pageCheck.cmbOwnDist.Text;
                pageCheck.txtPrevOwnZip.Text = pageCheck.txtOwnZip.Text;

                pageCheck.GetPage3OwnCode = AppSettingsManager.EnlistOwner(pageCheck.txtPrevOwnLn.Text.Trim(),
                    pageCheck.txtPrevOwnFn.Text.Trim(), pageCheck.txtPrevOwnMi.Text.Trim(),
                    pageCheck.txtPrevOwnAddNo.Text.Trim(), pageCheck.txtPrevOwnStreet.Text.Trim(),
                    pageCheck.cmbPrevOwnDist.Text.Trim().ToUpper(), "", pageCheck.cmbPrevOwnBrgy.Text.Trim().ToUpper(),
                    pageCheck.txtPrevOwnMun.Text.Trim(), pageCheck.txtPrevOwnProv.Text.Trim(), pageCheck.txtPrevOwnZip.Text.Trim());    // RMC 20110725 added toupper to dist & brgy

                pageCheck.txtPrevOwnCode.Text = pageCheck.GetPage3OwnCode;  // RMC 20110901 Added validation if Lessor is a registered business
                UpdateOwnProfileCode(pageCheck.m_sTmpBuss_Prev_BnsOwn, pageCheck.txtPrevOwnCode.Text);

                if (pageCheck.txtCurrentGross.Text == "")
                    pageCheck.txtCurrentGross.Text = "0.00";

                if (pageCheck.txtInitialCap.Text == "")
                    pageCheck.txtInitialCap.Text = "0.00";

                if (blnValidate)
                {
                    if ((bAns = sList.StringCheck(pageCheck.cmbBnsStat.Text.Trim())) == false)
                    {
                        m_sPage3 = "FALSE";
                        pageCheck.cmbBnsStat.Focus();
                        m_sMessage = "Business Status required";
                        return;
                    }

                    DateTime dtOperationStart = Convert.ToDateTime(pageCheck.dtpOperationStart.Value);

                    /* MCR 20141222 removed based on lubao request
                    //if (dtOperationStart > DateTime.Now)
                    if (dtOperationStart > AppSettingsManager.GetCurrentDate())    // RMC 20110725
                    {
                        m_sPage3 = "FALSE";
                        pageCheck.dtpOperationStart.Focus();
                        m_sMessage = "Invalid date of operation";
                        return;
                    }*/

                    /*if ((dtOperationStart.Month == DateTime.Now.Month) && (dtOperationStart.Day == DateTime.Now.Day)
                        && (dtOperationStart.Year == DateTime.Now.Year) && pageCheck.cmbBnsStat.Text.Trim() != "RETIRED")*/
                    if ((dtOperationStart.Month == AppSettingsManager.GetCurrentDate().Month) && (dtOperationStart.Day == AppSettingsManager.GetCurrentDate().Day)
                        && (dtOperationStart.Year == AppSettingsManager.GetCurrentDate().Year) && pageCheck.cmbBnsStat.Text.Trim() != "RETIRED")    // RMC 20110725
                    {
                        if (MessageBox.Show("Warning! Date of Operation is set to current date, Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            m_sPage3 = "FALSE";
                            pageCheck.dtpOperationStart.Focus();
                            return;
                        }
                    }

                    if (pageCheck.cmbBnsStat.Text.Trim() == "RETIRED")
                    {
                        if (pageCheck.txtMemo.Text.Trim() == "")
                        {
                            m_sPage3 = "FALSE";
                            pageCheck.txtMemo.Focus();
                            m_sMessage = "Reason of retirement required";
                            return;
                        }
                    }

                    if ((pageCheck.cmbBnsStat.Text.Trim() != "NEW") && Convert.ToDouble(pageCheck.txtCurrentGross.Text) == 0)
                    {
                        if (pageCheck.m_sRadioMain == "BRANCH")
                        {
                            if (MessageBox.Show("Warning! Zero gross due to consolidation. Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                m_sPage3 = "FALSE";
                                pageCheck.txtCurrentGross.Focus();
                                return;
                            }
                        }
                        else
                        {
                            if (pageCheck.m_sRadioMain == "MAIN")
                            {
                                m_sPage3 = "FALSE";
                                pageCheck.txtCurrentGross.Focus();
                                m_sMessage = "Main branch should declare the consolidated gross receipts";
                                return;
                            }
                            else
                            {
                                if (MessageBox.Show("Warning! Zero gross receipt detected. Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                {
                                    m_sPage3 = "FALSE";
                                    pageCheck.txtCurrentGross.Focus();
                                    return;
                                }
                            }

                        }
                    }

                    if (pageCheck.cmbBnsStat.Text.Trim() == "NEW" && Convert.ToDouble(pageCheck.txtInitialCap.Text.Trim()) == 0)
                    {
                        if (MessageBox.Show("Warning! Zero initial capital detected. Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            m_sPage3 = "FALSE";
                            pageCheck.txtInitialCap.Focus();
                            return;
                        }
                    }
                }

                if (pageCheck.m_sFormStatus != "REN-APP" && pageCheck.m_sFormStatus != "CANCEL-APP")
                    ManageControls(3, true);

            }

            if (pageCheck.GetPage3OwnCode == "")
            {
                m_sPage3 = "FALSE";
                m_sMessage = "Previous owner's code required!";
                return;
            }

            // RMC 20111007 corrected enabling/disabling of save button in Business records module (s)
            if ((pageCheck.m_sFormStatus == "BUSS-ADD-NEW" || pageCheck.m_sFormStatus == "NEW-APP" || pageCheck.m_sFormStatus == "SPL-APP")
                && pageCheck.bin1.GetBin().Length == 19)
                pageCheck.btnSave.Enabled = false;
            else
                pageCheck.btnSave.Enabled = true;
            // RMC 20111007 corrected enabling/disabling of save button in Business records module (e)
        }

        /*private void EnlistNewOwner()
        {
            string sFn = string.Empty;
            string sMi = string.Empty;
            string sHouseNo = string.Empty;
            string sBrgy = string.Empty;
            string sProv = string.Empty;
            string sStreet = string.Empty;
            string sMun = string.Empty;

            Serials.UpdateOwnSerial();
            string strOwnCode = Serials.GetOwnSerial();
            OracleResultSet result = new OracleResultSet();

            sFn = pageCheck.txtOwnFn.Text.Trim();
            if (pageCheck.txtOwnFn.Text.Trim() == null)
                sFn = " ";
                           
            sMi = pageCheck.txtOwnMi.Text.Trim();
            if (pageCheck.txtOwnMi.Text.Trim() == null)
                sMi = " ";

            sHouseNo = pageCheck.txtOwnAddNo.Text.Trim();
            if (pageCheck.txtOwnAddNo.Text.Trim() == null)
                sHouseNo = " ";

            sBrgy = pageCheck.cmbOwnBrgy.Text.Trim();     
            if (pageCheck.cmbOwnBrgy.Text.Trim() == null)
                sBrgy = " ";

            sProv = pageCheck.txtOwnProv.Text.Trim();
            if (pageCheck.txtOwnProv.Text.Trim() == null)
                sProv = " ";

            sStreet = pageCheck.txtOwnStreet.Text.Trim();
            if (pageCheck.txtOwnStreet.Text.Trim() == null)
                sStreet = " ";

            sMun = pageCheck.txtOwnMun.Text.Trim();
            if (pageCheck.txtOwnMun.Text.Trim() == null)
                sMun = " ";

            result.Query = string.Format("insert into own_names values ('{0}','{1}','{2}','{3}','{4}','{5}',' ',' ','{6}','{7}','{8}',' ')", strOwnCode.Trim(), pageCheck.txtOwnLn.Text.Trim(), sFn, sMi, sHouseNo, sStreet, sBrgy, sMun, sProv);
            if(result.ExecuteNonQuery() != 0)
            {
                MessageBox.Show("Owner Saved");
            }
            result.Close();
        }*/

        public void ManageControls(int iGroup, bool bState)
        {
            if (iGroup == 0)
            {
                pageCheck.bin1.Enabled = bState;
            }
            if (iGroup == 1)
            {
                pageCheck.txtOldBin.ReadOnly = !bState;
                pageCheck.txtTelNo.ReadOnly = !bState;
                pageCheck.dtpMPDate.Enabled = bState;
                pageCheck.txtBnsName.ReadOnly = !bState;
                pageCheck.txtBnsAddNo.ReadOnly = !bState;
                pageCheck.txtBnsStreet.ReadOnly = !bState;
                pageCheck.txtBnsType.ReadOnly = !bState;
                pageCheck.txtBnsZone.ReadOnly = !bState;
                pageCheck.cmbBnsDist.Enabled = bState;
                pageCheck.cmbBnsOrgnKind.Enabled = bState;
                pageCheck.cmbBnsBrgy.Enabled = bState;
                pageCheck.txtBussPlate.ReadOnly = !bState;   // RMC 20110819 added capturing/viewing of Business Plate

                if (pageCheck.m_sFormStatus != "REN-APP-EDIT")
                {
                    pageCheck.txtMPNo.ReadOnly = !bState;
                    pageCheck.txtTaxYear.ReadOnly = !bState;
                }

                // RMC 20170130 disable editing of tax year in if business has current year payment (s)
                if (pageCheck.m_sFormStatus == "BUSS-EDIT")
                {
                    OracleResultSet pTmp = new OracleResultSet();

                    pTmp.Query = "select * from pay_hist where bin = '" + pageCheck.bin1.GetBin() + "' and tax_year = '" + pageCheck.txtTaxYear.Text + "' and data_mode <> 'UNP'";
                    if (pTmp.Execute())
                    {
                        if (pTmp.Read())
                        {
                            pageCheck.txtTaxYear.ReadOnly = true;
                            // RMC 20180118 enable editing of MP if it has no value (s)
                            if (pageCheck.txtMPNo.Text == "")
                                pageCheck.txtMPNo.ReadOnly = false;
                            // RMC 20180118 enable editing of MP if it has no value (e)
                            else
                            pageCheck.txtMPNo.ReadOnly = true;
                        }
                    }
                    pTmp.Close();
                }
                // RMC 20170130 disable editing of tax year in if business has current year payment (e)

                pageCheck.btnBnsType.Enabled = bState;
                pageCheck.btnBnsTypeAll.Enabled = bState;
                pageCheck.txtOwnLn.ReadOnly = !bState;
                pageCheck.txtOwnFn.ReadOnly = !bState;
                pageCheck.txtOwnMi.ReadOnly = !bState;
                pageCheck.btnSearchOwner.Enabled = bState;
                pageCheck.txtOwnAddNo.ReadOnly = !bState;
                pageCheck.txtOwnStreet.ReadOnly = !bState;
                pageCheck.txtOwnMun.ReadOnly = !bState;
                pageCheck.txtOwnProv.ReadOnly = !bState;
                pageCheck.txtPINLand.ReadOnly = !bState;
                pageCheck.btnSearchLandPIN.Enabled = bState;
                pageCheck.txtPINBldg.ReadOnly = !bState;
                pageCheck.btnSearchBldgPIN.Enabled = bState;
                pageCheck.txtPINAddNo.ReadOnly = !bState;
                pageCheck.txtPINStreet.ReadOnly = !bState;
                pageCheck.txtPINMun.ReadOnly = !bState;
                pageCheck.txtPINProv.ReadOnly = !bState;
                pageCheck.cmbPINBrgy.Enabled = bState;
                pageCheck.cmbPINDist.Enabled = bState;
                pageCheck.cmbOwnBrgy.Enabled = bState;
                pageCheck.cmbOwnDist.Enabled = bState;
                pageCheck.txtEmailAdd.ReadOnly = !bState;   // RMC 20110803

            }
            if (iGroup == 2)
            {
                if (pageCheck.cmbPlaceBnsStat.Text.Trim() == "OWNED")
                {
                    pageCheck.txtSinceWhen.ReadOnly = true;
                    pageCheck.txtMonthlyRental.ReadOnly = true;
                    pageCheck.txtBnsOwnLn.ReadOnly = true;
                    pageCheck.txtBnsOwnFn.ReadOnly = true;
                    pageCheck.txtBnsOwnMi.ReadOnly = true;
                    pageCheck.txtBnsOwnAddNo.ReadOnly = true;
                    pageCheck.txtBnsOwnMun.ReadOnly = true;
                    pageCheck.txtBnsOwnProv.ReadOnly = true;
                    pageCheck.txtBnsOwnStreet.ReadOnly = true;
                    pageCheck.cmbBnsOwnBrgy.Enabled = false;
                    pageCheck.cmbBnsOwnDist.Enabled = false;
                    pageCheck.txtBnsOwnZip.ReadOnly = true;
                    pageCheck.ButtonBnsOwnSearch.Enabled = false;
                }
                else
                {
                    pageCheck.txtSinceWhen.ReadOnly = !bState;
                    pageCheck.txtMonthlyRental.ReadOnly = !bState;
                    pageCheck.txtBnsOwnLn.ReadOnly = !bState;
                    pageCheck.txtBnsOwnFn.ReadOnly = !bState;
                    pageCheck.txtBnsOwnMi.ReadOnly = !bState;
                    pageCheck.txtBnsOwnAddNo.ReadOnly = !bState;
                    pageCheck.txtBnsOwnMun.ReadOnly = !bState;
                    pageCheck.txtBnsOwnProv.ReadOnly = !bState;
                    pageCheck.txtBnsOwnStreet.ReadOnly = !bState;
                    pageCheck.cmbBnsOwnBrgy.Enabled = bState;
                    pageCheck.cmbBnsOwnDist.Enabled = bState;
                    pageCheck.txtBnsOwnZip.ReadOnly = !bState;
                    pageCheck.ButtonBnsOwnSearch.Enabled = bState;
                }
                pageCheck.cmbPlaceBnsStat.Enabled = bState;
                //pageCheck.txtNoEmp.ReadOnly = !bState;
                pageCheck.txtNoEmp.ReadOnly = true; // RMC 20140109 disabled no. of employees in page 2, since this is already included in additional info
                pageCheck.txtNoProf.ReadOnly = !bState;
                pageCheck.txtAnnualPayroll.ReadOnly = !bState;
                pageCheck.txtElecBill.Enabled = bState;
                pageCheck.txtTelBill.ReadOnly = !bState;
                pageCheck.txtWaterBill.ReadOnly = !bState;
                pageCheck.txtOtherExpenses.ReadOnly = !bState;

                if (pageCheck.m_sFormStatus != "REN-APP" && pageCheck.m_sFormStatus != "REN-APP-EDIT")  // RMC 20110824 added "REN-APP-EDIT"
                    pageCheck.btnAddlBnsInfo.Enabled = bState;

                pageCheck.txtNoDelivery.ReadOnly = !bState;
                pageCheck.txtNoStorey.ReadOnly = !bState;
                pageCheck.txtTotFloorArea.ReadOnly = !bState;
                pageCheck.txtNoMac.ReadOnly = !bState;
                pageCheck.txtGroundArea.ReadOnly = !bState;
                pageCheck.txtBldgValue.ReadOnly = !bState;
            }

            if (iGroup == 3)
            {
                pageCheck.txtPrevOwnAddNo.ReadOnly = !bState;
                pageCheck.txtPrevOwnLn.ReadOnly = !bState;
                pageCheck.txtPrevOwnFn.ReadOnly = !bState;
                pageCheck.txtPrevOwnMi.ReadOnly = !bState;
                pageCheck.txtPrevOwnMun.ReadOnly = !bState;
                pageCheck.txtPrevOwnProv.ReadOnly = !bState;
                pageCheck.txtPrevOwnStreet.ReadOnly = !bState;
                pageCheck.cmbPrevOwnBrgy.Enabled = bState;
                pageCheck.cmbPrevOwnDist.Enabled = bState;
                pageCheck.txtPrevOwnZip.ReadOnly = !bState;

                pageCheck.btnPrevOwnSearch.Enabled = bState;
                pageCheck.txtDTI.ReadOnly = !bState;
                pageCheck.txtSSSNo.ReadOnly = !bState;
                pageCheck.txtCTCNo.ReadOnly = !bState;
                pageCheck.dtpDTIIssued.Enabled = bState;
                pageCheck.dtpSSSIssued.Enabled = bState;
                pageCheck.dtpCTCNoIssued.Enabled = bState;
                pageCheck.txtCTCNoIssuedAt.ReadOnly = !bState;
                pageCheck.chkWithBrgyClearance.Enabled = bState;
                pageCheck.txtMemo.ReadOnly = !bState;

                if (pageCheck.m_sFormStatus != "NEW-APP" && pageCheck.m_sFormStatus != "NEW-APP-EDIT" && pageCheck.m_sFormStatus != "SPL-APP" && pageCheck.m_sFormStatus != "SPL-APP-EDIT"
                    && pageCheck.m_sFormStatus != "REN-APP-EDIT")
                    pageCheck.cmbBnsStat.Enabled = bState;

                if (pageCheck.cmbBnsStat.Text == "RENEWAL")
                {
                    if (pageCheck.m_sFormStatus == "BUSS-ADD-NEW" || pageCheck.m_sFormStatus == "BUSS-EDIT")  // RMC 20150108
                        pageCheck.txtInitialCap.ReadOnly = false;   // RMC 20150108
                    else
                        pageCheck.txtInitialCap.ReadOnly = true;

                    pageCheck.txtInitialCap.ReadOnly = true;
                }
                else if (pageCheck.cmbBnsStat.Text == "NEW")
                    pageCheck.txtCurrentGross.ReadOnly = true;

                pageCheck.dtpOperationStart.Enabled = bState;
                //pageCheck.txtCurrentGross.ReadOnly = !bState; //JARS 20170726 REM
                pageCheck.btnLinkUp.Enabled = bState;
                pageCheck.txtTINNo.ReadOnly = !bState;  // RMC 20110803
                pageCheck.dtpTINNoIssued.Enabled = bState;  // RMC 20110803
                pageCheck.dtpApplicationDate.Enabled = bState;  // RMC 20110803

            }

        }

        public string GetBnsDesc(string sBnsCode)
        {
            m_sBnsDesc = "";    // RMC 20110311
            pSet.Query = "SELECT BNS_DESC FROM BNS_TABLE ";
            pSet.Query += "WHERE BNS_CODE = '" + sBnsCode + "' ";
            pSet.Query += "AND FEES_CODE = 'B' AND LENGTH(BNS_CODE) > 2";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sBnsDesc = pSet.GetString("bns_desc").Trim();
                }
                else
                {
                    pSet.Query = "SELECT BNS_DESC FROM BNS_TABLE ";
                    pSet.Query += "WHERE BNS_CODE like '" + sBnsCode + "%' ";
                    pSet.Query += "AND FEES_CODE = 'B'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            frmBnsType.SetFormStyle(false);
                            frmBnsType.txtBnsCode.Text = sBnsCode;
                            frmBnsType.SearchCodeDesc();
                            frmBnsType.ShowDialog();
                            m_sBnsDesc = frmBnsType.txtBnsDesc.Text.Trim();
                            pageCheck.m_strBnsCode = frmBnsType.txtBnsCode.Text;    // RMC 20150615
                        }
                    }
                }
            }

            pSet.Close();
            return m_sBnsDesc;
        }

        public void ReturnValuesByBin(string sBIN)
        {
            string sPermit, sBnsCode, sOwnCode;
            string sMPDate, sCTCDate, sSSSDate, sDTIDate, sRentDate, sStartOperated, sCancDate, sSaveTmDate;

            sList.ModuleCode = pageCheck.m_sFormStatus;
            sList.ReturnValueByBin = sBIN;
            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                // GDE 20090212 page 1 values (s)
                pageCheck.txtTaxYear.Text = sList.BPLSAppSettings[i].sTaxYear;

                //if (pageCheck.m_sFormStatus == "BUSS-UPDATE" || pageCheck.m_sFormStatus == "REN-APP")
                if (pageCheck.m_sFormStatus == "BUSS-UPDATE")   // RMC 20140113 corrections in application if with prev year delinq
                    pageCheck.txtTaxYear.Text = string.Format("{0:000#}", Convert.ToInt32(sList.BPLSAppSettings[i].sTaxYear) + 1);

                // RMC 20140113 corrections in application if with prev year delinq (s)
                if (pageCheck.m_sFormStatus == "REN-APP")
                    pageCheck.txtTaxYear.Text = string.Format("{0:000#}", Convert.ToInt32(ConfigurationAttributes.CurrentYear));
                // RMC 20140113 corrections in application if with prev year delinq (e)

                sPermit = sList.BPLSAppSettings[i].sPermitNo;
                if (sPermit.Trim() != "")
                {
                    try
                    {
                        pageCheck.txtMPYear.Text = sPermit.Substring(0, 4);
                        pageCheck.txtMPNo.Text = sPermit.Substring(5, 5);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    pageCheck.txtMPNo.Text = string.Empty;
                    pageCheck.txtMPYear.Text = string.Empty;
                }
                pageCheck.dtpMPDate.Text = sList.BPLSAppSettings[i].sPermitDate;
                pageCheck.txtTelNo.Text = sList.BPLSAppSettings[i].sBnsTelNo;
                pageCheck.txtBnsName.Text = StringUtilities.StringUtilities.RemoveApostrophe(sList.BPLSAppSettings[i].sBnsNm);
                pageCheck.txtBnsAddNo.Text = sList.BPLSAppSettings[i].sBnsHouseNo;
                pageCheck.txtBnsStreet.Text = sList.BPLSAppSettings[i].sBnsStreet;
                pageCheck.cmbBnsBrgy.Text = sList.BPLSAppSettings[i].sBnsBrgy;
                pageCheck.txtBnsZone.Text = sList.BPLSAppSettings[i].sBnsZone;
                pageCheck.txtBnsMun.Text = sList.BPLSAppSettings[i].sBnsMun;
                pageCheck.txtBnsProv.Text = sList.BPLSAppSettings[i].sBnsProv;
                pageCheck.cmbBnsOrgnKind.Text = sList.BPLSAppSettings[i].sOrgnKind;
                pageCheck.txtEmailAdd.Text = sList.BPLSAppSettings[i].m_sEmail;     // RMC 20110803

                // GDE 20090213 searching bns_code desc (s){
                sBnsCode = sList.BPLSAppSettings[i].sBnsCode;
                pageCheck.m_strBnsCode = sBnsCode;
                pageCheck.m_strTmpBnsCode = sBnsCode;

                OracleResultSet resultBnsCode = new OracleResultSet();
                resultBnsCode.Query = "SELECT * FROM BNS_TABLE WHERE BNS_CODE = :1 AND FEES_CODE = :2";
                resultBnsCode.AddParameter(":1", sBnsCode);
                resultBnsCode.AddParameter(":2", "B");
                if (resultBnsCode.Execute())
                {
                    if (resultBnsCode.Read())
                    {
                        pageCheck.txtBnsType.Text = resultBnsCode.GetString("bns_desc").Trim();
                    }
                }
                resultBnsCode.Close();
                // GDE 20090213 searching bns_code desc (e)}                    

                sOwnCode = sList.BPLSAppSettings[i].sOwnCode;
                sList.OwnName = sOwnCode;
                pageCheck.GetPage1OwnCode = sOwnCode;
                pageCheck.GetPage2OwnCode = sList.BPLSAppSettings[i].sBussOwn;
                pageCheck.GetPage3OwnCode = sList.BPLSAppSettings[i].sPrevBnsOwn;

                // GDE 20090212 Owner's Information (s){
                for (int j = 0; j < sList.OwnNamesSetting.Count; j++)
                {
                    pageCheck.txtOwnCode.Text = sList.OwnNamesSetting[j].sOwnerCode;
                    pageCheck.txtOwnLn.Text = sList.OwnNamesSetting[j].sLn;
                    pageCheck.txtOwnFn.Text = sList.OwnNamesSetting[j].sFn;
                    pageCheck.txtOwnMi.Text = sList.OwnNamesSetting[j].sMi;
                    pageCheck.txtOwnAddNo.Text = sList.OwnNamesSetting[j].sOwnHouseNo;
                    pageCheck.txtOwnStreet.Text = sList.OwnNamesSetting[j].sOwnStreet;
                    pageCheck.cmbOwnBrgy.Text = sList.OwnNamesSetting[j].sOwnBrgy;
                    pageCheck.cmbOwnDist.Text = sList.OwnNamesSetting[j].sOwnDist;
                    pageCheck.txtOwnMun.Text = sList.OwnNamesSetting[j].sOwnMun;
                    pageCheck.txtOwnProv.Text = sList.OwnNamesSetting[j].sOwnProv;
                    pageCheck.txtOwnZip.Text = sList.OwnNamesSetting[j].sOwnZip;
                }
                // GDE 20090212 Owner's Information (e)}

                pageCheck.txtPINLand.Text = sList.BPLSAppSettings[i].sLandPin;
                pageCheck.txtPINBldg.Text = sList.BPLSAppSettings[i].sBldgPin;
                pageCheck.txtPINAddNo.Text = sList.BPLSAppSettings[i].sPoffHouseNo;
                pageCheck.txtPINStreet.Text = sList.BPLSAppSettings[i].sPoffStreet;
                pageCheck.cmbPINBrgy.Text = sList.BPLSAppSettings[i].sPoffbrgy;
                pageCheck.cmbPINDist.Text = sList.BPLSAppSettings[i].sPoffDist;
                pageCheck.txtPINMun.Text = sList.BPLSAppSettings[i].sPoffMun;
                pageCheck.txtPINProv.Text = sList.BPLSAppSettings[i].sPoffProv;

                pageCheck.m_sOrNo = sList.BPLSAppSettings[i].sOrNo;
                pageCheck.m_sCancelReason = sList.BPLSAppSettings[i].sCancReason;
                pageCheck.m_sCancelDate = sList.BPLSAppSettings[i].sCancDate;
                pageCheck.m_sCancelBy = sList.BPLSAppSettings[i].sCancBy;
                pageCheck.m_sBnsUser = sList.BPLSAppSettings[i].sBnsUser;
                pageCheck.m_sBnsZip = sList.BPLSAppSettings[i].sBnsZip;


                // GDE 20090212 page 1 values (e)}

                // GDE 20090212 page 2 values (s){
                if (sList.BPLSAppSettings[i].sPlaceOccupancy.Trim() == string.Empty)
                    pageCheck.cmbPlaceBnsStat.Text = "OWNED";
                else
                    pageCheck.cmbPlaceBnsStat.Text = sList.BPLSAppSettings[i].sPlaceOccupancy;
                pageCheck.txtSinceWhen.Text = sList.BPLSAppSettings[i].sRentLeaseSince;
                pageCheck.txtMonthlyRental.Text = string.Format("{0:#,##.00}", Convert.ToDouble(sList.BPLSAppSettings[i].sRentLeaseMo));

                pageCheck.txtNoEmp.Text = sList.BPLSAppSettings[i].sNumEmployees;
                pageCheck.txtNoProf.Text = sList.BPLSAppSettings[i].sNumProfessional;
                pageCheck.txtAnnualPayroll.Text = sList.BPLSAppSettings[i].sAnnualWages;
                pageCheck.txtElecBill.Text = sList.BPLSAppSettings[i].sAveElectricBill;
                pageCheck.txtWaterBill.Text = sList.BPLSAppSettings[i].sAveWaterBill;
                pageCheck.txtTelBill.Text = sList.BPLSAppSettings[i].sAvePhoneBill;
                pageCheck.txtOtherExpenses.Text = sList.BPLSAppSettings[i].sOtherUtil;
                pageCheck.txtNoDelivery.Text = sList.BPLSAppSettings[i].sNumDelivVehicle;
                pageCheck.txtNoStorey.Text = sList.BPLSAppSettings[i].sNumStorey;
                pageCheck.txtTotFloorArea.Text = sList.BPLSAppSettings[i].sTotFlrArea;
                pageCheck.txtNoMac.Text = sList.BPLSAppSettings[i].sNumMachineries;
                pageCheck.txtGroundArea.Text = sList.BPLSAppSettings[i].sFlrArea;
                pageCheck.txtBldgValue.Text = sList.BPLSAppSettings[i].sBldgVal;


                sList.OwnName = sList.BPLSAppSettings[i].sBussOwn;

                // copied form page 1 - verify info (s){
                // GDE 20090212 Owner's Information (s){
                for (int j = 0; j < sList.OwnNamesSetting.Count; j++)
                {
                    pageCheck.txtBnsOwnCode.Text = sList.OwnNamesSetting[j].sOwnerCode;
                    pageCheck.txtBnsOwnLn.Text = sList.OwnNamesSetting[j].sLn;
                    pageCheck.txtBnsOwnFn.Text = sList.OwnNamesSetting[j].sFn;
                    pageCheck.txtBnsOwnMi.Text = sList.OwnNamesSetting[j].sMi;
                    pageCheck.txtBnsOwnAddNo.Text = sList.OwnNamesSetting[j].sOwnHouseNo;
                    pageCheck.txtBnsOwnStreet.Text = sList.OwnNamesSetting[j].sOwnStreet;
                    pageCheck.cmbBnsOwnBrgy.Text = sList.OwnNamesSetting[j].sOwnBrgy;
                    pageCheck.cmbBnsOwnDist.Text = sList.OwnNamesSetting[j].sOwnDist;
                    pageCheck.txtBnsOwnMun.Text = sList.OwnNamesSetting[j].sOwnMun;
                    pageCheck.txtBnsOwnProv.Text = sList.OwnNamesSetting[j].sOwnProv;
                    pageCheck.txtBnsOwnZip.Text = sList.OwnNamesSetting[j].sOwnZip;
                }
                // GDE 20090212 Owner's Information (e)}
                // copied form page 1 - verify info (e)}

                //////////// add more values
                // GDE 20090212 page 2 values (e)}

                sList.OwnName = sList.BPLSAppSettings[i].sPrevBnsOwn;

                // GDE 20090212 page 3 values (s){
                pageCheck.txtInitialCap.Text = string.Format("{0:#,##.00}", Convert.ToDouble(sList.BPLSAppSettings[i].sCapital));
                pageCheck.m_strTempCapital = string.Format("{0:#,##.00}", Convert.ToDouble(pageCheck.txtInitialCap.Text.ToString()));   // RMC 20110725

                if (pageCheck.m_sFormStatus == "BUSS-UPDATE" || pageCheck.m_sFormStatus == "REN-APP")
                {
                    pageCheck.txtCurrentGross.Text = "0.00";
                    pageCheck.txtPrevGross.Text = string.Format("{0:#,##.00}", Convert.ToDouble(sList.BPLSAppSettings[i].sGr1));
                }
                else
                {
                    pageCheck.txtCurrentGross.Text = string.Format("{0:#,##.00}", Convert.ToDouble(sList.BPLSAppSettings[i].sGr1));
                    pageCheck.txtPrevGross.Text = "0.00";
                }


                pageCheck.m_strTempGross = string.Format("{0:#,##.00}", Convert.ToDouble(pageCheck.txtCurrentGross.Text.ToString()));

                // copied form page 1 - verify info (s){
                // GDE 20090212 Owner's Information (s){
                for (int j = 0; j < sList.OwnNamesSetting.Count; j++)
                {
                    pageCheck.txtPrevOwnCode.Text = sList.OwnNamesSetting[j].sOwnerCode;
                    pageCheck.txtPrevOwnLn.Text = sList.OwnNamesSetting[j].sLn;
                    pageCheck.txtPrevOwnFn.Text = sList.OwnNamesSetting[j].sFn;
                    pageCheck.txtPrevOwnMi.Text = sList.OwnNamesSetting[j].sMi;
                    pageCheck.txtPrevOwnAddNo.Text = sList.OwnNamesSetting[j].sOwnHouseNo;
                    pageCheck.txtPrevOwnStreet.Text = sList.OwnNamesSetting[j].sOwnStreet;
                    pageCheck.cmbPrevOwnBrgy.Text = sList.OwnNamesSetting[j].sOwnBrgy;
                    pageCheck.cmbPrevOwnDist.Text = sList.OwnNamesSetting[j].sOwnDist;
                    pageCheck.txtPrevOwnMun.Text = sList.OwnNamesSetting[j].sOwnMun;
                    pageCheck.txtPrevOwnProv.Text = sList.OwnNamesSetting[j].sOwnProv;
                    pageCheck.txtPrevOwnZip.Text = sList.OwnNamesSetting[j].sOwnZip;
                }
                // GDE 20090212 Owner's Information (e)}
                // copied form page 1 - verify info (e)}
                // GDE 20090212 page 3 values (e)}
                try
                {
                    pageCheck.txtDTI.Text = sList.BPLSAppSettings[i].sDTIRegNo;
                    pageCheck.dtpDTIIssued.Value = Convert.ToDateTime(sList.BPLSAppSettings[i].sDTIRegDate);
                    pageCheck.txtSSSNo.Text = sList.BPLSAppSettings[i].sSSSNo;
                    pageCheck.dtpSSSIssued.Value = Convert.ToDateTime(sList.BPLSAppSettings[i].sSSSIssuedOn);
                    pageCheck.txtCTCNo.Text = sList.BPLSAppSettings[i].sCTCCno;
                    pageCheck.dtpCTCNoIssued.Value = Convert.ToDateTime(sList.BPLSAppSettings[i].sCTCIssuedOn);
                    pageCheck.txtCTCNoIssuedAt.Text = sList.BPLSAppSettings[i].sCTCIssuedAt;
                    pageCheck.txtMemo.Text = sList.BPLSAppSettings[i].sMemo;
                    // RMC 20110803 (s)
                    pageCheck.txtTINNo.Text = sList.BPLSAppSettings[i].m_sTIN;
                    pageCheck.dtpTINNoIssued.Value = Convert.ToDateTime(sList.BPLSAppSettings[i].m_sTINIssuedOn);
                    pageCheck.dtpApplicationDate.Value = Convert.ToDateTime(sList.BPLSAppSettings[i].m_sDateApplied);
                    // RMC 20110803 (e)

                    // RMC 20140115 application date set to current date in renewal application add (s)
                    if (pageCheck.m_sFormStatus == "REN-APP")
                        pageCheck.dtpApplicationDate.Value = AppSettingsManager.GetCurrentDate();
                    // RMC 20140115 application date set to current date in renewal application add (e)

                    if (pageCheck.m_sFormStatus == "BUSS-UPDATE" || pageCheck.m_sFormStatus == "REN-APP")
                        pageCheck.cmbBnsStat.Text = "RENEWAL";
                    else
                    {
                        if (sList.BPLSAppSettings[i].sBnsStat == "REN" || sList.BPLSAppSettings[i].sBnsStat == "RENEWAL")
                            pageCheck.cmbBnsStat.Text = "RENEWAL";
                        else if (sList.BPLSAppSettings[i].sBnsStat == "RET" || sList.BPLSAppSettings[i].sBnsStat == "RETIRED")
                            pageCheck.cmbBnsStat.Text = "RETIRED";
                        else
                            pageCheck.cmbBnsStat.Text = "NEW";

                    }

                    pageCheck.dtpOperationStart.Text = sList.BPLSAppSettings[i].sDTOperated;
                    pageCheck.m_strOperationStart = string.Format("{0:MM/dd/yyyy}", pageCheck.dtpOperationStart.Text); // RMC 20110725

                    // RMC 20110906 Added saving of Application requirements in Business Records (s)
                    /*result.Query = string.Format("select * from requirements_tbl where bin = '{0}' and req_code = '001' and status = 'Y'", sBIN);
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            pageCheck.chkWithBrgyClearance.Checked = true;
                        }
                        else
                        {
                            pageCheck.chkWithBrgyClearance.Checked = false;
                        }
                    }
                    result.Close();*/
                    // RMC 20110906 commented. Added saving of Application requirements in Business Records (e)

                    result.Query = "select * from consol_gr where bin = '" + sBIN + "' and ofc_type <> 'SINGLE'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            pageCheck.rbtnMain.Enabled = true;
                            pageCheck.rbtnBranch.Enabled = true;
                            pageCheck.chkConsolidatedGross.Checked = true;

                            pageCheck.m_sOfcType = result.GetString("ofc_type");
                            pageCheck.m_sMotherBin = result.GetString("mother_bin");

                            if (pageCheck.m_sOfcType == "MAIN")
                            {
                                pageCheck.rbtnMain.Checked = true;
                                pageCheck.rbtnBranch.Checked = false;
                                pageCheck.m_sRadioMain = "MAIN";
                                pageCheck.btnLinkUp.Enabled = false;

                            }
                            else
                            {
                                pageCheck.rbtnMain.Checked = false;
                                pageCheck.rbtnBranch.Checked = true;
                                pageCheck.m_sRadioMain = "BRANCH";
                                pageCheck.btnLinkUp.Enabled = true;

                            }
                        }
                        else
                        {
                            pageCheck.chkConsolidatedGross.Checked = false;
                            pageCheck.rbtnBranch.Enabled = false;
                            pageCheck.rbtnMain.Enabled = false;
                            pageCheck.btnLinkUp.Enabled = false;
                        }
                    }
                    result.Close();

                    if (pageCheck.m_sFormStatus.Substring(0, 4) == "BUSS" || pageCheck.m_sFormStatus == "NEW-APP" || pageCheck.m_sFormStatus == "SPL-APP")
                    {
                        pageCheck.chkConsolidatedGross.Enabled = false;
                        pageCheck.rbtnBranch.Enabled = false;
                        pageCheck.rbtnMain.Enabled = false;
                        pageCheck.btnLinkUp.Enabled = false;
                    }

                    //result.Query = string.Format("select * from ref_no_tbl where bin = '{0}'", sBIN);
                    //MCR 20150116 (s)

                    if (pageCheck.cmbBnsStat.Text == "NEW")
                        result.Query = string.Format("select * from app_permit_no_new where bin = '{0}' and year = '{1}'", sBIN, sTaxYear); //MCR 2015011
                    else
                        result.Query = string.Format("select * from app_permit_no where bin = '{0}' and year = '{1}'", sBIN, sTaxYear); //MCR 20150114
                    if (result.Execute())
                        if (result.Read())
                        {
                            //pageCheck.txtOldBin.Text = result.GetString("old_bin");
                            pageCheck.txtOldBin.Text = result.GetString("app_no"); //MCR 20150114
                        }
                    result.Close();
                    //MCR 20150116 (e)

                    // RMC 20110891 added capturing/viewing of Business Plate (s)
                    result.Query = string.Format("select * from buss_plate where bin = '{0}'", sBIN);
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            pageCheck.txtBussPlate.Text = result.GetString("bns_plate");
                        }
                    }
                    result.Close();
                    // RMC 20110819 added capturing/viewing of Business Plate (e)

                    // RMC 20140104 Capturing of gender of employee (s)
                    result.Query = "delete from addl_info_tmp where bin = '" + sBIN + "'";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    result.Query = "insert into addl_info_tmp ";
                    result.Query += "select * from addl_info where bin = '" + sBIN + "'";
                    if (result.ExecuteNonQuery() == 0)
                    { }
                    // RMC 20140104 Capturing of gender of employee (e)
                }
                catch (System.Exception ex)
                {

                }

                if (pageCheck.m_sFormStatus == "REN-APP")
                {
                    string sTmpYear = string.Format("{0:000#}", Convert.ToInt32(sList.BPLSAppSettings[i].sTaxYear));
                    string sTmpCurrYear = string.Format("{0:000#}", Convert.ToInt32(sList.BPLSAppSettings[i].sTaxYear) + 1);
                    LoadOtherInfo(sTmpYear, sTmpCurrYear, sBIN);
                }

                pageCheck.chkECC.Checked = CheckEcc();  // RMC 20171128 added tagging of ECC 

            }
        }

        public void SaveUpdateBns(string sBin, string sAppStat)
        {
            // RMC 20111014 modified query commit in Business Records transactions (s)
            // changed all pSet transactional to pCmd
            OracleResultSet pCmd = new OracleResultSet();
            pCmd.Transaction = true;
            // RMC 20111014 modified query commit in Business Records transactions (e)

            m_objSystemUser = new SystemUser();
            string sBnsStat = string.Empty;
            /*string sBnsZip = string.Empty;*/
            string sBnsMacPin = string.Empty;

            if (pageCheck.cmbBnsStat.Text.Trim() == "RENEWAL")
                sBnsStat = "REN";
            else if (pageCheck.cmbBnsStat.Text.Trim() == "RETIRED")
                sBnsStat = "RET";
            else
                sBnsStat = "NEW";

            if (sAppStat == "NEW-APP" || sAppStat == "SPL-APP" || sAppStat == "REN-APP" || sAppStat == "NEW-APP-EDIT" || sAppStat == "SPL-APP-EDIT" || sAppStat == "REN-APP-EDIT")
            {
                if (sAppStat == "SPL-APP" || sAppStat == "SPL-APP-EDIT")
                    //pCmd.Query = "delete from business_que where bin = :1";
                    pCmd.Query = "delete from spl_business_que where bin = :1"; // RMC 20171123 enabled special business feature
                else
                    pCmd.Query = "delete from business_que where bin = :1";
                pCmd.AddParameter(":1", sBin);
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }
                //pCmd.Close();

                // business mapping
                //if (pageCheck.ValidateMapping(sBin))
                if (AppSettingsManager.ValidateMapping(sBin)) // RMC 20111220 transferred validation of business if business-mapped in AppSettingsManager
                {
                    string sTmpDate = "";
                    sTmpDate = Convert.ToString(AppSettingsManager.GetCurrentDate());

                    pCmd.Query = "update btm_update set def_settled = 'Y', ";
                    pCmd.Query += "settled_by = '" + AppSettingsManager.SystemUser.UserCode + "', ";
                    pCmd.Query += "settled_dt = to_date('" + sTmpDate + "', 'MM/dd/yyyy hh:mi:ss am') ";
                    pCmd.Query += "where bin = '" + sBin + "' and trim(def_settled) is null";
                    pCmd.AddParameter(":1", AppSettingsManager.GetCurrentDate());
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }
                }

            }
            else
            {
                pCmd.Query = "delete from businesses where bin = :1";
                pCmd.AddParameter(":1", sBin);
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }
                //pCmd.Close();
            }

            if (pageCheck.txtInitialCap.Text.Trim() == string.Empty)
                pageCheck.txtInitialCap.Text = "0.00";

            if (pageCheck.txtPrevGross.Text.Trim() == string.Empty)
                pageCheck.txtPrevGross.Text = "0.00";

            if (pageCheck.txtCurrentGross.Text.Trim() == string.Empty)
                pageCheck.txtCurrentGross.Text = "0.00";

            if (pageCheck.cmbPlaceBnsStat.Text.Trim() == "OWNED")
                pageCheck.txtMonthlyRental.Text = "0.00";

            string strPermitNo = string.Empty;

            double dblTemp = 0;

            if (pageCheck.txtMPYear.Text.Trim() != "" && pageCheck.txtMPNo.Text.Trim() != "")
                strPermitNo = pageCheck.txtMPYear.Text.Trim() + "-" + pageCheck.txtMPNo.Text.Trim();

            #region comments
            /*if (sAppStat == "NEW-APP" || sAppStat == "REN-APP" || sAppStat == "NEW-APP-EDIT" || sAppStat == "REN-APP-EDIT")
                pSet.Query = "insert into business_que values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14,:15,:16,:17,:18,:19,:20,:21,:22,:23,:24,:25,:26,:27,:28,:29,:30,:31,:32,:33,:34,:35,:36,:37,:38,:39,:40,:41,:42,:43,:44,:45,:46,:47,:48,:49,:50,:51,:52,:53,:54,:55,:56,:57,:58,:59,:60,:61,:62,:63,:64,:65,:66,:67,:68,:69)"; // RMC 20110803
            else
                pSet.Query = "insert into businesses values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14,:15,:16,:17,:18,:19,:20,:21,:22,:23,:24,:25,:26,:27,:28,:29,:30,:31,:32,:33,:34,:35,:36,:37,:38,:39,:40,:41,:42,:43,:44,:45,:46,:47,:48,:49,:50,:51,:52,:53,:54,:55,:56,:57,:58,:59,:60,:61,:62,:63,:64,:65,:66,:67,:68,:69)";   // RMC 20110803
            pSet.AddParameter(":1", sBin.Trim());    //BIN
            pSet.AddParameter(":2", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsName.Text.Trim())); //BNS_NM
            pSet.AddParameter(":3", sBnsStat);   //BNS_STAT
            pSet.AddParameter(":4", pageCheck.GetPage1OwnCode);  //OWN_CODE
            pSet.AddParameter(":5", pageCheck.txtTelNo.Text.Trim());     //BNS_TELNO
            pSet.AddParameter(":6", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsAddNo.Text.Trim()));    //BNS_HOUSE_NO
            pSet.AddParameter(":7", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsStreet.Text.Trim()));   //BNS_STREET
            pSet.AddParameter(":8", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsMun.Text.Trim()));  //BNS_MUN
            pSet.AddParameter(":9", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.cmbBnsDist.Text.Trim())));    //BNS_DIST
            pSet.AddParameter(":10", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsZone.Text.Trim()));    //BNS_ZONE
            pSet.AddParameter(":11", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.cmbBnsBrgy.Text.Trim())));   //BNS_BRGY
            pSet.AddParameter(":12", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsProv.Text.Trim()));    //BNS_PROV
            pSet.AddParameter(":13", pageCheck.m_sBnsZip);   //BNS_ZIP
            pSet.AddParameter(":14", pageCheck.txtPINLand.Text.Trim());  //LAND_PIN
            pSet.AddParameter(":15", pageCheck.txtPINBldg.Text.Trim());  //BLDG_PIN
            pSet.AddParameter(":16", sBnsMacPin);    //MACH_PIN
            pSet.AddParameter(":17", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtPINAddNo.Text.Trim()));   //POFF_HOUSE_NO
            pSet.AddParameter(":18", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtPINStreet.Text.Trim()));  //POFF_STREET
            pSet.AddParameter(":19", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtPINMun.Text.Trim())); //POFF_MUN
            pSet.AddParameter(":20", pageCheck.cmbPINDist.Text.Trim());  //POFF_DIST
            pSet.AddParameter(":21", string.Empty);  //POFF_ZONE
            pSet.AddParameter(":22", pageCheck.cmbPINBrgy.Text.Trim());  //POFF_BRGY
            pSet.AddParameter(":23", pageCheck.txtPINProv.Text.Trim());  //POFF_PROV
            pSet.AddParameter(":24", string.Empty);  //POFF_ZIP
            pSet.AddParameter(":25", pageCheck.cmbBnsOrgnKind.Text.Trim());  //ORGN_KIND
            pSet.AddParameter(":26", pageCheck.GetPage2OwnCode); //BUSN_OWN
            pSet.AddParameter(":27", pageCheck.txtCTCNo.Text.Trim());    //CTC_NO
            if(pageCheck.txtCTCNo.Text.Trim() == "")
                pSet.AddParameter(":28", null);  //CTC_ISSUED_ON
            else
                pSet.AddParameter(":28", pageCheck.dtpCTCNoIssued.Value);  //CTC_ISSUED_ON
            pSet.AddParameter(":29", pageCheck.txtCTCNoIssuedAt.Text.Trim());    //CTC_ISSUED_AT
            pSet.AddParameter(":30", AppSettingsManager.GetBnsCodeByDesc(pageCheck.txtBnsType.Text.Trim())); //BNS_CODE
            pSet.AddParameter(":31", pageCheck.txtSSSNo.Text.Trim());    //SSS_NO
            if(pageCheck.txtSSSNo.Text.Trim() == "")
                pSet.AddParameter(":32", null);  //SSS_ISSUED_ON
            else
                pSet.AddParameter(":32", pageCheck.c.Value);  //SSS_ISSUED_ON
            pSet.AddParameter(":33", pageCheck.txtDTI.Text.Trim());  //DTI_REG_NO
            if (pageCheck.txtDTI.Text.Trim() == "")
                pSet.AddParameter(":34", null);  //DTI_REG_DT
            else
                pSet.AddParameter(":34", pageCheck.dtpDTIIssued.Value);

            if (pageCheck.txtBldgValue.Text.Trim() == "")
                pageCheck.txtBldgValue.Text = "0";

            pSet.AddParameter(":35", pageCheck.txtBldgValue.Text.Trim());    //BLDG_VAL
            pSet.AddParameter(":36", pageCheck.cmbPlaceBnsStat.Text.Trim());     //PLACE_OCCUPANCY
            if (pageCheck.txtSinceWhen.Text.Trim() == "")
                pSet.AddParameter(":37", null);
            else
                pSet.AddParameter(":37", DateTime.Parse(pageCheck.txtSinceWhen.Text));   //RENT_LEASE_SINCE
            //dblTemp = Convert.ToDouble(pageCheck.txtMonthlyRental.Text.Trim());    // RMC 20110725
            dblTemp = this.ToDouble(pageCheck.txtMonthlyRental.Text.Trim());  // RMC 20110725
            pSet.AddParameter(":38", dblTemp);    //RENT_LEASE_MO

            //dblTemp = Convert.ToDouble(pageCheck.txtGroundArea.Text.Trim());   // RMC 20110725
            dblTemp = this.ToDouble(pageCheck.txtGroundArea.Text.Trim());   // RMC 20110725

            pSet.AddParameter(":39", dblTemp);     //FLR_AREA
            pSet.AddParameter(":40", pageCheck.txtNoStorey.Text.Trim());     //NUM_STOREYS

            //dblTemp = Convert.ToDouble(pageCheck.txtTotFloorArea.Text.Trim()); // RMC 20110725
            dblTemp = this.ToDouble(pageCheck.txtTotFloorArea.Text.Trim());  // RMC 20110725

            pSet.AddParameter(":41", dblTemp);     //TOT_FLR_AREA

            pSet.AddParameter(":42", pageCheck.GetPage3OwnCode);     //PREV_BNS_OWN
            pSet.AddParameter(":43", pageCheck.txtNoEmp.Text.Trim());    //NUM_EMPLOYEES
            pSet.AddParameter(":44", pageCheck.txtNoProf.Text.Trim());   //NUM_PROFESSIONAL

            //dblTemp = Convert.ToDouble(pageCheck.txtAnnualPayroll.Text.Trim());    // RMC 20110725 
            dblTemp = this.ToDouble(pageCheck.txtAnnualPayroll.Text.Trim());    // RMC 20110725 
            pSet.AddParameter(":45", dblTemp);    //ANNUAL_WAGES

            //dblTemp = Convert.ToDouble(pageCheck.txtElecBill.Text.Trim()); // RMC 20110725
            dblTemp = this.ToDouble(pageCheck.txtElecBill.Text.Trim()); // RMC 20110725
            pSet.AddParameter(":46", dblTemp); //AVE_ELECTRIC_BILL

            //dblTemp = Convert.ToDouble(pageCheck.txtWaterBill.Text.Trim());    // RMC 20110725
            dblTemp = this.ToDouble(pageCheck.txtWaterBill.Text.Trim());    // RMC 20110725
            pSet.AddParameter(":47", dblTemp);    //AVE_WATER_BILL

            //dblTemp = Convert.ToDouble(pageCheck.txtTelBill.Text.Trim());  // RMC 20110725
            dblTemp = this.ToDouble(pageCheck.txtTelBill.Text.Trim());  // RMC 20110725
            pSet.AddParameter(":48", dblTemp);  //AVE_PHONE_BILL

            dblTemp = 0;
            pSet.AddParameter(":49", dblTemp);  //OTHER_UTIL
            pSet.AddParameter(":50", pageCheck.txtNoDelivery.Text.Trim());   //NUM_DELIV_VEHICLE
            pSet.AddParameter(":51", pageCheck.txtNoMac.Text.Trim());    //NUM_MACHINERIES
            pSet.AddParameter(":52", pageCheck.dtpOperationStart.Value); //DT_OPERATED
            pSet.AddParameter(":53", StringUtilities.StringUtilities.SetEmptyToSpace(strPermitNo)); //PERMIT_NO
            pSet.AddParameter(":54", DateTime.Parse(pageCheck.dtpMPDate.Text));  //PERMIT_DT

            //dblTemp = Convert.ToDouble(pageCheck.txtCurrentGross.Text.Trim()); // RMC 20110725
            dblTemp = this.ToDouble(pageCheck.txtCurrentGross.Text.Trim()); // RMC 20110725
            pSet.AddParameter(":55", dblTemp); //GR_1

            //dblTemp = Convert.ToDouble(pageCheck.txtPrevGross.Text.Trim());    // RMC 20110725
            dblTemp = this.ToDouble(pageCheck.txtPrevGross.Text.Trim());    // RMC 20110725
            pSet.AddParameter(":56", dblTemp);    //GR_2

            //dblTemp = Convert.ToDouble(pageCheck.txtInitialCap.Text.Trim());   // RMC 20110725
            dblTemp = this.ToDouble(pageCheck.txtInitialCap.Text.Trim());   // RMC 20110725
            pSet.AddParameter(":57", dblTemp);   //CAPITAL

            pSet.AddParameter(":58", string.Empty);  //OR_NO
            pSet.AddParameter(":59", pageCheck.txtTaxYear.Text);     //TAX_YEAR
            pSet.AddParameter(":60", string.Empty);  //CANC_REASON
            pSet.AddParameter(":61", null);  //CANC_DATE
            pSet.AddParameter(":62", string.Empty);  //CANC_BY
            pSet.AddParameter(":63", AppSettingsManager.SystemUser.UserCode);    //BNS_USER
            //pSet.AddParameter(":64", DateTime.Now);  //SAVE_TM
            pSet.AddParameter(":64", AppSettingsManager.GetCurrentDate());    // RMC 20110725 
            pSet.AddParameter(":65", pageCheck.txtMemo.Text.Trim()); //MEMORANDA
            pSet.AddParameter(":66", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtEmailAdd.Text.Trim()));//bns_email
            pSet.AddParameter(":67", pageCheck.txtTINNo.Text.Trim());    //tin_no
            if (pageCheck.txtTINNo.Text.Trim() == "")
                pSet.AddParameter(":68", null);  //tin_issued_on
            else
                pSet.AddParameter(":68", pageCheck.dtpTINNoIssued.Value);  //tin_issued_on
            pSet.AddParameter(":69", pageCheck.dtpApplicationDate.Value);    //dt_applied
              */
            #endregion
            // RMC 20111014 modified saving of businesses, correcting buss name with apostrophe (note: dont use AddParameter if going to insert strings with apostrophe) (s)
            string sDate = "";
            if (sAppStat == "NEW-APP" || sAppStat == "REN-APP" || sAppStat == "NEW-APP-EDIT" || sAppStat == "REN-APP-EDIT")
                pCmd.Query = "insert into business_que values (";
            else if (sAppStat == "SPL-APP" || sAppStat == "SPL-APP-EDIT")
                pCmd.Query = "insert into spl_business_que values (";
            else
                pCmd.Query = "insert into businesses values (";
            pCmd.Query += "'" + sBin.Trim() + "', ";    //BIN
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsName.Text.Trim()) + "', "; //BNS_NM
            pCmd.Query += "'" + sBnsStat + "', ";   //BNS_STAT
            pCmd.Query += "'" + pageCheck.GetPage1OwnCode + "', ";  //OWN_CODE
            pCmd.Query += "'" + pageCheck.txtTelNo.Text.Trim() + "', ";     //BNS_TELNO
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsAddNo.Text.Trim()) + "', ";    //BNS_HOUSE_NO
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsStreet.Text.Trim()) + "', ";   //BNS_STREET
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsMun.Text.Trim()) + "', ";  //BNS_MUN
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.cmbBnsDist.Text.Trim())) + "', ";    //BNS_DIST
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsZone.Text.Trim()) + "', ";    //BNS_ZONE
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.cmbBnsBrgy.Text.Trim())) + "', ";   //BNS_BRGY
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsProv.Text.Trim()) + "', ";    //BNS_PROV
            pCmd.Query += "'" + pageCheck.m_sBnsZip + "', ";   //BNS_ZIP
            pCmd.Query += "'" + pageCheck.txtPINLand.Text.Trim() + "', ";  //LAND_PIN
            pCmd.Query += "'" + pageCheck.txtPINBldg.Text.Trim() + "', ";  //BLDG_PIN
            pCmd.Query += "'" + sBnsMacPin + "', ";    //MACH_PIN
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtPINAddNo.Text.Trim()) + "', ";   //POFF_HOUSE_NO
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtPINStreet.Text.Trim()) + "', ";  //POFF_STREET
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtPINMun.Text.Trim()) + "', "; //POFF_MUN
            pCmd.Query += "'" + pageCheck.cmbPINDist.Text.Trim() + "', ";  //POFF_DIST
            pCmd.Query += "' ', ";  //POFF_ZONE
            pCmd.Query += "'" + pageCheck.cmbPINBrgy.Text.Trim() + "', ";  //POFF_BRGY
            pCmd.Query += "'" + pageCheck.txtPINProv.Text.Trim() + "', ";  //POFF_PROV
            pCmd.Query += "' ', ";  //POFF_ZIP
            pCmd.Query += "'" + pageCheck.cmbBnsOrgnKind.Text.Trim() + "', ";  //ORGN_KIND
            pCmd.Query += "'" + pageCheck.GetPage2OwnCode + "', "; //BUSN_OWN
            pCmd.Query += "'" + pageCheck.txtCTCNo.Text.Trim() + "', ";    //CTC_NO
            if (pageCheck.txtCTCNo.Text.Trim() == "")
                pCmd.Query += "'', ";  //CTC_ISSUED_ON
            else
            {
                sDate = string.Format("{0:MM/dd/yyyy}", pageCheck.dtpCTCNoIssued.Value);
                pCmd.Query += " to_date('" + sDate + "','MM/dd/yyyy'), ";    //CTC_ISSUED_ON
            }
            pCmd.Query += "'" + pageCheck.txtCTCNoIssuedAt.Text.Trim() + "', ";    //CTC_ISSUED_AT
            pCmd.Query += "'" + AppSettingsManager.GetBnsCodeByDesc(pageCheck.txtBnsType.Text.Trim()) + "', "; //BNS_CODE
            pCmd.Query += "'" + pageCheck.txtSSSNo.Text.Trim() + "', ";   //SSS_NO
            if (pageCheck.txtSSSNo.Text.Trim() == "")
                pCmd.Query += "'', ";  //SSS_ISSUED_ON
            else
            {
                sDate = string.Format("{0:MM/dd/yyyy}", pageCheck.dtpSSSIssued.Value);
                pCmd.Query += " to_date('" + sDate + "','MM/dd/yyyy'), ";  //SSS_ISSUED_ON
            }

            pCmd.Query += "'" + pageCheck.txtDTI.Text.Trim() + "', ";  //DTI_REG_NO
            if (pageCheck.txtDTI.Text.Trim() == "")
                pCmd.Query += "'', ";  //DTI_REG_DT
            else
            {
                sDate = string.Format("{0:MM/dd/yyyy}", pageCheck.dtpDTIIssued.Value);
                pCmd.Query += " to_date('" + sDate + "','MM/dd/yyyy'), ";
            }

            if (pageCheck.txtBldgValue.Text.Trim() == "")
                pageCheck.txtBldgValue.Text = "0";

            pCmd.Query += "'" + pageCheck.txtBldgValue.Text.Trim() + "', ";   //BLDG_VAL
            pCmd.Query += "'" + pageCheck.cmbPlaceBnsStat.Text.Trim() + "', ";     //PLACE_OCCUPANCY
            if (pageCheck.txtSinceWhen.Text.Trim() == "")
                pCmd.Query += "'', ";
            else
                pCmd.Query += " to_date('" + pageCheck.txtSinceWhen.Text + "', 'MM/dd/yyyy'), ";   //RENT_LEASE_SINCE

            dblTemp = this.ToDouble(pageCheck.txtMonthlyRental.Text.Trim());  // RMC 20110725
            pCmd.Query += "" + dblTemp + ",";    //RENT_LEASE_MO

            dblTemp = this.ToDouble(pageCheck.txtGroundArea.Text.Trim());   // RMC 20110725

            pCmd.Query += "" + dblTemp + ",";     //FLR_AREA
            pCmd.Query += "'" + pageCheck.txtNoStorey.Text.Trim() + "', ";     //NUM_STOREYS

            dblTemp = this.ToDouble(pageCheck.txtTotFloorArea.Text.Trim());  // RMC 20110725

            pCmd.Query += "" + dblTemp + ",";     //TOT_FLR_AREA

            pCmd.Query += "'" + pageCheck.GetPage3OwnCode + "', ";   //PREV_BNS_OWN
            pCmd.Query += "'" + pageCheck.txtNoEmp.Text.Trim() + "', ";    //NUM_EMPLOYEES
            pCmd.Query += "'" + pageCheck.txtNoProf.Text.Trim() + "', ";   //NUM_PROFESSIONAL

            dblTemp = this.ToDouble(pageCheck.txtAnnualPayroll.Text.Trim());    // RMC 20110725 
            pCmd.Query += "" + dblTemp + ",";     //ANNUAL_WAGES

            dblTemp = this.ToDouble(pageCheck.txtElecBill.Text.Trim()); // RMC 20110725
            pCmd.Query += "" + dblTemp + ",";  //AVE_ELECTRIC_BILL

            dblTemp = this.ToDouble(pageCheck.txtWaterBill.Text.Trim());    // RMC 20110725
            pCmd.Query += "" + dblTemp + ",";     //AVE_WATER_BILL

            dblTemp = this.ToDouble(pageCheck.txtTelBill.Text.Trim());  // RMC 20110725
            pCmd.Query += "" + dblTemp + ",";   //AVE_PHONE_BILL

            dblTemp = 0;
            pCmd.Query += "" + dblTemp + ",";   //OTHER_UTIL
            pCmd.Query += "'" + pageCheck.txtNoDelivery.Text.Trim() + "', ";   //NUM_DELIV_VEHICLE
            pCmd.Query += "'" + pageCheck.txtNoMac.Text.Trim() + "', ";    //NUM_MACHINERIES

            sDate = string.Format("{0:MM/dd/yyyy}", pageCheck.dtpOperationStart.Value);
            pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'), "; //DT_OPERATED

            pCmd.Query += "'" + StringUtilities.StringUtilities.SetEmptyToSpace(strPermitNo) + "', "; //PERMIT_NO

            sDate = string.Format("{0:MM/dd/yyyy}", pageCheck.dtpMPDate.Value);
            pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'), ";  //PERMIT_DT

            dblTemp = this.ToDouble(pageCheck.txtCurrentGross.Text.Trim()); // RMC 20110725
            pCmd.Query += "" + dblTemp + ",";   //GR_1

            dblTemp = this.ToDouble(pageCheck.txtPrevGross.Text.Trim());    // RMC 20110725
            pCmd.Query += "" + dblTemp + ",";     //GR_2

            dblTemp = this.ToDouble(pageCheck.txtInitialCap.Text.Trim());   // RMC 20110725
            pCmd.Query += "" + dblTemp + ",";    //CAPITAL

            pCmd.Query += "'', ";   //OR_NO
            pCmd.Query += "'" + pageCheck.txtTaxYear.Text + "', ";    //TAX_YEAR
            pCmd.Query += "'', ";  //CANC_REASON
            pCmd.Query += "'', ";  //CANC_DATE
            pCmd.Query += "'', "; //CANC_BY
            pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";    //BNS_USER

            sDate = Convert.ToString(AppSettingsManager.GetCurrentDate());
            pCmd.Query += " to_date('" + sDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtMemo.Text.Trim()) + "', "; //MEMORANDA // RMC 20120116 added handleapostrophe in memoranda businesses
            pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtEmailAdd.Text.Trim()) + "', ";//bns_email
            pCmd.Query += "'" + pageCheck.txtTINNo.Text.Trim() + "', ";    //tin_no
            if (pageCheck.txtTINNo.Text.Trim() == "")
                pCmd.Query += "'', ";  //tin_issued_on
            else
            {
                sDate = string.Format("{0:MM/dd/yyyy}", pageCheck.dtpTINNoIssued.Value);
                pCmd.Query += " to_date('" + sDate + "','MM/dd/yyyy'),";  //tin_issued_on
            }
            sDate = string.Format("{0:MM/dd/yyyy}", pageCheck.dtpApplicationDate.Value);
            pCmd.Query += " to_date('" + sDate + "','MM/dd/yyyy'))";   //dt_applied
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }
            //pCmd.Close();
            // RMC 20111014 modified saving of businesses, correcting buss name with apostrophe (note: dont use AddParameter if going to insert strings with apostrophe) (e)

            // RMC 20110819 added capturing/viewing of Business Plate (s)
            pCmd.Query = string.Format("delete from buss_plate where bin = '{0}'", sBin);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }
            //pCmd.Close();

            if (pageCheck.txtBussPlate.Text.ToString().Trim() != "")
            {
                pCmd.Query = "insert into buss_plate values (:1,:2)";
                pCmd.AddParameter(":1", sBin);
                pCmd.AddParameter(":2", pageCheck.txtBussPlate.Text.ToString().Trim());
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }
                //pCmd.Close();
            }
            // RMC 20110819 added capturing/viewing of Business Plate (e)

            if (pageCheck.m_sFormStatus.Substring(0, 4) == "BUSS")
            {
                if (pageCheck.m_sFormStatus == "BUSS-EDIT" || pageCheck.m_sFormStatus == "BUSS-UPDATE")
                {
                    if (pageCheck.m_sFormStatus == "BUSS-EDIT")
                    {
                        // RMC 20110803 (s)
                        pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}'", sBin.Trim(), pageCheck.txtTaxYear.Text.Trim());
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                pSet.Close();
                            }
                            else
                            {
                                pSet.Close();
                                pCmd.Query = string.Format("insert into pay_hist values(' ', '{0}', '{1}', '{2}', ' ', ' ', null, ' ', 'UNP', ' ', null, null, '{3}', ' ', ' ', null)", sBin.Trim(), sBnsStat.Trim(), pageCheck.txtTaxYear.Text.Trim(), AppSettingsManager.SystemUser.UserCode);
                                if (pCmd.ExecuteNonQuery() == 0)
                                {
                                }
                                //pCmd.Close();
                            }
                        }
                        // RMC 20110803 (e)

                    }
                }
                else
                {
                    pCmd.Query = string.Format("insert into pay_hist values(' ', '{0}', '{1}', '{2}', ' ', ' ', null, ' ', 'UNP', ' ', null, null, '{3}', ' ', ' ', null)", sBin.Trim(), sBnsStat.Trim(), pageCheck.txtTaxYear.Text.Trim(), AppSettingsManager.SystemUser.UserCode);
                    if (pCmd.ExecuteNonQuery() == 0)
                    {
                    }
                    //pCmd.Close();

                }

                //REM MCR 20150114 (s)
                /*if (pageCheck.m_sFormStatus == "BUSS-ADD-NEW" || pageCheck.m_sFormStatus == "BUSS-EDIT")
                {
                    pCmd.Query = string.Format("delete from ref_no_tbl where bin = '{0}'", sBin.Trim());
                    if (pCmd.ExecuteNonQuery() == 0)
                    {
                    }
                    //pCmd.Close();

                    if (pageCheck.txtOldBin.Text.Trim() != "")
                    {
                        pCmd.Query = "insert into ref_no_tbl values (:1,:2)";
                        pCmd.AddParameter(":1", sBin.Trim());
                        pCmd.AddParameter(":2", pageCheck.txtOldBin.Text.Trim());
                        if (pCmd.ExecuteNonQuery() == 0)
                        {
                        }
                        //pCmd.Close();
                    }
                }*/
                //REM MCR 20150114 (s)
            }

            // RMC 20141217 adjustments (s)
            if (sAppStat == "NEW-APP")
            {
                pCmd.Query = "update addl_info_tmp set bin = '" + sBin + "' where bin = '" + pageCheck.m_strTempBIN + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }
            // RMC 20141217 adjustments (e)

            // RMC 20111014 modified query commit in Business Records transactions (s)

            // RMC 20140104 Capturing of gender of employee (s)
            pCmd.Query = "delete from addl_info where bin = '" + sBin + "'";
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            pCmd.Query = "insert into addl_info ";
            pCmd.Query += "select * from addl_info_tmp where bin = '" + sBin + "'";
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            /*pCmd.Query = "delete from addl_info_tmp where bin = '" + sBin + "'";
            if (pCmd.ExecuteNonQuery() == 0)
            { }*/
            // RMC 20141217 adjustments, put rem
            // RMC 20140104 Capturing of gender of employee (e)

            // save pin and bldg code
            bool bInsert = false;
            pSet.Query = "select * from btm_gis_loc where bin = '" + sBin.Trim() + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = "select * from btm_businesses where bin = '" + sBin.Trim() + "'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            // do not update location from business mapping drive data
                            bInsert = false;
                        }
                        else
                        {
                            pCmd.Query = "delete from btm_gis_loc where bin = '" + sBin.Trim() + "'";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }
                            bInsert = true;
                        }
                    }
                    pSet.Close();

                }
                else
                {
                    bInsert = true;
                }
            }
            pSet.Close();

            if (pageCheck.cmbLandPIN.Text.Trim() == "" && pageCheck.cmbBldgCode.Text.Trim() == "")
                bInsert = false;

            if (bInsert)
            {
                pCmd.Query = "insert into btm_gis_loc values(";
                pCmd.Query += "'" + sBin + "', ";
                pCmd.Query += "'" + pageCheck.cmbBldgCode.Text.Trim() + "', ";
                pCmd.Query += "'" + pageCheck.txtBldgName.Text.Trim() + "', ";
                pCmd.Query += "'" + pageCheck.cmbLandPIN.Text.Trim() + "')";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }

            SaveEcc();  // RMC 20171128 added tagging of ECC 

            if (!pCmd.Commit())
            {
                pCmd.Rollback();
                pCmd.Close();
                MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            pCmd.Close();
            // RMC 20111014 modified query commit in Business Records transactions (e)

            string strObj = sBin.Trim() + "/" + pageCheck.txtTaxYear.Text;
            //JARS 20180924 (S) way to add gross record in audit trail
            if (sAppStat == "REN-APP" || sAppStat == "REN-APP-EDIT")
            {
                strObj = strObj + "/GROSS:" + pageCheck.txtCurrentGross.Text.ToString();
            }
            else if(sAppStat == "NEW-APP" || sAppStat == "NEW-APP-EDIT")
            {
                strObj = strObj + "/CAPITAL:" + pageCheck.txtInitialCap.Text.ToString();
            }
            //JARS 20180924 (E)
            if (AuditTrail.AuditTrail.InsertTrail(m_strModuleCode, m_strTableUpdated, StringUtilities.StringUtilities.HandleApostrophe(strObj)) == 0)
            {
                /*pSet.Rollback();
                pSet.Close();*/

                pCmd.Rollback();
                pCmd.Close();
                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void AppPermitNo(string sBin, out string sAppNo, string sType)//MCR 20150114
        {
            sAppNo = "";
            OracleResultSet pSet = new OracleResultSet();
            //If Exist
            if (sType == "NEW")
                pSet.Query = "select * from app_permit_no_new where bin = '" + sBin + "' and year = '" + AppSettingsManager.GetSystemDate().Year + "'";
            else
                pSet.Query = "select * from app_permit_no where bin = '" + sBin + "' and year = '" + AppSettingsManager.GetSystemDate().Year + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sAppNo = pSet.GetString(1);
            //sAppDate = pSet.GetString(3);
            pSet.Close();

            if (sAppNo == "")
            {
                //Get MaxSeries
                if (sType == "NEW")
                    pSet.Query = "select coalesce(max(to_number(substr(app_no,6,10))),0) + 1 as Series from app_permit_no_new where year = '" + AppSettingsManager.GetSystemDate().Year + "'";
                else
                    pSet.Query = "select coalesce(max(to_number(substr(app_no,6,10))),0) + 1 as Series from app_permit_no where year = '" + AppSettingsManager.GetSystemDate().Year + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sAppNo = AppSettingsManager.GetSystemDate().Year + "-" + pSet.GetInt(0).ToString("00000");
                pSet.Close();
            }
        }

        public void AppNewSave()
        {
            Serials.UpdateBussSerial();
            //string strYear = DateTime.Now.Year.ToString();
            string strYear = AppSettingsManager.GetCurrentDate().Year.ToString();    // RMC 20110725
            //string m_sBin = "129-00-" + strYear + "-" + Serials.GetBussSerial();
            //string m_sBin = pageCheck.bin1.GetBin() + "-" + Serials.GetBussSerial();
            string m_sBin = StringUtilities.StringUtilities.Left(pageCheck.bin1.GetBin(), 6) + "-" + strYear + "-" + Serials.GetBussSerial();

            m_strModuleCode = "AANABA";
            if (pageCheck.m_sFormStatus == "SPL-APP")
                m_strTableUpdated = "spl_business_que";
            else
                m_strTableUpdated = "business_que";

            // RMC 20110901 Added validation if Lessor is a registered business (s)
            if (!ValidateLessor(m_sBin)) // RMC 20111012 added BIN in auto-tagging of unknown lessor 
            {
                MessageBox.Show("Transaction cancelled.\nLessor is subject for inspection", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // RMC 20110901 Added validation if Lessor is a registered business (e)
            else
            {
                //MCR 20150114 (s)
                //if (m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")
                //{
                string sAppNo = "";
                AppPermitNo(m_sBin, out sAppNo, "NEW");
                OracleResultSet pSet1 = new OracleResultSet();
                pSet1.Query = @"insert into app_permit_no_new values ('" + AppSettingsManager.GetSystemDate().Year + "','" + sAppNo + "','" + m_sBin + "','" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy") + "')";
                pSet1.ExecuteNonQuery();
                //}
                //MCR 20150114 (e)

                UpdateOtherInfoBin(m_sBin, pageCheck.m_strTempBIN); // RMC 20150117
                SaveUpdateBns(m_sBin, pageCheck.m_sFormStatus);
                //UpdateOtherInfoBin(m_sBin, pageCheck.m_strTempBIN);   // RMC 20150117
                pageCheck.bin1.txtTaxYear.Text = m_sBin.Substring(7, 4);
                pageCheck.bin1.txtBINSeries.Text = m_sBin.Substring(12, 7);

                // RMC 20110816 (s)
                /*if (pageCheck.m_strInspectionNo != "")
                {
                    pSet.Query = "update unofficial_info_tbl set bin_settled = '" + m_sBin + "' where is_number = '" + pageCheck.m_strInspectionNo + "' and trim(bin_settled) is null";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }
                }
                // RMC 20110816 (e)*/
                UpdateBusinessMapping(m_sBin);
                UpdateOtherOfficeTable(m_sBin, "NEW");  // RMC 20141228 modified permit printing (lubao) JARS 20170705 COMMENT
                UpdateBillGrossInfo(m_sBin);    // RMC 20150120

                TransactionLog.TransLog.UpdateLog(pageCheck.bin1.GetBin(), "NEW", pageCheck.txtTaxYear.Text, pageCheck.m_sFormStatus, pageCheck.m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                MessageBox.Show("Saved Record with BIN :" + m_sBin + " \nwith Application No." + sAppNo);

                pageCheck.btnSave.Enabled = false;
                pageCheck.m_strTempBIN = "";
                pageCheck.btnAddlBns.Enabled = true;
                pageCheck.btnAddNew.Enabled = true;
            }
        }

        public void AppRenSave()
        {
            //Serials.UpdateBussSerial();
            //string strYear = DateTime.Now.Year.ToString();
            //string m_sBin = "129-00-" + strYear + "-" + Serials.GetBussSerial();
            string sBin = pageCheck.bin1.GetBin();

            m_strModuleCode = "AARABA";
            m_strTableUpdated = "business_que";

            OracleResultSet result = new OracleResultSet();
            OracleResultSet rupdate = new OracleResultSet();
            string sInspectorCode = string.Empty;

            if (!ValidateMappingChanges(sBin))
                return;

            // RMC 20110901 Added validation if Lessor is a registered business (s)
            if (!ValidateLessor(sBin))  // RMC 20111012 added BIN in auto-tagging of unknown lessor 
            {
                MessageBox.Show("Transaction cancelled.\nLessor is subject for inspection", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // RMC 20110901 Added validation if Lessor is a registered business (e)
            else
            {
                result.Query = string.Format("select * from inspector_details where bin = '{0}' and is_settled = 'N'", pageCheck.bin1.GetBin());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sInspectorCode = result.GetString("inspector_code").Trim();

                        if (MessageBox.Show("This business has been inspected by '" + sInspectorCode + "'. Check the information in Inspector Details module.\nDo you want to continue this application?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            rupdate.Query = string.Format("update inspector_details set is_settled = 'Y' where bin = '{0}'", pageCheck.bin1.GetBin());
                            rupdate.Query += " and is_settled = 'N'";   // RMC 20110816
                            if (rupdate.ExecuteNonQuery() == 0)
                            { }
                        }
                        else
                            return;
                    }
                }
                result.Close();

                if (!CheckConsolGR())
                    return;

                //MCR 20150114 (s)
                //if (m_sFormStatus == "NEW-APP" || m_sFormStatus == "SPL-APP")
                //{
                string sAppNo = "";
                AppPermitNo(sBin, out sAppNo, "REN");
                OracleResultSet pSet1 = new OracleResultSet();
                pSet1.Query = @"insert into app_permit_no values ('" + AppSettingsManager.GetSystemDate().Year + "','" + sAppNo + "','" + sBin + "','" + AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy") + "')";
                pSet1.ExecuteNonQuery();
                //}
                //MCR 20150114 (e)

                SaveUpdateBns(sBin, pageCheck.m_sFormStatus);
                UpdateBusinessMapping(sBin);
                UpdateBillGrossInfo(sBin);    // RMC 20150120
                TransactionLog.TransLog.UpdateLog(pageCheck.bin1.GetBin(), "REN", pageCheck.txtTaxYear.Text, pageCheck.m_sFormStatus, pageCheck.m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                MessageBox.Show("Record with BIN :" + sBin + " \nwith Application No." + sAppNo + "saved for renewal.");

                pageCheck.btnSave.Enabled = false;
                pageCheck.m_strTempBIN = "";
                pageCheck.btnAddlBns.Enabled = true;
                pageCheck.btnAddNew.Enabled = true;
            }
        }

        public void BussSave()
        {
            Serials.UpdateBussSerial();
            //string strYear = DateTime.Now.Year.ToString();
            string strYear = AppSettingsManager.GetCurrentDate().Year.ToString();    // RMC 20110725
            //string m_sBin = "129-00-" + strYear + "-" + Serials.GetBussSerial();

            string m_sBin = StringUtilities.StringUtilities.Left(pageCheck.bin1.GetBin(), 6) + "-" + strYear + "-" + Serials.GetBussSerial();

            // RMC 20110901 Added validation if Lessor is a registered business (s)
            if (!ValidateLessor(m_sBin)) // RMC 20111012 added BIN in auto-tagging of unknown lessor 
            {
                MessageBox.Show("Transaction cancelled.\nLessor is subject for inspection", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // RMC 20110901 Added validation if Lessor is a registered business (e)
            else
            {
                if (pageCheck.m_sFormStatus == "BUSS-ADD-NEW")
                {
                    m_strModuleCode = "ABA";
                    m_strTableUpdated = "businesses";

                    UpdateOtherInfoBin(m_sBin, pageCheck.m_strTempBIN); // RMC 20150117
                    SaveUpdateBns(m_sBin, pageCheck.m_sFormStatus);
                    //UpdateOtherInfoBin(m_sBin, pageCheck.m_strTempBIN);   // RMC 20150117
                    pageCheck.bin1.txtTaxYear.Text = m_sBin.Substring(7, 4);
                    pageCheck.bin1.txtBINSeries.Text = m_sBin.Substring(12, 7);

                    UpdateBusinessMapping(m_sBin);

                    UpdateOtherOfficeTable(m_sBin, "NEW");  // RMC 20141228 modified permit printing (lubao)

                    TransactionLog.TransLog.UpdateLog(pageCheck.bin1.GetBin(), pageCheck.cmbBnsStat.Text.Trim().Substring(0, 3), pageCheck.txtTaxYear.Text, pageCheck.m_sFormStatus, pageCheck.m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                }
                MessageBox.Show("Record with BIN :" + m_sBin + " saved.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pageCheck.btnSave.Enabled = false;
                pageCheck.m_strTempBIN = "";
                pageCheck.btnAddlBns.Enabled = true;
                pageCheck.btnAddNew.Enabled = true;  // RMC 20111006 Added 'Add New' button in Business Records-Add module

                //add transferfile blob here

            }
        }

        public void ClearControls(int iGroup)
        {
            if (iGroup == 1)
            {
                pageCheck.txtOldBin.Text = "";
                pageCheck.txtTelNo.Text = "";
                pageCheck.txtBnsName.Text = "";
                pageCheck.txtBnsAddNo.Text = "";
                pageCheck.txtBnsStreet.Text = "";
                pageCheck.cmbBnsBrgy.Text = "";
                pageCheck.txtBnsZone.Text = "";
                pageCheck.cmbBnsDist.Text = "";
                pageCheck.cmbBnsOrgnKind.Text = "";
                pageCheck.txtBnsType.Text = "";
                pageCheck.txtMPNo.Text = "";
                pageCheck.txtTaxYear.Text = "";
                pageCheck.txtBussPlate.Text = "";   // RMC 20110819 added capturing/viewing of Business Plate

                pageCheck.txtOwnCode.Text = "";
                pageCheck.txtOwnLn.Text = "";
                pageCheck.txtOwnFn.Text = "";
                pageCheck.txtOwnMi.Text = "";
                pageCheck.txtOwnAddNo.Text = "";
                pageCheck.txtOwnStreet.Text = "";
                pageCheck.cmbOwnBrgy.Text = "";
                pageCheck.cmbOwnDist.Text = "";
                pageCheck.txtOwnMun.Text = "";
                pageCheck.txtOwnProv.Text = "";
                pageCheck.txtOwnZip.Text = "";
                pageCheck.txtPINLand.Text = "";
                pageCheck.txtPINBldg.Text = "";
                pageCheck.txtPINAddNo.Text = "";
                pageCheck.txtPINStreet.Text = "";
                pageCheck.cmbPINBrgy.Text = "";
                pageCheck.cmbPINDist.Text = "";
                pageCheck.txtPINMun.Text = "";
                pageCheck.txtPINProv.Text = "";
                pageCheck.cmbLandPIN.Items.Clear();
                pageCheck.cmbBldgCode.Items.Clear();
                pageCheck.txtBldgName.Text = "";
                pageCheck.chkGISLink.Checked = false;

            }
            if (iGroup == 2)
            {
                pageCheck.txtSinceWhen.Text = "";
                pageCheck.txtMonthlyRental.Text = "";
                pageCheck.txtBnsOwnCode.Text = "";
                pageCheck.txtBnsOwnLn.Text = "";
                pageCheck.txtBnsOwnFn.Text = "";
                pageCheck.txtBnsOwnMi.Text = "";
                pageCheck.txtBnsOwnAddNo.Text = "";
                pageCheck.txtBnsOwnMun.Text = "";
                pageCheck.txtBnsOwnProv.Text = "";
                pageCheck.txtBnsOwnStreet.Text = "";
                pageCheck.cmbBnsOwnBrgy.Text = "";
                pageCheck.cmbBnsOwnDist.Text = "";
                pageCheck.txtBnsOwnZip.Text = "";
            }

            // RMC 20111006 Added 'Add New' button in Business Records-Add module (s)
            if (iGroup == 3)
            {
                pageCheck.txtPrevOwnCode.Text = "";
                pageCheck.txtPrevOwnFn.Text = "";
                pageCheck.txtPrevOwnLn.Text = "";
                pageCheck.txtPrevOwnMi.Text = "";
                pageCheck.txtPrevOwnStreet.Text = "";
                pageCheck.txtPrevOwnAddNo.Text = "";
                pageCheck.cmbPrevOwnBrgy.Text = "";
                pageCheck.cmbPrevOwnDist.Text = "";
                pageCheck.txtPrevOwnMun.Text = "";
                pageCheck.txtPrevOwnProv.Text = "";
                pageCheck.txtPrevOwnZip.Text = "";
                pageCheck.cmbBnsStat.Text = "";
                pageCheck.txtInitialCap.Text = "";
                pageCheck.txtPrevGross.Text = "";
                pageCheck.txtCurrentGross.Text = "";
                pageCheck.txtDTI.Text = "";
                pageCheck.txtSSSNo.Text = "";
                pageCheck.txtCTCNo.Text = "";
                pageCheck.txtTINNo.Text = "";
                pageCheck.dtpDTIIssued.Value = AppSettingsManager.GetCurrentDate();
                pageCheck.dtpDTIIssued.Value = AppSettingsManager.GetCurrentDate();
                pageCheck.dtpCTCNoIssued.Value = AppSettingsManager.GetCurrentDate();
                pageCheck.dtpTINNoIssued.Value = AppSettingsManager.GetCurrentDate();
                pageCheck.dtpOperationStart.Value = AppSettingsManager.GetCurrentDate();
                pageCheck.dtpApplicationDate.Value = AppSettingsManager.GetCurrentDate();

            }
            // RMC 20111006 Added 'Add New' button in Business Records-Add module (e)
        }

        public void UpdateOtherInfoBin(string strBIN, string strTempBIN)
        {
            result.Query = string.Format("update other_info set bin = '{0}' where bin = '{1}'", strBIN, strTempBIN);
            if (result.ExecuteNonQuery() == 0)
            {
            }

            result.Query = "update addl_info_tmp set bin = '" + strBIN + "' where bin = '" + strTempBIN + "'";
            if (result.ExecuteNonQuery() == 0)
            { }

            result.Query = "update addl_info set bin = '" + strBIN + "' where bin = '" + strTempBIN + "'";
            if (result.ExecuteNonQuery() == 0)
            { }
        }

        public void BussUpdate()
        {
            m_strModuleCode = "ABE";
            m_strTableUpdated = "businesses";

            // RMC 20110901 Added validation if Lessor is a registered business (s)
            if (!ValidateLessor(pageCheck.bin1.GetBin()))   // RMC 20111012 added BIN in auto-tagging of unknown lessor 
            {
                MessageBox.Show("Transaction cancelled.\nLessor is subject for inspection", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // RMC 20110901 Added validation if Lessor is a registered business (e)
            else
            {
                if (!CheckConsolGR())
                    return;

                SaveUpdateBns(pageCheck.bin1.GetBin(), pageCheck.m_sFormStatus);
                UpdateOtherTables(pageCheck.bin1.GetBin());
                UpdateBusinessMapping(pageCheck.bin1.GetBin());
                UpdateOtherOfficeTable(pageCheck.bin1.GetBin(), "NEW");  // RMC 20141228 modified permit printing (lubao)

                TransactionLog.TransLog.UpdateLog(pageCheck.bin1.GetBin(), pageCheck.cmbBnsStat.Text.Trim().Substring(0, 3), pageCheck.txtTaxYear.Text, pageCheck.m_sFormStatus, pageCheck.m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                MessageBox.Show("Record with BIN :" + pageCheck.bin1.GetBin() + " updated.");

                pageCheck.btnSave.Enabled = false;
                pageCheck.m_strTempBIN = "";
                pageCheck.btnAddlBns.Enabled = true;
                pageCheck.btnAddNew.Enabled = true;
            }
        }

        public void UpdateOtherTables(string strBin)
        {
            string strBnsCode = AppSettingsManager.GetBnsCodeByDesc(pageCheck.txtBnsType.Text.Trim());
            string sBnsStat = string.Empty;

            if (pageCheck.cmbBnsStat.Text.Trim() == "RENEWAL")
                sBnsStat = "REN";
            else if (pageCheck.cmbBnsStat.Text.Trim() == "RETIRED")
                sBnsStat = "RET";
            else
                sBnsStat = "NEW";

            if (pageCheck.m_strTmpBnsCode != strBnsCode)
            {
                string strYear = "";

                pSet.Query = "select distinct(tax_year) from taxdues ";
                pSet.Query += string.Format("where bin = '{0}' and bns_code_main = '{1}' order by tax_year desc", strBin, pageCheck.m_strTmpBnsCode);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        strYear = pSet.GetString("tax_year");
                    }
                }
                pSet.Close();

                // taxdues
                pSet.Query = string.Format("update taxdues set bns_code_main = '{0}' where bin = '{1}' and bns_code_main = '{2}'", strBnsCode, strBin, pageCheck.m_strTmpBnsCode);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                if (strYear != "")
                {
                    // taxdues_hist
                    pSet.Query = string.Format("update taxdues_hist set bns_code_main = '{0}' where bin = '{1}' and bns_code_main = '{2}' and tax_year = '{3}'", strBnsCode, strBin, pageCheck.m_strTmpBnsCode, strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    //bill_hist
                    pSet.Query = string.Format("update bill_hist set bns_code_main = '{0}' where bin = '{1}' and bns_code_main = '{2}' and tax_year = '{3}'", strBnsCode, strBin, pageCheck.m_strTmpBnsCode, strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                }

                // bill_no
                pSet.Query = string.Format("update bill_no set bns_code_main = '{0}' where bin = '{1}' and bns_code_main = '{2}'", strBnsCode, strBin, pageCheck.m_strTmpBnsCode);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                // UPDATE TAX_AND_FEES
                pSet.Query = string.Format("update tax_and_fees set bns_code_main = '{0}' where bin = '{1}' and bns_code_main = '{2}'", strBnsCode, strBin, pageCheck.m_strTmpBnsCode);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                this.UpdateOtherInfo(strBin, strBnsCode);

            }

            pSet.Query = "update pay_hist set ";
            pSet.Query += string.Format("bns_stat = '{0}', ", sBnsStat);
            pSet.Query += string.Format("tax_year = '{0}', ", pageCheck.txtTaxYear.Text);
            pSet.Query += string.Format("bns_user = '{0}' ", AppSettingsManager.SystemUser.UserCode);
            pSet.Query += string.Format("where bin = '{0}' and tax_year = '{1}'", strBin, pageCheck.txtTaxYear.Text);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = "update or_table set ";
            pSet.Query += string.Format("bns_code_main = '{0}'", strBnsCode);
            pSet.Query += " where or_no in ";
            pSet.Query += string.Format("(select or_no from pay_hist where bin = '{0}')", strBin);
            pSet.Query += string.Format(" and bns_code_main = '{0}'", pageCheck.m_strTmpBnsCode);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            /* add blob transfer here
            if (!m_sFile.IsEmpty())
                TransferFile(sBIN); */



        }

        public void BussDelete()
        {
            bool bAns = false;
            string strBin = pageCheck.bin1.GetBin().Trim();
            string strBrgy = string.Empty;
            string strDist = string.Empty;
            string strZone = string.Empty;

            m_strModuleCode = "ABD";
            m_strTableUpdated = "businesses";

            if (strBin.Length < 19)
            {
                MessageBox.Show("Select record to delete.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this record permanently?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}'", strBin);
                pSet.Query += " and (data_mode = 'ONL' or data_mode = 'OFL')";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Deletion not allowed, record has existing online/offline payments.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        pSet.Close();

                        pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and data_mode <> 'UNP' ", strBin);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                /*    if (MessageBox.Show("Warning! Record has existing payments, continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        bAns = true;*/

                                // RMC 20110725 disable deletion of buss record if with posted payment (s)
                                MessageBox.Show("Deletion not allowed, record has existing payment.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);  // RMC 20110808 delete "online/offline"
                                return;
                                // RMC 20110725 disable deletion of buss record if with posted payment (e)
                            }
                            else
                                bAns = true;
                        }
                        pSet.Close();
                    }
                }

                if (bAns)
                {
                    pSet.Query = string.Format("select * from businesses where bin = '{0}'", strBin);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            strBrgy = pSet.GetString("bns_brgy");
                            strDist = pSet.GetString("bns_dist");
                            strZone = pSet.GetString("bns_zone");
                        }
                    }
                    pSet.Close();

                    pSet.Query = string.Format("delete from businesses where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from buss_hist where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bus_addl_info where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from taxdues_hist where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from taxdues where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_hist where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_no where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from btax where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from tax_and_fees where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from reg_table where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    OracleResultSet pRec = new OracleResultSet();

                    pRec.Query = string.Format("select distinct or_no from pay_hist where bin = '{0}'", strBin);
                    if (pRec.Execute())
                    {
                        string strConvert = string.Empty;

                        while (pRec.Read())
                        {
                            strConvert = pRec.GetString("or_no");

                            pSet.Query = string.Format("delete from or_table where or_no = '{0}'", strConvert);
                            if (pSet.ExecuteNonQuery() == 0)
                            {
                            }

                            pSet.Query = string.Format("delete from cancelled_or where or_no = '{0}'", strConvert);
                            if (pSet.ExecuteNonQuery() == 0)
                            {
                            }

                        }
                    }
                    pRec.Close();

                    pSet.Query = string.Format("delete from pay_hist where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    // RMC 20120221 added deletion of buss plate when buss record deleted (s)
                    pSet.Query = string.Format("delete from buss_plate where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    UpdateOtherOfficeTable(strBin, "CANCEL");   // RMC 20141228 modified permit printing (lubao)
                    TransactionLog.TransLog.UpdateLog(pageCheck.bin1.GetBin(), pageCheck.cmbBnsStat.Text.Trim().Substring(0, 3), pageCheck.txtTaxYear.Text, pageCheck.m_sFormStatus, pageCheck.m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                    // RMC 20120221 added deletion of buss plate when buss record deleted (e)

                    /*CString sQuery, sServer, sSource, sFileNew, sFile, sFileType, sFileName, sMainDir;
                    sQuery = "select * from directory_tbl";
                    sQuery += " where user_code = '" + pApp->sUser + "' and type_fld = 'BILLING'";
                    pSet.CreateInstance(__uuidof(Recordset));
                    pSet->Open(_bstr_t(sQuery), pApp->m_pConnection.GetInterfacePtr(), adOpenStatic, adLockReadOnly, adCmdText);
                    if (!pSet->adoEOF)
                    {
                        sServer = pApp->GetStrVariant(pSet->GetCollect(_variant_t("ddir_folder")));
                        sSource = pApp->GetStrVariant(pSet->GetCollect(_variant_t("sdir_folder")));
                    }
                    pSet->Close();

                    sMainDir = sServer;
                    sServer = sServer + "\\BILL\\";
                    if (!sDist.IsEmpty())
                        sServer += sDist + "\\";
                    if (!sZone.IsEmpty())
                        sServer += sZone + "\\";
                    if (!sBrgy.IsEmpty())
                        sServer += sBrgy + "\\";
                    MakeSureDirectoryPathExists(sServer, sMainDir);
                                        
                    sFile = sServer + sBIN + "*.*";
                    CFileFind finder;
                    while (finder.FindFile(sFile))
                    {
                        if (finder.FindFile(sFile))
                        {
                            finder.FindNextFile();
                            sFileName = finder.GetFileName();
                            sFileType = sFileName.Right(sFileName.GetLength() - sFileName.ReverseFind('.') - 1);
                            sFile = sServer + sFileName;
                        }
                        sFileNew = CreateTempSourceName(sSource, sFileType);
                        sFileNew = sSource + "\\" + sFileNew;

                        if (!finder.FindFile(sFileNew))
                        {
                            CopyFile(sFile, sFileNew, 0);
                            DeleteFile(sFile);
                        }
                        sFile = sServer + sBIN + "*.*";
                    }
                    // END			
                     * */

                    // add blob here

                    string strObj = strBin.Trim() + "/" + pageCheck.txtTaxYear.Text;
                    if (AuditTrail.AuditTrail.InsertTrail(m_strModuleCode, m_strTableUpdated, StringUtilities.StringUtilities.HandleApostrophe(strObj)) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("BIN :" + strBin + " has been deleted.");

                }

                pageCheck.btnSave.Enabled = false;
                pageCheck.m_strTempBIN = "";
                pageCheck.btnAddlBns.Enabled = true;

            }
        }

        public void UpdateRecord()
        {
            string strYear = string.Empty;
            string strBin = pageCheck.bin1.GetBin();

            pSet.Query = string.Format("select tax_year from business_que where bin = '{0}'", strBin);
            if (pSet.Execute())
            {
                if (pSet.Read())
                    strYear = pSet.GetString("tax_year").Trim();
            }
            pSet.Close();

            if (strYear != "")
            {
                pSet.Query = string.Format("delete from business_que where bin = '{0}'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("delete from bill_no where bin = '{0}'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("delete from taxdues where bin = '{0}'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("delete from bill_hist where bin = '{0}' and tax_year = '{1}'", strBin, strYear);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("delete from taxdues_hist where bin = '{0}' and tax_year = '{1}'", strBin, strYear);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

                pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                {
                }
            }

            pSet.Query = "insert into buss_hist";
            pSet.Query += string.Format(" select * from businesses where bin = '{0}'", strBin);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            m_strModuleCode = "ABU";
            m_strTableUpdated = "businesses";
            pageCheck.txtMemo.Text = "UPDATE RECORD";
            SaveUpdateBns(strBin, pageCheck.m_sFormStatus);

            TransactionLog.TransLog.UpdateLog(pageCheck.bin1.GetBin(), pageCheck.cmbBnsStat.Text.Trim().Substring(0, 3), pageCheck.txtTaxYear.Text, pageCheck.m_sFormStatus, pageCheck.m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

            MessageBox.Show("Record with BIN :" + strBin + " updated.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void CancelUpdate()
        {
            OracleResultSet pRec = new OracleResultSet();

            string strBin = pageCheck.bin1.GetBin();
            string strYear = string.Empty;
            string strYearPrev = string.Empty;
            string strORNo = string.Empty;

            strYear = pageCheck.txtTaxYear.Text;

            pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' and data_mode = 'POS'", strBin, strYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("Payment exist for tax year " + strYear + ". Delete posted payment first.", "Cancel Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    pSet.Close();

                    pSet.Query = string.Format("select distinct(tax_year) from buss_hist where bin = '{0}' order by 1 desc", strBin);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            strYearPrev = pSet.GetString(0);
                        }
                    }
                    pSet.Close();

                    pSet.Query = string.Format("delete from businesses where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from taxdues where bin = '{0}' and tax_year = '{1}'", strBin, strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pRec.Query = string.Format("select or_no from pay_hist where bin = '{0}' and tax_year = '{1}'", strBin, strYear);
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            strORNo = pRec.GetString(0).Trim();

                            pSet.Query = string.Format("delete from or_table where or_no = '{0}' and tax_year = '{1}'", strORNo, strYear);
                            if (pSet.ExecuteNonQuery() == 0)
                            {
                            }
                        }
                    }
                    pRec.Close();


                    pSet.Query = string.Format("delete from pay_hist where bin = '{0}' and tax_year = '{1}'", strBin, strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year = '{1}'", strBin, strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = "insert into businesses";
                    pSet.Query += " select * from buss_hist";
                    pSet.Query += string.Format(" where bin = '{0}' and tax_year = '{1}'", strBin, strYearPrev);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from buss_hist where bin = '{0}' and tax_year = '{1}'", strBin, strYearPrev);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    string strObj = strBin.Trim() + "/" + pageCheck.txtTaxYear.Text;
                    if (AuditTrail.AuditTrail.InsertTrail("ABCU", "businesses/other tables", StringUtilities.StringUtilities.HandleApostrophe(strObj)) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    TransactionLog.TransLog.UpdateLog(pageCheck.bin1.GetBin(), "CANCEL", pageCheck.txtTaxYear.Text, pageCheck.m_sFormStatus, pageCheck.m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                    MessageBox.Show("BIN :" + strBin + " has been cancelled.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void AppNewUpdate()
        {
            string sBin = pageCheck.bin1.GetBin();

            m_strModuleCode = "AANABE";
            m_strTableUpdated = "business_que";

            // RMC 20110901 Added validation if Lessor is a registered business (s)
            if (!ValidateLessor(sBin))  // RMC 20111012 added BIN in auto-tagging of unknown lessor 
            {
                MessageBox.Show("Transaction cancelled.\nLessor is subject for inspection", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // RMC 20110901 Added validation if Lessor is a registered business (e)
            else
            {
                SaveUpdateBns(sBin, pageCheck.m_sFormStatus);
                //UpdateOtherTables(pageCheck.bin1.GetBin()); // RMC 20141217 adjustments
                UpdateBusinessMapping(sBin);
                UpdateOtherOfficeTable(pageCheck.bin1.GetBin(), "NEW");  // RMC 20141228 modified permit printing (lubao)

                DeleteOtherTables(sBin);
                UpdateBillGrossInfo(sBin);    // RMC 20150120

                TransactionLog.TransLog.UpdateLog(pageCheck.bin1.GetBin(), "NEW", pageCheck.txtTaxYear.Text, pageCheck.m_sFormStatus, pageCheck.m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                MessageBox.Show("Record with BIN :" + sBin + " updated.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pageCheck.btnSave.Enabled = false;
                pageCheck.m_strTempBIN = "";
                pageCheck.btnAddlBns.Enabled = true;
                pageCheck.btnAddNew.Enabled = true;
            }
        }

        private void UpdateOtherInfo(string strBin, string strBnsCode)
        {
            pSet.Query = string.Format("select * from other_info where bin = '{0}' and bns_code = '{1}' and tax_year = '{2}'", strBin, pageCheck.m_strTmpBnsCode, pageCheck.txtTaxYear.Text);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from other_info where bin = '{0}' and bns_code = '{1}' and tax_year = '{2}'", strBin, strBnsCode, pageCheck.txtTaxYear.Text);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            pSet.Close();
                            if (pageCheck.m_strTmpBnsCode != strBnsCode)    // RMC 20141217 adjustments
                            {
                                pSet.Query = string.Format("delete from other_info where bin = '{0}' and bns_code = '{1}' and tax_year = '{2}'", strBin, pageCheck.m_strTmpBnsCode, pageCheck.txtTaxYear.Text);
                                if (pSet.ExecuteNonQuery() == 0)
                                {
                                }
                            }
                        }
                        else
                        {
                            pSet.Close();
                            pSet.Query = string.Format("update other_info set bns_code = '{0}' where bin = '{1}' and bns_code = '{2}' and tax_year = '{3}'", strBnsCode, strBin, pageCheck.m_strTmpBnsCode, pageCheck.txtTaxYear.Text);
                            if (pSet.ExecuteNonQuery() == 0)
                            {
                            }
                        }
                    }
                }

            }
        }

        public void AppRenUpdate()
        {
            string sBin = pageCheck.bin1.GetBin();

            m_strModuleCode = "AARABE";
            m_strTableUpdated = "business_que";

            if (!ValidateMappingChanges(sBin))
                return;

            // RMC 20110901 Added validation if Lessor is a registered business (s)
            if (!ValidateLessor(sBin))  // RMC 20111012 added BIN in auto-tagging of unknown lessor 
            {
                MessageBox.Show("Transaction cancelled.\nLessor is subject for inspection", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // RMC 20110901 Added validation if Lessor is a registered business (e)
            else
            {
                if (!CheckConsolGR())
                    return;

                SaveUpdateBns(sBin, pageCheck.m_sFormStatus);
                UpdateBusinessMapping(sBin);

                DeleteOtherTables(sBin);
                UpdateBillGrossInfo(sBin);    // RMC 20150120

                TransactionLog.TransLog.UpdateLog(pageCheck.bin1.GetBin(), "REN", pageCheck.txtTaxYear.Text, pageCheck.m_sFormStatus, pageCheck.m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                MessageBox.Show("Record with BIN :" + sBin + " updated.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pageCheck.btnSave.Enabled = false;
                pageCheck.m_strTempBIN = "";
                pageCheck.btnAddlBns.Enabled = true;
                pageCheck.btnAddNew.Enabled = true;
            }
        }

        private void DeleteOtherTables(string strBin)
        {
            string strBnsCode = AppSettingsManager.GetBnsCodeByDesc(pageCheck.txtBnsType.Text.Trim());
            string strCurrentGross = string.Empty;
            string strCurrentCapital = string.Empty;
            string strCurrentOperDate = string.Empty;

            bool bChanged = false;

            if (pageCheck.m_sFormStatus == "NEW-APP-EDIT")
            {
                strCurrentCapital = string.Format("{0:#,##.00}", Convert.ToDouble(pageCheck.txtInitialCap.Text.ToString()));
                strCurrentOperDate = string.Format("{0:MM/dd/yyyy}", pageCheck.dtpOperationStart.Text);

                if (pageCheck.m_strTmpBnsCode != strBnsCode || pageCheck.m_strTempCapital != strCurrentCapital
                    || pageCheck.m_strOperationStart != strCurrentOperDate)
                    bChanged = true;
            }
            else if (pageCheck.m_sFormStatus == "REN-APP-EDIT")
            {
                strCurrentGross = string.Format("{0:#,##.00}", Convert.ToDouble(pageCheck.txtCurrentGross.Text.ToString()));

                if (pageCheck.m_strTmpBnsCode != strBnsCode || pageCheck.m_strTempGross != strCurrentGross)
                    bChanged = true;
            }

            if (bChanged)
            {
                string strYear = "";

                pSet.Query = "select distinct(tax_year) from taxdues ";
                pSet.Query += string.Format("where bin = '{0}' and bns_code_main = '{1}' order by tax_year desc", strBin, pageCheck.m_strTmpBnsCode);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        strYear = pSet.GetString("tax_year").Trim();
                    }
                }
                pSet.Close();

                //if (strYear != pageCheck.txtTaxYear.Text && strYear != "")

                // RMC 20120316 corrected deletion of billing tables when application was edited (s)
                if (strYear.Trim() == "")
                    strYear = pageCheck.txtTaxYear.Text;
                // RMC 20120316 corrected deletion of billing tables when application was edited (e)

                if (strYear == pageCheck.txtTaxYear.Text)  // RMC 20120316 corrected deletion of billing tables when application was edited
                {
                    pSet.Query = string.Format("delete from taxdues where bin = '{0}'", strBin);
                    pSet.Query += string.Format(" and bns_code_main = '{0}'", pageCheck.m_strTmpBnsCode);
                    pSet.Query += string.Format(" and tax_year = '{0}'", strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from taxdues_hist where bin = '{0}'", strBin);
                    pSet.Query += string.Format(" and bns_code_main = '{0}'", pageCheck.m_strTmpBnsCode);
                    pSet.Query += string.Format(" and tax_year = '{0}'", strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_no where bin = '{0}'", strBin);
                    pSet.Query += string.Format(" and bns_code_main = '{0}'", pageCheck.m_strTmpBnsCode);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_hist where bin = '{0}'", strBin);
                    pSet.Query += string.Format(" and bns_code_main = '{0}'", pageCheck.m_strTmpBnsCode);
                    pSet.Query += string.Format(" and tax_year = '{0}'", strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from btax where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from tax_and_fees where bin = '{0}'", strBin);
                    pSet.Query += " and fees_code <> '01'";
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    pSet.Query = string.Format("update tax_and_fees set bns_code_main = '{0}' where bin = '{1}' and bns_code_main = '{2}'", strBnsCode, strBin, pageCheck.m_strTmpBnsCode);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from addl where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from fire_tax where bin = '{0}'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from reg_table where bin = '{0}' and tax_year = '{1}'", strBin, strYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}' and tax_year = '{1}' and bns_code = '{2}'", strBin, strYear, pageCheck.m_strTmpBnsCode);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    this.UpdateOtherInfo(strBin, strBnsCode);

                }

            }
        }

        public void CancelApplication()
        {
            string strBin = pageCheck.bin1.GetBin();
            string strYear = string.Empty;
            strYear = pageCheck.txtTaxYear.Text;

            m_strModuleCode = "AACA";
            m_strTableUpdated = "business_que";

            if (!Granted.Grant("AACA-SOA"))
            {
                pSet.Query = string.Format("select * from billing_tagging where bin = '{0}' and tax_year = '{1}'", strBin, strYear);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        MessageBox.Show("Cannot cancel application. Records already been processed.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                pSet.Close();

            }

            pSet.Query = string.Format("delete from business_que where bin = '{0}' and tax_year = '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }
           

            strYear = "";
            pSet.Query = string.Format("select * from businesses where bin = '{0}'", strBin);
            if (pSet.Execute())
            {
                if (pSet.Read())
                    strYear = pSet.GetString("tax_year");
            }
            pSet.Close();

            pSet.Query = string.Format("delete from taxdues where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            // RMC 20120126 added deletion in ass_taxdues when application cancelled (s)
            pSet.Query = string.Format("delete from ass_taxdues where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }
            // RMC 20120126 added deletion in ass_taxdues when application cancelled (e)

            pSet.Query = string.Format("delete from taxdues_hist where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from bill_no where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from bill_hist where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }
            // RMC 20170109 enable adding of new buss in renewal application (s)
            pSet.Query = string.Format("delete from addl_bns_que where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }
            // RMC 20170109 enable adding of new buss in renewal application (e)
            pSet.Query = string.Format("delete from btax where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from tax_and_fees where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from other_info where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from other_info_dec where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from billing_tagging where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from addl where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from fire_tax where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from reg_table where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("update gis_zoning set bin = '' where bin = '{0}'", strBin);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            // RMC 20111216 consider business-mapped-unofficial in cancel application transaction (s)
            pSet.Query = string.Format("update btm_temp_businesses set bin = ' ' where bin = '{0}'", strBin);
            if (pSet.ExecuteNonQuery() == 0)
            { }
            // RMC 20111216 consider business-mapped-unofficial in cancel application transaction (e)

            // RMC 20120127 delete record in billing-related tables in cancel appilication transaction (s)
            pSet.Query = string.Format("delete from treasurers_module where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from treasurers_module_tmp where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            pSet.Query = string.Format("delete from gross_monitoring where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }
            pSet.Query = string.Format("delete from ass_bill_gross_info where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }
            pSet.Query = string.Format("delete from ass_taxdues where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }
            pSet.Query = string.Format("delete from reass_bill_gross_info where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }
            pSet.Query = string.Format("delete from reass_taxdues where bin = '{0}' and tax_year > '{1}'", strBin, strYear);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }
            // RMC 20120127 delete record in billing-related tables in cancel appilication transaction (e)

            // RMC 20120320 Added deletiong in buss_plate in cancel application (s)
            pSet.Query = string.Format("delete from buss_plate where bin = '{0}'", strBin);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }
            // RMC 20120320 Added deletiong in buss_plate in cancel application (e)

            // RMC 20180118 added cancellation of permit update appl queued on prev year (s)
            if (pageCheck.DupCheck(string.Format("permit_update_appl where bin = '{0}' and data_mode = 'QUE'", strBin)))
            {
                pSet.Query = string.Format("delete from transfer_table where bin = '{0}'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from change_class_tbl where bin = '{0}'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from addl_bns_que where bin = '{0}'", strBin);
                pSet.Query += " and bns_code_main in (select new_bns_code from permit_update_appl";
                pSet.Query += string.Format(" where bin = '{0}' and data_mode = 'QUE')", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from permit_update_appl where bin = '{0}' and data_mode = 'QUE'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from taxdues where bin = '{0}'", strBin);
                pSet.Query += " and due_state = 'P'";
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }
            // RMC 20180118 added cancellation of permit update appl queued on prev year (e)

            UpdateOtherOfficeTable(strBin, "CANCEL");   // RMC 20141228 modified permit printing (lubao)

            TransactionLog.TransLog.UpdateLog(pageCheck.bin1.GetBin(), "CANCEL", pageCheck.txtTaxYear.Text, pageCheck.m_sFormStatus, pageCheck.m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

            string strObj = strBin.Trim() + "/" + pageCheck.txtTaxYear.Text;
            if (AuditTrail.AuditTrail.InsertTrail(m_strModuleCode, m_strTableUpdated, StringUtilities.StringUtilities.HandleApostrophe(strObj)) == 0)
            {
                pSet.Rollback();
                pSet.Close();
                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("BIN :" + strBin + " has been cancelled!", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // RMC 20161219 disable 'Void' button after cancel application (s)
            pageCheck.btnSave.Enabled = false;
            pageCheck.m_strTempBIN = "";
            // RMC 20161219 disable 'Void' button after cancel application (e)
        }

        private bool OnCheckIfRetiredBns()
        {
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "Select * from businesses";
            pRec.Query += string.Format(" where bns_nm = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.txtBnsName.Text.Trim())));
            pRec.Query += string.Format(" and bns_street = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.txtBnsStreet.Text.Trim())));
            pRec.Query += string.Format(" and bns_house_no = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.txtBnsAddNo.Text.Trim())));
            pRec.Query += string.Format(" and bns_brgy = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.cmbBnsBrgy.Text.Trim())));
            pRec.Query += string.Format(" and bns_mun  = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.txtBnsMun.Text.Trim())));
            pRec.Query += string.Format(" and bns_dist = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.cmbBnsDist.Text.Trim())));
            pRec.Query += string.Format(" and bns_zone = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.txtBnsZone.Text.Trim())));
            pRec.Query += " and bns_stat = 'RET'";
            pRec.Query += string.Format(" and own_code = '{0}'", pageCheck.txtOwnCode.Text.Trim());
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    return true;
                }
                else
                    return false;
            }
            pRec.Close();

            return true;
        }

        private bool ValidateNewApplication(string sFrmStat)
        {
            OracleResultSet pRec = new OracleResultSet();

            if (sFrmStat == "NEW-APP" || sFrmStat == "NEW-APP-EDIT")
            {
                pRec.Query = "select * from retired_bns where bin in ";
                pRec.Query += string.Format("(select bin from businesses where own_code = '{0}')", pageCheck.txtOwnCode.Text.Trim());
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        if (MessageBox.Show("Taxpayer has a retired business.\nPlease check retired business record.\nDo you want to continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            return false;
                        else
                        {
                            if (OnCheckIfRetiredBns())
                                return false;
                        }
                    }
                    else
                    {
                        pRec.Close();

                        pRec.Query = "select * from retired_bns where bin in ";
                        pRec.Query += "(select distinct bin from addl_bns where bin in ";
                        pRec.Query += string.Format("(select bin from businesses where own_code = '{0}'))", pageCheck.txtOwnCode.Text.Trim());
                        if (pRec.Execute())
                        {
                            if (pRec.Read())
                            {
                                if (MessageBox.Show("Taxpayer has a retired business.\nPPlease check retired business record.\nDo you want to continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    return false;
                                else
                                {
                                    if (OnCheckIfRetiredBns())
                                        return false;
                                }
                            }
                        }
                        pRec.Close();
                    }
                }
                pRec.Close();
            }

            return true;
        }

        private bool ValidateGIS(string sFrmStat)
        {
            OracleResultSet pRec = new OracleResultSet();

            if (sFrmStat != "BUSS-ADD-NEW" && sFrmStat != "NEW-APP")
            {
                pRec.Query = "select zc_no from gis_zoning";
                pRec.Query += string.Format(" where bin = '{0}'", pageCheck.bin1.GetBin());
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        pageCheck.m_sZCNo = pRec.GetString("zc_no");

                        if (pageCheck.cmbBnsBrgy.Text.Trim() != brgyList.GetBarangayName(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.cmbBnsDist.Text.Trim()), pageCheck.m_sZCNo.Substring(3, 4)))
                        {
                            //MessageBox.Show("Business address does not match the zoning location.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            m_sMessage = "Business address does not match the zoning location.";    // RMC 20111201
                            return false;
                        }
                    }
                }
                pRec.Close();
            }
            else if (pageCheck.m_sZCNo.Trim() != "")
            {
                if (pageCheck.cmbBnsBrgy.Text.Trim() != brgyList.GetBarangayName(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.cmbBnsDist.Text.Trim()), pageCheck.m_sZCNo.Substring(3, 4)))
                {
                    //MessageBox.Show("Business address does not match the zoning location.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    m_sMessage = "Business address does not match the zoning location.";    // RMC 20111201
                    return false;
                }
            }

            // RMC 20111201 (S)
            if (pageCheck.chkGISLink.Checked && (pageCheck.cmbLandPIN.Text.Trim() == "" || pageCheck.cmbBldgCode.Text.Trim() == ""))
            {
                m_sMessage = "Location land pin and bldg code required.";
                return false;
            }
            // RMC 20111201 (E)

            return true;
        }

        // RMC 20110725 (s)
        private double ToDouble(string sValue)
        {
            double dTemp = 0;

            try
            {
                dTemp = Convert.ToDouble(sValue);
            }
            catch
            {
                dTemp = 0;
            }

            return dTemp;
        }

        private bool CheckConsolGR()
        {
            OracleResultSet rupdate = new OracleResultSet();

            if (pageCheck.chkConsolidatedGross.Checked)
            {
                if (pageCheck.m_sRadioMain == "MAIN")
                {
                    if (pageCheck.m_sOfcType.Trim() != "")
                    {
                        rupdate.Query = string.Format("delete from consol_gr where bin = '{0}' and ofc_type <> 'SINGLE'", pageCheck.bin1.GetBin());
                        if (rupdate.ExecuteNonQuery() == 0)
                        { }
                    }

                    rupdate.Query = string.Format("insert into consol_gr values('{0}','{1}','{2}')", pageCheck.bin1.GetBin(), pageCheck.m_sRadioMain, pageCheck.bin1.GetBin());
                    if (rupdate.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("Main Branch for Consolidated Gross receipts.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (pageCheck.m_sRadioMain == "BRANCH")
                {
                    if (pageCheck.m_sOfcType.Trim() == "" || pageCheck.m_sOfcType.Trim() == "MAIN")  // no data in concol_gr
                    {
                        MessageBox.Show("Set up link up main branch information first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }

            }
            else
            {
                rupdate.Query = string.Format("delete from consol_gr where bin = '{0}' and ofc_type <> 'SINGLE'", pageCheck.bin1.GetBin());
                if (rupdate.ExecuteNonQuery() == 0)
                { }
            }

            return true;

        }
        // RMC 20110725 (e)

        // RMC 20110831 added validation if with addl_info 
        private bool ValidateAddlInfo()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sDefaultCode = "";
            int iDefaultCtr = 0;
            int iOtherInfoCtr = 0;
            // RMC 20110906 Added saving of Application requirements in Business Records (s)
            string sBIN = "";

            if (pageCheck.m_sFormStatus == "BUSS-ADD-NEW" || pageCheck.m_sFormStatus == "NEW-APP" || pageCheck.m_sFormStatus == "SPL-APP")
            {
                if (pageCheck.bin1.txtTaxYear.Text != "" && pageCheck.bin1.txtBINSeries.Text != "")
                    sBIN = pageCheck.bin1.GetBin();
                else
                    sBIN = pageCheck.m_strTempBIN;
            }
            else
                sBIN = pageCheck.bin1.GetBin();
            // RMC 20110906 Added saving of Application requirements in Business Records (e)


            // GDE 20130624 add temp bin
            if (sBIN.Trim() == string.Empty)
            {
                DateTime dtCurrent = AppSettingsManager.GetSystemDate();
                dtCurrent.Year.ToString();

                sBIN = string.Format("{0:0000#}-{1:00#}-{2:00#}-{3:00#}:{4:00#}:{5:00#}", dtCurrent.Year.ToString(), dtCurrent.Month.ToString(), dtCurrent.Day.ToString(), dtCurrent.Hour.ToString(), dtCurrent.Minute.ToString(), dtCurrent.Second.ToString());
                //AddlInfo.BIN = sBIN;
            }



            //pRec.Query = "select distinct (a.default_code), a.default_desc, a.data_type  from default_code a, default_others b where a.default_code = b.default_code and a.rev_year = b.rev_year and b.default_fee like :1 and a.rev_year = :2";
            //pRec.Query = "select distinct (a.default_code), a.default_desc, a.data_type  from default_code a, default_others b where a.default_code = b.default_code and a.rev_year = b.rev_year and b.default_fee like :1 and a.rev_year = :2";
            pRec.Query = "select distinct (a.default_code), a.default_desc, a.data_type  from default_code a, default_others b where  a.rev_year = b.rev_year and b.default_fee like :1 and a.rev_year = :2";
            pRec.AddParameter(":1", pageCheck.m_strBnsCode.Substring(0, 2) + "%");
            //pRec.AddParameter(":1", pageCheck.m_strBnsCode);
            pRec.AddParameter(":2", ConfigurationAttributes.RevYear);
            if (pRec.Execute())
            {
                OracleResultSet pSetOtherInfo = new OracleResultSet();
                while (pRec.Read())
                {
                    iDefaultCtr++;
                    sDefaultCode = pRec.GetString("default_code").Trim();

                    //pSetOtherInfo.Query = string.Format("select * from other_info where bin = '{0}'", pageCheck.bin1.GetBin());
                    pSetOtherInfo.Query = string.Format("select * from other_info where bin = '{0}'", sBIN);    // RMC 20110906 Added saving of Application requirements in Business Records
                    pSetOtherInfo.Query += string.Format(" and default_code = '{0}'", sDefaultCode);
                    pSetOtherInfo.Query += string.Format(" and tax_year = '{0}'", pageCheck.txtTaxYear.Text);   // RMC 20110908 added field in validation of addtional info
                    if (pSetOtherInfo.Execute())
                    {
                        if (pSetOtherInfo.Read())
                        {
                            iOtherInfoCtr++;
                        }

                    }
                    pSetOtherInfo.Close();
                }
            }
            pRec.Close();

            if (iDefaultCtr > 0 && iOtherInfoCtr == 0) // gde 20130624
                return false;

            // RMC 20111221 added validation of duplicate entries in other_info table (s)
            iDefaultCtr = 0;
            pRec.Query = "select bin, bns_code, default_code, count(*) from other_info ";
            pRec.Query += " where tax_year = '" + pageCheck.txtTaxYear.Text + "' ";
            pRec.Query += " and bin = '" + sBIN + "'";
            pRec.Query += " group by bin, bns_code, default_code having count(*) > 1";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    iDefaultCtr = pRec.GetInt(3);
                }
            }
            pRec.Close();
            // RMC 20111221 added validation of duplicate entries in other_info table (e)

            if (iDefaultCtr > 1)
                return false;

            return true;

        }

        // RMC 20110901 Added validation if Lessor is a registered business
        private bool ValidateLessor(string sBIN)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBnsCode = "";

            if (pageCheck.cmbPlaceBnsStat.Text.Trim() == "RENTED")
            {
                pRec.Query = "select * from bns_table where fees_code = 'B'";
                pRec.Query += " and length(bns_code) = 2 and (bns_desc like '%LESSOR COML%'";
                pRec.Query += " or bns_desc like '%COM% LESSOR%')";
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        sBnsCode = pRec.GetString("bns_code").Trim();
                    }
                }
                pRec.Close();

                // AST 20160203 considered sub-main bns for lessor(s)
                if (string.IsNullOrEmpty(sBnsCode.Trim()))
                {
                    pRec.Query = "SELECT * FROM bns_table WHERE fees_code = 'B' and length(bns_code) = 4 ";
                    pRec.Query += "and bns_desc like '%LESSOR%'";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sBnsCode = pRec.GetString("bns_code").Trim();
                        }
                    }
                    pRec.Close();
                }
                // AST 20160203 considered sub-main bns for lessor(e)
                //pRec.Query = string.Format("select * from businesses where own_code = '{0}'", pageCheck.txtBnsOwnCode.Text.Trim());
                pRec.Query = string.Format("select * from own_names where own_code = '{0}'", pageCheck.txtBnsOwnCode.Text.Trim()); //JARS 20171026
                // AST 20160203 considered sub-main bns for lessor (s)
                //pRec.Query += string.Format(" and substr(bns_code, 1,2) = '{0}'", sBnsCode); 
                //if (sBnsCode.Trim().Length == 2)
                //    pRec.Query += string.Format(" and substr(bns_code, 1,2) = '{0}'", sBnsCode);
                //else if (sBnsCode.Trim().Length == 4)
                //    pRec.Query += string.Format(" and substr(bns_code, 1,4) = '{0}'", sBnsCode);
                // AST 20160203 considered sub-main bns for lessor (e)
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        pRec.Close();
                        return true;
                    }
                    else
                    {
                        pRec.Close();

                        if (MessageBox.Show("Lessor is not registered.\nDo you want to continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            return false;
                        else
                        {
                            // auto-insert to inspector's module
                            //validate if owner already tagged

                            string sIsNumber = "";
                            DateTime dtCurrentDate = AppSettingsManager.GetCurrentDate();
                            string sDate = string.Format("{0:MM/dd/yyyy}", dtCurrentDate);
                            int iStrLen = 0;

                            /*pRec.Query = string.Format("select * from unofficial_info_tbl where own_code = '{0}' and is_number like 'A%'", pageCheck.txtBnsOwnCode.Text.Trim());
                            if(pRec.Execute())
                            {
                                if(pRec.Read())
                                {
                                    sIsNumber = pRec.GetString("is_number").Trim();
                                }
                            }
                            pRec.Close();*/

                            // RMC 20111012 added BIN in auto-tagging of unknown lessor (S)
                            pRec.Query = string.Format("select * from unofficial_dtls where is_number like 'A%' and addition_info like '%{0}%'", sBIN);
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                {
                                    sIsNumber = pRec.GetString("is_number").Trim();
                                }
                            }
                            pRec.Close();
                            // RMC 20111012 added BIN in auto-tagging of unknown lessor (E)

                            if (sIsNumber == "")
                            {
                                //pRec.Query = "select * from unofficial_dtls where is_number like 'A%' order by is_number desc";
                                pRec.Query = "select * from unofficial_dtls where is_number like 'A%' order by to_number(substr(is_number,2)) desc"; //MCR 20141222
                                if (pRec.Execute())
                                {
                                    if (pRec.Read())
                                    {
                                        sIsNumber = pRec.GetString("is_number").Trim();
                                        iStrLen = sIsNumber.Length;

                                        //sIsNumber = sIsNumber.Substring(2, iStrLen);
                                        sIsNumber = StringUtilities.StringUtilities.Right(sIsNumber, iStrLen - 1); // RMC 20111007 corrected error in validating lessor
                                        sIsNumber = "A" + string.Format("{0:##}", Convert.ToInt32(sIsNumber) + 1);  // Note use subscript "A" for Application, auto-generated is_number

                                    }
                                    else
                                    {
                                        sIsNumber = "A1";
                                    }
                                }
                                pRec.Close();
                            }

                            pRec.Query = string.Format("delete from unofficial_dtls where is_number = '{0}'", sIsNumber);
                            if (pRec.ExecuteNonQuery() == 0)
                            { }

                            pRec.Query = string.Format("delete from unofficial_info_tbl where is_number = '{0}'", sIsNumber);
                            if (pRec.ExecuteNonQuery() == 0)
                            { }

                            // RMC 20111012 added BIN in auto-tagging of unknown lessor (s)
                            string sRemarks = "";
                            sRemarks = "AUTO-TAGGED IN BUSINESS APPLICATION OF LESSEE (BIN:" + sBIN + ")";
                            // RMC 20111012 added BIN in auto-tagging of unknown lessor (e)

                            pRec.Query = "insert into unofficial_dtls values(:1,:2,:3,:4,:5,:6)";
                            pRec.AddParameter(":1", AppSettingsManager.SystemUser.UserCode);
                            pRec.AddParameter(":2", sIsNumber);
                            pRec.AddParameter(":3", sDate);
                            pRec.AddParameter(":4", "UNOFFICIAL BUSINESS LESSOR SUBJECT FOR INSPECTION");
                            pRec.AddParameter(":5", sRemarks);  // RMC 20111012 added BIN in auto-tagging of unknown lessor
                            pRec.AddParameter(":6", "I");
                            if (pRec.ExecuteNonQuery() == 0)
                            { }

                            pRec.Query = "insert into unofficial_info_tbl values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14,'')";
                            pRec.AddParameter(":1", sIsNumber);
                            pRec.AddParameter(":2", pageCheck.txtBnsOwnCode.Text.Trim());
                            pRec.AddParameter(":3", "UNKNOWN LESSOR");
                            pRec.AddParameter(":4", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsAddNo.Text.Trim()));    //BNS_HOUSE_NO
                            pRec.AddParameter(":5", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsStreet.Text.Trim()));   //BNS_STREET
                            pRec.AddParameter(":6", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.cmbBnsBrgy.Text.Trim())));   //BNS_BRGY
                            pRec.AddParameter(":7", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsZone.Text.Trim()));    //BNS_ZONE
                            pRec.AddParameter(":8", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(pageCheck.cmbBnsDist.Text.Trim())));    //BNS_DIST
                            pRec.AddParameter(":9", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsMun.Text.Trim()));  //BNS_MUN
                            pRec.AddParameter(":10", StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsProv.Text.Trim()));    //BNS_PROV
                            pRec.AddParameter(":11", ".");
                            pRec.AddParameter(":12", "LESSOR COML");
                            pRec.AddParameter(":13", AppSettingsManager.SystemUser.UserCode);
                            pRec.AddParameter(":14", DateTime.Parse(sDate));
                            if (pRec.ExecuteNonQuery() == 0)
                            { }

                            MessageBox.Show("Lessor is tagged under Unofficial Business for inspection", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (AuditTrail.AuditTrail.InsertTrail("ABIDU-ADD", "multiple table", sIsNumber) == 0)
                            {
                                pRec.Rollback();
                                pRec.Close();
                                MessageBox.Show(pRec.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            return true;
                        }

                    }
                }

            }

            return true;
        }

        private bool ValidateMappingChanges(string strBIN)
        {
            // validate business mapping changes
            OracleResultSet pRec = new OracleResultSet();
            string sApplType = "";
            string sNew = "";
            string sDetails = "";
            string sMessage = "";

            pRec.Query = "select * from btm_update where bin = '" + strBIN + "' and trim(def_settled) is null";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sApplType = pRec.GetString("appl_type");

                    if (sApplType == "CBNS")
                    {
                        sDetails = "Business Name";
                        sNew = pRec.GetString("new_bns_name");

                        if (sNew != pageCheck.txtBnsName.Text.Trim())
                        {
                            sMessage = "Deficiency discovered during business mapping drive.\n1. Saved business name does not match actual business name.\n\nContinue?";
                            if (MessageBox.Show(sMessage, "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return false;
                        }
                    }

                    if (sApplType == "TLOC")
                    {
                        sDetails = "Location";
                        sNew = pRec.GetString("new_bns_loc");

                        if (sNew != GetCurrentLocation())
                        {
                            sMessage = "Deficiency discovered during business mapping drive.\n1. Saved business location does not match actual business location.\n\nContinue?";
                            if (MessageBox.Show(sMessage, "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return false;
                        }

                    }
                    if (sApplType == "TOWN")
                    {
                        sDetails = "Owner";
                        sNew = pRec.GetString("new_own_code");

                        if (sNew != pageCheck.txtOwnCode.Text.Trim())
                        {
                            sMessage = "Deficiency discovered during business mapping drive.\n1. Saved owner's name does not match actual owner's name.\n\nContinue?";
                            if (MessageBox.Show(sMessage, "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return false;
                        }
                    }
                    if (sApplType == "CTYP")
                    {
                        sDetails = "Business Type";
                        sNew = pRec.GetString("new_bns_code");

                        if (sNew != AppSettingsManager.GetBnsCodeByDesc(pageCheck.txtBnsType.Text.Trim()))
                        {
                            sMessage = "Deficiency discovered during business mapping drive.\n1. Saved business type does not match actual business type.\n\nContinue?";
                            if (MessageBox.Show(sMessage, "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return false;
                        }

                    }

                    if (sApplType == "ADDL")
                    {
                        sDetails = "Other Line/s of Business";
                    }

                    if (sApplType == "CBOW")
                    {
                        sDetails = "Lessor's details";
                        sNew = pRec.GetString("new_own_code");

                        if (sNew != pageCheck.txtBnsOwnCode.Text.Trim())
                        {
                            sMessage = "Deficiency discovered during business mapping drive.\n1. Saved lessor's name does not match actual lessor's name.\n\nContinue?";
                            if (MessageBox.Show(sMessage, "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return false;
                        }

                    }

                    if (sApplType == "CPMT")
                    {
                        // this has no bearing in renewal since new permit will be printed after payment
                        sDetails = "Permit No.";
                        sNew = pRec.GetString("permit_no");
                    }

                    if (sApplType == "CTYR")
                    {
                        int iCurrent = 0;
                        int iNew = 0;

                        sDetails = "Tax Year";
                        sNew = string.Format("{0:yyyy}", pRec.GetDateTime("permit_dt"));

                        int.TryParse(sNew, out iNew);
                        int.TryParse(pageCheck.txtTaxYear.Text.ToString(), out iCurrent);

                        if (iNew < iCurrent)
                        {
                            MessageBox.Show("Business has conflict in Mayor's permit year found during Tax Mapping drive.\nCorrect previous record first.", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return false;
                        }
                    }
                    if (sApplType == "AREA")
                    {
                        double dNew = 0;
                        double dCurrent = 0;

                        sDetails = "Area";
                        sNew = pRec.GetString("new_bns_name");
                        double.TryParse(sNew, out dNew);
                        double.TryParse(pageCheck.txtGroundArea.Text.ToString(), out dCurrent);

                        if (dNew != dCurrent)
                        {
                            sMessage = "Deficiency discovered during business mapping drive.\n1. Saved floor area does not match actual floor area.\n\nContinue?";
                            if (MessageBox.Show(sMessage, "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return false;
                        }

                    }
                    if (sApplType == "CSTR")
                    {
                        int iNew = 0;
                        int iCurrent = 0;

                        sDetails = "No. of storeys";
                        sNew = pRec.GetString("new_bns_name");
                        int.TryParse(sNew, out iNew);
                        int.TryParse(pageCheck.txtNoStorey.Text.ToString(), out iCurrent);

                        if (iNew != iCurrent)
                        {
                            sMessage = "Deficiency discovered during business mapping drive.\n1. Saved no. of storeys does not match actual no. of storeys.\n\nContinue?";
                            if (MessageBox.Show(sMessage, "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return false;
                        }
                    }
                }
            }
            pRec.Close();


            return true;
        }


        private string GetCurrentLocation()
        {
            string sHouseNo, sStreet, sMun, sBrgy, sZone, sDistrict, sProv, sBnsAddress, sCurrentLoc;

            if (pageCheck.txtBnsAddNo.Text.Trim() == "." || pageCheck.txtBnsAddNo.Text.Trim() == "")
                sHouseNo = "";
            else
                sHouseNo = pageCheck.txtBnsAddNo.Text.Trim() + " ";
            if (pageCheck.txtBnsStreet.Text.Trim() == "." || pageCheck.txtBnsStreet.Text.Trim() == "")
                sStreet = "";
            else
                sStreet = pageCheck.txtBnsStreet.Text.Trim() + ", ";
            if (pageCheck.cmbBnsBrgy.Text.Trim() == "." || pageCheck.cmbBnsBrgy.Text.Trim() == "")
                sBrgy = "";
            else
                sBrgy = pageCheck.cmbBnsBrgy.Text.Trim() + ", ";
            if (pageCheck.txtBnsProv.Text.Trim() == "." || pageCheck.txtBnsProv.Text.Trim() == "")
                sProv = "";
            else
                sProv = pageCheck.txtBnsProv.Text.Trim();
            if (pageCheck.txtBnsZone.Text.Trim() == "." || pageCheck.txtBnsZone.Text.Trim() == "ZONE" || pageCheck.txtBnsZone.Text.Trim() == "")
                sZone = "";
            else
                sZone = "ZONE " + pageCheck.txtBnsZone.Text.Trim() + " ";
            if (pageCheck.cmbBnsDist.Text.Trim() == "." || pageCheck.cmbBnsDist.Text.Trim() == "")
                sDistrict = "";
            else
                sDistrict = pageCheck.cmbBnsDist.Text.Trim() + ", ";
            if (pageCheck.txtBnsMun.Text.Trim() == "." || pageCheck.txtBnsMun.Text.Trim() == "")
                sMun = "";
            else
                sMun = pageCheck.txtBnsMun.Text.Trim();

            sCurrentLoc = sHouseNo + sStreet + sBrgy + sZone + sDistrict + sMun;

            return sCurrentLoc;
        }

        private void UpdateOwnProfileCode(string sOldOwnCode, string sNewOwnCode)
        {
            OracleResultSet pProfile = new OracleResultSet();

            pProfile.Query = "update own_profile set own_code = '" + sNewOwnCode + "'";
            pProfile.Query += " where own_code = '" + sOldOwnCode + "'";
            if (pProfile.ExecuteNonQuery() == 0)
            { }
        }

        private void LoadOtherInfo(string sTaxYear, string sCurrentYear, string sBIN)
        {
            OracleResultSet pOtherInfo = new OracleResultSet();
            OracleResultSet pCmdOtherInfo = new OracleResultSet();

            string sBnsCode = "";
            string sDefaultCode = "";
            string sDataType = "";
            double dData = 0;
            int iCnt = 0;

            pOtherInfo.Query = "select count(*) from other_info where bin = '" + sBIN + "'";
            pOtherInfo.Query += " and tax_year = '" + sCurrentYear + "'";
            int.TryParse(pOtherInfo.ExecuteScalar(), out iCnt);

            if (iCnt == 0)
            {
                // RMC 20120103 added modification in saving other_info (s)
                pCmdOtherInfo.Query = "delete from other_info where bin = '" + sBIN + "'";
                pCmdOtherInfo.Query += " and tax_year = '" + sCurrentYear + "'";
                pCmdOtherInfo.ExecuteNonQuery();
                // RMC 20120103 added modification in saving other_info (e)

                pOtherInfo.Query = "select * from other_info where bin = '" + sBIN + "'";
                pOtherInfo.Query += " and tax_year = '" + sTaxYear + "' order by bns_code";
                if (pOtherInfo.Execute())
                {
                    while (pOtherInfo.Read())
                    {
                        sBnsCode = pOtherInfo.GetString("bns_code");
                        sDefaultCode = pOtherInfo.GetString("default_code");
                        sDataType = pOtherInfo.GetString("data_type");
                        dData = pOtherInfo.GetDouble("data");

                        if (pOtherInfo.GetDouble("data") > 0)
                        {
                            pCmdOtherInfo.Query = "INSERT INTO other_info VALUES (:1,:2,:3,:4,:5,:6,:7)";
                            pCmdOtherInfo.AddParameter(":1", sBIN);
                            pCmdOtherInfo.AddParameter(":2", sCurrentYear);
                            pCmdOtherInfo.AddParameter(":3", sBnsCode);
                            pCmdOtherInfo.AddParameter(":4", sDefaultCode);
                            pCmdOtherInfo.AddParameter(":5", sDataType);
                            pCmdOtherInfo.AddParameter(":6", dData);
                            pCmdOtherInfo.AddParameter(":7", ConfigurationAttributes.RevYear);
                            pCmdOtherInfo.ExecuteNonQuery();
                        }
                    }
                }
                pOtherInfo.Close();
            }

        }

        private void UpdateBusinessMapping(string sBIN)
        {

            // RMC 20111128 consider Business mapping - unencoded/undeclared in Business Records-Add/Applications-New
            OracleResultSet pBTMCmd = new OracleResultSet();
            OracleResultSet pGisRec = new OracleResultSet();
            try
            {
                pGisRec.CreateNewConnectionGIS();

                string sX = "";
                string sY = "";

                if (pageCheck.m_strInspectionNo.Trim() != "")
                {
                    pBTMCmd.Query = "select * from unofficial_info_tbl where is_number = '" + pageCheck.m_strInspectionNo + "'";
                    if (pBTMCmd.Execute())
                    {
                        if (pBTMCmd.Read())
                        {
                            pBTMCmd.Close();

                            pBTMCmd.Query = "update unofficial_info_tbl set bin_settled = '" + sBIN + "' where is_number = '" + pageCheck.m_strInspectionNo + "' and trim(bin_settled) is null";
                            if (pBTMCmd.ExecuteNonQuery() == 0)
                            { }
                        }
                        else
                        {
                            pBTMCmd.Close();

                            pBTMCmd.Query = "update btm_temp_businesses set bin = '" + sBIN + "' where tbin = '" + pageCheck.m_strInspectionNo + "'";
                            if (pBTMCmd.ExecuteNonQuery() == 0)
                            { }
                        }
                    }
                }

                int iCnt = 0;

                try
                {
                    pGisRec.Query = "select * from gis_business_location where pin = '" + pageCheck.cmbLandPIN.Text.Trim() + "' and bldg_code = '" + pageCheck.cmbBldgCode.Text.Trim() + "'";
                    if (pGisRec.Execute())
                    {
                        if (pGisRec.Read())
                        {
                            sX = pGisRec.GetDouble("x").ToString();
                            sY = pGisRec.GetDouble("y").ToString();
                        }
                    }
                    pGisRec.Close();

                    pGisRec.Query = "select count(*) from gis_businesses where bin = '" + sBIN + "'";
                    int.TryParse(pGisRec.ExecuteScalar(), out iCnt);

                    if (iCnt == 0 && (pageCheck.cmbLandPIN.Text.Trim() != "" && pageCheck.cmbBldgCode.Text.Trim() != ""))
                    {
                        int iID = 0;    // RMC 20120207 modifications in business mapping
                        iID = GetGisID(pGisRec);    // RMC 20120207 modifications in business mapping

                        if (iID == 0)   // RMC 20120207 modifications in business mapping
                        {
                            pGisRec.Query = "insert into gis_businesses values (";
                            pGisRec.Query += "id_seq.nextval, ";
                            pGisRec.Query += "'" + sBIN + "', ";
                            pGisRec.Query += "'', '" + pageCheck.cmbLandPIN.Text.Trim() + "',"; // bldg pin,land pin
                            pGisRec.Query += "'0', '.', '.',";	//legend, code, hotlink
                            pGisRec.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsName.Text.Trim()) + "', ";
                            pGisRec.Query += "'" + sX + "', ";	//longitude
                            pGisRec.Query += "'" + sY + "', ";	//latitude
                            pGisRec.Query += "'', ";
                            pGisRec.Query += "'" + pageCheck.cmbBnsBrgy.Text.Trim() + "', ";
                            pGisRec.Query += "'" + pageCheck.cmbBldgCode.Text.Trim() + "')";
                            if (pGisRec.ExecuteNonQuery() == 0)
                            { }

                        }

                        string sBrgyCode = "";

                        sBrgyCode = AppSettingsManager.GetBrgyCode(pageCheck.cmbBnsBrgy.Text.Trim());

                        pGisRec.Query = "insert into gis_bin values (";
                        pGisRec.Query += "'" + sBIN + "', ";
                        pGisRec.Query += "'', '" + pageCheck.cmbLandPIN.Text.Trim() + "',"; // bldg pin,land pin
                        //pGisRec.Query += "'" + sID + "', ";
                        pGisRec.Query += "" + iID + ", ";   // RMC 20120207 modifications in business mapping
                        pGisRec.Query += "'', ";	// district
                        pGisRec.Query += "'" + sBrgyCode + "', '0', '.', ";
                        pGisRec.Query += "'" + pageCheck.cmbBldgCode.Text.Trim() + "', ";
                        pGisRec.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(pageCheck.txtBnsName.Text.Trim()) + "')";
                        if (pGisRec.ExecuteNonQuery() == 0)
                        { }

                        // RMC 20120207 modifications in business mapping (s)
                        pGisRec.Query = "select * from gis_business_map where bin = '" + sBIN + "'";
                        if (pGisRec.Execute())
                        {
                            if (pGisRec.Read())
                            {
                                pGisRec.Close();

                                pGisRec.Query = "update gis_business_map set mapped = '1'";
                                pGisRec.Query += " where bin = '" + sBIN + "'";
                                if (pGisRec.ExecuteNonQuery() == 0)
                                { }
                            }
                            else
                            {
                                pGisRec.Close();

                                pGisRec.Query = "insert into gis_business_map values (";
                                pGisRec.Query += "'" + sBIN + "', ";
                                pGisRec.Query += "'" + pageCheck.cmbBnsBrgy.Text.Trim() + "', '1')";
                                if (pGisRec.ExecuteNonQuery() == 0)
                                { }
                            }
                        }
                        // RMC 20120207 modifications in business mapping (E)
                    }
                }
                catch { }
            }
            catch { }
        }

        private int GetGisID(OracleResultSet pGisRec2)
        {
            // RMC 20120207 modifications in business mapping
            int iID = 0;

            pGisRec2.Query = "select * from gis_businesses where land_pin = '" + pageCheck.cmbLandPIN.Text.Trim() + "'";
            pGisRec2.Query += " and bldgcode = '" + pageCheck.cmbBldgCode.Text.Trim() + "'";
            if (pGisRec2.Execute())
            {
                if (pGisRec2.Read())
                {
                    iID = pGisRec2.GetInt("id");

                    pGisRec2.Close();

                    pGisRec2.Query = "update gis_businesses set bin = '', legend = '1'";
                    pGisRec2.Query += " where land_pin = '" + pageCheck.cmbLandPIN.Text.Trim() + "'";
                    pGisRec2.Query += " and bldgcode = '" + pageCheck.cmbBldgCode.Text.Trim() + "'";
                    if (pGisRec2.ExecuteNonQuery() == 0)
                    { }
                }
                else
                    pGisRec2.Close();
            }

            return iID;
        }

        private string GetDuplicatePlate(string sPlateNo)
        {
            OracleResultSet result = new OracleResultSet();
            string sBin = "";

            result.Query = string.Format("select * from buss_plate where bns_plate = '{0}' ", sPlateNo);
            if (pageCheck.bin1.GetBin().Length == 19)
                result.Query += string.Format(" and bin <> '{0}'", pageCheck.bin1.GetBin());
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBin = result.GetString("bin");
                }
            }
            result.Close();

            return sBin;
        }

        private bool ValidatePlateNo(string sPlateNo, string sBin)
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = string.Format("select * from buss_plate where bns_plate = '{0}'", sPlateNo);
            if (sBin.Length == 19)
                result.Query += string.Format(" and bin <> '{0}'", sBin);
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();
                    return false;
                }
                else
                {
                    result.Close();
                    return true;
                }
            }
            result.Close();

            return true;

        }

        private bool ValidateOtherInfo()
        {
            // RMC 20140104 Capturing of gender of employee 
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();
            string sDefaultCode = string.Empty;
            string sBIN = "";
            double dValue = 0;
            string sAddlCode = string.Empty;
            double dOtherValue = 0;
            string sTmp = " and bns_code is not null";

            if (pageCheck.m_sFormStatus == "BUSS-ADD-NEW" || pageCheck.m_sFormStatus == "NEW-APP" || pageCheck.m_sFormStatus == "SPL-APP"
                || pageCheck.m_sFormStatus == "REN-APP" || pageCheck.m_sFormStatus == "REN-APP-EDIT"
                || pageCheck.m_sFormStatus == "NEW-APP-EDIT" || pageCheck.m_sFormStatus == "SPL-APP-EDIT")
            {
                if (pageCheck.bin1.txtTaxYear.Text != "" && pageCheck.bin1.txtBINSeries.Text != "")
                {
                    sBIN = pageCheck.bin1.GetBin();
                    sTmp = " and bns_code = '" + AppSettingsManager.GetBnsCodeMain(sBIN) + "'"; //MCR 20150113
                }
                else
                {
                    sBIN = pageCheck.m_strTempBIN;
                    sTmp = " and bns_code = '" + AppSettingsManager.GetBnsCodeByDesc(pageCheck.txtBnsType.Text.Trim()) + "'"; //MCR 20150113
                }
            }
            else
                sBIN = pageCheck.bin1.GetBin();

            //pSet.Query = "select * from default_code where default_desc like '%OF WORKERS%' and REV_YEAR = '" + AppSettingsManager.GetConfigValue("07") + "'  and default_desc not like '%FEE%'"; //MCR 20141119 added and default_desc not like '%FEE%'
            pSet.Query = "select * from default_code where default_desc like 'HEALTH%' and REV_YEAR = '" + AppSettingsManager.GetConfigValue("07") + "'";    // RMC 20150104 addl mods
            if (pSet.Execute())
            {
                if (pSet.Read())
                    sDefaultCode = pSet.GetString("default_code").Trim();
            }
            pSet.Close();

            pSet.Query = "select * from other_info where bin = '" + sBIN + "' and default_code = '" + sDefaultCode + "'";
            pSet.Query += " and tax_year = '" + pageCheck.txtTaxYear.Text + "'" + sTmp;
            if (pSet.Execute())
            {
                if (pSet.Read())
                    dValue = pSet.GetDouble("data");
                else
                {
                    // RMC 20150117 (s)
                    // for newly created temp bin
                    pSet.Close();
                   pSet.Query = "select * from other_info where bin = '" + sBIN + "' and default_code = '" + sDefaultCode + "'";
                   pSet.Query += " and tax_year = '" + pageCheck.txtTaxYear.Text + "'";
                   if (pSet.Execute())
                   {
                       if (pSet.Read())
                       {
                           dValue = pSet.GetDouble("data");


                           pSet.Close();
                           pSet.Query = "update other_info set bns_code = '" + AppSettingsManager.GetBnsCodeByDesc(pageCheck.txtBnsType.Text.Trim()) + "'";
                           pSet.Query += " where bin = '" + sBIN + "' and tax_year = '" + pageCheck.txtTaxYear.Text + "'";
                           pSet.Query += " and trim(bns_code) is null";
                           if (pSet.ExecuteNonQuery() == 0)
                           { }
                       }
                   }
                   pSet.Close();
                   // RMC 20150117 (e)
                }
            }
            pSet.Close();

            pSet.Query = "select * from addl_info_tbl where addl_desc like '%MALE WORKER%' or addl_desc like '%FEMALE WORKER%'";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sAddlCode = pSet.GetString("addl_code");

                    pRec.Query = "select * from addl_info_tmp where bin = '" + sBIN + "'";
                    pRec.Query += " and addl_code = '" + sAddlCode + "'";
                    if (pRec.Execute())
                    {
                        double dTmp = 0;
                        if (pRec.Read())
                        {
                            double.TryParse(pRec.GetString("value"), out dTmp);
                            dOtherValue += dTmp;
                        }
                    }
                    pRec.Close();
                }

                if (sAddlCode == "") // does not apply in lgu
                    dOtherValue = dValue;
            }
            pSet.Close();
            
            if (dValue != dOtherValue)
            {
                //m_sMessage = "Number of workers in Additional Business Information and Other Business Information does not match.\nPlease check.";
                m_sMessage = "Number of workers (for Health) in Additional Business Information and Other Business Information does not match.\nPlease check."; // RMC 20150117

                if (MessageBox.Show("Continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) // AST 20150312 Added This
                    return false;
            }

            if (dOtherValue == 0)
            {
                m_sMessage = "Please update Other Business information.";
                return false;
            }

            return true;
        }

        private void UpdateOtherOfficeTable(string sBIN, string sSwitch)
        {
            // RMC 20141228 modified permit printing (lubao)

            OracleResultSet pCmdT = new OracleResultSet();

            if (sSwitch == "NEW")
            {
                pCmdT.Query = "update zoning set bin = '" + sBIN + "' where bin = '" + pageCheck.m_strInspectionNo + "'";
                if (pCmdT.ExecuteNonQuery() == 0)
                { }

                pCmdT.Query = "update permit_type set bin = '" + sBIN + "' where bin = '" + pageCheck.m_strInspectionNo + "'";
                if (pCmdT.ExecuteNonQuery() == 0)
                { }

                pCmdT.Query = "update annual_insp set bin = '" + sBIN + "' where bin = '" + pageCheck.m_strInspectionNo + "'";
                if (pCmdT.ExecuteNonQuery() == 0)
                { }

                pCmdT.Query = "update emp_names set bin = '" + sBIN + "' where temp_bin = '" + pageCheck.m_strInspectionNo + "'";
                if (pCmdT.ExecuteNonQuery() == 0)
                { }

                // RMC 20150113 corrections in sanitary (s)
                pCmdT.Query = "Update sanitary_bldg_ext set bin = '" + sBIN + "' where bin = '" + pageCheck.m_strInspectionNo + "'";
                if (pCmdT.ExecuteNonQuery() == 0)
                { }
                // RMC 20150113 corrections in sanitary (e)
            }
            else if (sSwitch == "CANCEL")
            {
                OracleResultSet pSet = new OracleResultSet();
                string sTmpBIN = string.Empty;

                pSet.Query = "select * from emp_names where bin = '" + sBIN + "'";
                pSet.Query += " and temp_bin like '" + ConfigurationAttributes.CurrentYear + "%'";  // RMC 20150117
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sTmpBIN = pSet.GetString("temp_bin");
                        
                    }
                }
                pSet.Close();

                if (sTmpBIN != "")
                {
                    pCmdT.Query = "update zoning set bin = '" + sTmpBIN + "' where bin = '" + sBIN + "'";
                    if (pCmdT.ExecuteNonQuery() == 0)
                    { }

                    pCmdT.Query = "update permit_type set bin = '" + sTmpBIN + "' where bin = '" + sBIN + "'";
                    if (pCmdT.ExecuteNonQuery() == 0)
                    { }

                    pCmdT.Query = "update annual_insp set bin = '" + sTmpBIN + "' where bin = '" + sBIN + "'";
                    if (pCmdT.ExecuteNonQuery() == 0)
                    { }

                    pCmdT.Query = "update emp_names set bin = '' where bin = '" + sBIN + "'";
                    if (pCmdT.ExecuteNonQuery() == 0)
                    { }

                    // RMC 20150113 corrections in sanitary (s)
                    pCmdT.Query = "Update sanitary_bldg_ext set bin = '" + sTmpBIN + "' where bin = '" + sBIN + "'";
                    if (pCmdT.ExecuteNonQuery() == 0)
                    { }
                    // RMC 20150113 corrections in sanitary (e)

                    // RMC 20150117 (s)
                    pCmdT.Query = "update other_info set bin = '" + sTmpBIN + "' where bin = '" + sBIN + "'";
                    if (pCmdT.ExecuteNonQuery() == 0)
                    { }

                    pCmdT.Query = "update addl_info set bin = '" + sTmpBIN + "' where bin = '" + sBIN + "'";
                    if (pCmdT.ExecuteNonQuery() == 0)
                    { }
                    // RMC 20150117 (e)
                }
            }
        }

        private void UpdateBillGrossInfo(string sBIN)
        {
            // RMC 20150120
            OracleResultSet pSet = new OracleResultSet();
            string sBnsCode = string.Empty;
            string sTaxYear = string.Empty;
            string sBnsStat = string.Empty;
            double dCapital = 0;
            double dGross = 0;

            // RMC 20171123 enabled special business feature (s)
            if (pageCheck.m_sFormStatus == "SPL-APP" || pageCheck.m_sFormStatus == "SPL-APP-EDIT")
                pSet.Query = "select * from spl_business_que where bin = '" + sBIN + "'";
            else// RMC 20171123 enabled special business feature (e)
            pSet.Query = "select * from business_que where bin = '" + sBIN + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sBnsCode = pSet.GetString("bns_code");
                    sTaxYear = pSet.GetString("tax_year");
                    sBnsStat = pSet.GetString("bns_stat");
                    dCapital = pSet.GetDouble("capital");
                    dGross = pSet.GetDouble("gr_1");

                    // RMC 20150205 discovery delinquent mods (s)
                    int intCurrentYear = 0;
                    int.TryParse(ConfigurationAttributes.CurrentYear, out intCurrentYear);
                    int iTaxYear = 0;
                    int.TryParse(sTaxYear, out iTaxYear);

                    while (iTaxYear <= intCurrentYear)
                    {
                        sTaxYear = string.Format("{0:####}", iTaxYear);
                        UpdateBillGrossInfo(sBIN, sBnsCode, sTaxYear, sBnsStat, dCapital, dGross);
                        iTaxYear = iTaxYear + 1;
                        sBnsStat = "REN";
                    }
                    // RMC 20150205 discovery delinquent mods (e)

                    //UpdateBillGrossInfo(sBIN, sBnsCode, sTaxYear, sBnsStat, dCapital, dGross);
                }
            }
            pSet.Close();

            pSet.Query = "select * from addl_bns where bin = '" + sBIN + "' and tax_yeAR = '" + sTaxYear + "'";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sBnsCode = pSet.GetString("bns_code_main");
                    sTaxYear = pSet.GetString("tax_year");
                    sBnsStat = pSet.GetString("bns_stat");
                    dCapital = pSet.GetDouble("capital");
                    dGross = pSet.GetDouble("gross");

                    // RMC 20150205 discovery delinquent mods (s)
                    int intCurrentYear = 0;
                    int.TryParse(ConfigurationAttributes.CurrentYear, out intCurrentYear);
                    int iTaxYear = 0;
                    int.TryParse(sTaxYear, out iTaxYear);

                    while (iTaxYear <= intCurrentYear)
                    {
                        sTaxYear = string.Format("{0:####}", iTaxYear);
                        UpdateBillGrossInfo(sBIN, sBnsCode, sTaxYear, sBnsStat, dCapital, dGross);
                        iTaxYear = iTaxYear + 1;
                        sBnsStat = "REN";
                    }
                    // RMC 20150205 discovery delinquent mods (e)

                    //UpdateBillGrossInfo(sBIN, sBnsCode, sTaxYear, sBnsStat, dCapital, dGross);
                }
            }
            pSet.Close();

            pSet.Query = "select * from addl_bns_que where bin = '" + sBIN + "' and tax_yeAR = '" + sTaxYear + "'";
            pSet.Query += " and bns_code_main not in (select bns_code_main from addl_bns where bin = '" + sBIN + "' and tax_yeAR = '" + sTaxYear + "')";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sBnsCode = pSet.GetString("bns_code_main");
                    sTaxYear = pSet.GetString("tax_year");
                    sBnsStat = pSet.GetString("bns_stat");
                    dCapital = pSet.GetDouble("capital");
                    dGross = pSet.GetDouble("gross");

                    // RMC 20150205 discovery delinquent mods (s)

                    int intCurrentYear = 0;
                    int.TryParse(ConfigurationAttributes.CurrentYear, out intCurrentYear);
                    int iTaxYear = 0;
                    int.TryParse(sTaxYear, out iTaxYear);

                    while (iTaxYear <= intCurrentYear)
                    {
                        sTaxYear = string.Format("{0:####}", iTaxYear);
                        UpdateBillGrossInfo(sBIN, sBnsCode, sTaxYear, sBnsStat, dCapital, dGross);
                        iTaxYear = iTaxYear + 1;
                        sBnsStat = "REN";
                    }
                    // RMC 20150205 discovery delinquent mods (e)

                    //UpdateBillGrossInfo(sBIN, sBnsCode, sTaxYear, sBnsStat, dCapital, dGross);
                }
            }
            pSet.Close();
        }

        private void UpdateBillGrossInfo(string sBIN, string sBnsCode, string sTaxYear, string sBnsStat, double dCapital, double dGross)
        {
            // RMC 20150120
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            double dGrCap;
            int iIsReset = 0;
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "update_billgross_info";
            plsqlCmd.ParamValue = sBIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sBnsCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sTaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = "1";
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sBnsStat;
            plsqlCmd.AddParameter("p_sStatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = sBnsStat.Substring(0,1);
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);

            plsqlCmd.ParamValue = dCapital;
            plsqlCmd.AddParameter("p_fCapital", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);

            plsqlCmd.ParamValue = dGross;
            plsqlCmd.AddParameter("p_fGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);

            plsqlCmd.ParamValue = 0;
            plsqlCmd.AddParameter("p_fPreGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = 0;
            plsqlCmd.AddParameter("p_fAdjGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);

            plsqlCmd.ParamValue = 0;
            plsqlCmd.AddParameter("p_fVatGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = iIsReset;
            plsqlCmd.AddParameter("o_iIsReset", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Output);

            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
                plsqlCmd.Rollback();
            else
                int.TryParse(plsqlCmd.ReturnValue("o_iIsReset").ToString(), out iIsReset);
            plsqlCmd.Close();

        }

        private void SaveEcc()
        {
            // RMC 20171128 added tagging of ECC 
            OracleResultSet pCmd = new OracleResultSet();

            if (CheckEcc() && !pageCheck.chkECC.Checked)
            {
                pCmd.Query = "insert into ecc_tagging_hist ";
                pCmd.Query += " select * from ecc_tagging where bin = '" + pageCheck.bin1.GetBin() + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }


                pCmd.Query = "delete from ecc_tagging where bin = '" + pageCheck.bin1.GetBin() + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }

            if (!CheckEcc() && pageCheck.chkECC.Checked)
            {
                pCmd.Query = "insert into ecc_tagging values (:1,:2,:3)";
                pCmd.AddParameter(":1", pageCheck.bin1.GetBin());
                pCmd.AddParameter(":2", AppSettingsManager.SystemUser.UserCode);
                pCmd.AddParameter(":3", AppSettingsManager.GetCurrentDate());
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }
            
        }

        private bool CheckEcc()
        {
            // RMC 20171128 added tagging of ECC 
            OracleResultSet pRec = new OracleResultSet();
            int iCnt = 0;

            pRec.Query = "select count(*) from ecc_tagging where bin = '" + pageCheck.bin1.GetBin() + "'";
            int.TryParse(pRec.ExecuteScalar(), out iCnt);

            if (iCnt > 0)
                return true;
            else
                return false;
        }
        
    }
}